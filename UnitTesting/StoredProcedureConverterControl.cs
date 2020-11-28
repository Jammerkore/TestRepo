using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MIDRetail.Windows;
using MIDRetail.DataCommon;


namespace UnitTesting
{
    public partial class StoredProcedureConverterControl : UserControl
    {
        public StoredProcedureConverterControl()
        {
            InitializeComponent();
        }
        //private string sCurrentProjectDir;

        private Boolean addCanceled = false;

        private void TestManagerControl_Load(object sender, EventArgs e)
        {
            //sCurrentProjectDir = System.AppDomain.CurrentDomain.BaseDirectory;
           // sCurrentProjectDir = sCurrentProjectDir.Substring(0, sCurrentProjectDir.IndexOf("UnitTesting"));
            this.txtProject.Text = Shared_UtilityFunctions.GetCurrentProjectPath();
            DataView dv = Shared_BaseStoredProcedures.GetModuleListAsDataset().Tables[0].AsDataView();
            dv.Sort = "moduleName";

            this.cboModule.DataSource = dv;
            this.cboModule.Text = Shared_BaseStoredProcedures.defaultModule;
          
            this.cboProcedureType.SetDataBinding(GetProcedureTypesDataSet(), "ProcedureTypes");
            this.cboProcedureType.Text = "Read";

            this.ultraToolbarsManager1.Tools["btnConvertAll"].SharedProps.Visible = false;
        }
       
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnValidate":
                    ValidateStoredProcedure();
                    break;
                case "btnAddProcedure":
                    AddProcedure();
                    break;
                case "btnRevalidateClass":
                    RevalidateClass();
                    break;
                case "btnAddClassToCode":
                    AddClassToCode();
                    break;
                case "btnFromReadTemplate":
                    ShellToReadTemplate();
                    break;
                case "btnFromInsertTemplate":
                    ShellToInsertTemplate();
                    break;
                case "btnFromUpdateTemplate":
                    ShellToUpdateTemplate();
                    break;
                case "btnFromDeleteTemplate":
                    ShellToDeleteTemplate();
                    break;
                //case "btnConvertAll":
                //    ConvertAllFiles();
                //    break;

            }
        }

        private void ShellToReadTemplate()
        {
            string sPath = UnitTests.globalTemplateFilePath + "Read Template.sql";

            if (System.IO.File.Exists(sPath) == true)
            {
                System.Diagnostics.Process.Start(sPath);
            }
        }
        private void ShellToInsertTemplate()
        {
            string sPath = UnitTests.globalTemplateFilePath + "Insert Template.sql";

            if (System.IO.File.Exists(sPath) == true)
            {
                System.Diagnostics.Process.Start(sPath);
            }
        }
        private void ShellToUpdateTemplate()
        {
            string sPath = UnitTests.globalTemplateFilePath + "Update Template.sql";

            if (System.IO.File.Exists(sPath) == true)
            {
                System.Diagnostics.Process.Start(sPath);
            }
        }
        private void ShellToDeleteTemplate()
        {
            string sPath = UnitTests.globalTemplateFilePath + "Delete Template.sql";

            if (System.IO.File.Exists(sPath) == true)
            {
                System.Diagnostics.Process.Start(sPath);
            }
        }
        private void txtWrapper_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "a")
                {
                    this.txtWrapper.SelectAll();
                    e.Handled = true;
                }
            }
        }
        private void txtStoredProc_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "a")
                {
                    this.txtStoredProc.SelectAll();
                    e.Handled = true;
                }
            }
        }
        private void txtCall_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                if (e.KeyCode.ToString() == "A" || e.KeyCode.ToString() == "a")
                {
                    this.txtCall.SelectAll();
                    e.Handled = true;
                }
            }
        }
        //private void ConvertAllFiles()
        //{
        //    string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_TableKeys\\";
        //    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Functions_Table\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Indexes\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_TableKeys\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Tables\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Triggers\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Types\\";
        ////    ConvertPath(sPath);
        ////    sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_Views\\";
        ////    ConvertPath(sPath);
        //}
        //private void ConvertPath(string sPath)
        //{
        //    DirectoryInfo di = new DirectoryInfo(sPath);
        //    foreach (FileInfo fi in di.GetFiles("*.SQL"))
        //    {
        //        string fileName = fi.Name;
        //        StreamReader sr = fi.OpenText();
        //        string sNewFile = sr.ReadToEnd();
        //        //while (sr.EndOfStream == false)
        //        //{
        //        //    string line = sr.ReadLine();
        //        //    if (line.Trim().ToUpper() != "GO")
        //        //    {
        //        //        sNewFile += line + System.Environment.NewLine;
        //        //    }
        //        //}
        //        sr.Close();

        //        fi.Delete();

        //        sNewFile += System.Environment.NewLine + "GO" + System.Environment.NewLine;
        //        StreamWriter sw = new StreamWriter(sPath + fileName);
        //        sw.Write(sNewFile);
        //        sw.Flush();
        //        sw.Close();
        //    }
        //}
        //private void ConvertAll()
        //{
        //    bool inOtherObject = false;
        //    string sSchemaFile = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\MRS SQL Server Database Schema.SQL";

        //    System.IO.StreamReader sr = new System.IO.StreamReader(sSchemaFile);
        //    string sSchema = sr.ReadToEnd();
        //    sr.Close();

        //    string[] sSplit = sSchema.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //    int iCounter = 0;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE TYPE")))
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION")))
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    inOtherObject = false;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION") || s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            inOtherObject = true;
        //        }
        //        else if (s.Trim().ToUpper() == "GO")
        //        {
        //            inOtherObject = false;
        //        }

        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && s.Contains("#") == false && (s.ToUpper().Contains("CREATE TABLE")) && !inOtherObject)
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //            inOtherObject = false;
        //        }
        //        iCounter++;
        //    }

        //    inOtherObject = false;
        //    iCounter = 0;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION") || s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            inOtherObject = true;
        //        }
        //        else if (s.Trim().ToUpper() == "GO")
        //        {
        //            inOtherObject = false;
        //        }
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("ALTER TABLE")) && !inOtherObject)
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //            inOtherObject = false;
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    inOtherObject = false;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION") || s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            inOtherObject = true;
        //        }
        //        else if (s.Trim().ToUpper() == "GO")
        //        {
        //            inOtherObject = false;
        //        }
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE INDEX") || s.ToUpper().Contains("CREATE UNIQUE INDEX")) && !inOtherObject)
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //            inOtherObject = false;
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    inOtherObject = false;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION") || s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            inOtherObject = true;
        //        }
        //        else if (s.Trim().ToUpper() == "GO")
        //        {
        //            inOtherObject = false;
        //        }
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE VIEW")) && !inOtherObject)
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //            inOtherObject = false;
        //        }
        //        iCounter++;
        //    }

        //    iCounter = 0;
        //    inOtherObject = false;
        //    foreach (string s in sSplit)
        //    {
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE FUNCTION") || s.ToUpper().Contains("CREATE PROCEDURE")))
        //        {
        //            inOtherObject = true;
        //        }
        //        else if (s.Trim().ToUpper() == "GO")
        //        {
        //            inOtherObject = false;
        //        }
        //        if (s.StartsWith("/*") == false && s.StartsWith("--") == false && s.StartsWith("set") == false && (s.ToUpper().Contains("CREATE TRIGGER")) && !inOtherObject)
        //        {
        //            string comments = GetComments(iCounter, sSplit);
        //            string item = GetSQLItem(iCounter, sSplit);
        //            item = comments + item;
        //            string[] itemSplit = item.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
        //            string itemName = Shared_UtilityFunctions.DetermineSQLItemName(itemSplit);
        //            string strErr;
        //            AddSQLItemToProject(item, itemName, out strErr);
        //            if (strErr != string.Empty)
        //            {
        //                MessageBox.Show(item + ":" + strErr);
        //            }
        //            inOtherObject = false;
        //        }
        //        iCounter++;
        //    }
        //}
        private string GetComments(int i, string[] sSplit)
        {
            string comments = string.Empty;

            bool isComment = true;
            int iLine = i - 1;
            while (isComment == true)
            {
                if (sSplit[iLine].StartsWith("--") == true || sSplit[iLine].StartsWith("/*") == true)
                {
                    comments = sSplit[iLine] + System.Environment.NewLine + comments;
                }
                else
                {
                    isComment = false;
                }
                iLine--;
            }
                


            return comments;
        }
        private string GetSQLItem(int i, string[] sSplit)
        {
            string item = string.Empty;
            bool keepGoing = true;
            int iLine = i;

            // see if need to back up
            bool backUp = false;
            int iresetLine = iLine;
            int iPrevLine = i - 1;
            while (true)
            {
                // ignore known conditions
                if (iPrevLine < 0 ||
                    //sSplit[iPrevLine].Trim().Length == 0 ||
                    sSplit[iPrevLine].Trim().ToUpper() == "GO" ||
                    sSplit[iPrevLine].Trim().ToUpper().StartsWith("IF EXISTS") == true ||
                    sSplit[iPrevLine].Trim().ToUpper().StartsWith("IF NOT EXISTS*") == true ||
                    sSplit[iPrevLine].Trim().StartsWith("--") == true ||
                    sSplit[iPrevLine].Trim().StartsWith("/*") == true ||
                    sSplit[iPrevLine].Trim().EndsWith("*/") == true)
                {
                    break;
                }
                else if (sSplit[iPrevLine].Trim().Length > 0)
                {
                    backUp = true;
                    iresetLine = iPrevLine;
                }

                iPrevLine--;
            }
            if (backUp)
            {
                iLine = iresetLine;
            }

            while (keepGoing == true)
            {
                item += sSplit[iLine] + System.Environment.NewLine;
                if (sSplit[iLine].Trim().ToUpper() == "GO")
                {
                    keepGoing = false;
                } 
                iLine++;
            }
            return item;
        }
        private enum eSQL_Folders
        {
            StoredProcedures,
            Types,
            ScalarFunctions,
            TableFunctions,
            Tables,
            Constraints,
            Indexes,
            Views,
            Triggers
        }
       
        private void ValidateStoredProcedure()
        {
            this.ultraToolbarsManager1.Tools["btnValidate"].SharedProps.Enabled = false;
            txtName.Text = string.Empty;
            txtWrapper.Text = string.Empty;
            txtCall.Text = string.Empty;

            string[] sSplit = this.txtStoredProc.Text.Split(new string[] {System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            txtName.Text = Shared_UtilityFunctions.DetermineSQLItemName(sSplit);

            string sProcedureName = this.txtName.Text;
            string sProcedure = this.txtStoredProc.Text;
            string sTableName = GetTableNameFromProcedureName(sProcedureName);
            this.txtTableName.Text = sTableName;
            bool unknownType;
            MIDRetail.Data.storedProcedureTypes procedureType = GetProcedureTypeFromProcedure(sProcedure, sProcedureName, out unknownType);
            if (unknownType == false)
            {
                this.cboProcedureType.Text = procedureType.ToString();
                this.txtWrapper.Text = MakeWrapperClass(sProcedure, sProcedureName, procedureType, sTableName);
                this.txtCall.Text = MakeCallingCode(sProcedure, sProcedureName, procedureType, sTableName);
            }
            this.ultraToolbarsManager1.Tools["btnValidate"].SharedProps.Enabled = true;
        }

        private void RevalidateClass()
        {
            string sProcedureName = this.txtName.Text;
            string sProcedure = this.txtStoredProc.Text;
            MIDRetail.Data.storedProcedureTypes procedureType = GetProcedureTypeFromString(this.cboProcedureType.Text);
            string sTableName = this.txtTableName.Text;
            this.txtWrapper.Text = MakeWrapperClass(sProcedure, sProcedureName, procedureType, sTableName);
            this.txtCall.Text = MakeCallingCode(sProcedure, sProcedureName, procedureType, sTableName);
        }
        private MIDRetail.Data.storedProcedureTypes GetProcedureTypeFromString(string sProcedureType)
        {
            switch (sProcedureType)
            {
                case "Read":
                    return MIDRetail.Data.storedProcedureTypes.Read;
                case "ReadAsDataset":
                    return MIDRetail.Data.storedProcedureTypes.ReadAsDataset;
                case "Insert":
                    return MIDRetail.Data.storedProcedureTypes.Insert;
                case "InsertAndReturnRID":
                    return MIDRetail.Data.storedProcedureTypes.InsertAndReturnRID;
                case "Update":
                    return MIDRetail.Data.storedProcedureTypes.Update;
                case "UpdateWithReturnCode":
                    return MIDRetail.Data.storedProcedureTypes.UpdateWithReturnCode;
                case "Delete":
                    return MIDRetail.Data.storedProcedureTypes.Delete;
                case "RecordCount":
                    return MIDRetail.Data.storedProcedureTypes.RecordCount;
                case "ScalarValue":
                    return MIDRetail.Data.storedProcedureTypes.ScalarValue;
                case "Maintenance":
                    return MIDRetail.Data.storedProcedureTypes.Maintenance;
                case "OutputOnly":
                    return MIDRetail.Data.storedProcedureTypes.OutputOnly;
                default:
                    return MIDRetail.Data.storedProcedureTypes.Read;
            }
        }

        private eSQL_Folders DetermineSQLFolder(string[] sSplit)
        {
            bool foundAdd = false;
            foreach (string s in sSplit)
            {
                if (s.ToUpper().Contains("CREATE PROCEDURE"))
                {
                    return eSQL_Folders.StoredProcedures;
                }
                if (s.ToUpper().Contains("CREATE TYPE"))
                {
                    return eSQL_Folders.Types;
                }
                if (s.ToUpper().Contains("CREATE FUNCTION"))
                {
                    if (isTableFunction(sSplit) == true)
                    {
                        return eSQL_Folders.TableFunctions;
                    }
                    else
                    {
                        return eSQL_Folders.ScalarFunctions;
                    }
                }
                if (s.ToUpper().Contains("CREATE TABLE"))
                {
                    return eSQL_Folders.Tables;
                }
                if (s.ToUpper().Contains("ADD"))
                {
                    foundAdd = true;
                }
                if (foundAdd && s.ToUpper().Contains("CONSTRAINT"))
                {
                    return eSQL_Folders.Constraints;
                }
                if (s.ToUpper().Contains("CHECK") && s.ToUpper().Contains("CONSTRAINT"))
                {
                    return eSQL_Folders.Constraints;
                }
                if (s.ToUpper().Contains("CREATE INDEX") || s.ToUpper().Contains("CREATE UNIQUE INDEX"))
                {
                    return eSQL_Folders.Indexes;
                }
                if (s.ToUpper().Contains("CREATE VIEW"))
                {
                    return eSQL_Folders.Views;
                }
                if (s.ToUpper().Contains("CREATE TRIGGER"))
                {
                    return eSQL_Folders.Triggers;
                }
            }
            return eSQL_Folders.StoredProcedures;
        }
        private bool isTableFunction(string[] sSplit)
        {
            bool isTableFunc = false;
            foreach (string s in sSplit)
            {

                if (s.ToUpper().Contains("RETURNS TABLE"))
                    isTableFunc = true;
          
            }
            return isTableFunc;
        }
       
        private void AddProcedure()
        {
            this.ultraToolbarsManager1.Tools["btnAddProcedure"].SharedProps.Enabled = false;

            string strErr;
            AddSQLItemToProject(txtStoredProc.Text, txtName.Text, out strErr);


            this.ultraToolbarsManager1.Tools["btnAddProcedure"].SharedProps.Enabled = true;
            if (strErr != string.Empty)
            {
                MessageBox.Show(strErr);
            }
            else if (addCanceled)
            {
                addCanceled = false;
            }
            else
            {
                MessageBox.Show("Stored Procedure added to Project.");
            }
        }
        private void AddClassToCode()
        {
            if (txtWrapper.Text == string.Empty)
            {
                MessageBox.Show("Please generate a wrapper class first.");
                return;
            }
            if (cboModule.Text == Shared_BaseStoredProcedures.defaultModule)
            {
                MessageBox.Show("Please first select a module.");
                return;
            }

            string sModuleFile = Shared_BaseStoredProcedures.GetFileForModule(cboModule.Text);
            if (sModuleFile == string.Empty)
            {
                MessageBox.Show("No project file found for this module.");
                return;
            }

            StreamReader sr = new StreamReader(sModuleFile);
            string fileContents = sr.ReadToEnd();
            sr.Close();


            string sNewContent = txtWrapper.Text;
            string sSearchFor = "//INSERT NEW STORED PROCEDURES ABOVE HERE";

            int firstInstance = fileContents.IndexOf(sSearchFor);
            if (firstInstance != -1)
            {
                string[] sSplit = sNewContent.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.None);
                string aleadyInFileSearchString = sSplit[0];
                int alreadyInFile = fileContents.IndexOf(aleadyInFileSearchString);
                if (alreadyInFile == -1)
                {
                    string htab3 = "\t\t\t";
                    fileContents = fileContents.Substring(0, firstInstance) + sNewContent + System.Environment.NewLine + htab3 + sSearchFor + fileContents.Substring(firstInstance + sSearchFor.Length);

                    EnsureFileIsNotReadOnly(sModuleFile);
                    StreamWriter sw2 = new StreamWriter(sModuleFile, false);
                    sw2.Write(fileContents);
                    sw2.Flush();
                    sw2.Close();

                    MessageBox.Show("Wrapper class has been added.");
                }
                else
                {
                    MessageBox.Show("Wrapper class already found in this module.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Stored Procedures not setup for this module.");
            }
        }
        private void AddSQLItemToProject(string sItem, string itemName, out string errorDescription)
        {
            errorDescription = string.Empty;
            string sProjectFile = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\DatabaseDefinition.csproj";

            string[] sItemSplit = sItem.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            eSQL_Folders sqlFolder = DetermineSQLFolder(sItemSplit);
            string sSQLFolder = string.Empty;
            if (sqlFolder == eSQL_Folders.StoredProcedures)
            {
                sSQLFolder = Include.SQL_FOLDER_STORED_PROCEDURES;
            }
            if (sqlFolder == eSQL_Folders.Types)
            {
                sSQLFolder = Include.SQL_FOLDER_TYPES;
            }
            if (sqlFolder == eSQL_Folders.ScalarFunctions)
            {
                sSQLFolder = Include.SQL_FOLDER_SCALAR_FUNCTIONS;
            }
            if (sqlFolder == eSQL_Folders.TableFunctions)
            {
                sSQLFolder = Include.SQL_FOLDER_TABLE_FUNCTIONS;
            }
            if (sqlFolder == eSQL_Folders.Tables)
            {
                sSQLFolder = Include.SQL_FOLDER_TABLES;
            }
            if (sqlFolder == eSQL_Folders.Constraints)
            {
                sSQLFolder = Include.SQL_FOLDER_CONSTRAINTS;
            }
            if (sqlFolder == eSQL_Folders.Indexes)
            {
                sSQLFolder = Include.SQL_FOLDER_INDEXES;
            }
            if (sqlFolder == eSQL_Folders.Views)
            {
                sSQLFolder = Include.SQL_FOLDER_VIEWS;
            }
            if (sqlFolder == eSQL_Folders.Triggers)
            {
                sSQLFolder = Include.SQL_FOLDER_TRIGGERS;
            }
             if (sSQLFolder == string.Empty)
            {
                errorDescription = "Unknown SQL item.";
                return;
            }
            // EnsureFolderIsNotReadOnly(sCurrentProjectDir + "DatabaseDefinition\\" + sSQLFolder + "\\");
             //EnsureFolderIsNotReadOnly(sCurrentProjectDir + "DatabaseDefinition\\" + sSQLFolder );

             string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + sSQLFolder + "\\" + itemName + ".SQL";
             try
             {
                 if (File.Exists(sPath) == true)
                 {
                     string message = "Stored Procedure already exists in the project, do you wish to override?";
                     if (MessageBox.Show(message, " ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                             == DialogResult.Yes)
                     {
                         EnsureFileIsNotReadOnly(sPath);
                     }
                     else
                     {
                         addCanceled = true;
                         return;
                     }
                     
                 }
                 StreamWriter sw = new StreamWriter(sPath);
                 sw.Write(sItem);
                 sw.Flush();
                 sw.Close();
             }
             catch (Exception ex)
             {
                 errorDescription = ex.ToString();
             }


             //Microsoft.Build.BuildEngine.Engine msEngine = new Microsoft.Build.BuildEngine.Engine();
             //Microsoft.Build.BuildEngine.Project project = new Microsoft.Build.BuildEngine.Project(msEngine);

             //project.Load(sProjectFile);
             //project.AddNewItem("\\SQL_StoredProcedures\\" + txtName.Text, "Content");
             ////project.AddNewImport(sPath, "Content");

             //project.Save(sProjectFile);

             //StreamReader sr = new StreamReader(sProjectFile);
             //string projectFileContents = sr.ReadToEnd();
             //sr.Close();


             //string sNewContent = "<Content Include=\"" + sSQLFolder + "\\" + itemName + ".SQL" + "\" />";

             //if (projectFileContents.Contains(sNewContent) == false)
             //{

             //    int firstItemGroup = projectFileContents.IndexOf("<ItemGroup>");

             //    projectFileContents = projectFileContents.Substring(0, firstItemGroup) + "<ItemGroup>" + System.Environment.NewLine + "    " + sNewContent + projectFileContents.Substring(firstItemGroup + 11);


             //    EnsureFileIsNotReadOnly(sProjectFile);

             //    StreamWriter sw2 = new StreamWriter(sProjectFile, false);
             //    sw2.Write(projectFileContents);
             //    sw2.Flush();
             //    sw2.Close();
             //}
        }

        private void EnsureFileIsNotReadOnly(string fileFullPath)
        {
            FileAttributes attributes = File.GetAttributes(fileFullPath);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                // Make the file RW
                attributes = RemoveAttribute(attributes, FileAttributes.ReadOnly);
                File.SetAttributes(fileFullPath, attributes);
                //Console.WriteLine("The {0} file is no longer RO.", fileFullPath);
            }
            //else
            //{
            //    // Make the file RO
            //    File.SetAttributes(fileFullPath, File.GetAttributes(fileFullPath) | FileAttributes.Hidden);
            //    Console.WriteLine("The {0} file is now RO.", fileFullPath);
            //}
        }
        private void EnsureFolderIsNotReadOnly(string dirFullPath)
        {
            var di = new DirectoryInfo(dirFullPath);

            foreach(FileInfo fi in di.GetFiles("*.*"))
            {
                fi.Attributes &= ~FileAttributes.ReadOnly;
            }

            //di.Attributes &= ~FileAttributes.Hidden;
            //di.Attributes &= ~FileAttributes.ReadOnly;
            di.Attributes = FileAttributes.Normal;

            //File.SetAttributes(dirFullPath, FileAttributes.ReadOnly);
            //File.SetAttributes(dirFullPath, FileAttributes.ReadOnly);
 


        }
   
        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }

        private class sqlParms
        {
            public string paramName;
            public string paramDBType;
            public string paramWrapperClass;
            public string paramStructuredTypeName;
            public bool isOutput;
        }
        private List<sqlParms> GetSqlParmList(string sProcedure)
        {
            List<sqlParms> sqlParmList = new List<sqlParms>();
            string[] sProcedureSplit = sProcedure.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            bool beforeAS = true;
            int iCounter = 0;
            while (beforeAS)
            {
                string sLine = sProcedureSplit[iCounter];
                sLine = sLine.Trim();
                iCounter++;

                if (sLine.StartsWith("@"))
                {
                    sqlParms p = new sqlParms();

                    if (sLine.ToUpper().Contains("OUTPUT"))
                    {
                        p.isOutput = true;
                    }
                    sLine = sLine.Replace("\t", " ");
                    int spaceLen = sLine.IndexOf(' ');
                    p.paramName = sLine.Substring(1, spaceLen - 1);
                    p.paramDBType = sLine.Substring(spaceLen + 1);


                    int spaceOrEqualsLen = p.paramDBType.IndexOf(' ');
                    if (spaceOrEqualsLen == -1)
                    {
                        spaceOrEqualsLen = p.paramDBType.IndexOf('=');
                    }
                    if (spaceOrEqualsLen == -1)
                    {
                        spaceOrEqualsLen = p.paramDBType.IndexOf('(');
                    }
                    if (spaceOrEqualsLen != -1)
                    {
                        p.paramDBType = p.paramDBType.Substring(0, spaceOrEqualsLen);
                    }

                    p.paramDBType = p.paramDBType.Replace(",", string.Empty);
                    p.paramWrapperClass = GetWrapperParameterClassFromSQLDBType(p.paramDBType);
                    if (p.paramWrapperClass == "tableParameter")
                    {
                        string typeName = string.Empty;
                        //Find the READONLY
                        int readOnlyIndex = sLine.ToUpper().IndexOf("READONLY");
                        string sLineTemp = sLine.Substring(0, readOnlyIndex - 1);
                        int lastSpaceIndex = sLineTemp.LastIndexOf(" ");
                        typeName = sLineTemp.Substring(lastSpaceIndex).Trim();
                        p.paramStructuredTypeName = typeName;
                    }
                    sqlParmList.Add(p);
                }

                if (sLine == "AS")
                {
                    beforeAS = false;
                }
            }


            return sqlParmList;
        }
        private string GetWrapperParameterClassFromSQLDBType(string sDBType)
        {
            sDBType = sDBType.ToUpper();
            if (sDBType == "INT" || sDBType == "BIT")
            {
                return "intParameter";
            }
            else if (sDBType == "FLOAT")
            {
                return "floatParameter";
            }
            else if (sDBType == "DECIMAL")
            {
                return "decimalParameter";
            }
            else if (sDBType == "IMAGE")
            {
                return "byteArrayParameter";
            }
            else if (sDBType == "CHAR")
            {
                return "charParameter";
            }
            else if (sDBType == "VARCHAR" || sDBType == "TEXT")
            {
                return "stringParameter";
            }
            else if (sDBType == "DATETIME")
            {
                return "datetimeParameter";
            }
            else if (sDBType.Contains("TYPE"))
            {
                return "tableParameter";
            }
            return "unknownParameter";
        }
        private string MakeWrapperClass(string sProcedure, string sProcedureName, MIDRetail.Data.storedProcedureTypes procedureType, string sTableName)
        {
          
            string sOutput = string.Empty;
            string htab3 = "\t\t\t";
            sOutput += "public static " + sProcedureName + "_def " + sProcedureName + " = new " + sProcedureName + "_def();" + System.Environment.NewLine;
            sOutput += htab3 + "public class " + sProcedureName + "_def : baseStoredProcedure" + System.Environment.NewLine;
            sOutput += htab3 + "{" + System.Environment.NewLine;
            sOutput += htab3 + "\t" + "//\"file:///C:\\SCMVS2010\\gohere.html?filepath=DatabaseDefinition\\SQL_StoredProcedures\\" + sProcedureName + ".SQL\"" + System.Environment.NewLine;
            sOutput += System.Environment.NewLine;

            List<sqlParms> sqlParmList = GetSqlParmList(sProcedure);

            int TotalInputParameterCount = 0;
         
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    TotalInputParameterCount++;
                    sOutput += htab3 + "    public " + p.paramWrapperClass + " " + p.paramName + ";" + System.Environment.NewLine;
                }
            }
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == true)
                {
                    sOutput += htab3 + "    public " + p.paramWrapperClass + " " + p.paramName + "; //Declare Output Parameter" + System.Environment.NewLine;
                }
            }
 
      
            string sProcedureType;
            string sExecutionReturnType;
            string sExecutionName;
            string sExecutionCallName;
            GetWrapperStringsFromProcedureType(procedureType, out sProcedureType, out sExecutionReturnType, out sExecutionName, out sExecutionCallName);

            sOutput += htab3 + "" + System.Environment.NewLine;
            sOutput += htab3 + "    public " + sProcedureName + "_def()" + System.Environment.NewLine;
            sOutput += htab3 + "    {" + System.Environment.NewLine;
            sOutput += htab3 + "        base.procedureName = ~" + sProcedureName + "~;" + System.Environment.NewLine;
            sOutput += htab3 + "        base.procedureType = " + sProcedureType + ";" + System.Environment.NewLine;
            sOutput += htab3 + "        base.tableNames.Add(~" + sTableName + "~);" + System.Environment.NewLine;

            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    string tableStructuredType = string.Empty;
                    if (p.paramWrapperClass == "tableParameter")
                    {
                        tableStructuredType = "~" + p.paramStructuredTypeName + "~, ";
                    }
                    sOutput += htab3 + "        " + p.paramName + " = new " + p.paramWrapperClass + "(~@" + p.paramName + "~, " + tableStructuredType + "base.inputParameterList);" + System.Environment.NewLine;
                }
            }
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == true)
                {
                    sOutput += htab3 + "        " + p.paramName + " = new " + p.paramWrapperClass + "(~@" + p.paramName + "~, base.outputParameterList); //Add Output Parameter" + System.Environment.NewLine;
                }
            }
            sOutput += htab3 + "    }" + System.Environment.NewLine;
            sOutput += htab3 + "" + System.Environment.NewLine;
            sOutput += htab3 + "    public " + sExecutionReturnType + " " + sExecutionCallName + "(DatabaseAccess _dba";

            if (TotalInputParameterCount > 0)
            {
                sOutput += ", ";
            }


            string sComma = "," + System.Environment.NewLine;
            string spacer2 = "";
            string spacer2Length = "    public " + sExecutionReturnType + " " + sExecutionCallName + "(";

            for (int j = 1; j <= spacer2Length.Length; j++)
            {
                spacer2 += " ";
            }
            if (TotalInputParameterCount > 1)
            {
                sOutput += System.Environment.NewLine + htab3 + spacer2;
            }
            int parameterCount = 0;
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    parameterCount++;
                    if (parameterCount == TotalInputParameterCount)
                    {
                        sComma = "";
                    }
                    if (parameterCount == 1)
                    {
                        sOutput += GetParameterCodeTypeFromDBType(p.paramDBType) + " " + p.paramName + sComma;
                    }
                    else
                    {
                        sOutput += htab3 + spacer2 + GetParameterCodeTypeFromDBType(p.paramDBType) + " " + p.paramName + sComma;
                    }
                }
            }
            if (TotalInputParameterCount > 1)
            {
                sOutput += System.Environment.NewLine;
            }
            if (parameterCount == 1)
            {
                sOutput += ")" + System.Environment.NewLine;
            }
            else
            {
                if (TotalInputParameterCount > 0)
                {
                    sOutput += htab3 + spacer2;
                }
                sOutput += ")" + System.Environment.NewLine;
            }

            sOutput += htab3 + "    {" + System.Environment.NewLine;
           
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    sOutput += htab3 + "        this." + p.paramName + ".SetValue(" + p.paramName + ");" + System.Environment.NewLine;
                }
            }
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == true)
                {
                    sOutput += htab3 + "        this." + p.paramName + ".SetValue(null); //Initialize Output Parameter" + System.Environment.NewLine;
                }
            }
            if (sExecutionCallName == "UpdateWithReturnCode")
            {
                sOutput += htab3 + "        " + sExecutionName + "(_dba);" + System.Environment.NewLine;
                foreach (sqlParms p in sqlParmList)
                {
                    if (p.isOutput == true)
                    {
                        sOutput += htab3 + "        return (int)this." + p.paramName + ".Value;" + System.Environment.NewLine;
                    }
                }
            }
            else
            {
                sOutput += htab3 + "        return " + sExecutionName + "(_dba);" + System.Environment.NewLine;
            }
            sOutput += htab3 + "    }" + System.Environment.NewLine;
            sOutput += htab3 + "}" + System.Environment.NewLine;
            sOutput = sOutput.Replace('~', '"');
            return sOutput;
        }
        private string MakeCallingCode(string sProcedure, string sProcedureName, MIDRetail.Data.storedProcedureTypes procedureType, string sTableName)
        {

          
            string htab4 = "\t\t\t\t";
           

            List<sqlParms> sqlParmList = GetSqlParmList(sProcedure);

            int TotalInputParameterCount = 0;
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    TotalInputParameterCount++; 
                }
            }
           


            string sProcedureType;
            string sExecutionReturnType;
            string sExecutionName;
            string sExecutionCallName;
            GetWrapperStringsFromProcedureType(procedureType, out sProcedureType, out sExecutionReturnType, out sExecutionName, out sExecutionCallName);

            string sOutput2 = string.Empty;

            string sReturn = "";
            if (procedureType == MIDRetail.Data.storedProcedureTypes.Read)
            {
                sReturn = "return ";
            }
            sOutput2 += sReturn + "StoredProcedures." + sProcedureName + "." + sExecutionCallName + "(_dba";
            if (TotalInputParameterCount > 0)
            {
                sOutput2 += ", ";
            }
            int spaceLength = sReturn.Length + ("StoredProcedures." + sProcedureName + "." + sExecutionCallName + "(").Length;
            string spacer = htab4;
            for (int j = 1; j <= spaceLength; j++)
            {
                spacer += " ";
            }
            if (TotalInputParameterCount > 1)
            {
                sOutput2 += System.Environment.NewLine + spacer;
            }
            int parameterCount = 0;
            string sComma = "," + System.Environment.NewLine;
           
            foreach (sqlParms p in sqlParmList)
            {
                if (p.isOutput == false)
                {
                    parameterCount++;
                    if (parameterCount == TotalInputParameterCount)
                    {
                        sComma = "";
                    }
                    if (parameterCount == 1)
                    {
                        sOutput2 += p.paramName + ": a" + p.paramName + sComma;
                    }
                    else
                    {
                        sOutput2 += spacer + p.paramName + ": a" + p.paramName + sComma;
                    }
                }
            }
           
            if (TotalInputParameterCount > 1)
            {
                sOutput2 += System.Environment.NewLine + spacer;
            }
            sOutput2 += ");";
           
            return sOutput2;
        }
       
        private string GetParameterCodeTypeFromDBType(string s)
        {
            s = s.ToUpper();
            if (s == "INT" || s == "BIT")
            {
                return "int?";
            }
            else if (s == "FLOAT")
            {
                return "double?";
            }
            else if (s == "DECIMAL")
            {
                return "decimal?";
            }
            else if (s == "IMAGE")
            {
                return "byte[]";
            }
            else if (s == "CHAR")
            {
                return "char?";
            }
            else if (s == "VARCHAR" || s == "TEXT")
            {
                return "string";
            }
            else if (s == "DATETIME")
            {
                return "DateTime?";
            }
            else if (s.Contains("TYPE"))
            {
                return "DataTable";
            }

            return "int?";
        }
        private string GetTableNameFromProcedureName(string sProcedureName)
        {
            string sTableName = string.Empty;
            if (sProcedureName.StartsWith("MID"))
            {
                int endPoint = sProcedureName.IndexOf("READ");
                if (endPoint == -1)
                {
                    endPoint = sProcedureName.IndexOf("INSERT");
                }
                if (endPoint == -1)
                {
                    endPoint = sProcedureName.IndexOf("UPDATE");
                }
                if (endPoint == -1)
                {
                    endPoint = sProcedureName.IndexOf("DELETE");
                }
                if (endPoint != -1)
                {
                    sTableName = sProcedureName.Substring(4, endPoint - 5);
                }
                else
                {
                    sTableName = "<TABLENAME>";
                }
            }
            else
            {
                sTableName = "<TABLENAME>";
            }
            return sTableName;
        }
        private MIDRetail.Data.storedProcedureTypes GetProcedureTypeFromProcedure(string sProcedure, string sProcedureName, out bool unknown)
        {
            unknown = false;

            if (sProcedureName.StartsWith("MID"))
            {
                if (sProcedureName.Contains("READ_COUNT"))
                {
                    return MIDRetail.Data.storedProcedureTypes.RecordCount;
                }
                if (sProcedureName.Contains("READ"))
                {
                    return MIDRetail.Data.storedProcedureTypes.Read;
                }
                if (sProcedureName.Contains("UPDATE"))
                {
                    if (sProcedure.Contains("@RETURNCODE") || sProcedure.Contains("@RETURN_CODE") || sProcedure.Contains("@RC "))
                    {
                        return MIDRetail.Data.storedProcedureTypes.UpdateWithReturnCode;
                    }
                    else
                    {
                        return MIDRetail.Data.storedProcedureTypes.Update;
                    }
                }
                if (sProcedureName.Contains("INSERT"))
                {
                    if (sProcedure.Contains("SCOPE_IDENTITY"))
                    {
                        return MIDRetail.Data.storedProcedureTypes.InsertAndReturnRID;
                    }
                    else
                    {
                        return MIDRetail.Data.storedProcedureTypes.Insert;
                    }
                }
                if (sProcedureName.Contains("DELETE"))
                {
                    return MIDRetail.Data.storedProcedureTypes.Delete;
                }
   
            }
            unknown = true;
            return MIDRetail.Data.storedProcedureTypes.Read;
        }

        private void GetWrapperStringsFromProcedureType( MIDRetail.Data.storedProcedureTypes procedureType, out string sProcedureType, out string sExecutionReturnType, out string sExecutionName, out string sExecutionCallName)
        {
            sProcedureType = string.Empty;
            sExecutionReturnType = string.Empty;
            sExecutionName = string.Empty;
            sExecutionCallName = string.Empty;

            if (procedureType == MIDRetail.Data.storedProcedureTypes.Read)
            {
                sExecutionReturnType = "DataTable";
                sExecutionName = "ExecuteStoredProcedureForRead";
                sExecutionCallName = "Read";
                sProcedureType = "storedProcedureTypes.Read";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.ReadAsDataset)
            {
                sExecutionReturnType = "DataSet";
                sExecutionName = "ExecuteStoredProcedureForReadAsDataSet";
                sExecutionCallName = "ReadAsDataSet";
                sProcedureType = "storedProcedureTypes.ReadAsDataset";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.Delete)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForDelete";
                sExecutionCallName = "Delete";
                sProcedureType = "storedProcedureTypes.Delete";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.InsertAndReturnRID)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForInsertAndReturnRID";
                sExecutionCallName = "InsertAndReturnRID";
                sProcedureType = "storedProcedureTypes.InsertAndReturnRID";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.Insert)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForInsert";
                sExecutionCallName = "Insert";
                sProcedureType = "storedProcedureTypes.Insert";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.RecordCount)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForRecordCount";
                sExecutionCallName = "ReadRecordCount";
                sProcedureType = "storedProcedureTypes.RecordCount";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.ScalarValue)
            {
                sExecutionReturnType = "object";
                sExecutionName = "ExecuteStoredProcedureForScalarValue";
                sExecutionCallName = "ReadValues";
                sProcedureType = "storedProcedureTypes.ScalarValue";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.Update)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForUpdate";
                sExecutionCallName = "Update";
                sProcedureType = "storedProcedureTypes.Update";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.UpdateWithReturnCode)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForUpdate";
                sExecutionCallName = "UpdateWithReturnCode";
                sProcedureType = "storedProcedureTypes.UpdateWithReturnCode";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.Maintenance)
            {
                sExecutionReturnType = "int";
                sExecutionName = "ExecuteStoredProcedureForMaintenance";
                sExecutionCallName = "Execute";
                sProcedureType = "storedProcedureTypes.Maintenance";
            }
            if (procedureType == MIDRetail.Data.storedProcedureTypes.OutputOnly)
            {
                sExecutionReturnType = "void";
                sExecutionName = "ExecuteStoredProcedureForOutputParameters";
                sExecutionCallName = "GetOutput";
                sProcedureType = "storedProcedureTypes.OutputOnly";
            }
       
        }

        private static DataSet dsProcedureTypes = null;
        private DataSet GetProcedureTypesDataSet()
        {
            if (dsProcedureTypes == null)
            {
                dsProcedureTypes = new DataSet();
                dsProcedureTypes.Tables.Add("ProcedureTypes");
                dsProcedureTypes.Tables[0].Columns.Add("procedureTypeName");

                DataRow dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "Read";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "ReadAsDataset";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "Delete";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "InsertAndReturnRID";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "Insert";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "RecordCount";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "ScalarValue";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "Update";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "UpdateWithReturnCode";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "Maintenance";
                dsProcedureTypes.Tables[0].Rows.Add(dr);

                dr = dsProcedureTypes.Tables[0].NewRow();
                dr["procedureTypeName"] = "OutputOnly";
                dsProcedureTypes.Tables[0].Rows.Add(dr);
            }
            return dsProcedureTypes;
        }
    }
}
