using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;
using Infragistics.Win.UltraWinGrid;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for CalendarDisplay.
	/// </summary>
	public class CalendarDisplay : MIDFormBase
	{
		private Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid1;
		private System.Windows.Forms.Button btnLeft;
		private System.Windows.Forms.Button btnRight;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label lblYear;
		private System.Windows.Forms.TextBox txtFiscalYear;
		private DataView _dvCalendar;
		private MRSCalendar _calendar;
		private int _year;
		private System.Windows.Forms.CheckBox cbExpandAll;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblNumWeeks;
		private System.Windows.Forms.Label lblNumMonths;
		private System.Windows.Forms.Label lblFirstDay;
		private bool [,] _expanded;
		private System.Windows.Forms.ToolTip toolTip;
		private Button btnRightTop;
		private Button btnLeftTop;
		private Label label4;
		private Label label6;
		private Label lblNumQuarters;
		private Label lblNumSeasons;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="_SAB">SessionAddressBlock</param>
		public CalendarDisplay(SessionAddressBlock _SAB) : base(_SAB)
		{
			InitializeComponent();
			FunctionSecurity = _SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AdminCalendar);
			Common_Load ();

			_calendar = _SAB.ClientServerSession.Calendar;

			_year = _calendar.PostDate.FiscalYear;

			_dvCalendar = new DataView(_calendar.CalendarDisplayDataSet.Tables["Seasons"]);
			ultraGrid1.DataSource = _dvCalendar;
			SetDisplayYear();
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

				this.ultraGrid1.AfterRowExpanded -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowExpanded);
				this.ultraGrid1.InitializeLayout -= new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
                //Begin TT#169 - JSmith - enter max of 1000 and receive 'must be integer'
                MIDRetail.Windows.Controls.UltraGridLayoutDefaults ugld = new MIDRetail.Windows.Controls.UltraGridLayoutDefaults(ErrorImage);
                ugld.DetachGridEventHandlers(ultraGrid1);
                //End TT#169
				this.ultraGrid1.AfterRowCollapsed -= new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowCollapsed);
				this.btnLeft.Click -= new System.EventHandler(this.btnLeft_Click);
				this.btnRight.Click -= new System.EventHandler(this.btnRight_Click);
				this.btnClose.Click -= new System.EventHandler(this.btnClose_Click);
				this.txtFiscalYear.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.txtFiscalYear_KeyPress);
				this.txtFiscalYear.Leave -= new System.EventHandler(this.txtFiscalYear_Leave);
				this.cbExpandAll.CheckedChanged -= new System.EventHandler(this.cbExpandAll_CheckedChanged);
			}
			base.Dispose( disposing );
		}
		private void Common_Load ()
		{
			try
			{
				SetText();
				
				if (FunctionSecurity.AllowUpdate)
				{

					Format_Title(eDataState.Updatable, eMIDTextCode.frm_CalendarDisplay, null);
				}
				else
				{

					Format_Title(eDataState.ReadOnly, eMIDTextCode.frm_CalendarDisplay, null);
				}

				SetReadOnly(FunctionSecurity.AllowUpdate);  //Security changes - 1/24/2005 vg

                // Begin TT#2708 - JSmith - Calendar Security in Read-Only
                btnLeftTop.Enabled = true;
                btnLeft.Enabled = true;
                btnRightTop.Enabled = true;
                btnRight.Enabled = true;
                cbExpandAll.Enabled = true;
                // End TT#2708 - JSmith - Calendar Security in Read-Only

			}
			catch ( Exception exception )
			{
				HandleException(exception);
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
			Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
			this.ultraGrid1 = new Infragistics.Win.UltraWinGrid.UltraGrid();
			this.btnLeft = new System.Windows.Forms.Button();
			this.btnRight = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.lblYear = new System.Windows.Forms.Label();
			this.txtFiscalYear = new System.Windows.Forms.TextBox();
			this.cbExpandAll = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lblNumWeeks = new System.Windows.Forms.Label();
			this.lblNumMonths = new System.Windows.Forms.Label();
			this.lblFirstDay = new System.Windows.Forms.Label();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnRightTop = new System.Windows.Forms.Button();
			this.btnLeftTop = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lblNumQuarters = new System.Windows.Forms.Label();
			this.lblNumSeasons = new System.Windows.Forms.Label();
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
			this.ultraGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
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
			this.ultraGrid1.Location = new System.Drawing.Point(8, 43);
			this.ultraGrid1.Name = "ultraGrid1";
			this.ultraGrid1.Size = new System.Drawing.Size(275, 336);
			this.ultraGrid1.TabIndex = 0;
			this.ultraGrid1.AfterRowExpanded += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowExpanded);
			this.ultraGrid1.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.ultraGrid1_InitializeLayout);
			this.ultraGrid1.AfterRowCollapsed += new Infragistics.Win.UltraWinGrid.RowEventHandler(this.ultraGrid1_AfterRowCollapsed);
			this.ultraGrid1.MouseEnterElement += new Infragistics.Win.UIElementEventHandler(this.ultraGrid1_MouseEnterElement);
			// 
			// btnLeft
			// 
			this.btnLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnLeft.Location = new System.Drawing.Point(8, 387);
			this.btnLeft.Name = "btnLeft";
			this.btnLeft.Size = new System.Drawing.Size(32, 23);
			this.btnLeft.TabIndex = 1;
			this.btnLeft.Text = "<<";
			this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
			// 
			// btnRight
			// 
			this.btnRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRight.Location = new System.Drawing.Point(251, 387);
			this.btnRight.Name = "btnRight";
			this.btnRight.Size = new System.Drawing.Size(32, 23);
			this.btnRight.TabIndex = 2;
			this.btnRight.Text = ">>";
			this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.Location = new System.Drawing.Point(208, 459);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "&Close";
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lblYear
			// 
			this.lblYear.Location = new System.Drawing.Point(85, 15);
			this.lblYear.Name = "lblYear";
			this.lblYear.Size = new System.Drawing.Size(64, 16);
			this.lblYear.TabIndex = 4;
			this.lblYear.Text = "Fiscal Year:";
			// 
			// txtFiscalYear
			// 
			this.txtFiscalYear.Location = new System.Drawing.Point(152, 11);
			this.txtFiscalYear.Name = "txtFiscalYear";
			this.txtFiscalYear.Size = new System.Drawing.Size(48, 20);
			this.txtFiscalYear.TabIndex = 5;
			this.txtFiscalYear.Leave += new System.EventHandler(this.txtFiscalYear_Leave);
			this.txtFiscalYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFiscalYear_KeyPress);
			// 
			// cbExpandAll
			// 
			this.cbExpandAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbExpandAll.Location = new System.Drawing.Point(107, 385);
			this.cbExpandAll.Name = "cbExpandAll";
			this.cbExpandAll.Size = new System.Drawing.Size(77, 24);
			this.cbExpandAll.TabIndex = 6;
			this.cbExpandAll.Text = "Expand All";
			this.cbExpandAll.CheckedChanged += new System.EventHandler(this.cbExpandAll_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Location = new System.Drawing.Point(8, 421);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 7;
			this.label1.Text = "Number of Weeks:";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(148, 421);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Number of Months:";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(8, 461);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 9;
			this.label3.Text = "First Day of Week:";
			// 
			// lblNumWeeks
			// 
			this.lblNumWeeks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblNumWeeks.Location = new System.Drawing.Point(107, 421);
			this.lblNumWeeks.Name = "lblNumWeeks";
			this.lblNumWeeks.Size = new System.Drawing.Size(34, 16);
			this.lblNumWeeks.TabIndex = 10;
			// 
			// lblNumMonths
			// 
			this.lblNumMonths.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblNumMonths.Location = new System.Drawing.Point(246, 421);
			this.lblNumMonths.Name = "lblNumMonths";
			this.lblNumMonths.Size = new System.Drawing.Size(34, 16);
			this.lblNumMonths.TabIndex = 11;
			// 
			// lblFirstDay
			// 
			this.lblFirstDay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblFirstDay.Location = new System.Drawing.Point(105, 461);
			this.lblFirstDay.Name = "lblFirstDay";
			this.lblFirstDay.Size = new System.Drawing.Size(75, 16);
			this.lblFirstDay.TabIndex = 12;
			// 
			// btnRightTop
			// 
			this.btnRightTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRightTop.Location = new System.Drawing.Point(251, 10);
			this.btnRightTop.Name = "btnRightTop";
			this.btnRightTop.Size = new System.Drawing.Size(32, 23);
			this.btnRightTop.TabIndex = 13;
			this.btnRightTop.Text = ">>";
			this.btnRightTop.UseVisualStyleBackColor = true;
			this.btnRightTop.Click += new System.EventHandler(this.btnRight_Click);
			// 
			// btnLeftTop
			// 
			this.btnLeftTop.Location = new System.Drawing.Point(8, 10);
			this.btnLeftTop.Name = "btnLeftTop";
			this.btnLeftTop.Size = new System.Drawing.Size(32, 23);
			this.btnLeftTop.TabIndex = 14;
			this.btnLeftTop.Text = "<<";
			this.btnLeftTop.UseVisualStyleBackColor = true;
			this.btnLeftTop.Click += new System.EventHandler(this.btnLeft_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 441);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(102, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Number of Quarters:";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(148, 441);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(103, 13);
			this.label6.TabIndex = 17;
			this.label6.Text = "Number of Seasons:";
			// 
			// lblNumQuarters
			// 
			this.lblNumQuarters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblNumQuarters.Location = new System.Drawing.Point(107, 441);
			this.lblNumQuarters.Name = "lblNumQuarters";
			this.lblNumQuarters.Size = new System.Drawing.Size(34, 16);
			this.lblNumQuarters.TabIndex = 18;
			// 
			// lblNumSeasons
			// 
			this.lblNumSeasons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.lblNumSeasons.Location = new System.Drawing.Point(246, 441);
			this.lblNumSeasons.Name = "lblNumSeasons";
			this.lblNumSeasons.Size = new System.Drawing.Size(34, 16);
			this.lblNumSeasons.TabIndex = 19;
			// 
			// CalendarDisplay
			// 
			this.AllowDragDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(290, 486);
			this.Controls.Add(this.lblNumSeasons);
			this.Controls.Add(this.lblNumQuarters);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnLeftTop);
			this.Controls.Add(this.btnRightTop);
			this.Controls.Add(this.lblFirstDay);
			this.Controls.Add(this.lblNumMonths);
			this.Controls.Add(this.lblNumWeeks);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbExpandAll);
			this.Controls.Add(this.txtFiscalYear);
			this.Controls.Add(this.lblYear);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnRight);
			this.Controls.Add(this.btnLeft);
			this.Controls.Add(this.ultraGrid1);
			this.Name = "CalendarDisplay";
			this.Text = "CalendarDisplay";
			this.Controls.SetChildIndex(this.ultraGrid1, 0);
			this.Controls.SetChildIndex(this.btnLeft, 0);
			this.Controls.SetChildIndex(this.btnRight, 0);
			this.Controls.SetChildIndex(this.btnClose, 0);
			this.Controls.SetChildIndex(this.lblYear, 0);
			this.Controls.SetChildIndex(this.txtFiscalYear, 0);
			this.Controls.SetChildIndex(this.cbExpandAll, 0);
			this.Controls.SetChildIndex(this.label1, 0);
			this.Controls.SetChildIndex(this.label2, 0);
			this.Controls.SetChildIndex(this.label3, 0);
			this.Controls.SetChildIndex(this.lblNumWeeks, 0);
			this.Controls.SetChildIndex(this.lblNumMonths, 0);
			this.Controls.SetChildIndex(this.lblFirstDay, 0);
			this.Controls.SetChildIndex(this.btnRightTop, 0);
			this.Controls.SetChildIndex(this.btnLeftTop, 0);
			this.Controls.SetChildIndex(this.label4, 0);
			this.Controls.SetChildIndex(this.label6, 0);
			this.Controls.SetChildIndex(this.lblNumQuarters, 0);
			this.Controls.SetChildIndex(this.lblNumSeasons, 0);
			((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ultraGrid1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Close window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			Close();
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

			ultraGrid1.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False; 

			ultraGrid1.DisplayLayout.Bands[0].Columns["Year"].Hidden = true; 
			ultraGrid1.DisplayLayout.Bands[1].Columns["Year"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[1].Columns["Season"].Hidden = true; 
			ultraGrid1.DisplayLayout.Bands[2].Columns["Year"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[2].Columns["Quarter"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[3].Columns["Year"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[3].Columns["Month"].Hidden = true;
			ultraGrid1.DisplayLayout.Bands[3].Columns["JulianDate"].Hidden = true;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Season"].Width = 40;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].Width = 130;
			ultraGrid1.DisplayLayout.Bands[1].Columns["Quarter"].Width = 40;
			ultraGrid1.DisplayLayout.Bands[1].Columns["Name"].Width = 130;
			ultraGrid1.DisplayLayout.Bands[2].Columns["Month"].Width = 40;
			ultraGrid1.DisplayLayout.Bands[2].Columns["Name"].Width = 130;
			ultraGrid1.DisplayLayout.Bands[3].Columns["Week"].Width = 50;

			ultraGrid1.DisplayLayout.Bands[0].Columns["Season"].Header.Caption = "Season #";
			ultraGrid1.DisplayLayout.Bands[0].Columns["Name"].Header.Caption = "Season Name";
			ultraGrid1.DisplayLayout.Bands[1].Columns["Quarter"].Header.Caption = "Quarter #";
			ultraGrid1.DisplayLayout.Bands[1].Columns["Name"].Header.Caption = "Quarter Name";
			ultraGrid1.DisplayLayout.Bands[2].Columns["Month"].Header.Caption = "Month #";
			ultraGrid1.DisplayLayout.Bands[2].Columns["Name"].Header.Caption = "Month Name";
			ultraGrid1.DisplayLayout.Bands[3].Columns["Week"].Header.Caption = "Week #";
			ultraGrid1.DisplayLayout.Bands[3].Columns["Date"].Header.Caption = "First Day of Week";

			//ultraGrid1.DisplayLayout.Bands[0].Columns["Period"].CellAppearance.TextVAlign = Infragistics.Win.VAlign.Middle;
			ultraGrid1.DisplayLayout.Bands[0].Columns["Season"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[1].Columns["Quarter"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[2].Columns["Month"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[3].Columns["Week"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;
			ultraGrid1.DisplayLayout.Bands[3].Columns["Date"].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Center;

            // Begin TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.
            ultraGrid1.DisplayLayout.Bands[3].Columns["Date"].Format = "d";
            ultraGrid1.DisplayLayout.Bands[3].Columns["Date"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Date;
            // End TT#11 - JSmith - The view of the calendar needs to pay attention to the culture info when displaying the dates.

		}
		private void SetText()
		{
			try
			{
				
				this.btnClose.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Close);
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

		private void cbExpandAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if(cbExpandAll.Checked)
				ultraGrid1.Rows.ExpandAll(true);
			else
				ultraGrid1.Rows.CollapseAll(true);
		}

		private void btnLeft_Click(object sender, System.EventArgs e)
		{
			_year--;
			SetDisplayYear();
		}

		private void btnRight_Click(object sender, System.EventArgs e)
		{
			_year++;
			SetDisplayYear();
		}

		private void ValidateYear(int year)
		{
			_year = year;
			SetDisplayYear();
		}

		private void SetDisplayYear()
		{
			_calendar.CalendarDisplay_CheckYear(_year);
			txtFiscalYear.Text = _year.ToString("0000", CultureInfo.CurrentUICulture);
			_dvCalendar.RowFilter = "Year = " +  _year.ToString(CultureInfo.CurrentUICulture);

			int wks = _calendar.GetNumWeeks(_year);
			this.lblNumWeeks.Text = wks.ToString(CultureInfo.CurrentUICulture);
			YearProfile yr = _calendar.GetYear(_year);
			this.lblNumMonths.Text = yr.Periods.ArrayList.Count.ToString(CultureInfo.CurrentUICulture);
			this.lblFirstDay.Text = yr.Date.ToString("dddd", CultureInfo.CurrentUICulture);
			this.lblNumQuarters.Text = (_calendar.GetNumQuarters(_year)).ToString();
			this.lblNumSeasons.Text = (_calendar.GetNumSeasons(_year)).ToString();

			if (_expanded == null)
			{
				_expanded = new bool[ultraGrid1.DisplayLayout.Bands.Count, 60];
				_expanded.Initialize();
			}
			else if (_expanded.GetLength(0) != ultraGrid1.DisplayLayout.Bands.Count)
			{
				_expanded = new bool[ultraGrid1.DisplayLayout.Bands.Count, 60];
				if (cbExpandAll.Checked)
					ExpandAll();
				else
					_expanded.Initialize();
			}

			ApplyGridExpandedList(ultraGrid1.Rows);
		}

		private void SetGridExpandedList(RowsCollection rowCollection)
		{
			// Loop through every row in the passed in rows collection.
			foreach (UltraGridRow row in rowCollection)
            {
				_expanded[row.Band.Index, row.Index] = row.Expanded;
				
                if ( null != row.ChildBands )
                {
                    // Loop throgh each of the child bands.
                    foreach ( UltraGridChildBand childBand in row.ChildBands )
                    {
                        // Call this method recursivedly for each child rows collection.
						SetGridExpandedList(childBand.Rows);
                    }
                }
            }
		}

		private void ApplyGridExpandedList(RowsCollection rowCollection)
		{
			// Loop through every row in the passed in rows collection.
			foreach (UltraGridRow row in rowCollection)
			{
				row.Expanded = _expanded[row.Band.Index, row.Index];

				if (null != row.ChildBands)
				{
					// Loop throgh each of the child bands.
					foreach (UltraGridChildBand childBand in row.ChildBands)
					{
						// Call this method recursivedly for each child rows collection.
						ApplyGridExpandedList(childBand.Rows);
					}
				}
			}
		}
		private void ExpandAll()
		{
			int xMax = _expanded.GetLength(0);
			int yMax = _expanded.GetLength(1);

			for (int x = 0; x < xMax; x++)
			{
				for (int y = 0; y < yMax; y++)
				{
					_expanded[x,y] = true;
				}
			}
		}

		private void txtFiscalYear_Leave(object sender, System.EventArgs e)
		{
			if ( txtFiscalYear.Text != _year.ToString(CultureInfo.CurrentUICulture) )
			{
				try
				{
					ValidateYear(Convert.ToInt32(txtFiscalYear.Text, CultureInfo.CurrentUICulture));
				}
				catch ( System.FormatException )
				{
					ValidateYear(0);
				}
			}
		}


		private void txtFiscalYear_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			// if they've pressed enter and the year has changed...if (
			if ( e.KeyChar == 13 && txtFiscalYear.Text != _year.ToString(CultureInfo.CurrentUICulture) )
			{
				try
				{
					ValidateYear(Convert.ToInt32(txtFiscalYear.Text, CultureInfo.CurrentUICulture));
				}
				catch ( System.FormatException )
				{
					ValidateYear(0);
				}
			}
		}

		private void ultraGrid1_AfterRowCollapsed(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			_expanded[e.Row.Band.Index, e.Row.Index] = false;
		}

		private void ultraGrid1_AfterRowExpanded(object sender, Infragistics.Win.UltraWinGrid.RowEventArgs e)
		{
			_expanded[e.Row.Band.Index, e.Row.Index] = true;
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
//			
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

		private void ultraGrid1_MouseEnterElement(object sender, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				ShowGridToolTip(ultraGrid1, e);
			}
			catch(Exception ex)
			{
				HandleException(ex);
			}
		}

		/// <summary>
		/// Shows ToolTip to display error message in an UntraGrid cell 
		/// </summary>
		/// <param name="ultraGrid">The UltraGrid where the tool tip is to be displayed</param>
		/// <param name="e">The UIElementEventArgs arguments of the MouseEnterElement event</param>
		protected void ShowGridToolTip(Infragistics.Win.UltraWinGrid.UltraGrid ultraGrid, Infragistics.Win.UIElementEventArgs e)
		{
			try
			{
				if(this.toolTip != null && this.toolTip.Active) 
				{
					this.toolTip.Active = false; //turn it off 
				}

				UltraGridCell gridCell = (UltraGridCell)e.Element.GetContext(typeof(Infragistics.Win.UltraWinGrid.UltraGridCell));
				if (gridCell != null)
				{
					if (gridCell.Column.Header.Caption == "First Day of Week")
					{
						string julian = gridCell.Row.Cells["JulianDate"].Value.ToString();
						toolTip.Active = true; 
						toolTip.SetToolTip(ultraGrid, "Julian: " + julian);
					}
				}
			}
			catch( Exception exception )
			{
				HandleException(exception);
			}
		}

	}
}
