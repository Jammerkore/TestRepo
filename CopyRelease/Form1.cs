using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Threading;
using System.Reflection;

using MIDRetail.Common;
using MIDRetail.DataCommon;

namespace MIDRetail.CopyRelease
{
    public partial class Form1 : Form
    {
        string _SQLServerPath;
        string _DatabasePath;
        string _DatabasePathROExtract;
        string _LicenseKeyGeneratorPath;
        string _ReportsPath;
        string _XSDPath;
        string _UtilitiesPath;
        string _MIDRetailInfoPath;
        string _InstallerPath;
        string _InstallFilesPath;
        string _ConfigFilePath;

        string _folderRoot;
        string _buildFolder;
        string _documentationFolder;
        string _32bit = "32bit";
        string _64bit = "64bit";
        string _env = "{env}";
        bool _folderNameChanged;
        ClientInfo _clientInfo;
        object _currentObject = null;
        bool _64bitSupportNeeded = true;
        string _calcFileName;
        string _SCMRepository = null;
        string _SCMBranch = null;
        string _version = null;
        string _releasePath;
        string _repository;
        char _backSlash = '\\';
        string _zipFileName = null;
        string _installerCmd = null;

        string _signCmd = null;
        string _signLocation = null;
        string _32BitZip = null;
        string _32BitZipOn64Bit = null;
        string _64BitZip = null;
        string _piso = null;
        string _suffix = null; 

        //// Begin TT#TT#846-MD - JSmith - New Stored Procedures for Performance
        //private const string SQL_FOLDER_STORED_PROCEDURES = "SQL_StoredProcedures";
        //private const string SQL_FOLDER_TYPES = "SQL_Types";
        //private const string SQL_FOLDER_SCALAR_FUNCTIONS = "SQL_Functions_Scalar";
        //private const string SQL_FOLDER_TABLE_FUNCTIONS = "SQL_Functions_Table";
        //private const string SQL_FOLDER_TABLES = "SQL_Tables";
        //private const string SQL_FOLDER_TABLE_KEYS = "SQL_TableKeys";
        //private const string SQL_FOLDER_CONSTRAINTS = "SQL_Constraints";
        //private const string SQL_FOLDER_INDEXES = "SQL_Indexes";
        //private const string SQL_FOLDER_VIEWS = "SQL_Views";
        //private const string SQL_FOLDER_TRIGGERS = "SQL_Triggers";
        //// End TT#TT#846-MD - JSmith - New Stored Procedures for Performance
        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnQABuildPath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;
            string buildNumber;
            int index;

            try
            {
                fbdFilePath.SelectedPath = txtQABuildPath.Text;
                fbdFilePath.Description = "Select the directory where the release was compiled.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtQABuildPath.Text = fbdFilePath.SelectedPath;
                    if (radCreateRelease.Checked)
                    {
                        index = txtQABuildPath.Text.LastIndexOf('\\');
                        buildNumber = txtQABuildPath.Text.Substring(index + 1);
                        buildNumber = buildNumber.Replace(_folderRoot + " ", "");
                        index = buildNumber.IndexOf(" ");
                        if (index > 0)
                        {
                            buildNumber = buildNumber.Substring(0, index);
                        }
                        txtQAFolderName.Text = _folderRoot + "_" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + " (" + buildNumber + ") Client";
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void btnQAReleasePath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;

            try
            {
                fbdFilePath.SelectedPath = txtQAReleasePath.Text;
                fbdFilePath.Description = "Select the directory where the release is to be put.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtQAReleasePath.Text = fbdFilePath.SelectedPath;
                    if (!_folderNameChanged &&
                        (radQA.Checked || radDevelopment.Checked))
                    {
                        txtQAFolderName.Text = GetFolderName(true);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void btnQADocumentationPath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;

            try
            {
                fbdFilePath.SelectedPath = txtQADocumentationBranch.Text;
                fbdFilePath.Description = "Select the directory where the documentation is found.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtQADocumentationBranch.Text = fbdFilePath.SelectedPath;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TextReader r = null;
            XmlSerializer s;
            string fileLocation;
            Clients clients;
            ArrayList clientList;
            ClientInfo clientInfo;
            ClientInfo startingClientInfo = null;

            try
            {
                Application.EnableVisualStyles();

                _buildFolder = System.Configuration.ConfigurationManager.AppSettings["BuildFolder"];
                _documentationFolder = System.Configuration.ConfigurationManager.AppSettings["DocumentationFolder"];
                fileLocation = System.Configuration.ConfigurationManager.AppSettings["ClientFileLocation"];
                txtQABuildPath.Text = _buildFolder;

                txtQADocumentationBranch.Text = _documentationFolder;
                _folderRoot = System.Configuration.ConfigurationManager.AppSettings["ReleaseFolderRoot"];
                if (_folderRoot == null || _folderRoot.Trim().Length == 0)
                {
                    _folderRoot = "Logility-RO";
                }
                _repository = System.Configuration.ConfigurationManager.AppSettings["Repository"];
                _installerCmd = System.Configuration.ConfigurationManager.AppSettings["InstallerCommandFile"];
                _signCmd = System.Configuration.ConfigurationManager.AppSettings["SignCommandFile"];
                _signLocation = System.Configuration.ConfigurationManager.AppSettings["SignLocation"];
                
                // Begin TT#342-MD - JSmith - CopyRelease fails to zip file in 64 bit environment
                _32BitZip = System.Configuration.ConfigurationManager.AppSettings["32BitZip"];
                _32BitZipOn64Bit = System.Configuration.ConfigurationManager.AppSettings["32BitZipOn64Bit"];
                _64BitZip = System.Configuration.ConfigurationManager.AppSettings["64BitZip"];
                // End TT#342-MD - JSmith - CopyRelease fails to zip file in 64 bit environment
                // Begin TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
                _piso = System.Configuration.ConfigurationManager.AppSettings["piso"];
                // End TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
                // Begin TT#346-MD - JSmith - Modify CopyRelease to allow the starting client to be configured
                string startingClient = System.Configuration.ConfigurationManager.AppSettings["StartingClient"];
                if (startingClient == null)
                {
                    startingClient = "Base 5.0";
                }
                // End TT#346-MD - JSmith - Modify CopyRelease to allow the starting client to be configured

                _folderNameChanged = false;
                txtQAFolderName.Text = GetFolderName(false);
                radQA.Checked = true;

                s = new XmlSerializer(typeof(Clients)); // Create a Serializer
                r = new StreamReader(fileLocation);					  // Load the Xml File
                clients = (Clients)s.Deserialize(r);				  // Deserialize the Xml File to a strongly typed object

                clientList = new ArrayList();
                foreach (ClientsClient client in clients.Client)
                {
                    clientInfo = new ClientInfo(client);
                    // Begin TT#346-MD - JSmith - Modify CopyRelease to allow the starting client to be configured
                    //if (clientInfo.Name == "Base 4.0")
                    if (clientInfo.Name == startingClient)
                    // End TT#346-MD - JSmith - Modify CopyRelease to allow the starting client to be configured
                    {
                        startingClientInfo = clientInfo;
                    }
                    clientList.Add(clientInfo);
                }
                cboClient.DataSource = clientList;
                cboClient.DisplayMember = "Name";
                cboClient.ValueMember = "Name";

                cboClient.SelectedItem = startingClientInfo;

                pnlRelease.Location = pnlQABuild.Location;
                pnlRelease.Size = pnlQABuild.Size;
                pnlFTP.Location = pnlQABuild.Location;
                pnlFTP.Size = pnlQABuild.Size;

                // field documentation
                cboClient.Tag = "Select the client or base for which the build was performed";
                radQA.Tag = "Select this option if the build is intended for QA";
                radCreateRelease.Tag = "Select this option if the QA build is being scheduled for release";
                radPackageToFTP.Tag = "Select this option if the release is to be packaged for the client";

                cbxQACalcOnly.Tag = "Check this if calc build only";
                txtQABuildPath.Tag = "Select the location where the Visual Studios release build was performed";
                btnQABuildPath.Tag = "Select the location where the Visual Studios release build was performed";
                txtQAReleasePath.Tag = "Select the location where the QA build is to be put";
                btnQAReleasePath.Tag = "Select the location where the QA build is to be put";
                txtQACalcFile.Tag = "Select the file with the client calc definitions";
                btnQACalcFile.Tag = "Select the file with the client calc definitions";
                lblQADocumentationBranch.Tag = "Identify the SCM branch where the documentation is located";
                txtQADocumentationBranch.Tag = "Identify the SCM branch where the documentation is located";
                lblQAFolderName.Tag = "Identify the name of the folder to be used for the QA build";
                txtQAFolderName.Tag = "Identify the name of the folder to be used for the QA build";

                txtRLQAPath.Tag = "Select the location where the QA build to be release is found";
                btnRLQAPath.Tag = "Select the location where the QA build to be release is found";
                txtRLReleasePath.Tag = "Select the location where the release is to be put";
                btnRLReleasePath.Tag = "Select the location where the release is to be put";

                txtFTPReleasePath.Tag = "Select the location where the release to be sent to the client if found";
                btnFTPReleasePath.Tag = "Select the location where the release to be sent to the client if found";
                txtFTPPath.Tag = "Select the location where the client will retrive the release from the FTP site";
                btnFTPPath.Tag = "Select the location where the client will retrive the release from the FTP site";

                //GetSCMInfo();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetSCMInfo()
        {
            StreamReader reader = null;
            string line = null;
            int index;
            try
            {

                reader = new StreamReader(_buildFolder + "\\.MySCMServerInfo");
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("SCMBranch="))
                    {
                        index = line.LastIndexOf("=");
                        _SCMBranch = line.Substring(index + 1);
                        lblQASMBBranchName.Text = _SCMBranch;
                    }
                    else if (line.Contains("SCMRepository="))
                    {
                        index = line.LastIndexOf("=");
                        _SCMRepository = line.Substring(index + 1);
                        lblQASCMRepositoryName.Text = _SCMRepository;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                lblQASMBBranchName.Text = "Unknown";
                lblQASCMRepositoryName.Text = "Unknown";
            }
            catch
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                if (radQA.Checked || radDevelopment.Checked)
                {
                    if (txtQABuildPath.Text == null || txtQABuildPath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Compile Path is required");
                        return;
                    }
                    if (txtQAReleasePath.Text == null || txtQAReleasePath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Release Path is required");
                        return;
                    }
                    if (_clientInfo.CalcLocation != null && _clientInfo.CalcLocation.Trim().Length > 0)
                    {
                        if (txtQACalcFile.Text.Length == 0)
                        {
                            MessageBox.Show("Calc file is required");
                            return;
                        }
                        else if (!(txtQACalcFile.Text.EndsWith(".xls") || txtQACalcFile.Text.EndsWith(".xlsx")))
                        {
                            MessageBox.Show("Valid calc file not selected");
                            return;
                        }
                    }
                    if (txtQADocumentationBranch.Text == null || txtQADocumentationBranch.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("SCM documentation branch is required");
                        return;
                    }
                    if (txtQAFolderName.Text == null || txtQAFolderName.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Folder name is required");
                        return;
                    }
                    
                    CopyToQA();
                }
                else if (radCreateRelease.Checked)
                {
                    if (txtRLQAPath.Text == null || txtRLQAPath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("QA Path is required");
                        return;
                    }
                    if (txtRLReleasePath.Text == null || txtRLReleasePath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Release Path is required");
                        return;
                    }
                    if (txtRLFolderName.Text == null || txtRLFolderName.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Folder name is required");
                        return;
                    }
                    if (txtRLFolderName.Text.Contains(" Client"))
                    {
                        MessageBox.Show("Please replace folder name with correct client name");
                        return;
                    }
                    CopyToRelease();
                }
                else 
                {
                    if (txtFTPReleasePath.Text == null || txtFTPReleasePath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Release Path is required");
                        return;
                    }
                    if (txtFTPPath.Text == null || txtFTPPath.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("FTP Path is required");
                        return;
                    }
                    if (txtFTPZipFileName.Text == null || txtFTPZipFileName.Text.Trim().Length == 0)
                    {
                        MessageBox.Show("Zip file name is required");
                        return;
                    }
                    CopyToFTP();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CopyToQA()
        {
            try
            {
                UpdateStatus("Creating archives");
                BuildArchives();

                UpdateStatus("Creating folders");
                BuildQAReleaseFolders();

                UpdateStatus("Copying folders");
                CopyFolders();

                UpdateStatus("Copying individual files");
                CopyIndividualFiles();
                // Begin TT#881 - MD - stodd - Modify Copy Release to generate license key for QA release also
                UpdateStatus("Generating License Key");
                GenerateLicenseKey(true);
                // End TT#881 - MD - stodd - Modify Copy Release to generate license key for QA release also

                //UpdateStatus("Renaming SCM Branch");
                //RenameSCMBranch();

                UpdateStatus("Done");
                btnClose.Focus();
                MessageBox.Show("Release has been copied");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CopyToRelease()
        {
            try
            {
                //Begin TT#1973 - JSmith - Add custom attribute to contain client to use when creating releases
                //System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(@"C:\scmvs2017\Working 4.0 Fixes\ApplicationClient\bin\Release\MIDRetail.exe");
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(txtRLQAPath.Text + @"\Application\Installer\Install Files\Client\MIDRetail.exe");
                if (assembly == null)
                {
                    MessageBox.Show("Could not locate client executable");
                    return;
                }
                object[] customAttributes = assembly.GetCustomAttributes(typeof(AssemblyConfigurationAttribute), true);
      

                if (customAttributes.Length == 0)
                {
                    MessageBox.Show("Release version not specified in AssemblyInfo.cs of the client.  Release cannot be shipped.");
                    return;
                }

                string configuration = string.IsNullOrEmpty(((AssemblyConfigurationAttribute)customAttributes[0]).Configuration) ? "Base" : ((AssemblyConfigurationAttribute)customAttributes[0]).Configuration;
                if (_clientInfo.ReleaseType != "Base" &&
                    _clientInfo.ReleaseType != configuration)
                {
                    MessageBox.Show("Release does not match version for client.  Release cannot be shipped.");
                    return;
                }

                System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(txtRLQAPath.Text + @"\Application\Installer\Install Files\Client\MIDRetail.exe");
                if (!fvi.FileVersion.StartsWith(_clientInfo.ShipVersion))
                {
                    MessageBox.Show("Version of release does not match shipping version for client.  Release cannot be shipped.");
                    return;
                }
                //End TT#1973 

                UpdateStatus("Creating folders");
                BuildReleaseFolders();

                //CopyFiles(txtQADocumentationBranch.Text, _MIDRetailInfoPath);
                //CopyFolder(txtQABuildPath.Text + "\\SQLServer", _SQLServerPath);
                UpdateStatus("Copying release");
                CopyFolder(txtRLQAPath.Text, _releasePath);

                // Begin TT#448-MD - JSmith - Add different license keys by product for Installer
                UpdateStatus("Generating License Key");
                GenerateLicenseKey(false);
                // End TT#448-MD - JSmith - Add different license keys by product for Installer

                //Begin TT#1973 - JSmith - Add custom attribute to contain client to use when creating releases
                UpdateStatus("Signing modules");
                SignModules();
                //End TT#1973

                //System.IO.Directory.Delete(_releasePath + @"\temp", true);

                UpdateStatus("Done");
                MessageBox.Show("Release has been copied");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // Begin TT#448-MD - JSmith - Add different license keys by product for Installer
        private void GenerateLicenseKey(bool forQA)
        {
            StreamWriter writer = null;
            int allocationExpiration = 0;
            int planningExpiration = 0;
            int sizeExpiration = 0;
            int assortmentExpiration = 0;
            int groupAllocationExpiration = 0;	// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
            int masterExpiration = 0;
            string FileName = string.Empty; // TT#881 - MD - stodd - keyFile for QA

            try
            {
                // TT#881 - MD - stodd - keyFile for QA
                if (forQA)
                {
                    FileName = _DatabasePath + @"\KeyFile.txt";
                }
                else
                {
                    FileName = _releasePath + @"\Application\Database\KeyFile.txt";
                }
                // TT#881 - MD - stodd - keyFile for QA
                System.IO.File.SetAttributes(FileName, System.IO.File.GetAttributes(FileName) & ~(FileAttributes.ReadOnly));
                writer = new StreamWriter(FileName, false);
                AppConfig appConfig = new AppConfig();
                string key = appConfig.BuildLicenseKey(_clientInfo.AllocationInstalled, allocationExpiration,
                     _clientInfo.SizeInstalled, sizeExpiration,
                     _clientInfo.PlanningInstalled, planningExpiration,
                     _clientInfo.AssortmentInstalled, assortmentExpiration,
                     _clientInfo.GroupAllocationInstalled, groupAllocationExpiration,	// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                     _clientInfo.MasterInstalled, masterExpiration,
                     _clientInfo.AnalyticsInstalled, 0);    // TT#2131-MD - JSmith - Halo Integration

                writer.WriteLine(key);
            }
            catch (Exception ex)
            {
                MessageBox.Show("License key generate failed - " + ex.ToString());
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
                UpdateStatus("License key generated successfully");
            }
        }
        // End TT#448-MD - JSmith - Add different license keys by product for Installer

        private void CopyToFTP()
        {
            string ftpFileName;
            int index;
            // Begin TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
            //UpdateStatus("Zipping the release");
            //if (ZipRelease())
            //{
            //    index = _zipFileName.LastIndexOf(_backSlash);
            //    ftpFileName = txtFTPPath.Text + "\\" + _zipFileName.Substring(index + 1);
            //    UpdateStatus("Copying the release to the ftp site");
            //    CopyFile(_zipFileName, ftpFileName);

            //    MessageBox.Show("Release has been copied to the ftp site");
            //}
            if (_clientInfo.ShipAsISO)
            {
                UpdateStatus("Creating ISO/daa file of the release");
                if (PowerISORelease())
                {
                    index = _zipFileName.LastIndexOf(_backSlash);
                    ftpFileName = txtFTPPath.Text + "\\" + _zipFileName.Substring(index + 1);
                    UpdateStatus("Copying the release to the ftp site");
                    CopyFile(_zipFileName, ftpFileName);

                    MessageBox.Show("Release has been copied to the ftp site");
                }
            }
            else
            {
                UpdateStatus("Zipping the release");
                if (ZipRelease())
                {
                    index = _zipFileName.LastIndexOf(_backSlash);
                    ftpFileName = txtFTPPath.Text + "\\" + _zipFileName.Substring(index + 1);
                    UpdateStatus("Copying the release to the ftp site");
                    CopyFile(_zipFileName, ftpFileName);

                    MessageBox.Show("Release has been copied to the ftp site");
                }
            }
            // End TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
            UpdateStatus("Done");
        }

        private void BuildArchives()
        {
            Process archives = new Process();
            archives.StartInfo.FileName = _installerCmd;
            archives.Start();
            archives.WaitForExit();
        }

        private void SignModules()
        {

            UpdateConfigFile();

            Process archives = new Process();
            archives.StartInfo.FileName = _signCmd;
            archives.Start();
            archives.WaitForExit();

            File.Delete(_InstallerPath + @"\midretailinccert.pfx");
        }

        private void UpdateConfigFile()
        {
            string configFile = @"C:\scmvs2017\build\CopyRelease\files_sign.config";

            //get the xml document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(configFile);

            XmlNode appSettingsNode;
            appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");

            foreach (XmlNode childNode in appSettingsNode)
            {
                if (childNode.GetType() != typeof(System.Xml.XmlComment) &&
                    childNode.GetType() != typeof(System.Xml.XmlText) &&
                    childNode.Attributes != null)
                {
                    if (childNode.Attributes["key"].Value == "batchpath" ||
                        childNode.Attributes["key"].Value == "buildpath")
                    {
                        //Begin TT#1973 - JSmith - Add custom attribute to contain client to use when creating releases
                        //childNode.Attributes["value"].Value = txtQAReleasePath.Text + @"\" + txtQAFolderName.Text;
                        childNode.Attributes["value"].Value =  txtRLReleasePath.Text + @"\" + txtRLFolderName.Text;
                        //Begin TT#1973
                    }
                }
            }

            File.SetAttributes(configFile, File.GetAttributes(configFile) & ~(FileAttributes.ReadOnly));
            xmlDoc.Save(configFile);
        }

        private void BuildQAReleaseFolders()
        {
            string releasePath;
            try
            {
               releasePath = txtQAReleasePath.Text + "\\" + this.txtQAFolderName.Text;

                _MIDRetailInfoPath = releasePath + "\\" + "Information";
                _SQLServerPath = releasePath + "\\" + "Application";
                _DatabasePath = _SQLServerPath + "\\" + "Database";
                _DatabasePathROExtract = _SQLServerPath + "\\" + "DatabaseROExtract";

                _LicenseKeyGeneratorPath = _SQLServerPath + "\\" + "LicenseKeyGenerator";
                _ReportsPath = _SQLServerPath + "\\" + "Reports";
                _XSDPath = _SQLServerPath + "\\" + "XSDs";
                _UtilitiesPath = _SQLServerPath + "\\" + "Utilities";

                _InstallerPath = _SQLServerPath + "\\" + "Installer";
                _InstallFilesPath = _InstallerPath + "\\" + "Install Files";
               
                _ConfigFilePath = _InstallFilesPath + "\\" + "ConfigFile";

                if (!Directory.Exists(releasePath))
                {
                    Directory.CreateDirectory(releasePath);
                    Directory.CreateDirectory(_MIDRetailInfoPath);
                    Directory.CreateDirectory(_DatabasePath);
                    // Begin TT#TT#846-MD - JSmith - New Stored Procedures for Performance
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_CONSTRAINTS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_GENERATED_NONTABLE_FILES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_GENERATED_TABLE_FILES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_INDEXES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_SCRIPTS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_TABLE_KEYS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_TABLES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_TRIGGERS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_TYPES);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_VIEWS);
                    Directory.CreateDirectory(_DatabasePath + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS);
                    // End TT#TT#846-MD - JSmith - New Stored Procedures for Performance
                    Directory.CreateDirectory(_DatabasePathROExtract);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_CONSTRAINTS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_GENERATED_NONTABLE_FILES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_GENERATED_TABLE_FILES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_INDEXES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_SCRIPTS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLE_KEYS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TRIGGERS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TYPES);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_VIEWS);
                    Directory.CreateDirectory(_DatabasePathROExtract + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS);

                    if (!cbxQACalcOnly.Checked)
                    {
                        Directory.CreateDirectory(_SQLServerPath);
                        Directory.CreateDirectory(_InstallerPath);
                        Directory.CreateDirectory(_InstallFilesPath);
                        Directory.CreateDirectory(_ConfigFilePath);
                        Directory.CreateDirectory(_XSDPath);
                        Directory.CreateDirectory(_UtilitiesPath);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void BuildReleaseFolders()
        {
            try
            {
                _releasePath = txtRLReleasePath.Text + "\\" + this.txtRLFolderName.Text;

                if (!Directory.Exists(_releasePath))
                {
                    Directory.CreateDirectory(_releasePath);
                }
            }
            catch
            {
                throw;
            }
        }

        private void CopyFolders()
        {
            string buildPath;
            try
            {
                buildPath = txtQABuildPath.Text;
                CopyFiles(buildPath + "\\SQLServerDatabaseUpdate\\bin\\Release", _DatabasePath);
                // Begin TT#TT#846-MD - JSmith - New Stored Procedures for Performance
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_CONSTRAINTS, _DatabasePath + "\\" + Include.SQL_FOLDER_CONSTRAINTS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS, _DatabasePath + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS, _DatabasePath + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS, "*.SQL");
                //do not copy files in the generated folders, they will be generated
                //CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_GENERATED_NONTABLE_FILES, _DatabasePath + "\\" + Include.SQL_FOLDER_GENERATED_NONTABLE_FILES, "*.SQL");
                //CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_GENERATED_TABLE_FILES, _DatabasePath + "\\" + Include.SQL_FOLDER_GENERATED_TABLE_FILES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_INDEXES, _DatabasePath + "\\" + Include.SQL_FOLDER_INDEXES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_SCRIPTS, _DatabasePath + "\\" + Include.SQL_FOLDER_SCRIPTS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES, _DatabasePath + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_TABLE_KEYS, _DatabasePath + "\\" + Include.SQL_FOLDER_TABLE_KEYS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_TABLES, _DatabasePath + "\\" + Include.SQL_FOLDER_TABLES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_TRIGGERS, _DatabasePath + "\\" + Include.SQL_FOLDER_TRIGGERS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_TYPES, _DatabasePath + "\\" + Include.SQL_FOLDER_TYPES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_VIEWS, _DatabasePath + "\\" + Include.SQL_FOLDER_VIEWS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinition" + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS, _DatabasePath + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS, "*.SQL");

                CopyFiles(buildPath + "\\SQLServerDatabaseUpdate\\bin\\Release", _DatabasePathROExtract);
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_CONSTRAINTS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_CONSTRAINTS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_INDEXES, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_INDEXES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_SCRIPTS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_SCRIPTS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_STORED_PROCEDURES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_TABLE_KEYS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLE_KEYS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_TABLES, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TABLES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_TRIGGERS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TRIGGERS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_TYPES, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_TYPES, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_VIEWS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_VIEWS, "*.SQL");
                CopyFiles(buildPath + "\\DatabaseDefinitionROExtract" + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS, _DatabasePathROExtract + "\\" + Include.SQL_FOLDER_UPGRADE_VERSIONS, "*.SQL");

                if (!cbxQACalcOnly.Checked)
                {
                    CopyFiles(buildPath + "\\MIDAdvInstaller\\bin\\Release", _InstallerPath, new string[] { "Infragistics", "Microsoft.VisualStudio", "Microsoft.MSXML" });
                    CopyFiles(buildPath + "\\MIDRegExtract\\bin\\Release", _InstallerPath);
                    CopyFolder("C:\\Temp\\Installer", _InstallFilesPath);

                    CopyFiles(buildPath + "\\Utilities", _UtilitiesPath);
					CopyFiles(buildPath + "\\StoreBinViewer\\Bin\\Release", _UtilitiesPath + "\\StoreBinViewer", "*.EXE");
					CopyFiles(buildPath + "\\StoreBinViewer\\Bin\\Release", _UtilitiesPath + "\\StoreBinViewer", "*.DLL");
					CopyFiles(buildPath + "\\StoreBinViewer\\Bin\\Release", _UtilitiesPath + "\\StoreBinViewer", "*.CONFIG");

                    CopyFiles(buildPath + "\\CreateEventSource\\Bin\\Release", _UtilitiesPath + "\\CreateEventSource", "*.EXE");
                    CopyFiles(buildPath + "\\CreateEventSource\\Bin\\Release", _UtilitiesPath + "\\CreateEventSource", "*.DLL");
                    CopyFiles(buildPath + "\\CreateEventSource\\Bin\\Release", _UtilitiesPath + "\\CreateEventSource", "*.CONFIG");

                    CopyFiles(buildPath + "\\Reports", _ReportsPath);
                    CopyFiles(buildPath + "\\DataCommon\\Schema", _XSDPath, "*.XSD");
                    CopyFiles(buildPath + "\\DataCommon\\Schema", _XSDPath, "*.CS");
                }
            }
            catch
            {
                throw;
            }
        }

        public void CopyFolder(string sourceFolder, string destFolder)
        {
            string name;
            string dest;

            if (!Directory.Exists(destFolder))
            {
                Directory.CreateDirectory(destFolder);
            }
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                name = Path.GetFileName(file);
                dest = Path.Combine(destFolder, name);
                if (!name.Contains("MySCMServerInfo"))
                {
                    File.Copy(file, dest);
                }
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                name = Path.GetFileName(folder);
                dest = Path.Combine(destFolder, name);
                files = Directory.GetFiles(sourceFolder + "\\" + name);
                if (!name.Contains("LicenseKeyGenerator"))
                {
                    CopyFolder(folder, dest);
                }
            }
        }

        private void CopyIndividualFiles()
        {
            string buildPath;
            try
            {
                buildPath = txtQABuildPath.Text;

                // Begin TT#TT#846-MD - JSmith - New Stored Procedures for Performance
                //CopyFile(buildPath + "\\SQLServerDatabaseUpdate\\Upgrade.sql", _DatabasePath + "\\Upgrade.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\MRS Text Load.sql", _DatabasePath + "\\MRS Text Load.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\SP_MID_NEEDS_QUERY.sql", _DatabasePath + "\\SP_MID_NEEDS_QUERY.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Default Computation Driver Models.sql", _DatabasePath + "\\Default Computation Driver Models.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Default Forecast Models.sql", _DatabasePath + "\\Default Forecast Models.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Default Chain Forecast Models.sql", _DatabasePath + "\\Default Chain Forecast Models.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Default Computation Driver Models.sql", _DatabasePath + "\\Default Computation Driver Models.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\KeyFile.txt", _DatabasePath + "\\KeyFile.txt");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Rebuild Primary Keys.sql", _DatabasePath + "\\Rebuild Primary Keys.sql");
                CopyFile(buildPath + "\\DatabaseDefinition\\Rebuild Store Indexes.sql", _DatabasePath + "\\Rebuild Store Indexes.sql");  // TT#3599 - JSmith - Issue with Purge in MID Test
                //CopyFile(buildPath + "\\DatabaseDefinition\\MRS SQL Server After Database Copy.sql", _DatabasePath + "\\MRS SQL Server After Database Copy.sql");
     
                //CopyFile(buildPath + "\\DatabaseDefinition\\MRS SQL Server Database Schema.SQL", _DatabasePath + "\\MRS SQL Server Database Schema.SQL");
          
                //CopyFile(buildPath + "\\DatabaseDefinition\\MRS SQL Server Initial Load.sql", _DatabasePath + "\\MRS SQL Server Initial Load.sql");
    
                //CopyFile(buildPath + "\\DatabaseDefinition\\Remove Orphaned Size Curves.sql", _DatabasePath + "\\Remove Orphaned Size Curves.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Set FILLFACTOR.sql", _DatabasePath + "\\Set FILLFACTOR.sql");
                
        
                //CopyFile(buildPath + "\\DatabaseDefinition\\create MID_alloc_bulk.sql", _DatabasePath + "\\create MID_alloc_bulk.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\create MID_alloc_pack.sql", _DatabasePath + "\\create MID_alloc_pack.sql");
                //CopyFile(buildPath + "\\DatabaseDefinition\\Rebuild Daily Size History Indexes.sql", _DatabasePath + "\\Rebuild Daily Size History Indexes.sql");

                CopyFile(buildPath + "\\DatabaseDefinition\\SequenceSchema.xsd", _DatabasePath + "\\SequenceSchema.xsd");
                CopyFile(buildPath + "\\DatabaseDefinition\\SequenceForDeleting.xml", _DatabasePath + "\\SequenceForDeleting.xml");
                CopyFile(buildPath + "\\DatabaseDefinition\\SequenceForNewDB.xml", _DatabasePath + "\\SequenceForNewDB.xml");
                CopyFile(buildPath + "\\DatabaseDefinition\\SequenceForUpgradeDB.xml", _DatabasePath + "\\SequenceForUpgradeDB.xml");

                CopyFile(buildPath + "\\DatabaseDefinitionROExtract\\SequenceSchema.xsd", _DatabasePathROExtract + "\\SequenceSchema.xsd");
                CopyFile(buildPath + "\\DatabaseDefinitionROExtract\\SequenceForDeleting.xml", _DatabasePathROExtract + "\\SequenceForDeleting.xml");
                CopyFile(buildPath + "\\DatabaseDefinitionROExtract\\SequenceForNewDB.xml", _DatabasePathROExtract + "\\SequenceForNewDB.xml");
                CopyFile(buildPath + "\\DatabaseDefinitionROExtract\\SequenceForUpgradeDB.xml", _DatabasePathROExtract + "\\SequenceForUpgradeDB.xml");

                CopyFile(buildPath + "\\DataCommon\\MIDSettings.config", _ConfigFilePath + "\\MIDSettings.config");
                CopyFile(buildPath + "\\DatabaseDefinition\\MarkStoresForDeletion.SQL", _UtilitiesPath + "\\MarkStoresForDeletion.SQL");
                CopyFile(buildPath + "\\DatabaseDefinition\\ReadMe.txt", _MIDRetailInfoPath + "\\ReadMe.txt");
                // End TT#TT#846-MD - JSmith - New Stored Procedures for Performance
                
                if (Directory.Exists(_UtilitiesPath))
                {
                    CopyFile(buildPath + "\\DatabaseDefinition\\GradeSumMacro.xls", _UtilitiesPath + "\\GradeSumMacro.xls");
                }

                if (txtQACalcFile.Text != null && txtQACalcFile.Text.Length > 0)
                {
                    CopyFile(txtQACalcFile.Text, this._MIDRetailInfoPath + "\\" + _calcFileName);
                }
                CopyFile(buildPath + "\\DatabaseDefinition\\AddDefaultSchedules.SQL", _UtilitiesPath + "\\AddDefaultSchedules.SQL");  // TT#930 - MD - JSmith - Initial Install - Create recommended/default System Task Lists/Jobs with a SQL Script


                //CopyFile("\\\\Midretail14\\MIDRETAIL\\Development\\Crystal Reports VS2013 - V13_0_12\\CRRuntime_64bit_13_0_12.msi", _UtilitiesPath + "\\CRRuntime_64bit_13_0_12.msi");
                //CopyFile("\\\\Midretail14\\MIDRETAIL\\Development\\Crystal Reports VS2013 - V13_0_12\\CRRuntime_32bit_13_0_12.msi", _UtilitiesPath + "\\CRRuntime_32bit_13_0_12.msi");
				//CopyFile("\\\\Midretail14\\MIDRETAIL\\Development\\Crystal Reports VS2017 - V13_0_21\\CRRuntime_64bit_13_0_21.msi", _UtilitiesPath + "\\CRRuntime_64bit_13_0_21.msi");
    //            CopyFile("\\\\Midretail14\\MIDRETAIL\\Development\\Crystal Reports VS2017 - V13_0_21\\CRRuntime_32bit_13_0_21.msi", _UtilitiesPath + "\\CRRuntime_32bit_13_0_21.msi");
                
                CopyFiles(buildPath + "\\MIDAdvInstaller\\InstallUtil.exe", _InstallerPath);
            }
            catch
            {
                throw;
            }
        }

        private void CopyFile(string aSourceFile, string aDestinationFile)
        {
            FileAttributes fileAttributes;
            try
            {
                if (File.Exists(aSourceFile))
                {
                    if (File.Exists(aDestinationFile))
                    {
                        fileAttributes = System.IO.File.GetAttributes(aDestinationFile);
                        System.IO.File.SetAttributes(aDestinationFile, fileAttributes & ~FileAttributes.ReadOnly);
                    }

                    System.IO.File.Copy(aSourceFile, aDestinationFile, true);
                }
            }
            catch
            {
                throw;
            }
        }

        private void CopyFiles(string aSourceFolder, string aDestinationFolder, string[] strExcludeList = null)
        {
            CopyFiles(aSourceFolder, aDestinationFolder, "*", strExcludeList);
        }

        private void CopyFiles(string aSourceFolder, string aDestinationFolder, string aMask, string[] strExcludeList = null)
        {
            FileInfo[] files;
            DirectoryInfo directoryInfo;
            FileAttributes fileAttributes;
            string toFileName;

            try
            {
                if (Directory.Exists(aSourceFolder))
                {
                    directoryInfo = new DirectoryInfo(aSourceFolder);
                    files = directoryInfo.GetFiles(aMask);
                    foreach (FileInfo file in files)
                    {
                        bool blFoundExclusion = false;
                        if (file.Extension.Equals(".pdb", StringComparison.CurrentCultureIgnoreCase) || 
                            file.FullName.Contains("MySCMServerInfo") ||
                            file.FullName.Contains("CrystalDecisions") ||
                            file.Extension.Equals(".csproj", StringComparison.CurrentCultureIgnoreCase)
                            )
                        {
                            blFoundExclusion = true;
                        }

                        if (strExcludeList != null)
                        {
                            
                            foreach (string strExclude in strExcludeList)
                            {
                                if (file.FullName.Contains(strExclude))
                                {
                                    blFoundExclusion = true;
                                    break;
                                }
                            }
                        }

                        if (blFoundExclusion)
                        {
                            continue;
                        }

                        if (!Directory.Exists(aDestinationFolder))
                        {
                            Directory.CreateDirectory(aDestinationFolder);
                        }

                        toFileName = file.FullName.Replace(aSourceFolder, aDestinationFolder);
                        if (File.Exists(toFileName))
                        {
                            fileAttributes = System.IO.File.GetAttributes(toFileName);
                            System.IO.File.SetAttributes(toFileName, fileAttributes & ~FileAttributes.ReadOnly);
                        }

                        System.IO.File.Copy(file.FullName, toFileName, true);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void GetDocumentation()
        {
            if (Directory.Exists("C:\\TEMP\\SCMDocs"))
            {
                DeleteFolder("C:\\TEMP\\SCMDocs");
            }
            Process process = new Process();
            if (File.Exists("c:\\Program Files\\Perforce\\Surround SCM\\sscm.exe"))
            {
                process.StartInfo.FileName = "c:\\Program Files\\Perforce\\Surround SCM\\sscm";
            }
            else if (File.Exists("C:\\Program Files (x86)\\Perforce\\Surround SCM\\sscm.exe"))
            {
                process.StartInfo.FileName = "C:\\Program Files (x86)\\Perforce\\Surround SCM\\sscm";
            }
            else
            {
                MessageBox.Show("Unable to locate SCM.  Documentation will not be included.");
                return;
            }
            process.StartInfo.Arguments = @"get /TechnicalGuides -dC:\TEMP\SCMDocs -b""" + txtQADocumentationBranch.Text + @""" -r -f -q -tmodify -p""" + _clientInfo.DocumentationMainline + @" "" -yjsmith:john""";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            if (process.ExitCode > 0)
            {
                MessageBox.Show("An error was returned from getting the documentation.  Please check the results.");
                return;
            }

            if (File.Exists(@"C:\TEMP\SCMDocs\TechnicalGuides\MIDArchitecture.pdf"))
            {
                DeleteFile(@"C:\TEMP\SCMDocs\TechnicalGuides\MIDArchitecture.pdf");
            }

            CopyFiles("C:\\TEMP\\SCMDocs\\TechnicalGuides", _MIDRetailInfoPath, "*.pdf");
        }

        private void RenameSCMBranch()
        {
            string newName = null;

            try
            {
                if (_SCMBranch == null)
                {
                    MessageBox.Show("SCM Branch could not be determined and must be renamed manually");
                    return;
                }

                newName = _SCMBranch.Trim() + " " + _version;

                Process process = new Process();
                //if (IntPtr.Size == 8)
                //{
                //    process.StartInfo.FileName = "c:\\Program Files (x86)\\Perforce\\Surround SCM\\sscm";
                //}
                //else
                //{
                    process.StartInfo.FileName = "c:\\Program Files\\Perforce\\Surround SCM\\sscm";
                //}
                process.StartInfo.Arguments = @"renamebranch """ + _SCMBranch + @"""" + @" """ + newName + @""" -p""" + _SCMRepository + @""" -cc""Rename""";
                //rename "oldname" "newName" -p"repository"
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode > 0)
                {
                    MessageBox.Show("An error was returned from the branch rename.  Please check the results.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ZipRelease()
        {
            string tempFolderName = null;
            string tempFolderNameRoot = null;
            int index;
            string zipExecutable;

            try
            {
                zipExecutable = _32BitZip;
                if (!File.Exists(zipExecutable))
                {
                    zipExecutable = _32BitZipOn64Bit;
                    if (!File.Exists(zipExecutable))
                    {
                        zipExecutable = _64BitZip;
                        if (!File.Exists(zipExecutable))
                        {
                            MessageBox.Show("Win Zip not found.  Release cannot be zipped.");
                            return false;
                        }
                    }
                }

                // generate unique name
                index = txtFTPReleasePath.Text.LastIndexOf("\\");
                tempFolderNameRoot = _repository + @"\zip" + DateTime.Now.Ticks.ToString();
                tempFolderName = tempFolderNameRoot + "\\" + txtFTPReleasePath.Text.Substring(index + 1);
                if (Directory.Exists(tempFolderName))
                {
                    DeleteFolder(tempFolderName);
                }
                Directory.CreateDirectory(tempFolderName);
                CopyFolder(txtFTPReleasePath.Text, tempFolderName);


                _zipFileName = txtFTPReleasePath.Text + ".zip";

                Process process = new Process();
                process.StartInfo.FileName = zipExecutable;
                process.StartInfo.Arguments = @" -min -a -r """ + _zipFileName + @"""" + @" """ + tempFolderNameRoot + _backSlash + @"*.*""";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode > 0)
                {
                    MessageBox.Show("An error was returned zipping the release.  Please check the results.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tempFolderName != null &&
                    Directory.Exists(tempFolderNameRoot))
                {
                    DeleteFolder(tempFolderNameRoot);
                }
            }
        }

        // Begin TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
        private bool PowerISORelease()
        {
            try
            {
                if (!File.Exists(_piso))
                {
                    MessageBox.Show("Power ISO not found.  Release cannot be zipped.");
                    return false;
                }

                _zipFileName =  Directory.GetParent(txtFTPReleasePath.Text).ToString().Trim() + _backSlash + txtFTPZipFileName.Text;

                Process process = new Process();
                process.StartInfo.FileName = _piso;
                process.StartInfo.Arguments = @" create -o """ + _zipFileName + @"""" + @" -add """ + txtFTPReleasePath.Text + @"""" + @" /";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                if (process.ExitCode > 0)
                {
                    MessageBox.Show("An error was returned creating ISO for the release.  Please check the results.");
                    return false;
                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // End TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped

        private void DeleteFolder(string aFolderName)
        {
            // delete files in folder
            string[] names = Directory.GetFiles(aFolderName);
            foreach (string file in names)
            {
                DeleteFile(file);
            }

            // recurse to sub folders
            string[] dirs = Directory.GetDirectories(aFolderName);
            foreach (string sub in dirs)
            {
                DeleteFolder(sub);
            }

            // delete folder
            Directory.Delete(aFolderName);
        }

        private void DeleteFile(string aFileName)
        {
            if ((File.GetAttributes(aFileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(aFileName, FileAttributes.Normal);
            }

            File.Delete(aFileName);
        }

        private string GetFolderName(bool aThrowError)
        {
            string folderName;
            FileVersionInfo fvi;
            string moduleName;

            if (cbxQACalcOnly.Checked)
            {
                moduleName = txtQABuildPath.Text + "\\ForecastComputations\\bin\\Release\\" + System.Configuration.ConfigurationManager.AppSettings["ComputationsDll"];
            }
            else
            {
                moduleName = txtQABuildPath.Text + "\\Windows\\bin\\Release\\" + System.Configuration.ConfigurationManager.AppSettings["WindowsDll"];
            }
            try
            {
                folderName = _folderRoot.Trim();
                fvi = FileVersionInfo.GetVersionInfo(moduleName);
                if (fvi.FileVersion.StartsWith("2"))
                {
                    _64bitSupportNeeded = false;
                }
                else
                {
                    _64bitSupportNeeded = true;
                }
                _version = fvi.FileVersion;
                return folderName + " " + fvi.FileVersion;
            }
            catch
            {
                if (aThrowError)
                {
                    MessageBox.Show("Unable to retrieve information for " + moduleName
                        + ". \n  Please check your configuration information. \n  The application will be terminated",
                        "Configuration Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    throw;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        private void radDevelopment_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus("Ready");
            pnlQABuild.Visible = true;
            pnlQABuild.BringToFront();
            pnlRelease.Visible = false;
            pnlFTP.Visible = false;
            EnableLicenseKeys(true);	// TT#1247-MD - add Group Allocation license key - 
            if (_clientInfo != null)
            {
                if (radDevelopment.Checked)
                {
                    txtQAReleasePath.Text = _clientInfo.DevelopmentBuildLocation;
                }
                else
                {
                    txtQAReleasePath.Text = _clientInfo.QABuildLocation;
                }
            }
        }

        private void radQA_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus("Ready");
            pnlQABuild.Visible = true;
            pnlQABuild.BringToFront();
            pnlRelease.Visible = false;
            pnlFTP.Visible = false;
            EnableLicenseKeys(true);	// TT#1247-MD - add Group Allocation license key - 
            if (_clientInfo != null)
            {
                if (radDevelopment.Checked)
                {
                    txtQAReleasePath.Text = _clientInfo.DevelopmentBuildLocation;
                }
                else
                {
                    txtQAReleasePath.Text = _clientInfo.QABuildLocation;
                }
            }
        }

        private void radCreateRelease_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus("Ready");
            pnlQABuild.Visible = false;
            pnlRelease.Visible = true;
            pnlRelease.BringToFront();
            pnlFTP.Visible = false;
            EnableLicenseKeys(true);	// TT#1247-MD - add Group Allocation license key - 
        }

        private void radPackageToFTP_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStatus("Ready");
            pnlQABuild.Visible = false;
            pnlRelease.Visible = false;
            pnlFTP.Visible = true;
            pnlFTP.BringToFront();
            EnableLicenseKeys(false);	// TT#1247-MD - add Group Allocation license key - 
        }

		// Begin TT#1247-MD - add Group Allocation license key - 
        private void EnableLicenseKeys(bool enable)
        {
            gbKeys.Enabled = enable;
            cbxAllocation.Enabled = enable;
            cbxAssortment.Enabled = enable;
            cbxGroupAllocation.Enabled = enable;
            cbxMaster.Enabled = enable;
            cbxPlanning.Enabled = enable;
            cbxSize.Enabled = enable;
            cbxAnalytics.Enabled = Enabled;  // TT#2131-MD - JSmith - Halo Integration
        }
		// End TT#1247-MD - add Group Allocation license key - 

        private void cboClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboClient.SelectedItem != null)
            {
                _clientInfo = (ClientInfo)cboClient.SelectedItem;

                SetCLientKeys(_clientInfo);		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -

                txtQABuildPath.Text = _buildFolder;
                if (radDevelopment.Checked)
                {
                    txtQAReleasePath.Text = _clientInfo.DevelopmentBuildLocation;
                }
                else 
                { 
                    txtQAReleasePath.Text = _clientInfo.QABuildLocation; 
                }

                txtQADocumentationBranch.Text = _clientInfo.DocumentationBranch;
                if (_clientInfo.CalcLocation == null ||
                    _clientInfo.CalcLocation.Trim().Length == 0)
                {
                    txtQACalcFile.Visible = false;
                    btnQACalcFile.Visible = false;
                }
                else
                {
                    txtQACalcFile.Text = _clientInfo.CalcLocation;
                    txtQACalcFile.Visible = true;
                    btnQACalcFile.Visible = true;
                }

                txtRLQAPath.Text = _clientInfo.QABuildLocation;
                txtRLReleasePath.Text = _clientInfo.ReleaseLocation;

                txtFTPReleasePath.Text = _clientInfo.ReleaseLocation;
                txtFTPPath.Text = _clientInfo.FTPLocation;

                lblProdVersionNumber.Text = _clientInfo.ProdVersion;
            }
        }

		// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        private void SetCLientKeys(ClientInfo aClient)
        {
            cbxAllocation.Checked = aClient.AllocationInstalled;
            //if (cbxAllocation.Checked)
            //{
            //    cbxAllocation.CheckState = CheckState.Indeterminate;
            //}
            cbxPlanning.Checked = aClient.PlanningInstalled;
            cbxSize.Checked = aClient.SizeInstalled;
            cbxMaster.Checked = aClient.MasterInstalled;
            cbxAssortment.Checked = aClient.AssortmentInstalled;
            cbxGroupAllocation.Checked = aClient.GroupAllocationInstalled;
            cbxAnalytics.Checked = aClient.AnalyticsInstalled;  // TT#2131-MD - JSmith - Halo Integration
        }
		// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

        private void btnQACalcFile_Click(object sender, EventArgs e)
        {
            ofdFileName.InitialDirectory = _clientInfo.CalcLocation;
            if (ofdFileName.ShowDialog() != DialogResult.Cancel)
            {
                txtQACalcFile.Text = ofdFileName.FileName;
                _calcFileName = ofdFileName.SafeFileName;
            }
        }

        private void object_MouseHover(object sender, EventArgs e)
        {
            if (((Control)sender).Tag != null)
            {
                toolTip1.Active = true;
                toolTip1.SetToolTip((Control)sender, (string)((Control)sender).Tag);
            }
            else
            {
                toolTip1.Active = false;
            }
        }

        private void object_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender != _currentObject)
            {
                if (((Control)sender).Tag != null)
                {
                    toolTip1.Active = true;
                    toolTip1.SetToolTip((Control)sender, (string)((Control)sender).Tag);
                }
                else
                {
                    toolTip1.Active = false;
                }
                _currentObject = sender;
            }
        }

        private void cbxQACalcOnly_CheckedChanged(object sender, EventArgs e)
        {
            txtQAFolderName.Text = GetFolderName(true);
        }

        private void btnRLQAPath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;
            string buildNumber;
            int index;

            try
            {
                fbdFilePath.SelectedPath = txtRLQAPath.Text;
                fbdFilePath.Description = "Select the directory to the QA release.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtRLQAPath.Text = fbdFilePath.SelectedPath;
                    index = txtRLQAPath.Text.LastIndexOf('\\');
                    buildNumber = txtRLQAPath.Text.Substring(index + 1);
                    buildNumber = buildNumber.Replace(_folderRoot + " ", "");
                    index = buildNumber.IndexOf(" ");
                    if (index > 0)
                    {
                        buildNumber = buildNumber.Substring(0, index);
                    }
                    txtRLFolderName.Text = _folderRoot + "_" + DateTime.Now.Year + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00") + " (" + buildNumber + ") " + _clientInfo.Name;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void btnRLReleasePath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;
            
            try
            {
                fbdFilePath.SelectedPath = txtRLReleasePath.Text;
                fbdFilePath.Description = "Select the directory where the release is to be placed.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtRLReleasePath.Text = fbdFilePath.SelectedPath;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void btnFTPReleasePath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;
            int index;

            try
            {
                fbdFilePath.SelectedPath = txtFTPReleasePath.Text;
                fbdFilePath.Description = "Select the directory where the release is to be found.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtFTPReleasePath.Text = fbdFilePath.SelectedPath;
                    index = txtFTPReleasePath.Text.LastIndexOf("\\");
                    // Begin TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
                    //txtFTPZipFileName.Text = txtFTPReleasePath.Text.Substring(index + 1) + ".zip";
                    if (_clientInfo.ShipAsISO)
                    {
                            SetISO();
                            //txtFTPZipFileName.Text = txtFTPReleasePath.Text.Substring(index + 1) + ".daa";
                            txtFTPZipFileName.Text = txtFTPReleasePath.Text.Substring(index + 1) + _suffix;
                    }
                    else
                    {
                        txtFTPZipFileName.Text = txtFTPReleasePath.Text.Substring(index + 1) + ".zip";
                    }
                    // End TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }


        private void SetISO()
        {
            if (_clientInfo.Notdaa)
            {
                _suffix = ".iso";
            }
            else
            {
                _suffix = ".daa";
            }
        }
		
        private void btnFTPPath_Click(object sender, EventArgs e)
        {
            DialogResult retCode;

            try
            {
                fbdFilePath.SelectedPath = txtFTPPath.Text;
                fbdFilePath.Description = "Select the ftp directory where the zipped release is to be placed.";

                retCode = fbdFilePath.ShowDialog();

                if (retCode == DialogResult.OK)
                {
                    txtFTPPath.Text = fbdFilePath.SelectedPath;
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void UpdateStatus(string aText)
        {
            toolStripStatusLabel.Text = aText;
            Application.DoEvents();
        }

		// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        private void cbxAllocation_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.AllocationInstalled = cbxAllocation.Checked;
        }

        private void cbxPlanning_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.PlanningInstalled = cbxPlanning.Checked;
        }

        private void cbxSize_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.SizeInstalled = cbxSize.Checked;
        }

        private void cbxMaster_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.MasterInstalled = cbxMaster.Checked;
        }

        private void cbxAssortment_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.AssortmentInstalled = cbxAssortment.Checked;
        }

        private void cbxGroupAllocation_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.GroupAllocationInstalled = cbxGroupAllocation.Checked;
        }
		// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

        // Begin TT#2131-MD - JSmith - Halo Integration
        private void cbxAnalytics_CheckedChanged(object sender, EventArgs e)
        {
            _clientInfo.AnalyticsInstalled = cbxAnalytics.Checked;
        }
        // Begin TT#2131-MD - JSmith - Halo Integration

    }

    public class ClientInfo
    {
        // Fields

        ClientsClient _client;

        /// <summary>
        /// Used to construct an instance of the class.
        /// </summary>
        public ClientInfo(ClientsClient aClient)
        {
            _client = aClient;
        }

        public string Name
        {
            get { return _client.Name; }
        }

        public string ProdVersion
        {
            get { return _client.ProdVersion; }
        }

        public string ShipVersion
        {
            get { return _client.ShipVersion; }
        }

        public string CalcLocation
        {
            get { return _client.CalcLocation; }
        }

        public string DevelopmentBuildLocation
        {
            get { return _client.DevelopmentBuildLocation; }
        }

        public string QABuildLocation
        {
            get { return _client.QABuildLocation; }
        }

        public string ReleaseLocation
        {
            get { return _client.ReleaseLocation; }
        }

        public string DocumentationMainline
        {
            get { return _client.DocumentationMainline; }
        }

        public string DocumentationBranch
        {
            get { return _client.DocumentationBranch; }
        }

        public string FTPLocation
        {
            get { return _client.FTPLocation; }
        }

        //Begin TT#1973 - JSmith - Add custom attribute to contain client to use when creating releases
        public string ReleaseType
        {
            get { return _client.ReleaseType; }
        }
        //End TT#1973

        public bool CustomReports
        {
            get { return _client.CustomReports; }
        }

        public override string ToString()
        {
            return Name;
        }

        // Begin TT#448-MD - JSmith - Add different license keys by product for Installer
        public bool AllocationInstalled
        {
            get { return _client.AllocationInstalled; }
            set { _client.AllocationInstalled = value; }	// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        }

        public bool PlanningInstalled
        {
            get { return _client.PlanningInstalled; }
            set { _client.PlanningInstalled = value; }		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        }

        public bool SizeInstalled
        {
            get { return _client.SizeInstalled; }
            set { _client.SizeInstalled =  value; }		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        }

        public bool AssortmentInstalled
        {
            get { return _client.AssortmentInstalled; }
            set { _client.AssortmentInstalled = value; }		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        }
	
		// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        public bool GroupAllocationInstalled
        {
            get { return _client.GroupAllocationInstalled; }
            set { _client.GroupAllocationInstalled = value; }
        }
		// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

        public bool MasterInstalled
        {
            get { return _client.MasterInstalled; }
            set { _client.MasterInstalled = value; }		// TT#1247-MD - stodd - Add Group Allocation as a License Key option -
        }
        // End TT#448-MD - JSmith - Add different license keys by product for Installer

        // Begin TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped
        public bool ShipAsISO
        {
            get { return _client.ShipAsISO; }
        }
        // End TT#451-MD - JSmith - Deliver releases as iso or daa file instead of zipped

        // Begin TT#2131-MD - JSmith - Halo Integration
        public bool AnalyticsInstalled
        {
            get { return _client.AnalyticsInstalled; }
            set { _client.AnalyticsInstalled = value; }
        }
        // End TT#2131-MD - JSmith - Halo Integration
		
        public bool Notdaa
        {
            get { return _client.Notdaa; }
        }

    }

}
