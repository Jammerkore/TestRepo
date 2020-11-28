using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for AdvancedStoreEligibility.
	/// </summary>
	public class frmAdvancedStoreEligibility : System.Windows.Forms.Form
	{
		private System.Windows.Forms.CheckBox cbxApplyToLowerLevels;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnHelp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAdvancedStoreEligibility()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.cbxApplyToLowerLevels = new System.Windows.Forms.CheckBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbxApplyToLowerLevels
			// 
			this.cbxApplyToLowerLevels.Location = new System.Drawing.Point(48, 24);
			this.cbxApplyToLowerLevels.Name = "cbxApplyToLowerLevels";
			this.cbxApplyToLowerLevels.Size = new System.Drawing.Size(160, 24);
			this.cbxApplyToLowerLevels.TabIndex = 0;
			this.cbxApplyToLowerLevels.Text = "Apply to lower levels";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnCancel.Location = new System.Drawing.Point(208, 232);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.btnOK.Location = new System.Drawing.Point(112, 232);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.btnHelp.Location = new System.Drawing.Point(8, 232);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(24, 23);
			this.btnHelp.TabIndex = 12;
			this.btnHelp.Text = "?";
			// 
			// frmAdvancedStoreEligibility
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.btnHelp,
																		  this.btnCancel,
																		  this.btnOK,
																		  this.cbxApplyToLowerLevels});
			this.Name = "frmAdvancedStoreEligibility";
			this.Text = "AdvancedStoreEligibility";
			this.Load += new System.EventHandler(this.frmAdvancedStoreEligibility_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmAdvancedStoreEligibility_Load(object sender, System.EventArgs e)
		{
		
		}
	}
}
