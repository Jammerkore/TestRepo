using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
    public partial class EmailTaskList : UserControl
    {
        public EmailTaskList()
        {
            InitializeComponent();
        }

        private void EmailTaskList_Load(object sender, EventArgs e)
        {
            this.emailSuccessFieldEntry.requireValidFromWithTo = true;
            this.emailSuccessFieldEntry.requireFrom = false;
            this.emailSuccessFieldEntry.requireTo = false;
            this.emailSuccessFieldEntry.requireSubject = false;
            this.emailSuccessFieldEntry.requireBody = false;

            this.emailFailureFieldEntry.requireValidFromWithTo = true;
            this.emailFailureFieldEntry.requireFrom = false;
            this.emailFailureFieldEntry.requireTo = false;
            this.emailFailureFieldEntry.requireSubject = false;
            this.emailFailureFieldEntry.requireBody = false;

            
        }
        //public string taskTypeName;
        public int taskType;
        public string taskListName;
        private void SetToolTips()
        {
            this.emailSuccessFieldEntry.SetFromTooltip("This is a required field.");
            this.emailFailureFieldEntry.SetFromTooltip("This is a required field.");
            this.emailSuccessFieldEntry.SetToTooltip("This is a required field.");
            this.emailFailureFieldEntry.SetToTooltip("This is a required field.");
            string taskTypeName = ((eTaskType)taskType).ToString();
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            if (taskTypeName == "computationDriver")
            {
                taskTypeName = "Chain Forecasting";
            }
            //END TT#4574 - DOConnell - Purge Task List does not have an email option
            string emailSuccessSubject = "Default Value:" + System.Environment.NewLine + "Task " + taskTypeName + " on Task List " + taskListName + " was successful";
            string emailFailureSubject = "Default Value:" + System.Environment.NewLine + "Task " + taskTypeName + " on Task List " + taskListName + " has failed";
            this.emailSuccessFieldEntry.SetSubjectTooltip(emailSuccessSubject);
            this.emailFailureFieldEntry.SetSubjectTooltip(emailFailureSubject);

            eProcessCompletionStatus taskStatus;
            eMIDMessageLevel taskMsgLevel;

            taskStatus = eProcessCompletionStatus.Successful;
            taskMsgLevel = eMIDMessageLevel.Information;
            string emailSuccessBody = String.Empty;
            emailSuccessBody += "Default Value:" + System.Environment.NewLine + emailSuccessSubject + System.Environment.NewLine;
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            //emailSuccessBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)taskMsgLevel) + ")" + System.Environment.NewLine + System.Environment.NewLine;
            emailSuccessBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + taskTypeName + ")" + System.Environment.NewLine + System.Environment.NewLine;
            //END TT#4574 - DOConnell - Purge Task List does not have an email option
            emailSuccessBody += MIDEmail.CreateBasicUserInfoTextBlockWithDateTime(System.Environment.NewLine);

            taskStatus = eProcessCompletionStatus.Failed;
            taskMsgLevel = eMIDMessageLevel.Error;
            string emailFailureBody = String.Empty;
            emailFailureBody += "Default Value:" + System.Environment.NewLine + emailFailureSubject + System.Environment.NewLine;
            emailFailureBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)taskMsgLevel) + ")" + System.Environment.NewLine + System.Environment.NewLine;
            //BEGIN TT#4574 - DOConnell - Purge Task List does not have an email option
            //emailFailureBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)taskMsgLevel) + ")" + System.Environment.NewLine + System.Environment.NewLine;
            emailFailureBody += "Status: " + MIDText.GetTextOnly((int)taskStatus) + " (" + taskTypeName + ")" + System.Environment.NewLine + System.Environment.NewLine;
            //END TT#4574 - DOConnell - Purge Task List does not have an email option
            emailFailureBody += MIDEmail.CreateBasicUserInfoTextBlockWithDateTime(System.Environment.NewLine);

            this.emailSuccessFieldEntry.SetBodyTooltip(emailSuccessBody);
            this.emailFailureFieldEntry.SetBodyTooltip(emailFailureBody);
        }

        public bool IsValid()
        {
            bool valid = true;

            //ensure both controls validate
            if (this.emailSuccessFieldEntry.IsValid() == false)
                valid = false;
            if (this.emailFailureFieldEntry.IsValid() == false)
                valid = false;


            return valid;
        }


        public void LoadDefaults()
        {
            this.emailSuccessFieldEntry.emailFrom = String.Empty;
            this.emailSuccessFieldEntry.emailTo = String.Empty;
            this.emailSuccessFieldEntry.emailCC = String.Empty;
            this.emailSuccessFieldEntry.emailSubject = String.Empty;
            this.emailSuccessFieldEntry.emailBody = String.Empty;

            this.emailFailureFieldEntry.emailFrom = String.Empty;
            this.emailFailureFieldEntry.emailTo = String.Empty;
            this.emailFailureFieldEntry.emailCC = String.Empty;
            this.emailFailureFieldEntry.emailSubject = String.Empty;
            this.emailFailureFieldEntry.emailBody = String.Empty;

        }


        public void LoadFromDataRow(DataRow dr)
        {
            LoadDefaults();

            if (dr.Table.Columns.Contains("EMAIL_SUCCESS_FROM") && dr["EMAIL_SUCCESS_FROM"] != DBNull.Value)
            {
                this.emailSuccessFieldEntry.emailFrom = (string)dr["EMAIL_SUCCESS_FROM"];  
            }
            if (dr.Table.Columns.Contains("EMAIL_SUCCESS_TO") && dr["EMAIL_SUCCESS_TO"] != DBNull.Value)
            {
                this.emailSuccessFieldEntry.emailTo = (string)dr["EMAIL_SUCCESS_TO"];
            }
            if (dr.Table.Columns.Contains("EMAIL_SUCCESS_CC") && dr["EMAIL_SUCCESS_CC"] != DBNull.Value)
            {
                this.emailSuccessFieldEntry.emailCC = (string)dr["EMAIL_SUCCESS_CC"];
            }
            if (dr.Table.Columns.Contains("EMAIL_SUCCESS_SUBJECT") && dr["EMAIL_SUCCESS_SUBJECT"] != DBNull.Value)
            {
                this.emailSuccessFieldEntry.emailSubject = (string)dr["EMAIL_SUCCESS_SUBJECT"];
            }
            if (dr.Table.Columns.Contains("EMAIL_SUCCESS_BODY") && dr["EMAIL_SUCCESS_BODY"] != DBNull.Value)
            {
                this.emailSuccessFieldEntry.emailBody = (string)dr["EMAIL_SUCCESS_BODY"];
            }

            if (dr.Table.Columns.Contains("EMAIL_FAILURE_FROM") && dr["EMAIL_FAILURE_FROM"] != DBNull.Value)
            {
                this.emailFailureFieldEntry.emailFrom = (string)dr["EMAIL_FAILURE_FROM"];
            }
            if (dr.Table.Columns.Contains("EMAIL_FAILURE_TO") && dr["EMAIL_FAILURE_TO"] != DBNull.Value)
            {
                this.emailFailureFieldEntry.emailTo = (string)dr["EMAIL_FAILURE_TO"];
            }
            if (dr.Table.Columns.Contains("EMAIL_FAILURE_CC") && dr["EMAIL_FAILURE_CC"] != DBNull.Value)
            {
                this.emailFailureFieldEntry.emailCC = (string)dr["EMAIL_FAILURE_CC"];
            }
            if (dr.Table.Columns.Contains("EMAIL_FAILURE_SUBJECT") && dr["EMAIL_FAILURE_SUBJECT"] != DBNull.Value)
            {
                this.emailFailureFieldEntry.emailSubject = (string)dr["EMAIL_FAILURE_SUBJECT"];
            }
            if (dr.Table.Columns.Contains("EMAIL_FAILURE_BODY") && dr["EMAIL_FAILURE_BODY"] != DBNull.Value)
            {
                this.emailFailureFieldEntry.emailBody = (string)dr["EMAIL_FAILURE_BODY"];
            }
            SetToolTips();
        }

        public void SaveToDataRow(DataRow dr)
        {
            dr["EMAIL_SUCCESS_FROM"] = this.emailSuccessFieldEntry.emailFrom;
            dr["EMAIL_SUCCESS_TO"] = this.emailSuccessFieldEntry.emailTo;
            dr["EMAIL_SUCCESS_CC"] = this.emailSuccessFieldEntry.emailCC;
            dr["EMAIL_SUCCESS_SUBJECT"] = this.emailSuccessFieldEntry.emailSubject;
            dr["EMAIL_SUCCESS_BODY"] = this.emailSuccessFieldEntry.emailBody;

            dr["EMAIL_FAILURE_FROM"] = this.emailFailureFieldEntry.emailFrom;
            dr["EMAIL_FAILURE_TO"] = this.emailFailureFieldEntry.emailTo;
            dr["EMAIL_FAILURE_CC"] = this.emailFailureFieldEntry.emailCC;
            dr["EMAIL_FAILURE_SUBJECT"] = this.emailFailureFieldEntry.emailSubject;
            dr["EMAIL_FAILURE_BODY"] = this.emailFailureFieldEntry.emailBody;
        }




    }
}
