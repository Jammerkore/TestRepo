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
	/// Summary description for frmExportMethod.
	/// </summary>
	public partial class frmExportMethod : WorkflowMethodFormBase
	{
		// Begin MID Track 4858 - JSmith - Security changes
//		private SessionAddressBlock SAB;
//		private FunctionSecurityProfile _userSecurity;
//		private FunctionSecurityProfile _globalSecurity;
		// End MID Track 4858
		private RowColChooserOrderPanel _chainVarChooser;
		private RowColChooserOrderPanel _storeVarChooser;
		//private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL;
		private ProfileList _masterVersionProfList;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
        //private DataTable _dtChainForecastVersions;
        //private DataTable _dtStoreForecastVersions;
		private ProfileList _currentVersionProfList;
		private ArrayList _chainSelectableVariableList;
		private ArrayList _storeSelectableVariableList;
		private ProfileList _chainLowLevelVersionList;
		private ProfileList _storeLowLevelVersionList;
		//private ProfileList _lowlevelVersionOverrideList;
		private bool _loadingExportOptions = false;
		//private UltraGrid ugWorkflows;

		private MIDWorkflowMethodTreeNode _explorerNode = null;
		private OTSForecastExportMethod _exportMethod = null;
		private int _nodeRID = -1;
		private bool _textChanged = false;
		private bool _priorError = false;
		private DateRangeProfile _drTimePeriod = null;
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
// Begin Track #4868 - JSmith - Variable Groupings
        private ApplicationSessionTransaction _transaction;
// End Track #4868
		
		//=============
		// CONSTRUCTORS
		//=============

		public frmExportMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
			: base (SAB, aEAB, eMIDTextCode.frm_ExportMethod, eWorkflowMethodType.Method)
		{
			// Begin MID Track 4858 - JSmith - Security changes
//			_SAB = SAB;
			// End MID Track 4858
			InitializeComponent();
			// Begin MID Track 4858 - JSmith - Security changes
//			_userSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserExport);
//			_globalSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalExport);
			UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserExport);
			GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalExport);
			// End MID Track 4858
			// BEGIN Override Low Level Enhancement
			//_lowlevelVersionOverrideList = new ProfileList(eProfileType.LowLevelVersionOverride);
			// END Override Low Level Enhancement
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
		/// Opens a new Velocity Method.
		/// </summary>

		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_explorerNode = aParentNode;

				// Begin MID Track 4858 - JSmith - Security changes
//				FunctionSecurity = new FunctionSecurityProfile((int)eSecurityFunctions.NotSpecified);
//
//				if (_userSecurity.AllowUpdate || _globalSecurity.AllowUpdate)
//				{
//					FunctionSecurity.SetAllowUpdate();
//				}
//				else
//				{
//					FunctionSecurity.SetAccessDenied();
//				}
				// End MID Track 4858

				_exportMethod = new OTSForecastExportMethod(SAB, Include.NoRID);
				
				// Begin MID Track 4858 - JSmith - Security changes
//				_exportMethod.Method_Change_Type = eChangeType.add;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserExport, eSecurityFunctions.ForecastMethodsGlobalExport);
				// End MID Track 4858
				
				Common_Load(aParentNode.GlobalUserType);				 

				// Begin MID Track 4858 - JSmith - Security changes
//				if (_userSecurity.AllowUpdate)
//				{
//					rdoUser.Enabled = true;
//				}
//				else
//				{
//					rdoUser.Enabled = false;
//				}
//
//				if (_globalSecurity.AllowUpdate)
//				{
//					rdoGlobal.Enabled = true;
//				}
//				else
//				{
//					rdoGlobal.Enabled = false;
//				}
//
//				if (aParentNode.GlobalUserType == eGlobalUserType.User)
//				{
//					if (rdoUser.Enabled)
//					{
//						rdoUser.Checked = true;
//					}
//					else
//					{
//						rdoGlobal.Checked = true;
//					}
//				}
//				else
//				{
//					if (rdoGlobal.Enabled)
//					{
//						rdoGlobal.Checked = true;
//					}
//					else
//					{
//						rdoUser.Checked = true;
//					}
//				}
				// End MID Track 4858
			}
			catch (Exception err)
			{
				HandleException(err, "NewWorkflowMethod Method");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Opens an existing Export Method.
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

				// Begin MID Track 4858 - JSmith - Security changes
//				if (_explorerNode.GlobalUserType == eGlobalUserType.User)
//				{
//					FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserExport);
//				}
//				else
//				{
//					FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalExport);
//				}
//
//				if (aLockStatus == eLockStatus.ReadOnly)
//				{
//					FunctionSecurity.SetReadOnly();
//				}

				// End MID Track 4858

				_nodeRID = aNodeRID;
				_exportMethod = new OTSForecastExportMethod(SAB, aMethodRID);
				// Begin MID Track 4858 - JSmith - Security changes
//				_exportMethod.Method_Change_Type = eChangeType.update;
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserExport, eSecurityFunctions.ForecastMethodsGlobalExport);
				// End MID Track 4858

				Common_Load(aNode.GlobalUserType);
				// Begin MID Track 4858 - JSmith - Security changes
//				rdoGlobal.Enabled = false;
//				rdoUser.Enabled = false;
				// End MID Track 4858
				//Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
				//// Begin Track #5429 - JSmith - Cannot export if read only
				//if (FunctionSecurity.AllowView)
				//{
				//    btnProcess.Enabled = true;
				//}
				//// End Track #5429
				//Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
			}
			catch (Exception err)
			{
				HandleException(err, "UpdateWorkflowMethod Method");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes an Export Method.
		/// </summary>
		/// <param name="aMethodRID">
		/// The record ID of the method
		/// </param>

		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
                _exportMethod = new OTSForecastExportMethod(SAB, aMethodRID);
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
		/// Renames an Export Method.
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
				_exportMethod = new OTSForecastExportMethod(SAB, aMethodRID);
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
				_exportMethod = new OTSForecastExportMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.Export, true);
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
				// Begin MID Track 4858 - JSmith - Security changes
//				WorkflowMethodName = txtMethodName.Text;
//				WorkflowMethodDescription = txtMethodDesc.Text;
//				GlobalRadioButton = rdoGlobal;
//				UserRadioButton = rdoUser;
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

				_exportMethod.SG_RID = Include.AllStoreFilterRID;

				if (rdoChain.Checked)
				{
					_exportMethod.PlanType = ePlanType.Chain;
				}
				else
				{
					_exportMethod.PlanType = ePlanType.Store;
				}

                //Begin Track #5858 - KJohnson - Validating store security only
                _exportMethod.HierarchyRID = ((HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData).Key;
                //End Track #5858

				_exportMethod.VersionRID = Convert.ToInt32(cboVersion.SelectedValue, CultureInfo.CurrentUICulture);

				_exportMethod.DateRangeRID = drsTimePeriod.DateRangeRID;


                if (cboFilter.SelectedIndex != -1)
                {
                    _exportMethod.FilterRID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
                }
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                else
                {
                    _exportMethod.FilterRID = Include.Undefined;
                }
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

				_exportMethod.LowLevels = chkLowLevels.Checked;
				_exportMethod.LowLevelsOnly = this.chkLowLevelsOnly.Checked;

				if (cboLowLevel.SelectedIndex != -1)
				{
					_exportMethod.LowLevelsType = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType;
					_exportMethod.LowLevelOffset = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset;
					_exportMethod.LowLevelSequence = ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence;
				}
				else
				{
					_exportMethod.LowLevelsType = eLowLevelsType.None;
					_exportMethod.LowLevelOffset = -1;
					_exportMethod.LowLevelSequence = -1;
				}

				_exportMethod.ShowIneligible = chkShowIneligible.Checked;
				_exportMethod.UseDefaultSettings = chkDefaultSettings.Checked;

				if (!_exportMethod.UseDefaultSettings)
				{
					if (rdoCSV.Checked)
					{
						_exportMethod.ExportType = DataCommon.eExportType.CSV;  // Added DataCommon for 3.0 compile
						_exportMethod.Delimeter = txtDelimeter.Text.Trim();
//Begin Track #4942 - JScott - Correct problems in Export Method
						_exportMethod.CSVFileExtension = txtCSVFileSuffix.Text.Trim();
//End Track #4942 - JScott - Correct problems in Export Method
					}
					else
					{
                        _exportMethod.ExportType = DataCommon.eExportType.XML;  // Added DataCommon for 3.0 compile
						_exportMethod.Delimeter = string.Empty;
//Begin Track #4942 - JScott - Correct problems in Export Method
						_exportMethod.CSVFileExtension = string.Empty;
//End Track #4942 - JScott - Correct problems in Export Method
					}

//Begin Track #4942 - JScott - Correct problems in Export Method
					if (this.rdoCalendar.Checked)
					{
						_exportMethod.DateType = eExportDateType.Calendar;
					}
					else
					{
						_exportMethod.DateType = eExportDateType.Fiscal;
					}

//End Track #4942 - JScott - Correct problems in Export Method
					_exportMethod.PreinitValues = chkPreinitValues.Checked;
					//Begin Track #5395 - JScott - Add ability to discard zero values in Export
					_exportMethod.ExcludeZeroValues = chkExcludeZeroValues.Checked;
					//End Track #5395 - JScott - Add ability to discard zero values in Export
					_exportMethod.FilePath = txtFilePath.Text.Trim();
					_exportMethod.AddDateStamp = chkDateStamp.Checked;
					_exportMethod.AddTimeStamp = chkTimeStamp.Checked;

					if (rdoSplitNone.Checked)
					{
						_exportMethod.SplitType = eExportSplitType.None;
						_exportMethod.SplitNumEntries = 0;
						_exportMethod.ConcurrentProcesses = 0;
					}
					else if (rdoSplitMerchandise.Checked)
					{
						_exportMethod.SplitType = eExportSplitType.Merchandise;
						_exportMethod.SplitNumEntries = 0;
						//Begin TT#307 - JScott - Field overflow error (Number of entries)
						//_exportMethod.ConcurrentProcesses = Convert.ToInt16(txtConcurrentProcesses.Text);
						_exportMethod.ConcurrentProcesses = Convert.ToInt32(txtConcurrentProcesses.Text);
						//End TT#307 - JScott - Field overflow error (Number of entries)
					}
					else
					{
						_exportMethod.SplitType = eExportSplitType.NumEntries;
						//Begin TT#307 - JScott - Field overflow error (Number of entries)
						//_exportMethod.SplitNumEntries = Convert.ToInt16(txtSplitNumEntries.Text);
						_exportMethod.SplitNumEntries = Convert.ToInt32(txtSplitNumEntries.Text);
						//End TT#307 - JScott - Field overflow error (Number of entries)
						_exportMethod.ConcurrentProcesses = 0;
					}

					if (chkCreateFlagFile.Checked)
					{
						_exportMethod.CreateFlagFile = true;
						_exportMethod.FlagFileExtension = txtFlagSuffix.Text.Trim();
					}
					else
					{
						_exportMethod.CreateFlagFile = false;
						_exportMethod.FlagFileExtension = string.Empty;
					}

					if (chkCreateEndFile.Checked)
					{
						_exportMethod.CreateEndFile = true;
						_exportMethod.EndFileExtension = txtEndSuffix.Text.Trim();
					}
					else
					{
						_exportMethod.CreateEndFile = false;
						_exportMethod.EndFileExtension = string.Empty;
					}
				}

				if (rdoChain.Checked)
				{
					_exportMethod.SelectableVariableList = (ArrayList)_chainSelectableVariableList.Clone();
				}
				else
				{
					_exportMethod.SelectableVariableList = (ArrayList)_storeSelectableVariableList.Clone();
				}

				// BEGIN Override Low Level Enhancement
				//versOverList = new ArrayList();

				//foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList)
				//{
				//    if (lvop.VersionIsOverridden || lvop.Exclude)
				//    {
				//        if (lvop.VersionProfile != null)
				//        {
				//            verRID = lvop.VersionProfile.Key;
				//        }
				//        else
				//        {
				//            verRID = _exportMethod.VersionRID;
				//        }

				//        versOverList.Add(new ForecastExportMethodVersionOverrideEntry(_exportMethod.Key, lvop.NodeProfile.Key, verRID, lvop.Exclude));
				//    }
				//}

				//_exportMethod.VersionOverrideList = versOverList;  
				// END Override Low Level Enhancement
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
//Begin Track #4942 - JScott - Correct problems in Export Method
//					_chainVarChooser.ResetChangedFlag();
//					_storeVarChooser.ResetChangedFlag();
					if (_chainVarChooser != null)
					{
						_chainVarChooser.ResetChangedFlag();
					}

					if (_storeVarChooser != null)
					{
						_storeVarChooser.ResetChangedFlag();
					}
//End Track #4942 - JScott - Correct problems in Export Method
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
				ErrorProvider.SetError(txtDelimeter, string.Empty);
//Begin Track #4942 - JScott - Correct problems in Export Method
				ErrorProvider.SetError(txtCSVFileSuffix, string.Empty);
//End Track #4942 - JScott - Correct problems in Export Method
				ErrorProvider.SetError(txtFilePath, string.Empty);
				ErrorProvider.SetError(txtSplitNumEntries, string.Empty);
				ErrorProvider.SetError(txtConcurrentProcesses, string.Empty);
				ErrorProvider.SetError(txtFlagSuffix, string.Empty);
				ErrorProvider.SetError(txtEndSuffix, string.Empty);

				if (txtMerchandise.Text.Trim() == string.Empty) 
				{
					ErrorProvider.SetError(txtMerchandise, MIDText.GetTextOnly(eMIDTextCode.msg_MethodMerchandiseRequired));
					retCode = false;
				}

				if (cboVersion.SelectedValue == null || Convert.ToInt32(cboVersion.SelectedValue) == Include.NoRID)
				{
					ErrorProvider.SetError(cboVersion, MIDText.GetTextOnly(eMIDTextCode.msg_MethodVersionRequired));
					retCode = false;
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
				}
				else
				{
					if (!_storeVarChooser.ValidateData())
					{
						retCode = false;
					}
				}
				
				if (!chkDefaultSettings.Checked)
				{
					if (rdoCSV.Checked && txtDelimeter.Text.Trim().Length != 1)
					{
						ErrorProvider.SetError(txtDelimeter, MIDText.GetTextOnly(eMIDTextCode.msg_DelimeterRequired));
						retCode = false;
					}

					if (txtFilePath.Text.Trim() == string.Empty)
					{
						ErrorProvider.SetError(txtFilePath, MIDText.GetTextOnly(eMIDTextCode.msg_FilePathRequired));
						retCode = false;
					}

					if (rdoSplitNumEntries.Checked && (txtSplitNumEntries.Text.Trim() == string.Empty || Convert.ToInt32(txtSplitNumEntries.Text) <= 0))
					{
						ErrorProvider.SetError(txtSplitNumEntries, MIDText.GetTextOnly(eMIDTextCode.msg_NumberOfEntriesRequired));
						retCode = false;
					}

					if (rdoSplitMerchandise.Checked && (txtConcurrentProcesses.Text.Trim() == string.Empty || Convert.ToInt32(txtConcurrentProcesses.Text) <= 0))
					{
						ErrorProvider.SetError(txtConcurrentProcesses, MIDText.GetTextOnly(eMIDTextCode.msg_ConcurrentProcessesRequired));
						retCode = false;
					}

					if (chkCreateFlagFile.Checked && txtFlagSuffix.Text.Trim() == string.Empty)
					{
						ErrorProvider.SetError(txtFlagSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_FlagSuffixRequired));
						retCode = false;
					}

					if (chkCreateEndFile.Checked && txtEndSuffix.Text.Trim() == string.Empty)
					{
						ErrorProvider.SetError(txtEndSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_EndSuffixRequired));
						retCode = false;
					}

//Begin Track #4942 - JScott - Correct problems in Export Method
					if (chkCreateFlagFile.Checked && txtFlagSuffix.Text.Trim() == txtCSVFileSuffix.Text.Trim())
					{
						ErrorProvider.SetError(txtFlagSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						ErrorProvider.SetError(txtCSVFileSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						retCode = false;
					}

					if (chkCreateEndFile.Checked && txtEndSuffix.Text.Trim() == txtCSVFileSuffix.Text.Trim())
					{
						ErrorProvider.SetError(txtEndSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						ErrorProvider.SetError(txtCSVFileSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						retCode = false;
					}

//End Track #4942 - JScott - Correct problems in Export Method
					if (chkCreateFlagFile.Checked && chkCreateEndFile.Checked && txtFlagSuffix.Text.Trim() == txtEndSuffix.Text.Trim())
					{
						ErrorProvider.SetError(txtFlagSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						ErrorProvider.SetError(txtEndSuffix, MIDText.GetTextOnly(eMIDTextCode.msg_SuffixesMustBeUnique));
						retCode = false;
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
					// Begin MID Track 4858 - JSmith - Security changes
//					ErrorProvider.SetError (txtMethodName,WorkflowMethodNameMessage);
					ErrorProvider.SetError (txtName,WorkflowMethodNameMessage);
					// End MID Track 4858
				}
				else
				{
					// Begin MID Track 4858 - JSmith - Security changes
//					ErrorProvider.SetError (txtMethodName,string.Empty);
					ErrorProvider.SetError (txtName,string.Empty);
					// End MID Track 4858
				}
				if (!WorkflowMethodDescriptionValid)
				{
					// Begin MID Track 4858 - JSmith - Security changes
//					ErrorProvider.SetError (txtMethodDesc,WorkflowMethodDescriptionMessage);
					ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
					// End MID Track 4858
				}
				else
				{
					// Begin MID Track 4858 - JSmith - Security changes
//					ErrorProvider.SetError (txtMethodDesc,string.Empty);
					ErrorProvider.SetError (txtDesc,string.Empty);
					// End MID Track 4858
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
				ABM = _exportMethod;
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

        // Begin TT#719 - JSmith - Unhandled exception when change from user to global
        override protected void BuildAttributeList()
        {
        }
        // End TT#719

		private void SetText()
		{
			try
			{
				if (_exportMethod.Method_Change_Type == eChangeType.update)
				{
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}
				else
				{
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
				}
	
				this.grpCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Criteria);
				this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				this.lblVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod);
				this.lblStoreFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				this.grpLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevels);
				this.btnOverrideLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
				this.rdoChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ChainData);
				this.rdoStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_StoreData);
				this.chkShowIneligible.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ShowIneligible);
				this.lblOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Options);
				this.tabOptions.TabPages[0].Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Variables);
				this.tabOptions.TabPages[1].Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Format);
				this.chkDefaultSettings.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_UseDefaults);
				this.grpFileFormat.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_FileFormat);
				this.rdoCSV.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_CSV);
				this.rdoXML.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_XML);
				this.lblDelimeter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Delimeter);
//Begin Track #4942 - JScott - Correct problems in Export Method
//				this.grpValueType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ValueType);
				this.lblCSVFileSuffix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_CSVSuffix);
				this.grpOutputFormat.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_OutputFormat);
				this.lblDateFormat.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_DateType);
				this.rdoCalendar.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Calendar);
				this.rdoFiscal.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_Fiscal);
//End Track #4942 - JScott - Correct problems in Export Method
				this.chkPreinitValues.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_PreinitValues);
				this.ttpToolTip.SetToolTip(this.chkPreinitValues, MIDText.GetTextOnly(eMIDTextCode.tt_ExportMethod_PreinitValues));
				//Begin Track #5395 - JScott - Add ability to discard zero values in Export
				this.chkExcludeZeroValues.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ExcludeZeroValues);
				this.ttpToolTip.SetToolTip(this.chkExcludeZeroValues, MIDText.GetTextOnly(eMIDTextCode.tt_ExportMethod_ExcludeZeroValues));
				//End Track #5395 - JScott - Add ability to discard zero values in Export
				this.grpOutputOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_OutputOptions);
				this.lblFilePath.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_FilePath);
				this.btnBrowseFilePath.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_BrowseFilePath);
				this.lblFileNameInfo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_FileNameInfo);
				this.lblAddToFileName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_AddToFileName);
				this.chkDateStamp.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_DateStamp);
				this.chkTimeStamp.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_TimeStamp);
				this.lblSplitFiles.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_SplitFiles);
				this.rdoSplitMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_SplitMerchandise);
				this.rdoSplitNone.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_SplitNone);
				this.rdoSplitNumEntries.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_SplitNumEntries);
				this.lblConcurrentProcesses.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_ConcurrentProcesses);
				this.chkCreateFlagFile.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_CreateFlagFile);
				this.lblFlagSuffix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_FlagSuffix);
				this.chkCreateEndFile.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_CreateEndFile);
				this.lblEndSuffix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ExportMethod_EndSuffix);
				
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

				Name = MIDText.GetTextOnly((int)eMethodType.Export);

				// Begin MID Track 4858 - JSmith - Security changes
//				if (_exportMethod.Method_Change_Type == eChangeType.add)
//				{
//					Format_Title(eDataState.New, eMIDTextCode.frm_ExportMethod, null);
//				}
//				else if (FunctionSecurity.AllowUpdate)
//				{
//					Format_Title(eDataState.Updatable, eMIDTextCode.frm_ExportMethod, _exportMethod.Name);
//				}
//				else
//				{
//					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_ExportMethod, _exportMethod.Name);
//				}
//
//				if (FunctionSecurity.AllowExecute)
//				{
//					btnProcess.Enabled = true;
//				}
//				else
//				{
//					btnProcess.Enabled = false;
//				}
				// End MID Track 4858

				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

                LoadOverrideModelComboBox(cboOverride.ComboBox, _exportMethod.OverrideLowLevelRid, _exportMethod.CustomOLL_RID);  //TT#7 - RBeck - Dynamic dropdowns

                // Begin Track #6189 - JSmith - select save and receive and invalid cast exception error
                //BuildVersionTables();
                // End Track #6189
				BindFilterComboBox();
				BindVersionComboBox();

                // Begin Track #4868 - JSmith - Variable Groupings

                //Create an App Server Transaction

                _transaction = SAB.ApplicationServerSession.CreateTransaction();
                // End Track #4868

				LoadMethodData();

				// Begin MID Track 4858 - JSmith - Security changes
//				SetReadOnly(FunctionSecurity.AllowUpdate);
				// End MID Track 4858

				if (!FunctionSecurity.AllowUpdate)
				{
					this._chainVarChooser.Enabled = false;
					this._storeVarChooser.Enabled = false;
				}

				if (_exportMethod.PlanType == ePlanType.Chain)
				{
					cboFilter.Enabled = false;
					chkShowIneligible.Enabled = false;
				}
				else
				{
					cboFilter.Enabled = true;
					chkShowIneligible.Enabled = true;
				}

				//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
				//ApplySToreChainRBSecurity();
				ApplySecurity();
				//End TT#89 - JScott - Process button disabled on new method if ver=Act

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabGenAllocMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin Track #6189 - JSmith - select save and receive and invalid cast exception error
        //private void BuildVersionTables()
        //{
        //    try
        //    {
        //        _dtChainForecastVersions = MIDEnvironment.CreateDataTable("Versions");
        //        _dtChainForecastVersions.Columns.Add("Description", typeof(string));
        //        _dtChainForecastVersions.Columns.Add("Key", typeof(int));
        //        _dtChainForecastVersions.Rows.Add(new object[] {string.Empty, Include.NoRID});

        //        _dtStoreForecastVersions = MIDEnvironment.CreateDataTable("Versions");
        //        _dtStoreForecastVersions.Columns.Add("Description", typeof(string));
        //        _dtStoreForecastVersions.Columns.Add("Key", typeof(int));
        //        _dtStoreForecastVersions.Rows.Add(new object[] {string.Empty, Include.NoRID});

        //        _chainLowLevelVersionList = new ProfileList(eProfileType.Version);
        //        _storeLowLevelVersionList = new ProfileList(eProfileType.Version);

        //        foreach (VersionProfile verProf in _masterVersionProfList)
        //        {
        //            if ((!verProf.StoreSecurity.AccessDenied &&
        //                (!verProf.IsBlendedVersion || verProf.ForecastVersionRID == verProf.Key))
        //                || _exportMethod.VersionRID == verProf.Key)	// Track #5852
        //            {
        //                _dtStoreForecastVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
        //                _storeLowLevelVersionList.Add(verProf);
        //            }

        //            if ((!verProf.ChainSecurity.AccessDenied &&
        //                (!verProf.IsBlendedVersion || verProf.ForecastVersionRID == verProf.Key))
        //                || _exportMethod.VersionRID == verProf.Key)	// Track #5852

        //            {
        //                _dtChainForecastVersions.Rows.Add(new object[] {verProf.Description, verProf.Key});
        //                _chainLowLevelVersionList.Add(verProf);
        //            }
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        //private void BindVersionComboBox()
        //{
        //    try
        //    {
        //        if (_exportMethod.PlanType == ePlanType.Chain)
        //        {
        //            this.cboVersion.DataSource = _dtChainForecastVersions;
        //        }
        //        else
        //        {
        //            this.cboVersion.DataSource = _dtStoreForecastVersions;
        //        }

        //        this.cboVersion.DisplayMember = "Description";
        //        this.cboVersion.ValueMember = "Key";
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}
       
		private void BindVersionComboBox()
		{
            ProfileList versionProfList = null;
			try
			{
                _chainLowLevelVersionList = base.GetForecastVersionList(eSecuritySelectType.View, eSecurityTypes.Chain, false, _exportMethod.VersionRID, false);	
                _storeLowLevelVersionList = base.GetForecastVersionList(eSecuritySelectType.View, eSecurityTypes.Store, false, _exportMethod.VersionRID, false);	
				if (_exportMethod.PlanType == ePlanType.Chain)
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
        // End Track #6189

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
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

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

		private void LoadMethodData()
		{
			HierarchyNodeProfile hnp;
			DateRangeProfile drp;

			try
			{
				drsTimePeriod.DateRangeRID = Include.UndefinedCalendarDateRange;
				drsTimePeriod.SetImage(null);

                //Begin Track #5858 - KJohnson - Validating store security only
                if (_exportMethod.PlanType == ePlanType.Chain)
                {
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _exportMethod.GlobalUserType == eGlobalUserType.User);
                    // End TT#44
                    txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_ExportMethod, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
                }
                else
                {
                    // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                    //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                    cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _exportMethod.GlobalUserType == eGlobalUserType.User);  //TT#7 - RBeck - Dynamic dropdowns
                    // End TT#44
                    txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_ExportMethod, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                }
                //End Track #5858

				// Begin MID Track 4858 - JSmith - Security changes
//				if (_exportMethod.Method_Change_Type != eChangeType.add)
//				{
//					this.txtMethodName.Text = _exportMethod.Name;
//					this.txtMethodDesc.Text = _exportMethod.Method_Description;
//								
//					if (_exportMethod.User_RID == Include.GetGlobalUserRID())
//					{
//						rdoGlobal.Checked = true;
//					}
//					else
//					{
//						rdoUser.Checked = true;
//					}
//				}
				if (_exportMethod.Method_Change_Type != eChangeType.add)
				{
					this.txtName.Text = _exportMethod.Name;
					this.txtDesc.Text = _exportMethod.Method_Description;
				}
				// End MID Track 4858

				if (_exportMethod.PlanType == ePlanType.Chain)
				{
					rdoChain.Checked = true;
					_currentVersionProfList = _chainLowLevelVersionList;
                    //cboVersion.DataSource = _dtChainForecastVersions;
				}
				else
				{
					rdoStore.Checked = true;
					_currentVersionProfList = _storeLowLevelVersionList;
                    //cboVersion.DataSource = _dtStoreForecastVersions;
				}

				if (_exportMethod.HierarchyRID > 0)
				{
					//Begin Track #5378 - color and size not qualified
//					hnp = SAB.HierarchyServerSession.GetNodeData(_exportMethod.HierarchyRID);
                    hnp = SAB.HierarchyServerSession.GetNodeData(_exportMethod.HierarchyRID, true, true);
					//End Track #5378

                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtMerchandise.Text = hnp.Text;
                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
                    //End Track #5858

					cboLowLevel.Enabled = true;
                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox);  //TT#7 - RBeck - Dynamic dropdowns
				}
				else
				{
                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtMerchandise.Text = string.Empty;
                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;
                    //End Track #5858

					cboLowLevel.Enabled = false;
				}

				if (_exportMethod.VersionRID > 0)
				{
					cboVersion.SelectedValue = _exportMethod.VersionRID;
				}
				//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
				else
				{
					_exportMethod.VersionRID = (int)this.cboVersion.SelectedValue;
				}
				//End TT#89 - JScott - Process button disabled on new method if ver=Act

				if (_exportMethod.DateRangeRID > 0 && _exportMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					drp = SAB.ClientServerSession.Calendar.GetDateRange(_exportMethod.DateRangeRID);
					LoadDateRangeSelector(drsTimePeriod, drp);
				}
 			 
				if (_exportMethod.FilterRID > 0 && _exportMethod.FilterRID != Include.UndefinedStoreFilter)
				{
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_exportMethod.FilterRID, -1, ""));
				}

				chkLowLevels.Checked = _exportMethod.LowLevels;
				grpLowLevels.Enabled = chkLowLevels.Checked;

				chkLowLevelsOnly.Checked = _exportMethod.LowLevelsOnly;

				cboLowLevel.SelectedIndex = cboLowLevel.Items.IndexOf(new LowLevelCombo(_exportMethod.LowLevelsType, _exportMethod.LowLevelOffset, _exportMethod.LowLevelSequence, ""));

				_storeSelectableVariableList = new ArrayList();
				_chainSelectableVariableList = new ArrayList();

				foreach (RowColProfileHeader rowColHdr in _exportMethod.SelectableVariableList)
				{
					_chainSelectableVariableList.Add(rowColHdr.Copy());
					_storeSelectableVariableList.Add(rowColHdr.Copy());
				}

				UpdateSelectableList(ePlanType.Chain, _chainSelectableVariableList);
				UpdateSelectableList(ePlanType.Store, _storeSelectableVariableList);

// Begin Track #4868 - JSmith - Variable Groupings
                //_chainVarChooser = new RowColChooserOrderPanel(_chainSelectableVariableList, true);
                //_storeVarChooser = new RowColChooserOrderPanel(_storeSelectableVariableList, true);
                _chainVarChooser = new RowColChooserOrderPanel(_chainSelectableVariableList, true, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());
                _storeVarChooser = new RowColChooserOrderPanel(_storeSelectableVariableList, true, _transaction.PlanComputations.PlanVariables.GetVariableGroupings());
// End Track #4868

				tpgVariables.Controls.Add(_chainVarChooser);
				tpgVariables.Controls.SetChildIndex(_chainVarChooser, 0);
				_chainVarChooser.Dock = DockStyle.Fill;
				_chainVarChooser.FillControl();

				tpgVariables.Controls.Add(_storeVarChooser);
				tpgVariables.Controls.SetChildIndex(_storeVarChooser, 0);
				_storeVarChooser.Dock = DockStyle.Fill;
				_storeVarChooser.FillControl();

				if (_exportMethod.PlanType == ePlanType.Chain)
				{
					_storeVarChooser.Visible = false;
				}
				else
				{
					_chainVarChooser.Visible = false;
				}

				chkShowIneligible.Checked = _exportMethod.ShowIneligible;
				chkDefaultSettings.Checked = _exportMethod.UseDefaultSettings;

				grpFileFormat.Enabled = !chkDefaultSettings.Checked;
//Begin Track #4942 - JScott - Correct problems in Export Method
//				grpValueType.Enabled = !chkDefaultSettings.Checked;
				grpOutputFormat.Enabled = !chkDefaultSettings.Checked;
//End Track #4942 - JScott - Correct problems in Export Method
				grpOutputOptions.Enabled = !chkDefaultSettings.Checked;

				LoadExportOptions();

				LoadWorkflows();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		private void LoadExportOptions()
		{
			try
			{
				ErrorProvider.SetError(txtDelimeter, string.Empty);
//Begin Track #4942 - JScott - Correct problems in Export Method
				ErrorProvider.SetError(txtCSVFileSuffix, string.Empty);
//End Track #4942 - JScott - Correct problems in Export Method
				ErrorProvider.SetError(txtFilePath, string.Empty);
				ErrorProvider.SetError(txtSplitNumEntries, string.Empty);
				ErrorProvider.SetError(txtConcurrentProcesses, string.Empty);
				ErrorProvider.SetError(txtFlagSuffix, string.Empty);
				ErrorProvider.SetError(txtEndSuffix, string.Empty);

				switch (_exportMethod.ExportType)
				{
                    case DataCommon.eExportType.CSV:                      // Added DataCommon for 3.0 compile
						rdoCSV.Checked = true;
						txtDelimeter.Enabled = true;
//Begin Track #4942 - JScott - Correct problems in Export Method
						txtCSVFileSuffix.Enabled = true;
//End Track #4942 - JScott - Correct problems in Export Method
						break;
                    case DataCommon.eExportType.XML:                      // Added DataCommon for 3.0 compile
						rdoXML.Checked = true;
						txtDelimeter.Enabled = false;
//Begin Track #4942 - JScott - Correct problems in Export Method
						txtCSVFileSuffix.Enabled = false;
//End Track #4942 - JScott - Correct problems in Export Method
						break;
				}

				txtDelimeter.Text = _exportMethod.Delimeter;

//Begin Track #4942 - JScott - Correct problems in Export Method
				txtCSVFileSuffix.Text = _exportMethod.CSVFileExtension;

				if (_exportMethod.DateType == eExportDateType.Calendar)
				{
					rdoCalendar.Checked = true;
				}
				else
				{
					rdoFiscal.Checked = true;
				}

//End Track #4942 - JScott - Correct problems in Export Method
				chkPreinitValues.Checked = _exportMethod.PreinitValues;
				//Begin Track #5395 - JScott - Add ability to discard zero values in Export
				chkExcludeZeroValues.Checked = _exportMethod.ExcludeZeroValues;
				//End Track #5395 - JScott - Add ability to discard zero values in Export

				txtFilePath.Text = _exportMethod.FilePath;

				chkDateStamp.Checked = _exportMethod.AddDateStamp;

				chkTimeStamp.Checked = _exportMethod.AddTimeStamp;

				switch (_exportMethod.SplitType)
				{
					case eExportSplitType.None :
						rdoSplitNone.Checked = true;
						txtSplitNumEntries.Enabled = false;
						txtConcurrentProcesses.Enabled = false;
						break;
					case eExportSplitType.Merchandise :
						rdoSplitMerchandise.Checked = true;
						txtSplitNumEntries.Enabled = false;
						txtConcurrentProcesses.Enabled = true;
						break;
					case eExportSplitType.NumEntries :
						rdoSplitNumEntries.Checked = true;
						txtSplitNumEntries.Enabled = true;
						txtConcurrentProcesses.Enabled = false;
						break;
				}

				txtSplitNumEntries.Text = Convert.ToString(_exportMethod.SplitNumEntries, CultureInfo.CurrentUICulture);
				txtConcurrentProcesses.Text = Convert.ToString(_exportMethod.ConcurrentProcesses, CultureInfo.CurrentUICulture);

				chkCreateFlagFile.Checked = _exportMethod.CreateFlagFile;
				txtFlagSuffix.Enabled = _exportMethod.CreateFlagFile;
				txtFlagSuffix.Text = _exportMethod.FlagFileExtension;

				chkCreateEndFile.Checked = _exportMethod.CreateEndFile;
				txtEndSuffix.Enabled = _exportMethod.CreateEndFile;
				txtEndSuffix.Text = _exportMethod.EndFileExtension;
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

		// Begin MID Track 4858 - JSmith - Security changes
//		/// <summary>
//		/// Set text in the title bar of the window as it's changed in the textbox - [New] - if blank.
//		/// </summary>
//		/// <param name="sender"></param>
//		/// <param name="e"></param>
//
//		private void txtMethodName_TextChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (FormLoaded)
//				{
//					if (_exportMethod.Method_Change_Type == eChangeType.add)
//					{
//						Format_Title(eDataState.New, eMIDTextCode.frm_ExportMethod, txtMethodName.Text);
//					}
//					else
//					{
//						Format_Title(eDataState.Updatable, eMIDTextCode.frm_ExportMethod, txtMethodName.Text);
//					}
//
//					ChangePending = true;
//					NameChanged = true;
//				}
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}
//
//		private void rdoGlobal_CheckedChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (FormLoaded)
//				{
//					ChangePending = true;
//				}
//
//				if (rdoGlobal.Checked)
//				{
//					FunctionSecurity = _globalSecurity;
//				}
//
//				ApplySecurity();
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}
//
//		private void rdoUser_CheckedChanged(object sender, System.EventArgs e)
//		{
//			try
//			{
//				if (FormLoaded)
//				{
//					ChangePending = true;
//				}
//
//				if (rdoUser.Checked)
//				{
//					FunctionSecurity = _userSecurity;
//				}
//
//				ApplySecurity();
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}
		// End MID Track 4858

		private void rdoChain_CheckedChanged(object sender, System.EventArgs e)
		{
            HierarchyNodeProfile hnp = null;
			try
			{
				if (FormLoaded)
				{
					if (rdoChain.Checked)
					{
                        //Begin Track #5858 - KJohnson - Validating store security only
                        if (txtMerchandise.Tag != null && txtMerchandise.Tag is MIDTag)
                        {
                            hnp = (HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData;
                        }
                        txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_ExportMethod, eSecurityTypes.Chain, eSecuritySelectType.View | eSecuritySelectType.Update);
                        //txtMerchandise.Text = string.Empty;
                        //((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;
                        ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
                        //End Track #5858

						_currentVersionProfList = _chainLowLevelVersionList;
                        // Begin Track #6189 - JSmith - select save and receive and invalid cast exception error
                        //cboVersion.DataSource = _dtChainForecastVersions;
                        cboVersion.DataSource = _chainLowLevelVersionList.ArrayList;
                        // End Track #6189
						cboFilter.Enabled = false;
						chkShowIneligible.Enabled = false;
						_chainVarChooser.Visible = true;
						_storeVarChooser.Visible = false;
						_exportMethod.PlanType = ePlanType.Chain;

						//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
						//// Begion Track #5852
						//ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
						//ApplySToreChainRBSecurity();   
						////  End Track #5852

						ApplySecurity();
						//End TT#89 - JScott - Process button disabled on new method if ver=Act
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
                        //Begin Track #5858 - KJohnson - Validating store security only
                        if (txtMerchandise.Tag != null && txtMerchandise.Tag is MIDTag)
                        {
                            hnp = (HierarchyNodeProfile)((MIDTag)(txtMerchandise.Tag)).MIDTagData;
                        }
                        txtMerchandise.Tag = new MIDMerchandiseTextBoxTag(SAB, txtMerchandise, eMIDControlCode.form_ExportMethod, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                        //txtMerchandise.Text = string.Empty;
                        //((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;
                        ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
                        //End Track #5858

						_currentVersionProfList = _storeLowLevelVersionList;
                        // Begin Track #6189 - JSmith - select save and receive and invalid cast exception error
                        //cboVersion.DataSource = _dtStoreForecastVersions;
                        cboVersion.DataSource = _storeLowLevelVersionList.ArrayList;
                        // End Track #6189
						cboFilter.Enabled = true;
						chkShowIneligible.Enabled = true;
						_chainVarChooser.Visible = false;
						_storeVarChooser.Visible = true;
						_exportMethod.PlanType = ePlanType.Store;

						//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
						//// Begion Track #5852
						//ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
						//ApplySToreChainRBSecurity();   
						////  End Track #5852

						ApplySecurity();
						//End TT#89 - JScott - Process button disabled on new method if ver=Act
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

        //TT#695 - Begin - MD - RBeck - Drag and drop of size merchandise causes error  
            TreeNodeClipboardList cbList;
            if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
            {
                cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                if (hnp == null || hnp.LevelType == eHierarchyLevelType.Size)
                {
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
        //TT#695  - End - MD - RBeck - Drag and drop of size merchandise causes error
            //Image_DragOver(sender, e);
        }

        private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            //IDataObject data;
            //ClipboardProfile cbp;
            //HierarchyClipboardData MIDTreeNode_cbd;

            try
            {
                //Begin Track #5858 - Kjohnson - Validating store security only
                bool isSuccessfull = ((MIDTextBoxTag)(((TextBox)sender).Tag)).TextBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData;

                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox);  //TT#7 - RBeck - Dynamic dropdowns
                    _exportMethod.HierarchyRID = hnp.Key;

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858 - Kjohnson

                //// Create a new instance of the DataObject interface.

                //data = Clipboard.GetDataObject();

                ////If the data is ClipboardProfile, then retrieve the data

                //if (data.GetDataPresent(ClipboardProfile.Format.Name))
                //{
                //    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);

                //    if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
                //    {
                //        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
                //        {
                //            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
                //            //Begin Track #5378 - color and size not qualified
                //            //							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                //            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key, false, true);
                //            //End Track #5378

                //            //Begin Track #5858 - KJohnson - Validating store security only
                //            ((TextBox)sender).Text = hnp.Text;
                //            ((MIDTag)(((TextBox)sender).Tag)).MIDTagData = hnp;
                //            //End Track #5858

                //            PopulateLowLevelList(hnp, cboLowLevel);

                //            //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
                //            _exportMethod.HierarchyRID = hnp.Key;
                //            //ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
                //            //ApplySToreChainRBSecurity();   // Track #5852
                //            //End Track #5719 - JScott - Export Method will not allow me to export Act Version

                //            ApplySecurity();

                //            ////Begin Track #5858 - JSmith - Validating store security only
                //            //if (rdoChain.Checked)
                //            //{
                //            //    base.ValidatePlanNodeSecurity(txtMerchandise, true, eSecurityTypes.Chain);
                //            //}
                //            //else
                //            //{
                //            //    base.ValidatePlanNodeSecurity(txtMerchandise, true, eSecurityTypes.Store);
                //            //}
                //            ////End Track #5858

                //            ChangePending = true;
                //        }
                //        else
                //        {
                //            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //    }
                //}
                //else
                //{
                //    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
                //}
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
//            string errorMessage;

//            try
//            {
//                if (((TextBox)sender).Text == string.Empty && ((MIDTag)(((TextBox)sender).Tag)).MIDTagData != null)
//                {
//                    //Begin Track #5858 - KJohnson - Validating store security only
//                    txtMerchandise.Text = string.Empty;
//                    ((MIDTag)(txtMerchandise.Tag)).MIDTagData = null;
//                    //End Track #5858

//                    PopulateLowLevelList(null, cboLowLevel);
//                }
//                else
//                {
//                    if (_textChanged)
//                    {
//                        _textChanged = false;

//                        HierarchyNodeProfile hnp = GetNodeProfile(((TextBox)sender).Text);
//                        if (hnp.Key == Include.NoRID)
//                        {
//                            _priorError = true;

//                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
//                                ((TextBox)sender).Text );	
//                            ErrorProvider.SetError((TextBox)sender,errorMessage);
//                            MessageBox.Show( errorMessage);

//                            e.Cancel = true;
//                        }
//                        else 
//                        {
//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            txtMerchandise.Text = hnp.Text;
//                            ((MIDTag)(txtMerchandise.Tag)).MIDTagData = hnp;
//                            //End Track #5858

////Begin Track #4942 - JScott - Correct problems in Export Method
//                            PopulateLowLevelList(hnp, cboLowLevel);
////End Track #4942 - JScott - Correct problems in Export Method

//                            //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
//                            _exportMethod.HierarchyRID = hnp.Key;
//                            ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
//                            ApplySToreChainRBSecurity();   // Track #5852

//                            //End Track #5719 - JScott - Export Method will not allow me to export Act Version
//                            ChangePending = true;
//                        }	
//                    }
//                    else if (_priorError)
//                    {
//                        if (((MIDTag)(((TextBox)sender).Tag)).MIDTagData == null)
//                        {
//                            ((TextBox)sender).Text = string.Empty;
//                        }
//                        else
//                        {
//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            ((TextBox)sender).Text = ((HierarchyNodeProfile)((MIDTag)(((TextBox)sender).Tag)).MIDTagData).Text;
//                            //End Track #5858
//                        }
//                    }
//                }
//            }
//            catch (Exception err)
//            {
//                HandleException(err);
//            }
		}
	
		private void txtMerchandise_Validated(object sender, System.EventArgs e)
		{
			try
			{
				_textChanged = false;
				_priorError = false;

                //Begin Track #5858 - KJohnson- Validating store security only
                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
					//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
					//PopulateLowLevelList(null, cboLowLevel);
					_nodeRID = Include.NoRID;

                    PopulateLowLevelList(null, cboLowLevel.ComboBox);  //TT#7 - RBeck - Dynamic dropdowns

					_exportMethod.HierarchyRID = Include.NoRID;

					ChangePending = true;
					ApplySecurity();
					//End TT#89 - JScott - Process button disabled on new method if ver=Act
                }
                else
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    _nodeRID = hnp.Key;

                    //Begin Track #4942 - JScott - Correct problems in Export Method
                    PopulateLowLevelList(hnp, cboLowLevel.ComboBox);  //TT#7 - RBeck - Dynamic dropdowns
                    //End Track #4942 - JScott - Correct problems in Export Method

                    //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
                    _exportMethod.HierarchyRID = hnp.Key;
                    //ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
                    //ApplySToreChainRBSecurity();   // Track #5852
                    //End Track #5719 - JScott - Export Method will not allow me to export Act Version

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858
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
					//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
					////Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
					//_exportMethod.VersionRID = (int)this.cboVersion.SelectedValue;
					//ApplyCanUpdate(ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID));
					//ApplySToreChainRBSecurity();   // Track #5852

					////End Track #5719 - JScott - Export Method will not allow me to export Act Version
					//ChangePending = true;
					_exportMethod.VersionRID = (int)this.cboVersion.SelectedValue;

					ChangePending = true;
					ApplySecurity();
					//End TT#89 - JScott - Process button disabled on new method if ver=Act
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
					// BEGIN Override Low Level Enhancement
					//_lowlevelVersionOverrideList.Clear();
					// END Override Low Level Enhancement
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
//Begin Track #4942 - JScott - Correct problems in Export Method

				if (_exportMethod.DateRangeRID > 0 && _exportMethod.DateRangeRID != Include.UndefinedCalendarDateRange)
				{
					frm.DateRangeRID = _exportMethod.DateRangeRID;
				}

				frm.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;

				frm.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
				frm.AllowDynamicToStoreOpen = false;
//End Track #4942 - JScott - Correct problems in Export Method
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		/// <summary>
		/// After selection is made on midDateRangeSelector - Export
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

                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                if (cboFilter.SelectedIndex != -1)
                {
                    if (((FilterNameCombo)cboFilter.SelectedItem).FilterRID == Include.Undefined)
                    {
                        cboFilter.SelectedIndex = -1;
                    }
                }
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
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
            //Begin Track #5858 - Kjohnson - Validating store security only
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
            //End Track #5858
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
			//frmOverrideLowLevelVersions frmOverrideLowLevelVersions;
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


				// BEGIN Override Low Level Enhancements
				System.Windows.Forms.Form parentForm;
				parentForm = this.MdiParent;

				object[] args = null;

                //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
				//System.Windows.Forms.Form frm;
                //End tt#700

				// Begin Track #5909 - stodd
				FunctionSecurityProfile methodSecurity;
				if (radGlobal.Checked)
					methodSecurity = GlobalSecurity;
				else
					methodSecurity = UserSecurity;
				args = new object[] { SAB, _exportMethod.OverrideLowLevelRid, _exportMethod.HierarchyRID, _exportMethod.VersionRID, lowLevelText, _exportMethod.CustomOLL_RID, methodSecurity };
				// End Track #5909 - stodd

                //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                
                //frm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                //parentForm = this.MdiParent;
                //frm.MdiParent = parentForm;
                //frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                //frm.Show();
                //frm.BringToFront();
                //((frmOverrideLowLevelModel)frm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);

                _overrideLowLevelfrm = GetForm(typeof(frmOverrideLowLevelModel), args, false);
                parentForm = this.MdiParent;
                _overrideLowLevelfrm.MdiParent = parentForm;
                _overrideLowLevelfrm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                _overrideLowLevelfrm.Show();
                _overrideLowLevelfrm.BringToFront();
                ((frmOverrideLowLevelModel)_overrideLowLevelfrm).OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);
                
                //end tt#700

				//if (_lowlevelVersionOverrideList.Count == 0)
				//{
				//    if (txtMerchandise.Tag == null)
				//    {
				//        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_HierarchyRequired), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//        return;
				//    }

				//    hnp = (HierarchyNodeProfile)txtMerchandise.Tag;

				//    if (cboLowLevel.SelectedIndex == -1)
				//    {
				//        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				//        return;
				//    }

				//    _exportMethod.PopulateVersionOverrideList(
				//        SAB,
				//        _lowlevelVersionOverrideList, 
				//        ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelType,
				//        ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelOffset,
				//        ((LowLevelCombo)cboLowLevel.SelectedItem).LowLevelSequence,
				//        ((HierarchyNodeProfile)txtMerchandise.Tag).Key,
				//        verRID);
				//}

				//frmOverrideLowLevelVersions = new frmOverrideLowLevelVersions(SAB, _lowlevelVersionOverrideList, _currentVersionProfList, (VersionProfile)_currentVersionProfList.FindKey(verRID), false, false);
				//frmOverrideLowLevelVersions.ShowDialog();

				//ChangePending = true;
				// END Override Low Level Enhancements

			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

        //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
        System.Windows.Forms.Form _overrideLowLevelfrm;
        //End tt#700

		// BEGIN Override Low Level Enhancements
		private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (_exportMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_exportMethod.OverrideLowLevelRid = e.aOllRid;
					if (_exportMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_exportMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_exportMethod.Key, _exportMethod.CustomOLL_RID);
					}

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                    {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _exportMethod.CustomOLL_RID);  //TT#7 - RBeck - Dynamic dropdowns
                    }

                    _overrideLowLevelfrm = null;
                    // End tt#700

					//LoadOverrideModelComboBox(cboOverride, e.aOllRid, _exportMethod.CustomOLL_RID);
				}
			}
			catch
			{
				throw;
			}
		}
		// END Override Low Level Enhancements

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

		private void chkDefaultSettings_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					grpFileFormat.Enabled = !chkDefaultSettings.Checked;
//Begin Track #4942 - JScott - Correct problems in Export Method
//					grpValueType.Enabled = !chkDefaultSettings.Checked;
					grpOutputFormat.Enabled = !chkDefaultSettings.Checked;
//End Track #4942 - JScott - Correct problems in Export Method
					grpOutputOptions.Enabled = !chkDefaultSettings.Checked;

					_exportMethod.ForceDefaultFormatSettings = chkDefaultSettings.Checked;

					_loadingExportOptions = true;
					LoadExportOptions();
					_loadingExportOptions = false;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void rdoXML_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void rdoCSV_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					if (rdoCSV.Checked)
					{
						txtDelimeter.Enabled = true;
//Begin Track #4942 - JScott - Correct problems in Export Method
						txtCSVFileSuffix.Enabled = true;
//End Track #4942 - JScott - Correct problems in Export Method
					}
					else
					{
						txtDelimeter.Enabled = false;
//Begin Track #4942 - JScott - Correct problems in Export Method
						txtCSVFileSuffix.Enabled = false;
//End Track #4942 - JScott - Correct problems in Export Method
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void chkPreinitValues_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		//Begin Track #5395 - JScott - Add ability to discard zero values in Export
		private void chkExcludeZeroValues_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		//End Track #5395 - JScott - Add ability to discard zero values in Export
		private void txtDelimeter_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

//Begin Track #4942 - JScott - Correct problems in Export Method
		private void txtCSVFileSuffix_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void rdoCalendar_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void rdoFiscal_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

//End Track #4942 - JScott - Correct problems in Export Method
		private void txtFilePath_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void btnBrowseFilePath_Click(object sender, System.EventArgs e)
		{
			DialogResult retCode;

			try
			{
				fbdFilePath.SelectedPath = txtFilePath.Text;
				fbdFilePath.Description = "Select the directory where the output files will be created in.";

				retCode = fbdFilePath.ShowDialog();

				if (retCode == DialogResult.OK)
				{
					txtFilePath.Text = fbdFilePath.SelectedPath;
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkDateStamp_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void chkTimeStamp_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtSplitNumEntries_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
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
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtFlagSuffix_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtEndSuffix_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded && !_loadingExportOptions)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void rdoSplitNone_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void rdoSplitMerchandise_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rdoSplitMerchandise.Checked)
					{
						txtConcurrentProcesses.Enabled = true;
					}
					else
					{
						txtConcurrentProcesses.Enabled = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void rdoSplitNumEntries_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rdoSplitNumEntries.Checked)
					{
						txtSplitNumEntries.Enabled = true;
					}
					else
					{
						txtSplitNumEntries.Enabled = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void chkCreateFlagFile_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (chkCreateFlagFile.Checked)
					{
						txtFlagSuffix.Enabled = true;
					}
					else
					{
						txtFlagSuffix.Enabled = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		private void chkCreateEndFile_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (chkCreateEndFile.Checked)
					{
						txtEndSuffix.Enabled = true;
					}
					else
					{
						txtEndSuffix.Enabled = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ProcessAction(eMethodType.Export);
//
//				// as part of the  processing we saved the info, so it should be changed to update.
//				
//				if (!ErrorFound)
//				{
//					_exportMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
//				}
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}

		protected override void Call_btnProcess_Click()
		{
			try
			{
				ProcessAction(eMethodType.Export);

				// as part of the  processing we saved the info, so it should be changed to update.
				
				if (!ErrorFound)
				{
					_exportMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);			
				}
			}
			catch (Exception err)
			{
				HandleException(err);
			}
		}

//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//                Save_Click(true);
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}
//		
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch (Exception err)
//			{
//				HandleException(err);
//			}
//		}

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
		// End MID Track 4858
	
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
                GetOTSPLANWorkflows(_exportMethod.Key, ugWorkflows);
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

                            // Begin Track #4868 - JSmith - Variable Groupings
                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                            // End Track #4868
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
                            // Begin Track #4868 - JSmith - Variable Groupings
                            if (rowHeader.Profile.GetType() == typeof(VariableProfile))
                            {
                                rowHeader.Grouping = ((VariableProfile)rowHeader.Profile).Groupings;
                            }
                            // End Track #4868
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
							aComboBox.Items.Add(
								new LowLevelCombo(eLowLevelsType.HierarchyLevel,
								//Begin Track #5866 - JScott - Matrix Balance does not work
								//0,
								i - aHierarchyNodeProfile.HomeHierarchyLevel,
								//End Track #5866 - JScott - Matrix Balance does not work
								hlp.Key,
								hlp.LevelID));
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
										//Begin Track #5866 - JScott - Matrix Balance does not work
										//0,
										i,
										//End Track #5866 - JScott - Matrix Balance does not work
										hlp.Key,
										hlp.LevelID));
								}
							}
						}

						// add offsets to comboBox
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                            //_longestBranch = _sab.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                            

                            //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

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

					// BEGIN Override Low Level Enhancement
					//_lowlevelVersionOverrideList.Clear();
					// END Override Low Level Enhancement
					_currentLowLevelNode = aHierarchyNodeProfile.Key;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		
//		private bool PopulateVersionOverrideList(int aCurrentVersionRID)
//		{
//			HierarchyNodeList hnl;
//			LowLevelVersionOverrideProfile lvop;
//			LowLevelCombo lowLevel;
//			HierarchyNodeProfile hnp;
//
//			try
//			{
//				if (txtMerchandise.Tag == null)
//				{
//					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_HierarchyRequired), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//					return false;
//				}
//
//				hnp = (HierarchyNodeProfile)txtMerchandise.Tag;
//
//				if (cboLowLevel.SelectedIndex == -1)
//				{
//					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
//					return false;
//				}
//
//				lowLevel = (LowLevelCombo)cboLowLevel.SelectedItem;
//
//				_lowlevelVersionOverrideList.Clear();
//
//				if (lowLevel.LowLevelType == eLowLevelsType.LevelOffset)
//				{
//					hnl = SAB.HierarchyServerSession.GetDescendantData(hnp.Key, lowLevel.LowLevelOffset, true, eNodeSelectType.NoVirtual);
//				}
//				else
//				{
//					hnl = SAB.HierarchyServerSession.GetDescendantDataByLevel(hnp.Key, lowLevel.LowLevelSequence, true, eNodeSelectType.NoVirtual);
//				}
//
//				foreach (HierarchyNodeProfile llhnp in hnl)
//				{
//					lvop = new LowLevelVersionOverrideProfile(llhnp.Key);
//					lvop.NodeProfile = llhnp;
//					lvop.VersionIsOverridden = false;
//					lvop.VersionProfile = null;
//					lvop.Exclude = false;
//					_lowlevelVersionOverrideList.Add(lvop);
//				}
//
//				foreach (ForecastExportMethodVersionOverrideEntry ovrrdEntry in _exportMethod.VersionOverrideList)
//				{
//					lvop = (LowLevelVersionOverrideProfile)_lowlevelVersionOverrideList.FindKey(ovrrdEntry.MerchandiseRID);
//
//					if (lvop != null)
//					{
//						if (ovrrdEntry.VersionRID != aCurrentVersionRID)
//						{
//							lvop.VersionIsOverridden = true;
//							lvop.VersionProfile = (VersionProfile)_masterVersionProfList.FindKey(ovrrdEntry.VersionRID);
//						}
//
//						lvop.Exclude = ovrrdEntry.Exclude;
//					}
//				}
//
//				return true;
//			}
//			catch (Exception err)
//			{
//				string message = err.ToString();
//				throw;
//			}
//		}

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

		// Begin MID Track 4858 - JSmith - Security changes
//		private void ApplySecurity()
		override protected bool ApplySecurity()	// track 5871 stodd
		// End MID Track 4858
		{
			bool securityOk = true; // track #5871 stodd

            ////Begin Track #5858 - JSmith - Validating store security only
            //string errorMessage = string.Empty;
            //string item = string.Empty;
            //HierarchyNodeSecurityProfile hierNodeSecurity;
            ////End Track #5858

            ////Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
            //bool canUpdateData;
            //bool canViewStore;
            //bool canViewChain;

            ////End Track #5719 - JScott - Export Method will not allow me to export Act Version
            //try
            //{
            //    //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
            //    //if (FunctionSecurity.AllowUpdate)
            //    canUpdateData = ABM.AuthorizedToUpdate(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);

            //    if (canUpdateData &&
            //        FunctionSecurity.AllowUpdate)
            //    //End Track #5719 - JScott - Export Method will not allow me to export Act Version
            //    {
            //        btnSave.Enabled = true;
            //    }
            //    else
            //    {
            //        btnSave.Enabled = false;
            //    }

            //    //Begin Track #5719 - JScott - Export Method will not allow me to export Act Version
            //    //if (FunctionSecurity.AllowExecute)
            //    if (canUpdateData &&
            //        FunctionSecurity.AllowExecute)
            //    //End Track #5719 - JScott - Export Method will not allow me to export Act Version
            //    {
            //        btnProcess.Enabled = true;
            //    }
            //    else
            //    {
            //        btnProcess.Enabled = false;
            //    }

            //    //Begin Track #5858 - JSmith - Validating store security only
            //    //ErrorProvider.SetError(txtMerchandise, string.Empty);
            //    if (_exportMethod.HierarchyRID != Include.NoRID)
            //    {
            //        if (_exportMethod.PlanType == ePlanType.Chain)
            //        {
            //            hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_exportMethod.HierarchyRID, (int)eSecurityTypes.Chain);
            //        }
            //        else
            //        {
            //            hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_exportMethod.HierarchyRID, (int)eSecurityTypes.Store);
            //        }
            //        if (!hierNodeSecurity.AllowView)
            //        {
            //            btnProcess.Enabled = false;
            //            btnSave.Enabled = false;
            //            securityOk = false;
            //            errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
            //            ErrorProvider.SetError(txtMerchandise, errorMessage);
            //        }
            //    }
            //    //End Track #5858
            //}
            //catch (Exception err)
            //{
            //    string message = err.ToString();
            //    throw;
            //}

            if (_exportMethod.PlanType == ePlanType.Chain)
            {
                securityOk = base.ValidateChainPlanVersionSecurity(cboVersion.ComboBox, true);  //TT#7 - RBeck - Dynamic dropdowns

                if (securityOk)
                    securityOk = (((MIDControlTag)(txtMerchandise.Tag)).IsAuthorized(eSecurityTypes.Chain, eSecuritySelectType.Update));
            }
            else 
            {
                securityOk = base.ValidateStorePlanVersionSecurity(cboVersion.ComboBox, true);  //TT#7 - RBeck - Dynamic dropdowns

                if (securityOk)
                    securityOk = (((MIDControlTag)(txtMerchandise.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));
            }

			//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
			//this.ApplySToreChainRBSecurity();
			if (FunctionSecurity.AllowUpdate)
			{
				if (_exportMethod.StoreAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
				{
					rdoStore.Enabled = true;
				}
				else
				{
					rdoStore.Enabled = false;
				}

				if (_exportMethod.ChainAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID))
				{
					rdoChain.Enabled = true;
				}
				else
				{
					rdoChain.Enabled = false;
				}
			}

			//End TT#89 - JScott - Process button disabled on new method if ver=Act
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

		//Begin TT#89 - JScott - Process button disabled on new method if ver=Act
		//// Begin Track #5852 stodd
		//private void ApplySToreChainRBSecurity()
		//{
		//    if (FunctionSecurity.AllowUpdate)
		//    {
		//        bool canViewStore = _exportMethod.StoreAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
		//        if (canViewStore)
		//            rdoStore.Enabled = true;
		//        else
		//            rdoStore.Enabled = false;

		//        bool canViewChain = _exportMethod.ChainAuthorizedToView(SAB.ClientServerSession, SAB.ClientServerSession.UserRID);
		//        if (canViewChain)
		//            rdoChain.Enabled = true;
		//        else
		//            rdoChain.Enabled = false;
		//    }
		//}
		// End Track #5852

		//End TT#89 - JScott - Process button disabled on new method if ver=Act
		private bool ConvertBoolConfigValue(string aBoolConfigValue)
		{
			try
			{
				if (aBoolConfigValue != null)
				{
                    //BEGIN TT#0906-VStuart-Config Values of Yes and T not read correctly-MID
					if (aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "true" || aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "yes" ||
						aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "t" || aBoolConfigValue.ToLower(CultureInfo.CurrentUICulture) == "y")
                    //END TT#0906-VStuart-Config Values of Yes and T not read correctly-MID
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
                _exportMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
                ChangePending = true;
            }
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
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
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

	}
}
