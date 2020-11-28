using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
    public static class SharedRoutines
    {
        

        public static class GridExport
        {

            public static void ExportAllRowsToExcel(Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false)
            {
                SharedRoutines.GridExport.UltraGridExcelExportWrapper exporter = new SharedRoutines.GridExport.UltraGridExcelExportWrapper(ug, autoBlankRowSearchString, objectDescriptor, titleOnFirstRow, limitToOneHeaderPerBand);
                exporter.ExportAllRowsToExcel();
            }
            public static void ExportSelectedRowsToExcel(Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false)
            {
                SharedRoutines.GridExport.UltraGridExcelExportWrapper exporter = new SharedRoutines.GridExport.UltraGridExcelExportWrapper(ug, autoBlankRowSearchString, objectDescriptor, titleOnFirstRow, limitToOneHeaderPerBand);
                exporter.ExportSelectedRowsToExcel();
            }
            public static void EmailAllRows(string subject, string fileName, Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false)
            {
                SharedRoutines.GridExport.UltraGridExcelExportWrapper exporter = new SharedRoutines.GridExport.UltraGridExcelExportWrapper(ug, autoBlankRowSearchString, objectDescriptor, titleOnFirstRow, limitToOneHeaderPerBand);

                SharedRoutines.GridExport.ShowEmailForm(subject, exporter.ExportAllRowsToExcelAsAttachment(fileName));
            }
            public static void EmailSelectedRows(string subject, string fileName, Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false)
            {
                SharedRoutines.GridExport.UltraGridExcelExportWrapper exporter = new SharedRoutines.GridExport.UltraGridExcelExportWrapper(ug, autoBlankRowSearchString, objectDescriptor, titleOnFirstRow, limitToOneHeaderPerBand);

                SharedRoutines.GridExport.ShowEmailForm(subject, exporter.ExportSelectedRowsToExcelAsAttachment(fileName));
            }
            public static string BuildEmailSubjectWithUserName(MIDRetail.Business.SessionAddressBlock SAB, string initialSubject)
            {
                // string subject = "My Activity";
                MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
                System.Data.DataTable dt = secAdmin.GetUser(SAB.ClientServerSession.UserRID);
                if (dt.Rows.Count > 0)
                {
                    string userName = String.Empty;
                    string userFullName = String.Empty;
                    if (dt.Rows[0].IsNull("USER_NAME") == false)
                    {
                        userName = (string)dt.Rows[0]["USER_NAME"];
                    }
                    if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
                    {
                        userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
                    }
                    initialSubject += " - " + userName + " (" + userFullName + ")";

                }
                return initialSubject;
            }

            private static void ShowEmailForm(string subject, System.Net.Mail.Attachment a)
            {
                EmailMessageForm frm = new EmailMessageForm();
                frm.AddAttachment(a);


                frm.SetDefaults("", "", "", subject, subject + Environment.NewLine + "Please see attached file.");
                frm.ShowDialog();

            }
            /// <summary>
            /// Used to provide Excel Export Functionality for the Infragistics UltraWinGrid
            /// Just create an instance of this class, pass in your grid and call Export function.
            /// Saves in xls format - does not support xlsx format
            /// </summary>
            private class UltraGridExcelExportWrapper
            {
                private Infragistics.Win.UltraWinGrid.UltraGrid _ug;
                private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
                private bool _checkForExportSelected = false;
                private string _objectDescriptor;
                private bool _firstTime = true;
                private string _autoBlankRowSearchString;
                private bool _limitToOneHeaderPerBand;   //TT#1280-MD -jsobek -Audit Grid does not export correctly
                public UltraGridExcelExportWrapper(Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false) 
                {
                    _ug = ug;
                    _objectDescriptor = objectDescriptor;
                    _autoBlankRowSearchString = autoBlankRowSearchString;
                    _titleStringOnFirstRow = titleOnFirstRow;
                    _limitToOneHeaderPerBand = limitToOneHeaderPerBand;

                    this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter();
                    this.ultraGridExcelExporter1.RowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventHandler(ultraGridExcelExporter1_RowExporting);
                    this.ultraGridExcelExporter1.HeaderRowExporting += new Infragistics.Win.UltraWinGrid.ExcelExport.HeaderRowExportingEventHandler(ultraGridExcelExporter1_HeaderRowExporting);

                    this.ultraGridExcelExporter1.ExportStarted += new Infragistics.Win.UltraWinGrid.ExcelExport.ExportStartedEventHandler(ultraGridExcelExporter1_ExportStarted);
                    this.ultraGridExcelExporter1.ExportEnding += new Infragistics.Win.UltraWinGrid.ExcelExport.ExportEndingEventHandler(ultraGridExcelExporter1_ExportEnding);
                    this.ultraGridExcelExporter1.BandSpacing = 0;
                }

                public void ExportAllRowsToExcel()
                {
                    string myFilepath = FindSavePath();
                    string MessBoxText1 = "All " + _objectDescriptor + " sucessfully exported to \r\n";
                    string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";

                    if (myFilepath != null)
                    {
                        int rowCount = 0;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in _ug.DisplayLayout.Bands)
                        {
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in band.GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow))
                            {
                                rowCount++;
                            }
                        }
                        _checkForExportSelected = false;
                        this.ultraGridExcelExporter1.Export(_ug, myFilepath);
                        //MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Rows.Count);
                        System.Windows.Forms.MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + rowCount);
                    }
                }
                public void ExportAllRowsToExcelForPath(string filePathWithFileName)
                {
                        int rowCount = 0;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridBand band in _ug.DisplayLayout.Bands)
                        {
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in band.GetRowEnumerator(Infragistics.Win.UltraWinGrid.GridRowType.DataRow))
                            {
                                rowCount++;
                            }
                        }
                        _checkForExportSelected = false;
                        this.ultraGridExcelExporter1.Export(_ug, filePathWithFileName); 
                }


                public System.Net.Mail.Attachment ExportAllRowsToExcelAsAttachment(string workbookName)
                {

                    _checkForExportSelected = false;
                    return GetEmailAttachment(workbookName);

                }
                public System.Net.Mail.Attachment ExportSelectedRowsToExcelAsAttachment(string workbookName)
                {

                    _checkForExportSelected = true;
                    return GetEmailAttachment(workbookName);


                }
                private System.Net.Mail.Attachment GetEmailAttachment(string workbookName)
                {
                    Infragistics.Documents.Excel.Workbook wb = new Infragistics.Documents.Excel.Workbook();
                    this.ultraGridExcelExporter1.Export(_ug, wb);

                    //Infragistics does not save nicely directly to a memory stream, so saving as a file and reading it back into memory stream
                    string fileName = System.IO.Path.GetTempPath() + "\\tempExcelExport_" + Data.EnvironmentInfo.MIDInfo.userName + "_" + System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".tmp";
                    wb.Save(fileName);
                    byte[] b = System.IO.File.ReadAllBytes(fileName);
                    System.IO.File.Delete(fileName);
                    System.IO.MemoryStream streamAttachment = new System.IO.MemoryStream(b);

                    System.Net.Mail.Attachment attachmentWorkbook;
                    workbookName = (workbookName + ".xls"); //TT#1890-MD -AGallagher - Exporting Email does not include ext.
                    attachmentWorkbook = new System.Net.Mail.Attachment(streamAttachment, workbookName);
                    return attachmentWorkbook;
                }


                public void ExportSelectedRowsToExcel()
                {
                    string myFilepath = FindSavePath();
                    string MessBoxText1 = "Selected " + _objectDescriptor + " sucessfully exported to \r\n";
                    string MessBoxText2 = "Number of " + _objectDescriptor + " exported: ";

                    if (myFilepath != null)
                    {
                        _checkForExportSelected = true;
                        this.ultraGridExcelExporter1.Export(_ug, myFilepath);
                        System.Windows.Forms.MessageBox.Show(MessBoxText1 + myFilepath + "\r\n" + MessBoxText2 + _ug.Selected.Rows.Count);
                    }
                }

                private string _titleStringOnFirstRow = string.Empty;
                private bool addedTitle = false;
                private void ultraGridExcelExporter1_RowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventArgs e)
                {
                    // The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
                    //Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;
                    if (_autoBlankRowSearchString != string.Empty)
                    {
                        if (e.GridRow.ListObject != null)
                        {
                            DataRow dr = ((DataRowView)e.GridRow.ListObject).Row;
                            if (dr.Table.Columns.Contains("RowKey") == true)
                            {
                                if (e.GridRow.Cells["RowKey"].Value.ToString().StartsWith(_autoBlankRowSearchString) && _firstTime)
                                {
                                    _firstTime = false;
                                    e.CurrentRowIndex++; // this will put an empty row before the Totals
                                }
                            }
                        }

                        //if (e.GridRow.Cells.Contains("RowKey") == true)
                        //{

                        //}
                    }

                    //  Get the grid
                    //Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

                    // Get the real, on-screen row, from the export row. 
                    // Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

                    // If the on-screen row is not selected, do not export it. 
                    //if (onScreenRow.Selected == false && _checkForExportSelected == true)
                    //    e.Cancel = true;

                    // Check for the Child Band Key from the GridRow 
                    // to set CurrentColumnIndex Indentation to 0
                    // if (e.GridRow.Band.Key == "RowKeyDisplay")
                    //{
                    if (_checkForExportSelected == true)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);
                        if (onScreenRow.Selected == false)
                            e.Cancel = true;
                    }

                    e.CurrentColumnIndex = 0;
                    //}
                }
                //private void ultraGridExcelExporter1_RowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.RowExportingEventArgs e)
                //{
                //    // The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
                //    //Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;

                //    //  Get the grid
                //    //Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

                //    // Get the real, on-screen row, from the export row. 
                //    Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

                //    // If the on-screen row is not selected, do not export it. 
                //    if (onScreenRow.Selected == false && _checkForExportSelected == true)
                //        e.Cancel = true;
                //}

                //Begin TT#1280-MD -jsobek -Audit Grid does not export correctly
                private class showHeaderRowForBandClass
                {
                    public string bandKey;
                    public bool hasSelectedRows = false;
                    public bool isShown = false;
                }
                private List<showHeaderRowForBandClass> listShowHeaderRowForBand = null;
                //End TT#1280-MD -jsobek -Audit Grid does not export correctly

                private void ultraGridExcelExporter1_HeaderRowExporting(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.HeaderRowExportingEventArgs e)
                {
                    // The GridRow property on the event args is a clone of the on-screen row, and it will not pick up the Selected State 
                    //Infragistics.Win.UltraWinGrid.UltraGridRow exportRow = e.GridRow;

                    //  Get the grid
                    //Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.GridRow.Band.Layout.Grid;

                    // Get the real, on-screen row, from the export row. 
                    //Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

                    //if (onScreenRow != null)
                    //{
                    // If the on-screen row is not selected, do not export it. 
                    // if (onScreenRow.Selected == false && _checkForExportSelected == true)
                    //    e.Cancel = true;

                    // Check for the Child Band Key from the Band 
                    // to set CurrentColumnIndex Indentation to 0
                    // if (e.Band.Key == "RowKeyDisplay")
                    //{



                    //Begin TT#1280-MD -jsobek -Audit Grid does not export correctly
                    if (_limitToOneHeaderPerBand == true)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = _ug.GetRowFromPrintRow(e.GridRow);

                        if (listShowHeaderRowForBand == null)
                        {
                            listShowHeaderRowForBand = new List<showHeaderRowForBandClass>();
                            foreach (Infragistics.Win.UltraWinGrid.UltraGridBand b in _ug.DisplayLayout.Bands)
                            {
                                showHeaderRowForBandClass c = new showHeaderRowForBandClass();
                                c.bandKey = b.Key;
                                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow sRow in _ug.Selected.Rows)
                                {
                                    if (sRow.Band.Key == c.bandKey)
                                    {
                                        c.hasSelectedRows = true;
                                        break;
                                    }
                                }
                                listShowHeaderRowForBand.Add(c);
                            }
                        }
                        showHeaderRowForBandClass curHeader = listShowHeaderRowForBand.Find(x => x.bandKey == onScreenRow.Band.Key);
                        if (curHeader.hasSelectedRows && curHeader.isShown == false)
                        {
                            curHeader.isShown = true;
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;  //do not export
                        }
                    }
                    //End TT#1280-MD -jsobek -Audit Grid does not export correctly



                    e.CurrentColumnIndex = 0;
                    //}
                    //}
                    if (_titleStringOnFirstRow != string.Empty && addedTitle == false)
                    {
                        addedTitle = true;
                        Infragistics.Documents.Excel.WorksheetRow row = e.CurrentWorksheet.Rows[0];
                        row.SetCellValue(0, _titleStringOnFirstRow);
                        e.CurrentRowIndex++;
                    }
                }

                private void ultraGridExcelExporter1_ExportStarted(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.ExportStartedEventArgs e)
                {
                    // The total rows are hidden on the window ugGrid so unhide the Export rows
                    //bool firstTime = true;
                    Infragistics.Win.UltraWinGrid.UltraGrid grid = (Infragistics.Win.UltraWinGrid.UltraGrid)e.Layout.Grid;
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in e.Rows)
                    {
                        Infragistics.Win.UltraWinGrid.UltraGridRow onScreenRow = grid.GetRowFromPrintRow(row);
                        if (onScreenRow.Hidden)
                        {
                            row.Hidden = false;
                        }
                    }

                    if (_titleStringOnFirstRow != string.Empty && addedTitle == false)
                    {
                        addedTitle = true;
                        Infragistics.Documents.Excel.WorksheetRow row = e.CurrentWorksheet.Rows[0];
                        row.SetCellValue(0, _titleStringOnFirstRow);
                        e.CurrentRowIndex++;
                    }
                }

                private void ultraGridExcelExporter1_ExportEnding(object sender, Infragistics.Win.UltraWinGrid.ExcelExport.ExportEndingEventArgs e)
                {
                    foreach (Infragistics.Documents.Excel.WorksheetRow row in e.CurrentWorksheet.Rows)
                    {
                        row.Hidden = false; // this expands all parent-child rows
                    }
                }

                private String FindSavePath()
                {
                    System.IO.Stream myStream;
                    string myFilepath = null;

                    System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog1.Filter = "excel files (*.xls)|*.xls";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        try
                        {
                            if ((myStream = saveFileDialog1.OpenFile()) != null)
                            {
                                myFilepath = saveFileDialog1.FileName;
                                myStream.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("An error occurred while attempted to export: " + ex.ToString());
                        }
                    }

                    return myFilepath;
                }
            }

      
        }

        public static System.Drawing.Icon GetApplicationIcon()
        {
            return new System.Drawing.Icon(MIDRetail.Windows.Controls.MIDGraphics.ImageDir + "\\" + MIDRetail.Windows.Controls.MIDGraphics.ApplicationIcon);
        }

        public static frmFilterBuilder GetFilterFormForNewFilters(filterTypes filterType, SessionAddressBlock SAB, ExplorerAddressBlock EAB, int defaultOwnerUserRID)
        {
            frmFilterBuilder frm = new frmFilterBuilder(SAB, EAB, aReadOnly: false, executeAfterEditing: false);
            filter f = new filter(SAB.ClientServerSession.UserRID, defaultOwnerUserRID);
            f.filterType = filterType;

          
            CalendarDateSelector calendarDateSelectorForm = new CalendarDateSelector(SAB);
            if (filterUtility.getDateRangeTextForPlanDelegate == null)
            {
                filterUtility.getDateRangeTextForPlanDelegate = new GetDateRangeTextForPlanDelegate(calendarDateSelectorForm.GetDateRangeTextForPlan);
            }

            frm.filterBuilder1.SetManager(f, SAB, calendarDateSelectorForm, new SetCalendarDateRangeForPlanDelegate(calendarDateSelectorForm.SetDateForPlan), new IsStoreGroupOrLevelInUse(IsStoreGroupOrStoreGroupLevelInUse));

            f.AddInitialConditions(frm.filterBuilder1.manager); //add initial conditions for new filters
    

            frm.LoadFilter(f);
            return frm;
        }

        public static frmFilterBuilder GetFilterFormForExistingFilter(int filterRID, SessionAddressBlock SAB, ExplorerAddressBlock EAB, bool isReadOnly, bool executeAfterEditing, StoreGroupProfile groupProf = null) //TT#1313-MD -jsobek -Header Filters  
        {
           
            filter f = filterDataHelper.LoadExistingFilter(filterRID);
            if (f.filterType == filterTypes.StoreGroupDynamicFilter && groupProf != null)
            {
                //ArrayList levelRidList = new ArrayList();
                //ProfileList groupLevels = groupProf.GetGroupLevelList(false);
                //foreach (Profile p in groupLevels)
                //{
                //    levelRidList.Add(p.Key);
                //}
                // bool isInUse = IsGroupLevelInUse(levelRidList);

                //Store Groups are stored on the base METHOD table, as well as other tables
                //When group levels are referenced, the group will also be referenced in the In Use queries
                //So, rather than checked every single level for the group, we only need to see if the group is In Use
               
                bool isInUse = IsStoreGroupOrStoreGroupLevelInUse(eProfileType.StoreGroup, groupProf.Key);
                if (isInUse)
                {
                    isReadOnly = true;
                }
            }

            frmFilterBuilder frm = new frmFilterBuilder(SAB, EAB, isReadOnly, executeAfterEditing); //TT#1313-MD -jsobek -Header Filters  
            frm.filterBuilder1.isNewFilter = false;
            CalendarDateSelector calendarDateSelectorForm = new CalendarDateSelector(SAB);
            if (filterUtility.getDateRangeTextForPlanDelegate == null)
            {
                filterUtility.getDateRangeTextForPlanDelegate = new GetDateRangeTextForPlanDelegate(calendarDateSelectorForm.GetDateRangeTextForPlan);
            }       

            frm.filterBuilder1.SetManager(f, SAB, calendarDateSelectorForm, new SetCalendarDateRangeForPlanDelegate(calendarDateSelectorForm.SetDateForPlan), new IsStoreGroupOrLevelInUse(IsStoreGroupOrStoreGroupLevelInUse));
            frm.LoadFilter(f);

            return frm;
        }
        public static bool IsStoreGroupOrStoreGroupLevelInUse(eProfileType etype, int aRID)
        {
            bool isInUse = false;
            try
            {
                ArrayList ridList = new ArrayList();
                ridList.Add(aRID);
                if (ridList.Count > 0) //If no RID is selected do nothing.
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseFormAndShowProgress(ridList, etype, inUseTitle, false, out isInUse);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return isInUse;
        }
        //public static bool IsGroupLevelInUse(ArrayList ridList)
        //{
        //    bool isInUse = false;
        //    try
        //    {
        //        if (ridList.Count > 0) 
        //        {
        //            string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(eProfileType.StoreGroupLevel);
        //            MIDFormBase fb = new MIDFormBase();
        //            fb.DisplayInUseFormAndShowProgress(ridList, eProfileType.StoreGroupLevel, inUseTitle, false, out isInUse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.HandleException(ex);
        //    }
        //    return isInUse;
        //}

        public static frmFilterBuilder DoesFilterFormExist(int filterRID, ref bool foundForm, ExplorerAddressBlock EAB) 
        {
            frmFilterBuilder frm = null;

            foundForm = false;
            System.Type aType = typeof(frmFilterBuilder);

            foreach (Form childForm in EAB.Explorer.MdiChildren)
            {
                if (childForm.GetType().Equals(aType))
                {
                    frm = (frmFilterBuilder)childForm;

                    if (frm.filterBuilder1.manager.currentFilter.filterRID == filterRID)
                    {
                        foundForm = true;
                        break;
                    }
                }
            }
            return frm;
        }
     



        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        /// <summary>
        /// Single place to retrieve the header filter for assortment view screens
        /// </summary>
        /// <returns></returns>
        public static int GetHeaderFilterForAssortmentView()
        {
            return -1; //TO DO - not sure if this should default to assortment workspace filter or not
        }
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions

        //Begin TT#1388-MD -jsobek -Product Filters
        public static System.Windows.Forms.Form GetForm(Form[] MdiChildren, System.Type aType, object[] args, bool aAlwaysCreateNewForm)
        {
            try
            {
                bool foundForm = false;
                System.Windows.Forms.Form frm = null;

                if (!aAlwaysCreateNewForm)
                {
                    foreach (Form childForm in MdiChildren)
                    {
                        if (childForm.GetType().Equals(aType))
                        {
                            frm = childForm;
                            foundForm = true;
                            break;
                        }
                    }
                }

                if (aAlwaysCreateNewForm ||
                    !foundForm)
                {
                    frm = (System.Windows.Forms.Form)Activator.CreateInstance(aType, args);
                }

                return frm;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error opening window: " + aType.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        //End TT#1388-MD -jsobek -Product Filters

        public static StoreMgmt.ProgressBarOptions ProgressBarOptions_Get()
        {
            StoreMgmt.ProgressBarOptions opt = new StoreMgmt.ProgressBarOptions();
            opt.useProgressBar = true;
            opt.frm = new ProgressForm();
            opt.progressBarUpdateMax = new StoreMgmt.ProgressBar_UpdateMaxDelegate(ProgressBarOptions_UpdateMax);
            opt.progressBarUpdateText = new StoreMgmt.ProgressBar_UpdateTextDelegate(ProgressBarOptions_UpdateText);
            opt.progressBarIncrement = new StoreMgmt.ProgressBar_IncrementDelegate(ProgressBarOptions_Increment);
            opt.progressBarClose = new StoreMgmt.ProgressBar_CloseDelegate(ProgressBarOptions_Close);
            return opt;
        }

        public static void ProgressBarOptions_UpdateMax(StoreMgmt.ProgressBarOptions opt, int newMax)
        {
            ((ProgressForm)opt.frm).midProgressControl1.ultraProgressBar1.Maximum = newMax;
            opt.frm.Show();
            opt.frm.BringToFront();
        }
        public static void ProgressBarOptions_UpdateText(StoreMgmt.ProgressBarOptions opt, string newText)
        {
            ((ProgressForm)opt.frm).midProgressControl1.ultraLabel1.Text = newText;
            Application.DoEvents();
        }
        public static void ProgressBarOptions_Increment(StoreMgmt.ProgressBarOptions opt)
        {
            ((ProgressForm)opt.frm).midProgressControl1.ultraProgressBar1.IncrementValue(1);
        }
        public static void ProgressBarOptions_Close(StoreMgmt.ProgressBarOptions opt)
        {
            ((ProgressForm)opt.frm).Close();
        }
    }
}
