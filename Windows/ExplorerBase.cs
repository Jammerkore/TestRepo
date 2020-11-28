using System;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Windows.Forms;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ExplorerBase.
	/// </summary>
	public partial class ExplorerBase : UserControl , IFormBase
	{
		//=======
		// FEILDS
		//=======

		private SessionAddressBlock _SAB;
		protected ExplorerAddressBlock _EAB;
		private Form _mainMDIForm;
		private string _graphicsDir;
		private Image _userImage = null;
		private Image _globalImage = null;
		private Image _calendarImage = null;
		private Image _storeImage = null;
		private Image _errorImage = null;
		private Image _reoccurringImage = null;
        private Image _notesImage = null;
		private Image _inheritanceImage = null;

		private int cClosedFolderImage;
		private int cOpenFolderImage;

		private bool _formLoaded = false;
		private bool _allowUpdate;
		private System.Windows.Forms.TextBox txtGetReadOnlyColor;

        PopupMenuTool _fileMenuTool;
        PopupMenuTool _editMenuTool;

		private FolderDataLayer _dlFolder;
		private SecurityAdmin _dlSecurity;
		private MIDTreeView _treeView;

		private int _spacing = 2;
		private int _imageHeight;
		private int _imageWidth;

		protected eCutCopyOperation _cutCopyOperation;
        private ArrayList _cutCopyNodes;
        // Begin TT#564 - JSmith - Copy/Paste from search not working
        bool _pasteFromClipboard = false;
        // End TT#564

		private bool _showNewFolder;
		private bool _showNewItem;
		private bool _showCut;
		private bool _showCopy;
		private bool _showPaste;
		private bool _showDelete;
		private bool _showRename;
		private bool _showOpen;
        private bool _showInUse;    //TT#110-MD-VStuart - In Use Tool
		private bool _showRefresh;
        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        private bool _showRemove;
        private string sDelete = null;
        private string sRemove = null;
        // End TT#373

		//=============
		// CONSTRUCTORS
		//=============

		// Base constructor required for Windows Form Designer support
		public ExplorerBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeExplorer();
		}

		public ExplorerBase(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
		{
			string userFileName;
			string globalFileName;
			string errorFileName;
			string reoccurringFileName;
			string inheritanceFileName;
			string notesFileName;

			try
			{
				_SAB = aSAB;
				_EAB = aEAB;
				_mainMDIForm = aMainMDIForm;

				_allowUpdate = false;

				cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
				cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);

				InitializeExplorer();
				InitializeTreeView();

                // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
                //_graphicsDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + MIDGraphics.GraphicsDir;
                _graphicsDir = MIDGraphics.MIDGraphicsDir;
                // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration

				userFileName = _graphicsDir + "\\" + MIDGraphics.SecUserImage;

				if (System.IO.File.Exists(userFileName))
				{
					_userImage = Image.FromFile(userFileName);
				}

				globalFileName = _graphicsDir + "\\" + MIDGraphics.GlobalImage;

				if (System.IO.File.Exists(globalFileName))
				{
					_globalImage = Image.FromFile(globalFileName);
				}

				errorFileName = _graphicsDir + "\\" + MIDGraphics.ErrorImage;

				if (System.IO.File.Exists(errorFileName))
				{
					_errorImage = Image.FromFile(errorFileName);
				}

				reoccurringFileName = _graphicsDir + "\\" + MIDGraphics.ReoccurringImage;

				if (System.IO.File.Exists(reoccurringFileName))
				{
					_reoccurringImage = Image.FromFile(reoccurringFileName);
				}

				inheritanceFileName = _graphicsDir + "\\" + MIDGraphics.InheritanceImage;

				if (System.IO.File.Exists(inheritanceFileName))
				{
					_inheritanceImage = Image.FromFile(inheritanceFileName);
				}

				notesFileName = _graphicsDir + "\\" + MIDGraphics.NotesImage;

				if (System.IO.File.Exists(notesFileName))
				{
					_notesImage = Image.FromFile(notesFileName);
				}

				_fileMenuTool = new PopupMenuTool(Include.menuFile);
				_fileMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
				_fileMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
				utmMain.Tools.Add(_fileMenuTool);

				_editMenuTool = new PopupMenuTool(Include.menuEdit);
				_editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
				_editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
				utmMain.Tools.Add(_editMenuTool);

				_inheritanceProvider.Icon = new System.Drawing.Icon(inheritanceFileName);

				this.Load += new System.EventHandler(this.Explorer_Load);

				_dlFolder = new FolderDataLayer();
				_dlSecurity = new SecurityAdmin();
				//Begin TT#926 - JScott - Workspace Explorerer-Rt Click on a method, and "Process" is displayed 4 times in list

				ExplorerLoad();
				BuildTreeView();
				InitializeContextmenu();
				SetText();

				if (TreeView != null)
				{
					TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(TreeView_OnMIDNodeSelect);
					TreeView.ExpandFavoritesNode();
				}
				//End TT#926 - JScott - Workspace Explorerer-Rt Click on a method, and "Process" is displayed 4 times in list
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

        /// <summary>
        /// Gets the SessionAddressBlock.
        /// </summary>

		public SessionAddressBlock SAB
        {
            get
			{
				return _SAB;
			}
        }

		/// <summary>
		/// Gets the main MDI Form.
		/// </summary>

		public Form MainMDIForm
		{
			get
			{
				return _mainMDIForm;
			}
		}

		/// <summary>
		/// Gets the user image.
		/// </summary>

		public Image UserImage
		{
			get
			{
				return _userImage;
			}
		}

		/// <summary>
		/// Gets the global image.
		/// </summary>

		public Image GlobalImage
		{
			get
			{
				return _globalImage;
			}
		}

		/// <summary>
		/// Gets the graphics directory.
		/// </summary>

		public string GraphicsDirectory
		{
			get
			{
				return _graphicsDir;
			}
		}

		/// <summary>
		/// Gets the calendar image.
		/// </summary>

		public Image CalendarImage
		{
			get
			{
				return _calendarImage;
			}
		}

		/// <summary>
		/// Gets the store image.
		/// </summary>

		public Image StoreImage
		{
			get
			{
				return _storeImage;
			}
		}

		/// <summary>
		/// Gets the error image.
		/// </summary>

		public Image ErrorImage
		{
			get
			{
				return _errorImage;
			}
		}

		/// <summary>
		/// Gets the reoccurring image.
		/// </summary>

		public Image ReoccurringImage
		{
			get
			{
				return _reoccurringImage;
			}
		}

		/// <summary>
		/// Gets the calendar image.
		/// </summary>

		public Image InheritanceImage
		{
			get
			{
				return _inheritanceImage;
			}
		}

        /// <summary>
        /// Gets the notes image.
        /// </summary>

		public Image NotesImage
        {
            get
			{
				return _notesImage;
			}
        }

		/// <summary>
		/// Gets the inheritanceProvider object.
		/// </summary>

		public ErrorProvider ErrorProvider
		{
			get
			{
				return _errorProvider;
			}
		}

		/// <summary>
		/// Gets the inheritanceProvider object.
		/// </summary>

		public ErrorProvider InheritanceProvider
		{
			get
			{
				return _inheritanceProvider;
			}
		}

		/// <summary>
		/// Gets the ExplorerAddressBlock object.
		/// </summary>

		public ExplorerAddressBlock EAB
		{
			get
			{
				return _EAB;
			}
		}

		/// <summary>
		/// Gets a boolean identifying if the form load event has completed.
		/// </summary>

		public bool FormLoaded
		{
			get
			{
				return _formLoaded;
			}
		}

		/// <summary>
		/// Gets the flag identifying if the user can update the displayed data.
		/// </summary>
		/// <remarks>If not set, update is not allowed.</remarks>

		public bool AllowUpdate
		{
			get
			{
				return _allowUpdate;
			}
		}

		/// <summary>
		/// The data layer for folders
		/// </summary>

		public FolderDataLayer DlFolder
		{
			get
			{
				return _dlFolder;
			}
		}

		/// <summary>
		/// Gets or sets the base MIDTreeView control.
		/// </summary>

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		public MIDTreeView TreeView
		// END TT#217-MD - stodd - unable to run workflow methods against assortment
		{
			get
			{
				return _treeView;
			}
			set
			{
				_treeView = value;
			}
		}
		
		/// <summary>
        /// Gets the tooltip object.
        /// </summary>

		public System.Windows.Forms.ToolTip ToolTip
        {
            get
			{
				return toolTip1;
			}
        }

		/// <summary>
		/// Gets the eCutCopyOperation of the current action.
		/// </summary>

		protected eCutCopyOperation CutCopyOperation
		{
			get
			{
				return _cutCopyOperation;
			}
		}

		// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
        /// <summary>
        /// Gets the EnhancedToolTip
        /// </summary>
        public MIDEnhancedToolTip EnhancedToolTip
        {
            get { return midEnhancedToolTip; }
        }
		// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 

		//========
		// METHODS
		//========

        private void InitializeContextmenu()
        {
            cmiNew.Text = MIDText.GetTextOnly(eMIDTextCode.menu_File_New);
            cmiCut.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Cut);
            cmiCopy.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
            cmiPaste.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Paste);
            cmiRename.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Rename);
            cmiDelete.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
            cmiRefresh.Text = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Refresh);

            cmiCut.Image = MIDGraphics.GetImage(MIDGraphics.CutImage);
            cmiCopy.Image = MIDGraphics.GetImage(MIDGraphics.CopyImage);
            cmiPaste.Image = MIDGraphics.GetImage(MIDGraphics.PasteImage);
            cmiDelete.Image = MIDGraphics.GetImage(MIDGraphics.DeleteImage);
            cmiNewFolder.Image = MIDGraphics.GetImage(MIDGraphics.ClosedTreeFolder);
        }

        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        private void SetText()
        {
            sDelete = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
            sRemove = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Remove);
        }
        // End TT#373

        //-----------------------------------------
        //REQUIRED OVERRIDES FOR INHERITING CLASSES
        //-----------------------------------------

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        virtual protected void InitializeExplorer()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        virtual protected void InitializeTreeView()
        {
            throw new Exception("Method InitializeTreeView() must be overridden by inheriting class");
        }

        /// <summary>
        /// Virtual method that is called to perform Form Load tasks
        /// </summary>

        virtual protected void ExplorerLoad()
        {
            throw new Exception("Method ExplorerLoad() must be overridden by inheriting class");
        }

        /// <summary>
        /// Virtual method that is called to build the ExplorerBase TreeView
        /// </summary>

        virtual protected void BuildTreeView()
        {
            throw new Exception("Method BuildTreeView() must be overridden by inheriting class");
        }

		/// <summary>
		/// Virtual method that is called to refresh the ExplorerBase TreeView
		/// </summary>

		virtual protected void RefreshTreeView()
		{
			throw new Exception("Method RefreshTreeView() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method executed after the New Item menu item has been clicked.
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode that was clicked on
		/// </param>

		virtual protected void CustomizeActionMenu(MIDTreeNode aNode)
		{
		}

        protected void CustomizeActionMenuItem(eExplorerActionMenuItem aMenuItem, bool aVisible)
        {
            try
            {
                switch (aMenuItem)
                {
                    case eExplorerActionMenuItem.NewFolder:
                        _showNewFolder = aVisible;
                        break;

                    case eExplorerActionMenuItem.NewItem:
                        _showNewItem = aVisible;
                        break;

                    case eExplorerActionMenuItem.Cut:
                        _showCut = aVisible;
                        break;

                    case eExplorerActionMenuItem.Copy:
                        _showCopy = aVisible;
                        break;

                    case eExplorerActionMenuItem.Paste:
                        _showPaste = aVisible;
                        break;

                    case eExplorerActionMenuItem.Delete:
                        _showDelete = aVisible;
                        break;

                    case eExplorerActionMenuItem.Rename:
                        _showRename = aVisible;
                        break;

                    case eExplorerActionMenuItem.Open:
                        _showOpen = aVisible;
                        break;

                    //BEGIN TT#110-MD-VStuart - In Use Tool
                    case eExplorerActionMenuItem.InUse:
                        _showInUse = aVisible;
                        break;
                    //END TT#110-MD-VStuart - In Use Tool

                    case eExplorerActionMenuItem.Refresh:
                        _showRefresh = aVisible;
                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        protected void CustomizeTextMenuItem(eExplorerActionMenuItem aMenuItem, string aText)
        {
            try
            {
                switch (aMenuItem)
                {
                    case eExplorerActionMenuItem.New:
                        if (aText != null)
                        {
                            cmiNew.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.NewFolder:
                        if (aText != null)
                        {
                            cmiNewFolder.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.NewItem:
                        if (aText != null)
                        {
                            cmiNewItem.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Cut:
                        if (aText != null)
                        {
                            cmiCut.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Copy:
                        if (aText != null)
                        {
                            cmiCopy.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Paste:
                        if (aText != null)
                        {
                            cmiPaste.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Delete:
                        if (aText != null)
                        {
                            cmiDelete.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Rename:
                        if (aText != null)
                        {
                            cmiRename.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Open:
                        if (aText != null)
                        {
                            cmiOpen.Text = aText;
                        }
                        break;

                    case eExplorerActionMenuItem.Refresh:
                        if (aText != null)
                        {
                            cmiRefresh.Text = aText;
                        }
                        break;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#335

        /// <summary>
        /// Add menu item to context menu based on eExplorerActionMenuItem
        /// </summary>
        /// <param name="aToolStripMenuItem">The menu item to be added</param>
        /// <param name="aParentItem">The parent menu item where the item is to be added.  Use None for main menu. </param>
        /// <param name="aPriorItem">The eExplorerActionMenuItem of the item that is to be above this item in the menu.  Use None to insert at top.</param>
        /// <remarks>Use this method for base menu items only.</remarks>

		protected void AddContextMenuItem(ToolStripItem aToolStripMenuItem, eExplorerActionMenuItem aParentItem, eExplorerActionMenuItem aPriorItem)
        {
            string priorItemName = string.Empty;
            try
            {
                switch (aPriorItem)
                {
                    case eExplorerActionMenuItem.New:
                        priorItemName = cmiNew.Name;
                        break;

                    case eExplorerActionMenuItem.NewFolder:
                        priorItemName = cmiNewFolder.Name;
                        break;

                    case eExplorerActionMenuItem.NewItem:
                        priorItemName = cmiNewItem.Name;
                        break;

                    case eExplorerActionMenuItem.Cut:
                        priorItemName = cmiCut.Name;
                        break;

                    case eExplorerActionMenuItem.Copy:
                        priorItemName = cmiCopy.Name;
                        break;

                    case eExplorerActionMenuItem.Paste:
                        priorItemName = cmiPaste.Name;
                        break;

                    case eExplorerActionMenuItem.Delete:
                        priorItemName = cmiDelete.Name;
                        break;

                    case eExplorerActionMenuItem.Rename:
                        priorItemName = cmiRename.Name;
                        break;

                    case eExplorerActionMenuItem.Open:
                        priorItemName = cmiOpen.Name;
                        break;

                    case eExplorerActionMenuItem.Refresh:
                        priorItemName = cmiRefresh.Name;
                        break;

                    case eExplorerActionMenuItem.EditSeparator:
                        priorItemName = cmiEditSeparator.Name;
                        break;

                    case eExplorerActionMenuItem.ProcessSeparator:
                        priorItemName = cmiProcessSeparator.Name;
                        break;

                    case eExplorerActionMenuItem.RefreshSeparator:
                        priorItemName = cmiRefreshSeparator.Name;
                        break;

                    default:
                        throw new Exception("Do not use this method if not a base menu item");
                }

                AddContextMenuItem(aToolStripMenuItem, aParentItem, priorItemName);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Add menu item to context menu based on the name of the prior item
        /// </summary>
        /// <param name="aToolStripMenuItem">The menu item to be added</param>
        /// <param name="aParentItem">The parent menu item where the item is to be added.  Use None for main menu. </param>
        /// <param name="aPriorItem">The name of the item that is to be above this item in the menu.  Use None to insert at top.</param>
        /// <remarks>Use this method for custom menu items.</remarks>

		protected void AddContextMenuItem(ToolStripItem aToolStripMenuItem, eExplorerActionMenuItem aParentItem, string aPriorItemName)
        {
            int index;
            ToolStripItemCollection items;

            try
            {
                if (aParentItem == eExplorerActionMenuItem.New)
                {
                    items = cmiNew.DropDownItems;
                }
                else
                {
                    items = cmsNodeAction.Items;
                }

                index = 0;
                if (aPriorItemName != null)
                {
                    foreach (ToolStripItem item in items)
                    {
                        ++index;
                        if (item.Name == aPriorItemName)
                        {
                            break;
                        }
                    }
                }

                if (index < cmsNodeAction.Items.Count)
                {
                    items.Insert(index, aToolStripMenuItem);
                }
                else
                {
                    items.Add(aToolStripMenuItem);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		/// <summary>
		/// Virtual method that gets the text for the New Item menu item.
		/// </summary>
		/// <returns>
		/// The text for the New Item menu item.
		/// </returns>

		virtual protected string GetNewItemText(MIDTreeNode aCurrentNode)
		{
			throw new Exception("Method GetNewItemName() must be overridden by inheriting class");
		}

		//--------------
		// Event Methods
		//--------------

		private void cmsNodeAction_Opening(object sender, CancelEventArgs e)
		{
			try
			{
				NodeActionMenuOpening();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiNewItem_Click(object sender, EventArgs e)
		{
			MIDTreeNode parentNode;
			//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename
			MIDTreeNode newNode;
			//End Track #6257 - JScott - Create New Attribute requires user to right-click rename

			try
			{
				if (_treeView.SelectedNode != null)
				{
					parentNode = (MIDTreeNode)TreeView.SelectedNode;

					//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename
					//TreeView.CreateNewTreeItem(_treeView.SelectedNode);
					newNode = TreeView.CreateNewTreeItem(_treeView.SelectedNode);

					if (newNode != null)
					{
						newNode.BeginEdit();
					}
					//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiNewFolder_Click(object sender, EventArgs e)
		{
			MIDTreeNode currNode;
			MIDTreeNode parentNode;
			MIDTreeNode topNode;
            MIDTreeNode newNode = null;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                currNode = (MIDTreeNode)TreeView.SelectedNode;
                parentNode = currNode.GetParentNode();
				topNode = currNode.GetTopSourceNode();

                TreeView.BeginUpdate();

				try
				{
					if (topNode.isMainFavoriteFolder)
					{
						if (currNode.isFolder)
						{
							newNode = TreeView.CreateNewTreeFolder(currNode, SAB.ClientServerSession.UserRID);
						}
						else
						{
							newNode = TreeView.CreateNewTreeFolder(parentNode, SAB.ClientServerSession.UserRID);
						}
					}
					else
					{
						if (currNode.isFolder)
						{
							newNode = TreeView.CreateNewTreeFolder(currNode, currNode.UserId);
						}
						else
						{
							newNode = TreeView.CreateNewTreeFolder(parentNode, parentNode.UserId);
						}
					}
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					TreeView.EndUpdate();
				}

                if (newNode != null)
                {
                    newNode.BeginEdit();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
		}

		private void cmiCut_Click(object sender, EventArgs e)
		{
			try
			{
				ICut();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiCopy_Click(object sender, EventArgs e)
		{
			try
			{
				ICopy();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiPaste_Click(object sender, EventArgs e)
		{
			try
			{
				IPaste();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiDelete_Click(object sender, EventArgs e)
		{
			try
			{
				IDelete();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiRename_Click(object sender, EventArgs e)
		{
			try
			{
				if (TreeView.SelectedNode != null)
				{
					TreeView.SelectedNode.BeginEdit();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiOpen_Click(object sender, EventArgs e)
		{
			try
			{
                _treeView.EditTreeNode((MIDTreeNode)_treeView.SelectedNode);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cmiRefresh_Click(object sender, EventArgs e)
		{
			try
			{
				IRefresh();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void Explorer_Load(object sender, System.EventArgs e)
		{
			try
			{
				//Begin TT#926 - JScott - Workspace Explorerer-Rt Click on a method, and "Process" is displayed 4 times in list
				//ExplorerLoad();
				//BuildTreeView();
				//InitializeContextmenu();
				//// Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
				//SetText();
				//// End TT#373

				//if (TreeView != null)
				//{
				//    // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
				//    TreeView.OnMIDNodeSelect += new MIDTreeView.MIDTreeViewNodeSelectHandler(TreeView_OnMIDNodeSelect);
				//    // Begin TT#373
				//    TreeView.ExpandFavoritesNode();
				//}

				//End TT#926 - JScott - Workspace Explorerer-Rt Click on a method, and "Process" is displayed 4 times in list
				_formLoaded = true;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        void TreeView_OnMIDNodeSelect(object source, MIDTreeNode node)
        {
            // Begin TT#384 - JSmith - Error removing child from hierarchy
            // protect if child already removed
            if (OutOfDate(node))
            {
                string Title, Msg;
                MessageBoxIcon icon;
                MessageBoxButtons buttons;
                buttons = MessageBoxButtons.OK;
                icon = MessageBoxIcon.Warning;
                Title = MIDText.GetTextOnly(eMIDTextCode.lbl_AutoRefresh);
                Msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AutoRefreshExplorer);
                MessageBox.Show(this, Msg, Title,
                buttons, icon);
                IRefresh();
                return;
            }
            // End TT#384

            _showNewFolder = false;
            _showNewItem = false;
            _showCut = false;
            _showCopy = false;
            _showPaste = false;
            _showDelete = false;
            _showRename = false;
            _showOpen = false;
            _showInUse = false; //TT#110-MD-VStuart - In Use Tool
            _showRefresh = true;
            _showRemove = true;

            foreach (MIDTreeNode currNode in _treeView.GetSelectedNodes())
            {
                // Begin TT#478-MD - JSmith - workflow method explorer global - fill size holes method try to  copy/paste a fill size method from one folder to another and received an unhandled exception.
                if (currNode == null)
                {
                    continue;
                }
                // End TT#478-MD - JSmith - workflow method explorer global - fill size holes method try to  copy/paste a fill size method from one folder to another and received an unhandled exception.
                CheckNodeSecurity(currNode);
            }

            if (_showRemove)
            {
                RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
            }
            else
            {
                RenameMenuItem(this, eMIDMenuItem.EditDelete, sDelete);
            }

            if (_showDelete)
            {
                EnableMenuItem(this, eMIDMenuItem.EditDelete);
            }
            else 
            {
                DisableMenuItem(this, eMIDMenuItem.EditDelete);
            }

            if (_showPaste)
            {
                EnableMenuItem(this, eMIDMenuItem.EditPaste);
            }
            else
            {
                DisableMenuItem(this, eMIDMenuItem.EditPaste);
            }
        }
        // End TT#373

		protected void MIDTreeView_OnMIDDoubleClick(object source, MIDTreeNode node)
		{
			try
			{
                _treeView.EditTreeNode(node);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        //-----------------------
        // Miscellaneous routines
        //-----------------------

		protected ArrayList GetUsersAssignedToMe(int aUserRID)
		{
			DataTable dtUsers;
			ArrayList assignedUserRID = new ArrayList();

			try
			{
				dtUsers = _dlSecurity.GetUsersAssignedToMe(aUserRID);

				foreach (DataRow drUsers in dtUsers.Rows)
				{
					assignedUserRID.Add(Convert.ToInt32(drUsers["OWNER_USER_RID"]));
				}

				return assignedUserRID;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the controls on the form based on the security of the user
		/// </summary>

		protected void SetReadOnly(bool aAllowUpdate)
		{
			bool readOnlyFlag;
			bool enabledFlag;

			try
			{
				_allowUpdate = aAllowUpdate;
				readOnlyFlag = false;
				enabledFlag = true;

				if (!_allowUpdate)
				{
					readOnlyFlag = true;
					enabledFlag = false;
				}
				else
				{
					readOnlyFlag = false;
					enabledFlag = true;
				}

				// The following text box is only used to determine the appropriate system colors
				txtGetReadOnlyColor = new TextBox();
				txtGetReadOnlyColor.Visible = false;
				txtGetReadOnlyColor.ReadOnly = readOnlyFlag;

				// Loop through all controls in the form's control collection.
				foreach (Control ctrl in this.Controls)
				{
					// check for sub-controls for control like groupbox, panel, etc.  
					// If found, recursively loop through all sub-controls in the controls' control collection.  
					if (ctrl.Controls.Count > 0)
					{
						LoopReadOnly(ctrl, readOnlyFlag, enabledFlag);
					}
					else
					{
						// set the control to read only
						SetReadOnly(ctrl, readOnlyFlag, enabledFlag);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Sets the controls on the control based on the security of the user
		/// </summary>
		/// <param name="aCtrl">The control for which read only is to be set</param>
		/// <param name="aReadOnlyFlag">A flag identifying if the control and all subcontrols should be set to read only</param>

		protected void SetControlReadOnly(Control aCtrl, bool aReadOnlyFlag)
		{
			bool readOnlyFlag;
			bool enabledFlag;

			try
			{
				if (aReadOnlyFlag)
				{
					readOnlyFlag = true;
					enabledFlag = false;
				}
				else
				{
					readOnlyFlag = false;
					enabledFlag = true;
				}

				// The following text box is only used to determine the appropriate system colors
				txtGetReadOnlyColor = new TextBox();
				txtGetReadOnlyColor.Visible = false;
				txtGetReadOnlyColor.ReadOnly = readOnlyFlag;

				SetReadOnly(aCtrl, readOnlyFlag, enabledFlag);

				// Loop through all controls in the form's control collection.
				foreach (Control ctrl in aCtrl.Controls)
				{
					// check for sub-controls for control like groupbox, panel, etc.  
					// If found, recursively loop through all sub-controls in the controls' control collection.  
					if (ctrl.Controls.Count > 0)
					{
						LoopReadOnly(ctrl, readOnlyFlag, enabledFlag);
					}
					else
					{
						// set the control to read only
						SetReadOnly(ctrl, readOnlyFlag, enabledFlag);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method executed during the Opening event for the NodeAction Menu.
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode that was clicked on
		/// </param>

		private void NodeActionMenuOpening()
		{
            //bool showRemove = false;

			try
			{
				_showNewFolder = false;
				_showNewItem = false;
				_showCut = false;
				_showCopy = false;
				_showPaste = false;
				_showDelete = false;
				_showRename = false;
				_showOpen = false;
                _showInUse = false; //TT#110-MD-VStuart - In Use Tool
				_showRefresh = true;
                // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                _showRemove = true;
                // End TT#373

                // Begin Track #6291 - JSmith - Null reference when right click next to view drop down
                //if (_treeView.SelectedNode != null)
                if (_treeView != null && _treeView.SelectedNode != null)
                // End Track #6291
				{
                    foreach (MIDTreeNode currNode in _treeView.GetSelectedNodes())
                    {
                        cmiNewItem.Text = GetNewItemText(currNode);

                        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                        CheckNodeSecurity(currNode);
                        // End TT#373

                        CustomizeActionMenu(currNode);
                    }
				}

				//if (showRemove)
				//{
				//    cmiDelete.Text = "Remove";
				//}
				//else
				//{
				//    cmiDelete.Text = "Delete";
				//}

                // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
                if (_showRemove)
                {
                    RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
                    CustomizeTextMenuItem(eExplorerActionMenuItem.Delete, sRemove);
                    RenameMenuItem(this, eMIDMenuItem.EditDelete, sRemove);
                }
                else
                {
                    CustomizeTextMenuItem(eExplorerActionMenuItem.Delete, sDelete);
                    RenameMenuItem(this, eMIDMenuItem.EditDelete, sDelete);
                }
                // End TT#373

                cmiNew.Visible = false;
				cmiNewFolder.Visible = _showNewFolder;
				cmiNewItem.Visible = _showNewItem;
				cmiCut.Visible = _showCut;
				cmiCopy.Visible = _showCopy;
				cmiPaste.Visible = _showPaste;
				cmiDelete.Visible = _showDelete;
				cmiRename.Visible = _showRename;
				cmiOpen.Visible = _showOpen;
				cmiRefresh.Visible = _showRefresh;
                cmiInUse.Visible = _showInUse;  //TT#110-MD-VStuart - In Use Tool

                //if (_showNewItem || _showNewFolder)
                //{
                //    cmiNew.Visible = true;
                //}
                //else
                //{
                //    cmiNew.Visible = false;
                //}

                SetMenuItemsVisible(cmsNodeAction.Items);

                SetMenuSeparatorsVisible(cmsNodeAction.Items);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        private void CheckNodeSecurity(MIDTreeNode currNode)
        {
            try
            {
                // Begin TT#4047 - JSmith - User freezes dragging a merchandise hierarchy to a method
                //if (currNode.isLabelEditAllowed())
                //{
                //    _showRename = true;
                //}
                if (!((MIDTreeView)TreeView).ItemBeingDragged)
                {
                    if (currNode.isLabelEditAllowed())
                    {
                        _showRename = true;
                    }
                }
                // End TT#4047 - JSmith - User freezes dragging a merchandise hierarchy to a method
                //_showInUse = true;  //TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID

                if (currNode.isObjectShortcut)
                {
					//Begin TT#1255 - JScott - Tasklist disappears after save and refresh
					//if (currNode.GetTopNode().isMainFavoriteFolder &&
					//    currNode.FunctionSecurityProfile.AllowUpdate)
					//{
					//    if (!currNode.isShared)
					//    {
					//        _showNewItem = true;
					//    }
					//}

					//End TT#1255 - JScott - Tasklist disappears after save and refresh
                    _showInUse = true;  //TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
					if (currNode.FunctionSecurityProfile.AllowDelete)
                    {
                        _showDelete = true;
                        // Begin TT#394 - JSmith - Allow cut/paste of shortcuts
                        _showCut = true;
                        // End TT#394
                    }

                    if (currNode.FunctionSecurityProfile.AllowUpdate)
                    {
                        _showOpen = true;
                        //BEGIN TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                        //_showInUse = true;  //TT#110-MD-VStuart - In Use Tool
                    }
                    _showInUse = true;  //TT#110-MD-VStuart - In Use Tool
                    //END TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID

                    // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    if (!currNode.FunctionSecurityProfile.AccessDenied)
                    {
                        _showCopy = true;
                    }
                    // End TT#394
                }
                else if (currNode.isFolderShortcut)
                {
                    if (currNode.FunctionSecurityProfile.AllowDelete)
                    {
                        _showDelete = true;
                    }
                }
                else
                {
                    switch (currNode.TreeNodeType)
                    {
                        case eTreeNodeType.MainFavoriteFolderNode:

                            if (currNode.FolderSecurityProfile.AllowUpdate)
                            {
                                if (!currNode.isShared)
                                {
                                    _showNewFolder = true;
                                }
                            }

                            if (currNode.FunctionSecurityProfile.AllowUpdate)
                            {
                                if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                {
                                    _showPaste = true;
                                }
                                // Begin TT#564 - JSmith - Copy/Paste from search not working
                                else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                {
                                    _showPaste = true;
                                }
                                // End TT#564
                            }

                            break;

                        case eTreeNodeType.MainSourceFolderNode:

                            if (currNode.FolderSecurityProfile.AllowUpdate)
                            {
                                if (!currNode.isShared)
                                {
                                    _showNewFolder = true;
                                }
                            }

                            if (currNode.FunctionSecurityProfile.AllowUpdate)
                            {
                                if (!currNode.isShared)
                                {
                                    _showNewItem = true;
                                }

                               if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                {
                                    _showPaste = true;
                                }
                                else if (_cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Move, currNode, _cutCopyNodes) &&
                                            _cutCopyNodes.Count > 0 && currNode.GetTopSourceNode().Profile.ProfileType == ((MIDTreeNode)_cutCopyNodes[0]).GetTopSourceNode().Profile.ProfileType)
                                {
                                    _showPaste = true;
                                }
                               // Begin TT#564 - JSmith - Copy/Paste from search not working
                               else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                               {
                                   _showPaste = true;
                               }
                               else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                               {
                                   _showPaste = true;
                               }
                                // End TT#564
                            }

                            break;

                        case eTreeNodeType.ChildObjectShortcutNode:

                            if (currNode.FunctionSecurityProfile.AllowUpdate)
                            {
                                _showOpen = true;
                                //_showInUse = true;  //TT#110-MD-VStuart - In Use Tool
                            //BEGIN TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                                //_showInUse = false;  //TT#655-MD-VStuart-Remove In Use Menu Display from Merchandise Hierarchy sub nodes
                            }
                            _showInUse = false;  //TT#655-MD-VStuart-Remove In Use Menu Display from Merchandise Hierarchy sub nodes
                            //END TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID

                            break;

                        case eTreeNodeType.SubFolderNode:

                            switch (currNode.GetTopSourceNode().TreeNodeType)
                            {
                                case eTreeNodeType.MainFavoriteFolderNode:

                                    if (!currNode.FunctionSecurityProfile.AccessDenied)
                                    {
                                        _showCopy = true;
                                    }

                                    if (currNode.FolderSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewFolder = true;
                                        }
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowUpdate)
                                    {
                                        if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                        {
                                            _showPaste = true;
                                        }
                                        // only allow cut/paste if within same main folder
                                        else if (_cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Move, currNode, _cutCopyNodes) &&
                                            _cutCopyNodes.Count > 0 && currNode.GetTopSourceNode().Profile.ProfileType == ((MIDTreeNode)_cutCopyNodes[0]).GetTopSourceNode().Profile.ProfileType)
                                        {
                                            _showPaste = true;
                                        }
                                        // Begin TT#564 - JSmith - Copy/Paste from search not working
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        // End TT#564
                                    }

                                    if (currNode.FolderSecurityProfile.AllowDelete)
                                    {
                                        _showCut = true;
                                        _showDelete = true;
                                    }

                                    break;

                                case eTreeNodeType.MainSourceFolderNode:

                                    if (!currNode.FunctionSecurityProfile.AccessDenied)
                                    {
                                        _showCopy = true;
                                    }

                                    if (currNode.FolderSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewFolder = true;
                                        }
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewItem = true;
                                        }

                                        if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                        {
                                            _showPaste = true;
                                        }
                                        // only allow cut/paste if within same main folder
                                        else if (_cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Move, currNode, _cutCopyNodes) &&
                                            _cutCopyNodes.Count > 0 && currNode.GetTopSourceNode().Profile.ProfileType == ((MIDTreeNode)_cutCopyNodes[0]).GetTopSourceNode().Profile.ProfileType)
                                        {
                                            _showPaste = true;
                                        }
                                        // Begin TT#564 - JSmith - Copy/Paste from search not working
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        // End TT#564
                                    }

                                    if (currNode.FolderSecurityProfile.AllowDelete)
                                    {
                                        _showCut = true;
                                        _showDelete = true;
                                    }

                                    break;
                            }

                            break;

                        case eTreeNodeType.ObjectNode:

                            switch (currNode.GetTopSourceNode().TreeNodeType)
                            {
                                case eTreeNodeType.MainFavoriteFolderNode:

                                    if (!currNode.FunctionSecurityProfile.AccessDenied)
                                    {
                                        _showCopy = true;
                                    }

                                    if (currNode.FolderSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewFolder = true;
                                        }
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowUpdate)
                                    {
                                        if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                        {
                                            _showPaste = true;
                                        }
                                        // Begin TT#564 - JSmith - Copy/Paste from search not working
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        // End TT#564
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowDelete)
                                    {
                                        _showCut = true;
                                        _showDelete = true;
                                    }

                                    _showOpen = true;
                                    _showInUse = true;  //TT#110-MD-VStuart - In Use Tool

                                    break;

                                case eTreeNodeType.MainSourceFolderNode:

                                    if (!currNode.FunctionSecurityProfile.AccessDenied)
                                    {
                                        _showCopy = true;
                                    }

                                    if (currNode.FolderSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewFolder = true;
                                        }
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowUpdate)
                                    {
                                        if (!currNode.isShared)
                                        {
                                            _showNewItem = true;
                                        }

                                        if (_cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Copy, currNode, _cutCopyNodes))
                                        {
                                            _showPaste = true;
                                        }
                                        // only allow cut/paste if within same main folder
                                        else if (_cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isDropAllowed(DragDropEffects.Move, currNode, _cutCopyNodes) &&
                                            _cutCopyNodes.Count > 0 && currNode.GetTopSourceNode().Profile.ProfileType == ((MIDTreeNode)_cutCopyNodes[0]).GetTopSourceNode().Profile.ProfileType)
                                        {
                                            _showPaste = true;
                                        }
                                        // Begin TT#564 - JSmith - Copy/Paste from search not working
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Copy && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        else if (_pasteFromClipboard && _cutCopyOperation == eCutCopyOperation.Cut && ((MIDTreeView)currNode.TreeView).isPasteFromClipboardAllowed(DragDropEffects.Copy, currNode))
                                        {
                                            _showPaste = true;
                                        }
                                        // End TT#564
                                    }

                                    if (currNode.FunctionSecurityProfile.AllowDelete)
                                    {
                                        _showCut = true;
                                        _showDelete = true;
                                    }

                                    _showOpen = true;
                                    _showInUse = true;  //TT#110-MD-VStuart - In Use Tool

                                    break;
                            }

                            break;
                    }
                }
                if (currNode.TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                    currNode.TreeNodeType == eTreeNodeType.ObjectShortcutNode)
                {

                }
                else
                {
                    _showRemove = false;
                }
            }
            catch 
            {
                throw;
            }
        }

        private void ExplorerBase_Leave(object sender, EventArgs e)
        {
            RenameMenuItem(this, eMIDMenuItem.EditDelete, sDelete);
        }

        // End TT#373

        /// <summary>
        /// Recursive routine that checks all menu items to determine if separators need to be visible
        /// </summary>
        /// <param name="aItems">The ToolStripItemCollection to search</param>

        private bool SetMenuItemsVisible(ToolStripItemCollection aItems)
        {
            ToolStripMenuItem menuItem;
            bool isChildVisible;
            try
            {
                isChildVisible = false;
                foreach (ToolStripItem item in aItems)
                {
                    if (item is ToolStripMenuItem)
                    {
                        menuItem = (ToolStripMenuItem)item;
                        if (menuItem.Visible ||
                            menuItem.Available)
                        {
                            isChildVisible = true;
                        }
                        else if (!menuItem.Visible &&
                            !menuItem.Available)
                        {
                            if (menuItem.DropDownItems.Count > 0 &&
                                SetMenuItemsVisible(menuItem.DropDownItems))
                            {
                                item.Visible = true;
                                isChildVisible = true;
                            }
                        }
                    }
                }
                return isChildVisible;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Routine that checks all menu items to determine if separators need to be visible
        /// </summary>
        /// <param name="aItems">The ToolStripItemCollection to search</param>

		private void SetMenuSeparatorsVisible(ToolStripItemCollection aItems)
        {
            ArrayList availableItemsList;
            bool menuItemFoundBefore;
            int index;
           
            try
            {
                availableItemsList = new ArrayList();
                // put available items and ToolStripSeparators in list
                foreach (ToolStripItem item in aItems)
                {
                    if (item is ToolStripSeparator ||
                        item.Available)
                    {
                        availableItemsList.Add(item);
                    }
                }

                // find all separators and check for prior and next item for menu items
                menuItemFoundBefore = false;
                index = 0;
                foreach (ToolStripItem item in availableItemsList)
                {
                    if (item is ToolStripSeparator)
                    {
                        if (menuItemFoundBefore &&
                            MenuItemFoundAfter(availableItemsList, index))
                        {
                            item.Visible = true;
                        }
                        else
                        {
                            item.Visible = false;
                        }
                        menuItemFoundBefore = false;
                    }
                    else if (item is ToolStripMenuItem)
                    {
                        menuItemFoundBefore = true;
                    }
                    ++index;
                }
            }
            catch
            {
                throw;
            }
        }

        private bool MenuItemFoundAfter(ArrayList aAvailableItemsList, int aIndex)
        {
            ToolStripItem item;
            for (int i = aIndex; i < aAvailableItemsList.Count; i++)
            {
                item = (ToolStripItem) aAvailableItemsList[i];
                if (item is ToolStripMenuItem)
                {
                    return true;
                }
            }
            return false;
        }

		/// <summary>
		/// Recursive loop to go through all controls and sub-controls
		/// </summary>
		/// <param name="aCtrl">The current control</param>
		/// <param name="aReadOnlyFlag">A flag identifying if the form should be set to read only</param>
		/// <param name="aEnabledFlag">A flag identifying if objects should be disabled if read only</param>

		private void LoopReadOnly(Control aCtrl, bool aReadOnlyFlag, bool aEnabledFlag)
		{
			try
			{
				if (aCtrl.Controls.Count > 0)
				{
					// NumericUpDown has sub-controls need to set control collection before sub controls
					if (aCtrl.GetType() == typeof(System.Windows.Forms.NumericUpDown))
					{
						SetReadOnly(aCtrl, aReadOnlyFlag, aEnabledFlag);
						// This is hokey but I could not find another way to do this.
						// NumericUpDown has two sub-controls Forms.UpDownBase.UpDownButtons and
						// Forms.UpDownBase.UpDownEdit.  If both sub-controls are disabled, the NumericUpDown
						// control shows as disabled instead of read only.  But, if only the UpDownButtons
						// are disabled, the control shows as read only.  John Smith
						aCtrl.Controls[0].Enabled = aEnabledFlag;
					}
					else
					{
						foreach (Control subCtrl in aCtrl.Controls)
						{
							LoopReadOnly(subCtrl, aReadOnlyFlag, aEnabledFlag);
						}
					}
				}
				else
				{
					// set the control to read only
					SetReadOnly(aCtrl, aReadOnlyFlag, aEnabledFlag);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Set the properties of the control based on the readOnlyFlag
		/// </summary>
		/// <param name="ctrl">The current control</param>
		/// <param name="readOnlyFlag">A flag identifying if the form should be set to read only</param>
		/// <param name="enabledFlag">A flag identifying if objects should be disabled if read only</param>

		private void SetReadOnly(Control aCtrl, bool aReadOnlyFlag, bool aEnabledFlag)
		{
			UltraGrid ultragid;

			try
			{
				// format control style
				if ( aCtrl.GetType().BaseType == typeof(ButtonBase) )
				{
					((ButtonBase)aCtrl).FlatStyle = FlatStyle.System;
				}

				if (aCtrl.GetType() == typeof(System.Windows.Forms.TextBox))
				{
					((TextBox)aCtrl).ReadOnly = aReadOnlyFlag;
				}
				else if (aCtrl.GetType() == typeof(MIDRetail.Windows.MaskedEdit))
				{
					((MIDRetail.Windows.MaskedEdit)aCtrl).ReadOnly = aReadOnlyFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.NumericUpDown))
				{
					((NumericUpDown)aCtrl).ReadOnly = aReadOnlyFlag;
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.UpDownBase))
				{
					((UpDownBase)aCtrl).ReadOnly = aReadOnlyFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.ComboBox))
				{
					// process all ComboBoxes that start with 'cbo'.  All others are unaffected.
					if (aCtrl.Name.StartsWith("cbo"))
					{
						aCtrl.Enabled = aEnabledFlag;
					}
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.CheckBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.CheckedListBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.DateTimePicker))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.ListBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.RadioButton))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.Button))
				{
					if (aCtrl.Name == "btnHelp")
					{
						aCtrl.Hide();
					}
					else
						// do not disable the cancel, close and process buttons.
						if (aCtrl.Name != "btnCancel" &&
						aCtrl.Name != "btnClose" &&
						aCtrl.Name != "btnProcess")
					{
						aCtrl.Enabled = aEnabledFlag;
					}
				}
				else if (aCtrl.GetType() == typeof(System.Windows.Forms.PictureBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else if (aCtrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
				{
					ultragid = (UltraGrid)aCtrl;

					foreach (Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands)
					{
						// disables any Add New buttons on the grid
						if (!AllowUpdate)
						{
							oBand.Override.AllowAddNew = AllowAddNew.No;
						}

						foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns )
						{
							if (!AllowUpdate)
							{
								oColumn.CellActivation = Activation.NoEdit;

								if (oColumn.Style == Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton)
								{
									oColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Edit;
								}
							}
							else
							{
								oColumn.CellActivation = Activation.AllowEdit;
							}
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SaveExpandedNodes(TreeNodeCollection aNodes, Hashtable aExpandedList)
		{
			try
			{
				foreach (MIDTreeNode node in aNodes)
				{
					if (node.IsExpanded)
					{
						aExpandedList[new HashKeyObject((int)node.TreeNodeType, (int)node.NodeProfileType, node.Profile.Key)] = null;
					}

					SaveExpandedNodes(node.Nodes, aExpandedList);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ResetExpandedNodes(TreeNodeCollection aNodes, Hashtable aExpandedList)
		{
			try
			{
				foreach (MIDTreeNode node in aNodes)
				{
					if (node.Nodes.Count > 0 && node.Profile != null)
					{
						if (aExpandedList.Contains(new HashKeyObject((int)node.TreeNodeType, (int)node.NodeProfileType, node.Profile.Key)))
						{
							node.Expand();
						}

						ResetExpandedNodes(node.Nodes, aExpandedList);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		protected Form GetForm(System.Type aType, object[] args, bool aCreateNewForm, bool aGetLastForm)
		{
			bool foundForm;
			Form frm;

			try
			{
				foundForm = false;
				frm = null;

				foreach (Form childForm in this.ParentForm.MdiChildren) 
				{
					if (childForm.GetType().Equals(aType))
					{
						frm = childForm;
						foundForm = true;

						if (!aGetLastForm)
						{
							break;
						}
					}
				}

				if (aCreateNewForm && !foundForm)
				{
					frm = (System.Windows.Forms.Form)Activator.CreateInstance(aType, args);
				}

				return frm;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        protected void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
			UltraGridLayoutDefaults ugld;

			try
			{
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //ugld = new UltraGridLayoutDefaults();
                ugld = new UltraGridLayoutDefaults(ErrorImage);
                //End TT#169
				ugld.ApplyAppearance(aUltraGrid);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
        }

        protected void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraDropDown aUltraDropDown)
        {
			UltraGridLayoutDefaults ugld;

			try
			{
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //ugld = new UltraGridLayoutDefaults();
                ugld = new UltraGridLayoutDefaults(ErrorImage);
                //End TT#169
				ugld.ApplyAppearance(aUltraDropDown);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
        }

		public void Image_DragLeave(object sender, EventArgs e)
		{
			try
			{
				DragHelper.ImageList_DragLeave(Handle);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		public void Image_DragOver(object sender, DragEventArgs e)
		{
			Point p;

			try
			{
				p = PointToClient(new Point(e.X, e.Y));
				DragHelper.ImageList_DragMove((int)(p.X), (int)(p.Y));
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		public void Image_DragEnter(object sender, DragEventArgs e)
		{
			Point p;
            TreeNodeClipboardList cbList;
			int xPos, yPos;

			try
			{
				// Get mouse position in client coordinates
				p = this.PointToClient(Control.MousePosition);

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    MIDGraphics.BuildDragImage(cbList, imageListDrag, 0, _spacing,
								Font, ForeColor, out _imageHeight, out _imageWidth);

					xPos = _imageWidth / 2;
					yPos = _imageHeight;

					// Begin dragging image
					DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos);
				}

				DragHelper.ImageList_DragEnter(Handle, (int)(p.X), (int)(p.Y));
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		#region Menu

		public void ShowMenuItem(object aSource, eMIDMenuItem aMenuItem)
		{
			try
			{
				_SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Show);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HideMenuItem(object aSource, eMIDMenuItem aMenuItem)
		{
			try
			{
				_SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Hide);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void EnableMenuItem(object aSource, eMIDMenuItem aMenuItem)
		{
			try
			{
				_SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Enable);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void DisableMenuItem(object aSource, eMIDMenuItem aMenuItem)
		{
			try
			{
				_SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Disable);

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        public void RenameMenuItem(object aSource, eMIDMenuItem aMenuItem, string aText)
        {
            try
            {
                _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Rename, aText);

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#335

		#endregion

		#region Exception Handling

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <remarks>Uses this.name for module.  Logs all inner exceptions.</remarks>
		
		protected void HandleException(Exception ex)
		{
			HandleException(ex, this.Name, false, eExceptionLogging.logAllInnerExceptions);
		}

		/// <summary>
		/// Handle exception.  Uses this.name for module.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="rethrow">A flag identifying if the exception should be rethrown</param>
		/// <remarks>Uses this.name for module.  Logs all inner exceptions.</remarks>
		
		protected void HandleException(Exception ex, bool rethrow)
		{
			HandleException(ex, this.Name, rethrow, eExceptionLogging.logAllInnerExceptions);
		}

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="moduleName">The name of the module throwing the exception.</param>
		/// <remarks>Logs all inner exceptions.</remarks>
		
		protected void HandleException(Exception ex, string moduleName)
		{
			HandleException(ex, moduleName, false, eExceptionLogging.logAllInnerExceptions);
		}

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="moduleName">The name of the module throwing the exception.</param>
		/// <param name="rethrow">A flag identifying if the exception should be rethrown</param>
		/// <remarks>Logs all inner exceptions.</remarks>
		
		protected void HandleException(Exception ex, string moduleName, bool rethrow)
		{
			HandleException(ex, moduleName, rethrow, eExceptionLogging.logAllInnerExceptions);
		}

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>
		
		protected void HandleException(Exception ex, eExceptionLogging logOnlyInnerMostException)
		{
			HandleException(ex, this.Name, false, logOnlyInnerMostException);
		}

		/// <summary>
		/// Handle exception.  Uses this.name for module.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="rethrow">A flag identifying if the exception should be rethrown</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>
		
		protected void HandleException(Exception ex, bool rethrow, eExceptionLogging logOnlyInnerMostException)
		{
			HandleException(ex, this.Name, rethrow, logOnlyInnerMostException);
		}

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="moduleName">The name of the module throwing the exception.</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>
	
		protected void HandleException(Exception ex, string moduleName, eExceptionLogging logOnlyInnerMostException)
		{
			HandleException(ex, moduleName, false, logOnlyInnerMostException);
		}

		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <param name="moduleName">The name of the module throwing the exception.</param>
		/// <param name="rethrow">A flag identifying if the exception should be rethrown</param>
		/// <param name="logOnlyInnerMostException">This enumeration identifies if only the inner most exception
		/// is to be logged or if all inner exceptions are to be logged.</param>

		protected void HandleException(Exception ex, string moduleName, bool rethrow, eExceptionLogging logOnlyInnerMostException)
		{
			_SAB.ClientServerSession.Audit.Log_Exception(ex, moduleName, logOnlyInnerMostException);
			// show only the last since it should be real error
			if (ex is MIDException)
			{
				HandleMIDException((MIDException)ex);
			}
			else
			{
				MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
			}
			if (rethrow)
			{
				throw ex;
			}
		}

		private void HandleMIDException(MIDException MIDexc)
		{
			string Title, errLevel, Msg; 
			MessageBoxIcon icon;
			MessageBoxButtons buttons;
			buttons = MessageBoxButtons.OK;

			switch (MIDexc.ErrorLevel)
			{
				case eErrorLevel.severe:
					icon = MessageBoxIcon.Stop;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
					break;
				
				case eErrorLevel.information:
					icon = MessageBoxIcon.Information;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Information));
					break;
				
				case eErrorLevel.warning:
					icon = MessageBoxIcon.Warning;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Warning));
					break;

                //Begin TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                case eErrorLevel.error:
                    icon = MessageBoxIcon.Error;
                    errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Error));
                    break;

                //End TT#1020 - JScott - Add new Error level to the eErrorLevel enumerator
                default:
					icon = MessageBoxIcon.Stop;
					errLevel = MIDText.GetText(Convert.ToInt32(eMIDMessageLevel.Severe));
					break;
			}
			if (MIDexc.InnerException != null)
			{
				Title = errLevel + " - " + MIDexc.Message;
				Msg = MIDexc.InnerException.Message;
			}
			else
			{
				Title = errLevel;
				Msg = MIDexc.Message;
			}
			MessageBox.Show(this, Msg, Title,
				buttons, icon );
		}

		#endregion Exception Handling

		#region IFormBase Members

        // Begin TT#384 - JSmith - Error removing child from hierarchy
        virtual protected bool OutOfDate(MIDTreeNode aNode)
        {
            return false;
        }
        // End TT#384

		virtual public void ICut()
		{
			try
			{
				if (_treeView.SelectedNode != null)
				{
                    _cutCopyNodes = _treeView.GetSelectedNodes();
					_cutCopyOperation = eCutCopyOperation.Cut;
                    // Begin TT#564 - JSmith - Copy/Paste from search not working
                    _pasteFromClipboard = false;
                    // End TT#564
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		virtual public void ICopy()
		{
			try
			{
				if (_treeView.SelectedNode != null)
				{
					_cutCopyNodes = _treeView.GetSelectedNodes();
					_cutCopyOperation = eCutCopyOperation.Copy;
                    // Begin TT#564 - JSmith - Copy/Paste from search not working
                    _pasteFromClipboard = false;
                    // End TT#564
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		virtual public void IPaste()
		{
			try
			{
                // Begin TT#564 - JSmith - Copy/Paste from search not working
                if (_pasteFromClipboard)
                {
                    if (TreeView.PasteFromClipboard(_cutCopyOperation))
                    {
                        if (_cutCopyOperation == eCutCopyOperation.Cut)
                        {
                            _cutCopyOperation = eCutCopyOperation.None;
                        }
                    }
                }
                else if (TreeView.PasteTreeNode(_cutCopyNodes, _cutCopyOperation))
                //if (TreeView.PasteTreeNode(_cutCopyNodes, _cutCopyOperation))
                // End TT#564
				{
                    if (_cutCopyOperation == eCutCopyOperation.Cut)
                    {
                        _cutCopyOperation = eCutCopyOperation.None;
                    }
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}	

		public void IClose()
		{
			try
			{
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        virtual public void INew()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

		virtual public void IFind()
		{
			try
			{
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		virtual public void ISave()
		{
			try
			{
				throw new Exception("Save code goes here");
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		virtual public void ISaveAs()
		{
			try
			{
				throw new Exception("Can not call base method");
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        virtual public void IDelete()
        {
            try
            {
                TreeView.DeleteTreeNode();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        virtual public void IInUse()
        {
            try
            {
                TreeView.InUseTreeNode();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

        virtual public void IRefresh()
		{
			MIDTreeNode currNode;
			Hashtable expandedList;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				try
				{
					if (FormLoaded)
					{
						TreeView.BeginUpdate();
                        // Begin TT#4335 - JSmith - Error on Refresh after creating or removing a shortcut
                        _cutCopyOperation = eCutCopyOperation.None;
                        if (_cutCopyNodes != null)
                        {
                            _cutCopyNodes.Clear();
                        }
                        // End TT#4335 - JSmith - Error on Refresh after creating or removing a shortcut

						try
						{
							currNode = (MIDTreeNode)TreeView.SelectedNode;
							expandedList = new Hashtable();
							SaveExpandedNodes(TreeView.Nodes, expandedList);

							RefreshTreeView();

							ResetExpandedNodes(TreeView.Nodes, expandedList);
                            if (currNode != null)
                            {
                                TreeView.SelectedNode = TreeView.GetNodeByItemRID(TreeView.Nodes, currNode.Profile.Key, currNode.Profile.ProfileType);

                                if (TreeView.SelectedNode != null)
                                {
                                    TreeView.SelectedNode.EnsureVisible();
                                }
                            }
						}
						catch (Exception exc)
						{
							string message = exc.ToString();
							throw;
						}
						finally
						{
							TreeView.EndUpdate();
						}
					}
				}
				catch (Exception ex)
				{
					string message = ex.ToString();
					throw;
				}
				finally
				{
					Cursor.Current = Cursors.Default;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

        // BEGIN Workspace Usability Enhancement
        virtual public void IRestoreLayout()
        {
            try
            {
                throw new Exception("Can not call base method");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // END Workspace Usability Enhancement

		#endregion

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        public void SetCutCopyFromClipboard(eCutCopyOperation aCutCopyOperation)
        {
            _pasteFromClipboard = true;
            _cutCopyOperation = aCutCopyOperation;
            _cutCopyNodes = new ArrayList();
        }
        // End TT#564

        // BEGIN TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only
        protected void AdjustTextWidthComboBox_DropDown(object sender)
        {
            try
            {
                ComboBox senderComboBox;
                if (sender is MIDComboBoxEnh)
                {
                    senderComboBox = ((MIDComboBoxEnh)sender).ComboBox;
                }
                else
                {
                    senderComboBox = senderComboBox = (ComboBox)sender;
                }
                int width = senderComboBox.DropDownWidth;
                Graphics g = senderComboBox.CreateGraphics();
                Font font = senderComboBox.Font;
                //checks if a scrollbar will be displayed.
                //If yes, then get its width to adjust the size of the drop down list.
                int vertScrollBarWidth =
                    (senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
                    ? SystemInformation.VerticalScrollBarWidth : 0;

                //Loop through list items and check size of each items.
                //set the width of the drop down list to the width of the largest item.
                int newWidth;
                foreach (object o in senderComboBox.Items)
                {
                    if (o != null)
                    {
                        string s = string.Empty;
                        string displayMember = senderComboBox.DisplayMember;
                        if (o.GetType() == typeof(DataRowView))
                        {
                            DataRowView drv = (DataRowView)o;

                            if (drv.Row.Table.Columns.Contains(displayMember))
                            {
                                s = drv[displayMember].ToString();
                            }
                            else
                            {
                                s = o.ToString();
                            }
                        }
                        else if (o.GetType() == typeof(StoreGroupListViewProfile))
                        {
                            StoreGroupListViewProfile sgp = (StoreGroupListViewProfile)o;
                            s = sgp.GroupId;
                        }
                        else if (o.GetType() == typeof(StoreGroupLevelListViewProfile))
                        {
                            StoreGroupLevelListViewProfile sglp = (StoreGroupLevelListViewProfile)o;
                            s = sglp.Name;
                        }
                        else
                        {
                            //Type oType = o.GetType(); >> uncomment if a type needs to be looked at to cast
                            s = o.ToString();
                        }
                        newWidth = (int)g.MeasureString(s.Trim(), font).Width
                                    + vertScrollBarWidth;
                        if (width < newWidth)
                        {
                            width = newWidth;
                        }
                    }
                }
                senderComboBox.DropDownWidth = width;
            }
            catch (Exception objException)
            {
                //Catch objException
            }
        }
        // END TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only

		// BEGIN TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only
		protected void AdjustWidthComboBox_DropDown(object sender, System.EventArgs e)
		{
			try
			{
                ComboBox senderComboBox;
                if (sender is MIDComboBoxEnh)
                {
                    senderComboBox = ((MIDComboBoxEnh)sender).ComboBox;
                }
                else
                {
                    senderComboBox = senderComboBox = (ComboBox)sender;
                }
				int width = senderComboBox.DropDownWidth;
				Graphics g = senderComboBox.CreateGraphics();
				Font font = senderComboBox.Font;
				//checks if a scrollbar will be displayed.
				//If yes, then get its width to adjust the size of the drop down list.
				int vertScrollBarWidth =
					(senderComboBox.Items.Count > senderComboBox.MaxDropDownItems)
					? SystemInformation.VerticalScrollBarWidth : 0;

				//Loop through list items and check size of each items.
				//set the width of the drop down list to the width of the largest item.
				int newWidth;
                foreach (object o in senderComboBox.Items)
				{
					if (o != null)
					{
                        string s = string.Empty;
                        string displayMember = senderComboBox.DisplayMember;
                        if (o.GetType() == typeof(DataRowView))
                        {
                            DataRowView drv = (DataRowView)o;

                            if (drv.Row.Table.Columns.Contains(displayMember))
                            {
                                s = drv[displayMember].ToString();
                            }
                            else
                            {
                                s = o.ToString();
                            }
                        }
                        else if (o.GetType() == typeof(StoreGroupListViewProfile))
                        {
                            StoreGroupListViewProfile sgp = (StoreGroupListViewProfile)o;
                            s = sgp.GroupId;
                        }
                        else if (o.GetType() == typeof(StoreGroupLevelListViewProfile))
                        {
                            StoreGroupLevelListViewProfile sglp = (StoreGroupLevelListViewProfile)o;
                            s = sglp.Name;
                        }
                        else
                        {
                            //Type oType = o.GetType(); >> uncomment if a type needs to be looked at to cast
                            s = o.ToString();
                        }
						newWidth = (int)g.MeasureString(s.Trim(), font).Width
									+ vertScrollBarWidth;
						if (width < newWidth)
						{
							width = newWidth;
						}
					}
				}
				senderComboBox.DropDownWidth = width;
			}
			catch (Exception objException)
			{
				//Catch objException
			}
		}
        // END TT#1701 - AGallagher - Dynamic drop-down fields - Infrastructure change only

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private void cmiInUse_Click(object sender, EventArgs e)
        {
            try
            {
                if (_treeView.SelectedNode != null)
                {
                     IInUse();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

		// Begin TT#1212-MD - stodd - double-clicked on the assortment workspace to open an existing assortment receive an Argument Exception - 		
        /// <summary>
        /// Builds the full header list from the group allocation/assortment selected
        /// </summary>
        /// <param name="gaHeaderKeyList">A list of Group Allocation OR Assortment headers ONLY.</param>
        /// <param name="aTrans"></param>
        internal void AddSelectedHeadersToTrans(ArrayList gaHeaderKeyList, ApplicationSessionTransaction aTrans, ref ArrayList selectedAssortmentKeyList, ref ArrayList selectedHeaderKeyList)
        {
            //ArrayList gaHeaderKeyList = new ArrayList();
            // save off gaHeaders to a different list
            //gaHeaderKeyList.AddRange(_selectedHeaderKeyList);
            try
            {
                aTrans.NewAllocationMasterProfileList();

                aTrans.DequeueHeaders();
                selectedHeaderKeyList.Clear();
                selectedAssortmentKeyList.Clear(); // TT#488 - MD - Jellis - Group Allocation

                // find all "real" headers in each gaHeader
                bool createMaster = true;  // TT#488 - MD - Jellis - Group Allocation
                List<int> hdrList = new List<int>(); // TT#488 - MD - Jellis - Group ALlocation
                ArrayList selectedHeaderRIDs = new ArrayList(); // TT#488 - MD - Jellis - Group Allocation
                ArrayList selectedAssortmentRIDs = new ArrayList(); // TT#488 - MD - Jellis - Group Allocation
                string enqMessage = string.Empty; // TT#488 - MD - Jellis - Group Allocation
                foreach (int gaHeaderRid in gaHeaderKeyList)
                {
                    // Begin TT#1182-MD - stodd - unhandled exception
                    if (!selectedAssortmentRIDs.Contains(gaHeaderRid))
                    {
                        GetAllHeadersInAssortment(gaHeaderRid, ref selectedAssortmentKeyList, ref selectedHeaderKeyList);

                        // begin TT#488 - MD - Jellis - Group Allocation
                        //int[] selectedHeaderArray = new int[_selectedHeaderKeyList.Count];
                        //_selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                        // Begin TT#1542-MD - RMatelic Asst Workspace - select a basis date range and receive an Invalid Calendar Date Range Severe Error>>> unrelated - mulitple select error
                        //selectedHeaderRIDs.AddRange(selectedHeaderKeyList);

                        //selectedAssortmentRIDs.AddRange(selectedAssortmentKeyList);
                        // end TT#488 - MD - Jellis - Group Allocation
                        foreach (int rid in selectedHeaderKeyList)
                        {
                            if (!selectedHeaderRIDs.Contains(rid))
                            {
                                selectedHeaderRIDs.Add(rid);
                            }
                        }

                        foreach (int rid in selectedAssortmentKeyList)
                        {
                            if (!selectedAssortmentRIDs.Contains(rid))
                            {
                                selectedAssortmentRIDs.Add(rid);
                            }
                        }

                        // End TT#1542-MD

                        // BEGIN TT#66-MD - stodd - values not saving to Allocation profile
                        //string enqMessage = string.Empty; // TT#488 - MD - Jellis - Group Allocation
                        //List<int> hdrList = new List<int>(selectedHeaderArray);  // TT#488 - MD - Jellis - Group Allocation

                        //bool success = aTrans.EnqueueHeaders(hdrList, out enqMessage); // TT#488 - MD - Jellis - Group Allocation
                        // END TT#66-MD - stodd - 

                        // load the selected headers in the Application session transaction
                        // begin TT#488 - MD - Jellis - Group Allocation
                        //aTrans.LoadHeaders(selectedHeaderArray);
                        //aTrans.LoadAssortmentMemberHeaders(selectedHeaderArray);
                        // end TT#488 - MD - Jellis - Group Allocation
                    }
                    // End TT#1182-MD - stodd - unhandled exception
                }
                // begin TT#488 - MD - Jellis - Group Allocation
                int[] selectedHeaderArray = new int[selectedHeaderRIDs.Count];
                int[] selectedAssortmentArray = new int[selectedAssortmentRIDs.Count];
                selectedHeaderRIDs.CopyTo(selectedHeaderArray);
                selectedAssortmentRIDs.CopyTo(selectedAssortmentArray);
                hdrList.AddRange(selectedAssortmentArray);
                hdrList.AddRange(selectedHeaderArray);

                if (!aTrans.EnqueueHeaders(hdrList, out enqMessage))
                {
                    // HANDLE Locked selection!  set createMaster to FALSE if they do not want to proceed in READ MODE
                }
                if (createMaster)
                {
                    aTrans.CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray);
                    aTrans.CreateAllocationProfileListFromAssortmentMaster(false, false);  // TT#888 - MD - Jellis - Assortment/Group members not populated // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
                    //aTrans.CreateAllocationProfileListFromAssortmentMaster(true, false);  // TT#888 - MD - Jellis - Assortment/Group members not populated  // TT#1042 - MD - Jellis - Qty Allocated Cannot Be Negative part 2
                }
                // end TT#488 - MD - Jellis - Group Allocation
            }
            catch
            {
                // begin TT#946 - MD - Jellis - Group Allocation Not Working
                if (aTrans != null)
                {
                    if (aTrans.AllocationCriteria != null)  // TT#955 - MD - Jellis - Dup Color Error on Drag Drop Header into Group
                    {
                        aTrans.DataState = eDataState.ReadOnly;
                    }
                    aTrans.DequeueHeaders();
                }
                // end TT#946 - MD - Jellis - Group Allocation Not Working
                throw;
            }
        }

        internal void GetAllHeadersInAssortment(int aAsrtRID, ref ArrayList selectedAssortmentKeyList, ref ArrayList selectedHeaderKeyList)
        {
            try
            {
                ArrayList al = _SAB.HeaderServerSession.GetHeadersInAssortment(aAsrtRID);
                for (int i = 0; i < al.Count; i++)
                {
                    int hdrRID = (int)al[i];
                    // begin TT#488 - MD - Jellis - Group Allocation
                    if (hdrRID == aAsrtRID)
                    {
                        if (!selectedAssortmentKeyList.Contains(hdrRID))
                        {
                            selectedAssortmentKeyList.Add(hdrRID);
                        }
                    }
                    else
                    {
                        // end TT#488 - MD - Jellis - Group Allocation
                        if (!selectedHeaderKeyList.Contains(hdrRID))
                        {
                            selectedHeaderKeyList.Add(hdrRID);
                        }
                    }  // TT#488 - MD - Jellis - Group Allocation
                }
            }
            catch
            {
                throw;
            }
        }
		// End TT#1212-MD - stodd - double-clicked on the assortment workspace to open an existing assortment receive an Argument Exception - 
	}
}
