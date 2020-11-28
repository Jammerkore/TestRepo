namespace MIDRetailInstaller
{
    partial class ucInstallChooser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucInstallChooser));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoUpgradeAll = new System.Windows.Forms.RadioButton();
            this.rdoConfigure = new System.Windows.Forms.RadioButton();
            this.rdoUtilities = new System.Windows.Forms.RadioButton();
            this.rdoServer = new System.Windows.Forms.RadioButton();
            this.rdoClient = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rdoUpgradeAll);
            this.groupBox1.Controls.Add(this.rdoConfigure);
            this.groupBox1.Controls.Add(this.rdoUtilities);
            this.groupBox1.Controls.Add(this.rdoServer);
            this.groupBox1.Controls.Add(this.rdoClient);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Location = new System.Drawing.Point(5, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(680, 435);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // rdoUpgradeAll
            // 
            this.rdoUpgradeAll.AutoSize = true;
            this.rdoUpgradeAll.Location = new System.Drawing.Point(373, 69);
            this.rdoUpgradeAll.Name = "rdoUpgradeAll";
            this.rdoUpgradeAll.Size = new System.Drawing.Size(142, 17);
            this.rdoUpgradeAll.TabIndex = 15;
            this.rdoUpgradeAll.Text = "Upgrade All Components";
            this.rdoUpgradeAll.UseVisualStyleBackColor = true;
            this.rdoUpgradeAll.CheckedChanged += new System.EventHandler(this.rdoUpgradeAll_CheckedChanged);
            // 
            // rdoConfigure
            // 
            this.rdoConfigure.AutoSize = true;
            this.rdoConfigure.Location = new System.Drawing.Point(373, 246);
            this.rdoConfigure.Name = "rdoConfigure";
            this.rdoConfigure.Size = new System.Drawing.Size(70, 17);
            this.rdoConfigure.TabIndex = 14;
            this.rdoConfigure.Text = "Configure";
            this.rdoConfigure.UseVisualStyleBackColor = true;
            this.rdoConfigure.CheckedChanged += new System.EventHandler(this.rdoConfigure_CheckedChanged);
            // 
            // rdoUtilities
            // 
            this.rdoUtilities.AutoSize = true;
            this.rdoUtilities.Location = new System.Drawing.Point(373, 305);
            this.rdoUtilities.Name = "rdoUtilities";
            this.rdoUtilities.Size = new System.Drawing.Size(58, 17);
            this.rdoUtilities.TabIndex = 13;
            this.rdoUtilities.Text = "Utilities";
            this.rdoUtilities.UseVisualStyleBackColor = true;
            this.rdoUtilities.CheckedChanged += new System.EventHandler(this.rdoUtilities_CheckedChanged);
            // 
            // rdoServer
            // 
            this.rdoServer.AutoSize = true;
            this.rdoServer.Location = new System.Drawing.Point(373, 187);
            this.rdoServer.Name = "rdoServer";
            this.rdoServer.Size = new System.Drawing.Size(56, 17);
            this.rdoServer.TabIndex = 12;
            this.rdoServer.Text = "Server";
            this.rdoServer.UseVisualStyleBackColor = true;
            this.rdoServer.CheckedChanged += new System.EventHandler(this.rdoServer_CheckedChanged);
            // 
            // rdoClient
            // 
            this.rdoClient.AutoSize = true;
            this.rdoClient.Checked = true;
            this.rdoClient.Location = new System.Drawing.Point(373, 128);
            this.rdoClient.Name = "rdoClient";
            this.rdoClient.Size = new System.Drawing.Size(51, 17);
            this.rdoClient.TabIndex = 11;
            this.rdoClient.TabStop = true;
            this.rdoClient.Text = "Client";
            this.rdoClient.UseVisualStyleBackColor = true;
            this.rdoClient.CheckedChanged += new System.EventHandler(this.rdoClient_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(9, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(301, 345);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // ucInstallChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ucInstallChooser";
            this.Size = new System.Drawing.Size(680, 435);
            this.VisibleChanged += new System.EventHandler(this.ucInstallChooser_VisibleChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoUtilities;
        private System.Windows.Forms.RadioButton rdoServer;
        private System.Windows.Forms.RadioButton rdoClient;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rdoConfigure;
        private System.Windows.Forms.RadioButton rdoUpgradeAll;

    }
}
