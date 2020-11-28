using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using MIDRetail.DataCommon;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace MIDRetail.Windows.Controls
{
    public partial class PlanChainLadderGridControl : UserControl
    {
        
        private bool _useTheme = true; //tells the control to either use themes (fonts and colors) or use hardcoded fonts and colors

        private bool _firstTime = true;
        private int _variableCount = 0;
        private int _numQtys = 0;
        private int _numHiddenCols = 4;
        private int _rowKeyDisplayWidth = 0;
        private int _originalRowKeyDisplayWidth = 0;
        private bool _initializing;
        private string _timeTotalName;
      
        public PlanChainLadderGridControl()
        {
            InitializeComponent();
        }


        public Infragistics.Win.UltraWinGrid.UltraGrid grid
        {
            get { return this.ugGrid; }
        }

        public void BindGrid(DataSet aDataSet)
        {
            try
            {
                ugGrid.DataSource = null;

                string datamember = "";
                int maxBandDepth = 0;
                foreach (string member in aDataSet.ExtendedProperties.Keys)
                {
                    datamember = member;
                    maxBandDepth = (int)aDataSet.ExtendedProperties[member];
                }


                BindingSource bsChainLadder = new BindingSource(aDataSet, datamember);
                this.ugGrid.DisplayLayout.MaxBandDepth = maxBandDepth;
                this.ugGrid.DataSource = bsChainLadder;
            }
            catch
            {
                throw;
            }
        }
        public void UpdateTotalDataset(DataSet aDataSet)
        {
            ugTotal.DataSource = null;
            string datamember = "";
            foreach (string member in aDataSet.ExtendedProperties.Keys)
            {
                datamember = member;
            }
            DataTable dtTotal = aDataSet.Tables[datamember].Copy();
            for (int i = dtTotal.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dtTotal.Rows[i];
                if (!row["RowKey"].ToString().StartsWith(_timeTotalName))
                {
                    dtTotal.Rows.Remove(row);
                }
            }
            this.ugTotal.DataSource = dtTotal;
        }

        public void ApplyGridProperties()
        {
            this.ugGrid.DisplayLayout.Bands[0].Override.RowSizing = RowSizing.AutoFree;
            this.ugGrid.DisplayLayout.Bands[0].Override.CellMultiLine = DefaultableBoolean.True;
          
            this.ugGrid.Rows.FixedRows.RefreshSort();

            foreach (UltraGridBand band in this.ugGrid.DisplayLayout.Bands)
            {
                foreach (UltraGridColumn column in band.Columns)
                {
                    column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                    if (column.Key.EndsWith("~ADJ"))
                    {
                        if (column.Width < 50)
                        {
                            column.Width = 50;
                        }
                    }
                }
            }
            
            ApplyTotalGridProperties();
            AdjustGridColumns(true);
            this.ugGrid.ActiveRow = null;
            this.ugTotal.ActiveRow = null;
            this.BeginInvoke(new MethodInvoker(this.ResetSplitterLocation));  

            _initializing = false;
        }

        public void ApplyTotalGridProperties()
        {
            
            this.ugTotal.DisplayLayout.Bands[0].Override.RowSizing = RowSizing.AutoFree;
            this.ugTotal.DisplayLayout.Bands[0].Override.CellMultiLine = DefaultableBoolean.True;

            //this.ugTotal.Rows.FixedRows.RefreshSort();
            foreach (UltraGridRow row in this.ugTotal.Rows)
            {
                if (row.Cells["RowIndex"].Value.ToString() == "-1")
                {
                    foreach (UltraGridCell cell in row.Cells)
                    {
                        if (!cell.Column.Hidden && cell.Column.Key != "RowKeyDisplay")
                        {
                            cell.Appearance = ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"];
                        }
                    }
                }
            }
            foreach (UltraGridBand band in this.ugTotal.DisplayLayout.Bands)
            {
                foreach (UltraGridColumn column in band.Columns)
                {
                    column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                }
            }
        }

        public delegate bool IsCellNewValueValidDelegate(int rowIndex, int columnIndex, object newValue);
        private IsCellNewValueValidDelegate _isCellNewValueValid;
        private IsChainLadderCellValueNegative _isCubeCellValueNegative;
        private IsChainLadderCellIneligible _isCubeCellIneligible;
        private IsChainLadderCellLocked _isCubeCellLocked;
        private IsChainLadderCellEditable _isCubeCellEditable;
        private IsChainLadderCellBasis _isCubeCellBasis;
        //private GetCellDisplayFormattedValue _getCellDisplayFormattedValue;

        private Bitmap _picLock;
        private string _windowName;
        private string _invalidDataMsg;
        //private Theme _theme;
       
        private FontData defaultFontData;

        private Color negativeValueForeColor;
        private FontData lockedFontData;

        private Color editableBackColor;
        private Color editableForeColor;
        private FontData editableFontData;

        private Color basisCellBackColor; //will be displayed when not edtiable and basis
        private Color basisCellForeColor; //will be displayed when not edtiable and basis
        private FontData basisCellFontData; //will be displayed when not edtiable and basis

        private Color nonbasisCellBackColor;  //will be displayed when not edtiable and not basis
        private Color nonbasisCellForeColor; //will be displayed when not edtiable and not basis
        private FontData nonbasisCellFontData; //will be displayed when not edtiable and not basis

        private Color groupAndRowBackColor;
        private Color groupAndRowForeColor;
        private FontData groupAndRowFontData;

        private Color nodeTitleBackColor;
        private Color nodeTitleForeColor;
        private FontData nodeTitleFontData;

        private Color variableColumnBackColor;
        private Color variableColumnForeColor;
        private Color variableColumnBorderColor;
        private FontData variableColumnFontData;

        private Color fixedRowTotalBackColor;
        private Color fixedRowTotalForeColor;
        private FontData fixedRowTotalFontData;

        private Color activeCellBackColor;
        private Color activeCellForeColor;

        private Color editingCellBorderColor = Color.Black;
        private Color editingCellBackColor = Color.White;
        private Color editingCellForeColor = Color.Black;

        private const int cellPadding = 2;

        //private Color variableValueBackColor;

        public void ApplyTheme(Theme theme)
        {
            //apply the theme colors and fonts
            defaultFontData = ugGrid.DisplayLayout.Appearance.FontData;  //just picking the default font data from the grid
            if (_useTheme)
            {
                negativeValueForeColor = theme.NegativeForeColor; // Color.DarkRed
                lockedFontData = ApplyFontToFontData(theme.LockedFont);

                editableBackColor = theme.StoreDetailBackColor; //aka Store Plan Data - corresponds to Color.Wheat
                editableForeColor = theme.StoreDetailForeColor; //Color.Black
                editableFontData = ApplyFontToFontData(theme.EditableFont); //Italic

                nonbasisCellBackColor = theme.StoreDetailAlternateBackColor; //Color.Gray
                nonbasisCellForeColor = theme.StoreDetailForeColor; //Color.Black
                nonbasisCellFontData = ApplyFontToFontData(theme.DisplayOnlyFont);

                basisCellBackColor = theme.StoreSetBackColor; //Color.LightGray
                basisCellForeColor = theme.StoreSetForeColor; //Color.Black
                basisCellFontData = ApplyFontToFontData(theme.DisplayOnlyFont);

                groupAndRowBackColor = theme.ColumnGroupHeaderBackColor; //Color.Brown
                groupAndRowForeColor = theme.ColumnGroupHeaderForeColor; //Color.White
                groupAndRowFontData = ApplyFontToFontData(theme.ColumnGroupHeaderFont); //Bold

                nodeTitleBackColor = theme.NodeDescriptionBackColor; //Color.Black
                nodeTitleForeColor = theme.NodeDescriptionForeColor; //Color.White
                nodeTitleFontData = ApplyFontToFontData(theme.NodeDescriptionFont); //Bold

                variableColumnBackColor = theme.ColumnHeaderBackColor; //Color.Black
                variableColumnForeColor = theme.ColumnHeaderForeColor; //Color.White
                variableColumnBorderColor = theme.ColumnGroupDividerColor; //Color.White
                variableColumnFontData = ApplyFontToFontData(theme.ColumnHeaderFont); //Bold

                fixedRowTotalBackColor = theme.StoreDetailRowHeaderBackColor; //Color.DarkGray
                fixedRowTotalForeColor = theme.StoreDetailRowHeaderForeColor; //Color.White
                fixedRowTotalFontData = ApplyFontToFontData(theme.RowHeaderFont); //Bold

                activeCellBackColor = theme.StoreTotalBackColor; //Color.LightGray
                activeCellForeColor = theme.StoreTotalForeColor; //Color.Black

                this.ugGrid.DrawFilter = new GridDrawFilter(_numHiddenCols, _numQtys, theme);     // for vertical grid lines
                this.ugTotal.DrawFilter = new GridDrawFilter(_numHiddenCols, _numQtys, theme);
            }
            else
            {
                negativeValueForeColor = Color.DarkRed;
                lockedFontData = defaultFontData;

                editableBackColor = Color.Wheat;
                editableForeColor = Color.Black;
                editableFontData = defaultFontData;
                editableFontData.Italic = DefaultableBoolean.True;

                nonbasisCellBackColor = Color.Gray;
                nonbasisCellForeColor = Color.Black;
                nonbasisCellFontData = defaultFontData;

                basisCellBackColor = Color.LightGray;
                basisCellForeColor = Color.Black;
                basisCellFontData = defaultFontData;

                groupAndRowBackColor = Color.Brown;
                groupAndRowForeColor = Color.White;
                groupAndRowFontData = defaultFontData;
                groupAndRowFontData.Bold = DefaultableBoolean.True;

                nodeTitleBackColor = Color.Black;
                nodeTitleForeColor = Color.White;
                nodeTitleFontData = defaultFontData;
                nodeTitleFontData.Bold = DefaultableBoolean.True;

                variableColumnBackColor = Color.Black;
                variableColumnForeColor = Color.White;
                variableColumnBorderColor = Color.White;
                variableColumnFontData = defaultFontData;
                variableColumnFontData.Bold = DefaultableBoolean.True;

                fixedRowTotalBackColor = Color.DarkGray;
                fixedRowTotalForeColor = Color.White;
                fixedRowTotalFontData = defaultFontData;
                fixedRowTotalFontData.Bold = DefaultableBoolean.True;

                activeCellBackColor = Color.LightGray;
                activeCellForeColor = Color.Black;
            }
        }

        public void SetElementsToTheme()
        {
            txtBoxTitle.BackColor = nodeTitleBackColor;//Color.Black;
            txtBoxTitle.ForeColor = nodeTitleForeColor; //Color.White;
            FontStyle titleFontStyle = new FontStyle();
            if (nodeTitleFontData.Bold == DefaultableBoolean.True)
            {
                titleFontStyle = FontStyle.Bold;
            }
            if (nodeTitleFontData.Italic == DefaultableBoolean.True)
            {
                titleFontStyle = FontStyle.Italic;
            }
            if (nodeTitleFontData.Underline == DefaultableBoolean.True)
            {
                titleFontStyle = FontStyle.Underline;
            }

            Font titleFont = new Font(nodeTitleFontData.Name, nodeTitleFontData.SizeInPoints, titleFontStyle); //new Font(txtBoxTitle.Font, FontStyle.Bold);



            txtBoxTitle.Font = titleFont;

            



            //ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].TextHAlign = HAlign.Left;
            //ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].BackColor = gridCaptionBackColor; // Color.Black;
            //ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].ForeColor = gridCaptionForeColor; // Color.White;
            ////ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].FontData.Bold = DefaultableBoolean.True;
            //SetFontData(ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].FontData, gridCaptionFontData);
            //ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"].BackGradientStyle = GradientStyle.None;

            ugGrid.DisplayLayout.Appearances["timeValueAppearance"].BackColor = groupAndRowBackColor;  //Color.Brown;
            ugGrid.DisplayLayout.Appearances["timeValueAppearance"].ForeColor = groupAndRowForeColor; //Color.White;
       
            SetFontData(ugGrid.DisplayLayout.Appearances["timeValueAppearance"].FontData, groupAndRowFontData);

            ugGrid.DisplayLayout.Appearances["variableValueAppearance"].BackColor = variableColumnBackColor; //Color.Black;
            ugGrid.DisplayLayout.Appearances["variableValueAppearance"].ForeColor = variableColumnForeColor; //Color.White;
            //ugGrid.DisplayLayout.Appearances["variableValueAppearance"].FontData.Bold = DefaultableBoolean.True;
            SetFontData(ugGrid.DisplayLayout.Appearances["variableValueAppearance"].FontData, variableColumnFontData);
            ugGrid.DisplayLayout.Appearances["variableValueAppearance"].BackGradientStyle = GradientStyle.None;

            ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"].BackColor = fixedRowTotalBackColor; //Color.DarkGray;
            ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"].ForeColor = fixedRowTotalForeColor; //Color.White;
            //ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"].FontData.Bold = DefaultableBoolean.True;
            SetFontData(ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"].FontData, fixedRowTotalFontData);

            ugGrid.DisplayLayout.Appearances["emptySpaceAppearance"].BackGradientStyle = GradientStyle.None;
            ugGrid.DisplayLayout.Appearances["emptySpaceAppearance"].BorderColor = Color.Transparent;

            //ugGrid.DisplayLayout.CaptionAppearance = ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"];

            ugGrid.DisplayLayout.Override.ActiveRowAppearance.Reset();
            ugGrid.DisplayLayout.Override.ActiveCellAppearance.BackColor = activeCellBackColor; //Color.LightGray;
            ugGrid.DisplayLayout.Override.ActiveCellAppearance.ForeColor = activeCellForeColor; //Color.Black;

            ugTotal.DisplayLayout.Override.ActiveRowAppearance.Reset();
            ugTotal.DisplayLayout.Override.ActiveCellAppearance.BackColor = activeCellBackColor; //Color.LightGray;
            ugTotal.DisplayLayout.Override.ActiveCellAppearance.ForeColor = activeCellForeColor; //Color.Black;

            ugGrid.DisplayLayout.Override.EditCellAppearance.BorderColor = editingCellBorderColor; // Color.Black;
            ugGrid.DisplayLayout.Override.EditCellAppearance.BackColor = editingCellBackColor; //Color.White;
            ugGrid.DisplayLayout.Override.EditCellAppearance.ForeColor = editingCellForeColor; //Color.Black;

            ugTotal.DisplayLayout.Override.EditCellAppearance.BorderColor = editingCellBorderColor; // Color.Black;
            ugTotal.DisplayLayout.Override.EditCellAppearance.BackColor = editingCellBackColor; //Color.White;
            ugTotal.DisplayLayout.Override.EditCellAppearance.ForeColor = editingCellForeColor; //Color.Black;

            foreach (UltraGridBand band in ugGrid.DisplayLayout.Bands)
            {



                foreach (UltraGridColumn column in band.Columns)
                {
                    if (!column.Hidden && column.Key != "RowKeyDisplay")
                    {
                        column.Header.Appearance.BackColor = variableColumnBackColor; //variableValueBackColor;
                        column.Header.Appearance.ForeColor = variableColumnForeColor;
                        column.Header.Appearance.BorderColor = variableColumnBorderColor; // Color.White;
                    }
                }

                if (band.Index == 0)
                {
                    foreach (UltraGridGroup group in band.Groups)
                    {
                        if (group.Index != 0) //do not set the first group since it is in the period column
                        {
                            group.Header.Appearance.BackColor = groupAndRowBackColor; //= ugGrid.DisplayLayout.Appearances["timeValueAppearance"];
                            group.Header.Appearance.ForeColor = groupAndRowForeColor;
                            SetFontData(group.Header.Appearance.FontData, groupAndRowFontData);
                            group.Header.Appearance.TextHAlign = HAlign.Center;
                            group.Header.Appearance.BackGradientStyle = GradientStyle.None;
                        }
                    }
                }
            }

        }
        
        public void Initialize(IsCellNewValueValidDelegate isCellNewValueValid,
                               IsChainLadderCellValueNegative isCubeCellValueNegative,
                               IsChainLadderCellIneligible isCubeCellIneligible,
                               IsChainLadderCellLocked isCubeCellLocked,
                               IsChainLadderCellEditable isCubeCellEditable,
                               IsChainLadderCellBasis isCubeCellBasis,
                               //GetCellDisplayFormattedValue getCellDisplayFormattedValue,
                               string windowName,
                               string invalidDataMsg,
                               string aGridHeaderDescription, string aTimeTotalName, Theme theme)
        {

            _isCellNewValueValid = isCellNewValueValid;
            _isCubeCellValueNegative = isCubeCellValueNegative;
            _isCubeCellIneligible = isCubeCellIneligible;
            _isCubeCellLocked = isCubeCellLocked;
            _isCubeCellEditable = isCubeCellEditable;
            _isCubeCellBasis = isCubeCellBasis;
            //_getCellDisplayFormattedValue = getCellDisplayFormattedValue;

            _windowName = windowName;
            _invalidDataMsg = invalidDataMsg;
            //_theme = theme;
            _timeTotalName = aTimeTotalName;

            ugGrid.DisplayLayout.Appearances.Add("timeValueAppearance");
            ugGrid.DisplayLayout.Appearances.Add("variableValueAppearance");
            ugGrid.DisplayLayout.Appearances.Add("fixedRowLabelAppearance");
            ugGrid.DisplayLayout.Appearances.Add("emptySpaceAppearance");



          

            
            txtBoxTitle.Text = aGridHeaderDescription;
            txtBoxTitle.ReadOnly = true;

            _picLock = new Bitmap(MIDGraphics.MIDGraphicsDir + "\\lock.gif");

            ugGrid.Text = aGridHeaderDescription;
            //ugGrid.DisplayLayout.CaptionVisible = DefaultableBoolean.True; // Moved the title to Band[0] Header
            ugGrid.DisplayLayout.ViewStyleBand = ViewStyleBand.Vertical;

            //Frozen Column settings
            ugGrid.DisplayLayout.UseFixedHeaders = true;
            ugGrid.DisplayLayout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None;

            // Disable alpha-blending which may increase performance.
            //ugGrid.AlphaBlendMode = AlphaBlendMode.Optimized;
            //ugGrid.UseOsThemes = DefaultableBoolean.False;;

            // Set the update mode which dictates when the UltraGrid calls EndEdit
            // on IEditableObject row objects.
            //ugGrid.UpdateMode = UpdateMode.OnCellChangeOrLostFocus;

            // Set the scroll style to Immediate so that the UltraGrid scrolls the rows as
            // the vertical scroll bar thumb is dragged. Normally the UltraGrid defers the
            // scrolling until the thumb is released and displays scroll tips instead.
            ugGrid.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;

            //ugGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;


            //set the appearances of the grid one time
            ugGrid.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
            ugGrid.DisplayLayout.Override.AllowDelete = DefaultableBoolean.False;
            //ugGrid.DisplayLayout.Override.AllowRowFiltering = DefaultableBoolean.True;
            ugGrid.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.True;

            ugGrid.StyleLibraryName = "ChainLadderLibraryName1";
            ugTotal.StyleLibraryName = "ChainLadderLibraryName1";
            this.ugGrid.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDIPlus;
            this.ugTotal.TextRenderingMode = Infragistics.Win.TextRenderingMode.GDIPlus;
            ApplyTheme(theme);
            //SetElementsToTheme();
          

        }
        private FontData ApplyFontToFontData(Font aFont)
        {
            FontData f = new FontData();
            f.Name = aFont.Name;
            if (aFont.Bold)
            {
                f.Bold = DefaultableBoolean.True;
            }
            else
            {
                f.Bold = DefaultableBoolean.False;
            }
            if (aFont.Italic)
            {
                f.Italic = DefaultableBoolean.True;
            }
            else
            {
                f.Italic = DefaultableBoolean.False;
            }
            if (aFont.Underline)
            {
                f.Underline = DefaultableBoolean.True;
            }
            else
            {
                f.Underline = DefaultableBoolean.False;
            }
            if (aFont.Strikeout)
            {
                f.Strikeout = DefaultableBoolean.True;
            }
            else
            {
                f.Strikeout = DefaultableBoolean.False;
            }
            f.SizeInPoints = aFont.SizeInPoints;

            return f;
        }
        private void SetFontData(FontData target, FontData source)
        {
            target.Name = source.Name;
            target.Bold = source.Bold;
            target.Italic = source.Italic; 
            target.Underline = source.Underline;
            target.Strikeout = source.Strikeout;
            target.SizeInPoints = source.SizeInPoints;
        }

        public void AdjustGridColumns(bool bResetOrigColWidth)
        {
           // Adjust 1st column 
            int lastBandIndex = this.ugGrid.DisplayLayout.Bands.Count - 1;
            UltraGridBand band = this.ugGrid.DisplayLayout.Bands[lastBandIndex];
            
            UltraGridColumn col = band.Columns["RowKeyDisplay"];
            if (bResetOrigColWidth)
            {
                _originalRowKeyDisplayWidth = col.Width;
            }

            this.ugGrid.DisplayLayout.Bands[0].Columns["RowKeyDisplay"].Width = col.Width + (band.Indentation * lastBandIndex);

            if (lastBandIndex > 0)
            {
                this.ugTotal.DisplayLayout.Bands[0].Indentation = 19;
            }
            else
            {
                this.ugTotal.DisplayLayout.Bands[0].Indentation = this.ugGrid.DisplayLayout.Bands[0].Indentation;
            }
            this.ugTotal.DisplayLayout.Bands[0].Columns["RowKeyDisplay"].Width = this.ugGrid.DisplayLayout.Bands[0].Columns["RowKeyDisplay"].Width;
                                                                                 
            int bandWidth = 0;
            foreach (UltraGridColumn column in this.ugGrid.DisplayLayout.Bands[0].Columns)
            {
                if (!column.Hidden && column.Key != "RowKeyDisplay")
                {
                    if (column.Group.Columns.Count == 1)  // group header may be wider than single column so finagle the caption and resize
                    {
                        FontData fontData = column.Header.Appearance.FontData;
                        SetFontData(column.Header.Appearance.FontData, groupAndRowFontData);
                        column.Header.Caption = column.Group.Header.Caption;
                        column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All);
                        column.Header.Caption = string.Empty;
                        SetFontData(column.Header.Appearance.FontData, fontData);
                    }
                    if (this.ugTotal.DisplayLayout.Bands[0].Columns[column.Index].Width > column.Width)
                    {
                        column.Width = this.ugTotal.DisplayLayout.Bands[0].Columns[column.Index].Width;
                    }
                    else
                    {
                        this.ugTotal.DisplayLayout.Bands[0].Columns[column.Index].Width = column.Width;
                    }
                    bandWidth += column.Width;
                    for (int i = 1; i < this.ugGrid.DisplayLayout.Bands.Count; i++)
                    {
                        UltraGridBand bandLower = this.ugGrid.DisplayLayout.Bands[i];
                        bandLower.Columns[column.Index].Width = column.Width;
                    }
                   
                }
            }

            int rowExpanderPadding = (this.ugGrid.DisplayLayout.Bands.Count > 1) ? 19 : 0;
            txtBoxTitle.Height = panelTop.Height;
            txtBoxTitle.Width = bandWidth + 1;
            txtBoxTitle.Location = new System.Drawing.Point(this.ugGrid.DisplayLayout.Bands[0].Columns["RowKeyDisplay"].Width + rowExpanderPadding, txtBoxTitle.Location.Y);
        }

        public void ResetSplitterLocation()
        {
            int height = 0;
            foreach (UltraGridRow row in ugTotal.Rows)
            {
                height += row.Height;
            }
            splitContainer.SplitterDistance = splitContainer.Height - height; 
        }

        public void AddGridProperties()
        {
            this.ugGrid.DisplayLayout.RowConnectorStyle = RowConnectorStyle.None;
            this.ugGrid.DisplayLayout.InterBandSpacing = 0;
            this.ugGrid.DisplayLayout.Override.RowSpacingBefore = 0;
            this.ugGrid.DisplayLayout.Override.RowSpacingAfter = 0;
            //this.ugGrid.DisplayLayout.Override.DefaultColWidth = 100;   // This default was overriding wider autoresized width making it narrower than the text; commented out
            this.ugGrid.DisplayLayout.TabNavigation = TabNavigation.NextControlOnLastCell;
            this.ugGrid.DisplayLayout.Override.CellPadding = cellPadding;
            this.ugGrid.DisplayLayout.Override.ResetSelectedCellAppearance();
            this.ugTotal.DisplayLayout.Override.RowSpacingBefore = 0;
            this.ugTotal.DisplayLayout.Override.RowSpacingAfter = 0;
            this.ugTotal.DisplayLayout.Override.CellPadding = cellPadding;
            this.ugTotal.DisplayLayout.Override.ResetSelectedCellAppearance();
            SetElementsToTheme();
            ApplyGridProperties();

        }

        private void ugGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.HeaderPlacement = HeaderPlacement.FixedOnTop;
                //e.Layout.Override.BorderStyleRow = UIElementBorderStyle.Solid;
                e.Layout.Override.BorderStyleRow = UIElementBorderStyle.None;
                e.Layout.Override.BorderStyleCell = UIElementBorderStyle.Solid;
                //e.Layout.Override.SpecialRowSeparator = SpecialRowSeparator.FixedRows;
                //e.Layout.Override.SpecialRowSeparatorAppearance.BackColor = Color.Black;
                //e.Layout.Override.BorderStyleSpecialRowSeparator = UIElementBorderStyle.RaisedSoft;
                //e.Layout.Override.FixedRowSortOrder = FixedRowSortOrder.Sorted;
                //e.Layout.Override.FixedRowStyle = FixedRowStyle.Bottom;
            
                //e.Layout.Override.FixedHeaderAppearance.BackColor = Color.SkyBlue;
                e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;

                //e.Layout.Override.BorderStyleRow = UIElementBorderStyle.None;
                //e.Layout.Override.BorderStyleCell = UIElementBorderStyle.None;

                e.Layout.Scrollbars = Scrollbars.Vertical;
                _initializing = true;

                //set all tags first
                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    int cubeColumnIndex = 0;  //store in tag to allow dynamic switching of columns
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        column.Tag = cubeColumnIndex;
                        cubeColumnIndex++;
                    }
                }

       
                string[] colKeySplit;
                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    band.Columns["ParentRowKey"].Hidden = true;
                    band.Columns["RowIndex"].Hidden = true;
                    band.Columns["RowKey"].Hidden = true;
                    band.Columns["RowSortIndex"].Hidden = true;
                    band.Columns["RowKeyDisplay"].CellAppearance = ugGrid.DisplayLayout.Appearances["timeValueAppearance"];
                     
                    if (band.Index == 0)
                    {
                        //band.ColHeaderLines  = 3; // comment out; let headers auto size
                        band.Override.WrapHeaderText = DefaultableBoolean.True; // allows headers to readjust the height
                    }
                    else
                    {
                        band.ColHeadersVisible = false;
                        band.Indentation = 15;
                    }
                    
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        if (column.Key == "RowKeyDisplay")
                        {
                            
                            column.Header.Caption = string.Empty;

                            column.Header.Appearance = ugGrid.DisplayLayout.Appearances["emptySpaceAppearance"];
                            column.Header.Fixed = true;
                        }
                        else if (!column.Hidden)
                        {
                            colKeySplit = column.Key.Split(new char[] { '~' });
                            if (colKeySplit.Length > 1)
                            {
                                column.Header.Caption = colKeySplit[colKeySplit.Length - 1];
                                if (_firstTime && band.Index == 0)
                                {
                                    _numQtys++;
                                }
                            }
                            else if (band.Index == 0)
                            {
                                _variableCount++;
                                if (column.Key.Contains("@@"))
                                {
                                    //column.Header.Caption = string.Empty;
                                }
                                else
                                {
                                    column.Header.Caption = string.Empty;
                                }
                                if (_firstTime) 
                                {
                                    if (_variableCount > 1)
                                    {
                                        _firstTime = false;
                                    }
                                    else
                                    {
                                        _numQtys++;
                                    }
                                }
                            }
                            column.Header.Appearance = ugGrid.DisplayLayout.Appearances["variableValueAppearance"];
                            //column.CellAppearance.BackColor = variableColumnBackColor; //variableValueBackColor;
                            //column.CellAppearance.BorderColor = variableColumnBorderColor; //Color.White;
                            column.CellAppearance.TextHAlign = HAlign.Right;
                         
                            //column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All); //RowKey column still has issues resizing.
                            //if (column.Key.EndsWith("~ADJ"))
                            //{
                            //    if (column.Width < 50)
                            //    {
                            //        column.Width = 50;
                            //    }
                            //}
                        }
                        string[] columnToolTipSplit = column.Header.Caption.Split(new string[] { "<TOOLTIP>" }, StringSplitOptions.RemoveEmptyEntries);
                        if (columnToolTipSplit.Length > 1)
                        {
                            column.Header.Caption = columnToolTipSplit[0];
                            column.Header.ToolTipText = columnToolTipSplit[columnToolTipSplit.Length -1];
                            column.TipStyleCell = TipStyle.Show;
                        }
                        else
                        {
                            column.Header.ToolTipText = string.Empty;
                            column.TipStyleCell = TipStyle.Hide;
                        }
                    }
                }
                int i = -1;
                UltraGridBand band0 = e.Layout.Bands[0];
                UltraGridGroup group;

                //band0.HeaderVisible = true;
                //band0.Header.Caption = this.ugGrid.Text;
                //band0.Header.Appearance = ugGrid.DisplayLayout.Appearances["gridCaptionAppearance"];

                foreach (UltraGridColumn column in band0.Columns)
                {
                    if (column.Key == "RowKeyDisplay")
                    {
                        i++;
                        group = new UltraGridGroup();
                        group = band0.Groups.Add("Group" + i.ToString(), string.Empty);
                        group.Columns.Add(column, column.Header.VisiblePosition, 0);
                        group.Header.Appearance = ugGrid.DisplayLayout.Appearances["emptySpaceAppearance"];
                        group.Header.Fixed = true;
                    }
                    else if (!column.Hidden)
                    {
                        colKeySplit = column.Key.Split(new char[] { '~' });

                        if (colKeySplit.Length == 1 && column.Key.Contains("@@") == false)
                        {
                            i++;
                            group = new UltraGridGroup();
                            //if (column.Key.Contains("@@"))
                            //{
                                //group = band0.Groups.Add("Group" + i.ToString(), column.Header.Caption);
                                
                            //}
                            //else
                            //{
                                group = band0.Groups.Add("Group" + i.ToString(), column.Key.ToString());
                            //}
                            
                            group.Columns.Add(column, column.Header.VisiblePosition, 0);
                            group.Header.Appearance = ugGrid.DisplayLayout.Appearances["timeValueAppearance"];
                            group.Header.Appearance.TextHAlign = HAlign.Center;
                            group.Header.Appearance.BackGradientStyle = GradientStyle.None;
                        }
                        else
                        {
                            group = band0.Groups["Group" + i.ToString()];
                            group.Columns.Add(column, column.Header.VisiblePosition, 0);
                        }
                    }
                }
               
                e.Layout.Bands[0].SortedColumns.Add("RowSortIndex", false, false);                
            }
            catch
            {
                throw;
            }
        }
        
        private void ugGrid_InitializeRow(object sender, InitializeRowEventArgs e)
        {
             
            e.Row.ExpandAll();
            if (e.Row.Cells["RowKey"].Value.ToString().StartsWith(_timeTotalName))
            {
                UltraGrid grid = (UltraGrid)sender;

                if (grid.Name == "ugGrid")
                {
                    e.Row.Hidden = true;
                }
                //else
                //{
                //    e.Row.Fixed = true;
                //}
            }
            int cubeRowIndex = GetCellCubeRowIndex(e.Row.Cells[0]);
            //if (cubeRowIndex != -1) //ignore total label rows
            //{
                foreach (Infragistics.Win.UltraWinGrid.UltraGridCell c in e.Row.Cells)
                {
                    if (c.Column.Key == "RowKeyDisplay")  
                    {
                        c.Activation = Activation.Disabled;

                        c.Appearance.BackColor = groupAndRowBackColor;
                        c.Appearance.ForeColor = groupAndRowForeColor;

                        //c.Appearance.FontData.Bold = DefaultableBoolean.True;
                        c.Appearance.BackColorDisabled = groupAndRowBackColor;
                        c.Appearance.ForeColorDisabled = groupAndRowForeColor; //Color.White;
                        SetFontData(c.Appearance.FontData, groupAndRowFontData);
                    }
                    //if (c.Column.Key != "RowKeyDisplay") //ignore period column
                    else
                    {
                        int cubeColumnIndex = GetCellCubeColumnIndex(c);

                        //double dval;
                        //if (double.TryParse(c.Value.ToString(), out dval))
                        //{
                        //    c.Value = _getCellDisplayFormattedValue(cubeRowIndex, cubeColumnIndex, dval);
                        //}

                        if (_isCubeCellEditable(cubeRowIndex, cubeColumnIndex) == true)
                        {
                            c.Activation = Activation.AllowEdit;
                            c.Appearance.BackColor = editableBackColor;
                            c.Appearance.ForeColor = editableForeColor;
                            //c.Appearance.FontData.Italic = DefaultableBoolean.True;
                            SetFontData(c.Appearance.FontData, editableFontData);
                        }
                        else
                        {
                            c.Activation = Activation.NoEdit;
                             
                           

                            if (_isCubeCellBasis(cubeRowIndex, cubeColumnIndex) == true)
                            {

                                c.Appearance.BackColor = basisCellBackColor;
                                c.Appearance.ForeColor = basisCellForeColor;
                                SetFontData(c.Appearance.FontData, basisCellFontData);
                            }
                            else
                            {
                                if (cubeRowIndex == -1 && !c.Column.Hidden)
                                {
                                    c.Appearance = this.ugGrid.DisplayLayout.Appearances["fixedRowLabelAppearance"];
                                }
                                else
                                {
                                    //non-basis non-edit
                                    c.Appearance.BackColor = nonbasisCellBackColor;
                                    c.Appearance.ForeColor = nonbasisCellForeColor;
                                    SetFontData(c.Appearance.FontData, defaultFontData);
                                }
                            }
                           
                      
                        }

                        if (_isCubeCellValueNegative(cubeRowIndex, cubeColumnIndex) == true)
                        {
                            c.Appearance.ForeColor = negativeValueForeColor;
                        }
                        //else
                        //{
                        //    if (e.Row.Fixed == false)
                        //    {
                        //        if (cubeRowIndex != -1) // skip the total label cells
                        //        {
                        //            c.Appearance.ResetForeColor();
                        //        }
                        //    }
                        //}

                        //if (_isCubeCellIneligible(cubeRowIndex, cubeColumnIndex) == true)
                        //{
                        //    c.Appearance.FontData.Italic = DefaultableBoolean.True;
                        //}

                        if (_isCubeCellLocked(cubeRowIndex, cubeColumnIndex) == true)
                        {
                            c.Activation = Activation.NoEdit;
                            //c.Appearance.FontData.Bold = DefaultableBoolean.True;
                            SetFontData(c.Appearance.FontData, lockedFontData);
                            //c.Appearance.FontData.Italic = DefaultableBoolean.True;
                            c.Appearance.Image = _picLock;
                            c.Appearance.ImageHAlign = HAlign.Right;
                        }
                        else if (cubeRowIndex == -1) // skip the total label cells
                        {
                        }
                        else
                        {
                            //c.Appearance.FontData.Bold = DefaultableBoolean.False;
                            c.Appearance.ResetImage();
                        }
                       

                        //double result;
                        //if (Double.TryParse(c.Value.ToString(), out result) == true)
                        //{
                        //    if (result < 0)
                        //    {
                        //        //set negative appearance
                        //        c.Appearance = ugGrid.DisplayLayout.Appearances["negativeValueAppearance"];
                        //    }
                        //}
                    }
                }
            //}
        }




        //private bool _resetingCellValue = false;
        //private void ugGrid_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
        //{
            
        //}

        //private void ugGrid_AfterCellUpdate(object sender, CellEventArgs e)
        //{
           
        //}
        private void ugGrid_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {

            // If the user is canceling the modifications (for example by hitting Escape 
            // key, then just return because the cell will revert to its original value
            // in this case and not commit the user's input.
            if (e.CancellingEditOperation)
                return;




            //if (_resetingCellValue == false)
            //{
                int cubeColumnIndex = GetCellCubeColumnIndex(ugGrid.ActiveCell);
                int cubeRowIndex = GetCellCubeRowIndex(ugGrid.ActiveCell);


                //exit edit mode if the provided value is blank
                string strValue = ugGrid.ActiveCell.Text;
                if (strValue == String.Empty)
                {
                    // If the UltraGrid must exit the edit mode, then cancel the
                    // cell update so the original value gets restored in the cell.
                    this.ugGrid.ActiveCell.CancelUpdate();
                    return;
                }

                if (_isCellNewValueValid(cubeRowIndex, cubeColumnIndex, strValue) == true)
                {

                  
                    //ugGrid.ActiveRow.Band.Index,
                    RaiseCellValueChangedEvent(cubeRowIndex, cubeColumnIndex, System.Convert.ToDouble(strValue, CultureInfo.CurrentUICulture));
                }
                else
                {
                    //MessageBox.Show("Invalid value.");
                    MessageBox.Show(_invalidDataMsg, _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //_resetingCellValue = true;
                    //e.Cell.CancelUpdate();
                    //e.Cell.Value = (string)e.Cell.OriginalValue;
                    //ugGrid.ActiveCell = e.Cell;

                    //if (ugGrid.ActiveCell.IsInEditMode == false)
                    //{
                    //    this.ugGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode);
                    //}
                    //ugGrid.Focus();


                    // If ForceExit is true, then the UltraGrid will exit the edit mode
                    // regardless of whether you cancel this event or not. ForceExit would
                    // be true for example when the UltraGrid is being disposed of and thus
                    // it can't stay in edit mode. In which case setting Cancel won't do
                    // any good so just cancel the update to revert the cell's value back
                    // to its original value.
                    if (e.ForceExit)
                    {
                        // If the UltraGrid must exit the edit mode, then cancel the
                        // cell update so the original value gets restored in the cell.
                        this.ugGrid.ActiveCell.CancelUpdate();
                        return;
                    }

                    // In normal circumstances where ForceExit is false, set Cancel to 
                    // true so the UltraGrid doesn't exit the edit mode.
                    e.Cancel = true;


                    //_resetingCellValue = false;

                }
            //}
        }

        public void GetSelectedCellCubeRowAndColumnIndex(out int cubeRowIndex, out int cubeColumnIndex)
        {
            cubeRowIndex = -1;
            cubeColumnIndex = -1;
            if (ugGrid.Selected.Cells.Count > 0)
            {
                cubeRowIndex = GetCellCubeRowIndex(ugGrid.Selected.Cells[0]);
                cubeColumnIndex = GetCellCubeColumnIndex(ugGrid.Selected.Cells[0]);
            }
            else
            {
                if (ugGrid.ActiveCell != null)
                {
                    cubeRowIndex = GetCellCubeRowIndex(ugGrid.ActiveCell);
                    cubeColumnIndex = GetCellCubeColumnIndex(ugGrid.ActiveCell);
                }
            }
        }

        private int GetCellCubeRowIndex(UltraGridCell c)
        {
            return System.Convert.ToInt32(c.Row.Cells["RowIndex"].Value, CultureInfo.CurrentUICulture);
        }

        private int GetCellCubeColumnIndex(UltraGridCell c)
        {
            return (int)c.Column.Tag;
        }
        public void ReformatRows()
        {
            this.ReformatRowsHelper(this.ugGrid.Rows);
            this.ReformatRowsHelper(this.ugTotal.Rows);

        }
        private void ReformatRowsHelper(RowsCollection rows)
        {

            // Loop through every row in the passed in rows collection.
            foreach (UltraGridRow row in rows)
            {
                // If you are using Outlook GroupBy feature and have grouped rows by columns in the
                // UltraGrid, then rows collection can contain group-by rows or regular rows. So you 
                // may need to have code to handle group-by rows as well.
                //if (row is UltraGridGroupByRow)
                //{
                //    UltraGridGroupByRow groupByRow = (UltraGridGroupByRow)row;
                //   // groupByRow.Refresh(RefreshRow.FireInitializeRow);
                //}
                //else
                //{
                //    //row.Refresh(RefreshRow.FireInitializeRow);
                //    FormatRowCells(row);
                //}
                //FormatRowCells(row);
                row.Refresh(RefreshRow.FireInitializeRow);

                // If the row has any child rows. Typically, there is only a single child band. However,
                // there will be multiple child bands if the band associated with row1 has mupliple child
                // bands. This would be the case for exmple when you have a database hierarchy in which a
                // table has multiple child tables.
                if (null != row.ChildBands)
                {
                    // Loop throgh each of the child bands.
                    foreach (UltraGridChildBand childBand in row.ChildBands)
                    {
                        // Call this method recursivedly for each child rows collection.
                        this.ReformatRowsHelper(childBand.Rows);
                    }
                }
            }

        }


       
 
      
        //private void ugGrid_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        //{
        //    bool isColumnFrozen;
        //    if (e.Type == typeof(UltraGridColumn))
        //    {
        //        if (this.ugGrid.Selected.Columns.Count > 0)
        //        {
        //            if (this.ugGrid.Selected.Columns[0].Fixed == true)
        //            {
        //                isColumnFrozen = true;
        //            }
        //            else
        //            {
        //                isColumnFrozen = false;
        //            }

        //            RaiseSelectedCellChangedEvent(isColumnFrozen);
        //        }

        //    }
        //    else if (e.Type == typeof(UltraGridCell))
        //    {
        //        if (this.ugGrid.Selected.Cells.Count > 0)
        //        {
        //            if (this.ugGrid.Selected.Cells[0].Column.Header.Fixed == true)
        //            {
        //                isColumnFrozen = true;
        //            }
        //            else
        //            {
        //                isColumnFrozen = false;
        //            }

        //            RaiseSelectedCellChangedEvent(isColumnFrozen);
        //        }
        //    }	

        //}


        public void SetFreezeColumn(bool blnFreeze)
        {
            //always unfreeze all columns first
            for (int bandIndex = 0; bandIndex < ugGrid.DisplayLayout.Bands.Count; bandIndex++)
            {
                if (bandIndex == 0)
                {
                    for (int gIndex = 1; gIndex < this.ugGrid.DisplayLayout.Bands[0].Groups.Count; gIndex++)
                    {
                        this.ugGrid.DisplayLayout.Bands[bandIndex].Groups[gIndex].Header.Fixed = false;
                    }
                }
                else
                {
                    for (int columnIndex = 0; columnIndex < this.ugGrid.DisplayLayout.Bands[bandIndex].Columns.Count; columnIndex++)
                    {
                        if (this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Key != "RowKeyDisplay")
                        {
                            this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Header.Fixed = false;
                        }
                    }
                }
            }
            for (int columnIndex = 0; columnIndex < this.ugTotal.DisplayLayout.Bands[0].Columns.Count; columnIndex++)
            {
                if (this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Key != "RowKeyDisplay")
                {
                    this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Header.Fixed = false;
                }
            }

            if (blnFreeze == true)
            {
                //freeze columns if necessary

                int lastFrozenColumnIndex = -1;
                int lastGroupIndex = 0;
                int lastGroupColIndex;

                if (this.ugGrid.Selected.Columns.Count > 0)
                {
                    // All visble columns have a group which ignore the column header Fixed property; Group Fixed freezes all the columns in the group so we have to
                    // find the last column in the group to appyly to the other bands.
                    //lastFrozenColumnIndex = this.ugGrid.Selected.Columns[this.ugGrid.Selected.Columns.Count - 1].Column.Index;
                    lastGroupIndex = this.ugGrid.Selected.Columns[this.ugGrid.Selected.Columns.Count - 1].Column.Group.Index;
                    lastGroupColIndex = this.ugGrid.Selected.Columns[this.ugGrid.Selected.Columns.Count - 1].Column.Group.Columns.Count - 1;
                    lastFrozenColumnIndex = this.ugGrid.Selected.Columns[this.ugGrid.Selected.Columns.Count - 1].Column.Group.Columns[lastGroupColIndex].Index;
                     
                }
                else if (this.ugGrid.Selected.Cells.Count > 0)
                {
                    //lastFrozenColumnIndex = this.ugGrid.Selected.Cells[0].Column.Index;
                    int colIndex = this.ugGrid.Selected.Cells[0].Column.Index;
                    lastGroupIndex = this.ugGrid.DisplayLayout.Bands[0].Columns[colIndex].Group.Index; 
                    lastGroupColIndex = this.ugGrid.DisplayLayout.Bands[0].Columns[colIndex].Group.Columns.Count - 1;
                    lastFrozenColumnIndex = this.ugGrid.DisplayLayout.Bands[0].Columns[colIndex].Group.Columns[lastGroupColIndex].Index;
                }

                if (lastFrozenColumnIndex != -1)
                {
                    for (int bandIndex = 0; bandIndex < ugGrid.DisplayLayout.Bands.Count; bandIndex++)
                    {
                        //for (int columnIndex = 0; columnIndex <= lastFrozenColumnIndex; columnIndex++)
                        //{
                        //    if (!this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Hidden)
                        //    {
                        //        if (bandIndex == 0)
                        //        {
                        //            this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Group.Header.Fixed = blnFreeze;
                        //        }
                        //        else
                        //        {
                        //            this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Header.Fixed = blnFreeze;
                        //        }
                        //    }
                        //}
                        if (bandIndex == 0)
                        {
                            for (int gIndex = 1; gIndex <= lastGroupIndex; gIndex++)
                            {
                                this.ugGrid.DisplayLayout.Bands[bandIndex].Groups[gIndex].Header.Fixed = blnFreeze;
                            }
                        }
                        else
                        {
                            for (int columnIndex = 1; columnIndex <= lastFrozenColumnIndex; columnIndex++)  
                            {
                                if (!this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Hidden)
                                {
                                    if (this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Key == "RowKeyDisplay")
                                    {
                                        this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Header.Fixed = true; // always fixed
                                    }
                                    else
                                    {
                                        this.ugGrid.DisplayLayout.Bands[bandIndex].Columns[columnIndex].Header.Fixed = blnFreeze;
                                    }
                                }
                            }
                        }
                    }

                    for (int columnIndex = 0; columnIndex <= lastFrozenColumnIndex; columnIndex++)
                    {
                        if (!this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Hidden)
                        {
                            if (this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Key == "RowKeyDisplay")
                            {
                                this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Header.Fixed = true; // always fixed
                            }
                            else
                            {
                                this.ugTotal.DisplayLayout.Bands[0].Columns[columnIndex].Header.Fixed = blnFreeze;
                            }
                        }
                    }
                }
            }
        }

        public void CollapseAllPeriods()
        {
            ExpandRowsHelper(this.ugGrid.Rows, false);
        }
        public void ExpandAllPeriods()
        {
            ExpandRowsHelper(this.ugGrid.Rows, true);
        }
        private void ExpandRowsHelper(RowsCollection rows, bool expand)
        {

            // Loop through every row in the passed in rows collection.
            foreach (UltraGridRow row in rows)
            {
                // If you are using Outlook GroupBy feature and have grouped rows by columns in the
                // UltraGrid, then rows collection can contain group-by rows or regular rows. So you 
                // may need to have code to handle group-by rows as well.
                //if (row is UltraGridGroupByRow)
                //{
                //    UltraGridGroupByRow groupByRow = (UltraGridGroupByRow)row;
                //   // groupByRow.Refresh(RefreshRow.FireInitializeRow);
                //}
                //else
                //{
                //    //row.Refresh(RefreshRow.FireInitializeRow);
                //    FormatRowCells(row);
                //}
                //FormatRowCells(row);
                //row.Refresh(RefreshRow.FireInitializeRow);

                if (row.HasChild())
                {
                    if (expand)
                    {
                        row.ExpandAll();
                    }
                    else
                    {
                        row.CollapseAll();
                    }
                }

                // If the row has any child rows. Typically, there is only a single child band. However,
                // there will be multiple child bands if the band associated with row1 has mupliple child
                // bands. This would be the case for exmple when you have a database hierarchy in which a
                // table has multiple child tables.
                if (null != row.ChildBands)
                {
                    // Loop throgh each of the child bands.
                    foreach (UltraGridChildBand childBand in row.ChildBands)
                    {
                        // Call this method recursivedly for each child rows collection.
                        this.ExpandRowsHelper(childBand.Rows, expand);
                    }
                }
            }

        }

        #region "Events"
        public class CellValueChangedEventArgs
        {
            public CellValueChangedEventArgs(int rowIndex, int columnIndex, double newValue) {this.rowIndex = rowIndex; this.columnIndex = columnIndex; this.newValue = newValue; }
           // public int bandIndex { get; private set; }
            public int rowIndex { get; private set; }
            public int columnIndex { get; private set; }
            //public object originalValue { get; private set; }
            public double newValue { get; private set; }
            //public object cellTag { get; private set; }
        }
        public delegate void CellValueChangedEventHandler(object sender, CellValueChangedEventArgs e);
        public event CellValueChangedEventHandler CellValueChangedEvent;
        protected virtual void RaiseCellValueChangedEvent(int rowIndex, int columnIndex, double newValue)
        {
            if (CellValueChangedEvent != null)
                CellValueChangedEvent(this, new CellValueChangedEventArgs(rowIndex, columnIndex,  newValue));
        }

        private bool _wasLastKeyReturnKey = false;
        private void ugGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.ugGrid.PerformAction(UltraGridAction.ExitEditMode);

                if (_wasLastKeyReturnKey == true)
                {
                    _wasLastKeyReturnKey = false;
                    this.RaiseDoubleReturnKeyPressedEvent();
                }
                else
                {
                    _wasLastKeyReturnKey = true;
                }

            }
            else
            {
                _wasLastKeyReturnKey = false;
            }

        }

        private void ugGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Right || e.KeyData == Keys.Left || e.KeyData == Keys.Up || e.KeyData == Keys.Down)
            {
                this.ugGrid.PerformAction(UltraGridAction.ExitEditMode);
            }
        }

        private void ugGrid_BeforePerformAction(object sender, BeforeUltraGridPerformActionEventArgs e)
        {
            try
            {
                if (e.UltraGridAction == UltraGridAction.AboveCell || e.UltraGridAction == UltraGridAction.BelowCell)
                {
                    UltraGridCell cell = this.ugGrid.ActiveCell;
                    int index = cell.Column.Index;
                    this.ugGrid.PerformAction(UltraGridAction.DeactivateCell);
                    e.Cancel = true;
                    UltraGridRow row = cell.Row;
                    switch (e.UltraGridAction)
                    {
                        case UltraGridAction.BelowCell:

                            if (cell.Row.HasChild())
                            {
                                this.ugGrid.PerformAction(UltraGridAction.NextRow);
                            }
                            else if (cell.Row.HasNextSibling(true))
                            {
                                this.ugGrid.PerformAction(UltraGridAction.NextRowByTab);
                            }
                            else
                            {
                                row = row.ParentRow; 
                                while (row != null)
                                {
                                    if (row.HasNextSibling(true))
                                    {
                                        row = row.GetSibling(SiblingRow.Next);
                                        row.Activate(); 
                                        row.Selected = true;
                                        break;
                                    }
                                    else
                                    {
                                        row = row.ParentRow;
                                    }
                                }
                            }   
                            this.ugGrid.ActiveCell = this.ugGrid.ActiveRow.Cells[index];
                            break;

                        case UltraGridAction.AboveCell:

                            if (cell.Row.HasPrevSibling(true))
                            {
                                this.ugGrid.PerformAction(UltraGridAction.PrevRowByTab);
                            }
                            else if (cell.Row.HasParent())
                            {
                                this.ugGrid.PerformAction(UltraGridAction.PrevRow);
                            }
                            this.ugGrid.ActiveCell = this.ugGrid.ActiveRow.Cells[index];
                            break;
                    }
                }
                else if (e.UltraGridAction == UltraGridAction.PrevCell || e.UltraGridAction == UltraGridAction.NextCell)
                {
                  
                    this.ugGrid.PerformAction(UltraGridAction.EnterEditMode);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugGrid_AfterColPosChanged(object sender, AfterColPosChangedEventArgs e)
        {
            try
            {
                if (_initializing)
                {
                    return;
                }
                if (this.ugGrid.DisplayLayout.Bands.Count > 1)
                {
                    foreach (Infragistics.Win.UltraWinGrid.ColumnHeader colHeader in e.ColumnHeaders)
                    {
                        switch (e.PosChanged)
                        {
                            case PosChanged.HiddenStateChanged:

                                break;

                            case PosChanged.Moved:
                                foreach (UltraGridBand band in this.ugGrid.DisplayLayout.Bands)
                                {
                                    band.Columns[colHeader.Column.Key].Header.VisiblePosition = colHeader.VisiblePosition;
                                }
                                foreach (UltraGridBand band in this.ugTotal.DisplayLayout.Bands)
                                {
                                    band.Columns[colHeader.Column.Key].Header.VisiblePosition = colHeader.VisiblePosition;
                                }
                                break;

                            case PosChanged.Sized:
                                if (colHeader.Column.Key == "RowKeyDisplay") // this is disabled in BeforeGroupPosChanged event
                                {
                                    int lastBandIndex = this.ugGrid.DisplayLayout.Bands.Count - 1;
                                    this.ugGrid.EventManager.SetEnabled(GridEventIds.AfterColPosChanged, false);
                                    this.ugGrid.DisplayLayout.Bands[lastBandIndex].Columns[colHeader.Column.Key].Width = _originalRowKeyDisplayWidth;
                                    AdjustGridColumns(false);
                                    this.ugGrid.EventManager.SetEnabled(GridEventIds.AfterColPosChanged, true);
                                }
                                else
                                {
                                    if (colHeader.Column.Width < 60)
                                    {
                                        colHeader.Column.Width = 60;
                                    }
                                    foreach (UltraGridBand band in this.ugGrid.DisplayLayout.Bands)
                                    {
                                        band.Columns[colHeader.Column.Key].Width = colHeader.Column.Width;
                                    }
                                    foreach (UltraGridBand band in this.ugTotal.DisplayLayout.Bands)
                                    {
                                        band.Columns[colHeader.Column.Key].Width = colHeader.Column.Width;
                                    }
                                    int bandWidth = 0;
                                    foreach (UltraGridGroup group in this.ugGrid.DisplayLayout.Bands[0].Groups)
                                    {
                                        if (group.Index > 0)  // Skip period group 
                                        {
                                            bandWidth += group.Width;
                                        }
                                    }
                                    int rowExpanderPadding = (this.ugGrid.DisplayLayout.Bands.Count > 1) ? 19 : 0;
                                    txtBoxTitle.Height = panelTop.Height;
                                    txtBoxTitle.Width = bandWidth + 1;
                                    txtBoxTitle.Location = new System.Drawing.Point(this.ugGrid.DisplayLayout.Bands[0].Columns["RowKeyDisplay"].Width + rowExpanderPadding, txtBoxTitle.Location.Y);
                                }
                               
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugGrid_BeforeGroupPosChanged(object sender, BeforeGroupPosChangedEventArgs e)
        {
            try
            {
                if (_initializing)
                {
                    return;
                }
                if (this.ugGrid.DisplayLayout.Bands.Count > 1)
                {
                    foreach (Infragistics.Win.UltraWinGrid.GroupHeader groupHeader in e.GroupHeaders)
                    {
                        switch (e.PosChanged)
                        {
                            case PosChanged.HiddenStateChanged:

                                break;

                            case PosChanged.Moved:

                                break;

                            case PosChanged.Sized:
                                if (groupHeader.Group.Key == "Group0")
                                {
                                    _rowKeyDisplayWidth = groupHeader.Band.Columns["RowKeyDisplay"].Width;
                                    //e.Cancel = true;        // Can't get the first column to resize correctly; disable for now 
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void ugGrid_AfterGroupPosChanged(object sender, AfterGroupPosChangedEventArgs e)
        {
            try
            {
                if (_initializing)
                {
                    return;
                }
                if (this.ugGrid.DisplayLayout.Bands.Count > 1)
                {
                    UltraGridColumn lastCol;
                    foreach (Infragistics.Win.UltraWinGrid.GroupHeader groupHeader in e.GroupHeaders)
                    {
                        switch (e.PosChanged)
                        {
                            case PosChanged.HiddenStateChanged:

                                break;

                            case PosChanged.Moved:
                                lastCol = groupHeader.Group.Columns[groupHeader.Group.Columns.Count - 1];
                                foreach (UltraGridBand band in this.ugGrid.DisplayLayout.Bands)
                                {
                                    band.Columns[lastCol.Key].Header.VisiblePosition = lastCol.Header.VisiblePosition;
                                }
                                break;

                            case PosChanged.Sized:
                                lastCol = groupHeader.Group.Columns[groupHeader.Group.Columns.Count - 1];
                                if (lastCol.Key == "RowKeyDisplay")
                                {
                                    int newWidthChange = lastCol.Width - _rowKeyDisplayWidth;
                                    int lastBandIndex = this.ugGrid.DisplayLayout.Bands.Count - 1;
                                   
                                    this.ugGrid.EventManager.SetEnabled(GridEventIds.AfterColPosChanged, false);
                                    this.ugGrid.DisplayLayout.Bands[lastBandIndex].Columns[lastCol.Key].Width += newWidthChange;
                                    AdjustGridColumns(false);

                                    this.ugGrid.EventManager.SetEnabled(GridEventIds.AfterColPosChanged, true);
                                }
                                else
                                {
                                    foreach (UltraGridBand band in this.ugGrid.DisplayLayout.Bands)
                                    {
                                        band.Columns[lastCol.Key].Width = lastCol.Width;
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugTotal_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            try
            {
                 
                e.Layout.Override.BorderStyleRow = UIElementBorderStyle.Solid;
                e.Layout.Override.BorderStyleCell = UIElementBorderStyle.Solid;
                e.Layout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.Select;
                e.Layout.UseFixedHeaders = true;
                e.Layout.Override.FixedHeaderIndicator = FixedHeaderIndicator.None; 

                e.Layout.Override.RowSpacingBefore = 0;
                e.Layout.Override.RowSpacingAfter = 0;
                e.Layout.Override.CellPadding = cellPadding;
                e.Layout.Bands[0].Override.RowSizing = RowSizing.AutoFree;
                e.Layout.Bands[0].Override.CellMultiLine = DefaultableBoolean.True;
              

                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    int cubeColumnIndex = 0;  //store in tag to allow dynamic switching of columns
                    foreach (UltraGridColumn column in band.Columns)
                    {
                        column.Tag = cubeColumnIndex;
                        cubeColumnIndex++;
                    }
                }

            
                string[] colKeySplit;
                foreach (UltraGridBand band in e.Layout.Bands)
                {
                    band.Columns["ParentRowKey"].Hidden = true;
                    band.Columns["RowIndex"].Hidden = true;
                    band.Columns["RowKey"].Hidden = true;
                    band.Columns["RowSortIndex"].Hidden = true;
                    band.Columns["RowKeyDisplay"].CellAppearance = ugGrid.DisplayLayout.Appearances["timeValueAppearance"];

                    if (band.Index == 0)
                    {
                        band.ColHeadersVisible = false; 
                    }

                    foreach (UltraGridColumn column in band.Columns)
                    {
                        if (column.Key == "RowKeyDisplay")
                        {
                            column.Header.Caption = string.Empty;
                            column.Header.Appearance = ugGrid.DisplayLayout.Appearances["emptySpaceAppearance"];
                            column.Header.Fixed = true;
                        }
                        else if (!column.Hidden)
                        {
                            colKeySplit = column.Key.Split(new char[] { '~' });
                            if (colKeySplit.Length > 1)
                            {
                                column.Header.Caption = colKeySplit[colKeySplit.Length - 1];
                                if (_firstTime && band.Index == 0)
                                {
                                    _numQtys++;
                                }
                            }
                            else if (band.Index == 0)
                            {
                                _variableCount++;
                                column.Header.Caption = string.Empty;
                                if (_firstTime)
                                {
                                    if (_variableCount > 1)
                                    {
                                        _firstTime = false;
                                    }
                                    else
                                    {
                                        _numQtys++;
                                    }
                                }
                            }
                            column.Header.Appearance = ugGrid.DisplayLayout.Appearances["variableValueAppearance"];
                            //column.CellAppearance.BackColor = variableColumnBackColor; //variableValueBackColor;
                            //column.CellAppearance.ForeColor = variableColumnForeColor;
                            //column.CellAppearance.BorderColor = variableColumnBorderColor; // Color.White;
                            column.CellAppearance.TextHAlign = HAlign.Right;

                            //column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand, AutoResizeColumnWidthOptions.All); 
                        }
                    }
                }
                
                e.Layout.Bands[0].SortedColumns.Add("RowSortIndex", false, false);
            }
            catch
            {
                throw;
            }
        }


        private void ugTotal_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            ugGrid_InitializeRow(sender, e);
        }
       
        private void ugTotal_AfterColRegionScroll(object sender, ColScrollRegionEventArgs e)
        {

            ugGrid.DisplayLayout.ColScrollRegions[0].Position = e.ColScrollRegion.Position;
        }

        private void ugTotal_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
        {
            if (e.CancellingEditOperation)
                return;

            int cubeColumnIndex = GetCellCubeColumnIndex(ugTotal.ActiveCell);
            int cubeRowIndex = GetCellCubeRowIndex(ugTotal.ActiveCell);


            //exit edit mode if the provided value is blank
            string strValue = ugTotal.ActiveCell.Text;
            if (strValue == String.Empty)
            {
                // If the UltraGrid must exit the edit mode, then cancel the
                // cell update so the original value gets restored in the cell.
                this.ugTotal.ActiveCell.CancelUpdate();
                return;
            }

            if (_isCellNewValueValid(cubeRowIndex, cubeColumnIndex, strValue) == true)
            {


                //ugGrid.ActiveRow.Band.Index,
                RaiseCellValueChangedEvent(cubeRowIndex, cubeColumnIndex, System.Convert.ToDouble(strValue, CultureInfo.CurrentUICulture));
            }
            else
            {
                MessageBox.Show(_invalidDataMsg, _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                // If ForceExit is true, then the UltraGrid will exit the edit mode
                // regardless of whether you cancel this event or not. ForceExit would
                // be true for example when the UltraGrid is being disposed of and thus
                // it can't stay in edit mode. In which case setting Cancel won't do
                // any good so just cancel the update to revert the cell's value back
                // to its original value.
                if (e.ForceExit)
                {
                    // If the UltraGrid must exit the edit mode, then cancel the
                    // cell update so the original value gets restored in the cell.
                    this.ugTotal.ActiveCell.CancelUpdate();
                    return;
                }

                // In normal circumstances where ForceExit is false, set Cancel to 
                // true so the UltraGrid doesn't exit the edit mode.
                e.Cancel = true;
            }
        }

        private void ugTotal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                this.ugTotal.PerformAction(UltraGridAction.ExitEditMode);

                if (_wasLastKeyReturnKey == true)
                {
                    _wasLastKeyReturnKey = false;
                    this.RaiseDoubleReturnKeyPressedEvent();
                }
                else
                {
                    _wasLastKeyReturnKey = true;
                }

            }
            else
            {
                _wasLastKeyReturnKey = false;
            }
        }

        private void splitContainer_DoubleClick(object sender, EventArgs e)
        {
            ResetSplitterLocation();
        }
      
        //public class SelectedCellChangedEventArgs
        //{
        //    public SelectedCellChangedEventArgs( bool isColumnFrozen) { this.isColumnFrozen = isColumnFrozen; }
        //    public bool isColumnFrozen { get; private set; }
        //}
        //public delegate void SelectedCellChangedEventHandler(object sender, SelectedCellChangedEventArgs e);
        //public event SelectedCellChangedEventHandler SelectedCellChangedEvent;
        //protected virtual void RaiseSelectedCellChangedEvent(bool isColumnFrozen)
        //{
        //    if (SelectedCellChangedEvent != null)
        //        SelectedCellChangedEvent(this, new SelectedCellChangedEventArgs(isColumnFrozen));
        //}

        public class DoubleReturnKeyPressedEventArgs
        {
            public DoubleReturnKeyPressedEventArgs() { }
        }
        public delegate void DoubleReturnKeyPressedEventHandler(object sender, DoubleReturnKeyPressedEventArgs e);
        public event DoubleReturnKeyPressedEventHandler DoubleReturnKeyPressedEvent;
        protected virtual void RaiseDoubleReturnKeyPressedEvent()
        {
            if (DoubleReturnKeyPressedEvent != null)
                DoubleReturnKeyPressedEvent(this, new DoubleReturnKeyPressedEventArgs());
        }

       
        //public void SetColumnToolTips(List<BasisToolTip> basisToolTips)
        //{
            //foreach (UltraGridColumn column in ugGrid.DisplayLayout.Bands[0].Columns)
            //{
            //    column.TipStyleCell = TipStyle.Show;
            //    BasisToolTip tresult = basisToolTips.Find(delegate(BasisToolTip tip) { return column.Header.Caption == tip.basisHeaderName; });
            //    if (tresult != null && tresult.isDisplayed)
            //    {
            //        column.Header.ToolTipText = tresult.toolTip;
            //    }
            //    else
            //    {
            //        column.Header.ResetToolTipText();
            //    }
            //}

            //foreach (UltraGridGroup g in ugGrid.DisplayLayout.Bands[0].Groups)
            //{
            //    string s = g.Header.Caption;
            //    bool found = false;
            //    int i = 0;
            //    while (found == false && i < g.Columns.Count)
            //    {
            //        if (g.Columns[i].Header.Caption == s)
            //        {
            //            found = true;
            //            g.Header.ToolTipText = g.Columns[i].Header.ToolTipText;
            //        }
            //        else
            //        {
            //            i++;
            //        }
            //    }
            //    if (found == false)
            //    {
            //        g.Header.ResetToolTipText();
            //    }
            //}
        //}



        #endregion

     
    }

    class GridDrawFilter : IUIElementDrawFilter
    {

        int _numHiddenCols;
        int _numQtys;
        Theme _theme;
        bool _applyTheme;

        public GridDrawFilter(int aNumHiddenCols, int aNumQtys, Theme aTheme)
        {
            _numHiddenCols = aNumHiddenCols;
            _numQtys = aNumQtys;
            _theme = aTheme;
        }

        public bool DrawElement(DrawPhase drawPhase, ref UIElementDrawParams drawParams)
        {
            CellUIElement cell = drawParams.Element as CellUIElement;

            Color penColor = (_theme.DisplayColumnGroupDivider) ? _theme.ColumnGroupDividerBrushColor : Color.Transparent;
            Pen pen = new Pen(penColor);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            pen.Width = 2;

            if (cell != null && cell.Column.Group != null)
            {
                if (cell != null && cell.Column.Key != "RowKeyDisplay")
                {
                    if (cell.Column.Group.Columns[0].Equals(cell.Column))
                    {
                        drawParams.Graphics.DrawLine(pen, new Point(cell.Rect.Location.X, cell.Rect.Location.Y - 1), new Point(cell.Rect.Left, cell.Rect.Bottom + 1));
                        cell.Rect.Location.Offset(1, 0);
                    }

                    //if (cell.Column.Group.Columns[cell.Column.Group.Columns.Count - 1].Equals(cell.Column))
                    //{
                    //    drawParams.Graphics.DrawLine(pen, new Point(cell.Column.Header.GetUIElement().Rect.Right, cell.Rect.Location.Y - 1), new Point(cell.Column.Header.GetUIElement().Rect.Right, cell.Rect.Bottom + 1));
                    //    cell.Rect = new Rectangle(cell.Rect.Location, new Size(cell.Column.Header.GetUIElement().Rect.Width - 1, cell.Rect.Height));
                    //}

                    return true;
                }
            }
            else if (cell != null && cell.Column.Key != "RowKeyDisplay")
            {
                if (!cell.Column.Key.Contains("~") && !cell.Column.Key.Contains("@@"))
                {
                    drawParams.Graphics.DrawLine(pen, new Point(cell.Rect.Location.X, cell.Rect.Location.Y - 1), new Point(cell.Rect.Left, cell.Rect.Bottom + 1));
                    cell.Rect.Location.Offset(1, 0);
                }
                //else if (cell.Column.Band.Index > 0)
                //{  
                //    UltraGridRow hRow = cell.Cell.Row;
                //    while (hRow.Band.Index != 0)
                //    {
                //        hRow = hRow.ParentRow;
                //    }
                //    CellUIElement cell0 = (CellUIElement)hRow.Cells[cell.Column.Key].GetUIElement();
                    
                //    int colCount = cell0.Column.Group.Columns.Count;
                //    if ((cell.Column.Index - _numHiddenCols) % colCount == 0)
                //    {
                //        ////drawParams.Graphics.DrawLine(pen, new Point(cell.Rect.Right, cell.Rect.Location.Y - 1), new Point(cell.Rect.Right, cell.Rect.Bottom + 1));
                //        ////cell.Rect = new Rectangle(cell.Rect.Location, new Size(cell.Rect.Width - 1, cell.Rect.Height));
                //        //drawParams.Graphics.DrawLine(pen, new Point(cell0.Column.Header.GetUIElement().Rect.Right, cell.Rect.Location.Y - 1), new Point(cell0.Column.Header.GetUIElement().Rect.Right, cell.Rect.Bottom + 1));
                //        //cell.Rect = new Rectangle(cell.Rect.Location, new Size(cell0.Column.Header.GetUIElement().Rect.Width - 1, cell.Rect.Height));
                //    }
                //}
                
                return true;
            }

            HeaderUIElement header = drawParams.Element as HeaderUIElement;
            
            if (header != null && header.Header.Column != null && header.Header.Column.Band.Index == 0 ) 
            {
                if (header.Header.Column.Group.Columns[0].Equals(header.Header.Column) && header.Header.Column.Key != "RowKeyDisplay")
                {
                    //drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X - 1, header.Rect.Location.Y - 1), new Point(header.Rect.Left - 1, header.Rect.Bottom + 1));
                    drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X + 1, header.Rect.Location.Y - 1), new Point(header.Rect.Left + 1, header.Rect.Bottom + 1));
                }
                if (header.Header.Column.Group.Columns[header.Header.Column.Group.Columns.Count - 1].Equals(header.Header.Column) && header.Header.Column.Key != "RowKeyDisplay")
                {
                    if (header.Header.Group.Equals(header.Header.Band.Groups[header.Header.Band.Groups.Count - 1]))
                    {
                        pen.Width = 1;
                        //pen.Color = header.Header.Appearance.BorderColor;
                        pen.Color = Color.LightGray;
                    }
                    drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Right, header.Rect.Location.Y - 1), new Point(header.Rect.Right, header.Rect.Bottom + 1));
                }
                return true;
            }


            if (header != null && header.Header.Group != null && header.Header.Group.Key != "Group0")
            {
                //drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X - 1, header.Rect.Location.Y - 1), new Point(header.Rect.Left - 1, header.Rect.Bottom + 1));
                //drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X + header.Rect.Width, header.Rect.Location.Y), new Point(header.Rect.Right - 1, header.Rect.Bottom + 1));
                drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X, header.Rect.Location.Y - 1), new Point(header.Rect.Left, header.Rect.Bottom + 1));
                if (!header.Header.Group.Equals(header.Header.Band.Groups[header.Header.Band.Groups.Count - 1]))
                {
                    drawParams.Graphics.DrawLine(pen, new Point(header.Rect.Location.X - 1 + header.Rect.Width, header.Rect.Location.Y), new Point(header.Rect.Right, header.Rect.Bottom + 1));
                }
                return true;
            }

            return false;
        }

        public DrawPhase GetPhasesToFilter(ref UIElementDrawParams drawParams)
        {
            if (drawParams.Element is CellUIElement)
                return Infragistics.Win.DrawPhase.AfterDrawBorders;
            if (drawParams.Element is HeaderUIElement)
                return Infragistics.Win.DrawPhase.AfterDrawBorders;
            return DrawPhase.None;
        }
    }
}
