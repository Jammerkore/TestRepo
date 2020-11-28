namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareDateBetween
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
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.daysTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.daysFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.chkTimeSensitive = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            ((System.ComponentModel.ISupportInitialize)(this.daysTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.daysFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTimeSensitive)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(224, 7);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(28, 14);
            this.ultraLabel2.TabIndex = 51;
            this.ultraLabel2.Text = "days";
            this.ultraLabel2.UseAppStyling = false;
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(135, 7);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(23, 14);
            this.ultraLabel1.TabIndex = 50;
            this.ultraLabel1.Text = "and";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // daysTo
            // 
            this.daysTo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.daysTo.FormatString = "";
            this.daysTo.Location = new System.Drawing.Point(164, 3);
            this.daysTo.MaskInput = "-nnn";
            this.daysTo.Name = "daysTo";
            this.daysTo.Size = new System.Drawing.Size(54, 21);
            this.daysTo.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.daysTo.SpinIncrement = 1;
            this.daysTo.TabIndex = 49;
            this.daysTo.UseAppStyling = false;
            this.daysTo.ValueChanged += new System.EventHandler(this.daysTo_ValueChanged);
            // 
            // daysFrom
            // 
            this.daysFrom.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.daysFrom.FormatString = "";
            this.daysFrom.Location = new System.Drawing.Point(75, 3);
            this.daysFrom.MaskInput = "-nnn";
            this.daysFrom.Name = "daysFrom";
            this.daysFrom.Size = new System.Drawing.Size(54, 21);
            this.daysFrom.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.daysFrom.SpinIncrement = 1;
            this.daysFrom.TabIndex = 48;
            this.daysFrom.UseAppStyling = false;
            this.daysFrom.Value = -7;
            this.daysFrom.ValueChanged += new System.EventHandler(this.daysFrom_ValueChanged);
            // 
            // chkTimeSensitive
            // 
            this.chkTimeSensitive.Location = new System.Drawing.Point(75, 27);
            this.chkTimeSensitive.Name = "chkTimeSensitive";
            this.chkTimeSensitive.Size = new System.Drawing.Size(143, 20);
            this.chkTimeSensitive.TabIndex = 52;
            this.chkTimeSensitive.Text = "Time Sensitive";
            this.chkTimeSensitive.CheckedChanged += new System.EventHandler(this.chkTimeSensitive_CheckedChanged);
            // 
            // filterElementValueToCompareDateBetween
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.chkTimeSensitive);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.daysTo);
            this.Controls.Add(this.daysFrom);
            this.Name = "filterElementValueToCompareDateBetween";
            this.Size = new System.Drawing.Size(267, 49);
            ((System.ComponentModel.ISupportInitialize)(this.daysTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.daysFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkTimeSensitive)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor daysTo;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor daysFrom;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor chkTimeSensitive;



    }
}
