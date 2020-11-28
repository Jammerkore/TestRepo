using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmForecastBalModelMaint.
	/// </summary>
    public class frmForecastBalModelMaint : ModelFormBase
	{
        private DropIndicator _dropper;

//Begin  TT#1638 - Revised Model Save - RBeck
        //private bool _newModel = false;
        //private bool _changeMade = false;
        //private int _modelRID = -1;
        //private string _currModel = null;
        //private bool _modelLocked = false;
        //private int _modelIndex = 0;
        //private string _saveAsName = "";
//End   TT#1638 - Revised Model Save - RBeck

		private ForecastBalanceProfile _forecastBalanceProfile = null;
		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private ForecastBalanceProfile _old_forecastBalanceProfile;
		private bool _MIDOnlyFunctions = false;
		// END MID Track #5647
        private FunctionSecurityProfile BalModeModelSecurity;

		// drag drop fields
		// Properties
		protected DualListDragDropAction _action = DualListDragDropAction.Move;
		protected bool _showIndicator = true;
		protected Color _indicatorColor = Color.Red;

		// Extra Fields
		protected bool _mouseDown = false;
		protected int _indexOfItemUnderMouseToDrop;
		protected int _indexOfItemUnderMouseToDrag;
		protected Point _screenOffset;
		protected Rectangle _dragBoxFromMouseDown;
        //private DropIndicator _dropper;
		private System.Windows.Forms.GroupBox grpMatrixModel;
		private System.Windows.Forms.RadioButton radMatrixForecast;
		private System.Windows.Forms.RadioButton radMatrixBalance;
		private System.Windows.Forms.Label lblMatrix;
		private DateTime _nextScroll = DateTime.Now;

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private bool _clbVariablesChecked = false;
		private bool _setting_radMatrixBalance = false;
		private bool _setting_radMatrixForecast = false;
		// END MID Track #5647

		/// <summary>
		/// Returns/sets the action that is executed by the Drag and Drop
		/// </summary>
		[Category("Behavior")]
		[Description("The action that is executed by the Drag and Drop")]
		[DefaultValue(DualListDragDropAction.Move)]
		public DualListDragDropAction Action 
		{
			get { return _action; }
			set { _action = value; }
		}

		/// <summary>
		/// Returns/sets whether a drop indicator line is shown
		/// </summary>
		[Category("Behavior")]
		[Description("Determines whether a drop indicator line is shown.")]
		[DefaultValue(true)]
		public bool ShowDropIndicator 
		{
			get { return _showIndicator; }
			set { _showIndicator = value; }
		}

		/// <summary>
		/// Returns/sets the IndicatorColor used to display the indicator line in the 'To' ListBox.
		/// </summary>
		[Category("Appearance")]
		[Description("The IndicatorColor is used to display the indicator line in the 'To' ListBox.")]
		public Color IndicatorColor 
		{
			get { return _indicatorColor; }
			set { _indicatorColor = value; }
		}

		protected bool ShouldSerializeIndicatorColor() 
		{
			return IndicatorColor != Color.Red;
		}

		protected void ResetIndicatorColor() 
		{
			IndicatorColor = Color.Red;
		}
		
        //private System.Windows.Forms.Label lblModelName;
        //private System.Windows.Forms.ComboBox cbForecastBalanceName;
		private System.Windows.Forms.CheckedListBox clbVariables;
		private System.Windows.Forms.Label lblVariables;
		private System.Windows.Forms.Label lblIterations;
		private System.Windows.Forms.ComboBox cboIterations;
		private System.Windows.Forms.GroupBox grpBalanceMode;
		private System.Windows.Forms.RadioButton radBalanceToChain;
		private System.Windows.Forms.RadioButton radBalanceToStore;
		private System.Windows.Forms.Panel pnlData;
		private System.Windows.Forms.ComboBox cboComputationMode;
		private System.Windows.Forms.Label lblComputationMode;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public bool ChangeMade 
		{
			get { return _changeMade ; }
			set { _changeMade = value; }
		}

		public frmForecastBalModelMaint(SessionAddressBlock aSAB) : base (aSAB)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				_dropper = new DropIndicator();
				_dropper.Visible = false;

                LoadModelComboBox();        
				SetText();
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsForecastBalance);
                //BalModeModelSecurity = (FunctionSecurityProfile)FunctionSecurity.Clone();
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				SetMIDOnlyFlag();
                if (!_MIDOnlyFunctions)
                {
                    FunctionSecurity.SetReadOnly();
                }
                else
                {
                    FunctionSecurity.SetFullControl();
                }
                BalModeModelSecurity = (FunctionSecurityProfile)FunctionSecurity.Clone();
				// END MID Track #5647
				DetermineSecurity();
				pnlData.Enabled = false;
//Begin Track #3707 - JScott - Null Reference on model save
				btnSave.Enabled = false;
//End Track #3707 - JScott - Null Reference on model save
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			try
			{
				if( disposing )
				{
					if(components != null)
					{
						components.Dispose();
					}
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                    //this.cbModelName.SelectedIndexChanged -= new System.EventHandler(this.cbModelName_SelectedIndexChanged);
                    //this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
                    //this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
                    //this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
                    //this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
					this.clbVariables.DragLeave -= new System.EventHandler(this.clbVariables_DragLeave);
					this.clbVariables.ItemCheck -= new System.Windows.Forms.ItemCheckEventHandler(this.clbVariables_ItemCheck);
					this.cboIterations.SelectedIndexChanged -= new System.EventHandler(this.cboIterations_SelectedIndexChanged);
					this.radBalanceToChain.CheckedChanged -= new System.EventHandler(this.rad_CheckedChanged);
					this.radBalanceToStore.CheckedChanged -= new System.EventHandler(this.rad_CheckedChanged);
					this.Load -= new System.EventHandler(this.frmForecastBalModelMaint_Load);
					this.radMatrixBalance.CheckedChanged -= new System.EventHandler(this.radMatrixBalance_CheckedChanged);
					this.radMatrixForecast.CheckedChanged -= new System.EventHandler(this.radMatrixForecast_CheckedChanged);
					this.cboComputationMode.SelectedIndexChanged -= new System.EventHandler(this.cboComputationMode_SelectedIndexChanged);
					this.cbModelName.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cbModelName_KeyPress);
					// END MID Track #5647
				}
				
				base.Dispose( disposing );
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

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
            this.clbVariables = new System.Windows.Forms.CheckedListBox();
            this.lblVariables = new System.Windows.Forms.Label();
            this.lblIterations = new System.Windows.Forms.Label();
            this.cboIterations = new System.Windows.Forms.ComboBox();
            this.grpBalanceMode = new System.Windows.Forms.GroupBox();
            this.radBalanceToChain = new System.Windows.Forms.RadioButton();
            this.radBalanceToStore = new System.Windows.Forms.RadioButton();
            this.pnlData = new System.Windows.Forms.Panel();
            this.grpMatrixModel = new System.Windows.Forms.GroupBox();
            this.lblMatrix = new System.Windows.Forms.Label();
            this.radMatrixForecast = new System.Windows.Forms.RadioButton();
            this.radMatrixBalance = new System.Windows.Forms.RadioButton();
            this.cboComputationMode = new System.Windows.Forms.ComboBox();
            this.lblComputationMode = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.grpBalanceMode.SuspendLayout();
            this.pnlData.SuspendLayout();
            this.grpMatrixModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(297, 16);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(540, 16);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(459, 16);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Location = new System.Drawing.Point(378, 16);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(528, 437);
            // 
            // cbModelName
            // 
            this.cbModelName.Location = new System.Drawing.Point(114, 16);
            this.cbModelName.Size = new System.Drawing.Size(175, 21);
            this.cbModelName.TabIndex = 1;
            this.cbModelName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbModelName_KeyPress);
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(13, 16);
            this.lblModelName.Size = new System.Drawing.Size(80, 23);
            this.lblModelName.TabIndex = 0;
            this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ugModel
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugModel.DisplayLayout.Appearance = appearance1;
            this.ugModel.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugModel.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugModel.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugModel.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugModel.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugModel.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugModel.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugModel.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugModel.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugModel.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugModel.Size = new System.Drawing.Size(601, 385);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // clbVariables
            // 
            this.clbVariables.AllowDrop = true;
            this.clbVariables.CheckOnClick = true;
            this.clbVariables.Location = new System.Drawing.Point(32, 48);
            this.clbVariables.Name = "clbVariables";
            this.clbVariables.Size = new System.Drawing.Size(160, 244);
            this.clbVariables.TabIndex = 14;
            this.clbVariables.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbVariables_ItemCheck);
            this.clbVariables.DragDrop += new System.Windows.Forms.DragEventHandler(this.clbVariables_DragDrop);
            this.clbVariables.DragEnter += new System.Windows.Forms.DragEventHandler(this.clbVariables_DragEnter);
            this.clbVariables.DragOver += new System.Windows.Forms.DragEventHandler(this.clbVariables_DragOver);
            this.clbVariables.DragLeave += new System.EventHandler(this.clbVariables_DragLeave);
            this.clbVariables.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.clbVariables_QueryContinueDrag);
            this.clbVariables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clbVariables_MouseDown);
            this.clbVariables.MouseMove += new System.Windows.Forms.MouseEventHandler(this.clbVariables_MouseMove);
            this.clbVariables.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clbVariables_MouseUp);
            // 
            // lblVariables
            // 
            this.lblVariables.Location = new System.Drawing.Point(24, 16);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(100, 23);
            this.lblVariables.TabIndex = 15;
            this.lblVariables.Text = "Variables";
            this.lblVariables.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblIterations
            // 
            this.lblIterations.Location = new System.Drawing.Point(224, 118);
            this.lblIterations.Name = "lblIterations";
            this.lblIterations.Size = new System.Drawing.Size(64, 23);
            this.lblIterations.TabIndex = 17;
            this.lblIterations.Text = "Iterations:";
            this.lblIterations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboIterations
            // 
            this.cboIterations.Location = new System.Drawing.Point(296, 118);
            this.cboIterations.Name = "cboIterations";
            this.cboIterations.Size = new System.Drawing.Size(96, 21);
            this.cboIterations.TabIndex = 18;
            this.cboIterations.SelectedIndexChanged += new System.EventHandler(this.cboIterations_SelectedIndexChanged);
            // 
            // grpBalanceMode
            // 
            this.grpBalanceMode.Controls.Add(this.radBalanceToChain);
            this.grpBalanceMode.Controls.Add(this.radBalanceToStore);
            this.grpBalanceMode.Location = new System.Drawing.Point(227, 159);
            this.grpBalanceMode.Name = "grpBalanceMode";
            this.grpBalanceMode.Size = new System.Drawing.Size(232, 48);
            this.grpBalanceMode.TabIndex = 16;
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
            this.radBalanceToChain.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // radBalanceToStore
            // 
            this.radBalanceToStore.Location = new System.Drawing.Point(16, 16);
            this.radBalanceToStore.Name = "radBalanceToStore";
            this.radBalanceToStore.Size = new System.Drawing.Size(104, 24);
            this.radBalanceToStore.TabIndex = 0;
            this.radBalanceToStore.Text = "Store";
            this.radBalanceToStore.CheckedChanged += new System.EventHandler(this.rad_CheckedChanged);
            // 
            // pnlData
            // 
            this.pnlData.Controls.Add(this.grpMatrixModel);
            this.pnlData.Controls.Add(this.cboComputationMode);
            this.pnlData.Controls.Add(this.lblComputationMode);
            this.pnlData.Controls.Add(this.clbVariables);
            this.pnlData.Controls.Add(this.lblVariables);
            this.pnlData.Controls.Add(this.lblIterations);
            this.pnlData.Controls.Add(this.cboIterations);
            this.pnlData.Controls.Add(this.grpBalanceMode);
            this.pnlData.Location = new System.Drawing.Point(24, 64);
            this.pnlData.Name = "pnlData";
            this.pnlData.Size = new System.Drawing.Size(600, 306);
            this.pnlData.TabIndex = 19;
            // 
            // grpMatrixModel
            // 
            this.grpMatrixModel.Controls.Add(this.lblMatrix);
            this.grpMatrixModel.Controls.Add(this.radMatrixForecast);
            this.grpMatrixModel.Controls.Add(this.radMatrixBalance);
            this.grpMatrixModel.Location = new System.Drawing.Point(227, 48);
            this.grpMatrixModel.Name = "grpMatrixModel";
            this.grpMatrixModel.Size = new System.Drawing.Size(248, 48);
            this.grpMatrixModel.TabIndex = 21;
            this.grpMatrixModel.TabStop = false;
            // 
            // lblMatrix
            // 
            this.lblMatrix.Location = new System.Drawing.Point(8, 21);
            this.lblMatrix.Name = "lblMatrix";
            this.lblMatrix.Size = new System.Drawing.Size(48, 16);
            this.lblMatrix.TabIndex = 18;
            this.lblMatrix.Text = "Matrix:";
            this.lblMatrix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radMatrixForecast
            // 
            this.radMatrixForecast.Location = new System.Drawing.Point(156, 21);
            this.radMatrixForecast.Name = "radMatrixForecast";
            this.radMatrixForecast.Size = new System.Drawing.Size(81, 16);
            this.radMatrixForecast.TabIndex = 1;
            this.radMatrixForecast.Text = "Forecast";
            this.radMatrixForecast.CheckedChanged += new System.EventHandler(this.radMatrixForecast_CheckedChanged);
            // 
            // radMatrixBalance
            // 
            this.radMatrixBalance.Location = new System.Drawing.Point(78, 21);
            this.radMatrixBalance.Name = "radMatrixBalance";
            this.radMatrixBalance.Size = new System.Drawing.Size(71, 16);
            this.radMatrixBalance.TabIndex = 0;
            this.radMatrixBalance.Text = "Balance";
            this.radMatrixBalance.CheckedChanged += new System.EventHandler(this.radMatrixBalance_CheckedChanged);
            // 
            // cboComputationMode
            // 
            this.cboComputationMode.Location = new System.Drawing.Point(336, 234);
            this.cboComputationMode.Name = "cboComputationMode";
            this.cboComputationMode.Size = new System.Drawing.Size(209, 21);
            this.cboComputationMode.TabIndex = 20;
            this.cboComputationMode.SelectedIndexChanged += new System.EventHandler(this.cboComputationMode_SelectedIndexChanged);
            // 
            // lblComputationMode
            // 
            this.lblComputationMode.Location = new System.Drawing.Point(224, 234);
            this.lblComputationMode.Name = "lblComputationMode";
            this.lblComputationMode.Size = new System.Drawing.Size(104, 23);
            this.lblComputationMode.TabIndex = 19;
            this.lblComputationMode.Text = "Computation Mode:";
            this.lblComputationMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmForecastBalModelMaint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(641, 459);
            this.Controls.Add(this.pnlData);
            this.Name = "frmForecastBalModelMaint";
            this.Text = "Forecast Balance Model";
            this.Load += new System.EventHandler(this.frmForecastBalModelMaint_Load);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.ugModel, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.pnlData, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.grpBalanceMode.ResumeLayout(false);
            this.pnlData.ResumeLayout(false);
            this.grpMatrixModel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmForecastBalModelMaint_Load(object sender, System.EventArgs e)
		{
            try
            {
				BuildIterationComboBox(_forecastBalanceProfile);
				BuildComputationModeComboBox();

                //// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                //if (!_MIDOnlyFunctions)
                //{
                //    //Turn off all controls (Read Only Mode)
                //    btnNew.Enabled = false;
                //}
                //// END MID Track #5647

#if (DEBUG)
                lblComputationMode.Visible = true;
                cboComputationMode.Visible = true;
#else
                lblComputationMode.Visible = false;
                cboComputationMode.Visible = false; 
#endif
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
		}

        override public void InitializeForm()
		{
			try
			{
				FormLoaded = false;
				btnSave.Enabled = false;
				btnDelete.Enabled = false;
                //btnSaveAs.Enabled = false;
				if (_modelLocked)
				{
					DequeueModel(_forecastBalanceProfile);
					_modelLocked = false;
				}

				_newModel = true;
				_modelRID = -1;
				_currModel = null;
				_modelLocked = false;
				_saveAsName = "";
				_forecastBalanceProfile = new ForecastBalanceProfile(Include.NoRID);
				BuildVariableListBox(_forecastBalanceProfile);
				cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(GetDefaultComputationsName()));
				if (cboComputationMode.SelectedIndex == -1)
				{
					cboComputationMode.SelectedIndex = 0;
				}

				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				cboIterations.SelectedIndex = 2;
				radBalanceToStore.Checked = true;
				_setting_radMatrixBalance = true;
				_setting_radMatrixForecast = true;
				radMatrixBalance.Checked = true;
				_setting_radMatrixBalance = false;
				_setting_radMatrixForecast = false;
				cboIterations.Enabled = true;
				radBalanceToStore.Enabled = true;
				radBalanceToChain.Enabled = true;
				// END MID Track #5647
				this.cbModelName.SelectedIndex = -1;
				this.cbModelName.Text = "(new model)";
				this.cbModelName.Enabled = false;
				Format_Title(eDataState.New, eMIDTextCode.frm_Forecast_Balance_Model, null);
				SetReadOnly(true);
				DetermineSecurity();
				pnlData.Enabled = true;
				FormLoaded = true;
				ChangePending = true;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

        //public bool InitializeForm(int aKey )
        //{
        //    try
        //    {
        //        return InitializeForm(new ForecastBalanceProfile(aKey));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        override public bool InitializeForm(string aID)
		{
			try
			{
				ForecastBalanceProfile fbp = new ForecastBalanceProfile(aID);
				fbp.ModelID = aID;
				return InitializeForm(fbp);
			}
			catch
			{
				throw;
			}
		}

		private bool InitializeForm(ForecastBalanceProfile aForecastBalanceProfile)
		{
			bool initializeSuccessful = true;
			bool displayModel = true;
			int modelIndex = -1;
            eLockStatus lockStatus = eLockStatus.Undefined;
			try
			{
				FormLoaded = false;
				// reset security for next model
				FunctionSecurity = (FunctionSecurityProfile)BalModeModelSecurity.Clone();
				btnSave.Enabled = false;
                //btnSaveAs.Enabled = true;
                btnDelete.Enabled = true;
				_newModel = false;
				if (aForecastBalanceProfile.ModelID == "")
				{
					btnDelete.Enabled = false;
					if (cbModelName.Items.Count > 0)
					{
						displayModel = false;
						this.cbModelName.Text = "(select model)";
					}
					else
					{
						_newModel = true;
					}
				}
				else
				{
					btnDelete.Enabled = true;
//					modelIndex = LocateModelIndex(aForecastBalanceProfile.ModelID);
					modelIndex = cbModelName.Items.IndexOf(new ModelNameCombo(aForecastBalanceProfile.Key, aForecastBalanceProfile.ModelID));
				}

				this.cbModelName.SelectedIndex = modelIndex;
				_saveAsName = aForecastBalanceProfile.ModelID;
				if (_modelLocked)
				{
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					if ((_old_forecastBalanceProfile != null) && (_old_forecastBalanceProfile.Key != Include.NoRID)) {
						DequeueModel(_old_forecastBalanceProfile);
						_old_forecastBalanceProfile = new ForecastBalanceProfile(Include.NoRID);
					}
					// END MID Track #5647
					_modelLocked = false;
				}
		
				if (displayModel)
				{
                    //if (!_newModel && _MIDOnlyFunctions)
                    if (!_newModel)
					{
						lockStatus = EnqueueModel(aForecastBalanceProfile);
						if (lockStatus == eLockStatus.ReadOnly)
						{
                            FunctionSecurity.SetReadOnly(); 
						}
						else
							if (lockStatus == eLockStatus.Cancel)
						{
                            if (aForecastBalanceProfile.ModelID == null)
                            {
                                FormLoaded = true;
                                return false;
                            }
                            else
                            {
                                this.cbModelName.SelectedValue = aForecastBalanceProfile.ModelID;
                            }
						}
					}
				
					if (initializeSuccessful)
					{
						eDataState dataState;
                        if (!FunctionSecurity.AllowUpdate)
                        {
                            _modelLocked = false;
                            dataState = eDataState.ReadOnly;
                        }
                        else
                        {
                            _modelLocked = true;
                            dataState = eDataState.Updatable;
                        }

						BuildVariableListBox(_forecastBalanceProfile);

						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						if (aForecastBalanceProfile.MatrixType == eMatrixType.Balance)
						{
							_setting_radMatrixBalance = true;
							_setting_radMatrixForecast = true;
							radMatrixBalance.Checked = true;
							_setting_radMatrixBalance = false;
							_setting_radMatrixForecast = false;
						}
						else
						{
							_setting_radMatrixBalance = true;
							_setting_radMatrixForecast = true;
							radMatrixForecast.Checked = true;
							_setting_radMatrixBalance = false;
							_setting_radMatrixForecast = false;
						}
						// END MID Track #5647

						if (aForecastBalanceProfile.BalanceMode == eBalanceMode.Chain)
						{
							radBalanceToChain.Checked = true;
						}
						else
						{
							radBalanceToStore.Checked = true;
						}
						cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(aForecastBalanceProfile.ComputationMode));
						cboIterations.SelectedIndex = cboIterations.Items.IndexOf(new IterationsCombo(aForecastBalanceProfile.IterationType, aForecastBalanceProfile.IterationCount));

						Format_Title(dataState, eMIDTextCode.frm_Forecast_Balance_Model, aForecastBalanceProfile.ModelID);
						_modelRID = aForecastBalanceProfile.Key;
						_currModel = aForecastBalanceProfile.ModelID;
						
						this.cbModelName.Enabled = true;
						SetReadOnly(FunctionSecurity.AllowUpdate);

						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						if (aForecastBalanceProfile.MatrixType == eMatrixType.Balance &&
                            lockStatus == eLockStatus.Locked)
						{
							this.cboIterations.Enabled = true;
							this.radBalanceToStore.Enabled = true;
							this.radBalanceToChain.Enabled = true;
						}
						else
						{
							this.cboIterations.Enabled = false;
							this.radBalanceToStore.Enabled = false;
							this.radBalanceToChain.Enabled = false;
						}
						// END MID Track #5647
  					}
				}
				else
				{
                    eDataState dataState;
                    if (!FunctionSecurity.AllowUpdate)
                    {
                        dataState = eDataState.ReadOnly;
                    }
                    else
                    {
                        dataState = eDataState.Updatable;
                    }
					Format_Title(dataState, eMIDTextCode.frm_Forecast_Balance_Model, "");
					SetReadOnly(FunctionSecurity.AllowUpdate);  

					if (aForecastBalanceProfile.ModelID == "")
					{
						btnDelete.Enabled = false;
						btnSave.Enabled = false;
                        //btnSaveAs.Enabled = false;
					}
				}
				DetermineSecurity();
				pnlData.Enabled = true;
				FormLoaded = true;

                //// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                //if (_MIDOnlyFunctions && displayModel)
                //{
                //    //Override everything and make all controls work
                //} 
                //else
                //{
                //    //Turn off all controls (Read Only Mode)
                //    btnSave.Enabled = false;
                //    btnNew.Enabled = false;
                //    btnDelete.Enabled = false;
                //    clbVariables.Enabled = false;
                //    radMatrixBalance.Enabled = false;
                //    radMatrixForecast.Enabled = false;
                //    cboIterations.Enabled = false;
                //    radBalanceToStore.Enabled = false;
                //    radBalanceToChain.Enabled = false;
                //    cboComputationMode.Enabled = false;
                //}
                //// END MID Track #5647

				ChangePending = false;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				initializeSuccessful = false;
			}
			return initializeSuccessful;
		}

		private void SetText()
		{
            //btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
            //btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
            //btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			Text = MIDText.GetTextOnly(eMIDTextCode.frm_Forecast_Balance_Model);
			lblComputationMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ComputationMode) + ":";
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			this.lblVariables.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable) + ":";
			this.lblMatrix.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Matrix) + ":";
			this.radMatrixBalance.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Balance);
			this.radMatrixForecast.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Forecast);
			this.lblIterations.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Iterations) + ":";
			this.grpBalanceMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_BalanceMode) + ":";
			this.radBalanceToStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeStore);
			this.radBalanceToChain.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanTypeChain);
			// END MID Track #5647
		}

		private void DetermineSecurity()
		{
			try
			{
				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                //if ((FunctionSecurity.AllowDelete || _MIDOnlyFunctions) &&
                if (FunctionSecurity.AllowDelete &&
					cbModelName.SelectedIndex > -1)
				{
					btnDelete.Enabled = true;
				}
				else
				{
					btnDelete.Enabled = false;
				}

                //if (FunctionSecurity.AllowUpdate || _MIDOnlyFunctions)
                if (FunctionSecurity.AllowUpdate)
				{
					btnNew.Enabled = true;
					btnSave.Enabled = true;
				}
				else
				{
					btnNew.Enabled = false;
					btnSave.Enabled = false;
				}
				// END MID Track #5647
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}


        //Begin TT#1638 - MD - RBeck -  Revised Model Save 
        protected override DataTable GetFilteredModels(string forecastBalanceModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                MerchandiseHierarchyData modelsData = new MerchandiseHierarchyData();

                return modelsData.GetFilteredForcastBalModels(forecastBalanceModelNameFilter, isCaseSensitive);
            }
            catch (Exception ex)
            {
                HandleException(ex, "GetFilteredModels");
                throw ex;
            }
        }


        override protected void LoadModelComboBox(ProfileList aModelProfileList)
        {
            ForecastBalanceProfileList forecastBalanceProfileList = new ForecastBalanceProfileList(true);
            _ModelProfileList = aModelProfileList;
            cbModelName.Items.Clear();

            foreach (ForecastBalanceProfile fbp in forecastBalanceProfileList.ArrayList)
            {
                foreach (ModelName modelName in _ModelProfileList.ArrayList)
                {
                    if (fbp.Key == modelName.Key)
                    {
                        cbModelName.Items.Add(
                            new ModelNameCombo(fbp.Key, fbp.ModelID, fbp));
                    }
                }

            } 
        }
//End   TT#1638 - MD - RBeck -  Revised Model Save 

        override protected void LoadModelComboBox()
        {
            ForecastBalanceProfileList forecastBalanceProfileList = new ForecastBalanceProfileList(true);
            cbModelName.Items.Clear();

            foreach (ForecastBalanceProfile fbp in forecastBalanceProfileList.ArrayList)
            {
                cbModelName.Items.Add(
                    new ModelNameCombo(fbp.Key, fbp.ModelID, fbp));
            }

            // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
            if (forecastBalanceProfileList.ArrayList.Count == 0)
            {
                cbModelName.Enabled = false;
            }
            // END MID Track #5647
        }

//		private int LocateModelIndex(string aModelName)
//		{
//			int modelIndex = -1;
//			int i = 0;
//
//			foreach (MIDListBoxItem mlbi in cbModelName.Items)
//			{
//				ForecastBalanceProfile fbp = (ForecastBalanceProfile)mlbi.Tag;
//				if (aModelName == fbp.ModelID)
//				{
//					modelIndex = i;
//					break;
//				}
//				++i;
//			}
//			return modelIndex;
//		}

		// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
		private void SetMIDOnlyFlag()
		{
			try
			{
                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //string MIDOnlyFunctionsStr = System.Configuration.ConfigurationSettings.AppSettings["MIDOnlyFunctions"];
                string MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
                // End TT#1054
 
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
                #if (DEBUG)
                    _MIDOnlyFunctions = true;
                #endif
            }
			catch
			{
				throw;
			}
		}
		// END MID Track #5647

		private void BuildIterationComboBox(ForecastBalanceProfile aForecastBalanceProfile)
		{
			try
			{
				cboIterations.Items.Clear();
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

		private void BuildVariableListBox(ForecastBalanceProfile aForecastBalanceProfile)
		{
			try
			{
				clbVariables.Items.Clear();
				ProfileList variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
				
				// add variables from model
				foreach (ModelVariableProfile mvp in aForecastBalanceProfile.Variables)
				{
					VariableProfile vp = (VariableProfile)variables.FindKey(mvp.Key);
					if (vp != null && vp.AllowForecastBalance)
					{
						clbVariables.Items.Add(new MIDListBoxItem(vp.Key, vp.VariableName, vp), true);
					}
				}

				// add additional variables
				foreach (VariableProfile vp in variables.ArrayList)
				{
					if (!aForecastBalanceProfile.Variables.Contains(vp.Key))
					{
						if (vp.AllowForecastBalance)
						{
							clbVariables.Items.Add(new MIDListBoxItem(vp.Key, vp.VariableName, vp), false);
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

		private void BuildComputationModeComboBox()
		{
			try
			{
				cboComputationMode.Items.Clear();
				foreach (string comp in ComputationModes)
				{
					cboComputationMode.Items.Add(new ComputationModeCombo(comp));
				}

				// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
				if (cboComputationMode.Items.Count > 1) 
				{
					cboComputationMode.Enabled = true;
				}
				else 
				{
					cboComputationMode.Enabled = false;
				}
				// END MID Track #5647
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

//Begin     TT#1638 - MD - Revised Model Save - RBeck
        //private void btnCancel_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        Cancel_Click();
        //    }
        //    catch( Exception exception )
        //    {
        //        HandleException(exception);
        //    }
        //}
//End      TT#1638 - MD - Revised Model Save - RBeck

		override protected bool SaveChanges()
		{
			try
			{
				bool saveAsCanceled = false;
				ErrorFound = false;

				ProfileList variables = SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList;
				_forecastBalanceProfile.Variables.Clear();
				for (int i = 0; i < this.clbVariables.Items.Count; i++)
				{
					MIDListBoxItem mlbi = (MIDListBoxItem)clbVariables.Items[i];
					if (clbVariables.GetItemCheckState(i) == CheckState.Checked)
					{
						ModelVariableProfile mvp = new ModelVariableProfile(mlbi.Key);
						mvp.IsSelected = true;
						mvp.VariableProfile = (VariableProfile)variables.FindKey(mlbi.Key);
						_forecastBalanceProfile.Variables.Add(mvp);
					}
				}

				if (_forecastBalanceProfile.Variables.Count == 0)
				{
					ErrorProvider.SetError(clbVariables, SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NeedAtLeastOneVariable));
					ErrorFound = true;
				}
				else
				{
					ErrorProvider.SetError(clbVariables, string.Empty);
				}
								
				if (!ErrorFound)
				{
					if (_newModel)
					{
						_forecastBalanceProfile.ModelChangeType = eChangeType.add;
					}
					else
					{
						_forecastBalanceProfile.ModelChangeType = eChangeType.update;
					}

					if (_newModel && (_saveAsName == ""))
					{
						bool continueSave = false;
						frmSaveAs formSaveAs = new frmSaveAs(SAB);
						formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                        if (PerformingSaveAs)
                        {
                            formSaveAs.SaveAsName = _currModel;
                        }
						while (!continueSave)
						{
							formSaveAs.ShowDialog(this);
							saveAsCanceled = formSaveAs.SaveCanceled;
							if (!saveAsCanceled)
							{
								ForecastBalanceProfile checkExists = new ForecastBalanceProfile(formSaveAs.SaveAsName);
								if (checkExists.Key == -1)
								{
									_saveAsName = formSaveAs.SaveAsName;
									continueSave = true;
								}
								else
								{
									if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
										MessageBoxButtons.YesNo, MessageBoxIcon.Question)
										== DialogResult.No) 
									{
										saveAsCanceled = true;
										continueSave = true;
									}
									else
									{
										saveAsCanceled = false;
										continueSave = true;
                                        PerformingSaveAs = false;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
										_forecastBalanceProfile.ModelChangeType = eChangeType.update;
										ForecastBalanceProfile fbp2 =  new ForecastBalanceProfile(formSaveAs.SaveAsName);
										_modelRID = fbp2.Key;
										_saveAsName = formSaveAs.SaveAsName;
									}
								}
							}
							else
							{
								continueSave = true;
							}
						}
					}

					if (!saveAsCanceled)
					{
						_forecastBalanceProfile.ModelID = _saveAsName;
						_forecastBalanceProfile.Key = _modelRID;

						IterationsCombo ic = (IterationsCombo)cboIterations.Items[cboIterations.SelectedIndex];
						_forecastBalanceProfile.IterationType = ic.IterationType;
						_forecastBalanceProfile.IterationCount = ic.IterationCount;

						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						if (radMatrixBalance.Checked)
						{
							_forecastBalanceProfile.MatrixType = eMatrixType.Balance;
						}
						else
						{
							_forecastBalanceProfile.MatrixType = eMatrixType.Forecast;
						}
						// END MID Track #5647

						if (radBalanceToChain.Checked)
						{
							_forecastBalanceProfile.BalanceMode = eBalanceMode.Chain;
						}
						else
						{
							_forecastBalanceProfile.BalanceMode = eBalanceMode.Store;
						}

						_forecastBalanceProfile.ComputationMode = cboComputationMode.Text;

						_modelRID = _forecastBalanceProfile.WriteProfile();
						_forecastBalanceProfile.Key = _modelRID;
						_changeMade = true;
						ChangePending = false;
						btnSave.Enabled = false;
						if (_newModel)
						{
							// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
							_old_forecastBalanceProfile = _forecastBalanceProfile;
							// END MID Track #5647
							FormLoaded = false;
                            LoadModelComboBox();
							InitializeForm(_forecastBalanceProfile);
							FormLoaded = true;
							Format_Title(eDataState.Updatable, eMIDTextCode.frm_Forecast_Balance_Model, _saveAsName);
							_newModel = false;
						}
					}
					else
					{
                        PerformingSaveAs = false;  // RO-4989 - Ability to Save As a duplicate Size Constraint name
						MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
							MessageBoxButtons.OK, MessageBoxIcon.Information);
//Begin Track #3707 - JScott - Null Reference on model save
//						ChangePending = false;
//						if (cbModelName.Items.Count > 0)
//						{
//							ForecastBalanceProfile fpb = (ForecastBalanceProfile)((MIDListBoxItem)cbForecastBalanceName.Items[0]).Tag;
//							InitializeForm(fpb);
//						}
//						else
//						{
//							InitializeForm();
//						}
//
//End Track #3707 - JScott - Null Reference on model save
					}
					this.cbModelName.Enabled = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}

			return true;
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				SaveChanges();
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private eLockStatus EnqueueModel(ForecastBalanceProfile aForecastBalanceProfile)
		{
			try
			{
				eLockStatus lockStatus = eLockStatus.Undefined;

				ModelEnqueue modelEnqueue = new ModelEnqueue(
					eModelType.ForecastBalance,
					aForecastBalanceProfile.Key,
					SAB.ClientServerSession.UserRID,
					SAB.ClientServerSession.ThreadID);

				try
				{
					modelEnqueue.EnqueueModel();
					lockStatus = eLockStatus.Locked;
				}
				catch (ModelConflictException)
				{
					// release enqueue write lock incase they sit on the read only screen
					string errMsg = "The following model(s) requested:" + System.Environment.NewLine;
					foreach (ModelConflict MCon in modelEnqueue.ConflictList)
					{
						errMsg += System.Environment.NewLine + "Model: " + aForecastBalanceProfile.ModelID + ", User: " + MCon.UserName;
					}
					errMsg += System.Environment.NewLine + System.Environment.NewLine;
					errMsg += "Do you wish to continue with the model as read-only?";
					
					if (MessageBox.Show (errMsg,  this.Text,
						MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.Yes) 
					{
						lockStatus = eLockStatus.ReadOnly;
					}
					else
					{
						lockStatus = eLockStatus.Cancel;
					}
				}
				return lockStatus;
			}
			catch
			{
				throw;
			}
		}

		private void DequeueModel(ForecastBalanceProfile aForecastBalanceProfile)
		{
			try
			{
				ModelEnqueue modelEnqueue = new ModelEnqueue(
					eModelType.ForecastBalance,
					aForecastBalanceProfile.Key,
					SAB.ClientServerSession.UserRID,
					SAB.ClientServerSession.ThreadID);

				try
				{
					modelEnqueue.DequeueModel();

				}
				catch 
				{
					throw;
				}
			}
			catch
			{
				throw;
			}
		}

		override protected void BeforeClosing()
		{
			try
			{
				
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		override protected void AfterClosing()
		{
			try
			{
				if (_modelLocked)
				{
					DequeueModel(_forecastBalanceProfile);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "frmForecastBalModelMaint.AfterClosing");
			}
		}

        //private void btnDelete_Click(object sender, System.EventArgs e)
        override protected void DeleteModel()
		{
			try
			{
				int currIndex = cbModelName.SelectedIndex;
				bool itemDeleted = false;
				if (_modelRID != Include.NoRID)
				{
					string text = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
					text =  text.Replace("{0}", this.Text);
					if (MessageBox.Show (text,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.Yes) 
					{
						ForecastBalanceProfile fbp = new ForecastBalanceProfile(_modelRID);
						fbp.ModelChangeType = eChangeType.delete;
						fbp.Key = _modelRID;
						fbp.WriteProfile();
						_changeMade = true;
						itemDeleted = true;
						FormLoaded = false;

                        LoadModelComboBox();
					}
				}

				if (itemDeleted)
				{
					if (cbModelName.Items.Count > 0)
					{
						int nextItem;
						if (currIndex >= cbModelName.Items.Count)
						{
							nextItem = cbModelName.Items.Count - 1;
						}
						else
						{
							nextItem = currIndex;
						}
						// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
						_old_forecastBalanceProfile = _forecastBalanceProfile;
						_forecastBalanceProfile = (ForecastBalanceProfile)((ModelNameCombo)cbModelName.Items[nextItem]).Tag;
						InitializeForm(_forecastBalanceProfile);
						// END MID Track #5647
					}
					else
					{
						InitializeForm();
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
				MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

        //private void btnNew_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        FormLoaded = false;
        //        CheckForPendingChanges();
        //        InitializeForm();
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}

        //private void cbModelName_SelectedIndexChanged(object sender, System.EventArgs e) //TT#1638 - MD - RBeck -  Revised Model Save
        protected override void cbModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					CheckForPendingChanges();
					if (cbModelName.SelectedIndex >= 0)
					{
                        _modelIndex = cbModelName.SelectedIndex;
                        _forecastBalanceProfile = (ForecastBalanceProfile)((ModelNameCombo)(cbModelName.Items[cbModelName.SelectedIndex])).Tag;
                        InitializeForm(_forecastBalanceProfile);
                        // BEGIN MID Track #5647 - KJohnson - Matrix Forecast
                        _old_forecastBalanceProfile = _forecastBalanceProfile;
                        // END MID Track #5647
  					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void clbVariables_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
					if (!_clbVariablesChecked) 
					{
						ChangePending = true;
						btnSave.Enabled = true;

						//---Make sure only one item is checked
						_clbVariablesChecked = true;
						for (int i = 0; i < this.clbVariables.Items.Count; i++)
						{
							this.clbVariables.SetItemChecked(i, false);
						}
						_clbVariablesChecked = false;
					}
					// END MID Track #5647
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void cboIterations_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void rad_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}


		#region IFormBase Members
//Begin     TT#1638 - Revised Model Save - RBeck
        //override public void ICut()
        //{
			
        //}

        //override public void ICopy()
        //{
			
        //}

        //override public void IPaste()
        //{
			
        //}	

        //override public void IClose()
        //{
        //    try
        //    {
        //        this.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }

        //}

        //override public void ISave()
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        SaveChanges();
        //    }		
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        //override public void ISaveAs()
        //{
        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        _newModel = true;
        //        _saveAsName = string.Empty;
        //        PerformingSaveAs = true;
        //        SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        this.Cursor = Cursors.Default;
        //    }
        //}

        //override public void IDelete()
        //{
        //    try
        //    {
        //        DeleteModel();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //override public void IRefresh()
        //{
			
        //}
//End      TT#1638 - Revised Model Save - RBeck		
		#endregion

		private void clbVariables_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try 
			{
				_indexOfItemUnderMouseToDrag = clbVariables.IndexFromPoint(e.X, e.Y);
				_mouseDown = _indexOfItemUnderMouseToDrag > -1;
				Size dragSize = SystemInformation.DragSize;

				// Create a rectangle using the DragSize, with the mouse position being
				// at the center of the rectangle.
				_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
					e.Y - (dragSize.Height / 2)), dragSize);
			} 
			catch 
			{
				_mouseDown = false;
			}
		}

		private void clbVariables_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_mouseDown && (e.Button & MouseButtons.Left) == MouseButtons.Left) 
			{

				if (_dragBoxFromMouseDown != Rectangle.Empty && !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
				{

					// Start D&D
					_screenOffset = SystemInformation.WorkingArea.Location;
					DragDropEffects dropEffect = DragDropEffects.None;
					try 
					{
						dropEffect = clbVariables.DoDragDrop(_indexOfItemUnderMouseToDrag, DragDropEffects.All | DragDropEffects.Link);
					} 
					catch (System.Exception) 
					{
						// ignore errors
					}

					_mouseDown = false;
				}
			}
		}

		private void clbVariables_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_mouseDown = false;
			_dropper.Visible = false;
		}

		private void clbVariables_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (_mouseDown) 
			{
				if (ShowDropIndicator) 
				{
					Control.ControlCollection controls = clbVariables.Parent.Controls;
					if (!controls.Contains(_dropper)) 
					{
						_dropper.Left = clbVariables.Left - 5;
						_dropper.Height = 1;
						_dropper.Width = clbVariables.Width + 10;
						_dropper.Enabled = false;
						_dropper.Top = -500;
						_dropper.Anchor = clbVariables.Anchor;
						_dropper.ForeColor = IndicatorColor;
						controls.Add(_dropper);
						controls.SetChildIndex(_dropper, 0);
					}
				}
			}
		}

		private void clbVariables_DragLeave(object sender, System.EventArgs e)
		{
			_dropper.Visible = false;
		}

		private void clbVariables_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (_mouseDown) 
			{

				// Cursor
				SetCursor(e);

				// Drop Indicator
				if (ShowDropIndicator) 
				{
					_dropper.Visible = true;
					_indexOfItemUnderMouseToDrop = clbVariables.IndexFromPoint(clbVariables.PointToClient(new Point(e.X, e.Y + 4)));
					int y = 1;
					if (_indexOfItemUnderMouseToDrop > -1) 
					{
						Rectangle rect = clbVariables.GetItemRectangle(_indexOfItemUnderMouseToDrop);
						y = rect.Top + 1;
						if (y == 1 && clbVariables.TopIndex > 0) 
						{
							// Scroll Up
							if (DateTime.Now > _nextScroll) 
							{
								clbVariables.TopIndex -= 1;
								_nextScroll = DateTime.Now.AddMilliseconds(100);
							} 
							return; // Exit!
						} 
						else 
						{
							if (rect.Bottom + 1 > clbVariables.DisplayRectangle.Height) 
							{
								// Scroll Down
								if (DateTime.Now > _nextScroll) 
								{
									clbVariables.TopIndex += 1;
									_nextScroll = DateTime.Now.AddMilliseconds(100);
								}
							}
						}
					} 
					else 
					{
						if (clbVariables.Items.Count > 0) 
						{
							// Scrolled to last item, select botom
							Rectangle rect = clbVariables.GetItemRectangle(clbVariables.Items.Count - 1);
							y = rect.Bottom + 1;
						} 
						else 
						{
							if (!clbVariables.DisplayRectangle.Contains(clbVariables.PointToClient(new Point(e.X, e.Y)))) 
							{
								return;
							}
						}
					}

					// Set the Top of the dropper
					_dropper.Top = clbVariables.Top + y + 1;
				}
			}
		}

		private void clbVariables_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			try
			{
				if (_mouseDown) 
				{
					_dropper.Visible = false;
					object item = clbVariables.Items[_indexOfItemUnderMouseToDrag];
				
					CheckState checkState = clbVariables.GetItemCheckState(_indexOfItemUnderMouseToDrag);
					// Insert the item.
					if (ShowDropIndicator && _indexOfItemUnderMouseToDrop != ListBox.NoMatches) 
					{
						clbVariables.Items.Insert(_indexOfItemUnderMouseToDrop, item);
						clbVariables.SelectedIndex = _indexOfItemUnderMouseToDrop;
					} 
					else 
					{
						clbVariables.SelectedIndex = clbVariables.Items.Add(item);
					}

					// Move position, not from listbox
                    //if (clbVariables == clbVariables) 
                    //{
						if (_indexOfItemUnderMouseToDrop != ListBox.NoMatches && 
							_indexOfItemUnderMouseToDrag > _indexOfItemUnderMouseToDrop) 
						{
							_indexOfItemUnderMouseToDrag++;
						}
						clbVariables.Items.RemoveAt(_indexOfItemUnderMouseToDrag);
						e.Effect = DragDropEffects.None; // Cancels RemoveAt() in MouseMove eventhandler
						bool itemChecked = false;
						if (checkState == CheckState.Checked)
						{
							itemChecked = true;
						}
						clbVariables.SetItemChecked(clbVariables.SelectedIndex, itemChecked);
                    //} 
                    //else 
                    //{
                    //    // Finished
                    //    if (e.Effect == DragDropEffects.Move) 
                    //    {
                    //        clbVariables.Items.RemoveAt(_indexOfItemUnderMouseToDrag);
                    //        if (clbVariables.Items.Count > _indexOfItemUnderMouseToDrag) 
                    //        {
                    //            clbVariables.SelectedIndex = _indexOfItemUnderMouseToDrag;
                    //        } 
                    //        else 
                    //        {
                    //            clbVariables.SelectedIndex = clbVariables.Items.Count - 1;
                    //        }
                    //    }
                    //}
					_mouseDown = false;
				}
			}
			catch
			{
				throw;
			}
		}

		private void clbVariables_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
		{
			try
			{
				ListBox lb = sender as ListBox;
				if (lb != null) 
				{

					Form f = lb.FindForm();

					// Cancel the drag if the mouse moves off the form. The screenOffset
					// takes into account any desktop bands that may be at the top or left
					// side of the screen.
					if (((Control.MousePosition.X - _screenOffset.X) < f.DesktopBounds.Left) || 
						((Control.MousePosition.X - _screenOffset.X) > f.DesktopBounds.Right) || 
						((Control.MousePosition.Y - _screenOffset.Y) < f.DesktopBounds.Top) || 
						((Control.MousePosition.Y - _screenOffset.Y) > f.DesktopBounds.Bottom)) 
					{

						// Cancel D&D
						_mouseDown = false;
						_dropper.Visible = false;
						e.Action = DragAction.Cancel;
					} 
					else 
					{
						//e.Action = DragAction.Continue;
					}
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Set the Cursor depending on the Action and the KeyState
		/// </summary>
		/// <param name="e"></param>
		private void SetCursor(DragEventArgs e) 
		{
			try
			{
				// Cursor
				if (Action == DualListDragDropAction.Move) 
				{
					e.Effect = DragDropEffects.Move;
				} 
				else 
				{
					if (Action == DualListDragDropAction.Copy) 
					{
						e.Effect = DragDropEffects.Copy;
					} 
					else 
					{
						// MoveAndCopy
						if ((e.KeyState & 8) == 8) 
						{
							// 8 = Ctrl
							e.Effect = DragDropEffects.Copy;
						} 
						else 
						{
							e.Effect = DragDropEffects.Move;
						}
					}
				}
			}
			catch
			{
				throw;
			}
		}

		private void radMatrixBalance_CheckedChanged(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			if (FormLoaded)
			{
				if (!_setting_radMatrixBalance) 
				{
					ChangePending = true;
					btnSave.Enabled = true;
					this.cboIterations.Enabled = true;
					this.radBalanceToStore.Enabled = true;
					this.radBalanceToChain.Enabled = true;
				}
			}
			// END MID Track #5647
		}

		private void radMatrixForecast_CheckedChanged(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			if (FormLoaded)
			{
				if (!_setting_radMatrixForecast) 
				{
					ChangePending = true;
					btnSave.Enabled = true;
					this.radBalanceToStore.Checked = true;
					this.cboIterations.SelectedIndex = 0;
					this.cboIterations.Enabled = false;
					this.radBalanceToStore.Enabled = false;
					this.radBalanceToChain.Enabled = false;
				}
			}
			// END MID Track #5647
		}

		private void cboComputationMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			if (FormLoaded)
			{
				ChangePending = true;
				btnSave.Enabled = true;
			}
			// END MID Track #5647
		}

		private void cbModelName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// BEGIN MID Track #5647 - KJohnson - Matrix Forecast
			e.Handled = true;
			// END MID Track #5647
		}
	}

	public class DropIndicator : Control 
	{

		public DropIndicator() 
		{
			this.SetStyle(ControlStyles.UserPaint
				| ControlStyles.SupportsTransparentBackColor
				| ControlStyles.AllPaintingInWmPaint
				| ControlStyles.DoubleBuffer
				| ControlStyles.ResizeRedraw, true);
		}

		protected override void OnPaint(PaintEventArgs e) 
		{
			Graphics g = e.Graphics;
			using (Pen p = new Pen(this.ForeColor)) 
			{
				p.Width = 1;
				p.DashStyle = DashStyle.Dash;
				g.DrawLine(p, new Point(0, 0), new Point(this.Width, 0));
			}

			base.OnPaint (e);
		}

	}

	/// <summary>
	/// Action enum for a DualListDragDrop component
	/// </summary>
	public enum DualListDragDropAction 
	{
		Move, 
		Copy, 
		MoveAndCopy
	}
}
