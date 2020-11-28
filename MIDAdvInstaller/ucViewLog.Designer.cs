namespace MIDRetailInstaller
{
    partial class ucInstallationLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucInstallationLog));
            this.grpInstallDetails = new System.Windows.Forms.GroupBox();
            this.btnViewLog = new System.Windows.Forms.Button();
            this.lblMessages = new System.Windows.Forms.Label();
            this.picMessages = new System.Windows.Forms.PictureBox();
            this.lblWarnings = new System.Windows.Forms.Label();
            this.picWarnings = new System.Windows.Forms.PictureBox();
            this.lblErrors = new System.Windows.Forms.Label();
            this.picErrors = new System.Windows.Forms.PictureBox();
            this.grpInstallDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMessages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErrors)).BeginInit();
            this.SuspendLayout();
            // 
            // grpInstallDetails
            // 
            this.grpInstallDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpInstallDetails.Controls.Add(this.btnViewLog);
            this.grpInstallDetails.Controls.Add(this.lblMessages);
            this.grpInstallDetails.Controls.Add(this.picMessages);
            this.grpInstallDetails.Controls.Add(this.lblWarnings);
            this.grpInstallDetails.Controls.Add(this.picWarnings);
            this.grpInstallDetails.Controls.Add(this.lblErrors);
            this.grpInstallDetails.Controls.Add(this.picErrors);
            this.grpInstallDetails.Location = new System.Drawing.Point(0, 0);
            this.grpInstallDetails.Name = "grpInstallDetails";
            this.grpInstallDetails.Size = new System.Drawing.Size(663, 53);
            this.grpInstallDetails.TabIndex = 7;
            this.grpInstallDetails.TabStop = false;
            this.grpInstallDetails.Text = "Installation Details";
            // 
            // btnViewLog
            // 
            this.btnViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewLog.Location = new System.Drawing.Point(565, 19);
            this.btnViewLog.Name = "btnViewLog";
            this.btnViewLog.Size = new System.Drawing.Size(88, 23);
            this.btnViewLog.TabIndex = 10;
            this.btnViewLog.Text = "View Log ...";
            this.btnViewLog.UseVisualStyleBackColor = true;
            this.btnViewLog.Click += new System.EventHandler(this.btnViewLog_Click);
            // 
            // lblMessages
            // 
            this.lblMessages.AutoSize = true;
            this.lblMessages.Location = new System.Drawing.Point(232, 22);
            this.lblMessages.Name = "lblMessages";
            this.lblMessages.Size = new System.Drawing.Size(64, 13);
            this.lblMessages.TabIndex = 9;
            this.lblMessages.Text = "0 Messages";
            // 
            // picMessages
            // 
            this.picMessages.Image = ((System.Drawing.Image)(resources.GetObject("picMessages.Image")));
            this.picMessages.Location = new System.Drawing.Point(208, 19);
            this.picMessages.Name = "picMessages";
            this.picMessages.Size = new System.Drawing.Size(18, 18);
            this.picMessages.TabIndex = 8;
            this.picMessages.TabStop = false;
            // 
            // lblWarnings
            // 
            this.lblWarnings.AutoSize = true;
            this.lblWarnings.Location = new System.Drawing.Point(123, 22);
            this.lblWarnings.Name = "lblWarnings";
            this.lblWarnings.Size = new System.Drawing.Size(61, 13);
            this.lblWarnings.TabIndex = 7;
            this.lblWarnings.Text = "0 Warnings";
            // 
            // picWarnings
            // 
            this.picWarnings.Image = ((System.Drawing.Image)(resources.GetObject("picWarnings.Image")));
            this.picWarnings.Location = new System.Drawing.Point(99, 19);
            this.picWarnings.Name = "picWarnings";
            this.picWarnings.Size = new System.Drawing.Size(18, 18);
            this.picWarnings.TabIndex = 6;
            this.picWarnings.TabStop = false;
            // 
            // lblErrors
            // 
            this.lblErrors.AutoSize = true;
            this.lblErrors.Location = new System.Drawing.Point(32, 22);
            this.lblErrors.Name = "lblErrors";
            this.lblErrors.Size = new System.Drawing.Size(43, 13);
            this.lblErrors.TabIndex = 5;
            this.lblErrors.Text = "0 Errors";
            // 
            // picErrors
            // 
            this.picErrors.Image = ((System.Drawing.Image)(resources.GetObject("picErrors.Image")));
            this.picErrors.Location = new System.Drawing.Point(8, 19);
            this.picErrors.Name = "picErrors";
            this.picErrors.Size = new System.Drawing.Size(18, 18);
            this.picErrors.TabIndex = 2;
            this.picErrors.TabStop = false;
            // 
            // ucInstallationLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpInstallDetails);
            this.Name = "ucInstallationLog";
            this.Size = new System.Drawing.Size(665, 56);
            this.Load += new System.EventHandler(this.ucInstallationLog_Load);
            this.grpInstallDetails.ResumeLayout(false);
            this.grpInstallDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMessages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWarnings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picErrors)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpInstallDetails;
        private System.Windows.Forms.Button btnViewLog;
        private System.Windows.Forms.Label lblMessages;
        private System.Windows.Forms.PictureBox picMessages;
        private System.Windows.Forms.Label lblWarnings;
        private System.Windows.Forms.PictureBox picWarnings;
        private System.Windows.Forms.Label lblErrors;
        private System.Windows.Forms.PictureBox picErrors;

    }
}
