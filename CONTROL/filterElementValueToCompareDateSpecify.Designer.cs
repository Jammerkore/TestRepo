namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareDateSpecify
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
            this.dteToTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.lblTo = new Infragistics.Win.Misc.UltraLabel();
            this.lblFrom = new Infragistics.Win.Misc.UltraLabel();
            this.dteToDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromTime = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            this.dteFromDate = new Infragistics.Win.UltraWinEditors.UltraDateTimeEditor();
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // dteToTime
            // 
            this.dteToTime.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteToTime.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteToTime.DateTime = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteToTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.dteToTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dteToTime.Location = new System.Drawing.Point(177, 26);
            this.dteToTime.MaskInput = "{time}";
            this.dteToTime.Name = "dteToTime";
            this.dteToTime.Size = new System.Drawing.Size(78, 21);
            this.dteToTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dteToTime.TabIndex = 51;
            this.dteToTime.UseAppStyling = false;
            this.dteToTime.Value = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteToTime.ValueChanged += new System.EventHandler(this.dteToTime_ValueChanged);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(49, 29);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(21, 14);
            this.lblTo.TabIndex = 50;
            this.lblTo.Text = "To:";
            this.lblTo.UseAppStyling = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(38, 7);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(34, 14);
            this.lblFrom.TabIndex = 49;
            this.lblFrom.Text = "From:";
            this.lblFrom.UseAppStyling = false;
            // 
            // dteToDate
            // 
            this.dteToDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteToDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.dteToDate.Location = new System.Drawing.Point(75, 26);
            this.dteToDate.Name = "dteToDate";
            this.dteToDate.Size = new System.Drawing.Size(96, 21);
            this.dteToDate.TabIndex = 48;
            this.dteToDate.UseAppStyling = false;
            this.dteToDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteToDate.ValueChanged += new System.EventHandler(this.dteToDate_ValueChanged);
            // 
            // dteFromTime
            // 
            this.dteFromTime.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteFromTime.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteFromTime.DateTime = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteFromTime.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.dteFromTime.DropDownButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Never;
            this.dteFromTime.Location = new System.Drawing.Point(177, 3);
            this.dteFromTime.MaskInput = "{time}";
            this.dteFromTime.Name = "dteFromTime";
            this.dteFromTime.Size = new System.Drawing.Size(78, 21);
            this.dteFromTime.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.dteFromTime.TabIndex = 47;
            this.dteFromTime.UseAppStyling = false;
            this.dteFromTime.Value = new System.DateTime(2013, 8, 2, 9, 50, 0, 0);
            this.dteFromTime.ValueChanged += new System.EventHandler(this.dteFromTime_ValueChanged);
            // 
            // dteFromDate
            // 
            this.dteFromDate.AutoFillDate = Infragistics.Win.UltraWinMaskedEdit.AutoFillDate.MonthAndYear;
            this.dteFromDate.AutoFillTime = Infragistics.Win.UltraWinMaskedEdit.AutoFillTime.CurrentTime;
            this.dteFromDate.DateTime = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteFromDate.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.dteFromDate.Location = new System.Drawing.Point(75, 3);
            this.dteFromDate.MaskInput = "{date}";
            this.dteFromDate.Name = "dteFromDate";
            this.dteFromDate.Size = new System.Drawing.Size(96, 21);
            this.dteFromDate.TabIndex = 46;
            this.dteFromDate.UseAppStyling = false;
            this.dteFromDate.Value = new System.DateTime(2013, 8, 9, 0, 0, 0, 0);
            this.dteFromDate.ValueChanged += new System.EventHandler(this.dteFromDate_ValueChanged);
            // 
            // filterElementValueToCompareDateSpecify
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.dteToTime);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dteToDate);
            this.Controls.Add(this.dteFromTime);
            this.Controls.Add(this.dteFromDate);
            this.Name = "filterElementValueToCompareDateSpecify";
            this.Size = new System.Drawing.Size(267, 52);
            this.Load += new System.EventHandler(this.filterElementValueToCompareDateSpecify_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dteToTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFromDate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToTime;
        private Infragistics.Win.Misc.UltraLabel lblTo;
        private Infragistics.Win.Misc.UltraLabel lblFrom;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteToDate;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromTime;
        private Infragistics.Win.UltraWinEditors.UltraDateTimeEditor dteFromDate;




    }
}
