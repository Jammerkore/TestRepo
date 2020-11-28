// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
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
	/// Summary description for SalesModifierModelMaint.
	/// </summary>
	public class frmSalesModifierModelMaint : ModelFormBase
	{
        //		private SessionAddressBlock _SAB;   // TT#1638 - Revised Model Save - RBeck
//		private bool _cancelClicked = false;

		private bool _tableLoaded = false;

//End  TT#1638 - Revised Model Save - RBeck

        //private bool _newModel = false;
        //private bool _changeMade = false;
        //private int _modelRID = -1;
        //private string _currModel = null;
        //private bool _modelLocked = false;
        //private int _modelIndex = 0;
        //private string _saveAsName = "";

        SlsModModelProfile smmp;
        UltraGridRow firstErrorRow = null;
        UltraGridCell firstErrorCell = null;

//End  TT#1638 - Revised Model Save - RBeck

		private DataTable _gridDataTable;
//		private CalendarDateSelector _frmCalDtSelector;
        private ProfileList _slsModModelProfileList;
        //private System.Windows.Forms.Label lblModelName;
        //private Infragistics.Win.UltraWinGrid.UltraGrid ugModel;
		private System.Windows.Forms.Label lblSalesModifierDefault;
		private System.Windows.Forms.TextBox txtSalesModifierDefault;
        //private System.Windows.Forms.ComboBox cbModelName;
		private System.ComponentModel.IContainer components;
		public bool ChangeMade 
		{
			get { return _changeMade ; }
			set { _changeMade = value; }
		}

		public frmSalesModifierModelMaint(SessionAddressBlock aSAB) : base (aSAB)
		{
			try
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();

                // SAB = aSAB;     // TT#1638 - Revised Model Save - RBeck
				_gridDataTable = MIDEnvironment.CreateDataTable("GridDataTable");
				_gridDataTable.RowChanged += new DataRowChangeEventHandler(Row_Changed);
				DefineGridTable();
				LoadModelComboBox();
				SetText();
				FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminModelsSalesModifier);
				DetermineSecurity();
//Begin Track #3707 - JScott - Null Reference on model save
				btnSave.Enabled = false;
//End Track #3707 - JScott - Null Reference on model save
				btnSaveAs.Enabled = false;
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
					this.ugModel.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugModel_BeforeExitEditMode);
					this.ugModel.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
					this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
					this.ugModel.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
					this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                    MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                    ugld.DetachGridEventHandlers(ugModel);
                    //End TT#169
					this.txtSalesModifierDefault.TextChanged -= new System.EventHandler(this.txtSalesModifierDefault_TextChanged);
					this.txtSalesModifierDefault.Leave -= new System.EventHandler(this.txtSalesModifierDefault_Leave);
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
            this.lblSalesModifierDefault = new System.Windows.Forms.Label();
            this.txtSalesModifierDefault = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // cbModelName
            // 
            this.cbModelName.TabIndex = 23;
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
            this.ugModel.Location = new System.Drawing.Point(25, 96);
            this.ugModel.Size = new System.Drawing.Size(670, 337);
            this.ugModel.TabIndex = 21;
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
            this.ugModel.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
            this.ugModel.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
            this.ugModel.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugModel_BeforeExitEditMode);
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
            // lblSalesModifierDefault
            // 
            this.lblSalesModifierDefault.Location = new System.Drawing.Point(190, 55);
            this.lblSalesModifierDefault.Name = "lblSalesModifierDefault";
            this.lblSalesModifierDefault.Size = new System.Drawing.Size(120, 26);
            this.lblSalesModifierDefault.TabIndex = 27;
            this.lblSalesModifierDefault.Text = "Default Value:";
            this.lblSalesModifierDefault.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSalesModifierDefault
            // 
            this.txtSalesModifierDefault.Location = new System.Drawing.Point(320, 56);
            this.txtSalesModifierDefault.Name = "txtSalesModifierDefault";
            this.txtSalesModifierDefault.Size = new System.Drawing.Size(79, 20);
            this.txtSalesModifierDefault.TabIndex = 28;
            this.txtSalesModifierDefault.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSalesModifierDefault.TextChanged += new System.EventHandler(this.txtSalesModifierDefault_TextChanged);
            this.txtSalesModifierDefault.Leave += new System.EventHandler(this.txtSalesModifierDefault_Leave);
            // 
            // frmSalesModifierModelMaint
            // 
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.txtSalesModifierDefault);
            this.Controls.Add(this.lblSalesModifierDefault);
            this.Name = "frmSalesModifierModelMaint";
            this.Text = "Sales Modifier Model Maintenance";
            this.Controls.SetChildIndex(this.btnInUse, 0);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.ugModel, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.lblSalesModifierDefault, 0);
            this.Controls.SetChildIndex(this.txtSalesModifierDefault, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void ugModel_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			try
			{
				// check for saved layout
				InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
				InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(SAB.ClientServerSession.UserRID, eLayoutID.salesModifierModelGrid);
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
					SAB.HierarchyServerSession.DequeueModel(eModelType.SalesModifier, _modelRID);
					_modelLocked = false;
				}

				_tableLoaded = false;
				this.txtSalesModifierDefault.Text = "1.0";
				_gridDataTable.Rows.Clear();
				ugModel.DataSource = _gridDataTable;
//				_gridDataTable.Rows.Add(new object[] { "", ""});
				UltraGridRow addedRow = ugModel.DisplayLayout.Bands[0].AddNew();
				_tableLoaded = true;

//Begin TT#1638 - MD - Revised Model Save - RBeck
                //btnSave.Enabled = false;
                //btnSaveAs.Enabled = false;
                //btnDelete.Enabled = false;
                //_newModel = true;
                //_modelRID = -1;
                //_currModel = null;
                //_modelLocked = false;
                //_saveAsName = "";
                //this.cbModelName.SelectedIndex = -1;
                //this.cbModelName.Text = "(new model)";
                //this.cbModelName.Enabled = false;
//End  TT#1638 - MD - Revised Model Save - RBeck

				Format_Title(eDataState.New, eMIDTextCode.frm_SalesModifierModel, null);
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

		override public bool  InitializeForm(string currModel)
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
				DoValueByValueEdits = false;
				if (currModel == "")
				{
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
                    btnDelete.Enabled = false;
					if (_slsModModelProfileList.Count > 0)
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
				if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.SalesModifier, _modelRID);
					_modelLocked = false;
				}

				this.cbModelName.SelectedIndex = modelIndex;
				_saveAsName = currModel;
				SlsModModelProfile smmp = SAB.HierarchyServerSession.GetSlsModModelData(currModel);
				if (displayModel)
				{
					if (!_newModel && FunctionSecurity.AllowUpdate)
					{
						smmp = (SlsModModelProfile) SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SalesModifier, smmp.Key, true);
						if (smmp.ModelLockStatus == eLockStatus.ReadOnly)
						{
							FunctionSecurity.SetReadOnly(); 
						}
						else
							if (smmp.ModelLockStatus == eLockStatus.Cancel)
						{
							if (_currModel == null)
							{
                                return false;                                 
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
						Format_Title(dataState, eMIDTextCode.frm_SalesModifierModel,smmp.ModelID);
						_modelRID = smmp.Key;
						_currModel = smmp.ModelID;
						this.txtSalesModifierDefault.Text = smmp.SlsModModelDefault.ToString(CultureInfo.CurrentUICulture);
						_gridDataTable.Rows.Clear();
						foreach (SlsModModelEntry smme in smmp.ModelEntries)
						{
                            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                            //_gridDataTable.Rows.Add(new object[] {smme.SlsModModelEntryValue.ToString(CultureInfo.CurrentUICulture), smme.DateRange, smme.DateRange.DisplayDate});
                            _gridDataTable.Rows.Add(new object[] { smme.SlsModModelEntryValue, smme.DateRange, smme.DateRange.DisplayDate });
                            //End TT#169
						}
						this.ugModel.DataSource = _gridDataTable;

						

						// set the cell image if reoccurring
						int index = 0;
						foreach(  UltraGridRow gridRow in ugModel.Rows )
						{
							if (FunctionSecurity.AllowUpdate)
							{
								// do not allow user to key in date field
								gridRow.Cells["Valid Thru"].Activation = Activation.ActivateOnly;
							}
							else
							{
								gridRow.Cells["Valid Thru"].Activation = Activation.NoEdit;
							}

							SlsModModelEntry smme = (SlsModModelEntry)smmp.ModelEntries[index];
							//				gridRow.Cells["Valid Thru"].Tag = smme.DateRange;
							if (smme.DateRange.DateRangeType == eCalendarRangeType.Reoccurring)
							{
								gridRow.Cells["Valid Thru"].Appearance.Image = ReoccurringImage;
							}
							else
							{
								gridRow.Cells["Valid Thru"].Appearance.Image = null;
							}

							++index;
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
					Format_Title(dataState, eMIDTextCode.frm_SalesModifierModel, "");
					SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
				}
				if (currModel == "")
				{
					btnDelete.Enabled = false;
					btnSave.Enabled = false;
					btnSaveAs.Enabled = false;
                    btnInUse.Enabled = false;    //TT#110-MD-VStuart - In Use Tool
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

		private void SetText()
		{
			this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_SalesModifierModel);
		}

        //private void DetermineSecurity()
        //{
        //    try
        //    {
        //        if (FunctionSecurity.AllowDelete &&
        //            this.cbModelName.SelectedIndex > -1)
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
        //            btnSaveAs.Enabled = true;
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
				this.ugModel.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				this.ugModel.DisplayLayout.AddNewBox.Hidden = false;
				this.ugModel.DisplayLayout.AddNewBox.Prompt = "Add model row";
				this.ugModel.DisplayLayout.Bands[0].AddButtonCaption = "Date Information";
				this.ugModel.DisplayLayout.GroupByBox.Hidden = true;
				this.ugModel.DisplayLayout.GroupByBox.Prompt = "";
				this.ugModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugModel.DisplayLayout.Bands[0].Columns["Value"].Width = 75;
				this.ugModel.DisplayLayout.Bands[0].Columns["Valid Thru"].Width = 200;
				//this.ugModel.DisplayLayout.Bands[0].Columns["Value"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.;
				this.ugModel.DisplayLayout.Bands[0].Columns["Valid Thru"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				this.ugModel.DisplayLayout.Bands[0].Columns["Value"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
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
				this.ugModel.DisplayLayout.Bands[0].Columns["DateRange"].Hidden = true;
				this.ugModel.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
				this.ugModel.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
				this.ugModel.DisplayLayout.AddNewBox.Hidden = false;
				this.ugModel.DisplayLayout.AddNewBox.Prompt = "Add model row";
				this.ugModel.DisplayLayout.Bands[0].AddButtonCaption = "Date Information";
				this.ugModel.DisplayLayout.GroupByBox.Hidden = true;
				this.ugModel.DisplayLayout.GroupByBox.Prompt = "";
				this.ugModel.DisplayLayout.Bands[0].Columns["Valid Thru"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
				this.ugModel.DisplayLayout.Bands[0].Columns["Value"].CellAppearance.TextHAlign =  Infragistics.Win.HAlign.Right;
				
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
						gridRow.Cells["Valid Thru"].Activation = Activation.AllowEdit;
					}
					else
					{
						gridRow.Cells["Valid Thru"].Activation = Activation.NoEdit;
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
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //edDataColumn.DataType = System.Type.GetType("System.String");
                edDataColumn.DataType = System.Type.GetType("System.Double");
                //End TT#169
				edDataColumn.ColumnName = "Value";
				edDataColumn.Caption = "Value";
				edDataColumn.ReadOnly = false;
//Begin Track #4545 - JSmith - Does not allow duplicate values
//				edDataColumn.Unique = true;
				edDataColumn.Unique = false;
//End Track #4545
				_gridDataTable.Columns.Add(edDataColumn);

				edDataColumn = new DataColumn();
				edDataColumn.DataType = System.Type.GetType("System.Object");
				edDataColumn.ColumnName = "DateRange";
				edDataColumn.Caption = "DateRange";
				edDataColumn.ReadOnly = false;
				edDataColumn.Unique = false;
				_gridDataTable.Columns.Add(edDataColumn);

				edDataColumn = new DataColumn();
				edDataColumn.DataType = System.Type.GetType("System.String");
				edDataColumn.ColumnName = "Valid Thru";
				edDataColumn.Caption = "Valid Thru";
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
        //    _slsModModelProfileList = _SAB.HierarchyServerSession.GetSlsModModels();
        //    cbModelName.Items.Clear();

        //    foreach (ModelName modelName in _slsModModelProfileList.ArrayList)
        //    {
        //        cbModelName.Items.Add(
        //            new ModelNameCombo(modelName.Key, modelName.ModelID));
        //    }
        //}

        //private int LocateModelIndex(string aModelName)
        //{
        //    int modelIndex = -1;
        //    int i = 0;

        //    foreach (ModelName modelName in _slsModModelProfileList.ArrayList)
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
				if (_tableLoaded)
				{
					DataTable t = (DataTable)  ob; 
					ChangePending = true;
					btnSave.Enabled = true;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
//Begin     TT#1638 - MD - Revised Model Save - RBeck
//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
////				_cancelClicked = true;
//                Cancel_Click();
//            }
//            catch( Exception exception )
//            {
//                HandleException(exception);
//            }
////			_cancelClicked = true;
////			CheckForPendingChanges();
////			this.Close();
//        }

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
                        var emp = new SlsModModelProfile(_modelRID);
				        eProfileType type = emp.ProfileType;
				        int rid = _modelRID;

				        if (!CheckInUse(type, rid, false))
				        {
				            Cursor = Cursors.WaitCursor;
				            SlsModModelProfile smmp = new SlsModModelProfile(_modelRID);
				            smmp.ModelChangeType = eChangeType.delete;
				            smmp.Key = _modelRID;
				            SAB.HierarchyServerSession.SlsModModelUpdate(smmp);
				            _changeMade = true;
				            itemDeleted = true;
				            FormLoaded = false;
				            _gridDataTable.Rows.Clear();
				            this.txtSalesModifierDefault.Text = "1.0";
				            //_gridDataTable.Rows.Add(new object[] { "", ""});  TT#1638 - MD - Revised Model Save - RBeck
				            _gridDataTable.Rows.Add(new object[] {1, "", ""});

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
						string modelName = ((ModelName)_slsModModelProfileList[nextItem]).ModelID;
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

        // Begin TT#1638 - JSmith - Revised Model Save
        protected override DataTable GetFilteredModels(string salesModifierNameFilter, bool isCaseSensitive)
        {
            try
            {
                MerchandiseHierarchyData modelsData = new MerchandiseHierarchyData();

                return modelsData.GetFilteredSalesModifierModels(salesModifierNameFilter, isCaseSensitive);
            }
            catch (Exception ex)
            {
                HandleException(ex, "GetFilteredModels");
                throw ex;
            }
        }
        // End TT#1638 - JSmith - Revised Model Save

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
						layoutData.InfragisticsLayout_Save(SAB.ClientServerSession.UserRID, eLayoutID.salesModifierModelGrid, ugModel);
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
					SAB.HierarchyServerSession.DequeueModel(eModelType.SalesModifier, _modelRID);
				}
			}
			catch (Exception ex)
			{
				HandleException (ex, "frmSalesModifierModelMaint");
			}
		}

//Begin    TT#1638 - MD - Revised Model Save - RBeck
         protected override void ModelPreSave()
         {
            ErrorFound = false;
            string errorMessage = null;

            smmp = new SlsModModelProfile(_modelRID);

				if (_newModel)
				{
					smmp.ModelChangeType = eChangeType.add;
				}
				else
				{
					smmp.ModelChangeType = eChangeType.update;
				}

				if (this.txtSalesModifierDefault.Text != "")
				{
					if (!DefaultValueValid(this.txtSalesModifierDefault.Text, ref errorMessage))
					{
						defaultErrorFound = true;
						ErrorFound = true;
					}
					else
					{
						smmp.SlsModModelDefault = Convert.ToDouble(this.txtSalesModifierDefault.Text, CultureInfo.CurrentUICulture);
					}
				}
				else
				{
					smmp.SlsModModelDefault = 0;
				}

				smmp.ModelEntries = new ArrayList();
				int entrySeq = 0;
				foreach(  UltraGridRow gridRow in ugModel.Rows )
				{
                     if (Convert.IsDBNull(gridRow.Cells["Value"].Value) &&
                        (Convert.IsDBNull(gridRow.Cells["Valid Thru"].Value) || (string)gridRow.Cells["Valid Thru"].Value == ""))
                    {
                        break;
                    }

					SlsModModelEntry modelEntry = new SlsModModelEntry();

					if (!ValueValid(gridRow.Cells["Value"], ref errorMessage))
					{
						if (firstErrorCell == null)
						{
							firstErrorCell = gridRow.Cells["Value"];
							firstErrorRow = gridRow;
						}
						ErrorFound = true;
					}
					else
					{
						modelEntry.SlsModModelEntryValue = Convert.ToDouble(gridRow.Cells["Value"].Value, CultureInfo.CurrentUICulture);
					}

					if (!ValidThruValid(gridRow.Cells["Valid Thru"], ref errorMessage))
					{
						if (firstErrorCell == null)
						{
							firstErrorCell = gridRow.Cells["Valid Thru"];
							firstErrorRow = gridRow;
						}
						ErrorFound = true;
					}
					else
					{
						modelEntry.DateRange = (DateRangeProfile)gridRow.Cells["DateRange"].Value;
					}
				
					modelEntry.ModelEntrySeq = entrySeq;
					modelEntry.ModelEntryChangeType = eChangeType.add;
					smmp.ModelEntries.Add(modelEntry);
					++entrySeq;
				}
         }
        protected override void DefaultErrorFound(bool _defaultErrorFound)
		{
					DoValueByValueEdits = true;
					if (defaultErrorFound)
					{
						this.txtSalesModifierDefault.SelectionStart = 0;
						this.txtSalesModifierDefault.SelectionLength = this.txtSalesModifierDefault.Text.Length;
						this.txtSalesModifierDefault.Select();
						this.txtSalesModifierDefault.Focus();
					}
					else
					{
						ugModel.ActiveRow = firstErrorRow;
						ugModel.ActiveCell = firstErrorCell;
						ugModel.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
					}
					MessageBox.Show ("errors encountered",  this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
        protected override ProfileList GetModels()
        {
            _slsModModelProfileList = SAB.HierarchyServerSession.GetSlsModModels();
            return _slsModModelProfileList;
        }

        protected override void ModelType(int saveKey)
        {
            SAB.HierarchyServerSession.DequeueModel(eModelType.StockModifier, saveKey);
        }
        protected override void ModelText(string _saveAsName)
        {
            Format_Title(eDataState.Updatable, eMIDTextCode.frm_StockModifierModel, _saveAsName);
        }

        //BEGIN TT#110-MD-VStuart - In Use Tool
        public override void ShowInUse()
        {
            var emp = new SlsModModelProfile(_modelRID);
            eProfileType type = emp.ProfileType;
            int rid = _modelRID;
            base.ShowInUse(type, rid, inQuiry2);
        }
        //END TT#110-MD-VStuart - In Use Tool

        protected override void GetModelRID(string _saveAsName, ref int _modelRID)
        {
            smmp.ModelID = _saveAsName;
            smmp.Key = _modelRID;
            _modelRID = SAB.HierarchyServerSession.SlsModModelUpdate(smmp);
        }

        protected override void AlterModelChangeType(eChangeType changeType)
        {
            smmp.ModelChangeType = changeType;
        }

        protected override int GetProfileKey(string SaveAsName)
        {
            SlsModModelProfile checkExists = SAB.HierarchyServerSession.GetSlsModModelData(SaveAsName);
            return checkExists.Key;
        }
        //protected override string GetModelName(int _modelIndex)
        //{
        //    string modelName = ((ModelName)(_slsModModelProfileList[_modelIndex])).ModelID;
        //    return modelName;
        //}
		
        //override protected bool SaveChanges()
        //{
        //    try
        //    {
        //        bool saveAsCanceled = false;
        //        ErrorFound = false;
        //        bool defaultErrorFound = false;
        //        string errorMessage = null;
        //        UltraGridRow firstErrorRow = null;
        //        UltraGridCell firstErrorCell = null;
        //        int saveKey;

                //SlsModModelProfile smmp = new SlsModModelProfile(_modelRID);
                //if (_newModel)
                //{
                //    smmp.ModelChangeType = eChangeType.add;
                //}
                //else
                //{
                //    smmp.ModelChangeType = eChangeType.update;
                //}

                //if (this.txtSalesModifierDefault.Text != "")
                //{
                //    if (!DefaultValueValid(this.txtSalesModifierDefault.Text, ref errorMessage))
                //    {
                //        defaultErrorFound = true;
                //        ErrorFound = true;
                //    }
                //    else
                //    {
                //        smmp.SlsModModelDefault = Convert.ToDouble(this.txtSalesModifierDefault.Text, CultureInfo.CurrentUICulture);
                //    }
                //}
                //else
                //{
                //    smmp.SlsModModelDefault = 0;
                //}

                //smmp.ModelEntries = new ArrayList();
                //int entrySeq = 0;
                //foreach(  UltraGridRow gridRow in ugModel.Rows )
                //{
                //    //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //    //if ((Convert.IsDBNull(gridRow.Cells["Value"].Value) || (string)gridRow.Cells["Value"].Value == "") &&
                //    //    (Convert.IsDBNull(gridRow.Cells["Valid Thru"].Value) || (string)gridRow.Cells["Valid Thru"].Value == "") )
                //    //{
                //    //    break;
                //    //}
                //    if (Convert.IsDBNull(gridRow.Cells["Value"].Value) &&
                //        (Convert.IsDBNull(gridRow.Cells["Valid Thru"].Value) || (string)gridRow.Cells["Valid Thru"].Value == ""))
                //    {
                //        break;
                //    }
                //    //End TT#169

                //    SlsModModelEntry modelEntry = new SlsModModelEntry();

                //    if (!ValueValid(gridRow.Cells["Value"], ref errorMessage))
                //    {
                //        if (firstErrorCell == null)
                //        {
                //            firstErrorCell = gridRow.Cells["Value"];
                //            firstErrorRow = gridRow;
                //        }
                //        ErrorFound = true;
                //    }
                //    else
                //    {
                //        modelEntry.SlsModModelEntryValue = Convert.ToDouble(gridRow.Cells["Value"].Value, CultureInfo.CurrentUICulture);
                //    }

                //    if (!ValidThruValid(gridRow.Cells["Valid Thru"], ref errorMessage))
                //    {
                //        if (firstErrorCell == null)
                //        {
                //            firstErrorCell = gridRow.Cells["Valid Thru"];
                //            firstErrorRow = gridRow;
                //        }
                //        ErrorFound = true;
                //    }
                //    else
                //    {
                //        modelEntry.DateRange = (DateRangeProfile)gridRow.Cells["DateRange"].Value;
                //    }
				
                //    modelEntry.ModelEntrySeq = entrySeq;
                //    modelEntry.ModelEntryChangeType = eChangeType.add;
                //    smmp.ModelEntries.Add(modelEntry);
                //    ++entrySeq;
                //}

                //if (!ErrorFound)
                //{
                //    DoValueByValueEdits = false;

                //    if (_newModel && (_saveAsName == ""))
                //    {
                //        bool continueSave = false;
                //        frmSaveAs formSaveAs = new frmSaveAs(_SAB);
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
                //                SlsModModelProfile checkExists = _SAB.HierarchyServerSession.GetSlsModModelData(formSaveAs.SaveAsName);
                //                if (checkExists.Key == -1)
                //                {
                //                    _saveAsName = formSaveAs.SaveAsName;
                //                    continueSave = true;
                //                }
                //                else
                //                {
                //                    if (MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
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
                //                        smmp.ModelChangeType = eChangeType.update;
                //                        SlsModModelProfile smmp2 =  _SAB.HierarchyServerSession.GetSlsModModelData(formSaveAs.SaveAsName);
                //                        _modelRID = smmp2.Key;
                //                        _saveAsName = formSaveAs.SaveAsName;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                continueSave = true;
                //            }
                //        }
                    //}

                    //if (!saveAsCanceled)
                    //{
                        //smmp.ModelID = _saveAsName;
                        //smmp.Key = _modelRID;
                        //saveKey = _modelRID;
                        //_modelRID = _SAB.HierarchyServerSession.SlsModModelUpdate(smmp);
                        //_changeMade = true;
                        //ChangePending = false;
                        //btnSave.Enabled = false;
                        //if (_newModel)
                        //{
                        //    FormLoaded = false;
                        //    LoadModelComboBox();
                        //    if (_modelLocked &&
                        //        saveKey != Include.NoRID)
                        //    {
                        //        _SAB.HierarchyServerSession.DequeueModel(eModelType.SalesModifier, saveKey);
                        //        _modelLocked = false;
                        //    }
                        //    InitializeForm(_saveAsName);
                        //    FormLoaded = true;
                        //    Format_Title(eDataState.Updatable, eMIDTextCode.frm_SalesModifierModel, _saveAsName);
                        //    _newModel = false;
                        //}
                    //}
                    //else
                    //{
                    //    MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
                    //        MessageBoxButtons.OK, MessageBoxIcon.Information);
//						ChangePending = false;
//						if (cbModelName.Items.Count > 0)
//						{
//							string modelName = ((ModelName)(_slsModModelProfileList[0])).ModelID;
//							InitializeForm(modelName);
//						}
//						else
//						{
//							InitializeForm();
//						}
                //    }
                //    this.cbModelName.Enabled = true;
                //}		
                //else
                //{
                //    DoValueByValueEdits = true;
                //    if (defaultErrorFound)
                //    {
                //        this.txtSalesModifierDefault.SelectionStart = 0;
                //        this.txtSalesModifierDefault.SelectionLength = this.txtSalesModifierDefault.Text.Length;
                //        this.txtSalesModifierDefault.Select();
                //        this.txtSalesModifierDefault.Focus();
                //    }
                //    else
                //    {
                //        ugModel.ActiveRow = firstErrorRow;
                //        ugModel.ActiveCell = firstErrorCell;
                //        ugModel.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
                //    }
                //    MessageBox.Show ("errors encountered",  this.Text,
                //        MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }

        //    return true;
        //}

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
					if (e.Cell.Row.Cells["DateRange"].Value != null &&
						e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value &&
						e.Cell.Row.Cells["DateRange"].Text.Length > 0)
					{
						DateRangeProfile drp = (DateRangeProfile)e.Cell.Row.Cells["DateRange"].Value;
						frmCalDtSelector.DateRangeRID = drp.Key;
					}
					DialogResult DateRangeResult = frmCalDtSelector.ShowDialog();
					if (DateRangeResult == DialogResult.OK)
					{
						DateRangeProfile SelectedDateRange = (DateRangeProfile)frmCalDtSelector.Tag;
						e.Cell.Value = SelectedDateRange.DisplayDate;
						//				e.Cell.Tag = SelectedDateRange;
						// for some reason have to clear the cell before it can be updated??
						if (e.Cell.Row.Cells["DateRange"].Value != System.DBNull.Value)
						{
							e.Cell.Row.Cells["DateRange"].Value = System.DBNull.Value;
						}
						e.Cell.Row.Cells["DateRange"].Value = SelectedDateRange;
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
        //                string modelName = ((ModelName)(_slsModModelProfileList[_modelIndex])).ModelID;
        //                InitializeForm(modelName);
        //            }
        //        }
        //    }
        //    catch(Exception exception)
        //    {
        //        HandleException(exception);
        //    }
        //}
//End      TT#1638 - MD - Revised Model Save - RBeck
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

		private void txtSalesModifierDefault_Leave(object sender, System.EventArgs e)
		{
			try
			{
				string errorMessage = null;
				if (DoValueByValueEdits)
				{
					if (!DefaultValueValid(txtSalesModifierDefault.Text, ref errorMessage))
					{
						MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		
		private void ugModel_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
				string errorMessage = null;
				if (DoValueByValueEdits)
				{
					switch (this.ugModel.ActiveCell.Column.Key)
					{
						case "Value":
							if (!ValueValid(ugModel.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
						case "Valid Thru":
							if (!ValidThruValid(ugModel.ActiveCell, ref errorMessage))
							{
								MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
								e.Cancel = true;
							}
							break;
					}
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		
		private bool DefaultValueValid(string defaultValue, ref string errorMessage)
		{
			ErrorFound = false;
			try
			{
				if (defaultValue.Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorFound = true;
				}
				else
				{
					double cellValue = Convert.ToDouble(defaultValue, CultureInfo.CurrentUICulture);
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						ErrorFound = true;
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
				ErrorFound = true;
			}

			if (ErrorFound)
			{
				ErrorProvider.SetError (this.txtSalesModifierDefault,errorMessage);
				return false;
			}
			else
			{
				ErrorProvider.SetError (this.txtSalesModifierDefault,"");
				return true;
			}
		}

		private bool ValueValid(UltraGridCell gridCell, ref string errorMessage)
		{
			ErrorFound = false;
			try
			{
				if (Convert.IsDBNull(gridCell) || gridCell.Text.Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorFound = true;
				}
				else
				{
					double cellValue = Convert.ToDouble(gridCell.Text, CultureInfo.CurrentUICulture);
					if (cellValue < 0)
					{
						errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
						ErrorFound = true;
					}
				}
			}
			catch( Exception error)
			{
				string exceptionMessage = error.Message;
				errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNumeric);
				ErrorFound = true;
			}

			if (ErrorFound)
			{
				gridCell.Appearance.Image = ErrorImage;
				gridCell.Tag = errorMessage;
				return false;
			}
			else
			{
				gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				return true;
			}
		}

		private bool ValidThruValid(UltraGridCell gridCell, ref string errorMessage)
		{
			ErrorFound = false;
			try
			{
				if (Convert.IsDBNull(gridCell) || gridCell.Text.Length == 0)	// cell is empty
				{
					errorMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FieldIsRequired);
					ErrorFound = true;
				}
			}
			catch( Exception error)
			{
				errorMessage = error.Message;
				ErrorFound = true;
			}

			if (ErrorFound)
			{
				gridCell.Appearance.Image = this.ErrorImage;
				gridCell.Tag = errorMessage;
				return false;
			}
			else
			{
				gridCell.Appearance.Image = null;
				gridCell.Tag = null;
				return true;
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
//End     TT#1638 - MD - Revised Model Save - RBeck	
	
		#endregion

		private void ugModel_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (FormLoaded && _tableLoaded)
			{
				ChangePending = true;
				btnSave.Enabled = true;
			}
		}

		private void txtSalesModifierDefault_TextChanged(object sender, System.EventArgs e)
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
				e.Row.Cells["Valid Thru"].Activation = Activation.ActivateOnly;
			}
			catch( Exception error)
			{
				HandleException(error);
			}

		}
	}

}
