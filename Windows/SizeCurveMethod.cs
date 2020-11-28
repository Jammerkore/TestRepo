using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using MIDRetail.Windows.Controls;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;


namespace MIDRetail.Windows
{
    public partial class frmSizeCurveMethod : MIDRetail.Windows.SizeMethodsFormBase
    {
        #region Member Variables

		private SizeCurve _dlSizeCurve;
        private Image _dynamicToPlanImage;
        private Image _dynamicToCurrentImage;
		private Image _magnifyingGlassImage;
		private SizeCurveMethod _sizeCurveMethod;
        private System.Data.DataTable _dtMerchBasis;
		private System.Data.DataTable _dtCurveBasis;
		private bool _dragAndDrop;
        private bool _merchValChanged = false; //TT#4311 -  DOConnell - Keying Merchandise on Size Curve Method disappears when tab out of field

		#endregion
        #region Constructor

        public frmSizeCurveMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB)
            : base(SAB, aEAB, eMIDTextCode.frm_SizeCurveMethod, eWorkflowMethodType.Method)
        {
            try
            {
                AllowDragDrop = true;
                //
                // Required for Windows Form Designer support
                //
                InitializeComponent();
                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserSizeCurve);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalSizeCurve);
				_dlSizeCurve = new SizeCurve();
            }
            catch
            {
                FormLoadError = true;
                throw;
            }
        }

        #endregion
        #region WorkflowMethodFormBase Overrides

		override protected bool ApplySecurity()
		{
			bool securityOk = true;

			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					btnSave.Enabled = true;
				}
				else
				{
					btnSave.Enabled = false;
				}

				if (FunctionSecurity.AllowExecute)
				{
					btnProcess.Enabled = true;
				}
				else
				{
					btnProcess.Enabled = false;
				}

				return securityOk;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
        /// Opens an existing Fill Size Holes Method. 
        /// </summary>
        /// <param name="aMethodRID">method_RID</param>
        /// <param name="aNodeRID"></param>
        /// <param name="aLockStatus">The lock status of the data to be displayed</param>

		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
        {
            try
            {
                _sizeCurveMethod = new SizeCurveMethod(SAB, aMethodRID);
                base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserSizeCurve, eSecurityFunctions.AllocationMethodsGlobalSizeNeed);

                Common_Load(aNode.GlobalUserType);
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        /// <summary>
        /// Deletes a Fill Size Holes Method.
        /// </summary>
        /// <param name="method_RID">The record ID of the method</param>

		override public bool DeleteWorkflowMethod(int method_RID)
        {
            try
            {
                _sizeCurveMethod = new SizeCurveMethod(SAB, method_RID);

                return Delete();
            }
            catch (DatabaseForeignKeyViolation keyVio)
            {
                throw keyVio;
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
        }

        /// <summary>
        /// Gets if workflow or method.
        /// </summary>

		override protected eWorkflowMethodIND WorkflowMethodInd()
        {
            return eWorkflowMethodIND.SizeMethods;
        }

		override protected void Call_btnSave_Click()
        {
            try
            {
                base.btnSave_Click();
            }
			catch (Exception ex)
			{
				HandleException(ex, "Call_btnSave_Click");
			}
		}

		override protected void Call_btnProcess_Click()
        {
            try
            {
                ProcessAction(eMethodType.SizeCurve);

                if (!ErrorFound)
                {
                    _sizeCurveMethod.Method_Change_Type = eChangeType.update;
                    btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
                }

            }
			catch (Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
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
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
        }

        /// <summary>
        /// Use to set the specific fields in method object before updating
        /// </summary>

		override protected void SetSpecificFields()
        {
			try
			{
				SetMerchBasisValues();
				SetCurveBasisValues();
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        /// <summary>
        /// Use to validate the fields that are specific to this method type
        /// </summary>

		override protected bool ValidateSpecificFields()
        {
			bool methodFieldsValid = true;

			try
			{
				ErrorMessages.Clear();

				if (this._sizeCurveMethod.Method_Change_Type != DataCommon.eChangeType.delete)
				{
					if (!ValidBasisGrid())
					{
						methodFieldsValid = false;
					}
					//Begin TT#1076 - JScott - Size Curves by Set

					if (!ValidStoreGroup())
					{
						methodFieldsValid = false;
					}
					//End TT#1076 - JScott - Size Curves by Set
				}

				return methodFieldsValid;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
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
					ErrorMessages.Add(WorkflowMethodNameMessage);
					AttachErrors(txtName);
				}
				else
				{
					AttachErrors(txtName);
				}

				if (!WorkflowMethodDescriptionValid)
				{
					ErrorMessages.Add(WorkflowMethodDescriptionMessage);
					AttachErrors(txtDesc);
				}
				else
				{
					AttachErrors(txtDesc);
				}

				if (!UserGlobalValid)
				{
					ErrorMessages.Add(UserGlobalMessage);
					AttachErrors(pnlGlobalUser);
				}
				else
				{
					AttachErrors(pnlGlobalUser);
				}

			}
			catch
			{
				throw;
			}
        }

        /// <summary>
        /// Use to set the specific method object before updating
        /// </summary>

		override protected void SetObject()
        {
            try
            {
                ABM = _sizeCurveMethod;
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        /// <summary>
        /// Use to return the explorer node selected when form was opened
        /// </summary>

		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
        {
            return ExplorerNode;
        }

        /// <summary>
        /// Opens a new Fill Size Holes Method. 
        /// </summary>

		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
        {
            try
            {
				_sizeCurveMethod = new SizeCurveMethod(SAB, Include.NoRID);
				SetObject();
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserFillSizeHoles, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);

				Common_Load(aParentNode.GlobalUserType);
            }
            catch (Exception ex)
            {
                HandleException(ex, "NewWorkflowMethod");
                FormLoadError = true;
            }
		}


        /// <summary>
        /// Renames a Fill Size Holes Method.
        /// </summary>
        /// <param name="aMethodRID">The record ID of the method</param>
        /// <param name="aNewName">The new name of the workflow or method</param>

		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
        {
            try
            {
                _sizeCurveMethod = new SizeCurveMethod(SAB, aMethodRID);

				return Rename(aNewName);
            }
            catch (Exception ex)
            {
                HandleException(ex, "RenameWorkflowMethod");
                FormLoadError = true;
				return false;
            }
        }

        /// <summary>
        /// Processes a method.
        /// </summary>
        /// <param name="aWorkflowRID">The record ID of the method</param>

		override public void ProcessWorkflowMethod(int aMethodRID)
        {
            try
            {
                _sizeCurveMethod = new SizeCurveMethod(SAB, aMethodRID);
                ProcessAction(eMethodType.SizeCurve, true);
            }
            catch (Exception ex)
            {
                HandleException(ex);
                FormLoadError = true;
            }
        }

        #endregion WorkflowMethodFormBase Overrides
		#region Events

		private void txtName_TextChanged(object sender, EventArgs e)
		{
			try
			{
				txtDesc.Text = txtName.Text;
			}
			catch (Exception ex)
			{
				HandleException(ex, "txtName_TextChanged");
			}
		}

		private void rbMerchBasisEWYes_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rbMerchBasisEWYes.Checked)
					{
						_sizeCurveMethod.MerchBasisEqualizeWeight = true;
					}
					else
					{
						_sizeCurveMethod.MerchBasisEqualizeWeight = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rbMerchBasisEWYes_CheckedChanged");
			}
		}

		private void rbMerchBasisEWNo_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rbMerchBasisEWYes.Checked)
					{
						_sizeCurveMethod.MerchBasisEqualizeWeight = true;
					}
					else
					{
						_sizeCurveMethod.MerchBasisEqualizeWeight = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rbMerchBasisEWNo_CheckedChanged");
			}
		}

		private void rbCurveBasisEWYes_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rbCurveBasisEWYes.Checked)
					{
						_sizeCurveMethod.CurveBasisEqualizeWeight = true;
					}
					else
					{
						_sizeCurveMethod.CurveBasisEqualizeWeight = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWYes_CheckedChanged");
			}
		}

		private void rbCurveBasisEWNo_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (rbCurveBasisEWYes.Checked)
					{
						_sizeCurveMethod.CurveBasisEqualizeWeight = true;
					}
					else
					{
						_sizeCurveMethod.CurveBasisEqualizeWeight = false;
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		//Begin TT#1076 - JScott - Size Curves by Set
		private void rdoSizeCurvesByStore_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						_sizeCurveMethod.SizeCurvesByType = eSizeCurvesByType.Store;
						_sizeCurveMethod.SizeCurvesBySGRID = Include.NoRID;
						cboSizeCurveBySet.Enabled = false;
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void rdoSizeCurvesBySet_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						_sizeCurveMethod.SizeCurvesByType = eSizeCurvesByType.AttributeSet;
						_sizeCurveMethod.SizeCurvesBySGRID = Convert.ToInt32(this.cboSizeCurveBySet.SelectedValue, CultureInfo.CurrentUICulture);
						cboSizeCurveBySet.Enabled = true;
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeCurveBySet_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboSizeCurveBySet_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (FormLoaded)
				{
					_sizeCurveMethod.SizeCurvesBySGRID = Convert.ToInt32(this.cboSizeCurveBySet.SelectedValue, CultureInfo.CurrentUICulture);
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeCurveBySet_SelectionChangeCommitted");
                HandleException(ex, "cboSizeCurveBySet_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

		private void cboSizeCurveBySet_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				if (((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e))
				{
					_sizeCurveMethod.SizeCurvesBySGRID = Convert.ToInt32(this.cboSizeCurveBySet.SelectedValue, CultureInfo.CurrentUICulture);
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboSizeCurveBySet_DragDrop");
			}
		}

		private void cboSizeCurveBySet_DragEnter(object sender, DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void cboSizeCurveBySet_DragOver(object sender, DragEventArgs e)
		{
			Image_DragOver(sender, e);
		}

		//End TT#1076 - JScott - Size Curves by Set
		//Begin TT#155 - JScott - Add Size Curve info to Node Properties
		private void txtMinimumAvgPerSize_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						if (txtMinimumAvgPerSize.Text.Trim() == string.Empty)
						{
							_sizeCurveMethod.TolerMinAvgPerSize = Include.Undefined;
						}
						else
						{
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
							_sizeCurveMethod.TolerMinAvgPerSize = Convert.ToDouble(txtMinimumAvgPerSize.Text);
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						}
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void txtSalesTolerance_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						if (txtSalesTolerance.Text.Trim() == string.Empty)
						{
							_sizeCurveMethod.TolerSalesTolerance = Include.Undefined;
						}
						else
						{
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
							_sizeCurveMethod.TolerSalesTolerance = Convert.ToDouble(txtSalesTolerance.Text);
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						}
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void rdoIndexToAverage_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						_sizeCurveMethod.TolerIndexUnitsType = eNodeChainSalesType.IndexToAverage;
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void rdoUnits_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						_sizeCurveMethod.TolerIndexUnitsType = eNodeChainSalesType.Units;
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void txtMinimumPct_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						if (txtMinimumPct.Text.Trim() == string.Empty)
						{
							_sizeCurveMethod.TolerMinTolerancePct = Include.Undefined;
							//Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue 
                            cbxApplyMinToZeroTolerance.Checked = false;
                            cbxApplyMinToZeroTolerance.Enabled = false;
							//End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue 
						}
						else
						{
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
	                        _sizeCurveMethod.TolerMinTolerancePct = Convert.ToDouble(txtMinimumPct.Text);
                            //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue 
							cbxApplyMinToZeroTolerance.Enabled = true;
							//End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue 
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						}
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		private void txtMaximumPct_TextChanged(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					try
					{
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						if (txtMaximumPct.Text.Trim() == string.Empty)
						{
							_sizeCurveMethod.TolerMaxTolerancePct = Include.Undefined;
						}
						else
						{
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
	                        _sizeCurveMethod.TolerMaxTolerancePct = Convert.ToDouble(txtMaximumPct.Text);
						//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
						}
						//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
					}
					catch
					{
					}

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "rdCurveBasisEWNo_CheckedChanged");
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        override protected void cboSizeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded &&
					cboSizeGroup.SelectedIndex > Include.Undefined)
				{
					_sizeCurveMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
					ChangePending = true;
					ErrorProvider.SetError(cboSizeGroup, null);
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

        //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation issue
        private void cbxApplyMinToZeroTolerance_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (base.FormLoaded) 
                {
                    if (cbxApplyMinToZeroTolerance.Checked)
                    {
                        _sizeCurveMethod.ApplyMinToZeroTolerance = true;
                    }
                    else
                    {
                        _sizeCurveMethod.ApplyMinToZeroTolerance = false;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "cbxApplyMinToZeroTolerance_CheckChanged");
            }
        }
        //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation issue

		private void grdMerchBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
        {
            try
            {
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169

                e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

                //hide the key columns.
				e.Layout.Bands[0].Columns["BASIS_SEQ"].Hidden = true;
                e.Layout.Bands[0].Columns["HN_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["FV_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CDR_RID"].Hidden = true;
                e.Layout.Bands[0].Columns["MERCH_TYPE"].Hidden = true;
				e.Layout.Bands[0].Columns["OLL_RID"].Hidden = true;
				e.Layout.Bands[0].Columns["CUSTOM_OLL_RID"].Hidden = true;

                //Prevent the user from re-arranging columns.

				grdMerchBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

                if (FunctionSecurity.AllowUpdate)
                {
					grdMerchBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					grdMerchBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
                    BuildMerchBasisContextMenu();
					grdMerchBasis.ContextMenuStrip = cmsMerchBasisGrid;
                }
                else
                {
					grdMerchBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					grdMerchBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
                }

                //Set the header captions.

				grdMerchBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.VisiblePosition = 1;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Header.Caption = "Merchandise";
				grdMerchBasis.DisplayLayout.Bands[0].Columns["Merchandise"].Width = 200;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.VisiblePosition = 3;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = "Date Range";
				grdMerchBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 5;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				grdMerchBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Width = 45;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.VisiblePosition = 6;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = "Include/Exclude";
				grdMerchBasis.DisplayLayout.Bands[0].Columns["OverrideList"].Header.VisiblePosition = 7;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["OverrideList"].Header.Caption = "Override Model";

                //Make some columns readonly.

				grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 110;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;

				grdMerchBasis.DisplayLayout.Bands[0].Columns["OverrideList"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["OverrideList"].AutoEdit = true;
				grdMerchBasis.DisplayLayout.Bands[0].Columns["OverrideList"].Width = 100;

				//Set the width of the "DateRange" column so that the DateRangeSelector can fit.

				grdMerchBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 160;

                if (FunctionSecurity.AllowUpdate)
                {
					grdMerchBasis.DisplayLayout.Bands[0].Columns["DateRange"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
					grdMerchBasis.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.NoEdit;
                }

				grdMerchBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdMerchBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdMerchBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdMerchBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_InitializeLayout");
			}
		}

		private void grdMerchBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
        {
			DateRangeProfile dr;
			HierarchyNodeProfile hnp;
			int nodeKey;
			
			try
            {
				if (!e.ReInitialize)
				{
					e.Row.Cells["IncludeButton"].Value = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);

					PopulateOverrideLowLevelValueList(e.Row);
				}

                //Populate cell w/text description of Date Range
                if (e.Row.Cells["CDR_RID"].Value.ToString() != "")
                {
                    dr = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(e.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture));

                    e.Row.Cells["DateRange"].Value = dr.DisplayDate;

                    if (dr.DateRangeType == eCalendarRangeType.Dynamic)
                    {
                        switch (dr.RelativeTo)
                        {
                            case eDateRangeRelativeTo.Current:
                                e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToCurrentImage;
                                break;
                            case eDateRangeRelativeTo.Plan:
                                e.Row.Cells["DateRange"].Appearance.Image = this.DynamicToPlanImage;
                                break;
                            default:
                                e.Row.Cells["DateRange"].Appearance.Image = null;
                                break;
                        }
                    }
                }

                _merchValChanged = true; //TT#4311 -  DOConnell - Keying Merchandise on Size Curve Method disappears when tab out of field
                //Populate cell w/text description of Hierarchy Node
				if (e.Row.Cells["HN_RID"].Value.ToString() != string.Empty)
				{
					nodeKey = Convert.ToInt32(e.Row.Cells["HN_RID"].Value, CultureInfo.CurrentUICulture);
					hnp = SAB.HierarchyServerSession.GetNodeData(nodeKey, true, true);

					e.Row.Cells["Merchandise"].Value = hnp.Text;
					e.Row.Cells["MERCH_TYPE"].Value = eMerchandiseType.Node;

					if (Convert.ToInt32(e.Row.Cells["HN_RID"].Value) != Include.NoRID)
					{
						grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
					}
					else
					{
						grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
					}
				}
				else
				{
					grdMerchBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
				}
                _merchValChanged = false; //TT#4311 -  DOConnell - Keying Merchandise on Size Curve Method disappears when tab out of field

				e.Row.Cells["OverrideList"].Value = e.Row.Cells["OLL_RID"].Value;

                if (FormLoaded &&
                    !FormIsClosing)
                {

            //Begin TT#2864 - MD - Save when nothing has chenged 
                    if (!e.ReInitialize)
                    {
                        ChangePending = false;   
                    }
                    else
                    {
                        ChangePending = true;
                    }
            //End   TT#2864 _ MD - Save when nothing has chenged 
        
					ErrorProvider.SetError(grdMerchBasis, null);
                }
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_InitializeRow");
			}
		}

		private void grdMerchBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_CellChange");
			}
        }

		private void grdMerchBasis_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

		private void grdMerchBasis_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
        {
			int lastIndex;
			int i;
			DataRow lastRow;

			try
			{
				if (CheckMerchBasisInsertCondition(grdMerchBasis, e) == false)
				{
					e.Cancel = true;
					return;
				}

				this.Cursor = Cursors.WaitCursor;

				try
				{
					lastRow = null;
					lastIndex = _dtMerchBasis.Rows.Count - 1;

					for (i = lastIndex; i > -1; i--)
					{
						lastRow = _dtMerchBasis.Rows[i];

						if (lastRow.RowState != DataRowState.Deleted)
						{
							break;
						}
					}

					if (lastRow != null && lastRow.RowState != DataRowState.Deleted)
					{
						InsertMerchBasis(Convert.ToInt32(lastRow["BASIS_SEQ"]), false);
					}
					else
					{
						InsertMerchBasis(-1, false);
					}

					grdMerchBasis.ActiveRow = grdMerchBasis.Rows[grdMerchBasis.Rows.Count - 1];
					e.Cancel = true;
				}
				catch
				{
					throw;
				}
				finally
				{
					this.Cursor = Cursors.Default;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_BeforeRowInsert");
			}
        }

		private void grdMerchBasis_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
        }

		private void grdMerchBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
			try
			{
				if (FormLoaded && !FormIsClosing)
				{
                    ErrorFound = false; //TT#2864 - MD - Save when nothing has chenged 
                    switch (e.Cell.Column.Key)
					{
						case "OverrideList":
							e.Cell.Row.Cells["OLL_RID"].Value = e.Cell.Value;
                            ChangePending = true;  
							break;
						//BEGIN TT#4311 -  DOConnell - Keying Merchandise on Size Curve Method disappears when tab out of field
                        case "Merchandise":
                            string productID = e.Cell.Value.ToString();
                            if (string.IsNullOrWhiteSpace(productID) ||
                                _merchValChanged ||
                                _dragAndDrop)
                            {
                                return;
                            }
                            _merchValChanged = true;
                            HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
                            EditMsgs em = new EditMsgs();
                            HierarchyNodeProfile hnp = null;
                            hnp = hm.NodeLookup(ref em, productID, false, true);
                            if (hnp == null ||
                                hnp.Key == Include.NoRID)
                            {
                                string errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode),
                                    productID);
                                MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                e.Cell.Row.Cells["HN_RID"].Value = hnp.Key;
                                e.Cell.Row.Cells["Merchandise"].Value = hnp.Text;
                                e.Cell.Row.Cells["Merchandise"].Appearance.Image = null;
                                e.Cell.Row.Cells["Merchandise"].Tag = null;
                                grdMerchBasis.UpdateData();
                            }
                            _merchValChanged = false;
                            break;
							//END TT#4311 -  DOConnell - Keying Merchandise on Size Curve Method disappears when tab out of field
					}
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_AfterCellUpdate");
			}
        }

		private void grdMerchBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    ChangePending = true;
					ErrorProvider.SetError(grdMerchBasis, null);
                }
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_AfterRowInsert");
			}
		}

		private void grdMerchBasis_AfterRowsDeleted(object sender, EventArgs e)
        {
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_AfterRowsDeleted");
			}
        }

		private void grdMerchBasis_DragEnter(object sender, DragEventArgs e)
        {
			try
			{
				Image_DragEnter(sender, e);
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_DragEnter");
			}
        }

		private void grdMerchBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
			//Begin TT#426 - JScott - Node Properties  On the Size Curves Tab Should not be able to select “Size Level” at Node Property setting for Merchandise.
			HierarchyNodeProfile hnp;
			TreeNodeClipboardList cbList;

			//End TT#426 - JScott - Node Properties  On the Size Curves Tab Should not be able to select “Size Level” at Node Property setting for Merchandise.
			try
            {
                Image_DragOver(sender, e);
                Infragistics.Win.UIElement aUIElement;
				aUIElement = grdMerchBasis.DisplayLayout.UIElement.ElementFromPoint(grdMerchBasis.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridRow aRow;
                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
                if (aCell == null)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

				if (aCell == aRow.Cells["Merchandise"])
                {
					//Begin TT#426 - JScott - Node Properties  On the Size Curves Tab Should not be able to select “Size Level” at Node Property setting for Merchandise.
					//e.Effect = DragDropEffects.All;
					if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
						cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

						if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
						{
							hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);

							if (hnp.LevelType != eHierarchyLevelType.Size)
							{
								e.Effect = DragDropEffects.All;
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
					else
					{
						e.Effect = DragDropEffects.None;
					}
					//End TT#426 - JScott - Node Properties  On the Size Curves Tab Should not be able to select “Size Level” at Node Property setting for Merchandise.
				}
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_DragOver");
			}
		}

		private void grdMerchBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
			Infragistics.Win.UIElement aUIElement;
			UltraGridRow aRow;
			UltraGridCell aCell;
			HierarchyNodeProfile hnp;
            TreeNodeClipboardList cbList;

            try
            {
				aUIElement = grdMerchBasis.DisplayLayout.UIElement.ElementFromPoint(grdMerchBasis.PointToClient(new Point(e.X, e.Y)));

                if (aUIElement == null)
                {
                    return;
                }

                aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

                if (aRow == null)
                {
                    return;
                }

                aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));

                if (aCell == null)
                {
                    return;
                }

				if (aCell == aRow.Cells["Merchandise"])
				{
					if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
					{
						cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));

						if (cbList.ClipboardDataType == eProfileType.HierarchyNode)
						{
							hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);

							_dragAndDrop = true;
							aRow.Cells["HN_RID"].Value = hnp.Key;
							aRow.Cells["Merchandise"].Value = hnp.Text;
							aRow.Cells["Merchandise"].Appearance.Image = null;
							aRow.Cells["Merchandise"].Tag = null;
							grdMerchBasis.UpdateData();
							_dragAndDrop = false;
						}
						else
						{
							MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
						}
					}
					else
					{
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
					}                   
				}
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_DragDrop");
			}
		}

		private void grdMerchBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            CalendarDateSelector frmCalDtSelector;
            DialogResult dateRangeResult;
            DateRangeProfile selectedDateRange;
			//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
			string lowLevelText;
			//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed

            try
            {
                switch (e.Cell.Column.Key)
                {
                    case "DateRange":

                        frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });

                        if (e.Cell.Row.Cells["DateRange"].Value != null &&
                            e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
                            e.Cell.Row.Cells["DateRange"].Text.Length > 0)
                        {
                            frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["CDR_RID"].Value, CultureInfo.CurrentUICulture);
                        }

                        frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Current;
                        frmCalDtSelector.AllowDynamicToStoreOpen = false;
                        frmCalDtSelector.AllowDynamicToPlan = false;

                        dateRangeResult = frmCalDtSelector.ShowDialog();

                        if (dateRangeResult == DialogResult.OK)
                        {
                            selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;

                            e.Cell.Value = selectedDateRange.DisplayDate;
                            e.Cell.Row.Cells["CDR_RID"].Value = selectedDateRange.Key;

                            if (selectedDateRange.DateRangeType == eCalendarRangeType.Dynamic)
                            {
                                if (selectedDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
                                {
                                    e.Cell.Appearance.Image = _dynamicToPlanImage;
                                }
                                else
                                {
                                    e.Cell.Appearance.Image = _dynamicToCurrentImage;
                                }
                            }
                            else
                            {
                                e.Cell.Appearance.Image = null;
                            }
                        }

                        e.Cell.CancelUpdate();
						grdMerchBasis.PerformAction(UltraGridAction.DeactivateCell);
                        break;

                    case "IncludeButton":

						Cursor.Current = Cursors.WaitCursor;

						try
						{
							int nodeRid = Convert.ToInt32(e.Cell.Row.Cells["HN_RID"].Value);

							HierarchyNodeProfile hierNodeProf = SAB.HierarchyServerSession.GetNodeData(nodeRid);
							HierarchyProfile hierProf = SAB.HierarchyServerSession.GetHierarchyData(hierNodeProf.HomeHierarchyRID);
							//Begin TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
							//HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[hierNodeProf.HomeHierarchyRID + 1];

							//object[] args = new object[] { SAB, Convert.ToInt32(e.Cell.Row.Cells["OLL_RID"].Value), nodeRid, Convert.ToInt32(e.Cell.Row.Cells["FV_RID"].Value), hlp.LevelID, Convert.ToInt32(e.Cell.Row.Cells["CUSTOM_OLL_RID"].Value), GlobalSecurity };
							if (hierProf.HierarchyType == eHierarchyType.organizational)
							{
								HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[hierNodeProf.HomeHierarchyLevel + 1];
								lowLevelText = hlp.LevelID;
							}
							else
							{
								lowLevelText = "+1";
							}

							object[] args = new object[] { SAB, Convert.ToInt32(e.Cell.Row.Cells["OLL_RID"].Value), nodeRid, Convert.ToInt32(e.Cell.Row.Cells["FV_RID"].Value), lowLevelText, Convert.ToInt32(e.Cell.Row.Cells["CUSTOM_OLL_RID"].Value), GlobalSecurity };
							//End TT#490 - JScott - Size Curve Method using an Alternate Node as the Merchandise Basis when processing = No Action Performed
							frmOverrideLowLevelModel frm = (frmOverrideLowLevelModel)GetForm(typeof(frmOverrideLowLevelModel), args, false);

							frm.MdiParent = this.MdiParent;
							frm.Anchor = AnchorStyles.Left | AnchorStyles.Top;
							frm.OnOverrideLowLevelCloseHandler += new frmOverrideLowLevelModel.OverrideLowLevelCloseEventHandler(OnOverrideLowLevelCloseHandler);
							frm.Tag = e.Cell.Row;
							frm.Show();
							frm.BringToFront();
						}
						catch
						{
							throw;
						}
						finally
						{
							Cursor.Current = Cursors.Default;
						}
						break;
				}
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_ClickCellButton");
			}
		}

		private void grdMerchBasis_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
        {
            try
            {
				ShowUltraGridToolTip(grdMerchBasis, e);
            }
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_MouseEnterElement");
			}
		}

		private void grdCurveBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				//Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169

				e.Layout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;

				//hide the key columns.
				e.Layout.Bands[0].Columns["BASIS_SEQ"].Hidden = true;
				e.Layout.Bands[0].Columns["SIZE_CURVE_GROUP_RID"].Hidden = true;

				//Prevent the user from re-arranging columns.

				grdCurveBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				if (FunctionSecurity.AllowUpdate)
				{
					grdCurveBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.Yes;
					grdCurveBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.True;
					BuildCurveBasisContextMenu();
					grdCurveBasis.ContextMenuStrip = cmsCurveBasisGrid;
				}
				else
				{
					grdCurveBasis.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
					grdCurveBasis.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
				}

				//Set the header captions.

				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].Header.VisiblePosition = 1;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].Header.Caption = " ";
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurve"].Header.VisiblePosition = 3;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurve"].Header.Caption = "Size Curve";
				grdCurveBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.VisiblePosition = 5;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Header.Caption = "Weight";
				grdCurveBasis.DisplayLayout.Bands[0].Columns["WEIGHT"].Width = 45;

				//Make some columns readonly.

				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].MinWidth = 5;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].Width = 5;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurveFilter"].CellActivation = Activation.NoEdit;

				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurve"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurve"].AutoEdit = true;
				grdCurveBasis.DisplayLayout.Bands[0].Columns["SizeCurve"].Width = 100;

				grdCurveBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdCurveBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdCurveBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdCurveBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_InitializeLayout");
			}
		}

		private void grdCurveBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				if (!e.ReInitialize)
				{
					e.Row.Cells["SizeCurveFilter"].Appearance.Image = _magnifyingGlassImage;
					e.Row.Cells["SizeCurveFilter"].ButtonAppearance.Image = _magnifyingGlassImage;
					e.Row.Cells["SizeCurveFilter"].ButtonAppearance.Image = _magnifyingGlassImage;

					e.Row.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
					PopulateSizeCurveValueList(e.Row, Convert.ToInt32(e.Row.Cells["SIZE_CURVE_GROUP_RID"].Value));
				}

				e.Row.Cells["SizeCurve"].Value = e.Row.Cells["SIZE_CURVE_GROUP_RID"].Value;

				if (FormLoaded &&
					!FormIsClosing)
				{
					ChangePending = true;
					ErrorProvider.SetError(grdCurveBasis, null);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_InitializeRow");
			}
		}

		private void grdCurveBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_CellChange");
			}
		}

		private void grdCurveBasis_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
		}

		private void grdCurveBasis_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			int lastIndex;
			int i;
			DataRow lastRow;

			try
			{
				if (CheckCurveBasisInsertCondition(grdCurveBasis, e) == false)
				{
					e.Cancel = true;
					return;
				}

				this.Cursor = Cursors.WaitCursor;

				try
				{
					lastRow = null;
					lastIndex = _dtCurveBasis.Rows.Count - 1;

					for (i = lastIndex; i > -1; i--)
					{
						lastRow = _dtCurveBasis.Rows[i];

						if (lastRow.RowState != DataRowState.Deleted)
						{
							break;
						}
					}

					if (lastRow != null && lastRow.RowState != DataRowState.Deleted)
					{
						InsertCurveBasis(Convert.ToInt32(lastRow["BASIS_SEQ"]), false);
					}
					else
					{
						InsertCurveBasis(-1, false);
					}

					grdCurveBasis.ActiveRow = grdCurveBasis.Rows[grdCurveBasis.Rows.Count - 1];
					e.Cancel = true;
				}
				catch
				{
					throw;
				}
				finally
				{
					this.Cursor = Cursors.Default;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_BeforeRowInsert");
			}
		}

		private void grdCurveBasis_AfterCellListCloseUp(object sender, CellEventArgs e)
		{
		}

		private void grdCurveBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (FormLoaded && !FormIsClosing)
				{
					switch (e.Cell.Column.Key)
					{
						case "SizeCurve":
							e.Cell.Row.Cells["SIZE_CURVE_GROUP_RID"].Value = e.Cell.Value;
							ChangePending = true;
							break;
					}
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdMerchBasis_AfterCellUpdate");
			}
		}

		private void grdCurveBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
					ErrorProvider.SetError(grdCurveBasis, null);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_AfterRowInsert");
			}
		}

		private void grdCurveBasis_AfterRowsDeleted(object sender, EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_AfterRowsDeleted");
			}
		}

		private void grdCurveBasis_DragEnter(object sender, DragEventArgs e)
		{
			try
			{
				Image_DragEnter(sender, e);
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_DragEnter");
			}
		}

		private void grdCurveBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				Image_DragOver(sender, e);
				Infragistics.Win.UIElement aUIElement;
				aUIElement = grdCurveBasis.DisplayLayout.UIElement.ElementFromPoint(grdCurveBasis.PointToClient(new Point(e.X, e.Y)));

				if (aUIElement == null)
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				UltraGridRow aRow;
				aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

				if (aRow == null)
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));
				if (aCell == null)
				{
					e.Effect = DragDropEffects.None;
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_DragOver");
			}
		}

		private void grdCurveBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Infragistics.Win.UIElement aUIElement;
			UltraGridRow aRow;
			UltraGridCell aCell;

			try
			{
				aUIElement = grdCurveBasis.DisplayLayout.UIElement.ElementFromPoint(grdCurveBasis.PointToClient(new Point(e.X, e.Y)));

				if (aUIElement == null)
				{
					return;
				}

				aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow));

				if (aRow == null)
				{
					return;
				}

				aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell));

				if (aCell == null)
				{
					return;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_DragDrop");
			}
		}

		private void grdCurveBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			string enteredMask;
			bool caseSensitive;

			try
			{
				switch (e.Cell.Column.Key)
				{
					case "SizeCurveFilter":

						enteredMask = string.Empty;
						caseSensitive = false;

						if (CharMaskFromDialogOK(Convert.ToString(e.Cell.Row.Tag), ref enteredMask, ref caseSensitive))
						{
							PopulateSizeCurveValueList(e.Cell.Row, enteredMask, caseSensitive);
						}

						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_ClickCellButton");
			}
		}

		private void grdCurveBasis_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(grdCurveBasis, e);
			}
			catch (Exception ex)
			{
				HandleException(ex, "grdCurveBasis_MouseEnterElement");
			}
		}

		private void tsmiMerchBasisInsertBefore_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdMerchBasis.ActiveRow != null)
				{
					InsertMerchBasis(Convert.ToInt32(grdMerchBasis.ActiveRow.Cells["BASIS_SEQ"].Value), true);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiInsertBefore_Click");
			}
		}

		private void tsmiMerchBasisInsertAfter_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdMerchBasis.ActiveRow != null)
				{
					InsertMerchBasis(Convert.ToInt32(grdMerchBasis.ActiveRow.Cells["BASIS_SEQ"].Value), false);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiInsertAfter_Click");
			}
		}

		private void tsmiMerchBasisDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdMerchBasis.Selected.Rows.Count > 0)
				{
					grdMerchBasis.DeleteSelectedRows();
					_dtMerchBasis.AcceptChanges();

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiDelete_Click");
			}
		}

		private void tsmiMerchBasisDeleteAll_Click(object sender, EventArgs e)
		{
			try
			{
				_dtMerchBasis.Rows.Clear();
				_dtMerchBasis.AcceptChanges();

				ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiDeleteAll_Click");
			}
		}

		private void tsmiCurveBasisInsertBefore_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdCurveBasis.ActiveRow != null)
				{
					InsertCurveBasis(Convert.ToInt32(grdCurveBasis.ActiveRow.Cells["BASIS_SEQ"].Value), true);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiInsertBefore_Click");
			}
		}

		private void tsmiCurveBasisInsertAfter_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdCurveBasis.ActiveRow != null)
				{
					InsertCurveBasis(Convert.ToInt32(grdCurveBasis.ActiveRow.Cells["BASIS_SEQ"].Value), false);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiInsertAfter_Click");
			}
		}

		private void tsmiCurveBasisDelete_Click(object sender, EventArgs e)
		{
			try
			{
				if (grdCurveBasis.Selected.Rows.Count > 0)
				{
					grdCurveBasis.DeleteSelectedRows();
					_dtCurveBasis.AcceptChanges();

					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiDelete_Click");
			}
		}

		private void tsmiCurveBasisDeleteAll_Click(object sender, EventArgs e)
		{
			try
			{
				_dtCurveBasis.Rows.Clear();
				_dtCurveBasis.AcceptChanges();

				ChangePending = true;
			}
			catch (Exception ex)
			{
				HandleException(ex, "tsmiDeleteAll_Click");
			}
		}

		#endregion
		#region Methods

		/// <summary>
		/// Private method handles loading data on the form
		/// </summary>
		/// <param name="aGlobalUserType"></param>

		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			//Begin TT#1076 - JScott - Size Curves by Set
			GlobalOptionsProfile glblOpts;
			ProfileList strGrpProfList;

			//End TT#1076 - JScott - Size Curves by Set
			try
			{
				SetText();

				Name = MIDText.GetTextOnly((int)eMethodType.SizeCurve);

				txtDesc.Visible = false;
				radGlobal.Visible = false;
				radUser.Visible = false;

				//Begin TT#457 -- Workflow/Method Explorer - created a Size Curve method and processed after completing successfully the method appears in the Global area instead of the My Workflow Methods area
				//radGlobal.Checked = true;
				//End TT#457 -- Workflow/Method Explorer - created a Size Curve method and processed after completing successfully the method appears in the Global area instead of the My Workflow Methods area
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);
				_magnifyingGlassImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.MagnifyingGlassImage);

				BuildDataTables(); //Inherited from WorkflowMethodFormBase

				this.txtName.Text = _sizeCurveMethod.Name;
				this.txtDesc.Text = _sizeCurveMethod.Method_Description;

				// Set the Size Group Combo

				BindSizeGroupComboBox(true, _sizeCurveMethod.SizeGroupRid);

				if (_sizeCurveMethod.Method_Change_Type == eChangeType.update)
				{
					cboSizeGroup.SelectedValue = _sizeCurveMethod.SizeGroupRid;
                    //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}

				//Begin TT#1076 - JScott - Size Curves by Set
				// Set the Size Curves By radio button

				switch (_sizeCurveMethod.SizeCurvesByType)
				{
					case eSizeCurvesByType.Store:
						rdoSizeCurvesByStore.Checked = true;
						cboSizeCurveBySet.Enabled = false;
						break;
					case eSizeCurvesByType.AttributeSet:
						rdoSizeCurvesBySet.Checked = true;
                        // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
						//cboSizeCurveBySet.Enabled = true;
                        if (FunctionSecurity.AllowUpdate)
                        {
                            if (cboSizeCurveBySet.ContinueReadOnly)
                            {
                                SetMethodReadOnly();
                            }
                            else
                            {
                                cboSizeCurveBySet.Enabled = true;
                            }
                        }
                        else
                        {
                            cboSizeCurveBySet.Enabled = false;
                        }
                        // End TT#1530
						break;
					default:
						rdoSizeCurvesByStore.Checked = false;
						rdoSizeCurvesBySet.Checked = false;
						cboSizeCurveBySet.Enabled = false;
						break;
				}

				// Set the Store Group combo box

				strGrpProfList = GetStoreGroupList(_sizeCurveMethod.Method_Change_Type, _sizeCurveMethod.GlobalUserType, false);
				cboSizeCurveBySet.Initialize(SAB, FunctionSecurity, strGrpProfList.ArrayList, _sizeCurveMethod.GlobalUserType == eGlobalUserType.User);

                AdjustTextWidthComboBox_DropDown(cboSizeCurveBySet);  //TT#7 - RBeck - Dynamic dropdowns

				cboSizeCurveBySet.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboSizeCurveBySet, FunctionSecurity, _sizeCurveMethod.GlobalUserType == eGlobalUserType.User);

				if (_sizeCurveMethod.SizeCurvesBySGRID != Include.NoRID)
				{
					cboSizeCurveBySet.SelectedValue = _sizeCurveMethod.SizeCurvesBySGRID;
                    //this.cboSizeCurveBySet_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}
				else
				{
					glblOpts = new GlobalOptionsProfile(-1);
					glblOpts.LoadOptions();
					cboSizeCurveBySet.SelectedValue = glblOpts.AllocationStoreGroupRID;
                    //this.cboSizeCurveBySet_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}

				//End TT#1076 - JScott - Size Curves by Set
				// Set the Merch Basis Equalize radio button

				rbMerchBasisEWYes.Checked = _sizeCurveMethod.MerchBasisEqualizeWeight;
				rbMerchBasisEWNo.Checked = !_sizeCurveMethod.MerchBasisEqualizeWeight;

				// Set the Curve Basis Equalize radio button

				rbCurveBasisEWYes.Checked = _sizeCurveMethod.CurveBasisEqualizeWeight;
				rbCurveBasisEWNo.Checked = !_sizeCurveMethod.CurveBasisEqualizeWeight;

				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				// Set the Curve Tolerance Fields

				//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
				//txtMinimumAvgPerSize.Text = Convert.ToString(_sizeCurveMethod.TolerMinAvgPerSize);

				//txtSalesTolerance.Text = Convert.ToString(_sizeCurveMethod.TolerSalesTolerance);
				if (_sizeCurveMethod.TolerMinAvgPerSize != Include.Undefined)
				{
					txtMinimumAvgPerSize.Text = Convert.ToString(_sizeCurveMethod.TolerMinAvgPerSize);
				}
				else
				{
					txtMinimumAvgPerSize.Text = string.Empty;
				}

				if (_sizeCurveMethod.TolerSalesTolerance != Include.Undefined)
				{
					txtSalesTolerance.Text = Convert.ToString(_sizeCurveMethod.TolerSalesTolerance);
				}
				else
				{
					txtSalesTolerance.Text = string.Empty;
				}
				//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method

				if (_sizeCurveMethod.TolerIndexUnitsType == eNodeChainSalesType.IndexToAverage)
				{
					rdoIndexToAverage.Checked = true;
				}
				else
				{
					rdoIndexToAverage.Checked = false;
				}

				if (_sizeCurveMethod.TolerIndexUnitsType == eNodeChainSalesType.Units)
				{
					rdoUnits.Checked = true;
				}
				else
				{
					rdoUnits.Checked = false;
				}

				//Begin TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method
				//txtMinimumPct.Text = Convert.ToString(_sizeCurveMethod.TolerMinTolerancePct);

				//txtMaximumPct.Text = Convert.ToString(_sizeCurveMethod.TolerMaxTolerancePct);
				if (_sizeCurveMethod.TolerMinTolerancePct != Include.Undefined)
				{
					txtMinimumPct.Text = Convert.ToString(_sizeCurveMethod.TolerMinTolerancePct);
				}
				else
				{
					txtMinimumPct.Text = string.Empty;
				}

                //Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                cbxApplyMinToZeroTolerance.Checked = _sizeCurveMethod.ApplyMinToZeroTolerance;
                //End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue

				if (_sizeCurveMethod.TolerMaxTolerancePct != Include.Undefined)
				{
					txtMaximumPct.Text = Convert.ToString(_sizeCurveMethod.TolerMaxTolerancePct);
				}
				else
				{
					txtMaximumPct.Text = string.Empty;
				}
				//End TT#424 - JScott - SIZE CURVE METHOD – tolerance tab – fields not holding delete after updating method

				//End TT#155 - JScott - Add Size Curve info to Node Properties
				// Temporarily disable the Curve Basis Tab

				tabSizeCurveMethodOptions.TabPages.Remove(tabSizeCurveBasis);

				// Load the Merchandise Basis datatable and set it to the grid

				_dtMerchBasis = GetDataSourceMerchBasis();

				_dtMerchBasis.Columns["WEIGHT"].DefaultValue = 1;

				foreach (DataRow dr in _dtMerchBasis.Rows)
				{
					dr["Merchandise"] = SAB.HierarchyServerSession.GetNodeData(Convert.ToInt32(dr["HN_RID"]), true, true).Text;
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
					dr["DateRange"] = drp.DisplayDate;
				}

				grdMerchBasis.DataSource = _dtMerchBasis.DefaultView;

				// Load the Curve Basis datatable and set it to the grid

				_dtCurveBasis = GetDataSourceCurveBasis();

				_dtCurveBasis.Columns["WEIGHT"].DefaultValue = 1;

				foreach (DataRow dr in _dtCurveBasis.Rows)
				{
					dr["SizeCurve"] = _dlSizeCurve.GetSpecificSizeCurveGroup(Convert.ToInt32(dr["SIZE_CURVE_GROUP_RID"]));
				}

				grdCurveBasis.DataSource = _dtCurveBasis.DefaultView;

				LoadWorkflows();
                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabSizeCurveMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Controls the text for the Process, {Save,Update}, and Close buttons.
		/// </summary>

		private void SetText()
		{
			try
			{
                // BEGIN TT#3830 - AGallagher - Size Curve Method label has double colons
				//WorkflowMethodNameLabel = "Size Curve Name:";
                WorkflowMethodNameLabel = "Method";
                // END TT#3830 - AGallagher - Size Curve Method label has double colons
				this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				this.tabMerchandiseBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MerchandiseBasis);
				this.tabSizeCurveBasis.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_CurveBasis);
				//Begin TT#1076 - JScott - Size Curves by Set
				this.rdoSizeCurvesByStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurvesByStore);
				this.rdoSizeCurvesBySet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SizeCurvesBySet);
				//End TT#1076 - JScott - Size Curves by Set
				//Begin TT#155 - JScott - Add Size Curve info to Node Properties
				this.lblMinimumAvgPerSize.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MinimumAvgPerSize);
				this.lblSalesTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SalesTolerance);
				this.rdoIndexToAverage.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IndexToAverage);
				this.rdoUnits.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SalesToleranceUnits);
				this.lblMinimumPct.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MinTolerancePct);
				this.lblMaximumPct.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_MaxTolerancePct);
				//End TT#155 - JScott - Add Size Curve info to Node Properties
				this.rbMerchBasisEWNo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
				this.rbMerchBasisEWYes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
				this.rbCurveBasisEWNo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
				this.rbCurveBasisEWYes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
                // Begin TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
                this.cbxApplyMinToZeroTolerance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyMinToZeroTolerance);
                // End TT#2079 - DOConnell - Size - Minimum % Tolerance Calculation Issue
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Fill the workflow grid
		/// </summary>

		private void LoadWorkflows()
		{
			try
			{
				GetWorkflows(_sizeCurveMethod.Key, ugWorkflows);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a datatable for use as the grid's data source.
		/// </summary>
		/// <returns>DataSet</returns>

		private DataTable GetDataSourceMerchBasis()
		{
			try
			{
				//*****************************************************************************
				// This gets ALL of the basis info (both basis plan AND basis range) for the OTS plan method.  
				// The list is then 
				// filtered for each Store group level (attr set)
				//*****************************************************************************

				DataTable dt = _sizeCurveMethod.DTMerchBasisDetail;
				dt.TableName = "MerchBasisTable";

				dt.Columns.Add("Merchandise");
				dt.Columns.Add("DateRange");
				dt.Columns.Add("IncludeButton");
				dt.Columns.Add("OverrideList");

				dt.AcceptChanges();

				return dt;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a datatable for use as the grid's data source.
		/// </summary>
		/// <returns>DataSet</returns>

		private DataTable GetDataSourceCurveBasis()
		{
			try
			{
				//*****************************************************************************
				// This gets ALL of the basis info (both basis plan AND basis range) for the OTS plan method.  
				// The list is then 
				// filtered for each Store group level (attr set)
				//*****************************************************************************

				DataTable dt = _sizeCurveMethod.DTCurveBasisDetail;
				dt.TableName = "CurveBasisTable";

				dt.Columns.Add("SizeCurveFilter");
				dt.Columns.Add("SizeCurve");

				dt.AcceptChanges();

				return dt;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void SetMerchBasisValues()
		{
			int index;
			SizeCurveMerchBasisProfile scbp;

			try
			{
				index = -1;

				// rebuild grades each time
				_sizeCurveMethod.MerchBasisProfileList.Clear();

				foreach (UltraGridRow gridRow in grdMerchBasis.Rows)
				{
					scbp = new SizeCurveMerchBasisProfile(index);

					if (gridRow.Cells["HN_RID"].Value == DBNull.Value)
					{
						scbp.Basis_HN_RID = Include.NoRID;
					}
					else
					{
						scbp.Basis_HN_RID = Convert.ToInt32(gridRow.Cells["HN_RID"].Value);
					}

					scbp.Basis_FV_RID = Convert.ToInt32(gridRow.Cells["FV_RID"].Value);
					scbp.Basis_CDR_RID = Convert.ToInt32(gridRow.Cells["CDR_RID"].Value);
					scbp.Basis_Weight = Convert.ToDecimal(gridRow.Cells["WEIGHT"].Value);
					scbp.MerchType = (eMerchandiseType)Convert.ToInt32(gridRow.Cells["MERCH_TYPE"].Value);
					scbp.OverrideLowLevelRid = Convert.ToInt32(gridRow.Cells["OLL_RID"].Value);
					scbp.CustomOverrideLowLevelRid = Convert.ToInt32(gridRow.Cells["CUSTOM_OLL_RID"].Value);

					_sizeCurveMethod.MerchBasisProfileList.Add(scbp);

					index--;
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		public void SetCurveBasisValues()
		{
			int index;
			SizeCurveCurveBasisProfile scbp;

			try
			{
				index = -1;

				// rebuild grades each time
				_sizeCurveMethod.CurveBasisProfileList.Clear();

				foreach (UltraGridRow gridRow in grdCurveBasis.Rows)
				{
					scbp = new SizeCurveCurveBasisProfile(index);

					scbp.Basis_SizeCurveGroupRID = Convert.ToInt32(gridRow.Cells["SIZE_CURVE_GROUP_RID"].Value);
					scbp.Basis_Weight = Convert.ToDecimal(gridRow.Cells["WEIGHT"].Value);

					_sizeCurveMethod.CurveBasisProfileList.Add(scbp);

					index--;
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
			UltraGridRow row;
			bool changed;

			try
			{
				if (FormLoaded)
				{
					changed = false;
					row = (UltraGridRow)((frmOverrideLowLevelModel)source).Tag;

					if (Convert.ToInt32(row.Cells["OLL_RID"].Value) != e.aOllRid)
					{
						row.Cells["OLL_RID"].Value = e.aOllRid;

						changed = true;
						ChangePending = true;
					}

					if (Convert.ToInt32(row.Cells["CUSTOM_OLL_RID"].Value) != e.aCustomOllRid)
					{
						row.Cells["CUSTOM_OLL_RID"].Value = e.aCustomOllRid;

						changed = true;
						ChangePending = true;
					}

					if (changed)
					{
						foreach (UltraGridRow gridRow in grdMerchBasis.Rows)
						{
							PopulateOverrideLowLevelValueList(gridRow);
						}
					}
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void BuildMerchBasisContextMenu()
		{
			try
			{
				tsmiMerchBasisInsert.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert);
				tsmiMerchBasisInsertBefore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before);
				tsmiMerchBasisInsertAfter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after);
				tsmiMerchBasisDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
				tsmiMerchBasisDeleteAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void PopulateOverrideLowLevelValueList(UltraGridRow aRow)
		{
			ValueList objValueList;
			ArrayList overrideList;
			string listName;

			try
			{
				overrideList = BuildOverrideModelList(Convert.ToInt32(aRow.Cells["OLL_RID"].Value), Convert.ToInt32(aRow.Cells["CUSTOM_OLL_RID"].Value));

				listName = "OverrideList" + Convert.ToInt32(aRow.Cells["BASIS_SEQ"].Value);

				if (grdMerchBasis.DisplayLayout.ValueLists.Exists(listName))
				{
					objValueList = grdMerchBasis.DisplayLayout.ValueLists[listName];
					objValueList.ValueListItems.Clear();
				}
				else
				{
					objValueList = grdMerchBasis.DisplayLayout.ValueLists.Add(listName);
				}

				objValueList.ValueListItems.Add(Include.NoRID, " ");

				foreach (ComboObject comboObj in overrideList)
				{
					objValueList.ValueListItems.Add(comboObj.Key, comboObj.Value);
				}

				aRow.Cells["OverrideList"].ValueList = objValueList;
			}
			catch (Exception ex)
			{
				HandleException(ex, "NewWorkflowMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// checks to make sure that the previous row, if there is one, is completely
		/// filled out. If any information is missing, return false to the calling
		/// procedure to indicate that it should not proceed adding another row.
		/// </summary>
		/// <returns></returns>
		private bool CheckMerchBasisInsertCondition(UltraGrid aGrid, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			if (aGrid.Rows.Count > 0)
			{
				//Find the last Details row and check its values. 			
				UltraGridRow aRow = aGrid.Rows[aGrid.Rows.Count - 1];

				if (aRow.Cells["Merchandise"].Value.ToString() == string.Empty)
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Merchandise.", "Error");
					return false;
				}
				if (aRow.Cells["DateRange"].Value.ToString() == "")
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Date Range.", "Error");
					return false;
				}
			}
			return true;
		}

		private void InsertMerchBasis(int aCurrentRowBasisSeq, bool InsertBeforeRow)
		{
			int rowPosition;
			int seq;
			DataRow addedRow;

			try
			{
				rowPosition = aCurrentRowBasisSeq;

				foreach (DataRow row in _dtMerchBasis.Rows)
				{
					seq = (int)row["BASIS_SEQ"];

					if (InsertBeforeRow)
					{
						if (seq >= rowPosition)
						{
							row["BASIS_SEQ"] = seq + 1;
						}
					}
					else
					{
						if (seq > rowPosition)
						{
							row["BASIS_SEQ"] = seq + 1;
						}
					}
				}

				if (!InsertBeforeRow)
				{
					rowPosition++;
				}

				addedRow = _dtMerchBasis.NewRow();

				addedRow["BASIS_SEQ"] = rowPosition;
				addedRow["HN_RID"] = Include.NoRID;
				addedRow["FV_RID"] = Include.FV_ActualRID;
				addedRow["CDR_RID"] = Include.UndefinedCalendarDateRange;
				addedRow["WEIGHT"] = 1;
				addedRow["MERCH_TYPE"] = eMerchandiseType.Node;
				addedRow["OLL_RID"] = Include.NoRID;
				addedRow["CUSTOM_OLL_RID"] = Include.NoRID;

				_dtMerchBasis.Rows.Add(addedRow);
				_dtMerchBasis.AcceptChanges();
				grdMerchBasis.DisplayLayout.Bands[0].SortedColumns.Clear();
				grdMerchBasis.DisplayLayout.Bands[0].SortedColumns.Add("BASIS_SEQ", false);

				ChangePending = true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void BuildCurveBasisContextMenu()
		{
			try
			{
				tsmiCurveBasisInsert.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert);
				tsmiCurveBasisInsertBefore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before);
				tsmiCurveBasisInsertAfter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after);
				tsmiCurveBasisDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Delete);
				tsmiCurveBasisDeleteAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void PopulateSizeCurveValueList(UltraGridRow aRow, int aSizeCurveGroupRID)
		{
			try
			{
				PopulateSizeCurveValueList(aRow, aSizeCurveGroupRID, null, false);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void PopulateSizeCurveValueList(UltraGridRow aRow, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				PopulateSizeCurveValueList(aRow, -1, aFilterString, aCaseSensitive);
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void PopulateSizeCurveValueList(UltraGridRow aRow, int aSizeCurveGroupRID, string aFilterString, bool aCaseSensitive)
		{
			DataTable dtSizeCurve;
			ValueList objValueList;
			string listName;
			//string whereClause;

			try
			{
				if (aFilterString != null)
				{
					aFilterString = aFilterString.Replace("*", "%");
                    //aFilterString = aFilterString.Replace("'", "''");	// for string with single quote
                    //whereClause = "SIZE_CURVE_GROUP_NAME LIKE " + "'" + aFilterString + "'";

                    //if (!aCaseSensitive)
                    //{
                    //    whereClause += Include.CaseInsensitiveCollation;
                    //}

                    //dtSizeCurve = _dlSizeCurve.GetFilteredSizeCurveGroups(whereClause);
                    if (aCaseSensitive)
                    {
                        dtSizeCurve = _dlSizeCurve.GetFilteredSizeCurveGroupsCaseSensitive(aFilterString);
                    }
                    else
                    {
                        dtSizeCurve = _dlSizeCurve.GetFilteredSizeCurveGroupsCaseInsensitive(aFilterString);
                    }

				}
				else if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
				{
					dtSizeCurve = _dlSizeCurve.GetSpecificSizeCurveGroup(aSizeCurveGroupRID);
				}
				else
				{
					dtSizeCurve = _dlSizeCurve.GetSizeCurveGroups();
				}

				listName = "SizeCurveList" + Convert.ToInt32(aRow.Cells["BASIS_SEQ"].Value);

				if (grdMerchBasis.DisplayLayout.ValueLists.Exists(listName))
				{
					objValueList = grdMerchBasis.DisplayLayout.ValueLists[listName];
					objValueList.ValueListItems.Clear();
				}
				else
				{
					objValueList = grdMerchBasis.DisplayLayout.ValueLists.Add(listName);
				}

				objValueList.ValueListItems.Add(Include.NoRID, " ");

				foreach (DataRow row in dtSizeCurve.Rows)
				{
					objValueList.ValueListItems.Add(Convert.ToInt32(row["SIZE_CURVE_GROUP_RID"]), Convert.ToString(row["SIZE_CURVE_GROUP_NAME"]));
				}

				aRow.Cells["SizeCurve"].ValueList = objValueList;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// checks to make sure that the previous row, if there is one, is completely
		/// filled out. If any information is missing, return false to the calling
		/// procedure to indicate that it should not proceed adding another row.
		/// </summary>
		/// <returns></returns>
		private bool CheckCurveBasisInsertCondition(UltraGrid aGrid, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			if (aGrid.Rows.Count > 0)
			{
				//Find the last Details row and check its values. 			
				UltraGridRow aRow = aGrid.Rows[aGrid.Rows.Count - 1];

				if (Convert.ToInt32(aRow.Cells["SizeCurve"].Value) == Include.NoRID)
				{
					MessageBox.Show("You must finish filling out the previous row.\r\nData Missing: Size Curve.", "Error");
					return false;
				}
			}
			return true;
		}

		private void InsertCurveBasis(int aCurrentRowBasisSeq, bool InsertBeforeRow)
		{
			int rowPosition;
			int seq;
			DataRow addedRow;

			try
			{
				rowPosition = aCurrentRowBasisSeq;

				foreach (DataRow row in _dtCurveBasis.Rows)
				{
					seq = (int)row["BASIS_SEQ"];

					if (InsertBeforeRow)
					{
						if (seq >= rowPosition)
						{
							row["BASIS_SEQ"] = seq + 1;
						}
					}
					else
					{
						if (seq > rowPosition)
						{
							row["BASIS_SEQ"] = seq + 1;
						}
					}
				}

				if (!InsertBeforeRow)
				{
					rowPosition++;
				}

				addedRow = _dtCurveBasis.NewRow();

				addedRow["BASIS_SEQ"] = rowPosition;
				addedRow["SIZE_CURVE_GROUP_RID"] = Include.NoRID;
				addedRow["WEIGHT"] = 1;

				_dtCurveBasis.Rows.Add(addedRow);
				_dtCurveBasis.AcceptChanges();
				grdCurveBasis.DisplayLayout.Bands[0].SortedColumns.Clear();
				grdCurveBasis.DisplayLayout.Bands[0].SortedColumns.Add("BASIS_SEQ", false);

				ChangePending = true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        private bool ValidBasisGrid()
        {
			double totWeight = 0;
			
			try
            {
				ErrorProvider.SetError(grdMerchBasis, string.Empty);
				ErrorProvider.SetError(grdCurveBasis, string.Empty);

				if (grdMerchBasis.Rows.Count > 0)
                {
					totWeight = 0;

					foreach (UltraGridRow gridRow in grdMerchBasis.Rows)
                    {
						if (!ValidMerchandise(gridRow))
                        {
							return false;
                        }
						if (!ValidDateRange(gridRow))
                        {
							return false;
						}
						if (!ValidWeight(gridRow))
                        {
							return false;
						}
						else if (rbMerchBasisEWYes.Checked)
						{
							totWeight += Convert.ToDouble(gridRow.Cells["WEIGHT"].Value, CultureInfo.CurrentUICulture);
						}
					}

					if (rbMerchBasisEWYes.Checked && totWeight != 1)
					{
						ErrorProvider.SetError(grdMerchBasis, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_BasisWeightTotalInvalid));
						return false;
					}
				}
                else
                {
					ErrorProvider.SetError(grdMerchBasis, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneBasisRequired));
					return false;
                }

				if (grdCurveBasis.Rows.Count > 0)
				{
					foreach (UltraGridRow gridRow in grdCurveBasis.Rows)
					{
						if (!ValidSizeCurveGroup(gridRow))
						{
							return false;
						}
					}
				}

				return true;
            }
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private bool ValidMerchandise(UltraGridRow aGridRow)
		{
			try
			{
				if (Convert.ToInt32(aGridRow.Cells["HN_RID"].Value) == Include.NoRID)
				{
					aGridRow.Cells["Merchandise"].Appearance.Image = ErrorImage;
					aGridRow.Cells["Merchandise"].Tag = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					return false;
				}

				aGridRow.Cells["Merchandise"].Appearance.Image = null;
				aGridRow.Cells["Merchandise"].Tag = null;
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private bool ValidDateRange(UltraGridRow aGridRow)
        {
			try
			{
				if (Convert.ToInt32(aGridRow.Cells["CDR_RID"].Value) == Include.UndefinedCalendarDateRange)
				{
					aGridRow.Cells["DateRange"].Appearance.Image = ErrorImage;
					aGridRow.Cells["DateRange"].Tag = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					return false;
				}

				//Begin TT#778 - JScott - Size Curve Method Date Range Dynamic Icon dissapears when selecting Process
				if (aGridRow.Cells["DateRange"].Appearance.Image == ErrorImage)
				{
				//End TT#778 - JScott - Size Curve Method Date Range Dynamic Icon dissapears when selecting Process
					aGridRow.Cells["DateRange"].Appearance.Image = null;
					aGridRow.Cells["DateRange"].Tag = null;
				//Begin TT#778 - JScott - Size Curve Method Date Range Dynamic Icon dissapears when selecting Process
				}

				//End TT#778 - JScott - Size Curve Method Date Range Dynamic Icon dissapears when selecting Process
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
        }

		private bool ValidWeight(UltraGridRow aGridRow)
        {
            double dblValue;

            try
            {
				if (aGridRow.Cells["WEIGHT"].Text.Length == 0)
                {
					aGridRow.Cells["WEIGHT"].Appearance.Image = ErrorImage;
					aGridRow.Cells["WEIGHT"].Tag = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					return false;
                }

				dblValue = Convert.ToDouble(aGridRow.Cells["WEIGHT"].Text, CultureInfo.CurrentUICulture);

                if (dblValue > 9999)
                {
					aGridRow.Cells["WEIGHT"].Appearance.Image = ErrorImage;
					aGridRow.Cells["WEIGHT"].Tag = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_MaximumValueExceeded), dblValue, "9999");
					return false;
                }

				aGridRow.Cells["WEIGHT"].Appearance.Image = null;
				aGridRow.Cells["WEIGHT"].Tag = null;
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
        }

		private bool ValidSizeCurveGroup(UltraGridRow aGridRow)
		{
			try
			{
				if (Convert.ToInt32(aGridRow.Cells["SizeCurve"].Value) == Include.NoRID)
				{
					aGridRow.Cells["SizeCurve"].Appearance.Image = ErrorImage;
					aGridRow.Cells["SizeCurve"].Tag = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					return false;
				}

				aGridRow.Cells["SizeCurve"].Appearance.Image = null;
				aGridRow.Cells["SizeCurve"].Tag = null;
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		//Begin TT#1076 - JScott - Size Curves by Set
		private bool ValidStoreGroup()
		{
			try
			{
				if (Convert.ToInt32(cboSizeCurveBySet.SelectedValue, CultureInfo.CurrentUICulture) <= 0)
				{
					ErrorProvider.SetError(cboSizeCurveBySet, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeCurveBySetRequired));
					return false;
				}
				else
				{
					ErrorProvider.SetError(cboSizeCurveBySet, string.Empty);
					return true;
				}
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		//End TT#1076 - JScott - Size Curves by Set
		#endregion
		#region SizeMethodsFormBase Overrides

		override protected bool IsActiveRowValid(UltraGridRow activeRow)
		{
			try
			{
				bool IsValid = true;

				//COMMON BAND VALIDATIONS
				//==================================================================================

				if (!IsSizeRuleValid(activeRow))
				{
					IsValid = false;
				}
				//ADD ADDITIONAL COMMON VALIDATIONS HERE

				//=================================================================================


				//BAND SPECIFIC VALIDATIONS
				//=================================================================================
				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						break;
					case C_SET_CLR:
						if (!IsColorCodeValid(activeRow))
						{
							IsValid = false;
						}
						break;
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						if (!IsSizeValid(activeRow))
						{
							IsValid = false;
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (!IsDimensionValid(activeRow))
						{
							IsValid = false;
						}
						break;
				}

				return IsValid;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		override protected void PositionColumns()
		{
			try
			{
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

        /// <summary>
        /// Main validation routine for All Sizes Grid.  Each row will be sent to 
        /// have row validation done on it (IsActiveRowValid)
        /// </summary>
        /// <returns>Boolean value True=Valid|False=InValid</returns>
        /// <remarks>Method must be overridden</remarks>
		override protected bool IsGridValid()
		{
			try
			{
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		#endregion
		#region IFormBase Overrides

		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ISave");
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
 		}
        #endregion

        private void frmSizeCurveMethod_Load(object sender, EventArgs e)
        {
            bool stop = true;
        }

        private void cboSizeCurveBySet_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSizeCurveBySet_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
