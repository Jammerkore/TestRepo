namespace MIDRetail.Windows.Controls
{
    partial class ForecastAnalysisDateOptionsControl
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem11 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem12 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.dteToTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.dteToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.osAuditDates = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel7 = new Infragistics.Win.Misc.UltraLabel();
            this.dteForecastToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteForecastFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.osForecastDates = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel8 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osAuditDates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteForecastToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteForecastFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osForecastDates)).BeginInit();
            this.SuspendLayout();
            // 
            // dteToTime
            // 
            this.dteToTime.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteToTime.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteToTime.DateTime = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteToTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dteToTime.Enabled = false;
            this.dteToTime.Location = new System.Drawing.Point(165, 96);
            this.dteToTime.MaskInput = "{time}";
            this.dteToTime.Name = "dteToTime";
            this.dteToTime.Size = new System.Drawing.Size(96, 21);
            this.dteToTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dteToTime.TabIndex = 27;
            this.dteToTime.Value = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(26, 101);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(21, 14);
            this.ultraLabel3.TabIndex = 26;
            this.ultraLabel3.Text = "To:";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(26, 77);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(34, 14);
            this.ultraLabel4.TabIndex = 25;
            this.ultraLabel4.Text = "From:";
            // 
            // dteToDate
            // 
            this.dteToDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteToDate.Enabled = false;
            this.dteToDate.Location = new System.Drawing.Point(63, 97);
            this.dteToDate.Name = "dteToDate";
            this.dteToDate.Size = new System.Drawing.Size(96, 21);
            this.dteToDate.TabIndex = 24;
            this.dteToDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            // 
            // dteFromTime
            // 
            this.dteFromTime.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteFromTime.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteFromTime.DateTime = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteFromTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dteFromTime.Enabled = false;
            this.dteFromTime.Location = new System.Drawing.Point(165, 73);
            this.dteFromTime.MaskInput = "{time}";
            this.dteFromTime.Name = "dteFromTime";
            this.dteFromTime.Size = new System.Drawing.Size(96, 21);
            this.dteFromTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dteFromTime.TabIndex = 23;
            this.dteFromTime.Value = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            // 
            // dteFromDate
            // 
            this.dteFromDate.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteFromDate.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteFromDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteFromDate.Enabled = false;
            this.dteFromDate.Location = new System.Drawing.Point(63, 73);
            this.dteFromDate.MaskInput = "{date}";
            this.dteFromDate.Name = "dteFromDate";
            this.dteFromDate.Size = new System.Drawing.Size(96, 21);
            this.dteFromDate.TabIndex = 22;
            this.dteFromDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Location = new System.Drawing.Point(4, 0);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel5.TabIndex = 28;
            this.ultraLabel5.Text = "Audit Dates:";
            // 
            // osAuditDates
            // 
            this.osAuditDates.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osAuditDates.CheckedIndex = 0;
            valueListItem3.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem3.DataValue = "Unrestricted";
            valueListItem3.DisplayText = "Unrestricted";
            valueListItem4.DataValue = "Last_24_Hours";
            valueListItem4.DisplayText = "Last 24 Hours";
            valueListItem11.DataValue = "Last_7_Days";
            valueListItem11.DisplayText = "Last 7 days";
            valueListItem12.DataValue = "DateRange";
            valueListItem12.DisplayText = "Specify a date range:";
            this.osAuditDates.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4,
            valueListItem11,
            valueListItem12});
            this.osAuditDates.Location = new System.Drawing.Point(13, 15);
            this.osAuditDates.Name = "osAuditDates";
            this.osAuditDates.Size = new System.Drawing.Size(128, 59);
            this.osAuditDates.TabIndex = 33;
            this.osAuditDates.Text = "Unrestricted";
            this.osAuditDates.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osAuditDates.ValueChanged += new System.EventHandler(this.osAuditDates_ValueChanged);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(26, 221);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(21, 14);
            this.ultraLabel1.TabIndex = 38;
            this.ultraLabel1.Text = "To:";
            // 
            // ultraLabel7
            // 
            this.ultraLabel7.AutoSize = true;
            this.ultraLabel7.Location = new System.Drawing.Point(26, 197);
            this.ultraLabel7.Name = "ultraLabel7";
            this.ultraLabel7.Size = new System.Drawing.Size(34, 14);
            this.ultraLabel7.TabIndex = 37;
            this.ultraLabel7.Text = "From:";
            // 
            // dteForecastToDate
            // 
            this.dteForecastToDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteForecastToDate.Enabled = false;
            this.dteForecastToDate.Location = new System.Drawing.Point(63, 217);
            this.dteForecastToDate.Name = "dteForecastToDate";
            this.dteForecastToDate.Size = new System.Drawing.Size(96, 21);
            this.dteForecastToDate.TabIndex = 36;
            this.dteForecastToDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            // 
            // dteForecastFromDate
            // 
            this.dteForecastFromDate.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteForecastFromDate.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteForecastFromDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteForecastFromDate.Enabled = false;
            this.dteForecastFromDate.Location = new System.Drawing.Point(63, 193);
            this.dteForecastFromDate.MaskInput = "{date}";
            this.dteForecastFromDate.Name = "dteForecastFromDate";
            this.dteForecastFromDate.Size = new System.Drawing.Size(96, 21);
            this.dteForecastFromDate.TabIndex = 34;
            this.dteForecastFromDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            // 
            // osForecastDates
            // 
            this.osForecastDates.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osForecastDates.CheckedIndex = 0;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "Unrestricted";
            valueListItem1.DisplayText = "Unrestricted";
            valueListItem9.DataValue = "Last_24_Hours";
            valueListItem9.DisplayText = "Last 24 Hours";
            valueListItem10.DataValue = "Last_7_Days";
            valueListItem10.DisplayText = "Last 7 days";
            valueListItem2.DataValue = "DateRange";
            valueListItem2.DisplayText = "Specify a date range:";
            this.osForecastDates.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem9,
            valueListItem10,
            valueListItem2});
            this.osForecastDates.Location = new System.Drawing.Point(13, 135);
            this.osForecastDates.Name = "osForecastDates";
            this.osForecastDates.Size = new System.Drawing.Size(128, 59);
            this.osForecastDates.TabIndex = 41;
            this.osForecastDates.Text = "Unrestricted";
            this.osForecastDates.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osForecastDates.ValueChanged += new System.EventHandler(this.osForecastDates_ValueChanged);
            // 
            // ultraLabel8
            // 
            this.ultraLabel8.Location = new System.Drawing.Point(4, 120);
            this.ultraLabel8.Name = "ultraLabel8";
            this.ultraLabel8.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel8.TabIndex = 40;
            this.ultraLabel8.Text = "Forecast Dates:";
            // 
            // ForecastAnalysisDateOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.ultraLabel7);
            this.Controls.Add(this.dteForecastToDate);
            this.Controls.Add(this.dteForecastFromDate);
            this.Controls.Add(this.osForecastDates);
            this.Controls.Add(this.ultraLabel8);
            this.Controls.Add(this.dteToTime);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.dteToDate);
            this.Controls.Add(this.dteFromTime);
            this.Controls.Add(this.dteFromDate);
            this.Controls.Add(this.osAuditDates);
            this.Controls.Add(this.ultraLabel5);
            this.Name = "ForecastAnalysisDateOptionsControl";
            this.Size = new System.Drawing.Size(280, 245);
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osAuditDates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteForecastToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteForecastFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osForecastDates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToTime;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromTime;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromDate;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osAuditDates;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel7;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteForecastToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteForecastFromDate;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osForecastDates;
        private Infragistics.Win.Misc.UltraLabel ultraLabel8;
    }
}
