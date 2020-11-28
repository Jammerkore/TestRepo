using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public partial class AssortmentViewSelection : MIDRetail.Windows.AssortmentBasisBase
	{
		private ApplicationSessionTransaction _trans;
		private ExplorerAddressBlock _eab;
		private AllocationHeaderProfileList _headerList;
		private FunctionSecurityProfile _assortmentReviewSecurity;
       
        private bool _bindingView;
		private bool _bypassScreen;
        private bool _assormentselected;
        private int _lastViewValue;

        private AssortmentViewData _assortmentViewData;
        private ArrayList _userRIDList;

        private string _groupName;
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName = value;
            }
        }


		public AssortmentViewSelection(ExplorerAddressBlock eab, SessionAddressBlock sab, 
			ApplicationSessionTransaction trans, AssortmentProfile ap, bool readOnly)
			: base(sab, eab, trans)
		{
			_trans = trans;
			_eab = eab;
        	InitializeComponent();
			AllowDragDrop = true;
			_assortmentReviewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);

			//_trans = Trans;
			//_SAB = _trans.SAB;
			//_EAB = eab;
			//_lbl_Header = MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
			//_lbl_Description = MIDText.GetTextOnly(eMIDTextCode.lbl_Description);
			//_lbl_RowPosition = "Row Position";
			//_lbl_RID = "RID";
			//_allocationReviewSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
			//_allocationReviewStyleSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
			//_allocationReviewSummarySecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
			//_allocationReviewSizeSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
			//DisplayPictureBoxImages();
			//SetPictureBoxTags();
		}

		private void AssortmentViewSelection_Load(object sender, System.EventArgs e)
		{
			try
			{
				FormLoaded = false;

				SetScreenText();

				base.LoadBase();

				BindGroupByComboBox();

                _userRIDList = new ArrayList();

                _userRIDList.Add(-1);

                FunctionSecurityProfile userViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUser);
                FunctionSecurityProfile globalViewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobal);
                if (userViewSecurity.AllowUpdate || userViewSecurity.AllowView)
                {
                    _userRIDList.Add(SAB.ClientServerSession.UserRID);
                }

                if (globalViewSecurity.AllowUpdate || globalViewSecurity.AllowView)
                {
                    _userRIDList.Add(Include.GlobalUserRID);  
                }
                _assortmentViewData = new AssortmentViewData();
				BindViewComboBox();

				cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, eMIDControlCode.form_AssortmentViewSelection, true, FunctionSecurity, true);
				cboView.Tag = "IgnoreMouseWheel";
				cboGroupBy.Tag = "IgnoreMouseWheel";
				_trans.AssortmentStoreAttributeRid = Convert.ToInt32(cboStoreAttribute.SelectedValue);
				_trans.AssortmentGroupBy = Convert.ToInt32(cboGroupBy.SelectedValue);
				SetReadOnly(_assortmentReviewSecurity.AllowUpdate);
                
                AdjustTextWidthComboBox_DropDown(cboStoreAttribute); //TT#7 - MD - ComboBox Custom Control - rbeck

                // Begin TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
                cbxOnhand.Visible = false;
                cbxIntransit.Visible = false;
                // End TT#2081-MD - JSmith - Regardless of Variable when allocating using Need in an Asst OH, IT, and VSW are always used.
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				FormLoaded = true;
			}

		}

		/// <summary>
		/// decides whether the Assortment view selection screen should be shown or
		/// does it bypass the screen and go straight to the assortment review screen.
		/// </summary>
		public void DetermineWindow(eAllocationSelectionViewType viewType)
		{
            try
            {
                System.EventArgs args = new EventArgs();
                Cursor.Current = Cursors.WaitCursor;
                _trans.CreateAssortmentViewSelectionCriteria();
                _trans.CreateAllocationViewSelectionCriteria();

                _trans.NewAssortmentCriteriaHeaderList();

                _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);

                if (_trans.AssortmentStoreAttributeRid == Include.NoRID)
                {
                    _trans.AssortmentStoreAttributeRid = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                }
                if (_trans.AllocationStoreAttributeID == Include.NoRID)
                {
                    _trans.AllocationStoreAttributeID = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                }
                int asrtCount = 0;
                int asrtRID = 0;  // TT#1542-MD - RMatelic Asst Workspace - select a basis date range and receive an Invalid Calendar Date Range Severe Error 
                foreach (AllocationHeaderProfile ahp in _headerList)
                {
                    if (ahp.HeaderType == eHeaderType.Assortment)
                    {
                        // Begin TT#1542-MD - RMatelic Asst Workspace - select a basis date range and receive an Invalid Calendar Date Range Severe Error 
                        if (asrtRID == 0)
                        {
                            asrtRID = ahp.Key;
                        }
                        // End TT#1542-MD
                        asrtCount++;
                        if (asrtCount > 1)
                        {
                            break;
                        }
                    }
                }
                // BEGIN TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
                _assormentselected = false;
                if (asrtRID < 1)
                {
                    _assormentselected = true;
                    CheckForSelectedMsg();
                    return;
                }
                // END TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
                // Begin TT#1542-MD - RMatelic Asst Workspace - select a basis date range and receive an Invalid Calendar Date Range Severe Error 
                if (asrtRID > 0)
                {
                    Header headerData = new Header();
                    DataTable dtAssortProperties = headerData.GetAssortmentProperties(asrtRID);
                    AnchorNodeRid = Convert.ToInt32(dtAssortProperties.Rows[0]["ANCHOR_HN_RID"], CultureInfo.CurrentUICulture);
                    AnchorDateRangeRid = Convert.ToInt32(dtAssortProperties.Rows[0]["CDR_RID"], CultureInfo.CurrentUICulture);
                    _trans.SetCriteriaHeaderList(_headerList);
                }
                // End TT#1542-MD
                if (_headerList.Count == 0 || viewType == eAllocationSelectionViewType.None || asrtCount > 1)
                {
                    CheckSecurityAndShow();
                }
                else
                {
					// BEGIN TT#209-MD - stodd - Assortment selection from workspace - 
					if (_trans.AssortmentBasisDataTable.Rows.Count == 0 && asrtCount > 1)
					// END TT#209-MD - stodd - Assortment selection from workspace - 
                    {
                        CheckSecurityAndShow();
                    }
                    else
                    {
                        AssortmentViewSelection_Load(this, args);
						
						//Begin TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
                        //_bypassScreen = true;
                        _trans.AssortmentViewSelectionBypass = true;
						//End TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
                        
						OK_Click(this, args);
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
			Cursor.Current = Cursors.Default;
		}

        private void CheckSecurityAndShow()
        {
            try
            {
                if (_assortmentReviewSecurity.AccessDenied)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_UnauthorizedFunctionAccess);
                    MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.Show();
                }
            }
            catch
            {
                throw;
            } 
        }
        // BEGIN TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
        private void CheckForSelectedMsg()
        {
            try
            {
                if (_assormentselected == true)
                {
                    string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_NoAssortmentSelected);
                    MessageBox.Show(errorMessage, "No Assortments Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.Show();
                }
            }
            catch
            {
                throw;
            }
        }
        // END TT#1985-MD - AGallagher - Select Review Selection and receive a Severe Error - Invalid Date Range
		private void BindGroupByComboBox()
		{

			cboGroupBy.DataSource = null;
			DataTable dtGroupBy = MIDEnvironment.CreateDataTable();
			eAllocationSelectionViewType viewType = eAllocationSelectionViewType.Assortment;
			dtGroupBy = MIDText.GetTextType((eMIDTextType)viewType, eMIDTextOrderBy.TextCode);

			cboGroupBy.Items.Clear();
			cboGroupBy.DataSource = dtGroupBy;
			cboGroupBy.DisplayMember = "TEXT_VALUE";
			cboGroupBy.ValueMember = "TEXT_CODE";
			if (_trans.AssortmentGroupBy == Include.UndefinedGroupByRID)
				cboGroupBy.SelectedIndex = 0;
			else
			{
				cboGroupBy.SelectedValue = _trans.AssortmentGroupBy;
				if (cboGroupBy.SelectedValue == null)
					cboGroupBy.SelectedIndex = 0;
			}
		}

		private void BindViewComboBox()
		{
            // hard-code for demo
            //_trans.AssortmentViewRid = 1;
            DataTable dtView;

            try
            {
                _bindingView = true;

                dtView = _assortmentViewData.AssortmentView_Read(_userRIDList, eAssortmentViewType.Assortment);	// TT#1077 - MD - stodd - cannot create GA views 

                _lastViewValue = -1;
                cboView.ValueMember = "VIEW_RID";
                cboView.DisplayMember = "VIEW_ID";
                cboView.DataSource = dtView;
                // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> unrelated to TT
                if (_trans.AssortmentViewRid == Include.NoRID)
                {
                    _trans.AssortmentViewRid = Include.DefaultAssortmentViewRID;
                }
                // End TT#1442
                cboView.SelectedValue = _trans.AssortmentViewRid;
                _bindingView = false;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
		}
        
        private void cboView_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int selectedValue;

            try
            {
                selectedValue = Convert.ToInt32(cboView.SelectedValue, CultureInfo.CurrentUICulture);

                if ((!_bindingView && selectedValue != _lastViewValue) ||
                    (_bindingView && selectedValue == _lastViewValue))
                {
                    _lastViewValue = selectedValue;
                    _trans.AssortmentViewRid = selectedValue;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboView_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboView_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		override protected bool ValidateSpecificFields()
		{
			bool FieldsValid = true;
            ErrorProvider.SetError(cboGroupBy, string.Empty);
            
            if (_headerList.Count == 0)
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                if (selectedHeaderList.Count == 0)
                {
                    FieldsValid = false;
                    string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_NoAssortmentSelected);
                    MessageBox.Show(message,this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!CreateHeaderListFromSelectedList(selectedHeaderList))
                {
                    FieldsValid = false;
                    string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_as_NoAssortmentSelected);
                    MessageBox.Show(message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
			return FieldsValid;
		}

        private bool CreateHeaderListFromSelectedList(SelectedHeaderList aSelectedHeaderList)
        {
            bool asrtFound = false;
            try
            {
                ArrayList selectedHeaderKeyList = new ArrayList();
                ArrayList selectedAssortmentKeyList = new ArrayList();  // TT#488 - MD - Jellis - Group ALlocation
                foreach (SelectedHeaderProfile shp in aSelectedHeaderList)
                {
                    if (shp.HeaderType == eHeaderType.Assortment || shp.HeaderType == eHeaderType.Placeholder ||
                        shp.AsrtRID > 0)
                    {
                        int asrtRID;
                        if (shp.HeaderType == eHeaderType.Assortment)
                        {
                            asrtRID = shp.Key;
                        }
                        else
                        {
                            asrtRID = shp.AsrtRID;
                        }
                        // begin TT#488 - MD - Jellis - Group Allocation
                        if (!selectedAssortmentKeyList.Contains(asrtRID))
                        {
                            selectedAssortmentKeyList.Add(asrtRID);
                            // end TT#488 - MD - Jellis - Group Allocation
                            ArrayList al = SAB.HeaderServerSession.GetHeadersInAssortment(asrtRID);
                            for (int i = 0; i < al.Count; i++)
                            {
                                int hdrRID = (int)al[i];
                                // begin TT#488 - MD - Jellis - Group Allocation
                                if (hdrRID != asrtRID)
                                {
                                    // end TT#488 - MD - Jellis - Group Allocation
                                    if (!selectedHeaderKeyList.Contains(hdrRID))
                                    {
                                        selectedHeaderKeyList.Add(hdrRID);
                                    }
                                } // TT#488- MD - Jellis - Group Allocation
                            }
                        } // TT#488 - MD - Jellis - Group Allocation
                        asrtFound = true;
                    }
                    else
                    {
                        selectedHeaderKeyList.Add(shp.Key);
                    }
                }
                if (asrtFound)
                {
                    int[] selectedHeaderArray = new int[selectedHeaderKeyList.Count];
                    selectedHeaderKeyList.CopyTo(selectedHeaderArray);
                    // begin TT#488 - MD - Jellis - Group Allocation
                    int[] selectedAssortmentArray = new int[selectedAssortmentKeyList.Count];
                    selectedAssortmentKeyList.CopyTo(selectedAssortmentArray);
                    // end TT#488 - MD - JEllis - Group Allocation

                    // load the selected headers in the Application session transaction
                    _trans.LoadHeaders(selectedAssortmentArray, selectedHeaderArray); // TT#488 - MD - Jellis - Group Allocation
                    _trans.CreateAssortmentViewSelectionCriteria();
                    _trans.CreateAllocationViewSelectionCriteria();
                    _trans.NewAssortmentCriteriaHeaderList();
                    _headerList = (AllocationHeaderProfileList)_trans.GetMasterProfileList(eProfileType.AllocationHeader);
                }
            }
            catch
            {
                throw;
            }
            return asrtFound;
        }

        override protected void SetSpecificFields()
        {
            try
            {
               //base.SetBase();
                _trans.AssortmentGroupBy = Convert.ToInt32(cboGroupBy.SelectedValue);
            }
            catch
            {
                throw;
            }
        }

		new protected void SetScreenText()
		{
			base.SetScreenText();

			this.lbGroupBy.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupBy);
			this.lbView.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_View);

		}

		override protected void SaveWindow()
		{
			try
			{
				_trans.SaveAssortmentDefaults();
			}
			catch
			{
				throw;
			}
		}

		//============================
		// Processing Assortment View
		//============================
		override protected bool Process()
		{
			try
			{
				bool doCommit = false;
				bool processed = false;

				ApplicationSessionTransaction appTransaction = SAB.ApplicationServerSession.CreateTransaction();

				//=====================================================
				// sets the assortment info in the Assortment profile
				//=====================================================
				//AssortmentProfile asp = (AssortmentProfile)aAssortmentProfile;

				// If an update connection is already open, then lets use it.
				// Otherwise, we open one...and we'll commit and close when we're done.
				//if (!_asp.HeaderDataRecord.ConnectionIsOpen)
				{
					//_asp.HeaderDataRecord.OpenUpdateConnection();
					doCommit = true;
				}
				try
				{
					//took out To compile - stodd
					//asp.SetAssortmentHeader(asp.HeaderRID, _reserveAmount, _reserveType, SG_RID, _variableType, _variableNumber,
					//                    _onHandInd, _intransitInd, _simStoreInd, _averageBy, this.Key, aApplicationTransaction.UserRid,
					//                    DateTime.Now);

					// This lists should already be correct from the Save();
					////============================================================
					//// sets the assortment store grades in the Assortment profile
					////============================================================
					//List<string> gradeCodeList = new List<string>();
					//List<double> boundaryList = new List<double>();
					//List<double> boundaryUnitsList = new List<double>();
					//foreach (DataRow aRow in _dtStoreGrades.Rows)
					//{
					//    gradeCodeList.Add(aRow["GRADE_CODE"].ToString());
					//    boundaryList.Add(Convert.ToDouble(aRow["BOUNDARY_INDEX"], CultureInfo.CurrentUICulture));
					//    boundaryUnitsList.Add(Convert.ToDouble(aRow["BOUNDARY_UNITS"], CultureInfo.CurrentUICulture));
					//}
					//_asp.SetAssortmentGrades(_asp.HeaderRID, gradeCodeList, boundaryList, boundaryUnitsList);

					//=====================================================
					// sets the assortment basis in the Assortment profile
					//=====================================================
					List<int> hierNodeList = new List<int>();
					List<int> versionList = new List<int>();
					List<int> dateRangeList = new List<int>();
					List<double> weightList = new List<double>();
					//foreach (DataRow aRow in _dtBasis.Rows)
					foreach (DataRow aRow in _trans.AssortmentBasisDataTable.Rows)
					{
						hierNodeList.Add(Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture));
						versionList.Add(Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture));
						dateRangeList.Add(Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture));
						weightList.Add(Convert.ToDouble(aRow["WEIGHT"], CultureInfo.CurrentUICulture));
					}
					//_asp.SetAssortmentBasis(_asp.HeaderRID, hierNodeList, versionList, dateRangeList, weightList);

					//StoreGroupProfile sgp = SAB.StoreServerSession.GetStoreGroupFilled(_asp.AssortmentStoreGroupRID);
					// aSummaryProfile = new AssortmentSummaryProfile(Include.NoRID, SAB, _trans, sgp, _asp.StoreGradeList);

					//============================================================
					// Calculates and sets the store bases values for the method
					//============================================================
					//DataTable dtTotalAssortment = aSummaryProfile.Process(hierNodeList, versionList, dateRangeList, weightList, _asp.AssortmentIncludeSimilarStores,
						//_asp.AssortmentIncludeIntransit, _asp.AssortmentIncludeOnhand, _asp.AssortmentIncludeCommitted, _asp.AssortmentAverageBy);

					//_asp.WriteAssortmentTotal(dtTotalAssortment);

					if (doCommit)
					{
					//	_asp.HeaderDataRecord.CommitData();
					}

					processed = true;

					//if (_changeType == eChangeType.add)
					{
						//AssortmentPropertiesChangeEventArgs eventArgs = new AssortmentPropertiesChangeEventArgs(_parentNode, _asp);
					//	if (OnAssortmentPropertiesChangeHandler != null)
						{
						//	OnAssortmentPropertiesChangeHandler(this, eventArgs);
						}
					}
				}
				finally
				{
					if (doCommit)
					{
						//_asp.HeaderDataRecord.CloseUpdateConnection();
					}
				}

				return processed;

			}
			catch
			{

				throw;
			}
		}


		override protected void NextWindow(eAllocationSelectionViewType viewType)
		{
			try
			{
				//if (EAB.AssortmentWorkspaceExplorer.GetSelectedHeaders().Count > 0)
				//{
				//    foreach (int key in _selectedHeaderKeyList)
				//    {
				//        if (key < 1)
				//        {
				//            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HeadersChanged));
				//            return;
				//        }
				//    }
				//}

				if (_trans == null)
				{
					// S/B Error
					return;
				}

				Cursor.Current = Cursors.WaitCursor;
				try
				{
					MIDFormBase frm = null;
					System.Windows.Forms.Form parentForm;
					System.ComponentModel.CancelEventArgs args = new CancelEventArgs();


					//if (!_trans.VelocityCriteriaExists)
					//{
					//    AllocationSubtotalProfileList subtotalList = (AllocationSubtotalProfileList)_trans.GetMasterProfileList(eProfileType.AllocationSubtotal);
					//    if (subtotalList != null)
					//    {
					//        foreach (AllocationSubtotalProfile asp in subtotalList)
					//        {
					//            asp.RemoveAllSubtotalMembers();
					//        }
					//        _trans.RemoveAllocationSubtotalProfileList();
					//    }

                    _trans.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.UserSelectionCriteria; //TT#2505 - DOConnell - Placeholder values changing when combining assortments

                    _trans.SetCriteriaHeaderList(_headerList);

                    // Begin TT#2136-MD - JSmith - Assortment Selection Error
                    ArrayList selectedHeaderRIDs = new ArrayList(); 
                    ArrayList selectedAssortmentRIDs = new ArrayList(); 
                    foreach (AllocationHeaderProfile ahp in _headerList)
                    {
                        if (ahp.HeaderType == eHeaderType.Assortment)
                        {
                            selectedAssortmentRIDs.Add(ahp.Key);
                        }
                        else
                        {
                            selectedHeaderRIDs.Add(ahp.Key);
                        }
                    }

                    int[] selectedHeaderArray = new int[selectedHeaderRIDs.Count];
                    int[] selectedAssortmentArray = new int[selectedAssortmentRIDs.Count];
                    selectedHeaderRIDs.CopyTo(selectedHeaderArray);
                    selectedAssortmentRIDs.CopyTo(selectedAssortmentArray);
                    _trans.CreateMasterAssortmentMemberListFromSelectedHeaders(selectedAssortmentArray, selectedHeaderArray);
                    // End TT#2136-MD - JSmith - Assortment Selection Error

                    if (CheckSecurityEnqueue(_trans, _headerList))
                    {
                        // Close this form

                        parentForm = this.MdiParent;
                        this.Close();
                        this.MdiParent = null;

                        try
                        {
                            _trans.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.UserSelectionCriteria;
                            frm = new MIDRetail.Windows.AssortmentView(EAB, _trans, eAssortmentWindowType.Assortment);
                            ((AssortmentView)frm).Initialize();
                            //((AssortmentView)frm).GroupName = _groupName;
                            frm.MdiParent = parentForm;
                            //frm.WindowState = FormWindowState.Maximized; // Begin TT#441-MD - RMatelic - Placeholder fields are not editable >>> unrelated; move maximize to after Show()
                            frm.Show();
                            frm.WindowState = FormWindowState.Maximized;   // End TT#441-MD
                            if (frm.ExceptionCaught)
                            {
                                frm.Close();
                                frm.MdiParent = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            frm.Close();
                            frm.MdiParent = null;
                            HandleException(ex, frm.Name);
                        }
                    }
				}
				catch (Exception ex)
				{
					HandleException(ex);
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

		public void btnOK_Click(object sender, System.EventArgs e)
		{
			OK_Click(sender, e);
		}

		private void OK_Click(object sender, System.EventArgs e)
		{
			try
			{
				MIDFormBase frm = null;
				System.Windows.Forms.Form parentForm;
				System.ComponentModel.CancelEventArgs args = new CancelEventArgs();
               
				if (_trans.DataState == eDataState.New || _trans.DataState == eDataState.Updatable)
				{
				//Begin TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
                    if (_trans.AssortmentViewSelectionBypass && !OKToProcess())
					//if (_bypassScreen && !OKToProcess())
				//End TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
					{
                        this.Close();
						this.MdiParent = null;
                       	return;
					}
				}
				
				//Begin TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
			     if (_trans.AssortmentViewSelectionBypass)
                //if (_bypassScreen)  // Ron Matelic - added if...
				//End TT#1962 - DOConnell - When selecting a single assortment from the assrt workspace, review screen is using user selection basis.
                {
                    _trans.SetCriteriaHeaderList(_headerList);
                    if (_trans.DataState == eDataState.New)
                    {
                        Save(eUpdateMode.Create);
                    }
                    else
                    {
                        Save(eUpdateMode.Update);
                    }
               
                    Cursor.Current = Cursors.WaitCursor;
                    // Close this form

                    parentForm = this.MdiParent;
                    this.Close();
                    this.MdiParent = null;

                    try
                    {
						// BEGIN TT#209-MD - stodd - Assortment selection from workspace - 
						if (_trans.AssortmentViewSelectionBypass)
						{
							_trans.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.AssortmentProperties;
						}
						else
						{
							_trans.AssortmentViewLoadedFrom = eAssortmentBasisLoadedFrom.UserSelectionCriteria;
						}
						// END TT#209-MD - stodd - Assortment selection from workspace - 
                        frm = new MIDRetail.Windows.AssortmentView(_eab, _trans, eAssortmentWindowType.Assortment);
                        ((AssortmentView)frm).Initialize();
                        //((AssortmentView)frm).GroupName = _groupName;
                        frm.MdiParent = parentForm;
                        //frm.WindowState = FormWindowState.Maximized; // Begin TT#441-MD - RMatelic - Placeholder fields are not editable >>> unrelated; move maximize to after Show()
                        frm.Show();
                        frm.WindowState = FormWindowState.Maximized;   // End TT#441-MD
                        if (frm.ExceptionCaught)
                        {
                            frm.Close();
                            frm.MdiParent = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        frm.Close();
                        frm.MdiParent = null;
                        HandleException(ex, frm.Name);
                    }
                    finally
                    {
                        Cursor.Current = Cursors.Default;
                    }
                }
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			//finally
			//{
			//	this.Close();
			//	this.MdiParent = null;
			//}
		}

		private bool OKToProcess()
		{
            try
            {
                return CheckSecurityEnqueue(_trans, _headerList);
            }
            catch
            {
                throw;
            }
		}
     
	}
}
