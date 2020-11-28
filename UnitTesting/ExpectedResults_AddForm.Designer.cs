namespace UnitTesting
{
    partial class ExpectedResults_AddForm
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
            this.expectedResult_AddControl1 = new UnitTesting.ExpectedResult_AddControl();
            this.SuspendLayout();
            // 
            // expectedResult_AddControl1
            // 
            this.expectedResult_AddControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.expectedResult_AddControl1.Location = new System.Drawing.Point(0, 0);
            this.expectedResult_AddControl1.Name = "expectedResult_AddControl1";
            this.expectedResult_AddControl1.Size = new System.Drawing.Size(388, 246);
            this.expectedResult_AddControl1.TabIndex = 0;
            // 
            // ExpectedResults_AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 246);
            this.ControlBox = false;
            this.Controls.Add(this.expectedResult_AddControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExpectedResults_AddForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Expected Result";
            this.Load += new System.EventHandler(this.ExpectedResults_AddForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public ExpectedResult_AddControl expectedResult_AddControl1;



    }
}