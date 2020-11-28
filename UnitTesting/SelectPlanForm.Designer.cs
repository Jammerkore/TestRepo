namespace UnitTesting
{
    partial class SelectPlanForm
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
            this.SelectPlanForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radExistingPlan = new System.Windows.Forms.RadioButton();
            this.radNewPlan = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SelectPlanForm_Fill_Panel.ClientArea.SuspendLayout();
            this.SelectPlanForm_Fill_Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SelectPlanForm_Fill_Panel
            // 
            // 
            // SelectPlanForm_Fill_Panel.ClientArea
            // 
            this.SelectPlanForm_Fill_Panel.ClientArea.Controls.Add(this.panel2);
            this.SelectPlanForm_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.SelectPlanForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SelectPlanForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SelectPlanForm_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.SelectPlanForm_Fill_Panel.Name = "SelectPlanForm_Fill_Panel";
            this.SelectPlanForm_Fill_Panel.Size = new System.Drawing.Size(592, 254);
            this.SelectPlanForm_Fill_Panel.TabIndex = 8;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radExistingPlan);
            this.panel1.Controls.Add(this.radNewPlan);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 29);
            this.panel1.TabIndex = 0;
            // 
            // radExistingPlan
            // 
            this.radExistingPlan.AutoSize = true;
            this.radExistingPlan.Location = new System.Drawing.Point(89, 6);
            this.radExistingPlan.Name = "radExistingPlan";
            this.radExistingPlan.Size = new System.Drawing.Size(85, 17);
            this.radExistingPlan.TabIndex = 29;
            this.radExistingPlan.TabStop = true;
            this.radExistingPlan.Text = "Existing Plan";
            this.radExistingPlan.UseVisualStyleBackColor = true;
            this.radExistingPlan.CheckedChanged += new System.EventHandler(this.radExistingPlan_CheckedChanged);
            // 
            // radNewPlan
            // 
            this.radNewPlan.AutoSize = true;
            this.radNewPlan.Location = new System.Drawing.Point(12, 6);
            this.radNewPlan.Name = "radNewPlan";
            this.radNewPlan.Size = new System.Drawing.Size(71, 17);
            this.radNewPlan.TabIndex = 28;
            this.radNewPlan.TabStop = true;
            this.radNewPlan.Text = "New Plan";
            this.radNewPlan.UseVisualStyleBackColor = true;
            this.radNewPlan.CheckedChanged += new System.EventHandler(this.radNewPlan_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(592, 225);
            this.panel2.TabIndex = 1;
            // 
            // SelectPlanForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 254);
            this.ControlBox = false;
            this.Controls.Add(this.SelectPlanForm_Fill_Panel);
            this.Name = "SelectPlanForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Plan:";
            this.Load += new System.EventHandler(this.SelectPlanForm_Load);
            this.SelectPlanForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SelectPlanForm_Fill_Panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.Misc.UltraPanel SelectPlanForm_Fill_Panel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radExistingPlan;
        private System.Windows.Forms.RadioButton radNewPlan;


    }
}