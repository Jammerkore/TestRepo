using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business;

// Begin TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
// End TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.

namespace MIDRetail.Windows.Controls
{
    public static class SharedControlRoutines
    {
        //public delegate void HandleControlExceptionDelegate(Exception ex);
        //private static SharedControlRoutines.ControlExceptionHelper _controlExceptiontHelper = null;
        //public static SharedControlRoutines.ControlExceptionHelper controlExceptiontHelper
        //{
        //    get { return _controlExceptiontHelper; }
        //    set { _controlExceptiontHelper = value; }
        //}
        //public class ControlExceptionHelper
        //{
        //    public HandleControlExceptionDelegate HandleControlException;
        //}

        public delegate void ExportAllRowsToExcelDelegate(Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false);
        public delegate void ExportSelectedRowsToExcelDelegate(Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false);
        public delegate void EmailAllRowsDelegate(string subject, string fileName, Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false);
        public delegate void EmailSelectedRowsDelegate(string subject, string fileName, Infragistics.Win.UltraWinGrid.UltraGrid ug, string autoBlankRowSearchString = "", string objectDescriptor = "rows", string titleOnFirstRow = "", bool limitToOneHeaderPerBand = false);

        private static SharedControlRoutines.GridExportHelper _exportHelper = null;
        public static SharedControlRoutines.GridExportHelper exportHelper
        {
            get { return _exportHelper; }
            set { _exportHelper = value; }
        }

        public class GridExportHelper
        {
            public ExportAllRowsToExcelDelegate ExportAllRowsToExcel;
            public ExportSelectedRowsToExcelDelegate ExportSelectedRowsToExcel;
            public EmailAllRowsDelegate EmailAllRows;
            public EmailSelectedRowsDelegate EmailSelectedRows;

            public GridExportHelper(ExportAllRowsToExcelDelegate exportAllRowsToExcelDelegate,
                                            ExportSelectedRowsToExcelDelegate exportSelectedRowsToExcelDelegate,
                                            EmailAllRowsDelegate emailAllRowsDelegate,
                                            EmailSelectedRowsDelegate emailSelectedRowsDelegate
                                          )
            {
                this.ExportAllRowsToExcel = exportAllRowsToExcelDelegate;
                this.ExportSelectedRowsToExcel = exportSelectedRowsToExcelDelegate;
                this.EmailAllRows = emailAllRowsDelegate;
                this.EmailSelectedRows = emailSelectedRowsDelegate;
            }
        }

        //Begin TT#1432-MD -jsobek -Unhandled Exception on "Find" in the Product Search Results 
        /// <summary>
        /// Finds all instances of a string in the grid, and highlights cells and rows where the text is found
        /// </summary>
        /// <param name="ug">The Infragistics.Win.UltraWinGrid.UltraGrid reference.</param>
        /// <param name="sTextToFind">The string to search for.</</param>
        public static void SearchGrid(Infragistics.Win.UltraWinGrid.UltraGrid ug, String sTextToFind)
        {
            if (sTextToFind.Trim() == String.Empty)
            {
                return;
            }
            try
            {
                Infragistics.Win.UltraWinGrid.UltraGridRow firstrow = null;
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ug.Rows)
                {
                    bool foundInRow = false;
                    GridSearchSingleRow(row, sTextToFind, ref firstrow, ref foundInRow);
                    if (foundInRow)
                    {
                        row.ExpandAll();
                    }
                }

                if (firstrow != null)
                {
                    ug.DisplayLayout.RowScrollRegions[0].FirstRow = firstrow;
                }
            }
            catch (Exception ex) 
            {
                ExceptionHandler.HandleException(ex);
            }
        }

        public static void GridSearchSingleRow(Infragistics.Win.UltraWinGrid.UltraGridRow row, string sTextToFind, ref Infragistics.Win.UltraWinGrid.UltraGridRow firstrow, ref bool foundInRow)
        {
            try
            {
                if (row.Cells != null) 
                {
                    foundInRow = false;
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
                    {
                        if (cell.Column.Hidden == false && cell.Text.ToLower().Contains(sTextToFind.ToLower()))
                        {
                            foundInRow = true;
                            if (firstrow == null)
                                firstrow = row;
                        }
                        else
                        {
                            cell.Appearance.BackColor = System.Drawing.Color.White;
                        }
                    }

                    if (foundInRow)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
                        {
                            if (cell.Text.ToLower().Contains(sTextToFind.ToLower()))
                            {
                                cell.Appearance.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                            }
                            else
                            {
                                cell.Appearance.BackColor = System.Drawing.Color.BlanchedAlmond;
                            }
                        }
                    }
                }
                //loop thru the child bands
                if (row.ChildBands != null)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand band in row.ChildBands)
                    {
                        bool foundInChildren = false;
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow childRow in band.Rows)
                        {
                            GridSearchSingleRow(childRow, sTextToFind, ref firstrow, ref foundInChildren);
                        }
                        if (foundInChildren)
                        {
                            row.ExpandAll();
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                ExceptionHandler.HandleException(ex);
            }
        }



        /// <summary>
        /// Clears the search results in the grid.
        /// </summary>
        /// <param name="ug"></param>
        public static void ClearGridSearchResults(Infragistics.Win.UltraWinGrid.UltraGrid ug)
        {
            try
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridRow row in ug.Rows)
                {
                    GridSearchClearSingleRow(row);
                }
            }
            catch (Exception ex) 
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        public static void GridSearchClearSingleRow(Infragistics.Win.UltraWinGrid.UltraGridRow row)
        {
            try
            {
                if (row.Cells != null) 
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridCell cell in row.Cells)
                    {
                        cell.Appearance.BackColor = System.Drawing.Color.White;
                    }
                }
                //loop thru the child bands
                if (row.ChildBands != null)
                {
                    foreach (Infragistics.Win.UltraWinGrid.UltraGridChildBand band in row.ChildBands)
                    {
                        foreach (Infragistics.Win.UltraWinGrid.UltraGridRow childRow in band.Rows)
                        {
                            GridSearchClearSingleRow(childRow);
                        }
                   
                    }
                }
            }
            catch (Exception ex) 
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        //End TT#1432-MD -jsobek -Unhandled Exception on "Find" in the Product Search Results 

        public static void SetMaskForNumericEditor(filterNumericTypes nType, Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditor)
        {
            if (nType == filterNumericTypes.Dollar)
            {
                numericEditor.MaskInput = "{LOC}$-nnn,nnn,nnn,nnn.nn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
            else if (nType == filterNumericTypes.DoubleFreeForm)
            {
                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
                //this.numericEditor.ResetMaskInput();
                numericEditor.MaskInput = "{LOC}-nn,nnn,nnn.nnnn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
            else if (nType == filterNumericTypes.Integer)
            {
                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
                numericEditor.ResetMaskInput();
                numericEditor.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Integer;
                numericEditor.MaskInput = "{LOC}-nn,nnn,nnn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
        }
        public static void SetMaskForNumericEditor(fieldDataTypes nType, Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditor)
        {
            if (nType == fieldDataTypes.NumericDollar)
            {
                numericEditor.MaskInput = "{LOC}$-nnn,nnn,nnn,nnn,nnn.nn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
            else if (nType == fieldDataTypes.NumericDouble)
            {
                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
                //this.numericEditor.ResetMaskInput();
                numericEditor.MaskInput = "{LOC}-nnn,nnn,nnn,nnn,nnn.nnnn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
            else if (nType == fieldDataTypes.NumericInteger)
            {
                //this.numericEditor.MaskInput = "$ ###,###,##0.00";
                numericEditor.ResetMaskInput();
                numericEditor.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Integer;
                numericEditor.MaskInput = "{LOC}-nnn,nnn,nnn,nnn,nnn"; //TT#1415-MD -jsobek -Store Filters -not displaying negative values in details
            }
        }

        public static bool IsDateInValidFormat(fieldDataTypes dataType, Infragistics.Win.UltraWinGrid.UltraGridCell activeCell, List<MIDMsg> msgList)
        {
            bool isValid = true;
            if (dataType == fieldDataTypes.DateNoTime || dataType == fieldDataTypes.DateWithTime || dataType == fieldDataTypes.DateOnlyTime)
            {
                if (!activeCell.EditorResolved.IsValid)
                {
                    //em.AddMsg(eMIDMessageLevel.Edit, eMIDTextCode.msg_StoreProfileInvalidDateFormat, MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileInvalidDateFormat), sourceModuleText);
                    msgList.Add(new MIDMsg { msgLevel = eMIDMessageLevel.Edit, textCode = eMIDTextCode.msg_StoreProfileInvalidDateFormat, msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreProfileInvalidDateFormat) });
                    isValid = false;
                }
            }
            return isValid;
        }
        //public static void ConvertEditorToDateTime(fieldDataTypes dataType, ref object proposedValue)
        //{
        //    if (dataType == fieldDataTypes.DateNoTime || dataType == fieldDataTypes.DateWithTime || dataType == fieldDataTypes.DateOnlyTime)
        //    {
        //        proposedValue = Convert.ToDateTime(proposedValue);
        //    }   
        //}

        public static int FilterComboBoxDropDownMinWidth = 187;

        /// <summary>
        /// Sets the dropdown width with equal to the combobox current width, while honoring with minimum width
        /// Should be called from the Paint event of the combobox
        /// </summary>
        /// <param name="cbo"></param>
        /// <param name="minWidth"></param>
        public static void EnsureComboBoxDropDownMinWidth(Infragistics.Win.UltraWinGrid.UltraCombo cbo, int minWidth)
        {
            if (cbo.Width < minWidth)
            {
                cbo.DropDownWidth = minWidth;
            }
            else
            {
                cbo.DropDownWidth = cbo.Width;
            }
        }

        public static void SetUltraComboValue(Infragistics.Win.UltraWinGrid.UltraCombo cbo, int index)
        {
            string indexField = cbo.ValueMember;
            DataTable dt = (DataTable)cbo.DataSource;
            DataRow[] drFind = dt.Select(indexField + "=" + index.ToString());
            if (drFind.Length > 0)
            {
                cbo.Value = index;
            }
        }

        public static void SetEnqueueConflictMessage(SessionAddressBlock SAB,  GenericEnqueue objEnqueue, string objectTypeName, string objectName = "")
        {
            string[] errParms = new string[3];
            errParms.SetValue(objectTypeName, 0);
            errParms.SetValue(objectName, 1);
            errParms.SetValue(((GenericConflict)objEnqueue.ConflictList[0]).UserName, 2);
            string errMsg = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
            System.Windows.Forms.DialogResult diagResult = SAB.MessageCallback.HandleMessage(
                    errMsg,
                    objectTypeName + " Conflict",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
        }
    
       
    }

    // Begin TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
    public class UltraGridStringComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {

            UltraGridCell xCell = (UltraGridCell)x;
            UltraGridCell yCell = (UltraGridCell)y;

            string text1 = xCell.Value.ToString();
            string text2 = yCell.Value.ToString();

            return String.Compare(text1, text2, true);
        }
    }

    public class UltraGridIntegerComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {

            UltraGridCell xCell = (UltraGridCell)x;
            UltraGridCell yCell = (UltraGridCell)y;

            if (xCell.Value == System.DBNull.Value || yCell.Value == System.DBNull.Value)
            {
                if (yCell.Value != System.DBNull.Value)
                {
                    return -1;
                }
                if (xCell.Value != System.DBNull.Value)
                {
                    return +1;
                }
                return 0;
            }

            int xValue = Convert.ToInt32(xCell.Value);
            int yValue = Convert.ToInt32(yCell.Value);
            if (xValue < yValue)
            {
                return -1;
            }
            if (xValue > yValue)
            {
                return +1;
            }

            return 0;
        }
    }

    public class UltraGridDoubleComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {

            UltraGridCell xCell = (UltraGridCell)x;
            UltraGridCell yCell = (UltraGridCell)y;

            if (xCell.Value == System.DBNull.Value || yCell.Value == System.DBNull.Value)
            {
                if (yCell.Value != System.DBNull.Value)
                {
                    return -1;
                }
                if (xCell.Value != System.DBNull.Value)
                {
                    return +1;
                }
                return 0;
            }

            double xValue = Convert.ToDouble(xCell.Value);
            double yValue = Convert.ToDouble(yCell.Value);
            if (xValue < yValue)
            {
                return -1;
            }
            if (xValue > yValue)
            {
                return +1;
            }

            return 0;
        }
    }

    public class UltraGridDateComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {

            UltraGridCell xCell = (UltraGridCell)x;
            UltraGridCell yCell = (UltraGridCell)y;

            if (xCell.Value == System.DBNull.Value || yCell.Value == System.DBNull.Value)
            {
                if (yCell.Value != System.DBNull.Value)
                {
                    return -1;
                }
                if (xCell.Value != System.DBNull.Value)
                {
                    return +1;
                }
                return 0;
            }

            DateTime xValue = Convert.ToDateTime(xCell.Value);
            DateTime yValue = Convert.ToDateTime(yCell.Value);
            if (xValue < yValue)
            {
                return -1;
            }
            if (xValue > yValue)
            {
                return +1;
            }

            return 0;
        }
    }
    // End TT#1930-MD - JSmith - Sorting in Store Profiles for $, Number, and Date incorrect and appearance in Store Characteristics is  not in any order.
}
