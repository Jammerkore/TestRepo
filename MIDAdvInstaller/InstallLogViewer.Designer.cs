namespace MIDRetailInstaller
{
    partial class InstallLogViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallLogViewer));
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.saveInstallLog = new System.Windows.Forms.SaveFileDialog();
            this.gridInstallLog = new System.Windows.Forms.DataGridView();
            this.errorType = new System.Windows.Forms.DataGridViewImageColumn();
            this.ErrorMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeHidden = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imlErrorTypes = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridInstallLog)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(408, 239);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(327, 239);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "Export ...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // gridInstallLog
            // 
            this.gridInstallLog.AllowUserToAddRows = false;
            this.gridInstallLog.AllowUserToDeleteRows = false;
            this.gridInstallLog.AllowUserToResizeColumns = false;
            this.gridInstallLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gridInstallLog.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridInstallLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridInstallLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridInstallLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.errorType,
            this.ErrorMessage,
            this.TypeHidden});
            this.gridInstallLog.GridColor = System.Drawing.SystemColors.Window;
            this.gridInstallLog.Location = new System.Drawing.Point(8, 8);
            this.gridInstallLog.Name = "gridInstallLog";
            this.gridInstallLog.RowHeadersVisible = false;
            this.gridInstallLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridInstallLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInstallLog.ShowEditingIcon = false;
            this.gridInstallLog.Size = new System.Drawing.Size(475, 225);
            this.gridInstallLog.TabIndex = 3;
            // 
            // errorType
            // 
            this.errorType.HeaderText = "Type";
            this.errorType.Name = "errorType";
            this.errorType.Width = 50;
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.HeaderText = "Message";
            this.ErrorMessage.MinimumWidth = 200;
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ErrorMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ErrorMessage.Width = 400;
            // 
            // TypeHidden
            // 
            this.TypeHidden.HeaderText = "TypeHidden";
            this.TypeHidden.Name = "TypeHidden";
            this.TypeHidden.Visible = false;
            // 
            // imlErrorTypes
            // 
            this.imlErrorTypes.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlErrorTypes.ImageStream")));
            this.imlErrorTypes.TransparentColor = System.Drawing.Color.Transparent;
            this.imlErrorTypes.Images.SetKeyName(0, "Error16_TransBck.ico");
            this.imlErrorTypes.Images.SetKeyName(1, "Warning16_TransBck.ico");
            this.imlErrorTypes.Images.SetKeyName(2, "Messages16_TransBck.ico");
            this.imlErrorTypes.Images.SetKeyName(3, "magnifyingGlass.bmp");
            // 
            // InstallLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(492, 274);
            this.Controls.Add(this.gridInstallLog);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "InstallLogViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Install Log Viewer";
            this.Load += new System.EventHandler(this.InstallLogViewer_Load);
            this.Resize += new System.EventHandler(this.InstallLogViewer_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.gridInstallLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveInstallLog;
        private System.Windows.Forms.DataGridView gridInstallLog;
        private System.Windows.Forms.ImageList imlErrorTypes;
        private System.Windows.Forms.DataGridViewImageColumn errorType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorMessage;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeHidden;
    }
}