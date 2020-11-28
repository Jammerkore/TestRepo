namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareString
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
            this.txtValueToCompare = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            ((System.ComponentModel.ISupportInitialize)(this.txtValueToCompare)).BeginInit();
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
            // txtValueToCompare
            // 
            this.txtValueToCompare.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValueToCompare.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.txtValueToCompare.Location = new System.Drawing.Point(74, 3);
            this.txtValueToCompare.MaxLength = 200;
            this.txtValueToCompare.Name = "txtValueToCompare";
            this.txtValueToCompare.Size = new System.Drawing.Size(187, 21);
            this.txtValueToCompare.TabIndex = 2;
            this.txtValueToCompare.UseAppStyling = false;
            this.txtValueToCompare.ValueChanged += new System.EventHandler(this.txtValueToCompare_ValueChanged);
            // 
            // filterElementValueToCompareString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtValueToCompare);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "filterElementValueToCompareString";
            this.Size = new System.Drawing.Size(267, 28);
            ((System.ComponentModel.ISupportInitialize)(this.txtValueToCompare)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtValueToCompare;


    }
}
