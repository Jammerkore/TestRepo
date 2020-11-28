//using System;
//using System.Data;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Globalization;
//using System.Windows.Forms;

//using MIDRetail.Business;   
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.DataCommon;

//namespace MIDRetail.Windows
//{
//    /// <summary>
//    /// Summary description for AuditFilter.
//    /// </summary>
//    public class AuditFilter : MIDFormBase
//    {
//        // add event to update explorer when node is changed
//        public delegate void AuditFilterChangeEventHandler(object source, AuditFilterChangeEventArgs e);
//        public event AuditFilterChangeEventHandler OnAuditFilterChangeHandler;

//        private SessionAddressBlock _SAB;
//        //private System.Windows.Forms.CheckedListBox _rightMouseListBox;
//        private AuditFilterProfile _afp;
//        private bool _continueWithSave;

//        private System.Windows.Forms.Button btnApply;
//        private System.Windows.Forms.Button btnCancel;
//        private System.Windows.Forms.GroupBox gbxRunDate;
//        private System.Windows.Forms.RadioButton radRunDateAll;
//        private System.Windows.Forms.NumericUpDown numRunDateBetweenTo;
//        private System.Windows.Forms.Label lblRunDateBetweenAnd;
//        private System.Windows.Forms.Label lblRunDateBetweenDays;
//        private System.Windows.Forms.NumericUpDown numRunDateBetweenFrom;
//        private System.Windows.Forms.Label lblRunDateTo;
//        private System.Windows.Forms.DateTimePicker dtpRunDateTo;
//        private System.Windows.Forms.Label lblRunDateFrom;
//        private System.Windows.Forms.RadioButton radRunDateSpecify;
//        private System.Windows.Forms.RadioButton radRunDateBetween;
//        private System.Windows.Forms.RadioButton radRunDateToday;
//        private System.Windows.Forms.DateTimePicker dtpRunDateFrom;
//        private System.Windows.Forms.Label lblProcessHighestMessageLevel;
//        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboProcessMessageLevel;
//        private System.Windows.Forms.Label lblDetailMessageLevel;
//        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboDetailMessageLevel;
//        private System.Windows.Forms.Label lblDurationGreaterThan;
//        private System.Windows.Forms.NumericUpDown numDurationGreaterThan;
//        private System.Windows.Forms.Label lblDurationGreaterThanMinutes;
//        private System.Windows.Forms.GroupBox gbxStatus;
//        private System.Windows.Forms.CheckBox cbxRunning;
//        private System.Windows.Forms.CheckBox cbxCompleted;
//        private System.Windows.Forms.CheckBox cbxMyTasksOnly;
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.Container components = null;

//        public AuditFilter(SessionAddressBlock aSAB) : base (aSAB)
//        {
//            _SAB = aSAB;
//            //
//            // Required for Windows Form Designer support
//            //
//            InitializeComponent();

//            //
//            // TODO: Add any constructor code after InitializeComponent call
//            //
//        }

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        protected override void Dispose( bool disposing )
//        {
//            if( disposing )
//            {
//                if(components != null)
//                {
//                    components.Dispose();
//                }

//                this.btnApply.Click -= new System.EventHandler(this.btnApply_Click);
//                this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
//                this.dtpRunDateFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
//                this.numRunDateBetweenTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
//                this.numRunDateBetweenFrom.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
//                this.dtpRunDateTo.ValueChanged -= new System.EventHandler(this.Date_ValueChanged);
//                this.radRunDateSpecify.CheckedChanged -= new System.EventHandler(this.radRunDateSpecify_CheckedChanged);
//                this.radRunDateBetween.CheckedChanged -= new System.EventHandler(this.radRunDateBetween_CheckedChanged);
//                this.radRunDateToday.CheckedChanged -= new System.EventHandler(this.radRunDateToday_CheckedChanged);
//                this.Load -= new System.EventHandler(this.AuditFilter_Load);
//            }
//            base.Dispose( disposing );
//        }

//        #region Windows Form Designer generated code
//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.btnApply = new System.Windows.Forms.Button();
//            this.btnCancel = new System.Windows.Forms.Button();
//            this.lblProcessHighestMessageLevel = new System.Windows.Forms.Label();
//            this.cboProcessMessageLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//            this.gbxRunDate = new System.Windows.Forms.GroupBox();
//            this.radRunDateAll = new System.Windows.Forms.RadioButton();
//            this.numRunDateBetweenTo = new System.Windows.Forms.NumericUpDown();
//            this.lblRunDateBetweenAnd = new System.Windows.Forms.Label();
//            this.lblRunDateBetweenDays = new System.Windows.Forms.Label();
//            this.numRunDateBetweenFrom = new System.Windows.Forms.NumericUpDown();
//            this.lblRunDateTo = new System.Windows.Forms.Label();
//            this.dtpRunDateTo = new System.Windows.Forms.DateTimePicker();
//            this.lblRunDateFrom = new System.Windows.Forms.Label();
//            this.radRunDateSpecify = new System.Windows.Forms.RadioButton();
//            this.radRunDateBetween = new System.Windows.Forms.RadioButton();
//            this.radRunDateToday = new System.Windows.Forms.RadioButton();
//            this.dtpRunDateFrom = new System.Windows.Forms.DateTimePicker();
//            this.lblDetailMessageLevel = new System.Windows.Forms.Label();
//            this.cboDetailMessageLevel = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//            this.lblDurationGreaterThan = new System.Windows.Forms.Label();
//            this.numDurationGreaterThan = new System.Windows.Forms.NumericUpDown();
//            this.lblDurationGreaterThanMinutes = new System.Windows.Forms.Label();
//            this.gbxStatus = new System.Windows.Forms.GroupBox();
//            this.cbxCompleted = new System.Windows.Forms.CheckBox();
//            this.cbxRunning = new System.Windows.Forms.CheckBox();
//            this.cbxMyTasksOnly = new System.Windows.Forms.CheckBox();
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
//            this.gbxRunDate.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.numRunDateBetweenTo)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numRunDateBetweenFrom)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numDurationGreaterThan)).BeginInit();
//            this.gbxStatus.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // utmMain
//            // 
//            this.utmMain.MenuSettings.ForceSerialization = true;
//            this.utmMain.ToolbarSettings.ForceSerialization = true;
//            // 
//            // btnApply
//            // 
//            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnApply.Location = new System.Drawing.Point(272, 376);
//            this.btnApply.Name = "btnApply";
//            this.btnApply.Size = new System.Drawing.Size(75, 23);
//            this.btnApply.TabIndex = 0;
//            this.btnApply.Text = "Apply";
//            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
//            // 
//            // btnCancel
//            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(360, 376);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 23);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Cancel";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
//            // 
//            // lblProcessHighestMessageLevel
//            // 
//            this.lblProcessHighestMessageLevel.Location = new System.Drawing.Point(8, 288);
//            this.lblProcessHighestMessageLevel.Name = "lblProcessHighestMessageLevel";
//            this.lblProcessHighestMessageLevel.Size = new System.Drawing.Size(176, 23);
//            this.lblProcessHighestMessageLevel.TabIndex = 10;
//            this.lblProcessHighestMessageLevel.Text = "Highest Process Message Level:";
//            this.lblProcessHighestMessageLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // cboProcessMessageLevel
//            // 
//            this.cboProcessMessageLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.cboProcessMessageLevel.Location = new System.Drawing.Point(200, 288);
//            this.cboProcessMessageLevel.Name = "cboProcessMessageLevel";
//            this.cboProcessMessageLevel.Size = new System.Drawing.Size(216, 21);
//            this.cboProcessMessageLevel.TabIndex = 9;
//            // 
//            // gbxRunDate
//            // 
//            this.gbxRunDate.Controls.Add(this.radRunDateAll);
//            this.gbxRunDate.Controls.Add(this.numRunDateBetweenTo);
//            this.gbxRunDate.Controls.Add(this.lblRunDateBetweenAnd);
//            this.gbxRunDate.Controls.Add(this.lblRunDateBetweenDays);
//            this.gbxRunDate.Controls.Add(this.numRunDateBetweenFrom);
//            this.gbxRunDate.Controls.Add(this.lblRunDateTo);
//            this.gbxRunDate.Controls.Add(this.dtpRunDateTo);
//            this.gbxRunDate.Controls.Add(this.lblRunDateFrom);
//            this.gbxRunDate.Controls.Add(this.radRunDateSpecify);
//            this.gbxRunDate.Controls.Add(this.radRunDateBetween);
//            this.gbxRunDate.Controls.Add(this.radRunDateToday);
//            this.gbxRunDate.Controls.Add(this.dtpRunDateFrom);
//            this.gbxRunDate.Location = new System.Drawing.Point(16, 16);
//            this.gbxRunDate.Name = "gbxRunDate";
//            this.gbxRunDate.Size = new System.Drawing.Size(392, 160);
//            this.gbxRunDate.TabIndex = 8;
//            this.gbxRunDate.TabStop = false;
//            this.gbxRunDate.Text = "Run Date";
//            // 
//            // radRunDateAll
//            // 
//            this.radRunDateAll.Location = new System.Drawing.Point(16, 120);
//            this.radRunDateAll.Name = "radRunDateAll";
//            this.radRunDateAll.Size = new System.Drawing.Size(80, 24);
//            this.radRunDateAll.TabIndex = 17;
//            this.radRunDateAll.Text = "All";
//            this.radRunDateAll.CheckedChanged += new System.EventHandler(this.radRunDateAll_CheckedChanged);
//            // 
//            // numRunDateBetweenTo
//            // 
//            this.numRunDateBetweenTo.Location = new System.Drawing.Point(176, 48);
//            this.numRunDateBetweenTo.Maximum = new decimal(new int[] {
//            1000,
//            0,
//            0,
//            0});
//            this.numRunDateBetweenTo.Minimum = new decimal(new int[] {
//            1000,
//            0,
//            0,
//            -2147483648});
//            this.numRunDateBetweenTo.Name = "numRunDateBetweenTo";
//            this.numRunDateBetweenTo.Size = new System.Drawing.Size(48, 20);
//            this.numRunDateBetweenTo.TabIndex = 16;
//            this.numRunDateBetweenTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//            // 
//            // lblRunDateBetweenAnd
//            // 
//            this.lblRunDateBetweenAnd.Location = new System.Drawing.Point(144, 48);
//            this.lblRunDateBetweenAnd.Name = "lblRunDateBetweenAnd";
//            this.lblRunDateBetweenAnd.Size = new System.Drawing.Size(25, 23);
//            this.lblRunDateBetweenAnd.TabIndex = 15;
//            this.lblRunDateBetweenAnd.Text = "and";
//            this.lblRunDateBetweenAnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // lblRunDateBetweenDays
//            // 
//            this.lblRunDateBetweenDays.Location = new System.Drawing.Point(232, 48);
//            this.lblRunDateBetweenDays.Name = "lblRunDateBetweenDays";
//            this.lblRunDateBetweenDays.Size = new System.Drawing.Size(40, 23);
//            this.lblRunDateBetweenDays.TabIndex = 14;
//            this.lblRunDateBetweenDays.Text = "days";
//            this.lblRunDateBetweenDays.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            // 
//            // numRunDateBetweenFrom
//            // 
//            this.numRunDateBetweenFrom.Location = new System.Drawing.Point(88, 48);
//            this.numRunDateBetweenFrom.Maximum = new decimal(new int[] {
//            1000,
//            0,
//            0,
//            0});
//            this.numRunDateBetweenFrom.Minimum = new decimal(new int[] {
//            1000,
//            0,
//            0,
//            -2147483648});
//            this.numRunDateBetweenFrom.Name = "numRunDateBetweenFrom";
//            this.numRunDateBetweenFrom.Size = new System.Drawing.Size(48, 20);
//            this.numRunDateBetweenFrom.TabIndex = 13;
//            this.numRunDateBetweenFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//            this.numRunDateBetweenFrom.Value = new decimal(new int[] {
//            7,
//            0,
//            0,
//            -2147483648});
//            // 
//            // lblRunDateTo
//            // 
//            this.lblRunDateTo.Location = new System.Drawing.Point(104, 104);
//            this.lblRunDateTo.Name = "lblRunDateTo";
//            this.lblRunDateTo.Size = new System.Drawing.Size(40, 23);
//            this.lblRunDateTo.TabIndex = 12;
//            this.lblRunDateTo.Text = "to";
//            this.lblRunDateTo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // dtpRunDateTo
//            // 
//            this.dtpRunDateTo.Location = new System.Drawing.Point(160, 104);
//            this.dtpRunDateTo.Name = "dtpRunDateTo";
//            this.dtpRunDateTo.Size = new System.Drawing.Size(200, 20);
//            this.dtpRunDateTo.TabIndex = 11;
//            // 
//            // lblRunDateFrom
//            // 
//            this.lblRunDateFrom.Location = new System.Drawing.Point(104, 72);
//            this.lblRunDateFrom.Name = "lblRunDateFrom";
//            this.lblRunDateFrom.Size = new System.Drawing.Size(40, 23);
//            this.lblRunDateFrom.TabIndex = 10;
//            this.lblRunDateFrom.Text = "from";
//            this.lblRunDateFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // radRunDateSpecify
//            // 
//            this.radRunDateSpecify.Location = new System.Drawing.Point(16, 72);
//            this.radRunDateSpecify.Name = "radRunDateSpecify";
//            this.radRunDateSpecify.Size = new System.Drawing.Size(104, 24);
//            this.radRunDateSpecify.TabIndex = 9;
//            this.radRunDateSpecify.Text = "Specify Dates";
//            this.radRunDateSpecify.CheckedChanged += new System.EventHandler(this.radRunDateSpecify_CheckedChanged);
//            // 
//            // radRunDateBetween
//            // 
//            this.radRunDateBetween.Location = new System.Drawing.Point(16, 48);
//            this.radRunDateBetween.Name = "radRunDateBetween";
//            this.radRunDateBetween.Size = new System.Drawing.Size(72, 24);
//            this.radRunDateBetween.TabIndex = 8;
//            this.radRunDateBetween.Text = "Between";
//            this.radRunDateBetween.CheckedChanged += new System.EventHandler(this.radRunDateBetween_CheckedChanged);
//            // 
//            // radRunDateToday
//            // 
//            this.radRunDateToday.Location = new System.Drawing.Point(16, 24);
//            this.radRunDateToday.Name = "radRunDateToday";
//            this.radRunDateToday.Size = new System.Drawing.Size(104, 24);
//            this.radRunDateToday.TabIndex = 7;
//            this.radRunDateToday.Text = "Today";
//            this.radRunDateToday.CheckedChanged += new System.EventHandler(this.radRunDateToday_CheckedChanged);
//            // 
//            // dtpRunDateFrom
//            // 
//            this.dtpRunDateFrom.Location = new System.Drawing.Point(160, 72);
//            this.dtpRunDateFrom.Name = "dtpRunDateFrom";
//            this.dtpRunDateFrom.Size = new System.Drawing.Size(200, 20);
//            this.dtpRunDateFrom.TabIndex = 6;
//            // 
//            // lblDetailMessageLevel
//            // 
//            this.lblDetailMessageLevel.Location = new System.Drawing.Point(24, 328);
//            this.lblDetailMessageLevel.Name = "lblDetailMessageLevel";
//            this.lblDetailMessageLevel.Size = new System.Drawing.Size(160, 23);
//            this.lblDetailMessageLevel.TabIndex = 12;
//            this.lblDetailMessageLevel.Text = "Highest Detail Message Level:";
//            this.lblDetailMessageLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // cboDetailMessageLevel
//            // 
//            this.cboDetailMessageLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.cboDetailMessageLevel.Location = new System.Drawing.Point(200, 328);
//            this.cboDetailMessageLevel.Name = "cboDetailMessageLevel";
//            this.cboDetailMessageLevel.Size = new System.Drawing.Size(216, 21);
//            this.cboDetailMessageLevel.TabIndex = 11;
//            // 
//            // lblDurationGreaterThan
//            // 
//            this.lblDurationGreaterThan.Location = new System.Drawing.Point(16, 248);
//            this.lblDurationGreaterThan.Name = "lblDurationGreaterThan";
//            this.lblDurationGreaterThan.Size = new System.Drawing.Size(128, 23);
//            this.lblDurationGreaterThan.TabIndex = 13;
//            this.lblDurationGreaterThan.Text = "Duration greater than";
//            this.lblDurationGreaterThan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // numDurationGreaterThan
//            // 
//            this.numDurationGreaterThan.Location = new System.Drawing.Point(152, 248);
//            this.numDurationGreaterThan.Maximum = new decimal(new int[] {
//            10000,
//            0,
//            0,
//            0});
//            this.numDurationGreaterThan.Name = "numDurationGreaterThan";
//            this.numDurationGreaterThan.Size = new System.Drawing.Size(80, 20);
//            this.numDurationGreaterThan.TabIndex = 14;
//            this.numDurationGreaterThan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
//            // 
//            // lblDurationGreaterThanMinutes
//            // 
//            this.lblDurationGreaterThanMinutes.Location = new System.Drawing.Point(240, 248);
//            this.lblDurationGreaterThanMinutes.Name = "lblDurationGreaterThanMinutes";
//            this.lblDurationGreaterThanMinutes.Size = new System.Drawing.Size(100, 23);
//            this.lblDurationGreaterThanMinutes.TabIndex = 15;
//            this.lblDurationGreaterThanMinutes.Text = "minutes";
//            this.lblDurationGreaterThanMinutes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//            // 
//            // gbxStatus
//            // 
//            this.gbxStatus.Controls.Add(this.cbxCompleted);
//            this.gbxStatus.Controls.Add(this.cbxRunning);
//            this.gbxStatus.Location = new System.Drawing.Point(16, 184);
//            this.gbxStatus.Name = "gbxStatus";
//            this.gbxStatus.Size = new System.Drawing.Size(392, 48);
//            this.gbxStatus.TabIndex = 16;
//            this.gbxStatus.TabStop = false;
//            this.gbxStatus.Text = "Status";
//            // 
//            // cbxCompleted
//            // 
//            this.cbxCompleted.Location = new System.Drawing.Point(176, 16);
//            this.cbxCompleted.Name = "cbxCompleted";
//            this.cbxCompleted.Size = new System.Drawing.Size(104, 24);
//            this.cbxCompleted.TabIndex = 1;
//            this.cbxCompleted.Text = "Completed";
//            // 
//            // cbxRunning
//            // 
//            this.cbxRunning.Location = new System.Drawing.Point(40, 16);
//            this.cbxRunning.Name = "cbxRunning";
//            this.cbxRunning.Size = new System.Drawing.Size(104, 24);
//            this.cbxRunning.TabIndex = 0;
//            this.cbxRunning.Text = "Running";
//            // 
//            // cbxMyTasksOnly
//            // 
//            this.cbxMyTasksOnly.Location = new System.Drawing.Point(24, 368);
//            this.cbxMyTasksOnly.Name = "cbxMyTasksOnly";
//            this.cbxMyTasksOnly.Size = new System.Drawing.Size(168, 24);
//            this.cbxMyTasksOnly.TabIndex = 17;
//            this.cbxMyTasksOnly.Text = "Show My Tasks Only";
//            // 
//            // AuditFilter
//            // 
//            this.AllowDragDrop = true;
//            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//            this.ClientSize = new System.Drawing.Size(448, 406);
//            this.Controls.Add(this.cbxMyTasksOnly);
//            this.Controls.Add(this.gbxStatus);
//            this.Controls.Add(this.lblDurationGreaterThanMinutes);
//            this.Controls.Add(this.numDurationGreaterThan);
//            this.Controls.Add(this.lblDurationGreaterThan);
//            this.Controls.Add(this.btnCancel);
//            this.Controls.Add(this.btnApply);
//            this.Controls.Add(this.gbxRunDate);
//            this.Controls.Add(this.lblProcessHighestMessageLevel);
//            this.Controls.Add(this.cboProcessMessageLevel);
//            this.Controls.Add(this.lblDetailMessageLevel);
//            this.Controls.Add(this.cboDetailMessageLevel);
//            this.Name = "AuditFilter";
//            this.Text = "Audit Filter";
//            this.Load += new System.EventHandler(this.AuditFilter_Load);
//            this.Controls.SetChildIndex(this.cboDetailMessageLevel, 0);
//            this.Controls.SetChildIndex(this.lblDetailMessageLevel, 0);
//            this.Controls.SetChildIndex(this.cboProcessMessageLevel, 0);
//            this.Controls.SetChildIndex(this.lblProcessHighestMessageLevel, 0);
//            this.Controls.SetChildIndex(this.gbxRunDate, 0);
//            this.Controls.SetChildIndex(this.btnApply, 0);
//            this.Controls.SetChildIndex(this.btnCancel, 0);
//            this.Controls.SetChildIndex(this.lblDurationGreaterThan, 0);
//            this.Controls.SetChildIndex(this.numDurationGreaterThan, 0);
//            this.Controls.SetChildIndex(this.lblDurationGreaterThanMinutes, 0);
//            this.Controls.SetChildIndex(this.gbxStatus, 0);
//            this.Controls.SetChildIndex(this.cbxMyTasksOnly, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
//            this.gbxRunDate.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.numRunDateBetweenTo)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numRunDateBetweenFrom)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numDurationGreaterThan)).EndInit();
//            this.gbxStatus.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }
//        #endregion

//        private void AuditFilter_Load(object sender, System.EventArgs e)
//        {
//            FormLoaded = false;
//            _afp = new AuditFilterProfile(_SAB.ClientServerSession.UserRID);

//            Format_Title(eDataState.Updatable, eMIDTextCode.frm_AuditFilter, _SAB.ClientServerSession.GetUserName(_SAB.ClientServerSession.UserRID));

//            // load run date
//            switch (_afp.RunDateType)
//            {
//                case eFilterDateType.specify:
//                    radRunDateSpecify.Checked = true;
//                    dtpRunDateFrom.Value = _afp.RunDateFrom;
//                    dtpRunDateTo.Value = _afp.RunDateTo;
//                    break;
//                case eFilterDateType.today:
//                    radRunDateToday.Checked = true;
//                    break;
//                case eFilterDateType.between:
//                    radRunDateBetween.Checked = true;
//                    numRunDateBetweenFrom.Value = _afp.RunDateBetweenFrom;
//                    numRunDateBetweenTo.Value = _afp.RunDateBetweenTo;
//                    break;
//                case eFilterDateType.all:
//                    radRunDateAll.Checked = true;
//                    break;
//            }

//            cboProcessMessageLevel.DataSource = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
//            cboProcessMessageLevel.ValueMember = "TEXT_CODE";
//            cboProcessMessageLevel.DisplayMember = "TEXT_VALUE";

//            cboProcessMessageLevel.SelectedValue = _afp.HighestProcessMessageLevel;

//            cboDetailMessageLevel.DataSource = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
//            cboDetailMessageLevel.ValueMember = "TEXT_CODE";
//            cboDetailMessageLevel.DisplayMember = "TEXT_VALUE";

//            cboDetailMessageLevel.SelectedValue = _afp.HighestDetailMessageLevel;

//            numDurationGreaterThan.Value = _afp.Duration;

//            cbxCompleted.Checked = _afp.ShowCompletedProcesses;
//            cbxRunning.Checked = _afp.ShowRunningProcesses;
//            cbxMyTasksOnly.Checked = _afp.ShowMyTasksOnly;

//            SetText();
			
//            FormLoaded = true;
//        }

//        private void SetText()
//        {
//            try
//            {
//                this.btnApply.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Apply);
//                this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
//                this.lblProcessHighestMessageLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_HighestProcessLevel);
//                this.gbxRunDate.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDate);
//                this.radRunDateAll.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateAll);
//                this.lblRunDateBetweenAnd.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateBetweenAnd);
//                this.lblRunDateBetweenDays.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateBetweenDays);
//                this.lblRunDateTo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateSpecifyTo);
//                this.lblRunDateFrom.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateSpecifyFrom);
//                this.radRunDateSpecify.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateSpecify);
//                this.radRunDateBetween.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateBetween);
//                this.radRunDateToday.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_RunDateToday);
//                this.lblDetailMessageLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_HighestDetailLevel);
//                this.lblDurationGreaterThan.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_Duration);
//                this.lblDurationGreaterThanMinutes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_DurationMinutes);
//                this.gbxStatus.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_Status);
//                this.cbxRunning.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_StatusRunning);
//                this.cbxCompleted.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_StatusCompleted);
//                this.cbxMyTasksOnly.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_MyTasksOnly);

////				ToolTip.SetToolTip(radRunDateToday, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderToday));
////				ToolTip.SetToolTip(radRunDateBetween, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderBetween));
////				ToolTip.SetToolTip(numRunDateBetweenFrom, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderBetweenFrom));
////				ToolTip.SetToolTip(numRunDateBetweenTo, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderBetweenTo));
////				ToolTip.SetToolTip(radRunDateSpecify, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderSpecify));
////				ToolTip.SetToolTip(dtpRunDateFrom, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderSpecifyFrom));
////				ToolTip.SetToolTip(dtpRunDateTo, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_HeaderSpecifyTo));
////
////				ToolTip.SetToolTip(radReleaseDateToday, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseToday));
////				ToolTip.SetToolTip(radReleaseDateBetween, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseBetween));
////				ToolTip.SetToolTip(numReleaseDateBetweenFrom, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseBetweenFrom));
////				ToolTip.SetToolTip(numReleaseDateBetweenTo, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseBetweenTo));
////				ToolTip.SetToolTip(radReleaseDateSpecify, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseSpecify));
////				ToolTip.SetToolTip(dtpReleaseDateFrom, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseSpecifyFrom));
////				ToolTip.SetToolTip(dtpReleaseDateFrom, MIDText.GetTextOnly(eMIDTextCode.hlp_AllocationAuditFilter_ReleaseSpecifyTo));
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }


//        private void btnApply_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                SaveChanges();
//                if (!ErrorFound && _continueWithSave)
//                {
//                    this.Close();
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        override protected bool SaveChanges()
//        {
//            try
//            {
//                ErrorFound = false;
//                bool filterValid = true;
//                _continueWithSave = true;
//                DataRowView dvRow;
//                filterValid = ValidateFilter();
	
//                if (_continueWithSave)
//                {
//                    if (filterValid)
//                    {
//                        // set run dates
//                        _afp.RunDateBetweenFrom = Convert.ToInt32(numRunDateBetweenFrom.Value, CultureInfo.CurrentCulture);
//                        _afp.RunDateBetweenTo = Convert.ToInt32(numRunDateBetweenTo.Value, CultureInfo.CurrentCulture);
//                        _afp.RunDateFrom = dtpRunDateFrom.Value;
//                        _afp.RunDateTo = dtpRunDateTo.Value;

//                        // Begin Track #5803 - JSmith - object reference error
//                        //dvRow = (DataRowView)cboProcessMessageLevel.SelectedItem;
//                        //_afp.HighestProcessMessageLevel = Convert.ToInt32(dvRow.Row["TEXT_CODE"]);

//                        //dvRow = (DataRowView)cboDetailMessageLevel.SelectedItem;
//                        //_afp.HighestDetailMessageLevel = Convert.ToInt32(dvRow.Row["TEXT_CODE"]);

//                        if (cboProcessMessageLevel.SelectedItem != null)
//                        {
//                            dvRow = (DataRowView)cboProcessMessageLevel.SelectedItem;
//                            _afp.HighestProcessMessageLevel = Convert.ToInt32(dvRow.Row["TEXT_CODE"]);
//                        }

//                        if (cboDetailMessageLevel.SelectedItem != null)
//                        {
//                            dvRow = (DataRowView)cboDetailMessageLevel.SelectedItem;
//                            _afp.HighestDetailMessageLevel = Convert.ToInt32(dvRow.Row["TEXT_CODE"]);
//                        }
//                        // End Track #5803

//                        _afp.Duration = Convert.ToInt32(numDurationGreaterThan.Value);

//                        _afp.ShowCompletedProcesses = cbxCompleted.Checked;
//                        _afp.ShowRunningProcesses = cbxRunning.Checked;
//                        _afp.ShowMyTasksOnly = cbxMyTasksOnly.Checked;
                        
//                        _afp.WriteFilter();
                        
//                        ChangePending = false;

//                        AuditFilterChangeEventArgs ea = new AuditFilterChangeEventArgs();
//                        if (OnAuditFilterChangeHandler != null)  // throw event to viewer to apply changes
//                        {
//                            OnAuditFilterChangeHandler(this, ea);
//                        }
//                    }
//                    else
//                    {
//                        ErrorFound = true;
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                ErrorFound = true;
//                HandleException(exc);
//            }

//            return ErrorFound;
//        }

//        private bool ValidateFilter()
//        {
//            bool filterValid = true;
//            string errorMessage = null;
//            try
//            {
//                ErrorProvider.SetError (this.lblRunDateBetweenDays,"");
//                if (radRunDateBetween.Checked)
//                {
//                    if (numRunDateBetweenFrom.Value > numRunDateBetweenTo.Value)
//                    {
//                        filterValid = false;
//                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
//                        ErrorProvider.SetError (this.lblRunDateBetweenDays,errorMessage);
//                    }
//                }

//                ErrorProvider.SetError (this.dtpRunDateFrom,"");
//                if (radRunDateSpecify.Checked)
//                {
//                    if (dtpRunDateFrom.Value > dtpRunDateTo.Value)
//                    {
//                        filterValid = false;
//                        errorMessage = _SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_FromDateError);
//                        ErrorProvider.SetError (this.dtpRunDateFrom,errorMessage);
//                    }
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//            return filterValid;
//        }

//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            try
//            {
//                this.Close();
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        private void radRunDateToday_CheckedChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                numRunDateBetweenFrom.Enabled = false;
//                numRunDateBetweenTo.Enabled = false;
//                dtpRunDateFrom.Enabled = false;
//                dtpRunDateTo.Enabled = false;
//                if (FormLoaded)
//                {
//                    ChangePending = true;
//                    _afp.RunDateType = eFilterDateType.today;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        private void radRunDateBetween_CheckedChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                numRunDateBetweenFrom.Enabled = true;
//                numRunDateBetweenTo.Enabled = true;
//                dtpRunDateFrom.Enabled = false;
//                dtpRunDateTo.Enabled = false;
//                if (FormLoaded)
//                {
//                    ChangePending = true;
//                    _afp.RunDateType = eFilterDateType.between;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        private void radRunDateSpecify_CheckedChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                numRunDateBetweenFrom.Enabled = false;
//                numRunDateBetweenTo.Enabled = false;
//                dtpRunDateFrom.Enabled = true;
//                dtpRunDateTo.Enabled = true;
//                if (FormLoaded)
//                {
//                    ChangePending = true;
//                    _afp.RunDateType = eFilterDateType.specify;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        private void radRunDateAll_CheckedChanged(object sender, System.EventArgs e)
//        {
//            try
//            {
//                numRunDateBetweenFrom.Enabled = false;
//                numRunDateBetweenTo.Enabled = false;
//                dtpRunDateFrom.Enabled = false;
//                dtpRunDateTo.Enabled = false;
//                if (FormLoaded)
//                {
//                    if (radRunDateAll.Checked)
//                    {
//                        MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_PerformanceWarning), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                    }
//                    ChangePending = true;
//                    _afp.RunDateType = eFilterDateType.all;
//                }
//            }
//            catch (Exception exc)
//            {
//                HandleException(exc);
//            }
//        }

//        private void Date_ValueChanged(object sender, System.EventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
		
//        }

//        private void num_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
//            if (FormLoaded)
//            {
//                ChangePending = true;
//            }
//        }
//    }

//    public class AuditFilterChangeEventArgs : EventArgs
//    {
//        bool _formClosing;
		
//        public AuditFilterChangeEventArgs()
//        {
//            _formClosing = false;
//        }
//        public bool FormClosing 
//        {
//            get { return _formClosing ; }
//            set { _formClosing = value; }
//        }
//    }
//}
