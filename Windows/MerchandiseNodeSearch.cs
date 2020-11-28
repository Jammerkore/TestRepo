//using System;
//using System.Collections.Generic;
//using System.Collections;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Globalization;
//using System.Text;
//using System.Windows.Forms;
//using System.Threading;

//using MIDRetail.Business;
//using MIDRetail.Common;
//using MIDRetail.DataCommon;
//using MIDRetail.Data;
//using MIDRetail.Windows.Controls;


//namespace MIDRetail.Windows
//{
//    public partial class MerchandiseNodeSearch : MIDFormBase
//    {
//        // add events to update explorer when hierarchy is changed
//        public delegate void MerchandiseNodeLocateEventHandler(object source, MerchandiseNodeLocateEventArgs e);
//        public event MerchandiseNodeLocateEventHandler MerchandiseNodeLocateEvent;

//        public delegate void MerchandiseNodeRenameEventHandler(object source, MerchandiseNodeRenameEventArgs e);
//        public event MerchandiseNodeRenameEventHandler MerchandiseNodeRenameEvent;


//        public delegate void MerchandiseNodeDeleteEventHandler(object source, MerchandiseNodeDeleteEventArgs e);
//        public event MerchandiseNodeDeleteEventHandler MerchandiseNodeDeleteEvent;


//        #region Variable Declarations

//        private bool _searching;
//        private bool _nodeUpdated = false;
//        private bool _continueProcessing = false;
//        private bool _progressCancelClicked = false;
//        private bool _mouseDown = false;
//        private SessionAddressBlock _SAB;
//        private HierarchyMaintenance _hierarchyMaintenance;
//        private ArrayList _nodeList;
//        private Hashtable _results;
//        private Thread _thread;
//        private int sortColumn = -1;
//        private string sortColumnName = string.Empty;
//        private ListViewItem _selectedItem;
//        private int _XPos = 0;
//        private int _YPos = 0;
//        private string subItemText;
//        private int subItemSelected = 0;
//        private char _wildcard = '*';
//        private char _separator = ';';
//        //private ProductFilterData _dlFilter;
//        private ArrayList _selectableColumns;
//        private ProductCharProfileList _productCharProfileList;

//        // Begin TT#564 - JSmith - Copy and Paste from search not working
//        ExplorerAddressBlock _EAB;
//        // End TT#564
//        // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//        ProductSearchEngine searchEngine = null;
//        // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

//        private bool _formClosing = false;      //TT#826 - Error produced when closing the hierarchy search while the search is running - apicchetti - 1/19/2011


//        #endregion

//        // Begin TT#564 - JSmith - Copy/Paste from search not working
//        //public MerchandiseNodeSearch(SessionAddressBlock aSAB, ArrayList aNodeList)
//        //    : base(aSAB)
//        public MerchandiseNodeSearch(SessionAddressBlock aSAB, ArrayList aNodeList, ExplorerAddressBlock aEAB)
//            : base(aSAB)
//        // End TT#564
//        {
//            _SAB = aSAB;
//            // Begin TT#564 - JSmith - Copy/Paste from search not working
//            _EAB = aEAB;
//            // End TT#564
//            _hierarchyMaintenance = new HierarchyMaintenance(_SAB);
//            _nodeList = aNodeList;
//            _results = new Hashtable();
//            InitializeComponent();
//            _searching = false;
//            AllowDragDrop = true;
//        }

//        private void MerchandiseNodeSearch_Load(object sender, EventArgs e)
//        {
//            try
//            {
//                SetText();

//                pnlSearch.BringToFront();
//                pnlSearch.Visible = true;
//                pnlCharacteristics.Visible = false;
//                lvNodes.Visible = false;
//                BuildContextmenu();
//                lblInstructions.Visible = true;
//                BuildLevelsListBox();
//                BuildLevelsContextmenu();
//                lvNodes.ContextMenuStrip = cmsResults;
//                lvNodes.SmallImageList = MIDGraphics.ImageList;
//                clbLevel.ContextMenuStrip = cmsLevels;
//                pnlCharacteristics.ContextMenuStrip = cmsCharPanel;

//                lvLookIn.SmallImageList = MIDGraphics.ImageList;
//                BuildLookInListBox();

//                txtEdit.Size = new System.Drawing.Size(0, 0);
//                txtEdit.Location = new System.Drawing.Point(0, 0);
//                txtEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.EditOver);
//                txtEdit.LostFocus += new System.EventHandler(this.FocusOver);
//                txtEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

//                txtEdit.BorderStyle = BorderStyle.Fixed3D;
//                txtEdit.Hide();
//                txtEdit.Text = "";

//                //_dlFilter = new ProductFilterData();

//                //FilterDef = new ProductSearchFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator), 0);

//                //PanelTag panelTag;
//                //panelTag = new PanelTag(typeof(ProdCharQuerySpacerOperand), btnOr, ((ProductSearchFilterDefinition)FilterDef).CharacteristicOperandList);
//                //panelTag.AllowedDropTypes.Add(typeof(GenericQueryOperand));
//                //panelTag.AllowedDropTypes.Add(typeof(DataQueryNotOperand));
   
//                //panelTag.AllowedDropTypes.Add(typeof(ProductCharacteristicClipboardList));
//                //panelTag.AllowedClipboardProfileTypes.Add(eProfileType.ProductCharacteristicValue);


//                //pnlCharacteristicQuery.Tag = panelTag;
//                //btnAnd.Tag = typeof(GenericQueryAndOperand);
//                //btnOr.Tag = typeof(GenericQueryOrOperand);
//                //btnLParen.Tag = typeof(GenericQueryLeftParenOperand);
//                //btnRParen.Tag = typeof(GenericQueryRightParenOperand);
//                //btnNot.Tag = typeof(DataQueryNotOperand);

//                //CurrentPanel = pnlCharacteristicQuery;

//                //((PanelTag)pnlCharacteristicQuery.Tag).OperandArray = ((ProductSearchFilterDefinition)FilterDef).CharacteristicOperandList;

//                //RedrawOperands(pnlCharacteristicQuery);

//                _selectableColumns = new ArrayList();
//                _productCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics();

//                BuildColumns();
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void BuildPanelTag()
//        {
//            try
//            {
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void SetText()
//        {
//            this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
//            btnSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
//            btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
//            btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
//            lblInstructions.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchInstructions);
//            lblCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.msg_SearchCriteria);
//            lblID.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchID);
//            lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchName);
//            lblDescription.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchDescription);
//            lblLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchLevels);
//            gbxOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchOptions);
//            chkMatchCase.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchCase);
//            chkMatchWholeWord.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SearchMatchWholeWord);
//            colID.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Node_ID).Replace(":", "");
//            colName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Name).Replace(":", "");
//            colDescription.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Description).Replace(":", "");
//            colLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Node_Level);
//            btnCharacteristics.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Edit);
//            chkCharacteristics.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_UseCharacteristics);
//        }

//        private void BuildContextmenu()
//        {
//            try
//            {
//                cmiCut.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Cut);
//                cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Copy);
//                cmiDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
//                cmiRename.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Rename);
//                cmiLocate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Locate);
//                cmiSelectAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAllEntries);
//                cmiColChooser.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ColumnChooser) + "...";
//                cmiCut.Image = MIDGraphics.GetImage(MIDGraphics.CutImage);
//                cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
//                cmiDelete.Image = MIDGraphics.GetImage(MIDGraphics.DeleteImage);
//                //cmiRename.Image = MIDGraphics.GetImage(MIDGraphics.RenameImage);
//                cmiLocate.Image = MIDGraphics.GetImage(MIDGraphics.FindImage);
//                cmiCut.Click += new EventHandler(cmiCut_Click);
//                cmiCopy.Click += new EventHandler(cmiCopy_Click);
//                cmiDelete.Click += new EventHandler(cmiDelete_Click);
//                cmiRename.Click += new EventHandler(cmiRename_Click);
//                cmiLocate.Click += new EventHandler(cmiLocate_Click);

//                cmiCharClearAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ClearAllEntries);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void BuildLookInListBox()
//        {
//            try
//            {
//                string folderColor;
//                lvLookIn.Items.Clear();
//                foreach (SelectedHierarchyNode shn in _nodeList)
//                {
//                    HierarchyLevelProfile hierarchyLevelProfile;

//                    HierarchyProfile hp = SAB.HierarchyServerSession.GetHierarchyData(shn.NodeProfile.HomeHierarchyRID);

//                    if (hp.HierarchyType == eHierarchyType.organizational)
//                    {
//                        if (shn.NodeProfile.HomeHierarchyLevel > 0)
//                        {
//                            hierarchyLevelProfile = (HierarchyLevelProfile)hp.HierarchyLevels[shn.NodeProfile.HomeHierarchyLevel];
//                            folderColor = hierarchyLevelProfile.LevelColor;
//                        }
//                        else
//                        {
//                            folderColor = hp.HierarchyColor;
//                        }
//                    }
//                    else
//                    {
//                        folderColor = hp.HierarchyColor;
//                    }

//                    int imageIndex;
//                    imageIndex = MIDGraphics.ImageIndexWithDefault(folderColor, MIDGraphics.ClosedFolder);
//                    //System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(shn.NodeProfile.Text, imageIndex);
//                    lvLookIn.Items.Add(shn.NodeProfile.Text, imageIndex);
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void BuildLevelsListBox()
//        {
//            try
//            {
//                HierarchyProfile hierProf;
//                int startingLevel = 999;
//                clbLevel.Items.Clear();
//                if (_nodeList.Count > 0)
//                {
//                    foreach (SelectedHierarchyNode shn in _nodeList)
//                    {
//                        switch (shn.NodeType)
//                        {
//                            // Begin Track #5005 - JSmith - Explorer Organization
//                            //case eHierarchyNodeType.MyHierarchyFolder:
//                            case eHierarchySelectType.MyHierarchyFolder:
//                            // End Track #5005
//                                startingLevel = 0;
//                                break;
//                            // Begin Track #5005 - JSmith - Explorer Organization
//                            //case eHierarchyNodeType.OrganizationalHierarchyFolder:
//                            case eHierarchySelectType.OrganizationalHierarchyFolder:
//                            // End Track #5005
//                                startingLevel = 0;
//                                break;
//                            // Begin Track #5005 - JSmith - Explorer Organization
//                            //case eHierarchyNodeType.AlternateHierarchyFolder:
//                            case eHierarchySelectType.AlternateHierarchyFolder:
//                            // End Track #5005
//                                startingLevel = 0;
//                                break;
//                            default:
//                                {
//                                    if (shn.NodeProfile.HomeHierarchyType == eHierarchyType.organizational)
//                                    {
//                                        if (shn.NodeProfile.HomeHierarchyLevel < startingLevel)
//                                        {
//                                            startingLevel = shn.NodeProfile.HomeHierarchyLevel;
//                                        }
//                                    }
//                                    // include all level is alternate because do not know what levels are below
//                                    else
//                                    {
//                                        startingLevel = 0;
//                                    }
//                                    break;
//                                }
//                        }
//                    }
//                }
//                else
//                {
//                    startingLevel = 0;
//                }

//                if (startingLevel < 999)
//                {
//                    hierProf = _SAB.HierarchyServerSession.GetMainHierarchyData();
//                    for (int i = startingLevel; i <= hierProf.HierarchyLevels.Count; i++)
//                    {
//                        if (i == 0)
//                        {
//                            clbLevel.Items.Add(new MIDListBoxItem(i, hierProf.HierarchyID), true);
//                        }
//                        else
//                        {
//                            HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
//                            clbLevel.Items.Add(new MIDListBoxItem(i, hlp.LevelID), true);
//                        }
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void BuildLevelsContextmenu()
//        {
//            try
//            {
//                cmiLevelSelectAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAllEntries);
//                cmiLevelClearAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ClearAllEntries);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void BuildColumns()
//        {
//            try
//            {
//                MIDProductSearchColProfile searchColProfile;

//                // add all columns to list
//                searchColProfile = new MIDProductSearchColProfile(Convert.ToInt32(eMIDTextCode.lbl_Merchandise), eMIDProductSearchColType.MIDText, colMerchandise, eMIDTextCode.lbl_Merchandise);
//                colMerchandise.Tag = searchColProfile;
//                colMerchandise.Name = "txt.Merchandise";
//                _selectableColumns.Add(new RowColProfileHeader(MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise), false, -1, searchColProfile));


//                searchColProfile = new MIDProductSearchColProfile(Convert.ToInt32(eMIDTextCode.lbl_Node_ID), eMIDProductSearchColType.MIDText, colID, eMIDTextCode.lbl_Node_ID);
//                colID.Tag = searchColProfile;
//                colID.Name = "txt.ID";
//                _selectableColumns.Add(new RowColProfileHeader(MIDText.GetTextOnly(eMIDTextCode.lbl_Node_ID), false, -1, searchColProfile));

//                searchColProfile = new MIDProductSearchColProfile(Convert.ToInt32(eMIDTextCode.lbl_Name), eMIDProductSearchColType.MIDText, colName, eMIDTextCode.lbl_Name);
//                colName.Tag = searchColProfile;
//                colName.Name = "txt.Name";
//                _selectableColumns.Add(new RowColProfileHeader(MIDText.GetTextOnly(eMIDTextCode.lbl_Name), false, -1, searchColProfile));

//                searchColProfile = new MIDProductSearchColProfile(Convert.ToInt32(eMIDTextCode.lbl_Description), eMIDProductSearchColType.MIDText, colDescription, eMIDTextCode.lbl_Description);
//                colDescription.Tag = searchColProfile;
//                colDescription.Name = "txt.Description";
//                _selectableColumns.Add(new RowColProfileHeader(MIDText.GetTextOnly(eMIDTextCode.lbl_Description), false, -1,  searchColProfile));

//                searchColProfile = new MIDProductSearchColProfile(Convert.ToInt32(eMIDTextCode.lbl_OTS_Node_Level), eMIDProductSearchColType.MIDText, colLevel, eMIDTextCode.lbl_OTS_Node_Level);
//                colLevel.Tag = searchColProfile;
//                colLevel.Name = "txt.OTSNodeLevel";
//                _selectableColumns.Add(new RowColProfileHeader(MIDText.GetTextOnly(eMIDTextCode.lbl_OTS_Node_Level), false, -1, searchColProfile));

//                ColumnHeader colHeader;
//                foreach (ProductCharProfile productCharProf in _productCharProfileList)
//                {
//                    colHeader = new ColumnHeader();
//                    colHeader.Name = "char." + productCharProf.Text;
//                    colHeader.Text = productCharProf.Text;
//                    colHeader.Width = 150;
//                    searchColProfile = new MIDProductSearchColProfile(productCharProf.Key, eMIDProductSearchColType.Characteristic, colHeader, productCharProf);
//                    colHeader.Tag = searchColProfile;
//                    _selectableColumns.Add(new RowColProfileHeader(productCharProf.Text, false, -1, searchColProfile));
//                }

//                // apply view
//                //DataTable searchViewDt = _dlFilter.ProductSearchView_Read(SAB.ClientServerSession.UserRID);
//                //if (searchViewDt.Rows.Count == 0)
//                //{
//                    BuildDefaultView();
//                //}
//                //else
//                //{
//                //    int sequence = 0;
//                //    foreach (DataRow dr in searchViewDt.Rows)
//                //    {
//                //        ++sequence;
//                //        eMIDProductSearchColType dataType = (eMIDProductSearchColType)Convert.ToInt32(dr["DATA_TYPE"], CultureInfo.CurrentUICulture);
//                //        int dataKey = Convert.ToInt32(dr["DATA_KEY"], CultureInfo.CurrentUICulture);
//                //        int colWidth = Convert.ToInt32(dr["COLUMN_WIDTH"], CultureInfo.CurrentUICulture);
//                //        SetColumnValues(dataType, dataKey, true, sequence, colWidth);
//                //    }
//                //}

//                BuildColumnsInListView();
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void BuildDefaultView()
//        {
//            try
//            {
//                int sequence = 0;

//                SetColumnValues(eMIDProductSearchColType.MIDText, Convert.ToInt32(eMIDTextCode.lbl_Merchandise), true, ++sequence, -1);

//                SetColumnValues(eMIDProductSearchColType.MIDText, Convert.ToInt32(eMIDTextCode.lbl_Node_ID), true, ++sequence, -1);

//                SetColumnValues(eMIDProductSearchColType.MIDText, Convert.ToInt32(eMIDTextCode.lbl_Name), true, ++sequence, -1);

//                SetColumnValues(eMIDProductSearchColType.MIDText, Convert.ToInt32(eMIDTextCode.lbl_Description), true, ++sequence, -1);

//                SetColumnValues(eMIDProductSearchColType.MIDText, Convert.ToInt32(eMIDTextCode.lbl_OTS_Node_Level), true, ++sequence, -1);
				
//                foreach (ProductCharProfile productCharProf in _productCharProfileList)
//                {
//                    SetColumnValues(eMIDProductSearchColType.Characteristic, productCharProf.Key, false, ++sequence, -1);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void SetColumnValues(eMIDProductSearchColType aMIDProductSearchColType, int aKey, 
//            bool aIsDisplayed, int aSequence, int aColWidth)
//        {
//            try
//            {
//                foreach (RowColProfileHeader pchp in _selectableColumns)
//                {
//                    MIDProductSearchColProfile searchProfile = (MIDProductSearchColProfile)pchp.Profile;
//                    if (searchProfile.ColType == aMIDProductSearchColType &&
//                        searchProfile.Key == aKey)
//                    {
//                        pchp.IsDisplayed = aIsDisplayed;
//                        pchp.Sequence = aSequence;
//                        if (aColWidth > -1)
//                        {
//                            searchProfile.ColumnHeader.Width = aColWidth;
//                        }
//                        break;
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void BuildColumnsInListView()
//        {
//            try
//            {
//                SortedList sortedColumns;
//                CreateSortedList(_selectableColumns, out sortedColumns);

//                lvNodes.Columns.Clear();
//                foreach (DictionaryEntry colEntry in sortedColumns)
//                {
//                    RowColProfileHeader pchp = (RowColProfileHeader)colEntry.Value;
//                    if (pchp.IsDisplayed)
//                    {
//                        MIDProductSearchColProfile searchProfile = (MIDProductSearchColProfile)pchp.Profile;
//                        lvNodes.Columns.Add(searchProfile.ColumnHeader);
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void CreateSortedList(ArrayList aSelectableList, out SortedList aSortedList)
//        {
//            SortedList sortList;
//            IDictionaryEnumerator enumerator;
//            int i, j;
//            int newCols;

//            try
//            {
//                sortList = new SortedList();
//                newCols = 0;

//                for (i = 0; i < aSelectableList.Count; i++)
//                {
//                    if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
//                    {
//                        if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
//                        {
//                            newCols++;
//                            ((RowColProfileHeader)aSelectableList[i]).Sequence = newCols * -1;
//                        }
//                        sortList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
//                    }
//                    else
//                    {
//                        ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
//                    }
//                }

//                enumerator = sortList.GetEnumerator();
//                j = 0;

//                while (enumerator.MoveNext())
//                {
//                    if (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) < 0)
//                    {
//                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = sortList.Count - newCols + (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) * -1) - 1;
//                    }
//                    else
//                    {
//                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
//                        j++;
//                    }
//                }

//                aSortedList = new SortedList();

//                foreach (RowColProfileHeader rowColHeader in aSelectableList)
//                {
//                    if (rowColHeader.IsDisplayed)
//                    {
//                        aSortedList.Add(rowColHeader.Sequence, rowColHeader);
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void cmiColChooser_Click(object sender, EventArgs e)
//        {
//            RowColChooser frm;

//            try
//            {
//// Begin Track #4868 - JSmith - Variable Groupings
//                //frm = new RowColChooser(_selectableColumns, true, MIDText.GetTextOnly(eMIDTextCode.lbl_ColumnChooser), true);
//                frm = new RowColChooser(_selectableColumns, true, MIDText.GetTextOnly(eMIDTextCode.lbl_ColumnChooser), true, null);
//// End Track #4868

//                if (frm.ShowDialog() == DialogResult.OK)
//                {
//                    try
//                    {
//                        Cursor.Current = Cursors.WaitCursor;
//                        lvNodes.ListViewItemSorter = null;
//                        lvNodes.BeginUpdate();
//                        lvNodes.Items.Clear();
//                        BuildColumnsInListView();
//                        foreach (MIDProductSearchProductData searchData in _results.Values)
//                        {
//                            AddItem(searchData.Key);
//                        }
//                        foreach (ColumnHeader colHeader in lvNodes.Columns)
//                        {
//                            if (colHeader.Name == sortColumnName)
//                            {
//                                sortColumn = colHeader.DisplayIndex;
//                                lvNodes.ListViewItemSorter = new ListViewItemComparer(sortColumn, lvNodes.Sorting);
//                                break;
//                            }
//                        }
//                        lvNodes.EndUpdate();
//                    }
//                    catch (Exception exc)
//                    {
//                        HandleException(exc);
//                    }
//                    finally
//                    {
//                        Cursor.Current = Cursors.Default;
//                    }
//                }

//                frm.Dispose();
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex);
//            }
//        }

//        void cmiLocate_Click(object sender, EventArgs e)
//        {
//            foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//            {
//                MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                MerchandiseNodeLocateEvent(this, new MerchandiseNodeLocateEventArgs(itemTag.Key));
//            }
//        }

//        void cmiRename_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                EditText();
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        void cmiDelete_Click(object sender, EventArgs e)
//        {
//            ArrayList nodesToDelete = new ArrayList();
//            try
//            {
//                HierarchyMaintenance hm = new HierarchyMaintenance(_SAB);
//                frmProgress progress = new frmProgress(0, 0);
//                progress.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
//                _continueProcessing = true;
//                progress.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_LockingForDelete);
//                progress.Show();

//                string text = null;
//                ArrayList nodesInUse = new ArrayList();
//                bool performDelete = false;
//                bool performRemove = false;
//                NodeLockRequestList lockRequestList = new NodeLockRequestList(eProfileType.NodeLockRequest);
//                int deleteTotal = 0;


//                foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//                {
//                    MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                    if (itemTag.FunctionSecurity.AllowDelete &&
//                        itemTag.NodeSecurity.AllowDelete)
//                    {
//                        NodeLockRequestProfile lockProfile = new NodeLockRequestProfile(itemTag.Key);
//                        lockProfile.HierarchyRID = itemTag.HomeHierarchyRID;
//                        lockRequestList.Add(lockProfile);
//                    }
//                }

//                if (lockRequestList.Count == 0)
//                {
//                    return;
//                }

//                NodeLockConflictList conflictList = _SAB.HierarchyServerSession.LockHierarchyBranchForDelete(lockRequestList);

//                foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//                {
//                    MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                    if (conflictList.ContainsBranchKey(itemTag.Key))
//                    {
//                        nodesInUse.Add(item);
//                    }
//                    else
//                    {
//                        nodesToDelete.Add(item);
//                        performDelete = true;
//                        deleteTotal += _SAB.HierarchyServerSession.GetDescendantCount(itemTag.HomeHierarchyRID, itemTag.Key) + 1;
//                    }
//                }

//                bool messageDisplayed = false;
//                // determine message
//                if (nodesInUse.Count > 0)
//                {
//                    messageDisplayed = true;
//                    text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeLockConflictHeading, false) + System.Environment.NewLine;
//                    foreach (NodeLockConflictProfile conflictProfile in conflictList)
//                    {
//                        // Begin TT#1159 - APicchetti - Improve Messaging
//                        string[] errParms = new string[3];
//                        errParms.SetValue("Node Lock Conflict", 0);
//                        errParms.SetValue(conflictProfile.InUseNodeName.Trim(), 1);
//                        errParms.SetValue(conflictProfile.InUseByUserName, 2);
//                        string newConflict = MIDText.GetText(eMIDTextCode.msg_StandardInUseMsg, errParms);
//                        //string newConflict = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NodeLockConflictUser, false);
//                        //newConflict = newConflict.Replace("{0}", conflictProfile.InUseNodeName);
//                        //newConflict = newConflict.Replace("{1}", conflictProfile.InUseByUserName);
//                        // End TT#1159 - APicchetti - Improve Messaging

//                        text += System.Environment.NewLine + newConflict;
//                    }
//                    text += System.Environment.NewLine;
//                    if (nodesToDelete.Count == 0)
//                    {
//                        text += System.Environment.NewLine + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AllNodesInUse, false);
//                        if (MessageBox.Show(text, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
//                        == DialogResult.OK)
//                        {
//                            return;
//                        }
//                    }
//                    else
//                    {
//                        foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//                        {
//                            MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                            string nodeMsg = null;
//                            if (nodesInUse.Contains(item))
//                            {
//                                nodeMsg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ItemWillNotBeDeleted, false);
//                            }
//                            else
//                            {
//                                nodeMsg = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ItemWillBeDeleted, false);
//                            }
//                            nodeMsg = nodeMsg.Replace("{0}", item.Text);
//                            text += System.Environment.NewLine + nodeMsg;
//                        }
//                        text += System.Environment.NewLine + System.Environment.NewLine + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ContinuePartialDelete, false);
//                        if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                        == DialogResult.No)
//                        {
//                            return;
//                        }
//                    }
//                }
//                else if (lockRequestList.Count != lvNodes.SelectedItems.Count)
//                {
//                    if (lvNodes.SelectedItems.Count == 1)
//                    {
//                        text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorized, false);
//                    }
//                    else
//                    {
//                        if (performRemove && performDelete)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToDeleteRemoveAllNodes, false);
//                        }
//                        else if (performRemove)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToRemoveAllNodes, false);
//                        }
//                        else
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToDeleteAllNodes, false);
//                        }
//                    }
//                }
//                else if (nodesToDelete.Count > 0)
//                {
//                    if (nodesToDelete.Count == 1)
//                    {
//                        if (performRemove)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false);
//                            text = text.Replace("{0}", lvNodes.SelectedItems[0].Text);
//                        }
//                        else if (performDelete)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItem, false);
//                            text = text.Replace("{0}", lvNodes.SelectedItems[0].Text);
//                        }
//                    }
//                    else
//                    {
//                        if (performRemove && performDelete)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteRemoveItems, false);
//                        }
//                        else if (performRemove)
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItems, false);
//                        }
//                        else
//                        {
//                            text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteItems, false);
//                        }
//                        text = text.Replace("{0}", nodesToDelete.Count.ToString());
//                    }
//                }

//                _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, text, this.GetType().Name);
//                if (!messageDisplayed)
//                {
//                    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                        == DialogResult.No)
//                    {
//                        return;
//                    }
//                }

//                progress.SetMaxValue = deleteTotal;
//                //bool deleteSuccessful = false;
//                int count = 0;
//                int successfulCount = 0;
//                // perform delete
//                string deleteMsg = MIDText.GetText(eMIDTextCode.msg_DeleteStatus);
//                deleteMsg = deleteMsg.Replace("{1}", deleteTotal.ToString(CultureInfo.CurrentUICulture));
//                progress.SetMaxValue = deleteTotal;


//                _continueProcessing = true;
//                progress.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
//                EditMsgs em = new EditMsgs();
//                foreach (ListViewItem item in nodesToDelete)
//                {
//                    MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                    if (!_continueProcessing)
//                    {
//                        progress.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteCancelled);
//                        break;
//                    }
//                    else
//                    {
//                        progress.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
//                        _continueProcessing = true;
//                        HierarchyNodeList hierarchyChildrenList = _SAB.HierarchyServerSession.GetHierarchyChildren(itemTag.HierarchyLevel, itemTag.HomeHierarchyRID, itemTag.HomeHierarchyRID, itemTag.Key, false, eNodeSelectType.All);
//                        foreach (HierarchyNodeProfile hnp in hierarchyChildrenList) // delete descendants
//                        {
//                            if (_progressCancelClicked)
//                            {
//                                _continueProcessing = false;
//                                break;
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    DeleteDescendants(ref em, deleteMsg, ref count, ref successfulCount,
//                                        deleteTotal, itemTag.Key, hnp, progress, hm);
//                                }
//                                catch // discontinue if error
//                                {
//                                    _continueProcessing = false;
//                                    break;
//                                }
//                            }
//                        }
//                        //delete parent
//                        if (!_continueProcessing)
//                        {
//                            progress.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteCancelled);
//                        }
//                        else
//                        {
//                            ++count;
//                            progress.SetValue = count;
//                            progress.labelText = deleteMsg.Replace("{0}", count.ToString(CultureInfo.CurrentUICulture));
//                            HierarchyNodeProfile selectedNode = null;
//                            try
//                            {
//                                selectedNode = _SAB.HierarchyServerSession.GetNodeData(itemTag.Key);
//                                DeleteNode(ref em, selectedNode.HierarchyRID, selectedNode.Key, selectedNode.HomeHierarchyRID,
//                                    selectedNode.NodeLevel, selectedNode.HomeHierarchyParentRID, hm);
//                                ++successfulCount;
//                            }
//                            catch (DatabaseForeignKeyViolation)
//                            {
//                                // condition is already handled in hierarchy maintenance
//                            }
//                            catch
//                            {
//                            }
//                            if (!em.ErrorFound)
//                            {
//                                lvNodes.Items.Remove(item);
//                                MerchandiseNodeDeleteEvent(this, new MerchandiseNodeDeleteEventArgs(selectedNode.HomeHierarchyParentRID, selectedNode.Key));
//                            }
//                        }
//                    }
//                }

//                progress.SetValue = deleteTotal;
//                if (count == successfulCount)
//                {
//                    string message = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete);
//                    progress.labelText = message;
//                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name);
//                }
//                else
//                {
//                    if (em.ErrorFound)
//                    {
//                        DisplayMessages(em);
//                    }
//                    text = MIDText.GetTextOnly(eMIDTextCode.msg_PartialDelete);
//                    text = text.Replace("{0}", successfulCount.ToString(CultureInfo.CurrentUICulture));
//                    text = text.Replace("{1}", deleteTotal.ToString(CultureInfo.CurrentUICulture));
//                    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, text, this.GetType().Name);
//                    progress.labelText = text;
//                }

//                progress.EnableOKButton();

//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                // remove locks
//                foreach (ListViewItem item in nodesToDelete)
//                {
//                    MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                    _SAB.HierarchyServerSession.DequeueBranch(itemTag.HomeHierarchyRID, itemTag.Key);
//                }
//            }
//        }

//        private void DeleteDescendants(ref EditMsgs em, string deleteMsg, ref int count, 
//            ref int successfulCount, int deleteTotal, int parentRID, HierarchyNodeProfile delete_hnp,
//            frmProgress aProgress, HierarchyMaintenance aHierarchyMaintenance)
//        {
//            try
//            {
//                if (delete_hnp.HierarchyRID == delete_hnp.HomeHierarchyRID)		// only delete children if node is home
//                {
//                    HierarchyNodeList hierarchyChildrenList = _SAB.HierarchyServerSession.GetHierarchyChildren(delete_hnp.NodeLevel, delete_hnp.HierarchyRID, delete_hnp.HomeHierarchyRID, delete_hnp.Key, false, eNodeSelectType.All);
//                    if (hierarchyChildrenList.Count > 0)
//                    {
//                        foreach (HierarchyNodeProfile hnp in hierarchyChildrenList)
//                        {
//                            DeleteDescendants(ref em, deleteMsg, ref count, ref successfulCount, deleteTotal,
//                                delete_hnp.Key, hnp, aProgress, aHierarchyMaintenance);
//                        }
//                    }
//                }
//                if (_continueProcessing)
//                {
//                    ++count;
//                    aProgress.SetValue = count;
//                    aProgress.labelText = deleteMsg.Replace("{0}", count.ToString(CultureInfo.CurrentUICulture));

//                    try
//                    {
//                        DeleteNode(ref em, delete_hnp.HierarchyRID, delete_hnp.Key, delete_hnp.HomeHierarchyRID,
//                            delete_hnp.NodeLevel, delete_hnp.HomeHierarchyParentRID, aHierarchyMaintenance);
//                        ++successfulCount;
//                    }
//                    catch (DatabaseForeignKeyViolation)
//                    {
//                        // condition is already handled in hierarchy maintenance
//                    }
//                    catch (MIDDatabaseUnavailableException err)
//                    {
//                        string exceptionMessage = err.Message;
//                        _continueProcessing = false;
//                    }
//                    catch (Exception err)
//                    {
//                        string exceptionMessage = err.Message;
//                        string errors = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorDeleting, false);
//                        errors = errors.Replace("{0}", delete_hnp.LevelText);
//                        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errors, this.GetType().Name);
//                        errors += Environment.NewLine + _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FollowingErrors);
//                        for (int i = 0; i < em.EditMessages.Count; i++)
//                        {
//                            EditMsgs.Message emm = (EditMsgs.Message)em.EditMessages[i];
//                            errors += Environment.NewLine + "     ";
//                            if (emm.messageByCode)
//                            {
//                                errors += _SAB.ClientServerSession.Audit.GetText(emm.code);
//                            }
//                            else
//                            {
//                                errors += emm.msg;
//                            }
//                        }
//                        errors += _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ContinueQuestion);
//                        if (MessageBox.Show(errors, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//                            == DialogResult.No)
//                        {
//                            _continueProcessing = false;
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex);
//            }
//        }

//        private void DeleteNode(ref EditMsgs em, int hierarchyRID, int nodeRID, int homeHierarchyRID,
//            int nodeLevel, int parentRID, HierarchyMaintenance aHierarchyMaintenance)
//        {
//            try
//            {
//                em.ClearMsgs();
//                if (hierarchyRID != homeHierarchyRID)  // delete reference
//                {
//                    HierarchyJoinProfile hjp = new HierarchyJoinProfile(-1);
//                    hjp.JoinChangeType = eChangeType.delete;
//                    hjp.OldHierarchyRID = hierarchyRID;
//                    hjp.OldParentRID = parentRID;
//                    hjp.Key = nodeRID;
//                    HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(hierarchyRID);
//                    if (hp.HierarchyType == eHierarchyType.organizational)
//                    {
//                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)hp.HierarchyLevels[nodeLevel];
//                        hjp.LevelType = hlp.LevelType;
//                    }
//                    _SAB.HierarchyServerSession.JoinUpdate(hjp);
//                }
//                else
//                    if (nodeLevel == 0)   // delete hierarchy
//                    {
//                        HierarchyProfile hp = new HierarchyProfile(hierarchyRID);
//                        hp.HierarchyChangeType = eChangeType.delete;
//                        aHierarchyMaintenance.ProcessHierarchyData(ref em, hp);
//                    }
//                    else
//                    {
//                        HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(nodeRID);
//                        hnp.NodeChangeType = eChangeType.delete;
//                        hnp.HierarchyRID = hierarchyRID;
//                        hnp.HomeHierarchyParentRID = parentRID;
//                        if (!hnp.Parents.Contains(parentRID))
//                        {
//                            hnp.Parents.Add(parentRID);
//                        }
//                        aHierarchyMaintenance.ProcessNodeProfileInfo(ref em, hnp);
//                    }
//            }
//            catch (DatabaseForeignKeyViolation)
//            {
//                // condition is already handled in hierarchy maintenance
//                throw;
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex);
//            }
//        }

//        void cmiCopy_Click(object sender, EventArgs e)
//        {
//            //try
//            //{
//            //    CopyHierarchyNodesToClipboard(lvNodes, eProfileType.HierarchyNode, DragDropEffects.Copy);
//            //    // Begin TT#564 - JSmith - Copy/Paste from search not working
//            //    _EAB.MerchandiseExplorer.SetCutCopyFromClipboard(eCutCopyOperation.Copy);
//            //    // End TT#564
//            //}
//            //catch
//            //{
//            //    throw;
//            //}
//            PerformCopy();
//        }

//        void PerformCopy()
//        {
//            try
//            {
//                CopyHierarchyNodesToClipboard(lvNodes, eProfileType.HierarchyNode, DragDropEffects.Copy);
//                _EAB.MerchandiseExplorer.SetCutCopyFromClipboard(eCutCopyOperation.Copy);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        void cmiCut_Click(object sender, EventArgs e)
//        {
//            //try
//            //{
//            //    CopyHierarchyNodesToClipboard(lvNodes, eProfileType.HierarchyNode, DragDropEffects.Move);
//            //    // Begin TT#564 - JSmith - Copy/Paste from search not working
//            //    _EAB.MerchandiseExplorer.SetCutCopyFromClipboard(eCutCopyOperation.Cut);
//            //    // End TT#564
//            //}
//            //catch
//            //{
//            //    throw;
//            //}
//            PerformCut();
//        }

//        void PerformCut()
//        {
//            try
//            {
//                CopyHierarchyNodesToClipboard(lvNodes, eProfileType.HierarchyNode, DragDropEffects.Move);
//                _EAB.MerchandiseExplorer.SetCutCopyFromClipboard(eCutCopyOperation.Cut);
//            }
//            catch
//            {
//                throw;
//            }
//        }


//        private void btnSearch_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                if (_searching)
//                {
//                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//                    //_thread.Abort();
//                    searchEngine.RequestStop();
//                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//                    // wait for thread to exit
//                    _thread.Join();
//                    btnSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
//                    _searching = false;
//                    progressBar1.Visible = false;
//                    lblSearching.Visible = false;
//                    lvNodes.AllowColumnReorder = true;
//                }
//                else
//                {
//                    btnSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Stop);
//                    lblInstructions.Visible = false;
//                    pnlSearch.Visible = true;
//                    pnlSearch.BringToFront();
//                    lvNodes.AllowColumnReorder = false;
//                    lvNodes.Items.Clear();
//                    lvNodes.Visible = true;
//                    lvNodes.Dock = DockStyle.Fill;
//                    lvNodes.View = View.Details;
//                    _searching = true;
//                    _results.Clear();

//                    Hashtable levels = new Hashtable();
//                    MIDListBoxItem mlbi;

//                    for (int i = 0; i < clbLevel.Items.Count; i++)
//                    {
//                        if (clbLevel.GetItemChecked(i))
//                        {
//                            mlbi = (MIDListBoxItem)clbLevel.Items[i];
//                            levels.Add(mlbi.Key, null);
//                        }
//                    }

//                    progressBar1.Visible = true;
//                    lblSearching.Visible = true;
//                    progressBar1.Value = 0;
//                    lblSearching.Text = string.Empty;
//                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//                    //ProductSearchEngine searchEngine = new ProductSearchEngine(_SAB, _nodeList, _wildcard,
//                    //    _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
//                    //    txtID.Text, txtName.Text, txtDescription.Text, levels,
//                    //    chkCharacteristics.Checked, (ProductSearchFilterDefinition)FilterDef);
//                    searchEngine = new ProductSearchEngine(_SAB, _nodeList, _wildcard, 
//                        _separator, chkMatchCase.Checked, chkMatchWholeWord.Checked,
//                        txtID.Text, txtName.Text, txtDescription.Text, levels,
//                        chkCharacteristics.Checked);
//                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//                    searchEngine.MIDSearchEvent +=new MIDSearchEventHandler(searchEngine_MIDSearchEvent);
//                    searchEngine.MIDSearchCompletedEvent += new MIDSearchCompletedEventHandler(searchEngine_MIDSearchCompletedEvent);
//                    _thread = new Thread(new ThreadStart(searchEngine.GetSearchResults));
//                    _thread.Start();
//                    // Begin TT#438-MD - JSmith - Merchandise Search Stop Locks Client
//                    // Loop until worker thread activates.
//                    while (!_thread.IsAlive) ;
//                    // End TT#438-MD - JSmith - Merchandise Search Stop Locks Client

//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }


        


//        void searchEngine_MIDSearchCompletedEvent(object sender, MIDSearchCompletedEventArgs e)
//        {
//            try
//            {
//                if (btnSearch.InvokeRequired)
//                {
//                    btnSearch.Invoke(new MIDSearchCompletedEventHandler(searchEngine_MIDSearchCompletedEvent), new object[] { sender, e });
//                }
//                else
//                {
//                    btnSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search);
//                    _searching = false;
//                    progressBar1.Visible = false;
//                    lblSearching.Visible = false;
//                    lvNodes.AllowColumnReorder = true;
//                }

//            }
//            catch
//            {
//                throw;
//            }
//        }

//        void searchEngine_MIDSearchEvent(object sender, MIDSearchEventArgs e)
//        {
//            try
//            {
//                if (_formClosing == false)      //TT#826 - Error produced when closing the hierarchy search while the search is running - apicchetti - 1/19/2011
//                {
//                    MIDProductSearchProductData searchData;
//                    bool addToList = false;
//                    if (lvNodes.InvokeRequired)
//                    {
//                        lvNodes.Invoke(new MIDSearchEventHandler(searchEngine_MIDSearchEvent), new object[] { sender, e });
//                    }
//                    else
//                    {
//                        if (progressBar1.Value >= 100)
//                        {
//                            progressBar1.Value = 0;
//                        }
//                        else
//                        {
//                            progressBar1.Value += 10;
//                        }
//                        MIDProductSearchEventArgs ea = (MIDProductSearchEventArgs)e;
//                        lblSearching.Text = "Searching " + SAB.HierarchyServerSession.GetNodeData(ea.NodeRID, false, true).Text;
//                        Application.DoEvents();
//                        if (ea.Key > Include.NoRID)
//                        {
//                            lock (_results.SyncRoot)
//                            {
//                                HierarchyNodeSecurityProfile nodeSecurity = null;
//                                if (!_results.Contains(ea.Key))
//                                {
//                                    addToList = true;
//                                    nodeSecurity = _SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ea.Key, (int)eSecurityTypes.All);
//                                    searchData = new MIDProductSearchProductData(ea.Key, ea.ID, ea.Name, ea.Description,
//                                        ea.HierarchyRID, ea.HierarchyLevel,
//                                        ea.LevelName, ea.FolderColor, ea.NodeCharProfileList, nodeSecurity);
//                                    _results.Add(ea.Key, searchData);
//                                }

//                                if (addToList)
//                                {
//                                    if (!nodeSecurity.AccessDenied)
//                                    {
//                                        AddItem(ea.Key);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void AddItem(int aNodeRID)
//        {
//            try
//            {
//                MIDProductSearchProductData searchData = (MIDProductSearchProductData)_results[aNodeRID];
//                int imageIndex;
//                imageIndex = MIDGraphics.ImageIndexWithDefault(searchData.FolderColor, MIDGraphics.ClosedFolder);
//                //string[] items = new string[] { searchData.ID, searchData.Name, searchData.Description, searchData.LevelName };
//                System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(BuildItems(aNodeRID), imageIndex);
//                item.Tag = new MIDProductSearchItemTag(searchData.Key, searchData.HierarchyRID, searchData.HierarchyLevel, searchData.NodeSecurity);
//                lvNodes.Items.Add(item);
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private string[] BuildItems(int aNodeRID)
//        {
//            try
//            {
//                string[] items = new string[lvNodes.Columns.Count];
//                MIDProductSearchProductData searchData = (MIDProductSearchProductData)_results[aNodeRID];
//                foreach (ColumnHeader colHeader in lvNodes.Columns)
//                {
//                    string itemValue = string.Empty;
//                    MIDProductSearchColProfile colTag = (MIDProductSearchColProfile)colHeader.Tag;
//                    switch (colTag.ColType)
//                    {
//                        case eMIDProductSearchColType.MIDText:
//                            eMIDTextCode textCode = (eMIDTextCode)Convert.ToInt32(colTag.MIDTextCode);
//                            switch (textCode)
//                            {
//                                case eMIDTextCode.lbl_Merchandise:
//                                    if (searchData.Text == null)
//                                    {
//                                        searchData.Text = SAB.HierarchyServerSession.GetNodeData(searchData.Key, false, true).Text;
//                                    }
//                                    itemValue = searchData.Text;
//                                    break;
//                                case eMIDTextCode.lbl_Node_ID:
//                                    itemValue = searchData.ID;
//                                    break;
//                                case eMIDTextCode.lbl_Name:
//                                    itemValue = searchData.Name;
//                                    break;
//                                case eMIDTextCode.lbl_Description:
//                                    itemValue = searchData.Description;
//                                    break;
//                                case eMIDTextCode.lbl_OTS_Node_Level:
//                                    itemValue = searchData.LevelName;
//                                    break;
//                                default:
//                                    itemValue = "Bad text code";
//                                    break;
//                            }
//                            break;
//                        case eMIDProductSearchColType.Characteristic:
//                            NodeCharProfile ncp = (NodeCharProfile)searchData.NodeCharProfileList.FindKey(colTag.ProductCharProfile.Key);
//                            if (ncp != null)
//                            {
//                                itemValue = ncp.ProductCharValue;
//                            }
//                            break;
//                    }
//                    items[colHeader.DisplayIndex] = itemValue;
//                }
//                return items;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
//        {
//            try
//            {
//                foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//                {
//                    MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                    if (itemTag.FunctionSecurity == null)
//                    {
//                        SetSecurity(item, itemTag);
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void SetSecurity(ListViewItem aItem, MIDProductSearchItemTag aItemTag)
//        {
//            try
//            {
//                HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(aItemTag.HomeHierarchyRID);
//                if (hp.HierarchyType == eHierarchyType.organizational)
//                {
//                    aItemTag.FunctionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aItemTag.Key, eSecurityFunctions.AdminHierarchiesOrgNodes, (int)eSecurityTypes.All);
//                }
//                else if (hp.Owner == Include.GlobalUserRID)	// Issue 3806
//                {
//                    aItemTag.FunctionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aItemTag.Key, eSecurityFunctions.AdminHierarchiesAltNodes, (int)eSecurityTypes.All);
//                }
//                else
//                {
//                    aItemTag.FunctionSecurity = _SAB.ClientServerSession.GetMyUserNodeFunctionSecurityAssignment(aItemTag.Key, eSecurityFunctions.AdminHierarchiesAltUser, (int)eSecurityTypes.All);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_ColumnClick(object sender, ColumnClickEventArgs e)
//        {
//            try
//            {
//                Cursor.Current = Cursors.WaitCursor;
//                if (!_searching)
//                {
//                    // Determine whether the column is the same as the last column clicked.
//                    if (e.Column != sortColumn)
//                    {
//                        // Set the sort column to the new column.
//                        sortColumn = e.Column;
//                        foreach (ColumnHeader colHeader in lvNodes.Columns)
//                        {
//                            if (colHeader.DisplayIndex == e.Column)
//                            {
//                                sortColumnName = colHeader.Name;
//                            }
//                        }
//                        // Set the sort order to ascending by default.
//                        lvNodes.Sorting = SortOrder.Ascending;
//                    }
//                    else
//                    {
//                        // Determine what the last sort order was and change it.
//                        if (lvNodes.Sorting == SortOrder.Ascending)
//                            lvNodes.Sorting = SortOrder.Descending;
//                        else
//                            lvNodes.Sorting = SortOrder.Ascending;
//                    }

//                    // Call the sort method to manually sort.
//                    lvNodes.Sort();
//                    // Set the ListViewItemSorter property to a new ListViewItemComparer
//                    // object.
//                    lvNodes.ListViewItemSorter = new ListViewItemComparer(e.Column,
//                        lvNodes.Sorting);
//                }
//            }
//            catch (Exception exception)
//            {
//                HandleException(exception);
//            }
//            finally
//            {
//                Cursor.Current = Cursors.Default;
//            }
//        }

//        private void EditOver(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            try
//            {
//                MIDProductSearchItemTag itemTag = null;
//                if (e.KeyChar == 13) // return
//                {
//                    itemTag = (MIDProductSearchItemTag)_selectedItem.Tag;

//                    if (_selectedItem.SubItems[subItemSelected].Text != txtEdit.Text)
//                    {
//                        UpdateNode(itemTag, txtEdit.Text);
//                    }
//                }

//                if (e.KeyChar == 27) // Esc
//                {
//                    EndEdit();
//                    //itemTag = (MIDProductSearchItemTag)_selectedItem.Tag;
//                    //txtEdit.Hide();
//                    //if (itemTag.HierarchyLevel == 0)  // hierarchy
//                    //{
//                    //    _SAB.HierarchyServerSession.DequeueHierarchy(itemTag.HomeHierarchyRID);
//                    //}
//                    //else
//                    //{
//                    //    _SAB.HierarchyServerSession.DequeueNode(itemTag.Key);
//                    //}
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void EndEdit()
//        {
//            MIDProductSearchItemTag itemTag = null;
//            itemTag = (MIDProductSearchItemTag)_selectedItem.Tag;
//            txtEdit.Hide();
//            if (itemTag.HierarchyLevel == 0)  // hierarchy
//            {
//                _SAB.HierarchyServerSession.DequeueHierarchy(itemTag.HomeHierarchyRID);
//            }
//            else
//            {
//                _SAB.HierarchyServerSession.DequeueNode(itemTag.Key);
//            }
//        }

//        private void FocusOver(object sender, System.EventArgs e)
//        {
//            MIDProductSearchItemTag itemTag = null;
//            try
//            {
//                itemTag = (MIDProductSearchItemTag)_selectedItem.Tag;
//                if (_selectedItem.SubItems[subItemSelected].Text != txtEdit.Text)
//                {
//                    UpdateNode(itemTag, txtEdit.Text);
//                }
//            }
//            catch
//            {
//                throw;
//            }
//            finally
//            {
//                if (itemTag != null)
//                {
//                    if (itemTag.HierarchyLevel == 0)  // hierarchy
//                    {
//                        _SAB.HierarchyServerSession.DequeueHierarchy(itemTag.HomeHierarchyRID);
//                    }
//                    else
//                    {
//                        _SAB.HierarchyServerSession.DequeueNode(itemTag.Key);
//                    }
//                }
//            }
//        }

//        private void UpdateNode(MIDProductSearchItemTag aItemTag, string aText)
//        {
//            try
//            {
//                if (!_nodeUpdated)
//                {
//                    if (aItemTag.HierarchyLevel == 0)  // hierarchy
//                    {
//                        HierarchyProfile hp = _SAB.HierarchyServerSession.GetHierarchyData(aItemTag.HomeHierarchyRID);
//                        hp.HierarchyID = aText;
//                        hp.HierarchyChangeType = eChangeType.update;
//                        _SAB.HierarchyServerSession.HierarchyUpdate(hp);
//                    }
//                    else
//                    {
//                        EditMsgs em = new EditMsgs();
//                        HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(aItemTag.Key);
//                        hnp.NodeID = aText;
//                        hnp.NodeChangeType = eChangeType.update;
//                        _hierarchyMaintenance.ProcessNodeProfileInfo(ref em, hnp);
//                        if (em.ErrorFound)
//                        {
//                            DisplayMessages(em);
//                        }
//                    }
//                    txtEdit.Hide();
//                    if (aItemTag.HierarchyLevel == 0)  // hierarchy
//                    {
//                        _SAB.HierarchyServerSession.DequeueHierarchy(aItemTag.HomeHierarchyRID);
//                    }
//                    else
//                    {
//                        _SAB.HierarchyServerSession.DequeueNode(aItemTag.Key);
//                    }

//                    _selectedItem.SubItems[subItemSelected].Text = txtEdit.Text;
//                    MerchandiseNodeRenameEvent(this, new MerchandiseNodeRenameEventArgs(aItemTag.Key, aText));
//                    _nodeUpdated = true;
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_DoubleClick(object sender, EventArgs e)
//        {
//            EditText();
//        }

//        private void EditText()
//        {
//            try
//            {
//                _nodeUpdated = false;
//                MIDProductSearchItemTag itemTag = null;
//                HierarchyNodeProfile hnp = null;
//                HierarchyProfile hp = null;
//                eLockStatus lockStatus = eLockStatus.Undefined;
//                // Check the subitem clicked .
//                int nStart = _XPos;
//                int spos = 20;
//                int epos = lvNodes.Columns[0].Width;
//                for (int i = 0; i < lvNodes.Columns.Count; i++)
//                {
//                    if (nStart > spos && nStart < epos)
//                    {
//                        subItemSelected = i;
//                        break;
//                    }

//                    spos = epos;
//                    epos += lvNodes.Columns[i].Width;
//                }

//                //			Console.WriteLine("SUB ITEM SELECTED = " + _selectedItem.SubItems[subItemSelected].Text);
//                subItemText = _selectedItem.SubItems[subItemSelected].Text;

//                string colName = lvNodes.Columns[subItemSelected].Text;
//                if (colName == "ID")
//                {
//                    itemTag = (MIDProductSearchItemTag)_selectedItem.Tag;
//                    lockStatus = eLockStatus.Undefined;

//                    if (itemTag.HierarchyLevel == 0)
//                    {
//                        hp = _SAB.HierarchyServerSession.GetHierarchyDataForUpdate(itemTag.HomeHierarchyRID, false);
//                        lockStatus = hp.HierarchyLockStatus;
//                    }
//                    else
//                    {
//                        hnp = _SAB.HierarchyServerSession.GetNodeDataForUpdate(itemTag.Key, false);
//                        lockStatus = hnp.NodeLockStatus;
//                    }

//                    if (lockStatus == eLockStatus.Locked)
//                    {
//                        Rectangle r = new Rectangle(spos, _selectedItem.Bounds.Y, epos, _selectedItem.Bounds.Bottom);
//                        txtEdit.Size = new System.Drawing.Size(epos - spos, _selectedItem.Bounds.Bottom - _selectedItem.Bounds.Top);
//                        txtEdit.Location = new System.Drawing.Point(spos, _selectedItem.Bounds.Y);
//                        txtEdit.Show();
//                        txtEdit.Text = subItemText;
//                        txtEdit.SelectAll();
//                        txtEdit.Focus();
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_MouseDown(object sender, MouseEventArgs e)
//        {
//            try
//            {
//                if (txtEdit.Visible)
//                {
//                    EndEdit();
//                }

//                bool bControl = (ModifierKeys == Keys.Control);
//                bool bShift = (ModifierKeys == Keys.Shift);
//                bool selectItem = false;

//                if (e.Button == MouseButtons.Left)
//                {
//                    selectItem = true;
//                }
//                else if (e.Button == MouseButtons.Right &&
//                    !bControl &&
//                    !bShift)
//                {
//                    selectItem = true;
//                }

//                if (selectItem)
//                {
//                    _selectedItem = lvNodes.GetItemAt(e.X, e.Y);
//                    _XPos = e.X;
//                    _YPos = e.Y;
//                }
//                _mouseDown = true;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_MouseUp(object sender, MouseEventArgs e)
//        {
//            _mouseDown = false;
//        }

//        private void lvNodes_MouseMove(object sender, MouseEventArgs e)
//        {
//            HierarchyNodeClipboardList hnList;
//            try
//            {
//                if (_mouseDown &&
//                    lvNodes.SelectedItems.Count > 0)
//                {
//                    hnList = CopyHierarchyNodesToClipboard(lvNodes, eProfileType.HierarchyNode, DragDropEffects.Move);
//                    int xPos, yPos;
//                    int imageHeight, imageWidth;
//                    HierarchyTreeView tempTreeView = new HierarchyTreeView();
//                    tempTreeView.InitializeTreeView(_SAB, false, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
//                    MIDGraphics.BuildDragImage(hnList, ImageListDrag, tempTreeView.Indent, Spacing,
//                                    Font, ForeColor, out imageHeight, out imageWidth);

//                    xPos = imageWidth / 2;
//                    yPos = imageHeight / 2;

//                    // Begin dragging image
//                    if (DragHelper.ImageList_BeginDrag(ImageListDrag.Handle, 0, xPos, yPos))
//                    {
//                        // Begin dragging
//                        lvNodes.DoDragDrop(hnList, DragDropEffects.Move);
//                        // End dragging image
//                        DragHelper.ImageList_EndDrag();
//                    }
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void lvNodes_DragOver(object sender, DragEventArgs e)
//        {
//            Image_DragOver(sender, e);
//        }

//        private void lvNodes_DragEnter(object sender, DragEventArgs e)
//        {
//            Image_DragEnter(sender, e);
//        }

//        private void lvNodes_DragLeave(object sender, EventArgs e)
//        {
//            Image_DragLeave(sender, e);
//        }

//        private void cmiSelectAll_Click(object sender, EventArgs e)
//        {
//            foreach (ListViewItem item in lvNodes.Items)
//            {
//                item.Selected = true;
//            }
//        }

//        private void cmsResults_Opening(object sender, CancelEventArgs e)
//        {
//            try
//            {
//                //foreach (ColumnHeader colHeader in lvNodes.Columns)
//                //{
//                //    Debug.WriteLine(colHeader.Name);
//                //}

//                cmiRename.Enabled = false;
//                cmiLocate.Enabled = false;
//                cmiDelete.Enabled = false;
//                cmiCut.Enabled = false;
//                cmiCopy.Enabled = false;
//                if (lvNodes.SelectedItems.Count > 0)
//                {
//                    cmiLocate.Enabled = true;
//                    foreach (System.Windows.Forms.ListViewItem item in lvNodes.SelectedItems)
//                    {
//                        MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)item.Tag;
//                        if (itemTag.NodeSecurity.AllowDelete)
//                        {
//                            cmiDelete.Enabled = true;
//                        }
//                        if (itemTag.NodeSecurity.AllowMove)
//                        {
//                            cmiCut.Enabled = true;
//                        }

//                        if (itemTag.NodeSecurity.AllowUpdate)
//                        {
//                            if (lvNodes.SelectedItems.Count == 1 &&
//                                AllowRename(itemTag.Key))
//                            {
//                                cmiRename.Enabled = true;
//                            }
//                            cmiCopy.Enabled = true;
//                        }
//                    }
//                }
//            }
//            catch
//            {
//            }
//            //if (_searching)
//            //{
//            //    e.Cancel = true;
//            //    return;
//            //}
//        }

//        private bool AllowRename(int aNodeRID)
//        {
//            try
//            {
//                return true;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void cmiLevelClearAll_Click(object sender, EventArgs e)
//        {
//            for (int i = 0; i < clbLevel.Items.Count; i++)
//            {
//                clbLevel.SetItemChecked(i, false);
//            }
//        }

//        private void cmiLevelSelectAll_Click(object sender, EventArgs e)
//        {
//            for (int i = 0; i < clbLevel.Items.Count; i++)
//            {
//                clbLevel.SetItemChecked(i, true);
//            }
//        }

//        override protected ClipboardProfileBase BuildClipboardItem(System.Windows.Forms.ListViewItem aItem, DragDropEffects aAction)
//        {
//            HierarchyNodeClipboardProfile cbp;
//            FunctionSecurityProfile securityProfile;
//            try
//            {
//                MIDProductSearchItemTag itemTag = (MIDProductSearchItemTag)aItem.Tag;
//                HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(itemTag.Key, false);
//                securityProfile = new FunctionSecurityProfile(itemTag.Key);
//                securityProfile.SetFullControl();
//                cbp = new HierarchyNodeClipboardProfile(itemTag.Key, hnp.Text, securityProfile);
//                cbp.Action = aAction;

//                // Begin TT#564 - JSmith - Copy/Paste from search not working
//                cbp.HierarchyRID = hnp.HomeHierarchyRID;
//                cbp.HierarchyType = hnp.HomeHierarchyType;
//                cbp.HomeHierarchyRID = hnp.HomeHierarchyRID;
//                cbp.HomeHierarchyType = hnp.HomeHierarchyType;
//                // End TT#564

//                // use temp tree node to calculate dimensions so calculations for image dragging are consistent
//                HierarchyTreeView tempTreeView = new HierarchyTreeView();
//                MIDHierarchyNode tempNode = new MIDHierarchyNode(SAB, eTreeNodeType.ObjectNode, hnp, hnp.Text, Include.NoRID, Include.NoRID, null, Include.NoRID, Include.NoRID, Include.NoRID);
//                tempTreeView.Nodes.Add(tempNode);
//                cbp.DragImage = lvNodes.SmallImageList.Images[aItem.ImageIndex];
//                cbp.DragImageHeight = tempNode.Bounds.Height;
//                cbp.DragImageWidth = tempNode.Bounds.Width;

//                return cbp;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void DisplayMessages(EditMsgs em)
//        {
//            MIDRetail.Windows.DisplayMessages.Show(em, _SAB, this.Text);
//        }

//        private void splitContainer1_DragEnter(object sender, DragEventArgs e)
//        {
//            Image_DragEnter(sender, e);
//        }

//        private void splitContainer1_DragOver(object sender, DragEventArgs e)
//        {
//            Image_DragOver(sender, e);
//        }

//        private void splitContainer1_DragLeave(object sender, EventArgs e)
//        {
//            Image_DragLeave(sender, e);
//        }

//        private void splitContainer1_Panel1_DragEnter(object sender, DragEventArgs e)
//        {
//            Image_DragEnter(sender, e);
//        }

//        private void splitContainer1_Panel1_DragOver(object sender, DragEventArgs e)
//        {
//            Image_DragOver(sender, e);
//        }

//        private void splitContainer1_Panel1_DragLeave(object sender, EventArgs e)
//        {
//            Image_DragLeave(sender, e);
//        }

//        private void splitContainer1_Panel2_DragEnter(object sender, DragEventArgs e)
//        {
//            Image_DragEnter(sender, e);
//        }

//        private void splitContainer1_Panel2_DragOver(object sender, DragEventArgs e)
//        {
//            Image_DragOver(sender, e);
//        }

//        private void splitContainer1_Panel2_DragLeave(object sender, EventArgs e)
//        {
//            Image_DragLeave(sender, e);
//        }

//        #region Characteristics
//        private void btnCharacteristics_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                btnSearch.Enabled = false;
//                //RedrawOperands(pnlCharacteristicQuery);
//                pnlCharacteristics.BringToFront();
//                pnlCharacteristics.Visible = true;
//            }
//            catch
//            {
//                throw;
//            }
//        }

//        private void btnOK_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                //FilterDef.SaveFilter(Include.NoRID, SAB.ClientServerSession.UserRID, null);
//                pnlSearch.BringToFront();
//                pnlSearch.Visible = true;
//                pnlCharacteristics.Visible = false;
//                btnSearch.Enabled = true;
//            }
//            //catch (FilterSyntaxErrorException exc)
//            //{
//            //    if (exc.ErrorOperand != null)
//            //    {
//            //        ((BasicQueryLabel)exc.ErrorOperand.Label).HighlightClick();
//            //    }

//            //    System.Windows.Forms.MessageBox.Show(this, exc.Message, "Filter Syntax Error");

//            //}
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void btnClose_Click(object sender, EventArgs e)
//        {
//            try
//            {
//                // reload filter from last save
//                //FilterDef = new ProductSearchFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator), 0);
//                //((PanelTag)pnlCharacteristicQuery.Tag).OperandArray = ((ProductSearchFilterDefinition)FilterDef).CharacteristicOperandList;
//                pnlCharacteristicQuery.Controls.Clear();
//                pnlSearch.BringToFront();
//                pnlSearch.Visible = true;
//                pnlCharacteristics.Visible = false;
//                btnSearch.Enabled = true;
//            }
//            catch (Exception exc)
//            {
//                string message = exc.ToString();
//                throw;
//            }
//        }

//        private void cmiCharClearAll_Click(object sender, EventArgs e)
//        {
//            // clear filter definition
//            //FilterDef = new ProductSearchFilterDefinition(SAB, SAB.ClientServerSession, _dlFilter, new LabelCreatorDelegate(LabelCreator), 0);
//            //((ProductSearchFilterDefinition)FilterDef).CharacteristicOperandList.Clear();
//            //((PanelTag)pnlCharacteristicQuery.Tag).OperandArray = ((ProductSearchFilterDefinition)FilterDef).CharacteristicOperandList;
//            //pnlCharacteristicQuery.Controls.Clear();
//            //RedrawOperands(pnlCharacteristicQuery);
//        }

//        #region panel Events
//        private void panel_Click(object sender, EventArgs e)
//        {
//            //BasePanel_Click(sender, e);
//        }

//        public void panel_DragEnter(object sender, DragEventArgs e)
//        {
//            //BasePanel_DragEnter(sender, e);
//        }

//        private void panel_DragOver(object sender, DragEventArgs e)
//        {
//            //BasePanel_DragOver(sender, e);
//        }

//        public void panel_DragDrop(object sender, DragEventArgs e)
//        {
//            //BasePanel_DragDrop(sender, e);
//        }

//        public void panel_DragLeave(object sender, EventArgs e)
//        {
//            //BasePanel_DragLeave(sender, e);
//        }
//        #endregion panel Events

//        #region toolButton Events
//        private void toolButton_Click(object sender, EventArgs e)
//        {
//            //BaseToolButton_Click(sender, e);
//        }

//        private void toolButton_MouseDown(object sender, MouseEventArgs e)
//        {
//            //BaseToolButton_MouseDown(sender, e);
//        }

//        private void toolButton_MouseMove(object sender, MouseEventArgs e)
//        {
//            //BaseToolButton_MouseMove(sender, e);
//        }

//        private void toolButton_MouseUp(object sender, MouseEventArgs e)
//        {
//            //BaseToolButton_MouseUp(sender, e);
//        }

//        private void toolButton_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            //BaseToolButton_KeyDown(sender, e);
//        }

//        private void toolButton_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
//        {
//            //BaseToolButton_KeyPress(sender, e);
//        }
//        #endregion toolButton Events


//        #endregion Characteristics

//        override protected void BeforeClosing()
//        {
//            //try
//            //{
//                _formClosing = true;        //TT#826 - Error produced when closing the hierarchy search while the search is running - apicchetti - 1/19/2011
//            //    int dataKey = 0;
//            //    _dlFilter.OpenUpdateConnection();
//            //    _dlFilter.ProductSearchView_Delete(SAB.ClientServerSession.UserRID);
//            //    foreach (ColumnHeader colHeader in lvNodes.Columns)
//            //    {
//            //        MIDProductSearchColProfile colTag = (MIDProductSearchColProfile)colHeader.Tag;
//            //        switch (colTag.ColType)
//            //        {
//            //            case eMIDProductSearchColType.MIDText:
//            //                dataKey = Convert.ToInt32(colTag.MIDTextCode);
//            //                break;
//            //            case eMIDProductSearchColType.Characteristic:
//            //                dataKey = colTag.ProductCharProfile.Key;
//            //                break;
//            //        }
//            //        if (colHeader.Width > 0)
//            //        {
//            //            _dlFilter.ProductSearchView_Insert(SAB.ClientServerSession.UserRID, colHeader.DisplayIndex + 1,
//            //                Convert.ToInt32(colTag.ColType), dataKey, colHeader.Width);
//            //        }
//            //    }
//            //    _dlFilter.CommitData();
//            //}
//            //catch (Exception exception)
//            //{
//            //    HandleException(exception);
//            //}
//            //finally
//            //{
//            //    _dlFilter.CloseUpdateConnection();

//            //}
//        }

//        #region IFormBase Members
//        override public void ICut()
//        {
//            PerformCut();
//        }

//        override public void ICopy()
//        {
//            PerformCopy();
//        }

//        override public void IPaste()
//        {

//        }

//        override public void ISave()
//        {
//        }

//        override public void ISaveAs()
//        {

//        }

//        override public void IDelete()
//        {

//        }

//        override public void IRefresh()
//        {

//        }

//        #endregion
//    }


//    public class MIDProductSearchItemTag
//    {
//        private int _key;
//        private int _homeHierarchyRID;
//        private int _hierarchyLevel;
//        private HierarchyNodeSecurityProfile _nodeSecurity;
//        private FunctionSecurityProfile _functionSecurity;
		
//        public MIDProductSearchItemTag(int aKey, int aHomeHierarchyRID,
//            int aHierarchyLevel, HierarchyNodeSecurityProfile aNodeSecurity)
//        {
//            _key = aKey;
//            _homeHierarchyRID = aHomeHierarchyRID;
//            _hierarchyLevel = aHierarchyLevel;
//            _nodeSecurity = aNodeSecurity;
//            _functionSecurity = null;
//        }

//        public int Key
//        {
//            get { return _key; }
//        }

//        public int HomeHierarchyRID
//        {
//            get { return _homeHierarchyRID; }
//        }

//        public int HierarchyLevel
//        {
//            get { return _hierarchyLevel; }
//        }

//        /// <summary>
//        /// Gets or sets the security profile of the product for the user.
//        /// </summary>
//        public HierarchyNodeSecurityProfile NodeSecurity
//        {
//            get { return _nodeSecurity; }
//        }

//        /// <summary>
//        /// Gets or sets the security profile of the function for the user.
//        /// </summary>
//        public FunctionSecurityProfile FunctionSecurity
//        {
//            get { return _functionSecurity; }
//            set { _functionSecurity = value; }

//        }
//    }

//    public enum eMIDProductSearchColType
//    {
//        MIDText,
//        Characteristic
//    }

//    public class MIDProductSearchColProfile : Profile
//    {
//        private eMIDProductSearchColType _colType;
//        private ColumnHeader _columnHeader;
//        private ProductCharProfile _productCharProfile;
//        private eMIDTextCode _MIDTextCode;

//        public MIDProductSearchColProfile(
//            int aKey, 
//            eMIDProductSearchColType aColType,
//            ColumnHeader aColumnHeader,
//            ProductCharProfile aProductCharProfile)
//            : base(aKey)
//        {
//            _colType = aColType;
//            _columnHeader = aColumnHeader;
//            _productCharProfile = aProductCharProfile;
//        }

//        public MIDProductSearchColProfile(
//            int aKey, 
//            eMIDProductSearchColType aColType,
//            ColumnHeader aColumnHeader,
//            eMIDTextCode aMIDTextCode)
//            : base(aKey)
//        {
//            _colType = aColType;
//            _columnHeader = aColumnHeader;
//            _MIDTextCode = aMIDTextCode;
//        }

//        /// <summary>
//        /// Returns the eProfileType of this profile.
//        /// </summary>

//        override public eProfileType ProfileType
//        {
//            get
//            {
//                return eProfileType.ProductSearchCol;
//            }
//        }

//        public eMIDProductSearchColType ColType
//        {
//            get { return _colType; }
//        }

//        public ColumnHeader ColumnHeader
//        {
//            get { return _columnHeader; }
//        }

//        public ProductCharProfile ProductCharProfile
//        {
//            get { return _productCharProfile; }
//        }

//        public eMIDTextCode MIDTextCode
//        {
//            get { return _MIDTextCode; }
//        }
//    }

//    public class MIDProductSearchProductData
//    {
//        private int _key;
//        private string _text;
//        private string _ID;
//        private string _name;
//        private string _description;
//        private int _hierarchyRID;
//        private int _hierarchyLevel;
//        private string _levelName;
//        private string _folderColor;
//        private NodeCharProfileList _nodeCharProfileList;
//        private HierarchyNodeSecurityProfile _nodeSecurity;

//        public MIDProductSearchProductData(int aKey, string aID, string aName, string aDescription, 
//            int aHierarchyRID, int aHierarchyLevel, string aLevelName, string aFolderColor, 
//            NodeCharProfileList aNodeCharProfileList, HierarchyNodeSecurityProfile aNodeSecurity)
//        {
//            _key = aKey;
//            _text = null;
//            _ID = aID;
//            _name = aName;
//            _description = aDescription;
//            _hierarchyRID = aHierarchyRID;
//            _hierarchyLevel = aHierarchyLevel;
//            _levelName = aLevelName;
//            _folderColor = aFolderColor;
//            _nodeCharProfileList = aNodeCharProfileList;
//            _nodeSecurity = aNodeSecurity;
//        }

//        public int Key
//        {
//            get { return _key; }
//        }

//        public string Text
//        {
//            set { _text = value; }
//            get { return _text; }
//        }

//        public string ID
//        {
//            get { return _ID; }
//        }

//        public string Name
//        {
//            get { return _name; }
//        }

//        public string Description
//        {
//            get { return _description; }
//        }

//        public int HierarchyRID
//        {
//            get { return _hierarchyRID; }
//        }

//        public int HierarchyLevel
//        {
//            get { return _hierarchyLevel; }
//        }

//        public string LevelName
//        {
//            get { return _levelName; }
//        }

//        public string FolderColor
//        {
//            get { return _folderColor; }
//        }

//        public NodeCharProfileList NodeCharProfileList
//        {
//            get { return _nodeCharProfileList; }
//        }

//        public HierarchyNodeSecurityProfile NodeSecurity
//        {
//            get { return _nodeSecurity; }
//        }
//    }

//    public class MerchandiseNodeLocateEventArgs : EventArgs
//    {
//        int _nodeRID;

//        public MerchandiseNodeLocateEventArgs(int aNodeRID)
//        {
//            _nodeRID = aNodeRID;
//        }
//        public int NodeRID
//        {
//            get { return _nodeRID; }
//            set { _nodeRID = value; }
//        }
//    }

//    public class MerchandiseNodeRenameEventArgs : EventArgs
//    {
//        private int _key;
//        private string _text;

//        public MerchandiseNodeRenameEventArgs(int aKey, string aText)
//        {
//            _key = aKey;
//            _text = aText;
//        }
//        public int Key
//        {
//            get { return _key; }
//            set { _key = value; }
//        }
//        public string Text
//        {
//            get { return _text; }
//            set { _text = value; }
//        }
//    }

//    public class MerchandiseNodeDeleteEventArgs : EventArgs
//    {
//        private int _parentKey;
//        private int _key;

//        public MerchandiseNodeDeleteEventArgs(int aParentKey, int aKey)
//        {
//            _parentKey = aParentKey;
//            _key = aKey;
//        }
//        public int ParentKey
//        {
//            get { return _parentKey; }
//            set { _parentKey = value; }
//        }
//        public int Key
//        {
//            get { return _key; }
//            set { _key = value; }
//        }
//    }
//}