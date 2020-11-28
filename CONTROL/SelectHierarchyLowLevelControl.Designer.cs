namespace MIDRetail.Windows.Controls
{
    partial class SelectHierarchyLowLevelControl
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
            this.cboLowLevels = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.SuspendLayout();
            // 
            // cboLowLevels
            // 
            this.cboLowLevels.AutoAdjust = true;
            this.cboLowLevels.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboLowLevels.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboLowLevels.DataSource = null;
            this.cboLowLevels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboLowLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLowLevels.DropDownWidth = 240;
            this.cboLowLevels.FormattingEnabled = false;
            this.cboLowLevels.IgnoreFocusLost = false;
            this.cboLowLevels.ItemHeight = 13;
            this.cboLowLevels.Location = new System.Drawing.Point(0, 0);
            this.cboLowLevels.Margin = new System.Windows.Forms.Padding(0);
            this.cboLowLevels.MaxDropDownItems = 25;
            this.cboLowLevels.Name = "cboLowLevels";
            this.cboLowLevels.SetToolTip = "";
            this.cboLowLevels.Size = new System.Drawing.Size(240, 23);
            this.cboLowLevels.TabIndex = 0;
            this.cboLowLevels.Tag = null;
            // 
            // SelectHierarchyLowLevelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboLowLevels);
            this.Name = "SelectHierarchyLowLevelControl";
            this.Size = new System.Drawing.Size(240, 23);
            this.ResumeLayout(false);

        }

        #endregion

        private MIDComboBoxEnh cboLowLevels;
    }
}
