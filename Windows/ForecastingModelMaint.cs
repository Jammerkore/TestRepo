// BEGIN MID Track #5773 - KJohnson - Planned FWOS Enhancement
// Too many changes to mark.  Use difference tool for comparison.
// END MID Track #5773

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Configuration;
using System.Globalization;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for frmForecastBalModelMaint.
	/// </summary>
	public class frmForecastingModelMaint : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private bool _newModel = false;
		private bool _changeMade = false;
		private int _modelRID = -1;
		private string _currModel = null;
		private bool _modelLocked = false;
		private int _modelIndex = 0;
		private bool _setRowPosition = true;
		private string _saveAsName = "";
		private System.Data.DataTable _methodsDataTable;
		private ForecastModelProfile _forecastModelProfile;
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
		private bool _MIDOnlyFunctions;
		private bool _CurrentClientIsANF;
//End Modification - JScott - Allow access to Computation Mode with Config Setting

		// drag drop fields
		// Properties
		protected DualListDragDropAction _action = DualListDragDropAction.Move;
		protected bool _showIndicator = true;
		protected Color _indicatorColor = Color.Red;

		// Extra Fields
		protected bool _mouseDown = false;
		protected int _indexOfItemUnderMouseToDrop;
		protected int _indexOfItemUnderMouseToDrag;
		protected int _oldVariableRID;
		protected Point _screenOffset;
		protected Rectangle _dragBoxFromMouseDown;
		private DropIndicator _dropper;
		private System.Windows.Forms.CheckBox cbDefault;
		private DateTime _nextScroll = DateTime.Now;
		private ProfileList _variableVariables;
		private ProfileList _salesVariables;
		private Infragistics.Win.ValueList _forecastFormulaStockValueList;
		private Infragistics.Win.ValueList _forecastFormulaSalesValueList;
		private Infragistics.Win.ValueList _forecastFormulaAllValueList;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbModelName;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugMethods;
		private System.Windows.Forms.ContextMenu mnuWorkflowGrid;


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

		public override bool ChangePending
		{
			get	{return base.ChangePending;}
			set	
			{

				if (FormLoaded)
				{
					if (AllowUpdate)
					{
						base.ChangePending = value;

						if (value == true)
							btnSave.Enabled = true;
						else
							btnSave.Enabled = false;
					}

				}
			}
		}

		protected bool ShouldSerializeIndicatorColor() 
		{
			return IndicatorColor != Color.Red;
		}

		protected void ResetIndicatorColor() 
		{
			IndicatorColor = Color.Red;
		}
		
//		override public bool ChangePending
//		{
//			set	
//			{
//				base.ChangePending = value;
//
//				btnSave.Enabled = true;
//			}
//		}

		private System.Windows.Forms.Label lblModelName;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnNew;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Panel pnlData;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboComputationMode;
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

		public frmForecastingModelMaint(SessionAddressBlock aSAB) : base (aSAB)
		{
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
			string MIDOnlyFunctionsStr;

//End Modification - JScott - Allow access to Computation Mode with Config Setting
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				_dropper = new DropIndicator();
				_dropper.Visible = false;

				//==========================
				// For now this isn't used
				//==========================
				if (_MIDOnlyFunctions)
				{
					cbDefault.Visible = true;
					cbDefault.Enabled = true;
				} 
				else 
				{
					cbDefault.Visible = false;
					cbDefault.Enabled = false;
				}

				_SAB = aSAB;
				LoadModelComboBox();
				SetText();
				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsForecasting);
				//=================================================================================
				// This is a read only screen to clients. Only MID can update the forecast models.
				//=================================================================================
//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
//				#if (!DEBUG)
//				FunctionSecurity.SetReadOnly();
//				#endif

                // Begin TT#1054 - JSmith - Relieve Intransit not working.
                //MIDOnlyFunctionsStr = System.Configuration.ConfigurationSettings.AppSettings["MIDOnlyFunctions"];
                MIDOnlyFunctionsStr = MIDConfigurationManager.AppSettings["MIDOnlyFunctions"];
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

				if (!_MIDOnlyFunctions)
				{
					FunctionSecurity.SetReadOnly();
				}
//End Modification - JScott - Allow access to Computation Mode with Config Setting

                //---See If Current Client Using Software Is ANF---------
				CustomBusinessRoutines businessRoutines = new CustomBusinessRoutines(_SAB, null, null, Include.NoRID);
				_CurrentClientIsANF = businessRoutines.IsPresentationMinDefined();

				DetermineSecurity();
				pnlData.Enabled = false;

				_variableVariables = new ProfileList(eProfileType.Variable);
				_salesVariables = new ProfileList(eProfileType.Variable);
				_forecastFormulaStockValueList = new Infragistics.Win.ValueList();
				_forecastFormulaSalesValueList = new Infragistics.Win.ValueList();
				_forecastFormulaAllValueList = new Infragistics.Win.ValueList();
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

					this.cbModelName.SelectionChangeCommitted -= new System.EventHandler(this.cbModelName_SelectionChangeCommitted);
					this.cboComputationMode.SelectionChangeCommitted -= new System.EventHandler(this.cboComputationMode_SelectionChangeCommitted);
                    //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                    this.cbModelName.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cbModelName_MIDComboBoxPropertiesChangedEvent);
                    this.cboComputationMode.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboComputationMode_MIDComboBoxPropertiesChangedEvent);
                    //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
					this.cbDefault.CheckedChanged -= new System.EventHandler(this.cbDefault_CheckedChanged);
					this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
					this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
					this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
					this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
					this.ugMethods.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMethods_InitializeLayout);
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    ugld.DetachGridEventHandlers(ugMethods);
                    //End TT#169
					this.ugMethods.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellChange);
					this.ugMethods.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMethods_AfterRowInsert);
					this.ugMethods.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugMethods_BeforeCellListDropDown);
					this.ugMethods.AfterSortChange -= new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugMethods_AfterSortChange);

					// remove data bindings
					this.ugMethods.DataSource = null;

					this.mnuWorkflowGrid.Dispose();
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
			Infragistics.Win.UltraWinGrid.UltraGridBand ultraGridBand1 = new Infragistics.Win.UltraWinGrid.UltraGridBand("", -1);
			Infragistics.Win.UltraWinGrid.UltraGridLayout ultraGridLayout1 = new Infragistics.Win.UltraWinGrid.UltraGridLayout();
			this.lblModelName = new System.Windows.Forms.Label();
			this.cbModelName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnNew = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.pnlData = new System.Windows.Forms.Panel();
			this.ugMethods = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.cbDefault = new System.Windows.Forms.CheckBox();
			this.cboComputationMode = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.lblComputationMode = new System.Windows.Forms.Label();
			this.mnuWorkflowGrid = new System.Windows.Forms.ContextMenu();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			this.pnlData.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ugMethods)).BeginInit();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// lblModelName
			// 
			this.lblModelName.Location = new System.Drawing.Point(16, 16);
			this.lblModelName.Name = "lblModelName";
			this.lblModelName.Size = new System.Drawing.Size(80, 23);
			this.lblModelName.TabIndex = 0;
			this.lblModelName.Text = "Model Name";
			this.lblModelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbModelName
			// 
			this.cbModelName.Location = new System.Drawing.Point(104, 16);
			this.cbModelName.Name = "cbModelName";
			this.cbModelName.Size = new System.Drawing.Size(184, 21);
			this.cbModelName.TabIndex = 1;
			this.cbModelName.SelectionChangeCommitted += new System.EventHandler(this.cbModelName_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cbModelName.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cbModelName_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(696, 468);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnNew
			// 
			this.btnNew.Location = new System.Drawing.Point(376, 16);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(75, 23);
			this.btnNew.TabIndex = 10;
			this.btnNew.Text = "New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(456, 16);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 11;
			this.btnDelete.Text = "Delete";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(296, 16);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 13;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlData
			// 
			this.pnlData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlData.Controls.Add(this.ugMethods);
			this.pnlData.Controls.Add(this.cbDefault);
			this.pnlData.Controls.Add(this.cboComputationMode);
			this.pnlData.Controls.Add(this.lblComputationMode);
			this.pnlData.Location = new System.Drawing.Point(24, 48);
			this.pnlData.Name = "pnlData";
			this.pnlData.Size = new System.Drawing.Size(736, 412);
			this.pnlData.TabIndex = 19;
			// 
			// ugMethods
			// 
			this.ugMethods.AllowDrop = true;
			this.ugMethods.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ugMethods.DisplayLayout.AddNewBox.Hidden = false;
			this.ugMethods.DisplayLayout.AddNewBox.Prompt = " Add ...";
			ultraGridBand1.AddButtonCaption = " Action";
			this.ugMethods.DisplayLayout.BandsSerializer.Add(ultraGridBand1);
			ultraGridLayout1.AddNewBox.Hidden = false;
			this.ugMethods.Layouts.Add(ultraGridLayout1);
			this.ugMethods.Location = new System.Drawing.Point(14, 31);
			this.ugMethods.Name = "ugMethods";
			this.ugMethods.Size = new System.Drawing.Size(711, 368);
			this.ugMethods.TabIndex = 28;
			this.ugMethods.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ugMethods_BeforeCellListDropDown);
			this.ugMethods.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugMethods_InitializeLayout);
			this.ugMethods.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugMethods_AfterRowInsert);
			this.ugMethods.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugMethods_CellChange);
			this.ugMethods.AfterSortChange += new Infragistics.Win.UltraWinGrid.BandEventHandler(this.ugMethods_AfterSortChange);
			// 
			// cbDefault
			// 
			this.cbDefault.Location = new System.Drawing.Point(339, 7);
			this.cbDefault.Name = "cbDefault";
			this.cbDefault.Size = new System.Drawing.Size(104, 16);
			this.cbDefault.TabIndex = 25;
			this.cbDefault.Text = "Default Model";
			this.cbDefault.CheckedChanged += new System.EventHandler(this.cbDefault_CheckedChanged);
			// 
			// cboComputationMode
			// 
			this.cboComputationMode.Location = new System.Drawing.Point(121, 4);
			this.cboComputationMode.Name = "cboComputationMode";
			this.cboComputationMode.Size = new System.Drawing.Size(160, 21);
			this.cboComputationMode.TabIndex = 20;
			this.cboComputationMode.SelectionChangeCommitted += new System.EventHandler(this.cboComputationMode_SelectionChangeCommitted);
            //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
            this.cboComputationMode.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboComputationMode_MIDComboBoxPropertiesChangedEvent);
            //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
			// 
			// lblComputationMode
			// 
			this.lblComputationMode.Location = new System.Drawing.Point(9, 4);
			this.lblComputationMode.Name = "lblComputationMode";
			this.lblComputationMode.Size = new System.Drawing.Size(104, 23);
			this.lblComputationMode.TabIndex = 19;
			this.lblComputationMode.Text = "Computation Mode:";
			this.lblComputationMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmForecastingModelMaint
			// 
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(784, 498);
			this.Controls.Add(this.pnlData);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.cbModelName);
			this.Controls.Add(this.lblModelName);
			this.Name = "frmForecastingModelMaint";
			this.Text = "Forecasting Model";
			this.Load += new System.EventHandler(this.frmForecastBalModelMaint_Load);
			this.Controls.SetChildIndex(this.lblModelName, 0);
			this.Controls.SetChildIndex(this.cbModelName, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnNew, 0);
			this.Controls.SetChildIndex(this.btnDelete, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.pnlData, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			this.pnlData.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ugMethods)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void frmForecastBalModelMaint_Load(object sender, System.EventArgs e)
		{
			try
			{
				FormLoaded = false;
				BuildComputationModeComboBox();

//Begin Modification - JScott - Allow access to Computation Mode with Config Setting
//#if (DEBUG)
//				lblComputationMode.Visible = true;
//				cboComputationMode.Visible = true;
//#else
//				lblComputationMode.Visible = false;
//				cboComputationMode.Visible = false; 
//#endif
				if (_MIDOnlyFunctions)
				{
					lblComputationMode.Visible = true;
					cboComputationMode.Visible = true;
				}
				else
				{
					lblComputationMode.Visible = false;
					cboComputationMode.Visible = false; 
				}

//End Modification - JScott - Allow access to Computation Mode with Config Setting

				Methods_Define();
                this.ugMethods.DataSource = _methodsDataTable;

				BuildWorkflowContextmenu();
				this.ugMethods.ContextMenu = mnuWorkflowGrid;

				FormLoaded = true;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		public void InitializeForm()
		{
			try
			{
				FormLoaded = false;
				btnSave.Enabled = false;
				btnDelete.Enabled = false;
				if (_modelLocked)
				{
					DequeueModel(_forecastModelProfile);
					_modelLocked = false;
				}

				_newModel = true;
				_modelRID = -1;
				_currModel = null;
				_modelLocked = false;
				_saveAsName = "";
				_methodsDataTable.Rows.Clear();
				_methodsDataTable.AcceptChanges();
				_forecastModelProfile = new ForecastModelProfile(Include.NoRID);
				BuildVariableListBox(_forecastModelProfile);
				cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(GetDefaultComputationsName()));
				if (cboComputationMode.SelectedIndex == -1)
				{
					cboComputationMode.SelectedIndex = 0;
				}
				cbDefault.Checked = _forecastModelProfile.IsDefault;
				this.cbModelName.SelectedIndex = -1;
				this.cbModelName.Text = "(new model)";
				this.cbModelName.Enabled = false;
				Format_Title(eDataState.New, eMIDTextCode.frm_Forecast_Model, null);
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

		private bool InitializeForm(int aKey)
		{
			try
			{
				return InitializeForm(new ForecastModelProfile(aKey));
			}
			catch
			{
				throw;
			}
		}

		private bool InitializeForm(string aID)
		{
			try
			{
				ForecastModelProfile fmp = new ForecastModelProfile(aID);
				fmp.ModelID = aID;
				return InitializeForm(fmp);
			}
			catch
			{
				throw;
			}
		}

		private bool InitializeForm(ForecastModelProfile aForecastModelProfile)
		{
			bool initializeSuccessful = true;
			bool displayModel = true;
			int modelIndex = -1;
			int rowPosition = 0;
			object forecastFormula; 
			object assocSalesVar;

			try
			{
				FormLoaded = false;
				btnSave.Enabled = false;
				_newModel = false;
				if (aForecastModelProfile.ModelID == "")
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
					modelIndex = cbModelName.Items.IndexOf(new ModelNameCombo(aForecastModelProfile.Key, aForecastModelProfile.ModelID));
				}

				this.cbModelName.SelectedIndex = modelIndex;
				_saveAsName = aForecastModelProfile.ModelID;
				if (_modelLocked)
				{
					DequeueModel(_forecastModelProfile);
					_modelLocked = false;
				}
		
				if (displayModel)
				{
					if (!_newModel && FunctionSecurity.AllowUpdate)
					{
						eLockStatus lockStatus = EnqueueModel(aForecastModelProfile);
						if (lockStatus == eLockStatus.ReadOnly)
						{
							FunctionSecurity.SetReadOnly(); 
						}
						else
							if (lockStatus == eLockStatus.Cancel)
						{
							FormLoaded = true;
							return false;
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

						BuildVariableListBox(aForecastModelProfile);
						
						cboComputationMode.SelectedIndex = cboComputationMode.Items.IndexOf(new ComputationModeCombo(aForecastModelProfile.ComputationMode.Trim()));
						cbDefault.Checked = aForecastModelProfile.IsDefault;

						_methodsDataTable.Rows.Clear();
						_methodsDataTable.AcceptChanges();

						foreach (ModelVariableProfile mvp in aForecastModelProfile.Variables.ArrayList)
						{
							if (mvp.ForecastFormula == Include.NoRID) 
							{
								forecastFormula = System.DBNull.Value;
							} 
							else
							{
								forecastFormula = mvp.ForecastFormula;
							}

							if (mvp.AssocVariable == Include.NoRID) 
							{
								assocSalesVar = System.DBNull.Value;
							} 
							else
							{
								assocSalesVar = mvp.AssocVariable;
							}

							_methodsDataTable.Rows.Add(new object[] { rowPosition, mvp.ForcastModelRID, mvp.Key,
																		forecastFormula, assocSalesVar, mvp.UsePlan, mvp.GradeWOSIDX,	// Track #6187
																		mvp.StockModifier, mvp.FWOSOverride, mvp.StockMin,
																		mvp.StockMax, mvp.MinPlusSales, mvp.SalesModifier, mvp.AllowChainNegatives });  // Track #6271
							++rowPosition;
						}

						SetVariableListByRow();

						Format_Title(dataState, eMIDTextCode.frm_Forecast_Model, aForecastModelProfile.ModelID);
						_modelRID = aForecastModelProfile.Key;
						_currModel = aForecastModelProfile.ModelID;
						
						this.cbModelName.Enabled = true;
						SetReadOnly(FunctionSecurity.AllowUpdate);
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
					Format_Title(dataState, eMIDTextCode.frm_Forecast_Model, "");
					SetReadOnly(FunctionSecurity.AllowUpdate);  

					if (aForecastModelProfile.ModelID == "")
					{
						btnDelete.Enabled = false;
						btnSave.Enabled = false;
					}
				}
				DetermineSecurity();
				pnlData.Enabled = true;
				FormLoaded = true;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				initializeSuccessful = false;
			}
			return initializeSuccessful;
		}

		private void SetVariableListByRow()
		{
			foreach(  UltraGridRow gridRow in ugMethods.Rows )
			{
				SetVaraibleList(gridRow);
			}
		}

		private void SetVaraibleList(UltraGridRow aRow)
		{
			//int selectedVariableRID;
			eForecastFormulaType formulaType;
			VariableProfile vp;

			if (aRow.Cells["Variable"].Value != System.DBNull.Value) 
			{

				formulaType = (eForecastFormulaType)Convert.ToInt32(aRow.Cells["ForecastFormula"].Value, CultureInfo.CurrentUICulture);
				// Begin Track #6187
				//selectedVariableRID = Convert.ToInt32(aRow.Cells["Variable"].Value, CultureInfo.CurrentUICulture);
				//vp = (VariableProfile)_variableVariables.FindKey(selectedVariableRID);

				//if (vp.VariableForecastType == eVariableForecastType.Sales) 
				//{
				//    formulaType = eForecastFormulaType.Sales;
				//}
				//else if (vp.VariableForecastType == eVariableForecastType.Stock) 
				//{
				//    formulaType = eForecastFormulaType.Stock;
				//}
				//else 
				//{
				//    formulaType = eForecastFormulaType.PctContribution;
				//}
				// End track #6187
				SetComboBoxStockVarList(aRow, formulaType);
			}
		}

		private void SetText()
		{
			btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Save);
			btnDelete.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Delete);
			btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			Text = MIDText.GetTextOnly(eMIDTextCode.frm_Forecast_Model);
			lblComputationMode.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ComputationMode) + ":";
		}

		private void DetermineSecurity()
		{
			try
			{
				if (FunctionSecurity.AllowDelete &&
					cbModelName.SelectedIndex > -1)
				{
					btnDelete.Enabled = true;
				}
				else
				{
					btnDelete.Enabled = false;
				}

				if (FunctionSecurity.AllowUpdate)
				{
					btnNew.Enabled = true;

					if (cbModelName.Items.Count > 0)
					{
						btnSave.Enabled = true;
					}
					else 
					{
						btnSave.Enabled = false;
					}
				}
				else
				{
					btnNew.Enabled = false;
					btnSave.Enabled = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

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

				BuildStockVarComboBox();
				BuildSalesVarComboBox();

				CommonMethodsGridLayout();
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
				this.ugMethods.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns;
				this.ugMethods.DisplayLayout.Override.AllowColMoving = AllowColMoving.NotAllowed;

				this.ugMethods.DisplayLayout.AddNewBox.Hidden = false;
				this.ugMethods.DisplayLayout.GroupByBox.Hidden = true;
				this.ugMethods.DisplayLayout.GroupByBox.Prompt = string.Empty;
				this.ugMethods.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			    this.ugMethods.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_AddVariable);
			  //this.ugMethods.DisplayLayout.Bands[0].AddButtonCaption = MIDText.GetTextOnly(eMIDTextCode.lbl_Variable);


				this.ugMethods.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;

				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastModelRID"].Hidden = true;

				this.ugMethods.DisplayLayout.Bands[0].ColHeaderLines = 1;

				this.ugMethods.DisplayLayout.Bands[0].LevelCount = 1;
				this.ugMethods.DisplayLayout.Bands[0].Override.AllowGroupMoving = AllowGroupMoving.NotAllowed;
				this.ugMethods.DisplayLayout.Bands[0].Groups.Clear();
				this.ugMethods.DisplayLayout.Bands[0].Groups.Add("Variables", "      ");
				// Begin Track #6187 stodd
				this.ugMethods.DisplayLayout.Bands[0].Groups.Add("Basis", MIDText.GetTextOnly(eMIDTextCode.lbl_BasisOverride));
				// End Track #6187 stodd
				this.ugMethods.DisplayLayout.Bands[0].Groups.Add("Modifiers", MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyModifiers));

				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Variables"]; 
				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Variables"]; 
				this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Variables"];

				// Begin Track #6187 stodd
				this.ugMethods.DisplayLayout.Bands[0].Columns["UsePlan"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Basis"];
				// End Track #6187 stodd

				this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"].Header.Band.ColHeaderLines = 2;

				this.ugMethods.DisplayLayout.Bands[0].Columns["GradeWOSIndex"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
				this.ugMethods.DisplayLayout.Bands[0].Columns["StockModifier"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
                this.ugMethods.DisplayLayout.Bands[0].Columns["FWOSOverride"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
                this.ugMethods.DisplayLayout.Bands[0].Columns["StockMin"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
                this.ugMethods.DisplayLayout.Bands[0].Columns["StockMax"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
                this.ugMethods.DisplayLayout.Bands[0].Columns["MinPlusSales"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
                this.ugMethods.DisplayLayout.Bands[0].Columns["SalesModifier"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"]; 
				// Begin Track #6187 stodd
				this.ugMethods.DisplayLayout.Bands[0].Columns["AllowChainNegatives"].Group = this.ugMethods.DisplayLayout.Bands[0].Groups["Modifiers"];
				// End Track #6187 stodd

                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugMethods);
                //End TT#169
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
				_methodsDataTable = new System.Data.DataTable("methodsDataTable");
			
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
				dataColumn.DataType = System.Type.GetType("System.Int32");
				dataColumn.ColumnName = "ForecastModelRID";
				dataColumn.Caption = "ForecastModelRID";
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

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
				dataColumn.ColumnName = "ForecastFormula";
			    dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastFormula);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Object");
				dataColumn.ColumnName = "AssociatedVariable";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AssociatedSalesVar).Replace(" ", "\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				// BEGIN MID Track #6187 stodd
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "UsePlan";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_UsePlanAsBasis).Replace(" ", "\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);
				// END MID Track #6187 stodd

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "GradeWOSIndex";
			    dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Grade) + "\n" + MIDText.GetTextOnly(eMIDTextCode.lbl_WOS_Index);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "StockModifier";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Stock_Modifier).Replace(" ", "\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "FWOSOverride";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_FWOS_Modifier).Replace(" ", "\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "StockMin";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_StockMin);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "StockMax";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_StockMax);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "MinPlusSales";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_PMPlusSales).Replace("+ ", "+\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "SalesModifier";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Sales_Modifier).Replace(" ", "\n");
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);

				// Begin Track #6271 - spreading negatives
				dataColumn = new DataColumn();
				dataColumn.DataType = System.Type.GetType("System.Boolean");
				dataColumn.ColumnName = "AllowChainNegatives";
				dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_AllowChainNegatives);
				dataColumn.ReadOnly = false;
				dataColumn.Unique = false;
				dataColumn.AllowDBNull = true;
				_methodsDataTable.Columns.Add(dataColumn);
				// End Track #6271 - spreading negatives
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

		private void AddValueLists()
		{
			try
			{
				this.ugMethods.DisplayLayout.ValueLists.Clear();
				this.ugMethods.DisplayLayout.ValueLists.Add("Variable");
				this.ugMethods.DisplayLayout.ValueLists.Add("ForecastFormula");
				this.ugMethods.DisplayLayout.ValueLists.Add("AssociatedVariable");
				// Begin Track #6187 stodd
				this.ugMethods.DisplayLayout.ValueLists.Add("BasisVariable");
				// End Track #6187 
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void DefaultMethodsGridLayout()
		{
			try
			{
				this.ugMethods.DisplayLayout.Bands[0].Columns["RowPosition"].Hidden = true;

				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastModelRID"].Hidden = true;

				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].Width = 100;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].AutoEdit = true;
                this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["Variable"].ValueList = ugMethods.DisplayLayout.ValueLists["Variable"];

				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].Width = 100;
				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].AutoEdit = true;
                this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				this.ugMethods.DisplayLayout.Bands[0].Columns["ForecastFormula"].ValueList = ugMethods.DisplayLayout.ValueLists["ForecastFormula"];

				this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].Width = 100;
				this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].AutoEdit = true;
                this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
				// Begin Track #6187 stodd
				//this.ugMethods.DisplayLayout.Bands[0].Columns["AssociatedVariable"].ValueList = ugMethods.DisplayLayout.ValueLists["AssociatedVariable"];

				this.ugMethods.DisplayLayout.Bands[0].Columns["UsePlan"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["UsePlan"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				// End Track #6187

				this.ugMethods.DisplayLayout.Bands[0].Columns["GradeWOSIndex"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["GradeWOSIndex"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				this.ugMethods.DisplayLayout.Bands[0].Columns["StockModifier"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["StockModifier"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				this.ugMethods.DisplayLayout.Bands[0].Columns["FWOSOverride"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["FWOSOverride"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				this.ugMethods.DisplayLayout.Bands[0].Columns["StockMin"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["StockMin"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				this.ugMethods.DisplayLayout.Bands[0].Columns["StockMax"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["StockMax"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				this.ugMethods.DisplayLayout.Bands[0].Columns["MinPlusSales"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["MinPlusSales"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				if (_CurrentClientIsANF)
				{
					this.ugMethods.DisplayLayout.Bands[0].Columns["MinPlusSales"].Hidden = false;
				}
				else 
				{
					this.ugMethods.DisplayLayout.Bands[0].Columns["MinPlusSales"].Hidden = true;
				}

				this.ugMethods.DisplayLayout.Bands[0].Columns["SalesModifier"].Width = 50;
				this.ugMethods.DisplayLayout.Bands[0].Columns["SalesModifier"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;

				// Begin Track #6271 stodd
				this.ugMethods.DisplayLayout.Bands[0].Columns["AllowChainNegatives"].Width = 75;
				this.ugMethods.DisplayLayout.Bands[0].Columns["AllowChainNegatives"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
				// End Track #6271
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //FormatColumns(this.ugMethods);
                //End TT#169
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void BuildStockVarComboBox()
		{
			try
			{
				// add sales items to drop down
				_forecastFormulaSalesValueList.ValueListItems.Clear();
                _forecastFormulaSalesValueList.ValueListItems.Add((int)eForecastFormulaType.Sales, MIDText.GetTextOnly(eMIDTextCode.lbl_FWOS_Sales));
				_forecastFormulaSalesValueList.ValueListItems.Add((int)eForecastFormulaType.PctContribution, MIDText.GetTextOnly(eMIDTextCode.lbl_PctContribution));

				// add stock items to drop down
				_forecastFormulaStockValueList.ValueListItems.Clear();
                _forecastFormulaStockValueList.ValueListItems.Add((int)eForecastFormulaType.Stock, MIDText.GetTextOnly(eMIDTextCode.lbl_FWOS_Stock));
				_forecastFormulaStockValueList.ValueListItems.Add((int)eForecastFormulaType.PctContribution, MIDText.GetTextOnly(eMIDTextCode.lbl_PctContribution));

				// add all items to drop down
				_forecastFormulaAllValueList.ValueListItems.Clear();
				_forecastFormulaAllValueList.ValueListItems.Add((int)eForecastFormulaType.PctContribution, MIDText.GetTextOnly(eMIDTextCode.lbl_PctContribution));
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private void Set_ApplyModifiers(UltraGridRow aRow, eForecastFormulaType formulaType)
		{
			try
			{
				string var = aRow.Cells["ForecastFormula"].Value.ToString();
				Debug.WriteLine(var + " " + formulaType.ToString());				
				if (formulaType == eForecastFormulaType.Sales) 
				{
					aRow.Cells["ForecastFormula"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

					aRow.Cells["AssociatedVariable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["AssociatedVariable"].Value = System.DBNull.Value;

					aRow.Cells["GradeWOSIndex"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["FWOSOverride"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockMin"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockMax"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["MinPlusSales"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["SalesModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					// Begin Track #6187 stodd
					aRow.Cells["UsePlan"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["UsePlan"].Value = false;
					// End Track #6187 stodd

					aRow.Cells["GradeWOSIndex"].Value = false;
					aRow.Cells["StockModifier"].Value = false;
					aRow.Cells["FWOSOverride"].Value = false;
					aRow.Cells["StockMin"].Value = false;
					aRow.Cells["StockMax"].Value = false;
					aRow.Cells["MinPlusSales"].Value = false;
					// Begin Track #6271 stodd
					aRow.Cells["AllowChainNegatives"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					// End Track #6271 stodd
				} 
				else if (formulaType == eForecastFormulaType.Stock) 
				{
					aRow.Cells["ForecastFormula"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

					aRow.Cells["AssociatedVariable"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					// Begin Track #6187 stodd
					aRow.Cells["AssociatedVariable"].ValueList = ugMethods.DisplayLayout.ValueLists["AssociatedVariable"];
					// End Track #6187
					//if (aRow.Cells["AssociatedVariable"].ValueListResolved.ItemCount > 0) 
					//{
					//    //--Set Item In DropDown List To First One------
					//    aRow.Cells["AssociatedVariable"].Value = aRow.Cells["AssociatedVariable"].ValueListResolved.GetValue(0);
					//}

					aRow.Cells["GradeWOSIndex"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["StockModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["FWOSOverride"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["StockMin"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["StockMax"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["MinPlusSales"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["SalesModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

					aRow.Cells["SalesModifier"].Value = false;

					// Begin Track #6187 stodd
					aRow.Cells["UsePlan"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["UsePlan"].Value = false;
					// End Track #6187 stodd
					// Begin Track #6271 stodd
					aRow.Cells["AllowChainNegatives"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["AllowChainNegatives"].Value = false;
					// End Track #6271 stodd
				}
				else if (formulaType == eForecastFormulaType.PctContribution) 
				{
					aRow.Cells["ForecastFormula"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;

					// Begin Track #6187 stodd
					aRow.Cells["AssociatedVariable"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					aRow.Cells["AssociatedVariable"].ValueList = ugMethods.DisplayLayout.ValueLists["BasisVariable"];
					//if (aRow.Cells["AssociatedVariable"].ValueListResolved.ItemCount > 0)
					//{
					//    //--Set Item In DropDown List To First One------
					//    aRow.Cells["AssociatedVariable"].Value = aRow.Cells["AssociatedVariable"].ValueListResolved.GetValue(0);
					//}
					// End Track #6187 

					aRow.Cells["GradeWOSIndex"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["FWOSOverride"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockMin"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["StockMax"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["MinPlusSales"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					aRow.Cells["SalesModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

					aRow.Cells["GradeWOSIndex"].Value = false;
					aRow.Cells["StockModifier"].Value = false;
					aRow.Cells["FWOSOverride"].Value = false;
					aRow.Cells["StockMin"].Value = false;
					aRow.Cells["StockMax"].Value = false;
					aRow.Cells["MinPlusSales"].Value = false;
					aRow.Cells["SalesModifier"].Value = false;

					// Begin Track #6187 stodd
					aRow.Cells["UsePlan"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					//aRow.Cells["UsePlan"].Value = false;
					// End Track #6187 stodd
					// Begin Track #6271 stodd
					aRow.Cells["AllowChainNegatives"].Activation = Infragistics.Win.UltraWinGrid.Activation.AllowEdit;
					// End Track #6271 stodd
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
			}
		}

		private VariableProfile GetDefaultVariableProfile()
		{
			try
			{
                return new VariableProfile(0, "(No Override)", eVariableCategory.None, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.None, eVariableScope.None, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.None, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, null);
			}
			catch
			{
				throw;
			}
		}

		private void LoadModelComboBox()
		{
			ForecastModelProfileList forecastModelProfileList = new ForecastModelProfileList(true);
			cbModelName.Items.Clear();

			foreach (ForecastModelProfile fmp in forecastModelProfileList.ArrayList)
			{
				cbModelName.Items.Add(
					new ModelNameCombo(fmp.Key, fmp.ModelID.Trim(), fmp));
			}

			if (cbModelName.Items.Count > 0)
			{
				cbModelName.Enabled = true;
			}
			else 
			{
				cbModelName.Enabled = false;
			}
		}

		private void BuildSalesVarComboBox()
		{
			try
			{
				Infragistics.Win.ValueListItem valListItem;

				_salesVariables.Clear();

				ugMethods.DisplayLayout.ValueLists["AssociatedVariable"].ValueListItems.Clear();

				// add blank items at top of drop down
				//ugMethods.DisplayLayout.ValueLists["AssociatedVariable"].ValueListItems.Add(new MIDListBoxItem(Include.NoRID, " ", GetDefaultVariableProfile()));

                ApplicationSessionTransaction aApplicationTransaction = new ApplicationSessionTransaction(_SAB);
                PlanCubeGroup cubeGroup = (PlanCubeGroup)aApplicationTransaction.GetForecastCubeGroup();
                ProfileList variables = cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                //ProfileList variables = _SAB.ApplicationServerSession.Variables.VariableProfileList;
				
				// add variables from model
				foreach (VariableProfile vp in variables.ArrayList)
				{
					if (vp != null && vp.AllowOTSForecast)
					{
						if (vp.VariableForecastType == eVariableForecastType.Sales)
						{
							_salesVariables.Add(vp);
							//ugMethods.DisplayLayout.ValueLists["AssociatedVariable"].ValueListItems.Add(new MIDListBoxItem(vp.Key, vp.VariableName, vp));
							valListItem = new Infragistics.Win.ValueListItem();
							valListItem.DataValue= vp.Key;
							valListItem.DisplayText = vp.VariableName;
							ugMethods.DisplayLayout.ValueLists["AssociatedVariable"].ValueListItems.Add(valListItem);
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

		private void SetComboBoxStockVarList(UltraGridRow aRow, eForecastFormulaType formulaType)
		{
			try
			{
				switch (formulaType)
				{
					case eForecastFormulaType.Sales:
						aRow.Cells["ForecastFormula"].ValueList = _forecastFormulaSalesValueList;
						break;
					case eForecastFormulaType.Stock:
						aRow.Cells["ForecastFormula"].ValueList = _forecastFormulaStockValueList;
						break;
					case eForecastFormulaType.PctContribution:
						aRow.Cells["ForecastFormula"].ValueList = _forecastFormulaAllValueList;
						break;
				}
				Set_ApplyModifiers(aRow, formulaType);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void BuildVariableListBox(ForecastModelProfile aForecastModelProfile)
		{
			try
			{
				_variableVariables.Clear();

				ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Clear();

				// add blank items at top of drop down
				//ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(new MIDListBoxItem(Include.NoRID, " ", GetDefaultVariableProfile()));

                ApplicationSessionTransaction aApplicationTransaction = new ApplicationSessionTransaction(_SAB);
                PlanCubeGroup cubeGroup = (PlanCubeGroup)aApplicationTransaction.GetForecastCubeGroup();
                ProfileList variables = cubeGroup.Transaction.PlanComputations.PlanVariables.VariableProfileList;
                //ProfileList variables = _SAB.ApplicationServerSession.Variables.VariableProfileList;
				
				// add variables from model
				foreach (ModelVariableProfile mvp in aForecastModelProfile.Variables)
				{
					VariableProfile vp = (VariableProfile)variables.FindKey(mvp.Key);
					if (vp != null && vp.AllowOTSForecast)
					{
						if (vp.VariableForecastType == eVariableForecastType.Other
							|| vp.VariableForecastType == eVariableForecastType.Sales
							|| vp.VariableForecastType == eVariableForecastType.Stock)
						{
							//int salesVarKey = aForecastModelProfile.SalesVariable;
							//int stockVarKey = aForecastModelProfile.StockVariable;

							//if (vp.Key == salesVarKey || vp.Key == stockVarKey)
							//{
							//	// Don't add the sales and stock variables currently chosen
							//	// into the variable list box. They'll get added below, unchecked.
							//}
							//else
							//{
								_variableVariables.Add(vp);
								ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(vp.Key, vp.VariableName);
							//}
						}
					}
				}

				// add additional variables
				foreach (VariableProfile vp in variables.ArrayList)
				{
					if (!aForecastModelProfile.Variables.Contains(vp.Key))
					{
						if (vp.AllowOTSForecast)
						{
							if (vp.VariableForecastType == eVariableForecastType.Other
							  || vp.VariableForecastType == eVariableForecastType.Sales
							  || vp.VariableForecastType == eVariableForecastType.Stock)
							{
								_variableVariables.Add(vp);
								ugMethods.DisplayLayout.ValueLists["Variable"].ValueListItems.Add(vp.Key, vp.VariableName);
							}
						}
					}
				}

				// Begin Track #6187 stodd
				ugMethods.DisplayLayout.ValueLists["BasisVariable"].ValueListItems.Clear();
				ugMethods.DisplayLayout.ValueLists["BasisVariable"].ValueListItems.Add(Include.NoRID, " ");
				foreach (VariableProfile vp in _variableVariables.ArrayList)
				{
					ugMethods.DisplayLayout.ValueLists["BasisVariable"].ValueListItems.Add(vp.Key, vp.VariableName);
				}
				// End Track #6187

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
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void ugMethods_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				eForecastFormulaType formulaType;

				switch (ugMethods.ActiveCell.Column.Key)
				{
					case "Variable":
						int selectedVariableIndex = ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex;
						if (selectedVariableIndex != -1)
						{
							int selectedVariableRID = (int)ugMethods.ActiveCell.ValueListResolved.GetValue(selectedVariableIndex);
							VariableProfile vp = (VariableProfile)_variableVariables.FindKey(selectedVariableRID);

							//--- Look For Previously Selected Value In All The Other Rows Of Grid ---
							bool foundIt = false;
							foreach(  UltraGridRow gridRow in ugMethods.Rows )
							{
								if (gridRow.Index != ugMethods.ActiveRow.Index) 
								{
									if (gridRow.Cells["Variable"].Value != System.DBNull.Value) 
									{
										if (Convert.ToInt32(gridRow.Cells["Variable"].Value, CultureInfo.CurrentUICulture) == selectedVariableRID) 
										{
											foundIt = true;
											break;
										}
									}
								}
							}

							if (!foundIt) 
							{
								//-- Select Row For Further Editing By User ----
								if (vp.VariableForecastType == eVariableForecastType.Sales) 
								{
									formulaType = eForecastFormulaType.Sales;
								}
								else if (vp.VariableForecastType == eVariableForecastType.Stock) 
								{
									formulaType = eForecastFormulaType.Stock;
								}
								else 
								{
									formulaType = eForecastFormulaType.PctContribution;
								}

								SetComboBoxStockVarList(ugMethods.ActiveCell.Row, formulaType);
								if (ugMethods.ActiveRow.Cells["ForecastFormula"].ValueListResolved.ItemCount > 0) 
								{
									//--Set Item In DropDown List To First One------
									ugMethods.ActiveRow.Cells["ForecastFormula"].Value = ugMethods.ActiveRow.Cells["ForecastFormula"].ValueListResolved.GetValue(0);
								}
							}
							else
							{
								//-- Show Error And Wipe Out Row Information That Hasa Been Selected Already ---
								MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_VariableAlreadyAdded));

								//-- Reset Row Back To Previous Value ---------------------------------
								if (_oldVariableRID != Include.NoRID) 
								{
									ugMethods.ActiveCell.Row.Cells["Variable"].Value = _oldVariableRID;
								} 
								else 
								{
									ugMethods.ActiveCell.Row.Cells["Variable"].Value = System.DBNull.Value;
									ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex = -1;
									_oldVariableRID = Include.NoRID;
								}
							}
						}
						break;
					case "ForecastFormula":
						int selectedForcastFormulaIndex = ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex;
						formulaType = (eForecastFormulaType)ugMethods.ActiveCell.ValueListResolved.GetValue(selectedForcastFormulaIndex);
						Set_ApplyModifiers(ugMethods.ActiveRow, formulaType);
						break;
					case "AssociatedVariable":
						int selectedAssocSaleVarIndex = ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex;
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
				e.Row.Cells["RowPosition"].Value = ugMethods.Rows.Count - 1;
				e.Row.Cells["ForecastModelRID"].Value = Include.NoRID;

				e.Row.Cells["ForecastFormula"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["AssociatedVariable"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

				e.Row.Cells["GradeWOSIndex"].Value = false;
				e.Row.Cells["StockModifier"].Value = false;
				e.Row.Cells["FWOSOverride"].Value = false;
				e.Row.Cells["StockMin"].Value = false;
				e.Row.Cells["StockMax"].Value = false;
				e.Row.Cells["MinPlusSales"].Value = false;
				e.Row.Cells["SalesModifier"].Value = false;
				e.Row.Cells["UsePlan"].Value = false;
				e.Row.Cells["AllowChainNegatives"].Value = false;	// Track # 6271

				e.Row.Cells["GradeWOSIndex"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["StockModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["FWOSOverride"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["StockMin"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["StockMax"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["MinPlusSales"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["SalesModifier"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["UsePlan"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				e.Row.Cells["AllowChainNegatives"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;	// Track #6271

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
				if (e.Cell.Column.Key == "Variable")
				{
					int selectedVariableIndex = ugMethods.ActiveCell.ValueListResolved.SelectedItemIndex;
					if (selectedVariableIndex != -1)
					{
						_oldVariableRID = (int)ugMethods.ActiveCell.ValueListResolved.GetValue(selectedVariableIndex);
					} 
					else 
					{
						_oldVariableRID = Include.NoRID;
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex);
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

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cancel_Click();
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				bool saveAsCanceled = false;
				ErrorFound = false;

				if (ChangePending) 
				{
				  //ProfileList variables = _SAB.ApplicationServerSession.Variables.VariableProfileList;
					if (_forecastModelProfile != null) 
					{
						_forecastModelProfile.Variables.Clear();
					}

					ModelVariableProfile mvp;

					foreach(  UltraGridRow gridRow in ugMethods.Rows )
					{
						if (gridRow.Cells["Variable"].Value != System.DBNull.Value) 
						{
							int selectedVariableRID = Convert.ToInt32(gridRow.Cells["Variable"].Value, CultureInfo.CurrentUICulture);

							mvp = new ModelVariableProfile(selectedVariableRID);
							mvp.IsSelected = true;
							if (_forecastModelProfile.Variables.FindKey(mvp.Key) == null) 
							{
								if (gridRow.Cells["ForecastFormula"].Value != System.DBNull.Value) 
								{
									mvp.ForecastFormula = Convert.ToInt32(gridRow.Cells["ForecastFormula"].Value, CultureInfo.CurrentUICulture);
								}
								if (gridRow.Cells["AssociatedVariable"].Value != System.DBNull.Value) 
								{
									mvp.AssocVariable = Convert.ToInt32(gridRow.Cells["AssociatedVariable"].Value, CultureInfo.CurrentUICulture);
								}
								mvp.GradeWOSIDX = Convert.ToBoolean(gridRow.Cells["GradeWOSIndex"].Value, CultureInfo.CurrentUICulture);
								mvp.StockModifier = Convert.ToBoolean(gridRow.Cells["StockModifier"].Value, CultureInfo.CurrentUICulture);
								mvp.FWOSOverride = Convert.ToBoolean(gridRow.Cells["FWOSOverride"].Value, CultureInfo.CurrentUICulture);
								mvp.StockMin = Convert.ToBoolean(gridRow.Cells["StockMin"].Value, CultureInfo.CurrentUICulture);
								mvp.StockMax = Convert.ToBoolean(gridRow.Cells["StockMax"].Value, CultureInfo.CurrentUICulture);
								mvp.MinPlusSales = Convert.ToBoolean(gridRow.Cells["MinPlusSales"].Value, CultureInfo.CurrentUICulture);
								mvp.SalesModifier = Convert.ToBoolean(gridRow.Cells["SalesModifier"].Value, CultureInfo.CurrentUICulture);
								mvp.UsePlan = Convert.ToBoolean(gridRow.Cells["UsePlan"].Value, CultureInfo.CurrentUICulture);	// Track #6187
								mvp.AllowChainNegatives = Convert.ToBoolean(gridRow.Cells["AllowChainNegatives"].Value, CultureInfo.CurrentUICulture);	// Track #6271
								_forecastModelProfile.Variables.Add(mvp);
							}
						}
					}


	//				if (_forecastModelProfile.Variables.Count == 0)
	//				{
	//					ErrorProvider.SetError(clbVariables, _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NeedAtLeastOneVariable));
	//					ErrorFound = true;
	//				}
	//				else
	//				{
	//					ErrorProvider.SetError(clbVariables, string.Empty);
	//				}
								
					if (!ErrorFound)
					{
						if (_newModel)
						{
							_forecastModelProfile.ModelChangeType = eChangeType.add;
						}
						else
						{
							_forecastModelProfile.ModelChangeType = eChangeType.update;
						}

						if (_newModel && (_saveAsName == ""))
						{
							bool continueSave = false;
							frmSaveAs formSaveAs = new frmSaveAs(_SAB);
							formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
							while (!continueSave)
							{
								formSaveAs.ShowDialog(this);
								saveAsCanceled = formSaveAs.SaveCanceled;
								if (!saveAsCanceled)
								{
									ForecastModelProfile checkExists = new ForecastModelProfile(formSaveAs.SaveAsName);
									if (checkExists.Key == -1)
									{
										_saveAsName = formSaveAs.SaveAsName;
										continueSave = true;
									}
									else
									{
										if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
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
											_forecastModelProfile.ModelChangeType = eChangeType.update;
											ForecastModelProfile fmp2 =  new ForecastModelProfile(formSaveAs.SaveAsName);
											_modelRID = fmp2.Key;
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
							_forecastModelProfile.ModelID = _saveAsName.Trim();
							_forecastModelProfile.Key = _modelRID;

							if (cbDefault.Checked)
								_forecastModelProfile.IsDefault = true;
							else
								_forecastModelProfile.IsDefault = false;

							_forecastModelProfile.ComputationMode = cboComputationMode.Text;

							_modelRID = _forecastModelProfile.WriteProfile();
							_forecastModelProfile.Key = _modelRID;
							_changeMade = true;
							ChangePending = false;
							btnSave.Enabled = false;
							if (_newModel)
							{
								FormLoaded = false;
								LoadModelComboBox();
								InitializeForm(_forecastModelProfile);
								FormLoaded = true;
								Format_Title(eDataState.Updatable, eMIDTextCode.frm_Forecast_Model, _saveAsName);
								_newModel = false;
							}
						}
						else
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
								MessageBoxButtons.OK, MessageBoxIcon.Information);
							ChangePending = false;
							if (cbModelName.Items.Count > 0)
							{
								// stodd
								//_forecastModelProfile = (ForecastModelProfile)((MIDListBoxItem)cbModelName.Items[0]).Tag;
								_forecastModelProfile = (ForecastModelProfile)((MIDRetail.Common.ModelNameCombo)cbModelName.Items[0]).Tag;
								InitializeForm(_forecastModelProfile);
							}
							else
							{
								InitializeForm();
							}

						}
						this.cbModelName.Enabled = true;
					}
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

		private eLockStatus EnqueueModel(ForecastModelProfile aForecastingProfile)
		{
			try
			{
				eLockStatus lockStatus = eLockStatus.Undefined;

				ModelEnqueue modelEnqueue = new ModelEnqueue(
					eModelType.Forecasting,
					aForecastingProfile.Key,
					_SAB.ClientServerSession.UserRID,
					_SAB.ClientServerSession.ThreadID);

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
						errMsg += System.Environment.NewLine + "Model: " + aForecastingProfile.ModelID + ", User: " + MCon.UserName;
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

		private void DequeueModel(ForecastModelProfile aForecastingProfile)
		{
			try
			{
				ModelEnqueue modelEnqueue = new ModelEnqueue(
					eModelType.Forecasting,
					aForecastingProfile.Key,
					_SAB.ClientServerSession.UserRID,
					_SAB.ClientServerSession.ThreadID);

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
					DequeueModel(_forecastModelProfile);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "frmForecastBalModelMaint.AfterClosing");
			}
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			try
			{
				int currIndex = cbModelName.SelectedIndex;
				bool itemDeleted = false;
				if (_modelRID != Include.NoRID)
				{
					string text = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
					text =  text.Replace("{0}", this.Text.Trim() + " ");
					if (MessageBox.Show (text,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
						== DialogResult.Yes) 
					{
						ForecastModelProfile fmp = new ForecastModelProfile(_modelRID);
						fmp.ModelChangeType = eChangeType.delete;
						fmp.Key = _modelRID;
						fmp.WriteProfile();
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
						_forecastModelProfile = (ForecastModelProfile)((ModelNameCombo)cbModelName.Items[nextItem]).Tag;
						InitializeForm(_forecastModelProfile);
					}
					else
					{
						InitializeForm();
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
				MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void btnNew_Click(object sender, System.EventArgs e)
		{
			try
			{
				FormLoaded = false;
				CheckForPendingChanges();
				InitializeForm();
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void cbModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			try
			{
				if (FormLoaded)
				{
					if (FormLoaded && CheckForPendingChanges()) 
					{
						ChangePending = false;
					}

					if (cbModelName.SelectedIndex >= 0)
					{
						_modelIndex = cbModelName.SelectedIndex;
						ForecastModelProfile newModel = (ForecastModelProfile)((ModelNameCombo)(cbModelName.Items[cbModelName.SelectedIndex])).Tag;
						InitializeForm(newModel);
						_forecastModelProfile = newModel;
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cbModelName_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cbModelName_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		private void cboIterations_SelectionChangeCommitted(object sender, System.EventArgs e)
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

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboIterations_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboIterations_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

		private void cboComputationMode_SelectionChangeCommitted(object sender, System.EventArgs e)
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

        //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
        void cboComputationMode_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboComputationMode_SelectionChangeCommitted(source, new EventArgs());
        }
        //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

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

		private void cbDefault_CheckedChanged(object sender, System.EventArgs e)
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

		#region Context Menu

		private void BuildWorkflowContextmenu()
		{
			try
			{
				MenuItem mnuItemInsert = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert));
				MenuItem mnuItemInsertBefore = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_Before));
				MenuItem mnuItemInsertAfter = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Insert_after));
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

		#endregion Context Menu


		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

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

		override public void ISaveAs()
		{
			
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

	}
}

