using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
    /// <summary>
    /// Class used to isolate the data and business logic from the UI
    /// </summary>
    public class GlobalOptions_SMTP_BL : baseBL
    {
        public struct fieldDescriptions
        {
            public static string EMPTY = "";
            public static string SMTP_ENABLED = "Enable SMTP Emailing"; // MIDText.GetTextOnly(eMIDTextCode.msg_Email_SMTP_SendingEmailIssue);
            public static string SERVER = "Server";
            public static string PORT = "Port";
            public static string USE_SSL = "Enable SMTP Over SSL";
            public static string USER = "User";
            public static string PWD = "Password";
            public static string USE_OUTLOOK = "Use Outlook Contact Information";
            public static string FROM_ADDRESS = "From Address"; //TT#3600 -jsobek -Add a default email address on the global options screen...
        }
        



        public static BoolFieldDefinition STMP_Enabled_Definition = new BoolFieldDefinition(DatabaseName: "SMTP_ENABLED", Description: fieldDescriptions.SMTP_ENABLED, IsRequired: false, DefaultValue: false);
        public static StringFieldDefinition SMTP_Server_Definition = new StringFieldDefinition(DatabaseName: "SMTP_SERVER", Description: fieldDescriptions.SERVER, IsRequired: false, MaxLength: 250, AllowEmptyString: true);
        public static IntFieldDefinition SMTP_Port_Definition = new IntFieldDefinition(DatabaseName: "SMTP_PORT", Description: fieldDescriptions.PORT, IsRequired: false, AllowZero: false, AllowNegative: false, DefaultValue: 25);
        public static BoolFieldDefinition SMTP_Use_SSL_Definition = new BoolFieldDefinition(DatabaseName: "SMTP_USE_SSL", Description: fieldDescriptions.USE_SSL, IsRequired: false, DefaultValue: false);
        public static BoolFieldDefinition SMTP_Use_Default_Credentials_Definition = new BoolFieldDefinition(DatabaseName: "SMTP_USE_DEFAULT_CREDENTIALS", Description: fieldDescriptions.EMPTY, IsRequired: false, DefaultValue: true);
        public static StringFieldDefinition SMTP_UserName_Definition = new StringFieldDefinition(DatabaseName: "SMTP_USERNAME", Description: fieldDescriptions.USER, IsRequired: false, MaxLength: 250, AllowEmptyString: true);
        public static StringFieldDefinition SMTP_Pwd_Definition = new StringFieldDefinition(DatabaseName: "SMTP_PWD", Description: fieldDescriptions.PWD, IsRequired: false, MaxLength: 50, AllowEmptyString: true);
        public static BoolFieldDefinition SMTP_MessageFormatInHTML_Definition = new BoolFieldDefinition(DatabaseName: "SMTP_MESSAGE_FORMAT_IN_HTML", Description: fieldDescriptions.EMPTY, IsRequired: false, DefaultValue: true);
        public static BoolFieldDefinition SMTP_Use_Outlook_Contacts_Definition = new BoolFieldDefinition(DatabaseName: "SMTP_USE_OUTLOOK_CONTACTS", Description: fieldDescriptions.USE_OUTLOOK, IsRequired: false, DefaultValue: false);
        public static StringFieldDefinition SMTP_From_Address_Definition = new StringFieldDefinition(DatabaseName: "SMTP_FROM_ADDRESS", Description: fieldDescriptions.FROM_ADDRESS, IsRequired: true, MaxLength: 250, AllowEmptyString: false); //TT#3600 -jsobek -Add a default email address on the global options screen...
        
        public boolField SMTP_Enabled;   
        public stringField SMTP_Server;  
        public intField SMTP_Port;       
        public boolField SMTP_Use_SSL;   
        public boolField SMTP_Use_Default_Credentials;
        public stringField SMTP_UserName;
        public stringField SMTP_Pwd;
        public boolField SMTP_MessageFormatInHTML;
        public boolField SMTP_Use_Outlook_Contacts;
        public stringField SMTP_From_Address; //TT#3600 -jsobek -Add a default email address on the global options screen...

        public GlobalOptions_SMTP_BL()
        {
            SMTP_Enabled = new boolField(STMP_Enabled_Definition, Fields);
            SMTP_Server = new stringField(SMTP_Server_Definition, Fields);
            SMTP_Port = new intField(SMTP_Port_Definition, Fields);
            SMTP_Use_SSL = new boolField(SMTP_Use_SSL_Definition, Fields);
            SMTP_Use_Default_Credentials = new boolField(SMTP_Use_Default_Credentials_Definition, Fields);
            SMTP_UserName = new stringField(SMTP_UserName_Definition, Fields);
            SMTP_Pwd = new stringField(SMTP_Pwd_Definition, Fields);
            SMTP_MessageFormatInHTML = new boolField(SMTP_MessageFormatInHTML_Definition, Fields);
            SMTP_Use_Outlook_Contacts = new boolField(SMTP_Use_Outlook_Contacts_Definition, Fields);
            SMTP_From_Address = new stringField(SMTP_From_Address_Definition, Fields); //TT#3600 -jsobek -Add a default email address on the global options screen...

          
            this.SMTP_Enabled.Definition.AfterPropertyChangedEvent += new AfterPropertyChangedEventHandler(SMTP_Enabled_Changed);
            this.SMTP_Use_Default_Credentials.Definition.AfterPropertyChangedEvent += new AfterPropertyChangedEventHandler(credentialOptionsChanged);
 
        }

        private void credentialOptionsChanged(object sender, AfterPropertyChangedEventArgs e)
        {
            //FieldDefinition d = (FieldDefinition)sender;

            if ((bool)e.newValue == true)
            {
                this.SMTP_UserName.Value = String.Empty;
                this.SMTP_Pwd.Value = String.Empty;

                this.SMTP_UserName.Definition.IsRequired = false;
                this.SMTP_Pwd.Definition.IsRequired = false;
            }
            else
            {
                if (this.SMTP_Enabled.Value == true)
                {
                    this.SMTP_UserName.Definition.IsRequired = true;
                    this.SMTP_Pwd.Definition.IsRequired = true;
                }
                else
                {
                    this.SMTP_UserName.Definition.IsRequired = false;
                    this.SMTP_Pwd.Definition.IsRequired = false;
                }
            }
        }

        private void SMTP_Enabled_Changed(object sender, AfterPropertyChangedEventArgs e)
        {
            //FieldDefinition d = (FieldDefinition)sender;

            if ((bool)e.newValue == true)
            {
                this.SMTP_Server.Definition.IsRequired = true;
                this.SMTP_Port.Definition.IsRequired = true;
            }
            else
            {
                this.SMTP_Server.Definition.IsRequired = false;
                this.SMTP_Port.Definition.IsRequired = false;
            }
        }



       

    }
}
