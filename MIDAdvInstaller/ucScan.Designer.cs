namespace MIDRetailInstaller
{
    partial class ucScan
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucScan));
            this.grpScanning = new System.Windows.Forms.GroupBox();
            this.txtInstruction = new System.Windows.Forms.TextBox();
            this.tvInstalledComponents = new System.Windows.Forms.TreeView();
            this.cmInstalledCompTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deselectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblInstalledComponents = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.grpScanning.SuspendLayout();
            this.cmInstalledCompTree.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpScanning
            // 
            this.grpScanning.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpScanning.Controls.Add(this.txtInstruction);
            this.grpScanning.Controls.Add(this.tvInstalledComponents);
            this.grpScanning.Controls.Add(this.lblInstalledComponents);
            this.grpScanning.Controls.Add(this.btnScan);
            this.grpScanning.Location = new System.Drawing.Point(5, 0);
            this.grpScanning.Name = "grpScanning";
            this.grpScanning.Size = new System.Drawing.Size(670, 431);
            this.grpScanning.TabIndex = 0;
            this.grpScanning.TabStop = false;
            this.grpScanning.Text = "Computer Scan";
            // 
            // txtInstruction
            // 
            this.txtInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInstruction.BackColor = System.Drawing.SystemColors.Control;
            this.txtInstruction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInstruction.Location = new System.Drawing.Point(9, 17);
            this.txtInstruction.Multiline = true;
            this.txtInstruction.Name = "txtInstruction";
            this.txtInstruction.Size = new System.Drawing.Size(655, 44);
            this.txtInstruction.TabIndex = 7;
            this.txtInstruction.Text = resources.GetString("txtInstruction.Text");
            // 
            // tvInstalledComponents
            // 
            this.tvInstalledComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvInstalledComponents.CheckBoxes = true;
            this.tvInstalledComponents.ContextMenuStrip = this.cmInstalledCompTree;
            this.tvInstalledComponents.Location = new System.Drawing.Point(8, 96);
            this.tvInstalledComponents.Name = "tvInstalledComponents";
            this.tvInstalledComponents.Size = new System.Drawing.Size(655, 316);
            this.tvInstalledComponents.TabIndex = 6;
            this.tvInstalledComponents.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvInstalledComponents_AfterCheck);
            // 
            // cmInstalledCompTree
            // 
            this.cmInstalledCompTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deselectAllToolStripMenuItem,
            this.selectAllToolStripMenuItem});
            this.cmInstalledCompTree.Name = "cmInstalledCompTree";
            this.cmInstalledCompTree.Size = new System.Drawing.Size(136, 48);
            this.cmInstalledCompTree.Opening += new System.ComponentModel.CancelEventHandler(this.cmInstalledCompTree_Opening);
            // 
            // deselectAllToolStripMenuItem
            // 
            this.deselectAllToolStripMenuItem.Name = "deselectAllToolStripMenuItem";
            this.deselectAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.deselectAllToolStripMenuItem.Text = "Deselect All";
            this.deselectAllToolStripMenuItem.Click += new System.EventHandler(this.deselectAllToolStripMenuItem_Click);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
            // 
            // lblInstalledComponents
            // 
            this.lblInstalledComponents.AutoSize = true;
            this.lblInstalledComponents.Location = new System.Drawing.Point(6, 77);
            this.lblInstalledComponents.Name = "lblInstalledComponents";
            this.lblInstalledComponents.Size = new System.Drawing.Size(108, 13);
            this.lblInstalledComponents.TabIndex = 5;
            this.lblInstalledComponents.Text = "Installed Components";
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(588, 67);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "Sc&an";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // ucScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.grpScanning);
            this.Name = "ucScan";
            this.Size = new System.Drawing.Size(680, 435);
            this.grpScanning.ResumeLayout(false);
            this.grpScanning.PerformLayout();
            this.cmInstalledCompTree.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpScanning;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Label lblInstalledComponents;
        private System.Windows.Forms.TreeView tvInstalledComponents;
        private System.Windows.Forms.ContextMenuStrip cmInstalledCompTree;
        private System.Windows.Forms.ToolStripMenuItem deselectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        public System.Windows.Forms.TextBox txtInstruction;
    }
}
