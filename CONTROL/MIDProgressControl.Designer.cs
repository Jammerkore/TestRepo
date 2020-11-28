namespace MIDRetail.Windows.Controls
{
    partial class MIDProgressControl
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
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraProgressBar1 = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.SuspendLayout();
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraLabel1.Location = new System.Drawing.Point(21, 24);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(445, 17);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "ultraLabel1";
            // 
            // ultraProgressBar1
            // 
            this.ultraProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraProgressBar1.Location = new System.Drawing.Point(21, 47);
            this.ultraProgressBar1.Name = "ultraProgressBar1";
            this.ultraProgressBar1.Size = new System.Drawing.Size(445, 23);
            this.ultraProgressBar1.TabIndex = 1;
            this.ultraProgressBar1.Text = "[Formatted]";
            // 
            // MIDProgressControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ultraProgressBar1);
            this.Controls.Add(this.ultraLabel1);
            this.Name = "MIDProgressControl";
            this.Size = new System.Drawing.Size(493, 109);
            this.ResumeLayout(false);

        }

        #endregion

        public Infragistics.Win.Misc.UltraLabel ultraLabel1;
        public Infragistics.Win.UltraWinProgressBar.UltraProgressBar ultraProgressBar1;

    }
}
