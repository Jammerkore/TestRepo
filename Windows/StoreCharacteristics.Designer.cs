namespace MIDRetail.Windows
{
    partial class StoreCharacteristics
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
            this.charControl = new MIDRetail.Windows.Controls.CharacteristicMaintControl();
            this.SuspendLayout();
            // 
            // charControl
            // 
            this.charControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.charControl.Location = new System.Drawing.Point(0, 0);
            this.charControl.Name = "charControl";
            this.charControl.Size = new System.Drawing.Size(608, 414);
            this.charControl.TabIndex = 0;
            // 
            // StoreCharacteristics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 414);
            this.Controls.Add(this.charControl);
            this.Name = "StoreCharacteristics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Store Characteristics";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.CharacteristicMaintControl charControl;
    }
}