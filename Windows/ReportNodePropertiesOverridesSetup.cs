using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.ViewerObjectModel;

using System.IO;

using System.Configuration;
using System.Text;
using System.Data; //TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	public class frmReportNodePropertiesOverridesSetup : ReportFormBase
	{
		#region Controls
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.Label lblLowLevel;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboLowLevels;
		private System.Windows.Forms.Label lblStore;
		private System.Windows.Forms.TextBox txtStoreNumber;
		private System.Windows.Forms.Label lblSet;
		private System.Windows.Forms.TextBox txtSet;
		private System.Windows.Forms.GroupBox grbInclude;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.CheckBox chkEligibility;
		private System.Windows.Forms.CheckBox chkStoreGrades;
		private System.Windows.Forms.CheckBox chkVelocityGrades;
		private System.Windows.Forms.CheckBox chkPurgeCriteria;
		private System.Windows.Forms.CheckBox chkModifiers;
		private System.Windows.Forms.CheckBox chkAllocationMinMax;
		private System.Windows.Forms.CheckBox chkCapacity;
		private System.Windows.Forms.CheckBox chkForecastLevel;
		private System.Windows.Forms.CheckBox chkSimilarStore;
		private System.Windows.Forms.CheckBox chkStockMinMax;
		private System.Windows.Forms.CheckBox chkDailyPercents;
		private System.Windows.Forms.CheckBox chkForecastType;
		private System.Windows.Forms.Label lblError;
		
		#endregion

		#region Member Vars
		private int _currentLowLevelNode = Include.NoRID;
		private int _longestHighestGuest = Include.NoRID;
		private int _longestBranch = Include.NoRID;
		private int _storeRid = 0;
		SessionAddressBlock _SAB;
        ProfileList _storeGroupList;
        private ContextMenuStrip cmsInclude;
        private ToolStripMenuItem cmsIncludeSelectAll;
        private ToolStripMenuItem cmsIncludeClearAll;
        private IContainer components;
        private CheckBox chkSizeCurveCriteria;
        private CheckBox chkVSW;
        private CheckBox chkChainSetPercent;
        private CheckBox chkCharacteristics;
        private MIDDateRangeSelector midDateRangeSelector1;
        private Label lblTimePeriod;
        private CheckBox chkSizeCurveSimilarStores;
        private CheckBox chkSizeCurveTolerance;        // TT#209 
        private string _storeRIDTextList;  // TT#265-Eligibility Report not showing low level nodes and formatting problems   
		#endregion

		#region Properties

		#endregion

		#region Constructors and Initialization Code


        public frmReportNodePropertiesOverridesSetup(SessionAddressBlock sab)
            : base(sab)
        {
            _SAB = sab;
            InitializeComponent();
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
            this.components = new System.ComponentModel.Container();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.lblLowLevel = new System.Windows.Forms.Label();
            this.cboLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lblStore = new System.Windows.Forms.Label();
            this.txtStoreNumber = new System.Windows.Forms.TextBox();
            this.lblSet = new System.Windows.Forms.Label();
            this.txtSet = new System.Windows.Forms.TextBox();
            this.grbInclude = new System.Windows.Forms.GroupBox();
            this.chkSizeCurveSimilarStores = new System.Windows.Forms.CheckBox();
            this.chkSizeCurveTolerance = new System.Windows.Forms.CheckBox();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.midDateRangeSelector1 = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.chkSizeCurveCriteria = new System.Windows.Forms.CheckBox();
            this.chkVSW = new System.Windows.Forms.CheckBox();
            this.chkChainSetPercent = new System.Windows.Forms.CheckBox();
            this.chkCharacteristics = new System.Windows.Forms.CheckBox();
            this.chkForecastType = new System.Windows.Forms.CheckBox();
            this.chkDailyPercents = new System.Windows.Forms.CheckBox();
            this.chkStockMinMax = new System.Windows.Forms.CheckBox();
            this.chkSimilarStore = new System.Windows.Forms.CheckBox();
            this.chkForecastLevel = new System.Windows.Forms.CheckBox();
            this.chkCapacity = new System.Windows.Forms.CheckBox();
            this.chkAllocationMinMax = new System.Windows.Forms.CheckBox();
            this.chkModifiers = new System.Windows.Forms.CheckBox();
            this.chkPurgeCriteria = new System.Windows.Forms.CheckBox();
            this.chkVelocityGrades = new System.Windows.Forms.CheckBox();
            this.chkStoreGrades = new System.Windows.Forms.CheckBox();
            this.chkEligibility = new System.Windows.Forms.CheckBox();
            this.cmsInclude = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsIncludeSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsIncludeClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.grbInclude.SuspendLayout();
            this.cmsInclude.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMerchandise.Location = new System.Drawing.Point(8, 11);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 0;
            this.lblMerchandise.Text = "Merchandise:";
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Location = new System.Drawing.Point(80, 8);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(256, 20);
            this.txtMerchandise.TabIndex = 1;
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
            // lblLowLevel
            // 
            this.lblLowLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowLevel.Location = new System.Drawing.Point(8, 43);
            this.lblLowLevel.Name = "lblLowLevel";
            this.lblLowLevel.Size = new System.Drawing.Size(64, 16);
            this.lblLowLevel.TabIndex = 2;
            this.lblLowLevel.Text = "Low Level:";
            // 
            // cboLowLevels
            // 
            this.cboLowLevels.AutoAdjust = true;
            this.cboLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLowLevels.DataSource = null;
            this.cboLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevels.DropDownWidth = 176;
            this.cboLowLevels.FormattingEnabled = false;
            this.cboLowLevels.IgnoreFocusLost = false;
            this.cboLowLevels.ItemHeight = 13;
            this.cboLowLevels.Location = new System.Drawing.Point(80, 40);
            this.cboLowLevels.Margin = new System.Windows.Forms.Padding(0);
            this.cboLowLevels.MaxDropDownItems = 25;
            this.cboLowLevels.Name = "cboLowLevels";
            this.cboLowLevels.Size = new System.Drawing.Size(176, 21);
            this.cboLowLevels.TabIndex = 3;
            this.cboLowLevels.Tag = null;
            // 
            // lblStore
            // 
            this.lblStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStore.Location = new System.Drawing.Point(11, 75);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(40, 16);
            this.lblStore.TabIndex = 4;
            this.lblStore.Text = "Store:";
            // 
            // txtStoreNumber
            // 
            this.txtStoreNumber.AllowDrop = true;
            this.txtStoreNumber.Location = new System.Drawing.Point(48, 72);
            this.txtStoreNumber.Name = "txtStoreNumber";
            this.txtStoreNumber.Size = new System.Drawing.Size(100, 20);
            this.txtStoreNumber.TabIndex = 5;
            this.txtStoreNumber.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtStoreNumber_DragDrop);
            this.txtStoreNumber.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtStoreNumber_DragEnter);
            this.txtStoreNumber.DragOver += new System.Windows.Forms.DragEventHandler(this.txtStoreNumber_DragOver);
            this.txtStoreNumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtStoreNumber_Validating);
            // 
            // lblSet
            // 
            this.lblSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSet.Location = new System.Drawing.Point(165, 75);
            this.lblSet.Name = "lblSet";
            this.lblSet.Size = new System.Drawing.Size(48, 16);
            this.lblSet.TabIndex = 6;
            this.lblSet.Text = "OR Set:";
            // 
            // txtSet
            // 
            this.txtSet.AllowDrop = true;
            this.txtSet.Location = new System.Drawing.Point(216, 72);
            this.txtSet.Name = "txtSet";
            this.txtSet.Size = new System.Drawing.Size(120, 20);
            this.txtSet.TabIndex = 7;
            this.txtSet.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtSet_DragDrop);
            this.txtSet.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtSet_DragEnter);
            this.txtSet.DragOver += new System.Windows.Forms.DragEventHandler(this.txtSet_DragOver);
            this.txtSet.Validating += new System.ComponentModel.CancelEventHandler(this.txtSet_Validating);
            // 
            // grbInclude
            // 
            this.grbInclude.Controls.Add(this.chkSizeCurveSimilarStores);
            this.grbInclude.Controls.Add(this.chkSizeCurveTolerance);
            this.grbInclude.Controls.Add(this.lblTimePeriod);
            this.grbInclude.Controls.Add(this.midDateRangeSelector1);
            this.grbInclude.Controls.Add(this.chkSizeCurveCriteria);
            this.grbInclude.Controls.Add(this.chkVSW);
            this.grbInclude.Controls.Add(this.chkChainSetPercent);
            this.grbInclude.Controls.Add(this.chkCharacteristics);
            this.grbInclude.Controls.Add(this.chkForecastType);
            this.grbInclude.Controls.Add(this.chkDailyPercents);
            this.grbInclude.Controls.Add(this.chkStockMinMax);
            this.grbInclude.Controls.Add(this.chkSimilarStore);
            this.grbInclude.Controls.Add(this.chkForecastLevel);
            this.grbInclude.Controls.Add(this.chkCapacity);
            this.grbInclude.Controls.Add(this.chkAllocationMinMax);
            this.grbInclude.Controls.Add(this.chkModifiers);
            this.grbInclude.Controls.Add(this.chkPurgeCriteria);
            this.grbInclude.Controls.Add(this.chkVelocityGrades);
            this.grbInclude.Controls.Add(this.chkStoreGrades);
            this.grbInclude.Controls.Add(this.chkEligibility);
            this.grbInclude.Location = new System.Drawing.Point(8, 112);
            this.grbInclude.Name = "grbInclude";
            this.grbInclude.Size = new System.Drawing.Size(472, 241);
            this.grbInclude.TabIndex = 8;
            this.grbInclude.TabStop = false;
            this.grbInclude.Text = "Include";
            // 
            // chkSizeCurveSimilarStores
            // 
            this.chkSizeCurveSimilarStores.Location = new System.Drawing.Point(313, 144);
            this.chkSizeCurveSimilarStores.Name = "chkSizeCurveSimilarStores";
            this.chkSizeCurveSimilarStores.Size = new System.Drawing.Size(153, 24);
            this.chkSizeCurveSimilarStores.TabIndex = 34;
            this.chkSizeCurveSimilarStores.Text = "Size Curve Similar Stores";
            // 
            // chkSizeCurveTolerance
            // 
            this.chkSizeCurveTolerance.Location = new System.Drawing.Point(161, 144);
            this.chkSizeCurveTolerance.Name = "chkSizeCurveTolerance";
            this.chkSizeCurveTolerance.Size = new System.Drawing.Size(153, 24);
            this.chkSizeCurveTolerance.TabIndex = 33;
            this.chkSizeCurveTolerance.Text = "Size Curve Tolerance";
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.AutoSize = true;
            this.lblTimePeriod.Enabled = false;
            this.lblTimePeriod.Location = new System.Drawing.Point(108, 209);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(66, 13);
            this.lblTimePeriod.TabIndex = 32;
            this.lblTimePeriod.Text = "Time Period:";
            // 
            // midDateRangeSelector1
            // 
            this.midDateRangeSelector1.DateRangeForm = null;
            this.midDateRangeSelector1.DateRangeRID = 0;
            this.midDateRangeSelector1.Enabled = false;
            this.midDateRangeSelector1.Location = new System.Drawing.Point(180, 204);
            this.midDateRangeSelector1.Name = "midDateRangeSelector1";
            this.midDateRangeSelector1.Size = new System.Drawing.Size(160, 24);
            this.midDateRangeSelector1.TabIndex = 31;
            this.midDateRangeSelector1.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.midDateRangeSelector1_OnSelection);
            this.midDateRangeSelector1.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.midDateRangeSelector1_ClickCellButton);
            this.midDateRangeSelector1.Load += new System.EventHandler(this.midDateRangeSelector1_Load);
            // 
            // chkSizeCurveCriteria
            // 
            this.chkSizeCurveCriteria.Location = new System.Drawing.Point(8, 146);
            this.chkSizeCurveCriteria.Name = "chkSizeCurveCriteria";
            this.chkSizeCurveCriteria.Size = new System.Drawing.Size(153, 24);
            this.chkSizeCurveCriteria.TabIndex = 15;
            this.chkSizeCurveCriteria.Text = "Size Curve Criteria";
            // 
            // chkVSW
            // 
            this.chkVSW.Location = new System.Drawing.Point(161, 174);
            this.chkVSW.Name = "chkVSW";
            this.chkVSW.Size = new System.Drawing.Size(153, 24);
            this.chkVSW.TabIndex = 14;
            this.chkVSW.Text = "VSW";
            // 
            // chkChainSetPercent
            // 
            this.chkChainSetPercent.Location = new System.Drawing.Point(8, 206);
            this.chkChainSetPercent.Name = "chkChainSetPercent";
            this.chkChainSetPercent.Size = new System.Drawing.Size(96, 24);
            this.chkChainSetPercent.TabIndex = 13;
            this.chkChainSetPercent.Text = "Chain Set %";
            this.chkChainSetPercent.CheckedChanged += new System.EventHandler(this.chkChainSetPercent_CheckedChanged);
            // 
            // chkCharacteristics
            // 
            this.chkCharacteristics.Location = new System.Drawing.Point(8, 176);
            this.chkCharacteristics.Name = "chkCharacteristics";
            this.chkCharacteristics.Size = new System.Drawing.Size(153, 24);
            this.chkCharacteristics.TabIndex = 12;
            this.chkCharacteristics.Text = "Characteristics";
            // 
            // chkForecastType
            // 
            this.chkForecastType.Location = new System.Drawing.Point(314, 114);
            this.chkForecastType.Name = "chkForecastType";
            this.chkForecastType.Size = new System.Drawing.Size(153, 24);
            this.chkForecastType.TabIndex = 11;
            this.chkForecastType.Text = "Forecast Type";
            this.chkForecastType.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkDailyPercents
            // 
            this.chkDailyPercents.Location = new System.Drawing.Point(314, 84);
            this.chkDailyPercents.Name = "chkDailyPercents";
            this.chkDailyPercents.Size = new System.Drawing.Size(153, 24);
            this.chkDailyPercents.TabIndex = 10;
            this.chkDailyPercents.Text = "Daily %\'s";
            this.chkDailyPercents.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkStockMinMax
            // 
            this.chkStockMinMax.Location = new System.Drawing.Point(314, 54);
            this.chkStockMinMax.Name = "chkStockMinMax";
            this.chkStockMinMax.Size = new System.Drawing.Size(153, 24);
            this.chkStockMinMax.TabIndex = 9;
            this.chkStockMinMax.Text = "Stock Min/Max";
            this.chkStockMinMax.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkSimilarStore
            // 
            this.chkSimilarStore.Location = new System.Drawing.Point(314, 24);
            this.chkSimilarStore.Name = "chkSimilarStore";
            this.chkSimilarStore.Size = new System.Drawing.Size(153, 24);
            this.chkSimilarStore.TabIndex = 8;
            this.chkSimilarStore.Text = "Similar Store";
            this.chkSimilarStore.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkForecastLevel
            // 
            this.chkForecastLevel.Location = new System.Drawing.Point(161, 114);
            this.chkForecastLevel.Name = "chkForecastLevel";
            this.chkForecastLevel.Size = new System.Drawing.Size(153, 24);
            this.chkForecastLevel.TabIndex = 7;
            this.chkForecastLevel.Text = "Forecast Level";
            this.chkForecastLevel.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkCapacity
            // 
            this.chkCapacity.Location = new System.Drawing.Point(161, 84);
            this.chkCapacity.Name = "chkCapacity";
            this.chkCapacity.Size = new System.Drawing.Size(153, 24);
            this.chkCapacity.TabIndex = 6;
            this.chkCapacity.Text = "Capacity";
            this.chkCapacity.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkAllocationMinMax
            // 
            this.chkAllocationMinMax.Location = new System.Drawing.Point(161, 54);
            this.chkAllocationMinMax.Name = "chkAllocationMinMax";
            this.chkAllocationMinMax.Size = new System.Drawing.Size(153, 24);
            this.chkAllocationMinMax.TabIndex = 5;
            this.chkAllocationMinMax.Text = "Allocation Min/Max";
            this.chkAllocationMinMax.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkModifiers
            // 
            this.chkModifiers.Location = new System.Drawing.Point(161, 24);
            this.chkModifiers.Name = "chkModifiers";
            this.chkModifiers.Size = new System.Drawing.Size(153, 24);
            this.chkModifiers.TabIndex = 4;
            this.chkModifiers.Text = "Modifiers";
            this.chkModifiers.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkPurgeCriteria
            // 
            this.chkPurgeCriteria.Location = new System.Drawing.Point(8, 116);
            this.chkPurgeCriteria.Name = "chkPurgeCriteria";
            this.chkPurgeCriteria.Size = new System.Drawing.Size(153, 24);
            this.chkPurgeCriteria.TabIndex = 3;
            this.chkPurgeCriteria.Text = "Purge Criteria";
            this.chkPurgeCriteria.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkVelocityGrades
            // 
            this.chkVelocityGrades.Location = new System.Drawing.Point(8, 86);
            this.chkVelocityGrades.Name = "chkVelocityGrades";
            this.chkVelocityGrades.Size = new System.Drawing.Size(153, 24);
            this.chkVelocityGrades.TabIndex = 2;
            this.chkVelocityGrades.Text = "Velocity Grades";
            this.chkVelocityGrades.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkStoreGrades
            // 
            this.chkStoreGrades.Location = new System.Drawing.Point(8, 56);
            this.chkStoreGrades.Name = "chkStoreGrades";
            this.chkStoreGrades.Size = new System.Drawing.Size(153, 24);
            this.chkStoreGrades.TabIndex = 1;
            this.chkStoreGrades.Text = "Store Grades";
            this.chkStoreGrades.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // chkEligibility
            // 
            this.chkEligibility.Location = new System.Drawing.Point(8, 24);
            this.chkEligibility.Name = "chkEligibility";
            this.chkEligibility.Size = new System.Drawing.Size(153, 24);
            this.chkEligibility.TabIndex = 0;
            this.chkEligibility.Text = "Eligibility";
            this.chkEligibility.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // cmsInclude
            // 
            this.cmsInclude.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsIncludeSelectAll,
            this.cmsIncludeClearAll});
            this.cmsInclude.Name = "cmsInclude";
            this.cmsInclude.ShowImageMargin = false;
            this.cmsInclude.Size = new System.Drawing.Size(98, 48);
            // 
            // cmsIncludeSelectAll
            // 
            this.cmsIncludeSelectAll.Name = "cmsIncludeSelectAll";
            this.cmsIncludeSelectAll.Size = new System.Drawing.Size(97, 22);
            this.cmsIncludeSelectAll.Text = "Select All";
            this.cmsIncludeSelectAll.Click += new System.EventHandler(this.cmsIncludeSelectAll_Click);
            // 
            // cmsIncludeClearAll
            // 
            this.cmsIncludeClearAll.Name = "cmsIncludeClearAll";
            this.cmsIncludeClearAll.Size = new System.Drawing.Size(97, 22);
            this.cmsIncludeClearAll.Text = "Clear All";
            this.cmsIncludeClearAll.Click += new System.EventHandler(this.cmsIncludeClearAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(409, 359);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(321, 359);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblError
            // 
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(8, 359);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(296, 32);
            this.lblError.TabIndex = 11;
            this.lblError.Text = "You must include at least one subreport.";
            this.lblError.Visible = false;
            // 
            // frmReportNodePropertiesOverridesSetup
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 401);
            this.ContextMenuStrip = this.cmsInclude;
            this.Controls.Add(this.txtSet);
            this.Controls.Add(this.txtStoreNumber);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grbInclude);
            this.Controls.Add(this.txtMerchandise);
            this.Controls.Add(this.lblSet);
            this.Controls.Add(this.lblStore);
            this.Controls.Add(this.cboLowLevels);
            this.Controls.Add(this.lblLowLevel);
            this.Controls.Add(this.lblMerchandise);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmReportNodePropertiesOverridesSetup";
            this.Text = "Node Properties Overrides Report";
            this.Load += new System.EventHandler(this.frmReportNodePropertiesOverridesSetup_Load);
            this.Controls.SetChildIndex(this.lblMerchandise, 0);
            this.Controls.SetChildIndex(this.lblLowLevel, 0);
            this.Controls.SetChildIndex(this.cboLowLevels, 0);
            this.Controls.SetChildIndex(this.lblStore, 0);
            this.Controls.SetChildIndex(this.lblSet, 0);
            this.Controls.SetChildIndex(this.txtMerchandise, 0);
            this.Controls.SetChildIndex(this.grbInclude, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.lblError, 0);
            this.Controls.SetChildIndex(this.txtStoreNumber, 0);
            this.Controls.SetChildIndex(this.txtSet, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.grbInclude.ResumeLayout(false);
            this.grbInclude.PerformLayout();
            this.cmsInclude.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		#region Event Handlers
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if(!InputsValid())
			 {
				lblError.Visible = true;
				return;
			 }
			try
			{
                Cursor.Current = Cursors.WaitCursor;

				//MIDConfigurationManager.AppSettings["databaseName"]
				int merch = 0;			
				if(this.NodeRID >= 0) 
				   merch = this.NodeRID;			  
								
				int Eligibility = (chkEligibility.Checked)?1:0;
				int Modifiers = (chkModifiers.Checked) ? 1 : 0;
				int SimilarStore = (chkSimilarStore.Checked) ? 1 : 0;
				int StoreGrades = (chkStoreGrades.Checked) ? 1 : 0;
				int AllocationMinMax = (chkAllocationMinMax.Checked) ? 1 : 0;
				int Stock = (chkStockMinMax.Checked) ? 1 : 0;
				int VelocityGrades = (chkVelocityGrades.Checked) ? 1 : 0;
				int Capacity = (chkCapacity.Checked) ? 1 : 0;
				int Daily = (chkDailyPercents.Checked) ? 1 : 0;
				int PurgeCriteria = (chkPurgeCriteria.Checked) ? 1 : 0;
				int ForecastLevel = (chkForecastLevel.Checked) ? 1 : 0;
				int ForecastType= (chkForecastType.Checked) ? 1 : 0;


                //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                int Characteristics = (this.chkCharacteristics.Checked) ? 1 : 0;
                int ChainSetPercent = (this.chkChainSetPercent.Checked) ? 1 : 0;
                int VSW = (this.chkVSW.Checked) ? 1 : 0;
                int SizeCurveCriteria = (this.chkSizeCurveCriteria.Checked) ? 1 : 0;
                int SizeCurveTolerance = (this.chkSizeCurveTolerance.Checked) ? 1 : 0;
                int SizeCurveSimilarStores = (this.chkSizeCurveSimilarStores.Checked) ? 1 : 0;
                //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report

				int Lowerlevel;
                // Begin TT#264 - RMatelic - Forecast Level and Forecast Type report problems
                //if(cboLowLevels.SelectedText=="(Low Level)" ||cboLowLevels.SelectedText=="" )		
                if (cboLowLevels.Text == "(Low Level)" || cboLowLevels.Text == string.Empty)
                {
                    Lowerlevel = 0;
                }
                // End TT#264
                else
                {
                    try
                    {
                        // Begin TT#264 - RMatelic - Forecast Level and Forecast Type report problems
                        //Lowerlevel = Int32.Parse(cboLowLevels.SelectedItem.ToString());
                        Lowerlevel = cboLowLevels.SelectedIndex;
                        // End TT#264
                    }
                    catch (Exception)
                    {
                        lblError.Visible = true; //TT#726-MD -jsobek Node Properties Override Report - Chain Set % - data is not correct
                        lblError.Text = "You Must Enter a Numeric Value For Lower Level";
                        return;
                    }
                }


                //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                if (ChainSetPercent == 1 && chainSetPercentSelectedWeekRange == null)
                {
                    lblError.Visible = true; //TT#726-MD -jsobek Node Properties Override Report - Chain Set % - data is not correct
                    lblError.Text = "You Must Enter a Week Range For Chain Set %";
                    return;
                }
                //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report

                _storeRIDTextList = string.Empty;            // TT#265-Eligibility Report not showing low level nodes and formatting problems   
                //String Store = txtStoreNumber.Text;   // TT#265-Eligibility Report not showing low level nodes and formatting problems 
				String Store = "";
                String storeGroup = "";
                String storeSet = "";                   // TT#265-Eligibility Report not showing low level nodes and formatting problems - changed parm name
				if(txtSet.Text.Trim()=="")
				{
					storeGroup="";
                    storeSet = "";                      // TT#265-Eligibility Report not showing low level nodes and formatting problems - changed parm name
                     
				}
				else
				{
					int index = txtSet.Text.IndexOf("/");
					if(index <= 0)
				    {
                      lblError.Text = string.Format(MIDText.GetText(eMIDTextCode.msg_MustEnterValidValue), MIDText.GetTextOnly(eMIDTextCode.lbl_Set));
	                  return;				
					}
					storeGroup = txtSet.Text.Substring(0,index);
                    storeSet = txtSet.Text.Substring(index + 1);      // TT#265-Eligibility Report not showing low level nodes and formatting problems - changed parm name
                    GetStoresInSet(storeGroup, storeSet);
                }
                 
                // Begin TT#265 - RMAtelic - Eligibility Report not showing low level nodes and formatting problems
                if (txtStoreNumber.Text.Trim() == "")
                {
                    Store = "";
                }
                else
                {
                    int index = txtStoreNumber.Text.IndexOf("[");
                    if (index <= 0)
                    {
                        lblError.Text = string.Format(MIDText.GetText(eMIDTextCode.msg_MustEnterValidValue), MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular));
                        return;
                    }
                    Store  = txtStoreNumber.Text.Substring(0, index);
                }
                // End TT#265

				//Get the low level details
				int lowLevelNo = 0;
				if(cboLowLevels.SelectedIndex >= 0)
				{
					LowLevelCombo lowLevelComb = (LowLevelCombo)cboLowLevels.SelectedItem;
					lowLevelNo = lowLevelComb.LowLevelSequence;
				}

                ReportData reportData = new ReportData();
                Windows.CrystalReports.NodePropertiesOverrides nodePropertiesOverridesReport = new Windows.CrystalReports.NodePropertiesOverrides();

                System.Data.DataSet getReportsDataSet = MIDEnvironment.CreateDataSet("GetReportsDataSet");
                reportData.GetReports_Report(getReportsDataSet, 
                                            Eligibility,
                                            Modifiers,
                                            SimilarStore,
                                            StoreGrades,
                                            AllocationMinMax,
                                            VelocityGrades,
                                            Capacity,
                                            Daily,
                                            PurgeCriteria,
                                            ForecastLevel,
                                            ForecastType,
                                            Stock,
                                            //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                                            Characteristics,
                                            ChainSetPercent,
                                            VSW,
                                            SizeCurveCriteria,
                                            SizeCurveTolerance,
                                            SizeCurveSimilarStores     
                                            );
                                            //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                                            
                nodePropertiesOverridesReport.SetDataSource(getReportsDataSet);

                if (Daily == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet dailyPercentagesDataSet = MIDEnvironment.CreateDataSet("DailyPercentagesDataSet");
                    reportData.DailyPercentages_Report(dailyPercentagesDataSet,
                                                      merch,
                                                      lowLevelNo,
                                                      Store,
                        //storeGroup,        // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems  
                        //storeSet);   
                                                      _storeRIDTextList);     // End TT#265
                    nodePropertiesOverridesReport.Subreports["DailyPercentages.rpt"].SetDataSource(dailyPercentagesDataSet);
                }

                if (ForecastLevel == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet forecastLevelDataSet = MIDEnvironment.CreateDataSet("ForecastLevelDataSet");
                    reportData.ForecastLevel_Report(forecastLevelDataSet,
                                                   merch,
                                                   lowLevelNo,
                                                   ForecastLevel);
                    nodePropertiesOverridesReport.Subreports["OTSForecast.rpt"].SetDataSource(forecastLevelDataSet);
                }

                if (ForecastType == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet forecastTypeDataSet = MIDEnvironment.CreateDataSet("ForecastTypeDataSet");
                    reportData.ForecastType_Report(forecastTypeDataSet,
                                                  merch,
                                                  lowLevelNo);
                    nodePropertiesOverridesReport.Subreports["ForecasteType.rpt"].SetDataSource(forecastTypeDataSet);
                }

                string inheritedMsg = string.Empty; // TT#351-RMatelic-Receive Enter Parameter Values dialog when requesting Node Properties Overrides Report
                if (PurgeCriteria == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet purgeDatesDataSet = MIDEnvironment.CreateDataSet("PurgeDatesDataSet");
                    reportData.PurgeDates_Report(purgeDatesDataSet,
                                                merch,
                                                lowLevelNo);
                    //Begin TT#274 - RMatelic - Purge Criteria report intersperses nodes found with level definition purge criteria
                    bool foundInherited = false;
                    string lblAsterisks = MIDText.GetTextOnly(eMIDTextCode.lbl_Asterisks);
                    System.Data.DataTable dt = purgeDatesDataSet.Tables[0];
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        if (dr["SORTSEQ"].ToString() == "2")    // these are nodes
                        {
                            HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData((int)dr["HN_RID"]);
                            if (dr["PURGE_DAILY_HISTORY_WEEKS"] == System.DBNull.Value)
                            {
                                dr["PURGE_DAILY_HISTORY_WEEKS"] = hnp.PurgeDailyHistoryAfter;
                                dr["DAILY_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_WEEKLY_HISTORY_WEEKS"] == System.DBNull.Value)
                            {
                                dr["PURGE_WEEKLY_HISTORY_WEEKS"] = hnp.PurgeWeeklyHistoryAfter;
                                dr["WEEKLY_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_PLANS_WEEKS"] == System.DBNull.Value)
                            {
                                dr["PURGE_PLANS_WEEKS"] = hnp.PurgeOTSPlansAfter;
                                dr["PLANS_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            //Begin TT#400-MD -jsobek -Add Header Purge criteria by Header Type
                            if (dr["PURGE_HEADERS_WEEKS_RECEIPT"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_RECEIPT"] = hnp.PurgeHtReceiptAfter;
                                dr["HEADERS_RECEIPT_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_ASN"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_ASN"] = hnp.PurgeHtASNAfter;
                                dr["HEADERS_ASN_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_DUMMY"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_DUMMY"] = hnp.PurgeHtDummyAfter;
                                dr["HEADERS_DUMMY_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_DROPSHIP"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_DROPSHIP"] = hnp.PurgeHtDropShipAfter;
                                dr["HEADERS_DROPSHIP_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_RESERVE"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_RESERVE"] = hnp.PurgeHtReserveAfter;
                                dr["HEADERS_RESERVE_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_WORKUPTOTALBUY"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_WORKUPTOTALBUY"] = hnp.PurgeHtWorkUpTotAfter;
                                dr["HEADERS_WORKUPTOTALBUY_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_PO"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_PO"] = hnp.PurgeHtPurchaseOrderAfter;
                                dr["HEADERS_PO_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            if (dr["PURGE_HEADERS_WEEKS_VSW"] == System.DBNull.Value)
                            {
                                dr["PURGE_HEADERS_WEEKS_VSW"] = hnp.PurgeHtVSWAfter;
                                dr["HEADERS_VSW_INH"] = lblAsterisks;
                                foundInherited = true;
                            }
                            //End TT#400-MD -jsobek -Add Header Purge criteria by Header Type
                  
                            //Begin TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                            //if (dr["PURGE_HEADERS_WEEKS"] == System.DBNull.Value)
                            //{
                            //    dr["PURGE_HEADERS_WEEKS"]  = hnp.PurgeHeadersAfter;
                            //    dr["HEADERS_INH"] = lblAsterisks;
                            //    foundInherited = true;
                            //}
                            //End TT#400-MD - JSmith - Add Header Purge Criteria by Header Type
                        }
                    }
                    purgeDatesDataSet.AcceptChanges();
                    nodePropertiesOverridesReport.Subreports["PurgeCriteria.rpt"].SetDataSource(purgeDatesDataSet);
                    // Begin T#351-RMatelic-Receive Enter Parameter Values dialog when requesting Node Properties Overrides Report
                    //string inheritedMsg = string.Empty;
                    // End TT#351 
                    if (foundInherited)
                    {
                        //Begin TT#400-MD -jsobek -Add Header Purge criteria by Header Type
                        //Fixing message
                        inheritedMsg = MIDText.GetTextOnly(eMIDTextCode.msg_InheritedFromHigherLevel);
                        //End TT#400-MD -jsobek -Add Header Purge criteria by Header Type
                    }
                    nodePropertiesOverridesReport.SetParameterValue("@INHERITED_TEXT", inheritedMsg, "PurgeCriteria.rpt");
                    //End TT#274
                }

                if (Capacity == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet storeCapacityDataSet = MIDEnvironment.CreateDataSet("StoreCapacityDataSet");
                    reportData.StoreCapacity_Report(storeCapacityDataSet,
                                                   merch,
                                                   lowLevelNo,
                                                   Store,
                        //storeGroup,            // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems  
                        //storeSet);            
                                                   _storeRIDTextList);     // End TT#265
                    nodePropertiesOverridesReport.Subreports["StoreCapacity.rpt"].SetDataSource(storeCapacityDataSet);
                }

                if (Eligibility == 1 || Modifiers == 1 || SimilarStore == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet storeEligibilityDataSet = MIDEnvironment.CreateDataSet("StoreEligibilityDataSet");
                    reportData.StoreEligibility_Report(storeEligibilityDataSet,
                                                      merch,
                                                      lowLevelNo,
                                                      Store,
                                                      //storeGroup,         // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems  
                                                      //storeSet,          
                                                      _storeRIDTextList,    // End TT#265
                                                      Eligibility,
                                                      Modifiers,
                                                      SimilarStore);
                    // Begin TT#278 - RMatelic - Sim Store report shows "+315Weeks" instead of date range for one of the stores listed.
                    // This occurs when the date range is Dynamic
                    if (SimilarStore == 1)
                    {
                        System.Data.DataTable dt = storeEligibilityDataSet.Tables[0];
                        foreach (System.Data.DataRow dr in dt.Rows)
                        {
                            if (dr["CDR_RANGE_TYPE_ID"] != DBNull.Value)
                            {
                                if ((eCalendarRangeType)Convert.ToInt32(dr["CDR_RANGE_TYPE_ID"], CultureInfo.CurrentUICulture) == eCalendarRangeType.Dynamic)
                                {
                                    int storeRID = Convert.ToInt32(dr["STORE_RID"], CultureInfo.CurrentUICulture);
                                    StoreProfile storeProfile = StoreMgmt.StoreProfile_Get(storeRID); //_SAB.StoreServerSession.GetStoreProfile(storeRID);
                                    
                                    if (storeProfile.SellingOpenDt != Include.UndefinedDate)
                                    {
                                        DateRangeProfile drp = _SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture));
                                        WeekProfile mrsWeek = _SAB.ClientServerSession.Calendar.GetWeek(storeProfile.SellingOpenDt);
                                        drp.InternalAnchorDate = mrsWeek;
                                        dr["PERIOD"] = _SAB.ClientServerSession.Calendar.GetDisplayDate(drp);
                                    }
                                }
                            }  
                        }
                    }
                    nodePropertiesOverridesReport.Subreports["StoreEligibility.rpt"].SetDataSource(storeEligibilityDataSet);
                    // End TT#278  
                }

                if (StoreGrades == 1 || AllocationMinMax == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet storeGradesDataSet = MIDEnvironment.CreateDataSet("StoreGradesDataSet");
                    reportData.StoreGrades_Report(storeGradesDataSet,
                                                 merch,
                                                 lowLevelNo,
                                                 StoreGrades,
                                                 AllocationMinMax);
                    nodePropertiesOverridesReport.Subreports["StoreGrades.rpt"].SetDataSource(storeGradesDataSet);
                }

                if (VelocityGrades == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet velocityGradesDataSet = MIDEnvironment.CreateDataSet("VelocityGradesDataSet");
                    reportData.VelocityGrades_Report(velocityGradesDataSet,
                                                    merch,
                                                    lowLevelNo);
                    nodePropertiesOverridesReport.Subreports["VelocityGrades.rpt"].SetDataSource(velocityGradesDataSet);
                }

                if (Stock == 1) // TT#277 - unrelated but adds performance
                {
                    System.Data.DataSet stockMinMaxDataSet = MIDEnvironment.CreateDataSet("StockMinMaxDataSet");
                    reportData.StockMinMax_Report(stockMinMaxDataSet,
                                                 merch,
                                                 lowLevelNo);
                    nodePropertiesOverridesReport.Subreports["StockMinMax.rpt"].SetDataSource(stockMinMaxDataSet);
                }

                //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                string selectedWeeksCaption = String.Empty;
                if (Characteristics == 1) 
                {
                    System.Data.DataSet characteristicsDataSet = MIDEnvironment.CreateDataSet("CharacteristicsDataSet");
                    reportData.Characteristic_Report(characteristicsDataSet,
                                                 merch,
                                                 lowLevelNo);
                    nodePropertiesOverridesReport.Subreports["Characteristics.rpt"].SetDataSource(characteristicsDataSet);
                }
                if (ChainSetPercent== 1)
                {
                    System.Data.DataSet chainSetPercentDataSet = MIDEnvironment.CreateDataSet("ChainSetPercentagesDataSet");

                    if (Store != String.Empty)
                    {
                        reportData.ChainSetPercent_Report(chainSetPercentDataSet,
                                                     merch,
                                                     lowLevelNo,
                                                     Store,
                                                     null,
                                                     chainSetPercentStartDateKey,
                                                     chainSetPercentEndDateKey
                                                     );
                    }
                    else
                    {
                        reportData.ChainSetPercent_Report(chainSetPercentDataSet,
                             merch,
                             lowLevelNo,
                             null,
                             _storeRIDTextList,
                             chainSetPercentStartDateKey,
                             chainSetPercentEndDateKey
                             );
                    }
                   

                    //assign the percentage to the correct week in the dataset
                    //System.Data.DataView dv = new System.Data.DataView(chainSetPercentDataSet.Tables[0], "" ,"TIME_ID", System.Data.DataViewRowState.CurrentRows);

                    //foreach (System.Data.DataRow dr in chainSetPercentDataSet.Tables[0].Rows)
                    //{
                    //    int timeID = (int)dr["TIME_ID"];

                    //    //add the week columns to the dataset, on the first pass
                    //    int iWeekCounter = 1;
                        
                    //    foreach (WeekProfile wp in chainSetPercentSelectedWeekRange)
                    //    {
                    //        string stringWeek = Convert.ToString(wp);

                    //        int yearWeek = Convert.ToInt32(Convert.ToString(wp.YearWeek));

                    //        string strCounter = iWeekCounter.ToString().PadLeft(2, '0');

                    //        string dataColumnName = "WEEK" + strCounter + "_PERCENT";
                    //        if (chainSetPercentDataSet.Tables[0].Columns.Contains(dataColumnName) == false)
                    //        {

                    //            System.Data.DataColumn dc = new System.Data.DataColumn(dataColumnName);

                    //            chainSetPercentDataSet.Tables[0].Columns.Add(dc);
                    //            chainSetPercentDataSet.Tables[0].Columns.Add("WEEK" + strCounter + "_TIMEID");
                    //            chainSetPercentDataSet.Tables[0].Columns.Add("WEEK" + strCounter + "_CAPTION");

                    //            //Set the overall weeks caption Week xx thru Week yy
                    //            if (iWeekCounter == 1)
                    //            {
                    //                selectedWeeksCaption = stringWeek + " thru ";
                    //            }
                    //            else if (iWeekCounter == chainSetPercentSelectedWeekRange.Count)
                    //            {
                    //                selectedWeeksCaption += stringWeek;
                    //            }
                    //        }

                    //        int rowIndex = chainSetPercentDataSet.Tables[0].Rows.IndexOf(dr);
                    //        chainSetPercentDataSet.Tables[0].Rows[rowIndex]["WEEK" + strCounter + "_TIMEID"] = yearWeek.ToString();
                    //        chainSetPercentDataSet.Tables[0].Rows[rowIndex]["WEEK" + strCounter + "_CAPTION"] = yearWeek.ToString().Substring(4, 2); // stringWeek;
                    //        if (timeID.ToString() == yearWeek.ToString())
                    //        {
                    //            string percentage = String.Empty;
                    //            if (dr["PERCENTAGE"] != DBNull.Value)
                    //            {
                    //                //Begin TT#701-MD -jsobek -Node Properties Overrides Report Issues
                    //                //per Vickie - the percentage should match the number of decimals that are displayed in the application
                    //                //percentage = Convert.ToDouble(dr["PERCENTAGE"]).ToString(".000");
                    //                percentage = Convert.ToDecimal(dr["PERCENTAGE"], CultureInfo.CurrentUICulture).ToString();
                    //                //End TT#701-MD -jsobek -Node Properties Overrides Report Issues
                    //            }
                    //            chainSetPercentDataSet.Tables[0].Rows[rowIndex]["WEEK" + strCounter + "_PERCENT"] = percentage;          
                    //        }
                            
                    //        //only consider the first 26 weeks
                    //        if (iWeekCounter > 26)
                    //        {
                    //            break;
                    //        }
                    //        iWeekCounter++;

                           

                    //    }

                        
                    //}


 

                    //add the week columns to the dataset, on the first pass
                    int iWeekCounter = 1;
                    foreach (WeekProfile wp in chainSetPercentSelectedWeekRange)
                    {
                        string stringWeek = Convert.ToString(wp);

                        int yearWeek = Convert.ToInt32(Convert.ToString(wp.YearWeek));

                        string strCounter = iWeekCounter.ToString().PadLeft(2, '0');

                        string dataColumnName = "WEEK" + strCounter + "_PERCENT";
                        if (chainSetPercentDataSet.Tables[0].Columns.Contains(dataColumnName) == false)
                        {

                            System.Data.DataColumn dc = new System.Data.DataColumn(dataColumnName);

                            chainSetPercentDataSet.Tables[0].Columns.Add(dc);
                            chainSetPercentDataSet.Tables[0].Columns.Add("WEEK" + strCounter + "_TIMEID");
                            chainSetPercentDataSet.Tables[0].Columns.Add("WEEK" + strCounter + "_CAPTION");

                            //Set the overall weeks caption Week xx thru Week yy
                            if (iWeekCounter == 1)
                            {
                                selectedWeeksCaption = stringWeek + " thru ";
                            }
                            else if (iWeekCounter == chainSetPercentSelectedWeekRange.Count)
                            {
                                selectedWeeksCaption += stringWeek;
                            }
                        }



                        //only consider the first 26 weeks
                        if (iWeekCounter > 26)
                        {
                            break;
                        }
                        iWeekCounter++;

                    }
                     iWeekCounter = 1;

                    foreach (WeekProfile wp in chainSetPercentSelectedWeekRange)
                    {
                        string stringWeek = Convert.ToString(wp);

                        int yearWeek = Convert.ToInt32(Convert.ToString(wp.YearWeek));

                        string strCounter = iWeekCounter.ToString().PadLeft(2, '0');



                        foreach (System.Data.DataRow dr in chainSetPercentDataSet.Tables[0].Rows)
                        {
                            int timeID = (int)dr["TIME_ID"];
                            int hnID = (int)dr["HN_RID"];
                            string storeID = (string)dr["ST_ID"];
                            int rowIndex = chainSetPercentDataSet.Tables[0].Rows.IndexOf(dr);
                            chainSetPercentDataSet.Tables[0].Rows[rowIndex]["WEEK" + strCounter + "_TIMEID"] = yearWeek.ToString();
                            chainSetPercentDataSet.Tables[0].Rows[rowIndex]["WEEK" + strCounter + "_CAPTION"] = yearWeek.ToString().Substring(4, 2); // stringWeek;
                            if (timeID.ToString() == yearWeek.ToString())
                            {
                                string percentage = String.Empty;
                                if (dr["PERCENTAGE"] != DBNull.Value)
                                {
                                    //Begin TT#701-MD -jsobek -Node Properties Overrides Report Issues
                                    //per Vickie - the percentage should match the number of decimals that are displayed in the application
                                    //percentage = Convert.ToDouble(dr["PERCENTAGE"]).ToString(".000");
                                    percentage = Convert.ToDecimal(dr["PERCENTAGE"], CultureInfo.CurrentUICulture).ToString();
                                    //End TT#701-MD -jsobek -Node Properties Overrides Report Issues
                          
                                foreach (System.Data.DataRow dr2 in chainSetPercentDataSet.Tables[0].Rows)
                                {
                                    int hnID2 = (int)dr2["HN_RID"];
                                    string storeID2 = (string)dr2["ST_ID"];
                                    if (hnID2 == hnID && storeID==storeID2)
                                    {
                                        int rowIndex2 = chainSetPercentDataSet.Tables[0].Rows.IndexOf(dr2);
                                        chainSetPercentDataSet.Tables[0].Rows[rowIndex2]["WEEK" + strCounter + "_PERCENT"] = percentage;
                                    }
                                }
                                }
                            }
                        }
                          

                        //only consider the first 26 weeks
                        if (iWeekCounter > 26)
                        {
                            break;
                        }
                        iWeekCounter++;

                    }


                   

                       
               

                    nodePropertiesOverridesReport.Subreports["ChainSetPercentages.rpt"].SetDataSource(chainSetPercentDataSet);
                }
                if (VSW == 1)
                {
                    System.Data.DataSet VSWDataSet = MIDEnvironment.CreateDataSet("VSWDataSet");

                
                    if (Store != String.Empty)
                    {
                        reportData.VSW_Report(VSWDataSet,
                                                     merch,
                                                     lowLevelNo,
                                                     Store,
                                                     null
                                                     );
                    }
                    else
                    {
                        reportData.VSW_Report(VSWDataSet,
                             merch,
                             lowLevelNo,
                             null,
                             _storeRIDTextList
                             );
                    }

                    foreach (System.Data.DataRow dr in VSWDataSet.Tables[0].Rows)
                    {
                        int rowIndex = VSWDataSet.Tables[0].Rows.IndexOf(dr);
                        int itemMaxValue = (int)dr["ITEM_MAX_VALUE"];
                        if (itemMaxValue == int.MaxValue)
                        {
                            VSWDataSet.Tables[0].Rows[rowIndex]["ITEM_MAX_VALUE"] = DBNull.Value; 
                        }
                        if ((int)dr["PUSH_TO_BACKSTOCK"] == -1)
                        {
                            VSWDataSet.Tables[0].Rows[rowIndex]["PUSH_TO_BACKSTOCK"] = DBNull.Value;
                        }

                    }
                    nodePropertiesOverridesReport.Subreports["VSW.rpt"].SetDataSource(VSWDataSet);
                }
                if (SizeCurveCriteria == 1)
                {
                

                    System.Data.DataSet sizeCurveCriteriaDataSet = MIDEnvironment.CreateDataSet("SizeCurveCriteriaDataSet");
                    reportData.SizeCurveCriteria_Report(sizeCurveCriteriaDataSet,
                                                 merch,
                                                 lowLevelNo);

                    sizeCurveCriteriaDataSet.Tables[0].Columns.Add("INCLUDE_EXCLUDE");  

                    //go through and set the date range text based off of the calendar date range RID
                    foreach (System.Data.DataRow dr in sizeCurveCriteriaDataSet.Tables[0].Rows)
                    {
                        int rowIndex = sizeCurveCriteriaDataSet.Tables[0].Rows.IndexOf(dr);
                        if (dr["OVERRIDE_LOW_LEVEL_NODE"] != DBNull.Value)
                        {
                            sizeCurveCriteriaDataSet.Tables[0].Rows[rowIndex]["INCLUDE_EXCLUDE"] = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OverrideLowVersion);
                        }
                        sizeCurveCriteriaDataSet.Tables[0].Rows[rowIndex]["DISPLAY_DATE"] = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["CDR_RID"], CultureInfo.CurrentUICulture)).DisplayDate;
                    }

                    nodePropertiesOverridesReport.Subreports["SizeCurveCriteria.rpt"].SetDataSource(sizeCurveCriteriaDataSet);
                }
                if (SizeCurveTolerance == 1)
                {
                    System.Data.DataSet sizeCurveToleranceDataSet = MIDEnvironment.CreateDataSet("SizeCurveToleranceDataSet");
                    reportData.SizeCurveTolerance_Report(sizeCurveToleranceDataSet,
                                                 merch,
                                                 lowLevelNo);

                    nodePropertiesOverridesReport.Subreports["SizeCurveTolerance.rpt"].SetDataSource(sizeCurveToleranceDataSet);
                }
                if (SizeCurveSimilarStores == 1)
                {
                    System.Data.DataSet sizeCurveSimilarStoresDataSet = MIDEnvironment.CreateDataSet("SizeCurveSimilarStoresDataSet");
                    reportData.SizeCurveSimilarStores_Report(sizeCurveSimilarStoresDataSet,
                                                 merch,
                                                 lowLevelNo);

                    sizeCurveSimilarStoresDataSet.Tables[0].Columns.Add("UNTIL_DATE_TEXT"); 

                    //go through and set the date range text based off of the calendar date range RID
                    foreach (System.Data.DataRow dr in sizeCurveSimilarStoresDataSet.Tables[0].Rows)
                    {
                        int rowIndex = sizeCurveSimilarStoresDataSet.Tables[0].Rows.IndexOf(dr);
                        if (dr["SELLING_OPEN_DATE"] != DBNull.Value)
                        {
                            //we must use the store's open date as an "anchor" date in order to get the display date text
                            DateTime storeSellingOpenDate = (DateTime)dr["SELLING_OPEN_DATE"];
                            sizeCurveSimilarStoresDataSet.Tables[0].Rows[rowIndex]["UNTIL_DATE_TEXT"] = _SAB.ApplicationServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["UNTIL_DATE"], CultureInfo.CurrentUICulture), _SAB.ApplicationServerSession.Calendar.GetDay(storeSellingOpenDate)).DisplayDate;
                        }
                        else
                        {
                            sizeCurveSimilarStoresDataSet.Tables[0].Rows[rowIndex]["UNTIL_DATE_TEXT"] = SAB.ClientServerSession.Calendar.GetDateRange(Convert.ToInt32(dr["UNTIL_DATE"], CultureInfo.CurrentUICulture)).DisplayDate;
                        }
                       
                    }

                    nodePropertiesOverridesReport.Subreports["SizeCurveSimilarStores.rpt"].SetDataSource(sizeCurveSimilarStoresDataSet);
                }
                nodePropertiesOverridesReport.SetParameterValue("SELECTED_WEEKS_CAPTION", selectedWeeksCaption, "ChainSetPercentages.rpt");
            
                //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report

                nodePropertiesOverridesReport.SetParameterValue("@SELECTED_NODE_RID", merch);
                nodePropertiesOverridesReport.SetParameterValue("@LOWER_LEVEL", lowLevelNo);
                nodePropertiesOverridesReport.SetParameterValue("@STORE_ID", Store);
                // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems - changed parm name
                //nodePropertiesOverridesReport.SetParameterValue("@STORE_CHAR_GROUP", storeGroup);
                //nodePropertiesOverridesReport.SetParameterValue("@STORE_CHAR", storeSet);
                nodePropertiesOverridesReport.SetParameterValue("@STORE_RID_LIST", _storeRIDTextList);
                // End TT#265

                // Begin TT#277 - RMatelic - Report titles should show the override items selected
                nodePropertiesOverridesReport.SetParameterValue("@ELIGIBILITY_TITLE", SetEligibilityTitle(Eligibility,Modifiers,SimilarStore));
                nodePropertiesOverridesReport.SetParameterValue("@STOREGRADES_TITLE", SetStoreGradesTitle(StoreGrades, AllocationMinMax));
                // End TT#277
                // Begin TT#351-RMatelic-Receive Enter Parameter Values dialog when requesting Node Properties Overrides Report
                // unsure as to why the next line is necessary, but it prevents the parameter request dialog from needlessly displaying 
                nodePropertiesOverridesReport.SetParameterValue("@INHERITED_TEXT", inheritedMsg);
                // End TT#351 
                frmReportViewer viewer = new frmReportViewer(_SAB);
                //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                // Might as well show the correct name of the report on the viewer
                viewer.Text = "Node Properties Overrides Report"; //MIDText.GetTextOnly(eMIDTextCode.lbl_AllocationAudit);
                //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
                viewer.MdiParent = this.ParentForm;
                viewer.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                viewer.ReportSource = nodePropertiesOverridesReport;
                viewer.Show();
                viewer.BringToFront();
			}
            catch (Exception ex)
            {
                HandleException(ex);
            }
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

        // Begin TT#277 - RMatelic - Report titles should show the override items selected
        private string SetEligibilityTitle(int Eligibility, int Modifiers, int SimilarStore)
        {
            string title = string.Empty;
            if (Eligibility == 1 || Modifiers == 1 || SimilarStore == 1)
            {
                if (Eligibility == 1)
                {
                    title = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Eligibility);
                }
                if (Modifiers == 1)
                {
                    if (title == string.Empty)
                    {
                        title = MIDText.GetTextOnly(eMIDTextCode.lbl_Modifiers);
                    }
                    else
                    {
                        title += ", " + MIDText.GetTextOnly(eMIDTextCode.lbl_Modifiers);
                    }
                }
                if (SimilarStore == 1)
                {
                    if (title == string.Empty)
                    {
                        title = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);
                    }
                    else
                    {
                        title += ", " + MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);
                    }
                }
            }
            return title;
        }

        private string SetStoreGradesTitle(int StoreGrades, int AllocationMinMax)
        {
            string title = string.Empty;
            if (StoreGrades == 1 || AllocationMinMax == 1)
            {
                if (StoreGrades == 1)
                {
                    title = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Grades);
                }
                if (AllocationMinMax == 1)
                {
                    if (title == string.Empty)
                    {
                        title = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocationMinMax);
                    }
                    else
                    {
                        title += ", " + MIDText.GetTextOnly(eMIDTextCode.lbl_AllocationMinMax);
                    }
                }
            }
            return title;
        }
        // End TT#277

        // Begin TT#265 - RMatelic - Eligibility Report not showing low level nodes and formatting problems 
        private void GetStoresInSet(string aStoreGroup, string aStoreSet)
        {
            try
            {
                int index = txtSet.Text.IndexOf("/");
                int storeGroupRID = Include.NoRID;
                int storeGroupLevelRID = Include.NoRID;
              
                foreach (StoreGroupProfile sgp in _storeGroupList)
                {
                    if (aStoreGroup == sgp.Name)
                    {
                        storeGroupRID = sgp.Key;
                        ProfileList storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(storeGroupRID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(storeGroupRID);
                        foreach (StoreGroupLevelListViewProfile setView in storeGroupLevelList)
                        {
                            if (setView.Name == aStoreSet)
                            {
                                storeGroupLevelRID = setView.Key;
                                break;
                            }
                        }
                        break;
                    }
                }
                if (storeGroupRID != Include.NoRID && storeGroupLevelRID != Include.NoRID)
                {
                    StringBuilder sb = new StringBuilder();
                    ProfileList storeProfileList = StoreMgmt.StoreGroupLevel_GetStoreProfileList(storeGroupRID, storeGroupLevelRID); //_SAB.StoreServerSession.GetStoresInGroup(storeGroupRID, storeGroupLevelRID);
                    foreach (StoreProfile storeProfile in storeProfileList)
                    {
                        sb.Append(storeProfile.Key  .ToString());
                        sb.Append(",");
                    }
                    _storeRIDTextList = sb.ToString();
                }
                
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        // End TT#265

		private void CheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if(lblError.Visible)
				lblError.Visible = false;
		}

        protected void txtMerchandise_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            txtBaseMerchandise_KeyPress(sender, e);
        }

        protected void txtMerchandise_Validated(object sender, System.EventArgs e)
        {
            txtBaseMerchandise_Validated(sender, e);
        }

		protected void txtMerchandise_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
                txtBaseMerchandise_Validating(sender, e);
				if (NodeRID != Include.NoRID)
				{
					PopulateLowLevels();
				}
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        // Begin TT#264 - RMatelic - Forecast Level and Forecast Type report problems
        private void cmsIncludeSelectAll_Click(object sender, EventArgs e)
        {
            SetCheckedState(true);
        }

        private void cmsIncludeClearAll_Click(object sender, EventArgs e)
        {
            SetCheckedState(false);
        }
        // End TT#264 
		#endregion

		#region Private Methods
		private bool InputsValid()
		{
			if(chkEligibility.Checked)
				return true;

			if(chkStoreGrades.Checked)
				return true;

			if(chkVelocityGrades.Checked)
				return true;

			if(chkPurgeCriteria.Checked)
				return true;

			if(chkModifiers.Checked)
				return true;

			if(chkAllocationMinMax.Checked)
				return true;

			if(chkCapacity.Checked)
				return true;

			if(chkForecastLevel.Checked)
				return true;

			if(chkSimilarStore.Checked)
				return true;

			if(chkStockMinMax.Checked)
				return true;

			if(chkDailyPercents.Checked)
				return true;

			if(chkForecastType.Checked)
				return true;

            //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
            if (this.chkCharacteristics.Checked)
                return true;

            if (this.chkChainSetPercent.Checked)
                return true;

            if (this.chkVSW.Checked)
                return true;

            if (this.chkSizeCurveCriteria.Checked)
                return true;

            if (this.chkSizeCurveTolerance.Checked)
                return true;

            if (this.chkSizeCurveSimilarStores.Checked)
                return true;
            //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report

            lblError.Text = MIDText.GetTextOnly(eMIDTextCode.msg_MustIncludeOneSubreport);
			return false;
		}

        // Begin TT#264 - RMatelic - Forecast Level and Forecast Type report problems
        private void SetCheckedState(bool isChecked)
        {
            try
            {
                 chkEligibility.Checked  = isChecked;
			     chkStoreGrades.Checked = isChecked;    
			     chkVelocityGrades.Checked = isChecked;     
    			 chkPurgeCriteria.Checked = isChecked;
			     chkModifiers.Checked = isChecked;  
			     chkAllocationMinMax.Checked = isChecked;
			     chkCapacity.Checked = isChecked;
			     chkForecastLevel.Checked = isChecked;
                 chkSimilarStore.Checked = isChecked;
		    	 chkStockMinMax.Checked = isChecked;
                 chkDailyPercents.Checked = isChecked;
		         chkForecastType.Checked = isChecked;
                 chkSizeCurveCriteria.Checked = isChecked;
                 chkSizeCurveTolerance.Checked = isChecked;
                 chkSizeCurveSimilarStores.Checked = isChecked;
                 chkCharacteristics.Checked = isChecked;
                 chkVSW.Checked = isChecked;
                 chkChainSetPercent.Checked = isChecked;

            }
            catch (Exception ex)
            {
                HandleException(ex);
            } 
        }    
        // End TT#264 
		#endregion

		private void frmReportNodePropertiesOverridesSetup_Load(object sender, System.EventArgs e)
		{
            // Begin TT#209 - RMatelic - Audit Reports do not allow drag/drop of merchandise into selection
            FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ReportsNodePropertiesOverrides);
            SetReadOnly(FunctionSecurity.AllowUpdate);
            if (FunctionSecurity.AllowUpdate)
            {
                eStoreGroupSelectType storeGroupSelectType;
                FunctionSecurityProfile storeUserAttrSecLvl = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminStoreAttributesUser);
                if (storeUserAttrSecLvl.AccessDenied)
                {
                    storeGroupSelectType = eStoreGroupSelectType.GlobalOnly;
                }
                else
                {
                    storeGroupSelectType = eStoreGroupSelectType.All;
                }
                _storeGroupList = StoreMgmt.StoreGroup_GetListViewList(storeGroupSelectType, false); //_SAB.StoreServerSession.GetStoreGroupListViewList(storeGroupSelectType, false);
            }    
            // End TT#209  
            SetText();  // TT#274 - RMatelic - Unrelated to specific issue
		}
       
        //Begin TT#274 - RMatelic - Unrelated to specific issue
        private void SetText()
        {
            try
            {
                this.Text = MIDText.GetTextOnly(eMIDTextCode.frm_NodePropertiesOverridesReport);
                this.lblMerchandise.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Merchandise);
                this.lblLowLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OverrideModel_Low_Level);
                this.lblStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StoreSingular);
                this.lblSet.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_OR) + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Set);  
                this.cmsIncludeSelectAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_SelectAllEntries);
                this.cmsIncludeClearAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ClearAllEntries);
                this.chkEligibility.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Eligibility);  
                this.chkModifiers.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Modifiers);
                this.chkSimilarStore.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Similar_Store);   
                this.chkStoreGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Store_Grades);
                this.chkAllocationMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AllocationMinMax);
                this.chkStockMinMax.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_StockMinMax);
                this.chkVelocityGrades.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Velocity_Grades);
                this.chkCapacity.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Capacity);
                this.chkDailyPercents.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_DailyPcts);
                this.chkPurgeCriteria.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PurgeCriteria);
                this.chkForecastLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_ForecastLevel);   
                this.chkForecastType.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_PlanType);   
                this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
                this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel); 			
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        //End TT#274  

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

		private void PopulateLowLevels()
		{
			try
			{
				HierarchyProfile hierProf;
				cboLowLevels.Items.Clear();
				
				if (NodeRID == Include.NoRID) return;

				HierarchyNodeProfile aHierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(NodeRID, false);
				
				if (aHierarchyNodeProfile != null)
				{
					cboLowLevels.Enabled = true;
					
					hierProf = SAB.HierarchyServerSession.GetHierarchyData(aHierarchyNodeProfile.HierarchyRID);
					if (hierProf.HierarchyType == eHierarchyType.organizational)
					{
						for (int i = aHierarchyNodeProfile.HomeHierarchyLevel + 1; i <= hierProf.HierarchyLevels.Count; i++)
						{
							HierarchyLevelProfile hlp = (HierarchyLevelProfile)hierProf.HierarchyLevels[i];
							cboLowLevels.Items.Add(
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

						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
							_longestHighestGuest = SAB.HierarchyServerSession.GetHighestGuestLevel(aHierarchyNodeProfile.Key);
						}
						int highestGuestLevel = _longestHighestGuest;

						// add guest levels to comboBox
						if ((highestGuestLevel != int.MaxValue) && (aHierarchyNodeProfile.HomeHierarchyType != eHierarchyType.alternate)) // TT#55 - KJohnson - Override Level option needs to reflect Low level already selected(in all review screens and methods with override level option)
						{
							for (int i = highestGuestLevel; i <= mainHierProf.HierarchyLevels.Count; i++)
							{
								if (i == 0)
								{
									cboLowLevels.Items.Add(
										new LowLevelCombo(eLowLevelsType.HierarchyLevel,
										0,
										0,
										"Root"));
								}
								else
								{
									HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHierProf.HierarchyLevels[i];
									cboLowLevels.Items.Add(
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
						if (_currentLowLevelNode != aHierarchyNodeProfile.Key)
						{
                            //BEGIN TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
                            //_longestBranch = SAB.HierarchyServerSession.GetLongestBranch(aHierarchyNodeProfile.Key);
                            DataTable hierarchyLevels = SAB.HierarchyServerSession.GetHierarchyDescendantLevels(aHierarchyNodeProfile.Key);
                            _longestBranch = hierarchyLevels.Rows.Count - 1;
                            //END TT#4689 - DOConnell - OTS Forecast - Multi-Level Low Levels not being populated correctly
						}
						int longestBranchCount = _longestBranch; 
						int offset = 0;
						for (int i = 0; i < longestBranchCount; i++)
						{
							++offset;
							cboLowLevels.Items.Add(
								new LowLevelCombo(eLowLevelsType.LevelOffset,
								offset,
								0,
								null));
						}
					}
					if (cboLowLevels.Items.Count > 0)
					{
						cboLowLevels.SelectedIndex = 0;
					}
					
					_currentLowLevelNode = aHierarchyNodeProfile.Key;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void txtMerchandise_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            txtBaseMerchandise_DragDrop(sender, e);
			PopulateLowLevels();
		}

        protected void txtMerchandise_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            txtBaseMerchandise_DragEnter(sender, e);
        }

        private void txtMerchandise_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

		private void txtStoreNumber_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
            Store_DragEnter(sender, e);
		}

        private void txtStoreNumber_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

		private void txtStoreNumber_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			try
			{
				TextBox txtStore = (TextBox)sender;
                ErrorProvider.SetError(txtStoreNumber, string.Empty);
                ErrorProvider.SetError(txtSet, string.Empty);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    txtStore.Text = cbList.ClipboardProfile.Text;
                    _storeRid = cbList.ClipboardProfile.Key;
                    txtSet.Text = "";
                }
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}

		}

        private void txtSet_DragEnter(object sender, DragEventArgs e)
        {
            StoreGroupLevel_DragEnter(sender, e);
        }

        private void txtSet_DragOver(object sender, DragEventArgs e)
        {
            Image_DragOver(sender, e);
        }

		private void txtSet_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
            TreeNodeClipboardList cbList;
			try
			{
				TextBox txtSet = (TextBox)sender;
                ErrorProvider.SetError(txtStoreNumber, string.Empty);
                ErrorProvider.SetError(txtSet, string.Empty);
                if (e.Data.GetDataPresent(typeof(TreeNodeClipboardList)))
                {
                    cbList = (TreeNodeClipboardList)e.Data.GetData(typeof(TreeNodeClipboardList));
                    txtSet.Text = cbList.ClipboardProfile.Node.Parent.Text + "/" + cbList.ClipboardProfile.Node.InternalText;
                    txtStoreNumber.Text = "";
                }
			}
			catch (BadDataInClipboardException)
			{
				MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_BadDataInClipboard),
					this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				HandleException(ex);
			}
		}

        private void txtSet_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                string errorMessage;

                TextBox txtSet = (TextBox)sender;
                ErrorProvider.SetError(txtStoreNumber, string.Empty);
                ErrorProvider.SetError(txtSet, string.Empty);
                if (txtSet.Text.Trim() == string.Empty)
                {
                    txtSet.Text = string.Empty;
                }
                else
                {
                    txtStoreNumber.Text = string.Empty;
                    int index = txtSet.Text.IndexOf("/");
                    if (index <= 0)
                    {
                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidAttributeAndSetEntry);
                        ErrorProvider.SetError(txtSet, errorMessage);
                        MessageBox.Show(errorMessage);
                        e.Cancel = true;
                    }
                    else
                    {
                        string storeGroup = txtSet.Text.Substring(0, index);
                        string storeSet = txtSet.Text.Substring(index + 1);
                        bool storeGroupValid = false;
                        bool storeSetValid = false;
                        int storeGroupRID = Include.NoRID;
                        foreach (StoreGroupProfile sgp in _storeGroupList)
                        {
                            if (storeGroup == sgp.Name)
                            {
                                storeGroupRID = sgp.Key;
                                storeGroupValid = true;
                                break;
                            }
                        }
                        if (!storeGroupValid)
                        {
                            errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidAttributeName);
                            ErrorProvider.SetError(txtSet, errorMessage);
                            MessageBox.Show(errorMessage);
                            e.Cancel = true;
                        }
                        else
                        {
                            ProfileList storeGroupLevelList = StoreMgmt.StoreGroup_GetLevelListViewList(storeGroupRID); //_SAB.StoreServerSession.GetStoreGroupLevelListViewList(storeGroupRID);
                            foreach (StoreGroupLevelListViewProfile setView in storeGroupLevelList)
                            {
                                if (setView.Name == storeSet)
                                {
                                    storeSetValid = true;
                                    break;
                                }
                            }
                            if (!storeSetValid)
                            {
                                errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidAttributeSetName);
                                ErrorProvider.SetError(txtSet, errorMessage);
                                MessageBox.Show(errorMessage);
                                e.Cancel = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            } 
        }

        private void txtStoreNumber_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                TextBox txtStoreNumber = (TextBox)sender;
                ErrorProvider.SetError(txtStoreNumber, string.Empty);
                ErrorProvider.SetError(txtSet, string.Empty);
                if (txtStoreNumber.Text.Trim() == string.Empty)
                {
                    txtStoreNumber.Text = string.Empty;
                }
                else
                {
                    txtSet.Text = string.Empty;
                    // Begin TT#271 - RMatelic -Dragging a store to store field gets "Invalid Store Field" error
                    string storeID = string.Empty;
                    int index = txtStoreNumber.Text.IndexOf("[");
                    if (index > 0)
                    {
                        storeID = txtStoreNumber.Text.Substring(0, index).Trim();
                    }
                    else
                    {
                        storeID = txtStoreNumber.Text.Trim();
                    }    
                    //StoreProfile sp = _SAB.StoreServerSession.GetStoreProfile(txtStoreNumber.Text);
                    StoreProfile sp = StoreMgmt.StoreProfile_Get(storeID); //_SAB.StoreServerSession.GetStoreProfile(storeID);
                    // End TT#271

                    if (sp.Key == Include.NoRID)
                    {
                        string errorMessage;
                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidStoreField);
                        ErrorProvider.SetError(txtStoreNumber, errorMessage);
                        MessageBox.Show(errorMessage);

                        e.Cancel = true;
                    }
                    else
                    {
                        txtStoreNumber.Text = sp.Text;
                    }	
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            } 
        }

        //BEGIN TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
        private void chkChainSetPercent_CheckedChanged(object sender, EventArgs e)
        {

                this.lblTimePeriod.Enabled = chkChainSetPercent.Checked;
                this.midDateRangeSelector1.Enabled = chkChainSetPercent.Checked;
        }
        private void midDateRangeSelector1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
        {
            if (midDateRangeSelector1.Tag != null)
            {
                ((CalendarDateSelector)midDateRangeSelector1.DateRangeForm).DateRangeRID = (int)midDateRangeSelector1.Tag;
            }
            midDateRangeSelector1.ShowSelector();
        }

        private void midDateRangeSelector1_Load(object sender, System.EventArgs e)
        {
            try
            {
                CalendarDateSelector frm = (CalendarDateSelector)CreateControl(typeof(CalendarDateSelector), new object[] { SAB });
                midDateRangeSelector1.DateRangeForm = frm;
                frm.RestrictToSingleDate = false;
                frm.AllowDynamicToCurrent = true;
                frm.AllowDynamicToPlan = false;
                frm.AllowDynamicToStoreOpen = false;
            }
            catch (Exception ex)
            {
                HandleException(ex, "midDateRangeSelector1_Load");
            }

        }

        private int chainSetPercentStartDateKey = -1;
        private int chainSetPercentEndDateKey = -1;
        private int chainSetPercentWeekCount = -1;
        private ProfileList chainSetPercentSelectedWeekRange = null;
        private void midDateRangeSelector1_OnSelection(object sender, MIDRetail.Windows.Controls.DateRangeSelectorEventArgs e)
        {
            try
            {
                if (!e.SelectionCanceled)
                {
                    DateRangeProfile dateRangeSelectedProfile = e.SelectedDateRange;
                    midDateRangeSelector1.Tag = dateRangeSelectedProfile.Key;
                    SetDynamicPicturesOnDateRangeControl(dateRangeSelectedProfile);

                    chainSetPercentStartDateKey = -1;
                    chainSetPercentEndDateKey = -1;
                    chainSetPercentWeekCount = -1;
                    chainSetPercentSelectedWeekRange = null;
                    if (dateRangeSelectedProfile.SelectedDateType != 0)
                    {
                        //get the fiscal week keys needed for the Chain Set Percent report
                        chainSetPercentSelectedWeekRange = _SAB.ApplicationServerSession.Calendar.GetWeekRange(dateRangeSelectedProfile, null);
                        chainSetPercentStartDateKey = chainSetPercentSelectedWeekRange.MinValue;
                        chainSetPercentEndDateKey = chainSetPercentSelectedWeekRange.MaxValue;
                        chainSetPercentWeekCount = chainSetPercentSelectedWeekRange.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex, "midDateRangeSelector1_OnSelection");
            }
        }
        private void SetDynamicPicturesOnDateRangeControl(DateRangeProfile dateRangeSelectedProfile)
        {
            //Display Dynamic picture or not
            if (dateRangeSelectedProfile != null)
                if (dateRangeSelectedProfile.DateRangeType == eCalendarRangeType.Dynamic)
                    midDateRangeSelector1.SetImage(ReoccurringImage);
                else
                    midDateRangeSelector1.SetImage(null);
            else
                midDateRangeSelector1.SetImage(null);

        }
        //END TT#436-MD -jsobek -Add the new items to the Node Properties Overrides Report
	}

}
