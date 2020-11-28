namespace MIDRetail.Windows.Controls
{
    partial class filterElementValueToCompareBool
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
            Infragistics.Win.ValueListItem valueListItem1 = new Infragistics.Win.ValueListItem();
            Infragistics.Win.ValueListItem valueListItem2 = new Infragistics.Win.ValueListItem();
            this.cboBoolean = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            ((System.ComponentModel.ISupportInitialize)(this.cboBoolean)).BeginInit();
            this.SuspendLayout();
            // 
            // cboBoolean
            // 
            this.cboBoolean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBoolean.DisplayMember = "Variable";
            this.cboBoolean.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.cboBoolean.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "True";
            valueListItem1.DisplayText = "True";
            valueListItem2.DataValue = "False";
            valueListItem2.DisplayText = "False";
            this.cboBoolean.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cboBoolean.LimitToList = true;
            this.cboBoolean.Location = new System.Drawing.Point(75, 3);
            this.cboBoolean.Name = "cboBoolean";
            this.cboBoolean.Size = new System.Drawing.Size(187, 21);
            this.cboBoolean.TabIndex = 3;
            this.cboBoolean.Text = "False";
            this.cboBoolean.UseAppStyling = false;
            this.cboBoolean.ValueChanged += new System.EventHandler(this.cboBoolean_ValueChanged);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(35, 7);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(36, 14);
            this.ultraLabel1.TabIndex = 2;
            this.ultraLabel1.Text = "Value:";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // filterElementValueToCompareBool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cboBoolean);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "filterElementValueToCompareBool";
            this.Size = new System.Drawing.Size(267, 28);
            ((System.ComponentModel.ISupportInitialize)(this.cboBoolean)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboBoolean;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;




    }
}
