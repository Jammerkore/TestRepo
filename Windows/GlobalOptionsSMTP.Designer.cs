namespace MIDRetail.Windows
{
    partial class GlobalOptionsSMTP
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpSMTP_Settings = new System.Windows.Forms.GroupBox();
            this.grpSMTP_Server = new System.Windows.Forms.GroupBox();
            this.chkSMTP_UseOutlookContacts = new System.Windows.Forms.CheckBox();
            this.chkSMTP_SSL = new System.Windows.Forms.CheckBox();
            this.txtSMTP_Port = new System.Windows.Forms.TextBox();
            this.txtSMTP_Server = new System.Windows.Forms.TextBox();
            this.lblSMTP_Port = new System.Windows.Forms.Label();
            this.lblSMTP_Server = new System.Windows.Forms.Label();
            this.btnSMTP_SendTestEmail = new System.Windows.Forms.Button();
            this.grpSMTP_Authentication = new System.Windows.Forms.GroupBox();
            this.txtSMTP_Pwd = new System.Windows.Forms.TextBox();
            this.txtSMTP_UserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.radSMTP_SpecifyCredentials = new System.Windows.Forms.RadioButton();
            this.radSMTP_UseDefaultCredentials = new System.Windows.Forms.RadioButton();
            this.lblPwd = new System.Windows.Forms.Label();
            this.chkSMTP_Enable = new System.Windows.Forms.CheckBox();
            this.grpSMTP_MessageFormat = new System.Windows.Forms.GroupBox();
            this.radSMTP_FormatText = new System.Windows.Forms.RadioButton();
            this.radSMTP_FormatHTML = new System.Windows.Forms.RadioButton();
            this.lblFromAddress = new System.Windows.Forms.Label();
            this.txtSMTP_From_Address = new System.Windows.Forms.TextBox();
            this.grpSMTP_Settings.SuspendLayout();
            this.grpSMTP_Server.SuspendLayout();
            this.grpSMTP_Authentication.SuspendLayout();
            this.grpSMTP_MessageFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSMTP_Settings
            // 
            this.grpSMTP_Settings.Controls.Add(this.txtSMTP_From_Address);
            this.grpSMTP_Settings.Controls.Add(this.lblFromAddress);
            this.grpSMTP_Settings.Controls.Add(this.grpSMTP_Server);
            this.grpSMTP_Settings.Controls.Add(this.btnSMTP_SendTestEmail);
            this.grpSMTP_Settings.Controls.Add(this.grpSMTP_Authentication);
            this.grpSMTP_Settings.Controls.Add(this.chkSMTP_Enable);
            this.grpSMTP_Settings.Controls.Add(this.grpSMTP_MessageFormat);
            this.grpSMTP_Settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSMTP_Settings.Location = new System.Drawing.Point(0, 0);
            this.grpSMTP_Settings.Name = "grpSMTP_Settings";
            this.grpSMTP_Settings.Size = new System.Drawing.Size(579, 177);
            this.grpSMTP_Settings.TabIndex = 0;
            this.grpSMTP_Settings.TabStop = false;
            this.grpSMTP_Settings.Text = "SMTP Email Settings";
            // 
            // grpSMTP_Server
            // 
            this.grpSMTP_Server.Controls.Add(this.chkSMTP_UseOutlookContacts);
            this.grpSMTP_Server.Controls.Add(this.chkSMTP_SSL);
            this.grpSMTP_Server.Controls.Add(this.txtSMTP_Port);
            this.grpSMTP_Server.Controls.Add(this.txtSMTP_Server);
            this.grpSMTP_Server.Controls.Add(this.lblSMTP_Port);
            this.grpSMTP_Server.Controls.Add(this.lblSMTP_Server);
            this.grpSMTP_Server.Location = new System.Drawing.Point(7, 44);
            this.grpSMTP_Server.Name = "grpSMTP_Server";
            this.grpSMTP_Server.Size = new System.Drawing.Size(221, 122);
            this.grpSMTP_Server.TabIndex = 6;
            this.grpSMTP_Server.TabStop = false;
            this.grpSMTP_Server.Text = "SMTP Server";
            // 
            // chkSMTP_UseOutlookContacts
            // 
            this.chkSMTP_UseOutlookContacts.AutoSize = true;
            this.chkSMTP_UseOutlookContacts.Location = new System.Drawing.Point(9, 90);
            this.chkSMTP_UseOutlookContacts.Name = "chkSMTP_UseOutlookContacts";
            this.chkSMTP_UseOutlookContacts.Size = new System.Drawing.Size(180, 17);
            this.chkSMTP_UseOutlookContacts.TabIndex = 8;
            this.chkSMTP_UseOutlookContacts.Text = "Use Outlook Contact Information";
            this.chkSMTP_UseOutlookContacts.UseVisualStyleBackColor = true;
            // 
            // chkSMTP_SSL
            // 
            this.chkSMTP_SSL.AutoSize = true;
            this.chkSMTP_SSL.Location = new System.Drawing.Point(9, 71);
            this.chkSMTP_SSL.Name = "chkSMTP_SSL";
            this.chkSMTP_SSL.Size = new System.Drawing.Size(141, 17);
            this.chkSMTP_SSL.TabIndex = 4;
            this.chkSMTP_SSL.Text = "Enable SMTP Over SSL";
            this.chkSMTP_SSL.UseVisualStyleBackColor = true;
            // 
            // txtSMTP_Port
            // 
            this.txtSMTP_Port.Location = new System.Drawing.Point(48, 45);
            this.txtSMTP_Port.Name = "txtSMTP_Port";
            this.txtSMTP_Port.Size = new System.Drawing.Size(47, 20);
            this.txtSMTP_Port.TabIndex = 7;
            this.txtSMTP_Port.Text = "25";
            // 
            // txtSMTP_Server
            // 
            this.txtSMTP_Server.Location = new System.Drawing.Point(48, 19);
            this.txtSMTP_Server.MaxLength = 250;
            this.txtSMTP_Server.Name = "txtSMTP_Server";
            this.txtSMTP_Server.Size = new System.Drawing.Size(146, 20);
            this.txtSMTP_Server.TabIndex = 2;
            // 
            // lblSMTP_Port
            // 
            this.lblSMTP_Port.AutoSize = true;
            this.lblSMTP_Port.Location = new System.Drawing.Point(18, 48);
            this.lblSMTP_Port.Name = "lblSMTP_Port";
            this.lblSMTP_Port.Size = new System.Drawing.Size(26, 13);
            this.lblSMTP_Port.TabIndex = 6;
            this.lblSMTP_Port.Text = "Port";
            // 
            // lblSMTP_Server
            // 
            this.lblSMTP_Server.AutoSize = true;
            this.lblSMTP_Server.Location = new System.Drawing.Point(6, 21);
            this.lblSMTP_Server.Name = "lblSMTP_Server";
            this.lblSMTP_Server.Size = new System.Drawing.Size(38, 13);
            this.lblSMTP_Server.TabIndex = 4;
            this.lblSMTP_Server.Text = "Server";
            // 
            // btnSMTP_SendTestEmail
            // 
            this.btnSMTP_SendTestEmail.Location = new System.Drawing.Point(171, 13);
            this.btnSMTP_SendTestEmail.Name = "btnSMTP_SendTestEmail";
            this.btnSMTP_SendTestEmail.Size = new System.Drawing.Size(101, 23);
            this.btnSMTP_SendTestEmail.TabIndex = 5;
            this.btnSMTP_SendTestEmail.Text = "Send Test Email";
            this.btnSMTP_SendTestEmail.UseVisualStyleBackColor = true;
            this.btnSMTP_SendTestEmail.Click += new System.EventHandler(this.btnSMTP_SendTestEmail_Click);
            // 
            // grpSMTP_Authentication
            // 
            this.grpSMTP_Authentication.Controls.Add(this.txtSMTP_Pwd);
            this.grpSMTP_Authentication.Controls.Add(this.txtSMTP_UserName);
            this.grpSMTP_Authentication.Controls.Add(this.lblUserName);
            this.grpSMTP_Authentication.Controls.Add(this.radSMTP_SpecifyCredentials);
            this.grpSMTP_Authentication.Controls.Add(this.radSMTP_UseDefaultCredentials);
            this.grpSMTP_Authentication.Controls.Add(this.lblPwd);
            this.grpSMTP_Authentication.Location = new System.Drawing.Point(234, 44);
            this.grpSMTP_Authentication.Name = "grpSMTP_Authentication";
            this.grpSMTP_Authentication.Size = new System.Drawing.Size(220, 122);
            this.grpSMTP_Authentication.TabIndex = 4;
            this.grpSMTP_Authentication.TabStop = false;
            this.grpSMTP_Authentication.Text = "Authentication";
            // 
            // txtSMTP_Pwd
            // 
            this.txtSMTP_Pwd.Location = new System.Drawing.Point(78, 87);
            this.txtSMTP_Pwd.MaxLength = 50;
            this.txtSMTP_Pwd.Name = "txtSMTP_Pwd";
            this.txtSMTP_Pwd.Size = new System.Drawing.Size(116, 20);
            this.txtSMTP_Pwd.TabIndex = 8;
            this.txtSMTP_Pwd.UseSystemPasswordChar = true;
            // 
            // txtSMTP_UserName
            // 
            this.txtSMTP_UserName.Location = new System.Drawing.Point(78, 62);
            this.txtSMTP_UserName.MaxLength = 250;
            this.txtSMTP_UserName.Name = "txtSMTP_UserName";
            this.txtSMTP_UserName.Size = new System.Drawing.Size(116, 20);
            this.txtSMTP_UserName.TabIndex = 7;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(22, 66);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(29, 13);
            this.lblUserName.TabIndex = 4;
            this.lblUserName.Text = "User";
            // 
            // radSMTP_SpecifyCredentials
            // 
            this.radSMTP_SpecifyCredentials.AutoSize = true;
            this.radSMTP_SpecifyCredentials.Location = new System.Drawing.Point(6, 42);
            this.radSMTP_SpecifyCredentials.Name = "radSMTP_SpecifyCredentials";
            this.radSMTP_SpecifyCredentials.Size = new System.Drawing.Size(118, 17);
            this.radSMTP_SpecifyCredentials.TabIndex = 6;
            this.radSMTP_SpecifyCredentials.Text = "Specify Credentials:";
            this.radSMTP_SpecifyCredentials.UseVisualStyleBackColor = true;
            this.radSMTP_SpecifyCredentials.CheckedChanged += new System.EventHandler(this.radSMTP_SpecifyCredentials_CheckedChanged);
            // 
            // radSMTP_UseDefaultCredentials
            // 
            this.radSMTP_UseDefaultCredentials.AutoSize = true;
            this.radSMTP_UseDefaultCredentials.Checked = true;
            this.radSMTP_UseDefaultCredentials.Location = new System.Drawing.Point(6, 19);
            this.radSMTP_UseDefaultCredentials.Name = "radSMTP_UseDefaultCredentials";
            this.radSMTP_UseDefaultCredentials.Size = new System.Drawing.Size(170, 17);
            this.radSMTP_UseDefaultCredentials.TabIndex = 5;
            this.radSMTP_UseDefaultCredentials.TabStop = true;
            this.radSMTP_UseDefaultCredentials.Text = "Use Default Server Credentials";
            this.radSMTP_UseDefaultCredentials.UseVisualStyleBackColor = true;
            this.radSMTP_UseDefaultCredentials.CheckedChanged += new System.EventHandler(this.radSMTP_UseDefaultCredentials_CheckedChanged);
            // 
            // lblPwd
            // 
            this.lblPwd.AutoSize = true;
            this.lblPwd.Location = new System.Drawing.Point(22, 91);
            this.lblPwd.Name = "lblPwd";
            this.lblPwd.Size = new System.Drawing.Size(53, 13);
            this.lblPwd.TabIndex = 6;
            this.lblPwd.Text = "Password";
            // 
            // chkSMTP_Enable
            // 
            this.chkSMTP_Enable.AutoSize = true;
            this.chkSMTP_Enable.Location = new System.Drawing.Point(6, 19);
            this.chkSMTP_Enable.Name = "chkSMTP_Enable";
            this.chkSMTP_Enable.Size = new System.Drawing.Size(134, 17);
            this.chkSMTP_Enable.TabIndex = 1;
            this.chkSMTP_Enable.Text = "Enable SMTP Emailing";
            this.chkSMTP_Enable.UseVisualStyleBackColor = true;
            this.chkSMTP_Enable.CheckedChanged += new System.EventHandler(this.chkSMTP_Enable_CheckedChanged);
            // 
            // grpSMTP_MessageFormat
            // 
            this.grpSMTP_MessageFormat.Controls.Add(this.radSMTP_FormatText);
            this.grpSMTP_MessageFormat.Controls.Add(this.radSMTP_FormatHTML);
            this.grpSMTP_MessageFormat.Location = new System.Drawing.Point(460, 44);
            this.grpSMTP_MessageFormat.Name = "grpSMTP_MessageFormat";
            this.grpSMTP_MessageFormat.Size = new System.Drawing.Size(109, 71);
            this.grpSMTP_MessageFormat.TabIndex = 2;
            this.grpSMTP_MessageFormat.TabStop = false;
            this.grpSMTP_MessageFormat.Text = "Message Format";
            // 
            // radSMTP_FormatText
            // 
            this.radSMTP_FormatText.AutoSize = true;
            this.radSMTP_FormatText.Location = new System.Drawing.Point(6, 42);
            this.radSMTP_FormatText.Name = "radSMTP_FormatText";
            this.radSMTP_FormatText.Size = new System.Drawing.Size(46, 17);
            this.radSMTP_FormatText.TabIndex = 10;
            this.radSMTP_FormatText.Text = "Text";
            this.radSMTP_FormatText.UseVisualStyleBackColor = true;
            // 
            // radSMTP_FormatHTML
            // 
            this.radSMTP_FormatHTML.AutoSize = true;
            this.radSMTP_FormatHTML.Checked = true;
            this.radSMTP_FormatHTML.Location = new System.Drawing.Point(6, 19);
            this.radSMTP_FormatHTML.Name = "radSMTP_FormatHTML";
            this.radSMTP_FormatHTML.Size = new System.Drawing.Size(55, 17);
            this.radSMTP_FormatHTML.TabIndex = 9;
            this.radSMTP_FormatHTML.TabStop = true;
            this.radSMTP_FormatHTML.Text = "HTML";
            this.radSMTP_FormatHTML.UseVisualStyleBackColor = true;
            // 
            // lblFromAddress
            // 
            this.lblFromAddress.AutoSize = true;
            this.lblFromAddress.Location = new System.Drawing.Point(291, 19);
            this.lblFromAddress.Name = "lblFromAddress";
            this.lblFromAddress.Size = new System.Drawing.Size(30, 13);
            this.lblFromAddress.TabIndex = 7;
            this.lblFromAddress.Text = "From";
            // 
            // txtSMTP_From_Address
            // 
            this.txtSMTP_From_Address.Location = new System.Drawing.Point(326, 17);
            this.txtSMTP_From_Address.MaxLength = 250;
            this.txtSMTP_From_Address.Name = "txtSMTP_From_Address";
            this.txtSMTP_From_Address.Size = new System.Drawing.Size(243, 20);
            this.txtSMTP_From_Address.TabIndex = 8;
            // 
            // GlobalOptionsSMTP
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.grpSMTP_Settings);
            this.Name = "GlobalOptionsSMTP";
            this.Size = new System.Drawing.Size(579, 177);
            this.grpSMTP_Settings.ResumeLayout(false);
            this.grpSMTP_Settings.PerformLayout();
            this.grpSMTP_Server.ResumeLayout(false);
            this.grpSMTP_Server.PerformLayout();
            this.grpSMTP_Authentication.ResumeLayout(false);
            this.grpSMTP_Authentication.PerformLayout();
            this.grpSMTP_MessageFormat.ResumeLayout(false);
            this.grpSMTP_MessageFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSMTP_Settings;
        private System.Windows.Forms.GroupBox grpSMTP_Authentication;
        private System.Windows.Forms.RadioButton radSMTP_SpecifyCredentials;
        private System.Windows.Forms.RadioButton radSMTP_UseDefaultCredentials;
        private System.Windows.Forms.CheckBox chkSMTP_Enable;
        private System.Windows.Forms.GroupBox grpSMTP_MessageFormat;
        private System.Windows.Forms.RadioButton radSMTP_FormatText;
        private System.Windows.Forms.RadioButton radSMTP_FormatHTML;
        private System.Windows.Forms.Button btnSMTP_SendTestEmail;
        private System.Windows.Forms.TextBox txtSMTP_Pwd;
        private System.Windows.Forms.TextBox txtSMTP_UserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPwd;
        private System.Windows.Forms.GroupBox grpSMTP_Server;
        private System.Windows.Forms.TextBox txtSMTP_Port;
        private System.Windows.Forms.TextBox txtSMTP_Server;
        private System.Windows.Forms.Label lblSMTP_Port;
        private System.Windows.Forms.Label lblSMTP_Server;
        private System.Windows.Forms.CheckBox chkSMTP_SSL;
        private System.Windows.Forms.CheckBox chkSMTP_UseOutlookContacts;
        private System.Windows.Forms.TextBox txtSMTP_From_Address;
        private System.Windows.Forms.Label lblFromAddress;
    }
}
