using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
//using System.Collections;
using System.Globalization;
//using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinScrollBar;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for DynamicSwitchDateSelector.
	/// </summary>
	public class DynamicSwitchDateSelector : MIDFormBase
	{
		private SessionAddressBlock _sab;
		private MRSCalendar _calendar;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		private Color _bColor;
		private Color _fColor;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnOk;

		private WeekProfile _selectedWeek;
		private ArrayList _selectedDates;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.Button btnCancel;

		public WeekProfile SelectedWeek 
		{
			get{return _selectedWeek;}
			set{_selectedWeek = value;}
		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DynamicSwitchDateSelector(SessionAddressBlock sab) 
		{
			InitializeComponent();
			_sab = sab;
			_calendar = _sab.ClientServerSession.Calendar;
			ultraGrid1.DataSource = _calendar.DateSelectionDataTable;
			_selectedDates = new ArrayList();
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

		private void DynamicSwitchDateSelector_Load(object sender, System.EventArgs e)
		{
			FormLoaded = false;

			if (this.SelectedWeek != null)
				SetDateRangeSelection(SelectedWeek);
			SetText();
			InitializeGridShading();

			FormLoaded = true;
			SetReadOnly(true); 
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lbMsg = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// utmMain
			// 
			this.utmMain.MenuSettings.ForceSerialization = true;
			this.utmMain.ToolbarSettings.ForceSerialization = true;
			// 
			// ultraGrid1
			// 
			this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.ultraGrid1.DisplayLayout.GroupByBox.Hidden = true;
			this.ultraGrid1.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.NotAllowed;
			this.ultraGrid1.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.Single;
			this.ultraGrid1.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.ultraGrid1.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.None;
			this.ultraGrid1.Location = new System.Drawing.Point(0, 0);
			this.ultraGrid1.Name = "ultraGrid1";
			this.ultraGrid1.Size = new System.Drawing.Size(198, 264);
			this.ultraGrid1.TabIndex = 4;
			this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
			this.ultraGrid1.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ultraGrid1_AfterSelectChange);
			// 
			// btnCancel
			// 
			this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnCancel.Location = new System.Drawing.Point(110, 325);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnOk.Location = new System.Drawing.Point(13, 325);
			this.btnOk.Name = "btnOk";
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "&Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.lbMsg);
			this.groupBox1.Location = new System.Drawing.Point(2, 265);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(195, 55);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// lbMsg
			// 
			this.lbMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lbMsg.Location = new System.Drawing.Point(9, 15);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(176, 30);
			this.lbMsg.TabIndex = 0;
			// 
			// DynamicSwitchDateSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(198, 355);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.ultraGrid1);
			this.Name = "DynamicSwitchDateSelector";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Dynamic Switch Date Selector";
			this.Load += new System.EventHandler(this.DynamicSwitchDateSelector_Load);
			this.Controls.SetChildIndex(this.ultraGrid1, 0);
			this.Controls.SetChildIndex(this.btnCancel, 0);
			this.Controls.SetChildIndex(this.btnOk, 0);
			this.Controls.SetChildIndex(this.groupBox1, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			_bColor = this.BackColor;
			_fColor = this.ForeColor;

			ultraGrid1.DisplayLayout.Override.SelectTypeRow = SelectType.Extended;
			ultraGrid1.DisplayLayout.Override.CellClickAction = CellClickAction.CellSelect;
			ultraGrid1.DisplayLayout.MaxRowScrollRegions = 1;
			//ultraGrid1.DisplayLayout.ScrollStyle = ScrollStyle.Immediate;

			this.ultraGrid1.DisplayLayout.Bands[0].AutoPreviewEnabled = true; 

			ultraGrid1.DisplayLayout.Bands[0].ColHeadersVisible = false;
			ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
			ultraGrid1.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True;
			ultraGrid1.DisplayLayout.Override.AllowAddNew = AllowAddNew.No;
			ultraGrid1.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Year"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Period"].Hidden = true;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].Width = 75;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].Width = 20;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].Width = 20;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.FontData.SizeInPoints = 7;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.FontData.SizeInPoints = 7;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellActivation = Activation.Disabled;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week1"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week2"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week3"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week4"].CellActivation = Activation.ActivateOnly;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Week5"].CellActivation = Activation.ActivateOnly;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.BackColor = _bColor;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.ForeColorDisabled = _fColor;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].CellAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True;

			ultraGrid1.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.CellSelect;
		}

		private int DetermineSelectedDate()
		{
		int selectedDate = 9999999;

			GetSelectedCells();
			
			foreach (UltraGridCell aCell in _selectedDates)
			{
				//if ((int)aCell.Tag > endDate)
				//	endDate = (int)aCell.Tag;

				if ((int)aCell.Tag < selectedDate)
					selectedDate = (int)aCell.Tag;
			}

			return selectedDate;
		}

		private void GetSelectedCells()
		{
			_selectedDates.Clear();

			foreach (UltraGridCell aCell in ultraGrid1.Selected.Cells)
			{
				if (aCell.Text != "" && aCell.Column.Index > 2)
				{
					string ww = aCell.Text.PadLeft(2,'0');
					string yyyyww = aCell.Row.Cells["year"].Value.ToString() + ww;
					aCell.Tag = Convert.ToInt32(yyyyww, CultureInfo.CurrentUICulture);

					_selectedDates.Add(aCell);
				}
			}
		}

		private void SetDateRangeSelection(WeekProfile selectedWeek)
		{
			int year;
			int week = 0;
			int rowCount = ultraGrid1.Rows.Count;
			int colCount = 8;
			ultraGrid1.BeginUpdate();

			UltraGridRow firstRow = null;

			for (int r=0;r<rowCount;r++)
			{
				year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);

				{
					year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);

					for (int c=3;c<colCount;c++)
					{
						if (ultraGrid1.Rows[r].Cells[c].Value != System.DBNull.Value)
						{
							week = Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture);

							if (year == selectedWeek.FiscalYear && week == selectedWeek.WeekInYear) 
							{
								ultraGrid1.Rows[r].Cells[c].Selected = true;
								firstRow = ultraGrid1.Rows[r];
							}
						}
						else
						{
//							if (_selectedDateRange.SelectedDateType == eCalendarDateType.Week && prevWeekselected)
//							{
//								ultraGrid1.Rows[r].Cells[c].Selected = true;
//								prevWeekselected = true;
//
//								// catch first row selected
//								if (firstRow == null)
//									firstRow = ultraGrid1.Rows[r];
//							}
						}
					}
				}
			}

			// scroll row into view
			firstRow = firstRow.GetSibling(SiblingRow.Previous);
			ultraGrid1.DisplayLayout.RowScrollRegions[0].ScrollRowIntoView(firstRow);

			// make sure it's the top row.
			int limit = 0;
			do
			{
				limit++;
				if (limit > 14)
					break;
				if (ultraGrid1.DisplayLayout.RowScrollRegions[0].VisibleRows[0].Row == firstRow)
				{
					ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineUp);
					ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineUp);
					ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineUp);
					ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineUp);
					ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineUp);
					break;
				}

				ultraGrid1.DisplayLayout.RowScrollRegions[0].Scroll(RowScrollAction.LineDown);
			} while (true);

			
			ultraGrid1.EndUpdate();
		}

		private void InitializeGridShading()
		{
			int rowCount = ultraGrid1.Rows.Count;
			int colCount = 8;
			for (int r=0;r<rowCount;r++)
			{
				// applies coloring to year break headers
				int year = Convert.ToInt32(ultraGrid1.Rows[r].Cells["year"].Value, CultureInfo.CurrentUICulture);
				if (ultraGrid1.Rows[r].Cells["period"].Value == DBNull.Value)
				{
					ultraGrid1.Rows[r].Cells["week1"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week1"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week1"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					ultraGrid1.Rows[r].Cells["week2"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week2"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week2"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					ultraGrid1.Rows[r].Cells["week3"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week3"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week3"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					ultraGrid1.Rows[r].Cells["week4"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week4"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week4"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
					ultraGrid1.Rows[r].Cells["week5"].Appearance.BackColor = _bColor;
					ultraGrid1.Rows[r].Cells["week5"].Appearance.ForeColorDisabled = _fColor;
					ultraGrid1.Rows[r].Cells["week5"].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
				}

				for (int c=3;c<colCount;c++)
				{
					if (ultraGrid1.Rows[r].Cells[c].Value != System.DBNull.Value)
					{
						ultraGrid1.Rows[r].Cells[c].Appearance.ResetBackColor();
						ultraGrid1.Rows[r].Cells[c].Appearance.ResetForeColor();

						if ((year < _calendar.CurrentPeriod.FiscalYear) 
							|| (year == _calendar.CurrentPeriod.FiscalYear 
							&& Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture) < _calendar.CurrentWeek.WeekInYear))
						{
							ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = System.Drawing.Color.LightGray;
							ultraGrid1.Rows[r].Cells[c].Activation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
						}

						if (year == _calendar.CurrentPeriod.FiscalYear
							&& Convert.ToInt32(ultraGrid1.Rows[r].Cells[c].Value, CultureInfo.CurrentUICulture) == _calendar.CurrentWeek.WeekInYear)
						{
							ultraGrid1.Rows[r].Cells[c].Appearance.BackColor = System.Drawing.Color.SeaGreen;
							ultraGrid1.Rows[r].Cells[c].Appearance.ForeColor = System.Drawing.Color.White;
						}
					}
				}
			}
		}

		private void SetText()
		{
			try
			{
				this.btnOk.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
				lbMsg.Text = MIDText.GetTextOnly(eMIDTextCode.msg_EnterDynamicSwtchDate);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (ChangePending)
			{
				int selectedDate = DetermineSelectedDate();
				if (selectedDate < 9999999 && selectedDate > 0)
				{
					string yyyyww = selectedDate.ToString();
					int year = Convert.ToInt32(yyyyww.Substring(0,4));
					int week = Convert.ToInt32(yyyyww.Substring(4,2));
					this.SelectedWeek = _calendar.GetWeek(year, week);
					DialogResult = DialogResult.OK;
					ChangePending = false;
				}
			}
			Close();
		}

		private void ultraGrid1_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
			if (FormLoaded)
				ChangePending = true;
		}
	}
}
