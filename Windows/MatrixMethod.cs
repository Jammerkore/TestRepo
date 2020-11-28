using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for MatrixMethod.
	/// </summary>
	public class frmMatrixMethod : WorkflowMethodFormBase
	{
		private System.Windows.Forms.TabControl tabOTSMethod;
		private System.Windows.Forms.TabPage tabMethod;
        private System.Windows.Forms.TabPage tabProperties;

		private System.Windows.Forms.ImageList Icons;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdBasis;
        private System.Windows.Forms.Label lblVariable;
		private System.Windows.Forms.GroupBox grpBalanceMode;
		private System.Windows.Forms.RadioButton radBalanceToStore;
		private System.Windows.Forms.RadioButton radBalanceToChain;
		private System.Windows.Forms.GroupBox grpOptions;
		private System.Windows.Forms.Label lblIterations;
        //private MIDRetail.Windows.Controls.MIDComboBoxEnh cboIterations;  //TT#622 - Gets error when attempting to open screen in designer
		private System.Windows.Forms.Button btnVersionOverride;
        private System.Windows.Forms.GroupBox grpOTSPlan;
		private System.Windows.Forms.TextBox txtHighLevelNode;
		private System.Windows.Forms.CheckBox chkSimilarStores;
		private System.Windows.Forms.CheckBox chkIneligibleStores;
		private System.Windows.Forms.Label lblLowLevelsVersion;
		private System.Windows.Forms.Label lblHighLevelVersion;
		private System.Windows.Forms.Label lblLowLevels;
		private System.Windows.Forms.Label lblTimePeriod;
		private System.Windows.Forms.Label lblHighLevel;
        private MIDRetail.Windows.Controls.MIDDateRangeSelector mdsPlanDateRange;
        private System.Windows.Forms.Label lblFilter;
        private UltraGrid ugWorkflows;
		private System.Windows.Forms.GroupBox grpMatrixModel;
		private System.Windows.Forms.Label lblMatrix;
		private System.Windows.Forms.RadioButton radMatrixForecast;
        private System.Windows.Forms.RadioButton radMatrixBalance;
		private System.Windows.Forms.Label lblModelName;
        private System.ComponentModel.IContainer components;
        private MIDComboBoxEnh cboIterations;   //TT#622 - Gets error when attempting to open screen in designer
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cboHighLevelVersion;
        private MIDComboBoxEnh cboLowLevelsVersion;
        private MIDComboBoxEnh cboLowLevels;
        private MIDComboBoxEnh cboVariable;
        private MIDComboBoxEnh cboForecastBalanceName;
        private MIDComboBoxEnh cboOverride;
        private	bool _MIDOnlyFunctions;     

		/// <summary>
		/// Gets the id of the node.
		/// </summary>
		public int NodeRID 
		{
			get { return _nodeRID ; }
		}

		public frmMatrixMethod(SessionAddressBlock SAB, ExplorerAddressBlock aEAB) : base (SAB, aEAB, eMIDTextCode.frm_OTSForecastBalanceMethod, eWorkflowMethodType.Method)
		{
			try
			{
				InitializeComponent();

                // Begin TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method
                //UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSPlan);
                //GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
                UserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsUserOTSBalance);
                GlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
                // End TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastBalanceMethod");
				FormLoadError = true;
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
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				this.cboForecastBalanceName.SelectionChangeCommitted -= new System.EventHandler(this.cbForecastBalanceName_SelectionChangeCommitted);
                this.cboIterations.SelectionChangeCommitted -= new System.EventHandler(this.cboIterations_SelectionChangeCommitted);
                this.cboVariable.SelectionChangeCommitted -= new System.EventHandler(this.cboVariable_SelectionChangeCommitted);
                this.radBalanceToChain.CheckedChanged -= new System.EventHandler(this.radBalanceToChain_CheckedChanged);
                this.radBalanceToStore.CheckedChanged -= new System.EventHandler(this.radBalanceToStore_CheckedChanged);
                this.grdBasis.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
                this.grdBasis.AfterRowsDeleted -= new System.EventHandler(this.grdBasis_AfterRowsDeleted);
                this.grdBasis.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdBasis_BeforeRowInsert);
                this.grdBasis.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.grdBasis_MouseUp);
                this.grdBasis.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(grdBasis);
                //End TT#169
                this.grdBasis.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdBasis_AfterSelectChange);
                this.grdBasis.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdBasis_BeforeRowsDeleted);
                this.grdBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
                this.grdBasis.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
                this.grdBasis.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
                this.grdBasis.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_CellChange);
                this.grdBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
                this.grdBasis.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);  // TT#3986 - JSmith - Excluded basis does not display correctly
                this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DropDown -= new System.EventHandler(this.cboFilter_DropDown);
                this.chkSimilarStores.CheckedChanged -= new System.EventHandler(this.chkSimilarStores_CheckedChanged);
                this.chkIneligibleStores.CheckedChanged -= new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
                this.cboLowLevels.SelectionChangeCommitted -= new System.EventHandler(this.cboLowLevels_SelectionChangeCommitted);
                this.mdsPlanDateRange.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
                this.mdsPlanDateRange.Click -= new System.EventHandler(this.mdsPlanDateRange_Click);
                this.cboHighLevelVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboHighLevelVersion_SelectionChangeCommitted);
                this.cboLowLevelsVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboLowLevelsVersion_SelectionChangeCommitted);
                this.txtHighLevelNode.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
                this.txtHighLevelNode.Validated -= new System.EventHandler(this.txtHighLevelNode_Validated);
                this.txtHighLevelNode.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtHighLevelNode_KeyPress);
                this.txtHighLevelNode.Validating -= new System.ComponentModel.CancelEventHandler(this.txtHighLevelNode_Validating);
                this.txtHighLevelNode.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
                this.txtHighLevelNode.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
                this.btnVersionOverride.Click -= new System.EventHandler(this.btnVersionOverride_Click);
				this.radMatrixForecast.CheckedChanged -= new System.EventHandler(this.radMatrixForecast_CheckedChanged);
				this.radMatrixBalance.CheckedChanged -= new System.EventHandler(this.radMatrixBalance_CheckedChanged);
				// END MID Track #5647

                this.cboForecastBalanceName.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboForecastBalanceName_MIDComboBoxPropertiesChangedEvent);
                this.cboVariable.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboVariable_MIDComboBoxPropertiesChangedEvent);
                this.cboIterations.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboIterations_MIDComboBoxPropertiesChangedEvent);
                this.cboLowLevels.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboLowLevels_MIDComboBoxPropertiesChangedEvent);
                this.cboLowLevelsVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboHighLevelVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
			}
			base.Dispose( disposing );
		}

        //TT#622 - Gets error when attempting to open screen in designer  .ComboBox. removed 03/11/2013
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
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
            this.tabOTSMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.cboForecastBalanceName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboVariable = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblModelName = new System.Windows.Forms.Label();
            this.grpMatrixModel = new System.Windows.Forms.GroupBox();
            this.lblMatrix = new System.Windows.Forms.Label();
            this.radMatrixForecast = new System.Windows.Forms.RadioButton();
            this.radMatrixBalance = new System.Windows.Forms.RadioButton();
            this.cboIterations = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblIterations = new System.Windows.Forms.Label();
            this.lblVariable = new System.Windows.Forms.Label();
            this.grpBalanceMode = new System.Windows.Forms.GroupBox();
            this.radBalanceToChain = new System.Windows.Forms.RadioButton();
            this.radBalanceToStore = new System.Windows.Forms.RadioButton();
            this.grdBasis = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.grpOTSPlan = new System.Windows.Forms.GroupBox();
            this.cboLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboLowLevelsVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboHighLevelVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblFilter = new System.Windows.Forms.Label();
            this.chkSimilarStores = new System.Windows.Forms.CheckBox();
            this.chkIneligibleStores = new System.Windows.Forms.CheckBox();
            this.mdsPlanDateRange = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.txtHighLevelNode = new System.Windows.Forms.TextBox();
            this.lblLowLevelsVersion = new System.Windows.Forms.Label();
            this.lblHighLevelVersion = new System.Windows.Forms.Label();
            this.lblLowLevels = new System.Windows.Forms.Label();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.lblHighLevel = new System.Windows.Forms.Label();
            this.btnVersionOverride = new System.Windows.Forms.Button();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabOTSMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpMatrixModel.SuspendLayout();
            this.grpBalanceMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).BeginInit();
            this.grpOTSPlan.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(632, 528);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(544, 528);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(8, 528);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // tabOTSMethod
            // 
            this.tabOTSMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOTSMethod.Controls.Add(this.tabMethod);
            this.tabOTSMethod.Controls.Add(this.tabProperties);
            this.tabOTSMethod.Location = new System.Drawing.Point(8, 56);
            this.tabOTSMethod.Name = "tabOTSMethod";
            this.tabOTSMethod.SelectedIndex = 0;
            this.tabOTSMethod.Size = new System.Drawing.Size(696, 464);
            this.tabOTSMethod.TabIndex = 17;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.grpOptions);
            this.tabMethod.Controls.Add(this.grdBasis);
            this.tabMethod.Controls.Add(this.grpOTSPlan);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(688, 438);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // grpOptions
            // 
            this.grpOptions.Controls.Add(this.cboForecastBalanceName);
            this.grpOptions.Controls.Add(this.cboVariable);
            this.grpOptions.Controls.Add(this.lblModelName);
            this.grpOptions.Controls.Add(this.grpMatrixModel);
            this.grpOptions.Controls.Add(this.cboIterations);
            this.grpOptions.Controls.Add(this.lblIterations);
            this.grpOptions.Controls.Add(this.lblVariable);
            this.grpOptions.Controls.Add(this.grpBalanceMode);
            this.grpOptions.Location = new System.Drawing.Point(8, 152);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(672, 112);
            this.grpOptions.TabIndex = 10;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // cboForecastBalanceName
            // 
            this.cboForecastBalanceName.AutoAdjust = true;
            this.cboForecastBalanceName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboForecastBalanceName.AutoScroll = true;
            this.cboForecastBalanceName.DataSource = null;
            this.cboForecastBalanceName.DisplayMember = null;
            this.cboForecastBalanceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboForecastBalanceName.DropDownWidth = 0;
            this.cboForecastBalanceName.Location = new System.Drawing.Point(532, 75);
            this.cboForecastBalanceName.Name = "cboForecastBalanceName";
            this.cboForecastBalanceName.Size = new System.Drawing.Size(124, 21);
            this.cboForecastBalanceName.TabIndex = 24;
            this.cboForecastBalanceName.Tag = null;
            this.cboForecastBalanceName.ValueMember = null;
            this.cboForecastBalanceName.SelectionChangeCommitted += new System.EventHandler(this.cbForecastBalanceName_SelectionChangeCommitted);
            this.cboForecastBalanceName.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboForecastBalanceName_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboVariable
            // 
            this.cboVariable.AutoAdjust = true;
            this.cboVariable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVariable.AutoScroll = true;
            this.cboVariable.DataSource = null;
            this.cboVariable.DisplayMember = null;
            this.cboVariable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVariable.DropDownWidth = 0;
            this.cboVariable.Location = new System.Drawing.Point(88, 24);
            this.cboVariable.Name = "cboVariable";
            this.cboVariable.Size = new System.Drawing.Size(192, 21);
            this.cboVariable.TabIndex = 8;
            this.cboVariable.Tag = null;
            this.cboVariable.ValueMember = null;
            this.cboVariable.SelectionChangeCommitted += new System.EventHandler(this.cboVariable_SelectionChangeCommitted);
            this.cboVariable.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboVariable_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(440, 78);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(80, 16);
            this.lblModelName.TabIndex = 23;
            this.lblModelName.Text = "Model Name";
            this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpMatrixModel
            // 
            this.grpMatrixModel.Controls.Add(this.lblMatrix);
            this.grpMatrixModel.Controls.Add(this.radMatrixForecast);
            this.grpMatrixModel.Controls.Add(this.radMatrixBalance);
            this.grpMatrixModel.Location = new System.Drawing.Point(306, 15);
            this.grpMatrixModel.Name = "grpMatrixModel";
            this.grpMatrixModel.Size = new System.Drawing.Size(248, 41);
            this.grpMatrixModel.TabIndex = 22;
            this.grpMatrixModel.TabStop = false;
            // 
            // lblMatrix
            // 
            this.lblMatrix.Location = new System.Drawing.Point(12, 16);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(44, 16);
            this.lblMatrix.TabIndex = 18;
            this.lblMatrix.Text = "Matrix:";
            this.lblMatrix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radMatrixForecast
            // 
            this.radMatrixForecast.Location = new System.Drawing.Point(159, 17);
            this.radMatrixForecast.Name = "radMatrixForecast";
            this.radMatrixForecast.Size = new System.Drawing.Size(81, 16);
            this.radMatrixForecast.TabIndex = 1;
            this.radMatrixForecast.Text = "Forecast";
            this.radMatrixForecast.CheckedChanged += new System.EventHandler(this.radMatrixForecast_CheckedChanged);
            // 
            // radMatrixBalance
            // 
            this.radMatrixBalance.Location = new System.Drawing.Point(81, 17);
            this.radMatrixBalance.Name = "radMatrixBalance";
            this.radMatrixBalance.Size = new System.Drawing.Size(71, 16);
            this.radMatrixBalance.TabIndex = 0;
            this.radMatrixBalance.Text = "Balance";
            this.radMatrixBalance.CheckedChanged += new System.EventHandler(this.radMatrixBalance_CheckedChanged);
            // 
            // cboIterations
            // 
            this.cboIterations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIterations.Location = new System.Drawing.Point(370, 73);
            this.cboIterations.Name = "cboIterations";
            this.cboIterations.Size = new System.Drawing.Size(64, 21);
            this.cboIterations.TabIndex = 11;
            this.cboIterations.SelectionChangeCommitted += new System.EventHandler(this.cboIterations_SelectionChangeCommitted);
            this.cboIterations.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboIterations_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblIterations
            // 
            this.lblIterations.Location = new System.Drawing.Point(298, 72);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(64, 23);
            this.lblIterations.TabIndex = 10;
            this.lblIterations.Text = "Iterations:";
            this.lblIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVariable
            // 
            this.lblVariable.Location = new System.Drawing.Point(24, 23);
            this.lblVariable.Name = "lblVariable";
            this.lblVariable.Size = new System.Drawing.Size(56, 23);
            this.lblVariable.TabIndex = 7;
            this.lblVariable.Text = "Variable:";
            this.lblVariable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpBalanceMode
            // 
            this.grpBalanceMode.Controls.Add(this.radBalanceToChain);
            this.grpBalanceMode.Controls.Add(this.radBalanceToStore);
            this.grpBalanceMode.Location = new System.Drawing.Point(40, 56);
            this.grpBalanceMode.Name = "grpBalanceMode";
            this.grpBalanceMode.Size = new System.Drawing.Size(232, 48);
            this.grpBalanceMode.TabIndex = 9;
            this.grpBalanceMode.TabStop = false;
            this.grpBalanceMode.Text = "Balance Mode";
            // 
            // radBalanceToChain
            // 
            this.radBalanceToChain.Location = new System.Drawing.Point(112, 16);
            this.radBalanceToChain.Name = "radBalanceToChain";
            this.radBalanceToChain.Size = new System.Drawing.Size(104, 24);
            this.radBalanceToChain.TabIndex = 1;
            this.radBalanceToChain.Text = "Chain";
            this.radBalanceToChain.CheckedChanged += new System.EventHandler(this.radBalanceToChain_CheckedChanged);
            // 
            // radBalanceToStore
            // 
            this.radBalanceToStore.Location = new System.Drawing.Point(16, 16);
            this.radBalanceToStore.Name = "radBalanceToStore";
            this.radBalanceToStore.Size = new System.Drawing.Size(104, 24);
            this.radBalanceToStore.TabIndex = 0;
            this.radBalanceToStore.Text = "Store";
            this.radBalanceToStore.CheckedChanged += new System.EventHandler(this.radBalanceToStore_CheckedChanged);
            // 
            // grdBasis
            // 
            this.grdBasis.AllowDrop = true;
            this.grdBasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.grdBasis.DisplayLayout.Appearance = appearance1;
            this.grdBasis.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.grdBasis.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.grdBasis.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdBasis.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.grdBasis.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.grdBasis.DisplayLayout.Override.RowSelectorWidth = 12;
            this.grdBasis.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.grdBasis.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.grdBasis.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.grdBasis.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.grdBasis.Location = new System.Drawing.Point(8, 280);
            this.grdBasis.Name = "grdBasis";
            this.grdBasis.Size = new System.Drawing.Size(672, 144);
            this.grdBasis.TabIndex = 6;
            this.grdBasis.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_AfterCellUpdate);
            this.grdBasis.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdBasis_InitializeLayout);
            this.grdBasis.AfterRowsDeleted += new System.EventHandler(this.grdBasis_AfterRowsDeleted);
            this.grdBasis.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.grdBasis_AfterRowInsert);
            this.grdBasis.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_CellChange);
            this.grdBasis.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.grdBasis_ClickCellButton);
            this.grdBasis.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.grdBasis_AfterSelectChange);
            this.grdBasis.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.grdBasis_BeforeRowInsert);
            this.grdBasis.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.grdBasis_BeforeRowsDeleted);
            this.grdBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragDrop);
            this.grdBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.grdBasis_DragOver);
            this.grdBasis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.grdBasis_MouseUp);
            this.grdBasis.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdBasis_InitializeRow);
            // 
            // grpOTSPlan
            // 
            this.grpOTSPlan.Controls.Add(this.cboLowLevels);
            this.grpOTSPlan.Controls.Add(this.cboLowLevelsVersion);
            this.grpOTSPlan.Controls.Add(this.cboHighLevelVersion);
            this.grpOTSPlan.Controls.Add(this.cboFilter);
            this.grpOTSPlan.Controls.Add(this.cboOverride);
            this.grpOTSPlan.Controls.Add(this.lblFilter);
            this.grpOTSPlan.Controls.Add(this.chkSimilarStores);
            this.grpOTSPlan.Controls.Add(this.chkIneligibleStores);
            this.grpOTSPlan.Controls.Add(this.mdsPlanDateRange);
            this.grpOTSPlan.Controls.Add(this.txtHighLevelNode);
            this.grpOTSPlan.Controls.Add(this.lblLowLevelsVersion);
            this.grpOTSPlan.Controls.Add(this.lblHighLevelVersion);
            this.grpOTSPlan.Controls.Add(this.lblLowLevels);
            this.grpOTSPlan.Controls.Add(this.lblTimePeriod);
            this.grpOTSPlan.Controls.Add(this.lblHighLevel);
            this.grpOTSPlan.Controls.Add(this.btnVersionOverride);
            this.grpOTSPlan.Location = new System.Drawing.Point(8, 24);
            this.grpOTSPlan.Name = "grpOTSPlan";
            this.grpOTSPlan.Size = new System.Drawing.Size(672, 120);
            this.grpOTSPlan.TabIndex = 4;
            this.grpOTSPlan.TabStop = false;
            this.grpOTSPlan.Text = "OTS Plan";
            // 
            // cboLowLevels
            // 
            this.cboLowLevels.AutoAdjust = true;
            this.cboLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevels.AutoScroll = true;
            this.cboLowLevels.DataSource = null;
            this.cboLowLevels.DisplayMember = null;
            this.cboLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevels.DropDownWidth = 0;
            this.cboLowLevels.Location = new System.Drawing.Point(88, 88);
            this.cboLowLevels.Name = "cboLowLevels";
            this.cboLowLevels.Size = new System.Drawing.Size(175, 21);
            this.cboLowLevels.TabIndex = 10;
            this.cboLowLevels.Tag = null;
            this.cboLowLevels.ValueMember = null;
            this.cboLowLevels.SelectionChangeCommitted += new System.EventHandler(this.cboLowLevels_SelectionChangeCommitted);
            this.cboLowLevels.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboLowLevels_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboLowLevelsVersion
            // 
            this.cboLowLevelsVersion.AutoAdjust = true;
            this.cboLowLevelsVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevelsVersion.AutoScroll = true;
            this.cboLowLevelsVersion.DataSource = null;
            this.cboLowLevelsVersion.DisplayMember = null;
            this.cboLowLevelsVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevelsVersion.DropDownWidth = 152;
            this.cboLowLevelsVersion.Location = new System.Drawing.Point(336, 88);
            this.cboLowLevelsVersion.Name = "cboLowLevelsVersion";
            this.cboLowLevelsVersion.Size = new System.Drawing.Size(152, 21);
            this.cboLowLevelsVersion.TabIndex = 9;
            this.cboLowLevelsVersion.Tag = null;
            this.cboLowLevelsVersion.ValueMember = null;
            this.cboLowLevelsVersion.SelectionChangeCommitted += new System.EventHandler(this.cboLowLevels_SelectionChangeCommitted);
            this.cboLowLevelsVersion.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboLowLevelsVersion_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboHighLevelVersion
            // 
            this.cboHighLevelVersion.AutoAdjust = true;
            this.cboHighLevelVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHighLevelVersion.AutoScroll = true;
            this.cboHighLevelVersion.DataSource = null;
            this.cboHighLevelVersion.DisplayMember = null;
            this.cboHighLevelVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHighLevelVersion.DropDownWidth = 152;
            this.cboHighLevelVersion.Location = new System.Drawing.Point(336, 39);
            this.cboHighLevelVersion.Name = "cboHighLevelVersion";
            this.cboHighLevelVersion.Size = new System.Drawing.Size(152, 21);
            this.cboHighLevelVersion.TabIndex = 19;
            this.cboHighLevelVersion.Tag = null;
            this.cboHighLevelVersion.ValueMember = null;
            this.cboHighLevelVersion.SelectionChangeCommitted += new System.EventHandler(this.cboHighLevelVersion_SelectionChangeCommitted);
            this.cboHighLevelVersion.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboHighLevelVersion_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoScroll = true;
            this.cboFilter.DataSource = null;
            this.cboFilter.DisplayMember = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 0;
            this.cboFilter.Location = new System.Drawing.Point(88, 13);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(200, 21);
            this.cboFilter.TabIndex = 17;
            this.cboFilter.Tag = null;
            this.cboFilter.ValueMember = null;
            this.cboFilter.DropDown += new System.EventHandler(this.cboFilter_DropDown);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            cboFilter.SelectionChangeCommitted +=new EventHandler(cboFilter_SelectionChangeCommitted);
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.AutoScroll = true;
            this.cboOverride.DataSource = null;
            this.cboOverride.DisplayMember = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 0;
            this.cboOverride.Location = new System.Drawing.Point(504, 88);
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.Size = new System.Drawing.Size(152, 21);
            this.cboOverride.TabIndex = 18;
            this.cboOverride.Tag = null;
            this.cboOverride.ValueMember = null;
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblFilter
            // 
            this.lblFilter.Location = new System.Drawing.Point(41, 16);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(40, 16);
            this.lblFilter.TabIndex = 16;
            this.lblFilter.Text = "Filter:";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkSimilarStores
            // 
            this.chkSimilarStores.Location = new System.Drawing.Point(520, 37);
            this.chkSimilarStores.Name = "chkSimilarStores";
            this.chkSimilarStores.Size = new System.Drawing.Size(104, 16);
            this.chkSimilarStores.TabIndex = 15;
            this.chkSimilarStores.Text = "Similar Stores";
            this.chkSimilarStores.CheckedChanged += new System.EventHandler(this.chkSimilarStores_CheckedChanged);
            // 
            // chkIneligibleStores
            // 
            this.chkIneligibleStores.Location = new System.Drawing.Point(520, 17);
            this.chkIneligibleStores.Name = "chkIneligibleStores";
            this.chkIneligibleStores.Size = new System.Drawing.Size(104, 16);
            this.chkIneligibleStores.TabIndex = 14;
            this.chkIneligibleStores.Text = "Ineligible Stores";
            this.chkIneligibleStores.CheckedChanged += new System.EventHandler(this.chkIneligibleStores_CheckedChanged);
            // 
            // mdsPlanDateRange
            // 
            this.mdsPlanDateRange.DateRangeForm = null;
            this.mdsPlanDateRange.DateRangeRID = 0;
            this.mdsPlanDateRange.Enabled = false;
            this.mdsPlanDateRange.Location = new System.Drawing.Point(88, 64);
            this.mdsPlanDateRange.Name = "mdsPlanDateRange";
            this.mdsPlanDateRange.Size = new System.Drawing.Size(176, 24);
            this.mdsPlanDateRange.TabIndex = 3;
            this.mdsPlanDateRange.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.mdsPlanDateRange_OnSelection);
            this.mdsPlanDateRange.Click += new System.EventHandler(this.mdsPlanDateRange_Click);
            // 
            // txtHighLevelNode
            // 
            this.txtHighLevelNode.AllowDrop = true;
            this.txtHighLevelNode.Location = new System.Drawing.Point(88, 40);
            this.txtHighLevelNode.Name = "txtHighLevelNode";
            this.txtHighLevelNode.Size = new System.Drawing.Size(176, 20);
            this.txtHighLevelNode.TabIndex = 1;
            this.txtHighLevelNode.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtNode_DragDrop);
            this.txtHighLevelNode.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtNode_DragEnter);
            this.txtHighLevelNode.DragOver += new System.Windows.Forms.DragEventHandler(this.txtNode_DragOver);
            this.txtHighLevelNode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtHighLevelNode_KeyPress);
            this.txtHighLevelNode.Validating += new System.ComponentModel.CancelEventHandler(this.txtHighLevelNode_Validating);
            this.txtHighLevelNode.Validated += new System.EventHandler(this.txtHighLevelNode_Validated);
            // 
            // lblLowLevelsVersion
            // 
            this.lblLowLevelsVersion.Location = new System.Drawing.Point(280, 90);
            this.lblLowLevelsVersion.Name = "lblLowLevelsVersion";
            this.lblLowLevelsVersion.Size = new System.Drawing.Size(48, 16);
            this.lblLowLevelsVersion.TabIndex = 8;
            this.lblLowLevelsVersion.Text = "Version:";
            this.lblLowLevelsVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighLevelVersion
            // 
            this.lblHighLevelVersion.Location = new System.Drawing.Point(280, 41);
            this.lblHighLevelVersion.Name = "lblHighLevelVersion";
            this.lblHighLevelVersion.Size = new System.Drawing.Size(48, 16);
            this.lblHighLevelVersion.TabIndex = 6;
            this.lblHighLevelVersion.Text = "Version:";
            this.lblHighLevelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLowLevels
            // 
            this.lblLowLevels.Location = new System.Drawing.Point(8, 90);
            this.lblLowLevels.Name = "lblLowLevels";
            this.lblLowLevels.Size = new System.Drawing.Size(72, 16);
            this.lblLowLevels.TabIndex = 4;
            this.lblLowLevels.Text = "Low Levels:";
            this.lblLowLevels.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(8, 68);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 2;
            this.lblTimePeriod.Text = "Time Period:";
            this.lblTimePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighLevel
            // 
            this.lblHighLevel.Location = new System.Drawing.Point(16, 42);
            this.lblHighLevel.Name = "lblHighLevel";
            this.lblHighLevel.Size = new System.Drawing.Size(64, 16);
            this.lblHighLevel.TabIndex = 0;
            this.lblHighLevel.Text = "High Level:";
            this.lblHighLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnVersionOverride
            // 
            this.btnVersionOverride.Location = new System.Drawing.Point(504, 60);
            this.btnVersionOverride.Name = "btnVersionOverride";
            this.btnVersionOverride.Size = new System.Drawing.Size(152, 23);
            this.btnVersionOverride.TabIndex = 5;
            this.btnVersionOverride.Text = "Override Low Level Versions";
            this.btnVersionOverride.Click += new System.EventHandler(this.btnVersionOverride_Click);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(688, 438);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance7;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance8.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance8;
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance9.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Left";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance9;
            appearance10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance10;
            appearance11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance11.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance11.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance11;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance12.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance12.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance12.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance12;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(28, 15);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(632, 414);
            this.ugWorkflows.TabIndex = 4;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // frmMatrixMethod
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(720, 558);
            this.Controls.Add(this.tabOTSMethod);
            this.Name = "frmMatrixMethod";
            this.Text = "Matrix";
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.tabOTSMethod, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabOTSMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.grpOptions.ResumeLayout(false);
            this.grpMatrixModel.ResumeLayout(false);
            this.grpBalanceMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdBasis)).EndInit();
            this.grpOTSPlan.ResumeLayout(false);
            this.grpOTSPlan.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Fields

		private Bitmap _picInclude;
		private Bitmap _picExclude;
		private Image _dynamicToPlanImage;
		private Image _dynamicToCurrentImage;
		//private ProfileList _versionProfList;		// Removed. Issue 4858
		private ArrayList _storeList;
		private ProfileList _storeLowLevelVersionList;
		private MIDRetail.Business.OTSForecastBalanceMethod _OTSForecastBalanceMethod = null;
//		private string _strMethodType;
		private int _nodeRID = Include.NoRID;
		private DataSet _dsBasis;
		private VersionProfile _lowLevelVersionDefaultProfile = null;
		private ArrayList _userRIDList;
		//private StoreFilterData _storeFilterDL;
        private FilterData _storeFilterDL; //TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
		private ProfileList _variables;
		private int _planTimeLength = 0;
		private int _basisTimeLength = -1;
		private bool _textChanged = false;
		private bool _priorError = false;
        // Begin MID Issue 2612 - stodd
        HierarchyNodeSecurityProfile _hierNodeSecurity;
        // End MID Issue 2612 - stodd
		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private int _modelIndex = 0;
        private ForecastBalanceProfile _forecastBalanceProfile;
		private bool _setting_radMatrixBalance = false;
		private bool _setting_radMatrixForecast = false;
		// END MID Track #5647

		#endregion

		/// <summary>
		/// Create a new NewOTSForecastBalanceMethod
		/// </summary>
		override public void NewWorkflowMethod(MIDWorkflowMethodTreeNode aParentNode)
		{
			try
			{

				_OTSForecastBalanceMethod = new OTSForecastBalanceMethod(SAB, Include.NoRID);
				ABM = _OTSForecastBalanceMethod;
                // Begin TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method
                //base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
                base.NewWorkflowMethod(aParentNode, eSecurityFunctions.ForecastMethodsUserOTSBalance, eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
                // End TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method

				Common_Load();

				LoadDefaults();  // BEGIN MID Track #5906 - KJohnson - Defaults Not Loading Back In
			}
			catch(Exception ex)
			{
				HandleException(ex, "NewOTSForecastBalanceMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Opens an existing Matrix Method. //Eventually combine with NewOTSForecastBalanceMethod method
		/// 		/// Seperate for debugging & initial development
		/// </summary>
		/// <param name="aMethodRID">method_RID</param>
		/// <param name="aLockStatus">The lock status of the data to be displayed</param>
		override public void UpdateWorkflowMethod(int aMethodRID, int aNodeRID, MIDWorkflowMethodTreeNode aNode, eLockStatus aLockStatus)
		{
			try
			{
				_OTSForecastBalanceMethod = new OTSForecastBalanceMethod(SAB, aMethodRID);

                // Begin TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method
                //base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSPlan, eSecurityFunctions.ForecastMethodsGlobalOTSPlan);
                base.UpdateWorkflowMethod(aLockStatus, eSecurityFunctions.ForecastMethodsUserOTSBalance, eSecurityFunctions.ForecastMethodsGlobalOTSBalance);
                // End TT#3990 - JSmith - Unable to key name or click Save in Global Matrix Balance Method

				Common_Load();

				// Begin MID Track #5209 - JSmith - Unauthorized message
				SetLowLevelOverrideSecurity();
				// End MID Track #5209

				// Issue 4858 stodd 11.15.2007 method security
				ValidateLowLevelSecurity();

                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                if (_OTSForecastBalanceMethod.FoundDuplicate)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, _OTSForecastBalanceMethod.DuplicateMessage),
                    _OTSForecastBalanceMethod.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.btnProcess.Enabled = false;
                }
                // End TT#2281
			}
			catch(Exception ex)
			{
				HandleException(ex, "InitializeOTSForecastBalanceMethod");
				FormLoadError = true;
			}
		}

		/// <summary>
		/// Deletes an Matrix Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		override public bool DeleteWorkflowMethod(int aMethodRID)
		{
			try
			{       
				_OTSForecastBalanceMethod = new OTSForecastBalanceMethod(SAB, aMethodRID);
				return Delete();
			}
			catch(DatabaseForeignKeyViolation)
			{
				throw;
			}
			catch (Exception err)
			{
				HandleException(err);
				FormLoadError = true;
			}

			return true;
		}

		/// <summary>
		/// Renames an Matrix Method.
		/// </summary>
		/// <param name="aMethodRID">The record ID of the method</param>
		/// <param name="aNewName">The new name of the workflow or method</param>
		override public bool RenameWorkflowMethod(int aMethodRID, string aNewName)
		{
			try
			{       
				_OTSForecastBalanceMethod = new OTSForecastBalanceMethod(SAB, aMethodRID);
				return Rename(aNewName);
			}
			catch (Exception err)
			{
				HandleException(err);
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
				_OTSForecastBalanceMethod = new OTSForecastBalanceMethod(SAB, aMethodRID);
                // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                if (_OTSForecastBalanceMethod.FoundDuplicate)
                {
                    MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, _OTSForecastBalanceMethod.DuplicateMessage),
                    _OTSForecastBalanceMethod.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // End TT#2281
				ProcessAction(eMethodType.ForecastBalance, true);
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
				this.grpOTSPlan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlan);
				this.radBalanceToStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore);
				this.radBalanceToChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain);
				this.chkIneligibleStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeIneligibleStore) + ":";
				this.chkSimilarStores.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_IncludeSimilarStores) + ":";
				this.lblTimePeriod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod) + ":";
				this.lblHighLevelVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
				this.lblLowLevelsVersion.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Version) + ":";
				this.lblHighLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_HighLevel) + ":";
				this.lblLowLevels.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_LowLevels) + ":";
				this.btnVersionOverride.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
				this.lblVariable.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable) + ":";
				this.lblIterations.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Iterations) + ":";
				this.lblFilter.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Filter) + ":";
				this.grpBalanceMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BalanceMode) + ":";
				this.grpOptions.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Options) + ":";

				this.tabMethod.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Method);
				this.tabProperties.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Properties);
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				this.lblMatrix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Matrix);
				this.lblModelName.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ModelName) + ":";
				this.radMatrixForecast.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Forecast);
				this.radMatrixBalance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Balance);
				// END MID Track #5647
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void Common_Load()
		{
			try
			{
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				_forecastBalanceProfile = new ForecastBalanceProfile(Include.NoRID);
				// END MID Track #5647

				Icon = MIDGraphics.GetIcon(MIDGraphics.BalanceImage);
				//_storeFilterDL = new StoreFilterData();
                _storeFilterDL = new FilterData();

				//_versionProfList = SAB.ClientServerSession.GetUserForecastVersions();  // Removed. Issue 4858
				_variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;

				SetText();

//				//the following two controls will be used to 
//				this.btnIncExc.Visible = false;
				_picInclude = new Bitmap(GraphicsDirectory + "\\include.gif");
				_picExclude = new Bitmap(GraphicsDirectory + "\\exclude.gif");
				_dynamicToPlanImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToPlanImage);
				_dynamicToCurrentImage = Image.FromFile(GraphicsDirectory + "\\" + MIDGraphics.DynamicToCurrentImage);

				//Populate the form.

				// Get Versions and split into Store and Chain versions

				_storeList = new ArrayList();
				_storeLowLevelVersionList = new ProfileList(eProfileType.Version);

//				foreach (VersionProfile versionProfile in _versionProfList.ArrayList)
//				{
//					// Begin Issue 4562 - stodd - 8.6.07
//					if (versionProfile.Key == Include.FV_ActualRID ||
//						versionProfile.StoreSecurity.AccessDenied ||
//						// If Blended AND the forecast version isn't equal to itself.
//						(versionProfile.IsBlendedVersion && versionProfile.ForecastVersionRID != versionProfile.Key))
//					{
//						// Do not include this version
//					}
//					else
//					{
//						_storeList.Add(versionProfile);
//						_storeLowLevelVersionList.Add(versionProfile);
//					}
//					// End Issue 4562
//				}

				// BEGIN Issue 4858 stodd 10.30.2007 
				ProfileList versionHLProfList = null;
				ProfileList versionLLProfList = null;
				if (this._OTSForecastBalanceMethod.HighLevelVersionRID == Include.NoRID)
					versionHLProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);	// Track #5871
				else
					versionHLProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store, false, _OTSForecastBalanceMethod.HighLevelVersionRID);	// Track #5871
				_storeList.AddRange(versionHLProfList.ArrayList);
				if (this._OTSForecastBalanceMethod.LowLevelVersionRID == Include.NoRID)
					versionLLProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, true);	// Track #5871
				else
					versionLLProfList = base.GetForecastVersionList(eSecuritySelectType.Update, eSecurityTypes.Store, false, _OTSForecastBalanceMethod.LowLevelVersionRID, true);	// Track #5871
				_storeLowLevelVersionList.AddRange(versionLLProfList.ArrayList);

				foreach (LowLevelVersionOverrideProfile lvop in _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList)
				{
					if (lvop.VersionProfile != null)
					{
						if (!_storeLowLevelVersionList.Contains(lvop.VersionProfile.Key))
						{
							if (lvop.VersionProfile.StoreSecurity == null)
								lvop.VersionProfile.StoreSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(lvop.VersionProfile.Key, (int)eSecurityTypes.Store);
							_storeLowLevelVersionList.Add(lvop.VersionProfile);
						}
					}
				}
				// END Issue 4858 stodd 10.30.2007 

				_userRIDList = new ArrayList();
				_userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
				_userRIDList.Add(SAB.ClientServerSession.UserRID);

				// Load Filters

				FunctionSecurityProfile filterUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreUser);
				FunctionSecurityProfile filterGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsFiltersStoreGlobal);

				_userRIDList = new ArrayList();

				_userRIDList.Add(-1);

				if (filterUserSecurity.AllowUpdate || filterUserSecurity.AllowView)
				{
					_userRIDList.Add(SAB.ClientServerSession.UserRID);
				}

				if (filterGlobalSecurity.AllowUpdate || filterGlobalSecurity.AllowView)
				{
					_userRIDList.Add(Include.GlobalUserRID);	// Issue 3806
				}
                //Begin Track #5858 - Kjohnson - Validating store security only
                //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter.ComboBox, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update, FunctionSecurity, _OTSForecastBalanceMethod.GlobalUserType == eGlobalUserType.User);
                //Begin Track #5858 - Kjohnson
				if (this._OTSForecastBalanceMethod.OverrideLowLevelRid != Include.NoRID)
				{
					ModelsData modelData = new ModelsData();
                    cboOverride.SelectedValue = _OTSForecastBalanceMethod.OverrideLowLevelRid;
				}
				BindModelComboBox();  // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				LoadOverrideModelComboBox(cboOverride.ComboBox, _OTSForecastBalanceMethod.OverrideLowLevelRid, _OTSForecastBalanceMethod.CustomOLL_RID);
				BindFilterComboBox();
				BindVersionComboBoxes();
				CreateComboLists();
				BindVariableComboBox();
				BindIterationComboBox();
				// Begin Track # 5949 stodd
				string MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
				if (MIDOnlyFunctionsStr != null)
				{
					MIDOnlyFunctionsStr = MIDOnlyFunctionsStr.ToLower();

					if (MIDOnlyFunctionsStr == "true" || MIDOnlyFunctionsStr == "yes" || MIDOnlyFunctionsStr == "t" || MIDOnlyFunctionsStr == "y" || MIDOnlyFunctionsStr == "1")
					{
						_MIDOnlyFunctions = true;
					}
					else
					{
						_MIDOnlyFunctions = false;
					}
				}
				else
				{
					_MIDOnlyFunctions = false;
				}

                //if (_MIDOnlyFunctions)
                //{
                //    BindComputationModeComboBox();
                //    lblComputationMode.Visible = true;
                //    cboComputationMode.Visible = true;
                //}
                //else
                //{
                //    lblComputationMode.Visible = false;
                //    cboComputationMode.Visible = false;
                //}
				// End Track # 5949 stodd

				SetupDetailsTable();
				LoadMethods();
				grdBasis.DataSource = _dsBasis;
				ProcessBasisRows();

                LoadProperties();

			}

			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private void BindModelComboBox()
		{
			ForecastBalanceProfileList forecastBalanceProfileList = new ForecastBalanceProfileList(true);
			cboForecastBalanceName.Items.Clear();
            cboForecastBalanceName.Items.Add(new ModelNameCombo(Include.NoRID, "", null)); //<---Put Blank In For Default

			foreach (ForecastBalanceProfile fbp in forecastBalanceProfileList.ArrayList)
			{
				cboForecastBalanceName.Items.Add(new ModelNameCombo(fbp.Key, fbp.ModelID, fbp));
			}
		}
		// END MID Track #5647

		private void BindFilterComboBox()
		{
			DataTable dtFilter;

			try
			{
				cboFilter.Items.Clear();
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(-1, Include.GlobalUserRID, "(None)"));	// Issue 3806
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //Begin TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
                dtFilter = _storeFilterDL.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, _userRIDList); 

				foreach (DataRow row in dtFilter.Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
                //End TT#1170-MD -jsobek -Remove Binary database objects and normalize the Filter definitions
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}


		private void BindVersionComboBoxes()
		{
			try
			{
				if (_storeList.Count > 0)
				{
					// BEGIN Issue 4858 stodd 11.15.2007
					cboHighLevelVersion.DisplayMember = "Description";
					cboHighLevelVersion.ValueMember = "Key";
					cboHighLevelVersion.DataSource = _storeList;
					if (_OTSForecastBalanceMethod.Method_Change_Type == eChangeType.add)
					{
						cboHighLevelVersion.SelectedIndex = 0;
						_OTSForecastBalanceMethod.HighLevelVersionRID = Convert.ToInt32(cboHighLevelVersion.SelectedValue, CultureInfo.CurrentUICulture);
					}

					cboLowLevelsVersion.DisplayMember = "Description";
					cboLowLevelsVersion.ValueMember = "Key";
					cboLowLevelsVersion.DataSource = _storeLowLevelVersionList.ArrayList;
					if (_OTSForecastBalanceMethod.Method_Change_Type == eChangeType.add &&
						cboLowLevelsVersion.SelectedValue != null)
					{
						cboLowLevelsVersion.SelectedIndex = 0;
						_OTSForecastBalanceMethod.LowLevelVersionRID = Convert.ToInt32(cboLowLevelsVersion.SelectedValue, CultureInfo.CurrentUICulture);
					}
					// END Issue 4858 stodd 11.15.2007
				}
				// end MID Track # 2365
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindVariableComboBox()
		{
			try
			{
				cboVariable.Items.Clear();
				foreach (VariableProfile vp in _variables)
				{
					if (vp.AllowForecastBalance)
					{
						cboVariable.Items.Add(new VariableCombo(vp.Key, vp.VariableName));
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BindIterationComboBox()
		{
			try
			{
				cboIterations.Items.Clear();
//				cboIterations.Items.Add(new IterationsCombo(eIterationType.UseBase, 0));
				for (int i=1; i<10; i++)
				{
					cboIterations.Items.Add(
						new IterationsCombo(eIterationType.Custom, i));
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates a list for use on the "Version" column, which is a dropdown.
		/// </summary>
		
		private void CreateComboLists()
		{
			int i;
			Infragistics.Win.ValueList vl;
			Infragistics.Win.ValueListItem vli;

			try
			{
				vl = grdBasis.DisplayLayout.ValueLists.Add("Version");
				ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);	// Track #5871
				for (i = 0; i < versionProfList.Count; i++)
				{
					vli = new Infragistics.Win.ValueListItem();
					vli.DataValue= versionProfList[i].Key;
					vli.DisplayText = ((VersionProfile)versionProfList[i]).Description;
					vl.ValueListItems.Add(vli);
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private DataTable SetupDetailsTable()
		{
			try
			{
                _dsBasis = MIDEnvironment.CreateDataSet("basisDataSet");

//				DataColumn dataColumn;

				DataTable dt = _dsBasis.Tables.Add("BasisDetails");
			
//				dt.Columns.Add("BasisID",			System.Type.GetType("System.Int32")); //this column will be hidden.
//				dt.Columns.Add("Merchandise",		System.Type.GetType("System.String"));
//				dt.Columns.Add("MerchandiseID",	System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("Version",			System.Type.GetType("System.String"));
				dt.Columns.Add("VersionID",		System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("DateRange",		System.Type.GetType("System.String"));
				dt.Columns.Add("DateRangeID",		System.Type.GetType("System.Int32")); //this column will be hidden.
				dt.Columns.Add("Picture",			System.Type.GetType("System.String"));	//picture column
				dt.Columns.Add("Weight",			System.Type.GetType("System.Decimal"));
				dt.Columns.Add("IsIncluded",		System.Type.GetType("System.Boolean")); //this column will be hidden. We'll use the buttons column for display.
				dt.Columns.Add("IncludeButton",	System.Type.GetType("System.String")); //button column for include/exclude

				return dt;
			}
			catch
			{
				throw;
			}
		}


		#region Calls to Loading of Defaults and current values
		/// <summary>
		/// Load Methods Tab
		/// </summary>
		private void LoadMethods()
		{
			try
			{
				// Inititalize Fields

				mdsPlanDateRange.DateRangeRID = Include.UndefinedCalendarDateRange;
				mdsPlanDateRange.SetImage(null);

				// Set up "Display" controls

                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSForecastBalanceMethod.FilterRID, -1, ""));
                if (_OTSForecastBalanceMethod.FilterRID == Include.Undefined)
                {
                    cboFilter.SelectedIndex = -1;
                }
                else
                {
                    cboFilter.SelectedIndex = cboFilter.Items.IndexOf(new FilterNameCombo(_OTSForecastBalanceMethod.FilterRID, -1, ""));
                }
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow
				chkIneligibleStores.Checked = _OTSForecastBalanceMethod.IneligibleStoresInd;
				chkSimilarStores.Checked = _OTSForecastBalanceMethod.SimilarStoresInd;

                //Begin Track #5858 - KJohnson - Validating store security only
                txtHighLevelNode.Tag = new MIDMerchandiseTextBoxTag(SAB, txtHighLevelNode, eMIDControlCode.form_Forecast_Balance_Model, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
                //End Track #5858
				if (_OTSForecastBalanceMethod.HnRID > 0)
				{
					//Begin Track #5378 - color and size not qualified
//					HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastBalanceMethod.HnRID);
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastBalanceMethod.HnRID, true, true);
					//End Track #5378
                    //Begin Track #5858 - KJohnson - Validating store security only
                    txtHighLevelNode.Text = hnp.Text;
                    ((MIDTag)(txtHighLevelNode.Tag)).MIDTagData = hnp;
                    //End Track #5858

					PopulateLowLevels(hnp, cboLowLevels.ComboBox);
				}
				else
				{
					txtHighLevelNode.Text = "";
				}

				cboHighLevelVersion.SelectedValue = _OTSForecastBalanceMethod.HighLevelVersionRID;
				// BEGIN Issue 4858 stodd
                //Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboHighLevelVersion, true);
                base.ValidateStorePlanVersionSecurity(cboHighLevelVersion.ComboBox, true);
                //End Track #5858
				// END Issue 4858

				if (_OTSForecastBalanceMethod.CDR_RID > 0)
				{
					DateRangeProfile drp = SAB.ClientServerSession.Calendar.GetDateRange(_OTSForecastBalanceMethod.CDR_RID);
					LoadDateRangeSelector(mdsPlanDateRange, drp);
					SetPlanTimeLength(drp);
				}
				
				cboLowLevelsVersion.SelectedValue = _OTSForecastBalanceMethod.LowLevelVersionRID;
				// BEGIN Issue 4858 stodd
                //Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboLowLevelsVersion);
                base.ValidateStorePlanVersionSecurity(cboLowLevelsVersion.ComboBox);
                //End Track #5858
				// END Issue 4858

				cboLowLevels.SelectedIndex = cboLowLevels.Items.IndexOf(new LowLevelCombo(_OTSForecastBalanceMethod.LowLevelsType, _OTSForecastBalanceMethod.LowLevelsOffset, _OTSForecastBalanceMethod.LowLevelsSequence, ""));
				if (cboLowLevels.Items.Count > 0 &&
					cboLowLevels.SelectedIndex == -1)
				{
					cboLowLevels.SelectedIndex = 0;
				}

				cboVariable.SelectedIndex = cboVariable.Items.IndexOf(new VariableCombo(_OTSForecastBalanceMethod.VariableNumber, ""));
				if (cboVariable.Items.Count > 0 &&
					cboVariable.SelectedIndex == -1)
				{
					cboVariable.SelectedIndex = 0;
				}

				cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(_OTSForecastBalanceMethod.IterationType, _OTSForecastBalanceMethod.IterationsCount));
				if (_OTSForecastBalanceMethod.BalanceMode == eBalanceMode.Chain)
				{
					radBalanceToChain.Checked = true;
				}
				else
				{
					radBalanceToStore.Checked = true;
				}
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				this.cboForecastBalanceName.SelectedIndex = -1;
				_forecastBalanceProfile = new ForecastBalanceProfile(Include.NoRID);
				if (cboForecastBalanceName.Items.Count > 0)
				{
					//---------------Models To Use------------------------------
					bool modelFound = false;
					if (_OTSForecastBalanceMethod.Model_RID != Include.NoRID)
					{   
						for (int i = 0; i < cboForecastBalanceName.Items.Count; i++)
						{
							if (((ModelNameCombo)(cboForecastBalanceName.Items[i])).ModelRID == _OTSForecastBalanceMethod.Model_RID) 
							{
								modelFound = true;
								_modelIndex = i;
								this.cboForecastBalanceName.SelectedIndex = i;
								_forecastBalanceProfile = (ForecastBalanceProfile)((ModelNameCombo)(cboForecastBalanceName.Items[i])).Tag;
							}
						}
					}

					if(modelFound)
					{
                        if (FunctionSecurity.AllowUpdate)
                        {
                            this.cboForecastBalanceName.Enabled = true;
                        }
						LoadOptions(_forecastBalanceProfile);
					}
					else 
					{
                        if (FunctionSecurity.AllowUpdate)
                        {
                            this.cboForecastBalanceName.Enabled = true;
                        }
						LoadOptions(_OTSForecastBalanceMethod);
					}
				}
				else
				{
					//---------------No Models To Use------------------------------
					this.cboForecastBalanceName.Enabled = false;
					LoadOptions(_OTSForecastBalanceMethod);
				}
				// END MID Track #5647

				// End Track #5949 stodd
				LoadBasis();
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadMethods");
			}
		}

		private void LoadBasis()
		{
            // Begin Track #5937 - JSmith - Null reference exception
            ForecastVersionProfileBuilder fvpb = null;
            // End Track #5937
			try
			{
				ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store | eSecurityTypes.Chain);  // Issue 4858 && 5871
				foreach (BasisDetailProfile basisDetailProfile in _OTSForecastBalanceMethod.BasisProfile.BasisDetailProfileList)
				{
					VersionProfile versionProfile = (VersionProfile)versionProfList.FindKey(basisDetailProfile.VersionProfile.Key);
                    // Begin Track #5937 - JSmith - Null reference exception
                    if (versionProfile == null)
                    {
                        if (fvpb == null)
                        {
                            fvpb = new ForecastVersionProfileBuilder();
                        }
                        versionProfile = fvpb.Build(basisDetailProfile.VersionProfile.Key);
                    }
                    // End Track #5937
					bool isIncluded = true;
					if (basisDetailProfile.IncludeExclude == eBasisIncludeExclude.Exclude)
					{
						isIncluded = false;
					}
					_dsBasis.Tables["BasisDetails"].Rows.Add(new object[] { versionProfile.Description, versionProfile.Key,
																			string.Empty, basisDetailProfile.DateRangeProfile.Key,
																			string.Empty, basisDetailProfile.Weight, 
																			isIncluded, string.Empty});
				}

				SetBasisDates();
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadBasis");
			}
		}

		// BEGIN MID Track #5906 - KJohnson - Defaults Not Loading Back In
		private void LoadDefaults()
		{
			try
			{
				cboVariable.SelectedIndex = 0;
				cboIterations.SelectedIndex = 2;
				radBalanceToStore.Checked = true;
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				_setting_radMatrixBalance = true;
				_setting_radMatrixForecast = true;
				radMatrixBalance.Checked = true;
				_setting_radMatrixBalance = false;
				_setting_radMatrixForecast = false;
				// END MID Track #5647
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		// END MID Track #5906 - KJohnson

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private bool LoadOptions(ForecastBalanceProfile aForecastBalanceProfile)
		{
			bool initializeSuccessful = true;
			try
			{
				if (aForecastBalanceProfile != null) 
				{
					if (aForecastBalanceProfile.Variables.Count > 0) 
					{
						cboVariable.SelectedIndex = cboVariable.Items.IndexOf(new VariableCombo(aForecastBalanceProfile.Variables[0].Key, ""));
						if (cboVariable.Items.Count > 0 &&
							cboVariable.SelectedIndex == -1)
						{
							cboVariable.SelectedIndex = 0;
						}
					}

					cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(aForecastBalanceProfile.IterationType, aForecastBalanceProfile.IterationCount));

					if (aForecastBalanceProfile.MatrixType == eMatrixType.Balance)
					{
						_setting_radMatrixBalance = true;
						_setting_radMatrixForecast = true;
						radMatrixBalance.Checked = true;
						_setting_radMatrixBalance = false;
						_setting_radMatrixForecast = false;

						if (cboForecastBalanceName.SelectedIndex <= 0)
						{
							cboIterations.Enabled = true;
							radBalanceToStore.Enabled = true;
							radBalanceToChain.Enabled = true;
						}
					}
					else
					{
						_setting_radMatrixBalance = true;
						_setting_radMatrixForecast = true;
						radMatrixForecast.Checked = true;
						_setting_radMatrixBalance = false;
						_setting_radMatrixForecast = false;

						if (cboForecastBalanceName.SelectedIndex <= 0)
						{
							cboIterations.Enabled = false;
							radBalanceToStore.Enabled = false;
							radBalanceToChain.Enabled = false;
						}
					}

					if (aForecastBalanceProfile.BalanceMode == eBalanceMode.Chain)
					{
						radBalanceToChain.Checked = true;
					}
					else
					{
						radBalanceToStore.Checked = true;
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
				initializeSuccessful = false;
			}
			return initializeSuccessful;
		}

		private bool LoadOptions(OTSForecastBalanceMethod aOTSForecastBalanceMethod)
		{
			bool initializeSuccessful = true;
			try
			{
				if (aOTSForecastBalanceMethod != null) 
				{
					if (aOTSForecastBalanceMethod.VariableNumber != Include.NoRID) 
					{
						cboVariable.SelectedIndex = cboVariable.Items.IndexOf(new VariableCombo(aOTSForecastBalanceMethod.VariableNumber, ""));
						if (cboVariable.Items.Count > 0 &&
							cboVariable.SelectedIndex == -1)
						{
							cboVariable.SelectedIndex = 0;
						}
					}

					cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(aOTSForecastBalanceMethod.IterationType, aOTSForecastBalanceMethod.IterationsCount));

					if (aOTSForecastBalanceMethod.Matrix_Type == eMatrixType.Balance)
					{
						_setting_radMatrixBalance = true;
						_setting_radMatrixForecast = true;
						radMatrixBalance.Checked = true;
						_setting_radMatrixBalance = false;
						_setting_radMatrixForecast = false;

						if (cboForecastBalanceName.SelectedIndex <= 0)
						{
							cboIterations.Enabled = true;
							radBalanceToStore.Enabled = true;
							radBalanceToChain.Enabled = true;
						}
					}
					else
					{
						_setting_radMatrixBalance = true;
						_setting_radMatrixForecast = true;
						radMatrixForecast.Checked = true;
						_setting_radMatrixBalance = false;
						_setting_radMatrixForecast = false;

						if (cboForecastBalanceName.SelectedIndex <= 0)
						{
							cboIterations.Enabled = false;
							radBalanceToStore.Enabled = false;
							radBalanceToChain.Enabled = false;
						}
					}

					if (aOTSForecastBalanceMethod.BalanceMode == eBalanceMode.Chain)
					{
						radBalanceToChain.Checked = true;
					}
					else
					{
						radBalanceToStore.Checked = true;
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
				initializeSuccessful = false;
			}
			return initializeSuccessful;
		}
		// END MID Track #5647

		private void ProcessBasisRows()
		{
			try
			{
				DateRangeProfile dateDateRange;
				foreach(  UltraGridRow gridRow in grdBasis.Rows )
				{
					dateDateRange = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(gridRow.Cells["DateRangeID"].Value, CultureInfo.CurrentCulture));

					if (dateDateRange.DateRangeType == eCalendarRangeType.Dynamic)
					{
						if (dateDateRange.RelativeTo == eDateRangeRelativeTo.Plan)
						{
							gridRow.Cells["DateRange"].Appearance.Image = _dynamicToPlanImage;
						}
						else
						{
							gridRow.Cells["DateRange"].Appearance.Image = _dynamicToCurrentImage;
						}
					}
					else
					{
						gridRow.Cells["DateRange"].Appearance.Image = null;
					}

					double weight = (Convert.ToDouble(gridRow.Cells["Weight"].Value, CultureInfo.CurrentCulture));
					if (weight == 1)
					{
						gridRow.Cells["Weight"].Value = DBNull.Value;
					}

				}
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadBasis");
			}
		}

		/// <summary>
		/// Load Properties Tab of Matrix Method
		/// </summary>
		private void LoadProperties()
		{
			try
			{
				LoadCommon();
				LoadWorkflows();
                //BEGIN TT#110-MD-VStuart - In Use Tool
                tabOTSMethod.Controls.Remove(tabProperties);
                //END TT#110-MD-VStuart - In Use Tool
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadProperties");
			}
		}

		private void LoadCommon()
		{
			try
			{
				this.txtName.Text = _OTSForecastBalanceMethod.Name;
				this.txtDesc.Text = _OTSForecastBalanceMethod.Method_Description;	

				// This fixes the text showing bolder/differently from the other text
				if (!FunctionSecurity.AllowUpdate)
				{
					this.txtName.Enabled = false;
					this.txtDesc.Enabled = false;
				}
                // Begin Track #5476 - JSmith - Opening as wrong user/global type
                //if (_OTSForecastBalanceMethod.User_RID == Include.GetGlobalUserRID())
                //    radGlobal.Checked = true;
                //else
                //    radUser.Checked = true;
                // End Track #5476
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadCommon");
			}
		}
		#endregion

		#region Object Events

		private void cboFilter_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboFilter.SelectedIndex != -1)
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
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboHighLevelVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboHighLevelVersion.SelectedIndex != -1)
				{
					ChangePending = true;
					// change low level version
					int idx = cboLowLevelsVersion.Items.IndexOf(cboHighLevelVersion.SelectedItem);
					if (idx != -1 &&
						idx != cboLowLevelsVersion.SelectedIndex)
					{
						cboLowLevelsVersion.SelectedIndex = idx;
					}
				}
                //Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboHighLevelVersion, true);
                base.ValidateStorePlanVersionSecurity(cboHighLevelVersion.ComboBox, true);
                //End Track #5858

			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboLowLevels_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboLowLevels.SelectedIndex != -1)
				{
					ChangePending = true;
					_OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();

                    // Begin TT#2281 - JSmith - Error when updating Low Level in Matrix Balance OTS method
                    SetSpecificFields();

                    _OTSForecastBalanceMethod.CheckDescendantsForDuplicates();
                    if (_OTSForecastBalanceMethod.FoundDuplicate)
                    {
                        MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_DuplicateDescendantInMethodError, _OTSForecastBalanceMethod.DuplicateMessage),
                        _OTSForecastBalanceMethod.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.btnProcess.Enabled = false;
                    }
                    // End TT#2281
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboLowLevelsVersion_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboLowLevelsVersion.SelectedIndex != -1)
				{
					ChangePending = true;
					// BEGIN Issue 4858 stodd
					_OTSForecastBalanceMethod.LowLevelVersionRID = (int)this.cboLowLevelsVersion.SelectedValue;
					ValidateLowLevelSecurity();
					// END Issue 4858
				}
                //Begin Track #5858 - JSmith - Validating store security only
                //base.ValidatePlanVersionSecurity(cboLowLevelsVersion);
                base.ValidateStorePlanVersionSecurity(cboLowLevelsVersion.ComboBox);
                //End Track #5858

			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void chkIneligibleStores_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void chkSimilarStores_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboVariable_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboVariable.SelectedIndex != -1)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void cboIterations_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded &&
					cboIterations.SelectedIndex != -1)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void radBalanceToStore_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		private void radBalanceToChain_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		#endregion

		#region Grid Events

		private void grdBasis_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				//The following information pertains to the formatting of the grid.
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                //ugld.ApplyDefaults(e);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                //End TT#169

				//NOTE: Bands[0] refers to the "Basis" table.
				//NTOE: Bands[1] refers to the "Details" table.
				// BEGIN MID Track #3792 - replace obsolete method 
				//grdBasis.DisplayLayout.AutoFitColumns = true;
				grdBasis.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				// END MID Track #3792

				//hide the key columns.

//				e.Layout.Bands[0].Columns["BasisID"].Hidden = true;
//				e.Layout.Bands[1].Columns["BasisID"].Hidden = true;
//				e.Layout.Bands[1].Columns["MerchandiseID"].Hidden = true;
				e.Layout.Bands[0].Columns["VersionID"].Hidden = true;
				e.Layout.Bands[0].Columns["IsIncluded"].Hidden = true;
				e.Layout.Bands[0].Columns["DateRangeID"].Hidden = true;
				e.Layout.Bands[0].Columns["Picture"].Hidden = true;

				//Prevent the user from re-arranging columns.

				grdBasis.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				//Set the header captions.

//				grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
				grdBasis.DisplayLayout.Bands[0].Columns["Version"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Version);
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTimePeriod);
				grdBasis.DisplayLayout.Bands[0].Columns["Weight"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Weight);
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Header.Caption = " ";

				//Set the widths of the columns.

//				grdBasis.DisplayLayout.Bands[1].Columns["Merchandise"].Width = 200;
				grdBasis.DisplayLayout.Bands[0].Columns["Version"].Width = 100;
				grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Width = 200;
				grdBasis.DisplayLayout.Bands[0].Columns["Weight"].Width = 80;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Width = 20;

				//hide the column header.

//				grdBasis.DisplayLayout.Bands[0].ColHeadersVisible = false;

				//Make some columns readonly.

//				e.Layout.Bands[0].Columns["BasisName"].CellActivation = Activation.NoEdit;
		
				//make the "Version" column a drop down list.

				grdBasis.DisplayLayout.Bands[0].Columns["Version"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				grdBasis.DisplayLayout.Bands[0].Columns["Version"].ValueList = grdBasis.DisplayLayout.ValueLists["Version"];
		
				//the "IncludeButton" column is the column that contains buttons
				//to include/exclude a basis detail. 

				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageHAlign = Infragistics.Win.HAlign.Center;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellAppearance.ImageVAlign = Infragistics.Win.VAlign.Middle;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.Button;
				grdBasis.DisplayLayout.Bands[0].Columns["IncludeButton"].CellActivation = Activation.NoEdit;

				// BEGIN Issue 4640 stodd 09.21.2007
				if (FunctionSecurity.AllowUpdate)
				{
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
					grdBasis.DisplayLayout.Bands[0].Columns["DateRange"].CellActivation = Activation.ActivateOnly;
				}
				// END Issue 4640 stodd 09.21.2007

				//the following code tweaks the "Add New" buttons (which come with the grid).

//				grdBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasis);
//				grdBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasis);
				grdBasis.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddBasisDetails);
				grdBasis.DisplayLayout.Bands[0].AddButtonToolTipText = MIDText.GetTextOnly(eMIDTextCode.tt_Button_AddBasisDetails);
				grdBasis.DisplayLayout.AddNewBox.Hidden = false;
				grdBasis.DisplayLayout.AddNewBox.Style = AddNewBoxStyle.Compact;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				//Set the width of the "DateRange" column.

				if (e.Row.Cells["IsIncluded"].Value != DBNull.Value)
				{
					if (Convert.ToBoolean(e.Row.Cells["IsIncluded"].Value, CultureInfo.CurrentUICulture) == true)
					{
						e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;
					}
					else
					{
						e.Row.Cells["IncludeButton"].Appearance.Image = _picExclude;
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		private void grdBasis_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
//			DataRow basisRow;
//			DataRow detailsRow;
//			UltraGridRow row;

			try
			{
				this.Cursor = Cursors.WaitCursor;

				CheckInsertCondition(e);

				//If we are inserting a parent row (or "Basis"), we want to set both
				//its ID and Name, instead of letting the user do it. The best way
				//is to directly add the new row in the datatable, because the bound grid
				//will automatically reflect this change.

//				if (e.Band == grdBasis.DisplayLayout.Bands[0])
//				{
//					basisRow = (DataRow)_dsBasis.Tables["Basis"].NewRow();
//
//					if (_dsBasis.Tables["Basis"].Rows.Count == 0)
//					{
//						basisRow["BasisID"] = 0;
//					}
//					else
//					{
//						basisRow["BasisID"] = Convert.ToInt32(_dsBasis.Tables["Basis"].Rows[_dsBasis.Tables["Basis"].Rows.Count-1]["BasisID"], CultureInfo.CurrentUICulture) + 1; //Increase the ID by 1 (based on the last row's ID).
//					}
//
//					basisRow["BasisName"] = "Basis " + Convert.ToString(_dsBasis.Tables["Basis"].Rows.Count + 1, CultureInfo.CurrentUICulture);
//					detailsRow = (DataRow)_dsBasis.Tables["BasisDetails"].NewRow();
//					detailsRow["BasisID"] = basisRow["BasisID"]; 
//					detailsRow["IsIncluded"] = true;
//
//					_dsBasis.Tables["Basis"].Rows.Add(basisRow);
//					_dsBasis.Tables["BasisDetails"].Rows.Add(detailsRow);
//
//					//Set the active row to this newly added Basis row.
//
//					grdBasis.ActiveRow = grdBasis.Rows[grdBasis.Rows.Count - 1];
//
//					//Since we've already added the necessary information in the underlying
//					//datatable, we want to cancel out because if we don't, the grid will
//					//add another blank row (in addition to the row we just added to the datatable).
//
//					e.Cancel = true;
//				}
//
//				//Expand the parent row (Basis) so the user can see the child row (Details).
//
//				row = this.grdBasis.Rows[this.grdBasis.Rows.Count - 1];
//
//				if (row.IsExpandable)
//				{
//					row.Expanded = true;
//				}
			}
			catch (EditErrorException exc)
			{
				MessageBox.Show(exc.Message, "Edit error", MessageBoxButtons.OK);
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// checks to make sure that the previous row, if there is one, is completely
		/// filled out. If any information is missing, return false to the calling
		/// procedure to indicate that it should not proceed adding another row.
		/// </summary>
		/// <returns></returns>
		
		private void CheckInsertCondition(Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			DataView dv;
			DataRowView drv;

			//Check to see if there is already a Details row. It should be impossible to not have one, but we want to check anyway.

			try
			{
				if (e.Band == grdBasis.DisplayLayout.Bands[0])
				{
					//Find all the rows that are children of the Basis row. (Rows in Details table having the save BasisID.)

					dv = new DataView();
					dv.Table = _dsBasis.Tables["BasisDetails"];
//					dv.RowFilter = "BasisID = " + e.ParentRow.Cells["BasisID"].Value.ToString();
				
					if (dv.Count != 0) 
					{
						//We are in this block of code because the user is trying to insert a Details row, and there are Details rows already existing.
						//Retrieve the last child row of the Basis row. (The last row in Details table having the same BasisID)

						drv = dv[dv.Count - 1];

						//Check various fields to make sure that they're all filled out.

//						if (drv["Merchandise"].ToString() == string.Empty)
//						{
//							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
//						}
						if (drv["Version"].ToString() == string.Empty)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
						}
						if (drv["DateRange"].ToString() == string.Empty)
						{
							throw new EditErrorException(MIDText.GetText(eMIDTextCode.msg_pl_FillPreviousRow));
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void grdBasis_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				//if the updated cell is the version dropdown column, get the selected
				//item's id and put it in the "VersionID" column.
				
				if (e.Cell.Band == grdBasis.DisplayLayout.Bands[0])
				{
					if (e.Cell == e.Cell.Row.Cells["Version"])
					{
						int selectedIndex = grdBasis.DisplayLayout.ValueLists["Version"].SelectedIndex;

						e.Cell.Row.Cells["VersionID"].Value = Convert.ToInt32(grdBasis.DisplayLayout.ValueLists["Version"].ValueListItems[selectedIndex].DataValue, CultureInfo.CurrentUICulture);
					}
				}

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
		
		private void grdBasis_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				//The "IsIncluded" column is a checkbox column. Right after we inserted a
				//new row, we want to default this column to be checked (true). If we
				//don't, it will default to a grayed-out check, like the 3rd state in a 
				//tri-state checkbox, even if we explicitly set this column to be a normal
				//checkbox.
				
				if (e.Row.Band == grdBasis.DisplayLayout.Bands[0])
				{
					e.Row.Cells["IsIncluded"].Value = true;
					e.Row.Cells["DateRangeID"].Value = Include.UndefinedCalendarDateRange;
					e.Row.Cells["IncludeButton"].Appearance.Image = _picInclude;

//					//There is a bug where the grid is not refreshed to display the new
//					//cell. We will manually close and expand its parent row to trick the 
//					//grid to do a refresh.
//
//					e.Row.ParentRow.Expanded = false;
//					e.Row.ParentRow.Expanded = true;
				}

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

		private void grdBasis_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
//			int eMatchingID;
//			int i;
//			int j;
//
//			try
//			{
//				grdBasis.UpdateData();
//				_dsBasis.AcceptChanges();
//
//				//We want to make sure that after the deletion, there are no "orphaned parent".
//				//Every "Basis" row must have at least one "Details" row.
//
//				if (e.Rows[0].Band == grdBasis.DisplayLayout.Bands[0])
//				{
//					//Infragistics UltraGrid doesn't allow the user to delete a parent-level
//					//row and a child-level row at the same time. So by checking the first
//					//item in the collection, we can pretty much guarantee that the rest of 
//					//the collection is on the same leve.
//
//					//store the number of rows with the same ID (from the delete collection).
//
//					eMatchingID = 0; 
//
//					for (i = 0; i < e.Rows.Length; i += eMatchingID)
//					{
//						DataView dv = new DataView();
//						dv.Table = _dsBasis.Tables["BasisDetails"];
//						dv.RowFilter = "BasisID = " + e.Rows[i].Cells["BasisID"].Value.ToString();
//
//						//Count the number of rows in the delete rows collection that 
//						//have the same BasisID
//
//						eMatchingID = 0;
//
//						for (j = i; j < e.Rows.Length; j++)
//						{
//							if (e.Rows[j].Cells["BasisID"].Value.ToString() ==
//								e.Rows[i].Cells["BasisID"].Value.ToString())
//								eMatchingID ++;
//						}
//
//						if (dv.Count == eMatchingID)
//						{
//							//the user is trying to delete ALL the details rows. Prevent it.
//
//							e.DisplayPromptMsg = false;
//							MessageBox.Show("The delete cannot be performed because it makes at least one Basis without Details.\r\n" + 
//								"You must leave at least one Basis Detail for each Basis.\r\n\r\n" +
//								"If you are sure you want to delete all the details, \r\ndelete the Basis itself.", "Error", MessageBoxButtons.OK);
//							e.Cancel = true;
//						}
//					}
//				}
//			}
//			catch (Exception exc)
//			{
//				HandleException(exc);
//			}
		}

		private void grdBasis_AfterRowsDeleted(object sender, System.EventArgs e)
		{
//			int i;
//
			try
			{
//				//update row states so that the deleted row really DOES disappear.
//
//				grdBasis.DataSource = null;
//
//				_dsBasis.AcceptChanges();
//
//				//if the user deleted a BASIS row, we want to re-assign captions and IDs
//				//of the remaining Basis so that remaining Basis remain sequenced.
//				//For example, say we have Basis 1, Basis 2, Basis 3, and Basis 4. If the
//				//user deleted Basis 2, we want to rename Basis 3 and 4 so that after the
//				//deletion, we still see Basis 1, 2, and 3 (not 1, 3, 4).
//
//				for (i = 0; i < _dsBasis.Tables["Basis"].Rows.Count; i ++)
//				{
//					_dsBasis.Tables["Basis"].Rows[i]["BasisID"] = i;
//					_dsBasis.Tables["Basis"].Rows[i]["BasisName"] = "Basis " + (i + 1);
//				}
//
//				_dsBasis.AcceptChanges();
//
//				grdBasis.DataSource = _dsBasis;
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
		
		private void grdBasis_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Infragistics.Win.UIElement element;
			Point point;
			UltraGridRow row;

			try
			{
				//if the user clicked the right mouse, he/she probably wants to see the
				//context menu. We have only one item in the context menu: the "DELETE"
				//command. In order to delete a row, we must select the whole row first.

				if (e.Button == MouseButtons.Left)
				{
					return;
				}

				//get the row the mouse is on.
				//Get the GUI element where the mouse cursor is. (so that later on
				//we can retrieve the row and the cell based on the mouse location.)

				point = new Point(e.X, e.Y);
				element = grdBasis.DisplayLayout.UIElement.ElementFromPoint(point);

				if (element == null) 
				{					
					return;
				}

				//Retrieve the row where the mouse is

				row = (UltraGridRow)element.GetContext(typeof(UltraGridRow)); 

				if (row == null) 
				{
					return;
				}
			
				row.Selected = true;
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void grdBasis_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
			bool HasBand0Rows = false;
			bool HasBand1Rows = false;

			try
			{
				//Validate: If the selected rows collection consists of rows from 
				//both the parent table AND the child table, unselect all rows
				//but don't display any error messages (it doesn't appear when 
				//you would expect it).
			
				if (grdBasis.Selected.Rows.Count >= 2)
				{
					//we're only interested if a row is selected. (not cells, not columns)

					foreach (UltraGridRow row in grdBasis.Selected.Rows)
					{
						if (row.Band.Index == 0)
						{
							HasBand0Rows = true;
						}
						else if (row.Band.Index == 1)
						{
							HasBand1Rows = true;
						}
					}

					if (HasBand0Rows == true && HasBand1Rows == true)
					{
						foreach (UltraGridRow row in grdBasis.Selected.Rows)
						{
							row.Selected = false;
						}
					}
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		/// <summary>
		/// Get the cell where the mouse is. If (and only if) the cell is in 
		/// Band[0] (details grid) and the column is "Description", 
		/// set the effect to ALL.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		private void grdBasis_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
		}

		private void grdBasis_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
		}
		
		private void grdBasis_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			CalendarDateSelector frmCalDtSelector;
			DialogResult dateRangeResult;
			DateRangeProfile selectedDateRange;

			try
			{
				switch (e.Cell.Column.Key)
				{
					case "DateRange":

						frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});

						if (e.Cell.Row.Cells["DateRange"].Value != null &&
							e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
							e.Cell.Row.Cells["DateRange"].Text.Length > 0)
						{
							frmCalDtSelector.DateRangeRID = Convert.ToInt32(e.Cell.Row.Cells["DateRangeID"].Value, CultureInfo.CurrentUICulture);
						}

						if (mdsPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
						{
							frmCalDtSelector.AnchorDateRangeRID = mdsPlanDateRange.DateRangeRID;
							frmCalDtSelector.AllowDynamicToPlan = true;  // Issue 4605 stodd 9.28.2007
							frmCalDtSelector.AnchorDateRelativeTo = eDateRangeRelativeTo.Plan;  // Issue 4605 stodd 9.28.2007
						}
						else
						{
							frmCalDtSelector.AnchorDate = SAB.ClientServerSession.Calendar.CurrentDate;
							frmCalDtSelector.AllowDynamicToPlan = false;  // Issue 4605 stodd 9.28.2007
						}

						frmCalDtSelector.AllowDynamicToStoreOpen = false;

						dateRangeResult = frmCalDtSelector.ShowDialog();

						if (dateRangeResult == DialogResult.OK)
						{
							selectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
							
							if (selectedDateRange.Key != Include.UndefinedCalendarDateRange)  // Issue 4791 stodd 10.11.2007
								SetBasisTimeLength(selectedDateRange);

							e.Cell.Value = selectedDateRange.DisplayDate;
							e.Cell.Row.Cells["DateRangeID"].Value = selectedDateRange.Key;

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
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);


						break;

					case "IncludeButton":

						if (Convert.ToBoolean(e.Cell.Row.Cells["IsIncluded"].Value, CultureInfo.CurrentUICulture))
						{
							e.Cell.Row.Cells["IsIncluded"].Value = false;
							e.Cell.Appearance.Image = _picExclude;
						}
						else
						{
							e.Cell.Row.Cells["IsIncluded"].Value = true;
							e.Cell.Appearance.Image = _picInclude;
						}

						e.Cell.CancelUpdate();
						grdBasis.PerformAction(UltraGridAction.DeactivateCell);

						break;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		#endregion

		private void SetPlanTimeLength (DateRangeProfile aDateRangeProfile)
		{
			try
			{
				ProfileList timeList = null;
				if (aDateRangeProfile.SelectedDateType == eCalendarDateType.Period)
				{
					timeList = (ProfileList)SAB.ClientServerSession.Calendar.GetDateRangePeriods(aDateRangeProfile, null);
				}
				else
				{
					timeList = (ProfileList)SAB.ClientServerSession.Calendar.GetDateRangeWeeks(aDateRangeProfile, null);
				}
				if (timeList != null)
				{
					_planTimeLength = timeList.Count;
				}
			}
			catch
			{
				throw;
			}
		}

		private void SetBasisTimeLength (DateRangeProfile aDateRangeProfile)
		{
			try
			{
				ProfileList timeList = null;
				if (aDateRangeProfile.SelectedDateType == eCalendarDateType.Period)
				{
					timeList = (ProfileList)SAB.ClientServerSession.Calendar.GetDateRangePeriods(aDateRangeProfile, null);
				}
				else
				{
					timeList = (ProfileList)SAB.ClientServerSession.Calendar.GetDateRangeWeeks(aDateRangeProfile, null);
				}
				if (timeList != null)
				{
					_basisTimeLength = timeList.Count;
				}
			}
			catch
			{
				throw;
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
                GetOTSPLANWorkflows(_OTSForecastBalanceMethod.Key, ugWorkflows);
			}
			catch(Exception ex)
			{
				HandleException(ex, "LoadWorkflows");
			}
		}

		#endregion

		protected override void Call_btnProcess_Click()
		{
			try
			{
//#if (DEBUG)
				if (ValidateAdditionalFieldsForProcess())
				{
					ProcessAction(eMethodType.ForecastBalance);

					if (!this.ErrorFound)
					{
						_OTSForecastBalanceMethod.Method_Change_Type = eChangeType.update;
						btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
					}
				}
//#else
//				MessageBox.Show("Process not ready");
//#endif

			}
			catch(Exception ex)
			{
				HandleException(ex, "Call_btnProcess_Click");
			}

		}
		// End MID Track 4858

		private bool ValidateAdditionalFieldsForProcess()
		{
			try
			{
				return true;
			}
			catch(Exception ex)
			{
				string message = ex.ToString();
				throw;
			}
		}

		// Begin MID Track 4858 - JSmith - Security changes
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				this.Save_Click(false);
//
//				if (!ErrorFound)
//				{
//					// Now that this one has been saved, it should be changed to update.
//					_OTSForecastBalanceMethod.Method_Change_Type = eChangeType.update;
//					btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Update);
//				}
//			}
//			catch(Exception err)
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
//			catch(Exception err)
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

		private void PopulateLowLevels(HierarchyNodeProfile aHierarchyNodeProfile, ComboBox aComboBox)
		{
			try
			{
				HierarchyProfile hierProf;
				aComboBox.Items.Clear();
				if (aHierarchyNodeProfile == null ||
					LockStatus == eLockStatus.ReadOnly)
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
						for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
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
						HierarchyProfile mainHierProf = SAB.HierarchyServerSession.GetMainHierarchyData();

						int highestGuestLevel = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);

						// add guest levels to comboBox
						if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
						{
							for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
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
									HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
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
                        //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                        //int longestBranchCount = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                        DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                        int longestBranchCount = hierarchyLevels.Rows.Count - 1;
                        //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
						int offset = 0;
						for (int i = 0; i < longestBranchCount; i++)
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
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		#region TextBox Events

		private void txtNode_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Image_DragEnter(sender, e);
		}

		private void txtNode_DragOver(object sender, DragEventArgs e)
		{

        //TT#695  - Begin - MD - RBeck - Drag and drop of size merchandise causes error
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

		private void txtNode_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
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

                    _OTSForecastBalanceMethod.HnRID = hnp.Key;
                    PopulateLowLevels(hnp, cboLowLevels.ComboBox);
                    _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858

//                // Create a new instance of the DataObject interface.

//                data = Clipboard.GetDataObject();

//                //If the data is ClipboardProfile, then retrieve the data
				
//                if (data.GetDataPresent(ClipboardProfile.Format.Name))
//                {
//                    cbp = (ClipboardProfile)data.GetData(ClipboardProfile.Format.Name);

//                    if (cbp.ClipboardDataType == eClipboardDataType.HierarchyNode)
//                    {
//                        if (cbp.ClipboardData.GetType() == typeof(HierarchyClipboardData))
//                        {
//                            MIDTreeNode_cbd = (HierarchyClipboardData)cbp.ClipboardData;
//                            //Begin Track #5378 - color and size not qualified
////							HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
//                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key, false, true);
//                            //End Track #5378

//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            ((TextBox)sender).Text = hnp.Text;
//                            ((MIDTag)(((TextBox)sender).Tag)).MIDTagData = hnp;
//                            //End Track #5858

//                            _OTSForecastBalanceMethod.HnRID = cbp.Key;
//                            PopulateLowLevels(hnp, cboLowLevels);
//                            _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();

//                            // JBolles - MID Track #5020
//                            ErrorProvider.SetError(txtHighLevelNode, string.Empty);

//                            ApplySecurity();

//                            ////Begin Track #5858 - JSmith - Validating store security only
//                            //base.ValidatePlanNodeSecurity((TextBox)sender, false, eSecurityTypes.Store);
//                            ////End Track #5858

//                            _textChanged = false;
//                            _priorError = false;
//                        }
//                        else
//                        {
//                            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard));
//                        }
//                    }
//                    else
//                    {
//                        MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
//                    }
//                }
//                else
//                {
//                    MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNodeToDrop));
//                }
			}
            catch (BadDataInClipboardException)
            {
                MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void txtNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		#endregion

		#region MIDDateRangeSelector Events

		private void mdsPlanDateRange_Click(object sender, System.EventArgs e)
		{
			try
			{
				CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
				mdsPlanDateRange.DateRangeForm = frm;
				frm.DateRangeRID = mdsPlanDateRange.DateRangeRID;
				frm.AllowDynamicToPlan = false;
				frm.AllowDynamicToStoreOpen = false;
				frm.AllowDynamicSwitch = true;
				mdsPlanDateRange.ShowSelector();
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}

		private void mdsPlanDateRange_OnSelection(object source, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
		{
			try
			{
				if (e.SelectedDateRange != null)
				{
					LoadDateRangeSelector(mdsPlanDateRange, e.SelectedDateRange);
					SetBasisDates();
					grdBasis.Refresh();

					if (e.SelectedDateRange.Key != Include.UndefinedCalendarDateRange) // Issue 4605 stodd 9.28.2007
						SetPlanTimeLength(e.SelectedDateRange);
				}
			}
			catch (Exception exc)
			{
				HandleException(exc);
			}
		}
		
		#endregion
		
		private void SetBasisDates()
		{
			int i;
			int drID;
			DateRangeProfile drProf;
			string drText;

			try
			{
				for (i = 0; i < _dsBasis.Tables["BasisDetails"].Rows.Count; i++)
				{
					//Fill in the DateRange selector's display text.

					if (_dsBasis.Tables["BasisDetails"].Rows[i]["DateRangeID"] != System.DBNull.Value)
					{
						drID = Convert.ToInt32(_dsBasis.Tables["BasisDetails"].Rows[i]["DateRangeID"], CultureInfo.CurrentUICulture);

						if (drID != Include.UndefinedCalendarDateRange)
						{
							if (mdsPlanDateRange.DateRangeRID != Include.UndefinedCalendarDateRange)
							{
								drProf = SAB.ClientServerSession.Calendar.GetDateRange(drID, mdsPlanDateRange.DateRangeRID);
							}
							else
							{
								drProf = SAB.ClientServerSession.Calendar.GetDateRange(drID, SAB.ClientServerSession.Calendar.CurrentDate);
							}

							drText = SAB.ClientServerSession.Calendar.GetDisplayDate(drProf);

							_dsBasis.Tables["BasisDetails"].Rows[i]["DateRange"] = drText;

							SetBasisTimeLength(drProf);
						}
					}
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
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
				//=========================================================
				// Override the image if this is a dynamic switch date.
				//=========================================================
				if (aDateRangeProf.IsDynamicSwitch)
					aMIDDRS.SetImage(this.DynamicSwitchImage);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void btnVersionOverride_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (cboLowLevelsVersion.SelectedIndex == -1)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				if (cboLowLevels.SelectedIndex == -1)
				{
					MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				string lowLevelText = cboLowLevels.Items[cboLowLevels.SelectedIndex].ToString();
				// BEGIN Override Low Level 

                
				System.Windows.Forms.Form parentForm;
                
				parentForm = this.MdiParent;
                				
                object[] args = null;

                //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
				//System.Windows.Forms.Form frm;
                //end #tt700

				// Begin Track #5909 - stodd
				FunctionSecurityProfile methodSecurity;
				if (radGlobal.Checked)
					methodSecurity = GlobalSecurity;
				else
					methodSecurity = UserSecurity;
				args = new object[] { SAB, _OTSForecastBalanceMethod.OverrideLowLevelRid, _OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelVersionRID, lowLevelText, _OTSForecastBalanceMethod.CustomOLL_RID, methodSecurity };
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

                //end #700

				//_OTSForecastBalanceMethod.LowLevelsType = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelType;
				//_OTSForecastBalanceMethod.LowLevelsOffset = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelOffset;
				//_OTSForecastBalanceMethod.LowLevelsSequence = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelSequence;
				//_lowLevelVersionDefaultProfile = (VersionProfile)_storeLowLevelVersionList.FindKey(Convert.ToInt32(cboLowLevelsVersion.SelectedValue, CultureInfo.CurrentUICulture));
				//if (_OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Count == 0)
				//{
				//    PopulateVersionOverrideList();
				//}

				//// BEGIN Issue 4858 stodd 11.6.2007
				//foreach (LowLevelVersionOverrideProfile lvop in _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.ArrayList)
				//{
				//    if (lvop.VersionProfile == null)
				//    {
				//        if (_storeLowLevelVersionList.Contains((int)this.cboLowLevelsVersion.SelectedValue))
				//        {
				//            lvop.VersionProfile = (VersionProfile)_storeLowLevelVersionList.FindKey((int)this.cboLowLevelsVersion.SelectedValue);
				//        }
				//    }
				//    if (lvop.VersionProfile.StoreSecurity == null)
				//    {

				//        lvop.VersionProfile.StoreSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(lvop.VersionProfile.Key, (int)eSecurityTypes.Store);
				//    }
				//}
				//// END Issue 4858 stodd 11.6.2007

				//frmOverrideLowLevelVersions frmOverrideLowLevelVersions = new frmOverrideLowLevelVersions(SAB, _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList, _storeLowLevelVersionList, _lowLevelVersionDefaultProfile, false, true);
				//frmOverrideLowLevelVersions.ShowDialog();
				// BEGIN Issue 4858 stodd 11.6.2007
				// Begin Issue 4858
				//if (frmOverrideLowLevelVersions.ChangeMade)
				//{
				//    ChangePending = true;
				//    ValidateLowLevelSecurity();
				//}
				// END Issue 4858 stodd 11.6.2007
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
        
        //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
        System.Windows.Forms.Form _overrideLowLevelfrm;
        //End tt#700

		private void OnOverrideLowLevelCloseHandler(object source, OverrideLowLevelCloseEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (_OTSForecastBalanceMethod.OverrideLowLevelRid != e.aOllRid)
						ChangePending = true;
					_OTSForecastBalanceMethod.OverrideLowLevelRid = e.aOllRid;
					if (_OTSForecastBalanceMethod.CustomOLL_RID != e.aCustomOllRid)
					{
						_OTSForecastBalanceMethod.CustomOLL_RID = e.aCustomOllRid;
						UpdateMethodCustomOLLRid(_OTSForecastBalanceMethod.Key, _OTSForecastBalanceMethod.CustomOLL_RID);
					}

                    //Begin tt#700 - APicchetti - forecast methods show the method has changed when user copens Override Low Level model and then clicks close
                    if (_overrideLowLevelfrm.DialogResult != DialogResult.Cancel)
                    {
                        LoadOverrideModelComboBox(cboOverride.ComboBox, e.aOllRid, _OTSForecastBalanceMethod.CustomOLL_RID);
                    }

                    _overrideLowLevelfrm = null;
                    // End tt#700


					//LoadOverrideModelComboBox(cboOverride, e.aOllRid, _OTSForecastBalanceMethod.CustomOLL_RID);
				}
			}
			catch
			{
				throw;
			}

		}

		// BEGIN Issue 4858 stodd 11.15.2007
		private bool ValidateLowLevelSecurity()
		{
			bool isValid = true;
			bool canUpdate = true;
			if (_OTSForecastBalanceMethod.HnRID != Include.NoRID)
			{
				canUpdate = ABM.AuthorizedToUpdate(this.SAB.ClientServerSession, this.SAB.ClientServerSession.UserRID);
				base.ApplyCanUpdate(canUpdate);
			}
			if (!canUpdate)
			{
				string errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_ErrorsInLowLevels);
				ErrorProvider.SetError (btnVersionOverride,errorMessage);
				isValid = false;
			}
			else
			{
				ErrorProvider.SetError (btnVersionOverride,string.Empty);
			}
			return isValid;
		}
		// END Issue 4858 stodd 11.15.2007

		// Begin MID Track #5209 - JSmith - Unauthorized message
		private void SetLowLevelOverrideSecurity()
		{
			foreach (LowLevelVersionOverrideProfile lvop in _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.ArrayList)
			{
				if (lvop.VersionProfile == null)
				{
					if (_storeLowLevelVersionList.Contains((int)this.cboLowLevelsVersion.SelectedValue))
					{
						lvop.VersionProfile = (VersionProfile)_storeLowLevelVersionList.FindKey((int)this.cboLowLevelsVersion.SelectedValue);
					}
				}

				lvop.VersionProfile.StoreSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(lvop.VersionProfile.Key, (int)eSecurityTypes.Store);

				lvop.NodeProfile.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.NodeProfile.Key, (int)eSecurityTypes.Store);
			}
		}
		// End MID Track #5209

		private void cboFilter_DropDown(object sender, System.EventArgs e)
		{
			FilterNameCombo holdFilter;

			try
			{
                holdFilter = (FilterNameCombo)((ComboBox)sender).SelectedItem;
				BindFilterComboBox();
                ((ComboBox)sender).SelectedItem = holdFilter;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void cboFilter_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
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

		override protected bool ApplySecurity()	// Track 5871 stodd
		{
            //Begin Track #5858 - JSmith - Validating store security only
            string errorMessage = string.Empty;
            string item = string.Empty;
            //End Track #5858

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

            // Begin MID Issue 2612 - stodd
            ErrorProvider.SetError(txtHighLevelNode, string.Empty);
            if (_OTSForecastBalanceMethod.HnRID != Include.NoRID)
            {
                _hierNodeSecurity = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(_OTSForecastBalanceMethod.HnRID, (int)eSecurityTypes.Store);
                if (!_hierNodeSecurity.AllowUpdate)
                {
                    btnProcess.Enabled = false;
                    //Begin Track #5858 - JSmith - Validating store security only
                    btnSave.Enabled = false;
                    securityOk = false;
                    errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedToPlan);
                    item = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                    errorMessage = errorMessage + item + ".";
                    ErrorProvider.SetError(txtHighLevelNode, errorMessage);
                    //End Track #5858
                }
            }
            // End issue 2612

            // Begin Track #5937 - JSmith - Null reference exception
            if (!ValidateStorePlanVersionSecurity(cboLowLevelsVersion.ComboBox))
            {
                securityOk = false;
            }

            if (securityOk)
                securityOk = (((MIDControlTag)(txtHighLevelNode.Tag)).IsAuthorized(eSecurityTypes.Store, eSecuritySelectType.Update));

            if (!ValidateLowLevelSecurity())
            {
                securityOk = false;
            }
            // End Track #5937

            return securityOk;	// track 5871 stodd
		}

//        private void PopulateVersionOverrideList()
//        {
//            try
//            {
//                HierarchyNodeList hnl = null;
//                if (_OTSForecastBalanceMethod.LowLevelsType == eLowLevelsType.LevelOffset)
//                {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////					hnl = _SAB.HierarchyServerSession.GetDescendantData(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsOffset, true);
//                    //Begin Track #5378 - color and size not qualified
////					hnl = SAB.HierarchyServerSession.GetDescendantData(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsOffset, true, eNodeSelectType.NoVirtual);
//                    hnl = SAB.HierarchyServerSession.GetDescendantData(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsOffset, true, eNodeSelectType.NoVirtual, true);
//                    //End Track #5378
////End Track #4037
//                }
//                else
//                {
////Begin Track #4037 - JSmith - Optionally include dummy color in child list
////					hnl = _SAB.HierarchyServerSession.GetDescendantDataByLevel(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsSequence, true);
//                    //Begin Track #5378 - color and size not qualified
////					hnl = SAB.HierarchyServerSession.GetDescendantDataByLevel(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsSequence, true, eNodeSelectType.NoVirtual);
//                    hnl = SAB.HierarchyServerSession.GetDescendantDataByLevel(_OTSForecastBalanceMethod.HnRID, _OTSForecastBalanceMethod.LowLevelsSequence, true, eNodeSelectType.NoVirtual, true);
//                    //End Track #5378
////End Track #4037
//                }

//                foreach (HierarchyNodeProfile hnp in hnl)
//                {
//                    LowLevelVersionOverrideProfile lvop =  new LowLevelVersionOverrideProfile(hnp.Key);
//                    lvop.VersionIsOverridden = false;
//                    lvop.VersionProfile = null;
//                    lvop.Exclude = false;
//                    lvop.NodeProfile = hnp;
//                    lvop.NodeProfile.StoreSecurityProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(hnp.Key, (int)eSecurityTypes.Store);

//                    _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Add(lvop);

					
//                }
//            }
//            catch
//            {
//                throw;
//            }
//        }

		#region WorkflowMethodFormBase Overrides 

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
			}
			catch (Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to set the specific fields in method object before updating
		/// </summary>
		override protected void SetSpecificFields()
		{
			try
			{
                //Begin Track #5858 - KJohnson - Validating store security only
                _OTSForecastBalanceMethod.HnRID = ((HierarchyNodeProfile)((MIDTag)(txtHighLevelNode.Tag)).MIDTagData).Key;
                //End Track #5858
				_OTSForecastBalanceMethod.HighLevelVersionRID = Convert.ToInt32(cboHighLevelVersion.SelectedValue, CultureInfo.CurrentUICulture);
				_OTSForecastBalanceMethod.CDR_RID = mdsPlanDateRange.DateRangeRID;
				_OTSForecastBalanceMethod.LowLevelVersionRID = Convert.ToInt32(cboLowLevelsVersion.SelectedValue, CultureInfo.CurrentUICulture);
				
				if (cboFilter.SelectedIndex != -1)
				{
					_OTSForecastBalanceMethod.FilterRID = ((FilterNameCombo)cboFilter.SelectedItem).FilterRID;
				}
				else
				{
					_OTSForecastBalanceMethod.FilterRID = -1;
				}

				_OTSForecastBalanceMethod.VariableNumber = ((VariableCombo)cboVariable.SelectedItem).VariableNumber;

				if (cboLowLevels.SelectedIndex != -1)
				{
					_OTSForecastBalanceMethod.LowLevelsType = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelType;
					_OTSForecastBalanceMethod.LowLevelsOffset = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelOffset;
					_OTSForecastBalanceMethod.LowLevelsSequence = ((LowLevelCombo)cboLowLevels.SelectedItem).LowLevelSequence;
				}
				else
				{
					_OTSForecastBalanceMethod.LowLevelsType = eLowLevelsType.None;
					_OTSForecastBalanceMethod.LowLevelsOffset = 0;
					_OTSForecastBalanceMethod.LowLevelsSequence = 0;
				}

				_OTSForecastBalanceMethod.IneligibleStoresInd = chkIneligibleStores.Checked; //optIneligibleStoresYes.Checked;
				_OTSForecastBalanceMethod.SimilarStoresInd = chkSimilarStores.Checked; //optSimilarStoresYes.Checked;

				if (radBalanceToChain.Checked == true)
				{
					_OTSForecastBalanceMethod.BalanceMode = eBalanceMode.Chain;
				}
				else
				{
					_OTSForecastBalanceMethod.BalanceMode = eBalanceMode.Store;
				}

				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				if (cboForecastBalanceName.SelectedItem != null) 
				{
					_OTSForecastBalanceMethod.Model_RID = ((ModelNameCombo)cboForecastBalanceName.SelectedItem).ModelRID;
				} 
				else 
				{
					_OTSForecastBalanceMethod.Model_RID = Include.NoRID;
				}

				if (radMatrixBalance.Checked == true)
				{
					_OTSForecastBalanceMethod.Matrix_Type = eMatrixType.Balance;
				}
				else
				{
					_OTSForecastBalanceMethod.Matrix_Type = eMatrixType.Forecast;
				}
				// END MID Track #5647

				if (cboIterations.SelectedIndex != -1)
				{
					_OTSForecastBalanceMethod.IterationType = ((IterationsCombo)cboIterations.SelectedItem).IterationType;
					_OTSForecastBalanceMethod.IterationsCount = ((IterationsCombo)cboIterations.SelectedItem).IterationCount;
				}
				else
				{
//					_OTSForecastBalanceMethod.IterationType = eIterationType.UseBase;
//					_OTSForecastBalanceMethod.IterationCount = 0;
					_OTSForecastBalanceMethod.IterationType = eIterationType.Custom;
					_OTSForecastBalanceMethod.IterationsCount = 3;
				}

				AddBasis();
			}
			catch
			{
				throw;
			}
		}

		private void AddBasis()
		{
            // Begin Track #5937 - JSmith - Null reference exception
            ForecastVersionProfileBuilder fvpb = null;
            // End Track #5937
			try
			{
				int i = 0;
				//Begin Track #5378 - color and size not qualified
//				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastBalanceMethod.HnRID);
                HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(_OTSForecastBalanceMethod.HnRID, true, true);
				//End Track #5378
				_OTSForecastBalanceMethod.BasisProfile.BasisDetailProfileList.Clear();
				BasisDetailProfile basisDetailProfile;
				ProfileList versionProfList = base.GetForecastVersionList(eSecuritySelectType.View | eSecuritySelectType.Update, eSecurityTypes.Store);  // Issue 4858 & 5871
				foreach (DataRow dr in _dsBasis.Tables["BasisDetails"].Rows)
				{
					basisDetailProfile = new BasisDetailProfile(i + 1, null);
					basisDetailProfile.VersionProfile = (VersionProfile)versionProfList.FindKey((Convert.ToInt32(dr["VersionID"], CultureInfo.CurrentUICulture)));
                    // Begin Track #5937 - JSmith - Null reference exception
                    if (basisDetailProfile.VersionProfile == null)
                    {
                        if (fvpb == null)
                        {
                            fvpb = new ForecastVersionProfileBuilder();
                        }
                        basisDetailProfile.VersionProfile = fvpb.Build((Convert.ToInt32(dr["VersionID"], CultureInfo.CurrentUICulture)));
                    }
                    // End Track #5937
					basisDetailProfile.HierarchyNodeProfile = hnp;
					basisDetailProfile.DateRangeProfile = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["DateRangeID"], CultureInfo.CurrentUICulture));
					basisDetailProfile.DateRangeProfile.DisplayDate = Convert.ToString(dr["DateRange"], CultureInfo.CurrentUICulture);
					basisDetailProfile.DateRangeProfile.Name = "Basis Total";
					if (Convert.ToBoolean(dr["IsIncluded"], CultureInfo.CurrentUICulture) == true)
					{
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Include;
					}
					else
					{
						basisDetailProfile.IncludeExclude = eBasisIncludeExclude.Exclude;
					}
					if (dr["Weight"] == DBNull.Value)
					{
						basisDetailProfile.Weight = 1;
					}
					else
					{
						basisDetailProfile.Weight = Convert.ToSingle(dr["Weight"], CultureInfo.CurrentUICulture);
					}
					_OTSForecastBalanceMethod.BasisProfile.BasisDetailProfileList.Add(basisDetailProfile);					
					i++;
				}
			}
			catch
			{
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
				if (txtHighLevelNode.Text.Trim().Length == 0)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (txtHighLevelNode,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_StoreHierarchyNodeMissing));
				}
				else
				{
					ErrorProvider.SetError (txtHighLevelNode,string.Empty);
				}
        //Begin TT#736 - MD - Combobox causes NullReference - rbeck
                if (cboVariable.SelectedIndex == -1)
                {
                    methodFieldsValid = false;
                    ErrorProvider.SetError(cboVariable, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VExpected));
                }
                else
                {
                    ErrorProvider.SetError(cboVariable, string.Empty);
                }
        //End TT#736 - MD - Combobox causes NullReference - rbeck
				if (cboHighLevelVersion.SelectedIndex == -1)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboHighLevelVersion,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_StoreVersionMissing));
				}
				else
				{
					ErrorProvider.SetError (cboHighLevelVersion,string.Empty);
				}

				if (_dsBasis.Tables["BasisDetails"].Rows.Count == 0)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (grdBasis,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_MatrixMethodBasisRequired));
				}
				else
				{
					foreach (DataRow dr in _dsBasis.Tables["BasisDetails"].Rows)
					{
						if (dr["VersionID"] == DBNull.Value)
						{
							methodFieldsValid = false;
							ErrorProvider.SetError (grdBasis,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VersionRequired));
						}
						else
						{
							if (dr["DateRange"] == DBNull.Value || (string)dr["DateRange"] == string.Empty)  // Issue 4791 stodd 10.11.2007
							{
								methodFieldsValid = false;
								ErrorProvider.SetError (grdBasis,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DateRequired));
							}
							else
							{
								ErrorProvider.SetError (grdBasis,string.Empty);
							}
						}
					}
				}

				if (mdsPlanDateRange.DateRangeRID == Include.UndefinedCalendarDateRange)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (mdsPlanDateRange,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_PlanDateMissing));
				}
				else
				{
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					if (_dsBasis.Tables["BasisDetails"].Rows.Count > 0 && 
						_planTimeLength != _basisTimeLength && 
						!radMatrixForecast.Checked)
					{
						methodFieldsValid = false;
						ErrorProvider.SetError (mdsPlanDateRange,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_PlanBasisTimeNotSameLength));
					}
					else
					{
						ErrorProvider.SetError (mdsPlanDateRange,string.Empty);
					}
					// END MID Track #5647
				}

				if (cboLowLevelsVersion.SelectedIndex == -1)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboLowLevelsVersion,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_ChainVersionMissing));
				}
				else
				{
					ErrorProvider.SetError (cboLowLevelsVersion,string.Empty);
				}

				if (cboLowLevels.SelectedIndex == -1)
				{
					methodFieldsValid = false;
					ErrorProvider.SetError (cboLowLevels,SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_pl_LowLevelsNotDefined));
				}
				else
				{
					ErrorProvider.SetError (cboLowLevels,string.Empty);
				}

                // Begin Track #5937 - JSmith - Null reference exception
                //// BEGIN Issue 4858
                ////Begin Track #5858 - JSmith - Validating store security only
                ////if (!ValidatePlanVersionSecurity(cboLowLevelsVersion))
                //if (!ValidateStorePlanVersionSecurity(cboLowLevelsVersion))
                ////End Track #5858
                //{
                //    methodFieldsValid = false;
                //}
                //if (!ValidateLowLevelSecurity())
                //{
                //    methodFieldsValid = false;
                //}
                //// END Issue 4858
                // End Track #5937

				return methodFieldsValid;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				return methodFieldsValid;
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
					ErrorProvider.SetError (txtName,"");
				}
				if (!WorkflowMethodDescriptionValid)
				{
					ErrorProvider.SetError (txtDesc,WorkflowMethodDescriptionMessage);
				}
				else
				{
					ErrorProvider.SetError (txtDesc,"");
				}
				if (!UserGlobalValid)
				{
					ErrorProvider.SetError (pnlGlobalUser,UserGlobalMessage);
				}
				else
				{
					ErrorProvider.SetError (pnlGlobalUser,"");
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Use to set the specific method object before updating
		/// </summary>
		override protected void SetObject()
		{
			try
			{
				ABM = _OTSForecastBalanceMethod;
			}
			catch(Exception exception)
			{
				HandleException(exception);
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
		//				HandleException(ex);
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
				HandleException(ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void txtHighLevelNode_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			_textChanged = true;
		}

        private void txtHighLevelNode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
//            string errorMessage;

//            try
//            {
//                if (txtHighLevelNode.Text == string.Empty && ((MIDTag)(txtHighLevelNode.Tag)).MIDTagData != null)
//                {
//                    //Begin Track #5858 - KJohnson - Validating store security only
//                    txtHighLevelNode.Text = string.Empty;
//                    ((MIDTag)(txtHighLevelNode.Tag)).MIDTagData = null;
//                    //End Track #5858

//                    _OTSForecastBalanceMethod.HnRID = Include.NoRID;
//                    PopulateLowLevels(null, cboLowLevels);
//                    _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();
//                }
//                else
//                {
//                    if (_textChanged)
//                    {
//                        _textChanged = false;

//                        HierarchyNodeProfile hnp = GetNodeProfile(txtHighLevelNode.Text);
//                        if (hnp.Key == Include.NoRID)
//                        {
//                            _priorError = true;

//                            errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), txtHighLevelNode.Text);
//                            ErrorProvider.SetError(txtHighLevelNode, errorMessage);
//                            MessageBox.Show(errorMessage);

//                            e.Cancel = true;
//                        }
//                        else
//                        {
//                            _priorError = false;

//                            //Begin Track #5858 - KJohnson - Validating store security only
//                            txtHighLevelNode.Text = hnp.Text;
//                            ((MIDTag)(txtHighLevelNode.Tag)).MIDTagData = hnp;
//                            //End Track #5858

//                            _OTSForecastBalanceMethod.HnRID = hnp.Key;
//                            //if (!base.ValidatePlanNodeSecurity(txtHighLevelNode, true))
//                            //	e.Cancel = true;
//                            PopulateLowLevels(hnp, cboLowLevels);
//                            _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();
//                        }	
//                    }
//                    // JBolles - MID Track #5020 - Prevent errored textbox from resetting its value before error is corrected
////					else if (_priorError)
////					{
////						if (txtHighLevelNode.Tag == null)
////						{
////							txtHighLevelNode.Text = string.Empty;
////						}
////						else
////						{
////							txtHighLevelNode.Text = ((HierarchyNodeProfile)txtHighLevelNode.Tag).Text;
////						}
////					}
//                }
//            }
//            catch (Exception ex)
//            {
//                HandleException(ex);
//            }
        }
	
		private void txtHighLevelNode_Validated(object sender, System.EventArgs e)
		{
			try
			{
                _textChanged = false;
                _priorError = false;

                //Begin Track #5858 - KJohnson- Validating store security only
                if ((((TextBox)sender).Text.Trim() == string.Empty) && (((TextBox)sender).Tag != null))
                {
                    _OTSForecastBalanceMethod.HnRID = Include.NoRID;
                    PopulateLowLevels(null, cboLowLevels.ComboBox);
                    _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();
                }
                else
                {
                    HierarchyNodeProfile hnp = (HierarchyNodeProfile)((MIDTag)((TextBox)sender).Tag).MIDTagData;
                    _nodeRID = hnp.Key;

                    _OTSForecastBalanceMethod.HnRID = hnp.Key;
                    //if (!base.ValidatePlanNodeSecurity(txtHighLevelNode, true))
                    //	e.Cancel = true;

                    PopulateLowLevels(hnp, cboLowLevels.ComboBox);
                    _OTSForecastBalanceMethod.LowLevelVersionOverrideProfileList.Clear();

                    ChangePending = true;
                    ApplySecurity();
                }
                //End Track #5858
			}
			catch (Exception)
			{
				throw;
			}
		}

		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string desc = string.Empty;
			try
			{
				string productID = aProductID.Trim();
				string[] pArray = productID.Split(new char[] {'['});
				productID = pArray[0].Trim(); 
//				return SAB.HierarchyServerSession.GetNodeData(productID);
				HierarchyMaintenance hm = new HierarchyMaintenance(SAB);
				EditMsgs em = new EditMsgs();
				return hm.NodeLookup(ref em, productID, false);
			}
			catch (Exception)
			{
				throw;
			}
		}

		private void grdBasis_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        private void cboOverride_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
			if (FormLoaded)
			{
				_OTSForecastBalanceMethod.OverrideLowLevelRid = ((ComboObject)cboOverride.SelectedItem).Key;
				ChangePending = true;
			}
        }

        private void cboFilter_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private void cbForecastBalanceName_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (cboForecastBalanceName.SelectedIndex <= 0)
				{
					_modelIndex = cboForecastBalanceName.SelectedIndex;
					_forecastBalanceProfile = (ForecastBalanceProfile)((ModelNameCombo)(cboForecastBalanceName.Items[cboForecastBalanceName.SelectedIndex])).Tag;

					//---Load Defaults Back In------------------------------
                    LoadDefaults();  // BEGIN MID Track #5906 - KJohnson - Defaults Not Loading Back In

					this.cboVariable.Enabled = true;
					this.radMatrixBalance.Enabled = true;
					this.radMatrixForecast.Enabled = true;
					if (radMatrixForecast.Checked)
					{
						this.cboIterations.Enabled = false;
						this.radBalanceToStore.Enabled = false;
						this.radBalanceToChain.Enabled = false;
					} 
					else 
					{
						this.cboIterations.Enabled = true;
						this.radBalanceToStore.Enabled = true;
						this.radBalanceToChain.Enabled = true;
					}
				} 
				else if (cboForecastBalanceName.SelectedIndex > 0)
				{
					_modelIndex = cboForecastBalanceName.SelectedIndex;
					_forecastBalanceProfile = (ForecastBalanceProfile)((ModelNameCombo)(cboForecastBalanceName.Items[cboForecastBalanceName.SelectedIndex])).Tag;
					LoadOptions(_forecastBalanceProfile);
					this.cboVariable.Enabled = false;
					this.radMatrixBalance.Enabled = false;
					this.radMatrixForecast.Enabled = false;
					this.cboIterations.Enabled = false;
					this.radBalanceToStore.Enabled = false;
					this.radBalanceToChain.Enabled = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}		
		}

		private void radMatrixBalance_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_setting_radMatrixBalance) 
			{
				if (cboForecastBalanceName.SelectedIndex <= 0)
				{
					this.cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(_OTSForecastBalanceMethod.IterationType, _OTSForecastBalanceMethod.IterationsCount));
					if (_OTSForecastBalanceMethod.BalanceMode == eBalanceMode.Chain)
					{
						radBalanceToChain.Checked = true;
					}
					else
					{
						radBalanceToStore.Checked = true;
					}
					this.cboIterations.Enabled = true;
					this.radBalanceToStore.Enabled = true;
					this.radBalanceToChain.Enabled = true;
				} 
				else
				{
					this.cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(_forecastBalanceProfile.IterationType, _forecastBalanceProfile.IterationCount));
					if (_forecastBalanceProfile.BalanceMode == eBalanceMode.Chain)
					{
						radBalanceToChain.Checked = true;
					}
					else
					{
						radBalanceToStore.Checked = true;
					}
				}
			}
		}

		private void radMatrixForecast_CheckedChanged(object sender, System.EventArgs e)
		{
			if (!_setting_radMatrixForecast) 
			{
				this.radBalanceToStore.Checked = true;
				this.cboIterations.SelectedIndex = 0;
				if (cboForecastBalanceName.SelectedIndex <= 0)
				{
					this.cboIterations.Enabled = false;
					this.radBalanceToStore.Enabled = false;
					this.radBalanceToChain.Enabled = false;
				}
			}
		}

		// END MID Track #5647

//		override public void ISaveAs()
//		{
//					
//		}

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

        private void cboForecastBalanceName_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbForecastBalanceName_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboVariable_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboVariable_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboIterations_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboIterations_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboLowLevels_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboLowLevels_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboLowLevelsVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboLowLevelsVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboHighLevelVersion_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboHighLevelVersion_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboOverride_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboOverride_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
