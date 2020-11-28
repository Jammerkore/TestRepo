using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO.IsolatedStorage;
using System.IO;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for Login.
	/// </summary>
	/// 
	
	public class formDebugLogin : System.Windows.Forms.Form
	{
		string _user = null;
        string _environmentMsg = string.Empty;
		string _password = null;
		bool _cancelled = false;
		bool _OKPressed = false;
		
		public string User 
		{
			get { return _user ; }
			set { _user = value; }
		}
		public string Password 
		{
			get { return _password ; }
			set { _password = value; }
		}
		public string NewPassword 
		{
			get { return null ; }
		}
        public string EnvironmentMsg
        {
            get { return _environmentMsg; }
            set { _environmentMsg = value; }
        }
		public bool Cancelled 
		{
			get { return _cancelled ; }
			set { _cancelled = value; }
		}
		public bool ShowLogin 
		{
			get { return chbShowLogin.Checked ; }
			set {chbShowLogin.Checked = value;}
		}

		private System.Windows.Forms.Label lblUser;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button7;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox chbShowLogin;
		private System.Windows.Forms.Button button8;
		private System.Windows.Forms.Button button4;
        private Button button10;
        private Button button9;
        private Label lblEnvironment;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public formDebugLogin()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formDebugLogin));
            this.lblUser = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.chbShowLogin = new System.Windows.Forms.CheckBox();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblUser
            // 
            this.lblUser.Location = new System.Drawing.Point(48, 16);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(40, 23);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "User:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(96, 16);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(152, 20);
            this.txtUser.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(32, 56);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 23);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(96, 56);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(152, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(187, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(99, 350);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(21, 350);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(24, 23);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "?";
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button10);
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.button7);
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(16, 136);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 201);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(136, 126);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(80, 23);
            this.button10.TabIndex = 9;
            this.button10.Text = "Vic";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(27, 159);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(80, 23);
            this.button9.TabIndex = 8;
            this.button9.Text = "Dick";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(27, 126);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Jeff";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(136, 159);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(80, 23);
            this.button8.TabIndex = 6;
            this.button8.Text = "Administrator";
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(136, 92);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(80, 23);
            this.button7.TabIndex = 5;
            this.button7.Text = "Jim";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(136, 58);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 23);
            this.button6.TabIndex = 4;
            this.button6.Text = "Steve";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(136, 24);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 23);
            this.button5.TabIndex = 3;
            this.button5.Text = "John";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(27, 92);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Dan";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(27, 58);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Alan";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Ron";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chbShowLogin
            // 
            this.chbShowLogin.Location = new System.Drawing.Point(32, 111);
            this.chbShowLogin.Name = "chbShowLogin";
            this.chbShowLogin.Size = new System.Drawing.Size(176, 24);
            this.chbShowLogin.TabIndex = 8;
            this.chbShowLogin.Text = "Show login";
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(4, 89);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(35, 13);
            this.lblEnvironment.TabIndex = 9;
            this.lblEnvironment.Text = "label1";
            // 
            // formDebugLogin
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(281, 387);
            this.Controls.Add(this.lblEnvironment);
            this.Controls.Add(this.chbShowLogin);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formDebugLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.formDebugLogin_Closing);
            this.Load += new System.EventHandler(this.Login_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void Login_Load(object sender, System.EventArgs e)
		{
			try
			{
                lblEnvironment.Text = _environmentMsg;  // TT#563-MD AGallagher - Show Environment on Login screen
				Cursor.Current = Cursors.WaitCursor;
				if (User != null && User.Length > 0)
				{
					txtUser.Text = User;
				}
			}

			catch( Exception exception )
			{
				MessageBox.Show(this, exception.Message );
			}

			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}


		private void btnOK_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			if (txtUser.Text == "")
			{
				MessageBox.Show("User required");
			}
			else
			{
				User = txtUser.Text;
				Password = txtPassword.Text;
                this.Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Cancelled = true;
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "rmatelic";
			Password = "ron";
            this.Close();
        }

		private void button2_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "agallagher";
			Password = "agallagher";
            this.Close();
        }

		private void button3_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "doconnell";
			Password = "doconnell";
            this.Close();
        }

		private void button4_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "jsobek";
            Password = "jsobek";
			this.Close();
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "jsmith";
			Password = "jsmith";
            this.Close();
        }

		private void button6_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "stodd";
			Password = "stodd";
            this.Close();
        }

		private void button7_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "jellis";
			Password = "jim";
            this.Close();
        }

		private void button8_Click(object sender, System.EventArgs e)
		{
			_OKPressed = true;
			User = "administrator";
			Password = "administrator";
			this.Close();
		}

        private void button9_Click(object sender, EventArgs e)
        {
            _OKPressed = true;
            User = "rbeck";
            Password = "dick";
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _OKPressed = true;
            User = "vstuart";
            Password = "vic";
            this.Close();
        }

		private void formDebugLogin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!_OKPressed)
			{
				Cancelled = true;
			}
		}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			MessageBox.Show("There is no help for you.", "Help", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}
}
