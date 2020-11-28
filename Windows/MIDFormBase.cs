using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;   //TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Shared;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for MIDFormBase.
	/// </summary>
	public class MIDFormBase : System.Windows.Forms.Form, IFormBase
	{
        //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
        private const int WM_SETREDRAW = 0xb;
        [DllImport("user32.dll")]

        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, bool wParam, IntPtr lParam);
        //End TT#866

       private System.ComponentModel.IContainer components;

		
		#region Fields
		//=======
		// FIELDS
		//=======
        protected SessionAddressBlock _SAB;   //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
		private ClientSessionTransaction _clientTransaction = null;
		private ApplicationSessionTransaction _applicationTransaction = null;
		private bool _exceptionCaught = false;

        private bool _cancelFormClosing = false; // TT#1639 - RMatelic - Error message on Header with Piggybacking Workflow
		private bool _formLoadError = false;
		private bool _errorFound = false;
        private bool _changePending = false;
		private bool _formLoaded = false;
		private bool _formIsClosing = false;
		private bool _saveOnClose = false;
		private bool _checkChanged = false;
		private bool _disposeControlsOnClose = true;
        private bool _disposeChildControlsOnClose = true; //TT#609-MD -jsobek -OTS Forecast Chain Ladder View
		private bool _displayForm = true;
		internal bool _previouslyActivated = false;
		private bool _doValueByValueEdits = false;
		private bool _ignoreWindowState = false;
		private bool _setAutoScrollMinSize = true;
        // Begin TT#1580 - JSmith - Window Closes before Save Yes, No or Cancel can be selected
        private bool _bypassBaseClosingLogic = false;
        // End TT#1580
        // Begin TT#272-MD - JSmith - Version 5.0 - General screen cleanup 
        private bool _keyEventsAssigned = false;
        // End TT#272-MD - JSmith - Version 5.0 - General screen cleanup
        // Begin TT#2598 - JSmith - Save As button does not work
        private bool _isActivated = false;
        // End TT#2598 - JSmith - Save As button does not work
		private DialogResult _resultSaveChanges = DialogResult.None;
		private int _eventCount = 0;
		private System.Windows.Forms.ToolTip toolTip1;
		// BEGIN TT#1712 - stodd - add tool tip to disabled controls
		private MIDEnhancedToolTip _enhancedToolTip;
		// END TT#1712 - stodd - add tool tip to disabled controls
		private string _message;
		private FunctionSecurityProfile _functionSecurity;
		private bool _allowUpdate;
		private Image _calendarImage = null;
		private Image _storeImage = null;
		private Image _errorImage = null;
        private Image _warningImage = null;
		private Image _reoccurringImage = null;
		private Image _inheritanceImage = null;
		private Image _userImage = null;
		private Image _globalImage = null;
		private Image _dynamicToPlanImage = null;
		private Image _dynamicToCurrentImage = null;
		private Image _dynamicSwitchImage = null;
		private string _graphicsDir;
		private System.Windows.Forms.ErrorProvider _errorProvider;
		private System.Windows.Forms.ErrorProvider _inheritanceProvider;
		private System.Windows.Forms.TextBox txtGetReadOnlyColor;
		private string[] _computationModes;
        private Hashtable _menuItems = new Hashtable();

		private DataTable _dtStoreFilters = null;
		private ArrayList _myControls = null;
		private ArrayList _myComponents = null;

        private int _spacing = 2;
        private int _imageHeight;
        private int _imageWidth;

        protected Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;
        private UltraToolbarsDockArea _MIDFormBase_Toolbars_Dock_Area_Left;
        private UltraToolbarsDockArea _MIDFormBase_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _MIDFormBase_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _MIDFormBase_Toolbars_Dock_Area_Bottom;
        private PopupMenuTool _fileMenuTool;
		private PopupMenuTool _editMenuTool;
        private PopupMenuTool _toolsMenuTool;
        private ImageList imageListDrag;

		private FunctionSecurityProfile _userOverrideLowLevelModelSecurity = null;
		private FunctionSecurityProfile _globalOverrideLowLevelModelSecurity = null;
		// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		private bool _cancelClicked = false;
		// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		private bool _okClicked = false;	// TT#508-MD - stodd -  Correct Assortment Properties Enqueue

        //BEGIN TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
        private ArrayList _ridList;
        protected bool _isInQuiry = true;
        //END   TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
		private bool _showToolTip = false;		// TT#742-MD - Stodd - Assortment tooltips

        #endregion Fields

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		// Base constructor required for Windows Form Designer support
		public MIDFormBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public MIDFormBase(SessionAddressBlock aSAB)
		{
			if (aSAB == null)
			{
				throw new Exception("SessionAddressBlock is required");
			}
			_SAB = aSAB;
			_allowUpdate = false;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_myControls = new ArrayList();
			_myComponents = new ArrayList();

			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form_Closing);
			this.Load += new System.EventHandler(this.Form_Load);
			this.Activated +=new EventHandler(this.Form_Activated);
            this.Deactivate += new EventHandler(MIDFormBase_Deactivate); // TT#2598 - JSmith - Save As button does not work
			this.DragEnter += new DragEventHandler(Image_DragEnter);
			this.DragOver += new DragEventHandler(Image_DragOver);
			this.DragLeave += new EventHandler(MIDFormBase_DragLeave);

			// Set up the delays for the ToolTip.
			toolTip1.AutoPopDelay = Include.toolTipAutoPopDelay;
			toolTip1.InitialDelay = Include.toolTipInitialDelay;
			toolTip1.ReshowDelay = Include.toolTipReshowDelay;
			// Force the ToolTip text to be displayed whether or not the form is active.
			toolTip1.ShowAlways = true;

            // Begin TT#588-MD - JSmith - Remove ApplicationRoot from configuration
            //_graphicsDir = MIDConfigurationManager.AppSettings["ApplicationRoot"] + MIDGraphics.GraphicsDir;
            _graphicsDir = MIDGraphics.MIDGraphicsDir;
            // End TT#588-MD - JSmith - Remove ApplicationRoot from configuration
			string calendarFileName = _graphicsDir + "\\" + MIDGraphics.CalendarImage;
			try
			{
				_calendarImage = Image.FromFile(calendarFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string storeFileName = _graphicsDir + "\\" + MIDGraphics.StoreImage;
			try
			{
				_storeImage = Image.FromFile(storeFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string errorFileName = _graphicsDir + "\\" + MIDGraphics.ErrorImage;
			try
			{
				_errorImage = Image.FromFile(errorFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

            string warningFileName = _graphicsDir + "\\" + MIDGraphics.WarningImage;
            try
            {
                _warningImage = Image.FromFile(warningFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string reoccurringFileName = _graphicsDir + "\\" + MIDGraphics.ReoccurringImage;
			try
			{
				_reoccurringImage = Image.FromFile(reoccurringFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string inheritanceFileName = _graphicsDir + "\\" + MIDGraphics.InheritanceImage;
			try
			{
				_inheritanceImage = Image.FromFile(inheritanceFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string userFileName = _graphicsDir + "\\" + MIDGraphics.SecUserImage;
			try
			{
				_userImage = Image.FromFile(userFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string globalFileName = _graphicsDir + "\\" + MIDGraphics.GlobalImage;
			try
			{
				_globalImage = Image.FromFile(globalFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			string dynamicFileName = _graphicsDir + "\\" + MIDGraphics.DynamicToPlanImage;
			try
			{
				_dynamicToPlanImage = Image.FromFile(dynamicFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			dynamicFileName = _graphicsDir + "\\" + MIDGraphics.DynamicToCurrentImage;
			try
			{
				_dynamicToCurrentImage = Image.FromFile(dynamicFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			dynamicFileName = _graphicsDir + "\\" + MIDGraphics.DynamicSwitchImage;
			try
			{
				_dynamicSwitchImage = Image.FromFile(dynamicFileName);
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}

			_inheritanceProvider.Icon = new System.Drawing.Icon(GraphicsDirectory + "\\" + MIDGraphics.InheritanceImage);

            _fileMenuTool = new PopupMenuTool(Include.menuFile);
            _fileMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
            _fileMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
            utmMain.Tools.Add(_fileMenuTool);

            _editMenuTool = new PopupMenuTool(Include.menuEdit);
            _editMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
            _editMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
            utmMain.Tools.Add(_editMenuTool);

            _toolsMenuTool = new PopupMenuTool(Include.menuTools);
            _toolsMenuTool.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools);
            _toolsMenuTool.Settings.IsSideStripVisible = DefaultableBoolean.True;
            utmMain.Tools.Add(_toolsMenuTool);

            utmMain.ImageListSmall = MIDGraphics.ImageList;
            utmMain.ImageListLarge = MIDGraphics.ImageList;
            BuildFileMenu();
            BuildEditMenu();
            Icon = new System.Drawing.Icon(MIDGraphics.ImageDir + "\\" + MIDGraphics.ApplicationIcon);

			_userOverrideLowLevelModelSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
			_globalOverrideLowLevelModelSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);

		}

        void MIDFormBase_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}
				
		public void Image_DragEnter(object sender, DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			try
			{
				int X, Y;

				// Get mouse position in client coordinates
				Point p = PointToClient(Control.MousePosition);

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
					int xPos, yPos;
                    MIDGraphics.BuildDragImage(cbList, imageListDrag, 0, _spacing,
								Font, ForeColor, out _imageHeight, out _imageWidth);

					xPos = _imageWidth / 2;
					yPos = _imageHeight / 2;

					// Begin dragging image
					DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos);
				}
				else if (e.Data.GetDataPresent(typeof(FilterDragObject)))
				{
					FilterDragObject fdo = (FilterDragObject)e.Data.GetData(typeof(FilterDragObject));
					if (fdo.Text != null)
					{
						int xPos, yPos;
						Font f = new Font(Font.FontFamily, (float)(Font.Size * .75));
						//MIDGraphics.BuildDragImage(fdo.Text, imageListDrag, 0, _spacing,
						//            Font, ForeColor, out _imageHeight, out _imageWidth);
						MIDGraphics.BuildDragImage(fdo.Text, imageListDrag, 0, _spacing,
									f, ForeColor, out _imageHeight, out _imageWidth);

						xPos = _imageWidth / 2;
						yPos = _imageHeight / 2;

						// Begin dragging image
						DragHelper.ImageList_BeginDrag(this.imageListDrag.Handle, 0, xPos, yPos);
					}
				}

				// calculate image position based on upper left coordinate of window, not the client area
				X = p.X + SystemInformation.BorderSize.Width;
				Y = p.Y + SystemInformation.CaptionHeight;
				if (Menu != null && Menu.MenuItems.Count > 0)
				{
					Y += SystemInformation.MenuHeight;
				}

				DragHelper.ImageList_DragEnter(Handle, X, Y);
			}
			catch
			{
				throw;
			}
		}

		public void Image_DragOver(object sender, DragEventArgs e)
		{
			try
			{
				int X, Y;
				DragHelper.ImageList_DragLeave(Handle);

				// Get mouse position in client coordinates
				Point p = PointToClient(Control.MousePosition);

				// calculate image position based on upper left coordinate of window, not the client area
				X = p.X + SystemInformation.BorderSize.Width;
				Y = p.Y + SystemInformation.CaptionHeight;
				if (Menu != null && Menu.MenuItems.Count > 0)
				{
					Y += SystemInformation.MenuHeight;
				}

				DragHelper.ImageList_DragMove(X, Y);

				DragHelper.ImageList_DragEnter(Handle, X, Y);
			}
			catch
			{
				throw;
			}
		}

		public void Image_DragLeave(object sender, EventArgs e)
		{
			try
			{
				DragHelper.ImageList_DragLeave(Handle);
			}
			catch
			{
				throw;
			}
		}

		public void Image_EndDrag()
		{
			try
			{
				DragHelper.ImageList_EndDrag();
			}
			catch
			{
				throw;
			}
		}

		#endregion Constructors

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.Closing -= new System.ComponentModel.CancelEventHandler(this.Form_Closing);
				this.Load -= new System.EventHandler(this.Form_Load);
				this.Activated -=new EventHandler(this.Form_Activated);
                this.Deactivate -= new EventHandler(MIDFormBase_Deactivate);  // TT#2598 - JSmith - Save As button does not work

                // Begin TT#856 - JSmith - Out of memory
                this.DragEnter -= new DragEventHandler(Image_DragEnter);
                this.DragOver -= new DragEventHandler(Image_DragOver);
                this.DragLeave -= new EventHandler(MIDFormBase_DragLeave);
				// End TT#856

                // Begin TT#890-Header - RMatelic - Adding a header, select Bulk / Blank Row, get System Null Reference - move code down under 'if ' statement below
                //this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
                // End TT#890
				if (_disposeControlsOnClose)
				{
					if (_errorProvider != null)
					{
					    // Begin TT#856 - JSmith - Out of memory
                        _errorProvider.Clear();
						// End TT#856
						_errorProvider.Dispose();
						_errorProvider = null;
					}
					if (_inheritanceProvider != null)
					{
					    // Begin TT#856 - JSmith - Out of memory
                        _inheritanceProvider.Clear();
						// End TT#856
						_inheritanceProvider.Dispose();
						_inheritanceProvider = null;
					}
					if (txtGetReadOnlyColor != null)
					{
						txtGetReadOnlyColor.Dispose();
						txtGetReadOnlyColor = null;
					}
					if (toolTip1 != null)
					{
                        toolTip1.RemoveAll();
						toolTip1.Dispose();
						toolTip1 = null;
					}

					// BEGIN TT#1712 - stodd - add tool tip to disabled controls
					if (_enhancedToolTip != null)
					{
						_enhancedToolTip.RemoveAll();
						_enhancedToolTip.Dispose();
						_enhancedToolTip = null;
					}
					// END TT#1712 - stodd - add tool tip to disabled controls

                    // Begin TT#856 - JSmith - Out of memory
                    if (utmMain != null)
                    {
                        utmMain.DockWithinContainer = null;
                        _MIDFormBase_Toolbars_Dock_Area_Left.ToolbarsManager = null;
                        _MIDFormBase_Toolbars_Dock_Area_Top.ToolbarsManager = null;
                        _MIDFormBase_Toolbars_Dock_Area_Bottom.ToolbarsManager = null;
                        _MIDFormBase_Toolbars_Dock_Area_Left = null;
                        _MIDFormBase_Toolbars_Dock_Area_Top = null;
                        _MIDFormBase_Toolbars_Dock_Area_Bottom = null;
                        // Begin TT#890-Header - RMatelic - Adding a header, select Bulk / Blank Row, get System Null Reference - move code down under 'if ' statement below
                        this.utmMain.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
                        // End TT#890
                        utmMain.Tools.Clear();
                        utmMain.Dispose();
                        utmMain = null;
                    }
					// End TT#856

                    //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
                    if (_disposeChildControlsOnClose)
                    {
                        ClearControlsForDisposal();
                    }
                    //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

					if (_myControls != null)
					{
						foreach (Control control in _myControls)
						{
							if (control != null && !control.IsDisposed)
							{
								try
								{
									Include.DisposeControl(control);
								}
								catch
								{
								}
							}
						}
					}

					if (_myComponents != null)
					{
						foreach (Component component in _myComponents)
						{
							if (component != null)
							{
								try
								{
									component.Dispose();
								}
								catch
								{
								}
							}
						}
					}

					Include.DisposeControls(this.Controls);

                    // Begin TT#856 - JSmith - Out of memory
                    this.Controls.Clear();
					// End TT#856
				}
			}
			base.Dispose( disposing );
		}

        protected void ClearControlsForDisposal()
        {
            try
            {
                foreach (Control ctrl in this.Controls)
                {
                    ClearControlForDisposal(ctrl);
                }
            }
            catch
            {
            }
        }

        private void ClearControlForDisposal(Control aCtrl)
        {
            try
            {
                aCtrl.Tag = null;
                aCtrl.PerformLayout();

                if (aCtrl.Controls.Count > 0)
                {
                    if (aCtrl is System.Windows.Forms.SplitContainer)
                    {
                        foreach (Control subCtrl in ((System.Windows.Forms.SplitContainer)aCtrl).Panel1.Controls)
                        {
                            ClearControlForDisposal(subCtrl);
                        }
                        foreach (Control subCtrl in ((System.Windows.Forms.SplitContainer)aCtrl).Panel2.Controls)
                        {
                            ClearControlForDisposal(subCtrl);
                        }
                    }
                    else
                    {
                        foreach (Control subCtrl in aCtrl.Controls)
                        {
                            ClearControlForDisposal(subCtrl);
                        }
                    }
                }

                if (!aCtrl.Disposing)
                {
					// Begin TT#1386-MD - stodd - added Schedulet Job Manager
                    try
                    {
                        aCtrl.Dispose();
                    }
                    catch (InvalidOperationException e)
                    {
                        // Added because UltraPanel Controls collection is read-only and cannot be disposed this way.
                        // swallow error
                    }
					// End TT#1386-MD - stodd - added Schedulet Job Manager
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

		#region Properties
		//============
		// PROPERTIES
		//============

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
		/// Gets the Client Transaction object.
		/// </summary>
		protected ClientSessionTransaction ClientTransaction
		{
			get	
			{
				if (_clientTransaction == null)
				{
					_clientTransaction = _SAB.ClientServerSession.CreateTransaction();
				}
				return _clientTransaction;
			}
		}

		/// <summary>
		/// Gets the Application Transaction object.
		/// </summary>
		protected ApplicationSessionTransaction ApplicationTransaction
		{
			get	
			{
				if (_applicationTransaction == null)
				{
					_applicationTransaction = _SAB.ApplicationServerSession.CreateTransaction();
				}
				return _applicationTransaction;
			}
		}

		public bool ExceptionCaught
		{
			get {return this._exceptionCaught;}
		}
		/// <summary>
		/// Gets or sets a Dialog Result if changes are saved or cancelled.
		/// </summary>
		public DialogResult ResultSaveChanges
		{
			get {return _resultSaveChanges;}
			set {_resultSaveChanges = value;}
		}


		/// <summary>
		/// Gets or sets a boolean identifying if the form load event has completed.
		/// </summary>
		public bool FormLoaded
		{
			get	{return _formLoaded;}
			set	{_formLoaded = value;}
		}

		/// <summary>
		/// Gets or sets a boolean identifying if the controls owned by this form should be disposed on close.
		/// </summary>
		public bool DisposeControlsOnClose
		{
			get	{return _disposeControlsOnClose;}
			set	{_disposeControlsOnClose = value;}
		}
        //Begin TT#609-MD -jsobek - OTS Forecast Chain Ladder View
        /// <summary>
        /// Gets or sets a boolean identifying if the child controls owned by this form should be disposed on close.
        /// </summary>
        public bool DisposeChildControlsOnClose
        {
            get { return _disposeChildControlsOnClose; }
            set { _disposeChildControlsOnClose = value; }
        }
        //End TT#609-MD -jsobek - OTS Forecast Chain Ladder View

		/// <summary>
		/// Gets or sets a boolean identifying if the form should be displayed.
		/// </summary>
		public bool DisplayForm
		{
			get	{return _displayForm;}
			set	{_displayForm = value;}
		}

		/// <summary>
		/// Gets a boolean identifying if the form is closing.
		/// </summary>
		public bool FormIsClosing
		{
			get	{return _formIsClosing;}
		}

		/// <summary>
		/// Gets a boolean identifying if a save was requested during the close.
		/// </summary>
		public bool SaveOnClose
		{
			get	{return _saveOnClose;}
		}

		/// <summary>
		/// Gets a boolean identifying if an error was encountered during the form load.
		/// </summary>
		public bool FormLoadError
		{
			get	{return _formLoadError;}
			set	{_formLoadError = value;}
		}
		
		/// <summary>
		/// Gets a boolean identifying if an error was encountered during processing.
		/// </summary>
		public bool ErrorFound
		{
			get	{return _errorFound;}
			set	{_errorFound = value;}
		}

        /// <summary>
		/// Gets or sets a flag identifying if values should be editted after each change or by form.
		/// </summary>
		public bool DoValueByValueEdits
		{
			get	{return _doValueByValueEdits;}
			set	{_doValueByValueEdits = value;}
		}

		/// <summary>
		/// Gets or sets a flag identifying if the AutoScrollMinSize of the form should be set on activate.
		/// </summary>
		/// <value>Default value is true</value>
		public bool SetAutoScrollMinSize
		{
			get	{return _setAutoScrollMinSize;}
			set	{_setAutoScrollMinSize = value;}
		}

        // Begin TT#1580 - JSmith - Window Closes before Save Yes, No or Cancel can be selected
        /// <summary>
        /// Gets or sets a flag identifying if the closing logic in the base for should be bypassed.
        /// </summary>
        /// <value>Default value is true</value>
        public bool BypassBaseClosingLogic
        {
            get { return _bypassBaseClosingLogic; }
            set { _bypassBaseClosingLogic = value; }
        }
        // End TT#1580

		/// <summary>
		/// Gets a boolean identifying if there is a pending change to the data on the form.
		/// </summary>
		public virtual bool ChangePending
		{
			get	{return _changePending;}
			set	
			{
				char[] changeChar = {'*'};
				if (FormLoaded)
				{
					if (_allowUpdate)
					{
						_changePending = value;
						this.Text = this.Text.TrimEnd(changeChar);
						if (_changePending)
						{
							this.Text = this.Text + changeChar[0].ToString(CultureInfo.CurrentUICulture);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets a message.
		/// </summary>
		public string Message
		{
			get	{return _message;}
			set	{_message = value;}
		}

		/// <summary>
		/// Gets or sets the function security profile
		/// </summary>
		public FunctionSecurityProfile FunctionSecurity
		{
			get	{ return _functionSecurity;}
			set	{_functionSecurity = value;}
		}

		/// <summary>
		/// Gets or sets the flag identifying if the user can update the displayed data.
		/// </summary>
		/// <remarks>If not set, update is not allowed.</remarks>
		public bool AllowUpdate
		{
			get	{ return _allowUpdate;}
//			set	{_allowUpdate = value;}            Security changes - 1/24/2005 vg
		}

		/// <summary>
		/// Gets the graphics directory.
		/// </summary>
		public string GraphicsDirectory
		{
			get	{return _graphicsDir;}
		}

		/// <summary>
		/// Gets the calendar image.
		/// </summary>
		public Image CalendarImage
		{
			get	{return _calendarImage;}
		}

		/// <summary>
		/// Gets the store image.
		/// </summary>
		public Image StoreImage
		{
			get	{return _storeImage;}
		}

		/// <summary>
		/// Gets the error image.
		/// </summary>
		public Image ErrorImage
		{
			get	{return _errorImage;}
		}

        /// <summary>
        /// Gets the error image.
        /// </summary>
        public Image WarningImage
        {
            get { return _warningImage; }
        }

		/// <summary>
		/// Gets the reoccurring image.
		/// </summary>
		public Image ReoccurringImage
		{
			get	{return _reoccurringImage;}
		}

		/// <summary>
		/// Gets the dynamic to plan image.
		/// </summary>
		public Image DynamicToPlanImage
		{
			get	
			{
				return _dynamicToPlanImage;
			}
		}

		/// <summary>
		/// Gets the dynamic to current image.
		/// </summary>
		public Image DynamicToCurrentImage
		{
			get	
			{
				return _dynamicToCurrentImage;
			}
		}

		/// <summary>
		/// Gets the dynamic switch date image.
		/// </summary>
		public Image DynamicSwitchImage
		{
			get	
			{
				return _dynamicSwitchImage;
			}
		}

		/// <summary>
		/// Gets the calendar image.
		/// </summary>
		public Image InheritanceImage
		{
			get	{return _inheritanceImage;}
		}

		/// <summary>
		/// Gets the user image.
		/// </summary>
		public Image UserImage
		{
			get	{return _userImage;}
		}

		/// <summary>
		/// Gets the global image.
		/// </summary>
		public Image GlobalImage
		{
			get	{return _globalImage;}
		}

		/// <summary>
		/// Gets the inheritanceProvider object.
		/// </summary>
		public System.Windows.Forms.ErrorProvider ErrorProvider
		{
			get	{return _errorProvider;}
		}

		/// <summary>
		/// Gets the inheritanceProvider object.
		/// </summary>
		public System.Windows.Forms.ErrorProvider InheritanceProvider
		{
			get	{return _inheritanceProvider;}
		}

		/// <summary>
		/// Gets the tooltip object
		/// </summary>
		public System.Windows.Forms.ToolTip ToolTip
		{
			get	{return toolTip1;}
		}

		// BEGIN TT#1712 - stodd - add tool tip to disabled controls
		/// <summary>
		/// Gets the tooltip object
		/// </summary>
		public MIDEnhancedToolTip EnhancedToolTip
		{
			get { return _enhancedToolTip; }
		}
		// END TT#1712 - stodd - add tool tip to disabled controls
		// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		public bool CancelClicked
		{
			get { return _cancelClicked; }
		}
		// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		public bool OkClicked
		{
			get { return _okClicked; }
		}
		// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
		/// <summary>
		/// Gets a DataTable containing a list of store filters.
		/// </summary>
		public DataTable DtStoreFilters
		{
			get	
			{
				ArrayList userRIDList;
				FunctionSecurityProfile filterUserSecurity;
				FunctionSecurityProfile filterGlobalSecurity;
				//StoreFilterData storeFilterData;
                FilterData storeFilterData; 

				if (_dtStoreFilters == null)
				{
					filterUserSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
					filterGlobalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

					storeFilterData = new FilterData();
					userRIDList = new ArrayList();

					userRIDList.Add(-1);

					if (!filterUserSecurity.AccessDenied)
					{
						userRIDList.Add(_SAB.ClientServerSession.UserRID);
					}

					if (!filterGlobalSecurity.AccessDenied)
					{
						userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
					}

                    _dtStoreFilters = storeFilterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);
				}
				return _dtStoreFilters;
			}
		}

		/// <summary>
		/// Gets the tooltip object
		/// </summary>
		public string[] ComputationModes
		{
			get	
			{
				if (_computationModes == null)
				{
					_computationModes = _SAB.ApplicationServerSession.GetComputationModes();
				}
				return _computationModes;
			}
		}
        /// <summary>
        /// Gets the File menu tool
        /// </summary>
        protected PopupMenuTool FileMenuTool
        {
            get
            {
                return _fileMenuTool;
            }
        }

        /// <summary>
        /// Gets the Edit menu tool
        /// </summary>
        protected PopupMenuTool EditMenuTool
        {
            get
            {
                return _editMenuTool;
            }
        }

        /// <summary>
        /// Gets the Tools menu tool
        /// </summary>
        protected PopupMenuTool ToolsMenuTool
        {
            get
            {
                return _toolsMenuTool;
            }
        }
		/// <summary>
		/// Gets or sets a boolean identifying if drag/drop is allowed on the form.
		/// </summary>
		public bool AllowDragDrop
		{
			get { return this.AllowDrop; }
			set { this.AllowDrop = value; }
		}

		/// <summary>
		/// Gets the image list used for dragging.
		/// </summary>
		public ImageList ImageListDrag
		{
			get { return imageListDrag; }
		}

		/// <summary>
		/// Gets the spacing used to build drag images.
		/// </summary>
		public int Spacing
		{
			get { return _spacing; }
		}

        // Begin TT#1639 - RMatelic - Error message on Header with Piggybacking Workflow
        /// <summary>
		/// Gets or sets a boolean to cancel thw window closing event.
		/// </summary>
        public bool CancelFormClosing
		{
            get { return _cancelFormClosing; }
            set { _cancelFormClosing = value; }
		}
        //End TT#1613
		#endregion Properties

		#region Methods

		/// <summary>
		/// Creates a new control.
		/// </summary>
		/// <param name="aType">The type of control to be created</param>
		protected Control CreateControl(System.Type aType)
		{
			Control control = (System.Windows.Forms.Control)Activator.CreateInstance(aType);
			_myControls.Add(control);
			return control;
		}

		/// <summary>
		/// Creates a new control.
		/// </summary>
		/// <param name="aType">The type of control to be created</param>
		/// <param name="args">A list of objects containing the arguments needed to create the control</param>
		protected Control CreateControl(System.Type aType, object[] args)
		{
			Control control = (System.Windows.Forms.Control)Activator.CreateInstance(aType, args);
			_myControls.Add(control);
			return control;
		}

		/// <summary>
		/// Creates a new component.
		/// </summary>
		/// <param name="aType">The type of component to be created</param>
		protected Component CreateComponent(System.Type aType)
		{
			Component component = (System.ComponentModel.Component)Activator.CreateInstance(aType);
			_myComponents.Add(component);
			return component;
		}

		/// <summary>
		/// Creates a new component.
		/// </summary>
		/// <param name="aType">The type of component to be created</param>
		/// <param name="args">A list of objects containing the arguments needed to create the component</param>
		protected Component CreateComponent(System.Type aType, object[] args)
		{
			Component component = (System.ComponentModel.Component)Activator.CreateInstance(aType, args);
			_myComponents.Add(component);
			return component;
		}

		/// <summary>
		/// Sets the ClientSessionTransaction so that a new object will be retrieved next time.
		/// </summary>
		protected void ResetClientSessionTransaction()
		{
			if (_clientTransaction != null)
			{
				_clientTransaction.Dispose();
			}
			_clientTransaction = null;
		}

		/// <summary>
		/// Sets the ApplicationSessionTransaction so that a new object will be retrieved next time.
		/// </summary>
		protected void ResetApplicationSessionTransaction()
		{
			if (_applicationTransaction != null)
			{
				_applicationTransaction.Dispose();
			}
			_applicationTransaction = null;
		}

		/// <summary>
		/// Method to format the title
		/// </summary>
		/// <param name="aDataState">Identifies the state of the data in the form</param>
		/// <param name="aItemName">The name of the item to display.  Set to null if aNewItem is true.</param>
		/// <remarks>Uses the Name property of the form when setting the title.</remarks>
        /// protected void Format_Title(eDataState aDataState, eMIDTextCode aFormName, string aItemName)
		public void Format_Title(eDataState aDataState, eMIDTextCode aFormName, string aItemName) //TT#435-MD-DOConnell-Add new features to Audit
		{
			switch (aDataState)
			{
				case eDataState.New:
					aItemName += " [New]";
					break;
				case eDataState.ReadOnly:
					aItemName += " [Read Only]";
					break;
				default:
					break;
			}
			
			switch (aItemName)
			{
				case null:
				case "":
					this.Text = MIDText.GetTextOnly((int)aFormName);
					break;
				default:
					this.Text = MIDText.GetTextOnly((int)aFormName) + " - " + aItemName;
					break;
			}
		}

		/// <summary>
		/// Fills a supplied UltraGrid ValueList with data from APPLICATION_TEXT table.
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="textType">Enum value from eMIDTextType</param>
		/// <param name="orderBy">Enum value from eMIDTextOrderBy </param>
		protected void FillTextTypeList(Infragistics.Win.ValueList valueList, eMIDTextType textType, eMIDTextOrderBy orderBy)
		{
			foreach (DataRow drRule in MIDText.GetTextType(textType, orderBy).Rows)
			{
				valueList.ValueListItems.Add(Convert.ToInt32(drRule["TEXT_CODE"],
					CultureInfo.CurrentUICulture), drRule["TEXT_VALUE"].ToString());
			}
		}

		/// <summary>
		/// Method to call when cancel button is clicked
		/// </summary>
		protected void Cancel_Click()
		{
			// BEGIN MID Track #2670 - add Cancel option to Save Changes dialog
			_ignoreWindowState = true;
			// END MID Track #2670 
			// BEGIN TT#508-MD - stodd -  Correct Assortment Properties Enqueue
			_cancelClicked = true;
			// END TT#508-MD - stodd -  Correct Assortment Properties Enqueue
			Close();
		}

		/// <summary>
		/// Method to call when OK button is clicked
		/// </summary>
		protected void OK_Click()
		{
			_okClicked = true;	// TT#508-MD - stodd -  Correct Assortment Properties Enqueue
			SaveChanges();
			if (!_errorFound)
			{
				Close();
			}
		}

		/// <summary>
		/// Method to call when Save button is clicked
		/// </summary>
		/// <param name="aCloseForm">Identifies if the form should be close on successful save</param>
		protected void Save_Click(bool aCloseForm)
		{
			SaveChanges();
			if (!_errorFound && aCloseForm)
			{
				Close();
			}
		}

		/// <summary>
		/// Checks for pending changes
		/// </summary>
		protected bool CheckForPendingChanges()
		{
			if ((ChangePending) && (!ErrorFound))   // TT#2083 - gtaylor
			{
                // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
                //ResultSaveChanges =  MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
                //    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(GetPendingMessage()), "Save Changes",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway

				if (ResultSaveChanges == DialogResult.Yes) 
				{
					if (SaveChanges())
					{
//Begin Modification - JScott - Export Method - Fix 1
//						_changePending = false;
						ChangePending = false;
//End Modification - JScott - Export Method - Fix 1
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
                    UndoSaveChanges();
					return true;
				}
			}
			else
			{
				ResultSaveChanges = DialogResult.None;
				return true;
			}
		}

		/// <summary>
		/// Called when form is closing
		/// </summary>
		/// <remarks>Override BeforeClosing()and AfterClosing()to add code unique to a form</remarks>
//		protected void Form_Closing()
		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
                // Begin TT#1580 - JSmith - Window Closes before Save Yes, No or Cancel can be selected
                if (BypassBaseClosingLogic)
                {
                    return;
                }
                // End TT#1580
				FormWindowState fws = this.WindowState; // BEGIN / END  MID Track #2670
				_formIsClosing = true;
				BeforeClosing();

                // Begin TT#1639 - RMatelic - Error message on Header with Piggybacking Workflow
                if (_cancelFormClosing)
                {
                    _cancelFormClosing = false;
                    _formIsClosing = false;
                    e.Cancel = true;
                    return;
                }
                // End TT#1639  

				_saveOnClose = false;
//Begin Modification - JScott - Export Method - Part 10
//				if (_changePending)
				if ((ChangePending) && (!ErrorFound)) // TT#2083 - gtaylor
//End Modification - JScott - Export Method - Part 10
				{
					// BEGIN MID Track #2670 - add Cancel option to Save Changes dialog
					
					//if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
					//	MessageBoxButtons.YesNo, MessageBoxIcon.Question)
					//	== DialogResult.Yes) 
					//{
					//	ResultSaveChanges = DialogResult.Yes;
					//
					//	_saveOnClose = true;
					//	SaveChanges();
					//	if (_errorFound)
					//	{
					//		e.Cancel = true;
					//		_formIsClosing = false;
					//		_saveOnClose = false;
					//	}
					//}
					//else
					//{
					//	ResultSaveChanges = DialogResult.No;
					//}

                    // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
                    //ResultSaveChanges =  MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SavePendingChanges),  "Save Changes",
                    //    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    ResultSaveChanges = MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(GetPendingMessage()), "Save Changes",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
					switch (ResultSaveChanges)
					{
						case DialogResult.Yes:
							
							_saveOnClose = true;
							SaveChanges();
							if (_errorFound)
							{
								e.Cancel = true;
								_formIsClosing = false;
								_saveOnClose = false;
							}
							break;
						
						case DialogResult.No:
                            UndoSaveChanges();						 
							break;

						case DialogResult.Cancel:
							 
							e.Cancel = true;
							_formIsClosing = false;
							_saveOnClose = false;
							break;
					}
					// END MID Track #2670 
					if (!e.Cancel)
					{
//Begin Modification - JScott - Export Method - Fix 1
//						_changePending = false;
						ChangePending = false;
//End Modification - JScott - Export Method - Fix 1
					}
				}
				if (!e.Cancel)
				{
					AfterClosing();
					//Begin Track #5950 - JScott - Save Low Level to High may get warning message
					// This line is necessary to correct issue where some forms would not return to maximized after a
					// different form closed (Plan View/Plan View Save w/Cancel) due to disposing the ActiveControl
					this.ActiveControl = null;
					//End Track #5950 - JScott - Save Low Level to High may get warning message

                    // Begin TT#856 - JSmith - Out of memory
                    this.Visible = false;
                    this.MdiParent = null;
                    this.PerformLayout();
                    // End TT#856

					System.GC.WaitForPendingFinalizers();
					System.GC.Collect();
				}
					 // BEGIN MID Track #2670 - add Cancel option to Save Changes dialog
				else // the upper right corner buttons disappear if the window is maximized and Cancel is selected   
				{	 // after a Close via the 'X' or icon 'Close'; the _ignoreWindowState switch is set in
					 // IClose and Cancel_Click events which don't affect the corner buttons; this is a rigged 
					 // fix until a better solution is determined.
					if (_ignoreWindowState)
						_ignoreWindowState = false;
					else if (fws == FormWindowState.Maximized)
					{
						this.WindowState = FormWindowState.Normal;
						this.WindowState = FormWindowState.Maximized;
					}
				}
				// END MID Track #2670 
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

        // Begin TT#856 - JSmith - Out of memory

        // Begin TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        virtual protected eMIDTextCode GetPendingMessage()
        {
            return eMIDTextCode.msg_SavePendingChanges;
        }
        // End TT#4811 - JSmith - Choose to not save changes in style review but changes save anyway
        protected override void OnMdiChildActivate(EventArgs e)
        {
            base.OnMdiChildActivate(e);

            try
            {
                typeof(Form).InvokeMember("FormerlyActiveMdiChild",
                BindingFlags.Instance | BindingFlags.SetProperty |
                BindingFlags.NonPublic, null,
                this, new object[] { null });
            }

            catch (Exception)
            {
                // Something went wrong. Maybe we don't have enough 
                // permissions to perform this or the 
                // "FormerlyActiveMdiChild" property no longer 
                // exists. 
            }
        }
		// End TT#856

		protected System.Windows.Forms.Form GetForm(System.Type aType, object[] args, bool aAlwaysCreateNewForm)
		{
			try
			{
				bool foundForm = false;
				System.Windows.Forms.Form frm = null;

				if (!aAlwaysCreateNewForm)
				{
					foreach (Form childForm in this.MdiParent.MdiChildren)
					{
						if (childForm.GetType().Equals(aType))
						{
							frm = childForm;
							foundForm = true;
							break;
						}
						else
						{
							childForm.Enabled = false;
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
				MessageBox.Show(exception.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				throw;
			}
		}

		/// <summary>
		/// Use to allow form to add code before closing process is called
		/// </summary>
		/// <remarks>Not required to override this method</remarks>
		virtual protected void BeforeClosing()
		{
			
		}

		/// <summary>
		/// Use to allow form to add code after closing process is called
		/// </summary>
		/// <remarks>Not required to override this method</remarks>
		virtual protected void AfterClosing()
		{
			
		}

		private void Form_Load(object sender, System.EventArgs e)
		{
			try
			{
				FormLoaded = true;
                FormatControls();
                // Begin TT#272-MD - JSmith - Version 5.0 - General screen cleanup 
                SetCatchKeyDown();
                // End TT#272-MD - JSmith - Version 5.0 - General screen cleanup 
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		private void Form_Activated(object sender, System.EventArgs e)
		{
			try
			{
				if (!_previouslyActivated)
				{
                    // Begin TT#846 - JSmith - Windows open with scroll bars under Windows 7
                    //if (_setAutoScrollMinSize)
                    //{
                    //    this.AutoScrollMinSize = new System.Drawing.Size(this.Width - 35, this.Height - 35);
                    //}
                    // End TT#846
					this.AutoScroll = true;
					if (this.MdiParent != null)
					{
						this.SetDesktopLocation(0,0);
					}
				}
				_previouslyActivated = true;

                // Begin TT#2598 - JSmith - Save As button does not work
                _isActivated = true;
                // End TT#2598 - JSmith - Save As button does not work
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

        // Begin TT#2598 - JSmith - Save As button does not work
        void MIDFormBase_Deactivate(object sender, EventArgs e)
        {
            _isActivated = false;
        }
        // End TT#2598 - JSmith - Save As button does not work
		
		protected void Merchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e, eSecurityTypes aSecurityTypes)
		{
			try
			{
				MIDTreeNode node;
				HierarchyNodeSecurityProfile hierNodeSecurity;
				if (e.Data.GetDataPresent(typeof(MIDTreeNode))) 
				{
					node = (MIDTreeNode)e.Data.GetData(typeof(MIDTreeNode));
					hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(node.NodeRID, (int)aSecurityTypes);
					if (!hierNodeSecurity.AllowUpdate)
					{
						e.Effect = DragDropEffects.None;
					}
					else
					{
						e.Effect = DragDropEffects.All;
					}
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this._enhancedToolTip = new MIDRetail.Windows.Controls.MIDEnhancedToolTip(this.components);
            this._inheritanceProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.utmMain = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._MIDFormBase_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._MIDFormBase_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._MIDFormBase_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._MIDFormBase_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.imageListDrag = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._inheritanceProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // _errorProvider
            // 
            this._errorProvider.ContainerControl = this;
            // 
            // _inheritanceProvider
            // 
            this._inheritanceProvider.ContainerControl = this;
            // 
            // utmMain
            // 
            this.utmMain.DesignerFlags = 1;
            this.utmMain.DockWithinContainer = this;
            this.utmMain.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.utmMain.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
            // 
            // _MIDFormBase_Toolbars_Dock_Area_Left
            // 
            this._MIDFormBase_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDFormBase_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._MIDFormBase_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._MIDFormBase_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDFormBase_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._MIDFormBase_Toolbars_Dock_Area_Left.Name = "_MIDFormBase_Toolbars_Dock_Area_Left";
            this._MIDFormBase_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 266);
            this._MIDFormBase_Toolbars_Dock_Area_Left.ToolbarsManager = this.utmMain;
            // 
            // _MIDFormBase_Toolbars_Dock_Area_Right
            // 
            this._MIDFormBase_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDFormBase_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._MIDFormBase_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._MIDFormBase_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDFormBase_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(292, 0);
            this._MIDFormBase_Toolbars_Dock_Area_Right.Name = "_MIDFormBase_Toolbars_Dock_Area_Right";
            this._MIDFormBase_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 266);
            this._MIDFormBase_Toolbars_Dock_Area_Right.ToolbarsManager = this.utmMain;
            // 
            // _MIDFormBase_Toolbars_Dock_Area_Top
            // 
            this._MIDFormBase_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDFormBase_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._MIDFormBase_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._MIDFormBase_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDFormBase_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MIDFormBase_Toolbars_Dock_Area_Top.Name = "_MIDFormBase_Toolbars_Dock_Area_Top";
            this._MIDFormBase_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(292, 0);
            this._MIDFormBase_Toolbars_Dock_Area_Top.ToolbarsManager = this.utmMain;
            // 
            // _MIDFormBase_Toolbars_Dock_Area_Bottom
            // 
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 266);
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.Name = "_MIDFormBase_Toolbars_Dock_Area_Bottom";
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(292, 0);
            this._MIDFormBase_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.utmMain;
            // 
            // imageListDrag
            // 
            this.imageListDrag.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListDrag.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListDrag.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // MIDFormBase
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this._MIDFormBase_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._MIDFormBase_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._MIDFormBase_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._MIDFormBase_Toolbars_Dock_Area_Top);
            this.Name = "MIDFormBase";
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._inheritanceProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}

		/// <summary>
		/// Use to save pending changes on the form
		/// </summary>
		virtual protected bool SaveChanges()
		{
			throw new Exception("Can not call base method");
		}

        /// <summary>
        /// Use to undo pending changes on the form
        /// </summary>
        virtual protected bool UndoSaveChanges()
        {
            return true;
        }

        public void Merchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {
                Image_DragEnter(sender, e);

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                    if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
                    {
                        if (FunctionSecurity.AllowUpdate ||
                            FunctionSecurity.AllowView)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        protected void Merchandise_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }
        // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.

        public void Store_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {
                Image_DragEnter(sender, e);

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.Store)
                    if (cbList.ClipboardDataType == eProfileType.Store)
                    {
                        if (FunctionSecurity.AllowUpdate ||
                            FunctionSecurity.AllowView)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        // Begin TT#209 - RMatelic
        public void StoreGroupLevel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            TreeNodeClipboardList cbList = null;
            try
            {
                Image_DragEnter(sender, e);

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.Store)
                    if (cbList.ClipboardDataType == eProfileType.StoreGroupLevel)
                    {
                        if (FunctionSecurity.AllowUpdate ||
                            FunctionSecurity.AllowView)
                        {
                            e.Effect = DragDropEffects.All;
                        }
                        else
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        // End TT#209

        //Begin TT#1388-MD -jsobek -Product Filters -unused function
        //public void Method_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        //{
        //    TreeNodeClipboardList cbList = null;
        //    try
        //    {
        //        Image_DragEnter(sender, e);

        //        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
        //        {
        //            cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
        //            //if (cbp.ClipboardDataType == eClipboardDataType.Method)
        //            // Begin Track #6277 - JSmith - Cannot drag/drop a OTS Forecast Method in to a Task List for Forecasting
        //            //if (cbList.ClipboardDataType == eProfileType.Method)
        //            if (Enum.IsDefined(typeof(eMethodProfileType), Convert.ToInt32(cbList.ClipboardDataType)))
        //            // End Track #6277
        //            {
        //                if (FunctionSecurity.AllowUpdate ||
        //                    FunctionSecurity.AllowView)
        //                {
        //                    e.Effect = DragDropEffects.All;
        //                }
        //                else
        //                {
        //                    e.Effect = DragDropEffects.None;
        //                }
        //            }
        //            else
        //            {
        //                e.Effect = DragDropEffects.None;
        //            }
        //        }
        //        else
        //        {
        //            e.Effect = DragDropEffects.None;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}
        //End TT#1388-MD -jsobek -Product Filters -unused function

		/// <summary>
		/// Use to set cursor effect on drag enter.
		/// </summary>
		/// <param name="e">The DragEventArgs for the drag enter event</param>
		protected void ObjectDragEnter(System.Windows.Forms.DragEventArgs e)
		{
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

			// Begin Track #5231 - JSmith - Dragging Error
			//try
            //{
            //    IDataObject data = Clipboard.GetDataObject();
            //    // check for appropriate object in clipboard before setting effect
            //    if (data.GetDataPresent(ClipboardProfile.Format.Name))
            //    {
            //        e.Effect = DragDropEffects.All;
            //    }
            //    else
            //    {
            //        e.Effect = DragDropEffects.None;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    throw(ex);
            //}
            //bool retry = true;
            //int retryCount = 0, retryTimes = 3;
            //while (retry)
            //{
            //    try
            //    {
            //        //IDataObject data = Clipboard.GetDataObject();
            //        retry = false;
            //        // check for appropriate object in clipboard before setting effect
            //        if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            //        {
            //            e.Effect = DragDropEffects.All;
            //        }
            //        else
            //        {
            //            e.Effect = DragDropEffects.None;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // if Clipboard requests fails, wait 10 milliseconds and try again
            //        ++retryCount;
            //        if (retryCount >= retryTimes)
            //        {
            //            throw (ex);
            //        }
            //        else
            //        {
            //            System.Threading.Thread.Sleep(10);
            //        }
            //    }
            //}
			// End Track #5231
		}

        //Begin TT#1388-MD -jsobek -Product Filters -unused function
		/// <summary>
		/// Retrieves data and verifies type on drag drop.
		/// </summary>
		/// <param name="aClipboardDataType">The type of data to retrieve from the clipboard</param>
		/// <returns></returns>
        //protected ClipboardProfile GetClipboardData(eClipboardDataType aClipboardDataType)
        //protected TreeNodeClipboardList GetClipboardData(eProfileType aClipboardDataType)
        //{
        //    bool retry = true;
        //    int retryCount = 0, retryTimes = 3;

        //    TreeNodeClipboardList cbList = null;
        //    while (retry)
        //    {
        //        try
        //        {
        //            // Create a new instance of the DataObject interface.
        //            IDataObject data = Clipboard.GetDataObject();
        //            retry = false;

        //            //If the data is ClipboardProfile, then retrieve the data
        //            if (data.GetDataPresent(typeof(TreeNodeClipboardList)))
        //            {
        //                cbList = (TreeNodeClipboardList)data.GetData(typeof(TreeNodeClipboardList));
        //                // make sure you have the right type of data
        //                if (cbList.ClipboardDataType == aClipboardDataType)
        //                {
        //                    switch (cbList.ClipboardDataType)
        //                    {
        //                        //case eClipboardDataType.HierarchyNode:
        //                        case eProfileType.HierarchyNode:
        //                            //if (cbList.ClipboardData.GetType() == typeof(HierarchyClipboardData))
        //                            //{
        //                                return cbList;
        //                            //}
        //                            //else
        //                            //{
        //                            //    throw new BadDataInClipboardException();
        //                            //}
        //                        //case eClipboardDataType.Store:
        //                        case eProfileType.Store:
        //                            return cbList;
        //                        //case eClipboardDataType.Attribute:
        //                        case eProfileType.StoreGroup:
        //                            return cbList;
        //                        //case eClipboardDataType.AttributeSet:
        //                        case eProfileType.StoreGroupLevel:
        //                            return cbList;
        //                        //case eClipboardDataType.Workflow:
        //                        case eProfileType.Workflow:
        //                            //								if (cbp.ClipboardData.GetType() == typeof(MIDTreeNodeClipboardData))
        //                            //								{
        //                            return cbList;
        //                        //								}
        //                        //case eClipboardDataType.Method:
        //                        //Begin TT#523 - JScott - Duplicate folder when new folder added
        //                        //case eProfileType.Method:
        //                        case eProfileType.MethodOTSPlan:
        //                        case eProfileType.MethodForecastBalance:
        //                        case eProfileType.MethodModifySales:
        //                        case eProfileType.MethodForecastSpread:
        //                        case eProfileType.MethodCopyChainForecast:
        //                        case eProfileType.MethodCopyStoreForecast:
        //                        case eProfileType.MethodExport:
        //                        case eProfileType.MethodGlobalUnlock:
        //                        case eProfileType.MethodRollup:
        //                        case eProfileType.MethodGeneralAllocation:
        //                        case eProfileType.MethodAllocationOverride:
        //                        case eProfileType.MethodRule:
        //                        case eProfileType.MethodVelocity:
        //                        case eProfileType.MethodSizeNeedAllocation:
        //                        case eProfileType.MethodSizeCurve:
        //                        case eProfileType.MethodFillSizeHolesAllocation:
        //                        case eProfileType.MethodBasisSizeAllocation:
        //                        case eProfileType.MethodWarehouseSizeAllocation:
        //                        //End TT#523 - JScott - Duplicate folder when new folder added
        //                            //if (cbp.ClipboardData.GetType() == typeof(MethodClipboardData))
        //                            //{
        //                                return cbList;
        //                            //}
        //                            //else
        //                            //{
        //                            //    throw new BadDataInClipboardException();
        //                            //}
        //                        //case eClipboardDataType.Header:
        //                        case eProfileType.AllocationHeader:
        //                            return cbList;
        //                        default:
        //                            throw new BadDataInClipboardException();
        //                    }
        //                }
        //                else
        //                {
        //                    throw new BadDataInClipboardException();
        //                }
        //            }
        //            else
        //            {
        //                throw new BadDataInClipboardException();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // if Clipboard requests fails, wait 10 milliseconds and try again
        //            ++retryCount;
        //            if (retryCount >= retryTimes)
        //            {
        //                throw (ex);
        //            }
        //            else
        //            {
        //                System.Threading.Thread.Sleep(10);
        //            }
        //        }
        //    }
        //    throw new BadDataInClipboardException();
        //}
        //End TT#1388-MD -jsobek -Product Filters -unused function

		// BEGIN TT#742-MD - Stodd - Assortment tooltips
		/// <summary>
		/// Sets the tooltip on the grid for display during drag & drop
		/// </summary>
		/// <param name="ultraGrid"></param>
		/// <param name="toolTipText"></param>
		protected void SetUltraGridDragOverToolTip(Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid, string toolTipText)
		{
			string prevToolTip = this.toolTip1.GetToolTip(ultraGrid);
			_showToolTip = false;
			if (string.IsNullOrEmpty(prevToolTip))
			{
				_showToolTip = true;
			}

			if (toolTipText == null)
			{
				this.toolTip1.Active = false;
				this.toolTip1.SetToolTip(ultraGrid, null);
			}
			else
			{
				this.toolTip1.SetToolTip(ultraGrid, toolTipText);
				this.toolTip1.Active = true;
			}
		}

		/// <summary>
		/// Shows tooltip for grid--not one of it's elements.
		/// </summary>
		/// <param name="ultraGrid"></param>
		/// <param name="toolTip"></param>
		protected void ShowUltraGridToolTip(Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid)
		{
			string ugDetailsToolTip = this.toolTip1.GetToolTip(ultraGrid);
			if (_showToolTip)
			{
				this.toolTip1.Active = true;
				this.toolTip1.Show(this.toolTip1.GetToolTip(ultraGrid), ultraGrid, 0, 0);
			}
		}
		// END TT#742-MD - Stodd - Assortment tooltips
		
		/// <summary>
		/// Shows ToolTip to display error message in an UntraGrid cell 
		/// </summary>
		/// <param name="ultraGrid">The UltraGrid where the tool tip is to be displayed</param>
		/// <param name="e">The UIElementEventArgs arguments of the MouseEnterElement event</param>
		protected void ShowUltraGridToolTip(Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				if(this.toolTip1 != null && this.toolTip1.Active) 
				{
					this.toolTip1.Active = false; //turn it off 
				}

				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
				if (gridCell != null)
				{
					if (gridCell.Tag != null) 
					{
						if (gridCell.Tag.GetType() == typeof(System.String))
						{
							toolTip1.Active = true; 
							toolTip1.SetToolTip(ultraGrid, (string)gridCell.Tag);
						}
						else
							if (gridCell.Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
						{
							GridCellTag gct = (GridCellTag)gridCell.Tag;
							if (gct.Message != null)
							{
								toolTip1.Active = true; 
								toolTip1.SetToolTip(ultraGrid, gct.Message);
							}
							else
								if (gct.HelpText != null)
							{
								toolTip1.Active = true; 
								toolTip1.SetToolTip(ultraGrid, gct.HelpText);
							}
							else
							{
								toolTip1.Active = false;
							}
						}
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Shows ToolTip  
		/// </summary>
		/// <param name="sender">The object where the tool tip is to be displayed</param>
		/// <param name="e">The arguments of the event</param>
		protected void ShowToolTip(object sender, System.EventArgs e, string aMessage)
		{
			try
			{
				if(this.toolTip1 != null && this.toolTip1.Active) 
				{
					this.toolTip1.Active = false; //turn it off 
				}

				toolTip1.Active = true; 
				toolTip1.SetToolTip((System.Windows.Forms.Control)sender, aMessage);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        // BEGIN TT#62 - stodd - infrastructure changes EOF
        /// <summary>
        /// Assigns an event handler to all TextBox and CombBox controls to catch the key down event. 
        /// Enables the use of ESC and SHIFT-ESC.
        /// ESC: Acts as EOF. Removes characters from current cursor postion to end of line.
        /// SHIFT-ESC: removes ALL characters.
        /// To enable on a method, include a call to SetCatchKeyDown() in the Common_Load() method of the form.
        /// </summary>
        protected void SetCatchKeyDown()
        {
            // Begin TT#272-MD - JSmith - Version 5.0 - General screen cleanup 
            if (_keyEventsAssigned)
            {
                return;
            }
            _keyEventsAssigned = true;
            // End TT#272-MD - JSmith - Version 5.0 - General screen cleanup

            foreach (Control ctrl in this.Controls)
            {
                // check for sub-controls for control like groupbox, panel, etc.  
                // If found, recursively loop through all sub-controls in the controls' control collection.  
                if (ctrl.Controls.Count > 0)
                {
                    LoopCatchKeyDown(ctrl);
                }
                else
                {
                    if (ctrl.GetType() == typeof(System.Windows.Forms.TextBox)
                        || ctrl.GetType() == typeof(System.Windows.Forms.ComboBox))
                        AssignKeyEntryEventHandler(ctrl);
                }
            }
        }

        private void LoopCatchKeyDown(Control ctrl)
        {
            try
            {
                if (ctrl.Controls.Count > 0)
                {
                    // NumericUpDown has sub-controls need to set control collection before sub controls
                    if (ctrl.GetType() == typeof(System.Windows.Forms.NumericUpDown))
                    {
                        if (ctrl.GetType() == typeof(System.Windows.Forms.TextBox)
                            || ctrl.GetType() == typeof(System.Windows.Forms.ComboBox))
                            AssignKeyEntryEventHandler(ctrl);
                    }
                    else
                    {
                        foreach (Control subCtrl in ctrl.Controls)
                        {
                            LoopCatchKeyDown(subCtrl);
                        }
                    }
                }

                if (ctrl.GetType() == typeof(System.Windows.Forms.TextBox)
                    || ctrl.GetType() == typeof(System.Windows.Forms.ComboBox))
                    AssignKeyEntryEventHandler(ctrl);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        protected void AssignKeyEntryEventHandler(Control ctrl)
        {
            ctrl.KeyDown += new KeyEventHandler(this.OnKeyDownHandler);
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.Escape)
            {
                ((Control)sender).Text = "";
            }
            if (e.KeyCode == Keys.Escape)
            {
                if (sender.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    int idx = ((TextBox)sender).SelectionStart;
                    string temp = ((TextBox)sender).Text.Substring(0, idx);
                    ((TextBox)sender).Text = temp;
                    ((TextBox)sender).SelectionStart = idx;
                }

                if (sender.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    int idx = ((ComboBox)sender).SelectionStart;
                    string temp = ((ComboBox)sender).Text.Substring(0, idx);
                    ((ComboBox)sender).Text = temp;
                    ((ComboBox)sender).SelectionStart = idx;
                }
                else if (sender.GetType() == typeof(MIDComboBoxEnh))
                {
                    // Begin TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.
                    //int idx = ((MIDComboBoxEnh)sender).SelectionStart;
                    //string temp = ((MIDComboBoxEnh)sender).Text.Substring(0, idx);
                    //((MIDComboBoxEnh)sender).Text = temp;
                    //((MIDComboBoxEnh)sender).SelectionStart = idx;
                    int idx = ((MIDComboBoxEnh)sender).Get_SelectionStart();
                    string temp = ((MIDComboBoxEnh)sender).Text.Substring(0, idx);
                    ((MIDComboBoxEnh)sender).Text = temp;
                    ((MIDComboBoxEnh)sender).Set_SelectionStart(idx);
                    // End TT#532-MD - JSmith - Export method tried to create a new method and receive a TargetInvocationException.  Also tried to open all existing methods and received the same error.
                }
            }
        }
        // END TT#62 - stodd - infrastructure changes EOF

		/// <summary>
		/// Sets the controls on the form based on the security of the user
		/// </summary>
        //protected void SetReadOnly(bool aAllowUpdate)  //Security changes - 1/24/2005 vg
        public void SetReadOnly(bool aAllowUpdate)  //Security changes - 1/24/2005 vg //TT#435-MD-DOConnell-Add new features to Audit
		{
			try
			{
				_allowUpdate = aAllowUpdate;    //Security changes - 1/24/2005 vg
				bool readOnlyFlag = false;
				bool enabledFlag = true;
				if (!_allowUpdate)
				{
					readOnlyFlag = true;
					enabledFlag = false;
				}
				else
				{
//					return;
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
						// BEGIN TT#727-MD - Stodd - toolbar security
						if (ctrl.GetType() == typeof(Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea))
						{

						}
						else
						{
							LoopReadOnly(ctrl, readOnlyFlag, enabledFlag);
						}
						// END TT#727-MD - Stodd - toolbar security
					}
					else
						// set the control to read only
					{
						// BEGIN TT#727-MD - Stodd - toolbar security
						if (ctrl.GetType() == typeof(Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea))
						{
							LoopReadOnly(ctrl, readOnlyFlag, enabledFlag);
						}
						else
						{
							SetReadOnly(ctrl, readOnlyFlag, enabledFlag);
						}
						// END TT#727-MD - Stodd - toolbar security
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Sets the controls on the control based on the security of the user
		/// </summary>
		/// <param name="aCtrl">The control for which read only is to be set</param>
		/// <param name="aReadOnlyFlag">A flag identifying if the control and all subcontrols should be set to read only</param>
		protected void SetControlReadOnly(Control aCtrl, bool aReadOnlyFlag)
		{
			try
			{
				bool readOnlyFlag;
				bool enabledFlag;
		
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
						// set the control to read only
					{
						SetReadOnly(ctrl, readOnlyFlag, enabledFlag);
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
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
				// BEGIN TT#727-MD - Stodd - toolbar security
				if (aCtrl.GetType() == typeof(Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea))
				{
					Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea toolbarDockArea = (Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea)aCtrl;
					UltraToolbarsManager toolbarManager = toolbarDockArea.ToolbarsManager;
					foreach (UltraToolbar tb in toolbarDockArea.ToolbarsManager.Toolbars)
					{
						foreach (Infragistics.Win.UltraWinToolbars.ToolBase tBase in tb.Tools)
						{
							SetToolbarToolReadOnly(tBase, aReadOnlyFlag, aEnabledFlag);
						}

					}
				}
				else if (aCtrl.Controls.Count > 0)
				// END TT#727-MD - Stodd - toolbar security
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

				SetReadOnly(aCtrl, aReadOnlyFlag, aEnabledFlag);
			}
			catch( Exception exception )
			{
				HandleException(exception);
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
			try
			{
				// format control style
				if ( aCtrl.GetType().BaseType == typeof(ButtonBase) )
				{
					((ButtonBase)aCtrl).FlatStyle = FlatStyle.System;
				}

                if (aCtrl.GetType() == typeof(System.Windows.Forms.TextBox))
                {
					//Begin TT#201 - JScott - User ID rename is not allowed
					////((TextBox)aCtrl).ReadOnly = aReadOnlyFlag;
					((TextBox)aCtrl).ReadOnly = aReadOnlyFlag;
					//End TT#201 - JScott - User ID rename is not allowed
					aCtrl.Enabled = aEnabledFlag;
                }
				else if (aCtrl.GetType() == typeof(MIDRetail.Windows.MaskedEdit))
				{
					((MIDRetail.Windows.MaskedEdit)aCtrl).ReadOnly = aReadOnlyFlag;
				}
				else if (aCtrl.GetType() == typeof(MIDRetail.Windows.Controls.MIDDateRangeSelector))
				{
                    if (aCtrl.Name.ToUpper() == "MIDDATERANGESELECTOR1")
                        ((MIDRetail.Windows.Controls.MIDDateRangeSelector)aCtrl).Enabled = true;
                    else
					    ((MIDRetail.Windows.Controls.MIDDateRangeSelector)aCtrl).Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.NumericUpDown))
				{
					((NumericUpDown)aCtrl).ReadOnly = aReadOnlyFlag;
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.UpDownBase))
				{
					((UpDownBase)aCtrl).ReadOnly = aReadOnlyFlag;
				}
				else
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //if (aCtrl.GetType() == typeof(System.Windows.Forms.ComboBox))
                        if (aCtrl.GetType() == typeof(System.Windows.Forms.ComboBox) ||
                            aCtrl.GetType().BaseType == typeof(System.Windows.Forms.ComboBox) ||
                            // Begin TT#2 - stodd assortment
                            aCtrl.GetType().BaseType == typeof(MIDRetail.Windows.Controls.MIDWindowsComboBox) )
						// End TT#2 - stodd assortment|
                    // End Track #4872
				{
					// process all ComboBoxes that start with 'cbo'.  All others are unaffected.
					if (aCtrl.Name.StartsWith("cbo"))
					{
                            aCtrl.Enabled = aEnabledFlag;                  
					}
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.CheckBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.CheckedListBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.DateTimePicker))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.ListBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.RadioButton))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.Button))
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
				else
					if (aCtrl.GetType() == typeof(System.Windows.Forms.PictureBox))
				{
					aCtrl.Enabled = aEnabledFlag;
				}
				else
                        if ((aCtrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid)) && (((UltraGrid)aCtrl).Name.ToUpper() != "ULTRAGRID1"))
				{
					UltraGrid ultragid = (UltraGrid)aCtrl;
                    
					// BEGIN Issue 4640 stodd 9.21.2007
					// disables any Add New buttons on the grid
                    if (aReadOnlyFlag)
                    {
                        ultragid.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
                    }
                    // Begin Track #6305 - JSmith - Str Exp>right click>select new str>errs
                    else
                    {
                        ultragid.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
                    }
                    // End Track #6305

                    foreach (Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands)
                    {
                        if (aReadOnlyFlag)
                        {
                            oBand.Override.AllowAddNew = AllowAddNew.No;
                        }
                        // Begin Track #6305 - JSmith - Str Exp>right click>select new str>errs
                        else
                        {
                            oBand.Override.AllowAddNew = AllowAddNew.Yes;
                        }
                        // End Track #6305

                        foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns)
                        {
                            if (aReadOnlyFlag)
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
					// END Issue 4640

					if (aReadOnlyFlag)
					{
						ultragid.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
						ultragid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
						ultragid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
					}
					else
					{
						ultragid.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
						ultragid.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
						ultragid.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					}
// END: JScott: Modify way UltraGrids are disabled
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		// BEGIN TT#727-MD - Stodd - toolbar security
		/// <summary>
		/// Set the properties of the Tool COntrol based on the readOnlyFlag
		/// </summary>
		/// <param name="aTool">The current tool</param>
		/// <param name="readOnlyFlag">A flag identifying if the form should be set to read only</param>
		/// <param name="enabledFlag">A flag identifying if objects should be disabled if read only</param>
		private void SetToolbarToolReadOnly(ToolBase aTool, bool aReadOnlyFlag, bool aEnabledFlag)
		{
			try
			{
				if (aTool.GetType() == typeof(Infragistics.Win.UltraWinToolbars.TextBoxTool))
				{
					aTool.SharedProps.Enabled = aEnabledFlag;
				}

				if (aTool.GetType() == typeof(Infragistics.Win.UltraWinToolbars.ComboBoxTool))
				{
					// process all ComboBoxes that start with 'cbo'.  All others are unaffected.
					if (aTool.SharedProps.RootTool.Key.StartsWith("cbo"))
					{
						aTool.SharedProps.Enabled = aEnabledFlag;
					}
				}
				else if (aTool.GetType() == typeof(Infragistics.Win.UltraWinToolbars.PopupMenuTool))
				{
					// process all ComboBoxes that start with 'popu'.  All others are unaffected.
					if (aTool.SharedProps.RootTool.Key.StartsWith("popup"))
					{
						aTool.SharedProps.Enabled = aEnabledFlag;
					}
				}
				else if (aTool.GetType() == typeof(Infragistics.Win.UltraWinToolbars.ButtonTool))
				{
					if (aTool.SharedProps.RootTool.Key == "btnHelp")
					{
						aTool.SharedProps.Visible = false;
					}
					else
					{
						// do not disable the cancel, close and process buttons.
						if (aTool.SharedProps.RootTool.Key != "btnCancel" &&
							aTool.SharedProps.RootTool.Key != "btnClose" &&
							aTool.SharedProps.RootTool.Key != "btnProcess")
						{
							aTool.SharedProps.Enabled = aEnabledFlag;
						}
					}
				}

			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}
		// END TT#727-MD - Stodd - toolbar security
		
        private void FormatControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                // check for sub-controls for control like groupbox, panel, etc.  
                // If found, recursively loop through all sub-controls in the controls' control collection.  
                if (ctrl.Controls.Count > 0)
                {
                    LoopSetAppearance(ctrl);
                }
                else
                // set the control appearance
                {
                    SetControlAppearance(ctrl);
                }
            }
        }

        /// <summary>
        /// Recursive loop to go through all controls and sub-controls
        /// </summary>
        /// <param name="aCtrl">The current control</param>
        private void LoopSetAppearance(Control aCtrl)
        {
            try
            {
                SetControlAppearance(aCtrl);

                if (aCtrl.Controls.Count > 0)
                {
                    foreach (Control subCtrl in aCtrl.Controls)
                    {
                        LoopSetAppearance(subCtrl);
                    }
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void SetControlAppearance(Control aCtrl)
        {
            // format control style
            if (aCtrl.GetType().BaseType == typeof(ButtonBase))
            {
                ((ButtonBase)aCtrl).FlatStyle = FlatStyle.System;
            }

            // format control style
            if (aCtrl.GetType() == typeof(TabPage))
            {
                ((TabPage)aCtrl).UseVisualStyleBackColor = false;
                ((TabPage)aCtrl).BackColor = System.Drawing.SystemColors.ControlLightLight;
            }
            else if (aCtrl.GetType() == typeof(Infragistics.Win.UltraWinGrid.UltraGrid))
            {
                ApplyAppearance((UltraGrid)aCtrl);
            }
        }


		private void MIDFormBase_CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			if (FormLoaded)
			{
				if (!AllowUpdate && !_checkChanged)
				{
					// Unfortunately not a cancelable event. 
					// if the checkBox was changed and read only, need to set back
					if (((CheckBox)sender).Checked)
					{
						_checkChanged = true;
						((CheckBox)sender).Checked = false;
					}
					else
					{
						_checkChanged = true;
						((CheckBox)sender).Checked = true;
					}
				}
				_checkChanged = false;
			}
		}

		private void MIDFormBase_RadioButton_CheckedChanged(object sender, EventArgs e)
		{
			// Unfortunately not a cancelable event. 
			// if the RadioButton was changed and read only, need to set back
			// This event will fire 4 times when a radio button in a radio group is changed.
			//		1) The clicked radio button is unset by user clicking other button; this code sets it back
			//		2) The radio button the user clicked is unset by step 1.
			//		3) The original radio button is set back to true by step 1.
			//		4) The radio button the user clicked is unset by step 3.
			if (FormLoaded)
			{
				++_eventCount;
				if (!AllowUpdate && !_checkChanged)
				{
					if (((RadioButton)sender).Checked)
					{
						_checkChanged = true;
						((RadioButton)sender).Checked = false;
					}
					else
					{
						_checkChanged = true;
						((RadioButton)sender).Checked = true;
					}
				}
				if (_eventCount == 4)
				{
					_checkChanged = false;
					_eventCount = 0;
				}
			}
		}

		private void MIDFormBase_ReadOnly(object sender, EventArgs e)
		{
			Show_ReadOnly();
		}

		private void MIDFormBase_BeforeCellUpdate(object sender, BeforeCellUpdateEventArgs e)
		{
			Show_ReadOnly();
			e.Cancel = true;
		}

		private void MIDFormBase_BeforeRowInsert(object sender, BeforeRowInsertEventArgs e)
		{
			Show_ReadOnly();
			e.Cancel = true;
		}

		private void MIDFormBase_BeforeRowsDeleted(object sender, BeforeRowsDeletedEventArgs e)
		{
			Show_ReadOnly();
			e.Cancel = true;
		}

		private void Show_ReadOnly()
		{
			if (FormLoaded)
			{
				if (!AllowUpdate)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DataIsReadOnly),  this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
//Begin Modification - JScott - Export Method - Fix 1
//					_changePending = false;
					ChangePending = false;
//End Modification - JScott - Export Method - Fix 1
				}
			}
		}

		/// <summary>
		/// Resizes Infragistics UltraGrid columns to fit the data in the grid
		/// </summary>
		/// <param name="aGrid">The grid to be resized</param>
		protected void Resize_Columns(Infragistics.Win.UltraWinGrid.UltraGrid aGrid)
		{
			try
			{
				// BEGIN MID Track #3792 - replace obsolete method 
				//aGrid.DisplayLayout.AutoFitColumns = true;
				aGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792 
				foreach ( Infragistics.Win.UltraWinGrid.UltraGridBand band in aGrid.DisplayLayout.Bands )
				{
					foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn column in band.Columns )
					{
						column.PerformAutoResize(Infragistics.Win.UltraWinGrid.PerformAutoSizeType.AllRowsInBand);
					}
				}
				// BEGIN MID Track #3792 - replace obsolete method 
				//aGrid.DisplayLayout.AutoFitColumns = false;
				aGrid.DisplayLayout.AutoFitStyle = AutoFitStyle.None;
				// END MID Track #3792 
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Gets the name of the default computation mode
		/// </summary>
		/// <returns></returns>
		protected string GetDefaultComputationsName()
		{
			try
			{
				return _SAB.ApplicationServerSession.GetDefaultComputations();
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //protected void ApplyDefaults(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        //{
        //    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
        //    ugld.ApplyDefaults(aUltraGrid);
        //}
        protected void ApplyDefaults(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid, bool aApplyColumnFormat, int aDecimalPositions, bool aAutoResizeColumns)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            //End TT#169  
            ugld.ApplyDefaults(aUltraGrid, aApplyColumnFormat, aDecimalPositions, aAutoResizeColumns);
        }
        //End TT#169

        protected void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            //End TT#169
            ugld.ApplyAppearance(aUltraGrid);
        }

        // Begin TT#3139 - JSmith - Store Profiles – read-only access
        protected void ApplyAppearanceCenterText(Infragistics.Win.UltraWinGrid.UltraGrid aUltraGrid)
        {
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            ugld.ApplyAppearanceCenterText(aUltraGrid);
        }
        // End TT#3139 - JSmith - Store Profiles – read-only access

        protected void ApplyAppearance(Infragistics.Win.UltraWinGrid.UltraDropDown aUltraDropDown)
        {
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            //End TT#169
            ugld.ApplyAppearance(aUltraDropDown);
        }

		// BEGIN Override low level changes
		virtual protected int ValidateOverrideModel(TextBox aTxtOverride)
		{
			ModelsData modelsData;
			int modelRID;
			string modelID;
			OverrideLowLevelProfile overrideProfile;
			string message;

			try
			{
				modelRID = Include.NoRID;
				message = null;

				if (aTxtOverride.Modified)
				{
					ErrorProvider.SetError(aTxtOverride, string.Empty);
					if (aTxtOverride.Text.Trim().Length > 0)
					{
						modelID = aTxtOverride.Text.Trim();
						modelsData = new ModelsData();
						modelRID = modelsData.OverrideLowLevelsModel_GetModelKey(modelID);
						if (modelRID == Include.NoRID)
						{
							message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_EnterOrSelectValidItem);
						}
						else
						{
							overrideProfile = new OverrideLowLevelProfile(modelRID);

							if (overrideProfile.User_RID == Include.GlobalUserRID &&
								_globalOverrideLowLevelModelSecurity.AccessDenied)
							{
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForItem);
							}
							else if (_userOverrideLowLevelModelSecurity.AccessDenied)
							{
								message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForItem);
							}
						}
						if (message != null)
						{
							ErrorProvider.SetError(aTxtOverride, message);
							MessageBox.Show(message);
							modelRID = Include.UndefinedOverrideModel;
						}
					}
					else
					{
						modelRID = Include.NoRID;
					}
				}
				return modelRID;
			}
			catch
			{
				throw;
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		//virtual protected void LoadOverrideModelComboBox(ComboBox aComboBox, int selectedValue, int customModelRid)
		//{
		//    try
		//    {
		//        ComboObject co = null;
		//        FunctionSecurityProfile userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
		//        FunctionSecurityProfile globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);

		//        ProfileList overrideModelList = OverrideLowLevelProfile.LoadAllProfiles(selectedValue, _SAB.ClientServerSession.UserRID, globalSecurity.AllowView, userSecurity.AllowView, customModelRid);
		//        aComboBox.Items.Clear();

		//        // Add empty/none entry
		//        aComboBox.Items.Add(new ComboObject(Include.NoRID, ""));
		//        if (selectedValue == Include.NoRID)
		//            co = new ComboObject(Include.NoRID, "");

		//        SecurityAdmin secAdmin = new SecurityAdmin();
		//        foreach (OverrideLowLevelProfile ollp in overrideModelList)
		//        {
		//            string newName = ollp.Name;
		//            switch (ollp.User_RID)
		//            {
		//                case Include.GlobalUserRID:
		//                    newName = newName + " (global)";
		//                    break;
		//                case Include.CustomUserRID:
		//                    newName = newName + " (custom)";
		//                    break;
		//                default:
		//                    newName = newName + " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
		//                    break;
		//            }
		//            if (ollp.User_RID == Include.CustomUserRID)
		//                aComboBox.Items.Insert(1, new ComboObject(ollp.Key, newName));
		//            else
		//                aComboBox.Items.Add(new ComboObject(ollp.Key, newName));

		//            if (selectedValue == ollp.Key)
		//                co = new ComboObject(ollp.Key, newName);
		//        }
		//        if (co != null)
		//        {
		//            int idx = aComboBox.Items.IndexOf(co);
		//            aComboBox.SelectedIndex = idx;
		//        }
		//    }
		//    catch
		//    {
		//        throw;
		//    }
		//}
		protected ArrayList BuildOverrideModelList(int aModelRid, int aCustomModelRid)
		{
			ArrayList outList;
			FunctionSecurityProfile userSecurity;
			FunctionSecurityProfile globalSecurity;
			ProfileList overrideModelList;
            //SecurityAdmin secAdmin; //TT#827-MD -jsobek -Allocation Reviews Performance
			string newName;
			ComboObject comboObj;

			try
			{
				outList = new ArrayList();
				userSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsUserOverrideLowLevels);
				globalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsGlobalOverrideLowLevels);

				overrideModelList = OverrideLowLevelProfile.LoadAllProfiles(aModelRid, _SAB.ClientServerSession.UserRID, globalSecurity.AllowView, userSecurity.AllowView, aCustomModelRid);

                //secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance

				foreach (OverrideLowLevelProfile ollp in overrideModelList)
				{
					newName = ollp.Name;

                    // Begin TT#1125 - JSmith - Global/User should be consistent
                    //switch (ollp.User_RID)
                    //{
                    //    case Include.GlobalUserRID:
                    //        newName = newName + " (global)";
                    //        break;

                    //    case Include.CustomUserRID:
                    //        newName = newName + " (custom)";
                    //        break;

                    //    default:
                    //        newName = newName + " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
                    //        break;
                    //}
                    switch (ollp.User_RID)
                    {
                        case Include.GlobalUserRID:
                            break;

                        case Include.CustomUserRID:
                            newName = newName + " (Custom)";
                            break;

                        default:
                            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                            //newName = newName + " (" + secAdmin.GetUserName(ollp.User_RID) + ")";
                            newName = newName + " (" + UserNameStorage.GetUserName(ollp.User_RID) + ")";
                            //End TT#827-MD -jsobek -Allocation Reviews Performance
                            break;
                    }
                    // End TT#1125

					comboObj = new ComboObject(ollp.Key, newName);

					if (ollp.User_RID == Include.CustomUserRID)
					{
						if (outList.Count > 0)
						{
							outList.Insert(1, comboObj);
						}
						else
						{
							outList.Add(comboObj);
						}
					}
					else
					{
						outList.Add(comboObj);
					}
				}

				return outList;
			}
			catch
			{
				throw;
			}
		}

		virtual protected void LoadOverrideModelComboBox(ComboBox aComboBox, int aSelectedValue, int aCustomModelRid)
		{
			ArrayList overrideList;
			int tempIdx;
			int selectedIdx;

			try
			{
				selectedIdx = 0;

				aComboBox.Items.Clear();
				aComboBox.Items.Add(new ComboObject(Include.NoRID, ""));

				overrideList = BuildOverrideModelList(aSelectedValue, aCustomModelRid);

				foreach (ComboObject comboObj in overrideList)
				{
					tempIdx = aComboBox.Items.Add(comboObj);

					if (aSelectedValue == comboObj.Key)
					{
						selectedIdx = tempIdx;
					}
				}

				aComboBox.SelectedIndex = selectedIdx;
			}
			catch
			{
				throw;
			}
		}
		//End TT#155 - JScott - Size Curve Method

		virtual protected void UpdateMethodCustomOLLRid(int methodRid, int customOLLRid)
		{
			MethodBaseData mbData = null;
			try
			{
				if (methodRid != Include.NoRID)
				{
					mbData = new MethodBaseData();
					ClientTransaction.DataAccess.OpenUpdateConnection();
					mbData.UpdateMethodCustomOLLRid(ClientTransaction.DataAccess, methodRid, customOLLRid);
					ClientTransaction.DataAccess.CommitData();
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (methodRid != Include.NoRID)
				{
					if (ClientTransaction.DataAccess.ConnectionIsOpen)
						mbData.CloseUpdateConnection();
				}
			}
		}

		// END Override low level changes

		protected TreeNodeClipboardList CopyNodesToClipboard(System.Windows.Forms.ListView aListView, eProfileType aProfileType, DragDropEffects aAction)
        {
            try
            {

                TreeNodeClipboardList cbList;

                try
                {
                    cbList = new TreeNodeClipboardList(aProfileType);
                    foreach (ListViewItem item in aListView.SelectedItems)
                    {
                        cbList.ClipboardItems.Add(BuildClipboardItem(item, aAction));
                    }

                    CopyObject(typeof(TreeNodeClipboardList), cbList);
                    return cbList;

                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

		protected ColorCodeClipboardList CopyColorsToClipboard(System.Windows.Forms.ListView aListView, eProfileType aProfileType, DragDropEffects aAction)
        {
            try
            {

                ColorCodeClipboardList ccList;

                try
                {
                    ccList = new ColorCodeClipboardList();
                    foreach (ListViewItem item in aListView.SelectedItems)
                    {
                        ccList.ClipboardItems.Add(BuildClipboardItem(item, aAction));
                    }

                    CopyObject(typeof(ColorCodeClipboardList), ccList);
                    return ccList;

                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

		protected ProductCharacteristicClipboardList CopyProductCharacteristicsToClipboard(System.Windows.Forms.ListView aListView, eProfileType aProfileType, DragDropEffects aAction)
        {
            try
            {

                ProductCharacteristicClipboardList pcList;

                try
                {
                    pcList = new ProductCharacteristicClipboardList();
                    foreach (ListViewItem item in aListView.SelectedItems)
                    {
                        pcList.ClipboardItems.Add(BuildClipboardItem(item, aAction));
                    }

                    CopyObject(typeof(ProductCharacteristicClipboardList), pcList);
                    return pcList;

                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

		protected HierarchyNodeClipboardList CopyHierarchyNodesToClipboard(System.Windows.Forms.ListView aListView, eProfileType aProfileType, DragDropEffects aAction)
        {
            try
            {

                HierarchyNodeClipboardList hnList;

                try
                {
                    hnList = new HierarchyNodeClipboardList();
                    foreach (ListViewItem item in aListView.SelectedItems)
                    {
                        hnList.ClipboardItems.Add(BuildClipboardItem(item, aAction));
                    }

                    CopyObject(typeof(HierarchyNodeClipboardList), hnList);
                    return hnList;

                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }

        protected void CopyObject(Type type, object data)
        {
            IDataObject ido = new DataObject();
            ido.SetData(type, data);
            Clipboard.SetDataObject(ido, true);
        }

		virtual protected ClipboardProfileBase BuildClipboardItem(System.Windows.Forms.ListViewItem aItem, DragDropEffects aAction)
        {
            return null;
        }

        //Begin TT#866 - JSmith - Export of multiple sets has the incorrect data in the store columns
        protected void TurnRedrawOff()
        {
            SendMessage(this.Handle, WM_SETREDRAW, false, IntPtr.Zero);
        }

        protected void TurnRedrawOn()
        {
            SendMessage(this.Handle, WM_SETREDRAW, true, IntPtr.Zero);
        }
        //End TT#866

//		/// <summary>
//		/// Recursively disposes all controls in the form
//		/// </summary>
//		/// <param name="aControls">The ControlCollection to recursively dispose</param>
//		private void DisposeControls(System.Windows.Forms.Control.ControlCollection aControls)
//		{
//			try
//			{
//				foreach (Control control in aControls)
//				{
//					if (control != null)
//					{
//						DisposeControls(control.Controls);
//						// if the control has a context menu, dispose of the context menu first to remove reference
//						if (control.ContextMenu != null)
//						{
//							control.ContextMenu.Dispose();
//						}
//						// Clear DataSource if control has property
//						if (control.GetType().GetProperty("DataSource") != null)
//						{
//							PropertyInfo pi = control.GetType().GetProperty("DataSource");
//							pi.SetValue(control, null, null);
//						}
//	
//						control.Dispose();
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}

        //BEGIN TT#110-MD-VStuart - In Use Tool
        // This is a duplicate from C:\SCMVS2010\Working 5.x In Use Tool UC9\Windows\MIDFormBase.cs
        /// <summary>
        /// This is the base object from which the the InUse Info tool is called from.
        /// It's intent is to allow users to be able to find out what objects are in use by other objects.
        /// </summary>
        /// <param name="userRids">This is the list of RIDs we are investigating.</param>
        /// <param name="myEnum">This is the eprofileType that we are investigating.</param>
        /// <param name="itemTitle">This is the title of inquiry.</param>
        /// <param name="display">Indicates that the InUse Dialog should be displayed.</param>
        /// <param name="inQuiry">Indicates if this just an user inquiry or is a mandatory check.</param>
        public void DisplayInUseForm(ArrayList userRids, eProfileType myEnum, string itemTitle, ref bool display, bool inQuiry)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            myfrm.ResolveInUseData(ref display, inQuiry);
            if (display == true
                && MIDEnvironment.isWindows)
            { myfrm.ShowDialog(); }
        }

        public void DisplayInUseForm(ArrayList userRids, eProfileType myEnum, string itemTitle, bool inQuiry, out bool dialogShown)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            bool display = false;
            bool showDialog = false;
            myfrm.ResolveInUseData(ref display, inQuiry, true, out showDialog);
            dialogShown = showDialog;
            if (showDialog == true
                && MIDEnvironment.isWindows)
            { myfrm.ShowDialog(); }
        }
        //END TT#110-MD-VStuart - In Use Tool
        public void DisplayInUseFormAndShowProgress(ArrayList userRids, eProfileType myEnum, string itemTitle, bool inQuiry, out bool dialogShown)
        {
            if (itemTitle == null) throw new ArgumentNullException("itemTitle");
            var myInfo = new InUseInfo(userRids, myEnum, itemTitle);
            var myfrm = new InUseDialog(myInfo);
            bool display = false;
            bool showDialog = false;
            myfrm.ResolveInUseDataAndShowProgress(ref display, inQuiry, true, out showDialog, SharedRoutines.ProgressBarOptions_Get());


            dialogShown = showDialog;
            if (showDialog == true)
            { myfrm.ShowDialog(); }
        }
       

        //BEGIN TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool
        /// <summary>
        /// Checks for InUse during a delete.
        /// </summary>
        /// <param name="type">The model eProfileType in question.</param>
        /// <param name="rid">The model in question.</param>
        /// <param name="inQuiry2">False if this is a mandatory request.</param>
        protected bool CheckInUse(eProfileType type, int rid, bool inQuiry2)
        {
            bool isInUse = true;
            if (rid != Include.NoRID)
            {
                _ridList = new ArrayList();
                _ridList.Add(rid);
                //string inUseTitle = Regex.Replace(type.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(type); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                bool inQuiry = inQuiry2;
                DisplayInUseForm(_ridList, type, inUseTitle, inQuiry, out isInUse);
            }
            return isInUse;
        }
        //END   TT#641-MD-VStuart-Size Curve Delete Needs to use new In Use Tool

		#region Exception Handling
		/// <summary>
		/// Handle exception.
		/// </summary>
		/// <param name="ex">The exception</param>
		/// <remarks>Uses this.name for module.  Logs all inner exceptions.</remarks>
		public void HandleException(Exception ex)
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
//			_SAB.ClientServerSession.Audit.Log_Exception(ex, moduleName, eExceptionLogging.logAllInnerExceptions);
//			// show only the last since it should be real error
//			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
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
//			_SAB.ClientServerSession.Audit.Log_Exception(ex, moduleName, eExceptionLogging.logAllInnerExceptions);
//			// show only the last since it should be real error
//			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
//			if (rethrow)
//			{
//				throw;
//			}
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
//			_SAB.ClientServerSession.Audit.Log_Exception(ex, this.Name, logOnlyInnerMostException);
//			// show only the last since it should be real error
//			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + this.Name);
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
//			_SAB.ClientServerSession.Audit.Log_Exception(ex, this.Name, logOnlyInnerMostException);
//			// show only the last since it should be real error
//			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + this.Name);
//			if (rethrow)
//			{
//				throw;
//			}
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
//			_SAB.ClientServerSession.Audit.Log_Exception(ex, moduleName, logOnlyInnerMostException);
//			// show only the last since it should be real error
//			MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
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
                // begin TT#370 Build Packs Enhancement
                MIDException midEx = ex as MIDException;
                if (midEx.ExpandMidErrorMessage)
                {
                    ex =
                        new MIDException(
                            midEx.ErrorLevel,
                            midEx.ErrorNumber,
                            MIDText.GetTextOnly(midEx.ErrorNumber),
                            midEx.InnerException);                     
                }
                // end TT#370 Build Packs Enhancement
				HandleMIDException((MIDException)ex);
			}
			else
			{
                if (MIDEnvironment.isWindows)
                {
                    MessageBox.Show(ex.ToString(), "MIDFormBase.cs - " + moduleName);
                }
                else
                {
                    rethrow = true;
                }
			}
			this._exceptionCaught = true;
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
            if (MIDEnvironment.isWindows)
            {
                MessageBox.Show(this, Msg, Title,
                buttons, icon);
            }
            else
            {
                throw MIDexc;
            }
		}
		#endregion Exception Handling

		#endregion Methods

		#region IFormBase Members
		virtual public void ICut()
		{
			try
			{
//				throw new Exception("Can not call base method");
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		virtual public void ICopy()
		{
			try
			{
//				throw new Exception("Can not call base method");
				
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		virtual public void IPaste()
		{
			try
			{
//				throw new Exception("Can not call base method");

			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}	

		public void IClose()
		{
			try
			{
				// BEGIN MID Track #2670 - add Cancel option to Save Changes dialog
				_ignoreWindowState = true;
				// END MID Track #2670 
				this.Close();

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
                //				throw new Exception("Save code goes here");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

		virtual public void ISaveAs()
		{
			try
			{
//				throw new Exception("Can not call base method");

			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		// BEGIN MID Track #5170 - JSmith - Model enhancements
		virtual public void INew()
		{
			try
			{

			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		// End MID Track #5170

		virtual public void IDelete()
		{
			try
			{
//				throw new Exception("Can not call base method");

			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		virtual public void IRefresh()
		{
			try
			{
//				throw new Exception("Can not call base method");

			}		
			catch(Exception ex)
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

        virtual public void IReplace()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        virtual public void IExport()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        virtual public void IUndo()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        virtual public void IQuickFilter()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // BEGIN MID Track #5006 - Add Theme to Tools menu 
        virtual public void ITheme()
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        // END MID Track #5006
		#endregion

        #region Menu

        virtual protected void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                switch (e.Tool.Key)
                {
                    case Include.btNew:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            INew();
                        }
                        break;

                    case Include.btClose:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IClose();
                        }
                        break;
                    case Include.btSave:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            ISave();
                        }
                        break;
                    case Include.btSaveAs:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            ISaveAs();
                        }
                        break;
                    case Include.btCut:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            ICut();
                        }
                        break;
                    case Include.btCopy:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            ICopy();
                        }
                        break;
                    case Include.btPaste:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IPaste();
                        }
                        break;
                    case Include.btDelete:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        if (ActiveControl != null)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IDelete();
                        }
                        break;
                    case Include.btExport:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        if (ActiveControl != null)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IExport();
                        }
                        break;
                    case Include.btUndo:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IUndo();
                        }
                        break;
                    case Include.btFind:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IFind();
                        }
                        break;
                    case Include.btReplace:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IReplace();
                        }
                        break;
                    case Include.btQuickFilter:
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            IQuickFilter();
                        }
                        break;
                    case Include.btTheme:                   // MID Track #5006 - Add Theme to Tools menu 
                        // Begin TT#2598 - JSmith - Save As button does not work
                        //if (ActiveControl != null)
                        if (_isActivated)
                        // End TT#2598 - JSmith - Save As button does not work
                        {
                            ITheme();
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void BuildFileMenu()
        {
            try
            {
                AddMenuItem(eMIDMenuItem.FileClose);
                AddMenuItem(eMIDMenuItem.FileSave);
                AddMenuItem(eMIDMenuItem.FileSaveAs);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void AddMenuItem(eMIDMenuItem aMenuItem)
        {
            try
            {
                if (!_menuItems.Contains(aMenuItem))
                {

                    switch (aMenuItem)
                    {
                        case eMIDMenuItem.FileNew:
                            AddFileMenu_New();
                            break;
                        case eMIDMenuItem.FileClose:
                            AddFileMenu_Close();
                            break;
                        case eMIDMenuItem.FileSave:
                            AddFileMenu_Save();
                            break;
                        case eMIDMenuItem.FileSaveAs:
                            AddFileMenu_SaveAs();
                            break;
                        case eMIDMenuItem.FileExport:
                            AddFileMenu_Export();
                            break;
                        case eMIDMenuItem.EditCut:
                            AddEditMenu_Cut();
                            break;
                        case eMIDMenuItem.EditCopy:
                            AddEditMenu_Copy();
                            break;
                        case eMIDMenuItem.EditPaste:
                            AddEditMenu_Paste();
                            break;
                        case eMIDMenuItem.EditDelete:
                            AddEditMenu_Delete();
                            break;
                        case eMIDMenuItem.EditClear:
                            AddEditMenu_Clear();
                            break;
                        case eMIDMenuItem.EditUndo:
                            AddEditMenu_Undo();
                            break;
                        case eMIDMenuItem.EditFind:
                            AddEditMenu_Find();
                            break;
                        case eMIDMenuItem.EditReplace:
                            AddEditMenu_Replace();
                            break;
                        case eMIDMenuItem.ToolsQuickFilter:
                            AddToolsMenu_QuickFilter();
                            break;
                        case eMIDMenuItem.ToolsTheme:           // MID Track #5006 - Add Theme to Tools menu
                            AddToolsMenu_Theme();
                            break;

                    }
                    _menuItems.Add(aMenuItem, null);
                }

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void RemoveMenuItem(object aSource, eMIDMenuItem aMenuItem)
        {
            try
            {
                if (_menuItems.Contains(aMenuItem))
                {
                    string menuItem = Include.GetMenuItem(aMenuItem);

                    if (menuItem != string.Empty)
                    {
                        //if (utmMain.Tools.Contains(menuItem))
                        //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Visible = false;
                        }
                        catch
                        {
                        }
                        //}
                    }
                }
                else
                {
                    _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Remove);
                }
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
                if (_menuItems.Contains(aMenuItem))
                {
                    string menuItem = Include.GetMenuItem(aMenuItem);
                    
                    if (menuItem != string.Empty)
                    {
                        //if (utmMain.Tools.Contains(menuItem))
                        //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Visible = false;
                        }
                        catch
                        {
                        }
                        //}
                    }
                }
                else
                {
                    _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Hide);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ShowMenuItem(object aSource, eMIDMenuItem aMenuItem)
        {
            try
            {
                if (_menuItems.Contains(aMenuItem))
                {
                    string menuItem = Include.GetMenuItem(aMenuItem);
                    
                    if (menuItem != string.Empty)
                    {
                        //if (utmMain.Tools.Contains(menuItem))
                        //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Visible = true;
                        }
                        catch
                        {
                        }
                        //}
                    }
                }
                else
                {
                    _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Show);
                }
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
                if (_menuItems.Contains(aMenuItem))
                {
                    string menuItem = Include.GetMenuItem(aMenuItem);
                    
                    if (menuItem != string.Empty)
                    {
                        //if (utmMain.Tools.Contains(menuItem))
                        //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Enabled = true;
                        }
                        catch
                        {
                        }
                        //}
                    }
                }
                else
                {
                    _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Enable);
                }
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
                if (_menuItems.Contains(aMenuItem))
                {
                    string menuItem = Include.GetMenuItem(aMenuItem);
                    
                    if (menuItem != string.Empty)
                    {
                        //if (utmMain.Tools.Contains(menuItem))
                        //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Enabled = false;
                        }
                        catch
                        {
                        }
                        //}
                    }
                }
                else
                {
                    _SAB.MIDMenuEvent.ChangeMenu(aSource, null, aMenuItem, eMIDMenuAction.Disable);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private bool MenuItemExists(string aMenuItem)
        {
            //if (utmMain.Tools.Contains(menuItem))
            //{
            // contains doesn't work so swallow any error
            try
            {
                utmMain.Tools[aMenuItem].SharedProps.Visible = true;
                return true;
            }
            catch
            {
            }
            return false;
            //}
        }

        private void AddFileMenu_New()
        {
            try
            {
                string menuItem = Include.btNew;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btNew = new ButtonTool(menuItem);
                    btNew.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_New);
                    btNew.SharedProps.Shortcut = Shortcut.CtrlN;
                    btNew.SharedProps.MergeOrder = 5;
                    utmMain.Tools.Add(btNew);

                    _fileMenuTool.Tools.Add(btNew);

                    //if (utmMain.Tools.Contains(Include.btClose))
                    //{
                        // contains doesn't work so swallow any error
                        try
                        {
                            _fileMenuTool.Tools[Include.btClose].InstanceProps.IsFirstInGroup = true;
                        }
                        catch
                        {
                        }
                    //}
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddFileMenu_Save()
        {
            try
            {
                 string menuItem = Include.btSave;
                 if (!MenuItemExists(menuItem))
                 {
                     ButtonTool btSave = new ButtonTool(menuItem);
                     btSave.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Save);
                     btSave.SharedProps.MergeOrder = 10;
                     btSave.SharedProps.Shortcut = Shortcut.CtrlS;
                     btSave.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.SaveImage);
                     this.utmMain.Tools.Add(btSave);

                     _fileMenuTool.Tools.Add(btSave);

                     _fileMenuTool.Tools[Include.btSave].InstanceProps.IsFirstInGroup = true;
                 }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddFileMenu_SaveAs()
        {
            try
            {
                string menuItem = Include.btSaveAs;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btSaveAs = new ButtonTool(menuItem);
                    btSaveAs.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_SaveAs);
                    btSaveAs.SharedProps.MergeOrder = 10;
                    utmMain.Tools.Add(btSaveAs);

                    _fileMenuTool.Tools.Add(btSaveAs);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddFileMenu_Export()
        {
            try
            {
                string menuItem = Include.btExport;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btExport = new ButtonTool(menuItem);
                    btExport.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Export);
                    btExport.SharedProps.Shortcut = Shortcut.CtrlE;
                    btExport.SharedProps.MergeOrder = 5;
                    utmMain.Tools.Add(btExport);

                    _fileMenuTool.Tools.Add(btExport);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddFileMenu_Close()
        {
            try
            {
                string menuItem = Include.btClose;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btClose = new ButtonTool(menuItem);
                    btClose.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Close);
                    btClose.SharedProps.MergeOrder = 10;
                    this.utmMain.Tools.Add(btClose);

                    _fileMenuTool.Tools.Add(btClose);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildEditMenu()
        {
            try
            {
                AddMenuItem(eMIDMenuItem.EditCut);
                AddMenuItem(eMIDMenuItem.EditCopy);
                AddMenuItem(eMIDMenuItem.EditPaste);
                AddMenuItem(eMIDMenuItem.EditDelete);
                AddMenuItem(eMIDMenuItem.EditClear);
                HideMenuItem(this, eMIDMenuItem.EditClear);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Cut()
        {
            try
            {
                string menuItem = Include.btCut;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btCut = new ButtonTool(menuItem);
                    btCut.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Cut);
                    btCut.SharedProps.MergeOrder = 10;
                    btCut.SharedProps.Shortcut = Shortcut.CtrlX;
                    btCut.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.CutImage);
                    utmMain.Tools.Add(btCut);
                    _editMenuTool.Tools.Add(btCut);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Copy()
        {
            try
            {
                string menuItem = Include.btCopy;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btCopy = new ButtonTool(menuItem);
                    btCopy.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
                    btCopy.SharedProps.MergeOrder = 10;
                    btCopy.SharedProps.Shortcut = Shortcut.CtrlC;
                    btCopy.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.CopyImage);
                    utmMain.Tools.Add(btCopy);
                    _editMenuTool.Tools.Add(btCopy);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Paste()
        {
            try
            {
                string menuItem = Include.btPaste;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btPaste = new ButtonTool(menuItem);
                    btPaste.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Paste);
                    btPaste.SharedProps.MergeOrder = 10;
                    btPaste.SharedProps.Shortcut = Shortcut.CtrlV;
                    btPaste.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.PasteImage);
                    this.utmMain.Tools.Add(btPaste);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Delete()
        {
            try
            {
                string menuItem = Include.btDelete;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btDelete = new ButtonTool(menuItem);
                    btDelete.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
                    btDelete.SharedProps.MergeOrder = 10;
                    btDelete.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
                    utmMain.Tools.Add(btDelete);
                    _editMenuTool.Tools.Add(btDelete);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Clear()
        {
            try
            {
                string menuItem = Include.btClear;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btClear = new ButtonTool(menuItem);
                    btClear.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Clear);
                    btClear.SharedProps.MergeOrder = 20;
                    utmMain.Tools.Add(btClear);
                    _editMenuTool.Tools.Add(btClear);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Undo()
        {
            try
            {
                string menuItem = Include.btUndo;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btUndo = new ButtonTool(menuItem);
                    btUndo.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Undo);
                    btUndo.SharedProps.Shortcut = Shortcut.CtrlZ;
                    btUndo.SharedProps.MergeOrder = 0;
                    btUndo.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.UndoImage);
                    utmMain.Tools.Add(btUndo);
                    _editMenuTool.Tools.Add(btUndo);

                    _editMenuTool.Tools[Include.btUndo].InstanceProps.IsFirstInGroup = true;

                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Find()
        {
            try
            {
                string menuItem = Include.btFind;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btFind = new ButtonTool(menuItem);
                    btFind.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Find);
                    btFind.SharedProps.Shortcut = Shortcut.CtrlF;
                    btFind.SharedProps.MergeOrder = 20;
                    btFind.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FindImage);
                    utmMain.Tools.Add(btFind);
                    _editMenuTool.Tools.Add(btFind);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddEditMenu_Replace()
        {
            try
            {
                string menuItem = Include.btReplace;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btReplace = new ButtonTool(menuItem);
                    btReplace.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Replace);
                    btReplace.SharedProps.Shortcut = Shortcut.CtrlH;
                    btReplace.SharedProps.MergeOrder = 20;
                    btReplace.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.ReplaceImage);
                    utmMain.Tools.Add(btReplace);
                    _editMenuTool.Tools.Add(btReplace);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void AddToolsMenu_QuickFilter()
        {
            try
            {
                string menuItem = Include.btQuickFilter;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btQuickFilter = new ButtonTool(menuItem);
                    btQuickFilter.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Quick_Filter);
                    btQuickFilter.SharedProps.Shortcut = Shortcut.CtrlQ;
                    btQuickFilter.SharedProps.MergeOrder = 13;
                    utmMain.Tools.Add(btQuickFilter);
                    _toolsMenuTool.Tools.Add(btQuickFilter);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // BEGIN MID Track #5006 - Add Theme to Tools menu
        private void AddToolsMenu_Theme()
        {
            try
            {
                string menuItem = Include.btTheme;
                if (!MenuItemExists(menuItem))
                {
                    ButtonTool btTheme = new ButtonTool(menuItem);
                    //btTheme.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Theme);
                    btTheme.SharedProps.Caption = "T&heme";
                    btTheme.SharedProps.Shortcut = Shortcut.CtrlH;
                    btTheme.SharedProps.MergeOrder = 0;
                    btTheme.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.ThemeImage);
                    utmMain.Tools.Add(btTheme);
                    _toolsMenuTool.Tools.Add(btTheme);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // END MID Track #5006 

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
                //foreach (object o in ((MIDComboBoxEnh)sender).Items)
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
                //foreach (object o in ((MIDComboBoxEnh)sender).Items)
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

        // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
        protected FilterNameCombo GetRemoveFilterRow()
        {
            return new FilterNameCombo(Include.Undefined,
                    Include.GlobalUserRID,
                    //"<" + MIDText.GetTextOnly(eMIDTextCode.lbl_Remove) + ">");
                    "(None)");
        }
        // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

        #endregion
 
    }

	public class BadDataInClipboardException : Exception
	{
		
	}
}
