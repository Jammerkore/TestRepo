using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	public partial class ViewSave : MIDFormBase
	{
        // Begin TT#454 - RMatelic - Add Views in Style Review
        //public delegate void ViewSaveClosingEventHandler(object source, bool aViewSaved, int aViewRID, bool aViewDeleted);
        public delegate void ViewSaveClosingEventHandler(object source, ViewParms aViewParms);
        // End TT#454 
		public event ViewSaveClosingEventHandler OnViewSaveClosingEventHandler;

		private SessionAddressBlock _sab;
		
		private GridViewData _gridViewData;
		private ViewParms _viewParms = null;

        //Begin TT#1904 - DOConnell - Filter Error
        private MIDEnqueue _dlEnqueue;
        private int _VIEW_ID_Column_Size;
        private string msg_DatabaseColumnSizeExceeded;
        //End TT#1904 - DOConnell - Filter Error

        public bool useAssortmentFilters = false; //TT#1313-MD -jsobek -Header Filters

        public ViewSave(SessionAddressBlock aSAB, ViewParms aViewParms, bool useAssortmentFilters)	// TT#1390-MD - stodd - Assortment Workspace Save View lists headers filters instead of assortment header filters.
            : base(aSAB)
		{
			FunctionSecurityProfile functionSecurity;
			FunctionSecurityProfile userViewSecurity;
			FunctionSecurityProfile globalViewSecurity;

			InitializeComponent();

			_sab = aSAB;
            _viewParms = aViewParms;
			_gridViewData = new GridViewData();
            this.useAssortmentFilters = useAssortmentFilters;	// TT#1390-MD - stodd - Assortment Workspace Save View lists headers filters instead of assortment header filters.

            SetText();

			// Load View Info

            txtViewName.Text = _viewParms.ViewName;

            functionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(_viewParms.FunctionSecurity);

            userViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(_viewParms.UserViewSecurity);
            globalViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(_viewParms.GlobalViewSecurity);
            btnDelete.Visible = false;  // TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
            // Begin TT#454 - RMatelic - Add Views in Style Review - add 'if...' statement
            if (_viewParms.UpdateHandledByCaller)
            {
                chkApplyFilter.Visible = false;
                cboHeaderFilter.Visible = false;
                chkUseFilterSorting.Visible = false;
                //btnDelete.Visible = false;  // TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                chkShowDetails.Visible = false; // TT#953 - RMatelic - Saving Views in Style and Size Review-need to remove/disable "Save Details" checkbox
            }
            else
            {
                //Begin TT#1313-MD -jsobek -Header Filters
                //chkApplyFilter.Checked = _gridViewData.GridViewFilterExists(_viewParms.ViewRID);
                BindHeaderFilterComboBox(); //must bind first, then set
                bool useFilterSorting = false;
                int workspaceFilterRid = _gridViewData.GridViewReadWorkspaceFilterRID(_viewParms.ViewRID, ref useFilterSorting);
                if (workspaceFilterRid != Include.NoRID)
                {          
                    DataRow[] drFound = dtFilters.Select("FILTER_RID=" + workspaceFilterRid.ToString());
                    if (drFound.Length > 0)
                    {
                        chkApplyFilter.Checked = true;
                        cboHeaderFilter.SelectedIndex = cboHeaderFilter.Items.IndexOf(new FilterNameCombo(Convert.ToInt32(drFound[0]["FILTER_RID"], CultureInfo.CurrentUICulture),
                            Convert.ToInt32(drFound[0]["USER_RID"], CultureInfo.CurrentUICulture),
                            Convert.ToString(drFound[0]["FILTER_NAME"], CultureInfo.CurrentUICulture)));
                    }
                    else
                    {
                        chkApplyFilter.Checked = false;
                    }
                }
                else
                {
                    chkApplyFilter.Checked = false;
                }
                //End TT#1313-MD -jsobek -Header Filters

                //Begin TT#1477-MD -jsobek -Header Filter Sort on Workspace
                chkUseFilterSorting.Checked = useFilterSorting;
                //End TT#1477-MD -jsobek -Header Filter Sort on Workspace

            }
            // End TT#454

            // Begin TT#2 - RMatelic - Assortment Planning 
            //chkShowDetails.Checked = _viewParms.ViewShowDetails;    // Workspace Usability Enhancement
            if (!_viewParms.ShowDetailsCheckBox)
            {
                chkShowDetails.Visible = false;
            }
            else
            {
                chkShowDetails.Checked = _viewParms.ViewShowDetails;   
            }
            // End TT#2

			if (userViewSecurity.AllowUpdate || globalViewSecurity.AllowUpdate)
			{
				btnSave.Enabled = true;
                txtViewName.Enabled = true;
                chkApplyFilter.Enabled = true;
			}
			else
			{
				btnSave.Enabled = false;
                txtViewName.Enabled = false;
                chkApplyFilter.Enabled = false;
			}

			if (userViewSecurity.AllowUpdate)
			{
				rdoUser.Enabled = true;
			}
			else
			{
				rdoUser.Enabled = false;
			}

			if (globalViewSecurity.AllowUpdate)
			{
				rdoGlobal.Enabled = true;
			}
			else
			{
				rdoGlobal.Enabled = false;
			}

			if (_viewParms.ViewUserRID != Include.GlobalUserRID)	 
			{
				if (userViewSecurity.AllowUpdate)
				{
					rdoUser.Checked = true;
				}
				else
				{
					rdoGlobal.Checked = true;
				}
			}
			else
			{
				if (globalViewSecurity.AllowUpdate)
				{
					rdoGlobal.Checked = true;
				}
				else
				{
					rdoUser.Checked = true;
				}
			}
            // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
            //if (!_viewParms.UpdateHandledByCaller)      // TT#454 - RMatelic - Add Views in Style Review - add first 'if...' statement
            //{
            //    if (_viewParms.ViewRID != Include.NoRID)
            //    {
            //        if (_viewParms.ViewUserRID != Include.GlobalUserRID)
            //        {
            //            if (userViewSecurity.AllowDelete)
            //            {
            //                btnDelete.Enabled = true;
            //            }
            //            else
            //            {
            //                btnDelete.Enabled = false;
            //            }
            //        }
            //        else
            //        {
            //            if (globalViewSecurity.AllowUpdate)
            //            {
            //                btnDelete.Enabled = true;
            //            }
            //            else
            //            {
            //                btnDelete.Enabled = false;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        btnDelete.Enabled = false;
            //    }
            //}
            // End TT#1411-MD
        }   

        private void SetText()
        {
            try
            {
                this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SaveView);
                lblViewName.Text =  MIDText.GetTextOnly(eMIDTextCode.lbl_ViewName);
                rdoGlobal.Text = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsGlobal);
                rdoUser.Text = MIDText.GetTextOnly((int)eSecurityFunctions.AllocationViewsUser);
                //chkApplyFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SaveFilter);  //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
                //btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete); //TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
                btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        //Begin TT#1313-MD -jsobek -Header Filters
        private DataTable dtFilters;
        private void BindHeaderFilterComboBox()
        {
            try
            {
                FilterData filterData = new FilterData();
                ArrayList userRIDList = new ArrayList();
                userRIDList.Add(Include.GlobalUserRID);
                userRIDList.Add(SAB.ClientServerSession.UserRID);

         
                if (useAssortmentFilters)
                {
                    dtFilters = filterData.FilterRead(filterTypes.AssortmentFilter, eProfileType.FilterAssortment, userRIDList);
                }
                else
                {
                    dtFilters = filterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
                }

                foreach (DataRow row in dtFilters.Rows)
                {
                    cboHeaderFilter.Items.Add(
                        new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
                        Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
                        Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
                }
                AdjustTextWidthComboBox_DropDown(cboHeaderFilter);  
            }
            catch (Exception ex)
            {
                HandleException(ex, "BindFilterComboBox");
            }
        }
        //End TT#1313-MD -jsobek -Header Filters

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				_viewParms.ViewName = txtViewName.Text.Trim();
				//Begin TT#1904 - DOConnell - Filter Error
                string message;
                _dlEnqueue = new MIDEnqueue();
                _VIEW_ID_Column_Size = _dlEnqueue.GetColumnSize("GRID_VIEW", "VIEW_ID");

                msg_DatabaseColumnSizeExceeded = MIDText.GetText(eMIDTextCode.msg_DatabaseColumnSizeExceeded);
                if (_viewParms.ViewName.Length > _VIEW_ID_Column_Size)
                {
                    message = (string)msg_DatabaseColumnSizeExceeded.Clone();
                    message = message.Replace("{0}", "View Name= " + Convert.ToString(_viewParms.ViewName));
                    message = message.Replace("{1}", _VIEW_ID_Column_Size.ToString() + " characters ");
                    message = message.Replace("{2}", "VIEW_ID");
                    message = message.Replace("{3}", "GRID_VIEW");
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, message, "HierarchyReclassProcess");
                    string msg = MIDText.GetText(eMIDTextCode.msg_ViewSaveNameLength);
                    msg = msg.Replace("{0}", Convert.ToString(_VIEW_ID_Column_Size));
                    MessageBox.Show(msg, "View Save", MessageBoxButtons.OK);
                }
                else
                {

                    if (rdoUser.Checked)
                    {
                        _viewParms.ViewUserRID = _sab.ClientServerSession.UserRID;
                    }
                    else
                    {
                        _viewParms.ViewUserRID = Include.GlobalUserRID;
                    }

                    _viewParms.ViewFilterSaved = chkApplyFilter.Checked;
                    _viewParms.ViewShowDetails = chkShowDetails.Checked;    // Workspace Usability Enhancement

                    CheckForm();

                    // Begin TT#454 - RMatelic - Add Views in Style Review
                    //SaveValues();
                    if (!_viewParms.UpdateHandledByCaller)
                    {
                        SaveValues();
                    }
                    else
                    {
                        _viewParms.ViewSaved = true;
                    }
                    // End TT#454

                    this.Close();
                }
				//End TT#1904 - DOConnell - Filter Error
			}
			catch (EditErrorException exc)
			{
				MessageBox.Show(exc.Message, "Edit error", MessageBoxButtons.OK);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		
		private void CheckForm()
		{
			try
			{
                if (txtViewName.Text.Trim() == "")
                {
                    throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewNameMissing));
                }
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void SaveValues()
		{

            try
			{
                _gridViewData.OpenUpdateConnection();

				try
				{
                    _viewParms.ViewSaved = false;
                    _viewParms.ViewRID = _gridViewData.GridView_GetKey(_viewParms.ViewUserRID, _viewParms.LayoutID,_viewParms.ViewName);


                    //Begin TT#1313-MD -jsobek -Header Filter
                    int workspaceFilterRID = Include.NoRID;
                    bool useFilterSorting = false;
                    if (_viewParms.ViewFilterSaved && cboHeaderFilter.SelectedIndex != -1)
                    {
                        workspaceFilterRID = ((FilterNameCombo)cboHeaderFilter.SelectedItem).FilterRID;

                        if (chkUseFilterSorting.Checked)
                        {
                            useFilterSorting = true;
                        }
                    }

					if (_viewParms.ViewRID != -1)
					{
                        _gridViewData.GridView_Update(_viewParms.ViewRID, _viewParms.ViewShowDetails, Include.NoRID, Include.NoRID, false, workspaceFilterRID, useFilterSorting);
						_gridViewData.GridViewDetail_Delete(_viewParms.ViewRID);
					}
					else
					{
                        // Begin TT#456 - RMatelic - Add views to Size Review
                        //_viewParms.ViewRID = _gridViewData.GridView_Insert(_viewParms.ViewUserRID, _viewParms.LayoutID, _viewParms.ViewName);
                        _viewParms.ViewRID = _gridViewData.GridView_Insert(_viewParms.ViewUserRID, _viewParms.LayoutID, _viewParms.ViewName, _viewParms.ViewShowDetails, Include.NoRID, Include.NoRID, false, workspaceFilterRID, useFilterSorting);
                        // End TT#456
					}
                    //End TT#1313-MD -jsobek -Header Filter

                    _gridViewData.GridViewDetail_Insert(_viewParms.ViewRID, _viewParms.ViewGrid);

                    //Begin TT#1313-MD -jsobek -Header Filters
                    //_gridViewData.GridViewFilter_Delete(_viewParms.ViewRID);
                    //if (_viewParms.ViewFilterSaved)
                    //{
                    //    _gridViewData.GridViewFilter_Add(_viewParms.ViewRID, _sab.ClientServerSession.UserRID);
                    //}
                  
                    //End TT#1313-MD -jsobek -Header Filters


                    
              		_gridViewData.CommitData();
                    _viewParms.ViewSaved = true;
				}
				catch (Exception exc)
				{
					_gridViewData.Rollback();
					string message = exc.ToString();
					throw;
				}
				finally
				{
					_gridViewData.CloseUpdateConnection();
				}
			 
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Begin TT#1411-MD - RMatelic - Move the deletion of the allocation workspace views and the assortment workspace views to the View Maintenance window
            //try
            //{
            //    string viewType, message, lblDeleteView;
                
            //    if (_viewParms.ViewUserRID == Include.GlobalUserRID)
            //    {
            //        viewType = rdoGlobal.Text;
            //    }
            //    else
            //    {
            //        viewType = rdoUser.Text;
            //    }

            //    message = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_as_GridViewWillBeDeleted), viewType, _viewParms.ViewName);
            //    message += Environment.NewLine + MIDText.GetTextOnly((int)eMIDTextCode.msg_ContinueQuestion);
            //    lblDeleteView = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteView);

            //    DialogResult diagResult = MessageBox.Show(message, lblDeleteView, System.Windows.Forms.MessageBoxButtons.YesNo,
            //        System.Windows.Forms.MessageBoxIcon.Question);
              
            //    if (diagResult == System.Windows.Forms.DialogResult.No)
            //    {
            //        return;
            //    }
            //    DeleteView();
            //    this.Close();
            //}
          
            //catch (Exception exc)
            //{
            //    string message = exc.ToString();
            //    MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK);
            //}
            // End TT#1411-MD  
        }

        private void DeleteView()
        {
            try
            {
                _gridViewData.OpenUpdateConnection();

                try
                {
                    _gridViewData.GridView_Delete(_viewParms.ViewRID);            
                    _gridViewData.CommitData();
                    _viewParms.ViewDeleted = true;
                }
                catch (Exception exc)
                {
                    _gridViewData.Rollback();
                    string message = exc.ToString();
                    throw;
                }
                finally
                {
                    _gridViewData.CloseUpdateConnection();
                }

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		private void ViewSave_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            // Begin TT#454 - RMatelic - Add Views in Style Review
            //OnViewSaveClosingEventHandler(this, _viewParms.ViewSaved, _viewParms.ViewRID, _viewParms.ViewDeleted);
            OnViewSaveClosingEventHandler(this, _viewParms);
            // End TT#454 
		}

        //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
        private void chkApplyFilter_CheckedChanged(object sender, EventArgs e)
        {
            cboHeaderFilter.Enabled = chkApplyFilter.Checked;
            chkUseFilterSorting.Enabled = chkApplyFilter.Checked;   
        }
        //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
       
 	}

    public class ViewParms
	{
		//=======
		// FIELDS
		//=======

        private bool _viewSaved;
        private bool _viewDeleted;
        private bool _viewFilterSaved;
        private bool _viewShowDetails;  // Workspace Usability Enhancement
        private Infragistics.Win.UltraWinGrid.UltraGrid _viewGrid;
        private int _layoutID; 
        private int _viewRID;
		private string _viewName;
		private int _viewUserRID;
        private eSecurityFunctions _functionSecurity;
        private eSecurityFunctions _userViewSecurity;
        private eSecurityFunctions _globalViewSecurity;
        private bool _updateHandledByCaller = false;       // TT#454 - RMatelic - Add Views in Style Review
        private bool _showDetailsCheckBox = true;           // TT#2 - RMatelic - Assortment PLanning 
		//============= 
		// CONSTRUCTORS
		//=============

		public ViewParms()
		{
            _viewSaved = false;
            _viewDeleted = false;
            _viewFilterSaved = false;
            _viewShowDetails = false;   // Workspace Usability Enhancement
			_viewRID = Include.DefaultAssortmentViewRID;
            _viewName = string.Empty;
		}

		//===========
		// PROPERTIES
		//===========

        /// <summary>
        /// Gets or sets if the view was saved.
        /// </summary>
        /// 
        public bool ViewSaved
        {
            get
            {
                return _viewSaved;
            }
            set
            {
                _viewSaved = value;
            }
        }

        /// <summary>
        /// Gets or sets if the view was deleted.
        /// </summary>
        /// 
        public bool ViewDeleted
        {
            get
            {
                return _viewDeleted;
            }
            set
            {
                _viewDeleted = value;
            }
        }

        /// <summary>
        /// Gets or sets if the filter is saved.
        /// </summary>
        /// 
        public bool ViewFilterSaved
        {
            get
            {
                return _viewFilterSaved;
            }
            set
            {
                _viewFilterSaved = value;
            }
        }
        
        // BEGIN Workspace Usability Enhancement
        /// <summary>
        /// Gets or sets whether or not to show Workspace details grid.
        /// </summary>
        /// 
        public bool ViewShowDetails
        {
            get
            {
                return _viewShowDetails;
            }
            set
            {
                _viewShowDetails = value;
            }
        }
        // END Workspace Usability Enhancement

		/// <summary>
		/// Gets or sets the UltraGrid value.
		/// </summary>

        public Infragistics.Win.UltraWinGrid.UltraGrid ViewGrid
        {
            get
            {
                return  _viewGrid;
            }
            set
            {
                _viewGrid = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer value of eLayoutID of ViewGrid.
        /// </summary>

        public int LayoutID
        {
            get
            {
                return _layoutID;
            }
            set
            {
                _layoutID = value;
            }
        }

		/// <summary>
		/// Gets or sets the RID of View.
		/// </summary>

		public int ViewRID
		{
			get
			{
				return _viewRID;
			}
			set
			{
				_viewRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of View.
		/// </summary>

		public string ViewName
		{
			get
			{
				return _viewName;
			}
			set
			{
				_viewName = value;
			}
		}

		/// <summary>
		/// Gets or sets the User RID of View.
		/// </summary>

		public int ViewUserRID
		{
			get
			{
				return _viewUserRID;
			}
			set
			{
				_viewUserRID = value;
			}
		}

        /// <summary>
        /// Gets or sets the View function security.
        /// </summary>

        public eSecurityFunctions FunctionSecurity
        {
            get
            {
                return _functionSecurity;
            }
            set
            {
                _functionSecurity = value;
            }
        }

        /// <summary>
        /// Gets or sets the User View security.
        /// </summary>

        public eSecurityFunctions UserViewSecurity
        {
            get
            {
                return _userViewSecurity;
            }
            set
            {
                _userViewSecurity = value;
            }
        }

        /// <summary>
        /// Gets or sets the Global View security.
        /// </summary>
        public eSecurityFunctions GlobalViewSecurity
        {
            get
            {
                return _globalViewSecurity;
            }
            set
            {
                _globalViewSecurity = value;
            }
        }

        // Begin TT#454 - RMatelic - Add Views in Style Review
        /// <summary>
        /// Gets or sets if the invoking window will perform the uppdate.
        /// </summary>
        /// 
        public bool UpdateHandledByCaller
        {
            get
            {
                return _updateHandledByCaller;
            }
            set
            {
                _updateHandledByCaller = value;
            }
        }
        // End TT#454  

        // Begin TT#2 - RMatelic - Assortment PLanning
        /// <summary>
        /// Gets or sets if the Show Details checkbox is displayed.
        /// </summary>
        /// 
        public bool ShowDetailsCheckBox
        {
            get
            {
                return _showDetailsCheckBox;
            }
            set
            {
                _showDetailsCheckBox = value;
            }
        }
        // End TT#2  
	}
}
