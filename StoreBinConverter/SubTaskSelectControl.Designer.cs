namespace StoreBinConverter
{
    partial class SubTaskSelectControl
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            this.lblStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraCheckEditor1 = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this.ultraProgressBar1 = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.lblTime = new Infragistics.Win.Misc.UltraLabel();
            this.txtResult = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.lblEstTime = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResult)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.lblStatus.Appearance = appearance6;
            this.lblStatus.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblStatus.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblStatus.Location = new System.Drawing.Point(248, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Not Started";
            // 
            // ultraCheckEditor1
            // 
            this.ultraCheckEditor1.Location = new System.Drawing.Point(24, 0);
            this.ultraCheckEditor1.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ultraCheckEditor1.Name = "ultraCheckEditor1";
            this.ultraCheckEditor1.Size = new System.Drawing.Size(225, 23);
            this.ultraCheckEditor1.TabIndex = 1;
            this.ultraCheckEditor1.Text = "ultraCheckEditor1";
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
            appearance7.TextHAlignAsString = "Center";
            appearance7.TextVAlignAsString = "Middle";
            this.lblTime.Appearance = appearance7;
            this.lblTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblTime.Location = new System.Drawing.Point(496, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 3;
            // 
            // txtResult
            // 
            appearance3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColorDisabled = System.Drawing.SystemColors.ControlLightLight;
            appearance3.BackColorDisabled2 = System.Drawing.SystemColors.ControlLightLight;
            this.txtResult.Appearance = appearance3;
            this.txtResult.AutoSize = false;
            this.txtResult.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtResult.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.txtResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtResult.Location = new System.Drawing.Point(694, 0);
            this.txtResult.Name = "txtResult";
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColorDisabled = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColorDisabled2 = System.Drawing.SystemColors.ControlLightLight;
            this.txtResult.NullTextAppearance = appearance4;
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(200, 23);
            this.txtResult.TabIndex = 4;
            // 
            // lblEstTime
            // 
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.lblEstTime.Appearance = appearance8;
            this.lblEstTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.Solid;
            this.lblEstTime.Location = new System.Drawing.Point(595, 0);
            this.lblEstTime.Name = "lblEstTime";
            this.lblEstTime.Size = new System.Drawing.Size(100, 23);
            this.lblEstTime.TabIndex = 5;
            // 
            // SubTaskSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ultraProgressBar1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblEstTime);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.ultraCheckEditor1);
            this.Name = "SubTaskSelectControl";
            this.Size = new System.Drawing.Size(905, 23);
            ((System.ComponentModel.ISupportInitialize)(this.ultraCheckEditor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Infragistics.Win.Misc.UltraLabel lblStatus;
        public Infragistics.Win.UltraWinEditors.UltraCheckEditor ultraCheckEditor1;
        public Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar1;
        public Infragistics.Win.Misc.UltraLabel lblTime;
        public Infragistics.Win.UltraWinEditors.UltraTextEditor txtResult;
        public Infragistics.Win.Misc.UltraLabel lblEstTime;
    }
}
