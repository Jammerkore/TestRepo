namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareNumericBetween
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
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.numericEditorFrom = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.numericEditorTo = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditorFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditorTo)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(33, 7);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(34, 14);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "From:";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // numericEditorFrom
            // 
            this.numericEditorFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericEditorFrom.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.numericEditorFrom.FormatString = "";
            this.numericEditorFrom.Location = new System.Drawing.Point(74, 3);
            this.numericEditorFrom.MaskInput = "{LOC}nnn,nnn,nnn,nnn.nnnn";
            this.numericEditorFrom.Name = "numericEditorFrom";
            this.numericEditorFrom.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numericEditorFrom.Size = new System.Drawing.Size(187, 21);
            this.numericEditorFrom.SpinIncrement = 1;
            this.numericEditorFrom.TabIndex = 45;
            this.numericEditorFrom.UseAppStyling = false;
            this.numericEditorFrom.Value = -7D;
            this.numericEditorFrom.ValueChanged += new System.EventHandler(this.numericEditor_ValueChanged);
            // 
            // numericEditorTo
            // 
            this.numericEditorTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericEditorTo.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.numericEditorTo.FormatString = "";
            this.numericEditorTo.Location = new System.Drawing.Point(74, 30);
            this.numericEditorTo.MaskInput = "{LOC}nnn,nnn,nnn,nnn.nnnn";
            this.numericEditorTo.Name = "numericEditorTo";
            this.numericEditorTo.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numericEditorTo.Size = new System.Drawing.Size(187, 21);
            this.numericEditorTo.SpinIncrement = 1;
            this.numericEditorTo.TabIndex = 47;
            this.numericEditorTo.UseAppStyling = false;
            this.numericEditorTo.Value = -7D;
            this.numericEditorTo.ValueChanged += new System.EventHandler(this.numericEditor_ValueChanged);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(44, 34);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(21, 14);
            this.ultraLabel2.TabIndex = 46;
            this.ultraLabel2.Text = "To:";
            this.ultraLabel2.UseAppStyling = false;
            // 
            // filterElementValueToCompareNumericBetween
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.numericEditorTo);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.numericEditorFrom);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "filterElementValueToCompareNumericBetween";
            this.Size = new System.Drawing.Size(267, 60);
            ((System.ComponentModel.ISupportInitialize)(this.numericEditorFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditorTo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditorFrom;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditorTo;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;


    }
}
