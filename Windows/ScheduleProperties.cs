using System;
using System.Diagnostics;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for SchedulerProperties.
	/// </summary>
	public class frmScheduleProperties : MIDFormBase
	{
		#region Windows Form Designer generated code

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.DateTimePicker dtpRangeStartDate;
		private System.Windows.Forms.DateTimePicker dtpRangeEndDate;
		private System.Windows.Forms.DateTimePicker dtpStart;
		private System.Windows.Forms.CheckBox chkRangeEndDate;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.GroupBox grpByWeek;
		private System.Windows.Forms.GroupBox grpByDay;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.DateTimePicker dtpDuration;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox grpFileCondition;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Button btnDetails;
		private System.Windows.Forms.Button btnHide;
		private System.Windows.Forms.GroupBox grpRepeat;
		private System.Windows.Forms.GroupBox grpCondition;
		private System.Windows.Forms.GroupBox grpDateRange;
		private System.Windows.Forms.TextBox txtJobName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox txtScheduleName;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboScheduleByType;
		private System.Windows.Forms.GroupBox grpByMonth;
		private System.Windows.Forms.NumericUpDown nudMinutes;
		private System.Windows.Forms.NumericUpDown nudHours;
		private System.Windows.Forms.NumericUpDown nudRepeat;
		private System.Windows.Forms.NumericUpDown nudMonths;
		private System.Windows.Forms.NumericUpDown nudWeeks;
		private System.Windows.Forms.NumericUpDown nudDays;
		private System.Windows.Forms.CheckBox chkMonthSunday;
		private System.Windows.Forms.CheckBox chkMonthSaturday;
		private System.Windows.Forms.CheckBox chkMonthFriday;
		private System.Windows.Forms.CheckBox chkMonthThursday;
		private System.Windows.Forms.CheckBox chkMonthWednesday;
		private System.Windows.Forms.CheckBox chkMonthTuesday;
		private System.Windows.Forms.CheckBox chkMonthMonday;
		private System.Windows.Forms.CheckBox chkWeekSunday;
		private System.Windows.Forms.CheckBox chkWeekSaturday;
		private System.Windows.Forms.CheckBox chkWeekFriday;
		private System.Windows.Forms.CheckBox chkWeekThursday;
		private System.Windows.Forms.CheckBox chkWeekWednesday;
		private System.Windows.Forms.CheckBox chkWeekTuesday;
		private System.Windows.Forms.CheckBox chkWeekMonday;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboScheduleByMonthWeekType;
		private System.Windows.Forms.CheckBox chkDuration;
		private System.Windows.Forms.CheckBox chkUntil;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboRepeatType;
		private System.Windows.Forms.DateTimePicker dtpUntilTime;
		private System.Windows.Forms.CheckBox chkTerminateAfterConditionMet;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboConditionType;
		private System.Windows.Forms.CheckBox chkRepeatUntilSuccessful;
		private System.Windows.Forms.Button btnDirectory;
		private System.Windows.Forms.TextBox txtConditionDirectory;
		private System.Windows.Forms.TextBox txtConditionMask;
		private System.Windows.Forms.FolderBrowserDialog fbdConditionDirectory;
		private System.Windows.Forms.Label label19;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				this.chkRepeatUntilSuccessful.CheckedChanged -= new System.EventHandler(this.chkRepeatUntilSuccessful_CheckedChanged);
				this.nudMinutes.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.nudHours.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkDuration.CheckedChanged -= new System.EventHandler(this.chkDuration_CheckedChanged);
				this.chkUntil.CheckedChanged -= new System.EventHandler(this.chkUntil_CheckedChanged);
				this.cboRepeatType.SelectionChangeCommitted -= new System.EventHandler(this.cboRepeatType_SelectionChangeCommitted);
				this.nudRepeat.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.dtpUntilTime.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkTerminateAfterConditionMet.CheckedChanged -= new System.EventHandler(this.chkTerminateAfterConditionMet_CheckedChanged);
				this.dtpStart.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkRangeEndDate.CheckedChanged -= new System.EventHandler(this.chkRangeEndDate_CheckedChanged);
				this.dtpRangeEndDate.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.dtpRangeStartDate.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.nudMonths.ValueChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthSunday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.cboScheduleByMonthWeekType.SelectionChangeCommitted -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthSaturday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthFriday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthThursday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthWednesday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthTuesday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.chkMonthMonday.CheckedChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.btnDirectory.Click -= new System.EventHandler(this.btnDirectory_Click);
				this.txtConditionDirectory.TextChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.txtConditionMask.TextChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.cboConditionType.SelectionChangeCommitted -= new System.EventHandler(this.cboConditionType_SelectionChangeCommitted);
				this.cboScheduleByType.SelectionChangeCommitted -= new System.EventHandler(this.cboScheduleByType_SelectionChangeCommitted);
				this.btnOK.Click -= new System.EventHandler(this.btnOK_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnDetails.Click -= new System.EventHandler(this.btnDetails_Click);
				this.btnHide.Click -= new System.EventHandler(this.btnHide_Click);
				this.txtScheduleName.TextChanged -= new System.EventHandler(this.generic_ValueChanged);
				this.Load -= new System.EventHandler(this.frmScheduleProperties_Load);
				this.Activated -= new System.EventHandler(this.frmScheduleProperties_Activated);
                this.cboRepeatType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboRepeatType_MIDComboBoxPropertiesChangedEvent);
                this.cboScheduleByMonthWeekType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboScheduleByMonthWeekType_MIDComboBoxPropertiesChangedEvent);
                this.cboConditionType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboConditionType_MIDComboBoxPropertiesChangedEvent);
                this.cboScheduleByType.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboScheduleByType_MIDComboBoxPropertiesChangedEvent);
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpRepeat = new System.Windows.Forms.GroupBox();
			this.chkRepeatUntilSuccessful = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.nudMinutes = new System.Windows.Forms.NumericUpDown();
			this.nudHours = new System.Windows.Forms.NumericUpDown();
			this.chkDuration = new System.Windows.Forms.CheckBox();
			this.chkUntil = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cboRepeatType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.nudRepeat = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.dtpUntilTime = new System.Windows.Forms.DateTimePicker();
			this.chkTerminateAfterConditionMet = new System.Windows.Forms.CheckBox();
			this.dtpStart = new System.Windows.Forms.DateTimePicker();
			this.grpDateRange = new System.Windows.Forms.GroupBox();
			this.label19 = new System.Windows.Forms.Label();
			this.chkRangeEndDate = new System.Windows.Forms.CheckBox();
			this.dtpRangeEndDate = new System.Windows.Forms.DateTimePicker();
			this.dtpRangeStartDate = new System.Windows.Forms.DateTimePicker();
			this.label16 = new System.Windows.Forms.Label();
			this.grpByMonth = new System.Windows.Forms.GroupBox();
			this.nudMonths = new System.Windows.Forms.NumericUpDown();
			this.label13 = new System.Windows.Forms.Label();
			this.chkMonthSunday = new System.Windows.Forms.CheckBox();
			this.cboScheduleByMonthWeekType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.chkMonthSaturday = new System.Windows.Forms.CheckBox();
			this.chkMonthFriday = new System.Windows.Forms.CheckBox();
			this.chkMonthThursday = new System.Windows.Forms.CheckBox();
			this.chkMonthWednesday = new System.Windows.Forms.CheckBox();
			this.chkMonthTuesday = new System.Windows.Forms.CheckBox();
			this.chkMonthMonday = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.nudWeeks = new System.Windows.Forms.NumericUpDown();
			this.label11 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.nudDays = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.grpCondition = new System.Windows.Forms.GroupBox();
			this.grpFileCondition = new System.Windows.Forms.GroupBox();
			this.btnDirectory = new System.Windows.Forms.Button();
			this.txtConditionDirectory = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtConditionMask = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cboConditionType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboScheduleByType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.grpByWeek = new System.Windows.Forms.GroupBox();
			this.chkWeekSunday = new System.Windows.Forms.CheckBox();
			this.chkWeekSaturday = new System.Windows.Forms.CheckBox();
			this.chkWeekFriday = new System.Windows.Forms.CheckBox();
			this.chkWeekThursday = new System.Windows.Forms.CheckBox();
			this.chkWeekWednesday = new System.Windows.Forms.CheckBox();
			this.chkWeekTuesday = new System.Windows.Forms.CheckBox();
			this.chkWeekMonday = new System.Windows.Forms.CheckBox();
			this.grpByDay = new System.Windows.Forms.GroupBox();
			this.dtpDuration = new System.Windows.Forms.DateTimePicker();
			this.label17 = new System.Windows.Forms.Label();
			this.txtJobName = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnDetails = new System.Windows.Forms.Button();
			this.btnHide = new System.Windows.Forms.Button();
			this.txtScheduleName = new System.Windows.Forms.TextBox();
			this.label18 = new System.Windows.Forms.Label();
			this.fbdConditionDirectory = new System.Windows.Forms.FolderBrowserDialog();
			this.grpRepeat.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHours)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRepeat)).BeginInit();
			this.grpDateRange.SuspendLayout();
			this.grpByMonth.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMonths)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWeeks)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDays)).BeginInit();
			this.grpCondition.SuspendLayout();
			this.grpFileCondition.SuspendLayout();
			this.grpByWeek.SuspendLayout();
			this.grpByDay.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpRepeat
			// 
			this.grpRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpRepeat.Controls.Add(this.chkRepeatUntilSuccessful);
			this.grpRepeat.Controls.Add(this.label7);
			this.grpRepeat.Controls.Add(this.label6);
			this.grpRepeat.Controls.Add(this.nudMinutes);
			this.grpRepeat.Controls.Add(this.nudHours);
			this.grpRepeat.Controls.Add(this.chkDuration);
			this.grpRepeat.Controls.Add(this.chkUntil);
			this.grpRepeat.Controls.Add(this.label4);
			this.grpRepeat.Controls.Add(this.cboRepeatType);
			this.grpRepeat.Controls.Add(this.nudRepeat);
			this.grpRepeat.Controls.Add(this.label5);
			this.grpRepeat.Controls.Add(this.dtpUntilTime);
			this.grpRepeat.Controls.Add(this.chkTerminateAfterConditionMet);
			this.grpRepeat.Location = new System.Drawing.Point(304, 104);
			this.grpRepeat.Name = "grpRepeat";
			this.grpRepeat.Size = new System.Drawing.Size(288, 184);
			this.grpRepeat.TabIndex = 0;
			this.grpRepeat.TabStop = false;
			this.grpRepeat.Text = "Repeat";
			// 
			// chkRepeatUntilSuccessful
			// 
			this.chkRepeatUntilSuccessful.Location = new System.Drawing.Point(16, 152);
			this.chkRepeatUntilSuccessful.Name = "chkRepeatUntilSuccessful";
			this.chkRepeatUntilSuccessful.Size = new System.Drawing.Size(216, 24);
			this.chkRepeatUntilSuccessful.TabIndex = 19;
			this.chkRepeatUntilSuccessful.Text = "Terminate repeat after Job successful";
			this.chkRepeatUntilSuccessful.CheckedChanged += new System.EventHandler(this.chkRepeatUntilSuccessful_CheckedChanged);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(184, 103);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 23);
			this.label7.TabIndex = 18;
			this.label7.Text = "Minutes";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(184, 81);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(40, 23);
			this.label6.TabIndex = 17;
			this.label6.Text = "Hours";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudMinutes
			// 
			this.nudMinutes.Location = new System.Drawing.Point(128, 104);
			this.nudMinutes.Name = "nudMinutes";
			this.nudMinutes.Size = new System.Drawing.Size(56, 20);
			this.nudMinutes.TabIndex = 16;
			this.nudMinutes.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// nudHours
			// 
			this.nudHours.Location = new System.Drawing.Point(128, 82);
			this.nudHours.Name = "nudHours";
			this.nudHours.Size = new System.Drawing.Size(56, 20);
			this.nudHours.TabIndex = 15;
			this.nudHours.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkDuration
			// 
			this.chkDuration.Location = new System.Drawing.Point(56, 80);
			this.chkDuration.Name = "chkDuration";
			this.chkDuration.Size = new System.Drawing.Size(80, 24);
			this.chkDuration.TabIndex = 14;
			this.chkDuration.Text = "Duration:";
			this.chkDuration.CheckedChanged += new System.EventHandler(this.chkDuration_CheckedChanged);
			// 
			// chkUntil
			// 
			this.chkUntil.Location = new System.Drawing.Point(56, 56);
			this.chkUntil.Name = "chkUntil";
			this.chkUntil.Size = new System.Drawing.Size(56, 24);
			this.chkUntil.TabIndex = 13;
			this.chkUntil.Text = "Time:";
			this.chkUntil.CheckedChanged += new System.EventHandler(this.chkUntil_CheckedChanged);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 23);
			this.label4.TabIndex = 12;
			this.label4.Text = "Until";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cboRepeatType
			// 
			this.cboRepeatType.Location = new System.Drawing.Point(120, 24);
			this.cboRepeatType.Name = "cboRepeatType";
			this.cboRepeatType.Size = new System.Drawing.Size(88, 21);
			this.cboRepeatType.TabIndex = 11;
			this.cboRepeatType.SelectionChangeCommitted += new System.EventHandler(this.cboRepeatType_SelectionChangeCommitted);
            this.cboRepeatType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboRepeatType_MIDComboBoxPropertiesChangedEvent);
			// 
			// nudRepeat
			// 
			this.nudRepeat.Location = new System.Drawing.Point(56, 24);
			this.nudRepeat.Name = "nudRepeat";
			this.nudRepeat.Size = new System.Drawing.Size(56, 20);
			this.nudRepeat.TabIndex = 10;
			this.nudRepeat.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 23);
			this.label5.TabIndex = 9;
			this.label5.Text = "Every";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dtpUntilTime
			// 
			this.dtpUntilTime.CustomFormat = "hh:mm tt";
			this.dtpUntilTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpUntilTime.Location = new System.Drawing.Point(120, 56);
			this.dtpUntilTime.Name = "dtpUntilTime";
			this.dtpUntilTime.ShowUpDown = true;
			this.dtpUntilTime.Size = new System.Drawing.Size(80, 20);
			this.dtpUntilTime.TabIndex = 8;
			this.dtpUntilTime.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkTerminateAfterConditionMet
			// 
			this.chkTerminateAfterConditionMet.Location = new System.Drawing.Point(16, 128);
			this.chkTerminateAfterConditionMet.Name = "chkTerminateAfterConditionMet";
			this.chkTerminateAfterConditionMet.Size = new System.Drawing.Size(208, 24);
			this.chkTerminateAfterConditionMet.TabIndex = 6;
			this.chkTerminateAfterConditionMet.Text = "Terminate repeat after condition met";
			this.chkTerminateAfterConditionMet.CheckedChanged += new System.EventHandler(this.chkTerminateAfterConditionMet_CheckedChanged);
			// 
			// dtpStart
			// 
			this.dtpStart.CustomFormat = "hh:mm tt";
			this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpStart.Location = new System.Drawing.Point(136, 104);
			this.dtpStart.Name = "dtpStart";
			this.dtpStart.ShowUpDown = true;
			this.dtpStart.Size = new System.Drawing.Size(80, 20);
			this.dtpStart.TabIndex = 7;
			this.dtpStart.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// grpDateRange
			// 
			this.grpDateRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpDateRange.Controls.Add(this.label19);
			this.grpDateRange.Controls.Add(this.chkRangeEndDate);
			this.grpDateRange.Controls.Add(this.dtpRangeEndDate);
			this.grpDateRange.Controls.Add(this.dtpRangeStartDate);
			this.grpDateRange.Controls.Add(this.label16);
			this.grpDateRange.Location = new System.Drawing.Point(304, 16);
			this.grpDateRange.Name = "grpDateRange";
			this.grpDateRange.Size = new System.Drawing.Size(288, 80);
			this.grpDateRange.TabIndex = 1;
			this.grpDateRange.TabStop = false;
			this.grpDateRange.Text = "Date Range";
			// 
			// label19
			// 
			this.label19.Location = new System.Drawing.Point(40, 48);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(56, 23);
			this.label19.TabIndex = 11;
			this.label19.Text = "End Date:";
			this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkRangeEndDate
			// 
			this.chkRangeEndDate.Location = new System.Drawing.Point(16, 48);
			this.chkRangeEndDate.Name = "chkRangeEndDate";
			this.chkRangeEndDate.Size = new System.Drawing.Size(16, 24);
			this.chkRangeEndDate.TabIndex = 10;
			this.chkRangeEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkRangeEndDate.CheckedChanged += new System.EventHandler(this.chkRangeEndDate_CheckedChanged);
			// 
			// dtpRangeEndDate
			// 
			this.dtpRangeEndDate.CustomFormat = "MM/dd/yyyy";
			this.dtpRangeEndDate.Enabled = false;
			this.dtpRangeEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpRangeEndDate.Location = new System.Drawing.Point(104, 48);
			this.dtpRangeEndDate.Name = "dtpRangeEndDate";
			this.dtpRangeEndDate.Size = new System.Drawing.Size(88, 20);
			this.dtpRangeEndDate.TabIndex = 8;
			this.dtpRangeEndDate.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// dtpRangeStartDate
			// 
			this.dtpRangeStartDate.CustomFormat = "MM/dd/yyyy";
			this.dtpRangeStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpRangeStartDate.Location = new System.Drawing.Point(104, 24);
			this.dtpRangeStartDate.Name = "dtpRangeStartDate";
			this.dtpRangeStartDate.Size = new System.Drawing.Size(88, 20);
			this.dtpRangeStartDate.TabIndex = 7;
			this.dtpRangeStartDate.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label16
			// 
			this.label16.Location = new System.Drawing.Point(32, 24);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(64, 23);
			this.label16.TabIndex = 4;
			this.label16.Text = "Start Date:";
			this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpByMonth
			// 
			this.grpByMonth.Controls.Add(this.nudMonths);
			this.grpByMonth.Controls.Add(this.label13);
			this.grpByMonth.Controls.Add(this.chkMonthSunday);
			this.grpByMonth.Controls.Add(this.cboScheduleByMonthWeekType);
			this.grpByMonth.Controls.Add(this.chkMonthSaturday);
			this.grpByMonth.Controls.Add(this.chkMonthFriday);
			this.grpByMonth.Controls.Add(this.chkMonthThursday);
			this.grpByMonth.Controls.Add(this.chkMonthWednesday);
			this.grpByMonth.Controls.Add(this.chkMonthTuesday);
			this.grpByMonth.Controls.Add(this.chkMonthMonday);
			this.grpByMonth.Controls.Add(this.label12);
			this.grpByMonth.Location = new System.Drawing.Point(8, 136);
			this.grpByMonth.Name = "grpByMonth";
			this.grpByMonth.Size = new System.Drawing.Size(288, 168);
			this.grpByMonth.TabIndex = 2;
			this.grpByMonth.TabStop = false;
			this.grpByMonth.Text = "Schedule By Month";
			// 
			// nudMonths
			// 
			this.nudMonths.Location = new System.Drawing.Point(104, 25);
			this.nudMonths.Minimum = new System.Decimal(new int[] {
																	  1,
																	  0,
																	  0,
																	  0});
			this.nudMonths.Name = "nudMonths";
			this.nudMonths.Size = new System.Drawing.Size(56, 20);
			this.nudMonths.TabIndex = 3;
			this.nudMonths.Value = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.nudMonths.ValueChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(40, 24);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(56, 23);
			this.label13.TabIndex = 2;
			this.label13.Text = "Every";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkMonthSunday
			// 
			this.chkMonthSunday.Location = new System.Drawing.Point(16, 136);
			this.chkMonthSunday.Name = "chkMonthSunday";
			this.chkMonthSunday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthSunday.TabIndex = 11;
			this.chkMonthSunday.Text = "Sunday";
			this.chkMonthSunday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// cboScheduleByMonthWeekType
			// 
			this.cboScheduleByMonthWeekType.Location = new System.Drawing.Point(16, 56);
			this.cboScheduleByMonthWeekType.Name = "cboScheduleByMonthWeekType";
			this.cboScheduleByMonthWeekType.Size = new System.Drawing.Size(112, 21);
			this.cboScheduleByMonthWeekType.TabIndex = 12;
			this.cboScheduleByMonthWeekType.SelectionChangeCommitted += new System.EventHandler(this.generic_ValueChanged);
            this.cboScheduleByMonthWeekType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboScheduleByMonthWeekType_MIDComboBoxPropertiesChangedEvent);
			// 
			// chkMonthSaturday
			// 
			this.chkMonthSaturday.Location = new System.Drawing.Point(176, 112);
			this.chkMonthSaturday.Name = "chkMonthSaturday";
			this.chkMonthSaturday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthSaturday.TabIndex = 10;
			this.chkMonthSaturday.Text = "Saturday";
			this.chkMonthSaturday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkMonthFriday
			// 
			this.chkMonthFriday.Location = new System.Drawing.Point(96, 112);
			this.chkMonthFriday.Name = "chkMonthFriday";
			this.chkMonthFriday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthFriday.TabIndex = 9;
			this.chkMonthFriday.Text = "Friday";
			this.chkMonthFriday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkMonthThursday
			// 
			this.chkMonthThursday.Location = new System.Drawing.Point(16, 112);
			this.chkMonthThursday.Name = "chkMonthThursday";
			this.chkMonthThursday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthThursday.TabIndex = 8;
			this.chkMonthThursday.Text = "Thursday";
			this.chkMonthThursday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkMonthWednesday
			// 
			this.chkMonthWednesday.Location = new System.Drawing.Point(176, 88);
			this.chkMonthWednesday.Name = "chkMonthWednesday";
			this.chkMonthWednesday.Size = new System.Drawing.Size(88, 16);
			this.chkMonthWednesday.TabIndex = 7;
			this.chkMonthWednesday.Text = "Wednesday";
			this.chkMonthWednesday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkMonthTuesday
			// 
			this.chkMonthTuesday.Location = new System.Drawing.Point(96, 88);
			this.chkMonthTuesday.Name = "chkMonthTuesday";
			this.chkMonthTuesday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthTuesday.TabIndex = 6;
			this.chkMonthTuesday.Text = "Tuesday";
			this.chkMonthTuesday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// chkMonthMonday
			// 
			this.chkMonthMonday.Location = new System.Drawing.Point(16, 88);
			this.chkMonthMonday.Name = "chkMonthMonday";
			this.chkMonthMonday.Size = new System.Drawing.Size(80, 16);
			this.chkMonthMonday.TabIndex = 5;
			this.chkMonthMonday.Text = "Monday";
			this.chkMonthMonday.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(168, 24);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(72, 23);
			this.label12.TabIndex = 4;
			this.label12.Text = "Month(s) on:";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(160, 32);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(72, 23);
			this.label10.TabIndex = 4;
			this.label10.Text = "Week(s) on:";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudWeeks
			// 
			this.nudWeeks.Location = new System.Drawing.Point(104, 33);
			this.nudWeeks.Minimum = new System.Decimal(new int[] {
																	 1,
																	 0,
																	 0,
																	 0});
			this.nudWeeks.Name = "nudWeeks";
			this.nudWeeks.Size = new System.Drawing.Size(48, 20);
			this.nudWeeks.TabIndex = 3;
			this.nudWeeks.Value = new System.Decimal(new int[] {
																   1,
																   0,
																   0,
																   0});
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(56, 32);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(40, 23);
			this.label11.TabIndex = 2;
			this.label11.Text = "Every";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(168, 48);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(56, 23);
			this.label8.TabIndex = 4;
			this.label8.Text = "Days";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// nudDays
			// 
			this.nudDays.Location = new System.Drawing.Point(104, 49);
			this.nudDays.Minimum = new System.Decimal(new int[] {
																	1,
																	0,
																	0,
																	0});
			this.nudDays.Name = "nudDays";
			this.nudDays.Size = new System.Drawing.Size(56, 20);
			this.nudDays.TabIndex = 3;
			this.nudDays.Value = new System.Decimal(new int[] {
																  1,
																  0,
																  0,
																  0});
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(40, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 23);
			this.label9.TabIndex = 2;
			this.label9.Text = "Every";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// grpCondition
			// 
			this.grpCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpCondition.Controls.Add(this.grpFileCondition);
			this.grpCondition.Controls.Add(this.label1);
			this.grpCondition.Controls.Add(this.cboConditionType);
			this.grpCondition.Location = new System.Drawing.Point(304, 296);
			this.grpCondition.Name = "grpCondition";
			this.grpCondition.Size = new System.Drawing.Size(288, 152);
			this.grpCondition.TabIndex = 3;
			this.grpCondition.TabStop = false;
			this.grpCondition.Text = "Condition";
			// 
			// grpFileCondition
			// 
			this.grpFileCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpFileCondition.Controls.Add(this.btnDirectory);
			this.grpFileCondition.Controls.Add(this.txtConditionDirectory);
			this.grpFileCondition.Controls.Add(this.label2);
			this.grpFileCondition.Controls.Add(this.txtConditionMask);
			this.grpFileCondition.Controls.Add(this.label3);
			this.grpFileCondition.Location = new System.Drawing.Point(8, 56);
			this.grpFileCondition.Name = "grpFileCondition";
			this.grpFileCondition.Size = new System.Drawing.Size(272, 88);
			this.grpFileCondition.TabIndex = 8;
			this.grpFileCondition.TabStop = false;
			this.grpFileCondition.Text = "Trigger File";
			// 
			// btnDirectory
			// 
			this.btnDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDirectory.Location = new System.Drawing.Point(192, 24);
			this.btnDirectory.Name = "btnDirectory";
			this.btnDirectory.Size = new System.Drawing.Size(72, 20);
			this.btnDirectory.TabIndex = 10;
			this.btnDirectory.Text = "Directory...";
			this.btnDirectory.Click += new System.EventHandler(this.btnDirectory_Click);
			// 
			// txtConditionDirectory
			// 
			this.txtConditionDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.txtConditionDirectory.Location = new System.Drawing.Point(72, 24);
			this.txtConditionDirectory.Name = "txtConditionDirectory";
			this.txtConditionDirectory.Size = new System.Drawing.Size(112, 20);
			this.txtConditionDirectory.TabIndex = 6;
			this.txtConditionDirectory.Text = "C:\\Input Directory";
			this.txtConditionDirectory.TextChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(56, 23);
			this.label2.TabIndex = 7;
			this.label2.Text = "Directory:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtConditionMask
			// 
			this.txtConditionMask.Location = new System.Drawing.Point(72, 56);
			this.txtConditionMask.Name = "txtConditionMask";
			this.txtConditionMask.Size = new System.Drawing.Size(112, 20);
			this.txtConditionMask.TabIndex = 8;
			this.txtConditionMask.Text = ".TRG";
			this.txtConditionMask.TextChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 56);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 9;
			this.label3.Text = "File Suffix:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "Condition:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cboConditionType
			// 
			this.cboConditionType.Location = new System.Drawing.Point(64, 24);
			this.cboConditionType.Name = "cboConditionType";
			this.cboConditionType.Size = new System.Drawing.Size(120, 21);
			this.cboConditionType.TabIndex = 0;
			this.cboConditionType.SelectionChangeCommitted += new System.EventHandler(this.cboConditionType_SelectionChangeCommitted);
            this.cboConditionType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboConditionType_MIDComboBoxPropertiesChangedEvent);
			// 
			// cboScheduleByType
			// 
			this.cboScheduleByType.Location = new System.Drawing.Point(8, 104);
			this.cboScheduleByType.Name = "cboScheduleByType";
			this.cboScheduleByType.Size = new System.Drawing.Size(112, 21);
			this.cboScheduleByType.TabIndex = 4;
			this.cboScheduleByType.SelectionChangeCommitted += new System.EventHandler(this.cboScheduleByType_SelectionChangeCommitted);
            this.cboScheduleByType.MIDComboBoxPropertiesChangedEvent += new MIDComboBoxPropertiesChangedEventHandler(cboScheduleByType_MIDComboBoxPropertiesChangedEvent);
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(8, 80);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(64, 23);
			this.label14.TabIndex = 5;
			this.label14.Text = "Schedule";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(136, 80);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(64, 23);
			this.label15.TabIndex = 8;
			this.label15.Text = "Start Time";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// grpByWeek
			// 
			this.grpByWeek.Controls.Add(this.chkWeekSunday);
			this.grpByWeek.Controls.Add(this.chkWeekSaturday);
			this.grpByWeek.Controls.Add(this.chkWeekFriday);
			this.grpByWeek.Controls.Add(this.chkWeekThursday);
			this.grpByWeek.Controls.Add(this.chkWeekWednesday);
			this.grpByWeek.Controls.Add(this.chkWeekTuesday);
			this.grpByWeek.Controls.Add(this.chkWeekMonday);
			this.grpByWeek.Controls.Add(this.label11);
			this.grpByWeek.Controls.Add(this.nudWeeks);
			this.grpByWeek.Controls.Add(this.label10);
			this.grpByWeek.Location = new System.Drawing.Point(8, 136);
			this.grpByWeek.Name = "grpByWeek";
			this.grpByWeek.Size = new System.Drawing.Size(288, 168);
			this.grpByWeek.TabIndex = 17;
			this.grpByWeek.TabStop = false;
			this.grpByWeek.Text = "Schedule By Week";
			// 
			// chkWeekSunday
			// 
			this.chkWeekSunday.Location = new System.Drawing.Point(16, 112);
			this.chkWeekSunday.Name = "chkWeekSunday";
			this.chkWeekSunday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekSunday.TabIndex = 18;
			this.chkWeekSunday.Text = "Sunday";
			// 
			// chkWeekSaturday
			// 
			this.chkWeekSaturday.Location = new System.Drawing.Point(176, 88);
			this.chkWeekSaturday.Name = "chkWeekSaturday";
			this.chkWeekSaturday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekSaturday.TabIndex = 17;
			this.chkWeekSaturday.Text = "Saturday";
			// 
			// chkWeekFriday
			// 
			this.chkWeekFriday.Location = new System.Drawing.Point(96, 88);
			this.chkWeekFriday.Name = "chkWeekFriday";
			this.chkWeekFriday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekFriday.TabIndex = 16;
			this.chkWeekFriday.Text = "Friday";
			// 
			// chkWeekThursday
			// 
			this.chkWeekThursday.Location = new System.Drawing.Point(16, 88);
			this.chkWeekThursday.Name = "chkWeekThursday";
			this.chkWeekThursday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekThursday.TabIndex = 15;
			this.chkWeekThursday.Text = "Thursday";
			// 
			// chkWeekWednesday
			// 
			this.chkWeekWednesday.Location = new System.Drawing.Point(176, 64);
			this.chkWeekWednesday.Name = "chkWeekWednesday";
			this.chkWeekWednesday.Size = new System.Drawing.Size(88, 16);
			this.chkWeekWednesday.TabIndex = 14;
			this.chkWeekWednesday.Text = "Wednesday";
			// 
			// chkWeekTuesday
			// 
			this.chkWeekTuesday.Location = new System.Drawing.Point(96, 64);
			this.chkWeekTuesday.Name = "chkWeekTuesday";
			this.chkWeekTuesday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekTuesday.TabIndex = 13;
			this.chkWeekTuesday.Text = "Tuesday";
			// 
			// chkWeekMonday
			// 
			this.chkWeekMonday.Location = new System.Drawing.Point(16, 64);
			this.chkWeekMonday.Name = "chkWeekMonday";
			this.chkWeekMonday.Size = new System.Drawing.Size(80, 16);
			this.chkWeekMonday.TabIndex = 12;
			this.chkWeekMonday.Text = "Monday";
			// 
			// grpByDay
			// 
			this.grpByDay.Controls.Add(this.label8);
			this.grpByDay.Controls.Add(this.nudDays);
			this.grpByDay.Controls.Add(this.label9);
			this.grpByDay.Location = new System.Drawing.Point(8, 136);
			this.grpByDay.Name = "grpByDay";
			this.grpByDay.Size = new System.Drawing.Size(288, 168);
			this.grpByDay.TabIndex = 18;
			this.grpByDay.TabStop = false;
			this.grpByDay.Text = "Schedule By Day";
			// 
			// dtpDuration
			// 
			this.dtpDuration.CustomFormat = "hh:mm";
			this.dtpDuration.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpDuration.Location = new System.Drawing.Point(128, 80);
			this.dtpDuration.Name = "dtpDuration";
			this.dtpDuration.ShowUpDown = true;
			this.dtpDuration.Size = new System.Drawing.Size(80, 20);
			this.dtpDuration.TabIndex = 15;
			this.dtpDuration.Value = new System.DateTime(2004, 7, 16, 7, 58, 4, 734);
			// 
			// label17
			// 
			this.label17.Location = new System.Drawing.Point(24, 48);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(40, 23);
			this.label17.TabIndex = 19;
			this.label17.Text = "Job:";
			this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtJobName
			// 
			this.txtJobName.Location = new System.Drawing.Point(64, 48);
			this.txtJobName.Name = "txtJobName";
			this.txtJobName.ReadOnly = true;
			this.txtJobName.Size = new System.Drawing.Size(208, 20);
			this.txtJobName.TabIndex = 20;
			this.txtJobName.Text = "";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.Location = new System.Drawing.Point(452, 456);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 22;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(524, 456);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 23;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnDetails
			// 
			this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDetails.Location = new System.Drawing.Point(380, 456);
			this.btnDetails.Name = "btnDetails";
			this.btnDetails.Size = new System.Drawing.Size(64, 24);
			this.btnDetails.TabIndex = 24;
			this.btnDetails.Text = "Details >>";
			this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
			// 
			// btnHide
			// 
			this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnHide.Location = new System.Drawing.Point(380, 456);
			this.btnHide.Name = "btnHide";
			this.btnHide.Size = new System.Drawing.Size(64, 24);
			this.btnHide.TabIndex = 25;
			this.btnHide.Text = "<< Hide";
			this.btnHide.Click += new System.EventHandler(this.btnHide_Click);
			// 
			// txtScheduleName
			// 
			this.txtScheduleName.Location = new System.Drawing.Point(64, 16);
			this.txtScheduleName.Name = "txtScheduleName";
			this.txtScheduleName.Size = new System.Drawing.Size(208, 20);
			this.txtScheduleName.TabIndex = 27;
			this.txtScheduleName.Text = "";
			this.txtScheduleName.TextChanged += new System.EventHandler(this.generic_ValueChanged);
			// 
			// label18
			// 
			this.label18.Location = new System.Drawing.Point(8, 16);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(56, 23);
			this.label18.TabIndex = 26;
			this.label18.Text = "Schedule:";
			this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmScheduleProperties
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(600, 486);
			this.Controls.Add(this.grpByWeek);
			this.Controls.Add(this.grpByMonth);
			this.Controls.Add(this.grpByDay);
			this.Controls.Add(this.txtScheduleName);
			this.Controls.Add(this.txtJobName);
			this.Controls.Add(this.label18);
			this.Controls.Add(this.btnDetails);
			this.Controls.Add(this.btnHide);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.cboScheduleByType);
			this.Controls.Add(this.grpCondition);
			this.Controls.Add(this.grpDateRange);
			this.Controls.Add(this.grpRepeat);
			this.Controls.Add(this.dtpStart);
			this.Name = "frmScheduleProperties";
			this.Text = "Schedule Job/Task List";
			this.Load += new System.EventHandler(this.frmScheduleProperties_Load);
			this.Activated += new System.EventHandler(this.frmScheduleProperties_Activated);
			this.grpRepeat.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudHours)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRepeat)).EndInit();
			this.grpDateRange.ResumeLayout(false);
			this.grpByMonth.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.nudMonths)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudWeeks)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudDays)).EndInit();
			this.grpCondition.ResumeLayout(false);
			this.grpFileCondition.ResumeLayout(false);
			this.grpByWeek.ResumeLayout(false);
			this.grpByDay.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		const int cHideHeight = 376;
		const int cShowHeight = 520;
		const int cHideWidth = 312;
		const int cShowWidth = 608;
		
		public delegate void SchedulePropertiesSaveEventHandler(object source, SchedulePropertiesSaveEventArgs e);
		public event SchedulePropertiesSaveEventHandler OnSchedulePropertiesSaveHandler;

		private SessionAddressBlock _SAB;
		private FunctionSecurityProfile _secLevel;
		private eScheduleUpdateStatus _updateStat;
		private ScheduleData _dlSchedule;
		private TaskListProfile _taskListProf;
		private JobProfile _jobProf;
		private ScheduleProfile _scheduleProf;

		public frmScheduleProperties(SessionAddressBlock aSAB, FunctionSecurityProfile aSecurityLevel, ScheduleProfile aSchedProfile, JobProfile aJobProfile)
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				_secLevel = aSecurityLevel;
				_updateStat = eScheduleUpdateStatus.UpdateSchedule;
				_taskListProf = null;
				_jobProf = aJobProfile;
				_scheduleProf = aSchedProfile;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public frmScheduleProperties(SessionAddressBlock aSAB, FunctionSecurityProfile aSecurityLevel, JobProfile aJobProfile)
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				_secLevel = aSecurityLevel;
				_updateStat = eScheduleUpdateStatus.CreateSchedule;
				_taskListProf = null;
				_jobProf = aJobProfile;
				_scheduleProf = new ScheduleProfile(-1);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public frmScheduleProperties(SessionAddressBlock aSAB, FunctionSecurityProfile aSecurityLevel, TaskListProfile aTaskListProfile)
			: base(aSAB)
		{
			try
			{
				InitializeComponent();

				_SAB = aSAB;
				_secLevel = aSecurityLevel;
				_updateStat = eScheduleUpdateStatus.CreateScheduleAndJob;
				_taskListProf = aTaskListProfile;
				_jobProf = new JobProfile(-1, _taskListProf.GetUniqueName(), true);
				_scheduleProf = new ScheduleProfile(-1);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public void HandleExceptions(Exception exc)
		{
			Debug.WriteLine(exc.ToString());
			MessageBox.Show(exc.ToString());
		}

		private void frmScheduleProperties_Load(object sender, System.EventArgs e)
		{
			eProcessExecutionStatus currProcStat;

			try
			{
				FormLoaded = false;

				_dlSchedule = new ScheduleData();

				// Set MIDFormBase security

				FunctionSecurity = _secLevel;

				if (_scheduleProf.Key != -1)
				{
					currProcStat = _SAB.SchedulerServerSession.GetJobStatus(_scheduleProf.Key, _jobProf.Key);

					if (_updateStat == eScheduleUpdateStatus.UpdateSchedule &&
						(currProcStat == eProcessExecutionStatus.Running ||
						currProcStat == eProcessExecutionStatus.Waiting ||
						currProcStat == eProcessExecutionStatus.Executed))
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HoldOrCancelBeforeModifying), Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						SetReadOnly(false);
					}
					else
					{
						SetReadOnly(_secLevel.AllowUpdate);
					}
				}
				else
				{
					SetReadOnly(_secLevel.AllowUpdate);
				}

				btnDetails.Enabled = true;
				btnHide.Enabled = true;
				txtJobName.Enabled = false;

				// Setup combo lists

				cboScheduleByType.Items.Add(new ComboObject((int)eScheduleByType.Once, MIDText.GetTextOnly((int)eScheduleByType.Once)));
				cboScheduleByType.Items.Add(new ComboObject((int)eScheduleByType.Day, MIDText.GetTextOnly((int)eScheduleByType.Day)));
				cboScheduleByType.Items.Add(new ComboObject((int)eScheduleByType.Week, MIDText.GetTextOnly((int)eScheduleByType.Week)));
				cboScheduleByType.Items.Add(new ComboObject((int)eScheduleByType.Month, MIDText.GetTextOnly((int)eScheduleByType.Month)));

				cboConditionType.Items.Add(new ComboObject((int)eScheduleConditionType.None, "(None)"));
				cboConditionType.Items.Add(new ComboObject((int)eScheduleConditionType.ByFileExtension, MIDText.GetTextOnly((int)eScheduleConditionType.ByFileExtension)));

				cboRepeatType.Items.Add(new ComboObject((int)eScheduleRepeatIntervalType.None, "(None)"));
				cboRepeatType.Items.Add(new ComboObject((int)eScheduleRepeatIntervalType.Seconds, MIDText.GetTextOnly((int)eScheduleRepeatIntervalType.Seconds)));
				cboRepeatType.Items.Add(new ComboObject((int)eScheduleRepeatIntervalType.Minutes, MIDText.GetTextOnly((int)eScheduleRepeatIntervalType.Minutes)));
				cboRepeatType.Items.Add(new ComboObject((int)eScheduleRepeatIntervalType.Hours, MIDText.GetTextOnly((int)eScheduleRepeatIntervalType.Hours)));

				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.Every, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.Every)));
				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.First, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.First)));
				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.Second, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.Second)));
				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.Third, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.Third)));
				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.Fourth, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.Fourth)));
				cboScheduleByMonthWeekType.Items.Add(new ComboObject((int)eScheduleByMonthWeekType.Last, MIDText.GetTextOnly((int)eScheduleByMonthWeekType.Last)));

				txtJobName.Text = _jobProf.Name;
				grpByDay.Visible = false;
				grpByWeek.Visible = false;
				grpByMonth.Visible = false;
				grpFileCondition.Visible = false;
				chkRangeEndDate.Checked = false;
				chkRangeEndDate.Enabled = false;
				chkUntil.Checked = false;
				chkDuration.Checked = false;
				chkRepeatUntilSuccessful.Checked = false;
				chkTerminateAfterConditionMet.Checked = false;
				dtpRangeEndDate.Enabled = false;
				dtpUntilTime.Enabled = false;
				nudHours.Enabled = false;
				nudMinutes.Enabled = false;
				cboScheduleByType.SelectedIndex = 0;
				HideDetails();

				LoadScheduleValues();

				FormLoaded = true;
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void frmScheduleProperties_Activated(object sender, System.EventArgs e)
		{
			try
			{
				txtScheduleName.Focus();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Save())
				{
					this.Close();
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnDirectory_Click(object sender, System.EventArgs e)
		{
			try
			{
				fbdConditionDirectory.SelectedPath = txtConditionDirectory.Text.Trim();
				fbdConditionDirectory.Description = "Select the directory where the Condition file will be found.";

				if (fbdConditionDirectory.ShowDialog() == DialogResult.OK)
				{
					txtConditionDirectory.Text = fbdConditionDirectory.SelectedPath;

					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void chkRangeEndDate_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkRangeEndDate.Checked)
				{
					dtpRangeEndDate.Enabled = true;
				}
				else
				{
					dtpRangeEndDate.Enabled = false;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		private void chkUntil_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkUntil.Checked)
				{
					dtpUntilTime.Enabled = true;
					chkDuration.Enabled = false;
				}
				else
				{
					dtpUntilTime.Enabled = false;
					chkDuration.Enabled = true;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void chkDuration_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkDuration.Checked)
				{
					nudHours.Enabled = true;
					nudMinutes.Enabled = true;
					chkUntil.Enabled = false;
				}
				else
				{
					nudHours.Enabled = false;
					nudMinutes.Enabled = false;
					chkUntil.Enabled = true;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}
		
		private void chkRepeatUntilSuccessful_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkRepeatUntilSuccessful.Checked)
				{
					chkTerminateAfterConditionMet.Enabled = false;
				}
				else
				{
					chkTerminateAfterConditionMet.Enabled = true;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void chkTerminateAfterConditionMet_CheckedChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (chkTerminateAfterConditionMet.Checked)
				{
					chkRepeatUntilSuccessful.Enabled = false;
				}
				else
				{
					chkRepeatUntilSuccessful.Enabled = true;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cboScheduleByType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			eScheduleByType newValue;

			try
			{
				newValue = (eScheduleByType)((ComboObject)cboScheduleByType.SelectedItem).Key;

				switch (newValue)
				{
					case eScheduleByType.Once:

						grpByDay.Visible = false;
						grpByWeek.Visible = false;
						grpByMonth.Visible = false;
						chkRangeEndDate.Enabled = false;
						break;

					case eScheduleByType.Day:

						grpByDay.Visible = true;
						grpByWeek.Visible = false;
						grpByMonth.Visible = false;
						chkRangeEndDate.Enabled = true;
						break;

					case eScheduleByType.Week:

						grpByDay.Visible = false;
						grpByWeek.Visible = true;
						grpByMonth.Visible = false;
						chkRangeEndDate.Enabled = true;
						break;

					case eScheduleByType.Month:

						grpByDay.Visible = false;
						grpByWeek.Visible = false;
						grpByMonth.Visible = true;
						chkRangeEndDate.Enabled = true;
						break;
				}

				if (FormLoaded)
				{
					ChangePending = true;
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cboConditionType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			eScheduleConditionType newValue;

			try
			{
				if (cboConditionType.SelectedIndex != -1)
				{
					newValue = (eScheduleConditionType)((ComboObject)cboConditionType.SelectedItem).Key;

					switch (newValue)
					{
						case eScheduleConditionType.None :

							grpFileCondition.Visible = false;
							cboConditionType.SelectedIndex = -1;
							break;
																	 
						case eScheduleConditionType.ByFileExtension :

							grpFileCondition.Visible = true;
							break;
					}

					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void cboRepeatType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			eScheduleRepeatIntervalType newValue;

			try
			{
				if (cboRepeatType.SelectedIndex != -1)
				{
					newValue = (eScheduleRepeatIntervalType)((ComboObject)cboRepeatType.SelectedItem).Key;

					if (newValue == eScheduleRepeatIntervalType.None)
					{
						cboRepeatType.SelectedIndex = -1;
					}

					if (FormLoaded)
					{
						ChangePending = true;
					}
				}
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

        // Begin TT#316-MD - RMatelic - Replace all Windows Combobox controls with new enhanced control 
        void cboRepeatType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboRepeatType_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboScheduleByMonthWeekType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.generic_ValueChanged(this, new EventArgs());
        }

        void cboConditionType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboConditionType_SelectionChangeCommitted(source, new EventArgs());
        }

        void cboScheduleByType_MIDComboBoxPropertiesChangedEvent(object source, MIDComboBoxPropertiesChangedEventArgs args)
        {
            this.cboScheduleByType_SelectionChangeCommitted(source, new EventArgs());
        }
        // End TT#316-MD

		private void btnHide_Click(object sender, System.EventArgs e)
		{
			try
			{
				HideDetails();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void btnDetails_Click(object sender, System.EventArgs e)
		{
			try
			{
				ShowDetails();
			}
			catch (Exception exc)
			{
				HandleExceptions(exc);
			}
		}

		private void generic_ValueChanged(object sender, System.EventArgs e)
		{
			if (FormLoaded)
			{
				ChangePending = true;
			}
		}

		override protected bool SaveChanges()
		{
			try
			{
				if (Save())
				{
					ErrorFound = false;
				}
				else
				{
					ErrorFound = true;
				}

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadScheduleValues()
		{
			try
			{
				txtScheduleName.Text = _scheduleProf.Name;
				dtpStart.Value = _scheduleProf.StartTime;
				cboScheduleByType.SelectedItem = new ComboObject((int)_scheduleProf.ScheduleByType, MIDText.GetTextOnly((int)_scheduleProf.ScheduleByType));

				switch (_scheduleProf.ScheduleByType)
				{
					case eScheduleByType.Day :
						nudDays.Value = (decimal)_scheduleProf.ScheduleByInterval;
						break;
					case eScheduleByType.Week :
						nudWeeks.Value = (decimal)_scheduleProf.ScheduleByInterval;
						chkWeekSunday.Checked = _scheduleProf.ScheduleByDaysInWeek.Sunday;
						chkWeekMonday.Checked = _scheduleProf.ScheduleByDaysInWeek.Monday;
						chkWeekTuesday.Checked = _scheduleProf.ScheduleByDaysInWeek.Tuesday;
						chkWeekWednesday.Checked = _scheduleProf.ScheduleByDaysInWeek.Wednesday;
						chkWeekThursday.Checked = _scheduleProf.ScheduleByDaysInWeek.Thursday;
						chkWeekFriday.Checked = _scheduleProf.ScheduleByDaysInWeek.Friday;
						chkWeekSaturday.Checked = _scheduleProf.ScheduleByDaysInWeek.Saturday;
						break;
					case eScheduleByType.Month :
						nudMonths.Value = (decimal)_scheduleProf.ScheduleByInterval;
						chkMonthSunday.Checked = _scheduleProf.ScheduleByDaysInWeek.Sunday;
						chkMonthMonday.Checked = _scheduleProf.ScheduleByDaysInWeek.Monday;
						chkMonthTuesday.Checked = _scheduleProf.ScheduleByDaysInWeek.Tuesday;
						chkMonthWednesday.Checked = _scheduleProf.ScheduleByDaysInWeek.Wednesday;
						chkMonthThursday.Checked = _scheduleProf.ScheduleByDaysInWeek.Thursday;
						chkMonthFriday.Checked = _scheduleProf.ScheduleByDaysInWeek.Friday;
						chkMonthSaturday.Checked = _scheduleProf.ScheduleByDaysInWeek.Saturday;
						cboScheduleByMonthWeekType.SelectedItem = new ComboObject((int)_scheduleProf.ScheduleByMonthWeekType, MIDText.GetTextOnly((int)_scheduleProf.ScheduleByMonthWeekType));
						break;
				}

				dtpRangeStartDate.Value = _scheduleProf.StartDateRange;

				chkRangeEndDate.Checked = _scheduleProf.EndDate;

				dtpRangeEndDate.Value = _scheduleProf.EndDateRange;

				nudRepeat.Value = (decimal)_scheduleProf.RepeatInterval;
				cboRepeatType.SelectedItem = new ComboObject((int)_scheduleProf.RepeatIntervalType, MIDText.GetTextOnly((int)_scheduleProf.RepeatIntervalType));
				chkUntil.Checked = _scheduleProf.RepeatUntil;
				dtpUntilTime.Value = _scheduleProf.RepeatUntilTime;
				chkDuration.Checked = _scheduleProf.RepeatDuration;
				nudHours.Value = (decimal)_scheduleProf.RepeatDurationHours;
				nudMinutes.Value = (decimal)_scheduleProf.RepeatDurationMinutes;
				chkTerminateAfterConditionMet.Checked = _scheduleProf.TerminateAfterConditionMet;
				chkRepeatUntilSuccessful.Checked = _scheduleProf.RepeatUntilSuccessful;

				cboConditionType.SelectedItem = new ComboObject((int)_scheduleProf.ConditionType, MIDText.GetTextOnly((int)_scheduleProf.ConditionType));
				txtConditionDirectory.Text = _scheduleProf.ConditionTriggerDirectory;
				txtConditionMask.Text = _scheduleProf.ConditionTriggerSuffix;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool Save()
		{
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				if (CheckValues())
				{
					return SaveScheduleValues();
				}
				else
				{
					return false;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

		private bool CheckValues()
		{
// BEGIN MID Track #2423 - Restart run cycle after schedule is updated
			bool isJobInCycle;
// END MID Track #2423 - Restart run cycle after schedule is updated
// BEGIN MID Track #2396 - Add confirmation for Schedule Properties
			DateTime nextRunDateTime;

// END MID Track #2396 - Add confirmation for Schedule Properties
			try
			{
				if (txtScheduleName.Text.Trim().Length == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleNameRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (_scheduleProf.Key == -1 || _scheduleProf.Name != txtScheduleName.Text)
				{
					if (_dlSchedule.Schedule_GetKey(txtScheduleName.Text.Trim()) != -1)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ScheduleNameExists), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}

				if (nudRepeat.Value > 0 && cboRepeatType.SelectedIndex == -1)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RepeatTypeNotDefined), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				switch ((eScheduleByType)((ComboObject)cboScheduleByType.SelectedItem).Key)
				{
					case eScheduleByType.Day :

						if (nudDays.Value == 0)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_DaysMustBeGreaterThanZero), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}
						break;

					case eScheduleByType.Week :

						if (nudWeeks.Value == 0)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_WeeksMustBeGreaterThanZero), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}

						if (chkWeekSunday.Checked == false && chkWeekMonday.Checked == false && chkWeekTuesday.Checked == false && chkWeekWednesday.Checked == false &&
							chkWeekThursday.Checked == false && chkWeekFriday.Checked == false && chkWeekSaturday.Checked == false)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneDayOfWeekRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}
						break;

					case eScheduleByType.Month :

						if (nudMonths.Value == 0)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MonthsMustBeGreaterThanZero), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}

						if (cboScheduleByMonthWeekType.SelectedIndex == -1)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_MonthWeekTypeRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}

						if (chkMonthSunday.Checked == false && chkMonthMonday.Checked == false && chkMonthTuesday.Checked == false && chkMonthWednesday.Checked == false &&
							chkMonthThursday.Checked == false && chkMonthFriday.Checked == false && chkMonthSaturday.Checked == false)
						{
							MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_AtLeastOneDayOfWeekRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return false;
						}
						break;
				}

				if (chkDuration.Checked && nudHours.Value == 0 && nudMinutes.Value == 0)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_HoursOrMinutesRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				if (cboConditionType.SelectedIndex != -1)
				{
					if (txtConditionDirectory.Text.Trim().Length == 0)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConditionDirectoryRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else if (txtConditionDirectory.Text.Trim().Length > 250)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConditionDirectoryTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}

					if (txtConditionMask.Text.Trim().Length == 0)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConditionSuffixRequired), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else if (txtConditionMask.Text.Trim().Length > 50)
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConditionSuffixTooLong), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
					else if (!CheckMaskForValidCharacters(txtConditionMask.Text))
					{
						MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_InvalidCharactersInSuffix), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
						return false;
					}
				}

				if (chkTerminateAfterConditionMet.Checked && cboConditionType.SelectedIndex == -1)
				{
					MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ConditionMustBeSpecified), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

// BEGIN MID Track #2423 - Restart run cycle after schedule is updated
//// BEGIN MID Track #2396 - Add confirmation for Schedule Properties
//				LoadScheduleProfile();
//
//				nextRunDateTime = _SAB.SchedulerServerSession.GetNextRunDate(_scheduleProf, _jobProf.Key);
//
//				if (nextRunDateTime != DateTime.MinValue && nextRunDateTime <= DateTime.Now)
//				{
//					if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunNowConfirmation), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
//					{
//						return false;
//					}
//				}
//
//// END MID Track #2396 - Add confirmation for Schedule Properties
				LoadScheduleProfile();

				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//nextRunDateTime = _SAB.SchedulerServerSession.GetNextRunDate(_scheduleProf, _jobProf.Key);
				nextRunDateTime = _SAB.SchedulerServerSession.GetNextRunDate(_scheduleProf, _jobProf.Key).ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
				isJobInCycle = _SAB.SchedulerServerSession.isJobInCycle(_scheduleProf, _jobProf.Key);

				if (nextRunDateTime != DateTime.MinValue && nextRunDateTime <= DateTime.Now)
				{
					if (isJobInCycle)
					{
						if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TerminateJobCycleAndRunNow), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return false;
						}
					}
					else
					{
						if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_RunNowConfirmation), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return false;
						}
					}
				}
				else
				{
					if (isJobInCycle)
					{
						if (MessageBox.Show(_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_TerminateJobCycle), Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return false;
						}
					}
				}

// END MID Track #2423 - Restart run cycle after schedule is updated
				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private bool CheckMaskForValidCharacters(string aMask)
		{
			char[] invalidChars = { '*' };

			try
			{
				if (aMask.IndexOfAny(invalidChars) >= 0)
				{
					return false;
				}
				else
				{
					return true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// BEGIN MID Track #2396 - Add confirmation for Schedule Properties
//		private void ShowDetails()
//		{
//			try
//			{
//				grpDateRange.Visible = true;
//				grpRepeat.Visible = true;
//				grpCondition.Visible = true;
//				this.Height = cShowHeight;
//				this.Width = cShowWidth;
//				btnHide.BringToFront();
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
//		private void HideDetails()
//		{
//			try
//			{
//				grpDateRange.Visible = false;
//				grpRepeat.Visible = false;
//				grpCondition.Visible = false;
//				this.Height = cHideHeight;
//				this.Width = cHideWidth;
//				btnDetails.BringToFront();
//			}
//			catch (Exception exc)
//			{
//				throw;
//			}
//		}
//
		private void LoadScheduleProfile()
		{
			//Begin Track #398 - JScott - Scheduler does not recognize different time zones
			TimeSpan untilTimeSpan;

			//End Track #398 - JScott - Scheduler does not recognize different time zones
			try
			{
				// Pull info from screen into ScheduleProfile object

				_scheduleProf.Name = txtScheduleName.Text.Trim();
				_scheduleProf.StartTime = dtpStart.Value;
				_scheduleProf.ScheduleByType = (eScheduleByType)((ComboObject)cboScheduleByType.SelectedItem).Key;

				switch (_scheduleProf.ScheduleByType)
				{
					case eScheduleByType.Day :
						_scheduleProf.ScheduleByInterval = (int)nudDays.Value;
						break;
					case eScheduleByType.Week :
						_scheduleProf.ScheduleByInterval = (int)nudWeeks.Value;
						_scheduleProf.ScheduleByDaysInWeek = new DaysInWeek(
							chkWeekSunday.Checked,
							chkWeekMonday.Checked,
							chkWeekTuesday.Checked,
							chkWeekWednesday.Checked,
							chkWeekThursday.Checked,
							chkWeekFriday.Checked,
							chkWeekSaturday.Checked);
						break;
					case eScheduleByType.Month :
						_scheduleProf.ScheduleByInterval = (int)nudMonths.Value;
						_scheduleProf.ScheduleByDaysInWeek = new DaysInWeek(
							chkMonthSunday.Checked,
							chkMonthMonday.Checked,
							chkMonthTuesday.Checked,
							chkMonthWednesday.Checked,
							chkMonthThursday.Checked,
							chkMonthFriday.Checked,
							chkMonthSaturday.Checked);

						_scheduleProf.ScheduleByMonthWeekType = (eScheduleByMonthWeekType)((ComboObject)cboScheduleByMonthWeekType.SelectedItem).Key;
						break;
				}

				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_scheduleProf.StartDateRange =
				//    new DateTime(dtpRangeStartDate.Value.Year,
				//    dtpRangeStartDate.Value.Month,
				//    dtpRangeStartDate.Value.Day);
				_scheduleProf.StartDateRange =
					new DateTime(dtpRangeStartDate.Value.Year,
					dtpRangeStartDate.Value.Month,
					dtpRangeStartDate.Value.Day,
					dtpStart.Value.Hour,
					dtpStart.Value.Minute,
					dtpStart.Value.Second);
				//End Track #398 - JScott - Scheduler does not recognize different time zones

				_scheduleProf.EndDate = chkRangeEndDate.Checked;

				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_scheduleProf.EndDateRange =
				//    new DateTime(dtpRangeEndDate.Value.Year,
				//    dtpRangeEndDate.Value.Month,
				//    dtpRangeEndDate.Value.Day);
				_scheduleProf.EndDateRange =
					new DateTime(dtpRangeEndDate.Value.Year,
					dtpRangeEndDate.Value.Month,
					dtpRangeEndDate.Value.Day,
					dtpStart.Value.Hour,
					dtpStart.Value.Minute,
					dtpStart.Value.Second);
				//End Track #398 - JScott - Scheduler does not recognize different time zones

				_scheduleProf.RepeatInterval = (int)nudRepeat.Value;

				if (cboRepeatType.SelectedItem != null)
				{
					_scheduleProf.RepeatIntervalType = (eScheduleRepeatIntervalType)((ComboObject)cboRepeatType.SelectedItem).Key;
				}
				else
				{
					_scheduleProf.RepeatIntervalType = eScheduleRepeatIntervalType.None;
				}

				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_scheduleProf.RepeatUntil = chkUntil.Checked;
				//_scheduleProf.RepeatUntilTime = dtpUntilTime.Value;
				//_scheduleProf.RepeatDuration = chkDuration.Checked;
				//_scheduleProf.RepeatDurationHours = (int)nudHours.Value;
				//_scheduleProf.RepeatDurationMinutes = (int)nudMinutes.Value;
				_scheduleProf.RepeatUntil = chkUntil.Checked;
				_scheduleProf.RepeatDuration = chkDuration.Checked;

				if (_scheduleProf.RepeatUntil)
				{
					_scheduleProf.RepeatUntilTime = dtpUntilTime.Value;

					if (_scheduleProf.RepeatUntilTime < _scheduleProf.StartTime)
					{
						_scheduleProf.RepeatUntilTime = _scheduleProf.RepeatUntilTime.AddDays(1);
					}

					untilTimeSpan = _scheduleProf.RepeatUntilTime.Subtract(_scheduleProf.StartTime);
					_scheduleProf.RepeatDurationHours = untilTimeSpan.Hours;
					_scheduleProf.RepeatDurationMinutes = untilTimeSpan.Minutes;
				}
				else
				{
					_scheduleProf.RepeatDurationHours = (int)nudHours.Value;
					_scheduleProf.RepeatDurationMinutes = (int)nudMinutes.Value;
				}
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			
				if (cboConditionType.SelectedItem != null)
				{
					_scheduleProf.ConditionType = (eScheduleConditionType)((ComboObject)cboConditionType.SelectedItem).Key;
				}
				else
				{
					_scheduleProf.ConditionType = eScheduleConditionType.None;
				}

				_scheduleProf.ConditionTriggerDirectory = txtConditionDirectory.Text.Trim();
				_scheduleProf.ConditionTriggerSuffix = txtConditionMask.Text.Trim();
				_scheduleProf.TerminateAfterConditionMet = chkTerminateAfterConditionMet.Checked;
				_scheduleProf.RepeatUntilSuccessful = chkRepeatUntilSuccessful.Checked;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
// END MID Track #2396 - Add confirmation for Schedule Properties

		private bool SaveScheduleValues()
		{
			try
			{
// BEGIN MID Track #2396 - Add confirmation for Schedule Properties
//				// Pull info from screen into ScheduleProfile object
//
//				_scheduleProf.Name = txtScheduleName.Text.Trim();
//				_scheduleProf.StartTime = dtpStart.Value;
//				_scheduleProf.ScheduleByType = (eScheduleByType)((ComboObject)cboScheduleByType.SelectedItem).Key;
//
//				switch (_scheduleProf.ScheduleByType)
//				{
//					case eScheduleByType.Day :
//						_scheduleProf.ScheduleByInterval = (int)nudDays.Value;
//						break;
//					case eScheduleByType.Week :
//						_scheduleProf.ScheduleByInterval = (int)nudWeeks.Value;
//						_scheduleProf.ScheduleByDaysInWeek = new DaysInWeek(
//							chkWeekSunday.Checked,
//							chkWeekMonday.Checked,
//							chkWeekTuesday.Checked,
//							chkWeekWednesday.Checked,
//							chkWeekThursday.Checked,
//							chkWeekFriday.Checked,
//							chkWeekSaturday.Checked);
//						break;
//					case eScheduleByType.Month :
//						_scheduleProf.ScheduleByInterval = (int)nudMonths.Value;
//						_scheduleProf.ScheduleByDaysInWeek = new DaysInWeek(
//							chkMonthSunday.Checked,
//							chkMonthMonday.Checked,
//							chkMonthTuesday.Checked,
//							chkMonthWednesday.Checked,
//							chkMonthThursday.Checked,
//							chkMonthFriday.Checked,
//							chkMonthSaturday.Checked);
//
//						_scheduleProf.ScheduleByMonthWeekType = (eScheduleByMonthWeekType)((ComboObject)cboScheduleByMonthWeekType.SelectedItem).Key;
//						break;
//				}
//
//				_scheduleProf.StartDateRange =
//					new DateTime(dtpRangeStartDate.Value.Year,
//					dtpRangeStartDate.Value.Month,
//					dtpRangeStartDate.Value.Day);
//
//				_scheduleProf.EndDate = chkRangeEndDate.Checked;
//
//				_scheduleProf.EndDateRange =
//					new DateTime(dtpRangeEndDate.Value.Year,
//					dtpRangeEndDate.Value.Month,
//					dtpRangeEndDate.Value.Day);
//
//				_scheduleProf.RepeatInterval = (int)nudRepeat.Value;
//
//				if (cboRepeatType.SelectedItem != null)
//				{
//					_scheduleProf.RepeatIntervalType = (eScheduleRepeatIntervalType)((ComboObject)cboRepeatType.SelectedItem).Key;
//				}
//				else
//				{
//					_scheduleProf.RepeatIntervalType = eScheduleRepeatIntervalType.None;
//				}
//
//				_scheduleProf.RepeatUntil = chkUntil.Checked;
//				_scheduleProf.RepeatUntilTime = dtpUntilTime.Value;
//				_scheduleProf.RepeatDuration = chkDuration.Checked;
//				_scheduleProf.RepeatDurationHours = (int)nudHours.Value;
//				_scheduleProf.RepeatDurationMinutes = (int)nudMinutes.Value;
//			
//				if (cboConditionType.SelectedItem != null)
//				{
//					_scheduleProf.ConditionType = (eScheduleConditionType)((ComboObject)cboConditionType.SelectedItem).Key;
//				}
//				else
//				{
//					_scheduleProf.ConditionType = eScheduleConditionType.None;
//				}
//
//				_scheduleProf.ConditionTriggerDirectory = txtConditionDirectory.Text.Trim();
//				_scheduleProf.ConditionTriggerSuffix = txtConditionMask.Text.Trim();
//				_scheduleProf.TerminateAfterConditionMet = chkTerminateAfterConditionMet.Checked;
//				_scheduleProf.RepeatUntilSuccessful = chkRepeatUntilSuccessful.Checked;
//
// END MID Track #2396 - Add confirmation for Schedule Properties
				// Call Scheduler Session to update DB

				switch (_updateStat)
				{
					case eScheduleUpdateStatus.CreateSchedule :

// Begin Alert Events Code -- DO NOT REMOVE
						_SAB.SchedulerServerSession.ScheduleExistingJob(_scheduleProf, _jobProf.Key, _SAB.ClientServerSession.UserRID);
//						_SAB.SchedulerServerSession.ScheduleExistingJob(_scheduleProf, _jobProf.Key, _SAB.ClientServerSession.UserRID, new JobFinishAlertEvent(new AlertEventHandler(_SAB.MessageCallback.HandleAlert)));
// End Alert Events Code -- DO NOT REMOVE
						_updateStat = eScheduleUpdateStatus.UpdateSchedule;
						break;

					case eScheduleUpdateStatus.CreateScheduleAndJob :

// Begin Alert Events Code -- DO NOT REMOVE
						_SAB.SchedulerServerSession.ScheduleNewJob(_scheduleProf, _jobProf, _taskListProf.Key, _SAB.ClientServerSession.UserRID);
//						_SAB.SchedulerServerSession.ScheduleNewJob(_scheduleProf, _jobProf, _taskListProf.Key, _SAB.ClientServerSession.UserRID, new JobFinishAlertEvent(new AlertEventHandler(_SAB.MessageCallback.HandleAlert)));
// End Alert Events Code -- DO NOT REMOVE
						_updateStat = eScheduleUpdateStatus.UpdateSchedule;
						break;

					case eScheduleUpdateStatus.UpdateSchedule :

						_SAB.SchedulerServerSession.UpdateSchedule(_scheduleProf, _jobProf.Key, _SAB.ClientServerSession.UserRID);
						break;
				}

				ChangePending = false;

				if (OnSchedulePropertiesSaveHandler != null)
				{
					OnSchedulePropertiesSaveHandler(this, new SchedulePropertiesSaveEventArgs());
				}

				RefreshScheduleBrowserWindow();

				return true;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// BEGIN MID Track #2396 - Add confirmation for Schedule Properties
		private void ShowDetails()
		{
			try
			{
				grpDateRange.Visible = true;
				grpRepeat.Visible = true;
				grpCondition.Visible = true;
				this.Height = cShowHeight;
				this.Width = cShowWidth;
				btnHide.BringToFront();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void HideDetails()
		{
			try
			{
				grpDateRange.Visible = false;
				grpRepeat.Visible = false;
				grpCondition.Visible = false;
				this.Height = cHideHeight;
				this.Width = cHideWidth;
				btnDetails.BringToFront();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// END MID Track #2396 - Add confirmation for Schedule Properties
		private void RefreshScheduleBrowserWindow()
		{
			frmScheduleBrowser schedBrowser;

			try
			{
				foreach (Form childForm in ParentForm.MdiChildren)
				{
					if (childForm.GetType() == typeof(frmScheduleBrowser))
					{
						schedBrowser = (frmScheduleBrowser)childForm;
						schedBrowser.Refresh();
					}
				}
			}
			catch (Exception error)
			{
				HandleException(error);
			}
		}
	}
	
	public class SchedulePropertiesSaveEventArgs : EventArgs
	{
	}
}
