using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
   

    public class frmReportViewer : MIDFormBase
	{
		#region Controls
		//private CrystalDecisions.Windows.Forms.CrystalReportViewer crvMainViewer;
		#endregion

		#region Member Vars
		private object _reportSource;
        private Controls.ReportGridControl reportGridControl1;
        //private eReportType _reportType;
        //private string _reportName;
        //private string _reportTitle;
        private List<ReportInfo> _reports;
        private ListBox listBox1;
        private Label label1;

        #endregion

        #region Properties
  //      public object ReportSource
		//{
		//	get { return _reportSource; }
		//	set { _reportSource = value; }
		//}

        #endregion

        #region Constructor and Form Initialization Code
        private System.ComponentModel.Container components = null;

		public frmReportViewer(SessionAddressBlock aSAB, List<ReportInfo> reports) : base (aSAB)
		{
            //_reportType = reportType;
            //_reportName = reportName;
            //_reportTitle = reportTitle;
            _reports = reports;
            InitializeComponent();
            listBox1.Items.Clear();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.reportGridControl1 = new MIDRetail.Windows.Controls.ReportGridControl();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // reportGridControl1
            // 
            this.reportGridControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reportGridControl1.Location = new System.Drawing.Point(122, 23);
            this.reportGridControl1.Name = "reportGridControl1";
            this.reportGridControl1.Size = new System.Drawing.Size(810, 474);
            this.reportGridControl1.TabIndex = 4;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(4, 47);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(112, 446);
            this.listBox1.TabIndex = 5;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 509);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // frmReportViewer
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(944, 534);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.reportGridControl1);
            this.Name = "frmReportViewer";
            this.Text = "Report Viewer";
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.Controls.SetChildIndex(this.reportGridControl1, 0);
            this.Controls.SetChildIndex(this.listBox1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		#region Event Handlers
		private void frmReportViewer_Load(object sender, System.EventArgs e)
		{
			InitializeForm();
			this.Dock = DockStyle.Fill;
			FormLoaded = true;
		}

        //private void crvMainViewer_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        //{
        //    e.Handled = true;   // ignores Refresh request which was prompting for parameters
        //}
      	#endregion

		#region Private Methods
		private void InitializeForm()
		{
            try
            {
                FunctionSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAuditViewer);
                //eDataState dataState;
                //if (!FunctionSecurity.AllowUpdate)
                //{
                //    dataState = eDataState.ReadOnly;
                //}
                //else
                //{
                //    dataState = eDataState.Updatable;
                //}
                listBox1.Sorted = true;
                if (_reports.Count > 0)
                {
                    foreach (ReportInfo reportInfo in _reports)
                    {
                        listBox1.Items.Add(reportInfo);
                    }

                    //Get_Report(_reports[0]);
                    Get_Report((ReportInfo)listBox1.Items[0]);
                }
                //SetReadOnly(false);
                
                listBox1.Enabled = true;
                //if (_reports.Count > 0)
                //{
                //    Get_Report((ReportInfo)listBox1.Items[0]);
                //}
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message);
                HandleException(exception);
            }
		}

		private void Get_Report(ReportInfo reportInfo)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;

                //crvMainViewer.ReportSource = ReportSource;
                //crvMainViewer.RefreshReport();
                //reportGridControl1.BindGrid((DataSet)ReportSource, _reportType);

                if (reportInfo.ReportSource != null)
                {
                    reportGridControl1.BindGrid(aDataSet: reportInfo.ReportSource, reportType: reportInfo.ReportType, reportName: reportInfo.ReportName, reportTitle: reportInfo.ReportTitle, reportInformation: reportInfo.ReportInformation);
                }
                label1.Text = reportInfo.ReportComment;

                this.Cursor = Cursors.Default;
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message);
				HandleException(exception);
			}
		}
        #endregion

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportInfo reportInfo = (ReportInfo)listBox1.SelectedItem;
            Get_Report(reportInfo);
            SetReadOnly(false);
            listBox1.Enabled = true;
        }
    }

    public class ReportInfo
    {
        private eReportType _reportType;
        private string _reportTitle;
        private string _reportName;
        private string _reportComment;
        private string _displayValue;
        private DataSet _reportSource;
        private string _reportInformation;

        public ReportInfo(DataSet aReportSource, eReportType reportType, string reportName, string reportTitle, string reportComment, string reportInformation, string displayValue)
        {
            _reportSource = aReportSource;
            _reportType = reportType;
            _reportTitle = reportTitle;
            _reportName = reportName;
            _reportComment = reportComment;
            _reportInformation = reportInformation;
            _displayValue = displayValue;
        }

        public DataSet ReportSource
        {
            get
            {
                return _reportSource;
            }
        }

        public eReportType ReportType
        {
            get
            {
                return _reportType;
            }
        }

        public string ReportTitle
        {
            get
            {
                return _reportTitle;
            }
        }

        public string ReportName
        {
            get
            {
                return _reportName;
            }
        }

        public string ReportComment
        {
            get
            {
                return _reportComment;
            }
        }

        public string ReportInformation
        {
            get
            {
                return _reportInformation;
            }
        }

        override public string ToString()
        {
            return _displayValue;
        }
    }
}
