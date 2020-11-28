namespace MIDRetail.Windows.Controls
{
    partial class ForecastAnalysisOptionsControl
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
            Infragistics.Win.ValueListItem valueListItem7 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem8 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.ValueListItem valueListItem5 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem6 = new Infragistics.Win.ValueListItem();
            this.txtResultLimit = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.osResultLimit = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel6 = new Infragistics.Win.Misc.UltraLabel();
            this.osMerchandise = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.osLowLevel = new Infragistics.Win.UltraWinEditors.UltraOptionSet();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.selectHierarchyLowLevelControl1 = new MIDRetail.Windows.Controls.SelectHierarchyLowLevelControl();
            this.selectSingleHierarchyNodeControl1 = new MIDRetail.Windows.Controls.SelectSingleHierarchyNodeControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osResultLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osMerchandise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osLowLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // txtResultLimit
            // 
            this.txtResultLimit.Enabled = false;
            this.txtResultLimit.Location = new System.Drawing.Point(28, 185);
            this.txtResultLimit.Name = "txtResultLimit";
            this.txtResultLimit.Size = new System.Drawing.Size(76, 21);
            this.txtResultLimit.TabIndex = 31;
            this.txtResultLimit.Text = "5000";
            // 
            // osResultLimit
            // 
            this.osResultLimit.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osResultLimit.CheckedIndex = 0;
            valueListItem7.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem7.DataValue = "Unrestricted";
            valueListItem7.DisplayText = "Unrestricted";
            valueListItem8.DataValue = "Limit";
            valueListItem8.DisplayText = "Limit the maximum results:";
            this.osResultLimit.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem7,
            valueListItem8});
            this.osResultLimit.Location = new System.Drawing.Point(13, 157);
            this.osResultLimit.Name = "osResultLimit";
            this.osResultLimit.Size = new System.Drawing.Size(274, 35);
            this.osResultLimit.TabIndex = 30;
            this.osResultLimit.Text = "Unrestricted";
            this.osResultLimit.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osResultLimit.ValueChanged += new System.EventHandler(this.osResultLimit_ValueChanged);
            // 
            // ultraLabel6
            // 
            this.ultraLabel6.AutoSize = true;
            this.ultraLabel6.Location = new System.Drawing.Point(4, 142);
            this.ultraLabel6.Name = "ultraLabel6";
            this.ultraLabel6.Size = new System.Drawing.Size(67, 14);
            this.ultraLabel6.TabIndex = 29;
            this.ultraLabel6.Text = "Result Limit:";
            // 
            // osMerchandise
            // 
            this.osMerchandise.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osMerchandise.CheckedIndex = 0;
            valueListItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem1.DataValue = "Unrestricted";
            valueListItem1.DisplayText = "Unrestricted";
            valueListItem2.DataValue = "Restrict";
            valueListItem2.DisplayText = "Restrict to following node and descendants:";
            this.osMerchandise.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.osMerchandise.Location = new System.Drawing.Point(13, 15);
            this.osMerchandise.Name = "osMerchandise";
            this.osMerchandise.Size = new System.Drawing.Size(274, 35);
            this.osMerchandise.TabIndex = 19;
            this.osMerchandise.Text = "Unrestricted";
            this.osMerchandise.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osMerchandise.ValueChanged += new System.EventHandler(this.osMerchandise_ValueChanged);
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(4, 1);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(72, 14);
            this.ultraLabel2.TabIndex = 20;
            this.ultraLabel2.Text = "Merchandise:";
            // 
            // osLowLevel
            // 
            appearance1.BackColorDisabled = System.Drawing.SystemColors.Window;
            appearance1.BackColorDisabled2 = System.Drawing.SystemColors.Window;
            this.osLowLevel.Appearance = appearance1;
            this.osLowLevel.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
            this.osLowLevel.CheckedIndex = 0;
            this.osLowLevel.Enabled = false;
            valueListItem5.CheckState = System.Windows.Forms.CheckState.Checked;
            valueListItem5.DataValue = "Unrestricted";
            valueListItem5.DisplayText = "Unrestricted";
            valueListItem6.DataValue = "Restrict";
            valueListItem6.DisplayText = "Restrict to following level and above:";
            this.osLowLevel.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem5,
            valueListItem6});
            this.osLowLevel.Location = new System.Drawing.Point(41, 77);
            this.osLowLevel.Name = "osLowLevel";
            this.osLowLevel.Size = new System.Drawing.Size(206, 35);
            this.osLowLevel.TabIndex = 33;
            this.osLowLevel.Text = "Unrestricted";
            this.osLowLevel.UseFlatMode = Infragistics.Win.DefaultableBoolean.False;
            this.osLowLevel.ValueChanged += new System.EventHandler(this.osLowLevel_ValueChanged);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(28, 64);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(58, 14);
            this.ultraLabel1.TabIndex = 34;
            this.ultraLabel1.Text = "Low Level:";
            // 
            // selectHierarchyLowLevelControl1
            // 
            this.selectHierarchyLowLevelControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectHierarchyLowLevelControl1.Enabled = false;
            this.selectHierarchyLowLevelControl1.Location = new System.Drawing.Point(56, 106);
            this.selectHierarchyLowLevelControl1.Name = "selectHierarchyLowLevelControl1";
            this.selectHierarchyLowLevelControl1.Size = new System.Drawing.Size(195, 23);
            this.selectHierarchyLowLevelControl1.TabIndex = 35;
            // 
            // selectSingleHierarchyNodeControl1
            // 
            this.selectSingleHierarchyNodeControl1.AllowEnchancedNodeSearching = true;
            this.selectSingleHierarchyNodeControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.selectSingleHierarchyNodeControl1.Enabled = false;
            this.selectSingleHierarchyNodeControl1.Location = new System.Drawing.Point(28, 43);
            this.selectSingleHierarchyNodeControl1.Name = "selectSingleHierarchyNodeControl1";
            this.selectSingleHierarchyNodeControl1.Size = new System.Drawing.Size(223, 21);
            this.selectSingleHierarchyNodeControl1.TabIndex = 32;
            // 
            // ForecastAnalysisOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.selectHierarchyLowLevelControl1);
            this.Controls.Add(this.osLowLevel);
            this.Controls.Add(this.selectSingleHierarchyNodeControl1);
            this.Controls.Add(this.txtResultLimit);
            this.Controls.Add(this.osResultLimit);
            this.Controls.Add(this.ultraLabel6);
            this.Controls.Add(this.osMerchandise);
            this.Controls.Add(this.ultraLabel2);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "ForecastAnalysisOptionsControl";
            this.Size = new System.Drawing.Size(280, 220);
            ((System.ComponentModel.ISupportInitialize)(this.txtResultLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osResultLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osMerchandise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osLowLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraTextEditor txtResultLimit;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osResultLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel6;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osMerchandise;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private SelectSingleHierarchyNodeControl selectSingleHierarchyNodeControl1;
        private Infragistics.Win.UltraWinEditors.UltraOptionSet osLowLevel;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private SelectHierarchyLowLevelControl selectHierarchyLowLevelControl1;
    }
}
