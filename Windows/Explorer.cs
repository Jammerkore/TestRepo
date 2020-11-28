using System;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
 
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinToolbars;

//using MIDRetail.Windows.DemoViews;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class Explorer : System.Windows.Forms.Form
	{
		SessionAddressBlock _SAB;
		ExplorerAddressBlock _EAB;
		//bool _showLogin;
		bool _inNavigate = false;
		//		ClientSessionTransaction _ClientTransaction;
//		ApplicationSessionTransaction _AppTransaction;
//		System.Windows.Forms.Form _SecurityFrm;
//		System.Windows.Forms.Form _OptionsFrm;
//		System.Windows.Forms.Form _SizeGroupFrm;
//		System.Windows.Forms.Form _SizeCurveFrm;
//		System.Windows.Forms.Form _AllocSchedFrm;
//		System.Windows.Forms.Form _OTSPlanSchedFrm;
//		System.Windows.Forms.Form _StoreProfileFrm;
//		System.Windows.Forms.Form _StoreCharacteristicsFrm;

//		frmFilter _FilterFrm;
//		frmFilterWizard _FilterWizardFrm;
//		System.Windows.Forms.Form _ReleaseFrm;
//		System.Windows.Forms.Form _AuditFrm;
//		System.Windows.Forms.Form _TextEditorFrm;
//		frmScheduleBrowser _ScheduleBrowserFrm;
//		frmEligModelMaint			_EligModelsFrm;
//		frmStockModifierModelMaint	_StkModModelsFrm;
//		frmSalesModifierModelMaint	_SlsModModelsFrm;

        eMIDMessageLevel _currentActivityLevel = eMIDMessageLevel.Information;  // TT#46 MD - JSmith - User Dashboard

		private System.ComponentModel.IContainer components;

		// Controls for toolbars, menus, and fly-outs
		private Infragistics.Win.UltraWinDock.UltraDockManager udmMain;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ExplorerUnpinnedTabAreaLeft;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ExplorerUnpinnedTabAreaRight;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ExplorerUnpinnedTabAreaTop;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ExplorerUnpinnedTabAreaBottom;
		private Infragistics.Win.UltraWinDock.AutoHideControl _ExplorerAutoHideControl;
		
		private Infragistics.Win.UltraWinDock.DockableWindow dwAllocationWorkspaceExplorer;
		private Infragistics.Win.UltraWinDock.DockableWindow dwMerchandiseExplorer;
		private Infragistics.Win.UltraWinDock.DockableWindow dwWorkflowMethodExplorer;
		private Infragistics.Win.UltraWinDock.DockableWindow dwStoreGroupExplorer;
		private Infragistics.Win.UltraWinDock.DockableWindow dwAssortmentExplorer;
		private Infragistics.Win.UltraWinDock.DockableWindow dwFilterExplorerStore;
        private Infragistics.Win.UltraWinDock.DockableWindow dwFilterExplorerHeader; //TT#1313-MD -jsobek -Header Filters
        private Infragistics.Win.UltraWinDock.DockableWindow dwFilterExplorerAssortment; //TT#1313-MD -jsobek -Header Filters
        private Infragistics.Win.UltraWinDock.DockableWindow dwAssortmentWorkspace;     // TT#2 Asssortment Planning
        private Infragistics.Win.UltraWinDock.DockableWindow dwUserDashboard;  // TT#46 MD - JSmith - User Dashboard
		private Infragistics.Win.UltraWinDock.WindowDockingArea wdaMerchandiseExplorer;
		private Infragistics.Win.UltraWinDock.WindowDockingArea wdaAllocationWorkspaceExplorer;
        private Infragistics.Win.UltraWinDock.WindowDockingArea wdaAssortmentWorkspace; // TT#2 Asssortment Planning
        private Infragistics.Win.UltraWinDock.WindowDockingArea wdaUserDashboard;  // TT#46 MD - JSmith - User Dashboard
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea7;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea4;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea6;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea8;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea9;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea wdaAllocationWorkspaceExplorer0;
//		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea3;

		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager utmMain;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Explorer_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Explorer_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Explorer_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Explorer_Toolbars_Dock_Area_Bottom;

		private Infragistics.Shared.ResourceCustomizer _resourceCustomizer;

		private Infragistics.Win.UltraWinDock.DockableControlPane dcpAssortmentExplorer;
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpStoreExplorer;
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpAllocationWorkspace;
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpWorkflowMethodExplorer;
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpFilterExplorerStore;
        private Infragistics.Win.UltraWinDock.DockableControlPane dcpFilterExplorerHeader; // TT#1313-MD -jsobek -Header Filters
        private Infragistics.Win.UltraWinDock.DockableControlPane dcpFilterExplorerAssortment; // TT#1313-MD -jsobek -Header Filters
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpMerchandiseExplorer;
		private Infragistics.Win.UltraWinDock.DockableControlPane dcpTaskListExplorer;
        private Infragistics.Win.UltraWinDock.DockableControlPane dcpAssortmentWorkspace;
        private Infragistics.Win.UltraWinDock.DockableControlPane dcpUserDashboard;  // TT#46 MD - JSmith - User Dashboard
		
		// Custom MID Controls
		private MIDRetail.Windows.MerchandiseExplorer	meMerchandiseExplorer;
		private WorkflowMethodExplorer		tvwWorkflowMethodExplorer;
		//private MIDQuickFilter			quickFilter1;
		private MIDRetail.Windows.StoreGroupExplorer storeGroupExplorer1;
		private MIDRetail.Windows.AssortmentExplorer assortmentExplorer;
		private MIDRetail.Windows.TaskListExplorer taskListExplorer;
		private MIDRetail.Windows.AllocationWorkspaceExplorer AllocationWorkspaceExplorer1;
        private MIDRetail.Windows.AssortmentWorkspaceExplorer assortmentWorkspaceExplorer;
		private FilterStoreExplorer	filterExplorerStore1;
        private FilterHeaderExplorer filterExplorerHeader1;    //TT#1313-MD -jsobek -Header Filters
        private FilterAssortmentExplorer filterExplorerAssortment1;    //TT#1313-MD -jsobek -Header Filters
        private MIDRetail.Windows.UserActivityExplorer UserDashboardExplorer;  // TT#46 MD - JSmith - User Dashboard
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
		private OpenFileDialog ofdOpenCustom;
		private eLastHeaderSelection _lastHeaderSelection;
		StatusBarWindow _explorer_splash = new StatusBarWindow();	// TT#739 - MD - stodd - store delete
        bool _showSplashScreen = true;

		// Begin TT#2 - stodd - assortment
		public eLastHeaderSelection LastHeaderSelection
		{
			get { return _lastHeaderSelection; }
			set { _lastHeaderSelection = value; }
		}
		// End TT#2 - stodd - assortment


		public Explorer(SessionAddressBlock aSAB)
		{
            string strSetting = MIDConfigurationManager.AppSettings["ShowSplashScreen"];
            if (strSetting != null)
            {
                _showSplashScreen = Convert.ToBoolean(strSetting);
            }
            // Begin TT#1269 - gtaylor
            bool status_done = false;
            //StatusBarWindow explorer_splash = new StatusBarWindow();	// TT#739 - MD - stodd - store delete

            if (_showSplashScreen)
            {
                ThreadPool.QueueUserWorkItem((x) =>
                {
                    using (_explorer_splash)
                    {
                        _explorer_splash.Show();
                        Application.DoEvents();
                        while (!status_done)
                            Application.DoEvents();
                        _explorer_splash.Close();
                    }
                });

                _explorer_splash.StatusText = "Application Starting ...";
                _explorer_splash.StatusBarValue = 30;
                Application.DoEvents();
            }
            // End TT#1269

			_SAB = aSAB;
			//_showLogin = aShowLogin;

            // Begin TT#1269 - gtaylor
            if (_showSplashScreen)
            {
                _explorer_splash.StatusBarValue = 35;
                Application.DoEvents();
            }
            // End TT#1269 - gtaylor

//			_SecurityFrm = null;
//			_OptionsFrm = null;
//			_SizeGroupFrm = null;
//			_SizeCurveFrm = null;
//			_AllocSchedFrm = null;
//			_OTSPlanSchedFrm = null;
//			_StoreProfileFrm = null;
//			_StoreCharacteristicsFrm = null;
//			_FilterFrm = null;
//			_FilterWizardFrm = null;
//			_ScheduleBrowserFrm = null;
//			_ReleaseFrm = null;
//			_AuditFrm = null;


			// Create the client transaction
			//			_ClientTransaction = _SAB.ClientServerSession.CreateTransaction(); 

			// load the allocation headers
			//			_ClientTransaction.NewAllocationMasterProfileList();
			//			((AllocationProfileList)_ClientTransaction.GetMasterProfileList(eProfileType.Allocation)).LoadAll(_ClientTransaction);
			//
			//  moved to AllocationView requests
			//			_AppTransaction = _SAB.ApplicationServerSession.CreateTransaction(); 
			//			_AppTransaction.NewAllocationMasterProfileList();
			//// 			((AllocationProfileList)_AppTransaction.GetMasterProfileList(eProfileType.Allocation)).LoadAll(_AppTransaction);
			//			_AppTransaction.LoadAll();

			_EAB = new ExplorerAddressBlock();
			_EAB.Explorer = this;

            // Begin TT#1269 - gtaylor
            if (_showSplashScreen)
            {
                _explorer_splash.StatusBarValue = 40;
                Application.DoEvents();
            }
            // End TT#1269 - gtaylor

			InitializeComponent();
			this.IsMdiContainer = true;

            // Begin TT#TT#739-MD - JSmith - delete stores
            // prime the lists during startup so the firt call will not take long
            //ProfileList pl = StoreMgmt.GetActiveStoresList(); //_SAB.ApplicationServerSession.GetProfileList(eProfileType.Store); //TT#1517-MD -jsobek -Store Service Optimization
            // End TT#TT#739-MD - JSmith - delete stores

			Application.AddMessageFilter(new ExplorerMessageFilter());


            // Begin TT#698 - JSmith - Enhance environment information
            toolStripStatusLabel1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_User) + ":" + aSAB.ClientServerSession.GetUserName(aSAB.ClientServerSession.UserRID) + ";";
            System.Diagnostics.FileVersionInfo fvi = Include.GetMainAssemblyInfo();
            toolStripStatusLabel1.Text += MIDText.GetTextOnly(eMIDTextCode.lbl_EnvVersion) + ":" + fvi.ProductVersion;
			// Begin TT#2 - stodd - assortment
			#if (DEBUG)
                toolStripStatusLabel1.Text += ";" + MIDText.GetTextOnly(eMIDTextCode.lbl_Database) + ":" + _SAB.ClientServerSession.GetDatabaseName();
			#endif
			// End TT#2
            strSetting = MIDConfigurationManager.AppSettings["IncludeEnvironmentOnStatus"];
            if (strSetting != null)
            {
                if (Convert.ToBoolean(strSetting))
                {
                    toolStripStatusLabel1.Text += ";" + MIDText.GetTextOnly(eMIDTextCode.lbl_EnvEnvironment) + ":" + aSAB.ControlServerSession.GetMIDEnvironment();
                }
            }

            _SAB.MIDMenuEvent.OnMenuChangeHandler += new MIDMenuEvent.MenuChangeEventHandler(OnMenuChange);


            //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis - Grid Exporting
            MIDRetail.Windows.Controls.SharedControlRoutines.exportHelper =
                  new Controls.SharedControlRoutines.GridExportHelper(new Controls.SharedControlRoutines.ExportAllRowsToExcelDelegate(SharedRoutines.GridExport.ExportAllRowsToExcel),
                                                                      new Controls.SharedControlRoutines.ExportSelectedRowsToExcelDelegate(SharedRoutines.GridExport.ExportSelectedRowsToExcel),
                                                                      new Controls.SharedControlRoutines.EmailAllRowsDelegate(SharedRoutines.GridExport.EmailAllRows),
                                                                      new Controls.SharedControlRoutines.EmailSelectedRowsDelegate(SharedRoutines.GridExport.EmailSelectedRows)
                                                                      );
            //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis - Grid Exporting


            //Begin TT#827-MD -jsobek -Allocation Reviews Performance
            MIDText.PreLoadText();
            //End TT#827-MD -jsobek -Allocation Reviews Performance

            //Begin TT#1313-MD -jsobek -Header Filters -Set the Style and Parent of Style level names one time
            SetMerchandiseStyleAndParentofStyleLevelNames();
            //End TT#1313-MD -jsobek -Header Filters -Set the Style and Parent of Style level names one time

            // Begin TT#1808-MD - JSmith - Store Load Error
            //ExceptionHandler.SABforExceptions = _SAB; //TT#1313-MD -jsobek -Header Filters -Allow for common exception handling across application
            ExceptionHandler.Initialize(_SAB.ClientServerSession, true);
            // End TT#1808-MD - JSmith - Store Load Error

            ExceptionHandler.InDebugMode = getMIDOnlyFlag();
            if (ExceptionHandler.InDebugMode)
            if (SQLMonitorList.showFormAtStartup)
            {
                SQLMonitorList.PrepareDataTableForBinding();
                SQLMonitorForm f = new SQLMonitorForm();
                f.Show();
            }
            // Begin TT#1269 - gtaylor
            status_done = true;
            // End TT#1269
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();

            //Begin TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
            MIDRetail.Common.AssortmentActiveProcessToolbarHelper.IsAssortmentInstalled = _SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled;
            FunctionSecurityProfile _assortmentSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
			// BEGIN TT#488-MD - Stodd - Group Allocation
			// Because of GA, the Active Process toolbar is always available.
			//if (_assortmentSecurity.AccessDenied == true || _assortmentSecurity.AllowUpdate == false)
			//{
			//    MIDRetail.Common.AssortmentActiveProcessToolbarHelper.IsAssortmentActiveProcessAccessAllowed = false;
			//}
			//else
			//{
                MIDRetail.Common.AssortmentActiveProcessToolbarHelper.IsAssortmentActiveProcessAccessAllowed = true;
			//}
            //End TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek

			// Begin TT#2 - stodd - assortment
			Infragistics.Win.UltraWinDock.DockAreaPane dapAssortmentExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("56B45F8E-9320-4e6a-8F7F-2DF603BA6030"));
			dcpAssortmentExplorer = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("D6542228-7B26-4bbd-8E7F-1616003388A9"), new System.Guid("8FF94555-6911-463b-9DE9-158F9D974F9C"), 0, new System.Guid("56B45F8E-9320-4e6a-8F7F-2DF603BA6030"), 0);
			Infragistics.Win.Appearance dcpAssortmentExplorerAppearance = new Infragistics.Win.Appearance();
			// End TT#2 - stodd - assortment
			Infragistics.Win.UltraWinDock.DockAreaPane dapStoreExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("ec5cbb36-be05-44c2-a4ef-8eb92b7c2481"));
			dcpStoreExplorer = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("5cbfa006-5e49-4771-8328-d22a9263f908"), new System.Guid("626ef272-5cbc-45a1-96b0-943e6391c094"), 0, new System.Guid("ec5cbb36-be05-44c2-a4ef-8eb92b7c2481"), 0);
			Infragistics.Win.Appearance dcpStoreExplorerAppearance = new Infragistics.Win.Appearance();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Explorer));
			Infragistics.Win.UltraWinDock.DockAreaPane dapAllocationWorkspaceExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop, new System.Guid("5982dbca-1ed6-414a-8608-3e5bcc532594"));
			dcpAllocationWorkspace = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("270244e8-cf81-4b23-92e8-5c398b665c6f"), new System.Guid("852e5e8d-6474-4cd4-840b-8ab76d225731"), -1, new System.Guid("5982dbca-1ed6-414a-8608-3e5bcc532594"), -1);
			Infragistics.Win.Appearance dcpAllocationWorkspaceAppearance = new Infragistics.Win.Appearance();
			// Begin TT#2 - stodd - assortment
			Infragistics.Win.UltraWinDock.DockAreaPane dapAssortmentWorkspaceExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop, new System.Guid("80193C4F-2D40-4864-8AA8-FE7201EFB7EB"));
			dcpAssortmentWorkspace = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("8DD340B3-AA38-4355-88A2-CD90373F3F06"), new System.Guid("B110A8F1-72A8-441a-8EF1-010847222383"), -1, new System.Guid("{AD4C6B38-C787-4fce-9FAD-1A240E6290FB}"), -1);
			Infragistics.Win.Appearance dcpAssortmentWorkspaceAppearance = new Infragistics.Win.Appearance();
			// End TT#2 - stodd - assortment
            // Begin TT#46 MD - JSmith - User Dashboard
            Infragistics.Win.UltraWinDock.DockAreaPane dapUserDashboardExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedTop, new System.Guid("a60233a6-f3d9-4096-a53f-87d223232ec1"));
            dcpUserDashboard = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("14c2e226-3249-48b4-8efb-efff0f16c2d9"), new System.Guid("5c4ec7f5-4923-4f55-a407-c2cdcc507a32"), -1, new System.Guid("{fa96056e-ab11-444c-a9f1-06de6ff5824e}"), -1);
            Infragistics.Win.Appearance dcpUserDashboardAppearance = new Infragistics.Win.Appearance();
            // End TT#46 MD - JSmith - User Dashboard
			Infragistics.Win.UltraWinDock.DockAreaPane dapWorkflowMethodExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("a663b80f-c675-495f-8572-982fe011cbca"));
			dcpWorkflowMethodExplorer = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("d0f0d51a-8337-44ff-9269-b3981180c94e"), new System.Guid("5188137e-5dcb-4f95-8cf8-ec03ea61004b"), -1, new System.Guid("a663b80f-c675-495f-8572-982fe011cbca"), 4);
			Infragistics.Win.Appearance dcpWorkflowMethodExplorerAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinDock.DockAreaPane dapTasklistExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("852e5e8d-6474-4cd4-840b-8ab76d225731"));
			dcpTaskListExplorer = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("a9cc7f64-a6fe-47bc-b32c-f3523ecea465"), new System.Guid("1623d567-744a-44f8-8b74-e348caff8d37"), 0, new System.Guid("0d0459db-5e84-4f58-9fd5-4ae6ea8ea792"), 0);
			Infragistics.Win.Appearance dcpTaskListExplorerAppearance = new Infragistics.Win.Appearance();
//			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane5 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("037897e4-a186-44c0-a406-4ff3db5bd3a8"));
//			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane6 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("5188137e-5dcb-4f95-8cf8-ec03ea61004b"));
//			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane7 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("680061dc-aa6c-403d-a4a3-2852e2cba4b2"));
//			Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane8 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.Floating, new System.Guid("626ef272-5cbc-45a1-96b0-943e6391c094"));
			Infragistics.Win.UltraWinDock.DockAreaPane dapFilterExplorerStore = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("97a5f127-b6db-467d-827e-9010e43e42f1"));
            Infragistics.Win.UltraWinDock.DockAreaPane dapFilterExplorerHeader = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("97a5f128-b6db-467d-827e-9010e43e42f1")); //TT#1313-MD -jsobek -Header Filters
            Infragistics.Win.UltraWinDock.DockAreaPane dapFilterExplorerAssortment = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("97a5f129-b6db-467d-827e-9010e43e42f1")); //TT#1313-MD -jsobek -Header Filters
            dcpFilterExplorerStore = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("4f5fdece-4e5e-43bf-a031-8dc044efa223"), new System.Guid("680061dc-aa6c-403d-a4a3-2852e2cba4b2"), -1, new System.Guid("97a5f127-b6db-467d-827e-9010e43e42f1"), 0);
            dcpFilterExplorerHeader = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("4f5fdece-4e5e-43bf-a031-8dc044efa224"), new System.Guid("680061dc-aa6c-403d-a4a3-2852e2cba4b3"), -1, new System.Guid("97a5f128-b6db-467d-827e-9010e43e42f1"), 0); //TT#1313-MD -jsobek -Header Filters
            dcpFilterExplorerAssortment = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("4f5fdece-4e5e-43bf-a031-8dc044efa225"), new System.Guid("680061dc-aa6c-403d-a4a3-2852e2cba4b4"), -1, new System.Guid("97a5f129-b6db-467d-827e-9010e43e42f1"), 0); //TT#1313-MD -jsobek -Header Filters
            Infragistics.Win.Appearance dcpFilterExplorerAppearanceStore = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance dcpFilterExplorerAppearanceHeader = new Infragistics.Win.Appearance(); //TT#1313-MD -jsobek -Header Filters
            Infragistics.Win.Appearance dcpFilterExplorerAppearanceAssortment = new Infragistics.Win.Appearance(); //TT#1313-MD -jsobek -Header Filters
            Infragistics.Win.UltraWinDock.DockAreaPane dapMerchandiseExplorer = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("3f23e818-fa26-4b49-987f-abee33deb980"));
			dcpMerchandiseExplorer = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("db7a67af-7f24-4247-995e-6c6845f023fb"), new System.Guid("037897e4-a186-44c0-a406-4ff3db5bd3a8"), 0, new System.Guid("3f23e818-fa26-4b49-987f-abee33deb980"), 3);
			Infragistics.Win.Appearance dcpMerchandiseExplorerAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.OptionSet optionSet1 = new Infragistics.Win.UltraWinToolbars.OptionSet(Include.tbExplorers);
			Infragistics.Win.UltraWinToolbars.UltraToolbar utMainMenu = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Main Menu");
			Infragistics.Win.UltraWinToolbars.UltraToolbar utExplorers = new Infragistics.Win.UltraWinToolbars.UltraToolbar(Include.tbExplorers);
			Infragistics.Win.UltraWinToolbars.UltraToolbar utAnalysis = new Infragistics.Win.UltraWinToolbars.UltraToolbar(Include.tbAnalysis);
			Infragistics.Win.UltraWinToolbars.UltraToolbar utTools = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Tools");
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolOTSPlan = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbOTSPlan);
			Infragistics.Win.Appearance btnToolOTSPlanAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolAllocationSummary = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbSummary);
			Infragistics.Win.Appearance btnToolAllocationSummaryAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolStyleView = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbStyle);
			Infragistics.Win.Appearance btnStyleViewAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolWorkspace = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbAllocationWorkspace);
			Infragistics.Win.Appearance btnToolWorkspaceAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolStoreProfiles = new Infragistics.Win.UltraWinToolbars.ButtonTool("Store Profiles");
			Infragistics.Win.Appearance btnToolStoreProfilesAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolStoreFilters = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbStoreFilters);
			Infragistics.Win.Appearance btnToolStoreFiltersAppearance = new Infragistics.Win.Appearance();

            //Begin TT#1313-MD -jsobek -Header Filters
            Infragistics.Win.UltraWinToolbars.ButtonTool btnToolHeaderFilters = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbHeaderFilters); 
            Infragistics.Win.Appearance btnToolHeaderFiltersAppearance = new Infragistics.Win.Appearance(); 

            Infragistics.Win.UltraWinToolbars.ButtonTool btnToolAssortmentFilters = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbAssortmentFilters); 
            Infragistics.Win.Appearance btnToolAssortmentFiltersAppearance = new Infragistics.Win.Appearance(); 
            //End TT#1313-MD -jsobek -Header Filters

			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolMerchandiseExplorer = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbMerchandise);
			Infragistics.Win.Appearance btnToolMerchandiseExplorerAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolWorkflowMethodExplorer = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbWorkflowMethods);
			Infragistics.Win.Appearance btnToolWorkflowMethodExplorerAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolStoreExplorer = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbStores);
			Infragistics.Win.Appearance btnToolStoreExplorerAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolTaskListExplorer = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbTaskLists);
			Infragistics.Win.Appearance btnToolTaskListExplorerAppearance = new Infragistics.Win.Appearance();
//			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolScheduler = new Infragistics.Win.UltraWinToolbars.ButtonTool("Scheduling");
//			Infragistics.Win.Appearance btnToolSchedulerAppearance = new Infragistics.Win.Appearance();
			// Begin TT#2 Assortment Planning
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolAssortment = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbAssortment);
			Infragistics.Win.Appearance btnToolAssortmentAppearance = new Infragistics.Win.Appearance();
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolAssortmentExplorer = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbAssortmentExplorer);
			Infragistics.Win.Appearance btnToolAssortmentExplorerAppearance = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool btnToolAssortmentWorkspace = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbAssortmentWorkspace);
            Infragistics.Win.Appearance btnToolAssortmentWorkspaceAppearance = new Infragistics.Win.Appearance();
            // End TT#2
			// BEGIN TT#488-MD - STodd - Group Allocation - 
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolGroupAllocation = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbGroupAllocation);
			Infragistics.Win.Appearance btnToolGroupAllocationAppearance = new Infragistics.Win.Appearance();
			// END TT#488-MD - STodd - Group Allocation - 
            // Begin TT#46 MD - JSmith - User Dashboard
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolUserDashboard = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbUserActivityExplorer);
            Infragistics.Win.Appearance btnToolUserDashboardAppearance = new Infragistics.Win.Appearance();
            // End TT#46 MD - JSmith - User Dashboard
			Infragistics.Win.UltraWinToolbars.ButtonTool btnToolOTSPlan2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Header Characteristics");
			Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Filter", Include.tbExplorers);
			// Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("Quick Filter");
			// BEGIN TT#739 - MD - stodd - delete stores
            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Store Explorer ...";
                _explorer_splash.StatusBarValue = 35;
                Application.DoEvents();
            }
			this.storeGroupExplorer1 = new StoreGroupExplorer(_SAB, _EAB, this);
            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Task List Explorer ...";
                _explorer_splash.StatusBarValue = 40;
                Application.DoEvents();
            }
			this.taskListExplorer = new TaskListExplorer(_SAB, _EAB, this);

            //this.storeGroupExplorer1 = new StoreGroupExplorer(_SAB, _EAB, this); //TT#1584-MD -jsobek -Store Group Explorer Instantiated Twice

			// Begin TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
            //_explorer_splash.StatusText = "Building Allocation Workspace Explorer ...";
            //_explorer_splash.StatusBarValue = 45;
            //Application.DoEvents();
			// End TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
            this.AllocationWorkspaceExplorer1 = new AllocationWorkspaceExplorer(_SAB, _EAB, this);
            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Workflow Methods Explorer ...";
                _explorer_splash.StatusBarValue = 55;
                Application.DoEvents();
            }
			this.tvwWorkflowMethodExplorer = new WorkflowMethodExplorer(_SAB, _EAB, this);
            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Filter Store Explorer ...";
                _explorer_splash.StatusBarValue = 65;
                Application.DoEvents();
            }
			this.filterExplorerStore1 = new FilterStoreExplorer(_SAB, _EAB, this);

            //Begin TT#1313-MD -jsobek -Header Filters
            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Filter Header Explorer ...";
                _explorer_splash.StatusBarValue = 70;
                Application.DoEvents();
            }
            this.filterExplorerHeader1 = new FilterHeaderExplorer(_SAB, _EAB, this);
            //End TT#1313-MD -jsobek -Header Filters

            if (_showSplashScreen)
            {
                _explorer_splash.StatusText = "Building Merchandise Explorer ...";
                _explorer_splash.StatusBarValue = 75;
                Application.DoEvents();
            }
			this.meMerchandiseExplorer = new MerchandiseExplorer(_SAB, _EAB, this);
			Assembly assem = Assembly.GetEntryAssembly();
			AssemblyName assemName = assem.GetName();
			Version ver = assemName.Version;
            //Begin TT#1313-MD -jsobek -Header Filters
            if (ver.Major >= 5 && ver.MajorRevision >= 3)
            {
                if (_showSplashScreen)
                {
                    _explorer_splash.StatusText = "Building Filter Assortment Explorer ...";
                    _explorer_splash.StatusBarValue = 77;
                    Application.DoEvents();
                }
            }
            this.filterExplorerAssortment1 = new FilterAssortmentExplorer(_SAB, _EAB, this);
            //End TT#1313-MD -jsobek -Header Filters
			if (ver.Major >= 5 && ver.MajorRevision >= 3)
			{
                if (_showSplashScreen)
                {
                    _explorer_splash.StatusText = "Building Assortment Explorer ...";
                    _explorer_splash.StatusBarValue = 78;
                    Application.DoEvents();
                }
			}
			this.assortmentExplorer = new AssortmentExplorer(_SAB, _EAB, this); // Track 5835
			if (ver.Major >= 5 && ver.MajorRevision >= 3)
			{
                if (_showSplashScreen)
                {
                    _explorer_splash.StatusText = "Building Assortment Workspace Explorer ...";
                    _explorer_splash.StatusBarValue = 80;
                    Application.DoEvents();
                }
			}
			// END TT#739 - MD - stodd - delete stores
            this.assortmentWorkspaceExplorer = new AssortmentWorkspaceExplorer(_SAB, _EAB, this); // TT#2 Assortment Planning
            this.UserDashboardExplorer = new UserActivityExplorer(_SAB, _EAB, this);  // TT#46 MD - JSmith - User Dashboard
			//this.quickFilter1 = new MIDQuickFilter();
			this.udmMain = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
			this._ExplorerUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._ExplorerUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._ExplorerUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._ExplorerUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
			this._ExplorerAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
			this.dwMerchandiseExplorer = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dwFilterExplorerStore = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dwFilterExplorerHeader = new Infragistics.Win.UltraWinDock.DockableWindow(); //TT#1313-MD -jsobek -Header Filters
			this.dwWorkflowMethodExplorer = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dwAllocationWorkspaceExplorer = new Infragistics.Win.UltraWinDock.DockableWindow();
			this.dwStoreGroupExplorer = new Infragistics.Win.UltraWinDock.DockableWindow();
			// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
                this.dwFilterExplorerAssortment = new Infragistics.Win.UltraWinDock.DockableWindow(); //TT#1313-MD -jsobek -Header Filters
				this.dwAssortmentExplorer = new Infragistics.Win.UltraWinDock.DockableWindow();
				this.dwAssortmentWorkspace = new Infragistics.Win.UltraWinDock.DockableWindow();
			}
			// End TT#2 - stodd Assortment
            this.dwUserDashboard = new Infragistics.Win.UltraWinDock.DockableWindow();  // TT#46 MD - JSmith - User Dashboard
			this.utmMain = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
			this._Explorer_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._Explorer_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._Explorer_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this._Explorer_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
			this.wdaMerchandiseExplorer = new Infragistics.Win.UltraWinDock.WindowDockingArea();
			this.wdaAllocationWorkspaceExplorer = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.wdaAssortmentWorkspace = new Infragistics.Win.UltraWinDock.WindowDockingArea();    // TT#2 Assortment Planning
            this.wdaUserDashboard = new Infragistics.Win.UltraWinDock.WindowDockingArea();  // TT#46 MD - JSmith - User Dashboard
			((System.ComponentModel.ISupportInitialize)(this.udmMain)).BeginInit();
			this._ExplorerAutoHideControl.SuspendLayout();
			this.dwMerchandiseExplorer.SuspendLayout();
			this.dwFilterExplorerStore.SuspendLayout();
            this.dwFilterExplorerHeader.SuspendLayout(); //TT#1313-MD -jsobek -Header Filters
			this.dwWorkflowMethodExplorer.SuspendLayout();
			this.dwAllocationWorkspaceExplorer.SuspendLayout();
			this.dwStoreGroupExplorer.SuspendLayout();
			// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
                this.dwFilterExplorerAssortment.SuspendLayout(); //TT#1313-MD -jsobek -Header Filters
				this.dwAssortmentExplorer.SuspendLayout();
				this.dwAssortmentWorkspace.SuspendLayout(); // TT#2 Assortment Planning
			}
			// End TT#2 - stodd Assortment
            this.dwUserDashboard.SuspendLayout();  // TT#46 MD - JSmith - User Dashboard
			this.ofdOpenCustom = new OpenFileDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.wdaMerchandiseExplorer.SuspendLayout();
			this.SuspendLayout();
			// 
			// storeGroupExplorer1
			// 
			this.storeGroupExplorer1.Location = new System.Drawing.Point(0, 23);
			this.storeGroupExplorer1.Name = "storeGroupExplorer1";
			this.storeGroupExplorer1.Size = new System.Drawing.Size(302, 600);
			this.storeGroupExplorer1.TabIndex = 25;
			// 
			// taskListExplorer
			// 
			this.taskListExplorer.Location = new System.Drawing.Point(0, 23);
			this.taskListExplorer.Name = "taskListExplorer";
			this.taskListExplorer.Size = new System.Drawing.Size(302, 600);
			this.taskListExplorer.TabIndex = 29;
			// 
			// AllocationWorkspaceExplorer1
			// 
			this.AllocationWorkspaceExplorer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.AllocationWorkspaceExplorer1.Location = new System.Drawing.Point(300, 0);
			this.AllocationWorkspaceExplorer1.Name = "AllocationWorkspaceExplorer1";
			this.AllocationWorkspaceExplorer1.Size = new System.Drawing.Size(600, 200);    // doesn't make any difference
			this.AllocationWorkspaceExplorer1.TabIndex = 12;
			// 
			// tvwWorkflowMethodExplorer
			// 
			this.tvwWorkflowMethodExplorer.Location = new System.Drawing.Point(0, 23);
			this.tvwWorkflowMethodExplorer.Name = "tvwWorkflowMethodExplorer";
			this.tvwWorkflowMethodExplorer.Size = new System.Drawing.Size(279, 600);
			this.tvwWorkflowMethodExplorer.TabIndex = 11;
			// 
			// filterExplorerStore1
			// 
			this.filterExplorerStore1.Location = new System.Drawing.Point(0, 23);
			this.filterExplorerStore1.Name = "filterExplorerStore1";
			this.filterExplorerStore1.Size = new System.Drawing.Size(246, 600);
			this.filterExplorerStore1.TabIndex = 26;
            // 
            // filterExplorerHeader1
            // 
            this.filterExplorerHeader1.Location = new System.Drawing.Point(0, 23);
            this.filterExplorerHeader1.Name = "filterExplorerHeader1";
            this.filterExplorerHeader1.Size = new System.Drawing.Size(246, 600);
            this.filterExplorerHeader1.TabIndex = 26;
			// 
			// meMerchandiseExplorer
			// 
			this.meMerchandiseExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
			this.meMerchandiseExplorer.Location = new System.Drawing.Point(0, 23);
			this.meMerchandiseExplorer.Name = "meMerchandiseExplorer";
			this.meMerchandiseExplorer.Size = new System.Drawing.Size(292, 600);
			this.meMerchandiseExplorer.TabIndex = 10;
						// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
				// 
				// assortmentExplorer
				// 
				this.assortmentExplorer.Location = new System.Drawing.Point(100, 100);
				this.assortmentExplorer.Name = "assortmentExplorer";
				this.assortmentExplorer.Size = new System.Drawing.Size(246, 600);
				this.assortmentExplorer.TabIndex = 27;
				// 
				// assortmentWorkspaceExplorer
				// 
				this.assortmentWorkspaceExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
				this.assortmentWorkspaceExplorer.Location = new System.Drawing.Point(400, 0);
				this.assortmentWorkspaceExplorer.Name = "AssortmentWorkspaceExplorer";
				this.assortmentWorkspaceExplorer.Size = new System.Drawing.Size(600, 200);
				this.assortmentWorkspaceExplorer.TabIndex = 13;

				// quickFilter1
				// 
				//			this.quickFilter1.Location = new System.Drawing.Point(904, 72);
				//			this.quickFilter1.Location = new System.Drawing.Point(492, 23);
				//			this.quickFilter1.Name = "quickFilter1";
				//			this.quickFilter1.Size = new System.Drawing.Size(288, 24);
				//			this.quickFilter1.TabIndex = 19;
				// 
				// udmMain
				// 
				dcpAssortmentExplorer.Control = this.assortmentExplorer;
				dcpAssortmentExplorer.FlyoutSize = new System.Drawing.Size(280, -1);
				dcpAssortmentExplorer.Key = Include.tbbAssortmentExplorer;
				dcpAssortmentExplorer.OriginalControlBounds = new System.Drawing.Rectangle(968, 128, 216, 352);
				dcpAssortmentExplorer.Pinned = false;
				dcpAssortmentExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
				dcpAssortmentExplorer.Settings.Appearance = dcpAssortmentExplorerAppearance;
				dcpAssortmentExplorer.Size = new System.Drawing.Size(316, 599);
				dcpAssortmentExplorer.Text = "Assortment Explorer";
				dcpAssortmentExplorer.TextTab = "Assortment Explorer";
				dcpAssortmentExplorer.ToolTipCaption = "Assortment Explorer";
				dcpAssortmentExplorer.ToolTipTab = "Assortment Explorer";
				dapAssortmentExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpAssortmentExplorer });
				dapAssortmentExplorer.Size = new System.Drawing.Size(210, 599);
			}
			// End TT#2 - stodd Assortment
            // Begin TT#46 MD - JSmith - User Dashboard
            // UserDashboardExplorer
            // 
            this.UserDashboardExplorer.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.UserDashboardExplorer.Location = new System.Drawing.Point(400, 0);
            this.UserDashboardExplorer.Name = "UserDashboardExplorer";
            this.UserDashboardExplorer.Size = new System.Drawing.Size(600, 200);
            this.UserDashboardExplorer.TabIndex = 13;
            // End TT#46 MD
			dcpStoreExplorer.Control = this.storeGroupExplorer1;
			dcpStoreExplorer.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpStoreExplorer.Key = Include.tbbStores;
			dcpStoreExplorer.OriginalControlBounds = new System.Drawing.Rectangle(968, 128, 216, 352);
			dcpStoreExplorer.Pinned = false;
			dcpStoreExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.StoreImage);
			dcpStoreExplorer.Settings.Appearance = dcpStoreExplorerAppearance;
			dcpStoreExplorer.Size = new System.Drawing.Size(316, 599);
			dcpStoreExplorer.Text = "Store Explorer";
			dcpStoreExplorer.TextTab = "Store Explorer";
			dcpStoreExplorer.ToolTipCaption = "Store  Explorer";
			dcpStoreExplorer.ToolTipTab = "Store  Explorer";
			dapStoreExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpStoreExplorer});
			dapStoreExplorer.Size = new System.Drawing.Size(210, 599);
			dapAllocationWorkspaceExplorer.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
			dapAllocationWorkspaceExplorer.FloatingLocation = new System.Drawing.Point(102, 420);
			dcpAllocationWorkspace.Control = this.AllocationWorkspaceExplorer1;
			dcpAllocationWorkspace.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpAllocationWorkspace.Key = Include.tbbAllocationWorkspace;
			dcpAllocationWorkspace.OriginalControlBounds = new System.Drawing.Rectangle(472, 112, 240, 392);
			dcpAllocationWorkspace.Pinned = false;
			dcpAllocationWorkspaceAppearance.Image = MIDGraphics.GetImage(MIDGraphics.WorkspaceImage);
			dcpAllocationWorkspace.Settings.Appearance = dcpAllocationWorkspaceAppearance;
			dcpAllocationWorkspace.Size = new System.Drawing.Size(600, 200);    
			dcpAllocationWorkspace.Text = "Allocation Workspace Explorer";
			dcpAllocationWorkspace.TextTab = "Allocation Workspace Explorer";
			dcpAllocationWorkspace.ToolTipCaption = "Allocation Workspace Explorer";
			dcpAllocationWorkspace.ToolTipTab = "Allocation Workspace Explorer";
			dapAllocationWorkspaceExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpAllocationWorkspace});
			dapAllocationWorkspaceExplorer.Size = new System.Drawing.Size(600, 300);     // really affects the size

			dapWorkflowMethodExplorer.FloatingLocation = new System.Drawing.Point(413, 255);
			dcpWorkflowMethodExplorer.Control = this.tvwWorkflowMethodExplorer;
			dcpWorkflowMethodExplorer.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpWorkflowMethodExplorer.Key = Include.tbbWorkflowMethods;
			dcpWorkflowMethodExplorer.OriginalControlBounds = new System.Drawing.Rectangle(504, 184, 232, 424);
			dcpWorkflowMethodExplorer.Pinned = false;
			dcpWorkflowMethodExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.MethodsImage);
			dcpWorkflowMethodExplorer.Settings.Appearance = dcpWorkflowMethodExplorerAppearance;
			dcpWorkflowMethodExplorer.Size = new System.Drawing.Size(316, 599);
			dcpWorkflowMethodExplorer.Text = "Workflow/Method Explorer";
			dcpWorkflowMethodExplorer.TextTab = "Workflow/Method Explorer";
			dcpWorkflowMethodExplorer.ToolTipCaption = "Workflow/Method Explorer";
			dcpWorkflowMethodExplorer.ToolTipTab = "Workflow/Method Explorer";
			dapWorkflowMethodExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpWorkflowMethodExplorer});
			dapWorkflowMethodExplorer.Size = new System.Drawing.Size(278, 623);
			dcpTaskListExplorer.Control = this.taskListExplorer;
			dcpTaskListExplorer.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpTaskListExplorer.Key = Include.tbbTaskLists;
			dcpTaskListExplorer.OriginalControlBounds = new System.Drawing.Rectangle(800, 128, 216, 352);
			dcpTaskListExplorer.Pinned = false;
			dcpTaskListExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.TaskListImage);
			dcpTaskListExplorer.Settings.Appearance = dcpTaskListExplorerAppearance;
			dcpTaskListExplorer.Size = new System.Drawing.Size(316, 599);
			dcpTaskListExplorer.Text = "Task List Explorer";
			dcpTaskListExplorer.TextTab = "Task List Explorer";
			dcpTaskListExplorer.ToolTipCaption = "Task List Explorer";
			dcpTaskListExplorer.ToolTipTab = "Task List Explorer";
			dapTasklistExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {dcpTaskListExplorer});
			dapTasklistExplorer.Size = new System.Drawing.Size(210, 599);
			dapFilterExplorerStore.FloatingLocation = new System.Drawing.Point(682, 255);
			dcpFilterExplorerStore.Control = this.filterExplorerStore1;
			dcpFilterExplorerStore.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpFilterExplorerStore.Key = Include.tbbStoreFilters;
			dcpFilterExplorerStore.OriginalControlBounds = new System.Drawing.Rectangle(728, 120, 232, 480);
			dcpFilterExplorerStore.Pinned = false;
            //Begin TT#1313-MD -jsobek -Header Filters
            dapFilterExplorerHeader.FloatingLocation = new System.Drawing.Point(682, 255);
            dcpFilterExplorerHeader.Control = this.filterExplorerHeader1;
            dcpFilterExplorerHeader.FlyoutSize = new System.Drawing.Size(280, -1);
            dcpFilterExplorerHeader.Key = Include.tbbHeaderFilters;
            dcpFilterExplorerHeader.OriginalControlBounds = new System.Drawing.Rectangle(728, 120, 232, 480);
            dcpFilterExplorerHeader.Pinned = false;
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                dapFilterExplorerAssortment.FloatingLocation = new System.Drawing.Point(682, 255);
                dcpFilterExplorerAssortment.Control = this.filterExplorerAssortment1;
                dcpFilterExplorerAssortment.FlyoutSize = new System.Drawing.Size(280, -1);
                dcpFilterExplorerAssortment.Key = Include.tbbAssortmentFilters;
                dcpFilterExplorerAssortment.OriginalControlBounds = new System.Drawing.Rectangle(728, 120, 232, 480);
                dcpFilterExplorerAssortment.Pinned = false;
            }
            //End TT#1313-MD -jsobek -Header Filters
			dcpFilterExplorerAppearanceStore.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage);
            dcpFilterExplorerAppearanceHeader.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage); //TT#1313-MD -jsobek -Header Filters
            dcpFilterExplorerAppearanceAssortment.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage); //TT#1313-MD -jsobek -Header Filters
            dcpFilterExplorerStore.Settings.Appearance = dcpFilterExplorerAppearanceStore;
			dcpFilterExplorerStore.Size = new System.Drawing.Size(316, 599);
			dcpFilterExplorerStore.Text = "Store Filters";
            dcpFilterExplorerStore.TextTab = "Store Filters";
            dcpFilterExplorerStore.ToolTipCaption = "Store Filters";
            dcpFilterExplorerStore.ToolTipTab = "Store Filters";
			dapFilterExplorerStore.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpFilterExplorerStore});
			dapFilterExplorerStore.Size = new System.Drawing.Size(293, 623);
            //Begin TT#1313-MD -jsobek -Header Filters
            dcpFilterExplorerHeader.Settings.Appearance = dcpFilterExplorerAppearanceHeader;
            dcpFilterExplorerHeader.Size = new System.Drawing.Size(316, 599);
            dcpFilterExplorerHeader.Text = "Header Filters";
            dcpFilterExplorerHeader.TextTab = "Header Filters";
            dcpFilterExplorerHeader.ToolTipCaption = "Header Filters";
            dcpFilterExplorerHeader.ToolTipTab = "Header Filters";
            dapFilterExplorerHeader.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpFilterExplorerHeader });
            dapFilterExplorerHeader.Size = new System.Drawing.Size(293, 623);
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                dcpFilterExplorerAssortment.Settings.Appearance = dcpFilterExplorerAppearanceAssortment;
                dcpFilterExplorerAssortment.Size = new System.Drawing.Size(316, 599);
                dcpFilterExplorerAssortment.Text = "Assortment Filters";
                dcpFilterExplorerAssortment.TextTab = "Assortment Filters";
                dcpFilterExplorerAssortment.ToolTipCaption = "Assortment Filters";
                dcpFilterExplorerAssortment.ToolTipTab = "Assortment Filters";
                dapFilterExplorerAssortment.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpFilterExplorerAssortment });
                dapFilterExplorerAssortment.Size = new System.Drawing.Size(293, 623);
            }
            //End TT#1313-MD -jsobek -Header Filters
			dapMerchandiseExplorer.FloatingLocation = new System.Drawing.Point(615, 557);
			dcpMerchandiseExplorer.Control = this.meMerchandiseExplorer;
			dcpMerchandiseExplorer.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpMerchandiseExplorer.Key = Include.tbbMerchandise;
			dcpMerchandiseExplorer.OriginalControlBounds = new System.Drawing.Rectangle(760, 192, 232, 344);
			dcpMerchandiseExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.HierarchyExplorerImage);
			dcpMerchandiseExplorer.Settings.Appearance = dcpMerchandiseExplorerAppearance;
			dcpMerchandiseExplorer.Size = new System.Drawing.Size(316, 599);
			dcpMerchandiseExplorer.Text = "Merchandise Explorer";
			dcpMerchandiseExplorer.TextTab = "Merchandise Explorer";
			dcpMerchandiseExplorer.ToolTipCaption = "Merchandise Explorer";
			dcpMerchandiseExplorer.ToolTipTab = "Merchandise Explorer";
			dapMerchandiseExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpMerchandiseExplorer});
			dapMerchandiseExplorer.Size = new System.Drawing.Size(292, 623);
		    // Begin TT#2 Assortment Planning
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
				dapAssortmentWorkspaceExplorer.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
				dapAssortmentWorkspaceExplorer.FloatingLocation = new System.Drawing.Point(102, 420);
				dcpAssortmentWorkspace.Control = this.assortmentWorkspaceExplorer;
				dcpAssortmentWorkspace.FlyoutSize = new System.Drawing.Size(280, -1);
				dcpAssortmentWorkspace.Key = Include.tbbAssortmentWorkspace;
				dcpAssortmentWorkspace.OriginalControlBounds = new System.Drawing.Rectangle(472, 112, 240, 392);
				dcpAssortmentWorkspace.Pinned = false;
				dcpAssortmentWorkspaceAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
				dcpAssortmentWorkspace.Settings.Appearance = dcpAssortmentWorkspaceAppearance;
				dcpAssortmentWorkspace.Size = new System.Drawing.Size(600, 200);
				dcpAssortmentWorkspace.Text = "Assortment Workspace Explorer";
				dcpAssortmentWorkspace.TextTab = "Assortment Workspace Explorer";
				dcpAssortmentWorkspace.ToolTipCaption = "Assortment Workspace Explorer";
				dcpAssortmentWorkspace.ToolTipTab = "Assortment Workspace Explorer";
				dapAssortmentWorkspaceExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpAssortmentWorkspace });
				dapAssortmentWorkspaceExplorer.Size = new System.Drawing.Size(600, 300);     // really affects the size
			}

            dapAllocationWorkspaceExplorer.Key = Include.tbbAllocationWorkspace;
            dapStoreExplorer.Key = Include.tbbStores;
            dapAssortmentExplorer.Key = Include.tbbAssortmentExplorer;
            dapAssortmentWorkspaceExplorer.Key = Include.tbbAssortmentWorkspace;
            dapFilterExplorerStore.Key = Include.tbbStoreFilters;
            //Begin TT#1313-MD -jsobek -Header Filters
            dapFilterExplorerHeader.Key = Include.tbbHeaderFilters;
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                dapFilterExplorerAssortment.Key = Include.tbbAssortmentFilters;
            }
            //End TT#1313-MD -jsobek -Header Filters
            dapMerchandiseExplorer.Key = Include.tbbMerchandise;
            dapTasklistExplorer.Key = Include.tbbTaskLists;
            dapWorkflowMethodExplorer.Key = Include.tbbWorkflowMethods;
            // End TT#2
            // Begin TT#46 MD - JSmith - User Dashboard
			dapUserDashboardExplorer.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.TabGroup;
			dapUserDashboardExplorer.FloatingLocation = new System.Drawing.Point(102, 420);
			dcpUserDashboard.Control = this.UserDashboardExplorer;
			dcpUserDashboard.FlyoutSize = new System.Drawing.Size(280, -1);
			dcpUserDashboard.Key = Include.tbbUserActivityExplorer;
			dcpUserDashboard.OriginalControlBounds = new System.Drawing.Rectangle(472, 112, 240, 392);
			dcpUserDashboard.Pinned = false;
            dcpUserDashboardAppearance.Image = MIDGraphics.GetImage(MIDGraphics.UserActivityImage);
			dcpUserDashboard.Settings.Appearance = dcpUserDashboardAppearance;
			dcpUserDashboard.Size = new System.Drawing.Size(600, 200);
			dcpUserDashboard.Text = "My Activity";
            dcpUserDashboard.TextTab = "My Activity";
            dcpUserDashboard.ToolTipCaption = "My Activity";
            dcpUserDashboard.ToolTipTab = "My Activity";
			dapUserDashboardExplorer.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] { dcpUserDashboard });
			dapUserDashboardExplorer.Size = new System.Drawing.Size(600, 300);     // really affects the size
            dapUserDashboardExplorer.Key = Include.tbbUserActivityExplorer;
            // End TT#46 MD - JSmith - User Dashboard
			this.udmMain.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
																								 dapStoreExplorer,
																								 dapAllocationWorkspaceExplorer,
																								 dapWorkflowMethodExplorer,
																								 dapTasklistExplorer,
																								 dapFilterExplorerStore,
                                                                                                 dapFilterExplorerHeader,  //TT#1313-MD -jsobek -Header Filters
																								 dapUserDashboardExplorer,   // TT#46 MD - JSmith - User Dashboard
																								 dapMerchandiseExplorer
																							});

			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
                this.udmMain.DockAreas.Add(dapFilterExplorerAssortment); //TT#1313-MD -jsobek -Header Filters
                this.udmMain.DockAreas.Add(dapAssortmentWorkspaceExplorer);
				this.udmMain.DockAreas.Add(dapAssortmentExplorer);
			}

			this.udmMain.HostControl = this;
            // Assortment addition - RonM
            this.udmMain.AfterPaneButtonClick += new Infragistics.Win.UltraWinDock.PaneButtonEventHandler(this.udmMain_AfterPaneButtonClick);
            //
            
            // 
			// _ExplorerUnpinnedTabAreaLeft
			// 
			this._ExplorerUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this._ExplorerUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 40);
			this._ExplorerUnpinnedTabAreaLeft.Name = "_ExplorerUnpinnedTabAreaLeft";
			this._ExplorerUnpinnedTabAreaLeft.Owner = this.udmMain;
			this._ExplorerUnpinnedTabAreaLeft.Size = new System.Drawing.Size(25, 623);
			this._ExplorerUnpinnedTabAreaLeft.TabIndex = 1;
			// 
			// _ExplorerUnpinnedTabAreaRight
			// 
			this._ExplorerUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
			this._ExplorerUnpinnedTabAreaRight.Location = new System.Drawing.Point(1288, 40);
			this._ExplorerUnpinnedTabAreaRight.Name = "_ExplorerUnpinnedTabAreaRight";
			this._ExplorerUnpinnedTabAreaRight.Owner = this.udmMain;
			this._ExplorerUnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 623);
			this._ExplorerUnpinnedTabAreaRight.TabIndex = 2;
			// 
			// _ExplorerUnpinnedTabAreaTop
			// 
			this._ExplorerUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
			this._ExplorerUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 40);
			this._ExplorerUnpinnedTabAreaTop.Name = "_ExplorerUnpinnedTabAreaTop";
			this._ExplorerUnpinnedTabAreaTop.Owner = this.udmMain;
			this._ExplorerUnpinnedTabAreaTop.Size = new System.Drawing.Size(1288, 0);
			this._ExplorerUnpinnedTabAreaTop.TabIndex = 3;
			// 
			// _ExplorerUnpinnedTabAreaBottom
			// 
			this._ExplorerUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._ExplorerUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 663);
			this._ExplorerUnpinnedTabAreaBottom.Name = "_ExplorerUnpinnedTabAreaBottom";
			this._ExplorerUnpinnedTabAreaBottom.Owner = this.udmMain;
			this._ExplorerUnpinnedTabAreaBottom.Size = new System.Drawing.Size(1288, 0);
			this._ExplorerUnpinnedTabAreaBottom.TabIndex = 4;
			// 
			// _ExplorerAutoHideControl
			// 
			this._ExplorerAutoHideControl.Controls.AddRange(new System.Windows.Forms.Control[] {
																								   this.dwStoreGroupExplorer,
																								   this.dwAllocationWorkspaceExplorer,
																								   this.dwWorkflowMethodExplorer,
																								   this.dwFilterExplorerStore, 
                                                                                                   this.dwFilterExplorerHeader}); //TT#1313-MD -jsobek -Header Filters
			// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
                this._ExplorerAutoHideControl.Controls.Add(this.dwFilterExplorerAssortment); //TT#1313-MD -jsobek -Header Filters
				this._ExplorerAutoHideControl.Controls.Add(this.dwAssortmentWorkspace);
				this._ExplorerAutoHideControl.Controls.Add(this.dwAssortmentExplorer);
			}
			// END TT#2 - stodd
            this._ExplorerAutoHideControl.Controls.Add(this.dwUserDashboard);  // TT#46 MD - JSmith - User Dashboard

			this._ExplorerAutoHideControl.Location = new System.Drawing.Point(25, 40);
			this._ExplorerAutoHideControl.Name = "_ExplorerAutoHideControl";
			this._ExplorerAutoHideControl.Owner = this.udmMain;
			this._ExplorerAutoHideControl.Size = new System.Drawing.Size(284, 623);
			this._ExplorerAutoHideControl.TabIndex = 5;
			// 
			// dwMerchandiseExplorer
			// 
			this.dwMerchandiseExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.meMerchandiseExplorer});
			this.dwMerchandiseExplorer.Name = "dwMerchandiseExplorer";
			this.dwMerchandiseExplorer.Owner = this.udmMain;
			this.dwMerchandiseExplorer.Size = new System.Drawing.Size(292, 623);
			this.dwMerchandiseExplorer.TabIndex = 1;
			// 
			// dwFilterExplorerStore
			// 
			this.dwFilterExplorerStore.Controls.AddRange(new System.Windows.Forms.Control[] { this.filterExplorerStore1});
			this.dwFilterExplorerStore.Name = "dwFilterExplorerStore";
			this.dwFilterExplorerStore.Owner = this.udmMain;
			this.dwFilterExplorerStore.Size = new System.Drawing.Size(302, 623);
			this.dwFilterExplorerStore.TabIndex = 2;

            //Begin TT#1313-MD -jsobek -Header Filters
            // 
            // dwFilterExplorerHeader
            // 
            this.dwFilterExplorerHeader.Controls.AddRange(new System.Windows.Forms.Control[] { this.filterExplorerHeader1 });
            this.dwFilterExplorerHeader.Name = "dwFilterExplorerHeader";
            this.dwFilterExplorerHeader.Owner = this.udmMain;
            this.dwFilterExplorerHeader.Size = new System.Drawing.Size(302, 623);
            this.dwFilterExplorerHeader.TabIndex = 3;
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                // 
                // dwFilterExplorerAssortment
                // 
                this.dwFilterExplorerAssortment.Controls.AddRange(new System.Windows.Forms.Control[] { this.filterExplorerAssortment1 });
                this.dwFilterExplorerAssortment.Name = "dwFilterExplorerAssortment";
                this.dwFilterExplorerAssortment.Owner = this.udmMain;
                this.dwFilterExplorerAssortment.Size = new System.Drawing.Size(302, 623);
                this.dwFilterExplorerAssortment.TabIndex = 4;
            }
            //End TT#1313-MD -jsobek -Header Filters

			// 
			// dwWorkflowMethodExplorer
			// 
			this.dwWorkflowMethodExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.tvwWorkflowMethodExplorer});
			this.dwWorkflowMethodExplorer.Name = "dwWorkflowMethodExplorer";
			this.dwWorkflowMethodExplorer.Owner = this.udmMain;
			this.dwWorkflowMethodExplorer.Size = new System.Drawing.Size(246, 623);
			this.dwWorkflowMethodExplorer.TabIndex = 0;
			// 
			// dwAllocationWorkspaceExplorer
			// 
			this.dwAllocationWorkspaceExplorer.Controls.AddRange(new System.Windows.Forms.Control[] {	 this.AllocationWorkspaceExplorer1});
			this.dwAllocationWorkspaceExplorer.Name = "dwAllocationWorkspaceExplorer";
			this.dwAllocationWorkspaceExplorer.Owner = this.udmMain;
			this.dwAllocationWorkspaceExplorer.Size = new System.Drawing.Size(279, 623);
			this.dwAllocationWorkspaceExplorer.TabIndex = 0;
			// 
			// dwStoreGroupExplorer
			// 
			this.dwStoreGroupExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.storeGroupExplorer1});
			this.dwStoreGroupExplorer.Name = "dwStoreGroupExplorer";
			this.dwStoreGroupExplorer.Owner = this.udmMain;
			this.dwStoreGroupExplorer.Size = new System.Drawing.Size(287, 623);
			this.dwStoreGroupExplorer.TabIndex = 1;
			// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{

				// 
				// dwAssortmentExplorer
				// 
				this.dwAssortmentExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.assortmentExplorer });
				this.dwAssortmentExplorer.Name = "dwAssortmentExplorer";
				this.dwAssortmentExplorer.Owner = this.udmMain;
				this.dwAssortmentExplorer.Size = new System.Drawing.Size(287, 623);
				this.dwAssortmentExplorer.TabIndex = 1;
				// 
				// dwAssortmentWorkspace
				// 
				this.dwAssortmentWorkspace.Controls.AddRange(new System.Windows.Forms.Control[] { this.assortmentWorkspaceExplorer });
				this.dwAssortmentWorkspace.Name = "dwAssortmentWorkspace";
				this.dwAssortmentWorkspace.Owner = this.udmMain;
				this.dwAssortmentWorkspace.Size = new System.Drawing.Size(279, 623);
				this.dwAssortmentWorkspace.TabIndex = 0;
			}
			// End TT#2 - stodd Assortment
            // Begin TT#46 MD - JSmith - User Dashboard
			// dwUserDashboard
			// 
			this.dwUserDashboard.Controls.AddRange(new System.Windows.Forms.Control[] { this.UserDashboardExplorer });
			this.dwUserDashboard.Name = "dwUserDashboard";
			this.dwUserDashboard.Owner = this.udmMain;
			this.dwUserDashboard.Size = new System.Drawing.Size(279, 623);
			this.dwUserDashboard.TabIndex = 0;
            // End TT#46 MD
			// 
			// utmMain
			// 
			this.utmMain.DockWithinContainer = this;
            //this.utmMain.FlatMode = true; obsolete
            this.utmMain.UseFlatMode = DefaultableBoolean.True;
			optionSet1.AllowAllUp = false;
			this.utmMain.OptionSets.Add(optionSet1);
			this.utmMain.ShowFullMenusDelay = 500;
			this.utmMain.ShowMenuShadows = Infragistics.Win.DefaultableBoolean.True;
			this.utmMain.ShowQuickCustomizeButton = false;
			utMainMenu.DockedColumn = 0;
			utMainMenu.DockedRow = 0;
			utMainMenu.IsMainMenuBar = true;
			utMainMenu.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
			utMainMenu.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.False;
			utMainMenu.ShowInToolbarList = false;
			utMainMenu.Text = "Main Menu";
			utExplorers.DockedColumn = 0;
			utExplorers.DockedRow = 1;
			utExplorers.Text = Include.tbExplorers;
			utAnalysis.DockedColumn = 7;
			utAnalysis.DockedRow = 1;
			utAnalysis.Text = Include.tbAnalysis;
			utTools.DockedColumn = 1;
			utTools.DockedRow = 1;
			utTools.Text = "Tools";

            //Begin TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
            AddAssortmentToolbarToMainMenu();
            //End TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek

			this.utmMain.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
																									utMainMenu,
																									utExplorers,
																									utAnalysis
																									//	utViews,
																									//	utConfiguration,
																									//utTools
																								});

			btnToolOTSPlanAppearance.Image = MIDGraphics.GetImage(MIDGraphics.ForecastImage);
			btnToolOTSPlan.SharedProps.AppearancesSmall.Appearance = btnToolOTSPlanAppearance;
			btnToolOTSPlan.SharedProps.AppearancesLarge.Appearance = btnToolOTSPlanAppearance;
			btnToolOTSPlan.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_OTS_Plan);
			btnToolOTSPlan.SharedProps.Category = Include.tbAnalysis;

			btnToolAllocationSummaryAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AllocationSummaryImage);	// TT#707 - separate summary image from assortment
			btnToolAllocationSummary.SharedProps.AppearancesSmall.Appearance = btnToolAllocationSummaryAppearance;
			btnToolAllocationSummary.SharedProps.AppearancesLarge.Appearance = btnToolAllocationSummaryAppearance;
			btnToolAllocationSummary.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Summary);
			btnToolAllocationSummary.SharedProps.Category = Include.tbAnalysis;
			//Begin Assortment Planning - JScott - Assortment Planning Changes
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
				btnToolAssortmentAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
				btnToolAssortment.SharedProps.AppearancesSmall.Appearance = btnToolAssortmentAppearance;
				btnToolAssortment.SharedProps.AppearancesLarge.Appearance = btnToolAssortmentAppearance;
				btnToolAssortment.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Assortment);
				btnToolAssortment.SharedProps.Caption = Include.tbbAssortment;
				btnToolAssortment.SharedProps.Category = Include.tbAnalysis;

				btnToolAssortmentExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
				btnToolAssortmentExplorer.SharedProps.AppearancesSmall.Appearance = btnToolAssortmentExplorerAppearance;
				btnToolAssortmentExplorer.SharedProps.AppearancesLarge.Appearance = btnToolAssortmentExplorerAppearance;
				btnToolAssortmentExplorer.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Explorer);
				btnToolAssortmentExplorer.SharedProps.Caption = Include.tbbAssortmentExplorer;
				btnToolAssortmentExplorer.SharedProps.Category = Include.tbExplorers;

                btnToolAssortmentWorkspaceAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
                btnToolAssortmentWorkspace.SharedProps.AppearancesLarge.Appearance = btnToolAssortmentWorkspaceAppearance;
                btnToolAssortmentWorkspace.SharedProps.AppearancesSmall.Appearance = btnToolAssortmentWorkspaceAppearance;
                btnToolAssortmentWorkspace.SharedProps.Caption = Include.tbbAssortmentWorkspace;
                btnToolAssortmentWorkspace.SharedProps.Category = Include.tbExplorers;
			}
			//End Assortment Planning - JScott - Assortment Planning Changes

            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)	// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
            {
                // BEGIN TT#488-MD - STodd - Group Allocation - 
                btnToolGroupAllocationAppearance.Image = MIDGraphics.GetImage(MIDGraphics.AssortmentImage);
                btnToolGroupAllocation.SharedProps.AppearancesSmall.Appearance = btnToolGroupAllocationAppearance;
                btnToolGroupAllocation.SharedProps.AppearancesLarge.Appearance = btnToolGroupAllocationAppearance;
                btnToolGroupAllocation.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Group_Allocation_Review);
                btnToolGroupAllocation.SharedProps.Caption = Include.tbbGroupAllocation;
                btnToolGroupAllocation.SharedProps.Category = Include.tbAnalysis;
                // END TT#488-MD - STodd - Group Allocation - 
            }

            // Begin TT#46 MD - JSmith - User Dashboard
			btnToolUserDashboardAppearance.Image = MIDGraphics.GetImage(MIDGraphics.UserActivityImage);
            btnToolUserDashboard.SharedProps.AppearancesLarge.Appearance = btnToolUserDashboardAppearance;
            btnToolUserDashboard.SharedProps.AppearancesSmall.Appearance = btnToolUserDashboardAppearance;
            btnToolUserDashboard.SharedProps.Caption = Include.tbbUserActivityExplorer;
            btnToolUserDashboard.SharedProps.Category = Include.tbExplorers;
            // End TT#46 MD

			btnStyleViewAppearance.Image = MIDGraphics.GetImage(MIDGraphics.StyleViewImage);
			btnToolStyleView.SharedProps.AppearancesSmall.Appearance = btnStyleViewAppearance;
			btnToolStyleView.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style);
			btnToolStyleView.SharedProps.Category = Include.tbAnalysis;
			
			btnToolWorkspaceAppearance.Image = MIDGraphics.GetImage(MIDGraphics.WorkspaceImage);
			btnToolWorkspace.SharedProps.AppearancesLarge.Appearance = btnToolWorkspaceAppearance;
			btnToolWorkspace.SharedProps.AppearancesSmall.Appearance = btnToolWorkspaceAppearance;
			btnToolWorkspace.SharedProps.Caption = Include.tbbAllocationWorkspace;
			btnToolWorkspace.SharedProps.Category = Include.tbExplorers;

			btnToolStoreProfilesAppearance.Image = MIDGraphics.GetImage(MIDGraphics.StoreProfileImage);
			btnToolStoreProfiles.SharedProps.AppearancesLarge.Appearance = btnToolStoreProfilesAppearance;
			btnToolStoreProfiles.SharedProps.AppearancesSmall.Appearance = btnToolStoreProfilesAppearance;

			btnToolStoreFiltersAppearance.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage);
			btnToolStoreFilters.SharedProps.AppearancesLarge.Appearance = btnToolStoreFiltersAppearance;
			btnToolStoreFilters.SharedProps.AppearancesSmall.Appearance = btnToolStoreFiltersAppearance;
			btnToolStoreFilters.SharedProps.Caption = Include.tbbStoreFilters;
			btnToolStoreFilters.SharedProps.Category = Include.tbExplorers;

            //Begin TT#1313-MD -jsobek -Header Filters
            btnToolHeaderFiltersAppearance.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage);
            btnToolHeaderFilters.SharedProps.AppearancesLarge.Appearance = btnToolHeaderFiltersAppearance;
            btnToolHeaderFilters.SharedProps.AppearancesSmall.Appearance = btnToolHeaderFiltersAppearance;
            btnToolHeaderFilters.SharedProps.Caption = Include.tbbHeaderFilters;
            btnToolHeaderFilters.SharedProps.Category = Include.tbExplorers;

            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                btnToolAssortmentFiltersAppearance.Image = MIDGraphics.GetImage(MIDGraphics.FilterExplorerImage);
                btnToolAssortmentFilters.SharedProps.AppearancesLarge.Appearance = btnToolAssortmentFiltersAppearance;
                btnToolAssortmentFilters.SharedProps.AppearancesSmall.Appearance = btnToolAssortmentFiltersAppearance;
                btnToolAssortmentFilters.SharedProps.Caption = Include.tbbAssortmentFilters;
                btnToolAssortmentFilters.SharedProps.Category = Include.tbExplorers; 
            }
            //End TT#1313-MD -jsobek -Header Filters

			btnToolMerchandiseExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.HierarchyExplorerImage);
			btnToolMerchandiseExplorer.SharedProps.AppearancesLarge.Appearance = btnToolMerchandiseExplorerAppearance;
			btnToolMerchandiseExplorer.SharedProps.AppearancesSmall.Appearance = btnToolMerchandiseExplorerAppearance;
			btnToolMerchandiseExplorer.SharedProps.Caption = Include.tbbMerchandise;
			btnToolMerchandiseExplorer.SharedProps.Category = Include.tbExplorers;

			btnToolWorkflowMethodExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.MethodsImage);
			btnToolWorkflowMethodExplorer.SharedProps.AppearancesLarge.Appearance = btnToolWorkflowMethodExplorerAppearance;
			btnToolWorkflowMethodExplorer.SharedProps.AppearancesSmall.Appearance = btnToolWorkflowMethodExplorerAppearance;
			btnToolWorkflowMethodExplorer.SharedProps.Caption = Include.tbbWorkflowMethods;
			btnToolWorkflowMethodExplorer.SharedProps.Category = Include.tbExplorers;
			
            btnToolStoreExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.StoreImage);
			btnToolStoreExplorer.SharedProps.AppearancesLarge.Appearance = btnToolStoreExplorerAppearance;
			btnToolStoreExplorer.SharedProps.AppearancesSmall.Appearance = btnToolStoreExplorerAppearance;
			btnToolStoreExplorer.SharedProps.Caption = Include.tbbStores;
			btnToolStoreExplorer.SharedProps.Category = Include.tbExplorers;
			
			btnToolTaskListExplorerAppearance.Image = MIDGraphics.GetImage(MIDGraphics.TaskListImage);
			btnToolTaskListExplorer.SharedProps.AppearancesLarge.Appearance = btnToolTaskListExplorerAppearance;
			btnToolTaskListExplorer.SharedProps.AppearancesSmall.Appearance = btnToolTaskListExplorerAppearance;
			btnToolTaskListExplorer.SharedProps.Caption = Include.tbbTaskLists;
			btnToolTaskListExplorer.SharedProps.Category = Include.tbExplorers;

			this.utmMain.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
																							 btnToolOTSPlan,
																							 btnToolAllocationSummary,
																							 btnToolStyleView,
																							 btnToolWorkspace,
																							 btnToolStoreProfiles,
																							 btnToolStoreFilters,
                                                                                             btnToolHeaderFilters, // TT#1313-MD -jsobek -Header Filters
																							 btnToolTaskListExplorer,
																							 btnToolMerchandiseExplorer,
																							 btnToolWorkflowMethodExplorer,
																							 btnToolStoreExplorer,
//																							 btnToolScheduler,
																							 btnToolOTSPlan2,
                                                                                             btnToolUserDashboard,   // TT#46 MD - JSmith - User Dashboard
																							 stateButtonTool1 
																						 });



			//Begin Assortment Planning - JScott - Assortment Planning Changes
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
				this.utmMain.Tools.Add(btnToolAssortment);
                this.utmMain.Tools.Add(btnToolAssortmentWorkspace); //TT#2 Assortment Planning
				this.utmMain.Tools.Add(btnToolAssortmentExplorer);
			}
			//End Assortment Planning - JScott - Assortment Planning Changes

            //Begin TT#1313-MD -jsobek -Header Filters
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                this.utmMain.Tools.Add(btnToolAssortmentFilters);
            }
            //End TT#1313-MD -jsobek -Header Filters

			// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
            {
                this.utmMain.Tools.Add(btnToolGroupAllocation);		// TT#488-MD - STodd - Group Allocation - 
            }
			// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

			this.utmMain.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.utmMain_ToolClick);
			// 
			// _Explorer_Toolbars_Dock_Area_Left
			// 
			this._Explorer_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
			this._Explorer_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
			this._Explorer_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Explorer_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 40);
			this._Explorer_Toolbars_Dock_Area_Left.Name = "_Explorer_Toolbars_Dock_Area_Left";
			this._Explorer_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 623);
			this._Explorer_Toolbars_Dock_Area_Left.ToolbarsManager = this.utmMain;
			// 
			// _Explorer_Toolbars_Dock_Area_Right
			// 
			this._Explorer_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
			this._Explorer_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
			this._Explorer_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Explorer_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1288, 40);
			this._Explorer_Toolbars_Dock_Area_Right.Name = "_Explorer_Toolbars_Dock_Area_Right";
			this._Explorer_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 623);
			this._Explorer_Toolbars_Dock_Area_Right.ToolbarsManager = this.utmMain;
			// 
			// _Explorer_Toolbars_Dock_Area_Top
			// 
			this._Explorer_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
			this._Explorer_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
			this._Explorer_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Explorer_Toolbars_Dock_Area_Top.Name = "_Explorer_Toolbars_Dock_Area_Top";
			this._Explorer_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1288, 40);
			this._Explorer_Toolbars_Dock_Area_Top.ToolbarsManager = this.utmMain;
			// 
			// _Explorer_Toolbars_Dock_Area_Bottom
			// 
			this._Explorer_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
			this._Explorer_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
			this._Explorer_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
			this._Explorer_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 663);
			this._Explorer_Toolbars_Dock_Area_Bottom.Name = "_Explorer_Toolbars_Dock_Area_Bottom";
			this._Explorer_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1288, 40);
			this._Explorer_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.utmMain;
			// 
			// wdaMerchandiseExplorer
			// 
			this.wdaMerchandiseExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.dwMerchandiseExplorer});
			this.wdaMerchandiseExplorer.Dock = System.Windows.Forms.DockStyle.Left;
			this.wdaMerchandiseExplorer.Location = new System.Drawing.Point(25, 40);
			this.wdaMerchandiseExplorer.Name = "wdaMerchandiseExplorer";
			this.wdaMerchandiseExplorer.Owner = this.udmMain;
			this.wdaMerchandiseExplorer.Size = new System.Drawing.Size(297, 623);
			this.wdaMerchandiseExplorer.TabIndex = 0;
			// 
			// wdaAllocationWorkspaceExplorer
			// 
			this.wdaAllocationWorkspaceExplorer.Controls.AddRange(new System.Windows.Forms.Control[] { this.dwAllocationWorkspaceExplorer});
			this.wdaAllocationWorkspaceExplorer.Dock = System.Windows.Forms.DockStyle.Top;
			this.wdaAllocationWorkspaceExplorer.Location = new System.Drawing.Point(325, 40);
			this.wdaAllocationWorkspaceExplorer.Name = "wdaAllocationWorkspaceExplorer";
			this.wdaAllocationWorkspaceExplorer.Owner = this.udmMain;
			this.wdaAllocationWorkspaceExplorer.Size = new System.Drawing.Size(623, 298);
			this.wdaAllocationWorkspaceExplorer.TabIndex = 1;
            // Begin TT#2 Assortment Planning
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{

				// 
				// wdaAssortmentWorkspace
				// 
				this.wdaAssortmentWorkspace.Controls.AddRange(new System.Windows.Forms.Control[] { this.dwAssortmentWorkspace });
				this.wdaAssortmentWorkspace.Dock = System.Windows.Forms.DockStyle.Top;
				this.wdaAssortmentWorkspace.Location = new System.Drawing.Point(325, 40);
				this.wdaAssortmentWorkspace.Name = "wdaAssortmentWorkspace";
				this.wdaAssortmentWorkspace.Owner = this.udmMain;
				this.wdaAssortmentWorkspace.Size = new System.Drawing.Size(623, 298);
				this.wdaAssortmentWorkspace.TabIndex = 2;
			}
            // End TT#2
            // Begin TT#46 MD - JSmith - User Dashboard
			// 
			// wdaUserDashboard
			// 
			this.wdaUserDashboard.Controls.AddRange(new System.Windows.Forms.Control[] { this.dwUserDashboard });
			this.wdaUserDashboard.Dock = System.Windows.Forms.DockStyle.Top;
			this.wdaUserDashboard.Location = new System.Drawing.Point(325, 40);
			this.wdaUserDashboard.Name = "wdaUserDashboard";
			this.wdaUserDashboard.Owner = this.udmMain;
			this.wdaUserDashboard.Size = new System.Drawing.Size(623, 298);
			this.wdaUserDashboard.TabIndex = 2;
            // End TT#46 MD
            //
			// Explorer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(1288, 663);
			// Begin TT#2 - stodd - assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{

				this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._ExplorerAutoHideControl,
																		  this.wdaMerchandiseExplorer,
																		  this.wdaAllocationWorkspaceExplorer,
                                                                          this.wdaAssortmentWorkspace,      // TT#2 Assortment Planning
																		  this.wdaUserDashboard,  // TT#46 MD - JSmith - User Dashboard
																		  this._ExplorerUnpinnedTabAreaTop,
																		  this._ExplorerUnpinnedTabAreaBottom,
																		  this._ExplorerUnpinnedTabAreaLeft,
																		  this._ExplorerUnpinnedTabAreaRight,
																		  this._Explorer_Toolbars_Dock_Area_Left,
																		  this._Explorer_Toolbars_Dock_Area_Right,
																		  this._Explorer_Toolbars_Dock_Area_Top,
																		  this._Explorer_Toolbars_Dock_Area_Bottom});
			}
			else
			{
				this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this._ExplorerAutoHideControl,
																		  this.wdaMerchandiseExplorer,
																		  this.wdaAllocationWorkspaceExplorer,
																		  this._ExplorerUnpinnedTabAreaTop,
																		  this._ExplorerUnpinnedTabAreaBottom,
																		  this._ExplorerUnpinnedTabAreaLeft,
																		  this._ExplorerUnpinnedTabAreaRight,
																		  this._Explorer_Toolbars_Dock_Area_Left,
																		  this._Explorer_Toolbars_Dock_Area_Right,
																		  this._Explorer_Toolbars_Dock_Area_Top,
																		  this._Explorer_Toolbars_Dock_Area_Bottom});
			}
			// End TT#2 - stodd Assortment
//			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Icon = new System.Drawing.Icon(MIDGraphics.ImageDir + "\\" + MIDGraphics.ApplicationIcon);
			this.IsMdiContainer = true;
			this.Name = "Explorer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Logility - RO";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Explorer_Closing);
			this.Load += new System.EventHandler(this.Explorer_Load);
			((System.ComponentModel.ISupportInitialize)(this.udmMain)).EndInit();
			this._ExplorerAutoHideControl.ResumeLayout(false);
			this.dwMerchandiseExplorer.ResumeLayout(false);
			this.dwFilterExplorerStore.ResumeLayout(false);
            this.dwFilterExplorerHeader.ResumeLayout(false); //TT#1313-MD -jsobek -Header Filters
			this.dwWorkflowMethodExplorer.ResumeLayout(false);
			this.dwAllocationWorkspaceExplorer.ResumeLayout(false);
			this.dwStoreGroupExplorer.ResumeLayout(false);
			// Begin TT#2 - stodd Assortment
			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
			{
                this.dwFilterExplorerAssortment.ResumeLayout(false); //TT#1313-MD -jsobek -Header Filters
				this.dwAssortmentExplorer.ResumeLayout(false);
				this.dwAssortmentWorkspace.ResumeLayout(); // TT#2 Assortment Planning
			}
			// End TT#2 - stodd Assortment
            
           
            this.dwUserDashboard.ResumeLayout();  // TT#46 MD - JSmith - User Dashboard

			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.wdaMerchandiseExplorer.ResumeLayout(false);
			this.ResumeLayout(false);

			// ofdOpenCustom
			this.ofdOpenCustom.Filter = "Crystal Reports|*.rpt|All Files|*.*";
			this.ofdOpenCustom.DefaultExt = "rpt";
			this.ofdOpenCustom.Title = "Open Custom Report";
			this.ofdOpenCustom.RestoreDirectory = true;
            // 
            // statusStrip1
            // 
            //this.statusStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            //this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            //this.statusStrip1.Location = new System.Drawing.Point(1086, 0);
            //this.statusStrip1.Name = "statusStrip1";
            //this.statusStrip1.Size = new System.Drawing.Size(202, 22);
            //this.statusStrip1.TabIndex = 11;
            //this.statusStrip1.Text = "statusStrip1";

            statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripStatusLabel1});
            statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip1.Location = new System.Drawing.Point(0, 0);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.ShowItemToolTips = true;
            statusStrip1.Size = new System.Drawing.Size(292, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.Stretch = false;
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";

            Controls.Add(statusStrip1);
          
		}
		#endregion

		#region InitializeutbMainMenu

		private void InitializeutbMainMenu()
		{
			try
			{
				UltraToolbar utbMainMenu = this.utmMain.Toolbars["Main Menu"];
				this.utmMain.ImageListSmall = MIDGraphics.ImageList;
				this.utmMain.ImageListLarge = MIDGraphics.ImageList;
				
				#region FileMenu
				// Create the File Menu
				PopupMenuTool fileMenuTool		= new PopupMenuTool(Include.menuFile);

				fileMenuTool.SharedProps.Caption	 = MIDText.GetTextOnly(eMIDTextCode.menu_File);
				fileMenuTool.Settings.IsSideStripVisible						= DefaultableBoolean.False;
				fileMenuTool.Settings.SideStripText								= "MID Allocation";
				fileMenuTool.Settings.SideStripAppearance.ForeColor				= SystemColors.Highlight;
				fileMenuTool.Settings.SideStripAppearance.BackColor2			= Color.WhiteSmoke;
				fileMenuTool.Settings.SideStripAppearance.BackGradientStyle		= GradientStyle.Vertical;
				fileMenuTool.Settings.SideStripAppearance.FontData.Bold			= DefaultableBoolean.False;
				fileMenuTool.Settings.SideStripAppearance.FontData.SizeInPoints	= 16;
				fileMenuTool.Settings.SideStripAppearance.FontData.Name			= "Arial";
				//fileMenuTool.Settings.SideStripAppearance.Image					= 24;


				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(fileMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(fileMenuTool);

				ButtonTool btClose					= new ButtonTool(Include.btClose);
				btClose.SharedProps.Caption			= "&Close";
				btClose.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btClose);

				ButtonTool btSave				= new ButtonTool(Include.btSave);
				btSave.SharedProps.Caption			= "&Save";
				btSave.SharedProps.MergeOrder		= 10;
				btSave.SharedProps.Shortcut			= Shortcut.CtrlS;
				btSave.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.SaveImage);
				this.utmMain.Tools.Add(btSave);

				ButtonTool btSaveAs				= new ButtonTool(Include.btSaveAs);
				btSaveAs.SharedProps.Caption			= "Save &As";
				btSaveAs.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSaveAs);

                //Begin TT#1521-MD -jsobek -Active Directory Authentication
                ButtonTool btLoginAsAdmin = new ButtonTool(Include.btLoginAsAdmin);
                btLoginAsAdmin.SharedProps.Caption = "Login as Administrator";
                btLoginAsAdmin.SharedProps.MergeOrder = 15;

                // Always add the button and control with security 

                //SecurityAdmin secAdmin = new SecurityAdmin();
                //bool isUserInAdminGroup = secAdmin.IsUserInAdminGroup(_SAB.ClientServerSession.UserRID);

                //if (isUserInAdminGroup)
                //{
                    this.utmMain.Tools.Add(btLoginAsAdmin);
                //}
                //End TT#1521-MD -jsobek -Active Directory Authentication

				ButtonTool btExit				= new ButtonTool(Include.btExit);
				btExit.SharedProps.Caption			= "E&xit";
				btExit.SharedProps.MergeOrder		= 20;
				this.utmMain.Tools.Add(btExit);


      


				fileMenuTool.Tools.Add(btClose);
				fileMenuTool.Tools.Add(btSave);
				fileMenuTool.Tools.Add(btSaveAs);
                //if (isUserInAdminGroup)
                //{
                    fileMenuTool.Tools.Add(btLoginAsAdmin); //TT#1521-MD -jsobek -Active Directory Authentication
                //}
				fileMenuTool.Tools.Add(btExit);

				fileMenuTool.Tools[Include.btSave].InstanceProps.IsFirstInGroup = true;
				// fileMenuTool.Tools[Include.btExport].InstanceProps.IsFirstInGroup = true;
                //Begin TT#1521-MD -jsobek -Active Directory Authentication
                //if (isUserInAdminGroup)
                //{
                    fileMenuTool.Tools[Include.btLoginAsAdmin].InstanceProps.IsFirstInGroup = true;
                //}
                //else
                //{
                //    fileMenuTool.Tools[Include.btExit].InstanceProps.IsFirstInGroup = true;
                //}
                //End TT#1521-MD -jsobek -Active Directory Authentication

				#endregion

				#region EditMenu
				// Create the Edit Menu
				PopupMenuTool editMenuTool		= new PopupMenuTool(Include.menuEdit);

				editMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
				editMenuTool.Settings.IsSideStripVisible						= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(editMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(editMenuTool);


				ButtonTool btCut					= new ButtonTool(Include.btCut);
				btCut.SharedProps.Caption			= "Cu&t";
				btCut.SharedProps.MergeOrder		= 10;
				btCut.SharedProps.Shortcut			= Shortcut.CtrlX;
				btCut.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.CutImage);
				this.utmMain.Tools.Add(btCut);

				ButtonTool btCopy					= new ButtonTool(Include.btCopy);
				btCopy.SharedProps.Caption			= "&Copy";
				btCopy.SharedProps.MergeOrder		= 10;
				btCopy.SharedProps.Shortcut			= Shortcut.CtrlC;
				btCopy.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.CopyImage);
				this.utmMain.Tools.Add(btCopy);

				ButtonTool btPaste					= new ButtonTool(Include.btPaste);
				btPaste.SharedProps.Caption			= "&Paste";
				btPaste.SharedProps.MergeOrder		= 10;
				btPaste.SharedProps.Shortcut		= Shortcut.CtrlV;
				btPaste.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.PasteImage);
				this.utmMain.Tools.Add(btPaste);

				ButtonTool btDelete					= new ButtonTool(Include.btDelete);
				btDelete.SharedProps.Caption		= "&Delete";
				btDelete.SharedProps.MergeOrder		= 10;
				//				btDelete.SharedProps.Shortcut		= Shortcut.Del;
				btDelete.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.DeleteImage);
				this.utmMain.Tools.Add(btDelete);

				ButtonTool btFind					= new ButtonTool(Include.btFind);
				btFind.SharedProps.Caption			= "&Find";
				btFind.SharedProps.MergeOrder		= 15;
				btFind.SharedProps.Shortcut			= Shortcut.CtrlF;
				btFind.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FindImage);
				this.utmMain.Tools.Add(btFind);


				editMenuTool.Tools.Add(btCut);
				editMenuTool.Tools.Add(btCopy);
				editMenuTool.Tools.Add(btPaste);
				editMenuTool.Tools.Add(btDelete);
				editMenuTool.Tools.Add(btFind);
				
				editMenuTool.Tools[Include.btCut].InstanceProps.IsFirstInGroup = true;
				this.utmMain.Tools[Include.btFind].SharedProps.Visible = false;
				this.utmMain.Tools[Include.btFind].SharedProps.Enabled = false;
				
				#endregion

				#region ViewMenu
				// Create the View Menu
				PopupMenuTool viewMenuTool	= new PopupMenuTool(Include.menuView);

				viewMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_View);
				viewMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(viewMenuTool);

				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(viewMenuTool);
								
				StateButtonTool btWorkspace				= new StateButtonTool(Include.btWorkspace);
				btWorkspace.SharedProps.Caption			= Include.tbbAllocationWorkspace;
				btWorkspace.SharedProps.MergeOrder		= 10;
				btWorkspace.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.WorkspaceImage);
				this.utmMain.Tools.Add(btWorkspace);

				StateButtonTool btPlanWorkspace				= new StateButtonTool(Include.btPlanWorkspace);
				btPlanWorkspace.SharedProps.Caption			= "OTS Plan Workspace";
				btPlanWorkspace.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btPlanWorkspace);

				StateButtonTool btMerchandise				= new StateButtonTool(Include.btMerchandise);
				btMerchandise.SharedProps.Caption			= "Merchandise Explorer";
				btMerchandise.SharedProps.MergeOrder		= 10;
				btMerchandise.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.HierarchyExplorerImage);
				this.utmMain.Tools.Add(btMerchandise);

				StateButtonTool btStore				= new StateButtonTool(Include.btStore);
				btStore.SharedProps.Caption			= "Store Explorer";
				btStore.SharedProps.MergeOrder		= 10;
				btStore.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.StoreImage);
				this.utmMain.Tools.Add(btStore);

				StateButtonTool btStoreFilter			= new StateButtonTool(Include.btStoreFilter);
				btStoreFilter.SharedProps.Caption			= "Store Filters";
				btStoreFilter.SharedProps.MergeOrder		= 10;
				btStoreFilter.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.FilterExplorerImage);
				this.utmMain.Tools.Add(btStoreFilter);

                //Begin TT#1313-MD -jsobek -Header Filters
                StateButtonTool btHeaderFilter = new StateButtonTool(Include.btHeaderFilter);
                btHeaderFilter.SharedProps.Caption = "Header Filters";
                btHeaderFilter.SharedProps.MergeOrder = 10;
                btHeaderFilter.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FilterExplorerImage);
                this.utmMain.Tools.Add(btHeaderFilter);
                StateButtonTool btAssortmentFilter = null;  // TT#2014-MD - JSmith - Assortment Security
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    // Begin TT#2014-MD - JSmith - Assortment Security
                    //StateButtonTool btAssortmentFilter = new StateButtonTool(Include.btAssortmentFilter);
                    btAssortmentFilter = new StateButtonTool(Include.btAssortmentFilter);
                    // End TT#2014-MD - JSmith - Assortment Security
                    btAssortmentFilter.SharedProps.Caption = "Assortment Filters";
                    btAssortmentFilter.SharedProps.MergeOrder = 10;
                    btAssortmentFilter.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.FilterExplorerImage);
                    this.utmMain.Tools.Add(btAssortmentFilter);
                }
                //End TT#1313-MD -jsobek -Header Filters

				StateButtonTool btTaskList			= new StateButtonTool(Include.btTaskList);
				btTaskList.SharedProps.Caption			= "Task List Explorer";
				btTaskList.SharedProps.MergeOrder		= 10;
				btTaskList.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.TaskListImage);
				this.utmMain.Tools.Add(btTaskList);

				StateButtonTool btWorkflow				= new StateButtonTool(Include.btWorkflow);
				btWorkflow.SharedProps.Caption			= "Workflow/Method Explorer";
				btWorkflow.SharedProps.MergeOrder		= 10;
				btWorkflow.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.MethodsImage);
				this.utmMain.Tools.Add(btWorkflow);

				viewMenuTool.Tools.Add(btWorkspace);
				// remove from menu until needed
				//				viewMenuTool.Tools.Add(btPlanWorkspace);
				viewMenuTool.Tools.Add(btMerchandise);
				viewMenuTool.Tools.Add(btStore);
				viewMenuTool.Tools.Add(btWorkflow);
				viewMenuTool.Tools.Add(btStoreFilter);
				viewMenuTool.Tools.Add(btTaskList);
				//Begin Track #5835 stodd
                viewMenuTool.Tools.Add(btHeaderFilter);  // TT#2014-MD - JSmith - Assortment Security
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
                    viewMenuTool.Tools.Add(btAssortmentFilter);  // TT#2014-MD - JSmith - Assortment Security
					StateButtonTool btAssortmentExplorer = new StateButtonTool(Include.btAssortmentExplorer);
					btAssortmentExplorer.SharedProps.Caption = Include.tbbAssortmentExplorer;
					btAssortmentExplorer.SharedProps.MergeOrder = 10;
					btAssortmentExplorer.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.AssortmentImage);
					this.utmMain.Tools.Add(btAssortmentExplorer);
					viewMenuTool.Tools.Add(btAssortmentExplorer);
                    // Begin TT#2 - RMatelic - Assortment Planning
                    StateButtonTool btAssortmentWorkspace = new StateButtonTool(Include.btAssortmentWorkspace);
                    btAssortmentWorkspace.SharedProps.Caption = Include.tbbAssortmentWorkspace;
                    btAssortmentWorkspace.SharedProps.MergeOrder = 10;
                    btAssortmentWorkspace.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.AssortmentImage);
                    this.utmMain.Tools.Add(btAssortmentWorkspace);
                    viewMenuTool.Tools.Add(btAssortmentWorkspace);
                    // End TT#2 
                    
				}
				//End Track #5835 stodd
                // Begin TT#46 MD - JSmith - User Dashboard
                //BEGIN TT#46-MD -jsobek -Develop My Activity Log
				StateButtonTool btUserDashboard = new StateButtonTool(Include.btUserActivityExplorer);
                btUserDashboard.SharedProps.Caption = Include.tbbUserActivityExplorer;
                btUserDashboard.SharedProps.MergeOrder = 10;
                btUserDashboard.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.UserActivityImage);
                this.utmMain.Tools.Add(btUserDashboard);
                viewMenuTool.Tools.Add(btUserDashboard);
                //END TT#46-MD -jsobek -Develop My Activity Log
                // End TT#46 MD - JSmith - User Dashboard

				viewMenuTool.Tools[Include.btWorkspace].InstanceProps.IsFirstInGroup = true;

				#endregion

				#region AllocationMenu
				// Create the Allocation Menu
				PopupMenuTool allocMenuTool	= new PopupMenuTool(Include.menuAlloc);

				allocMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Allocation);
				allocMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(allocMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(allocMenuTool);
				
				//Create View SubMenu
				PopupMenuTool pmtAllocationReview		= new PopupMenuTool(Include.menuAllocSub);
				pmtAllocationReview.SharedProps.MergeOrder = 10;
				pmtAllocationReview.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Review);

				ButtonTool btSelect			= new ButtonTool(Include.btSelect);
				btSelect.SharedProps.Caption			= MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Select);
				btSelect.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSelect);
				pmtAllocationReview.Tools.Add(btSelect);

				ButtonTool btStyle					= new ButtonTool(Include.btStyle);
				btStyle.SharedProps.Caption			= MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style);
				btStyle.SharedProps.MergeOrder		= 10;
				btStyle.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.StyleViewImage); 
				this.utmMain.Tools.Add(btStyle);
				pmtAllocationReview.Tools.Add(btStyle);
				
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					Infragistics.Win.UltraWinToolbars.ButtonTool btnToolSizeView = new Infragistics.Win.UltraWinToolbars.ButtonTool(Include.tbbSize);
					Infragistics.Win.Appearance btnSizeViewAppearance = new Infragistics.Win.Appearance();
					
					btnSizeViewAppearance.Image = MIDGraphics.GetImage(MIDGraphics.SizeImage);
					btnToolSizeView.SharedProps.AppearancesSmall.Appearance = btnSizeViewAppearance;
					btnToolSizeView.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
					btnToolSizeView.SharedProps.Category = Include.tbAnalysis;

					ButtonTool btSize					= new ButtonTool(Include.btSize);
					btSize.SharedProps.Caption			= MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
					btSize.SharedProps.MergeOrder		= 10;
					btSize.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.SizeImage); 
					this.utmMain.Tools.Add(btSize);
					this.utmMain.Tools.Add(btnToolSizeView);
					pmtAllocationReview.Tools.Add(btSize);
				}

				ButtonTool btSummary = new ButtonTool(Include.btSummary);
				btSummary.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Summary);
				btSummary.SharedProps.MergeOrder = 10;
				btSummary.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.AssortmentImage);
				this.utmMain.Tools.Add(btSummary);
				pmtAllocationReview.Tools.Add(btSummary);
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					ButtonTool btAssortment = new ButtonTool(Include.btAssortment);
					btAssortment.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Assortment);
					btAssortment.SharedProps.MergeOrder = 10;
					btAssortment.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.AssortmentImage);
					this.utmMain.Tools.Add(btAssortment);
					pmtAllocationReview.Tools.Add(btAssortment);
				}
				//End Assortment Planning - JScott - Assortment Planning Changes

				// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    // Begin TT#950 - MD - stodd - missing "Group Allocation" button
                    ButtonTool btGroupAllocation = new ButtonTool(Include.btGroupAllocation);
                    btGroupAllocation.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Group_Allocation_Review);
                    btGroupAllocation.SharedProps.MergeOrder = 10;
                    btGroupAllocation.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.AssortmentImage);
                    this.utmMain.Tools.Add(btGroupAllocation);
                    pmtAllocationReview.Tools.Add(btGroupAllocation);
                    // End TT#950 - MD - stodd - missing "Group Allocation" button
                }
				// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

				// Add the Cascade1 menu tool to the File menu tools collection.
				this.utmMain.Tools.Add(pmtAllocationReview);
				allocMenuTool.Tools.Add(pmtAllocationReview);

				allocMenuTool.Tools[Include.menuAllocSub].InstanceProps.IsFirstInGroup = true;
				
				#endregion

				#region PlanningMenu
				// Create the Allocation Menu
				PopupMenuTool planningMenuTool	= new PopupMenuTool(Include.menuPlanning);

				planningMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_OTS_Plan);
				planningMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(planningMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(planningMenuTool);

				ButtonTool btPlanView					= new ButtonTool(Include.btPlanView);
				btPlanView.SharedProps.Caption			= "Re&view";
				btPlanView.SharedProps.MergeOrder		= 10;
				btPlanView.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.ForecastImage);
				this.utmMain.Tools.Add(btPlanView);

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail  : move Views to Tools  
                planningMenuTool.Tools.Add(btPlanView);
				/* BEGIN MID Track #4386 - Justin Bolles - Delete Views */
                //ButtonTool btViewAdmin					= new ButtonTool(Include.btViewAdmin);
                //btViewAdmin.SharedProps.Caption			= "V&iews";
                //btViewAdmin.SharedProps.MergeOrder		= 10;
                ////btPlanView.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.ForecastImage);
                //this.utmMain.Tools.Add(btViewAdmin);
                ///* END MID Track #4386 - Justin Bolles - Delete Views */
                //planningMenuTool.Tools.Add(btPlanView);
                //planningMenuTool.Tools.Add(btViewAdmin); //MID Track #4386 - Justin Bolles - Delete Views
                // End TT#231 

				#endregion

				#region AdminMenu
				// Create the Allocation Menu
				PopupMenuTool adminMenuTool	= new PopupMenuTool(Include.menuAdmin);

				adminMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Admin);
				adminMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(adminMenuTool);

				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(adminMenuTool);
				
				// Add the security menu tool to the File menu tools collection.
                ButtonTool btSecurity = new ButtonTool(Include.btSecurity);
				btSecurity.SharedProps.Caption			= "Securit&y";
				btSecurity.SharedProps.MergeOrder		= 10;
				btSecurity.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.SecurityImage);
				this.utmMain.Tools.Add(btSecurity);
				adminMenuTool.Tools.Add(btSecurity);

				

				ButtonTool btCalendar					= new ButtonTool(Include.btCalendar);
				btCalendar.SharedProps.Caption			= "&Calendar";
				btCalendar.SharedProps.MergeOrder		= 10;
				btCalendar.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.CalendarImage);
				this.utmMain.Tools.Add(btCalendar);
				adminMenuTool.Tools.Add(btCalendar);

				//Create Characteristics SubMenu
				PopupMenuTool characteristicSubMenu = new PopupMenuTool(Include.menuCharacteristicSubMenu);
				characteristicSubMenu.SharedProps.MergeOrder = 10;
				characteristicSubMenu.SharedProps.Caption = "&Characteristics";

				ButtonTool btHeaderChar					= new ButtonTool(Include.btHeaderChar);
				btHeaderChar.SharedProps.Caption			= "&Header";
				btHeaderChar.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btHeaderChar);
				//adminMenuTool.Tools.Add(btHeaderChar);
				characteristicSubMenu.Tools.Add(btHeaderChar);

				ButtonTool btProductChar = new ButtonTool(Include.btProductChar);
				btProductChar.SharedProps.Caption = "&Product";
				btProductChar.SharedProps.MergeOrder = 10;
				this.utmMain.Tools.Add(btProductChar);
				characteristicSubMenu.Tools.Add(btProductChar);

				ButtonTool btStoreChar = new ButtonTool(Include.btStoreChar);
				btStoreChar.SharedProps.Caption = "&Store";
				btStoreChar.SharedProps.MergeOrder = 10;
				this.utmMain.Tools.Add(btStoreChar);
				characteristicSubMenu.Tools.Add(btStoreChar);

				this.utmMain.Tools.Add(characteristicSubMenu);
				adminMenuTool.Tools.Add(characteristicSubMenu);
				
				//Create Stores SubMenu
                PopupMenuTool storesSubMenu = new PopupMenuTool(Include.menuStoreSub);
				storesSubMenu.SharedProps.MergeOrder = 10;
				storesSubMenu.SharedProps.Caption	= "&Store";

				ButtonTool btProfiles					= new ButtonTool(Include.btProfiles);
				btProfiles.SharedProps.Caption			= "&Profiles";
				btProfiles.SharedProps.MergeOrder		= 10;
				btProfiles.SharedProps.AppearancesSmall.Appearance.Image	= MIDGraphics.ImageIndex(MIDGraphics.StoreProfileImage);
				this.utmMain.Tools.Add(btProfiles);
				storesSubMenu.Tools.Add(btProfiles);

				ButtonTool btCharacteristics			= new ButtonTool(Include.btCharacteristics);
				btCharacteristics.SharedProps.Caption			= "&Characteristics";
				btCharacteristics.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btCharacteristics);
				storesSubMenu.Tools.Add(btCharacteristics);

				// Add the Cascade1 menu tool to the File menu tools collection.
				this.utmMain.Tools.Add(storesSubMenu);
				adminMenuTool.Tools.Add(storesSubMenu);


				ButtonTool btOptions					= new ButtonTool(Include.btOptions);
				btOptions.SharedProps.Caption			= "&Options";
				btOptions.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btOptions);
				adminMenuTool.Tools.Add(btOptions);

				//Create Models SubMenu
				PopupMenuTool modelsSubMenu		= new PopupMenuTool(Include.menuModelsSubMenu);
				modelsSubMenu.SharedProps.MergeOrder = 10;
				modelsSubMenu.SharedProps.Caption	= "&Models";

				ButtonTool btEligModels			= new ButtonTool(Include.btEligModels);
				btEligModels.SharedProps.Caption			= "&Eligibility";
				btEligModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btEligModels);
				modelsSubMenu.Tools.Add(btEligModels);

				ButtonTool btStkModModels			= new ButtonTool(Include.btStkModModels);
				btStkModModels.SharedProps.Caption			= "&Stock Modifier";
				btStkModModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btStkModModels);
				modelsSubMenu.Tools.Add(btStkModModels);

				ButtonTool btSlsModModels			= new ButtonTool(Include.btSlsModModels);
				btSlsModModels.SharedProps.Caption			= "S&ales Modifier";
				btSlsModModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSlsModModels);
				modelsSubMenu.Tools.Add(btSlsModModels);

                ButtonTool btOverrideLowLevel = new ButtonTool(Include.btOverrideLowLevel);
                btOverrideLowLevel.SharedProps.Caption = "&Override Low Levels";
                btOverrideLowLevel.SharedProps.MergeOrder = 10;
                this.utmMain.Tools.Add(btOverrideLowLevel);
                modelsSubMenu.Tools.Add(btOverrideLowLevel);

				// BEGIN MID Track #4370 - John Smith - FWOS Models
				ButtonTool btFWOSModModels			= new ButtonTool(Include.btFWOSModModels);
                btFWOSModModels.SharedProps.Caption = "F&WOS Modifier";
				btFWOSModModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btFWOSModModels);
				modelsSubMenu.Tools.Add(btFWOSModModels);
				// END MID Track #4370

                // BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                ButtonTool btFWOSMaxModels = new ButtonTool(Include.btFWOSMaxModels);
                btFWOSMaxModels.SharedProps.Caption = "F&WOS Modifier";
                btFWOSMaxModels.SharedProps.MergeOrder = 10;
                this.utmMain.Tools.Add(btFWOSMaxModels);
                modelsSubMenu.Tools.Add(btFWOSMaxModels);
                // END TT#108 - MD - DOConnell - FWOS Max Model

				ButtonTool btForecastingModels			= new ButtonTool(Include.btForecastingModels);
				btForecastingModels.SharedProps.Caption			= "&Forecasting";
				btForecastingModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btForecastingModels);
				modelsSubMenu.Tools.Add(btForecastingModels);

				ButtonTool btForecastBalModels			= new ButtonTool(Include.btForecastBalModels);
				btForecastBalModels.SharedProps.Caption			= "Forecast &Balance";
				btForecastBalModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btForecastBalModels);
				modelsSubMenu.Tools.Add(btForecastBalModels);

                ButtonTool btSizeConstraintsModels = new ButtonTool(Include.btSizeConstraintsModels);
				btSizeConstraintsModels.SharedProps.Caption			= "S&ize Constraints";
				btSizeConstraintsModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSizeConstraintsModels);
				modelsSubMenu.Tools.Add(btSizeConstraintsModels);

				ButtonTool btSizeAlternatesModels			= new ButtonTool(Include.btSizeAlternatesModels);
				btSizeAlternatesModels.SharedProps.Caption			= "S&ize &Alternates";
				btSizeAlternatesModels.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSizeAlternatesModels);
				modelsSubMenu.Tools.Add(btSizeAlternatesModels);

                // Add the Cascade1 menu tool to the File menu tools collection.
				this.utmMain.Tools.Add(modelsSubMenu);
				adminMenuTool.Tools.Add(modelsSubMenu);

				//Create Size SubMenu
				PopupMenuTool sizeSubMenu		= new PopupMenuTool(Include.menuSizeSubMenu);
				sizeSubMenu.SharedProps.MergeOrder = 10;
				sizeSubMenu.SharedProps.Caption	= "&Size";

				ButtonTool btSizeGroups					= new ButtonTool(Include.btSizeGroups);
				btSizeGroups.SharedProps.Caption			= "Si&ze Groups";
				btSizeGroups.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSizeGroups);
				sizeSubMenu.Tools.Add(btSizeGroups);

				ButtonTool btSizeCurves					= new ButtonTool(Include.btSizeCurves);
				btSizeCurves.SharedProps.Caption			= "Size Cur&ves";
				btSizeCurves.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSizeCurves);
				sizeSubMenu.Tools.Add(btSizeCurves);

				
				// Add the Cascade1 menu tool to the File menu tools collection.
				this.utmMain.Tools.Add(sizeSubMenu);
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					adminMenuTool.Tools.Add(sizeSubMenu);
				}
				
				adminMenuTool.Tools[Include.btSecurity].InstanceProps.IsFirstInGroup = true;

    			#endregion

				#region ToolsMenu
				// Create the Allocation Menu
				PopupMenuTool toolsMenuTool	= new PopupMenuTool(Include.menuTools);

				toolsMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Tools);
				toolsMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(toolsMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(toolsMenuTool);
				
				ButtonTool btSort					= new ButtonTool(Include.btSort);
				btSort.SharedProps.Caption			= "&Sort";
				btSort.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btSort);

				ButtonTool btFilter					= new ButtonTool(Include.btFilter);
				btFilter.SharedProps.Caption			= "&Filter";
				btFilter.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btFilter);

                // Begin TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
                //ButtonTool btFilterWizard					= new ButtonTool(Include.btFilterWizard);
                //btFilterWizard.SharedProps.Caption			= "Filter Wi&zard";
                //btFilterWizard.SharedProps.MergeOrder		= 10;
                //this.utmMain.Tools.Add(btFilterWizard);
                // End TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.

				ButtonTool btRelease					= new ButtonTool(Include.btRelease);
				btRelease.SharedProps.Caption			= "&Release Resources";
				btRelease.SharedProps.MergeOrder		= 20;
				this.utmMain.Tools.Add(btRelease);

				ButtonTool btRefresh				= new ButtonTool(Include.btRefresh);
				btRefresh.SharedProps.Caption		= "&Refresh";
				btRefresh.SharedProps.MergeOrder	= 20;
				btRefresh.SharedProps.Shortcut		= Shortcut.CtrlR;
				this.utmMain.Tools.Add(btRefresh);

				ButtonTool btAudit				= new ButtonTool(Include.btAudit);
				btAudit.SharedProps.Caption		= "Au&dit";
				btAudit.SharedProps.MergeOrder	= 20;
				this.utmMain.Tools.Add(btAudit);

				// Begin - Reclass
				ButtonTool btAuditReclass				= new ButtonTool(Include.btAuditReclass);
				btAudit.SharedProps.Caption		= "&Audit - Reclass";
				btAudit.SharedProps.MergeOrder	= 20;
				this.utmMain.Tools.Add(btAuditReclass);
				// End

                //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
                // Begin - NodePropertiesOverrides
                ButtonTool btNodePropertiesOverrides = new ButtonTool(Include.btNodePropertiesOverrides);
                btNodePropertiesOverrides.SharedProps.Caption = "&Node Properties Overrides";
                btNodePropertiesOverrides.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btNodePropertiesOverrides);
                // End

                // Begin - ForecastAuditMerchandise
                ButtonTool btForecastAuditMerchandise = new ButtonTool(Include.btForecastAuditMerchandise);
                btForecastAuditMerchandise.SharedProps.Caption = "&Forecast Audit Merchandise";
                btForecastAuditMerchandise.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btForecastAuditMerchandise);
                // End

                // Begin - ForecastAuditMethod
                ButtonTool btForecastAuditMethod = new ButtonTool(Include.btForecastAuditMethod);
                btForecastAuditMethod.SharedProps.Caption = "&Forecast Audit Method";
                btForecastAuditMethod.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btForecastAuditMethod);
                // End

                // Begin - AllocationAudit
                ButtonTool btAllocationAudit = new ButtonTool(Include.btAllocationAudit);
                btAllocationAudit.SharedProps.Caption = "&Allocation Audit";
                btAllocationAudit.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btAllocationAudit);
                // End
                //End Track #6232 - KJohnson

				ButtonTool btTextEditor				= new ButtonTool(Include.btTextEditor);
				btTextEditor.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.frm_TextEditor);
				btTextEditor.SharedProps.MergeOrder	= 20;
				this.utmMain.Tools.Add(btTextEditor);

                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                ButtonTool btEmailMessage = new ButtonTool(Include.btEmailMessage);
                btEmailMessage.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Email_Message);
                btEmailMessage.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btEmailMessage);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                ButtonTool btAllocationAnalysis = new ButtonTool(Include.btAllocationAnalysis);
                btAllocationAnalysis.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Allocation_Analysis);
                btAllocationAnalysis.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btAllocationAnalysis);

                ButtonTool btForecastAnalysis = new ButtonTool(Include.btForecastAnalysis);
                btForecastAnalysis.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Forecast_Analysis);
                btForecastAnalysis.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btForecastAnalysis);
                //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

				ButtonTool btScheduleBrowser				= new ButtonTool(Include.btScheduleBrowser);
				btScheduleBrowser.SharedProps.Caption			= "Schedule &Browser";
				btScheduleBrowser.SharedProps.MergeOrder		= 20;
				this.utmMain.Tools.Add(btScheduleBrowser);

				// Begin TT#1581-MD - stodd - API Header Reconcile
                ButtonTool btProcessControl = new ButtonTool(Include.btProcessControl);
                btProcessControl.SharedProps.Caption = "Process Control";
                btProcessControl.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btProcessControl);
				// End TT#1581-MD - stodd - API Header Reconcile
				// Begin TT#1386-MD - stodd - added Schedulet Job Manager
                ButtonTool btSchedulerJobManager = new ButtonTool(Include.btSchedulerJobManager);
                btSchedulerJobManager.SharedProps.Caption = "Scheduler Job Manager";
                btSchedulerJobManager.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btSchedulerJobManager);
				// End TT#1386-MD - stodd - added Schedulet Job Manager

				ButtonTool btUserOptions				= new ButtonTool(Include.btUserOptions);
				btUserOptions.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.frm_UserOptions);
				btUserOptions.SharedProps.MergeOrder	= 20;
				this.utmMain.Tools.Add(btUserOptions);

                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail  : move Views to Tools
                ButtonTool btViewAdmin = new ButtonTool(Include.btViewAdmin);
                btViewAdmin.SharedProps.Caption = "V&iews";
                btViewAdmin.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btViewAdmin);
                // End TT#231
                // BEGIN Workspace Usability Enhancement
                ButtonTool btRestoreLayout = new ButtonTool(Include.btRestoreLayout);
                btRestoreLayout.SharedProps.Caption = "Restore &Layout";
                btRestoreLayout.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btRestoreLayout);
                // END Workspace Usability Enhancement

				toolsMenuTool.Tools.Add(btSort);
				toolsMenuTool.Tools.Add(btFilter);
                //toolsMenuTool.Tools.Add(btFilterWizard);       // TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
				toolsMenuTool.Tools.Add(btAudit);
                toolsMenuTool.Tools.Add(btAllocationAnalysis); //TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                toolsMenuTool.Tools.Add(btForecastAnalysis); //TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                //toolsMenuTool.Tools.Add(btAuditReclass);      // TT#209 RMatelic - Move to Reports
				toolsMenuTool.Tools.Add(btRelease);
				toolsMenuTool.Tools.Add(btRefresh);
				toolsMenuTool.Tools.Add(btTextEditor);
                toolsMenuTool.Tools.Add(btEmailMessage); //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
				toolsMenuTool.Tools.Add(btScheduleBrowser);
                toolsMenuTool.Tools.Add(btProcessControl);	// TT#1581-MD - stodd - API Header Reconcile
                toolsMenuTool.Tools.Add(btSchedulerJobManager);		// TT#1386-MD - stodd - added Schedulet Job Manager
				toolsMenuTool.Tools.Add(btUserOptions);
                // Begin TT#231 - RMatelic - Add Views to Velocity Matrix and Store Detail  : move Views to Tools  
                toolsMenuTool.Tools.Add(btViewAdmin); 
                // End TT#231 
                toolsMenuTool.Tools.Add(btRestoreLayout);   // Workspace Usability Enhancement
				
				toolsMenuTool.Tools[Include.btSort].InstanceProps.IsFirstInGroup = true;
				toolsMenuTool.Tools[Include.btRelease].InstanceProps.IsFirstInGroup = true;
				
				#endregion

				#region ReportsMenu
				// Create the Allocation Menu
				PopupMenuTool reportsMenuTool	= new PopupMenuTool(Include.menuReports);

				reportsMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Reports);
				reportsMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(reportsMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(reportsMenuTool);

				ButtonTool btCustomReports		= new ButtonTool(Include.btCustomReports);
				btCustomReports.SharedProps.Caption		= "&Custom";
				btCustomReports.SharedProps.MergeOrder	= 20;
				this.utmMain.Tools.Add(btCustomReports);

                //Begin TT#554-MD -jsobek -User Log Level Report
                ButtonTool btReportUserOptionsReview = new ButtonTool(Include.btReportUserOptionsReview);
                btReportUserOptionsReview.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports_User_Options_Review);
                btReportUserOptionsReview.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btReportUserOptionsReview);
                //End TT#554-MD -jsobek -User Log Level Report

                //Begin TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
                ButtonTool btReportAllocationByStore = new ButtonTool(Include.btReportAllocationByStore);
                btReportAllocationByStore.SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports_Allocation_By_Store);
                btReportAllocationByStore.SharedProps.MergeOrder = 20;
                this.utmMain.Tools.Add(btReportAllocationByStore);
                //End TT#739-MD -jsobek -Delete Stores -Allocation by Store Report

				reportsMenuTool.Tools.Add(btAuditReclass);
                //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
                reportsMenuTool.Tools.Add(btNodePropertiesOverrides);
                reportsMenuTool.Tools.Add(btForecastAuditMerchandise);
                //reportsMenuTool.Tools.Add(btForecastAuditMethod);     // TT#209 - RMatelic - exclude for now 
                reportsMenuTool.Tools.Add(btAllocationAudit);
                //End Track #6232 - KJohnson

                reportsMenuTool.Tools.Add(btReportUserOptionsReview); //TT#554-MD -jsobek -User Log Level Report
                reportsMenuTool.Tools.Add(btReportAllocationByStore); //TT#739-MD -jsobek -Delete Stores -Allocation by Store Report

				reportsMenuTool.Tools.Add(btCustomReports);
				#endregion

				#region WindowMenu
				// Create the Allocation Menu
				PopupMenuTool windowMenuTool	= new PopupMenuTool(Include.menuWindow);

				MdiWindowListTool    mdiWindowListTool    = new MdiWindowListTool("WindowList");
				// Specify which window list commands to show.
				mdiWindowListTool.DisplayArrangeIconsCommand    = MdiWindowListCommandDisplayStyle.Hide;
				mdiWindowListTool.DisplayCascadeCommand    = MdiWindowListCommandDisplayStyle.DisplayOnMenu;
				mdiWindowListTool.DisplayCloseWindowsCommand    = MdiWindowListCommandDisplayStyle.DisplayOnMenuAndDialog;
				mdiWindowListTool.DisplayMinimizeCommand    = MdiWindowListCommandDisplayStyle.DisplayOnMenuAndDialog;
				mdiWindowListTool.DisplayTileHorizontalCommand= MdiWindowListCommandDisplayStyle.DisplayOnMenu;
				mdiWindowListTool.DisplayTileVerticalCommand    = MdiWindowListCommandDisplayStyle.DisplayOnMenu;
				
				windowMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Window);
				windowMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;
				
				this.utmMain.Tools.Add(mdiWindowListTool);
				utbMainMenu.Tools.Add(mdiWindowListTool);

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(windowMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(windowMenuTool);

				windowMenuTool.Tools.AddToolRange(new string [] {"WindowList"});
	
				#endregion

				#region HelpMenu
				// Create the Allocation Menu
				PopupMenuTool helpMenuTool	= new PopupMenuTool(Include.menuHelp);

				helpMenuTool.SharedProps.Caption	= MIDText.GetTextOnly(eMIDTextCode.menu_Help);
				helpMenuTool.Settings.IsSideStripVisible	= DefaultableBoolean.False;

				// Add the tool to the root tools collection.
				this.utmMain.Tools.Add(helpMenuTool);


				// Add the tool to the main menu bar.
				utbMainMenu.Tools.Add(helpMenuTool);

				ButtonTool btAbout					= new ButtonTool(Include.btAbout);
				btAbout.SharedProps.Caption			= "&About MIDRetail";
				btAbout.SharedProps.MergeOrder		= 10;
				this.utmMain.Tools.Add(btAbout);

				helpMenuTool.Tools.Add(btAbout);

				helpMenuTool.Tools[Include.btAbout].InstanceProps.IsFirstInGroup = true;
								
				#endregion

        
			}
			catch ( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
		}

        //Begin TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek
        private void AddAssortmentToolbarToMainMenu()
        {
			// BEGIN TT#488-MD - Stodd - Group Allocation
			// Removed "if" 
            //if (MIDRetail.Common.AssortmentActiveProcessToolbarHelper.IsAssortmentInstalled == true)
            //{
			// END TT#488-MD - Stodd - Group Allocation
                MIDRetail.Common.AssortmentActiveProcessToolbarHelper.ListChangedEvent += new MIDRetail.Common.AssortmentActiveProcessToolbarHelper.ListChangedEventHandler(HandleActiveProcessListChanged);


                Infragistics.Win.UltraWinToolbars.UltraToolbar activeProcessToolbar = new Infragistics.Win.UltraWinToolbars.UltraToolbar("Active Process Toolbar");
                Infragistics.Win.UltraWinToolbars.ComboBoxTool activeProcessComboBoxTool = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("activeProcessComboBox");
           

                Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
                Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
                appearance23.BackColor = System.Drawing.SystemColors.Control;
                appearance24.Image = MIDGraphics.GetImage(MIDGraphics.targetImage); //((object)(resources.GetObject("appearance24.Image")));

                activeProcessComboBoxTool.EditAppearance = appearance23;
                activeProcessComboBoxTool.SharedPropsInternal.AppearancesSmall.Appearance = appearance24;
                activeProcessComboBoxTool.SharedPropsInternal.Caption = "Active Process:";
                activeProcessComboBoxTool.SharedPropsInternal.Category = "Assortment";
                activeProcessComboBoxTool.SharedPropsInternal.Width = 300;
                activeProcessComboBoxTool.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;

                if (MIDRetail.Common.AssortmentActiveProcessToolbarHelper.IsAssortmentActiveProcessAccessAllowed == false)
                {
                    activeProcessComboBoxTool.SharedPropsInternal.Enabled = false;
                }

                Infragistics.Win.ValueList activeProcessValueList = new Infragistics.Win.ValueList(0);
                foreach (MIDRetail.Common.AssortmentActiveProcessToolbarHelper.AssortmentScreen screen in MIDRetail.Common.AssortmentActiveProcessToolbarHelper.assortmentScreenList)
                {
                    activeProcessValueList.ValueListItems.Add(new Infragistics.Win.ValueListItem(screen.screenID, screen.screenTitle));
                }

                activeProcessComboBoxTool.ValueList = activeProcessValueList;
				// BEGIN TT#696-MD - Stodd - add "active process"
				utmMain.ToolValueChanged += new Infragistics.Win.UltraWinToolbars.ToolEventHandler(HandleActiveProcessChanged);
				// END TT#696-MD - Stodd - add "active process"
                activeProcessComboBoxTool.Text = "Allocation Workspace";

                utmMain.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] { activeProcessComboBoxTool });

                activeProcessToolbar.DockedPosition = DockedPosition.Bottom;
                activeProcessToolbar.DockedColumn = 10;
                activeProcessToolbar.DockedRow = 0;
                activeProcessToolbar.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {activeProcessComboBoxTool});
                //ultraToolbar.Settings.AllowCustomize = Infragistics.Win.DefaultableBoolean.False;
                activeProcessToolbar.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.True;
                activeProcessToolbar.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.True;
                activeProcessToolbar.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.True;
                //ultraToolbar.Settings.AllowHiding = Infragistics.Win.DefaultableBoolean.True;
                //ultraToolbar.Settings.ToolOrientation = Infragistics.Win.UltraWinToolbars.ToolOrientation.Horizontal;
                activeProcessToolbar.Text = "Active Process Toolbar";
                activeProcessToolbar.ShowInToolbarList = true;
                utmMain.Toolbars.Add(activeProcessToolbar);

			//}		// TT#488-MD - Stodd - Group Allocation
        }
        private bool _resettingActiveProcessList = false;
        private void HandleActiveProcessListChanged(MIDRetail.Common.AssortmentActiveProcessToolbarHelper.ListChangedEventArgs e)
        {
            _resettingActiveProcessList = true;
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)utmMain.Tools["activeProcessComboBox"];
            comboBoxTool.ValueList.ValueListItems.Clear();
			
			// BEGIN TT#696-MD - Stodd - add "active process"
			int width = comboBoxTool.SharedProps.Width;
			float maxWidth = 0;
			// END TT#696-MD - Stodd - add "active process"	
            string lastScreenTitle = string.Empty;
            foreach (MIDRetail.Common.AssortmentActiveProcessToolbarHelper.AssortmentScreen screen in MIDRetail.Common.AssortmentActiveProcessToolbarHelper.assortmentScreenList)
            {
                comboBoxTool.ValueList.ValueListItems.Add(new Infragistics.Win.ValueListItem(screen.screenID, screen.screenTitle));
                lastScreenTitle = screen.screenTitle;
				// BEGIN TT#696-MD - Stodd - add "active process"
				float tmpWidth = System.Drawing.Graphics.FromHwnd(base.Handle).MeasureString(screen.screenTitle, this.Font).Width;
				if (tmpWidth > maxWidth) maxWidth = tmpWidth;
				// END TT#696-MD - Stodd - add "active process"
            }
            _resettingActiveProcessList = false;

			comboBoxTool.SharedPropsInternal.Width = 150 + (int)maxWidth; //giving some room for the label.
			// BEGIN TT#696-MD - Stodd - add "active process"
			comboBoxTool.SelectedIndex = AssortmentActiveProcessToolbarHelper.assortmentScreenList.Count - 1;	// set the combo to the last screen in the list
			MIDRetail.Common.AssortmentActiveProcessToolbarHelper.ActiveProcessChanged(comboBoxTool.Text, (int)comboBoxTool.Value);		// set the active process
			// END TT#696-MD - Stodd - add "active process"
        }
        private void HandleActiveProcessChanged(object sender, Infragistics.Win.UltraWinToolbars.ToolEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "activeProcessComboBox":
                    if (_resettingActiveProcessList == false)
                    {
                        Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool = (Infragistics.Win.UltraWinToolbars.ComboBoxTool)e.Tool;
                        MIDRetail.Common.AssortmentActiveProcessToolbarHelper.ActiveProcessChanged(comboBoxTool.Text, (int)comboBoxTool.Value);
                    }
                    break;
            }
        }
        //End TT#696-MD -Add "Active Process" selection to application to specify where methods should look for selected headers -jsobek

		public void AddMenuOption(string aOption)
		{
			try
			{
				this.utmMain.Tools[aOption].SharedProps.Enabled = true;
				this.utmMain.Tools[aOption].SharedProps.Visible = true;
			}
			catch ( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
		}

		public void RemoveMenuOption(string aOption)
		{
			try
			{
				this.utmMain.Tools[aOption].SharedProps.Enabled = false;
				this.utmMain.Tools[aOption].SharedProps.Visible = false;
			}
			catch ( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}
		}
		#endregion

		private void Explorer_Load(object sender, System.EventArgs e)
		{
            // Begin TT#1269
            StatusBarWindow splashFormLoad = new StatusBarWindow();
            if (_showSplashScreen)
            {
                splashFormLoad.BringToFront();  // TT#3858 - stodd - cross threading error 
            }
            bool status_done = false;  // TT#46 MD - JSmith - User Dashboard
            // End TT#1269

			try
			{
                // Begin TT#1269 - gtaylor
                //bool status_done = false;  // TT#46 MD - JSmith - User Dashboard
                if (_showSplashScreen)
                {
                    ThreadPool.QueueUserWorkItem((x) =>
                    {
                        using (splashFormLoad)
                        {
                            splashFormLoad.Show();
                            Application.DoEvents();
                            while (!status_done)
                                Application.DoEvents();
                            splashFormLoad.Close();
                        }
                    });
                }
            	// BEGIN TT#739 - MD - stodd - delete stores
				//splashFormLoad.StatusText = "Constructing Explorers ...";
				// Begin TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
                //splashFormLoad.StatusText = "Finalizing Initialization ..."; 
                //splashFormLoad.StatusBarValue = 85;
				// END TT#739 - MD - stodd - delete stores
                //Application.DoEvents();
				// End TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
                // End TT#1269

				Cursor.Current = Cursors.WaitCursor;

				// Load Dock and Toolbar Formatting
				InitializeutbMainMenu();

                //splashFormLoad.BringToFront();  // TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual  // TT#3858 - stodd - cross threading error 
                // Begin TT#1269 - gtaylor
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusBarValue = 65;		// TT#739 - MD - stodd - delete stores
                    Application.DoEvents();
                }
                // End TT#1269

                // Begin TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
                //splashFormLoad.BringToFront();    // TT#3858 - stodd - cross threading error 
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusText = "Building Allocation Workspace Explorer ...";
                    splashFormLoad.StatusBarValue = 67;
                    Application.DoEvents();
                }
				// End TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
                LoadExplorerDockFormat();

                // Begin TT#1269 - gtaylor
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusBarValue = 70;		// TT#739 - MD - stodd - delete stores
                    Application.DoEvents();
                }
                // End TT#1269

				LoadExplorerToolbarFormat();


                // Begin TT#1269 - gtaylor
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusBarValue = 75;		// TT#739 - MD - stodd - delete stores
                    Application.DoEvents();
                }
                // End TT#1269

                // Begin TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusText = "Finalizing Initialization ...";
                    splashFormLoad.StatusBarValue = 85;
                    Application.DoEvents();
                }
				// End TT#3840 - JSmith - Logging into MID Application seems a bit longer than usual
				ButtonTool btMemory					= new ButtonTool(Include.btMemory);
				btMemory.SharedProps.Caption		= "Show Hierarchy Memory";
				btMemory.SharedProps.MergeOrder		= 50;
				btMemory.SharedProps.Shortcut		= Shortcut.CtrlShiftM;
//				if (!this.utmMain.Tools.Contains(Include.btMemory))
				// contains doesn't work so swallow the duplicate add the error
				try
				{
					this.utmMain.Tools.Add(btMemory);
				}
				catch
				{
				}

                // Begin TT#1269 - gtaylor
				// BEGIN TT#739 - MD - stodd - delete stores
                //splashFormLoad.StatusText = "Allocation Workspace Loading ...";
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusBarValue = 95;
                    // END TT#739 - MD - stodd - delete stores
                    Application.DoEvents();
                }
                // End TT#1269

                SetText();
               // SetActivityImage(eMIDMessageLevel.Information, true);  // TT#46 MD - JSmith - User Dashboard

                // Begin TT#1269 - gtaylor
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusText = "Application almost ready for use ...";
                    splashFormLoad.StatusBarValue = 98;		// TT#739 - MD - stodd - delete stores
                    Application.DoEvents();
                }
                // End TT#1269

				DetermineSecurity();

                ModifyMessages();

//				// set button state for show login
//				if (_showLogin)
//				{
//					StateButtonTool btShowLogin = (StateButtonTool)this.utmMain.Tools["btShowLogin"];
//					btShowLogin.Checked = true;
//				}
//				else
//				{
//					StateButtonTool btShowLogin = (StateButtonTool)this.utmMain.Tools["btShowLogin"];
//					btShowLogin.Checked = false;
//				}




                //testing all store group
                //int allStoresFilterRID = 51258;
                //filter allStoresFilter = filterDataHelper.LoadExistingFilter(allStoresFilterRID);
                //StoreMgmt.StoreGroup_AddOrUpdate(allStoresFilter, false, false);
                
                //testing store load
                //StoreLoadProcessManager sLoad = new StoreLoadProcessManager(_SAB);
                //sLoad.ProcessDelimitedFile("C:\\MIDRetail\\running\\StoreLoad2.txt", "~".ToCharArray(), true, "~".ToCharArray(), true);


				
				// Format for XP, if applicable
				if ( Environment.OSVersion.Version.Major > 4 && Environment.OSVersion.Version.Minor > 0 && System.IO.File.Exists( System.Windows.Forms.Application.ExecutablePath + ".manifest") )
					FormatForXP(this);


                // Begin TT#1269 - gtaylor
                if (_showSplashScreen)
                {
                    splashFormLoad.StatusBarValue = 100;
                    Application.DoEvents();
                }

                // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
                // Remove any pending messages;
                refreshDisplayed = true;
                RemoveStoreExplorerPendingRefresh();
                // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

                status_done = true;
                // End TT#1269
                // Begin TT#2004 - JSmith - Infragistics layouts getting corrupted resulting in key not found during startup
                _SAB.ApplicationStarted = true;
                // End TT#2004
			}

			catch( Exception exception )
			{
                status_done = true;  // TT#46 MD - JSmith - User Dashboard
				MessageBox.Show(this, exception.Message );
			}

			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

        // Begin TT#4630 - JSmith - Unhandled exception when exporting data. 
        public const int SC_CLOSE = 61536;
        public const int WM_SYSCOMMAND = 274;        

        protected override void WndProc(ref Message msg)
        {
            bool processMessage = true;
            if (msg.Msg == WM_SYSCOMMAND && msg.WParam.ToInt32() == SC_CLOSE)
            {
                processMessage = CheckForSignOff();
            }

            if (processMessage)
            {
                base.WndProc(ref msg);
            }
        }

        private bool CheckForSignOff()
        {
            bool signoff = true;
            if (_SAB.ClientServerSession.UserOptions.ShowSignOffPrompt)
            {
                string message = MIDText.GetText(eMIDTextCode.msg_ConfirmApplicationSignOff);
                if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                {
                    signoff = false;
                }
            }

            return signoff;
        }
        // End TT#4630 - JSmith - Unhandled exception when exporting data. 

		private void Explorer_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            try
            {
                if (e.Cancel == false)  // TT#2092 - gtaylor - Item Max Node Properties Fix - the Explorer should not close or be allowed to close when it is cancelled
                {
                    Cursor.Current = Cursors.WaitCursor;
                    // BEGIN MID Track #5501 - add 'Save Changes?' message
                    if (AllocationWorkspaceExplorer1.OkToClose())
                    {
                        // Begin TT#4630 - JSmith - Unhandled exception when exporting data. 
                        // Begin TT#4243 - RMatelic - Add a prompt when the user closes the MID app using the "x" in the top right corner
                        //if (_SAB.ClientServerSession.UserOptions.ShowSignOffPrompt)
                        //{
                        //    string message = MIDText.GetText(eMIDTextCode.msg_ConfirmApplicationSignOff);
                        //    if (MessageBox.Show(message, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                        //    {
                        //        e.Cancel = true;
                        //        return;
                        //    }
                        //}
                        // End TT#4243
                        // End TT#4630 - JSmith - Unhandled exception when exporting data. 

                        // Save current formatting
                        // Begin TT#2004 - JSmith - Infragistics layouts getting corrupted resulting in key not found during startup
                        //SaveExplorerDockFormat();
                        //SaveExplorerToolbarFormat();
                        if (_SAB.ApplicationStarted)
                        {
                            SaveExplorerDockFormat();
                            SaveExplorerToolbarFormat();
                        }
                        // End TT#2004
                        // Begin MID Track #5576 - KJohnson - Users still being logged into system when they shouldn't

                        // make sure all locks are cleaned up.
                        MIDEnqueue midNQ = new MIDEnqueue();
                        try
                        {
                            midNQ.OpenUpdateConnection();
                            midNQ.Enqueue_DeleteAll(_SAB.ClientServerSession.UserRID, _SAB.ClientServerSession.ThreadID);
                            midNQ.CommitData();
                        }
                        catch
                        {

                        }
                        finally
                        {
                            midNQ.CloseUpdateConnection();
                        }



                        if (_SAB != null)
                        {

                            //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
                            // If the User selected Infragistics Layouts to be cleared, clear them here when the User logs off
                            try
                            {
                                //string sessionUserName = Convert.ToString(_SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture).ToUpper();
                                string sessionUserName = Convert.ToString(_SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID), CultureInfo.CurrentUICulture);

                                int retRows = _SAB.ClientServerSession.ScheduledLayoutDelete(_SAB.ClientServerSession.UserRID, true);
                                {
                                    if (retRows > -1)
                                    {
                                        string msg = MIDText.GetText(eMIDTextCode.msg_SchdLayoutClearProcessed);
                                        msg = msg.Replace("{0}", Convert.ToString(retRows));
                                        msg = msg.Replace("{1}", sessionUserName);
                                        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, "Explorer Closing", true);
                                    }
                                }
                            }
                            catch
                            {

                            }
                            finally
                            {
                  
                                //Begin TT#901-MD -jsobek -Batch Only Mode
                                if (_SAB.clientSocketManager != null)
                                {
                                    //MessageBox.Show("ThreadCount=" + _SAB.clientSocketManager.threadCount.ToString());
                                    _SAB.clientSocketManager.StopClient();
                                }
                                //End TT#901-MD -jsobek -Batch Only Mode
                                _SAB.CloseSessions();
                            }
                            //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
                        }

                        // End MID Track #5576 - KJohnson


                    }
                    else
                    {
                        e.Cancel = true;
                    }
                }   // END MID Track #5501 
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
            }

			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private void ModifyMessages()
		{
			_resourceCustomizer = Infragistics.Win.UltraWinGrid.Resources.Customizer;
			_resourceCustomizer.SetCustomizedString("DataErrorCellUpdateUnableToConvert", 
				_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DataTypeMismatch, false));
			// "LER_Exception_328" is supposedly the 'Not enough context to add row... message
			// which is desiganted as 'unfriendly' per MIDTrack 1862 
			_resourceCustomizer.SetCustomizedString("LER_Exception_328", 
				_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnableToAddRow, false));
		}

		private void FormatForXP( System.Windows.Forms.Control ctl )
		{
			foreach ( System.Windows.Forms.Control c in ctl.Controls )
				FormatForXP(c);
	
			if ( ctl.GetType().BaseType == typeof(ButtonBase) )
				((ButtonBase)ctl).FlatStyle = FlatStyle.System;
		}

		
		private void LoadExplorerDockFormat()
		{
			try
			{
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.explorerDock);
				if (layout.LayoutLength > 0)
				{
					this.udmMain.LoadFromBinary(layout.LayoutStream);
				}
			}

			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}			
		}
		
		private void LoadExplorerToolbarFormat()
		{
			try
			{
				//				if ( System.IO.File.Exists( System.Windows.Forms.Application.UserAppDataPath + @"\" + sExplorerToolbarFormat ) )
				//				{
				//					FileStream fsLayout = new FileStream(System.Windows.Forms.Application.UserAppDataPath + @"\" + sExplorerToolbarFormat, FileMode.OpenOrCreate);
				//					fsLayout.Seek(0, SeekOrigin.Begin);
				//					this.utmMain.LoadFromBinary(fsLayout);
				//					fsLayout.Close();
				//				}
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.explorerToolbar);
				if (layout.LayoutLength > 0)
				{
					this.utmMain.LoadFromBinary(layout.LayoutStream);
				}
				else
				{
					ResetToolbarFormat();
				}
				RemoveMenuOption(Include.btSave);
				RemoveMenuOption(Include.btSaveAs);
   			}

			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}

		private void SetText()
		{
			try
			{
				utmMain.Tools[Include.menuFile].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File);
				utmMain.Tools[Include.btClose].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Close);
				utmMain.Tools[Include.btSave].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Save);
				utmMain.Tools[Include.btSaveAs].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_SaveAs);
                //if (utmMain.Tools.Contains(Include.btLoginAsAdmin))
                //{
                    utmMain.Tools[Include.btLoginAsAdmin].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_LoginAsAdmin); //TT#1521-MD -jsobek -Active Directory Authentication
                //}
				utmMain.Tools[Include.btExit].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_File_Exit);

				utmMain.Tools[Include.menuEdit].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit);
				utmMain.Tools[Include.btCut].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Cut);
				utmMain.Tools[Include.btCopy].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Copy);
				utmMain.Tools[Include.btPaste].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Paste);
				utmMain.Tools[Include.btDelete].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Delete);
                //utmMain.Tools[Include.btClear].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Clear);
				utmMain.Tools[Include.btFind].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Edit_Find);

				utmMain.Tools[Include.menuView].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View);
				utmMain.Tools[Include.btWorkspace].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Allocation_Workspace);
//				utmMain.Tools[Include.btPlanWorkspace].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.);
				utmMain.Tools[Include.btMerchandise].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Merchandise_Explorer);
				utmMain.Tools[Include.btStore].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Store_Explorer);
				utmMain.Tools[Include.btStoreFilter].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Filter_Explorer);
                utmMain.Tools[Include.btHeaderFilter].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Header_Filter_Explorer); //TT#1313-MD -jsobek -Header Filters
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    utmMain.Tools[Include.btAssortmentFilter].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Filter_Explorer); //TT#1313-MD -jsobek -Header Filters
                }
                utmMain.Tools[Include.btTaskList].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Tasklist_Explorer);

				utmMain.Tools[Include.menuAlloc].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation);
				utmMain.Tools[Include.menuAllocSub].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Review);
				utmMain.Tools[Include.btSelect].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Select);
				utmMain.Tools[Include.btStyle].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style);
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					utmMain.Tools[Include.tbbSize].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
					utmMain.Tools[Include.btSize].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
				}
				utmMain.Tools[Include.btSummary].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Summary);
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					utmMain.Tools[Include.btAssortment].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Assortment);
                    utmMain.Tools[Include.btAssortmentWorkspace].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Workspace);
					utmMain.Tools[Include.btAssortmentExplorer].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Explorer);
				}
		
				// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    // Begin TT#950 - MD - stodd - missing "Group Allocation" button
                    utmMain.Tools[Include.btGroupAllocation].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Group_Allocation_Review);
                    // End TT#950 - MD - stodd - missing "Group Allocation" button
                }
				// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

                if (utmMain.Tools.Contains(Include.btUserActivityExplorer))
                {
                    utmMain.Tools[Include.btUserActivityExplorer].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);  // TT#46 MD - JSmith - User Dashboard
                }
				//End Assortment Planning - JScott - Assortment Planning Changes
				
				utmMain.Tools[Include.menuPlanning].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_OTS_Plan);
				utmMain.Tools[Include.btPlanView].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_OTS_Plan_Review);

                utmMain.Tools[Include.menuAdmin].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin);
				utmMain.Tools[Include.btSecurity].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Security);
				utmMain.Tools[Include.btCalendar].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Calendar);
				utmMain.Tools[Include.btHeaderChar].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Header);
				utmMain.Tools[Include.btProductChar].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Product);
				utmMain.Tools[Include.btStoreChar].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Store);
                utmMain.Tools[Include.menuStoreSub].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Store);
				utmMain.Tools[Include.btProfiles].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Store_Profiles);
				utmMain.Tools[Include.btCharacteristics].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Store_Chars);
				utmMain.Tools[Include.btOptions].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Options);
				utmMain.Tools[Include.menuModelsSubMenu].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models);
				utmMain.Tools[Include.btEligModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Eligibility);
				utmMain.Tools[Include.btStkModModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Stock_Modifier);
				utmMain.Tools[Include.btSlsModModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Sales_Modifier);
                utmMain.Tools[Include.btOverrideLowLevel].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Override_Low_Levels);
				// BEGIN MID Track #4370 - John Smith - FWOS Models
				utmMain.Tools[Include.btFWOSModModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_FWOS_Modifier);
				// END MID Track #4370
                // BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                utmMain.Tools[Include.btFWOSMaxModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_FWOS_Max);
                // END TT#108 - MD - DOConnell - FWOS Max Model
				utmMain.Tools[Include.btForecastingModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Forecasting);
				utmMain.Tools[Include.btForecastBalModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Forecast_Balance);
				utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Size_Constraints);
				//utmMain.Tools["btSizeFringeModels"].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Size_Fringe);  // MID Track 3619 Remove Fringe
				utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Models_Size_Alternates);
                utmMain.Tools[Include.menuSizeSubMenu].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Size);
				utmMain.Tools[Include.btSizeGroups].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Size_Groups);
				utmMain.Tools[Include.btSizeCurves].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Admin_Size_Curves);

				utmMain.Tools[Include.menuTools].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools);
				utmMain.Tools[Include.btSort].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Sort);
				utmMain.Tools[Include.btFilter].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Filter);
                //utmMain.Tools[Include.btFilterWizard].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Filter_Wizard);   // Begin TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
				utmMain.Tools[Include.btRelease].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Release_Resources);
				utmMain.Tools[Include.btRefresh].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Refresh);
				utmMain.Tools[Include.btAudit].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Audit);
				// Begin - Reclass
				utmMain.Tools[Include.btAuditReclass].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Audit_Reclass);
				// End
				utmMain.Tools[Include.btTextEditor].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Text_Editor);
                //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                utmMain.Tools[Include.btEmailMessage].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Email_Message);
                //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                utmMain.Tools[Include.btAllocationAnalysis].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Allocation_Analysis);
                utmMain.Tools[Include.btForecastAnalysis].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Forecast_Analysis);
                //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                utmMain.Tools[Include.btProcessControl].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Process_Control);	// TT#1581-MD - stodd - API Header Reconcile
                utmMain.Tools[Include.btSchedulerJobManager].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Scheduler_Job_Manager);	// TT#1386-MD - stodd - added Schedulet Job Manager

                //Begin TT#554-MD -jsobek -User Log Level Report
                utmMain.Tools[Include.btReportUserOptionsReview].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports_User_Options_Review);
                //End TT#554-MD -jsobek -User Log Level Report
                //Begin TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
                utmMain.Tools[Include.btReportAllocationByStore].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports_Allocation_By_Store);
                //End TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
                

				utmMain.Tools[Include.btScheduleBrowser].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Schedule_Browser);
//				utmMain.Tools["btShowLogin"].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Show_Login);
				utmMain.Tools[Include.btUserOptions].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_User_Options);
                // BEGIN Workspace Usability Enhancement   
                utmMain.Tools[Include.btRestoreLayout].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Restore_Layout);
                // END Workspace Usability Enhancement
				utmMain.Tools[Include.menuReports].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports);
				// Begin - Reclass
				utmMain.Tools[Include.btAuditReclass].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Audit_Reclass);
				// End
                //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
                utmMain.Tools[Include.btNodePropertiesOverrides].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_NodePropertiesOverrides);
                utmMain.Tools[Include.btForecastAuditMerchandise].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_ForecastAuditMerchandise);
                utmMain.Tools[Include.btForecastAuditMethod].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_ForecastAuditMethod);
                utmMain.Tools[Include.btAllocationAudit].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_AllocationAudit);
                //End Track #6232 - KJohnson
				utmMain.Tools[Include.btCustomReports].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Reports_Custom);
				
				utmMain.Tools[Include.menuWindow].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Window);

				utmMain.Tools[Include.menuHelp].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Help);
				utmMain.Tools[Include.btAbout].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Help_About);

				utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbMerchandise].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Merchandise_Explorer);
				utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbWorkflowMethods].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Workflow_Method_Explorer);
				utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStores].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Store_Explorer);
				utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAllocationWorkspace].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Allocation_Workspace);
				utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStoreFilters].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Filter_Explorer);
                //Begin TT#1313-MD -jsobek -Header Filters
                utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbHeaderFilters].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Header_Filter_Explorer); 
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled) 
                {
                    utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentFilters].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Filter_Explorer); 
                }
                //End TT#1313-MD -jsobek -Header Filters
                utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbTaskLists].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Tasklist_Explorer);

				utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSummary].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Summary);
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Assortment);
					utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentExplorer].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Explorer);
					utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentWorkspace].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_View_Assortment_Workspace);
				}
				//End Assortment Planning - JScott - Assortment Planning Changes

				// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Group_Allocation_Review);	// TT#488-MD - STodd - Group Allocation - 
                }
				// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

				utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbStyle].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Style);
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSize].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Size);
				}
				utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbOTSPlan].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_OTS_Plan);

                // Begin TT#46 MD - JSmith - User Dashboard
                if (utmMain.Toolbars[Include.tbExplorers].Tools.Contains(Include.tbbUserActivityExplorer))
                {
                    utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbUserActivityExplorer].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);
                }
                if (udmMain.ControlPanes.Contains(Include.tbbUserActivityExplorer))
                {
                    udmMain.ControlPanes[Include.tbbUserActivityExplorer].Text = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);
                    udmMain.ControlPanes[Include.tbbUserActivityExplorer].TextTab = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);
                    udmMain.ControlPanes[Include.tbbUserActivityExplorer].ToolTipCaption = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);
                    udmMain.ControlPanes[Include.tbbUserActivityExplorer].ToolTipTab = MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor);
                }
                // Begin TT#46 MD - JSmith - User Dashboard
			}
			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}

        // Begin TT#46 MD - JSmith - User Dashboard
        //BEGIN TT#46-MD -jsobek -Develop My Activity Log
        //public void SetActivityImage(eMIDMessageLevel aMIDMessageLevel, bool aForce)
        //{
        //    Image image;
        //    if (!aForce &&
        //        (int)aMIDMessageLevel <= (int)_currentActivityLevel)
        //    {
        //        return;
        //    }

        //    _currentActivityLevel = aMIDMessageLevel;

        //    switch (aMIDMessageLevel)
        //    {
        //        case eMIDMessageLevel.Severe:
        //            image = MIDGraphics.GetImage(MIDGraphics.msgSevereImage);

        //            break;
        //        case eMIDMessageLevel.Error:
        //            image = MIDGraphics.GetImage(MIDGraphics.msgErrorImage);

        //            break;
        //        case eMIDMessageLevel.Warning:
        //            image = MIDGraphics.GetImage(MIDGraphics.msgWarningImage);

        //            break;
        //        case eMIDMessageLevel.Edit:
        //            image = MIDGraphics.GetImage(MIDGraphics.msgWarningImage);

        //            break;
        //        default:
        //            image = MIDGraphics.GetImage(MIDGraphics.msgOKImage);
        //            break;
        //    }

        //    utmMain.BeginUpdate();
        //    // update toolbar button
        //    utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbUserActivityExplorer].SharedProps.AppearancesSmall.Appearance.Image = image;
        //    utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbUserActivityExplorer].SharedProps.AppearancesLarge.Appearance.Image = image;

            
        //    // update menu button
        //    //utmMain.Tools[Include.btUserDashboard].SharedProps.AppearancesSmall.Appearance.Image = image;
        //    //utmMain.Tools[Include.btUserDashboard].SharedProps.AppearancesLarge.Appearance.Image = image;

        //    // update control button
        //    //udmMain.ControlPanes[Include.tbbUserDashboard].Settings.Appearance.Image = image;
        //    //udmMain.ControlPanes[Include.tbbUserDashboard].Settings.SelectedTabAppearance.Image = image;
            

        //    utmMain.EndUpdate();
        //}
        //END TT#46-MD -jsobek -Develop My Activity Log
        // End TT#46 MD - JSmith - User Dashboard

		private void DetermineSecurity()
		{
			FunctionSecurityProfile planningSecurity;

			planningSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastReview);

			if (planningSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btPlanView].SharedProps.Enabled = false;
				this.utmMain.Tools[Include.btViewAdmin].SharedProps.Enabled = false; // MID Track #4386 - Justin Bolles - Delete Views
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbOTSPlan].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btPlanView].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.btViewAdmin].SharedProps.Enabled = true;// MID Track #4386 - Justin Bolles - Delete Views
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbOTSPlan].SharedProps.Enabled = true;
			}
			FunctionSecurityProfile allocViewsSecurity;

			allocViewsSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);

			if (allocViewsSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.menuAllocSub].SharedProps.Enabled = false;
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSummary].SharedProps.Enabled = false;
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Enabled = false;
				}
                // Begin TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                else
                {
                    // Begin TT#3242 - JSmith - Key not found: Assortment Error on Login
                    //this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Visible = false;
                    if (this.utmMain.Toolbars[Include.tbAnalysis].Tools.Contains(Include.tbbAssortment))
                    {
                        this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Visible = false;
                    }
                    // End TT#3242 - JSmith - Key not found: Assortment Error on Login
                }
                // End TT#72 MD
				//End Assortment Planning - JScott - Assortment Planning Changes
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbStyle].SharedProps.Enabled = false;
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{	
					this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSize].SharedProps.Enabled = false;
				}
			}
			else
			{
				this.utmMain.Tools[Include.menuAllocSub].SharedProps.Enabled = true;
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSummary].SharedProps.Enabled = true;
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Enabled = true;
				}
                // Begin TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                else
                {
                    if (this.utmMain.Toolbars[Include.tbAnalysis].Tools.Contains(Include.tbbAssortment))
                    {
                    this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Visible = true;
                    }
                }

				// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    // Begin TT#950 - MD - stodd - missing "Group Allocation" button
                    this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Enabled = true;
                    if (this.utmMain.Toolbars[Include.tbAnalysis].Tools.Contains(Include.tbbGroupAllocation))
                    {
                        this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Visible = true;
                    }
                    this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Visible = true;
                    // End TT#950 - MD - stodd - missing "Group Allocation" button
                }
				// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

                // End TT#72 MD
				//End Assortment Planning - JScott - Assortment Planning Changes
				this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbStyle].SharedProps.Enabled = true;
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					this.utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbSize].SharedProps.Enabled = true;
				}
			}
            
			//View - Allocation Workspace
			FunctionSecurityProfile functionSecurity;
			// BEGIN MID Track #4357 - security for interfaced and non-interfaced headers
			FunctionSecurityProfile interfacedFunctionSecurity, nonInterfacedFunctionSecurity;
//			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeaders);
			nonInterfacedFunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersNonInterfaced);
			interfacedFunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersInterfaced);

//			if (functionSecurity.AccessDenied)
			if (nonInterfacedFunctionSecurity.AccessDenied && interfacedFunctionSecurity.AccessDenied)
			// END MID Track #4357
			{
				this.utmMain.Tools[Include.btWorkspace].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btWorkspace].SharedProps.Enabled = true;				
			}

			//Allocation - Review
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.menuAllocSub].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.menuAllocSub].SharedProps.Enabled = true;				
			}

			//			//Allocation - Scheduler
			//			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationProcessScheduler);
			//
			//			if (FunctionSecurity == eSecurityLevel.NoAccess || FunctionSecurity == eSecurityLevel.NotSpecified)
			//			{
			//				this.utmMain.Tools["btAllocationSchedule"].SharedProps.Enabled = false;	
			//			}
			//			else
			//			{
			//				this.utmMain.Tools["btAllocationSchedule"].SharedProps.Enabled = true;				
			//			}

			//			//OTS Plan - Scheduler
			//			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastProcessScheduler);
			//
			//			if (FunctionSecurity == eSecurityLevel.NoAccess || FunctionSecurity == eSecurityLevel.NotSpecified)
			//			{
			//				this.utmMain.Tools["btOTSPlanSchedule"].SharedProps.Enabled = false;
			//			}
			//			else
			//			{
			//				this.utmMain.Tools["btOTSPlanSchedule"].SharedProps.Enabled = true;				
			//			}

			//OTS Plan - Review
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastReview);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btPlanView].SharedProps.Enabled = false;
				this.utmMain.Tools[Include.btViewAdmin].SharedProps.Enabled = false;// MID Track #4386 - Justin Bolles - Delete Views
			}
			else
			{
				this.utmMain.Tools[Include.btPlanView].SharedProps.Enabled = true;
			}	
		
			//Admin - Security
			FunctionSecurityProfile groupSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSecurityGroups);
			FunctionSecurityProfile userSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSecurityUsers);

			if (groupSecurity.AccessDenied &&
				userSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btSecurity].SharedProps.Enabled = false;	

			}
			else
			{
				this.utmMain.Tools[Include.btSecurity].SharedProps.Enabled = true;				
			}	
		
			//Admin - Calender
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendar);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btCalendar].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btCalendar].SharedProps.Enabled = true;				
			}		

			//Admin - Characteristics
			FunctionSecurityProfile headerCharfunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHeadersCharacteristics);
			FunctionSecurityProfile productCharfunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminHierarchiesCharacteristics);
			FunctionSecurityProfile storeCharfunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);

			this.utmMain.Tools[Include.menuCharacteristicSubMenu].SharedProps.Enabled = false;
			if (headerCharfunctionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btHeaderChar].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btHeaderChar].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.menuCharacteristicSubMenu].SharedProps.Enabled = true;
			}
			if (productCharfunctionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btProductChar].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btProductChar].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.menuCharacteristicSubMenu].SharedProps.Enabled = true;
			}
			if (storeCharfunctionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btStoreChar].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btStoreChar].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.menuCharacteristicSubMenu].SharedProps.Enabled = true;
			}
	
			//Admin - Store
			this.utmMain.Tools[Include.menuStoreSub].SharedProps.Enabled = false;

			//Admin - Store Profiles
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStores);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btProfiles].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.menuStoreSub].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.btProfiles].SharedProps.Enabled = true;				
			}		

			//Admin - Store Characteristics
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresCharacteristics);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btCharacteristics].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.menuStoreSub].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.btCharacteristics].SharedProps.Enabled = true;				
			}	
	
			// BEGIN MID ISSUE # 2568 - stodd
			//Admin - Store Profiles
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoresProfiles);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btProfiles].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.menuStoreSub].SharedProps.Enabled = true;
				this.utmMain.Tools[Include.btProfiles].SharedProps.Enabled = true;				
			}	
			// END MID ISSUE # 2568 - stodd

			//Admin - Options
//			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptions);
			FunctionSecurityProfile securityCompanyInfo = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsCompanyInfo);
			FunctionSecurityProfile securityDisplayOptions = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsDisplay);
			FunctionSecurityProfile securityOTSPlanVersions = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsOTSVersions);
			FunctionSecurityProfile securityAllocationDefaults = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminGlobalOptionsAlDefaults);

			if (securityCompanyInfo.AccessDenied &&
				securityDisplayOptions.AccessDenied &&
				securityOTSPlanVersions.AccessDenied &&
				securityAllocationDefaults.AccessDenied)
			{
				this.utmMain.Tools[Include.btOptions].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btOptions].SharedProps.Enabled = true;				
			}	
	
			//Admin - Size Groups
			// Don't put size groups on menu if no size
			FunctionSecurityProfile functionSecuritySizeGroups;
			FunctionSecurityProfile functionSecuritySizeCurves;

			if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
			{
				functionSecuritySizeGroups = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeGroups);

				if (functionSecuritySizeGroups.AccessDenied)
				{
					this.utmMain.Tools[Include.btSizeGroups].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btSizeGroups].SharedProps.Enabled = true;				
				}
				
				functionSecuritySizeCurves = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeCurves);

				if (functionSecuritySizeCurves.AccessDenied)
				{
					this.utmMain.Tools[Include.btSizeCurves].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btSizeCurves].SharedProps.Enabled = true;				
				}

				if (!functionSecuritySizeGroups.AccessDenied ||
					!functionSecuritySizeCurves.AccessDenied)
				{
					functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSize);

					if (functionSecurity.AccessDenied)
					{
						this.utmMain.Tools[Include.menuSizeSubMenu].SharedProps.Enabled = false;	
					}
					else
					{
						this.utmMain.Tools[Include.menuSizeSubMenu].SharedProps.Enabled = true;				
					}
				}
				else
				{
					this.utmMain.Tools[Include.menuSizeSubMenu].SharedProps.Enabled = false;	
				}
			}
			else
			{
				this.utmMain.Tools[Include.btSizeGroups].SharedProps.Enabled = false;	
				this.utmMain.Tools[Include.btSizeCurves].SharedProps.Enabled = false;	
				this.utmMain.Tools[Include.menuSizeSubMenu].SharedProps.Enabled = false;	
			}
	
			//Admin - Models
			FunctionSecurityProfile functionSecurityEligibilityModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsEligibility);
			FunctionSecurityProfile functionSecuritySalesModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsSalesModifier);
			FunctionSecurityProfile functionSecurityStockModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsStockModifier);
            FunctionSecurityProfile functionSecurityOverrideLowLevelModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
			// BEGIN MID Track #4370 - John Smith - FWOS Models
			FunctionSecurityProfile functionSecurityFWOSModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsFWOSModifier);
			// END MID Track #4370
            // BEGIN TT#108 - MD - DOConnell - FWOS Max Model Enhancment
            FunctionSecurityProfile functionSecurityFWOSMaxModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminFWOSMax);
            // END TT#108 - MD - DOConnell - FWOS Max Model Enhancment
			FunctionSecurityProfile functionSecuritySizeConstraintsModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeConstraints);
			//FunctionSecurityProfile functionSecuritySizeFringeModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeFringe);  // MID Track 3619 Remove Fringe
			FunctionSecurityProfile functionSecuritySizeAlternatesModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeAlternates);
			FunctionSecurityProfile functionSecurityForecastingModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsForecasting);
			FunctionSecurityProfile functionSecurityForecastBalanceModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsForecastBalance);
            // Security for the the forecasting models are being handled differently for now.
			// For clients the form is always view only. This is handled by the MIDOnlyFunctions config setting.
			// If turned on, the form will override to full control for MID.
            // Begin #338 - JSmith - Remove Forecast Balance Models from menu
            //functionSecurityForecastingModels.SetFullControl();
            if (_SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.CustomVariablesDefined)
            {
                functionSecurityForecastingModels.SetFullControl();
            }
            // End #338
            // set access denied until models are being used
            // Begin TT#1741 - JSmith - Show Forecast Balance Model Administration Function
            //functionSecurityForecastBalanceModels.SetAccessDenied();
            // End TT#1741


			if (functionSecurityEligibilityModels.AccessDenied &&
				functionSecuritySalesModels.AccessDenied &&
				functionSecurityStockModels.AccessDenied &&
                functionSecurityOverrideLowLevelModels.AccessDenied &&
				// BEGIN MID Track #4370 - John Smith - FWOS Models
				functionSecurityFWOSModels.AccessDenied &&
				// END MID Track #4370
				functionSecuritySizeConstraintsModels.AccessDenied &&
				functionSecurityForecastingModels.AccessDenied &&
                functionSecurityForecastBalanceModels.AccessDenied)
			{
				this.utmMain.Tools[Include.menuModelsSubMenu].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.menuModelsSubMenu].SharedProps.Enabled = true;	

				if (functionSecurityEligibilityModels.AccessDenied)
				{
					this.utmMain.Tools[Include.btEligModels].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btEligModels].SharedProps.Enabled = true;				
				}

				if (functionSecuritySalesModels.AccessDenied)
				{
					this.utmMain.Tools[Include.btSlsModModels].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btSlsModModels].SharedProps.Enabled = true;				
				}

				if (functionSecurityStockModels.AccessDenied)
				{
					this.utmMain.Tools[Include.btStkModModels].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btStkModModels].SharedProps.Enabled = true;				
				}

                if (functionSecurityOverrideLowLevelModels.AccessDenied)
                {
                    this.utmMain.Tools[Include.btOverrideLowLevel].SharedProps.Enabled = false;
                }
                else
                {
                    this.utmMain.Tools[Include.btOverrideLowLevel].SharedProps.Enabled = true;
                }

				// BEGIN TT#108 - MD - DOConnell - FWOS Max Model
				if (functionSecurityFWOSModels.AccessDenied)
				{
					this.utmMain.Tools[Include.btFWOSModModels].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btFWOSModModels].SharedProps.Enabled = true;				
				}
				// END MID Track #4370

                // BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                if (functionSecurityFWOSModels.AccessDenied)
                {
                    this.utmMain.Tools[Include.btFWOSMaxModels].SharedProps.Enabled = false;
                }
                else
                {
                    this.utmMain.Tools[Include.btFWOSMaxModels].SharedProps.Enabled = true;
                }
                // END TT#108 - MD - DOConnell - FWOS Max Model

				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{
					this.utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Visible = true;
//					this.utmMain.Tools["btSizeFringeModels"].SharedProps.Visible = true;
					this.utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Visible = true;
					if (functionSecuritySizeConstraintsModels.AccessDenied)
					{
						this.utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Enabled = false;	
					}
					else 
					{
						this.utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Enabled = true;				
					}
					// begin MID Track 3619 Remove Fringe
					//if (functionSecuritySizeFringeModels.AccessDenied)
					//{
					//	this.utmMain.Tools["btSizeFringeModels"].SharedProps.Enabled = false;	
					//}
					//else
					//{
					//	this.utmMain.Tools["btSizeFringeModels"].SharedProps.Enabled = true;				
					//}
					// end MID Track 3619 Remove Fringe
					if (functionSecuritySizeAlternatesModels.AccessDenied)
					{
						this.utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Enabled = false;	
					}
					else
					{
						this.utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Enabled = true;				
					}
				}
				else
				{
					this.utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Visible = false;
//					this.utmMain.Tools["btSizeFringeModels"].SharedProps.Visible = false;
					this.utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Visible = false;
				}

				if (!_SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.CustomVariablesDefined)
				{
					this.utmMain.Tools[Include.btForecastingModels].SharedProps.Visible = false;
				}
				else if (functionSecurityForecastingModels.AccessDenied)
				{
					this.utmMain.Tools[Include.btForecastingModels].SharedProps.Visible = true;
					this.utmMain.Tools[Include.btForecastingModels].SharedProps.Enabled = false;	
				}
				else
				{
					this.utmMain.Tools[Include.btForecastingModels].SharedProps.Visible = true;
					this.utmMain.Tools[Include.btForecastingModels].SharedProps.Enabled = true;				
				}
	
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				// Begin MID Track #5078 - JSmith - Remove forecast balance models
                // Begin TT#1741 - JSmith - Show Forecast Balance Model Administration Function
                if (!_SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.CustomVariablesDefined)
				{
					this.utmMain.Tools["btForecastBalModels"].SharedProps.Visible = false;
				}
                else if (functionSecurityForecastBalanceModels.AccessDenied)
                {
                    this.utmMain.Tools["btForecastBalModels"].SharedProps.Visible = true;
                    this.utmMain.Tools["btForecastBalModels"].SharedProps.Enabled = false;
                }
                else
                {
                    this.utmMain.Tools["btForecastBalModels"].SharedProps.Visible = true;
                    this.utmMain.Tools["btForecastBalModels"].SharedProps.Enabled = true;
                }
                // End TT#1741
				// End MID Track #5078
				// END MID Track #5647
			}	
	
			// Stodd 03-17-2005 - adding size models
			//==================================================
			// If SIZE is NOT on, disable these size models
			//==================================================
			if (!_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
			{
				this.utmMain.Tools[Include.btSizeConstraintsModels].SharedProps.Visible = false;	
				// this.utmMain.Tools["btSizeFringeModels"].SharedProps.Visible = false; // MID Track 3619 Remove Fringe	
				this.utmMain.Tools[Include.btSizeAlternatesModels].SharedProps.Visible = false;	
			}

			//Tools - Release Resources
			FunctionSecurityProfile functionSecurityPersonal = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesPersonal);
			FunctionSecurityProfile functionSecurityOthers = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsReleaseResourcesOthers);

			if (functionSecurityPersonal.AccessDenied &&
				functionSecurityOthers.AccessDenied)
			{
				this.utmMain.Tools[Include.btRelease].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btRelease].SharedProps.Enabled = true;				
			}	

			//Tools - Audit
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAuditViewer);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btAudit].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btAudit].SharedProps.Enabled = true;
			}
			// Begin - Reclass
			//Tools - Audit Reclass
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAuditReclassViewer);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btAuditReclass].SharedProps.Enabled = false;
			}
			else
			{
				this.utmMain.Tools[Include.btAuditReclass].SharedProps.Enabled = true;
			}
			// End
	
			//Tools - Text Editor
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsTextEditor);

			if (functionSecurity.AccessDenied)
			{
				this.utmMain.Tools[Include.btTextEditor].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btTextEditor].SharedProps.Enabled = true;				
			}


            //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsEmailMessage);

            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btEmailMessage].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btEmailMessage].SharedProps.Enabled = true;
            }
            //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

            //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAllocationAnalysis);

            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btAllocationAnalysis].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btAllocationAnalysis].SharedProps.Enabled = true;
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsForecastAnalysis);

            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btForecastAnalysis].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btForecastAnalysis].SharedProps.Enabled = true;
            }
            //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis

	
			//Tools - ScheduleBrowser
			functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsSchedulerBrowser);

			if (functionSecurity.AccessDenied || _SAB.SchedulerServerSession == null)
			{
				this.utmMain.Tools[Include.btScheduleBrowser].SharedProps.Enabled = false;	
			}
			else
			{
				this.utmMain.Tools[Include.btScheduleBrowser].SharedProps.Enabled = true;				
			}
			
			// Begin TT#1581-MD - stodd - API Header Reconcile
            //Tools - Process Control
            //functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsProcessControl);
            //if (functionSecurity.AccessDenied)

            if (_SAB.ClientServerSession.UserRID == Include.AdministratorUserRID)
            {
                this.utmMain.Tools[Include.btProcessControl].SharedProps.Enabled = true;
            }
            else
            {
                this.utmMain.Tools[Include.btProcessControl].SharedProps.Enabled = false;
            }
			// End TT#1581-MD - stodd - API Header Reconcile

			// Begin TT#1386-MD - stodd - added Schedulet Job Manager
            if (_SAB.ClientServerSession.UserRID != Include.AdministratorUserRID || _SAB.SchedulerServerSession == null)
            {
                this.utmMain.Tools[Include.btSchedulerJobManager].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btSchedulerJobManager].SharedProps.Enabled = true;
            }
			// End TT#1386-MD - stodd - added Schedulet Job Manager
	
			//Tools - Filter
			FunctionSecurityProfile userFilterSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
			FunctionSecurityProfile globalFilterSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

//			if ((userFilterSecurity.AccessDenied) &&
//				(globalFilterSecurity.AccessDenied))
//			{
//				this.utmMain.Tools[Include.btFilter].SharedProps.Enabled = false;
//			}
//			else
//			{
//				this.utmMain.Tools[Include.btFilter].SharedProps.Enabled = true;
//			}
			this.utmMain.Tools[Include.btFilter].SharedProps.Visible = false;

			//Begin Track #5305 - JScott - Filter Wizard User having View only access is able to create a filter.
			//if (userFilterSecurity.AccessDenied)
            // Begin TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
            //if (!userFilterSecurity.AllowUpdate && !globalFilterSecurity.AllowUpdate)
            ////End Track #5305 - JScott - Filter Wizard User having View only access is able to create a filter.
            //{
            //    this.utmMain.Tools[Include.btFilterWizard].SharedProps.Enabled = false;
            //}
            //else
            //{
            //    this.utmMain.Tools[Include.btFilterWizard].SharedProps.Enabled = true;
            //}
            // End TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.

// BEGIN Track #3650 - JSmith - Login security
            //functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsShowLogin);

            //if (functionSecurity.AccessDenied ||
            //    !_SAB.ClientServerSession.GlobalOptions.UseWindowsLogin)
            //{
            //    this.utmMain.Tools["btShowLogin"].SharedProps.Enabled = false;	
            //    this.utmMain.Tools["btShowLogin"].SharedProps.Visible = false;
            //}
            //else
            //{
            //    this.utmMain.Tools["btShowLogin"].SharedProps.Enabled = true;	
            //    this.utmMain.Tools["btShowLogin"].SharedProps.Visible = true;
            //}
// END Track #3650

            // Begin TT#209 - Unrelated to specific issue ; no security dfeined for Reports
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsAuditReclass);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btAuditReclass].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btAuditReclass].SharedProps.Enabled = true;
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsAllocationAudit);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btAllocationAudit].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btAllocationAudit].SharedProps.Enabled = true;
            }

            //Begin TT#554-MD -jsobek -User Log Level Report
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsUserOptionsReview);

            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btReportUserOptionsReview].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btReportUserOptionsReview].SharedProps.Enabled = true;
            }
            //End TT#554-MD -jsobek -User Log Level Report

            //Begin TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsAllocationByStore);

            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btReportAllocationByStore].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btReportAllocationByStore].SharedProps.Enabled = true;
            }
            //End TT#739-MD -jsobek -Delete Stores -Allocation by Store Report

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsCustom);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btCustomReports].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btCustomReports].SharedProps.Enabled = true;
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsNodePropertiesOverrides);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btNodePropertiesOverrides].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btNodePropertiesOverrides].SharedProps.Enabled = true;
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsForecastAuditMerchandise);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Tools[Include.btForecastAuditMerchandise].SharedProps.Enabled = false;
            }
            else
            {
                this.utmMain.Tools[Include.btForecastAuditMerchandise].SharedProps.Enabled = true;
            }
            // End TT#209

            // Begin TT#2 - JSmith - Assortment Security
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersMerchandise);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbMerchandise].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btMerchandise].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbMerchandise))
                {
                    if (!udmMain.ControlPanes[Include.tbbMerchandise].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbMerchandise].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbMerchandise].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btMerchandise].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersWorkflowMethod);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbWorkflowMethods].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btWorkflow].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbWorkflowMethods))
                {
                    if (!udmMain.ControlPanes[Include.tbbWorkflowMethods].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbWorkflowMethods].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbWorkflowMethods].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btWorkflow].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersStore);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStores].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btStore].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbStores))
                {
                    if (!udmMain.ControlPanes[Include.tbbStores].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbStores].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStores].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btStore].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAllocationWorkspace);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAllocationWorkspace].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btWorkspace].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbAllocationWorkspace))
                {
                    if (!udmMain.ControlPanes[Include.tbbAllocationWorkspace].Closed)
                    {
                       udmMain.ControlPanes[Include.tbbAllocationWorkspace].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAllocationWorkspace].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btWorkspace].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersFilterStore);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStoreFilters].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btStoreFilter].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbStoreFilters))
                {
                    if (!udmMain.ControlPanes[Include.tbbStoreFilters].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbStoreFilters].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbStoreFilters].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btStoreFilter].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            //Begin TT#1313-MD -jsobek -Header Filters
            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersFilterHeader);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbHeaderFilters].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btHeaderFilter].SharedProps.Enabled = false;   
                if (ContainsDockAreaPane(Include.tbbHeaderFilters))
                {
                    if (!udmMain.ControlPanes[Include.tbbHeaderFilters].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbHeaderFilters].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbHeaderFilters].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btHeaderFilter].SharedProps.Enabled = true;   
            }
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
                functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersFilterAssortment);
                if (functionSecurity.AccessDenied)
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentFilters].SharedProps.Enabled = false;
                    this.utmMain.Tools[Include.btAssortmentFilter].SharedProps.Enabled = false;
                    if (ContainsDockAreaPane(Include.tbbAssortmentFilters))
                    {
                        if (!udmMain.ControlPanes[Include.tbbAssortmentFilters].Closed)
                        {
                            udmMain.ControlPanes[Include.tbbAssortmentFilters].Close();
                        }
                    }
                }
                else
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentFilters].SharedProps.Enabled = true;
                    this.utmMain.Tools[Include.btAssortmentFilter].SharedProps.Enabled = true;
                }
            }
            //End TT#1313-MD -jsobek -Header Filters

            functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersTasklist);
            if (functionSecurity.AccessDenied)
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbTaskLists].SharedProps.Enabled = false;
                this.utmMain.Tools[Include.btTaskList].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                if (ContainsDockAreaPane(Include.tbbTaskLists))
                {
                    if (!udmMain.ControlPanes[Include.tbbTaskLists].Closed)
                    {
                        udmMain.ControlPanes[Include.tbbTaskLists].Close();
                    }
                }
            }
            else
            {
                this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbTaskLists].SharedProps.Enabled = true;
                this.utmMain.Tools[Include.btTaskList].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            }

            // Begin TT#88 MD - JSmith - Key not found exception logging into application.
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
            {
            // End TT#88 MD
                functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentExplorer);
                if (functionSecurity.AccessDenied)
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentExplorer].SharedProps.Enabled = false;
                    this.utmMain.Tools[Include.btAssortmentExplorer].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                    if (ContainsDockAreaPane(Include.tbbAssortmentExplorer))
                    {
                        if (!udmMain.ControlPanes[Include.tbbAssortmentExplorer].Closed)
                        {
                            udmMain.ControlPanes[Include.tbbAssortmentExplorer].Close();
                        }
                    }
                }
                else
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentExplorer].SharedProps.Enabled = true;
                    this.utmMain.Tools[Include.btAssortmentExplorer].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                }

                functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ExplorersAssortmentWorkspace);
                if (functionSecurity.AccessDenied)
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentWorkspace].SharedProps.Enabled = false;
                    this.utmMain.Tools[Include.btAssortmentWorkspace].SharedProps.Enabled = false;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                    if (ContainsDockAreaPane(Include.tbbAssortmentWorkspace))
                    {
                        if (!udmMain.ControlPanes[Include.tbbAssortmentWorkspace].Closed)
                        {
                            udmMain.ControlPanes[Include.tbbAssortmentWorkspace].Close();
                        }
                    }
                }
                else
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentWorkspace].SharedProps.Enabled = true;
                    this.utmMain.Tools[Include.btAssortmentWorkspace].SharedProps.Enabled = true;   // TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                }


                // Begin TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
                functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                if (functionSecurity.AccessDenied)
                {
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Enabled = false;
                }
                else
                {
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Enabled = true;
                }
                // End TT#72 MD
                // Begin TT#88 MD - JSmith - Key not found exception logging into application.
            }
            // Begin TT#72 MD - JSmith - Assortment Explorer should not display when user does not have security for assortment
            else
            {
                if (this.utmMain.Toolbars[Include.tbAnalysis].Tools.Contains(Include.tbbAssortment))
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentExplorer].SharedProps.Visible = false;
                    this.utmMain.Toolbars[Include.tbExplorers].Tools[Include.tbbAssortmentWorkspace].SharedProps.Visible = false;
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Visible = false;
                    // Begin TT#236 MD - JSmith - Create License Key option for Assortment
                    this.utmMain.Tools[Include.btAssortmentExplorer].SharedProps.Visible = false;
                    this.utmMain.Tools[Include.btAssortmentWorkspace].SharedProps.Visible = false;
                    this.utmMain.Tools[Include.btAssortment].SharedProps.Visible = false;
                    // End TT#236 MD
                }
            }
            // End TT#72 MD
            // End TT#88 MD
            // End TT#2 - JSmith - Assortment Security

			// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
            if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
            {
                // Begin TT#950 - MD - stodd - missing "Group Allocation" button
                functionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationReview); // TT#1007 - md - stodd - change group allocation security - 
                if (functionSecurity.AccessDenied)
                {
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Enabled = false;
                }
                else
                {
                    utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbGroupAllocation].SharedProps.Enabled = true;
                }
                // End TT#950 - MD - stodd - missing "Group Allocation" button
            }
			// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

            SecurityAdmin secAdmin = new SecurityAdmin();
            bool isUserInAdminGroup = secAdmin.IsUserInAdminGroup(_SAB.ClientServerSession.UserRID);
            if (isUserInAdminGroup)
            {
                this.utmMain.Tools[Include.btLoginAsAdmin].SharedProps.Visible = true;
                this.utmMain.Tools[Include.btLoginAsAdmin].SharedProps.Enabled = true;
            }
            else
            {
                this.utmMain.Tools[Include.btLoginAsAdmin].SharedProps.Visible = false;
                this.utmMain.Tools[Include.btLoginAsAdmin].SharedProps.Enabled = false;
            }
		}

        // Begin TT#2 - JSmith - Assortment Security
        private bool ContainsDockAreaPane(string aKey)
        {
            foreach (DockAreaPane dap in this.udmMain.DockAreas)
            {
                if (dap.Key == aKey)
                {
                    return true;
                }
            }
            return false;
        }
        // End TT#2 - JSmith - Assortment Security

		private void SaveExplorerDockFormat()
		{
			try
			{
				//				FileStream fsLayout = new FileStream(System.Windows.Forms.Application.UserAppDataPath + @"\" + sExplorerDockFormat, FileMode.OpenOrCreate);
				//				fsLayout.Seek(0, SeekOrigin.Begin); 
				//				this.udmMain.SaveAsBinary(fsLayout);
				//				fsLayout.Close();
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				MemoryStream layoutStream = new MemoryStream();
				this.udmMain.SaveAsBinary(layoutStream);
				layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.explorerDock, layoutStream);
			}

			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private bool getMIDOnlyFlag()
		{
			try
			{
				bool returnVar = false;
                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //string MIDOnlyFunctionsStr = System.Configuration.ConfigurationManager.AppSettings["MIDOnlyFunctions"];
                string MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
                // End TT#1054

				if (MIDOnlyFunctionsStr != null)
				{
					MIDOnlyFunctionsStr = MIDOnlyFunctionsStr.ToLower();

					if (MIDOnlyFunctionsStr == "true" || MIDOnlyFunctionsStr == "yes" || MIDOnlyFunctionsStr == "t" || MIDOnlyFunctionsStr == "y" || MIDOnlyFunctionsStr == "1")
					{
						returnVar = true;
					}
					else
					{
						returnVar = false;
					}
				}
				else
				{
					returnVar = false;
				}

				return returnVar;
			}
			catch
			{
				throw;
			}
		}
		// END MID Track #5647

		private void SaveExplorerToolbarFormat()
		{
			try
			{
				//				FileStream fsLayout = new FileStream(System.Windows.Forms.Application.UserAppDataPath + @"\" + sExplorerToolbarFormat, FileMode.OpenOrCreate);
				//				fsLayout.Seek(0, SeekOrigin.Begin); 
				//				this.utmMain.SaveAsBinary(fsLayout, true);
				//				fsLayout.Close();	
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				MemoryStream layoutStream = new MemoryStream();
				this.utmMain.SaveAsBinary(layoutStream, true);
				layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.explorerToolbar, layoutStream);
			}

			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}
		
		private void ResetToolbarFormat()
		{
			try
			{			
				// Force all tools to show image and text, user can customize later	
				foreach( ToolBase tool in this.utmMain.Tools )
					tool.SharedProps.DisplayStyle = ToolDisplayStyle.ImageAndText;
				
				// Add all default tools to the toolbars
//				this.utmMain.Toolbars["Tools"].Tools.AddTool("Quick Filter");
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbMerchandise);
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbWorkflowMethods);
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbStores);
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbAllocationWorkspace);
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbStoreFilters);
                //Begin TT#1313-MD -jsobek -Header Filters
                this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbHeaderFilters); 
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
                {
                    this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbAssortmentFilters); 
                }
                //End TT#1313-MD -jsobek -Header Filters
				this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbTaskLists);
				// Begin TT#2 - stodd - assortment
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbAssortmentExplorer);
					this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbAssortmentWorkspace);  // TT#2 Assortment Planning
				}
				// End TT#2 - stodd - assortment
                this.utmMain.Toolbars[Include.tbExplorers].Tools.AddTool(Include.tbbUserActivityExplorer);  // TT#46 MD - JSmith - User Dashboard

				//	this.utmMain.Toolbars["Configuration"].DockedPosition = DockedPosition.Bottom;
				//	this.utmMain.Toolbars["Configuration"].Tools.AddTool("Calendar");
				//	this.utmMain.Toolbars["Configuration"].Tools.AddTool("Store Profiles");

				//this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool("Chain Plan");
				this.utmMain.Toolbars[Include.tbAnalysis].DockedPosition = DockedPosition.Bottom;
				this.utmMain.Toolbars[Include.tbAnalysis].DockedRow = 0;
				this.utmMain.Toolbars[Include.tbAnalysis].DockedColumn = 1;
				this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbOTSPlan);
				//	this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool("Charts");
				//	this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool("OTS Plan Method");
				//this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool("Gen Alloc Method");

				this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbSummary);
				//Begin Assortment Planning - JScott - Assortment Planning Changes
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbAssortment);
				}
				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.AssortmentInstalled)
				{
					utmMain.Toolbars[Include.tbAnalysis].Tools[Include.tbbAssortment].SharedProps.Caption = MIDText.GetTextOnly(eMIDTextCode.menu_Allocation_Assortment);
				}
				//End Assortment Planning - JScott - Assortment Planning Changes

				// Begin TT#1247-MD - stodd - Add Group Allocation as a License Key option -
                if (_SAB.ClientServerSession.GlobalOptions.AppConfig.GroupAllocationInstalled)
                {
                    this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbGroupAllocation);		// TT#488-MD - STodd - Group Allocation - 
                }
				// End TT#1247-MD - stodd - Add Group Allocation as a License Key option -

				if (_SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
				{	
					this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbSize);
				}
				this.utmMain.Toolbars[Include.tbAnalysis].Tools.AddTool(Include.tbbStyle);
				//	this.utmMain.Toolbars["Views"].Tools.AddTool("FlexStyle");
				//				this.utmMain.Toolbars["Views"].Tools.AddTool(Include.tbbAllocationWorkspace);
				//				this.utmMain.Toolbars["Views"].Tools.AddTool("Workspace2");


			}
			
			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}

		private void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			System.Windows.Forms.Form frm, navToForm;
			object[] args = null;
			try
			{
				string x = e.Tool.Key;
				switch ( e.Tool.Key )
				{
						// Explorer buttons pressed
					case Include.tbbMerchandise	:	
					case Include.tbbWorkflowMethods	:		
					case Include.tbbAllocationWorkspace	:	
					case Include.tbbStores :	
					case Include.tbbTaskLists :	
					case Include.tbbStoreFilters :
                    case Include.tbbHeaderFilters: //TT#1313-MD -jsobek -Header Filters
                    case Include.tbbAssortmentFilters: //TT#1313-MD -jsobek -Header Filters
					case Include.tbbAssortmentExplorer:	
                    case Include.tbbAssortmentWorkspace :
                        ShowExplorer(e.Tool.Key); 
						break;
                    //BEGIN TT#46-MD -jsobek -Develop My Activity Log
                    case Include.btUserActivityExplorer:
                    case Include.tbbUserActivityExplorer:	  // TT#46 MD - JSmith - User Dashboard
                        ShowExplorer(Include.tbbUserActivityExplorer); 
						break;
                    //END TT#46-MD -jsobek -Develop My Activity Log

						//	case "FlexStyle"		:	
						//		Cursor.Current = Cursors.WaitCursor;
						//		frm = new Form1();
						//					frm.MdiParent = this.GetMainMenu().GetForm();
						//		frm.WindowState = FormWindowState.Maximized;
						//		frm.Show();
						//		Cursor.Current = Cursors.Default;
						//		break;

						//					case "Chain Plan"		:		
						//						Cursor.Current = Cursors.WaitCursor;
						//						frm = new ChainForecast();
						//						frm.MdiParent = this;
						//						frm.WindowState = FormWindowState.Maximized;
						//						frm.Show();
						//						Cursor.Current = Cursors.Default;
						//						break;

					case Include.tbbOTSPlan		:	
					case Include.btPlanView		:	
						//						//frm = new ChainWStore();
						//						MIDRetail.Windows.OTSPlanSelection frmSelection = new MIDRetail.Windows.OTSPlanSelection(_SAB);
						//						//frmSelection.MdiParent = this;
						//						//frmSelection.WindowState = FormWindowState.Maximized;
						//						//frmSelection.Show();
						//						//if (frmSelection.EverythingOK == true)
						//						if (frmSelection.ShowDialog() == DialogResult.OK)
						//						{
						//							frmPlanView = new PlanView(_SAB, frmSelection.SelectionData, frmSelection.dsBasis);
						//							frmPlanView.MdiParent = frmSelection.MdiParent;
						//							
						//							frmSelection.Dispose();
						//							Cursor.Current = Cursors.WaitCursor;
						//							frmPlanView.WindowState = FormWindowState.Maximized;
						//							frmPlanView.Show();
						//							Cursor.Current = Cursors.Default;
						//						}		
						frm = new MIDRetail.Windows.OTSPlanSelection(_SAB);
						frm.MdiParent = this;
						Cursor.Current = Cursors.WaitCursor;
						// frm.WindowState = FormWindowState.Maximized;
						frm.Show();
						Cursor.Current = Cursors.Default;
						
						break;
					/* BEGIN MID Track #4386 - Justin Bolles - Delete Views */
					case Include.btViewAdmin:
						frm = new MIDRetail.Windows.OTSViewMaintenance(_SAB);
						frm.MdiParent = this;
						Cursor.Current = Cursors.WaitCursor;
						frm.Show();
						Cursor.Current = Cursors.Default;
						break;
					/* END MID Track #4386 - Justin Bolles - Delete Views */
					case Include.tbbSize:	
					case Include.btSize:	
					case "Select":
					case Include.btSelect:
					case Include.tbbStyle:
					case Include.btStyle:
					case Include.tbbSummary:		
					case Include.btSummary:
					//Begin Assortment Planning - JScott - Assortment Planning Changes
					//case Include.tbbAssortment:
					//case Include.btAssortment:
					//End Assortment Planning - JScott - Assortment Planning Changes
						// Begin TT#1154-MD - stodd - null reference when opening selection - 
						//frm = this.ActiveMdiChild;
                        frm = AssortmentActiveProcessToolbarHelper.ActiveProcess.form;
                        // begin TT#1174 - MD - Jellis - Cannot toggle among Style, Size and Summary for non-grouped headers
                        // Begin TT#1190-MD - stodd - active process issue
                        //===================================================================================================================
                        // If frm == null, this means that the active process is something other than an assortment or group allocation.
                        // At this time, this means to use the allocation workspace to get the header selection list.
                        // If the ActiveMdiChild is not null, and it's not an assortment or group allocation, we want to use it just as it was used 
                        // prior to Assortment or group allocation.
                        //===================================================================================================================
                        if (frm == null)
                        {
                            if (ActiveMdiChild != null && ActiveMdiChild.Name != "AssortmentView" && ActiveMdiChild.Name != "Group Allocation View")
                            {
                                frm = ActiveMdiChild;
                            }
                        }
                        // End TT#1190-MD - stodd - active process issue
                        // end TT#1174 - MD - Jellis - Cannot toggle among Style, Size and Summary for non-grouped headers
						// End TT#1154-MD - stodd - null reference when opening selection - 

						if (frm != null)
						{
							if (frm.Name == "StyleView")
							{	
								if ( e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle )  
									return;
								else if ( e.Tool.Key == Include.tbbSummary || e.Tool.Key == Include.btSummary
									//Begin Assortment Planning - JScott - Assortment Planning Changes
									|| e.Tool.Key == Include.tbbAssortment || e.Tool.Key == Include.btAssortment
									//End Assortment Planning - JScott - Assortment Planning Changes
									|| e.Tool.Key == Include.tbbGroupAllocation || e.Tool.Key == Include.btGroupAllocation		// TT#488-MD - STodd - Group Allocation - 	// TT#950 - MD - stodd - missing "Group Allocation" button
									|| e.Tool.Key == Include.tbbSize || e.Tool.Key == Include.btSize) 
								{
									NavigateToWindow(frm, e.Tool.Key);
									return;
								}
							}
							else if (frm.Name == "SummaryView")
							{
								if ( e.Tool.Key == Include.tbbSummary || e.Tool.Key == Include.btSummary )  
									return;
								else if ( e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle 
									|| e.Tool.Key == Include.tbbSize    || e.Tool.Key == Include.btSize  ) 
								{
									NavigateToWindow(frm, e.Tool.Key);
									return;
								}
							}
							//Begin Assortment Planning - JScott - Assortment Planning Changes
							else if (frm.Name == "AssortmentView" // TT#488 - MD - Jellis - Group Allocation 
                                     || frm.Name == "Group Allocation View") // TT#488 - MD - Jellis - Group Allocation
							{
								if (e.Tool.Key == Include.tbbAssortment || e.Tool.Key == Include.btAssortment
                                    || e.Tool.Key == Include.tbbGroupAllocation || e.Tool.Key == Include.btGroupAllocation) 	// TT#950 - MD - stodd - missing "Group Allocation" button
									return;
								else if (e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle
									|| e.Tool.Key == Include.tbbSize || e.Tool.Key == Include.btSize
									|| e.Tool.Key == Include.tbbSummary || e.Tool.Key == Include.btSummary)
								{
									NavigateToWindow(frm, e.Tool.Key);
									return;
								}
							}
							//End Assortment Planning - JScott - Assortment Planning Changes
                            else if (frm.Name == "SizeView")
                            {
                                if (e.Tool.Key == Include.tbbSize || e.Tool.Key == Include.btSize)
                                    return;
                                else if (e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle
                                    //Begin Assortment Planning - JScott - Assortment Planning Changes
                                    //|| e.Tool.Key == Include.tbbSummary || e.Tool.Key == Include.btSummary	)
                                    || e.Tool.Key == Include.tbbSummary || e.Tool.Key == Include.btSummary
                                    || e.Tool.Key == Include.tbbAssortment || e.Tool.Key == Include.btAssortment
                                    || e.Tool.Key == Include.tbbGroupAllocation || e.Tool.Key == Include.btGroupAllocation)	// TT#488-MD - STodd - Group Allocation - // TT#950 - MD - stodd - missing "Group Allocation" button
                                //End Assortment Planning - JScott - Assortment Planning Changes
                                {
                                    NavigateToWindow(frm, e.Tool.Key);
                                    return;
                                }
                            }
							// Begin TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
                            else
                            {
                                if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)
                                {
                                    NavigateToWindow(AssortmentActiveProcessToolbarHelper.ActiveProcess.form, e.Tool.Key);
                                    return;

                                }
                            }
							// End TT#998 - MD - stodd - Enqueue error when opening style review with GA open, but Size Need Method open as the active window.
							
						}
						eAllocationSelectionViewType viewType = eAllocationSelectionViewType.None; 
						navToForm = null;
					switch ( e.Tool.Key )
					{
						case "Select":
						case Include.btSelect:
							viewType = eAllocationSelectionViewType.None;
							break;
							
						case Include.tbbSize:	
						case Include.btSize:
							viewType = eAllocationSelectionViewType.Size;
							for (int i = 0; i < this.MdiChildren.Length; i++)
							{
								if (this.MdiChildren[i].Name == "SizeView")
								{
									navToForm = (MIDRetail.Windows.SizeView)(this.MdiChildren[i]);
									break;
								}
							}
							break;
							
						case Include.tbbStyle:
						case Include.btStyle:
							viewType = eAllocationSelectionViewType.Style;
							for (int i = 0; i < this.MdiChildren.Length; i++)
							{
								if (this.MdiChildren[i].Name == "StyleView")
								{
									navToForm = (MIDRetail.Windows.StyleView)(this.MdiChildren[i]);
									break;
								}
							}
							break;

						case Include.tbbSummary:		
						case Include.btSummary:
							viewType = eAllocationSelectionViewType.Summary;
							for (int i = 0; i < this.MdiChildren.Length; i++)
							{
								if (this.MdiChildren[i].Name == "SummaryView")
								{
									navToForm = (MIDRetail.Windows.SummaryView)(this.MdiChildren[i]);
									break;
								}
							}
							break;
						//Begin Assortment Planning - JScott - Assortment Planning Changes

						//case Include.tbbAssortment:
						//case Include.btAssortment:
						//    viewType = eAllocationSelectionViewType.Assortment;
						//    for (int i = 0; i < this.MdiChildren.Length; i++)
						//    {
						//        if (this.MdiChildren[i].Name == "AssortmentView")
						//        {
						//            navToForm = (MID.MRS.Windows.AssortmentView)(this.MdiChildren[i]);
						//            break;
						//        }
						//    }
						//    break;
						//End Assortment Planning - JScott - Assortment Planning Changes
					}
						if (navToForm != null)
							navToForm.Activate();
						else
						{
							MIDRetail.Windows.AllocationViewSelection form;
						 
							if (AllocationWorkspaceExplorer1.GetSelectedHeaders() > 0)
								AllocationWorkspaceExplorer1.DetermineWindow(viewType);
							else
							{	
								ApplicationSessionTransaction appTransaction = _SAB.ApplicationServerSession.CreateTransaction(); 
								appTransaction.AllocationWorkspaceExplorer = AllocationWorkspaceExplorer1;
                                form = new MIDRetail.Windows.AllocationViewSelection(this._EAB, appTransaction);
								form.MdiParent = this;
								// BEGIN MID Track #4311 - enhance Allocation View Selection
								if (e.Tool.Key != "Select" && e.Tool.Key != Include.btSelect)
								{
									form.WindowState = FormWindowState.Maximized;
								}
								// END MID Track #4311
								form.DetermineWindow(viewType);
							}
						}
						break;

					//Begin TT#2 - stodd - Assortment Planning Changes
					case Include.tbbAssortment:
					case Include.btAssortment:
						frm = this.ActiveMdiChild;
						if (frm != null)
						{
							if (frm.Name == "AssortmentView")
							{
								if (e.Tool.Key == Include.tbbAssortment || e.Tool.Key == Include.btAssortment)
									return;
								else if (e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle
									|| e.Tool.Key == Include.tbbSize || e.Tool.Key == Include.btSize)
								{
									NavigateToWindow(frm, e.Tool.Key);
									return;
								}
							}

						}
						viewType = eAllocationSelectionViewType.Assortment;
						navToForm = null;
						switch (e.Tool.Key)
						{
							case "Select":
							case Include.tbbAssortment:
							case Include.btAssortment:
								viewType = eAllocationSelectionViewType.Assortment;
								for (int i = 0; i < this.MdiChildren.Length; i++)
								{
									if (this.MdiChildren[i].Name == "AssortmentView")
									{
										navToForm = (MIDRetail.Windows.AssortmentView)(this.MdiChildren[i]);
										break;
									}
								}
								break;
						}
                        if (navToForm != null)
                        {
                            navToForm.Activate();
                        }
                        // Begin TT#2 - RMatelic - Assortment Planning >> add if...
                        //else    
                        else if (frm == null)      
                        // End TT#2   
                        {
                            // BEGIN TT#2016-MD - AGallagher - Assortment Review Navigation
                            if (assortmentWorkspaceExplorer.GetSelectedHeaders() == 1)
                            {
                                int hdrRID = assortmentWorkspaceExplorer.GetSelectedAssortmentKey();
                                ApplicationSessionTransaction appTransaction = _SAB.ApplicationServerSession.CreateTransaction();
                                AssortmentProfile ap = new AssortmentProfile(appTransaction, null, hdrRID, _SAB.ClientServerSession);
                                frmAssortmentProperties assortmentProperties = new frmAssortmentProperties(this._SAB, _EAB, null, ap, false);
                                assortmentProperties.MdiParent = this;
                                assortmentProperties.Show();
                            }
                            else
                            {
                            // END TT#2016-MD - AGallagher - Assortment Review Navigation
                                MIDRetail.Windows.AssortmentViewSelection form;

                                if (assortmentWorkspaceExplorer.GetSelectedHeaders() > 0)
                                    assortmentWorkspaceExplorer.DetermineWindow(viewType);
                                else
                                {
                                    ApplicationSessionTransaction appTransaction = _SAB.ApplicationServerSession.CreateTransaction();
                                    appTransaction.AllocationWorkspaceExplorer = AllocationWorkspaceExplorer1;
                                    form = new MIDRetail.Windows.AssortmentViewSelection(this._EAB, _SAB, appTransaction, null, false);
                                    form.MdiParent = this;
                                    // Ron Matelic - comment following lines
                                    //if (e.Tool.Key != "Select" && e.Tool.Key != Include.btSelect)
                                    //{
                                    //    form.WindowState = FormWindowState.Maximized;
                                    //}
                                    form.DetermineWindow(viewType);
                                }
                            }  // TT#2016-MD - AGallagher - Assortment Review Navigation
                        }
						break;
					//End TT#2 - stodd - Assortment Planning Changes

					// BEGIN TT#488-MD - STodd - Group Allocation - 
					case Include.tbbGroupAllocation:
					case Include.btGroupAllocation:
						frm = this.ActiveMdiChild;
						if (frm != null)
						{
							if (frm.Name == "Group Allocation View")
							{
								if (e.Tool.Key == Include.tbbGroupAllocation || e.Tool.Key == Include.btGroupAllocation)
									return;
								else if (e.Tool.Key == Include.tbbStyle || e.Tool.Key == Include.btStyle
									|| e.Tool.Key == Include.tbbSize || e.Tool.Key == Include.btSize)
								{
									NavigateToWindow(frm, e.Tool.Key);
									return;
								}
							}

						}
						viewType = eAllocationSelectionViewType.GroupAllocation;
						navToForm = null;
						switch (e.Tool.Key)
						{
							case "Select":
							case Include.tbbGroupAllocation:
							case Include.btGroupAllocation:
								viewType = eAllocationSelectionViewType.GroupAllocation;
								for (int i = 0; i < this.MdiChildren.Length; i++)
								{
									if (this.MdiChildren[i].Name == "Group Allocation View")
									{
										navToForm = (MIDRetail.Windows.AssortmentView)(this.MdiChildren[i]);
										break;
									}
								}
								break;
						}
						if (navToForm != null)
						{
							navToForm.Activate();
						}
						// Begin TT#2 - RMatelic - Assortment Planning >> add if...
						//else    
						else if (frm == null)
						// End TT#2   
						{
                            MIDRetail.Windows.AssortmentView form;

							if (assortmentWorkspaceExplorer.GetSelectedHeaders() > 0)
							{
								assortmentWorkspaceExplorer.DetermineWindow(viewType);
							}
							//else
							//{
							//    ApplicationSessionTransaction appTransaction = _SAB.ApplicationServerSession.CreateTransaction();
							//    appTransaction.AllocationWorkspaceExplorer = AllocationWorkspaceExplorer1;
							//    form = new MIDRetail.Windows.GroupAllocationView(this._EAB, appTransaction);
							//    form.MdiParent = this;
							//    // Ron Matelic - comment following lines
							//    //if (e.Tool.Key != "Select" && e.Tool.Key != Include.btSelect)
							//    //{
							//    //    form.WindowState = FormWindowState.Maximized;
							//    //}
							//    form.DetermineWindow(viewType);
							//}
						}
						break;
						// END TT#488-MD - STodd - Group Allocation - 
						
					case "Calendar"	:	
					case Include.btCalendar	:
						Cursor.Current = Cursors.WaitCursor;
//						frm = new CalendarModelList(_SAB);
//						//					frm.MdiParent = this.GetMainMenu().GetForm();
//						frm.MdiParent = this;
//						frm.Show();
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(CalendarModelList), args, false);
							frm.MdiParent = this;
//							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							frm.Dock = DockStyle.Fill;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case "Header Characteristics"	:	
					case Include.btHeaderChar	:
						Cursor.Current = Cursors.WaitCursor;
//						frm = new HeaderCharacteristicMaint(_SAB);
//						if(AllocationWorkspaceExplorer1 != null)
//							AllocationWorkspaceExplorer1.startListening((HeaderCharacteristicMaint)frm);
//						//					frm.MdiParent = this.GetMainMenu().GetForm();
//						frm.MdiParent = this;
//						frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						frm.Show();
//						frm.Dock = DockStyle.Fill;
//						Cursor.Current = Cursors.Default;

						try
						{
                            //args  =  new object[]{_SAB};
                            //frm = GetForm(typeof(HeaderCharacteristicMaint), args, false);
                            //if(AllocationWorkspaceExplorer1 != null
                            //    && AllocationWorkspaceExplorer1.FormLoaded)
                            //    AllocationWorkspaceExplorer1.startListening((HeaderCharacteristicMaint)frm);
                            //frm.MdiParent = this;
                            //// BEGIN MID Track #3978 - error deleting characteristic; this is just a cosmetic change 
                            ////frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            ////frm.Dock = DockStyle.Fill;
                            //// END MID Track #3978  
                            //frm.Show();
                            //frm.BringToFront();

                            args = new object[] { };
                            System.Windows.Forms.Form frmCharMaint = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(HeaderCharacteristics), args, false);
                            // Begin TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
                            if (AllocationWorkspaceExplorer1 != null
                                && AllocationWorkspaceExplorer1.FormLoaded)
                                AllocationWorkspaceExplorer1.startListening((HeaderCharacteristics)frmCharMaint);
                            // End TT#1914-MD - JSmith - Header Characteristics upon creation in 8.6 SVC the user must close and open the app for it to appear in the column chooser and alloc workspace.
                            frmCharMaint.MdiParent = this;
                            frmCharMaint.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            //frmCharMaint.Dock = DockStyle.Fill;
                            ((HeaderCharacteristics)frmCharMaint).LoadCharacteristics(_SAB, _EAB);
                            frmCharMaint.Show();
                            frmCharMaint.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case "Product Characteristics":
					case Include.btProductChar:

						try
						{
							args = new object[] { _SAB, true };
							frm = GetForm(typeof(ProductCharacteristicMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case "Store Profiles"	:
					case Include.btProfiles	: 
						Cursor.Current = Cursors.WaitCursor;

//						// Is there a store profile form already opened (by a diff pgm)?
//						if (_StoreCharacteristicsFrm == null)
//						{
//							foreach (Form aForm in this.MdiChildren)  // see if store profile already open
//							{
//								if ( aForm.GetType() == typeof(StoreProfileMaint) )
//									_StoreProfileFrm = (StoreProfileMaint)aForm;
//							}
//						}
//
//						if (_StoreProfileFrm == null || _StoreProfileFrm.IsDisposed)
//						{
//							_StoreProfileFrm = new StoreProfileMaint(_SAB, _EAB);
//							_StoreProfileFrm.MdiParent = this;
//							_StoreProfileFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							_StoreProfileFrm.Dock = DockStyle.Fill;
//						}
//						_StoreProfileFrm.Show();
//						_StoreProfileFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
                            args = new object[] { _SAB, _EAB };
                            System.Windows.Forms.Form frmStoreMaint = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(StoreProfileMaintForm), args, false);
                            frmStoreMaint.MdiParent = this;
                            frmStoreMaint.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frmStoreMaint.Dock = DockStyle.Fill;
                            ((StoreProfileMaintForm)frmStoreMaint).LoadStores();
                            frmStoreMaint.Show();
                            frmStoreMaint.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case "Store Characteristics"	:
					case Include.btCharacteristics	:
					case Include.btStoreChar:
						Cursor.Current = Cursors.WaitCursor;

//						// Is there a store profile form already opened (by a diff pgm)?
//						if (_StoreCharacteristicsFrm == null)
//						{
//							foreach (Form aForm in this.MdiChildren)  // see if store profile already open
//							{
//								if ( aForm.GetType() == typeof(StoreCharacteristicMaint) )
//									_StoreCharacteristicsFrm = (StoreCharacteristicMaint)aForm;
//							}
//						}
//
//						if (_StoreCharacteristicsFrm == null || _StoreCharacteristicsFrm.IsDisposed)
//						{
//							_StoreCharacteristicsFrm = new StoreCharacteristicMaint(_SAB, _EAB);
//							_StoreCharacteristicsFrm.MdiParent = this;
//							_StoreCharacteristicsFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							_StoreCharacteristicsFrm.Dock = DockStyle.Fill;
//						}
//						_StoreCharacteristicsFrm.Show();
//						_StoreCharacteristicsFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

                        //try
                        //{
                        //    //args  =  new object[]{_SAB, _EAB};
                        //    //frm = GetForm(typeof(StoreCharacteristicMaint), args, false);
                        //    //frm.MdiParent = this;
                        //    //frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                        //    //frm.Show();
                        //    //frm.BringToFront();

                        //    //TO DO - Make new store characterisics maintenance screen
                        //}
                        //catch (Exception)
                        //{
                        //    throw;
                        //}
                        //finally
                        //{
                        //    Cursor.Current = Cursors.Default;
                        //}
                        Cursor.Current = Cursors.WaitCursor;

//						// Is there a store profile form already opened (by a diff pgm)?
//						if (_StoreCharacteristicsFrm == null)
//						{
//							foreach (Form aForm in this.MdiChildren)  // see if store profile already open
//							{
//								if ( aForm.GetType() == typeof(StoreProfileMaint) )
//									_StoreProfileFrm = (StoreProfileMaint)aForm;
//							}
//						}
//
//						if (_StoreProfileFrm == null || _StoreProfileFrm.IsDisposed)
//						{
//							_StoreProfileFrm = new StoreProfileMaint(_SAB, _EAB);
//							_StoreProfileFrm.MdiParent = this;
//							_StoreProfileFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							_StoreProfileFrm.Dock = DockStyle.Fill;
//						}
//						_StoreProfileFrm.Show();
//						_StoreProfileFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
                            args = new object[] {  };
                            System.Windows.Forms.Form frmCharMaint = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(StoreCharacteristics), args, false);
                            frmCharMaint.MdiParent = this;
                            frmCharMaint.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            //frmCharMaint.Dock = DockStyle.Fill;
                            ((StoreCharacteristics)frmCharMaint).LoadCharacteristics(_SAB, _EAB);
                            frmCharMaint.Show();
                            frmCharMaint.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case Include.btSecurity	:		
						Cursor.Current = Cursors.WaitCursor;
//						if (_SecurityFrm == null || _SecurityFrm.IsDisposed)
//						{
//							_SecurityFrm = new frmSecurity( _SAB );
//							_SecurityFrm.MdiParent = this;
//							_SecurityFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_SecurityFrm.Show();
//						_SecurityFrm.BringToFront();
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmSecurity), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case Include.btOptions	:		
						Cursor.Current = Cursors.WaitCursor;
//						if (_OptionsFrm == null || _OptionsFrm.IsDisposed)
//						{
//							_OptionsFrm = new frmGlobalOptions( _SAB );
//							_OptionsFrm.MdiParent = this;
//							_OptionsFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_OptionsFrm.Show();
//						_OptionsFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmGlobalOptions), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case Include.btSizeGroups	:		
						Cursor.Current = Cursors.WaitCursor;
//						if (_SizeGroupFrm == null || _SizeGroupFrm.IsDisposed)
//						{
//							_SizeGroupFrm = new frmSizeGroup( _SAB );
//							_SizeGroupFrm.MdiParent = this;
//							_SizeGroupFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_SizeGroupFrm.Show();
//						_SizeGroupFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmSizeGroup), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;

					case Include.btSizeCurves	:		
						Cursor.Current = Cursors.WaitCursor;
//						if (_SizeCurveFrm == null || _SizeCurveFrm.IsDisposed)
//						{
//							_SizeCurveFrm = new frmSizeCurve( _SAB );
//							_SizeCurveFrm.MdiParent = this;
//							_SizeCurveFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_SizeCurveFrm.Show();
//						_SizeCurveFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmSizeCurve), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					
					case Include.btEligModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmEligModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btStkModModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmStockModifierModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btSlsModModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmSalesModifierModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
                    case Include.btOverrideLowLevel:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
							// Begin Track #5909 - stodd
							FunctionSecurityProfile functionSecurityOverrideLowLevelModels = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsOverrideLowLevels);
							args = new object[] { _SAB, Include.NoRID, Include.NoRID, Include.NoRID, false, functionSecurityOverrideLowLevelModels };
							// End Track #5909 - stodd
                            frm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
					// BEGIN MID Track #4370 - John Smith - FWOS Models
					case Include.btFWOSModModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmFWOSModifierModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					// END MID Track #4370

                    // BEGIN TT#108 - MD - DOConnell - FWOS Max Model
                    case Include.btFWOSMaxModels:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB };
                            frm = GetForm(typeof(frmFWOSMaxModelMaint), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    // END TT#108 - MD - DOConnell - FWOS Max Model
					case Include.btForecastingModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmForecastingModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btForecastBalModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmForecastBalModelMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btSizeConstraintsModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(SizeConstraintsMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
						// begin MID Track 3619 Remove Fringe
					//case "btSizeFringeModels"	:		
					//	Cursor.Current = Cursors.WaitCursor;
					//	try
					//	{
					//		args  =  new object[]{_SAB};
					//		frm = GetForm(typeof(SizeFringeMaint), args, false);
					//		frm.MdiParent = this;
					//		frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					//		frm.Show();
					//		frm.BringToFront();
					//	}
					//	catch (Exception)
					//	{
					//		throw;
					//	}
					//	finally
					//	{
					//		Cursor.Current = Cursors.Default;
					//	}
					//	break;
						// end MID Track Remove Fringe
					case Include.btSizeAlternatesModels	:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(SizeAlternatesMaint), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btCut		:
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).ICut();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btCopy		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).ICopy();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btPaste		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).IPaste();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btFind		:		
						if (this.ActiveControl != null)
						{
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).IFind();
							}
						}
						break;
					case Include.btDelete		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).IDelete();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btClose		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).IClose();
							}
                            // Begin TT#4360 - RMatelic - File>Close does not close the Audit window
                            else if (this.ActiveControl.GetType().BaseType == typeof(Form))
                            {
                                Form form = (Form)this.ActiveControl;
                                form.Close();
                            }
                            // End TT#4360
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btSave		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).ISave();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btSaveAs		:		
						if (this.ActiveControl != null)
						{
							Cursor.Current = Cursors.WaitCursor;
							if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
							{
								((IFormBase)this.ActiveControl).ISaveAs();
							}
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btSort:
						for (int i = 0; i < this.MdiChildren.Length; i++)
						{
							if (this.MdiChildren[i].Name == "PlanView")
							{
								MIDRetail.Windows.PlanView frmPlanView = (MIDRetail.Windows.PlanView)(this.MdiChildren[i]);
								frmPlanView.DoSorts();
							}
							else if (this.MdiChildren[i].Name == "StyleView")
							{	
								MIDRetail.Windows.StyleView frmStyleView = (MIDRetail.Windows.StyleView)(this.MdiChildren[i]);
								// MIDTrack 948
								if (this.ActiveMdiChild == frmStyleView)
									frmStyleView.DoSorts();
							}
							else if (this.MdiChildren[i].Name == "SizeView")
							{
								MIDRetail.Windows.SizeView frmSizeView = (MIDRetail.Windows.SizeView)(this.MdiChildren[i]);
								if (this.ActiveMdiChild == frmSizeView)
									frmSizeView.DoSorts();
							}
							else if (this.MdiChildren[i].Name == "SummaryView")
							{
								MIDRetail.Windows.SummaryView frmSummaryView = (MIDRetail.Windows.SummaryView)(this.MdiChildren[i]);
								if (this.ActiveMdiChild == frmSummaryView)
									frmSummaryView.DoSorts();
							}
							//Begin Assortment Planning - JScott - Assortment Planning Changes
							else if (this.MdiChildren[i].Name == "AssortmentView")
							{
								MIDRetail.Windows.AssortmentView frmAssortmentView = (MIDRetail.Windows.AssortmentView)(this.MdiChildren[i]);
								if (this.ActiveMdiChild == frmAssortmentView)
									frmAssortmentView.DoSorts();
							}
							//End Assortment Planning - JScott - Assortment Planning Changes
                            // Begin TT#2975 - RMAtelic - OTS - Totals on Right Option not sorting
                            else if (this.MdiChildren[i].Name == "PlanViewRT")
							{
								MIDRetail.Windows.PlanViewRT frmPlanViewRT = (MIDRetail.Windows.PlanViewRT)(this.MdiChildren[i]);
								frmPlanViewRT.DoSorts();
							}
                            // End TT#2975
						}
						break;
//					case Include.btFilter:
//						Cursor.Current = Cursors.WaitCursor;
////						try
////						{
////							if (_FilterFrm == null || _FilterFrm.IsDisposed)
////							{
////								_FilterFrm = new frmFilter(_SAB);
////								_FilterFrm.MdiParent = this;
////								_FilterFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
////								_FilterFrm.InitializeForm();
////							}
////							_FilterFrm.Show();
////							_FilterFrm.BringToFront();
////						}
////						catch (Exception)
////						{
////							_FilterFrm = null;
////						}
////						finally
////						{
////							Cursor.Current = Cursors.Default;
////						}
//
//						try
//						{
//							args  =  new object[]{_SAB};
//							frm = GetForm(typeof(frmFilter), args, true);
//							frm.MdiParent = this;
//							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							((frmFilter)frm).InitializeForm();
//							frm.Show();
//							frm.BringToFront();
//						}
//						catch (Exception)
//						{
//							throw;
//						}
//						finally
//						{
//							Cursor.Current = Cursors.Default;
//						}
//						break;
                    // Begin TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
//                    case Include.btFilterWizard:
//                        Cursor.Current = Cursors.WaitCursor;
////						try
////						{
////							if (_FilterWizardFrm == null || _FilterWizardFrm.IsDisposed)
////							{
////								_FilterWizardFrm = new frmFilterWizard(_SAB);
////								_FilterWizardFrm.MdiParent = this;
////								_FilterWizardFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
////							}
////							_FilterWizardFrm.Show();
////							_FilterWizardFrm.BringToFront();
////						}
////						catch (Exception)
////						{
////							_FilterWizardFrm = null;
////						}
////						finally
////						{
////							Cursor.Current = Cursors.Default;
////						}

//                        try
//                        {
//                            //BEGIN TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard
//                            //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
//                            //args = new object[] { _SAB, _EAB, false };
                            





//                            //END TT#406-MD -jsobek -The User's Filter Explorer is not refreshed when a new Filter is added through the Filter Wizard
//                           //frm = GetForm(typeof(frmFilterWizard), args, true);
//                            //frm = GetForm(typeof(frmFilterBuilder), args, true);


//                            int defaultOwnerUserRID = _SAB.ClientServerSession.UserRID;  //From the menu - just default to a user filter
//                            frm = SharedRoutines.GetFilterFormForNewFilters(filterTypes.StoreFilter, _SAB, _EAB, defaultOwnerUserRID);
                            
                    
//                            frm.MdiParent = this;
//                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//                            //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
//                            frm.Show();
//                            frm.BringToFront();
//                        }
//                        catch (Exception)
//                        {
//                            throw;
//                        }
//                        finally
//                        {
//                            Cursor.Current = Cursors.Default;
//                        }
//                        break;
                    // End TT#1352-MD - stodd - Store Filters - Remove Tools-Filter Wizard from the main menu.
					case Include.btRelease:
						Cursor.Current = Cursors.WaitCursor;
//						if (_ReleaseFrm == null || _ReleaseFrm.IsDisposed)
//						{
//							_ReleaseFrm = new frmReleaseResources(_SAB);
//							_ReleaseFrm.MdiParent = this;
//							_ReleaseFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_ReleaseFrm.Show();
//						_ReleaseFrm.BringToFront();

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmReleaseResources), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btAudit:
						Cursor.Current = Cursors.WaitCursor;
//						if (_AuditFrm == null || _AuditFrm.IsDisposed)
//						{
//							_AuditFrm = new frmAuditViewer(_SAB);
//							_AuditFrm.MdiParent = this;
//							_AuditFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_AuditFrm.Show();
//						_AuditFrm.BringToFront();

						try
						{
                            //Begin TT#1443-MD -jsobek -Audit Filter
                            //args  =  new object[]{_SAB};
                            ////frm = GetForm(typeof(frmAuditViewer), args, false); //TT#435-MD-DOConnell-Add new features to Audit
                            //frm = GetForm(typeof(frm_AuditViewer), args, false); //TT#435-MD-DOConnell-Add new features to Audit
                            //frm.MdiParent = this;
                            //frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            //frm.Show();
                            //frm.BringToFront();


                            args = new object[] { _SAB, _EAB, filterTypes.AuditFilter };
                            System.Windows.Forms.Form frmSearchResults = SharedRoutines.GetForm(_EAB.Explorer.MdiChildren, typeof(SearchResultsForm), args, false);
                            frmSearchResults.MdiParent = this;
                            frmSearchResults.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frmSearchResults.Show();
                            frmSearchResults.BringToFront();
                            //End TT#1443-MD -jsobek -Audit Filter
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					// Begin - Reclass
					case Include.btAuditReclass:
						Cursor.Current = Cursors.WaitCursor;
						try
						{
//							args  =  new object[]{_SAB};
//							frm = GetForm(typeof(frmAuditReclassViewer), args, false);
//							frm.MdiParent = this;
//							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//							frm.Show();
//							frm.BringToFront();
							AuditData reclassAuditData = new AuditData();

                            System.Data.DataSet ds = MIDEnvironment.CreateDataSet("AuditReclassDataSet");
							reclassAuditData.ReclassAudit_Report(ds);

                            Windows.CrystalReports.AuditReclassReport2 auditReclassReport = new Windows.CrystalReports.AuditReclassReport2();
							auditReclassReport.SetDataSource(ds);

							frmReportViewer viewer = new frmReportViewer(_SAB);
							viewer.Text = "Audit Reclass";
							viewer.MdiParent = this;
							viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							viewer.ReportSource = auditReclassReport;
							viewer.Show();
							viewer.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					// End 
                    //Begin Track #6232 - KJohnson - Incorporate Audit Reports in Version 3.0 Base
                    // Begin - NodePropertiesOverrides
                    case Include.btNodePropertiesOverrides:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, };
                            frm = GetForm(typeof(frmReportNodePropertiesOverridesSetup), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    // End 
                    // Begin - ForecastAuditMerchandise
                    case Include.btForecastAuditMerchandise:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, "ForecastAuditMerchandise" };
                            frm = GetForm(typeof(frmReportForcastAuditSetup), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    // End 
                    // Begin - ForecastAuditMethod
                    case Include.btForecastAuditMethod:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, "ForecastAuditMethod" };
                            frm = GetForm(typeof(frmReportForcastAuditSetup), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    // End 
                    // Begin - AllocationAudit
                    case Include.btAllocationAudit:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB };
                            frm = GetForm(typeof(frmReportAllocationAuditSetup), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    // End 
                    //End Track #6232 - KJohnson
                    case Include.btCustomReports:
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							string reportPath = MIDConfigurationManager.AppSettings["ReportPath"];
							if (reportPath != null &&
								(ofdOpenCustom.InitialDirectory == null ||
								ofdOpenCustom.InitialDirectory.Trim().Length == 0))
							{
								ofdOpenCustom.InitialDirectory = Convert.ToString(reportPath, CultureInfo.CurrentCulture);
							}
							if(ofdOpenCustom.ShowDialog() != DialogResult.Cancel)
							{
								Reports openReport = new Reports(ofdOpenCustom.FileName);
								string[] fileParts = openReport.file.Split('\\');
								string fileName = fileParts[fileParts.Length - 1];
								
								switch(fileName)
								{
									default:
										frmReportViewer viewer = new frmReportViewer(_SAB);
										viewer.Text = "Custom Report";
										viewer.MdiParent = this;
										viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
										
										int res = openReport.GetParameterCount();
										if (res < 1)
											viewer.ReportSource = openReport.LoadParameters();
										else
										{
											bool bUpdate = false;
											ReportParam paraDialog = new ReportParam(); 
											paraDialog.m_rptFile = openReport.file;
											paraDialog.m_rptFileName = openReport.file;
											paraDialog.Anchor = AnchorStyles.Left | AnchorStyles.Top;
											paraDialog.ShowDialog();
											paraDialog.BringToFront();
											if(paraDialog.DialogResult == DialogResult.Cancel) 
												return;
											openReport.xmlFile = paraDialog.m_xmlFile;
											if(paraDialog.DialogResult == DialogResult.OK) 
												bUpdate = true;
											paraDialog.Close(); 
											if (bUpdate) 
												viewer.ReportSource = openReport.LoadParameters();
										}

										viewer.Show();
										viewer.BringToFront();
										break;
								}
							}
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btTextEditor:
						Cursor.Current = Cursors.WaitCursor;
//						if (_TextEditorFrm == null || _TextEditorFrm.IsDisposed)
//						{
//							_TextEditorFrm = new frmTextEditor(_SAB);
//							_TextEditorFrm.MdiParent = this;
//							_TextEditorFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						if (((frmTextEditor)_TextEditorFrm).DisplayForm)
//						{
//							_TextEditorFrm.Show();
//							_TextEditorFrm.BringToFront();
//						}
//						else
//						{
//							_TextEditorFrm = null;
//						}

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmTextEditor), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    case Include.btEmailMessage:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = null; // new object[] { _SAB };
                            frm = GetForm(typeof(EmailMessageForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    //Begin TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                    case Include.btAllocationAnalysis:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, _EAB };
                            frm = GetForm(typeof(AllocationAnalysisForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    case Include.btForecastAnalysis:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB };
                            frm = GetForm(typeof(ForecastAnalysisForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    //End TT#739-MD -jsobek -Delete Stores -Allocation & Forecast Analysis
                    //Begin TT#554-MD -jsobek -User Log Level Report
                    case Include.btReportUserOptionsReview:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, };
                            frm = GetForm(typeof(ReportUserOptionsReviewForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    //End TT#554-MD -jsobek -User Log Level Report
                    //Begin TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
                    case Include.btReportAllocationByStore:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, };
                            frm = GetForm(typeof(ReportAllocationByStoreForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
                    //End TT#739-MD -jsobek -Delete Stores -Allocation by Store Report
					case Include.btScheduleBrowser:
						Cursor.Current = Cursors.WaitCursor;
//						if (_ScheduleBrowserFrm == null || _ScheduleBrowserFrm.IsDisposed)
//						{
//							_ScheduleBrowserFrm = new frmScheduleBrowser(_SAB);
//							_ScheduleBrowserFrm.MdiParent = this;
//							_ScheduleBrowserFrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
//						}
//						_ScheduleBrowserFrm.Show();
//						_ScheduleBrowserFrm.BringToFront();
//
//						Cursor.Current = Cursors.Default;

						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmScheduleBrowser), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					// Begin TT#1581-MD - stodd - API Header Reconcile
                    case Include.btProcessControl:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, _EAB };
                            frm = GetForm(typeof(ProcessControlForm), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
					// End TT#1581-MD - stodd - API Header Reconcile

					// Begin TT#1386-MD - stodd - added Schedulet Job Manager
                    case Include.btSchedulerJobManager:
                        Cursor.Current = Cursors.WaitCursor;
                        try
                        {
                            args = new object[] { _SAB, _EAB };
                            frm = GetForm(typeof(SchedulerJobManager), args, false);
                            frm.MdiParent = this;
                            frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            frm.Show();
                            frm.BringToFront();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                        break;
					// End TT#1386-MD - stodd - added Schedulet Job Manager

					case Include.btWorkspace:
						ShowExplorer(Include.tbbAllocationWorkspace);
						break;
					case Include.btPlanWorkspace:
						MessageBox.Show("No Plan explorer yet");
						break;
					case Include.btMerchandise:
						ShowExplorer(Include.tbbMerchandise);
						break;
					case Include.btStore:
						ShowExplorer(Include.tbbStores);
						break;
					case Include.btStoreFilter:
						ShowExplorer(Include.tbbStoreFilters);
						break;
                    case Include.btHeaderFilter: //TT#1313-MD -jsobek -Header Filters
                        ShowExplorer(Include.tbbHeaderFilters);
                        break;
                    case Include.btAssortmentFilter: //TT#1313-MD -jsobek -Header Filters
                        ShowExplorer(Include.tbbAssortmentFilters);
                        break;
					case Include.btTaskList:
						ShowExplorer(Include.tbbTaskLists);
						break;
					case Include.btWorkflow:
						ShowExplorer(Include.tbbWorkflowMethods);
						break;
                    case Include.btAssortmentWorkspace:                   // TT#2 Assortment Planning  
                        ShowExplorer(Include.tbbAssortmentWorkspace);
                        break;
					case Include.btAssortmentExplorer:                   // TT#2 Assortment Planning  
						ShowExplorer(Include.tbbAssortmentExplorer);
						break;
                    // Begin TT#46 MD - JSmith - User Dashboard
                    //case Include.btUserActivityExplorer:
                    //    ShowExplorer(Include.tbbUserActivityExplorer);
                    //    break;
                    // End TT#46 MD
					case Include.btRefresh		:
                        // Begin TT#4287 - JSmith - Do not allow refresh if windows are open
                        if (this.MdiChildren.Length > 0)
                        {
                            MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RefreshWarning, true), MIDText.GetTextOnly(eMIDTextCode.lbl_CompanyName), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        // End TT#4287 - JSmith - Do not allow refresh if windows are open

                        // Begin TT#1878-MD - JSmith - Tools Refresh during Store Load zeros attribute sets
                        //Do not allow refresh if Store Load API is running
                        GenericEnqueue genericEnqueueStoreLoad = new GenericEnqueue(eLockType.StoreLoadRunning, -1, _SAB.ClientServerSession.UserRID, _SAB.ClientServerSession.ThreadID);
                        if (genericEnqueueStoreLoad.DoesHaveConflicts())
                        {
                            MIDRetail.Windows.Controls.SharedControlRoutines.SetEnqueueConflictMessage(_SAB, genericEnqueueStoreLoad, "Store Load API");
                            return;
                        }
                        // End TT#1878-MD - JSmith - Tools Refresh during Store Load zeros attribute sets
                        
                        Cursor.Current = Cursors.WaitCursor;
						_SAB.ClientServerSession.Refresh();
						_SAB.ApplicationServerSession.Refresh();
						_SAB.HierarchyServerSession.Refresh();
						_SAB.StoreServerSession.Refresh();
						_SAB.HeaderServerSession.Refresh();

                        StoreMgmt.LoadInitialStoresAndGroups(_SAB, _SAB.ClientServerSession, false, true);  // TT#1876-MD - JSmith - Store Load shows zero stores in sets after Tools>Refresh
                        StoreMgmt.BuildUsersAssignedToMe();  // TT#5664 - JSmith - All User Store Attributes appear in User Methods 
                        filterDataHelper.Refresh();  // TT#1909-MD - JSmith - Str_Vesioning - Interfaced Store not available for selection in the STore List for a Static Store Attribute
                        MaxStoresHelper.Refresh();

                        //BEGIN TT#1097 - 4.0 Testing: Tools Refresh does not update all Explorer - apicchetti - 1/26/2011

                        // will refresh all explorer objects no matter if that list of objects grows or shrinks
                        foreach (DockableControlPane cp in udmMain.ControlPanes)
                        {
                            if (cp.Control.GetType().GetInterface("IFormBase") != null)
                            {
                                ExplorerBase GenericExplorer = (ExplorerBase)cp.Control;
                                GenericExplorer.IRefresh();
                            }
                        }

                        //meMerchandiseExplorer.IRefresh();
                        //storeGroupExplorer1.IRefresh();
                        //AllocationWorkspaceExplorer1.IRefresh();  // MID Track 1955
                        //taskListExplorer.IRefresh();
                        //tvwWorkflowMethodExplorer.IRefresh();

                        //END TT#1097 - 4.0 Testing: Tools Refresh does not update all Explorer - apicchetti - 1/26/2011

                        
						if (this.ActiveControl != null)
						{
							// do not refresh explorers again
							if (this.ActiveControl.GetType().Name != meMerchandiseExplorer.GetType().Name &&
								this.ActiveControl.GetType().Name != storeGroupExplorer1.GetType().Name &&
								this.ActiveControl.GetType().Name != AllocationWorkspaceExplorer1.GetType().Name &&
								this.ActiveControl.GetType().Name != taskListExplorer.GetType().Name &&
								this.ActiveControl.GetType().Name != assortmentExplorer.GetType().Name &&		 // TT#2 Assortment Planning
                                this.ActiveControl.GetType().Name != assortmentWorkspaceExplorer.GetType().Name) // TT#2 Assortment Planning   
							{
								if (this.ActiveControl.GetType().GetInterface("IFormBase") != null)
								{
									((IFormBase)this.ActiveControl).IRefresh();
								}
							}
						}
						Cursor.Current = Cursors.Default;
						break;
//					case "btShowLogin"		:		
//						Cursor.Current = Cursors.WaitCursor;
//						MIDUserInfo userInfo = new MIDUserInfo();
//						StateButtonTool btShowLogin = (StateButtonTool)this.utmMain.Tools["btShowLogin"];
//						userInfo.ShowLogin = btShowLogin.Checked;
//						userInfo.WriteUserInfo();
//						Cursor.Current = Cursors.Default;
//						break;
					case Include.btUserOptions		:		
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmUserOptions), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
                    // BEGIN Workspace Usability Enhancement
                    case Include.btRestoreLayout:
                        Cursor.Current = Cursors.WaitCursor;
                        if (this.ActiveControl != null)
                        {
                            if (this.ActiveControl.GetType().Name == AllocationWorkspaceExplorer1.GetType().Name)
                            {
                                AllocationWorkspaceExplorer1.IRestoreLayout();
                            }
                            else if (this.ActiveControl.GetType().Name == assortmentWorkspaceExplorer.GetType().Name)
                            {
                                assortmentWorkspaceExplorer.IRestoreLayout();
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        break;
                    // END Workspace Usability Enhancement
                    //Begin TT#1521-MD -jsobek -Active Directory Authentication
                    case Include.btLoginAsAdmin:
                        string restartArgs = "\"" + "autoadmin" + "\"" + " \"" + "nada" + "\""; //pass in two arguements since that is required to get the username passed thru
                        ProcessStartInfo AdminLoginProcess = new ProcessStartInfo("MIDRetail.exe", restartArgs);
                        AdminLoginProcess.WorkingDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                        Process.Start(AdminLoginProcess);
                        this.Close();
                        break;
                    //End TT#1521-MD -jsobek -Active Directory Authentication
					case Include.btExit:
                        // Begin TT#4630 - JSmith - Unhandled exception when exporting data. 
                        //this.Close();
                        if (CheckForSignOff())
                        {
                            this.Close();
                        }
                        // End TT#4630 - JSmith - Unhandled exception when exporting data.
						break;
					case Include.btMemory:
//						MessageBox.Show(_SAB.HierarchyServerSession.ShowMemory(), "Hierarchy Service Memory", MessageBoxButtons.OK, MessageBoxIcon.None);
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmMemoryViewer), args, false);
							frm.MdiParent = this;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.Show();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
					case Include.btAbout:
						Cursor.Current = Cursors.WaitCursor;
						try
						{
							args  =  new object[]{_SAB};
							frm = GetForm(typeof(frmAbout), args, false);
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.ShowDialog();
							frm.BringToFront();
						}
						catch (Exception)
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
						// Unknown button pressed
					default:
						break;
				}
			}
			catch( Exception exception )
			{
				MessageBox.Show(exception.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				Debug.WriteLine(exception.ToString());
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private System.Windows.Forms.Form GetForm( System.Type aType, object[] args, bool aAlwaysCreateNewForm)
		{
            //Begin TT#1388-MD -jsobek -Product Filters
            //try
            //{
            //    bool foundForm = false;
            //    System.Windows.Forms.Form frm = null;

            //    if (!aAlwaysCreateNewForm)
            //    {
            //        foreach (Form childForm in this.MdiChildren) 
            //        {
            //            if (childForm.GetType().Equals(aType))
            //            {
            //                frm = childForm;
            //                foundForm = true;
            //                break;
            //            }
            //        }
            //    }

            //    if (aAlwaysCreateNewForm ||
            //        !foundForm)
            //    {
            //        frm = (System.Windows.Forms.Form)Activator.CreateInstance(aType, args);
            //    }

            //    return frm;
            //}
            //catch( Exception exception )
            //{
            //    MessageBox.Show(exception.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    throw;
            //}
            return SharedRoutines.GetForm(this.MdiChildren, aType, args, aAlwaysCreateNewForm);
            //End TT#1388-MD -jsobek -Product Filters
		}

		private void NavigateToWindow( System.Windows.Forms.Form frm, string request)
		{
			string navTo = string.Empty;
			try
			{
				// Navigated-To windows were being opened twice within the same transaction
				// when the icons were rapidly double-clicked. The _inNavigate switch prevents
				// that from occurring
				if (!_inNavigate)
				{
					_inNavigate = true;
					switch (frm.Name)
					{
						case "StyleView":	
							MIDRetail.Windows.StyleView frmStyleView = (MIDRetail.Windows.StyleView)frm;
							if (request == Include.tbbSummary || request == Include.btSummary)  
								navTo = Include.tbbSummary;
							//Begin Assortment Planning - JScott - Assortment Planning Changes
							else if (request == Include.tbbAssortment || request == Include.btAssortment)
								navTo = Include.tbbAssortment;
							// Begin TT#950 - MD - stodd - missing "Group Allocation" button
                            else if (request == Include.tbbGroupAllocation || request == Include.btGroupAllocation)
                                navTo = Include.tbbGroupAllocation;
							// End TT#950 - MD - stodd - missing "Group Allocation" button
							//End Assortment Planning - JScott - Assortment Planning Changes
							else if (request == Include.tbbSize || request == Include.btSize) 
								navTo = Include.tbbSize;
					
							frmStyleView.Navigate(navTo);
							break;
						 
						case "SummaryView":
							MIDRetail.Windows.SummaryView frmSummaryView = (MIDRetail.Windows.SummaryView)frm;
							if (request == Include.tbbStyle || request == Include.btStyle)  
								navTo = Include.tbbStyle;
							//Begin Assortment Planning - JScott - Assortment Planning Changes
							else if (request == Include.tbbAssortment || request == Include.btAssortment)
								navTo = Include.tbbAssortment;
							// Begin TT#950 - MD - stodd - missing "Group Allocation" button
                            else if (request == Include.tbbGroupAllocation || request == Include.btGroupAllocation)
                                navTo = Include.tbbGroupAllocation;
							// End TT#950 - MD - stodd - missing "Group Allocation" button
							//End Assortment Planning - JScott - Assortment Planning Changes
							else if (request == Include.tbbSize || request == Include.btSize) 
								navTo = Include.tbbSize;
					
							frmSummaryView.Navigate(navTo);
							break;

						//Begin Assortment Planning - JScott - Assortment Planning Changes
						case "AssortmentView":
							MIDRetail.Windows.AssortmentView frmAssortmentView = (MIDRetail.Windows.AssortmentView)frm;
							if (request == Include.tbbStyle || request == Include.btStyle)
								navTo = Include.tbbStyle;
							else if (request == Include.tbbSummary || request == Include.btSummary)
								navTo = Include.tbbSummary;
							else if (request == Include.tbbSize || request == Include.btSize)
								navTo = Include.tbbSize;

							frmAssortmentView.Navigate(navTo);
							break;
						//End Assortment Planning - JScott - Assortment Planning Changes

						// BEGIN TT#488-MD - STodd - Group Allocation - 
						case "Group Allocation View":
                            MIDRetail.Windows.AssortmentView frmGroupAllocationView = (MIDRetail.Windows.AssortmentView)frm;
							if (request == Include.tbbStyle || request == Include.btStyle)
								navTo = Include.tbbStyle;
							else if (request == Include.tbbSummary || request == Include.btSummary)
								navTo = Include.tbbSummary;
							else if (request == Include.tbbSize || request == Include.btSize)
								navTo = Include.tbbSize;

							frmGroupAllocationView.Navigate(navTo);
							break;
						// END TT#488-MD - STodd - Group Allocation - 

						case "SizeView":
							MIDRetail.Windows.SizeView frmSizeView = (MIDRetail.Windows.SizeView)frm;
							if (request == Include.tbbStyle || request == Include.btStyle)  
								navTo = Include.tbbStyle;
							else if (request == Include.tbbSummary || request == Include.btSummary) 
								navTo = Include.tbbSummary;
							//Begin Assortment Planning - JScott - Assortment Planning Changes
							else if (request == Include.tbbAssortment || request == Include.btAssortment)
								navTo = Include.tbbAssortment;
							// Begin TT#950 - MD - stodd - missing "Group Allocation" button
                            else if (request == Include.tbbGroupAllocation || request == Include.btGroupAllocation)
                                navTo = Include.tbbGroupAllocation;
							// End TT#950 - MD - stodd - missing "Group Allocation" button
							//End Assortment Planning - JScott - Assortment Planning Changes
					
							frmSizeView.Navigate(navTo);
							break;
					}
					_inNavigate = false;
				}
			}
			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
		}

		// Begin TT#2 - stodd - assortment
		internal void ShowExplorer( string Key )
		// End TT#2 - stodd - assortment
		{
			try
			{
				//Begin TT#1138 - JScott - Tabbed Explorers do not re-open as active
				//if ( udmMain.ControlPanes[Key].Closed )
				//    udmMain.ControlPanes[Key].Show();
				//else
				//    udmMain.ControlPanes[Key].Activate();

                //if (udmMain.ControlPanes.Contains(Key))
                //{
                    if (udmMain.ControlPanes[Key].Closed)
                    {
                        udmMain.ControlPanes[Key].Show();
                    }

                    udmMain.ControlPanes[Key].Activate();
                //}
                //else
                //{
                //    string lstPanes = "";
                //    foreach (Infragistics.Win.UltraWinDock.DockableControlPane p in udmMain.ControlPanes)
                //    {
                //        lstPanes += p.Key + "\r\n";
                //    }
                //    throw new Exception("Explorer key: " + Key + " not found in the following list: \r\n" + lstPanes);
                //}
				//End TT#1138 - JScott - Tabbed Explorers do not re-open as active
			}
			catch ( Exception exception )
			{
				throw new Exception(exception.Message, exception.InnerException);
			}
        }

        //Begin TT#1313-MD -jsobek -Header Filters -Set the Style and Parent of Style level names one time
        public void SetMerchandiseStyleAndParentofStyleLevelNames()
        {
            HierarchyProfile _mainHp = _SAB.HierarchyServerSession.GetMainHierarchyData();
         
            // get hierarchy profiles for style and parent of style
            HierarchyLevelProfile hlpStyle = null;
            HierarchyLevelProfile hlpParentOfStyle = null;
            for (int level = 1; level <= _mainHp.HierarchyLevels.Count; level++)
            {
                hlpParentOfStyle = hlpStyle;
                hlpStyle = (HierarchyLevelProfile)_mainHp.HierarchyLevels[level];
                if (hlpStyle.LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }
            if (hlpStyle != null)
            {
                FilterCommon.MerchandiseStyleLevelName = hlpStyle.LevelID;
            }
            if (hlpParentOfStyle != null)
            {
                FilterCommon.MerchandiseParentofStyleLevelName = hlpParentOfStyle.LevelID;
            }
        }
        //End TT#1313-MD -jsobek -Header Filters -Set the Style and Parent of Style level names one time

        #region Menu Handler

        /// <summary>
        /// This is the event handler that is called when a menu change is desired
        /// </summary>
        /// <param name="source">The module requesting the change</param>
        /// <param name="e">The MIDMenuEventArgs that contain the change</param>
        void OnMenuChange(object source, MIDMenuEventArgs e)
        {
            switch (e.MIDMenuAction)
            {
                case eMIDMenuAction.Add:
                    AddMenuItem(e.MIDMenuItem);
                    break;
                case eMIDMenuAction.Remove:
                    RemoveMenuItem(e.MIDMenuItem);
                    break;
                case eMIDMenuAction.Disable:
                    DisableMenuItem(e.MIDMenuItem);
                    break;
                case eMIDMenuAction.Enable:
                    EnableMenuItem(e.MIDMenuItem);
                    break;
                case eMIDMenuAction.Hide:
                    HideMenuItem(e.MIDMenuItem);
                    break;
                case eMIDMenuAction.Show:
                    ShowMenuItem(e.MIDMenuItem);
                    break;
                // Begin TT#335 - JSmith - Menu should say Remove and not Delete
                case eMIDMenuAction.Rename:
                    RenameMenuItem(e.MIDMenuItem, e.Text);
                    break;
                // End TT#335
            }
        }

        public void RemoveMenuItem(eMIDMenuItem aMenuItem)
        {
            try
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void EnableMenuItem(eMIDMenuItem aMenuItem)
        {
            try
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void DisableMenuItem(eMIDMenuItem aMenuItem)
        {
            try
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void HideMenuItem(eMIDMenuItem aMenuItem)
        {
            try
            {
                string menuItem = Include.GetMenuItem(aMenuItem);
                
                if (menuItem != string.Empty)
                {
                    //if (utmMain.Tools.Contains(menuItem))
                    //{
                    // contains doesn't work so swallow any error
                    //BEGIN TT#3046-MD-VStuart-Allocation Workspace View-Error cause login problems
                    if (utmMain.Tools.Contains(menuItem))
                    {
                        try
                        {
                            utmMain.Tools[menuItem].SharedProps.Visible = false;
                        }
                        catch
                        {
                        }
                    }
                    //END TT#3046-MD-VStuart-Allocation Workspace View-Error cause login problems
                    //}
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ShowMenuItem(eMIDMenuItem aMenuItem)
        {
            try
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
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        public void RenameMenuItem(eMIDMenuItem aMenuItem, string aText)
        {
            try
            {
                string menuItem = Include.GetMenuItem(aMenuItem);

                if (menuItem != string.Empty)
                {
                    try
                    {
                        utmMain.Tools[menuItem].SharedProps.Caption = aText;
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        // End TT#335

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

        #endregion

        #region Resize Allocation Workspace

        // Assortment addition - RonM
        public void ResizeWorkspacePane(Size aNewSize)
        {
            try
            {
                DockableControlPane dcPane = udmMain.PaneFromControl(this.AllocationWorkspaceExplorer1);
                if (dcPane.Pinned)
                {
                    dcPane.DockAreaPane.Size = aNewSize;
                }
                else
                {
                    dcPane.FlyoutSize = aNewSize;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        // // BEGIN MID Track #5501 - add 'Save Changes?' message
        public void DisplayWorkspacePane()
        {
            try
            {
                DockableControlPane dcPane = udmMain.PaneFromControl(this.AllocationWorkspaceExplorer1);
                if (!dcPane.IsVisible)
                {
                    dcPane.Show();
                    if (!dcPane.Pinned)
                    {
                        dcPane.Flyout(false);
                    }
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }
        // END MID Track #5501  

        // Begin TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields
        bool refreshDisplayed = false;
        public void AddStoreExplorerPendingRefresh()
        {
            try
            {
                if (!refreshDisplayed)
                {
                    string refreshMsg = MIDText.GetTextOnly(eMIDTextCode.lbl_RefreshToSeeChanges);
                    DockableControlPane dcPane = udmMain.PaneFromControl(this.storeGroupExplorer1);
                    dcPane.Text = "Store Explorer - " + refreshMsg;
                    dcPane.TextTab = "Store Explorer - " + refreshMsg;
                    dcPane.ToolTipCaption = "Store  Explorer - " + refreshMsg;
                    dcPane.ToolTipTab = "Store  Explorer - " + refreshMsg;
                    refreshDisplayed = true;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        public void RemoveStoreExplorerPendingRefresh()
        {
            try
            {
                if (refreshDisplayed)
                {
                    DockableControlPane dcPane = udmMain.PaneFromControl(this.storeGroupExplorer1);
                    dcPane.Text = "Store Explorer";
                    dcPane.TextTab = "Store Explorer";
                    dcPane.ToolTipCaption = "Store  Explorer";
                    dcPane.ToolTipTab = "Store  Explorer";
                    refreshDisplayed = false;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }
        // End TT#1844-MD - JSmith - Store Name when changed not changing in Store Explorer for Edit Store or Edit Fields

        // Assortment addition - RonM
        private void udmMain_AfterPaneButtonClick(object sender, Infragistics.Win.UltraWinDock.PaneButtonEventArgs e)
        {
            try
            {
                if (e.Pane == udmMain.PaneFromControl(this.AllocationWorkspaceExplorer1) && e.Button == PaneButton.Pin)
                {
                    ResizeWorkspacePane(e.Pane.ActualSize);
                }    
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception.InnerException);
            }
        }

        #endregion
    }

	public class ExplorerMessageFilter : IMessageFilter 
	{
		public bool PreFilterMessage(ref Message m) 
		{
			System.Windows.Forms.Control control;
			System.Windows.Forms.ComboBox comboBox;

			if (m.Msg == 522)
			{
				control = System.Windows.Forms.Control.FromHandle(m.HWnd);
				if (control == null)
				{
					control = System.Windows.Forms.Control.FromChildHandle(m.HWnd);
				}

				if (control != null)
				{
                    // Begin TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns
                    if (control.GetType() == typeof(System.Windows.Forms.ComboBox) ||
                        control.GetType().IsSubclassOf(typeof(System.Windows.Forms.ComboBox)))
                    {
                        comboBox = (System.Windows.Forms.ComboBox)control;
                        if (!comboBox.DroppedDown)
                        {
                            return true;
                        }
                    }
                    //else
                    //{
                    //    return true;
                    //}
                    // End TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns

                    //Begin Track #5858 - JSmith - Validating store security only
                    if (control.Tag != null)
                    {
                        if (control.Tag.GetType() == typeof(System.String))
                        {
                    //End Track #5858
                            if (Convert.ToString(control.Tag, CultureInfo.CurrentUICulture) == "IgnoreMouseWheel")
                            {
                                if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                                {
                                    comboBox = (System.Windows.Forms.ComboBox)control;
                                    if (!comboBox.DroppedDown)
                                    {
                                        return true;
                                    }
                                }
                                // Begin TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns
                                //else
                                //{
                                //    return true;
                                //}
                                // End TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns
                            }
                        //Begin Track #5858 - JSmith - Validating store security only
                        }
                        else if (control.Tag.GetType().IsSubclassOf(typeof(MIDRetail.Windows.MIDControlTag)))
                        {
                            if (((MIDRetail.Windows.MIDControlTag)control.Tag).IgnoreWheelMouse)
                            {
                                if (control.GetType() == typeof(System.Windows.Forms.ComboBox))
                                {
                                    comboBox = (System.Windows.Forms.ComboBox)control;
                                    if (!comboBox.DroppedDown)
                                    {
                                        return true;
                                    }
                                }
                                // Begin TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns
                                //else
                                //{
                                //    return true;
                                //}
                                // End TT#2683 - JSmith - Unable to use the mouse scroller in dropdowns
                            }
                        }
                    }
                    //End Track #5858
				}
			}

			return false;
		}

	}
}
