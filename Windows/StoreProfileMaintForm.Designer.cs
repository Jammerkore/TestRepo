namespace MIDRetail.Windows
{
    partial class StoreProfileMaintForm
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
            this.storeProfileMaintControl1 = new MIDRetail.Windows.Controls.StoreProfileMaintControl();
            this.SuspendLayout();
            // 
            // storeProfileMaintControl1
            // 
            this.storeProfileMaintControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.storeProfileMaintControl1.Location = new System.Drawing.Point(0, 0);
            this.storeProfileMaintControl1.Name = "storeProfileMaintControl1";
            this.storeProfileMaintControl1.Size = new System.Drawing.Size(796, 551);
            this.storeProfileMaintControl1.TabIndex = 0;
            // 
            // StoreProfileMaintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 551);
            this.Controls.Add(this.storeProfileMaintControl1);
            this.Name = "StoreProfileMaintForm";
            this.Text = "Store Profiles";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.StoreProfileMaintControl storeProfileMaintControl1;
    }
}