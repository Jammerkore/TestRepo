using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	public class frmReportViewer : MIDFormBase
	{
		#region Controls
		private CrystalDecisions.Windows.Forms.CrystalReportViewer crvMainViewer;
		#endregion

		#region Member Vars
		private object _reportSource;
		#endregion
		
		#region Properties
		public object ReportSource
		{
			get { return _reportSource; }
			set { _reportSource = value; }
		}
		#endregion

		#region Constructor and Form Initialization Code
		private System.ComponentModel.Container components = null;

		public frmReportViewer(SessionAddressBlock aSAB) : base (aSAB)
		{
			InitializeComponent();
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
            this.crvMainViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // crvMainViewer
            // 
            this.crvMainViewer.ActiveViewIndex = -1;
            this.crvMainViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crvMainViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crvMainViewer.Location = new System.Drawing.Point(0, 0);
            this.crvMainViewer.Name = "crvMainViewer";
            this.crvMainViewer.SelectionFormula = "";
            this.crvMainViewer.ShowRefreshButton = false;
            this.crvMainViewer.Size = new System.Drawing.Size(944, 534);
            this.crvMainViewer.TabIndex = 0;
            this.crvMainViewer.ViewTimeSelectionFormula = "";
            this.crvMainViewer.ReportRefresh += new CrystalDecisions.Windows.Forms.RefreshEventHandler(this.crvMainViewer_ReportRefresh);
            // 
            // frmReportViewer
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(944, 534);
            this.Controls.Add(this.crvMainViewer);
            this.Name = "frmReportViewer";
            this.Text = "Report Viewer";
            this.Load += new System.EventHandler(this.frmReportViewer_Load);
            this.Controls.SetChildIndex(this.crvMainViewer, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

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

        private void crvMainViewer_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            e.Handled = true;   // ignores Refresh request which was prompting for parameters
        }
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
				
				Get_Report();
				SetReadOnly(FunctionSecurity.AllowUpdate);  
			}
			catch(Exception exception)
			{
				MessageBox.Show(this, exception.Message);
				HandleException(exception);
			}
		}

		private void Get_Report()
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				
				crvMainViewer.ReportSource = ReportSource;
				//crvMainViewer.RefreshReport();

				this.Cursor = Cursors.Default;
			}
			catch (Exception exception)
			{
				MessageBox.Show(this, exception.Message);
				HandleException(exception);
			}
		}
		#endregion
	}
}
