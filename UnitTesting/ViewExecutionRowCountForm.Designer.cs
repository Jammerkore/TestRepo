namespace UnitTesting
{
    partial class ViewExecutionRowCountForm
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
            this.viewExecutionRowCountControl1 = new UnitTesting.ViewExecutionRowCountControl();
            this.viewExecutionOutputParameters1 = new UnitTesting.ViewExecutionOutputParameters();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.SuspendLayout();
            // 
            // viewExecutionRowCountControl1
            // 
            this.viewExecutionRowCountControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewExecutionRowCountControl1.Location = new System.Drawing.Point(0, 0);
            this.viewExecutionRowCountControl1.Name = "viewExecutionRowCountControl1";
            this.viewExecutionRowCountControl1.Size = new System.Drawing.Size(492, 531);
            this.viewExecutionRowCountControl1.TabIndex = 0;
            // 
            // viewExecutionOutputParameters1
            // 
            this.viewExecutionOutputParameters1.Dock = System.Windows.Forms.DockStyle.Right;
            this.viewExecutionOutputParameters1.Location = new System.Drawing.Point(498, 0);
            this.viewExecutionOutputParameters1.Name = "viewExecutionOutputParameters1";
            this.viewExecutionOutputParameters1.Size = new System.Drawing.Size(308, 531);
            this.viewExecutionOutputParameters1.TabIndex = 1;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(492, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 308;
            this.ultraSplitter1.Size = new System.Drawing.Size(6, 531);
            this.ultraSplitter1.TabIndex = 2;
            // 
            // ViewExecutionRowCountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 531);
            this.Controls.Add(this.viewExecutionRowCountControl1);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.viewExecutionOutputParameters1);
            this.MinimizeBox = false;
            this.Name = "ViewExecutionRowCountForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ViewExecutionRowCountForm";
            this.Load += new System.EventHandler(this.ViewExecutionRowCountForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewExecutionRowCountControl viewExecutionRowCountControl1;
        private ViewExecutionOutputParameters viewExecutionOutputParameters1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;

    }
}