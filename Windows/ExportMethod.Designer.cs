using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class frmExportMethod
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				if (_chainVarChooser != null)
				{
					_chainVarChooser.Dispose();
				}

				if (_storeVarChooser != null)
				{
					_storeVarChooser.Dispose();
				}

				this.rdoChain.CheckedChanged -= new System.EventHandler(this.rdoChain_CheckedChanged);
				this.rdoStore.CheckedChanged -= new System.EventHandler(this.rdoStore_CheckedChanged);
				this.cboFilter.SelectionChangeCommitted -= new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboFilter.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboFilter_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

				this.cboVersion.SelectionChangeCommitted -= new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboVersion.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboVersion_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

				this.chkLowLevels.CheckedChanged -= new System.EventHandler(this.chkLowLevels_CheckedChanged);
				this.btnOverrideLowLevels.Click -= new System.EventHandler(this.btnOverrideLowLevels_Click);
				this.cboLowLevel.SelectionChangeCommitted -= new System.EventHandler(this.cboLowLevel_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboLowLevel.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboLowLevel_MIDComboBoxPropertiesChangedEvent);
                this.cboOverride.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboOverride_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control

				this.chkLowLevelsOnly.CheckedChanged -= new System.EventHandler(this.chkLowLevelsOnly_CheckedChanged);
				this.txtMerchandise.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
				this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
				this.txtMerchandise.Validated -= new System.EventHandler(this.txtMerchandise_Validated);
				this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
				this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
				this.drsTimePeriod.Load -= new System.EventHandler(this.drsTimePeriod_Load);
				this.drsTimePeriod.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.drsTimePeriod_ClickCellButton);
				this.drsTimePeriod.OnSelection -= new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsTimePeriod_OnSelection);
				this.txtConcurrentProcesses.TextChanged -= new System.EventHandler(this.txtConcurrentProcesses_TextChanged);
				this.txtFlagSuffix.TextChanged -= new System.EventHandler(this.txtFlagSuffix_TextChanged);
				this.chkCreateFlagFile.CheckedChanged -= new System.EventHandler(this.chkCreateFlagFile_CheckedChanged);
				this.txtSplitNumEntries.TextChanged -= new System.EventHandler(this.txtSplitNumEntries_TextChanged);
				this.rdoSplitNumEntries.CheckedChanged -= new System.EventHandler(this.rdoSplitNumEntries_CheckedChanged);
				this.rdoSplitMerchandise.CheckedChanged -= new System.EventHandler(this.rdoSplitMerchandise_CheckedChanged);
				this.rdoSplitNone.CheckedChanged -= new System.EventHandler(this.rdoSplitNone_CheckedChanged);
				this.chkTimeStamp.CheckedChanged -= new System.EventHandler(this.chkTimeStamp_CheckedChanged);
				this.chkDateStamp.CheckedChanged -= new System.EventHandler(this.chkDateStamp_CheckedChanged);
				this.btnBrowseFilePath.Click -= new System.EventHandler(this.btnBrowseFilePath_Click);
				this.txtFilePath.TextChanged -= new System.EventHandler(this.txtFilePath_TextChanged);
				this.txtDelimeter.TextChanged -= new System.EventHandler(this.txtDelimeter_TextChanged);
				this.rdoXML.CheckedChanged -= new System.EventHandler(this.rdoXML_CheckedChanged);
				this.rdoCSV.CheckedChanged -= new System.EventHandler(this.rdoCSV_CheckedChanged);
				this.chkDefaultSettings.CheckedChanged -= new System.EventHandler(this.chkDefaultSettings_CheckedChanged);
				this.chkShowIneligible.CheckedChanged -= new System.EventHandler(this.chkShowIneligible_CheckedChanged);
				this.chkPreinitValues.CheckedChanged -= new System.EventHandler(this.chkPreinitValues_CheckedChanged);
				//Begin Track #5395 - JScott - Add ability to discard zero values in Export
				this.chkExcludeZeroValues.CheckedChanged -= new System.EventHandler(this.chkExcludeZeroValues_CheckedChanged);
				//End Track #5395 - JScott - Add ability to discard zero values in Export
				this.chkCreateEndFile.CheckedChanged -= new System.EventHandler(this.chkCreateEndFile_CheckedChanged);
				this.txtEndSuffix.TextChanged -= new System.EventHandler(this.txtEndSuffix_TextChanged);
				this.rdoCalendar.CheckedChanged -= new System.EventHandler(this.rdoCalendar_CheckedChanged);
				this.rdoFiscal.CheckedChanged -= new System.EventHandler(this.rdoFiscal_CheckedChanged);
				this.txtCSVFileSuffix.TextChanged -= new System.EventHandler(this.txtCSVFileSuffix_TextChanged);

				if (ApplicationTransaction != null)
				{
					ApplicationTransaction.Dispose();
				}
			}

			base.Dispose(disposing);
		}

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
            this.Icons = new System.Windows.Forms.ImageList(this.components);
            this.tabGenAllocMethod = new System.Windows.Forms.TabControl();
            this.tabMethod = new System.Windows.Forms.TabPage();
            this.lblOptions = new System.Windows.Forms.Label();
            this.grpCriteria = new System.Windows.Forms.GroupBox();
            this.cboFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboVersion = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkShowIneligible = new System.Windows.Forms.CheckBox();
            this.rdoChain = new System.Windows.Forms.RadioButton();
            this.rdoStore = new System.Windows.Forms.RadioButton();
            this.lblStoreFilter = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.chkLowLevels = new System.Windows.Forms.CheckBox();
            this.grpLowLevels = new System.Windows.Forms.GroupBox();
            this.cboOverride = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboLowLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.btnOverrideLowLevels = new System.Windows.Forms.Button();
            this.lblLowLevel = new System.Windows.Forms.Label();
            this.chkLowLevelsOnly = new System.Windows.Forms.CheckBox();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.lblTimePeriod = new System.Windows.Forms.Label();
            this.drsTimePeriod = new MIDRetail.Windows.Controls.MIDDateRangeSelector();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.tabOptions = new System.Windows.Forms.TabControl();
            this.tpgVariables = new System.Windows.Forms.TabPage();
            this.tpgFormat = new System.Windows.Forms.TabPage();
            this.pnlFormat = new System.Windows.Forms.Panel();
            this.grpOutputFormat = new System.Windows.Forms.GroupBox();
            this.chkExcludeZeroValues = new System.Windows.Forms.CheckBox();
            this.lblDateFormat = new System.Windows.Forms.Label();
            this.chkPreinitValues = new System.Windows.Forms.CheckBox();
            this.rdoFiscal = new System.Windows.Forms.RadioButton();
            this.rdoCalendar = new System.Windows.Forms.RadioButton();
            this.grpOutputOptions = new System.Windows.Forms.GroupBox();
            this.txtEndSuffix = new System.Windows.Forms.TextBox();
            this.lblEndSuffix = new System.Windows.Forms.Label();
            this.chkCreateEndFile = new System.Windows.Forms.CheckBox();
            this.txtConcurrentProcesses = new System.Windows.Forms.TextBox();
            this.lblConcurrentProcesses = new System.Windows.Forms.Label();
            this.txtFlagSuffix = new System.Windows.Forms.TextBox();
            this.lblFlagSuffix = new System.Windows.Forms.Label();
            this.chkCreateFlagFile = new System.Windows.Forms.CheckBox();
            this.txtSplitNumEntries = new System.Windows.Forms.TextBox();
            this.rdoSplitNumEntries = new System.Windows.Forms.RadioButton();
            this.rdoSplitMerchandise = new System.Windows.Forms.RadioButton();
            this.rdoSplitNone = new System.Windows.Forms.RadioButton();
            this.lblSplitFiles = new System.Windows.Forms.Label();
            this.chkTimeStamp = new System.Windows.Forms.CheckBox();
            this.chkDateStamp = new System.Windows.Forms.CheckBox();
            this.lblAddToFileName = new System.Windows.Forms.Label();
            this.lblFileNameInfo = new System.Windows.Forms.Label();
            this.btnBrowseFilePath = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.grpFileFormat = new System.Windows.Forms.GroupBox();
            this.txtCSVFileSuffix = new System.Windows.Forms.TextBox();
            this.lblCSVFileSuffix = new System.Windows.Forms.Label();
            this.txtDelimeter = new System.Windows.Forms.TextBox();
            this.lblDelimeter = new System.Windows.Forms.Label();
            this.rdoXML = new System.Windows.Forms.RadioButton();
            this.rdoCSV = new System.Windows.Forms.RadioButton();
            this.chkDefaultSettings = new System.Windows.Forms.CheckBox();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fbdFilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.ttpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabGenAllocMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpCriteria.SuspendLayout();
            this.grpLowLevels.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tpgFormat.SuspendLayout();
            this.pnlFormat.SuspendLayout();
            this.grpOutputFormat.SuspendLayout();
            this.grpOutputOptions.SuspendLayout();
            this.grpFileFormat.SuspendLayout();
            this.tabProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(632, 648);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(544, 648);
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(16, 648);
            // 
            // txtDesc
            // 
            this.txtDesc.TabIndex = 2;
            // 
            // txtName
            // 
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(6, 18);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // Icons
            // 
            this.Icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.Icons.ImageSize = new System.Drawing.Size(16, 16);
            this.Icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabGenAllocMethod
            // 
            this.tabGenAllocMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabGenAllocMethod.Controls.Add(this.tabMethod);
            this.tabGenAllocMethod.Controls.Add(this.tabProperties);
            this.tabGenAllocMethod.Location = new System.Drawing.Point(36, 64);
            this.tabGenAllocMethod.Name = "tabGenAllocMethod";
            this.tabGenAllocMethod.SelectedIndex = 0;
            this.tabGenAllocMethod.Size = new System.Drawing.Size(656, 568);
            this.tabGenAllocMethod.TabIndex = 14;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.lblOptions);
            this.tabMethod.Controls.Add(this.grpCriteria);
            this.tabMethod.Controls.Add(this.tabOptions);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(648, 542);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // lblOptions
            // 
            this.lblOptions.Location = new System.Drawing.Point(8, 192);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(128, 24);
            this.lblOptions.TabIndex = 3;
            this.lblOptions.Text = "Export File Options";
            this.lblOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpCriteria
            // 
            this.grpCriteria.Controls.Add(this.cboFilter);
            this.grpCriteria.Controls.Add(this.cboVersion);
            this.grpCriteria.Controls.Add(this.chkShowIneligible);
            this.grpCriteria.Controls.Add(this.rdoChain);
            this.grpCriteria.Controls.Add(this.rdoStore);
            this.grpCriteria.Controls.Add(this.lblStoreFilter);
            this.grpCriteria.Controls.Add(this.lblVersion);
            this.grpCriteria.Controls.Add(this.chkLowLevels);
            this.grpCriteria.Controls.Add(this.grpLowLevels);
            this.grpCriteria.Controls.Add(this.txtMerchandise);
            this.grpCriteria.Controls.Add(this.lblTimePeriod);
            this.grpCriteria.Controls.Add(this.drsTimePeriod);
            this.grpCriteria.Controls.Add(this.lblMerchandise);
            this.grpCriteria.Location = new System.Drawing.Point(8, 8);
            this.grpCriteria.Name = "grpCriteria";
            this.grpCriteria.Size = new System.Drawing.Size(616, 176);
            this.grpCriteria.TabIndex = 2;
            this.grpCriteria.TabStop = false;
            this.grpCriteria.Text = "Export Criteria";
            // 
            // cboFilter
            // 
            this.cboFilter.AllowDrop = true;
            this.cboFilter.AutoAdjust = true;
            this.cboFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboFilter.DataSource = null;
            this.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFilter.DropDownWidth = 200;
            this.cboFilter.FormattingEnabled = false;
            this.cboFilter.IgnoreFocusLost = false;
            this.cboFilter.ItemHeight = 13;
            this.cboFilter.Location = new System.Drawing.Point(96, 144);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.MaxDropDownItems = 25;
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.SetToolTip = "";
            this.cboFilter.Size = new System.Drawing.Size(200, 21);
            this.cboFilter.TabIndex = 25;
            this.cboFilter.Tag = null;
            this.cboFilter.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboFilter_MIDComboBoxPropertiesChangedEvent);
            this.cboFilter.SelectionChangeCommitted += new System.EventHandler(this.cboFilter_SelectionChangeCommitted);
            this.cboFilter.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboFilter.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboFilter.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // cboVersion
            // 
            this.cboVersion.AutoAdjust = true;
            this.cboVersion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboVersion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboVersion.DataSource = null;
            this.cboVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVersion.DropDownWidth = 200;
            this.cboVersion.FormattingEnabled = false;
            this.cboVersion.IgnoreFocusLost = false;
            this.cboVersion.ItemHeight = 13;
            this.cboVersion.Location = new System.Drawing.Point(96, 80);
            this.cboVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboVersion.MaxDropDownItems = 25;
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.SetToolTip = "";
            this.cboVersion.Size = new System.Drawing.Size(200, 21);
            this.cboVersion.TabIndex = 21;
            this.cboVersion.Tag = null;
            this.cboVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            this.cboVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // chkShowIneligible
            // 
            this.chkShowIneligible.Location = new System.Drawing.Point(320, 144);
            this.chkShowIneligible.Name = "chkShowIneligible";
            this.chkShowIneligible.Size = new System.Drawing.Size(184, 24);
            this.chkShowIneligible.TabIndex = 28;
            this.chkShowIneligible.Text = "Extract Ineligible Stores";
            this.chkShowIneligible.CheckedChanged += new System.EventHandler(this.chkShowIneligible_CheckedChanged);
            // 
            // rdoChain
            // 
            this.rdoChain.Location = new System.Drawing.Point(96, 16);
            this.rdoChain.Name = "rdoChain";
            this.rdoChain.Size = new System.Drawing.Size(96, 24);
            this.rdoChain.TabIndex = 27;
            this.rdoChain.Text = "Chain Data";
            this.rdoChain.CheckedChanged += new System.EventHandler(this.rdoChain_CheckedChanged);
            // 
            // rdoStore
            // 
            this.rdoStore.Location = new System.Drawing.Point(200, 16);
            this.rdoStore.Name = "rdoStore";
            this.rdoStore.Size = new System.Drawing.Size(96, 24);
            this.rdoStore.TabIndex = 0;
            this.rdoStore.Text = "Store Data";
            this.rdoStore.CheckedChanged += new System.EventHandler(this.rdoStore_CheckedChanged);
            // 
            // lblStoreFilter
            // 
            this.lblStoreFilter.Location = new System.Drawing.Point(16, 146);
            this.lblStoreFilter.Name = "lblStoreFilter";
            this.lblStoreFilter.Size = new System.Drawing.Size(72, 16);
            this.lblStoreFilter.TabIndex = 26;
            this.lblStoreFilter.Text = "Store Filter";
            this.lblStoreFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(16, 82);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 16);
            this.lblVersion.TabIndex = 22;
            this.lblVersion.Text = "Version";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkLowLevels
            // 
            this.chkLowLevels.Location = new System.Drawing.Point(320, 16);
            this.chkLowLevels.Name = "chkLowLevels";
            this.chkLowLevels.Size = new System.Drawing.Size(16, 24);
            this.chkLowLevels.TabIndex = 20;
            this.chkLowLevels.CheckedChanged += new System.EventHandler(this.chkLowLevels_CheckedChanged);
            // 
            // grpLowLevels
            // 
            this.grpLowLevels.Controls.Add(this.cboOverride);
            this.grpLowLevels.Controls.Add(this.cboLowLevel);
            this.grpLowLevels.Controls.Add(this.btnOverrideLowLevels);
            this.grpLowLevels.Controls.Add(this.lblLowLevel);
            this.grpLowLevels.Controls.Add(this.chkLowLevelsOnly);
            this.grpLowLevels.Location = new System.Drawing.Point(344, 16);
            this.grpLowLevels.Name = "grpLowLevels";
            this.grpLowLevels.Size = new System.Drawing.Size(264, 120);
            this.grpLowLevels.TabIndex = 19;
            this.grpLowLevels.TabStop = false;
            this.grpLowLevels.Text = "Low Levels";
            // 
            // cboOverride
            // 
            this.cboOverride.AutoAdjust = true;
            this.cboOverride.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboOverride.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboOverride.DataSource = null;
            this.cboOverride.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOverride.DropDownWidth = 126;
            this.cboOverride.FormattingEnabled = false;
            this.cboOverride.IgnoreFocusLost = false;
            this.cboOverride.ItemHeight = 13;
            this.cboOverride.Location = new System.Drawing.Point(132, 80);
            this.cboOverride.Margin = new System.Windows.Forms.Padding(0);
            this.cboOverride.MaxDropDownItems = 25;
            this.cboOverride.Name = "cboOverride";
            this.cboOverride.SetToolTip = "";
            this.cboOverride.Size = new System.Drawing.Size(126, 21);
            this.cboOverride.TabIndex = 28;
            this.cboOverride.Tag = null;
            this.cboOverride.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboOverride_MIDComboBoxPropertiesChangedEvent);
            this.cboOverride.SelectionChangeCommitted += new System.EventHandler(this.cboOverride_SelectionChangeCommitted);
            // 
            // cboLowLevel
            // 
            this.cboLowLevel.AutoAdjust = true;
            this.cboLowLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLowLevel.DataSource = null;
            this.cboLowLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevel.DropDownWidth = 170;
            this.cboLowLevel.FormattingEnabled = false;
            this.cboLowLevel.IgnoreFocusLost = true;
            this.cboLowLevel.ItemHeight = 13;
            this.cboLowLevel.Location = new System.Drawing.Point(88, 48);
            this.cboLowLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboLowLevel.MaxDropDownItems = 25;
            this.cboLowLevel.Name = "cboLowLevel";
            this.cboLowLevel.SetToolTip = "";
            this.cboLowLevel.Size = new System.Drawing.Size(170, 21);
            this.cboLowLevel.TabIndex = 1;
            this.cboLowLevel.Tag = null;
            this.cboLowLevel.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboLowLevel_MIDComboBoxPropertiesChangedEvent);
            this.cboLowLevel.SelectionChangeCommitted += new System.EventHandler(this.cboLowLevel_SelectionChangeCommitted);
            // 
            // btnOverrideLowLevels
            // 
            this.btnOverrideLowLevels.Location = new System.Drawing.Point(14, 80);
            this.btnOverrideLowLevels.Name = "btnOverrideLowLevels";
            this.btnOverrideLowLevels.Size = new System.Drawing.Size(114, 24);
            this.btnOverrideLowLevels.TabIndex = 3;
            this.btnOverrideLowLevels.Text = "Override Low Levels";
            this.btnOverrideLowLevels.Click += new System.EventHandler(this.btnOverrideLowLevels_Click);
            // 
            // lblLowLevel
            // 
            this.lblLowLevel.Location = new System.Drawing.Point(16, 50);
            this.lblLowLevel.Name = "lblLowLevel";
            this.lblLowLevel.Size = new System.Drawing.Size(64, 16);
            this.lblLowLevel.TabIndex = 2;
            this.lblLowLevel.Text = "Low Level";
            this.lblLowLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkLowLevelsOnly
            // 
            this.chkLowLevelsOnly.Location = new System.Drawing.Point(16, 16);
            this.chkLowLevelsOnly.Name = "chkLowLevelsOnly";
            this.chkLowLevelsOnly.Size = new System.Drawing.Size(200, 24);
            this.chkLowLevelsOnly.TabIndex = 0;
            this.chkLowLevelsOnly.Text = "Low Levels Only";
            this.chkLowLevelsOnly.CheckedChanged += new System.EventHandler(this.chkLowLevelsOnly_CheckedChanged);
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Location = new System.Drawing.Point(96, 48);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(200, 20);
            this.txtMerchandise.TabIndex = 18;
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(16, 116);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 6;
            this.lblTimePeriod.Text = "Time Period";
            this.lblTimePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // drsTimePeriod
            // 
            this.drsTimePeriod.DateRangeForm = null;
            this.drsTimePeriod.DateRangeRID = 0;
            this.drsTimePeriod.Enabled = false;
            this.drsTimePeriod.Location = new System.Drawing.Point(96, 112);
            this.drsTimePeriod.Name = "drsTimePeriod";
            this.drsTimePeriod.Size = new System.Drawing.Size(200, 24);
            this.drsTimePeriod.TabIndex = 15;
            this.drsTimePeriod.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsTimePeriod_OnSelection);
            this.drsTimePeriod.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.drsTimePeriod_ClickCellButton);
            this.drsTimePeriod.Load += new System.EventHandler(this.drsTimePeriod_Load);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(16, 50);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 6;
            this.lblMerchandise.Text = "Merchandise";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabOptions
            // 
            this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOptions.Controls.Add(this.tpgVariables);
            this.tabOptions.Controls.Add(this.tpgFormat);
            this.tabOptions.Location = new System.Drawing.Point(8, 216);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(632, 320);
            this.tabOptions.TabIndex = 0;
            // 
            // tpgVariables
            // 
            this.tpgVariables.Location = new System.Drawing.Point(4, 22);
            this.tpgVariables.Name = "tpgVariables";
            this.tpgVariables.Size = new System.Drawing.Size(624, 294);
            this.tpgVariables.TabIndex = 0;
            this.tpgVariables.Text = "Variables";
            // 
            // tpgFormat
            // 
            this.tpgFormat.Controls.Add(this.pnlFormat);
            this.tpgFormat.Location = new System.Drawing.Point(4, 22);
            this.tpgFormat.Name = "tpgFormat";
            this.tpgFormat.Size = new System.Drawing.Size(624, 294);
            this.tpgFormat.TabIndex = 1;
            this.tpgFormat.Text = "Format";
            // 
            // pnlFormat
            // 
            this.pnlFormat.AutoScroll = true;
            this.pnlFormat.Controls.Add(this.grpOutputFormat);
            this.pnlFormat.Controls.Add(this.grpOutputOptions);
            this.pnlFormat.Controls.Add(this.grpFileFormat);
            this.pnlFormat.Controls.Add(this.chkDefaultSettings);
            this.pnlFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormat.Location = new System.Drawing.Point(0, 0);
            this.pnlFormat.Name = "pnlFormat";
            this.pnlFormat.Size = new System.Drawing.Size(624, 294);
            this.pnlFormat.TabIndex = 0;
            // 
            // grpOutputFormat
            // 
            this.grpOutputFormat.Controls.Add(this.chkExcludeZeroValues);
            this.grpOutputFormat.Controls.Add(this.lblDateFormat);
            this.grpOutputFormat.Controls.Add(this.chkPreinitValues);
            this.grpOutputFormat.Controls.Add(this.rdoFiscal);
            this.grpOutputFormat.Controls.Add(this.rdoCalendar);
            this.grpOutputFormat.Location = new System.Drawing.Point(312, 8);
            this.grpOutputFormat.Name = "grpOutputFormat";
            this.grpOutputFormat.Size = new System.Drawing.Size(280, 104);
            this.grpOutputFormat.TabIndex = 4;
            this.grpOutputFormat.TabStop = false;
            this.grpOutputFormat.Text = "Output Format";
            // 
            // chkExcludeZeroValues
            // 
            this.chkExcludeZeroValues.Location = new System.Drawing.Point(104, 70);
            this.chkExcludeZeroValues.Name = "chkExcludeZeroValues";
            this.chkExcludeZeroValues.Size = new System.Drawing.Size(152, 24);
            this.chkExcludeZeroValues.TabIndex = 5;
            this.chkExcludeZeroValues.Text = "Exclude Zero Values";
            this.ttpToolTip.SetToolTip(this.chkExcludeZeroValues, "Indicates whether the pre-inited (raw database) values should be extracted");
            this.chkExcludeZeroValues.CheckedChanged += new System.EventHandler(this.chkExcludeZeroValues_CheckedChanged);
            // 
            // lblDateFormat
            // 
            this.lblDateFormat.Location = new System.Drawing.Point(8, 24);
            this.lblDateFormat.Name = "lblDateFormat";
            this.lblDateFormat.Size = new System.Drawing.Size(80, 23);
            this.lblDateFormat.TabIndex = 4;
            this.lblDateFormat.Text = "Date Format";
            this.lblDateFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPreinitValues
            // 
            this.chkPreinitValues.Location = new System.Drawing.Point(104, 48);
            this.chkPreinitValues.Name = "chkPreinitValues";
            this.chkPreinitValues.Size = new System.Drawing.Size(152, 24);
            this.chkPreinitValues.TabIndex = 3;
            this.chkPreinitValues.Text = "Extract Pre-Init Values";
            this.ttpToolTip.SetToolTip(this.chkPreinitValues, "Indicates whether the pre-inited (raw database) values should be extracted");
            this.chkPreinitValues.CheckedChanged += new System.EventHandler(this.chkPreinitValues_CheckedChanged);
            // 
            // rdoFiscal
            // 
            this.rdoFiscal.Location = new System.Drawing.Point(192, 24);
            this.rdoFiscal.Name = "rdoFiscal";
            this.rdoFiscal.Size = new System.Drawing.Size(80, 24);
            this.rdoFiscal.TabIndex = 3;
            this.rdoFiscal.Text = "Fiscal";
            this.rdoFiscal.CheckedChanged += new System.EventHandler(this.rdoFiscal_CheckedChanged);
            // 
            // rdoCalendar
            // 
            this.rdoCalendar.Location = new System.Drawing.Point(104, 24);
            this.rdoCalendar.Name = "rdoCalendar";
            this.rdoCalendar.Size = new System.Drawing.Size(80, 24);
            this.rdoCalendar.TabIndex = 2;
            this.rdoCalendar.Text = "Calendar";
            this.rdoCalendar.CheckedChanged += new System.EventHandler(this.rdoCalendar_CheckedChanged);
            // 
            // grpOutputOptions
            // 
            this.grpOutputOptions.Controls.Add(this.txtEndSuffix);
            this.grpOutputOptions.Controls.Add(this.lblEndSuffix);
            this.grpOutputOptions.Controls.Add(this.chkCreateEndFile);
            this.grpOutputOptions.Controls.Add(this.txtConcurrentProcesses);
            this.grpOutputOptions.Controls.Add(this.lblConcurrentProcesses);
            this.grpOutputOptions.Controls.Add(this.txtFlagSuffix);
            this.grpOutputOptions.Controls.Add(this.lblFlagSuffix);
            this.grpOutputOptions.Controls.Add(this.chkCreateFlagFile);
            this.grpOutputOptions.Controls.Add(this.txtSplitNumEntries);
            this.grpOutputOptions.Controls.Add(this.rdoSplitNumEntries);
            this.grpOutputOptions.Controls.Add(this.rdoSplitMerchandise);
            this.grpOutputOptions.Controls.Add(this.rdoSplitNone);
            this.grpOutputOptions.Controls.Add(this.lblSplitFiles);
            this.grpOutputOptions.Controls.Add(this.chkTimeStamp);
            this.grpOutputOptions.Controls.Add(this.chkDateStamp);
            this.grpOutputOptions.Controls.Add(this.lblAddToFileName);
            this.grpOutputOptions.Controls.Add(this.lblFileNameInfo);
            this.grpOutputOptions.Controls.Add(this.btnBrowseFilePath);
            this.grpOutputOptions.Controls.Add(this.txtFilePath);
            this.grpOutputOptions.Controls.Add(this.lblFilePath);
            this.grpOutputOptions.Location = new System.Drawing.Point(8, 120);
            this.grpOutputOptions.Name = "grpOutputOptions";
            this.grpOutputOptions.Size = new System.Drawing.Size(592, 168);
            this.grpOutputOptions.TabIndex = 2;
            this.grpOutputOptions.TabStop = false;
            this.grpOutputOptions.Text = "Output Options";
            // 
            // txtEndSuffix
            // 
            this.txtEndSuffix.Location = new System.Drawing.Point(480, 136);
            this.txtEndSuffix.Name = "txtEndSuffix";
            this.txtEndSuffix.Size = new System.Drawing.Size(100, 20);
            this.txtEndSuffix.TabIndex = 19;
            this.txtEndSuffix.TextChanged += new System.EventHandler(this.txtEndSuffix_TextChanged);
            // 
            // lblEndSuffix
            // 
            this.lblEndSuffix.Location = new System.Drawing.Point(424, 136);
            this.lblEndSuffix.Name = "lblEndSuffix";
            this.lblEndSuffix.Size = new System.Drawing.Size(48, 23);
            this.lblEndSuffix.TabIndex = 18;
            this.lblEndSuffix.Text = "Suffix";
            this.lblEndSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCreateEndFile
            // 
            this.chkCreateEndFile.Location = new System.Drawing.Point(304, 136);
            this.chkCreateEndFile.Name = "chkCreateEndFile";
            this.chkCreateEndFile.Size = new System.Drawing.Size(112, 24);
            this.chkCreateEndFile.TabIndex = 17;
            this.chkCreateEndFile.Text = "Create End File";
            this.chkCreateEndFile.CheckedChanged += new System.EventHandler(this.chkCreateEndFile_CheckedChanged);
            // 
            // txtConcurrentProcesses
            // 
            this.txtConcurrentProcesses.Location = new System.Drawing.Point(392, 112);
            this.txtConcurrentProcesses.Name = "txtConcurrentProcesses";
            this.txtConcurrentProcesses.Size = new System.Drawing.Size(100, 20);
            this.txtConcurrentProcesses.TabIndex = 16;
            this.txtConcurrentProcesses.TextChanged += new System.EventHandler(this.txtConcurrentProcesses_TextChanged);
            // 
            // lblConcurrentProcesses
            // 
            this.lblConcurrentProcesses.Location = new System.Drawing.Point(256, 112);
            this.lblConcurrentProcesses.Name = "lblConcurrentProcesses";
            this.lblConcurrentProcesses.Size = new System.Drawing.Size(120, 23);
            this.lblConcurrentProcesses.TabIndex = 15;
            this.lblConcurrentProcesses.Text = "Concurrent Processes";
            this.lblConcurrentProcesses.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFlagSuffix
            // 
            this.txtFlagSuffix.Location = new System.Drawing.Point(184, 136);
            this.txtFlagSuffix.Name = "txtFlagSuffix";
            this.txtFlagSuffix.Size = new System.Drawing.Size(100, 20);
            this.txtFlagSuffix.TabIndex = 14;
            this.txtFlagSuffix.TextChanged += new System.EventHandler(this.txtFlagSuffix_TextChanged);
            // 
            // lblFlagSuffix
            // 
            this.lblFlagSuffix.Location = new System.Drawing.Point(128, 136);
            this.lblFlagSuffix.Name = "lblFlagSuffix";
            this.lblFlagSuffix.Size = new System.Drawing.Size(48, 23);
            this.lblFlagSuffix.TabIndex = 13;
            this.lblFlagSuffix.Text = "Suffix";
            this.lblFlagSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkCreateFlagFile
            // 
            this.chkCreateFlagFile.Location = new System.Drawing.Point(8, 136);
            this.chkCreateFlagFile.Name = "chkCreateFlagFile";
            this.chkCreateFlagFile.Size = new System.Drawing.Size(112, 24);
            this.chkCreateFlagFile.TabIndex = 12;
            this.chkCreateFlagFile.Text = "Create Flag File";
            this.chkCreateFlagFile.CheckedChanged += new System.EventHandler(this.chkCreateFlagFile_CheckedChanged);
            // 
            // txtSplitNumEntries
            // 
            this.txtSplitNumEntries.Location = new System.Drawing.Point(392, 88);
            this.txtSplitNumEntries.Name = "txtSplitNumEntries";
            this.txtSplitNumEntries.Size = new System.Drawing.Size(100, 20);
            this.txtSplitNumEntries.TabIndex = 11;
            this.txtSplitNumEntries.TextChanged += new System.EventHandler(this.txtSplitNumEntries_TextChanged);
            // 
            // rdoSplitNumEntries
            // 
            this.rdoSplitNumEntries.Location = new System.Drawing.Point(264, 88);
            this.rdoSplitNumEntries.Name = "rdoSplitNumEntries";
            this.rdoSplitNumEntries.Size = new System.Drawing.Size(128, 24);
            this.rdoSplitNumEntries.TabIndex = 10;
            this.rdoSplitNumEntries.Text = "Number of Entries";
            this.rdoSplitNumEntries.CheckedChanged += new System.EventHandler(this.rdoSplitNumEntries_CheckedChanged);
            // 
            // rdoSplitMerchandise
            // 
            this.rdoSplitMerchandise.Location = new System.Drawing.Point(176, 88);
            this.rdoSplitMerchandise.Name = "rdoSplitMerchandise";
            this.rdoSplitMerchandise.Size = new System.Drawing.Size(96, 24);
            this.rdoSplitMerchandise.TabIndex = 9;
            this.rdoSplitMerchandise.Text = "Merchandise";
            this.rdoSplitMerchandise.CheckedChanged += new System.EventHandler(this.rdoSplitMerchandise_CheckedChanged);
            // 
            // rdoSplitNone
            // 
            this.rdoSplitNone.Location = new System.Drawing.Point(112, 88);
            this.rdoSplitNone.Name = "rdoSplitNone";
            this.rdoSplitNone.Size = new System.Drawing.Size(64, 24);
            this.rdoSplitNone.TabIndex = 8;
            this.rdoSplitNone.Text = "None";
            this.rdoSplitNone.CheckedChanged += new System.EventHandler(this.rdoSplitNone_CheckedChanged);
            // 
            // lblSplitFiles
            // 
            this.lblSplitFiles.Location = new System.Drawing.Point(8, 88);
            this.lblSplitFiles.Name = "lblSplitFiles";
            this.lblSplitFiles.Size = new System.Drawing.Size(88, 23);
            this.lblSplitFiles.TabIndex = 7;
            this.lblSplitFiles.Text = "Split Files by:";
            this.lblSplitFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkTimeStamp
            // 
            this.chkTimeStamp.Location = new System.Drawing.Point(200, 64);
            this.chkTimeStamp.Name = "chkTimeStamp";
            this.chkTimeStamp.Size = new System.Drawing.Size(88, 24);
            this.chkTimeStamp.TabIndex = 6;
            this.chkTimeStamp.Text = "Time Stamp";
            this.chkTimeStamp.CheckedChanged += new System.EventHandler(this.chkTimeStamp_CheckedChanged);
            // 
            // chkDateStamp
            // 
            this.chkDateStamp.Location = new System.Drawing.Point(112, 64);
            this.chkDateStamp.Name = "chkDateStamp";
            this.chkDateStamp.Size = new System.Drawing.Size(88, 24);
            this.chkDateStamp.TabIndex = 5;
            this.chkDateStamp.Text = "Date Stamp";
            this.chkDateStamp.CheckedChanged += new System.EventHandler(this.chkDateStamp_CheckedChanged);
            // 
            // lblAddToFileName
            // 
            this.lblAddToFileName.Location = new System.Drawing.Point(8, 64);
            this.lblAddToFileName.Name = "lblAddToFileName";
            this.lblAddToFileName.Size = new System.Drawing.Size(104, 23);
            this.lblAddToFileName.TabIndex = 4;
            this.lblAddToFileName.Text = "Add to File Name:";
            this.lblAddToFileName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFileNameInfo
            // 
            this.lblFileNameInfo.Location = new System.Drawing.Point(96, 40);
            this.lblFileNameInfo.Name = "lblFileNameInfo";
            this.lblFileNameInfo.Size = new System.Drawing.Size(336, 23);
            this.lblFileNameInfo.TabIndex = 3;
            this.lblFileNameInfo.Text = "File Name will be generated from Merchandise and Level";
            this.lblFileNameInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowseFilePath
            // 
            this.btnBrowseFilePath.Location = new System.Drawing.Point(408, 16);
            this.btnBrowseFilePath.Name = "btnBrowseFilePath";
            this.btnBrowseFilePath.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFilePath.TabIndex = 2;
            this.btnBrowseFilePath.Text = "Browse";
            this.btnBrowseFilePath.Click += new System.EventHandler(this.btnBrowseFilePath_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(96, 16);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(304, 20);
            this.txtFilePath.TabIndex = 1;
            this.txtFilePath.TextChanged += new System.EventHandler(this.txtFilePath_TextChanged);
            // 
            // lblFilePath
            // 
            this.lblFilePath.Location = new System.Drawing.Point(16, 16);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(72, 23);
            this.lblFilePath.TabIndex = 0;
            this.lblFilePath.Text = "File Path";
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpFileFormat
            // 
            this.grpFileFormat.Controls.Add(this.txtCSVFileSuffix);
            this.grpFileFormat.Controls.Add(this.lblCSVFileSuffix);
            this.grpFileFormat.Controls.Add(this.txtDelimeter);
            this.grpFileFormat.Controls.Add(this.lblDelimeter);
            this.grpFileFormat.Controls.Add(this.rdoXML);
            this.grpFileFormat.Controls.Add(this.rdoCSV);
            this.grpFileFormat.Location = new System.Drawing.Point(8, 32);
            this.grpFileFormat.Name = "grpFileFormat";
            this.grpFileFormat.Size = new System.Drawing.Size(296, 80);
            this.grpFileFormat.TabIndex = 1;
            this.grpFileFormat.TabStop = false;
            this.grpFileFormat.Text = "File Format";
            // 
            // txtCSVFileSuffix
            // 
            this.txtCSVFileSuffix.Location = new System.Drawing.Point(184, 48);
            this.txtCSVFileSuffix.Name = "txtCSVFileSuffix";
            this.txtCSVFileSuffix.Size = new System.Drawing.Size(100, 20);
            this.txtCSVFileSuffix.TabIndex = 5;
            this.txtCSVFileSuffix.TextChanged += new System.EventHandler(this.txtCSVFileSuffix_TextChanged);
            // 
            // lblCSVFileSuffix
            // 
            this.lblCSVFileSuffix.Location = new System.Drawing.Point(112, 48);
            this.lblCSVFileSuffix.Name = "lblCSVFileSuffix";
            this.lblCSVFileSuffix.Size = new System.Drawing.Size(64, 23);
            this.lblCSVFileSuffix.TabIndex = 4;
            this.lblCSVFileSuffix.Text = "File Suffix";
            this.lblCSVFileSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtDelimeter
            // 
            this.txtDelimeter.Location = new System.Drawing.Point(184, 24);
            this.txtDelimeter.Name = "txtDelimeter";
            this.txtDelimeter.Size = new System.Drawing.Size(32, 20);
            this.txtDelimeter.TabIndex = 3;
            this.txtDelimeter.TextChanged += new System.EventHandler(this.txtDelimeter_TextChanged);
            // 
            // lblDelimeter
            // 
            this.lblDelimeter.Location = new System.Drawing.Point(112, 24);
            this.lblDelimeter.Name = "lblDelimeter";
            this.lblDelimeter.Size = new System.Drawing.Size(64, 23);
            this.lblDelimeter.TabIndex = 2;
            this.lblDelimeter.Text = "Delimeter";
            this.lblDelimeter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rdoXML
            // 
            this.rdoXML.Location = new System.Drawing.Point(16, 24);
            this.rdoXML.Name = "rdoXML";
            this.rdoXML.Size = new System.Drawing.Size(88, 24);
            this.rdoXML.TabIndex = 1;
            this.rdoXML.Text = "XML File";
            this.rdoXML.CheckedChanged += new System.EventHandler(this.rdoXML_CheckedChanged);
            // 
            // rdoCSV
            // 
            this.rdoCSV.Location = new System.Drawing.Point(16, 48);
            this.rdoCSV.Name = "rdoCSV";
            this.rdoCSV.Size = new System.Drawing.Size(88, 24);
            this.rdoCSV.TabIndex = 0;
            this.rdoCSV.Text = "CSV File";
            this.rdoCSV.CheckedChanged += new System.EventHandler(this.rdoCSV_CheckedChanged);
            // 
            // chkDefaultSettings
            // 
            this.chkDefaultSettings.Location = new System.Drawing.Point(8, 8);
            this.chkDefaultSettings.Name = "chkDefaultSettings";
            this.chkDefaultSettings.Size = new System.Drawing.Size(160, 24);
            this.chkDefaultSettings.TabIndex = 0;
            this.chkDefaultSettings.Text = "User Default Settings";
            this.chkDefaultSettings.CheckedChanged += new System.EventHandler(this.chkDefaultSettings_CheckedChanged);
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(648, 542);
            this.tabProperties.TabIndex = 0;
            this.tabProperties.Text = "Properties";
            this.tabProperties.Visible = false;
            // 
            // ugWorkflows
            // 
            this.ugWorkflows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugWorkflows.DisplayLayout.Appearance = appearance1;
            this.ugWorkflows.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugWorkflows.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugWorkflows.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugWorkflows.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugWorkflows.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugWorkflows.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugWorkflows.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugWorkflows.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugWorkflows.Location = new System.Drawing.Point(16, 19);
            this.ugWorkflows.Name = "ugWorkflows";
            this.ugWorkflows.Size = new System.Drawing.Size(616, 505);
            this.ugWorkflows.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 50);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // frmExportMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 686);
            this.Controls.Add(this.tabGenAllocMethod);
            this.Name = "frmExportMethod";
            this.Text = "Export Method";
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnProcess, 0);
            this.Controls.SetChildIndex(this.pnlGlobalUser, 0);
            this.Controls.SetChildIndex(this.txtDesc, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.tabGenAllocMethod, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.pnlGlobalUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.tabGenAllocMethod.ResumeLayout(false);
            this.tabMethod.ResumeLayout(false);
            this.grpCriteria.ResumeLayout(false);
            this.grpCriteria.PerformLayout();
            this.grpLowLevels.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tpgFormat.ResumeLayout(false);
            this.pnlFormat.ResumeLayout(false);
            this.grpOutputFormat.ResumeLayout(false);
            this.grpOutputOptions.ResumeLayout(false);
            this.grpOutputOptions.PerformLayout();
            this.grpFileFormat.ResumeLayout(false);
            this.grpFileFormat.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.PictureBox pictureBox1;
		private MIDRetail.Windows.Controls.MIDDateRangeSelector drsTimePeriod;
		private System.Windows.Forms.TabControl tabGenAllocMethod;
		private System.Windows.Forms.Label lblMerchandise;
		private System.Windows.Forms.GroupBox grpCriteria;
		private System.Windows.Forms.TextBox txtMerchandise;
		private System.Windows.Forms.GroupBox grpLowLevels;
		private System.Windows.Forms.CheckBox chkLowLevels;
		private System.Windows.Forms.CheckBox chkLowLevelsOnly;
		private System.Windows.Forms.Label lblLowLevel;
		private System.Windows.Forms.Label lblTimePeriod;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblStoreFilter;
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.TabPage tpgVariables;
		private System.Windows.Forms.TabPage tpgFormat;
		private System.Windows.Forms.Label lblOptions;
		private System.Windows.Forms.Button btnOverrideLowLevels;
		private System.Windows.Forms.Panel pnlFormat;
		private System.Windows.Forms.CheckBox chkDefaultSettings;
		private System.Windows.Forms.GroupBox grpFileFormat;
		private System.Windows.Forms.RadioButton rdoCSV;
		private System.Windows.Forms.Label lblDelimeter;
		private System.Windows.Forms.TextBox txtDelimeter;
		private System.Windows.Forms.GroupBox grpOutputOptions;
		private System.Windows.Forms.Label lblFilePath;
		private System.Windows.Forms.TextBox txtFilePath;
		private System.Windows.Forms.Button btnBrowseFilePath;
		private System.Windows.Forms.Label lblFileNameInfo;
		private System.Windows.Forms.Label lblAddToFileName;
		private System.Windows.Forms.CheckBox chkDateStamp;
		private System.Windows.Forms.CheckBox chkTimeStamp;
		private System.Windows.Forms.Label lblSplitFiles;
		private System.Windows.Forms.RadioButton rdoSplitNone;
		private System.Windows.Forms.RadioButton rdoSplitMerchandise;
		private System.Windows.Forms.RadioButton rdoSplitNumEntries;
		private System.Windows.Forms.TextBox txtSplitNumEntries;
		private System.Windows.Forms.CheckBox chkCreateFlagFile;
		private System.Windows.Forms.RadioButton rdoXML;
		private System.Windows.Forms.RadioButton rdoChain;
		private System.Windows.Forms.RadioButton rdoStore;
		private System.Windows.Forms.FolderBrowserDialog fbdFilePath;
		private System.Windows.Forms.Label lblConcurrentProcesses;
		private System.Windows.Forms.TextBox txtConcurrentProcesses;
		private System.Windows.Forms.TextBox txtFlagSuffix;
		private System.Windows.Forms.Label lblFlagSuffix;
		private System.Windows.Forms.CheckBox chkCreateEndFile;
		private System.Windows.Forms.Label lblEndSuffix;
		private System.Windows.Forms.TextBox txtEndSuffix;
		private System.Windows.Forms.CheckBox chkPreinitValues;
		private System.Windows.Forms.ToolTip ttpToolTip;
		private System.Windows.Forms.CheckBox chkShowIneligible;
		private System.Windows.Forms.GroupBox grpOutputFormat;
		private System.Windows.Forms.Label lblCSVFileSuffix;
		private System.Windows.Forms.RadioButton rdoCalendar;
		private System.Windows.Forms.RadioButton rdoFiscal;
		private System.Windows.Forms.Label lblDateFormat;
		private System.Windows.Forms.TextBox txtCSVFileSuffix;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.ImageList Icons;
		private System.Windows.Forms.CheckBox chkExcludeZeroValues;
        private Controls.MIDComboBoxEnh cboVersion;
        private Controls.MIDComboBoxEnh cboFilter;
        private Controls.MIDComboBoxEnh cboLowLevel;
        private Controls.MIDComboBoxEnh cboOverride;
	}
}
