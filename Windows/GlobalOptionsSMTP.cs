using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
    public partial class GlobalOptionsSMTP : UserControl
    {
        public GlobalOptionsSMTP()
        {
            InitializeComponent();
        }

        public struct controlText
        {
            public static string OPTION_USE_DEFAULT_CREDENTIALS = "Use Default Server Credentials";
            public static string OPTION_SPECIFY_CREDENTIALS = "Specify Credentials";
            public static string OPTION_FORMAT_HTML = "HTML";
            public static string OPTION_FORMAT_TEXT = "Text";

            public static string GROUP_SMTP = "SMTP Email Settings";
            public static string GROUP_SERVER = "SMTP Server";
            public static string GROUP_AUTHENTICATION = "Authentication";
            public static string GROUP_MESSAGE_FORMAT = "Message Format";

            public static string BUTTON_TEST_EMAIL = "Send Test Email";
        }

        public struct UIMessages
        {
            public static string EMAIL_SUCCESS = ((int)eMIDTextCode.msg_Email_Test_Email_Successful).ToString() + ": " + MIDText.GetTextOnly(eMIDTextCode.msg_Email_Test_Email_Successful); //"Email message was sent successfully to MID Retail.";
        }
 

        private GlobalOptions_SMTP_BL _SMTP_Options;

        //provide a way to bind the control to the field, so we can use ErrorProvider more easily
        List<System.Windows.Forms.Control> controlList = new List<System.Windows.Forms.Control>();
        

        //provide a list of error providers
        List<ErrorProvider> errorProviderList = new List<ErrorProvider>();

        public void Load_UI_From_BL(GlobalOptions_SMTP_BL m)
        {
            this._SMTP_Options = m;
            //Load non field text
            this.grpSMTP_Settings.Text = controlText.GROUP_SMTP;
            this.grpSMTP_Server.Text = controlText.GROUP_SERVER; 
            this.grpSMTP_Authentication.Text = controlText.GROUP_AUTHENTICATION;
            this.grpSMTP_MessageFormat.Text = controlText.GROUP_MESSAGE_FORMAT;

            this.radSMTP_UseDefaultCredentials.Text = controlText.OPTION_USE_DEFAULT_CREDENTIALS;
            this.radSMTP_SpecifyCredentials.Text = controlText.OPTION_SPECIFY_CREDENTIALS;
            this.radSMTP_FormatHTML.Text = controlText.OPTION_FORMAT_HTML;
            this.radSMTP_FormatText.Text = controlText.OPTION_FORMAT_TEXT;

            this.btnSMTP_SendTestEmail.Text = controlText.BUTTON_TEST_EMAIL;

            //Set the control values based on the business layer object values
            //Bind the fields to the controls via the tag object
            //Add the controls to the control list
            this.chkSMTP_Enable.Checked = m.SMTP_Enabled.Value;
            this.chkSMTP_Enable.Tag = m.SMTP_Enabled;
            controlList.Add(chkSMTP_Enable); 

            this.txtSMTP_Server.Text = m.SMTP_Server.Value;
            this.txtSMTP_Server.Tag = m.SMTP_Server;
            controlList.Add(txtSMTP_Server); 

            this.txtSMTP_Port.Text = m.SMTP_Port.Value.ToString();
            this.txtSMTP_Port.Tag = m.SMTP_Port;
            controlList.Add(txtSMTP_Port);

            this.chkSMTP_SSL.Checked = m.SMTP_Use_SSL.Value;
            this.chkSMTP_SSL.Tag = m.SMTP_Use_SSL;
            controlList.Add(chkSMTP_SSL);

            this.radSMTP_UseDefaultCredentials.Checked = m.SMTP_Use_Default_Credentials.Value;
            this.radSMTP_UseDefaultCredentials.Tag = m.SMTP_Use_Default_Credentials;
            controlList.Add(radSMTP_UseDefaultCredentials);

            this.radSMTP_SpecifyCredentials.Checked = !m.SMTP_Use_Default_Credentials.Value;

            this.txtSMTP_UserName.Text = m.SMTP_UserName.Value;
            this.txtSMTP_UserName.Tag = m.SMTP_UserName;
            controlList.Add(txtSMTP_UserName);


            this.txtSMTP_Pwd.Text = m.SMTP_Pwd.Value;
            this.txtSMTP_Pwd.Tag = m.SMTP_Pwd;
            controlList.Add(txtSMTP_Pwd);

            this.radSMTP_FormatHTML.Checked = m.SMTP_MessageFormatInHTML.Value;
            this.radSMTP_FormatHTML.Tag = m.SMTP_MessageFormatInHTML;
            controlList.Add(radSMTP_FormatHTML);


            this.radSMTP_FormatText.Checked = !m.SMTP_MessageFormatInHTML.Value;


            this.chkSMTP_UseOutlookContacts.Checked = m.SMTP_Use_Outlook_Contacts.Value;
            this.chkSMTP_UseOutlookContacts.Tag = m.SMTP_Use_Outlook_Contacts;
            controlList.Add(chkSMTP_UseOutlookContacts);

            //Begin TT#3600 -jsobek -Add a default email address on the global options screen...
            this.txtSMTP_From_Address.Text = m.SMTP_From_Address.Value;
            this.txtSMTP_From_Address.Tag = m.SMTP_From_Address;
            controlList.Add(txtSMTP_From_Address);
            //End TT#3600 -jsobek -Add a default email address on the global options screen...

        }

        /// <summary>
        /// Attempts to save the UI values to the business layer object
        /// If a value is invalid, it will not be saved, but will have an invalid message in the field
        /// </summary>
        /// <param name="m"></param>
        public void Save_UI_To_BL(GlobalOptions_SMTP_BL m)
        {
            m.SMTP_Enabled.SetValue(this.chkSMTP_Enable.Checked);
            m.SMTP_Server.SetValue(this.txtSMTP_Server.Text.Trim());
            m.SMTP_Port.SetValue(this.txtSMTP_Port.Text);
            m.SMTP_Use_SSL.SetValue(this.chkSMTP_SSL.Checked);
            m.SMTP_Use_Default_Credentials.SetValue(this.radSMTP_UseDefaultCredentials.Checked); 
            m.SMTP_UserName.SetValue(this.txtSMTP_UserName.Text.Trim());
            m.SMTP_Pwd.SetValue(this.txtSMTP_Pwd.Text.Trim());
            m.SMTP_MessageFormatInHTML.SetValue(this.radSMTP_FormatHTML.Checked);
            m.SMTP_Use_Outlook_Contacts.SetValue(this.chkSMTP_UseOutlookContacts.Checked);
            m.SMTP_From_Address.SetValue(this.txtSMTP_From_Address.Text.Trim()); //TT#3600 -jsobek -Add a default email address on the global options screen...
        }

        /// <summary>
        /// Load the values from the database row
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="m"></param>
        private void SettingsLoadFromDataRow(DataRow dr, GlobalOptions_SMTP_BL m)
        {
            m.LoadFromDataRow(dr);
            Load_UI_From_BL(m);
        }

        /// <summary>
        /// Checks if all the controls in this user control have valid values
        /// If not, it creates an errorProvider in a list along with setting the invalid message
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool IsValid(GlobalOptions_SMTP_BL m)
        {
            Save_UI_To_BL(m);  //must put the UI values to the business layer object first
            bool valid = true;

            //Clear any previous errors
            foreach (System.Windows.Forms.ErrorProvider errProvider in this.errorProviderList)
            {
                errProvider.Clear();
                errProvider.Dispose();
            }
            this.errorProviderList.Clear();

            //Validate each of the controls
            foreach (System.Windows.Forms.Control c in controlList)
            {
                baseField b = (baseField)c.Tag; 
                if (b.HasInvalidMessage)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(c, b.InvalidMessage);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
                //also check for required values after other validation
                else if (b.HasRequiredValue() == false)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(c, b.InvalidMessage);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
            }

            return valid;
        }

        public void DoEnable()
        {
            //set the default enabled state
            this.chkSMTP_Enable.Enabled = true;
            this.txtSMTP_Server.Enabled = false;
            this.txtSMTP_Port.Enabled = false;
            this.radSMTP_UseDefaultCredentials.Enabled = false;
            this.radSMTP_SpecifyCredentials.Enabled = false;
            this.txtSMTP_UserName.Enabled = false;
            this.txtSMTP_Pwd.Enabled = false;
            this.btnSMTP_SendTestEmail.Enabled = false;
            this.radSMTP_FormatHTML.Enabled = false;
            this.radSMTP_FormatText.Enabled = false;
            this.chkSMTP_SSL.Enabled = false;
            this.txtSMTP_From_Address.Enabled = false; //TT#3600 -jsobek -Add a default email address on the global options screen...
        }

        public void SetReadOnly()
        {
            //disable all controls
            this.chkSMTP_Enable.Enabled = false;
            this.txtSMTP_Server.Enabled = false;
            this.txtSMTP_Port.Enabled = false;
            this.radSMTP_UseDefaultCredentials.Enabled = false;
            this.radSMTP_SpecifyCredentials.Enabled = false;
            this.txtSMTP_UserName.Enabled = false;
            this.txtSMTP_Pwd.Enabled = false;
            this.btnSMTP_SendTestEmail.Enabled = false;
            this.radSMTP_FormatHTML.Enabled = false;
            this.radSMTP_FormatText.Enabled = false;
            this.chkSMTP_SSL.Enabled = false;
            this.txtSMTP_From_Address.Enabled = false; //TT#3600 -jsobek -Add a default email address on the global options screen...
        }


        public MIDRetail.Business.SessionAddressBlock _SAB = null;

        /// <summary>
        /// Attempts to send a test email to MID.  If unsucessful, display any error messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSMTP_SendTestEmail_Click(object sender, EventArgs e)
        {
            if (IsValid(this._SMTP_Options) == false)
            {
                return;
            }

            string emailReturnMsg = String.Empty;
            string emailEnvironmentalInfo = EnvironmentInfo.MIDInfo.GetAllEnvironmentInfo(Environment.NewLine);
            string emailEnvironmentalBusinessInfo = String.Empty;
            if (_SAB != null)
            {
                emailEnvironmentalBusinessInfo = MIDRetail.Business.EnvironmentBusinessInfo.GetAllBusinessEnvironmentInfo(_SAB, Environment.NewLine);
            }

            if (MIDEmail.SendEmailTest(out emailReturnMsg, _SMTP_Options.SMTP_Server.Value, _SMTP_Options.SMTP_Port.Value, _SMTP_Options.SMTP_Use_SSL.Value, _SMTP_Options.SMTP_Use_Default_Credentials.Value, _SMTP_Options.SMTP_UserName.Value, _SMTP_Options.SMTP_Pwd.Value, emailEnvironmentalInfo, emailEnvironmentalBusinessInfo) == MIDEmail.emailReturnMessageTypes.Success)
            {
                MessageBox.Show(UIMessages.EMAIL_SUCCESS, MIDEmail.emailReturnMessages.Success);
            }
            else
            {
                MessageBox.Show(emailReturnMsg);
            }
        }


        private void chkSMTP_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkSMTP_Enable.Checked == true)
            {
                this.btnSMTP_SendTestEmail.Enabled = true;

                this.txtSMTP_Server.Enabled = true;
                this.txtSMTP_Port.Enabled = true;
                this.chkSMTP_SSL.Enabled = true;

                this.radSMTP_UseDefaultCredentials.Enabled = true;
                this.radSMTP_SpecifyCredentials.Enabled = true;
                if (this.radSMTP_SpecifyCredentials.Checked == true)
                {
                    this.txtSMTP_UserName.Enabled = true;
                    this.txtSMTP_Pwd.Enabled = true;
                }
                else
                {
                    this.txtSMTP_UserName.Enabled = false;
                    this.txtSMTP_Pwd.Enabled = false;
                }

                this.radSMTP_FormatHTML.Enabled = true;
                this.radSMTP_FormatText.Enabled = true;
                this.txtSMTP_From_Address.Enabled = true; //TT#3600 -jsobek -Add a default email address on the global options screen...
            }
            else
            {
                this.txtSMTP_Server.Enabled = false;
                this.txtSMTP_Port.Enabled = false;
                this.radSMTP_UseDefaultCredentials.Enabled = false;
                this.radSMTP_SpecifyCredentials.Enabled = false;
                this.txtSMTP_UserName.Enabled = false;
                this.txtSMTP_Pwd.Enabled = false;
                this.btnSMTP_SendTestEmail.Enabled = false;
                this.radSMTP_FormatHTML.Enabled = false;
                this.radSMTP_FormatText.Enabled = false;
                this.chkSMTP_SSL.Enabled = false;
                this.txtSMTP_From_Address.Enabled = false; //TT#3600 -jsobek -Add a default email address on the global options screen...
            }
        }



        private void radSMTP_UseDefaultCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radSMTP_UseDefaultCredentials.Checked == true)
            {
                this.txtSMTP_UserName.Enabled = false;
                this.txtSMTP_Pwd.Enabled = false;
            }
            else
            {
                this.txtSMTP_UserName.Enabled = true;
                this.txtSMTP_Pwd.Enabled = true;
            }
        }

        private void radSMTP_SpecifyCredentials_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radSMTP_UseDefaultCredentials.Checked == true)
            {
                this.txtSMTP_UserName.Text = String.Empty;
                this.txtSMTP_Pwd.Text = String.Empty;
                this.txtSMTP_UserName.Enabled = false;
                this.txtSMTP_Pwd.Enabled = false;
            }
            else
            {
                this.txtSMTP_UserName.Enabled = true;
                this.txtSMTP_Pwd.Enabled = true;
            }
        }

        //Example of immediate validation
        private void txtSMTP_Port_Validating(object sender, CancelEventArgs e)
        {
            if (_SMTP_Options.SMTP_Port.IsNewValueValid(txtSMTP_Port.Text) == false)
            {
                System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                errProvider.SetError(this.txtSMTP_Port, _SMTP_Options.SMTP_Port.InvalidMessage);
                this.errorProviderList.Add(errProvider);
                e.Cancel = true;
            }
        }

       

      

 
    }
}
