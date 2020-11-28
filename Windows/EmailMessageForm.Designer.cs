namespace MIDRetail.Windows
{
    partial class EmailMessageForm
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
            this.emailMessageControl1 = new MIDRetail.Windows.Controls.EmailMessageControl();
            this.SuspendLayout();
            // 
            // emailMessageControl1
            // 
            this.emailMessageControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emailMessageControl1.Location = new System.Drawing.Point(0, 0);
            this.emailMessageControl1.MinimumSize = new System.Drawing.Size(425, 314);
            this.emailMessageControl1.Name = "emailMessageControl1";
            this.emailMessageControl1.Size = new System.Drawing.Size(425, 314);
            this.emailMessageControl1.TabIndex = 0;
            // 
            // EmailMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 311);
            this.Controls.Add(this.emailMessageControl1);
            this.Name = "EmailMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Email Message";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.EmailMessageControl emailMessageControl1;
    }
}