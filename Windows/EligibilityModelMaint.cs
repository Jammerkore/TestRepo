// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Drawing;
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
	/// Summary description for EligModelMaint.
	/// </summary>
	public class frmEligModelMaint : ModelFormBase
	{
		private bool _tableLoaded = false;

//Begin  TT#1638 - Revised Model Save - RBeck
        //private bool _newModel = false;    
        //private bool _changeMade = false;
        //private int _modelRID = -1;
        //private string _currModel = null;
        //private bool _modelLocked = false;
        //private int _modelIndex = 0;
        //private string _saveAsName = "";

        EligModelProfile emp;

//End   TT#1638 - Revised Model Save - RBeck

		private DataTable _gridDataTable;
        private ProfileList _eligModelProfileList;
        //private PictureBox picBoxName;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public bool ChangeMade 
		{
			get { return _changeMade ; }
			set { _changeMade = value; }
		}

		public frmEligModelMaint(SessionAddressBlock aSAB) : base (aSAB)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

				_gridDataTable = MIDEnvironment.CreateDataTable("GridDataTable");
				_gridDataTable.RowChanged += new DataRowChangeEventHandler(Row_Changed); 
				DefineGridTable();
				LoadModelComboBox();
                //SetText();    //TT#1638 - MD - Revised Model Save - RBeck
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsEligibility);
				DetermineSecurity();
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

					if (_gridDataTable != null)
					{
						_gridDataTable.RowChanged -= new DataRowChangeEventHandler(Row_Changed); 
					}

                    //this.cbModelName.SelectedIndexChanged -= new System.EventHandler(this.cbModelName_SelectedIndexChanged);
					this.ugModel.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
					this.ugModel.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
					this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
					this.ugModel.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
					this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    ugld.DetachGridEventHandlers(ugModel);
                    //End TT#169
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
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // picBoxName
            // 
            this.picBoxName.InitialImage = null;
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
            this.ugModel.Location = new System.Drawing.Point(25, 42);
            this.ugModel.Size = new System.Drawing.Size(670, 391);
            this.ugModel.TabIndex = 12;
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
            this.ugModel.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
            this.ugModel.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
            this.ugModel.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
            // 
            // btnInUse
            // 
            this.btnInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // frmEligModelMaint
            // 
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Name = "frmEligModelMaint";
            this.Text = "Eligibility Model";
            this.Load += new System.EventHandler(this.frmEligModelMaint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void ugModel_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				// check for saved layout
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.eligibilityModelGrid);
				if (layout.LayoutLength > 0)
				{
					ugModel.DisplayLayout.Load(layout.LayoutStream);
				}
				else
				{	// DEFAULT grid layout
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
                    //ugld.ApplyDefaults(e);
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
                    //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
                    ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
                    // End TT#1164
                    //End TT#169
					DefaultGridLayout();
				}

				CommonGridLayout();
				
				if (!FunctionSecurity.AllowUpdate)
				{
					foreach (UltraGridBand ugb in this.ugModel.DisplayLayout.Bands)
					{
						ugb.Override.AllowDelete = DefaultableBoolean.False;
					}
				}
				else
				{
					foreach (UltraGridBand ugb in this.ugModel.DisplayLayout.Bands)
					{
						ugb.Override.AllowDelete = DefaultableBoolean.True;
					}
				}


			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		override public void InitializeForm()
		{
			try
			{
                FormLoaded = false;

                InitializeBaseForm();

                Cursor = Cursors.WaitCursor;

                if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.Eligibility, _modelRID);
					_modelLocked = false;
				}

				ugModel.BeginUpdate();
				_gridDataTable.Rows.Clear();
//				_gridDataTable.Rows.Add(new object[] { "", "", ""});
				ugModel.DataSource = _gridDataTable;
				UltraGridRow addedRow = ugModel.DisplayLayout.Bands[0].AddNew();
				ugModel.EndUpdate();
                _tableLoaded = true;

//Begin TT#1638 - MD - Revised Model Save - RBeck
                //btnSave.Enabled = false;
                //btnDelete.Enabled = false;
                //_newModel = true;
                //_modelRID = -1;
                //_currModel = null;
                //_modelLocked = false;
                //_saveAsName = "";

                //this.cbModelName.SelectedIndex = -1;
                //this.cbModelName.Text = "(new model)";
                //this.cbModelName.Enabled = false;
//End   TT#1638 - MD - Revised Model Save - RBeck

				Format_Title(eDataState.New, eMIDTextCode.frm_EligibilityModel, null);
				SetReadOnly(true);
				DetermineSecurity();
				
                FormLoaded = true;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
            finally
            {
                Cursor = Cursors.Default;
            }
		}

		override public bool InitializeForm(string currModel)
		{
			bool initializeSuccessful = true;
			bool displayModel = true;
			int modelIndex = -1;
			try
			{
				btnSave.Enabled = false;
				btnSaveAs.Enabled = false;
				_tableLoaded = false;
				_newModel = false;
				if (currModel == "")
				{
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
					btnDelete.Enabled = false;
					if (_eligModelProfileList.Count > 0)
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
                    btnInUse.Enabled = true;    //TT#110-MD-VStuart - In Use Tool
					btnDelete.Enabled = true;
					modelIndex = LocateModelIndex(currModel);
				}

				this.cbModelName.SelectedIndex = modelIndex;
				_saveAsName = currModel;
				if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.Eligibility, _modelRID);
					_modelLocked = false;
				}
		
				if (displayModel)
				{
					EligModelProfile emp = SAB.HierarchyServerSession.GetEligModelData(currModel);
					if (!_newModel && FunctionSecurity.AllowUpdate)
					{
						emp = (EligModelProfile) SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.Eligibility, emp.Key, true);
						if (emp.ModelLockStatus == eLockStatus.ReadOnly)
						{
							FunctionSecurity.SetReadOnly(); 
						}
						else
							if (emp.ModelLockStatus == eLockStatus.Cancel)
						{
							if (_currModel == null)
							{
                                return false ;                              
							}
							else
							{
								this.cbModelName.SelectedValue = _currModel;
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
						Format_Title(dataState, eMIDTextCode.frm_EligibilityModel, emp.ModelID);
						_modelRID = emp.Key;
						_currModel = emp.ModelID;
						_gridDataTable.Rows.Clear();
						int currRow = 0;
						if (emp.ModelEntries.Count > 0 ||
							emp.SalesEligibilityEntries.Count > 0 ||
							emp.PriorityShippingEntries.Count > 0)
						{
							bool allEntriesProcessed = false;
							string salesEligibility = "";
							string stockEligibility = "";
							string priorityShipping = "";
							while (!allEntriesProcessed)
							{
								if (emp.ModelEntries.Count > currRow)
								{
									EligModelEntry eme = (EligModelEntry)emp.ModelEntries[currRow];
									salesEligibility = eme.DateRange.DisplayDate;
								}
								else
								{
									salesEligibility = "";
								}
								if (emp.SalesEligibilityEntries.Count > currRow)
								{
									EligModelEntry eme = (EligModelEntry)emp.SalesEligibilityEntries[currRow];
									stockEligibility = eme.DateRange.DisplayDate;
								}
								else
								{
									stockEligibility = "";
								}
								if (emp.PriorityShippingEntries.Count > currRow)
								{
									EligModelEntry eme = (EligModelEntry)emp.PriorityShippingEntries[currRow];
									priorityShipping = eme.DateRange.DisplayDate;
								}
								else
								{
									priorityShipping = "";
								}
								_gridDataTable.Rows.Add(new object[] {salesEligibility, stockEligibility, priorityShipping});
								++currRow;
								if (currRow < emp.ModelEntries.Count ||
									currRow < emp.SalesEligibilityEntries.Count ||
									currRow < emp.PriorityShippingEntries.Count)
								{
									allEntriesProcessed = false;
								}
								else
								{
									allEntriesProcessed = true;
								}
							}
						}
						this.ugModel.DataSource = _gridDataTable;

						// set the cell tag to the date range object
						currRow = 0;
						foreach(  UltraGridRow gridRow in ugModel.Rows )
						{
							if (emp.ModelEntries.Count > currRow)
							{
								EligModelEntry eme = (EligModelEntry)emp.ModelEntries[currRow];
								GridCellTag cellTag = new GridCellTag();
								cellTag.GridCellTagData = eme.DateRange;
								gridRow.Cells["Stock Eligibility"].Tag = cellTag;
								if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
								{
									gridRow.Cells["Stock Eligibility"].Appearance.Image = ReoccurringImage;
								}
								else
								{
									gridRow.Cells["Stock Eligibility"].Appearance.Image = null;
								}
								if (FunctionSecurity.AllowUpdate)
								{
									// allow user to blank out field
									gridRow.Cells["Stock Eligibility"].Activation = Activation.AllowEdit;
								}
								else
								{
									gridRow.Cells["Stock Eligibility"].Activation = Activation.NoEdit;
								}
							}
							if (emp.SalesEligibilityEntries.Count > currRow)
							{
								EligModelEntry eme = (EligModelEntry)emp.SalesEligibilityEntries[currRow];
								GridCellTag cellTag = new GridCellTag();
								cellTag.GridCellTagData = eme.DateRange;
								gridRow.Cells["Sales Eligibility"].Tag = cellTag;
								if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
								{
									gridRow.Cells["Sales Eligibility"].Appearance.Image = ReoccurringImage;
								}
								else
								{
									gridRow.Cells["Sales Eligibility"].Appearance.Image = null;
								}
								if (FunctionSecurity.AllowUpdate)
								{
									// allow user to blank out field
									gridRow.Cells["Sales Eligibility"].Activation = Activation.AllowEdit;
								}
								else
								{
									gridRow.Cells["Sales Eligibility"].Activation = Activation.NoEdit;
								}
							}
							if (emp.PriorityShippingEntries.Count > currRow)
							{
								EligModelEntry eme = (EligModelEntry)emp.PriorityShippingEntries[currRow];
								GridCellTag cellTag = new GridCellTag();
								cellTag.GridCellTagData = eme.DateRange;
								gridRow.Cells["Priority Shipping"].Tag = cellTag;
								if (eme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
								{
									gridRow.Cells["Priority Shipping"].Appearance.Image = ReoccurringImage;
								}
								else
								{
									gridRow.Cells["Priority Shipping"].Appearance.Image = null;
								}
								if (FunctionSecurity.AllowUpdate)
								{
									// allow user to blank out field
									gridRow.Cells["Priority Shipping"].Activation = Activation.AllowEdit;
								}
								else
								{
									gridRow.Cells["Priority Shipping"].Activation = Activation.NoEdit;
								}
							}

							++currRow;
						}
						
                        this.cbModelName.Enabled = true;
						SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
						_tableLoaded = true;
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
					
                    Format_Title(dataState, eMIDTextCode.frm_EligibilityModel, "");
					SetReadOnly(FunctionSecurity.AllowUpdate);      //Security changes - 1/24/2005 vg

					if (currModel == "")
					{
						btnDelete.Enabled = false;
						btnSave.Enabled = false;
						btnSaveAs.Enabled = false;
                        btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
					}
				}
				DetermineSecurity();
				FormLoaded = true;
			}
			catch(Exception exception)
			{
				HandleException(exception);
				initializeSuccessful = false;
			}
            return initializeSuccessful;
		}

        //private void SetText()
        //{

        //}
        //private void DetermineSecurity()
        //{
        //    try
        //    {
        //        if (FunctionSecurity.AllowDelete &&
        //            cbModelName.SelectedIndex > -1)
        //        {
        //            btnDelete.Enabled = true;
        //        }
        //        else
        //        {
        //            btnDelete.Enabled = false;
        //        }

        //        if (FunctionSecurity.AllowUpdate)
        //        {
        //            btnNew.Enabled = true;
        //            btnSave.Enabled = true;
        //            //btnSaveAs.Enabled = true;
        //            btnSaveAs.Enabled = !_newModel;
        //        }
        //        else
        //        {
        //            btnNew.Enabled = false;
        //            btnSave.Enabled = false;
        //            btnSaveAs.Enabled = false;
        //        }
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}

		private void DefaultGridLayout()
		{
			try
			{
				this.ugModel.DisplayLayout.Bands[0].Columns["Stock Eligibility"].Width = (this.ugModel.Width / 3) - 10;
				this.ugModel.DisplayLayout.Bands[0].Columns["Sales Eligibility"].Width = (this.ugModel.Width / 3) - 10;
				this.ugModel.DisplayLayout.Bands[0].Columns["Priority Shipping"].Width = (this.ugModel.Width / 3) - 10;
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void CommonGridLayout()
		{
			try
			{
				this.ugModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugModel.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				this.ugModel.DisplayLayout.AddNewBox.Hidden = false;
				this.ugModel.DisplayLayout.AddNewBox.Prompt = "Add model row";
				this.ugModel.DisplayLayout.Bands[0].AddButtonCaption = "Date Information";
				this.ugModel.DisplayLayout.GroupByBox.Hidden = true;
				this.ugModel.DisplayLayout.GroupByBox.Prompt = "";
				this.ugModel.DisplayLayout.Bands[0].Columns["Stock Eligibility"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				this.ugModel.DisplayLayout.Bands[0].Columns["Sales Eligibility"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				this.ugModel.DisplayLayout.Bands[0].Columns["Priority Shipping"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;

				if (FunctionSecurity.AllowUpdate)
				{
					this.ugModel.DisplayLayout.AddNewBox.Hidden = false;
				}
				else
				{
					this.ugModel.DisplayLayout.AddNewBox.Hidden = true;
				}

				foreach(  UltraGridRow gridRow in ugModel.Rows )
				{
					if (FunctionSecurity.AllowUpdate)
					{
						// allow user to blank out field
						gridRow.Cells["Stock Eligibility"].Activation = Activation.AllowEdit;
						gridRow.Cells["Sales Eligibility"].Activation = Activation.AllowEdit;
						gridRow.Cells["Priority Shipping"].Activation = Activation.AllowEdit;
					}
					else
					{
						gridRow.Cells["Stock Eligibility"].Activation = Activation.NoEdit;
						gridRow.Cells["Sales Eligibility"].Activation = Activation.NoEdit;
						gridRow.Cells["Priority Shipping"].Activation = Activation.NoEdit;
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		 
		private void DefineGridTable()
		{
			try
			{
				DataColumn edDataColumn;

				//Create Columns and rows for datatable
				edDataColumn = new DataColumn();
				edDataColumn.DataType = System.Type.GetType("System.String");
				edDataColumn.ColumnName = "Stock Eligibility";
				edDataColumn.Caption = "Stock Eligibility";
				edDataColumn.ReadOnly = false;
				edDataColumn.Unique = false;
				_gridDataTable.Columns.Add(edDataColumn);

				edDataColumn = new DataColumn();
				edDataColumn.DataType = System.Type.GetType("System.String");
				edDataColumn.ColumnName = "Sales Eligibility";
				edDataColumn.Caption = "Sales Eligibility";
				edDataColumn.ReadOnly = false;
				edDataColumn.Unique = false;
				_gridDataTable.Columns.Add(edDataColumn);

				edDataColumn = new DataColumn();
				edDataColumn.DataType = System.Type.GetType("System.String");
				edDataColumn.ColumnName = "Priority Shipping";
				edDataColumn.Caption = "Priority Shipping";
				edDataColumn.ReadOnly = false;
				edDataColumn.Unique = false;
				_gridDataTable.Columns.Add(edDataColumn);
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
//Begin     TT#1638 - MD - Revised Model Save - RBeck

 
        //private void LoadModelComboBox()
        //{
        //    _eligModelProfileList = SAB.HierarchyServerSession.GetEligModels();
        //    cbModelName.Items.Clear();

        //    foreach (ModelName modelName in _eligModelProfileList.ArrayList)
        //    {
        //        cbModelName.Items.Add(
        //            new ModelNameCombo(modelName.Key, modelName.ModelID));
        //    }
        //    AdjustTextWidthComboBox_DropDown(cbModelName);
        //}

        //private int LocateModelIndex(string aModelName)
        //{
        //    int modelIndex = -1;
        //    int i = 0;
        //    foreach (ModelName modelName in _eligModelProfileList.ArrayList)
        //    {
        //        if (aModelName == modelName.ModelID)
        //        {
        //            modelIndex = i;
        //            break;
        //        }
        //        ++i;
        //    }
        //    return modelIndex;
        //}
//End     TT#1638 - MD - Revised Model Save - RBeck

		private void Row_Changed(object ob, DataRowChangeEventArgs e) 
		{ 
			try
			{
				DataTable t = (DataTable)  ob; 
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void ugModel_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
					CalendarDateSelector frmCalDtSelector = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[]{SAB});
					frmCalDtSelector.AllowReoccurring = true;
					frmCalDtSelector.AllowDynamic = false;
                    // Begin Track #5833 - JSmith - Null reference when dynamic to plan selected
                    //frmCalDtSelector.AllowDynamic = false;
                    frmCalDtSelector.AllowDynamic = true;
                    frmCalDtSelector.AllowDynamicToCurrent = false;
                    frmCalDtSelector.AllowDynamicToPlan = false;
                    frmCalDtSelector.AllowDynamicToStoreOpen = true;
                    frmCalDtSelector.OverrideNullAnchorDateDefaults = true;
                    // End Track #5833
					if (e.Cell.Tag != null &&
						e.Cell.Tag != System.DBNull.Value)
					{
						if (e.Cell.Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
						{
							GridCellTag cellTag = (GridCellTag)e.Cell.Tag;
							if (cellTag.GridCellTagData != null &&
								cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
							{
								DateRangeProfile drp = (DateRangeProfile)cellTag.GridCellTagData;
								frmCalDtSelector.DateRangeRID = drp.Key;
							}
						}
					}
					DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
					if (DateRangeResult == DialogResult.OK)
					{
						DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
						e.Cell.Value = SelectedDateRange.DisplayDate;
						GridCellTag cellTag = new GridCellTag();
						cellTag.GridCellTagData = SelectedDateRange;
						e.Cell.Tag = cellTag;
						if (SelectedDateRange.DateRangeType == eCalendarRangeType.Reoccurring)
						{
							e.Cell.Appearance.Image = ReoccurringImage;
						}
						else
						{
							e.Cell.Appearance.Image = null;
						}
						ChangePending = true;
						btnSave.Enabled = true;
                        //btnSaveAs.Enabled = true;
                        btnSaveAs.Enabled = !_newModel; //TT#1638 - MD - Revised Model Save - RBeck
					}
//					frmCalDtSelector.Remove();
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

//Begin     TT#1638 - MD - Revised Model Save - RBeck

        //override protected bool SaveChanges()
        //{
        //    try
        //    {
        //      GridCellTag cellTag = null;
        //      int saveKey;
        //      EligModelProfile emp = new EligModelProfile(_modelRID);
        //      if (_newModel)
        //      {
        //      emp.ModelChangeType = eChangeType.add;
        //      }
        //      else
        //      {
        //          emp.ModelChangeType = eChangeType.update;
        //      }			
        ////			emp.ModelEntries = new ArrayList();
        //      int stockEligEntrySeq = 0;
        //      int salesEligEntrySeq = 0;
        //      int priShipEntrySeq = 0;
        //foreach(  UltraGridRow gridRow in ugModel.Rows )
        //{
        //    if (!Convert.IsDBNull(gridRow.Cells["Stock Eligibility"].Value) &&
        //        (string)gridRow.Cells["Stock Eligibility"].Value != "")
        //    {
        //        EligModelEntry modelEntry = new EligModelEntry(); 
        //        modelEntry.EligModelEntryType = eEligModelEntryType.StockEligibility;
        //        if (gridRow.Cells["Stock Eligibility"].Tag != null &&
        //            gridRow.Cells["Stock Eligibility"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
        //        {
        //            cellTag = (GridCellTag)gridRow.Cells["Stock Eligibility"].Tag;
        //            if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
        //            {
        //                modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
        //                modelEntry.ModelEntrySeq = stockEligEntrySeq;
        //                modelEntry.ModelEntryChangeType = eChangeType.add;
        //                emp.ModelEntries.Add(modelEntry);
        //                ++stockEligEntrySeq;
        //                gridRow.Cells["Stock Eligibility"].Appearance.Image = null;
        //                cellTag.Message = null;
        //            }
        //            else
        //            {
        //                gridRow.Cells["Stock Eligibility"].Appearance.Image = ErrorImage;
        //                cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //                ErrorFound = true;
        //            }
        //        }
        //        else
        //        {
        //            cellTag = new GridCellTag();
        //            gridRow.Cells["Stock Eligibility"].Appearance.Image = ErrorImage;
        //            cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //            gridRow.Cells["Stock Eligibility"].Tag = cellTag;
        //            ErrorFound = true;
        //        }
        //    }
        //    if (!Convert.IsDBNull(gridRow.Cells["Sales Eligibility"].Value) &&
        //      (string)gridRow.Cells["Sales Eligibility"].Value != "")
        //    {
        //     EligModelEntry modelEntry = new EligModelEntry(); 
        //     modelEntry.EligModelEntryType = eEligModelEntryType.SalesEligibility;
        //     if (gridRow.Cells["Sales Eligibility"].Tag != null &&
        //         gridRow.Cells["Sales Eligibility"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
        //     {
        //         cellTag = (GridCellTag)gridRow.Cells["Sales Eligibility"].Tag;
        //         if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
        //         {
        //             modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
        //             modelEntry.ModelEntrySeq = salesEligEntrySeq;
        //             modelEntry.ModelEntryChangeType = eChangeType.add;
        //             emp.SalesEligibilityEntries.Add(modelEntry);
        //             ++salesEligEntrySeq;
        //             gridRow.Cells["Sales Eligibility"].Appearance.Image = null;
        //             cellTag.Message = null;
        //         }
        //         else
        //         {
        //             gridRow.Cells["Sales Eligibility"].Appearance.Image = ErrorImage;
        //             cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //             ErrorFound = true;
        //         }
        //     }
        //     else
        //     {
        //         cellTag = new GridCellTag();
        //         gridRow.Cells["Sales Eligibility"].Appearance.Image = ErrorImage;
        //         cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //         gridRow.Cells["Sales Eligibility"].Tag = cellTag;
        //         ErrorFound = true;
        //     }
        //  }
        //  if (!Convert.IsDBNull(gridRow.Cells["Priority Shipping"].Value) &&
        //      (string)gridRow.Cells["Priority Shipping"].Value != "")
        //  {
        //     EligModelEntry modelEntry = new EligModelEntry(); 
        //     modelEntry.EligModelEntryType = eEligModelEntryType.PriorityShipping;
        //     if (gridRow.Cells["Priority Shipping"].Tag != null &&
        //         gridRow.Cells["Priority Shipping"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
        //     {
        //         cellTag = (GridCellTag)gridRow.Cells["Priority Shipping"].Tag;
        //         if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
        //         {
        //             modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
        //             modelEntry.ModelEntrySeq = priShipEntrySeq;
        //             modelEntry.ModelEntryChangeType = eChangeType.add;
        //             emp.PriorityShippingEntries.Add(modelEntry);
        //             ++priShipEntrySeq;
        //             gridRow.Cells["Priority Shipping"].Appearance.Image = null;
        //             cellTag.Message = null;
        //         }
        //         else
        //         {
        //             gridRow.Cells["Priority Shipping"].Appearance.Image = ErrorImage;
        //             cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //             ErrorFound = true;
        //         }
        //     }
        //     else
        //     {
        //        cellTag = new GridCellTag();
        //        gridRow.Cells["Priority Shipping"].Appearance.Image = ErrorImage;
        //        cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
        //        gridRow.Cells["Priority Shipping"].Tag = cellTag;
        //        ErrorFound = true;
        //     }
        //}			 
        //if (!ErrorFound)
        //{  
        //        bool continueSave = false;
        //        frmSaveAs formSaveAs = new frmSaveAs(SAB);
        //        formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        //        if (PerformingSaveAs)
        //        {
        //            formSaveAs.SaveAsName = _currModel;
        //        }
        //        while (!continueSave)
        //        {
        //            formSaveAs.ShowDialog(this);
        //            saveAsCanceled = formSaveAs.SaveCanceled;
        //            if (!saveAsCanceled)
        //            {
        //                EligModelProfile checkExists = SAB.HierarchyServerSession.GetEligModelData(formSaveAs.SaveAsName);
        //                if (checkExists.Key == -1)
        //                {
        //                    _saveAsName = formSaveAs.SaveAsName;
        //                    continueSave = true;
        //                }
        //                else
        //                {
        //                    if (MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
        //                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        //                        == DialogResult.No) 
        //                    {
        //                        saveAsCanceled = true;
        //                        continueSave = true;
        //                    }
        //                    else
        //                    {
        //                        saveAsCanceled = false;
        //                        continueSave = true;
        //                        emp.ModelChangeType = eChangeType.update;
        //                        EligModelProfile emp2 =  SAB.HierarchyServerSession.GetEligModelData(formSaveAs.SaveAsName);
        //                        _modelRID = emp2.Key;
        //                        _saveAsName = formSaveAs.SaveAsName;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                continueSave = true;
        //            }
        //        }
        //      }                    				
        //      if (!saveAsCanceled) 
        //      {
        //          emp.ModelID = _saveAsName;
        //          _modelRID = GetProfileKey(_saveAsName); //
        //          emp.Key = _modelRID;
        //          saveKey = _modelRID;
        //          _modelRID = SAB.HierarchyServerSession.EligModelUpdate(emp);
        //          _changeMade = true;
        //          ChangePending = false;
        //          btnSave.Enabled = false;
        //     //btnSaveAs.Enabled = false;
        //          if (_newModel)
        //          {
        //              FormLoaded = false;
        //              LoadModelComboBox();
        //              if (_modelLocked &&
        //             saveKey != Include.NoRID)
        //              {
        //               SAB.HierarchyServerSession.DequeueModel(eModelType.Eligibility, saveKey);
        //               _modelLocked = false;
        //              }
        //              InitializeForm(_saveAsName);
        //              FormLoaded = true;
        //              Format_Title(eDataState.Updatable, eMIDTextCode.frm_EligibilityModel, _saveAsName);
        //              _newModel = false;
        //          }
        //      }
        //      else
        //          {
        //          MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled), this.Text,
        //          MessageBoxButtons.OK, MessageBoxIcon.Information);
        //          }
        //          this.cbModelName.Enabled = true;
        //          }
        //      }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    } 
        //    return true;
        //}
         protected override void ModelPreSave()
        {
            GridCellTag cellTag = null;

            emp = new EligModelProfile(_modelRID);

            if (_newModel)
            {
                emp.ModelChangeType = eChangeType.add;
            }
            else
            {
                emp.ModelChangeType = eChangeType.update;
            }

            int stockEligEntrySeq = 0;
            int salesEligEntrySeq = 0;
            int priShipEntrySeq = 0;
            foreach (UltraGridRow gridRow in ugModel.Rows)
            {
                if (!Convert.IsDBNull(gridRow.Cells["Stock Eligibility"].Value) &&
                    (string)gridRow.Cells["Stock Eligibility"].Value != "")
                {
                    EligModelEntry modelEntry = new EligModelEntry();
                    modelEntry.EligModelEntryType = eEligModelEntryType.StockEligibility;
                    if (gridRow.Cells["Stock Eligibility"].Tag != null &&
                        gridRow.Cells["Stock Eligibility"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
                    {
                        cellTag = (GridCellTag)gridRow.Cells["Stock Eligibility"].Tag;
                        if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
                        {
                            modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
                            modelEntry.ModelEntrySeq = stockEligEntrySeq;
                            modelEntry.ModelEntryChangeType = eChangeType.add;
                            emp.ModelEntries.Add(modelEntry);
                            ++stockEligEntrySeq;
                            gridRow.Cells["Stock Eligibility"].Appearance.Image = null;
                            cellTag.Message = null;
                        }
                        else
                        {
                            gridRow.Cells["Stock Eligibility"].Appearance.Image = ErrorImage;
                            cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                            ErrorFound = true;
                        }
                    }
                    else
                    {
                        cellTag = new GridCellTag();
                        gridRow.Cells["Stock Eligibility"].Appearance.Image = ErrorImage;
                        cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                        gridRow.Cells["Stock Eligibility"].Tag = cellTag;
                        ErrorFound = true;
                    }
                }
                if (!Convert.IsDBNull(gridRow.Cells["Sales Eligibility"].Value) &&
                    (string)gridRow.Cells["Sales Eligibility"].Value != "")
                {
                    EligModelEntry modelEntry = new EligModelEntry();
                    modelEntry.EligModelEntryType = eEligModelEntryType.SalesEligibility;
                    if (gridRow.Cells["Sales Eligibility"].Tag != null &&
                        gridRow.Cells["Sales Eligibility"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
                    {
                        cellTag = (GridCellTag)gridRow.Cells["Sales Eligibility"].Tag;
                        if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
                        {
                            modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
                            modelEntry.ModelEntrySeq = salesEligEntrySeq;
                            modelEntry.ModelEntryChangeType = eChangeType.add;
                            emp.SalesEligibilityEntries.Add(modelEntry);
                            ++salesEligEntrySeq;
                            gridRow.Cells["Sales Eligibility"].Appearance.Image = null;
                            cellTag.Message = null;
                        }
                        else
                        {
                            gridRow.Cells["Sales Eligibility"].Appearance.Image = ErrorImage;
                            cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                            ErrorFound = true;
                        }
                    }
                    else
                    {
                        cellTag = new GridCellTag();
                        gridRow.Cells["Sales Eligibility"].Appearance.Image = ErrorImage;
                        cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                        gridRow.Cells["Sales Eligibility"].Tag = cellTag;
                        ErrorFound = true;
                    }
                }
                if (!Convert.IsDBNull(gridRow.Cells["Priority Shipping"].Value) &&
                    (string)gridRow.Cells["Priority Shipping"].Value != "")
                {
                    EligModelEntry modelEntry = new EligModelEntry();
                    modelEntry.EligModelEntryType = eEligModelEntryType.PriorityShipping;
                    if (gridRow.Cells["Priority Shipping"].Tag != null &&
                        gridRow.Cells["Priority Shipping"].Tag.GetType() == typeof(MIDRetail.DataCommon.GridCellTag))
                    {
                        cellTag = (GridCellTag)gridRow.Cells["Priority Shipping"].Tag;
                        if (cellTag.GridCellTagData.GetType() == typeof(MIDRetail.DataCommon.DateRangeProfile))
                        {
                            modelEntry.DateRange = (DateRangeProfile)cellTag.GridCellTagData;
                            modelEntry.ModelEntrySeq = priShipEntrySeq;
                            modelEntry.ModelEntryChangeType = eChangeType.add;
                            emp.PriorityShippingEntries.Add(modelEntry);
                            ++priShipEntrySeq;
                            gridRow.Cells["Priority Shipping"].Appearance.Image = null;
                            cellTag.Message = null;
                        }
                        else
                        {
                            gridRow.Cells["Priority Shipping"].Appearance.Image = ErrorImage;
                            cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                            ErrorFound = true;
                        }
                    }
                    else
                    {
                        cellTag = new GridCellTag();
                        gridRow.Cells["Priority Shipping"].Appearance.Image = ErrorImage;
                        cellTag.Message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCalendarDate);
                        gridRow.Cells["Priority Shipping"].Tag = cellTag;
                        ErrorFound = true;
                    }
                }
            }
        }

        protected override ProfileList GetModels()
        {
            _eligModelProfileList = SAB.HierarchyServerSession.GetEligModels();
            return _eligModelProfileList;
        }
 
        protected override void ModelType(int _saveKey)
        {
            SAB.HierarchyServerSession.DequeueModel(eModelType.Eligibility, _saveKey);
        }

        protected override void ModelText(string _saveAsName)
        {
            Format_Title(eDataState.Updatable, eMIDTextCode.frm_EligibilityModel, _saveAsName);
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
	    public override void ShowInUse()
	    {
	        var emp = new EligModelProfile(_modelRID);
	        eProfileType type = emp.ProfileType;
	        int rid = _modelRID;
	        base.ShowInUse(type, rid, inQuiry2);
	    }
        //END TT#110-MD-VStuart - In Use Tool

	    protected override void GetModelRID(string _saveAsName, ref int _modelRID)
        {
            emp.ModelID = _saveAsName;
            emp.Key = _modelRID;
            _modelRID = SAB.HierarchyServerSession.EligModelUpdate(emp);
        }

        protected override void AlterModelChangeType(eChangeType _changeType)
        {
            emp.ModelChangeType = _changeType;
        }
    
        protected override int GetProfileKey(string _SaveAsName)
        {
            EligModelProfile checkExists = SAB.HierarchyServerSession.GetEligModelData(_SaveAsName);
             
            return checkExists.Key;
        }

        protected override void DefaultErrorFound(bool _defaultErrorFound)
        {

        }

        //private void btnSave_Click(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        SaveChanges();
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}
//End     TT#1638 - MD - Revised Model Save - RBeck

		private void ugModel_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded && _tableLoaded)
			{
				ChangePending = true;
				btnSave.Enabled = true;
			}
		}

		private void ugModel_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				e.Row.Cells["Stock Eligibility"].Activation = Activation.AllowEdit;
				e.Row.Cells["Sales Eligibility"].Activation = Activation.AllowEdit;
				e.Row.Cells["Priority Shipping"].Activation = Activation.AllowEdit;
			}
			catch( Exception error)
			{
				HandleException(error);
			}
		}

		override protected void BeforeClosing()
		{
			try
			{
				if (FunctionSecurity.AllowUpdate)
				{
                    // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
                    //if (!ugModel.IsDisposed)
                    if (FormLoaded &&
                        !ugModel.IsDisposed)
                    // End TT#2012
					{
						InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
						layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.eligibilityModelGrid, ugModel);
					}
				}
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
					SAB.HierarchyServerSession.DequeueModel(eModelType.Eligibility, _modelRID);
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "frmEligModelMaint.AfterClosing");
			}
		}

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
				    if (MessageBox.Show(text, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
				        == DialogResult.Yes)
				    //BEGIN TT#601-MD-VStuart-FWOS Model attempts delete while InUse
				    {
				        //If the RID is InUse don't delete. If RID is NOT InUse go ahead and delete.
                        var empro = new EligModelProfile(_modelRID);
                        eProfileType type = empro.ProfileType;
				        int rid = _modelRID;

				        if (!CheckInUse(type, rid, false))
				        {
				            Cursor = Cursors.WaitCursor;
				            EligModelProfile emp = new EligModelProfile(_modelRID);
				            emp.ModelChangeType = eChangeType.delete;
				            emp.Key = _modelRID;
				            SAB.HierarchyServerSession.EligModelUpdate(emp);
				            _changeMade = true;
				            itemDeleted = true;
				            FormLoaded = false;
				            _gridDataTable.Rows.Clear();
				            _gridDataTable.Rows.Add(new object[] {"", ""});

				            LoadModelComboBox();
				        }
				    }
                    //END  TT#601-MD-VStuart-FWOS Model attempts delete while InUse
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
						string modelName = ((ModelName)_eligModelProfileList[nextItem]).ModelID;
						InitializeForm(modelName);
					}
					else
					{
						InitializeForm();
					}
				}
			}
			catch(DatabaseForeignKeyViolation)
			{
                //BEGIN TT#110-MD-VStuart - In Use Tool
				//MessageBox.Show (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
			    ShowInUse();
                //END TT#110-MD-VStuart - In Use Tool
            }
			catch(Exception exception)
			{
				HandleException(exception);
			}
            finally
            {
                Cursor = Cursors.Default;
            }
		}
//Begin     TT#1638 - MD - Revised Model Save - RBeck
        // Begin TT#1638 - JSmith - Revised Model Save
        protected override DataTable GetFilteredModels(string eligibilityModelNameFilter, bool isCaseSensitive)
        {
            try
            {
                MerchandiseHierarchyData modelsData = new MerchandiseHierarchyData();

                return modelsData.GetFilteredEligibilityModels(eligibilityModelNameFilter, isCaseSensitive);
            }
            catch (Exception ex)
            {
                HandleException(ex, "GetFilteredModels");
                throw ex;
            }               
        }
        // End TT#1638 - JSmith - Revised Model Save
 
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
 
        //private void cbModelName_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (FormLoaded)
        //        {
        //            CheckForPendingChanges();
        //            if (cbModelName.SelectedIndex >= 0)
        //            {
        //                _modelIndex = cbModelName.SelectedIndex;
        //                string modelName = ((ModelName)(_eligModelProfileList[_modelIndex])).ModelID;
        //                InitializeForm(modelName);
        //            }
        //        }
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}
//End     TT#1638 - MD - Revised Model Save - RBeck
		private void ugModel_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowUltraGridToolTip(ugModel, e);
			}
			catch( Exception exception )
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
        //        PerformingSaveAs = false;
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
        //    catch(Exception ex)
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

        private void frmEligModelMaint_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {

        }
   }
}
