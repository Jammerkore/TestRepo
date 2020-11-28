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
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for PlanViewSave.
	/// </summary>
	public partial class PlanViewSave : MIDFormBase
	{
		#region Variable Declarations

		//Begin Track #5690 - JScott - Can not save low to high
		//public delegate void PlanSaveClosingEventHandler(object source);
		public delegate void PlanSaveClosingEventHandler(object source, ePlanViewSaveResult closeResult);
		//End Track #5690 - JScott - Can not save low to high
		public event PlanSaveClosingEventHandler OnPlanSaveClosingEventHandler;

		private SessionAddressBlock _sab;
		private PlanOpenParms _openParms;
		private PlanCubeGroup _planCubeGroup;
		//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		//private ArrayList _selectableColHeaders;
		//private ArrayList _selectableRowHeaders;
		private ArrayList _selectableVariableHeaders;
		private ArrayList _selectableQuantityHeaders;
		private ArrayList _selectablePeriodHeaders;
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
		private PlanViewData _planViewData;
		private ProfileList _versionProfList;
		private ProfileList _chainFullAccessVersionProfList;
		private ProfileList _storeFullAccessVersionProfList;
		private PlanSaveParms _planSaveParms = null;

		private DateRangeProfile _strHighLvlTime;
		private DateRangeProfile _strLowLvlTime;
		private DateRangeProfile _chnHighLvlTime;
		private DateRangeProfile _chnLowLvlTime;
		//Begin Track #5690 - JScott - Can not save low to high
		private ePlanViewSaveResult _closeResult = ePlanViewSaveResult.Cancel;
		//End Track #5690 - JScott - Can not save low to high

		#endregion

		#region Constructor

        //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
		////Begin Track #5690 - JScott - Can not save low to high
		////public PlanViewSave(SessionAddressBlock aSAB, PlanOpenParms aOpenParms, PlanCubeGroup aPlanCubeGroup, ArrayList aSelectableColHeaders, ArrayList aSelectableRowHeaders)
		//public PlanViewSave(SessionAddressBlock aSAB, PlanOpenParms aOpenParms, PlanCubeGroup aPlanCubeGroup, ArrayList aSelectableColHeaders, ArrayList aSelectableRowHeaders, bool aSaveAs)
		////End Track #5690 - JScott - Can not save low to high
		public PlanViewSave(
			SessionAddressBlock aSAB,
			PlanOpenParms aOpenParms,
			PlanCubeGroup aPlanCubeGroup,
			ArrayList aSelectableVariableHeaders,
			ArrayList aSelectableQuantityHeaders,
            bool aSaveAs,
			ArrayList aSelectablePeriodHeaders)
		//End Track #5121 - JScott - Add Year/Season/Quarter totals
			: base(aSAB)
		{
			FunctionSecurityProfile functionSecurity;
			FunctionSecurityProfile userViewSecurity;
			FunctionSecurityProfile globalViewSecurity;

			InitializeComponent();

			this.drsStoreHighLevelTime.Enabled = true;
			this.drsStoreLowLevelTime.Enabled = true;
			this.drsChainHighLevelTime.Enabled = true;
			this.drsChainLowLevelTime.Enabled = true;

			_sab = aSAB;
			_openParms = aOpenParms;
			_planCubeGroup = aPlanCubeGroup;
			//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
			//_selectableColHeaders = aSelectableColHeaders;
			//_selectableRowHeaders = aSelectableRowHeaders;
			_selectableVariableHeaders = aSelectableVariableHeaders;
			_selectableQuantityHeaders = aSelectableQuantityHeaders;
			_selectablePeriodHeaders = aSelectablePeriodHeaders;
			//End Track #5121 - JScott - Add Year/Season/Quarter totals

			_planViewData = new PlanViewData();

			_versionProfList = _sab.ClientServerSession.GetUserForecastVersions();
			_chainFullAccessVersionProfList = new ProfileList(eProfileType.Version);

			foreach (VersionProfile versionProf in _versionProfList)
			{
				if (versionProf.ChainSecurity.AllowUpdate)
				{
					_chainFullAccessVersionProfList.Add(versionProf);
				}
			}

			if (_chainFullAccessVersionProfList.Count > 0)
			{
				BindChainHighLevelVersionComboBox();
				BindChainLowLevelVersionComboBox();
			}

			_storeFullAccessVersionProfList = new ProfileList(eProfileType.Version);

			foreach (VersionProfile versionProf in _versionProfList)
			{
				if (versionProf.StoreSecurity.AllowUpdate)
				{
					_storeFullAccessVersionProfList.Add(versionProf);
				}
			}

			if (_storeFullAccessVersionProfList.Count > 0)
			{
				BindStoreHighLevelVersionComboBox();
				BindStoreLowLevelVersionComboBox();
			}

			// Load Store High Level Info

			txtStoreHighLevelMerch.Text = _openParms.StoreHLPlanProfile.NodeProfile.Text;
			txtStoreHighLevelMerch.Tag = _openParms.StoreHLPlanProfile.NodeProfile.Key;
			cboStoreHighLevelVers.SelectedValue = _openParms.StoreHLPlanProfile.VersionProfile.Key;
			drsStoreHighLevelTime.Text = _openParms.DateRangeProfile.DisplayDate;
			drsStoreHighLevelTime.DateRangeRID = _openParms.DateRangeProfile.Key;

			// Load Store Low Level Info

			cboStoreLowLevelVers.SelectedValue = _openParms.LowLevelVersionDefault.Key;
			drsStoreLowLevelTime.Text = _openParms.DateRangeProfile.DisplayDate;
			drsStoreLowLevelTime.DateRangeRID = _openParms.DateRangeProfile.Key;

			// Load Chain High Level Info

			txtChainHighLevelMerch.Text = _openParms.ChainHLPlanProfile.NodeProfile.Text;
			txtChainHighLevelMerch.Tag = _openParms.ChainHLPlanProfile.NodeProfile.Key;
			cboChainHighLevelVers.SelectedValue = _openParms.ChainHLPlanProfile.VersionProfile.Key;
			drsChainHighLevelTime.Text = _openParms.DateRangeProfile.DisplayDate;
			drsChainHighLevelTime.DateRangeRID = _openParms.DateRangeProfile.Key;

			// Load Chain Low Level Info

			cboChainLowLevelVers.SelectedValue = _openParms.LowLevelVersionDefault.Key;
			drsChainLowLevelTime.Text = _openParms.DateRangeProfile.DisplayDate;
			drsChainLowLevelTime.DateRangeRID = _openParms.DateRangeProfile.Key;

			// Load View Info

			txtViewName.Text = _openParms.ViewName;

			functionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastReview);

			switch (_openParms.PlanSessionType)
			{
				case ePlanSessionType.StoreSingleLevel :

					pnlStore.Visible = true;
					pnlChain.Visible = true;
					pnlStoreHighLevel.Visible = true;
					pnlStoreLowLevel.Visible = false;
					pnlChainHighLevel.Visible = true;
					pnlChainLowLevel.Visible = false;
					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					chkChainHighLevelAllStore.Visible = true;
					chkSaveLowToHighLevel.Visible = false;
					////End Enhancement - JScott - Add Balance Low Levels functionality

					grpStoreHighLevel.Text = "Store Plan";
					grpChainHighLevel.Text = "Chain Plan";

					if (functionSecurity.IsReadOnly || _storeFullAccessVersionProfList.Count == 0)
					{
						chkStoreHighLevel.Checked = false;
						chkStoreHighLevel.Enabled = false;
						grpStoreHighLevel.Enabled = false;
					}
					else
					{
						chkStoreHighLevel.Checked = true;
						grpStoreHighLevel.Enabled = true;
					}

					if (functionSecurity.IsReadOnly || _chainFullAccessVersionProfList.Count == 0)
					{
						chkChainHighLevel.Checked = false;
						chkChainHighLevel.Enabled = false;
						grpChainHighLevel.Enabled = false;
					}
					else
					{
						chkChainHighLevel.Checked = true;
						grpChainHighLevel.Enabled = true;
					}

					chkView.Checked = false;
					grpView.Enabled = false;

					//Begin Track #5690 - JScott - Can not save low to high
					if (aSaveAs)
					{
						txtChainHighLevelMerch.Enabled = true;
						cboChainHighLevelVers.Enabled = true;
						drsChainHighLevelTime.Enabled = true;
						chkChainHighLevelAllStore.Enabled = true;

						txtStoreHighLevelMerch.Enabled = true;
						cboStoreHighLevelVers.Enabled = true;
						drsStoreHighLevelTime.Enabled = true;
					}
					else
					{
						txtChainHighLevelMerch.Enabled = false;
						cboChainHighLevelVers.Enabled = false;
						drsChainHighLevelTime.Enabled = false;
						chkChainHighLevelAllStore.Enabled = false;

						txtStoreHighLevelMerch.Enabled = false;
						cboStoreHighLevelVers.Enabled = false;
						drsStoreHighLevelTime.Enabled = false;
					}

					//End Track #5690 - JScott - Can not save low to high
					this.Size = new Size(336, 416);

					break;

				case ePlanSessionType.StoreMultiLevel :

					pnlStore.Visible = true;
					pnlChain.Visible = true;
					pnlStoreHighLevel.Visible = true;
					pnlStoreLowLevel.Visible = true;
					pnlChainHighLevel.Visible = true;
					pnlChainLowLevel.Visible = true;
					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					chkChainHighLevelAllStore.Visible = true;
					chkSaveLowToHighLevel.Visible = false;
					////End Enhancement - JScott - Add Balance Low Levels functionality

					grpStoreHighLevel.Text = "Store High-level Plan";
					grpChainHighLevel.Text = "Chain High-level Plan";

					if (functionSecurity.IsReadOnly || _storeFullAccessVersionProfList.Count == 0)
					{
						chkStoreHighLevel.Checked = false;
						chkStoreHighLevel.Enabled = false;
						grpStoreHighLevel.Enabled = false;
						chkStoreLowLevel.Checked = false;
						chkStoreLowLevel.Enabled = false;
						grpStoreLowLevel.Enabled = false;
					}
					else
					{
						chkStoreHighLevel.Checked = true;
						grpStoreHighLevel.Enabled = true;
						chkStoreLowLevel.Checked = true;
						grpStoreLowLevel.Enabled = true;
					}

					if (functionSecurity.IsReadOnly || _chainFullAccessVersionProfList.Count == 0)
					{
						chkChainHighLevel.Checked = false;
						chkChainHighLevel.Enabled = false;
						grpChainHighLevel.Enabled = false;
						chkChainLowLevel.Checked = false;
						chkChainLowLevel.Enabled = false;
						grpChainLowLevel.Enabled = false;
					}
					else
					{
						chkChainHighLevel.Checked = true;
						grpChainHighLevel.Enabled = true;
						chkChainLowLevel.Checked = true;
						grpChainLowLevel.Enabled = true;
					}

					chkView.Checked = false;
					grpView.Enabled = false;

					chkStoreLowLevelOverride.Checked = false;
					pnlStoreLowLevelOverride.Enabled = false;
					chkChainLowLevelOverride.Checked = false;
					pnlChainLowLevelOverride.Enabled = false;

					//Begin Track #5690 - JScott - Can not save low to high
					if (aSaveAs)
					{
						txtStoreHighLevelMerch.Enabled = true;
						cboStoreHighLevelVers.Enabled = true;
						drsStoreHighLevelTime.Enabled = true;
						chkStoreLowLevelOverride.Enabled = true;

						txtChainHighLevelMerch.Enabled = true;
						cboChainHighLevelVers.Enabled = true;
						drsChainHighLevelTime.Enabled = true;
						chkChainHighLevelAllStore.Enabled = true;
						chkChainLowLevelOverride.Enabled = true;
						chkChainLowLevelAllStore.Enabled = true;
					}
					else
					{
						txtStoreHighLevelMerch.Enabled = false;
						cboStoreHighLevelVers.Enabled = false;
						drsStoreHighLevelTime.Enabled = false;
						chkStoreLowLevelOverride.Enabled = false;

						txtChainHighLevelMerch.Enabled = false;
						cboChainHighLevelVers.Enabled = false;
						drsChainHighLevelTime.Enabled = false;
						chkChainHighLevelAllStore.Enabled = false;
						chkChainLowLevelOverride.Enabled = false;
						chkChainLowLevelAllStore.Enabled = false;
					}

					//End Track #5690 - JScott - Can not save low to high
					this.Size = new Size(664, 416);

					break;

				case ePlanSessionType.ChainSingleLevel :

					pnlStore.Visible = false;
					pnlChain.Visible = true;
					pnlStoreHighLevel.Visible = false;
					pnlStoreLowLevel.Visible = false;
					pnlChainHighLevel.Visible = true;
					pnlChainLowLevel.Visible = false;
					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					chkChainHighLevelAllStore.Visible = false;
					chkSaveLowToHighLevel.Visible = true;
					////End Enhancement - JScott - Add Balance Low Levels functionality

					grpChainHighLevel.Text = "Chain Plan";

					if (functionSecurity.IsReadOnly || _chainFullAccessVersionProfList.Count == 0)
					{
						chkChainHighLevel.Checked = false;
						chkChainHighLevel.Enabled = false;
						grpChainHighLevel.Enabled = false;
					}
					else
					{
						chkChainHighLevel.Checked = true;
						grpChainHighLevel.Enabled = true;
					}

					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					//chkChainHighLevelAllStore.Checked = false;
					//chkChainHighLevelAllStore.Enabled = false;
					chkSaveLowToHighLevel.Checked = false;
					chkSaveLowToHighLevel.Enabled = false;
					////End Enhancement - JScott - Add Balance Low Levels functionality

					chkView.Checked = false;
					grpView.Enabled = false;

					//Begin Track #5690 - JScott - Can not save low to high
					if (aSaveAs)
					{
						txtChainHighLevelMerch.Enabled = true;
						cboChainHighLevelVers.Enabled = true;
						drsChainHighLevelTime.Enabled = true;
					}
					else
					{
						txtChainHighLevelMerch.Enabled = false;
						cboChainHighLevelVers.Enabled = false;
						drsChainHighLevelTime.Enabled = false;
					}

					//End Track #5690 - JScott - Can not save low to high
					this.Size = new Size(336, 296);

					break;

				case ePlanSessionType.ChainMultiLevel :

					pnlStore.Visible = false;
					pnlChain.Visible = true;
					pnlStoreHighLevel.Visible = false;
					pnlStoreLowLevel.Visible = false;
					pnlChainHighLevel.Visible = true;
					pnlChainLowLevel.Visible = true;
					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					chkChainHighLevelAllStore.Visible = false;
					chkSaveLowToHighLevel.Visible = true;
					////End Enhancement - JScott - Add Balance Low Levels functionality

					grpChainHighLevel.Text = "Chain High-level Plan";

					if (functionSecurity.IsReadOnly || _chainFullAccessVersionProfList.Count == 0)
					{
						chkChainHighLevel.Checked = false;
						chkChainHighLevel.Enabled = false;
						grpChainHighLevel.Enabled = false;
						chkChainLowLevel.Checked = false;
						chkChainLowLevel.Enabled = false;
						grpChainLowLevel.Enabled = false;
					}
					else
					{
						chkChainHighLevel.Checked = true;
						grpChainHighLevel.Enabled = true;
						chkChainLowLevel.Checked = true;
						grpChainLowLevel.Enabled = true;
					}

					////Begin Enhancement - JScott - Add Balance Low Levels functionality
					//chkChainHighLevelAllStore.Checked = false;
					//chkChainHighLevelAllStore.Enabled = false;
					////End Enhancement - JScott - Add Balance Low Levels functionality
					chkChainLowLevelAllStore.Checked = false;
					chkChainLowLevelAllStore.Enabled = false;

					chkView.Checked = false;
					grpView.Enabled = false;

					chkChainLowLevelOverride.Checked = false;
					pnlChainLowLevelOverride.Enabled = false;

					//Begin Track #5690 - JScott - Can not save low to high
					if (aSaveAs)
					{
						txtChainHighLevelMerch.Enabled = true;
						cboChainHighLevelVers.Enabled = true;
						drsChainHighLevelTime.Enabled = true;
						chkChainLowLevelOverride.Enabled = true;
						chkSaveLowToHighLevel.Enabled = true;
					}
					else
					{
						txtChainHighLevelMerch.Enabled = false;
						cboChainHighLevelVers.Enabled = false;
						drsChainHighLevelTime.Enabled = false;
						chkChainLowLevelOverride.Enabled = false;
						chkSaveLowToHighLevel.Enabled = false;
					}

					//End Track #5690 - JScott - Can not save low to high
					this.Size = new Size(664, 296);

					break;
			}

			userViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
			globalViewSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);

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

			btnSave.Focus();
		}

		#endregion

		private DateRangeProfile StrHighLvlTime
		{
			get
			{
				if (_strHighLvlTime == null)
				{
					_strHighLvlTime = SAB.ClientServerSession.Calendar.GetDateRangeClone(_openParms.DateRangeProfile.Key);
				}

				return _strHighLvlTime;
			}
		}

		private DateRangeProfile StrLowLvlTime
		{
			get
			{
				if (_strLowLvlTime == null)
				{
					_strLowLvlTime = SAB.ClientServerSession.Calendar.GetDateRangeClone(_openParms.DateRangeProfile.Key);
				}

				return _strLowLvlTime;
			}
		}

		private DateRangeProfile ChnHighLvlTime
		{
			get
			{
				if (_chnHighLvlTime == null)
				{
					_chnHighLvlTime = SAB.ClientServerSession.Calendar.GetDateRangeClone(_openParms.DateRangeProfile.Key);
				}

				return _chnHighLvlTime;
			}
		}

		private DateRangeProfile ChnLowLvlTime
		{
			get
			{
				if (_chnLowLvlTime == null)
				{
					_chnLowLvlTime = SAB.ClientServerSession.Calendar.GetDateRangeClone(_openParms.DateRangeProfile.Key);
				}

				return _chnLowLvlTime;
			}
		}

        //Begin TT#609-MD -jsobek -OTS Forecast Chain Ladder View
        public void DisableControlsForSave()
        {
            drsChainHighLevelTime.Enabled = false;
            txtChainHighLevelMerch.Enabled = false;
            cboChainHighLevelVers.Enabled = false;
            cboChainLowLevelVers.Enabled = false;
        }
        //End TT#609-MD -jsobek -OTS Forecast Chain Ladder View

		#region ComboBox Binding and Selection

		private void BindChainHighLevelVersionComboBox()
		{
			cboChainHighLevelVers.DataSource = (ArrayList)_chainFullAccessVersionProfList.ArrayList.Clone();
			cboChainHighLevelVers.DisplayMember = "Description";
			cboChainHighLevelVers.ValueMember = "Key";		
		}

		private void BindChainLowLevelVersionComboBox()
		{
			cboChainLowLevelVers.DataSource = (ArrayList)_chainFullAccessVersionProfList.ArrayList.Clone();
			cboChainLowLevelVers.DisplayMember = "Description";
			cboChainLowLevelVers.ValueMember = "Key";		
		}

		private void BindStoreHighLevelVersionComboBox()
		{
			cboStoreHighLevelVers.DataSource = (ArrayList)_storeFullAccessVersionProfList.ArrayList.Clone();
			cboStoreHighLevelVers.DisplayMember = "Description";
			cboStoreHighLevelVers.ValueMember = "Key";		
		}

		private void BindStoreLowLevelVersionComboBox()
		{
			cboStoreLowLevelVers.DataSource = (ArrayList)_storeFullAccessVersionProfList.ArrayList.Clone();
			cboStoreLowLevelVers.DisplayMember = "Description";
			cboStoreLowLevelVers.ValueMember = "Key";		
		}

		#endregion




		#region Events

		#region Button Events

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				_planSaveParms = new PlanSaveParms();

				_planSaveParms.SaveStoreHighLevel = chkStoreHighLevel.Checked;
				_planSaveParms.SaveStoreLowLevel = chkStoreLowLevel.Checked;
				_planSaveParms.SaveChainHighLevel = chkChainHighLevel.Checked;
				_planSaveParms.SaveChainLowLevel = chkChainLowLevel.Checked;
				_planSaveParms.SaveView = chkView.Checked;
				
				if (_planSaveParms.SaveStoreHighLevel)
				{
					_planSaveParms.StoreHighLevelNodeRID = Convert.ToInt32(txtStoreHighLevelMerch.Tag, CultureInfo.CurrentUICulture);
					_planSaveParms.StoreHighLevelVersionRID = Convert.ToInt32(cboStoreHighLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
					_planSaveParms.StoreHighLevelDateRangeRID = drsStoreHighLevelTime.DateRangeRID;
				}

				if (_planSaveParms.SaveStoreLowLevel)
				{
					if (chkStoreLowLevelOverride.Checked)
					{
						_planSaveParms.OverrideStoreLowLevel = true;
						_planSaveParms.StoreLowLevelVersionRID = Convert.ToInt32(cboStoreLowLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
						_planSaveParms.StoreLowLevelDateRangeRID = drsStoreLowLevelTime.DateRangeRID;
					}
				}

				if (_planSaveParms.SaveChainHighLevel)
				{
					_planSaveParms.ChainHighLevelNodeRID = Convert.ToInt32(txtChainHighLevelMerch.Tag, CultureInfo.CurrentUICulture);
					_planSaveParms.ChainHighLevelVersionRID = Convert.ToInt32(cboChainHighLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
					_planSaveParms.ChainHighLevelDateRangeRID = drsChainHighLevelTime.DateRangeRID;
					_planSaveParms.SaveHighLevelAllStoreAsChain = chkChainHighLevelAllStore.Checked;
					//Begin Enhancement - JScott - Add Balance Low Levels functionality
					_planSaveParms.SaveLowLevelChainAsChain = chkSaveLowToHighLevel.Checked;
					//End Enhancement - JScott - Add Balance Low Levels functionality
				}

				if (_planSaveParms.SaveChainLowLevel)
				{
					_planSaveParms.SaveLowLevelAllStoreAsChain = chkChainLowLevelAllStore.Checked;

					if (chkChainLowLevelOverride.Checked)
					{
						_planSaveParms.OverrideChainLowLevel = true;
						_planSaveParms.ChainLowLevelVersionRID = Convert.ToInt32(cboChainLowLevelVers.SelectedValue, CultureInfo.CurrentUICulture);
						_planSaveParms.ChainLowLevelDateRangeRID = drsChainLowLevelTime.DateRangeRID;
					}
				}

				if (_planSaveParms.SaveView)
				{
					_planSaveParms.ViewName = txtViewName.Text;
					if (rdoUser.Checked)
					{
						_planSaveParms.ViewUserRID = _sab.ClientServerSession.UserRID;
					}
					else
					{
						_planSaveParms.ViewUserRID = Include.GlobalUserRID;	// Issue 3806
					}
				}

				CheckForm();
				SaveValues();

				//Begin Track #5950 - JScott - Save Low Level to High may get warning message
				////Begin Track #5690 - JScott - Can not save low to high
				//_closeResult = ePlanViewSaveResult.Save;
				////End Track #5690 - JScott - Can not save low to high
				if (!_planSaveParms.SaveStoreHighLevel &&
					!_planSaveParms.SaveStoreLowLevel &&
					!_planSaveParms.SaveChainHighLevel &&
					!_planSaveParms.SaveChainLowLevel)
				{
					_closeResult = ePlanViewSaveResult.SaveViewOnly;

				}
				else
				{
					_closeResult = ePlanViewSaveResult.Save;
				}
				//End Track #5950 - JScott - Save Low Level to High may get warning message

				this.Close();
			}
			catch (PlanInUseException)
			{
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
			//Begin Track #5690 - JScott - Can not save low to high
			_closeResult = ePlanViewSaveResult.Cancel;

			//End Track #5690 - JScott - Can not save low to high
			this.Close();
		}

		#endregion

		#region TextBox Events

		private void txtMerch_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			IDataObject data;
            TreeNodeClipboardList cbList;
            TreeNodeClipboardProfile cbProf;
			HierarchyNodeProfile hnp;
            //HierarchyClipboardData cbd;

			try
			{
				// Create a new instance of the DataObject interface.
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //Begin Track #5875 - JScott - Dropping Color node into Plan View Save does not show full description
                    //hnp = _sab.HierarchyServerSession.GetNodeData(cbp.Key);
                    hnp = _sab.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                    //End Track #5875 - JScott - Dropping Color node into Plan View Save does not show full description
                    ((System.Windows.Forms.TextBox)sender).Text = hnp.Text;
                    ((System.Windows.Forms.TextBox)sender).Tag = cbList.ClipboardProfile.Key;
                }
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void txtMerch_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            //IDataObject data;
            TreeNodeClipboardList cbList;

            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
			{
				Image_DragEnter(sender, e);
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                //if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
				{
                    //if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                    //{
						e.Effect = DragDropEffects.All;
                    //}
                    //else
                    //{
                    //    e.Effect = DragDropEffects.None;
                    //}
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

		private void txtMerch_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		#endregion

		#region MIDDateRangeSelector Events

		private void drsStoreHighLevelTime_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile _dateRange = e.SelectedDateRange;

			if (_dateRange != null)
			{
				drsStoreHighLevelTime.DateRangeRID = _dateRange.Key;
			}
		}

		private void drsStoreHighLevelTime_Click(object sender, System.EventArgs e)
		{
			CalendarDateSelector cds;

			cds = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
			// BEGIN Issue 4443 - stodd 06.12.2007	
			cds.AllowDynamicToPlan = false;
			cds.AllowDynamicToStoreOpen = false;
			cds.DateRangeRID = StrHighLvlTime.Key;
			// END Issue 4443

			drsStoreHighLevelTime.DateRangeForm = cds;
			drsStoreHighLevelTime.ShowSelector();
			cds.Dispose();
		}

		private void drsStoreLowLevelTime_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile _dateRange = e.SelectedDateRange;

			if (_dateRange != null)
			{
				drsStoreLowLevelTime.DateRangeRID = _dateRange.Key;
			}
		}

		private void drsStoreLowLevelTime_Click(object sender, System.EventArgs e)
		{
			CalendarDateSelector cds;

			cds = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
			// BEGIN Issue 4443 - stodd 06.12.2007	
			cds.AllowDynamicToPlan = false;
			cds.AllowDynamicToStoreOpen = false;
			cds.DateRangeRID = StrLowLvlTime.Key;
			// END Issue 4443

			drsStoreLowLevelTime.DateRangeForm = cds;
			drsStoreLowLevelTime.ShowSelector();
			cds.Dispose();
		}

		private void drsChainHighLevelTime_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile _dateRange = e.SelectedDateRange;

			if (_dateRange != null)
			{
				drsChainHighLevelTime.DateRangeRID = _dateRange.Key;
			}
		}

		private void drsChainHighLevelTime_Click(object sender, System.EventArgs e)
		{
			CalendarDateSelector cds;

			cds = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
			// BEGIN Issue 4443 - stodd 06.12.2007	
			cds.AllowDynamicToPlan = false;
			cds.AllowDynamicToStoreOpen = false;
			cds.DateRangeRID = ChnHighLvlTime.Key;
			// END Issue 4443

			drsChainHighLevelTime.DateRangeForm = cds;
			drsChainHighLevelTime.ShowSelector();
			cds.Dispose();
		}

		private void drsChainLowLevelTime_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			DateRangeProfile _dateRange = e.SelectedDateRange;

			if (_dateRange != null)
			{
				drsChainLowLevelTime.DateRangeRID = _dateRange.Key;
			}
		}

		private void drsChainLowLevelTime_Click(object sender, System.EventArgs e)
		{
			CalendarDateSelector cds;

			cds = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{_sab});
			// BEGIN Issue 4443 - stodd 06.12.2007	
			cds.AllowDynamicToPlan = false;
			cds.AllowDynamicToStoreOpen = false;
			cds.DateRangeRID = ChnLowLvlTime.Key;
			// END Issue 4443

			drsChainLowLevelTime.DateRangeForm = cds;
			drsChainLowLevelTime.ShowSelector();
			cds.Dispose();
		}

		#endregion

		#region CheckBox Events

		private void chkStoreHighLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkStoreHighLevel.Checked)
			{
				grpStoreHighLevel.Enabled = true;
			}
			else
			{
				grpStoreHighLevel.Enabled = false;
			}
		}

		private void chkStoreLowLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkStoreLowLevel.Checked)
			{
				grpStoreLowLevel.Enabled = true;
			}
			else
			{
				grpStoreLowLevel.Enabled = false;
			}
		}

		private void chkChainHighLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkChainHighLevel.Checked)
			{
				grpChainHighLevel.Enabled = true;
			}
			else
			{
				grpChainHighLevel.Enabled = false;
			}
		}

		private void chkChainLowLevel_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkChainLowLevel.Checked)
			{
				grpChainLowLevel.Enabled = true;
			}
			else
			{
				grpChainLowLevel.Enabled = false;
			}
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

		private void chkStoreLowLevelOverride_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkStoreLowLevelOverride.Checked)
			{
				pnlStoreLowLevelOverride.Enabled = true;
			}
			else
			{
				pnlStoreLowLevelOverride.Enabled = false;
			}
		}

		private void chkChainLowLevelOverride_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkChainLowLevelOverride.Checked)
			{
				pnlChainLowLevelOverride.Enabled = true;
			}
			else
			{
				pnlChainLowLevelOverride.Enabled = false;
			}
		}

		#endregion

		#endregion

		#region Methods

		private void CheckForm()
		{
			HierarchyNodeSecurityProfile hierNodeSecProf;

			try
			{
				if (chkStoreHighLevel.Checked)
				{
					if (txtStoreHighLevelMerch.Text == "")
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHierarchyNodeMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHighLevelHierarchyNodeMissing));
						}
					}

					hierNodeSecProf = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(Convert.ToInt32(txtStoreHighLevelMerch.Tag), (int)eSecurityTypes.Store);

					if (!hierNodeSecProf.AllowUpdate)
					{
						throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreNodeNotAuthorized));
					}

					if (cboStoreHighLevelVers.SelectedIndex == -1)
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreVersionMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHighLevelVersionMissing));
						}
					}

					if (drsStoreHighLevelTime.Text.Trim() == "")
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreDateMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreHighLevelDateMissing));
						}
					}
				}

				if (chkStoreLowLevel.Checked)
				{
					if (chkStoreLowLevelOverride.Checked)
					{
						if (cboStoreLowLevelVers.SelectedIndex == -1)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreLowLevelVersionMissing));
						}

						if (drsStoreLowLevelTime.Text.Trim() == "")
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_StoreLowLevelDateMissing));
						}
					}
				}

				if (chkChainHighLevel.Checked)
				{
					if (txtChainHighLevelMerch.Text == "")
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHierarchyNodeMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHighLevelHierarchyNodeMissing));
						}
					}

					hierNodeSecProf = _sab.ClientServerSession.GetMyUserNodeSecurityAssignment(Convert.ToInt32(txtChainHighLevelMerch.Tag), (int)eSecurityTypes.Chain);

					if (!hierNodeSecProf.AllowUpdate)
					{
						throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainNodeNotAuthorized));
					}

					if (cboChainHighLevelVers.SelectedIndex == -1)
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainVersionMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHighLevelVersionMissing));
						}
					}

					if (drsChainHighLevelTime.Text.Trim() == "")
					{
						if (_openParms.PlanSessionType == ePlanSessionType.StoreSingleLevel)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainDateMissing));
						}
						else
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainHighLevelDateMissing));
						}
					}
				}

				if (chkChainLowLevel.Checked)
				{
					if (chkChainLowLevelOverride.Checked)
					{
						if (cboChainLowLevelVers.SelectedIndex == -1)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainLowLevelVersionMissing));
						}

						if (drsChainLowLevelTime.Text.Trim() == "")
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_ChainLowLevelDateMissing));
						}
					}
				}

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
			int viewRID;

			try
			{
				_planCubeGroup.SaveCubeGroup(_planSaveParms);

				if (_planSaveParms.SaveView)
				{
					_planViewData.OpenUpdateConnection();

					try
					{
						viewRID = _planViewData.PlanView_GetKey(_planSaveParms.ViewUserRID, _planSaveParms.ViewName);

						if (viewRID != -1)
						{
							_planViewData.PlanViewDetail_Delete(viewRID);
						}
						else
						{
							viewRID = _planViewData.PlanView_Insert(_planSaveParms.ViewUserRID, _planSaveParms.ViewName, eStorePlanSelectedGroupBy.ByTimePeriod);
						}

						//Begin Track #5121 - JScott - Add Year/Season/Quarter totals
						//_planViewData.PlanViewDetail_Insert(viewRID, _selectableColHeaders, _selectableRowHeaders);
						_planViewData.PlanViewDetail_Insert(viewRID, _selectableVariableHeaders, _selectableQuantityHeaders, _selectablePeriodHeaders);
						//End Track #5121 - JScott - Add Year/Season/Quarter totals
						_planViewData.CommitData();
					}
					catch (Exception exc)
					{
						_planViewData.Rollback();
						string message = exc.ToString();
						throw;
					}
					finally
					{
						_planViewData.CloseUpdateConnection();
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#endregion

		private void PlanViewSave_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			//Begin Track #5690 - JScott - Can not save low to high
			//OnPlanSaveClosingEventHandler(this);
			OnPlanSaveClosingEventHandler(this, _closeResult);
			//End Track #5690 - JScott - Can not save low to high
		}

    }

	#region Class/Structure Declarations

	#endregion
}
