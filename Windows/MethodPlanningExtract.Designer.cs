using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	partial class frmPlanningExtractMethod
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
				this.chkShowIneligible.CheckedChanged -= new System.EventHandler(this.chkShowIneligible_CheckedChanged);
				this.chkExcludeZeroValues.CheckedChanged -= new System.EventHandler(this.chkExcludeZeroValues_CheckedChanged);

                _chainVarChooser.ListChanged -= VarChooser_ListChanged;
                _storeVarChooser.ListChanged -= VarChooser_ListChanged;
                _chainTotalVarChooser.ListChanged -= VarChooser_ListChanged;
                _storeTotalVarChooser.ListChanged -= VarChooser_ListChanged;

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
            this.txtConcurrentProcesses = new System.Windows.Forms.TextBox();
            this.lblConcurrentProcesses = new System.Windows.Forms.Label();
            this.chkExcludeZeroValues = new System.Windows.Forms.CheckBox();
            this.cboStoreAttribute = new MIDRetail.Windows.Controls.MIDAttributeComboBox();
            this.lblStoreAttribute = new System.Windows.Forms.Label();
            this.chxAttributeSetData = new System.Windows.Forms.CheckBox();
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
            this.tpgTimeTotalVariables = new System.Windows.Forms.TabPage();
            this.pnlFormat = new System.Windows.Forms.Panel();
            this.tabProperties = new System.Windows.Forms.TabPage();
            this.ugWorkflows = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fbdFilePath = new System.Windows.Forms.FolderBrowserDialog();
            this.ttpToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlGlobalUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.tabGenAllocMethod.SuspendLayout();
            this.tabMethod.SuspendLayout();
            this.grpLowLevels.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.tpgTimeTotalVariables.SuspendLayout();
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
            this.tabGenAllocMethod.Size = new System.Drawing.Size(680, 568);
            this.tabGenAllocMethod.TabIndex = 14;
            // 
            // tabMethod
            // 
            this.tabMethod.Controls.Add(this.txtConcurrentProcesses);
            this.tabMethod.Controls.Add(this.lblConcurrentProcesses);
            this.tabMethod.Controls.Add(this.chkExcludeZeroValues);
            this.tabMethod.Controls.Add(this.cboStoreAttribute);
            this.tabMethod.Controls.Add(this.lblStoreAttribute);
            this.tabMethod.Controls.Add(this.chxAttributeSetData);
            this.tabMethod.Controls.Add(this.cboFilter);
            this.tabMethod.Controls.Add(this.cboVersion);
            this.tabMethod.Controls.Add(this.chkShowIneligible);
            this.tabMethod.Controls.Add(this.rdoChain);
            this.tabMethod.Controls.Add(this.rdoStore);
            this.tabMethod.Controls.Add(this.lblStoreFilter);
            this.tabMethod.Controls.Add(this.lblVersion);
            this.tabMethod.Controls.Add(this.chkLowLevels);
            this.tabMethod.Controls.Add(this.grpLowLevels);
            this.tabMethod.Controls.Add(this.txtMerchandise);
            this.tabMethod.Controls.Add(this.lblTimePeriod);
            this.tabMethod.Controls.Add(this.drsTimePeriod);
            this.tabMethod.Controls.Add(this.lblMerchandise);
            this.tabMethod.Controls.Add(this.tabOptions);
            this.tabMethod.Location = new System.Drawing.Point(4, 22);
            this.tabMethod.Name = "tabMethod";
            this.tabMethod.Size = new System.Drawing.Size(672, 542);
            this.tabMethod.TabIndex = 1;
            this.tabMethod.Text = "Method";
            // 
            // txtConcurrentProcesses
            // 
            this.txtConcurrentProcesses.Location = new System.Drawing.Point(207, 177);
            this.txtConcurrentProcesses.Name = "txtConcurrentProcesses";
            this.txtConcurrentProcesses.Size = new System.Drawing.Size(100, 20);
            this.txtConcurrentProcesses.TabIndex = 49;
            this.txtConcurrentProcesses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtConcurrentProcesses.TextChanged += new System.EventHandler(this.txtConcurrentProcesses_TextChanged);
            // 
            // lblConcurrentProcesses
            // 
            this.lblConcurrentProcesses.Location = new System.Drawing.Point(71, 177);
            this.lblConcurrentProcesses.Name = "lblConcurrentProcesses";
            this.lblConcurrentProcesses.Size = new System.Drawing.Size(120, 23);
            this.lblConcurrentProcesses.TabIndex = 48;
            this.lblConcurrentProcesses.Text = "Concurrent Processes";
            this.lblConcurrentProcesses.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkExcludeZeroValues
            // 
            this.chkExcludeZeroValues.Location = new System.Drawing.Point(482, 176);
            this.chkExcludeZeroValues.Name = "chkExcludeZeroValues";
            this.chkExcludeZeroValues.Size = new System.Drawing.Size(152, 24);
            this.chkExcludeZeroValues.TabIndex = 47;
            this.chkExcludeZeroValues.Text = "Exclude Zero Values";
            this.ttpToolTip.SetToolTip(this.chkExcludeZeroValues, "Indicates whether zero values should be extracted");
            this.chkExcludeZeroValues.CheckedChanged += new System.EventHandler(this.chkExcludeZeroValues_CheckedChanged);
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.AllowDrop = true;
            this.cboStoreAttribute.AllowUserAttributes = false;
            this.cboStoreAttribute.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboStoreAttribute.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboStoreAttribute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStoreAttribute.FormattingEnabled = true;
            this.cboStoreAttribute.Location = new System.Drawing.Point(421, 10);
            this.cboStoreAttribute.Name = "cboStoreAttribute";
            this.cboStoreAttribute.Size = new System.Drawing.Size(192, 21);
            this.cboStoreAttribute.TabIndex = 46;
            this.cboStoreAttribute.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboStoreAttribute_MIDComboBoxPropertiesChangedEvent);
            this.cboStoreAttribute.SelectionChangeCommitted += new System.EventHandler(this.cboStoreAttribute_SelectionChangeCommitted);
            this.cboStoreAttribute.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragEnter);
            this.cboStoreAttribute.DragOver += new System.Windows.Forms.DragEventHandler(this.cboStoreAttribute_DragOver);
            // 
            // lblStoreAttribute
            // 
            this.lblStoreAttribute.Location = new System.Drawing.Point(373, 16);
            this.lblStoreAttribute.Name = "lblStoreAttribute";
            this.lblStoreAttribute.Size = new System.Drawing.Size(80, 16);
            this.lblStoreAttribute.TabIndex = 45;
            this.lblStoreAttribute.Text = "Attibute:";
            // 
            // chxAttributeSetData
            // 
            this.chxAttributeSetData.AutoSize = true;
            this.chxAttributeSetData.Location = new System.Drawing.Point(243, 14);
            this.chxAttributeSetData.Name = "chxAttributeSetData";
            this.chxAttributeSetData.Size = new System.Drawing.Size(110, 17);
            this.chxAttributeSetData.TabIndex = 44;
            this.chxAttributeSetData.Text = "Attribute Set Data";
            this.chxAttributeSetData.UseVisualStyleBackColor = true;
            this.chxAttributeSetData.CheckedChanged += new System.EventHandler(this.chxAttributeSetData_CheckedChanged);
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
            this.cboFilter.Location = new System.Drawing.Point(108, 144);
            this.cboFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboFilter.MaxDropDownItems = 25;
            this.cboFilter.Name = "cboFilter";
            this.cboFilter.SetToolTip = "";
            this.cboFilter.Size = new System.Drawing.Size(200, 21);
            this.cboFilter.TabIndex = 38;
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
            this.cboVersion.Location = new System.Drawing.Point(108, 80);
            this.cboVersion.Margin = new System.Windows.Forms.Padding(0);
            this.cboVersion.MaxDropDownItems = 25;
            this.cboVersion.Name = "cboVersion";
            this.cboVersion.SetToolTip = "";
            this.cboVersion.Size = new System.Drawing.Size(200, 21);
            this.cboVersion.TabIndex = 36;
            this.cboVersion.Tag = null;
            this.cboVersion.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboVersion_MIDComboBoxPropertiesChangedEvent);
            this.cboVersion.SelectionChangeCommitted += new System.EventHandler(this.cboVersion_SelectionChangeCommitted);
            this.cboVersion.DragDrop += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragDrop);
            this.cboVersion.DragEnter += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragEnter);
            this.cboVersion.DragOver += new System.Windows.Forms.DragEventHandler(this.cboFilter_DragOver);
            // 
            // chkShowIneligible
            // 
            this.chkShowIneligible.Location = new System.Drawing.Point(332, 176);
            this.chkShowIneligible.Name = "chkShowIneligible";
            this.chkShowIneligible.Size = new System.Drawing.Size(184, 24);
            this.chkShowIneligible.TabIndex = 41;
            this.chkShowIneligible.Text = "Extract Ineligible Stores";
            this.chkShowIneligible.CheckedChanged += new System.EventHandler(this.chkShowIneligible_CheckedChanged);
            // 
            // rdoChain
            // 
            this.rdoChain.Location = new System.Drawing.Point(31, 10);
            this.rdoChain.Name = "rdoChain";
            this.rdoChain.Size = new System.Drawing.Size(96, 24);
            this.rdoChain.TabIndex = 40;
            this.rdoChain.Text = "Chain Data";
            this.rdoChain.CheckedChanged += new System.EventHandler(this.rdoChain_CheckedChanged);
            // 
            // rdoStore
            // 
            this.rdoStore.Location = new System.Drawing.Point(135, 10);
            this.rdoStore.Name = "rdoStore";
            this.rdoStore.Size = new System.Drawing.Size(96, 24);
            this.rdoStore.TabIndex = 29;
            this.rdoStore.Text = "Store Data";
            this.rdoStore.CheckedChanged += new System.EventHandler(this.rdoStore_CheckedChanged);
            // 
            // lblStoreFilter
            // 
            this.lblStoreFilter.Location = new System.Drawing.Point(28, 146);
            this.lblStoreFilter.Name = "lblStoreFilter";
            this.lblStoreFilter.Size = new System.Drawing.Size(72, 16);
            this.lblStoreFilter.TabIndex = 39;
            this.lblStoreFilter.Text = "Store Filter";
            this.lblStoreFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblVersion
            // 
            this.lblVersion.Location = new System.Drawing.Point(28, 82);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(72, 16);
            this.lblVersion.TabIndex = 37;
            this.lblVersion.Text = "Version";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkLowLevels
            // 
            this.chkLowLevels.Location = new System.Drawing.Point(332, 43);
            this.chkLowLevels.Name = "chkLowLevels";
            this.chkLowLevels.Size = new System.Drawing.Size(16, 24);
            this.chkLowLevels.TabIndex = 35;
            this.chkLowLevels.CheckedChanged += new System.EventHandler(this.chkLowLevels_CheckedChanged);
            // 
            // grpLowLevels
            // 
            this.grpLowLevels.Controls.Add(this.cboOverride);
            this.grpLowLevels.Controls.Add(this.cboLowLevel);
            this.grpLowLevels.Controls.Add(this.btnOverrideLowLevels);
            this.grpLowLevels.Controls.Add(this.lblLowLevel);
            this.grpLowLevels.Controls.Add(this.chkLowLevelsOnly);
            this.grpLowLevels.Location = new System.Drawing.Point(356, 43);
            this.grpLowLevels.Name = "grpLowLevels";
            this.grpLowLevels.Size = new System.Drawing.Size(264, 120);
            this.grpLowLevels.TabIndex = 34;
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
            this.txtMerchandise.Location = new System.Drawing.Point(108, 48);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(200, 20);
            this.txtMerchandise.TabIndex = 33;
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.Validated += new System.EventHandler(this.txtMerchandise_Validated);
            // 
            // lblTimePeriod
            // 
            this.lblTimePeriod.Location = new System.Drawing.Point(28, 116);
            this.lblTimePeriod.Name = "lblTimePeriod";
            this.lblTimePeriod.Size = new System.Drawing.Size(72, 16);
            this.lblTimePeriod.TabIndex = 30;
            this.lblTimePeriod.Text = "Time Period";
            this.lblTimePeriod.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // drsTimePeriod
            // 
            this.drsTimePeriod.DateRangeForm = null;
            this.drsTimePeriod.DateRangeRID = 0;
            this.drsTimePeriod.Enabled = false;
            this.drsTimePeriod.Location = new System.Drawing.Point(108, 112);
            this.drsTimePeriod.Name = "drsTimePeriod";
            this.drsTimePeriod.Size = new System.Drawing.Size(200, 24);
            this.drsTimePeriod.TabIndex = 32;
            this.drsTimePeriod.OnSelection += new MIDRetail.Windows.Controls.MIDDateRangeSelector.SelectionEventHandler(this.drsTimePeriod_OnSelection);
            this.drsTimePeriod.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.drsTimePeriod_ClickCellButton);
            this.drsTimePeriod.Load += new System.EventHandler(this.drsTimePeriod_Load);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(28, 50);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(72, 16);
            this.lblMerchandise.TabIndex = 31;
            this.lblMerchandise.Text = "Merchandise";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabOptions
            // 
            this.tabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOptions.Controls.Add(this.tpgVariables);
            this.tabOptions.Controls.Add(this.tpgTimeTotalVariables);
            this.tabOptions.Location = new System.Drawing.Point(8, 216);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.SelectedIndex = 0;
            this.tabOptions.Size = new System.Drawing.Size(649, 320);
            this.tabOptions.TabIndex = 0;
            // 
            // tpgVariables
            // 
            this.tpgVariables.Location = new System.Drawing.Point(4, 22);
            this.tpgVariables.Name = "tpgVariables";
            this.tpgVariables.Size = new System.Drawing.Size(641, 294);
            this.tpgVariables.TabIndex = 0;
            this.tpgVariables.Text = "Variables";
            // 
            // tpgTimeTotalVariables
            // 
            this.tpgTimeTotalVariables.Controls.Add(this.pnlFormat);
            this.tpgTimeTotalVariables.Location = new System.Drawing.Point(4, 22);
            this.tpgTimeTotalVariables.Name = "tpgTimeTotalVariables";
            this.tpgTimeTotalVariables.Size = new System.Drawing.Size(629, 294);
            this.tpgTimeTotalVariables.TabIndex = 1;
            this.tpgTimeTotalVariables.Text = "Total Variables";
            // 
            // pnlFormat
            // 
            this.pnlFormat.AutoScroll = true;
            this.pnlFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFormat.Location = new System.Drawing.Point(0, 0);
            this.pnlFormat.Name = "pnlFormat";
            this.pnlFormat.Size = new System.Drawing.Size(629, 294);
            this.pnlFormat.TabIndex = 0;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.ugWorkflows);
            this.tabProperties.Location = new System.Drawing.Point(4, 22);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.Size = new System.Drawing.Size(668, 542);
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
            this.ugWorkflows.Size = new System.Drawing.Size(636, 505);
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
            // frmPlanningExtractMethod
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(728, 686);
            this.Controls.Add(this.tabGenAllocMethod);
            this.Name = "frmPlanningExtractMethod";
            this.Text = "Planning Extract Method";
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
            this.tabMethod.PerformLayout();
            this.grpLowLevels.ResumeLayout(false);
            this.tabOptions.ResumeLayout(false);
            this.tpgTimeTotalVariables.ResumeLayout(false);
            this.tabProperties.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ugWorkflows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		private System.Windows.Forms.TabPage tabMethod;
		private System.Windows.Forms.TabPage tabProperties;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TabControl tabGenAllocMethod;
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.TabPage tpgVariables;
		private System.Windows.Forms.TabPage tpgTimeTotalVariables;
		private System.Windows.Forms.Panel pnlFormat;
		private System.Windows.Forms.FolderBrowserDialog fbdFilePath;
		private System.Windows.Forms.ToolTip ttpToolTip;
        private Infragistics.Win.UltraWinGrid.UltraGrid ugWorkflows;
		private System.Windows.Forms.ImageList Icons;
        private MIDComboBoxEnh cboFilter;
        private MIDComboBoxEnh cboVersion;
        private System.Windows.Forms.CheckBox chkShowIneligible;
        private System.Windows.Forms.RadioButton rdoChain;
        private System.Windows.Forms.RadioButton rdoStore;
        private System.Windows.Forms.Label lblStoreFilter;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.CheckBox chkLowLevels;
        private System.Windows.Forms.GroupBox grpLowLevels;
        private MIDComboBoxEnh cboOverride;
        private MIDComboBoxEnh cboLowLevel;
        private System.Windows.Forms.Button btnOverrideLowLevels;
        private System.Windows.Forms.Label lblLowLevel;
        private System.Windows.Forms.CheckBox chkLowLevelsOnly;
        private System.Windows.Forms.TextBox txtMerchandise;
        private System.Windows.Forms.Label lblTimePeriod;
        private MIDDateRangeSelector drsTimePeriod;
        private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.CheckBox chxAttributeSetData;
        private MIDAttributeComboBox cboStoreAttribute;
        private System.Windows.Forms.Label lblStoreAttribute;
        private System.Windows.Forms.TextBox txtConcurrentProcesses;
        private System.Windows.Forms.Label lblConcurrentProcesses;
        private System.Windows.Forms.CheckBox chkExcludeZeroValues;
    }
}
