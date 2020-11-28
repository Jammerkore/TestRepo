namespace MIDRetailInstaller
{
    partial class Config_Numeric
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
            this.nmConfig_Numeric = new System.Windows.Forms.NumericUpDown();
            this.lblLabelBold = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConfig_Numeric)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLabel
            // 
            this.lblLabel.AutoEllipsis = true;
            this.lblLabel.Location = new System.Drawing.Point(23, 5);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(165, 16);
            this.lblLabel.TabIndex = 6;
            this.lblLabel.Text = "Config_Numeric_Label";
            this.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabel.Click += new System.EventHandler(this.lblLabel_Click);
            this.lblLabel.LostFocus += new System.EventHandler(this.Config_Numeric_LostFocus);
            // 
            // nmConfig_Numeric
            // 
            this.nmConfig_Numeric.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nmConfig_Numeric.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nmConfig_Numeric.Location = new System.Drawing.Point(195, 0);
            this.nmConfig_Numeric.Name = "nmConfig_Numeric";
            this.nmConfig_Numeric.Size = new System.Drawing.Size(377, 20);
            this.nmConfig_Numeric.TabIndex = 7;
            this.nmConfig_Numeric.ValueChanged += new System.EventHandler(this.nmConfig_Numeric_ValueChanged);
            this.nmConfig_Numeric.Click += new System.EventHandler(this.nmConfig_Numeric_Click);
            this.nmConfig_Numeric.LostFocus += new System.EventHandler(this.Config_Numeric_LostFocus);
            // 
            // lblLabelBold
            // 
            this.lblLabelBold.AutoEllipsis = true;
            this.lblLabelBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBold.Location = new System.Drawing.Point(23, 5);
            this.lblLabelBold.Name = "lblLabelBold";
            this.lblLabelBold.Size = new System.Drawing.Size(165, 16);
            this.lblLabelBold.TabIndex = 8;
            this.lblLabelBold.Text = "Config_Numeric_Label";
            this.lblLabelBold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabelBold.Visible = false;
            this.lblLabelBold.LostFocus += new System.EventHandler(this.Config_Numeric_LostFocus);
            // 
            // Config_Numeric
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblLabelBold);
            this.Controls.Add(this.nmConfig_Numeric);
            this.Controls.Add(this.lblLabel);
            this.Name = "Config_Numeric";
            this.Size = new System.Drawing.Size(573, 26);
            this.Click += new System.EventHandler(this.Config_NumericClick);
            this.GotFocus += new System.EventHandler(this.Config_Numeric_GotFocus);
            this.LostFocus += new System.EventHandler(this.Config_Numeric_LostFocus);
            this.Controls.SetChildIndex(this.lblLabel, 0);
            this.Controls.SetChildIndex(this.nmConfig_Numeric, 0);
            this.Controls.SetChildIndex(this.lblLabelBold, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmConfig_Numeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.NumericUpDown nmConfig_Numeric;
        private System.Windows.Forms.Label lblLabelBold;

    }
}
