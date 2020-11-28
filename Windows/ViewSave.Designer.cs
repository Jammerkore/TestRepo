namespace MIDRetail.Windows
{
	partial class ViewSave
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

			if (disposing)
			{
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.ViewSave_Closing);
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.rdoUser = new System.Windows.Forms.RadioButton();
            this.rdoGlobal = new System.Windows.Forms.RadioButton();
            this.txtViewName = new System.Windows.Forms.TextBox();
            this.lblViewName = new System.Windows.Forms.Label();
            this.pnlView = new System.Windows.Forms.Panel();
            this.chkUseFilterSorting = new System.Windows.Forms.CheckBox();
            this.cboHeaderFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
            this.chkShowDetails = new System.Windows.Forms.CheckBox();
            this.chkApplyFilter = new System.Windows.Forms.CheckBox();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.pnlView.SuspendLayout();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSave.Location = new System.Drawing.Point(44, 196);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(226, 196);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // rdoUser
            // 
            this.rdoUser.Location = new System.Drawing.Point(115, 53);
            this.rdoUser.Name = "rdoUser";
            this.rdoUser.Size = new System.Drawing.Size(48, 16);
            this.rdoUser.TabIndex = 2;
            this.rdoUser.Text = "User";
            // 
            // rdoGlobal
            // 
            this.rdoGlobal.Location = new System.Drawing.Point(171, 53);
            this.rdoGlobal.Name = "rdoGlobal";
            this.rdoGlobal.Size = new System.Drawing.Size(56, 16);
            this.rdoGlobal.TabIndex = 3;
            this.rdoGlobal.Text = "Global";
            // 
            // txtViewName
            // 
            this.txtViewName.AllowDrop = true;
            this.txtViewName.Location = new System.Drawing.Point(115, 21);
            this.txtViewName.Name = "txtViewName";
            this.txtViewName.Size = new System.Drawing.Size(175, 20);
            this.txtViewName.TabIndex = 0;
            // 
            // lblViewName
            // 
            this.lblViewName.Location = new System.Drawing.Point(27, 21);
            this.lblViewName.Name = "lblViewName";
            this.lblViewName.Size = new System.Drawing.Size(72, 20);
            this.lblViewName.TabIndex = 10;
            this.lblViewName.Text = "View Name:";
            this.lblViewName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlView
            // 
            this.pnlView.Controls.Add(this.chkUseFilterSorting);
            this.pnlView.Controls.Add(this.cboHeaderFilter);
            this.pnlView.Controls.Add(this.chkShowDetails);
            this.pnlView.Controls.Add(this.chkApplyFilter);
            this.pnlView.Controls.Add(this.rdoUser);
            this.pnlView.Controls.Add(this.txtViewName);
            this.pnlView.Controls.Add(this.lblViewName);
            this.pnlView.Controls.Add(this.rdoGlobal);
            this.pnlView.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlView.Location = new System.Drawing.Point(0, 0);
            this.pnlView.Name = "pnlView";
            this.pnlView.Size = new System.Drawing.Size(345, 186);
            this.pnlView.TabIndex = 0;
            // 
            // chkUseFilterSorting
            // 
            this.chkUseFilterSorting.AutoSize = true;
            this.chkUseFilterSorting.Enabled = false;
            this.chkUseFilterSorting.Location = new System.Drawing.Point(115, 116);
            this.chkUseFilterSorting.Name = "chkUseFilterSorting";
            this.chkUseFilterSorting.Size = new System.Drawing.Size(106, 17);
            this.chkUseFilterSorting.TabIndex = 14;
            this.chkUseFilterSorting.Text = "Use Filter Sorting";
            this.chkUseFilterSorting.UseVisualStyleBackColor = true;
            // 
            // cboHeaderFilter
            // 
            this.cboHeaderFilter.AutoAdjust = true;
            this.cboHeaderFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboHeaderFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboHeaderFilter.DataSource = null;
            this.cboHeaderFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHeaderFilter.DropDownWidth = 175;
            this.cboHeaderFilter.Enabled = false;
            this.cboHeaderFilter.FormattingEnabled = false;
            this.cboHeaderFilter.IgnoreFocusLost = false;
            this.cboHeaderFilter.ItemHeight = 13;
            this.cboHeaderFilter.Location = new System.Drawing.Point(115, 85);
            this.cboHeaderFilter.Margin = new System.Windows.Forms.Padding(0);
            this.cboHeaderFilter.MaxDropDownItems = 25;
            this.cboHeaderFilter.Name = "cboHeaderFilter";
            this.cboHeaderFilter.SetToolTip = "";
            this.cboHeaderFilter.Size = new System.Drawing.Size(175, 23);
            this.cboHeaderFilter.TabIndex = 13;
            this.cboHeaderFilter.Tag = null;
            // 
            // chkShowDetails
            // 
            this.chkShowDetails.AutoSize = true;
            this.chkShowDetails.Location = new System.Drawing.Point(19, 143);
            this.chkShowDetails.Name = "chkShowDetails";
            this.chkShowDetails.Size = new System.Drawing.Size(88, 17);
            this.chkShowDetails.TabIndex = 12;
            this.chkShowDetails.Text = "Show Details";
            this.chkShowDetails.UseVisualStyleBackColor = true;
            // 
            // chkApplyFilter
            // 
            this.chkApplyFilter.AutoSize = true;
            this.chkApplyFilter.Location = new System.Drawing.Point(19, 85);
            this.chkApplyFilter.Name = "chkApplyFilter";
            this.chkApplyFilter.Size = new System.Drawing.Size(80, 17);
            this.chkApplyFilter.TabIndex = 11;
            this.chkApplyFilter.Text = "Apply Filter:";
            this.chkApplyFilter.UseVisualStyleBackColor = true;
            this.chkApplyFilter.CheckedChanged += new System.EventHandler(this.chkApplyFilter_CheckedChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDelete.Location = new System.Drawing.Point(135, 196);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ViewSave
            // 
            this.AcceptButton = this.btnSave;
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(345, 231);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pnlView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ViewSave";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save View";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ViewSave_Closing);
            this.Controls.SetChildIndex(this.pnlView, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnDelete, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.pnlView.ResumeLayout(false);
            this.pnlView.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblViewName;
		private System.Windows.Forms.TextBox txtViewName;
		private System.Windows.Forms.RadioButton rdoGlobal;
		private System.Windows.Forms.RadioButton rdoUser;
        private System.Windows.Forms.Panel pnlView;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.CheckBox chkApplyFilter;
        private System.Windows.Forms.CheckBox chkShowDetails;
        private Controls.MIDComboBoxEnh cboHeaderFilter;
        private System.Windows.Forms.CheckBox chkUseFilterSorting;
	}
}