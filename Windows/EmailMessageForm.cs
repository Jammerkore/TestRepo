using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
    public partial class EmailMessageForm : Form
    {
        public EmailMessageForm()
        {
            InitializeComponent();
            this.Icon = SharedRoutines.GetApplicationIcon();
        }

        public void AddAttachment(System.Net.Mail.Attachment attach)
        {
            this.emailMessageControl1.AddAttachment(attach);
        }
        public void SetDefaults(string sFrom = "", string sTo = "", string sCC = "", string sSubject = "", string sBody = "")
        {
            this.emailMessageControl1.SetDefaults(sFrom, sTo, sCC, sSubject, sBody); 
        }
    }
}
