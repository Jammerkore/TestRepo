namespace MIDRetail.Windows
{
    partial class ReportUserOptionsReviewForm
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
            this.reportUserOptionsReview1 = new MIDRetail.Windows.Controls.ReportUserOptionsReview();
            this.SuspendLayout();
            // 
            // reportUserOptionsReview1
            // 
            this.reportUserOptionsReview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportUserOptionsReview1.Location = new System.Drawing.Point(0, 0);
            this.reportUserOptionsReview1.Name = "reportUserOptionsReview1";
            this.reportUserOptionsReview1.Size = new System.Drawing.Size(424, 311);
            this.reportUserOptionsReview1.TabIndex = 0;
            // 
            // ReportUserOptionsReviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 311);
            this.Controls.Add(this.reportUserOptionsReview1);
            this.Name = "ReportUserOptionsReviewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Options Review Report";
            this.Load += new System.EventHandler(this.LoadForm);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ReportUserOptionsReview reportUserOptionsReview1;

    }
}