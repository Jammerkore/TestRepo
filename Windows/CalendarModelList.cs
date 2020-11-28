using System;
using System.Drawing;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for CalendarModelList.
	/// </summary>
	public class CalendarModelList : MIDFormBase
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid ugridCalendarModelList;
		System.Data.DataTable _dtCalendarModel;

		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.Button btnClose;
		private System.ComponentModel.Container components = null;

		private int	_newModelRID;
		private string _newModelName;
		private int _newModelFiscalYear;
		private DateTime _newModelStartDate;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItemEdit;
		private System.Windows.Forms.MenuItem menuItemNew;
		private System.Windows.Forms.MenuItem menuItemDelete;
		private bool _updateApproved;
//		private int _deletedRID;
		private ArrayList _deletedModelsList;

		private DateTime _prevMouseDownTime;
		private DateTime _mouseDownTime;
		private SessionAddressBlock _SAB;
		CalendarModelMaint _calendarModelForm;

		private CalendarData _cd;	// connects to data layer

		public CalendarModelList(SessionAddressBlock SAB) : base(SAB)
		{
			_SAB = SAB;
			_updateApproved = false;
			InitializeComponent();
			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendarDefine);
			Common_Load ();

			BuildContextMenu();

			_cd = new CalendarData();
			_dtCalendarModel = _cd.CalendarModel_Read();

			//make Store RID column the primary key
			DataColumn[] PrimaryKeyColumn = new DataColumn[1];
			PrimaryKeyColumn[0] = _dtCalendarModel.Columns["CM_RID"];
			_dtCalendarModel.PrimaryKey = PrimaryKeyColumn;


			ugridCalendarModelList.DataSource = _dtCalendarModel;
		}
		private void Common_Load ()
		{
			try
			{
				SetText();
				
				if (FunctionSecurity.AllowUpdate)
				{

					Format_Title(eDataState.Updatable, eMIDTextCode.frm_CalendarModelList, null);
				}
				else
				{

					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_CalendarModelList, null);
				}

				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

                // Begin Track #5901 - JSmith - cannot select view when view authority
                if (FunctionSecurity.AllowView)
                {
                    btnPreview.Enabled = true;
                }
                // End Track #5901

			}
			catch ( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void BuildContextMenu()
		{
			// Define the MenuItem objects to display for the TextBox.
			menuItemEdit = new MenuItem("&Edit Calendar Model");
			menuItemNew = new MenuItem("&New Calendar Model");
			menuItemDelete = new MenuItem("&Delete Calendar Model");

			// Clear all previously added MenuItems.
			contextMenu1.MenuItems.Clear();
	 
			// Add MenuItems to display for the TextBox.
			if (FunctionSecurity.AllowUpdate)
			{
				contextMenu1.MenuItems.Add(menuItemEdit);
				contextMenu1.MenuItems.Add(menuItemNew);
			}
			if (FunctionSecurity.AllowDelete)
			{
				contextMenu1.MenuItems.Add(menuItemDelete);
			}

			menuItemEdit.Click += new System.EventHandler(this.menuItemEdit_Click);
			menuItemNew.Click += new System.EventHandler(this.menuItemNew_Click);
			menuItemDelete.Click += new System.EventHandler(this.menuItemDelete_Click);
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

				this.ugridCalendarModelList.Click -= new System.EventHandler(this.ugridCalendarModelList_Click);
				this.ugridCalendarModelList.DoubleClick -= new System.EventHandler(this.ugridCalendarModelList_DoubleClick);
				this.ugridCalendarModelList.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.ugridCalendarModelList_MouseDown);
				this.ugridCalendarModelList.AfterSelectChange -= new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugridCalendarModelList_AfterSelectChange);
				this.ugridCalendarModelList.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.ugridCalendarModelList_MouseUp);
				this.ugridCalendarModelList.AfterRowsDeleted -= new System.EventHandler(this.ugridCalendarModelList_AfterRowsDeleted);
				this.ugridCalendarModelList.AfterRowInsert -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugridCalendarModelList_AfterRowInsert);
				this.ugridCalendarModelList.BeforeCellUpdate -= new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugridCalendarModelList_BeforeCellUpdate);
				this.ugridCalendarModelList.BeforeRowUpdate -= new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugridCalendarModelList_BeforeRowUpdate);
				this.ugridCalendarModelList.BeforeRowsDeleted -= new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugridCalendarModelList_BeforeRowsDeleted);
				this.ugridCalendarModelList.BeforeRowInsert -= new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugridCalendarModelList_BeforeRowInsert);
				this.ugridCalendarModelList.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugridCalendarModelList_InitializeLayout);
				this.ugridCalendarModelList.BeforeEnterEditMode -= new System.ComponentModel.CancelEventHandler(this.ugridCalendarModelList_BeforeEnterEditMode);
				this.contextMenu1.Popup -= new System.EventHandler(this.contextMenu1_Popup);
				this.btnPreview.Click -= new System.EventHandler(this.btnPreview_Click);
				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.ugridCalendarModelList = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugridCalendarModelList)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ugridCalendarModelList
            // 
            this.ugridCalendarModelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ugridCalendarModelList.ContextMenu = this.contextMenu1;
            this.ugridCalendarModelList.DisplayLayout.AddNewBox.Hidden = false;
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ugridCalendarModelList.DisplayLayout.Appearance = appearance1;
            this.ugridCalendarModelList.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ugridCalendarModelList.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ugridCalendarModelList.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugridCalendarModelList.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ugridCalendarModelList.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ugridCalendarModelList.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ugridCalendarModelList.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ugridCalendarModelList.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ugridCalendarModelList.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ugridCalendarModelList.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ugridCalendarModelList.Location = new System.Drawing.Point(2, 2);
            this.ugridCalendarModelList.Name = "ugridCalendarModelList";
            this.ugridCalendarModelList.Size = new System.Drawing.Size(292, 185);
            this.ugridCalendarModelList.TabIndex = 0;
            this.ugridCalendarModelList.AfterRowsDeleted += new System.EventHandler(this.ugridCalendarModelList_AfterRowsDeleted);
            this.ugridCalendarModelList.BeforeRowInsert += new Infragistics.Win.UltraWinGrid.BeforeRowInsertEventHandler(this.ugridCalendarModelList_BeforeRowInsert);
            this.ugridCalendarModelList.BeforeCellUpdate += new Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventHandler(this.ugridCalendarModelList_BeforeCellUpdate);
            this.ugridCalendarModelList.BeforeRowUpdate += new Infragistics.Win.UltraWinGrid.CancelableRowEventHandler(this.ugridCalendarModelList_BeforeRowUpdate);
            this.ugridCalendarModelList.BeforeEnterEditMode += new System.ComponentModel.CancelEventHandler(this.ugridCalendarModelList_BeforeEnterEditMode);
            this.ugridCalendarModelList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ugridCalendarModelList_MouseDown);
            this.ugridCalendarModelList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ugridCalendarModelList_MouseUp);
            this.ugridCalendarModelList.Click += new System.EventHandler(this.ugridCalendarModelList_Click);
            this.ugridCalendarModelList.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ugridCalendarModelList_InitializeLayout);
            this.ugridCalendarModelList.AfterSelectChange += new Infragistics.Win.UltraWinGrid.AfterSelectChangeEventHandler(this.ugridCalendarModelList_AfterSelectChange);
            this.ugridCalendarModelList.BeforeRowsDeleted += new Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventHandler(this.ugridCalendarModelList_BeforeRowsDeleted);
            this.ugridCalendarModelList.AfterRowInsert += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ugridCalendarModelList_AfterRowInsert);
            this.ugridCalendarModelList.DoubleClick += new System.EventHandler(this.ugridCalendarModelList_DoubleClick);
            // 
            // contextMenu1
            // 
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(129, 198);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 3;
            this.btnPreview.Text = "&View";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(211, 198);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CalendarModelList
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(295, 226);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.ugridCalendarModelList);
            this.MaximizeBox = false;
            this.Name = "CalendarModelList";
            this.Text = "Calendar Model List";
            this.Controls.SetChildIndex(this.ugridCalendarModelList, 0);
            this.Controls.SetChildIndex(this.btnPreview, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ugridCalendarModelList)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		private void SetText()
		{
			try
			{
				this.btnPreview.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Preview);
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void btnClose_Click(object sender, System.EventArgs e)
		{
			IClose();
		}

		private void btnPreview_Click(object sender, System.EventArgs e)
		{
			CalendarDisplay calendarDisplayForm;
			calendarDisplayForm = new CalendarDisplay(_SAB);
			calendarDisplayForm.ShowDialog();
			calendarDisplayForm.Dispose();
		}

		private void ugridCalendarModelList_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
            //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
            //MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults();
            //ugld.ApplyDefaults(e);
            MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
            // Begin TT#1164 - JSmith - When going in to some methods or Admin features the columns are appearing too wide.
            //ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, true);
            ugld.ApplyDefaults((Infragistics.Win.UltraWinGrid.UltraGrid)sender, e, true, 2, false);
            // End TT#1164
            //End TT#169

			//this.ugridCalendarModelList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False; 
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Layout.AddNewBox.Hidden = false;
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["CM_ID"].Header.Caption = "Model Name"; 
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["CM_ID"].Width = 145;
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["FISCAL_YEAR"].Header.Caption = "Fiscal\nYear";
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["FISCAL_YEAR"].Width = 50;
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["FISCAL_YEAR"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
            this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["FISCAL_YEAR"].Format = "######0"; ; // TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["START_DATE"].Header.Caption = "Start Date";
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["START_DATE"].Width = 75;
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["START_DATE"].Format = "d";
            this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["START_DATE"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date; // TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["START_DATE"].CellAppearance.TextHAlign =
				Infragistics.Win.HAlign.Center;
			this.ugridCalendarModelList.DisplayLayout.Bands[0].ColHeaderLines = 2;
			//this.ugridPeriods.Rows[0].Appearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True; 
			this.ugridCalendarModelList.DisplayLayout.Override.HeaderAppearance.FontData.Bold = 
				Infragistics.Win.DefaultableBoolean.True;

			this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["CM_RID"].Hidden = true;
			//this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["WEEK53_OFFSET_BASE_YEAR"].Hidden = true;
			//this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["WEEK53_OFFSET_PERIOD"].Hidden = true;
			//this.ugridCalendarModelList.DisplayLayout.Bands[0].Columns["WEEK53_OFFSET_YEAR_INTERVAL"].Hidden = true;

			this.ugridCalendarModelList.DisplayLayout.Bands[0].AddButtonCaption = "Calendar Model";
		}

		private void RefreshCalendar()
		{
			Cursor.Current = Cursors.WaitCursor;

			_SAB.ClientServerSession.Calendar.Realign53WeekSelections();

			DateTime refreshTime = DateTime.Now;

			if (_SAB.ApplicationServerSession != null)
				_SAB.ApplicationServerSession.RefreshCalendar(refreshTime);
			if (_SAB.ClientServerSession != null)
				_SAB.ClientServerSession.RefreshCalendar(refreshTime);
			//			if (_SAB.ControlServerSession != null)
			//				_SAB.ControlServerSession.RefreshCalendar(refreshTime);
			if (_SAB.HierarchyServerSession != null)
				_SAB.HierarchyServerSession.RefreshCalendar(refreshTime);
			if (_SAB.StoreServerSession != null)
				_SAB.StoreServerSession.RefreshCalendar(refreshTime);

			Cursor.Current = Cursors.Default;
		}

		private void ugridCalendarModelList_BeforeRowInsert(object sender, Infragistics.Win.UltraWinGrid.BeforeRowInsertEventArgs e)
		{
			CalendarModelMaint calendarModelForm;
			DialogResult calendarModelResult;
			
			calendarModelForm = new CalendarModelMaint(_SAB, 0);
			
			calendarModelResult = calendarModelForm.ShowDialog();
			if (calendarModelResult == DialogResult.Cancel)
			{
				e.Cancel = true;
			}

			_newModelRID = calendarModelForm.CalendarModelRID;
			_newModelName = calendarModelForm.CalendarModelName;
			_newModelFiscalYear = calendarModelForm.FiscalYear;
			_newModelStartDate = calendarModelForm.StartDate;

			calendarModelForm.Dispose();
		}

		private void ugridCalendarModelList_AfterRowInsert(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			_updateApproved = true;
			e.Row.Cells["CM_RID"].Value = _newModelRID;
			e.Row.Cells["CM_ID"].Value = _newModelName;
			e.Row.Cells["FISCAL_YEAR"].Value = _newModelFiscalYear;
			e.Row.Cells["START_DATE"].Value = _newModelStartDate;
			_updateApproved = false;
		}

		private void ugridCalendarModelList_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
		{
			if(!_updateApproved)
			{
				e.Cancel = true;
			}
		}

		private void contextMenu1_Popup(object sender, System.EventArgs e)
		{
			// see defines and definition up under InitializeComponent
		}

		private void menuItemEdit_Click(object sender, System.EventArgs e)
		{
			try
			{
				int selectedRowCount = ugridCalendarModelList.Selected.Rows.Count;
				SelectedRowsCollection sRow = ugridCalendarModelList.Selected.Rows;
			
				if (selectedRowCount == 1)
				{


					//******************************************************************************
					// Is there a calendar maint dialog form already opened?
					// WHY ARE WE CHECKING FOR THIS ON A DIALOG WINDOW?
					// HERE's WHY...
					// There fixes a quirk that can occasionaly happen where if you left click 
					// on the calendar model line and then quickly right click to get the 
					// menu list, and then left-click again to select 'edit', the program interprets this as a double click.   
					// This causes the double-click event to fire which open the edit dialog window.  Then this 
					// event fires an another edit dialog window is opened.
					//******************************************************************************	
					if (_calendarModelForm == null)
					{
						_calendarModelForm = new CalendarModelMaint(_SAB, Convert.ToInt32(sRow[0].Cells["CM_RID"].Value, CultureInfo.CurrentUICulture));
						DialogResult calendarModelResult = _calendarModelForm.ShowDialog();
						if (calendarModelResult == DialogResult.OK)
						{
							_updateApproved = true;
							sRow[0].Cells["CM_ID"].Value = _calendarModelForm.CalendarModelName;
							sRow[0].Cells["START_DATE"].Value = _calendarModelForm.StartDate;
							sRow[0].Cells["FISCAL_YEAR"].Value = _calendarModelForm.FiscalYear;
							_updateApproved = false;
						}
						_calendarModelForm.Dispose();
						_calendarModelForm = null;
					}
				}
				else if (selectedRowCount > 1)
				{
					MessageBox.Show("Multiple Calendar Models Selected.  Only one" +
						" can be edited at a time.  Change your selection to include" +
						" only one Calendar Model.");
				}
				else if (selectedRowCount == 0)
				{
					MessageBox.Show("No Calendar Model Selected.");
				}

			}
			catch ( Exception err )
			{
				MessageBox.Show(this, err.Message );
			}
			
		}
		private void menuItemNew_Click(object sender, System.EventArgs e)
		{
			ugridCalendarModelList.DisplayLayout.Bands[0].AddNew();
		}
		private void menuItemDelete_Click(object sender, System.EventArgs e)
		{
			ugridCalendarModelList.DeleteSelectedRows();
		}

		private void ugridCalendarModelList_DoubleClick(object sender, System.EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			int selectedRowCount = ugridCalendarModelList.Selected.Rows.Count;
			SelectedRowsCollection sRow = ugridCalendarModelList.Selected.Rows;
			
			if (selectedRowCount == 1)
			{
				_calendarModelForm = new CalendarModelMaint(_SAB, Convert.ToInt32(sRow[0].Cells["CM_RID"].Value, CultureInfo.CurrentUICulture));
				DialogResult calendarModelResult = _calendarModelForm.ShowDialog();
				if (calendarModelResult == DialogResult.OK)
				{
					_updateApproved = true;
					sRow[0].Cells["CM_ID"].Value = _calendarModelForm.CalendarModelName;
					sRow[0].Cells["START_DATE"].Value = _calendarModelForm.StartDate;
					sRow[0].Cells["FISCAL_YEAR"].Value = _calendarModelForm.FiscalYear;
					_updateApproved = false;
				}
				_calendarModelForm.Dispose();
				_calendarModelForm = null;
			}
			else if (selectedRowCount > 1)
			{
				MessageBox.Show("Multiple Calendar Models Selected.  Only one" +
					" can be edited at a time.  Change your selection to include" +
					" only one Calendar Model.");
			}
			else if (selectedRowCount == 0)
			{
				MessageBox.Show("No Calendar Model Selected.");
			}

			Cursor.Current = Cursors.WaitCursor;

		}

		private void ugridCalendarModelList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			ugridCalendarModelList.Selected.Rows.Clear();
			_prevMouseDownTime = _mouseDownTime;
			_mouseDownTime = DateTime.Now;

			Infragistics.Win.UIElement aUIElement;
			aUIElement = ugridCalendarModelList.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

			UltraGridRow aRow;
			aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow) );
			if (aRow != null)
			{
				aRow.Selected = true;
				this.ugridCalendarModelList.ActiveRow = aRow;
			}

			//			Infragistics.Win.UIElement aUIElement;
			//			aUIElement = ugridCalendarModelList.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));
			//
			//			UltraGridRow aRow;
			//			aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow) );
			//			if (aRow != null)
			//			{
			//				aRow.Selected = true;
			//				this.ugridCalendarModelList.ActiveRow = aRow;
			//				//aRow.Activate();
			//				//this.ugridCalendarModelList.DisplayLayout.Override.ActiveRowAppearance.BackColor = Color.Red;
			//			}
		}

		private void ugridCalendarModelList_Click(object sender, System.EventArgs e)
		{
			// This is here to catch a double-click on a cell
			if(_mouseDownTime < _prevMouseDownTime.AddMilliseconds(SystemInformation.DoubleClickTime))
			{
				ugridCalendarModelList_DoubleClick(sender, e);
			}
		}

		private void ugridCalendarModelList_AfterSelectChange(object sender, Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs e)
		{
		}

		private void ugridCalendarModelList_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Infragistics.Win.UIElement aUIElement;
			aUIElement = ugridCalendarModelList.DisplayLayout.UIElement.ElementFromPoint(new Point(e.X, e.Y));

			UltraGridRow aRow;
			aRow = (UltraGridRow)aUIElement.GetContext(typeof(UltraGridRow) );
			if (aRow != null)
			{
				aRow.Selected = true;
				this.ugridCalendarModelList.ActiveRow = aRow;
			}
		}

		private void ugridCalendarModelList_BeforeRowsDeleted(object sender, Infragistics.Win.UltraWinGrid.BeforeRowsDeletedEventArgs e)
		{
			int RowCount = e.Rows.Length;
			// check to see if all calendar models are being deleted.
			if (RowCount == ugridCalendarModelList.Rows.Count)
			{
				MessageBox.Show("You are attempting to delete all remaining Calendar Models." +
					" The System needs at least one model to function properly. " +
					" Try removing all but one model and then edit the remaining model to meet your needs.", 
					"Invalid Delete of Calendar Model.");

				e.Cancel = true;
			}
			else
			{
				if (_deletedModelsList == null)
					_deletedModelsList = new ArrayList();
				else
					_deletedModelsList.Clear();

				for (int i=0;i<RowCount;i++)
				{
					UltraGridRow row = e.Rows[i];
					int modelRid = Convert.ToInt32(row.Cells["CM_RID"].Value, CultureInfo.CurrentUICulture);
					_deletedModelsList.Add(modelRid);
				}
			}
		}

		private void ugridCalendarModelList_AfterRowsDeleted(object sender, System.EventArgs e)
		{
			CalendarModels cModels = new CalendarModels();
			foreach (int modelRid in _deletedModelsList)
			{
				CalendarModel currModel = cModels.GetCalendarModel(modelRid);
				CalendarModel prevModel = cModels.GetPriorCalendarModel(modelRid);
				//=====================================================
				// Move 53week definitions to prior model, if possible
				//=====================================================
				if (prevModel != null)
				{
					cModels.Move53WeekToPriorModel(modelRid);
					// Update prior model's ending year
					prevModel.LastModelYear = currModel.LastModelYear;
				}

                //Begin TT#1283-MD -jsobek -Object Reference Error trying to delete newly created Calendar Model
                try
                {
                    _cd.OpenUpdateConnection();

                    // delete current model's periods
                    _cd.CalendarModelPeriods_Delete(modelRid);
                    // delete current model
                    _cd.CalendarModel_Delete(modelRid);

                    _cd.CommitData();
                }
                finally
                {
                    _cd.CloseUpdateConnection();
                }
                //End TT#1283-MD -jsobek -Object Reference Error trying to delete newly created Calendar Model
			}

			RefreshCalendar();
		}

		private void ugridCalendarModelList_BeforeRowUpdate(object sender, Infragistics.Win.UltraWinGrid.CancelableRowEventArgs e)
		{
			
		}

		private void ugridCalendarModelList_BeforeEnterEditMode(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
		}


		#region IFormBase Members
		override public void ICut()
		{
			
		}

		override public void ICopy()
		{
			
		}

		override public void IPaste()
		{
			
		}	

		//		override public void IClose()
		//		{
		//			try
		//			{
		//				this.Close();
		//
		//			}		
		//			catch(Exception ex)
		//			{
		//				MessageBox.Show(ex.Message);
		//			}
		//			
		//		}

		override public void ISave()
		{
			
		}

		override public void ISaveAs()
		{
			
		}

		override public void IDelete()
		{
			
		}

		override public void IRefresh()
		{
			
		}
		
		#endregion

	}
}
