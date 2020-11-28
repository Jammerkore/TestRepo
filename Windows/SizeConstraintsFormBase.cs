// BEGIN MID Track #5170 - JSmith - Model enhancements
// Too many lines changed to mark.  Use SCM Compare for details.
//End Track #5170
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Globalization;

using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeMethodsFormBase.
	/// </summary>
	public class SizeConstraintsFormBase : ModelFormBase
	{

	#region Form Fields
        //protected Infragistics.Win.UltraWinGrid.UltraGrid ugModel;
	#endregion

	#region Member Variables
        //private SessionAddressBlock _SAB;
		private DataSet _dsBackup = null; //Copy of the dataset that is bound to the grid
		private DataTable _dtSizes = null;
		private DataTable _dtColors = null;
		private DataTable _dtDimensions = null;

		private ContextMenu _setContext;
		private ContextMenu _colorContext;
		private ContextMenu _sizeContext;
		private ContextMenu _dimensionContext;
		private ArrayList _errMsgs = new ArrayList();
		private bool _editByCell = false;
		private bool _constraintRollback = false;
		private bool _showContext = false;
		private KeyEventArgs _gridKeyEvent = null;
		private SizeGroupList _sgl = null;
		private StoreData _storeData = null;
		//private StoreFilterData _storeFilterData = null;
		private CollectionSets _setsCollection = null;
		private DataSet _sizeConstraints;
		//private int _key = Include.NoRID;
		private bool _getSizes;
		private eGetSizes _getSizesUsing;
		private eGetDimensions _getDimensionsUsing;
		//private int _storeGroupRid;
		//private int	_sizeGroupRid;
		//private int _sizeCurveGroupRid;
		private SizeModelData _sizeModelData = null;
		private SizeConstraintModelProfile _sizeConstraintProfile;
		private bool _promptAttributeChange = true;
		private bool _promptSizeChange = true;
		private bool _modelLocked = false;

	#endregion

	#region Properties
		protected const string  C_SET = "SETLEVEL", 
								C_SET_ALL_CLR = "SETALLCOLOR", 
								C_SET_CLR = "SETCOLOR",
								C_ALL_CLR_SZ = "ALLCOLORSIZE", 
								C_CLR_SZ = "COLORSIZE", 
								C_CLR_SZ_DIM = "COLORSIZEDIMENSION",
								C_ALL_CLR_SZ_DIM = "ALLCOLORSIZEDIMENSION";

		protected const int C_CONTEXT_SET_APPLY = 0, 
							C_CONTEXT_SET_CLEAR = 1, 
							C_CONTEXT_SET_SEP1 = 2,
							C_CONTEXT_SET_ADD_CLR = 3;

		protected const int C_CONTEXT_CLR_APPLY = 0,
							C_CONTEXT_CLR_CLEAR = 1,
							C_CONTEXT_CLR_SEP1 = 2,
							C_CONTEXT_CLR_ADD_CLR = 3,
							C_CONTEXT_CLR_ADD_SZ_DIM = 4,
							C_CONTEXT_CLR_SEP2 = 5,
							C_CONTEXT_CLR_DELETE = 6;

		protected const int C_CONTEXT_SZ_ADD_SZ = 1,
							C_CONTEXT_SZ_SEP1 = 2,
							C_CONTEXT_SZ_DELETE = 3;

		protected const int C_CONTEXT_DIM_APPLY = 0,
							C_CONTEXT_DIM_CLEAR = 1,
							C_CONTEXT_DIM_SEP1 = 2,
							C_CONTEXT_DIM_ADD_DIM = 3,
							C_CONTEXT_DIM_ADD_SZ = 4,
							C_CONTEXT_DIM_SEP2 = 5,
							C_CONTEXT_DIM_DELETE = 6;

        //protected StoreFilterData storeFilterData
        //{
        //    get
        //    {
        //        if (_storeFilterData == null)
        //        {
        //            _storeFilterData = new StoreFilterData();
        //        }

        //        return _storeFilterData;
        //    }
        //}

		protected SizeGroupList sizeGroupList
		{
			get
			{ 
				if (_sgl == null)
				{
					_sgl = new SizeGroupList(eProfileType.SizeGroup);
				}

				return _sgl;
			}
		}

		protected StoreData storeData
		{
			get
			{
				if (_storeData == null)
				{
					_storeData= new StoreData();
				}

				return _storeData;
			}
		}

		protected SizeModelData SizeModelData
		{
			get
			{
				if (_sizeModelData == null)
				{
					_sizeModelData= new SizeModelData();
				}

				return _sizeModelData;
			}
		}

		protected KeyEventArgs GridKeyEvent
		{
			get {return _gridKeyEvent;}
			set {_gridKeyEvent = value;}
		}

		protected bool ShowContext
		{
			get {return _showContext;}
		}

		protected bool EditByCell
		{
			get {return _editByCell;}
			set {_editByCell = value;}
		}

		protected ArrayList ErrorMessages
		{
			get {return _errMsgs;}
		}

		protected bool ConstraintRollback
		{
			get {return _constraintRollback;}
			set {_constraintRollback = value;}
		}

		protected DataTable DtSizes
		{
			get {return _dtSizes;}
			set {_dtSizes = value;}
		}

		protected DataTable DtDimensions
		{
			get {return _dtDimensions;}
			set {_dtDimensions = value;}
		}

		protected DataTable DtColors
		{
			get {return _dtColors;}
			set {_dtColors = value;}
		}

		protected DataSet DataSetBackup
		{
			get {return _dsBackup;}
			set {_dsBackup = value;}
		}

		protected ContextMenu SetContext
		{
			get {return _setContext;}
			set {_setContext = value;}
		}

		protected ContextMenu ColorContext
		{
			get {return _colorContext;}
			set {_colorContext = value;}
		}
		protected ContextMenu SizeContext
		{
			get {return _sizeContext;}
			set {_sizeContext = value;}
		}
		protected ContextMenu DimensionContext		
		{
			get {return _dimensionContext;}
			set {_dimensionContext = value;}
		}

		/// <summary>
		/// Gets or sets dataset of min/max constraints for the method.
		/// </summary>
		public DataSet SizeConstraints
		{
			get {return _sizeConstraints;}
			set {_sizeConstraints = value;}
		}
		/// <summary>
		/// Gets or sets whether to include size data in SizeConstraints dataset.
		/// </summary>
		public bool GetSizes
		{
			get {return _getSizes;}
			set {_getSizes = value;}
		}

		public eGetSizes GetSizesUsing
		{
			get {return _getSizesUsing;}
			set {_getSizesUsing = value;}
		}

		public eGetDimensions GetDimensionsUsing
		{
			get {return _getDimensionsUsing;}
			set {_getDimensionsUsing = value;}
		}

		/// <summary>
		/// Gets or sets Size Constraint Model RID.
		/// </summary>
		public int SizeConstraintModelRid
		{
			get	{return SizeConstraintProfile.Key;}
			set {SizeConstraintProfile.Key = value;}
		}

		/// <summary>
		/// Gets or sets Size Constraint Model name.
		/// </summary>
		public string SizeConstraintModelName
		{
			get	{return SizeConstraintProfile.SizeConstraintName;}
			set {SizeConstraintProfile.SizeConstraintName = value;}
		}

		/// <summary>
		/// Gets or sets Size Group RID.
		/// </summary>
		public int SizeGroupRid
		{
			get	{return SizeConstraintProfile.SizeGroupRid;}
			set {SizeConstraintProfile.SizeGroupRid = value;}
		}

		/// <summary>
		/// Gets or sets Size Curve Group RID.
		/// </summary>
		public int SizeCurveGroupRid
		{
			get {return SizeConstraintProfile.SizeCurveGroupRid;}
			set {SizeConstraintProfile.SizeCurveGroupRid = value;}
		}

		/// <summary>
		/// Gets or set the Store Group RID.
		/// </summary>
		public int StoreGroupRid
		{
			get {return SizeConstraintProfile.StoreGroupRid;}
			set {SizeConstraintProfile.StoreGroupRid = value;}
		}

		/// <summary>
		/// Gets or set the Store constraint model change type.
		/// </summary>
		public eChangeType ModelChangeType
		{
			get {return SizeConstraintProfile.ModelChangeType;}
			set {SizeConstraintProfile.ModelChangeType = value;}
		}

		/// <summary>
		/// Gets or set the Size Constraint Profile.
		/// </summary>
		public SizeConstraintModelProfile SizeConstraintProfile
		{
			get 
			{
				if (_sizeConstraintProfile == null)
					_sizeConstraintProfile = new SizeConstraintModelProfile(Include.NoRID);

				return _sizeConstraintProfile;
			}
			set {_sizeConstraintProfile = value;}
		}

		public bool PromptAttributeChange
		{
			get 
			{
				return _promptAttributeChange;
			}
			set {_promptAttributeChange = value;}
		}

		public bool PromptSizeChange
		{
			get {return _promptSizeChange;}
			set {_promptSizeChange = value;}
		}
		// BEGIN MID Track #4970 - modify to emulate other models 	
		public bool ModelLocked
		{
			get {return _modelLocked;}
			set {_modelLocked = value;}
		}
		// END MID Track #4970	
	#endregion
		
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;

	#region Constructors / Dispose
		public SizeConstraintsFormBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.ugModel.Text = string.Empty;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		public SizeConstraintsFormBase(SessionAddressBlock aSAB)  : base (aSAB)
		{
			InitializeComponent();

			if (aSAB == null)
			{
				throw new Exception("SessionAddressBlock is required");
			}
            //_SAB = aSAB;

			this.ugModel.Text = string.Empty;
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

//				if (this._dsBackup != null)
//				{
//					this._dsBackup.Dispose();
//				}
//
//				if (this._dtColors != null)
//				{
//					this._dtColors.Dispose();
//				}
//
//				if (this._dtDimensions != null)
//				{
//					this._dtDimensions.Dispose();
//				}
//
//				if (this._dtSizes != null)
//				{
//					this._dtSizes.Dispose();
//				}
//
//				if (this._sizeContext != null)
//				{
//					this._sizeContext.Dispose();
//				}
//
//				if (this._dimensionContext != null)
//				{
//					this._dimensionContext.Dispose();
//				}
//
//				if (this._colorContext != null)
//				{
//					this._colorContext.Dispose();
//				}
//
//				if (this._setContext != null)
//				{
//					this._setContext.Dispose();
//				}
//
//				if (this._sgl != null)
//				{
//					this._sgl = null;
//				}
//
//				if (this._storeData != null)
//				{
//					this._storeData = null;
//				}
//
//				if (this._storeFilterData != null)
//				{
//					this._storeFilterData = null;
//				}
//
//				if (this._errMsgs != null)
//				{
//					this._errMsgs = null;
//				}
			}

			base.Dispose( disposing );
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
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(547, 402);
            // 
            // cbModelName
            // 
            //this.cbModelName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            //this.cbModelName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbModelName.Size = new System.Drawing.Size(114, 24);
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
            this.ugModel.Location = new System.Drawing.Point(12, 77);
            this.ugModel.Size = new System.Drawing.Size(630, 309);
            this.ugModel.TabIndex = 0;
            this.ugModel.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterRowsDeleted += new System.EventHandler(this.ugModel_AfterRowsDeleted);
            this.ugModel.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowUpdate);
            this.ugModel.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugModel_BeforeRowUpdate);
            this.ugModel.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugModel_BeforeCellDeactivate);
            this.ugModel.BeforeExitEditMode += new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugModel_BeforeExitEditMode);
            this.ugModel.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugModel_SelectionDrag);
            this.ugModel.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugModel_BeforeRowInsert);
            this.ugModel.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
            this.ugModel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugModel_KeyDown);
            this.ugModel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ugModel_KeyUp);
            this.ugModel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseDown);
            this.ugModel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseUp);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // SizeConstraintsFormBase
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(649, 460);
            this.Name = "SizeConstraintsFormBase";
            this.Text = "SizeConstraintsFormBase";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SizeMethodsFormBase_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
	#endregion

	#region Value Lists
		/// <summary>
		/// Sets up value lists that will be used on ugModel.  This method should be overridden.
		/// </summary>
		/// <param name="MyGrid">UltraGrid object</param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void InitializeValueLists(UltraGrid myGrid)
		{
			try
			{
				myGrid.DisplayLayout.ValueLists.Add("Colors");
				myGrid.DisplayLayout.ValueLists.Add("Rules");
				myGrid.DisplayLayout.ValueLists.Add("SortOrder");
				myGrid.DisplayLayout.ValueLists.Add("Sizes");
				myGrid.DisplayLayout.ValueLists.Add("Dimensions");
				myGrid.DisplayLayout.ValueLists.Add("SizeCell");
				// Begin Issue # 3685 - stodd 2/7/2006
				//myGrid.DisplayLayout.ValueLists["SizeCell"].SortStyle = ValueListSortStyle.Ascending;
				// End Issue # 3685 - stodd 2/7/2006
				myGrid.DisplayLayout.ValueLists.Add("DimensionCell");
				// Begin Issue # 3685 - stodd 2/7/2006
				//myGrid.DisplayLayout.ValueLists["DimensionCell"].SortStyle = ValueListSortStyle.Ascending;
				// End Issue # 3685 - stodd 2/7/2006
				myGrid.DisplayLayout.ValueLists.Add("ColorCell");
				myGrid.DisplayLayout.ValueLists["ColorCell"].SortStyle = ValueListSortStyle.Ascending;

				FillColorList(myGrid.DisplayLayout.ValueLists["Colors"]);

				FillTextTypeList(myGrid.DisplayLayout.ValueLists["Rules"], 
					eMIDTextType.eFillSizeHolesRuleType, 
					eMIDTextOrderBy.TextValue);

				FillTextTypeList(myGrid.DisplayLayout.ValueLists["SortOrder"], 
					eMIDTextType.eFillSizeHolesSort, 
					eMIDTextOrderBy.TextValue);

				if (_sizeConstraintProfile.SizeGroupRid != Include.NoRID)
				{
					FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
						_sizeConstraintProfile.SizeGroupRid, GetSizesUsing);

					FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
						_sizeConstraintProfile.SizeGroupRid, GetDimensionsUsing);
				}
				else
				{
					FillSizesList(myGrid.DisplayLayout.ValueLists["Sizes"],
						_sizeConstraintProfile.SizeCurveGroupRid, GetSizesUsing);

					FillSizeDimensionList(myGrid.DisplayLayout.ValueLists["Dimensions"],
						_sizeConstraintProfile.SizeCurveGroupRid, GetDimensionsUsing);
				}

			}
			catch (Exception ex)
			{
				HandleException(ex, "InitializeValueLists");
			}
		}


		/// <summary>
		/// Fills a UltraGrid ValueList with sizes based on a selected Size Group
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="SizeGroupRID">Size Group ID to find sizes</param>
		virtual protected void FillSizesList(ValueList valueList, int RID, eGetSizes getSizes)
		{
			MaintainSizeConstraints maint = new MaintainSizeConstraints(this._sizeModelData);
			_dtSizes = maint.FillSizesList(RID,getSizes);

			foreach (DataRow dr in _dtSizes.Rows)
			{
				valueList.ValueListItems.Add(Convert.ToInt32(dr["SIZE_CODE_RID"]), 
					dr["SIZE_CODE_PRIMARY"].ToString());
			}
		}


		/// <summary>
		/// Fills a UltraGrid ValueList with size dimensions.
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="SizeGroupRID">Size Group to find dimensions</param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void FillSizeDimensionList(ValueList valueList, int RID, eGetDimensions getDimensions)
		{
			MaintainSizeConstraints maint = new MaintainSizeConstraints(this._sizeModelData);
			this._dtDimensions = maint.FillSizeDimensionList(RID,getDimensions);

			foreach (DataRow dr in _dtDimensions.Rows)
			{
				valueList.ValueListItems.Add(Convert.ToInt32(dr["DIMENSIONS_RID"]), 
					dr["SIZE_CODE_SECONDARY"].ToString());
			}
		}

		/// <summary>
		/// Fills a UltraGrid ValueList with colors.
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		virtual protected void FillColorList(ValueList valueList)
		{
			ColorData cData = new ColorData();
			_dtColors = cData.Colors_Read();	
		
			foreach (DataRow dr in _dtColors.Rows)
			{
				valueList.ValueListItems.Add(Convert.ToInt32(dr["COLOR_CODE_RID"], 
					CultureInfo.CurrentUICulture),
					dr["COLOR_CODE_ID"].ToString() + " - " + dr["COLOR_CODE_NAME"].ToString());
			}

		}

	#endregion 

	#region Context Menu Event Handlers

		/// <summary>
		/// Applies data from current row to any child of the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void ug_allsize_apply(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugModel.ActiveRow;

				ugModel.EventManager.AllEventsEnabled = false;
				ugModel.PerformAction(UltraGridAction.ExitEditMode);
				ugModel.EventManager.AllEventsEnabled = true;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						if ((eSizeMethodRowType)Convert.ToInt32(activeRow.Cells["ROW_TYPE_ID"].Value,CultureInfo.CurrentUICulture) == eSizeMethodRowType.AllSize)
						{
							CopyAllSizeData(activeRow);
						}
						else
						{
							CopySetData(activeRow);
						}	
						break;
					case C_SET_ALL_CLR:
						CopyAllColorData(activeRow);
						break;
					case C_SET_CLR:
						CopyColorData(activeRow);
						break;
					case C_CLR_SZ:
						break;
					case C_ALL_CLR_SZ:
						break;
					case C_ALL_CLR_SZ_DIM:
						CopyAllColorDimensionData(activeRow);
						break;
					case C_CLR_SZ_DIM:
						CopyColorDimensionData(activeRow);
						break;
				}
				ugModel.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_apply");
			}		
		}


		/// <summary>
		/// Clears all data in ugModel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void ug_allsize_clear(object sender, System.EventArgs e)
		{
			try
			{
				UltraGridRow activeRow = ugModel.ActiveRow;

				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						if ((eSizeMethodRowType)Convert.ToInt32(activeRow.Cells["ROW_TYPE_ID"].Value,CultureInfo.CurrentUICulture) == eSizeMethodRowType.AllSize)
						{
							ClearAllSizeData(activeRow);
						}
						else
						{
							ClearSetData(activeRow);
						}
						break;
					case C_SET_ALL_CLR:
						ClearAllColorData(activeRow);
						break;
					case C_SET_CLR:
						ClearColorData(activeRow);
						break;
					case C_CLR_SZ:
						break;
					case C_ALL_CLR_SZ:
						break;
					case C_CLR_SZ_DIM:
						ClearColorDimensionData(activeRow);
						break;
					case C_ALL_CLR_SZ_DIM:
						ClearAllColorDimensionData(activeRow);
						break;
				}
				ugModel.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_clear");
			}		
		}


		/// <summary>
		/// Adds a child row to the current band.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void ug_allsize_addchildrow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugModel.ActiveRow;
				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						//SET LEVEL HAS ALL COLOR AND COLOR BAND,
						//ADDNEW IS ONLY ALLOWED FOR COLOR.
						//******************************************
						activeRow.Update();
						ugModel.ActiveRow = activeRow;
						activeRow.ChildBands[C_SET_CLR].Band.AddNew();
						break;
					default:
						//ADD ROW TO ACTIVEROWS FIRST CHILD BAND.
						//********************************************
						activeRow.Update();
						ugModel.ActiveRow = activeRow;
						activeRow.ChildBands[0].Band.AddNew();
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_addchildrow");
			}	
		}


		/// <summary>
		/// Adds a row to ugModel to the current band.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		virtual protected void ug_allsize_addrow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugModel.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugModel.ActiveRow = activeRow;
				}
				activeRow.Band.AddNew();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_addrow");
			}	
		}


		/// <summary>
		/// Deletes selected row(s) from ugModel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		virtual protected void ug_allsize_deleterow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugModel.ActiveRow;

				activeRow.Selected = true;
				ugModel.DeleteSelectedRows();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_deleterow");
			}	
		}


	#endregion

	#region Methods

		private bool VerifyBeforeInsert(UltraGridRow activeRow)
		{
			bool isValid = true;

			switch (activeRow.Band.Key.ToUpper())
			{
				case C_SET_CLR:
					isValid = IsColorCodeValid(activeRow);
					break;
				case C_ALL_CLR_SZ_DIM:
				case C_CLR_SZ_DIM:
					isValid = IsDimensionValid(activeRow);
					break;
				case C_CLR_SZ:
				case C_ALL_CLR_SZ:
					isValid = IsSizeValid(activeRow);
					break;
			}

			return isValid;
		}


		/// <summary>
		/// Tries to convert existing size_code_rid values to size_codes for the newly selected size dimension.
		/// If an existing value does not exist in the new dimension, it will be removed.
		/// </summary>
		/// <param name="childBand">Band of child values to modify</param>
		virtual protected void RemapSizesToNewDimension(UltraGridChildBand childBand)
		{
			UltraGridRow activeRow = ugModel.ActiveRow;

			if (activeRow.HasChild())
			{
				DataRow[] SelectRows = _dtSizes.Select("SIZE_CODE_SECONDARY = '" + ugModel.ActiveRow.Cells["DIMENSIONS_RID"].Text.ToString() + "'");

				//LOOP FROM LAST CHILD TO FIRST CHILD
				for (int i = childBand.Rows.Count - 1; i >= 0; i--)
				{
					UltraGridRow ugRow = childBand.Rows[i];

					if (ugRow.Cells["SIZE_CODE_RID"].Value != System.DBNull.Value)
					{
						bool rowFound = false;

						for (int index = 0; index <= SelectRows.Length - 1; index++)
						{
							if (ugRow.Cells["SIZE_CODE_RID"].Text.Trim() == SelectRows[index]["SIZE_CODE_PRIMARY"].ToString().Trim())
							{
								ugRow.Cells["SIZE_CODE_RID"].Value = Convert.ToInt32(SelectRows[index]["SIZE_CODE_RID"].ToString(),CultureInfo.CurrentUICulture);
								ugRow.Update();  //REMOVE PENCIL ICON FROM ROW SELECTOR.
								rowFound = true;
								break;
							}
						}

						if (!rowFound)
						{
							ugRow.Delete(false);
						}
					}
				}
			}
		}


		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugModel_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		virtual protected bool CreateColorCellList(UltraGridRow myRow, UltraGridBand childBand, ValueList valueList)
		{
			bool hasValues = false;
			valueList.ValueListItems.Clear();

			foreach (DataRow dr in _dtColors.Rows)
			{
				bool addVal = true;

				foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[childBand].Rows)
				{
					if (ugRow.Cells["COLOR_CODE_RID"].Value != System.DBNull.Value)
					{
						if (Convert.ToInt32(ugRow.Cells["COLOR_CODE_RID"].Value) == Convert.ToInt32(dr["COLOR_CODE_RID"]))
						{
							if (ugRow.Index != myRow.Index)
							{
								addVal = false;
							}
							break;
						}
					}
				}

				if (addVal)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(dr["COLOR_CODE_RID"]), 
												 dr["COLOR_CODE_ID"].ToString() + " - " + dr["COLOR_CODE_NAME"].ToString());
					hasValues = true;
				}
			}
			return hasValues;
		}



		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugModel_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		virtual protected bool CreateDimensionCellList(UltraGridRow myRow, UltraGridBand childBand, ValueList valueList)
		{
			bool hasValues = false;

			valueList.ValueListItems.Clear();

			foreach (DataRow dr in _dtDimensions.Rows)
			{
				bool addVal = true;

				foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[childBand].Rows)
				{
					
					if (ugRow.Cells["DIMENSIONS_RID"].Value != System.DBNull.Value)
					{
						if (Convert.ToInt32(ugRow.Cells["DIMENSIONS_RID"].Value) == Convert.ToInt32(dr["DIMENSIONS_RID"]))
						{
							if (ugRow.Index != myRow.Index)
							{
								addVal = false;
							}
							break;
						}
					}
				}

				if (addVal)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(dr["DIMENSIONS_RID"]), dr["SIZE_CODE_SECONDARY"].ToString());
					hasValues = true;
				}
			}

			return hasValues;
		}


		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugModel_AfterRowInsert event which determines if a new size should be allowed.
		/// 2) Creates the "SizeCell" value list to be used for the Size dropdown.
		/// </summary>
		/// <param name="myRow"></param>
		/// <param name="childBand"></param>
		/// <returns></returns>
		virtual protected bool CreateSizeCellList(UltraGridRow myRow, UltraGridBand childBand, ValueList valueList)
		{
			bool hasValues = false;
			
			DataRow[] SelectRows = _dtSizes.Select("SIZE_CODE_SECONDARY = '" + myRow.ParentRow.Cells["DIMENSIONS_RID"].Text.ToString() + "'");
			valueList.ValueListItems.Clear();

			for (int idx = 0; idx <= SelectRows.Length - 1; idx++)
			{
				bool addVal = true;

				foreach (UltraGridRow ugRow in myRow.ParentRow.ChildBands[childBand].Rows)
				{
					if (ugRow.Cells["SIZE_CODE_RID"].Value != System.DBNull.Value)
					{
						if (Convert.ToInt32(ugRow.Cells["SIZE_CODE_RID"].Value) == Convert.ToInt32(SelectRows[idx]["SIZE_CODE_RID"]))
						{
							if (ugRow.Index != myRow.Index)
							{
								addVal = false;
							}
							break;
						}
					}
				}

				if (addVal)
				{
					valueList.ValueListItems.Add(Convert.ToInt32(SelectRows[idx]["SIZE_CODE_RID"]), SelectRows[idx]["SIZE_CODE_PRIMARY"].ToString());
					hasValues = true;
				}
			}

			return hasValues;
		}


		/// <summary>
		/// Main validation routine for All Sizes Grid.  Each row will be sent to 
		/// have row validation done on it (IsActiveRowValid)
		/// </summary>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		/// <remarks>Method must be overridden</remarks>
		virtual protected bool IsGridValid()
		{
			try
			{
				bool IsValid = true;

				//================================================================================
				//WALK THE GRID - VALIDATING EACH ROW
				//================================================================================
				foreach(UltraGridRow setRow in ugModel.Rows)
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
					ugModel.Rows.ExpandAll(true);
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


		/// <summary>
		/// Validation for rows within All Sizes Grid.  Each row will broken down to individual cells, 
		/// these cells will then be validated.
		/// </summary>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		/// <remarks>Method must be overridden</remarks>
		virtual protected bool IsActiveRowValid(UltraGridRow activeRow)
		{
			try
			{
				bool IsValid = true;

				//COMMON BAND VALIDATIONS
				//==================================================================================
				if (!IsSizeMinValid(activeRow))
				{
					IsValid = false;
				}
						
				if (!IsSizeMaxValid(activeRow))
				{
					IsValid = false;
				}

				if (!IsSizeMultValid(activeRow))
				{
					IsValid = false;
				}

				if (!IsSizeQtyValid(activeRow))
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
						if (!IsFillZerosQtyValid(activeRow))
						{
							IsValid = false;
						}
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

		/// <summary>
		/// Validates a string for a numeric value.
		/// </summary>
		/// <param name="thisString">String to compare</param>
		/// <param name="thisType">System.Type to compare</param></param>
		/// <returns></returns>
		/// <remarks>Checks numeric for given System.Type by calling IsNumeric</remarks>
		virtual protected bool ValidateCellsNumeric(string thisString, System.Type thisType)
		{
			bool IsValid = true;

			if (thisString.Trim() != "")
			{
				if (!IsNumeric(thisString, thisType))
				{
					IsValid = false;
				}
			}

			return IsValid;
		}

		/// <summary>
		/// Validates a grid cell for a numeric value.
		/// </summary>
		/// <param name="thisCell">UltraGridCell to compare</param>
		/// <param name="thisType">System.Type to compare</param></param>
		/// <returns></returns>
		/// <remarks>Checks numeric for given System.Type by calling IsNumeric</remarks>
		virtual protected bool ValidateCellsNumeric(UltraGridCell thisCell, System.Type thisType)
		{
			bool IsValid = true;

			if (thisCell.Text.Trim() != "")
			{
				if (!IsNumeric(thisCell.Text.ToString(), thisType))
				{
					IsValid = false;
				}
			}

			return IsValid;
		}

		virtual protected bool IsNumeric(string thisValue, Type thisType)
		{
			bool Numeric = true;
			
			if (typeof(int) == thisType)
			{
				try
				{
					int.Parse(thisValue);
				}
				catch
				{
					Numeric = false;
				}
			}

			if (typeof(double) == thisType)
			{
				try
				{
					double.Parse(thisValue);
				}
				catch
				{
					Numeric = false;
				}
			}

			return Numeric;
		}


		/// <summary>
		/// Validates the size maximum cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeMaxValid(UltraGridRow thisRow)
		{
			bool IsValid = true;

			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["SIZE_MAX"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			//VERIFY VALUE IS AN INTEGER
			//==========================================
			if (!ValidateCellsNumeric(activeCell, typeof(int)))
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
			}
			else
			{
				//NO USE TO DO OTHER VALIDATIONS UNLESS THE VALUE IS VALID.
				//=========================================================
							
				//ADD ADDITIONAL VALIDATIONS HERE.
				if (activeCell.Text.Trim() != string.Empty)
				{
					if (Convert.ToInt32(activeCell.Text.Trim()) < 0)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
					}
				}
			}

			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Validates the size minimum cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeMinValid(UltraGridRow thisRow)
		{
			bool IsValid = true;

			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["SIZE_MIN"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			//VERIFY VALUE IS AN INTEGER
			//==========================================
			if (!ValidateCellsNumeric(activeCell, typeof(int)))
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
			}
			else
			{
				//=========================================================
				//ONLY DO OTHER VALIDATIONS IF THE VALUE IS VALID.
				//=========================================================

				//Validate > 0
				if (activeCell.Text.Trim() != string.Empty)
				{
					if (Convert.ToInt32(activeCell.Text.Trim()) < 0)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
					}
				}			

				//Size max must be numeric for this comparison.
				//=============================================
				if (ValidateCellsNumeric(thisRow.Cells["SIZE_MAX"], typeof(int)))
				{
					if (activeCell.Text.Trim() != "" &&
						thisRow.Cells["SIZE_MAX"].Text.Trim() != "")
					{
						if (Convert.ToInt32(activeCell.Text) > Convert.ToInt32(thisRow.Cells["SIZE_MAX"].Text))
						{
							ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueExceedsMaximum));
							IsValid = false;
						}
					}
				}

				if (ValidateCellsNumeric(thisRow.Cells["SIZE_MULT"], typeof(int)))
				{
					if (thisRow.Cells["SIZE_MULT"].Text.Trim() != "" && 
						activeCell.Text.Trim() != "")
					{
						if (Convert.ToInt32(thisRow.Cells["SIZE_MULT"].Text) < Convert.ToInt32(activeCell.Text))
						{
							IsValid = false;
							ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueExceedsMultiple));
						}
					}
				}

			}
			//==========================================

			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================


			return IsValid;

		}


		/// <summary>
		/// Validates the size multiple cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeMultValid(UltraGridRow thisRow)
		{
			bool IsValid = true;

			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["SIZE_MULT"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			//VERIFY VALUE IS AN INTEGER
			//==========================================
			if (!ValidateCellsNumeric(activeCell, typeof(int)))
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
			}
			else
			{
				//NO USE TO DO OTHER VALIDATIONS UNLESS THE VALUE IS VALID.
				//=========================================================

				//Validate > 1
				if (activeCell.Text.Trim() != string.Empty)
				{
					if (Convert.ToInt32(activeCell.Text.Trim()) < 1)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MultipleCannotBeLessThan1));
					}
				}

				//Size Max must be numeric for this validation
				if (ValidateCellsNumeric(thisRow.Cells["SIZE_MAX"], typeof(int)))
				{
					if (activeCell.Text.Trim() != "" &&
						thisRow.Cells["SIZE_MAX"].Text.Trim() != "")
					{
						if (Convert.ToInt32(activeCell.Text) > Convert.ToInt32(thisRow.Cells["SIZE_MAX"].Text))
						{
							ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ValueExceedsMaximum));
							IsValid = false;
						}
					}
				}

			}


			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;

		}


		/// <summary>
		/// Validates the size quantity cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeQtyValid(UltraGridRow thisRow)
		{
			bool IsValid = true;
						
			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["SIZE_QUANTITY"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			//VERIFY VALUE IS AN INTEGER
			//==========================================
			if (!ValidateCellsNumeric(activeCell, typeof(int)))
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
			}
			else
			{
				//NO USE TO DO OTHER VALIDATIONS UNLESS THE VALUE IS VALID.
				//=========================================================
				
				//Validate > 0
				if (activeCell.Text.Trim() != string.Empty)
				{
					if (Convert.ToInt32(activeCell.Text.Trim()) < 0)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
					}
				}

				//Validate value is present when specified rules are chosen.
				//if (thisRow.Cells["SIZE_RULE"].Value != System.DBNull.Value)
				if (thisRow.Cells["SIZE_RULE"].Text.Trim() != string.Empty)
				{
//					switch (base.ABM.MethodType)
//					{
//						case eMethodType.FillSizeHolesAllocation:
//						switch ((eFillSizeHolesRuleType)Convert.ToInt32(thisRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture))
//						{
//							case eFillSizeHolesRuleType.SizeFillUpTo:
//								if (activeCell.Text.Trim() == string.Empty)
//								{
//									IsValid = false;
//									ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FillUpToRuleQuantityRequired));
//								}
//								break;
//							default:
//								if (activeCell.Text.Trim() != string.Empty)
//								{
//									IsValid = false;
//									ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidFillUpToRuleQuantity));
//								}
//								break;
//						}
//							break;
//						case eMethodType.BasisSizeAllocation:
//						switch ((eBasisSizeMethodRuleType)Convert.ToInt32(thisRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture))
//						{
//							case eBasisSizeMethodRuleType.AbsoluteQuantity:
//								if (activeCell.Text.Trim() == string.Empty)
//								{
//									IsValid = false;
//									ErrorMessages.Add(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QuantityRuleQuantityRequired));
//								}
//								break;
//							default:
//								if (activeCell.Text.Trim() != string.Empty)
//								{
//									IsValid = false;
//									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidQuantityRuleQuantity));
//								}
//								break;
//						}
//							break;
//					}
				}
				else
				{
					//If no rule is selected the size quantity should not be filled.
					if (activeCell.Text.Trim() != string.Empty)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidRuleQuantity));
					}

				}
								
				//ADD ADDITIONAL VALIDATIONS HERE.
			}

			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Validates the fill zeros quantity cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsFillZerosQtyValid(UltraGridRow thisRow)
		{
			bool IsValid = true;
						
			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["FILL_ZEROS_QUANTITY"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			//VERIFY VALUE IS AN INTEGER
			//==========================================
			if (!ValidateCellsNumeric(activeCell, typeof(int)))
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
			}
			else
			{
				//NO USE TO DO OTHER VALIDATIONS UNLESS THE VALUE IS VALID.
				//=========================================================
				//ADD ADDITIONAL VALIDATIONS HERE.

				if (activeCell.Text.Trim() != string.Empty)
				{
					if (Convert.ToInt32(activeCell.Text.Trim()) < 0)
					{
						IsValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
					}
				}

				if ((bool)thisRow.Cells["FZ_IND"].Value)
				{
					if (activeCell.Text.Trim() == "")	
					{
						IsValid = false;
						activeCell.Appearance.Image = ErrorImage;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FillZeroRequired));
					}
				}
				else
				{
					if (activeCell.Text.Trim() != "")
					{
						IsValid = false;
						activeCell.Appearance.Image = ErrorImage;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidFillZeroEntry));
					}
				}
			}


			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Validates the color code rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsColorCodeValid(UltraGridRow thisRow)
		{
			bool IsValid = true;
						
			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["COLOR_CODE_RID"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;	
			//ADD ADDITIONAL VALIDATIONS HERE.
			if (activeCell.Value == System.DBNull.Value)
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ColorRequired));
			}


			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Validates the sizes_rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeValid(UltraGridRow thisRow)
		{
			bool IsValid = true;
			ErrorMessages.Clear();

			//UltraGridCell activeCell = thisRow.Cells["SIZES_RID"];
			UltraGridCell activeCell = thisRow.Cells["SIZE_CODE_RID"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;	
			//ADD ADDITIONAL VALIDATIONS HERE.

			if (activeCell.Value == System.DBNull.Value)
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeRequired));
			}

			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Validates the size_code_rid cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns></returns>
		virtual protected bool IsDimensionValid(UltraGridRow thisRow)
		{
			bool IsValid = true;
			ErrorMessages.Clear();
			//UltraGridCell activeCell = thisRow.Cells["SIZE_CODE_RID"];
			UltraGridCell activeCell = thisRow.Cells["DIMENSIONS_RID"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;	
			//ADD ADDITIONAL VALIDATIONS HERE.

			if (activeCell.Value == System.DBNull.Value)
			{
				IsValid = false;
				ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SizeDimensionRequired));
			}

			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!IsValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return IsValid;
		}


		/// <summary>
		/// Sets the initial layout of ugModel
		/// </summary>
		virtual protected void DefaultGridLayout() 
		{
			try
			{
				//Create any context menus that may be used on the grid.
				BuildContextMenus();

				//Set cancel update action
 				ugModel.RowUpdateCancelAction = RowUpdateCancelAction.RetainDataAndActivation;

				//Create the Value List Collections
				ugModel.DisplayLayout.ValueLists.Clear();
				InitializeValueLists(ugModel);

				ugModel.DisplayLayout.AddNewBox.Hidden = false;
				ugModel.DisplayLayout.Override.SelectTypeCell = SelectType.ExtendedAutoDrag;

				PositionColumns();
				CustomizeColumns();

			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.DefaultGridLayout");
			}	
		    
		}


		virtual protected void PositionColumns()
		{
			try
			{
				UltraGridColumn column;

				#region SET BAND

				if (ugModel.DisplayLayout.Bands.Exists(C_SET))
				{
					column = ugModel.DisplayLayout.Bands[C_SET].Columns["BAND_DSC"];
					column.Header.VisiblePosition = 0;
					column.Header.Caption = "Store Sets";
					column.Width = 200;

					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_MIN"];
					column.Header.VisiblePosition = 1;
					column.Header.Caption = "Min";

					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_MAX"];
					column.Header.VisiblePosition = 2;
					column.Header.Caption = "Max";


					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_MULT"];
					column.Header.VisiblePosition = 3;
					column.Header.Caption = "Mult";

//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"];
//					column.Header.VisiblePosition = 4;
//					column.Header.Caption = "Rule";
//					column.Hidden = true;
//
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"];
//					column.Header.VisiblePosition = 5;
//					column.Header.Caption = "Qty";
//					column.Hidden = true;

//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["FZ_IND"];
//					column.Header.VisiblePosition = 6;
//					column.Header.Caption = "Fill Zero";
//					column.Hidden = true;
//
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["FILL_ZEROS_QUANTITY"];
//					column.Header.VisiblePosition = 7;
//					column.Header.Caption = "Fill Zero Qty";
//					column.Hidden = true;
//
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["FSH_IND"];
//					column.Header.VisiblePosition = 8;
//					column.Header.Caption = "Fill Size Holes";
//					column.Hidden = true;
//
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["FILL_SEQUENCE"];
//					column.Header.VisiblePosition = 9;
//					column.Header.Caption = "Order";
//					column.Hidden = true;


					ugModel.DisplayLayout.Bands[C_SET].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET].Columns["ROW_TYPE_ID"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
					ugModel.DisplayLayout.Bands[C_SET].AddButtonCaption = "Set";
				}
				#endregion


				#region ALL COLOR BAND

				if (ugModel.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["BAND_DSC"].Header.VisiblePosition = 0;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["COLOR_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
					ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].AddButtonCaption = "All Color";
				}
				#endregion


				#region COLOR BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"].Header.VisiblePosition = 0;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["BAND_DSC"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_SET_CLR].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_SET_CLR].AddButtonCaption = "Color";
				}
				#endregion


				#region COLOR SIZE BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{

					column = ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;

//					column.SortIndicator = SortIndicator.Ascending;

					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_CLR_SZ].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].AddButtonCaption = "Size";

					// BEGIN MID Track #4989 - dimensions & sizes not sequenced;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"].SortIndicator = SortIndicator.Ascending;
					// END MID Track #4989
				}
				#endregion


				#region ALL COLOR SIZE BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;
//					column.SortIndicator = SortIndicator.Ascending;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].AddButtonCaption = "Size";

					// BEGIN MID Track #4989 - dimensions & sizes not sequenced;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"].SortIndicator = SortIndicator.Ascending;
					// END MID Track #4989
				}
				#endregion


				#region ALL COLOR DIMENSION BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;
//					column.SortIndicator = SortIndicator.Ascending;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";

					// BEGIN MID Track #4989 - dimensions & sizes not sequenced;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"].SortIndicator = SortIndicator.Ascending;
					// END MID Track #4989
					
				}
				#endregion


				#region COLOR DIMENSION BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;
//					column.SortIndicator = SortIndicator.Ascending;

					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_MIN"].Header.VisiblePosition = 1;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_MAX"].Header.VisiblePosition = 2;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_MULT"].Header.VisiblePosition = 3;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;


					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CONSTRAINT_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"].Hidden = true;
//					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true;

					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].ColHeadersVisible = false;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";

					// BEGIN MID Track #4989 - dimensions & sizes not sequenced;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;
					ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"].SortIndicator = SortIndicator.Ascending;
					// END MID Track #4989
				}
				#endregion
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.PositionColumns");
			}
		}


		virtual protected void CustomizeColumns()
		{
			try
			{
				UltraGridColumn column;

				#region SET BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_SET))
				{
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["FILL_SEQUENCE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["SortOrder"];
//					column.Width = 150;
//
//					column = ugModel.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
//					column.Width = 150;
				}
				#endregion

				#region ALL COLOR BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
//					column = ugModel.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR
				if (ugModel.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					column = ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugModel.DisplayLayout.ValueLists["Colors"];

//					column = ugModel.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR SIZE BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{
					column = ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugModel.DisplayLayout.ValueLists["Sizes"];
				
//					column = ugModel.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region ALL COLOR SIZE BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugModel.DisplayLayout.ValueLists["Sizes"];
			
//					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR DIM BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugModel.DisplayLayout.ValueLists["Dimensions"];

//					column = ugModel.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region ALL COLOR DIM BAND
				if (ugModel.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugModel.DisplayLayout.ValueLists["Dimensions"];
				
//					column = ugModel.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;
//					column.ValueList = ugModel.DisplayLayout.ValueLists["Rules"];
				}
				#endregion
			}
			catch (Exception ex)
			{
				HandleException(ex, "CustomizeColumns");
			}
		}


		/// <summary>
		/// Builds context menus to be used with ugModel
		/// </summary>
		/// <remarks>Base method is currently used by the following:
		/// Fill Size Holes Method, Basis Size Method</remarks>
		virtual protected void BuildContextMenus()
		{
			try
			{
				#region SET LEVEL
				MenuItem mnuSetApply = new MenuItem("Apply", new EventHandler(this.ug_allsize_apply));
				mnuSetApply.Index = C_CONTEXT_SET_APPLY;
				MenuItem mnuSetClear = new MenuItem("Clear", new EventHandler(this.ug_allsize_clear));
				mnuSetClear.Index = C_CONTEXT_SET_CLEAR;
				MenuItem mnuSetSeparator1= new MenuItem("-");
				mnuSetSeparator1.Index = C_CONTEXT_SET_SEP1;
				MenuItem mnuSetAddColor = new MenuItem("Add Color", new EventHandler(this.ug_allsize_addchildrow));
				mnuSetAddColor.Index = C_CONTEXT_SET_ADD_CLR;
				SetContext = new ContextMenu( new MenuItem[] {	
																 mnuSetApply, 
																 mnuSetClear, 
																 mnuSetSeparator1, 
																 mnuSetAddColor 
															 });
				#endregion


				#region COLOR LEVEL
				MenuItem mnuColorApply = new MenuItem("Apply", new EventHandler(this.ug_allsize_apply));
				mnuColorApply.Index = C_CONTEXT_CLR_APPLY;
				MenuItem mnuColorClear = new MenuItem("Clear", new EventHandler(this.ug_allsize_clear));
				mnuColorClear.Index = C_CONTEXT_CLR_CLEAR;
				MenuItem mnuColorSeparator1 = new MenuItem("-");
				mnuColorSeparator1.Index = C_CONTEXT_CLR_SEP1;
				MenuItem mnuColorAddColor = new MenuItem("Add Color", new EventHandler(this.ug_allsize_addrow));
				mnuColorAddColor.Index = C_CONTEXT_CLR_ADD_CLR;
				MenuItem mnuColorAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.ug_allsize_addchildrow));
				mnuColorAddSizeDim.Index = C_CONTEXT_CLR_ADD_SZ_DIM;
				MenuItem mnuColorSeparator2 = new MenuItem("-");
				mnuColorSeparator2.Index = C_CONTEXT_CLR_SEP2;
				MenuItem mnuColorDelete = new MenuItem("Delete", new EventHandler(this.ug_allsize_deleterow));
				mnuColorDelete.Index = C_CONTEXT_CLR_DELETE;

				ColorContext = new ContextMenu( new MenuItem[]  {
																	mnuColorApply,
																	mnuColorClear,
																	mnuColorSeparator1,
																	mnuColorAddColor,
																	mnuColorAddSizeDim,
																	mnuColorSeparator2,
																	mnuColorDelete
																});
				#endregion


				#region SIZE LEVEL
				MenuItem mnuSizeAddSize = new MenuItem("Add Size", new EventHandler(this.ug_allsize_addrow));
				mnuSizeAddSize.Index = C_CONTEXT_SZ_ADD_SZ;
				MenuItem mnuSizeSeparator1 = new MenuItem("-");
				mnuSizeSeparator1.Index = C_CONTEXT_SZ_SEP1;	
				MenuItem mnuSizeDelete = new MenuItem("Delete", new EventHandler(this.ug_allsize_deleterow));
				mnuSizeDelete.Index = C_CONTEXT_SZ_DELETE;
				SizeContext = new ContextMenu( new MenuItem[]  {   mnuSizeAddSize,
																   mnuSizeSeparator1,
																   mnuSizeDelete
															   });
				#endregion


				#region DIMENSION LEVEL
				MenuItem mnuDimensionApply = new MenuItem("Apply", new EventHandler(this.ug_allsize_apply));
				mnuDimensionApply.Index = C_CONTEXT_DIM_APPLY;
				MenuItem mnuDimensionClear = new MenuItem("Clear", new EventHandler(this.ug_allsize_clear));
				mnuDimensionClear.Index = C_CONTEXT_DIM_CLEAR;
				MenuItem mnuDimensionSeparator1 = new MenuItem("-");
				mnuDimensionSeparator1.Index = C_CONTEXT_DIM_SEP1;
				MenuItem mnuDimensionAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.ug_allsize_addrow));
				mnuDimensionAddSizeDim.Index = C_CONTEXT_DIM_ADD_DIM;
				MenuItem mnuDimensionAddSize = new MenuItem("Add Size", new EventHandler(this.ug_allsize_addchildrow));
				mnuDimensionAddSize.Index = C_CONTEXT_DIM_ADD_SZ;
				MenuItem mnuDimensionSeparator2 = new MenuItem("-");
				mnuDimensionSeparator2.Index = C_CONTEXT_DIM_SEP2;
				MenuItem mnuDimensionDelete = new MenuItem("Delete", new EventHandler(this.ug_allsize_deleterow));
				mnuDimensionDelete.Index = C_CONTEXT_DIM_DELETE;
				DimensionContext = new ContextMenu( new MenuItem[]  {
																		mnuDimensionApply,
																		mnuDimensionClear,
																		mnuDimensionSeparator1,
																		mnuDimensionAddSizeDim,
																		mnuDimensionAddSize,
																		mnuDimensionSeparator2,
																		mnuDimensionDelete
																	});
				#endregion

			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.BuildContextMenus");
			}
		}


        // begin TT#488 - MD - Jellis - Group Allocation (Code not used)
        ///// <summary>
        ///// Returns a AllocationProfileList for an array of headers.
        ///// </summary>
        ///// <param name="selectedHeaders">Array of headers to get profiles for</param>
        ///// <returns>AllocationProfileList object</returns>
        ///// <example>
        ///// <code>
        /////		int intHeaderRID = 100;
        /////		int[] selectedHeaders = {intHeaderRID};
        /////		AllocationProfileList apl = null;
        /////		apl = GetAllocationProfile(selectedHeaders);
        /////		_basisHeaderProfile = (AllocationProfile)apl[0];
        ///// </code> 
        /////</example>
        //protected AllocationProfileList GetAllocationProfile(int[] selectedHeaders)
        //{
        //    try
        //    {
        //        AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
        //        apl.LoadHeaders(SAB.ClientServerSession.CreateTransaction(), selectedHeaders, SAB.ApplicationServerSession);
        //        return apl;
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "GetAllocationProfile(int[] selectedHeaders)");
        //        return null;
        //    }
        //}


        ///// <summary>
        ///// Returns a AllocationProfile object for a single header.
        ///// </summary>
        ///// <param name="aHeaderRID">Header to get profile for</param>
        ///// <returns>AllocationProfile object</returns>
        ///// <example>
        ///// <code>
        /////		AllocationProfile _basisHeaderProfile = null;
        /////		int intHeaderRID = 100;
        /////		_basisHeaderProfile = GetAllocationProfile(intHeaderRID);
        ///// </code>
        ///// </example>
        //protected AllocationProfile GetAllocationProfile(int aHeaderRID)
        //{
        //    try
        //    {
        //        int[] selectedHeaders = {aHeaderRID};
				
        //        return (AllocationProfile)GetAllocationProfile(selectedHeaders)[0];

        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex, "GetAllocationProfile(int aHeaderRID)");
        //        return null;
        //    }
        //}
        // end TT#488 - MD - Jellis - Group Allocation (Code not used)


		

	

		/// <summary>
		/// Used to attach an error provider with messages to a Windows form control.
		/// </summary>
		/// <param name="pControl"></param>
		virtual protected void AttachErrors(Control pControl)
		{
			try
			{
				string Msg = "";

				for (int errIdx=0; errIdx <= ErrorMessages.Count - 1; errIdx++)
				{
					Msg = (errIdx == 0) ? ErrorMessages[errIdx].ToString() : Msg + "\n" + ErrorMessages[errIdx].ToString();
				}

				ErrorProvider.SetError(pControl, Msg);
				ErrorMessages.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.AttachErrors(Control pControl)");
			}	
		}


		/// <summary>
		/// Used to attach an error image and tag values to a UltraGridCell.  Tag may contain multiple error messages.
		/// </summary>
		/// <param name="activeCell"></param>
		virtual protected void AttachErrors(UltraGridCell activeCell)
		{
			try
			{
				activeCell.Appearance.Image = ErrorImage;

				for (int errIdx=0; errIdx <= ErrorMessages.Count - 1; errIdx++)
				{
					activeCell.Tag = (errIdx == 0) ? ErrorMessages[errIdx] : activeCell.Tag + "\n" + ErrorMessages[errIdx];
				}

				ErrorMessages.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.AttachErrors(UltraGridCell activeCell)");
			}	
		}


		protected DialogResult ShowWarningPrompt()
		{
			
			
			try
			{
				DialogResult drResult;

				drResult = DialogResult.Yes;
				ChangePending = false;
				ConstraintRollback = true;

				drResult = MessageBox.Show("Warning:\nChanging this value will cause the current constraint information to be immediately erased." +
										"\nTo return to the original constraint information close the form without saving or updating.\nDo you wish to continue?",
										"Confirmation",
										MessageBoxButtons.YesNo);

				if (drResult == DialogResult.Yes)
				{
					ChangePending = true;
					ConstraintRollback = true;
				}

				return drResult;
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.ShowWarningPrompt");
				return DialogResult.No;
			}	

			
		}


		virtual protected void CopyAllSizeData(UltraGridRow activeRow)
		{
			try
			{
				foreach(UltraGridRow setRow in ugModel.Rows)
				{
					if ((eSizeMethodRowType)Convert.ToInt32(setRow.Cells["ROW_TYPE_ID"].Value,CultureInfo.CurrentUICulture) != eSizeMethodRowType.AllSize)
					{
						setRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						setRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						setRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						setRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						setRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
//						setRow.Cells["FZ_IND"].Value = activeRow.Cells["FZ_IND"].Value;
//						setRow.Cells["FSH_IND"].Value = activeRow.Cells["FSH_IND"].Value;
//						setRow.Cells["FILL_ZEROS_QUANTITY"].Value = activeRow.Cells["FILL_ZEROS_QUANTITY"].Value;
//						setRow.Cells["FILL_SEQUENCE"].Value = activeRow.Cells["FILL_SEQUENCE"].Value;

						if (setRow.HasChild())
						{
							//ALL COLORS PATH
							//========================================================================
							foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
							{
								allColorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								allColorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								allColorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (allColorRow.HasChild())
								{
									foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
									{
										allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

										if (allColorDimRow.HasChild())
										{
											foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
											{
												allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
												allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
												allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
											}
										}
									}
								}
							}



							foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
							{
								colorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								colorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								colorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (colorRow.HasChild())
								{
									foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
									{
										colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

										if (colorDimRow.HasChild())
										{
											foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
											{
												colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
												colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
												colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//												colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//												colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
											}
										}
									}
								}
							}
						}
					}
				}
				ugModel.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopyAllSizeData");
			}
		}

		virtual protected void ClearAllSizeData(UltraGridRow activeRow)
		{
			try
			{
				foreach(UltraGridRow setRow in ugModel.Rows)
				{

                    setRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                    setRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                    setRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//					setRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//					setRow.Cells["SIZE_RULE"].Value = string.Empty;
//					setRow.Cells["FZ_IND"].Value = false;
//					setRow.Cells["FSH_IND"].Value = false;
//					setRow.Cells["FILL_ZEROS_QUANTITY"].Value = string.Empty;
//					setRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eFillSizeHolesSort.Ascending, CultureInfo.CurrentUICulture);

					if (setRow.HasChild())
					{
						//ALL COLORS PATH
						//========================================================================
						foreach(UltraGridRow allColorRow in setRow.ChildBands[0].Rows)
						{
                            allColorRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                            allColorRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                            allColorRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//							allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							allColorRow.Cells["SIZE_RULE"].Value = string.Empty;

							if (allColorRow.HasChild())
							{
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
								{
                                    allColorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                    allColorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                    allColorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//									allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
										{
                                            allColorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                            allColorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                            allColorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//											allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
										}
									}
								}
							}
						}


						foreach(UltraGridRow colorRow in setRow.ChildBands[1].Rows)
						{
                            colorRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                            colorRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                            colorRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//							colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//							colorRow.Cells["SIZE_RULE"].Value = string.Empty;

							if (colorRow.HasChild())
							{
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
								{
                                    colorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                    colorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                    colorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//									colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//									colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

									if (colorDimRow.HasChild())
									{
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
										{
                                            colorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                            colorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                            colorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//											colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//											colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
										}
									}
								}
							}
						}
					}
				}
				ugModel.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearAllSizeData");
			}
		}

		virtual protected void ClearSetData(UltraGridRow activeRow)
		{
			try
			{
                activeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;
//				activeRow.Cells["FZ_IND"].Value = false;
//				activeRow.Cells["FSH_IND"].Value = false;
//				activeRow.Cells["FILL_ZEROS_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["FILL_SEQUENCE"].Value = Convert.ToInt32(eFillSizeHolesSort.Ascending, CultureInfo.CurrentUICulture);
				
				if (activeRow.HasChild())
				{
					//COPY TO ALL COLOR ROW
					foreach(UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
					{
                        allColorRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        allColorRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        allColorRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						allColorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorRow.Cells["SIZE_RULE"].Value = string.Empty;

						if (allColorRow.HasChild())
						{
							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
							{
                                allColorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                allColorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                allColorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//								allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

								if (allColorDimRow.HasChild())
								{
									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
									{
                                        allColorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                        allColorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                        allColorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;									
									}
								}
							}
						}
					}

					//COPY TO COLOR
					foreach(UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
					{
                        colorRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        colorRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        colorRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						colorRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorRow.Cells["SIZE_RULE"].Value = string.Empty;


						if (colorRow.HasChild())
						{
							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
							{
                                colorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                colorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                colorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//								colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

								if (colorDimRow.HasChild())
								{
									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
									{
                                        colorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                        colorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                        colorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//										colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//										colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
									}
								}
							}
						}
					}
				}
				activeRow.Update();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearSetData");
			}
		}

		virtual protected void CopySetData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					//COPY TO ALL COLOR ROW
					foreach(UltraGridRow allColorRow in activeRow.ChildBands[0].Rows)
					{
						allColorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						allColorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						allColorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;


						if (allColorRow.HasChild())
						{
							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
							{
								allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (allColorDimRow.HasChild())
								{
									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
									{
										allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
									}
								}
							}
						}
					}

					//COPY TO COLOR
					foreach(UltraGridRow colorRow in activeRow.ChildBands[1].Rows)
					{
						colorRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						colorRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						colorRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;


						if (colorRow.HasChild())
						{
							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
							{
								colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (colorDimRow.HasChild())
								{
									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
									{
										colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//										colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//										colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopySetData");
			}
				
		}
		virtual protected void CopyColorData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorDimRow in activeRow.ChildBands[0].Rows)
					{
						colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
								colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopyColorData");
			}
		}

		virtual protected void ClearColorData(UltraGridRow activeRow)
		{
			try
			{
                activeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorDimRow in activeRow.ChildBands[0].Rows)
					{
                        colorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        colorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        colorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
                                colorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                colorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                colorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//								colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearColorData");
			}

		}

		virtual protected void CopyAllColorData(UltraGridRow activeRow)
		{
			try
			{
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorDimRow in activeRow.ChildBands[0].Rows)
					{
						allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
								allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//								allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//								allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopyAllColorData");
			}

		}

		virtual protected void ClearAllColorData(UltraGridRow activeRow)
		{
			try
			{
                activeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorDimRow in activeRow.ChildBands[0].Rows)
					{
                        allColorDimRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        allColorDimRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        allColorDimRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
                                allColorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                                allColorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                                allColorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//								allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//								allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearAllColorData");
			}

		}

		virtual protected void CopyAllColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorSizeRow in activeRow.ChildBands[0].Rows)
					{
						allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopyAllColorSizeData");
			}
		}

		virtual protected void ClearAllColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
                activeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorSizeRow in activeRow.ChildBands[0].Rows)
					{
                        allColorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        allColorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        allColorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearAllColorSizeData");
			}
		}

		virtual protected void CopyColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorSizeRow in activeRow.ChildBands[0].Rows)
					{
						colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
						colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
						colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
//						colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
//						colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "CopyColorSizeData");
			}
		}

		virtual protected void ClearColorDimensionData(UltraGridRow activeRow)
		{
			try
			{
                activeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                activeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorSizeRow in activeRow.ChildBands[0].Rows)
					{
                        colorSizeRow.Cells["SIZE_MIN"].Value = System.DBNull.Value;
                        colorSizeRow.Cells["SIZE_MAX"].Value = System.DBNull.Value;
                        colorSizeRow.Cells["SIZE_MULT"].Value = System.DBNull.Value;
//						colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
//						colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearColorSizeData");
			}
		}



		/// <summary>
		/// Binds ugModel datagrid
		/// </summary>
		/// <remarks>
		/// If the datasource will not be a dataset, then override the overloaded method
		///	that takes no arguments.
		/// </remarks>
		/// <param name="dsGridData">Dataset to bind to ugModel</param>
		virtual protected void BindAllSizeGrid(DataSet dsGridData)
		{
			try
			{
			
				if (DataSetBackup == null)
				{
					DataSetBackup = dsGridData.Copy(); //dsGridData.Copy();
				}

				this.ugModel.DataSource = null;
				this.ugModel.DataSource = dsGridData;
				//this.ugModel.DataBind();
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindAllSizeGrid");
			}
		}


		/// <summary>
		/// Override this method to bind to a datasource other than a dataset
		/// </summary>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void BindAllSizeGrid()
		{
			throw new Exception("Can not call base method BindAllSizeGrid().");
		}

		
	#endregion

	#region Control Event Handlers

//Begin     TT#1638 - MD - Revised Model Save - RBeck
        //private void cboFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    ChangePending = true;
        //}
//End     TT#1638 - MD - Revised Model Save - RBeck

		private void ugModel_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (ugModel.Selected.Cells.Count > 0)
				{
					ugModel.DoDragDrop(ugModel.Selected.Cells, DragDropEffects.Move);
					return;
				}
			
				if (ugModel.Selected.Rows.Count > 0)
				{
					ugModel.DoDragDrop(ugModel.Selected.Rows, DragDropEffects.Move);
					return;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugModel_SelectionDrag");
			}
		}


		/// <summary>
		/// Sets the pending change flag.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugModel_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
		}


		/// <summary>
		/// Sets the pending change flag.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugModel_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			ChangePending = true;
		}


		/// <summary>
		/// Presents a tooltip to user displaying validation messages that are held in the cell tag.
		/// Uses a method in the MIDFormBase to do so.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugModel_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{

				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(UltraGridCell));
				if (gridCell != null) 
				{
					switch (gridCell.Column.Style)
					{
						case Infragistics.Win.UltraWinGrid.ColumnStyle.TriStateCheckBox:
						case Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox:
							ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
							break;
						default:
						switch (gridCell.Column.DataType.Name.ToUpper())
						{
							case "BOOLEAN":
								ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
								break;
							default:
								ugModel.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
								break;
						}
							break;
					}
				}


				ShowUltraGridToolTip(ugModel, e);
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugModel_MouseEnterElement");
			}
		}


		private void ugModel_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = e;
		}


		private void ugModel_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = null;
		}


		/// <summary>
		/// Determines if right click, and where the grid was right clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugModel_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				Point point = new Point(e.X, e.Y);
				Infragistics.Win.UIElement mouseUIElement;
				Infragistics.Win.UIElement headerUIElement;
				Infragistics.Win.UltraWinGrid.UltraGridCell mouseCell;
				Infragistics.Win.UltraWinGrid.RowSelectorUIElement selectorUIElement;

				// RETRIEVE THE UIELEMENT FROM THE LOCATION OF THE MOUSE.
				mouseUIElement = ugModel.DisplayLayout.UIElement.ElementFromPoint(point);


				if (e.Button == MouseButtons.Right)
				{
					ugModel.ContextMenu = null;
					_showContext = false;

					if ( mouseUIElement == null ) 
					{ 
						return; 
					}

					// WAS A ROW SELECTOR CLICKED?
					selectorUIElement = (RowSelectorUIElement)mouseUIElement.GetAncestor(typeof(RowSelectorUIElement));
					if (selectorUIElement != null)
					{
						if (FunctionSecurity.AllowUpdate)
						{
							_showContext = true;
						}
						else
						{
							_showContext = false;
						}
						ugModel.ActiveRow = selectorUIElement.Row;
						return;
					}


					headerUIElement = mouseUIElement.GetAncestor(typeof(HeaderUIElement));
					
					//*********************************************
					// IF IT WASN'T A ROW SELECTOR WHAT WAS IT?
					// HEADER OR CELL.
					//*********************************************
					if(headerUIElement == null)
					{
						// TRY RETRIEVING THE CELL FROM UIELEMENT.
						mouseCell = (UltraGridCell)mouseUIElement.GetContext(typeof(UltraGridCell));

						//**************************************************
						// IF THE USER IS ON A CELL OBJECT, SET IT ACTIVE
						// AND SET FLAG TO SHOW THE CONTEXT MENU.
						//**************************************************
						if (mouseCell != null)
						{
							ugModel.ActiveCell = mouseCell;
							if (FunctionSecurity.AllowUpdate)
							{
								_showContext = true;
							}
							else
							{
								_showContext = false;
							}
						}
					}
					else
					{
						//IT WAS A HEADER DON'T DO ANYTHING
						return;
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugModel_MouseDown");
			}

		}


		private void ugModel_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (_gridKeyEvent == null || !_gridKeyEvent.Control)
				{
					ugModel.PerformAction(UltraGridAction.EnterEditMode);
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugModel_MouseUp");
			}
		}


		/// <summary>
		/// Sets the initial layout of ugModel grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugModel_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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
			DefaultGridLayout();
	
		}


		/// <summary>
		/// Refresh the rows after updating specific bands.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ugModel_AfterRowUpdate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				UltraGrid myGrid = (UltraGrid)sender;

				switch (e.Row.Band.Key.ToUpper())
				{
					//FOR PERFORMANCE ONLY DO FOR BANDS THAT WE KNOW COULD HAVE 
					//CHILD ROWS.
					case C_SET_CLR:
					case C_ALL_CLR_SZ_DIM:
					case C_CLR_SZ_DIM:
						//KEEPS CHILD ROWS VISIBLE

							myGrid.Rows.Refresh(RefreshRow.ReloadData, true);
							//Use this line for Infragistics 2004 Vol 2.
							//myGrid.Rows.Refresh(RefreshRow.ReloadData, true, false);

							//RESET THE ACTIVE ROW.  IF AFTERROWUPDATE WAS TRIGGERED BY THE
							//ADDING A NEW CHILD ROW FOR ONE OF THE ABOVE BANDS.
							//IF REMOVED 'UNABLE TO DETERMINE WHERE TO ADD ROW' ERROR
							//WILL BEGIN APPEARING.
							myGrid.ActiveRow = e.Row;
						break;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugModel_AfterRowUpdate");
			}
		}


		private void ugModel_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				UltraGrid myGrid = (UltraGrid)sender;
				UltraGridRow activeRow = myGrid.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugModel.ActiveRow = activeRow;
				}

				//Fixes an issue with the SortIndicator being set.
				//***********************************************************
				if (activeRow.HasChild() && activeRow.Expanded == false)
				{
					activeRow.Expanded = true;
				}
				//***********************************************************

				if (!VerifyBeforeInsert(activeRow))
				{
					e.Cancel = true;
				}

			}
			catch( Exception ex )
			{
				e.Cancel = true;
				HandleException(ex, "ugModel_BeforeRowInsert");
			}
		}


		private void ugModel_BeforeExitEditMode(object sender, Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs e)
		{
			try
			{
//				if (EditByCell)
//				{
//					UltraGridCell activeCell = ((UltraGrid)sender).ActiveCell;
//					UltraGridRow activeRow = activeCell.Row;
//					IsActiveRowValid(activeRow);
//				}
			}
			catch( Exception ex )
			{
				e.Cancel = true;
				HandleException(ex, "ugModel_BeforeExitEditMode");
			}

		}


		private void ugModel_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				UltraGrid myGrid = (UltraGrid)sender;
				UltraGridRow myRow = myGrid.ActiveRow;

				switch (myRow.Band.Key.ToUpper())
				{
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						if (myGrid.ActiveCell.Column.Key == "SIZE_CODE_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (myGrid.ActiveCell.Column.Key == "DIMENSIONS_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
					case C_SET_CLR:
						if (myGrid.ActiveCell.Column.Key == "COLOR_CODE_RID")
						{
							//SWAP THE VALUE LIST SO THAT THE CORRECT TEXT WILL DISPLAY
							myGrid.ActiveCell.ValueList = null;
						}
						break;
				}

			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugModel_BeforeCellDeactivate");
			}
		}


		private void ugModel_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			try
			{
				//bool IsValid = true;

				//IsValid = VerifyBeforeInsert(((UltraGrid)sender).ActiveRow);

				if (!VerifyBeforeInsert(((UltraGrid)sender).ActiveRow))
				{
					e.Cancel = true;
				}

			}
			catch( Exception ex )
			{
				e.Cancel = true;
				HandleException(ex, "ugModel_BeforeRowUpdate");
			}
		}


		private void SizeMethodsFormBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				this.ugModel.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ugModel_SelectionDrag);
				this.ugModel.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseDown);
				this.ugModel.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugModel_MouseUp);
				this.ugModel.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowUpdate);
				this.ugModel.BeforeExitEditMode -= new Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventHandler(this.ugModel_BeforeExitEditMode);
				this.ugModel.AfterRowsDeleted -= new System.EventHandler(this.ugModel_AfterRowsDeleted);
				this.ugModel.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
				this.ugModel.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugModel_BeforeRowUpdate);
				this.ugModel.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugModel_BeforeRowInsert);
				this.ugModel.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugModel_KeyDown);
				this.ugModel.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.ugModel_KeyUp);
				this.ugModel.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
				this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugModel);
                //End TT#169
				this.ugModel.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugModel_BeforeCellDeactivate);

				if (this._sizeContext != null)
				{
					this._sizeContext.Dispose();
				}
				if (this._dimensionContext != null)
				{
					this._dimensionContext.Dispose();
				}
				if (this._colorContext != null)
				{
					this._colorContext.Dispose();
				}
				if (this._setContext != null)
				{
					this._setContext.Dispose();
				}

			}
			catch
			{

			}
		}


	#endregion

		/// <summary>
		/// Fills the constraint collection.  Collection can be retrieved with CollectionSets property.
		/// </summary>
		virtual public void FillCollections()
		{
			int setIdx;
			int allColorIdx;
			int colorIdx;
			int allColorSizeIdx;
			int colorSizeIdx;
			int	colorSizeDimIdx;
			int allColorSizeDimIdx;

			if (_setsCollection != null)
			{
				_setsCollection.Clear();
			}

			_setsCollection = new CollectionSets();

			if (SizeConstraints.Tables["SetLevel"].Rows.Count > 0)
			{
				//FOR EACH SET LEVEL
				foreach (DataRow drSet in SizeConstraints.Tables["SetLevel"].Rows)
				{
					setIdx = _setsCollection.Add(new ItemSet(_sizeConstraintProfile.Key, drSet));

					//ALL COLOR
					foreach (DataRow drAllColor in drSet.GetChildRows("SetAllColor"))
					{
						allColorIdx = _setsCollection[setIdx].collectionAllColors.Add(new ItemAllColor(_sizeConstraintProfile.Key, drAllColor));

						//SIZE DIMENSION
						if (SizeConstraints.Relations.Contains("AllColorSizeDimension"))
						{
							foreach (DataRow drACSizeDim in drAllColor.GetChildRows("AllColorSizeDimension"))
							{
								allColorSizeDimIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(_sizeConstraintProfile.Key, drACSizeDim));
									
								//SIZE
								foreach (DataRow drACSize in drACSizeDim.GetChildRows("AllColorSize"))
								{
									allColorSizeIdx = _setsCollection[setIdx].collectionAllColors[allColorIdx].collectionSizeDimensions[allColorSizeDimIdx].collectionSizes.Add(new ItemSize(_sizeConstraintProfile.Key, drACSize));
								}
							}
						}
					}

					//COLOR
					foreach (DataRow drColor in drSet.GetChildRows("SetColor"))
					{
						colorIdx = _setsCollection[setIdx].collectionColors.Add(new ItemColor(_sizeConstraintProfile.Key, drColor));

						//SIZE DIMENSION
						if (SizeConstraints.Relations.Contains("ColorSizeDimension"))
						{
							foreach (DataRow drCSizeDim in drColor.GetChildRows("ColorSizeDimension"))
							{
								colorSizeDimIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions.Add(new ItemSizeDimension(_sizeConstraintProfile.Key, drCSizeDim));
									
								//SIZE
								foreach (DataRow drCSize in drCSizeDim.GetChildRows("ColorSize"))
								{
									colorSizeIdx = _setsCollection[setIdx].collectionColors[colorIdx].collectionSizeDimensions[colorSizeDimIdx].collectionSizes.Add(new ItemSize(_sizeConstraintProfile.Key, drCSize));
								}
							}
						}
					}
				}
			}
		}


		/// <summary>
		/// Fills the SizeConstraints DataSet.
		/// </summary>
		/// <returns></returns>
		virtual public bool CreateConstraintData()
		{
			bool Success = true;

			try
			{
				SizeConstraints = MIDEnvironment.CreateDataSet();

				switch (GetSizesUsing)
				{
					case eGetSizes.SizeGroupRID:
						if (SizeGroupRid != Include.NoRID)
						{
							GetSizes = true;
						}
						else
						{
							GetSizes = false;
						}
						break;
					case eGetSizes.SizeCurveGroupRID:
						if (SizeCurveGroupRid != Include.NoRID)
						{
							GetSizes = true;
						}
						else
						{
							GetSizes = false;
						}
						break;
				}

                // Begin TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
				//SizeConstraints = SizeModelData.GetChildData(_sizeConstraintProfile.Key, _sizeConstraintProfile.StoreGroupRid, GetSizes);
                SizeConstraints = SizeModelData.GetChildData(_sizeConstraintProfile.Key, _sizeConstraintProfile.StoreGroupRid, GetSizes, StoreMgmt.StoreGroup_GetVersion(_sizeConstraintProfile.StoreGroupRid));
                // End TT#1901-MD - JSmith - Versioning Test - make a str inactive and process Fill Size Holes - Receive a system argument exception
								
				DetermineSizeSequence(); // MID Track #4989 - dimensions & sizes not sequenced;

				FillCollections();

				//DebugSetsCollection();

//				CollectionDecoder cd = new CollectionDecoder(_setsCollection);
//
//				MinMaxItemBase minMax = (MinMaxItemBase) cd.GetItem(148, 27, 973);
			}
			catch
			{
				Success = false;
				throw;
			}
			return Success;

		}

		// BEGIN MID Track #4989 - dimensions & sizes not sequenced;
		private void DetermineSizeSequence()
		{
			try
			{
				SizeGroupProfile sgp = null;
				SizeCurveGroupProfile scgp; 

				if (SizeGroupRid != Include.NoRID)
				{
					sgp = new SizeGroupProfile(SizeGroupRid);
				}
				else if (SizeCurveGroupRid != Include.NoRID)
				{
					scgp = new SizeCurveGroupProfile(SizeCurveGroupRid);
					sgp = new SizeGroupProfile(scgp.DefinedSizeGroupRID);
				}
  
				if (sgp != null)
				{
					if (SizeConstraints.Tables.Contains(C_ALL_CLR_SZ_DIM))
					{
						SetSizeSequence(sgp, SizeConstraints.Tables[C_ALL_CLR_SZ_DIM]);
					}
					if (SizeConstraints.Tables.Contains(C_ALL_CLR_SZ))
					{
						SetSizeSequence(sgp, SizeConstraints.Tables[C_ALL_CLR_SZ]);
					}
					if (SizeConstraints.Tables.Contains(C_CLR_SZ_DIM))
					{
						SetSizeSequence(sgp, SizeConstraints.Tables[C_CLR_SZ_DIM]);
					}
					if (SizeConstraints.Tables.Contains(C_CLR_SZ))
					{
						SetSizeSequence(sgp, SizeConstraints.Tables[C_CLR_SZ]);
					}
					SizeConstraints.AcceptChanges();
				}
			}
			catch
			{
				throw;
			}
		}	
		
		/// <summary>
		/// Add sequence column and determine value; then sort
		/// </summary>
		private void SetSizeSequence(SizeGroupProfile aSizeGroupProfile, DataTable dtSizeCollection)
		{
			if (dtSizeCollection != null)
			{
				foreach	(DataRow aRow in dtSizeCollection.Rows)
				{
					int sizeCodeRid = Convert.ToInt32(aRow["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
					int dimensionsRid = Convert.ToInt32(aRow["DIMENSIONS_RID"], CultureInfo.CurrentUICulture);
					if (sizeCodeRid != Include.NoRID)
					{
						aRow["SIZE_SEQ"] = GetSizeCodeSequence(aSizeGroupProfile, sizeCodeRid);
					}
					else if (dimensionsRid != Include.NoRID)
					{
						aRow["SIZE_SEQ"] = GetDimensionSequence(aSizeGroupProfile, dimensionsRid);
					}
				}
				dtSizeCollection.DefaultView.Sort = "SIZE_SEQ";	
			}
		}

		private int GetSizeCodeSequence(SizeGroupProfile aSizeGroupProfile, int sizeCodeRid)
		{
			int seq = int.MaxValue;
			if (aSizeGroupProfile.SizeCodeList.Contains(sizeCodeRid))
			{
				SizeCodeProfile scp = (SizeCodeProfile)aSizeGroupProfile.SizeCodeList.FindKey(sizeCodeRid);
				seq = scp.PrimarySequence;
			}
			return seq;
		}
		 
		private int GetDimensionSequence(SizeGroupProfile aSizeGroupProfile, int dimensionRid)
		{
			int seq = int.MaxValue;
			foreach (SizeCodeProfile scp in aSizeGroupProfile.SizeCodeList)
			{
				if (scp.SizeCodeSecondaryRID == dimensionRid)
				{ 
					seq = scp.SecondarySequence;
					break;
				}
			}
			return seq;
		}
		// END MID Track #4989  

		public void DebugSetsCollection()
		{
			//PROCESS SETS AND ALL DESCENDANTS
			Debug.WriteLine("---------------------------------------------");
			foreach (ItemSet oItemSet in _setsCollection)
			{
				Debug.WriteLine("RID " + oItemSet.MethodRid.ToString() +
					" SGL" + oItemSet.SglRid.ToString() + " MIN " + oItemSet.Min.ToString() +
					" MAX " + oItemSet.Max.ToString() + " MULT " + oItemSet.Mult.ToString());
									
				//PROCESS ALL COLOR LEVEL AND ALL DESCENDANTS
				foreach (ItemAllColor oItemAllColor in oItemSet.collectionAllColors)
				{
					DebugItem(1, oItemAllColor);

					foreach (ItemSizeDimension oItemSizeDimension in oItemAllColor.collectionSizeDimensions)
					{
						DebugItem(2, oItemSizeDimension);

						foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
						{
							DebugItem(3, oItemSize);
						}
					}
				}

				//PROCESS COLOR LEVEL AND ALL DESCENDANTS
				foreach (ItemColor oItemColor in oItemSet.collectionColors)
				{
					DebugItem(1, oItemColor);

					foreach (ItemSizeDimension oItemSizeDimension in oItemColor.collectionSizeDimensions)
					{
						DebugItem(2, oItemSizeDimension);

						foreach (ItemSize oItemSize in oItemSizeDimension.collectionSizes)
						{
							DebugItem(3, oItemSize);
						}

					}
				}
			}


		}

		private void DebugItem(int level, MinMaxItemBase pItem)
		{
			string sLevel = string.Empty;
			switch (level)
			{
				case 1:
					sLevel = "  ";
					break;
				case 2: 
					sLevel = "    ";
					break;
				case 3:
					sLevel = "      ";
					break;
				default:
					sLevel = "--";
					break;
			}



			Debug.WriteLine(sLevel + "SGL " + pItem.SglRid.ToString() +
				" COLOR " + pItem.ColorCodeRid.ToString() +
				" DIM " + pItem.DimensionsRid.ToString() +
				" SIZE/SIZES " + pItem.SizeCodeRid.ToString() + "/" + pItem.SizesRid.ToString() +
				" MIN " + pItem.Min.ToString() +
				" MAX " + pItem.Max.ToString() + 
				" MULT " + pItem.Mult.ToString());
		}


		/// <summary>
		/// Public method that will delete method constraint data using
		/// SizeMethodBaseData.DeleteSizeConstraints
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <remarks>
		/// If connection is closed, method will handle opening, commit|rollback, and closing.
		/// </remarks>
		/// <returns></returns>
		virtual public bool DeleteSizeConstraintChildren(TransactionData td)
		{
			MaintainSizeConstraints maint = new MaintainSizeConstraints(this.SizeModelData);
			try
			{
				return maint.deleteSizeConstraintChildren(this.SizeConstraintModelRid, td);
			}
			catch 
			{
				throw;
			}
		}


		/// <summary>
		/// Public method that will insert method constraint data using
		/// SizeMethodBaseData.ProcessGroupLevel and SizeMethodBaseData.ProcessMinMax
		/// </summary>
		/// <param name="td">TransactionData object</param>
		/// <remarks>
		/// If connection is closed, method will handle opening, commit|rollback, and closing.
		/// </remarks>
		/// <returns></returns>
		virtual public bool InsertUpdateSizeConstraints(TransactionData td)
		{
			bool Successfull = true;
			MaintainSizeConstraints maint = new MaintainSizeConstraints(this.SizeModelData);

			try
			{	
				FillCollections();
				Successfull = maint.insertUpdateCollection(this.SizeConstraintModelRid,td,_setsCollection);
			}
			catch 
			{
				throw;
			}

			return Successfull;

		}

		/// <summary>
		/// Determines if the constraint data should be restored for this method.
		/// </summary>
		override protected void AfterClosing()
		{
			TransactionData td = new TransactionData();

			try
			{
				if (ResultSaveChanges != DialogResult.None)
				{
					if (ResultSaveChanges == DialogResult.No)
					{
						//ONLY ROLLBACK IF UPDATING THE METHOD
						if (_sizeConstraintProfile.ModelChangeType == eChangeType.update)
						{
							//CREATE AND OPEN CONNECTION HERE SO THE POSSIBLE ROLLBACKS
							//ARE ON THE SAME TRANSACTION.  THE INSERT UPDATES WILL
							//OPEN AND COMMIT/ROLLBACK DATA UNLESS THE PROVIDED TRANSACTIONDATA
							//OBJECT IS ALREADY OPEN.
							td = new TransactionData();

							if (ConstraintRollback)
							{
								if (!td.ConnectionIsOpen) 
								{
									td.OpenUpdateConnection();
								}								
								_sizeConstraints = DataSetBackup;
								InsertUpdateSizeConstraints(td);

								if (td.ConnectionIsOpen) 
								{
									td.CommitData();
									td.CloseUpdateConnection();
								}
							}
						}
					}
				}
				CheckForDequeue();	// MID Track #4970
			}
			catch (Exception ex)
			{
				if (td.ConnectionIsOpen) 
				{
					td.Rollback();
					td.CloseUpdateConnection();
				}
				HandleException(ex, "SizeConstraintsFromBase.AfterClosing");
			}
		}

		// BEGIN MID Track #4970 - modify to emulate other models 
		public SizeConstraintModelProfile GetModelForUpdate(int aModelRID)
		{
			try
			{
				return (SizeConstraintModelProfile)SAB.HierarchyServerSession.GetModelDataForUpdate(eModelType.SizeConstraints, aModelRID, true);  
			}
			catch 
			{
				throw;
			}
		}
		
		public SizeConstraintModelProfile GetSizeConstraintModelData(int aModelRID)
		{
			try
			{
				return (SizeConstraintModelProfile)SAB.HierarchyServerSession.GetSizeConstraintModelData(aModelRID);  
			}
			catch 
			{
				throw;
			}
		}

		public void CheckForDequeue()
		{
			try
			{
				if (_modelLocked)
				{
					SAB.HierarchyServerSession.DequeueModel(eModelType.SizeConstraints, SizeConstraintModelRid);
					_modelLocked = false;
				}
			}
			catch(Exception exception)
			{
				HandleException(exception);
			}
		}
		// END MID Track #4970  
	}
}
