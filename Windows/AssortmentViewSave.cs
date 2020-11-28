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
	public partial class AssortmentViewSave : MIDFormBase
	{
		public delegate void AssortmentSaveClosingEventHandler(object source, bool aViewSaved, int aViewRID);
		public event AssortmentSaveClosingEventHandler OnAssortmentSaveClosingEventHandler;
        // Begin TT#2 Ron Matelic Assortment Planning
        public delegate void AssortmentSaveHeaderDataEventHandler(object source);
        public event AssortmentSaveHeaderDataEventHandler OnAssortmentSaveHeaderDataEventHandler;
        // End TT#2

		private SessionAddressBlock _sab;
		private AssortmentOpenParms _openParms;
		private AssortmentCubeGroup _assrtCubeGroup;
		private ArrayList _selectableComponentHeaders;
		private ArrayList _selectableSummaryHeaders;
		private ArrayList _selectableTotalHeaders;
		private ArrayList _selectableDetailColHeaders;
		private ArrayList _selectableDetailRowHeaders;
		private AssortmentViewData _assrtViewData;
		private AssortmentSaveParms _assrtSaveParms = null;
		// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
		private eAllocationAssortmentViewGroupBy _assrtGroupBy;
		// END TT#359-MD - stodd - add GroupBy to Asst View
        private eAssortmentViewType _assrtViewType;	// TT#1077 - MD - stodd - cannot create GA views 
        private int _assrtSgRid;	// TT#4247 - stodd - Store attribute not being saved in matrix view

        // Begin TT#857 - MD - stodd - assortment not honoring view
        public AssortmentSaveParms AssortmentSaveParms
        {
            get { return _assrtSaveParms; }
        }
        // End TT#857 - MD - stodd - assortment not honoring view

		// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
		public AssortmentViewSave(
			SessionAddressBlock aSAB,
			AssortmentOpenParms aOpenParms,
			AssortmentCubeGroup aAssrtCubeGroup,
			ArrayList aSelectableComponentHeaders,
			ArrayList aSelectableSummaryHeaders,
			ArrayList aSelectableTotalHeaders,
			ArrayList aSelectableDetailColHeaders,
			// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
			ArrayList aSelectableDetailRowHeaders,
			eAllocationAssortmentViewGroupBy aAsstGroupBy,
            int aAssrtSgRid,	// TT#4247 - stodd - Store attribute not being saved in matrix view
            eAssortmentViewType viewType)	// TT#1077 - MD - stodd - cannot create GA views 
			// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
			: base(aSAB)
		// END TT#359-MD - stodd - add GroupBy to Asst View
		{
			FunctionSecurityProfile functionSecurity;
			FunctionSecurityProfile userViewSecurity;
			FunctionSecurityProfile globalViewSecurity;

			InitializeComponent();

			_sab = aSAB;
			_openParms = aOpenParms;
			_assrtCubeGroup = aAssrtCubeGroup;
			_selectableComponentHeaders = aSelectableComponentHeaders;
			_selectableSummaryHeaders = aSelectableSummaryHeaders;
			_selectableTotalHeaders = aSelectableTotalHeaders;
			_selectableDetailColHeaders = aSelectableDetailColHeaders;
			_selectableDetailRowHeaders = aSelectableDetailRowHeaders;
			// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
			_assrtGroupBy = aAsstGroupBy;
			// END TT#359-MD - stodd - add GroupBy to Asst View
            _assrtSgRid = aAssrtSgRid;	// TT#4247 - stodd - Store attribute not being saved in matrix view
            _assrtViewType = viewType;	// TT#1077 - MD - stodd - cannot create GA views 

			_assrtViewData = new AssortmentViewData();

			// Load View Info

			txtViewName.Text = _openParms.ViewName;

			// Begin TT#1077 - MD - stodd - cannot create GA views 
            if (viewType == eAssortmentViewType.Assortment)
            {
                functionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentReview);
                // Begin TT#1995-MD - JSmith - Security - Assortment Views
                //userViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUserAssortmentReview);            // TT#1409-md - stodd - assortment view security wrong
                //globalViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobalAssortmentReview);        // TT#1409-md - stodd - assortment view security wrong
                userViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsUser);            
                globalViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AssortmentViewsGlobal);        
                // End TT#1995-MD - JSmith - Security - Assortment Views
                // Begin TT#1112 - MD -stodd - save box says 'assortment' - 
                this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment) + " Save";
                chkSaveAssortment.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Assortment) + " Values";
                // End TT#1112 - MD -stodd - save box says 'assortment' - 
            }
            else
            {
                functionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationReview);
                userViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationViewsUser);
                globalViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.GroupAllocationViewsGlobal);
                // Begin TT#1112 - MD -stodd - save box says 'assortment' - 
                this.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupAllocation) + " Save";
                chkSaveAssortment.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_GroupAllocation) + " Values";
                // End TT#1112 - MD -stodd - save box says 'assortment' - 
            }
			// End TT#1077 - MD - stodd - cannot create GA views 

			//BEGIN TT#4356 - DOConnell - Save View pop-up defaults to Save Group Allocation Values instead of the View
            //chkSaveAssortment.Checked = true;
            //chkView.Checked = false;
            //grpView.Enabled = false;

            chkSaveAssortment.Checked = false;
            chkView.Checked = true;
            grpView.Enabled = true;
			//END TT#4356 - DOConnell - Save View pop-up defaults to Save Group Allocation Values instead of the View

			if (userViewSecurity.AllowUpdate || globalViewSecurity.AllowUpdate)
			{
				chkView.Enabled = true;
			}
			else
			{
				chkView.Enabled = false;
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

			if (_openParms.ViewUserID != Include.GlobalUserRID)	// Issue 3806
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

            _assrtSaveParms = new AssortmentSaveParms();
            btnSave.Focus();
            // Begin TT#2014-MD - JSmith - Assortment Security 
            if (!userViewSecurity.AllowUpdate && !globalViewSecurity.AllowUpdate)
            {
                chkView.Checked = false;
                chkView.Enabled = false;
                txtViewName.Enabled = false;
            }
            // End TT#2014-MD - JSmith - Assortment Security 
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
            try
            {
                Cursor.Current = Cursors.WaitCursor;        // TT#3760 - stodd - group allocation view performance 
                //_assrtSaveParms = new AssortmentSaveParms();

				_assrtSaveParms.SaveAssortmentValues = chkSaveAssortment.Checked;
				_assrtSaveParms.SaveView = chkView.Checked;

                // Begin TT#892 - MD - stodd - error saving assortment
                // Reguardless of whether we're saving the view, we need to grab the view info (rid, name, userRid)
                // to save with the assortment for the next time it's opened by this user.
				//if (_assrtSaveParms.SaveView)
                _assrtSaveParms.ViewRID = Include.NoRID;
                if (txtViewName.Text != null && txtViewName.Text != string.Empty)
                // End TT#892 - MD - stodd - error saving assortment
				{
					_assrtSaveParms.ViewName = txtViewName.Text;
					if (rdoUser.Checked)
					{
						_assrtSaveParms.ViewUserRID = _sab.ClientServerSession.UserRID;
					}
					else
					{
						_assrtSaveParms.ViewUserRID = Include.GlobalUserRID;
					}
				}

				CheckForm();
				SaveValues();

                this.Close();
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
            // Begin TT#3760 - stodd - group allocation view performance 
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            // End TT#3760 - stodd - group allocation view performance 
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void chkView_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkView.Checked)
			{
				grpView.Enabled = true;
			}
			else
			{
				grpView.Enabled = false;
			}
		}

		private void CheckForm()
		{
			try
			{
				if (chkView.Checked)
				{
					if (txtViewName.Text.Trim() == "")
					{
						throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ViewNameMissing));
					}
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
				// Begin TT#857 - MD - stodd - assortment not honoring view
				// Moved to end.
                //if (_assrtSaveParms.SaveAssortmentValues)
                //{
                //    _assrtCubeGroup.SaveCubeGroup();
                //    OnAssortmentSaveHeaderDataEventHandler(this);
                //}
				// End TT#857 - MD - stodd - assortment not honoring view

				if (_assrtSaveParms.SaveView)
				{
					_assrtViewData.OpenUpdateConnection();

					try
					{
                        _assrtSaveParms.ViewRID = _assrtViewData.AssortmentView_GetKey(_assrtSaveParms.ViewUserRID, _assrtSaveParms.ViewName, _assrtViewType);	// TT#1456-MD - stodd - When saving views in Assortment with same name as a Group Allocation View, the Group Allocation view is overlaid.

						if (_assrtSaveParms.ViewRID != -1)
						{
							//BEGIN TT#594-MD - stodd - Saving Assortment "Default View" gets a foreign key constraint
							// Begin TT#4247 - stodd - Store attribute not being saved in matrix view
                            _assrtViewData.AssortmentView_Update(_assrtSaveParms.ViewRID, _assrtSaveParms.ViewUserRID, _assrtSaveParms.ViewName, _assrtGroupBy, _assrtSgRid, _assrtViewType);	// TT#1077 - MD - stodd - cannot create GA views 
							// End TT#4247 - stodd - Store attribute not being saved in matrix view
							_assrtViewData.AssortmentViewDetail_Delete(_assrtSaveParms.ViewRID);
							// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
							//_assrtViewData.AssortmentView_Delete(_assrtSaveParms.ViewRID);
							//_assrtSaveParms.ViewRID = _assrtViewData.AssortmentView_Insert(_assrtSaveParms.ViewUserRID, _assrtSaveParms.ViewName, _assrtGroupBy, _sab.ClientServerSession.UserRID);
							// END TT#359-MD - stodd - add GroupBy to Asst View
							//END TT#594-MD - stodd - Saving Assortment "Default View" gets a foreign key constraint
						}
						else
						{
							// BEGIN TT#359-MD - stodd - add GroupBy to Asst View
							// Begin TT#4247 - stodd - Store attribute not being saved in matrix view
                            _assrtSaveParms.ViewRID = _assrtViewData.AssortmentView_Insert(_assrtSaveParms.ViewUserRID, _assrtSaveParms.ViewName, _assrtGroupBy, _assrtSgRid, _sab.ClientServerSession.UserRID, _assrtViewType);		// TT#1077 - MD - stodd - cannot create GA views 
							// End TT#4247 - stodd - Store attribute not being saved in matrix view
							// END TT#359-MD - stodd - add GroupBy to Asst View
						}

						_assrtViewData.AssortmentViewDetail_Insert(
							_assrtSaveParms.ViewRID,
							_selectableComponentHeaders,
							_selectableSummaryHeaders,
							_selectableTotalHeaders,
							_selectableDetailColHeaders,
							_selectableDetailRowHeaders);
						_assrtViewData.CommitData();
					}
					catch (Exception exc)
					{
						_assrtViewData.Rollback();
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_assrtViewData.CloseUpdateConnection();
					}
				}

				// Begin TT#857 - MD - stodd - assortment not honoring view
                if (_assrtSaveParms.SaveAssortmentValues)
                {
                    _assrtCubeGroup.SaveCubeGroup();
                    _assrtCubeGroup.SaveBlockedStyles();	// TT#1488-MD - stodd - ASST - open and closing rows values returned inconsistent - 
                    OnAssortmentSaveHeaderDataEventHandler(this);
                }
				// End TT#857 - MD - stodd - assortment not honoring view
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void AssortmentViewSave_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// BEGIN TT#376-MD - stodd - Update Enqueue logic
			if (OnAssortmentSaveClosingEventHandler != null)
			{
				OnAssortmentSaveClosingEventHandler(this, _assrtSaveParms.SaveView, _assrtSaveParms.ViewRID);
			}
			// END TT#376-MD - stodd - Update Enqueue logic
		}
	}
}