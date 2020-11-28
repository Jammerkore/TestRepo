namespace MIDRetail.Windows.Controls
{
    partial class filterElementMerchandise
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.selectSingleHierarchyNodeControl1 = new MIDRetail.Windows.Controls.SelectSingleHierarchyNodeControl();
            this.SuspendLayout();
            // 
            // ultraLabel3
            // 
            appearance1.TextHAlignAsString = "Right";
            this.ultraLabel3.Appearance = appearance1;
            this.ultraLabel3.Location = new System.Drawing.Point(2, 4);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(72, 14);
            this.ultraLabel3.TabIndex = 10;
            this.ultraLabel3.Text = "Merchandise:";
            this.ultraLabel3.UseAppStyling = false;
            // 
            // selectSingleHierarchyNodeControl1
            // 
            this.selectSingleHierarchyNodeControl1.AllowEnchancedNodeSearching = true;
            this.selectSingleHierarchyNodeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectSingleHierarchyNodeControl1.Location = new System.Drawing.Point(74, 0);
            this.selectSingleHierarchyNodeControl1.Name = "selectSingleHierarchyNodeControl1";
            this.selectSingleHierarchyNodeControl1.Size = new System.Drawing.Size(187, 21);
            this.selectSingleHierarchyNodeControl1.TabIndex = 11;
            // 
            // filterElementMerchandise
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.selectSingleHierarchyNodeControl1);
            this.Controls.Add(this.ultraLabel3);
            this.Name = "filterElementMerchandise";
            this.Size = new System.Drawing.Size(262, 21);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private SelectSingleHierarchyNodeControl selectSingleHierarchyNodeControl1;



    }
}
