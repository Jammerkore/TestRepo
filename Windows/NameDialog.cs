using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for NameDialog.
	/// </summary>
	public class NameDialog : MIDFormBase
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string _textLabel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private string _textValue;
		private System.Windows.Forms.CheckBox cbxCaseSensitive;
        private RadioButton rdoUser;
        private RadioButton rdoGlobal;
		private bool _treatEmptyAsCancel = true;
        // Begin Track #4872 - JSmith - Global/User Attributes
        private bool _showUserGlobal = false;
        // End Track #4872

		/// <summary>
		/// Gets or sets the text label for the forms text box
		/// </summary>
		public string TextLabel 
		{
			get{return _textLabel;}
			set{_textLabel = value;}
		}

		/// <summary>
		/// Gets or sets the Text of the text box
		/// </summary>
		public string TextValue 
		{
			get{return _textValue;}
			set{_textValue = value;}
		}

		/// <summary>
		/// Gets or sets the flag identifying if no value should be treated as a cancel 
		/// </summary>
		public bool TreatEmptyAsCancel 
		{
			get{return _treatEmptyAsCancel;}
			set{_treatEmptyAsCancel = value;}
		}

		// BEGIN MID Track #5170 - JSmith - Model enhancements
		/// <summary>
		/// Gets the flag identifying if case sensitivity is to be used 
		/// </summary>
		public bool CaseSensitive 
		{
			get{return cbxCaseSensitive.Checked;}
		}
		// End MID Track #5170

        // Begin Track #4872 - JSmith - Global/User Attributes
        public bool ShowUserGlobal
        {
            get { return _showUserGlobal; }
            set { _showUserGlobal = value; }
        }
        public bool isGlobalChecked
        {
            get { return rdoGlobal.Checked; }
            set { rdoGlobal.Checked = value; }
        }
        public bool isUserChecked
        {
            get { return rdoUser.Checked; }
            set { rdoUser.Checked = value; }
        }
        // End Track #4872

		/// <summary>
		/// contructor that pre-fills the dialog title and the label text
		/// </summary>
		/// <param name="DialogLabel"></param>
		/// <param name="textLabel"></param>
		public NameDialog(string DialogLabel, string textLabel)
		{
			InitializeComponent();
			this.Text = DialogLabel;
			_textLabel = textLabel;
            // BEGIN MID Track #5170 - JSmith - Model enhancements
            CommonConstructor();
            // END MID Track #5170
		}

		/// <summary>
		/// contructor that pre-fills the label text
		/// </summary>
		/// <param name="textLabel"></param>
		public NameDialog(string textLabel)
		{
			_textLabel = textLabel;
			InitializeComponent();
            // BEGIN MID Track #5170 - JSmith - Model enhancements
            CommonConstructor();
            // END MID Track #5170
		}
		
		// BEGIN ANF Generic Size Constraint
		/// <summary>
		/// contructor that pre-fills the dialog title, the label text and the text value
		/// </summary>
		/// <param name="DialogLabel"></param>
		/// <param name="textLabel"></param>
		/// <param name="textValue"></param>
		public NameDialog(string DialogLabel, string textLabel, string textValue)
		{
			InitializeComponent();
			this.Text = DialogLabel;
			_textLabel = textLabel;
			_textValue = textValue;
            // BEGIN MID Track #5170 - JSmith - Model enhancements
            CommonConstructor();
            // END MID Track #5170
		}
		// END ANF Generic Size Constraint

		/// <summary>
		/// base constructor
		/// </summary>
		public NameDialog()
		{
			InitializeComponent();
            // BEGIN MID Track #5170 - JSmith - Model enhancements
            CommonConstructor();
            // END MID Track #5170
		}

        // BEGIN MID Track #5170 - JSmith - Model enhancements
        public void CommonConstructor()
		{
			cbxCaseSensitive.Visible = false;
		}
		// END MID Track #5170

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

				this.textBox1.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
				this.btnOk.Click -= new System.EventHandler(this.btnOk_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.NameDialog_Load);
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbxCaseSensitive = new System.Windows.Forms.CheckBox();
            this.rdoUser = new System.Windows.Forms.RadioButton();
            this.rdoGlobal = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(16, 40);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(304, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(160, 93);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&Ok";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(248, 93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbxCaseSensitive
            // 
            this.cbxCaseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxCaseSensitive.Location = new System.Drawing.Point(16, 89);
            this.cbxCaseSensitive.Name = "cbxCaseSensitive";
            this.cbxCaseSensitive.Size = new System.Drawing.Size(104, 24);
            this.cbxCaseSensitive.TabIndex = 4;
            this.cbxCaseSensitive.Text = "Case Sensitive";
            // 
            // rdoUser
            // 
            this.rdoUser.Location = new System.Drawing.Point(166, 66);
            this.rdoUser.Name = "rdoUser";
            this.rdoUser.Size = new System.Drawing.Size(48, 19);
            this.rdoUser.TabIndex = 17;
            this.rdoUser.Text = "User";
            this.rdoUser.Visible = false;
            // 
            // rdoGlobal
            // 
            this.rdoGlobal.Location = new System.Drawing.Point(104, 66);
            this.rdoGlobal.Name = "rdoGlobal";
            this.rdoGlobal.Size = new System.Drawing.Size(56, 19);
            this.rdoGlobal.TabIndex = 16;
            this.rdoGlobal.Text = "Global";
            this.rdoGlobal.Visible = false;
            // 
            // NameDialog
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(336, 123);
            this.Controls.Add(this.rdoUser);
            this.Controls.Add(this.rdoGlobal);
            this.Controls.Add(this.cbxCaseSensitive);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NameDialog";
            this.Text = "Enter Name";
            this.Load += new System.EventHandler(this.NameDialog_Load);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.textBox1, 0);
            this.Controls.SetChildIndex(this.cbxCaseSensitive, 0);
            this.Controls.SetChildIndex(this.rdoGlobal, 0);
            this.Controls.SetChildIndex(this.rdoUser, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void NameDialog_Load(object sender, System.EventArgs e)
		{
			this.label1.Text = _textLabel;
			this.textBox1.Text = _textValue;
			// BEGIN MID Track #5170 - JSmith - Model enhancements
			cbxCaseSensitive.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Case_Sensitive);
			// END MID Track #5170
            // Begin Track #4872 - JSmith - Global/User Attributes
            if (_showUserGlobal)
            {
                rdoGlobal.Enabled = true;
                rdoUser.Enabled = true;
                rdoGlobal.Visible = true;
                rdoUser.Visible = true;
                rdoGlobal.Checked = true;
            }
            else
            {
                rdoGlobal.Enabled = false;
                rdoUser.Enabled = false;
                rdoGlobal.Visible = false;
                rdoUser.Visible = false;
            }
            // End Track #4872
			this.textBox1.Select();
		}

		// BEGIN MID Track #5170 - JSmith - Model enhancements
		public void AllowCaseSensitive()
		{
			cbxCaseSensitive.Visible = true;
			cbxCaseSensitive.Checked = true;
		}
		// END MID Track #5170

        // Begin TT#814 - RMatelic - DB error creating Multi Header name with more than 32 characters
        public void SetTextMaxLength(int aMaxLength)
        {
            textBox1.MaxLength = aMaxLength;
        }
        // End TT#814 

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			_textValue = this.textBox1.Text.TrimStart();

			if ((_textValue == null || _textValue == "") &&
				_treatEmptyAsCancel)
				DialogResult = DialogResult.Cancel;
			else
				DialogResult = DialogResult.OK;

			Close();
		}

		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 )
			{
				_textValue = this.textBox1.Text;

				if (_textValue == null || _textValue == "")
					DialogResult = DialogResult.Cancel;
				else
					DialogResult = DialogResult.OK;

				Close();
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			_textValue = string.Empty;
			DialogResult = DialogResult.Cancel;
		}
	}
}
