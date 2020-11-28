using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using Infragistics.Win.UltraWinGrid;
using Infragistics.Shared;
using Infragistics.Win;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows.Controls
{
	
	/// <summary>
	/// Summary description for MIDComboBox.
	/// </summary>
	public class MIDComboBox : System.Windows.Forms.UserControl
	{
		// add event to update explorer when hierarchy is changed
		public delegate void SelectedIndexChangedEventHandler(object source, MIDComboBoxChangeEventArgs e);
		public event SelectedIndexChangedEventHandler SelectedIndexChanged;
		private Infragistics.Win.UltraWinGrid.UltraCombo ultraCombo1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private string _displayMember;

		
		public object DataSource
		//public DataTable DataSource
		{
			set
			{
//				this.ultraDropDown1.DataSource = value;
				this.ultraCombo1.DataSource = value;
			}
		}

		public string DisplayMember
		{
			set
			{
				this.ultraCombo1.DisplayMember = value;
				_displayMember = value;
//				this.ultraCombo1.DisplayLayout.Bands[0].Columns[value].ValueList = ultraDropDown1;
			}
		}

		public string ValueMember
		{
			set
			{
				this.ultraCombo1.ValueMember = value;
			}
			get
			{
				return this.ultraCombo1.ValueMember;
			}
		}

		public string SelectedValue
		{
			set
			{
				this.ultraCombo1.Value = value;
			}
			get 
			{
				return this.ultraCombo1.Value.ToString();
			}
		}

		public override string Text
		{
			get
			{
				return this.ultraCombo1.Text;
			}
		}

		public MIDComboBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitForm call
			this.ultraCombo1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
			this.ultraCombo1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
			this.ultraCombo1.DisplayLayout.Bands[0].ColHeadersVisible = false;
//			this.ultraDropDown1.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.None;
//			this.ultraDropDown1.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.None;
//			this.ultraDropDown1.DisplayLayout.Bands[0].ColHeadersVisible = false;
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

				this.ultraCombo1.RowSelected -= new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.ultraCombo1_RowSelected);

				if (SelectedIndexChanged != null)
				{
					foreach (SelectedIndexChangedEventHandler handler in SelectedIndexChanged.GetInvocationList())
					{
						SelectedIndexChanged -= handler;
					}
				}

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
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			this.ultraCombo1 = new Infragistics.Win.UltraWinGrid.UltraCombo();
			((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).BeginInit();
			this.SuspendLayout();
			// 
			// ultraCombo1
			// 
			this.ultraCombo1.Anchor = ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.ultraCombo1.Appearance = appearance1;
			this.ultraCombo1.AutoEdit = true;
			this.ultraCombo1.DisplayMember = "";
			this.ultraCombo1.Name = "ultraCombo1";
			this.ultraCombo1.Size = new System.Drawing.Size(112, 21);
			this.ultraCombo1.TabIndex = 0;
			this.ultraCombo1.ValueMember = "";
			this.ultraCombo1.RowSelected += new Infragistics.Win.UltraWinGrid.RowSelectedEventHandler(this.ultraCombo1_RowSelected);
			// 
			// MIDComboBox
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.ultraCombo1});
			this.Name = "MIDComboBox";
			this.Size = new System.Drawing.Size(112, 24);
			((System.ComponentModel.ISupportInitialize)(this.ultraCombo1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public void HideColumn(string columnName)
		{
			this.ultraCombo1.DisplayLayout.Bands[0].Columns[columnName].Hidden = true;
		}

		private void ultraCombo1_RowSelected(object sender, Infragistics.Win.UltraWinGrid.RowSelectedEventArgs e)
		{
			MIDComboBoxChangeEventArgs ea = new MIDComboBoxChangeEventArgs();
			ea.SelectedRowIndex = e.Row.ListIndex;
			ea.SelectedRowText = e.Row.Cells[_displayMember].Text;
			if (SelectedIndexChanged != null)
			{
				SelectedIndexChanged(this, ea);
			}
		}

	}

	public class MIDComboBoxChangeEventArgs
	{
		int _selectedRowIndex;
		string _selectedRowText;
		 
		public MIDComboBoxChangeEventArgs()
		{
			
		}

		public int SelectedRowIndex 
		{
			get { return _selectedRowIndex ; }
			set { _selectedRowIndex = value; }
		}

		public string SelectedRowText 
		{
			get { return _selectedRowText ; }
			set { _selectedRowText = value; }
		}
		
	}
}
