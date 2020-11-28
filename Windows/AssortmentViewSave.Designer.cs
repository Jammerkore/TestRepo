namespace MIDRetail.Windows
{
	partial class AssortmentViewSave
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
				this.chkView.CheckedChanged -= new System.EventHandler(this.chkView_CheckedChanged);
				this.Closing -= new System.ComponentModel.CancelEventHandler(this.AssortmentViewSave_Closing);
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
			this.grpView = new System.Windows.Forms.GroupBox();
			this.rdoUser = new System.Windows.Forms.RadioButton();
			this.rdoGlobal = new System.Windows.Forms.RadioButton();
			this.txtViewName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.chkView = new System.Windows.Forms.CheckBox();
			this.pnlView = new System.Windows.Forms.Panel();
			this.pnlStore = new System.Windows.Forms.Panel();
			this.chkSaveAssortment = new System.Windows.Forms.CheckBox();
			this.grpView.SuspendLayout();
			this.pnlView.SuspendLayout();
			this.pnlStore.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnSave.Location = new System.Drawing.Point(91, 157);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(179, 157);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// grpView
			// 
			this.grpView.Controls.Add(this.rdoUser);
			this.grpView.Controls.Add(this.rdoGlobal);
			this.grpView.Controls.Add(this.txtViewName);
			this.grpView.Controls.Add(this.label1);
			this.grpView.Location = new System.Drawing.Point(32, 8);
			this.grpView.Name = "grpView";
			this.grpView.Size = new System.Drawing.Size(288, 72);
			this.grpView.TabIndex = 9;
			this.grpView.TabStop = false;
			this.grpView.Text = "View";
			// 
			// rdoUser
			// 
			this.rdoUser.Location = new System.Drawing.Point(104, 48);
			this.rdoUser.Name = "rdoUser";
			this.rdoUser.Size = new System.Drawing.Size(48, 16);
			this.rdoUser.TabIndex = 13;
			this.rdoUser.Text = "User";
			// 
			// rdoGlobal
			// 
			this.rdoGlobal.Location = new System.Drawing.Point(160, 48);
			this.rdoGlobal.Name = "rdoGlobal";
			this.rdoGlobal.Size = new System.Drawing.Size(56, 16);
			this.rdoGlobal.TabIndex = 12;
			this.rdoGlobal.Text = "Global";
			// 
			// txtViewName
			// 
			this.txtViewName.AllowDrop = true;
			this.txtViewName.Location = new System.Drawing.Point(104, 16);
			this.txtViewName.Name = "txtViewName";
			this.txtViewName.Size = new System.Drawing.Size(175, 20);
			this.txtViewName.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 20);
			this.label1.TabIndex = 10;
			this.label1.Text = "View Name:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// chkView
			// 
			this.chkView.Location = new System.Drawing.Point(8, 8);
			this.chkView.Name = "chkView";
			this.chkView.Size = new System.Drawing.Size(16, 24);
			this.chkView.TabIndex = 10;
			this.chkView.CheckedChanged += new System.EventHandler(this.chkView_CheckedChanged);
			// 
			// pnlView
			// 
			this.pnlView.Controls.Add(this.grpView);
			this.pnlView.Controls.Add(this.chkView);
			this.pnlView.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlView.Location = new System.Drawing.Point(0, 57);
			this.pnlView.Name = "pnlView";
			this.pnlView.Size = new System.Drawing.Size(345, 88);
			this.pnlView.TabIndex = 19;
			// 
			// pnlStore
			// 
			this.pnlStore.Controls.Add(this.chkSaveAssortment);
			this.pnlStore.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlStore.Location = new System.Drawing.Point(0, 0);
			this.pnlStore.Name = "pnlStore";
			this.pnlStore.Size = new System.Drawing.Size(345, 57);
			this.pnlStore.TabIndex = 21;
			// 
			// chkSaveAssortment
			// 
			this.chkSaveAssortment.AutoSize = true;
			this.chkSaveAssortment.Location = new System.Drawing.Point(8, 21);
			this.chkSaveAssortment.Name = "chkSaveAssortment";
			this.chkSaveAssortment.Size = new System.Drawing.Size(113, 17);
			this.chkSaveAssortment.TabIndex = 0;
			this.chkSaveAssortment.Text = "Assortment Values";
			this.chkSaveAssortment.UseVisualStyleBackColor = true;
			// 
			// AssortmentViewSave
			// 
			this.AcceptButton = this.btnSave;
			this.AllowDragDrop = true;
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(345, 189);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.pnlView);
			this.Controls.Add(this.pnlStore);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AssortmentViewSave";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AssortmentView Save";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.AssortmentViewSave_Closing);
			this.Controls.SetChildIndex(this.pnlStore, 0);
			this.Controls.SetChildIndex(this.pnlView, 0);
			this.Controls.SetChildIndex(this.btnSave, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.grpView.ResumeLayout(false);
			this.grpView.PerformLayout();
			this.pnlView.ResumeLayout(false);
			this.pnlStore.ResumeLayout(false);
			this.pnlStore.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.GroupBox grpView;
		private System.Windows.Forms.CheckBox chkView;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtViewName;
		private System.Windows.Forms.RadioButton rdoGlobal;
		private System.Windows.Forms.RadioButton rdoUser;
		private System.Windows.Forms.Panel pnlView;
		private System.Windows.Forms.Panel pnlStore;
		private System.Windows.Forms.CheckBox chkSaveAssortment;
	}
}