using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{

	public class frmFilterBuilder : MIDFormBase
	{
        private SessionAddressBlock _SAB;
        private ExplorerAddressBlock _EAB;
        public filterBuilderControl filterBuilder1;

        //private MIDFilterNode _currParentNode;
        //private MIDFilterNode _userNode;
        //private MIDFilterNode _globalNode;
        //private int _initialUserRID;
        //private int _initialOwnerRID;
        private bool _readOnly;
        private bool _executeAfterEditing = false;
        //public eUpdateMode aUpdateMode;
        //private FunctionSecurityProfile _filterUserSecurity;
        //private FunctionSecurityProfile _filterGlobalSecurity;

        public bool isFromAllocationWorkspace = false;
        public bool isFromAssortmentWorkpace = false;

   

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        public frmFilterBuilder(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, bool aReadOnly, bool executeAfterEditing) //TT#1313-MD -jsobek -Header Filters  
            : base(aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            _SAB = aSAB;
            _EAB = aEAB;

            //_currParentNode = aCurrParentNode;
            //_userNode = aUserNode;
            //_globalNode = aGlobalNode;
            //_initialUserRID = aUserRID;
            //_initialOwnerRID = aOwnerRID;
			
             SetReadOnly(!aReadOnly);	// TT#1350-MD - stodd - Prompt user when closing form when changes pending
            // SetReadOnly enables the "Apply" control. This sets it back to enabled=false.
            this.filterBuilder1.Handle_DisableConditionToolbar(new object(), null);

            _readOnly = aReadOnly;
            _executeAfterEditing = executeAfterEditing;
            this.filterBuilder1.readOnly = _readOnly;
		}

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
		
				this.Load -= new System.EventHandler(this.frmFilterBuilder_Load);
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
            this.filterBuilder1 = new MIDRetail.Windows.Controls.filterBuilderControl();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // filterBuilder1
            // 
            this.filterBuilder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterBuilder1.Location = new System.Drawing.Point(0, 0);
            this.filterBuilder1.Name = "filterBuilder1";
            this.filterBuilder1.Size = new System.Drawing.Size(730, 585);
            this.filterBuilder1.TabIndex = 20;
            // 
            // frmFilterBuilder
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(730, 510);
            this.Controls.Add(this.filterBuilder1);
            this.Name = "frmFilterBuilder";
            this.Text = "Filter Builder";
            this.Load += new System.EventHandler(this.frmFilterBuilder_Load);
            this.Controls.SetChildIndex(this.filterBuilder1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void frmFilterBuilder_Load(object sender, System.EventArgs e)
		{
			//FunctionSecurityProfile showLoginSecurity;
			FormLoaded = false;
            base.DisposeChildControlsOnClose = false;
            //Format_Title(eDataState.Updatable, eMIDTextCode.frm_UserOptions, MIDText.GetTextOnly(eMIDTextCode.frm_UserOptions));
            //SetReadOnly(true);
            //SetText();
            //if (SAB.ClientServerSession.UserOptions.ForecastMonitorIsActive)
            //{
            //    this.radForecastMonitorOn.Checked = true;
            //}
            //else
            //{
            //    this.radForecastMonitorOff.Checked = true;
            //}
            //this.txtForecastMonitorDirectory.Text = SAB.ClientServerSession.UserOptions.ForecastMonitorDirectory;
            //if (SAB.ClientServerSession.UserOptions.ModifySalesMonitorIsActive)
            //{
            //    this.radModifySalesMonitorOn.Checked = true;
            //}
            //else
            //{
            //    this.radModifySalesMonitorOff.Checked = true;
            //}
            //this.txtModifySalesMonitorDirectory.Text = SAB.ClientServerSession.UserOptions.ModifySalesMonitorDirectory;

            //showLoginSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsShowLogin);
            //if (SAB.ClientServerSession.GlobalOptions.UseWindowsLogin &&
            //    showLoginSecurity.AllowView)
            //{
            //    if (SAB.ClientServerSession.UserOptions.ShowLogin)
            //    {
            //        this.cbxShowLogin.Checked = true;
            //    }
            //    else
            //    {
            //        this.cbxShowLogin.Checked = false;
            //    }
            //}
            //else
            //{
            //    this.cbxShowLogin.Checked = false;
            //    this.cbxShowLogin.Visible = false;
            //}
            //LoadLoggingLevels();
            //cbxAuditLoggingLevel.SelectedValue = SAB.ClientServerSession.UserOptions.AuditLoggingLevel;
            this.filterBuilder1.SaveFilterEvent += new filterBuilderControl.SaveFilterEventHandler(Handle_SaveFilter);	
            this.filterBuilder1.CloseFilterEvent += new filterBuilderControl.CloseFilterEventHandler(Handle_CloseFilter);
            this.filterBuilder1.UpdateTitleEvent += new filterBuilderControl.UpdateTitleEventHandler(Handle_UpdateTitle); // TT#1351-MD - stodd - change window text when filter name changes - 
			// Begin TT#1350-MD - stodd - Prompt user when closing form when changes pending
            this.filterBuilder1.UpdateWindowDirtyEvent += new filterBuilderControl.UpdateWindowDirtyEventHandler(Handle_UpdateWindowDirty); // TT#1351-MD - stodd - change window text when filter name changes - 
            this.filterBuilder1.UpdateChangePendingEvent += new filterBuilderControl.UpdateChangePendingEventHandler(Handle_SetChangePending); // TT#1351-MD - stodd - change window text when filter name changes - 
			// End TT#1350-MD - stodd - Prompt user when closing form when changes pending
            this.filterBuilder1.ShowFilterMonitorEvent += new filterBuilderControl.ShowFilterMonitorEventHandler(HandleShowFilterMonitor);
            // Begin TT#4561 - JSmith - File>Save and File>Save As are not recognized
            DisableMenuItem(this, eMIDMenuItem.FileSave);
			DisableMenuItem(this, eMIDMenuItem.FileSaveAs);
            // End TT#4561 - JSmith - File>Save and File>Save As are not recognized
			FormLoaded = true;

            
		}

		private void SetText()
		{
			try
			{
                //this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
                //this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
                //this.gbxForecastMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastMonitor);
                //this.radForecastMonitorOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Off);
                //this.radForecastMonitorOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_On);
                //this.gbxModifySalesMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModifySalesMonitor);
                //this.radModifySalesMonitorOff.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Off);
                //this.radModifySalesMonitorOn.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_On);
                //this.lblAuditLoggingLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AuditLoggingLevel);
                //this.btnForecastMonitorDirectory.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Directory);
                //this.btnModifySalesMonitor.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Directory);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

        private bool IsInUse(eProfileType etype, int aRID)
        {
            bool isInUse = false;
            try
            {
                ArrayList ridList = new ArrayList();
                ridList.Add(aRID);
                if (ridList.Count > 0) //If no RID is selected do nothing.
                {
                    string inUseTitle = InUseUtility.GetInUseTitleFromProfileType(etype);
                    MIDFormBase fb = new MIDFormBase();
                    fb.DisplayInUseForm(ridList, etype, inUseTitle, false, out isInUse);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            return isInUse;
        }

        // Begin TT#5783 - JSmith - Fill Size Error - System Error
        protected override void BeforeClosing()
        {
            try
            {
                base.BeforeClosing();

                // Refresh Store Explorer to insure it is in sync with StoreManagement
				if (this.filterBuilder1.manager.currentFilter.filterType == filterTypes.StoreGroupFilter)
                {
                    this._EAB.StoreGroupExplorer.IRefresh();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // End TT#5783 - JSmith - Fill Size Error - System Error

        protected override void AfterClosing()
        {
            try
            {
                base.AfterClosing();

                if (OnFilterPropertiesCloseHandler != null)
                {
                    FilterPropertiesCloseEventArgs e = new FilterPropertiesCloseEventArgs();
                    e.newFilterRID = this.filterBuilder1.manager.currentFilter.filterRID;
                    OnFilterPropertiesCloseHandler(this, e);
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
		

        //private void btnOK_Click(object sender, System.EventArgs e)
        //{
        //    SaveChanges();
        //    if (!ErrorFound)
        //    {
        //        this.Close();
        //    }
        //}

        //private void btnCancel_Click(object sender, System.EventArgs e)
        //{
        //    this.Close();
        //}

		override protected bool SaveChanges()
		{
			ErrorFound = false;
			// Begin TT#1350-MD - stodd - Prompt user when closing form when changes pending
            if (ChangePending)
            {
                this.filterBuilder1.Save();
                ChangePending = false;
            }
			// End TT#1350-MD - stodd - Prompt user when closing form when changes pending
			return true;
		}
        public CalendarDateSelector calendarDateSelectorForm = null;
        private string _filterTypeName;  //TT#1313-MD -jsobek -Header Filters
        public void LoadFilter(filter f)
        {
            if (calendarDateSelectorForm  == null)
            {
                calendarDateSelectorForm = new CalendarDateSelector(_SAB);
            }
            if (filterUtility.getDateRangeTextForPlanDelegate == null)
            {
                filterUtility.getDateRangeTextForPlanDelegate = new GetDateRangeTextForPlanDelegate(calendarDateSelectorForm.GetDateRangeTextForPlan);
            }
            this.filterBuilder1.LoadFilter(f, _SAB, calendarDateSelectorForm, new SetCalendarDateRangeForPlanDelegate(calendarDateSelectorForm.SetDateForPlan), new IsStoreGroupOrLevelInUse(SharedRoutines.IsStoreGroupOrStoreGroupLevelInUse));
            //Begin TT#1313-MD -jsobek -Header Filters
            _filterTypeName = f.filterType.Name;
            SetTitle(f.filterName);
            //End TT#1313-MD -jsobek -Header Filters
        }
        //Begin TT#1313-MD -jsobek -Header Filters
        private void SetTitle(string filterName)
        {
            string windowTitle = _filterTypeName + " -" + filterName;
            if (_readOnly)
            {
                windowTitle += " (Read Only)";
            }
            this.Text = windowTitle;
        }
        //End TT#1313-MD -jsobek -Header Filters


        public void SelectCondition(int levelRID, string levelName)
        {
            this.filterBuilder1.SelectCondition(levelRID, levelName);
        }


		// Begin TT#1350-MD - stodd - Prompt user when closing form when changes pending
        private void SetWindowDirty(bool isWindowDirty)
        {
            ChangePending = isWindowDirty;
        }
		// End TT#1350-MD - stodd - Prompt user when closing form when changes pending

        private void Handle_CloseFilter(object sender, filterBuilderControl.CloseFilterEventArgs e)
        {
         
            this.Close();
            //if (OnFilterPropertiesCloseHandler != null)
            //{
            //    OnFilterPropertiesCloseHandler(this, new FilterPropertiesCloseEventArgs());
            //}
        }
        //Begin TT#1313-MD -jsobek -Header Filters
        private void Handle_UpdateTitle(object sender, filterBuilderControl.UpdateTitleEventArgs e)
        {
            SetTitle(e.newFilterName);
        }
        //End TT#1313-MD -jsobek -Header Filters
				// Begin TT#1350-MD - stodd - Prompt user when closing form when changes pending
        private void Handle_UpdateWindowDirty(object sender, filterBuilderControl.UpdateWindowDirtyEventArgs e)
        {
            SetWindowDirty(e.isWindowDirty);
        }

        private void Handle_SetChangePending(object sender, filterBuilderControl.UpdateChangePendingEventArgs e)
        {
            ChangePending = e.isChangePending;
        }
		// End TT#1350-MD - stodd - Prompt user when closing form when changes pending

        private void Handle_SaveFilter(object sender, filterBuilderControl.SaveFilterEventArgs e)
        {
            try
            {

                if (e.filterToSave.filterType == filterTypes.StoreFilter)
                {
                    ((FilterStoreTreeView)this._EAB.FilterExplorerStore.TreeView).AfterSave(e.filterToSave);
                }
                if (e.filterToSave.filterType == filterTypes.StoreGroupFilter || e.filterToSave.filterType == filterTypes.StoreGroupDynamicFilter)
                {

                    //Filter and its conditions have been saved to the db by now.
                    StoreGroupProfile groupProfile = StoreMgmt.StoreGroup_AddOrUpdate(e.filterToSave, isNewGroup: e.isNewFilter, loadNewResults: true);  //Adds group and levels and results to the db.  Executes the filter.
                    ((StoreTreeView)(this._EAB.StoreGroupExplorer.TreeView)).AfterSave(e.filterToSave, groupProfile);  //Adds db entry to the FOLDER table
                    ChangePending = false;
                    this.Close();
                }
                else if (e.filterToSave.filterType == filterTypes.HeaderFilter)
                {
                    ((FilterHeaderTreeView)this._EAB.FilterExplorerHeader.TreeView).AfterSave(e.filterToSave);
                    filterEngineSQL.CreateOrUpdateSqlForFilter(e.filterToSave);


                    if (this._executeAfterEditing)
                    {
                        this._EAB.AllocationWorkspaceExplorer.BindFilterComboBox();
                        this._EAB.AllocationWorkspaceExplorer.SetHeaderFilter(e.filterToSave.filterRID);
                    }
                    else
                    {
                        this._EAB.AllocationWorkspaceExplorer.BindFilterComboBox();
                    }
                    //if (isFromAllocationWorkspace)
                    //{
                    //    this._EAB.AllocationWorkspaceExplorer.workspaceFilter = e.filterToSave;
                    //    this._EAB.AllocationWorkspaceExplorer.ApplyNewFilter();
                    //}
                    //else if (isFromAssortmentWorkpace)
                    //{
                    //    this._EAB.AssortmentWorkspaceExplorer.workspaceFilter = e.filterToSave;
                    //    this._EAB.AssortmentWorkspaceExplorer.ApplyNewFilter();
                    //}
                }
                else if (e.filterToSave.filterType == filterTypes.AssortmentFilter)
                {
                    ((FilterAssortmentTreeView)this._EAB.FilterExplorerAssortment.TreeView).AfterSave(e.filterToSave);
                    filterEngineSQL.CreateOrUpdateSqlForFilter(e.filterToSave);

                    if (this._executeAfterEditing)
                    {
                        // Begin TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                        //this._EAB.AllocationWorkspaceExplorer.BindFilterComboBox();
                        this._EAB.AssortmentWorkspaceExplorer.BindFilterComboBox();
                        // End TT#2134-MD - JSmith - Assortment Filter conditions need to be limited to Assortment fields only
                        this._EAB.AssortmentWorkspaceExplorer.SetHeaderFilter(e.filterToSave.filterRID);
                    }
                    else
                    {
                        this._EAB.AssortmentWorkspaceExplorer.BindFilterComboBox();
                    }
                    //if (isFromAllocationWorkspace)
                    //{
                    //    this._EAB.AllocationWorkspaceExplorer.workspaceFilter = e.filterToSave;
                    //    this._EAB.AllocationWorkspaceExplorer.ApplyNewFilter();
                    //}
                    //else if (isFromAssortmentWorkpace)
                    //{
                    //    this._EAB.AssortmentWorkspaceExplorer.workspaceFilter = e.filterToSave;
                    //    this._EAB.AssortmentWorkspaceExplorer.ApplyNewFilter();
                    //}
                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
        }
        //private MIDFilterNode GetNewSaveToNode()
        //{
        //    try
        //    {
        //        //if (rdoOwner.Checked)
        //        //{
        //        //    return _currParentNode;
        //        //}
        //        //else if (rdoUser.Checked)
        //        //{
        //        //    if (rdoUser.Tag != null)
        //        //    {
        //        //        return _currParentNode;
        //        //    }
        //        //    else
        //        //    {
        //        //        return _userNode;
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    if (rdoGlobal.Tag != null)
        //        //    {
        //        //        return _currParentNode;
        //        //    }
        //        //    else
        //        //    {
        //        //        return _globalNode;
        //        //    }
        //        //}
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}
        //private int GetNewUserRID()
        //{
        //    try
        //    {
        //        //if (rdoOwner.Checked)
        //        //{
        //        //    return _initialUserRID;
        //        //}
        //        //else if (rdoUser.Checked)
        //        //{
        //        //    return SAB.ClientServerSession.UserRID;
        //        //}
        //        //else if (rdoGlobal.Checked)
        //        //{
        //        //    return Include.GlobalUserRID;
        //        //}
        //        //else
        //        //{
        //        //    return Include.SystemUserRID;
        //        //}
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}

        //private int GetNewOwnerRID()
        //{
        //    try
        //    {
        //        //if (rdoOwner.Checked)
        //        //{
        //        //    return _initialOwnerRID;
        //        //}
        //        //else if (rdoUser.Checked)
        //        //{
        //        //    return SAB.ClientServerSession.UserRID;
        //        //}
        //        //else if (rdoGlobal.Checked)
        //        //{
        //        //    return Include.GlobalUserRID;
        //        //}
        //        //else
        //        //{
        //        //    return Include.SystemUserRID;
        //        //}
        //    }
        //    catch (Exception exc)
        //    {
        //        string message = exc.ToString();
        //        throw;
        //    }
        //}


        //public delegate void FilterPropertiesSaveEventHandler(object source, FilterPropertiesSaveEventArgs e);
        //public event FilterPropertiesSaveEventHandler OnFilterPropertiesSaveHandler;


        private void HandleShowFilterMonitor(object sender, filterBuilderControl.ShowFilterMonitorEventArgs e)
        {
            //if (e.showMonitor)
            FilterMonitorForm frmFilterMonitor = new FilterMonitorForm();
            FilterMonitor.PrepareDataTableForBinding();
            frmFilterMonitor.Show();
            //object[] args = new object[] { };
            //System.Windows.Forms.Form frmFilterMonitor = SharedRoutines.GetForm(this.MdiParent.MdiChildren, typeof(FilterMonitorForm), args, false);

            //if (e.showMonitor)
            //{
            //    FilterMonitor.PrepareDataTableForBinding();
            //    frmFilterMonitor.MdiParent = this.MdiParent;
            //    frmFilterMonitor.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            //    frmFilterMonitor.Show();
            //    frmFilterMonitor.BringToFront();
            //}
            //else
            //{
            //    if (frmFilterMonitor != null)
            //    {
            //        frmFilterMonitor.Close();
            //    }
            //}
    
        }


        public delegate void FilterPropertiesCloseEventHandler(object source, FilterPropertiesCloseEventArgs e);
        public event FilterPropertiesCloseEventHandler OnFilterPropertiesCloseHandler;
        public class FilterPropertiesCloseEventArgs : EventArgs
        {
            public int newFilterRID = -1;
        }
	}
}
