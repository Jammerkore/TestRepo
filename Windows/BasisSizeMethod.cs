using System;
using System.Drawing;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using MIDRetail.Common;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
using Infragistics.Shared;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmBasisSizeMethod.
	/// </summary>
	public class frmBasisSizeMethod : SizeMethodsFormBase
	{
		#region Member Variables
		private int _nodeRID = -1;
//		private string _strMethodType;
		private BasisSizeAllocationMethod _basisSizeMethod;
		private AllocationProfile _basisHeaderProfile = null;
		private AllocationProfile _activeHeaderProfile = null;
		private DataTable _ColorDataTable = null;
		private DataTable _RulesDataTable = null;
		private DataTable _componentDataTable = null;
		private ArrayList _hiddenColumns; 
		private string _noSizeDimensionLbl;
        // Begin TT#319-MD - JSmith - Delay when selecting Size Curve 
        private int _currSizeRID = Include.NoRID;
        private eGetSizes _currGetSizes;
        // End TT#319-MD - JSmith - Delay when selecting Size Curve

		#endregion

		#region Form Fields
		private System.Windows.Forms.TabPage tabGeneral;
		private System.Windows.Forms.TabPage tabConstraints;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.Panel pnlGridContainer;
		private System.Windows.Forms.TextBox txtBasisHeader;
		private System.Windows.Forms.Label lblBasisHeader;
        private Controls.MIDComboBoxEnh cboBasisColor;
		private System.Windows.Forms.Label lblBasisColor;
		private System.Windows.Forms.Button btnGetHeader;
        private Controls.MIDComboBoxEnh cboBasisComponent;
		private System.Windows.Forms.Label lblBasisComponent;
        private Controls.MIDComboBoxEnh cboRule;
		private System.Windows.Forms.Button cmdSizeOverride;
		private System.Windows.Forms.GroupBox grpPrior;
		#endregion
		private System.Windows.Forms.Label lblQuantity;
		private System.Windows.Forms.TextBox txtQuantity;
		private System.Windows.Forms.TabControl tabControl3;
		private System.Windows.Forms.TabPage tabSubstitutes;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugSubstitutes;
		private System.Windows.Forms.Label lblEmptyGrid;
		private System.Windows.Forms.GroupBox gbRule;
        private CheckBox ckbIncludeReserve;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructor/Dispose
		public frmBasisSizeMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_BasisSizeMethod, eWorkflowMethodType.Method)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserBasisSize);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalBasisSize);
				_hiddenColumns = new ArrayList();
				this.lblEmptyGrid.Visible = false;

                // Begin TT#41-MD - GTaylor - UC #2 - hide the inherited elements
                lblInventoryBasis.Visible = false;
                cboInventoryBasis.Visible = false;
                // End TT#41-MD - GTaylor - UC #2
            }
			catch
			{
				FormLoadError = true;
				throw;
			}
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
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                this.cbxUseDefaultcurve.CheckedChanged -= new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                //this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                // End TT#301-MD - JSmith - Controls are not functioning properly
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Control Event Handlers
		
		override protected bool ApplySecurity()	// track #5871 stodd
		{
			bool securityOk = true; // track #5871 stodd
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
			return securityOk; // track #5871 stodd
		}

		private void cmdSizeOverride_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("This will be available when Size Override method is completed.");
		}


		private void txtDesc_TextChanged(object sender, System.EventArgs e)
		{
			ChangePending = true;
		}


		private void txtBasisHeader_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				this.DoPriorDragEnter(sender, e);

				//				AttachErrors(txtBasisHeader);
				//
				//				if (e.Data.GetDataPresent(typeof(SelectedHeaderProfile)))
				//				{
				//					e.Effect = DragDropEffects.Move;
				//					return;
				//				}
				//				else
				//				{
				//					e.Effect = DragDropEffects.None;
				//				}
				//			
				//				//Data source is unknown, create error provider.
				//				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidDataType));
				//				AttachErrors(txtBasisHeader);
			}
			catch( Exception ex )
			{
				HandleException(ex, "txtBasisHeader_DragEnter");
			}

		}


		private void txtBasisHeader_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				this.DoPriorDragDrop(sender, e);


				//				AttachErrors(txtBasisHeader);
				//
				//				bool hasErrors = false;
				//				SelectedHeaderProfile shp = null;
				//
				//				shp = (SelectedHeaderProfile)e.Data.GetData(typeof(SelectedHeaderProfile));
				//
				//				if (shp == null)
				//				{
				//					hasErrors = true;
				//					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidDataType));
				//				}
				//
				//				if (!hasErrors)
				//				{
				//					ProcessReceivedHeader(shp);
				//				}
				//				else
				//				{
				//					AttachErrors(txtBasisHeader);
				//				}
			}							
			catch( Exception ex )
			{
				HandleException(ex, "txtBasisHeader_DragDrop");
			}

		}


		
		private void grpPrior_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				this.DoPriorDragDrop(sender, e);
			}							
			catch( Exception ex )
			{
				HandleException(ex, "grpPrior_DragDrop");
			}
		}


		private void DoPriorDragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				AttachErrors(txtBasisHeader);

				bool hasErrors = false;
				SelectedHeaderProfile shp = null;

				shp = (SelectedHeaderProfile)e.Data.GetData(typeof(SelectedHeaderProfile));

				if (shp == null)
				{
					hasErrors = true;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidDataType));
				}

				if (!hasErrors)
				{
					ProcessReceivedHeader(shp);
				}
				else
				{
					AttachErrors(txtBasisHeader);
				}
			}							
			catch( Exception ex )
			{
				HandleException(ex, "DoPriorDragDrop");
			}
		}


		private void DoPriorDragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				AttachErrors(txtBasisHeader);

				if (e.Data.GetDataPresent(typeof(SelectedHeaderProfile)))
				{
					e.Effect = DragDropEffects.Move;
					return;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			
				//Data source is unknown, create error provider.
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidDataType));
				AttachErrors(txtBasisHeader);
			}
			catch( Exception ex )
			{
				HandleException(ex, "DoPriorDragEnter");
			}
		}


		private void grpPrior_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				this.DoPriorDragEnter(sender, e);
			}
			catch( Exception ex )
			{
				HandleException(ex, "grpPrior_DragEnter");
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboBasisComponent_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboBasisComponent_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (cboBasisComponent.SelectedIndex > -1)
				{
					DataRow dr = _componentDataTable.Rows[cboBasisComponent.SelectedIndex];
					eComponentType bct = (eComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));

					if (bct == eComponentType.SpecificColor)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							cboBasisColor.Enabled = true;
						}
                        if (_ColorDataTable.Rows.Count > 0)
                        {
                            cboBasisColor.SelectedIndex = 0;
                            //this.cboBasisColor_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                        }
					}
					else
					{
						cboBasisColor.SelectedIndex = -1;
                        //this.cboBasisColor_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						cboBasisColor.Enabled = false;
					}

					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
				
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
			
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboBasisColor_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboBasisColor_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
            if (FormLoaded)
            {
                ChangePending = true;
            }
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboConstraints_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboConstraints_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
            if (FormLoaded)
            {
                ChangePending = true;
            }
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboRule_SelectionChangeCommitted(object sender, System.EventArgs e)
        private void cboRule_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
            if (FormLoaded)
            {
                ChangePending = true;
            }
			if (cboRule.SelectedIndex > -1)
			{
				DataRowView drv = (DataRowView)cboRule.SelectedItem;
				DataRow dr = drv.Row;

				// MID Issue 2514 stodd
				eBasisSizeRuleRequiresQuantity rqm = (eBasisSizeRuleRequiresQuantity)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
				if (Enum.IsDefined(typeof(eBasisSizeRuleRequiresQuantity),rqm))
				{
					txtQuantity.Enabled = true;
				}
				else
				{
					txtQuantity.Text = string.Empty;
					txtQuantity.Enabled = false;
				}

				//				if (FormLoaded)
				//				{
				//					ChangePending = true;
				//				}
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeGroup_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
                if (base.FormLoaded)
                {
                    if (_basisSizeMethod.PromptSizeChange)
                    {
                        int newRid = Convert.ToInt32(cboSizeGroup.SelectedValue.ToString(), CultureInfo.CurrentUICulture); ;
                        if (newRid != _basisSizeMethod.SizeGroupRid)  // If selection has really changed...
                        {
                            if (ShowWarningPrompt(true) == DialogResult.Yes)
                            {
                                this.Cursor = Cursors.WaitCursor;
                                _basisSizeMethod.DeleteMethodRules(new TransactionData());
                                _basisSizeMethod.SizeGroupRid = newRid;
                                if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
                                {
                                    // Change the other (Size Curve) combo to "empty"
                                    if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
                                    {
                                        _basisSizeMethod.SizeCurveGroupRid = Include.NoRID; ;
                                        _basisSizeMethod.PromptSizeChange = false;
                                        cboSizeCurve.SelectedValue = Include.NoRID;
                                        //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                                    }
                                    _basisSizeMethod.GetSizesUsing = eGetSizes.SizeGroupRID;
                                    _basisSizeMethod.GetDimensionsUsing = eGetDimensions.SizeGroupRID;
                                }
                                _basisSizeMethod.CreateConstraintData();
                                BindAllSizeGrid(_basisSizeMethod.MethodConstraints);
                                _basisSizeMethod.SubstituteList.Clear();  // Size group has changed, wipe out previous data
                                BindSubstitutesGrid(_basisSizeMethod.SizeGroupRid, eGetSizes.SizeGroupRID);

                                if (_basisSizeMethod.SizeGroupRid == Include.NoRID && _basisSizeMethod.SizeCurveGroupRid == Include.NoRID)
                                    this.lblEmptyGrid.Visible = true;
                                else
                                    this.lblEmptyGrid.Visible = false;
                                CheckExpandAll();
                                _basisSizeMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference

                                ChangePending = true;
                            }

                            else
                            {
                                this.Cursor = Cursors.WaitCursor;
                                //Shut off the prompt so the combo can be reset to original value.
                                _basisSizeMethod.PromptSizeChange = false;
                                cboSizeGroup.SelectedValue = _basisSizeMethod.SizeGroupRid;
                                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                                _basisSizeMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
                            }
                        }
                        else
                        {
                            _basisSizeMethod.PromptSizeChange = true;
                        }
                    }
                    else
                    {
                        //Turn the prompt back on.
                        _basisSizeMethod.PromptSizeChange = true;
                    }
                    // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                    SetApplyRulesOnly_State();
                    // end TT#2155 - JEllis - Fill Size holes Null Reference
                }
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
            finally
            {
                this.Cursor = Cursors.Default;
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
                {   
					if (_basisSizeMethod.PromptSizeChange)
					{
						int newRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
						if (newRid != _basisSizeMethod.SizeCurveGroupRid)  // If selection has really changed...
						{
                            // Begin TT#499 - RMatelic - Check Rules and Basis Substitutes to determine if warning message is shown
                            if (RuleExists() || BasisSubstituteExists())
                            {
                                if (ShowWarningPrompt(true) == DialogResult.Yes)
                                {
                                    //_basisSizeMethod.DeleteMethodRules(new TransactionData());
                                    //_basisSizeMethod.SizeCurveGroupRid = newRid;
                                    //if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
                                    //{
                                    //    // Change the other (Size Group) combo to "empty"
                                    //    if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
                                    //    {
                                    //        _basisSizeMethod.SizeGroupRid = Include.NoRID;
                                    //        _basisSizeMethod.PromptSizeChange = false;
                                    //        cboSizeGroup.SelectedValue = Include.NoRID;
                                    //    }
                                    //    _basisSizeMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                                    //    _basisSizeMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                                    //}
                                    //_basisSizeMethod.CreateConstraintData();
                                    //BindAllSizeGrid(_basisSizeMethod.MethodConstraints);
                                    //_basisSizeMethod.SubstituteList.Clear();  // Size group has changed, wipe out previous data
                                    //BindSubstitutesGrid(_basisSizeMethod.SizeCurveGroupRid, eGetSizes.SizeCurveGroupRID);

                                    //if (_basisSizeMethod.SizeGroupRid == Include.NoRID && _basisSizeMethod.SizeCurveGroupRid == Include.NoRID)
                                    //    this.lblEmptyGrid.Visible = true;
                                    //else
                                    //    this.lblEmptyGrid.Visible = false;

                                    //CheckExpandAll();

                                    //ChangePending = true;
                                    this.Cursor = Cursors.WaitCursor;
                                    UpdateCurveData();
                                }
                                else
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    //Shut off the prompt so the combo can be reset to original value.
                                    _basisSizeMethod.PromptSizeChange = false;
                                    cboSizeCurve.SelectedValue = _basisSizeMethod.SizeCurveGroupRid;
                                    //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                                    _basisSizeMethod.PromptSizeChange = true; // TT#2155 - JEllis - Fill Size Holes Null Reference
                                }
                            }
                            else
                            {
                                this.Cursor = Cursors.WaitCursor;
                                UpdateCurveData();
                            }
                        }   // End TT#499
						else
						{
							_basisSizeMethod.PromptSizeChange = true;
						}
					}
					else
					{
						//Turn the prompt back on.
						_basisSizeMethod.PromptSizeChange = true;
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboSizeGroup_SelectionChangeCommitted");
                HandleException(ex, "cboSizeCurve_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
            finally
            {
                this.Cursor = Cursors.Default;
			}
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboBasisColor_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboBasisColor_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboBasisComponent_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboBasisComponent_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboRule_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboRule_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control


        // Begin TT#499 - RMatelic - Size Need Method Default checked on the Rule Tab the Add only goes down to Color - move code for RuleExists Check & add public event
        // when updated by SizeMethodForm Base
        private void UpdateCurveData()
        {
            try
            {
                int newRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(), CultureInfo.CurrentUICulture);
                _basisSizeMethod.DeleteMethodRules(new TransactionData());
                _basisSizeMethod.SizeCurveGroupRid = newRid;
                if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
                {
                    // Change the other (Size Group) combo to "empty"
                    if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
                    {
                        _basisSizeMethod.SizeGroupRid = Include.NoRID;
                        _basisSizeMethod.PromptSizeChange = false;
                        cboSizeGroup.SelectedValue = Include.NoRID;
                        //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    }
                    _basisSizeMethod.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                    _basisSizeMethod.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                }
                SetApplyRulesOnly_State(); // TT#2155 - JEllis - Fill Size Holes Null Reference
                _basisSizeMethod.CreateConstraintData();
                BindAllSizeGrid(_basisSizeMethod.MethodConstraints);
                _basisSizeMethod.SubstituteList.Clear();  // Size group has changed, wipe out previous data
                BindSubstitutesGrid(_basisSizeMethod.SizeCurveGroupRid, eGetSizes.SizeCurveGroupRID);

                SetSubsituteGridMessage();
                CheckExpandAll();
                ChangePending = true;
                _basisSizeMethod.PromptSizeChange = true;
            }
            catch
            {
                throw;
            }
        }
        // begin TT#2155 - JEllis - Fill Size Holes Null Reference
        private void cbxUseDefaultCurve_CheckChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                HandleException(ex, "cbxUseDefaultCurve_CheckChanged");
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboHierarchyLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboHierarchyLevel_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboHierarchyLevel_SelectionChangeCommitted");
                HandleException(ex, "cboHierarchyLevel_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboHeaderChar_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboHeaderChar_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboHeaderChar_SelectionChangeCommitted");
                HandleException(ex, "cboHeaderChar_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboNameExtension_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboNameExtension_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
        {
            try
            {
                if (base.FormLoaded)
                {
                    SetApplyRulesOnly_State();
                }

            }
            catch (Exception ex)
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboNameExtension_SelectionChangeCommitted");
                HandleException(ex, "cboNameExtension_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
            }
        }

        private void SetApplyRulesOnly_State()
        {
            cbxApplyRulesOnly.Visible = false;
            cbxApplyRulesOnly.Checked = false;
            //if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
            //{
            //    if (!cbxUseDefaultcurve.Checked
            //       && Convert.ToInt32(cboHierarchyLevel.SelectedValue, CultureInfo.CurrentUICulture) == -2
            //       && Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID
            //       && Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture) == Include.NoRID)
            //    {
            //        cbxApplyRulesOnly.Checked = false;
            //        _basisSizeMethod.ApplyRulesOnly = false;
            //        cbxApplyRulesOnly.Enabled = false;
            //    }
            //    else
            //    {
            //        cbxApplyRulesOnly.Enabled = true;
            //    }
            //}
            //else if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
            //{
            //    cbxApplyRulesOnly.Enabled = true;
            //}
            //else
            //{
            //    cbxApplyRulesOnly.Checked = false;
            //    _basisSizeMethod.ApplyRulesOnly = false;
            //    cbxApplyRulesOnly.Enabled = false;
            //}
        }
        // end TT#2155 - JEllis - Fill Size Holes Null Reference
        public void SetSubsituteGridMessage()
        {
            try
            {
                if (_basisSizeMethod.SizeGroupRid == Include.NoRID && _basisSizeMethod.SizeCurveGroupRid == Include.NoRID)
                {
                    this.lblEmptyGrid.Visible = true;
                }
                else
                {
                    this.lblEmptyGrid.Visible = false;
                }
            }
            catch
            {
                throw;
            }    
        }
        // End TT#499 

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        override protected void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				if (base.FormLoaded)
				{
					//if (_basisSizeMethod.SgRid != Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture))
					//{
					if (_basisSizeMethod.PromptAttributeChange)
					{
						if (ShowWarningPrompt(false) == DialogResult.Yes)
						{

							_basisSizeMethod.DeleteMethodRules(new TransactionData());

							_basisSizeMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
							_basisSizeMethod.CreateConstraintData();
							BindAllSizeGrid(_basisSizeMethod.MethodConstraints);
							CheckExpandAll();
							ChangePending = true;
						}
						else
						{
							//Shut off the prompt so the combo can be reset to original value.
							_basisSizeMethod.PromptAttributeChange = false;
							cboStoreAttribute.SelectedValue = _basisSizeMethod.SG_RID;
                            //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						}
					}
					else
					{
						//Turn the prompt back on.
						_basisSizeMethod.PromptAttributeChange = true;
					}
				}
			}
			catch (Exception ex)
			{
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //HandleException(ex, "cboStoreAttribute_SelectionChangeCommitted");
                HandleException(ex, "cboStoreAttribute_SelectionChangeCommitted");
                // End TT#301-MD - JSmith - Controls are not functioning properly
			}
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        //{

        //}

        //private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        //{
        //    //Begin Track #5858 - Kjohnson - Validating store security only
        //    try
        //    {
        //        bool isSuccessfull = ((MIDComboBoxTag)(((MIDComboBoxEnh)sender).Tag)).ComboBox_DragDrop(sender, e);

        //        if (isSuccessfull)
        //        {
        //            ChangePending = true;
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        HandleException(exc);
        //    }
        //    //End Track #5858
        //}
        // End TT#301-MD - JSmith - Controls are not functioning properly

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch
//			{
//				MessageBox.Show("Error in btnClose_Click");
//			}
//		}
//
//
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save_Click(true);
//			}
//			catch( Exception exception )
//			{
//				HandleException(exception);
//			}		
//		}
//
//
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{	// begin A&F add Generic Size Curve Name 
//				if (!OkToProcess())		 
//				{
//					return;
//				}						
//				// end A&F add Generic Size Curve Name  
//
//				ProcessAction(eMethodType.BasisSizeAllocation);
//
//				// as part of the  processing we saved the info, so it should be changed to update.
//				_basisSizeMethod.Method_Change_Type = eChangeType.update;
//				btnSave.Text = "&Update";
//
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
//		}

//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch
//			{
//				MessageBox.Show("Error in btnClose_Click");
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

		protected override void Call_btnProcess_Click()
		{
			try
			{	// begin A&F add Generic Size Curve Name 
				// BEGIN TT#696-MD - Stodd - add "active process"
				//====================================================
				// Checks to be sure there are valid selected headers
				//====================================================
				if (!OkToProcess(this, eMethodType.BasisSizeAllocation))		 
				{
					return;
				}
				// END TT#696-MD - Stodd - add "active process"						
				// end A&F add Generic Size Curve Name  

				ProcessAction(eMethodType.BasisSizeAllocation);

				// as part of the  processing we saved the info, so it should be changed to update.
				//Begin TT#991 - JScott - Processed without changing focus to the header I wanted to Allocate(still focused on the prior header) selected process and received a null reference exception.
				//_basisSizeMethod.Method_Change_Type = eChangeType.update;
				//btnSave.Text = "&Update";
				if (!ErrorFound)
				{
					_basisSizeMethod.Method_Change_Type = eChangeType.update;
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}
				//End TT#991 - JScott - Processed without changing focus to the header I wanted to Allocate(still focused on the prior header) selected process and received a null reference exception.
			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858


		private void btnGetHeader_Click(object sender, System.EventArgs e)
		{
			try
			{
				SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();

				if (selectedHeaderList.Count == 0)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
					return;
				}
				if (selectedHeaderList.Count > 1)
				{
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultHeadersSelectedOnWorkspace));
					return;
				}
				if (selectedHeaderList.Count == 1)
				{
					SelectedHeaderProfile shp = (SelectedHeaderProfile)selectedHeaderList[0];

					ProcessReceivedHeader(shp);

					cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.MatchingColor;
                    //this.cboBasisComponent_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					//SetIncludeCombo();	
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Private method loads the Basis Size Data associated with the current Basis Size Method.
		/// </summary>
		private void LoadBasisSizeValues()
		{
			try
			{
				//chkSizeEquates.Checked = _basisSizeMethod.EquateSizeInd;


				GetRules();
				cboRule.SelectedValue = _basisSizeMethod.Rule;
                //this.cboRule_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly

				if (_basisSizeMethod.BasisHdrRid != Include.NoRID)
				{
					_activeHeaderProfile = GetAllocationProfile(_basisSizeMethod.BasisHdrRid);
					txtBasisHeader.Text = _activeHeaderProfile.HeaderID;
					GetColors(_activeHeaderProfile.HeaderRID);

					cboBasisColor.SelectedValue = _basisSizeMethod.BasisClrRid;
                    //this.cboBasisColor_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					cboBasisComponent.Enabled = true;
                    ckbIncludeReserve.Enabled = true; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
					cboBasisComponent.SelectedValue = _basisSizeMethod.HeaderComponent;
                    //this.cboBasisComponent_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}
				else
				{
					cboBasisComponent.Enabled = false;
                    ckbIncludeReserve.Enabled = false; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
				}

				if (_basisSizeMethod.StoreFilterRid != Include.NoRID)
				{
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_basisSizeMethod.StoreFilterRid, -1, ""));
				}

				if (_basisSizeMethod.SG_RID != Include.NoRID)
				{
					cboStoreAttribute.SelectedValue = _basisSizeMethod.SG_RID;
                    //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    // Begin Track #4872 - JSmith - Global/User Attributes
                    //if (cboStoreAttribute.ContinueReadOnly)
                    //{
                    //    SetMethodReadOnly();
                    //}
                    // End Track #4872
                    if (FunctionSecurity.AllowUpdate)
                    {
                        if (cboStoreAttribute.ContinueReadOnly)
                        {
                            SetMethodReadOnly();
                        }
                    }
                    else
                    {
                        cboStoreAttribute.Enabled = false;
                    }
                    // End TT#1530
				}
				else
				{
					cboStoreAttribute.SelectedValue = SAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
                    //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                    // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
                    cboStoreAttribute.Enabled = FunctionSecurity.AllowUpdate;
                    // End TT#1530
				}

                AdjustTextWidthComboBox_DropDown(cboStoreAttribute); //TT#7 - MD - RBeck - Dynamic dropdowns

				if (_basisSizeMethod.SizeConstraintRid != Include.NoRID)
				{
					cboConstraints.SelectedValue = _basisSizeMethod.SizeConstraintRid;
                    //this.cboConstraints_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}
				else
				{
					cboConstraints.SelectedValue = Include.NoRID;
                    //this.cboConstraints_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}

				if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
				{
					cboSizeGroup.SelectedValue = _basisSizeMethod.SizeGroupRid;
                    //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					BindSubstitutesGrid(_basisSizeMethod.SizeGroupRid, eGetSizes.SizeGroupRID);
				}
				else if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
				{
					cboSizeCurve.SelectedValue = _basisSizeMethod.SizeCurveGroupRid;
                    //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					BindSubstitutesGrid(_basisSizeMethod.SizeCurveGroupRid, eGetSizes.SizeCurveGroupRID);
				}
				cboBasisColor.Enabled = false;

				// begin Generic Size Curve data
				if (_basisSizeMethod.GenCurveCharGroupRID != Include.NoRID)
				{
					this.cboHeaderChar.SelectedValue = _basisSizeMethod.GenCurveCharGroupRID;
                    //this.cboHeaderChar_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				}

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                this.cboNameExtension.SelectedValue = _basisSizeMethod.GenCurveNsccdRID;
                // End TT#413  

                switch (_basisSizeMethod.GenCurveMerchType)
				{
					case eMerchandiseType.Undefined:
						cboHierarchyLevel.SelectedIndex = 0;
                        //this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						break;
				
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.HierarchyLevel:
						SetGenSizeCurveComboToLevel(_basisSizeMethod.GenCurvePhlSequence);
						break;
					
					case eMerchandiseType.Node:
						//Begin Track #5378 - color and size not qualified
//						HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_basisSizeMethod.GenCurveHnRID);
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_basisSizeMethod.GenCurveHnRID, true, true);
						//End Track #5378
						AddNodeToGenSizeCurveCombo(hnp);
						break;
					
				}
				cbColor.Checked = _basisSizeMethod.GenCurveColorInd;
                // Begin TT#438 - RMatelic - Size Curve generated in node properties- used default in Size Need method but requires a size curve (incorrect)
                if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName && cbColor.Checked)
                {
                    cbColor.Checked = false;
                }
                // End TT#438  
				// end Generic Size Curve data
				
				// begin Generic Size Constraint data
				if (_basisSizeMethod.GenConstraintCharGroupRID != Include.NoRID)
				{
					this.cboConstrHeaderChar.SelectedValue = _basisSizeMethod.GenConstraintCharGroupRID;
				}
					
				switch (_basisSizeMethod.GenConstraintMerchType)
				{
					case eMerchandiseType.Undefined:
						cboConstrHierLevel.SelectedIndex = 0;
						break;
				
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.HierarchyLevel:
						SetGenSizeConstraintComboToLevel(_basisSizeMethod.GenConstraintPhlSequence);
						break;
					
					case eMerchandiseType.Node:
						HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_basisSizeMethod.GenConstraintHnRID);
						AddNodeToGenSizeConstraintCombo(hnp);
						break;
					
				}
					
				cbConstrColor.Checked = _basisSizeMethod.GenConstraintColorInd;
				// end Generic Size Constraint data
                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                SetApplyRulesOnly_State();
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                SetSubsituteGridMessage();  //TT#499 - set basis substitute grid
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error in Load Basis Size Values");
			}

					
		}


		/// <summary>
		/// Populates the datatable used to bind cboRule.
		/// </summary>
		private void GetRules()
		{
			try
			{
				_RulesDataTable.Rows.Add(new object[] { "",Include.NoRID} );
	  		
				// these are done one at a time to get them in alpha order
				DataTable dtRules = MIDText.GetLabels((int) eBasisSizeMethodRuleType.Exclude,(int) eBasisSizeMethodRuleType.AbsoluteQuantity);
				foreach (DataRow row in dtRules.Rows)
				{
					DataRow nRow = _RulesDataTable.NewRow();
					nRow["TEXT_CODE"] = row["TEXT_CODE"];
					nRow["TEXT_VALUE"] = row["TEXT_VALUE"];
					_RulesDataTable.Rows.Add(nRow);
				}

				dtRules = MIDText.GetLabels((int) eBasisSizeMethodRuleType.ProportionalAllocated,(int) eBasisSizeMethodRuleType.Fill);
				
				foreach (DataRow row in dtRules.Rows)
				{
					DataRow nRow = _RulesDataTable.NewRow();
					nRow["TEXT_CODE"] = row["TEXT_CODE"];
					nRow["TEXT_VALUE"] = row["TEXT_VALUE"];
					_RulesDataTable.Rows.Add(nRow);
				}

				_RulesDataTable.DefaultView.Sort = "TEXT_VALUE ASC"; 
			}
			catch( Exception ex )
			{
				HandleException(ex, "GetRules");
			}
		}


		/// <summary>
		/// Populates the datatable used to bind cboBasisColor.
		/// </summary>
		/// <param name="aHeaderRID">Key of the header to get basis colors for.</param>
		private void GetColors(int aHeaderRID)
		{
			try
			{
				Header header = new Header();
				_ColorDataTable.Clear();
				DataTable dtBulkColors = header.GetBulkColors(aHeaderRID);

				if (dtBulkColors.Rows.Count > 0)
				{	
					foreach (DataRow cRow in dtBulkColors.Rows)
					{
						int colorKey = Convert.ToInt32(cRow["COLOR_CODE_RID"],CultureInfo.CurrentUICulture);
						ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorKey);
						_ColorDataTable.Rows.Add( new object[] {colorKey, ccp.ColorCodeName} ) ;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "GetColors");
			}
			 
		}


		/// <summary>
		/// Loads datatable and binds to cboBasisComponent
		/// </summary>
		private void BindComponentComboBox()
		{
			try
			{
				_componentDataTable = MIDText.GetTextType(eMIDTextType.eComponentType, eMIDTextOrderBy.TextValue);
		
				foreach(DataRow dr in _componentDataTable.Rows)
				{
					switch ((eRuleMethodComponentType)(Convert.ToInt32(dr["TEXT_CODE"])))
					{
						case eRuleMethodComponentType.MatchingColor:
						case eRuleMethodComponentType.SpecificColor:
							break;
						default:
							dr.Delete();
							break;
					}
				}

				_componentDataTable.AcceptChanges();
				this.cboBasisComponent.DataSource = _componentDataTable;
				this.cboBasisComponent.DisplayMember = "TEXT_VALUE";
				this.cboBasisComponent.ValueMember = "TEXT_CODE";
			}
			catch( Exception ex )
			{
				HandleException(ex, "BindComponentComboBox");
			}
		}

		/// <summary>
		/// Private method that initializes controls on the form
		/// </summary>
		private void LoadMethods()
		{
			try
			{			
                //Begin Track #5858 - KJohnson - Validating store security only
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
                cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, _basisSizeMethod.GlobalUserType == eGlobalUserType.User);
                // End TT#44
                //End Track #5858
				BindFilterComboBox(); //Inherited from SizeMethodsFormBase
                // Begin Track #4872 - JSmith - Global/User Attributes
                //BindStoreAttrComboBox(); //Inherited from SizeMethodsFormBase
                BindStoreAttrComboBox(_basisSizeMethod.Method_Change_Type, _basisSizeMethod.GlobalUserType); //Inherited from SizeMethodsFormBase
                // End Track #4872
				BindComponentComboBox();
				BindAllSizeGrid(_basisSizeMethod.MethodConstraints); //Inherited from SizeMethodsFormBase
		
				// BEGIN ANF Generic Size Constraint
				BindSizeComboBoxes(_basisSizeMethod.SizeGroupRid, Include.NoRID, 
						           _basisSizeMethod.SizeCurveGroupRid,_basisSizeMethod.SizeConstraintRid);
				// END ANF Generic Size Constraint
			
				// ANF - add Generic Size Curve
				LoadGenericSizeCurveGroupBox();

				grpPrior.AllowDrop = true;
				txtName.Text = _basisSizeMethod.Name;
				txtDesc.Text = _basisSizeMethod.Method_Description;
                ckbIncludeReserve.Checked = _basisSizeMethod.IncludeReserve;  // TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
				
				//OK If New
				LoadBasisSizeValues();
			}
			catch (Exception ex)
			{
				HandleException(ex, "LoadMethods");
			}
		}


		/// <summary>
		/// Controls the text for the Process, {Save,Update}, and Close buttons.
		/// </summary>
		private void SetText()
		{
			try
			{
				if (_basisSizeMethod.Method_Change_Type == eChangeType.update)
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				else
					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);

				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
				this.btnProcess.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Process);
                this.lblEmptyGrid.Text = MIDText.GetTextOnly(eMIDTextCode.msg_al_NoSizeCurveOrGroupSelected);
                this.ckbIncludeReserve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeReserve);
			}
			catch (Exception ex)
			{
				HandleException(ex, "SetText");
			}
		}


		/// <summary>
		/// Creates the structure of the datatable and binds to cboBasisColor.
		/// </summary>
		private void CreateColorDataTable()
		{
			try
			{
				_ColorDataTable = MIDEnvironment.CreateDataTable();
				_ColorDataTable.Columns.Add("ColorRID");
				_ColorDataTable.Columns.Add("ColorName");
				this.cboBasisColor.DataSource = _ColorDataTable;
				this.cboBasisColor.DisplayMember = "ColorName";
				this.cboBasisColor.ValueMember = "ColorRID";
			}
			catch( Exception ex )
			{
				HandleException(ex, "CreateColorDataTable");
			}
		}


		/// <summary>
		/// Creates the structure of the datatable and binds to cboRule.
		/// </summary>
		private void CreateRuleDataTable()
		{
			try
			{
				_RulesDataTable = MIDEnvironment.CreateDataTable();
				_RulesDataTable.Columns.Add("TEXT_VALUE");
				_RulesDataTable.Columns.Add("TEXT_CODE");
				cboRule.DataSource = _RulesDataTable;
				cboRule.DisplayMember = "TEXT_VALUE";
				cboRule.ValueMember = "TEXT_CODE";
			}
			catch( Exception ex )
			{
				HandleException(ex, "CreateRuleDataTable");
			}
		}


		/// <summary>
		/// Private method handles loading data on the form
		/// </summary>
		/// <param name="aGlobalUserType"></param>
		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			try
			{		
				SetText();
			
				Name = MIDText.GetTextOnly((int)eMethodType.BasisSizeAllocation);

				CreateColorDataTable();
				CreateRuleDataTable();

				LoadMethods();

                LoadWorkflows();
			
				txtQuantity.Text = string.Empty;
				if (_basisSizeMethod.RuleQuantity != Include.UndefinedQuantity)
					txtQuantity.Text = _basisSizeMethod.RuleQuantity.ToString(CultureInfo.CurrentUICulture);

				if (_basisSizeMethod.BasisHdrRid == Include.NoRID)
				{
					cboBasisComponent.Enabled = false;
					cboBasisColor.Enabled = false;
                    ckbIncludeReserve.Enabled = false; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
				}
				else
				{
					DataRow dr = _componentDataTable.Rows[cboBasisComponent.SelectedIndex];
					eComponentType bct = (eComponentType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));

					if (bct == eComponentType.SpecificColor)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							cboBasisColor.Enabled = true;
						}
					}
					else
					{
						cboBasisColor.SelectedIndex = -1;
                        //this.cboBasisColor_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
						cboBasisColor.Enabled = false;
					}


				}
				// BEGIN MID Track #3942  'None' is now another name for NoSecondarySize 
				_noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
				// END MID Track #3942

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                cbxUseDefaultcurve.Checked = _basisSizeMethod.UseDefaultCurve;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                cbxApplyRulesOnly.Checked = _basisSizeMethod.ApplyRulesOnly;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

//				if (aGlobalUserType == eGlobalUserType.User &&
//					!SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.AllocationMethods).AllowUpdate)
//				{
//					radGlobal.Enabled = false;
//				}
//						
				// BEGIN ANF Generic Size Constraint
				SetMaskedComboBoxesEnabled(); 
				// END ANF Generic Size Constraint
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, aGlobalUserType == eGlobalUserType.User);
                // End TT#44

                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabControl2.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool

                //BEGIN TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015
                if (_basisSizeMethod.IncludeReserve == true)
                {
                    ckbIncludeReserve.Checked = true;
                }
                else
                {
                    ckbIncludeReserve.Checked = false;
                }
                //END TT#1608-MD - srisch - Working 5.4 - Prior Header 05/27/2015 
            }
			catch( Exception ex )
			{
				HandleException(ex, "Common_Load");
			}
		}

		/// <summary>
		/// Process the basis header that was chosen with button click or by drag/drop.
		/// </summary>
		/// <param name="pSHP">SelectedHeaderProfile object</param>
		private void ProcessReceivedHeader(SelectedHeaderProfile pSHP)
		{
			bool processHeader = true;

			try
			{
				//ErrorMessages.Clear();
				AttachErrors(txtBasisHeader);

				_basisHeaderProfile = GetAllocationProfile(pSHP.Key);
				
				if (_basisHeaderProfile.BulkColors.Count == 0)
				{
					processHeader = false;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderBulkColors));
				}

				//				if (!_basisHeaderProfile.BottomUpSizePerformed && !_basisHeaderProfile.BulkSizeBreakoutPerformed)
				//				{
				//					processHeader = false;
				//					ErrorMessages.Add("Selected header must be at least size allocated status.");
				//				}


				if (!processHeader)
				{
					AttachErrors(txtBasisHeader);
				}
				else
				{
					GetColors(pSHP.Key);
					txtBasisHeader.Text = _basisHeaderProfile.HeaderID; //pSHP.HeaderID;
					cboBasisComponent.SelectedValue = (int) eRuleMethodComponentType.MatchingColor;
                    //this.cboBasisComponent_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
					_activeHeaderProfile = _basisHeaderProfile;
					cboBasisComponent.Enabled = true;
                    ckbIncludeReserve.Enabled = true; //TT#1608-MD - SRisch - Prior Header - Include Reserve 06/02/15
					ChangePending = true;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ProcessReceivedHeader(SelectedHeaderProfile pSHP)");
			}
		}





		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.cmdSizeOverride = new System.Windows.Forms.Button();
            this.grpPrior = new System.Windows.Forms.GroupBox();
            this.ckbIncludeReserve = new System.Windows.Forms.CheckBox();
            this.lblBasisComponent = new System.Windows.Forms.Label();
            this.cboBasisComponent = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtBasisHeader = new System.Windows.Forms.TextBox();
            this.lblBasisHeader = new System.Windows.Forms.Label();
            this.cboBasisColor = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblBasisColor = new System.Windows.Forms.Label();
            this.btnGetHeader = new System.Windows.Forms.Button();
            this.gbRule = new System.Windows.Forms.GroupBox();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.cboRule = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.tabConstraints = new System.Windows.Forms.TabPage();
            this.pnlGridContainer = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabSubstitutes = new System.Windows.Forms.TabPage();
            this.lblEmptyGrid = new System.Windows.Forms.Label();
            this.ugSubstitutes = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).BeginInit();
            this.gbGenericSizeCurve.SuspendLayout();
            this.gbSizeCurve.SuspendLayout();
            this.gbSizeConstraints.SuspendLayout();
            this.gbGenericConstraint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).BeginInit();
            this.gbSizeGroup.SuspendLayout();
            this.gbSizeAlternate.SuspendLayout();
            this.gbxNormalizeSizeCurves.SuspendLayout();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabGeneral.SuspendLayout();
            this.grpPrior.SuspendLayout();
            this.gbRule.SuspendLayout();
            this.tabConstraints.SuspendLayout();
            this.pnlGridContainer.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabSubstitutes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugSubstitutes)).BeginInit();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(120, 16);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(208, 14);
            this.cboStoreAttribute.Size = new System.Drawing.Size(224, 21);
            // 
            // cboFilter
            // 
            this.cboFilter.Location = new System.Drawing.Point(62, 10);
            this.cboFilter.Size = new System.Drawing.Size(272, 21);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(16, 8);
            // 
            // cboConstraints
            // 
            this.cboConstraints.Location = new System.Drawing.Point(48, 25);
            // 
            // cboAlternates
            // 
            this.cboAlternates.Visible = false;
            // 
            // ugRules
            // 
            this.ugRules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugRules.DisplayLayout.Appearance = appearance1;
            this.ugRules.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugRules.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugRules.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugRules.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugRules.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugRules.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugRules.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugRules.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugRules.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugRules.Location = new System.Drawing.Point(6, 7);
            this.ugRules.Size = new System.Drawing.Size(704, 371);
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Location = new System.Drawing.Point(14, 24);
            this.cbExpandAll.Size = new System.Drawing.Size(77, 22);
            this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
            // 
            // gbGenericSizeCurve
            // 
            this.gbGenericSizeCurve.Location = new System.Drawing.Point(15, 63);
            this.gbGenericSizeCurve.Size = new System.Drawing.Size(269, 100);
            // 
            // gbSizeCurve
            // 
            this.gbSizeCurve.Location = new System.Drawing.Point(13, 92);
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.Location = new System.Drawing.Point(340, 92);
            this.gbSizeConstraints.Size = new System.Drawing.Size(289, 171);
            // 
            // gbGenericConstraint
            // 
            this.gbGenericConstraint.Location = new System.Drawing.Point(15, 52);
            // 
            // picBoxConstraint
            // 
            this.picBoxConstraint.Location = new System.Drawing.Point(19, 25);
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Location = new System.Drawing.Point(13, 34);
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Location = new System.Drawing.Point(375, 34);
            this.gbSizeAlternate.Visible = false;
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Enabled = false;
            this.gbxNormalizeSizeCurves.Location = new System.Drawing.Point(528, 139);
            this.gbxNormalizeSizeCurves.Visible = false;
            // 
            // cbxUseDefaultcurve
            // 
            this.cbxUseDefaultcurve.CheckedChanged += new System.EventHandler(this.cbxUseDefaultCurve_CheckChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(695, 550);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(607, 550);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(12, 550);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gbSizeConstraints);
            this.tabGeneral.Controls.Add(this.gbSizeCurve);
            this.tabGeneral.Controls.Add(this.gbSizeAlternate);
            this.tabGeneral.Controls.Add(this.gbSizeGroup);
            this.tabGeneral.Controls.Add(this.lblFilter);
            this.tabGeneral.Controls.Add(this.cboFilter);
            this.tabGeneral.Controls.Add(this.cmdSizeOverride);
            this.tabGeneral.Controls.Add(this.grpPrior);
            this.tabGeneral.Controls.Add(this.gbRule);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(736, 427);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.Controls.SetChildIndex(this.gbRule, 0);
            this.tabGeneral.Controls.SetChildIndex(this.grpPrior, 0);
            this.tabGeneral.Controls.SetChildIndex(this.cmdSizeOverride, 0);
            this.tabGeneral.Controls.SetChildIndex(this.cboFilter, 0);
            this.tabGeneral.Controls.SetChildIndex(this.lblFilter, 0);
            this.tabGeneral.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.tabGeneral.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.tabGeneral.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.tabGeneral.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            // 
            // cmdSizeOverride
            // 
            this.cmdSizeOverride.Location = new System.Drawing.Point(623, 5);
            this.cmdSizeOverride.Name = "cmdSizeOverride";
            this.cmdSizeOverride.Size = new System.Drawing.Size(88, 23);
            this.cmdSizeOverride.TabIndex = 34;
            this.cmdSizeOverride.Text = "Size Override";
            this.cmdSizeOverride.Visible = false;
            this.cmdSizeOverride.Click += new System.EventHandler(this.cmdSizeOverride_Click);
            // 
            // grpPrior
            // 
            this.grpPrior.Controls.Add(this.ckbIncludeReserve);
            this.grpPrior.Controls.Add(this.lblBasisComponent);
            this.grpPrior.Controls.Add(this.cboBasisComponent);
            this.grpPrior.Controls.Add(this.txtBasisHeader);
            this.grpPrior.Controls.Add(this.lblBasisHeader);
            this.grpPrior.Controls.Add(this.cboBasisColor);
            this.grpPrior.Controls.Add(this.lblBasisColor);
            this.grpPrior.Controls.Add(this.btnGetHeader);
            this.grpPrior.Location = new System.Drawing.Point(13, 274);
            this.grpPrior.Name = "grpPrior";
            this.grpPrior.Size = new System.Drawing.Size(623, 81);
            this.grpPrior.TabIndex = 31;
            this.grpPrior.TabStop = false;
            this.grpPrior.Text = "Prior";
            this.grpPrior.DragDrop += new System.Windows.Forms.DragEventHandler(this.grpPrior_DragDrop);
            this.grpPrior.DragEnter += new System.Windows.Forms.DragEventHandler(this.grpPrior_DragEnter);
            // 
            // ckbIncludeReserve
            // 
            this.ckbIncludeReserve.AutoSize = true;
            this.ckbIncludeReserve.Location = new System.Drawing.Point(185, 51);
            this.ckbIncludeReserve.Name = "ckbIncludeReserve";
            this.ckbIncludeReserve.Size = new System.Drawing.Size(107, 17);
            this.ckbIncludeReserve.TabIndex = 34;
            this.ckbIncludeReserve.Text = "Include Reserve:";
            this.ckbIncludeReserve.UseVisualStyleBackColor = true;
            this.ckbIncludeReserve.CheckedChanged += new System.EventHandler(this.ckbIncludeReserve_CheckedChanged);
            // 
            // lblBasisComponent
            // 
            this.lblBasisComponent.AutoSize = true;
            this.lblBasisComponent.Location = new System.Drawing.Point(306, 24);
            this.lblBasisComponent.Name = "lblBasisComponent";
            this.lblBasisComponent.Size = new System.Drawing.Size(61, 13);
            this.lblBasisComponent.TabIndex = 32;
            this.lblBasisComponent.Text = "Component";
            // 
            // cboBasisComponent
            // 
            this.cboBasisComponent.AutoAdjust = true;
            this.cboBasisComponent.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBasisComponent.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBasisComponent.DataSource = null;
            this.cboBasisComponent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBasisComponent.DropDownWidth = 168;
            this.cboBasisComponent.FormattingEnabled = false;
            this.cboBasisComponent.IgnoreFocusLost = false;
            this.cboBasisComponent.ItemHeight = 13;
            this.cboBasisComponent.Location = new System.Drawing.Point(381, 20);
            this.cboBasisComponent.Margin = new System.Windows.Forms.Padding(0);
            this.cboBasisComponent.MaxDropDownItems = 25;
            this.cboBasisComponent.Name = "cboBasisComponent";
            this.cboBasisComponent.SetToolTip = "";
            this.cboBasisComponent.Size = new System.Drawing.Size(168, 21);
            this.cboBasisComponent.TabIndex = 31;
            this.cboBasisComponent.Tag = null;
            this.cboBasisComponent.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboBasisComponent_MIDComboBoxPropertiesChangedEvent);
            this.cboBasisComponent.SelectionChangeCommitted += new System.EventHandler(this.cboBasisComponent_SelectionChangeCommitted);
            // 
            // txtBasisHeader
            // 
            this.txtBasisHeader.AllowDrop = true;
            this.txtBasisHeader.Location = new System.Drawing.Point(72, 20);
            this.txtBasisHeader.Name = "txtBasisHeader";
            this.txtBasisHeader.Size = new System.Drawing.Size(192, 20);
            this.txtBasisHeader.TabIndex = 27;
            this.txtBasisHeader.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragDrop);
            this.txtBasisHeader.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragEnter);
            // 
            // lblBasisHeader
            // 
            this.lblBasisHeader.AutoSize = true;
            this.lblBasisHeader.Location = new System.Drawing.Point(8, 24);
            this.lblBasisHeader.Name = "lblBasisHeader";
            this.lblBasisHeader.Size = new System.Drawing.Size(56, 13);
            this.lblBasisHeader.TabIndex = 28;
            this.lblBasisHeader.Text = "Header ID";
            // 
            // cboBasisColor
            // 
            this.cboBasisColor.AutoAdjust = true;
            this.cboBasisColor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboBasisColor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboBasisColor.DataSource = null;
            this.cboBasisColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBasisColor.DropDownWidth = 168;
            this.cboBasisColor.FormattingEnabled = false;
            this.cboBasisColor.IgnoreFocusLost = false;
            this.cboBasisColor.ItemHeight = 13;
            this.cboBasisColor.Location = new System.Drawing.Point(381, 48);
            this.cboBasisColor.Margin = new System.Windows.Forms.Padding(0);
            this.cboBasisColor.MaxDropDownItems = 25;
            this.cboBasisColor.Name = "cboBasisColor";
            this.cboBasisColor.SetToolTip = "";
            this.cboBasisColor.Size = new System.Drawing.Size(168, 21);
            this.cboBasisColor.TabIndex = 33;
            this.cboBasisColor.Tag = null;
            this.cboBasisColor.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboBasisColor_MIDComboBoxPropertiesChangedEvent);
            this.cboBasisColor.SelectionChangeCommitted += new System.EventHandler(this.cboBasisColor_SelectionChangeCommitted);
            // 
            // lblBasisColor
            // 
            this.lblBasisColor.AutoSize = true;
            this.lblBasisColor.Location = new System.Drawing.Point(306, 52);
            this.lblBasisColor.Name = "lblBasisColor";
            this.lblBasisColor.Size = new System.Drawing.Size(31, 13);
            this.lblBasisColor.TabIndex = 30;
            this.lblBasisColor.Text = "Color";
            // 
            // btnGetHeader
            // 
            this.btnGetHeader.Location = new System.Drawing.Point(74, 48);
            this.btnGetHeader.Name = "btnGetHeader";
            this.btnGetHeader.Size = new System.Drawing.Size(104, 23);
            this.btnGetHeader.TabIndex = 10;
            this.btnGetHeader.Text = "Get Basis Header";
            this.btnGetHeader.Click += new System.EventHandler(this.btnGetHeader_Click);
            // 
            // gbRule
            // 
            this.gbRule.Controls.Add(this.lblQuantity);
            this.gbRule.Controls.Add(this.txtQuantity);
            this.gbRule.Controls.Add(this.cboRule);
            this.gbRule.Location = new System.Drawing.Point(13, 362);
            this.gbRule.Name = "gbRule";
            this.gbRule.Size = new System.Drawing.Size(626, 57);
            this.gbRule.TabIndex = 37;
            this.gbRule.TabStop = false;
            this.gbRule.Text = "Rule";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(306, 26);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(48, 16);
            this.lblQuantity.TabIndex = 35;
            this.lblQuantity.Text = "Quantity";
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(376, 22);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(80, 20);
            this.txtQuantity.TabIndex = 36;
            this.txtQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cboRule
            // 
            this.cboRule.AutoAdjust = true;
            this.cboRule.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboRule.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboRule.DataSource = null;
            this.cboRule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRule.DropDownWidth = 216;
            this.cboRule.FormattingEnabled = false;
            this.cboRule.IgnoreFocusLost = false;
            this.cboRule.ItemHeight = 13;
            this.cboRule.Location = new System.Drawing.Point(48, 22);
            this.cboRule.Margin = new System.Windows.Forms.Padding(0);
            this.cboRule.MaxDropDownItems = 25;
            this.cboRule.Name = "cboRule";
            this.cboRule.SetToolTip = "";
            this.cboRule.Size = new System.Drawing.Size(216, 21);
            this.cboRule.TabIndex = 32;
            this.cboRule.Tag = null;
            this.cboRule.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboRule_MIDComboBoxPropertiesChangedEvent);
            this.cboRule.SelectionChangeCommitted += new System.EventHandler(this.cboRule_SelectionChangeCommitted);
            // 
            // tabConstraints
            // 
            this.tabConstraints.Controls.Add(this.cbExpandAll);
            this.tabConstraints.Controls.Add(this.lblStoreAttribute);
            this.tabConstraints.Controls.Add(this.cboStoreAttribute);
            this.tabConstraints.Controls.Add(this.pnlGridContainer);
            this.tabConstraints.Location = new System.Drawing.Point(4, 22);
            this.tabConstraints.Name = "tabConstraints";
            this.tabConstraints.Size = new System.Drawing.Size(736, 427);
            this.tabConstraints.TabIndex = 1;
            this.tabConstraints.Text = "Rules";
            this.tabConstraints.Controls.SetChildIndex(this.pnlGridContainer, 0);
            this.tabConstraints.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.tabConstraints.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.tabConstraints.Controls.SetChildIndex(this.cbExpandAll, 0);
            // 
            // pnlGridContainer
            // 
            this.pnlGridContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGridContainer.Controls.Add(this.ugRules);
            this.pnlGridContainer.Location = new System.Drawing.Point(8, 40);
            this.pnlGridContainer.Name = "pnlGridContainer";
            this.pnlGridContainer.Size = new System.Drawing.Size(720, 381);
            this.pnlGridContainer.TabIndex = 25;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabMethod);
            this.tabControl2.Controls.Add(this.tabProperties);
            this.tabControl2.Location = new System.Drawing.Point(8, 48);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(763, 492);
            this.tabControl2.TabIndex = 2;
            this.tabControl2.SelectedIndexChanged += new System.EventHandler(this.tabControl2_SelectedIndexChanged);
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.tabControl3);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(755, 466);
            this.tabMethod.TabIndex = 0;
            this.tabMethod.Text = "Method";
            // 
            // tabControl3
            // 
            this.tabControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl3.Controls.Add(this.tabGeneral);
            this.tabControl3.Controls.Add(this.tabConstraints);
            this.tabControl3.Controls.Add(this.tabSubstitutes);
            this.tabControl3.Location = new System.Drawing.Point(6, 1);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(744, 453);
            this.tabControl3.TabIndex = 0;
            this.tabControl3.SelectedIndexChanged += new System.EventHandler(this.tabControl3_SelectedIndexChanged);
            // 
            // tabSubstitutes
            // 
            this.tabSubstitutes.Controls.Add(this.lblEmptyGrid);
            this.tabSubstitutes.Controls.Add(this.ugSubstitutes);
            this.tabSubstitutes.Location = new System.Drawing.Point(4, 22);
            this.tabSubstitutes.Name = "tabSubstitutes";
            this.tabSubstitutes.Size = new System.Drawing.Size(736, 427);
            this.tabSubstitutes.TabIndex = 2;
            this.tabSubstitutes.Text = "Basis Substitutes";
            // 
            // lblEmptyGrid
            // 
            this.lblEmptyGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmptyGrid.Location = new System.Drawing.Point(198, 24);
            this.lblEmptyGrid.Name = "lblEmptyGrid";
            this.lblEmptyGrid.Size = new System.Drawing.Size(300, 12);
            this.lblEmptyGrid.TabIndex = 1;
            this.lblEmptyGrid.Text = "* No size curve or size group has been selected. *";
            // 
            // ugSubstitutes
            // 
            this.ugSubstitutes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSubstitutes.DisplayLayout.Appearance = appearance7;
            this.ugSubstitutes.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugSubstitutes.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSubstitutes.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSubstitutes.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSubstitutes.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugSubstitutes.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSubstitutes.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugSubstitutes.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugSubstitutes.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSubstitutes.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSubstitutes.Location = new System.Drawing.Point(12, 8);
            this.ugSubstitutes.Name = "ugSubstitutes";
            this.ugSubstitutes.Size = new System.Drawing.Size(714, 407);
            this.ugSubstitutes.TabIndex = 0;
            this.ugSubstitutes.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSubstitutes_AfterCellUpdate);
            this.ugSubstitutes.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSubstitutes_InitializeLayout);
            this.ugSubstitutes.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSubstitutes_BeforeExitEditMode);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(755, 466);
            this.tabProperties.TabIndex = 1;
            this.tabProperties.Text = "Properties";
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance13.BackColor = System.Drawing.Color.White;
            appearance13.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance13.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance13;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance14.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance14;
            appearance15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance15.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance15.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance15.ForeColor = System.Drawing.Color.Black;
            appearance15.TextHAlignAsString = "Left";
            appearance15.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance15;
            appearance16.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance16;
            appearance17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance17.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance17;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance18.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance18.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance18;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(16, 16);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(721, 434);
            this.ugWorkflows.TabIndex = 0;
            // 
            // frmBasisSizeMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(779, 585);
            this.Controls.Add(this.tabControl2);
            this.Name = "frmBasisSizeMethod";
            this.Text = "Basis Size Method";
            this.Load += new System.EventHandler(this.frmBasisSizeMethod_Load);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.tabControl2, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).EndInit();
            this.gbGenericSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.PerformLayout();
            this.gbSizeConstraints.ResumeLayout(false);
            this.gbSizeConstraints.PerformLayout();
            this.gbGenericConstraint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).EndInit();
            this.gbSizeGroup.ResumeLayout(false);
            this.gbSizeAlternate.ResumeLayout(false);
            this.gbxNormalizeSizeCurves.ResumeLayout(false);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.grpPrior.ResumeLayout(false);
            this.grpPrior.PerformLayout();
            this.gbRule.ResumeLayout(false);
            this.gbRule.PerformLayout();
            this.tabConstraints.ResumeLayout(false);
            this.tabConstraints.PerformLayout();
            this.pnlGridContainer.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabSubstitutes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugSubstitutes)).EndInit();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region MIDFormBase Overrides

		/// <summary>
		/// Determines if the contraint data should be restored for this method.
		/// </summary>
		override protected void AfterClosing()
		{
			try
			{
				if (ResultSaveChanges != DialogResult.None)
				{
					if (ResultSaveChanges == DialogResult.No)
					{
						//ONLY ROLLBACK IF UPDATING THE METHOD
						if (_basisSizeMethod.Method_Change_Type == eChangeType.update)
						{
							if (ConstraintRollback)
							{
								_basisSizeMethod.MethodConstraints = DataSetBackup;
								_basisSizeMethod.InsertUpdateMethodRules(new TransactionData());
							}
						}
					}
				}

				if (_basisSizeMethod != null)
				{
					_basisSizeMethod = null;
				}
//
//				this.ugRules.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseDown);
//				this.ugRules.AfterRowActivate -= new System.EventHandler(this.ugRules_AfterRowActivate);
//				this.ugRules.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowInsert);
//				this.ugRules.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellUpdate);
//				this.ugRules.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeEnterEditMode);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                //this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                //this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                //this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                //this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.cmdSizeOverride.Click -= new System.EventHandler(this.cmdSizeOverride_Click);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboRule.SelectionChangeCommitted -= new System.EventHandler(this.cboRule_SelectionChangeCommitted);
                this.cboRule.SelectionChangeCommitted -= new System.EventHandler(this.cboRule_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboRule.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboRule_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.grpPrior.DragEnter -= new System.Windows.Forms.DragEventHandler(this.grpPrior_DragEnter);
				this.grpPrior.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grpPrior_DragDrop);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboBasisComponent.SelectionChangeCommitted -= new System.EventHandler(this.cboBasisComponent_SelectionChangeCommitted);
                this.cboBasisComponent.SelectionChangeCommitted -= new System.EventHandler(this.cboBasisComponent_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboBasisComponent.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboBasisComponent_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.txtBasisHeader.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragDrop);
				this.txtBasisHeader.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtBasisHeader_DragEnter);
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.cboBasisColor.SelectionChangeCommitted -= new System.EventHandler(this.cboBasisColor_SelectionChangeCommitted);
                this.cboBasisColor.SelectionChangeCommitted -= new System.EventHandler(this.cboBasisColor_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboBasisColor.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboBasisColor_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                // End TT#301-MD - JSmith - Controls are not functioning properly
				this.btnGetHeader.Click -= new System.EventHandler(this.btnGetHeader_Click);
				// Begin MID Track 4858 - JSmith - Security changes
//				this.txtName.TextChanged -= new System.EventHandler(this.txtName_TextChanged);
//				this.txtDesc.TextChanged -= new System.EventHandler(this.txtDesc_TextChanged);
//				this.radGlobal.CheckedChanged -= new System.EventHandler(this.radGlobal_CheckedChanged);
//				this.radUser.CheckedChanged -= new System.EventHandler(this.radUser_CheckedChanged);
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				// End MID Track 4858
                this.ugSubstitutes.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSubstitutes_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugSubstitutes);
                //End TT#169
                this.ugSubstitutes.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugSubstitutes_BeforeExitEditMode);
                this.ugSubstitutes.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSubstitutes_AfterCellUpdate);
//
//				this.ugRules.DataSource = null;
				this.ugWorkflows.DataSource = null;
				this.cboBasisColor.DataSource = null;
				this.cboBasisComponent.DataSource = null;
				this.cboFilter.DataSource = null;
				this.cboRule.DataSource = null;
				this.cboSizeCurve.DataSource = null;
				this.cboSizeGroup.DataSource = null;
				this.cboStoreAttribute.DataSource = null;
                this.ugSubstitutes.DataSource = null;




			}
			catch (Exception ex)
			{
				HandleException(ex, "frmBasisSizeMethod.AfterClosing");
			}
		}
		#endregion MIDFormBase Overrides

		#region WorkflowMethodFormBase Overrides

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
				_nodeRID = aNodeRID;
				_basisSizeMethod = new BasisSizeAllocationMethod(SAB,aMethodRID);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.AllocationMethodsUserBasisSize, eSecurityFunctions.AllocationMethodsGlobalBasisSize);

				Common_Load(aNode.GlobalUserType);
			}
			catch(Exception ex)
			{
				HandleException(ex, "UpdateWorkflowMethod");
				FormLoadError = true;
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
				_basisSizeMethod = new BasisSizeAllocationMethod(SAB,method_RID);
				return Delete();
			}
			// Begin MID Track 2401 - delete error when data in use
			catch(DatabaseForeignKeyViolation)
			{
				throw;
			}
			// End MID Track 2401
			catch (Exception ex)
			{
				HandleException(ex, "DeleteWorkflowMethod");
			}

			return true;
		}


		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.SizeMethods;	
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
			catch (Exception)
			{
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
//				_basisSizeMethod.EquateSizeInd = chkSizeEquates.Checked;

				if (cboFilter.Text.Trim() != string.Empty)
				{
					_basisSizeMethod.StoreFilterRid = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				else
				{
					_basisSizeMethod.StoreFilterRid = Include.NoRID;
				}

				if (cboSizeGroup.Text.Trim() != string.Empty)
				{
					_basisSizeMethod.SizeGroupRid = Convert.ToInt32(cboSizeGroup.SelectedValue,CultureInfo.CurrentUICulture);
				}
				else
				{
					_basisSizeMethod.SizeGroupRid = Include.NoRID;
				}

				if (cboConstraints.Text.Trim() != string.Empty)
				{
					_basisSizeMethod.SizeConstraintRid = Convert.ToInt32(cboConstraints.SelectedValue,CultureInfo.CurrentUICulture);
				}
				else
				{
					_basisSizeMethod.SizeConstraintRid = Include.NoRID;
				}

				_basisSizeMethod.SG_RID = Convert.ToInt32(cboStoreAttribute.SelectedValue,CultureInfo.CurrentUICulture);


				if (_activeHeaderProfile != null)
				{
					_basisSizeMethod.BasisHdrRid = _activeHeaderProfile.HeaderRID;
					_basisSizeMethod.BasisClrRid = Convert.ToInt32(cboBasisColor.SelectedValue,CultureInfo.CurrentUICulture);
					_basisSizeMethod.HeaderComponent = Convert.ToInt32(cboBasisComponent.SelectedValue,CultureInfo.CurrentUICulture);
				}
				else
				{
					//This should never happen if ValidateSpecificFields is working properly.
					_basisSizeMethod.BasisHdrRid = Include.NoRID;
					_basisSizeMethod.BasisClrRid = Include.NoRID;
					_basisSizeMethod.HeaderComponent = Include.Undefined;
				}

				_basisSizeMethod.Rule = Convert.ToInt32(cboRule.SelectedValue,CultureInfo.CurrentUICulture);

				if (txtQuantity.Text.Trim() != string.Empty)
				{
					_basisSizeMethod.RuleQuantity = Convert.ToInt32(this.txtQuantity.Text,CultureInfo.CurrentUICulture);
				}
				else
				{
					_basisSizeMethod.RuleQuantity = Include.UndefinedQuantity;
				}

				//SetSubstitutes();

				// begin Generic Size Curve data
				_basisSizeMethod.GenCurveCharGroupRID =  Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_basisSizeMethod.GenCurveColorInd = cbColor.Checked; 
				 
				DataRowView drv = (DataRowView)cboHierarchyLevel.SelectedItem;
				DataRow dRow  = drv.Row;

				_basisSizeMethod.GenCurveMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_basisSizeMethod.GenCurveMerchType)
				{
					case eMerchandiseType.Node:
						_basisSizeMethod.GenCurveHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_basisSizeMethod.GenCurvePhRID = Include.NoRID;
						_basisSizeMethod.GenCurvePhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_basisSizeMethod.GenCurvePhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_basisSizeMethod.GenCurvePhRID = HP.Key;
						_basisSizeMethod.GenCurveHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_basisSizeMethod.GenCurveHnRID = Include.NoRID;
						_basisSizeMethod.GenCurvePhRID = Include.NoRID;
						_basisSizeMethod.GenCurvePhlSequence  = 0;
						break;
				}
 				// end  Generic Size Curve data

				// begin Generic Size Constraint data
				_basisSizeMethod.GenConstraintCharGroupRID =  Convert.ToInt32(cboConstrHeaderChar.SelectedValue, CultureInfo.CurrentUICulture);
				_basisSizeMethod.GenConstraintColorInd = cbConstrColor.Checked; 
				
				drv = (DataRowView)cboConstrHierLevel.SelectedItem;
				dRow  = drv.Row;
				 
				_basisSizeMethod.GenConstraintMerchType = (eMerchandiseType)(Convert.ToInt32(dRow["leveltypename"], CultureInfo.CurrentUICulture)); 
				switch(_basisSizeMethod.GenConstraintMerchType)
				{
					case eMerchandiseType.Node:
						_basisSizeMethod.GenConstraintHnRID = Convert.ToInt32(dRow["key"], CultureInfo.CurrentUICulture);
						_basisSizeMethod.GenConstraintPhRID = Include.NoRID;
						_basisSizeMethod.GenConstraintPhlSequence = 0;
						break;
					case eMerchandiseType.HierarchyLevel:
						_basisSizeMethod.GenConstraintPhlSequence  = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);
						_basisSizeMethod.GenConstraintPhRID = HP.Key;
						_basisSizeMethod.GenConstraintHnRID = Include.NoRID;
						break;
					case eMerchandiseType.OTSPlanLevel:
					case eMerchandiseType.Undefined:
						_basisSizeMethod.GenConstraintHnRID = Include.NoRID;
						_basisSizeMethod.GenConstraintPhRID = Include.NoRID;
						_basisSizeMethod.GenConstraintPhlSequence  = 0;
						break;
				}
				// end  Generic Size Constraint data

                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                _basisSizeMethod.UseDefaultCurve = cbxUseDefaultcurve.Checked;
                // End TT#413

                // begin TT#2155 - JEllis - Fill Size Holes Null Reference
                _basisSizeMethod.ApplyRulesOnly = cbxApplyRulesOnly.Checked;
                // end TT#2155 - JEllis - Fill Size Holes Null Reference

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                if (SAB.ApplicationServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
                {
                    _basisSizeMethod.GenCurveNsccdRID = Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture);
                    _basisSizeMethod.GenCurveCharGroupRID = Include.NoRID;
                }
                else
                {
                    _basisSizeMethod.GenCurveNsccdRID = Include.NoRID;
                }
                // End TT#413
			}
			catch (Exception)
			{
				throw;
			}

		}

//		private void SetSubstitutes()
//		{
//			DataSet dsSizeSub = MIDEnvironment.CreateDataSet("SizeSub");
//			dsSizeSub.Tables.Add((DataTable) ugSubstitutes.DataSource);
//			DataTable dtSizeSub = dsSizeSub.Tables[0];
//
//			for (int r=0;r<dtSizeSub.Rows.Count;r++)
//			{
//				DataRow aRow = dtSizeSub.Rows[r];
//				for (int c=0;c<aRow.Table.Columns.Count;c++)
//				{
//				}
//
//			}
//		}

		/// <summary>
		/// Use to validate the fields that are specific to this method type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{				
			bool isFormValid = true;

			try
			{
				//initialize all fields to not having an error
				//ErrorMessages.Clear();
				AttachErrors(txtBasisHeader);

				if (_activeHeaderProfile == null)
				{
					isFormValid = false;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MissingBasisHeader));
					AttachErrors(txtBasisHeader);
					//ErrorMessages.Clear();
				}

				//==================
				// Rule
				//==================
				AttachErrors(cboRule);
				eBasisSizeMethodRuleType bsm = (eBasisSizeMethodRuleType)(Convert.ToInt32(cboRule.SelectedValue, CultureInfo.CurrentUICulture));
				if (!Enum.IsDefined(typeof(eBasisSizeMethodRuleType),bsm))
				{
					isFormValid = false;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_RuleRequiredForBasisSize));
					AttachErrors(cboRule);
				}

				//==================
				// Rule Quantity
				//==================
				AttachErrors(txtQuantity);
				eBasisSizeRuleRequiresQuantity rqm = (eBasisSizeRuleRequiresQuantity)(Convert.ToInt32(cboRule.SelectedValue, CultureInfo.CurrentUICulture));
				if (Enum.IsDefined(typeof(eBasisSizeRuleRequiresQuantity),rqm))
				{
					try
					{
						int quantity = Convert.ToInt32(txtQuantity.Text, CultureInfo.CurrentUICulture);
						if (quantity < 1)
						{
							isFormValid = false;
							ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
							AttachErrors(txtQuantity);
						}
					}
					catch
					{
						isFormValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBePositiveInteger));
						AttachErrors(txtQuantity);
					}
				}

				if (!IsGridValid())
				{
					isFormValid = false;
				}
			}
			catch
			{
				throw;
			}

			return isFormValid;
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

				//ErrorMessages.Clear();
				if (!WorkflowMethodDescriptionValid)
				{
					ErrorMessages.Add(WorkflowMethodDescriptionMessage);
					AttachErrors(txtDesc);
				}
				else
				{	
					AttachErrors(txtDesc);
				}

				//ErrorMessages.Clear();
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
			catch (Exception ex)
			{
				string message = ex.ToString();
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
				ABM =_basisSizeMethod;
			}
			catch(Exception ex)
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
		/// Opens a new Basis Size Method. 
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_basisSizeMethod = new BasisSizeAllocationMethod(SAB, Include.NoRID);
				ABM = _basisSizeMethod;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.AllocationMethodsUserFillSizeHoles, eSecurityFunctions.AllocationMethodsGlobalFillSizeHoles);
				// Store Attribute to allocation default
                // Begin TT#5124 - JSmith - Performance
                //GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
                //gop.LoadOptions();
                //_basisSizeMethod.SG_RID = gop.AllocationStoreGroupRID;
                //gop = null;
                _basisSizeMethod.SG_RID = SAB.ClientServerSession.GlobalOptions.AllocationStoreGroupRID;
                // End TT#5124 - JSmith - Performance
				this.lblEmptyGrid.Visible = true;

				txtQuantity.Enabled = false;

				// Begin issue 3779
				Common_Load(aParentNode.GlobalUserType);
				// End issue 3779
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewWorkflowMethod");
				FormLoadError = true;
			}
		}


		/// <summary>
		/// Renames a Basis Size Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_basisSizeMethod = new BasisSizeAllocationMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception ex)
			{
				HandleException(ex, "RenameWorkflowMethod");
				FormLoadError = true;
			}
			return false;
		}

		/// <summary>
		/// Processes a method.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		override public void ProcessWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_basisSizeMethod = new BasisSizeAllocationMethod(SAB, aMethodRID);
				ProcessAction(eMethodType.BasisSizeAllocation, true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}


		#endregion WorkflowMethodFormBase Overrides

		#region IFormBase Members
//		override public void ICut()
//		{
//			
//		}
//
//
//		override public void ICopy()
//		{
//			
//		}
//
//
//		override public void IPaste()
//		{
//			
//		}	

		override public void ISave()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				SaveChanges();
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}


//		override public void ISaveAs()
//		{
//			
//		}
//
//
//		override public void IDelete()
//		{
//			
//		}
//
//
//		override public void IRefresh()
//		{
//			
//		}
			

		#endregion

		override protected void InitializeValueLists(UltraGrid myGrid)
		{
			myGrid.DisplayLayout.ValueLists.Add("Colors");
			myGrid.DisplayLayout.ValueLists.Add("Rules");
			//myGrid.DisplayLayout.ValueLists.Add("SortOrder");
			myGrid.DisplayLayout.ValueLists.Add("Sizes");
			//myGrid.DisplayLayout.ValueLists.Add("DimensionMaster");
			myGrid.DisplayLayout.ValueLists.Add("Dimensions");
			myGrid.DisplayLayout.ValueLists.Add("DimensionCell");
			myGrid.DisplayLayout.ValueLists.Add("SizeCell");
			//Begin MID Track # 2936 - stodd
			myGrid.DisplayLayout.ValueLists.Add("ColorCell");
			//End MID track # 2936
			//Begin MID Track # 3685 - stodd
			//myGrid.DisplayLayout.ValueLists["SizeCell"].SortStyle = ValueListSortStyle.Ascending;
			//End MID Track # 3685 - stodd

			FillColorList(myGrid.DisplayLayout.ValueLists["Colors"]);


			base.FillRulesList(myGrid.DisplayLayout.ValueLists["Rules"], false);
			//
			//			FillSortList(myGrid.DisplayLayout.ValueLists["SortOrder"],
			//				eMIDTextType.eFillSizeHolesSort,
			//				eMIDTextOrderBy.TextValue);

			//Fill the size lists if there is a size selected.
			if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_basisSizeMethod.SizeGroupRid, _basisSizeMethod.GetSizesUsing);

				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_basisSizeMethod.SizeGroupRid, _basisSizeMethod.GetDimensionsUsing);
			}
			// Begin MID Track 3158 - add valuelists with size curve is used
			else if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
			{
				FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
					_basisSizeMethod.SizeCurveGroupRid, _basisSizeMethod.GetSizesUsing);

				FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
					_basisSizeMethod.SizeCurveGroupRid, _basisSizeMethod.GetDimensionsUsing);
			}
			// End MID Track 3158
		}

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        //private void ugRules_AfterRowActivate(object sender, System.EventArgs e)
        override protected void ugRules_AfterRowActivate(object sender, System.EventArgs e)
        // End TT#301-MD - JSmith - Controls are not functioning properly
		{
			try
			{
				UltraGridRow myRow = ((UltraGrid)sender).ActiveRow;

				switch (myRow.Band.Key.ToUpper())
				{
					case C_SET:
						if ((eSizeMethodRowType)myRow.Cells["ROW_TYPE_ID"].Value == eSizeMethodRowType.AllSize)
						{
							ToggleAddNewBoxButtons(false);
						}
						else
						{
							ToggleAddNewBoxButtons(true);
						}
						break;
					default:
						ToggleAddNewBoxButtons(true);
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugRules_AfterRowActivate");
			}
		}

		/// <summary>
		/// Private method, enables or disables the buttons on the AddNewBox on ugAllSize.
		/// Called from ugAllSize_AfterRowActivate
		/// </summary>
		/// <param name="Defaults"></param>
		private void ToggleAddNewBoxButtons(bool Defaults)
		{
			try
			{
				//THESE TWO BANDS SHOULD NEVER BE ALLOWED TO ADD NEW OR DELETE
				//============================================================
				ugRules.DisplayLayout.Bands[C_SET].Override.AllowAddNew = AllowAddNew.No;
				ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowAddNew = AllowAddNew.No;
				ugRules.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
				ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
				//============================================================

				if (FunctionSecurity.AllowUpdate)
				{

					if (Defaults)
					{
						//SETUP FOR WHEN THE ACTIVE ROW IS NOT THE ALLSIZE ROW FROM THE SET BAND
						ugRules.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.Yes;
					
						if (this._basisSizeMethod.SizeGroupRid != Include.NoRID)
						{
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.Yes;
						}
					}
					else
					{
						//SETUP FOR WHEN THE ACTIVE ROW IS THE ALLSIZE ROW FROM THE SET BAND
						ugRules.DisplayLayout.Bands[C_SET_CLR].Override.AllowAddNew = AllowAddNew.No;
					
						if (this._basisSizeMethod.SizeGroupRid != Include.NoRID)
						{
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_CLR_SZ].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
							ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Override.AllowAddNew = AllowAddNew.No;
						}
					}
				}
				else
				{
					foreach (UltraGridBand ugb in ugRules.DisplayLayout.Bands)
					{
						ugb.Override.AllowAddNew = AllowAddNew.No;
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ToggleAddNewBoxButtons");
			}
		
		}

        #region Properties Tab - Workflows
        /// <summary>
        /// Fill the workflow grid
        /// </summary>
        private void LoadWorkflows()//9-17
        {
            try
            {
                GetWorkflows(_basisSizeMethod.Key, ugWorkflows);
            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadWorkflows");
            }
        }
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
				HandleException(ex, "IsActiveRowValid");
				return false;
			}
		}


		override protected bool IsGridValid()
		{
			try
			{
				bool IsValid = true;
			
				//================================================================================
				//WALK THE GRID - VALIDATING EACH ROW
				//================================================================================
				foreach(UltraGridRow setRow in ugRules.Rows)
				{
					if (!IsActiveRowValid(setRow))
					{
						IsValid = false;
					}	

					if (setRow.HasChild())
					{
						//ALL COLORS ROW
						//===============
						foreach(UltraGridRow allColorRow in setRow.ChildBands[C_SET_ALL_CLR].Rows)
						{
							if (!IsActiveRowValid(allColorRow))
							{
								IsValid = false;
							}

							if (allColorRow.HasChild())
							{

								//ALL COLOR DIMENSION ROWS
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[C_ALL_CLR_SZ_DIM].Rows)
								{

									if (!IsActiveRowValid(allColorDimRow))
									{
										IsValid = false;
									}

									//ALL COLOR DIMENSION/SIZE ROWS
									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[C_ALL_CLR_SZ].Rows)
										{
											if (!IsActiveRowValid(allColorSizeRow))
											{
												IsValid = false;
											}
										}	
									}
								}
							}
						}
						//========================================================================


						//COLOR ROWS 
						//===========
						foreach(UltraGridRow colorRow in setRow.ChildBands[C_SET_CLR].Rows)
						{
							if (!IsActiveRowValid(colorRow))
							{
								IsValid = false;
							}

							if (colorRow.HasChild())
							{
								//COLOR SIZE
								//=============
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[C_CLR_SZ_DIM].Rows)
								{
									if (!IsActiveRowValid(colorDimRow))
									{
										IsValid = false;
									}

									if (colorDimRow.HasChild())
									{
										//COLOR SIZE DIMENSION
										//======================
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[C_CLR_SZ].Rows)
										{
											if (!IsActiveRowValid(colorSizeRow))
											{
												IsValid = false;
											}
										}
									}
								}
							}
						}
					}
				}

				if (!IsValid) 
				{ 
					ugRules.Rows.ExpandAll(true);
					EditByCell = true;
				}

				return IsValid;
			}
			catch (Exception ex)
			{
				HandleException(ex, "IsGridValid");
				return false;
			}
		}

        // Begin TT#498 - RMatelic - Size Need Method Default checked on the Rule Tab the Add only goes down to Color - change to public event
        //private void BindSubstitutesGrid(int sizeRID, eGetSizes getSizes) 
        public void BindSubstitutesGrid(int sizeRID, eGetSizes getSizes)
        // End TT#498
		{
			try
			{
                // Begin TT#319-MD - JSmith - Delay when selecting Size Curve 
                if (tabControl3.SelectedTab != tabSubstitutes ||
                    (_currSizeRID == sizeRID && _currGetSizes == getSizes))
                {
                    return;
                }
                _currSizeRID = sizeRID;
                _currGetSizes = getSizes;
                Cursor = Cursors.WaitCursor;
                // End TT#319-MD - JSmith - Delay when selecting Size Curve

				if (sizeRID != Include.NoRID)
				{
					System.Collections.ArrayList sizeAL = new ArrayList(); 
					System.Collections.ArrayList sizeID = new ArrayList();
					System.Collections.ArrayList widthAL = new ArrayList(); 
					System.Collections.ArrayList sizeKeys = new ArrayList(); 
					System.Collections.ArrayList widthKeys = new ArrayList(); 

					System.Collections.Hashtable bothHash = new Hashtable(); 
					SizeGroupProfile sgp = null;
					SizeCurveGroupProfile scgp = null;
					ProfileList scl = null;

					string productCatStr = string.Empty;
					_hiddenColumns.Clear();
					string tableName = "Substitutes";
			
					// is there a size group?
					if (sizeRID != Include.UndefinedSizeGroupRID) 
					{
						// load existing group
						if (getSizes == eGetSizes.SizeGroupRID)
						{
							sgp = new SizeGroupProfile(sizeRID);
							scl = (ProfileList)sgp.SizeCodeList;
						}
						else if (getSizes == eGetSizes.SizeCurveGroupRID)
						{
							scgp = new SizeCurveGroupProfile(sizeRID);
							scl = scgp.SizeCodeList;
						}

						productCatStr = ((SizeCodeProfile)(scl.ArrayList[0])).SizeCodeProductCategory;
				
						foreach(SizeCodeProfile scp in scl.ArrayList) 
						{
							if (scp.Key == -1) 
							{
								throw new MIDException (eErrorLevel.severe,
									(int)eMIDTextCode.msg_CantRetrieveSizeCode,
									MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
							}
							sizeID.Add(scp.SizeCodeID);
							if (! sizeAL.Contains(scp.SizeCodePrimary))
							{
								sizeAL.Add(scp.SizeCodePrimary);
								sizeKeys.Add(scp.SizeCodePrimaryRID);
							}
							if (! widthAL.Contains(scp.SizeCodeSecondary)) 
							{
								widthAL.Add(scp.SizeCodeSecondary);
								widthKeys.Add(scp.SizeCodeSecondaryRID);
							}
						}
					}
			

					//==============================
					// Build columns
					//===============================
					DataTable dtSizes = MIDEnvironment.CreateDataTable(tableName);
					dtSizes.Columns.Add("   ");
					dtSizes.Columns.Add("Dimension");
					dtSizes.Columns.Add("DimensionHIDDEN");
					dtSizes.Columns["DimensionHIDDEN"].DataType = System.Type.GetType("System.Int32");
					_hiddenColumns.Add("DimensionHIDDEN");
					dtSizes.Columns.Add("DimensionHIDDENTYPE");
					dtSizes.Columns["DimensionHIDDENTYPE"].DataType = System.Type.GetType("System.Int32");
					_hiddenColumns.Add("DimensionHIDDENTYPE");
					foreach(string size in sizeAL) 
					{
						dtSizes.Columns.Add(size);
						dtSizes.Columns[size].DataType = System.Type.GetType("System.String");
						dtSizes.Columns.Add(size+"HIDDEN");
						dtSizes.Columns[size+"HIDDEN"].DataType = System.Type.GetType("System.Int32");
						_hiddenColumns.Add(size+"HIDDEN");
						dtSizes.Columns.Add(size+"HIDDENTYPE");
						dtSizes.Columns[size+"HIDDENTYPE"].DataType = System.Type.GetType("System.Int32");
						_hiddenColumns.Add(size+"HIDDENTYPE");

					}

					//=======================================================
					// Builds grid when there is no seconday size (dimension)
					//=======================================================
					string width = (string)widthAL[0];		// MID Track #3942 - add _noSizeDimensionLbl
					if ((widthAL.Count == 1) && (width == Include.NoSecondarySize || width.Trim() == string.Empty || width.Trim() == _noSizeDimensionLbl))
					{
						ArrayList al = new ArrayList();
						al.Add("Size");
						al.Add("");
						al.Add(-1);
						al.Add(-1);
				
						foreach(string size in sizeAL) 
						{
							if ((getSizes == eGetSizes.SizeGroupRID && sgp == null) 
								|| (getSizes == eGetSizes.SizeCurveGroupRID && scgp == null))
							{
								al.Add("");
								al.Add(-1);
								al.Add(-1);
							}
							else 
							{
								SizeCodeList hscl = this.SAB.HierarchyServerSession.GetSizeCodeList(
									productCatStr, eSearchContent.WholeField,
									size, eSearchContent.WholeField, 
									width, eSearchContent.WholeField);
								if (hscl.Count == 1) 
								{
									al.Add(_basisSizeMethod.GetSubstitute(((SizeCodeProfile)(hscl.ArrayList[0])).Key, eEquateOverrideSizeType.DimensionSize));
									al.Add(((SizeCodeProfile)(hscl.ArrayList[0])).Key);
									al.Add((int)eEquateOverrideSizeType.DimensionSize);
								}
								else 
								{
									al.Add("");
									al.Add(-1);
									al.Add(-1);
								}
							}
						}
						object[] myArray = new object[al.Count]; 
						al.CopyTo(myArray);
						dtSizes.Rows.Add(myArray);
					}
						//==================================
						// Builds Size / Dimension grid
						//==================================
					else
					{

						// Size Row
						ArrayList al = new ArrayList();
						al.Add("Size");
						al.Add("");
						al.Add(-1);
						al.Add(-1);
						for (int s=0;s<sizeAL.Count;s++)
						{
							al.Add(_basisSizeMethod.GetSubstitute((int)sizeKeys[s], eEquateOverrideSizeType.Size));
							al.Add(sizeKeys[s]);
							al.Add((int)eEquateOverrideSizeType.Size);
						}
						object[] myArray = new object[al.Count]; 
						al.CopyTo(myArray);
						dtSizes.Rows.Add(myArray);

						// dimension rows
						for (int w=0;w<widthAL.Count;w++)
						{
							width = (string)widthAL[w];
							int widthKey = (int)widthKeys[w];

							al = new ArrayList();
							al.Add(width);

							al.Add(_basisSizeMethod.GetSubstitute(widthKey, eEquateOverrideSizeType.Dimensions));
							al.Add(widthKey);
							al.Add((int)eEquateOverrideSizeType.Dimensions);
							int totalColumn = al.Count - 1;
							foreach(string size in sizeAL) 
							{
								if ((getSizes == eGetSizes.SizeGroupRID && sgp == null) 
									|| (getSizes == eGetSizes.SizeCurveGroupRID && scgp == null))
								{
									al.Add("");
									al.Add(-1);
									al.Add(-1);
								}
								else 
								{
									SizeCodeList hscl = this.SAB.HierarchyServerSession.GetSizeCodeList(
										productCatStr, eSearchContent.WholeField,
										size, eSearchContent.WholeField, 
										width, eSearchContent.WholeField);
									if (hscl.Count == 1) 
									{
										al.Add(_basisSizeMethod.GetSubstitute(((SizeCodeProfile)(hscl.ArrayList[0])).Key, eEquateOverrideSizeType.DimensionSize));
										al.Add(((SizeCodeProfile)(hscl.ArrayList[0])).Key);
										al.Add((int)eEquateOverrideSizeType.DimensionSize);
									}
									else 
									{
										al.Add("");
										al.Add(-1);
										al.Add(-1);
									}
								}
								//					}
							}
							myArray = new object[al.Count]; 
							al.CopyTo(myArray);
							dtSizes.Rows.Add(myArray);
						}
					}
					ugSubstitutes.DataSource = null;
					ugSubstitutes.DataSource = dtSizes;
				}
				else
				{
					ugSubstitutes.DataSource = null;
				}
			}
			catch
			{
				throw;
			}
            // Begin TT#319-MD - JSmith - Delay when selecting Size Curve 
            finally
            {
                Cursor = Cursors.Default;
            }
            // End TT#319-MD - JSmith - Delay when selecting Size Curve
		}

		private void ugSubstitutes_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169
			ugSubstitutes.DisplayLayout.Reset();
			ugSubstitutes.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False; 

			ugSubstitutes.DisplayLayout.ValueLists.Clear();
			ugSubstitutes.DisplayLayout.ValueLists.Add("Sizes");
			ugSubstitutes.DisplayLayout.ValueLists.Add("Dimensions");
			ugSubstitutes.DisplayLayout.ValueLists.Add("Both");

			// Fills value lists for Size Group
			if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
			{
				FillSizesList(ugSubstitutes.DisplayLayout.ValueLists["Sizes"],
					_basisSizeMethod.SizeGroupRid, _basisSizeMethod.GetSizesUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Sizes"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*

				FillSizeDimensionList(ugSubstitutes.DisplayLayout.ValueLists["Dimensions"],
					_basisSizeMethod.SizeGroupRid, _basisSizeMethod.GetDimensionsUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Dimensions"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*  

				FillBothSizeDimensionList(ugSubstitutes.DisplayLayout.ValueLists["Both"],
					_basisSizeMethod.SizeGroupRid, _basisSizeMethod.GetSizesUsing, _basisSizeMethod.GetDimensionsUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Both"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*  
			}
			else if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
			{
				FillSizesList(ugSubstitutes.DisplayLayout.ValueLists["Sizes"],
					_basisSizeMethod.SizeCurveGroupRid, _basisSizeMethod.GetSizesUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Sizes"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*  

				FillSizeDimensionList(ugSubstitutes.DisplayLayout.ValueLists["Dimensions"],
					_basisSizeMethod.SizeCurveGroupRid, _basisSizeMethod.GetDimensionsUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Dimensions"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*  

				FillBothSizeDimensionList(ugSubstitutes.DisplayLayout.ValueLists["Both"],
					_basisSizeMethod.SizeCurveGroupRid, _basisSizeMethod.GetSizesUsing, _basisSizeMethod.GetDimensionsUsing);
				ugSubstitutes.DisplayLayout.ValueLists["Both"].ValueListItems.Add(-1, " ");	// Issue 4050 - stodd*  
			}

			// *Issue 4050 - these statements used to be in the size methods form base program, but some screens want 'empty' 


			//====================================
			// Assigns Value lists to columns
			//====================================
			int sizeTypeNum = 0;
			eEquateOverrideSizeType sizeType;
			for (int r=0;r<ugSubstitutes.Rows.Count;r++)
			{
				for (int c=1;c<ugSubstitutes.Rows[r].Cells.Count;c++)
				{
					UltraGridColumn aColumn = ugSubstitutes.Rows[r].Cells[c].Column;
					if (!_hiddenColumns.Contains(aColumn.Header.Caption))
					{
						sizeTypeNum = Convert.ToInt32(ugSubstitutes.Rows[r].Cells[aColumn.Key+"HIDDENTYPE"].Value, CultureInfo.CurrentUICulture);
						if (sizeTypeNum != Include.NoRID)
						{
							sizeType = (eEquateOverrideSizeType)sizeTypeNum;
							switch (sizeType)
							{
								case eEquateOverrideSizeType.Dimensions:
									ugSubstitutes.Rows[r].Cells[c].ValueList = ugSubstitutes.DisplayLayout.ValueLists["Dimensions"];
									break;

								case eEquateOverrideSizeType.Size:
									ugSubstitutes.Rows[r].Cells[c].ValueList = ugSubstitutes.DisplayLayout.ValueLists["Sizes"];
									break;

								case eEquateOverrideSizeType.DimensionSize:
									ugSubstitutes.Rows[r].Cells[c].ValueList = ugSubstitutes.DisplayLayout.ValueLists["Both"];
									break;

								default:
									break;
							}
						}
					}
				}
			}

			//=======================
			// Hides HIDDEN columns
			//=======================
			foreach(string aColumn in _hiddenColumns) 
			{
				this.ugSubstitutes.DisplayLayout.Bands[0].Columns[aColumn].Hidden = true;
			}

			// center values
			for (int c=1;c<ugSubstitutes.DisplayLayout.Bands[0].Columns.Count;c++)
			{
				ugSubstitutes.DisplayLayout.Bands[0].Columns[c].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			}

			// disable and set color to first column.
			ugSubstitutes.DisplayLayout.Bands[0].Columns[0].CellActivation = Activation.Disabled;
			ugSubstitutes.DisplayLayout.Bands[0].Columns[0].CellAppearance.BackColor = this.BackColor;
			ugSubstitutes.DisplayLayout.Bands[0].Columns[0].CellAppearance.ForeColorDisabled = this.ForeColor;
			// disable and set color to first row and second column.
			ugSubstitutes.Rows[0].Cells[1].Appearance.BackColor = this.BackColor;
			ugSubstitutes.Rows[0].Cells[1].Appearance.ForeColor = this.ForeColor;
			ugSubstitutes.Rows[0].Cells[1].Activation = Activation.Disabled;

            // Begin TT#1530 - RMatelic - Record locking violations from Store Attribute change
            if (!FunctionSecurity.AllowUpdate)
            {
                foreach (Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in ugSubstitutes.DisplayLayout.Bands[0].Columns)
                {
                    oColumn.CellActivation = Activation.NoEdit;
                }
            }
            // End TT#1530

			// Splitter
			this.ugSubstitutes.DisplayLayout.MaxColScrollRegions = 2;
			int colScrollWidth = this.ugSubstitutes.DisplayLayout.Bands[0].Columns[0].Width;
			this.ugSubstitutes.DisplayLayout.ColScrollRegions[0].Width = colScrollWidth;
			this.ugSubstitutes.DisplayLayout.ColScrollRegions[0].Split(this.ugSubstitutes.DisplayLayout.ColScrollRegions[0].Width);
			this.ugSubstitutes.DisplayLayout.Bands[0].Columns[0].Header.ExclusiveColScrollRegion = this.ugSubstitutes.DisplayLayout.ColScrollRegions[0];
		}

		private void ugSubstitutes_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
//				UltraGrid myGrid = (UltraGrid)sender;
//				UltraGridRow myRow = myGrid.ActiveRow;
//			
//				
//				
//						if (myGrid.ActiveCell.Column.)
//						{
//							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
//							myGrid.ActiveCell.ValueList = null;
//						}
//					
//					
//				}

			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugSubstitutes_BeforeExitEditMode");
			}
		}

		private void ugSubstitutes_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				int sizeCodeKey = Include.NoRID;
				int sizeSubKey = Include.NoRID;
				eEquateOverrideSizeType sizeType = eEquateOverrideSizeType.DimensionSize;

				if (e.Cell.Value == System.DBNull.Value)
				{
					sizeSubKey = Include.NoRID;
				}
				else
				{
					sizeSubKey = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
				}
				
				sizeCodeKey = Convert.ToInt32(e.Cell.Row.Cells[e.Cell.Column.Key+"HIDDEN"].Value, CultureInfo.CurrentUICulture);
				sizeType = (eEquateOverrideSizeType)Convert.ToInt32(e.Cell.Row.Cells[e.Cell.Column.Key+"HIDDENTYPE"].Value, CultureInfo.CurrentUICulture);
				_basisSizeMethod.UpdateSubstituteList(sizeCodeKey, sizeSubKey, sizeType);
				ChangePending = true;
			}
			catch
			{
				throw;
			}
		}

		private void cbExpandAll_CheckedChanged(object sender, System.EventArgs e)
		{
			CheckExpandAll();
		}
	
		// begin A&F Add Generic Size Curve data 
		private void tabControl2_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			 
			if (tabControl2.SelectedTab == tabMethod && tabControl3.SelectedTab == tabGeneral)
			{
				gbGenericSizeCurve.Visible =   true;
			}
			else
			{
				gbGenericSizeCurve.Visible =   false;
			}
		}
		
		private void tabControl3_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			gbGenericSizeCurve.Visible =  (tabControl3.SelectedTab == tabGeneral) ? true : false;

            // Begin TT#319-MD - JSmith - Delay when selecting Size Curve 
            if (tabControl3.SelectedTab == tabSubstitutes)
            {
                if (_basisSizeMethod.SizeGroupRid != Include.NoRID)
                {
                    BindSubstitutesGrid(_basisSizeMethod.SizeGroupRid, eGetSizes.SizeGroupRID);
                }
                else if (_basisSizeMethod.SizeCurveGroupRid != Include.NoRID)
                {
                    BindSubstitutesGrid(_basisSizeMethod.SizeCurveGroupRid, eGetSizes.SizeCurveGroupRID);
                }
            }
            // End TT#319-MD - JSmith - Delay when selecting Size Curve
            
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void frmBasisSizeMethod_Load(object sender, EventArgs e)
        {
            if (cboStoreAttribute.ReplaceAttribute)
            {
                ChangePending = true;
            }
        }

        private void ckbIncludeReserve_CheckedChanged(object sender, EventArgs e)
        {
            _basisSizeMethod.IncludeReserve = ckbIncludeReserve.Checked;
        }
        // End Track #4872
		// end A&F Add Generic Size Curve data 
//
//
//		override protected void CopyAllSizeData(UltraGridRow activeRow)
//		{
//			try
//			{
//				foreach(UltraGridRow setRow in ugRules.Rows)
//				{
//					if ((eSizeMethodRowType)Convert.ToInt32(setRow.Cells["ROW_TYPE_ID"].Value,CultureInfo.CurrentUICulture) != eSizeMethodRowType.AllSize)
//					{
//						setRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//						setRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//						setRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						setRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						setRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//						setRow.Cells["FILL_SEQUENCE"].Value = activeRow.Cells["FILL_SEQUENCE"].Value;
//						
//						if (setRow.HasChild())
//						{
//							//ALL COLORS PATH
//							//========================================================================
//							foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
//							{
//								allColorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//								allColorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//								allColorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//								if (allColorRow.HasChild())
//								{
//									foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//									{
//										allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//										allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//										allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//										if (allColorDimRow.HasChild())
//										{
//											foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//											{
//												allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//												allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//												allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//											}
//										}
//									}
//								}
//							}
//
//							foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
//							{
//								colorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//								colorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//								colorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//								if (colorRow.HasChild())
//								{
//									foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//									{
//										colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//										colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//										colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//
//										if (colorDimRow.HasChild())
//										{
//											foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//											{
//												colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
//												colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
//												colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//											}
//										}
//									}
//								}
//							}
//						}
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "CopyAllSizeData");
//			}
//		}
//
//
//		override protected void ClearAllSizeData(UltraGridRow activeRow)
//		{
//			try
//			{
//				foreach(UltraGridRow setRow in ugRules.Rows)
//				{
//
//					setRow.Cells["SIZE_MIN"].Value = string.Empty;
//					setRow.Cells["SIZE_MAX"].Value = string.Empty;
//					setRow.Cells["SIZE_MULT"].Value = string.Empty;
//					setRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//					setRow.Cells["SIZE_RULE"].Value = string.Empty;
//					setRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eBasisSizeSort.Descending, CultureInfo.CurrentUICulture);
//
//					if (setRow.HasChild())
//					{
//						//ALL COLORS PATH
//						//========================================================================
//						foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
//						{
//							allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//							allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//							allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//							allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (allColorRow.HasChild())
//							{
//								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//								{
//									allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//									if (allColorDimRow.HasChild())
//									{
//										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//										{
//											allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//										}
//									}
//								}
//							}
//						}
//
//
//
//						foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
//						{
//							colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//							colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//							colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//							colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//							if (colorRow.HasChild())
//							{
//								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//								{
//									colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//									if (colorDimRow.HasChild())
//									{
//										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//										{
//											colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//										}
//									}
//								}
//							}
//						}
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "ClearAllSizeData");
//			}
//		}
//
//
//		override protected void ClearSetData(UltraGridRow activeRow)
//		{
//			try
//			{
//				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
//				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
//				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;
//				activeRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eBasisSizeSort.Descending, CultureInfo.CurrentUICulture);
//
//				if (activeRow.HasChild())
//				{
//					//COPY TO ALL COLOR ROW
//					foreach(UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
//					{
//						allColorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						allColorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//
//						if (allColorRow.HasChild())
//						{
//							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
//							{
//								allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (allColorDimRow.HasChild())
//								{
//									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
//									{
//										allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//									}
//								}
//							}
//						}
//					}
//
//					//COPY TO COLOR
//					foreach(UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
//					{
//						colorRow.Cells["SIZE_MIN"].Value = string.Empty;
//						colorRow.Cells["SIZE_MAX"].Value = string.Empty;
//						colorRow.Cells["SIZE_MULT"].Value = string.Empty;
//						colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//
//						if (colorRow.HasChild())
//						{
//							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
//							{
//								colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;
//
//								if (colorDimRow.HasChild())
//								{
//									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
//									{
//										colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
//									}
//								}
//							}
//						}
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "ClearSetData");
//			}
//
//		}

		#endregion

	}
}
