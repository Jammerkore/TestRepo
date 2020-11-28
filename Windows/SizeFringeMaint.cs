// begin MID Track 3619 Remove Fringe
//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//using System.Globalization;
//
//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;
//using Infragistics.Shared;
//
//using MIDRetail.Business;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//
//namespace MIDRetail.Windows
//{
//	/// <summary>
//	/// Summary description for SizeFringeMaint.
//	/// </summary>
//	public class SizeFringeMaint : MIDFormBase
//	{
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		/// 
//		private SessionAddressBlock _sab;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboModelName;
//		private System.Windows.Forms.Label label2;
//		private System.Windows.Forms.Button btnSave;
//		private System.Windows.Forms.Button btnNew;
//		private System.Windows.Forms.Button btnDelete;
//		private System.Windows.Forms.Button btnClose;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboSizeCurve;
//		private System.Windows.Forms.Label label1;
//		private Infragistics.Win.UltraWinGrid.UltraGrid ugFringeGrid;
//		private Infragistics.Win.UltraWinGrid.UltraGrid ugFilterGrid;
////		private SizeFringeModelProfile _sizeFringeModel;
//		private System.Windows.Forms.RadioButton rbAscending;
//		private System.Windows.Forms.RadioButton rbDescending;
//		private System.Windows.Forms.GroupBox groupBox1;
//
//		private System.ComponentModel.Container components = null;
//		private int _sizeFringeRid;
//		private string _saveAsName;
//		private bool _newModel;
//		private bool PromptSizeChange = false;
//		private SizeModelData _sizeModelData;
//		private ArrayList _hiddenColumns; 
//		private DataSet _dsFilter;
//		private ArrayList _errorMessages = new ArrayList();
//
//		public SizeModelData SizeModelData
//		{
//			get	
//			{
//				if (_sizeModelData == null)
//				{
//					_sizeModelData = new SizeModelData();
//				}
//				return _sizeModelData;
//			}
//		}
//
//		public SizeFringeMaint(SessionAddressBlock SAB) : base (SAB)
//		{
//			_sab = SAB;
////			InitializeComponent();
////			FunctionSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminSizeFringe);
//		}
//
//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			if( disposing )
//			{
//				if(components != null)
//				{
//					components.Dispose();
//				}
//				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
//				this.btnNew.Click -= new System.EventHandler(this.btnNew_Click);
//				this.btnDelete.Click -= new System.EventHandler(this.btnDelete_Click);
//				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
//				this.cboModelName.SelectionChangeCommitted -= new System.EventHandler(this.cboModelName_SelectionChangeCommitted);
//				this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
//				this.ugFringeGrid.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugFringeGrid_AfterCellUpdate);
//				this.ugFringeGrid.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugFringeGrid_InitializeLayout);
//				this.ugFilterGrid.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugFilterGrid_AfterRowInsert);
//				this.ugFilterGrid.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugFilterGrid_InitializeLayout);
//				this.Load -= new System.EventHandler(this.SizeFringeMaint_Load);
//
//			}
//			base.Dispose( disposing );
//		}
//
//		#region Windows Form Designer generated code
//		/// <summary>
//		/// Required method for Designer support - do not modify
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.btnSave = new System.Windows.Forms.Button();
//			this.btnNew = new System.Windows.Forms.Button();
//			this.btnDelete = new System.Windows.Forms.Button();
//			this.btnClose = new System.Windows.Forms.Button();
//			this.cboModelName = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.cboSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.label2 = new System.Windows.Forms.Label();
//			this.label1 = new System.Windows.Forms.Label();
//			this.ugFringeGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
//			this.ugFilterGrid = new Infragistics.Win.UltraWinGrid.UltraGrid();
//			this.rbAscending = new System.Windows.Forms.RadioButton();
//			this.rbDescending = new System.Windows.Forms.RadioButton();
//			this.groupBox1 = new System.Windows.Forms.GroupBox();
//			((System.ComponentModel.ISupportInitialize)(this.ugFringeGrid)).BeginInit();
//			((System.ComponentModel.ISupportInitialize)(this.ugFilterGrid)).BeginInit();
//			this.SuspendLayout();
//			// 
//			// btnSave
//			// 
//			this.btnSave.Location = new System.Drawing.Point(360, 32);
//			this.btnSave.Name = "btnSave";
//			this.btnSave.TabIndex = 2;
//			this.btnSave.Text = "&Save";
//			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
//			// 
//			// btnNew
//			// 
//			this.btnNew.Location = new System.Drawing.Point(448, 32);
//			this.btnNew.Name = "btnNew";
//			this.btnNew.TabIndex = 3;
//			this.btnNew.Text = "&New";
//			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
//			// 
//			// btnDelete
//			// 
//			this.btnDelete.Location = new System.Drawing.Point(536, 32);
//			this.btnDelete.Name = "btnDelete";
//			this.btnDelete.TabIndex = 4;
//			this.btnDelete.Text = "&Delete";
//			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
//			// 
//			// btnClose
//			// 
//			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//			this.btnClose.Location = new System.Drawing.Point(536, 528);
//			this.btnClose.Name = "btnClose";
//			this.btnClose.TabIndex = 5;
//			this.btnClose.Text = "&Close";
//			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
//			// 
//			// cboModelName
//			// 
//			this.cboModelName.Location = new System.Drawing.Point(104, 32);
//			this.cboModelName.Name = "cboModelName";
//			this.cboModelName.Size = new System.Drawing.Size(232, 21);
//			this.cboModelName.TabIndex = 6;
//			this.cboModelName.Text = "comboBox1";
//			this.cboModelName.SelectionChangeCommitted += new System.EventHandler(this.cboModelName_SelectionChangeCommitted);
//			// 
//			// cboSizeCurve
//			// 
//			this.cboSizeCurve.Location = new System.Drawing.Point(104, 64);
//			this.cboSizeCurve.Name = "cboSizeCurve";
//			this.cboSizeCurve.Size = new System.Drawing.Size(232, 21);
//			this.cboSizeCurve.TabIndex = 7;
//			this.cboSizeCurve.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
//			// 
//			// label2
//			// 
//			this.label2.Location = new System.Drawing.Point(24, 64);
//			this.label2.Name = "label2";
//			this.label2.Size = new System.Drawing.Size(64, 23);
//			this.label2.TabIndex = 8;
//			this.label2.Text = "Size Curve";
//			// 
//			// label1
//			// 
//			this.label1.Location = new System.Drawing.Point(24, 40);
//			this.label1.Name = "label1";
//			this.label1.Size = new System.Drawing.Size(72, 16);
//			this.label1.TabIndex = 0;
//			this.label1.Text = "Model Name";
//			// 
//			// ugFringeGrid
//			// 
//			this.ugFringeGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
//				| System.Windows.Forms.AnchorStyles.Left) 
//				| System.Windows.Forms.AnchorStyles.Right)));
//			this.ugFringeGrid.Location = new System.Drawing.Point(8, 120);
//			this.ugFringeGrid.Name = "ugFringeGrid";
//			this.ugFringeGrid.Size = new System.Drawing.Size(608, 208);
//			this.ugFringeGrid.TabIndex = 9;
//			this.ugFringeGrid.AfterExitEditMode += new System.EventHandler(this.ugFringeGrid_AfterExitEditMode);
//			this.ugFringeGrid.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugFringeGrid_AfterCellUpdate);
//			this.ugFringeGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugFringeGrid_InitializeLayout);
//			// 
//			// ugFilterGrid
//			// 
//			this.ugFilterGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
//				| System.Windows.Forms.AnchorStyles.Right)));
//			this.ugFilterGrid.Location = new System.Drawing.Point(8, 344);
//			this.ugFilterGrid.Name = "ugFilterGrid";
//			this.ugFilterGrid.Size = new System.Drawing.Size(608, 168);
//			this.ugFilterGrid.TabIndex = 10;
//			this.ugFilterGrid.Text = "Fringe Filter";
//			this.ugFilterGrid.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugFilterGrid_MouseEnterElement);
//			this.ugFilterGrid.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugFilterGrid_AfterRowInsert);
//			this.ugFilterGrid.AfterExitEditMode += new System.EventHandler(this.ugFilterGrid_AfterExitEditMode);
//			this.ugFilterGrid.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugFilterGrid_InitializeLayout);
//			// 
//			// rbAscending
//			// 
//			this.rbAscending.Location = new System.Drawing.Point(368, 80);
//			this.rbAscending.Name = "rbAscending";
//			this.rbAscending.Size = new System.Drawing.Size(80, 24);
//			this.rbAscending.TabIndex = 11;
//			this.rbAscending.Text = "Ascending";
//			this.rbAscending.CheckedChanged += new System.EventHandler(this.rbAscending_CheckedChanged);
//			// 
//			// rbDescending
//			// 
//			this.rbDescending.Location = new System.Drawing.Point(448, 80);
//			this.rbDescending.Name = "rbDescending";
//			this.rbDescending.TabIndex = 12;
//			this.rbDescending.Text = "Descending";
//			this.rbDescending.CheckedChanged += new System.EventHandler(this.rbDescending_CheckedChanged);
//			// 
//			// groupBox1
//			// 
//			this.groupBox1.Location = new System.Drawing.Point(360, 64);
//			this.groupBox1.Name = "groupBox1";
//			this.groupBox1.Size = new System.Drawing.Size(248, 48);
//			this.groupBox1.TabIndex = 13;
//			this.groupBox1.TabStop = false;
//			this.groupBox1.Text = "Fringe Sort Sequence";
//			// 
//			// SizeFringeMaint
//			// 
//			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//			this.ClientSize = new System.Drawing.Size(624, 566);
//			this.Controls.Add(this.rbDescending);
//			this.Controls.Add(this.rbAscending);
//			this.Controls.Add(this.ugFilterGrid);
//			this.Controls.Add(this.ugFringeGrid);
//			this.Controls.Add(this.label2);
//			this.Controls.Add(this.cboSizeCurve);
//			this.Controls.Add(this.cboModelName);
//			this.Controls.Add(this.btnClose);
//			this.Controls.Add(this.btnDelete);
//			this.Controls.Add(this.btnNew);
//			this.Controls.Add(this.btnSave);
//			this.Controls.Add(this.label1);
//			this.Controls.Add(this.groupBox1);
//			this.Name = "SizeFringeMaint";
//			this.Text = "Size Fringe Model Maintenance";
//			this.Load += new System.EventHandler(this.SizeFringeMaint_Load);
//			((System.ComponentModel.ISupportInitialize)(this.ugFringeGrid)).EndInit();
//			((System.ComponentModel.ISupportInitialize)(this.ugFilterGrid)).EndInit();
//			this.ResumeLayout(false);
//
//		}
//		#endregion
//
//		private void SizeFringeMaint_Load(object sender, System.EventArgs e)
//		{
//			try
//			{		
//				base.FormLoaded = false;
//
////				if (FunctionSecurity.AllowUpdate)
////				{
////					Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeFringeModel, null);						
////				}
////				else
////				{
////					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_SizeFringeModel, null);
////					this.ugFringeGrid.Enabled = false;
////					this.ugFilterGrid.Enabled = false;
////				}
////
////				BindModelNameComboBox();
////				BindSizeCurveComboBox();
////
////				_sizeFringeRid = Include.NoRID;
////				if (cboModelName.SelectedIndex != Include.NoRID)
////				{
////					DataRowView aRow = (DataRowView)cboModelName.SelectedItem;
////					_sizeFringeRid = Convert.ToInt32(aRow.Row["SIZE_FRINGE_RID"]);	
////				}
////
////				SetReadOnly(FunctionSecurity.AllowUpdate);
////				this.cboModelName.Enabled = true;
////
////				if (_sizeFringeRid == Include.NoRID)
////					InitializeForm();
////				else
////				{
////					InitializeForm(_sizeFringeRid);
////				}
////				//===========================================
////				// Specific security setting for this form
////				//===========================================
////				if (FunctionSecurity.AllowDelete && _sizeFringeRid != Include.NoRID)
////					btnDelete.Enabled = true;
//
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex, "SizeFringeMaint_Load");
//			}
//		}
//
//
//		public void InitializeForm()
//		{
////			try
////			{
////				btnDelete.Enabled = false;
////				_newModel = true;
////				_sizeFringeModel = new SizeFringeModelProfile(Include.NoRID);
////				_saveAsName = "";
////
////				//SetReadOnly(true);
////				PromptSizeChange = false;
////
////				this.cboModelName.SelectedIndex = -1;
////				this.cboModelName.Text = "(new model)";
////				this.cboModelName.Enabled = false;
////
////				cboSizeCurve.SelectedIndex = 0;
////				_sizeFringeModel.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);
////
////				rbAscending.Checked = true;
////
////				BindFringeGrid();
////				BindFilterGrid();
////
////				Format_Title(eDataState.New, eMIDTextCode.frm_SizeFringeModel, null);
////				PromptSizeChange = true;
////				ChangePending = false;
////				FormLoaded = true;
////			}
////			catch(Exception exception)
////			{
////				HandleException(exception);
////			}
//		}
//
//
//		public void InitializeForm(int modelRid)
//		{
////			try
////			{
////				if (FunctionSecurity.AllowDelete)
////					btnDelete.Enabled = true;
////
////				_newModel = false;
////				_sizeFringeModel = new SizeFringeModelProfile(modelRid);
////				_saveAsName = "";
////				PromptSizeChange = false;
////
////				cboSizeCurve.SelectedValue = _sizeFringeModel.SizeCurveGroupRid;
////				if (_sizeFringeModel.SortOrder == eFringeOverrideSort.Ascending)
////					rbAscending.Checked = true;
////				else
////					rbDescending.Checked = true;
////
////				BindFringeGrid();
////				BindFilterGrid();
////
////				Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeFringeModel, null);
////				PromptSizeChange = true;
////				ChangePending = false;
////				FormLoaded = true;
////			}
////			catch(Exception exception)
////			{
////				HandleException(exception);
////			}
//		}
//
//		private void BindModelNameComboBox()
//		{
////			try
////			{
////				DataTable dtSizeModel = SizeModelData.SizeFringeModel_Read();
////				cboModelName.DataSource = dtSizeModel;
////				cboModelName.DisplayMember = "SIZE_FRINGE_NAME";
////				cboModelName.ValueMember = "SIZE_FRINGE_RID";
////			}		
////			catch (Exception ex)
////			{
////				HandleException(ex, "BindModelNameComboBox");
////			}
//		}
//
//		/// <summary>
//		/// Populates the size curve combo box.
//		/// </summary>
//		private void BindSizeCurveComboBox()
//		{
//			try
//			{
//				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
//				SizeCurve objSizeCurve = new SizeCurve();
//				dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
//				//dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
//				
//				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
//				ClearSizeCurveComboBox();
//
//				cboSizeCurve.DataSource = dvSizeCurve; //dtSizeCurve;
//				cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
//				cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
//			}		
//			catch (Exception ex)
//			{
//				HandleException(ex, "BindSizeCurveComboBox");
//			}
//		}
//
//		/// <summary>
//		/// Clears the size curve combo box.
//		/// </summary>
//		private void ClearSizeCurveComboBox()
//		{
//			try
//			{
//				cboSizeCurve.DataSource = null;
//				cboSizeCurve.Items.Clear();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "SizeFringeMaint.ClearSizeCurveComboBox");
//			}	
//		}
//
//		private void BindFringeGrid() 
//		{
////			try
////			{
////				if (_sizeFringeModel.SizeCurveGroupRid != Include.NoRID)
////				{
////					System.Collections.ArrayList sizeAL = new ArrayList(); 
////					System.Collections.ArrayList sizeID = new ArrayList();
////					System.Collections.ArrayList widthAL = new ArrayList(); 
////					System.Collections.ArrayList sizeKeys = new ArrayList(); 
////					System.Collections.ArrayList widthKeys = new ArrayList(); 
////
////					System.Collections.Hashtable bothHash = new Hashtable(); 
////					SizeCurveGroupProfile scgp = null;
////					string productCatStr = string.Empty;
////					if (_hiddenColumns == null)
////						_hiddenColumns = new ArrayList();
////					else
////						_hiddenColumns.Clear();
////					string tableName = "Fringe";
////			
////					// is there a size group?
////					if (_sizeFringeModel.SizeCurveGroupRid != Include.NoRID) 
////					{
////						// load existing group
////						scgp = new SizeCurveGroupProfile(_sizeFringeModel.SizeCurveGroupRid);
////						scgp.Resequence();
////						ProfileList sizeCodeList = scgp.GetSizeCodeList();
////						productCatStr = ((SizeCodeProfile)(sizeCodeList.ArrayList[0])).SizeCodeProductCategory;
////
////						foreach(SizeCodeProfile scp in sizeCodeList.ArrayList) 
////						{
////							if (scp.Key == -1) 
////							{
////								throw new MIDException (eErrorLevel.severe,
////									(int)eMIDTextCode.msg_CantRetrieveSizeCode,
////									MIDText.GetText(eMIDTextCode.msg_CantRetrieveSizeCode));
////							}
////							if (!sizeID.Contains(scp.SizeCodeID))
////							{
////								sizeID.Add(scp.SizeCodeID);
////								if (! sizeAL.Contains(scp.SizeCodePrimary))
////								{
////									sizeAL.Add(scp.SizeCodePrimary);
////									sizeKeys.Add(scp.SizeCodePrimaryRID);
////								}
////								if (! widthAL.Contains(scp.SizeCodeSecondary)) 
////								{
////									widthAL.Add(scp.SizeCodeSecondary);
////									widthKeys.Add(scp.SizeCodeSecondaryRID);
////								}
////							}
////						}
////						
////					}
////			
////
////					//==============================
////					// Build columns
////					//===============================
////					DataTable dtSizes = MIDEnvironment.CreateDataTable(tableName);
////					dtSizes.Columns.Add("   ");
////					dtSizes.Columns.Add("Dimension");
////					dtSizes.Columns.Add("DimensionHIDDEN");
////					dtSizes.Columns["DimensionHIDDEN"].DataType = System.Type.GetType("System.Int32");
////					_hiddenColumns.Add("DimensionHIDDEN");
////					dtSizes.Columns.Add("DimensionHIDDENTYPE");
////					dtSizes.Columns["DimensionHIDDENTYPE"].DataType = System.Type.GetType("System.Int32");
////					_hiddenColumns.Add("DimensionHIDDENTYPE");
////					foreach(string size in sizeAL) 
////					{
////						dtSizes.Columns.Add(size);
////						dtSizes.Columns[size].DataType = System.Type.GetType("System.String");
////						dtSizes.Columns.Add(size+"HIDDEN");
////						dtSizes.Columns[size+"HIDDEN"].DataType = System.Type.GetType("System.Int32");
////						_hiddenColumns.Add(size+"HIDDEN");
////						dtSizes.Columns.Add(size+"HIDDENTYPE");
////						dtSizes.Columns[size+"HIDDENTYPE"].DataType = System.Type.GetType("System.Int32");
////						_hiddenColumns.Add(size+"HIDDENTYPE");
////
////					}
////
////					//=======================================================
////					// Builds grid when there is no seconday size (dimension)
////					//=======================================================
////					string width = (string)widthAL[0];
////					if ((widthAL.Count == 1) && ((width == "No Secondary Size" || width.Trim() == string.Empty)))
////					{
////						ArrayList al = new ArrayList();
////						al.Add("Size");
////						al.Add("");
////						al.Add(-1);
////						al.Add(-1);
////				
////						foreach(string size in sizeAL) 
////						{
////							if (scgp == null) 
////							{
////								al.Add("");
////								al.Add(-1);
////								al.Add(-1);
////							}
////							else 
////							{
////								SizeCodeList scl = this._sab.HierarchyServerSession.GetSizeCodeList(
////									productCatStr, eSearchContent.WholeField,
////									size, eSearchContent.WholeField, 
////									width, eSearchContent.WholeField);
////								if (scl.Count == 1) 
////								{
////									al.Add(_sizeFringeModel.GetFringeInd(((SizeCodeProfile)(scl.ArrayList[0])).Key, eEquateOverrideSizeType.DimensionSize));
////									al.Add(((SizeCodeProfile)(scl.ArrayList[0])).Key);
////									al.Add((int)eEquateOverrideSizeType.DimensionSize);
////								}
////								else 
////								{
////									al.Add("");
////									al.Add(-1);
////									al.Add(-1);
////								}
////							}
////						}
////						object[] myArray = new object[al.Count]; 
////						al.CopyTo(myArray);
////						dtSizes.Rows.Add(myArray);
////					}
////						//==================================
////						// Builds Size / Dimension grid
////						//==================================
////					else
////					{
////
////						// Size Row
////						ArrayList al = new ArrayList();
////						al.Add("Size");
////						al.Add("");
////						al.Add(-1);
////						al.Add(-1);
////						for (int s=0;s<sizeAL.Count;s++)
////						{
////							al.Add(_sizeFringeModel.GetFringeInd((int)sizeKeys[s], eEquateOverrideSizeType.Size));
////							al.Add(sizeKeys[s]);
////							al.Add((int)eEquateOverrideSizeType.Size);
////						}
////						object[] myArray = new object[al.Count]; 
////						al.CopyTo(myArray);
////						dtSizes.Rows.Add(myArray);
////
////						// dimension rows
////						for (int w=0;w<widthAL.Count;w++)
////						{
////							width = (string)widthAL[w];
////							int widthKey = (int)widthKeys[w];
////
////							al = new ArrayList();
////							al.Add(width);
////
////							al.Add(_sizeFringeModel.GetFringeInd(widthKey, eEquateOverrideSizeType.Dimensions));
////							al.Add(widthKey);
////							al.Add((int)eEquateOverrideSizeType.Dimensions);
////							int totalColumn = al.Count - 1;
////							foreach(string size in sizeAL) 
////							{
////								if (scgp == null) 
////								{
////									al.Add("");
////									al.Add(-1);
////									al.Add(-1);
////								}
////								else 
////								{
////									SizeCodeList scl = this._sab.HierarchyServerSession.GetSizeCodeList(
////										productCatStr, eSearchContent.WholeField,
////										size, eSearchContent.WholeField, 
////										width, eSearchContent.WholeField);
////									if (scl.Count == 1) 
////									{
////										al.Add(_sizeFringeModel.GetFringeInd(((SizeCodeProfile)(scl.ArrayList[0])).Key, eEquateOverrideSizeType.DimensionSize));
////										al.Add(((SizeCodeProfile)(scl.ArrayList[0])).Key);
////										al.Add((int)eEquateOverrideSizeType.DimensionSize);
////									}
////									else 
////									{
////										al.Add("");
////										al.Add(-1);
////										al.Add(-1);
////									}
////								}
////								//					}
////							}
////							myArray = new object[al.Count]; 
////							al.CopyTo(myArray);
////							dtSizes.Rows.Add(myArray);
////						}
////					}
////					_sizeFringeModel.CleanupFringeList();
////					DataSet ds = MIDEnvironment.CreateDataSet();
////					ds.Tables.Add(dtSizes);
////					ugFringeGrid.BeginUpdate();
////					ugFringeGrid.DataSource = null;
////					ugFringeGrid.DataSource = dtSizes;
////					ugFringeGrid.EndUpdate();
////				}
////				else
////				{
////					ugFringeGrid.DataSource = null;
////				}
////			}
////			catch
////			{
////				throw;
////			}
//		}
//
//		private void BindFilterGrid() 
//		{
////			DataSet dsFilter = BuildFilterDataSet();
////			int cnt = _sizeFringeModel.FilterList.Count;
////
////			for (int f=0;f<cnt;f++)
////			{
////				object[] aRow = new object[5];
////
////				SizeFringeFilter sff = (SizeFringeFilter)_sizeFringeModel.FilterList[f];
////				aRow[0] = (int)sff.Criteria;
////				aRow[1] = (int)sff.Condition;
////				aRow[2] = sff.Value;
////				aRow[3] = (int)sff.ValueType;
////
////				if(f == (cnt-1))
////					aRow[4] = string.Empty;
////				else
////					aRow[4] = "Or";
////
////				dsFilter.Tables["Filter"].Rows.Add(aRow);
////			}
////
////			ugFilterGrid.DataSource = dsFilter;
//		}
//
//
//		private void ugFringeGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//		{
////			ugFringeGrid.DisplayLayout.Reset();
////			ugFringeGrid.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False; 
////
////			ugFringeGrid.DisplayLayout.ValueLists.Clear();
////			ugFringeGrid.DisplayLayout.ValueLists.Add("Fringe");
////
////			// Fills value lists for Size Group
////			if (_sizeFringeModel.SizeCurveGroupRid != Include.NoRID)
////			{
////				ugFringeGrid.DisplayLayout.ValueLists["Fringe"].ValueListItems.Add(-1, " ");
////				ugFringeGrid.DisplayLayout.ValueLists["Fringe"].ValueListItems.Add(1, "Fringe");
////				ugFringeGrid.DisplayLayout.ValueLists["Fringe"].ValueListItems.Add(0, "Never Fringe");				
////			}
////
////			//====================================
////			// Assigns Value lists to columns
////			//====================================
//////			int sizeTypeNum = 0;
//////			eEquateOverrideSizeType sizeType;
////			for (int r=0;r<ugFringeGrid.Rows.Count;r++)
////			{
////				for (int c=1;c<ugFringeGrid.Rows[r].Cells.Count;c++)
////				{
////					UltraGridColumn aColumn = ugFringeGrid.Rows[r].Cells[c].Column;
////					if (!_hiddenColumns.Contains(aColumn.Header.Caption))
////					{
////						ugFringeGrid.Rows[r].Cells[c].ValueList = ugFringeGrid.DisplayLayout.ValueLists["Fringe"];
////					}
////				}
////			}
////
////			//=======================
////			// Hides HIDDEN columns
////			//=======================
////			foreach(string aColumn in _hiddenColumns) 
////			{
////				this.ugFringeGrid.DisplayLayout.Bands[0].Columns[aColumn].Hidden = true;
////			}
////
////			// center values
////			for (int c=1;c<ugFringeGrid.DisplayLayout.Bands[0].Columns.Count;c++)
////			{
////				ugFringeGrid.DisplayLayout.Bands[0].Columns[c].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
////			}
////
////			// disable and set color to first column.
////			ugFringeGrid.DisplayLayout.Bands[0].Columns[0].CellActivation = Activation.Disabled;
////			ugFringeGrid.DisplayLayout.Bands[0].Columns[0].CellAppearance.BackColor = this.BackColor;
////			ugFringeGrid.DisplayLayout.Bands[0].Columns[0].CellAppearance.ForeColorDisabled = this.ForeColor;
////			// disable and set color to first row and second column.
////			if (ugFringeGrid.Rows.Count > 0)
////			{
////				ugFringeGrid.Rows[0].Cells[1].Appearance.BackColor = this.BackColor;
////				ugFringeGrid.Rows[0].Cells[1].Appearance.ForeColor = this.ForeColor;
////				ugFringeGrid.Rows[0].Cells[1].Activation = Activation.Disabled;
////			}
////
////			// Splitter
////			this.ugFringeGrid.DisplayLayout.MaxColScrollRegions = 2;
////			int colScrollWidth = this.ugFringeGrid.DisplayLayout.Bands[0].Columns[0].Width;
////			this.ugFringeGrid.DisplayLayout.ColScrollRegions[0].Width = colScrollWidth;
////			this.ugFringeGrid.DisplayLayout.ColScrollRegions[0].Split(this.ugFringeGrid.DisplayLayout.ColScrollRegions[0].Width);
////			this.ugFringeGrid.DisplayLayout.Bands[0].Columns[0].Header.ExclusiveColScrollRegion = this.ugFringeGrid.DisplayLayout.ColScrollRegions[0];
//
//		}
//
//		private void ugFilterGrid_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
//		{
////			ugFilterGrid.DisplayLayout.Reset();
////			ugFilterGrid.DisplayLayout.AddNewBox.Hidden = false;
////
////			ugFilterGrid.DisplayLayout.ValueLists.Clear();
////			ugFilterGrid.DisplayLayout.ValueLists.Add("Criteria");
////			ugFilterGrid.DisplayLayout.ValueLists.Add("Condition");
////			ugFilterGrid.DisplayLayout.ValueLists.Add("Type");
////
////			base.FillTextTypeList(ugFilterGrid.DisplayLayout.ValueLists["Criteria"],
////				eMIDTextType.eFringeOverrideUnitCriteria, eMIDTextOrderBy.TextValue);
////			base.FillTextTypeList(ugFilterGrid.DisplayLayout.ValueLists["Condition"],
////				eMIDTextType.eFringeOverrideCondition, eMIDTextOrderBy.TextValue);
////			base.FillTextTypeList(ugFilterGrid.DisplayLayout.ValueLists["Type"],
////				eMIDTextType.eFringeFilterValueType, eMIDTextOrderBy.TextValue);
////
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Criteria"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Criteria"].ValueList = ugFilterGrid.DisplayLayout.ValueLists["Criteria"];
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Criteria"].Width += ugFilterGrid.DisplayLayout.Bands[0].Columns["Criteria"].Width;
////
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Condition"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Condition"].ValueList = ugFilterGrid.DisplayLayout.ValueLists["Condition"];
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Condition"].CellAppearance.TextHAlign = HAlign.Center;
////
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Type"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Type"].ValueList = ugFilterGrid.DisplayLayout.ValueLists["Type"];
////
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Value"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Default;
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Value"].CellActivation = Activation.AllowEdit;
////
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Operator"].Header.Caption = " ";
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Operator"].Width = 30;
////			ugFilterGrid.DisplayLayout.Bands[0].Columns["Operator"].CellActivation = Activation.NoEdit;
////
////			ugFilterGrid.DisplayLayout.Bands[0].AddButtonCaption = "Fringe Filter";
//
//		}
//
//		private DataSet BuildFilterDataSet()
//		{
//			_dsFilter = MIDEnvironment.CreateDataSet("Filter");
//
//			DataTable dtFilter= MIDEnvironment.CreateDataTable("Filter");
//			dtFilter.Columns.Add("Criteria");
//			dtFilter.Columns["Criteria"].DataType = System.Type.GetType("System.Int32");
//			dtFilter.Columns.Add("Condition");
//			dtFilter.Columns["Condition"].DataType = System.Type.GetType("System.Int32");
//			dtFilter.Columns.Add("Value");
//			dtFilter.Columns["Value"].DataType = System.Type.GetType("System.Decimal");
//			dtFilter.Columns.Add("Type");
//			dtFilter.Columns["Type"].DataType = System.Type.GetType("System.Int32");
//			dtFilter.Columns.Add("Operator");
//
//			_dsFilter.Tables.Add(dtFilter);
//
//			return _dsFilter;
//		}
//
//		private void btnSave_Click(object sender, System.EventArgs e)
//		{
//			ISave();
//		}
//
//		private void btnNew_Click(object sender, System.EventArgs e)
//		{
//			try
//			{
//				FormLoaded = false;
//				CheckForPendingChanges();
//				InitializeForm();
//			}
//			catch(Exception exception)
//			{
//				HandleException(exception);
//			}
//		}
//
//		private void btnDelete_Click(object sender, System.EventArgs e)
//		{
//			IDelete();
//		}
//
//		private void btnClose_Click(object sender, System.EventArgs e)
//		{
//			Close();
//		}
//
//		private void cboModelName_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
////			try
////			{
////				if (FormLoaded)
////				{
////					CheckForPendingChanges();
////					if (cboModelName.SelectedIndex >= 0)
////					{
////						DataRowView aRow = (DataRowView)cboModelName.SelectedItem;
////						_sizeFringeModel.Key = Convert.ToInt32(aRow.Row["SIZE_FRINGE_RID"]);	
////						InitializeForm(_sizeFringeModel.Key);
////					}
////				}
////			}
////			catch(Exception exception)
////			{
////				HandleException(exception);
////			}
//		}
//
//		private void cboSizeCurve_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
////			try
////			{
////				if (base.FormLoaded)
////				{
////					if (PromptSizeChange)
////					{
////						
////						if (ShowWarningPrompt(true) == DialogResult.Yes)
////						{
////							//_basisSizeMethod.DeleteMethodRules(new TransactionData());
////
////							_sizeFringeModel.SizeCurveGroupRid = Convert.ToInt32(cboSizeCurve.SelectedValue.ToString(),CultureInfo.CurrentUICulture);;
////							//_basisSizeMethod.SubstituteList.Clear();  // Size group has changed, wipe out previous data
////							BindFringeGrid();
////							ChangePending = true;
////						}
////
////						else
////						{
////							//Shut off the prompt so the combo can be reset to original value.
////							PromptSizeChange = false;
////							cboSizeCurve.SelectedValue = _sizeFringeModel.SizeCurveGroupRid;
////						}
////					}
////					else
////					{
////						//Turn the prompt back on.
////						PromptSizeChange = true;
////					}
////				}
////			}
////			catch (Exception ex)
////			{
////				HandleException(ex, "cboSizeCurve_SelectionChangeCommitted");
////			}
//		}
//
//		protected DialogResult ShowWarningPrompt(bool basisSubstitute)
//		{
//			try
//			{
//				DialogResult drResult;
//				drResult = DialogResult.Yes;
//				ChangePending = true;
//				//ConstraintRollback = true;
//
//				string msg = "Warning:\nChanging this value will cause the current Fringe ";
//				msg += "information to be immediately erased." +
//					"\nTo return to the original constraint information close the form without saving or updating.\nDo you wish to continue?";
//
//				drResult = MessageBox.Show(msg,	"Confirmation", MessageBoxButtons.YesNo);
//
//				if (drResult == DialogResult.Yes)
//				{
//					ChangePending = true;
//					//ConstraintRollback = true;
//				}
//
//				return drResult;
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "SizeFringeMaint.ShowWarningPrompt");
//				return DialogResult.No;
//			}	
//		}
//
//
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
//
//		override public void ISave()
//		{
//			try
//			{
//				this.Cursor = Cursors.WaitCursor;
//				if (Validation())
//				{
//					SaveChanges();
//				}
//			}		
//			catch(Exception ex)
//			{
//				MessageBox.Show(ex.Message);
//			}
//			finally
//			{
//				this.Cursor = Cursors.Default;
//			}
//		}
//
//		override public void ISaveAs()
//		{
//			
//		}
//
//		override public void IDelete()
//		{
////			TransactionData td = new TransactionData();
////			try
////			{
////				int currIndex = cboModelName.SelectedIndex;
////				bool itemDeleted = false;
////				if (this._sizeFringeModel.Key != Include.NoRID)
////				{
////					string text = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConfirmDelete);
////					text =  text.Replace("{0}", "Size Fringe Model: " + _sizeFringeModel.SizeFringeName);
////					if (MessageBox.Show (text,  this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
////						== DialogResult.Yes) 
////					{
////						_sizeFringeModel.ModelChangeType = eChangeType.delete;
////						td.OpenUpdateConnection();
////						// This uses the Key(SizeAltRid) as defined in the SizeAlternateProfile clase
////						DeleteSizeFringeChildren(td);
////						this.SizeModelData.SizeFringeModel_Delete(this._sizeFringeModel.Key, td);
////						td.CommitData();
////						DataTable dt = (DataTable)cboModelName.DataSource;
////						DataRow [] rows = dt.Select("SIZE_FRINGE_RID = " + _sizeFringeModel.Key.ToString(CultureInfo.CurrentUICulture));
////						dt.Rows.Remove(rows[0]);
////						dt.AcceptChanges();
////
////						//_changeMade = true;
////						itemDeleted = true;
////						//FormLoaded = false;				
////					}
////				}
////
////				if (itemDeleted)
////				{
////					if (cboModelName.Items.Count > 0)
////					{
////						int nextItem;
////						if (currIndex >= cboModelName.Items.Count)
////						{
////							nextItem = cboModelName.Items.Count - 1;
////						}
////						else
////						{
////							nextItem = currIndex;
////						}
////
////						DataRowView aRow = (DataRowView)cboModelName.SelectedItem;
////						int SizeFringeModelRid = Convert.ToInt32(aRow.Row["SIZE_FRINGE_RID"]);	
////						InitializeForm(SizeFringeModelRid);
////					}
////					else
////					{
////						InitializeForm();
////					}
////				}
////			}
////			catch(DatabaseForeignKeyViolation)
////			{
////				td.Rollback();
////				MessageBox.Show (_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DeleteFailedDataInUse));
////			}
////			catch(Exception exception)
////			{
////				td.Rollback();
////				HandleException(exception);
////			}
////			finally
////			{
////				td.CloseUpdateConnection();
////			}
//		}
//
//		override public void IRefresh()
//		{
//			
//		}
//
//		protected override bool SaveChanges()
//		{
////			try
////			{
////				bool continueSave = false;
////				bool saveAsCanceled = false;
////
////				if (!ErrorFound)
////				{
////					if (_newModel && _saveAsName == "")
////					{
////						frmSaveAs formSaveAs = new frmSaveAs(_sab);
////						formSaveAs.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
////						while (!continueSave)
////						{
////							formSaveAs.ShowDialog(this);
////							saveAsCanceled = formSaveAs.SaveCanceled;
////							if (!saveAsCanceled)
////							{
////								SizeFringeModelProfile checkExists = GetFringeModel(formSaveAs.SaveAsName);
////								if (checkExists.Key == Include.NoRID)
////								{
////									this._sizeFringeModel.SizeFringeName = formSaveAs.SaveAsName;
////									continueSave = true;
////								}
////								else
////								{
////									if (MessageBox.Show (_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DuplicateName),  this.Text,
////										MessageBoxButtons.YesNo, MessageBoxIcon.Question)
////										== DialogResult.No) 
////									{
////										saveAsCanceled = true;
////										continueSave = true;
////									}
////									else
////									{
////										saveAsCanceled = false;
////										continueSave = true;
////										_sizeFringeModel.ModelChangeType = eChangeType.update;
////									}
////								}
////							}
////							else
////							{
////								continueSave = true;
////							}
////						}
////					}
////
////					if (!saveAsCanceled)
////					{
////						SaveGridsToProfile();
////						TransactionData td = new TransactionData();
////						
////						td.OpenUpdateConnection();
////						// Save sile alternate model data
////						int SizeFringeModelRid = SizeModelData.SizeFringeModel_Insert(_sizeFringeModel.Key,
////							_sizeFringeModel.SizeFringeName, this._sizeFringeModel.SizeCurveGroupRid,
////							_sizeFringeModel.SortOrder, td);
////						_sizeFringeModel.Key = SizeFringeModelRid;
////						// Save Grid data
////						InsertSizeFringeChildren(td);
////						td.CommitData();
////						td.CloseUpdateConnection();
////						ChangePending = false;
////						// adds row to cboModelName 
////						if (_sizeFringeModel.ModelChangeType == eChangeType.add)
////						{
////
////							DataTable dt = (DataTable)cboModelName.DataSource;
////							dt.Rows.Add(new object[] { _sizeFringeModel.Key, _sizeFringeModel.SizeFringeName, 
////														 _sizeFringeModel.SizeCurveGroupRid, _sizeFringeModel.SortOrder} );
////							dt.AcceptChanges();
////							cboModelName.SelectedValue = _sizeFringeModel.Key;
////						}
////					}
////					else
////					{
////						MessageBox.Show(_sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SaveCanceled),  this.Text,
////							MessageBoxButtons.OK, MessageBoxIcon.Information);
////					}
////					this.cboModelName.Enabled = true;
////					_sizeFringeModel.ModelChangeType = eChangeType.update;
////					_newModel = false;
////					Format_Title(eDataState.Updatable, eMIDTextCode.frm_SizeFringeModel, null);
////					if (FunctionSecurity.AllowDelete)
////						btnDelete.Enabled = true;
////				}
////			}
////			catch(Exception exception)
////			{
////				HandleException(exception);
////			}
//
//			return true;
//		}
//
////		private SizeFringeModelProfile GetFringeModel(string modelName)
////		{
////			SizeFringeModelProfile aModel = null;
////			DataTable dt = this.SizeModelData.SizeFringeModel_Read(modelName);
////			if (dt.Rows.Count == 0)
////			{
////				aModel = new SizeFringeModelProfile(Include.NoRID);
////			}
////			else
////			{	
////				DataRow aRow = dt.Rows[0];
////				int aModelKey = Convert.ToInt32(aRow["SIZE_FRINGE_RID"], CultureInfo.CurrentUICulture);
////				aModel = new SizeFringeModelProfile(aModelKey);
////			}
////			return aModel;
////		}
//
//		private bool InsertSizeFringeChildren(TransactionData td)
//		{
//			bool successful = true;
//
////			try
////			{
////				successful = DeleteSizeFringeChildren(td);
////				if (successful)
////				{
////					foreach (SizeFringe sf in _sizeFringeModel.FringeList)
////					{
////						SizeModelData.SizeFringe_insert(_sizeFringeModel.Key,sf.SizeTypeRid, sf.FringeInd, sf.SizeType, td);
////					}
////
////					foreach (SizeFringeFilter sff in _sizeFringeModel.FilterList)
////					{
////						SizeModelData.SizeFringeFilter_insert(_sizeFringeModel.Key, sff.Criteria, sff.Condition, sff.Value, sff.ValueType, td);
////					}
////				}				
////			}
////			catch
////			{
////				throw;
////			}
//
//			return successful;
//
//		}
//
//		private bool DeleteSizeFringeChildren(TransactionData td)
//		{
//			bool Successful;
//			Successful = true;
////
////			try
////			{
////				Successful = SizeModelData.DeleteSizeFringeChildren(this._sizeFringeModel.Key, td);
////			}
////			catch(Exception)
////			{
////				throw;
////			}
//
//			return Successful;
//
//		}
//
//		private void SaveGridsToProfile()
//		{
//			SaveFringeGridToProfile();
//			SaveFilterGridToProfile();
//		}
//
//		private void SaveFringeGridToProfile()
//		{
//			// This is handled in the ugFringeGrid_AfterCellUpdate event.
//		}
//
//		private void SaveFilterGridToProfile()
//		{
////			_sizeFringeModel.FilterList.Clear();
////			foreach (DataRow aRow in _dsFilter.Tables["Filter"].Rows)
////			{
////				eFringeOverrideUnitCriteria criteria = (eFringeOverrideUnitCriteria)Convert.ToInt32(aRow["Criteria"], CultureInfo.CurrentUICulture);	
////				decimal aValue = Convert.ToInt32(aRow["Value"], CultureInfo.CurrentUICulture);
////				eFringeOverrideCondition condition = (eFringeOverrideCondition)Convert.ToInt32(aRow["Condition"], CultureInfo.CurrentUICulture);	
////				eFringeFilterValueType type = (eFringeFilterValueType)Convert.ToInt32(aRow["Type"], CultureInfo.CurrentUICulture);	
////				SizeFringeFilter aFilter = new SizeFringeFilter(criteria, condition, aValue, type);
////				_sizeFringeModel.FilterList.Add(aFilter);
////			}
//		}
//
//
//		private bool Validation()
//		{
//			bool isValid = true;
//			isValid = ValidateFilterGrid();
//
//			return isValid;
//		}
//
//		private bool ValidateFilterGrid()
//		{
//			bool isValid = true;
//
//			//================================================================
//			// Make sure each filter line is completly filled in
//			//================================================================
//			foreach (UltraGridRow filterRow in ugFilterGrid.Rows)
//			{
//				if(!ValidateFilterCells(filterRow))
//					isValid = false;
//			}
//			return isValid;
//		}
//
//		private bool ValidateFilterCells(UltraGridRow aRow)
//		{
//			bool isValid = true;
////			_errorMessages.Clear();
////			UltraGridCell aCell;
////
////			decimal aValue;
////			eFringeFilterValueType aValueType;
////			
////			for (int c=0;c<4;c++)
////			{
////				aCell = aRow.Cells[c];
////				ClearErrors(aCell);
////				if (aCell.Text.Trim() == string.Empty)
////				{
////					isValid = false;
////					ValueRequired(aCell);
////				}
////			}
////
////			if (isValid)
////			{
////				aCell = aRow.Cells["Value"];
////				aValue = Convert.ToDecimal(aCell.Value, CultureInfo.CurrentUICulture);
////				aValueType = (eFringeFilterValueType)Convert.ToInt32(aRow.Cells["Type"].Value, CultureInfo.CurrentUICulture);
////
////				if (aValue < 0)
////				{
////					isValid = false;
////					string msg = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative);
////					_errorMessages.Add(msg);
////					AttachErrors(aCell);
////				}
////				else if (aValueType == eFringeFilterValueType.Units)
////				{
////					int iValue = (int)aValue;
////					if (aValue > (decimal)iValue)
////					{
////						isValid = false;
////						string msg = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidUnits);
////						_errorMessages.Add(msg);
////						AttachErrors(aCell);
////					}
////				}
////				else
////				{
////					if (aValue > 100)
////					{
////						isValid = false;
////						string msg = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidPercent);
////						_errorMessages.Add(msg);
////						AttachErrors(aCell);
////					}
////				}
////			}
//
//			return isValid;
//
//		}
//
//		private void ValueRequired(UltraGridCell aCell)
//		{
//			string msg = _sab.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueRequired);
//			msg = msg.Replace("{0}",aCell.Column.Header.Caption);
//			_errorMessages.Add(msg);
//			AttachErrors(aCell);
//		}
//
//		private void ClearErrors(UltraGridCell aCell)
//		{
//			aCell.Appearance.Image = null;
//			aCell.Tag = null;
//		}
//
//		private void AttachErrors(UltraGridCell activeCell)
//		{
//			try
//			{
//				activeCell.Appearance.Image = ErrorImage;
//
//				for (int errIdx=0; errIdx <= _errorMessages.Count - 1; errIdx++)
//				{
//					activeCell.Tag = (errIdx == 0) ? _errorMessages[errIdx] : activeCell.Tag + "\n" + _errorMessages[errIdx];
//				}
//
//				_errorMessages.Clear();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "AttachErrors(UltraGridCell activeCell)");
//			}	
//		}
//
//		private void ugFilterGrid_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
//		{
//			if (e.Row.HasPrevSibling(false, false))
//			{
//				UltraGridRow prevSibRow = e.Row.GetSibling(SiblingRow.Previous,false, false);
//				prevSibRow.Cells["Operator"].Value = "Or";
//			}
//		}
//
//		private void ugFringeGrid_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
//		{
////			try
////			{
////				int sizeCodeKey = Include.NoRID;
////				int fingeInd = Include.NoRID;
////				eEquateOverrideSizeType sizeType = eEquateOverrideSizeType.DimensionSize;
////
////				if (e.Cell.Value == System.DBNull.Value)
////				{
////					fingeInd = Include.NoRID;
////				}
////				else
////				{
////					fingeInd = Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture);
////				}
////				
////				sizeCodeKey = Convert.ToInt32(e.Cell.Row.Cells[e.Cell.Column.Key+"HIDDEN"].Value, CultureInfo.CurrentUICulture);
////				sizeType = (eEquateOverrideSizeType)Convert.ToInt32(e.Cell.Row.Cells[e.Cell.Column.Key+"HIDDENTYPE"].Value, CultureInfo.CurrentUICulture);
////				_sizeFringeModel.UpdateFringeList(sizeCodeKey, sizeType, fingeInd);
////				ChangePending = true;
////			}
////			catch
////			{
////				throw;
////			}
//		}
//
//		private void rbAscending_CheckedChanged(object sender, System.EventArgs e)
//		{
////			ChangePending = true;
////			if (rbAscending.Checked)
////			{
////				_sizeFringeModel.SortOrder = eFringeOverrideSort.Ascending;
////			}
//		}
//
//		private void rbDescending_CheckedChanged(object sender, System.EventArgs e)
//		{
////			ChangePending = true;
////			if (rbDescending.Checked)
////			{
////				_sizeFringeModel.SortOrder = eFringeOverrideSort.Descending;
////			}
//		}
//
//		private void ugFringeGrid_AfterExitEditMode(object sender, System.EventArgs e)
//		{
//			ChangePending = true;
//		}
//
//		private void ugFilterGrid_AfterExitEditMode(object sender, System.EventArgs e)
//		{
//			ChangePending = true;
//		}
//
//		private void ugFilterGrid_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
//		{
//			try
//			{
//				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(UltraGridCell));
////				if (gridCell != null) 
////				{
////					switch (gridCell.Column.Style)
////					{
////						case Infragistics.Win.UltraWinGrid.ColumnStyle.TriStateCheckBox:
////						case Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox:
////							ugFilterGrid.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
////							break;
////						default:
////						switch (gridCell.Column.DataType.Name.ToUpper())
////						{
////							case "BOOLEAN":
////								ugFilterGrid.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
////								break;
////							default:
////								ugFilterGrid.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
////								break;
////						}
////							break;
////					}
////				}
//
//
//				ShowUltraGridToolTip(ugFilterGrid, e);
//
//				UltraGridRow aRow = (UltraGridRow)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridRow));
//				if (aRow != null)
//				{
//					if (aRow.Tag != null) 
//					{
//						if (aRow.Tag.GetType() == typeof(System.String))
//						{
//							ToolTip.Active = true; 
//							ToolTip.SetToolTip(ugFilterGrid, (string)aRow.Tag);
//						}
//					}
//				}
//			}
//			catch( Exception ex )
//			{
//				HandleException(ex, "ugFilterGrid_MouseEnterElement");
//			}
//		}
//
//	}
//}
// end MID Track 3619 Remove Fringe
