namespace MIDRetail.Windows.Controls
{
    partial class MIDMultiColumnDropDown
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MIDMultiColumnDropDown));
            this.pnlDropDown = new System.Windows.Forms.Panel();
            this.pnlHeaders = new System.Windows.Forms.Panel();
            this.btnDrop = new System.Windows.Forms.Button();
            this.grdList = new System.Windows.Forms.DataGridView();
            this.pnlDropDown.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDropDown
            // 
            this.pnlDropDown.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDropDown.Controls.Add(this.pnlHeaders);
            this.pnlDropDown.Controls.Add(this.btnDrop);
            this.pnlDropDown.Location = new System.Drawing.Point(0, 0);
            this.pnlDropDown.Name = "pnlDropDown";
            this.pnlDropDown.Size = new System.Drawing.Size(199, 40);
            this.pnlDropDown.TabIndex = 1;
            this.pnlDropDown.Resize += new System.EventHandler(this.pnlDropDown_Resize);
            // 
            // pnlHeaders
            // 
            this.pnlHeaders.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pnlHeaders.Location = new System.Drawing.Point(0, 0);
            this.pnlHeaders.Name = "pnlHeaders";
            this.pnlHeaders.Size = new System.Drawing.Size(196, 16);
            this.pnlHeaders.TabIndex = 3;
            // 
            // btnDrop
            // 
            this.btnDrop.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrop.Image = ((System.Drawing.Image)(resources.GetObject("btnDrop.Image")));
            this.btnDrop.Location = new System.Drawing.Point(176, 17);
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.Size = new System.Drawing.Size(18, 18);
            this.btnDrop.TabIndex = 2;
            this.btnDrop.UseVisualStyleBackColor = false;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            // 
            // grdList
            // 
            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToResizeColumns = false;
            this.grdList.AllowUserToResizeRows = false;
            this.grdList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.grdList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdList.EnableHeadersVisualStyles = false;
            this.grdList.Location = new System.Drawing.Point(0, 40);
            this.grdList.MultiSelect = false;
            this.grdList.Name = "grdList";
            this.grdList.RowHeadersVisible = false;
            this.grdList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdList.Size = new System.Drawing.Size(199, 170);
            this.grdList.TabIndex = 2;
            this.grdList.SelectionChanged += new System.EventHandler(this.grdList_SelectionChanged);
            // 
            // MIDMultiColumnDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.grdList);
            this.Controls.Add(this.pnlDropDown);
            this.Name = "MIDMultiColumnDropDown";
            this.Size = new System.Drawing.Size(199, 40);
            this.Resize += new System.EventHandler(this.MIDMultiColumnDropDown_Resize);
            this.pnlDropDown.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdList)).EndInit();
            this.ResumeLayout(false);
            this.Load += new System.EventHandler(this.MIDMultiColumnDropDown_Load);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDropDown;
        private System.Windows.Forms.Button btnDrop;
        private System.Windows.Forms.DataGridView grdList;
        private System.Windows.Forms.Panel pnlHeaders;
    }
}
