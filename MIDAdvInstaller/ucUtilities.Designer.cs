namespace MIDRetailInstaller
{
    partial class ucUtilities
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
            this.rdoDatabaseMaintenance = new System.Windows.Forms.RadioButton();
            this.rdoStartServices = new System.Windows.Forms.RadioButton();
            this.rdoStopServices = new System.Windows.Forms.RadioButton();
            this.rdoRescan = new System.Windows.Forms.RadioButton();
            this.rdoEventSource = new System.Windows.Forms.RadioButton();
            this.rdoCrystalReports = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // rdoDatabaseMaintenance
            // 
            this.rdoDatabaseMaintenance.AutoSize = true;
            this.rdoDatabaseMaintenance.Location = new System.Drawing.Point(252, 63);
            this.rdoDatabaseMaintenance.Name = "rdoDatabaseMaintenance";
            this.rdoDatabaseMaintenance.Size = new System.Drawing.Size(136, 17);
            this.rdoDatabaseMaintenance.TabIndex = 0;
            this.rdoDatabaseMaintenance.TabStop = true;
            this.rdoDatabaseMaintenance.Text = "Database Maintenance";
            this.rdoDatabaseMaintenance.UseVisualStyleBackColor = true;
            this.rdoDatabaseMaintenance.CheckedChanged += new System.EventHandler(this.rdoDatabaseMaintenance_CheckedChanged);
            // 
            // rdoStartServices
            // 
            this.rdoStartServices.AutoSize = true;
            this.rdoStartServices.Location = new System.Drawing.Point(252, 104);
            this.rdoStartServices.Name = "rdoStartServices";
            this.rdoStartServices.Size = new System.Drawing.Size(91, 17);
            this.rdoStartServices.TabIndex = 1;
            this.rdoStartServices.TabStop = true;
            this.rdoStartServices.Text = "Start Services";
            this.rdoStartServices.UseVisualStyleBackColor = true;
            this.rdoStartServices.CheckedChanged += new System.EventHandler(this.rdoStartServices_CheckedChanged);
            // 
            // rdoStopServices
            // 
            this.rdoStopServices.AutoSize = true;
            this.rdoStopServices.Location = new System.Drawing.Point(252, 144);
            this.rdoStopServices.Name = "rdoStopServices";
            this.rdoStopServices.Size = new System.Drawing.Size(91, 17);
            this.rdoStopServices.TabIndex = 2;
            this.rdoStopServices.TabStop = true;
            this.rdoStopServices.Text = "Stop Services";
            this.rdoStopServices.UseVisualStyleBackColor = true;
            this.rdoStopServices.CheckedChanged += new System.EventHandler(this.rdoStopServices_CheckedChanged);
            // 
            // rdoRescan
            // 
            this.rdoRescan.AutoSize = true;
            this.rdoRescan.Location = new System.Drawing.Point(252, 183);
            this.rdoRescan.Name = "rdoRescan";
            this.rdoRescan.Size = new System.Drawing.Size(229, 17);
            this.rdoRescan.TabIndex = 15;
            this.rdoRescan.Text = "Rescan for previously installed components";
            this.rdoRescan.UseVisualStyleBackColor = true;
            this.rdoRescan.CheckedChanged += new System.EventHandler(this.rdoRescan_CheckedChanged);
            // 
            // rdoEventSource
            // 
            this.rdoEventSource.AutoSize = true;
            this.rdoEventSource.Location = new System.Drawing.Point(252, 222);
            this.rdoEventSource.Name = "rdoEventSource";
            this.rdoEventSource.Size = new System.Drawing.Size(152, 17);
            this.rdoEventSource.TabIndex = 16;
            this.rdoEventSource.Text = "Add Event Viewer Sources";
            this.rdoEventSource.UseVisualStyleBackColor = true;
            this.rdoEventSource.CheckedChanged += new System.EventHandler(this.rdoEventSource_CheckedChanged);
            // 
            // rdoCrystalReports
            // 
            this.rdoCrystalReports.AutoSize = true;
            this.rdoCrystalReports.Location = new System.Drawing.Point(252, 261);
            this.rdoCrystalReports.Name = "rdoCrystalReports";
            this.rdoCrystalReports.Size = new System.Drawing.Size(126, 17);
            this.rdoCrystalReports.TabIndex = 17;
            this.rdoCrystalReports.Text = "Install Crystal Reports";
            this.rdoCrystalReports.UseVisualStyleBackColor = true;
            this.rdoCrystalReports.CheckedChanged += new System.EventHandler(this.rdoCrystalReports_CheckedChanged);
            // 
            // ucUtilities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoCrystalReports);
            this.Controls.Add(this.rdoEventSource);
            this.Controls.Add(this.rdoRescan);
            this.Controls.Add(this.rdoStopServices);
            this.Controls.Add(this.rdoStartServices);
            this.Controls.Add(this.rdoDatabaseMaintenance);
            this.Name = "ucUtilities";
            this.Size = new System.Drawing.Size(680, 435);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoDatabaseMaintenance;
        private System.Windows.Forms.RadioButton rdoStartServices;
        private System.Windows.Forms.RadioButton rdoStopServices;
        private System.Windows.Forms.RadioButton rdoRescan;
        private System.Windows.Forms.RadioButton rdoEventSource;
        private System.Windows.Forms.RadioButton rdoCrystalReports;
    }
}
