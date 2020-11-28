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

namespace MIDRetail.Windows
{
    public partial class ColorBrowser : MIDFormBase
    {
        // add event to throw when form is closed
        public delegate void ColorBrowserSelectEventHandler(object source, ColorBrowserSelectEventArgs e);
        public event ColorBrowserSelectEventHandler OnColorBrowserSelectHandler;

        #region Fields
        //=======
        // FIELDS
        //=======
        private bool _allowPlaceHolderColors;
        private bool _OKClicked = false;
        private ArrayList _lockControl = new ArrayList();
        private ArrayList _selectedColors = new ArrayList();
        private int _numberOfPlaceholderColors = 0;
        private ArrayList _currentPlaceHolders;
        private ColorCodeList _placeHolders;
        private ArrayList _placeHolderKeys = new ArrayList();

        #endregion Fields

        #region Constructors
        //=============
        // CONSTRUCTORS
        //=============
        public ColorBrowser(SessionAddressBlock aSAB, bool aAllowPlaceHolderColors)
            : base(aSAB)
        {
            _allowPlaceHolderColors = aAllowPlaceHolderColors;
            _currentPlaceHolders = new ArrayList();
            InitializeComponent();
            //Begin TT#6 - Color Browser object reference error
            //tvColors.InitializeTreeView(aSAB, true, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
            //End TT#6
        }
        public ColorBrowser(SessionAddressBlock aSAB, bool aAllowPlaceHolderColors, ArrayList aPlaceHolders)
            : base(aSAB)
        {
            _allowPlaceHolderColors = aAllowPlaceHolderColors;
            SetCurrentPlaceHolders(aPlaceHolders);
            InitializeComponent();
            //Begin TT#6 - Color Browser object reference error
            //tvColors.InitializeTreeView(aSAB, true, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
            //End TT#6
        }

        // Used to get placeholder colors without showing the dialog window along with GetPhColors... method
        public ColorBrowser(SessionAddressBlock aSAB, ArrayList aPlaceHolders, int aRequestedNumPlaceholders)
            : base(aSAB)
        {
            _allowPlaceHolderColors = true;
            SetCurrentPlaceHolders(aPlaceHolders);
            _numberOfPlaceholderColors = aRequestedNumPlaceholders;
        }
        #endregion Constructors

        #region Properties
        //============
        // PROPERTIES
        //============

        public bool OKClicked
        {
            get { return _OKClicked; }
        }

        public int NumberOfPlaceholderColors
        {
            get { return _numberOfPlaceholderColors; }
        }

        public ArrayList PlaceHolders
        {
            get { return _placeHolderKeys; }
        }

        public ArrayList SelectedColors
        {
            get { return _selectedColors; }
        }

        #endregion Properties

        private void SetCurrentPlaceHolders(ArrayList aPlaceHolders)
        {
            try
            {
                if (aPlaceHolders == null)
                {
                    _currentPlaceHolders = new ArrayList();
                }
                else
                {
                    _currentPlaceHolders = aPlaceHolders;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void ColorBrowser_Load(object sender, EventArgs e)
        {
            try
            {
                //Begin TT#6 - Color Browser object reference error
                tvColors.InitializeTreeView(SAB, true, ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer" ? ParentForm : ParentForm.Owner);
                tvColors.OnMIDDoubleClick += new MIDTreeView.MIDTreeViewDoubleClickHandler(tvColors_OnMIDDoubleClick);
                //End TT#6
                Cursor.Current = Cursors.WaitCursor;
                tvColors.ImageList = MIDGraphics.ImageList;
                SetText();
                BuildContextMenu();
                Format_Title(eDataState.None, eMIDTextCode.frm_ColorBrowser, null);
                AddColorGroups();
                // if placeholders are not allowed, hide fields and fill space with treeview
                if (!_allowPlaceHolderColors)
                {
                    lblPlaceHolders.Visible = false;
                    txtPlaceHolders.Visible = false;
                    tvColors.Dock = DockStyle.Fill;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        //Begin TT#6 - JSmith - Color Browser object reference error
        void tvColors_OnMIDDoubleClick(object source, MIDTreeNode node)
        {
            EventArgs e = new EventArgs();
            btnOK_Click(source, e);
        }
        //End TT#6

        private void SetText()
        {
            btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
            btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
            lblPlaceHolders.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NumberOfPlaceholderColors);
        }

        private void BuildContextMenu()
        {
            tvColors.ContextMenuStrip = this.cmsColorTreeView;
            cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
            cmiSearch.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search) + "...";
            cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
        }

        private void AddColorGroups()
        {
            try
            {
                MIDColorNode node = null;
                ColorData data = new ColorData();
                DataTable colorGroups = data.GetColorGroups();
                bool unassignedAdded = false;
                bool addNode = false;
                int key = 0;

                foreach (DataRow dr in colorGroups.Rows)
                {
                    if (dr["COLOR_CODE_GROUP"] == DBNull.Value ||
                        (Convert.ToString(dr["COLOR_CODE_GROUP"], CultureInfo.CurrentUICulture)).Trim().Length == 0)
                    {
                        if (!unassignedAdded)
                        {
                            node = tvColors.BuildColorGroupNode(MIDText.GetTextOnly(eMIDTextCode.Unassigned), key);
                            unassignedAdded = true;
                            addNode = true;
                        }
                        else
                        {
                            addNode = false;
                        }
                    }
                    else
                    {
                        //BEGIN TT#3128- Color Browser not displaying members of the first color group-AGACI
                        // Handles assigned and unassigned color groups.
                        if (key == 0)
                            key = 1;
                        //END TT#3128- Color Browser not displaying members of the first color group-AGACI
                        node = tvColors.BuildColorGroupNode(Convert.ToString(dr["COLOR_CODE_GROUP"], CultureInfo.CurrentUICulture), key);
                        addNode = true;
                    }

                    if (addNode)
                    {
                        tvColors.Nodes.Add(node);
                        ++key;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _OKClicked = false;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                _OKClicked = true;
                if (ValidValues())
                {
                    foreach (MIDColorNode selectedNode in tvColors.GetSelectedNodes())
                    {
                        //if (selectedNode.NodeType == eColorNodeType.ColorNode)
                        if (selectedNode.NodeType == eProfileType.ColorCode)
                        {
                            _selectedColors.Add(selectedNode.Profile.Key);
                        }
                    }
                    GetPhColorsAndSetEventHandler();
                }
                else
                {
                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorsFoundReviewCorrect), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private bool ValidValues()
        {
            try
            {
                bool validValues = true;
                if (txtPlaceHolders.Text.Trim().Length > 0)
                {
                    try
                    {
                        _numberOfPlaceholderColors = Convert.ToInt32(txtPlaceHolders.Text, CultureInfo.CurrentCulture);
                        if (_numberOfPlaceholderColors < 0)
                        {
                            string error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
                            ErrorProvider.SetError(txtPlaceHolders, error);
                            validValues = false;
                        }
                    }
                    catch
                    {
                        string error = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger);
                        ErrorProvider.SetError(txtPlaceHolders, error);
                        validValues = false;
                    }
                }

                return validValues;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return false;
            }
        }

        public void GetPhColorsAndSetEventHandler()  // executed internally & externally
        {
            try
            {
                if (_numberOfPlaceholderColors > 0)
                {
                    HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                    _placeHolders = hm.GetPlaceholderColors(_numberOfPlaceholderColors, _currentPlaceHolders);
                    foreach (ColorCodeProfile ccp in _placeHolders)
                    {
                        _placeHolderKeys.Add(ccp);
                    }
                }
                ColorBrowserSelectEventArgs ea = new ColorBrowserSelectEventArgs(_selectedColors, _placeHolderKeys);
                if (OnColorBrowserSelectHandler != null)  // throw event to explorer to make changes
                {
                    OnColorBrowserSelectHandler(this, ea);
                }
                Close();
            }
            catch
            {
                throw;
            }
        }

        private void tvColors_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                MIDColorNode node = (MIDColorNode)e.Node;
                if (!node.ChildrenLoaded)
                {
                    AddColors(node);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        //private void tvColors_AfterExpand(object sender, TreeViewEventArgs e)
        //{
        //    MIDColorNode node = (MIDColorNode)e.Node;
        //    node.ImageIndex = tvColors.GetImageIndex(node, MIDGraphics.OpenFolder);
        //    node.SelectedImageIndex = node.ImageIndex;
        //}

        //private void tvColors_AfterCollapse(object sender, TreeViewEventArgs e)
        //{
        //    MIDColorNode node = (MIDColorNode)e.Node;
        //    node.ImageIndex = tvColors.GetImageIndex(node, MIDGraphics.ClosedFolder);
        //    node.SelectedImageIndex = node.ImageIndex;
        //}

        private void AddColors(MIDColorNode aGroupNode)
        {
            try
            {
                bool addNode = false;
                aGroupNode.Nodes.Clear();
                MIDColorNode node = null;
                ColorData data = new ColorData();
                DataTable colorGroups = null;
                // replace name with null if unassigned group
                if (aGroupNode.Profile.Key == 0)
                {
                    colorGroups = data.GetColorsForGroup(null);
                }
                else
                {
                    colorGroups = data.GetColorsForGroup(aGroupNode.ColorGroup);
                }

                foreach (DataRow dr in colorGroups.Rows)
                {
                    addNode = false;
                    // if unassigned group, treat a blank group name as null
                    if (aGroupNode.Profile.Key == 0)
                    {
                        if (dr["COLOR_CODE_GROUP"] == DBNull.Value ||
                            (Convert.ToString(dr["COLOR_CODE_GROUP"], CultureInfo.CurrentUICulture)).Trim().Length == 0)
                        {
                            addNode = true;
                        }
                    }
                    else
                    {
                        addNode = true;
                    }

                    if (addNode)
                    {
                        node = tvColors.BuildColorNode(Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture));
                        aGroupNode.Nodes.Add(node);
                    }
                }
                aGroupNode.ChildrenLoaded = true;
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        public override void ICopy()
        {
            try
            {
                ArrayList copyNodes = new ArrayList();

                foreach (MIDColorNode selectedNode in tvColors.GetSelectedNodes())
                {
                    //if (selectedNode.NodeType == eColorNodeType.ColorNode)
                    if (selectedNode.NodeType == eProfileType.ColorCode)
                    {
                        copyNodes.Add(selectedNode);
                    }
                }

                if (copyNodes.Count > 0)
                {
                    //tvColors.CopyToClipboard(copyNodes, eDropAction.Copy);
					tvColors.CopyToClipboard(tvColors.BuildClipboardList(copyNodes, DragDropEffects.Copy));
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private void cmiCopy_Click(object sender, EventArgs e)
        {
            ICopy();
        }

        private void cmiSearch_Click(object sender, EventArgs e)
        {
            try
            {
                ColorNodeSearch searchForm = new ColorNodeSearch(SAB);
                searchForm.ColorNodeLocateEvent += new ColorNodeSearch.ColorNodeLocateEventHandler(searchForm_OnColorNodeLocateHandler);
                searchForm.ColorNodeRenameEvent += new ColorNodeSearch.ColorNodeRenameEventHandler(searchForm_ColorNodeRenameEvent);
                searchForm.ColorNodeDeleteEvent += new ColorNodeSearch.ColorNodeDeleteEventHandler(searchForm_ColorNodeDeleteEvent);
                //  Allow for a floating explorer.   ParentForm is not Client.Explorer if floating
                if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
                {
                    searchForm.MdiParent = this.ParentForm;
                }
                else
                {
                    searchForm.MdiParent = this.ParentForm.Owner;
                }
                searchForm.Show();
            }
            catch
            {
                throw;
            }
        }

        private void tvColors_KeyDown(object sender, KeyEventArgs e)
        {
            if (tvColors.CurrentState != eDragStates.Idle)
            {
                return; // precondition, can't change effect while moving
            }

            if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
            {
                ICopy();
                e.Handled = true;
            }
            //else if (e.KeyCode == Keys.X && e.Modifiers == Keys.Control)
            //{
            //    ICut();
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            //{
            //    _pasteNode = (MIDColorNode)tvColors.MouseDownOnNode;
            //    PasteNodes();
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.R && e.Modifiers == Keys.Control)
            //{
            //    Rename();
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.N && e.Modifiers == Keys.Control)
            //{
            //    NewNode();
            //    e.Handled = true;
            //}
            //else if (e.KeyCode == Keys.D && e.Modifiers == Keys.Control)
            //{
            //    IDelete();
            //    e.Handled = true;
            //}
        }

        void searchForm_ColorNodeDeleteEvent(object source, ColorNodeDeleteEventArgs e)
        {
            try
            {
                lock (_lockControl.SyncRoot)
                {
                    Cursor.Current = Cursors.WaitCursor;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        void searchForm_ColorNodeRenameEvent(object source, ColorNodeRenameEventArgs e)
        {
            try
            {
                lock (_lockControl.SyncRoot)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(e.Key);
                    //MIDColorNode node = new MIDColorNode(SAB, ccp);
                    MIDColorNode node = tvColors.BuildColorNode(ccp.Key);

                    //node.Text = ccp.Text;
                    TreeNodeCollection nodes = tvColors.Nodes;  // update all occurrances of the node
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void searchForm_OnColorNodeLocateHandler(object source, ColorNodeLocateEventArgs e)
        {
            try
            {
                lock (_lockControl.SyncRoot)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    tvColors.SelectedNode = null;
                    tvColors.SimulateMultiSelect = true;
                    MIDColorNode treeNode;

                    // locate group folder
                    string groupName;
                    if (e.GroupName == null ||
                        e.GroupName.Trim().Length == 0)
                    {
                        groupName = MIDText.GetTextOnly(eMIDTextCode.Unassigned);
                    }
                    else
                    {
                        groupName = e.GroupName;
                    }
                    treeNode = LocateGroupNode(groupName);

                    // locate color
                    if (treeNode != null)
                    {
                        // if group is not expanded, expand and make sure colors are loaded
                        if (!treeNode.IsExpanded)
                        {
                            treeNode.Expand();

                            bool loop = true;
                            while (loop)
                            {
                                if (treeNode.ChildrenLoaded &&
                                    treeNode.IsExpanded)
                                {
                                    loop = false;
                                }
                                else
                                {
                                    System.Threading.Thread.Sleep(10);
                                }
                            }
                        }
                        treeNode = LocateNode(e.Key, treeNode);

                        if (treeNode != null)
                        {
                            tvColors.SelectedNode = treeNode;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                tvColors.SimulateMultiSelect = false;
                Cursor.Current = Cursors.Default;
            }
        }

        private MIDColorNode LocateGroupNode(string aGroupName)
        {
            try
            {
                TreeNodeCollection nodes = tvColors.Nodes;
                foreach (MIDColorNode node in nodes)
                {
                    if (node.ColorGroup == aGroupName)
                    {
                        return node;
                    }
                }
                MessageBox.Show("Node not found");
                return null;
            }
            catch
            {
                throw;
            }
        }

        private MIDColorNode LocateNode(int aNodeRID, MIDColorNode aParentFolder)
        {
            try
            {
                foreach (MIDColorNode node in aParentFolder.Nodes)
                {
                    if (node.Profile.Key == aNodeRID)
                    {
                        return node;
                    }
                }
                MessageBox.Show("Node not found");
                return null;
            }
            catch
            {
                throw;
            }
        }

        private void txtPlaceHolders_TextChanged(object sender, EventArgs e)
        {
            InheritanceProvider.SetError(txtPlaceHolders, string.Empty);
        }
    }

    public class ColorBrowserSelectEventArgs : EventArgs
    {
        private ArrayList _selectedColors;
        private ArrayList _placeHolders;

        public ColorBrowserSelectEventArgs(ArrayList aSelectedColors, ArrayList aPlaceHolders)
        {
            if (aSelectedColors == null)
            {
                _selectedColors = new ArrayList();
            }
            else
            {
                _selectedColors = aSelectedColors;
            }
            if (aPlaceHolders == null)
            {
                _placeHolders = new ArrayList();
            }
            else
            {
                _placeHolders = aPlaceHolders;
            }
        }
        public int NumberOfPlaceholderColors
        {
            get { return _placeHolders.Count; }
        }
        public ArrayList SelectedColors
        {
            get { return _selectedColors; }
        }
        public ArrayList PlaceHolders
        {
            get { return _placeHolders; }
        }
    }
}