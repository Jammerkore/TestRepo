namespace MIDRetail.Windows
{
    partial class AllocationAnalysisForm
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
            this.allocationAnalysisControl1 = new MIDRetail.Windows.Controls.AllocationAnalysisControl();
            this.SuspendLayout();
            // 
            // allocationAnalysisControl1
            // 
            this.allocationAnalysisControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.allocationAnalysisControl1.Location = new System.Drawing.Point(0, 0);
            this.allocationAnalysisControl1.Name = "allocationAnalysisControl1";
            this.allocationAnalysisControl1.Size = new System.Drawing.Size(784, 562);
            this.allocationAnalysisControl1.TabIndex = 0;
            // 
            // AllocationAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.allocationAnalysisControl1);
            this.Name = "AllocationAnalysisForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Allocation Analysis";
            this.Load += new System.EventHandler(this.AllocationAnalysisForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.AllocationAnalysisControl allocationAnalysisControl1;


    }
}