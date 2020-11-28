using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinEditors;

namespace MIDRetail.Windows
{
    public partial class ComponentMatch : MIDFormBase
    {

        // add event to throw when OK button is clicked
        public delegate void ComponentMatchEventHandler(object source, ComponentMatchEventArgs e);
        public event ComponentMatchEventHandler OnComponentMatchEventHandler;

        #region Fields
        //=======
        // FIELDS
        //=======
        private BindingSource _bindSourceLink;
        private BindingSource _bindSourceHeader;

        private DataSet   _linkDataSet = MIDEnvironment.CreateDataSet();
        private DataSet   _hdrDataSet = MIDEnvironment.CreateDataSet();
        private DataTable _dtLinkToParent = MIDEnvironment.CreateDataTable("LinkToParent");
        private DataTable _dtLinkToComponent = MIDEnvironment.CreateDataTable("LinkToComponent");
        private DataTable _dtLinkedComponent = MIDEnvironment.CreateDataTable("LinkedComponent");
        private DataTable _dtHdrParent = MIDEnvironment.CreateDataTable("HdrParent");
        private DataTable _dtHdrComponent = MIDEnvironment.CreateDataTable("HdrComponent");
        private UltraGrid _rClickGrid = null;
        private Hashtable _parentHash = new Hashtable();
        private Hashtable _hdrHash = new Hashtable();
        private Hashtable _colorHash = new Hashtable();
        private PlaceholderComponentLinks _phCompLinks;

        private bool _ctrlOrShiftKeyPressed = false;
      
        #endregion Fields

        #region Constructors
        //=============
        // CONSTRUCTORS
        //=============
        public ComponentMatch(SessionAddressBlock aSAB, PlaceholderComponentLinks aPhCompLinks)
            : base(aSAB)
        {
            _phCompLinks = aPhCompLinks;
            InitializeComponent();
        }
        #endregion Constructors

        private void ComponentMatch_Load(object sender, EventArgs e)
        {
            try
            {
                LoadColors();
                BuildDataSets();
                LoadData();
                LoadGrids();
            }
            catch
            {
                throw;
            }
        }

        private void LoadColors()
        {
            try
            {
                Array colorsArray = Enum.GetValues(typeof(KnownColor));
                KnownColor[] allColors = new KnownColor[colorsArray.Length];
                Array.Copy(colorsArray, allColors, colorsArray.Length);

                for (int i = 0; i < allColors.Length; i++)
                {
                    Color color = Color.FromName(allColors[i].ToString());
                    if (!_colorHash.ContainsKey(color.Name))
                    {
                        _colorHash.Add(color.Name, color);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void BuildDataSets()
        {
            try
            {
                _dtLinkToParent.Columns.Add("HdrRID", System.Type.GetType("System.Int32"));
                _dtLinkToParent.Columns.Add("HeaderID");
              
                SetupBulkColor();   //TODO qualify & add SetupPack
               
            }
            catch
            {
                throw;
            }
        }

        private void SetupBulkColor()
        {
            try
            {
                _dtLinkToComponent.Columns.Add("HdrRID", System.Type.GetType("System.Int32"));
                _dtLinkToComponent.Columns.Add("HdrBCRID", System.Type.GetType("System.Int32"));
                _dtLinkToComponent.Columns.Add("ColorCodeRID", System.Type.GetType("System.Int32"));
                _dtLinkToComponent.Columns.Add("AsrtBCRID", System.Type.GetType("System.Int32"));
                _dtLinkToComponent.Columns.Add("BulkColor");
                _dtLinkToComponent.Columns.Add("Description");
                _dtLinkToComponent.Columns.Add("IsVirtual", System.Type.GetType("System.Boolean"));
               // _dtLinkToComponent.Columns.Add("DisplayColor", System.Type.GetType("System.Object"));

                _dtLinkedComponent.Columns.Add("HdrRID", System.Type.GetType("System.Int32"));
                _dtLinkedComponent.Columns.Add("HdrBCRID", System.Type.GetType("System.Int32"));
                _dtLinkedComponent.Columns.Add("ColorCodeRID", System.Type.GetType("System.Int32"));
                _dtLinkedComponent.Columns.Add("PhRID", System.Type.GetType("System.Int32"));
                _dtLinkedComponent.Columns.Add("AsrtBCRID", System.Type.GetType("System.Int32"));
                _dtLinkedComponent.Columns.Add("HeaderID");
                _dtLinkedComponent.Columns.Add("BulkColor");
                _dtLinkedComponent.Columns.Add("Description");
                _dtLinkedComponent.Columns.Add("HdrGridRow", System.Type.GetType("System.Object"));

                _linkDataSet.Tables.Add(_dtLinkToParent);
                _linkDataSet.Tables.Add(_dtLinkToComponent);
                _linkDataSet.Tables.Add(_dtLinkedComponent);

                _linkDataSet.Relations.Add("LinkToComponent", _linkDataSet.Tables["LinkToParent"].Columns["HdrRID"], _linkDataSet.Tables["LinkToComponent"].Columns["HdrRID"]);
                
                _linkDataSet.Relations.Add("LinkedComponent", new DataColumn[] {_linkDataSet.Tables["LinkToComponent"].Columns["HdrRID"], _linkDataSet.Tables["LinkToComponent"].Columns["HdrBCRID"] },
                   new DataColumn[] {_linkDataSet.Tables["LinkedComponent"].Columns["PhRID"], _linkDataSet.Tables["LinkedComponent"].Columns["AsrtBCRID"]}, true);



                _dtHdrParent.Columns.Add("HdrRID", System.Type.GetType("System.Int32"));
                _dtHdrParent.Columns.Add("HeaderID");
                _dtHdrParent.Columns.Add("PlaceholderRID", System.Type.GetType("System.Int32"));

                _dtHdrComponent.Columns.Add("HdrRID", System.Type.GetType("System.Int32"));
                _dtHdrComponent.Columns.Add("HdrBCRID", System.Type.GetType("System.Int32"));
                _dtHdrComponent.Columns.Add("ColorCodeRID", System.Type.GetType("System.Int32"));
                _dtHdrComponent.Columns.Add("PhRID", System.Type.GetType("System.Int32"));
                _dtHdrComponent.Columns.Add("AsrtBCRID", System.Type.GetType("System.Int32"));
                _dtHdrComponent.Columns.Add("BulkColor");
                _dtHdrComponent.Columns.Add("Description");

                _hdrDataSet.Tables.Add(_dtHdrParent);
                _hdrDataSet.Tables.Add(_dtHdrComponent);

                _hdrDataSet.Relations.Add("HdrComponent", _hdrDataSet.Tables["HdrParent"].Columns["HdrRID"], _hdrDataSet.Tables["HdrComponent"].Columns["HdrRID"]);

                _bindSourceLink = new BindingSource(_linkDataSet, "");
                _bindSourceHeader = new BindingSource(_hdrDataSet, "");
            
            }
            catch
            {
                throw;
            }
        }

        private void LoadData()
        {
            try
            {
                ProfileList phProfList = (ProfileList)_phCompLinks.PlaceHolderList;
                _linkDataSet.Clear();
                _hdrDataSet.Clear();
                ugLinkGrid.DataSource = null;
                ugHdrGrid.DataSource = null;
                for (int i = 0; i < phProfList.Count; i++)
                {
                    PlaceholderComponentLinkProfile pclp = (PlaceholderComponentLinkProfile)phProfList[i];

                    if (pclp.HeaderColorList.Count > 0)
                    {
                        DataRow phHdrRow = _dtLinkToParent.NewRow();
                        phHdrRow["HdrRID"] = pclp.PlaceholderRID;
                        phHdrRow["HeaderID"] = pclp.PlaceholderID;
                        _dtLinkToParent.Rows.Add(phHdrRow);

                        for (int j = 0; j < pclp.PhColorList.Count; j++)
                        {
                            HeaderComponentProfile hcp = (HeaderComponentProfile)pclp.PhColorList[j];
                            DataRow phCompRow = _dtLinkToComponent.NewRow();
                            phCompRow["HdrRID"] = hcp.HeaderRID;
                            phCompRow["HdrBCRID"] = hcp.HdrBCRID;
                            phCompRow["ColorCodeRID"] = hcp.ColorCodeRID;
                            phCompRow["AsrtBCRID"] = hcp.AsrtBCRID;
                            phCompRow["BulkColor"] = hcp.BulkColor;
                            phCompRow["Description"] = hcp.Description;
                            phCompRow["IsVirtual"] = hcp.IsVirtual;
                            //phCompRow["DisplayColor"] = hcp.DisplayColor; 
                            _dtLinkToComponent.Rows.Add(phCompRow);
                        }

                        foreach (int hdrRID in pclp.HeaderColorList.Keys)
                        {
                            DataRow hdrRow = _dtHdrParent.NewRow();
                            ProfileList profList = (ProfileList)pclp.HeaderColorList[hdrRID];
                            foreach (HeaderComponentProfile hcp in profList)
                            { 
                                
                                if (hdrRow["HdrRID"] == DBNull.Value)
                                {
                                    hdrRow["HdrRID"] = hcp.HeaderRID;
                                    hdrRow["HeaderID"] = hcp.HeaderID;
                                    hdrRow["PlaceholderRID"] = pclp.PlaceholderRID;
                                    _dtHdrParent.Rows.Add(hdrRow);
                                    _dtHdrParent.AcceptChanges();
                                }
                                DataRow hdrCompRow = _dtHdrComponent.NewRow();
                                hdrCompRow["HdrRID"] = hcp.HeaderRID;
                                hdrCompRow["HdrBCRID"] = hcp.HdrBCRID;
                                hdrCompRow["ColorCodeRID"] = hcp.ColorCodeRID;
                                hdrCompRow["PhRID"] = pclp.PlaceholderRID;
                                hdrCompRow["AsrtBCRID"] = hcp.AsrtBCRID;
                                hdrCompRow["BulkColor"] = hcp.BulkColor;
                                hdrCompRow["Description"] = hcp.Description;
                                _dtHdrComponent.Rows.Add(hdrCompRow);

                                if (hcp.AsrtBCRID != 0)
                                {
                                    DataRow linkedCompRow = _dtLinkedComponent.NewRow();
                                    linkedCompRow["HdrRID"] = hcp.HeaderRID;
                                    linkedCompRow["HdrBCRID"] = hcp.HdrBCRID;
                                    linkedCompRow["ColorCodeRID"] = hcp.ColorCodeRID;
                                    linkedCompRow["AsrtBCRID"] = hcp.AsrtBCRID;
                                    linkedCompRow["HeaderID"] = hcp.HeaderID;
                                    linkedCompRow["BulkColor"] = hcp.BulkColor;
                                    linkedCompRow["Description"] = hcp.Description;
                                    _dtLinkedComponent.Rows.Add(linkedCompRow);
                                }
                                 
                            }
                        }
                        _linkDataSet.AcceptChanges();
                        _hdrDataSet.AcceptChanges();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void LoadGrids()
        {
            try
            {
                ugLinkGrid.DataSource = _bindSourceLink;
                //ugLinkGrid.Rows[0].Selected = true;
                //ugLinkGrid.Rows[0].ExpandAll();
                ugLinkGrid.Rows.ExpandAll(true);

                ugHdrGrid.DataSource = _bindSourceHeader;
                ugHdrGrid.Rows.ExpandAll(false);

                ugLinkGrid.Rows[0].Selected = true;
                //ugLinkGrid.Rows[0].ExpandAll();
            }
            catch
            {
                throw;
            }
        }

        #region LinkGrid
        private void ugLinkGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                e.Layout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
                e.Layout.Override.ExpansionIndicator = Infragistics.Win.UltraWinGrid.ShowExpansionIndicator.CheckOnDisplay;
                e.Layout.Bands[0].Columns["HdrRID"].Hidden = true;
             
                e.Layout.Bands[1].Columns["HdrRID"].Hidden = true;
                e.Layout.Bands[1].Columns["HdrBCRID"].Hidden = true;
                e.Layout.Bands[1].Columns["AsrtBCRID"].Hidden = true;
                e.Layout.Bands[1].Columns["ColorCodeRID"].Hidden = true;
                e.Layout.Bands[1].Columns["IsVirtual"].Hidden = true;
              
                e.Layout.Bands[2].Columns["HdrRID"].Hidden = true;
                e.Layout.Bands[2].Columns["HdrBCRID"].Hidden = true;
                e.Layout.Bands[2].Columns["PhRID"].Hidden = true;
                e.Layout.Bands[2].Columns["AsrtBCRID"].Hidden = true;
                e.Layout.Bands[2].Columns["ColorCodeRID"].Hidden = true;
                e.Layout.Bands[2].Columns["HdrGridRow"].Hidden = true;

                e.Layout.Bands[0].Columns["HeaderID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PlaceholderID);

                //EmbeddableEditorBase editor = null;
                //DefaultEditorOwnerSettings editorSettings = null;

                // Add an item for Color picker.
                //editorSettings = new DefaultEditorOwnerSettings();
                //editorSettings.DataType = typeof(Color);
                //editor = new ColorPickerEditor(new DefaultEditorOwner(editorSettings));

                //e.Layout.Bands[1].Columns["DisplayColor"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
                //e.Layout.Bands[1].Columns["DisplayColor"].ButtonDisplayStyle = Infragistics.Win.UltraWinGrid.ButtonDisplayStyle.Always;
                //e.Layout.Bands[1].Columns["DisplayColor"].Editor = editor;
                //e.Layout.Bands[1].Columns["DisplayColor"].PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                //e.Layout.Bands[1].Columns["DisplayColor"].CellClickAction = CellClickAction.CellSelect;
            }
            catch
            {
                throw;
            }
        }

        private void ugLinkGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            {
                switch (e.Row.Band.Key)
                {
                    case "LinkToParent":
                        e.Row.Activation = Activation.NoEdit;
                        break;
                    case "LinkToComponent":
                        e.Row.Cells["BulkColor"].Activation = Activation.NoEdit;
                        e.Row.Cells["Description"].Activation = Activation.NoEdit;
                        //e.Row.Cells["DisplayColor"].Activation = Activation.AllowEdit;
                        //if (e.Row.Cells["DisplayColor"].Value != DBNull.Value)
                        //{
                        //    e.Row.Appearance.BackColor = (Color)e.Row.Cells["DisplayColor"].Value;
                        //}
                        break;
                    default:
                        e.Row.Activation = Activation.NoEdit;
                        break;
                }
            }
            catch
            {
                throw;
            }
        }

        private void ugLinkGrid_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                _rClickGrid = (UltraGrid)sender;
            }
            catch
            {
                throw;
            }
        }

        private void ugLinkGrid_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            try
            {
                ColorPickerEditor colorEditor = (ColorPickerEditor)e.Cell.EditorResolved;
                System.Drawing.Color color = colorEditor.Color;
                e.Cell.Row.Appearance.BackColor = color;
                int dbColorValue;
                dbColorValue = color.ToArgb();

                if (e.Cell.Row.HasChild(false))
                {
                    UltraGridRow chRow = e.Cell.Row.GetChild(ChildRow.First);
                    while (chRow != null)
                    {
                        UltraGridRow hdrRow = (UltraGridRow)chRow.Cells["HdrGridRow"].Value;
                        hdrRow.Appearance.BackColor = color;
                        hdrRow.Appearance.BackColorDisabled = color;
                        chRow.Appearance.BackColor = color;
                        chRow = chRow.GetSibling(SiblingRow.Next, true, false);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void ugLinkGrid_AfterCellUpdate(object sender, CellEventArgs e)
        {
            try
            { 
                if (e.Cell.Column.Key == "DisplayColor")
                {
                    e.Cell.Column.PerformAutoResize(PerformAutoSizeType.AllRowsInBand);
                }
            }
            catch
            {
                throw;
            }

        }
        private void ugLinkGrid_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            try
            {
                UltraGridRow selRow = null;
                if (typeof(UltraGridRow) == e.Type)
                {
                    if (ugLinkGrid.Selected.Rows.Count > 0)
                    {
                        selRow = ugLinkGrid.Selected.Rows[0];
                        while (selRow.Band.Key != "LinkToParent")
                        {
                            selRow = selRow.ParentRow;
                        }
                    }
                }
                else if (typeof(UltraGridCell) == e.Type)
                {
                    selRow = ugLinkGrid.ActiveCell.Row;
                    while (selRow.Band.Key != "LinkToParent")
                    {
                        selRow = selRow.ParentRow;
                    }
                    if (ugLinkGrid.ActiveCell.Column.Key == "DisplayColor")
                    {
                        ugLinkGrid.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
                        ugLinkGrid.ActiveCell.Row.Selected = true;
                    }
                }
                if (selRow != null)
                {
                    int phRID = Convert.ToInt32(selRow.Cells["HdrRID"].Value, CultureInfo.CurrentUICulture);

                    int rowCount = ugHdrGrid.Rows.Count;
                    UltraGridRow row = ugHdrGrid.GetRow(ChildRow.First);
                    {
                        while (row != null)
                        {
                            if (Convert.ToInt32(row.Cells["PlaceholderRID"].Value, CultureInfo.CurrentUICulture) == phRID)
                            {
                                SetRowActivation(row, Activation.NoEdit);
                            }
                            else
                            {
                                SetRowActivation(row, Activation.Disabled);
                            }
                            row = row.GetSibling(SiblingRow.Next, true, false);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void SetRowActivation(UltraGridRow aRow, Activation aRowActivation)
        {
            try
            {
                aRow.Activation = aRowActivation;
                if (aRow.HasChild())
                {
                    UltraGridRow childRow = aRow.GetChild(ChildRow.First);
                    while (childRow != null)
                    {
                        bool activationSet = false;
                        if (childRow.Band.Key == "HdrComponent")
                        {
                            if (Convert.ToInt32(childRow.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) != 0)
                            {
                                SetRowActivation(childRow, Activation.Disabled);
                                activationSet = true;
                            }
                        }
                        if (!activationSet)
                        {
                            SetRowActivation(childRow, aRowActivation);
                        }
                        childRow = childRow.GetSibling(SiblingRow.Next, true, false);
                    }
                 }
            }
            catch
            {
                throw;
            }
        }

        private void ugLinkGrid_DragEnter(object sender, DragEventArgs e)
        {
            try
            {
                Image_DragEnter(sender, e);
            }
            catch
            {
                throw;
            }
        }

        private void ugLinkGrid_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Image_DragOver(sender, e);

                Infragistics.Win.UIElement aUIElement;

                Point pt = PointToClient(new Point(e.X, e.Y));
                Point realPoint = new Point(pt.X - ugLinkGrid.Location.X, pt.Y - ugLinkGrid.Location.Y); // - panel1.Height - splitContainer.Panel1.Height);

                aUIElement = ugLinkGrid.DisplayLayout.UIElement.ElementFromPoint(realPoint);

                if (aUIElement == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridRow row = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));
                bool validDrag = false;
                if (row == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else
                {
                    validDrag = false;
                    switch (row.Band.Key)
                    {
                        case "LinkToComponent":
                            validDrag = DragOverComponent(sender, aUIElement, ref e);
                            break;

                        default:
                            break;
                    }
                }
                if (validDrag)
                {
                    e.Effect = DragDropEffects.All;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch  
            {
                throw;
            }
        }

        private bool DragOverComponent(object sender, Infragistics.Win.UIElement aUIElement, ref DragEventArgs e)
        {
            bool validDrag = false;
            try
            {
                
                UltraGridRow dragOverRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));
                int hdrRID = Convert.ToInt32(dragOverRow.Cells["HdrRID"].Value, CultureInfo.CurrentUICulture);
                
                SelectedRowsCollection SelRows = (SelectedRowsCollection)e.Data.GetData(typeof(SelectedRowsCollection));

                UltraGridRow selRow = (UltraGridRow)SelRows[0];

                if (Convert.ToInt32(selRow.Cells["PhRID"].Value, CultureInfo.CurrentUICulture) == hdrRID)
                {
                    validDrag = true;
                }
            }
            catch
            {
                throw;
            }
            return validDrag;
        }
        

        private void ugLinkGrid_DragLeave(object sender, EventArgs e)
        {
            try
            {
                Image_DragLeave(sender, e);
            }
            catch
            {
                throw;
            }
        }
        
        private void ugLinkGrid_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Infragistics.Win.UIElement aUIElement;
                
                Point pt = PointToClient(new Point(e.X, e.Y));
                Point realPoint = new Point(pt.X - ugLinkGrid.Location.X, pt.Y - ugLinkGrid.Location.Y); // - panel1.Height - splitContainer.Panel1.Height);

                aUIElement = ugLinkGrid.DisplayLayout.UIElement.ElementFromPoint(realPoint);

                UltraGridRow dropRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));
               
                if (dropRow != null)
                {
                    if (e.Data.GetDataPresent(typeof(SelectedRowsCollection)))
                    {
                        dropRow.Selected = true;
                        LinkColorComponents();
                    }
                    else
                    {
                        MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_InvalidDataType));
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion LinkGrid

        #region HeaderGrid
        private void ugHdrGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                e.Layout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect;
                e.Layout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.ExtendedAutoDrag;
                e.Layout.Override.AllowColMoving = AllowColMoving.NotAllowed;
                e.Layout.Bands[0].Columns["HdrRID"].Hidden = true;
                e.Layout.Bands[0].Columns["PlaceholderRID"].Hidden = true;
                e.Layout.Bands[1].Columns["HdrRID"].Hidden = true;
                e.Layout.Bands[1].Columns["HdrBCRID"].Hidden = true;
                e.Layout.Bands[1].Columns["ColorCodeRID"].Hidden = true;
                e.Layout.Bands[1].Columns["PhRID"].Hidden = true;
                e.Layout.Bands[1].Columns["AsrtBCRID"].Hidden = true;
            }
            catch
            {
                throw;
            }
        }

        private void ugHdrGrid_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
            try
            { 
                e.Row.Activation = Activation.NoEdit;
                if (e.Row.Band.Key == "HdrComponent")
                {
                    UpdateLinkGridRow(e.Row);
                }
            }
            catch
            {
                throw;
            }
        }

        private void UpdateLinkGridRow(UltraGridRow aRow)
        {
            try
            {
                int asrtBCRID = Convert.ToInt32(aRow.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture);
                if (asrtBCRID != 0)
                {
                    IEnumerable enumerator = ugLinkGrid.DisplayLayout.Bands[2].GetRowEnumerator(GridRowType.DataRow);
                    foreach (UltraGridRow row in enumerator)
                    {
                        if (Convert.ToInt32(row.Cells["HdrBCRID"].Value, CultureInfo.CurrentUICulture) == asrtBCRID)
                        {
                            row.Cells["HdrGridRow"].Value = aRow;
                            aRow.Appearance.BackColor = row.Appearance.BackColor;
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

        private void ugHdrGrid_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                _rClickGrid = (UltraGrid)sender;
            }
            catch
            {
                throw;
            }
        }

        private void ugHdrGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Shift)
            {
                e.SuppressKeyPress = true;
                _ctrlOrShiftKeyPressed = true;
            } 
            if (e.Control)
            {
                _ctrlOrShiftKeyPressed = true;
            }    
        }

        private void ugHdrGrid_BeforeSelectChange(object sender, BeforeSelectChangeEventArgs e)
        {
            try
            {
                if (typeof(UltraGridRow) == e.Type && e.NewSelections.Rows.Count > 0)
                {
                    if (_ctrlOrShiftKeyPressed)
                    {
                        foreach (UltraGridRow newSelRow in e.NewSelections.Rows)
                        {   // disallow rows with the same parent to be selected
                            foreach (UltraGridRow selRow in ugHdrGrid.Selected.Rows)
                            {
                                if (selRow != newSelRow &&
                                   (selRow.ParentRow == newSelRow.ParentRow))
                                {
                                    e.Cancel = true;
                                    break;
                                }
                            }
                            if (e.Cancel)
                            {
                                break;
                            }
                        }
                        _ctrlOrShiftKeyPressed = false;
                    }
                }
            }
            catch
            {
                throw;
            }  
        }
       
        private void ugHdrGrid_AfterSelectChange(object sender, AfterSelectChangeEventArgs e)
        {
            try
            { 
                if (typeof(UltraGridRow) == e.Type)
                {
                    ClearErrorImage(ugHdrGrid.DisplayLayout.Bands["HdrComponent"]);
                    if (ugHdrGrid.Selected.Rows.Count > 1)
                    {
                        UltraGridRow firstRow = ugHdrGrid.Selected.Rows[0];
                        int colorCodeRID = Convert.ToInt32(firstRow.Cells["ColorCodeRID"].Value, CultureInfo.CurrentUICulture);

                        foreach (UltraGridRow selHdrRow in ugHdrGrid.Selected.Rows)
                        {
                            if (Convert.ToInt32(selHdrRow.Cells["ColorCodeRID"].Value, CultureInfo.CurrentUICulture) != colorCodeRID)
                            {
                                selHdrRow.Appearance.Image = ErrorImage;
                                MessageBox.Show("Selected header color component must have the same color code");
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }  
        }

        private void ugHdrGrid_SelectionDrag(object sender, CancelEventArgs e)
        {
            try
            {
                if (ugHdrGrid.Selected.Rows.Count == 0)
                {
                    return;
                }
                else  if (ugHdrGrid.Selected.Rows[0].Band.Key != "HdrComponent")
                {
                    return;
                }

                int xPos, yPos;
                int imageHeight, imageWidth, Indent = 0, spacing = 2;
                string hdrIDs = string.Empty;
           
                foreach (UltraGridRow selRow in ugHdrGrid.Selected.Rows)
                {
                    if (hdrIDs == string.Empty)
                    {
                        hdrIDs = selRow.ParentRow.Cells["HeaderID"].Value.ToString();
                    }
                    else
                    {
                        hdrIDs += ", " + selRow.ParentRow.Cells["HeaderID"].Value.ToString();
                    }
                }
               
                MIDGraphics.BuildDragImage(hdrIDs, imageListDrag, Indent, spacing,
                           Font, ForeColor, out imageHeight, out imageWidth);
             
                xPos = imageWidth / 2;
                yPos = imageHeight / 2;
             
                if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos))
                {
                    DragDropEffects dde = ugHdrGrid.DoDragDrop(ugHdrGrid.Selected.Rows, DragDropEffects.Move);

                    if (dde == DragDropEffects.None)
                    {   // for some reason the link grid is in a semi-locked state if no row is dropped
                        // so the EndUpdate() seems to remedy that behavior
                        ugLinkGrid.EndUpdate();
                    }
                    DragHelper.ImageList_EndDrag();
                }    
            }
            catch
            {
                throw;
            } 
        }
        private void ugHdrGrid_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point p = PointToClient(new Point(e.X, e.Y));
                DragHelper.ImageList_DragMove((int)(p.X), (int)(p.Y));
            }
            catch
            {
                throw;
            } 
        }

        #endregion HeaderGrid

        #region ContextMenu
        private void cmsMenu_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                //if (ugLinkGrid.Selected.Rows.Count == 0 || ugHdrGrid.Selected.Rows.Count == 0)
                //{
                //    cmsConnect.Visible = false;
                //    cmsDisconnect.Visible = false;

                //    if (_dtLinkedComponent.Rows.Count == 0)
                //    {
                //        e.Cancel = true;
                //    }     
                //}
                if (_rClickGrid != null)
                {

                    switch (_rClickGrid.Name)
                    {
                        case "ugLinkGrid":
                            cmsConnect.Visible = false;
                            break;

                        case "ugHdrGrid":
                            if (_rClickGrid.Selected.Rows.Count == 0)
                            {
                                //cmsConnect.Visible = false;
                                //if (_dtLinkedComponent.Rows.Count == 0)
                                //{
                                    e.Cancel = true;
                                //}
                            }
                            else if (_rClickGrid.Selected.Rows[0].Band.Key == "HdrComponent")
                            {
                                if (ugLinkGrid.Selected.Rows.Count == 0)
                                {
                                    e.Cancel = true;
                                }
                                else if (ugLinkGrid.Selected.Rows[0].Band.Key == "LinkToParent" ||
                                         ugLinkGrid.Selected.Rows[0].Band.Key == "LinkedComponent")
                                {
                                    e.Cancel = true;
                                }
                                else  
                                {
                                    e.Cancel = true;
                                    foreach (UltraGridRow hdrRow in _rClickGrid.Selected.Rows)
                                    {
                                        if (Convert.ToInt32(hdrRow.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) == 0)
                                        {
                                            cmsConnect.Visible = true;
                                            e.Cancel = false;
                                            break;
                                        }
                                        //if (Convert.ToInt32(hdrRow.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) != 0)
                                        //{
                                        //    MessageBox.Show("Selected headers are already connected");
                                        //}

                                    }
                                }
                            }
                            else
                            {
                                e.Cancel = true;
                            }
                            break;

                    }
          
                    cmsDisconnect.Visible = (_rClickGrid.Name == "ugLinkGrid") ? true : false;
                    cmsDisconnectAll.Visible = (_rClickGrid.Name == "ugLinkGrid") ? true : false;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmsConnect_Click(object sender, EventArgs e)
        {
            try
            {
                LinkColorComponents();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }    

        private void LinkColorComponents()
        {
            try
            {
                string errorMessage = string.Empty;
                bool errorFound = false;

                ClearErrorImage(ugHdrGrid.DisplayLayout.Bands["HdrComponent"]);

                if (ugLinkGrid.Selected.Rows.Count == 0 || ugHdrGrid.Selected.Rows.Count == 0)
                {
                    return;
                }

                UltraGridRow selLinkRow = ugLinkGrid.Selected.Rows[0];
                int phRID = Convert.ToInt32(selLinkRow.Cells["HdrRID"].Value, CultureInfo.CurrentUICulture);
                int phBCRID = Convert.ToInt32(selLinkRow.Cells["HdrBCRID"].Value, CultureInfo.CurrentUICulture);
                int linkColorRID = Convert.ToInt32(selLinkRow.Cells["ColorCodeRID"].Value, CultureInfo.CurrentUICulture);
                bool linkIsVirtual = Convert.ToBoolean(selLinkRow.Cells["IsVirtual"].Value, CultureInfo.CurrentUICulture);
           
                foreach (UltraGridRow selHdrRow in ugHdrGrid.Selected.Rows)
                {
                    if (Convert.ToInt32(selHdrRow.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) != 0)
                    {
                        errorFound = true;
                        selHdrRow.Appearance.Image = ErrorImage;
                        errorMessage = "Selected header component already assigned to a placeholder";
                        break;
                    }
                  
                    UltraGridRow row = selHdrRow.GetSibling(SiblingRow.First, false);
                    while (row != null)
                    {
                        selHdrRow.Appearance.Image = null;
                        if (row != selHdrRow &&
                             Convert.ToInt32(row.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) == phBCRID)
                        {
                            errorFound = true;
                            selHdrRow.Appearance.Image = ErrorImage;
                            errorMessage = "Another component of this header is already assigned to this placeholder";
                            break;
                        }
                        row = row.GetSibling(SiblingRow.Next, false);
                    }
                    if (errorFound)
                    {
                        break;
                    }
                    else
                    {
                        if (!linkIsVirtual)
                        {
                            if (Convert.ToInt32(selHdrRow.Cells["ColorCodeRID"].Value, CultureInfo.CurrentUICulture) != linkColorRID)
                            {
                                errorFound = true;
                                selHdrRow.Appearance.Image = ErrorImage;
                                errorMessage = "Selected header color must be the same color code as the placeholder";
                                break;
                            }    
                        }
                    }
                }
                if (errorFound)
                {
                    MessageBox.Show(errorMessage);
                    return;
                }
                if (linkIsVirtual)
                {
                    UltraGridRow firstRow = ugHdrGrid.Selected.Rows[0];
                    selLinkRow.Cells["ColorCodeRID"].Value = firstRow.Cells["ColorCodeRID"].Value;
                    selLinkRow.Cells["BulkColor"].Value = firstRow.Cells["BulkColor"].Value;
                    selLinkRow.Cells["Description"].Value = firstRow.Cells["Description"].Value;
                    selLinkRow.Cells["IsVirtual"].Value = false;
                }

                ugLinkGrid.ActiveRow = selLinkRow;
                foreach (UltraGridRow selHdrRow in ugHdrGrid.Selected.Rows)
                {
                    string headerID = selHdrRow.ParentRow.Cells["HeaderID"].Value.ToString();
                    selHdrRow.Cells["AsrtBCRID"].Value = phBCRID;
                    selHdrRow.Cells["PhRID"].Value = phRID;

                    UltraGridRow row = ugLinkGrid.DisplayLayout.Bands["LinkedComponent"].AddNew();
                    row.Cells["HeaderID"].Value = headerID;
                    row.Cells["HdrRID"].Value = selHdrRow.Cells["HdrRID"].Value;
                    row.Cells["HdrBCRID"].Value = selHdrRow.Cells["HdrBCRID"].Value;
                    row.Cells["ColorCodeRID"].Value = selHdrRow.Cells["ColorCodeRID"].Value;
                    row.Cells["AsrtBCRID"].Value = selHdrRow.Cells["AsrtBCRID"].Value;
                    row.Cells["BulkColor"].Value = selHdrRow.Cells["BulkColor"].Value;
                    row.Cells["Description"].Value = selHdrRow.Cells["Description"].Value;
                    row.Cells["HdrGridRow"].Value = selHdrRow;

                    row.Appearance.BackColor = selLinkRow.Appearance.BackColor;
                    selHdrRow.Appearance.BackColor = selLinkRow.Appearance.BackColor;
                    selHdrRow.Appearance.BackColorDisabled = selLinkRow.Appearance.BackColor;
                    //selHdrRow.Activation = Activation.ActivateOnly;
                    selHdrRow.Activation = Activation.Disabled;

                }

                ugLinkGrid.Selected.Rows.Clear();
                ugHdrGrid.Selected.Rows.Clear();
            }
            catch 
            {
                throw;
            }
         }

        private void ClearErrorImage(UltraGridBand aBand)
        {
            try
            { 
                IEnumerable enumerator = aBand.GetRowEnumerator(GridRowType.DataRow);
                foreach (UltraGridRow row in enumerator)
                {
                    row.Appearance.Image = null;
                }
            }
            catch
            {
                throw;
            }
        }
        private void cmsDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (ugLinkGrid.Selected.Rows.Count == 0)
                {
                    return;
                }
                ugLinkGrid.BeginUpdate();
                ugLinkGrid.SuspendRowSynchronization();
                ugLinkGrid.DisplayLayout.Bands[2].Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;
                ArrayList deleteRows = new ArrayList();
               
                foreach (UltraGridRow row in ugLinkGrid.Selected.Rows)
                {
                    SelectRowsForDelete(row, ref deleteRows);
                }
             
                if (deleteRows.Count > 0)
                {
                    foreach (UltraGridRow delRow in deleteRows)
                    {
                        UltraGridRow hdrRow = (UltraGridRow)delRow.Cells["HdrGridRow"].Value;
                        hdrRow.Cells["phRID"].Value = 0;
                        hdrRow.Cells["AsrtBCRID"].Value = 0;
                        hdrRow.Appearance.BackColor = ugHdrGrid.DisplayLayout.Appearance.BackColor;
                        delRow.Selected = true;
                    }
                    ugLinkGrid.DeleteSelectedRows(false);
                }

                ugLinkGrid.DisplayLayout.Bands[2].Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
                ugLinkGrid.UpdateData();
                ugLinkGrid.ResumeRowSynchronization();
                ugLinkGrid.EndUpdate();
               
            }
            catch
            {
                throw;
            }
        }
        private void SelectRowsForDelete(UltraGridRow aRow, ref ArrayList aDeleteRows)
        {
            try
            {
                switch (aRow.Band.Key)
                {
                    case "LinkToParent":
                    case "LinkToComponent":
                        if (aRow.HasChild())
                        {
                            UltraGridRow chRow = aRow.GetChild(ChildRow.First);
                            while (chRow != null)
                            {
                                SelectRowsForDelete(chRow, ref aDeleteRows);
                                chRow = chRow.GetSibling(SiblingRow.Next, false, false);
                            }
                        }
                        break;

                    case "LinkedComponent":
                        aDeleteRows.Add(aRow);
                        break;
                }
            }
            catch
            {
                throw;
            }
        }  
  
        private void cmsDisconnectAll_Click(object sender, EventArgs e)
        {
            try
            {
                ugLinkGrid.BeginUpdate();
                ugLinkGrid.DisplayLayout.Bands[2].Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended;

                UltraGridBand band = ugLinkGrid.DisplayLayout.Bands["LinkedComponent"];
                IEnumerable enumerator = band.GetRowEnumerator(GridRowType.DataRow);
                foreach (UltraGridRow row in enumerator)
                {
                    UltraGridRow hdrRow = (UltraGridRow)row.Cells["HdrGridRow"].Value;
                    hdrRow.Cells["AsrtBCRID"].Value = 0;
                    hdrRow.Appearance.BackColor = ugLinkGrid.DisplayLayout.Appearance.BackColor;
                    row.Selected = true;
                }
               
                ugLinkGrid.DeleteSelectedRows(false);
                ugLinkGrid.EndUpdate();

                ugLinkGrid.DisplayLayout.Bands[2].Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Single;
                enumerator = ugLinkGrid.DisplayLayout.Bands[1].GetRowEnumerator(GridRowType.DataRow);
                foreach (UltraGridRow row in enumerator)
                {
                    row.Activation = Activation.NoEdit;
                }
                ugHdrGrid.Selected.Rows.Clear();
            }
            catch
            {
                throw;
            }
        }
        #endregion ContextMenu

        #region Buttons
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidValues())
                {
                    Hashtable linkedComponents = null;
                    ComponentMatchEventArgs ea = new ComponentMatchEventArgs(linkedComponents);
                    if (OnComponentMatchEventHandler != null)  // throw event to explorer to make changes
                    {
                        OnComponentMatchEventHandler(this, ea);
                    }
                    Close();
                }
                //else
                //{
                //     MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorsFoundReviewCorrect), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
            catch
            {
                throw;
            }
        }

        private bool ValidValues()
        {
            bool validValues = true;
            try
            {
                ClearErrorImage(ugHdrGrid.DisplayLayout.Bands["HdrComponent"]);
                
                IEnumerable enumerator = ugHdrGrid.DisplayLayout.Bands["HdrComponent"].GetRowEnumerator(GridRowType.DataRow);
                foreach (UltraGridRow row in enumerator)
                {
                    if (Convert.ToInt32(row.Cells["AsrtBCRID"].Value, CultureInfo.CurrentUICulture) == 0)
                    {
                        row.Appearance.Image = ErrorImage;
                        validValues = false;
                    }
                }
                if (!validValues)
                {
                    MessageBox.Show("Header component not connected to a placeholder");
                }
            }
            catch  
            {
                throw;
            }
            return validValues;
        }
        #endregion Buttons

        

       
        
        

    }

    public class ComponentMatchEventArgs : EventArgs
    {
        Hashtable _linkedComponents;
     
        public ComponentMatchEventArgs(Hashtable aLinkedComponents)
        {
            if (aLinkedComponents == null)
            {
                _linkedComponents = new Hashtable();
            }
            else
            {
                _linkedComponents = aLinkedComponents;
            }
        }
        //public int NumberOfPlaceholderColors
        //{
        //    get { return _placeHolders.Count; }
        //}
        public Hashtable LinkedComponents
        {
            get { return _linkedComponents; }
        }
    }
}