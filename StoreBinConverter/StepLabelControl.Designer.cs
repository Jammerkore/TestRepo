namespace StoreBinConverter
{
    partial class StepLabelControl
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFlowLayoutManager1
            // 
            this.ultraFlowLayoutManager1.ContainerControl = this;
            this.ultraFlowLayoutManager1.HorizontalAlignment = Infragistics.Win.Layout.DefaultableFlowLayoutAlignment.Near;
            this.ultraFlowLayoutManager1.HorizontalGap = -1;
            this.ultraFlowLayoutManager1.Margins.Left = 249;
            this.ultraFlowLayoutManager1.VerticalGap = 0;
            // 
            // ultraLabel2
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraLabel2.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel2.Location = new System.Drawing.Point(248, 0);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel2, new System.Drawing.Size(100, 23));
            this.ultraLabel2.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel2.TabIndex = 1;
            this.ultraLabel2.Text = "Status";
            // 
            // ultraLabel3
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance2;
            this.ultraLabel3.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel3.Location = new System.Drawing.Point(347, 0);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel3, new System.Drawing.Size(150, 23));
            this.ultraLabel3.Size = new System.Drawing.Size(150, 23);
            this.ultraLabel3.TabIndex = 2;
            this.ultraLabel3.Text = "Progress";
            // 
            // ultraLabel4
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel4.Appearance = appearance3;
            this.ultraLabel4.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel4.Location = new System.Drawing.Point(496, 0);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel4, new System.Drawing.Size(100, 23));
            this.ultraLabel4.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel4.TabIndex = 3;
            this.ultraLabel4.Text = "Time Elapsed";
            // 
            // ultraLabel5
            // 
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.ultraLabel5.Appearance = appearance5;
            this.ultraLabel5.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel5.Location = new System.Drawing.Point(694, 0);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel5, new System.Drawing.Size(200, 23));
            this.ultraLabel5.Size = new System.Drawing.Size(200, 23);
            this.ultraLabel5.TabIndex = 4;
            this.ultraLabel5.Text = "Result";
            // 
            // ultraLabel6
            // 
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.ultraLabel6.Appearance = appearance4;
            this.ultraLabel6.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.ultraLabel6.Location = new System.Drawing.Point(595, 0);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel6, new System.Drawing.Size(100, 23));
            this.ultraLabel6.Size = new System.Drawing.Size(100, 23);
            this.ultraLabel6.TabIndex = 5;
            this.ultraLabel6.Text = "Est. Time Left";
            // 
            // TaskSelectLabelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel3);
            this.Controls.Add(this.ultraLabel4);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.ultraLabel5);
            this.Name = "TaskSelectLabelControl";
            this.Size = new System.Drawing.Size(905, 23);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
    }
}
