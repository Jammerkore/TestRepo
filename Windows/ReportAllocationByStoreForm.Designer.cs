namespace MIDRetail.Windows
{
    partial class ReportAllocationByStoreForm
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
            this.reportAllocationByStoreControl = new MIDRetail.Windows.Controls.ReportAllocationByStore();
            this.SuspendLayout();
            // 
            // reportUserOptionsReview1
            // 
            this.reportAllocationByStoreControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportAllocationByStoreControl.Location = new System.Drawing.Point(0, 0);
            this.reportAllocationByStoreControl.Name = "reportUserOptionsReview1";
            this.reportAllocationByStoreControl.Size = new System.Drawing.Size(424, 311);
            this.reportAllocationByStoreControl.TabIndex = 0;
            // 
            // ReportAllocationByStoreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 311);
            this.Controls.Add(this.reportAllocationByStoreControl);
            this.Name = "ReportAllocationByStoreForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allocation By Store Report";
            this.Load += new System.EventHandler(this.LoadForm);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ReportAllocationByStore reportAllocationByStoreControl;

    }
}