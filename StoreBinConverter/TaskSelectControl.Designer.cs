namespace StoreBinConverter
{
    partial class TaskSelectControl
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.lblStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.lblTime = new Infragistics.Win.Misc.UltraLabel();
            this.txtTask = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.lblEstTime = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.lblStatus.Appearance = appearance1;
            this.lblStatus.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblStatus.Location = new System.Drawing.Point(250, 0);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 0;
            // 
            // ultraFlowLayoutManager1
            // 
            this.ultraFlowLayoutManager1.ContainerControl = this;
            this.ultraFlowLayoutManager1.HorizontalAlignment = Infragistics.Win.Layout.DefaultableFlowLayoutAlignment.Near;
            this.ultraFlowLayoutManager1.HorizontalGap = 0;
            this.ultraFlowLayoutManager1.VerticalGap = 0;
            this.ultraFlowLayoutManager1.WrapItems = false;
            // 
            // lblTime
            // 
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.lblTime.Appearance = appearance4;
            this.lblTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblTime.Location = new System.Drawing.Point(500, 0);
            this.lblTime.Name = "lblTime";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.lblTime, new System.Drawing.Size(100, 23));
            this.lblTime.Size = new System.Drawing.Size(100, 23);
            this.lblTime.TabIndex = 3;
            // 
            // txtTask
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.TextHAlignAsString = "Left";
            appearance2.TextVAlignAsString = "Middle";
            this.txtTask.Appearance = appearance2;
            this.txtTask.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.txtTask.Location = new System.Drawing.Point(0, 0);
            this.txtTask.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.txtTask.Name = "txtTask";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.txtTask, new System.Drawing.Size(250, 23));
            this.txtTask.Size = new System.Drawing.Size(250, 23);
            this.txtTask.TabIndex = 5;
            this.txtTask.Text = "Task";
            // 
            // ultraLabel1
            // 
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance3;
            this.ultraLabel1.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.ultraLabel1.Location = new System.Drawing.Point(350, 0);
            this.ultraLabel1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.ultraLabel1, new System.Drawing.Size(150, 23));
            this.ultraLabel1.Size = new System.Drawing.Size(150, 23);
            this.ultraLabel1.TabIndex = 6;
            // 
            // lblEstTime
            // 
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.lblEstTime.Appearance = appearance5;
            this.lblEstTime.BorderStyleOuter = Infragistics.Win.UIElementBorderStyle.None;
            this.lblEstTime.Location = new System.Drawing.Point(600, 0);
            this.lblEstTime.Name = "lblEstTime";
            this.ultraFlowLayoutManager1.SetPreferredSize(this.lblEstTime, new System.Drawing.Size(100, 23));
            this.lblEstTime.Size = new System.Drawing.Size(100, 23);
            this.lblEstTime.TabIndex = 7;
            // 
            // TaskSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.txtTask);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblEstTime);
            this.Name = "TaskSelectControl";
            this.Size = new System.Drawing.Size(905, 23);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
        public Infragistics.Win.Misc.UltraLabel lblStatus;
        public Infragistics.Win.Misc.UltraLabel lblTime;
        public Infragistics.Win.Misc.UltraLabel txtTask;
        public Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.Misc.UltraLabel lblEstTime;
    }
}
