namespace UnitTesting
{
    partial class UnitTest_AddForm
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
            this.unitTest_AddControl1 = new UnitTest_AddControl();
            this.SuspendLayout();
            // 
            // unitTest_AddControl1
            // 
            this.unitTest_AddControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unitTest_AddControl1.Location = new System.Drawing.Point(0, 0);
            this.unitTest_AddControl1.Name = "unitTest_AddControl1";
            this.unitTest_AddControl1.Size = new System.Drawing.Size(732, 602);
            this.unitTest_AddControl1.TabIndex = 0;
            // 
            // UnitTest_AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(732, 602);
            this.ControlBox = false;
            this.Controls.Add(this.unitTest_AddControl1);
            this.Name = "UnitTest_AddForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Unit Test";
            this.Load += new System.EventHandler(this.UnitTest_AddForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public UnitTest_AddControl unitTest_AddControl1;

    }
}