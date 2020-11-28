namespace MIDRetail.Windows.Controls
{
    partial class filterElementFolder
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
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.cboFolder = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            ((System.ComponentModel.ISupportInitialize)(this.cboFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(31, 7);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(39, 14);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Folder:";
            this.ultraLabel1.UseAppStyling = false;
            // 
            // cboFolder
            // 
            this.cboFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFolder.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this.cboFolder.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            valueListItem1.DataValue = "User";
            valueListItem1.DisplayText = "User";
            valueListItem2.DataValue = "Global";
            valueListItem2.DisplayText = "Global";
            this.cboFolder.Items.AddRange(new Infragistics.Win.ValueListItem[] {
            valueListItem1,
            valueListItem2});
            this.cboFolder.LimitToList = true;
            this.cboFolder.Location = new System.Drawing.Point(74, 3);
            this.cboFolder.Name = "cboFolder";
            this.cboFolder.Size = new System.Drawing.Size(187, 21);
            this.cboFolder.TabIndex = 4;
            this.cboFolder.Text = "User";
            this.cboFolder.UseAppStyling = false;
            this.cboFolder.TextChanged += new System.EventHandler(this.cboFolder_TextChanged);
            // 
            // filterElementFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.cboFolder);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "filterElementFolder";
            this.Size = new System.Drawing.Size(267, 27);
            ((System.ComponentModel.ISupportInitialize)(this.cboFolder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor cboFolder;


    }
}
