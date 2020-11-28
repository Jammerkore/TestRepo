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
	/// Summary description for FilterWizardGrade.
	/// </summary>
	public class FilterWizardGrade : FilterWizardControl
	{
		#region Component Designer generated code
		private FilterWizardConditionGradePanel pnlGrade;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label28;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboComparison;
		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtGrade;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterWizardGrade()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FilterWizardGrade(frmFilterWizard aParentForm, int aConditionIdx)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_parentForm = aParentForm;

			_parentForm.SuspendLayout();
			_parentForm.Controls.Add(pnlGrade);
			_parentForm.Controls.Add(pnlTitle);
			_parentForm.ResumeLayout(false);

			pnlGrade.Index = aConditionIdx;
			pnlGrade.Name += "-" + aConditionIdx;
			pnlGrade.ParentControl = this;
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

				this.txtGrade.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtGrade_KeyPress);
				this.txtGrade.TextChanged -= new System.EventHandler(this.txtGrade_TextChanged);
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlGrade = new MIDRetail.Windows.FilterWizardConditionGradePanel();
			this.label36 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.txtGrade = new System.Windows.Forms.TextBox();
			this.cboComparison = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.pnlGrade.SuspendLayout();
			this.pnlTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlGrade
			// 
			this.pnlGrade.BackColor = System.Drawing.SystemColors.Control;
			this.pnlGrade.BackPanel = null;
			this.pnlGrade.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlGrade.Controls.Add(this.label36);
			this.pnlGrade.Controls.Add(this.label28);
			this.pnlGrade.Controls.Add(this.txtGrade);
			this.pnlGrade.Controls.Add(this.cboComparison);
			this.pnlGrade.DefaultControl = null;
			this.pnlGrade.Index = 0;
			this.pnlGrade.IsBackEnabled = true;
			this.pnlGrade.IsNextEnabled = false;
			this.pnlGrade.Location = new System.Drawing.Point(0, 56);
			this.pnlGrade.Name = "pnlGrade";
			this.pnlGrade.NextPanel = null;
			this.pnlGrade.NextText = "Next >";
			this.pnlGrade.ParentControl = null;
			this.pnlGrade.Size = new System.Drawing.Size(600, 280);
			this.pnlGrade.TabIndex = 18;
			this.pnlGrade.TitlePanel = this.pnlTitle;
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
			// txtGrade
			// 
			this.txtGrade.Location = new System.Drawing.Point(24, 80);
			this.txtGrade.Name = "txtGrade";
			this.txtGrade.Size = new System.Drawing.Size(120, 20);
			this.txtGrade.TabIndex = 48;
			this.txtGrade.Text = "";
			this.txtGrade.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGrade_KeyPress);
			this.txtGrade.TextChanged += new System.EventHandler(this.txtGrade_TextChanged);
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
			this.label7.Text = "Specify the Grade that the first variable in the condition will be compared to" +
				".";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(24, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Grade Definition";
			// 
			// FilterWizardGrade
			// 
			this.Controls.Add(this.pnlTitle);
			this.Controls.Add(this.pnlGrade);
			this.Name = "FilterWizardGrade";
			this.Size = new System.Drawing.Size(600, 336);
			this.pnlGrade.ResumeLayout(false);
			this.pnlTitle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public FilterWizardConditionGradePanel MainPanel
		{
			get
			{
				return pnlGrade;
			}
		}

		public System.Windows.Forms.TextBox GradeTextBox
		{
			get
			{
				return txtGrade;
			}
		}

        public MIDRetail.Windows.Controls.MIDComboBoxEnh ComparisonCombo
		{
			get
			{
				return cboComparison;
			}
		}

		private void txtGrade_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtGrade.Text.Length > 0)
				{
					pnlGrade.IsNextEnabled = true;
				}
				else
				{
					pnlGrade.IsNextEnabled = false;
				}

				FireNextButtonStatusChangedEvent(pnlGrade.IsNextEnabled);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void txtGrade_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
				}
				else if (e.KeyChar == 27)
				{
				}
				else if (Char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
				{
				}
				else
				{
					e.Handled = true;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
