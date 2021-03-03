using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices; // TT#609-MD - RMatelic - OTS Forecast Chain Ladder View change explorers for Windows arrow expanders

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
    public partial class MIDTreeView : System.Windows.Forms.TreeView
    {
        //=======
        // EVENTS
        //=======

        public delegate void MIDTreeViewDoubleClickHandler(object source, MIDTreeNode node);
        public event MIDTreeViewDoubleClickHandler OnMIDDoubleClick;

        public delegate void MIDTreeViewNodeSelectHandler(object source, MIDTreeNode node);
        public event MIDTreeViewNodeSelectHandler OnMIDNodeSelect;

        //=======
        // FIELDS
        //=======
        const int                       _progressMinCount = 5;

        // Begin Track #6295 - JSmith - Try to attach sku to added header 
        private ArrayList               _alPaintedNodes;
        // End Track #6295
        private ArrayList               _alSelectedNodes;
        protected Hashtable				_folderNodeHash;
        protected Hashtable				_itemNodeHash;
        private SessionAddressBlock		_SAB;
        private Form					_MDIParentForm;
        private frmProgress             _progressForm;

        private DragDropEffects _dragEffect = DragDropEffects.None;
        private DragDropEffects _currentEffect = DragDropEffects.Move;
        private eDragStates _currentState = eDragStates.Idle;

        private System.Timers.Timer _timer = null;

        private TreeNode _tnLastNode, _tnFirstNode;
        private TreeNode _tnAutoExpandNode = null;
        private TreeNode _tnCurrentAutoExpandNode = null;
        private TreeNode _tnMouseDownOnNode = null;
        private TreeNode _tnArrowNode = null;
		private MIDTreeNode _lastValidDragOverNode = null;
		protected MIDTreeNode _favoritesNode = null;

        private bool _bAllowMultiSelect = false;
        private bool _bAllowAutoExpand = false;
        private bool _bTimerRunning = false;
        private bool _bTimerActivated = false;
        private bool _bExpandingNode = false;
        private bool _bUpArrowPressed = false;
        private bool _bDownArrowPressed = false;
        private bool _bRightMousePressed = false;
        private bool _bLeftMousePressed = false;
        private bool _bShiftKeyPressed = false;
        private bool _bControlKeyPressed = false;
        private bool _bDragOperation = false;
        private bool _bSimulateMultiSelect = false;
        private bool _bDraggingThru = false;
        private bool _bPerformingCopy = false;
        private bool _bPerformingCut = false;
        private bool _bDebugActivated = false;

        private int _iTimerInterval = 2000;
        private int _spacing = 2;
        private MIDTreeNode _dragOverNode = null;
        private int _dragOverIndex = 0;
        private int _progressStatusCount;

        private Point _mouseDownPoint;
		//Begin Track #6201 - JScott - Store Count removed from attr sets
		//protected string _originalTextLabel;
		//End Track #6201 - JScott - Store Count removed from attr sets
		protected string _progressStatusMessage;

        private FolderDataLayer _dlFolder;
		private SecurityAdmin _secAdmin;

        // Begin TT#42 - JSmith - Explorers and My Favorites cannont create Folders and My Favorites Workflow/Methods not behaving as expected.
        private MIDTreeNodeSecurityGroup _favoritesSecGrp = null;
        // End TT#42
        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        private MIDTreeNodeSecurityGroup _favoritesFolderSecGrp = null;
        // End TT#373

        //=============
        // CONSTRUCTORS
        //=============

        public MIDTreeView()
        {
            InitializeComponent();

            // Begin Track #6295 - JSmith - Try to attach sku to added header 
            _alPaintedNodes = new ArrayList();
            // End Track #6295
			_alSelectedNodes = new ArrayList();
			_folderNodeHash = new Hashtable();
			_itemNodeHash = new Hashtable();
			_dlFolder = new FolderDataLayer();
			_secAdmin = new SecurityAdmin();

            // Begin TT#42 - JSmith - Explorers and My Favorites cannont create Folders and My Favorites Workflow/Methods not behaving as expected.
            _favoritesSecGrp = new MIDTreeNodeSecurityGroup();
            _favoritesSecGrp.FunctionSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
            _favoritesSecGrp.FunctionSecurityProfile.SetFullControl();
            _favoritesSecGrp.FolderSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
            _favoritesSecGrp.FolderSecurityProfile.SetFullControl();
            // End TT#42

            // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
            _favoritesFolderSecGrp = new MIDTreeNodeSecurityGroup();
            _favoritesFolderSecGrp.FunctionSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
            _favoritesFolderSecGrp.FunctionSecurityProfile.AddAllSecurityActions();
            _favoritesFolderSecGrp.FunctionSecurityProfile.SetFullControl();
            _favoritesFolderSecGrp.FunctionSecurityProfile.SetDenyDelete();
            _favoritesFolderSecGrp.FolderSecurityProfile = new FunctionSecurityProfile(Include.NoRID);
            _favoritesFolderSecGrp.FolderSecurityProfile.AddAllSecurityActions();
            _favoritesFolderSecGrp.FolderSecurityProfile.SetFullControl();
            _favoritesFolderSecGrp.FolderSecurityProfile.SetDenyDelete();
            // End TT#373
            
		}
        // Begin TT#609-MD - RMatelic - OTS Forecast Chain Ladder View change explorers for Windows arrow expanders
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private extern static int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        protected override void CreateHandle()
        {
            base.CreateHandle();

            SetWindowTheme(this.Handle, "explorer", null);
        }
        // End TT#609-MD 

        //===========
        // PROPERTIES
        //===========
        /// <summary>
        /// Returns the SessionAddressBlock.
        /// </summary>

        public SessionAddressBlock SAB
        {
            get { return _SAB; }
        }

        /// <summary>
        /// Returns the last node selected if multiple are selected
        /// </summary>

        new public MIDTreeNode SelectedNode
        {
            get
            {
                if (_alSelectedNodes.Count > 0)
                {
                    return (MIDTreeNode)_alSelectedNodes[_alSelectedNodes.Count - 1];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (value == null)
                {
                    ClearSelectedNodes();
                }

                base.SelectedNode = value;
            }
        }

		/// <summary>
		/// Returns the count of selected nodes
		/// </summary>

		public int SelectedNodeCount
		{
			get
			{
				return _alSelectedNodes.Count;
			}
		}

        /// <summary>
        /// A flag identifying if there are not any selected nodes
        /// </summary>

        public bool NoNodesSelected
        {
            get
            {
                return _alSelectedNodes.Count == 0;
            }
        }

        /// <summary>
        /// A flag identifying if there is only one selected node
        /// </summary>

        public bool OnlyOneNodesSelected
        {
            get
            {
                return _alSelectedNodes.Count == 1;
            }
		}

		/// <summary>
		/// A flag identifying if there are more than one selected nodes
        /// </summary>

        public bool MultipleNodesSelected
        {
            get
            {
                return _alSelectedNodes.Count > 1;
            }
        }

        /// <summary>
        /// The TreeNode on which the mouse was clicked
        /// </summary>

        public TreeNode MouseDownOnNode
        {
            get
            {
                return _tnMouseDownOnNode;
            }
        }

        /// <summary>
        /// A flag identifying if multiple nodes can be selected
        /// </summary>

        public bool AllowMultiSelect
        {
            get
            {
                return _bAllowMultiSelect;
            }
            set
            {
                _bAllowMultiSelect = value;
            }
        }

        /// <summary>
        /// A flag identifying that multiple nodes will be selected
        /// </summary>

        public bool SimulateMultiSelect
        {
            get
            {
                return _bSimulateMultiSelect;
            }
            set
            {
                _bSimulateMultiSelect = value;
            }
        }

        /// <summary>
        /// A flag identifying if the auto expand feature is to be enabled
        /// </summary>

        public bool AllowAutoExpand
        {
            get
            {
                return _bAllowAutoExpand;
            }
            set
            {
                _bAllowAutoExpand = value;
            }
        }

        public void ExpandNode(TreeNode node)
        {
            MIDTreeView_BeforeExpand(null, new TreeViewCancelEventArgs(node, false, TreeViewAction.Expand));
        }

        /// <summary>
        /// A flag identifying if a copy action has been requested
        /// </summary>

        public bool PerformingCopy
        {
            get
            {
                return _bPerformingCopy;
            }
            set
            {
                _bPerformingCopy = value;
            }
        }

        /// <summary>
        /// A flag identifying if a cut action has been requested
        /// </summary>

        public bool PerformingCut
        {
            get
            {
                return _bPerformingCut;
            }
            set
            {
                _bPerformingCut = value;
            }
        }

		/// <summary>
		/// A flag identifying if debug has been activated
		/// </summary>

		public bool DebugActivated
		{
			get
			{
				return _bDebugActivated;
			}
		}

		/// <summary>
		/// The current DragDropEffects
        /// </summary>

        public System.Windows.Forms.DragDropEffects DragEffect
        {
            get
            {
                return _dragEffect;
            }
            set
            {
                _dragEffect = value;
            }
        }

        /// <summary>
        /// The current DragDropEffects
        /// </summary>

        public System.Windows.Forms.DragDropEffects CurrentEffect
        {
            get
            {
                return _currentEffect;
            }
            set
            {
                _currentEffect = value;
            }
        }

        /// <summary>
        /// The current state of the drag operation
        /// </summary>

        public eDragStates CurrentState
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
            }
        }

        /// <summary>
        /// A flag identifying if the timer is running
        /// </summary>

        public bool TimerRunning
        {
            get
            {
                return _bTimerRunning;
            }
        }

        /// <summary>
        /// A flag identifying if the right mouse button is down
        /// </summary>

        public bool RightMouseDown
        {
            get
            {
                return _bRightMousePressed;
            }
        }

        /// <summary>
        /// A flag identifying if the left mouse button is down
        /// </summary>

        public bool LeftMousePressed
        {
            get
            {
                return _bLeftMousePressed;
            }
        }

        /// <summary>
        /// The interval used by the auto expand feature to determine when a node is to be expanded
        /// </summary>

        public int TimerInterval
        {
            get
            {
                return _iTimerInterval;
            }
            set
            {
                _iTimerInterval = value;
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

		public SecurityAdmin DlSecurity
		{
			get
			{
				return _secAdmin;
			}
		}

		public MIDTreeNode FavoritesNode
        {
            get
            {
                return _favoritesNode;
            }
            set
            {
                _favoritesNode = value;
            }
        }

        public Hashtable FolderNodeHash
        {
            get
            {
                return _folderNodeHash;
            }
        }


        public Hashtable ItemNodeHash
        {
            get
            {
                return _itemNodeHash;
            }
        }

        public Form MDIParentForm
        {
            get
            {
                return _MDIParentForm;
            }
        }

        public bool ProgressFormIsDisplayed
        {
            get
            {
                return _progressForm != null;
            }
        }

        // Begin TT#42 - JSmith - Explorers and My Favorites cannont create Folders and My Favorites Workflow/Methods not behaving as expected.
        public MIDTreeNodeSecurityGroup FavoritesSecGrp
        {
            get
            {
                return _favoritesSecGrp;
            }
        }
        // End TT#42

        // Begin TT#373 - JSmith - User can delete nodes with menu or delete key that they cannot delete with right mouse
        public MIDTreeNodeSecurityGroup FavoritesFolderSecGrp
        {
            get
            {
                return _favoritesFolderSecGrp;
            }
        }
        // End TT#373

        // Begin TT#4047 - JSmith - User freezes dragging a merchandise hierarchy to a method
        public bool ItemBeingDragged
        {
            get
            {
                return _bDragOperation || _bDraggingThru;
            }
        }
        // End TT#4047 - JSmith - User freezes dragging a merchandise hierarchy to a method

        //========
        // METHODS
        //========

		//----------------------------------------------
		//OVERRIDES TO VIRTUAL METHODS IN THE BASE CLASS
		//----------------------------------------------

		override protected void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
			try
			{
                // Begin Track #6202 - JSmith - select not allowed
                //if (((MIDTreeNode)e.Node).FunctionSecurityProfile != null &&
                //    ((MIDTreeNode)e.Node).FunctionSecurityProfile.AccessDenied)
                //{
                //    e.Cancel = true;
                //    return;
                //}
                // Begin Track #6202 - JSmith - select not allowed

				bool bControl = false;
				bool bShift = false;

				if (_bAllowMultiSelect)
				{
					if (_bSimulateMultiSelect)
					{
						bControl = true;
					}
					else
					{
						bControl = (ModifierKeys == Keys.Control);
					}
					bShift = (ModifierKeys == Keys.Shift);
				}

				if (e.Node != null && !((MIDTreeNode)e.Node).isSelectAllowed((bControl || bShift), _alSelectedNodes))
				{
                    Debug.WriteLine("select not allowed");
					e.Cancel = true;
					return;
				}

				base.OnBeforeSelect(e);

				// selecting twice the node while pressing CTRL ?
				if (!_bDownArrowPressed && !_bUpArrowPressed)
				{
					if (bControl && _alSelectedNodes.Contains(e.Node))
					{
						// unselect it (let framework know we don't want selection this time)
						e.Cancel = true;

						// update nodes
						RemovePaintFromSelectedNodes();
						_alSelectedNodes.Remove(e.Node);
						PaintSelectedNodes();
						return;
					}

					_tnLastNode = (MIDTreeNode)e.Node;
					if (!bShift) _tnFirstNode = (MIDTreeNode)e.Node; // store begin of shift sequence
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		override protected void OnAfterSelect(TreeViewEventArgs e)
		{
			try
			{
				base.OnAfterSelect(e);

				bool bControl = false;
				bool bShift = false;

				if (_bAllowMultiSelect)
				{
					if (_bSimulateMultiSelect)
					{
						bControl = true;
					}
					else
					{
						bControl = (ModifierKeys == Keys.Control);
					}
					bShift = (ModifierKeys == Keys.Shift);
				}

				if (bControl)
				{
					if (!_alSelectedNodes.Contains(e.Node)) // new node ?
					{
						_alSelectedNodes.Add(e.Node);
                        if (OnMIDNodeSelect != null)
                        {
                            OnMIDNodeSelect(this, (MIDTreeNode)e.Node);
                        }
					}
					else if (!_bDownArrowPressed && !_bUpArrowPressed) // not new, remove it from the collection
					{
						RemovePaintFromSelectedNodes();
						_alSelectedNodes.Remove(e.Node);
					}
					PaintSelectedNodes();
				}
				else
				{
					// SHIFT is pressed
                    // Begin TT#568-MD - JSmith - Treeview Error when opening application
                    //if (bShift)
                    if (bShift &&
                        _tnFirstNode != null)
                    // End TT#568-MD - JSmith - Treeview Error when opening application
					{
						Queue myQueue = new Queue();

						TreeNode uppernode = _tnFirstNode;
						TreeNode bottomnode = e.Node;
						if (uppernode.Parent != bottomnode.Parent)
						{
							RemovePaintFromNodes();
							_alSelectedNodes.Clear();
							MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_ShiftSelectMustHaveSameParent));
							return;
						}
						// case 1 : begin and end nodes are parent
						bool bParent = isParent(_tnFirstNode, e.Node); // is _tnFirstNode parent (direct or not) of e.Node
						if (!bParent)
						{
							bParent = isParent(bottomnode, uppernode);
							if (bParent) // swap nodes
							{
								TreeNode t = uppernode;
								uppernode = bottomnode;
								bottomnode = t;
							}
						}
						if (bParent)
						{
							TreeNode n = bottomnode;
							while (n != uppernode.Parent)
							{
								if (!_alSelectedNodes.Contains(n)) // new node ?
									myQueue.Enqueue(n);

								n = n.Parent;
							}
						}
						// case 2 : neither the begin nor the end node are descendant one another
						else
						{
							if ((uppernode.Parent == null && bottomnode.Parent == null) || (uppernode.Parent != null && uppernode.Parent.Nodes.Contains(bottomnode))) // are they siblings ?
							{
								int nIndexUpper = uppernode.Index;
								int nIndexBottom = bottomnode.Index;
								if (nIndexBottom < nIndexUpper) // reversed?
								{
									TreeNode t = uppernode;
									uppernode = bottomnode;
									bottomnode = t;
									nIndexUpper = uppernode.Index;
									nIndexBottom = bottomnode.Index;
								}

								TreeNode n = uppernode;
								while (nIndexUpper <= nIndexBottom)
								{
									if (!_alSelectedNodes.Contains(n)) // new node ?
										myQueue.Enqueue(n);

									n = n.NextNode;

									nIndexUpper++;
								} // end while

							}
							else
							{
								if (!_alSelectedNodes.Contains(uppernode)) myQueue.Enqueue(uppernode);
								if (!_alSelectedNodes.Contains(bottomnode)) myQueue.Enqueue(bottomnode);
							}
						}

						_alSelectedNodes.AddRange(myQueue);

						PaintSelectedNodes();
						if (!_bDownArrowPressed && !_bUpArrowPressed)
						{
							_tnFirstNode = (MIDTreeNode)e.Node; // let us chain several SHIFTs if we like it
						}
					} // end if m_bShift
					else
					{
                        if (!_alSelectedNodes.Contains(e.Node))
                        {
                            ClearSelectedNodes();
                            _alSelectedNodes.Add(e.Node);
                            if (OnMIDNodeSelect != null)
                            {
                                OnMIDNodeSelect(this, (MIDTreeNode)e.Node);
                            }

                            PaintSelectedNodes();
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

		//----------------------------------------------------
		//VIRTUAL METHODS TO BE OVERRIDDEN BY INHERITING CLASS
		//----------------------------------------------------

		/// <summary>
		/// Virtual method that initializes the MIDTreeView
		/// </summary>
		/// <param name="aSAB">
		/// The SessionAddressBlock
		/// </param>
		/// <param name="aAllowMultiSelect">
		/// A boolean indicating if multi-select is allowed
		/// </param>
		/// <param name="aMDIParentForm">
		/// The MDI parent form
		/// </param>

		virtual public void InitializeTreeView(SessionAddressBlock aSAB, bool aAllowMultiSelect, Form aMDIParentForm)
        {
            try
            {
                _SAB = aSAB;
				_bAllowMultiSelect = aAllowMultiSelect;
				_MDIParentForm = aMDIParentForm;

				this.DragOver += new System.Windows.Forms.DragEventHandler(this.MIDTreeView_DragOver);
                this.DoubleClick += new System.EventHandler(this.MIDTreeView_DoubleClick);
                this.DragLeave += new System.EventHandler(this.MIDTreeView_DragLeave);
                this.DragDrop += new DragEventHandler(MIDTreeView_DragDrop);
                this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MIDTreeView_MouseUp);
                this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MIDTreeView_MouseMove);
                this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MIDTreeView_DragEnter);
                this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MIDTreeView_KeyUp);
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MIDTreeView_KeyDown);
                this.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.MIDTreeView_ItemDrag);
                this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MIDTreeView_MouseDown);
                this.BeforeExpand += new TreeViewCancelEventHandler(MIDTreeView_BeforeExpand);
                this.AfterExpand += new TreeViewEventHandler(MIDTreeView_AfterExpand);
                this.AfterCollapse += new TreeViewEventHandler(MIDTreeView_AfterCollapse);
                this.BeforeLabelEdit += new NodeLabelEditEventHandler(MIDTreeView_BeforeLabelEdit);
                this.AfterLabelEdit += new NodeLabelEditEventHandler(MIDTreeView_AfterLabelEdit);

                _dragOverIndex = MIDGraphics.ImageIndex(MIDGraphics.RightSelectArrow);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		/// <summary>
		/// Virtual method that loads the nodes for the MIDTreeView
		/// </summary>

		virtual public void LoadNodes()
		{
			throw new Exception("Method LoadNodes() must be overridden by inheriting class");
		}

        /// <summary>
        /// Virtual method that loads the nodes for the MIDTreeView
        /// </summary>

        virtual public void ReloadCache(int userRID, int nodeRID)
        {
            throw new Exception("Method ReloadCache() must be overridden by inheriting class");
        }

		/// <summary>
		/// Virtual method executed after the New Item menu item has been clicked.
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode that was clicked on
		/// </param>
		//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

		//virtual protected void CreateNewItem(MIDTreeNode aParentNode)
		/// <returns>
		/// The new node that was created.  If node is returned, it will be placed in edit mode.
		/// If node is not available or edit mode is not desired, return null.
		/// </returns>
	
		virtual protected MIDTreeNode CreateNewItem(MIDTreeNode aParentNode)
		//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
		{
			throw new Exception("Method CreateNewItem() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method executed after the New Folder menu item has been clicked.
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode that was clicked on
		/// </param>
		/// <param name="aUserId">
		/// The user Id to create the Folder under
		/// </param>

		virtual protected MIDTreeNode CreateNewFolder(MIDTreeNode aNode, int aUserId)
		{
			throw new Exception("Method CreateNewFolder() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method that is called after a label has been updated
		/// </summary>

		virtual protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
		{
			throw new Exception("Method AfterLabelUpdate() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method used to create a shortcut in the Favorites folder
		/// </summary>
		/// <param name="aFromNode">
		/// The MIDTreeNode being copied
		/// </param>
		/// <param name="aToNode">
		/// The MIDTreeNode where new node is being copied to
		/// </param>

		virtual public void CreateShortcut(MIDTreeNode aFromNode, MIDTreeNode aToNode)
		{
			throw new Exception("Method CreateShortcut() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual Method that builds a new MIDTreeNode
		/// </summary>
		/// <param name="aText">
		/// The Text to be displayed in the MIDTreeNode
		/// </param>
		/// <param name="aKey">
		/// The key of the MIDTreeNode
		/// </param>
		/// <returns>
		/// The new MIDTreeNode
		/// </returns>

		virtual protected MIDTreeNode BuildNode(string aText, int aKey)
		{
			throw new Exception("Method BuildNode() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method used to move a MIDTreeNode from one place to another
		/// </summary>
		/// <param name="aFromNode">
		/// The MIDTreeNode being moved
		/// </param>
		/// <param name="aToNode">
		/// The MIDTreeNode where new node is being move to
		/// </param>

		virtual protected MIDTreeNode MoveNode(MIDTreeNode aFromNode, MIDTreeNode aToNode)
		{
			throw new Exception("Method MoveNode() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method used to copy a MIDTreeNode from one place to another
		/// </summary>
		/// <param name="aFromNode">
		/// The MIDTreeNode being copied
		/// </param>
		/// <param name="aToNode">
		/// The MIDTreeNode where new node is being copied to
		/// </param>
		/// <param name="aFindUniqueName">
		/// A boolean indicating if the procedure should insure create a unique name in case of a duplicate
		/// </param>

		virtual protected MIDTreeNode CopyNode(MIDTreeNode aFromNode, MIDTreeNode aToNode, bool aFindUniqueName)
		{
			throw new Exception("Method CopyNode() must be overridden by inheriting class");
		}

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Virtual method used to determine InUse of a MIDTreeNode.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being evaluated for InUse.
        /// </param>

        virtual protected void InUseNode(MIDTreeNode aNode)
        {
            throw new Exception("Method InUseNode() must be overridden by inheriting class");
        }

        virtual protected bool AllowInUseDelete(ArrayList aDeleteList)
        {
            throw new Exception("Method AllowInUseDelete() must be overridden by inheriting class");
        }
        //END TT#110-MD-VStuart - In Use Tool

        // Begin TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder
        public ArrayList GetObjectNodes(ArrayList aNodeList)
        {
            ArrayList alObjectNodeList = new ArrayList();
            foreach (MIDTreeNode node in aNodeList)
            {
                if (node.isFolder ||
                    node.isSubFolder)
                {
                    AddDescendantNodes(node, alObjectNodeList);
                }
                else
                {
                    alObjectNodeList.Add(node);
                }
            }

            return alObjectNodeList;
        }

        private void AddDescendantNodes(MIDTreeNode aNode, ArrayList alObjectNodeList)
        {
            // call expand to load children
            if (aNode.HasChildren && aNode.DisplayChildren && !aNode.ChildrenLoaded)
            {
                MIDTreeView_BeforeExpand(this, new TreeViewCancelEventArgs(aNode,false,TreeViewAction.Expand));
            }
            foreach (MIDTreeNode node in aNode.Nodes)
            {
                if (node.isFolder ||
                    node.isSubFolder)
                {
                    AddDescendantNodes(node, alObjectNodeList);
                }
                else if (node.isObject)
                {
                    alObjectNodeList.Add(node);
                }
            }
        }
        // End TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder

        public ArrayList GetFolderNodes(ArrayList aNodeList)
        {
            ArrayList alObjectNodeList = new ArrayList();
            foreach (MIDTreeNode node in aNodeList)
            {
                if (node.isFolder ||
                    node.isSubFolder)
                {
                    alObjectNodeList.Add(node);
                    AddDescendantFolderNodes(node, alObjectNodeList);
                }

            }

            return alObjectNodeList;
        }

        private void AddDescendantFolderNodes(MIDTreeNode aNode, ArrayList alObjectNodeList)
        {
            // call expand to load children
            if (aNode.HasChildren && aNode.DisplayChildren && !aNode.ChildrenLoaded)
            {
                MIDTreeView_BeforeExpand(this, new TreeViewCancelEventArgs(aNode, false, TreeViewAction.Expand));
            }
            foreach (MIDTreeNode node in aNode.Nodes)
            {
                if (node.isFolder ||
                    node.isSubFolder)
                {
                    alObjectNodeList.Add(node);
                    AddDescendantFolderNodes(node, alObjectNodeList);
                }
            }
        }

        /// <summary>
        /// Virtual method used to delete a MIDTreeNode
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being deleted
        /// </param>
        /// <param name="aDeleteCancelled">
        /// Flag identifying if the delete action was cancelled.
        /// </param>

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        //virtual protected void DeleteNode(MIDTreeNode aNode)
        virtual protected void DeleteNode(MIDTreeNode aNode, out bool aDeleteCancelled)
		// End TT#3630 - JSmith - Delete My Hierarchy
        {
            throw new Exception("Method DeleteNode() must be overridden by inheriting class");
        }

        /// <summary>
		/// Virtual method used to edit a MIDTreeNode
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode being edited
		/// </param>

		virtual protected void EditNode(MIDTreeNode aNode)
		{
			throw new Exception("Method EditNode() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method used to refresh a favorites branch
		/// </summary>
		/// <param name="aStartNode">
		/// The MIDTreeNode to start searching for changed node
		/// </param>
		/// <param name="aChangedNode">
		/// The MIDTreeNode to that was changed
		/// </param>

		virtual public void RefreshShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
		{
			throw new Exception("Method RefreshShortcuts() must be overridden by inheriting class");
		}

		/// <summary>
		/// Virtual method that indicates if the data in the clipboard is valid
		/// </summary>
		/// <param name="aClipboardDataType">
		/// The eProfileType of the clipboard data
		/// </param>
		/// A boolean indicating if the data in the clipboard is valid
		/// <returns></returns>

		virtual public bool isAllowedDataType(eProfileType aClipboardDataType)
		{
			throw new Exception("Method isAllowedDataType() must be overridden by inheriting class");
		}

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        /// <summary>
        /// Virtual method that indicates if the data in the clipboard can be pasted
        /// </summary>
        /// <param name="aDropAction">
        /// The eProfileType of the clipboard data
        /// </param>
        /// <param name="aDropNode">
        /// The MIDTreeNode where the clipboard values are to be pasted
        /// </param>
        /// A boolean indicating if the data in the clipboard can be pasted
        /// <returns></returns>
        virtual public bool isPasteFromClipboardAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode)
        {
            throw new Exception("Method isPasteFromClipboardAllowed() must be overridden by inheriting class");
        }
        // End TT#564

		/// <summary>
		/// Virtual method used to determine if a drop is allowed for a ClipboardListBase
		/// </summary>
		/// <param name="aDropAction">
		/// The DragDropEffects of the action being performed.
		/// </param>
		/// <param name="aDropNode">
		/// The node being dropped on.
		/// </param>
		/// <param name="aClipboardList">
		/// The ClipboardListBase of nodes being dropped.
		/// </param>
		/// <returns>
		/// A boolean indicating if a drop is allowed for a ClipboardListBase
		/// </returns>

		virtual public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aClipboardList)
		{
			throw new Exception("Method isDropAllowed() must be overridden by inheriting class");
		}

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Virtual method used to determine the number of nodes to be evaluated for In Use.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being evaluated.
        /// </param>

        virtual protected int GetInUseNodeCount(MIDTreeNode aNode)
        {
            return 1;
        }
        //END TT#110-MD-VStuart - In Use Tool

        /// <summary>
        /// Virtual method used to determine the number if items to be deleted
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being deleted
        /// </param>

        virtual protected int GetDeleteNodeCount(MIDTreeNode aNode)
        {
            return 1;
        }

        /// <summary>
		/// Virtual method used to determine the number of descendants for the node
		/// </summary>
		/// <param name="aNode">
		/// The MIDTreeNode 
		/// </param>

		virtual protected int GetDescendantCount(MIDTreeNode aNode)
		{
			return 1;
		}

		virtual protected string FindNewFolderName(string aFolderName, int aUserRID, int aParentRID, eProfileType aItemType)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 1;
				newName = aFolderName;
				key = _dlFolder.Folder_GetKey(aUserRID, newName, aParentRID, aItemType);

				while (key != -1)
				{
					index++;
					newName = aFolderName + " (" + index + ")";
					key = _dlFolder.Folder_GetKey(aUserRID, newName, aParentRID, aItemType);
				}

				return newName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Virtual method that is executed when an object other than a TreeNodeClipboardList is dropped on a MIDTreeNode.
		/// </summary>
		/// <param name="sender">
		/// The calling program.
		/// </param>
		/// <param name="e">
		/// The DragEventArgs originaly received by the DragDrop event.
		/// </param>

		virtual protected void CustomDragDrop(object sender, DragEventArgs e)
		{
		}

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        /// <summary>
        /// Virtual method that is executed when an object is pasted from the clipboard.
        /// </summary>

        virtual protected bool CustomPasteFromClipboard(eCutCopyOperation aCutCopyOperation)
        {
            return false;
        }
        // End TT#564

		/// <summary>
		/// Virtual method used to determine if a shortcut should be make for this situation
		/// </summary>
		/// <param name="aFromNode">
		/// The MIDTreeNode being copied
		/// </param>
		/// <param name="aToNode">
		/// The MIDTreeNode where new node is being copied to
		/// </param>

        // Begin TT#394 - JSmith - Cut and Paste not performing consistently
        //virtual protected bool MakeShortcutNode(MIDTreeNode aNode, ArrayList aCutCopyNodes)
        virtual protected bool MakeShortcutNode(MIDTreeNode aNode, ArrayList aCutCopyNodes, eCutCopyOperation aCutCopyOperation)
        // End TT#394
		{
			return false;
		}

		/// <summary>
		/// Takes MIDTreeNodes and creates a ClipboardProfile
		/// </summary>
		/// <param name="aNodes"></param>
		/// <param name="aAction">
		/// The eDropAction associatied with the ClipboardProfile data
		/// </param>
		/// <returns></returns>

		virtual public ClipboardListBase BuildClipboardList(ArrayList aNodes, DragDropEffects aAction)
		{
			TreeNodeClipboardList clipboardList;

			try
			{
				clipboardList = new TreeNodeClipboardList(((MIDTreeNode)aNodes[0]).Profile.ProfileType);

				foreach (MIDTreeNode node in aNodes)
				{
					clipboardList.ClipboardItems.Add(BuildClipboardProfile(node, aAction));
				}

				return clipboardList;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//--------------
		//PUBLIC METHODS
		//--------------

		/// <summary>
		/// Gets a copy of the list of all selected nodes
		/// </summary>

		public ArrayList GetSelectedNodes()
		{
			try
			{
				return (ArrayList)_alSelectedNodes.Clone();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public MIDTreeNode GetSelectedNode(int aIndex)
		{
			try
			{
				return (MIDTreeNode)_alSelectedNodes[aIndex];
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void ExpandFavoritesNode()
        {
            try
            {
                if (_favoritesNode != null)
                {
                    _favoritesNode.Expand();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RemoveSelectedNode(MIDTreeNode aTreeNode)
        {
            try
            {
                if (_alSelectedNodes.Contains(aTreeNode))
                {
                    _alSelectedNodes.Remove(aTreeNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ClearSelectedNodes()
        {
            try
            {
                if (_alSelectedNodes == null)
                {
                    _alSelectedNodes = new ArrayList();
                }
                else
                {
                    RemovePaintFromSelectedNodes();
                    _alSelectedNodes.Clear();
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void AddSelectedNode(MIDTreeNode aTreeNode)
        {
            try
            {
                if (!_alSelectedNodes.Contains(aTreeNode))
                {
                    _alSelectedNodes.Add(aTreeNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename
		//public void CreateNewTreeItem(MIDTreeNode aParentNode)
		//{
		//    try
		//    {
		//        CreateNewItem(aParentNode);
		//        RefreshShortcuts(FavoritesNode, aParentNode);
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		public MIDTreeNode CreateNewTreeItem(MIDTreeNode aParentNode)
		{
			MIDTreeNode newNode;

			try
			{
				newNode = CreateNewItem(aParentNode);
				RefreshShortcuts(FavoritesNode, aParentNode);
				return newNode;
			}
			catch
			{
				throw;
			}
		}
		//End Track #6257 - JScott - Create New Attribute requires user to right-click rename

		public MIDTreeNode CreateNewTreeFolder(MIDTreeNode aNode, int aUserId)
		{
			MIDTreeNode newNode;

			try
			{
				newNode = CreateNewFolder(aNode, aUserId);
				RefreshShortcuts(FavoritesNode, aNode);

				return newNode;
			}
			catch
			{
				throw;
			}
		}

		public void DeleteTreeNode()
		{
			string msg;
			DialogResult retCode;
			MIDTreeNode nextNode;
			MIDTreeNode parentNode;
			ArrayList tempList;
			ArrayList deleteList;
			int totalCount;
			int count;
			//bool performRemove = false;
            bool deleteCancelled = false;   // TT#3630 - JSmith - Delete My Hierarchy

			try
			{
				//Begin TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
				//if (SelectedNode != null)
				//{
				//    if (SelectedNode.Nodes.Count > 0)
				//    {
				//        if (SelectedNode.isShortcut)
				//        {
				//            msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false);
				//            //performRemove = true;
				//        }
				//        else
				//        {
				//            msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerParent);
				//        }
				//    }
				//    else
				//    {
				//        if (SelectedNode.isShortcut)
				//        {
				//            msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false);
				//            //performRemove = true;
				//        }
				//        else
				//        {
				//            msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerChild);
				//        }
				//    }

				//    msg = msg.Replace("{0}", SelectedNode.Text);
				//    //if (performRemove)
				//    //{
				//    //    retCode = MessageBox.Show(msg, "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				//    //}
				//    //else
				//    //{
				//    retCode = MessageBox.Show(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				//    //}

				//    if (retCode == DialogResult.Yes)
				//    {
				//        try
				//        {
				//            Cursor.Current = Cursors.WaitCursor;
				//            BeginUpdate();

				//            if (SelectedNode.NextNode != null)
				//            {
				//                nextNode = (MIDTreeNode)SelectedNode.NextNode;
				//            }
				//            else if (SelectedNode.PrevNode != null)
				//            {
				//                nextNode = (MIDTreeNode)SelectedNode.PrevNode;
				//            }
				//            else
				//            {
				//                nextNode = (MIDTreeNode)SelectedNode.Parent;
				//            }

				//            tempList = (ArrayList)_alSelectedNodes.Clone();

				//            deleteList = new ArrayList();
				//            totalCount = 0;
				//            foreach (MIDTreeNode node in tempList)
				//            {
				//                if (node.FunctionSecurityProfile.AllowDelete)
				//                {
				//                    count = GetDeleteNodeCount(node);
				//                    if (count > 0)
				//                    {
				//                        totalCount += count;
				//                        deleteList.Add(node);
				//                    }
				//                }
				//            }

				//            if (totalCount > _progressMinCount)
				//            {
				//                _progressStatusCount = 0;
				//                _progressStatusMessage = MIDText.GetText(eMIDTextCode.msg_DeleteStatus);
				//                _progressStatusMessage = _progressStatusMessage.Replace("{1}", totalCount.ToString(CultureInfo.CurrentUICulture));
				//                _progressForm = new frmProgress(0, totalCount);
				//                _progressForm.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
				//                _progressForm.Icon = MIDGraphics.GetIcon(MIDGraphics.DeleteIcon);
				//                _progressForm.Show();
				//            }

				//            foreach (MIDTreeNode node in deleteList)
				//            {
				//                parentNode = (MIDTreeNode)node.Parent;
				//                DeleteNode(node);
				//                //Begin Track #6201 - JScott - Store Count removed from attr sets
				//                parentNode.UpdateExternalText();
				//                //End Track #6201 - JScott - Store Count removed from attr sets
				//                if (parentNode.Nodes.Count == 0)
				//                {
				//                    parentNode.SetCollapseImage();
				//                }
				//            }

				//            SelectedNode = nextNode;
				//        }
				//        catch
				//        {
				//            throw;
				//        }
				//        finally
				//        {
				//            EndUpdate();
				//            if (ProgressFormIsDisplayed)
				//            {
				//                _progressForm.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete);
				//                _progressForm.EnableOKButton();
				//            }
				//            _progressForm = null;
				//            Cursor.Current = Cursors.Default;
				//        }
				//    }
				//}
				if (SelectedNode != null)
				{
					try
					{
						Cursor.Current = Cursors.WaitCursor;

						tempList = (ArrayList)_alSelectedNodes.Clone();

						deleteList = new ArrayList();
						totalCount = 0;

						foreach (MIDTreeNode node in tempList)
						{
							if (node.FunctionSecurityProfile.AllowDelete)
							{
								count = GetDeleteNodeCount(node);

								if (count > 0)
								{
									totalCount += count;
									deleteList.Add(node);
								}
							}
						}

						if (totalCount > 0)
						{
                            // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                            //if (SelectedNode.Nodes.Count > 0)
                            //{
                            //    if (SelectedNode.isShortcut)
                            //    {
                            //        msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false);
                            //    }
                            //    else
                            //    {
                            //        msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerParent);
                            //    }
                            //}
                            //else
                            //{
                            //    if (SelectedNode.isShortcut)
                            //    {
                            //        msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false);
                            //    }
                            //    else
                            //    {
                            //        msg = SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerChild);
                            //    }
                            //}

                            eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
							if (SelectedNode.Nodes.Count > 0)
							{
								if (SelectedNode.isShortcut)
								{
                                    msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false, SelectedNode.Text);
                                    messageLevel = SAB.ClientServerSession.Audit.GetMessageLevel(eMIDTextCode.msg_ConfirmRemoveItem);
								}
								else
								{
                                    msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerParent, false, SelectedNode.Text);
                                    messageLevel = SAB.ClientServerSession.Audit.GetMessageLevel(eMIDTextCode.msg_ConfirmDeleteExplorerParent);
								}
							}
							else
							{
								if (SelectedNode.isShortcut)
								{
                                    msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmRemoveItem, false, SelectedNode.Text);
                                    messageLevel = SAB.ClientServerSession.Audit.GetMessageLevel(eMIDTextCode.msg_ConfirmRemoveItem);
								}
								else
								{
                                    msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDeleteExplorerChild, false, SelectedNode.Text);
                                    messageLevel = SAB.ClientServerSession.Audit.GetMessageLevel(eMIDTextCode.msg_ConfirmDeleteExplorerChild);
								}
							}
                            SAB.ClientServerSession.Audit.Add_Msg(messageLevel, msg, this.GetType().Name, true);
                            // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.

							msg = msg.Replace("{0}", SelectedNode.Text);
                            if (MIDEnvironment.isWindows)
                            {
                                retCode = MessageBox.Show(msg, "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            }
                            else // Continue with delete if not Windows
                            {
                                retCode = DialogResult.Yes;
                            }

                            if (retCode == DialogResult.Yes)
                            {
                                try
                                {
                                    if (!AllowInUseDelete(deleteList))
                                    {
                                        MIDEnvironment.requestFailed = true;
                                        return;
                                    }
                                    BeginUpdate();

                                    if (SelectedNode.NextNode != null)
                                    {
                                        nextNode = (MIDTreeNode)SelectedNode.NextNode;
                                    }
                                    else if (SelectedNode.PrevNode != null)
                                    {
                                        nextNode = (MIDTreeNode)SelectedNode.PrevNode;
                                    }
                                    else
                                    {
                                        nextNode = (MIDTreeNode)SelectedNode.Parent;
                                    }

                                    if (totalCount > _progressMinCount)
                                    {
                                        _progressStatusCount = 0;
                                        _progressStatusMessage = MIDText.GetText(eMIDTextCode.msg_DeleteStatus);
                                        _progressStatusMessage = _progressStatusMessage.Replace("{1}", totalCount.ToString(CultureInfo.CurrentUICulture));
                                        _progressForm = new frmProgress(0, totalCount);
                                        _progressForm.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Deleting);
                                        _progressForm.Icon = MIDGraphics.GetIcon(MIDGraphics.DeleteIcon);
                                        _progressForm.Show();
                                    }

                                    foreach (MIDTreeNode node in deleteList)
                                    {
                                        parentNode = (MIDTreeNode)node.Parent;
                                        // Begin TT#3630 - JSmith - Delete My Hierarchy
                                        //DeleteNode(node);
                                        //parentNode.UpdateExternalText();
                                        //if (parentNode.Nodes.Count == 0)
                                        //{
                                        //    parentNode.SetCollapseImage();
                                        //}
                                        DeleteNode(node, out deleteCancelled);
                                        if (!deleteCancelled)
                                        {
                                            parentNode.UpdateExternalText();
                                            if (parentNode.Nodes.Count == 0)
                                            {
                                                parentNode.SetCollapseImage();
                                            }
                                        }
										// End TT#3630 - JSmith - Delete My Hierarchy
                                    }

                                    // Begin TT#3630 - JSmith - Delete My Hierarchy
                                    //SelectedNode = nextNode;
                                    //// Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                                    //SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_DeleteComplete, this.GetType().Name, true);
                                    //// End TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                                    if (!deleteCancelled)
                                    {
                                        SelectedNode = nextNode;
                                        // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_DeleteComplete, this.GetType().Name, true);
                                        // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                                    }
                                    else
                                    {
                                        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DeleteCancelled));
                                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_DeleteCancelled, this.GetType().Name, true);
                                    }
                                    // End TT#3630 - JSmith - Delete My Hierarchy
                                }
                                catch
                                {
                                    throw;
                                }
                                finally
                                {
                                    EndUpdate();
                                    if (ProgressFormIsDisplayed)
                                    {
                                        _progressForm.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_DeleteComplete);
                                        _progressForm.EnableOKButton();
                                    }
                                    _progressForm = null;
                                }
                            }
                            // Begin TT#3435 - JSmith - Errors messages not complete with encounter database errors.
                            else
                            {
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, eMIDTextCode.msg_DeleteCancelled, this.GetType().Name, true);
                            }
                            // End TT#3435 - JSmith - Errors messages not complete with encounter database errors.
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
				//End TT#716 - JScott - prevent the "Available Stores' set from being deleted from the Store Explorer
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public void InUseTreeNode()
        {
            string msg;
            DialogResult retCode;
            MIDTreeNode nextNode;
            MIDTreeNode parentNode;
            ArrayList tempList;
            ArrayList inUseList;
            int totalCount;
            int count;

            if (SelectedNode != null)
            {
                Cursor.Current = Cursors.WaitCursor;

                tempList = (ArrayList)_alSelectedNodes.Clone();

                inUseList = new ArrayList();
                totalCount = 0;
                
						foreach (MIDTreeNode node in tempList)
						{
                            //BEGIN TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                            //if (node.FunctionSecurityProfile.AllowDelete)
                            //{
                                count = GetInUseNodeCount(node);

                                if (count > 0)
                                {
                                    totalCount += count;
                                    inUseList.Add(node);
                                }
                            //}
                            //END TT#3185-M-VStuart-In Use does not work in all explorers-ANFUser
                        }
                        // We don't expect more than one node at a time.
                        if (totalCount > 0 && totalCount < 2)
                        {
                            foreach (MIDTreeNode node in inUseList)
                            {
                                parentNode = (MIDTreeNode)node.Parent;
                                InUseNode(node);
                            }
                        }
            }

        }
        //END TT#110-MD-VStuart - In Use Tool

        // Begin TT#564 - JSmith - Copy/Paste from search not working
        public bool PasteFromClipboard(eCutCopyOperation aCutCopyOperation)
        {
            MIDTreeNode prevParent;
            TreeNodeClipboardList cbList;
            DragDropEffects dropAction;
            string message = null;

            try
            {
                DataObject data = (DataObject)Clipboard.GetDataObject();
                if (SelectedNode != null)
                {
                    if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));

                        if (SelectedNode.GetTopSourceNode() == ((TreeNodeClipboardProfile)cbList.ClipboardItems[0]).Node.GetTopSourceNode())
                        {
                            dropAction = DragDropEffects.Copy;
                        }
                        else if (SelectedNode.GetTopSourceNode().isMainFavoriteFolder ||
                            MakeShortcutNode(SelectedNode, cbList.ClipboardItems, eCutCopyOperation.Copy))
                        {
                            dropAction = DragDropEffects.Link;
                        }
                        else
                        {
                            dropAction = DragDropEffects.Copy;
                        }

                        if (dropAction == DragDropEffects.Copy)
                        {
                            if (cbList.ClipboardItems.Count == 1)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNode, false);
                                message = message.Replace("{0}", cbList.ClipboardProfile.Node.Text);
                                message = message.Replace("{1}", SelectedNode.Text);
                            }
                            else
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNodes, false);
                                message = message.Replace("{0}", SelectedNode.Text);
                            }

                            if (MIDEnvironment.isWindows && MessageBox.Show(message, "Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return false;
                            }
                        }
                        else if (dropAction == DragDropEffects.Link)
                        {
                            if (cbList.ClipboardItems.Count == 1)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortCut, false);
                                message = message.Replace("{0}", cbList.ClipboardProfile.Node.Text);
                                message = message.Replace("{1}", SelectedNode.Text);
                            }
                            else
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortcuts, false);
                                message = message.Replace("{0}", SelectedNode.Text);
                            }

                            if (MIDEnvironment.isWindows && MessageBox.Show(message, "Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return false;
                            }
                        }

                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            BeginUpdate();

                            foreach (TreeNodeClipboardProfile clipBoardProfile in cbList.ClipboardItems)
                            {
                                if (dropAction == DragDropEffects.Move)
                                {
                                    prevParent = (MIDTreeNode)clipBoardProfile.Node.Parent;
                                    MoveNode(clipBoardProfile.Node, SelectedNode);
                                    RefreshShortcuts(_favoritesNode, prevParent);
                                    RefreshShortcuts(_favoritesNode, SelectedNode);
                                    SortChildNodes(SelectedNode);
                                    SelectedNode.UpdateExternalText();
                                    prevParent.UpdateExternalText();
                                    if (prevParent.Nodes.Count == 0)
                                    {
                                        prevParent.SetCollapseImage();
                                    }
                                }
                                else if (dropAction == DragDropEffects.Copy)
                                {
                                    CopyNode(clipBoardProfile.Node, SelectedNode, true);
                                    RefreshShortcuts(_favoritesNode, SelectedNode);
                                    SortChildNodes(SelectedNode);
                                    SelectedNode.UpdateExternalText();
                                }
                                else if (dropAction == DragDropEffects.Link)
                                {
                                    CreateShortcut(clipBoardProfile.Node, SelectedNode);
                                    SortChildNodes(SelectedNode);
                                    SelectedNode.UpdateExternalText();
                                }
                            }
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                            EndUpdate();
                        }
                    }
                    else
                    {
                        return CustomPasteFromClipboard(aCutCopyOperation);
                    }
                }

                this.CurrentEffect = DragDropEffects.Move;
                this.DragEffect = DragDropEffects.None;
            }
            catch (Exception exc)
            {
                HandleException(exc);
                return false;
            }
            finally
            {
                this.Invalidate();
                Cursor.Current = Cursors.Default;
            }
            return true;
        }
        // End TT#564

		public bool PasteTreeNode(ArrayList aCutCopyNodes, eCutCopyOperation aCutCopyOperation)
		{
			MIDTreeNode currNode;
			MIDTreeNode nextNode = null;
			MIDTreeNode prevParent;
            // Begin TT#22 - JSmith - Cannot cut/paste sub folders to move folders in Favorites
            MIDTreeNode cutCopyNode;
            // End TT#22
			string msg = null;
			string title = null;
			int totalCount;
			int count;

			try
			{
                currNode = (MIDTreeNode)SelectedNode;
                // Begin TT#22 - JSmith - Cannot cut/paste sub folders to move folders in Favorites
                if (aCutCopyNodes.Count < 1)
                {
                    return true;
                }
                cutCopyNode = (MIDTreeNode)aCutCopyNodes[0];
                
                //if (currNode.GetTopSourceNode().isMainFavoriteFolder ||
                //    MakeShortcutNode(currNode, aCutCopyNodes))
				if ((currNode.GetTopSourceNode().isMainFavoriteFolder &&
                    !cutCopyNode.GetTopSourceNode().isMainFavoriteFolder) ||
                    // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                    (currNode.GetTopSourceNode().isMainFavoriteFolder &&
                     cutCopyNode.isObjectShortcut &&
                     aCutCopyOperation == eCutCopyOperation.Copy) ||
                    MakeShortcutNode(currNode, aCutCopyNodes, aCutCopyOperation))
                    //MakeShortcutNode(currNode, aCutCopyNodes))
                // End TT#394
                // End TT#22
				{
					title = "Reference";
					if (aCutCopyNodes.Count == 1)
					{
						msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortCut, false);
						msg = msg.Replace("{0}", ((MIDTreeNode)aCutCopyNodes[0]).Text);
						msg = msg.Replace("{1}", currNode.Text);
					}
					else
					{
						msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortcuts, false);
						msg = msg.Replace("{0}", currNode.Text);
					}

					if (MIDEnvironment.isWindows && MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return false;
					}

					if (aCutCopyNodes.Count > 0)
					{
						try
						{
							Cursor.Current = Cursors.WaitCursor;
							BeginUpdate();

							foreach (MIDTreeNode node in aCutCopyNodes)
							{
								CreateShortcut(node, (MIDTreeNode)SelectedNode);
								SortChildNodes(currNode);
							}
						}
						catch
						{
							throw;
						}
						finally
						{
							EndUpdate();
							Cursor.Current = Cursors.Default;
						}
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					if (SelectedNode != null)
					{
                        //totalCount = 0;
                        //foreach (MIDTreeNode node in aCutCopyNodes)
                        //{
                        //    count = GetDescendantCount(node);
                        //    if (count > 0)
                        //    {
                        //        totalCount += count;
                        //    }
                        //}

						if (aCutCopyOperation == eCutCopyOperation.Copy)
						{
                            totalCount = 0;
                            foreach (MIDTreeNode node in aCutCopyNodes)
                            {
                                count = GetDescendantCount(node);
                                if (count > 0)
                                {
                                    totalCount += count;
                                }
                            }

							title = "Copy";
							//if (aCutCopyNodes.Count == 1)
							if (totalCount == 1)
							{
								msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNode, false);
								msg = msg.Replace("{0}", ((MIDTreeNode)aCutCopyNodes[0]).Text);
								msg = msg.Replace("{1}", currNode.Text);
							}
							else if (aCutCopyNodes.Count == 1)
							{
								msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyExplorerParent);
                                // Begin TT#3281 - JSmith - Copying node in Merchandise Explorer
                                //msg = msg.Replace("{0}", currNode.Text);
                                msg = msg.Replace("{0}", ((MIDTreeNode)aCutCopyNodes[0]).Text);
                                // End TT#3281 - JSmith - Copying node in Merchandise Explorer
							}
							else if (aCutCopyNodes.Count == 1)
							{
								msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNodes, false);
								msg = msg.Replace("{0}", currNode.Text);
							}
						}
						else
						{
                            totalCount = aCutCopyNodes.Count;

							title = "Move";
							//if (aCutCopyNodes.Count == 1)
							if (totalCount == 1)
							{
								msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNode, false);
								msg = msg.Replace("{0}", ((MIDTreeNode)aCutCopyNodes[0]).Text);
								msg = msg.Replace("{1}", currNode.Text);
							}
							else
							{
								msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmMoveNodes, false);
								msg = msg.Replace("{0}", currNode.Text);
							}
						}

						if (MIDEnvironment.isWindows && MessageBox.Show(msg, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
						{
							return false;
						}

						try
						{
							Cursor.Current = Cursors.WaitCursor;

							if (totalCount > _progressMinCount)
							{
								_progressStatusCount = 0;
								_progressStatusMessage = MIDText.GetText(eMIDTextCode.msg_CopyStatus);
								_progressStatusMessage = _progressStatusMessage.Replace("{1}", totalCount.ToString(CultureInfo.CurrentUICulture));
								_progressForm = new frmProgress(0, totalCount);
								_progressForm.Title = MIDText.GetTextOnly(eMIDTextCode.msg_Copying);
								_progressForm.Icon = MIDGraphics.GetIcon(MIDGraphics.CopyIcon);
								_progressForm.Show();
							}

							BeginUpdate();

							foreach (MIDTreeNode node in aCutCopyNodes)
							{
								if (aCutCopyOperation == eCutCopyOperation.Copy)
								{
									nextNode = CopyNode(node, currNode, true);
									RefreshShortcuts(_favoritesNode, currNode);
									SortChildNodes(currNode);
									currNode.UpdateExternalText();
								}
								else if (aCutCopyOperation == eCutCopyOperation.Cut)
								{
									if (currNode != node.GetParentNode() && currNode != node && !currNode.isChildOf(node))
									{
										prevParent = (MIDTreeNode)node.Parent;
										nextNode = MoveNode(node, currNode);
										RefreshShortcuts(_favoritesNode, prevParent);
										RefreshShortcuts(_favoritesNode, currNode);
										SortChildNodes(currNode);
										//Begin Track #6201 - JScott - Store Count removed from attr sets
										currNode.UpdateExternalText();
										prevParent.UpdateExternalText();
										//End Track #6201 - JScott - Store Count removed from attr sets
										if (prevParent.Nodes.Count == 0)
                                        {
                                            prevParent.SetCollapseImage();
                                        }
									}
								}
							}

							if (nextNode != null)
							{
								SelectedNode = nextNode;
							}
						}
						catch
						{
							throw;
						}
						finally
						{
							EndUpdate();
							Cursor.Current = Cursors.Default;
						}

						return true;
					}
					else
					{
						return false;
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
				if (ProgressFormIsDisplayed)
				{
					_progressForm.labelText = MIDText.GetTextOnly(eMIDTextCode.msg_CopyComplete);
					_progressForm.EnableOKButton();
				}
				_progressForm = null;
			}
		}

		public void EditTreeNode(MIDTreeNode aNode)
		{
			MIDTreeNode parentNode;

			try
			{
                // Begin TT#62 - JSmith - Object reference error when double-click folder
                if (aNode == null)
                {
                    return;
                }
                // End TT#62
				parentNode = (MIDTreeNode)aNode.Parent;
				EditNode(aNode);
				RefreshShortcuts(FavoritesNode, parentNode);
			}
			catch
			{
				throw;
			}
		}

		public MIDTreeNode FindTreeNode(TreeNodeCollection aNodes, eProfileType aNodeType, int aNodeRID, bool autoExpandWhileFinding = true)
		{
			MIDTreeNode findNode;

			try
			{
				foreach (MIDTreeNode tn in aNodes)
				{
					if (tn.NodeProfileType == aNodeType && tn.NodeRID == aNodeRID)
					{
						return tn;
					}
				}

				foreach (MIDTreeNode tn in aNodes)
				{
                    if (autoExpandWhileFinding && tn.HasChildren && tn.DisplayChildren && !tn.ChildrenLoaded)
                    {
                        MIDTreeView_BeforeExpand(this, new TreeViewCancelEventArgs(tn, false, TreeViewAction.Expand));
                    }
					
                    findNode = FindTreeNode(tn.Nodes, aNodeType, aNodeRID, autoExpandWhileFinding);

					if (findNode != null)
					{
						return findNode;
					}
				}

				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        public MIDTreeNode FindTreeNode(TreeNodeCollection aNodes, eProfileType aNodeType, int aNodeRID, int ownerUserRID, bool autoExpandWhileFinding = true)
        {
            MIDTreeNode findNode;

            try
            {
                foreach (MIDTreeNode tn in aNodes)
                {
                    if (tn.NodeProfileType == aNodeType && tn.NodeRID == aNodeRID && tn.OwnerUserRID == ownerUserRID)
                    {
                        return tn;
                    }
                }

                foreach (MIDTreeNode tn in aNodes)
                {
                    if (autoExpandWhileFinding && tn.HasChildren && tn.DisplayChildren && !tn.ChildrenLoaded)
                    {
                        MIDTreeView_BeforeExpand(this, new TreeViewCancelEventArgs(tn, false, TreeViewAction.Expand));
                    }

                    findNode = FindTreeNode(tn.Nodes, aNodeType, aNodeRID, ownerUserRID, autoExpandWhileFinding);

                    if (findNode != null)
                    {
                        return findNode;
                    }
                }

                return null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDTreeNode FindTreeNode(TreeNodeCollection aNodes, string uniqueID, bool autoExpandWhileFinding = true)
        {
            MIDTreeNode findNode;

            try
            {
                foreach (MIDTreeNode tn in aNodes)
                {
                    if (tn.UniqueID == uniqueID)
                    {
                        return tn;
                    }
                }

                foreach (MIDTreeNode tn in aNodes)
                {
                    if (autoExpandWhileFinding && tn.HasChildren && tn.DisplayChildren && !tn.ChildrenLoaded)
                    {
                        MIDTreeView_BeforeExpand(this, new TreeViewCancelEventArgs(tn, false, TreeViewAction.Expand));
                    }

                    findNode = FindTreeNode(tn.Nodes, uniqueID, autoExpandWhileFinding);

                    if (findNode != null)
                    {
                        return findNode;
                    }
                }

                return null;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDTreeNode GetTreeNode(TreeNodeClipboardProfile aClipboardProfile)
		{
			try
			{
				MIDTreeNode treeNode = (MIDTreeNode)FindTreeNode(this.Nodes, aClipboardProfile.Node.Profile.ProfileType, aClipboardProfile.Key);
				if (treeNode == null)
				{
					return BuildNode(string.Empty, aClipboardProfile.Key);
				}
				else
				{
					return treeNode;
				}
			}
			catch
			{
				throw;
			}
		}

        public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ArrayList aSelectedNodes)
		{
			bool dropAllowed;

			try
			{
                // Begin TT#564 - JSmith - Copy/Paste from search not working
                //if (aDropNode == null || aSelectedNodes.Count == 0)
                //{
                //    return false;
                //}
                if (aDropNode == null || aSelectedNodes == null || aSelectedNodes.Count == 0)
                {
                    return false;
                }
                // End TT#564

                // Begin Track #6478 - JSmith - Create My Hier-drag sku and it droped under a sku in org hier 
                //dropAllowed = false;

                //foreach (MIDTreeNode treeNode in aSelectedNodes)
                //{
                //    treeNode.AllowDrop = treeNode.isDropAllowed(aDropAction, aDropNode);

                //    if (treeNode.AllowDrop)
                //    {
                //        dropAllowed = true;
                //        CurrentEffect = treeNode.DropAction;
                //    }
                //}
                dropAllowed = true;

                foreach (MIDTreeNode treeNode in aSelectedNodes)
                {
                    treeNode.AllowDrop = treeNode.isDropAllowed(aDropAction, aDropNode);

                    if (!treeNode.AllowDrop)
                    {
                        dropAllowed = false;
                        CurrentEffect = treeNode.DropAction;
                    }
                }
                // End Track #6478

				return dropAllowed;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Copies a ClipboardProfile to the WIndows clipboard
		/// </summary>
		/// <param name="aClipboardList">
		/// The ClipboardList object to put in the clipboard
		/// </param>

		public void CopyToClipboard(ClipboardListBase aClipboardList)
		{
			IDataObject ido;

			try
			{
				ido = new DataObject();
				ido.SetData(typeof(TreeNodeClipboardList), aClipboardList);
				Clipboard.SetDataObject(ido, true);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public string GetUserName(int aUserRID)
		{
			try
			{
				switch (aUserRID)
				{
					case Include.GlobalUserRID:		// Issue 3806
						return "Global";
					default:
						return SAB.ClientServerSession.GetUserName(aUserRID);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void PaintSelectedNodes()
		{
			try
			{
				if (_alSelectedNodes.Count == 0)
				{
					return;
				}

				foreach (MIDTreeNode tn in _alSelectedNodes)
				{
					PaintNode(tn);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemovePaintFromNodes()
		{
            //TreeNodeCollection nodes;

			try
			{
                // Begin Track #6295 - JSmith - Try to attach sku to added header 
                //nodes = this.Nodes;

                //foreach (MIDTreeNode tn in nodes)
                //{
                //    if (tn.Nodes.Count > 0)
                //    {
                //        RemovePaintFromNode(tn);
                //        RemovePaintFromNodes(tn); // repaint the children
                //    }
                //    else
                //    {
                //        RemovePaintFromNode(tn);
                //    }
                //}

                foreach (MIDTreeNode tn in _alPaintedNodes)
                {
                    RemovePaintFromNode(tn);
                }
                _alPaintedNodes.Clear();
                // End Track #6295
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void RemoveDeletedNodes()
		{
			TreeNodeCollection nodes;

			try
			{
				nodes = Nodes;

				foreach (MIDTreeNode tn in nodes)
				{
					if (tn.NodeChangeType == eChangeType.delete)
					{
						Nodes.Remove(tn);
					}
					else
					{
						RecursiveRemoveDeletedNodes(tn, null);
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public int GetFolderImageIndex(bool aIsReference, string aColor, string aFolderType)
		{
			try
			{
				if (aIsReference)
				{
					return MIDGraphics.ImageShortcutIndexWithDefault(aColor, aFolderType);
				}
				else
				{
					return MIDGraphics.ImageIndexWithDefault(aColor, aFolderType);
				}
			}
			catch
			{
				throw;
			}
		}

		public MIDTreeNode GetNodeByItemRID(TreeNodeCollection aNodes, int aItemRID, eProfileType aItemType)
		{
			MIDTreeNode foundNode;

			try
			{
				foreach (MIDTreeNode node in aNodes)
				{
					if (node.Profile.Key == aItemRID && node.Profile.ProfileType == aItemType)
					{
						return node;
					}

					if (node.ChildrenLoaded)
					{
						foundNode = GetNodeByItemRID(node.Nodes, aItemRID, aItemType);

						if (foundNode != null)
						{
							return foundNode;
						}
					}
				}

				return null;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void DeleteChildNodes(MIDTreeNode aNode)
		{
			object[] deleteList;

			try
			{
				deleteList = new object[aNode.Nodes.Count];
				aNode.Nodes.CopyTo(deleteList, 0);

				foreach (MIDTreeNode node in deleteList)
				{
					DeleteChildNodes(node);
					node.Remove();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void SortChildNodes(MIDTreeNode aParentNode)
		{
			MIDTreeNode selectedNode;
			MIDTreeNode[] nodeArray;
			int i;
			SortedList folderList;
			SortedList itemList;
			IDictionaryEnumerator dictEnum;

			try
			{
                //if (!aParentNode.isChildrenSorted())
                //{
                //    return;
                //}
				BeginUpdate();

				try
				{
					selectedNode = SelectedNode;

					if (aParentNode.Nodes.Count > 0)
					{
						nodeArray = new MIDTreeNode[aParentNode.Nodes.Count];
						aParentNode.Nodes.CopyTo(nodeArray, 0);
						aParentNode.Nodes.Clear();

						folderList = new SortedList();
						itemList = new SortedList();

						for (i = 0; i < nodeArray.Length; i++)
						{
							if (nodeArray[i].TreeNodeType == eTreeNodeType.SubFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.MainNonSourceFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.MainSourceFolderNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                                nodeArray[i].TreeNodeType == eTreeNodeType.ChildFolderShortcutNode)
							{
								if (nodeArray[i].Nodes.Count > 0)
								{
									SortChildNodes(nodeArray[i]);
								}
                                try
                                {
								folderList.Add(nodeArray[i], nodeArray[i]);
                                }
                                catch
                                {

                                }
							}
							else if (nodeArray[i].TreeNodeType == eTreeNodeType.ObjectNode ||
								nodeArray[i].TreeNodeType == eTreeNodeType.ChildObjectShortcutNode ||
								nodeArray[i].TreeNodeType == eTreeNodeType.ObjectShortcutNode)
							{
                                try
                                {
                                    itemList.Add(nodeArray[i], nodeArray[i]);
                                }
                                catch
                                {

                                }
							}
						}

						dictEnum = folderList.GetEnumerator();

						while (dictEnum.MoveNext())
						{
							aParentNode.Nodes.Add((MIDTreeNode)dictEnum.Value);
						}

						dictEnum = itemList.GetEnumerator();

						while (dictEnum.MoveNext())
						{
							aParentNode.Nodes.Add((MIDTreeNode)dictEnum.Value);
						}
					}

					SelectedNode = (MIDTreeNode)selectedNode;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					EndUpdate();
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // Begin TT#176 - JSmith - New database has errors
        public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText)
        {
            bool newFolder = false;

            try
            {
                return Folder_Get(aUserRID, aFolderType, aDefaultText, ref newFolder);
            }
            catch
            {
                throw;
            }
        }
        // End TT#176

		/// <summary>
		/// Retrieves a FolderProfile object for the user and profile type.
		/// </summary>
		/// <param name="aUserRID">The key of the user</param>
		/// <param name="aFolderType">The type of folder</param>
		/// <param name="aDefaultText">The text to use to create the node if not found</param>
		/// <returns></returns>

        // Begin TT#176 - JSmith - New database has errors
        //public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText)
        public FolderProfile Folder_Get(int aUserRID, eProfileType aFolderType, string aDefaultText, ref bool aNewFolder)
        // End TT#176
		{
			DataTable dtFolders;
			FolderProfile folderProf;
			int key;

			try
			{
				dtFolders = DlFolder.Folder_Read(aUserRID, aFolderType);
				if (dtFolders == null || dtFolders.Rows.Count == 0)
				{
					key = Folder_Create(aUserRID, aDefaultText, aFolderType);
					folderProf = new FolderProfile(key, aUserRID, aFolderType, aDefaultText, aUserRID);
                    // Begin TT#176 - JSmith - New database has errors
                    aNewFolder = true;
                    // End TT#176
				}
				else
				{
					folderProf = new FolderProfile(dtFolders.Rows[0]);
                    // Begin TT#176 - JSmith - New database has errors
                    aNewFolder = false;
                    // End TT#176
				}
			}
			catch
			{
				throw;
			}

			return folderProf;
		}

		/// <summary>
		/// Retrieves a list of folder for a list of users and profile type
		/// </summary>
		/// <param name="aUserRIDList">A list of user keys</param>
		/// <param name="aFolderType">The type of folder</param>
		/// <returns></returns>

		protected DataTable Folder_Get(ArrayList aUserRIDList, eProfileType aFolderType)
		{
			try
			{
				return DlFolder.Folder_Read(aUserRIDList, aFolderType, true, false);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Check is children exist for the folder
		/// </summary>
		/// <param name="aUserRID">
		/// The key of the user for the folder
		/// </param>
		/// <param name="aKey">The key of the folder</param>
		/// <returns>A flag identifying if children exist for the folder</returns>

		public bool Folder_Children_Exists(int aUserRID, int aKey)
		{
			try
			{
				return DlFolder.Folder_Children_Exists(aUserRID, aKey);
			}
			catch
			{
				throw;
			}
		}

		//-----------------
		//PROTECTED METHODS
		//-----------------

		protected void IncrementProgressStatusCount()
		{
			int count;
			if (_progressForm != null &&
				_progressStatusMessage != null)
			{
				count = ++_progressStatusCount;
				_progressForm.SetValue = count;
				_progressForm.labelText = _progressStatusMessage.Replace("{0}", count.ToString(CultureInfo.CurrentUICulture));
			}
		}

		protected bool isParent(TreeNode parentNode, TreeNode childNode)
        {
            TreeNode n;
            bool bFound;

            try
            {
                if (parentNode == childNode)
                {
                    return true;
                }

                n = childNode;
                bFound = false;

                while (!bFound && n != null)
                {
                    n = n.Parent;
                    bFound = (n == parentNode);
                }

                return bFound;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		/// <summary>
		/// Creates a folder for a user
		/// </summary>
		/// <param name="aUserRID">The key of the user</param>
		/// <param name="aText">The text for the folder</param>
		/// <param name="aFolderType">The type of the folder</param>
		/// <returns>The key of the new folder</returns>

		protected int Folder_Create(int aUserRID, string aText, eProfileType aFolderType)
		{
			int key;
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					key = DlFolder.Folder_Create(aUserRID, aText, aFolderType);
					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}

				return key;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates a folder for a user
		/// </summary>
		/// <param name="aUserRID">The key of the user</param>
		/// <param name="aParentFolderKey">The key of the parent folder to which the new folder is to be added</param>
		/// <param name="aText">The text for the folder</param>
		/// <param name="aFolderType">The type of the folder</param>
		/// <returns>The key of the new folder</returns>

		protected int Folder_Create(int aUserRID, int aParentFolderKey, string aText, eProfileType aFolderType)
		{
			int key;
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					key = DlFolder.Folder_Create(aUserRID, aText, aFolderType);
					DlFolder.Folder_Item_Insert(aParentFolderKey, key, aFolderType);
					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}

				return key;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates a shortcut in a folder
		/// </summary>
		/// <param name="aParentKey">The key of the folder where the shortcut is being added</param>
		/// <param name="aKey">The key of the item to which a shortcut is being made</param>
		/// <param name="aFolderChildType">The profile type of the item to which a shortcut is being made</param>

		protected void Folder_Create_Shortcut(int aParentKey, int aKey, eProfileType aFolderChildType)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					DlFolder.Folder_Shortcut_Insert(aParentKey, aKey, aFolderChildType);
					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Retrieves a list of shortcuts for a user list and folder type list
		/// </summary>
		/// <param name="aUserRIDList">An ArrayList of user keys</param>
		/// <param name="aFolderTypeList">An ArrayList of profile types</param>
		/// <returns></returns>

		protected DataTable Folder_Get_Shortcuts(ArrayList aUserRIDList, ArrayList aFolderTypeList)
		{
			try
			{
				return DlFolder.Folder_Shortcut_Folder_Read(aUserRIDList, aFolderTypeList);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes a folder
		/// </summary>
		/// <param name="aKey">The key of the folder</param>

		protected void Folder_Delete(int aKey, eProfileType aFolderType)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
                    DlFolder.Folder_Item_Delete(aKey, aFolderType);
					DlFolder.Folder_Delete(aKey, aFolderType);
					DlFolder.CommitData();
				}
				catch (DatabaseForeignKeyViolation)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes a shortcut
		/// </summary>
		/// <param name="aFolderKey">The key of the folder</param>
		/// <param name="aKey">The key of the item being deleted</param>
		/// <param name="aFolderType">The folder type of the item being deleted</param>

		protected void Folder_Delete_Shortcut(int aFolderKey, int aKey, eProfileType aFolderType)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					DlFolder.Folder_Shortcut_Delete(aFolderKey, aKey, aFolderType);

					DlFolder.CommitData();
				}
				catch (DatabaseForeignKeyViolation)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#29 - JSmith - Deleting node is not removing reference in favorites
        /// <summary>
        /// Deletes all shortcuts for a key
        /// </summary>
        /// <param name="aKey">The key of the item being deleted</param>
        /// <param name="aFolderType">The folder type of the item being deleted</param>

        protected void Folder_DeleteAll_Shortcut(int aKey, eProfileType aFolderType)
        {
            try
            {
                DlFolder.OpenUpdateConnection();

                try
                {
                    DlFolder.Folder_Shortcut_DeleteAll(aKey, aFolderType);

                    DlFolder.CommitData();
                }
                catch (DatabaseForeignKeyViolation)
                {
                    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    DlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#29

		/// <summary>
		/// Renames a folder
		/// </summary>
		/// <param name="aKey">The key of the folder</param>
		/// <param name="aText">The new text for the folder</param>

		protected void Folder_Rename(int aKey, string aText)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					DlFolder.Folder_Rename(aKey, aText.Trim());
					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Moves a folder from one folder to another
		/// </summary>
		/// <param name="aCurrentParentKey">The key of the current parent folder</param>
		/// <param name="aNewParentKey">The key of the new parent folder</param>
		/// <param name="aKey">The key of the item being moved</param>
		/// <param name="aFolderType">The folder type of the item being moved</param>

		protected void Folder_Move(int aCurrentParentKey, int aNewParentKey, int aKey, eProfileType aFolderType)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					DlFolder.Folder_Item_Delete(aKey, aFolderType);
					DlFolder.Folder_Item_Insert(aNewParentKey, aKey, aFolderType);

					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

        /// <summary>
        /// Add an item to a folder
        /// </summary>
        /// <param name="aNewParentKey">The key of the new parent folder</param>
        /// <param name="aKey">The key of the item being moved</param>
        /// <param name="aFolderType">The folder type of the item being moved</param>

        protected void Folder_Item_Insert(int aParentKey, int aKey, eProfileType aFolderType)
        {
            try
            {
                DlFolder.OpenUpdateConnection();

                try
                {
                    DlFolder.Folder_Item_Insert(aParentKey, aKey, aFolderType);

                    DlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    DlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// Moves a shortcut from one folder to another
		/// </summary>
		/// <param name="aCurrentParentKey">The key of the current parent folder</param>
		/// <param name="aNewParentKey">The key of the new parent folder</param>
		/// <param name="aKey">The key of the item being moved</param>
		/// <param name="aFolderType">The folder type of the item being moved</param>

		protected void Folder_Move_Shortcut(int aCurrentParentKey, int aNewParentKey, int aKey, eProfileType aFolderType)
		{
			try
			{
				DlFolder.OpenUpdateConnection();

				try
				{
					DlFolder.Folder_Shortcut_Delete(aCurrentParentKey, aKey, aFolderType);
					DlFolder.Folder_Shortcut_Insert(aNewParentKey, aKey, aFolderType);

					DlFolder.CommitData();
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
				finally
				{
					DlFolder.CloseUpdateConnection();
				}
			}
			catch
			{
				throw;
			}
		}

		protected FolderProfile Folder_Create_New(int aParentRID, int aUserRID, eProfileType aFolderType)
		{
			FolderProfile newFolderProf;
			string newName;

			try
			{
				newName = Folder_Find_New_Name("New Folder", aUserRID, aParentRID, aFolderType);
				newFolderProf = new FolderProfile(Include.NoRID, aUserRID, eProfileType.MerchandiseSubFolder, newName, aUserRID);
				newFolderProf.Key = Folder_Create(newFolderProf.UserRID, aParentRID, newFolderProf.Name, newFolderProf.FolderType);

			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}

			return newFolderProf;
		}

		protected string Folder_Find_New_Name(string aFolderName, int aUserRID, int aParentRID, eProfileType aFolderType)
		{
			int index;
			string newName;
			int key;

			try
			{
				index = 1;
				newName = aFolderName;
				key = DlFolder.Folder_GetKey(aUserRID, newName, aParentRID, aFolderType);

				while (key != -1)
				{
					index++;
					newName = aFolderName + " (" + index + ")";
					key = DlFolder.Folder_GetKey(aUserRID, newName, aParentRID, eProfileType.MerchandiseSubFolder);
				}

				return newName;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//---------------
		//PRIVATE METHODS
		//---------------

        private void MIDTreeView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            bool bControl;
            bool bShift;

            try
            {
                bControl = false;
                bShift = false;

                _mouseDownPoint.X = e.X;
                _mouseDownPoint.Y = e.Y;

                if (_bAllowMultiSelect)
                {
                    if (_bSimulateMultiSelect)
                    {
                        bControl = true;
                    }
                    else
                    {
                        bControl = (ModifierKeys == Keys.Control);
                    }

                    bShift = (ModifierKeys == Keys.Shift);
                }

                if (bControl)
                {
                    CurrentEffect = DragDropEffects.Copy;
                }
                else
                {
                    CurrentEffect = DragDropEffects.Move;
                }

                _tnMouseDownOnNode = (MIDTreeNode)GetNodeAt(_mouseDownPoint.X, _mouseDownPoint.Y);

                if (_tnMouseDownOnNode != null)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        _bRightMousePressed = true;
                        base.SelectedNode = _tnMouseDownOnNode;
                    }
                    else if (e.Button == MouseButtons.Left)
                    {
                        _bLeftMousePressed = true;
                    }

                    if (!bShift)
                    {
                        _tnArrowNode = _tnMouseDownOnNode;
                        _tnFirstNode = _tnMouseDownOnNode;
                        _tnLastNode = _tnMouseDownOnNode;
                    }
                }
                else
                {
                    base.SelectedNode = null;
                    _tnFirstNode = null;
                    _tnLastNode = null;
                    _alSelectedNodes.Clear();
                    RemovePaintFromNodes();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                CurrentState = eDragStates.Idle;
                DragEffect = DragDropEffects.None;

                if (_timer != null)
                {
                    _timer.Stop();
                }

                _bTimerRunning = false;
                _bTimerActivated = false;
                _tnAutoExpandNode = null;
                _bRightMousePressed = false;
                _bLeftMousePressed = false;
                _bDragOperation = false;

                // End dragging image
                DragHelper.ImageList_EndDrag();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            MIDTreeNode currNode;
            string message;

            try
            {
                if (_timer != null && _bAllowAutoExpand)
                {
                    _timer.Stop();
                    _bTimerRunning = false;

                    if (_tnAutoExpandNode != null)
                    {
                        _timer.Start();
                        _bTimerRunning = true;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            MIDTreeNode node;
			//ArrayList children;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                node = (MIDTreeNode)e.Node;
                if (node.HasChildren && node.DisplayChildren && !node.ChildrenLoaded)
                {
                    BeginUpdate();

                    node.Nodes.Clear();
					//children = node.BuildChildren();
					//node.Nodes.AddRange((MIDTreeNode[])children.ToArray(typeof(MIDTreeNode)));
					node.BuildChildren();
					SortChildNodes(node);
                    
                    node.ChildrenLoaded = true;
                }
            }

            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                EndUpdate();

                Cursor.Current = Cursors.Default;
            }
        }

        private void MIDTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
                ((MIDTreeNode)e.Node).SetExpandImage();
                base.Invalidate(e.Node.Bounds);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            try
            {
                ((MIDTreeNode)e.Node).SetCollapseImage();
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            MIDTreeNode node;

            try
            {
                node = (MIDTreeNode)e.Node;

				//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename
				if (node.Profile != null)
				{
				//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
					if (node.isShortcut)
					{
						e.CancelEdit = true;
					}
					else
					{
						if (node.isLabelEditAllowed())
						{
							if (!node.FunctionSecurityProfile.AllowUpdate)
							{
								e.CancelEdit = true;
							}
						}
						else
						{
							e.CancelEdit = true;
						}
					}
				//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename
				}
				else
				{
					e.CancelEdit = true;
				}
				//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
				//Begin Track #6201 - JScott - Store Count removed from attr sets

				//if (!e.CancelEdit)
				//{
				//    _originalTextLabel = node.Text;
				//    _originalTextLabel = _originalTextLabel.TrimEnd();
				//}
				//End Track #6201 - JScott - Store Count removed from attr sets
			}
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        public void RenameNode(TreeNode node, string label)
        {
            MIDTreeView_AfterLabelEdit(sender: null, e: new NodeLabelEditEventArgs(node, label));
        }

        private void MIDTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
			//Begin Track #6201 - JScott - Store Count removed from attr sets
			//string tempName;

			//try
			//{
			//    if (e.Node != null && e.Label != null)
			//    {
			//        tempName = e.Label.TrimStart();

			//        if (e.Label.Length == 0 || e.Label == _originalTextLabel)
			//        {
			//            e.CancelEdit = true;
			//            return;
			//        }

			//        if (AfterLabelUpdate((MIDTreeNode)e.Node, e.Label))
			//        {
			//            e.Node.EndEdit(false);

			//            BeginUpdate();

			//            try
			//            {
			//                e.Node.Text = e.Label;
			//                if (_favoritesNode != null)
			//                {
			//                    RefreshShortcuts(_favoritesNode, (MIDTreeNode)e.Node);
			//                }
			//            }
			//            catch (Exception exc)
			//            {
			//                string message = exc.ToString();
			//                throw;
			//            }
			//            finally
			//            {
			//                EndUpdate();
			//            }
			//        }
			//        else
			//        {
			//            e.CancelEdit = true;
			//        }
			//    }
			//}
			//catch (Exception exc)
			//{
			//    HandleException(exc);
			//}
			MIDTreeNode node;

			try
			{
                if (e.Node != null && e.Label != null)
                {
                    node = (MIDTreeNode)e.Node;

                    if (e.Label.Length == 0 || e.Label == node.InternalText)
                    {
                        node.UpdateExternalText();
                        e.CancelEdit = true;
                    }

                    if (!e.CancelEdit)
                    {
                        if (AfterLabelUpdate(node, e.Label))
                        {
                            node.EndEdit(false);
                            node.InternalText = e.Label;
                            e.CancelEdit = true;

                            BeginUpdate();

                            try
                            {
                                if (_favoritesNode != null)
                                {
                                    RefreshShortcuts(_favoritesNode, node);
                                }
                            }
                            catch (Exception exc)
                            {
                                string message = exc.ToString();
                                throw;
                            }
                            finally
                            {
                                EndUpdate();
                            }
                        }
                        else
                        {
                            e.CancelEdit = true;
                        }
                    }
                }
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			//End Track #6201 - JScott - Store Count removed from attr sets
		}

        private void MIDTreeView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                }
                if (DragEffect != DragDropEffects.None)
                {
                    return;
                }

                if (e.Control == true)
                {
                    _bControlKeyPressed = true;
                    CurrentEffect = DragDropEffects.Copy;
                    e.Handled = true;
                }
                else
                {
                    CurrentEffect = DragDropEffects.Move;
                }

                if (e.KeyCode == Keys.OemQuestion && (e.Control && e.Shift))
                {
                    if (SAB.AllowDebugging)
                    {
                        if (_bDebugActivated)
                        {
                            _bDebugActivated = false;
                            MessageBox.Show("Debug mode has been deactivated");
                        }
                        else
                        {
                            _bDebugActivated = true;
                            MessageBox.Show("Debug mode has been activated");
                        }
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Delete:
                            //BEGIN TT#3917-VStuart-Object reference not set to an instance of an object-MID
                            if (_tnLastNode != null)
                            {
                                if (!((MIDTreeNode) _tnLastNode).isChildShortcut)
                                {
                                    DeleteTreeNode();
                                }
                            }
                            break;
                            //END TT#3917-VStuart-Object reference not set to an instance of an object-MID
 
                        case Keys.Up:
                            _bUpArrowPressed = true;
                            //BEGIN TT#3917-VStuart-Object reference not set to an instance of an object-MID
                            if (_tnLastNode != null)
                                //END TT#3917-VStuart-Object reference not set to an instance of an object-MID
                                HandleArrowKey(_tnLastNode.PrevVisibleNode);
                            break;

                        case Keys.Down:
                            _bDownArrowPressed = true;
                            //BEGIN TT#3917-VStuart-Object reference not set to an instance of an object-MID
                            if (_tnLastNode != null)
                                //END TT#3917-VStuart-Object reference not set to an instance of an object-MID
                                HandleArrowKey(_tnLastNode.NextVisibleNode);
                            break;

                        case Keys.Shift:
                            _bShiftKeyPressed = true;
                            break;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        _bUpArrowPressed = false;
                        break;

                    case Keys.Down:
                        _bDownArrowPressed = false;
                        break;

                    case Keys.Shift:
                        _bShiftKeyPressed = false;
                        break;

                    case Keys.Control:
                        _bShiftKeyPressed = false;
                        break;
                }

                if (CurrentState != eDragStates.Idle ||
                    DragEffect != DragDropEffects.None)
                {
                    return;
                }

                // restore to a move effect
                CurrentEffect = DragDropEffects.Move;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                if (_tnMouseDownOnNode != null)
                {
                    base.SelectedNode = _tnMouseDownOnNode;
                }

                OnMIDDoubleClick(this, SelectedNode);
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            int X, Y;
            Point p;
            TreeNodeClipboardList cbList;
            int xPos, yPos;
            int imageHeight, imageWidth;
            bool bControl;
            bool bShift;

            try
            {
                // Get mouse position in client coordinates
                p = this.PointToClient(Control.MousePosition);

                // drag started on other form
                if (!_bDragOperation)
                {
                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                        MIDGraphics.BuildDragImage(cbList, imageListDrag, Indent, _spacing,
                            Font, ForeColor, out imageHeight, out imageWidth);

                        xPos = imageWidth / 2;
                        yPos = imageHeight;

                        // Begin dragging image
                        DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos);

                        if (isAllowedDataType(cbList.ClipboardDataType))
                        {
                            _bDraggingThru = false;
                        }
                        else
                        {
                            _bDraggingThru = true;
                        }
                    }

                    X = p.X + SystemInformation.BorderSize.Width;
                    Y = p.Y + SystemInformation.BorderSize.Height;
                }
                else
                {
                    X = e.X - Left;
                    Y = e.Y - Top;
                }

                DragHelper.ImageList_DragEnter(Handle, X, Y);

                // set CurrentEffect if dragged from other form
                if (CurrentEffect == DragDropEffects.None)
                {
                    bControl = false;
                    bShift = false;

                    if (_bAllowMultiSelect)
                    {
                        if (_bSimulateMultiSelect)
                        {
                            bControl = true;
                        }
                        else
                        {
                            bControl = (ModifierKeys == Keys.Control);
                        }

                        bShift = (ModifierKeys == Keys.Shift);
                    }

                    if (bControl)
                    {
                        CurrentEffect = DragDropEffects.Copy;
                    }
                    else
                    {
                        CurrentEffect = DragDropEffects.Move;
                    }
                }

                e.Effect = CurrentEffect;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            int X, Y;
            Point p;
            Point pt;
            MIDTreeNode treeNode;
			DragDropEffects dropAction;
            int delta;

            try
            {
                // Compute drag position and move image
                p = PointToClient(Control.MousePosition);

                X = p.X + SystemInformation.BorderSize.Width;
                Y = p.Y + SystemInformation.BorderSize.Height;

                pt = PointToClient(new Point(e.X, e.Y));
                treeNode = (MIDTreeNode)GetNodeAt(PointToClient(Cursor.Position));

                if (treeNode == null)
                {
                    DragHelper.ImageList_DragMove(X, Y);
                    e.Effect = DragDropEffects.None;
                    return;
                }

                DragHelper.ImageList_DragMove(X, Y);

                if (_dragOverNode == null || treeNode != _dragOverNode)
                {
                    DragHelper.ImageList_DragShowNolock(false);
                    base.SelectedNode = treeNode;
                    DragHelper.ImageList_DragShowNolock(true);
                    _dragOverNode = treeNode;
                }

                if (CurrentEffect == DragDropEffects.Move)
                {
					dropAction = DragDropEffects.Move;
                }
                else
                {
					dropAction = DragDropEffects.Copy;
                }

                // to handle drag/drop from other form
                if (!treeNode.AllowDrop)
                {
                    e.Effect = DragDropEffects.None;
                }
                else if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    if (isDropAllowed(dropAction, treeNode, (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList))))
                    {
                        e.Effect = CurrentEffect;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (e.Data.GetDataPresent(typeof(ProductCharacteristicClipboardList)))
                {
                    if (isDropAllowed(dropAction, treeNode, (ProductCharacteristicClipboardList)e.Data.GetData(typeof(ProductCharacteristicClipboardList))))
                    {
                        e.Effect = CurrentEffect;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (e.Data.GetDataPresent(typeof(HierarchyNodeClipboardList)))
                {
                    if (isDropAllowed(dropAction, treeNode, (HierarchyNodeClipboardList)e.Data.GetData(typeof(HierarchyNodeClipboardList))))
                    {
                        e.Effect = CurrentEffect;
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else if (isDropAllowed(dropAction, treeNode, _alSelectedNodes))
                {
                    e.Effect = CurrentEffect;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }

				if (e.Effect == DragDropEffects.None)
				{
					_lastValidDragOverNode = null;
				}
				else
				{
					_lastValidDragOverNode = treeNode;
				}

                delta = this.Height - pt.Y;

                if ((delta < this.Height / 2) && (delta > 0))
                {
                    _tnAutoExpandNode = treeNode;

                    if (_tnAutoExpandNode != null && _tnAutoExpandNode.NextVisibleNode != null)
                    {
                        _tnAutoExpandNode.NextVisibleNode.EnsureVisible();
                    }
                }

                if ((delta > this.Height / 2) && (delta < this.Height))
                {
                    _tnAutoExpandNode = treeNode;

                    if (_tnAutoExpandNode != null && _tnAutoExpandNode.PrevVisibleNode != null)
                    {
                        _tnAutoExpandNode.PrevVisibleNode.EnsureVisible();
                    }
                }

                if (_tnAutoExpandNode != null &&
                    _bAllowAutoExpand)
                {
                    if (_tnAutoExpandNode.Nodes.Count > 0 && !_tnAutoExpandNode.IsExpanded)
                    {
                        if (_tnAutoExpandNode.Nodes.Count > 0 && (_tnCurrentAutoExpandNode != _tnAutoExpandNode))
                        {
                            if (!_tnAutoExpandNode.IsExpanded)
                            {
                                TimerStart();
                                _bTimerRunning = true;
                                _bTimerActivated = true;
                            }
                        }
                        else
                        {
                            if (_bTimerActivated && !_bTimerRunning && !_bExpandingNode)
                            {
                                if (_tnAutoExpandNode != null && !_tnAutoExpandNode.IsExpanded)
                                {
                                    _bExpandingNode = true;
                                    _tnAutoExpandNode.Expand();
                                    _bExpandingNode = false;
                                }
                            }
                        }
                    }

                    _tnCurrentAutoExpandNode = _tnAutoExpandNode;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_DragLeave(object sender, System.EventArgs e)
        {
            try
            {
                DragHelper.ImageList_DragLeave(Handle);
                _tnAutoExpandNode = null;
                _bDragOperation = false;

                if (_timer != null)
                {
                    _timer.Stop();
                }

                _bTimerRunning = false;
                _bTimerActivated = false;
                CurrentState = eDragStates.Idle; // assume dropping elsewhere
                RemovePaintFromNodes();   //  reset any highlighted drop nodes
                base.SelectedNode = null;
                DragEffect = DragDropEffects.None;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void MIDTreeView_DragDrop(object sender, DragEventArgs e)
        {
			MIDTreeNode prevParent;
            TreeNodeClipboardList cbList;
			DragDropEffects dropAction;
            string message = null;

            try
            {
				if (_lastValidDragOverNode != null)
				{
                    if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                    {
                        cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

                        //Begin TT#1037 - JSmith - Drag/Drop product characteristic issue
                        if (e.Effect == DragDropEffects.None)
                        {
                            return;
                        }
                        //End TT#1037
						if (_lastValidDragOverNode.GetTopSourceNode() == ((TreeNodeClipboardProfile)cbList.ClipboardItems[0]).Node.GetTopSourceNode())
						{
                            // Begin TT#1744 - JSmith - Alternate Hierarchy
                            //dropAction = e.Effect;
                            if (MakeShortcutNode(_lastValidDragOverNode, cbList.ClipboardItems, eCutCopyOperation.Copy))
                            {
                                dropAction = DragDropEffects.Link;
                            }
                            else
                            {
                                dropAction = e.Effect;
                            }
                            // End TT#1744
						}
						else if (_lastValidDragOverNode.GetTopSourceNode().isMainFavoriteFolder ||
                            // Begin TT#394 - JSmith - Cut and Paste not performing consistently
                            //MakeShortcutNode(_lastValidDragOverNode, cbList.ClipboardItems))
                            MakeShortcutNode(_lastValidDragOverNode, cbList.ClipboardItems, eCutCopyOperation.Copy))
                            // End TT#394
                        {
                            dropAction = DragDropEffects.Link;
                        }
						else
						{
							dropAction = DragDropEffects.Copy;
						}

                        if (dropAction == DragDropEffects.Copy)
                        {
                            if (cbList.ClipboardItems.Count == 1)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNode, false);
                                message = message.Replace("{0}", cbList.ClipboardProfile.Node.Text);
								message = message.Replace("{1}", _lastValidDragOverNode.Text);
                            }
                            else
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmCopyNodes, false);
								message = message.Replace("{0}", _lastValidDragOverNode.Text);
                            }

							if (MIDEnvironment.isWindows && MessageBox.Show(message, "Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								return;
							}
						}
						else if (dropAction == DragDropEffects.Link)
                        {
                            if (cbList.ClipboardItems.Count == 1)
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortCut, false);
                                message = message.Replace("{0}", cbList.ClipboardProfile.Node.Text);
								message = message.Replace("{1}", _lastValidDragOverNode.Text);
                            }
                            else
                            {
                                message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmShortcuts, false);
								message = message.Replace("{0}", _lastValidDragOverNode.Text);
                            }

							if (MIDEnvironment.isWindows && MessageBox.Show(message, "Reference", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								return;
							}
						}

                        try
                        {
                            Cursor.Current = Cursors.WaitCursor;
                            BeginUpdate();

                            foreach (TreeNodeClipboardProfile clipBoardProfile in cbList.ClipboardItems)
                            {
                                if (dropAction == DragDropEffects.Move)
                                {
									prevParent = (MIDTreeNode)clipBoardProfile.Node.Parent;
									MoveNode(clipBoardProfile.Node, _lastValidDragOverNode);
									RefreshShortcuts(_favoritesNode, prevParent);
									RefreshShortcuts(_favoritesNode, _lastValidDragOverNode);
									SortChildNodes(_lastValidDragOverNode);
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									_lastValidDragOverNode.UpdateExternalText();
									prevParent.UpdateExternalText();
									//End Track #6201 - JScott - Store Count removed from attr sets
									if (prevParent.Nodes.Count == 0)
                                    {
                                        prevParent.SetCollapseImage();
                                    }
								}
                                else if (dropAction == DragDropEffects.Copy)
                                {
									CopyNode(clipBoardProfile.Node, _lastValidDragOverNode, true);
									RefreshShortcuts(_favoritesNode, _lastValidDragOverNode);
									SortChildNodes(_lastValidDragOverNode);
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									_lastValidDragOverNode.UpdateExternalText();
									//End Track #6201 - JScott - Store Count removed from attr sets
								}
								else if (dropAction == DragDropEffects.Link)
								{
									CreateShortcut(clipBoardProfile.Node, _lastValidDragOverNode);
									SortChildNodes(_lastValidDragOverNode);
									//Begin Track #6201 - JScott - Store Count removed from attr sets
									_lastValidDragOverNode.UpdateExternalText();
									//End Track #6201 - JScott - Store Count removed from attr sets
								}
							}
                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                            EndUpdate();
                        }
                    }
                    else
                    {
                        CustomDragDrop(sender, e);
                    }

					this.SelectedNode = _lastValidDragOverNode;
				}

                this.CurrentEffect = DragDropEffects.Move;
                this.DragEffect = DragDropEffects.None;
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            finally
            {
                this.Invalidate();
                Cursor.Current = Cursors.Default;
            }
        }

        private void MIDTreeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
        {
            bool bControl;
            bool bShift;
            MIDTreeNode selectedNode;
            ClipboardListBase cbList;
            int xPos, yPos;
            int imageHeight, imageWidth;
            Point p;

            try
            {
                bControl = false;
                bShift = false;
                _bDragOperation = true;

                if (_bAllowMultiSelect)
                {
                    if (_bSimulateMultiSelect)
                    {
                        bControl = true;
                    }
                    else
                    {
                        bControl = (ModifierKeys == Keys.Control);
                    }

                    bShift = (ModifierKeys == Keys.Shift);
                }

                if (_tnMouseDownOnNode != null)
                {
                    if (!bControl && !bShift)
                    {
                        // set the SelectedNode to null first incase node is currently selected in the tree view
                        base.SelectedNode = null;
                        base.SelectedNode = _tnMouseDownOnNode;
                    }
                }

				if (_alSelectedNodes.Count == 0 ||
                    e.Item == null ||
                    !((MIDTreeNode)e.Item).isDragAllowed(CurrentEffect))
                    //!((MIDTreeNode)e.Item).isDragAllowed(CurrentEffect) ||
                    //(((MIDTreeNode)e.Item).FunctionSecurityProfile != null && ((MIDTreeNode)e.Item).FunctionSecurityProfile.AccessDenied))
                {
                    _bDragOperation = false;
                    return;
                }

                if (_currentEffect == DragDropEffects.Move)
                {
                    _currentState = eDragStates.Move;
                }
                else
                {
                    _currentState = eDragStates.Copy;
                }

				selectedNode = (MIDTreeNode)_alSelectedNodes[0];
				cbList = BuildClipboardList(_alSelectedNodes, DragDropEffects.Move);
                MIDGraphics.BuildDragImage(cbList, imageListDrag, Indent, _spacing,
                            Font, ForeColor, out imageHeight, out imageWidth);

                // Get mouse position in client coordinates
                p = this.PointToClient(Control.MousePosition);

                // Compute delta between mouse position and node bounds
                xPos = p.X + this.Indent - selectedNode.Bounds.Left;
                yPos = p.Y - selectedNode.Bounds.Top;

                xPos = imageWidth / 2;
                yPos = imageHeight / 2;

                // Begin dragging image
                if (DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos))
                {
					_lastValidDragOverNode = null;
                    RemovePaintFromNodes();
                    // Begin dragging
                    DoDragDrop(cbList, DragDropEffects.All);
                    // End dragging image
                    DragHelper.ImageList_EndDrag();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

		/// <summary>
		/// Starts the autoexpand timer with the default interval
		/// </summary>

		private void TimerStart()
		{
			try
			{
				TimerStart(_iTimerInterval);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Starts the autoexpand timer with a provided interval
		/// </summary>
		/// <param name="aTimerInterval"></param>

		private void TimerStart(int aTimerInterval)
		{
			try
			{
				if (_timer == null)
				{
					_timer = new System.Timers.Timer();
					_iTimerInterval = aTimerInterval;
					_timer.Elapsed += new ElapsedEventHandler(OnTimer);
					_timer.Interval = _iTimerInterval;
					_timer.Stop();
				}

				if (_bTimerRunning)
				{
					_timer.Stop();
				}

				_timer.Start();
				_bTimerRunning = true;
				_bTimerActivated = true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Stops the autoexpand timer.
		/// </summary>

		private void TimerStop()
		{
			try
			{
				if (_timer != null &&
					_bTimerRunning)
				{
					_timer.Stop();
				}

				_bTimerRunning = false;
				_bTimerActivated = false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Resets the autoexpand timer
		/// </summary>

		private void TimerReset()
		{
			try
			{
				TimerStop();
				TimerStart();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private void OnTimer(Object source, ElapsedEventArgs e)
        {
            try
            {
                _timer.Stop();
                _bTimerRunning = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void HandleArrowKey(TreeNode aNextNode)
        {
            bool bControl;
            bool bShift;

            try
            {
                bControl = false;
                bShift = false;

                if (_bAllowMultiSelect)
                {
                    if (_bSimulateMultiSelect)
                    {
                        bControl = true;
                    }
                    else
                    {
                        bControl = (ModifierKeys == Keys.Control);
                    }

                    bShift = (ModifierKeys == Keys.Shift);
                }

                if (aNextNode != null)
                {
                    if (bShift || bControl)
                    {
                        if (aNextNode.Parent != _tnLastNode.Parent)
                        {
                            return;
                        }

                        _tnFirstNode = _tnArrowNode;
                        _tnLastNode = aNextNode;
                        RebuildSelectedNodes(_tnFirstNode, _tnLastNode);

                        if (bControl)
                        {
                            base.SelectedNode = aNextNode;
                        }
                    }
                    else
                    {
                        _tnArrowNode = aNextNode;
                        _tnFirstNode = aNextNode;
                        _tnLastNode = aNextNode;
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void RebuildSelectedNodes(TreeNode aFirstNode, TreeNode aLastNode)
        {
            int firstIndex, lastIndex;
            ArrayList temp;

            try
            {
                if (_tnFirstNode.Index < _tnLastNode.Index)
                {
                    firstIndex = _tnFirstNode.Index;
                    lastIndex = _tnLastNode.Index;
                }
                else
                {
                    firstIndex = _tnLastNode.Index;
                    lastIndex = _tnFirstNode.Index;
                }

                temp = new ArrayList();

                foreach (MIDTreeNode tn in _alSelectedNodes)
                {
                    if (tn.Index < firstIndex ||
                        tn.Index > lastIndex)
                    {
                        RemovePaintFromNode(tn);
                    }
                    else
                    {
                        temp.Add(tn);
                    }
                }

                _alSelectedNodes.Clear();

                foreach (MIDTreeNode tn in temp)
                {
                    _alSelectedNodes.Add(tn);
                    if (OnMIDNodeSelect != null)
                    {
                        OnMIDNodeSelect(this, (MIDTreeNode)tn);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private TreeNodeClipboardProfile BuildClipboardProfile(MIDTreeNode aNode, DragDropEffects aAction)
		{
			try
			{
				TreeNodeClipboardProfile cbp = new TreeNodeClipboardProfile(aNode);

				cbp.DragImage = this.ImageList.Images[aNode.SelectedImageIndex];
				cbp.DragImageHeight = aNode.Bounds.Height;
				cbp.DragImageWidth = aNode.Bounds.Width;
				cbp.Action = aAction;

				return cbp;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void PaintNode(MIDTreeNode aTreeNode)
        {
            try
            {
                aTreeNode.BackColor = SystemColors.Highlight;

                if (aTreeNode.NodeChangeType == eChangeType.delete)
                {
                    aTreeNode.ForeColor = SystemColors.InactiveCaption;
                }
                else
                {
                    aTreeNode.ForeColor = SystemColors.HighlightText;
                }
                // Begin Track #6295 - JSmith - Try to attach sku to added header 
                _alPaintedNodes.Add(aTreeNode);
                // End Track #6295
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void RecursiveRemoveDeletedNodes(MIDTreeNode aTreeNode, MIDTreeNode aParentNode)
        {
            try
            {
                if (aParentNode != null &&
                    aTreeNode.NodeChangeType == eChangeType.delete)
                {
                    aParentNode.Nodes.Remove(aTreeNode);
                }
                else
                {
                    foreach (MIDTreeNode tn in aTreeNode.Nodes)
                    {
                        // Begin Track #4977 - BVaughan - Deleting Product Characteristic gives Null Reference Exception
                        //RecursiveRemoveDeletedNodes(tn, aTreeNode);
                        if (tn != null)
                        {
                            RecursiveRemoveDeletedNodes(tn, aTreeNode);
                        }
                        // End Track #4977 
                    }
                }
            }
            catch
            {
                throw;
            }
        }

		//private void RefreshShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
		//{
		//    try
		//    {
		//        foreach (MIDTreeNode node in aStartNode.Nodes)
		//        {
		//            if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
		//            {
		//                node.RefreshShortcutNode(aChangedNode);
		//            }
		//            else if (node.isSubFolder || node.isRootShortcut ||
		//                (node.isChildShortcut && node.isSubFolder))
		//            {
		//                RefreshShortcuts(node, aChangedNode);
		//            }
		//        }
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		//private void AddNodeToSortedList(SortedList aSortedList, TreeNode aNode, object aValue)
		//{
		//    string text;

		//    try
		//    {
		//        text = aNode.Text;

		//        while (aSortedList.Contains(text))
		//        {
		//            text += "_";
		//        }

		//        aSortedList.Add(text, aValue);
		//    }
		//    catch (Exception exc)
		//    {
		//        string message = exc.ToString();
		//        throw;
		//    }
		//}

		private void RemovePaintFromNode(MIDTreeNode aTreeNode)
		{
			try
			{
				if (aTreeNode != null && aTreeNode.TreeView != null)
				{
					aTreeNode.BackColor = aTreeNode.TreeView.BackColor;

                    // Begin Track #6202 - JSmith - select not allowed
                    //if (aTreeNode.NodeChangeType == eChangeType.delete ||
                        //(aTreeNode.FunctionSecurityProfile != null && aTreeNode.FunctionSecurityProfile.AccessDenied))
					if (aTreeNode.NodeChangeType == eChangeType.delete ||
                        !aTreeNode.isAccessAllowed)
                    // End Track #6202
					{
						aTreeNode.ForeColor = SystemColors.InactiveCaption;
					}
					else
					{
						aTreeNode.ForeColor = aTreeNode.TreeView.ForeColor;
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void RemovePaintFromSelectedNodes()
		{
			try
			{
				if (_alSelectedNodes.Count == 0)
				{
					return;
				}

				foreach (MIDTreeNode tn in _alSelectedNodes)
				{
					RemovePaintFromNode(tn);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void RemovePaintFromNodes(TreeNode aTreeNode)
		{
			try
			{
				// Reset each node recursively.
				foreach (MIDTreeNode tn in aTreeNode.Nodes)
				{
					if (tn.Nodes.Count > 0)
					{
						RemovePaintFromNode(tn);
						RemovePaintFromNodes(tn);
					}
					else
					{
						RemovePaintFromNode(tn);
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
		/// Retrieves a FolderProfile object for the key.
		/// </summary>
		/// <param name="aFolderRID">The key of the folder</param>
		/// <returns>
		/// A FolderProfile object
		/// </returns>
		/// <remarks>
		/// The key will equal Include.NoRID if the folder does not exist
		/// </remarks>

		private FolderProfile Folder_Get(int aFolderRID)
		{
			DataTable dtFolders;
			FolderProfile folderProf;

			try
			{
				dtFolders = DlFolder.Folder_Read(aFolderRID);
				if (dtFolders == null || dtFolders.Rows.Count != 1)
				{
					folderProf = new FolderProfile(Include.NoRID);
				}
				else
				{
					folderProf = new FolderProfile(dtFolders.Rows[0]);
				}
			}
			catch
			{
				throw;
			}

			return folderProf;
		}

		private void HandleException(Exception exc)
        {
            _SAB.ClientServerSession.Audit.Log_Exception(exc, this.Name, eExceptionLogging.logAllInnerExceptions);
            if (MIDEnvironment.isWindows)
            {
                MessageBox.Show(exc.ToString(), "MIDFormBase.cs - " + this.Name);
            }
            else // rethrow if not in Windows environment.
            {
                throw exc;
            }
        }

        private void HandleException(MIDException MIDexc)
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
            if (MIDEnvironment.isWindows)
            {
                MessageBox.Show(this, Msg, Title,
                    buttons, icon);
            }
            else // rethrow if not in Windows environment.
            {
                throw MIDexc;
            }
        }
	}
}