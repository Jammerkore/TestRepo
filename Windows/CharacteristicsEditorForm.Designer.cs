namespace MIDRetail.Windows
{
    partial class CharacteristicsEditorForm
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
            this.gridFields = new MIDRetail.Windows.Controls.MIDGridFieldEditor();
            this.SuspendLayout();
            // 
            // gridFields
            // 
            this.gridFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridFields.Location = new System.Drawing.Point(0, 0);
            this.gridFields.Name = "gridFields";
            this.gridFields.Size = new System.Drawing.Size(533, 398);
            this.gridFields.TabIndex = 0;
            // 
            // CharacteristicsEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 398);
            this.Controls.Add(this.gridFields);
            this.Name = "CharacteristicsEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "StoreCharacteristicsGroupForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CharacteristicsEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.CharacteristicsEditorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MIDGridFieldEditor gridFields;
    }
}