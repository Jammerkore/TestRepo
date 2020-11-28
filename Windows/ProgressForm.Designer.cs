namespace MIDRetail.Windows
{
    partial class ProgressForm
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
            this.midProgressControl1 = new MIDRetail.Windows.Controls.MIDProgressControl();
            this.SuspendLayout();
            // 
            // midProgressControl1
            // 
            this.midProgressControl1.BackColor = System.Drawing.Color.White;
            this.midProgressControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midProgressControl1.Location = new System.Drawing.Point(0, 0);
            this.midProgressControl1.Name = "midProgressControl1";
            this.midProgressControl1.Size = new System.Drawing.Size(555, 114);
            this.midProgressControl1.TabIndex = 0;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 114);
            this.ControlBox = false;
            this.Controls.Add(this.midProgressControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ProgressForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Wait...";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public Controls.MIDProgressControl midProgressControl1;

    }
}