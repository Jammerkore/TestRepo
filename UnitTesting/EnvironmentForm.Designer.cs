namespace UnitTesting
{
    partial class EnvironmentForm
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
            this.environmentControl1 = new UnitTesting.EnvironmentControl();
            this.SuspendLayout();
            // 
            // environmentControl1
            // 
            this.environmentControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.environmentControl1.Location = new System.Drawing.Point(0, 0);
            this.environmentControl1.Name = "environmentControl1";
            this.environmentControl1.Size = new System.Drawing.Size(674, 283);
            this.environmentControl1.TabIndex = 0;
            // 
            // EnvironmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 283);
            this.Controls.Add(this.environmentControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnvironmentForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Environments";
            this.Load += new System.EventHandler(this.EnvironmentForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private EnvironmentControl environmentControl1;


    }
}