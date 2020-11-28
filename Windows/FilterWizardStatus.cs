using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for FilterWizardStatus.
	/// </summary>
	public class FilterWizardStatus : FilterWizardControl
	{
		#region Component Designer generated code
		private FilterWizardConditionStatusPanel pnlStatus;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label28;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboComparison;
		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboStatus;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterWizardStatus()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FilterWizardStatus(frmFilterWizard aParentForm, int aConditionIdx)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_parentForm = aParentForm;

			_parentForm.SuspendLayout();
			_parentForm.Controls.Add(pnlStatus);
			_parentForm.Controls.Add(pnlTitle);
			_parentForm.ResumeLayout(false);

			pnlStatus.Index = aConditionIdx;
			pnlStatus.Name += "-" + aConditionIdx;
			pnlStatus.ParentControl = this;
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

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlStatus = new MIDRetail.Windows.FilterWizardConditionStatusPanel();
			this.label36 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.cboStatus = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.cboComparison = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.pnlStatus.SuspendLayout();
			this.pnlTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlStatus
			// 
			this.pnlStatus.BackColor = System.Drawing.SystemColors.Control;
			this.pnlStatus.BackPanel = null;
			this.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlStatus.Controls.Add(this.label36);
			this.pnlStatus.Controls.Add(this.label28);
			this.pnlStatus.Controls.Add(this.cboStatus);
			this.pnlStatus.Controls.Add(this.cboComparison);
			this.pnlStatus.DefaultControl = null;
			this.pnlStatus.Index = 0;
			this.pnlStatus.IsBackEnabled = true;
			this.pnlStatus.IsNextEnabled = true;
			this.pnlStatus.Location = new System.Drawing.Point(0, 56);
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.NextPanel = null;
			this.pnlStatus.NextText = "Next >";
			this.pnlStatus.ParentControl = null;
			this.pnlStatus.Size = new System.Drawing.Size(600, 280);
			this.pnlStatus.TabIndex = 18;
			this.pnlStatus.TitlePanel = this.pnlTitle;
			// 
			// label36
			// 
			this.label36.Location = new System.Drawing.Point(24, 64);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(80, 16);
			this.label36.TabIndex = 47;
			this.label36.Text = "Value:";
			// 
			// label28
			// 
			this.label28.Location = new System.Drawing.Point(24, 24);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(80, 16);
			this.label28.TabIndex = 46;
			this.label28.Text = "Condition:";
			// 
			// cboStatus
			// 
			this.cboStatus.Location = new System.Drawing.Point(24, 80);
			this.cboStatus.Name = "cboStatus";
			this.cboStatus.Size = new System.Drawing.Size(120, 21);
			this.cboStatus.TabIndex = 50;
			// 
			// cboComparison
			// 
			this.cboComparison.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboComparison.Location = new System.Drawing.Point(24, 40);
			this.cboComparison.Name = "cboComparison";
			this.cboComparison.Size = new System.Drawing.Size(121, 21);
			this.cboComparison.TabIndex = 45;
			// 
			// pnlTitle
			// 
			this.pnlTitle.BackColor = System.Drawing.SystemColors.Window;
			this.pnlTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlTitle.Controls.Add(this.label7);
			this.pnlTitle.Controls.Add(this.label8);
			this.pnlTitle.Location = new System.Drawing.Point(0, 0);
			this.pnlTitle.Name = "pnlTitle";
			this.pnlTitle.Size = new System.Drawing.Size(600, 56);
			this.pnlTitle.TabIndex = 19;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label7.Location = new System.Drawing.Point(48, 32);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(472, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Specify the Status that the first variable in the condition will be compared to.";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(24, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Status Definition";
			// 
			// FilterWizardStatus
			// 
			this.Controls.Add(this.pnlTitle);
			this.Controls.Add(this.pnlStatus);
			this.Name = "FilterWizardStatus";
			this.Size = new System.Drawing.Size(600, 336);
			this.pnlStatus.ResumeLayout(false);
			this.pnlTitle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public FilterWizardConditionStatusPanel MainPanel
		{
			get
			{
				return pnlStatus;
			}
		}

        public MIDRetail.Windows.Controls.MIDComboBoxEnh StatusCombo
		{
			get
			{
				return cboStatus;
			}
		}

		public MIDRetail.Windows.Controls.MIDComboBoxEnh ComparisonCombo
		{
			get
			{
				return cboComparison;
			}
		}
	}
}
