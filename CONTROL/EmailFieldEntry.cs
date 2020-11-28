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
    public partial class EmailFieldEntry : UserControl
    {
        public EmailFieldEntry()
        {
            InitializeComponent();
        }

        public string emailSubject
        {
            get { return txtSubject.Text.Trim(); }
            set { txtSubject.Text = value; } 
        }
        public string emailBody
        {
            get { return txtMessage.Text.Trim(); }
            set { txtMessage.Text = value; } 
        }
        public string emailFrom
        {
            get { return txtFrom.Text.Trim(); }
            set { txtFrom.Text = value; } 
        }
        public string emailTo
        {
            get { return txtTo.Text.Trim(); }
            set { txtTo.Text = value; }
        }
        public string emailCC
        {
            get { return txtCC.Text.Trim(); }
            set { txtCC.Text = value; }
        }


        private bool _requireValidFromWithTo = false;
        public bool requireValidFromWithTo
        {
            get { return _requireValidFromWithTo; }
            set { _requireValidFromWithTo = value; }
        }
        private bool _requireFrom = true;
        public bool requireFrom
        {
            get { return _requireFrom; }
            set { _requireFrom = value; }
        }
        private bool _requireTo = true;
        public bool requireTo
        {
            get { return _requireTo; }
            set { _requireTo = value; }
        }
        private bool _requireSubject = true;
        public bool requireSubject
        {
            get { return _requireSubject; }
            set { _requireSubject = value; }
        }
        private bool _requireBody = true;
        public bool requireBody
        {
            get { return _requireBody; }
            set { _requireBody = value; }
        }

        private const int TooltipInitialDelay = 200;
        private const int TooltipAutoPopDelay = 5000;


        MIDEnhancedToolTip ttFrom= null;
        public void SetFromTooltip(string s)
        {
            if (ttFrom == null)
            {
                ttFrom = new MIDEnhancedToolTip(this.components);
                ttFrom.AutoPopDelay = TooltipAutoPopDelay;
                ttFrom.InitialDelay = TooltipInitialDelay;
            }
            ttFrom.SetToolTip(this.txtFrom, s);
        }
        MIDEnhancedToolTip ttTo = null;
        public void SetToTooltip(string s)
        {
            if (ttTo == null)
            {
                ttTo = new MIDEnhancedToolTip(this.components);
                ttTo.AutoPopDelay = TooltipAutoPopDelay;
                ttTo.InitialDelay = TooltipInitialDelay;
            }
            ttTo.SetToolTip(this.txtTo, s);
        }
        MIDEnhancedToolTip ttSubject = null;
        public void SetSubjectTooltip(string s)
        {
            if (ttSubject == null)
            {
                ttSubject = new MIDEnhancedToolTip(this.components);
                ttSubject.AutoPopDelay = TooltipAutoPopDelay;
                ttSubject.InitialDelay = TooltipInitialDelay;
            }
            ttSubject.SetToolTip(this.txtSubject, s);
        }
        MIDEnhancedToolTip ttBody = null;
        public void SetBodyTooltip(string s)
        {
            if (ttBody == null)
            {
                ttBody = new MIDEnhancedToolTip(this.components);
                ttBody.AutoPopDelay = TooltipAutoPopDelay;
                ttBody.InitialDelay = TooltipInitialDelay;
            }
            ttBody.SetToolTip(this.txtMessage, s);
        }

        //public string emailBCC
        //{
        //    get { return txtBCC.Text.Trim(); }
        //    set { txtBCC.Text = value; }
        //}

        //provide a list of error providers
        List<ErrorProvider> errorProviderList = new List<ErrorProvider>();
        public bool IsValid()
        {
            bool valid = true;

            //Clear any previous errors
            foreach (System.Windows.Forms.ErrorProvider errProvider in this.errorProviderList)
            {
                errProvider.Clear();
                errProvider.Dispose();
            }
            this.errorProviderList.Clear();


            if (_requireValidFromWithTo && (this.txtFrom.Text != String.Empty || this.txtTo.Text != String.Empty))
            {
                if (this.txtFrom.Text == String.Empty)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(this.txtFrom, MIDEmail.emailReturnMessages.From_Address_Required);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
                if (this.txtTo.Text == String.Empty)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(this.txtTo, MIDEmail.emailReturnMessages.To_Address_Required);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }

                if (this.txtFrom.Text != String.Empty)
                {
                    if (this.txtFrom.IsEveryAddressInTextValid() == false)
                    {
                        System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                        errProvider.SetError(this.txtFrom, MIDEmail.emailReturnMessages.From_Address_Invalid);
                        this.errorProviderList.Add(errProvider);
                        valid = false;
                    }
                }
                if (this.txtTo.Text != String.Empty)
                {
                    if (this.txtTo.IsEveryAddressInTextValid() == false)
                    {
                        System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                        errProvider.SetError(this.txtTo, MIDEmail.emailReturnMessages.To_Address_Invalid);
                        this.errorProviderList.Add(errProvider);
                        valid = false;
                    }
   
                }
            }
            else
            {
                //validate the FROM email address
                if (_requireFrom && this.txtFrom.Text == String.Empty)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(this.txtFrom, MIDEmail.emailReturnMessages.From_Address_Required);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
                else if (this.txtFrom.Text != String.Empty)
                {
                    if (this.txtFrom.IsEveryAddressInTextValid() == false)
                    {
                        System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                        errProvider.SetError(this.txtFrom, MIDEmail.emailReturnMessages.From_Address_Invalid);
                        this.errorProviderList.Add(errProvider);
                        valid = false;
                    }
                }

                //validate the TO email address
                if (_requireTo && this.txtTo.Text == String.Empty)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(this.txtTo, MIDEmail.emailReturnMessages.To_Address_Required);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
                else if (this.txtTo.Text != String.Empty)
                {
                    if (this.txtTo.IsEveryAddressInTextValid() == false)
                    {
                        System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                        errProvider.SetError(this.txtTo, MIDEmail.emailReturnMessages.To_Address_Invalid);
                        this.errorProviderList.Add(errProvider);
                        valid = false;
                    }
                }
            }

            //validate the CC email address, if provided
            if (this.txtCC.Text.Trim() != String.Empty)
            {
                if (this.txtCC.IsEveryAddressInTextValid() == false)
                {
                    System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                    errProvider.SetError(this.txtCC, MIDEmail.emailReturnMessages.CC_Address_Invalid);
                    this.errorProviderList.Add(errProvider);
                    valid = false;
                }
            }

           
            
            //validate the subject
            if (_requireSubject && this.txtSubject.Text == String.Empty)
            {
                System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                errProvider.SetError(this.txtSubject, MIDEmail.emailReturnMessages.Subject_Required);
                this.errorProviderList.Add(errProvider);
                valid = false;
            }
            
            //validate the body
            if (_requireBody && this.txtMessage.Text.Trim() == String.Empty)
            {
                System.Windows.Forms.ErrorProvider errProvider = new ErrorProvider();
                errProvider.SetError(this.txtMessage, MIDEmail.emailReturnMessages.Body_Required);
                this.errorProviderList.Add(errProvider);
                valid = false;
            }

            return valid;
        }

       
      



    }




    
}
