using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// This control is used to allow users a short-cut
	/// type filtering of the active window. For some
	/// windows, this may not be valid, in which case this
	/// control should throw an error.
	/// </summary>
	public class MIDQuickFilter : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.Label label1;
		private MIDRetail.Windows.Controls.MIDComboBoxEnh cboQuickFilter;
		private System.Windows.Forms.Button btnGo;
		
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Standard constructor
		/// </summary>
		public MIDQuickFilter()
		{
			InitializeComponent();
			BindFilterCombo();
		}

		/// <summary>
		/// Standard deconstrtuction
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				this.btnGo.Click -= new System.EventHandler(this.btnGo_Click);
				
				Include.DisposeControls(this.Controls);
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cboQuickFilter = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
			this.btnGo = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Quick Filter:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cboQuickFilter
			// 
			this.cboQuickFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cboQuickFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboQuickFilter.Location = new System.Drawing.Point(65, 0);
			this.cboQuickFilter.Name = "cboQuickFilter";
			this.cboQuickFilter.Size = new System.Drawing.Size(136, 21);
			this.cboQuickFilter.TabIndex = 1;
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(204, 0);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(32, 21);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// MIDQuickFilter
			// 
			this.Controls.Add(this.btnGo);
			this.Controls.Add(this.cboQuickFilter);
			this.Controls.Add(this.label1);
			this.Name = "MIDQuickFilter";
			this.Size = new System.Drawing.Size(256, 26);
			this.ResumeLayout(false);

		}
		#endregion

		private void BindFilterCombo()
		{	
			try 
			{
				PopulateCommonCriteria((int)eQuickFilterSelectionType.AllocationHeader,
					(int)eQuickFilterSelectionType.Variable, this.cboQuickFilter.ComboBox,
					string.Empty);
			}
			catch (Exception ex)
			{
				throw (ex);
			}
		}

		/// <summary>
		/// Generic method to populate all MIDComboBoxes that use MIDText.GetLabels function
		/// </summary>
		/// <param name="startVal">enum start value</param>
		/// <param name="endVal">enum end value</param>
		/// <param name="CboBox">MIDComboBox control name</param>
		/// <param name="selectVal">selected value for combo box</param>
		private void PopulateCommonCriteria(int startVal, int endVal, ComboBox CboBox, string selectVal) 
		{
			try
			{
				DataRow dr;
				DataTable dt = MIDEnvironment.CreateDataTable();
				dt = MIDText.GetLabels(startVal, endVal);
				dr = dt.NewRow();
				dr["TEXT_CODE"] = 0;
				dr["TEXT_VALUE"] = string.Empty;
				dt.Rows.Add(dr);
				// Temporarily remove Size * Color Group until each is implemented  
				for (int i = dt.Rows.Count - 1; i >= 0; i--)
				{
					DataRow dRow = dt.Rows[i];
					if ( Convert.ToInt32(dRow["TEXT_CODE"])  ==  Convert.ToInt32(eQuickFilterSelectionType.ColorGroup)
						|| 	Convert.ToInt32(dRow["TEXT_CODE"]) == Convert.ToInt32(eQuickFilterSelectionType.Size)
						) 
					{
						dt.Rows.Remove(dRow);
					}
				}	 

				// Switch to DataView in order to sort
				DataView dv = new DataView(dt);
				dv.Sort = "TEXT_CODE";

				CboBox.DisplayMember = "TEXT_VALUE";
				CboBox.ValueMember = "TEXT_CODE";
				CboBox.DataSource = dv;
		
				CboBox.SelectedValue = -1;
			}
			catch(Exception ex)
			{
				throw(ex);
			}
		}
		private void btnGo_Click(object sender, System.EventArgs e)
		{
			 
		}
	}
}
