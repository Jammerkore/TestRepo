using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for TextDialog.
	/// </summary>
	public class TextDialog : MIDFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private string _textLabel;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtBox;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private string _textValue;
        private bool _displayOnly; // TT#3040 - RMatelic - Header Notes Box

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
		/// contructor that pre-fills the dialog title and text box text
		/// </summary>
		///<param name="DialogLabel"></param>
		/// <param name="textLabel"></param>
		public TextDialog(string aTitle, string aTextString, int aMaxTextLength, FunctionSecurityProfile aSecurityLevel)
		{
            bool displayOnly = false;
            if (!aSecurityLevel.AllowUpdate)
            {
                displayOnly = true;
            }
          
            LoadDialog(aTitle, aTextString, displayOnly); 
			this.txtBox.MaxLength = aMaxTextLength;
	    }

        /// <summary>
        /// overloaded constructor for display only; added for Assortment
        /// </summary>
        public TextDialog(string aTitle, string aTextString)
        {
            LoadDialog(aTitle, aTextString, true); 
        }

        private void LoadDialog(string aTitle, string aTextString, bool aDisplayOnly)
        {
            try
            {
                InitializeComponent();
                this.btnOK.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
                this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
                this.Text = aTitle;
                _textValue = aTextString;

                if (aDisplayOnly)
                {
                    this.txtBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
                    this.txtBox.Enabled = false;
                    this.btnOK.Enabled = false;
                    _displayOnly = aDisplayOnly;   // TT#3040 - RMatelic - Header Notes Box
                }
            }
            catch
            {
                throw;
            }
        }

		/// <summary>
		/// base constructor
		/// </summary>
		public TextDialog()
		{
			InitializeComponent();
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

				this.txtBox.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtBox_KeyPress);
				this.btnOK.Click -= new System.EventHandler(this.btnOk_Click);
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.Load -= new System.EventHandler(this.TextDialog_Load);
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
            this.lblName = new System.Windows.Forms.Label();
            this.txtBox = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(16, 8);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(40, 16);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Text:";
            // 
            // txtBox
            // 
            this.txtBox.AcceptsReturn = true;
            this.txtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBox.Location = new System.Drawing.Point(16, 24);
            this.txtBox.Multiline = true;
            this.txtBox.Name = "txtBox";
            this.txtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBox.Size = new System.Drawing.Size(376, 128);
            this.txtBox.TabIndex = 1;
            this.txtBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBox_KeyPress);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(232, 168);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(320, 168);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TextDialog
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(408, 198);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtBox);
            this.Controls.Add(this.lblName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextDialog";
            this.Text = "Enter Text";
            this.Load += new System.EventHandler(this.TextDialog_Load);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.Controls.SetChildIndex(this.txtBox, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void TextDialog_Load(object sender, System.EventArgs e)
		{
			this.txtBox.Text = _textValue;
			this.txtBox.Select(0,0);
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			_textValue = this.txtBox.Text.TrimStart();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void txtBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if ( e.KeyChar == 13 )
			{
                // Begin TT#3040 - RMatelic - Header Notes Box
                //_textValue = this.txtBox.Text;
                //DialogResult = DialogResult.OK;
                //Close();
                if (_displayOnly)
                {
                    _textValue = this.txtBox.Text;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                // End TT#3040
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

	}
}
