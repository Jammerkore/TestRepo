using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for DateSelectorSingle.
	/// </summary>
	public class DateSelectorSingle : System.Windows.Forms.Form
	{
		private DateTime _selectedDate;
		private System.Windows.Forms.MonthCalendar monthCalendar1;
		private System.ComponentModel.Container components = null;
		public DateTime SelectedDate {get{return _selectedDate;} set{_selectedDate = value;}}
		private DateTime _mouseDownTime, _prevMouseDownTime, _prevSelectedDate;

		public DateSelectorSingle()
		{
			InitializeComponent();		
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				this.monthCalendar1.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.monthCalendar1_KeyDown);
				this.monthCalendar1.DateSelected -= new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
				this.monthCalendar1.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.monthCalendar1_MouseDown);
				this.Load -= new System.EventHandler(this.DateSelectorSingle_Load);
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
			this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
			this.SuspendLayout();
			// 
			// monthCalendar1
			// 
			this.monthCalendar1.Location = new System.Drawing.Point(0, -8);
			this.monthCalendar1.MaxSelectionCount = 1;
			this.monthCalendar1.Name = "monthCalendar1";
			this.monthCalendar1.ShowToday = false;
			this.monthCalendar1.TabIndex = 0;
			this.monthCalendar1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.monthCalendar1_KeyDown);
			this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
			this.monthCalendar1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.monthCalendar1_MouseDown);
			// 
			// DateSelectorSingle
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(192, 149);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.monthCalendar1});
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DateSelectorSingle";
			this.Text = "Date Selector";
			this.Load += new System.EventHandler(this.DateSelectorSingle_Load);
			this.ResumeLayout(false);

		}
		#endregion

		

		private void DateSelectorSingle_Load(object sender, System.EventArgs e)
		{
			if (SelectedDate != Include.UndefinedDate)
				monthCalendar1.SetDate(SelectedDate);
		}

		private void monthCalendar1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Close();
		}

		private void monthCalendar1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_prevMouseDownTime = _mouseDownTime;
			_mouseDownTime = DateTime.Now;
			_prevSelectedDate = _selectedDate;
		}

		private void monthCalendar1_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
		{
			_selectedDate = monthCalendar1.SelectionStart;
			// This is here to catch a double-click
			if(_mouseDownTime < _prevMouseDownTime.AddMilliseconds(SystemInformation.DoubleClickTime)
				&& monthCalendar1.SelectionStart.Date == _prevSelectedDate.Date)
			{
				Close();
			}
		}
	}
}
