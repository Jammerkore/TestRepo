using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for ChangePassword.
	/// </summary>
	public class ChangePassword : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtOldPass;
		private System.Windows.Forms.TextBox txtNewPass;
		private System.Windows.Forms.TextBox txtConfirm;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnHelp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ChangePassword()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.txtOldPass = new System.Windows.Forms.TextBox();
			this.txtNewPass = new System.Windows.Forms.TextBox();
			this.txtConfirm = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label1.Location = new System.Drawing.Point(32, 32);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "User:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label2.Location = new System.Drawing.Point(32, 72);
			this.label2.Name = "label2";
			this.label2.TabIndex = 1;
			this.label2.Text = "Old Password:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label3.Location = new System.Drawing.Point(32, 112);
			this.label3.Name = "label3";
			this.label3.TabIndex = 2;
			this.label3.Text = "New Password:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
			this.label4.Location = new System.Drawing.Point(20, 152);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "Confirm Password:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// txtUser
			// 
			this.txtUser.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtUser.Location = new System.Drawing.Point(144, 32);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(128, 20);
			this.txtUser.TabIndex = 4;
			this.txtUser.Text = "";
			// 
			// txtOldPass
			// 
			this.txtOldPass.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtOldPass.Location = new System.Drawing.Point(144, 72);
			this.txtOldPass.Name = "txtOldPass";
			this.txtOldPass.PasswordChar = '*';
			this.txtOldPass.Size = new System.Drawing.Size(128, 20);
			this.txtOldPass.TabIndex = 5;
			this.txtOldPass.Text = "";
			// 
			// txtNewPass
			// 
			this.txtNewPass.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtNewPass.Location = new System.Drawing.Point(144, 112);
			this.txtNewPass.Name = "txtNewPass";
			this.txtNewPass.PasswordChar = '*';
			this.txtNewPass.Size = new System.Drawing.Size(128, 20);
			this.txtNewPass.TabIndex = 6;
			this.txtNewPass.Text = "";
			// 
			// txtConfirm
			// 
			this.txtConfirm.BackColor = System.Drawing.Color.WhiteSmoke;
			this.txtConfirm.Location = new System.Drawing.Point(144, 152);
			this.txtConfirm.Name = "txtConfirm";
			this.txtConfirm.PasswordChar = '*';
			this.txtConfirm.Size = new System.Drawing.Size(128, 20);
			this.txtConfirm.TabIndex = 7;
			this.txtConfirm.Text = "";
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.ForeColor = System.Drawing.Color.Blue;
			this.btnCancel.Location = new System.Drawing.Point(232, 216);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 8;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnOK.ForeColor = System.Drawing.Color.Blue;
			this.btnOK.Location = new System.Drawing.Point(136, 216);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.BackColor = System.Drawing.Color.WhiteSmoke;
			this.btnHelp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnHelp.ForeColor = System.Drawing.Color.Blue;
			this.btnHelp.Location = new System.Drawing.Point(16, 216);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(24, 23);
			this.btnHelp.TabIndex = 10;
			this.btnHelp.Text = "?";
			// 
			// ChangePassword
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 13);
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(328, 254);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.txtConfirm);
			this.Controls.Add(this.txtNewPass);
			this.Controls.Add(this.txtOldPass);
			this.Controls.Add(this.txtUser);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "ChangePassword";
			this.Text = "Change Password";
			this.Activated += new System.EventHandler(this.ChangePassword_Activated);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (txtNewPass.Text != txtConfirm.Text)
			{
				MessageBox.Show("Password confirmation failed!");
				txtNewPass.Clear();
				txtConfirm.Clear();
				txtNewPass.Focus();
			} 
			else if (txtUser.Text == "")
			{
				MessageBox.Show("User required");
				txtUser.Focus();
			}
//			else if (txtOldPass.Text == "")
//			{
//				MessageBox.Show("Old password required");
//				txtOldPass.Focus();
//			}
//			else if (txtNewPass.Text == "")
//			{
//				MessageBox.Show("New password required");
//				txtNewPass.Focus();
//			}
			else
			{
				this.User = txtUser.Text;
				this.OldPassword = txtOldPass.Text;
				this.NewPassword = txtNewPass.Text;
				DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void ChangePassword_Activated(object sender, System.EventArgs e)
		{
			if (txtUser.Text == "")
			{
				txtUser.Focus();
			}
			else if (txtOldPass.Text == "")
			{
				txtOldPass.Focus();
			}
			else 
			{
				txtNewPass.Focus();
			}
		}


		public string User 
		{
			get{return txtUser.Text;}
			set{txtUser.Text = value;}
		}

		public string OldPassword 
		{
			get{return txtOldPass.Text;}
			set{txtOldPass.Text = value;}
		}

		public string NewPassword 
		{
			get{return txtNewPass.Text;}
			set{txtNewPass.Text = value;}
		}

	}
}
