namespace MIDRetail.Windows.Controls
{
    partial class AllocationAnalysisOptionsControl
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
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem3 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem4 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem9 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem10 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.txtResultLimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.osResultLimit = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.dteToTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.dteToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.osMerchandise = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.osHeaders = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.osAuditDates = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.selectSingleHierarchyNodeControl1 = new MIDRetail.Windows.Controls.SelectSingleHierarchyNodeControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osResultLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osMerchandise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osHeaders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osAuditDates)).BeginInit();
            this.SuspendLayout();
            // 
            // txtResultLimit
            // 
            this.txtResultLimit.Enabled = false;
            this.txtResultLimit.Location = new System.Drawing.Point(28, 289);
            this.txtResultLimit.Name = "txtResultLimit";
            this.txtResultLimit.Size = new System.Drawing.Size(76, 21);
            this.txtResultLimit.TabIndex = 31;
            this.txtResultLimit.Text = "5000";
            // 
            // osResultLimit
            // 
            this.osResultLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osResultLimit.CheckedIndex = 0;
            valueListItem7.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem7.DataValue = "Unrestricted";
            valueListItem7.DisplayText = "Unrestricted";
            valueListItem8.DataValue = "Limit";
            valueListItem8.DisplayText = "Limit the maximum results:";
            this.osResultLimit.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem7,
            valueListItem8});
            this.osResultLimit.Location = new System.Drawing.Point(13, 261);
            this.osResultLimit.Name = "osResultLimit";
            this.osResultLimit.Size = new System.Drawing.Size(274, 35);
            this.osResultLimit.TabIndex = 30;
            this.osResultLimit.Text = "Unrestricted";
            this.osResultLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osResultLimit.ValueChanged += new System.EventHandler(this.osResultLimit_ValueChanged);
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.Location = new System.Drawing.Point(4, 246);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel6.TabIndex = 29;
            this.ultraLabel6.Text = "Result Limit:";
            // 
            // dteToTime
            // 
            this.dteToTime.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteToTime.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteToTime.DateTime = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteToTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dteToTime.Enabled = false;
            this.dteToTime.Location = new System.Drawing.Point(165, 219);
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
            this.ultraLabel3.Location = new System.Drawing.Point(26, 224);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(21, 14);
            this.ultraLabel3.TabIndex = 26;
            this.ultraLabel3.Text = "To:";
            // 
            // ultraLabel4
            // 
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(26, 200);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(34, 14);
            this.ultraLabel4.TabIndex = 25;
            this.ultraLabel4.Text = "From:";
            // 
            // dteToDate
            // 
            this.dteToDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteToDate.Enabled = false;
            this.dteToDate.Location = new System.Drawing.Point(63, 220);
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
            this.dteFromTime.Location = new System.Drawing.Point(165, 196);
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
            this.dteFromDate.Location = new System.Drawing.Point(63, 196);
            this.dteFromDate.MaskInput = "{date}";
            this.dteFromDate.Name = "dteFromDate";
            this.dteFromDate.Size = new System.Drawing.Size(96, 21);
            this.dteFromDate.TabIndex = 22;
            this.dteFromDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            // 
            // osMerchandise
            // 
            this.osMerchandise.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osMerchandise.CheckedIndex = 0;
            valueListItem5.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem5.DataValue = "Unrestricted";
            valueListItem5.DisplayText = "Unrestricted";
            valueListItem6.DataValue = "Restrict";
            valueListItem6.DisplayText = "Restrict to following style node:";
            this.osMerchandise.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6});
            this.osMerchandise.Location = new System.Drawing.Point(13, 69);
            this.osMerchandise.Name = "osMerchandise";
            this.osMerchandise.Size = new System.Drawing.Size(274, 35);
            this.osMerchandise.TabIndex = 19;
            this.osMerchandise.Text = "Unrestricted";
            this.osMerchandise.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osMerchandise.ValueChanged += new System.EventHandler(this.osMerchandise_ValueChanged);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.Location = new System.Drawing.Point(4, 55);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 20;
            this.ultraLabel2.Text = "Merchandise Style:";
            // 
            // osHeaders
            // 
            this.osHeaders.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osHeaders.CheckedIndex = 0;
            valueListItem3.DataValue = "Unrestricted";
            valueListItem3.DisplayText = "Unrestricted";
            valueListItem4.DataValue = "Selected";
            valueListItem4.DisplayText = "Use selected headers from Allocation Workspace";
            this.osHeaders.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem3,
            valueListItem4});
            this.osHeaders.Location = new System.Drawing.Point(13, 19);
            this.osHeaders.Name = "osHeaders";
            this.osHeaders.Size = new System.Drawing.Size(274, 35);
            this.osHeaders.TabIndex = 17;
            this.osHeaders.Text = "Unrestricted";
            this.osHeaders.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Location = new System.Drawing.Point(4, 5);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel1.TabIndex = 18;
            this.ultraLabel1.Text = "Headers:";
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.Location = new System.Drawing.Point(4, 123);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel5.TabIndex = 28;
            this.ultraLabel5.Text = "Audit Dates:";
            // 
            // osAuditDates
            // 
            this.osAuditDates.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osAuditDates.CheckedIndex = 0;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "Unrestricted";
            valueListItem1.DisplayText = "Unrestricted";
            valueListItem9.DataValue = "Last_24_Hours";
            valueListItem9.DisplayText = "Last 24 Hours";
            valueListItem10.DataValue = "Last_7_Days";
            valueListItem10.DisplayText = "Last 7 days";
            valueListItem2.DataValue = "DateRange";
            valueListItem2.DisplayText = "Specify a date range:";
            this.osAuditDates.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem9,
            valueListItem10,
            valueListItem2});
            this.osAuditDates.Location = new System.Drawing.Point(13, 138);
            this.osAuditDates.Name = "osAuditDates";
            this.osAuditDates.Size = new System.Drawing.Size(128, 59);
            this.osAuditDates.TabIndex = 33;
            this.osAuditDates.Text = "Unrestricted";
            this.osAuditDates.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osAuditDates.ValueChanged += new System.EventHandler(this.osAuditDates_ValueChanged);
            // 
            // selectSingleHierarchyNodeControl1
            // 
            this.selectSingleHierarchyNodeControl1.AllowEnchancedNodeSearching = true;
            this.selectSingleHierarchyNodeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectSingleHierarchyNodeControl1.Enabled = false;
            this.selectSingleHierarchyNodeControl1.Location = new System.Drawing.Point(28, 97);
            this.selectSingleHierarchyNodeControl1.Name = "selectSingleHierarchyNodeControl1";
            this.selectSingleHierarchyNodeControl1.Size = new System.Drawing.Size(223, 21);
            this.selectSingleHierarchyNodeControl1.TabIndex = 32;
            // 
            // AllocationAnalysisOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.selectSingleHierarchyNodeControl1);
            this.Controls.Add(this.txtResultLimit);
            this.Controls.Add(this.osResultLimit);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.dteToTime);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.dteToDate);
            this.Controls.Add(this.dteFromTime);
            this.Controls.Add(this.dteFromDate);
            this.Controls.Add(this.osMerchandise);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.osHeaders);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.osAuditDates);
            this.Controls.Add(this.ultraLabel5);
            this.Name = "AllocationAnalysisOptionsControl";
            this.Size = new System.Drawing.Size(280, 339);
            ((System.ComponentModel.ISupportInitialize)(this.txtResultLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osResultLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osMerchandise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osHeaders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osAuditDates)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtResultLimit;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osResultLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToTime;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromTime;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromDate;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osMerchandise;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osHeaders;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private SelectSingleHierarchyNodeControl selectSingleHierarchyNodeControl1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osAuditDates;
    }
}
