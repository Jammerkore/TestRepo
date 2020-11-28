namespace MIDRetail.Windows
{
    partial class frmOverrideLowLevelModel
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
                this.mniSelectAll.Click -= new System.EventHandler(this.mniSelectAll_Click);
                this.mniClearAll.Click -= new System.EventHandler(this.mniClearAll_Click);
                this.cbModelName.SelectedIndexChanged -= new System.EventHandler(this.cbModelName_SelectedIndexChanged);
                this.cbModelName.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.cbModelName_KeyPress);
                //this.cbModelName.SelectedValueChanged -= new System.EventHandler(this.cbModelName_SelectedValueChanged);
                this.ugModel.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
                this.ugModel.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ugModel);
                //End TT#169
                this.ugModel.CellListSelect -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellListSelect);
                this.ugModel.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
                this.ugModel.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
                this.ugModel.CellChange -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
                this.ugModel.MouseEnterElement -= new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
                this.cboHighLevelMerchandise.SelectedIndexChanged -= new System.EventHandler(this.cboHighLevelMerchandise_SelectedIndexChanged);
                this.cboHighLevel.SelectedIndexChanged -= new System.EventHandler(this.cboHighLevel_SelectedIndexChanged);
                this.txtMerchandise.TextChanged -= new System.EventHandler(this.txtMerchandise_TextChanged);
                this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
                this.txtMerchandise.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
                this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
                this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
                this.txtMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
                this.cboLowLevel.SelectedIndexChanged -= new System.EventHandler(this.cboLowLevel_SelectedIndexChanged);
                this.cbxSEApplyToLowerLevels.CheckedChanged -= new System.EventHandler(this.cbxSEApplyToLowerLevels_CheckedChanged);
                this.cbxSEInheritFromHigherLevel.CheckedChanged -= new System.EventHandler(this.cbxSEInheritFromHigherLevel_CheckedChanged);
                this.Load -= new System.EventHandler(this.frmOverrideLowLevelModel_Load);
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

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
            this.menuListBox = new System.Windows.Forms.ContextMenu();
            this.mniSelectAll = new System.Windows.Forms.MenuItem();
            this.mniClearAll = new System.Windows.Forms.MenuItem();
            this.radUser = new System.Windows.Forms.RadioButton();
            this.radGlobal = new System.Windows.Forms.RadioButton();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.gbxExcludes = new System.Windows.Forms.GroupBox();
            this.cbxActiveOnly = new System.Windows.Forms.CheckBox();
            this.cboHighLevelMerchandise = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.MerchandiseLabel = new System.Windows.Forms.Label();
            this.cboHighLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.cboLowLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lowLevelLabel = new System.Windows.Forms.Label();
            this.highLevelLabel = new System.Windows.Forms.Label();
            this.cbxSEApplyToLowerLevels = new System.Windows.Forms.CheckBox();
            this.cbxSEInheritFromHigherLevel = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxExcludes.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(317, 461);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(512, 461);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNew.Location = new System.Drawing.Point(137, 461);
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.Location = new System.Drawing.Point(416, 461);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(608, 461);
            // 
            // picBoxName
            // 
            this.picBoxName.Location = new System.Drawing.Point(93, 8);
            // 
            // cbModelName
            // 
            this.cbModelName.DropDownWidth = 261;
            this.cbModelName.Location = new System.Drawing.Point(115, 9);
            this.cbModelName.Size = new System.Drawing.Size(261, 24);
            this.cbModelName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbModelName_KeyPress);
            // 
            // lblModelName
            // 
            this.lblModelName.Location = new System.Drawing.Point(12, 9);
            this.lblModelName.Size = new System.Drawing.Size(80, 18);
            // 
            // ugModel
            // 
            this.ugModel.AllowDrop = true;
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
            this.ugModel.Location = new System.Drawing.Point(10, 102);
            this.ugModel.Size = new System.Drawing.Size(661, 269);
            this.ugModel.TabIndex = 10;
            this.ugModel.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_AfterCellUpdate);
            this.ugModel.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugModel_InitializeLayout);
            this.ugModel.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugModel_AfterRowInsert);
            this.ugModel.CellChange += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellChange);
            this.ugModel.CellListSelect += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_CellListSelect);
            this.ugModel.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ugModel_ClickCellButton);
            this.ugModel.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ugModel_MouseEnterElement);
            // 
            // btnInUse
            // 
            this.btnInUse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInUse.Location = new System.Drawing.Point(37, 460);
            this.btnInUse.Click += new System.EventHandler(this.btnInUse_Click);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // menuListBox
            // 
            this.menuListBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniSelectAll,
            this.mniClearAll});
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Index = 0;
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // mniClearAll
            // 
            this.mniClearAll.Index = 1;
            this.mniClearAll.Text = "Clear All";
            this.mniClearAll.Click += new System.EventHandler(this.mniClearAll_Click);
            // 
            // radUser
            // 
            this.radUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radUser.AutoSize = true;
            this.radUser.Location = new System.Drawing.Point(485, 10);
            this.radUser.Name = "radUser";
            this.radUser.Size = new System.Drawing.Size(47, 17);
            this.radUser.TabIndex = 21;
            this.radUser.Text = "User";
            this.radUser.UseVisualStyleBackColor = true;
            // 
            // radGlobal
            // 
            this.radGlobal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radGlobal.AutoSize = true;
            this.radGlobal.Checked = true;
            this.radGlobal.Location = new System.Drawing.Point(419, 10);
            this.radGlobal.Name = "radGlobal";
            this.radGlobal.Size = new System.Drawing.Size(55, 17);
            this.radGlobal.TabIndex = 22;
            this.radGlobal.TabStop = true;
            this.radGlobal.Text = "Global";
            this.radGlobal.UseVisualStyleBackColor = true;
            // 
            // radCustom
            // 
            this.radCustom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radCustom.AutoSize = true;
            this.radCustom.Location = new System.Drawing.Point(547, 10);
            this.radCustom.Name = "radCustom";
            this.radCustom.Size = new System.Drawing.Size(60, 17);
            this.radCustom.TabIndex = 23;
            this.radCustom.Text = "Custom";
            this.radCustom.UseVisualStyleBackColor = true;
            // 
            // gbxExcludes
            // 
            this.gbxExcludes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxExcludes.Controls.Add(this.cbxActiveOnly);
            this.gbxExcludes.Controls.Add(this.ugModel);
            this.gbxExcludes.Controls.Add(this.cboHighLevelMerchandise);
            this.gbxExcludes.Controls.Add(this.MerchandiseLabel);
            this.gbxExcludes.Controls.Add(this.cboHighLevel);
            this.gbxExcludes.Controls.Add(this.txtMerchandise);
            this.gbxExcludes.Controls.Add(this.cboLowLevel);
            this.gbxExcludes.Controls.Add(this.lowLevelLabel);
            this.gbxExcludes.Controls.Add(this.highLevelLabel);
            this.gbxExcludes.Location = new System.Drawing.Point(17, 34);
            this.gbxExcludes.Name = "gbxExcludes";
            this.gbxExcludes.Size = new System.Drawing.Size(677, 382);
            this.gbxExcludes.TabIndex = 24;
            this.gbxExcludes.TabStop = false;
            this.gbxExcludes.Controls.SetChildIndex(this.highLevelLabel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.lowLevelLabel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.cboLowLevel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.txtMerchandise, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.cboHighLevel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.MerchandiseLabel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.cboHighLevelMerchandise, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.ugModel, 0);
            this.gbxExcludes.Controls.SetChildIndex(this.cbxActiveOnly, 0);
            // 
            // cbxActiveOnly
            // 
            this.cbxActiveOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxActiveOnly.AutoSize = true;
            this.cbxActiveOnly.Location = new System.Drawing.Point(552, 20);
            this.cbxActiveOnly.Name = "cbxActiveOnly";
            this.cbxActiveOnly.Size = new System.Drawing.Size(80, 17);
            this.cbxActiveOnly.TabIndex = 11;
            this.cbxActiveOnly.Text = "Active Only";
            this.cbxActiveOnly.UseVisualStyleBackColor = true;
            this.cbxActiveOnly.CheckedChanged += new System.EventHandler(this.cbxActiveOnly_CheckedChanged);
            // 
            // cboHighLevelMerchandise
            // 
            this.cboHighLevelMerchandise.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHighLevelMerchandise.AutoAdjust = true;
            this.cboHighLevelMerchandise.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHighLevelMerchandise.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHighLevelMerchandise.DataSource = null;
            this.cboHighLevelMerchandise.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHighLevelMerchandise.DropDownWidth = 296;
            this.cboHighLevelMerchandise.FormattingEnabled = false;
            this.cboHighLevelMerchandise.IgnoreFocusLost = false;
            this.cboHighLevelMerchandise.ItemHeight = 13;
            this.cboHighLevelMerchandise.Location = new System.Drawing.Point(380, 43);
            this.cboHighLevelMerchandise.Margin = new System.Windows.Forms.Padding(0);
            this.cboHighLevelMerchandise.MaxDropDownItems = 8;
            this.cboHighLevelMerchandise.Name = "cboHighLevelMerchandise";
            this.cboHighLevelMerchandise.Size = new System.Drawing.Size(292, 24);
            this.cboHighLevelMerchandise.TabIndex = 9;
            this.cboHighLevelMerchandise.Tag = null;
            this.cboHighLevelMerchandise.SelectedIndexChanged += new System.EventHandler(this.cboHighLevelMerchandise_SelectedIndexChanged);
            this.cboHighLevelMerchandise.MouseHover += new System.EventHandler(this.cboHighLevelMerchandise_MouseHover);
            // 
            // MerchandiseLabel
            // 
            this.MerchandiseLabel.AutoSize = true;
            this.MerchandiseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MerchandiseLabel.Location = new System.Drawing.Point(12, 21);
            this.MerchandiseLabel.Name = "MerchandiseLabel";
            this.MerchandiseLabel.Size = new System.Drawing.Size(83, 13);
            this.MerchandiseLabel.TabIndex = 7;
            this.MerchandiseLabel.Text = "Merchandise:";
            this.MerchandiseLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboHighLevel
            // 
            this.cboHighLevel.AutoAdjust = true;
            this.cboHighLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHighLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHighLevel.DataSource = null;
            this.cboHighLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHighLevel.DropDownWidth = 176;
            this.cboHighLevel.FormattingEnabled = true;
            this.cboHighLevel.IgnoreFocusLost = false;
            this.cboHighLevel.ItemHeight = 13;
            this.cboHighLevel.Location = new System.Drawing.Point(98, 40);
            this.cboHighLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboHighLevel.MaxDropDownItems = 8;
            this.cboHighLevel.Name = "cboHighLevel";
            this.cboHighLevel.Size = new System.Drawing.Size(261, 24);
            this.cboHighLevel.TabIndex = 6;
            this.cboHighLevel.Tag = null;
            this.cboHighLevel.SelectedIndexChanged += new System.EventHandler(this.cboHighLevel_SelectedIndexChanged);
            this.cboHighLevel.MouseHover += new System.EventHandler(this.cboHighLevel_MouseHover);
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMerchandise.Location = new System.Drawing.Point(99, 17);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(407, 20);
            this.txtMerchandise.TabIndex = 5;
            this.txtMerchandise.TextChanged += new System.EventHandler(this.txtMerchandise_TextChanged);
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            this.txtMerchandise.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMerchandise_KeyPress);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            // 
            // cboLowLevel
            // 
            this.cboLowLevel.AutoAdjust = true;
            this.cboLowLevel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLowLevel.DataSource = null;
            this.cboLowLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevel.DropDownWidth = 176;
            this.cboLowLevel.FormattingEnabled = true;
            this.cboLowLevel.IgnoreFocusLost = false;
            this.cboLowLevel.ItemHeight = 13;
            this.cboLowLevel.Location = new System.Drawing.Point(98, 69);
            this.cboLowLevel.Margin = new System.Windows.Forms.Padding(0);
            this.cboLowLevel.MaxDropDownItems = 8;
            this.cboLowLevel.Name = "cboLowLevel";
            this.cboLowLevel.Size = new System.Drawing.Size(261, 24);
            this.cboLowLevel.TabIndex = 3;
            this.cboLowLevel.Tag = null;
            this.cboLowLevel.SelectedIndexChanged += new System.EventHandler(this.cboLowLevel_SelectedIndexChanged);
            this.cboLowLevel.MouseHover += new System.EventHandler(this.cboLowLevel_MouseHover);
            // 
            // lowLevelLabel
            // 
            this.lowLevelLabel.AutoSize = true;
            this.lowLevelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lowLevelLabel.Location = new System.Drawing.Point(23, 73);
            this.lowLevelLabel.Name = "lowLevelLabel";
            this.lowLevelLabel.Size = new System.Drawing.Size(69, 13);
            this.lowLevelLabel.TabIndex = 2;
            this.lowLevelLabel.Text = "Low Level:";
            this.lowLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // highLevelLabel
            // 
            this.highLevelLabel.AutoSize = true;
            this.highLevelLabel.Cursor = System.Windows.Forms.Cursors.No;
            this.highLevelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highLevelLabel.Location = new System.Drawing.Point(20, 45);
            this.highLevelLabel.Name = "highLevelLabel";
            this.highLevelLabel.Size = new System.Drawing.Size(72, 13);
            this.highLevelLabel.TabIndex = 0;
            this.highLevelLabel.Text = "High Level:";
            this.highLevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbxSEApplyToLowerLevels
            // 
            this.cbxSEApplyToLowerLevels.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSEApplyToLowerLevels.Location = new System.Drawing.Point(240, 411);
            this.cbxSEApplyToLowerLevels.Name = "cbxSEApplyToLowerLevels";
            this.cbxSEApplyToLowerLevels.Size = new System.Drawing.Size(131, 43);
            this.cbxSEApplyToLowerLevels.TabIndex = 29;
            this.cbxSEApplyToLowerLevels.Text = "Apply to lower levels";
            this.cbxSEApplyToLowerLevels.CheckedChanged += new System.EventHandler(this.cbxSEApplyToLowerLevels_CheckedChanged);
            // 
            // cbxSEInheritFromHigherLevel
            // 
            this.cbxSEInheritFromHigherLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxSEInheritFromHigherLevel.Location = new System.Drawing.Point(93, 411);
            this.cbxSEInheritFromHigherLevel.Name = "cbxSEInheritFromHigherLevel";
            this.cbxSEInheritFromHigherLevel.Size = new System.Drawing.Size(141, 43);
            this.cbxSEInheritFromHigherLevel.TabIndex = 30;
            this.cbxSEInheritFromHigherLevel.Text = "Inherit from higher level";
            this.cbxSEInheritFromHigherLevel.CheckedChanged += new System.EventHandler(this.cbxSEInheritFromHigherLevel_CheckedChanged);
            // 
            // frmOverrideLowLevelModel
            // 
            this.ClientSize = new System.Drawing.Size(720, 517);
            this.Controls.Add(this.cbxSEInheritFromHigherLevel);
            this.Controls.Add(this.cbxSEApplyToLowerLevels);
            this.Controls.Add(this.gbxExcludes);
            this.Controls.Add(this.radGlobal);
            this.Controls.Add(this.radUser);
            this.Controls.Add(this.radCustom);
            this.Name = "frmOverrideLowLevelModel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OverrideLowLevelModel";
            this.Load += new System.EventHandler(this.frmOverrideLowLevelModel_Load);
            this.Controls.SetChildIndex(this.btnInUse, 0);
            this.Controls.SetChildIndex(this.picBoxName, 0);
            this.Controls.SetChildIndex(this.cbModelName, 0);
            this.Controls.SetChildIndex(this.radCustom, 0);
            this.Controls.SetChildIndex(this.radUser, 0);
            this.Controls.SetChildIndex(this.radGlobal, 0);
            this.Controls.SetChildIndex(this.gbxExcludes, 0);
            this.Controls.SetChildIndex(this.btnNew, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnSaveAs, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.cbxSEApplyToLowerLevels, 0);
            this.Controls.SetChildIndex(this.cbxSEInheritFromHigherLevel, 0);
            this.Controls.SetChildIndex(this.lblModelName, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugModel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxExcludes.ResumeLayout(false);
            this.gbxExcludes.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radUser;
        private System.Windows.Forms.RadioButton radGlobal;
        private System.Windows.Forms.RadioButton radCustom;
        private System.Windows.Forms.GroupBox gbxExcludes;
        private System.Windows.Forms.Label highLevelLabel;
        private System.Windows.Forms.Label lowLevelLabel;
        //private System.Windows.Forms.ComboBox cboLowLevel;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboLowLevel;
        private System.Windows.Forms.TextBox txtMerchandise;
        //private System.Windows.Forms.ComboBox cboHighLevel;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboHighLevel;
        private System.Windows.Forms.Label MerchandiseLabel;
        //private System.Windows.Forms.ComboBox cboHighLevelMerchandise;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboHighLevelMerchandise;
        private System.Windows.Forms.ContextMenu menuListBox;
        private System.Windows.Forms.MenuItem mniSelectAll;
        private System.Windows.Forms.MenuItem mniClearAll;
        //private Infragistics.Win.UltraWinGrid.UltraGrid ugModel;
        private System.Windows.Forms.CheckBox cbxSEApplyToLowerLevels;
        private System.Windows.Forms.CheckBox cbxSEInheritFromHigherLevel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.CheckBox cbxActiveOnly;
    }
}