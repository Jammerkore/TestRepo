using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Globalization;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Shared;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for WorkflowMethodFormBase.
	/// </summary>
	public class WorkflowMethodFormBase : MIDFormBase
	{
		private System.ComponentModel.IContainer components;
		
		// add event to update explorer when node is changed
		public delegate void PropertyChangeEventHandler(object source, PropertyChangeEventArgs e);
		public event PropertyChangeEventHandler OnPropertyChangeHandler;

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		// event to process methods on assortment headers & placeholders
		//public delegate void ProcessMethodOnAssortmentEventHandler(object source, ProcessMethodOnAssortmentEventArgs e);
		//public event ProcessMethodOnAssortmentEventHandler OnProcessMethodOnAssortmentEvent;
		// END TT#217-MD - stodd - unable to run workflow methods against assortment

		#region Fields
		//=======
		// FIELDS
		//=======
		private ExplorerAddressBlock _EAB;
		private ApplicationBaseMethod _ABM;
		private ApplicationBaseWorkFlow _ABW;
		//Begin TT#155 - JScott - Size Curve Method
		private string _workflowMethodNameLabel = string.Empty;
		//End TT#155 - JScott - Size Curve Method
		private string _workflowMethodName;
		private string _workflowMethodDescription;
		private RadioButton _userRadioButton;
		private RadioButton _globalRadioButton;

		private bool _nameChanged = false;
		private bool _nameValid = true;
		private string _nameMessage;
		private bool _descriptionValid = true;
		private string _descriptionMessage;
		private bool _userGlobalValid = true;
		private string _userGlobalMessage;
		private int _workflowMethodRID;
		private eWorkflowMethodIND _workflowMethodIND;
		private eLockStatus _lockStatus;
//		private DataRow leveldr;
		private DataTable _merchandiseDataTable = null;
		private HierarchyProfile _hp;
		private DataTable _dtActions = null;
		private DataTable _dtComponents = null;
		private DataTable _dtMethods = null;
		protected System.Windows.Forms.Button btnClose;
		protected System.Windows.Forms.Button btnSave;
		protected System.Windows.Forms.Button btnProcess;
		protected System.Windows.Forms.Panel pnlGlobalUser;
		protected System.Windows.Forms.RadioButton radGlobal;
		protected System.Windows.Forms.RadioButton radUser;
		protected System.Windows.Forms.TextBox txtDesc;
		protected System.Windows.Forms.TextBox txtName;
		protected System.Windows.Forms.Label lblName;
		MIDWorkflowMethodTreeNode _explorerNode = null;
		private eMIDTextCode _formName;
		private eWorkflowMethodType _workflowMethodType;
		private FunctionSecurityProfile _userSecurity = null;
		private FunctionSecurityProfile _globalSecurity = null;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private FunctionSecurityProfile _storeUserAttrSecLvl;
        // End Track #4872

		private bool _inDesigner = false;
        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        private GridViewData _gridViewData;      
        private UserGridView _userGridView;
        private UltraGrid _saveViewGrid;
        private bool _saveUserGridView = false;
        private bool _showDetailViewOption = false;
        private int _saveAsViewUserRID;
        private int _saveAsDetailViewUserRID;
        private int _viewRID;
        private int _detailViewRID;
        private eLayoutID _layoutID;
        private string _saveAsViewName;
        private string _saveAsDetailViewName;
        private FunctionSecurityProfile _methodViewGlobalSecurity = null;
        private FunctionSecurityProfile _methodViewUserSecurity = null;
        private FunctionSecurityProfile _methodDetailViewGlobalSecurity = null;
        private FunctionSecurityProfile _methodDetailViewUserSecurity = null;
        private MIDRetail.Windows.StyleView _frmStyleView = null;  
        // End TT#231
        private bool _closeOnSave = true; // TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
        // begin TT#1185 - JEllis - Verify ENQ before Update (Enq transaction must be the owner of the headers when they are processed)
        //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
        //ApplicationSessionTransaction _headerEnqueueTransaction = null;
        //// End TT#1163
        // end TT#1185 - JEllis - Verify ENQ before Update (Enq transaction must be the owner of the headers when they are processed)
        #endregion Fields

		// Begin MID Track 4858 - JSmith - Security changes
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

				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
				this.radUser.CheckedChanged -= new System.EventHandler(this.radUser_CheckedChanged);
				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
			}

			base.Dispose( disposing );
		}
		// End MID Track 4858

		#region Constructors
		//=============
		// CONSTRUCTORS
		//=============
		// Base constructor required for Windows Form Designer support
		public WorkflowMethodFormBase()
		{
			// Begin MID Track 4858 - JSmith - Security changes
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			// End MID Track 4858
			_inDesigner = true;
		}

		public WorkflowMethodFormBase(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, eMIDTextCode aFormName, eWorkflowMethodType aWorkflowMethodType)  : base (aSAB)
		{
			// Begin MID Track 4858 - JSmith - Security changes
			InitializeComponent();
			// End MID Track 4858

			if (aSAB == null)
			{
				throw new Exception("SessionAddressBlock is required");
			}
			if (aEAB == null)
			{
				throw new Exception("ExplorerAddressBlock is required");
			}
			_EAB = aEAB;
			_formName = aFormName;
			_workflowMethodType = aWorkflowMethodType;

            // begin TT#1185 - JEllis - Verify ENQ before Update (ENQ Transaction must own the headers at process time)
            //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
            //_headerEnqueueTransaction = aSAB.ApplicationServerSession.CreateTransaction();
            //// End TT#1163
            // end TT#1185 - JEllis - Verify ENQ before Update (ENQ Transaction must own the headers at process time)

			this.Closing +=new CancelEventHandler(WorkflowMethodFormBase_Closing);
		}

		#endregion Constructors

		#region Properties
		//============
		// PROPERTIES
		//============
		/// <summary>
		/// Gets or sets the HierarchyProfile object.
		/// </summary>
		protected HierarchyProfile HP
		{
			get	{return _hp;}	
		}
		/// <summary>
		/// Gets or sets the ApplicationBaseMethod object.
		/// </summary>
		public ApplicationBaseMethod ABM
		{
			get	{return _ABM;}
			set	{_ABM = value;}
		}

		/// <summary>
		/// Gets or sets the ApplicationBaseWorkFlow object.
		/// </summary>
		public ApplicationBaseWorkFlow ABW
		{
			get	{return _ABW;}
			set	{_ABW = value;}
		}

		//Begin TT#155 - JScott - Size Curve Method
		public string WorkflowMethodNameLabel
		{
			get { return _workflowMethodNameLabel; }
			set { _workflowMethodNameLabel = value; }
		}

		//End TT#155 - JScott - Size Curve Method
		/// <summary>
		/// Gets or sets the ExplorerAddressBlock object.
		/// </summary>
		public ExplorerAddressBlock EAB
		{
			get	{return _EAB;}
			set	{_EAB = value;}
		}

		/// <summary>
		/// Gets a DataTable containing a list of actions.
		/// </summary>
		public DataTable DtActions
		{
			get	
			{
				if (_dtActions == null)
				{
					// begin MID Track 4554 AnF Enhancement API Workflow
					//_dtActions = MIDText.GetLabels((int) eAllocationMethodType.GeneralAllocation, (int)eAllocationMethodType.Release);
					_dtActions = MIDText.GetLabels((int) eAllocationMethodType.ApplyAPI_Workflow, (int)eAllocationMethodType.ApplyAPI_Workflow);
					DataTable dt = MIDText.GetLabels((int) eAllocationMethodType.GeneralAllocation, (int)eAllocationMethodType.Release);
					foreach (DataRow dr in dt.Rows)
					{
						_dtActions.Rows.Add(dr.ItemArray);
					}
					// end MID Track 4554 AnF Enhancement API Workflow
					//Begin TT#155 - JScott - Size Curve Method
					dt = MIDText.GetLabels((int)eAllocationMethodType.SizeCurve, (int)eAllocationMethodType.BuildPacks);
					foreach (DataRow dr in dt.Rows)
					{
						_dtActions.Rows.Add(dr.ItemArray);
					}
					//End TT#155 - JScott - Size Curve Method

                    // Begin TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
                    dt = MIDText.GetLabels((int)eAllocationMethodType.ReapplyTotalAllocation, (int)eAllocationMethodType.ReapplyTotalAllocation);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#785  
                    // Begin TT#843 - New Size Constraint Balance
                    //dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceSizeWithConstraints, (int)eAllocationMethodType.BalanceSizeWithConstraints);  // TT#794 - New Size Balance for Wet Seal
                    //dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceSizeWithConstraints, (int)eAllocationMethodType.BalanceSizeBilaterally);    // TT#794 - New Size Balance for Wet Seal
					dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceSizeWithConstraints, (int)eAllocationMethodType.BreakoutSizesAsReceivedWithConstraints); // TT#1391 - JEllis - Balance Size WIth Constraints Other Options
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#843 - New Size Constraint Balance 

					// Begin TT#1098 - MD - stodd - add Group Allocation Method to workflow screen - 
                    dt = MIDText.GetLabels((int)eAllocationMethodType.GroupAllocation, (int)eAllocationMethodType.GroupAllocation); 
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#1098 - MD - stodd - add Group Allocation Method to workflow screen - 
                   
                    // Begin TT#1652-MD - RMatelic - DC Carton Rounding 
                    dt = MIDText.GetLabels((int)eAllocationMethodType.DCCartonRounding, (int)eAllocationMethodType.DCCartonRounding);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#1652-MD -

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    dt = MIDText.GetLabels((int)eAllocationMethodType.CreateMasterHeaders, (int)eAllocationMethodType.CreateMasterHeaders);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.DCFulfillment, (int)eAllocationMethodType.DCFulfillment);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#1966-MD - JSmith- DC Fulfillment
                    
                    // Begin TT#1334-MD - stodd - Balance to VSW Action
                    dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceToVSW, (int)eAllocationMethodType.BalanceToVSW);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    // End TT#1334-MD - stodd - Balance to VSW Action
					
                    //Begin TT#1108 - Add Cancel Allocation options to the Allocation Workflow - apicchetti - 02/08/2011
                    dt = MIDText.GetLabels((int)eAllocationActionType.BackoutAllocation, (int)eAllocationActionType.BackoutAllocation);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                    //End TT#1108 - Add Cancel Allocation options to the Allocation Workflow - apicchetti - 02/08/2011

				}
				return _dtActions;
			}
		}

		/// <summary>
		/// Gets a DataTable containing a list of components.
		/// </summary>
		public DataTable DtComponents
		{
			get	
			{
				if (_dtComponents == null)
				{
					if (SAB.ApplicationServerSession.GlobalOptions.AppConfig.SizeInstalled)
					{
						_dtComponents = MIDText.GetLabels((int) eComponentType.Total, (int)eComponentType.ColorAndSize);
					}
					else
					{
						_dtComponents = MIDText.GetLabels((int) eComponentType.Total, (int)eComponentType.SpecificColor);
					}
				}
				return _dtComponents;
			}
		}

		/// <summary>
		/// Gets a DataTable containing a list of methods.
		/// </summary>
		public DataTable DtMethods
		{
			get	
			{
				if (_dtMethods == null)
				{
//Begin Modification - KJohnson - Rollup
//Begin Modification - KJohnson - Global Unlock
//Begin Modification - JScott - Export Method - Part 10
//					_dtMethods = MIDText.GetLabels((int) eMethodType.OTSPlan, (int)eMethodType.ForecastModifySales);
                    _dtMethods = MIDText.GetLabels((int)eMethodType.OTSPlan, (int)eMethodType.Rollup);
//End Modification - JScott - Export Method - Part 10
//End Modification - KJohnson - Global Unlock
//End Modification - KJohnson - Rollup
                }
				return _dtMethods;
			}
		}
		
		/// <summary>
		/// Gets or sets the workflow or method name.
		/// </summary>
		public string WorkflowMethodName
		{
			get	{return _workflowMethodName;}
			set	{_workflowMethodName = value.Trim();}
		}

		/// <summary>
		/// Gets or sets the workflow or method description.
		/// </summary>
		public string WorkflowMethodDescription
		{
			get	{return _workflowMethodDescription;}
			set	{_workflowMethodDescription = value.Trim();}
		}

		/// <summary>
		/// Gets or sets the control used for the global radio button.
		/// </summary>
		public RadioButton GlobalRadioButton
		{
			get	{return _globalRadioButton;}
			set	{_globalRadioButton = value;}
		}

		/// <summary>
		/// Gets or sets the control used for the user radio button.
		/// </summary>
		public RadioButton UserRadioButton
		{
			get	{return _userRadioButton;}
			set	{_userRadioButton = value;}
		}

		/// <summary>
		/// Gets or sets flag identifying if the name has changes so the explorer can be updated.
		/// </summary>
		public bool NameChanged
		{
			get	{return _nameChanged;}
			set	{_nameChanged = value;}
		}

		/// <summary>
		/// Gets a boolean identifying if the method name is valid.
		/// </summary>
		public bool WorkflowMethodNameValid
		{
			get	{return _nameValid;}
		}

		/// <summary>
		/// Gets a message associated with the method name.
		/// </summary>
		public string WorkflowMethodNameMessage
		{
			get	{return _nameMessage;}
		}

		/// <summary>
		/// Gets a boolean identifying if the method description is valid.
		/// </summary>
		public bool WorkflowMethodDescriptionValid
		{
			get	{return _descriptionValid;}
		}

		/// <summary>
		/// Gets a message associated with the method description.
		/// </summary>
		public string WorkflowMethodDescriptionMessage
		{
			get	{return _descriptionMessage;}
		}

		/// <summary>
		/// Gets a boolean identifying if the user or global selection is valid.
		/// </summary>
		public bool UserGlobalValid
		{
			get	{return _userGlobalValid;}
		}

		/// <summary>
		/// Gets a message associated with the user or global selection.
		/// </summary>
		public string UserGlobalMessage
		{
			get	{return _userGlobalMessage;}
		}

		/// <summary>
		/// Gets the lock status associated with the method or workflow.
		/// </summary>
		public eLockStatus LockStatus
		{
			get	{return _lockStatus;}
		}

		/// <summary>
		/// Gets a datatable containing Merchandise Information.
		/// </summary>
		protected DataTable MerchandiseDataTable
		{
			get	{return _merchandiseDataTable;}
		}

		/// <summary>
		/// Gets or sets the eMIDTextCode of the form name.
		/// </summary>
		public eMIDTextCode FormName
		{
			get	{return _formName;}
			set	{_formName = value;}
		}

		/// <summary>
		/// Gets or sets the user security profile.
		/// </summary>
		public FunctionSecurityProfile UserSecurity
		{
			get	
			{
				if (_userSecurity == null)
				{
					_userSecurity = new FunctionSecurityProfile(Include.NoRID);
				}
				return _userSecurity;
			}
			set	{_userSecurity = value;}
		}

		/// <summary>
		/// Gets or sets the user global profile.
		/// </summary>
		public FunctionSecurityProfile GlobalSecurity
		{
			get	
			{
				if (_globalSecurity == null)
				{
					_globalSecurity = new FunctionSecurityProfile(Include.NoRID);
				}
				return _globalSecurity;
			}
			set	{_globalSecurity = value;}
		}

		/// <summary>
		/// Gets or sets the user global profile.
		/// </summary>
		public MIDWorkflowMethodTreeNode ExplorerNode
		{
			get	{return _explorerNode;}
			set	{_explorerNode = value;}
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        /// <summary>
        /// Gets or sets the GridViewData object.
        /// </summary>
        public GridViewData GridViewData
        {
            get { return _gridViewData; }
            set { _gridViewData = value; }
        }
        /// <summary>
        /// Gets or sets the UserViewData object.
        /// </summary>
        public UserGridView UserGridView
        {
            get { return _userGridView; }
            set { _userGridView = value; }
        }
        /// <summary>
        /// Gets or sets the UserViewData object.
        /// </summary>
        public UltraGrid SaveViewGrid
        {
            get { return _saveViewGrid; }
            set { _saveViewGrid = value; }
        }
        /// <summary>
        /// Gets or sets flag whether to save a grid view for a user
        /// </summary>
        public bool SaveUserGridView 
        {
            get { return _saveUserGridView; }
            set { _saveUserGridView = value; }
        }
        /// <summary>
        /// Gets or sets flag whether to show the Save Detail grid option
        /// </summary>
        public bool ShowDetailViewOption
        {
            get { return _showDetailViewOption; }
            set { _showDetailViewOption = value; }
        }
        /// <summary>
        /// Gets or sets a Grid View RID.
        /// </summary>
        public int ViewRID
        {
            get { return _viewRID; }
            set { _viewRID = value; }
        }
        /// <summary>
        /// Gets or sets a Detail Grid View RID.
        /// </summary>
        public int DetailViewRID
        {
            get { return _detailViewRID; }
            set { _detailViewRID = value; }
        }
        /// <summary>
        /// Gets or sets a eLayoutID.
        /// </summary>
        public eLayoutID LayoutID
        {
            get { return _layoutID; }
            set { _layoutID = value; }
        }
        /// <summary>
        /// Gets or sets the Grid View name.
        /// </summary>
        public string SaveAsViewName
        {
            get { return _saveAsViewName; }
            set { _saveAsViewName = value; }
        }
        /// <summary>
        /// Gets or sets the user RID for the Grid View.
        /// </summary>
        public int SaveAsViewUserRID
        {
            get { return _saveAsViewUserRID; }
            set { _saveAsViewUserRID = value; }
        }
        /// <summary>
        /// Gets or sets the Detail Grid View name.
        /// </summary>
        public string SaveAsDetailViewName
        {
            get { return _saveAsDetailViewName; }
            set { _saveAsDetailViewName = value; }
        }
        /// <summary>
        /// Gets or sets the user RID for the Detail Grid View.
        /// </summary>
        public int SaveAsDetailViewUserRID
        {
            get { return _saveAsDetailViewUserRID; }
            set { _saveAsDetailViewUserRID = value; }
        }
        /// <summary>
        /// Gets or sets the grid view global profile.
        /// </summary>
        public FunctionSecurityProfile MethodViewGlobalSecurity
        {
            get
            {
                if (_methodViewGlobalSecurity == null)
                {
                    _methodViewGlobalSecurity = new FunctionSecurityProfile(Include.NoRID);
                }
                return _methodViewGlobalSecurity;
            }
            set { _methodViewGlobalSecurity = value; }
        }
        /// <summary>
        /// Gets or sets the grid view user profile.
        /// </summary>
        public FunctionSecurityProfile MethodViewUserSecurity
        {
            get
            {
                if (_methodViewUserSecurity == null)
                {
                    _methodViewUserSecurity = new FunctionSecurityProfile(Include.NoRID);
                }
                return _methodViewUserSecurity;
            }
            set { _methodViewUserSecurity = value; }
        }
        /// <summary>
        /// Gets or sets the grid view global profile.
        /// </summary>
        public FunctionSecurityProfile MethodDetailViewGlobalSecurity
        {
            get
            {
                if (_methodDetailViewGlobalSecurity == null)
                {
                    _methodDetailViewGlobalSecurity = new FunctionSecurityProfile(Include.NoRID);
                }
                return _methodDetailViewGlobalSecurity;
            }
            set { _methodDetailViewGlobalSecurity = value; }
        }
        /// <summary>
        /// Gets or sets the grid view user profile.
        /// </summary>
        public FunctionSecurityProfile MethodDetailViewUserSecurity
        {
            get
            {
                if (_methodDetailViewUserSecurity == null)
                {
                    _methodDetailViewUserSecurity = new FunctionSecurityProfile(Include.NoRID);
                }
                return _methodDetailViewUserSecurity;
            }
            set { _methodDetailViewUserSecurity = value; }
        }
        /// <summary>
        /// Gets or sets the StyleView from from Velocity.   
        /// </summary>
        public MIDRetail.Windows.StyleView FrmStyleView
        {
            get { return _frmStyleView; }
            set { _frmStyleView = value; }
        }
        // End TT#231  
		#endregion Properties

		#region Methods
		//========
		// METHODS
		//========
		
		/// <summary>
		/// Use to get if workflow or method
		/// </summary>
		virtual protected eWorkflowMethodIND WorkflowMethodInd()
		{
			throw new Exception("Can not call base method");
		}
		
		/// <summary>
		/// Use to display the form to create a new workflow or method
		/// </summary>
		/// <param name="aParentNode">The node to which the workflow or method is being added</param>
		/// <param name="aDummy">Dummy field just to differentiate it from the virtual method</param>
		public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode, int aDummy)
		{
			_explorerNode = aParentNode;
			_workflowMethodIND = aParentNode.WorkflowMethodIND;
			NewWorkflowMethod(aParentNode);
			SetObject();
		}
		
		/// <summary>
		/// Use to display the form to create a new workflow or method
		/// </summary>
		/// <param name="aParentNode">The node to which the workflow or method is being added</param>
		virtual public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to display the form to create a new workflow or method
		/// </summary>
		/// <param name="aParentNode">The node to which the workflow or method is being added</param>
		protected void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode, eSecurityFunctions aUserSecurityFunction, eSecurityFunctions aGlobalSecurityFunction)
		{

            SetObject();

			if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
			{
				_ABM.Method_Change_Type = eChangeType.add;
			}
			else
			{
				_ABW.Workflow_Change_Type = eChangeType.add;
			}

			if (ExplorerNode.GlobalUserType == eGlobalUserType.User)
			{
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(aUserSecurityFunction);
			}
			else
			{
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(aGlobalSecurityFunction);
			}

			if (FunctionSecurity.AllowUpdate)
			{
				FunctionSecurity.SetAllowUpdate();
			}
			else
			{
				FunctionSecurity.SetAccessDenied();
			}

			// Begin MID Track #5224 - JSmith - Able to create global when not authorized
			if (ExplorerNode.GlobalUserType == eGlobalUserType.User ||
				ExplorerNode.isMyFolder)
			{
				radUser.Checked = true;
			}
			else
			{
				radGlobal.Checked = true;
			}
//			if (ExplorerNode.GlobalUserType == eGlobalUserType.User)
//			{
//				radUser.Checked = true;
//			}
//			else
//			{
//				radGlobal.Checked = true;
//			}
			// End MID Track #5224

			// Begin MID Track #4971 - JSmith - Calendar selector will not display
			SetReadOnly(true);
			// End MID Track #4971

			Format_Title(eDataState.New, _formName, null);

			// Begin MID Track #5224 - JSmith - Able to create global when not authorized
			if (ExplorerNode.GlobalUserType == eGlobalUserType.User ||
				ExplorerNode.isMyFolder)
			{
				if (!_globalSecurity.AllowUpdate)
				{
					radUser.Enabled = false;
					radGlobal.Enabled = false;
				}
			}
			// End MID Track #5224

			if (FunctionSecurity.AllowExecute)
			{
				btnProcess.Enabled = true;
			}
			else
			{
				btnProcess.Enabled = false;
			}
		}

		/// <summary>
		/// Use to display the form to update an existing workflow or method
		/// </summary>
		/// <param name="aNode">The node being updated</param>
		public void UpdateWorkflowMethod(MIDWorkflowMethodTreeNode aNode, out eLockStatus aLockedStatus)
		{
			_explorerNode = aNode;
			_lockStatus = eLockStatus.ReadOnly;
			_workflowMethodIND = aNode.WorkflowMethodIND;
			_workflowMethodRID = aNode.Key;
			// Begin MID Track #5076 - JSmith - Added conflict to message
			if (_workflowMethodType == eWorkflowMethodType.Method)
			{
				Text = MIDText.GetTextOnly(Convert.ToInt32(aNode.MethodType));
			}
			// End MID Track #5076
			if (aNode.FunctionSecurityProfile.AllowUpdate)
			{
				LockWorkflowMethod(eChangeType.update, aNode, true);
			}
			
			aLockedStatus = _lockStatus;
			if (_lockStatus != eLockStatus.Cancel)
			{
//				// override the security to read if could not get a lock
//				if (_lockStatus == eLockStatus.ReadOnly)
//				{
//					aNode.FunctionSecurity.SetReadOnly();
//				}
				UpdateWorkflowMethod(aNode.Key, aNode.NodeRID, aNode, _lockStatus);
			}
			else
			{
				this.Close();
			}
		}

		/// <summary>
		/// Use to display the form to update an existing workflow or method
		/// </summary>
		/// <param name="aWorkflowMethodRID">The record ID of the workflow or method</param>
		/// <param name="aNodeRID">The record ID of the hierarchy node</param>
		/// <param name="aNode">The node being updated</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		virtual public void UpdateWorkflowMethod(int aWorkflowMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to display the form to create a new workflow or method
		/// </summary>
		protected void UpdateWorkflowMethod(eLockStatus aLockStatus, eSecurityFunctions aUserSecurityFunction, eSecurityFunctions aGlobalSecurityFunction)
		{
			int userRID = Include.NoRID;
			string name = string.Empty;
			bool canUpdateData = true;
            bool canViewData = true;  // TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

			SetObject();

			if (ExplorerNode.GlobalUserType == eGlobalUserType.User)
			{
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(aUserSecurityFunction);
			}
			else
			{
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(aGlobalSecurityFunction);
			}


			if (_workflowMethodType == eWorkflowMethodType.Workflow)
			{
				userRID = _ABW.UserRID;
				_ABW.Workflow_Change_Type = eChangeType.update;
				txtName.Text = _ABW.WorkFlowName;
				txtDesc.Text = _ABW.WorkFlowDescription;
				name = _ABW.WorkFlowName;
			}
			else
			{
				userRID = _ABM.User_RID;
				_ABM.Method_Change_Type = eChangeType.update;
				txtName.Text = _ABM.Name;
				txtDesc.Text = _ABM.Method_Description;
				name = _ABM.Name;
				canUpdateData = _ABM.AuthorizedToUpdate (SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
                // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
                if (canUpdateData)
                {
                    canViewData = _ABM.AuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
                    if (!canViewData)
                    {
                        canUpdateData = false; 
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_updateOverriddenToRead), txtName.Text + " Overridden", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        FunctionSecurity.SetReadOnly();
                        GlobalSecurity.SetReadOnly();
                    }
                }
                // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
			}

			if (userRID == Include.GetGlobalUserRID())
			{
				this.radGlobal.Checked = true;
			}
			else
			{
				this.radUser.Checked = true;
			}

//Begin Track #5142 - JScott - Security changes - Part 2
//			// Begin MID Track #5142 - JSmith - Security changes
////			if (aLockStatus == eLockStatus.ReadOnly)
////			{
////				FunctionSecurity.SetReadOnly();
////			}
//			if (aLockStatus == eLockStatus.ReadOnly &&
//				!canUpdateData)
//			{
//				FunctionSecurity.SetReadOnly();
//			}
//			else if (!canUpdateData)
//			{
////Begin Track #5219 - JScott - Method Read-only when Version is inactive
////				FunctionSecurity.SetDenyUpdate();
//				FunctionSecurity.SetDenyExecute();
////End Track #5219 - JScott - Method Read-only when Version is inactive
//			}
//			// End MID Track #5142

			// BEGIN Track #5849 stodd
			// NOTE: Need to look into having the SetReadOnly() not overlaying the Execute 
			// Authority.
			bool allowExec = FunctionSecurity.AllowExecute;

			if (aLockStatus == eLockStatus.ReadOnly)
			{
				FunctionSecurity.SetReadOnly();
				if (allowExec)
					FunctionSecurity.SetAllowExecute();
			}
			// END Track 5849
			// BEgin Track #5852
			//else if (!canUpdateData)
			//{
			//    FunctionSecurity.SetDenyExecute();
			//}
			// END Track #5852
//End Track #5142 - JScott - Security changes - Part 2

			SetReadOnly(FunctionSecurity.AllowUpdate);

			if (FunctionSecurity.AllowUpdate)
			{
				Format_Title(eDataState.Updatable, _formName, name);
			}
			else
			{
				Format_Title(eDataState.ReadOnly, _formName, name);
			}

			btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);

			ApplyCanUpdate(canUpdateData);	// Issue 4858

			this.radGlobal.Enabled = false;
			this.radUser.Enabled = false;

		}

		// BEGIN Issue 4858 stodd 11.2.2007
		protected void ApplyCanUpdate(bool canUpdateData)
		{
			// Begin MID Track #5142 - JSmith - Security changes
//			if (canUpdateData)
			if (canUpdateData &&
				FunctionSecurity.AllowUpdate)
			// End MID Track #5142
			{
				btnSave.Enabled = true;
                // BEGIN Version 3.0 Conversion RonM 
				//AddMenuOption(Include.btSave);
				//AddMenuOption(Include.btSaveAs);
                AddMenuItem(eMIDMenuItem.FileSave);
                AddMenuItem(eMIDMenuItem.FileSaveAs);
                // END Version 3.0 Conversion
			}
			else
			{
				btnSave.Enabled = false;
                // BEGIN Version 3.0 Conversion RonM 
                //RemoveMenuOption(Include.btSave);
                //RemoveMenuOption(Include.btSaveAs);
                RemoveMenuItem(this, eMIDMenuItem.FileSave);
                // Begin MID Track #5857 - JSmith - Cannot “Save As” to a User Method or Workflow
                //RemoveMenuItem(this, eMIDMenuItem.FileSaveAs);
                if (!_userSecurity.AllowUpdate &&
                    !_globalSecurity.AllowUpdate)
                {
                    RemoveMenuItem(this, eMIDMenuItem.FileSaveAs);
                }
                // End MID Track #5857
                // END Version 3.0 Conversion
			}

			if (canUpdateData &&
				FunctionSecurity.AllowExecute)
			{
				btnProcess.Enabled = true;
			}
			else
			{
				btnProcess.Enabled = false;
			}
		}
		// END Issue 4858 stodd 11.2.2007

		/// <summary>
		/// Use to to delete a workflow or method.
		/// </summary>
		/// <param name="aWorkflowMethodRID">The record ID of the workflow or method</param>
		/// <param name="aNode">The node being updated</param>
		public bool DeleteWorkflowMethod(MIDWorkflowMethodTreeNode aNode)
		{
			try
			{
				_explorerNode = aNode;
				_lockStatus = eLockStatus.ReadOnly;
				_workflowMethodIND = aNode.WorkflowMethodIND;
				_workflowMethodRID = aNode.Key;

				LockWorkflowMethod(eChangeType.delete, aNode, false);
			
				if (_lockStatus != eLockStatus.Cancel)
				{
					//Begin TT#492 - JScott - Received a Database Foreign Key Violation when deleting a Size Curve Method
					//return DeleteWorkflowMethod(aNode.Key);
					if (DeleteWorkflowMethod(aNode.Key))
					{
						return true;
					}
					else
					{
						UnlockWorkflowMethod();
						return false;
					}
					//End TT#492 - JScott - Received a Database Foreign Key Violation when deleting a Size Curve Method
				}
				else
				{
					return false;
				}
			}
			catch(DatabaseForeignKeyViolation keyEx)
			{
				UnlockWorkflowMethod();
				string message = keyEx.ToString();
				throw;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Use to to delete a workflow or method.  
		/// </summary>
		/// <param name="aWorkflowMethodRID"></param>
		virtual public bool DeleteWorkflowMethod(int aWorkflowMethodRID)
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to to rename a workflow or method.
		/// </summary>
		/// <param name="aWorkflowMethodRID">The record ID of the workflow or method</param>
		/// <param name="aNode">The node being updated</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		public bool RenameWorkflowMethod(MIDWorkflowMethodTreeNode aNode, string aNewName)
		{
			_explorerNode = aNode;
			bool renameSuccessful = false;
			_lockStatus = eLockStatus.ReadOnly;
			_workflowMethodIND = aNode.WorkflowMethodIND;
			_workflowMethodRID = aNode.Key;
			LockWorkflowMethod(eChangeType.update, aNode, false);
			
			if (_lockStatus != eLockStatus.Cancel)
			{
				renameSuccessful = RenameWorkflowMethod(aNode.Key, aNewName);
			}

			return renameSuccessful;
		}

		/// <summary>
		/// Use to to rename a workflow or method.  
		/// </summary>
		/// <param name="aWorkflowMethodRID"></param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		virtual public bool RenameWorkflowMethod(int aWorkflowMethodRID, string aNewName)
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to to copy a workflow or method.
		/// </summary>
		/// <param name="aCopyNode">The node being copied</param>
		/// <param name="aToNode">The node being copied to</param>
        // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
        //public bool CopyWorkflowMethod(MIDWorkflowMethodTreeNode aCopyNode, MIDWorkflowMethodTreeNode aToNode, ref int aNewItemKey)
        public bool CopyWorkflowMethod(MIDWorkflowMethodTreeNode aCopyNode, MIDWorkflowMethodTreeNode aToNode, ref int aNewItemKey, ref string aNewItemName)
        // End TT#1167
		{
			try
			{
				_explorerNode = aToNode;
				_lockStatus = eLockStatus.ReadOnly;
				_workflowMethodIND = aCopyNode.WorkflowMethodIND;
				_workflowMethodRID = aCopyNode.Key;
				LockWorkflowMethod(eChangeType.update, aCopyNode, false);
			
				if (_lockStatus != eLockStatus.Cancel)
				{
					// call update to populate object
					UpdateWorkflowMethod(aCopyNode.Key, aCopyNode.NodeRID, aCopyNode, _lockStatus);
                    // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                    //return CopyWorkflowMethod(ref aNewItemKey);
                    return CopyWorkflowMethod(ref aNewItemKey, ref aNewItemName);
                    // End TT#1167
				}
				else
				{
					return false;
				}
			}
			catch(DatabaseForeignKeyViolation keyEx)
			{
				string message = keyEx.ToString();
				throw;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_lockStatus != eLockStatus.Cancel)
				{
					_workflowMethodRID = aCopyNode.Key;
					UnlockWorkflowMethod();
				}
			}
		}

		/// <summary>
		/// Use to to move a workflow or method from user to global or global to user.
		/// </summary>
		/// <param name="aMoveNode">The node being moved</param>
		/// <param name="aToNode">The node being moved to</param>
		public void MoveWorkflowMethod(MIDWorkflowMethodTreeNode aMoveNode, MIDWorkflowMethodTreeNode aToNode)
		{
			try
			{
				_explorerNode = aToNode;
				_lockStatus = eLockStatus.ReadOnly;
				_workflowMethodIND = aMoveNode.WorkflowMethodIND;
				_workflowMethodRID = aMoveNode.Key;
				LockWorkflowMethod(eChangeType.update, aMoveNode, false);
			
				if (_lockStatus != eLockStatus.Cancel)
				{
					// call update to populate object
					UpdateWorkflowMethod(aMoveNode.Key, aMoveNode.NodeRID, aMoveNode, _lockStatus);
					MoveWorkflowMethod();
				}
				else
				{
					return;
				}
			}
			catch(DatabaseForeignKeyViolation keyEx)
			{
				string message = keyEx.ToString();
				throw;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				if (_lockStatus != eLockStatus.Cancel)
				{
					_workflowMethodRID = aMoveNode.Key;
					UnlockWorkflowMethod();
				}
			}
		}

        public void SetMethodReadOnly()
        {
            string name;
            if (_workflowMethodType == eWorkflowMethodType.Workflow)
            {
                name = _ABW.WorkFlowName;
            }
            else
            {
                name = _ABM.Name;
            }
            FunctionSecurity.SetReadOnly();
            SetReadOnly(FunctionSecurity.AllowUpdate);
            Format_Title(eDataState.ReadOnly, _formName, name);
        }

		/// <summary>
		/// Use to to process a workflow or method.
		/// </summary>
		/// <param name="aNode">The node being processed</param>
		public void ProcessWorkflowMethod(MIDWorkflowMethodTreeNode aNode)
		{
            // begin TT#1185 - JEllis - Verify ENQ before update (need to do this in block where ENQ and process happens)
            //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
            //bool headersEnqueued = false;
            //// End TT#1163
            // end TT#1185 - JEllis - Verify ENQ before update (need to do this in block where ENQ and process happens)
			try
			{
				_explorerNode = aNode;
				_lockStatus = eLockStatus.ReadOnly;
				_workflowMethodIND = aNode.WorkflowMethodIND;
				_workflowMethodRID = aNode.Key;
                // Begin TT#2183 - JSmith - Global Workflow Not Able To Be Used By Multiple Users
                //LockWorkflowMethod(eChangeType.update, aNode, false);
                _lockStatus = eLockStatus.ReadOnly;
                // End TT#2183
			
				if (_lockStatus != eLockStatus.Cancel)
				{
                    // begin TT#1185 - JEllis -  Verify ENQ before Update (need to delay this until the transaction that will process headers is created)
                    ProcessWorkflowMethod(aNode.Key);
                    //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
                    ////ProcessWorkflowMethod(aNode.Key);
                    //if (EnqueueHeadersForProcessing())
                    //{
                    //    if (_headerEnqueueTransaction.EnqueueSelectedHeaders())
                    //    {
                    //        headersEnqueued = true;
                    //        ProcessWorkflowMethod(aNode.Key);
                    //    }
                    //    else
                    //    {
                    //        _headerEnqueueTransaction.SAB.MessageCallback.HandleMessage(
                    //            _headerEnqueueTransaction.HeaderEnqueueMessage,
                    //            "Header Lock Conflict",
                    //            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                    //    }
                    //}
                    //else
                    //{
                    //    ProcessWorkflowMethod(aNode.Key);
                    //}
                    //// End TT#1163
                    // end TT#1185 - JEllis - Verify ENQ before Update (need to delay this until the transaction that will process headers is created)
				}
				else
				{
					return;
				}
			}
			catch(DatabaseForeignKeyViolation keyEx)
			{
				string message = keyEx.ToString();
				throw;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
			finally
			{
				UnlockWorkflowMethod();
                
                // begin TT#1185 - JEllis - Verify ENQ before Update (need to do this in the block where the ENQ is established)
                //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
                //if (headersEnqueued)
                //{
                //    _headerEnqueueTransaction.DequeueHeaders(); 
                //}
                //// End TT#1163
                // end TT#1185 - JEllis - Verify ENQ before Update (need to do this in the block where the ENQ is established)
			}
		}

		/// <summary>
		/// Use to to delete a workflow or method.  
		/// </summary>
		/// <param name="aWorkflowMethodRID"></param>
		virtual public void ProcessWorkflowMethod(int aWorkflowMethodRID)
		{
			throw new Exception("Can not call base method");
		}

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Use to invoke the method to call the processing of the workflow or method
		virtual protected void Call_btnProcess_Click()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to invoke the method to call the save method of the workflow or method
		virtual protected void Call_btnSave_Click()
		{
			throw new Exception("Can not call base method");
		}
		// End MID Track 4858

		/// <summary>
		/// Use to lock a workflow or method before allowing updating
		/// </summary>
		/// <param name="aWorkflowMethodRID">The record ID of the workflow or method</param>
		/// <param name="aNode">The node being updated</param>
		public void LockWorkflowMethod(eChangeType aChangeType, MIDWorkflowMethodTreeNode aNode, bool aAllowReadOnly)
		{
			string errMsg;
			if (aNode.WorkflowMethodIND == eWorkflowMethodIND.Workflows)
			{
				WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
					aNode.Key,
					SAB.ClientServerSession.UserRID,
					SAB.ClientServerSession.ThreadID);

				try
				{
					workflowEnqueue.EnqueueWorkflow();
					_lockStatus = eLockStatus.Locked;
				}
				catch (WorkflowConflictException)
				{
					errMsg = "The following workflow requested:" + System.Environment.NewLine;
					foreach (WorkflowConflict WCon in workflowEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Workflow: " + aNode.Text + ", User: " + WCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					if (aChangeType == eChangeType.update &&
						aAllowReadOnly)
					{
						errMsg += "Do you wish to continue with the workflow as read-only?";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//							== DialogResult.Yes) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							== DialogResult.Yes) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.ReadOnly;
						}
						else
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
					else
						if (aChangeType == eChangeType.delete)
					{
						errMsg += "The selected workflow can not be deleted at this time.";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
//							== DialogResult.OK) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
							== DialogResult.OK) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
					else
					{
						errMsg += "The selected workflow can not be updated at this time.";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
//							== DialogResult.OK) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
							== DialogResult.OK) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
				}
			}
			else
			{
				MethodEnqueue methodEnqueue = new MethodEnqueue(
					aNode.Key,
					SAB.ClientServerSession.UserRID,
					SAB.ClientServerSession.ThreadID);

				try
				{
					methodEnqueue.EnqueueMethod();
					_lockStatus = eLockStatus.Locked;
				}
				catch (MethodConflictException)
				{
					errMsg = "The following method requested:" + System.Environment.NewLine;
					foreach (MethodConflict MCon in methodEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Method: " + aNode.Text + ", User: " + MCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					if (aChangeType == eChangeType.update &&
						aAllowReadOnly)
					{
						errMsg += "Do you wish to continue with the method as read-only?";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
//							== DialogResult.Yes) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
							== DialogResult.Yes) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.ReadOnly;
						}
						else
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
					else
						if (aChangeType == eChangeType.delete)
					{
						errMsg += "The selected method can not be deleted at this time.";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
//							== DialogResult.OK) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
							== DialogResult.OK) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
					else
					{
						errMsg += "The selected method can not be updated at this time.";

						// Begin MID Track #5076 - JSmith - Added conflict to message
//						if (MessageBox.Show (errMsg,  this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
//							== DialogResult.OK) 
						if (MessageBox.Show (errMsg,  this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning)
							== DialogResult.OK) 
						// End MID Track #5076
						{
							_lockStatus = eLockStatus.Cancel;
						}
					}
				}
			}
		}

		// Begin Track #5882 stodd
		// BEGIN Issue 4858 stodd 10.30.2007 forecast methods security
		/// <summary>
		/// Gets a VersionProfile list determined by security.
		/// </summary>
		/// <param name="planOrBasis"></param>
		/// <param name="chainOrStore"></param>
		/// <returns></returns>
		public ProfileList GetForecastVersionList(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore)
		{
			return GetForecastVersionList(securitySelectType, chainOrStore, false, Include.NoRID, false);
		}

		public ProfileList GetForecastVersionList(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore, bool excludeActual)
		{
			return GetForecastVersionList(securitySelectType, chainOrStore, false, Include.NoRID, excludeActual);
		}

		public ProfileList GetForecastVersionList(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore, bool addEmptySelection, int methodVersionRid)	// Track #5871
		{
			return GetForecastVersionList(securitySelectType, chainOrStore, addEmptySelection, methodVersionRid, false);
		}
		// End Track #5882 stodd

		// BEgin track #5871
        //Begin Track #5858 - JSmith - Validating store security only
        //public ProfileList GetForecastVersionList(ePlanBasisType planOrBasis, eSecurityTypes chainOrStore, bool addEmptySelection, int methodVersionRid)
		//public ProfileList GetForecastVersionList(ePlanBasisType planOrBasis, eSecurityTypes chainOrStore, bool addEmptySelection, int methodVersionRid)
		//{
		//    return GetForecastVersionList(planOrBasis, chainOrStore, addEmptySelection, methodVersionRid, false);
		//}
        //End Track #5858
		//End Track #5871

		/// <summary>
		/// Gets a VersionProfile list determined by security.
		/// </summary>
		/// <param name="planOrBasis"></param>
		/// <param name="chainOrStore"></param>
		/// <param name="addEmptySelection"></param>
		/// <returns></returns>
        //Begin Track #5858 - JSmith - Validating store security only
        //public ProfileList GetForecastVersionList(ePlanBasisType planOrBasis, eSecurityTypes chainOrStore, bool addEmptySelection, int methodVersionRid)
		public ProfileList GetForecastVersionList(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore, bool addEmptySelection, int methodVersionRid, bool excludeActual)	// Track #5871
        //End Track #5858
		{

            //Begin Track #5858 - JSmith - Validating store security only
            //ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions(planOrBasis, chainOrStore, methodVersionRid);
			ProfileList versionProfList = SAB.ClientServerSession.GetUserForecastVersions(securitySelectType, chainOrStore, methodVersionRid);
			//End Track #5858 stodd 
			if (addEmptySelection)
			{
                //Begin Track #5927 - KJohnson - Must pass in Space and not a Empty string to get grid to not show -1
				VersionProfile vp = new VersionProfile(Include.NoRID, " ", false);
                //End Track #5927 - KJohnson
				versionProfList.Insert(0, vp);
			}
			// Begin Track #5882 stodd Added back check for actuals
			// Begin track #5871 stodd
			if (excludeActual)
			{
				if (versionProfList.Contains(Include.FV_ActualRID))
				{
					VersionProfile vp = (VersionProfile)versionProfList.FindKey(Include.FV_ActualRID);
					versionProfList.Remove(vp);
				}
			}
			// End track #5871 stodd
			// End track #5882 stodd
			return versionProfList;
		}

        //Begin Track #5858 - JSmith - Validating store security only
        //public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers, bool canReadOnly)
        //{
        //    return ValidatePlanVersionSecurity(cboPlanVers, canReadOnly, ePlanType.Store);
        //}

        //public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers)
        //{
        //    return ValidatePlanVersionSecurity(cboPlanVers, false, ePlanType.Store);
        //}

        //public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers, ePlanType planType)
        //{
        //    return ValidatePlanVersionSecurity(cboPlanVers, false, planType);
        //}

        //// BEGIN Issue 5844 stodd
        //public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers, bool canReadOnly, ePlanType planType)
        //{
        //    bool validVersion = true;
        //    string errorMessage = string.Empty;
        //    string item = string.Empty;
        //    VersionSecurityProfile versionSecurity = null;

        //    if (cboPlanVers.SelectedValue != null && (int)cboPlanVers.SelectedValue != Include.NoRID)
        //    {
        //        VersionProfile vp = (VersionProfile)cboPlanVers.SelectedItem;

        //        if (planType == ePlanType.Chain)
        //            versionSecurity = vp.ChainSecurity;
        //        else
        //            versionSecurity = vp.StoreSecurity;

        //        if (versionSecurity.AccessDenied)
        //        {
        //            validVersion = false;
        //            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
        //            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
        //            errorMessage = errorMessage + item + ".";
        //            ErrorProvider.SetError(cboPlanVers, errorMessage);
        //        }
        //        else if (versionSecurity.IsReadOnly)
        //        {
        //            if (!canReadOnly)
        //            {
        //                validVersion = false;
        //                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
        //                item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
        //                errorMessage = errorMessage + item + ".";
        //                ErrorProvider.SetError(cboPlanVers, errorMessage);
        //            }
        //        }
        //    }
        //    if (validVersion)
        //        ErrorProvider.SetError(cboPlanVers, string.Empty);

        //    return validVersion;
        //}

        //public bool ValidatePlanNodeSecurity(TextBox txtOTSHNDesc)
        //{
        //    return ValidatePlanNodeSecurity(txtOTSHNDesc, false);
        //}

        //public bool ValidatePlanNodeSecurity(TextBox txtOTSHNDesc, bool canReadOnly)
        //{
        //    bool validNode = true;
        //    int nodeRid = Include.NoRID;
        //    string errorMessage = string.Empty;
        //    string item = string.Empty;

        //    if (txtOTSHNDesc.Tag != null)
        //    {
        //        nodeRid = (int)txtOTSHNDesc.Tag;

        //        if (nodeRid != Include.NoRID)
        //        {
        //            HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeRid, (int)eSecurityTypes.Store);
        //            if (nodeSecurity.AccessDenied)
        //            {
        //                validNode = false;
        //                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
        //                item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
        //                errorMessage = errorMessage + item + ".";
        //                ErrorProvider.SetError(txtOTSHNDesc, errorMessage);
        //            }
        //            else if (nodeSecurity.IsReadOnly)
        //            {
        //                if (!canReadOnly)
        //                {
        //                    validNode = false;
        //                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
        //                    item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
        //                    errorMessage = errorMessage + item + ".";
        //                    ErrorProvider.SetError(txtOTSHNDesc, errorMessage);
        //                }
        //            }
        //        }
        //    }
        //    if (validNode)
        //        ErrorProvider.SetError(txtOTSHNDesc, string.Empty);

        //    return validNode;
        //}
        //// END Issue 4858 stodd 10.30.2007 forecast methods security

        public bool ValidateChainPlanVersionSecurity(ComboBox cboPlanVers, bool canReadOnly)
        {
            return ValidatePlanVersionSecurity(cboPlanVers, canReadOnly, ePlanSelectType.Chain);
        }

        public bool ValidateStorePlanVersionSecurity(ComboBox cboPlanVers, bool canReadOnly)
        {
			return ValidatePlanVersionSecurity(cboPlanVers, canReadOnly, ePlanSelectType.Store);
        }

        public bool ValidateChainPlanVersionSecurity(ComboBox cboPlanVers)
        {
			return ValidatePlanVersionSecurity(cboPlanVers, false, ePlanSelectType.Chain);
        }

        public bool ValidateStorePlanVersionSecurity(ComboBox cboPlanVers)
        {
			return ValidatePlanVersionSecurity(cboPlanVers, false, ePlanSelectType.Store);
		}

		public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers, ePlanSelectType planType)
		{
			return ValidatePlanVersionSecurity(cboPlanVers, false, planType);
		}

		public bool ValidatePlanVersionSecurity(ComboBox cboPlanVers, bool canReadOnly, ePlanSelectType planType)
		{
			bool validVersion = false; // Track #5859 stodd
			string errorMessage = string.Empty;
			string item = string.Empty;
			VersionSecurityProfile versionSecurity = null;
            bool accessDenied, readOnly, chainChecked;

			if (cboPlanVers.SelectedValue != null && (int)cboPlanVers.SelectedValue != Include.NoRID)
			{
				validVersion = true;	// Track #5859 stodd
				VersionProfile vp = (VersionProfile)cboPlanVers.SelectedItem;

                accessDenied = false;
                readOnly = false;
                chainChecked = false;

				if (System.Convert.ToBoolean(planType & ePlanSelectType.Chain))
                {
                    chainChecked = true;
                    versionSecurity = vp.ChainSecurity;
                    if (versionSecurity.AccessDenied)
                    {
                        accessDenied = true;
                    }
                    else if (versionSecurity.IsReadOnly)
                    {
                        if (!canReadOnly)
                        {
                            readOnly = true;
                        }
                    }
                }

				if (System.Convert.ToBoolean(planType & ePlanSelectType.Store))
                {
                    versionSecurity = vp.StoreSecurity;
                    if (accessDenied && !versionSecurity.AccessDenied) // denied for chain but not for store
                    {
                        accessDenied = false;
                    }
                    else if (!chainChecked && versionSecurity.AccessDenied) // chain not checked but denied for store
                    {
                        accessDenied = true;
                    }

                    if (!accessDenied)
                    {
                        if (readOnly && !versionSecurity.IsReadOnly) // chain is read only but not store
                        {
                            if (!canReadOnly)
                            {
                                readOnly = false;
                            }
                        }
                        else if (!chainChecked && versionSecurity.IsReadOnly) // chain not checked but store is read only
                        {
                            readOnly = true;
                        }
                    }
                }

                if (accessDenied)
				{
					validVersion = false;
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
					item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
					errorMessage = errorMessage + item + ".";
					ErrorProvider.SetError (cboPlanVers,errorMessage);
				}
                else if (readOnly)
				{
					if (!canReadOnly)
					{
						validVersion = false;
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
						item = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
						errorMessage = errorMessage + item + ".";
						ErrorProvider.SetError (cboPlanVers,errorMessage);
					}
				}
			}
			if (validVersion)
				ErrorProvider.SetError (cboPlanVers,string.Empty);
			
			return validVersion;
		}

        public bool ValidateStorePlanNodeSecurity(TextBox txtOTSHNDesc)
		{
            return ValidatePlanNodeSecurity(txtOTSHNDesc, false, eSecurityTypes.Store);
		}

        public bool ValidateChainPlanNodeSecurity(TextBox txtOTSHNDesc)
        {
            return ValidatePlanNodeSecurity(txtOTSHNDesc, false, eSecurityTypes.Chain);
        }

        public bool ValidatePlanNodeSecurity(TextBox txtOTSHNDesc, bool canReadOnly, eSecurityTypes aSecurityTypes)
		{
			
			bool validNode = false;	// Track #5859 stodd
			int nodeRid = Include.NoRID;
			string errorMessage = string.Empty;
			string item = string.Empty;
            //Begin Track #5858 - JSmith - Validating store security only
            HierarchyNodeProfile hnp;
            //End Track #5858

			if (txtOTSHNDesc.Tag != null)
			{
                //Begin Track #5858 - JSmith - Validating store security only
                //nodeRid = (int)txtOTSHNDesc.Tag;
                if (txtOTSHNDesc.Tag.GetType() == typeof(int))
                {
                    nodeRid = (int)txtOTSHNDesc.Tag;
                }
                else if (txtOTSHNDesc.Tag.GetType() == typeof(HierarchyNodeProfile))
                {
                    hnp = (HierarchyNodeProfile)txtOTSHNDesc.Tag;
                    nodeRid = hnp.Key;
                }
                else if (txtOTSHNDesc.Tag.GetType() == typeof(MIDMerchandiseTextBoxTag))
                {
                    if (((MIDTag)txtOTSHNDesc.Tag).MIDTagData != null)
                    {
                        hnp = (HierarchyNodeProfile)((MIDTag)txtOTSHNDesc.Tag).MIDTagData;
                        nodeRid = hnp.Key;
                    }
                }
                //End Track #5858

				if (nodeRid != Include.NoRID)
				{
					validNode = true;	// Track #5859 stodd
                    HierarchyNodeSecurityProfile nodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(nodeRid, (int)aSecurityTypes);
                    if (nodeSecurity.AccessDenied)
					{
						validNode = false;
                        if (canReadOnly)
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                        }
                        else
                        {
                            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                            item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                            errorMessage = errorMessage + item + ".";
                        }
						ErrorProvider.SetError (txtOTSHNDesc,errorMessage);
					}
					else if (nodeSecurity.IsReadOnly)
					{
						if (!canReadOnly)
						{
							validNode = false;
                            if (canReadOnly)
                            {
                                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                            }
                            else
                            {
                                errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                                item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                                errorMessage = errorMessage + item + ".";
                            }
							ErrorProvider.SetError (txtOTSHNDesc,errorMessage);
						}
					}
				}
			}
			if (validNode)
				ErrorProvider.SetError (txtOTSHNDesc,string.Empty);
			
			return validNode;
		}
		// END Issue 4858 stodd 10.30.2007 forecast methods security

        // Begin Track #4872 - JSmith - Global/User Attributes
        protected ProfileList GetStoreGroupList(eChangeType aChangeType, eGlobalUserType aGlobalUserType, bool aAddEmptyGroup)
        {
            ProfileList al = null;
            StoreGroupListViewProfile sgp;
            try
            {
                if (aChangeType == eChangeType.add)
                {
                    if (radGlobal.Checked)
                    {
                        aGlobalUserType = eGlobalUserType.Global;
                    }
                    else
                    {
                        aGlobalUserType = eGlobalUserType.User;
                    }
                }
                if (aGlobalUserType == eGlobalUserType.User)
                {
                    al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, true); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, true);
                }
                else
                {
                    al = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.GlobalOnly, true); //SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.GlobalOnly, true);
                }

                if (aAddEmptyGroup)
                {
                    sgp = new StoreGroupListViewProfile(Include.NoRID);
                    sgp.Name = string.Empty;
                    al.Add(sgp);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return al;
        }
        // End Track #4872

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>
		virtual protected void SetCommonFields()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to validate the fields that are specific to the rule
		/// </summary>
		virtual protected bool ValidateSpecificFields()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>
		virtual protected void HandleErrors()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to set the specific fields in the method object before updating
		/// </summary>
		virtual protected void SetSpecificFields()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to set the specific object before updating
		/// </summary>
		virtual protected void SetObject()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Use to get the explorer node selected when form was opened
		/// </summary>
		virtual protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			throw new Exception("Can not call base method");
		}

		/// <summary>
		/// Validate the base method information
		/// </summary>
		private bool ValidateCommonFields(bool aSaveAs)
		{
			_nameValid = true;
			_nameMessage = string.Empty;
			_descriptionValid = true;
			_descriptionMessage = string.Empty;
			_userGlobalValid = true;
			_userGlobalMessage = string.Empty;

			//Method_Name
			if (_workflowMethodName == string.Empty)
			{
				_nameValid = false;
				_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NameRequired);
			}
			if (_workflowMethodName.Length > 50)
			{
				_nameValid = false;
				_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueExceedsMaximum);
			}
			//Method Description
            // Begin TT#838 - JSmith - Workflows/Methods - Make Description "not required"
            //if (_workflowMethodDescription == string.Empty)
            //{
            //    _descriptionValid = false;
            //    _descriptionMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DescriptionRequired);
            //}
            // End TT#838 - JSmith
			//User Global selection
			if (_userRadioButton.Checked == false && _globalRadioButton.Checked == false)
			{
				_userGlobalValid = false;
				_userGlobalMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UserOrGlobalRequired);
			}
			if (_nameValid &&
				!aSaveAs)
			{
				//string whereClause;
				SetObject();
				switch (_workflowMethodIND)
				{
					case eWorkflowMethodIND.Methods:
					case eWorkflowMethodIND.SizeMethods:
						
						MethodBaseData mb = new MethodBaseData();
						if (_globalRadioButton.Checked)
						{
							_ABM.User_RID = Include.GetGlobalUserRID();
						}
						else
						{
							_ABM.User_RID = SAB.ClientServerSession.UserRID;
						}
						//whereClause = mb.BuildMethodAK(_workflowMethodName, _ABM.MethodType, _ABM.User_RID, _ABM.Key);
						//if (mb.GenericRowExists("METHOD", whereClause))
                        if (mb.GenericRowExists(_workflowMethodName, _ABM.MethodType, _ABM.User_RID, _ABM.Key))
						{
							_nameValid = false;
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
						}
						break;

					case eWorkflowMethodIND.Workflows:
					
						WorkflowBaseData wb = new WorkflowBaseData();
						if (_globalRadioButton.Checked)
						{
							_ABW.UserRID = Include.GetGlobalUserRID();
							_ABW.IsGlobal = true;
						}
						else
						{
							_ABW.UserRID = SAB.ClientServerSession.UserRID;
							_ABW.IsGlobal = false;
						}
						//whereClause = wb.BuildWorkflowAK(_workflowMethodName, _ABW.WorkFlowType, _ABW.UserRID, _ABW.Key);
						//if (wb.GenericRowExists("WORKFLOW", whereClause))
                        if (wb.GenericRowExists(_workflowMethodName, (int)_ABW.WorkFlowType, _ABW.UserRID, _ABW.Key))
						{
							_nameValid = false;
                            // Begin TT#1742 - JSmith - Displays wrong message for duplicate workflow
                            //_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateWorkflow);
                            // End TT#1742
						}
						break;
				}
			}
			if (_nameValid && _descriptionValid && _userGlobalValid)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public bool IsNameDuplicate() 
        {
            return isDuplicateName();
        }
        // End TT#231  

		private bool isDuplicateName()
		{
			bool duplicateFound = false;
			try
			{
				WorkflowMethodManager wmManager = new WorkflowMethodManager(SAB.ClientServerSession.UserRID);
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					if (wmManager.CheckForDuplicateMethodID(SAB.ClientServerSession.UserRID, _ABM))
					{
						_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
						duplicateFound = true;
					}
				}
				else
				{
					if (wmManager.CheckForDuplicateWorkflowID(SAB.ClientServerSession.UserRID, _ABW))
					{
						_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateWorkflow);
						duplicateFound = true;
					}
				}
			}
			catch
			{
				throw;
			}
			return duplicateFound;
		}

		private bool SaveAsMethodWorkflow()
		{
			// Begin #4255 - jsmith
			int saveWorkflowMethodRID = _workflowMethodRID;
			// End #4255
            // Begin Track #5905 - JSmith - Close after Save As
            bool successful = true;
            // End Track #5905

            int newItemKey = Include.NoRID;
			try
			{
				bool continueSave = false;
				bool saveAsCanceled = false;
				ErrorFound = ValidateAndSetFields(true);
				if (ErrorFound)
				{
                    // Begin Track #5905 - JSmith - Close after Save As
                    //return true;
                    return false;
                    // End Track #5905
				}

				frmSaveAs formSaveAs = new frmSaveAs(SAB);
				
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
                    // Begin Track #5912 - JSmith - Save As needs to clone custom override models
                    //_ABM = _ABM.Copy(SAB.ClientServerSession, true);
                    _ABM = _ABM.Copy(SAB.ClientServerSession, true, true);
                    // End Track #5912
					_ABM.Key = Include.NoRID;
					_ABM.Method_Change_Type = eChangeType.add;
					formSaveAs.SaveAsName = _ABM.Name;
					if (_ABM.User_RID == Include.GlobalUserRID)
					{
						formSaveAs.isGlobalChecked = true;
					}
					else
					{
						formSaveAs.isUserChecked = true;
					}
                    // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                    formSaveAs.SaveAsMethod = MIDText.GetTextOnly((int)_ABM.MethodType); 
                    if (_ABM.MethodType == eMethodType.Velocity)
                    {
                        formSaveAs.SaveMethod = false;
                        formSaveAs.SaveView = true;
                        formSaveAs.ShowViewOption = true;
                        formSaveAs.SaveAsViewName = _saveAsViewName;
                        if (_saveAsViewUserRID == Include.GlobalUserRID)
                        {
                            formSaveAs.isGlobalViewChecked = true;
                        }
                        else
                        {
                            formSaveAs.isUserViewChecked = true;
                        }
                        formSaveAs.EnableGlobalView = _methodViewGlobalSecurity.AllowUpdate;
                        formSaveAs.EnableUserView = _methodViewUserSecurity.AllowUpdate;
                        formSaveAs.ShowDetailViewOption = ShowDetailViewOption;
                        if (ShowDetailViewOption)
                        {
                            formSaveAs.SaveAsDetailViewName = SaveAsDetailViewName;
                            if (_saveAsDetailViewUserRID == Include.GlobalUserRID)
                            {
                                formSaveAs.isGlobalDetailViewChecked = true;
                            }
                            else
                            {
                                formSaveAs.isUserDetailViewChecked = true;
                            }
                            formSaveAs.EnableGlobalDetail = _methodDetailViewGlobalSecurity.AllowUpdate;
                            formSaveAs.EnableUserDetail =  _methodDetailViewUserSecurity.AllowUpdate;
                        }
                    }
                    // End TT#231  
				}
				else
				{
					_ABW = _ABW.Copy(SAB.ClientServerSession, true);
					_ABW.Key = Include.NoRID;
					_ABW.Workflow_Change_Type = eChangeType.add;
					formSaveAs.SaveAsName = _ABW.WorkFlowName;
					if (_ABW.UserRID == Include.GlobalUserRID)
					{
						formSaveAs.isGlobalChecked = true;
					}
					else
					{
						formSaveAs.isUserChecked = true;
					}
                    formSaveAs.SaveAsMethod = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);
				}

                // Begin MID Track #5857 - JSmith - Cannot “Save As” to a User Method or Workflow
				// Begin MID Track #5224 - JSmith - Able to create global when not authorized
////				formSaveAs.ShowUserGlobal = true;
//                if (_userSecurity.AllowUpdate && 
//                    _globalSecurity.AllowUpdate)
//                {
//                    formSaveAs.ShowUserGlobal = true;
//                }
                if (_userSecurity.AllowUpdate ||
                    _globalSecurity.AllowUpdate)
                {
                    formSaveAs.ShowUserGlobal = true;
                    if (_userSecurity.AllowUpdate &&
                        !_globalSecurity.AllowUpdate)
                    {
                        formSaveAs.isUserChecked = true;
                        formSaveAs.EnableGlobal = false;
                        formSaveAs.EnableUser = false;
                    }
                    else if (!_userSecurity.AllowUpdate &&
                        _globalSecurity.AllowUpdate)
                    {
                        formSaveAs.isGlobalChecked = true;
                        formSaveAs.EnableGlobal = false;
                        formSaveAs.EnableUser = false;
                    }
                }
				// End MID Track #5224
                // End MID Track #5857
				formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
				while (!continueSave)
				{
					formSaveAs.ShowDialog(this);
					saveAsCanceled = formSaveAs.SaveCanceled;
					if (!saveAsCanceled)
					{
						saveAsCanceled = false;
						continueSave = true;
                        if (formSaveAs.SaveMethod)  // TT#231  RMatelic - Add Views to Velocity Matrix and Store Detail
                        {
                            if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
                            {
                                _ABM.Name = formSaveAs.SaveAsName;
                                if (formSaveAs.isUserChecked)
                                {
                                    _ABM.User_RID = SAB.ClientServerSession.UserRID;
                                }
                                else
                                {
                                    _ABM.User_RID = Include.GlobalUserRID;
                                }
                            }
                            else
                            {
                                _ABW.WorkFlowName = formSaveAs.SaveAsName;
                                if (formSaveAs.isUserChecked)
                                {
                                    _ABW.UserRID = SAB.ClientServerSession.UserRID;
                                }
                                else
                                {
                                    _ABW.UserRID = Include.GlobalUserRID;
                                }
                            }
                            if (isDuplicateName())
                            {
                                MessageBox.Show(_nameMessage, this.Text);
                                continueSave = false;
                            }
                            else if (!SaveValues(ref newItemKey))
                            {
                                MessageBox.Show(_nameMessage, this.Text);
                                continueSave = true;
                                // Begin Track #5905 - JSmith - Close after Save As
                                successful = false;
                                // End Track #5905
                            }
                        }
                        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                        if (successful && continueSave && formSaveAs.SaveView)  
                        {
                            _saveAsViewUserRID = (formSaveAs.isUserViewChecked ? SAB.ClientServerSession.UserRID : Include.GlobalUserRID);
                            _saveAsViewName = formSaveAs.SaveAsViewName;
                            SaveGridView();
                            // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                            if (!formSaveAs.SaveMethod)
                            {
                                _closeOnSave = false;
                            }
                            // End TT#330
                        }
                        if (successful && continueSave && formSaveAs.SaveDetailView)
                        {
                            int saveAsDetailViewUserRID = (formSaveAs.isUserDetailViewChecked ? SAB.ClientServerSession.UserRID : Include.GlobalUserRID);
                            _saveAsDetailViewName = formSaveAs.SaveAsDetailViewName;
                            SaveDetailView(saveAsDetailViewUserRID, _saveAsDetailViewName);
                            // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                            if (!formSaveAs.SaveMethod)
                            {
                                _closeOnSave = false;
                            }
                            // End TT#330
                        }
                        // End TT#231  


                        //BEGIN TT#459-MD -jsobek -Method created with File->Save As is not viewable until after a refresh of the explorer
                        if (successful && continueSave)
                        {
                            if (_EAB != null && _EAB.WorkflowMethodExplorer != null)
                            {
                                _EAB.WorkflowMethodExplorer.RefreshData();
                            }
                        }
                        //END TT#459-MD -jsobek -Method created with File->Save As is not viewable until after a refresh of the explorer



					}
					else
					{
						continueSave = true;
                        // Begin Track #5905 - JSmith - Close after Save As
                        successful = false;
                        // End Track #5905
					}
				}
			}
			catch(Exception err)
			{
				ErrorFound = true;
				HandleException(err);
			}
			// Begin #4255 - jsmith
			finally
			{
			// retore the object back to the original
				SetObject();
				_workflowMethodRID = saveWorkflowMethodRID;
			}
			// End #4255
		
            // Begin Track #5905 - JSmith - Close after Save As
            //return true;
            return successful;
            // End Track #5905
		}
        
        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public void SaveGridView()
        {
            try
            {
                _gridViewData.OpenUpdateConnection();

                try
                {
                    _viewRID = _gridViewData.GridView_GetKey(_saveAsViewUserRID, (int)_layoutID, _saveAsViewName);
                    if (_viewRID != Include.NoRID)
                    {
                        _gridViewData.GridViewDetail_Delete(_viewRID);
                    }
                    else
                    {
                        // Begin TT#456 - RMatelic - Add views to Size Review
                        //_viewRID = _gridViewData.GridView_Insert(_saveAsViewUserRID, (int)_layoutID, _saveAsViewName);
                        _viewRID = _gridViewData.GridView_Insert(_saveAsViewUserRID, (int)_layoutID, _saveAsViewName, true, Include.NoRID, Include.NoRID, false, Include.NoRID, false); //TT#1313-MD -jsobek -Header Filters
                        //End TT#456  
                    }

                    _gridViewData.GridViewDetail_Insert(_viewRID, _saveViewGrid);
                    _gridViewData.CommitData();
                }
                catch (Exception exc)
                {
                    ErrorFound = true;
                    _gridViewData.Rollback();
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    _gridViewData.CloseUpdateConnection();
                    UpdateAdditionalData();      // TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                }
            }
            catch (Exception exc)
            {
                ErrorFound = true;
                string message = exc.ToString();
                throw;
            }
        }

        public void SaveDetailView(int aViewUserRID, string aViewName) 
        {
            try
            {
                FrmStyleView.SaveGridView(aViewUserRID, aViewName);
            }
            catch (Exception exc)
            {
                ErrorFound = true;
                string message = exc.ToString();
                throw;
            }
        }    
        // End TT#231  

        // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
        //private bool CopyWorkflowMethod(ref int aNewItemKey)
        private bool CopyWorkflowMethod(ref int aNewItemKey, ref string aNewItemName)
        // End TT#1167
		{
			try
			{
				bool duplicateName = true;
				bool isMethod = true;
				string name = null;
				string newName = null;
				SetObject();

				// Begin MID Track 4858 - JSmith - Security changes
				bool allowProcess = true;
				if (_workflowMethodType == eWorkflowMethodType.Method)
				{
					allowProcess = _ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
				}
				
				if (!allowProcess)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
					UnlockWorkflowMethod();
					return false;
				}
				// End MID Track 4858

				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
                    // Begin Track #5912 - JSmith - Save As needs to clone custom override models
                    //_ABM = _ABM.Copy(SAB.ClientServerSession, true);
                    _ABM = _ABM.Copy(SAB.ClientServerSession, true, true);
                    // End Track #5912
					_ABM.Key = Include.NoRID;
					_ABM.Method_Change_Type = eChangeType.add;
					name = _ABM.Name;
					if (_explorerNode.GlobalUserType == eGlobalUserType.Global)
					{
						_ABM.User_RID = Include.GlobalUserRID;
					}
					else
					{
						_ABM.User_RID = SAB.ClientServerSession.UserRID;
					}
				}
				else
				{
					isMethod = false;
					_ABW = _ABW.Copy(SAB.ClientServerSession, true);
					_ABW.Key = Include.NoRID;
					_ABW.Workflow_Change_Type = eChangeType.add;
					name = _ABW.WorkFlowName;
					if (_explorerNode.GlobalUserType == eGlobalUserType.Global)
					{
						_ABW.UserRID = Include.GlobalUserRID;
					}
					else
					{
						_ABW.UserRID = SAB.ClientServerSession.UserRID;
					}
				}

				int copyCntr = 0;
				while (duplicateName)
				{
					if (!isDuplicateName())
					{
						duplicateName = false;
					}
					else
					{
						copyCntr++;
						if (copyCntr > 1)
						{
							newName = "Copy" + copyCntr.ToString(CultureInfo.CurrentUICulture) + " of " + name;
						}
						else
						{
							newName = "Copy of " + name;
						}
						if (isMethod)
						{
							_ABM.Name = newName;
						}
						else
						{
							_ABW.WorkFlowName = newName;
						}
					}
				}
				SaveValues(ref aNewItemKey);
                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                if (newName != null)
                {
                    aNewItemName = newName;
                }
                else
                {
                    aNewItemName = name;
                }
                // End TT#1167
			}
			catch(Exception err)
			{
				ErrorFound = true;
				HandleException(err);
			}
			finally
			{
				// retore the object back to the original
				SetObject();
			}
		
			return true;
		}

		private bool MoveWorkflowMethod()
		{
            int newItemKey = Include.NoRID;

			try
			{
				bool duplicateName = true;
				bool isMethod = true;
				string name = null;
				string newName = null;
				SetObject();
				
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					_ABM.Method_Change_Type = eChangeType.update;
					name = _ABM.Name;
					if (_explorerNode.GlobalUserType == eGlobalUserType.Global)
					{
						_ABM.User_RID = Include.GlobalUserRID;
					}
					else
					{
						_ABM.User_RID = SAB.ClientServerSession.UserRID;
					}
				}
				else
				{
					isMethod = false;
					name = _ABW.WorkFlowName;
					_ABW.Workflow_Change_Type = eChangeType.update;
					if (_explorerNode.GlobalUserType == eGlobalUserType.Global)
					{
						_ABW.UserRID = Include.GlobalUserRID;
					}
					else
					{
						_ABW.UserRID = SAB.ClientServerSession.UserRID;
					}
				}

				int copyCntr = 0;
				while (duplicateName)
				{
					if (!isDuplicateName())
					{
						duplicateName = false;
					}
					else
					{
						copyCntr++;
						if (copyCntr > 1)
						{
							newName = "Copy" + copyCntr.ToString(CultureInfo.CurrentUICulture) + " of " + name;
						}
						else
						{
							newName = "Copy of " + name;
						}
						if (isMethod)
						{
							_ABM.Name = newName;
						}
						else
						{
							_ABW.WorkFlowName = newName;
						}
					}
				}

				SaveValues(ref newItemKey);
				//change type to add before updating explorer so it will add new item
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					_ABM.Method_Change_Type = eChangeType.add;
					UpdateExplorer(_ABM);
				}
				else
				{
					_ABW.Workflow_Change_Type = eChangeType.add;
					UpdateExplorer(_ABW);
				}
			}
			catch(Exception err)
			{
				ErrorFound = true;
				HandleException(err);
			}
		
			return true;
		}

		override protected bool SaveChanges()
		{
            int newItemKey = Include.NoRID;
			try
			{
				// Replaced above code with following method
				ErrorFound = ValidateAndSetFields(false);
				if (ErrorFound)
				{
					return true;
				}
				else
				{
                    return SaveValues(ref newItemKey);
				}
			}
			catch(Exception err)
			{
				ErrorFound = true;
				HandleException(err);
			}
		
			return true;
		}

        // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
        public bool SaveValuesFromExternalSource(ref int aNewItemKey)
        {
            return SaveValues(ref aNewItemKey);
        }    
        // End TT#231 

        private bool SaveValues(ref int aNewItemKey)
		{
			try
			{
//				// Replaced above code with following method
//				ErrorFound = ValidateAndSetFields();
//				if (ErrorFound)
//					return true;
//				

				//For now save method and refresh Explorer
				ClientTransaction.DataAccess.OpenUpdateConnection();
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					_ABM.Update(ClientTransaction.DataAccess);
                    aNewItemKey = _ABM.Key;
					switch (_ABM.Key)
					{
						case (int)eGenericDBError.GenericDBError:
							_nameValid = false;
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GenericMethodInsertError);
							return true;
						case (int)eGenericDBError.DuplicateKey:
							_nameValid = false;
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
							return true;
						default:
							break;
					}
				}
				else
				{
					_ABW.Update(ClientTransaction.DataAccess);
                    aNewItemKey = _ABW.Key;
					switch (_ABW.Key)
					{
						case (int)eGenericDBError.GenericDBError:
							_nameValid = false;
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_GenericMethodInsertError);
							return true;
						case (int)eGenericDBError.DuplicateKey:
							_nameValid = false;
							_nameMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateMethod);
							return true;
						default:
							break;
					}
				}

				ClientTransaction.DataAccess.CommitData();
				ClientTransaction.DataAccess.CloseUpdateConnection();
				ChangePending = false;

				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					if (_ABM.Method_Change_Type == eChangeType.add ||
						_nameChanged)
					{
						UpdateExplorer(_ABM);
					}
				}
				else
				{
					if (_ABW.Workflow_Change_Type == eChangeType.add ||
						_nameChanged)
					{
						UpdateExplorer(_ABW);
					}
				}
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail
                if (SaveUserGridView)
                {
                    UserGridView.UserGridView_Update(SAB.ClientServerSession.UserRID, LayoutID, ViewRID);
                }
                // End TT#231  
			}
			catch(Exception err)
			{
				ErrorFound = true;
				HandleException(err);
			}
		
			return true;
		}

		protected bool ValidateAndSetFields(bool aSaveAs)
		{
			try
			{
				ErrorFound = false;
				SetCommonFields();
				if (!ValidateCommonFields(aSaveAs))
				{
					ErrorFound = true;
				}
				if (!ValidateSpecificFields())
				{
					ErrorFound = true;
				}

				// BEGIN Track #5871 stodd
                // Begin Track #5926 - JSmith - Save As when no security
                //if (!ApplySecurity())
				if (!aSaveAs &&
                    !ApplySecurity())
				{
                // End Track #5926
					ErrorFound = true;
				}
				// End Track 5871
		
				if (ErrorFound)
				{
					HandleErrors();
					string text = MIDText.GetTextOnly(eMIDTextCode.msg_ErrorsFoundReviewCorrect);
                    MessageBox.Show(this, text, this.Text.Replace("*",""), MessageBoxButtons.OK, MessageBoxIcon.Error);
					//return true;
				}
				else
				{
					SetSpecificFields();
					SetObject();
					LoadCommonFields();
				}
			}
			catch(Exception err)
			{
				HandleException(err);
                ErrorFound = true; // TT#370 Build Packs Enhancement (errors were occurring, yet database was still being updated; traced to lack of this statement)
			}
		
			return ErrorFound;
		}
		protected bool Delete()
		{
			try
			{
				SetObject();

                // Begin MID Track #5903 - JSmith - User not able to delete method
                //// Begin MID Track 4858 - JSmith - Security changes
                //bool allowProcess = true;
                //if (_workflowMethodType == eWorkflowMethodType.Method)
                //{
                //    allowProcess = _ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
                //}
				
                //if (!allowProcess)
                //{
                //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
                //    UnlockWorkflowMethod();
                //    return false;
                //}
                //// End MID Track 4858
                // End MID Track #5903

				ClientTransaction.DataAccess.OpenUpdateConnection();
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					_ABM.Method_Change_Type = eChangeType.delete;
					_ABM.Update(ClientTransaction.DataAccess);
				}
				else
				{
					_ABW.Workflow_Change_Type = eChangeType.delete;
					_ABW.Update(ClientTransaction.DataAccess);
				}

				ClientTransaction.DataAccess.CommitData();
				ClientTransaction.DataAccess.CloseUpdateConnection();
				ChangePending = false;

				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					UpdateExplorer(_ABM);
				}
				else
				{
					UpdateExplorer(_ABW);
				}
				UnlockWorkflowMethod();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
				//Begin TT#492 - JScott - Received a Database Foreign Key Violation when deleting a Size Curve Method
				//throw keyVio;
				return false;
				//End TT#492 - JScott - Received a Database Foreign Key Violation when deleting a Size Curve Method
			}
			catch(Exception err)
			{
				HandleException(err);
			}

			return true;
		}

		protected bool Rename(string aNewName)
		{
			bool renameSuccessful = true;
			try
			{
				SetObject();

				// Begin MID Track 4858 - JSmith - Security changes
				bool allowProcess = true;
				if (_workflowMethodType == eWorkflowMethodType.Method)
				{
					allowProcess = _ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
				}
				
				if (!allowProcess)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
					UnlockWorkflowMethod();
					return false;
				}
				// End MID Track 4858
				
				// Begin Track #5056 - JSmith - 845 - Can not rename a workflow using the Raname shortcut menu
				if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
				{
					_ABM.Method_Change_Type = eChangeType.update;
					_ABM.Name = aNewName;
				}
				else
				{
					_ABW.Workflow_Change_Type = eChangeType.update;
					_ABW.WorkFlowName = aNewName;
				}
				// End Track #5056

				if (isDuplicateName())
				{
					MessageBox.Show(_nameMessage);
					renameSuccessful = false;
				}

				if (renameSuccessful)
				{
					ClientTransaction.DataAccess.OpenUpdateConnection();
					if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
					{
						_ABM.Method_Change_Type = eChangeType.update;
						_ABM.Name = aNewName;
						_ABM.Update(ClientTransaction.DataAccess);
					}
					else
					{
						_ABW.Workflow_Change_Type = eChangeType.update;
						_ABW.WorkFlowName = aNewName;
						_ABW.Update(ClientTransaction.DataAccess);
					}

					ClientTransaction.DataAccess.CommitData();
					ClientTransaction.DataAccess.CloseUpdateConnection();
				}
				ChangePending = false;
				UnlockWorkflowMethod();

			}
			catch(Exception err)
			{
				string message = err.ToString();
				renameSuccessful = false;
			}
			return renameSuccessful;
		}

		// BEGIN TT#696-MD - Stodd - add "active process"
		/// <summary>
		/// Checks to see if headers are selected.
		/// </summary>
		/// <param name="aThis"></param>
		/// <param name="methodType"></param>
		/// <returns></returns>
		public bool OkToProcess(object aThis, eMethodType methodType)
		{
			bool okToProcess = true;
			SelectedHeaderList selectedHeaderList = null;
			//bool useAssortmentHeaders = false;
			try
			{
				//useAssortmentHeaders = UseAssortmentSelectedHeaders();
				if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)  // use an assortment
				{
					selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(aThis, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, methodType);
					if (selectedHeaderList.Count == 0)
					{
                        // Begin TT#1559-MD - stodd - Asst Run method on a header with the method open and closed receive 2 different messages.  The messages need to be consistant.
                        // Begin TT#1061 - MD - stodd - make msg review specific
                        //string msg = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedInAssortment);
                        //msg = msg.Replace("{0}", AssortmentActiveProcessToolbarHelper.ActiveProcess.screenType);
                        //MessageBox.Show(msg);
                        // End TT#1061 - MD - stodd - make msg review specific

                        string msg = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
                        SAB.ClientServerSession.Audit.Add_Msg(
                            eMIDMessageLevel.Error,
                            eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
                            msg,
                            AssortmentActiveProcessToolbarHelper.ActiveProcess.screenType);
                        MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // End TT#1559-MD - stodd - Asst Run method on a header with the method open and closed receive 2 different messages.  The messages need to be consistant.

						okToProcess = false;
					}
				}
				else
				{
					selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
					if (selectedHeaderList.Count == 0)
					{
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
						okToProcess = false;
					}
				}
			}
			catch (Exception ex)
			{
				okToProcess = false;
				HandleException(ex, methodType.ToString() + ": OkToProcess");
			}
			return okToProcess;
		}
		// END TT#696-MD - Stodd - add "active process"


        // BEGIN TT#1560 - MD - DOConnell - Allocation Override method - Do not allow the forecast level to be Style or below if running against a placeholder with a placeholder style
        /// <summary>
        /// Does not allow Method to run against placeholder when forecast level below Style
        /// </summary>
        /// <param name="aThis"></param>
        /// <param name="methodType"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool OkToProcess_Assortment(object aThis, eMethodType methodType, int index)
        {
            bool okToProcess = true;
            try
            { 
                SelectedHeaderList selectedHeaderList = null;
                selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.AllocationOverride);
                if (methodType == eMethodType.AllocationOverride)
                {
                    foreach (SelectedHeaderProfile shp in selectedHeaderList)
                    {
                        if (shp.HeaderType == eHeaderType.Placeholder && shp.HeaderID == null && (index == 5 || index == 6))
                        {
                            string msg = MIDText.GetText(eMIDTextCode.msg_as_ForecastLevelBelowStyleNotValidForPlaceholder);
                            SAB.ClientServerSession.Audit.Add_Msg(
                                eMIDMessageLevel.Error,
                                eMIDTextCode.msg_as_ForecastLevelBelowStyleNotValidForPlaceholder,
                                msg,
                                AssortmentActiveProcessToolbarHelper.ActiveProcess.screenType);
                            MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // End TT#1559-MD - stodd - Asst Run method on a header with the method open and closed receive 2 different messages.  The messages need to be consistant.

                            okToProcess = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                okToProcess = false;
                HandleException(ex, methodType.ToString() + ": OkToProcess_Assortment");
            }
            return okToProcess;
        }
        // END TT#1560 - MD - DOConnell - Allocation Override method - Do not allow the forecast level to be Style or below if running against a placeholder with a placeholder style
		protected void ProcessAction(eMethodType aMethodType)
		{
			try
			{
				ProcessAction(aMethodType, false);
			}
			catch
			{
				throw;
			}
		}

		protected void ProcessAction(eMethodType aMethodType, bool aFromExplorer)
		{
            bool useAssortmentHeaders = false; // TT#981 - MD - Jellis - GA Size Need Gets Argument Out of Range
			try
			{
				Cursor.Current = Cursors.WaitCursor;

				// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
				// BEGIN TT#696-MD - Stodd - add "active process"
				//bool isProcessingInAssortment = false;
                //bool useAssortmentHeaders = false;  // TT#981 - MD - Jellis - GA Size Need Gets Argument Out of Range 
				int assrtRid = Include.NoRID;
				if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)aMethodType))
				{
				}
				else
				{
					assrtRid = AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID;
					if (assrtRid != Include.NoRID)
					{
						useAssortmentHeaders = true;
					}
				}
				// END TT#696-MD - Stodd - add "active process"
				if (aFromExplorer)
				{
					SetObject();
				}
				else
				{
					SaveChanges();
				}
				if (!ErrorFound)
				{	// BEGIN TT#696-MD - Stodd - add "active process"
					if (useAssortmentHeaders)
					{
						// Begin TT#969 - MD - stodd - method rid cannot be less than 1 - 
						//SAB.ProcessMethodOnAssortmentEvent.ProcessMethod(this, assrtRid, (eMethodType)(eMethodType)aMethodType, _workflowMethodRID);
                        SAB.ProcessMethodOnAssortmentEvent.ProcessMethod(this, assrtRid, (eMethodType)(eMethodType)aMethodType, _ABM.Key);
						// End TT#969 - MD - stodd - method rid cannot be less than 1 - 
					}
					else
					{
						// END TT#217-MD - stodd - unable to run workflow methods against assortment
						// make sure you get a new transaction each time
						ResetApplicationSessionTransaction();

						bool allowProcess = true;
						//if (!ErrorFound)
						//{
					// END TT#696-MD - Stodd - add "active process"
						// Begin TT#155 - JSmith - Size Curve Method
						//if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethodType))
						if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)aMethodType))
						// End TT#155
						{
							ApplicationTransaction.NeedHeaders = false;
							// Executes the overridden function in each forecast method
							allowProcess = VerifySecurity();
						}
						else
						{
							ApplicationTransaction.NeedHeaders = true;
							allowProcess = VerifySecurity();


							//						if (!allowProcess)
							//						{
							//							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
							//						}
						}

						// Begin MID Track 4858 - JSmith - Security changes
						if (_workflowMethodType == eWorkflowMethodType.Method)
						{
							allowProcess = _ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
						}
						// End MID Track 4858

                        // Begin TT#4120 - stodd - GA header highligthed and run an OTS Forecast method.  Receive a mssg.  If it's a non GA header the OTS forecast processes as expected.  Do not expect to receive a message.
                        if (Enum.IsDefined(typeof(eAllocationMethodType), (eAllocationMethodType)aMethodType))
                        {
                            // Begin TT#1154-MD - stodd - null reference when opening selection - 
                            SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                            ArrayList headerInGAList = new ArrayList();
                            if (ApplicationTransaction.ContainsGroupAllocationHeaders(ref headerInGAList, selectedHeaderList))
                            {
                                throw new HeaderInGroupAllocationException(MIDText.GetText(eMIDTextCode.msg_al_HeaderBelongsToGroupAllocation), headerInGAList);
                            }
                            // End TT#1154-MD - stodd - null reference when opening selection - 
                        }
                        // End TT#4120 - stodd - GA header highligthed and run an OTS Forecast method.  Receive a mssg.  If it's a non GA header the OTS forecast processes as expected.  Do not expect to receive a message.

						if (allowProcess)
						{
							// begin TT#1185 - JEllis - Verify ENQ before Update
							string enqMessage;

               
							if (EnqueueHeadersForProcessing())
							{
								if (ApplicationTransaction.EnqueueSelectedHeaders(out enqMessage))
								{
									ApplicationTransaction.ProcessMethod(aMethodType, _ABM.Key);
								}
								else
								{
									ApplicationTransaction.SAB.MessageCallback.HandleMessage(
									enqMessage,
									"Header Lock Conflict",
									System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
								}
							}
							else
							{
								// end TT#1185 - JEllis - Verify ENQ before Update
								ApplicationTransaction.ProcessMethod(aMethodType, _ABM.Key);
							}   // TT#1185 - JEllis - Verify ENQ before Update 

							// OTS Plan actions and Allocation Actions resolve thier status a little differently...
							if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethodType))
							{
								eOTSPlanActionStatus actionStatus = ApplicationTransaction.OTSPlanActionStatus;
								string message = MIDText.GetTextOnly((int)actionStatus);
								MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

							}
							else
							{
								eAllocationActionStatus actionStatus = ApplicationTransaction.AllocationActionAllHeaderStatus;
								//							if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
								//							{
								// Begin TT#155 - JSmith - Size Curve Method
								//if (Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)aMethodType))
								if (!Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)aMethodType))
									// End TT#155
									UpdateAllocationWorkspace();
								//							}
								string message = MIDText.GetTextOnly((int)actionStatus);
								MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
								// begin TT#241 - MD - JEllis - Header Enqueue Process
								if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
									|| actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
								{
									CloseForms();
								}
								// end TT#241 - MD - JEllis - Header Enqueue Process
							}
							// end TT#1185 - JEllis - Verify ENQ before Update
						}
						else
						{
							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
						}
						//}
					}
				}
			}
			catch (MIDException MIDexc)
			{
				HandleMIDException(MIDexc);
			}
			catch(SpreadFailed err)
			{
				MessageBox.Show(err.Message, this.Text);
			}
			catch(HeaderInUseException err)
			{
				string headerListMsg = string.Empty;
				foreach (string headerId in err.HeaderList)
				{
					if (headerListMsg.Length > 0)
						headerListMsg += ", " + headerId;
					else
						headerListMsg = " " + headerId;
				}
				MessageBox.Show(err.Message + headerListMsg , this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			// Begin TT#1019 - MD - stodd - prohibit allocation actions against GA - 
            catch (HeaderInGroupAllocationException err)
            {
                string headerListMsg = string.Empty;
                foreach (string headerId in err.HeaderList)
                {
                    if (headerListMsg.Length > 0)
                        headerListMsg += ", " + headerId;
                    else
                        headerListMsg = " " + headerId;
                }
                MessageBox.Show(err.Message + headerListMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
			// End TT#1019 - MD - stodd - prohibit allocation actions against GA - 
			catch(Exception err)
			{
				HandleException(err);
			}
			finally
			{
                // begin TT#981 - MD - Jellis - GA Size Need Gets Argument Out of Range
                if (!useAssortmentHeaders)
                {
                    ApplicationTransaction.DequeueHeaders();
                    ResetApplicationSessionTransaction();
                }
                //// begin TT#1185 - JEllis - Verify ENQ before Update
                //ApplicationTransaction.DequeueHeaders();
                //// end TT#1185 - Jellis - Verify ENQ before Update
                //ResetApplicationSessionTransaction();
                // end TT#981 - MD - Jellis - GA Size Need Gets Argument Out of Range
				Cursor.Current = Cursors.Default;
			}
		}

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
        // Begin TT#1034 - MD - stodd - Resource Locks are GONE after Group Allocation Velocity - 
        /// <summary>
        /// Returns the transaction for the Assortment or Group Allocation Review indicated by the assortment RID.
        /// </summary>
        /// <param name="asrtRid"></param>
        /// <returns></returns>
		public ApplicationSessionTransaction GetAssortmentViewWindowTransaction(int asrtRid)
		// BEGIN TT#371-MD - stodd -  Velocity Interactive on Assortment
		{
			try
			{
				foreach (Form childForm in _EAB.WorkflowMethodExplorer.MainMDIForm.MdiChildren)
				{
					if (childForm.GetType() == typeof(AssortmentView))
					{
                        if (((AssortmentView)childForm).AssortmentRid == asrtRid)
                        {
                            return ((AssortmentView)childForm).Transaction;
                        }
					}
                    // Begin TT#1208-MD - remove GroupAllocationView Module - 
                    //if (childForm.GetType() == typeof(GroupAllocationView))
                    //{
                    //    if (((AssortmentView)childForm).AssortmentRid == asrtRid)
                    //    {
                    //        return ((AssortmentView)childForm).Transaction;
                    //    }
                    //}
                    // End TT#1208-MD - remove GroupAllocationView Module - 
				}

				return null;
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}
        // End TT#1034 - MD - stodd - Resource Locks are GONE after Group Allocation Velocity - 
		// END TT#217-MD - stodd - unable to run workflow methods against assortment

		// BEGIN TT#696-MD - Stodd - add "active process"
		//public bool UseAssortmentSelectedHeaders()
		//{
		//    bool useAssortment = false;
		//    try
		//    {
		//        foreach (Form childForm in _EAB.WorkflowMethodExplorer.MainMDIForm.MdiChildren)
		//        {
		//            if (childForm.GetType() == typeof(AssortmentView))
		//            {
		//                useAssortment = true;
		//                break;
		//            }
		//        }

		//        return useAssortment;
		//    }
		//    catch (Exception error)
		//    {
		//        string message = error.ToString();
		//        throw;
		//    }
		//}
		// END TT#696-MD - Stodd - add "active process"

		protected void ProcessWorkflow()
		{
			try
			{
				ProcessWorkflow(false);
			}
			catch
			{
				throw;
			}
		}

		protected void ProcessWorkflow(bool aFromExplorer)
		{
			bool useAssortment = false;  // TT#698-MD - Stodd - add ability for workflows to be run against assortments.
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				// make sure you get a new transaction each time
				// BEGIN TT#696-MD - Stodd - add "active process"
				if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// Use assortment
				{
					// We'll want to use the assortment transaction
					useAssortment = true;
				}
				else
				{
					ResetApplicationSessionTransaction();
				}
				// END TT#696-MD - Stodd - add "active process"
				if (aFromExplorer)
				{
					SetObject();
				}
				else
				{
					SaveChanges();
				}
				if (!ErrorFound)
				{
					if (_ABW.WorkFlowType == eWorkflowType.Allocation)
					{
						// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
						if (useAssortment)
						{
							ApplicationSessionTransaction assortTrans = SAB.AssortmentTransactionEvent.GetAssortmentTransaction(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID);
                            SelectedHeaderList shl = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.GroupAllocation);	// TT#1098 - MD - stodd - add Group Allocation Method to workflow screen - 
							assortTrans.ProcessAllocationWorkflow(_ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
							eAllocationActionStatus actionStatus = assortTrans.AllocationActionAllHeaderStatus;
                            UpdateAllocationWorkspace(assortTrans);	// TT#1142-MD - stodd - after workflow header status are not changing - 
							string message = MIDText.GetTextOnly((int)actionStatus);
							MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
							if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
								|| actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
							{
								CloseForms();
							}
						}
						else
						// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.
						{
							if (VerifySecurity())
							{
								// begin TT#1185 - Verify ENQ before Updat
								if (EnqueueHeadersForProcessing())
								{
									string enqMessage = string.Empty;
									if (ApplicationTransaction.EnqueueSelectedHeaders(out enqMessage))
									{
										ApplicationTransaction.ProcessAllocationWorkflow(_ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
										eAllocationActionStatus actionStatus = ApplicationTransaction.AllocationActionAllHeaderStatus;
										UpdateAllocationWorkspace();
										string message = MIDText.GetTextOnly((int)actionStatus);
										MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
										// begin TT#241 - MD - JEllis - Header Enqueue Process
										if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
											|| actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
										{
											CloseForms();
										}
										// end TT#241 - MD - JEllis - Header Enqueue Process
									}
									else
									{
										ApplicationTransaction.SAB.MessageCallback.HandleMessage(
											enqMessage,
											"Header Lock Conflict",
											System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
									}
								}
								else
								{
									ApplicationTransaction.ProcessAllocationWorkflow(_ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
									eAllocationActionStatus actionStatus = ApplicationTransaction.AllocationActionAllHeaderStatus;
									UpdateAllocationWorkspace();
									string message = MIDText.GetTextOnly((int)actionStatus);
									MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
									// begin TT#241 - MD - JEllis - Header Enqueue Process
									if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
										|| actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
									{
										CloseForms();
									}
									// end TT#241 - MD - JEllis - Header Enqueue Process
								}
								//ApplicationTransaction.ProcessAllocationWorkflow(_ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
								//eAllocationActionStatus actionStatus = ApplicationTransaction.AllocationActionAllHeaderStatus;
								// removed unnecessary comments // TT#1185 - Verify ENQ before Update
								//UpdateAllocationWorkspace();
								//string message = MIDText.GetTextOnly((int)actionStatus);
								//MessageBox.Show(message,this.Text,MessageBoxButtons.OK, MessageBoxIcon.Information);
								// end TT#1185 - JEllis - Verify ENQ before Update
							}
							else
							{
								MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
							}
						}
					}
					else
					{
						ApplicationTransaction.ProcessOTSPlanWorkflow(_ABW.Key, true, true, 1);

						eOTSPlanActionStatus actionStatus = ApplicationTransaction.OTSPlanActionStatus;
						string message = MIDText.GetTextOnly((int)actionStatus);
						MessageBox.Show(message,this.Text,MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}
			catch (MIDException MIDexc)
			{
				HandleMIDException(MIDexc);
			}
			catch(Exception err)
			{
				HandleException(err);
			}
			finally
			{
				// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				//======================================================================
				// We want to dequeue and reset the trans if we did NOT use assortment 
				//======================================================================
				if (!useAssortment)
				{
					ApplicationTransaction.DequeueHeaders();  // TT#1185 - Verify ENQ before update
					ResetApplicationSessionTransaction();
				}
				// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.
				Cursor.Current = Cursors.Default;
			}
		}

		virtual public bool VerifySecurity()
		{
            HierarchyNodeSecurityProfile hierNodeSecProfile; // TT#2 - RMatelic - Assortment Planning
			try
			{
				bool allowUpdate = true;
				SelectedHeaderList selectedHeaderList = (SelectedHeaderList)ApplicationTransaction.GetSelectedHeaders();
				foreach (SelectedHeaderProfile shp in selectedHeaderList)
				{
                    // Begin TT#2 - RMatelic - Assortment Planning >> changed to do own security lookup  
                    //if (!_EAB.AllocationWorkspaceExplorer.IsHeaderUpdateable(shp.Key))
                    //{
                    //    allowUpdate = false;
                    //    break;
                    //}
                    hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(shp.StyleHnRID, (int)eSecurityTypes.Allocation);
                    if (!hierNodeSecProfile.AllowUpdate)
                    {
                        allowUpdate = false;
                        break;
                    }
                    // End TT#2
                }
				return allowUpdate;
			}
			catch
			{
				throw;
			}
		}
        // begin TT#241 - MD - JEllis - Header Enqueue Process
        private void CloseForms()
        {
            MIDRetail.Windows.StyleView frmStyleView;
            MIDRetail.Windows.SummaryView frmSummaryView;
            MIDRetail.Windows.SizeView frmSizeView;
            MIDRetail.Windows.AssortmentView frmAssortmentView;
            MIDRetail.Windows.frmVelocityMethod frmVelocityMethod;
            try
            {
                if (ApplicationTransaction.StyleView != null)
                {
                    frmStyleView = (MIDRetail.Windows.StyleView)ApplicationTransaction.StyleView;
                    if (ErrorFound)
                    {
                        frmStyleView.ErrorFound = true;
                    }
                    frmStyleView.Close();
                }
                if (ApplicationTransaction.SummaryView != null)
                {
                    frmSummaryView = (MIDRetail.Windows.SummaryView)ApplicationTransaction.SummaryView;
                    if (ErrorFound)
                    {
                        frmSummaryView.ErrorFound = true;
                    }
                    frmSummaryView.Close();
                }
                if (ApplicationTransaction.SizeView != null)
                {
                    frmSizeView = (MIDRetail.Windows.SizeView)ApplicationTransaction.SizeView;
                    if (ErrorFound)
                    {
                        frmSizeView.ErrorFound = true;
                    }
                    frmSizeView.Close();
                }
                if (ApplicationTransaction.AssortmentView != null)
                {
                    frmAssortmentView = (MIDRetail.Windows.AssortmentView)ApplicationTransaction.AssortmentView;
                    if (ErrorFound)
                    {
                        frmAssortmentView.ErrorFound = true;
                    }
                    frmAssortmentView.Close();
                }
                if (ApplicationTransaction.VelocityWindow != null)
                {
                    frmVelocityMethod = (MIDRetail.Windows.frmVelocityMethod)ApplicationTransaction.VelocityWindow;
                    if (ErrorFound)
                    {
                        frmVelocityMethod.ErrorFound = true;
                    }
                    frmVelocityMethod.Close();
                }
                Close();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // end TT#241 - MD - JEllis - Header Enqueue Process

		// Begin TT#1142-MD - stodd - after workflow header status are not changing - 
        private void UpdateAllocationWorkspace()
        {
            UpdateAllocationWorkspace(ApplicationTransaction);
        }


		private void UpdateAllocationWorkspace(ApplicationSessionTransaction aTrans)
		// End TT#1142-MD - stodd - after workflow header status are not changing - 
		{
			try
			{
				// BEGIN MID Track #4315 - JSmith - do not update Workspace if no headers selected
				// make sure headers were selected
//				_EAB.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(ApplicationTransaction.GetAllocationProfileKeys());
				// begin MID Track 5067 Release Action Fails yet header is released
				//int[] keys = ApplicationTransaction.GetAllocationProfileKeys();
                int[] keys = aTrans.GetChangedHeaderKeys();		// TT#1142-MD - stodd - after workflow header status are not changing - 
				// end MID Track 5067 Release Action Fails yet header is released
				if (keys != null && keys.Length > 0)
				{
					_EAB.AllocationWorkspaceExplorer.ReloadUpdatedHeaders(keys);
                
                    // Begin TT#1465 - RMatelic - Methods/workflows need to update Assortment Review
                    CheckForAssortmentReview(keys);
                    // End TT#1465
				}
				// END MID Track #4315
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}

		}

        // Begin TT#1465 - RMatelic - Methods/workflows need to update Assortment Review
        private void CheckForAssortmentReview(int[] aKeys)
        {
            try
            {
                // Begin TT#1448 - RMatelic - Created a WUB and attached to an Assortment.. Received an unhandled exception when trying to process the Velocity Method
                //                 not the specific error reported in issue but found when testing it.
                //foreach (Form childForm in this.MdiParent.MdiChildren)
                foreach (Form childForm in _EAB.Explorer.MdiChildren)
                // End TT#1448
                {
                    if (childForm.GetType().Equals(typeof(AssortmentView)))
                    {
                        AssortmentView frm = (AssortmentView)childForm;
                        frm.CheckHeaderListForUpdate(aKeys, true); 	// TT#1197-MD - stodd - header status not getting updated correctly - 
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#1465

		protected void BuildDataTables()
		{
			try
			{		
				_merchandiseDataTable = MIDEnvironment.CreateDataTable();
				DataColumn myDataColumn;
				DataRow myDataRow;
				// Create new DataColumn, set DataType, ColumnName and add to DataTable.  
				// Level sequence number
				myDataColumn = new DataColumn();
				myDataColumn.DataType = System.Type.GetType("System.Int32");
				myDataColumn.ColumnName = "seqno";
				myDataColumn.ReadOnly = false;
				myDataColumn.Unique = true;				
				_merchandiseDataTable.Columns.Add(myDataColumn);
 
				// Create second column - enum name.
				//Create Merchandise types - eMerchandiseType
				myDataColumn = new DataColumn();
				myDataColumn.DataType = System.Type.GetType("System.Int32");
				myDataColumn.ColumnName = "leveltypename";
				myDataColumn.AutoIncrement = false;
				myDataColumn.ReadOnly = false;
				myDataColumn.Unique = false;				
				_merchandiseDataTable.Columns.Add(myDataColumn);

				// Create third column - text
				myDataColumn = new DataColumn();
				myDataColumn.DataType = System.Type.GetType("System.String");
				myDataColumn.ColumnName = "text";
				myDataColumn.AutoIncrement = false;
				myDataColumn.ReadOnly = false;
				myDataColumn.Unique = false;				
				_merchandiseDataTable.Columns.Add(myDataColumn);

				// Create fourth column - Key
				myDataColumn = new DataColumn();
				myDataColumn.DataType = System.Type.GetType("System.Int32");
				myDataColumn.ColumnName = "key";
				myDataColumn.AutoIncrement = false;
				myDataColumn.ReadOnly = false;
				myDataColumn.Unique = false;				
				_merchandiseDataTable.Columns.Add(myDataColumn);

				_hp = SAB.HierarchyServerSession.GetMainHierarchyData();

				//Default Selection to OTSPlanLevel
				myDataRow = _merchandiseDataTable.NewRow();
				myDataRow["seqno"] = 0;
				myDataRow["leveltypename"] = eMerchandiseType.OTSPlanLevel;
				myDataRow["text"] = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
				myDataRow["key"] = Include.Undefined;
				_merchandiseDataTable.Rows.Add(myDataRow);	

				for (int levelIndex = 1; levelIndex <= _hp.HierarchyLevels.Count; levelIndex++)
				{
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hp.HierarchyLevels[levelIndex];
					//hlp.LevelID is level name 
					//hlp.Level is level number 
					//hlp.LevelType is level type 
					// BEGIN MID Track #4094 - as part of this Track, exclude size level from drop down list
					if (hlp.LevelType != eHierarchyLevelType.Size)
					{
						myDataRow = _merchandiseDataTable.NewRow();
						myDataRow["seqno"] = hlp.Level;
						myDataRow["leveltypename"] = eMerchandiseType.HierarchyLevel;
						myDataRow["text"] = hlp.LevelID;
						myDataRow["key"] = hlp.Key;
						_merchandiseDataTable.Rows.Add(myDataRow);    
					}	
					// END MID Track #4094
				}						      
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error in BuildDataTable");
				HandleException(ex);
			}
		}

		/// <summary>
		/// Used to move common method fields to the method object
		/// </summary>
		private void LoadCommonFields()
		{
			if (WorkflowMethodInd() == eWorkflowMethodIND.Methods || WorkflowMethodInd() == eWorkflowMethodIND.SizeMethods)
			{
				_ABM.MethodStatus = eMethodStatus.ValidMethod;

				_ABM.Name = _workflowMethodName;

				_ABM.Method_Description = _workflowMethodDescription;
			
				if (GlobalRadioButton.Checked)
				{
					_ABM.User_RID = Include.GetGlobalUserRID();
				}
				else
				{
					_ABM.User_RID = SAB.ClientServerSession.UserRID;
				}
			}
			else
			{
				_ABW.WorkFlowName = _workflowMethodName;

				_ABW.WorkFlowDescription = _workflowMethodDescription;
			
				if (GlobalRadioButton.Checked)
				{
					_ABW.UserRID = Include.GetGlobalUserRID();
				}
				else
				{
					_ABW.UserRID = SAB.ClientServerSession.UserRID;
				}
			}
		}

		/// <summary>
		/// Used to get workflows for Allocation methods
		/// </summary>
		protected void GetWorkflows(int aMethodRID, UltraGrid aUltraGrid)
		{
			WorkflowBaseData GenAllocWkfl = new WorkflowBaseData();
			// Begin MID ISssue #3501 - stodd
			aUltraGrid.DataSource = GenAllocWkfl.GetAllocMethodPropertiesUIWorkflows(aMethodRID);
            SetWorkflowColumnAttributes(aUltraGrid);
		}

        /// <summary>
        /// Used to get workflows for OTS Plan methods
        /// </summary>
        protected void GetOTSPLANWorkflows(int aMethodRID, UltraGrid aUltraGrid)
        {
            WorkflowBaseData GenOTSWkfl = new WorkflowBaseData();
            aUltraGrid.DataSource = GenOTSWkfl.GetOTSMethodPropertiesUIWorkflows(aMethodRID);
            SetWorkflowColumnAttributes(aUltraGrid);
            aUltraGrid.DisplayLayout.Bands[0].Columns["USER_NAME"].Hidden = true;
        }

        private void SetWorkflowColumnAttributes(UltraGrid aUltraGrid)
        {
            aUltraGrid.DisplayLayout.Bands[0].Columns["WORKFLOW_NAME"].Header.Caption = "Name";
            aUltraGrid.DisplayLayout.Bands[0].Columns["WORKFLOW_DESCRIPTION"].Header.Caption = "Description";
            aUltraGrid.DisplayLayout.Bands[0].Columns["USER_FULLNAME"].Header.Caption = "User";
            aUltraGrid.DisplayLayout.Bands[0].Columns["WORKFLOW_USER_RID"].Hidden = true;
            aUltraGrid.Text = MIDText.GetTextOnly(Convert.ToInt32(eWorkflowMethodIND.Workflows));
            aUltraGrid.DisplayLayout.Bands[0].Columns["WORKFLOW_NAME"].Width = 200;
            aUltraGrid.DisplayLayout.Bands[0].Columns["WORKFLOW_DESCRIPTION"].Width = 250;
            aUltraGrid.DisplayLayout.Bands[0].Columns["USER_FULLNAME"].Width = 150;

            if (!FunctionSecurity.AllowUpdate)
            {
                foreach (UltraGridBand ugr in aUltraGrid.DisplayLayout.Bands)
                {
                    ugr.Override.AllowDelete = DefaultableBoolean.False;
                }
            }
        }

		// begin MID Track # 2376 stodd  
		/// <summary>
		/// after a new Method is processed, this allows the explorer node to be
		/// updated with the newly added method node (instead of the parent node)
		/// </summary>
		/// <param name="aNode"></param>
		public void SetExplorerNode(MIDWorkflowMethodTreeNode aNode)
		{
			_explorerNode = aNode;
			_workflowMethodIND = aNode.WorkflowMethodIND;
			_workflowMethodRID = aNode.Key;
		}
		// end MID Track # 2376 stodd  

		public void UpdateExplorer(ApplicationBaseMethod aABM)
		{
			MIDWorkflowMethodTreeNode explorerNode = null;
			if (_explorerNode == null)
			{
				explorerNode = GetExplorerNode();
			}
			else
			{
				explorerNode = _explorerNode;
			}
			PropertyChangeEventArgs ea = new PropertyChangeEventArgs(aABM, explorerNode);
			if (OnPropertyChangeHandler != null)  // throw event to explorer to make changes
			{
				OnPropertyChangeHandler(this, ea);
			}
		}

		public void UpdateExplorer(ApplicationBaseWorkFlow aABW)
		{
			MIDWorkflowMethodTreeNode explorerNode = null;
			if (_explorerNode == null)
			{
				explorerNode = GetExplorerNode();
			}
			else
			{
				explorerNode = _explorerNode;
			}
			PropertyChangeEventArgs ea = new PropertyChangeEventArgs(aABW, explorerNode);
			if (OnPropertyChangeHandler != null)  // throw event to explorer to make changes
			{
				OnPropertyChangeHandler(this, ea);
			}
		}

		private void WorkflowMethodFormBase_Closing(object sender, CancelEventArgs e)
		{
			UnlockWorkflowMethod();
		}

		private void UnlockWorkflowMethod()
		{
			if (_lockStatus == eLockStatus.Locked)
			{
				if (_workflowMethodIND == eWorkflowMethodIND.Workflows)
				{
					WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
						_workflowMethodRID,
						SAB.ClientServerSession.UserRID,
						SAB.ClientServerSession.ThreadID);

					try
					{
						workflowEnqueue.DequeueWorkflow();
						_lockStatus = eLockStatus.Undefined;
					}
					catch (Exception ex) 
					{
						HandleException(ex);
					}
				}
				else
				{
					MethodEnqueue methodEnqueue = new MethodEnqueue(
						_workflowMethodRID,
						SAB.ClientServerSession.UserRID,
						SAB.ClientServerSession.ThreadID);

					try
					{
						methodEnqueue.DequeueMethod();
						_lockStatus = eLockStatus.Undefined;
					}
					catch (Exception ex) 
					{
						HandleException(ex);
					}
				}
			}
		}

		#endregion Methods\

		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		override public void ISaveAs()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
                // Begin Track #5905 - JSmith - Close after Save As
                //SaveAsMethodWorkflow();
                if (SaveAsMethodWorkflow())
                {
                    // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
                    //Close();
                    if (_closeOnSave)
                     {
                        Close();
                    }
                    // End TT#330
                }
                // End Track #5905
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

		private void InitializeComponent()
		{
			this.btnClose = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnProcess = new System.Windows.Forms.Button();
			this.pnlGlobalUser = new System.Windows.Forms.Panel();
			this.radGlobal = new System.Windows.Forms.RadioButton();
			this.radUser = new System.Windows.Forms.RadioButton();
			this.txtDesc = new System.Windows.Forms.TextBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.pnlGlobalUser.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(670, 232);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 8;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(574, 232);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "&Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnProcess
			// 
			this.btnProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnProcess.Location = new System.Drawing.Point(63, 220);
			this.btnProcess.Name = "btnProcess";
			this.btnProcess.Size = new System.Drawing.Size(75, 23);
			this.btnProcess.TabIndex = 9;
			this.btnProcess.Text = "&Process";
			this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
			// 
			// pnlGlobalUser
			// 
			this.pnlGlobalUser.Controls.Add(this.radGlobal);
			this.pnlGlobalUser.Controls.Add(this.radUser);
			this.pnlGlobalUser.Location = new System.Drawing.Point(596, 12);
			this.pnlGlobalUser.Name = "pnlGlobalUser";
			this.pnlGlobalUser.Size = new System.Drawing.Size(116, 28);
			this.pnlGlobalUser.TabIndex = 17;
			// 
			// radGlobal
			// 
			this.radGlobal.Location = new System.Drawing.Point(4, 4);
			this.radGlobal.Name = "radGlobal";
			this.radGlobal.Size = new System.Drawing.Size(56, 20);
			this.radGlobal.TabIndex = 7;
			this.radGlobal.Text = "Global";
			this.radGlobal.CheckedChanged += new System.EventHandler(this.radGlobal_CheckedChanged);
			// 
			// radUser
			// 
			this.radUser.Location = new System.Drawing.Point(68, 4);
			this.radUser.Name = "radUser";
			this.radUser.Size = new System.Drawing.Size(56, 20);
			this.radUser.TabIndex = 8;
			this.radUser.Text = "User";
			this.radUser.CheckedChanged += new System.EventHandler(this.radUser_CheckedChanged);
			// 
			// txtDesc
			// 
			this.txtDesc.Location = new System.Drawing.Point(303, 16);
			this.txtDesc.Name = "txtDesc";
			this.txtDesc.Size = new System.Drawing.Size(287, 20);
			this.txtDesc.TabIndex = 16;
			this.txtDesc.TextChanged += new System.EventHandler(this.txtDesc_TextChanged);
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(107, 16);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(190, 20);
			this.txtName.TabIndex = 15;
			this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(6, 17);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(95, 16);
			this.lblName.TabIndex = 14;
			this.lblName.Text = "Name:";
			this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// WorkflowMethodFormBase
			// 
			this.AllowDragDrop = true;
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(758, 266);
			this.Controls.Add(this.btnProcess);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.pnlGlobalUser);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.txtDesc);
			this.Controls.Add(this.txtName);
			this.Name = "WorkflowMethodFormBase";
			this.Load += new System.EventHandler(this.WorkflowMethodFormBase_Load);
			this.Controls.SetChildIndex(this.txtName, 0);
			this.Controls.SetChildIndex(this.txtDesc, 0);
			this.Controls.SetChildIndex(this.lblName, 0);
			this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.btnClose, 0);
			this.Controls.SetChildIndex(this.btnProcess, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.pnlGlobalUser.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#region Exception Handling
		protected void HandleMIDException(MIDException MIDexc)
		{
			string Title = this.Text, errLevel, Msg; 
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
			Title += " - ";
			if (MIDexc.InnerException != null)
			{
				Title += errLevel + " - " + MIDexc.Message;
				Msg = MIDexc.InnerException.Message;
			}
			else
			{
				Title += errLevel;
				Msg = MIDexc.Message;
			}
			MessageBox.Show(this, Msg, Title,
				buttons, icon );
		} 
		#endregion Exception Handling

		// Begin MID Track 4858 - JSmith - Security changes
		private void btnProcess_Click(object sender, System.EventArgs e)
		{
            // begin TT#1185 - JEllis - Verify ENQ before Update
            //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
            //bool headersEnqueued = false;
            //// End TT#1163
            // end TT#1185 - JEllis - Verify ENQ before Update
            try
            {
                // begin TT#1185 - JEllis - Verify ENQ before Update (Do ENQ only when the Transaction that will do the process is known)
                Call_btnProcess_Click();
                //// Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
                ////Call_btnProcess_Click();
                //if (EnqueueHeadersForProcessing())
                //{
                //    if (_headerEnqueueTransaction.EnqueueSelectedHeaders())
                //    {
                //        headersEnqueued = true;
                //        Call_btnProcess_Click();
                //    }
                //    else
                //    {
                //        _headerEnqueueTransaction.SAB.MessageCallback.HandleMessage(
                //            _headerEnqueueTransaction.HeaderEnqueueMessage,
                //            "Header Lock Conflict",
                //            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
                //    }
                //}
                //else
                //{
                //    Call_btnProcess_Click();
                //}
                //// End TT#1163
                // end TT#1185 - JEllis - Verify ENQ before Update (Do ENQ only when the Transaction that will do the process is known)

            }
            catch (Exception ex)
            {
                HandleException(ex, "btnProcess_Click");
            }
            // Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
            finally
            {
                // begin TT#1185 - JEllis - Verify ENQ before Update
                //if (headersEnqueued)
                //{
                //    _headerEnqueueTransaction.DequeueHeaders();
                //}
                // end TT#1185 - JEllist Verify ENQ before Update
            }
            // End TT#1163
		}

        // Begin TT#1163 - JSmith - Headers are not locked when processing Methods or Workflows
        private bool EnqueueHeadersForProcessing()
        {
            if (this.GetType() == typeof(frmAllocationWorkflow) ||
                this.GetType() == typeof(GeneralAllocationMethod) ||
                this.GetType() == typeof(frmOverrideMethod) ||
                this.GetType() == typeof(frmRuleMethod) ||
                this.GetType() == typeof(frmVelocityMethod) ||
                this.GetType() == typeof(frmBasisSizeMethod) ||
                this.GetType() == typeof(frmFillSizeHolesMethod) ||
                this.GetType() == typeof(frmSizeNeedMethod) ||
                this.GetType() == typeof(frmDCFulfillmentMethod) ||  // TT#1966-MD - JSmith - DC Fulfillment
                // Begin TT#1652-MD - stodd - DC Carton Rounding
                this.GetType() == typeof(frmSizeWarehouseMethod) ||
                this.GetType() == typeof(frmDCCartonRoundingMethod))
                // End TT#1652-MD - stodd - DC Carton Rounding
            {
                // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>>temporary
                //return true;
                return DoEnqueue();
                // End TT#1442
            }

            return false;
        }
        // End TT#1163
        
        // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>>temporary
        private bool DoEnqueue()
        {
            bool doEnqueue = true;
            try
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    if (shp.HeaderType != eHeaderType.Assortment)
                    {
                        if (shp.BypassEnqueue)
                        {
                            doEnqueue = false;
                        }
                        else
                        {
                            doEnqueue = true;
                            break;
                        }
                    }
                    else
                    {
                        selectedHeaderList.Remove(shp);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            return doEnqueue;
        }

        // End TT#1442  

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				Call_btnSave_Click();
			}
			catch( Exception ex )
			{
				HandleException(ex, "btnSave_Click");
			}
		}

		protected void btnSave_Click()
		{
			try
			{
				Save_Click(true);
				DoValueByValueEdits = true;
				// Begin MID Track #4969 - JSmith - Constraint error on save
				if (!ErrorFound)
				{
				// End MID Track #4969
					if (_workflowMethodType == eWorkflowMethodType.Workflow)
					{
						_ABW.Workflow_Change_Type = eChangeType.update;
					}
					else
					{
						_ABM.Method_Change_Type = eChangeType.update;
					}
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				// Begin MID Track #4969 - JSmith - Constraint error on save
				}
				// End MID Track #4969
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cancel_Click();
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void WorkflowMethodFormBase_Load(object sender, System.EventArgs e)
		{
			try
			{
				// do not execute if in designer
				if (_inDesigner)
				{
					return;
				}
				FormLoaded = false;
				SetText();

                // Begin Track #4872 - JSmith - Global/User Attributes
                _storeUserAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                // End Track #4872
				
				FormLoaded = true;
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void SetText()
		{
			try
			{
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
				if ( _workflowMethodType == eWorkflowMethodType.Workflow)
				{
					//Begin TT#155 - JScott - Size Curve Method
					//this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);
					if (_workflowMethodNameLabel != string.Empty)
					{
						this.lblName.Text = _workflowMethodNameLabel;
					}
					else
					{
						this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Workflow);
					}

					//End TT#155 - JScott - Size Curve Method
					this.txtName.Text = _ABW.WorkFlowName;
					this.txtDesc.Text = _ABW.WorkFlowDescription;
					if (_ABW.Workflow_Change_Type == eChangeType.update)
					{
						this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
					}
					else
					{
						this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
					}
				}
				else
				{
					//Begin TT#155 - JScott - Size Curve Method
					//this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
					if (_workflowMethodNameLabel != string.Empty)
					{
						this.lblName.Text = _workflowMethodNameLabel;
					}
					else
					{
						this.lblName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
					}

					//End TT#155 - JScott - Size Curve Method
                    //Begin TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference)
                    if (ABM != null)
                    {
                        this.txtName.Text = _ABM.Name;
                        this.txtDesc.Text = _ABM.Method_Description;

                        if (_ABM.Method_Change_Type == eChangeType.update)
                        {
                            this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                        }
                        else
                        {
                            this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                        }
                    }
                    else
                    {
                        this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                    }
                    //End TT#1235-MD -jsobek -Size Curve Null Reference Exception (Object Reference)
				}
				this.lblName.Text += ":";
				this.radGlobal.Text = MIDText.GetTextOnly((int)eGlobalUserType.Global);
				this.radUser.Text = MIDText.GetTextOnly((int)eGlobalUserType.User);
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Set text in the title bar of the window as it's changed in the textbox - [New] - if blank.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtName_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if ( _workflowMethodType == eWorkflowMethodType.Workflow)
					{
						if (_ABW.Workflow_Change_Type == eChangeType.add)
						{
							Format_Title(eDataState.New, _formName, txtName.Text);
						}
						else
						{
							Format_Title(eDataState.Updatable, _formName, txtName.Text);
						}
					}
					else
					{
						if (_ABM.Method_Change_Type == eChangeType.add)
						{
							Format_Title(eDataState.New, _formName, txtName.Text);
						}
						else
						{
							Format_Title(eDataState.Updatable, _formName, txtName.Text);
						}
					}
					ChangePending = true;
					NameChanged = true;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}	
		}

		private void txtDesc_TextChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void radGlobal_CheckedChanged(object sender, System.EventArgs e)
		{
		
			if (radGlobal.Checked)
			{
				FunctionSecurity = GlobalSecurity;
			}
			
			if (FormLoaded)
			{
                ApplySecurity(); //Begin Track #5858 - KJohnson - Validating store security only
				ChangePending = true;
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (!_storeUserAttrSecLvl.AccessDenied)
                {
                    if (radGlobal.Checked)
                    {
                        BuildAttributeList();
                    }
                }
                // End Track #4872
			}
		}

		private void radUser_CheckedChanged(object sender, System.EventArgs e)
		{
			if (radUser.Checked)
			{
				FunctionSecurity = UserSecurity;
			}
			
			if (FormLoaded)
			{
                ApplySecurity(); //Begin Track #5858 - KJohnson - Validating store security only
				ChangePending = true;
                // Begin Track #4872 - JSmith - Global/User Attributes
                if (!_storeUserAttrSecLvl.AccessDenied)
                {
                    if (radUser.Checked)
                    {
                        BuildAttributeList();
                    }
                }
                // End Track #4872
			}
		
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        /// <summary>
        /// Use to get if workflow or method
        /// </summary>
        virtual protected void BuildAttributeList()
        {
            throw new Exception("Can not call base method");
        }
        // End Track #4872

		// Begin Track 5871 stodd
		/// <summary>
		/// Return is to designate to say whether security should cause an
		/// error in ValidateSpecificFields().
		/// </summary>
		virtual protected bool ApplySecurity()
		{
			throw new Exception("Can not call base method");
		}
		// End Track 5871 stodd
		// End MID Track 4858

        // Begin TT#330 - RMatelic -Velocity Method View when do a Save As of a view after the Save the method closes
        /// <summary>
        /// Update any other necessary fields like new SaveAs View
        /// </summary>
        virtual protected void UpdateAdditionalData()
        {
            throw new Exception("Can not call base method");
        }
        // End TT#330 
	}

	#region PropertyChangeEventArgs Class

	public class PropertyChangeEventArgs : EventArgs
	{
		eWorkflowMethodIND _workflowMethodInd;
		ApplicationBaseMethod _ABM;
		ApplicationBaseWorkFlow _ABW;
		MIDWorkflowMethodTreeNode _explorerNode;
		Profile _p;
		bool _formClosing;
		
		public PropertyChangeEventArgs(ApplicationBaseMethod ABM, MIDWorkflowMethodTreeNode aExplorerNode)
		{
			//_workflowMethodInd = eWorkflowMethodIND.Methods;
			_workflowMethodInd = aExplorerNode.WorkflowMethodIND;
			_ABM = ABM;
			_ABW = null;
			_explorerNode = aExplorerNode;
			_formClosing = false;
		}
		public PropertyChangeEventArgs(ApplicationBaseWorkFlow ABW, MIDWorkflowMethodTreeNode aExplorerNode)
		{
			_workflowMethodInd = eWorkflowMethodIND.Workflows;
			_ABM = null;
			_ABW = ABW;
			_explorerNode = aExplorerNode;
			_formClosing = false;
		}
		public PropertyChangeEventArgs(Profile p)
		{
			_p = p;
			_formClosing = false;
		}
		public bool FormClosing 
		{
			get { return _formClosing ; }
			set { _formClosing = value; }
		}
		public Profile p
		{
			get { return _p ; }
			set { _p = value; }
		}
		public eWorkflowMethodIND WorkflowMethodInd
		{
			get { return _workflowMethodInd ; }
			set { _workflowMethodInd = value; }
		}
		public ApplicationBaseMethod ABM 
		{
			get { return _ABM ; }
			set { _ABM = value; }
		}
		public ApplicationBaseWorkFlow ABW 
		{
			get { return _ABW ; }
			set { _ABW = value; }
		}
		public MIDWorkflowMethodTreeNode ExplorerNode
		{
			get { return _explorerNode ; }
			set { _explorerNode = value; }
		}
	}

	#endregion PropertyChangeEventArgs Class
}
