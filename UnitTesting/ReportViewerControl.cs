using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Windows;
using MIDRetail.DataCommon;

namespace UnitTesting
{
    public partial class ReportViewerControl : UserControl
    {
        public ReportViewerControl()
        {
            InitializeComponent();
        }
        private DataSet dsStoredProcedures;
        public void BindGrid(DataSet aDataSet)
        {
            dsStoredProcedures = aDataSet;
            ultraGrid1.DataSource = null;

            BindingSource bs = new BindingSource(aDataSet, aDataSet.Tables[0].TableName);
            this.ultraGrid1.DataSource = bs;
            this.ultraGrid1.DisplayLayout.Bands[0].PerformAutoResizeColumns(false, Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
            //this.ultraGrid1.Rows[0].Selected = true;
        }
        private void TestManagerControl_Load(object sender, EventArgs e)
        {
            Infragistics.Shared.ResourceCustomizer rc = Infragistics.Win.UltraWinGrid.Resources.Customizer;
            rc.SetCustomizedString("GroupByBoxDefaultPrompt", "Drag a column here to group by that column.");
            rc.SetCustomizedString("ColumnChooserButtonToolTip", "Click here to show/hide columns");
            rc.SetCustomizedString("ColumnChooserDialogCaption", "Choose Columns");
            this.ultraGrid1.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;


            this.ultraGrid1.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this.ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this.ultraGrid1.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this.ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;

         
        }
        private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            //e.Layout.Bands[0].Columns["AUDIT_DATETIME"].Format = "MM/dd/yyyy HH:mm:ss";
           // e.Layout.Bands[0].Columns["testName"].Header.Caption = "Test";
           // e.Layout.Bands[0].Columns["procedureName"].Hidden = true;
            //e.Layout.Bands[0].Columns["moduleName"].Header.Caption = "Module";
            //e.Layout.Bands[0].Columns["procedureName"].Header.Caption = "Procedure";
            //e.Layout.Bands[0].Columns["procedureType"].Header.Caption = "Type";
            //e.Layout.Bands[0].Columns["tableNames"].Header.Caption = "Table";
            //e.Layout.Bands[0].Columns["testCount"].Header.Caption = "Test Count";
            //e.Layout.Bands[0].Columns["hasDefaults"].Hidden = true;
            //e.Layout.Bands[0].Columns["hasNoLock"].Header.Caption = "Has NoLock";
            //e.Layout.Bands[0].Columns["hasRowLock"].Header.Caption = "Has RowLock";
            //e.Layout.Bands[0].Columns["canRestoreState"].Header.Caption = "Can Reset State";
            //e.Layout.Bands[0].Columns["validation"].Header.Caption = "Validation Msg";
          
            //e.Layout.Bands[0].Columns["moduleName"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            //e.Layout.Bands[0].Columns["procedureName"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;
            //e.Layout.Bands[0].Columns["testCount"].AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.AllRowsInBand;

            //if (e.Layout.Bands[0].Columns.Contains("body"))
            //{
            //    e.Layout.Bands[0].Columns["body"].Hidden = true;
            //    e.Layout.Bands[0].Columns["bodyOnDB"].Hidden = true;
            //}
        }
        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btnRefresh":
                    //UnitTests.RefreshStoredProcedures();
                    if (ReportType == UnitTests.ReportTypes.ProceduresNotReferenced)
                    {
                        MakeProceduresNotReferencedDataSet();
                    }
                    else if (ReportType == UnitTests.ReportTypes.ParameterListing)
                    {
                        MakeParameterListingDataSet();
                    }
                    else if (ReportType == UnitTests.ReportTypes.DatabaseComparison)
                    {
                        MakeDatabaseComparisonDataSet();
                    }
                    else if (ReportType == UnitTests.ReportTypes.ObjectDBOCheck)
                    {
                        MakeObjectDBOCheckDataSet();
                    }
                    break;
                case "btnEdit":
                    if (this.ultraGrid1.Selected.Rows.Count > 0)
                    {
                        EditStoredProcedures(GetSelectedStoredProcedures());
                    }
                    else
                    {
                        MessageBox.Show("Please select one or more stored procedures.");
                    }
                    break;
                case "btnCompare":
                      if (this.ultraGrid1.Selected.Rows.Count > 0)
                    {
                        CompareStoredProcedures(GetSelectedStoredProcedures());
                    }
                    else
                    {
                        MessageBox.Show("Please select one or more stored procedures.");
                    }
                    break;
                #region "Grid Tools"

                case "gridSearchFindButton":
                    MIDRetail.Windows.Controls.SharedControlRoutines.SearchGrid(ultraGrid1, (((Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"]).Text));
                    break;
                case "gridSearchClearButton":
                    Infragistics.Win.UltraWinToolbars.TextBoxTool t = (Infragistics.Win.UltraWinToolbars.TextBoxTool)this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Tools["gridSearchText"];
                    t.Text = "";
                    MIDRetail.Windows.Controls.SharedControlRoutines.ClearGridSearchResults(ultraGrid1);
                    break;

                case "gridShowSearchToolbar":
                    this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible = !this.ultraToolbarsManager1.Toolbars["Grid Search Toolbar"].Visible;
                    break;

                case "gridShowGroupArea":
                    this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = !this.ultraGrid1.DisplayLayout.GroupByBox.Hidden;
                    break;

                case "gridShowFilterRow":
                    if (this.ultraGrid1.DisplayLayout.Override.FilterUIType == Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow)
                    {
                        this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.HeaderIcons;
                    }
                    else
                    {
                        this.ultraGrid1.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
                    }
                    break;

                case "gridExportSelected":
                    //UltraGridExcelExportWrapper exporter = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //exporter.ExportSelectedRowsToExcel();
                    SharedRoutines.GridExport.ExportSelectedRowsToExcel(this.ultraGrid1);
                    break;

                case "gridExportAll":
                    //UltraGridExcelExportWrapper exporter2 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //exporter2.ExportAllRowsToExcel();
                    SharedRoutines.GridExport.ExportAllRowsToExcel(this.ultraGrid1);
                    break;

                case "gridEmailSelectedRows":
                    //UltraGridExcelExportWrapper exporter3 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter3.ExportSelectedRowsToExcelAsAttachment());

                    SharedRoutines.GridExport.EmailSelectedRows("Stored Procedures", "StoredProcedures.xls", this.ultraGrid1);
                    break;

                case "gridEmailAllRows":
                    //UltraGridExcelExportWrapper exporter4 = new UltraGridExcelExportWrapper(this.ultraGrid1);
                    //ShowEmailForm(exporter4.ExportAllRowsToExcelAsAttachment());
                    SharedRoutines.GridExport.EmailAllRows("Stored Procedures", "StoredProcedures.xls", this.ultraGrid1);
                    break;

                

                #endregion

            }
        }

        private void EditStoredProcedures(DataRow[] drProcedures)
        {
            //string sCurrentProjectDir = System.AppDomain.CurrentDomain.BaseDirectory;
            //sCurrentProjectDir = sCurrentProjectDir.Substring(0, sCurrentProjectDir.IndexOf("UnitTesting"));
            foreach (DataRow dr in drProcedures)
            {

                string fileName = (string)dr["File"];

                //Check #1 - Ensure procedure exists in project
                string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\" + fileName;

                if (System.IO.File.Exists(sPath) == true)
                {
                    System.Diagnostics.Process.Start(sPath);
                }
            }
        }
        private void CompareStoredProcedures(DataRow[] drProcedures)
        {
            foreach (DataRow dr in drProcedures)
            {
                string procedureName = (string)dr["Procedure"];
                string onDB = (string)dr["On Database"];
                if (onDB == "N")
                {
                    MessageBox.Show("Procedure must exist on database before comparing.");
                    return;
                }

                string body = (string)dr["body"];
                string bodyOnDB = (string)dr["bodyOnDB"];

                string fileName = procedureName + "_inProject.txt";
                string fullpath = string.Empty;
                UnitTests.SaveTempSQLFile(body, fileName, out fullpath);

                string fileName2 = procedureName + "_onDatabase.txt";
                string fullpath2 = string.Empty;
                UnitTests.SaveTempSQLFile(bodyOnDB, fileName2, out fullpath2);

                if (System.IO.File.Exists(fullpath) == true)
                {
                    //System.Diagnostics.Process.Start("BCompare.exe fullpath", );
                    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    psi.Arguments = fullpath + " " + fullpath2;
                    //psi.FileName = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\streams.exe";
                    psi.FileName = "C:\\Program Files (x86)\\Beyond Compare 3\\BCompare.exe";
                    System.Diagnostics.Process.Start(psi);
                }
             
            }
        }

        public DataRow[] GetSelectedStoredProcedures()
        {
            List<DataRow> drList = new List<DataRow>();
            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow ur in this.ultraGrid1.Selected.Rows)
            {
                if (ur.ListObject != null)
                {
                    DataRow dr = ((DataRowView)ur.ListObject).Row;
                    drList.Add(dr);
                }
            }
            return drList.ToArray<DataRow>();
        }

        public DataSet dsReportData;
        public UnitTests.ReportTypes ReportType;
        public void MakeProceduresNotReferencedDataSet()
        {
            this.ultraToolbarsManager1.Tools["btnEdit"].SharedProps.Visible = true;
            this.ultraToolbarsManager1.Tools["btnCompare"].SharedProps.Visible = false;
            dsReportData = new DataSet();
            dsReportData.Tables.Add("Data");
            dsReportData.Tables[0].Columns.Add("File");
            dsReportData.Tables[0].Columns.Add("Procedure");

            string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sPath);
            foreach (System.IO.FileInfo fi in di.GetFiles("*.SQL"))
            {
                string exclude_flag = "REFERENCED_FROM_SQL_ONLY";
                string exclude_flag2 = "REFERENCED_FROM_SQL_ADAPTER";
                string exclude_flag3 = "REFERENCED_DYNAMICALLY_IN_CODE";
                string procedureBody = System.IO.File.ReadAllText(fi.FullName);

                if (procedureBody.Contains(exclude_flag) == false && procedureBody.Contains(exclude_flag2) == false && procedureBody.Contains(exclude_flag3) == false)
                {
                    string procedureName = fi.Name.Replace(".SQL", string.Empty);
                    if (UnitTests.DoesProcedureExist(procedureName) == false)
                    {
                        DataRow dr = dsReportData.Tables[0].NewRow();
                        dr["File"] = fi.Name;
                        dr["Procedure"] = procedureName;
                        dsReportData.Tables[0].Rows.Add(dr);
                    }
                }
            }
            this.BindGrid(dsReportData);
          
        }

        public void MakeParameterListingDataSet()
        {
            this.ultraToolbarsManager1.Tools["btnEdit"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnCompare"].SharedProps.Visible = false;
            dsReportData = new DataSet();
            dsReportData.Tables.Add("Data");
            dsReportData.Tables[0].Columns.Add("Module");
            dsReportData.Tables[0].Columns.Add("Procedure");
            dsReportData.Tables[0].Columns.Add("Procedure Type");
            dsReportData.Tables[0].Columns.Add("Direction");
            dsReportData.Tables[0].Columns.Add("Parameter");
            dsReportData.Tables[0].Columns.Add("Parameter Type");

            foreach (MIDRetail.Data.baseStoredProcedure sp in Shared_BaseStoredProcedures.storedProcedureList)
            {
                int firstPlusLen = sp.GetType().FullName.IndexOf('+');
                string moduleName = sp.GetType().FullName.Substring(0, firstPlusLen);

                Type declaringType = sp.GetType().DeclaringType;
                string declaringTypeName = declaringType.Name;
                moduleName = moduleName.Replace("MIDRetail.Data.", "");


                foreach (MIDRetail.Data.baseParameter bp in sp.inputParameterList)
                {
                    DataRow dr = dsReportData.Tables[0].NewRow();
                    dr["Module"] = moduleName;
                    dr["Procedure"] = sp.procedureName;
                    dr["Procedure Type"] = sp.procedureType;
                    dr["Direction"] = "Input";
                    dr["Parameter"] = bp.parameterName;
                    dr["Parameter Type"] = bp.DBType.ToString();
                    dsReportData.Tables[0].Rows.Add(dr);
                }
                foreach (MIDRetail.Data.baseParameter bp in sp.outputParameterList)
                {
                    DataRow dr = dsReportData.Tables[0].NewRow();
                    dr["Module"] = moduleName;
                    dr["Procedure"] = sp.procedureName;
                    dr["Procedure Type"] = sp.procedureType;
                    dr["Direction"] = "Output";
                    dr["Parameter"] = bp.parameterName;
                    dr["Parameter Type"] = bp.DBType.ToString();
                    dsReportData.Tables[0].Rows.Add(dr);
                }
            }

            this.BindGrid(dsReportData);
        }

        public void MakeDatabaseComparisonDataSet()
        {
            this.ultraToolbarsManager1.Tools["btnEdit"].SharedProps.Visible = true;
            this.ultraToolbarsManager1.Tools["btnCompare"].SharedProps.Visible = true;
            dsReportData = new DataSet();
            dsReportData.Tables.Add("Data");
            dsReportData.Tables[0].Columns.Add("Module");
            dsReportData.Tables[0].Columns.Add("Procedure");
            dsReportData.Tables[0].Columns.Add("Procedure Type");
            dsReportData.Tables[0].Columns.Add("On Database");
            dsReportData.Tables[0].Columns.Add("In Project");
            dsReportData.Tables[0].Columns.Add("Syncronized");
            dsReportData.Tables[0].Columns.Add("body");
            dsReportData.Tables[0].Columns.Add("bodyOnDB");
            dsReportData.Tables[0].Columns.Add("File");

            string cmd = "select o.name as procedureName, c.text as procedureBody from sys.syscomments c inner join sys.objects o on c.id = o.object_id and o.type = 'P'";
            bool hasError;
            string failureMsg;
            DataTable dtProcedures = Shared_GenericExecution.GetGenericDataTable(UnitTests.GetConnectionForEnvironment(UnitTests.defaultEnvironment), cmd, out hasError, out failureMsg);


            foreach (MIDRetail.Data.baseStoredProcedure sp in Shared_BaseStoredProcedures.storedProcedureList)
            {
                int firstPlusLen = sp.GetType().FullName.IndexOf('+');
                string moduleName = sp.GetType().FullName.Substring(0, firstPlusLen);

                Type declaringType = sp.GetType().DeclaringType;
                string declaringTypeName = declaringType.Name;
                moduleName = moduleName.Replace("MIDRetail.Data.", "");

                DataRow dr = dsReportData.Tables[0].NewRow();
                dr["Module"] = moduleName;
                dr["Procedure"] = sp.procedureName;
                dr["Procedure Type"] = sp.procedureType.ToString();
                dr["In Project"] = "Y";

                string sPath = Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\SQL_StoredProcedures\\" + sp.procedureName + ".SQL";
                dr["File"] = sPath;

                DataRow[] drFind = dtProcedures.Select("procedureName = '" + sp.procedureName + "'");
                if (drFind.Length > 0)
                {
                    dr["On Database"] = "Y";

                  
                    if (System.IO.File.Exists(sPath))
                    {
                        string[] procedureBodyLines = System.IO.File.ReadAllLines(sPath);
                        string procedureBody = string.Empty;
                        foreach(string s in procedureBodyLines)
                        {
                            if (s != string.Empty && s != "GO")
                            {
                                procedureBody += s + System.Environment.NewLine;
                            }
                        }
                        
                        string bodyOnDatabase = (string)drFind[0]["procedureBody"];
                        dr["body"] = procedureBody;
                        dr["bodyOnDB"] = bodyOnDatabase;

                        if (procedureBody == bodyOnDatabase)
                        {
                            dr["Syncronized"] = "Y";
                        }
                        else
                        {
                            dr["Syncronized"] = "N";
                        }

                    }
                    else
                    {
                        dr["Syncronized"] = "?";
                    }
                }
                else
                {
                    dr["On Database"] = "N";
                    dr["Syncronized"] = "N";
                }
              

                dsReportData.Tables[0].Rows.Add(dr);
              
            }
            foreach (DataRow drProcedure in dtProcedures.Rows)
            {
                string procedureName = (string)drProcedure["procedureName"];
                MIDRetail.Data.baseStoredProcedure bp = Shared_BaseStoredProcedures.GetStoredProcedure(procedureName);
                if (bp == null)
                {

                    DataRow dr = dsReportData.Tables[0].NewRow();
                    dr["Module"] = string.Empty;
                    dr["Procedure"] = procedureName;
                    dr["Procedure Type"] = string.Empty;
                    dr["In Project"] = "N";
                    dr["On Database"] = "Y";
                    dsReportData.Tables[0].Rows.Add(dr);
                }
                
            }

            this.BindGrid(dsReportData);
            ultraGrid1.DisplayLayout.Bands[0].Columns["body"].Hidden = true;
            ultraGrid1.DisplayLayout.Bands[0].Columns["bodyOnDB"].Hidden = true;
            ultraGrid1.DisplayLayout.Bands[0].Columns["File"].Hidden = true;

        }

        public void MakeObjectDBOCheckDataSet()
        {
            this.ultraToolbarsManager1.Tools["btnEdit"].SharedProps.Visible = false;
            this.ultraToolbarsManager1.Tools["btnCompare"].SharedProps.Visible = false;
            dsReportData = new DataSet();
            dsReportData.Tables.Add("Data");
            dsReportData.Tables[0].Columns.Add("Name");
            dsReportData.Tables[0].Columns.Add("Type");
            dsReportData.Tables[0].Columns.Add("Has DBO");

            GetDBORowsForType("Procedure", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_STORED_PROCEDURES + "\\");
            GetDBORowsForType("Table", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_TABLES + "\\");
            GetDBORowsForType("Function", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_SCALAR_FUNCTIONS + "\\");
            GetDBORowsForType("Function", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_TABLE_FUNCTIONS + "\\");
            GetDBORowsForType("Trigger", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_TRIGGERS + "\\");
            GetDBORowsForType("Type", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_TYPES + "\\");
            GetDBORowsForType("View", Shared_UtilityFunctions.GetCurrentProjectPath() + "DatabaseDefinition\\" + Include.SQL_FOLDER_VIEWS + "\\");
       

            this.BindGrid(dsReportData);
           
        }
        private void GetDBORowsForType(string sType, string sPath)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(sPath);

            foreach (System.IO.FileInfo fi in di.GetFiles("*.SQL"))
            {
                bool hasDBO = false;


                string objBody = System.IO.File.ReadAllText(fi.FullName);

                string[] itemSplit = objBody.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sline in itemSplit)
                {
                    string s = sline.Trim().Replace("\t", string.Empty);
                    if (s.StartsWith("/*") == true || s.StartsWith("--") == true)
                    {
                        continue;
                    }
                    if (s.ToUpper().Contains("CREATE " + sType.ToUpper() + " [DBO].") == true)
                    {
                        hasDBO = true;

                    }
                }

                DataRow dr = dsReportData.Tables[0].NewRow();
                dr["Name"] = fi.Name.Replace(".SQL", string.Empty);
                dr["Type"] = sType;
                if (hasDBO)
                {
                    dr["Has DBO"] = "Y";
                }
                else
                {
                    dr["Has DBO"] = "N";
                }

                dsReportData.Tables[0].Rows.Add(dr);

            }
        }

    }
}
