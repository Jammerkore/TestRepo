namespace MIDRetailInstaller
{
    partial class Config_Text
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
            this.lblLabel = new System.Windows.Forms.Label();
            this.txtConfig_Text = new System.Windows.Forms.TextBox();
            this.lblLabelBold = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLabel
            // 
            this.lblLabel.AutoEllipsis = true;
            this.lblLabel.Location = new System.Drawing.Point(23, 5);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(165, 16);
            this.lblLabel.TabIndex = 6;
            this.lblLabel.Text = "Config_Text_Label";
            this.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabel.Click += new System.EventHandler(this.lblLabel_Click);
            this.lblLabel.LostFocus += new System.EventHandler(this.Config_Text_LostFocus);
            // 
            // txtConfig_Text
            // 
            this.txtConfig_Text.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfig_Text.Location = new System.Drawing.Point(195, 0);
            this.txtConfig_Text.Multiline = true;
            this.txtConfig_Text.Name = "txtConfig_Text";
            this.txtConfig_Text.Size = new System.Drawing.Size(377, 22);
            this.txtConfig_Text.TabIndex = 7;
            this.txtConfig_Text.Click += new System.EventHandler(this.txtConfig_Text_Click);
            this.txtConfig_Text.TextChanged += new System.EventHandler(this.txtConfig_Text_TextChanged);
            this.txtConfig_Text.LostFocus += new System.EventHandler(this.Config_Text_LostFocus);
            // 
            // lblLabelBold
            // 
            this.lblLabelBold.AutoEllipsis = true;
            this.lblLabelBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBold.Location = new System.Drawing.Point(23, 5);
            this.lblLabelBold.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLabelBold.Name = "lblLabelBold";
            this.lblLabelBold.Size = new System.Drawing.Size(165, 16);
            this.lblLabelBold.TabIndex = 8;
            this.lblLabelBold.Text = "Config_Text_Label";
            this.lblLabelBold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabelBold.Visible = false;
            this.lblLabelBold.LostFocus += new System.EventHandler(this.Config_Text_LostFocus);
            // 
            // Config_Text
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblLabelBold);
            this.Controls.Add(this.txtConfig_Text);
            this.Controls.Add(this.lblLabel);
            this.Name = "Config_Text";
            this.Size = new System.Drawing.Size(573, 26);
            this.Click += new System.EventHandler(this.Config_TextClick);
            this.GotFocus += new System.EventHandler(this.Config_Text_GotFocus);
            this.LostFocus += new System.EventHandler(this.Config_Text_LostFocus);
            this.Controls.SetChildIndex(this.lblLabel, 0);
            this.Controls.SetChildIndex(this.txtConfig_Text, 0);
            this.Controls.SetChildIndex(this.lblLabelBold, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.TextBox txtConfig_Text;
        private System.Windows.Forms.Label lblLabelBold;

    }
}
