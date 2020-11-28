namespace MIDRetail.Windows.Controls
{
    partial class SearchResultContainer
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
            this.searchLoadingUI1 = new MIDRetail.Windows.Controls.SearchLoadingUI();
            this.gridControl1 = new MIDRetail.Windows.Controls.MIDGridControl();
            this.SuspendLayout();
            // 
            // searchLoadingUI1
            // 
            this.searchLoadingUI1.BackColor = System.Drawing.Color.White;
            this.searchLoadingUI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchLoadingUI1.Location = new System.Drawing.Point(0, 0);
            this.searchLoadingUI1.Name = "searchLoadingUI1";
            this.searchLoadingUI1.Size = new System.Drawing.Size(330, 247);
            this.searchLoadingUI1.TabIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(330, 247);
            this.gridControl1.TabIndex = 0;
            // 
            // SearchResultContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchLoadingUI1);
            this.Controls.Add(this.gridControl1);
            this.Name = "SearchResultContainer";
            this.Size = new System.Drawing.Size(330, 247);
            this.Load += new System.EventHandler(this.SearchResultContainer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public MIDGridControl gridControl1;
        public SearchLoadingUI searchLoadingUI1;
    }
}
