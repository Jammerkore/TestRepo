namespace MIDRetail.Windows
{
    partial class SearchResultsForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchResults1 = new MIDRetail.Windows.Controls.SearchResults();
            this.SuspendLayout();
            // 
            // searchResults1
            // 
            this.searchResults1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResults1.Location = new System.Drawing.Point(0, 0);
            this.searchResults1.Name = "searchResults1";
            this.searchResults1.Size = new System.Drawing.Size(715, 443);
            this.searchResults1.TabIndex = 0;
            // 
            // SearchResultsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 443);
            this.Controls.Add(this.searchResults1);
            this.Name = "SearchResultsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search";
            this.Load += new System.EventHandler(this.SearchResultsForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SearchResults searchResults1;


    }
}