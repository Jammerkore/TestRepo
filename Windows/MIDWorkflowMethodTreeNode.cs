// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Diagnostics;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;
using System.Text.RegularExpressions;   //TT#110-MD-VStuart - In Use Tool

namespace MIDRetail.Windows
{
	public class WorkflowMethodTreeView : MIDTreeView
	{
        private int cClosedFolderImage;
        private int cOpenFolderImage;
        private int cClosedShortcutFolderImage;
        private int cOpenShortcutFolderImage;
        private int cSharedClosedFolderImage;
        private int cSharedOpenFolderImage;
        private int cUserImage;
        private int cGlobalImage;
        private int cGlobalShortcutImage;
        private int cUserShortcutImage;
        private int cSharedGlobalImage;
        private int cSharedUserImage;
        private int cFavoritesImage;

        private string _sForecastFolder;
        private string _sForecastMethodsFolder;
        private string _sForecastOTSPlan;
        private string _sForecastOTSBalance;
        private string _sForecastSpread;
        private string _sForecastCopyChain;
        private string _sForecastCopyStore;
        private string _sForecastExport;
        // Begin TT#2131-MD - JSmith - Halo Integration
        private string _sForecastPlanningExtract;
        // End TT#2131-MD - JSmith - Halo Integration
        private string _sForecastModifySales;
        private string _sForecastGlobalUnlock;
        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        private string _sForecastGlobalLock;
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
        private string _sForecastRollup;
        private string _sForecastWorkflowsFolder;

        private string _sAllocationFolder;
        private string _sAllocationMethodsFolder;
        private string _sAllocationGeneralAllocation;
        private string _sAllocationAllocationOverride;
        private string _sAllocationRule;
        private string _sAllocationVelocity;
        private string _sAllocationFillSizeHoles;
        private string _sAllocationSizeMethodsFolder;
        private string _sAllocationBasisSize;
        private string _sAllocationSizeNeed;
        // Begin TT#155 - JSmith - Size Curve Method
        private string _sAllocationSizeCurve;
        // End TT#155

        // Begin TT#370 - APicchetti - Build Packs Method
        private string _sAllocationBuildPacks;
        // End TT#370
		private string _sAllocationGroupAllocation;		// TT#708-MD - Stodd - Group Allocation Prototype.

        private string _sAllocationWorkflowsFolder;

        private string _sAllocationDCCartonRounding;	// TT#1652-MD - stodd - DC Carton Rounding

        private string _sAllocationCreateMasterHeaders;	// TT#1966-MD - JSmith- DC Fulfillment
        private string _sAllocationDCFulfillment;	// TT#1966-MD - JSmith- DC Fulfillment

        private FunctionSecurityProfile _forecastGlobal;
        private FunctionSecurityProfile _forecastUser;
        private FunctionSecurityProfile _forecastMethodsGlobal;
        private FunctionSecurityProfile _forecastMethodsUser;
        private FunctionSecurityProfile _forecastMethodsGlobalOTSPlan;
		private FunctionSecurityProfile _forecastMethodsUserOTSPlan;
		private FunctionSecurityProfile _forecastMethodsGlobalOTSBalance;
		private FunctionSecurityProfile _forecastMethodsUserOTSBalance;
		private FunctionSecurityProfile _forecastMethodsGlobalSpread;
		private FunctionSecurityProfile _forecastMethodsUserSpread;
		private FunctionSecurityProfile _forecastMethodsGlobalCopyChain;
		private FunctionSecurityProfile _forecastMethodsUserCopyChain;
		private FunctionSecurityProfile _forecastMethodsGlobalCopyStore;
		private FunctionSecurityProfile _forecastMethodsUserCopyStore;
        private FunctionSecurityProfile _forecastMethodsGlobalExport;
        private FunctionSecurityProfile _forecastMethodsUserExport;
        // Begin TT#2131-MD - JSmith - Halo Integration
        private FunctionSecurityProfile _forecastMethodsGlobalPlanningExtract;
        private FunctionSecurityProfile _forecastMethodsUserPlanningExtract;
        // End TT#2131-MD - JSmith - Halo Integration
        private FunctionSecurityProfile _forecastMethodsGlobalModifySales;
        private FunctionSecurityProfile _forecastMethodsUserModifySales;
        private FunctionSecurityProfile _forecastMethodsGlobalGlobalUnlock;
        private FunctionSecurityProfile _forecastMethodsUserGlobalUnlock;
        private FunctionSecurityProfile _forecastMethodsGlobalRollup;
        private FunctionSecurityProfile _forecastMethodsUserRollup;
        private FunctionSecurityProfile _forecastWorkflowsGlobal;
		private FunctionSecurityProfile _forecastWorkflowsUser;

        private FunctionSecurityProfile _allocationGlobal;
        private FunctionSecurityProfile _allocationUser;
        private FunctionSecurityProfile _allocationMethodsGlobal;
        private FunctionSecurityProfile _allocationMethodsUser;
		private FunctionSecurityProfile _allocationMethodsGlobalGeneralAllocation;
		private FunctionSecurityProfile _allocationMethodsGlobalAllocationOverride;
		private FunctionSecurityProfile _allocationMethodsGlobalRule;
		private FunctionSecurityProfile _allocationMethodsGlobalVelocity;
        private FunctionSecurityProfile _allocationSizeMethodsGlobal;
		private FunctionSecurityProfile _allocationMethodsGlobalFillSizeHoles;
		private FunctionSecurityProfile _allocationMethodsGlobalBasisSize;
		private FunctionSecurityProfile _allocationMethodsGlobalSizeNeed;
        // Begin TT#155 - JSmith - Size Curve Method
        private FunctionSecurityProfile _allocationMethodsGlobalSizeCurve;
        // End TT#155

        // Begin TT#370 - APicchetti - Build Packs Method
        private FunctionSecurityProfile _allocationMethodsGlobalBuildPacks;
        // End TT#370

		private FunctionSecurityProfile _allocationMethodsGlobalGroupAllocation;	// TT#708-MD - Stodd - Group Allocation Prototype.
        private FunctionSecurityProfile _allocationMethodsGlobalDCCartonRounding;	// TT#1652-MD - stodd - DC Carton Rounding
        private FunctionSecurityProfile _allocationMethodsGlobalCreateMasterHeaders;	// TT#1966-MD - JSmith- DC Fulfillment
        private FunctionSecurityProfile _allocationMethodsGlobalDCFulfillment;	// TT#1966-MD - JSmith- DC Fulfillment

		private FunctionSecurityProfile _allocationMethodsUserGeneralAllocation;
		private FunctionSecurityProfile _allocationMethodsUserAllocationOverride;
		private FunctionSecurityProfile _allocationMethodsUserRule;
		private FunctionSecurityProfile _allocationMethodsUserVelocity;
        private FunctionSecurityProfile _allocationSizeMethodsUser;
		private FunctionSecurityProfile _allocationMethodsUserFillSizeHoles;
		private FunctionSecurityProfile _allocationMethodsUserBasisSize;
		private FunctionSecurityProfile _allocationMethodsUserSizeNeed;
        // Begin TT#155 - JSmith - Size Curve Method
        private FunctionSecurityProfile _allocationMethodsUserSizeCurve;
        // End TT#155

        // Begin TT#370 - APicchetti - Build Packs Method
        private FunctionSecurityProfile _allocationMethodsUserBuildPacks;
        // End TT#370
        private FunctionSecurityProfile _allocationMethodsUserGroupAllocation;	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
        private FunctionSecurityProfile _allocationMethodsUserDCCartonRounding;	// TT#1652-MD - stodd - DC Carton Rounding
        private FunctionSecurityProfile _allocationMethodsUserCreateMasterHeaders;	// TT#1966-MD - JSmith- DC Fulfillment
        private FunctionSecurityProfile _allocationMethodsUserDCFulfillment;	// TT#1966-MD - JSmith- DC Fulfillment

		private FunctionSecurityProfile _allocationWorkflowsGlobal;
		private FunctionSecurityProfile _allocationWorkflowsUser;

		// Begin TT#2 - stodd - assortment
		//private FunctionSecurityProfile _assortmentMethodsGlobalGeneralAssortment;
		//private FunctionSecurityProfile _assortmentMethodsUserGeneralAssortment;
		// End TT#2

        private FunctionSecurityProfile _userFolders;
        private FunctionSecurityProfile _globalFolders;

        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
        private FunctionSecurityProfile _forecastMethodsGlobalGlobalLock;
        private FunctionSecurityProfile _forecastMethodsUserGlobalLock;
        //End TT#43 - MD - DOConnell - Projected Sales Enhancement

        private MIDWorkflowMethodTreeNode _userNode;
        private MIDWorkflowMethodTreeNode _globalNode;
        private MIDWorkflowMethodTreeNode _sharedNode;

        private GetMethods _getMethods;
        private WorkflowBaseData _workflowData;
        private MethodBaseData _methodData;

        private ExplorerAddressBlock _EAB;

        // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
        public Hashtable _htChildren;
        private DataTable _dtMethods;
        private DataTable _dtWorkflows;
        // End TT#1167

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		// Moved to it's own class in SAB
		// event to process methods on assortment headers & placeholders
		//public delegate void ProcessMethodOnAssortmentEventHandler(object source, ProcessMethodOnAssortmentEventArgs e);
		//public event ProcessMethodOnAssortmentEventHandler OnProcessMethodOnAssortmentEvent;
		// END TT#217-MD - stodd - unable to run workflow methods against assortment

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private ArrayList _nodeArrayList;
	    private eProfileType _eProfileType;
        //END TT#110-MD-VStuart - In Use Tool

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// For designer.
		/// </summary>
		public WorkflowMethodTreeView()
		{
		}
        

        public void InitializeTreeView(SessionAddressBlock aSAB, bool aAllowMultiSelect, Form aMDIParentForm, ExplorerAddressBlock aEAB)
        {
            base.InitializeTreeView(aSAB, aAllowMultiSelect, aMDIParentForm);

            _EAB = aEAB;
            LoadSecurity();

            cClosedFolderImage = MIDGraphics.ImageIndex(MIDGraphics.ClosedTreeFolder);
            cOpenFolderImage = MIDGraphics.ImageIndex(MIDGraphics.OpenTreeFolder);
            cClosedShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.ClosedTreeFolder);
            cOpenShortcutFolderImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.OpenTreeFolder);
            cSharedClosedFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.ClosedTreeFolder);
            cSharedOpenFolderImage = MIDGraphics.ImageSharedIndex(MIDGraphics.OpenTreeFolder);
            cUserImage = MIDGraphics.ImageIndex(MIDGraphics.SecUserImage);
            cGlobalImage = MIDGraphics.ImageIndex(MIDGraphics.GlobalImage);
            cGlobalShortcutImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.GlobalImage);
            cUserShortcutImage = MIDGraphics.ImageShortcutIndex(MIDGraphics.SecUserImage);
            cFavoritesImage = MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage);
            cSharedGlobalImage = MIDGraphics.ImageSharedIndex(MIDGraphics.GlobalImage);
            cSharedUserImage = MIDGraphics.ImageSharedIndex(MIDGraphics.SecUserImage);

            _sForecastFolder = MIDText.GetTextOnly((int)eWorkflowType.Forecast);
            _sForecastMethodsFolder = MIDText.GetTextOnly((int)eWorkflowMethodIND.Methods);
            _sForecastOTSPlan = MIDText.GetTextOnly((int)eMethodType.OTSPlan);
            _sForecastOTSBalance = MIDText.GetTextOnly((int)eMethodType.ForecastBalance);
            _sForecastSpread = MIDText.GetTextOnly((int)eMethodType.ForecastSpread);
            _sForecastCopyChain = MIDText.GetTextOnly((int)eMethodType.CopyChainForecast);
            _sForecastCopyStore = MIDText.GetTextOnly((int)eMethodType.CopyStoreForecast);
            _sForecastExport = MIDText.GetTextOnly((int)eMethodType.Export);
            // Begin TT#2131-MD - JSmith - Halo Integration
            _sForecastPlanningExtract = MIDText.GetTextOnly((int)eMethodType.PlanningExtract);
            // End TT#2131-MD - JSmith - Halo Integration
            _sForecastModifySales = MIDText.GetTextOnly((int)eMethodType.ForecastModifySales);
            _sForecastGlobalUnlock = MIDText.GetTextOnly((int)eMethodType.GlobalUnlock);
            //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
            _sForecastGlobalLock = MIDText.GetTextOnly((int)eMethodType.GlobalLock);
            //End TT#43 - MD - DOConnell - Projected Sales Enhancement
            _sForecastRollup = MIDText.GetTextOnly((int)eMethodType.Rollup);
            _sForecastWorkflowsFolder = MIDText.GetTextOnly((int)eWorkflowMethodIND.Workflows);

            _sAllocationFolder = MIDText.GetTextOnly((int)eWorkflowType.Allocation);
            _sAllocationMethodsFolder = MIDText.GetTextOnly((int)eWorkflowMethodIND.Methods);
            _sAllocationGeneralAllocation = MIDText.GetTextOnly((int)eMethodType.GeneralAllocation);
            _sAllocationAllocationOverride = MIDText.GetTextOnly((int)eMethodType.AllocationOverride);
            _sAllocationRule = MIDText.GetTextOnly((int)eMethodType.Rule);
            _sAllocationVelocity = MIDText.GetTextOnly((int)eMethodType.Velocity);
            _sAllocationFillSizeHoles = MIDText.GetTextOnly((int)eMethodType.FillSizeHolesAllocation);
            _sAllocationSizeMethodsFolder = MIDText.GetTextOnly((int)eWorkflowMethodIND.SizeMethods);
            _sAllocationBasisSize = MIDText.GetTextOnly((int)eMethodType.BasisSizeAllocation);
            _sAllocationSizeNeed = MIDText.GetTextOnly((int)eMethodType.SizeNeedAllocation);
            // Begin TT#155 - JSmith - Size Curve Method
            _sAllocationSizeCurve = MIDText.GetTextOnly((int)eMethodType.SizeCurve);
            // End TT#155

            // Begin TT#370 - APicchetti - Build Packs Method
            _sAllocationBuildPacks = MIDText.GetTextOnly((int) eMethodType.BuildPacks);
            // End TT#370
			_sAllocationGroupAllocation = MIDText.GetTextOnly((int)eMethodType.GroupAllocation);	// TT#708-MD - Stodd - Group Allocation Prototype.

            _sAllocationDCCartonRounding = MIDText.GetTextOnly((int)eMethodType.DCCartonRounding);	// TT#1652-MD - stodd - DC Carton Rounding
            _sAllocationCreateMasterHeaders = MIDText.GetTextOnly((int)eMethodType.CreateMasterHeaders);	// TT#1966-MD - JSmith- DC Fulfillment
            _sAllocationDCFulfillment = MIDText.GetTextOnly((int)eMethodType.DCFulfillment);	// TT#1966-MD - JSmith- DC Fulfillment

            _sAllocationWorkflowsFolder = MIDText.GetTextOnly((int)eWorkflowMethodIND.Workflows);

            _getMethods = new GetMethods(SAB);

            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
            _htChildren = new Hashtable();
            // End TT#1167
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        public GetMethods GetMethods
        {
            get
            {
                return _getMethods;
            }
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        public bool isGlobalForecastingAllowed
        {
            get
            {
                if (_forecastMethodsGlobalOTSPlan.AllowView ||
                    _forecastMethodsGlobalOTSBalance.AllowView ||
                    _forecastMethodsGlobalSpread.AllowView ||
                    _forecastMethodsGlobalCopyChain.AllowView ||
                    _forecastMethodsGlobalCopyStore.AllowView ||
                    _forecastMethodsGlobalExport.AllowView ||
                    _forecastMethodsGlobalPlanningExtract.AllowView ||   // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsGlobalModifySales.AllowView ||
                    _forecastMethodsGlobalGlobalUnlock.AllowView ||
                    _forecastMethodsGlobalGlobalLock.AllowView ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsGlobalRollup.AllowView ||
                    _forecastWorkflowsGlobal.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isUserForecastingAllowed
        {
            get
            {
                if (_forecastMethodsUserOTSPlan.AllowView ||
                    _forecastMethodsUserOTSBalance.AllowView ||
                    _forecastMethodsUserSpread.AllowView ||
                    _forecastMethodsUserCopyChain.AllowView ||
                    _forecastMethodsUserCopyStore.AllowView ||
                    _forecastMethodsUserExport.AllowView ||
                    _forecastMethodsUserPlanningExtract.AllowView || // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsUserModifySales.AllowView ||
                    _forecastMethodsUserGlobalUnlock.AllowView ||
                    _forecastMethodsUserGlobalLock.AllowView || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsUserRollup.AllowView ||
                    _forecastWorkflowsUser.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isGlobalAllocationAllowed
        {
            get
            {
                if (_allocationMethodsGlobalGeneralAllocation.AllowView ||
                    _allocationMethodsGlobalAllocationOverride.AllowView ||
                    _allocationMethodsGlobalRule.AllowView ||
                    _allocationMethodsGlobalVelocity.AllowView ||
                    _allocationMethodsGlobalFillSizeHoles.AllowView ||
                    _allocationMethodsGlobalBasisSize.AllowView ||
                    _allocationMethodsGlobalSizeNeed.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowView ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowView ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowView ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                    _allocationMethodsGlobalDCCartonRounding.AllowView ||	// TT#1652-MD - stodd - DC Carton Rounding
                    _allocationMethodsGlobalCreateMasterHeaders.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
                    _allocationMethodsGlobalDCFulfillment.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
                    _allocationWorkflowsGlobal.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isGlobalSizeAllocationAllowed
        {
            get
            {
                if (_allocationMethodsGlobalFillSizeHoles.AllowView ||
                    _allocationMethodsGlobalBasisSize.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                     _allocationMethodsGlobalSizeCurve.AllowView ||
                    // End TT#155
                    _allocationMethodsGlobalSizeNeed.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isUserAllocationAllowed
        {
            get
            {
                if (_allocationMethodsUserGeneralAllocation.AllowView ||
                    _allocationMethodsUserAllocationOverride.AllowView ||
                    _allocationMethodsUserRule.AllowView ||
                    _allocationMethodsUserVelocity.AllowView ||
                    _allocationMethodsUserFillSizeHoles.AllowView ||
                    _allocationMethodsUserBasisSize.AllowView ||
                    _allocationMethodsUserSizeNeed.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowView ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowView ||
                    // End TT#370

                    _allocationWorkflowsUser.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isUserSizeAllocationAllowed
        {
            get
            {
                if (_allocationMethodsUserFillSizeHoles.AllowView ||
                    _allocationMethodsUserBasisSize.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowView ||
                    // End TT#155
                    _allocationMethodsUserSizeNeed.AllowView)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isUserFolderMaintenanceAllowed
        {
            get
            {
                return _userFolders.AllowUpdate;
            }
        }

        public bool isGlobalFolderMaintenanceAllowed
        {
            get
            {
                return _globalFolders.AllowUpdate;
            }
        }

        /// <summary>
        /// Determines if a method type is a corresponding eProfileType for a method
        /// </summary>
        /// <param name="aMethodType">The method type to check</param>
        /// <returns>A flag identifying if the integer is a valid method</returns>
        /// <remarks>The method must be change if a new method is added</remarks>
        private bool isMethodProfileType(int aMethodType)
        {
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//if (aMethodType == (int)eProfileType.Method ||
            //    aMethodType == (int)eProfileType.MethodOTSPlan ||
            if (aMethodType == (int)eProfileType.MethodOTSPlan ||
			//End TT#523 - JScott - Duplicate folder when new folder added
				aMethodType == (int)eProfileType.MethodForecastBalance ||
                aMethodType == (int)eProfileType.MethodModifySales ||
                aMethodType == (int)eProfileType.MethodForecastSpread ||
                aMethodType == (int)eProfileType.MethodCopyChainForecast ||
                aMethodType == (int)eProfileType.MethodCopyStoreForecast ||
                aMethodType == (int)eProfileType.MethodExport ||
                aMethodType == (int)eProfileType.MethodPlanningExtract ||  // TT#2131-MD - JSmith - Halo Integration
                aMethodType == (int)eProfileType.MethodGlobalUnlock ||
                aMethodType == (int)eProfileType.MethodGlobalLock ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                aMethodType == (int)eProfileType.MethodRollup ||
                aMethodType == (int)eProfileType.MethodGeneralAllocation ||
                aMethodType == (int)eProfileType.MethodAllocationOverride ||
                aMethodType == (int)eProfileType.MethodRule ||
                aMethodType == (int)eProfileType.MethodVelocity ||
                aMethodType == (int)eProfileType.MethodSizeNeedAllocation ||
                aMethodType == (int)eProfileType.MethodFillSizeHolesAllocation ||
                aMethodType == (int)eProfileType.MethodBasisSizeAllocation ||
                // Begin TT#155 - JSmith - Size Curve Method
                aMethodType == (int)eProfileType.MethodSizeCurve ||
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                aMethodType == (int) eProfileType.MethodBuildPacks ||
                // End TT#370

                // Begin TT#992 - MD - stodd - null ref when expanding GA folder 
                aMethodType == (int)eProfileType.MethodWarehouseSizeAllocation ||
                aMethodType == (int) eProfileType.MethodGroupAllocation ||	// TT#1652-MD - stodd - DC Carton Rounding
                // End TT#992 - MD - stodd - null ref when expanding GA folder 
                aMethodType == (int) eProfileType.MethodDCCartonRounding ||  // TT#1652-MD - stodd - DC Carton Rounding
                aMethodType == (int) eProfileType.MethodCreateMasterHeaders ||	// TT#1966-MD - JSmith- DC Fulfillment
                aMethodType == (int) eProfileType.MethodDCFulfillment   	// TT#1966-MD - JSmith- DC Fulfillment
                )	
            {
                return true;
            }
            return false;
        }

		public void LoadSecurity()
		{
			try
			{
				_forecastMethodsGlobalOTSPlan = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
				_forecastMethodsUserOTSPlan = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSPlan);
				_forecastMethodsGlobalOTSBalance = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
				_forecastMethodsUserOTSBalance = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSBalance);
				_forecastMethodsGlobalSpread = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSSpread);
				_forecastMethodsUserSpread = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSSpread);
				_forecastMethodsGlobalCopyChain = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalCopyChain);
				_forecastMethodsUserCopyChain = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserCopyChain);
				_forecastMethodsGlobalCopyStore = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalCopyStore);
				_forecastMethodsUserCopyStore = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserCopyStore);
                _forecastMethodsGlobalGlobalUnlock = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalGlobalUnlock);
                _forecastMethodsUserGlobalUnlock = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserGlobalUnlock);
                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                _forecastMethodsGlobalGlobalLock = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalGlobalLock);
                _forecastMethodsUserGlobalLock = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserGlobalLock);
                //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                _forecastMethodsGlobalRollup = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalRollup);
                _forecastMethodsUserRollup = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserRollup);
                _forecastMethodsGlobalExport = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalExport);
                _forecastMethodsUserExport = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserExport);
                // Begin TT#2131-MD - JSmith - Halo Integration
                _forecastMethodsGlobalPlanningExtract = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalPlanningExtract);
                _forecastMethodsUserPlanningExtract = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserPlanningExtract);
                // End TT#2131-MD - JSmith - Halo Integration
                _forecastMethodsGlobalModifySales = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSModifySales);
                _forecastMethodsUserModifySales = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSModifySales);

                _forecastMethodsGlobal = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobal);
                _forecastMethodsUser = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUser);
                _forecastWorkflowsGlobal = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastWorkflowsGlobal);
                _forecastWorkflowsUser = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastWorkflowsUser);
                
				_allocationMethodsGlobalGeneralAllocation = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalGeneralAllocation);
				_allocationMethodsGlobalAllocationOverride = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalAllocationOverride);
				_allocationMethodsGlobalRule = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalRule);
				_allocationMethodsGlobalVelocity = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalVelocity);
				_allocationMethodsGlobalFillSizeHoles = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
				_allocationMethodsGlobalBasisSize = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalBasisSize);
				_allocationMethodsGlobalSizeNeed = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalSizeNeed);
                // Begin TT#155 - JSmith - Size Curve Method
                _allocationMethodsGlobalSizeCurve = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalSizeCurve);
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                _allocationMethodsGlobalBuildPacks = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalBuildPacks);
                // End TT#370
				// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
				_allocationMethodsGlobalGroupAllocation = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalGroupAllocation);	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
				// END TT#708-MD - Stodd - Group Allocation Prototype.
                _allocationMethodsGlobalDCCartonRounding = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalDCCartonRounding);	 // TT#1652-MD - stodd - DC Carton Rounding
                _allocationMethodsGlobalCreateMasterHeaders = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalCreateMasterHeaders);	 // TT#1966-MD - JSmith- DC Fulfillment
                _allocationMethodsGlobalDCFulfillment = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobalDCFulfillment);	 // TT#1966-MD - JSmith- DC Fulfillment

				_allocationMethodsUserGeneralAllocation = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserGeneralAllocation);
				_allocationMethodsUserAllocationOverride = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserAllocationOverride);
				_allocationMethodsUserRule = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserRule);
				_allocationMethodsUserVelocity = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserVelocity);
				_allocationMethodsUserFillSizeHoles = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserFillSizeHoles);
				_allocationMethodsUserBasisSize = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserBasisSize);
				_allocationMethodsUserSizeNeed = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserSizeNeed);
                // Begin TT#155 - JSmith - Size Curve Method
                _allocationMethodsUserSizeCurve = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserSizeCurve);
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                _allocationMethodsUserBuildPacks = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserBuildPacks);
                // End TT#370
                _allocationMethodsUserGroupAllocation = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserGroupAllocation);	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                _allocationMethodsUserDCCartonRounding = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserDCCartonRounding);	// TT#1652-MD - stodd - DC Carton Rounding
                _allocationMethodsUserCreateMasterHeaders = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserCreateMasterHeaders);	 // TT#1966-MD - JSmith- DC Fulfillment
                _allocationMethodsUserDCFulfillment = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUserDCFulfillment);	 // TT#1966-MD - JSmith- DC Fulfillment
                _allocationMethodsGlobal = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsGlobal);
                _allocationMethodsUser = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethodsUser);
                _allocationWorkflowsGlobal = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationWorkflowsGlobal);
				_allocationWorkflowsUser = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationWorkflowsUser);
				// Begin TT#2 - stodd - assortment
				//_assortmentMethodsGlobalGeneralAssortment = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AssortmentMethodsGlobalGeneralAssortment);
				//_assortmentMethodsUserGeneralAssortment = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AssortmentMethodsUserGeneralAssortment);
				// End TT#2
                _userFolders = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ExplorersWorkflowMethodFoldersUser);
                _globalFolders = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ExplorersWorkflowMethodFoldersGlobal);

                // disable security for components not installed
				if (!SAB.ClientServerSession.GlobalOptions.AppConfig.PlanningInstalled)
				{
					_forecastWorkflowsGlobal.SetAccessDenied();
					_forecastWorkflowsUser.SetAccessDenied();
					_forecastMethodsGlobalSpread.SetAccessDenied();
					_forecastMethodsUserSpread.SetAccessDenied();
					_forecastMethodsGlobalOTSBalance.SetAccessDenied();
					_forecastMethodsUserOTSBalance.SetAccessDenied();
					_forecastMethodsGlobalCopyChain.SetAccessDenied();
					_forecastMethodsUserCopyChain.SetAccessDenied();
					_forecastMethodsGlobalCopyStore.SetAccessDenied();
					_forecastMethodsUserCopyStore.SetAccessDenied();
                    _forecastMethodsGlobalGlobalUnlock.SetAccessDenied();
                    _forecastMethodsGlobalGlobalLock.SetAccessDenied(); //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsUserGlobalUnlock.SetAccessDenied();
                    _forecastMethodsUserGlobalLock.SetAccessDenied();   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsGlobalRollup.SetAccessDenied();
                    _forecastMethodsUserRollup.SetAccessDenied();
                    _forecastMethodsGlobalExport.SetAccessDenied();
                    _forecastMethodsUserExport.SetAccessDenied();
                    _forecastMethodsGlobalPlanningExtract.SetAccessDenied(); // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsUserPlanningExtract.SetAccessDenied(); // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsGlobalModifySales.SetAccessDenied();
                    _forecastMethodsUserModifySales.SetAccessDenied();

				}

                if (_forecastWorkflowsUser.AccessDenied &&
                    _forecastMethodsUserSpread.AccessDenied &&
                    _forecastMethodsUserOTSBalance.AccessDenied &&
                    _forecastMethodsUserCopyChain.AccessDenied &&
                    _forecastMethodsUserCopyStore.AccessDenied &&
                    _forecastMethodsUserExport.AccessDenied &&
                    _forecastMethodsUserPlanningExtract.AccessDenied && // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsUserModifySales.AccessDenied &&
                    _forecastMethodsUserGlobalUnlock.AccessDenied &&
                    _forecastMethodsUserGlobalLock.AccessDenied &&  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsUserRollup.AccessDenied)
                {
                    _forecastMethodsUser.SetAccessDenied();
                }
                if (_forecastWorkflowsGlobal.AccessDenied &&
                    _forecastMethodsGlobalSpread.AccessDenied &&
                    _forecastMethodsGlobalOTSBalance.AccessDenied &&
                    _forecastMethodsGlobalCopyChain.AccessDenied &&
                    _forecastMethodsGlobalCopyStore.AccessDenied &&
                    _forecastMethodsGlobalExport.AccessDenied &&
                    _forecastMethodsGlobalPlanningExtract.AccessDenied && // TT#2131-MD - JSmith - Halo Integration
                    _forecastMethodsGlobalModifySales.AccessDenied &&
                    _forecastMethodsGlobalGlobalUnlock.AccessDenied &&
                    _forecastMethodsGlobalGlobalLock.AccessDenied &&    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    _forecastMethodsGlobalRollup.AccessDenied)
                {
                    _forecastMethodsGlobal.SetAccessDenied();
                }

				if (!SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
                    _allocationMethodsGlobalFillSizeHoles.SetAccessDenied();
					_allocationMethodsGlobalBasisSize.SetAccessDenied();
                    _allocationMethodsGlobalSizeNeed.SetAccessDenied();
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.SetAccessDenied();
                    // End TT#155

                    _allocationMethodsUserFillSizeHoles.SetAccessDenied();
                    _allocationMethodsUserBasisSize.SetAccessDenied();
                    _allocationMethodsUserSizeNeed.SetAccessDenied();
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.SetAccessDenied();
                    // End TT#155
				}

                // Begin TT#2131-MD - JSmith - Halo Integration
                if (!SAB.ROExtractEnabled)
                {
                    _forecastMethodsGlobalPlanningExtract.SetAccessDenied();
                    _forecastMethodsUserPlanningExtract.SetAccessDenied();
                }
                // End TT#2131-MD - JSmith - Halo Integration

				// Begin TT#2 - stodd - assortment
				//if (!SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				//{
				//    _assortmentMethodsGlobalGeneralAssortment.SetAccessDenied();
				//    _assortmentMethodsUserGeneralAssortment.SetAccessDenied();
				//}
				// End TT#2 - stodd - assortment

               _forecastGlobal = new FunctionSecurityProfile(Include.NoRID);
               if (_forecastMethodsGlobalOTSPlan.AllowView ||
                   _forecastMethodsGlobalOTSBalance.AllowView ||
                   _forecastMethodsGlobalSpread.AllowView ||
                   _forecastMethodsGlobalCopyChain.AllowView ||
                   _forecastMethodsGlobalCopyStore.AllowView ||
                   _forecastMethodsGlobalExport.AllowView ||
                   _forecastMethodsGlobalPlanningExtract.AllowView || // TT#2131-MD - JSmith - Halo Integration
                   _forecastMethodsGlobalModifySales.AllowView ||
                   _forecastMethodsGlobalGlobalUnlock.AllowView ||
                   _forecastMethodsGlobalGlobalLock.AllowView ||    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                   _forecastMethodsGlobalRollup.AllowView ||
                   _forecastWorkflowsGlobal.AllowView)
               {
                   _forecastGlobal.SetAllowView();
               }
               if (_forecastMethodsGlobalOTSPlan.AllowUpdate ||
                  _forecastMethodsGlobalOTSBalance.AllowUpdate ||
                  _forecastMethodsGlobalSpread.AllowUpdate ||
                  _forecastMethodsGlobalCopyChain.AllowUpdate ||
                  _forecastMethodsGlobalCopyStore.AllowUpdate ||
                  _forecastMethodsGlobalExport.AllowUpdate ||
                  _forecastMethodsGlobalPlanningExtract.AllowUpdate || // TT#2131-MD - JSmith - Halo Integration
                  _forecastMethodsGlobalModifySales.AllowUpdate ||
                  _forecastMethodsGlobalGlobalUnlock.AllowUpdate ||
                  _forecastMethodsGlobalGlobalLock.AllowUpdate ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                  _forecastMethodsGlobalRollup.AllowUpdate ||
                  _forecastWorkflowsGlobal.AllowUpdate)
               {
                   _forecastGlobal.SetAllowUpdate();
               }
               if (_forecastMethodsGlobalOTSPlan.AllowDelete ||
                 _forecastMethodsGlobalOTSBalance.AllowDelete ||
                 _forecastMethodsGlobalSpread.AllowDelete ||
                 _forecastMethodsGlobalCopyChain.AllowDelete ||
                 _forecastMethodsGlobalCopyStore.AllowDelete ||
                 _forecastMethodsGlobalExport.AllowDelete ||
                 _forecastMethodsGlobalPlanningExtract.AllowDelete || // TT#2131-MD - JSmith - Halo Integration
                 _forecastMethodsGlobalModifySales.AllowDelete ||
                 _forecastMethodsGlobalGlobalUnlock.AllowDelete ||
                 _forecastMethodsGlobalGlobalLock.AllowDelete ||    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                 _forecastMethodsGlobalRollup.AllowDelete ||
                 _forecastWorkflowsGlobal.AllowDelete)
               {
                   _forecastGlobal.SetAllowDelete();
               }
               if (_forecastMethodsGlobalOTSPlan.AllowExecute ||
                 _forecastMethodsGlobalOTSBalance.AllowExecute ||
                 _forecastMethodsGlobalSpread.AllowExecute ||
                 _forecastMethodsGlobalCopyChain.AllowExecute ||
                 _forecastMethodsGlobalCopyStore.AllowExecute ||
                 _forecastMethodsGlobalExport.AllowExecute ||
                 _forecastMethodsGlobalPlanningExtract.AllowExecute || // TT#2131-MD - JSmith - Halo Integration
                 _forecastMethodsGlobalModifySales.AllowExecute ||
                 _forecastMethodsGlobalGlobalUnlock.AllowExecute ||
                 _forecastMethodsGlobalGlobalLock.AllowExecute ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                 _forecastMethodsGlobalRollup.AllowExecute ||
                 _forecastWorkflowsGlobal.AllowExecute)
               {
                   _forecastGlobal.SetAllowExecute();
               }
               if (_forecastMethodsGlobalOTSPlan.AllowMove ||
                 _forecastMethodsGlobalOTSBalance.AllowMove ||
                 _forecastMethodsGlobalSpread.AllowMove ||
                 _forecastMethodsGlobalCopyChain.AllowMove ||
                 _forecastMethodsGlobalCopyStore.AllowMove ||
                 _forecastMethodsGlobalExport.AllowMove ||
                 _forecastMethodsGlobalPlanningExtract.AllowMove || // TT#2131-MD - JSmith - Halo Integration
                 _forecastMethodsGlobalModifySales.AllowMove ||
                 _forecastMethodsGlobalGlobalUnlock.AllowMove ||
                 _forecastMethodsGlobalGlobalLock.AllowMove ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                 _forecastMethodsGlobalRollup.AllowMove ||
                 _forecastWorkflowsGlobal.AllowMove)
               {
                   _forecastGlobal.SetAllowMove();
               }

                _forecastUser = new FunctionSecurityProfile(Include.NoRID);
                if (_forecastMethodsUserOTSPlan.AllowView ||
                   _forecastMethodsUserOTSBalance.AllowView ||
                   _forecastMethodsUserSpread.AllowView ||
                   _forecastMethodsUserCopyChain.AllowView ||
                   _forecastMethodsUserCopyStore.AllowView ||
                   _forecastMethodsUserExport.AllowView ||
                   _forecastMethodsUserPlanningExtract.AllowView || // TT#2131-MD - JSmith - Halo Integration
                   _forecastMethodsUserModifySales.AllowView ||
                   _forecastMethodsUserGlobalUnlock.AllowView ||
                   _forecastMethodsUserGlobalLock.AllowView ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                   _forecastMethodsUserRollup.AllowView ||
                   _forecastWorkflowsUser.AllowView)
                {
                    _forecastUser.SetAllowView();
                }
                if (_forecastMethodsUserOTSPlan.AllowUpdate ||
                   _forecastMethodsUserOTSBalance.AllowUpdate ||
                   _forecastMethodsUserSpread.AllowUpdate ||
                   _forecastMethodsUserCopyChain.AllowUpdate ||
                   _forecastMethodsUserCopyStore.AllowUpdate ||
                   _forecastMethodsUserExport.AllowUpdate ||
                   _forecastMethodsUserPlanningExtract.AllowUpdate || // TT#2131-MD - JSmith - Halo Integration
                   _forecastMethodsUserModifySales.AllowUpdate ||
                   _forecastMethodsUserGlobalUnlock.AllowUpdate ||
                   _forecastMethodsUserGlobalLock.AllowUpdate ||    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                   _forecastMethodsUserRollup.AllowUpdate ||
                   _forecastWorkflowsUser.AllowUpdate)
                {
                    _forecastUser.SetAllowUpdate();
                }
                if (_forecastMethodsUserOTSPlan.AllowDelete ||
                  _forecastMethodsUserOTSBalance.AllowDelete ||
                  _forecastMethodsUserSpread.AllowDelete ||
                  _forecastMethodsUserCopyChain.AllowDelete ||
                  _forecastMethodsUserCopyStore.AllowDelete ||
                  _forecastMethodsUserExport.AllowDelete ||
                  _forecastMethodsUserPlanningExtract.AllowDelete || // TT#2131-MD - JSmith - Halo Integration
                  _forecastMethodsUserModifySales.AllowDelete ||
                  _forecastMethodsUserGlobalUnlock.AllowDelete ||
                  _forecastMethodsUserGlobalLock.AllowDelete || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                  _forecastMethodsUserRollup.AllowDelete ||
                  _forecastWorkflowsUser.AllowDelete)
                {
                    _forecastUser.SetAllowDelete();
                }
                if (_forecastMethodsUserOTSPlan.AllowExecute ||
                  _forecastMethodsUserOTSBalance.AllowExecute ||
                  _forecastMethodsUserSpread.AllowExecute ||
                  _forecastMethodsUserCopyChain.AllowExecute ||
                  _forecastMethodsUserCopyStore.AllowExecute ||
                  _forecastMethodsUserExport.AllowExecute ||
                  _forecastMethodsUserPlanningExtract.AllowExecute || // TT#2131-MD - JSmith - Halo Integration
                  _forecastMethodsUserModifySales.AllowExecute ||
                  _forecastMethodsUserGlobalUnlock.AllowExecute ||
                  _forecastMethodsUserGlobalLock.AllowExecute ||    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                  _forecastMethodsUserRollup.AllowExecute ||
                  _forecastWorkflowsUser.AllowExecute)
                {
                    _forecastGlobal.SetAllowExecute();
                }
                if (_forecastMethodsUserOTSPlan.AllowMove ||
                  _forecastMethodsUserOTSBalance.AllowMove ||
                  _forecastMethodsUserSpread.AllowMove ||
                  _forecastMethodsUserCopyChain.AllowMove ||
                  _forecastMethodsUserCopyStore.AllowMove ||
                  _forecastMethodsUserExport.AllowMove ||
                  _forecastMethodsUserPlanningExtract.AllowMove || // TT#2131-MD - JSmith - Halo Integration
                  _forecastMethodsUserModifySales.AllowMove ||
                  _forecastMethodsUserGlobalUnlock.AllowMove ||
                  _forecastMethodsUserGlobalLock.AllowMove ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                  _forecastMethodsUserRollup.AllowMove ||
                  _forecastWorkflowsUser.AllowMove)
                {
                    _forecastUser.SetAllowMove();
                }

                _allocationGlobal = new FunctionSecurityProfile(Include.NoRID);
                if (_allocationMethodsGlobalGeneralAllocation.AllowView ||
		              _allocationMethodsGlobalAllocationOverride.AllowView ||
		              _allocationMethodsGlobalRule.AllowView ||
		              _allocationMethodsGlobalVelocity.AllowView ||
		              _allocationMethodsGlobalFillSizeHoles.AllowView ||
		              _allocationMethodsGlobalBasisSize.AllowView ||
		              _allocationMethodsGlobalSizeNeed.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                     _allocationMethodsGlobalSizeCurve.AllowView ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowView ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowView ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                    _allocationMethodsGlobalDCCartonRounding.AllowView ||	// TT#1652-MD - stodd - DC Carton Rounding
                    _allocationMethodsGlobalCreateMasterHeaders.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
                    _allocationMethodsGlobalDCFulfillment.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
		              _allocationWorkflowsGlobal.AllowView)
                  {
                      _allocationGlobal.SetAllowView();
                  }
                if (_allocationMethodsGlobalGeneralAllocation.AllowUpdate ||
                      _allocationMethodsGlobalAllocationOverride.AllowUpdate ||
                      _allocationMethodsGlobalRule.AllowUpdate ||
                      _allocationMethodsGlobalVelocity.AllowUpdate ||
                      _allocationMethodsGlobalFillSizeHoles.AllowUpdate ||
                      _allocationMethodsGlobalBasisSize.AllowUpdate ||
                      _allocationMethodsGlobalSizeNeed.AllowUpdate ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowUpdate ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowUpdate ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowUpdate ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                     _allocationMethodsGlobalDCCartonRounding.AllowUpdate ||	// TT#1652-MD - stodd - DC Carton Rounding
                     _allocationMethodsGlobalCreateMasterHeaders.AllowUpdate ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsGlobalDCFulfillment.AllowUpdate ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsGlobal.AllowUpdate)
                {
                    _allocationGlobal.SetAllowUpdate();
                }
                if (_allocationMethodsGlobalGeneralAllocation.AllowDelete ||
                      _allocationMethodsGlobalAllocationOverride.AllowDelete ||
                      _allocationMethodsGlobalRule.AllowDelete ||
                      _allocationMethodsGlobalVelocity.AllowDelete ||
                      _allocationMethodsGlobalFillSizeHoles.AllowDelete ||
                      _allocationMethodsGlobalBasisSize.AllowDelete ||
                      _allocationMethodsGlobalSizeNeed.AllowDelete ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowDelete ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowDelete ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowDelete ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                    _allocationMethodsGlobalDCCartonRounding.AllowDelete ||	// TT#1652-MD - stodd - DC Carton Rounding
                    _allocationMethodsGlobalCreateMasterHeaders.AllowDelete ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsGlobalDCFulfillment.AllowDelete ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsGlobal.AllowDelete)
                {
                    _allocationGlobal.SetAllowDelete();
                }
                if (_allocationMethodsGlobalGeneralAllocation.AllowExecute ||
                      _allocationMethodsGlobalAllocationOverride.AllowExecute ||
                      _allocationMethodsGlobalRule.AllowExecute ||
                      _allocationMethodsGlobalVelocity.AllowExecute ||
                      _allocationMethodsGlobalFillSizeHoles.AllowExecute ||
                      _allocationMethodsGlobalBasisSize.AllowExecute ||
                      _allocationMethodsGlobalSizeNeed.AllowExecute ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowExecute ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowExecute ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowExecute ||		// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                     _allocationMethodsGlobalDCCartonRounding.AllowExecute ||	// TT#1652-MD - stodd - DC Carton Rounding
                     _allocationMethodsGlobalCreateMasterHeaders.AllowExecute ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsGlobalDCFulfillment.AllowExecute ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsGlobal.AllowExecute)
                {
                    _allocationGlobal.SetAllowExecute();
                }
                if (_allocationMethodsGlobalGeneralAllocation.AllowMove ||
                      _allocationMethodsGlobalAllocationOverride.AllowMove ||
                      _allocationMethodsGlobalRule.AllowMove ||
                      _allocationMethodsGlobalVelocity.AllowMove ||
                      _allocationMethodsGlobalFillSizeHoles.AllowMove ||
                      _allocationMethodsGlobalBasisSize.AllowMove ||
                      _allocationMethodsGlobalSizeNeed.AllowMove ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowMove ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsGlobalBuildPacks.AllowMove ||
                    // End TT#370
                    _allocationMethodsGlobalGroupAllocation.AllowMove ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                     _allocationMethodsGlobalDCCartonRounding.AllowMove ||	// TT#1652-MD - stodd - DC Carton Rounding
                     _allocationMethodsGlobalCreateMasterHeaders.AllowMove ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsGlobalDCFulfillment.AllowMove ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsGlobal.AllowMove)
                {
                    _allocationGlobal.SetAllowMove();
                }

                _allocationSizeMethodsGlobal = new FunctionSecurityProfile(Include.NoRID);
                if (_allocationMethodsGlobalFillSizeHoles.AllowView ||
                      _allocationMethodsGlobalBasisSize.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowView ||
                    // End TT#155

                      _allocationMethodsGlobalSizeNeed.AllowView)
                {
                    _allocationSizeMethodsGlobal.SetAllowView();
                }
                if (_allocationMethodsGlobalFillSizeHoles.AllowUpdate ||
                      _allocationMethodsGlobalBasisSize.AllowUpdate ||
                    // Begin TT#155 - JSmith - Size Curve Method
                     _allocationMethodsGlobalSizeCurve.AllowUpdate ||
                    // End TT#155

                    _allocationMethodsGlobalSizeNeed.AllowUpdate)
                {
                    _allocationSizeMethodsGlobal.SetAllowUpdate();
                }
                if (_allocationMethodsGlobalFillSizeHoles.AllowDelete ||
                      _allocationMethodsGlobalBasisSize.AllowDelete ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowDelete ||
                    // End TT#155
                      _allocationMethodsGlobalSizeNeed.AllowDelete)
                {
                    _allocationSizeMethodsGlobal.SetAllowDelete();
                }
                if (_allocationMethodsGlobalFillSizeHoles.AllowExecute ||
                      _allocationMethodsGlobalBasisSize.AllowExecute ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowExecute ||
                    // End TT#155
                      _allocationMethodsGlobalSizeNeed.AllowExecute)
                {
                    _allocationSizeMethodsGlobal.SetAllowExecute();
                }
                if (_allocationMethodsGlobalFillSizeHoles.AllowMove ||
                      _allocationMethodsGlobalBasisSize.AllowMove ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsGlobalSizeCurve.AllowMove ||
                    // End TT#155
                      _allocationMethodsGlobalSizeNeed.AllowMove)
                {
                    _allocationSizeMethodsGlobal.SetAllowMove();
                }

                _allocationUser = new FunctionSecurityProfile(Include.NoRID);
                if (_allocationMethodsUserGeneralAllocation.AllowView ||
                      _allocationMethodsUserAllocationOverride.AllowView ||
                      _allocationMethodsUserRule.AllowView ||
                      _allocationMethodsUserVelocity.AllowView ||
                      _allocationMethodsUserFillSizeHoles.AllowView ||
                      _allocationMethodsUserBasisSize.AllowView ||
                      _allocationMethodsUserSizeNeed.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowView ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowView ||
                    // End TT#370
                    _allocationMethodsUserGroupAllocation.AllowView ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                     _allocationMethodsUserDCCartonRounding.AllowView ||	// TT#1652-MD - stodd - DC Carton Rounding
                     _allocationMethodsUserCreateMasterHeaders.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsUserDCFulfillment.AllowView ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsUser.AllowView)
                {
                    _allocationUser.SetAllowView();
                }
                if (_allocationMethodsUserGeneralAllocation.AllowUpdate ||
                      _allocationMethodsUserAllocationOverride.AllowUpdate ||
                      _allocationMethodsUserRule.AllowUpdate ||
                      _allocationMethodsUserVelocity.AllowUpdate ||
                      _allocationMethodsUserFillSizeHoles.AllowUpdate ||
                      _allocationMethodsUserBasisSize.AllowUpdate ||
                      _allocationMethodsUserSizeNeed.AllowUpdate ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowUpdate ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowUpdate ||
                    // End TT#370
                     _allocationMethodsUserGroupAllocation.AllowUpdate ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                      _allocationMethodsUserDCCartonRounding.AllowUpdate ||		// TT#1652-MD - stodd - DC Carton Rounding
                      _allocationMethodsUserCreateMasterHeaders.AllowUpdate ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsUserDCFulfillment.AllowUpdate ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsUser.AllowUpdate)
                {
                    _allocationUser.SetAllowUpdate();
                }
                if (_allocationMethodsUserGeneralAllocation.AllowDelete ||
                      _allocationMethodsUserAllocationOverride.AllowDelete ||
                      _allocationMethodsUserRule.AllowDelete ||
                      _allocationMethodsUserVelocity.AllowDelete ||
                      _allocationMethodsUserFillSizeHoles.AllowDelete ||
                      _allocationMethodsUserBasisSize.AllowDelete ||
                      _allocationMethodsUserSizeNeed.AllowDelete ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowDelete ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowDelete ||
                    // End TT#370
                    _allocationMethodsUserGroupAllocation.AllowDelete ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                    _allocationMethodsUserDCCartonRounding.AllowDelete ||	// TT#1652-MD - stodd - DC Carton Rounding
                    _allocationMethodsUserCreateMasterHeaders.AllowDelete ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsUserDCFulfillment.AllowDelete ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsUser.AllowDelete)
                {
                    _allocationUser.SetAllowDelete();
                }
                if (_allocationMethodsUserGeneralAllocation.AllowExecute ||
                      _allocationMethodsUserAllocationOverride.AllowExecute ||
                      _allocationMethodsUserRule.AllowExecute ||
                      _allocationMethodsUserVelocity.AllowExecute ||
                      _allocationMethodsUserFillSizeHoles.AllowExecute ||
                      _allocationMethodsUserBasisSize.AllowExecute ||
                      _allocationMethodsUserSizeNeed.AllowExecute ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowExecute ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowExecute ||
                    // End TT#370
                    _allocationMethodsUserGroupAllocation.AllowExecute ||	// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                    _allocationMethodsUserDCCartonRounding.AllowExecute ||	// TT#1652-MD - stodd - DC Carton Rounding
                    _allocationMethodsUserCreateMasterHeaders.AllowExecute ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsUserDCFulfillment.AllowExecute ||	// TT#1966-MD - JSmith- DC Fulfillment
                      _allocationWorkflowsUser.AllowExecute)
                {
                    _allocationUser.SetAllowExecute();
                }
                if (_allocationMethodsUserGeneralAllocation.AllowMove ||
                      _allocationMethodsUserAllocationOverride.AllowMove ||
                      _allocationMethodsUserRule.AllowMove ||
                      _allocationMethodsUserVelocity.AllowMove ||
                      _allocationMethodsUserFillSizeHoles.AllowMove ||
                      _allocationMethodsUserBasisSize.AllowMove ||
                      _allocationMethodsUserSizeNeed.AllowMove ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowMove ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    _allocationMethodsUserBuildPacks.AllowMove ||
                    // End TT#370
                     _allocationMethodsUserGroupAllocation.AllowMove ||		// TT#1136-md - stodd - cannot create a "user" group allocation method - 
                     _allocationMethodsUserDCCartonRounding.AllowMove ||	// TT#1652-MD - stodd - DC Carton Rounding
                     _allocationMethodsUserCreateMasterHeaders.AllowMove ||	// TT#1966-MD - JSmith- DC Fulfillment
                     _allocationMethodsUserDCFulfillment.AllowMove ||	// TT#1966-MD - JSmith- DC Fulfillment

                      _allocationWorkflowsUser.AllowMove)
                {
                    _allocationUser.SetAllowMove();
                }

                _allocationSizeMethodsUser = new FunctionSecurityProfile(Include.NoRID);
                if (_allocationMethodsUserFillSizeHoles.AllowView ||
                      _allocationMethodsUserBasisSize.AllowView ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowView ||
                    // End TT#155
                      _allocationMethodsUserSizeNeed.AllowView)
                {
                    _allocationSizeMethodsUser.SetAllowView();
                }
                if (_allocationMethodsUserFillSizeHoles.AllowUpdate ||
                      _allocationMethodsUserBasisSize.AllowUpdate ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowUpdate ||
                    // End TT#155
                      _allocationMethodsUserSizeNeed.AllowUpdate)
                {
                    _allocationSizeMethodsUser.SetAllowUpdate();
                }
                if (_allocationMethodsUserFillSizeHoles.AllowDelete ||
                      _allocationMethodsUserBasisSize.AllowDelete ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowDelete ||
                    // End TT#155
                      _allocationMethodsUserSizeNeed.AllowDelete)
                {
                    _allocationSizeMethodsUser.SetAllowDelete();
                }
                if (_allocationMethodsUserFillSizeHoles.AllowExecute ||
                      _allocationMethodsUserBasisSize.AllowExecute ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowExecute ||
                    // End TT#155
                      _allocationMethodsUserSizeNeed.AllowExecute)
                {
                    _allocationSizeMethodsUser.SetAllowExecute();
                }
                if (_allocationMethodsUserFillSizeHoles.AllowMove ||
                      _allocationMethodsUserBasisSize.AllowMove ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    _allocationMethodsUserSizeCurve.AllowMove ||
                    // End TT#155
                      _allocationMethodsUserSizeNeed.AllowMove)
                {
                    _allocationSizeMethodsUser.SetAllowMove();
                }
			}
			catch
			{
				throw;
			}
		}

        public FunctionSecurityProfile UserFolderSecurity
        {
            get
            {
                return _userFolders;
            }
        }

        public FunctionSecurityProfile GlobalFolderSecurity
        {
            get
            {
                return _globalFolders;
            }
        }

        /// <summary>
        /// Gets the workflow data layer
        /// </summary>
        public WorkflowBaseData DlWorkflowData
        {
            get
            {
                if (_workflowData == null)
                {
                    _workflowData = new WorkflowBaseData();
                }
                return _workflowData;
            }
        }

        /// <summary>
        /// Gets the method data layer
        /// </summary>
        public MethodBaseData DlMethodData
        {
            get
            {
                if (_methodData == null)
                {
                    _methodData = new MethodBaseData();
                }
                return _methodData;
            }
        }

        /// <summary>
		/// Virtual method that loads the nodes for the MIDTreeView
		/// </summary>

        override public void LoadNodes()
        {
            FolderProfile folderProf;
            // Begin TT#63 - JSmith - Shared folder showing when nothing shared
            //DataTable dtSharedFolders;
            DataTable dtSharedMethods, dtSharedWorkflows;
            // End TT#63
            FunctionSecurityProfile functionSecurity;
            ArrayList userList;

            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
            MethodBaseData methodsData;
            WorkflowBaseData workflowsData;
            // ENd TT#1167

            try
            {
                Nodes.Clear();

                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                _htChildren.Clear();
                methodsData = new MethodBaseData();
                _dtMethods = methodsData.GetMethods(SAB.ClientServerSession.UserRID);
                workflowsData = new WorkflowBaseData();
                _dtWorkflows = workflowsData.GetWorkflows(SAB.ClientServerSession.UserRID);
                // End TT#1167

                // Begin TT#106 - JSmith - No Favorites folder if user methods and workflows are denied.
                //if (isUserForecastingAllowed ||
                //    isUserAllocationAllowed)
                //{
                // End TT#106
                    //----------------------
                    // Build Faviorites node
                    //----------------------

                    folderProf = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.WorkflowMethodMainFavoritesFolder, "My Favorites");

                    // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                    //FavoritesNode = new MIDWorkflowMethodTreeNode(
                    //    SAB,
                    //    eTreeNodeType.MainFavoriteFolderNode,
                    //    folderProf,
                    //    folderProf.Name,
                    //    Include.NoRID,
                    //    folderProf.UserRID,
                    //    _userFolders,
                    //    MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage),
                    //    MIDGraphics.ImageIndex(MIDGraphics.FavoritesImage),
                    //    cFavoritesImage,
                    //    cFavoritesImage,
                    //    folderProf.OwnerUserRID);
                    FavoritesNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.MainFavoriteFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        FavoritesSecGrp.FolderSecurityProfile,
                        cFavoritesImage,
                        cFavoritesImage,
                        cFavoritesImage,
                        cFavoritesImage,
                        folderProf.OwnerUserRID);
                    // End TT#42

                    FolderNodeHash[folderProf.Key] = FavoritesNode;

                    // check for children
                    if (Folder_Children_Exists(folderProf.UserRID, FavoritesNode.Profile.Key))
                    {
                        FavoritesNode.Nodes.Add(new MIDWorkflowMethodTreeNode());
                        FavoritesNode.ChildrenLoaded = false;
                        FavoritesNode.HasChildren = true;
                        FavoritesNode.DisplayChildren = true;
                    }
                    else
                    {
                        FavoritesNode.ChildrenLoaded = true;
                    }

                    Nodes.Add(FavoritesNode);

                if (isUserForecastingAllowed ||
                    isUserAllocationAllowed)
                {
                    //----------------
                    // Build User node
                    //----------------

                    // Begin TT#176 - JSmith - New database has errors
                    //folderProf = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.WorkflowMethodMainUserFolder, "My Workflow/Methods");
                    bool newFolder = false;
                    folderProf = Folder_Get(SAB.ClientServerSession.UserRID, eProfileType.WorkflowMethodMainUserFolder, "My Workflow/Methods", ref newFolder);
                    if (newFolder)
                    {
                        CreateNewOTSForecastGroup(SAB.ClientServerSession.UserRID, folderProf.Key);
                        CreateNewAllocationGroup(SAB.ClientServerSession.UserRID, folderProf.Key);
                    }
                    // End TT#176

                    // Begin TT#135 - JSmith - Unhandled Exception when Security is Deny all for the Explorers
                    functionSecurity = new FunctionSecurityProfile((int)eProfileType.WorkflowMethodMainUserFolder);
                    functionSecurity.SetAllowUpdate();
                    // End TT#135

                    _userNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        // Begin TT#135 - JSmith - Unhandled Exception when Security is Deny all for the Explorers
                        //_userFolders,
                        functionSecurity,
                        // End TT#135
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID);

                    // check for children
                    if (Folder_Children_Exists(folderProf.UserRID, folderProf.Key))
                    {
                        _userNode.Nodes.Add(new MIDWorkflowMethodTreeNode());
                        _userNode.ChildrenLoaded = false;
                        _userNode.HasChildren = true;
                        _userNode.DisplayChildren = true;
                    }
                    else
                    {
                        _userNode.ChildrenLoaded = true;
                    }

                    FolderNodeHash[folderProf.Key] = _userNode;

                    Nodes.Add(_userNode);
                }

                // Begin TT#106 - JSmith - No Favorites folder if user methods and workflows are denied.
                if (isGlobalForecastingAllowed ||
                    isGlobalAllocationAllowed)
                {
                // End TT#106
                    //------------------
                    // Build Global node
                    //------------------

                    folderProf = Folder_Get(Include.GlobalUserRID, eProfileType.WorkflowMethodMainGlobalFolder, "Global Workflow/Methods");

                    // Begin TT#135 - JSmith - Unhandled Exception when Security is Deny all for the Explorers
                    functionSecurity = new FunctionSecurityProfile((int)eProfileType.WorkflowMethodMainGlobalFolder);
                    functionSecurity.SetAllowView();
                    // End TT#135

                    _globalNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.MainSourceFolderNode,
                        folderProf,
                        folderProf.Name,
                        Include.NoRID,
                        folderProf.UserRID,
                        // Begin TT#135 - JSmith - Unhandled Exception when Security is Deny all for the Explorers
                        //_globalFolders,
                        functionSecurity,
                        // End TT#135
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        folderProf.OwnerUserRID);

                    // check for children
                    if (Folder_Children_Exists(folderProf.UserRID, folderProf.Key))
                    {
                        _globalNode.Nodes.Add(new MIDWorkflowMethodTreeNode());
                        _globalNode.ChildrenLoaded = false;
                        _globalNode.HasChildren = true;
                        _globalNode.DisplayChildren = true;
                    }
                    else
                    {
                        _globalNode.ChildrenLoaded = true;
                    }

                    FolderNodeHash[folderProf.Key] = _globalNode;

                    Nodes.Add(_globalNode);

                    //build shared node
                    _sharedNode = null;
                    userList = new ArrayList();
                    userList.Add(SAB.ClientServerSession.UserRID);
                    // Begin TT#63 - JSmith - Shared folder showing when nothing shared
                    //dtSharedFolders = DlFolder.Folder_Read(userList, eProfileType.WorkflowMethodMainUserFolder, false, true);
                    //if (dtSharedFolders.Rows.Count > 0)
                    dtSharedMethods = DlMethodData.GetSharedMethods(SAB.ClientServerSession.UserRID, false);
                    dtSharedWorkflows = DlWorkflowData.GetSharedWorkflows(SAB.ClientServerSession.UserRID);
                    if (dtSharedMethods.Rows.Count > 0 ||
                        dtSharedWorkflows.Rows.Count > 0)
                    // End TT#63
                    {
                        folderProf = new FolderProfile(Convert.ToInt32(eProfileType.WorkflowMethodMainSharedFolder), SAB.ClientServerSession.UserRID, eProfileType.WorkflowMethodMainSharedFolder, MIDText.GetTextOnly(eMIDTextCode.msg_SharedFolderName), SAB.ClientServerSession.UserRID);
                        functionSecurity = new FunctionSecurityProfile(Include.NoRID);
                        functionSecurity.SetReadOnly();
                        _sharedNode = new MIDWorkflowMethodTreeNode(
                           SAB,
                           eTreeNodeType.MainSourceFolderNode,
                           folderProf,
                           folderProf.Name,
                           Include.NoRID,
                           folderProf.UserRID,
                           _userFolders,
                           cSharedClosedFolderImage,
                           cSharedClosedFolderImage,
                           cSharedOpenFolderImage,
                           cSharedOpenFolderImage,
                           folderProf.OwnerUserRID);

                        // add dummy node for expanding
                        _sharedNode.Nodes.Add(new MIDWorkflowMethodTreeNode());
                        _sharedNode.ChildrenLoaded = false;
                        _sharedNode.HasChildren = true;
                        _sharedNode.DisplayChildren = true;
                        
                        FolderNodeHash[folderProf.Key] = _sharedNode;

                        Nodes.Add(_sharedNode);
                    }

                    // expand all base nodes
                    // Begin TT#106 - JSmith - No Favorites folder if user methods and workflows are denied.
                    //FavoritesNode.Expand();
                    //_userNode.Expand();
                    //_globalNode.Expand();
                    if (FavoritesNode != null)
                    {
                        FavoritesNode.Expand();
                    }
                    if (_userNode != null)
                    {
                        _userNode.Expand();
                    }
                    if (_globalNode != null)
                    {
                        _globalNode.Expand();
                    }
                    // End TT#106
                    if (_sharedNode != null)
                    {
                        _sharedNode.Expand();
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public void ReloadCache(int userRID, int groupKey)
        {
            DataTable children;

            try
            {
                _htChildren.Remove(groupKey);
                children = DlFolder.Folder_Children_Read(userRID, groupKey);

                _htChildren.Add(groupKey, children);

                MethodBaseData methodsData = new MethodBaseData();
                _dtMethods = methodsData.GetMethods(SAB.ClientServerSession.UserRID);
                WorkflowBaseData workflowsData = new WorkflowBaseData();
                _dtWorkflows = workflowsData.GetWorkflows(SAB.ClientServerSession.UserRID);
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#176 - JSmith - New database has errors
        private void CreateNewOTSForecastGroup(int aUserRID, int aFolderRID)
        {
            string newNodeName;
            FolderProfile newFolderProf;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();

                newNodeName = MIDText.GetTextOnly((int)eWorkflowType.Forecast);

                dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserRID, eProfileType.WorkflowMethodOTSForcastFolder, newNodeName, aUserRID);
                    newFolderProf.Key = dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    dlFolder.Folder_Item_Insert(aFolderRID, newFolderProf.Key, eProfileType.WorkflowMethodOTSForcastFolder);

                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }

        private void CreateNewAllocationGroup(int aUserRID, int aFolderRID)
        {
            string newNodeName;
            FolderProfile newFolderProf;
            FolderDataLayer dlFolder;

            try
            {
                dlFolder = new FolderDataLayer();

                newNodeName = MIDText.GetTextOnly((int)eWorkflowType.Allocation);

                dlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserRID, eProfileType.WorkflowMethodAllocationFolder, newNodeName, aUserRID);
                    newFolderProf.Key = dlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    dlFolder.Folder_Item_Insert(aFolderRID, newFolderProf.Key, eProfileType.WorkflowMethodAllocationFolder);

                    dlFolder.CommitData();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    dlFolder.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#176

        override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDropNode, ClipboardListBase aSelectedNodes)
		{
			try
			{
                if (aSelectedNodes.ClipboardDataType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.WorkflowMethodAllocationFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.Workflow ||
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					//aSelectedNodes.ClipboardDataType == eProfileType.Method ||
					//End TT#523 - JScott - Duplicate folder when new folder added
					aSelectedNodes.ClipboardDataType == eProfileType.MethodOTSPlan ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodForecastBalance ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodModifySales ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodForecastSpread ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCopyChainForecast ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCopyStoreForecast ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodExport ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodPlanningExtract ||  // Begin TT#2131-MD - JSmith - Halo Integration
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGlobalUnlock ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGlobalLock ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodRollup ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGeneralAllocation ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodAllocationOverride ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodRule ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodVelocity ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodSizeNeedAllocation ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodSizeCurve ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodBuildPacks ||
                    // End TT#370
                     aSelectedNodes.ClipboardDataType == eProfileType.MethodGroupAllocation ||	// TT#995 - MD - stodd - GA method - cannot drag-drop a method in to a Folder
                      aSelectedNodes.ClipboardDataType == eProfileType.MethodDCCartonRounding ||	// TT#1652-MD - stodd - DC Carton Rounding
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCreateMasterHeaders ||	// TT#1966-MD - JSmith- DC Fulfillment
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodDCFulfillment ||	// TT#1966-MD - JSmith- DC Fulfillment
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodFillSizeHolesAllocation ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodBasisSizeAllocation ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodWarehouseSizeAllocation ||
                    aSelectedNodes.ClipboardDataType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodOTSPlanSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodForecastBalanceSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodModifySalesSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodForecastSpreadSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCopyChainForecastSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCopyStoreForecastSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodExportSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodPlanningExtractSubFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGlobalUnlockSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGlobalLockSubFolder || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodRollupSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGeneralAllocationSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodAllocationOverrideSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodRuleSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodVelocitySubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodSizeMethodSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodFillSizeHolesSubFolder ||
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodBasisSizeSubFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodSizeCurveSubFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodBuildPacksSubFolder ||
                    // End TT#370

                    aSelectedNodes.ClipboardDataType == eProfileType.MethodGroupAllocationSubFolder ||		// TT#995 - MD - stodd - GA method - cannot drag-drop a method in to a Folder
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodDCCartonRoundingSubFolder ||	// TT#1652-MD - stodd - DC Carton Rounding
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodCreateMasterHeadersSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodDCFulfillmentSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    aSelectedNodes.ClipboardDataType == eProfileType.MethodSizeNeedSubFolder) 
				{
                    return GetTreeNode(((TreeNodeClipboardList)aSelectedNodes).ClipboardProfile).isDropAllowed(aDropAction, aDropNode);
				}

				return false;
			}
			catch
			{
				throw;
			}
		}

        override public bool isAllowedDataType(eProfileType aClipboardDataType)
        {
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//return aClipboardDataType == eProfileType.Method ||
            //    aClipboardDataType == eProfileType.Workflow;
            return aClipboardDataType == eProfileType.Workflow ||
                    aClipboardDataType == eProfileType.MethodOTSPlan ||
                    aClipboardDataType == eProfileType.MethodForecastBalance ||
                    aClipboardDataType == eProfileType.MethodModifySales ||
                    aClipboardDataType == eProfileType.MethodForecastSpread ||
                    aClipboardDataType == eProfileType.MethodCopyChainForecast ||
                    aClipboardDataType == eProfileType.MethodCopyStoreForecast ||
                    aClipboardDataType == eProfileType.MethodExport ||
                    aClipboardDataType == eProfileType.MethodPlanningExtract ||  // TT#2131-MD - JSmith - Halo Integration
                    aClipboardDataType == eProfileType.MethodGlobalUnlock ||
                    aClipboardDataType == eProfileType.MethodGlobalLock ||    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    aClipboardDataType == eProfileType.MethodRollup ||
                    aClipboardDataType == eProfileType.MethodGeneralAllocation ||
                    aClipboardDataType == eProfileType.MethodAllocationOverride ||
                    aClipboardDataType == eProfileType.MethodRule ||
                    aClipboardDataType == eProfileType.MethodVelocity ||
                    aClipboardDataType == eProfileType.MethodSizeNeedAllocation ||
                    aClipboardDataType == eProfileType.MethodSizeCurve ||
                    aClipboardDataType == eProfileType.MethodFillSizeHolesAllocation ||
                    aClipboardDataType == eProfileType.MethodBasisSizeAllocation ||
                    aClipboardDataType == eProfileType.MethodWarehouseSizeAllocation;
			//End TT#523 - JScott - Duplicate folder when new folder added
		}



        //public void CreateShortcutChildren(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        //{
        //    MIDWorkflowMethodTreeNode newNode = null;

        //    try
        //    {
        //        foreach (MIDWorkflowMethodTreeNode node in aFromNode.Nodes)
        //        {
        //            if (node.isMethod)
        //            {
        //                newNode = BuildMethodNode(node.Profile.Key, (eMethodType)node.MethodType, node.FunctionSecurityProfile, node.UserId, node.OwnerUserRID, aToNode.Profile.Key, true);
        //            }
        //            else if (node.isWorkflow)
        //            {
        //                newNode = BuildWorkflowNode(node.Profile.Key, node.WorkflowType, node.FunctionSecurityProfile, node.UserId, node.OwnerUserRID, aToNode.Profile.Key, true);
        //            }
        //            else if (node.NodeProfileType == eProfileType.WorkflowMethodSubFolder)
        //            {
        //                newNode = BuildFolderNode(node.Profile.Key, aToNode.Profile.Key, node.UserId, node.Profile.ProfileType, node.Text, node.FunctionSecurityProfile, node.OwnerUserRID, eTreeNodeType.ChildShortcutNode, true);
        //            }

        //            aToNode.Nodes.Add(newNode);

        //            CreateShortcutChildren(node, newNode);
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //    finally
        //    {
        //        Cursor.Current = Cursors.Default;
        //    }
        //}

        public void RebuildShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
        {
            try
            {
                foreach (MIDWorkflowMethodTreeNode node in aStartNode.Nodes)
                {
                    if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                    {
                        node.RefreshShortcutNode(aChangedNode);

                        if (!node.isWorkflowMethod)
                        {
							if (node.isObjectShortcut || node.isFolderShortcut)
                            {
								//Begin Track #6201 - JScott - Store Count removed from attr sets
								//node.Text = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
                                node.InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
								//End Track #6201 - JScott - Store Count removed from attr sets
								DeleteChildNodes(node);
                                //CreateShortcutChildren(aChangedNode, node);
                            }
                            else if (node.isChildShortcut)
                            {
								//Begin Track #6201 - JScott - Store Count removed from attr sets
								//node.Text = ((FolderProfile)aChangedNode.Profile).Name;
								node.InternalText = ((FolderProfile)aChangedNode.Profile).Name;
								//End Track #6201 - JScott - Store Count removed from attr sets
								DeleteChildNodes(node);
                                //CreateShortcutChildren(aChangedNode, node);
                            }
                        }
                    }
					else if (node.isWorkflowMethod || node.isFolderShortcut)
                    {
                        RebuildShortcuts(node, aChangedNode);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void DeleteShortcuts(MIDTreeNode aStartNode, MIDTreeNode aDeleteNode)
        {
            object[] deleteList;

            try
            {
                deleteList = new object[aStartNode.Nodes.Count];
                aStartNode.Nodes.CopyTo(deleteList, 0);

                foreach (MIDWorkflowMethodTreeNode node in deleteList)
                {
                    if (node.Profile.Key == aDeleteNode.Profile.Key && node.Profile.ProfileType == aDeleteNode.Profile.ProfileType &&
                        node.isShortcut)
                    {
                        DeleteChildNodes(node);
                        node.Remove();
                    }
                    else if (node.NodeProfileType == eProfileType.WorkflowMethodSubFolder ||
                        node.isMethodFolder ||
                        node.isWorkflowFolder ||
						node.isFolderShortcut ||
                        (node.isChildShortcut && node.Profile.ProfileType == eProfileType.WorkflowMethodSubFolder))
                    {
                        DeleteShortcuts(node, aDeleteNode);
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#1167 - Login Performance - Opening the application after Login
        //private bool AddFoldersToFolder(MIDWorkflowMethodTreeNode aFolder, eProfileType aProfileType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity, eWorkflowType aWorkflowType)
        internal bool AddFoldersToFolder(MIDWorkflowMethodTreeNode aFolder, eProfileType aProfileType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity, eWorkflowType aWorkflowType, bool aBuildNodes)
        // End TT#1167
        {
            int itemId;
            int userId;
            int ownerUserId;
            bool isShortcut;
            DataRow[] folderChildrenList;

            try
            {
                folderChildrenList = aChildren.Select("USER_RID = " + aFolder.UserId + " AND CHILD_ITEM_TYPE = " + (int)aProfileType);

                // Begin TT#1167 - Login Performance - Opening the application after Login
                if (aBuildNodes)
                {
                // End TT#1167
                    foreach (DataRow child in folderChildrenList)
                    {
                        itemId = Convert.ToInt32(child["CHILD_ITEM_RID"], CultureInfo.CurrentUICulture);
                        if (aFolder.isShared)
                        {
                            userId = SAB.ClientServerSession.UserRID;
                        }
                        else
                        {
                            userId = Convert.ToInt32(child["USER_RID"], CultureInfo.CurrentUICulture);
                        }
                        ownerUserId = Convert.ToInt32(child["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
                        isShortcut = Include.ConvertCharToBool(Convert.ToChar(child["SHORTCUT_IND"], CultureInfo.CurrentUICulture));
                        aFolder.Nodes.Add(BuildFolderNode(itemId, aFolder, userId, aProfileType, DlFolder.Folder_GetName(itemId), aFunctionSecurity, ownerUserId, eTreeNodeType.SubFolderNode, isShortcut));
                    }

                    aFolder.DisplayChildren = true;
                    aFolder.ChildrenLoaded = true;
                    SortChildNodes((MIDWorkflowMethodTreeNode)aFolder);
                // Begin TT#1167 - Login Performance - Opening the application after Login
                }

                return folderChildrenList.Length > 0;
                // End TT#1167

            }
            
            catch
            {
                throw;
            }
        }

        // Begin TT#1167 - Login Performance - Opening the application after Login
        //private bool AddMethodsToFolder(MIDWorkflowMethodTreeNode aFolder, eMethodType aMethodType, eProfileType aProfileType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity)
        internal bool AddMethodsToFolder(MIDWorkflowMethodTreeNode aFolder, eMethodType aMethodType, eProfileType aProfileType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity, bool aBuildNodes)
        // End TT#1167
        {
            int itemId;
			int userId;
            int ownerUserId;
            DataRow[] folderChildrenList;
            bool isShortcut;

            try
            {
                folderChildrenList = aChildren.Select("USER_RID = " + aFolder.UserId + " AND CHILD_ITEM_TYPE = " + (int)aProfileType);

                // Begin TT#1167 - Login Performance - Opening the application after Login
                if (aBuildNodes)
                {
                // End TT#1167
                    foreach (DataRow child in folderChildrenList)
                    {
                        itemId = Convert.ToInt32(child["CHILD_ITEM_RID"], CultureInfo.CurrentUICulture);
                        if (aFolder.isShared)
                        {
                            userId = SAB.ClientServerSession.UserRID;
                        }
                        else
                        {
                            userId = Convert.ToInt32(child["USER_RID"], CultureInfo.CurrentUICulture);
                        }
                        ownerUserId = Convert.ToInt32(child["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
                        isShortcut = Include.ConvertCharToBool(Convert.ToChar(child["SHORTCUT_IND"], CultureInfo.CurrentUICulture));
                        aFolder.Nodes.Add(BuildMethodNode(itemId, aFolder, aMethodType, aFunctionSecurity, userId, ownerUserId, aFolder.Profile.Key, isShortcut));
                    }

                    aFolder.DisplayChildren = true;
                    aFolder.ChildrenLoaded = true;
                    SortChildNodes((MIDWorkflowMethodTreeNode)aFolder);
                // Begin TT#1167 - Login Performance - Opening the application after Login
                }

                return folderChildrenList.Length > 0;
                // End TT#1167
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#1167 - Login Performance - Opening the application after Login
        //private bool AddWorkflowsToFolder(MIDWorkflowMethodTreeNode aFolder, eWorkflowType aWorkflowType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity)
        internal bool AddWorkflowsToFolder(MIDWorkflowMethodTreeNode aFolder, eWorkflowType aWorkflowType, DataTable aChildren, FunctionSecurityProfile aFunctionSecurity, bool aBuildNodes)
        // End TT#1167
        {
            int itemId;
            int userId;
            int ownerUserId;
            DataRow[] folderChildrenList;
            bool isShortcut;

            try
            {
                folderChildrenList = aChildren.Select("USER_RID = " + aFolder.UserId + " AND CHILD_ITEM_TYPE = " + (int)eProfileType.Workflow);

                // Begin TT#1167 - Login Performance - Opening the application after Login
                if (aBuildNodes)
                {
                // End TT#1167
                    foreach (DataRow child in folderChildrenList)
                    {
                        itemId = Convert.ToInt32(child["CHILD_ITEM_RID"], CultureInfo.CurrentUICulture);
                        if (aFolder.isShared)
                        {
                            userId = SAB.ClientServerSession.UserRID;
                        }
                        else
                        {
                            userId = Convert.ToInt32(child["USER_RID"], CultureInfo.CurrentUICulture);
                        }
                        ownerUserId = Convert.ToInt32(child["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
                        isShortcut = Include.ConvertCharToBool(Convert.ToChar(child["SHORTCUT_IND"], CultureInfo.CurrentUICulture));
                        aFolder.Nodes.Add(BuildWorkflowNode(itemId, aFolder, aWorkflowType, aFunctionSecurity, userId, ownerUserId, aFolder.Profile.Key, isShortcut));
                    }

                    aFolder.DisplayChildren = true;
                    aFolder.ChildrenLoaded = true;
                    SortChildNodes((MIDWorkflowMethodTreeNode)aFolder);
                // Begin TT#1167 - Login Performance - Opening the application after Login
                }

                return folderChildrenList.Length > 0;
                // End TT#1167
            }
            catch
            {
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildOTSForecastGroup(MIDWorkflowMethodTreeNode aNode, int aGroupKey, int aUserRID, int aOwnerUserRID, bool aIsShortcut)
        {
            DataTable children;
            //int groupKey;

            try
            {
                if ((aUserRID == Include.GlobalUserRID && isGlobalForecastingAllowed) ||
                    (aUserRID != Include.GlobalUserRID && isUserForecastingAllowed))
                {
                    //children = DlFolder.Folder_Children_Read(aNode.UserId, aNode.Profile.Key, (int)eProfileType.WorkflowMethodOTSForcastFolder);
                    //groupKey = Convert.ToInt32(children.Rows[0]["CHILD_ITEM_RID"]);
                    //children = DlFolder.Folder_Children_Read(aOwnerUserRID, aGroupKey);
                    children = DlFolder.Folder_Children_Read(aUserRID, aGroupKey);
                    // Begin TT#1167 - Login Performance - Opening the application after Login
                    _htChildren.Add(aGroupKey, children);
                    // End TT#1167
                    return BuildOTSForecastGroup(aNode, aGroupKey, aUserRID, aOwnerUserRID, children, aIsShortcut);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode BuildOTSForecastGroup(MIDWorkflowMethodTreeNode aNode, int aGroupKey, int aUserRID, int aOwnerUserRID, DataTable aChildren, bool aIsShortcut)
        {
            MIDWorkflowMethodTreeNode groupNode;

            groupNode = null;

            groupNode = BuildFolderNode(aGroupKey, aNode, aUserRID, eProfileType.WorkflowMethodOTSForcastFolder, DlFolder.Folder_GetName(aGroupKey), GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
            groupNode.Sequence = 0;

            return BuildOTSForecastGroupChildren(groupNode, aGroupKey, aUserRID, aOwnerUserRID, aChildren, aIsShortcut);
        }

        public MIDWorkflowMethodTreeNode BuildOTSForecastGroupChildren(MIDWorkflowMethodTreeNode groupNode, int aGroupKey, int aUserRID, int aOwnerUserRID, DataTable aChildren, bool aIsShortcut)
        {
            MIDWorkflowMethodTreeNode methodsNode;
            MIDWorkflowMethodTreeNode node;
            int sequence = -1;

            try
            {
                if (GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Forecast).AllowView)
                {
                    node = BuildFolderNode((int)eProfileType.WorkflowMethodOTSForcastWorkflowsFolder, groupNode, aUserRID, eProfileType.WorkflowMethodOTSForcastWorkflowsFolder, _sForecastWorkflowsFolder, GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastWorkflowsFolder, eWorkflowType.Forecast), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                    //node.WorkflowType = eWorkflowType.Forecast;
                    node.Sequence = 0;
                    groupNode.Nodes.Add(node);
					// Begin TT#1167 - Login Performance - Opening the application after Login
                    //AddFoldersToFolder(node, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, eWorkflowType.Forecast), eWorkflowType.Forecast);
                    //AddWorkflowsToFolder(node, eWorkflowType.Forecast, aChildren, GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Forecast));
                    if (AddFoldersToFolder(node, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, eWorkflowType.Forecast), eWorkflowType.Forecast, false) ||
                        AddWorkflowsToFolder(node, eWorkflowType.Forecast, aChildren, GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Forecast), false))
                    {
                        node.Nodes.Add(BuildPlaceHolderNode());
                        node.ChildrenLoaded = false;
                        node.HasChildren = true;
                        node.DisplayChildren = true;
                    }
					// End TT#1167
                }
                if (GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastFolder, eWorkflowType.None).AllowView)
                {
                    methodsNode = BuildFolderNode((int)eProfileType.WorkflowMethodOTSForcastMethodsFolder, groupNode, aUserRID, eProfileType.WorkflowMethodOTSForcastMethodsFolder, _sForecastMethodsFolder, GetSecurity(aUserRID, eProfileType.WorkflowMethodOTSForcastMethodsFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                    methodsNode.Sequence = 1;

                    if (GetSecurity(aUserRID, eProfileType.MethodOTSPlan, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodOTSPlanFolder, methodsNode, aUserRID, eProfileType.MethodOTSPlanFolder, _sForecastOTSPlan, GetSecurity(aUserRID, eProfileType.MethodOTSPlanFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodOTSPlanSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodOTSPlanSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.OTSPlan, eProfileType.MethodOTSPlan, aChildren, GetSecurity(aUserRID, eProfileType.MethodOTSPlan, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodOTSPlanSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodOTSPlanSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.OTSPlan, eProfileType.MethodOTSPlan, aChildren, GetSecurity(aUserRID, eProfileType.MethodOTSPlan, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodForecastBalance, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodForecastBalanceFolder, methodsNode, aUserRID, eProfileType.MethodForecastBalanceFolder, _sForecastOTSBalance, GetSecurity(aUserRID, eProfileType.MethodForecastBalanceFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodForecastBalanceSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastBalanceSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.ForecastBalance, eProfileType.MethodForecastBalance, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastBalance, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodForecastBalanceSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastBalanceSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.ForecastBalance, eProfileType.MethodForecastBalance, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastBalance, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodModifySales, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodModifySalesFolder, methodsNode, aUserRID, eProfileType.MethodModifySalesFolder, _sForecastModifySales, GetSecurity(aUserRID, eProfileType.MethodModifySalesFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodModifySalesSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodModifySalesSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.ForecastModifySales, eProfileType.MethodModifySales, aChildren, GetSecurity(aUserRID, eProfileType.MethodModifySales, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodModifySalesSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodModifySalesSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.ForecastModifySales, eProfileType.MethodModifySales, aChildren, GetSecurity(aUserRID, eProfileType.MethodModifySales, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodForecastSpread, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodForecastSpreadFolder, methodsNode, aUserRID, eProfileType.MethodForecastSpreadFolder, _sForecastSpread, GetSecurity(aUserRID, eProfileType.MethodForecastSpreadFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodForecastSpreadSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastSpreadSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.ForecastSpread, eProfileType.MethodForecastSpread, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastSpread, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodForecastSpreadSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastSpreadSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.ForecastSpread, eProfileType.MethodForecastSpread, aChildren, GetSecurity(aUserRID, eProfileType.MethodForecastSpread, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodCopyChainForecast, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodCopyChainForecastFolder, methodsNode, aUserRID, eProfileType.MethodCopyChainForecastFolder, _sForecastCopyChain, GetSecurity(aUserRID, eProfileType.MethodCopyChainForecastFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodCopyChainForecastSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyChainForecastSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.CopyChainForecast, eProfileType.MethodCopyChainForecast, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyChainForecast, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodCopyChainForecastSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyChainForecastSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.CopyChainForecast, eProfileType.MethodCopyChainForecast, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyChainForecast, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecast, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodCopyStoreForecastFolder, methodsNode, aUserRID, eProfileType.MethodCopyStoreForecastFolder, _sForecastCopyStore, GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecastFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodCopyStoreForecastSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecastSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.CopyStoreForecast, eProfileType.MethodCopyStoreForecast, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecast, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodCopyStoreForecastSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecastSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.CopyStoreForecast, eProfileType.MethodCopyStoreForecast, aChildren, GetSecurity(aUserRID, eProfileType.MethodCopyStoreForecast, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodExport, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodExportFolder, methodsNode, aUserRID, eProfileType.MethodExportFolder, _sForecastExport, GetSecurity(aUserRID, eProfileType.MethodExportFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodExportSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodExportSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.Export, eProfileType.MethodExport, aChildren, GetSecurity(aUserRID, eProfileType.MethodExport, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodExportSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodExportSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.Export, eProfileType.MethodExport, aChildren, GetSecurity(aUserRID, eProfileType.MethodExport, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodGlobalUnlock, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodGlobalUnlockFolder, methodsNode, aUserRID, eProfileType.MethodGlobalUnlockFolder, _sForecastGlobalUnlock, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlockFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodGlobalUnlockSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlockSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.GlobalUnlock, eProfileType.MethodGlobalUnlock, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlock, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodGlobalUnlockSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlockSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.GlobalUnlock, eProfileType.MethodGlobalUnlock, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlock, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }

                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    if (GetSecurity(aUserRID, eProfileType.MethodGlobalLock, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodGlobalLockFolder, methodsNode, aUserRID, eProfileType.MethodGlobalLockFolder, _sForecastGlobalLock, GetSecurity(aUserRID, eProfileType.MethodGlobalLockFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        // Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodGlobalUnlockSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlockSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.GlobalUnlock, eProfileType.MethodGlobalUnlock, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalUnlock, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodGlobalLockSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalLockSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.GlobalLock, eProfileType.MethodGlobalLock, aChildren, GetSecurity(aUserRID, eProfileType.MethodGlobalLock, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                        // End TT#1167
                    }
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement

                    if (GetSecurity(aUserRID, eProfileType.MethodRollup, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodRollupFolder, methodsNode, aUserRID, eProfileType.MethodRollupFolder, _sForecastRollup, GetSecurity(aUserRID, eProfileType.MethodRollupFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodRollupSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodRollupSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.Rollup, eProfileType.MethodRollup, aChildren, GetSecurity(aUserRID, eProfileType.MethodRollup, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodRollupSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodRollupSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.Rollup, eProfileType.MethodRollup, aChildren, GetSecurity(aUserRID, eProfileType.MethodRollup, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    // Begin TT#2131-MD - JSmith - Halo Integration
                    if (GetSecurity(aUserRID, eProfileType.MethodPlanningExtract, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodPlanningExtractFolder, methodsNode, aUserRID, eProfileType.MethodPlanningExtractFolder, _sForecastPlanningExtract, GetSecurity(aUserRID, eProfileType.MethodPlanningExtractFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        if (AddFoldersToFolder(node, eProfileType.MethodPlanningExtractSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodPlanningExtractSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.PlanningExtract, eProfileType.MethodPlanningExtract, aChildren, GetSecurity(aUserRID, eProfileType.MethodPlanningExtract, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                    }
                    // End TT#2131-MD - JSmith - Halo Integration
                    groupNode.Nodes.Add(methodsNode);
                }

                return groupNode;
            }
            catch
            {
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildAllocationGroup(MIDWorkflowMethodTreeNode aNode, int aGroupKey, int aUserRID, int aOwnerUserRID, bool aIsShortcut)
        {
            DataTable children;
            //int groupKey;

            try
            {
                if ((aUserRID == Include.GlobalUserRID && isGlobalAllocationAllowed) ||
                    (aUserRID != Include.GlobalUserRID && isUserAllocationAllowed))
                {
                    //children = DlFolder.Folder_Children_Read(aNode.UserId, aNode.Profile.Key, (int)eProfileType.WorkflowMethodAllocationFolder);
                    //groupKey = Convert.ToInt32(children.Rows[0]["CHILD_ITEM_RID"]);
                    //children = DlFolder.Folder_Children_Read(aOwnerUserRID, aGroupKey);
                    children = DlFolder.Folder_Children_Read(aUserRID, aGroupKey);
                    // Begin TT#1167 - Login Performance - Opening the application after Login
                    _htChildren.Add(aGroupKey, children);
                    // End TT#1167
                    return BuildAllocationGroup(aNode, aGroupKey, aUserRID, aOwnerUserRID, children, aIsShortcut);
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode BuildAllocationGroup(MIDWorkflowMethodTreeNode aNode, int aGroupKey, int aUserRID, int aOwnerUserRID, DataTable aChildren, bool aIsShortcut)
        {
            MIDWorkflowMethodTreeNode groupNode;
            groupNode = null;

            groupNode = BuildFolderNode(aGroupKey, aNode, aUserRID, eProfileType.WorkflowMethodAllocationFolder, DlFolder.Folder_GetName(aGroupKey), GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
            groupNode.Sequence = 1;
            return BuildAllocationGroupChildren(groupNode, aGroupKey, aUserRID, aOwnerUserRID, aChildren, aIsShortcut);
        }

        public MIDWorkflowMethodTreeNode BuildAllocationGroupChildren(MIDWorkflowMethodTreeNode groupNode, int aGroupKey, int aUserRID, int aOwnerUserRID, DataTable aChildren, bool aIsShortcut)
        {
            MIDWorkflowMethodTreeNode sizeNode;
            MIDWorkflowMethodTreeNode methodsNode;
            MIDWorkflowMethodTreeNode node;
            int sequence = -1;

            try
            {
                if (GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Allocation).AllowView)
                {
                    node = BuildFolderNode((int)eProfileType.WorkflowMethodAllocationWorkflowsFolder, groupNode, aUserRID, eProfileType.WorkflowMethodAllocationWorkflowsFolder, _sAllocationWorkflowsFolder, GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationWorkflowsFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                    node.Sequence = 0;
                    //node.WorkflowType = eWorkflowType.Allocation;
                    groupNode.Nodes.Add(node);
					// Begin TT#1167 - Login Performance - Opening the application after Login
                    //AddFoldersToFolder(node, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, eWorkflowType.Allocation), eWorkflowType.Allocation);
                    //AddWorkflowsToFolder(node, eWorkflowType.Allocation, aChildren, GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Allocation));
                    if (AddFoldersToFolder(node, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, eWorkflowType.Allocation), eWorkflowType.Allocation, false) ||
                        AddWorkflowsToFolder(node, eWorkflowType.Allocation, aChildren, GetSecurity(aUserRID, eProfileType.Workflow, eWorkflowType.Allocation), false))
                    {
                        node.Nodes.Add(BuildPlaceHolderNode());
                        node.ChildrenLoaded = false;
                        node.HasChildren = true;
                        node.DisplayChildren = true;
                    }
					// End TT#1167
                }
                if (GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationMethodsFolder, eWorkflowType.None).AllowView)
                {
                    methodsNode = BuildFolderNode((int)eProfileType.WorkflowMethodAllocationMethodsFolder, groupNode, aUserRID, eProfileType.WorkflowMethodAllocationMethodsFolder, _sAllocationMethodsFolder, GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationMethodsFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                    methodsNode.Sequence = 1;

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    if (GetSecurity(aUserRID, eProfileType.MethodCreateMasterHeaders, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodCreateMasterHeadersFolder, methodsNode, aUserRID, eProfileType.MethodCreateMasterHeadersFolder, _sAllocationCreateMasterHeaders, GetSecurity(aUserRID, eProfileType.MethodCreateMasterHeadersFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        if (AddFoldersToFolder(node, eProfileType.MethodCreateMasterHeadersSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodCreateMasterHeadersSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.CreateMasterHeaders, eProfileType.MethodCreateMasterHeaders, aChildren, GetSecurity(aUserRID, eProfileType.MethodCreateMasterHeaders, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                    }
                    // End TT#1966-MD - JSmith- DC Fulfillment

                    // BEGIN TT#724-MD - Stodd - unhide Group Allocation
                    // BEGIN TT#488-MD - Stodd - Group Allocation
                    //==========================================================================
                    // BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
                    if (GetSecurity(aUserRID, eProfileType.MethodGroupAllocation, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodGroupAllocationFolder, methodsNode, aUserRID, eProfileType.MethodGroupAllocationFolder, _sAllocationGroupAllocation, GetSecurity(aUserRID, eProfileType.MethodGroupAllocationFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        // Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodBuildPacksSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacksSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.BuildPacks, eProfileType.MethodBuildPacks, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacks, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodGroupAllocationSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGroupAllocationSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.GroupAllocation, eProfileType.MethodGroupAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodGroupAllocation, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                        // End TT#1167
                    }
                    // END TT#708-MD - Stodd - Group Allocation Prototype.
                    // END TT#488-MD - Stodd - Group Allocation
                    // BEGIN TT#724-MD - Stodd - unhide Group Allocation

                    if (GetSecurity(aUserRID, eProfileType.MethodGeneralAllocation, eWorkflowType.None).AllowView)
                    {
                        // Begin TT#66 - JSmith - User has full security for the General Allocation Method but cannot create one
                        //node = BuildFolderNode((int)eProfileType.MethodGeneralAllocationFolder, methodsNode, aUserRID, eProfileType.MethodGeneralAllocationFolder, _sAllocationGeneralAllocation, GetSecurity(aUserRID, eProfileType.MethodExportFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node = BuildFolderNode((int)eProfileType.MethodGeneralAllocationFolder, methodsNode, aUserRID, eProfileType.MethodGeneralAllocationFolder, _sAllocationGeneralAllocation, GetSecurity(aUserRID, eProfileType.MethodGeneralAllocationFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        // End TT#66
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodGeneralAllocationSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGeneralAllocationSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.GeneralAllocation, eProfileType.MethodGeneralAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodGeneralAllocation, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodGeneralAllocationSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodGeneralAllocationSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.GeneralAllocation, eProfileType.MethodGeneralAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodGeneralAllocation, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodAllocationOverride, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodAllocationOverrideFolder, methodsNode, aUserRID, eProfileType.MethodAllocationOverrideFolder, _sAllocationAllocationOverride, GetSecurity(aUserRID, eProfileType.MethodAllocationOverrideFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodAllocationOverrideSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodAllocationOverrideSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.AllocationOverride, eProfileType.MethodAllocationOverride, aChildren, GetSecurity(aUserRID, eProfileType.MethodAllocationOverride, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodAllocationOverrideSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodAllocationOverrideSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.AllocationOverride, eProfileType.MethodAllocationOverride, aChildren, GetSecurity(aUserRID, eProfileType.MethodAllocationOverride, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodRule, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodRuleFolder, methodsNode, aUserRID, eProfileType.MethodRuleFolder, _sAllocationRule, GetSecurity(aUserRID, eProfileType.MethodRuleFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodRuleSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodRuleSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.Rule, eProfileType.MethodRule, aChildren, GetSecurity(aUserRID, eProfileType.MethodRule, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodRuleSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodRuleSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.Rule, eProfileType.MethodRule, aChildren, GetSecurity(aUserRID, eProfileType.MethodRule, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodVelocity, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodVelocityFolder, methodsNode, aUserRID, eProfileType.MethodVelocityFolder, _sAllocationVelocity, GetSecurity(aUserRID, eProfileType.MethodVelocityFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodVelocitySubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodVelocitySubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.Velocity, eProfileType.MethodVelocity, aChildren, GetSecurity(aUserRID, eProfileType.MethodVelocity, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodVelocitySubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodVelocitySubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.Velocity, eProfileType.MethodVelocity, aChildren, GetSecurity(aUserRID, eProfileType.MethodVelocity, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }
                    if (GetSecurity(aUserRID, eProfileType.MethodBuildPacks, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodBuildPacksFolder, methodsNode, aUserRID, eProfileType.MethodBuildPacksFolder, _sAllocationBuildPacks, GetSecurity(aUserRID, eProfileType.MethodBuildPacksFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
						// Begin TT#1167 - Login Performance - Opening the application after Login
                        //AddFoldersToFolder(node, eProfileType.MethodBuildPacksSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacksSubFolder, eWorkflowType.None), eWorkflowType.None);
                        //AddMethodsToFolder(node, eMethodType.BuildPacks, eProfileType.MethodBuildPacks, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacks, eWorkflowType.None));
                        if (AddFoldersToFolder(node, eProfileType.MethodBuildPacksSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacksSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.BuildPacks, eProfileType.MethodBuildPacks, aChildren, GetSecurity(aUserRID, eProfileType.MethodBuildPacks, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
						// End TT#1167
                    }

					// Begin TT#1652-MD - stodd - DC Carton Rounding
                    if (GetSecurity(aUserRID, eProfileType.MethodDCCartonRounding, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodDCCartonRoundingFolder, methodsNode, aUserRID, eProfileType.MethodDCCartonRoundingFolder, _sAllocationDCCartonRounding, GetSecurity(aUserRID, eProfileType.MethodDCCartonRoundingFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        if (AddFoldersToFolder(node, eProfileType.MethodDCCartonRoundingSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodDCCartonRoundingSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.DCCartonRounding, eProfileType.MethodDCCartonRounding, aChildren, GetSecurity(aUserRID, eProfileType.MethodDCCartonRounding, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                    }
					// End TT#1652-MD - stodd - DC Carton Rounding

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    if (GetSecurity(aUserRID, eProfileType.MethodDCFulfillment, eWorkflowType.None).AllowView)
                    {
                        node = BuildFolderNode((int)eProfileType.MethodDCFulfillmentFolder, methodsNode, aUserRID, eProfileType.MethodDCFulfillmentFolder, _sAllocationDCFulfillment, GetSecurity(aUserRID, eProfileType.MethodDCFulfillmentFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        node.Sequence = ++sequence;
                        methodsNode.Nodes.Add(node);
                        if (AddFoldersToFolder(node, eProfileType.MethodDCFulfillmentSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodDCFulfillmentSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                            AddMethodsToFolder(node, eMethodType.DCFulfillment, eProfileType.MethodDCFulfillment, aChildren, GetSecurity(aUserRID, eProfileType.MethodDCFulfillment, eWorkflowType.None), false))
                        {
                            node.Nodes.Add(BuildPlaceHolderNode());
                            node.ChildrenLoaded = false;
                            node.HasChildren = true;
                            node.DisplayChildren = true;
                        }
                    }
                    // End TT#1966-MD - JSmith- DC Fulfillment

                    if ((aUserRID == Include.GlobalUserRID && isGlobalSizeAllocationAllowed) ||
                    (aUserRID != Include.GlobalUserRID && isUserSizeAllocationAllowed))
                    {
                        sizeNode = BuildFolderNode((int)eProfileType.WorkflowMethodAllocationSizeMethodsFolder, methodsNode, aUserRID, eProfileType.WorkflowMethodAllocationSizeMethodsFolder, _sAllocationSizeMethodsFolder, GetSecurity(aUserRID, eProfileType.WorkflowMethodAllocationSizeMethodsFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                        sizeNode.Sequence = ++sequence;
                        sequence = -1;
                        if (GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesAllocation, eWorkflowType.None).AllowView)
                        {
                            node = BuildFolderNode((int)eProfileType.MethodFillSizeHolesFolder, sizeNode, aUserRID, eProfileType.MethodFillSizeHolesFolder, _sAllocationFillSizeHoles, GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                            node.Sequence = ++sequence;
                            sizeNode.Nodes.Add(node);
							// Begin TT#1167 - Login Performance - Opening the application after Login
                            //AddFoldersToFolder(node, eProfileType.MethodFillSizeHolesSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesSubFolder, eWorkflowType.None), eWorkflowType.None);
                            //AddMethodsToFolder(node, eMethodType.FillSizeHolesAllocation, eProfileType.MethodFillSizeHolesAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesAllocation, eWorkflowType.None));
                            if (AddFoldersToFolder(node, eProfileType.MethodFillSizeHolesSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                                AddMethodsToFolder(node, eMethodType.FillSizeHolesAllocation, eProfileType.MethodFillSizeHolesAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodFillSizeHolesAllocation, eWorkflowType.None), false))
                            {
                                node.Nodes.Add(BuildPlaceHolderNode());
                                node.ChildrenLoaded = false;
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                            }
							// End TT#1167
                        }
                        if (GetSecurity(aUserRID, eProfileType.MethodBasisSizeAllocation, eWorkflowType.None).AllowView)
                        {
                            node = BuildFolderNode((int)eProfileType.MethodBasisSizeFolder, sizeNode, aUserRID, eProfileType.MethodBasisSizeFolder, _sAllocationBasisSize, GetSecurity(aUserRID, eProfileType.MethodBasisSizeFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                            node.Sequence = ++sequence;
                            sizeNode.Nodes.Add(node);
							// Begin TT#1167 - Login Performance - Opening the application after Login
                            //AddFoldersToFolder(node, eProfileType.MethodBasisSizeSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodBasisSizeSubFolder, eWorkflowType.None), eWorkflowType.None);
                            //AddMethodsToFolder(node, eMethodType.BasisSizeAllocation, eProfileType.MethodBasisSizeAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodBasisSizeAllocation, eWorkflowType.None));
                            if (AddFoldersToFolder(node, eProfileType.MethodBasisSizeSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodBasisSizeSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                                AddMethodsToFolder(node, eMethodType.BasisSizeAllocation, eProfileType.MethodBasisSizeAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodBasisSizeAllocation, eWorkflowType.None), false))
                            {
                                node.Nodes.Add(BuildPlaceHolderNode());
                                node.ChildrenLoaded = false;
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                            }
							// End TT#1167
                        }
                        if (GetSecurity(aUserRID, eProfileType.MethodSizeNeedAllocation, eWorkflowType.None).AllowView)
                        {
                            node = BuildFolderNode((int)eProfileType.MethodSizeNeedFolder, sizeNode, aUserRID, eProfileType.MethodSizeNeedFolder, _sAllocationSizeNeed, GetSecurity(aUserRID, eProfileType.MethodSizeNeedFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                            node.Sequence = ++sequence;
                            sizeNode.Nodes.Add(node);
							// Begin TT#1167 - Login Performance - Opening the application after Login
                            //AddFoldersToFolder(node, eProfileType.MethodSizeNeedSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeNeedSubFolder, eWorkflowType.None), eWorkflowType.None);
                            //AddMethodsToFolder(node, eMethodType.SizeNeedAllocation, eProfileType.MethodSizeNeedAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeNeedAllocation, eWorkflowType.None));
                            if (AddFoldersToFolder(node, eProfileType.MethodSizeNeedSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeNeedSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                                AddMethodsToFolder(node, eMethodType.SizeNeedAllocation, eProfileType.MethodSizeNeedAllocation, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeNeedAllocation, eWorkflowType.None), false))
                            {
                                node.Nodes.Add(BuildPlaceHolderNode());
                                node.ChildrenLoaded = false;
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                            }
							// End TT#1167
                        }
                        // Begin TT#155 - JSmith - Size Curve Method
                        if (GetSecurity(aUserRID, eProfileType.MethodSizeCurve, eWorkflowType.None).AllowView)
                        {
                            node = BuildFolderNode((int)eProfileType.MethodSizeCurveFolder, sizeNode, aUserRID, eProfileType.MethodSizeCurveFolder, _sAllocationSizeCurve, GetSecurity(aUserRID, eProfileType.MethodSizeCurveFolder, eWorkflowType.None), aOwnerUserRID, eTreeNodeType.MainSourceFolderNode, aIsShortcut);
                            node.Sequence = ++sequence;
                            sizeNode.Nodes.Add(node);
							// Begin TT#1167 - Login Performance - Opening the application after Login
                            //AddFoldersToFolder(node, eProfileType.MethodSizeCurveSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeCurveSubFolder, eWorkflowType.None), eWorkflowType.None);
                            //AddMethodsToFolder(node, eMethodType.SizeCurve, eProfileType.MethodSizeCurve, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeCurve, eWorkflowType.None));
                            if (AddFoldersToFolder(node, eProfileType.MethodSizeCurveSubFolder, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeCurveSubFolder, eWorkflowType.None), eWorkflowType.None, false) ||
                                AddMethodsToFolder(node, eMethodType.SizeCurve, eProfileType.MethodSizeCurve, aChildren, GetSecurity(aUserRID, eProfileType.MethodSizeCurve, eWorkflowType.None), false))
                            {
                                node.Nodes.Add(BuildPlaceHolderNode());
                                node.ChildrenLoaded = false;
                                node.HasChildren = true;
                                node.DisplayChildren = true;
                            }
							// End TT#1167
                        }
                        // End TT#155
                        methodsNode.Nodes.Add(sizeNode);
                    }

                    groupNode.Nodes.Add(methodsNode);
                }

                return groupNode;
            }
            catch
            {
                throw;
            }
        }

        public int GetImageIndex(MIDWorkflowMethodTreeNode aNode, string aFolderType)
        {
            try
            {
                return MIDGraphics.ImageIndexWithDefault(Include.MIDDefaultColor, aFolderType);
            }
            catch
            {
                throw;
            }
        }

        override public void RefreshShortcuts(MIDTreeNode aStartNode, MIDTreeNode aChangedNode)
        {
            try
            {
                if (aChangedNode != null)
                {
                    foreach (MIDTreeNode node in aStartNode.Nodes)
                    {
                        if (node.Profile.Key == aChangedNode.Profile.Key && node.Profile.ProfileType == aChangedNode.Profile.ProfileType)
                        {
                            node.RefreshShortcutNode(aChangedNode);
                        }
                        else if (node.isSubFolder || node.isFolderShortcut ||
                            (node.isChildShortcut && node.isSubFolder))
                        {
                            RefreshShortcuts(node, aChangedNode);
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

        /// <summary>
        /// Virtual method that is called after a label has been updated
        /// </summary>
        /// <returns>
        /// A boolean indicating if post-processing was successful
        /// </returns>

        override protected bool AfterLabelUpdate(MIDTreeNode aNode, string aNewName)
        {
            int key;
            FolderProfile folderProf;
            try
            {
                if (((MIDWorkflowMethodTreeNode)aNode).isMethod ||
                    ((MIDWorkflowMethodTreeNode)aNode).isWorkflow)
                {
                    // Begin TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application
                    string errMsg;

                    MIDEnqueue MIDEnqueueData = new MIDEnqueue();

                    if (((MIDWorkflowMethodTreeNode)aNode).isMethod)
                    {
                        MethodEnqueue methodEnqueue = new MethodEnqueue(
                                                                        ((MIDWorkflowMethodTreeNode)aNode).Key,
                                                                        SAB.ClientServerSession.UserRID,
                                                                        SAB.ClientServerSession.ThreadID);

                        try
                        {
                            methodEnqueue.EnqueueMethod(false);
                        }
                        catch (MethodConflictException)
                        {
                            errMsg = "The following method requested:" + System.Environment.NewLine;
                            foreach (MethodConflict MCon in methodEnqueue.ConflictList)
                            {
                                errMsg += System.Environment.NewLine + "Method: " + Text + ", User: " + MCon.UserName;
                            }
                            errMsg += System.Environment.NewLine + System.Environment.NewLine;

                            errMsg += "The selected method can not be updated at this time.";

                            if (MIDEnvironment.isWindows)
                            {
                                MessageBox.Show(errMsg, this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MIDEnvironment.Message = errMsg;
                                MIDEnvironment.requestFailed = true;
                            }
                            return false;
                        }
                    }
                    else
                    {
                        WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
                                                                            ((MIDWorkflowMethodTreeNode)aNode).Key,
                                                                            SAB.ClientServerSession.UserRID,
                                                                            SAB.ClientServerSession.ThreadID);

                        try
                        {
                            workflowEnqueue.EnqueueWorkflow(false);
                        }
                        catch (WorkflowConflictException)
                        {
                            errMsg = "The following workflow requested:" + System.Environment.NewLine;
                            foreach (WorkflowConflict WCon in workflowEnqueue.ConflictList)
                            {
                                errMsg += System.Environment.NewLine + "Workflow: " + Text + ", User: " + WCon.UserName;
                            }
                            errMsg += System.Environment.NewLine + System.Environment.NewLine;
                            errMsg += "The selected workflow can not be updated at this time.";

                            if (MIDEnvironment.isWindows)
                            {
                                MessageBox.Show(errMsg, this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MIDEnvironment.Message = errMsg;
                                MIDEnvironment.requestFailed = true;
                            }
                            return false;
                        }
                    }
                    // End TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application

                    return RenameNode((MIDWorkflowMethodTreeNode)aNode, aNewName);
                }
                else
                {
                    key = DlFolder.Folder_GetKey(aNode.UserId, aNewName, aNode.ParentId, aNode.NodeProfileType);

                    if (key != -1)
                    {
                        if (MIDEnvironment.isWindows)
                        {
                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FolderNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FolderNameExists);
                            MIDEnvironment.requestFailed = true;
                        }
                        return false;
                    }

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Rename(aNode.Profile.Key, aNewName);
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

                    folderProf = (FolderProfile)aNode.Profile;
                    folderProf.Name = aNewName;
                    return true;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method used to create a shortcut in another folder
        /// </summary>
        /// <param name="aFromNode">
        /// The MIDTreeNode being copied
        /// </param>
        /// <param name="aToNode">
        /// The MIDTreeNode where new node is being copied to
        /// </param>

        override public void CreateShortcut(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDWorkflowMethodTreeNode toNode;
            MIDWorkflowMethodTreeNode newNode = null;
            MIDWorkflowMethodTreeNode fromNode = null;
            int folderRID;

            try
            {
                fromNode = (MIDWorkflowMethodTreeNode)aFromNode;
                if (((MIDWorkflowMethodTreeNode)aToNode).isMethod ||
                    ((MIDWorkflowMethodTreeNode)aToNode).isWorkflow)
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode.Parent;
                }
                else
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode;
                }

                if (toNode.isMethodFolder ||
                    toNode.isWorkflowFolder)
                {
                    folderRID = toNode.GetGroupKey();
                }
                else
                {
                    folderRID = toNode.Profile.Key;
                }

                if (DlFolder.Folder_Shortcut_Exists(folderRID, aFromNode.Profile.Key, aFromNode.Profile.ProfileType))
                {
                    if (MIDEnvironment.isWindows)
                    {
                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ShortcutExists);
                        MIDEnvironment.requestFailed = true;
                    }
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;
                BeginUpdate();

                if (fromNode.isMethod ||
                    fromNode.isWorkflow)
                {
                    if (fromNode.isMethod)
                    {
                        newNode = (MIDWorkflowMethodTreeNode)BuildMethodNode(aFromNode.Profile.Key, toNode, (eMethodType)fromNode.MethodType, aFromNode.FunctionSecurityProfile, aFromNode.UserId, aFromNode.OwnerUserRID, aToNode.Profile.Key, true);
                    }
                    else
                    {
                        newNode = (MIDWorkflowMethodTreeNode)BuildWorkflowNode(aFromNode.Profile.Key, toNode, fromNode.WorkflowType, aFromNode.FunctionSecurityProfile, aFromNode.UserId, aFromNode.OwnerUserRID, aToNode.Profile.Key, true);
                    }

                    aToNode.Nodes.Add(newNode);

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(folderRID, newNode.Profile.Key, aFromNode.NodeProfileType);
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
                else if (fromNode.NodeProfileType == eProfileType.WorkflowMethodSubFolder ||
                    fromNode.isWorkflowMethodSubFolder)
                {
                    newNode = BuildFolderShortcutNode((MIDWorkflowMethodTreeNode)aFromNode, toNode);
                    
                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, aFromNode.NodeProfileType);
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
                else if (fromNode.NodeProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    newNode = BuildOTSForecastGroup(toNode, aFromNode.Profile.Key, aFromNode.UserId, aFromNode.OwnerUserRID, true);
                    toNode.Nodes.Add(newNode);

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, aFromNode.NodeProfileType);
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
                else if (fromNode.NodeProfileType == eProfileType.WorkflowMethodAllocationFolder)
                {
                    newNode = BuildAllocationGroup(toNode, aFromNode.Profile.Key, aFromNode.UserId, aFromNode.OwnerUserRID, true);
                    toNode.Nodes.Add(newNode);

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(toNode.Profile.Key, newNode.Profile.Key, aFromNode.NodeProfileType);
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

                SortChildNodes(toNode);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                EndUpdate();
                Cursor.Current = Cursors.Default;
            }
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

        override protected MIDTreeNode MoveNode(MIDTreeNode aFromNode, MIDTreeNode aToNode)
        {
            MIDWorkflowMethodTreeNode toNode;

            try
            {
                if (((MIDWorkflowMethodTreeNode)aToNode).isMethod ||
                    ((MIDWorkflowMethodTreeNode)aToNode).isWorkflow)
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode.Parent;
                }
                else
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode;
                }

                try
                {
                    return MoveWorkflowMethodNode((MIDWorkflowMethodTreeNode)aFromNode, toNode);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    EndUpdate();
                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
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

        override protected MIDTreeNode CopyNode(MIDTreeNode aFromNode, MIDTreeNode aToNode, bool aFindUniqueName)
        {
            MIDWorkflowMethodTreeNode toNode;
            MIDWorkflowMethodTreeNode fromNode;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                fromNode = (MIDWorkflowMethodTreeNode)aFromNode;
                if (((MIDWorkflowMethodTreeNode)aToNode).isMethod ||
                    ((MIDWorkflowMethodTreeNode)aToNode).isWorkflow)
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode.Parent;
                }
                else
                {
                    toNode = (MIDWorkflowMethodTreeNode)aToNode;
                }

                try
                {
                    return CopyWorkflowMethodNode(fromNode, toNode, aFindUniqueName);
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        /// <summary>
        /// Virtual method used to determine if a MIDTreeNode is InUse.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being evaluated.
        /// </param>

        override protected void InUseNode(MIDTreeNode aNode)
        {
            try
            {
                if (aNode != null)
                {
                    InUseWorflowMethodNode(aNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override protected bool AllowInUseDelete(ArrayList aDeleteList)
        {
            // Begin TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder
            //return InUseDeleteNode(aDeleteList);
            return InUseDeleteNode(GetObjectNodes(aDeleteList));
            // End TT#4285 - JSmith - Foreign Key Error When Delete Attribute Folder
        }
        //END TT#110-MD-VStuart - In Use Tool

        /// <summary>
        /// Virtual method used to delete a MIDTreeNode
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being deleted
        /// </param>

        // Begin TT#3630 - JSmith - Delete My Hierarchy
        //override protected void DeleteNode(MIDTreeNode aNode)
        override protected void DeleteNode(MIDTreeNode aNode, out bool aDeleteCancelled)
        // End TT#3630 - JSmith - Delete My Hierarchy
        {
            try
            {
                aDeleteCancelled = false;  // TT#3630 - JSmith - Delete My Hierarchy
                if (aNode != null)
                {
                    ////BEGIN TT#110-MD-VStuart - In Use Tool
                    //var allowDelete = false;
                    //var _nodeArrayList = new ArrayList();
                    //_nodeArrayList.Add(aNode.NodeRID);
                    //var _eProfileType = new eProfileType();
                    //_eProfileType = aNode.NodeProfileType;
                    //string inUseTitle = Regex.Replace(_eProfileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                    //DisplayInUseForm(_nodeArrayList, _eProfileType, inUseTitle, false, out allowDelete);
                    //if (!allowDelete)
                    ////END TT#110-MD-VStuart - In Use Tool
                    DeleteWorflowMethodNode((MIDWorkflowMethodTreeNode)aNode);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode MoveWorkflowMethodNode(MIDWorkflowMethodTreeNode aFromNode, MIDWorkflowMethodTreeNode aToNode)
        {
            MIDWorkflowMethodTreeNode newNode;
            FolderProfile folderProf;
            int newParentKey;
            ApplicationBaseMethod method;
            ApplicationBaseWorkFlow workflow;
            bool isShortcut = false;
            MIDWorkflowMethodTreeNode toNode;
            int folderRID;
            eProfileType folderType;
            object[] moveArray;
            
            try
            {
                toNode = (MIDWorkflowMethodTreeNode)aToNode;
				if (aFromNode.isFolderShortcut || aFromNode.isObjectShortcut)
                {
                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Delete(aFromNode.ParentId, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                        DlFolder.Folder_Shortcut_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

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

                    aFromNode.Remove();

					if (aFromNode.isFolderShortcut)
                    {
                        newNode = BuildFolderShortcutNode(aFromNode, aToNode);
                    }
                    else
                    {
                        newNode = BuildObjectShortcutNode(aFromNode, aToNode);
                    }

                    SortChildNodes(aToNode);

                    return aFromNode;
                }
                else if (aFromNode.isSubFolder || aFromNode.isGroupFolder)
                {
                    folderProf = (FolderProfile)aFromNode.Profile;
                    folderRID = aToNode.GetFolderParentKey();
                    if (aFromNode.isGroupFolder)
                    {
                        folderType = aFromNode.Profile.ProfileType;
                    }
                    else
                    {
                        folderType = aToNode.GetFolderType();
                    }

                    if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                    {
                        DlSecurity.OpenUpdateConnection();

                        try
                        {
                            DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(folderProf.ProfileType), folderProf.Key);
                            DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(folderProf.ProfileType), folderProf.Key, aToNode.UserId);

                            DlSecurity.CommitData();
                        }
                        catch (Exception exc)
                        {
                            string message = exc.ToString();
                            throw;
                        }
                        finally
                        {
                            DlSecurity.CloseUpdateConnection();
                        }

                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, folderRID, folderType);
                        folderProf.UserRID = aToNode.UserId;
                        folderProf.OwnerUserRID = aToNode.UserId;
                    }

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Update(folderProf.Key, folderProf.UserRID, folderProf.Name, folderProf.ProfileType);
                        DlFolder.Folder_Item_Delete(aFromNode.Profile.Key, folderProf.ProfileType);
                        DlFolder.Folder_Item_Insert(folderRID, aFromNode.Profile.Key, folderProf.ProfileType);

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

                    newNode = BuildSubFolderNode(folderProf, aToNode, aFromNode.Sequence);

                    if (aFromNode.HasChildren &&
                        !aFromNode.ChildrenLoaded)
                    {
                        aFromNode.BuildChildren();
                    }

                    moveArray = new object[aFromNode.Nodes.Count];
                    aFromNode.Nodes.CopyTo(moveArray, 0);

                    foreach (MIDWorkflowMethodTreeNode node in moveArray)
                    {
                        MoveWorkflowMethodNode(node, newNode);
                    }

                    aFromNode.Remove();

                    return newNode;
                }
                else if (aFromNode.isMethod)
                {
                    try
                    {
                        method = (ApplicationBaseMethod)aFromNode.Profile;

                        if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                        {
                            DlSecurity.OpenUpdateConnection();

                            try
                            {
                                DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(method.ProfileType), method.Key);
                                DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(method.ProfileType), method.Key, aToNode.UserId);

                                DlSecurity.CommitData();
                            }
                            catch (Exception exc)
                            {
                                string message = exc.ToString();
                                throw;
                            }
                            finally
                            {
                                DlSecurity.CloseUpdateConnection();
                            }

                            method.Name = FindNewMethodName(method.Name, aToNode.UserId);
                            method.User_RID = aToNode.UserId;
                            //method. = aToNode.UserId;
                        }

                        // Begin TT#3108 - JSmith - Missing Forecast Workflows
                        //if (aToNode.isMethodFolder)
                        if (aToNode.isMethodFolder ||
                            aToNode.isWorkflowFolder)
                        // End TT#3108 - JSmith - Missing Forecast Workflows
                        {
                            newParentKey = aToNode.GetGroupKey();
                        }
                        else
                        {
                            newParentKey = aToNode.Profile.Key;
                        }
                        DlFolder.OpenUpdateConnection();
                        try
                        {
                            DlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                            DlFolder.Folder_Item_Insert(newParentKey, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

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

                        aFromNode.Remove();
                    }
                    catch
                    {
                        throw;
                    }

                    newNode = (MIDWorkflowMethodTreeNode)BuildMethodNode(aFromNode.Profile.Key, toNode, (eMethodType)aFromNode.MethodType, aFromNode.FunctionSecurityProfile, aToNode.UserId, aToNode.OwnerUserRID, aToNode.Profile.Key, isShortcut);
                    aToNode.Nodes.Add(newNode);
                    return newNode;
                }
                else if (aFromNode.isWorkflow)
                {
                    try
                    {
                        workflow = (ApplicationBaseWorkFlow)aFromNode.Profile;

                        if (aToNode.GetTopSourceNode() != aFromNode.GetTopSourceNode())
                        {
                            DlSecurity.OpenUpdateConnection();

                            try
                            {
                                DlSecurity.DeleteUserItemByTypeAndRID(Convert.ToInt32(workflow.ProfileType), workflow.Key);
                                DlSecurity.AddUserItem(aToNode.UserId, Convert.ToInt32(workflow.ProfileType), workflow.Key, aToNode.UserId);

                                DlSecurity.CommitData();
                            }
                            catch (Exception exc)
                            {
                                string message = exc.ToString();
                                throw;
                            }
                            finally
                            {
                                DlSecurity.CloseUpdateConnection();
                            }

                            workflow.WorkFlowName = FindNewWorkflowName(workflow.WorkFlowName, aToNode.UserId);
                            workflow.UserRID = aToNode.UserId;
                            //workflow.OwnerUserRID = aToNode.UserId;
                        }

                        // Begin TT#3108 - JSmith - Missing Forecast Workflows
                        //if (aToNode.isMethodFolder)
                        if (aToNode.isMethodFolder ||
                            aToNode.isWorkflowFolder)
                        // End TT#3108 - JSmith - Missing Forecast Workflows
                        {
                            newParentKey = aToNode.GetGroupKey();
                        }
                        else
                        {
                            newParentKey = aToNode.Profile.Key;
                        }
                        DlFolder.OpenUpdateConnection();
                        try
                        {
                            DlFolder.Folder_Item_Delete(aFromNode.Profile.Key, aFromNode.Profile.ProfileType);
                            DlFolder.Folder_Item_Insert(newParentKey, aFromNode.Profile.Key, aFromNode.Profile.ProfileType);

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

                        aFromNode.Remove();
                    }
                    catch 
                    {
                        throw;
                    }

                    newNode = (MIDWorkflowMethodTreeNode)BuildWorkflowNode(aFromNode.Profile.Key, toNode, aFromNode.WorkflowType, aFromNode.FunctionSecurityProfile, aToNode.UserId, aToNode.OwnerUserRID, aToNode.Profile.Key, isShortcut);
                    aToNode.Nodes.Add(newNode);
                    return newNode;
                }
                else if (aFromNode.isMethodFolder ||
                    aFromNode.isWorkflowFolder ||
                    aFromNode.isMethodsFolder) // group member node so just loop through them to get to the descendants
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;
                    folderProf.OwnerUserRID = aToNode.UserId;
                    newNode = BuildSubFolderNode(folderProf, aToNode, aFromNode.Sequence);

                    if (aFromNode.HasChildren)
                    {
                        if (aFromNode.HasChildren &&
                            !aFromNode.ChildrenLoaded)
                        {
                            aFromNode.BuildChildren();
                        }

                        moveArray = new object[aFromNode.Nodes.Count];
                        aFromNode.Nodes.CopyTo(moveArray, 0);

                        foreach (MIDWorkflowMethodTreeNode node in moveArray)
                        {
                            MoveWorkflowMethodNode(node, newNode);
                        }

                        aFromNode.Remove();
                        //SortChildNodes(newNode);
                    }

                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode CopyWorkflowMethodNode(MIDWorkflowMethodTreeNode aFromNode, MIDWorkflowMethodTreeNode aToNode, bool aFindUniqueName)
        {
            FolderProfile folderProf;
            MIDWorkflowMethodTreeNode newNode = null;
            WorkflowMethodFormBase childForm = null;
            bool newForm;
            bool copySuccessful = true;
            int newItemKey;
            bool isShortcut = false;
            MIDWorkflowMethodTreeNode toNode;
            int userId;
            int ownerUserRID;
            int folderRID;
            eProfileType folderType;
            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
            string newItemName = string.Empty;
            // End TT#1167

            try
            {
                toNode = (MIDWorkflowMethodTreeNode)aToNode;
                if (aFromNode.isSubFolder || aFromNode.isGroupFolder)
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;
                    folderProf.OwnerUserRID = aToNode.UserId;
                    folderRID = aToNode.GetFolderParentKey();
                    if (aFromNode.isGroupFolder)
                    {
                        folderType = aFromNode.Profile.ProfileType;
                    }
                    else
                    {
                        folderType = aToNode.GetFolderType();
                    }

                    if (aFindUniqueName)
                    {
                        folderProf.Name = FindNewFolderName(folderProf.Name, aToNode.UserId, folderRID, folderType);
                    }

                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        folderProf.Key = DlFolder.Folder_Create(folderProf.UserRID, folderProf.Name, folderType);
                        DlFolder.Folder_Item_Insert(folderRID, folderProf.Key, folderType);

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

                    newNode = BuildSubFolderNode(folderProf, aToNode, aFromNode.Sequence);

                    if (aFromNode.HasChildren &&
                        !aFromNode.ChildrenLoaded)
                    {
                        aFromNode.BuildChildren();
                    }

                    foreach (MIDWorkflowMethodTreeNode node in aFromNode.Nodes)
                    {
                        CopyWorkflowMethodNode(node, newNode, aFindUniqueName);
                    }

                    SortChildNodes(newNode);

                    return newNode;
                }
				else if (aFromNode.isFolderShortcut)
                {
                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType);
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

                    newNode = BuildFolderShortcutNode(aFromNode, aToNode);
                    SortChildNodes(newNode);

                    return newNode;
                }
                else if (aFromNode.isObjectShortcut)
                {
                    DlFolder.OpenUpdateConnection();

                    try
                    {
                        DlFolder.Folder_Shortcut_Insert(aToNode.Profile.Key, aFromNode.Profile.Key, aFromNode.NodeProfileType);
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

                    newNode = BuildObjectShortcutNode(aFromNode, aToNode);
                    toNode.Nodes.Add(newNode);
                    SortChildNodes(newNode);

                    return newNode;
                }
                else if (aFromNode.isMethod || aFromNode.isWorkflow)
                {
                    try
                    {
                        newForm = true;
                        childForm = GetForm((MIDWorkflowMethodTreeNode)aFromNode, ref newForm);
                        copySuccessful = true;
                        newItemKey = Include.NoRID;
                        if (newForm)
                        {
                            childForm.Tag = SelectedNode.Profile.Key;
                            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                            //copySuccessful = childForm.CopyWorkflowMethod(aFromNode, aToNode, ref newItemKey);
                            copySuccessful = childForm.CopyWorkflowMethod(aFromNode, aToNode, ref newItemKey, ref newItemName);
                            // End TT#1167
                        }
                        else
                        {
                            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                            //copySuccessful = childForm.CopyWorkflowMethod(aFromNode, aToNode, ref newItemKey);
                            copySuccessful = childForm.CopyWorkflowMethod(aFromNode, aToNode, ref newItemKey, ref newItemName);
                            // End TT#1167
                            childForm.Close();
                        }
                        if (copySuccessful)
                        {
                            DlFolder.OpenUpdateConnection();

                            try
                            {
                                // Begin Track #6375 - JSmith - Assign user multiple times does not work
                                //if (aToNode.isMethodFolder)
                                //{
                                if (aToNode.isMethodFolder ||
                                    aToNode.isWorkflowFolder)
                                {
                                // End Track #6375
                                    folderRID = aToNode.GetGroupKey();
                                }
                                else
                                {
                                    folderRID = aToNode.Profile.Key;
                                }
                                DlFolder.Folder_Item_Insert(folderRID, newItemKey, aFromNode.Profile.ProfileType);

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

                            if (toNode.GetTopNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
                            {
                                userId = toNode.GetTopNode().UserId;
                                ownerUserRID = toNode.GetTopNode().OwnerUserRID;
                            }
                            else
                            {
                                userId = aFromNode.UserId;
                                ownerUserRID = aFromNode.OwnerUserRID;
                            }

                            if (aFromNode.isMethod)
                            {
                                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                                AddMethodToList(newItemKey, newItemName, userId);
                                AddToChildrenList(toNode, newItemKey, Convert.ToInt32(aFromNode.Profile.ProfileType), userId, ownerUserRID, isShortcut);
                                // End TT#1167
                                // Begin TT#1437 - JSmith - Security wrong after copy
                                //newNode = (MIDWorkflowMethodTreeNode)BuildMethodNode(newItemKey, toNode, (eMethodType)aFromNode.MethodType, aFromNode.FunctionSecurityProfile, userId, ownerUserRID, aToNode.Profile.Key, isShortcut);
                                newNode = (MIDWorkflowMethodTreeNode)BuildMethodNode(newItemKey, toNode, (eMethodType)aFromNode.MethodType, GetSecurity(userId, aFromNode.Profile.ProfileType, aFromNode.WorkflowType), userId, ownerUserRID, aToNode.Profile.Key, isShortcut);
                                // BegEnd TT#1437
                                
                            }
                            else
                            {
                                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                                AddWorkflowToList(newItemKey, newItemName, userId);
                                AddToChildrenList(toNode, newItemKey, Convert.ToInt32(aFromNode.Profile.ProfileType), userId, ownerUserRID, isShortcut);
                                // End TT#1167
                                // Begin TT#1437 - JSmith - Security wrong after copy
                                //newNode = (MIDWorkflowMethodTreeNode)BuildWorkflowNode(newItemKey, toNode, aFromNode.WorkflowType, aFromNode.FunctionSecurityProfile, userId, ownerUserRID, aToNode.Profile.Key, isShortcut);
                                newNode = (MIDWorkflowMethodTreeNode)BuildWorkflowNode(newItemKey, toNode, aFromNode.WorkflowType, GetSecurity(userId, aFromNode.Profile.ProfileType, aFromNode.WorkflowType), userId, ownerUserRID, aToNode.Profile.Key, isShortcut);
                                // BegEnd TT#1437
                            }
                            toNode.Nodes.Add(newNode);
                        }
                    }
                    catch (DatabaseForeignKeyViolation)
                    {
                        copySuccessful = false;
                    }

                    return newNode;
                }
                else if (aFromNode.isMethodFolder ||
                    aFromNode.isWorkflowFolder ||
                    aFromNode.isMethodsFolder) // group member node so just loop through them to get to the descendants
                {
                    folderProf = (FolderProfile)((FolderProfile)aFromNode.Profile).Clone();
                    folderProf.UserRID = aToNode.UserId;
                    folderProf.OwnerUserRID = aToNode.UserId;
                    newNode = BuildSubFolderNode(folderProf, aToNode, aFromNode.Sequence);

                    if (aFromNode.HasChildren)
                    {
                        if (aFromNode.HasChildren &&
                            !aFromNode.ChildrenLoaded)
                        {
                            aFromNode.BuildChildren();
                        }

                        foreach (MIDWorkflowMethodTreeNode node in aFromNode.Nodes)
                        {
                            CopyWorkflowMethodNode(node, newNode, aFindUniqueName);
                        }

                        SortChildNodes(newNode);
                    }

                    return newNode;
                }
                else
                {
                    throw new Exception("Unexpected Node encountered");
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        private bool InUseWorflowMethodNode(MIDTreeNode aNode)
        {
            const bool inUseSuccessful = false;

            try
            {
                if (aNode != null)
                {
                    int scgRid = aNode.NodeRID;
                    var ridList = new ArrayList();
                    //BEGIN TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                    //if (aNode.FunctionSecurityProfile.AllowView)
                    //{
                        if (scgRid > 0)
                            ridList.Add(scgRid);

                        eProfileType etype = aNode.NodeProfileType;
                        //string inUseTitle = Regex.Replace(etype.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim(); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                        string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype); //TT#1532-MD -jsobek -Add In Use for Header Characteristics
                        bool display = true;
                        const bool inQuiry = true;
                        DisplayInUseForm(ridList, etype, inUseTitle, ref display, inQuiry);
                    //}
                    //END TT#3195-M-VStuart-In Use is not picking up the Filter XREF information-MID
                }
                return inUseSuccessful;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private bool InUseDeleteNode(ArrayList aDeleteList)
        {
            bool allowDelete = true;
            bool dialogShown = false;
            ArrayList nodeArrayList = new ArrayList();
            eProfileType profileType =eProfileType.None;
            string inUseTitle;

            try
            {
                if (aDeleteList != null)
                {
                    foreach (MIDTreeNode aNode in aDeleteList)
                    {
                        // Begin TT#4326 - JSmith - Unable to Remove shortcuts that are In Use
                        //nodeArrayList.Add(aNode.NodeRID);
                        //profileType = aNode.NodeProfileType;
                        if (!aNode.isShortcut)
                        {
                            nodeArrayList.Add(aNode.NodeRID);
                            profileType = aNode.NodeProfileType;
                        }
                        // End TT#4326 - JSmith - Unable to Remove shortcuts that are In Use
                    }

                    if (nodeArrayList.Count > 0)
                    {
                        //inUseTitle = Regex.Replace(profileType.ToString(), "((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", " $1").Trim();
                        inUseTitle = InUseUtility.GetInUseTitleFromProfileType(profileType); //TT#4304 -jsobek -Store Characteristic In Use not being reported on Store Filters
                        DisplayInUseForm(nodeArrayList, profileType, inUseTitle, false, out dialogShown);
                        if (dialogShown)
                        {
                            allowDelete = false;
                        }
                    }
                }
                return allowDelete;

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //END TT#110-MD-VStuart - In Use Tool

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


        private bool DeleteWorflowMethodNode(MIDTreeNode aNode)
        {
            bool newForm;
            bool deleteSuccessful = false;
            object[] deleteArray;

            try
            {
                WorkflowMethodFormBase childForm = null;
                newForm = true;

                if (aNode != null)
                {
                    if (aNode.FunctionSecurityProfile.AllowDelete)
                    {
                        if (aNode.isObject)
                        {
                            childForm = GetForm((MIDWorkflowMethodTreeNode)aNode, ref newForm);

                            if (newForm)
                            {
                                childForm.Tag = SelectedNode.Profile.Key;
                                deleteSuccessful = childForm.DeleteWorkflowMethod((MIDWorkflowMethodTreeNode)aNode);
                            }
                            else
                            {
                                deleteSuccessful = childForm.DeleteWorkflowMethod((MIDWorkflowMethodTreeNode)aNode);
                                childForm.Close();
                            }
                            if (deleteSuccessful)
                            {
                                DlFolder.OpenUpdateConnection();

                                try
                                {
                                    DlFolder.Folder_Item_Delete(aNode.Profile.Key, aNode.Profile.ProfileType);
                                    DlFolder.Folder_Shortcut_DeleteAll(aNode.Profile.Key, aNode.Profile.ProfileType);
                                    
                                    DlFolder.CommitData();
                                }
                                catch (DatabaseForeignKeyViolation)
                                {
                                    if (MIDEnvironment.isWindows)
                                    {
                                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                                        MIDEnvironment.requestFailed = true;
                                    }
                                    return false;
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

                                if (_favoritesNode.HasChildren
                                    && _favoritesNode.DisplayChildren
                                    && !_favoritesNode.ChildrenLoaded)
                                {
                                    ExpandNode(_favoritesNode);
                                }

                                DeleteShortcuts(_favoritesNode, aNode);
                                aNode.Remove();
                            }
                        }
                        else if (aNode.isObjectShortcut)
                        {
                            DlFolder.OpenUpdateConnection();

                            try
                            {
                                DlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, aNode.Profile.ProfileType);
                                DlFolder.CommitData();
                            }
                            catch (DatabaseForeignKeyViolation)
                            {
                                if (MIDEnvironment.isWindows)
                                {
                                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                                    MIDEnvironment.requestFailed = true;
                                }
                                return false;
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

                            aNode.Remove();
                        }
						else if (aNode.isFolderShortcut)
                        {
                            DlFolder.OpenUpdateConnection();

                            try
                            {
                                DlFolder.Folder_Shortcut_Delete(aNode.ParentId, aNode.Profile.Key, aNode.Profile.ProfileType);
                                DlFolder.CommitData();
                            }
                            catch (DatabaseForeignKeyViolation)
                            {
                                if (MIDEnvironment.isWindows)
                                {
                                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    MIDEnvironment.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse);
                                    MIDEnvironment.requestFailed = true;
                                }
                                return false;
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

                            DeleteChildNodes((MIDWorkflowMethodTreeNode)aNode);
                            aNode.Remove();
                        }
                        else if (aNode.isSubFolder ||
                            ((MIDWorkflowMethodTreeNode)aNode).isGroupFolder)
                        {
                            if (aNode.HasChildren &&
                                !aNode.ChildrenLoaded)
                            {
                                aNode.BuildChildren();
                            }

                            deleteArray = new object[aNode.Nodes.Count];
                            aNode.Nodes.CopyTo(deleteArray, 0);

                            foreach (MIDWorkflowMethodTreeNode node in deleteArray)
                            {
                                DeleteWorflowMethodNode(node);
                            }

                            if (aNode.Nodes.Count == 0)
                            {
                                DlFolder.OpenUpdateConnection();

                                try
                                {
                                    DlFolder.Folder_Item_Delete(aNode.Profile.Key, aNode.Profile.ProfileType);
                                    DlFolder.Folder_Delete(aNode.Profile.Key, aNode.Profile.ProfileType);
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

                                DeleteShortcuts(_favoritesNode, aNode);
                                DeleteChildNodes((MIDWorkflowMethodTreeNode)aNode);
                                aNode.Remove();
                            }
                        }
                        else if (aNode.HasChildren)
                        {
                            deleteArray = new object[aNode.Nodes.Count];
                            aNode.Nodes.CopyTo(deleteArray, 0);

                            foreach (MIDWorkflowMethodTreeNode node in deleteArray)
                            {
                                DeleteWorflowMethodNode(node);
                            }

                            aNode.Remove();
                        }
                        else
                        {
                            aNode.Remove();
                        }
                    }
                }
                return deleteSuccessful;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
        /// Virtual method executed after the New Item menu item has been clicked.
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode that was clicked on
        /// </param>
		//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

		//override protected void CreateNewItem(MIDTreeNode aParentNode)
		/// <returns>
		/// The new node that was created.  If node is returned, it will be placed in edit mode.
		/// If node is not available or edit mode is not desired, return null.
		/// </returns>

		override protected MIDTreeNode CreateNewItem(MIDTreeNode aParentNode)
		//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
		{
            bool newForm;
            try
            {
                WorkflowMethodFormBase childForm = null;
                newForm = true;

                if (SelectedNode != null)
                {
                    if (SelectedNode.FunctionSecurityProfile.AllowUpdate ||
                        SelectedNode.FunctionSecurityProfile.AllowView)
                    {
                        childForm = GetForm((MIDWorkflowMethodTreeNode)aParentNode, ref newForm);
                        childForm.MdiParent = MDIParentForm;

                        int dummy = 0; // dummy field that is just filler for the base form call
                        childForm.NewWorkflowMethod((MIDWorkflowMethodTreeNode)SelectedNode, dummy);
                        childForm.Tag = SelectedNode.Profile.Key;
                        childForm.OnPropertyChangeHandler += new WorkflowMethodFormBase.PropertyChangeEventHandler(OnPropertiesChange);

                        childForm.Show();
                        childForm.Focus();
                        if (childForm.WindowState == FormWindowState.Minimized)
                        {
                            childForm.WindowState = FormWindowState.Normal;
                        }
                    }
                }
				//Begin Track #6257 - JScott - Create New Attribute requires user to right-click rename

				return null;
				//End Track #6257 - JScott - Create New Attribute requires user to right-click rename
			}
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
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

        override protected MIDTreeNode CreateNewFolder(MIDTreeNode aNode, int aUserId)
        {
            FolderProfile newFolderProf;
            string newNodeName;
            MIDWorkflowMethodTreeNode newNode;
            MIDWorkflowMethodTreeNode folderNode;
            FunctionSecurityProfile functionSecurity;
            eProfileType folderType;
            int folderRID;

            try
            {
                folderNode = (MIDWorkflowMethodTreeNode)aNode;
                if (folderNode.isMethodSubFolder ||
                    folderNode.isWorkflowSubFolder)
                {
                    folderType = aNode.Profile.ProfileType;
                    folderRID = aNode.Profile.Key;
                }
                else
                {
                    folderType = folderNode.GetFolderType();
                    folderRID = folderNode.GetFolderParentKey();
                    //switch (folderNode.GetPathType())
                    //{
                    //    case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                    //        folderType = eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodOTSPlanFolder:
                    //        folderType = eProfileType.MethodOTSPlanSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodForecastBalanceFolder:
                    //        folderType = eProfileType.MethodForecastBalanceSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodModifySalesFolder:
                    //        folderType = eProfileType.MethodModifySalesSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodForecastSpreadFolder:
                    //        folderType = eProfileType.MethodForecastSpreadSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodCopyChainForecastFolder:
                    //        folderType = eProfileType.MethodCopyChainForecastSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodCopyStoreForecastFolder:
                    //        folderType = eProfileType.MethodCopyStoreForecastSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodExportFolder:
                    //        folderType = eProfileType.MethodExportSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodGlobalUnlockFolder:
                    //        folderType = eProfileType.MethodGlobalUnlockSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodRollupFolder:
                    //        folderType = eProfileType.MethodRollupSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                    //        folderType = eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodGeneralAllocationFolder:
                    //        folderType = eProfileType.MethodGeneralAllocationSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodAllocationOverrideFolder:
                    //        folderType = eProfileType.MethodAllocationOverrideSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodRuleFolder:
                    //        folderType = eProfileType.MethodRuleSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodVelocityFolder:
                    //        folderType = eProfileType.MethodVelocitySubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodSizeNeedFolder:
                    //        folderType = eProfileType.MethodSizeNeedSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodFillSizeHolesFolder:
                    //        folderType = eProfileType.MethodFillSizeHolesSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    case eProfileType.MethodBasisSizeFolder:
                    //        folderType = eProfileType.MethodBasisSizeSubFolder;
                    //        folderRID = folderNode.GetGroupKey();
                    //        break;
                    //    //case eProfileType.MethodWarehouseSizeAllocation:
                    //    //folderType = eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
                    //    //break;
                    //    default:
                    //        folderType = eProfileType.WorkflowMethodSubFolder;
                    //        folderRID = aNode.Profile.Key;
                    //        break;
                    //}
                }

                newNodeName = FindNewFolderName("New Folder", aUserId, folderRID, folderType);

                DlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aUserId, folderType, newNodeName, aUserId);
                    newFolderProf.Key = DlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    DlFolder.Folder_Item_Insert(folderRID, newFolderProf.Key, folderType);

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

                try
                {
                    // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                    //if (newFolderProf.UserRID == Include.GlobalUserRID)
                    //{
                    //    functionSecurity = _globalFolders;
                    //}
                    //else
                    //{
                    //    functionSecurity = _userFolders;
                    //}
                    if (newFolderProf.UserRID == Include.GlobalUserRID)
                    {
                        functionSecurity = _globalFolders;
                    }
                    else if (aNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                    {
                        functionSecurity = FavoritesSecGrp.FolderSecurityProfile;
                    }
                    else
                    {
                        functionSecurity = _userFolders;
                    }
                    // End TT#42
                    newNode = BuildFolderNode(
                        newFolderProf.Key,
                        (MIDWorkflowMethodTreeNode)aNode,
                        newFolderProf.UserRID,
                        newFolderProf.ProfileType,
                        newFolderProf.Name,
                        functionSecurity,
                        newFolderProf.OwnerUserRID,
                        eTreeNodeType.SubFolderNode,
                        false);

                    _folderNodeHash[newFolderProf.Key] = newNode;
                    aNode.Nodes.Insert(0, newNode);
                    //RebuildShortcuts(_favoritesNode, aNode);
                    aNode.Expand();
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CreateNewOTSForecastGroup (MIDWorkflowMethodTreeNode aNode)
        {
            MIDWorkflowMethodTreeNode groupNode;
            string newNodeName;
            FolderProfile newFolderProf;

            try
            {
                newNodeName = FindNewFolderName("OTS Forecast", aNode.UserId, aNode.Profile.Key, eProfileType.WorkflowMethodOTSForcastFolder);

                DlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aNode.UserId, eProfileType.WorkflowMethodOTSForcastFolder, newNodeName, aNode.UserId);
                    newFolderProf.Key = DlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    DlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.WorkflowMethodOTSForcastFolder);

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

                groupNode = BuildOTSForecastGroup(aNode, newFolderProf.Key, aNode.UserId, aNode.OwnerUserRID, false);

                aNode.Nodes.Add(groupNode);
            }
            catch
            {
                throw;
            }
        }

        public void CreateNewAllocationGroup(MIDWorkflowMethodTreeNode aNode)
        {
            MIDWorkflowMethodTreeNode groupNode;
            string newNodeName;
            FolderProfile newFolderProf;

            try
            {
                newNodeName = FindNewFolderName("Allocation", aNode.UserId, aNode.Profile.Key, eProfileType.WorkflowMethodAllocationFolder);

                DlFolder.OpenUpdateConnection();

                try
                {
                    newFolderProf = new FolderProfile(Include.NoRID, aNode.UserId, eProfileType.WorkflowMethodAllocationFolder, newNodeName, aNode.UserId);
                    newFolderProf.Key = DlFolder.Folder_Create(newFolderProf.UserRID, newFolderProf.Name, newFolderProf.FolderType);
                    DlFolder.Folder_Item_Insert(aNode.Profile.Key, newFolderProf.Key, eProfileType.WorkflowMethodAllocationFolder);

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

                groupNode = BuildAllocationGroup(aNode, newFolderProf.Key, aNode.UserId, aNode.OwnerUserRID, false);

                aNode.Nodes.Add(groupNode);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Virtual method used to edit a MIDTreeNode
        /// </summary>
        /// <param name="aNode">
        /// The MIDTreeNode being edited
        /// </param>

        override protected void EditNode(MIDTreeNode aNode)
        {
            bool newForm;
            try
            {
                WorkflowMethodFormBase childForm = null;
                newForm = true;

                if (SelectedNode != null &&
                    ((MIDWorkflowMethodTreeNode)SelectedNode).isMethod ||
                    ((MIDWorkflowMethodTreeNode)SelectedNode).isWorkflow)
                {
                    if (SelectedNode.FunctionSecurityProfile.AllowUpdate ||
                        SelectedNode.FunctionSecurityProfile.AllowView)
                    {
                        childForm = GetForm((MIDWorkflowMethodTreeNode)aNode, ref newForm);
                        childForm.MdiParent = MDIParentForm;


                        eLockStatus aLockedStatus = eLockStatus.Undefined;
                        if (newForm)
                        {
                            aLockedStatus = eLockStatus.Cancel;
                            childForm.MdiParent = MDIParentForm;
                            childForm.Tag = SelectedNode.Profile.Key;
                            childForm.UpdateWorkflowMethod((MIDWorkflowMethodTreeNode)SelectedNode, out aLockedStatus);
                            childForm.OnPropertyChangeHandler += new WorkflowMethodFormBase.PropertyChangeEventHandler(OnPropertiesChange);
                            if (aLockedStatus == eLockStatus.Cancel)
                            {
                                childForm.MdiParent = null;
                            }
                        }

                        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement

                        //End TT#43 - MD - DOConnell - Projected Sales Enhancement

                        if (aLockedStatus != eLockStatus.Cancel)
                        {
                            childForm.Show();

                            // Checks to see if Method is valid and displays an information message if it's not
                            IsMethodValid((MIDWorkflowMethodTreeNode)SelectedNode);

                            childForm.Focus();
                            if (childForm.WindowState == FormWindowState.Minimized)
                            {
                                childForm.WindowState = FormWindowState.Normal;
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

        public void ProcessNode(MIDWorkflowMethodTreeNode aNode)
        {
            WorkflowMethodFormBase form;
            bool newForm;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
				// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
				//bool isProcessingInAssortment = false;
				//int assrtRid = Include.NoRID;
				//if (Enum.IsDefined(typeof(eNoHeaderMethodType), (eNoHeaderMethodType)aNode.MethodType))
				//{
				//}
				//else
				//{
				//    AssortmentView av = GetAssortmentViewWindow();
				//    if (av != null)
				//    {
				//        SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
				//        if (selectedHeaderList.Count > 0)
				//        {
				//            SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];
				//            if (shp.AsrtRID > 0)
				//            {
				//                assrtRid = shp.AsrtRID;
				//                isProcessingInAssortment = true;
				//            }
				//        }
				//    }
				//}

				//if (isProcessingInAssortment)
				//{
					
				//    SAB.ProcessMethodOnAssortmentEvent.ProcessMethod(this, assrtRid, (eMethodType)aNode.MethodType, aNode.Key);
					

				//}
				//else
				{
					newForm = true;
					form = GetForm(aNode, ref newForm);

					if (newForm)
					{
						form.Tag = SelectedNode.Profile.Key;
						form.ProcessWorkflowMethod(aNode);
					}
					else
					{
						form.ProcessWorkflowMethod(aNode);
						form.Close();
					}

				}
				// END TT#217-MD - stodd - unable to run workflow methods against assortment

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

		// BEGIN TT#217-MD - stodd - unable to run workflow methods against assortment
		private AssortmentView GetAssortmentViewWindow()
		{
			try
			{
				foreach (Form childForm in _EAB.WorkflowMethodExplorer.MainMDIForm.MdiChildren)
				{
					if (childForm.GetType() == typeof(AssortmentView))
					{
						return (AssortmentView)childForm;
					}
				}

				return null;
			}
			catch (Exception error)
			{
				string message = error.ToString();
				throw;
			}
		}
		// END TT#217-MD - stodd - unable to run workflow methods against assortment
        public bool RenameNode(MIDWorkflowMethodTreeNode aNode, string aNewName)
        {
            WorkflowMethodFormBase childForm;
            bool newForm;
            bool renameSuccessful;

            try
            {
                newForm = true;
                renameSuccessful = false;
                childForm = GetForm(aNode, ref newForm);

                if (newForm)
                {
                    childForm.Tag = SelectedNode.Profile.Key;
                    renameSuccessful = childForm.RenameWorkflowMethod(aNode, aNewName);
                }
                else
                {
                    renameSuccessful = childForm.RenameWorkflowMethod(aNode, aNewName);
                    childForm.Close();
                }

                return renameSuccessful;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks to see if Method is valid and displays an information message if it's not
        /// </summary>
        /// <param name="_selectedNode"></param>
        /// <returns></returns>
        private bool IsMethodValid(MIDWorkflowMethodTreeNode aNode)
        {
            bool isValid = true;

            switch (aNode.MethodType)
            {
                case eMethodTypeUI.OTSPlan:
                case eMethodTypeUI.ForecastBalance:
                case eMethodTypeUI.ForecastSpread:
                case eMethodTypeUI.GlobalUnlock:
                case eMethodTypeUI.Rollup:
                case eMethodTypeUI.ForecastModifySales:
                case eMethodTypeUI.GeneralAllocation:
                case eMethodTypeUI.AllocationOverride:
                    break;
                case eMethodTypeUI.Rule:
                    RuleMethod rm = new RuleMethod(SAB, aNode.Key);
                    if (rm.MethodStatus == eMethodStatus.InvalidMethod)
                    {
                        string msgText = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MethodInvalidMethodChanged);
                        if (MIDEnvironment.isWindows)
                        {
                            MessageBox.Show(msgText, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MIDEnvironment.Message = msgText;
                            MIDEnvironment.requestFailed = true;
                        }
                        isValid = false;
                    }
                    break;
                case eMethodTypeUI.Velocity:
                case eMethodTypeUI.FillSizeHolesAllocation:
                case eMethodTypeUI.BasisSizeAllocation:
                    break;
                default:
                    break;
            }


            return isValid;
        }

        private MIDWorkflowMethodTreeNode BuildFolderShortcutNode(MIDWorkflowMethodTreeNode aFromNode, MIDWorkflowMethodTreeNode aToNode)
        {
            MIDWorkflowMethodTreeNode newNode;

            try
            {
                newNode = new MIDWorkflowMethodTreeNode(
                    SAB,
                    eTreeNodeType.FolderShortcutNode,
                    aFromNode.Profile,
                    aFromNode.Text,
                    aToNode.Profile.Key,
                    aFromNode.UserId,
                    aFromNode.FunctionSecurityProfile,
                    cClosedShortcutFolderImage,
                    cClosedShortcutFolderImage,
                    cOpenShortcutFolderImage,
                    cOpenShortcutFolderImage,
                    aFromNode.OwnerUserRID);

                // Begin TT#33 - JSmith - No children in folder after copy
                if (aFromNode.HasChildren)
                {
                    newNode.Nodes.Add(BuildPlaceHolderNode());
                    newNode.ChildrenLoaded = false;
                    newNode.HasChildren = true;
                    newNode.DisplayChildren = true;
                }
                else
                {
                    newNode.ChildrenLoaded = true;
                }
                // End TT#33
                
                aToNode.Nodes.Add(newNode);

                //CreateShortcutChildren(aFromNode, newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode BuildObjectShortcutNode(MIDWorkflowMethodTreeNode aFromNode, MIDWorkflowMethodTreeNode aToNode)
        {
            MIDWorkflowMethodTreeNode newNode;

            try
            {
                if (aFromNode.UserId == Include.GlobalUserRID)
                {
                    newNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.ObjectShortcutNode,
                        aFromNode.Profile,
                        aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                        aToNode.Profile.Key,
                        aFromNode.UserId,
                        aFromNode.FunctionSecurityProfile,
                        cGlobalShortcutImage,
                        cGlobalShortcutImage,
                        aFromNode.UserId);
                }
                else
                {
                    newNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.ObjectShortcutNode,
                        aFromNode.Profile,
                        aFromNode.Text + " (" + GetUserName(aFromNode.UserId) + ")",
                        aToNode.Profile.Key,
                        aFromNode.UserId,
                        aFromNode.FunctionSecurityProfile,
                        cUserShortcutImage,
                        cUserShortcutImage,
                        aFromNode.OwnerUserRID);
                }

                aToNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildMethodNode(
            int aMethodRID,
            MIDWorkflowMethodTreeNode aParentNode,
            eMethodType aMethodType, 
            FunctionSecurityProfile aFunctionSecurity, 
            int aUserRID, 
            int aOwnerUserRID,
            int aParentKey,
            bool aIsShortcut)
        {
            eTreeNodeType treeNodeType;
            ApplicationBaseMethod method;
            MIDWorkflowMethodTreeNode node;
            int imageIndex;
            string name;

            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
            DataRow[] methodRows;
            // End TT#1167

            try
            {
                // need to determine type of method
                if (!Enum.IsDefined(typeof(eMethodType), (int)aMethodType) &&
                    isMethodProfileType((int)aMethodType))
                {
                    aMethodType = DlMethodData.GetMethodType(aMethodRID);
                }

                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                //method = (ApplicationBaseMethod)_getMethods.GetMethod(aMethodRID, aMethodType);
                method = (ApplicationBaseMethod)_getMethods.GetMethod(Include.NoRID, aMethodType);
                // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
                //if (!method.AuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
                //{
                //    return null;
                //}
                // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
                method.Key = aMethodRID;
                methodRows = _dtMethods.Select("METHOD_RID = " + aMethodRID);
                if (methodRows.Length > 0)
                {
                    method.Name = methodRows[0]["METHOD_NAME"].ToString();
                    method.User_RID = Convert.ToInt32(methodRows[0]["USER_RID"]);
                }
                // End TT#1167

                if (aParentNode.TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                    aParentNode.TreeNodeType == eTreeNodeType.ChildFolderShortcutNode)
                {
                    treeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                    name = method.Name;
                    //name = method.Name + " (" + GetUserName(method.User_RID) + ")";
                    aIsShortcut = true;
                }
                else if (aIsShortcut)
                {
                    treeNodeType = eTreeNodeType.ObjectShortcutNode;
                    name = method.Name + " (" + GetUserName(method.User_RID) + ")";
                }
                else
                {
                    treeNodeType = eTreeNodeType.ObjectNode;
                    name = method.Name;
                }

                if (aOwnerUserRID == Include.GlobalUserRID)
                {
                    if (aIsShortcut)
                    {
                        imageIndex = cGlobalShortcutImage;
                    }
                    else
                    {
                        imageIndex = cGlobalImage;
                    }
                }
                //else if (aOwnerUserRID != aUserRID)
                //{
                //    imageIndex = cSharedUserImage;
                //}
                else
                {
                    if (aIsShortcut)
                    {
                        imageIndex = cUserShortcutImage;
                    }
                    else
                    {
                        imageIndex = cUserImage;
                    }
                }

                // Begin TT#2014-MD - JSmith - Assortment Security
                if (aIsShortcut
                     && aParentNode.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                {
                    aFunctionSecurity = (FunctionSecurityProfile)aFunctionSecurity.Clone();
                    aFunctionSecurity.SetAllowDelete();
                }
                // End TT#2014-MD - JSmith - Assortment Security

                node = new MIDWorkflowMethodTreeNode(
                SAB,
                treeNodeType,
                method,
                name,
                aParentKey,
                aUserRID,
                aFunctionSecurity,
                imageIndex,
                imageIndex,
                imageIndex,
                imageIndex,
                aOwnerUserRID
                );

                node.NodeChangeType = eChangeType.none;

                node.HasChildren = false;
                node.DisplayChildren = false;

                //if (aOwnerUserRID == Include.GlobalUserRID)
                //{
                //    node.GlobalUserType = eGlobalUserType.Global;
                //}
                //else
                //{
                //    node.GlobalUserType = eGlobalUserType.User;
                //}

                return node;
            }
            catch
            {
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildWorkflowNode(
            int aWorkflowRID,
            MIDWorkflowMethodTreeNode aParentNode,
            eWorkflowType aWorkflowType, 
            FunctionSecurityProfile aFunctionSecurity, 
            int aUserRID, 
            int aOwnerUserRID,
            int aParentKey,
            bool aIsShortcut)
        {
            eTreeNodeType treeNodeType;
            MIDWorkflowMethodTreeNode node;
            ApplicationBaseWorkFlow workflow;
            int imageIndex;
            DataTable dt;
            string name;

            // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
            DataRow[] workflowRows;
            // End TT#1167

            try
            {
                // need to determine type of workflow
                if ((int)aWorkflowType == (int)eProfileType.Workflow)
                {
                    dt = DlWorkflowData.GetWorkflow(aWorkflowRID);
                    if (dt.Rows.Count > 0)
                    {
                        aWorkflowType = (eWorkflowType)(Convert.ToInt32(dt.Rows[0]["WORKFLOW_TYPE_ID"]));
                    }
                    else
                    {
                        workflow = null;
                    }
                }

                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                //if (aWorkflowType == eWorkflowType.Forecast)
                //{
                //    workflow = new OTSPlanWorkFlow(SAB, aWorkflowRID, aUserRID, aUserRID == Include.GlobalUserRID);
                //}
                //else if (aWorkflowType == eWorkflowType.Allocation)
                //{
                //    workflow = new AllocationWorkFlow(SAB, aWorkflowRID, aUserRID, aUserRID == Include.GlobalUserRID);
                //}
                //else
                //{
                //    workflow = null;
                //}

                if (aWorkflowType == eWorkflowType.Forecast)
                {
                    workflow = new OTSPlanWorkFlow(SAB, Include.NoRID, aUserRID, aUserRID == Include.GlobalUserRID);
                }
                else if (aWorkflowType == eWorkflowType.Allocation)
                {
                    workflow = new AllocationWorkFlow(SAB, Include.NoRID, aUserRID, aUserRID == Include.GlobalUserRID);
                }
                else
                {
                    workflow = null;
                }

                workflow.Key = aWorkflowRID;
                workflowRows = _dtWorkflows.Select("WORKFLOW_RID = " + aWorkflowRID);
                if (workflowRows.Length > 0)
                {
                    workflow.WorkFlowName = workflowRows[0]["WORKFLOW_NAME"].ToString();
                    workflow.UserRID = Convert.ToInt32(workflowRows[0]["WORKFLOW_USER"]);
                }
                // End TT#1167

                if (aParentNode.TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                    aParentNode.TreeNodeType == eTreeNodeType.ChildFolderShortcutNode)
                {
                    treeNodeType = eTreeNodeType.ChildObjectShortcutNode;
                    //name = workflow.WorkFlowName + " (" + GetUserName(workflow.UserRID) + ")";
                    name = workflow.WorkFlowName;
                    aIsShortcut = true;
                }
                else if (aIsShortcut)
                {
                    treeNodeType = eTreeNodeType.ObjectShortcutNode;
                    name = workflow.WorkFlowName + " (" + GetUserName(workflow.UserRID) + ")";
                }
                else
                {
                    treeNodeType = eTreeNodeType.ObjectNode;
                    name = workflow.WorkFlowName;
                }
                
                if (aOwnerUserRID == Include.GlobalUserRID)
                {
                    if (aIsShortcut)
                    {
                        imageIndex = cGlobalShortcutImage;
                    }
                    else
                    {
                        imageIndex = cGlobalImage;
                    }
                }
                //else if (aOwnerUserRID != aUserRID)
                //{
                //    imageIndex = cSharedUserImage;
                //}
                else
                {
                    if (aIsShortcut)
                    {
                        imageIndex = cUserShortcutImage;
                    }
                    else
                    {
                        imageIndex = cUserImage;
                    }
                }

                node = new MIDWorkflowMethodTreeNode(
                    SAB,
                    treeNodeType,
                    workflow,
                    name,
                    aParentKey,
                    aUserRID,
                    aFunctionSecurity,
                    imageIndex,
                    imageIndex,
                    imageIndex,
                    imageIndex,
                    aOwnerUserRID
                    );


                node.NodeChangeType = eChangeType.none;
                //node.WorkflowType = aWorkflowType;

                node.HasChildren = false;
                node.DisplayChildren = false;

                //if (aOwnerUserRID == Include.GlobalUserRID)
                //{
                //    node.GlobalUserType = eGlobalUserType.Global;
                //}
                //else
                //{
                //    node.GlobalUserType = eGlobalUserType.User;
                //}

                return node;
            }
            catch
            {
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildFolderNode(
            int aKey, 
            MIDWorkflowMethodTreeNode aParentNode, 
            int aUserRID, 
            eProfileType aFolderType,
            string aFolderName, 
            FunctionSecurityProfile aFunctionSecurity, 
            int aOwnerUserRID,
            eTreeNodeType aNodeType, 
            bool aIsShortcut)
        {
            MIDWorkflowMethodTreeNode node;
            FolderProfile folderProf;
            string name;
            int closedImageIndex;
            int openImageIndex;

            try
            {
                // override the node type for shortcuts
                if (aIsShortcut)
                {
                    if (aParentNode.TreeNodeType == eTreeNodeType.FolderShortcutNode ||
                        aParentNode.TreeNodeType == eTreeNodeType.ChildFolderShortcutNode)
                    {
                        aNodeType = eTreeNodeType.ChildFolderShortcutNode;
                    }
                    else
                    {
                        aNodeType = eTreeNodeType.FolderShortcutNode;
                    }
                }

                folderProf = new FolderProfile(aKey, aUserRID, aFolderType, aFolderName, aOwnerUserRID);

                //if (aNodeType == eTreeNodeType.FolderShortcutNode ||
                //    aNodeType == eTreeNodeType.ChildShortcutNode)
                if (aNodeType == eTreeNodeType.FolderShortcutNode)
                {
                    name = folderProf.Name + " (" + GetUserName(folderProf.OwnerUserRID) + ")";
                }
                else
                {
                    name = folderProf.Name;
                }

                if (aOwnerUserRID == Include.GlobalUserRID ||
                    aIsShortcut)
                {
                    if (aNodeType == eTreeNodeType.FolderShortcutNode ||
                    aNodeType == eTreeNodeType.ChildFolderShortcutNode)
                    {
                        closedImageIndex = cClosedShortcutFolderImage;
                        openImageIndex = cOpenShortcutFolderImage;
                    }
                    else
                    {
                        closedImageIndex = cClosedFolderImage;
                        openImageIndex = cOpenFolderImage;
                    }
                }
                //else if (aOwnerUserRID != aUserRID)
                //{
                //    closedImageIndex = cSharedClosedFolderImage;
                //    openImageIndex = cSharedOpenFolderImage;
                //}
                else
                {
                    closedImageIndex = cClosedFolderImage;
                    openImageIndex = cOpenFolderImage;
                }

                node = new MIDWorkflowMethodTreeNode(
                    SAB,
                    aNodeType,
                    folderProf,
                    name,
                    aParentNode.Profile.Key,
                    folderProf.UserRID,
                    aFunctionSecurity,
                    closedImageIndex,
                    closedImageIndex,
                    openImageIndex,
                    openImageIndex,
                    folderProf.OwnerUserRID);

                node.NodeChangeType = eChangeType.none;

                if (node.isMethodSubFolder ||
                    node.isWorkflowSubFolder)
                {

                    if (DlFolder.Folder_Children_Exists(aUserRID, node.Profile.Key))
                    {
                        node.Nodes.Add(BuildPlaceHolderNode());
                        node.ChildrenLoaded = false;
                        node.HasChildren = true;
                        node.DisplayChildren = true;
                    }
                    else
                    {
                        node.ChildrenLoaded = true;
                    }
                }
                else
                {
                    node.HasChildren = false;
                    node.ChildrenLoaded = true;
                }

                return node;
            }
            catch
            {
                throw;
            }
        }

        private MIDWorkflowMethodTreeNode BuildSubFolderNode(FolderProfile aFolderProf, MIDTreeNode aParentNode, int aSequence)
        {
            MIDWorkflowMethodTreeNode newNode;

            try
            {
                if (aFolderProf.UserRID == Include.GlobalUserRID)
                {
                    newNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        aFolderProf,
                        aFolderProf.Name,
                        aParentNode.NodeRID,
                        aFolderProf.UserRID,
                        _globalFolders,
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        aFolderProf.OwnerUserRID);
                }
                else if (aFolderProf.UserRID == aFolderProf.OwnerUserRID)
                {
                    newNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        aFolderProf,
                        aFolderProf.Name,
                        aParentNode.NodeRID,
                        aFolderProf.UserRID,
                        _userFolders,
                        cClosedFolderImage,
                        cClosedFolderImage,
                        cOpenFolderImage,
                        cOpenFolderImage,
                        aFolderProf.OwnerUserRID);
                }
                else
                {
                    newNode = new MIDWorkflowMethodTreeNode(
                        SAB,
                        eTreeNodeType.SubFolderNode,
                        aFolderProf,
                        aFolderProf.Name,
                        aParentNode.NodeRID,
                        aFolderProf.UserRID,
                        _userFolders,
                        cSharedClosedFolderImage,
                        cSharedClosedFolderImage,
                        cSharedOpenFolderImage,
                        cSharedOpenFolderImage,
                        aFolderProf.OwnerUserRID);
                }

                newNode.Sequence = aSequence;
                FolderNodeHash[aFolderProf.Key] = newNode;
                aParentNode.Nodes.Add(newNode);

                return newNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildPlaceHolderNode()
        {
            MIDWorkflowMethodTreeNode newNode;

            try
            {
                newNode = new MIDWorkflowMethodTreeNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            new HierarchyNodeProfile(Include.NoRID),
                            null,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID,
                            new FunctionSecurityProfile(Include.NoRID),
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID);


                return newNode;
            }
            catch
            {
                throw;
            }
        }

        public FunctionSecurityProfile GetSecurity(int aUserRID, eProfileType aProfileType, eWorkflowType aWorkflowType)
        {
            if (aUserRID == Include.GlobalUserRID)
            { 
                return GetGlobalSecurity(aProfileType, aWorkflowType);
            }
            {
                return GetUserSecurity(aProfileType, aWorkflowType);
            }
        }

        private FunctionSecurityProfile GetGlobalSecurity(eProfileType aProfileType, eWorkflowType aWorkflowType)
        {
            switch (aProfileType)
            {
                case eProfileType.Workflow:
                    if (aWorkflowType == eWorkflowType.Allocation)
                    {
                        return _allocationWorkflowsGlobal;
                    }
                    else
                    {
                        return _forecastWorkflowsGlobal;
                    }
                case eProfileType.MethodOTSPlanFolder:
                case eProfileType.MethodOTSPlan:
                    return _forecastMethodsGlobalOTSPlan;
                case eProfileType.MethodForecastBalanceFolder:
                case eProfileType.MethodForecastBalance:
                    return _forecastMethodsGlobalOTSBalance;
                case eProfileType.MethodModifySalesFolder:
                case eProfileType.MethodModifySales:
                    return _forecastMethodsGlobalModifySales;
                case eProfileType.MethodForecastSpreadFolder:
                case eProfileType.MethodForecastSpread:
                    return _forecastMethodsGlobalSpread;
                case eProfileType.MethodCopyChainForecastFolder:
                case eProfileType.MethodCopyChainForecast:
                    return _forecastMethodsGlobalCopyChain;
                case eProfileType.MethodCopyStoreForecastFolder:
                case eProfileType.MethodCopyStoreForecast:
                    return _forecastMethodsGlobalCopyStore;
                case eProfileType.MethodExportFolder:
                case eProfileType.MethodExport:
                    return _forecastMethodsGlobalExport;
                // Begin TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodPlanningExtractFolder:
                case eProfileType.MethodPlanningExtract:
                    return _forecastMethodsGlobalPlanningExtract;
                // End TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodGlobalUnlockFolder:
                case eProfileType.MethodGlobalUnlock:
                    return _forecastMethodsGlobalGlobalUnlock;
                //BeginTT#43 - MD - DOConnell - Projected Sales Enhancement
                case eProfileType.MethodGlobalLockFolder:
                case eProfileType.MethodGlobalLock:
                    return _forecastMethodsGlobalGlobalLock;
                //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                case eProfileType.MethodRollupFolder:
                case eProfileType.MethodRollup:
                    return _forecastMethodsGlobalRollup;
                case eProfileType.MethodGeneralAllocationFolder:
                case eProfileType.MethodGeneralAllocation:
                    return _allocationMethodsGlobalGeneralAllocation;
                case eProfileType.MethodAllocationOverrideFolder:
                case eProfileType.MethodAllocationOverride:
                    return _allocationMethodsGlobalAllocationOverride;
                case eProfileType.MethodRuleFolder:
                case eProfileType.MethodRule:
                    return _allocationMethodsGlobalRule;
                case eProfileType.MethodVelocityFolder:
                case eProfileType.MethodVelocity:
                    return _allocationMethodsGlobalVelocity;
                case eProfileType.MethodSizeMethodFolder:
                    return _allocationSizeMethodsGlobal;
                case eProfileType.MethodFillSizeHolesFolder:
                case eProfileType.MethodFillSizeHolesAllocation:
                    return _allocationMethodsGlobalFillSizeHoles;
                case eProfileType.MethodBasisSizeFolder:
                case eProfileType.MethodBasisSizeAllocation:
                    return _allocationMethodsGlobalBasisSize;
                case eProfileType.MethodSizeNeedFolder:
                case eProfileType.MethodSizeNeedAllocation:
                    return _allocationMethodsGlobalSizeNeed;
                // Begin TT#155 - JSmith - Size Curve Method
                case eProfileType.MethodSizeCurveFolder:
                case eProfileType.MethodSizeCurve:
                    return _allocationMethodsGlobalSizeCurve;
                // End TT#155
                // Begin TT#370 - APicchetti - Build Packs Method
                case eProfileType.MethodBuildPacksFolder:
                case eProfileType.MethodBuildPacks:
                    return _allocationMethodsGlobalBuildPacks;
                // End TT#370
				// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
				case eProfileType.MethodGroupAllocationFolder:
				case eProfileType.MethodGroupAllocation:
					return _allocationMethodsGlobalGroupAllocation;
				// END TT#708-MD - Stodd - Group Allocation Prototype.
				// Begin TT#1652-MD - stodd - DC Carton Rounding
                case eProfileType.MethodDCCartonRoundingFolder:
                case eProfileType.MethodDCCartonRounding:
                    return _allocationMethodsGlobalDCCartonRounding;
				// End TT#1652-MD - stodd - DC Carton Rounding
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                case eProfileType.MethodCreateMasterHeadersFolder:
                case eProfileType.MethodCreateMasterHeaders:
                    return _allocationMethodsGlobalCreateMasterHeaders;
                case eProfileType.MethodDCFulfillmentFolder:
                case eProfileType.MethodDCFulfillment:
                    return _allocationMethodsGlobalDCFulfillment;
                // End TT#1966-MD - JSmith- DC Fulfillment
                case eProfileType.WorkflowMethodOTSForcastFolder:
                    return _forecastGlobal;
                case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                    return _forecastWorkflowsGlobal;
                case eProfileType.WorkflowMethodOTSForcastMethodsFolder:
                    return _forecastMethodsGlobal;
                case eProfileType.WorkflowMethodAllocationFolder:
                    return _allocationGlobal;
                case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                    return _allocationWorkflowsGlobal;
                case eProfileType.WorkflowMethodAllocationMethodsFolder:
                    return _allocationMethodsGlobal;
                case eProfileType.WorkflowMethodAllocationSizeMethodsFolder:
                    return _allocationSizeMethodsGlobal;
                case eProfileType.WorkflowMethodSubFolder:
                case eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder:
                case eProfileType.MethodOTSPlanSubFolder:
                case eProfileType.MethodForecastBalanceSubFolder:
                case eProfileType.MethodModifySalesSubFolder:
                case eProfileType.MethodForecastSpreadSubFolder:
                case eProfileType.MethodCopyChainForecastSubFolder:
                case eProfileType.MethodCopyStoreForecastSubFolder:
                case eProfileType.MethodExportSubFolder:
                case eProfileType.MethodPlanningExtractSubFolder:  // TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodGlobalUnlockSubFolder:
                case eProfileType.MethodGlobalLockSubFolder:    //TT#43 - MD - DOConnell - Projected Sales Enhancement
                case eProfileType.MethodRollupSubFolder:
                case eProfileType.WorkflowMethodAllocationWorkflowsSubFolder:
                case eProfileType.MethodGeneralAllocationSubFolder:
                case eProfileType.MethodAllocationOverrideSubFolder:
                case eProfileType.MethodRuleSubFolder:
                case eProfileType.MethodVelocitySubFolder:
                case eProfileType.MethodFillSizeHolesSubFolder:
                case eProfileType.MethodBasisSizeSubFolder:
                case eProfileType.MethodSizeNeedSubFolder:
                // Begin TT#155 - JSmith - Size Curve Method
                case eProfileType.MethodSizeCurveSubFolder:
                // End TT#155
                // Begin TT#370 - APicchetti - Build Packs Method
                case eProfileType.MethodBuildPacksSubFolder:
                // End TT#370
				case eProfileType.MethodGroupAllocationSubFolder:		// TT#708-MD - Stodd - Group Allocation Prototype.
                case eProfileType.MethodDCCartonRoundingSubFolder:		// TT#1652-MD - stodd - DC Carton Rounding
                case eProfileType.MethodCreateMasterHeadersSubFolder:		// TT#1966-MD - JSmith- DC Fulfillment
                case eProfileType.MethodDCFulfillmentSubFolder:		// TT#1966-MD - JSmith- DC Fulfillment

                    return _globalFolders;

                default:
                    return new FunctionSecurityProfile(-1);
            }
        }

        private FunctionSecurityProfile GetUserSecurity(eProfileType aProfileType, eWorkflowType aWorkflowType)
        {
            switch (aProfileType)
            {
                case eProfileType.Workflow:
                    if (aWorkflowType == eWorkflowType.Allocation)
                    {
                        return _allocationWorkflowsUser;
                    }
                    else
                    {
                        return _forecastWorkflowsUser;
                    }
                case eProfileType.MethodOTSPlanFolder:
                case eProfileType.MethodOTSPlan:
                    return _forecastMethodsUserOTSPlan;
                case eProfileType.MethodForecastBalanceFolder:
                case eProfileType.MethodForecastBalance:
                    return _forecastMethodsUserOTSBalance;
                case eProfileType.MethodModifySalesFolder:
                case eProfileType.MethodModifySales:
                    return _forecastMethodsUserModifySales;
                case eProfileType.MethodForecastSpreadFolder:
                case eProfileType.MethodForecastSpread:
                    return _forecastMethodsUserSpread;
                case eProfileType.MethodCopyChainForecastFolder:
                case eProfileType.MethodCopyChainForecast:
                    return _forecastMethodsUserCopyChain;
                case eProfileType.MethodCopyStoreForecastFolder:
                case eProfileType.MethodCopyStoreForecast:
                    return _forecastMethodsUserCopyStore;
                case eProfileType.MethodExportFolder:
                case eProfileType.MethodExport:
                    return _forecastMethodsUserExport;
                // Begin TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodPlanningExtractFolder:
                case eProfileType.MethodPlanningExtract:
                    return _forecastMethodsUserPlanningExtract;
                // End TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodGlobalUnlockFolder:
                case eProfileType.MethodGlobalUnlock:
                    return _forecastMethodsUserGlobalUnlock;
                // Begin TT#3471 - JSmith - Incorrect selections on menu.
                case eProfileType.MethodGlobalLock:
                    return _forecastMethodsUserGlobalLock;
                // End TT#3471 - JSmith - Incorrect selections on menu.
                case eProfileType.MethodRollupFolder:
                case eProfileType.MethodRollup:
                    return _forecastMethodsUserRollup;
                case eProfileType.MethodGeneralAllocationFolder:
                case eProfileType.MethodGeneralAllocation:
                    return _allocationMethodsUserGeneralAllocation;
                case eProfileType.MethodAllocationOverrideFolder:
                case eProfileType.MethodAllocationOverride:
                    return _allocationMethodsUserAllocationOverride;
                case eProfileType.MethodRuleFolder:
                case eProfileType.MethodRule:
                    return _allocationMethodsUserRule;
                case eProfileType.MethodVelocityFolder:
                case eProfileType.MethodVelocity:
                    return _allocationMethodsUserVelocity;
                case eProfileType.MethodSizeMethodFolder:
                    return _allocationSizeMethodsUser;
                case eProfileType.MethodFillSizeHolesFolder:
                case eProfileType.MethodFillSizeHolesAllocation:
                    return _allocationMethodsUserFillSizeHoles;
                case eProfileType.MethodBasisSizeFolder:
                case eProfileType.MethodBasisSizeAllocation:
                    return _allocationMethodsUserBasisSize;
                case eProfileType.MethodSizeNeedFolder:
                case eProfileType.MethodSizeNeedAllocation:
                    return _allocationMethodsUserSizeNeed;
                // Begin TT#155 - JSmith - Size Curve Method
                case eProfileType.MethodSizeCurveFolder:
                case eProfileType.MethodSizeCurve:
                    return _allocationMethodsUserSizeCurve;
                // End TT#155
                
                // Begin TT#370 - APicchetti - Build Packs Method
                case eProfileType.MethodBuildPacksFolder:
                case eProfileType.MethodBuildPacks:
                    return _allocationMethodsUserBuildPacks;
                // End TT#370

				// Begin TT#1136-md - stodd - cannot create a "user" group allocation method - 
                case eProfileType.MethodGroupAllocationFolder:
                case eProfileType.MethodGroupAllocation:
                    return _allocationMethodsUserGroupAllocation;
				// End TT#1136-md - stodd - cannot create a "user" group allocation method - 

				// Begin TT#1652-MD - stodd - DC Carton Rounding
                case eProfileType.MethodDCCartonRoundingFolder:
                case eProfileType.MethodDCCartonRounding:
                    return _allocationMethodsUserDCCartonRounding;
				// End TT#1652-MD - stodd - DC Carton Rounding
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                case eProfileType.MethodCreateMasterHeadersFolder:
                case eProfileType.MethodCreateMasterHeaders:
                    return _allocationMethodsUserCreateMasterHeaders;
                case eProfileType.MethodDCFulfillmentFolder:
                case eProfileType.MethodDCFulfillment:
                    return _allocationMethodsUserDCFulfillment;
                // End TT#1966-MD - JSmith- DC Fulfillment
				
                case eProfileType.WorkflowMethodOTSForcastFolder:
                    return _forecastUser;
                case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                    return _forecastWorkflowsUser;
                case eProfileType.WorkflowMethodOTSForcastMethodsFolder:
                    return _forecastMethodsUser;
                case eProfileType.WorkflowMethodAllocationFolder:
                    return _allocationUser;
                case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                    return _allocationWorkflowsUser;
                case eProfileType.WorkflowMethodAllocationMethodsFolder:
                    return _allocationMethodsUser;
                case eProfileType.WorkflowMethodAllocationSizeMethodsFolder:
                    return _allocationSizeMethodsUser;
                case eProfileType.WorkflowMethodSubFolder:
                case eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder:
                case eProfileType.MethodOTSPlanSubFolder:
                case eProfileType.MethodForecastBalanceSubFolder:
                case eProfileType.MethodModifySalesSubFolder:
                case eProfileType.MethodForecastSpreadSubFolder:
                case eProfileType.MethodCopyChainForecastSubFolder:
                case eProfileType.MethodCopyStoreForecastSubFolder:
                case eProfileType.MethodExportSubFolder:
                case eProfileType.MethodPlanningExtractSubFolder:  // TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodGlobalUnlockSubFolder:
                case eProfileType.MethodRollupSubFolder:
                case eProfileType.WorkflowMethodAllocationWorkflowsSubFolder:
                case eProfileType.MethodGeneralAllocationSubFolder:
                case eProfileType.MethodAllocationOverrideSubFolder:
                case eProfileType.MethodRuleSubFolder:
                case eProfileType.MethodVelocitySubFolder:
                case eProfileType.MethodFillSizeHolesSubFolder:
                case eProfileType.MethodBasisSizeSubFolder:
                case eProfileType.MethodSizeNeedSubFolder:
                // Begin TT#155 - JSmith - Size Curve Method
                case eProfileType.MethodSizeCurveSubFolder:
                // End TT#155
                
                // Begin TT#370 - APicchetti - Build Packs Method
                case eProfileType.MethodBuildPacksSubFolder:
                // End TT#370
                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                // Begin TT#3471 - JSmith - Incorrect selections on menu.
                //case eProfileType.MethodGlobalLock:
                //    return _forecastMethodsUserGlobalLock;
                // ENd TT#3471 - JSmith - Incorrect selections on menu.
                case eProfileType.MethodGlobalLockFolder:
                case eProfileType.MethodGlobalLockSubFolder:
                //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    return _userFolders;
                default:
                    return new FunctionSecurityProfile(-1);
            }
        }

        public string FindNewMethodName(string aMethodName, int aUserRID)
        {
            int index;
            string newName;
            int key;

            try
            {
                index = 0;
                newName = aMethodName;
                key = DlMethodData.Method_GetKey(aUserRID, newName);

                while (key != -1)
                {
                    index++;

                    //if (index > 1)
                    //{
                    //    newName = "Copy (" + index + ") of " + aMethodName;
                    //}
                    //else
                    //{
                    //    newName = "Copy of " + aMethodName;
                    //}
                    newName = Include.GetNewName(name: aMethodName, index: index);

                    key = DlMethodData.Method_GetKey(aUserRID, newName);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public string FindNewWorkflowName(string aWorkflowName, int aUserRID)
        {
            int index;
            string newName;
            int key;

            try
            {
                index = 0;
                newName = aWorkflowName;
                key = DlWorkflowData.Workflow_GetKey(aUserRID, newName);

                while (key != -1)
                {
                    index++;

                    //if (index > 1)
                    //{
                    //    newName = "Copy (" + index + ") of " + aWorkflowName;
                    //}
                    //else
                    //{
                    //    newName = "Copy of " + aWorkflowName;
                    //}
                    newName = Include.GetNewName(name: aWorkflowName, index: index);

                    key = DlWorkflowData.Workflow_GetKey(aUserRID, newName);
                }

                return newName;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        //public WorkflowMethodFormBase GetInUseMethType(MIDWorkflowMethodTreeNode aNode)
        //{
        //    try
        //    {
        //        if (aNode.isMethod ||
        //            aNode.isMethodFolder ||
        //            aNode.isMethodSubFolder)
        //        {
        //            switch (aNode.MethodType)
        //            {
        //                case eMethodTypeUI.OTSPlan:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.ForecastBalance:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.ForecastSpread:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.GlobalUnlock:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.GlobalLock:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.Rollup:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.ForecastModifySales:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.CopyChainForecast:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.CopyStoreForecast:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.Export:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.GeneralAllocation:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.AllocationOverride:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.Rule:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.Velocity:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.FillSizeHolesAllocation:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.SizeNeedAllocation:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.BasisSizeAllocation:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.GeneralAssortment:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.SizeCurve:
        //                    return GetInUseMethType(aNode);
        //                case eMethodTypeUI.BuildPacks:
        //                    return GetInUseMethType(aNode);
        //                default:
        //                    return null;
        //            }
        //        }
        //        else if (aNode.isWorkflow ||
        //            aNode.isWorkflowFolder ||
        //            aNode.isWorkflowSubFolder)
        //        {
        //            switch (aNode.WorkflowType)
        //            {
        //                case eWorkflowType.Allocation:
        //                    return GetInUseMethType(aNode);
        //                case eWorkflowType.Forecast:
        //                    return GetInUseMethType(aNode);
        //            }
        //        }
        //        return null;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //END TT#110-MD-VStuart - In Use Tool

        public WorkflowMethodFormBase GetForm(MIDWorkflowMethodTreeNode aNode, ref bool aNewForm)
        {
            try
            {
                object[] args = null;

                if (aNode.isMethod ||
                    aNode.isMethodFolder ||
                    aNode.isMethodSubFolder)
                {
                    switch (aNode.MethodType)
                    {
                        case eMethodTypeUI.OTSPlan:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmOTSPlanMethod), args, ref aNewForm);
                        case eMethodTypeUI.ForecastBalance:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmMatrixMethod), args, ref aNewForm);
                        case eMethodTypeUI.ForecastSpread:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmForecastSpreadMethod), args, ref aNewForm);
                        case eMethodTypeUI.GlobalUnlock:
                            //Arguments to pass to the constructor of the Maint. form.
							//Begin TT#2576 - DOConnell - Global Lock Create - Title bar name changes
							//args = new object[] { SAB, _EAB };					
                            args = new object[] { SAB, _EAB, eMethodType.GlobalUnlock };
							//End TT#2576 - DOConnell - Global Lock Create - Title bar name changes
                            return GetForm(aNode, typeof(frmGlobalUnlockMethod), args, ref aNewForm);
                        //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                        case eMethodTypeUI.GlobalLock:
                            //Arguments to pass to the constructor of the Maint. form.	
							//Begin TT#2576 - DOConnell - Global Lock Create - Title bar name changes
							//args = new object[] { SAB, _EAB };					
                            args = new object[] { SAB, _EAB, eMethodType.GlobalLock };
							//End TT#2576 - DOConnell - Global Lock Create - Title bar name changes
                            return GetForm(aNode, typeof(frmGlobalUnlockMethod), args, ref aNewForm);
                        //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                        case eMethodTypeUI.Rollup:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmRollupMethod), args, ref aNewForm);
                        case eMethodTypeUI.ForecastModifySales:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmModifySalesMethod), args, ref aNewForm);
                        case eMethodTypeUI.CopyChainForecast:
                            //Arguments to pass to the constructor of the Maint. form.						
							//Begin TT#523 - JScott - Duplicate folder when new folder added
							//args = new object[] { SAB, _EAB, eMethodType.CopyChainForecast };
							args = new object[] { SAB, _EAB, eMethodType.CopyChainForecast, eProfileType.MethodCopyChainForecast };
							//End TT#523 - JScott - Duplicate folder when new folder added
							return GetForm(aNode, typeof(frmCopyForecastMethod), args, ref aNewForm);
                        case eMethodTypeUI.CopyStoreForecast:
                            //Arguments to pass to the constructor of the Maint. form.						
							//Begin TT#523 - JScott - Duplicate folder when new folder added
							//args = new object[] { SAB, _EAB, eMethodType.CopyStoreForecast };
							args = new object[] { SAB, _EAB, eMethodType.CopyStoreForecast, eProfileType.MethodCopyStoreForecast };
							//End TT#523 - JScott - Duplicate folder when new folder added
							return GetForm(aNode, typeof(frmCopyForecastMethod), args, ref aNewForm);
                        case eMethodTypeUI.Export:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmExportMethod), args, ref aNewForm);
                        // Begin TT#2131-MD - JSmith - Halo Integration
                        case eMethodTypeUI.PlanningExtract:
                            //Arguments to pass to the constructor of the Maint. form.						
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmPlanningExtractMethod), args, ref aNewForm);
                        // End TT#2131-MD - JSmith - Halo Integration
                        case eMethodTypeUI.GeneralAllocation:
                            //args  =  new object[]{SAB, this};
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(GeneralAllocationMethod), args, ref aNewForm);
                        case eMethodTypeUI.AllocationOverride:
                            //args  =  new object[]{SAB, this};
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmOverrideMethod), args, ref aNewForm);
						// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
						case eMethodTypeUI.GroupAllocation:
							//args  =  new object[]{SAB, this};
							args = new object[] { SAB, _EAB };
							return GetForm(aNode, typeof(frmGroupAllocationMethod), args, ref aNewForm);
						// END TT#708-MD - Stodd - Group Allocation Prototype.
						// Begin TT#1652-MD - stodd - DC Carton Rounding
                        case eMethodTypeUI.DCCartonRounding:
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmDCCartonRoundingMethod), args, ref aNewForm);
						// End TT#1652-MD - stodd - DC Carton Rounding
                        // Begin TT#1966-MD - JSmith- DC Fulfillment
                        case eMethodTypeUI.CreateMasterHeaders:
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmCreateMasterHeadersMethod), args, ref aNewForm);
                        case eMethodTypeUI.DCFulfillment:
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmDCFulfillmentMethod), args, ref aNewForm);
                        // End TT#1966-MD - JSmith- DC Fulfillment
                        case eMethodTypeUI.Rule:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmRuleMethod), args, ref aNewForm);
                        case eMethodTypeUI.Velocity:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmVelocityMethod), args, ref aNewForm);
                        case eMethodTypeUI.FillSizeHolesAllocation:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmFillSizeHolesMethod), args, ref aNewForm);
                        case eMethodTypeUI.SizeNeedAllocation:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmSizeNeedMethod), args, ref aNewForm);
                        case eMethodTypeUI.BasisSizeAllocation:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmBasisSizeMethod), args, ref aNewForm);
                        case eMethodTypeUI.GeneralAssortment:
                        //case eProfileType.MethodGeneralFolder:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmAssortmentProperties), args, ref aNewForm);
                        // Begin TT#155 - JSmith - Size Curve Method
                        case eMethodTypeUI.SizeCurve:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmSizeCurveMethod), args, ref aNewForm);
                        // End TT#155

                        // Begin TT#370 - APicchetti - Build Packs Method
                        case eMethodTypeUI.BuildPacks:
                            //Create arguments to be passed into maint form.
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmBuildPacksMethod), args, ref aNewForm);
                        // End TT#370



                        default:
                            return null;
                    }
                }
                else if (aNode.isWorkflow ||
                    aNode.isWorkflowFolder ||
                    aNode.isWorkflowSubFolder)
                {
                    //Create arguments to be passed into maint form.
                    switch (aNode.WorkflowType)
                    {
                        case eWorkflowType.Allocation:
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmAllocationWorkflow), args, ref aNewForm);
                        case eWorkflowType.Forecast:
                            args = new object[] { SAB, _EAB };
                            return GetForm(aNode, typeof(frmForecastWorkflow), args, ref aNewForm);
                    }
                }

                return null;
            }
            catch
            {
                throw;
            }
            //finally
            //{
            //    Cursor.Current = Cursors.Default;
            //}
        }

        private WorkflowMethodFormBase GetForm(MIDWorkflowMethodTreeNode aNode, Type childFormType, object[] args, ref bool aNewForm)
        {
            bool nodeFound = false;
            try
            {
                WorkflowMethodFormBase childForm = null;

                if (aNode != null)
                {
                    if (!aNode.FunctionSecurityProfile.AccessDenied)
                    {
                        nodeFound = false;

                        if (this.MDIParentForm != null
                            && this.MDIParentForm.MdiChildren != null)
                        {
                            foreach (Form frm in this.MDIParentForm.MdiChildren)
                            {
                                if (frm.GetType().Equals(childFormType))
                                {
                                    childForm = (WorkflowMethodFormBase)frm;
                                    if (Convert.ToInt32(childForm.Tag, CultureInfo.CurrentUICulture) == aNode.Profile.Key)
                                    {
                                        nodeFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    try
                    {
                        if (!nodeFound)
                        {
                            childForm = (WorkflowMethodFormBase)Activator.CreateInstance(childFormType, args);
                            aNewForm = true;
                            childForm.Tag = aNode;
                        }
                        else
                        {
                            aNewForm = false;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }

                return childForm;
            }
            catch
            {
                throw;
            }
        }

        public void OnPropertiesChange(object source, PropertyChangeEventArgs e)
        {
            ApplicationBaseMethod abm = null;
            ApplicationBaseWorkFlow abw = null;
            Profile p = null;
            MIDRetail.Business.OTSPlanMethod mp = null;
            MIDWorkflowMethodTreeNode wmTreeNode;
            MIDWorkflowMethodTreeNode parentTreeNode;

            try
            {
                if (e.ABW != null) // workflow
                {
                    abw = e.ABW;
                }
                else if (e.ABM == null)	// profile
                {
                    p = e.p;
                }
                else	// method
                {
                    abm = e.ABM;
                }
                BeginUpdate();

                if (abw != null)	// workflow
                {
                    switch (abw.Workflow_Change_Type)
                    {
                        case eChangeType.update:
                            {
                                wmTreeNode = (MIDWorkflowMethodTreeNode)FindTreeNode(Nodes, abw.ProfileType, abw.Key);
								//Begin Track #6201 - JScott - Store Count removed from attr sets
								//wmTreeNode.Text = abw.WorkFlowName;
								wmTreeNode.InternalText = abw.WorkFlowName;
								//End Track #6201 - JScott - Store Count removed from attr sets
								SortChildNodes((MIDWorkflowMethodTreeNode)wmTreeNode.Parent);
                                break;
                            }
                        case eChangeType.add:
                            {
                                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                                AddWorkflowToList(abw.Key, abw.WorkFlowName, abw.UserRID);
                                // End TT#1167

                                if (!e.ExplorerNode.ChildrenLoaded && e.ExplorerNode.HasChildren) //  add children
                                {
                                    e.ExplorerNode.Expand();
                                }
                                wmTreeNode = BuildWorkflowNode(abw.Key, (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, e.ExplorerNode.WorkflowType, e.ExplorerNode.FunctionSecurityProfile, abw.UserRID, abw.UserRID, e.ExplorerNode.Profile.Key, false);

                                if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode;
                                }
                                else if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent;
                                }
                                else
                                {
                                    parentTreeNode = LocateParent(wmTreeNode);
                                }

                                if (e.ExplorerNode.isSubFolder)
                                {
                                    Folder_Item_Insert(parentTreeNode.Profile.Key, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                }
                                else
                                {
                                    //TT#1102 - Method moves out of Folder after a Refresh - APicchetti - 2/4/2011
                                    if (e.ExplorerNode.GetParentNode().isSubFolder == false)
                                    {
                                        Folder_Item_Insert(parentTreeNode.GetGroupKey(), wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                    }
                                    else
                                    {
                                        Folder_Item_Insert(e.ExplorerNode.GetParentNode().NodeRID, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                    }
                                }

                                //e.ExplorerNode.Nodes.Add(wmTreeNode);
                                //SortChildNodes((MIDWorkflowMethodTreeNode)e.ExplorerNode);
                                parentTreeNode.Nodes.Add(wmTreeNode);
                                SortChildNodes(parentTreeNode);

                                break;
                            }
                    }
                }
                else if (p == null)
                {
                    switch (abm.Method_Change_Type)
                    {
                        case eChangeType.update:
                            {
                                wmTreeNode = (MIDWorkflowMethodTreeNode)FindTreeNode(Nodes, abm.ProfileType, abm.Key);
								//Begin Track #6201 - JScott - Store Count removed from attr sets
								//wmTreeNode.Text = abm.Name;
								wmTreeNode.InternalText = abm.Name;
								//End Track #6201 - JScott - Store Count removed from attr sets
								SortChildNodes((MIDWorkflowMethodTreeNode)wmTreeNode.Parent);
                                break;
                            }
                        case eChangeType.add:
                            {
                                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                                AddMethodToList(abm.Key, abm.Name, abm.User_RID);
                                // End TT#1167

                                if (!e.ExplorerNode.ChildrenLoaded && e.ExplorerNode.HasChildren) //  add children
                                {
                                    e.ExplorerNode.Expand();
                                }

                                wmTreeNode = BuildMethodNode(abm.Key, (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, (eMethodType)e.ExplorerNode.MethodType, e.ExplorerNode.FunctionSecurityProfile, abm.User_RID, abm.User_RID, e.ExplorerNode.Profile.Key, false);
                                
                                if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode;
                                }
                                else if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent;
                                }
                                else
                                {
                                    parentTreeNode = LocateParent(wmTreeNode);
                                }

                                if (e.ExplorerNode.isSubFolder)
                                {
                                    Folder_Item_Insert(parentTreeNode.Profile.Key, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                }
                                else
                                {
                                    //TT#1102 - Method moves out of Folder after a Refresh - APicchetti - 2/4/2011
                                    if (e.ExplorerNode.GetParentNode().isSubFolder == false)
                                    {
                                        Folder_Item_Insert(parentTreeNode.GetGroupKey(), wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                    }
                                    else
                                    {
                                        //BEGIN TT#37 - MD- DOConnell - Modify Sales method did a save as of a method from global to user and had to do a refresh for it to appear in the workflow method explorer.
                                        if (e.ExplorerNode.UserId == wmTreeNode.UserId)
                                        {
                                            Folder_Item_Insert(parentTreeNode.Key, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                        }
                                        else
                                        {
                                            // Begin TT#1983 - JSmith - Cannont save global methods in user methods
                                            //Folder_Item_Insert(e.ExplorerNode.GetParentNode().NodeRID, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                            Folder_Item_Insert(parentTreeNode.GetGroupKey(), wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                            // End TT#1983
                                        }
                                        //END TT#37 - MD- DOConnell - Modify Sales method did a save as of a method from global to user and had to do a refresh for it to appear in the workflow method explorer.
                                    }
                                }
                                
                                parentTreeNode.Nodes.Add(wmTreeNode);
                                SortChildNodes(parentTreeNode);

                                break;
                            }
                    }
                }
                else
                {
                    switch (p.ProfileType)
                    {
                        case eProfileType.MethodOTSPlan:
                            mp = (MIDRetail.Business.OTSPlanMethod)e.p;
                            break;
                    }
                    switch (mp.Method_Change_Type)
                    {
                        case eChangeType.update:
                            {
								//Begin Track #6201 - JScott - Store Count removed from attr sets
								//e.ExplorerNode.Text = mp.Name;
								e.ExplorerNode.InternalText = mp.Name;
								//End Track #6201 - JScott - Store Count removed from attr sets
								SortChildNodes((MIDWorkflowMethodTreeNode)e.ExplorerNode);
                                break;
                            }
                        case eChangeType.add:
                            {
                                // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
                                AddMethodToList(mp.Key, mp.Name, mp.User_RID);
                                // End TT#1167

                                if (!e.ExplorerNode.ChildrenLoaded && e.ExplorerNode.HasChildren) //  add children
                                {
                                    e.ExplorerNode.Expand();
                                }

                                wmTreeNode = BuildMethodNode(mp.Key, (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, (eMethodType)e.ExplorerNode.MethodType, e.ExplorerNode.FunctionSecurityProfile, mp.User_RID, mp.User_RID, e.ExplorerNode.Profile.Key, false);

                                if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode;
                                }
                                else if (CorrectParent((MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent, wmTreeNode))
                                {
                                    parentTreeNode = (MIDWorkflowMethodTreeNode)e.ExplorerNode.Parent;
                                }
                                else 
                                {
                                    parentTreeNode = LocateParent(wmTreeNode);
                                }

                                if (e.ExplorerNode.isSubFolder)
                                {
                                    Folder_Item_Insert(parentTreeNode.Profile.Key, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                }
                                else
                                {
                                    //TT#1102 - Method moves out of Folder after a Refresh - APicchetti - 2/4/2011
                                    if (e.ExplorerNode.GetParentNode().isSubFolder == false)
                                    {
                                        Folder_Item_Insert(parentTreeNode.GetGroupKey(), wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                    }
                                    else
                                    {
                                        Folder_Item_Insert(e.ExplorerNode.GetParentNode().NodeRID, wmTreeNode.Profile.Key, wmTreeNode.Profile.ProfileType);
                                    }
                                }
                                //e.ExplorerNode.Nodes.Add(wmTreeNode);
                                //SortChildNodes((MIDWorkflowMethodTreeNode)e.ExplorerNode);
                                parentTreeNode.Nodes.Add(wmTreeNode);
                                SortChildNodes(parentTreeNode);

                                break;
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
                EndUpdate();
            }
        }

        // Begin TT#1167 - JSmith - Login Performance - Opening the application after Login
        private void AddWorkflowToList(int aKey, string aName, int aUserRID)
        {
            DataRow newWorkflowRow = _dtWorkflows.NewRow();

            newWorkflowRow["WORKFLOW_RID"] = aKey;
            newWorkflowRow["WORKFLOW_NAME"] = aName;
            newWorkflowRow["WORKFLOW_USER"] = aUserRID;

            _dtWorkflows.Rows.Add(newWorkflowRow);
        }


        private void AddMethodToList(int aKey, string aName, int aUserRID)
        {
            DataRow newMethodRow = _dtMethods.NewRow();

            newMethodRow["METHOD_RID"] = aKey;
            newMethodRow["METHOD_NAME"] = aName;
            newMethodRow["USER_RID"] = aUserRID;

            _dtMethods.Rows.Add(newMethodRow);
        }

        private void AddToChildrenList(MIDWorkflowMethodTreeNode aParent, int aKey, int aItemType, int aUserRID, int aOwnerUserRID, bool aShortcutInd)
        {
            DataTable dtChildren = (DataTable)_htChildren[aParent.GetGroupNode().Profile.Key];
            DataRow newChildRow = dtChildren.NewRow();

            newChildRow["PARENT_FOLDER_RID"] = aParent.Key;
            newChildRow["CHILD_ITEM_RID"] = aKey;
            newChildRow["CHILD_ITEM_TYPE"] = aItemType;
            newChildRow["USER_RID"] = aUserRID;
            newChildRow["OWNER_USER_RID"] = aOwnerUserRID;
            newChildRow["SHORTCUT_IND"] = Include.ConvertBoolToChar(aShortcutInd);

            dtChildren.Rows.Add(newChildRow);
        }
        // End TT#1167

        public MIDWorkflowMethodTreeNode LocateParent(MIDWorkflowMethodTreeNode aNode)
        {
            MIDWorkflowMethodTreeNode findNode = null;
            int folderKey;
            eProfileType profileType;
            string folderName;
            MIDWorkflowMethodTreeNode mainFolder = null;

            try
            {
                foreach (MIDWorkflowMethodTreeNode tn in Nodes)
                {
                    if (findNode == null)
                    {
                        if (aNode.isGlobalItem &&
                            tn.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder)
                        {
                            mainFolder = tn;
                            findNode = SearchPath(tn.Nodes, aNode);
                        }
                        else if (aNode.isUserItem &&
                            tn.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder)
                        {
                            mainFolder = tn;
                            findNode = SearchPath(tn.Nodes, aNode);
                        }
                    }
                }

                if (findNode == null &&
                    mainFolder != null)
                {
                    if (aNode.isAllocationMethod ||
                            aNode.isAllocationWorkflow)
                    {
                        profileType = eProfileType.WorkflowMethodAllocationFolder;
                        folderName = "Allocation";
                    }
                    else
                    {
                        profileType = eProfileType.WorkflowMethodOTSForcastFolder;
                        folderName = "OTS Forecast";
                    }
                    try
                    {
                        folderKey = Folder_Create(aNode.UserId, folderName, profileType);
                        Folder_Item_Insert(mainFolder.Profile.Key, folderKey, profileType);
                        BeginUpdate();
                        if (aNode.isAllocationMethod ||
                            aNode.isAllocationWorkflow)
                        {
                            BuildAllocationGroup(mainFolder, folderKey, aNode.UserId, aNode.OwnerUserRID, false);
                        }
                        else
                        {
                            BuildOTSForecastGroup(mainFolder, folderKey, aNode.UserId, aNode.OwnerUserRID, false);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        EndUpdate();
                    }
                }

                return findNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode SearchPath(TreeNodeCollection aNodes, MIDWorkflowMethodTreeNode aNode)
        {
            MIDWorkflowMethodTreeNode findNode = null;

            try
            {
                foreach (MIDWorkflowMethodTreeNode tn in aNodes)
                {
                    if (CorrectParent(tn, aNode))
                    {
                        return tn;
                    }
                }

                foreach (MIDWorkflowMethodTreeNode tn in aNodes)
                {
                    findNode = SearchPath(tn.Nodes, aNode);

                    if (findNode != null)
                    {
                        return findNode;
                    }
                }

                return findNode;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        private bool CorrectParent(MIDWorkflowMethodTreeNode aParentNode, MIDWorkflowMethodTreeNode aNewNode)
        {
            if (aNewNode.GlobalUserType != aParentNode.GlobalUserType)
            {
                return false;
            }

            //if (!aParentNode.isMethodSubFolder &&
            //    aNewNode.WorkflowMethodIND != aParentNode.WorkflowMethodIND)
            //{
            //    return false;
            //}

            if (aNewNode.MethodType != aParentNode.MethodType)
            {
                return false;
            }

            if (aNewNode.WorkflowType != aParentNode.WorkflowType)
            {
                return false;
            }

            // Begin Track #6403 - JSmith - Save As on Method appears under incorrect node
            if (aParentNode.isWorkflowMethod)
            {
                return false;
            }
            // End Track #6403

            return true;
        }
	}

	/// <summary>
	/// Summary description for MIDWorkflowMethodTreeNode.
	/// </summary>
	public class MIDWorkflowMethodTreeNode : MIDTreeNode
	{
        private bool						_isMethodOfWorkflow;
        //private eWorkflowType				_workflowType;
        private string						_nodeDescription;
        private int                         _sequence;
        private bool _isMethodOrWorkflowResolved = false;  // TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        
        public MIDWorkflowMethodTreeNode()
            : base()
		{
            CommonLoad();	
		}

        public MIDWorkflowMethodTreeNode(
            SessionAddressBlock aSAB,
            eTreeNodeType aTreeNodeType,
            Profile aProfile,
            string aText,
            int aParentId,
            int aUserId,
            FunctionSecurityProfile aFunctionSecurityProfile,
            int aCollapsedImageIndex,
            int aSelectedCollapsedImageIndex,
            int aExpandedImageIndex,
            int aSelectedExpandedImageIndex,
            int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, true, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, new MIDTreeNodeSecurityGroup(aFunctionSecurityProfile, null), true, aText, aParentId, aUserId, aCollapsedImageIndex, aSelectedCollapsedImageIndex, aExpandedImageIndex, aSelectedExpandedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
        {
            CommonLoad();
        }

        public MIDWorkflowMethodTreeNode(
			SessionAddressBlock aSAB,
			eTreeNodeType aTreeNodeType,
			Profile aProfile,
			string aText,
			int aParentId,
			int aUserId,
			FunctionSecurityProfile aFunctionSecurityProfile,
			int aImageIndex,
			int aSelectedImageIndex,
			int aOwnerUserRID)
			//Begin Track #6321 - JScott - User has ability to to create folders when security is view
			//: base(aSAB, aTreeNodeType, aProfile, aFunctionSecurityProfile, true, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			: base(aSAB, aTreeNodeType, aProfile, new MIDTreeNodeSecurityGroup(aFunctionSecurityProfile, null), true, aText, aParentId, aUserId, aImageIndex, aSelectedImageIndex, aOwnerUserRID)
			//End Track #6321 - JScott - User has ability to to create folders when security is view
		{
            CommonLoad();
		}

        private void CommonLoad()
        {
            _sequence = -1;
        }

        /// <summary>
        /// Gets or sets the Method or Workflow record id
        /// </summary>
        public int Key
        {
            get { return Profile.Key; }
        }

        /// <summary>
        /// Gets or sets the Workflow Method Indicator of the node.
        /// </summary>
        public eWorkflowMethodIND EditType
        {
            get 
            {
                if (isMethod)
                {
                    return eWorkflowMethodIND.Methods;
                }
                else if (isSizeMethod)
                {
                    return eWorkflowMethodIND.SizeMethods;
                }
                else if (isWorkflow)
                {
                    return eWorkflowMethodIND.Workflows;
                }
                else
                {
                    return 0;
                }

            }
        }

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        /// <summary>
        /// Gets or sets the flag to identify if the method or workflow associated with the node has been resolved.
        /// </summary>
        public bool IsMethodOrWorkflowResolved
        {
            get { return _isMethodOrWorkflowResolved; }
            set { _isMethodOrWorkflowResolved = value; }
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        /// <summary>
		/// Gets or sets the flag to identify if the node is the child of a Workflow.
		/// </summary>
		public bool IsMethodOfWorkflow
		{
			get { return _isMethodOfWorkflow ; }
			set { _isMethodOfWorkflow = value; }
		}

        /// <summary>
        /// Gets or sets the sequence of the child.
        /// </summary>
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        /// <summary>
		/// Gets or sets the Workflow Type of the node.
		/// </summary>
		public eWorkflowType WorkflowType 
		{
            get 
            {
                if (Profile.ProfileType == eProfileType.Workflow)
                {
                    return ((ApplicationBaseWorkFlow)Profile).WorkFlowType;
                }
                else if (Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder)
                {
                    return eWorkflowType.Allocation;
                }
                else if (Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder)
                {
                    return eWorkflowType.Forecast;
                }
                return eWorkflowType.None; 
            }
            //get { return _workflowType ; }
            //set { _workflowType = value; }
		}

        /// <summary>
        /// Gets or sets the Workflow Method Indicator of the node.
        /// </summary>
        public eWorkflowMethodIND WorkflowMethodIND
        {
            get
            {
                // Begin TT#131 - JSmith - velocity method recieve database unique index constraint violation upon save
                //if (isSizeMethod || isSizeMethodFolder)
                //{
                //    return eWorkflowMethodIND.SizeMethods;
                //}
                //else if (isMethod || isMethodFolder)
                //{
                //    return eWorkflowMethodIND.Methods;
                //}
                //else if (isWorkflow || isWorkflowFolder)
                //{
                //    return eWorkflowMethodIND.Workflows;
                //}
                //else
                //{
                //    return 0;
                //}
                if (isSizeMethod || isSizeMethodFolder || isSizeMethodSubFolder)
                {
                    return eWorkflowMethodIND.SizeMethods;
                }
                else if (isMethod || isMethodFolder || isMethodSubFolder)
                {
                    return eWorkflowMethodIND.Methods;
                }
                else if (isWorkflow || isWorkflowFolder || isWorkflowSubFolder)
                {
                    return eWorkflowMethodIND.Workflows;
                }
                else
                {
                    return 0;
                }
                // End TT#131
            }
        }

        /// <summary>
        /// Gets or sets the display option of the node.
        /// </summary>
        public eGlobalUserType GlobalUserType
        {
            get 
            {
                if (OwnerUserRID == Include.GlobalUserRID)
                {
                    return eGlobalUserType.Global;
                }
                else
                {
                    return eGlobalUserType.User;
                }
            }
        }

        public bool isGlobalItem
        {
            get
            {
                if (OwnerUserRID == Include.GlobalUserRID)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool isUserItem
        {
            get
            {
                if (OwnerUserRID == Include.GlobalUserRID)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the Method Type of the node.
        /// </summary>
        public eMethodTypeUI MethodType
        {
            get
            {
                switch (Profile.ProfileType)
                {
                    case eProfileType.MethodOTSPlan:
                    case eProfileType.MethodOTSPlanFolder:
                    case eProfileType.MethodOTSPlanSubFolder:
                        return eMethodTypeUI.OTSPlan;
                    case eProfileType.MethodForecastBalance:
                    case eProfileType.MethodForecastBalanceFolder:
                    case eProfileType.MethodForecastBalanceSubFolder:
                        return eMethodTypeUI.ForecastBalance;
                    case eProfileType.MethodForecastSpread:
                    case eProfileType.MethodForecastSpreadFolder:
                    case eProfileType.MethodForecastSpreadSubFolder:
                        return eMethodTypeUI.ForecastSpread;
                    case eProfileType.MethodGlobalUnlock:
                    case eProfileType.MethodGlobalUnlockFolder:
                    case eProfileType.MethodGlobalUnlockSubFolder:
                        return eMethodTypeUI.GlobalUnlock;
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eProfileType.MethodGlobalLock:
                    case eProfileType.MethodGlobalLockFolder:
                    case eProfileType.MethodGlobalLockSubFolder:
                        return eMethodTypeUI.GlobalLock;
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eProfileType.MethodRollup:
                    case eProfileType.MethodRollupFolder:
                    case eProfileType.MethodRollupSubFolder:
                        return eMethodTypeUI.Rollup;
                    case eProfileType.MethodModifySales:
                    case eProfileType.MethodModifySalesFolder:
                    case eProfileType.MethodModifySalesSubFolder:
                        return eMethodTypeUI.ForecastModifySales;
                    case eProfileType.MethodCopyChainForecast:
                    case eProfileType.MethodCopyChainForecastFolder:
                    case eProfileType.MethodCopyChainForecastSubFolder:
                        return eMethodTypeUI.CopyChainForecast;
                    case eProfileType.MethodCopyStoreForecast:
                    case eProfileType.MethodCopyStoreForecastFolder:
                    case eProfileType.MethodCopyStoreForecastSubFolder:
                        return eMethodTypeUI.CopyStoreForecast;
                    case eProfileType.MethodExport:
                    case eProfileType.MethodExportFolder:
                    case eProfileType.MethodExportSubFolder:
                        return eMethodTypeUI.Export;
                    // Begin TT#2131-MD - JSmith - Halo Integration
                    case eProfileType.MethodPlanningExtract:
                    case eProfileType.MethodPlanningExtractFolder:
                    case eProfileType.MethodPlanningExtractSubFolder:
                        return eMethodTypeUI.PlanningExtract;
                    // End TT#2131-MD - JSmith - Halo Integration
                    case eProfileType.MethodGeneralAllocation:
                    case eProfileType.MethodGeneralAllocationFolder:
                    case eProfileType.MethodGeneralAllocationSubFolder:
                        return eMethodTypeUI.GeneralAllocation;
                    case eProfileType.MethodAllocationOverride:
                    case eProfileType.MethodAllocationOverrideFolder:
                    case eProfileType.MethodAllocationOverrideSubFolder:
                        return eMethodTypeUI.AllocationOverride;
                    case eProfileType.MethodRule:
                    case eProfileType.MethodRuleFolder:
                    case eProfileType.MethodRuleSubFolder:
                        return eMethodTypeUI.Rule;
                    case eProfileType.MethodVelocity:
                    case eProfileType.MethodVelocityFolder:
                    case eProfileType.MethodVelocitySubFolder:
                        return eMethodTypeUI.Velocity;
                    case eProfileType.MethodFillSizeHolesAllocation:
                    case eProfileType.MethodFillSizeHolesFolder:
                    case eProfileType.MethodFillSizeHolesSubFolder:
                        return eMethodTypeUI.FillSizeHolesAllocation;
                    case eProfileType.MethodSizeNeedAllocation:
                    case eProfileType.MethodSizeNeedFolder:
                    case eProfileType.MethodSizeNeedSubFolder:
                        return eMethodTypeUI.SizeNeedAllocation;
                    // Begin TT#155 - JSmith - Size Curve Method
                    case eProfileType.MethodSizeCurve:
                    case eProfileType.MethodSizeCurveFolder:
                    case eProfileType.MethodSizeCurveSubFolder:
                        return eMethodTypeUI.SizeCurve;
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    case eProfileType.MethodBuildPacks:
                    case eProfileType.MethodBuildPacksFolder:
                    case eProfileType.MethodBuildPacksSubFolder:
                        return eMethodTypeUI.BuildPacks;
                    // End TT#370
					// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
					case eProfileType.MethodGroupAllocation:
					case eProfileType.MethodGroupAllocationFolder:
					case eProfileType.MethodGroupAllocationSubFolder:
						return eMethodTypeUI.GroupAllocation;
					// END TT#708-MD - Stodd - Group Allocation Prototype.
					// Begin TT#1652-MD - stodd - DC Carton Rounding
                    case eProfileType.MethodDCCartonRounding:
                    case eProfileType.MethodDCCartonRoundingFolder:
                    case eProfileType.MethodDCCartonRoundingSubFolder:
                        return eMethodTypeUI.DCCartonRounding;
					// End TT#1652-MD - stodd - DC Carton Rounding
                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    case eProfileType.MethodCreateMasterHeaders:
                    case eProfileType.MethodCreateMasterHeadersFolder:
                    case eProfileType.MethodCreateMasterHeadersSubFolder:
                        return eMethodTypeUI.CreateMasterHeaders;
                    case eProfileType.MethodDCFulfillment:
                    case eProfileType.MethodDCFulfillmentFolder:
                    case eProfileType.MethodDCFulfillmentSubFolder:
                        return eMethodTypeUI.DCFulfillment;
                    // End TT#1966-MD - JSmith- DC Fulfillment
                    case eProfileType.MethodBasisSizeAllocation:
                    case eProfileType.MethodBasisSizeFolder:
                    case eProfileType.MethodBasisSizeSubFolder:
                        return eMethodTypeUI.BasisSizeAllocation;
                    case eProfileType.MethodGeneralAssortment:
                        //case eProfileType.MethodGeneralFolder:
                        return eMethodTypeUI.GeneralAssortment;
                    default:
                        return eMethodTypeUI.None;
                }
            }
        }

        /// <summary>
		/// Gets or sets the description of the child.
		/// </summary>
		public string NodeDescription 
		{
			get { return _nodeDescription ; }
			set { _nodeDescription = value; }
		}

        public bool isMyFolder
        {
            get
            {
                if (this.UserId == SAB.ClientServerSession.UserRID &&
                    (isWorkflowFolder || isMethodFolder))
                {
                    return true;
                }
                return false;
            }
        }

        public bool isMethod
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlan ||
                    Profile.ProfileType == eProfileType.MethodForecastBalance ||
                    Profile.ProfileType == eProfileType.MethodModifySales ||
                    Profile.ProfileType == eProfileType.MethodForecastSpread ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecast ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecast ||
                    Profile.ProfileType == eProfileType.MethodExport ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtract ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlock ||
                    Profile.ProfileType == eProfileType.MethodGlobalLock ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollup ||
                    Profile.ProfileType == eProfileType.MethodGeneralAllocation ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverride ||
                    Profile.ProfileType == eProfileType.MethodRule ||
                    Profile.ProfileType == eProfileType.MethodVelocity ||
                    Profile.ProfileType == eProfileType.MethodSizeNeedAllocation ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurve ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacks ||
                    // End TT#370
					// BEGIN TT#708-MD - Stodd - Group Allocation Prototype.
					Profile.ProfileType == eProfileType.MethodGroupAllocation ||
                    Profile.ProfileType == eProfileType.MethodDCCartonRounding ||	// TT#1652-MD - stodd - DC Carton Rounding
					// END TT#708-MD - Stodd - Group Allocation Prototype.
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeaders ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillment ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesAllocation ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeAllocation ||
                    Profile.ProfileType == eProfileType.MethodWarehouseSizeAllocation)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isOTSForecastMethod
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlan ||
                    Profile.ProfileType == eProfileType.MethodForecastBalance ||
                    Profile.ProfileType == eProfileType.MethodModifySales ||
                    Profile.ProfileType == eProfileType.MethodForecastSpread ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecast ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecast ||
                    Profile.ProfileType == eProfileType.MethodExport ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtract ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlock ||
                    Profile.ProfileType == eProfileType.MethodGlobalLock ||   //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollup)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isOTSForecastWorkflow
        {
            get
            {
                if (Profile.ProfileType == eProfileType.Workflow &&
                    WorkflowType == eWorkflowType.Forecast)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isAllocationMethod
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodGeneralAllocation ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverride ||
                    Profile.ProfileType == eProfileType.MethodRule ||
                    Profile.ProfileType == eProfileType.MethodVelocity ||
                    Profile.ProfileType == eProfileType.MethodSizeNeedAllocation ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurve ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacks ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocation ||		// TT#708-MD - Stodd - Group Allocation Prototype.
                    Profile.ProfileType == eProfileType.MethodDCCartonRounding ||		// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeaders ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillment ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodFillSizeHolesAllocation ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeAllocation ||
                    Profile.ProfileType == eProfileType.MethodWarehouseSizeAllocation)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isAllocationWorkflow
        {
            get
            {
                if (Profile.ProfileType == eProfileType.Workflow &&
                    WorkflowType == eWorkflowType.Allocation)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isSizeMethod
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodSizeNeedAllocation ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurve ||
                    // End TT#155
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesAllocation ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeAllocation ||
                    Profile.ProfileType == eProfileType.MethodWarehouseSizeAllocation)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWorkflow
        {
            get
            {
                if (Profile.ProfileType == eProfileType.Workflow)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWorkflowMethod
        {
            get
            {
                if (isMethod || isWorkflow)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWorkflowFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWorkflowMethodSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder ||
                    Profile.ProfileType == eProfileType.MethodOTSPlanSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastBalanceSubFolder ||
                    Profile.ProfileType == eProfileType.MethodModifySalesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastSpreadSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodExportSubFolder ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtractSubFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlockSubFolder ||
                    Profile.ProfileType == eProfileType.MethodGlobalLockSubFolder ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollupSubFolder ||
                    Profile.ProfileType == eProfileType.MethodGeneralAllocationSubFolder ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverrideSubFolder ||
                    Profile.ProfileType == eProfileType.MethodRuleSubFolder ||
                    Profile.ProfileType == eProfileType.MethodVelocitySubFolder ||
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeSubFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveSubFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacksSubFolder ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocationSubFolder ||	// TT#708-MD - Stodd - Group Allocation Prototype.
                    Profile.ProfileType == eProfileType.MethodDCCartonRoundingSubFolder ||	// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeadersSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillmentSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodSizeNeedSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWorkflowSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isMethodFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlanFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastBalanceFolder ||
                    Profile.ProfileType == eProfileType.MethodModifySalesFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastSpreadFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecastFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecastFolder ||
                    Profile.ProfileType == eProfileType.MethodExportFolder ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtractFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlockFolder ||
                    Profile.ProfileType == eProfileType.MethodGlobalLockFolder || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollupFolder ||
                    Profile.ProfileType == eProfileType.MethodGeneralAllocationFolder ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverrideFolder ||
                    Profile.ProfileType == eProfileType.MethodRuleFolder ||
                    Profile.ProfileType == eProfileType.MethodVelocityFolder ||
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacksFolder ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocationFolder ||
                    Profile.ProfileType == eProfileType.MethodDCCartonRoundingFolder ||		// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeadersFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillmentFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodSizeNeedFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isMethodsFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isMethodSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlanSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastBalanceSubFolder ||
                    Profile.ProfileType == eProfileType.MethodModifySalesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastSpreadSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodExportSubFolder ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtractSubFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlockSubFolder ||
                    Profile.ProfileType == eProfileType.MethodGlobalLockSubFolder ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollupSubFolder ||
                    Profile.ProfileType == eProfileType.MethodGeneralAllocationSubFolder ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverrideSubFolder ||
                    Profile.ProfileType == eProfileType.MethodRuleSubFolder ||
                    Profile.ProfileType == eProfileType.MethodVelocitySubFolder ||
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeSubFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveSubFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacksSubFolder ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocationSubFolder ||
                    Profile.ProfileType == eProfileType.MethodDCCartonRoundingSubFolder ||		// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeadersSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillmentSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodSizeNeedSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isOTSForecastMethodFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlanFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastBalanceFolder ||
                    Profile.ProfileType == eProfileType.MethodModifySalesFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastSpreadFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecastFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecastFolder ||
                    Profile.ProfileType == eProfileType.MethodExportFolder ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtractFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlockFolder ||
                    Profile.ProfileType == eProfileType.MethodGlobalLockFolder || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollupFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isOTSForecastMethodSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodOTSPlanSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastBalanceSubFolder ||
                    Profile.ProfileType == eProfileType.MethodModifySalesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodForecastSpreadSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyChainForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                    Profile.ProfileType == eProfileType.MethodExportSubFolder ||
                    Profile.ProfileType == eProfileType.MethodPlanningExtractSubFolder ||  // TT#2131-MD - JSmith - Halo Integration
                    Profile.ProfileType == eProfileType.MethodGlobalUnlockSubFolder ||
                    Profile.ProfileType == eProfileType.MethodGlobalLockSubFolder ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                    Profile.ProfileType == eProfileType.MethodRollupSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isAllocationMethodFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodGeneralAllocationFolder ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverrideFolder ||
                    Profile.ProfileType == eProfileType.MethodRuleFolder ||
                    Profile.ProfileType == eProfileType.MethodVelocityFolder ||
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacksFolder ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocationFolder ||
                    Profile.ProfileType == eProfileType.MethodDCCartonRoundingFolder ||		// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeadersFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillmentFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodSizeNeedFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isAllocationMethodSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodGeneralAllocationSubFolder ||
                    Profile.ProfileType == eProfileType.MethodAllocationOverrideSubFolder ||
                    Profile.ProfileType == eProfileType.MethodRuleSubFolder ||
                    Profile.ProfileType == eProfileType.MethodVelocitySubFolder ||
                    Profile.ProfileType == eProfileType.MethodFillSizeHolesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeSubFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveSubFolder ||
                    // End TT#155

                    // Begin TT#370 - APicchetti - Build Packs Method
                    Profile.ProfileType == eProfileType.MethodBuildPacksSubFolder ||
                    // End TT#370
					Profile.ProfileType == eProfileType.MethodGroupAllocationSubFolder ||
                    Profile.ProfileType == eProfileType.MethodDCCartonRoundingSubFolder ||		// TT#1652-MD - stodd - DC Carton Rounding
                    Profile.ProfileType == eProfileType.MethodCreateMasterHeadersSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                    Profile.ProfileType == eProfileType.MethodDCFulfillmentSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                    Profile.ProfileType == eProfileType.MethodSizeNeedSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isSizeMethodFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodFillSizeHolesFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveFolder ||
                    // End TT#155
                    Profile.ProfileType == eProfileType.MethodSizeNeedFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isSizeMethodSubFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.MethodFillSizeHolesSubFolder ||
                    Profile.ProfileType == eProfileType.MethodBasisSizeSubFolder ||
                    // Begin TT#155 - JSmith - Size Curve Method
                    Profile.ProfileType == eProfileType.MethodSizeCurveSubFolder ||
                    // End TT#155
                    Profile.ProfileType == eProfileType.MethodSizeNeedSubFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isGroupFolder
        {
            get
            {
                if (Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                {
                    return true;
                }
                return false;
            }
        }

        public bool isWithinItemFolder
        {
            get
            {
                MIDTreeNode node;

                try
                {
                    node = this;

                    while (node.Parent != null)
                    {
                        if (((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                            ((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodAllocationWorkflowsFolder ||
                            ((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                            ((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsFolder)
                        {
                            return true;
                        }
                        node = (MIDTreeNode)node.Parent;
                    }

                    return false;
                }
                catch (Exception exc)
                {
                    string message = exc.ToString();
                    throw;
                }
            }
        }

        /// <summary>
        /// Determines if a method type is a corresponding eProfileType for a method
        /// </summary>
        /// <param name="aProfileType">The profile type to check</param>
        /// <returns>A flag identifying if the profile type is a valid sub folder type</returns>
        /// <remarks>The method must be change if a new sub folder is added</remarks>
        private bool isSubFolderType(eProfileType aProfileType)
        {
            if (aProfileType == eProfileType.WorkflowMethodSubFolder ||
                // Begin TT#33 - JSmith - OTS Workflow sub folder not displaying in favorites
                aProfileType == eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder ||
                // End TT#33
                aProfileType == eProfileType.MethodOTSPlanSubFolder ||
                aProfileType == eProfileType.MethodForecastBalanceSubFolder ||
                aProfileType == eProfileType.MethodModifySalesSubFolder ||
                aProfileType == eProfileType.MethodForecastSpreadSubFolder ||
                aProfileType == eProfileType.MethodCopyChainForecastSubFolder ||
                aProfileType == eProfileType.MethodCopyStoreForecastSubFolder ||
                aProfileType == eProfileType.MethodExportSubFolder ||
                aProfileType == eProfileType.MethodPlanningExtractSubFolder ||  // End TT#2131-MD - JSmith - Halo Integration
                aProfileType == eProfileType.MethodGlobalUnlockSubFolder ||
                aProfileType == eProfileType.MethodGlobalLockSubFolder || //TT#43 - MD - DOConnell - Projected Sales Enhancement
                aProfileType == eProfileType.MethodRollupSubFolder ||
                aProfileType == eProfileType.WorkflowMethodAllocationWorkflowsSubFolder ||
                aProfileType == eProfileType.MethodGeneralAllocationSubFolder ||
                aProfileType == eProfileType.MethodAllocationOverrideSubFolder ||
                aProfileType == eProfileType.MethodRuleSubFolder ||
                aProfileType == eProfileType.MethodVelocitySubFolder ||
                aProfileType == eProfileType.MethodSizeMethodSubFolder ||
                aProfileType == eProfileType.MethodFillSizeHolesSubFolder ||
                aProfileType == eProfileType.MethodBasisSizeSubFolder ||
                // Begin TT#155 - JSmith - Size Curve Method
                aProfileType == eProfileType.MethodSizeCurveSubFolder ||
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                aProfileType == eProfileType.MethodBuildPacksSubFolder ||
                // End TT#370
				aProfileType == eProfileType.MethodGroupAllocationSubFolder ||
                aProfileType == eProfileType.MethodDCCartonRoundingSubFolder ||		// TT#1652-MD - stodd - DC Carton Rounding
                aProfileType == eProfileType.MethodCreateMasterHeadersSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment
                aProfileType == eProfileType.MethodDCFulfillmentSubFolder ||	// TT#1966-MD - JSmith- DC Fulfillment

                aProfileType == eProfileType.MethodSizeNeedSubFolder)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if a method type is a corresponding eProfileType for a method
        /// </summary>
        /// <param name="aMethodType">The method type to check</param>
        /// <returns>A flag identifying if the integer is a valid method</returns>
        /// <remarks>The method must be change if a new method is added</remarks>
        private bool isMethodProfileType(int aMethodType)
        {
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//if (aMethodType == (int)eProfileType.Method ||
            //    aMethodType == (int)eProfileType.MethodOTSPlan ||
            if (aMethodType == (int)eProfileType.MethodOTSPlan ||
			//End TT#523 - JScott - Duplicate folder when new folder added
				aMethodType == (int)eProfileType.MethodForecastBalance ||
                aMethodType == (int)eProfileType.MethodModifySales ||
                aMethodType == (int)eProfileType.MethodForecastSpread ||
                aMethodType == (int)eProfileType.MethodCopyChainForecast ||
                aMethodType == (int)eProfileType.MethodCopyStoreForecast ||
                aMethodType == (int)eProfileType.MethodExport ||
                aMethodType == (int)eProfileType.MethodPlanningExtract ||  // TT#2131-MD - JSmith - Halo Integration
                aMethodType == (int)eProfileType.MethodGlobalUnlock ||
                aMethodType == (int)eProfileType.MethodGlobalLock ||  //TT#43 - MD - DOConnell - Projected Sales Enhancement
                aMethodType == (int)eProfileType.MethodRollup ||
                aMethodType == (int)eProfileType.MethodGeneralAllocation ||
                aMethodType == (int)eProfileType.MethodAllocationOverride ||
                aMethodType == (int)eProfileType.MethodRule ||
                aMethodType == (int)eProfileType.MethodVelocity ||
                aMethodType == (int)eProfileType.MethodSizeNeedAllocation ||
                // Begin TT#155 - JSmith - Size Curve Method
                aMethodType == (int)eProfileType.MethodSizeCurve ||
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                aMethodType == (int)eProfileType.MethodBuildPacks||
                // End TT#370
				aMethodType == (int)eProfileType.MethodGroupAllocation ||
                aMethodType == (int)eProfileType.MethodDCCartonRounding ||		// TT#1652-MD - stodd - DC Carton Rounding
                aMethodType == (int)eProfileType.MethodCreateMasterHeaders ||	// TT#1966-MD - JSmith- DC Fulfillment
                aMethodType == (int)eProfileType.MethodDCFulfillment ||	   // TT#1966-MD - JSmith- DC Fulfillment

                aMethodType == (int)eProfileType.MethodFillSizeHolesAllocation ||
                aMethodType == (int)eProfileType.MethodBasisSizeAllocation ||
                aMethodType == (int)eProfileType.MethodWarehouseSizeAllocation)
            {
                return true;
            }
            return false;
        }

        public int GetGroupKey()
        {
            MIDTreeNode node;

            try
            {
                node = this;

                while (node.Parent != null)
                {
                    if (((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                        ((MIDTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder)
                    {
                        return node.Profile.Key;
                    }
                    node = (MIDTreeNode)node.Parent;
                }

                return Include.NoRID;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public eProfileType GetPathType()
        {
            MIDTreeNode node;

            try
            {
                node = this;
                if (node.Profile.ProfileType == eProfileType.WorkflowMethodMainFavoritesFolder ||
                        node.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder ||
                        node.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                        ((MIDWorkflowMethodTreeNode)node).isMethodFolder ||
                        ((MIDWorkflowMethodTreeNode)node).isMethodSubFolder ||
                        ((MIDWorkflowMethodTreeNode)node).isWorkflowFolder ||
                        ((MIDWorkflowMethodTreeNode)node).isWorkflowSubFolder)
                {
                    return Profile.ProfileType;
                }
                else
                {
                    while (node.Parent != null)
                    {
                        node = (MIDTreeNode)node.Parent;
                        if (node.Profile.ProfileType == eProfileType.WorkflowMethodMainFavoritesFolder ||
                            node.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder ||
                            node.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                            ((MIDWorkflowMethodTreeNode)node).isMethodFolder ||
                            ((MIDWorkflowMethodTreeNode)node).isMethodSubFolder ||
                            ((MIDWorkflowMethodTreeNode)node).isWorkflowFolder ||
                            ((MIDWorkflowMethodTreeNode)node).isWorkflowSubFolder)
                        {
                            return Profile.ProfileType;
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            return eProfileType.None;
        }

		//Begin TT#922 - JScott - Object reference error when trying to create method
		//public eProfileType GetFolderType()
		//{
		//    MIDTreeNode node;

		//    switch (GetPathType())
		//    {
		//        case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
		//            return eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder;
		//        case eProfileType.MethodOTSPlanFolder:
		//            return eProfileType.MethodOTSPlanSubFolder;
		//        case eProfileType.MethodForecastBalanceFolder:
		//            return eProfileType.MethodForecastBalanceSubFolder;
		//        case eProfileType.MethodModifySalesFolder:
		//            return eProfileType.MethodModifySalesSubFolder;
		//        case eProfileType.MethodForecastSpreadFolder:
		//            return eProfileType.MethodForecastSpreadSubFolder;
		//        case eProfileType.MethodCopyChainForecastFolder:
		//            return eProfileType.MethodCopyChainForecastSubFolder;
		//        case eProfileType.MethodCopyStoreForecastFolder:
		//            return eProfileType.MethodCopyStoreForecastSubFolder;
		//        case eProfileType.MethodExportFolder:
		//            return eProfileType.MethodExportSubFolder;
		//        case eProfileType.MethodGlobalUnlockFolder:
		//            return eProfileType.MethodGlobalUnlockSubFolder;
		//        case eProfileType.MethodRollupFolder:
		//            return eProfileType.MethodRollupSubFolder;
		//        case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
		//            return eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
		//        case eProfileType.MethodGeneralAllocationFolder:
		//            return eProfileType.MethodGeneralAllocationSubFolder;
		//        case eProfileType.MethodAllocationOverrideFolder:
		//            return eProfileType.MethodAllocationOverrideSubFolder;
		//        case eProfileType.MethodRuleFolder:
		//            return eProfileType.MethodRuleSubFolder;
		//        case eProfileType.MethodVelocityFolder:
		//            return eProfileType.MethodVelocitySubFolder;
		//        case eProfileType.MethodSizeNeedFolder:
		//            return eProfileType.MethodSizeNeedSubFolder;
		//        // Begin TT#155 - JSmith - Size Curve Method
		//        case eProfileType.MethodSizeCurveFolder:
		//            return eProfileType.MethodSizeCurveSubFolder;
		//        // End TT#155

		//        // Begin TT#370 - APicchetti - Build Packs Method
		//        case eProfileType.MethodBuildPacksFolder:
		//            return eProfileType.MethodBuildPacksSubFolder;
		//        // End TT#370

		//        case eProfileType.MethodFillSizeHolesFolder:
		//            return eProfileType.MethodFillSizeHolesSubFolder;
		//        case eProfileType.MethodBasisSizeFolder:
		//            return eProfileType.MethodBasisSizeSubFolder;
		//        //case eProfileType.MethodWarehouseSizeAllocation:
		//        //folderType = eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
		//        //break;
		//        default:
		//            return eProfileType.WorkflowMethodSubFolder;
		//    }
		//    return eProfileType.None;
		//}
		public eProfileType GetFolderType()
        {
			try
			{
				switch (GetPathType())
				{
					case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
					case eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder:
						return eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder;
					case eProfileType.MethodOTSPlanFolder:
					case eProfileType.MethodOTSPlanSubFolder:
						return eProfileType.MethodOTSPlanSubFolder;
					case eProfileType.MethodForecastBalanceFolder:
					case eProfileType.MethodForecastBalanceSubFolder:
						return eProfileType.MethodForecastBalanceSubFolder;
					case eProfileType.MethodModifySalesFolder:
					case eProfileType.MethodModifySalesSubFolder:
						return eProfileType.MethodModifySalesSubFolder;
					case eProfileType.MethodForecastSpreadFolder:
					case eProfileType.MethodForecastSpreadSubFolder:
						return eProfileType.MethodForecastSpreadSubFolder;
					case eProfileType.MethodCopyChainForecastFolder:
					case eProfileType.MethodCopyChainForecastSubFolder:
						return eProfileType.MethodCopyChainForecastSubFolder;
					case eProfileType.MethodCopyStoreForecastFolder:
					case eProfileType.MethodCopyStoreForecastSubFolder:
						return eProfileType.MethodCopyStoreForecastSubFolder;
					case eProfileType.MethodExportFolder:
					case eProfileType.MethodExportSubFolder:
						return eProfileType.MethodExportSubFolder;
                    // Begin TT#2131-MD - JSmith - Halo Integration
                    case eProfileType.MethodPlanningExtractFolder:
                    case eProfileType.MethodPlanningExtractSubFolder:
                        return eProfileType.MethodPlanningExtractSubFolder;
                    // End TT#2131-MD - JSmith - Halo Integration
					case eProfileType.MethodGlobalUnlockFolder:
					case eProfileType.MethodGlobalUnlockSubFolder:
						return eProfileType.MethodGlobalUnlockSubFolder;
                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eProfileType.MethodGlobalLockFolder:
                    case eProfileType.MethodGlobalLockSubFolder:
                        return eProfileType.MethodGlobalLockSubFolder;
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement
					case eProfileType.MethodRollupFolder:
					case eProfileType.MethodRollupSubFolder:
						return eProfileType.MethodRollupSubFolder;
					case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
					case eProfileType.WorkflowMethodAllocationWorkflowsSubFolder:
						return eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
					case eProfileType.MethodGeneralAllocationFolder:
					case eProfileType.MethodGeneralAllocationSubFolder:
						return eProfileType.MethodGeneralAllocationSubFolder;
					case eProfileType.MethodAllocationOverrideFolder:
					case eProfileType.MethodAllocationOverrideSubFolder:
						return eProfileType.MethodAllocationOverrideSubFolder;
					case eProfileType.MethodGroupAllocationFolder:
					case eProfileType.MethodGroupAllocationSubFolder:
						return eProfileType.MethodGroupAllocationSubFolder;

					// Begin TT#1652-MD - stodd - DC Carton Rounding
                    case eProfileType.MethodDCCartonRoundingFolder:
                    case eProfileType.MethodDCCartonRoundingSubFolder:
                        return eProfileType.MethodDCCartonRoundingSubFolder;
					// End TT#1652-MD - stodd - DC Carton Rounding

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    case eProfileType.MethodCreateMasterHeadersFolder:
                    case eProfileType.MethodCreateMasterHeadersSubFolder:
                        return eProfileType.MethodCreateMasterHeadersSubFolder;
                    case eProfileType.MethodDCFulfillmentFolder:
                    case eProfileType.MethodDCFulfillmentSubFolder:
                        return eProfileType.MethodDCFulfillmentSubFolder;
                    // End TT#1966-MD - JSmith- DC Fulfillment

					case eProfileType.MethodRuleFolder:
					case eProfileType.MethodRuleSubFolder:
						return eProfileType.MethodRuleSubFolder;
					case eProfileType.MethodVelocityFolder:
					case eProfileType.MethodVelocitySubFolder:
						return eProfileType.MethodVelocitySubFolder;
					case eProfileType.MethodSizeNeedFolder:
					case eProfileType.MethodSizeNeedSubFolder:
						return eProfileType.MethodSizeNeedSubFolder;
					case eProfileType.MethodSizeCurveFolder:
					case eProfileType.MethodSizeCurveSubFolder:
						return eProfileType.MethodSizeCurveSubFolder;
					case eProfileType.MethodBuildPacksFolder:
					case eProfileType.MethodBuildPacksSubFolder:
						return eProfileType.MethodBuildPacksSubFolder;
					case eProfileType.MethodFillSizeHolesFolder:
					case eProfileType.MethodFillSizeHolesSubFolder:
						return eProfileType.MethodFillSizeHolesSubFolder;
					case eProfileType.MethodBasisSizeFolder:
					case eProfileType.MethodBasisSizeSubFolder:
						return eProfileType.MethodBasisSizeSubFolder;
					default:
						return eProfileType.WorkflowMethodSubFolder;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
        }
		//End TT#922 - JScott - Object reference error when trying to create method

        public int GetFolderParentKey()
        {
            MIDTreeNode node;

            switch (GetPathType())
            {
                case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                    return GetGroupKey();
                case eProfileType.MethodOTSPlanFolder:
                    return GetGroupKey();
                case eProfileType.MethodForecastBalanceFolder:
                    return GetGroupKey();
                case eProfileType.MethodModifySalesFolder:
                    return GetGroupKey();
                case eProfileType.MethodForecastSpreadFolder:
                    return GetGroupKey();
                case eProfileType.MethodCopyChainForecastFolder:
                    return GetGroupKey();
                case eProfileType.MethodCopyStoreForecastFolder:
                    return GetGroupKey();
                case eProfileType.MethodExportFolder:
                    return GetGroupKey();
                // Begin TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodPlanningExtractFolder:
                    return GetGroupKey();
                // End TT#2131-MD - JSmith - Halo Integration
                case eProfileType.MethodGlobalUnlockFolder:
                    return GetGroupKey();
                //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                case eProfileType.MethodGlobalLockFolder:
                    return GetGroupKey();
                //End TT#43 - MD - DOConnell - Projected Sales Enhancement
                case eProfileType.MethodRollupFolder:
                    return GetGroupKey();
                case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                    return GetGroupKey();
                case eProfileType.MethodGeneralAllocationFolder:
                    return GetGroupKey();
                case eProfileType.MethodAllocationOverrideFolder:
                    return GetGroupKey();
                case eProfileType.MethodRuleFolder:
                    return GetGroupKey();
                case eProfileType.MethodVelocityFolder:
                    return GetGroupKey();
                case eProfileType.MethodSizeNeedFolder:
                    return GetGroupKey();
                // Begin TT#155 - JSmith - Size Curve Method
                case eProfileType.MethodSizeCurveFolder:
                    return GetGroupKey();
                // End TT#155

                // Begin TT#370 - APicchetti - Build Packs Method
                case eProfileType.MethodBuildPacksFolder:
                    return GetGroupKey();
                // End TT#370
				case eProfileType.MethodGroupAllocationFolder:
					return GetGroupKey();

				// Begin TT#1652-MD - stodd - DC Carton Rounding
                case eProfileType.MethodDCCartonRoundingFolder:
                    return GetGroupKey();
				// End TT#1652-MD - stodd - DC Carton Rounding
                // Begin TT#1966-MD - JSmith- DC Fulfillment
                case eProfileType.MethodCreateMasterHeadersFolder:
                    return GetGroupKey();
                case eProfileType.MethodDCFulfillmentFolder:
                    return GetGroupKey();
                // End TT#1966-MD - JSmith- DC Fulfillment

                case eProfileType.MethodFillSizeHolesFolder:
                    return GetGroupKey();
                case eProfileType.MethodBasisSizeFolder:
                    return GetGroupKey();
                //case eProfileType.MethodWarehouseSizeAllocation:
                //folderType = eProfileType.WorkflowMethodAllocationWorkflowsSubFolder;
                //break;
                default:
                    return Profile.Key;
            }
        }

        override public int CompareTo(object obj)
        {
            try
            {
                //return base.CompareTo(obj);

                if (_sequence < ((MIDWorkflowMethodTreeNode)obj)._sequence)
                {
                    return -1;
                }
                else if (_sequence > ((MIDWorkflowMethodTreeNode)obj)._sequence)
                {
                    return 1;
                }
                else
                {
                    return base.CompareTo(obj);
                    //return Text.CompareTo(((MIDWorkflowMethodTreeNode)obj).Text);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        /// <summary>
		/// Used to clone or copy a node.
		/// </summary>
		/// <remarks>
		/// This method must be change when fields are added or removed from the class.
		/// </remarks>
		public MIDWorkflowMethodTreeNode CloneNode() 
		{
			MIDWorkflowMethodTreeNode mwmtn = (MIDWorkflowMethodTreeNode) base.Clone();
			
            mwmtn._isMethodOfWorkflow = this._isMethodOfWorkflow;
            //mwmtn._workflowType = this._workflowType;

			return mwmtn;
		}

		//Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode can be selected
		/// </summary>
		/// <param name="aMultiSelect">
		/// A boolean indication if multiselect is being performed.
		/// </param>
		/// <param name="aSelectedNodes">
		/// An ArrayList of the currently selected nodes.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode can be selected
		/// </returns>

		override public bool isSelectAllowed(bool aMultiSelect, ArrayList aSelectedNodes)
		{
			bool allowSelect;

			try
			{
				allowSelect = true;

				if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
				{
					allowSelect = false;
				}

				return allowSelect;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode is draggable for a given DragDropEffects
		/// </summary>
		/// <param name="aDragDropEffects">
		/// The current DragDropEffects type being processed.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode is draggable for a given DragDropEffects
		/// </returns>

		override public bool isDragAllowed(DragDropEffects aCurrentEffect)
		{
			bool allowDrag;

			try
			{
                if (FunctionSecurityProfile != null && FunctionSecurityProfile.AccessDenied)
                {
                    return false;
                }
                allowDrag = false;

                if (!isChildShortcut && 
                    (isMethod ||
                    isWorkflow ||
                    isSubFolder ||
                    isGroupFolder))
				{
					allowDrag = true;
				}
				
				return allowDrag;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode can be dropped in the given MIDTreeNode
		/// </summary>
		/// <param name="aDropAction">
		/// The DragDropEffects that is being processed.
		/// </param>
		/// <param name="aSelectedNode">
		/// The MIDTreeNode that this node is being dragged over.
		/// </param>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode can be dropped in the given MIDTreeNode
		/// </returns>

		override public bool isDropAllowed(DragDropEffects aDropAction, MIDTreeNode aDestinationNode)
		{
			MIDWorkflowMethodTreeNode destNode;

			try
			{
                // do not allow drop in shared path or shared node in favorites
                if (aDestinationNode.isShared ||
                    (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
                    this.isShared))
                {
                    return false;
                }

                if (aDestinationNode == null)
                {
                    return false;
                }

				destNode = (MIDWorkflowMethodTreeNode)aDestinationNode;

				// do not allow drop on same node
				if (destNode == this)
				{
					return false;
				}

                // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
				if (destNode.GlobalUserType == eGlobalUserType.Global)
                {
                    if (!_isMethodOrWorkflowResolved)
                    {
                        if (this.isWorkflow)
                        {
                            if (WorkflowType == eWorkflowType.Forecast)
                            {
                                this.Profile = new OTSPlanWorkFlow(SAB, Key, UserId, UserId == Include.GlobalUserRID);
                            }
                            else if (WorkflowType == eWorkflowType.Allocation)
                            {
                                this.Profile = new AllocationWorkFlow(SAB, Key, UserId, UserId == Include.GlobalUserRID);
                            }
                        }
                        else
                        {
                            this.Profile = (ApplicationBaseMethod)((WorkflowMethodTreeView)aDestinationNode.TreeView).GetMethods.GetMethod(Key, (eMethodType)MethodType);
                        }
                        _isMethodOrWorkflowResolved = true;
                    }

                    if (this.isWorkflow
                        && ((ApplicationBaseWorkFlow)this.Profile).ContainsUserData == true)
                    {
                        return false;
                    }
                    else if (!this.isWorkflow
                        && ((ApplicationBaseMethod)this.Profile).ContainsUserData == true)
                    {
                        return false;
                    }
                }
				// End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

				// do not allow drop in same parent
				if (destNode == Parent &&
                    aDropAction != DragDropEffects.Copy)
				{
					return false;
				}

                if (!aDestinationNode.FunctionSecurityProfile.AllowUpdate)
                {
                    return false;
                }

                if (destNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    destNode.Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastMethodsFolder ||
                    destNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    destNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationMethodsFolder ||
                    destNode.Profile.ProfileType == eProfileType.WorkflowMethodAllocationSizeMethodsFolder)
                {
                    return false;
                }

                Debug.WriteLine("--------------------------------------");
                Debug.WriteLine("drag node = " + this.Text);
                Debug.WriteLine("dest node = " + destNode.Text);
                Debug.WriteLine("UserId = " + this.UserId.ToString());
                Debug.WriteLine("dest UserId = " + destNode.UserId.ToString());

                Debug.WriteLine("this.isWorkflowFolder = " + this.isWorkflowFolder.ToString());
                Debug.WriteLine("destNode.isWorkflowFolder = " + destNode.isWorkflowFolder.ToString());
                Debug.WriteLine("this.isWorkflowSubFolder = " + this.isWorkflowSubFolder.ToString());
                Debug.WriteLine("destNode.isWorkflowSubFolder = " + destNode.isWorkflowSubFolder.ToString());
                Debug.WriteLine("this.isWorkflow = " + this.isWorkflow.ToString());
                Debug.WriteLine("destNode.isWorkflow = " + destNode.isWorkflow.ToString());
                Debug.WriteLine("this.WorkflowType = " + this.WorkflowType.ToString());
                Debug.WriteLine("destNode.WorkflowType = " + destNode.WorkflowType.ToString());

                Debug.WriteLine("this.isMethodFolder = " + this.isMethodFolder.ToString());
                Debug.WriteLine("destNode.isMethodFolder = " + destNode.isMethodFolder.ToString());
                Debug.WriteLine("this.isMethodSubFolder = " + this.isMethodSubFolder.ToString());
                Debug.WriteLine("destNode.isMethodSubFolder = " + destNode.isMethodSubFolder.ToString());
                Debug.WriteLine("this.isMethod = " + this.isMethod.ToString());
                Debug.WriteLine("destNode.isMethod = " + destNode.isMethod.ToString());
                Debug.WriteLine("this.MethodType = " + this.MethodType.ToString());
                Debug.WriteLine("destNode.MethodType = " + destNode.MethodType.ToString());
                Debug.WriteLine("destNode.ProfileType = " + destNode.Profile.ProfileType.ToString());

				if (destNode.isWorkflowFolder || destNode.isWorkflowSubFolder)
                {
                    if ((this.isWorkflow || this.isWorkflowFolder || this.isWorkflowSubFolder)
                        && WorkflowType == destNode.WorkflowType)
                    {
                        // Begin TT#41 - JSmith - Workflow method explorer null ref error
                        //if (((WorkflowMethodTreeView)TreeView).PerformingCut)
                        if (aDropAction == DragDropEffects.Move)
                        // End TT#41
                        {
                            if (aDestinationNode == GetParentNode() || aDestinationNode.isChildOf(this) ||
                                (aDestinationNode.isObject && aDestinationNode.GetParentNode() == GetParentNode()))
                            {
                                return false;
                            }
                            else
                            {
                                Debug.WriteLine("1 Allow drop true");
                                // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                                }
                                else
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                return true;
                            }
                        }
                        else
                        {
                            if (aDestinationNode.isChildOf(this))
                            {
                                return false;
                            }
                            else
                            {
                                Debug.WriteLine("2 Allow drop true");
                                // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                                }
                                else
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                return true;
                            }
                        }
                    }
				}
                else if (destNode.isMethodFolder || destNode.isMethodSubFolder)
                {
                    if ((this.isMethod || this.isMethodFolder || this.isMethodSubFolder)
                        && MethodType == destNode.MethodType)
                    {
                        // Begin TT#41 - JSmith - Workflow method explorer null ref error
                        //if (((WorkflowMethodTreeView)TreeView).PerformingCut)
                        if (aDropAction == DragDropEffects.Move)
                        // Begin TT#41
                        {
                            if (aDestinationNode == GetParentNode() || aDestinationNode.isChildOf(this) ||
                                (aDestinationNode.isObject && aDestinationNode.GetParentNode() == GetParentNode()))
                            {
                                return false;
                            }
                            else
                            {
                                Debug.WriteLine("3 Allow drop true");
                                // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                                }
                                else
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                return true;
                            }
                        }
                        else
                        {
                            if (aDestinationNode.isChildOf(this))
                            {
                                return false;
                            }
                            else
                            {

                                Debug.WriteLine("4 Allow drop true");
                                // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                                }
                                else
                                {
                                    ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                                }
                                // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                                return true;
                            }
                        }
                    }
                }                
                else if (aDestinationNode.TreeNodeType == eTreeNodeType.MainFavoriteFolderNode ||
                    aDestinationNode.TreeNodeType == eTreeNodeType.MainSourceFolderNode ||
                    aDestinationNode.isSubFolder)
                {

                    if (GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode &&
                        aDestinationNode.GetTopSourceNode().TreeNodeType != eTreeNodeType.MainFavoriteFolderNode)
                    {
                        Debug.WriteLine("5 Allow drop false");
                        return false;
                    }

                    if ((destNode.Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                        destNode.Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder) &&
                        (isMethod || isWorkflow))
                    {
                        Debug.WriteLine("5a Allow drop false");
                        return false;
                    }

                    if (destNode.GetTopSourceNode().Profile.ProfileType == eProfileType.WorkflowMethodMainGlobalFolder ||
                        destNode.GetTopSourceNode().Profile.ProfileType == eProfileType.WorkflowMethodMainUserFolder)
                    {
                        if ((isWorkflow && !destNode.isWorkflowFolder && !destNode.isWorkflowSubFolder) ||
                            (isMethod && !destNode.isMethodFolder && !destNode.isMethodSubFolder))
                        {
                            Debug.WriteLine("5b Allow drop false");
                            return false;
                        }
                    }

                    if (aDropAction == DragDropEffects.Copy)
                    {
                        if (aDestinationNode.isChildOf(this))
                        {
                            Debug.WriteLine("6 Allow drop false");
                            return false;
                        }
                        else
                        {
                            Debug.WriteLine("7 Allow drop true");
                            // Begin TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                            if (aDestinationNode.GetTopSourceNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                            {
                                ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                            }
                            else if (aDestinationNode.OwnerUserRID == OwnerUserRID)
                            {
                                ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Move;
                            }
                            else
                            {
                                ((MIDTreeView)aDestinationNode.TreeView).CurrentEffect = DragDropEffects.Copy;
                            }
                            // End TT#4359 - JSmith - Unable to drag/drop Methods/Workflows into Folders
                            return true;
                        }
                    }
                    else
                    {
                        if (aDestinationNode == GetParentNode() || aDestinationNode.isChildOf(this))
                        {
                            Debug.WriteLine("8 Allow drop false");
                            return false;
                        }

                        if (aDestinationNode.GetTopSourceNode().Profile.ProfileType != GetTopSourceNode().Profile.ProfileType)
                        {
                            Debug.WriteLine("9 Allow drop " + FunctionSecurityProfile.AllowDelete.ToString());
                            return FunctionSecurityProfile.AllowDelete;
                        }
                        else
                        {
                            Debug.WriteLine("10 Allow drop " + FunctionSecurityProfile.AllowUpdate.ToString());
                            return FunctionSecurityProfile.AllowUpdate;
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("9 Allow drop false");
                    return false;
                }
                

                //DropAction = aDropAction;

                return false;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Abstract method that indicates if this MIDTreeNode's label can be edited
		/// </summary>
		/// <returns>
		/// A boolean indicating if this MIDTreeNode's label can be edited
		/// </returns>

		override public bool isLabelEditAllowed()
		{
			try
			{
                if (FunctionSecurityProfile.AllowUpdate && !isShortcut &&
                    (NodeProfileType == eProfileType.WorkflowMethodMainFavoritesFolder ||
                    NodeProfileType == eProfileType.WorkflowMethodMainUserFolder ||
                    NodeProfileType == eProfileType.WorkflowMethodAllocationFolder ||
                    NodeProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    isSubFolder ||
                    isMethod ||
                    isWorkflow))
                {
                    // Begin TT#2627 - JSmith - Conflict Error Message will not release
                    string errMsg;
                    //return true;
                    MIDEnqueue MIDEnqueueData = new MIDEnqueue();
                    if (isMethod)
                    {
                        // Begin TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application
                        //MethodEnqueue methodEnqueue = new MethodEnqueue(
                        //                                                Key,
                        //                                                SAB.ClientServerSession.UserRID,
                        //                                                SAB.ClientServerSession.ThreadID);

                        //try
                        //{
                        //    methodEnqueue.EnqueueMethod(false);
                        //}
                        //catch (MethodConflictException)
                        //{
                        //    errMsg = "The following method requested:" + System.Environment.NewLine;
                        //    foreach (MethodConflict MCon in methodEnqueue.ConflictList)
                        //    {
                        //        errMsg += System.Environment.NewLine + "Method: " + Text + ", User: " + MCon.UserName;
                        //    }
                        //    errMsg += System.Environment.NewLine + System.Environment.NewLine;
                            
                        //    errMsg += "The selected method can not be updated at this time.";

                        //    MessageBox.Show(errMsg, this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return false;
                        //}
                        // End TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application

                        return true;
                    }
                    else if (isWorkflow)
                    {
                        // Begin TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application
                        //WorkflowEnqueue workflowEnqueue = new WorkflowEnqueue(
                        //                                                    Key,
                        //                                                    SAB.ClientServerSession.UserRID,
                        //                                                    SAB.ClientServerSession.ThreadID);

                        //try
                        //{
                        //    workflowEnqueue.EnqueueWorkflow(false);
                        //}
                        //catch (WorkflowConflictException)
                        //{
                        //    errMsg = "The following workflow requested:" + System.Environment.NewLine;
                        //    foreach (WorkflowConflict WCon in workflowEnqueue.ConflictList)
                        //    {
                        //        errMsg += System.Environment.NewLine + "Workflow: " + Text + ", User: " + WCon.UserName;
                        //    }
                        //    errMsg += System.Environment.NewLine + System.Environment.NewLine;
                        //    errMsg += "The selected workflow can not be updated at this time.";

                        //    MessageBox.Show(errMsg, this.Text + " Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return false;
                        //}
                        // End TT#3992 - JSmith - Dragging method of same name from global to user when user method open lock application

                        return true;
                    }
                    else
                    {
                        return true;
                    }
                    // End TT#2579 - TT#2627 - JSmith - Conflict Error Message will not release
                }
                else
                {
                    return false;
                }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        /// <summary>
        /// Abstract method that refreshes the shortcut node
        /// </summary>

        override public void RefreshShortcutNode(MIDTreeNode aChangedNode)
        {
            try
            {
				if (isObjectShortcut || isFolderShortcut)
                {
                    UserId = aChangedNode.UserId;

                    if (isMethod)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((ApplicationBaseMethod)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((ApplicationBaseMethod)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
					}
                    else if (isWorkflow)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((ApplicationBaseWorkFlow)aChangedNode.Profile).WorkFlowName + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((ApplicationBaseWorkFlow)aChangedNode.Profile).WorkFlowName + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                    else
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						InternalText = ((FolderProfile)aChangedNode.Profile).Name + " (" + GetUserName(aChangedNode.UserId) + ")";
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                }
                else if (isChildShortcut)
                {
                    UserId = aChangedNode.UserId;

                    if (isMethod)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((ApplicationBaseMethod)aChangedNode.Profile).Name;
						InternalText = ((ApplicationBaseMethod)aChangedNode.Profile).Name;
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                    else if (isWorkflow)
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((ApplicationBaseWorkFlow)aChangedNode.Profile).WorkFlowName;
						InternalText = ((ApplicationBaseWorkFlow)aChangedNode.Profile).WorkFlowName;
						//End Track #6201 - JScott - Store Count removed from attr sets
                    }
                    else
                    {
						//Begin Track #6201 - JScott - Store Count removed from attr sets
						//Text = ((FolderProfile)aChangedNode.Profile).Name;
						InternalText = ((FolderProfile)aChangedNode.Profile).Name;
						//End Track #6201 - JScott - Store Count removed from attr sets
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
        /// Retrieves a list of children for the node
        /// </summary>
        /// <returns>An ArrayList containing profiles for each child</returns>
        override public void BuildChildren()
        {
            ArrayList children;
            // Begin TT#1167 - Login Performance - Opening the application after Login
            DataTable dtChildren;
            // End TT#1167;
            try
            {
                Nodes.Clear();
                // Begin TT#1167 - Login Performance - Opening the application after Login
                //children = BuildFolderChildren();
                //Nodes.AddRange((MIDTreeNode[])children.ToArray(typeof(MIDTreeNode)));
                switch (Profile.ProfileType)
                {
                    case eProfileType.WorkflowMethodOTSForcastWorkflowsFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.WorkflowMethodOTSForcastWorkflowsSubFolder, eWorkflowType.Forecast), eWorkflowType.Forecast, true);
                        ((WorkflowMethodTreeView)TreeView).AddWorkflowsToFolder(this, eWorkflowType.Forecast, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.Workflow, eWorkflowType.Forecast), true);
                        break;

                    case eProfileType.MethodOTSPlanFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodOTSPlanSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodOTSPlanSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.OTSPlan, eProfileType.MethodOTSPlan, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodOTSPlan, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodForecastBalanceFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodForecastBalanceSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodForecastBalanceSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.ForecastBalance, eProfileType.MethodForecastBalance, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodForecastBalance, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodModifySalesFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodModifySalesSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodModifySalesSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.ForecastModifySales, eProfileType.MethodModifySales, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodModifySales, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodForecastSpreadFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodForecastSpreadSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodForecastSpreadSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.ForecastSpread, eProfileType.MethodForecastSpread, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodForecastSpread, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodCopyChainForecastFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodCopyChainForecastSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCopyChainForecastSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.CopyChainForecast, eProfileType.MethodCopyChainForecast, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCopyChainForecast, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodCopyStoreForecastFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodCopyStoreForecastSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCopyStoreForecastSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.CopyStoreForecast, eProfileType.MethodCopyStoreForecast, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCopyStoreForecast, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodExportFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodExportSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodExportSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.Export, eProfileType.MethodExport, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodExport, eWorkflowType.None), true);
                        break;

                    // Begin TT#2131-MD - JSmith - Halo Integration
                    case eProfileType.MethodPlanningExtractFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodPlanningExtractSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodPlanningExtractSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.PlanningExtract, eProfileType.MethodPlanningExtract, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodPlanningExtract, eWorkflowType.None), true);
                        break;
                    // End TT#2131-MD - JSmith - Halo Integration

                    case eProfileType.MethodGlobalUnlockFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodGlobalUnlockSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGlobalUnlockSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.GlobalUnlock, eProfileType.MethodGlobalUnlock, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGlobalUnlock, eWorkflowType.None), true);
                        break;

                    //Begin TT#43 - MD - DOConnell - Projected Sales Enhancement
                    case eProfileType.MethodGlobalLockFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodGlobalLockSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGlobalLockSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.GlobalLock, eProfileType.MethodGlobalLock, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGlobalLock, eWorkflowType.None), true);
                        break;
                    //End TT#43 - MD - DOConnell - Projected Sales Enhancement

                    case eProfileType.MethodRollupFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodRollupSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodRollupSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.Rollup, eProfileType.MethodRollup, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodRollup, eWorkflowType.None), true);
                        break;

                    case eProfileType.WorkflowMethodAllocationWorkflowsFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[((MIDTreeNode)Parent).Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.WorkflowMethodAllocationWorkflowsSubFolder, eWorkflowType.Allocation), eWorkflowType.Allocation, true);
                        ((WorkflowMethodTreeView)TreeView).AddWorkflowsToFolder(this, eWorkflowType.Allocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.Workflow, eWorkflowType.Allocation), true);
                        break;

                    case eProfileType.MethodGeneralAllocationFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodGeneralAllocationSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGeneralAllocationSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.GeneralAllocation, eProfileType.MethodGeneralAllocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGeneralAllocation, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodAllocationOverrideFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodAllocationOverrideSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodAllocationOverrideSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.AllocationOverride, eProfileType.MethodAllocationOverride, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodAllocationOverride, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodRuleFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodRuleSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodRuleSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.Rule, eProfileType.MethodRule, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodRule, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodVelocityFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodVelocitySubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodVelocitySubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.Velocity, eProfileType.MethodVelocity, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodVelocity, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodBuildPacksFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodBuildPacksSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodBuildPacksSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.BuildPacks, eProfileType.MethodBuildPacks, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodBuildPacks, eWorkflowType.None), true);
                        break;

					case eProfileType.MethodGroupAllocationFolder:
						dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
						((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodGroupAllocationSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGroupAllocationSubFolder, eWorkflowType.None), eWorkflowType.None, true);
						((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.GroupAllocation, eProfileType.MethodGroupAllocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodGroupAllocation, eWorkflowType.None), true);
						break;

					// Begin TT#1652-MD - stodd - DC Carton Rounding
                    case eProfileType.MethodDCCartonRoundingFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodDCCartonRoundingSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodDCCartonRoundingSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.DCCartonRounding, eProfileType.MethodDCCartonRounding, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodDCCartonRounding, eWorkflowType.None), true);
                        break;
					// End TT#1652-MD - stodd - DC Carton Rounding

                    // Begin TT#1966-MD - JSmith- DC Fulfillment
                    case eProfileType.MethodCreateMasterHeadersFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodCreateMasterHeadersSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCreateMasterHeadersSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.CreateMasterHeaders, eProfileType.MethodCreateMasterHeaders, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodCreateMasterHeaders, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodDCFulfillmentFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodDCFulfillmentSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodDCFulfillmentSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.DCFulfillment, eProfileType.MethodDCFulfillment, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodDCFulfillment, eWorkflowType.None), true);
                        break;
                    // End TT#1966-MD - JSmith- DC Fulfillment

                    case eProfileType.MethodFillSizeHolesFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodFillSizeHolesSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodFillSizeHolesSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.FillSizeHolesAllocation, eProfileType.MethodFillSizeHolesAllocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodFillSizeHolesAllocation, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodBasisSizeFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodBasisSizeSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodBasisSizeSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.BasisSizeAllocation, eProfileType.MethodBasisSizeAllocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodBasisSizeAllocation, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodSizeNeedFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodSizeNeedSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodSizeNeedSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.SizeNeedAllocation, eProfileType.MethodSizeNeedAllocation, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodSizeNeedAllocation, eWorkflowType.None), true);
                        break;

                    case eProfileType.MethodSizeCurveFolder:
                        dtChildren = (DataTable)((WorkflowMethodTreeView)TreeView)._htChildren[GetGroupNode().Profile.Key];
                        ((WorkflowMethodTreeView)TreeView).AddFoldersToFolder(this, eProfileType.MethodSizeCurveSubFolder, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodSizeCurveSubFolder, eWorkflowType.None), eWorkflowType.None, true);
                        ((WorkflowMethodTreeView)TreeView).AddMethodsToFolder(this, eMethodType.SizeCurve, eProfileType.MethodSizeCurve, dtChildren, ((WorkflowMethodTreeView)TreeView).GetSecurity(UserId, eProfileType.MethodSizeCurve, eWorkflowType.None), true);
                        break;

                    default:
                        children = BuildFolderChildren();
                        Nodes.AddRange((MIDTreeNode[])children.ToArray(typeof(MIDTreeNode)));
                        break;
                }
                // End TT#1167
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#1167 - Login Performance - Opening the application after Login
        public MIDWorkflowMethodTreeNode GetGroupNode()
        {
            MIDWorkflowMethodTreeNode node;

            try
            {
                node = this;

                while (node.Parent != null)
                {
                    if (((MIDWorkflowMethodTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodOTSForcastFolder ||
                    ((MIDWorkflowMethodTreeNode)node).Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder)
                    {
                        break;
                    }
                    else
                    {
                        node = (MIDWorkflowMethodTreeNode)node.Parent;
                    }
                }

                return node;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#1167

        private ArrayList BuildFolderChildren()
        {
            switch (Profile.ProfileType)
            {
                case eProfileType.WorkflowMethodMainSharedFolder:
                    return BuildSharedChildren();
                default:
                    // Begin TT#33 - JSmith - No children in folder after copy
                    //return BuildFolderChildren(UserId, Profile.Key);
                    // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                    //return BuildFolderChildren(OwnerUserRID, Profile.Key);
                    if (this.GetTopNode().NodeProfileType == eProfileType.WorkflowMethodMainFavoritesFolder)
                    {
                        return BuildFolderChildren(OwnerUserRID, Profile.Key);
                    }
                    else
                    {
                        return BuildFolderChildren(UserId, Profile.Key);
                    }
                    // End Track #6302
                    // End TT#33
            }
        }

        private ArrayList BuildFolderChildren(int aUserRID, int aKey)
        {
            ArrayList children;
            FolderDataLayer dlFolder;
            DataTable dtFolder;
            eProfileType childItemType;
            int childItemRID, userRID, ownerUserRID;
            FolderProfile folderProf;
            MIDWorkflowMethodTreeNode newNode = null;
            FunctionSecurityProfile functionSecurity;
            bool isShortcut;

            children = new ArrayList();
            dlFolder = new FolderDataLayer();
            dtFolder = dlFolder.Folder_Children_Read(aUserRID, aKey);
            foreach (DataRow dr in dtFolder.Rows)
            {
                childItemType = (eProfileType)Convert.ToInt32(dr["CHILD_ITEM_TYPE"]);
                childItemRID = Convert.ToInt32(dr["CHILD_ITEM_RID"]);
                if (isShared)
                {
                    userRID = SAB.ClientServerSession.UserRID;
                }
                else
                {
                    userRID = Convert.ToInt32(dr["USER_RID"]);
                }
                ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                isShortcut = Include.ConvertCharToBool(Convert.ToChar(dr["SHORTCUT_IND"], CultureInfo.CurrentUICulture));

                if (isSubFolderType(childItemType))
                {
                    folderProf = new FolderProfile(childItemRID, userRID, childItemType, dlFolder.Folder_GetName(childItemRID), ownerUserRID);

                    // Begin TT#42 - JSmith - Explorers and My Favorites cannot create Folders and My Favorites Workflow/Methods not behaving as expected.
                    //functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(userRID, childItemType, eWorkflowType.None);
                    if (this.GetTopNode().TreeNodeType == eTreeNodeType.MainFavoriteFolderNode)
                    {
                        functionSecurity = ((WorkflowMethodTreeView)TreeView).FavoritesSecGrp.FolderSecurityProfile;
                    }
                    else
                    {
                        functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(userRID, childItemType, eWorkflowType.None);
                    }
                    // End TT#42
                    if (functionSecurity.AllowView)
                    {
                        newNode = ((WorkflowMethodTreeView)TreeView).BuildFolderNode(folderProf.Key,
                            (MIDWorkflowMethodTreeNode)this,
                            folderProf.UserRID,
                            folderProf.ProfileType,
                            folderProf.Name,
                            functionSecurity,
                            folderProf.OwnerUserRID,
                            eTreeNodeType.SubFolderNode,
                            isShortcut);

                        newNode.Sequence = 0;
                        // Begin TT#33 - JSmith - No children in folder after copy
                        //if (((MIDTreeView)TreeView).Folder_Children_Exists(userRID, newNode.Profile.Key))
                        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
                        if (this.GetTopNode().NodeProfileType == eProfileType.WorkflowMethodMainFavoritesFolder)
                        {
                            userRID = ownerUserRID;
                        }
                        if (((MIDTreeView)TreeView).Folder_Children_Exists(userRID, newNode.Profile.Key))
                        //if (((MIDTreeView)TreeView).Folder_Children_Exists(ownerUserRID, newNode.Profile.Key))
                        // End Track #6302
                        // End TT#33
                        {
                            newNode.Nodes.Add(((WorkflowMethodTreeView)TreeView).BuildPlaceHolderNode());
                            newNode.ChildrenLoaded = false;
                            newNode.HasChildren = true;
                            newNode.DisplayChildren = true;
                        }
                        else
                        {
                            newNode.ChildrenLoaded = true;
                        }
                        children.Add(newNode);
                    }
                }
                else if (isMethodProfileType((int)childItemType))
                {
                    // Begin TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                    //functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(userRID, (eProfileType)childItemType, eWorkflowType.None);
                    functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(ownerUserRID, (eProfileType)childItemType, eWorkflowType.None);
                    // End TT#4562 - JSmith - Null Reference error if filter in My Favorites but do not have security to view
                    if (functionSecurity.AllowView)
                    {
                        newNode = ((WorkflowMethodTreeView)TreeView).BuildMethodNode(childItemRID, this, (eMethodType)childItemType, functionSecurity, userRID, ownerUserRID, Profile.Key, isShortcut);
                        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
                        //newNode.Sequence = 4;
                        //children.Add(newNode);
                        if (newNode != null)
                        {
                            newNode.Sequence = 4;
                            children.Add(newNode);
                        }
                        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
                    }
                }
                else
                {
                    switch (childItemType)
                    {
                        case eProfileType.WorkflowMethodAllocationFolder:
                            if ((aUserRID == Include.GlobalUserRID && ((WorkflowMethodTreeView)TreeView).isGlobalAllocationAllowed) ||
                                (aUserRID != Include.GlobalUserRID && ((WorkflowMethodTreeView)TreeView).isUserAllocationAllowed))
                            {
                                newNode = ((WorkflowMethodTreeView)TreeView).BuildAllocationGroup((MIDWorkflowMethodTreeNode)this, childItemRID, userRID, ownerUserRID, isShortcut);
                                newNode.Sequence = 2;
                                if (newNode != null)
                                {
                                    children.Add(newNode);
                                }
                            }
                            break;
                        case eProfileType.WorkflowMethodOTSForcastFolder:
                            if ((aUserRID == Include.GlobalUserRID && ((WorkflowMethodTreeView)TreeView).isGlobalForecastingAllowed) ||
                                (aUserRID != Include.GlobalUserRID && ((WorkflowMethodTreeView)TreeView).isUserForecastingAllowed))
                            {
                                newNode = ((WorkflowMethodTreeView)TreeView).BuildOTSForecastGroup((MIDWorkflowMethodTreeNode)this, childItemRID, userRID, ownerUserRID, isShortcut);
                                newNode.Sequence = 1;
                                if (newNode != null)
                                {
                                    children.Add(newNode);
                                }
                            }
                            break;
                        case eProfileType.Workflow:
                            // Begin TT#2921 - JSmith - Security Question on Global Workflows
                            //functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(userRID, (eProfileType)childItemType, eWorkflowType.None);
                            eWorkflowType workflowType = eWorkflowType.None;
                            if (GetGroupNode().Profile.ProfileType == eProfileType.WorkflowMethodAllocationFolder)
                            {
                                workflowType = eWorkflowType.Allocation;
                            }
                            else
                            {
                                workflowType = eWorkflowType.Forecast;
                            }
                            functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(userRID, (eProfileType)childItemType, workflowType);
                            // End TT#2921 - JSmith - Security Question on Global Workflows
                            if (functionSecurity.AllowView)
                            {
                                newNode = ((WorkflowMethodTreeView)TreeView).BuildWorkflowNode(childItemRID, this, (eWorkflowType)childItemType, functionSecurity, userRID, ownerUserRID, Profile.Key, isShortcut);
                                newNode.Sequence = 3;
                                children.Add(newNode);
                            }
                            break;
                    }
                }
            }

            return children;
        }

        private ArrayList BuildSharedChildren()
        {
            ArrayList children;
            FolderDataLayer dlFolder;
            DataTable dtFolder;
            eProfileType childItemType;
            int childItemRID, userRID, ownerUserRID;
            FolderProfile folderProf;
            MIDWorkflowMethodTreeNode newNode = null;
            FunctionSecurityProfile functionSecurity;
            ArrayList userList;
            // Begin TT#63 - JSmith - Shared folder showing when nothing shared
            MethodBaseData dlMethodData;
            WorkflowBaseData dlWorkflowData;
            DataTable dtSharedMethods, dtSharedWorkflows;
            // End TT#63

            try
            {
                children = new ArrayList();
                dlFolder = new FolderDataLayer();
                // Begin TT#63 - JSmith - Shared folder showing when nothing shared
                dlMethodData = new MethodBaseData();
                dlWorkflowData = new WorkflowBaseData();
                // End TT#63
                userList = new ArrayList();
                userList.Add(SAB.ClientServerSession.UserRID);
                dtFolder = DlFolder.Folder_Read(userList, eProfileType.WorkflowMethodMainUserFolder, false, true);
                foreach (DataRow dr in dtFolder.Rows)
                {
                    childItemType = (eProfileType)Convert.ToInt32(dr["ITEM_TYPE"]);
                    childItemRID = Convert.ToInt32(dr["FOLDER_RID"]);
                    userRID = Convert.ToInt32(dr["USER_RID"]);
                    ownerUserRID = Convert.ToInt32(dr["OWNER_USER_RID"]);
                    // Begin TT#63 - JSmith - Shared folder showing when nothing shared
                    dtSharedMethods = dlMethodData.GetSharedMethods(SAB.ClientServerSession.UserRID, ownerUserRID, false);
                    dtSharedWorkflows = dlWorkflowData.GetSharedWorkflows(SAB.ClientServerSession.UserRID, ownerUserRID);
                    if (dtSharedMethods.Rows.Count > 0 ||
                        dtSharedWorkflows.Rows.Count > 0)
                    {
                        // End TT#63
                        folderProf = new FolderProfile(childItemRID, userRID, childItemType, GetUserName(ownerUserRID), ownerUserRID);
                        //functionSecurity = ((WorkflowMethodTreeView)TreeView).GetSecurity(ownerUserRID, childItemType, eWorkflowType.None);
                        functionSecurity = new FunctionSecurityProfile(childItemRID);
                        functionSecurity.SetReadOnly();

                        newNode = ((WorkflowMethodTreeView)TreeView).BuildFolderNode(folderProf.Key,
                                (MIDWorkflowMethodTreeNode)this,
                                folderProf.UserRID,
                                folderProf.ProfileType,
                                folderProf.Name,
                                functionSecurity,
                                folderProf.OwnerUserRID,
                                eTreeNodeType.MainSourceFolderNode,
                                isShortcut);

                        if (((MIDTreeView)TreeView).Folder_Children_Exists(folderProf.UserRID, newNode.Profile.Key))
                        {
                            newNode.Nodes.Add(BuildPlaceHolderNode());
                            newNode.ChildrenLoaded = false;
                            newNode.HasChildren = true;
                            newNode.DisplayChildren = true;
                        }
                        else
                        {
                            newNode.ChildrenLoaded = true;
                        }
                        children.Add(newNode);
                    // Begin TT#63 - JSmith - Shared folder showing when nothing shared
                    }
                    // End TT#63

                }
                return children;
            }
            catch
            {
                throw;
            }
        }
        private bool CheckMethodForInUse(int methodKey, eMethodTypeUI methodType, InUseInfo inUseInfo)
        {
            ScheduleData schedData;

            try
            {
                bool inUse = false;
                DataTable dtWorkflows = null;
                DataTable dtTaskLists = null;
                WorkflowBaseData workflowBaseData = new WorkflowBaseData();
                if (Enum.IsDefined(typeof(eAllocationMethodType), (int)methodType))
                    dtWorkflows = workflowBaseData.GetAllocMethodPropertiesUIWorkflows(methodKey);
                else if (Enum.IsDefined(typeof(eForecastMethodType), (int)methodType))
                    dtWorkflows = workflowBaseData.GetOTSMethodPropertiesUIWorkflows(methodKey);

                if (dtWorkflows != null)
                {
                    if (dtWorkflows.Rows.Count > 0)
                    {
                        inUse = true;
                        foreach (DataRow aRow in dtWorkflows.Rows)
                        {
                           inUseInfo.AddItem(aRow["WORKFLOW_NAME"].ToString(), aRow["WORKFLOW_DESCRIPTION"].ToString(), aRow["USER_FULLNAME"].ToString());
                        }
                    }
                }

                schedData = new ScheduleData();
                dtTaskLists = schedData.TaskForecastDetail_ReadByMethod(methodKey);

                if (dtTaskLists.Rows.Count > 0)
                {
                    inUse = true;
                    foreach (DataRow aRow in dtTaskLists.Rows)
                    {
                        inUseInfo.AddItem("*" + aRow["TASKLIST_NAME"].ToString(), "*" + aRow["TASKLIST_NAME"].ToString(), aRow["USER_NAME"].ToString());
                    }
                }

                return inUse;
            }
            catch
            {
                throw;
            }
        }

        private bool CheckWorkflowForInUse(int workflowKey, InUseInfo inUseInfo)
        {
            try
            {
                bool inUse = false;

                ScheduleData schedData = new ScheduleData();
                DataTable dtTaskLists = schedData.TaskForecastDetail_ReadByWorkflow(workflowKey);

                if (dtTaskLists.Rows.Count > 0)
                {
                    inUse = true;
                    foreach (DataRow aRow in dtTaskLists.Rows)
                    {
                        inUseInfo.AddItem(aRow["TASKLIST_NAME"].ToString(), aRow["TASKLIST_NAME"].ToString(), aRow["USER_NAME"].ToString());
                    }
                }

                return inUse;
            }
            catch
            {
                throw;
            }
        }

        public MIDWorkflowMethodTreeNode BuildPlaceHolderNode()
        {
            MIDWorkflowMethodTreeNode newNode;

            try
            {
                newNode = new MIDWorkflowMethodTreeNode(
                            SAB,
                            eTreeNodeType.ObjectNode,
                            new HierarchyNodeProfile(Include.NoRID),
                            null,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID,
                            new FunctionSecurityProfile(Include.NoRID),
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            Include.NoRID,
                            SAB.ClientServerSession.UserRID);


                return newNode;
            }
            catch
            {
                throw;
            }
        }
	}
}