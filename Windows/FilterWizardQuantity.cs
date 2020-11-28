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
	/// Summary description for FilterWizardQuantity.
	/// </summary>
	public class FilterWizardQuantity : FilterWizardControl
	{
		#region Component Designer generated code
		private FilterWizardConditionQuantityPanel pnlQuantity;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label28;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboComparison;
		private System.Windows.Forms.Panel pnlTitle;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox txtQuantity;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FilterWizardQuantity()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public FilterWizardQuantity(frmFilterWizard aParentForm, int aConditionIdx)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_parentForm = aParentForm;

			_parentForm.SuspendLayout();
			_parentForm.Controls.Add(pnlQuantity);
			_parentForm.Controls.Add(pnlTitle);
			_parentForm.ResumeLayout(false);

			pnlQuantity.Index = aConditionIdx;
			pnlQuantity.Name += "-" + aConditionIdx;
			pnlQuantity.ParentControl = this;
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

				this.txtQuantity.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
				this.txtQuantity.TextChanged -= new System.EventHandler(this.txtQuantity_TextChanged);
			}
			base.Dispose( disposing );
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlQuantity = new MIDRetail.Windows.FilterWizardConditionQuantityPanel();
			this.label36 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.txtQuantity = new System.Windows.Forms.TextBox();
			this.cboComparison = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.pnlTitle = new System.Windows.Forms.Panel();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.pnlQuantity.SuspendLayout();
			this.pnlTitle.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlQuantity
			// 
			this.pnlQuantity.BackColor = System.Drawing.SystemColors.Control;
			this.pnlQuantity.BackPanel = null;
			this.pnlQuantity.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlQuantity.Controls.Add(this.label36);
			this.pnlQuantity.Controls.Add(this.label28);
			this.pnlQuantity.Controls.Add(this.txtQuantity);
			this.pnlQuantity.Controls.Add(this.cboComparison);
			this.pnlQuantity.DefaultControl = null;
			this.pnlQuantity.Index = 0;
			this.pnlQuantity.IsBackEnabled = true;
			this.pnlQuantity.IsNextEnabled = false;
			this.pnlQuantity.Location = new System.Drawing.Point(0, 56);
			this.pnlQuantity.Name = "pnlQuantity";
			this.pnlQuantity.NextPanel = null;
			this.pnlQuantity.NextText = "Next >";
			this.pnlQuantity.ParentControl = null;
			this.pnlQuantity.Size = new System.Drawing.Size(600, 280);
			this.pnlQuantity.TabIndex = 18;
			this.pnlQuantity.TitlePanel = this.pnlTitle;
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
			// txtQuantity
			// 
			this.txtQuantity.Location = new System.Drawing.Point(24, 80);
			this.txtQuantity.Name = "txtQuantity";
			this.txtQuantity.Size = new System.Drawing.Size(120, 20);
			this.txtQuantity.TabIndex = 48;
			this.txtQuantity.Text = "";
			this.txtQuantity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQuantity_KeyPress);
			this.txtQuantity.TextChanged += new System.EventHandler(this.txtQuantity_TextChanged);
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
			this.label7.Text = "Specify the quantity that the first variable in the condition will be compared to" +
				".";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label8.Location = new System.Drawing.Point(24, 8);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(256, 16);
			this.label8.TabIndex = 0;
			this.label8.Text = "Quantity Definition";
			// 
			// FilterWizardQuantity
			// 
			this.Controls.Add(this.pnlTitle);
			this.Controls.Add(this.pnlQuantity);
			this.Name = "FilterWizardQuantity";
			this.Size = new System.Drawing.Size(600, 336);
			this.pnlQuantity.ResumeLayout(false);
			this.pnlTitle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public FilterWizardConditionQuantityPanel MainPanel
		{
			get
			{
				return pnlQuantity;
			}
		}

		public System.Windows.Forms.TextBox QuantityTextBox
		{
			get
			{
				return txtQuantity;
			}
		}

        public MIDRetail.Windows.Controls.MIDComboBoxEnh ComparisonCombo
		{
			get
			{
				return cboComparison;
			}
		}

		private void txtQuantity_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				if (txtQuantity.Text.Length > 0)
				{
					pnlQuantity.IsNextEnabled = true;
				}
				else
				{
					pnlQuantity.IsNextEnabled = false;
				}

				FireNextButtonStatusChangedEvent(pnlQuantity.IsNextEnabled);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void txtQuantity_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				if (e.KeyChar == 13 || e.KeyChar == 9)
				{
				}
				else if (e.KeyChar == 27)
				{
				}
				else if (Char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || Char.IsControl(e.KeyChar))
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
