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
using System.Text;
using System.Text.RegularExpressions;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SizeMethodsFormBase.
	/// </summary>
	public class SizeMethodsFormBase : WorkflowMethodFormBase
	{
		protected Controls.MIDComboBoxEnh cboSizeGroup;

		#region Form Fields

		protected System.Windows.Forms.Label lblStoreAttribute;
        // Begin Track #4872 - JSmith - Global/User Attributes
        //protected System.Windows.Forms.ComboBox cboStoreAttribute;
        protected MIDAttributeComboBox cboStoreAttribute;
        // End Track #4872
        protected Controls.MIDComboBoxEnh cboFilter;
		protected System.Windows.Forms.Label lblFilter;
		#endregion

		#region Member Variables
		private DataSet _dsBackup = null; //Copy of the dataset that is bound to the grid
		private DataTable _dtSizes = null;
		private DataTable _dtColors = null;
		private DataTable _dtDimensions = null;
		private DataTable _dtCurveHierLevel = null;	// A&F Generic Size Curve
		private DataTable _dtConstraintHierLevel = null;	// A&F Generic Size Constraint

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
        private FilterData _storeFilterData = null;
		private SizeModelData _sizeModelData = null;
		//private CollectionSets _ruleCollectionSets;
		private HeaderCharGroupProfileList _headerCharGroupProfileList;	// A&F Generic Size Curve 
		private bool _textChanged = false;
		private bool _priorError = false;
		private int _lastMerchIndex = -1;
		private string _noHeaderCharSelectedLabel; // MID Track 4372 Generic Size Constraint
		private string _noHierLevelSelectedLabel; // MID Track 4372 Generic Size Constraint
        private string _noNameExtensionSelectedLabel;   // TT#413-RMatelic- Add new Generic Size Curve names 

        // Begin Track #4872 - JSmith - Global/User Attributes
        private eChangeType _changeType;
        private eGlobalUserType _globalUserType;
        // End Track #4872

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
		protected Controls.MIDComboBoxEnh cboSizeCurve;
		protected Controls.MIDComboBoxEnh cboConstraints;
		protected Controls.MIDComboBoxEnh cboAlternates;
		protected Infragistics.Win.UltraWinGrid.UltraGrid ugRules;
		protected System.Windows.Forms.CheckBox cbExpandAll;
		protected Controls.MIDComboBoxEnh cboHeaderChar;
		protected Controls.MIDComboBoxEnh cboHierarchyLevel;
		protected System.Windows.Forms.CheckBox cbColor;
		protected System.Windows.Forms.GroupBox gbGenericSizeCurve;
		protected System.Windows.Forms.GroupBox gbSizeCurve;
		protected System.Windows.Forms.GroupBox gbSizeConstraints;
		protected System.Windows.Forms.GroupBox gbGenericConstraint;
		protected Controls.MIDComboBoxEnh cboConstrHeaderChar;
		protected Controls.MIDComboBoxEnh cboConstrHierLevel;
		protected System.Windows.Forms.CheckBox cbConstrColor;
		protected System.Windows.Forms.PictureBox picBoxCurve;
		protected System.Windows.Forms.PictureBox picBoxConstraint;
		protected System.Windows.Forms.PictureBox picBoxGroup;
		protected System.Windows.Forms.PictureBox picBoxAlternate;
		protected System.Windows.Forms.GroupBox gbSizeGroup;
		protected System.Windows.Forms.GroupBox gbSizeAlternate;
		protected System.Windows.Forms.GroupBox gbxNormalizeSizeCurves;
		protected System.Windows.Forms.RadioButton radNormalizeSizeCurves_No;
		protected System.Windows.Forms.RadioButton radNormalizeSizeCurves_Yes;
		protected System.Windows.Forms.CheckBox cbxOverrideNormalizeDefault;
        protected CheckBox cbxUseDefaultcurve;
        protected Controls.MIDComboBoxEnh cboNameExtension;
        protected CheckBox cbxApplyRulesOnly;
        protected internal Controls.MIDComboBoxEnh cboInventoryBasis;
        protected internal Label lblInventoryBasis;

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
        protected FilterData storeFilterData
        {
            get
            {
                if (_storeFilterData == null)
                {
                    _storeFilterData = new FilterData();
                }

                return _storeFilterData;
            }
        }

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
		protected DataTable DtHierarchyLevel
		{
			get {return _dtCurveHierLevel;}
		}
		
		#endregion
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Constructors / Dispose
		public SizeMethodsFormBase()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			//			this.ugRules.Text = string.Empty;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}


		public SizeMethodsFormBase(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, eMIDTextCode aFormName, eWorkflowMethodType aWorkflowMethodType)  : base (aSAB, aEAB, aFormName, aWorkflowMethodType)
		{
			InitializeComponent();

			DisplayPictureBoxImages();
			SetPictureBoxTags();
			// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
			SetBaseText();
			// END MID Track #4826
            //Begin Track #5858 - KJohnson - Validating store security only
            // Begin TT#44 - JSmith - Drag/Drop User Attributes or Filters in to Global Methods does not react consistantly
            //cboFilter.Tag = new MIDStoreFilterComboBoxTag(SAB, cboFilter, eSecurityTypes.Store, eSecuritySelectType.View | eSecuritySelectType.Update);
            // End TT#44
            //End Track #5858 - KJohnson
		}

		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		/// <summary>
		/// Controls the text.
		/// </summary>
		private void SetBaseText()
		{
			try
			{
				this.gbxNormalizeSizeCurves.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_NormalizeSizeCurves);
				this.cbxOverrideNormalizeDefault.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideDefault);
				this.radNormalizeSizeCurves_Yes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
				this.radNormalizeSizeCurves_No.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
                // Begin TT#413 - JSmith - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings.
                this.cbxUseDefaultcurve.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_UseDefaultCurve);
                // Begin TT#413
                // Begin TT#2155 - JEllis - Fill Size Holes Null Reference
                this.cbxApplyRulesOnly.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ApplyRulesOnly);
                // end TT#2155 - JEllis - Fill Size Holes Null Reference
			}
			catch (Exception ex)
			{
				HandleException(ex, "SetText");
			}
		}
		// END MID Track #4826

		protected void DisplayPictureBoxImages()
		{
			DisplayPictureBoxImage(picBoxGroup);
			DisplayPictureBoxImage(picBoxAlternate);
			DisplayPictureBoxImage(picBoxCurve);
			DisplayPictureBoxImage(picBoxConstraint);
		}	
		
		protected void SetPictureBoxTags()
		{
			picBoxGroup.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask;
			picBoxAlternate .Tag= SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask;
			picBoxCurve.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask;
			picBoxConstraint.Tag = SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask;
		}	

		private void DisplayPictureBoxImage(System.Windows.Forms.PictureBox aPicBox)
		{
			Image image;
			try
			{
				image = Image.FromFile(MIDGraphics.ImageDir + "\\" + MIDGraphics.MagnifyingGlassImage);
				SizeF sizef = new SizeF(aPicBox.Width, aPicBox.Height);
				Size size = Size.Ceiling(sizef);
				Bitmap bitmap = new Bitmap(image, size);
				aPicBox.Image = bitmap;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
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
            this.cboSizeGroup = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblStoreAttribute = new System.Windows.Forms.Label();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblFilter = new System.Windows.Forms.Label();
            this.cboSizeCurve = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboAlternates = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboConstraints = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.ugRules = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.cbExpandAll = new System.Windows.Forms.CheckBox();
            this.cboHeaderChar = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboHierarchyLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbColor = new System.Windows.Forms.CheckBox();
            this.gbGenericSizeCurve = new System.Windows.Forms.GroupBox();
            this.cboNameExtension = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.gbSizeCurve = new System.Windows.Forms.GroupBox();
            this.cbxApplyRulesOnly = new System.Windows.Forms.CheckBox();
            this.cbxUseDefaultcurve = new System.Windows.Forms.CheckBox();
            this.picBoxCurve = new System.Windows.Forms.PictureBox();
            this.gbSizeConstraints = new System.Windows.Forms.GroupBox();
            this.picBoxConstraint = new System.Windows.Forms.PictureBox();
            this.gbGenericConstraint = new System.Windows.Forms.GroupBox();
            this.cboConstrHeaderChar = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboConstrHierLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cbConstrColor = new System.Windows.Forms.CheckBox();
            this.lblInventoryBasis = new System.Windows.Forms.Label();
            this.cboInventoryBasis = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.picBoxGroup = new System.Windows.Forms.PictureBox();
            this.picBoxAlternate = new System.Windows.Forms.PictureBox();
            this.gbSizeGroup = new System.Windows.Forms.GroupBox();
            this.gbSizeAlternate = new System.Windows.Forms.GroupBox();
            this.gbxNormalizeSizeCurves = new System.Windows.Forms.GroupBox();
            this.radNormalizeSizeCurves_No = new System.Windows.Forms.RadioButton();
            this.radNormalizeSizeCurves_Yes = new System.Windows.Forms.RadioButton();
            this.cbxOverrideNormalizeDefault = new System.Windows.Forms.CheckBox();
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).BeginInit();
            this.gbGenericSizeCurve.SuspendLayout();
            this.gbSizeCurve.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).BeginInit();
            this.gbSizeConstraints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).BeginInit();
            this.gbGenericConstraint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).BeginInit();
            this.gbSizeGroup.SuspendLayout();
            this.gbSizeAlternate.SuspendLayout();
            this.gbxNormalizeSizeCurves.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(773, 232);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(644, 240);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(644, 279);
            // 
            // pnlGlobalUser
            // 
            this.pnlGlobalUser.Size = new System.Drawing.Size(123, 28);
            // 
            // radUser
            // 
            this.radUser.Size = new System.Drawing.Size(48, 20);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // cboSizeGroup
            // 
            this.cboSizeGroup.AutoAdjust = false;
            this.cboSizeGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeGroup.DataSource = null;
            this.cboSizeGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeGroup.DropDownWidth = 216;
            this.cboSizeGroup.FormattingEnabled = false;
            this.cboSizeGroup.Location = new System.Drawing.Point(48, 20);
            this.cboSizeGroup.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeGroup.Name = "cboSizeGroup";
            this.cboSizeGroup.Size = new System.Drawing.Size(216, 21);
            this.cboSizeGroup.TabIndex = 4;
            this.cboSizeGroup.Tag = null;
            this.cboSizeGroup.SelectionChangeCommitted += new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
            this.cboSizeGroup.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSizeGroup_MIDComboBoxPropertiesChangedEvent);
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.AutoSize = true;
            this.lblStoreAttribute.Location = new System.Drawing.Point(300, 43);
            this.lblStoreAttribute.Name = "lblStoreAttribute";
            this.lblStoreAttribute.Size = new System.Drawing.Size(74, 13);
            this.lblStoreAttribute.TabIndex = 24;
            this.lblStoreAttribute.Text = "Store Attribute";
            this.lblStoreAttribute.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboStoreAttribute.Location = new System.Drawing.Point(382, 41);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(216, 21);
            this.cboStoreAttribute.TabIndex = 23;
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = false;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 216;
            this.cboFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFilter.FormattingEnabled = false;
            this.cboFilter.Location = new System.Drawing.Point(65, 42);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.Size = new System.Drawing.Size(216, 21);
            this.cboFilter.TabIndex = 25;
            this.cboFilter.Tag = null;
            //this.cboFilter.SelectedIndexChanged += new System.EventHandler(this.cboFilter_SelectedIndexChanged);
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(19, 39);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(29, 13);
            this.lblFilter.TabIndex = 26;
            this.lblFilter.Text = "Filter";
            this.lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboSizeCurve
            // 
            this.cboSizeCurve.AutoAdjust = false;
            this.cboSizeCurve.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSizeCurve.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSizeCurve.DataSource = null;
            this.cboSizeCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSizeCurve.DropDownWidth = 216;
            this.cboSizeCurve.FormattingEnabled = false;
            this.cboSizeCurve.Location = new System.Drawing.Point(48, 33);
            this.cboSizeCurve.Margin = new System.Windows.Forms.Padding(0);
            this.cboSizeCurve.Name = "cboSizeCurve";
            this.cboSizeCurve.Size = new System.Drawing.Size(216, 21);
            this.cboSizeCurve.TabIndex = 28;
            this.cboSizeCurve.Tag = null;
            this.cboSizeCurve.SelectionChangeCommitted += new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
            this.cboSizeCurve.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboSizeCurve_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboAlternates
            // 
            this.cboAlternates.AutoAdjust = false;
            this.cboAlternates.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboAlternates.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboAlternates.DataSource = null;
            this.cboAlternates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAlternates.DropDownWidth = 210;
            this.cboAlternates.FormattingEnabled = false;
            this.cboAlternates.Location = new System.Drawing.Point(54, 20);
            this.cboAlternates.Margin = new System.Windows.Forms.Padding(0);
            this.cboAlternates.Name = "cboAlternates";
            this.cboAlternates.Size = new System.Drawing.Size(210, 21);
            this.cboAlternates.TabIndex = 47;
            this.cboAlternates.Tag = null;
            this.cboAlternates.SelectionChangeCommitted += new System.EventHandler(this.cboAlternates_SelectionChangeCommitted);
            this.cboAlternates.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboAlternates_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboConstraints
            // 
            this.cboConstraints.AutoAdjust = false;
            this.cboConstraints.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboConstraints.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboConstraints.DataSource = null;
            this.cboConstraints.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConstraints.DropDownWidth = 210;
            this.cboConstraints.FormattingEnabled = false;
            this.cboConstraints.Location = new System.Drawing.Point(48, 57);
            this.cboConstraints.Margin = new System.Windows.Forms.Padding(0);
            this.cboConstraints.Name = "cboConstraints";
            this.cboConstraints.Size = new System.Drawing.Size(210, 21);
            this.cboConstraints.TabIndex = 44;
            this.cboConstraints.Tag = null;
            this.cboConstraints.SelectionChangeCommitted += new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
            this.cboConstraints.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboConstraints_MIDComboBoxPropertiesChangedEvent);
            // 
            // ugRules
            // 
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
            this.ugRules.Location = new System.Drawing.Point(16, 334);
            this.ugRules.Name = "ugRules";
            this.ugRules.Size = new System.Drawing.Size(307, 124);
            this.ugRules.TabIndex = 49;
            this.ugRules.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellUpdate);
            this.ugRules.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
            this.ugRules.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugRules_InitializeRow);
            this.ugRules.AfterRowsDeleted += new System.EventHandler(this.ugRules_AfterRowsDeleted);
            this.ugRules.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowInsert);
            this.ugRules.AfterRowUpdate += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowUpdate);
            this.ugRules.AfterRowActivate += new EventHandler(ugRules_AfterRowActivate);
            this.ugRules.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugRules_BeforeRowUpdate);
            this.ugRules.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellListCloseUp);
            this.ugRules.BeforeCellDeactivate += new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeCellDeactivate);
            this.ugRules.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeEnterEditMode);
            this.ugRules.SelectionDrag += new System.ComponentModel.CancelEventHandler(this.ugRules_SelectionDrag);
            this.ugRules.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugRules_BeforeRowInsert);
            this.ugRules.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugRules_MouseEnterElement);
            this.ugRules.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyDown);
            this.ugRules.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyUp);
            this.ugRules.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseDown);
            this.ugRules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseUp);
            // 
            // cbExpandAll
            // 
            this.cbExpandAll.Enabled = false;
            this.cbExpandAll.Location = new System.Drawing.Point(518, 330);
            this.cbExpandAll.Name = "cbExpandAll";
            this.cbExpandAll.Size = new System.Drawing.Size(104, 24);
            this.cbExpandAll.TabIndex = 50;
            this.cbExpandAll.Text = "Expand All";
            // 
            // cboHeaderChar
            // 
            this.cboHeaderChar.AutoAdjust = false;
            this.cboHeaderChar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHeaderChar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHeaderChar.DataSource = null;
            this.cboHeaderChar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHeaderChar.DropDownWidth = 216;
            this.cboHeaderChar.FormattingEnabled = false;
            this.cboHeaderChar.Location = new System.Drawing.Point(33, 17);
            this.cboHeaderChar.Margin = new System.Windows.Forms.Padding(0);
            this.cboHeaderChar.Name = "cboHeaderChar";
            this.cboHeaderChar.Size = new System.Drawing.Size(216, 21);
            this.cboHeaderChar.TabIndex = 52;
            this.cboHeaderChar.Tag = null;
            this.cboHeaderChar.SelectedIndexChanged += new System.EventHandler(this.cboHeaderChar_SelectedIndexChanged);
            this.cboHeaderChar.SelectionChangeCommitted += new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
            this.cboHeaderChar.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboHeaderChar_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboHierarchyLevel
            // 
            this.cboHierarchyLevel.AllowDrop = true;
            this.cboHierarchyLevel.AutoAdjust = false;
            this.cboHierarchyLevel.IgnoreFocusLost = true;
            this.cboHierarchyLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHierarchyLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHierarchyLevel.DataSource = null;
            this.cboHierarchyLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHierarchyLevel.DropDownWidth = 216;
            this.cboHierarchyLevel.FormattingEnabled = false;
            this.cboHierarchyLevel.Location = new System.Drawing.Point(33, 17);
            this.cboHierarchyLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboHierarchyLevel.Name = "cboHierarchyLevel";
            this.cboHierarchyLevel.Size = new System.Drawing.Size(216, 21);
            this.cboHierarchyLevel.TabIndex = 53;
            this.cboHierarchyLevel.Tag = null;
            this.cboHierarchyLevel.SelectedIndexChanged += new System.EventHandler(this.cboHierarchyLevel_SelectedIndexChanged);
            this.cboHierarchyLevel.SelectionChangeCommitted += new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
            this.cboHierarchyLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent);
            this.cboHierarchyLevel.DragDrop += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragDrop);
            this.cboHierarchyLevel.DragEnter += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
            this.cboHierarchyLevel.DragOver += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragOver);
            //this.cboHierarchyLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxHierLevel_KeyDown);
            this.cboHierarchyLevel.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxHierLevel_Validating);
            this.cboHierarchyLevel.Validated += new System.EventHandler(this.comboBoxHierLevel_Validated);
            // 
            // cbColor
            // 
            this.cbColor.Location = new System.Drawing.Point(33, 70);
            this.cbColor.Name = "cbColor";
            this.cbColor.Size = new System.Drawing.Size(104, 24);
            this.cbColor.TabIndex = 54;
            this.cbColor.Text = "Color";
            this.cbColor.CheckedChanged += new System.EventHandler(this.cbColor_CheckedChanged);
            // 
            // gbGenericSizeCurve
            // 
            this.gbGenericSizeCurve.Controls.Add(this.cboNameExtension);
            this.gbGenericSizeCurve.Controls.Add(this.cbColor);
            this.gbGenericSizeCurve.Controls.Add(this.cboHeaderChar);
            this.gbGenericSizeCurve.Controls.Add(this.cboHierarchyLevel);
            this.gbGenericSizeCurve.Location = new System.Drawing.Point(15, 62);
            this.gbGenericSizeCurve.Name = "gbGenericSizeCurve";
            this.gbGenericSizeCurve.Size = new System.Drawing.Size(268, 100);
            this.gbGenericSizeCurve.TabIndex = 55;
            this.gbGenericSizeCurve.TabStop = false;
            this.gbGenericSizeCurve.Text = "Generic";
            // 
            // cboNameExtension
            // 
            this.cboNameExtension.AutoAdjust = false;
            this.cboNameExtension.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboNameExtension.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboNameExtension.DataSource = null;
            this.cboNameExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNameExtension.DropDownWidth = 216;
            this.cboNameExtension.FormattingEnabled = true;
            this.cboNameExtension.Location = new System.Drawing.Point(33, 47);
            this.cboNameExtension.Margin = new System.Windows.Forms.Padding(0);
            this.cboNameExtension.Name = "cboNameExtension";
            this.cboNameExtension.Size = new System.Drawing.Size(216, 21);
            this.cboNameExtension.TabIndex = 55;
            this.cboNameExtension.Tag = null;
            this.cboNameExtension.SelectedIndexChanged += new System.EventHandler(this.cboNameExtension_SelectedIndexChanged);
            this.cboNameExtension.SelectionChangeCommitted += new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
            this.cboNameExtension.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboNameExtension_MIDComboBoxPropertiesChangedEvent);
            // 
            // gbSizeCurve
            // 
            this.gbSizeCurve.Controls.Add(this.cboSizeCurve);
            this.gbSizeCurve.Controls.Add(this.cbxApplyRulesOnly);
            this.gbSizeCurve.Controls.Add(this.cbxUseDefaultcurve);
            this.gbSizeCurve.Controls.Add(this.picBoxCurve);
            this.gbSizeCurve.Controls.Add(this.gbGenericSizeCurve);
            this.gbSizeCurve.Location = new System.Drawing.Point(17, 140);
            this.gbSizeCurve.Name = "gbSizeCurve";
            this.gbSizeCurve.Size = new System.Drawing.Size(294, 180);
            this.gbSizeCurve.TabIndex = 0;
            this.gbSizeCurve.TabStop = false;
            this.gbSizeCurve.Text = "Size Curve";
            // 
            // cbxApplyRulesOnly
            // 
            this.cbxApplyRulesOnly.AutoSize = true;
            this.cbxApplyRulesOnly.Location = new System.Drawing.Point(127, 14);
            this.cbxApplyRulesOnly.Name = "cbxApplyRulesOnly";
            this.cbxApplyRulesOnly.Size = new System.Drawing.Size(109, 17);
            this.cbxApplyRulesOnly.TabIndex = 57;
            this.cbxApplyRulesOnly.Text = ":Apply Rules Only";
            this.cbxApplyRulesOnly.UseVisualStyleBackColor = true;
            // 
            // cbxUseDefaultcurve
            // 
            this.cbxUseDefaultcurve.AutoSize = true;
            this.cbxUseDefaultcurve.Location = new System.Drawing.Point(27, 14);
            this.cbxUseDefaultcurve.Name = "cbxUseDefaultcurve";
            this.cbxUseDefaultcurve.Size = new System.Drawing.Size(82, 17);
            this.cbxUseDefaultcurve.TabIndex = 56;
            this.cbxUseDefaultcurve.Text = "Use Default";
            this.cbxUseDefaultcurve.UseVisualStyleBackColor = true;
            this.cbxUseDefaultcurve.CheckedChanged += new System.EventHandler(this.cbxUseDefaultcurve_CheckedChanged);
            // 
            // picBoxCurve
            // 
            this.picBoxCurve.Location = new System.Drawing.Point(19, 33);
            this.picBoxCurve.Name = "picBoxCurve";
            this.picBoxCurve.Size = new System.Drawing.Size(20, 20);
            this.picBoxCurve.TabIndex = 0;
            this.picBoxCurve.TabStop = false;
            this.picBoxCurve.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxCurve.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // gbSizeConstraints
            // 
            this.gbSizeConstraints.AutoSize = true;
            this.gbSizeConstraints.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gbSizeConstraints.Controls.Add(this.picBoxConstraint);
            this.gbSizeConstraints.Controls.Add(this.gbGenericConstraint);
            this.gbSizeConstraints.Controls.Add(this.cboConstraints);
            this.gbSizeConstraints.Controls.Add(this.lblInventoryBasis);
            this.gbSizeConstraints.Controls.Add(this.cboInventoryBasis);
            this.gbSizeConstraints.Location = new System.Drawing.Point(328, 140);
            this.gbSizeConstraints.Name = "gbSizeConstraints";
            this.gbSizeConstraints.Size = new System.Drawing.Size(289, 204);
            this.gbSizeConstraints.TabIndex = 52;
            this.gbSizeConstraints.TabStop = false;
            this.gbSizeConstraints.Text = "Size Constraints Model";
            // 
            // picBoxConstraint
            // 
            this.picBoxConstraint.Location = new System.Drawing.Point(19, 58);
            this.picBoxConstraint.Name = "picBoxConstraint";
            this.picBoxConstraint.Size = new System.Drawing.Size(20, 20);
            this.picBoxConstraint.TabIndex = 57;
            this.picBoxConstraint.TabStop = false;
            this.picBoxConstraint.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxConstraint.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // gbGenericConstraint
            // 
            this.gbGenericConstraint.Controls.Add(this.cboConstrHeaderChar);
            this.gbGenericConstraint.Controls.Add(this.cboConstrHierLevel);
            this.gbGenericConstraint.Controls.Add(this.cbConstrColor);
            this.gbGenericConstraint.Location = new System.Drawing.Point(15, 85);
            this.gbGenericConstraint.Name = "gbGenericConstraint";
            this.gbGenericConstraint.Size = new System.Drawing.Size(268, 100);
            this.gbGenericConstraint.TabIndex = 56;
            this.gbGenericConstraint.TabStop = false;
            this.gbGenericConstraint.Text = "Generic ";
            // 
            // cboConstrHeaderChar
            // 
            this.cboConstrHeaderChar.AutoAdjust = false;
            this.cboConstrHeaderChar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboConstrHeaderChar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboConstrHeaderChar.DataSource = null;
            this.cboConstrHeaderChar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConstrHeaderChar.DropDownWidth = 210;
            this.cboConstrHeaderChar.FormattingEnabled = false;
            this.cboConstrHeaderChar.Location = new System.Drawing.Point(33, 17);
            this.cboConstrHeaderChar.Margin = new System.Windows.Forms.Padding(0);
            this.cboConstrHeaderChar.Name = "cboConstrHeaderChar";
            this.cboConstrHeaderChar.Size = new System.Drawing.Size(210, 21);
            this.cboConstrHeaderChar.TabIndex = 52;
            this.cboConstrHeaderChar.Tag = null;
            this.cboConstrHeaderChar.SelectedIndexChanged += new System.EventHandler(this.cboHeaderChar_SelectedIndexChanged);
            this.cboConstrHeaderChar.SelectionChangeCommitted += new System.EventHandler(this.cboConstrHeaderChar_SelectionChangeCommitted);
            this.cboConstrHeaderChar.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboConstrHeaderChar_MIDComboBoxPropertiesChangedEvent);
            // 
            // cboConstrHierLevel
            // 
            this.cboConstrHierLevel.AllowDrop = true;
            this.cboConstrHierLevel.AutoAdjust = false;
            this.cboConstrHierLevel.IgnoreFocusLost = true;
            this.cboConstrHierLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboConstrHierLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboConstrHierLevel.DataSource = null;
            this.cboConstrHierLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConstrHierLevel.DropDownWidth = 210;
            this.cboConstrHierLevel.FormattingEnabled = false;
            this.cboConstrHierLevel.Location = new System.Drawing.Point(33, 47);
            this.cboConstrHierLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboConstrHierLevel.Name = "cboConstrHierLevel";
            this.cboConstrHierLevel.Size = new System.Drawing.Size(210, 21);
            this.cboConstrHierLevel.TabIndex = 53;
            this.cboConstrHierLevel.Tag = null;
            this.cboConstrHierLevel.SelectedIndexChanged += new System.EventHandler(this.cboHierarchyLevel_SelectedIndexChanged);
            this.cboConstrHierLevel.SelectionChangeCommitted += new System.EventHandler(this.cboConstrHierLevel_SelectionChangeCommitted);
            this.cboConstrHierLevel.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboConstrHierLevel_MIDComboBoxPropertiesChangedEvent);
            this.cboConstrHierLevel.DragDrop += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragDrop);
            this.cboConstrHierLevel.DragEnter += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
            this.cboConstrHierLevel.DragOver += new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragOver);
            //this.cboConstrHierLevel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxHierLevel_KeyDown);
            this.cboConstrHierLevel.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxHierLevel_Validating);
            this.cboConstrHierLevel.Validated += new System.EventHandler(this.comboBoxHierLevel_Validated);
            // 
            // cbConstrColor
            // 
            this.cbConstrColor.Location = new System.Drawing.Point(33, 70);
            this.cbConstrColor.Name = "cbConstrColor";
            this.cbConstrColor.Size = new System.Drawing.Size(104, 24);
            this.cbConstrColor.TabIndex = 54;
            this.cbConstrColor.Text = "Color";
            this.cbConstrColor.CheckedChanged += new System.EventHandler(this.cbColor_CheckedChanged);
            // 
            // lblInventoryBasis
            // 
            this.lblInventoryBasis.AutoSize = true;
            this.lblInventoryBasis.Location = new System.Drawing.Point(11, 28);
            this.lblInventoryBasis.Name = "lblInventoryBasis";
            this.lblInventoryBasis.Size = new System.Drawing.Size(82, 13);
            this.lblInventoryBasis.TabIndex = 58;
            this.lblInventoryBasis.Text = "Inventory Basis:";
            // 
            // cboInventoryBasis
            // 
            this.cboInventoryBasis.AllowDrop = true;
            this.cboInventoryBasis.AutoAdjust = false;
            this.cboInventoryBasis.IgnoreFocusLost = true;
            this.cboInventoryBasis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.cboInventoryBasis.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboInventoryBasis.DataSource = null;
            this.cboInventoryBasis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboInventoryBasis.DropDownWidth = 184;
            this.cboInventoryBasis.FormattingEnabled = true;
            this.cboInventoryBasis.Location = new System.Drawing.Point(97, 24);
            this.cboInventoryBasis.Margin = new System.Windows.Forms.Padding(0);
            this.cboInventoryBasis.Name = "cboInventoryBasis";
            this.cboInventoryBasis.Size = new System.Drawing.Size(184, 21);
            this.cboInventoryBasis.TabIndex = 59;
            this.cboInventoryBasis.Tag = null;
            this.cboInventoryBasis.SelectionChangeCommitted += new System.EventHandler(this.cboInventoryBasis_SelectionChangeCommitted);
            this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);
            this.cboInventoryBasis.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
            this.cboInventoryBasis.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
            this.cboInventoryBasis.DragOver += new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
            this.cboInventoryBasis.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
            this.cboInventoryBasis.Validating += new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
            this.cboInventoryBasis.Validated += new System.EventHandler(this.cboInventoryBasis_Validated);
            // 
            // picBoxGroup
            // 
            this.picBoxGroup.Location = new System.Drawing.Point(19, 20);
            this.picBoxGroup.Name = "picBoxGroup";
            this.picBoxGroup.Size = new System.Drawing.Size(19, 20);
            this.picBoxGroup.TabIndex = 53;
            this.picBoxGroup.TabStop = false;
            this.picBoxGroup.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxGroup.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // picBoxAlternate
            // 
            this.picBoxAlternate.Location = new System.Drawing.Point(19, 20);
            this.picBoxAlternate.Name = "picBoxAlternate";
            this.picBoxAlternate.Size = new System.Drawing.Size(20, 20);
            this.picBoxAlternate.TabIndex = 54;
            this.picBoxAlternate.TabStop = false;
            this.picBoxAlternate.Click += new System.EventHandler(this.picBoxFilter_Click);
            this.picBoxAlternate.MouseHover += new System.EventHandler(this.picBoxFilter_MouseHover);
            // 
            // gbSizeGroup
            // 
            this.gbSizeGroup.Controls.Add(this.picBoxGroup);
            this.gbSizeGroup.Controls.Add(this.cboSizeGroup);
            this.gbSizeGroup.Location = new System.Drawing.Point(17, 73);
            this.gbSizeGroup.Name = "gbSizeGroup";
            this.gbSizeGroup.Size = new System.Drawing.Size(294, 56);
            this.gbSizeGroup.TabIndex = 55;
            this.gbSizeGroup.TabStop = false;
            this.gbSizeGroup.Text = "Size Group";
            // 
            // gbSizeAlternate
            // 
            this.gbSizeAlternate.Controls.Add(this.cboAlternates);
            this.gbSizeAlternate.Controls.Add(this.picBoxAlternate);
            this.gbSizeAlternate.Location = new System.Drawing.Point(328, 73);
            this.gbSizeAlternate.Name = "gbSizeAlternate";
            this.gbSizeAlternate.Size = new System.Drawing.Size(294, 56);
            this.gbSizeAlternate.TabIndex = 56;
            this.gbSizeAlternate.TabStop = false;
            this.gbSizeAlternate.Text = "Size Alternates Model";
            // 
            // gbxNormalizeSizeCurves
            // 
            this.gbxNormalizeSizeCurves.Controls.Add(this.radNormalizeSizeCurves_No);
            this.gbxNormalizeSizeCurves.Controls.Add(this.radNormalizeSizeCurves_Yes);
            this.gbxNormalizeSizeCurves.Controls.Add(this.cbxOverrideNormalizeDefault);
            this.gbxNormalizeSizeCurves.Location = new System.Drawing.Point(371, 360);
            this.gbxNormalizeSizeCurves.Name = "gbxNormalizeSizeCurves";
            this.gbxNormalizeSizeCurves.Size = new System.Drawing.Size(221, 42);
            this.gbxNormalizeSizeCurves.TabIndex = 59;
            this.gbxNormalizeSizeCurves.TabStop = false;
            this.gbxNormalizeSizeCurves.Text = "Normalize Size Curves";
            // 
            // radNormalizeSizeCurves_No
            // 
            this.radNormalizeSizeCurves_No.Location = new System.Drawing.Point(167, 16);
            this.radNormalizeSizeCurves_No.Name = "radNormalizeSizeCurves_No";
            this.radNormalizeSizeCurves_No.Size = new System.Drawing.Size(48, 24);
            this.radNormalizeSizeCurves_No.TabIndex = 25;
            this.radNormalizeSizeCurves_No.Text = "No";
            this.radNormalizeSizeCurves_No.CheckedChanged += new System.EventHandler(this.radNormalizeSizeCurves_No_CheckedChanged);
            // 
            // radNormalizeSizeCurves_Yes
            // 
            this.radNormalizeSizeCurves_Yes.Location = new System.Drawing.Point(116, 16);
            this.radNormalizeSizeCurves_Yes.Name = "radNormalizeSizeCurves_Yes";
            this.radNormalizeSizeCurves_Yes.Size = new System.Drawing.Size(47, 24);
            this.radNormalizeSizeCurves_Yes.TabIndex = 24;
            this.radNormalizeSizeCurves_Yes.Text = "Yes";
            this.radNormalizeSizeCurves_Yes.CheckedChanged += new System.EventHandler(this.radNormalizeSizeCurves_Yes_CheckedChanged);
            // 
            // cbxOverrideNormalizeDefault
            // 
            this.cbxOverrideNormalizeDefault.Location = new System.Drawing.Point(10, 16);
            this.cbxOverrideNormalizeDefault.Name = "cbxOverrideNormalizeDefault";
            this.cbxOverrideNormalizeDefault.Size = new System.Drawing.Size(111, 24);
            this.cbxOverrideNormalizeDefault.TabIndex = 0;
            this.cbxOverrideNormalizeDefault.Text = "Override Default";
            this.cbxOverrideNormalizeDefault.CheckedChanged += new System.EventHandler(this.cbxOverrideNormalizeDefault_CheckedChanged);
            // 
            // SizeMethodsFormBase
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(759, 469);
            this.Controls.Add(this.cbExpandAll);
            this.Controls.Add(this.gbSizeAlternate);
            this.Controls.Add(this.gbSizeGroup);
            this.Controls.Add(this.gbSizeConstraints);
            this.Controls.Add(this.gbSizeCurve);
            this.Controls.Add(this.lblStoreAttribute);
            this.Controls.Add(this.lblFilter);
            this.Controls.Add(this.gbxNormalizeSizeCurves);
            this.Controls.Add(this.ugRules);
            this.Controls.Add(this.cboStoreAttribute);
            this.Controls.Add(this.cboFilter);
            this.Name = "SizeMethodsFormBase";
            this.Text = "SizeMethodsFormBase";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.SizeMethodsFormBase_Closing);
            this.Controls.SetChildIndex(this.cboFilter, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.ugRules, 0);
            this.Controls.SetChildIndex(this.gbxNormalizeSizeCurves, 0);
            this.Controls.SetChildIndex(this.lblFilter, 0);
            this.Controls.SetChildIndex(this.lblStoreAttribute, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.gbSizeCurve, 0);
            this.Controls.SetChildIndex(this.gbSizeConstraints, 0);
            this.Controls.SetChildIndex(this.gbSizeGroup, 0);
            this.Controls.SetChildIndex(this.gbSizeAlternate, 0);
            this.Controls.SetChildIndex(this.cbExpandAll, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugRules)).EndInit();
            this.gbGenericSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.ResumeLayout(false);
            this.gbSizeCurve.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCurve)).EndInit();
            this.gbSizeConstraints.ResumeLayout(false);
            this.gbSizeConstraints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxConstraint)).EndInit();
            this.gbGenericConstraint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAlternate)).EndInit();
            this.gbSizeGroup.ResumeLayout(false);
            this.gbSizeAlternate.ResumeLayout(false);
            this.gbxNormalizeSizeCurves.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}


	#endregion

	#region Value Lists
		/// <summary>
		/// Sets up value lists that will be used on ugRules.  This method should be overridden.
		/// </summary>
		/// <param name="MyGrid">UltraGrid object</param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void InitializeValueLists(UltraGrid MyGrid)
		{
			throw new Exception("Can not call base method InitializeValueLists(UltraGrid MyGrid).");
		}


		/// <summary>
		/// Fills a UltraGrid ValueList with sizes based on a selected Size Group
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="SizeGroupRID">Size Group ID to find sizes</param>
		virtual protected void FillSizesList(ValueList valueList, int RID, eGetSizes getSizes)
		{
			valueList.ValueListItems.Clear();
			
			// Begin Issue # 3685 - stodd 2/7/2006
			// Needed sizes to be in correct sequence
			//=========================
			// Fills _dtSizes
			//=========================
			GetSizes(RID, getSizes);
			// End Issue # 3685 - stodd 2/7/2006
			ArrayList aSizeCodeList = new ArrayList();

			//valueList.ValueListItems.Add(-1, " ");
			foreach (DataRow dr in _dtSizes.Rows)
			{
				string aSize = dr["SIZE_CODE_PRIMARY"].ToString();
				int sSizeKey = Convert.ToInt32(dr["SIZE_CODE_RID"]);

				aSizeCodeList.Add(aSize);
				valueList.ValueListItems.Add(sSizeKey, aSize);
			}
		}

		/// <summary>
		/// fills _dtSizes table
		/// </summary>
		/// <param name="RID"></param>
		/// <param name="getSizes"></param>
		/// <returns></returns>
		private void GetSizes(int RID, eGetSizes getSizes)
		{

			AddColumnsToSizesDataTable();

			if (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeGroupProfile sgp = new SizeGroupProfile(RID);
				foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = _dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					_dtSizes.Rows.Add(newRow);
				}

			}
			else  // (getSizes == eGetSizes.SizeGroupRID)
			{
				SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(RID);
				foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
				{
					DataRow newRow = _dtSizes.NewRow();
					newRow["SIZES_RID"] = scp.SizeCodePrimaryRID;
					newRow["SIZE_CODE_PRIMARY"] = scp.SizeCodePrimary;
					newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
					newRow["SIZE_CODE_RID"] = scp.Key;
					newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
					_dtSizes.Rows.Add(newRow);
				}
			}
		}


		private void AddColumnsToSizesDataTable()
		{
			_dtSizes = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZES_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "SIZE_CODE_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "DIMENSIONS_RID";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_PRIMARY";
			_dtSizes.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_SECONDARY";
			_dtSizes.Columns.Add(dataColumn);
		}

		/// <summary>
		/// Fills a UltraGrid ValueList with size dimensions.
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="SizeGroupRID">Size Group to find dimensions</param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void FillSizeDimensionList(ValueList valueList, int RID, eGetDimensions getDimensions)
		{
			valueList.ValueListItems.Clear();
			// Begin Issue # 3685 - stodd 2/7/2006
			// Needed sizes to be in correct sequence
			AddColumnsToDimensionDataTable();

			if (getDimensions == eGetDimensions.SizeGroupRID)
			{
				SizeGroupProfile sgp = new SizeGroupProfile(RID);
				foreach (SizeCodeProfile scp in sgp.SizeCodeList.ArrayList)
				{
					DataRow [] hits = _dtDimensions.Select("DIMENSIONS_RID = " + scp.SizeCodeSecondaryRID.ToString(CultureInfo.CurrentUICulture));
					if (hits.Length == 0)
					{
						DataRow newRow = _dtDimensions.NewRow();
						newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
						newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
						_dtDimensions.Rows.Add(newRow);
					}
				}

			}
			else
			{
				SizeCurveGroupProfile scgp = new SizeCurveGroupProfile(RID);
				foreach (SizeCodeProfile scp in scgp.SizeCodeList.ArrayList)
				{
					DataRow [] hits = _dtDimensions.Select("DIMENSIONS_RID = " + scp.SizeCodeSecondaryRID.ToString(CultureInfo.CurrentUICulture));
					if (hits.Length == 0)
					{
						DataRow newRow = _dtDimensions.NewRow();
						newRow["DIMENSIONS_RID"] = scp.SizeCodeSecondaryRID;
						newRow["SIZE_CODE_SECONDARY"] = scp.SizeCodeSecondary;
						_dtDimensions.Rows.Add(newRow);
					}
				}
			}
			// End Issue # 3685 - stodd 2/7/2006

			//valueList.ValueListItems.Add(-1, " ");
			foreach (DataRow dr in _dtDimensions.Rows)
			{
				valueList.ValueListItems.Add(Convert.ToInt32(dr["DIMENSIONS_RID"]), 
					dr["SIZE_CODE_SECONDARY"].ToString());
			}
		}

		private void AddColumnsToDimensionDataTable()
		{
			_dtDimensions = MIDEnvironment.CreateDataTable();

			DataColumn dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.Int32");
			dataColumn.ColumnName = "DIMENSIONS_RID";
			_dtDimensions.Columns.Add(dataColumn);

			dataColumn = new DataColumn();
			dataColumn.DataType = System.Type.GetType("System.String");
			dataColumn.ColumnName = "SIZE_CODE_SECONDARY";
			_dtDimensions.Columns.Add(dataColumn);
		}

		virtual protected void FillBothSizeDimensionList(ValueList valueList, int RID, eGetSizes getSizes, eGetDimensions getDimensions)
		{
			valueList.ValueListItems.Clear();
			//=========================
			// Fills _dtSizes
			//=========================
			GetSizes(RID, getSizes);

			string sizeName;
			//valueList.ValueListItems.Add(-1, " ");

			// BEGIN MID Track #3942  'None' is now another name for NoSecondarySize 
			string noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
			// END MID Track #3942

			foreach (DataRow dr in _dtSizes.Rows)
			{
				string secondarySize = dr["SIZE_CODE_SECONDARY"].ToString().Trim();
				// No Secondary Size // BEGIN MID Track #3942 - add noSizeDimensionLbl
				if (secondarySize == Include.NoSecondarySize || secondarySize == string.Empty || secondarySize == noSizeDimensionLbl)
					sizeName = dr["SIZE_CODE_PRIMARY"].ToString();
				else
					sizeName = dr["SIZE_CODE_PRIMARY"].ToString() + " / " + dr["SIZE_CODE_SECONDARY"].ToString();
				valueList.ValueListItems.Add(Convert.ToInt32(dr["SIZE_CODE_RID"]), sizeName);
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

//		/// <summary>
//		/// Fills a supplied UltraGrid ValueList with data from APPLICATION_TEXT table.
//		/// </summary>
//		/// <param name="valueList">ValueList object to fill</param>
//		/// <param name="textType">Enum value from eMIDTextType</param>
//		/// <param name="orderBy">Enum value from eMIDTextOrderBy </param>
//		virtual protected void FillTextTypeList(ValueList valueList, eMIDTextType textType, eMIDTextOrderBy orderBy)
//		{
//			foreach (DataRow drRule in MIDText.GetTextType(textType, orderBy).Rows)
//			{
//				valueList.ValueListItems.Add(Convert.ToInt32(drRule["TEXT_CODE"],
//					CultureInfo.CurrentUICulture), drRule["TEXT_VALUE"].ToString());
//			}
//		}

		/// <summary>
		/// Fills a supplied UltraGrid ValueList with data from APPLICATION_TEXT table.
		/// </summary>
		/// <param name="valueList">ValueList object to fill</param>
		/// <param name="withQuantity">Include rule quantity column</param>         // MID track 3619 Remove Fringe
		virtual protected void FillRulesList(ValueList valueList, bool withQuantity) // MID Track 3619 Remove Fringe
		{
			valueList.ValueListItems.Add(-1, " ");

			if (withQuantity)   // MID Track 3619 Remove Fringe
			{
				DataTable dtRules = MIDText.GetLabels((int) eSizeRuleType.Exclude,(int) eSizeRuleType.AbsoluteQuantity); // MID Track 3619 Remove Fringe
				foreach (DataRow row in dtRules.Rows)
				{
					int textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
					valueList.ValueListItems.Add(textCode, row["TEXT_VALUE"].ToString());
				}

				dtRules = MIDText.GetLabels((int) eSizeRuleType.SizeMinimum,(int) eSizeRuleType.SizeMaximum); // MID Track 3619 Remove Fringe
				foreach (DataRow row in dtRules.Rows)
				{
					int textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
					valueList.ValueListItems.Add(textCode, row["TEXT_VALUE"].ToString());
				}

			}
			else
			{
				DataTable dtRules = MIDText.GetLabels((int) eSizeRuleType.Exclude,(int) eSizeRuleType.Exclude); // MID Track 3619 Remove Fringe
				foreach (DataRow row in dtRules.Rows)
				{
					int textCode = Convert.ToInt32(row["TEXT_CODE"], CultureInfo.CurrentUICulture);
					valueList.ValueListItems.Add(textCode, row["TEXT_VALUE"].ToString());
				}

			}

//			foreach (DataRow drRule in MIDText.GetTextType(eMIDTextType.eSizeFringeRuleType, eMIDTextOrderBy.TextValue).Rows)
//			{
//				int textCode = Convert.ToInt32(drRule["TEXT_CODE"],	CultureInfo.CurrentUICulture);
//				if (withFringe)
//				{
//					valueList.ValueListItems.Add(textCode, drRule["TEXT_VALUE"].ToString());
//				}
//				else
//				{
//					if (textCode == (int)eSizeFringeRuleType.Exclude)
//					{
//						valueList.ValueListItems.Add(textCode, drRule["TEXT_VALUE"].ToString());
//						break;
//					}
//				}
//			}
		}

	#endregion 

	#region Context Menu Event Handlers

		/// <summary>
		/// Serves two functions:
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
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
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
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
		/// 1) Returns boolean to the ugAllSize_AfterRowInsert event which determines if a new size should be allowed.
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
			throw new Exception("Can not call base method IsGridValid().");
		}


		/// <summary>
		/// Validation for rows within All Sizes Grid.  Each row will broken down to individual cells, 
		/// these cells will then be validated.
		/// </summary>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		/// <remarks>Method must be overridden</remarks>
		virtual protected bool IsActiveRowValid(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method IsActiveRowValid(UltraGridRow activeRow).");
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
					switch (base.ABM.MethodType)
					{
						case eMethodType.FillSizeHolesAllocation:
						switch ((eFillSizeHolesRuleType)Convert.ToInt32(thisRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture))
						{
							case eFillSizeHolesRuleType.SizeFillUpTo:
								if (activeCell.Text.Trim() == string.Empty)
								{
									IsValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FillUpToRuleQuantityRequired));
								}
								break;
							default:
								if (activeCell.Text.Trim() != string.Empty)
								{
									IsValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidFillUpToRuleQuantity));
								}
								break;
						}
							break;
						case eMethodType.BasisSizeAllocation:
						switch ((eBasisSizeMethodRuleType)Convert.ToInt32(thisRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture))
						{
							case eBasisSizeMethodRuleType.AbsoluteQuantity:
								if (activeCell.Text.Trim() == string.Empty)
								{
									IsValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QuantityRuleQuantityRequired));
								}
								break;
							default:
								if (activeCell.Text.Trim() != string.Empty)
								{
									IsValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidQuantityRuleQuantity));
								}
								break;
						}
							break;
					}
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
		/// Validates the size rule cell vs. the size quantity cell
		/// </summary>
		/// <param name="thisRow"></param>
		/// <returns>Boolean value True=Valid|False=InValid</returns>
		virtual protected bool IsSizeRuleValid(UltraGridRow thisRow)
		{
			bool isValid = true;
						
			ErrorMessages.Clear();
			UltraGridCell activeCell = thisRow.Cells["SIZE_QUANTITY"];

			activeCell.Appearance.Image = null;
			activeCell.Tag = null;

			if (thisRow.Cells["SIZE_RULE"].Value != DBNull.Value)
			{
				eSizeRuleType ruleType = ruleType = (eSizeRuleType)Convert.ToInt32(thisRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture); // MID Track 3619 Remove Fringe
				// MID Track 3813 Issue message when rules applied without constraint
				if (this.cboConstrHeaderChar.Text.Trim() == _noHeaderCharSelectedLabel && // MID Track 4372 Generic Size Constraint
					this.cboConstrHierLevel.Text.Trim() == _noHierLevelSelectedLabel &&  // MID Track 4372 Generic Size Constraint
					!this.cbConstrColor.Checked &&                           // MID Track 4372 Generic Size Constraint
					this.cboConstraints.Text.Trim() == string.Empty)
				{
					if (ruleType == eSizeRuleType.SizeMaximum
						|| ruleType == eSizeRuleType.SizeMinimum
						|| ruleType == eSizeRuleType.SizeMinimumPlus1)
					{
						isValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_ConstraintModelRequiredForMinMaxRules));
					}
				}
				// end MID Track 3813 Issue message when rules applied without constraint
				if (activeCell.Value != DBNull.Value)
				{
					if (!ValidateCellsNumeric(activeCell, typeof(int)))
					{
						isValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeInteger));
					}
					else
					{
						if (activeCell.Text.Trim() != string.Empty)
						{
							if (Convert.ToInt32(activeCell.Text.Trim()) < 0)
							{
								isValid = false;
								ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MustBeNonNegative));
							}
							else
							{
								if (ruleType != eSizeRuleType.AbsoluteQuantity) // MID Track 3619 Remove Fringe
								{
									isValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidQuantityRuleQuantity));
								}
							}
						}	
						else
						{
							if (ruleType == eSizeRuleType.AbsoluteQuantity) // MID Track 3619 Remove Fringe
							{
								if (activeCell.Text.Trim() != string.Empty)
								{
									isValid = false;
									ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QuantityRuleQuantityRequired));
								}
							}
						}
					}		
				}
				else
				{
					if (ruleType == eSizeRuleType.AbsoluteQuantity) // MID Track 3619 Remove Fringe
					{
						isValid = false;
						ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_QuantityRuleQuantityRequired));
					}
				}
			}
			else
			{
				if (activeCell.Text.Trim() != string.Empty)
				{
					isValid = false;
					ErrorMessages.Add(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidRuleQuantity));
				}
			}
				
			//ADD THE MESSAGES TO THE CELL TAG.
			//==========================================
			if (!isValid)
			{
				AttachErrors(activeCell);
			}
			//==========================================

			return isValid;
			
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




		virtual protected void PositionColumns()
		{
			try
			{
				UltraGridColumn column;

				#region SET BAND

				if (ugRules.DisplayLayout.Bands.Exists(C_SET))
				{
					column = ugRules.DisplayLayout.Bands[C_SET].Columns["BAND_DSC"];
					column.Header.VisiblePosition = 0;
					column.Header.Caption = "Store Sets";
					column.Width = 200;

					

					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"];
					column.Header.VisiblePosition = 4;
					column.Header.Caption = "Rule";

					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"];
					column.Header.VisiblePosition = 5;
					column.Header.Caption = "Qty";

					


					ugRules.DisplayLayout.Bands[C_SET].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET].Override.AllowDelete = DefaultableBoolean.False;
					ugRules.DisplayLayout.Bands[C_SET].AddButtonCaption = "Set";
				}
				#endregion


				#region ALL COLOR BAND

				if (ugRules.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["BAND_DSC"].Header.VisiblePosition = 0;
					
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Override.AllowDelete = DefaultableBoolean.False;
					ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].AddButtonCaption = "All Color";
				}
				#endregion


				#region COLOR BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"].Header.VisiblePosition = 0;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_SET_CLR].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_SET_CLR].AddButtonCaption = "Color";
				}
				#endregion


				#region COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_CLR_SZ].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_CLR_SZ].AddButtonCaption = "Size";
				}
				#endregion


				#region ALL COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Header.VisiblePosition = 0;
					
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["DIMENSIONS_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].AddButtonCaption = "Size";
				}
				#endregion


				#region ALL COLOR DIMENSION BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;
					
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";
					
				}
				#endregion


				#region COLOR DIMENSION BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Header.VisiblePosition = 0;
					
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"];
					column.SortIndicator = SortIndicator.Ascending;

					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"].Header.VisiblePosition = 1;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Header.VisiblePosition = 2;

					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["METHOD_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZES_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SGL_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["ROW_TYPE_ID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["COLOR_CODE_RID"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["BAND_DSC"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_QUANTITY"].Hidden = true;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_SEQ"].Hidden = true;

					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].ColHeadersVisible = false;
					ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].AddButtonCaption = "Size Dimension";
				}
				#endregion
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.PositionColumns");
			}
		}

		/// <summary>
		/// Sets the initial layout of ugRules
		/// </summary>
		virtual protected void DefaultGridLayout() 
		{
			try
			{
				//Create any context menus that may be used on the grid.
				BuildContextMenus();

				//Set cancel update action
				ugRules.RowUpdateCancelAction = RowUpdateCancelAction.RetainDataAndActivation;

				//Create the Value List Collections
				ugRules.DisplayLayout.ValueLists.Clear();
				InitializeValueLists(ugRules);

				ugRules.DisplayLayout.AddNewBox.Hidden = false;
				ugRules.DisplayLayout.Override.SelectTypeCell = SelectType.ExtendedAutoDrag;

				PositionColumns();
				CustomizeColumns();

			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.DefaultGridLayout");
			}	
		    
		}

		virtual protected void CustomizeColumns()
		{
			try
			{
				UltraGridColumn column;

				#region SET BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_SET))
				{
//					column = ugRules.DisplayLayout.Bands[C_SET].Columns["FILL_SEQUENCE"];
//					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
//					column.ValueList = ugRules.DisplayLayout.ValueLists["SortOrder"];
//					column.Width = 150;

					column = ugRules.DisplayLayout.Bands[C_SET].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
					column.Width = 150;
				}
				#endregion

				#region ALL COLOR BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_ALL_CLR))
				{
					column = ugRules.DisplayLayout.Bands[C_SET_ALL_CLR].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		// BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	// END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR
				if (ugRules.DisplayLayout.Bands.Exists(C_SET_CLR))
				{
					column = ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["COLOR_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugRules.DisplayLayout.ValueLists["Colors"];

					column = ugRules.DisplayLayout.Bands[C_SET_CLR].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ))
				{
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugRules.DisplayLayout.ValueLists["Sizes"];
				
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region ALL COLOR SIZE BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_CODE_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugRules.DisplayLayout.ValueLists["Sizes"];
			
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region COLOR DIM BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugRules.DisplayLayout.ValueLists["Dimensions"];

					column = ugRules.DisplayLayout.Bands[C_CLR_SZ_DIM].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion

				#region ALL COLOR DIM BAND
				if (ugRules.DisplayLayout.Bands.Exists(C_ALL_CLR_SZ_DIM))
				{
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["DIMENSIONS_RID"];
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
					column.ValueList = ugRules.DisplayLayout.ValueLists["Dimensions"];
				
					column = ugRules.DisplayLayout.Bands[C_ALL_CLR_SZ_DIM].Columns["SIZE_RULE"];
					//column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDown;		 // BEGIN MID Track #4375 - Quantity column not enterable
					column.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;	 // END MID Track #4375 
					column.ValueList = ugRules.DisplayLayout.ValueLists["Rules"];
				}
				#endregion
			}
			catch (Exception ex)
			{
				HandleException(ex, "CustomizeColumns");
			}
		}

		/// <summary>
		/// Builds context menus to be used with ugRules
		/// </summary>
		/// <remarks>Base method is currently used by the following:
		/// Fill Size Holes Method, Basis Size Method</remarks>
		virtual protected void BuildContextMenus()
		{
			try
			{
				#region SET LEVEL
				MenuItem mnuSetApply = new MenuItem("Apply", new EventHandler(this.ug_rules_apply));
				mnuSetApply.Index = C_CONTEXT_SET_APPLY;
				MenuItem mnuSetClear = new MenuItem("Clear", new EventHandler(this.ug_rules_clear));
				mnuSetClear.Index = C_CONTEXT_SET_CLEAR;
				MenuItem mnuSetSeparator1= new MenuItem("-");
				mnuSetSeparator1.Index = C_CONTEXT_SET_SEP1;
				MenuItem mnuSetAddColor = new MenuItem("Add Color", new EventHandler(this.ug_rules_addchildrow));
				mnuSetAddColor.Index = C_CONTEXT_SET_ADD_CLR;
				SetContext = new ContextMenu( new MenuItem[] {	
																 mnuSetApply, 
																 mnuSetClear, 
																 mnuSetSeparator1, 
																 mnuSetAddColor 
															 });
				#endregion


				#region COLOR LEVEL
				MenuItem mnuColorApply = new MenuItem("Apply", new EventHandler(this.ug_rules_apply));
				mnuColorApply.Index = C_CONTEXT_CLR_APPLY;
				MenuItem mnuColorClear = new MenuItem("Clear", new EventHandler(this.ug_rules_clear));
				mnuColorClear.Index = C_CONTEXT_CLR_CLEAR;
				MenuItem mnuColorSeparator1 = new MenuItem("-");
				mnuColorSeparator1.Index = C_CONTEXT_CLR_SEP1;
				MenuItem mnuColorAddColor = new MenuItem("Add Color", new EventHandler(this.ug_rules_addrow));
				mnuColorAddColor.Index = C_CONTEXT_CLR_ADD_CLR;
				MenuItem mnuColorAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.ug_rules_addchildrow));
				mnuColorAddSizeDim.Index = C_CONTEXT_CLR_ADD_SZ_DIM;
				MenuItem mnuColorSeparator2 = new MenuItem("-");
				mnuColorSeparator2.Index = C_CONTEXT_CLR_SEP2;
				MenuItem mnuColorDelete = new MenuItem("Delete", new EventHandler(this.ug_rules_deleterow));
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
				MenuItem mnuSizeAddSize = new MenuItem("Add Size", new EventHandler(this.ug_rules_addrow));
				mnuSizeAddSize.Index = C_CONTEXT_SZ_ADD_SZ;
				MenuItem mnuSizeSeparator1 = new MenuItem("-");
				mnuSizeSeparator1.Index = C_CONTEXT_SZ_SEP1;	
				MenuItem mnuSizeDelete = new MenuItem("Delete", new EventHandler(this.ug_rules_deleterow));
				mnuSizeDelete.Index = C_CONTEXT_SZ_DELETE;
				SizeContext = new ContextMenu( new MenuItem[]  {   mnuSizeAddSize,
																   mnuSizeSeparator1,
																   mnuSizeDelete
															   });
				#endregion


				#region DIMENSION LEVEL
				MenuItem mnuDimensionApply = new MenuItem("Apply", new EventHandler(this.ug_rules_apply));
				mnuDimensionApply.Index = C_CONTEXT_DIM_APPLY;
				MenuItem mnuDimensionClear = new MenuItem("Clear", new EventHandler(this.ug_rules_clear));
				mnuDimensionClear.Index = C_CONTEXT_DIM_CLEAR;
				MenuItem mnuDimensionSeparator1 = new MenuItem("-");
				mnuDimensionSeparator1.Index = C_CONTEXT_DIM_SEP1;
				MenuItem mnuDimensionAddSizeDim = new MenuItem("Add Size Dimension", new EventHandler(this.ug_rules_addrow));
				mnuDimensionAddSizeDim.Index = C_CONTEXT_DIM_ADD_DIM;
				MenuItem mnuDimensionAddSize = new MenuItem("Add Size", new EventHandler(this.ug_rules_addchildrow));
				mnuDimensionAddSize.Index = C_CONTEXT_DIM_ADD_SZ;
				MenuItem mnuDimensionSeparator2 = new MenuItem("-");
				mnuDimensionSeparator2.Index = C_CONTEXT_DIM_SEP2;
				MenuItem mnuDimensionDelete = new MenuItem("Delete", new EventHandler(this.ug_rules_deleterow));
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

		/// <summary>
		/// Applies data from current row to any child of the current row.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void ug_rules_apply(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugRules.ActiveRow;

				ugRules.EventManager.AllEventsEnabled = false;
				ugRules.PerformAction(UltraGridAction.ExitEditMode);
				ugRules.EventManager.AllEventsEnabled = true;

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
				ugRules.UpdateData();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_apply");
			}		
		}


		/// <summary>
		/// Clears all data in ugRules.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Method must be overridden</remarks>
		virtual protected void ug_rules_clear(object sender, System.EventArgs e)
		{
			try
			{
				UltraGridRow activeRow = ugRules.ActiveRow;

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
				ugRules.UpdateData();
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
		virtual protected void ug_rules_addchildrow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugRules.ActiveRow;
				switch (activeRow.Band.Key.ToUpper())
				{
					case C_SET:
						//SET LEVEL HAS ALL COLOR AND COLOR BAND,
						//ADDNEW IS ONLY ALLOWED FOR COLOR.
						//******************************************
						activeRow.Update();
						ugRules.ActiveRow = activeRow;
						activeRow.ChildBands[C_SET_CLR].Band.AddNew();
						break;
					default:
						//ADD ROW TO ACTIVEROWS FIRST CHILD BAND.
						//********************************************
						activeRow.Update();
						ugRules.ActiveRow = activeRow;
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
		/// Adds a row to ugRules to the current band.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		virtual protected void ug_rules_addrow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugRules.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugRules.ActiveRow = activeRow;
				}
				activeRow.Band.AddNew();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_addrow");
			}	
		}


		/// <summary>
		/// Deletes selected row(s) from ugRules
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		virtual protected void ug_rules_deleterow(object sender, System.EventArgs e) 
		{
			try
			{
				UltraGridRow activeRow = ugRules.ActiveRow;

				activeRow.Selected = true;
				ugRules.DeleteSelectedRows();
			}
			catch (Exception ex)
			{
				HandleException(ex, "ug_allsize_deleterow");
			}	
		}


		

		/// <summary>
		/// Returns a AllocationProfileList for an array of headers.
		/// </summary>
		/// <param name="selectedHeaders">Array of headers to get profiles for</param>
		/// <returns>AllocationProfileList object</returns>
		/// <example>
		/// <code>
		///		int intHeaderRID = 100;
		///		int[] selectedHeaders = {intHeaderRID};
		///		AllocationProfileList apl = null;
		///		apl = GetAllocationProfile(selectedHeaders);
		///		_basisHeaderProfile = (AllocationProfile)apl[0];
		/// </code> 
		///</example>
		protected AllocationProfileList GetAllocationProfile(int[] selectedHeaders)
		{
			try
			{
				AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
				apl.LoadHeaders(SAB.ClientServerSession.CreateTransaction(), null, selectedHeaders, SAB.ApplicationServerSession); // TT#488 - MD - Jellis - Group Allocation
				//  This method appears to be called only from BasisSizeMethod.cs to fill in the "basis".
                //  It is assumed that they basis header will not be an Assortment or a member of an assortment.
                //  IF this assumption is wrong, then the Assortment Header list will need to be NOT NULL and the Transaction will need to be an Application Sesssion Transaction.
                return apl;
			}
			catch (Exception ex)
			{
				HandleException(ex, "GetAllocationProfile(int[]  selectedAssortmentRIDs, int[] selectedHeaders)"); // TT#488 - MD - Jellis - Group Allocation
				return null;
			}
		}


		/// <summary>
		/// Returns a AllocationProfile object for a single header.
		/// </summary>
		/// <param name="aHeaderRID">Header to get profile for</param>
		/// <returns>AllocationProfile object</returns>
		/// <example>
		/// <code>
		///		AllocationProfile _basisHeaderProfile = null;
		///		int intHeaderRID = 100;
		///		_basisHeaderProfile = GetAllocationProfile(intHeaderRID);
		/// </code>
		/// </example>
		protected AllocationProfile GetAllocationProfile(int aHeaderRID)
		{
			try
			{
				int[] selectedHeaders = {aHeaderRID};
				
				return (AllocationProfile)GetAllocationProfile(selectedHeaders)[0];

			}
			catch (Exception ex)
			{
				HandleException(ex, "GetAllocationProfile(int aHeaderRID)");
				return null;
			}
		}


		/// <summary>
		/// Clears the size group combo box.
		/// </summary>
		virtual protected void ClearSizeGroupComboBox()
		{
			try
			{
				cboSizeGroup.DataSource = null;
                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeGroup.Items.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.ClearSizeGroupComboBox");
			}	
		}

		/// <summary>
		/// Clears the size curve combo box.
		/// </summary>
		virtual protected void ClearSizeCurveComboBox()
		{
			try
			{
				cboSizeCurve.DataSource = null;
                //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeCurve.Items.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.ClearSizeCurveComboBox");
			}	
		}

		/// <summary>
		/// Clears the filter combo box.
		/// </summary>
		virtual protected void ClearAttributeComboBox()
		{
			try
			{
				cboStoreAttribute.DataSource = null;
                //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboStoreAttribute.Items.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.ClearAttributeComboBox");
			}	
		}


		/// <summary>
		/// Clears the filter combo box.
		/// </summary>
		virtual protected void ClearFilterComboBox()
		{
			try
			{
				cboFilter.DataSource = null;
                //this.cboFilter_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboFilter.Items.Clear();
			}
			catch (Exception ex)
			{
				HandleException(ex, "SizeMethodFormBase.ClearFilterComboBox");
			}	
		}


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


		protected DialogResult ShowWarningPrompt(bool basisSubstitute)
		{
			
			
			try
			{
				DialogResult drResult;
				drResult = DialogResult.Yes;
				ChangePending = true;
				ConstraintRollback = true;

                // Begin TT#498 - RMatelic - Size Need Method Default checked on the Rule Tab the Add only goes down to Color.- unrelated to error: use APPLICATION _TEXT  
                //string msg = "Warning:\nChanging this value will cause the current Rule ";
                //if (basisSubstitute)
                //    msg += "and Basis Substitute ";
                //msg += "information to be immediately erased." +
                //        "\nTo return to the original constraint information close the form without saving or updating.\nDo you wish to continue?";
                string msg1 = string.Empty;
                if (basisSubstitute)
                {
                    msg1 = MIDText.GetTextOnly(eMIDTextCode.lbl_AndBasisSubstitute);
                }
                string msg = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_al_SizeWarningPrompt), msg1);
                // End TT#498
               
				drResult = MessageBox.Show(msg,	"Confirmation", MessageBoxButtons.YesNo);

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
			throw new Exception("Can not call base method CopyAllSizeData(UltraGridRow activeRow).");
		}

		virtual protected void ClearAllSizeData(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method ClearAllSizeData(UltraGridRow activeRow).");
		}

		virtual protected void ClearSetData(UltraGridRow activeRow)
		{
			throw new Exception("Can not call base method ClearSetData(UltraGridRow activeRow).");
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
						allColorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						allColorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;


						if (allColorRow.HasChild())
						{
							foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[0].Rows)
							{
								allColorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								allColorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								allColorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
								allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
								allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (allColorDimRow.HasChild())
								{
									foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
									{
										allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
										allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
										allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
						colorRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						colorRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;


						if (colorRow.HasChild())
						{
							foreach (UltraGridRow colorDimRow in colorRow.ChildBands[0].Rows)
							{
								colorDimRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								colorDimRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								colorDimRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
								colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
								colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

								if (colorDimRow.HasChild())
								{
									foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
									{
										colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
										colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
										colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
										colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
										colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
						colorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						colorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
								colorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								colorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								colorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
								colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
								colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorDimRow in activeRow.ChildBands[0].Rows)
					{
						colorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
						colorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
						colorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
						colorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
						colorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

						if (colorDimRow.HasChild())
						{
							foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[0].Rows)
							{
								colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
								colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
								colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
								colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
								colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
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
						allColorDimRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						allColorDimRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
								allColorSizeRow.Cells["SIZE_MIN"].Value = activeRow.Cells["SIZE_MIN"].Value;
								allColorSizeRow.Cells["SIZE_MAX"].Value = activeRow.Cells["SIZE_MAX"].Value;
								allColorSizeRow.Cells["SIZE_MULT"].Value = activeRow.Cells["SIZE_MULT"].Value;
								allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
								allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorDimRow in activeRow.ChildBands[0].Rows)
					{
						allColorDimRow.Cells["SIZE_MIN"].Value = string.Empty;
						allColorDimRow.Cells["SIZE_MAX"].Value = string.Empty;
						allColorDimRow.Cells["SIZE_MULT"].Value = string.Empty;
						allColorDimRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
						allColorDimRow.Cells["SIZE_RULE"].Value = string.Empty;

						if (allColorDimRow.HasChild())
						{
							foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[0].Rows)
							{
								allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
								allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
								allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
								allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
								allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
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
						allColorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						allColorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow allColorSizeRow in activeRow.ChildBands[0].Rows)
					{
						allColorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
						allColorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
						allColorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
						allColorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
						allColorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
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
						colorSizeRow.Cells["SIZE_QUANTITY"].Value = activeRow.Cells["SIZE_QUANTITY"].Value;
						colorSizeRow.Cells["SIZE_RULE"].Value = activeRow.Cells["SIZE_RULE"].Value;
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
				activeRow.Cells["SIZE_MIN"].Value = string.Empty;
				activeRow.Cells["SIZE_MAX"].Value = string.Empty;
				activeRow.Cells["SIZE_MULT"].Value = string.Empty;
				activeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
				activeRow.Cells["SIZE_RULE"].Value = string.Empty;

				//COPY ALL COLOR DATA TO ALL COLOR ANCESTORS
				if (activeRow.HasChild())
				{
					foreach(UltraGridRow colorSizeRow in activeRow.ChildBands[0].Rows)
					{
						colorSizeRow.Cells["SIZE_MIN"].Value = string.Empty;
						colorSizeRow.Cells["SIZE_MAX"].Value = string.Empty;
						colorSizeRow.Cells["SIZE_MULT"].Value = string.Empty;
						colorSizeRow.Cells["SIZE_QUANTITY"].Value = string.Empty;
						colorSizeRow.Cells["SIZE_RULE"].Value = string.Empty;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "ClearColorSizeData");
			}
		}

		


//		/// <summary>
//		/// Binds ugRules datagrid
//		/// </summary>
//		/// <remarks>
//		/// If the datasource will not be a dataset, then override the overloaded method
//		///	that takes no arguments.
//		/// </remarks>
//		/// <param name="dsGridData">Dataset to bind to ugRules</param>
//		virtual protected void BindAllSizeGrid(DataSet dsGridData)
//		{
//			try
//			{
//				if (DataSetBackup == null)
//				{
//					DataSetBackup = dsGridData; //dsGridData.Copy();
//				}
//
//				this.ugRules.DataSource = null;
//				this.ugRules.DataSource = dsGridData;
//				//this.ugRules.DataBind();
//			}
//			catch (Exception ex)
//			{
//				HandleException(ex, "BindAllSizeGrid");
//			}
//		}


		/// <summary>
		/// Binds ugAllSize datagrid
		/// </summary>
		/// <remarks>
		/// If the datasource will not be a dataset, then override the overloaded method
		///	that takes no arguments.
		/// </remarks>
		/// <param name="dsGridData">Dataset to bind to ugAllSize</param>
		virtual protected void BindAllSizeGrid(DataSet dsGridData)
		{
			try
			{
				if (DataSetBackup == null)
				{
					DataSetBackup = dsGridData.Copy(); //dsGridData.Copy();
				}

				this.ugRules.DataSource = null;
				this.ugRules.DataSource = dsGridData;
				//this.ugAllSize.DataBind();
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindAllSizeGrid");
			}
		}
		// BEGIN ANF Generic Size Cnstraint
		/// <summary>
		/// Populates the size combo boxes.
		/// </summary>
		virtual protected void BindSizeComboBoxes(int aSizeGroupRID, int aSizeAlternteRID, int aSizeCurevGroupRID,
												  int aSizeConstraintRID)
		{
			try
			{
				if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
				{
					BindSizeGroupComboBox(true, aSizeGroupRID);
				}
				else
				{
					BindSizeGroupComboBox(true, Include.NoRID);  
				}
				if (SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask!= string.Empty)
				{
					BindSizeConstraintsComboBox(aSizeAlternteRID);
				}
				else
				{
					BindSizeAlternatesComboBox(Include.NoRID);  
				}
				if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
				{
					BindSizeCurveComboBox(true, aSizeCurevGroupRID);
				}
				else
				{
					BindSizeCurveComboBox(true, Include.NoRID);  
				} 
				if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
				{
					BindSizeConstraintsComboBox(aSizeConstraintRID);
				}
				else
				{
					BindSizeConstraintsComboBox(Include.NoRID);  
				}
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END ANF Generic Size Cnstraint

		/// <summary>
		/// Populates the size curve combo box.
		/// </summary>
		virtual protected void BindSizeCurveComboBox()
		{
			try
			{
				BindSizeCurveComboBox(true, Include.NoRID); // MID Track 3781 Size Curve not required for Fill Size Holes
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}


		/// <summary>
		/// Populates the size curve combo box.
		/// </summary>
		virtual protected void BindSizeCurveComboBox(bool includeEmptySelection, int aSizeCurveGroupRID)
		{
			try
			{
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
				// BEGIN ANF Generic Size Cnstraint
				if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
				{
					// Do datatable lookup even if aSizeCurveGroupRID = -1 to get the columns for the empty row 
					dtSizeCurve = objSizeCurve.GetSpecificSizeCurveGroup(aSizeCurveGroupRID);
				}
				else
				{
					dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
				}
				// END ANF Generic Size Cnstraint
				if (includeEmptySelection)
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				
				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
				ClearSizeCurveComboBox();

				cboSizeCurve.DataSource = dvSizeCurve;
                //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
                AdjustTextWidthComboBox_DropDown(cboSizeCurve);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		
		// BEGIN ANF Generic Size Cnstraint
		virtual protected void BindSizeCurveComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				DataTable dtSizeCurve = MIDEnvironment.CreateDataTable();
				SizeCurve objSizeCurve = new SizeCurve();
//				dtSizeCurve = objSizeCurve.GetSizeCurveGroups();
//				 
//				if (includeEmptySelection)
//				{							
//					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
//				}
//
//				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
//				//ClearSizeCurveComboBox();
//				aFilterString = aFilterString.Replace("*","%");
//				aFilterString = aFilterString.Replace("'","''");	// for string with single quote
//
//				dvSizeCurve.RowFilter = "SIZE_CURVE_GROUP_NAME LIKE '' or " +
//								        "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";
				
				// RowFilter didn't work with multiple wild cards 
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_CURVE_GROUP_NAME LIKE " +  "'" + aFilterString + "'";	
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
				
				//dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroups(whereClause);
                if (aCaseSensitive)
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeCurve = objSizeCurve.GetFilteredSizeCurveGroupsCaseInsensitive(aFilterString);
                }

				if (includeEmptySelection)
				{							
					dtSizeCurve.Rows.Add(new object[] { Include.NoRID, ""} );
				}

				DataView dvSizeCurve = new DataView(dtSizeCurve, "", "SIZE_CURVE_GROUP_NAME", DataViewRowState.CurrentRows);
				cboSizeCurve.DataSource = dvSizeCurve;
                //this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeCurve.DisplayMember = "SIZE_CURVE_GROUP_NAME";
				cboSizeCurve.ValueMember = "SIZE_CURVE_GROUP_RID";
				cboSizeCurve.Enabled = true;
                AdjustTextWidthComboBox_DropDown(cboSizeCurve);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeCurveComboBox");
			}
		}
		// END ANF Generic Size Cnstraint

		/// <summary>
		/// Populates the size group combo box.
		/// </summary>
		virtual protected void BindSizeGroupComboBox()
		{
			try
			{
				BindSizeGroupComboBox(false, Include.NoRID); 
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeGroupComboBox");
			}
		}

		/// <summary>
		/// Populates the size group combo box.
		/// </summary>
		virtual protected void BindSizeGroupComboBox(bool includeEmptySelection, int aSizeGroupRID)
		{
			try
			{
				DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
				
				sizeGroupList.LoadAll(false);
				
				dtGroups.Columns.Add("Key");
				dtGroups.Columns.Add("Name");		
				if (includeEmptySelection)
				{
					dtGroups.Rows.Add( new object[] { Include.NoRID, "" } ) ;
				}

				// BEGIN ANF Generic Size Consraint
				if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
				{
					if (aSizeGroupRID != Include.NoRID)
					{
						SizeGroupProfile sgp = (SizeGroupProfile)sizeGroupList.FindKey(aSizeGroupRID);
						dtGroups.Rows.Add(new object[] { sgp.Key, sgp.SizeGroupName });
					}
				}
				else
				{
					foreach(SizeGroupProfile sgp in sizeGroupList.ArrayList)
					{
						dtGroups.Rows.Add(new object[] { sgp.Key, sgp.SizeGroupName });
					}
				}
				// END ANF Generic Size Consraint

				ClearSizeGroupComboBox();

				cboSizeGroup.DataSource = dtGroups;
                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeGroup.DisplayMember = "Name";
				cboSizeGroup.ValueMember = "Key";
                AdjustTextWidthComboBox_DropDown(cboSizeGroup);  // TT#1401 - AGallagher - Reservation Stores
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeGroupComboBox");
			}
		}

		// BEGIN ANF Generic Size Consraint
		virtual protected void BindSizeGroupComboBox(bool includeEmptySelection, string aFilterString, bool aCaseSensitive)
		{
			try
			{
				DataTable dtGroups = MIDEnvironment.CreateDataTable("Groups");
				// Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
			
				SizeGroup sizeGroup = new SizeGroup();
				aFilterString = aFilterString.Replace("*","%");
				//aFilterString = aFilterString.Replace("'","''");	// for string with single quote
			
                //string whereClause = "SIZE_GROUP_NAME LIKE '" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
                //dtGroups = sizeGroup.SizeGroup_FilterRead(whereClause);
                if (aCaseSensitive)
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtGroups = sizeGroup.SizeGroup_FilterReadCaseInsensitive(aFilterString);
                }
				if (includeEmptySelection)
				{
					dtGroups.Rows.Add( new object[] { Include.NoRID, "" } ) ;
				}

				dtGroups.DefaultView.Sort = "SIZE_GROUP_NAME ASC"; 
				dtGroups.Columns["SIZE_GROUP_RID"].ColumnName = "Key";
				dtGroups.Columns["SIZE_GROUP_NAME"].ColumnName = "Name";
				dtGroups.AcceptChanges();
				
				cboSizeGroup.DataSource = dtGroups;
                //this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboSizeGroup.DisplayMember = "Name";
				cboSizeGroup.ValueMember = "Key";
				cboSizeGroup.Enabled = true;
                AdjustTextWidthComboBox_DropDown(cboSizeGroup);  // TT#1401 - AGallagher - Reservation Stores
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeGroupComboBox");
			}
		}

		// END ANF Generic Size Consraint

		/// <summary>
		/// Populate all Store_Groups (Attributes); 1st sel if new else selection made
		/// in load
		/// </summary>
        virtual protected void BindStoreAttrComboBox(eChangeType aChangeType, eGlobalUserType aGlobalUserType)
		{
            // Begin Track #4872 - JSmith - Global/User Attributes
            ProfileList profileList;
            // End Track #4872
			try
			{
				ClearAttributeComboBox();
				// BEGIN MID Track 4246 - Attribute list needs to be in alphabetical order 
				//cboStoreAttribute.DataSource = storeData.StoreGroup_Read(eDataOrderBy.RID);
                // Begin Track #4872 - JSmith - Global/User Attributes
                //cboStoreAttribute.DataSource = storeData.StoreGroup_Read(eDataOrderBy.ID);
                // END MID Track 4246
                //cboStoreAttribute.ValueMember = "SG_RID";
                //cboStoreAttribute.DisplayMember = "SG_ID";
                _changeType = aChangeType;
                _globalUserType = aGlobalUserType;
                BuildAttributeList();
                //profileList = GetStoreGroupList(aChangeType, aGlobalUserType, false);
                //cboStoreAttribute.Initialize(SAB, FunctionSecurity, profileList.ArrayList, aGlobalUserType == eGlobalUserType.User);

                //this.cboStoreAttribute.ValueMember = "Key";
                //this.cboStoreAttribute.DisplayMember = "Name";
                //this.cboStoreAttribute.DataSource = profileList.ArrayList;
                // End Track #4872
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}


		/// <summary>
		/// Populates the filter combo box.
		/// </summary>
		virtual protected void BindFilterComboBox()
		{
			try
			{

				_storeFilterData = new FilterData();

				ArrayList userRIDList = new ArrayList();
				userRIDList.Add(Include.GlobalUserRID);		// Issue 3806
				userRIDList.Add(SAB.ClientServerSession.UserRID);
				//_dtStoreFilter = _storeFilterData.StoreFilter_Read(userRIDList);

				// Add 'empty' or 'none' row
                // Begin TT#2669 - JSmith - Unable to remove a filter from a wokflow
                //cboFilter.Items.Add(new FilterNameCombo(Include.NoRID, Include.GlobalUserRID, " "));	//issue 3806
                cboFilter.Items.Add(GetRemoveFilterRow());
                // End TT#2669 - JSmith - Unable to remove a filter from a wokflow

                foreach (DataRow row in storeFilterData.FilterRead(filterTypes.StoreFilter, eProfileType.FilterStore, userRIDList).Rows)
				{
					cboFilter.Items.Add(
						new FilterNameCombo(Convert.ToInt32(row["FILTER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture),
						Convert.ToString(row["FILTER_NAME"], CultureInfo.CurrentUICulture)));
				}
                AdjustTextWidthComboBox_DropDown(cboFilter);  // TT#1401 - AGallagher - Reservation Stores
			}
			catch (Exception ex)
			{
				HandleException(ex, "BindFilterComboBox");
			}
		}

		public void BindSizeConstraintsComboBox(int aSizeConstraintRID)
		{
			try
			{	// BEGIN ANF Generic Size Constraint
				DataTable dtSizeModel;
				if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
				{
					dtSizeModel = SizeModelData.SizeConstraintModel_Read(aSizeConstraintRID);
				}
				else
				{
					dtSizeModel = SizeModelData.SizeConstraintModel_Read();
				}
				// END ANF Generic Size Constraint

				DataRow emptyRow = dtSizeModel.NewRow();
				emptyRow["SIZE_CONSTRAINT_NAME"] = "";
				emptyRow["SIZE_CONSTRAINT_RID"] = Include.NoRID;
				dtSizeModel.Rows.Add(emptyRow);
				dtSizeModel.DefaultView.Sort = "SIZE_CONSTRAINT_NAME ASC"; 
				dtSizeModel.AcceptChanges();

				cboConstraints.DataSource = dtSizeModel;
                //this.cboConstraints_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboConstraints.DisplayMember = "SIZE_CONSTRAINT_NAME";
				cboConstraints.ValueMember = "SIZE_CONSTRAINT_RID";
                AdjustTextWidthComboBox_DropDown(cboConstraints);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeConstraintsComboBox");
			}
		}
		// BEGIN ANF Generic Size Constraint
		public void BindSizeConstraintsComboBox(string aFilterString, bool aCaseSensitive)
		{
			try
			{	
				// Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
                //aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote

                //string whereClause = "SIZE_CONSTRAINT_NAME LIKE " +  "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}

                aFilterString = aFilterString.Replace("*", "%");
                //DataTable dtSizeModel = SizeModelData.SizeConstraintModel_FilterRead(whereClause);
                DataTable dtSizeModel;
                if (aCaseSensitive)
                {
                    dtSizeModel = SizeModelData.SizeConstraintModel_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeModel = SizeModelData.SizeConstraintModel_FilterReadCaseInsensitive(aFilterString);
                }

				DataRow emptyRow = dtSizeModel.NewRow();
				emptyRow["SIZE_CONSTRAINT_NAME"] = "";
				emptyRow["SIZE_CONSTRAINT_RID"] = Include.NoRID;
				dtSizeModel.Rows.Add(emptyRow);
				dtSizeModel.DefaultView.Sort = "SIZE_CONSTRAINT_NAME ASC"; 
				dtSizeModel.AcceptChanges();

				cboConstraints.DataSource = dtSizeModel;
                //this.cboConstraints_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboConstraints.DisplayMember = "SIZE_CONSTRAINT_NAME";
				cboConstraints.ValueMember = "SIZE_CONSTRAINT_RID";
				cboConstraints.Enabled = true;
                AdjustTextWidthComboBox_DropDown(cboConstraints);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeConstraintsComboBox");
			}
		}
		// END ANF Generic Size Constraint
		public void BindSizeAlternatesComboBox(int aSizeAlternateRID)
		{
			try
			{	// BEGIN ANF Generic Size Constraint
				DataTable dtSizeModel;
				if (SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask != string.Empty)
				{
					dtSizeModel = SizeModelData.SizeAlternateModel_Read(aSizeAlternateRID);
				}
				else
				{
					dtSizeModel = SizeModelData.SizeAlternateModel_Read();
				}
				// END ANF Generic Size Constraint

				DataRow emptyRow = dtSizeModel.NewRow();
				emptyRow["SIZE_ALTERNATE_NAME"] = "";
				emptyRow["SIZE_ALTERNATE_RID"] = Include.NoRID;
				dtSizeModel.Rows.Add(emptyRow);
				dtSizeModel.DefaultView.Sort = "SIZE_ALTERNATE_NAME ASC"; 
				dtSizeModel.AcceptChanges();

				cboAlternates.DataSource = dtSizeModel;
                //this.cboAlternates_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboAlternates.DisplayMember = "SIZE_ALTERNATE_NAME";
				cboAlternates.ValueMember = "SIZE_ALTERNATE_RID";
                AdjustTextWidthComboBox_DropDown(cboAlternates);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeAlternatesComboBox");
			}
		}

		// BEGIN ANF Generic Size Constraint
		public void BindSizeAlternatesComboBox(string aFilterString, bool aCaseSensitive)
		{
			try
			{	 
				// Originally tried RowFilter but received invalid rowfilter msg when multiple wild cards
				aFilterString = aFilterString.Replace("*","%");
                //aFilterString = aFilterString.Replace("'","''");	// for string with single quote

                //string whereClause = "SIZE_ALTERNATE_NAME LIKE " +  "'" + aFilterString + "'";
                //if (!aCaseSensitive)
                //{
                //    whereClause += Include.CaseInsensitiveCollation;
                //}
				//DataTable dtSizeModel = SizeModelData.SizeAlternateModel_FilterRead(whereClause);
                DataTable dtSizeModel;
                if (aCaseSensitive)
                {
                    dtSizeModel = SizeModelData.SizeAlternateModel_FilterReadCaseSensitive(aFilterString);
                }
                else
                {
                    dtSizeModel = SizeModelData.SizeAlternateModel_FilterReadCaseInsensitive(aFilterString);
                }

				DataRow emptyRow = dtSizeModel.NewRow();
				emptyRow["SIZE_ALTERNATE_NAME"] = "";
				emptyRow["SIZE_ALTERNATE_RID"] = Include.NoRID;
				dtSizeModel.Rows.Add(emptyRow);
				dtSizeModel.DefaultView.Sort = "SIZE_ALTERNATE_NAME ASC"; 
				dtSizeModel.AcceptChanges();
		
				cboAlternates.DataSource = dtSizeModel;
                //this.cboAlternates_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboAlternates.DisplayMember = "SIZE_ALTERNATE_NAME";
				cboAlternates.ValueMember = "SIZE_ALTERNATE_RID";
				cboAlternates.Enabled = true;
                AdjustTextWidthComboBox_DropDown(cboAlternates);  // TT#1401 - AGallagher - Reservation Stores
			}		
			catch (Exception ex)
			{
				HandleException(ex, "BindSizeAlternatesComboBox");
			}
		}
		
		// A&F Begin Generic SIze Curve
		virtual protected void LoadGenericSizeCurveGroupBox()
		{
			try 
			{
				gbSizeGroup.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Size_Group); 
				gbSizeAlternate.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_AlternatesModel); 
				gbSizeCurve.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_SizeCurve); 
				gbSizeConstraints.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_SizeConstraints); 
				gbGenericSizeCurve.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Generic); 
				gbGenericConstraint.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Generic); 
				cbColor.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
				cbConstrColor.Text = MIDText.GetTextOnly((int)eMIDTextCode.lbl_Color);
				LoadHeaderCharacteristicsComboBoxes();
				LoadHierarchyLevelComboBoxes();

                // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings
                LoadNameExtenstionComboBox();
                if (SAB.ClientServerSession.GlobalOptions.GenericSizeCurveNameType == eGenericSizeCurveNameType.NodePropertiesName)
                {
                    cboNameExtension.Visible = true;
                    cboHeaderChar.Visible = false;
                    cboHierarchyLevel.Location = new System.Drawing.Point(33, 17);
                    cboHierarchyLevel.BringToFront();
                    cbColor.Checked = false;
                    cbColor.Visible = false;
                }
                else
                {
                    cboNameExtension.Visible = false;
                    cboHeaderChar.Visible = true;
                    cboHeaderChar.BringToFront();
                    cboHierarchyLevel.BringToFront();
                    cboHierarchyLevel.Location = new System.Drawing.Point(33, 47);
                    cbColor.Visible = true;
                }
                // End TT#413 
			}		
			catch (Exception ex)
			{
				HandleException(ex, "LoadGenericSizeCurveGroupBox");
			}
		}

		private void LoadHeaderCharacteristicsComboBoxes()
		{
			try
			{
				DataTable dtHeaderChar = MIDEnvironment.CreateDataTable();
				dtHeaderChar.Columns.Add("Key");
                // Begin TT#1408-MD - RMatelic - Starting a header characteristic with a special character causes issues within size methods (Size Need, Fill Size, & Basis Size)
                dtHeaderChar.Columns.Add("PreSortKey");
                // End TT#1408-MD
				dtHeaderChar.Columns.Add("CharGroupID");
				DataRow dRow;
			
				_headerCharGroupProfileList = SAB.HeaderServerSession.GetHeaderCharGroups();
				foreach (HeaderCharGroupProfile hcgp in _headerCharGroupProfileList)
				{
					dRow = dtHeaderChar.NewRow();
					dRow["Key"] = hcgp.Key;
					dRow["CharGroupID"] = hcgp.ID;
                    // Begin TT#1408-MD - RMatelic - Starting a header characteristic with a special character causes issues within size methods (Size Need, Fill Size, & Basis Size)
                    dRow["PreSortKey"] = 9;
                    // End TT#1408-MD
				    dtHeaderChar.Rows.Add(dRow);
				}
				dRow = dtHeaderChar.NewRow();
				dRow["Key"] = Include.NoRID;
				// Begin MID Track 4372 Generic Size Constraint
				//dRow["CharGroupID"] = MIDText.GetTextOnly(eMIDTextCode.lbl_NoHeaderCharSelected);
				_noHeaderCharSelectedLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_NoHeaderCharSelected);
				dRow["CharGroupID"] = _noHeaderCharSelectedLabel;
				// End MID Track 4372 Generic Size Constraint
                // Begin TT#1408-MD - RMatelic - Starting a header characteristic with a special character causes issues within size methods (Size Need, Fill Size, & Basis Size)
                dRow["PreSortKey"] = 0;
                // End TT#1408-MD
				dtHeaderChar.Rows.Add(dRow);
                // Begin TT#1408-MD - RMatelic - Starting a header characteristic with a special character causes issues within size methods (Size Need, Fill Size, & Basis Size)
                //dtHeaderChar.DefaultView.Sort = "CharGroupID ASC";
                dtHeaderChar.DefaultView.Sort = "PreSortKey, CharGroupID ASC"; 
                // End TT#1408-MD
				dtHeaderChar.AcceptChanges();

				cboHeaderChar.DataSource = dtHeaderChar;
                //this.cboHeaderChar_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboHeaderChar.DisplayMember = "CharGroupID";
				cboHeaderChar.ValueMember = "Key";
                AdjustTextWidthComboBox_DropDown(cboHeaderChar);  // TT#1401 - AGallagher - Reservation Stores
				
				// BEGIN ANF Generic Size Constraint
				DataTable dtConstrHeaderChar = dtHeaderChar.Copy();
                // Begin TT#1408-MD - RMatelic - Starting a header characteristic with a special character causes issues within size methods (Size Need, Fill Size, & Basis Size)
                //dtConstrHeaderChar.DefaultView.Sort = "CharGroupID ASC"; 
                dtConstrHeaderChar.DefaultView.Sort = "PreSortKey, CharGroupID ASC"; 
                // End TT#1408-MD
				dtConstrHeaderChar.AcceptChanges();
				cboConstrHeaderChar.DataSource = dtConstrHeaderChar;
                //this.cboConstrHeaderChar_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboConstrHeaderChar.DisplayMember = "CharGroupID";
				cboConstrHeaderChar.ValueMember = "Key";
                AdjustTextWidthComboBox_DropDown(cboConstrHeaderChar);  // TT#1401 - AGallagher - Reservation Stores
				// END ANF Generic Size Constraint
			}		
			catch (Exception ex)
			{
				HandleException(ex, "LoadHeaderCharacteristicsComboBox");
			}
		}
		
	 	private void LoadHierarchyLevelComboBoxes()
		{
			try
			{
				DataRow dRow;
				if (MerchandiseDataTable == null)
				{
					BuildDataTables();
				}
				_dtCurveHierLevel = MerchandiseDataTable.Copy();
 
				// remove the color level
				for (int i = _dtCurveHierLevel.Rows.Count - 1; i >= 0; i--)
				{
					dRow = _dtCurveHierLevel.Rows[i];
					int level = Convert.ToInt32(dRow["seqno"], CultureInfo.CurrentUICulture);  
					HierarchyLevelProfile hlp = (HierarchyLevelProfile)HP.HierarchyLevels[level];
					if (hlp.LevelType == eHierarchyLevelType.Color)
					{
						_dtCurveHierLevel.Rows.Remove(dRow);
						break;
					}
				}

			    dRow = _dtCurveHierLevel.NewRow();
				dRow["seqno"] = -2;
				dRow["leveltypename"] = eMerchandiseType.Undefined;
				// Begin MID Track 4372 Generic Size Constraint
				//dRow["text"] = MIDText.GetTextOnly(eMIDTextCode.lbl_NoHierLevelSelected);
				_noHierLevelSelectedLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_NoHierLevelSelected);
				dRow["text"] = _noHierLevelSelectedLabel;
				// End MID Track 4372 Generic Size Constraint

				dRow["key"] = -2;
				_dtCurveHierLevel.Rows.Add(dRow); 
				_dtCurveHierLevel.DefaultView.Sort = "seqno ASC"; 
				_dtCurveHierLevel.AcceptChanges();
			 
				cboHierarchyLevel.DataSource = _dtCurveHierLevel;
                //this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboHierarchyLevel.DisplayMember = "text";
				cboHierarchyLevel.ValueMember = "seqno";
                AdjustTextWidthComboBox_DropDown(cboHierarchyLevel);  // TT#1401 - AGallagher - Reservation Stores
				
				// BEGIN ANF Generic Size Constraint
				_dtConstraintHierLevel = _dtCurveHierLevel.Copy();
				_dtConstraintHierLevel.DefaultView.Sort = "seqno ASC"; 
				_dtConstraintHierLevel.AcceptChanges();
			
				cboConstrHierLevel.DataSource = _dtConstraintHierLevel;
                //this.cboConstrHierLevel_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
				cboConstrHierLevel.DisplayMember = "text";
				cboConstrHierLevel.ValueMember = "seqno";
                AdjustTextWidthComboBox_DropDown(cboConstrHierLevel);  // TT#1401 - AGallagher - Reservation Stores
				// END ANF Generic Size Constraint
			}		
			catch (Exception ex)
			{
				HandleException(ex, "LoadHierarchyLevelComboBox");
			}
		}

        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings
        private void LoadNameExtenstionComboBox()
        {
            try
            {
                DataTable dtNameExtension = MIDEnvironment.CreateDataTable();
                dtNameExtension.Columns.Add("Key");
                dtNameExtension.Columns.Add("CurveName");
                DataRow dRow;

                MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
                DataTable dtNodeSizeCurves = mhd.SizeCurveNames_Read();
                if (dtNodeSizeCurves.Rows.Count > 0)
                {
                    for (int i = 0; i < dtNodeSizeCurves.Rows.Count; i++)
                    {
                        DataRow dr = dtNodeSizeCurves.Rows[i];
                        dRow = dtNameExtension.NewRow();
                        dRow["Key"] = dr["NSCCD_RID"];;
                        dRow["CurveName"] = dr["CURVE_NAME"];
                        dtNameExtension.Rows.Add(dRow);
                    }
                }

                dRow = dtNameExtension.NewRow();
                dRow["Key"] = Include.NoRID;
                _noNameExtensionSelectedLabel = MIDText.GetTextOnly(eMIDTextCode.lbl_NoNameExtensionSelected);
                dRow["CurveName"] = _noNameExtensionSelectedLabel;
                dtNameExtension.Rows.Add(dRow);
                dtNameExtension.DefaultView.Sort = "CurveName ASC";
                dtNameExtension.AcceptChanges();

                cboNameExtension.DataSource = dtNameExtension;
                //this.cboNameExtension_SelectionChangeCommitted(source, new EventArgs()); // TT#301-MD - JSmith - Controls are not functioning properly
                cboNameExtension.DisplayMember = "CurveName";
                cboNameExtension.ValueMember = "Key";
                AdjustTextWidthComboBox_DropDown(cboNameExtension);  // TT#1401 - AGallagher - Reservation Stores

            }
            catch (Exception ex)
            {
                HandleException(ex, "LoadNameExtensionComboBox");
            }
        }
        // End TT#413

		// BEGIN TT#696-MD - Stodd - add "active process"
		// Moved to WorkflowMethodFormBase.cs
		//public bool OkToProcess()
		//{
		//    bool okToProcess = true;

		//    // BEGIN TT#497-MD - stodd -  Methods will not process with Method open
		//    SelectedHeaderList selectedHeaderList = null;
		//    //bool isProcessingInAssortment = false;
		//    bool useAssortmentHeaders = false;
		//    try
		//    {
		//        useAssortmentHeaders = UseAssortmentSelectedHeaders();
		//        if (useAssortmentHeaders)
		//        {
		//            selectedHeaderList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, eMethodType.SizeNeedAllocation);
		//        }
		//        else
		//        {
		//            // BEGIN MID Track #6022 - DB error trying to process new unsaved header
		//            selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
		//        }
		//        //SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
		//        // END TT#497-MD - stodd -  Methods will not process with Method open

		//        if (selectedHeaderList.Count == 0)
		//        {
		//            MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NoHeaderSelectedOnWorkspace));
		//            okToProcess = false;
		//        }

		//    }
		//    catch (Exception ex)
		//    {
		//        okToProcess = false;
		//        HandleException(ex, "SizeMethodFormBase: OkToProcess");
		//    }
		//    return okToProcess;
		//}
		// END TT#696-MD - Stodd - add "active process"
		// A&F End Generic SIze Curve		
	
		//public void BindSizeFringeComboBox()
		//{
			// begin MID Track 3619 Remove Fringe
//			try
//			{
//				DataTable dtSizeModel = SizeModelData.SizeFringeModel_Read();
//				DataRow emptyRow = dtSizeModel.NewRow();
//				emptyRow["SIZE_FRINGE_NAME"] = "";
//				emptyRow["SIZE_FRINGE_RID"] = Include.NoRID;
//				dtSizeModel.Rows.Add(emptyRow);
//				dtSizeModel.DefaultView.Sort = "SIZE_FRINGE_NAME ASC"; 
//				dtSizeModel.AcceptChanges();
//
//				cboFringe.DataSource = dtSizeModel;
//				cboFringe.DisplayMember = "SIZE_FRINGE_NAME";
//				cboFringe.ValueMember = "SIZE_FRINGE_RID";
//			}		
//			catch (Exception ex)
//			{
//				HandleException(ex, "BindSizeFringeComboBox");
//			}
			// end MID Track 3619 Remove Fringe
		//}

	#endregion

	#region Control Event Handlers
        //private void cboFilter_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    if (FormLoaded)
        //    {
        //        ChangePending = true;
        //    }
        //}

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
		/// <summary>
		/// Sets the initial layout of ugRules grid.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <remarks>Add customized code to the this event on the form that inherits SizeMethodFormBase</remarks>
		private void ugRules_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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

		private void SizeMethodsFormBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            try
            {
                // Begin TT#301-MD - JSmith - Controls are not functioning properly
                //this.ugRules.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ugRules_SelectionDrag);
                //this.ugRules.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseDown);
                //this.ugRules.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseUp);
                //this.ugRules.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowUpdate);
                //this.ugRules.AfterRowsDeleted -= new System.EventHandler(this.ugRules_AfterRowsDeleted);
                //this.ugRules.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugRules_MouseEnterElement);
                //this.ugRules.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugRules_BeforeRowUpdate);
                //this.ugRules.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugRules_BeforeRowInsert);
                //this.ugRules.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyDown);
                //this.ugRules.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyUp);
                //this.ugRules.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellUpdate);
                //this.ugRules.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
                ////Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                //this.ugRules.AfterCellListCloseUp -= new CellEventHandler(ugRules_AfterCellListCloseUp);
                //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                //ugld.DetachGridEventHandlers(ugRules);
                ////End TT#169
                //this.ugRules.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeCellDeactivate);
                //this.ugRules.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeEnterEditMode);
                //this.ugRules.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowInsert);

                //this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                //this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                //this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                //this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                //this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                //this.cboHierarchyLevel.DragEnter -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
                //this.cboHierarchyLevel.DragOver -= new DragEventHandler(comboHierLevel_DragOver);
                //// Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
                //this.cboConstrHierLevel.DragEnter -= new System.Windows.Forms.DragEventHandler(comboHierLevel_DragEnter);
                //this.cboConstrHierLevel.DragOver -= new DragEventHandler(comboHierLevel_DragOver);
                //// End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
                //this.cboHierarchyLevel.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.comboBoxHierLevel_KeyDown);
                //this.cboHierarchyLevel.Validating -= new System.ComponentModel.CancelEventHandler(this.comboBoxHierLevel_Validating);
                //this.cboHierarchyLevel.Validated -= new System.EventHandler(this.comboBoxHierLevel_Validated);
                //// Begin TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
                ////this.cboHierarchyLevel.DragDrop -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
                //this.cboHierarchyLevel.DragDrop -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragDrop);
                //// End TT#296-MD - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
                //this.cbColor.CheckedChanged -= new System.EventHandler(this.cbColor_CheckedChanged); 

                this.cboSizeGroup.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeGroup_SelectionChangeCommitted);
                this.cboStoreAttribute.SelectionChangeCommitted -= new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
                this.cboStoreAttribute.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragDrop);
                this.cboStoreAttribute.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
                this.cboStoreAttribute.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
                //this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                this.cboFilter.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
                this.cboFilter.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
                this.cboFilter.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
                this.cboSizeCurve.SelectionChangeCommitted -= new System.EventHandler(this.cboSizeCurve_SelectionChangeCommitted);
                this.cboAlternates.SelectionChangeCommitted -= new System.EventHandler(this.cboAlternates_SelectionChangeCommitted);
                this.cboConstraints.SelectionChangeCommitted -= new System.EventHandler(this.cboConstraints_SelectionChangeCommitted);
                this.ugRules.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellUpdate);
                this.ugRules.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugRules_InitializeLayout);
                this.ugRules.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ugRules_InitializeRow);
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugRules);
                this.ugRules.AfterRowsDeleted -= new System.EventHandler(this.ugRules_AfterRowsDeleted);
                this.ugRules.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowInsert);
                this.ugRules.AfterRowUpdate -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugRules_AfterRowUpdate);
                this.ugRules.AfterRowActivate -= new EventHandler(ugRules_AfterRowActivate);
                this.ugRules.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugRules_BeforeRowUpdate);
                this.ugRules.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugRules_AfterCellListCloseUp);
                this.ugRules.BeforeCellDeactivate -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeCellDeactivate);
                this.ugRules.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugRules_BeforeEnterEditMode);
                this.ugRules.SelectionDrag -= new System.ComponentModel.CancelEventHandler(this.ugRules_SelectionDrag);
                this.ugRules.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugRules_BeforeRowInsert);
                this.ugRules.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugRules_MouseEnterElement);
                this.ugRules.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyDown);
                this.ugRules.KeyUp -= new System.Windows.Forms.KeyEventHandler(this.ugRules_KeyUp);
                this.ugRules.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseDown);
                this.ugRules.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugRules_MouseUp);
                this.cboHeaderChar.SelectedIndexChanged -= new System.EventHandler(this.cboHeaderChar_SelectedIndexChanged);
                this.cboHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboHeaderChar_SelectionChangeCommitted);
                this.cboHierarchyLevel.SelectedIndexChanged -= new System.EventHandler(this.cboHierarchyLevel_SelectedIndexChanged);
                this.cboHierarchyLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboHierarchyLevel_SelectionChangeCommitted);
                this.cboHierarchyLevel.DragDrop -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragDrop);
                this.cboHierarchyLevel.DragEnter -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
                this.cboHierarchyLevel.DragOver -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragOver);
                //this.cboHierarchyLevel.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.comboBoxHierLevel_KeyDown);
                this.cboHierarchyLevel.Validating -= new System.ComponentModel.CancelEventHandler(this.comboBoxHierLevel_Validating);
                this.cboHierarchyLevel.Validated -= new System.EventHandler(this.comboBoxHierLevel_Validated);
                this.cbColor.CheckedChanged -= new System.EventHandler(this.cbColor_CheckedChanged);
                this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectedIndexChanged);
                this.cboNameExtension.SelectionChangeCommitted -= new System.EventHandler(this.cboNameExtension_SelectionChangeCommitted);
                this.cbxUseDefaultcurve.CheckedChanged -= new System.EventHandler(this.cbxUseDefaultcurve_CheckedChanged);
                this.picBoxCurve.Click -= new System.EventHandler(this.picBoxFilter_Click);
                this.picBoxCurve.MouseHover -= new System.EventHandler(this.picBoxFilter_MouseHover);
                this.picBoxConstraint.Click -= new System.EventHandler(this.picBoxFilter_Click);
                this.picBoxConstraint.MouseHover -= new System.EventHandler(this.picBoxFilter_MouseHover);
                this.cboConstrHeaderChar.SelectedIndexChanged -= new System.EventHandler(this.cboHeaderChar_SelectedIndexChanged);
                this.cboConstrHeaderChar.SelectionChangeCommitted -= new System.EventHandler(this.cboConstrHeaderChar_SelectionChangeCommitted);
                this.cboConstrHierLevel.SelectedIndexChanged -= new System.EventHandler(this.cboHierarchyLevel_SelectedIndexChanged);
                this.cboConstrHierLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboConstrHierLevel_SelectionChangeCommitted);
                this.cboConstrHierLevel.DragDrop -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragDrop);
                this.cboConstrHierLevel.DragEnter -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragEnter);
                this.cboConstrHierLevel.DragOver -= new System.Windows.Forms.DragEventHandler(this.comboHierLevel_DragOver);
                //this.cboConstrHierLevel.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.comboBoxHierLevel_KeyDown);
                this.cboConstrHierLevel.Validating -= new System.ComponentModel.CancelEventHandler(this.comboBoxHierLevel_Validating);
                this.cboConstrHierLevel.Validated -= new System.EventHandler(this.comboBoxHierLevel_Validated);
                this.cbConstrColor.CheckedChanged -= new System.EventHandler(this.cbColor_CheckedChanged);
                this.cboInventoryBasis.SelectionChangeCommitted -= new System.EventHandler(this.cboInventoryBasis_SelectionChangeCommitted);
                this.cboInventoryBasis.DragDrop -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragDrop);
                this.cboInventoryBasis.DragEnter -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragEnter);
                this.cboInventoryBasis.DragOver -= new System.Windows.Forms.DragEventHandler(this.cboInventoryBasis_DragOver);
                this.cboInventoryBasis.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.cboInventoryBasis_KeyDown);
                this.cboInventoryBasis.Validating -= new System.ComponentModel.CancelEventHandler(this.cboInventoryBasis_Validating);
                this.cboInventoryBasis.Validated -= new System.EventHandler(this.cboInventoryBasis_Validated);
                this.picBoxGroup.Click -= new System.EventHandler(this.picBoxFilter_Click);
                this.picBoxGroup.MouseHover -= new System.EventHandler(this.picBoxFilter_MouseHover);
                this.picBoxAlternate.Click -= new System.EventHandler(this.picBoxFilter_Click);
                this.picBoxAlternate.MouseHover -= new System.EventHandler(this.picBoxFilter_MouseHover);
                this.radNormalizeSizeCurves_No.CheckedChanged -= new System.EventHandler(this.radNormalizeSizeCurves_No_CheckedChanged);
                this.radNormalizeSizeCurves_Yes.CheckedChanged -= new System.EventHandler(this.radNormalizeSizeCurves_Yes_CheckedChanged);
                this.cbxOverrideNormalizeDefault.CheckedChanged -= new System.EventHandler(this.cbxOverrideNormalizeDefault_CheckedChanged);

                this.cboSizeGroup.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSizeGroup_MIDComboBoxPropertiesChangedEvent);
                this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                this.cboSizeCurve.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboSizeCurve_MIDComboBoxPropertiesChangedEvent);
                this.cboAlternates.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboAlternates_MIDComboBoxPropertiesChangedEvent);
                this.cboConstraints.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboConstraints_MIDComboBoxPropertiesChangedEvent);
                this.cboHeaderChar.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboHeaderChar_MIDComboBoxPropertiesChangedEvent);
                this.cboHierarchyLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboNameExtension.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboNameExtension_MIDComboBoxPropertiesChangedEvent);
                this.cboConstrHeaderChar.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboConstrHeaderChar_MIDComboBoxPropertiesChangedEvent);
                this.cboConstrHierLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboConstrHierLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboInventoryBasis.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboInventoryBasis_MIDComboBoxPropertiesChangedEvent);

                this.Closing -= new System.ComponentModel.CancelEventHandler(this.SizeMethodsFormBase_Closing);
                // End TT#301-MD - JSmith - Controls are not functioning properly

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

		// Begin A&F Generic Size Curve
		// ANF Generic Size Constraints - made certain events more generic to handle multiple controls
		private void comboBoxHierLevel_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				// Begin MID Track #4956 - JSmith - Merchandise error (handle wheel mouse and arrow keys)
				if (e.KeyCode == Keys.Up ||
					e.KeyCode == Keys.Down)
				{
					return;
				}
				// End MID Track #4956

                ComboBox comboBox = (ComboBox)sender;
				_textChanged = true;

				if (_lastMerchIndex == -1)
				{
					_lastMerchIndex = comboBox.SelectedIndex;
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "cboHierarchyLevel_KeyDown");
			}
		}
	
		private void comboBoxHierLevel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			string errorMessage;

			try
			{
                ComboBox comboBox = (ComboBox)sender;
				if (comboBox.Text == string.Empty)
				{
					comboBox.SelectedIndex = 0;
				}
				else
				{
					if (_textChanged)
					{
						_textChanged = false;

						HierarchyNodeProfile hnp = GetNodeProfile(comboBox.Text);
						if (hnp.Key == Include.NoRID)
						{
							_priorError = true;

							errorMessage = string.Format(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidTreeNode), cboHierarchyLevel.Text);
							ErrorProvider.SetError(comboBox, errorMessage);
							MessageBox.Show(errorMessage);

							e.Cancel = true;
						}
						else 
						{
							if (comboBox.Name == "cboHierarchyLevel")
							{
								AddNodeToGenSizeCurveCombo(hnp);
							}
							else
							{
								AddNodeToGenSizeConstraintCombo(hnp);
							}
						}	
					}
					else if (_priorError)
					{
						comboBox.SelectedIndex = _lastMerchIndex;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, " comboBoxHierLevel_Validating");
			}
		}

		private void comboBoxHierLevel_Validated(object sender, System.EventArgs e)
		{
			try
			{
				ComboBox comboBox = (ComboBox)sender;
				ErrorProvider.SetError(comboBox, string.Empty);
				_textChanged = false;
				_priorError = false;
				_lastMerchIndex = -1;
			}
			catch (Exception)
			{
				throw;
			}
		}

		private HierarchyNodeProfile GetNodeProfile(string aProductID)
		{
			string productID;
			string[] pArray;

			try
			{
				productID = aProductID.Trim();
				pArray = productID.Split(new char[] {'['});
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

        // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        private void comboHierLevel_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            //Merchandise_DragEnter(sender, e);
            //try
            //{
            //    ObjectDragEnter(e);
            //}
            //catch (Exception ex)
            //{
            //    HandleException(ex);
            //}
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

        void comboHierLevel_DragOver(object sender, DragEventArgs e)
        {
            // Begin TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
            Merchandise_DragOver(sender, e);
            //Image_DragOver(sender, e);
            // End TT#296 - JSmith - Methods and Workflows can be dragged and dropped into Inventory Basis drop down.
        }

		private void comboHierLevel_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList = null;
			try
			{
                //ClipboardProfile cbp = GetClipboardData(eClipboardDataType.HierarchyNode);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    //Begin Track #5378 - color and size not qualified
                    //				HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbp.Key);
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(cbList.ClipboardProfile.Key, true, true);
                    //End Track #5378

                    ComboBox comboBox = ((MIDComboBoxEnh)sender).ComboBox;
                    if (comboBox.Name == "cboHierarchyLevel")
                    {
                        AddNodeToGenSizeCurveCombo(hnp);
                    }
                    else
                    {
                        AddNodeToGenSizeConstraintCombo(hnp);
                    }
                }
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		public void AddNodeToGenSizeCurveCombo(HierarchyNodeProfile hnp )
		{
			try
			{
				DataRow myDataRow;
				bool nodeFound = false;
				int nodeRID = Include.NoRID;
				int levIndex;
				for (levIndex = 0; levIndex < _dtCurveHierLevel.Rows.Count; levIndex++)
				{	
					myDataRow = _dtCurveHierLevel.Rows[levIndex];
					if (myDataRow["leveltypename"] != DBNull.Value &&
						(eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
					{
						nodeRID = (Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture));
						if (hnp.Key == nodeRID)
						{
							nodeFound = true;
							break;
						}
					}
				}
				if (!nodeFound)
				{
					myDataRow = _dtCurveHierLevel.NewRow();
					myDataRow["seqno"] = _dtCurveHierLevel.Rows.Count;
					myDataRow["leveltypename"] = eMerchandiseType.Node;
					myDataRow["text"] = hnp.Text;	
					myDataRow["key"] = hnp.Key;
					_dtCurveHierLevel.Rows.Add(myDataRow);

					cboHierarchyLevel.SelectedIndex = _dtCurveHierLevel.Rows.Count - 1;
				}	
				else
				{
					cboHierarchyLevel.SelectedIndex = levIndex;
				}
				
				if (FormLoaded)
				{
					ChangePending = true; 
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		} 
		
		public void SetGenSizeCurveComboToLevel(int seq)
		{
			try
			{
				DataRow myDataRow;
				for (int levIndex = 0; levIndex < _dtCurveHierLevel.Rows.Count; levIndex++)
				{	
					myDataRow = _dtCurveHierLevel.Rows[levIndex];
					if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
					{
						cboHierarchyLevel.SelectedValue = levIndex;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}

		// BEGIN ANF Generic Size Constraint
		public void AddNodeToGenSizeConstraintCombo(HierarchyNodeProfile hnp )
		{
			try
			{
				DataRow myDataRow;
				bool nodeFound = false;
				int nodeRID = Include.NoRID;
				int levIndex;
				for (levIndex = 0; levIndex < _dtConstraintHierLevel.Rows.Count; levIndex++)
				{	
					myDataRow = _dtConstraintHierLevel.Rows[levIndex];
					if (myDataRow["leveltypename"] != DBNull.Value &&
						(eMerchandiseType)(Convert.ToInt32(myDataRow["leveltypename"], CultureInfo.CurrentUICulture)) == eMerchandiseType.Node)
					{
						nodeRID = (Convert.ToInt32(myDataRow["key"], CultureInfo.CurrentUICulture));
						if (hnp.Key == nodeRID)
						{
							nodeFound = true;
							break;
						}
					}
				}
				if (!nodeFound)
				{
					myDataRow = _dtConstraintHierLevel.NewRow();
					myDataRow["seqno"] = _dtConstraintHierLevel.Rows.Count;
					myDataRow["leveltypename"] = eMerchandiseType.Node;
					myDataRow["text"] = hnp.Text;	
					myDataRow["key"] = hnp.Key;
					_dtConstraintHierLevel.Rows.Add(myDataRow);

					cboConstrHierLevel.SelectedIndex = _dtConstraintHierLevel.Rows.Count - 1;
				}	
				else
				{
					cboConstrHierLevel.SelectedIndex = levIndex;
				}
				
				if (FormLoaded)
				{
					ChangePending = true; 
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		} 
		
		public void SetGenSizeConstraintComboToLevel(int seq)
		{
			try
			{
				DataRow myDataRow;
				for (int levIndex = 0; levIndex < _dtConstraintHierLevel.Rows.Count; levIndex++)
				{	
					myDataRow = _dtConstraintHierLevel.Rows[levIndex];
					if (Convert.ToInt32(myDataRow["seqno"], CultureInfo.CurrentUICulture) == seq)
					{
						cboConstrHierLevel.SelectedValue = levIndex;
						break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		private void picBoxFilter_MouseHover(object sender, System.EventArgs e)
		{
			try
			{
				string message = MIDText.GetTextOnly((int)eMIDTextCode.tt_ClickToFilterDropDown);
				ToolTip.Active = true; 
				ToolTip.SetToolTip((System.Windows.Forms.Control)sender, message);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		public void picBoxFilter_Click(object sender, System.EventArgs e)
		{
			try
			{	
				string enteredMask = string.Empty;
				bool caseSensitive = false;
				PictureBox picBox = (PictureBox)sender;

				//Begin TT#155 - JScott - Size Curve Method
				//if (CharMaskFromDialogOK(picBox, ref enteredMask, ref caseSensitive))
				if (CharMaskFromDialogOK(Convert.ToString(picBox.Tag), ref enteredMask, ref caseSensitive))
				//End TT#155 - JScott - Size Curve Method
				{
					//MessageBox.Show("Filter selection process not yet available");
					switch (picBox.Name)
					{
						case "picBoxGroup":
							BindSizeGroupComboBox(true, enteredMask, caseSensitive);
							break;
						
						case "picBoxAlternate":
							BindSizeAlternatesComboBox(enteredMask, caseSensitive);
							break;
						
						case "picBoxCurve":
							BindSizeCurveComboBox(true, enteredMask, caseSensitive);
							break;

						case "picBoxConstraint":
							BindSizeConstraintsComboBox(enteredMask, caseSensitive);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

		//Begin TT#155 - JScott - Size Curve Method
		//private bool CharMaskFromDialogOK(PictureBox aPicBox, ref string aEnteredMask, ref bool aCaseSensitive)
		protected bool CharMaskFromDialogOK(string aGlobalMask, ref string aEnteredMask, ref bool aCaseSensitive)
		//End TT#155 - JScott - Size Curve Method
		{
			bool maskOK = false;
			string errMessage = string.Empty;
                       
			try
			{
				bool cancelAction = false;
				string dialogLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelection);
				string textLabel = MIDText.GetTextOnly((int)eMIDTextCode.lbl_FilterSelectionText);

				//Begin TT#155 - JScott - Size Curve Method
				//string globalMask = Convert.ToString(aPicBox.Tag, CultureInfo.CurrentUICulture);

				//NameDialog nameDialog = new NameDialog(dialogLabel, textLabel, globalMask);
				NameDialog nameDialog = new NameDialog(dialogLabel, textLabel, aGlobalMask);
				//End TT#155 - JScott - Size Curve Method
				nameDialog.AllowCaseSensitive();
     
				while (!(maskOK || cancelAction))
				{
					nameDialog.StartPosition = FormStartPosition.CenterParent;
					nameDialog.TreatEmptyAsCancel = false;
//					Point pt = new Point();
//
//					pt.X = 200;
// 					pt.Y = this.
//					nameDialog.Location = aPicBox.Container.;
					DialogResult dialogResult = nameDialog.ShowDialog();

					if (dialogResult == DialogResult.Cancel)
						cancelAction = true;
					else
					{
						maskOK = false;
						aEnteredMask = nameDialog.TextValue.Trim(); 
						aCaseSensitive = nameDialog.CaseSensitive;
						// append mask with trailing asterisk
						if (!aEnteredMask.EndsWith("*"))
						{
							aEnteredMask += "*";
						}
						//Begin TT#155 - JScott - Size Curve Method
						//maskOK = (globalMask == string.Empty) ? true : EnteredMaskOK(aPicBox, aEnteredMask, globalMask);
						maskOK = (aGlobalMask == string.Empty) ? true : EnteredMaskOK(aEnteredMask, aGlobalMask);
						//End TT#155 - JScott - Size Curve Method
						
						if (!maskOK)
						{
							errMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_FilterGlobalOptionMismatch);
							MessageBox.Show(errMessage, dialogLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}

				if (cancelAction)
				{
					maskOK = false;
				}
				else
				{
					nameDialog.Dispose();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
			return maskOK;
		}

		//Begin TT#155 - JScott - Size Curve Method
		//private bool EnteredMaskOK(PictureBox aPicBox, string aEnteredMask, string aGlobalMask)
		private bool EnteredMaskOK(string aEnteredMask, string aGlobalMask)
		//End TT#155 - JScott - Size Curve Method
		{
			bool maskOK = true;
			try
			{
                //bool okToContinue = true;
				int gXCount = 0;
				int eXCount = 0;
				int gWCount = 0;
				int eWCount = 0;
				char wildCard = '*';
				//char xChar = 'X';
				char[] cGlobalArray = aGlobalMask.ToCharArray(0, aGlobalMask.Length);
				for (int i = 0; i < cGlobalArray.Length; i++)
				{
					if (cGlobalArray[i] == wildCard)
					{
						gWCount++;
					}
					else
					{
						gXCount++;
					}
				}
				char[] cEnteredArray = aEnteredMask.ToCharArray(0, aEnteredMask.Length);
				for (int i = 0; i < cEnteredArray.Length; i++)
				{
					if (cEnteredArray[i] == wildCard)
					{
						eWCount++;
					}
					else
					{
						eXCount++;
					}
				}

				if (eXCount < gXCount)
				{
					maskOK = false;
				}
				else if (eXCount > gXCount && gWCount == 0)
				{  
					maskOK = false;
				}
				else if (aEnteredMask.Length < aGlobalMask.Length && !aGlobalMask.EndsWith("*"))	
				{  
					maskOK = false;
				}
				string[] globalParts = aGlobalMask.Split(new char[] {'*'});
				string[] enteredParts = aEnteredMask.Split(new char[] {'*'});
				int gLastEntry = globalParts.Length - 1;
				int eLastEntry = enteredParts.Length - 1;
				if (enteredParts[0].Length < globalParts[0].Length)
				{
					maskOK = false;
				}
				else if (enteredParts[eLastEntry].Length < globalParts[gLastEntry].Length)
				{
					maskOK = false;
				}
//				Regex regex = new Regex(aGlobalMask,RegexOptions.IgnoreCase);
//				if (!regex.IsMatch(aEnteredMask))
//				{
//					maskOK = false;
//				}

//				if (maskOK)
//				{
//					int pos = 0;
//					// validate considering optional chars
//					while(okToContinue && pos < aEnteredMask.Length && pos < aGlobalMask.Length) 
//					{ 
//						switch (cGlobalArray[pos])
//						{
//							case 'X':
//								if (aEnteredMask[pos] == wildCard) 
//								{
//									okToContinue = false;
//								}
//								break;
//
//							case '*':
//								break;
//						}
//						if (!okToContinue)	
//						{
//							maskOK = false;
//							break;
//						}
//						else
//						{
//							pos++;
//						}
//					}
//				} 
			}
			catch
			{
				throw;
			}
			return maskOK;
		}

		protected void SetMaskedComboBoxesEnabled()
		{
			if (SAB.ApplicationServerSession.GlobalOptions.SizeGroupCharMask != string.Empty)
			{
				this.cboSizeGroup.Enabled = false;
			}
			if (SAB.ApplicationServerSession.GlobalOptions.SizeAlternateCharMask != string.Empty)
			{
				this.cboAlternates.Enabled = false;
			}
			if (SAB.ApplicationServerSession.GlobalOptions.SizeCurveCharMask != string.Empty)
			{
				this.cboSizeCurve.Enabled = false;
			}
			if (SAB.ApplicationServerSession.GlobalOptions.SizeConstraintCharMask != string.Empty)
			{
				this.cboConstraints.Enabled = false;
			}

		}	
		// END ANF Generic Size Constraint

		private void cbColor_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        private void cboHeaderChar_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true; 
			}
		}

        private void cboHierarchyLevel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded &&
				!FormIsClosing)
			{
				ErrorProvider.SetError(cboHierarchyLevel, string.Empty);
				_lastMerchIndex = cboHierarchyLevel.SelectedIndex;
				ChangePending = true;
                _textChanged = false;
			}
		}

        // Begin TT#413 - RMatelic - Add new Generic Size Curve names to correspond with the new Size Curves being generated within Node Properties settings
        private void cboNameExtension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                ChangePending = true;
            }
        }
        // End TT#413  
		// End A&F Generic Size Curve

        // Begin TT#498 - RMatelic - Size Need Method Default checked on the Rule Tab the Add only goes down to Color. Rewrite of this
        private void cbxUseDefaultcurve_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded && cbxUseDefaultcurve.Checked)
                {
                    if (Convert.ToInt32(cboSizeCurve.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID ||
                        Convert.ToInt32(cboHierarchyLevel.SelectedValue, CultureInfo.CurrentUICulture) != -2 ||
                        Convert.ToInt32(cboNameExtension.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID ||
                        Convert.ToInt32(cboHeaderChar.SelectedValue, CultureInfo.CurrentUICulture) != Include.NoRID)
                    {
                        if (RuleExists() || BasisSubstituteExists())
                        {
                            bool isBasisMethod =  ABM.MethodType == eMethodType.BasisSizeAllocation ? true : false ;
                            if (ShowWarningPrompt(isBasisMethod) == DialogResult.Yes)
                            {
                                UpdateRules();
                            }
                            else
                            {
                                cbxUseDefaultcurve.Checked = false;
                                SetPromptSwitch();
                            }
                        }
                        else
                        {
                            ClearSizeCurveGroup();
                        }
                    }
                    
                }
                cboSizeCurve.Enabled = !cbxUseDefaultcurve.Checked;
                picBoxCurve.Enabled = !cbxUseDefaultcurve.Checked;
                gbGenericSizeCurve.Enabled = !cbxUseDefaultcurve.Checked;
            }
            catch (Exception ex)
            {
                HandleException(ex, "cbxUseDefaultcurve_CheckedChanged");
            }
        }

        private void UpdateRules()
        {
            try
            {
                ClearSizeCurveGroup();
                
                switch (ABM.MethodType)
                {
                    case eMethodType.FillSizeHolesAllocation:
                        FillSizeHolesMethod fsm = (FillSizeHolesMethod)ABM;
                        fsm.DeleteMethodRules(new TransactionData());
                        fsm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        fsm.SizeCurveGroupRid = Include.NoRID;
                        fsm.GetSizesUsing = eGetSizes.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                        fsm.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID; // MID Track 3781 Size Curve Not Required in Fill Size Holes
                        fsm.CreateConstraintData();
                        BindAllSizeGrid(fsm.MethodConstraints);
                        CheckExpandAll();
                        fsm.PromptSizeChange = true;
                        break;

                    case eMethodType.BasisSizeAllocation:
                        BasisSizeAllocationMethod bsm = (BasisSizeAllocationMethod)ABM;
                        bsm.DeleteMethodRules(new TransactionData());
                        bsm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        bsm.SizeCurveGroupRid = Include.NoRID;
                        bsm.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                        bsm.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                        bsm.CreateConstraintData();
                        BindAllSizeGrid(bsm.MethodConstraints);
                        MIDRetail.Windows.frmBasisSizeMethod bsmw = (MIDRetail.Windows.frmBasisSizeMethod)this;
                        bsmw.BindSubstitutesGrid(bsm.SizeCurveGroupRid, eGetSizes.SizeCurveGroupRID);
                        bsmw.SetSubsituteGridMessage();
                        CheckExpandAll();
                        bsm.PromptSizeChange = true;
                        break;

                    case eMethodType.SizeNeedAllocation:
                        SizeNeedMethod snm = (SizeNeedMethod)ABM;
                        snm.DeleteMethodRules(new TransactionData());
                        snm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        snm.SizeCurveGroupRid = Include.NoRID;
                        snm.GetSizesUsing = eGetSizes.SizeCurveGroupRID;
                        snm.GetDimensionsUsing = eGetDimensions.SizeCurveGroupRID;
                        snm.CreateConstraintData();
                        BindAllSizeGrid(snm.MethodConstraints);
                        CheckExpandAll();
                        snm.PromptSizeChange = true;
                        break;
                }
            }
            catch
            {
                throw;
            }   
        }

        private void SetPromptSwitch()
        {
            try
            {
                switch (ABM.MethodType)
                {
                    case eMethodType.FillSizeHolesAllocation:
                        FillSizeHolesMethod fsm = (FillSizeHolesMethod)ABM;
                        fsm.PromptSizeChange = true;
                        break;
                    case eMethodType.BasisSizeAllocation:
                        BasisSizeAllocationMethod bsm = (BasisSizeAllocationMethod)ABM;
                        bsm.PromptSizeChange = true;
                        break;
                    case eMethodType.SizeNeedAllocation:
                        SizeNeedMethod snm = (SizeNeedMethod)ABM;
                        snm.PromptSizeChange = true;
                        break;
                }
            }
            catch
            {
                throw;
            }
        }    

        private void ClearSizeCurveGroup()
        {
            try
            {
                switch (ABM.MethodType)
                {
                    case eMethodType.FillSizeHolesAllocation:
                        FillSizeHolesMethod fsm = (FillSizeHolesMethod)ABM;
                        fsm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        fsm.PromptSizeChange = true;
                        break;
                    case eMethodType.BasisSizeAllocation:
                        BasisSizeAllocationMethod bsm = (BasisSizeAllocationMethod)ABM;
                        bsm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        bsm.PromptSizeChange = true;
                        break;
                    case eMethodType.SizeNeedAllocation:
                        SizeNeedMethod snm = (SizeNeedMethod)ABM;
                        snm.PromptSizeChange = false;
                        cboSizeCurve.SelectedValue = Include.NoRID;
                        snm.PromptSizeChange = true;
                        break;
                }
                cboHierarchyLevel.SelectedValue = -2;
                cboNameExtension.SelectedValue = Include.NoRID;
                cboHeaderChar.SelectedValue = Include.NoRID;
                cbColor.Checked = false;
                ChangePending = true;
                ErrorMessages.Clear();
                AttachErrors(gbSizeCurve);
            }
            catch
            {
                throw;
            }
        }    
       
        public bool RuleExists()
		{
            bool ruleExists = false;
			try
			{
                foreach (UltraGridRow row in ugRules.Rows)
                {
                    if (RuleExistForRow(row))
                    {
                        ruleExists = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "RuleExists method");
            }
            return ruleExists;
        }

        private bool RuleExistForRow(UltraGridRow aRow)
        {
            bool ruleExists = false;
            try
            {
                if (aRow.Band.Key.ToUpper() == C_SET_CLR || aRow.Band.Key.ToUpper() == C_ALL_CLR_SZ_DIM.ToUpper() ||
                    aRow.Band.Key.ToUpper() == C_ALL_CLR_SZ || aRow.Band.Key.ToUpper() == C_CLR_SZ_DIM || aRow.Band.Key.ToUpper() == C_CLR_SZ)
                {
                    ruleExists = true;
                }
			    else if (aRow.Cells["SIZE_RULE"].Value != DBNull.Value)
                {
                    ruleExists = true;
                }
                else
                {
                    if (aRow.HasChild(false))
                    {
                        UltraGridRow cRow = aRow.GetChild(ChildRow.First);
                        while (cRow != null && !ruleExists)
                        {
                            ruleExists = RuleExistForRow(cRow);
                            cRow = cRow.GetSibling(SiblingRow.Next, true, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "RulesExistForRow method");
            }
            return ruleExists;
        }

        public bool BasisSubstituteExists()
        {
            bool basisSubstituteExists = false;
            try
            {
                if (ABM.MethodType == eMethodType.BasisSizeAllocation)
                {
                    BasisSizeAllocationMethod bsm = (BasisSizeAllocationMethod)ABM;
                    if (bsm.SubstituteList.Count > 0)
                    {
                        basisSubstituteExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "RuleExists method");
            }
            return basisSubstituteExists;
        }
        // End TT#498/TT#499
         #endregion

        /// <summary>
		/// walks through the grid and sets the quantity fields to editable or disabled
		/// depending upon the rule value.
		/// </summary>
		public void SetEditableQuantityCells()
		{
			try
			{
				//================================================================================
				//WALK THE GRID - Checking EACH ROW
				//================================================================================
				foreach(UltraGridRow setRow in ugRules.Rows)
				{
					SetQuantityActivation(setRow);

					if (setRow.HasChild())
					{
						//ALL COLORS ROW
						//===============
						foreach(UltraGridRow allColorRow in setRow.ChildBands[C_SET_ALL_CLR].Rows)
						{
							SetQuantityActivation(allColorRow);

							if (allColorRow.HasChild())
							{
								//ALL COLOR DIMENSION ROWS
								foreach (UltraGridRow allColorDimRow in allColorRow.ChildBands[C_ALL_CLR_SZ_DIM].Rows)
								{
									SetQuantityActivation(allColorDimRow);

									//ALL COLOR DIMENSION/SIZE ROWS
									if (allColorDimRow.HasChild())
									{
										foreach (UltraGridRow allColorSizeRow in allColorDimRow.ChildBands[C_ALL_CLR_SZ].Rows)
										{
											SetQuantityActivation(allColorSizeRow);
										}	
									}
								}
							}
						}
						//===========
						//COLOR ROWS 
						//===========
						foreach(UltraGridRow colorRow in setRow.ChildBands[C_SET_CLR].Rows)
						{
							SetQuantityActivation(colorRow);

							if (colorRow.HasChild())
							{
								//COLOR SIZE
								//=============
								foreach (UltraGridRow colorDimRow in colorRow.ChildBands[C_CLR_SZ_DIM].Rows)
								{
									SetQuantityActivation(colorDimRow);

									if (colorDimRow.HasChild())
									{
										//COLOR SIZE DIMENSION
										//======================
										foreach (UltraGridRow colorSizeRow in colorDimRow.ChildBands[C_CLR_SZ].Rows)
										{
											SetQuantityActivation(colorSizeRow);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				HandleException(ex, "SetEditableQuantityCells");
			}
		}

		public void SetQuantityActivation(UltraGridRow aRow)
		{			
			if (aRow.Cells["SIZE_RULE"].Value == DBNull.Value)
			{
				aRow.Cells["SIZE_QUANTITY"].Activation = Activation.NoEdit;
			}
			else
			{
				eSizeRuleType ruleType = ruleType = (eSizeRuleType)Convert.ToInt32(aRow.Cells["SIZE_RULE"].Value,CultureInfo.CurrentUICulture); // MID Track 3619 Remove Fringe
				if (aRow.Cells["SIZE_QUANTITY"].Hidden == false)
				{
                    if (ruleType == eSizeRuleType.AbsoluteQuantity) // MID Track 3619 Remove Fringe
                        //aRow.Cells["SIZE_QUANTITY"].Activation = Activation.AllowEdit;
                        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                        if (aRow.Cells["SIZE_QUANTITY"].Activation != Activation.AllowEdit)
                        {
                            aRow.Cells["SIZE_QUANTITY"].Activation = Activation.AllowEdit;
                        }
                        else
                        {
                        }
                        //End TT#169
                    else
                    {
                        aRow.Cells["SIZE_QUANTITY"].Value = DBNull.Value;
                        aRow.Cells["SIZE_QUANTITY"].Activation = Activation.NoEdit;
                    }
				}
			}
		}

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

		private void ugRules_SelectionDrag(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (ugRules.Selected.Cells.Count > 0)
				{
					ugRules.DoDragDrop(ugRules.Selected.Cells, DragDropEffects.Move);
					return;
				}
			
				if (ugRules.Selected.Rows.Count > 0)
				{
					ugRules.DoDragDrop(ugRules.Selected.Rows, DragDropEffects.Move);
					return;
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugRules_SelectionDrag");
			}
		}

        //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
        void ugRules_AfterCellListCloseUp(object sender, CellEventArgs e)
        {
            if (ugRules.ActiveCell != null)
            {
                if (ugRules.ActiveCell.Column.Key == "SIZE_RULE")
                {
                    ugRules.UpdateData();
                    SetQuantityActivation(ugRules.ActiveCell.Row);
                }
            }
        }
        //End TT#169

		private void ugRules_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			ChangePending = true;
			try
			{
				if (ugRules.ActiveCell != null)
				{
					if (ugRules.ActiveCell.Column.Key == "SIZE_RULE")
					{
						SetQuantityActivation(ugRules.ActiveCell.Row);
					}
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugRules_AfterCellUpdate");
			}
		}

		private void ugRules_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			ChangePending = true;
		}

		private void ugRules_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
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
							ugRules.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
							break;
						default:
						switch (gridCell.Column.DataType.Name.ToUpper())
						{
							case "BOOLEAN":
								ugRules.DisplayLayout.Override.CellClickAction = CellClickAction.Default;
								break;
							default:
								ugRules.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
								break;
						}
							break;
					}
				}


				ShowUltraGridToolTip(ugRules, e);
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugRules_MouseEnterElement");
			}
		}

		private void ugRules_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = e;
		}

		private void ugRules_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			_gridKeyEvent = null;
		}

		private void ugRules_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				Point point = new Point(e.X, e.Y);
				Infragistics.Win.UIElement mouseUIElement;
				Infragistics.Win.UIElement headerUIElement;
				Infragistics.Win.UltraWinGrid.UltraGridCell mouseCell;
				Infragistics.Win.UltraWinGrid.RowSelectorUIElement selectorUIElement;

				// RETRIEVE THE UIELEMENT FROM THE LOCATION OF THE MOUSE.
				mouseUIElement = ugRules.DisplayLayout.UIElement.ElementFromPoint(point);


				if (e.Button == MouseButtons.Right)
				{
					ugRules.ContextMenu = null;
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
						ugRules.ActiveRow = selectorUIElement.Row;
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
							ugRules.ActiveCell = mouseCell;
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
				HandleException(ex, "ugRules_MouseDown");
			}
		}

		private void ugRules_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			try
			{
				if (_gridKeyEvent == null || !_gridKeyEvent.Control)
				{
					// BEGIN Issue #### - abend scrolling
					Infragistics.Win.UIElement aUIElement;

					// BEGIN MID Track #4375 - Quantity column not enterable
					//aUIElement = ugRules.DisplayLayout.UIElement.ElementFromPoint(ugRules.PointToClient(new Point(e.X, e.Y)));
					aUIElement = ugRules.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
					// END MID Track #4375 

					if (aUIElement == null) 
					{
						return;
					}

					UltraGridRow aRow;
					aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow)); 

					if (aRow == null) 
					{
						return;
					}

					UltraGridCell aCell = (UltraGridCell)aUIElement.GetContext(typeof(UltraGridCell)); 
					if (aCell == null) 
					{
						return;
					}
					// END Issue #### - abend scrolling

					ugRules.PerformAction(UltraGridAction.EnterEditMode);
				}
			}
			catch( Exception ex )
			{
				HandleException(ex, "ugRules_MouseUp");
			}
		}

        virtual protected void ugRules_AfterRowActivate(object sender, EventArgs e)
        {
            
        }

		private void ugRules_AfterRowUpdate(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
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
				HandleException(ex, "ugAllSize_AfterRowUpdate");
			}
		}

		private void ugRules_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			try
			{
				UltraGrid myGrid = (UltraGrid)sender;
				UltraGridRow activeRow = myGrid.ActiveRow;

				if (activeRow.IsAddRow)
				{
					activeRow.Update();
					ugRules.ActiveRow = activeRow;
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
				HandleException(ex, "ugAllSize_BeforeRowInsert");
			}
		}

		private void ugRules_BeforeCellDeactivate(object sender, System.ComponentModel.CancelEventArgs e)
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
				HandleException(ex, "ugAllSize_BeforeCellDeactivate");
			}
		}

		private void ugRules_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			try
			{
				bool IsValid = true;

				IsValid = VerifyBeforeInsert(((UltraGrid)sender).ActiveRow);

				if (!VerifyBeforeInsert(((UltraGrid)sender).ActiveRow))
				{
					e.Cancel = true;
				}

			}
			catch( Exception ex )
			{
				e.Cancel = true;
				HandleException(ex, "ugAllSize_BeforeRowUpdate");
			}
		}

		private void ugRules_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			try
			{
				ChangePending = true;
				bool hasValues;
				UltraGrid myGrid = (UltraGrid)sender;
					
				switch (e.Row.Band.Key.ToUpper())
				{
					case C_CLR_SZ:
					case C_ALL_CLR_SZ:
						hasValues = CreateSizeCellList(e.Row,
							e.Row.Band, 
							myGrid.DisplayLayout.ValueLists["SizeCell"]); 
											
						if (!hasValues)
						{
							MessageBox.Show("All available sizes are being used for this dimension.\nThis row will be removed.",
								"MIDRetail",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:

						hasValues = CreateDimensionCellList(e.Row,
							e.Row.Band,
							myGrid.DisplayLayout.ValueLists["DimensionCell"]);
							
						if (!hasValues)
						{
							MessageBox.Show("All available dimensions are being used for this color.\nThis row will be removed.",
								"MIDRetail",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
					case C_SET_CLR:

						hasValues = CreateColorCellList(e.Row,
							e.Row.Band,
							myGrid.DisplayLayout.ValueLists["ColorCell"]);
							
						if (!hasValues)
						{
							MessageBox.Show("All available colors are being used for this set.\nThis row will be removed.",
								"MIDRetail",
								MessageBoxButtons.OK);
							e.Row.Delete(false);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				e.Row.Delete(false);
				HandleException(ex, "ugRules_AfterRowInsert");
			}
		}

		private void ugRules_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
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
							CreateSizeCellList(myRow, 
								myGrid.ActiveCell.Band, 
								myGrid.DisplayLayout.ValueLists["SizeCell"]);  
			
							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["SizeCell"]; 
						}
						break;
					case C_CLR_SZ_DIM:
					case C_ALL_CLR_SZ_DIM:
						if (myGrid.ActiveCell.Column.Key == "DIMENSIONS_RID")
						{
							CreateDimensionCellList(myRow, 
								myGrid.ActiveCell.Band, 
								myGrid.DisplayLayout.ValueLists["DimensionCell"]);
			
							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["DimensionCell"];
						}
						break;
					case C_SET_CLR:
						if (myGrid.ActiveCell.Column.Key == "COLOR_CODE_RID")
						{
							CreateColorCellList(myRow, 
								myGrid.ActiveCell.Band, 
								myGrid.DisplayLayout.ValueLists["ColorCell"]);
			
							myGrid.ActiveCell.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList;
							myGrid.ActiveCell.ValueList = myGrid.DisplayLayout.ValueLists["ColorCell"];
						}
						break;
				}
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				HandleException(ex, "ugRules_BeforeEnterEditMode");
			}
		}

		public void CheckExpandAll()
		{
			if (cbExpandAll.Checked)
				ugRules.Rows.ExpandAll(true);
			else
				ugRules.Rows.CollapseAll(true);
		}

		// BEGIN MID Track #4375 - Quantity column not enterable
		private void ugRules_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			try
			{
				SetQuantityActivation(e.Row);
			}
			catch (Exception ex)
			{
				HandleException(ex, "ugRules_InitializeRow");
			}
		}
		// END MID Track #4375

		// BEGIN MID Track #4826 - JSmith - Normalize Size Curves
		private void cbxOverrideNormalizeDefault_CheckedChanged(object sender, System.EventArgs e)
		{
			if (cbxOverrideNormalizeDefault.Checked)
			{
				this.radNormalizeSizeCurves_No.Enabled = true;
				this.radNormalizeSizeCurves_Yes.Enabled = true;
			}
			else
			{
				this.radNormalizeSizeCurves_No.Enabled = false;
				this.radNormalizeSizeCurves_Yes.Enabled = false;
				SetNormalizeSizeCurvesDefault();
			}

			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void SetNormalizeSizeCurvesDefault()
		{
			if (SAB.ClientServerSession.GlobalOptions.NormalizeSizeCurves)
			{
				radNormalizeSizeCurves_Yes.Checked = true;
			}
			else
			{
				radNormalizeSizeCurves_No.Checked = true;
			}
		}

		private void radNormalizeSizeCurves_Yes_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		private void radNormalizeSizeCurves_No_CheckedChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

        // Begin Track #4872 - JSmith - Global/User Attributes
        private void cboStoreAttribute_DragEnter(object sender, DragEventArgs e)
        {
            Image_DragEnter(sender, e);
        }

        private void cboStoreAttribute_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        virtual protected void cboStoreAttribute_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                bool isSuccessfull = ((MIDComboBoxTag)(((ComboBox)sender).Tag)).ComboBox_DragDrop(sender, e);

                if (isSuccessfull)
                {
                    //this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
                    ChangePending = true;
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }
        // End TT#301-MD - JSmith - Controls are not functioning properly

        override protected void BuildAttributeList()
        {
            ProfileList profileList;
            int currValue;
            bool setFormLoaded = false;
            try
            {
                // if FormLoaded, set to false so warning does not appear
                if (FormLoaded)
                {
                    FormLoaded = false;
                    setFormLoaded = true;
                }
                if (cboStoreAttribute.SelectedValue != null &&
                    cboStoreAttribute.SelectedValue.GetType() == typeof(System.Int32))
                {
                    currValue = Convert.ToInt32(cboStoreAttribute.SelectedValue);
                }
                else
                {
                    currValue = Include.NoRID;
                }
                profileList = GetStoreGroupList(_changeType, _globalUserType, false);
                cboStoreAttribute.Initialize(SAB, FunctionSecurity, profileList.ArrayList, _globalUserType == eGlobalUserType.User);
                if (currValue != Include.NoRID)
                {
                    cboStoreAttribute.SelectedValue = currValue;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (setFormLoaded)
                {
                    FormLoaded = true;
                }
            }
        }


        // Begin TT#301-MD - JSmith - Controls are not functioning properly
        virtual protected void cboFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
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

        virtual protected void cboStoreAttribute_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboSizeGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboAlternates_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboSizeCurve_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboHeaderChar_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboNameExtension_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboInventoryBasis_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboConstraints_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboConstrHeaderChar_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboConstrHierLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void cboInventoryBasis_DragEnter(object sender, DragEventArgs e)
        {
            Merchandise_DragEnter(sender, e);
        }

        private void cboInventoryBasis_DragOver(object sender, DragEventArgs e)
        {
            Merchandise_DragOver(sender, e);
        }

        virtual protected void cboInventoryBasis_DragDrop(object sender, DragEventArgs e)
        {

        }

        virtual protected void cboHierarchyLevel_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        virtual protected void cboInventoryBasis_KeyDown(object sender, KeyEventArgs e)
        {

        }

        virtual protected void cboInventoryBasis_Validating(object sender, CancelEventArgs e)
        {

        }

        virtual protected void cboInventoryBasis_Validated(object sender, EventArgs e)
        {

        }

        // End TT#301-MD - JSmith - Controls are not functioning properly
        // End Track #4872
		// END MID Track #4826

        private void cboSizeGroup_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSizeGroup_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboStoreAttribute_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboStoreAttribute_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboFilter_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboFilter_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboSizeCurve_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboSizeCurve_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboAlternates_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboAlternates_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboConstraints_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboConstraints_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboHeaderChar_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboHeaderChar_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboHierarchyLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboHierarchyLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboNameExtension_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboNameExtension_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboConstrHeaderChar_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboConstrHeaderChar_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboConstrHierLevel_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboConstrHierLevel_SelectionChangeCommitted(source, new EventArgs());
        }
        private void cboInventoryBasis_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboInventoryBasis_SelectionChangeCommitted(source, new EventArgs());
        }
	}
}
