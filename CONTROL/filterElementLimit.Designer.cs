namespace MIDRetail.Windows.Controls
{
    partial class filterElementLimit
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.ultraLabel5 = new Infragistics.Win.Misc.UltraLabel();
            this.pnlLimit = new Infragistics.Win.Misc.UltraPanel();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.resultLimit = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.cboLimitType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.pnlLimit.ClientArea.SuspendLayout();
            this.pnlLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLimitType)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel5
            // 
            this.ultraLabel5.AutoSize = true;
            this.ultraLabel5.Location = new System.Drawing.Point(5, 7);
            this.ultraLabel5.Name = "ultraLabel5";
            this.ultraLabel5.Size = new System.Drawing.Size(67, 14);
            this.ultraLabel5.TabIndex = 40;
            this.ultraLabel5.Text = "Result Limit:";
            this.ultraLabel5.UseAppStyling = false;
            // 
            // pnlLimit
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.pnlLimit.Appearance = appearance1;
            // 
            // pnlLimit.ClientArea
            // 
            this.pnlLimit.ClientArea.Controls.Add(this.ultraLabel2);
            this.pnlLimit.ClientArea.Controls.Add(this.resultLimit);
            this.pnlLimit.Location = new System.Drawing.Point(72, 30);
            this.pnlLimit.Name = "pnlLimit";
            this.pnlLimit.Size = new System.Drawing.Size(183, 28);
            this.pnlLimit.TabIndex = 44;
            this.pnlLimit.UseAppStyling = false;
            this.pnlLimit.Visible = false;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(109, 7);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(28, 14);
            this.ultraLabel2.TabIndex = 47;
            this.ultraLabel2.Text = "rows";
            this.ultraLabel2.UseAppStyling = false;
            // 
            // resultLimit
            // 
            this.resultLimit.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.resultLimit.FormatString = "";
            this.resultLimit.Location = new System.Drawing.Point(3, 3);
            this.resultLimit.MaskInput = "{LOC}nn,nnn,nnn";
            this.resultLimit.Name = "resultLimit";
            this.resultLimit.Size = new System.Drawing.Size(100, 21);
            this.resultLimit.SpinButtonDisplayStyle = Infragistics.Win.ButtonDisplayStyle.Always;
            this.resultLimit.SpinIncrement = 1000;
            this.resultLimit.TabIndex = 44;
            this.resultLimit.UseAppStyling = false;
            this.resultLimit.Value = 5000;
            this.resultLimit.ValueChanged += new System.EventHandler(this.resultLimit_ValueChanged);
            // 
            // cboLimitType
            // 
            this.cboLimitType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLimitType.DisplayMember = "";
            this.cboLimitType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.cboLimitType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "Unrestricted";
            valueListItem1.DisplayText = "Unrestricted";
            valueListItem2.DataValue = "Restricted";
            valueListItem2.DisplayText = "Restricted:";
            this.cboLimitType.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cboLimitType.LimitToList = true;
            this.cboLimitType.Location = new System.Drawing.Point(74, 3);
            this.cboLimitType.Name = "cboLimitType";
            this.cboLimitType.Size = new System.Drawing.Size(187, 21);
            this.cboLimitType.TabIndex = 46;
            this.cboLimitType.UseAppStyling = false;
            this.cboLimitType.ValueChanged += new System.EventHandler(this.cboLimitType_ValueChanged);
            // 
            // filterElementLimit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cboLimitType);
            this.Controls.Add(this.pnlLimit);
            this.Controls.Add(this.ultraLabel5);
            this.Name = "filterElementLimit";
            this.Size = new System.Drawing.Size(267, 87);
            this.pnlLimit.ClientArea.ResumeLayout(false);
            this.pnlLimit.ClientArea.PerformLayout();
            this.pnlLimit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLimitType)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel5;
        private Infragistics.Win.Misc.UltraPanel pnlLimit;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor resultLimit;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboLimitType;



    }
}
