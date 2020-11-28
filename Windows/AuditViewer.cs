//using System;
//using System.Data;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Globalization;
//using System.IO;
//using System.Windows.Forms;
//using Infragistics.Win;
//using Infragistics.Win.UltraWinGrid;

//using MIDRetail.DataCommon;
//using MIDRetail.Common;
//using MIDRetail.Data;
//using MIDRetail.Business;

//namespace MIDRetail.Windows
//{
//    public partial class frm_AuditViewer : Form
//    {
//        private AuditData _auditData;
//        SessionAddressBlock _SAB;

//        private FunctionSecurityProfile _functionSecurity;

//        MIDFormBase MIDFB = new MIDFormBase();

//        public frm_AuditViewer(SessionAddressBlock aSAB)
//        {
//            if (aSAB == null)
//            {
//                throw new Exception("SessionAddressBlock is required");
//            }
//            _SAB = aSAB;

//            InitializeComponent();
//            _auditData = new AuditData();
//        }

//        private void auditContainer1_Load(object sender, EventArgs e)
//        {
//            this.Dock = DockStyle.Fill;

//        }

//        private void AuditControl1_Load(object sender, EventArgs e)
//        {
//            auditContainer1.containerddfunctionDelegate = new Include.AuditFilterFormDelegate(btnFilter_Click);
//            auditContainer1.Populate_Audit();
//        }

//        /// <summary>
//        /// Gets or sets the function security profile
//        /// </summary>
//        public FunctionSecurityProfile FunctionSecurity
//        {
//            get { return _functionSecurity; }
//            set { _functionSecurity = value; }
//        }

//        public void refreshGrid()
//        { 
        
//        }

//        private void btnFilter_Click(object sender, EventArgs e)
//        {
//            try
//            {

//                AuditFilter af = new AuditFilter(_SAB);

//                af.OnAuditFilterChangeHandler += new AuditFilter.AuditFilterChangeEventHandler(OnAuditFilterChangeHandler);
//                if (this.ParentForm.GetType().FullName == "MIDRetail.Windows.Explorer")
//                {
//                    af.MdiParent = this.ParentForm;
//                }
//                else
//                {
//                    af.MdiParent = this.ParentForm.Owner;
//                }
//                af.Show();
//            }
//            catch (Exception exception)
//            {
//                throw exception;
//                //HandleException(exception);
//            }
//        }

//        void OnAuditFilterChangeHandler(object source, AuditFilterChangeEventArgs e)
//        {
//            auditContainer1.resetFilterProfile();
//            InitializeForm();
//        }

//        public void InitializeForm()
//        {
//            try
//            {
//                auditContainer1.InitializeForm();
//            }
//            catch (Exception exception)
//            {
//                MessageBox.Show(this, exception.Message);
//            }
//        }
//    }
//}
