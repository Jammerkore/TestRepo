using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
    public partial class NameDescriptionDialog : MIDFormBase
    {
        private string _windowTitle;
        private string _messageText;
        private string _label1_Text;
        private string _label2_Text;
        private string _textBox1_Text;
        private string _textBox2_Text;

        /// <summary>
        /// Gets the Text of the text box
        /// </summary>
        public string TextValue2
        {
            get { return textBox2.Text; }
        }

        public NameDescriptionDialog()
        {
            InitializeComponent();
        }

        public NameDescriptionDialog(string aWindowTitle, string aMessageText, string aLabel1, string aLabel2, string aTextBox1Value, string aTextBox2Value)
        {
            InitializeComponent();
            _windowTitle = aWindowTitle;
            _messageText = aMessageText;
            _label1_Text = aLabel1;
            _label2_Text = aLabel2;
            _textBox1_Text = aTextBox1Value;
            _textBox2_Text = aTextBox2Value;
        }

        private void NameDescriptionDialog_Load(object sender, EventArgs e)
        {
            SetText();
           
            this.textBox2.Select(0,0);
        }

        private void SetText() 
        {
            this.Text = _windowTitle;
            this.lblMessageText.Text = _messageText;
            this.label1.Text = _label1_Text;
            this.label2.Text = _label2_Text;
            this.textBox1.Text = _textBox1_Text;
            this.textBox2.Text = _textBox2_Text;

            this.btnYes.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Yes);
            this.btnNo.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_No);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (!ValidTextField(this.textBox2.Text))
            {
                string errorMessage = MIDText.GetTextOnly(eMIDTextCode.msg_FieldIsRequired);
                ErrorProvider.SetError(this.textBox2, errorMessage);
            }
            else
            {
                DialogResult = DialogResult.Yes;
                Close();
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)    // Enter key
            {
                if (!ValidTextField(this.textBox2.Text))
                {
                    string errorMessage = MIDText.GetTextOnly(eMIDTextCode.msg_FieldIsRequired);
                    ErrorProvider.SetError(this.textBox2, errorMessage);
                }
                else
                {
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
        }

        private bool ValidTextField(string aTextField)           
        {
            bool validField;
            try
            {
                aTextField = aTextField.Trim();
                if (aTextField == null || aTextField == string.Empty)
                {
                    validField = false;
                }
                else
                {
                    validField = true;
                }
            }
            catch
            {
                throw;
            }
            return validField;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ErrorProvider.SetError(this.textBox2, string.Empty);
        }
    }
}
