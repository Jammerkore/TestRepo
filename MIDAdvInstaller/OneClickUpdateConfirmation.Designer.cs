namespace MIDRetailInstaller
{
    partial class OncClickUpdateConfirmation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OncClickUpdateConfirmation));
            this.cbConfirmN = new System.Windows.Forms.CheckBox();
            this.btnTerminateN = new System.Windows.Forms.Button();
            this.btnContinueN = new System.Windows.Forms.Button();
            this.stop_picture = new System.Windows.Forms.PictureBox();
            this.lblWarningMessage = new System.Windows.Forms.Label();
            this.lblWarningMessageN2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.stop_picture)).BeginInit();
            this.SuspendLayout();
            // 
            // cbConfirmN
            // 
            this.cbConfirmN.AutoSize = true;
            this.cbConfirmN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbConfirmN.Location = new System.Drawing.Point(30, 207);
            this.cbConfirmN.Name = "cbConfirmN";
            this.cbConfirmN.Size = new System.Drawing.Size(15, 14);
            this.cbConfirmN.TabIndex = 8;
            this.cbConfirmN.UseVisualStyleBackColor = true;
            this.cbConfirmN.CheckedChanged += new System.EventHandler(this.cbConfirmN_CheckedChanged);
            // 
            // btnTerminateN
            // 
            this.btnTerminateN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTerminateN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTerminateN.Location = new System.Drawing.Point(214, 248);
            this.btnTerminateN.Name = "btnTerminateN";
            this.btnTerminateN.Size = new System.Drawing.Size(96, 45);
            this.btnTerminateN.TabIndex = 6;
            this.btnTerminateN.UseVisualStyleBackColor = true;
            this.btnTerminateN.Click += new System.EventHandler(this.btnTerminate_Click);
            // 
            // btnContinueN
            // 
            this.btnContinueN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnContinueN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContinueN.Location = new System.Drawing.Point(76, 248);
            this.btnContinueN.Name = "btnContinueN";
            this.btnContinueN.Size = new System.Drawing.Size(96, 45);
            this.btnContinueN.TabIndex = 9;
            this.btnContinueN.UseVisualStyleBackColor = true;
            this.btnContinueN.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // stop_picture
            // 
            this.stop_picture.Image = ((System.Drawing.Image)(resources.GetObject("stop_picture.Image")));
            this.stop_picture.Location = new System.Drawing.Point(12, 12);
            this.stop_picture.Name = "stop_picture";
            this.stop_picture.Size = new System.Drawing.Size(33, 33);
            this.stop_picture.TabIndex = 5;
            this.stop_picture.TabStop = false;
            // 
            // lblWarningMessage
            // 
            this.lblWarningMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarningMessage.Location = new System.Drawing.Point(51, 12);
            this.lblWarningMessage.Name = "lblWarningMessage";
            this.lblWarningMessage.Size = new System.Drawing.Size(326, 59);
            this.lblWarningMessage.TabIndex = 7;
            // 
            // lblWarningMessageN2
            // 
            this.lblWarningMessageN2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarningMessageN2.Location = new System.Drawing.Point(51, 73);
            this.lblWarningMessageN2.Name = "lblWarningMessageN2";
            this.lblWarningMessageN2.Size = new System.Drawing.Size(326, 120);
            this.lblWarningMessageN2.TabIndex = 11;
            // 
            // OncClickUpdateConfirmation
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(386, 307);
            this.ControlBox = false;
            this.Controls.Add(this.lblWarningMessageN2);
            this.Controls.Add(this.cbConfirmN);
            this.Controls.Add(this.btnTerminateN);
            this.Controls.Add(this.lblWarningMessage);
            this.Controls.Add(this.btnContinueN);
            this.Controls.Add(this.stop_picture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OncClickUpdateConfirmation";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "One-Click Upgrade Notification";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.stop_picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbConfirmN;
        private System.Windows.Forms.Button btnTerminateN;
        private System.Windows.Forms.Button btnContinueN;
        private System.Windows.Forms.PictureBox stop_picture;
        private System.Windows.Forms.Label lblWarningMessage;
        private System.Windows.Forms.Label lblWarningMessageN2;
    }
}