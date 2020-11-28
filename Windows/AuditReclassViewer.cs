using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.Business;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for AuditReclassViewer.
	/// </summary>
	public class frmAuditReclassViewer :  MIDFormBase
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		SessionAddressBlock _SAB;
        private Controls.ReportGridControl reportGridControl1;
        private System.ComponentModel.Container components = null;
		//private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
  //      private Windows.CrystalReports.AuditReclassReport2 auditReclassReport;




//		private System.Data.DataSet _reclassAuditDataSet;
//		private AuditReclassReport _auditReclassReport;

		public frmAuditReclassViewer(SessionAddressBlock aSAB) : base (aSAB)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_SAB = aSAB;
			
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
				this.Load -= new System.EventHandler(this.frmAuditReclassViewer_Load);
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
            this.reportGridControl1.Location = new System.Drawing.Point(12, 25);
            this.reportGridControl1.Name = "reportGridControl1";
            this.reportGridControl1.Size = new System.Drawing.Size(864, 481);
            this.reportGridControl1.TabIndex = 4;
            // 
            // frmAuditReclassViewer
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(888, 518);
            this.Controls.Add(this.reportGridControl1);
            this.Name = "frmAuditReclassViewer";
            this.Text = "Audit Reclass";
            this.Load += new System.EventHandler(this.frmAuditReclassViewer_Load);
            this.Controls.SetChildIndex(this.reportGridControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		public void InitializeForm()
		{
			try
			{
				FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ToolsAuditViewer);
                //eDataState dataState;
                //if (!FunctionSecurity.AllowUpdate)
                //{
                //    dataState = eDataState.ReadOnly;
                //}
                //else
                //{
                //    dataState = eDataState.Updatable;
                //}
				//Format_Title(dataState, eMIDTextCode.frm_AuditReclassViewer,null);
				
				Get_Report();
				SetReadOnly(FunctionSecurity.AllowUpdate);  
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}
		private void frmAuditReclassViewer_Load(object sender, System.EventArgs e)
		{
			InitializeForm();
			this.Dock = DockStyle.Fill;
			FormLoaded = true;
		}
		private void Get_Report()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				AuditData reclassAuditData = new AuditData();

				DataSet ds = MIDEnvironment.CreateDataSet("AuditReclassDataSet");
				reclassAuditData.ReclassAudit_Report(ds);

                reportGridControl1.BindGrid(aDataSet: ds, reportType: eReportType.AuditReclassViewer, reportName: "AuditReclass", reportTitle: "Logilitity - RO - Reclassification", reportInformation: "");

                //MessageBox.Show("Under Construction");
				//auditReclassReport = new Windows.CrystalReports.AuditReclassReport2();
				//auditReclassReport.SetDataSource(ds);

				//crystalReportViewer1.ReportSource = auditReclassReport;
				//crystalReportViewer1.RefreshReport();

//				CrystalDecisions.Shared.TableLogOnInfo tli = new CrystalDecisions.Shared.TableLogOnInfo();	
//				CrystalDecisions.Shared.TableLogOnInfos tlis = new CrystalDecisions.Shared.TableLogOnInfos();
//				CrystalDecisions.Shared.ConnectionInfo ci = new CrystalDecisions.Shared.ConnectionInfo();
//				ConnectionString = MIDConnectionString.ConnectionString;
//				ci.ServerName = 
//				ci.DatabaseName = 
//				ci.UserID = 
//				ci.Password = 
				this.Cursor = Cursors.Default;
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message );
			}
		}

        void formatReclassViewer()
        {
            MessageBox.Show("formatReclassViewer");
        }

    }

    
}
