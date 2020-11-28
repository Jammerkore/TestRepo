namespace MIDRetailInstaller
{
    partial class Config_OpenFile
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
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.rectangleShape1 = new Microsoft.VisualBasic.PowerPacks.RectangleShape();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblConfig_File = new System.Windows.Forms.TextBox();
            this.btnConfig_File = new System.Windows.Forms.Button();
            this.openConfig_File = new System.Windows.Forms.OpenFileDialog();
            this.lblLabel = new System.Windows.Forms.Label();
            this.lblLabelBold = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, -1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.rectangleShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(573, 26);
            this.shapeContainer1.TabIndex = 3;
            this.shapeContainer1.TabStop = false;
            // 
            // rectangleShape1
            // 
            this.rectangleShape1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rectangleShape1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.rectangleShape1.FillColor = System.Drawing.Color.Transparent;
            this.rectangleShape1.Location = new System.Drawing.Point(195, 0);
            this.rectangleShape1.Name = "rectangleShape1";
            this.rectangleShape1.Size = new System.Drawing.Size(377, 22);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lblConfig_File);
            this.panel1.Location = new System.Drawing.Point(197, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 17);
            this.panel1.TabIndex = 4;
            // 
            // lblConfig_File
            // 
            this.lblConfig_File.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblConfig_File.BackColor = System.Drawing.Color.White;
            this.lblConfig_File.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblConfig_File.Location = new System.Drawing.Point(6, 3);
            this.lblConfig_File.Name = "lblConfig_File";
            this.lblConfig_File.Size = new System.Drawing.Size(343, 13);
            this.lblConfig_File.TabIndex = 3;
            this.lblConfig_File.Click += new System.EventHandler(this.lblConfig_File_Click);
            this.lblConfig_File.TextChanged += new System.EventHandler(this.lblConfig_File_TextChanged);
            this.lblConfig_File.LostFocus += new System.EventHandler(this.Config_OpenFile_LostFocus);
            // 
            // btnConfig_File
            // 
            this.btnConfig_File.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfig_File.Image = global::MIDRetail.MIDRetailInstaller.Properties.Resources.builder;
            this.btnConfig_File.Location = new System.Drawing.Point(550, 2);
            this.btnConfig_File.Name = "btnConfig_File";
            this.btnConfig_File.Size = new System.Drawing.Size(20, 19);
            this.btnConfig_File.TabIndex = 2;
            this.btnConfig_File.UseVisualStyleBackColor = true;
            this.btnConfig_File.Click += new System.EventHandler(this.btnConfig_File_Click);
            this.btnConfig_File.LostFocus += new System.EventHandler(this.Config_OpenFile_LostFocus);
            // 
            // lblLabel
            // 
            this.lblLabel.AutoEllipsis = true;
            this.lblLabel.Location = new System.Drawing.Point(23, 5);
            this.lblLabel.Name = "lblLabel";
            this.lblLabel.Size = new System.Drawing.Size(165, 16);
            this.lblLabel.TabIndex = 6;
            this.lblLabel.Text = "Config_File_Label";
            this.lblLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabel.Click += new System.EventHandler(this.lblLabel_Click);
            this.lblLabel.LostFocus += new System.EventHandler(this.Config_OpenFile_LostFocus);
            // 
            // lblLabelBold
            // 
            this.lblLabelBold.AutoEllipsis = true;
            this.lblLabelBold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLabelBold.Location = new System.Drawing.Point(23, 5);
            this.lblLabelBold.Name = "lblLabelBold";
            this.lblLabelBold.Size = new System.Drawing.Size(165, 16);
            this.lblLabelBold.TabIndex = 7;
            this.lblLabelBold.Text = "Config_File_Label";
            this.lblLabelBold.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLabelBold.Visible = false;
            this.lblLabelBold.LostFocus += new System.EventHandler(this.Config_OpenFile_LostFocus);
            // 
            // Config_OpenFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnConfig_File);
            this.Controls.Add(this.lblLabelBold);
            this.Controls.Add(this.lblLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.shapeContainer1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Config_OpenFile";
            this.Size = new System.Drawing.Size(573, 26);
            this.Click += new System.EventHandler(this.Config_FileClick);
            this.GotFocus += new System.EventHandler(this.Config_OpenFile_GotFocus);
            this.LostFocus += new System.EventHandler(this.Config_OpenFile_LostFocus);
            this.Controls.SetChildIndex(this.shapeContainer1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.lblLabel, 0);
            this.Controls.SetChildIndex(this.lblLabelBold, 0);
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.btnConfig_File, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.RectangleShape rectangleShape1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox lblConfig_File;
        private System.Windows.Forms.Button btnConfig_File;
        private System.Windows.Forms.OpenFileDialog openConfig_File;
        private System.Windows.Forms.Label lblLabel;
        private System.Windows.Forms.Label lblLabelBold;
    }
}
