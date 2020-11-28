namespace MIDRetail.Windows
{
    partial class AssortmentWorkspaceExplorerFilter
    {
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblMerchandise;
        private System.Windows.Forms.TextBox txtMerchandise;
        private System.Windows.Forms.DateTimePicker dtpHeaderDateFrom;
        private System.Windows.Forms.GroupBox gbxHeaderDate;
        private System.Windows.Forms.RadioButton radHeaderDateToday;
        private System.Windows.Forms.RadioButton radHeaderDateSpecify;
        private System.Windows.Forms.Label lblHeaderDateFrom;
        private System.Windows.Forms.DateTimePicker dtpHeaderDateTo;
        private System.Windows.Forms.Label lblHeaderDateTo;
        private System.Windows.Forms.RadioButton radHeaderDateBetween;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabDates;
        private System.Windows.Forms.GroupBox gbxReleaseDate;
        private System.Windows.Forms.Label lblReleaseDateTo;
        private System.Windows.Forms.DateTimePicker dtpReleaseDateTo;
        private System.Windows.Forms.Label lblReleaseDateFrom;
        private System.Windows.Forms.RadioButton radReleaseDateSpecify;
        private System.Windows.Forms.RadioButton radReleaseDateBetween;
        private System.Windows.Forms.RadioButton radReleaseDateToday;
        private System.Windows.Forms.DateTimePicker dtpReleaseDateFrom;
        private System.Windows.Forms.CheckedListBox lstStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ContextMenu menuListBox;
        private System.Windows.Forms.MenuItem mniRestoreDefaults;
        private System.Windows.Forms.MenuItem mniSelectAll;
        private System.Windows.Forms.MenuItem mniClearAll;
        private System.Windows.Forms.Label lblHeaderDateBetweenDays;
        private System.Windows.Forms.NumericUpDown numHeaderDateBetweenFrom;
        private System.Windows.Forms.Label lblReleaseDateBetweenDays;
        private System.Windows.Forms.NumericUpDown numReleaseDateBetweenFrom;
        private System.Windows.Forms.Label lblHeaderDateBetweenAnd;
        private System.Windows.Forms.Label lblReleaseDateBetweenAnd;
        private System.Windows.Forms.NumericUpDown numHeaderDateBetweenTo;
        private System.Windows.Forms.NumericUpDown numReleaseDateBetweenTo;
        private System.Windows.Forms.RadioButton radHeaderDateAll;
        private System.Windows.Forms.RadioButton radReleaseDateAll;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.TextBox txtMaximumHeaders;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        #region Dispose
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
                this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
                this.txtMerchandise.DragDrop -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
                this.txtMerchandise.TextChanged -= new System.EventHandler(this.txtMerchandise_TextChanged);
                this.txtMerchandise.DragEnter -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
                this.txtMaximumHeaders.TextChanged -= new System.EventHandler(this.txtMaximumHeaders_TextChanged);
                this.txtMerchandise.DragOver -= new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
                this.txtMerchandise.Validating -= new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
                this.dtpHeaderDateFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.numHeaderDateBetweenTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.numHeaderDateBetweenFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.dtpHeaderDateTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.radHeaderDateSpecify.CheckedChanged -= new System.EventHandler(this.radHeaderDateSpecify_CheckedChanged);
                this.radHeaderDateBetween.CheckedChanged -= new System.EventHandler(this.radHeaderDateBetween_CheckedChanged);
                this.radHeaderDateToday.CheckedChanged -= new System.EventHandler(this.radHeaderDateToday_CheckedChanged);
                this.tabControl.SelectedIndexChanged -= new System.EventHandler(this.tabControl_SelectedIndexChanged);
                this.lstStatus.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.lst_MouseDown);
                this.mniRestoreDefaults.Click -= new System.EventHandler(this.mniRestoreDefaults_Click);
                this.mniSelectAll.Click -= new System.EventHandler(this.mniSelectAll_Click);
                this.mniClearAll.Click -= new System.EventHandler(this.mniClearAll_Click);
                this.numReleaseDateBetweenTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.numReleaseDateBetweenFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.dtpReleaseDateTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.radReleaseDateSpecify.CheckedChanged -= new System.EventHandler(this.radReleaseDateSpecify_CheckedChanged);
                this.radReleaseDateBetween.CheckedChanged -= new System.EventHandler(this.radReleaseDateBetween_CheckedChanged);
                this.radReleaseDateToday.CheckedChanged -= new System.EventHandler(this.radReleaseDateToday_CheckedChanged);
                this.dtpReleaseDateFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
                this.Load -= new System.EventHandler(this.AssortmentWorkspaceExplorerFilter_Load);
            }
            base.Dispose(disposing);
        }
        #endregion		
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblMerchandise = new System.Windows.Forms.Label();
            this.txtMerchandise = new System.Windows.Forms.TextBox();
            this.dtpHeaderDateFrom = new System.Windows.Forms.DateTimePicker();
            this.gbxHeaderDate = new System.Windows.Forms.GroupBox();
            this.radHeaderDateAll = new System.Windows.Forms.RadioButton();
            this.numHeaderDateBetweenTo = new System.Windows.Forms.NumericUpDown();
            this.lblHeaderDateBetweenAnd = new System.Windows.Forms.Label();
            this.lblHeaderDateBetweenDays = new System.Windows.Forms.Label();
            this.numHeaderDateBetweenFrom = new System.Windows.Forms.NumericUpDown();
            this.lblHeaderDateTo = new System.Windows.Forms.Label();
            this.dtpHeaderDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblHeaderDateFrom = new System.Windows.Forms.Label();
            this.radHeaderDateSpecify = new System.Windows.Forms.RadioButton();
            this.radHeaderDateBetween = new System.Windows.Forms.RadioButton();
            this.radHeaderDateToday = new System.Windows.Forms.RadioButton();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.txtMaximumHeaders = new System.Windows.Forms.TextBox();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lstStatus = new System.Windows.Forms.CheckedListBox();
            this.menuListBox = new System.Windows.Forms.ContextMenu();
            this.mniRestoreDefaults = new System.Windows.Forms.MenuItem();
            this.mniSelectAll = new System.Windows.Forms.MenuItem();
            this.mniClearAll = new System.Windows.Forms.MenuItem();
            this.tabDates = new System.Windows.Forms.TabPage();
            this.gbxReleaseDate = new System.Windows.Forms.GroupBox();
            this.radReleaseDateAll = new System.Windows.Forms.RadioButton();
            this.numReleaseDateBetweenTo = new System.Windows.Forms.NumericUpDown();
            this.lblReleaseDateBetweenAnd = new System.Windows.Forms.Label();
            this.lblReleaseDateBetweenDays = new System.Windows.Forms.Label();
            this.numReleaseDateBetweenFrom = new System.Windows.Forms.NumericUpDown();
            this.lblReleaseDateTo = new System.Windows.Forms.Label();
            this.dtpReleaseDateTo = new System.Windows.Forms.DateTimePicker();
            this.lblReleaseDateFrom = new System.Windows.Forms.Label();
            this.radReleaseDateSpecify = new System.Windows.Forms.RadioButton();
            this.radReleaseDateBetween = new System.Windows.Forms.RadioButton();
            this.radReleaseDateToday = new System.Windows.Forms.RadioButton();
            this.dtpReleaseDateFrom = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbxHeaderDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderDateBetweenTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderDateBetweenFrom)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.tabDates.SuspendLayout();
            this.gbxReleaseDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReleaseDateBetweenTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReleaseDateBetweenFrom)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(312, 420);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(400, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblMerchandise
            // 
            this.lblMerchandise.Location = new System.Drawing.Point(42, 24);
            this.lblMerchandise.Name = "lblMerchandise";
            this.lblMerchandise.Size = new System.Drawing.Size(82, 23);
            this.lblMerchandise.TabIndex = 2;
            this.lblMerchandise.Text = "Merchandise:";
            this.lblMerchandise.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMerchandise
            // 
            this.txtMerchandise.AllowDrop = true;
            this.txtMerchandise.Location = new System.Drawing.Point(128, 24);
            this.txtMerchandise.Name = "txtMerchandise";
            this.txtMerchandise.Size = new System.Drawing.Size(264, 20);
            this.txtMerchandise.TabIndex = 3;
            this.txtMerchandise.TextChanged += new System.EventHandler(this.txtMerchandise_TextChanged);
            this.txtMerchandise.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragDrop);
            this.txtMerchandise.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMerchandise_KeyDown);
            this.txtMerchandise.Validating += new System.ComponentModel.CancelEventHandler(this.txtMerchandise_Validating);
            this.txtMerchandise.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragEnter);
            this.txtMerchandise.DragOver += new System.Windows.Forms.DragEventHandler(this.txtMerchandise_DragOver);
            // 
            // dtpHeaderDateFrom
            // 
            this.dtpHeaderDateFrom.Location = new System.Drawing.Point(160, 72);
            this.dtpHeaderDateFrom.Name = "dtpHeaderDateFrom";
            this.dtpHeaderDateFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpHeaderDateFrom.TabIndex = 6;
            this.dtpHeaderDateFrom.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // gbxHeaderDate
            // 
            this.gbxHeaderDate.Controls.Add(this.radHeaderDateAll);
            this.gbxHeaderDate.Controls.Add(this.numHeaderDateBetweenTo);
            this.gbxHeaderDate.Controls.Add(this.lblHeaderDateBetweenAnd);
            this.gbxHeaderDate.Controls.Add(this.lblHeaderDateBetweenDays);
            this.gbxHeaderDate.Controls.Add(this.numHeaderDateBetweenFrom);
            this.gbxHeaderDate.Controls.Add(this.lblHeaderDateTo);
            this.gbxHeaderDate.Controls.Add(this.dtpHeaderDateTo);
            this.gbxHeaderDate.Controls.Add(this.lblHeaderDateFrom);
            this.gbxHeaderDate.Controls.Add(this.radHeaderDateSpecify);
            this.gbxHeaderDate.Controls.Add(this.radHeaderDateBetween);
            this.gbxHeaderDate.Controls.Add(this.radHeaderDateToday);
            this.gbxHeaderDate.Controls.Add(this.dtpHeaderDateFrom);
            this.gbxHeaderDate.Location = new System.Drawing.Point(24, 16);
            this.gbxHeaderDate.Name = "gbxHeaderDate";
            this.gbxHeaderDate.Size = new System.Drawing.Size(392, 160);
            this.gbxHeaderDate.TabIndex = 7;
            this.gbxHeaderDate.TabStop = false;
            this.gbxHeaderDate.Text = "Header Date";
            // 
            // radHeaderDateAll
            // 
            this.radHeaderDateAll.Location = new System.Drawing.Point(16, 120);
            this.radHeaderDateAll.Name = "radHeaderDateAll";
            this.radHeaderDateAll.Size = new System.Drawing.Size(80, 24);
            this.radHeaderDateAll.TabIndex = 17;
            this.radHeaderDateAll.Text = "All";
            this.radHeaderDateAll.CheckedChanged += new System.EventHandler(this.radHeaderDateAll_CheckedChanged);
            // 
            // numHeaderDateBetweenTo
            // 
            this.numHeaderDateBetweenTo.Location = new System.Drawing.Point(176, 48);
            this.numHeaderDateBetweenTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHeaderDateBetweenTo.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numHeaderDateBetweenTo.Name = "numHeaderDateBetweenTo";
            this.numHeaderDateBetweenTo.Size = new System.Drawing.Size(48, 20);
            this.numHeaderDateBetweenTo.TabIndex = 16;
            this.numHeaderDateBetweenTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHeaderDateBetweenTo.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            this.numHeaderDateBetweenTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.num_KeyDown);
            // 
            // lblHeaderDateBetweenAnd
            // 
            this.lblHeaderDateBetweenAnd.Location = new System.Drawing.Point(144, 48);
            this.lblHeaderDateBetweenAnd.Name = "lblHeaderDateBetweenAnd";
            this.lblHeaderDateBetweenAnd.Size = new System.Drawing.Size(25, 23);
            this.lblHeaderDateBetweenAnd.TabIndex = 15;
            this.lblHeaderDateBetweenAnd.Text = "and";
            this.lblHeaderDateBetweenAnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeaderDateBetweenDays
            // 
            this.lblHeaderDateBetweenDays.Location = new System.Drawing.Point(232, 48);
            this.lblHeaderDateBetweenDays.Name = "lblHeaderDateBetweenDays";
            this.lblHeaderDateBetweenDays.Size = new System.Drawing.Size(40, 23);
            this.lblHeaderDateBetweenDays.TabIndex = 14;
            this.lblHeaderDateBetweenDays.Text = "days";
            this.lblHeaderDateBetweenDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numHeaderDateBetweenFrom
            // 
            this.numHeaderDateBetweenFrom.Location = new System.Drawing.Point(88, 48);
            this.numHeaderDateBetweenFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numHeaderDateBetweenFrom.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numHeaderDateBetweenFrom.Name = "numHeaderDateBetweenFrom";
            this.numHeaderDateBetweenFrom.Size = new System.Drawing.Size(48, 20);
            this.numHeaderDateBetweenFrom.TabIndex = 13;
            this.numHeaderDateBetweenFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numHeaderDateBetweenFrom.Value = new decimal(new int[] {
            7,
            0,
            0,
            -2147483648});
            this.numHeaderDateBetweenFrom.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            this.numHeaderDateBetweenFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.num_KeyDown);
            // 
            // lblHeaderDateTo
            // 
            this.lblHeaderDateTo.Location = new System.Drawing.Point(104, 104);
            this.lblHeaderDateTo.Name = "lblHeaderDateTo";
            this.lblHeaderDateTo.Size = new System.Drawing.Size(40, 23);
            this.lblHeaderDateTo.TabIndex = 12;
            this.lblHeaderDateTo.Text = "to";
            this.lblHeaderDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpHeaderDateTo
            // 
            this.dtpHeaderDateTo.Location = new System.Drawing.Point(160, 104);
            this.dtpHeaderDateTo.Name = "dtpHeaderDateTo";
            this.dtpHeaderDateTo.Size = new System.Drawing.Size(200, 20);
            this.dtpHeaderDateTo.TabIndex = 11;
            this.dtpHeaderDateTo.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // lblHeaderDateFrom
            // 
            this.lblHeaderDateFrom.Location = new System.Drawing.Point(104, 72);
            this.lblHeaderDateFrom.Name = "lblHeaderDateFrom";
            this.lblHeaderDateFrom.Size = new System.Drawing.Size(40, 23);
            this.lblHeaderDateFrom.TabIndex = 10;
            this.lblHeaderDateFrom.Text = "from";
            this.lblHeaderDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radHeaderDateSpecify
            // 
            this.radHeaderDateSpecify.Location = new System.Drawing.Point(16, 72);
            this.radHeaderDateSpecify.Name = "radHeaderDateSpecify";
            this.radHeaderDateSpecify.Size = new System.Drawing.Size(104, 24);
            this.radHeaderDateSpecify.TabIndex = 9;
            this.radHeaderDateSpecify.Text = "Specify Dates";
            this.radHeaderDateSpecify.CheckedChanged += new System.EventHandler(this.radHeaderDateSpecify_CheckedChanged);
            // 
            // radHeaderDateBetween
            // 
            this.radHeaderDateBetween.Location = new System.Drawing.Point(16, 48);
            this.radHeaderDateBetween.Name = "radHeaderDateBetween";
            this.radHeaderDateBetween.Size = new System.Drawing.Size(72, 24);
            this.radHeaderDateBetween.TabIndex = 8;
            this.radHeaderDateBetween.Text = "Between";
            this.radHeaderDateBetween.CheckedChanged += new System.EventHandler(this.radHeaderDateBetween_CheckedChanged);
            // 
            // radHeaderDateToday
            // 
            this.radHeaderDateToday.Location = new System.Drawing.Point(16, 24);
            this.radHeaderDateToday.Name = "radHeaderDateToday";
            this.radHeaderDateToday.Size = new System.Drawing.Size(104, 24);
            this.radHeaderDateToday.TabIndex = 7;
            this.radHeaderDateToday.Text = "Today";
            this.radHeaderDateToday.CheckedChanged += new System.EventHandler(this.radHeaderDateToday_CheckedChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabDates);
            this.tabControl.Location = new System.Drawing.Point(24, 16);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(440, 400);
            this.tabControl.TabIndex = 8;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.txtMaximumHeaders);
            this.tabGeneral.Controls.Add(this.lblMaximum);
            this.tabGeneral.Controls.Add(this.lblStatus);
            this.tabGeneral.Controls.Add(this.lstStatus);
            this.tabGeneral.Controls.Add(this.lblMerchandise);
            this.tabGeneral.Controls.Add(this.txtMerchandise);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Size = new System.Drawing.Size(432, 374);
            this.tabGeneral.TabIndex = 1;
            this.tabGeneral.Text = "General";
            // 
            // txtMaximumHeaders
            // 
            this.txtMaximumHeaders.Location = new System.Drawing.Point(128, 58);
            this.txtMaximumHeaders.Name = "txtMaximumHeaders";
            this.txtMaximumHeaders.Size = new System.Drawing.Size(100, 20);
            this.txtMaximumHeaders.TabIndex = 9;
            this.txtMaximumHeaders.TextChanged += new System.EventHandler(this.txtMaximumHeaders_TextChanged);
            // 
            // lblMaximum
            // 
            this.lblMaximum.Location = new System.Drawing.Point(10, 56);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(114, 23);
            this.lblMaximum.TabIndex = 8;
            this.lblMaximum.Text = "Maximum Assortments:";
            this.lblMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(128, 88);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 16);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lstStatus
            // 
            this.lstStatus.CheckOnClick = true;
            this.lstStatus.ContextMenu = this.menuListBox;
            this.lstStatus.Location = new System.Drawing.Point(128, 104);
            this.lstStatus.Name = "lstStatus";
            this.lstStatus.Size = new System.Drawing.Size(216, 229);
            this.lstStatus.TabIndex = 5;
            this.lstStatus.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstStatus_ItemCheck);
            this.lstStatus.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lst_MouseDown);
            // 
            // menuListBox
            // 
            this.menuListBox.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mniRestoreDefaults,
            this.mniSelectAll,
            this.mniClearAll});
            // 
            // mniRestoreDefaults
            // 
            this.mniRestoreDefaults.Index = 0;
            this.mniRestoreDefaults.Text = "Restore Defaults";
            this.mniRestoreDefaults.Click += new System.EventHandler(this.mniRestoreDefaults_Click);
            // 
            // mniSelectAll
            // 
            this.mniSelectAll.Index = 1;
            this.mniSelectAll.Text = "Select All";
            this.mniSelectAll.Click += new System.EventHandler(this.mniSelectAll_Click);
            // 
            // mniClearAll
            // 
            this.mniClearAll.Index = 2;
            this.mniClearAll.Text = "Clear All";
            this.mniClearAll.Click += new System.EventHandler(this.mniClearAll_Click);
            // 
            // tabDates
            // 
            this.tabDates.Controls.Add(this.gbxReleaseDate);
            this.tabDates.Controls.Add(this.gbxHeaderDate);
            this.tabDates.Location = new System.Drawing.Point(4, 22);
            this.tabDates.Name = "tabDates";
            this.tabDates.Size = new System.Drawing.Size(432, 374);
            this.tabDates.TabIndex = 2;
            this.tabDates.Text = "Dates";
            // 
            // gbxReleaseDate
            // 
            this.gbxReleaseDate.Controls.Add(this.radReleaseDateAll);
            this.gbxReleaseDate.Controls.Add(this.numReleaseDateBetweenTo);
            this.gbxReleaseDate.Controls.Add(this.lblReleaseDateBetweenAnd);
            this.gbxReleaseDate.Controls.Add(this.lblReleaseDateBetweenDays);
            this.gbxReleaseDate.Controls.Add(this.numReleaseDateBetweenFrom);
            this.gbxReleaseDate.Controls.Add(this.lblReleaseDateTo);
            this.gbxReleaseDate.Controls.Add(this.dtpReleaseDateTo);
            this.gbxReleaseDate.Controls.Add(this.lblReleaseDateFrom);
            this.gbxReleaseDate.Controls.Add(this.radReleaseDateSpecify);
            this.gbxReleaseDate.Controls.Add(this.radReleaseDateBetween);
            this.gbxReleaseDate.Controls.Add(this.radReleaseDateToday);
            this.gbxReleaseDate.Controls.Add(this.dtpReleaseDateFrom);
            this.gbxReleaseDate.Location = new System.Drawing.Point(24, 192);
            this.gbxReleaseDate.Name = "gbxReleaseDate";
            this.gbxReleaseDate.Size = new System.Drawing.Size(392, 160);
            this.gbxReleaseDate.TabIndex = 8;
            this.gbxReleaseDate.TabStop = false;
            this.gbxReleaseDate.Text = "Release Date";
            // 
            // radReleaseDateAll
            // 
            this.radReleaseDateAll.Location = new System.Drawing.Point(16, 120);
            this.radReleaseDateAll.Name = "radReleaseDateAll";
            this.radReleaseDateAll.Size = new System.Drawing.Size(72, 24);
            this.radReleaseDateAll.TabIndex = 18;
            this.radReleaseDateAll.Text = "All";
            this.radReleaseDateAll.CheckedChanged += new System.EventHandler(this.radReleaseDateAll_CheckedChanged);
            // 
            // numReleaseDateBetweenTo
            // 
            this.numReleaseDateBetweenTo.Location = new System.Drawing.Point(176, 48);
            this.numReleaseDateBetweenTo.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numReleaseDateBetweenTo.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numReleaseDateBetweenTo.Name = "numReleaseDateBetweenTo";
            this.numReleaseDateBetweenTo.Size = new System.Drawing.Size(48, 20);
            this.numReleaseDateBetweenTo.TabIndex = 16;
            this.numReleaseDateBetweenTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numReleaseDateBetweenTo.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            this.numReleaseDateBetweenTo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.num_KeyDown);
            // 
            // lblReleaseDateBetweenAnd
            // 
            this.lblReleaseDateBetweenAnd.Location = new System.Drawing.Point(144, 48);
            this.lblReleaseDateBetweenAnd.Name = "lblReleaseDateBetweenAnd";
            this.lblReleaseDateBetweenAnd.Size = new System.Drawing.Size(25, 23);
            this.lblReleaseDateBetweenAnd.TabIndex = 15;
            this.lblReleaseDateBetweenAnd.Text = "and";
            this.lblReleaseDateBetweenAnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReleaseDateBetweenDays
            // 
            this.lblReleaseDateBetweenDays.Location = new System.Drawing.Point(232, 48);
            this.lblReleaseDateBetweenDays.Name = "lblReleaseDateBetweenDays";
            this.lblReleaseDateBetweenDays.Size = new System.Drawing.Size(40, 23);
            this.lblReleaseDateBetweenDays.TabIndex = 14;
            this.lblReleaseDateBetweenDays.Text = "days";
            this.lblReleaseDateBetweenDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numReleaseDateBetweenFrom
            // 
            this.numReleaseDateBetweenFrom.Location = new System.Drawing.Point(88, 48);
            this.numReleaseDateBetweenFrom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numReleaseDateBetweenFrom.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numReleaseDateBetweenFrom.Name = "numReleaseDateBetweenFrom";
            this.numReleaseDateBetweenFrom.Size = new System.Drawing.Size(48, 20);
            this.numReleaseDateBetweenFrom.TabIndex = 13;
            this.numReleaseDateBetweenFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numReleaseDateBetweenFrom.Value = new decimal(new int[] {
            7,
            0,
            0,
            -2147483648});
            this.numReleaseDateBetweenFrom.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            this.numReleaseDateBetweenFrom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.num_KeyDown);
            // 
            // lblReleaseDateTo
            // 
            this.lblReleaseDateTo.Location = new System.Drawing.Point(104, 104);
            this.lblReleaseDateTo.Name = "lblReleaseDateTo";
            this.lblReleaseDateTo.Size = new System.Drawing.Size(40, 23);
            this.lblReleaseDateTo.TabIndex = 12;
            this.lblReleaseDateTo.Text = "to";
            this.lblReleaseDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dtpReleaseDateTo
            // 
            this.dtpReleaseDateTo.Location = new System.Drawing.Point(160, 104);
            this.dtpReleaseDateTo.Name = "dtpReleaseDateTo";
            this.dtpReleaseDateTo.Size = new System.Drawing.Size(200, 20);
            this.dtpReleaseDateTo.TabIndex = 11;
            this.dtpReleaseDateTo.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // lblReleaseDateFrom
            // 
            this.lblReleaseDateFrom.Location = new System.Drawing.Point(104, 72);
            this.lblReleaseDateFrom.Name = "lblReleaseDateFrom";
            this.lblReleaseDateFrom.Size = new System.Drawing.Size(40, 23);
            this.lblReleaseDateFrom.TabIndex = 10;
            this.lblReleaseDateFrom.Text = "from";
            this.lblReleaseDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radReleaseDateSpecify
            // 
            this.radReleaseDateSpecify.Location = new System.Drawing.Point(16, 72);
            this.radReleaseDateSpecify.Name = "radReleaseDateSpecify";
            this.radReleaseDateSpecify.Size = new System.Drawing.Size(104, 24);
            this.radReleaseDateSpecify.TabIndex = 9;
            this.radReleaseDateSpecify.Text = "Specify Dates";
            this.radReleaseDateSpecify.CheckedChanged += new System.EventHandler(this.radReleaseDateSpecify_CheckedChanged);
            // 
            // radReleaseDateBetween
            // 
            this.radReleaseDateBetween.Location = new System.Drawing.Point(16, 48);
            this.radReleaseDateBetween.Name = "radReleaseDateBetween";
            this.radReleaseDateBetween.Size = new System.Drawing.Size(72, 24);
            this.radReleaseDateBetween.TabIndex = 8;
            this.radReleaseDateBetween.Text = "Between";
            this.radReleaseDateBetween.CheckedChanged += new System.EventHandler(this.radReleaseDateBetween_CheckedChanged);
            // 
            // radReleaseDateToday
            // 
            this.radReleaseDateToday.Location = new System.Drawing.Point(16, 24);
            this.radReleaseDateToday.Name = "radReleaseDateToday";
            this.radReleaseDateToday.Size = new System.Drawing.Size(104, 24);
            this.radReleaseDateToday.TabIndex = 7;
            this.radReleaseDateToday.Text = "Today";
            this.radReleaseDateToday.CheckedChanged += new System.EventHandler(this.radReleaseDateToday_CheckedChanged);
            // 
            // dtpReleaseDateFrom
            // 
            this.dtpReleaseDateFrom.Location = new System.Drawing.Point(160, 72);
            this.dtpReleaseDateFrom.Name = "dtpReleaseDateFrom";
            this.dtpReleaseDateFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpReleaseDateFrom.TabIndex = 6;
            this.dtpReleaseDateFrom.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // AssortmentWorkspaceExplorerFilter
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(488, 462);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Name = "AssortmentWorkspaceExplorerFilter";
            this.Text = "Assortment Workspace Filter";
            this.Load += new System.EventHandler(this.AssortmentWorkspaceExplorerFilter_Load);
            this.Controls.SetChildIndex(this.btnApply, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.tabControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbxHeaderDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderDateBetweenTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeaderDateBetweenFrom)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            this.tabDates.ResumeLayout(false);
            this.gbxReleaseDate.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numReleaseDateBetweenTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReleaseDateBetweenFrom)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        
    }
}