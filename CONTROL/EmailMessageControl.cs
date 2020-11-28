using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MIDRetail.Data;

namespace MIDRetail.Windows.Controls
{
    public partial class EmailMessageControl : UserControl
    {
        public EmailMessageControl()
        {
            InitializeComponent();
        }

        private void ultraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "emailSend":
                    SendEmail();
                    break;
                case "emailAddAttachment":
                    AddAttachment();
                    break;
                case "emailRemoveAttachments":
                    RemoveAttachments();
                    break;
            }
        }
        

        private void SendEmail()
        {
            
            if (this.emailFieldEntry1.IsValid() == true)
            {
                string emailReturnMsg = String.Empty;
                MIDEmail.emailReturnMessageTypes emailReturnMessageType;

                System.Net.Mail.MailMessage m = MIDEmail.CreateMailMessage(out emailReturnMessageType, out emailReturnMsg, this.emailFieldEntry1.emailSubject, this.emailFieldEntry1.emailBody, this.emailFieldEntry1.emailFrom, this.emailFieldEntry1.emailTo, this.emailFieldEntry1.emailCC, null, getAttachmentPaths());
                if (emailReturnMessageType == MIDEmail.emailReturnMessageTypes.Success)
                {
                    //attach the permanent attachments
                    foreach (System.Net.Mail.Attachment a in permanentAttachmentList)
                    {
                        m.Attachments.Add(a);
                    }

                    if (MIDEmail.SendEmail(out emailReturnMsg, m) == MIDEmail.emailReturnMessageTypes.Success)
                    {
                        MessageBox.Show("Email sent.", MIDEmail.emailReturnMessages.Success);
                    }
                    else
                    {
                        MessageBox.Show(emailReturnMsg);
                    }


                }
                else
                {
                    MessageBox.Show(emailReturnMsg);
                }


            }
        }
        private string[] getAttachmentPaths()
        {
            string[] attachmentPaths = null;
            if (this.attachmentList.Count > 0)
            {
                attachmentPaths = new string[this.attachmentList.Count];

                int index = 0;
                foreach (attachmentFile a in attachmentList)
                {
                    attachmentPaths[index] = a.fullpath;
                    index ++;
                }
            }
            return attachmentPaths;
        }



        private class attachmentFile
        {
            public string fileName;
            public string fullpath;
        }

        private List<attachmentFile> attachmentList = new List<attachmentFile>();
        private List<System.Net.Mail.Attachment> permanentAttachmentList = new List<System.Net.Mail.Attachment>();  //permenant attachments are in-memory objects that should not be deleted.
    
        

        private void updateAttachmentLabel()
        {
            string s = String.Empty;

            foreach (System.Net.Mail.Attachment a in permanentAttachmentList)
            {
                s += "[" + a.Name + "] ";
            }


            foreach (attachmentFile a in attachmentList)
            {
                s += a.fileName;
                if (attachmentList.IndexOf(a) != attachmentList.Count - 1)
                    s += ", ";
            }
            this.txtAttachments.Text = s;
        }

        private void RemoveAttachments()
        {
           
            this.attachmentList.Clear();
            updateAttachmentLabel();
        }

        string _previousDirectory  = String.Empty ;
        private void AddAttachment()
        {
            System.IO.Stream myStream;
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "All files (*.*)|*.*";
                fileDialog.FilterIndex = 0;
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;
                fileDialog.CheckFileExists = true;
                if (_previousDirectory != String.Empty)
                {
                    fileDialog.InitialDirectory = _previousDirectory;
                }
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {


                    if ((myStream = fileDialog.OpenFile()) != null)
                    {
                        myStream.Close();

                        foreach (string s in fileDialog.FileNames)
                        {
                            string fullpath = s;
                            string filename = s;
                            int pos = s.LastIndexOf('\\');
                            if (pos != -1 && pos !=s.Length)
                            {
                               filename = s.Substring(pos + 1);
                            }


                            attachmentFile a = new attachmentFile();

                            a.fileName = filename;
                            a.fullpath = fullpath;

                            this.attachmentList.Add(a);
                            
                        }
                        
                    }
                }

                updateAttachmentLabel();

            }
            catch (Exception ex)
            {
                throw ex;
            }


           
        }
        public void AddAttachment(System.Net.Mail.Attachment attach)
        {
            this.permanentAttachmentList.Add(attach);
            updateAttachmentLabel();
        }

        public void SetDefaults(string sFrom = "", string sTo = "", string sCC = "", string sSubject = "", string sBody = "")
        {
            this.emailFieldEntry1.emailFrom = sFrom;
            this.emailFieldEntry1.emailTo = sTo;
            this.emailFieldEntry1.emailCC = sCC;
            this.emailFieldEntry1.emailSubject = sSubject;
            this.emailFieldEntry1.emailBody = sBody;
        }


    }
}
