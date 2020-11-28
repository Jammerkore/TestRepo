namespace UnitTesting
{
    partial class ViewExecutionForm
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
            this.viewExecutionGridControl1 = new UnitTesting.ViewExecutionGridControl();
            this.viewExecutionOutputParameters1 = new UnitTesting.ViewExecutionOutputParameters();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.SuspendLayout();
            // 
            // viewExecutionGridControl1
            // 
            this.viewExecutionGridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewExecutionGridControl1.Location = new System.Drawing.Point(0, 0);
            this.viewExecutionGridControl1.Name = "viewExecutionGridControl1";
            this.viewExecutionGridControl1.Size = new System.Drawing.Size(491, 531);
            this.viewExecutionGridControl1.TabIndex = 0;
            // 
            // viewExecutionOutputParameters1
            // 
            this.viewExecutionOutputParameters1.Dock = System.Windows.Forms.DockStyle.Right;
            this.viewExecutionOutputParameters1.Location = new System.Drawing.Point(497, 0);
            this.viewExecutionOutputParameters1.Name = "viewExecutionOutputParameters1";
            this.viewExecutionOutputParameters1.Size = new System.Drawing.Size(309, 531);
            this.viewExecutionOutputParameters1.TabIndex = 1;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(491, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 309;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 531);
            this.ultraSplitter1.TabIndex = 2;
            // 
            // ViewExecutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 531);
            this.Controls.Add(this.viewExecutionGridControl1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.viewExecutionOutputParameters1);
            this.MinimizeBox = false;
            this.Name = "ViewExecutionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewExecutionForm";
            this.Load += new System.EventHandler(this.ViewExecutionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewExecutionGridControl viewExecutionGridControl1;
        private ViewExecutionOutputParameters viewExecutionOutputParameters1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
    }
}