using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Business;


namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for NameDialog.
	/// </summary>
	public class SelectSizeCode : MIDFormBase
	{
		private string _sizeCodeID;
		private string _labelText;
		private SizeCodeList _scl;
		private SessionAddressBlock _SAB;

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.ListBox lstSizeCode;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnHelp;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public string LabelText 
		{
			get{return _labelText;}
			set{_labelText = value;}
		}

		public string SizeCodeID 
		{
			get{return _sizeCodeID;}
			set{_sizeCodeID = value;}
		}

		/// <summary>
		/// base constructor
		/// </summary>
		public SelectSizeCode(SizeCodeList scl, SessionAddressBlock aSAB) : base (aSAB)
		{
			InitializeComponent();

			_scl = scl;
			_SAB = aSAB;
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

				this.btnOk.Click -= new System.EventHandler(this.btnOk_Click);
				this.btnHelp.Click -= new System.EventHandler(this.btnHelp_Click);
				this.Load -= new System.EventHandler(this.SelectSizeCode_Load);
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
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.lstSizeCode = new System.Windows.Forms.ListBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnHelp = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(500, 16);
			this.label1.TabIndex = 0;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOk.Location = new System.Drawing.Point(348, 174);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "&Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// lstSizeCode
			// 
			this.lstSizeCode.Location = new System.Drawing.Point(16, 56);
			this.lstSizeCode.Name = "lstSizeCode";
			this.lstSizeCode.Size = new System.Drawing.Size(528, 108);
			this.lstSizeCode.TabIndex = 3;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(456, 174);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 17;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnHelp
			// 
			this.btnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnHelp.Location = new System.Drawing.Point(16, 174);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(24, 23);
			this.btnHelp.TabIndex = 18;
			this.btnHelp.Text = "?";
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// SelectSizeCode
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(560, 206);
			this.Controls.Add(this.btnHelp);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lstSizeCode);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectSizeCode";
			this.Text = "Select Size Code";
			this.Load += new System.EventHandler(this.SelectSizeCode_Load);
			this.ResumeLayout(false);

		}
		#endregion


		private void btnOk_Click(object sender, System.EventArgs e)
		{

			if (lstSizeCode.SelectedItem == null)
			{
				MessageBox.Show (_SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_SelectEntry),  this.Text, 	MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			string listStr = lstSizeCode.SelectedItem.ToString();
			string[] fields = listStr.Split(new char[] {'-'});

			this.SizeCodeID = fields[0].Trim();
			DialogResult = DialogResult.OK;
			this.Close();

		}


		private void btnHelp_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void SelectSizeCode_Load(object sender, System.EventArgs e)
		{
			bool labelSet = false;
			foreach(SizeCodeProfile scp in _scl.ArrayList)
			{
				if (! labelSet)
				{
					this.label1.Text = this.LabelText;
					labelSet = true;
				}
				string listStr = scp.SizeCodeID;
				if (scp.SizeCodePrimary != null && scp.SizeCodePrimary.Trim().Length != 0)
				{
					listStr += "   -   " + scp.SizeCodePrimary;
				}
				if (scp.SizeCodeSecondary != null && scp.SizeCodeSecondary.Trim().Length != 0)
				{
					listStr += "   -   " + scp.SizeCodeSecondary;
				}
//				if (scp.SizeCodeHeading1.Trim().Length != 0 && scp.SizeCodeHeading1.Trim() != null)
//				{
//					listStr += "   -   " + scp.SizeCodeHeading1;
//				}
//				if (scp.SizeCodeHeading2.Trim().Length != 0 && scp.SizeCodeHeading2.Trim() != null)
//				{
//					listStr += "   -   " + scp.SizeCodeHeading2;
//				}
//				if (scp.SizeCodeHeading3.Trim().Length != 0 && scp.SizeCodeHeading3.Trim() != null)
//				{
//					listStr += "   -   " + scp.SizeCodeHeading3;
//				}
//				if (scp.SizeCodeHeading4.Trim().Length != 0 && scp.SizeCodeHeading4.Trim() != null)
//				{
//					listStr += "   -   " + scp.SizeCodeHeading4;
//				}
				lstSizeCode.Items.Add(listStr);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
