namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareNumeric
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
            this.numericEditor = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            ((System.ComponentModel.ISupportInitialize)(this.numericEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(33, 7);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(36, 14);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Value:";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // numericEditor
            // 
            this.numericEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericEditor.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.numericEditor.FormatString = "";
            this.numericEditor.Location = new System.Drawing.Point(74, 3);
            this.numericEditor.MaskInput = "{LOC}nn,nnn,nnn.nnnn";
            this.numericEditor.Name = "numericEditor";
            this.numericEditor.NumericType = Infragistics.Win.UltraWinEditors.NumericType.Double;
            this.numericEditor.Size = new System.Drawing.Size(187, 21);
            this.numericEditor.SpinIncrement = 1;
            this.numericEditor.TabIndex = 45;
            this.numericEditor.UseAppStyling = false;
            this.numericEditor.ValueChanged += new System.EventHandler(this.numericEditor_ValueChanged);
            // 
            // filterElementValueToCompareNumeric
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.numericEditor);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "filterElementValueToCompareNumeric";
            this.Size = new System.Drawing.Size(267, 28);
            ((System.ComponentModel.ISupportInitialize)(this.numericEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor numericEditor;


    }
}
