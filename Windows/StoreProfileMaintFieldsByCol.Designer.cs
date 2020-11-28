namespace MIDRetail.Windows
{
    partial class StoreProfileMaintFieldsByCol
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
            this.gridFieldsByCol = new MIDRetail.Windows.Controls.MIDGridFieldEditorColumns();
            this.SuspendLayout();
            // 
            // gridFieldsByCol
            // 
            this.gridFieldsByCol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFieldsByCol.Location = new System.Drawing.Point(0, 0);
            this.gridFieldsByCol.Name = "gridFieldsByCol";
            this.gridFieldsByCol.Size = new System.Drawing.Size(523, 511);
            this.gridFieldsByCol.TabIndex = 0;
            // 
            // StoreProfileMaintFieldsByCol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 511);
            this.Controls.Add(this.gridFieldsByCol);
            this.Name = "StoreProfileMaintFieldsByCol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Store Profiles - Fields [Edit]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StoreProfileMaintSingleStore_FormClosing);
            this.Load += new System.EventHandler(this.StoreProfileMaintSingleStore_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MIDGridFieldEditorColumns gridFieldsByCol;


    }
}