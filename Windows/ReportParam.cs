using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using System.Data;
using System.IO;
using System.Xml;

//using System.Runtime.InteropServices; // for user32.dll if used directly
using MIDRetail.Business;
using MIDRetail.DataCommon;


namespace MIDRetail.Windows
{
	
	/// <summary>
	/// Summary description for ParamForm.
	/// </summary>
	public class ReportParam : System.Windows.Forms.Form
	{
		private DataTable						dataTable;
		private DataGridTextBoxColumn			datagridtextBox;
		private DataGrid.HitTestInfo			hitTestGrid;	   
		//private	string[]						arrstr ;
        //private bool bBusy = false;
        //private bool bValidate = false;
		private string msg = string.Empty;

		protected static CrystalHelper cryCrystalHelper;

		public string m_txtFile = string.Empty;
		public string m_xmlFile = string.Empty;
		public string m_xsdFile = string.Empty;
		public string m_rptFile = string.Empty;
		public string m_rptFileName = string.Empty;

//		FlatDateTimePicker dtp	= new FlatDateTimePicker(); //Display DateTime
//		FlatDateTimePicker ddp	= new FlatDateTimePicker(); //Display Date ONLY
//		FlatDateTimePicker ttp	= new FlatDateTimePicker(); //Display Time ONLY
		CheckBox chk			= new CheckBox();			//Display CheckBox

		// report document
		private CrystalDecisions.CrystalReports.Engine.ReportDocument crDoc;
		private System.Windows.Forms.DataGrid dgParam;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnViewReport;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Label lblError;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ReportParam()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
	
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ReportParam));
			this.dgParam = new System.Windows.Forms.DataGrid();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnViewReport = new System.Windows.Forms.Button();
			this.lblError = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgParam)).BeginInit();
			this.SuspendLayout();
			// 
			// dgParam
			// 
			this.dgParam.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dgParam.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.dgParam.CaptionBackColor = System.Drawing.SystemColors.Control;
			this.dgParam.CaptionFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgParam.CaptionForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParam.CaptionText = "Crystal Report Parameters";
			this.dgParam.DataMember = "";
			this.dgParam.Dock = System.Windows.Forms.DockStyle.Top;
			this.dgParam.FlatMode = true;
			this.dgParam.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgParam.Location = new System.Drawing.Point(20, 5);
			this.dgParam.Name = "dgParam";
			this.dgParam.Size = new System.Drawing.Size(624, 270);
			this.dgParam.TabIndex = 0;
			this.dgParam.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgParam_MouseDown);
			this.dgParam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgParam_MouseUp);
			// 
			// btnRefresh
			// 
			this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnRefresh.Location = new System.Drawing.Point(384, 312);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(80, 23);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnCancel.Location = new System.Drawing.Point(480, 312);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnViewReport
			// 
			this.btnViewReport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.btnViewReport.Location = new System.Drawing.Point(568, 312);
			this.btnViewReport.Name = "btnViewReport";
			this.btnViewReport.TabIndex = 6;
			this.btnViewReport.Text = "View Report";
			this.btnViewReport.Click += new System.EventHandler(this.btnViewReport_Click);
			// 
			// lblError
			// 
			this.lblError.BackColor = System.Drawing.SystemColors.Control;
			this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(24, 288);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(344, 48);
			this.lblError.TabIndex = 7;
			// 
			// ParamForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(664, 342);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.btnViewReport);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.dgParam);
			this.DockPadding.Bottom = 20;
			this.DockPadding.Left = 20;
			this.DockPadding.Right = 20;
			this.DockPadding.Top = 5;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ParamForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select Parameters";
			this.Load += new System.EventHandler(this.ParamForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgParam)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

//		/// <summary>
//		/// The main entry point for the application.
//		/// </summary>
//		[STAThread]
//		static void Main() 
//		{
//			Application.Run(new ParamForm());
//		}

		/// <summary>
		/// Method defn to create the Data Grid using data table
		/// </summary>
		private void CreateGrid()
		{
			DataColumn dtCol = null;
			//Create Parameter Data Table object to hold columns and rows
			dataTable = MIDEnvironment.CreateDataTable("dtParam");

			// This array method gives larger, incorrect column widths!
			// Array seems to set a larger default font size!
			/*
			//Create String array object, initialize array with column names
			arrstr			= new string [7];
			arrstr[0]		= "Prompt";		
			arrstr[1]		= "Value";
			arrstr[2]		= "Kind";
			arrstr[3]		= "Min";
			arrstr[4]		= "Max";
			arrstr[5]		= "Parameter";
			//Add string array of columns to the DataColumn object 		
			for(int i=0; i< 7;i++)
			{	
				string str		= arrstr[i];
				dtCol			= new DataColumn(str);
				dtCol.DataType		= System.Type.GetType("System.String");
				dtCol.DefaultValue  	= "";
				dataTable.Columns.Add(dtCol);		
			}
			*/

			dtCol				= new DataColumn("Prompt");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue  = "";
			dataTable.Columns.Add(dtCol);		
	
			dtCol				= new DataColumn("Value");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue	= "";
			dataTable.Columns.Add(dtCol);		

			dtCol				= new DataColumn("Kind");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue	= "";
			dataTable.Columns.Add(dtCol);

			dtCol				= new DataColumn("Min");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue  = "";
			dataTable.Columns.Add(dtCol);		

			dtCol				= new DataColumn("Max");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue  = "";
			dataTable.Columns.Add(dtCol);	
			
			dtCol				= new DataColumn("Param");
			dtCol.DataType		= System.Type.GetType("System.String");
			dtCol.DefaultValue  = "";
			dataTable.Columns.Add(dtCol);

		}

		/// <summary>
		/// Load .rpt report report file & read parameters and constraints
		/// Dynamically create datagrid & controls to display parameters
		/// </summary>
		/// <param name="reportName">Name of report to load</param>
		private void LoadReport(string reportName)
		{
			//if (bLoad)
			//	return;

			//bLoad = true;
			//MessageBox.Show(m_rptFile);

			CreateGrid();

			crDoc =	new CrystalDecisions.CrystalReports.Engine.ReportDocument();
			crDoc.Load(reportName);


			// ******* Read parameters directly from .rpt file ********

			// get parameter field definitions in collection of ParameterFieldDefinition objects
			CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinitions 
				crParamFieldDefinitions = crDoc.DataDefinition.ParameterFields;

			
			// if no params, then go to report page
			if (crParamFieldDefinitions.Count < 1)
			{
				// You can return or write special xml file here!
				return;
			}

			string paramName;

			int i = 0;

			// iterate over collection, processing each ParameterFieldDefinition 
			foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition
						 def in crParamFieldDefinitions)
			{
				// extract parameter field name
				paramName = def.ParameterFieldName;

				if ( paramName.ToString().Length < 1 )
					return;

				// get current values which should be none...
				CrystalDecisions.Shared.ParameterValues crCurrentValues = def.CurrentValues;
				//Debug.Assert(crCurrentValues.Count == 0);  

				// get default values if any
				CrystalDecisions.Shared.ParameterValues crDefaultValues = def.DefaultValues;
				int countDefaultValues = crDefaultValues.Count;

				// These are the 3 value types used in Crystal Reports:
				// DiscreteOrRangeKind.DiscreteAndRangeValue;
				// DiscreteOrRangeKind.DiscreteValue;
				// DiscreteOrRangeKind.RangeValue			
				CrystalDecisions.Shared.DiscreteOrRangeKind discreteOrRange = def.DiscreteOrRangeKind;

				// Get the Edit Mask so you can use this in validating parameter
				string editMask = def.EditMask;

				bool enableAllowEditingDefaultValue = def.EnableAllowEditingDefaultValue;
				bool enableAllowMultipleValue = def.EnableAllowMultipleValue;
				bool enableNullValue = def.EnableNullValue;
				object maxValue = def.MaximumValue;
				object minValue = def.MinimumValue;

			
				CrystalDecisions.Shared.ParameterValueKind kind = def.ParameterValueKind;
				string promptText = def.PromptText;
	
				// Create control, if any, based on ParameterValueKind
				// These are the different Parameter Value Kinds used in Crystal Reports:
				// ParameterValueKind.StringParameter;
				// ParameterValueKind.NumberParameter;
				// ParameterValueKind.CurrencyParameter;
				// ParameterValueKind.BooleanParameter;
				// ParameterValueKind.DateTimeParameter;
				// ParameterValueKind.DateParameter;
				// ParameterValueKind.TimeParameter;
				switch (kind)
				{
						// StringParameter
					case CrystalDecisions.Shared.ParameterValueKind.StringParameter:
						//For Client you would only show these 3 columns: Prompt & Default Value
						//dataTable.LoadDataRow(new string[3]{promptText,GetDefaultValue(crDefaultValues),"String"},true);
						//For purposes of this article I will display ALL parameter info in datagrid...
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultValue(crDefaultValues),"String","","",paramName},true);
						i++;
						break;

						// NumberParameter
					case CrystalDecisions.Shared.ParameterValueKind.NumberParameter:
						string min = string.Empty;
						string max = string.Empty;
						if ( (minValue != null) && (maxValue != null) &&
							(minValue.ToString().Length != 0) && (maxValue.ToString().Length != 0) ) 
						{
							min = minValue.ToString();
							max = maxValue.ToString();
						}
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultValue(crDefaultValues),"Number",min.ToString(),max.ToString(),paramName},true);
						i++;
						break;

						// CurrencyParameter
					case CrystalDecisions.Shared.ParameterValueKind.CurrencyParameter:
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultValue(crDefaultValues),"Currency","","",paramName},true);
						i++;
						break;

						// BooleanParameter
					case CrystalDecisions.Shared.ParameterValueKind.BooleanParameter:
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultValue(crDefaultValues),"Boolean","","",paramName},true);
						i++;
						break;
						
						// DateTimeParameter
					case CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter:
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultDate(crDefaultValues).ToString(),"DateTime","","",paramName},true);
						i++;
						break;

						// DateParameter
					case CrystalDecisions.Shared.ParameterValueKind.DateParameter:
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultDate(crDefaultValues).ToShortDateString(),"Date","","",paramName},true);
						i++;
						break;

						// TimeParameter
					case CrystalDecisions.Shared.ParameterValueKind.TimeParameter:
						dataTable.LoadDataRow(new string[6]{promptText,GetDefaultDate(crDefaultValues).ToShortTimeString(),"Time","","",paramName},true);
						i++;
						break;

				}	// end switch

			}
	
			//Set Data Grid Source as Data Table created above
			dgParam.DataSource	= dataTable;

			//Set style property first time grid loads, after that it maintains properties
			if(!dgParam.TableStyles.Contains("dtParam"))
			{
				dgParam.TableStyles.Clear();

				// Continue to set DataGrid properties directly, but only
				// those that are not covered by DataGridTableStyle properties.
				dgParam.CaptionFont				= new Font("Tahoma", (float)10.0, FontStyle.Bold);
				dgParam.CaptionForeColor		= Color.Black;
				dgParam.CaptionText				= "Crystal Report Parameters";
				dgParam.Font					= new Font("Tahoma", (float)8.0);
				dgParam.BorderStyle				= BorderStyle.None;
				dgParam.BackgroundColor			= Color.FromArgb(238, 237, 221);
				dgParam.CaptionBackColor		= Color.FromArgb(238, 237, 221);
				dgParam.CaptionForeColor		= Color.FromArgb(0, 0, 0);

				//Create DataGridTableStyle object	
				DataGridTableStyle tableStyle	= new DataGridTableStyle();

				//Set its properties
				tableStyle.MappingName			= dataTable.TableName; //its table name of dataset
				dgParam.TableStyles.Add(tableStyle);

				tableStyle.RowHeadersVisible	= false; //Shows/Hides Column on left with arrow
				tableStyle.ColumnHeadersVisible	= true;	
				tableStyle.HeaderBackColor		= Color.LightSteelBlue;
				tableStyle.AllowSorting			= false;
				tableStyle.HeaderBackColor		= Color.DarkSeaGreen;//.FromArgb(238, 237, 221);
				tableStyle.HeaderForeColor		= Color.White;
				tableStyle.HeaderFont			= new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,	((System.Byte)(0)));
				tableStyle.GridLineColor		= Color.DarkGray;
				tableStyle.PreferredRowHeight	= 24;

				//set size of individual columns in GridColumnStylesCollection object 
				GridColumnStylesCollection	colStyle;
				colStyle				= dgParam.TableStyles["dtParam"].GridColumnStyles;
				colStyle[0].Width		= 190;	//Prompt for parameter
				colStyle[1].Width		= 194;	//Parameter Value
				colStyle[2].Width		= 60;	//Parameter Type
				//I only show these other columns for purposes of this article
				//For clients the first 3 parameters are sufficient
				colStyle[3].Width		= 50;	//Min value of parameter
				colStyle[4].Width		= 50;	//Max value of parameter
				colStyle[5].Width      	= 80;	//Parameter Name

				// Set Datagrid's width equal to sum of all coulmn widths
				dgParam.Width = colStyle[0].Width + colStyle[1].Width + colStyle[2].Width + 
								colStyle[3].Width + colStyle[4].Width + colStyle[5].Width;

				colStyle[0].Alignment = HorizontalAlignment.Center;
				colStyle[1].Alignment = HorizontalAlignment.Center;
				colStyle[2].Alignment = HorizontalAlignment.Center;
				colStyle[3].Alignment = HorizontalAlignment.Center;
				colStyle[4].Alignment = HorizontalAlignment.Center;
				colStyle[5].Alignment = HorizontalAlignment.Center;

				colStyle[0].ReadOnly = true;
				colStyle[1].ReadOnly = false;	//value
				colStyle[2].ReadOnly = true;
				colStyle[3].ReadOnly = true;
				colStyle[4].ReadOnly = true;
				colStyle[5].ReadOnly = true;

				// Add handlers to column for "value": GridColumnStyles[1]
				datagridtextBox = (DataGridTextBoxColumn)dgParam.TableStyles[0].GridColumnStyles[1];
				datagridtextBox.TextBox.GotFocus	+= new EventHandler(this.dgFunction_GotFocus);
				datagridtextBox.TextBox.LostFocus	+= new EventHandler(this.dgFunction_LostFocus);

				dgParam.TableStyles.Add(tableStyle);

			}
			//this.ResumeLayout(false);
		}


		// To automatically drop down calendar disply we send "WM_KEYDOWN = 0x100;"
		// Update calendar button using SendMessage(dtp.Handle, 0x100, 0x73, 0x3E0001);
		// This requires that we import SendMessage method of the User32 DLL.   
		// Moved this to Win32.cs:
		//[DllImport("user32.dll", CharSet=CharSet.Auto)]
		//public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

		/// <summary>
		/// Method defn to add the control of your choice to the data grid
		/// </summary>
		/// <param name="o"></param>
		/// <param name="e"></param>
		private void dgFunction_GotFocus(object o, EventArgs e)
		{
			if(hitTestGrid == null)
				return;

//			if(dtp != null)
//				datagridtextBox.TextBox.Controls.Remove(dtp);
//
//			if(ddp != null)
//				datagridtextBox.TextBox.Controls.Remove(ddp);
//
//			if(chk != null)
//				datagridtextBox.TextBox.Controls.Remove(chk);
//
//			if(ttp != null)
//				datagridtextBox.TextBox.Controls.Remove(ttp);
//
//			//Create datetime picker control to be added and set its properties
//			dtp							= new FlatDateTimePicker();
//			dtp.Dock					= DockStyle.Fill;
//			dtp.Cursor					= Cursors.Arrow;
//
//			//Create datetime picker control to be added and set its properties
//			ddp							= new FlatDateTimePicker();
//			ddp.Dock					= DockStyle.Fill;
//			ddp.Cursor					= Cursors.Arrow;
//
//			//Create check box control to be added and set its properties
//			chk							= new CheckBox();
//			chk.Dock					= DockStyle.Fill;
//			chk.Cursor					= Cursors.Arrow;
//
//			//These settings convert DateTime Control to Time Spinner Control
//			ttp							= new FlatDateTimePicker();
//			ttp.Dock					= DockStyle.Fill;
//			ttp.Cursor					= Cursors.Arrow;
//			ttp.CustomFormat = "hh:mm:tt";
//			ttp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
//			ttp.Name = "ttp";
//			ttp.ShowUpDown = true;
//
//			//dtp.CloseUp      += new System.EventHandler(this.dtp_ClosedUp);
//			dtp.ValueChanged += new System.EventHandler(this.dtp_ValueChanged);
//			ddp.ValueChanged += new System.EventHandler(this.ddp_ValueChanged);
//			chk.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
//			ttp.ValueChanged += new System.EventHandler(this.ttp_ValueChanged);
//
//			if(dgParam[hitTestGrid.Row,2].ToString().Equals("DateTime"))
//			{			
//				DateTime value = DateTime.Parse(dgParam[hitTestGrid.Row,1].ToString());
//				datagridtextBox.TextBox.Controls.Add(dtp);
//				dtp.Value = value;
//				chk.SendToBack();
//				ddp.SendToBack();
//				ttp.SendToBack();
//				dtp.BringToFront();
//				// Use this if you want the calendar to drop down automatically
//				// Send Windows message "WM_KEYDOWN = 0x100" to update button. 
//				// Unslash this to automatically drop down calendar disply
//				//Win32.SendMessage(dtp.Handle, 0x100, 0x73, 0x3E0001);
//			}
//			else if(dgParam[hitTestGrid.Row,2].ToString().Equals("Date"))
//			{	
//				DateTime value = DateTime.Parse(dgParam[hitTestGrid.Row,1].ToString());
//				datagridtextBox.TextBox.Controls.Add(ddp);
//				ddp.Value = value;
//				dtp.SendToBack();
//				chk.SendToBack();
//				ttp.SendToBack();
//				ddp.BringToFront();
//			}
//			else if(dgParam[hitTestGrid.Row,2].ToString().Equals("Boolean"))
//			{
//				//I prefer using a checkbox here instead of radio buttons
//				// but you could use radio buttons here if you prefer
//				datagridtextBox.TextBox.Controls.Add(chk);
//				if(dgParam[hitTestGrid.Row,1].ToString().Equals("True"))
//					chk.Checked = true;
//				else
//					chk.Checked = false;
//				dtp.SendToBack();
//				ddp.SendToBack();
//				ttp.SendToBack();
//				chk.BringToFront();
//			}
//			else if(dgParam[hitTestGrid.Row,2].ToString().Equals("Time"))
//			{
//				DateTime value = DateTime.Parse(dgParam[hitTestGrid.Row,1].ToString());
//				datagridtextBox.TextBox.Controls.Add(ttp);
//				ttp.Value = value;
//				chk.SendToBack();
//				dtp.SendToBack();
//				ddp.SendToBack();
//				ttp.BringToFront();
//			}

			// for the timepicker you could use this code instead:
			/*
			DTS_TIMEFORMAT = 0x9;
			this._hWnd = Win32.CreateWindowEx(
				0,
				"SysDateTimePick32",
				"",
				Win32.WS_VISIBLE | Win32.WS_BORDER | Win32.WS_CHILD | Win32.DTS_TIMEFORMAT,
				this.Left,
				this.Top,
				this.Width,
				this.Height,
				hWnd,
				0,
				0,
				null);
			*/

            //bBusy = false;

		}

		private void dgFunction_LostFocus(object o, EventArgs e)
		{
			if(hitTestGrid == null)
				return;
		}

		/// <summary>
		/// Mouse down event of data grid
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgParam_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			//Take hit test that will be used to identify which row has been clicked 
			hitTestGrid = dgParam.HitTest(e.X,e.Y);
		}

		private void dgParam_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			hitTestGrid = dgParam.HitTest(e.X,e.Y);
		}

		private void dtp_ValueChanged(object sender, System.EventArgs e)
		{
//			if(dgParam[hitTestGrid.Row,2].ToString().Equals("DateTime"))
//				dgParam[hitTestGrid.Row,1] = dtp.Value.ToString();
		}
	
		private void ddp_ValueChanged(object sender, System.EventArgs e)
		{
//			if(dgParam[hitTestGrid.Row,2].ToString().Equals("Date") &&
//				!dgParam[hitTestGrid.Row,2].ToString().Equals("DateTime"))
//				dgParam[hitTestGrid.Row,1] = ddp.Value.ToShortDateString();
		}
	
		private void chk_CheckedChanged(object sender, System.EventArgs e)
		{
			if(dgParam[hitTestGrid.Row,2].ToString().Equals("Boolean"))
				dgParam[hitTestGrid.Row,1] = chk.Checked;		
		}

		private void ttp_ValueChanged(object sender, System.EventArgs e)
		{
//			if(dgParam[hitTestGrid.Row,2].ToString().Equals("Time"))
//				dgParam[hitTestGrid.Row,1] = ttp.Value.ToShortTimeString();
		}

		/// <summary>
		/// Returns default DateTime, Date, and Time
		/// </summary>
		/// <param name="crDefaultValues"></param>
		/// <returns></returns>
		private DateTime GetDefaultDate(CrystalDecisions.Shared.ParameterValues crDefaultValues)
		{
			// If there is a defalut value and it is not null
			if ( crDefaultValues.Count > 0 && (crDefaultValues[0] != null) )
			{
				// Extract first element in collection and cast
				// it to a ParameterDiscreteValue type.
				CrystalDecisions.Shared.ParameterDiscreteValue theDefaultValue = 
					(CrystalDecisions.Shared.ParameterDiscreteValue) 
					crDefaultValues[0];

				// Cast value property of ParameterDiscreteValue to DateTime object
				DateTime defaultVal = (DateTime) theDefaultValue.Value;
				return defaultVal;
			}
			else
			{
				return DateTime.Now;
			}
		}

		/// <summary>
		/// Returns default string value
		/// </summary>
		/// <param name="crDefaultValues"></param>
		/// <returns></returns>
		private string GetDefaultValue(CrystalDecisions.Shared.ParameterValues crDefaultValues)
		{
			// Test if there are any default values and that they are not null
			if ( crDefaultValues.Count > 0 && (crDefaultValues[0] != null) )
			{
				// Extract from default values collection and cast
				// to type ParameterDiscreteValue
				CrystalDecisions.Shared.ParameterDiscreteValue theDefaultValue = 
					(CrystalDecisions.Shared.ParameterDiscreteValue) 
					crDefaultValues[0];

				// Return default value as a string
				string defaultVal = theDefaultValue.Value.ToString();
				return defaultVal;
			}
			else
			{
				return "";
			}
		}

		private void ParamForm_Load(object sender, System.EventArgs e)
		{
			if(File.Exists(m_rptFile)) 	
				LoadReport(m_rptFile);	
		}

		private void btnRefresh_Click(object sender, System.EventArgs e)
		{	
            //bBusy = false;
			if(File.Exists(m_rptFile)) 	
				LoadReport(m_rptFile);		
		}


		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnViewReport_Click(object sender, System.EventArgs e)
		{
			//if(XMLValidate())  // You can use an xml to transfer parameter values,
			// OR, do it through singleton object, either way works fine!
			if(ValidateParams())
				this.DialogResult = DialogResult.OK;
			else
				this.DialogResult = DialogResult.None;
		}


		private bool ValidateParams()
		{	 
			singleton oSingleton = singleton.GetCurrentSingleton;


			//////////////////////////////////////////
			bool bOk = true;
			string sError = string.Empty;
			DataSet dsParam = MIDEnvironment.CreateDataSet();
			dsParam = CloneDataTable(dgParam);		
			if (dsParam == null) 
				return false; 

			//VALIDATION
			try
			{
				foreach (DataRow dr in dsParam.Tables[0].Rows) 
				{ 
					if(dr["value"].ToString().Length < 1) 
					{ 
						bOk = false;
						sError = sError + dr["param"].ToString() + " is invalid\r\n";
					}
					if((dr["kind"].ToString().CompareTo("number")>0) &&
						(dr["min"].ToString().Length != 0) && 
						(dr["max"].ToString().Length != 0) )
					{ 
						Int32 iMin = Int32.Parse(dr["min"].ToString());
						Int32 iMax = Int32.Parse(dr["max"].ToString());
						Int32 iValue = Int32.Parse(dr["value"].ToString());
						if( (iValue<iMin) || (iValue>iMax) )
						{
							bOk = false;
							sError = sError + dr["param"].ToString() + " violates Min/Max\r\n";
						}
					}
				}
			}
			catch {	}

			if(bOk)
			{
				// get parameter field definitions 
				CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinitions 
					crParamFieldDefinitions = crDoc.DataDefinition.ParameterFields;

				bool bFound = false;
				foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition
							 def in crParamFieldDefinitions)
				{
					string paramName = def.ParameterFieldName;
					CrystalDecisions.Shared.ParameterValueKind kind = def.ParameterValueKind;
					string currentValue = "";

					try
					{
						foreach (DataRow dr in dsParam.Tables[0].Rows) 
						{ 
							int cmpVal = dr["param"].ToString().CompareTo(paramName.ToString());
							//if (cmpVal == 0) // the values are the same
							//else if (cmpVal > 0) // the first value is greater than the second value          
							//else // the second string is greater than the first string
							if(cmpVal == 0)
							{
								currentValue = dr["value"].ToString();
								bFound = true;
							}
						}
					}
					catch {	}

					if(bFound)
					{
						bFound = false;
						// create new Parameter Discrete Value object
						CrystalDecisions.Shared.ParameterDiscreteValue crParamDiscreteValue = 
							new CrystalDecisions.Shared.ParameterDiscreteValue();

						// Set value of Discrete value object 
						crParamDiscreteValue.Value = currentValue;

						// extract collection of current values
						CrystalDecisions.Shared.ParameterValues crCurrentValues = def.CurrentValues;

						// Add Discrete value object to collection of current values
						crCurrentValues.Add(crParamDiscreteValue);	

						// apply modified current values to param collection
						def.ApplyCurrentValues(crCurrentValues);
					}

				}	// end foreach parameter

				//singleton oSingleton = singleton.GetCurrentSingleton();
				oSingleton.CrystalDoc = crDoc;
				oSingleton.LoadReport = true;

			}

			if(!bOk)
				lblError.Text = sError.ToString();
			else
			{
				lblError.Text = "No errors!";
			}
            //bBusy = false;
			return bOk;

		}


		private bool XMLValidate()
		{	 
			bool bOk = true;
			string sError = string.Empty;
			
			DataSet dsParam = MIDEnvironment.CreateDataSet();
			dsParam = CloneDataTable(dgParam);
		
			if (dsParam == null) 
			{ 
				m_txtFile = string.Empty;
				m_xmlFile = string.Empty;
				m_xsdFile = string.Empty;
				return false; 
			}	

			//VALIDATION
			try
			{
				foreach (DataRow dr in dsParam.Tables[0].Rows) 
				{ 
					if(dr["value"].ToString().Length < 1) 
					{ 
						bOk = false;
						sError = sError + dr["param"].ToString() + " is invalid\r\n";
					}
					if((dr["kind"].ToString().CompareTo("number")>0) &&
						(dr["min"].ToString().Length != 0) && 
						(dr["max"].ToString().Length != 0) )
					{ 
						Int32 iMin = Int32.Parse(dr["min"].ToString());
						Int32 iMax = Int32.Parse(dr["max"].ToString());
						Int32 iValue = Int32.Parse(dr["value"].ToString());
						if( (iValue<iMin) || (iValue>iMax) )
						{
							bOk = false;
							sError = sError + dr["param"].ToString() + " violates Min/Max\r\n";
						}
					}
				}
			}
			catch {	}
			
			// Writes current data for DataSet created to specified files.
			m_txtFile = MIDConfigurationManager.AppSettings["InitialReportsDirectory"] + @"\reports.txt";
			m_xmlFile = MIDConfigurationManager.AppSettings["InitialReportsDirectory"] + @"\reports.xml";
			m_xsdFile = MIDConfigurationManager.AppSettings["InitialReportsDirectory"] + @"\reports.xsd";

			try
			{
				dsParam.WriteXml(m_txtFile);
				dsParam.WriteXml(m_xmlFile, XmlWriteMode.WriteSchema);
				dsParam.WriteXmlSchema(m_xsdFile);
			}
			catch 
			{ 
				bOk = false;
				sError = sError + "Error in writing xml file!\r\n";
			}
			if(!bOk)
				lblError.Text = sError.ToString();
			else
			{
				lblError.Text = "No errors!";
			}
            //bBusy = false;
			return bOk;
		}

		private DataSet CloneDataTable(DataGrid dgTable)
		{
			DataSet ds = MIDEnvironment.CreateDataSet();
			DataTable dtSource = null;
			DataTable dt = MIDEnvironment.CreateDataTable();
			DataRow dr;
			if(dgTable.DataSource != null)
			{
				if (dgTable.DataSource.GetType() == typeof(DataSet))
				{
					DataSet dsSource = (DataSet)dgTable.DataSource;
					// assume DataSet contains desired table at index 0
					dtSource = dsSource.Tables[0];
				}
				else
					if (dgTable.DataSource.GetType() == typeof(DataTable))
					dtSource = (DataTable)dgTable.DataSource;
				if (dtSource != null)
				{
					dt = dtSource.Clone();
					// dgConversionTable.CaptionText;
					//dt.TableName = dtSource.TableName;
					dt.TableName = "parameter";
					
					dt.BeginLoadData();
					foreach (DataRow drSource in dtSource.Rows)
					{
						dr = dt.NewRow();
						foreach (DataColumn dc in dtSource.Columns)
						{
							dr[dc.ColumnName] = drSource[dc.ColumnName];
						}
						dt.Rows.Add(dr);
					}
					dt.AcceptChanges();
					dt.EndLoadData();
					ds.Tables.Add(dt);
				}
			}
			return ds;
		}
	}
}

