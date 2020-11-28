using System;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinListBar;
using Infragistics.Win.UltraWinTree;
using Infragistics.Win.UltraWinMaskedEdit;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SimilarStoreSelect.
	/// </summary>
	public class frmSimilarStoreSelect : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private System.Data.DataSet _similarStoresDataSet;
		private ProfileList _storeGroupLevelList;
		private ProfileList _storeGroupList;
		private int _groupRID = -1;
		private int _nodeRID = -1;
        //Begin TT#??? - JSmith - Add Size Curve info to Node Properties
        private int _selectedAttributeRID = -1;
        //End TT#??? - JSmith - Add Size Curve info to Node Properties
		private ArrayList _similarStoreList;
		private Hashtable _similarStoreHashList = new Hashtable();
		private MenuItem _mnuItemApply = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Apply));
//		private string _similarStoreLayoutFile = "SimilarStoreLayout.txt";
//		private bool _similarStoresGridBuilt = false;
		private bool _similarStoresIsPopulated = false;
		private bool _cancelClicked = false;

		/// <summary>
		/// Gets or sets the similar store list.
		/// </summary>
		public ArrayList SimilarStoreList 
		{
			get { return _similarStoreList ; }
			set { _similarStoreList = value ; }
		}

		/// <summary>
		/// Gets or sets if the cancel button was clicked.
		/// </summary>
		public bool CancelClicked 
		{
			get { return _cancelClicked ; }
			set { _cancelClicked = value ; }
		}

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtStore;
		private System.Windows.Forms.Label lblSEAttribute;
		private Infragistics.Win.UltraWinGrid.UltraGrid ugSimilarStores;
		private System.Windows.Forms.ContextMenu mnuSimilarStoresGrid;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        //Begin TT#??? - JSmith - Add Size Curve info to Node Properties
        //public frmSimilarStoreSelect(SessionAddressBlock aSAB, FunctionSecurityProfile aSecurityEligibility) : base (aSAB)
        public frmSimilarStoreSelect(SessionAddressBlock aSAB, FunctionSecurityProfile aSecurityEligibility, int aSelectedAttribute)
            : base(aSAB)
        //End TT#??? - JSmith - Add Size Curve info to Node Properties
		{
            //
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_SAB = aSAB;
            // Begin Track #4872 - JSmith - Global/User Attributes
            //_storeGroupList = _SAB.StoreServerSession.GetStoreGroupListViewList();
            _storeGroupList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.All, true); //_SAB.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, true);
            // End Track #4872
			FunctionSecurity = aSecurityEligibility;
            _selectedAttributeRID = aSelectedAttribute;
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

				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
				this.ugSimilarStores.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSimilarStores_CellChange);
				this.ugSimilarStores.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSimilarStores_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugSimilarStores);
                //End TT#169
				this.mnuSimilarStoresGrid.Popup -= new System.EventHandler(this.mnuSimilarStoresGrid_Popup);
				if (_mnuItemApply != null)
				{
					_mnuItemApply.Click -= new System.EventHandler(this.mnuItemApply_Click);
				}

                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);

				ugSimilarStores.DataSource = null;
				cboStoreAttribute.DataSource = null;
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStore = new System.Windows.Forms.TextBox();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblSEAttribute = new System.Windows.Forms.Label();
            this.ugSimilarStores = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.mnuSimilarStoresGrid = new System.Windows.Forms.ContextMenu();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugSimilarStores)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(232, 456);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(320, 456);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(8, 456);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(24, 23);
            this.btnHelp.TabIndex = 12;
            this.btnHelp.Text = "?";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "Similar Store(s) for Store:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStore
            // 
            this.txtStore.Location = new System.Drawing.Point(168, 9);
            this.txtStore.Name = "txtStore";
            this.txtStore.Size = new System.Drawing.Size(100, 20);
            this.txtStore.TabIndex = 16;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Location = new System.Drawing.Point(168, 40);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(152, 21);
            this.cboStoreAttribute.TabIndex = 35;
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            // 
            // lblSEAttribute
            // 
            this.lblSEAttribute.Location = new System.Drawing.Point(56, 40);
            this.lblSEAttribute.Name = "lblSEAttribute";
            this.lblSEAttribute.Size = new System.Drawing.Size(104, 23);
            this.lblSEAttribute.TabIndex = 34;
            this.lblSEAttribute.Text = "Store Attribute:";
            this.lblSEAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ugSimilarStores
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugSimilarStores.DisplayLayout.Appearance = appearance1;
            this.ugSimilarStores.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugSimilarStores.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugSimilarStores.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSimilarStores.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugSimilarStores.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugSimilarStores.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugSimilarStores.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugSimilarStores.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugSimilarStores.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugSimilarStores.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugSimilarStores.Location = new System.Drawing.Point(56, 80);
            this.ugSimilarStores.Name = "ugSimilarStores";
            this.ugSimilarStores.Size = new System.Drawing.Size(296, 360);
            this.ugSimilarStores.TabIndex = 36;
            this.ugSimilarStores.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugSimilarStores_InitializeLayout);
            this.ugSimilarStores.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugSimilarStores_CellChange);
            // 
            // mnuSimilarStoresGrid
            // 
            this.mnuSimilarStoresGrid.Popup += new System.EventHandler(this.mnuSimilarStoresGrid_Popup);
            // 
            // frmSimilarStoreSelect
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(408, 494);
            this.Controls.Add(this.ugSimilarStores);
            this.Controls.Add(this.cboStoreAttribute);
            this.Controls.Add(this.lblSEAttribute);
            this.Controls.Add(this.txtStore);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "frmSimilarStoreSelect";
            this.Text = "Similar Store Select";
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnHelp, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtStore, 0);
            this.Controls.SetChildIndex(this.lblSEAttribute, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.ugSimilarStores, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugSimilarStores)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		public void InitializeForm(string storeName, int nodeRID, string nodeName, ArrayList similarStores)
		{
			this.txtStore.Text = storeName;
			_nodeRID = nodeRID;
			_similarStoreList = similarStores;
			SimilarStoresForm_Load();
			eDataState dataState;
			// security changes 1/25/04 stodd
			if (FunctionSecurity.AllowUpdate)
			{
//				AllowUpdate = true;  Security changes 1/24/05 vg
				dataState = eDataState.Updatable;
			}
			else
			{
//				AllowUpdate = false;  Security changes 1/24/05 vg
				dataState = eDataState.ReadOnly;
			}
			Format_Title(dataState, eMIDTextCode.frm_SimilarStoresSelect,nodeName);
			SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg
		}

		private void ugSimilarStores_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			// check for saved layout
			InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
			InfragisticsLayout layout = layoutData.InfragisticsLayout_Read(_SAB.ClientServerSession.UserRID, eLayoutID.similarStoresSelectGrid);
			if (layout.LayoutLength > 0)
			{
				ugSimilarStores.DisplayLayout.Load(layout.LayoutStream);
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
				DefaultSimilarStoreGridLayout();
			}
			 
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["SetID"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Set);
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["Selected"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Selected);
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["Store"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_ST_ID);
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["Selected"].Header.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Selected);
//			_similarStoresGridBuilt = true;
		}

		private void DefaultSimilarStoreGridLayout()
		{
			this.ugSimilarStores.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.ugSimilarStores.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
			this.ugSimilarStores.DisplayLayout.AddNewBox.Hidden = true;
			this.ugSimilarStores.DisplayLayout.GroupByBox.Hidden = true;
			this.ugSimilarStores.DisplayLayout.GroupByBox.Prompt = " ";
			this.ugSimilarStores.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["SetID"].Width = 150;
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["Selected"].Width = 55;
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["Selected"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
			this.ugSimilarStores.DisplayLayout.Bands["Sets"].Columns["Selected"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["Store"].Width = 150;
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["Selected"].Width = 55;
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["SetID"].Hidden = true;
			this.ugSimilarStores.DisplayLayout.Bands["Stores"].Columns["Store RID"].Hidden = true;
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //FormatColumns(this.ugSimilarStores);
            //End TT#169
		}
		private void SimilarStoresForm_Load()
		{
			BuildSimilarStoreHashList();
			
			SimilarStores_Define();
			SimilarStores_Attributes_Populate();

			ugSimilarStores.DataSource = _similarStoresDataSet;

			//			_similarStoreList = _SAB.HierarchyServerSession.GetSimilarStoresList(_nodeRID);
			SimilarStores_Populate(false);

			BuildSimilarStoresContextmenu();
			this.ugSimilarStores.ContextMenu = mnuSimilarStoresGrid;

			ugSimilarStores.ActiveRow = ugSimilarStores.GetRow(Infragistics.Win.UltraWinGrid.ChildRow.First);

			_similarStoresIsPopulated = true;
			
            //Begin Track #5858 - KJohnson - Validating store security only
            // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
            //cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute);
            cboStoreAttribute.Tag = new MIDStoreAttributeComboBoxTag(SAB, cboStoreAttribute, FunctionSecurity, true);
            // End TT#44
            //End Track #5858

		}

		private void BuildSimilarStoreHashList()
		{
			_similarStoreHashList.Clear();
			foreach(int similarStore in _similarStoreList)
			{
				_similarStoreHashList.Add(similarStore, true);
			}
		}

		private void SimilarStores_Define()
		{
            _similarStoresDataSet = MIDEnvironment.CreateDataSet("SimilarStoresDataSet");

			DataColumn dataColumn;

			DataTable setTable = _similarStoresDataSet.Tables.Add("Sets");

			//Create Columns and rows for datatable
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SetID";
			dataColumn.ReadOnly = true;
			dataColumn.Unique = true;
			setTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Boolean");
			dataColumn.ColumnName = "Selected";
			dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Selected);;
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			setTable.Columns.Add(dataColumn);

			//make set ID the primary key
			DataColumn[] PrimaryKeyColumn = new DataColumn[1];
			PrimaryKeyColumn[0] = setTable.Columns["SetID"];
			setTable.PrimaryKey = PrimaryKeyColumn;


			DataTable storeTable = _similarStoresDataSet.Tables.Add("Stores");

			//Create Columns and rows for datatable
			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SetID";
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			storeTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "Store RID";
			dataColumn.ReadOnly = true;
			dataColumn.Unique = true;
			storeTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "Store";
			dataColumn.ReadOnly = true;
			dataColumn.Unique = true;
			storeTable.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Boolean");
			dataColumn.ColumnName = "Selected";
			dataColumn.Caption = MIDText.GetTextOnly(eMIDTextCode.lbl_Selected);;
			dataColumn.ReadOnly = false;
			dataColumn.Unique = false;
			storeTable.Columns.Add(dataColumn);

			//make grade column the primary key
			PrimaryKeyColumn = new DataColumn[1];
			PrimaryKeyColumn[0] = storeTable.Columns["Store"];
			storeTable.PrimaryKey = PrimaryKeyColumn;

			_similarStoresDataSet.Relations.Add("Stores",
				_similarStoresDataSet.Tables["Sets"].Columns["SetID"],
				_similarStoresDataSet.Tables["Stores"].Columns["SetID"]);

		}

		private void SimilarStores_Populate(bool doingCopy)
		{
			bool selected = false;
			ugSimilarStores.BeginUpdate();
			ugSimilarStores.SuspendRowSynchronization();
			_similarStoresDataSet.Tables["Stores"].Rows.Clear();
			_similarStoresDataSet.Tables["Sets"].Rows.Clear();
			
			foreach(StoreGroupLevelListViewProfile sglp in _storeGroupLevelList)
			{
				_similarStoresDataSet.Tables["Sets"].Rows.Add(new object[] { sglp.Name, false});
				foreach(StoreProfile storeProfile in sglp.Stores.ArrayList)
				{
					if (_similarStoreHashList.Contains(storeProfile.Key))
					{
						selected = true;
					}
					else
					{
						selected = false;
					}
					_similarStoresDataSet.Tables["Stores"].Rows.Add(new object[] {sglp.Name, storeProfile.Key, storeProfile.Text, selected});
				}
			}
			_similarStoresDataSet.AcceptChanges();
			ugSimilarStores.ResumeRowSynchronization();
			ugSimilarStores.EndUpdate();
		}

		private void BuildSimilarStoresContextmenu()
		{
			mnuSimilarStoresGrid.MenuItems.Clear();
			MenuItem mnuItemExpandAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_ExpandAll));
			MenuItem mnuItemCollapseAll = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_CollapseAll));
			MenuItem mnuItemClear = new MenuItem(MIDText.GetTextOnly(eMIDTextCode.lbl_Clear_Values));
			mnuSimilarStoresGrid.MenuItems.Add(_mnuItemApply);
			mnuSimilarStoresGrid.MenuItems.Add(mnuItemClear);
			mnuSimilarStoresGrid.MenuItems.Add("-");
			mnuSimilarStoresGrid.MenuItems.Add(mnuItemExpandAll);
			mnuSimilarStoresGrid.MenuItems.Add(mnuItemCollapseAll);
			_mnuItemApply.Click += new System.EventHandler(this.mnuItemApply_Click);
			mnuItemExpandAll.Click += new System.EventHandler(this.mnuItemExpandAll_Click);
			mnuItemCollapseAll.Click += new System.EventHandler(this.mnuItemCollapseAll_Click);
			mnuItemClear.Click += new System.EventHandler(this.mnuItemClear_Click);
		}

		private void mnuItemExpandAll_Click(object sender, System.EventArgs e)
		{
			this.ugSimilarStores.Rows.ExpandAll(true);
		}

		private void mnuItemCollapseAll_Click(object sender, System.EventArgs e)
		{
			this.ugSimilarStores.Rows.CollapseAll(true);
		}

		private void mnuSimilarStoresGrid_Popup(object sender, System.EventArgs e)
		{
			if (this.ugSimilarStores.ActiveRow == null ||
				this.ugSimilarStores.ActiveRow.Band.Key != "Sets")
			{
				_mnuItemApply.Enabled = false;
			}
			else
			{
				_mnuItemApply.Enabled = true;
			}
		}

		private void mnuItemApply_Click(object sender, System.EventArgs e)
		{
			string errorMessage = null;
			if (ugSimilarStores.ActiveRow == null)
			{
				MessageBox.Show (errorMessage,  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
//Begin Track #4586 - JScott - Unhandled Exception when clearing Average Store Model
//				ugSimilarStores.ActiveCell.Row.Update();
				ugSimilarStores.ActiveRow.Update();
//End Track #4586 - JScott - Unhandled Exception when clearing Average Store Model

				Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugSimilarStores.ActiveRow.Band;
				Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugSimilarStores.ActiveRow;

				Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
				string setid = (string)activeRow.Cells["SetID"].Value;
				if (activeRowBand.Key == "Sets")		// apply values to all stores in set
				{
					ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
					foreach(  UltraGridRow storeRow in ultraGridStoreBand.Rows )
					{
						storeRow.Cells["Selected"].Value = activeRow.Cells["Selected"].Value;
					}
					_similarStoresDataSet.AcceptChanges();
				}
			}
		}

		private void mnuItemClear_Click(object sender, System.EventArgs e)
		{
			Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugSimilarStores.ActiveRow.Band;
			Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugSimilarStores.ActiveRow;
			Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;
			if (activeRowBand.Key == "Sets")		// apply values to all stores in set
			{
				ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
				foreach(  UltraGridRow storeRow in ultraGridStoreBand.Rows )
				{
					storeRow.Cells["Selected"].Value = false;
				}
				
			}
			else
				if (activeRowBand.Key == "Stores")		// clear store value
			{
				activeRow.Cells["Selected"].Value = false;
			}
			_similarStoresDataSet.AcceptChanges();
		}

		private void SimilarStores_Attributes_Populate()
		{
			StoreGroupListViewProfile storeGroup = (StoreGroupListViewProfile)_storeGroupList[0];
			_groupRID = storeGroup.Key;

			//			this.cboStoreAttribute.DataSource = _storeGroupList.ArrayList;
            // Begin Track #4872 - JSmith - Global/User Attributes
            //this.cboStoreAttribute.ValueMember = "Key";
            //this.cboStoreAttribute.DisplayMember = "Name";
            //this.cboStoreAttribute.DataSource = _storeGroupList.ArrayList;
            cboStoreAttribute.Initialize(SAB, FunctionSecurity, _storeGroupList.ArrayList, true);
            // End Track #4872
            //Begin TT#??? - JSmith - Add Size Curve info to Node Properties
            //_groupRID = storeGroup.Key;
            //this.cboStoreAttribute.SelectedValue = storeGroup.Key;
            _groupRID = _selectedAttributeRID;
            this.cboStoreAttribute.SelectedValue = _selectedAttributeRID;
            //End TT#??? - JSmith - Add Size Curve info to Node Properties
            _storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(_groupRID, true); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(_groupRID, true);
		}

		private void cboStoreAttribute_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			UpdateSimilarStoreData();
			StoreGroupListViewProfile storeGroup = (StoreGroupListViewProfile)_storeGroupList[cboStoreAttribute.SelectedIndex];
			_groupRID = storeGroup.Key;
            _storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(_groupRID, true); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(_groupRID, true);
			SimilarStores_Populate(false);
		}

        private void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            //Begin Track #5858 - Kjohnson - Validating store security only
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
            //End Track #5858
        }
		private void UpdateSimilarStoreData()
		{
			int storeRID;
			if (_similarStoresIsPopulated)
			{
				// rebuild similar stores each time
				_similarStoreList.Clear();
				Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;

				foreach(  UltraGridRow setRow in this.ugSimilarStores.Rows )
				{
					ultraGridStoreBand = setRow.ChildBands["Stores"];	// get "Stores" band
					foreach(  UltraGridRow storeRow in ultraGridStoreBand.Rows )
					{
						if ((bool)storeRow.Cells["Selected"].Value)
						{
							storeRID = (int)storeRow.Cells["Store RID"].Value;
							_similarStoreList.Add(storeRID);
						}
					}
				}
				BuildSimilarStoreHashList();
			}
		}

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        //private void FormatColumns(Infragistics.Win.UltraWinGrid.UltraGrid ultragid)
        //{
        //    foreach ( Infragistics.Win.UltraWinGrid.UltraGridBand oBand in ultragid.DisplayLayout.Bands )
        //    {
        //        foreach ( Infragistics.Win.UltraWinGrid.UltraGridColumn oColumn in oBand.Columns )
        //        {
        //            switch (oColumn.DataType.ToString())
        //            {
        //                case "System.Int32":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#,###,##0";
        //                    break;
        //                case "System.Double":
        //                    oColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
        //                    oColumn.Format = "#,###,###.00";
        //                    break;
        //            }
        //        }
        //    }
        //}
        //End TT#169
        		
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			UpdateSimilarStoreData();
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			CancelClicked = true;
			this.Close();
		}

		override protected void BeforeClosing()
		{
			try
			{
                // Begin TT#2012 - JSmith - Layout corrupted if close before opens completely
                //if (!ugSimilarStores.IsDisposed)
                if (FormLoaded && 
                    !ugSimilarStores.IsDisposed)
                // End TT#2012
				{
					InfragisticsLayoutData layoutData = new InfragisticsLayoutData();
					layoutData.InfragisticsLayout_Save(_SAB.ClientServerSession.UserRID, eLayoutID.similarStoresSelectGrid, ugSimilarStores);
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}

		private void ugSimilarStores_CellChange(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if (_similarStoresIsPopulated)
			{
				switch (ugSimilarStores.ActiveCell.Column.Key)
				{
					case "Selected":
						Infragistics.Win.UltraWinGrid.UltraGridBand activeRowBand = this.ugSimilarStores.ActiveRow.Band;
						Infragistics.Win.UltraWinGrid.UltraGridRow activeRow = this.ugSimilarStores.ActiveRow;
						Infragistics.Win.UltraWinGrid.UltraGridChildBand ultraGridStoreBand;

						if (activeRowBand.Key == "Sets")		// apply values to all stores in set
						{
							ugSimilarStores.BeginUpdate();
							ultraGridStoreBand = activeRow.ChildBands["Stores"];	// get "Stores" band
							foreach(  UltraGridRow storeRow in ultraGridStoreBand.Rows )
							{
								if (e.Cell.Text == "True")
								{
									storeRow.Cells["Selected"].Value = true;
								}
								else
								{
									storeRow.Cells["Selected"].Value = false;
								}
							}
							_similarStoresDataSet.AcceptChanges();
							ugSimilarStores.EndUpdate();
						}

						break;
				}
					
			}
		}

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

        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }

	}
}
