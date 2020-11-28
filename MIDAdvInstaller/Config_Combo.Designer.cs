namespace MIDRetailInstaller
{
    partial class Config_Combo
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
            this.cboConfig_Combo = new System.Windows.Forms.ComboBox();
            this.lblLabel = new System.Windows.Forms.Label();
            this.lblLabelBold = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            // 
            // cboConfig_Combo
            // 
            this.cboConfig_Combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboConfig_Combo.FormattingEnabled = true;
            this.cboConfig_Combo.Location = new System.Drawing.Point(195, 0);
            this.cboConfig_Combo.Name = "cboConfig_Combo";
            this.cboConfig_Combo.Size = new System.Drawing.Size(377, 21);
            this.cboConfig_Combo.TabIndex = 0;
            this.cboConfig_Combo.SelectedIndexChanged += new System.EventHandler(this.cboConfig_Combo_SelectedIndexChanged);
            this.cboConfig_Combo.TextChanged += new System.EventHandler(this.cboConfig_Combo_SelectedIndexChanged);
            this.cboConfig_Combo.Click += new System.EventHandler(this.cboConfig_ComboClick);
            this.cboConfig_Combo.LostFocus += new System.EventHandler(this.Config_Combo_LostFocus);
            // 
            // lblLabel
            // 
            this.lblLabel.AutoEllipsis = true;
            this.lblLabel.Location = new System.Drawing.Point(23, 5);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(165, 16);
            this.lblLabel.TabIndex = 6;
            this.lblLabel.Text = "Config_Combo_Label";
            this.lblLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblLabel.Click += new System.EventHandler(this.lblLabel_Click);
            this.lblLabel.LostFocus += new System.EventHandler(this.Config_Combo_LostFocus);
            this.lblLabel.Resize += new System.EventHandler(this.lblLabel_Resize);
            // 
            // lblLabelBold
            // 
            this.lblLabelBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBold.Location = new System.Drawing.Point(23, 5);
            this.lblLabelBold.Name = "lblLabelBold";
            this.lblLabelBold.Size = new System.Drawing.Size(165, 16);
            this.lblLabelBold.TabIndex = 7;
            this.lblLabelBold.Text = "Config_Combo_Label";
            this.lblLabelBold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabelBold.Visible = false;
            this.lblLabelBold.Resize += new System.EventHandler(this.lblLabelBold_Resize);
            // 
            // Config_Combo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblLabelBold);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.cboConfig_Combo);
            this.Name = "Config_Combo";
            this.Size = new System.Drawing.Size(573, 26);
            this.GotFocus += new System.EventHandler(this.Config_Combo_GotFocus);
            this.LostFocus += new System.EventHandler(this.Config_Combo_LostFocus);
            this.Controls.SetChildIndex(this.cboConfig_Combo, 0);
            this.Controls.SetChildIndex(this.lblLabel, 0);
            this.Controls.SetChildIndex(this.lblLabelBold, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboConfig_Combo;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.Label lblLabelBold;

    }
}
