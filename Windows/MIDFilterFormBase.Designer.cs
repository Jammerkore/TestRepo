namespace MIDRetail.Windows
{
	partial class MIDFilterFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		///// <summary>
		///// Clean up any resources being used.
		///// </summary>
		///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		//protected override void Dispose(bool disposing)
		//{
		//    if (disposing && (components != null))
		//    {
		//        components.Dispose();
		//    }
		//    base.Dispose(disposing);
		//}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.cmsLabelMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.txtGradeEdit = new System.Windows.Forms.TextBox();
			this.txtLiteralEdit = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmsLabelMenu
			// 
			this.cmsLabelMenu.Name = "cmsLabelMenu";
			this.cmsLabelMenu.Size = new System.Drawing.Size(61, 4);
			this.cmsLabelMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsLabelMenu_Opening);
			// 
			// txtGradeEdit
			// 
			this.txtGradeEdit.AcceptsReturn = true;
			this.txtGradeEdit.AcceptsTab = true;
			this.txtGradeEdit.BackColor = System.Drawing.SystemColors.Window;
			this.txtGradeEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtGradeEdit.Enabled = false;
			this.txtGradeEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtGradeEdit.Location = new System.Drawing.Point(114, 135);
			this.txtGradeEdit.Multiline = true;
			this.txtGradeEdit.Name = "txtGradeEdit";
			this.txtGradeEdit.Size = new System.Drawing.Size(64, 20);
			this.txtGradeEdit.TabIndex = 56;
			this.txtGradeEdit.TabStop = false;
			this.txtGradeEdit.Visible = false;
			this.txtGradeEdit.WordWrap = false;
			this.txtGradeEdit.Leave += new System.EventHandler(this.txtGradeEdit_Leave);
			this.txtGradeEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGradeEdit_KeyPress);
			// 
			// txtLiteralEdit
			// 
			this.txtLiteralEdit.AcceptsReturn = true;
			this.txtLiteralEdit.AcceptsTab = true;
			this.txtLiteralEdit.BackColor = System.Drawing.SystemColors.Window;
			this.txtLiteralEdit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtLiteralEdit.Enabled = false;
			this.txtLiteralEdit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLiteralEdit.Location = new System.Drawing.Point(114, 111);
			this.txtLiteralEdit.Multiline = true;
			this.txtLiteralEdit.Name = "txtLiteralEdit";
			this.txtLiteralEdit.Size = new System.Drawing.Size(64, 20);
			this.txtLiteralEdit.TabIndex = 55;
			this.txtLiteralEdit.TabStop = false;
			this.txtLiteralEdit.Visible = false;
			this.txtLiteralEdit.WordWrap = false;
			this.txtLiteralEdit.Leave += new System.EventHandler(this.txtLiteralEdit_Leave);
			this.txtLiteralEdit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLiteralEdit_KeyPress);
			// 
			// MIDFilterFormBase
			// 
			this.AllowDragDrop = true;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.txtGradeEdit);
			this.Controls.Add(this.txtLiteralEdit);
			this.Name = "MIDFilterFormBase";
			this.Controls.SetChildIndex(this.txtLiteralEdit, 0);
			this.Controls.SetChildIndex(this.txtGradeEdit, 0);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip cmsLabelMenu;
		protected System.Windows.Forms.TextBox txtGradeEdit;
        protected System.Windows.Forms.TextBox txtLiteralEdit;
	}
}
