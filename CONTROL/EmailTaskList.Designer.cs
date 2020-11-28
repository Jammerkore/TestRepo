namespace MIDRetail.Windows.Controls
{
    partial class EmailTaskList
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
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this.emailSuccessFieldEntry = new MIDRetail.Windows.Controls.EmailFieldEntry();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this.emailFailureFieldEntry = new MIDRetail.Windows.Controls.EmailFieldEntry();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox1.Controls.Add(this.emailSuccessFieldEntry);
            this.ultraGroupBox1.Location = new System.Drawing.Point(4, 4);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(417, 215);
            this.ultraGroupBox1.TabIndex = 0;
            this.ultraGroupBox1.Text = "On Success:";
            // 
            // emailSuccessFieldEntry
            // 
            this.emailSuccessFieldEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailSuccessFieldEntry.emailBody = "";
            this.emailSuccessFieldEntry.emailCC = "";
            this.emailSuccessFieldEntry.emailFrom = "";
            this.emailSuccessFieldEntry.emailSubject = "";
            this.emailSuccessFieldEntry.emailTo = "";
            this.emailSuccessFieldEntry.Location = new System.Drawing.Point(3, 16);
            this.emailSuccessFieldEntry.MinimumSize = new System.Drawing.Size(405, 191);
            this.emailSuccessFieldEntry.Name = "emailSuccessFieldEntry";
            this.emailSuccessFieldEntry.Size = new System.Drawing.Size(411, 196);
            this.emailSuccessFieldEntry.TabIndex = 0;
            // 
            // emailFailureFieldEntry
            // 
            this.emailFailureFieldEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailFailureFieldEntry.emailBody = "";
            this.emailFailureFieldEntry.emailCC = "";
            this.emailFailureFieldEntry.emailFrom = "";
            this.emailFailureFieldEntry.emailSubject = "";
            this.emailFailureFieldEntry.emailTo = "";
            this.emailFailureFieldEntry.Location = new System.Drawing.Point(3, 16);
            this.emailFailureFieldEntry.MinimumSize = new System.Drawing.Size(405, 191);
            this.emailFailureFieldEntry.Name = "emailFailureFieldEntry";
            this.emailFailureFieldEntry.Size = new System.Drawing.Size(411, 196);
            this.emailFailureFieldEntry.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraGroupBox2.Controls.Add(this.emailFailureFieldEntry);
            this.ultraGroupBox2.Location = new System.Drawing.Point(4, 225);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(417, 215);
            this.ultraGroupBox2.TabIndex = 1;
            this.ultraGroupBox2.Text = "On Failure:";
            // 
            // EmailTaskList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ultraGroupBox2);
            this.Controls.Add(this.ultraGroupBox1);
            this.Name = "EmailTaskList";
            this.Size = new System.Drawing.Size(426, 444);
            this.Load += new System.EventHandler(this.EmailTaskList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private EmailFieldEntry emailSuccessFieldEntry;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private EmailFieldEntry emailFailureFieldEntry;
    }
}
