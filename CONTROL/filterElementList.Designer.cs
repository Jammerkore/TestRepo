namespace MIDRetail.Windows.Controls
{
    partial class filterElementList
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
            this.midSelectMultiNodeControl1 = new MIDRetail.Windows.Controls.MIDSelectMultiNodeControl();
            this.SuspendLayout();
            // 
            // midSelectMultiNodeControl1
            // 
            this.midSelectMultiNodeControl1.CheckAllByDefault = false;
            this.midSelectMultiNodeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.midSelectMultiNodeControl1.FieldToDisplay = "";
            this.midSelectMultiNodeControl1.FieldToTag = "";
            this.midSelectMultiNodeControl1.Location = new System.Drawing.Point(0, 0);
            this.midSelectMultiNodeControl1.MappingRelationshipColumnKey = "";
            this.midSelectMultiNodeControl1.Name = "midSelectMultiNodeControl1";
            this.midSelectMultiNodeControl1.ShowRootLines = false;
            this.midSelectMultiNodeControl1.Size = new System.Drawing.Size(300, 333);
            this.midSelectMultiNodeControl1.TabIndex = 1;
            this.midSelectMultiNodeControl1.Title = "Store Status";
            // 
            // filterElementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.midSelectMultiNodeControl1);
            this.Name = "filterElementList";
            this.Size = new System.Drawing.Size(300, 333);
            this.ResumeLayout(false);

        }

        #endregion

        private MIDSelectMultiNodeControl midSelectMultiNodeControl1;



    }
}
