using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;

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
	/// Summary description for ForecastWorkflow.
	/// </summary>
	public class frmForecastWorkflow : WorkflowMethodFormBase
	{
//		private SessionAddressBlock SAB;
//		private FunctionSecurityProfile UserSecurity;
//		private FunctionSecurityProfile GlobalSecurity;
		private FunctionSecurityProfile _forecastMethodsGlobalOTSPlan;
		private FunctionSecurityProfile _forecastMethodsUserOTSPlan;
		private FunctionSecurityProfile _forecastMethodsGlobalOTSBalance;
		private FunctionSecurityProfile _forecastMethodsUserOTSBalance;
//		private FunctionSecurityProfile _forecastWorkflowSecurity;
//		private MIDWorkflowMethodTreeNode _explorerNode = null;
		private OTSPlanWorkFlow _forecastWorkflow;
		private WorkflowMethodManager _wmManager;
		private ColorData _ColorData;
		private SizeGroup _SizeData;
		private Header _Header;
		private HierarchyMaintenance _hm;
		private GetMethods _getMethods;

		private bool _setRowPosition = true;
		private int _nodeRID = -1;
		
		private System.Data.DataTable _methodsDataTable;
		private System.Windows.Forms.GroupBox gbxWorkflow;
		private System.Windows.Forms.Label lblFilter;
		private System.Windows.Forms.CheckBox cbxOverride;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugMethods;
		private System.Windows.Forms.ContextMenu mnuWorkflowGrid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Hashtable _methodStatusHash;
        private MIDComboBoxEnh cboFilter;
        //private SecurityAdmin _secAdmin;	// Issue 5253 //TT#827-MD -jsobek -Allocation Reviews Performance
 
		/// <summary>
		/// Creates an instance of frmForecastWorkflow
		/// </summary>
		/// <param name="SAB">SessionAddressBlock</param>
		public frmForecastWorkflow(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_ForecastWorkflow, eWorkflowMethodType.Workflow)
		{
			try
			{
				
				InitializeComponent();

				_methodStatusHash = new Hashtable();

				_wmManager = new WorkflowMethodManager(SAB.ClientServerSession.UserRID);
				_ColorData = new ColorData();
				_SizeData = new SizeGroup();
				_Header = new Header();
				_hm = new HierarchyMaintenance(SAB);
				_getMethods = new GetMethods(SAB);
				UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastWorkflowsUser);
				GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastWorkflowsGlobal);
                //_secAdmin = new SecurityAdmin();	// Issue 5253 //TT#827-MD -jsobek -Allocation Reviews Performance
				AllowDragDrop = true;
			}
			catch(Exception ex)
			{
				string exceptionMessage = ex.Message;
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

				//remove handlers
				this.ugMethods.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugMethods_MouseUp);
				this.ugMethods.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellChange);
				this.ugMethods.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugMethods_BeforeExitEditMode);
				this.ugMethods.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugMethods_MouseEnterElement);
				this.ugMethods.DragDrop -= new System.Windows.Forms.DragEventHandler(this.ugMethods_DragDrop);
				this.ugMethods.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMethods_AfterRowInsert);
				this.ugMethods.DragEnter -= new System.Windows.Forms.DragEventHandler(this.ugMethods_DragEnter);
				this.ugMethods.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugMethods_BeforeCellUpdate);
				this.ugMethods.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugMethods_BeforeCellListDropDown);
				this.ugMethods.DragOver -= new System.Windows.Forms.DragEventHandler(this.ugMethods_DragOver);
				this.ugMethods.CellListSelect -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellListSelect);
				this.ugMethods.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMethods_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugMethods);
                //End TT#169
				this.ugMethods.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugMethods_AfterSortChange);
				// Begin MID Track 4858 - JSmith - Security changes
//				this.btnProcess.Click -= new System.EventHandler(this.btnProcess_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				// End MID Track 4858
                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                // End TT#44

				// remove data bindings
				this.ugMethods.DataSource = null;
				this.cboFilter.DataSource = null;
				this.mnuWorkflowGrid.Dispose();
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
            this.gbxWorkflow = new System.Windows.Forms.GroupBox();
            this.ugMethods = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbxOverride = new System.Windows.Forms.CheckBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.mnuWorkflowGrid = new System.Windows.Forms.ContextMenu();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxWorkflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugMethods)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(696, 488);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(608, 488);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(24, 488);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // gbxWorkflow
            // 
            this.gbxWorkflow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxWorkflow.Controls.Add(this.cboFilter);
            this.gbxWorkflow.Controls.Add(this.ugMethods);
            this.gbxWorkflow.Controls.Add(this.cbxOverride);
            this.gbxWorkflow.Controls.Add(this.lblFilter);
            this.gbxWorkflow.Location = new System.Drawing.Point(24, 48);
            this.gbxWorkflow.Name = "gbxWorkflow";
            this.gbxWorkflow.Size = new System.Drawing.Size(752, 408);
            this.gbxWorkflow.TabIndex = 29;
            this.gbxWorkflow.TabStop = false;
            // 
            // ugMethods
            // 
            this.ugMethods.AllowDrop = true;
            this.ugMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ugMethods.DisplayLayout.AddNewBox.Hidden = false;
            this.ugMethods.DisplayLayout.AddNewBox.Prompt = " Add ...";
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugMethods.DisplayLayout.Appearance = appearance1;
            ultraGridBand1.AddButtonCaption = " Action";
            this.ugMethods.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
            this.ugMethods.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugMethods.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugMethods.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMethods.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugMethods.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugMethods.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugMethods.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugMethods.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugMethods.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugMethods.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            ultraGridLayout1.AddNewBox.Hidden = false;
            this.ugMethods.Layouts.Add(ultraGridLayout1);
            this.ugMethods.Location = new System.Drawing.Point(16, 72);
            this.ugMethods.Name = "ugMethods";
            this.ugMethods.Size = new System.Drawing.Size(728, 312);
            this.ugMethods.TabIndex = 0;
            this.ugMethods.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMethods_InitializeLayout);
            this.ugMethods.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMethods_AfterRowInsert);
            this.ugMethods.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellChange);
            this.ugMethods.CellListSelect += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellListSelect);
            this.ugMethods.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugMethods_BeforeCellListDropDown);
            this.ugMethods.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugMethods_BeforeExitEditMode);
            this.ugMethods.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugMethods_BeforeCellUpdate);
            this.ugMethods.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugMethods_AfterSortChange);
            this.ugMethods.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugMethods_MouseEnterElement);
            this.ugMethods.DragDrop += new System.Windows.Forms.DragEventHandler(this.ugMethods_DragDrop);
            this.ugMethods.DragEnter += new System.Windows.Forms.DragEventHandler(this.ugMethods_DragEnter);
            this.ugMethods.DragOver += new System.Windows.Forms.DragEventHandler(this.ugMethods_DragOver);
            this.ugMethods.DragLeave += new System.EventHandler(this.ugMethods_DragLeave);
            this.ugMethods.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugMethods_MouseUp);
            // 
            // cbxOverride
            // 
            this.cbxOverride.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbxOverride.Location = new System.Drawing.Point(376, 24);
            this.cbxOverride.Name = "cbxOverride";
            this.cbxOverride.Size = new System.Drawing.Size(104, 24);
            this.cbxOverride.TabIndex = 2;
            this.cbxOverride.Text = "Override";
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(24, 24);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 23);
            this.lblFilter.TabIndex = 1;
            this.lblFilter.Text = "Filter";
            // 
            // cboFilter
            //
			this.cboFilter.AllowDrop = true; 
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.DataSource = null;
            this.cboFilter.DisplayMember = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 0;
            this.cboFilter.Location = new System.Drawing.Point(72, 24);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(224, 21);
            this.cboFilter.TabIndex = 0;
            this.cboFilter.Tag = null;
            this.cboFilter.ValueMember = null;
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            cboFilter.SelectionChangeCommitted += new EventHandler(cboFilter_SelectionChangeCommitted);
            // 
            // frmForecastWorkflow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(800, 534);
            this.Controls.Add(this.gbxWorkflow);
            this.Name = "frmForecastWorkflow";
            this.Text = "Forecast Workflow";
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.gbxWorkflow, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxWorkflow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugMethods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{
				_forecastWorkflow = new OTSPlanWorkFlow(SAB,Include.NoRID,SAB.ClientServerSession.UserRID, false);
				ABW = _forecastWorkflow;
				_forecastWorkflow.WorkFlowType = eWorkflowType.Forecast;
				base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastWorkflowsUser, eSecurityFunctions.ForecastWorkflowsGlobal);

				Common_Load(aParentNode.GlobalUserType);

				cboFilter.SelectedIndex = -1;
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}
		/// <summary>
		/// Opens an existing Workflow. 
		/// </summary>
		/// <param name="aWorkflowRID">workflow_RID</param>
		/// <param name="aNodeRID"></param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aWorkflowRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_nodeRID = aNodeRID;

				_forecastWorkflow = new OTSPlanWorkFlow(SAB,aWorkflowRID,SAB.ClientServerSession.UserRID, false);
				base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastWorkflowsUser, eSecurityFunctions.ForecastWorkflowsGlobal);

				Common_Load(aNode.GlobalUserType);

				if (_forecastWorkflow.ManualOverride)
				{
					this.cbxOverride.Checked = true;
				}
				else
				{
					this.cbxOverride.Checked = false;
				}

				if (_forecastWorkflow.StoreFilterRID == Include.UndefinedStoreFilter)
				{
					cboFilter.SelectedIndex = -1;
				}
				else
				{
					cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_forecastWorkflow.StoreFilterRID, _forecastWorkflow.UserRID, _forecastWorkflow.WorkFlowName));
				}

				int rowPosition = 0;
				string methodName = string.Empty;
				string tolerancePct = string.Empty;
				int methodType;
				ApplicationBaseMethod abm = null;

				foreach (OTSPlanWorkFlowStep aws in _forecastWorkflow.Workflow_Steps)
				{
					tolerancePct = string.Empty;
					methodName = string.Empty;

					methodType = Convert.ToInt32(aws.Method.MethodType, CultureInfo.CurrentUICulture);
					// get method name if method
					if (Enum.IsDefined(typeof(eMethodTypeUI),methodType))
					{
						abm = (ApplicationBaseMethod)aws.Method;
						methodName = Adjust_Method_Name(abm.Name, abm.User_RID);
					}

					if (!aws.UsedSystemTolerancePercent)
					{
						tolerancePct = aws.TolerancePercent.ToString(CultureInfo.CurrentUICulture);
					}

                    //string variableName;
                    //VariableProfile varProf = null;
                    //if (aws.VariableNumber > 0)
                    //{
                    //    varProf = (VariableProfile)SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList.FindKey(aws.VariableNumber);
                    //    variableName = varProf.VariableName;
                    //}
                    //else
                    //{
                    //    varProf = GetDefaultVariableProfile();
                    //    variableName = string.Empty;
                    //}

                    //_methodsDataTable.Rows.Add(new object[] { rowPosition, methodType, aws.Method.Key,
                    //                                            methodName, aws.StoreFilterRID, varProf,
                    //                                            variableName, aws.ComputationMode, aws.Balance,
                    //                                            aws.Review, tolerancePct });
            //Begin TT#255 - MD - Balance option to be hidden - R Beck
                    //_methodsDataTable.Rows.Add(new object[] { rowPosition, methodType, aws.Method.Key,
                    //                                            methodName, aws.StoreFilterRID, 
                    //                                            aws.VariableNumber, aws.ComputationMode, aws.Balance,
                    //                                            aws.Review, tolerancePct });
             
                    _methodsDataTable.Rows.Add(new object[] { rowPosition, methodType, aws.Method.Key,
                                                                methodName, aws.StoreFilterRID, 
                                                                aws.VariableNumber, aws.ComputationMode, 
                                                                aws.Review, tolerancePct });
            //End  TT#255 - MD - Balance option to be hidden - R Beck       
					++rowPosition;
				}

				// BEGIN Issue 4962
				SetVariableListByRow();
				// END Issue 4962
				
				SetMethodCharacteristics();
			}
			catch(Exception ex)
			{
				HandleException(ex, "InitializeForecastWorkflow");
				FormLoadError = true;
			}
		}

		// BEGIN Issue 4962 stodd 11.28.2007
		//==========================================================================================
		// OTS Plan Forecast Only. Fills the variable dropdown with only those variables in 
		// forecasting model or if no model is present, with the default sales and stock variables
		// based upon the merchandise node in the method.
		//==========================================================================================
		private void SetVariableListByRow()
		{
			foreach(  UltraGridRow gridRow in ugMethods.Rows )
			{
                // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                //SetVaraibleList(gridRow);
                SetVaraibleList(gridRow, (eMethodTypeUI)Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture));
                // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
			}
		}

        // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
        //private void SetVaraibleList(UltraGridRow aRow)
        private void SetVaraibleList(UltraGridRow aRow, eMethodTypeUI methodType)
        // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
		{
            VariableProfile vp;
			int methodRID = (int)aRow.Cells["MethodRID"].Value;
			// BEGIN Issue 4962 stodd 11.28.2007
			//==========================================================================================
			// OTS Plan Forecast Only. Fills the variable dropdown with only those variables in 
			// forecasting model or if no model is present, with the default sales and stock variables
			// based upon the merchandise node in the method.
			//==========================================================================================
            //eMethodTypeUI methodType = (eMethodTypeUI)Convert.ToInt32(aRow.Cells["Action"].Value, CultureInfo.CurrentUICulture);  // TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.

            // Begin Issue # 5595 kjohnson
			// Begin Issue #6247 stodd
			if (aRow.Cells["Variable"].Value != DBNull.Value)
			{
				if (aRow.Cells["Variable"].Value.GetType() != typeof(System.String))
				{
					if (Convert.ToInt32(aRow.Cells["Variable"].Value, CultureInfo.CurrentUICulture) == -1)
					{
						aRow.Cells["Variable"].Value = "";
					}
				}
			}
			// End Issue #6247 stodd
            // END Issue 5595

			if ((eForecastVariableMethodType)methodType == eForecastVariableMethodType.OTSPlan)
			{
				OTSPlanMethod forecastMethod = new OTSPlanMethod(SAB, methodRID);
				if (!ugMethods.DisplayLayout.ValueLists.Exists(forecastMethod.Name))
				{
					ugMethods.DisplayLayout.ValueLists.Add(forecastMethod.Name);
					//Begin TT#870 - JScott - Workflow with OTS Forecast Method if a user selects a Variable there is no way to delete it.
					vp = GetDefaultVariableProfile();
					ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(vp.Key, vp.VariableName);
					//End TT#870 - JScott - Workflow with OTS Forecast Method if a user selects a Variable there is no way to delete it.
					if (forecastMethod.ForecastingModel == null)
					{
						forecastMethod.SetDefaultSalesStockVariables(this.ApplicationTransaction);
						if (forecastMethod.SalesVariable.Key != Include.NoRID)
                            //ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(forecastMethod.SalesVariable.VariableProfile);
                            ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(forecastMethod.SalesVariable.VariableProfile.Key, forecastMethod.SalesVariable.VariableProfile.VariableName);
						if (forecastMethod.StockVariable.Key != Include.NoRID)
                            //ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(forecastMethod.StockVariable.VariableProfile);
                            ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(forecastMethod.StockVariable.VariableProfile.Key, forecastMethod.StockVariable.VariableProfile.VariableName);
						aRow.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists[forecastMethod.Name];
					}
					else
					{
						//Begin TT#870 - JScott - Workflow with OTS Forecast Method if a user selects a Variable there is no way to delete it.
						////ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(GetDefaultVariableProfile());
						//vp = GetDefaultVariableProfile();
						//ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(vp.Key, vp.VariableName);
						//End TT#870 - JScott - Workflow with OTS Forecast Method if a user selects a Variable there is no way to delete it.
						foreach (ModelVariableProfile mvp in forecastMethod.ForecastingModel.Variables)
						{
                            //ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(mvp.VariableProfile);
                            ugMethods.DisplayLayout.ValueLists[forecastMethod.Name].ValueListItems.Add(mvp.VariableProfile.Key, mvp.VariableProfile.VariableName);
						}
					}
				}

				aRow.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists[forecastMethod.Name];
			}
            // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
            else if ((eForecastVariableMethodType)methodType == eForecastVariableMethodType.ForecastBalance)
            {
                if (!ugMethods.DisplayLayout.ValueLists.Exists("ForecastBalance"))
                {
                    ugMethods.DisplayLayout.ValueLists.Add("ForecastBalance");
                    vp = GetDefaultVariableProfile();
                    ugMethods.DisplayLayout.ValueLists["ForecastBalance"].ValueListItems.Add(vp.Key, vp.VariableName);
                    foreach (VariableProfile vp2 in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                    {
                        if (vp2.AllowForecastBalance)
                        {
                            ugMethods.DisplayLayout.ValueLists["ForecastBalance"].ValueListItems.Add(vp2.Key, vp2.VariableName);
                        }
                    }
                }


                aRow.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["ForecastBalance"];
            }
            // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.

		}
		// END Issue 4962

		/// <summary>
		/// Deletes a Workflow.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the workflow</param>
		override public bool DeleteWorkflowMethod(int aWorkflowRID)
		{
			try
			{  
				_forecastWorkflow = new OTSPlanWorkFlow(SAB,aWorkflowRID,SAB.ClientServerSession.UserRID, false);
				return Delete();
			}
				// Begin MID Track 2401 - delete error when data in use
			catch(DatabaseForeignKeyViolation keyVio)
			{
				throw keyVio;
			}
				// End MID Track 2401
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames a Forecast Workflow.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aWorkflowRID, string aNewName)
		{
			try
			{       
				_forecastWorkflow = new OTSPlanWorkFlow(SAB,aWorkflowRID,SAB.ClientServerSession.UserRID, false);
				return Rename(aNewName);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
			return false;
		}

		/// <summary>
		/// Processes a Forecast Workflow.
		/// </summary>
		/// <param name="aWorkflowRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public void ProcessWorkflowMethod(int aWorkflowRID)
		{
			try
			{       
				_forecastWorkflow = new OTSPlanWorkFlow(SAB,aWorkflowRID,SAB.ClientServerSession.UserRID, false);
				ProcessWorkflow(true);
			}
			catch (Exception ex)
			{
				HandleException(ex);
				FormLoadError = true;
			}
		}

		private void SetText()
		{
			try
			{
				this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				this.cbxOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Override);
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}
		private void Common_Load(eGlobalUserType aGlobalUserType)
		{
			try
			{
				SetText();

				_forecastMethodsGlobalOTSPlan = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
				_forecastMethodsUserOTSPlan = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSPlan);
				_forecastMethodsGlobalOTSBalance = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
				_forecastMethodsUserOTSBalance = SAB.ClientServerSession.GetUserFunctionSecurityAssignment(SAB.ClientServerSession.UserRID, eSecurityFunctions.ForecastMethodsUserOTSBalance);

				Methods_Define();

                // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Allocation, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _forecastWorkflow.GlobalUserType == eGlobalUserType.User);
                // End TT#44

				BuildFilterComboBox();
				
				this.ugMethods.DataSource = _methodsDataTable;

				BuildWorkflowContextmenu();
				this.ugMethods.ContextMenu = mnuWorkflowGrid;
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		#region Methods Table

		private void ugMethods_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				// check for saved layout
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.OTSForecastWorkflowGrid);
				if (layout.LayoutLength > 0)
				{
					ugMethods.DisplayLayout.Load(layout.LayoutStream);
					AddValueLists();
				}
				else
				{	// DEFAULT grid layout
					AddValueLists();
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                    //ugld.ApplyDefaults(e);
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                    //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                    // End TT#1164
                    //End TT#169
					DefaultMethodsGridLayout();
				}

				this.ugMethods.DisplayLayout.Override.HeaderClickAction = HeaderClickAction.Select;

				if (!FunctionSecurity.AllowUpdate)
				{
					foreach (UltraGridBand ugb in ugMethods.DisplayLayout.Bands)
					{
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}
				else
				{
					foreach (UltraGridBand ugb in ugMethods.DisplayLayout.Bands)
					{
						ugb.Override.AllowDelete = DefaultableBoolean.True;
					}
				}

				Populate_ValueLists();
				CommonMethodsGridLayout();
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void AddValueLists()
		{
			try
			{
				this.ugMethods.DisplayLayout.ValueLists.Clear();
				this.ugMethods.DisplayLayout.ValueLists.Add("Action");
				this.ugMethods.DisplayLayout.ValueLists.Add("Component");
				this.ugMethods.DisplayLayout.ValueLists.Add("Filter");
				this.ugMethods.DisplayLayout.ValueLists.Add("Variable");
				this.ugMethods.DisplayLayout.ValueLists.Add("Mode");

			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void BuildFilterComboBox()
		{
			try
			{
				cboFilter.Items.Clear();
                // Begin TT#2741 - JSmith - Error with [None] filter in allocation workflow
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2741 - JSmith - Error with [None] filter in allocation workflow

				foreach (DataRow row in DtStoreFilters.Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
				
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		private void DefaultMethodsGridLayout()
		{
			try
			{
				this.ugMethods.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].Width = 175;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].ValueList = ugMethods.DisplayLayout.ValueLists["Action"];
				//				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].Header.Caption = "Method/Action";
				this.ugMethods.DisplayLayout.Bands[0].Columns["MethodRID"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].Width = 150;
				//				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].Header.Caption = "Method Name";	
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].Width = 175;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].ValueList = ugMethods.DisplayLayout.ValueLists["Filter"];

				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].Width = 100;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["Variable"];

				this.ugMethods.DisplayLayout.Bands[0].Columns["Mode"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Mode"].Width = 125;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Mode"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Mode"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Mode"].ValueList = ugMethods.DisplayLayout.ValueLists["Mode"];
				//			this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Tolerance"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Tolerance"].Width = 80;

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugMethods);
                //End TT#169
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void CommonMethodsGridLayout()
		{	
			try
			{
				this.ugMethods.DisplayLayout.AddNewBox.Hidden = false;
				this.ugMethods.DisplayLayout.GroupByBox.Hidden = true;
				this.ugMethods.DisplayLayout.GroupByBox.Prompt = string.Empty;
				this.ugMethods.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugMethods.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_ActionOrMethod);

				this.ugMethods.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].ValueList = ugMethods.DisplayLayout.ValueLists["Action"];
				this.ugMethods.DisplayLayout.Bands[0].Columns["Action"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ActionOrMethod);
				this.ugMethods.DisplayLayout.Bands[0].Columns["MethodRID"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MethodName);	
				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				//				this.ugMethods.DisplayLayout.Bands[0].Columns["Method"].ValueList = ugMethods.DisplayLayout.ValueLists["Method"];
                //this.ugMethods.DisplayLayout.Bands[0].Columns["VariableProfile"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].AutoEdit = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].ValueList = ugMethods.DisplayLayout.ValueLists["Filter"];
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				// temorarily hide filter column until ready
				this.ugMethods.DisplayLayout.Bands[0].Columns["Filter"].Hidden = true;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Review);
				this.ugMethods.DisplayLayout.Bands[0].Columns["Tolerance"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Tolerance"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Tolerance);

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugMethods);
                //End TT#169

				// temporarily remove features until ready
				this.ugMethods.DisplayLayout.Bands[0].Columns["Review"].Hidden = true;

				// do not display computation mode column if only one mode
				if (ugMethods.DisplayLayout.ValueLists["Mode"].ValueListItems.Count == 1)
				{
#if (DEBUG)
#else
					ugMethods.DisplayLayout.Bands[0].Columns["Mode"].Hidden = true;
#endif
				}
				else
				{
					ugMethods.DisplayLayout.Bands[0].Columns["Mode"].Hidden = false;
				}
				this.ugMethods.DisplayLayout.Bands[0].Columns["Tolerance"].Hidden = true;

				this.cbxOverride.Visible = false;
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void Methods_Define()
		{
			try
			{
				_methodsDataTable = MIDEnvironment.CreateDataTable("methodsDataTable");
			
				DataColumn dataColumn;

				//Create Columns and rows for datatable
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "RowPosition";
				dataColumn.Caption = "RowPosition";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);


				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Action";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ActionOrMethod);
				//				dataColumn.Caption = "Method/Action";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "MethodRID";
				dataColumn.Caption = "MethodRID";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Method";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_MethodName);
				//				dataColumn.Caption = "Method Name";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Object");
				dataColumn.ColumnName = "Filter";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter);
				//				dataColumn.Caption = "Filter";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Object");
                //dataColumn.ColumnName = "VariableProfile";
                //dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //dataColumn.AllowDBNull = true;
                //_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Object");
				dataColumn.ColumnName = "Variable";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Object");
				dataColumn.ColumnName = "Mode";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ComputationMode);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

            //Begin TT#255 - MD - Balance option to be hidden - R Beck
                //dataColumn = new DataColumn();
                //dataColumn.DataType = System.Type.GetType("System.Boolean");
                //dataColumn.ColumnName = "Balance";
                //dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Balance);
                //dataColumn.ReadOnly = false;
                //dataColumn.Unique = false;
                //dataColumn.AllowDBNull = true;
                //_methodsDataTable.Columns.Add(dataColumn);   
            //End  TT#255 - MD - Balance option to be hidden - R Beck

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Review";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Review);
				//				dataColumn.Caption = "Review";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.String");
				dataColumn.ColumnName = "Tolerance";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Tolerance);
				//				dataColumn.Caption = "Tolerance %";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				//make Sequence column the primary key
				//			DataColumn[] PrimaryKeyColumn = new DataColumn[1];
				//			PrimaryKeyColumn[0] = _methodsDataTable.Columns["RowPosition"];
				//			_methodsDataTable.PrimaryKey = PrimaryKeyColumn;
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void Populate_ValueLists()
		{
            VariableProfile vp;
			try
			{
				ugMethods.DisplayLayout.ValueLists["Action"].ValueListItems.Clear();
				ugMethods.DisplayLayout.ValueLists["Component"].ValueListItems.Clear();
				ugMethods.DisplayLayout.ValueLists["Filter"].ValueListItems.Clear();
				ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Clear();
				ugMethods.DisplayLayout.ValueLists["Mode"].ValueListItems.Clear();

				foreach (DataRow dr in DtMethods.Rows)
				{
					bool addAction = false;
					// exclude non OTS Plan methods
					eForecastMethodType methodType = (eForecastMethodType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
					if (Enum.IsDefined(typeof(eForecastMethodType),methodType))
					{
						addAction = true;
					}

					if (addAction)
					{
						ugMethods.DisplayLayout.ValueLists["Action"].ValueListItems.Add(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture), dr["TEXT_VALUE"].ToString());
					}
				}

				this.ugMethods.DisplayLayout.ValueLists["Filter"].ValueListItems.Add(new FilterNameCombo(Include.UndefinedStoreFilter));
				foreach (DataRow dr in DtStoreFilters.Rows)
				{
					this.ugMethods.DisplayLayout.ValueLists["Filter"].ValueListItems.Add(new FilterNameCombo(Convert.ToInt32(dr["FILTER_RID"], CultureInfo.CurrentUICulture), Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture), dr["FILTER_NAME"].ToString()));
				}

                //ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(GetDefaultVariableProfile());
                //foreach (VariableProfile varProf in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                //{
                //    if (varProf.AllowOTSForecast)
                //    {
                //        ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(varProf);
                //    }
                //}
                vp = GetDefaultVariableProfile();
                ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(vp.Key, vp.VariableName);
                foreach (VariableProfile varProf in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                {
                    if (varProf.AllowOTSForecast)
                    {
                        ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(varProf.Key, varProf.VariableName);
                    }
                }

				foreach (string comp in ComputationModes)
				{
					ugMethods.DisplayLayout.ValueLists["Mode"].ValueListItems.Add(new ComputationModeCombo(comp));
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private FunctionSecurityProfile GetSecurity(eMethodTypeUI aMethod)
		{
			try
			{
				FunctionSecurityProfile functionSecurity = null;
				if (radUser.Checked)
				{
					switch (aMethod)
					{
						case eMethodTypeUI.AllocationOverride:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.BasisSizeAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.FillSizeHolesAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.ForecastBalance:
							functionSecurity = _forecastMethodsUserOTSBalance;
							break;
						case eMethodTypeUI.GeneralAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.OTSPlan:
							functionSecurity = _forecastMethodsUserOTSPlan;
							break;
						case eMethodTypeUI.Rule:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.SizeNeedAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.Velocity:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.WarehouseSizeAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						default:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
					}	
				}
				else
				{
					switch (aMethod)
					{
						case eMethodTypeUI.AllocationOverride:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.BasisSizeAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.FillSizeHolesAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.ForecastBalance:
							functionSecurity = _forecastMethodsGlobalOTSBalance;
							break;
						case eMethodTypeUI.GeneralAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.OTSPlan:
							functionSecurity = _forecastMethodsGlobalOTSPlan;
							break;
						case eMethodTypeUI.Rule:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.SizeNeedAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.Velocity:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						case eMethodTypeUI.WarehouseSizeAllocation:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
						default:
							functionSecurity = new FunctionSecurityProfile(-1);
							break;
					}
				}
				return functionSecurity;
			}
			catch( Exception ex )
			{
				HandleException(ex);
				throw;
			}
		}

		private void SetMethodCharacteristics()
		{
			try
			{
				foreach(  UltraGridRow gridRow in this.ugMethods.Rows )
				{
					eMethodTypeUI methodType = (eMethodTypeUI)Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture);
					// set method column for action
					if (Enum.IsDefined(typeof(eMethodTypeUI),methodType))
					{
						gridRow.Cells["Method"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						Populate_Method_ValueList((eMethodTypeUI)methodType, gridRow.Cells["Method"]);
					}
					else
					{
						gridRow.Cells["Method"].ValueList = null;
						gridRow.Cells["Method"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					}

					// set store filter column for action
					if (Enum.IsDefined(typeof(eFilteredMethodType),(eFilteredMethodType)methodType))
					{
						gridRow.Cells["Filter"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					}
					else
					{
						gridRow.Cells["Filter"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					}

//					// set component column for action
//					if (Enum.IsDefined(typeof(eComponentMethodType),(eComponentMethodType)methodType))
//					{
//						gridRow.Cells["Component"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
//						eComponentType component = (eComponentType)(Convert.ToInt32(gridRow.Cells["Component"].Value, CultureInfo.CurrentUICulture));
//						Populate_Specific_ValueList(component,gridRow.Cells["Specific"]);
//					}
//					else
//					{
//						gridRow.Cells["Specific"].ValueList = null;
//						gridRow.Cells["Component"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
//						gridRow.Cells["Specific"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
//					}

					// set tolerance column for action
					if (Enum.IsDefined(typeof(eToleranceMethodType),(eToleranceMethodType)methodType))
					{
						gridRow.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					}
					else
					{
						gridRow.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					}

					// Begin Issue 4010 - stodd
					if (Enum.IsDefined(typeof(eForecastVariableMethodType),(eForecastVariableMethodType)methodType))
					{
						gridRow.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						gridRow.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					}
					else
					{
						gridRow.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						gridRow.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					}
					// End issue 4010 - stodd

            //Begin TT#255 - MD - Balance option to be hidden - R Beck
                    //if (Enum.IsDefined(typeof(eBalanceStepMethodType),(eBalanceStepMethodType)methodType))
                    //{
                    //    gridRow.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    //}
                    //else
                    //{
                    //    gridRow.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                    //}
            //End   TT#255 - MD - Balance option to be hidden - R Beck

				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void Populate_Method_ValueList(eMethodTypeUI aMethodType, UltraGridCell aCell)
		{
			try
			{
				Infragistics.Win.ValueList valueList = null;
				if (Enum.IsDefined(typeof(eMethodTypeUI), aMethodType))
				{
					// add valuelist to cell if not already have one
					if (aCell.ValueList == null)
					{
						valueList = new Infragistics.Win.ValueList();
						valueList.SortStyle = ValueListSortStyle.Ascending;
						aCell.ValueList = valueList;
					}
					else
					{
						valueList = (Infragistics.Win.ValueList)aCell.ValueList;
						// make sure valuelist is empty
						((Infragistics.Win.ValueList)aCell.ValueList).ValueListItems.Clear();
					}

					DataTable DtMethods = _wmManager.GetMethodList(aMethodType, SAB.ClientServerSession.UserRID);
			
					_methodStatusHash.Clear();
					foreach (DataRow dr in DtMethods.Rows)
					{
						int methodRid = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
						eMethodStatus methodStatus = (eMethodStatus)Convert.ToInt32(dr["METHOD_STATUS"], CultureInfo.CurrentUICulture);
						valueList.ValueListItems.Add(methodRid, Adjust_Method_Name(dr["METHOD_NAME"].ToString(), Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture)));

						_methodStatusHash.Add(methodRid, methodStatus);
					}

				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

//		private void Populate_Specific_ValueList(eComponentType aComponentType, UltraGridCell aCell)
//		{
//			try
//			{
//				Infragistics.Win.ValueList valueList = null;
//				switch (aComponentType)
//				{
//					case eComponentType.SpecificColor:
//						aCell.Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
//						// add valuelist to cell if not already have one
//						if (aCell.ValueList == null)
//						{
//							valueList = new Infragistics.Win.ValueList();
//							valueList.SortStyle = ValueListSortStyle.Ascending;
//							aCell.ValueList = valueList;
//						}
//						else
//						{
//							valueList = (Infragistics.Win.ValueList)aCell.ValueList;
//							// make sure valuelist is empty
//							((Infragistics.Win.ValueList)aCell.ValueList).ValueListItems.Clear();
//						}
//
//						DataTable DtColors = _ColorData.Colors_Read();
//			
//						foreach (DataRow dr in DtColors.Rows)
//						{
//							valueList.ValueListItems.Add(Convert.ToInt32(dr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture), Convert.ToString(dr["COLOR_CODE_ID"], CultureInfo.CurrentUICulture));
//						}
//						break;
//					case eComponentType.SpecificPack:
//						aCell.Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
//						// add valuelist to cell if not already have one
//						if (aCell.ValueList == null)
//						{
//							valueList = new Infragistics.Win.ValueList();
//							valueList.SortStyle = ValueListSortStyle.Ascending;
//							aCell.ValueList = valueList;
//						}
//						else
//						{
//							valueList = (Infragistics.Win.ValueList)aCell.ValueList;
//							// make sure valuelist is empty
//							((Infragistics.Win.ValueList)aCell.ValueList).ValueListItems.Clear();
//						}
//
//						DataTable DtPacks = _Header.GetDistinctPacks();
//			
//						int packCount = 0;
//						foreach (DataRow dr in DtPacks.Rows)
//						{
//							valueList.ValueListItems.Add(packCount, Convert.ToString(dr["HDR_PACK_NAME"], CultureInfo.CurrentUICulture));
//							++packCount;
//						}
//						break;
//					case eComponentType.SpecificSize:
//						aCell.Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
//						// add valuelist to cell if not already have one
//						if (aCell.ValueList == null)
//						{
//							valueList = new Infragistics.Win.ValueList();
//							valueList.SortStyle = ValueListSortStyle.Ascending;
//							aCell.ValueList = valueList;
//						}
//						else
//						{
//							valueList = (Infragistics.Win.ValueList)aCell.ValueList;
//							// make sure valuelist is empty
//							((Infragistics.Win.ValueList)aCell.ValueList).ValueListItems.Clear();
//						}
//
//						DataTable DtSizes = _SizeData.Sizes_Read();
//			
//						foreach (DataRow dr in DtSizes.Rows)
//						{
//							valueList.ValueListItems.Add(Convert.ToInt32(dr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture), Convert.ToString(dr["SIZE_CODE_ID"], CultureInfo.CurrentUICulture));
//						}
//						break;
//					default:
//						aCell.Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
//						break;
//				}
//				
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex);
//			}
//		}

		
		#endregion

		#region Context Menu

		private void BuildWorkflowContextmenu()
		{
			try
			{
				MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
				MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
				//				MenuItem mnuItemMoveUp = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.menu_Move_Up));
				//				MenuItem mnuItemMoveDOwn = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.menu_Move_Down));
				MenuItem mnuItemMoveUp = new MenuItem("Move Up");
				MenuItem mnuItemMoveDown = new MenuItem("Move Down");
				MenuItem mnuItemCut= new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Cut));
				MenuItem mnuItemCopy = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Copy));
				MenuItem mnuItemPaste = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Paste));
				MenuItem mnuItemDelete = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Delete));
				MenuItem mnuItemDeleteAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_DeleteAll));
				if (!FunctionSecurity.AllowUpdate)
				{
					mnuItemInsert.Enabled = false;
					mnuItemInsertBefore.Enabled = false;
					mnuItemInsertAfter.Enabled = false;
					mnuItemMoveUp.Enabled = false;
					mnuItemMoveDown.Enabled = false;
					mnuItemCut.Enabled = false;
					mnuItemCopy.Enabled = false;
					mnuItemPaste.Enabled = false;
					mnuItemDelete.Enabled = false;
					mnuItemDeleteAll.Enabled = false;
				}
				mnuWorkflowGrid.MenuItems.Add(mnuItemInsert);
				mnuItemInsert.MenuItems.Add(mnuItemInsertBefore);
				mnuItemInsert.MenuItems.Add(mnuItemInsertAfter);
				mnuWorkflowGrid.MenuItems.Add(mnuItemMoveUp);
				mnuWorkflowGrid.MenuItems.Add(mnuItemMoveDown);
				mnuWorkflowGrid.MenuItems.Add(mnuItemDelete);
				mnuWorkflowGrid.MenuItems.Add(mnuItemDeleteAll);
				mnuItemInsert.Click += new System.EventHandler(this.mnuItemInsert_Click);
				mnuItemInsertBefore.Click += new System.EventHandler(this.mnuItemInsertBefore_Click);
				mnuItemInsertAfter.Click += new System.EventHandler(this.mnuItemInsertAfter_Click);
				mnuItemMoveUp.Click += new System.EventHandler(this.mnuItemMoveUp_Click);
				mnuItemMoveDown.Click += new System.EventHandler(this.mnuItemMoveDown_Click);
				mnuItemCut.Click += new System.EventHandler(this.mnuItemCut_Cut);
				mnuItemCopy.Click += new System.EventHandler(this.mnuItemCopy_Click);
				mnuItemPaste.Click += new System.EventHandler(this.mnuItemPaste_Click);
				mnuItemDelete.Click += new System.EventHandler(this.mnuItemDelete_Click);
				mnuItemDeleteAll.Click += new System.EventHandler(this.mnuItemDeleteAll_Click);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemInsert_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuItemInsertBefore_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				if (this.ugMethods.ActiveRow != null)
				{
					int rowPosition = Convert.ToInt32(this.ugMethods.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugMethods.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) >= rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
					UltraGridRow addedRow = this.ugMethods.DisplayLayout.Bands[0].AddNew();
					addedRow.Cells["RowPosition"].Value = rowPosition;
					this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Clear();
					this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
					_setRowPosition = true;
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemInsertAfter_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				if (this.ugMethods.ActiveRow != null)
				{
					int rowPosition = Convert.ToInt32(this.ugMethods.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					// increment the position of the active row to end of grid
					foreach(  UltraGridRow gridRow in ugMethods.Rows )
					{
						if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) > rowPosition)
						{
							gridRow.Cells["RowPosition"].Value = Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) + 1;
						}
					}
					UltraGridRow addedRow = this.ugMethods.DisplayLayout.Bands[0].AddNew();
					addedRow.Cells["RowPosition"].Value = rowPosition + 1;
					this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Clear();
					this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
					_setRowPosition = true;
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemMoveUp_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				if (this.ugMethods.ActiveRow != null)
				{
					int rowPosition = Convert.ToInt32(this.ugMethods.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					if (rowPosition > 0)
					{
						// switch the position of the active row with the row above
						foreach(  UltraGridRow gridRow in ugMethods.Rows )
						{
							if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) == rowPosition - 1)
							{
								ugMethods.ActiveRow.Cells["RowPosition"].Value = gridRow.Cells["RowPosition"].Value;
								gridRow.Cells["RowPosition"].Value = rowPosition;
								break;
							}
						}
						this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Clear();
						this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
						ChangePending = true;
						_setRowPosition = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemMoveDown_Click(object sender, System.EventArgs e)
		{
			try
			{
				_setRowPosition = false;
				if (this.ugMethods.ActiveRow != null)
				{
					int rowPosition = Convert.ToInt32(this.ugMethods.ActiveRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture);
					if (rowPosition < this.ugMethods.Rows.Count)
					{
						// switch the position of the active row with the row below
						foreach(  UltraGridRow gridRow in ugMethods.Rows )
						{
							if (Convert.ToInt32(gridRow.Cells["RowPosition"].Value, CultureInfo.CurrentUICulture) == rowPosition + 1)
							{
								ugMethods.ActiveRow.Cells["RowPosition"].Value = gridRow.Cells["RowPosition"].Value;
								gridRow.Cells["RowPosition"].Value = rowPosition;
								break;
							}
						}
						this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Clear();
						this.ugMethods.DisplayLayout.Bands[0].SortedColumns.Add("RowPosition", false);
						ChangePending = true;
						_setRowPosition = true;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemCut_Cut(object sender, System.EventArgs e)
		{
		}

		private void mnuItemCopy_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuItemPaste_Click(object sender, System.EventArgs e)
		{
		}

		private void mnuItemDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (this.ugMethods.ActiveRow != null)
				{
					this.ugMethods.ActiveRow.Delete();
					ChangePending = true;
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void mnuItemDeleteAll_Click(object sender, System.EventArgs e)
		{
			try
			{
				_methodsDataTable.Rows.Clear();
				_methodsDataTable.AcceptChanges();
				ChangePending = true;
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Reassignes the RowPosition after a sort
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ugMethods_AfterSortChange(object sender, Infragistics.Win.UltraWinGrid.BandEventArgs e)
		{
			try
			{
				int count = 0;
				if (_setRowPosition)
				{
					foreach(  UltraGridRow gridRow in ugMethods.Rows )
					{
						gridRow.Cells["RowPosition"].Value = count;
						++count;
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		#endregion Context Menu

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragid)
        //{
        //    try
        //    {
        //        foreach ( Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands )
        //        {
        //            foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns )
        //            {
        //                switch (oColumn.DataType.ToString())
        //                {
        //                    case "System.Int32":
        //                        oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                        oColumn.Format = "#,###,##0";
        //                        break;
        //                    case "System.Double":
        //                        oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                        oColumn.Format = "#,###,###.00";
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch( Exception ex )
        //    {
        //        HandleException(ex);
        //    }
        //}
        //End TT#169

		private void ugMethods_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				switch (ugMethods.ActiveCell.Column.Key)
				{
					case "Action":
						if (Enum.IsDefined(typeof(eMethodTypeUI),ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex)))
						{
							ugMethods.ActiveCell.Row.Cells["Method"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							Populate_Method_ValueList((eMethodTypeUI)ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex), ugMethods.ActiveCell.Row.Cells["Method"]);
                            ugMethods.ActiveCell.Row.Cells["Method"].Value = null;
                            ugMethods.ActiveCell.Row.Cells["MethodRID"].Value = 0;
						}
						else
						{
							ugMethods.ActiveCell.Row.Cells["Method"].ValueList = null;
							ugMethods.ActiveCell.Row.Cells["Method"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}

						// set store filter column for action
						if (Enum.IsDefined(typeof(eFilteredMethodType),ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex)))
						{
							ugMethods.ActiveCell.Row.Cells["Filter"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						}
						else
						{
							ugMethods.ActiveCell.Row.Cells["Filter"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}

						// set tolerance column for action
						if (Enum.IsDefined(typeof(eToleranceMethodType),ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex)))
						{
							ugMethods.ActiveCell.Row.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						}
						else
						{
							ugMethods.ActiveCell.Row.Cells["Tolerance"].Value = string.Empty;
							ugMethods.ActiveCell.Row.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}

                        // Begin Issue # 5595 kjohnson
                        ugMethods.ActiveCell.Row.Cells["Variable"].Value = "";
                        //ugMethods.ActiveCell.Row.Cells["Mode"].Value = "";
                        ugMethods.ActiveCell.Row.Cells["Mode"].Value = SAB.ApplicationServerSession.GetDefaultComputations();
                        // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                        //ugMethods.ActiveCell.Row.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["Variable"];
                        //if ((eMethodTypeUI)Convert.ToInt32(ugMethods.ActiveCell.Row.Cells["Action"].Value, CultureInfo.CurrentUICulture) == eMethodTypeUI.ForecastBalance)
                        if ((eMethodTypeUI)ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex) == eMethodTypeUI.ForecastBalance)
                        {
                            SetVaraibleList(ugMethods.ActiveCell.Row, (eMethodTypeUI)ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex));
                        }
                        else
                        {
                            ugMethods.ActiveCell.Row.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["Variable"];
                        }
                        // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                        // END Issue 5595

						// Begin Issue 4010 - stodd
						if (Enum.IsDefined(typeof(eForecastVariableMethodType),ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex)))
						{
							ugMethods.ActiveCell.Row.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
							ugMethods.ActiveCell.Row.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
						}
						else
						{
							ugMethods.ActiveCell.Row.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
							ugMethods.ActiveCell.Row.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}						// Begin Issue 4010 - stodd

//						else
//						{
//							ugMethods.ActiveCell.Row.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
//						}


                //Begin TT#255 - MD - Balance option to be hidden - R Beck
                        //if (Enum.IsDefined(typeof(eBalanceStepMethodType),ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex)))
                        //{
                        //    ugMethods.ActiveCell.Row.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                        //}
                        //else
                        //{
                        //    ugMethods.ActiveCell.Row.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                        //}
                //End   TT#255 - MD - Balance option to be hidden - R Beck

						break;
					case "Method":
						int methodRID = (int)ugMethods.ActiveCell.ValueListResolved.GetValue(ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex);
						ugMethods.ActiveCell.Row.Cells["MethodRID"].Value = methodRID;

                        // Begin Issue # 5595 kjohnson
                        ugMethods.ActiveCell.Row.Cells["Variable"].Value = "";
                        //ugMethods.ActiveCell.Row.Cells["Mode"].Value = "";
                        ugMethods.ActiveCell.Row.Cells["Mode"].Value = SAB.ApplicationServerSession.GetDefaultComputations();
                        ugMethods.ActiveCell.Row.Cells["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["Variable"];
                        // END Issue 5595

						// BEGIN Issue 4962 stodd 11.28.2007
                        // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                        //SetVaraibleList(ugMethods.ActiveCell.Row);
                        SetVaraibleList(ugMethods.ActiveCell.Row, (eMethodTypeUI)Convert.ToInt32(ugMethods.ActiveCell.Row.Cells["Action"].Value, CultureInfo.CurrentUICulture));
                        // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
						// END Issue 4962
						break;
					case "Variable":
                        //ugMethods.ActiveCell.Row.Cells["VariableProfile"].Value = ugMethods.ActiveCell.Row.Cells["Variable"].ValueListResolved.GetValue(ugMethods.ActiveCell.Row.Cells["Variable"].ValueListResolved.SelectedItemIndex);
						break;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void ugMethods_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				e.Row.Cells["Review"].Value = false;
				e.Row.Cells["RowPosition"].Value = ugMethods.Rows.Count - 1;
				e.Row.Cells["MethodRID"].Value = Include.NoRID;
				e.Row.Cells["Filter"].Value = Include.NoRID;
				e.Row.Cells["Mode"].Value = SAB.ApplicationServerSession.GetDefaultComputations();
                //e.Row.Cells["Balance"].Value = false;                  //TT#255 - MD - Balance option to be hidden - R Beck
				e.Row.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                e.Row.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                e.Row.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void ugMethods_BeforeCellListDropDown(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
		{
			try
			{
				//				if (e.Cell.Column.Key == "Method")
				//				{
				//					e.Cancel = true;
				//					Populate_Method_DropDown_ValueList((eMethodTypeUI)Convert.ToInt32(e.Cell.Row.Cells["Action"].Value));
				//
				//				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void ugMethods_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
				if (ugMethods.ActiveCell.Column.Key == "Specific")
				{
					if (!Convert.IsDBNull(ugMethods.ActiveCell.Row.Cells["Component"].Value) &&
						ugMethods.ActiveCell.Text.Length > 0)	
					{
						// check to add color or size
						eComponentType component = (eComponentType)(Convert.ToInt32(ugMethods.ActiveCell.Row.Cells["Component"].Value, CultureInfo.CurrentUICulture));
						switch (component)
						{
							case eComponentType.SpecificColor:
								try
								{
									ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(ugMethods.ActiveCell.Text);
									if (ccp.Key == Include.NoRID)
									{
										if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AutoAddColorCode),  "Add Color",
											MessageBoxButtons.YesNo, MessageBoxIcon.Question)
											== DialogResult.Yes) 
										{
											string errorMessage = null;
											EditMsgs em = new EditMsgs();
											ccp.ColorCodeChangeType = eChangeType.add;
											ccp.ColorCodeID = ugMethods.ActiveCell.Text;
											ccp.ColorCodeName = ugMethods.ActiveCell.Text;
											ccp.ColorCodeGroup = ugMethods.ActiveCell.Text;
											ccp = _hm.ColorCodeUpdate(ref em, ccp);
											if (em.ErrorFound)
											{
												for (int i=0; i<em.EditMessages.Count; i++)
												{
													EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[i];
													errorMessage += SAB.ClientServerSession.Audit.GetText(emm.code);
												}
											}
											else
											{
												Infragistics.Win.ValueList valueList = (Infragistics.Win.ValueList)ugMethods.ActiveCell.ValueList;
												valueList.ValueListItems.Add(ccp.Key, ccp.ColorCodeID);
												ugMethods.ActiveCell.Value = ugMethods.ActiveCell.Text;
											}
										}
									}
								}
								catch( Exception ex )
								{
									HandleException(ex);
								}
								break;
							case eComponentType.SpecificSize:
								try
								{
									SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(ugMethods.ActiveCell.Text);
									if (scp.Key == Include.NoRID)
									{
										string message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AddSizeCodeQuery);
										message =  message.Replace("{0}", ugMethods.ActiveCell.Text);
										if (MessageBox.Show (message,  "Add Size",
											MessageBoxButtons.YesNo, MessageBoxIcon.Question)
											== DialogResult.Yes) 
										{
											string errorMessage = null;
											EditMsgs em = new EditMsgs();
											scp.SizeCodeChangeType = eChangeType.add;
											scp.SizeCodeID = ugMethods.ActiveCell.Text;
											scp.SizeCodePrimary = ugMethods.ActiveCell.Text;
											scp = _hm.SizeCodeUpdate(ref em, scp);
											if (em.ErrorFound)
											{
												for (int i=0; i<em.EditMessages.Count; i++)
												{
													EditMsgs.Message emm = (EditMsgs.Message) em.EditMessages[i];
													errorMessage += SAB.ClientServerSession.Audit.GetText(emm.code);
												}
											}
											else
											{
												Infragistics.Win.ValueList valueList = (Infragistics.Win.ValueList)ugMethods.ActiveCell.ValueList;
												valueList.ValueListItems.Add(scp.Key, scp.SizeCodeID);
												ugMethods.ActiveCell.Value = ugMethods.ActiveCell.Text;
											}
										}
									}
								}
								catch( Exception ex )
								{
									HandleException(ex);
								}
								break;
						}
					}
				}

				if (DoValueByValueEdits)
				{
					if (ugMethods.ActiveCell.Column.Key == "Tolerance")
					{
						try
						{
							ugMethods.ActiveCell.Appearance.Image = null;
							ugMethods.ActiveCell.Tag = null;
							if (ugMethods.ActiveCell.Text.Length != 0)
							{
								double tolerancePercent = Convert.ToDouble(ugMethods.ActiveCell.Text, CultureInfo.CurrentUICulture);
								if (tolerancePercent < 0)
								{
									string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg);
									ugMethods.ActiveCell.Appearance.Image = ErrorImage;
									ugMethods.ActiveCell.Tag = errorMessage;
								}
							}
						}
						catch
						{
							string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TolerancePctInvalid);
							ugMethods.ActiveCell.Appearance.Image = ErrorImage;
							ugMethods.ActiveCell.Tag = errorMessage;
						}
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void ugMethods_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugMethods, e);
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void ugMethods_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Save_Click(true);
//				DoValueByValueEdits = true;
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex);
//			}
//		}
//
//		private void btnCancel_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				Cancel_Click();
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex);
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

		override protected void BeforeClosing()
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					if (!ugMethods.IsDisposed)
					{
						InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
//						layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.OTSForecastWorkflowGrid, ugMethods);
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void ugMethods_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
            MIDWorkflowMethodTreeNode node;
			try
			{
				Infragistics.Win.UIElement aUIElement;

				Point pt = PointToClient(new Point(e.X, e.Y));
				//				Point realPoint = new Point(pt.X - ugMethods.Location.X, pt.Y - ugMethods.Location.Y - panel1.Height);
				//				Point realPoint = new Point(pt.X - ugMethods.Location.X, pt.Y - ugMethods.Location.Y );
				int X = pt.X - ugMethods.Location.X - this.gbxWorkflow.Left; 
				int Y = pt.Y - ugMethods.Location.Y - this.gbxWorkflow.Top; 
				Point realPoint = new Point(X, Y);
					
				aUIElement = ugMethods.DisplayLayout.UIElement.ElementFromPoint(realPoint);

				UltraGridRow aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)); 

				// Create a new instance of the DataObject interface.
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.Method);
                //MethodClipboardData method_cbd = (MethodClipboardData)cbp.ClipboardData;
                //IDataObject data = Clipboard.GetDataObject();
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;
                    if (!Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)node.MethodType))
                    {
                        string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeOTSPlanMethodToDrop);
                        MessageBox.Show(errorMessage);
                        return;
                    }

                    aRow.Cells["MethodRID"].Value = cbList.ClipboardProfile.Key;
                    aRow.Cells["Method"].Value = Adjust_Method_Name(cbList.ClipboardProfile.Text, node.UserId);
                    aRow.Cells["Action"].Value = Convert.ToInt32(node.MethodType, CultureInfo.CurrentUICulture);
                    if (Enum.IsDefined(typeof(eMethodTypeUI), (eMethodTypeUI)node.MethodType))
                    {
                        Populate_Method_ValueList((eMethodTypeUI)node.MethodType, aRow.Cells["Method"]);
                    }

                    if (Enum.IsDefined(typeof(eToleranceMethodType), (eToleranceMethodType)node.MethodType))
                    {
                        aRow.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    }
                    else
                    {
                        aRow.Cells["Tolerance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                    }

                    // Begin Issue 4010 - stodd
                    if (Enum.IsDefined(typeof(eForecastVariableMethodType), (eForecastVariableMethodType)node.MethodType))
                    {
                        aRow.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                        aRow.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    }
                    else
                    {
                        aRow.Cells["Variable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                        aRow.Cells["Mode"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                    }
                    // Begin Issue 4010 - stodd
                    
            //Begin TT#255 - MD - Balance option to be hidden - R Beck
                    //if (Enum.IsDefined(typeof(eBalanceStepMethodType), (eBalanceStepMethodType)node.MethodType))
                    //{
                    //    aRow.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
                    //}
                    //else
                    //{
                    //    aRow.Cells["Balance"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
                    //}
            //End   TT#255 - MD - Balance option to be hidden - R Beck

                    // BEGIN Issue 4962 stodd 11.28.2007
                    // Begin TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                    //SetVaraibleList(aRow);
                    SetVaraibleList(aRow, (eMethodTypeUI)Convert.ToInt32(aRow.Cells["Action"].Value, CultureInfo.CurrentUICulture));
                    // End TT#4159 - JSmith - Workflow- Matrix Balance Method- Variable drop down has additional variables that are not expected.
                    // END Issue 4962
                }
			}
			catch (Exception ex)
			{
				string message = ex.ToString();
				string text = MIDText.GetTextOnly(eMIDTextCode.msg_MustBeMethodToDrop);
				MessageBox.Show(text);
			}

		}

		private string Adjust_Method_Name(string aMethodName, int aUserRID)
		{
            // Begin TT#1125 - JSmith - Global/User should be consistent
            //if (aUserRID == Include.GlobalUserRID)	// Issue 3806
            //{
            //    aMethodName += " (Global)";
            //}
            //else
            //{
            //    // BEGIN Issue 5253 stodd
            //    string methodUserName = string.Empty;
            //    if (aUserRID == Include.UndefinedUserRID)
            //        methodUserName = " (User)";
            //    else
            //    {
            //        methodUserName = _secAdmin.GetUserName(aUserRID);
            //        methodUserName = " (" + methodUserName + ")";
            //    }
            //    aMethodName += methodUserName;
            //    // END Issue 5253
            //}
            if (aUserRID != Include.GlobalUserRID)	// Issue 3806
            {
                //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                //aMethodName += " (" + _secAdmin.GetUserName(aUserRID) + ")";
                aMethodName += " (" + UserNameStorage.GetUserName(aUserRID) + ")";
                //End TT#827-MD -jsobek -Allocation Reviews Performance
            }
            // End TT#1125
			return aMethodName;
		}

		private void ugMethods_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Image_DragEnter(sender, e);
		}

		private void ugMethods_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
            MIDWorkflowMethodTreeNode node = null;
			try
			{
				Image_DragOver(sender, e);
				if (!FunctionSecurity.AllowUpdate)
				{
					e.Effect = DragDropEffects.None;
					return;
				}

				Infragistics.Win.UIElement aUIElement;

				Point pt = PointToClient(new Point(e.X, e.Y));
				int X = pt.X - ugMethods.Location.X - this.gbxWorkflow.Left; 
				int Y = pt.Y - ugMethods.Location.Y - this.gbxWorkflow.Top; 
				Point realPoint = new Point(X, Y);
					
				aUIElement = ugMethods.DisplayLayout.UIElement.ElementFromPoint(realPoint);
				
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
				if (aRow.Band != ugMethods.DisplayLayout.Bands[0])
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
				if (aCell.Band != ugMethods.DisplayLayout.Bands[0])
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (aCell != aRow.Cells["Method"])
				{
					e.Effect = DragDropEffects.None;
					return;
				}

                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
				{
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //if (cbp.ClipboardDataType == eClipboardDataType.Method)

					// Begin Issue #6247 stodd
                    //if (cbList.ClipboardDataType == eProfileType.Method)
					if (Enum.IsDefined(typeof(eMethodForecastProfileType), Convert.ToInt32(cbList.ClipboardDataType)))
					// End Issue #6247
					{
                        //MethodClipboardData method_cbd = (MethodClipboardData)cbp.ClipboardData;
                        node = (MIDWorkflowMethodTreeNode)cbList.ClipboardProfile.Node;
                        if (!Enum.IsDefined(typeof(eForecastMethodType), (eForecastMethodType)node.MethodType))
						{
							e.Effect = DragDropEffects.None;
						}
						else
						{
							e.Effect = DragDropEffects.Copy;
						}
						return;
					}
				}

				e.Effect = DragDropEffects.None;
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void ugMethods_DragLeave(object sender, EventArgs e)
		{
			Image_DragLeave(sender, e);
		}

        // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
        private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
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
        //End TT#44

        // Begin TT#2741 - JSmith - Error with [None] filter in allocation workflow
        void cboFilter_SelectionChangeCommitted(object sender, EventArgs e)
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
        // End TT#2741 - JSmith - Error with [None] filter in allocation workflow

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnProcess_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				ProcessWorkflow();
//
//				// as part of the  processing we saved the info, so it should be changed to update.
//				if (!ErrorFound)
//				{
//					_forecastWorkflow.Workflow_Change_Type = eChangeType.update;
//					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//				}
//
//			}
//			catch(Exception ex)
//			{
//				HandleException(ex, "btnProcess_Click");
//			}
//		}

		protected override void Call_btnProcess_Click()
		{
			try
			{
				ProcessWorkflow();

				// as part of the  processing we saved the info, so it should be changed to update.
				if (!ErrorFound)
				{
					_forecastWorkflow.Workflow_Change_Type = eChangeType.update;
					this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
				}

			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}
		}
		// End MID Track 4858

		#region WorkflowMethodFormBase Overrides 

		/// <summary>
		/// Gets if workflow or method.
		/// </summary>
		override protected eWorkflowMethodIND WorkflowMethodInd()
		{
			return eWorkflowMethodIND.Workflows;	
		}

		/// <summary>
		/// Use to set the workflow name, description, user and global radio buttons
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
				HandleException(ex);
			}
		}

		/// <summary>
		/// Use to set the specific fields in workflow object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
			try
			{
				//				_forecastWorkflow.StoreFilterRID = Convert.ToInt32(cboFilter.SelectedValue,CultureInfo.CurrentUICulture);
				if (cboFilter.SelectedItem != null)
				{
					_forecastWorkflow.StoreFilterRID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				else
				{
					_forecastWorkflow.StoreFilterRID = Include.UndefinedStoreFilter;
				}

				if (cbxOverride.Checked == true)
				{
					_forecastWorkflow.ManualOverride = true;
				}
				else
				{
					_forecastWorkflow.ManualOverride = false;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Use to validate the fields that are specific to this workflow type
		/// </summary>
		override protected bool ValidateSpecificFields()
		{
			bool fieldsAreValid = true;
			OTSPlanWorkFlowStep awfs = null;
			string errorMessage = null;
			ApplicationBaseAction method = null;
			int methodRID = Include.NoRID;
			eMethodType methodType;
			string methodName;
			bool reviewFlag = false;
			bool useSystemTolerancePercent = true;
			double tolerancePercent = Include.UseSystemTolerancePercent;
			int storeFilter = Include.NoRID;
			int workFlowStepKey = 0;
			int variableNumber = -1;
			string computationMode = SAB.ApplicationServerSession.GetDefaultComputations();
			bool balInd = false;
			try
			{
				// clear and reload all steps
				_forecastWorkflow.Workflow_Steps.Clear();
				if (ugMethods.Rows.Count == 0)
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WorkflowHasNoSteps);
					fieldsAreValid = false;
					ErrorProvider.SetError (ugMethods,errorMessage);
					MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WorkflowHasNoSteps));
				}
				else
				{
					foreach(  UltraGridRow gridRow in this.ugMethods.Rows )
					{
						method = null;
						reviewFlag = false;
						useSystemTolerancePercent = true;
						tolerancePercent = SAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;
						storeFilter = Include.NoRID;
						
						if (gridRow.Cells["Action"].Text.Length == 0)
						{
							if (gridRow.Cells["Method"].Text.Length == 0)
							{
								errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionIsRequired);
								fieldsAreValid = false;
								gridRow.Cells["Action"].Appearance.Image = ErrorImage;
								gridRow.Cells["Action"].Tag = errorMessage;
							}
							else
							{
								methodName = (Convert.ToString(gridRow.Cells["Method"].Value, CultureInfo.CurrentUICulture));
								method = _getMethods.GetMethod(methodName);
								if (method.Key == Include.NoRID)
								{
									errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MethodNameNotFound);
									fieldsAreValid = false;
									gridRow.Cells["Method"].Appearance.Image = ErrorImage;
									gridRow.Cells["Method"].Tag = errorMessage;
								}
							}
						}
//						else
//							if (Enum.IsDefined(typeof(eForecastActionType),Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture)))
//						{
//							// its an action
//							methodType = (eMethodType)Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture);
//							method = new OTSPlanAction(methodType);
//						}
						else
							if (Enum.IsDefined(typeof(eMethodTypeUI),Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture)))
						{
							if (gridRow.Cells["Method"].Text.Length == 0)
							{
								errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MethodRequiredForAction);
								fieldsAreValid = false;
								gridRow.Cells["Method"].Appearance.Image = ErrorImage;
								gridRow.Cells["Method"].Tag = errorMessage;
							}
							else
							{
								gridRow.Cells["Method"].Appearance.Image = null;
								gridRow.Cells["Method"].Tag = null;
								methodType = (eMethodType)(Convert.ToInt32(gridRow.Cells["Action"].Value, CultureInfo.CurrentUICulture));
								methodName = (Convert.ToString(gridRow.Cells["Method"].Text, CultureInfo.CurrentUICulture));
								if (Enum.IsDefined(typeof(eMethodType), methodType))
								{
									methodRID = Convert.ToInt32(gridRow.Cells["MethodRID"].Value, CultureInfo.CurrentUICulture);
									if (methodRID == Include.NoRID)
									{
										// verify method exists for method type
										method = _getMethods.GetMethod(methodName, methodType);
										if (method.Key == Include.NoRID)
										{
											errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WrongTypeOfMethodForAction);
											fieldsAreValid = false;
											gridRow.Cells["Method"].Appearance.Image = ErrorImage;
											gridRow.Cells["Method"].Tag = errorMessage;
										}
									}
									else
									{
										method = _getMethods.GetMethod(methodRID, methodType);
									}

								}
							}
						}
						else
							if (gridRow.Cells["Method"].Text.Length > 0)
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionNotAllowMethod);
							fieldsAreValid = false;
							gridRow.Cells["Method"].Appearance.Image = ErrorImage;
							gridRow.Cells["Method"].Tag = errorMessage;
						}


						try
						{
							gridRow.Cells["Filter"].Appearance.Image = null;
							gridRow.Cells["Filter"].Tag = null;
							storeFilter = Convert.ToInt32(gridRow.Cells["Filter"].Value, CultureInfo.CurrentUICulture);
						}
						catch
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ErrorProcessingStoreFilter);
							fieldsAreValid = false;
							gridRow.Cells["Filter"].Appearance.Image = ErrorImage;
							gridRow.Cells["Filter"].Tag = errorMessage;
						}


						try
						{
							gridRow.Cells["Review"].Appearance.Image = null;
							gridRow.Cells["Review"].Tag = null;
							reviewFlag = Convert.ToBoolean(gridRow.Cells["Review"].Value, CultureInfo.CurrentUICulture);
						}
						catch
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ReviewFlagInvalid);
							fieldsAreValid = false;
							gridRow.Cells["Review"].Appearance.Image = ErrorImage;
							gridRow.Cells["Review"].Tag = errorMessage;
						}

						try
						{
							gridRow.Cells["Variable"].Appearance.Image = null;
							gridRow.Cells["Variable"].Tag = null;
							if (gridRow.Cells["Variable"].Text.Length != 0)
							{
                                //VariableProfile varProf = (VariableProfile)gridRow.Cells["VariableProfile"].Value;
                                //variableNumber = varProf.Key;
                                variableNumber = Convert.ToInt32(gridRow.Cells["Variable"].Value, CultureInfo.CurrentUICulture);
							}
							// Begin Issue 4010 - stodd
							else
							{
                                variableNumber = -1;

								// Begin Issue # 5982 stodd
								// Variable is not a required field
								//=====================================
								//// Begin Issue # 5595 kjohnson
								//if (gridRow.Cells["Variable"].ValueList != null)
								//{
								//    if ((gridRow.Cells["Variable"].ValueList.ItemCount > 0) &&
								//        (gridRow.Cells["Variable"].Activation == Infragistics.Win.UltraWinGrid.Activation.AllowEdit))
								//    {
								//        errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
								//        fieldsAreValid = false;
								//        gridRow.Cells["Variable"].Appearance.Image = ErrorImage;
								//        gridRow.Cells["Variable"].Tag = errorMessage;
								//    }
								//}
								//// End Issue # 5595
								// End Issue # 5982 stodd

							}
							// End Issue 4010 - stodd
						}
						catch
						{
//							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
							fieldsAreValid = false;
							gridRow.Cells["Variable"].Appearance.Image = ErrorImage;
							gridRow.Cells["Variable"].Tag = errorMessage;
						}

						try
						{
							gridRow.Cells["Mode"].Appearance.Image = null;
							gridRow.Cells["Mode"].Tag = null;
							if (gridRow.Cells["Mode"].Text.Length != 0)
							{
								computationMode = Convert.ToString(gridRow.Cells["Mode"].Value, CultureInfo.CurrentCulture);
							}
						}
						catch
						{
							//							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
							fieldsAreValid = false;
							gridRow.Cells["Mode"].Appearance.Image = ErrorImage;
							gridRow.Cells["Mode"].Tag = errorMessage;
						}

                //Begin TT#255 - MD - Balance option to be hidden - R Beck
                        //try
                        //{
                        //    gridRow.Cells["Balance"].Appearance.Image = null;
                        //    gridRow.Cells["Balance"].Tag = null;
                        //    if (gridRow.Cells["Balance"].Text == "True")
                        //    {
                        //        balInd = true;
                        //    }
                        //    else
                        //    {
                        //        balInd = false;
                        //    }
                        //}
                        //catch
                        //{
                        //    //							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
                        //    fieldsAreValid = false;
                        //    gridRow.Cells["Balance"].Appearance.Image = ErrorImage;
                        //    gridRow.Cells["Balance"].Tag = errorMessage;
                        //}
                //End   TT#255 - MD - Balance option to be hidden - R Beck

						try
						{
							gridRow.Cells["Tolerance"].Appearance.Image = null;
							gridRow.Cells["Tolerance"].Tag = null;
							if (gridRow.Cells["Tolerance"].Text.Length != 0)
							{
								tolerancePercent = Convert.ToDouble(gridRow.Cells["Tolerance"].Value, CultureInfo.CurrentUICulture);
								useSystemTolerancePercent = false;
								if (tolerancePercent < 0)
								{
									errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg);
									fieldsAreValid = false;
									gridRow.Cells["Tolerance"].Appearance.Image = ErrorImage;
									gridRow.Cells["Tolerance"].Tag = errorMessage;
								}
							}
						}
						catch
						{
							errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TolerancePctInvalid);
							fieldsAreValid = false;
							gridRow.Cells["Tolerance"].Appearance.Image = ErrorImage;
							gridRow.Cells["Tolerance"].Tag = errorMessage;
						}
						
						if (fieldsAreValid)
						{
							awfs = new OTSPlanWorkFlowStep(method, reviewFlag,
								useSystemTolerancePercent, tolerancePercent, storeFilter, workFlowStepKey,
								variableNumber, computationMode, balInd);
							_forecastWorkflow.Workflow_Steps.Add(awfs);
							++workFlowStepKey;
						}
					}
				}
				
				return fieldsAreValid;
			}
			catch(Exception ex)
			{
				HandleException(ex);
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
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Use to set the specific workflow object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABW = _forecastWorkflow;
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Use to return the explorer node selected when form was opened
		/// </summary>
		override protected MIDWorkflowMethodTreeNode GetExplorerNode()
		{
			return ExplorerNode;
		}

		#endregion WorkflowMethodFormBase Overrides

		#region IFormBase Members
//		override public void ICut()
//		{
//			
//		}
//
//		override public void ICopy()
//		{
//			
//		}
//
//		override public void IPaste()
//		{
//			
//		}	

		//		override public void IClose()
		//		{
		//			try
		//			{
		//				this.Close();
		//
		//			}		
		//			catch(Exception ex)
		//			{
		//				MessageBox.Show(ex.Message);
		//			}
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
//		override public void IDelete()
//		{
//			
//		}
//
//		override public void IRefresh()
//		{
//			
//		}
		
		#endregion

		private void ugMethods_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{

        }

		private void ugMethods_CellListSelect(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			
		}

		override protected bool ApplySecurity()	// Track 5871 stodd
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
			return securityOk;	// track 5871 stodd
		}

		private VariableProfile GetDefaultVariableProfile()
		{
			try
			{
// Begin Track #4868 - JSmith - Variable Groupings
				//return new VariableProfile(0, "(No Override)", eVariableCategory.None, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.None, eVariableScope.None, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.None, 0, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None,0,0,0,0,0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None);
				return new VariableProfile(0, "(No Override)", eVariableCategory.None, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.None, eVariableScope.None, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.None, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None,0,0,0,0,0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, null);
// End Track #4868
			}
			catch
			{
				throw;
			}
		}

				
	}
}
