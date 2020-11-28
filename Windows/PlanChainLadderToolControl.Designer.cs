namespace MIDRetail.Windows
{
    partial class PlanChainLadderToolControl
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


        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.Panel pnlSpacer1;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboView;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboDollarScaling;
        private MIDRetail.Windows.Controls.MIDComboBoxEnh cboUnitScaling;

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApply = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.pnlSpacer1 = new System.Windows.Forms.Panel();
            this.cboView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboDollarScaling = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboUnitScaling = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.pnlTop.SuspendLayout();
            this.pnlControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlControls);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(8, 55);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1082, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // pnlControls
            // 
            this.pnlControls.Controls.Add(this.btnApply);
            this.pnlControls.Controls.Add(this.pnlSpacer1);
            this.pnlControls.Controls.Add(this.cboDollarScaling);
            this.pnlControls.Controls.Add(this.cboUnitScaling);
            this.pnlControls.Controls.Add(this.cboView);
            this.pnlControls.Location = new System.Drawing.Point(8, 8);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(901, 24);
            this.pnlControls.TabIndex = 11;
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnApply.Location = new System.Drawing.Point(656, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 48);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // pnlSpacer1
            // 
            this.pnlSpacer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSpacer1.Location = new System.Drawing.Point(797, 0);
            this.pnlSpacer1.Name = "pnlSpacer1";
            this.pnlSpacer1.Size = new System.Drawing.Size(16, 24);
            this.pnlSpacer1.TabIndex = 11;
            // 
            // cboView
            // 
            this.cboView.AutoAdjust = true;
            this.cboView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboView.DataSource = null;
            this.cboView.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.DropDownWidth = 100;
            this.cboView.FormattingEnabled = false;
            this.cboView.IgnoreFocusLost = false;
            this.cboView.ItemHeight = 13;
            this.cboView.Location = new System.Drawing.Point(456, 0);
            this.cboView.Margin = new System.Windows.Forms.Padding(0);
            this.cboView.MaxDropDownItems = 25;
            this.cboView.Name = "cboView";
            this.cboView.SetToolTip = "";
            this.cboView.Size = new System.Drawing.Size(100, 48);
            this.cboView.TabIndex = 9;
            this.cboView.Tag = null;
            this.cboView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboView_MIDComboBoxPropertiesChangedEvent);
            this.cboView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // cboDollarScaling
            // 
            this.cboDollarScaling.AutoAdjust = true;
            this.cboDollarScaling.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboDollarScaling.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboDollarScaling.DataSource = null;
            this.cboDollarScaling.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboDollarScaling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDollarScaling.DropDownWidth = 68;
            this.cboDollarScaling.FormattingEnabled = false;
            this.cboDollarScaling.IgnoreFocusLost = false;
            this.cboDollarScaling.ItemHeight = 13;
            this.cboDollarScaling.Location = new System.Drawing.Point(188, 0);
            this.cboDollarScaling.Margin = new System.Windows.Forms.Padding(0);
            this.cboDollarScaling.MaxDropDownItems = 25;
            this.cboDollarScaling.Name = "cboDollarScaling";
            this.cboDollarScaling.SetToolTip = "";
            this.cboDollarScaling.Size = new System.Drawing.Size(68, 48);
            this.cboDollarScaling.TabIndex = 15;
            this.cboDollarScaling.Tag = null;
            this.cboDollarScaling.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboDollarScaling_MIDComboBoxPropertiesChangedEvent);
            this.cboDollarScaling.SelectionChangeCommitted += new System.EventHandler(this.cboDollarScaling_SelectionChangeCommitted);
            // 
            // cboUnitScaling
            // 
            this.cboUnitScaling.AutoAdjust = true;
            this.cboUnitScaling.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboUnitScaling.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboUnitScaling.DataSource = null;
            this.cboUnitScaling.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboUnitScaling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnitScaling.DropDownWidth = 68;
            this.cboUnitScaling.FormattingEnabled = false;
            this.cboUnitScaling.IgnoreFocusLost = false;
            this.cboUnitScaling.ItemHeight = 13;
            this.cboUnitScaling.Location = new System.Drawing.Point(120, 0);
            this.cboUnitScaling.Margin = new System.Windows.Forms.Padding(0);
            this.cboUnitScaling.MaxDropDownItems = 25;
            this.cboUnitScaling.Name = "cboUnitScaling";
            this.cboUnitScaling.SetToolTip = "";
            this.cboUnitScaling.Size = new System.Drawing.Size(68, 48);
            this.cboUnitScaling.TabIndex = 14;
            this.cboUnitScaling.Tag = null;
            this.cboUnitScaling.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboUnitScaling_MIDComboBoxPropertiesChangedEvent);
            this.cboUnitScaling.SelectionChangeCommitted += new System.EventHandler(this.cboUnitScaling_SelectionChangeCommitted);

            // 
            // PlanChainLadderToolControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlTop);
            this.Name = "PlanChainLadderToolControl";
            this.Size = new System.Drawing.Size(748, 40);
            this.Load += new System.EventHandler(this.PlanChainLadderToolControl_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


    }
}
