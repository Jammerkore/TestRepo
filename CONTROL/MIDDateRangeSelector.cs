using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using MIDRetail.Windows;
using MIDRetail.Common;
using MIDRetail.DataCommon;


namespace MIDRetail.Windows.Controls
{
	/// <summary>
	/// Summary description for MIDDateRangeSelector.
	/// </summary>
	public class MIDDateRangeSelector : System.Windows.Forms.UserControl
	{
		// add event to update explorer when hierarchy is changed
		public delegate void SelectionEventHandler(object source, DateRangeSelectorEventArgs e);
		public event SelectionEventHandler OnSelection;

		private DataTable _dateGridTable;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Form _frm = null;
		private int _dateRangeRID;
		private Stack _clickEventHandlers;
		private Stack _clickCellButtonEventHandlers;

		public override string Text 
		{
			get
			{
				if (ultraGrid1.Rows[0].Cells["Date Range"].Value != System.DBNull.Value)
				{
					return (string)ultraGrid1.Rows[0].Cells["Date Range"].Value;
				}
				else
				{
					return "";
				}
			}
			set 
			{ 
				_dateGridTable.Clear(); 
				_dateGridTable.Rows.Add(new object[] { value});
				ultraGrid1.DataSource = _dateGridTable;
			}
		}

//Begin Track #5111 - JScott - Add additional filter functionality
//		new public bool Enabled
//		{
//			get
//			{
//				return (ultraGrid1.DisplayLayout.Override.AllowUpdate == Infragistics.Win.DefaultableBoolean.True);
//			}
//			set
//			{
//				if (value)
//				{
//					ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
//				}
//				else
//				{
//					ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
//				}
//			}
//		}
//
		new public bool Enabled
		{
			get
			{
				return base.Enabled;
			}
			set
			{
				base.Enabled = value;
				ultraGrid1.Enabled = value;
			}
		}

//End Track #5111 - JScott - Add additional filter functionality
		public int DateRangeRID 
		{
			get { return _dateRangeRID; }
			set { _dateRangeRID = value; }
		}

		public System.Windows.Forms.Form DateRangeForm 
		{
			get { return _frm; }
			set { _frm = value; }
		}

		public MIDDateRangeSelector()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            // Begin TT#2972 - RMatelic - Select Date Range highlights months at a time
            ultraGrid1.StyleLibraryName = "Windows7NoHotTrackLibraryName";
            // End TT#2972 
			// TODO: Add any initialization after the InitForm call
			buildDateRangeGrid();

			_clickEventHandlers = new Stack();
			_clickCellButtonEventHandlers = new Stack();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
//				if (_frm != null)
//				{
//					_frm.Dispose();
					_frm = null;
//				}
				if(components != null)
				{
					components.Dispose();
				}

				this.ultraGrid1.Resize -= new System.EventHandler(this.ultraGrid1_Resize);
				this.ultraGrid1.ClickCellButton -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_ClickCellButton);

				if (OnSelection != null)
				{
					foreach (SelectionEventHandler handler in OnSelection.GetInvocationList())
					{
						OnSelection -= handler;
					}
				}

				while (_clickEventHandlers.Count > 0)
				{
					ultraGrid1.Click -= (EventHandler)_clickEventHandlers.Pop();
				}

				while (_clickCellButtonEventHandlers.Count > 0)
				{
					ultraGrid1.ClickCellButton -= (CellEventHandler)_clickCellButtonEventHandlers.Pop();
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
			this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
			this.SuspendLayout();
			// 
			// ultraGrid1
			// 
			this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ultraGrid1.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
			this.ultraGrid1.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.None;
			this.ultraGrid1.Location = new System.Drawing.Point(0, 1);
			this.ultraGrid1.Name = "ultraGrid1";
			this.ultraGrid1.Size = new System.Drawing.Size(160, 24);
			this.ultraGrid1.TabIndex = 1;
			this.ultraGrid1.Resize += new System.EventHandler(this.ultraGrid1_Resize);
			// 
			// MIDDateRangeSelector
			// 
			this.Controls.Add(this.ultraGrid1);
			this.Name = "MIDDateRangeSelector";
			this.Size = new System.Drawing.Size(160, 24);
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		public new event EventHandler Click
		{
			add
			{
				ultraGrid1.Click += value;
				_clickEventHandlers.Push(value);
			}
			remove
			{
				ultraGrid1.Click -= value;
			}
		}

		public event CellEventHandler ClickCellButton
		{
			add
			{
				ultraGrid1.ClickCellButton += value;
				_clickCellButtonEventHandlers.Push(value);
			}
			remove
			{
				ultraGrid1.ClickCellButton -= value;
			}
		}

		private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			
		}

		private void buildDateRangeGrid()
		{
			//Create Columns and rows for datatable
			_dateGridTable = MIDEnvironment.CreateDataTable("_dateGridTable");
			DataColumn edDataColumn;
			edDataColumn = new DataColumn();
			edDataColumn.DataType = System.Type.GetType("System.String");
			edDataColumn.ColumnName = "Date Range";
			edDataColumn.Caption = "Date Range";
			edDataColumn.ReadOnly = true;
			edDataColumn.Unique = true;
			_dateGridTable.Columns.Add(edDataColumn);
			_dateGridTable.Rows.Add(new object[] { " "});
			ultraGrid1.DataSource = _dateGridTable;

			this.ultraGrid1.DisplayLayout.Appearance.ImageBackgroundStyle = Infragistics.Win.ImageBackgroundStyle.Stretched;
			this.ultraGrid1.DisplayLayout.AddNewBox.Hidden = true;
			this.ultraGrid1.DisplayLayout.AddNewBox.Prompt = "";
			this.ultraGrid1.DisplayLayout.Bands[0].AddButtonCaption = "";
			this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;
			this.ultraGrid1.DisplayLayout.GroupByBox.Prompt = "";
			this.ultraGrid1.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
			this.ultraGrid1.DisplayLayout.Bands[0].ColHeadersVisible = false;
			this.ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			this.ultraGrid1.DisplayLayout.Bands[0].Columns["Date Range"].Width = this.ultraGrid1.Width - 5;
			this.ultraGrid1.DisplayLayout.Bands[0].Columns["Date Range"].Style  = Infragistics.Win.UltraWinGrid.ColumnStyle.EditButton;
			this.ultraGrid1.ClickCellButton += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_ClickCellButton);
		}

		private void ultraGrid1_ClickCellButton(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
//			//MessageBox.Show("popup date selector here");
//			Point pt = this.Location;
//			int wd = this.Size.Width;
//			pt.X += wd;
//			_frm.StartPosition = FormStartPosition.Manual;
//			_frm.SetDesktopLocation(pt.X, pt.Y);
//
//			DateRangeSelectorEventArgs arg = new DateRangeSelectorEventArgs();
//
//			DialogResult DateRangeResult = _frm.ShowDialog();
//
//			if (DateRangeResult == DialogResult.OK)
//			{
//				arg.SelectedDateRange = (DateRangeProfile)_frm.Tag;
//				arg.SelectionCanceled = false;
//				this.Text = arg.SelectedDateRange.DisplayDate;
//				if (OnSelection != null)
//				{
//					OnSelection(this, arg);
//				}
//			}
//			else
//			{
//				arg.SelectionCanceled = true;
//				if (OnSelection != null)
//				{
//					OnSelection(this, arg);
//				}
//			}
		}


		/// <summary>
		/// Displays the date range selector
		/// </summary>
		public void ShowSelector()
		{
//			Point pt = this.Location;
//			int wd = this.Size.Width;
//			pt.X += wd;
//			_frm.StartPosition = FormStartPosition.Manual;
//			_frm.SetDesktopLocation(pt.X, pt.Y);
			if (ultraGrid1.DisplayLayout.Override.AllowUpdate == Infragistics.Win.DefaultableBoolean.True ||
				ultraGrid1.DisplayLayout.Override.AllowUpdate == Infragistics.Win.DefaultableBoolean.Default)
			{
				_frm.StartPosition = FormStartPosition.CenterScreen;

				DateRangeSelectorEventArgs arg = new DateRangeSelectorEventArgs();

				DialogResult DateRangeResult = _frm.ShowDialog();

				if (DateRangeResult == DialogResult.OK)
				{
					arg.SelectedDateRange = (DateRangeProfile)_frm.Tag;
					arg.SelectionCanceled = false;
					this.Text = arg.SelectedDateRange.DisplayDate;
					this._dateRangeRID = arg.SelectedDateRange.Key;
					if (OnSelection != null)
					{
						OnSelection(this, arg);
					}
				}
				else
				{
					arg.SelectionCanceled = true;
					if (OnSelection != null)
					{
						OnSelection(this, arg);
					}
				}
			}
		}

		private void ultraGrid1_Resize(object sender, System.EventArgs e)
		{
            //Begin TT#1449-MD -jsobek -Store Filter - In the window moving the bar around to make the section wider and received an unhandled exception.
            int newWidth = this.ultraGrid1.Width - 5;
            if (newWidth < 0)
            {
                newWidth = 0;
            }
            this.ultraGrid1.DisplayLayout.Bands[0].Columns["Date Range"].Width = newWidth;
            //End TT#1449-MD -jsobek -Store Filter - In the window moving the bar around to make the section wider and received an unhandled exception.
		}

		public void SetImage(Image aImage)
		{
			this.ultraGrid1.Rows[0].Cells["Date Range"].Appearance.Image = aImage;
		}
	}

	public class DateRangeSelectorEventArgs : EventArgs
	{
		private bool _selectionCanceled;
		private string _displayDate;
		private DateRangeProfile _selectedDateRange;

		public DateRangeSelectorEventArgs()
		{
			_selectionCanceled = false;
			//_selectedDateRange = new DateRangeProfile();
			_selectedDateRange = null;
		}
		public bool SelectionCanceled 
		{
			get { return _selectionCanceled ; }
			set { _selectionCanceled = value; }
		}
		public string displayDate 
		{
			get { return _displayDate ; }
			set { _displayDate = value; }
		}
		public DateRangeProfile SelectedDateRange 
		{
			get { return _selectedDateRange ; }
			set { _selectedDateRange = value; }
		}
	}
}
