using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using System.Data;
using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{

	/// <summary>
	/// Summary description for CalendarWeek53Selector.
	/// </summary>
	public class CalendarWeek53Selector : MIDFormBase
	{
		private SessionAddressBlock _SAB;
		private MRSCalendar	_cal;
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		private DataTable _dtWeek53;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnSave;
		private int _modelRID;

		UltraGridColumn _visibleColumn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public DataTable DtWeek53 
		{
			get { return _dtWeek53 ; }
		}


		public CalendarWeek53Selector(SessionAddressBlock SAB, int modelRID) : base(SAB)
		{
			InitializeComponent();

			_SAB = SAB;
			_cal = SAB.ClientServerSession.Calendar;
			_modelRID = modelRID;

			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendarDefine);
			Common_Load ();

			_cal.Week53_Populate(_modelRID);

			// setup date view
			_dtWeek53 = _cal.CalendarWeek53DataTable;
	
			ultraGrid1.DataSource = _dtWeek53;
			
			btnCancel.DialogResult = DialogResult.Cancel;
			btnSave.DialogResult = DialogResult.OK;

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
				this.ultraGrid1.BeforeColRegionScroll -= new Infragistics.Win.UltraWinGrid.BeforeColRegionScrollEventHandler(this.ultraGrid1_BeforeColRegionScroll);
				this.ultraGrid1.InitializeRow -= new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
				this.ultraGrid1.BeforeCellListDropDown -= new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ultraGrid1_BeforeCellListDropDown);
				this.ultraGrid1.AfterCellListCloseUp -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellListCloseUp);
				this.ultraGrid1.AfterCellUpdate -= new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellUpdate);
				this.ultraGrid1.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ultraGrid1);
                //End TT#169
				this.btnCancel.Click -= new System.EventHandler(this.btnCancel_Click);
				this.btnSave.Click -= new System.EventHandler(this.btnSave_Click);
				this.Load -= new System.EventHandler(this.CalendarWeek53Selector_Load);
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
            this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // ultraGrid1
            // 
            this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance1.BackGradientStyle = Infragistics.Win.GradientStyle.ForwardDiagonal;
            this.ultraGrid1.DisplayLayout.Appearance = appearance1;
            this.ultraGrid1.DisplayLayout.InterBandSpacing = 10;
            appearance2.BackColor = System.Drawing.Color.Transparent;
            this.ultraGrid1.DisplayLayout.Override.CardAreaAppearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Left";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.ultraGrid1.DisplayLayout.Override.HeaderAppearance = appearance3;
            appearance4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ultraGrid1.DisplayLayout.Override.RowAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorAppearance = appearance5;
            this.ultraGrid1.DisplayLayout.Override.RowSelectorWidth = 12;
            this.ultraGrid1.DisplayLayout.Override.RowSpacingBefore = 2;
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            appearance6.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(247)))), ((int)(((byte)(249)))));
            appearance6.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance6.ForeColor = System.Drawing.Color.Black;
            this.ultraGrid1.DisplayLayout.Override.SelectedRowAppearance = appearance6;
            this.ultraGrid1.DisplayLayout.RowConnectorColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(167)))), ((int)(((byte)(191)))));
            this.ultraGrid1.DisplayLayout.RowConnectorStyle = Infragistics.Win.UltraWinGrid.RowConnectorStyle.Solid;
            this.ultraGrid1.Location = new System.Drawing.Point(8, 16);
            this.ultraGrid1.Name = "ultraGrid1";
            this.ultraGrid1.Size = new System.Drawing.Size(576, 56);
            this.ultraGrid1.TabIndex = 0;
            this.ultraGrid1.BeforeCellListDropDown += new Infragistics.Win.UltraWinGrid.CancelableCellEventHandler(this.ultraGrid1_BeforeCellListDropDown);
            this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
            this.ultraGrid1.BeforeColRegionScroll += new Infragistics.Win.UltraWinGrid.BeforeColRegionScrollEventHandler(this.ultraGrid1_BeforeColRegionScroll);
            this.ultraGrid1.AfterCellListCloseUp += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellListCloseUp);
            this.ultraGrid1.AfterCellUpdate += new Infragistics.Win.UltraWinGrid.CellEventHandler(this.ultraGrid1_AfterCellUpdate);
            this.ultraGrid1.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.ultraGrid1_InitializeRow);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(512, 80);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(424, 80);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Ok";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // CalendarWeek53Selector
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 110);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.ultraGrid1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalendarWeek53Selector";
            this.Text = "53rd Week Selector";
            this.Load += new System.EventHandler(this.CalendarWeek53Selector_Load);
            this.Controls.SetChildIndex(this.ultraGrid1, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void Common_Load ()
		{
			try
			{
				SetText();
				
				if (FunctionSecurity.AllowUpdate)
				{

					Format_Title(eDataState.Updatable, eMIDTextCode.frm_Calendar53Week, null);
				}
				else
				{

					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_Calendar53Week, null);
				}

				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

			}
			catch ( Exception exception )
			{
				HandleException(exception);
			}
		}
		private void LoadPeriodDropDowns()
		{
	
			string modelName = null;
			ArrayList calModels = _cal.GetCalendarModels();
			foreach (CalendarModel cm in calModels)
			{
				modelName = cm.RID.ToString(CultureInfo.CurrentUICulture);
				ultraGrid1.DisplayLayout.ValueLists.Add(modelName);
				ultraGrid1.DisplayLayout.ValueLists[modelName].ValueListItems.Clear();

				ultraGrid1.DisplayLayout.ValueLists[modelName].ValueListItems.Add(0, "(none)");
				foreach(CalendarModelPeriod mp in cm.Months)
				{
					ultraGrid1.DisplayLayout.ValueLists[modelName].ValueListItems.Add(mp.Sequence, mp.Name);		
				}

			}

			//ultraGrid1.DisplayLayout.Bands[0].Columns["CMP_SEQUENCE"].ValueList = ultraGrid1.DisplayLayout.ValueLists[modelName];
		}

		private void ultraGrid1_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
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

			ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;  //hide row selector
			ultraGrid1.Rows[0].Hidden = true;	// hide row with Model RID

			LoadPeriodDropDowns();

			foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
			{
				// all this code is to give each year the right dropdown of periods
				cColumn.Width = 100;
				
				// get row[1] value which is the period
				UltraGridRow PeriodRow = ultraGrid1.Rows[1];
				UltraGridCell PeriodCell = PeriodRow.Cells[cColumn];

				if ((Convert.ToInt32(PeriodCell.Value, CultureInfo.CurrentUICulture)) > 0)
					PeriodCell.Appearance.BackColor  = System.Drawing.Color.LightCyan; 
				else 
					PeriodCell.Appearance.BackColor  = System.Drawing.Color.White;
 
				//PeriodCell.Activation = Activation.ActivateOnly;

				// Use Model ID to get the right drop down
				// model ID is in row[0] (hidden row)
				UltraGridRow ModelRIDRow = ultraGrid1.Rows[0];

				UltraGridCell ModelCell = ModelRIDRow.Cells[cColumn];
				String ModelRID = ModelCell.Value.ToString();

				cColumn.Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownValidate;
				cColumn.ValueList = ultraGrid1.DisplayLayout.ValueLists[ModelRID];
				cColumn.CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

			}

			
		}

		private void ultraGrid1_BeforeCellListDropDown(object sender, Infragistics.Win.UltraWinGrid.CancelableCellEventArgs e)
		{
		}

		private void ultraGrid1_InitializeRow(object sender, Infragistics.Win.UltraWinGrid.InitializeRowEventArgs e)
		{
			if (e.ReInitialize)
			{
				foreach (UltraGridColumn cColumn in this.ultraGrid1.DisplayLayout.Bands[0].Columns)
				{
					if (Convert.ToInt32(e.Row.Cells[cColumn].Value, CultureInfo.CurrentUICulture) > 0)
						e.Row.Cells[cColumn].Appearance.BackColor = System.Drawing.Color.LightCyan; 
					else 
						e.Row.Cells[cColumn].Appearance.BackColor = System.Drawing.Color.White; 
				}
			}
		}

		private void ultraGrid1_AfterCellUpdate(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if ((Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture)) > 0)
				e.Cell.Appearance.BackColor  = System.Drawing.Color.LightCyan; 
			else 
				e.Cell.Appearance.BackColor  = System.Drawing.Color.White; 
		}

		private void ultraGrid1_AfterCellListCloseUp(object sender, Infragistics.Win.UltraWinGrid.CellEventArgs e)
		{
			if ((Convert.ToInt32(e.Cell.Value, CultureInfo.CurrentUICulture)) > 0)
				e.Cell.Appearance.BackColor  = System.Drawing.Color.LightCyan; 
			else 
				e.Cell.Appearance.BackColor  = System.Drawing.Color.White; 
		}

		private void ultraGrid1_BeforeColRegionScroll(object sender, Infragistics.Win.UltraWinGrid.BeforeColRegionScrollEventArgs e)
		{
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			_dtWeek53.RejectChanges();
			Close();
		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{	
			ISave();
		}

		override protected bool SaveChanges()
		{
			//_cal.Week53_Delete(_modelRID);

			// remember that...
			// year is in the column heading
			// calendar model RID (cm_rid) is in row (0)
			// selected period sequence is in row (1)

			DataRow drMoldelRID = _dtWeek53.Rows[0];
			DataRow drPeriodSeq = _dtWeek53.Rows[1];
			//DataRow drOffset = _dtWeek53.Rows[2];

			for (int col=0;col<drPeriodSeq.ItemArray.Length; col++)
			{
				int periodSeq = Convert.ToInt32( drPeriodSeq[col], CultureInfo.CurrentUICulture );
				int cm_rid = 0;	// model RID
				int fiscalYear = 0;
//				DataCommon.eWeek53Offset offset;
				if (periodSeq > 0)
				{
					cm_rid = Convert.ToInt32( drMoldelRID[col], CultureInfo.CurrentUICulture );
					fiscalYear = Convert.ToInt32( _dtWeek53.Columns[col].Caption.ToString(CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture );
//					offset = eWeek53Offset.Offset1Week;
					//_cal.Week53_Insert(fiscalYear, cm_rid, periodSeq, offset);
				}
			}

			_dtWeek53.AcceptChanges();

			//_cal.Week53_Refresh();
			return true;
		}

		private void CalendarWeek53Selector_Load(object sender, System.EventArgs e)
		{
			// if possible scroll to the current year
			int firstYear = Convert.ToInt32( _dtWeek53.Columns[0].Caption.ToString(CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture );
			int count = _dtWeek53.Columns.Count;
			int lastYear = Convert.ToInt32( _dtWeek53.Columns[count - 1].Caption.ToString(CultureInfo.CurrentUICulture), CultureInfo.CurrentUICulture );
			DateProfile postDate = _cal.PostDate;
			if (postDate.FiscalYear >= firstYear && postDate.FiscalYear <= lastYear)
			{
				_visibleColumn = this.ultraGrid1.DisplayLayout.Bands[0].Columns[postDate.FiscalYear.ToString(CultureInfo.CurrentUICulture)];
				this.ultraGrid1.ActiveColScrollRegion.ScrollColIntoView(_visibleColumn, true);
			}
		}
		private void SetText()
		{
			try
			{
				this.btnSave.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_OK);
				this.btnCancel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Cancel);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
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
			try
			{
				SaveChanges();
			}		
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
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
