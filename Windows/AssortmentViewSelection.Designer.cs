using MIDRetail.Windows.Controls;
namespace MIDRetail.Windows
{
	partial class AssortmentViewSelection
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
			if (disposing) 
			{
                if (components != null)
                {
				    components.Dispose();
                }
                this.cboView.SelectionChangeCommitted -= new System.EventHandler(this.cboView_SelectionChangeCommitted);
                //Begin TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.cboView.MIDComboBoxPropertiesChangedEvent -= new MIDComboBoxPropertiesChangedEventHandler(cboView_MIDComboBoxPropertiesChangedEvent);
                //End TT#316 - MD - DOConnell - Replace all Windows Combobox controls with new enhanced control
                this.btOk.Click -= new System.EventHandler(this.btnOK_Click);
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
            this.gbDisplay = new System.Windows.Forms.GroupBox();
            this.cboView = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.cboGroupBy = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.lbView = new System.Windows.Forms.Label();
            this.lbGroupBy = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.gbAverage.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.gbDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboStoreAttribute
            // 
            this.cboStoreAttribute.Location = new System.Drawing.Point(92, 30);
            this.cboStoreAttribute.Size = new System.Drawing.Size(193, 21);
            // 
            // lblAttribute
            // 
            this.lblAttribute.Location = new System.Drawing.Point(33, 32);
            // 
            // cbxSimilarStores
            // 
            this.cbxSimilarStores.Location = new System.Drawing.Point(106, 426);
            // 
            // cbxIntransit
            // 
            this.cbxIntransit.Location = new System.Drawing.Point(380, 426);
            // 
            // cbxOnhand
            // 
            this.cbxOnhand.Location = new System.Drawing.Point(232, 426);
            // 
            // tabControl2
            // 
            this.tabControl2.Size = new System.Drawing.Size(661, 321);
            // 
            // cbxCommitted
            // 
            this.cbxCommitted.Location = new System.Drawing.Point(494, 428);
            // 
            // btOk
            // 
            this.btOk.Location = new System.Drawing.Point(492, 465);
            this.btOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(594, 465);
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // gbDisplay
            // 
            this.gbDisplay.Controls.Add(this.cboView);
            this.gbDisplay.Controls.Add(this.cboGroupBy);
            this.gbDisplay.Controls.Add(this.lbView);
            this.gbDisplay.Controls.Add(this.lbGroupBy);
            this.gbDisplay.Location = new System.Drawing.Point(12, 12);
            this.gbDisplay.Name = "gbDisplay";
            this.gbDisplay.Size = new System.Drawing.Size(655, 83);
            this.gbDisplay.TabIndex = 15;
            this.gbDisplay.TabStop = false;
            this.gbDisplay.Text = "Display";
            // 
            // cboView
            // 
            this.cboView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboView.AutoAdjust = true;
            this.cboView.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboView.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboView.DataSource = null;
            this.cboView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboView.DropDownWidth = 193;
            this.cboView.FormattingEnabled = false;
            this.cboView.IgnoreFocusLost = false;
            this.cboView.ItemHeight = 13;
            this.cboView.Location = new System.Drawing.Point(377, 46);
            this.cboView.Margin = new System.Windows.Forms.Padding(0);
            this.cboView.MaxDropDownItems = 25;
            this.cboView.Name = "cboView";
            this.cboView.SetToolTip = "";
            this.cboView.Size = new System.Drawing.Size(193, 21);
            this.cboView.TabIndex = 2;
            this.cboView.Tag = null;
            this.cboView.MIDComboBoxPropertiesChangedEvent += new MIDRetail.Windows.Controls.MIDComboBoxPropertiesChangedEventHandler(this.cboView_MIDComboBoxPropertiesChangedEvent);
            this.cboView.SelectionChangeCommitted += new System.EventHandler(this.cboView_SelectionChangeCommitted);
            // 
            // cboGroupBy
            // 
            this.cboGroupBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboGroupBy.AutoAdjust = true;
            this.cboGroupBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboGroupBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboGroupBy.DataSource = null;
            this.cboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroupBy.DropDownWidth = 193;
            this.cboGroupBy.FormattingEnabled = false;
            this.cboGroupBy.IgnoreFocusLost = false;
            this.cboGroupBy.ItemHeight = 13;
            this.cboGroupBy.Location = new System.Drawing.Point(80, 46);
            this.cboGroupBy.Margin = new System.Windows.Forms.Padding(0);
            this.cboGroupBy.MaxDropDownItems = 25;
            this.cboGroupBy.Name = "cboGroupBy";
            this.cboGroupBy.SetToolTip = "";
            this.cboGroupBy.Size = new System.Drawing.Size(193, 21);
            this.cboGroupBy.TabIndex = 0;
            this.cboGroupBy.Tag = null;
            // 
            // lbView
            // 
            this.lbView.AutoSize = true;
            this.lbView.Location = new System.Drawing.Point(335, 51);
            this.lbView.Name = "lbView";
            this.lbView.Size = new System.Drawing.Size(33, 13);
            this.lbView.TabIndex = 3;
            this.lbView.Text = "View:";
            // 
            // lbGroupBy
            // 
            this.lbGroupBy.AutoSize = true;
            this.lbGroupBy.Location = new System.Drawing.Point(19, 51);
            this.lbGroupBy.Name = "lbGroupBy";
            this.lbGroupBy.Size = new System.Drawing.Size(54, 13);
            this.lbGroupBy.TabIndex = 1;
            this.lbGroupBy.Text = "Group By:";
            // 
            // AssortmentViewSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 500);
            this.Controls.Add(this.gbDisplay);
            this.Name = "AssortmentViewSelection";
            this.Text = "Assortment Selection";
            this.Load += new System.EventHandler(this.AssortmentViewSelection_Load);
            this.Controls.SetChildIndex(this.gbDisplay, 0);
            this.Controls.SetChildIndex(this.cbxCommitted, 0);
            this.Controls.SetChildIndex(this.tabControl2, 0);
            this.Controls.SetChildIndex(this.cbxOnhand, 0);
            this.Controls.SetChildIndex(this.cbxIntransit, 0);
            this.Controls.SetChildIndex(this.cbxSimilarStores, 0);
            this.Controls.SetChildIndex(this.lblAttribute, 0);
            this.Controls.SetChildIndex(this.cboStoreAttribute, 0);
            this.Controls.SetChildIndex(this.btOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.panel2.ResumeLayout(false);
            this.gbAverage.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.gbDisplay.ResumeLayout(false);
            this.gbDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox gbDisplay;
        private System.Windows.Forms.Label lbGroupBy;
        private System.Windows.Forms.Label lbView;
        private Controls.MIDComboBoxEnh cboGroupBy;
        private Controls.MIDComboBoxEnh cboView;
	}
}