using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Windows.Forms;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmPlanningExtractMethod.
	/// </summary>
	public partial class frmPlanningExtractMethod : WorkflowMethodFormBase
	{
		private RowColChooserOrderPanel _chainVarChooser;
		private RowColChooserOrderPanel _storeVarChooser;
        private RowColChooserOrderPanel _chainTotalVarChooser;
        private RowColChooserOrderPanel _storeTotalVarChooser;
        private FilterData _storeFilterDL;
		private ProfileList _masterVersionProfList;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		private ProfileList _currentVersionProfList;
		private ArrayList _chainSelectableVariableList;
		private ArrayList _storeSelectableVariableList;
        private ArrayList _chainSelectableTotalVariableList;
        private ArrayList _storeSelectableTotalVariableList;
        private ProfileList _chainLowLevelVersionList;
		private ProfileList _storeLowLevelVersionList;
		private bool _loadingPlanningExtractOptions = false;

		private MIDWorkflowMethodTreeNode _explorerNode = null;
		private OTSForecastPlanningExtractMethod _planningExtractMethod = null;
		private int _nodeRID = -1;
		private bool _textChanged = false;
		private bool _priorError = false;
		private DateRangeProfile _drTimePeriod = null;
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
        private ApplicationSessionTransaction _transaction;
		
		//=============
		// CONSTRUCTORS
		//=============

		public frmPlanningExtractMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
			: base (SAB, aEAB, eMIDTextCode.frm_PlanningExtractMethod, eWorkflowMethodType.Method)
		{
			InitializeComponent();
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserPlanningExtract);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalPlanningExtract);
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Opens a new Planning Extract Method.
		/// </summary>

		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_explorerNode = aParentNode;

				_planningExtractMethod = new OTSForecastPlanningExtractMethod(SAB, Include.NoRID);
				
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserPlanningExtract, eSecurityFunctions.ForecastMethodsGlobalPlanningExtract);
				
				Common_Load(aParentNode.GlobalUserType);				 

			}
			catch (Exception err)
			{
				HandleException(err, "NewWorkflowMethod Method");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Opens an existing Planning Extract Method.
		/// </summary>
		/// <param name="aMethodRID">
		/// aMethodRID
		/// </param>
		/// <param name="aLockStatus">
		/// The lock status of the data to be displayed
		/// </param>

		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{       
				_explorerNode = aNode;

				_nodeRID = aNodeRID;
				_planningExtractMethod = new OTSForecastPlanningExtractMethod(SAB, aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserPlanningExtract, eSecurityFunctions.ForecastMethodsGlobalPlanningExtract);

				Common_Load(aNode.GlobalUserType);
			}
			catch (Exception err)
			{
				HandleException(err, "UpdateWorkflowMethod Method");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes an Planning Extract Method.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>

		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
                _planningExtractMethod = new OTSForecastPlanningExtractMethod(SAB, aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
			catch (Exception err)
			{
				HandleException(err, "DeleteWorkflowMethod Method");
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames an Planning Extract Method.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>
		/// <param name="aNewName">
		/// The new name of the workflow or method
		/// </param>

		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_planningExtractMethod = new OTSForecastPlanningExtractMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err, "RenameWorkflowMethod Method");
				FormLoadError = true;
				return false;
			}
		}

		/// <summary>
		/// Processes a method.
		/// </summary>
		/// <param name="aWorkflowRID">
		/// The record ID of the method
		/// </param>

		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_planningExtractMethod = new OTSForecastPlanningExtractMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.PlanningExtract, true);
			}
			catch (Exception err)
			{
				HandleException(err, "ProcessWorkflowMethod Method");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.Methods;	
		}

		/// <summary>
		/// Use to set the method name, description, user and global radio buttons
		/// </summary>

		override protected void SetCommonFields()
		{
			try
			{
				WorkflowMethodName = txtName.Text;
				WorkflowMethodDescription = txtDesc.Text;
				GlobalRadioButton = radGlobal;
				UserRadioButton = radUser;
				// End MID Track 4858
			}
			catch (Exception err)
			{
				HandleException(err, "SetCommonFields Method");
			}
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>

		override protected void SetSpecificFields()
		{
			ArrayList versOverList;
			int verRID;

			try
			{
				// Default to All Stores
				_chainVarChooser.UpdateData();
				_storeVarChooser.UpdateData();

                _chainTotalVarChooser.UpdateData();
                _storeTotalVarChooser.UpdateData();

                _planningExtractMethod.SG_RID = Include.AllStoreFilterRID;

                _planningExtractMethod.Chain = rdoChain.Checked;

                _planningExtractMethod.Store = rdoStore.Checked;

                _planningExtractMethod.AttributeSet = chxAttributeSetData.Checked;

                _planningExtractMethod.AttributeRID = (int)cboStoreAttribute.SelectedValue;
                
                _planningExtractMethod.HierarchyRID = ((HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData).Key;

				_planningExtractMethod.VersionRID = Convert.ToInt32(cboVersion.SelectedValue, CultureInfo.CurrentUICulture);

				_planningExtractMethod.DateRangeRID = drsTimePeriod.DateRangeRID;


                if (cboFilter.SelectedIndex != -1)
                {
                    _planningExtractMethod.FilterRID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
                }
                else
                {
                    _planningExtractMethod.FilterRID = Include.Undefined;
                }

				_planningExtractMethod.LowLevels = chkLowLevels.Checked;
				_planningExtractMethod.LowLevelsOnly = this.chkLowLevelsOnly.Checked;

				if (cboLowLevel.SelectedIndex != -1)
				{
					_planningExtractMethod.LowLevelsType = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType;
					_planningExtractMethod.LowLevelOffset = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset;
					_planningExtractMethod.LowLevelSequence = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence;
				}
				else
				{
					_planningExtractMethod.LowLevelsType = eLowLevelsType.None;
					_planningExtractMethod.LowLevelOffset = -1;
					_planningExtractMethod.LowLevelSequence = -1;
				}

				_planningExtractMethod.ShowIneligible = chkShowIneligible.Checked;
                _planningExtractMethod.ExcludeZeroValues = chkExcludeZeroValues.Checked;
                if (string.IsNullOrEmpty(txtConcurrentProcesses.Text))
                {
                    _planningExtractMethod.ConcurrentProcesses = 1;
                }
                else
                {
                    _planningExtractMethod.ConcurrentProcesses = Convert.ToInt32(txtConcurrentProcesses.Text);
                }

                if (rdoChain.Checked)
				{
					_planningExtractMethod.SelectableVariableList = (ArrayList)_chainSelectableVariableList.Clone();
                    _planningExtractMethod.SelectableTimetimeTotalVariableList = (ArrayList)_chainSelectableTotalVariableList.Clone();
                }
				else
				{
					_planningExtractMethod.SelectableVariableList = (ArrayList)_storeSelectableVariableList.Clone();
                    _planningExtractMethod.SelectableTimetimeTotalVariableList = (ArrayList)_storeSelectableTotalVariableList.Clone();
                }

                if (ChangePending)
                {
                    _planningExtractMethod.UpdateDate = DateTime.Now;
                }
			}			
			catch (Exception err)
			{
				HandleException(err, "SetSpecificFields Method");
			}
		}

		override public bool ChangePending
		{
			get
			{
				bool pending;

				try
				{
					pending = base.ChangePending;

					if (!pending)
					{
						if (rdoChain.Checked)
						{
							pending = _chainVarChooser.isChanged;
						}
						else
						{
							pending = _storeVarChooser.isChanged;
						}
					}

					return pending;
				}
				catch (Exception err)
				{
					HandleException(err, "ChangePending Method");
					throw;
				}
			}
			set
			{
				base.ChangePending = value;

				if (!value)
				{
					if (_chainVarChooser != null)
					{
						_chainVarChooser.ResetChangedFlag();
					}

					if (_storeVarChooser != null)
					{
						_storeVarChooser.ResetChangedFlag();
					}
				}
			}
		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>

		override protected bool ValidateSpecificFields()
		{
			bool retCode;

			try
			{
				retCode = true;

				//initialize all fields to not having an error
				ErrorProvider.SetError(txtMerchandise, string.Empty);
				ErrorProvider.SetError(cboVersion, string.Empty);
				ErrorProvider.SetError(drsTimePeriod, string.Empty);
				ErrorProvider.SetError(_chainVarChooser, string.Empty);
				ErrorProvider.SetError(_storeVarChooser, string.Empty);
                ErrorProvider.SetError(_chainTotalVarChooser, string.Empty);
                ErrorProvider.SetError(_storeTotalVarChooser, string.Empty);
                ErrorProvider.SetError(txtConcurrentProcesses, string.Empty);
				
				if (txtMerchandise.Text.Trim() == string.Empty
                    || ((MIDTag)(txtMerchandise.Tag)).MIDTagData == null) 
				{
					ErrorProvider.SetError(txtMerchandise, MIDText.GetTextOnly(eMIDTextCode.msg_MethodMerchandiseRequired));
					retCode = false;
				}

                HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData;
                if (hnp == null || hnp.HomeHierarchyType != eHierarchyType.organizational || hnp.LevelType == eHierarchyLevelType.Color || hnp.LevelType == eHierarchyLevelType.Size)  // Do not allow color or size levels at this time
                {
                    ErrorProvider.SetError(txtMerchandise, MIDText.GetTextOnly(eMIDTextCode.msg_MerchandiseInvalid));
                    retCode = false;
                }

				if (cboVersion.SelectedValue == null || Convert.ToInt32(cboVersion.SelectedValue) == Include.NoRID)
				{
					ErrorProvider.SetError(cboVersion, MIDText.GetTextOnly(eMIDTextCode.msg_MethodVersionRequired));
					retCode = false;
				}

                if (!string.IsNullOrEmpty(txtConcurrentProcesses.Text))
                {
                    int result;
                    if (!int.TryParse(txtConcurrentProcesses.Text, out result))
                    {
                        ErrorProvider.SetError(txtConcurrentProcesses, MIDText.GetTextOnly(eMIDTextCode.msg_MustBeInteger));
                        retCode = false;
                    }
                }

                if (drsTimePeriod.Text.Trim() == string.Empty) 
				{
					ErrorProvider.SetError(drsTimePeriod, MIDText.GetTextOnly(eMIDTextCode.msg_TimePeriodRequired));
					retCode = false;
				}

				if (rdoChain.Checked)
				{
					if (!_chainVarChooser.ValidateData())
					{
						retCode = false;
					}
                    if (!_chainTotalVarChooser.ValidateData())
                    {
                        retCode = false;
                    }

                    if (_chainVarChooser.SelectedCount == 0
                        && _chainTotalVarChooser.SelectedCount == 0)
                    {
                        retCode = false;
                        ErrorProvider.SetError(tabOptions, MIDText.GetTextOnly(eMIDTextCode.msg_NeedAtLeastOneVariable));
                        ErrorProvider.SetError(tabOptions, MIDText.GetTextOnly(eMIDTextCode.msg_NeedAtLeastOneVariable));
                    }
                }
				else
				{
					if (!_storeVarChooser.ValidateData())
					{
						retCode = false;
					}
                    if (!_storeTotalVarChooser.ValidateData())
                    {
                        retCode = false;
                    }

                    if (_storeVarChooser.SelectedCount == 0
                        && _storeTotalVarChooser.SelectedCount == 0)
                    {
                        retCode = false;
                        ErrorProvider.SetError(tabOptions, MIDText.GetTextOnly(eMIDTextCode.msg_NeedAtLeastOneVariable));
                        ErrorProvider.SetError(tabOptions, MIDText.GetTextOnly(eMIDTextCode.msg_NeedAtLeastOneVariable));
                    }
                }
				
				return retCode;
			}
			catch (Exception err)
			{
				HandleException(err, "ValidateSpecificFields Method");
				return false;
			}
		}

		/// <summary>
		/// Use to set the errors to the screen
		/// </summary>

		override protected void HandleErrors()
		{
			try
			{
				if (!WorkflowMethodNameValid)
				{
					ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
				}
				else
				{
					ErrorProvider.SetError (txtName,string.Empty);
				}
				if (!WorkflowMethodDescriptionValid)
				{
					ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
				}
				else
				{
					ErrorProvider.SetError (txtDesc,string.Empty);
				}
				if (!UserGlobalValid)
				{
					ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
				}
				else
				{
					ErrorProvider.SetError (pnlGlobalUser,string.Empty);
				}
			}
			catch (Exception err)
			{
				HandleException(err, "HandleErrors Method");
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>

		override protected void SetObject()
		{
			try
			{
				ABM = _planningExtractMethod;
			}
			catch (Exception err)
			{
				HandleException(err, "SetObject Method");
			}
		}

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>

		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			return _explorerNode;
		}

        override protected void BuildAttributeList()
        {
        }

		private void SetText()
		{
			try
			{
				if (_planningExtractMethod.Method_Change_Type == eChangeType.update)
				{
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}
				else
				{
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				}
	
                this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_PlanningExtractMethod);
                this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod);
				this.lblStoreFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				this.grpLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevels);
				this.btnOverrideLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
				this.rdoChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ChainData);
				this.rdoStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_StoreData);
                this.chxAttributeSetData.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_AttributeSetData);
                this.lblStoreAttribute.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_Attribute);
                this.chkShowIneligible.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_ShowIneligible);
				this.tabOptions.TabPages[0].Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_Variables);
                this.tabOptions.TabPages[1].Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_TotalVariables);
                this.chkExcludeZeroValues.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_ExcludeZeroValues);
				this.ttpToolTip.SetToolTip(this.chkExcludeZeroValues, MIDText.GetTextOnly(eMIDTextCode.tt_ExportMethod_ExcludeZeroValues));
				this.lblConcurrentProcesses.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanningExtractMethod_ConcurrentProcesses);
				
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			try
			{
				_storeFilterDL = new FilterData();
				_masterVersionProfList = SAB.ClientServerSession.GetUserForecastVersions();

				SetText();

				Name = MIDText.GetTextOnly((int)eMethodType.PlanningExtract);

				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

                LoadOverrideModelComboBox(cboOverride.ComboBox, _planningExtractMethod.OverrideLowLevelRid, _planningExtractMethod.CustomOLL_RID);  

				BindFilterComboBox();
				BindVersionComboBox();
                BindStoreAttributeComboBox();

                //Create an App Server Transaction

                _transaction = SAB.ApplicationServerSession.CreateTransaction();

				LoadMethodData();

                if (!FunctionSecurity.AllowUpdate)
                {
                    this._chainVarChooser.Enabled = false;
                    this._storeVarChooser.Enabled = false;
                    this._chainTotalVarChooser.Enabled = false;
                    this._storeTotalVarChooser.Enabled = false;
                }
                else
                {
                    if (_planningExtractMethod.Store)
                    {
                        cboFilter.Enabled = true;
                        lblStoreFilter.Enabled = true;
                        chkShowIneligible.Enabled = true;
                        if (_planningExtractMethod.AttributeSet)
                        {
                            cboStoreAttribute.Enabled = true;
                        }
                        else
                        {
                            cboStoreAttribute.Enabled = false;
                        }
                    }
                    else
                    {
                        cboFilter.Enabled = false;
                        lblStoreFilter.Enabled = false;
                        chkShowIneligible.Enabled = false;
                        lblStoreAttribute.Enabled = false;
                        cboStoreAttribute.Enabled = false;
                        chxAttributeSetData.Enabled = false;
                    }
                }

                ApplySecurity();

                tabGenAllocMethod.Controls.Remove(tabProperties);
                _chainVarChooser.ListChanged += VarChooser_ListChanged;
                _storeVarChooser.ListChanged += VarChooser_ListChanged;
                _chainTotalVarChooser.ListChanged += VarChooser_ListChanged;
                _storeTotalVarChooser.ListChanged += VarChooser_ListChanged;
            }
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void BindVersionComboBox()
		{
            ProfileList versionProfList = null;
			try
			{
                _chainLowLevelVersionList = base.GetForecastVersionList(eSecuritySelectType.View, eSecurityTypes.Chain, false, _planningExtractMethod.VersionRID, false);	
                _storeLowLevelVersionList = base.GetForecastVersionList(eSecuritySelectType.View, eSecurityTypes.Store, false, _planningExtractMethod.VersionRID, false);

				if (_planningExtractMethod.Chain)
				{
                    versionProfList = _chainLowLevelVersionList;
				}
				else
				{
                    versionProfList = _storeLowLevelVersionList;
				}

				this.cboVersion.DisplayMember = "Description";
				this.cboVersion.ValueMember = "Key";
                this.cboVersion.DataSource = versionProfList.ArrayList;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void BindFilterComboBox()
		{
			FunctionSecurityProfile filterUserSecurity;
			FunctionSecurityProfile filterGlobalSecurity;
			ArrayList userRIDList;
			DataTable dtFilter;

			try
			{
				filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
				filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

				userRIDList = new ArrayList();
				userRIDList.Add(-1);

				if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
				{
					userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
				{
					userRIDList.Add(Include.GlobalUserRID);
				}

				cboFilter.Items.Clear();
                cboFilter.Items.Add(GetRemoveFilterRow());

                dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList);

				foreach (DataRow row in dtFilter.Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        private void BindStoreAttributeComboBox()
        {
            ProfileList sgpl;
            StoreGroupListViewProfile sgp;
            FunctionSecurityProfile userAttrSecLvl;

            try
            {
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, true);
                AdjustTextWidthComboBox_DropDown(cboStoreAttribute);

                userAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                sgpl = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, !userAttrSecLvl.AccessDenied);
                sgp = (StoreGroupListViewProfile)sgpl[0];

                cboStoreAttribute.Initialize(SAB, FunctionSecurity, sgpl.ArrayList, true);
                if (_planningExtractMethod.AttributeRID == Include.NoRID)
                {
                    //this.cboStoreAttribute.SelectedValue = sgp.Key;
                    this.cboStoreAttribute.SelectedValue = Include.AllStoreGroupRID;
                }
                else
                {
                    this.cboStoreAttribute.SelectedValue = _planningExtractMethod.AttributeRID;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private void LoadMethodData()
		{
			HierarchyNodeProfile hnp;
			DateRangeProfile drp;

			try
			{
				drsTimePeriod.DateRangeRID = Include.UndefinedCalendarDateRange;
				drsTimePeriod.SetImage(null);

                if (_planningExtractMethod.Chain)
                {
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _planningExtractMethod.GlobalUserType == eGlobalUserType.User);
                    txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_PlanningExtractMethod, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
                }
                else
                {
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _planningExtractMethod.GlobalUserType == eGlobalUserType.User);  
                    txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_PlanningExtractMethod, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                }

				if (_planningExtractMethod.Method_Change_Type != eChangeType.add)
				{
					this.txtName.Text = _planningExtractMethod.Name;
					this.txtDesc.Text = _planningExtractMethod.Method_Description;
				}

				if (_planningExtractMethod.Store)
				{
                    rdoStore.Checked = true;
                    _currentVersionProfList = _storeLowLevelVersionList;
                }
				else
				{
                    rdoChain.Checked = true;
                    _currentVersionProfList = _chainLowLevelVersionList;
                }

                if (_planningExtractMethod.AttributeSet)
                {
                    chxAttributeSetData.Checked = true;
                }
                else
                {

                }

				if (_planningExtractMethod.HierarchyRID > 0)
				{
                    hnp = SAB.HierarchyServerSession.GetNodeData(_planningExtractMethod.HierarchyRID, true, true);

                    txtMerchandise.Text = hnp.Text;
                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;

					cboLowLevel.Enabled = true;
                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox);
				}
				else
				{
                    txtMerchandise.Text = string.Empty;
                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;

					cboLowLevel.Enabled = false;
				}

				if (_planningExtractMethod.VersionRID > 0)
				{
					cboVersion.SelectedValue = _planningExtractMethod.VersionRID;
				}
				else
				{
					_planningExtractMethod.VersionRID = (int)this.cboVersion.SelectedValue;
				}

				if (_planningExtractMethod.DateRangeRID > 0 && _planningExtractMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					drp = SAB.ClientServerSession.Calendar.GetDateRange(_planningExtractMethod.DateRangeRID);
					LoadDateRangeSelector(drsTimePeriod, drp);
				}
 			 
				if (_planningExtractMethod.FilterRID > 0 && _planningExtractMethod.FilterRID != Include.UndefinedStoreFilter)
				{
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_planningExtractMethod.FilterRID, -1, ""));
				}

				chkLowLevels.Checked = _planningExtractMethod.LowLevels;
				grpLowLevels.Enabled = chkLowLevels.Checked;

				chkLowLevelsOnly.Checked = _planningExtractMethod.LowLevelsOnly;

				cboLowLevel.SelectedIndex = cboLowLevel.Items.IndexOf(new LowLevelCombo(_planningExtractMethod.LowLevelsType, _planningExtractMethod.LowLevelOffset, _planningExtractMethod.LowLevelSequence, ""));

				_storeSelectableVariableList = new ArrayList();
				_chainSelectableVariableList = new ArrayList();

				foreach (RowColProfileHeader rowColHdr in _planningExtractMethod.SelectableVariableList)
				{
					_chainSelectableVariableList.Add(rowColHdr.Copy());
					_storeSelectableVariableList.Add(rowColHdr.Copy());
				}

				UpdateSelectableList(ePlanType.Chain, _chainSelectableVariableList);
				UpdateSelectableList(ePlanType.Store, _storeSelectableVariableList);

                _chainVarChooser = new RowColChooserOrderPanel(_chainSelectableVariableList, false, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());
                _storeVarChooser = new RowColChooserOrderPanel(_storeSelectableVariableList, false, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());

				tpgVariables.Controls.Add(_chainVarChooser);
				tpgVariables.Controls.SetChildIndex(_chainVarChooser, 0);
				_chainVarChooser.Dock = DockStyle.Fill;
				_chainVarChooser.FillControl();

				tpgVariables.Controls.Add(_storeVarChooser);
				tpgVariables.Controls.SetChildIndex(_storeVarChooser, 0);
				_storeVarChooser.Dock = DockStyle.Fill;
				_storeVarChooser.FillControl();

				if (_planningExtractMethod.Chain
                    || rdoChain.Checked)
				{
					_storeVarChooser.Visible = false;
				}
				else
				{
					_chainVarChooser.Visible = false;
				}

                _storeSelectableTotalVariableList = new ArrayList();
                _chainSelectableTotalVariableList = new ArrayList();

                foreach (RowColProfileHeader rowColHdr in _planningExtractMethod.SelectableTimetimeTotalVariableList)
                {
                    _chainSelectableTotalVariableList.Add(rowColHdr.Copy());
                    _storeSelectableTotalVariableList.Add(rowColHdr.Copy());
                }

                UpdateSelectableTimeTotalList(ePlanType.Chain, _chainSelectableTotalVariableList);
                UpdateSelectableTimeTotalList(ePlanType.Store, _storeSelectableTotalVariableList);

                _chainTotalVarChooser = new RowColChooserOrderPanel(_chainSelectableTotalVariableList, false, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());
                _storeTotalVarChooser = new RowColChooserOrderPanel(_storeSelectableTotalVariableList, false, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());

                tpgTimeTotalVariables.Controls.Add(_chainTotalVarChooser);
                tpgTimeTotalVariables.Controls.SetChildIndex(_chainTotalVarChooser, 0);
                _chainTotalVarChooser.Dock = DockStyle.Fill;
                _chainTotalVarChooser.FillControl();

                tpgTimeTotalVariables.Controls.Add(_storeTotalVarChooser);
                tpgTimeTotalVariables.Controls.SetChildIndex(_storeTotalVarChooser, 0);
                _storeTotalVarChooser.Dock = DockStyle.Fill;
                _storeTotalVarChooser.FillControl();

                if (_planningExtractMethod.Chain
                    || rdoChain.Checked)
                {
                    _storeTotalVarChooser.Visible = false;
                }
                else
                {
                    _chainTotalVarChooser.Visible = false;
                }

                chkShowIneligible.Checked = _planningExtractMethod.ShowIneligible;
                chkExcludeZeroValues.Checked = _planningExtractMethod.ExcludeZeroValues;
                txtConcurrentProcesses.Text = _planningExtractMethod.ConcurrentProcesses.ToString();

				LoadWorkflows();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//==============
		//EVENT HANDLERS
		//==============

        private void VarChooser_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }

		private void rdoChain_CheckedChanged(object sender, System.EventArgs e)
		{
            HierarchyNodeProfile hnp = null;
			try
			{
				if (FormLoaded)
				{
					if (rdoChain.Checked)
					{
                        if (txtMerchandise.Tag != null && txtMerchandise.Tag is MIDTag)
                        {
                            hnp = (HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData;
                        }
                        txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_PlanningExtractMethod, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
                        ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;

						_currentVersionProfList = _chainLowLevelVersionList;
                        cboVersion.DataSource = _chainLowLevelVersionList.ArrayList;
						cboFilter.Enabled = false;
                        lblStoreFilter.Enabled = false;
                        chkShowIneligible.Enabled = false;
                        lblStoreAttribute.Enabled = false;
                        cboStoreAttribute.Enabled = false;
                        chxAttributeSetData.Enabled = false;
                        _chainVarChooser.Visible = true;
						_storeVarChooser.Visible = false;
                        _chainTotalVarChooser.Visible = true;
                        _storeTotalVarChooser.Visible = false;
                        _planningExtractMethod.Chain = true;
                        _planningExtractMethod.Store = false;

                        ApplySecurity();
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void rdoStore_CheckedChanged(object sender, System.EventArgs e)
		{
            HierarchyNodeProfile hnp = null;
			try
			{
				if (FormLoaded)
				{
					if (rdoStore.Checked)
					{
                        if (txtMerchandise.Tag != null && txtMerchandise.Tag is MIDTag)
                        {
                            hnp = (HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData;
                        }
                        txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_PlanningExtractMethod, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                        ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;

						_currentVersionProfList = _storeLowLevelVersionList;
                        cboVersion.DataSource = _storeLowLevelVersionList.ArrayList;
						cboFilter.Enabled = true;
                        lblStoreFilter.Enabled = true;
                        chkShowIneligible.Enabled = true;
                        lblStoreAttribute.Enabled = true;
                        cboStoreAttribute.Enabled = true;
                        chxAttributeSetData.Enabled = true;
                        _chainVarChooser.Visible = false;
						_storeVarChooser.Visible = true;
                        _chainTotalVarChooser.Visible = false;
                        _storeTotalVarChooser.Visible = true;
                        _planningExtractMethod.Store = true;
                        _planningExtractMethod.Chain = false;

                        ApplySecurity();
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Merchandise_DragEnter(sender, e);
		}

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
        {

            TreeNodeClipboardList cbList;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                if (hnp == null || hnp.HomeHierarchyType != eHierarchyType.organizational || hnp.LevelType == eHierarchyLevelType.Color || hnp.LevelType == eHierarchyLevelType.Size)  // Do not allow color or size levels at this time
                {
                    Image_DragOver(sender, e);
                    e.Effect = DragDropEffects.None;
                    return;
                }
                else
                {
                    Image_DragOver(sender, e);
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox); 
                    _planningExtractMethod.HierarchyRID = hnp.Key;

                    ChangePending = true;
                    ApplySecurity();
                }
            }
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception err)
            {
                HandleException(err);
            }
        }

		private void txtMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			_textChanged = true;
		}

		private void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}
	
		private void txtMerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
				_textChanged = false;
				_priorError = false;

                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
					_nodeRID = Include.NoRID;

                    PopulateLowLevelList(null, cboLowLevel.ComboBox);  

					_planningExtractMethod.HierarchyRID = Include.NoRID;

					ChangePending = true;
					ApplySecurity();
                }
                else
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    _nodeRID = hnp.Key;

                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox);  

                    _planningExtractMethod.HierarchyRID = hnp.Key;

                    ChangePending = true;
                    ApplySecurity();
                }
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void cboVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					_planningExtractMethod.VersionRID = (int)this.cboVersion.SelectedValue;

					ChangePending = true;
					ApplySecurity();
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void cboLowLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void drsTimePeriod_ClickCellButton(object sender, CellEventArgs e)
		{
			try
			{
				if (drsTimePeriod.Tag !=null)
				{
					((CalendarDateSelector)drsTimePeriod.DateRangeForm).DateRangeRID = (int)drsTimePeriod.Tag;
				} 

				drsTimePeriod.ShowSelector();
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void drsTimePeriod_Load(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				drsTimePeriod.DateRangeForm = frm;
				frm.AllowDynamicToCurrent = true;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;

				if (_planningExtractMethod.DateRangeRID > 0 && _planningExtractMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					frm.DateRangeRID = _planningExtractMethod.DateRangeRID;
				}

				frm.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;

				frm.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
				frm.AllowDynamicToStoreOpen = false;
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		/// <summary>
		/// After selection is made on midDateRangeSelector - Planning Extract
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

		private void drsTimePeriod_OnSelection(object sender, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (!e.SelectionCanceled)
				{
					_drTimePeriod = e.SelectedDateRange;
					LoadDateRangeText(_drTimePeriod, drsTimePeriod);
					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}

                if (cboFilter.SelectedIndex != -1)
                {
                    if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == Include.Undefined)
                    {
                        cboFilter.SelectedIndex = -1;
                    }
                }
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboFilter_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                    ((MIDComboBoxEnh)((ComboBox)sender).Parent).FirePropertyChangeEvent();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

        private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    ChangePending = true;
                    if (((ComboBox)sender).SelectedIndex != -1)
                    {
                        // set all store attribute combo boxes to the same index
                        if (cboStoreAttribute.SelectedIndex != ((ComboBox)sender).SelectedIndex)
                        {
                            cboStoreAttribute.SelectedIndex = ((ComboBox)sender).SelectedIndex;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void chkLowLevels_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (chkLowLevels.Checked)
					{
						grpLowLevels.Enabled = true;
					}
					else
					{
						grpLowLevels.Enabled = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void chkLowLevelsOnly_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnOverrideLowLevels_Click(object sender, System.EventArgs e)
		{
			int verRID;
			HierarchyNodeProfile hnp;

			try
			{
				verRID = Convert.ToInt32(cboVersion.SelectedValue, CultureInfo.CurrentUICulture);

				if (verRID == -1)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_VersionRequired), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				string lowLevelText = string.Empty;
				if (cboLowLevel.SelectedIndex != -1)
					lowLevelText = cboLowLevel.Items[cboLowLevel.SelectedIndex].ToString();


				System.Windows.Forms.Form parentForm;
				parentForm = this.MdiParent;

				object[] args = null;

				FunctionSecurityProfile methodSecurity;
				if (radGlobal.Checked)
					methodSecurity = GlobalSecurity;
				else
					methodSecurity = UserSecurity;
				args = new object[] { SAB, _planningExtractMethod.OverrideLowLevelRid, _planningExtractMethod.HierarchyRID, _planningExtractMethod.VersionRID, lowLevelText, _planningExtractMethod.CustomOLL_RID, methodSecurity };

                _overrideLowLevelfrm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                parentForm = this.MdiParent;
                _overrideLowLevelfrm.MdiParent = parentForm;
                _overrideLowLevelfrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                _overrideLowLevelfrm.Show();
                _overrideLowLevelfrm.BringToFront();
                ((frmOverrideLowLevelModel)_overrideLowLevelfrm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);
                
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

        System.Windows.Forms.Form _overrideLowLevelfrm;

        private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (_planningExtractMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_planningExtractMethod.OverrideLowLevelRid = e.aOllRid;
					if (_planningExtractMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_planningExtractMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_planningExtractMethod.Key, _planningExtractMethod.CustomOLL_RID);
					}

                    if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                    {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _planningExtractMethod.CustomOLL_RID);  
                    }

                    _overrideLowLevelfrm = null;
				}
			}
			catch
			{
				throw;
			}
		}

		private void chkShowIneligible_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkExcludeZeroValues_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingPlanningExtractOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtConcurrentProcesses_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingPlanningExtractOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

        private void chxAttributeSetData_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded && !_loadingPlanningExtractOptions)
                {
                    ChangePending = true;
                    if (chxAttributeSetData.Checked)
                    {
                        cboStoreAttribute.Enabled = true;
                        lblStoreAttribute.Enabled = true;
                    }
                    else
                    {
                        cboStoreAttribute.Enabled = false;
                        lblStoreAttribute.Enabled = false;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        protected override void Call_btnProcess_Click()
		{
			try
			{
				ProcessAction(eMethodType.PlanningExtract);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_planningExtractMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		protected override void Call_btnSave_Click()
		{
			try
			{
				base.btnSave_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}	
		}
	
		#region IFormBase Members

		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch (Exception err)
			{
				HandleException(err);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		#endregion

		private void LoadWorkflows()
		{
			try
			{
                GetOTSPLANWorkflows(_planningExtractMethod.Key, ugWorkflows);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void UpdateSelectableList(ePlanType aPlanType, ArrayList aSelectableList)
		{
			try
			{
				switch (aPlanType)
				{
					case ePlanType.Chain :

						foreach (RowColProfileHeader rowHeader in aSelectableList)
						{
							if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Chain)
							{
								rowHeader.IsSelectable = false;
							}
							else
							{
								rowHeader.IsSelectable = true;
							}

                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
						}

						break;

					case ePlanType.Store :

						foreach (RowColProfileHeader rowHeader in aSelectableList)
						{
							if (((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((VariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Store)
							{
								rowHeader.IsSelectable = false;
							}
							else
							{
								rowHeader.IsSelectable = true;
							}
                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
						}

						break;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        private void UpdateSelectableTimeTotalList(ePlanType aPlanType, ArrayList aSelectableList)
        {
            try
            {
                switch (aPlanType)
                {
                    case ePlanType.Chain:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Chain)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }

                            if (rowHeader.Profile.GetType() == typeof(TimeTotalVariableProfile))
                            {
                                rowHeader.Grouping = ((TimeTotalVariableProfile)rowHeader.Profile).ParentVariableProfile.Groupings;
                            }
                        }

                        break;

                    case ePlanType.Store:

                        foreach (RowColProfileHeader rowHeader in aSelectableList)
                        {
                            if (((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Both && ((TimeTotalVariableProfile)rowHeader.Profile).VariableCategory != eVariableCategory.Store)
                            {
                                rowHeader.IsSelectable = false;
                            }
                            else
                            {
                                rowHeader.IsSelectable = true;
                            }
                            if (rowHeader.Profile.GetType() == typeof(TimeTotalVariableProfile))
                            {
                                rowHeader.Grouping = ((TimeTotalVariableProfile)rowHeader.Profile).ParentVariableProfile.Groupings;
                            }
                        }

                        break;
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        private void PopulateLowLevelList(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox)
		{
			HierarchyProfile hierProf;
			int i;
			HierarchyLevelProfile hlp;
			HierarchyProfile mainHierProf;
			int highestGuestLevel;
			int longestBranchCount;
			int offset;

			try
			{
				aComboBox.Items.Clear();

				if (aHierarchyNodeProfile == null)
				{
					aComboBox.Enabled = false;
				}
				else
				{
					aComboBox.Enabled = true;
				}

				if (aHierarchyNodeProfile != null)
				{
					hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);

					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						for (i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
                            // Do not allow color or size levels at this time
                            if (hlp.LevelType != eHierarchyLevelType.Color
                                && hlp.LevelType != eHierarchyLevelType.Size)
                            {
                                aComboBox.Items.Add(
                                    new LowLevelCombo(eLowLevelsType.HierarchyLevel,
                                    i - aHierarchyNodeProfile.HomeHierarchyLevel,
                                    hlp.Key,
                                    hlp.LevelID));
                            }
						}
					}
					else
					{
						mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
							_longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
						}

						highestGuestLevel = _longestHighestGuest;

						// add guest levels to comboBox
						if (highestGuestLevel != int.MaxValue)
						{
							for (i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
							{
								if (i == 0)
								{
									aComboBox.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										0,
										0,
										"Root"));
								}
								else
								{
									hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
									aComboBox.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										i,
										hlp.Key,
										hlp.LevelID));
								}
							}
						}

						// add offsets to comboBox
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
						}

						longestBranchCount = _longestBranch; 
						offset = 0;

						for (i = 0; i < longestBranchCount; i++)
						{
							++offset;
                            aComboBox.Items.Add(
                                new LowLevelCombo(eLowLevelsType.LevelOffset,
                                offset,
                                0,
                                null));
						}
					}

					if (aComboBox.Items.Count > 0)
					{
						aComboBox.SelectedIndex = 0;
					}

					_currentLowLevelNode = aHierarchyNodeProfile.Key;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string productID;
			string[] pArray;

			try
			{
				productID = aProductID.Trim();
				pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 

				HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
				EditMsgs em = new EditMsgs();
				return hm.NodeLookup(ref em, productID, false);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void LoadDateRangeSelector(Controls.MIDDateRangeSelector aMIDDRS, DateRangeProfile aDateRangeProf)
		{
			try
			{
				aMIDDRS.Text = aDateRangeProf.DisplayDate;
				aMIDDRS.DateRangeRID = aDateRangeProf.Key;

				if (aDateRangeProf.DateRangeType == eCalendarRangeType.Dynamic)
				{
					aMIDDRS.SetImage(_dynamicToCurrentImage);
				}
				else
				{
					aMIDDRS.SetImage(null);
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void LoadDateRangeText(DateRangeProfile aDateRangeProf, Controls.MIDDateRangeSelector aMIDDRS)
		{
			try
			{
				if (aDateRangeProf != null)
				{
					if (aDateRangeProf.DisplayDate != null)
					{
						aMIDDRS.Text = aDateRangeProf.DisplayDate;
					}
					else
					{
						aMIDDRS.Text = string.Empty;
					}

					aMIDDRS.Tag = aDateRangeProf.Key;
				 
					if (aDateRangeProf.DateRangeType == eCalendarRangeType.Dynamic)
					{
						aMIDDRS.SetImage(ReoccurringImage);
					}
					else
					{
						aMIDDRS.SetImage(null);
					}
				}
				else
				{
					aMIDDRS.Text = string.Empty;
					aMIDDRS.Tag = null;
					aMIDDRS.SetImage(null);
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		override protected bool ApplySecurity()	
		{
			bool securityOk = true;

            if (_planningExtractMethod.Chain)
            {
                securityOk = base.ValidateChainPlanVersionSecurity(cboVersion.ComboBox, true);  

                if (securityOk)
                    securityOk = (((MIDControlTag)(txtMerchandise.Tag)).IsAuthorized(eSecurityTypes.Chain, eSecuritySelectType.Update));
            }
            else 
            {
                securityOk = base.ValidateStorePlanVersionSecurity(cboVersion.ComboBox, true);  

                if (securityOk)
                    securityOk = (((MIDControlTag)(txtMerchandise.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));
            }

			if (FunctionSecurity.AllowUpdate)
			{
				if (_planningExtractMethod.StoreAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
				{
					rdoStore.Enabled = true;
				}
				else
				{
					rdoStore.Enabled = false;
				}

				if (_planningExtractMethod.ChainAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
				{
					rdoChain.Enabled = true;
				}
				else
				{
					rdoChain.Enabled = false;
				}
			}

            bool canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
            base.ApplyCanUpdate(canUpdate);
            if (!canUpdate)
            {
                if (FunctionSecurity.IsReadOnly
                    || txtMerchandise.Text.Trim().Length == 0
                    || (cboVersion.SelectedValue == null
                    || Convert.ToInt32(cboVersion.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID))
                {
                    // Skip
                }
                else
                {
                    securityOk = false;
                }
            }

			return securityOk;	// track 5871 stodd
		}

		private bool ConvertBoolConfigValue(string aBoolConfigValue)
		{
			try
			{
				if (aBoolConfigValue != null)
				{
					if (aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "true" || aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "yes" ||
						aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "t" || aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "y")
					{
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private int ConvertNumericConfigValue(string aNumericConfigValue)
		{
			try
			{
				return Convert.ToInt32(aNumericConfigValue);
			}
			catch (FormatException)
			{
				return 0;
			}
			catch (OverflowException)
			{
				return 0;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            if (FormLoaded)
            {
                _planningExtractMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
                ChangePending = true;
            }
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVersion_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOverride_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboLowLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboLowLevel_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
