namespace StoreBinConverter
{
    partial class StepControl
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.lblStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraProgressBar1 = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.lblTime = new Infragistics.Win.Misc.UltraLabel();
            this.lblEstTime = new Infragistics.Win.Misc.UltraLabel();
            this.lblStep = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditor1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.lblProgress = new Infragistics.Win.Misc.UltraLabel();
            this.lblResult = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblStatus.Appearance = appearance1;
            this.lblStatus.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblStatus.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblStatus.Location = new System.Drawing.Point(248, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Not Started";
            // 
            // ultraProgressBar1
            // 
            this.ultraProgressBar1.Location = new System.Drawing.Point(347, 0);
            this.ultraProgressBar1.Name = "ultraProgressBar1";
            this.ultraProgressBar1.Size = new System.Drawing.Size(150, 23);
            this.ultraProgressBar1.TabIndex = 2;
            this.ultraProgressBar1.Text = "[Formatted]";
            this.ultraProgressBar1.UseFlatMode = Infragistics.Win.DefaultableBoolean.True;
            // 
            // lblTime
            // 
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.lblTime.Appearance = appearance2;
            this.lblTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTime.Location = new System.Drawing.Point(496, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 3;
            // 
            // lblEstTime
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.lblEstTime.Appearance = appearance3;
            this.lblEstTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblEstTime.Location = new System.Drawing.Point(595, 0);
            this.lblEstTime.Name = "lblEstTime";
            this.lblEstTime.Size = new System.Drawing.Size(100, 23);
            this.lblEstTime.TabIndex = 5;
            // 
            // lblStep
            // 
            appearance4.TextHAlignAsString = "Left";
            appearance4.TextVAlignAsString = "Middle";
            this.lblStep.Appearance = appearance4;
            this.lblStep.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblStep.Location = new System.Drawing.Point(0, 0);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(248, 23);
            this.lblStep.TabIndex = 7;
            this.lblStep.Text = "Table 0";
            // 
            // ultraCheckEditor1
            // 
            this.ultraCheckEditor1.Location = new System.Drawing.Point(19, 0);
            this.ultraCheckEditor1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ultraCheckEditor1.Name = "ultraCheckEditor1";
            this.ultraCheckEditor1.Size = new System.Drawing.Size(225, 23);
            this.ultraCheckEditor1.TabIndex = 8;
            this.ultraCheckEditor1.Text = "ultraCheckEditor1";
            this.ultraCheckEditor1.Visible = false;
            // 
            // lblProgress
            // 
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.lblProgress.Appearance = appearance5;
            this.lblProgress.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblProgress.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblProgress.Location = new System.Drawing.Point(347, 0);
            this.lblProgress.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(150, 23);
            this.lblProgress.TabIndex = 9;
            this.lblProgress.Visible = false;
            // 
            // lblResult
            // 
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.lblResult.Appearance = appearance6;
            this.lblResult.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblResult.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblResult.Location = new System.Drawing.Point(694, 0);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(200, 23);
            this.lblResult.TabIndex = 10;
            // 
            // StepControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ultraProgressBar1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblEstTime);
            this.Controls.Add(this.ultraCheckEditor1);
            this.Controls.Add(this.lblStep);
            this.Name = "StepControl";
            this.Size = new System.Drawing.Size(905, 23);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Infragistics.Win.Misc.UltraLabel lblStatus;
        public Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar1;
        public Infragistics.Win.Misc.UltraLabel lblTime;
        public Infragistics.Win.Misc.UltraLabel lblEstTime;
        public Infragistics.Win.Misc.UltraLabel lblStep;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditor1;
        public Infragistics.Win.Misc.UltraLabel lblProgress;
        public Infragistics.Win.Misc.UltraLabel lblResult;
    }
}
