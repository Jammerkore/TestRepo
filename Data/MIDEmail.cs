using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using MIDRetail.DataCommon;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.InteropServices;
//using Microsoft.Office.Interop;

namespace MIDRetail.Data
{

    public static class MIDEmail
    {
        public const string MIDEmailSupportTOAddress = "support@midretail.com";
        //public const string MIDEmailApplicationFromAddress = "app@midretail.com"; //TT#3600 -jsobek -Add a default email address on the global options screen...

        public enum emailReturnMessageTypes
        {
            Success,
            From_Address_Required,
            From_Address_Invalid,
            To_Address_Required,
            To_Address_Invalid,
            CC_Address_Invalid,
            BCC_Address_Invalid,
            Subject_Required,
            Body_Required,
            Attachments_Invalid,
            SMTP_SettingsNotSetup,
            SMTP_ConnectionIssue,
            SMTP_SendingEmailIssue
        }
        public enum emailPriorityEnum
        {
            Low,
            Normal,
            High
        }

        //Soft Text Messages
        public struct emailReturnMessages
        {
            public static string Success = "Success";
            public static string From_Address_Required = ((int)eMIDTextCode.msg_Email_From_Address_Required).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_From_Address_Required));
            public static string From_Address_Invalid    =((int)eMIDTextCode.msg_Email_From_Address_Invalid).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_From_Address_Invalid));
            public static string To_Address_Required     =((int)eMIDTextCode.msg_Email_To_Address_Required).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_To_Address_Required));
            public static string To_Address_Invalid      =((int)eMIDTextCode.msg_Email_To_Address_Invalid).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_To_Address_Invalid));
            public static string CC_Address_Invalid      =((int)eMIDTextCode.msg_Email_CC_Address_Invalid).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_CC_Address_Invalid));
            public static string BCC_Address_Invalid     =((int)eMIDTextCode.msg_Email_BCC_Address_Invalid).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_BCC_Address_Invalid));
            public static string Subject_Required        =((int)eMIDTextCode.msg_Email_Subject_Required).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_Subject_Required));
            public static string Body_Required           =((int)eMIDTextCode.msg_Email_Body_Required).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_Body_Required));
            public static string Attachments_Invalid     =((int)eMIDTextCode.msg_Email_Attachments_Invalid).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_Attachments_Invalid));
            public static string SMTP_SettingsNotSetup   =((int)eMIDTextCode.msg_Email_SMTP_SettingsNotSetup).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_SettingsNotSetup));
            public static string SMTP_ConnectionIssue    =((int)eMIDTextCode.msg_Email_SMTP_ConnectionIssue).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_ConnectionIssue));
            public static string SMTP_SendingEmailIssue  =((int)eMIDTextCode.msg_Email_SMTP_SendingEmailIssue).ToString() + ": " + MIDText.ReplaceControlChars(MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_SendingEmailIssue));
        }

        public static bool UseOutlookContacts
        {
            get { Load_SMTP_Options();  return _SMTP_Options.SMTP_Use_Outlook_Contacts.Value; }
            private set { }
        }

        
        //Begin TT#3624 -jsobek -Unhandled Exception opening Task List when "Use Outlook Contact Information" is checked on 
        public static bool IsOutlookInstalled()
        {
            Type officeType = Type.GetTypeFromProgID("Outlook.Application");

            if (officeType == null)
            {
                // Outlook is not installed.   
                return false;
            }
            else
            {
                // Outlook is installed.    
                return true;
            }
        }
        //End TT#3624 -jsobek -Unhandled Exception opening Task List when "Use Outlook Contact Information" is checked on 

        /// <summary>
        /// Creates a mail message and attempts to send it
        /// Validates email From, TO, CC, and BCC addresses
        /// Requires that messages contain a subject and body.
        /// </summary>
        /// <param name="emailReturnMessage"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="emailCC"></param>
        /// <param name="emailBCC"></param>
        /// <param name="emailAttachmentFileFullPaths"></param>
        /// <param name="emailPriority"></param>
        /// <returns></returns>
        public static emailReturnMessageTypes SendEmail(out string emailReturnMessage, string emailSubject, string emailBody, string emailFrom, string emailTo, string emailCC = null, string emailBCC = null,  string[] emailAttachmentFileFullPaths = null, emailPriorityEnum emailPriority = emailPriorityEnum.Normal)
        {
            emailReturnMessageTypes returnMessageType; 
            MailMessage email = CreateMailMessage(out returnMessageType, out emailReturnMessage, emailSubject, emailBody, emailFrom, emailTo, emailCC, emailBCC, emailAttachmentFileFullPaths, emailPriority);

            //Do not send the email if validation failed.
            if (returnMessageType != emailReturnMessageTypes.Success)
            {
                return returnMessageType;
            }
            else 
            {
                return SendEmail(out emailReturnMessage, email);
            }
        }

        /// <summary>
        /// Creates a mail message 
        /// Validates email From, TO, CC, and BCC addresses
        /// Requires that messages contain a subject and body.
        /// </summary>
        /// <param name="emailReturnMessageType"></param>
        /// <param name="emailReturnMessage"></param>
        /// <param name="emailSubject"></param>
        /// <param name="emailBody"></param>
        /// <param name="emailFrom"></param>
        /// <param name="emailTo"></param>
        /// <param name="emailCC"></param>
        /// <param name="emailBCC"></param>
        /// <param name="emailAttachmentFileFullPaths"></param>
        /// <param name="emailPriority"></param>
        /// <returns></returns>
        public static MailMessage CreateMailMessage(out emailReturnMessageTypes emailReturnMessageType, out string emailReturnMessage, string emailSubject, string emailBody, string emailFrom, string emailTo, string emailCC = null, string emailBCC = null, string[] emailAttachmentFileFullPaths = null, emailPriorityEnum emailPriority = emailPriorityEnum.Normal)
        {
            emailReturnMessage = String.Empty;
            
            //Validate subject
            if (emailSubject == null)
            {
                emailReturnMessage = emailReturnMessages.Subject_Required;
                emailReturnMessageType = emailReturnMessageTypes.Subject_Required;
                return null;
            }
            if (emailSubject == String.Empty)
            {
                emailReturnMessage = emailReturnMessages.Subject_Required;
                emailReturnMessageType = emailReturnMessageTypes.Subject_Required;
                return null;
            }

            //Validate body
            if (emailBody == null)
            {
                emailReturnMessage = emailReturnMessages.Body_Required;
                emailReturnMessageType = emailReturnMessageTypes.Body_Required;
                return null;
            }
            if (emailBody == String.Empty)
            {
                emailReturnMessage = emailReturnMessages.Body_Required;
                emailReturnMessageType = emailReturnMessageTypes.Body_Required;
                return null;
            }

            //Validate from address
            //if (emailFrom == null)
            //{
            //    emailReturnMessage = emailReturnMessages.From_Address_Required;
            //    emailReturnMessageType = emailReturnMessageTypes.From_Address_Required;
            //    return null;
            //}
            //if (emailFrom == String.Empty)
            //{
            //    emailReturnMessage = emailReturnMessages.From_Address_Required;
            //    emailReturnMessageType = emailReturnMessageTypes.From_Address_Required;
            //    return null;
            //}
            if (emailFrom == null || emailFrom == String.Empty)
            {
                //use the default email address from global options
                emailFrom = GetFromAddressFromOptions();
            }

            if (IsAddressValid(emailFrom) == false)
            {
                emailReturnMessage = emailReturnMessages.From_Address_Invalid;
                emailReturnMessageType = emailReturnMessageTypes.From_Address_Invalid;
                return null;
            }

            //Validate To address
            if (emailTo == null)
            {
                emailReturnMessage = emailReturnMessages.To_Address_Required;
                emailReturnMessageType = emailReturnMessageTypes.To_Address_Required;
                return null;
            }
            if (emailTo == String.Empty)
            {
                emailReturnMessage = emailReturnMessages.To_Address_Required;
                emailReturnMessageType = emailReturnMessageTypes.To_Address_Required;
                return null;
            }
            string[] to_addresses = emailTo.Split(';');


            foreach (string emailAddress in to_addresses)
            {
                if (emailAddress != string.Empty)
                {
                    if (IsAddressValid(emailAddress) == false)
                    {
                        emailReturnMessage = emailReturnMessages.To_Address_Invalid;
                        emailReturnMessageType = emailReturnMessageTypes.To_Address_Invalid;
                        return null;
                    }
                }
            }

            //Validate CC address
            if (emailCC != null && emailCC != String.Empty)
            {

                string[] cc_addresses = emailCC.Split(';');


                foreach (string emailAddress in cc_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        if (IsAddressValid(emailAddress) == false)
                        {
                            emailReturnMessage = emailReturnMessages.CC_Address_Invalid;
                            emailReturnMessageType = emailReturnMessageTypes.CC_Address_Invalid;
                            return null;
                        }
                    }
                }
            }

            //Validate BCC address
            if (emailBCC != null && emailBCC != String.Empty)
            {

                string[] bcc_addresses = emailBCC.Split(';');


                foreach (string emailAddress in bcc_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        if (IsAddressValid(emailAddress) == false)
                        {
                            emailReturnMessage = emailReturnMessages.BCC_Address_Invalid;
                            emailReturnMessageType = emailReturnMessageTypes.BCC_Address_Invalid;
                            return null;
                        }
                    }
                }
            }

             
            MailMessage email = new MailMessage();
            email.Subject = emailSubject;
            email.Body = emailBody;
            email.From = new MailAddress(GetSMTPAddressFromExtendedAddress(emailFrom));

            foreach (string emailAddress in to_addresses)
            {
                if (emailAddress != string.Empty)
                {
                    email.To.Add(GetSMTPAddressFromExtendedAddress(emailAddress));
                }
            }

            if (email.To.Count == 0)
            {
                emailReturnMessage = emailReturnMessages.To_Address_Invalid;
                emailReturnMessageType = emailReturnMessageTypes.To_Address_Invalid;
                return null;
            }


            if (emailCC != null && emailCC != String.Empty)
            {
                string[] cc_addresses = emailCC.Split(';');

                foreach (string emailAddress in cc_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        email.CC.Add(GetSMTPAddressFromExtendedAddress(emailAddress));
                    }
                }
            }
            if (emailBCC != null && emailBCC != String.Empty)
            {
                string[] bcc_addresses = emailBCC.Split(';');

                foreach (string emailAddress in bcc_addresses)
                {
                    if (emailAddress != string.Empty)
                    {
                        email.Bcc.Add(GetSMTPAddressFromExtendedAddress(emailAddress));
                    }
                }
            }
            SetEmailFormatting(email);

            //Add and validate attachments
            if (emailAttachmentFileFullPaths != null)
            {
                foreach (string attachmentFullFilePath in emailAttachmentFileFullPaths)
                {
                    try
                    {
                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(attachmentFullFilePath);
                        email.Attachments.Add(attachment);
                    }
                    catch (Exception ex)
                    {
                        emailReturnMessage = emailReturnMessages.Attachments_Invalid.Replace("{0}", ex.ToString());
                        emailReturnMessageType = emailReturnMessageTypes.Attachments_Invalid;
                        return null;
                    }
                }
            }


            emailReturnMessage = emailReturnMessages.Success;
            emailReturnMessageType = emailReturnMessageTypes.Success;
            return email;
        }

        /// <summary>
        /// Returns the SMTP address portion of an extended email address.
        /// Example: "John Smith (jsmith@midretail.com)" would return "jsmith@midretail.com"
        /// </summary>
        /// <param name="extendedAddress"></param>
        /// <returns></returns>
        private static string GetSMTPAddressFromExtendedAddress(string extendedAddress)
        {
            string SMTPEmailAddress = extendedAddress;
            //allow outlook style email addresses with the format: full name (emailaddress)
            if (extendedAddress.Contains(emailExtendedAddressStartIndicator) && extendedAddress.Contains(emailExtendedAddressEndIndicator))
            {
                int start = extendedAddress.IndexOf(emailExtendedAddressStartIndicator) + 1;
                int end = extendedAddress.IndexOf(emailExtendedAddressEndIndicator);
                int length = end - start;
                SMTPEmailAddress = extendedAddress.Substring(start, length);
            }
            return SMTPEmailAddress;
        }


        private static MIDRetail.Data.GlobalOptions_SMTP_BL _SMTP_Options = null;

        private static void Load_SMTP_Options()
        {
            if (_SMTP_Options == null)
            {
                GlobalOptions globalOptions = new GlobalOptions();
                System.Data.DataTable dtGlobalOptions = globalOptions.GetGlobalOptions();

                _SMTP_Options = new MIDRetail.Data.GlobalOptions_SMTP_BL();
                _SMTP_Options.LoadFromDataRow(dtGlobalOptions.Rows[0]);
            }
        }

        /// <summary>
        /// Loads the options from the database if needed
        /// Checks to ensure at least the SMTP server name is set
        /// </summary>
        /// <returns></returns>
        public static bool Is_SMTP_Configured()
        {
            Load_SMTP_Options();
            //Use the SMTP_Server to determine if options were configured
            if (_SMTP_Options.SMTP_Server.Value == String.Empty)
            {
                return false;
            }
            else
            {
                return true;
            }

            
        }

        public static SmtpClient GetEmailClient(string aSMTP_Server, int aSMTP_Port, bool aSMTP_Use_SSL, bool aSMTP_Use_Default_Credentials, string aSMTP_UserName, string aSMTP_Pwd)
        {
            SmtpClient client = new SmtpClient(aSMTP_Server);
            client.Port = aSMTP_Port;
            client.EnableSsl = aSMTP_Use_SSL;
            if (aSMTP_Use_Default_Credentials == true)
            {
                client.UseDefaultCredentials = true; 
            }
            else
            {
                client.Credentials = new System.Net.NetworkCredential(aSMTP_UserName, aSMTP_Pwd);
            }
            return client;
        }

        private static void SetEmailFormatting(MailMessage email)
        {
            Load_SMTP_Options();
            email.IsBodyHtml = _SMTP_Options.SMTP_MessageFormatInHTML.Value; 
        }
        private static string GetFromAddressFromOptions()
        {
            Load_SMTP_Options();
            return _SMTP_Options.SMTP_From_Address.Value;
        }


        public static emailReturnMessageTypes SendEmail(out string emailReturnMessage, MailMessage email)
        {
            //Load_SMTP_Options();
            
            if (Is_SMTP_Configured() == false)
            {
                    emailReturnMessage = emailReturnMessages.SMTP_SettingsNotSetup;
                    return emailReturnMessageTypes.SMTP_SettingsNotSetup;
            }
          
      
            SmtpClient client = new SmtpClient(_SMTP_Options.SMTP_Server.Value);
            client.Port = _SMTP_Options.SMTP_Port.Value ;
            client.EnableSsl = _SMTP_Options.SMTP_Use_SSL.Value; 

            try
            {
                if (_SMTP_Options.SMTP_Use_Default_Credentials.Value == true)
                {
                    client.UseDefaultCredentials = true; 
                }
                else
                {
                    client.Credentials = new System.Net.NetworkCredential(_SMTP_Options.SMTP_UserName.Value , _SMTP_Options.SMTP_Pwd.Value);
                }
                
                
            }
            catch (Exception ex)
            {
                emailReturnMessage = emailReturnMessages.SMTP_ConnectionIssue.Replace("{0}", ex.ToString());
                return emailReturnMessageTypes.SMTP_ConnectionIssue;
            }

            
            try
            {
                SetEmailFormatting(email);
                client.Send(email);
            }
            catch (Exception ex)
            {
                emailReturnMessage = emailReturnMessages.SMTP_SendingEmailIssue.Replace("{0}", ex.ToString());
                return emailReturnMessageTypes.SMTP_SendingEmailIssue;
            }


            emailReturnMessage = emailReturnMessages.Success;
            return emailReturnMessageTypes.Success;
        }


        

       

        /// <summary>
        /// Creates a test email message and sends it via the client parameters that are provided
        /// </summary>
        /// <param name="emailReturnMessage"></param>
        /// <param name="aSMTP_Server"></param>
        /// <param name="aSMTP_Port"></param>
        /// <param name="aSMTP_Use_Default_Credentials"></param>
        /// <param name="aSMTP_UserName"></param>
        /// <param name="aSMTP_Pwd"></param>
        /// <returns></returns>
        public static emailReturnMessageTypes SendEmailTest(out string emailReturnMessage, string aSMTP_Server, int aSMTP_Port, bool aSMTP_Use_SSL, bool aSMTP_Use_Default_Credentials, string aSMTP_UserName, string aSMTP_Pwd, string environmentInfoToAttach = "", string environmentBusinessInfoToAttach = "")
        {
            try
            {
                SmtpClient client = GetEmailClient(aSMTP_Server, aSMTP_Port, aSMTP_Use_SSL, aSMTP_Use_Default_Credentials, aSMTP_UserName, aSMTP_Pwd);
                client.Send(CreateEmailTestMessage(environmentInfoToAttach, environmentBusinessInfoToAttach));
            }
            catch (Exception ex)
            {
                emailReturnMessage = emailReturnMessages.SMTP_SendingEmailIssue.Replace("{0}", ex.ToString());
                return emailReturnMessageTypes.SMTP_SendingEmailIssue;
            }
           


            emailReturnMessage = emailReturnMessages.Success;
            return emailReturnMessageTypes.Success;
        }

     
        /// <summary>
        /// Creates a test email message
        /// </summary>
        /// <returns></returns>
        public static MailMessage CreateEmailTestMessage(string environmentInfoToAttach="", string environmentBusinessInfoToAttach="" )
        {
            MailMessage email = new MailMessage();
            email.Subject = "MID SMTP Email Test Message";
            email.Body = CreateTestEmailBody(true);
            email.To.Add(MIDEmailSupportTOAddress);


            string emailFrom = GetFromAddressFromOptions(); //TT#3600 -jsobek -Add a default email address on the global options screen...
            email.From = new MailAddress(emailFrom); //TT#3600 -jsobek -Add a default email address on the global options screen...
            email.IsBodyHtml = true;

            if (environmentInfoToAttach != String.Empty || environmentBusinessInfoToAttach != String.Empty)
            {
                if (environmentBusinessInfoToAttach != String.Empty)
                {
                    environmentInfoToAttach += Environment.NewLine + environmentBusinessInfoToAttach;
                }

            
                System.Net.Mail.Attachment attachment;
                byte[] byteArray = Encoding.ASCII.GetBytes(environmentInfoToAttach);
                System.IO.MemoryStream envAttachment = new System.IO.MemoryStream(byteArray);
                attachment = new System.Net.Mail.Attachment(envAttachment, "MIDEnvironmentInfo.txt");
                email.Attachments.Add(attachment);

            }

            return email;
        }
        public static MailMessage CreateEmailSystemMessage(DateTime messageTime, string messageModule, int messageLevel, string messageLevelText, int messageCode, string messageText, string messageDetails, string environmentInfoToAttach = "", string environmentBusinessInfoToAttach = "", string emailFrom = "", string emailTo = "", string emailCC = "", string emailBCC = "", string emailSubject="", string emailBody="", string emailErrorAttachmentFileName = "")
        {
            MailMessage email = new MailMessage();
            email.IsBodyHtml = true;

            if (emailFrom == string.Empty)
            {
                //use default system error message From
                emailFrom = GetFromAddressFromOptions(); //TT#3600 -jsobek -Add a default email address on the global options screen...
            }
            email.From = new MailAddress(emailFrom);
            if (emailTo != string.Empty)
            {
                email.To.Add(emailTo);
            }
            else
            {
                //use default system error message To
                email.To.Add(MIDEmailSupportTOAddress);
            }
            if (emailCC != string.Empty)
            {
                email.CC.Add(emailCC);
            }
            if (emailBCC != string.Empty)
            {
                email.Bcc.Add(emailBCC);
            }
            if (emailSubject != string.Empty)
            {
                email.Subject = emailSubject;
            }
            else
            {
                //create default system error message subject
                email.Subject = CreateSystemMessageSubject(messageTime, messageModule, messageLevel, messageLevelText, messageCode, messageText, messageDetails);
            }
            if (emailBody != string.Empty)
            {
                email.Body = emailBody;
            }
            else
            {
                //create default system error message body
                email.Body = CreateSystemMessageTextBlock(true, messageTime, messageModule, messageLevel, messageLevelText, messageCode, messageText, messageDetails);
            }


            if (environmentInfoToAttach != String.Empty || environmentBusinessInfoToAttach != String.Empty)
            {
                if (environmentBusinessInfoToAttach != String.Empty)
                {
                    environmentInfoToAttach += Environment.NewLine + environmentBusinessInfoToAttach;
                }


                System.Net.Mail.Attachment attachment;
                byte[] byteArray = Encoding.ASCII.GetBytes(environmentInfoToAttach);
                System.IO.MemoryStream envAttachment = new System.IO.MemoryStream(byteArray);
                attachment = new System.Net.Mail.Attachment(envAttachment, "MIDEnvironmentInfo.txt");
                email.Attachments.Add(attachment);

            }
            //create the error message also as a txt attachment
            if (emailErrorAttachmentFileName == string.Empty)
            {
                emailErrorAttachmentFileName = "SystemMessage.txt";
            }
            string errorMessageAttachment = CreateSystemMessageTextBlock(false, messageTime, messageModule, messageLevel, messageLevelText, messageCode, messageText, messageDetails);
            System.Net.Mail.Attachment attachmentErrorMessage;
            byte[] byteArrayErrorMessage = Encoding.ASCII.GetBytes(errorMessageAttachment);
            System.IO.MemoryStream steamErrorMessage = new System.IO.MemoryStream(byteArrayErrorMessage);
            attachmentErrorMessage = new System.Net.Mail.Attachment(steamErrorMessage, emailErrorAttachmentFileName);
            email.Attachments.Add(attachmentErrorMessage);

            return email;
        }

        private static string CreateSystemMessageTextBlock(bool use_HTML_Formatting, DateTime messageTime, string messageModule, int messageLevel, string messageLevelText, int messageCode, string messageText, string messageDetails)
        {
            string newline;
            if (use_HTML_Formatting)
            {
                newline = "<br />";
            }
            else
            {
                newline = Environment.NewLine;
            }


            string s = string.Empty;
            s += "System Message" + newline + newline;

            s += CreateBasicUserInfoTextBlock(newline);
           

            s += "Time: " + messageTime.ToString("yyyy-MM-dd") + " " + messageTime.ToLongTimeString() + newline;
            s += "Module: " + messageModule + newline;
            s += "Level Code: " + messageLevel.ToString() + newline;
            s += "Level: " + messageLevelText + newline;
            s += "Message Code: " + messageCode + newline;
            s += "Message: " + messageText + newline;
            s += "Details: " + messageDetails + newline;

            return s;
        }
        private static string CreateSystemMessageSubject(DateTime messageTime, string messageModule, int messageLevel, string messageLevelText, int messageCode, string messageText, string messageDetails)
        {

            string s = string.Empty;
            s += "System Message-";
            //s += "Time: " + messageTime.ToString("yyyy-MM-dd") + " " + messageTime.ToLongTimeString();
            //s += "Module: " + messageModule;
            //s += "Level Code: " + messageLevel.ToString();
            s += "Level: " + messageLevelText;
            //s += " Message Code: " + messageCode;
            s += " Message: " + messageText;
            //s += "Details: " + messageDetails;


            return s;
        }

       


        /// <summary>
        /// Creates the body of the test email and returns it as a string
        /// </summary>
        /// <param name="use_HTML_Formatting"></param>
        /// <returns></returns>
        public static string CreateTestEmailBody(bool use_HTML_Formatting)
        {
            string s = String.Empty;

            string newline;
            if (use_HTML_Formatting)
            {
                newline = "<br />";
            }
            else
            {
                newline = Environment.NewLine;
            }

            s += "Test Email." + newline + newline;

            s += CreateBasicUserInfoTextBlockWithDateTime(newline);

            return s;
        }

        public static string CreateBasicUserInfoTextBlock(string newline)
        {
            string s = String.Empty;
            s += EnvironmentInfo.MIDInfo.userNameLabelText + EnvironmentInfo.MIDInfo.userName + newline;
            s += EnvironmentInfo.MIDInfo.machineNameLabelText + EnvironmentInfo.MIDInfo.machineName + newline;
            s += EnvironmentInfo.MIDInfo.domainNameLabelText + EnvironmentInfo.MIDInfo.domainName + newline;

            return s;
        }
        public static string CreateBasicUserInfoTextBlockWithDateTime(string newline)
        {
            string s = String.Empty;
            s += CreateBasicUserInfoTextBlock(newline);
            s += EnvironmentInfo.MIDInfo.machineDateTimeLabelText + EnvironmentInfo.MIDInfo.machineDateTime + newline;

            return s;
        }
        public static string GetNewline()
        {
            string newline;
            Load_SMTP_Options();
            if (_SMTP_Options.SMTP_MessageFormatInHTML.Value == true)
            {
                newline = "<br />";
            }
            else
            {
                newline = Environment.NewLine;
            }
            return newline;
        }

        public static List<string> GetOutlookContractList()
        {
            if (outlookContactList == null)
            {
                Microsoft.Office.Interop.Outlook.Application outlookApp = null;
                try
                {
                    outlookApp = new Microsoft.Office.Interop.Outlook.Application();
                    //List<string> outlookContactList = null;
                    Microsoft.Office.Interop.Outlook.Items folderItems = null;
                    Microsoft.Office.Interop.Outlook.MAPIFolder folderSuggestedContacts = null;
                    Microsoft.Office.Interop.Outlook.NameSpace ns = null;
                    Microsoft.Office.Interop.Outlook.MAPIFolder folderContacts = null;
                    object itemObj = null;
                    try
                    {
                        outlookContactList = new List<string>();
                        ns = outlookApp.GetNamespace("MAPI");
                        // getting items from the Contacts folder in Outlook 
                        folderContacts = ns.GetDefaultFolder(Microsoft.Office.Interop.Outlook.OlDefaultFolders.olFolderContacts);
                        folderItems = folderContacts.Items;
                        for (int i = 1; folderItems.Count >= i; i++)
                        {
                            itemObj = folderItems[i];
                            if (itemObj is Microsoft.Office.Interop.Outlook.ContactItem)
                            {
                                string emailAddressType1 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email1AddressType;
                                string emailContactFullName1 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email1DisplayName;
                                string emailAddress1 = string.Empty;
                                if (emailAddressType1 != string.Empty && emailAddressType1 != null)
                                {
                                    if (emailAddressType1 == emailAddressTypeSMTP)
                                    {
                                        emailAddress1 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email1Address;
                                    }
                                    else if (emailAddressType1 == emailAddressTypeExchange)
                                    {
                                        Microsoft.Office.Interop.Outlook.Recipient recipient = ns.CreateRecipient(((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email1Address);
                                        if (recipient != null)
                                        {
                                            try
                                            {
                                                if (recipient.Resolve())
                                                {
                                                    Microsoft.Office.Interop.Outlook.ExchangeUser exUser = null;
                                                    try
                                                    {
                                                        exUser = recipient.AddressEntry.GetExchangeUser();
                                                        if (exUser != null)
                                                        {
                                                            emailAddress1 = exUser.PrimarySmtpAddress;
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        if (exUser != null)
                                                        {
                                                            Marshal.ReleaseComObject(exUser);
                                                        }
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                if (recipient != null)
                                                {
                                                    Marshal.ReleaseComObject(recipient);
                                                }
                                            }
                                        }


                                    }
                                }

                                if (emailAddress1 != string.Empty)
                                {
                                    outlookContactList.Add(emailContactFullName1 + " " + emailExtendedAddressStartIndicator + emailAddress1 + emailExtendedAddressEndIndicator + ";");
                                }



                                //2nd email address
                                string emailAddressType2 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email2AddressType;
                                string emailContactFullName2 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email2DisplayName;
                                string emailAddress2 = string.Empty;
                                if (emailAddressType2 != string.Empty && emailAddressType2 != null)
                                {
                                    if (emailAddressType2 == emailAddressTypeSMTP)
                                    {
                                        emailAddress2 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email2Address;
                                    }
                                    else if (emailAddressType2 == emailAddressTypeExchange)
                                    {
                                        Microsoft.Office.Interop.Outlook.Recipient recipient = ns.CreateRecipient(((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email2Address);
                                        if (recipient != null)
                                            try
                                            {
                                                if (recipient.Resolve())
                                                {
                                                    Microsoft.Office.Interop.Outlook.ExchangeUser exUser = null;
                                                    try
                                                    {
                                                        exUser = recipient.AddressEntry.GetExchangeUser();
                                                        if (exUser != null)
                                                        {
                                                            emailAddress2 = exUser.PrimarySmtpAddress;
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        if (exUser != null)
                                                        {
                                                            Marshal.ReleaseComObject(exUser);
                                                        }
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                if (recipient != null)
                                                {
                                                    Marshal.ReleaseComObject(recipient);
                                                }
                                            }
                                    }
                                }

                                if (emailAddress2 != string.Empty)
                                {
                                    outlookContactList.Add(emailContactFullName2 + " " + emailExtendedAddressStartIndicator + emailAddress2 + emailExtendedAddressEndIndicator + ";");
                                }

                                //3rd email address
                                string emailAddressType3 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email3AddressType;
                                string emailContactFullName3 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email3DisplayName;
                                string emailAddress3 = string.Empty;
                                if (emailAddressType3 != string.Empty && emailAddressType3 != null)
                                {
                                    if (emailAddressType3 == emailAddressTypeSMTP)
                                    {

                                        emailAddress3 = ((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email3Address;

                                    }
                                    else if (emailAddressType3 == emailAddressTypeExchange)
                                    {
                                        Microsoft.Office.Interop.Outlook.Recipient recipient = ns.CreateRecipient(((Microsoft.Office.Interop.Outlook.ContactItem)itemObj).Email3Address);
                                        if (recipient != null)
                                            try
                                            {
                                                if (recipient.Resolve())
                                                {
                                                    Microsoft.Office.Interop.Outlook.ExchangeUser exUser = null;
                                                    try
                                                    {
                                                        exUser = recipient.AddressEntry.GetExchangeUser();
                                                        if (exUser != null)
                                                        {
                                                            emailAddress3 = exUser.PrimarySmtpAddress;
                                                        }
                                                    }
                                                    finally
                                                    {
                                                        if (exUser != null)
                                                        {
                                                            Marshal.ReleaseComObject(exUser);
                                                        }
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                if (recipient != null)
                                                {
                                                    Marshal.ReleaseComObject(recipient);
                                                }
                                            }
                                    }
                                }

                                if (emailAddress3 != string.Empty)
                                {
                                    outlookContactList.Add(emailContactFullName3 + " " + emailExtendedAddressStartIndicator + emailAddress3 + emailExtendedAddressEndIndicator + ";");
                                }


                            }
                            else
                            {
                                Marshal.ReleaseComObject(itemObj);
                            }
                        }
                        Marshal.ReleaseComObject(folderItems);
                        folderItems = null;

                        Microsoft.Office.Interop.Outlook.AddressList globalAddressList = null;


                        globalAddressList = outlookApp.Session.GetGlobalAddressList();

                        foreach (Microsoft.Office.Interop.Outlook.AddressEntry a in globalAddressList.AddressEntries)
                        {
                            string emailAddressType = a.Type;
                            string emailContactFullName = a.Name;
                            string emailAddress = string.Empty;
                            if (emailAddressType != string.Empty && emailAddressType != null)
                            {
                                if (emailAddressType == emailAddressTypeSMTP)
                                {

                                    emailAddress = a.Address;

                                }
                                else if (emailAddressType == emailAddressTypeExchange)
                                {
                                    Microsoft.Office.Interop.Outlook.Recipient recipient = ns.CreateRecipient(a.Address);
                                    if (recipient != null)
                                        try
                                        {
                                            if (recipient.Resolve())
                                            {
                                                Microsoft.Office.Interop.Outlook.ExchangeUser exUser = null;
                                                try
                                                {
                                                    exUser = recipient.AddressEntry.GetExchangeUser();
                                                    if (exUser != null)
                                                    {
                                                        emailAddress = exUser.PrimarySmtpAddress;
                                                    }
                                                }
                                                finally
                                                {
                                                    if (exUser != null)
                                                    {
                                                        Marshal.ReleaseComObject(exUser);
                                                    }
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            if (recipient != null)
                                            {
                                                Marshal.ReleaseComObject(recipient);
                                            }
                                        }
                                }
                            }

                            if (emailAddress != string.Empty)
                            {
                                string s = emailContactFullName + " " + emailExtendedAddressStartIndicator + emailAddress + emailExtendedAddressEndIndicator + ";";
                                if (outlookContactList.Contains(s) == false)
                                {
                                    outlookContactList.Add(s);
                                }
                            }
                        }


                        //// getting items from the Suggested Contacts folder in Outlook 
                        //folderSuggestedContacts = ns.GetDefaultFolder(
                        //Microsoft.Office.Interop.Outlook.OlDefaultFolders.ol); 
                        //folderItems = folderSuggestedContacts.Items; 
                        //for (int i = 1; folderItems.Count >= i; i++) 
                        //{ 
                        //itemObj = folderItems[i];
                        //if (itemObj is Microsoft.Office.Interop.Outlook.ContactItem)
                        //    contactItemsList.Add(itemObj as Microsoft.Office.Interop.Outlook.ContactItem); 
                        //else 
                        //Marshal.ReleaseComObject(itemObj); 

                        //} 
                    }

                    finally
                    {
                        if (folderItems != null)
                            Marshal.ReleaseComObject(folderItems);
                        if (folderContacts != null)
                            Marshal.ReleaseComObject(folderContacts);
                        if (folderSuggestedContacts != null)
                            Marshal.ReleaseComObject(folderSuggestedContacts);
                        if (ns != null)
                            Marshal.ReleaseComObject(ns);
                    }
                }
                finally
                {
                    if (outlookApp != null)
                        Marshal.ReleaseComObject(outlookApp);
                }
            }
            
            //tb.AutoCompleteCustomSource.Clear();
            //foreach (string s in outlookContactList)
            //{
            //    tb.AutoCompleteCustomSource.Add(s);
            //}

            return outlookContactList;

        }

        private const string emailAddressTypeSMTP = "SMTP";
        private const string emailAddressTypeExchange = "EX";
        private const string emailExtendedAddressStartIndicator = "(";
        private const string emailExtendedAddressEndIndicator = ")";

        private static List<string> outlookContactList = null;

        #region "Address Validation"
        private static bool _invalid;
        public static bool IsAddressValid(string emailAddress)
        {
            string tempEmailAddress = GetSMTPAddressFromExtendedAddress(emailAddress); //allow outlook style extended email addresses with the format: "Full Name <emailaddress>"
            
           

            //Lifted from here: http://msdn.microsoft.com/en-us/library/01escwtf.aspx
            if (String.IsNullOrEmpty(tempEmailAddress))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                tempEmailAddress = Regex.Replace(tempEmailAddress, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None);
            }
            catch (Exception e)
            {
                string s = e.ToString();
                return false;
            }


            if (_invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(tempEmailAddress,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
            }
            catch (Exception e)
            {
                string s = e.ToString();
                return false;
            }

        }
        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

     

#endregion

    }
}
