//using System;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Globalization;
//using System.Drawing.Text;
//using System.Drawing.Printing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
////using System.Data.OleDb;
//using Infragistics.UltraChart.Shared.Styles;
//using Infragistics.UltraChart.Resources;
//using Infragistics.UltraChart.Data;
//using Infragistics.UltraChart.Render;
//
//namespace MIDRetail.Windows
//{
//	/// <summary>
//	/// Summary description for Charts.
//	/// </summary>
//	public class Charts : System.Windows.Forms.Form
//	{
//		public enum ChartTypeEnum
//		{
//			column,
//			bar, 
//			line,
//			pie,
//			threeDBar
//		}
//		private ChartTypeEnum _chartType;
//		private bool _chainData = true;
//		private DataTable _chainTable;
//		private DataTable _storeTable;
//		private DataTable _pieChainTable;
//		private DataTable _pieStoreTable;
//		private DataTable _lineChainTable;
//		private DataTable _lineStoreTable;
////		private DataSet _3DDataSet;
////		private DataTable _3DTable;
//		private DataTable _typeTable;
//		internal System.Windows.Forms.GroupBox GroupBox1;
//		internal System.Windows.Forms.DataGrid DataGrid1;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxChartType;
//		private System.Windows.Forms.Label lblChartType;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxChart;
//		private System.Windows.Forms.Label label1;
//		private System.Windows.Forms.Label label2;
//		private MIDRetail.Windows.Controls.MIDComboBoxEnh cbxLegendLoc;
//		private Infragistics.Win.UltraWinChart.UltraChart ultraChart1;
//		/// <summary>
//		/// Required designer variable.
//		/// </summary>
//		private System.ComponentModel.Container components = null;
//
//		public Charts()
//		{
//
//			//
//			// Required for Windows Form Designer support
//			//
//			InitializeComponent();
//
//			//
//			// TODO: Add any constructor code after InitializeComponent call
//			//
//		}
//
//		/// <summary>
//		/// Clean up any resources being used.
//		/// </summary>
//		protected override void Dispose( bool disposing )
//		{
//			if( disposing )
//			{
//				if (components != null) 
//				{
//					components.Dispose();
//				}
//			}
//			base.Dispose( disposing );
//		}
//
//		#region Windows Form Designer generated code
//		/// <summary>
//		/// Required method for Designer support - do not modify
//		/// the contents of this method with the code editor.
//		/// </summary>
//		private void InitializeComponent()
//		{
//			this.GroupBox1 = new System.Windows.Forms.GroupBox();
//			this.DataGrid1 = new System.Windows.Forms.DataGrid();
//			this.cbxChartType = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.lblChartType = new System.Windows.Forms.Label();
//			this.cbxChart = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.label1 = new System.Windows.Forms.Label();
//			this.label2 = new System.Windows.Forms.Label();
//			this.cbxLegendLoc = new MIDRetail.Windows.Controls.MIDComboBoxEnh();
//			this.ultraChart1 = new Infragistics.Win.UltraWinChart.UltraChart();
//			this.GroupBox1.SuspendLayout();
//			((System.ComponentModel.ISupportInitialize)(this.DataGrid1)).BeginInit();
//			this.SuspendLayout();
//			// 
//			// GroupBox1
//			// 
//			this.GroupBox1.Controls.AddRange(new System.Windows.Forms.Control[] {
//																					this.DataGrid1});
//			this.GroupBox1.Location = new System.Drawing.Point(80, 512);
//			this.GroupBox1.Name = "GroupBox1";
//			this.GroupBox1.Size = new System.Drawing.Size(656, 168);
//			this.GroupBox1.TabIndex = 5;
//			this.GroupBox1.TabStop = false;
//			this.GroupBox1.Text = "Data";
//			// 
//			// DataGrid1
//			// 
//			this.DataGrid1.CaptionText = "Data";
//			this.DataGrid1.DataMember = "";
//			this.DataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
//			this.DataGrid1.Location = new System.Drawing.Point(19, 17);
//			this.DataGrid1.Name = "DataGrid1";
//			this.DataGrid1.Size = new System.Drawing.Size(621, 140);
//			this.DataGrid1.TabIndex = 5;
//			// 
//			// cbxChartType
//			// 
//			this.cbxChartType.Location = new System.Drawing.Point(864, 184);
//			this.cbxChartType.Name = "cbxChartType";
//			this.cbxChartType.Size = new System.Drawing.Size(121, 21);
//			this.cbxChartType.TabIndex = 6;
//			this.cbxChartType.SelectionChangeCommitted += new System.EventHandler(this.cbxChartType_SelectionChangeCommitted);
//			// 
//			// lblChartType
//			// 
//			this.lblChartType.Location = new System.Drawing.Point(864, 152);
//			this.lblChartType.Name = "lblChartType";
//			this.lblChartType.Size = new System.Drawing.Size(128, 23);
//			this.lblChartType.TabIndex = 7;
//			this.lblChartType.Text = "Chart Type";
//			this.lblChartType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			// 
//			// cbxChart
//			// 
//			this.cbxChart.Location = new System.Drawing.Point(864, 112);
//			this.cbxChart.Name = "cbxChart";
//			this.cbxChart.Size = new System.Drawing.Size(121, 21);
//			this.cbxChart.TabIndex = 9;
//			this.cbxChart.SelectionChangeCommitted += new System.EventHandler(this.cbxChart_SelectionChangeCommitted);
//			// 
//			// label1
//			// 
//			this.label1.Location = new System.Drawing.Point(864, 80);
//			this.label1.Name = "label1";
//			this.label1.TabIndex = 10;
//			this.label1.Text = "Chart";
//			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			// 
//			// label2
//			// 
//			this.label2.Location = new System.Drawing.Point(864, 224);
//			this.label2.Name = "label2";
//			this.label2.TabIndex = 12;
//			this.label2.Text = "Legend Location";
//			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			// 
//			// cbxLegendLoc
//			// 
//			this.cbxLegendLoc.Location = new System.Drawing.Point(864, 256);
//			this.cbxLegendLoc.Name = "cbxLegendLoc";
//			this.cbxLegendLoc.Size = new System.Drawing.Size(121, 21);
//			this.cbxLegendLoc.TabIndex = 13;
//			this.cbxLegendLoc.SelectionChangeCommitted += new System.EventHandler(this.cbxLegendLoc_SelectionChangeCommitted);
//			// 
//			// ultraChart1
//			// 
//			this.ultraChart1.AutoScrollMargin = new System.Drawing.Size(0, 0);
//			this.ultraChart1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
//			this.ultraChart1.Axis.X.TickmarkInterval = 0;
//			this.ultraChart1.Axis.Y.TickmarkInterval = 0;
//			this.ultraChart1.Axis.Z.TickmarkInterval = 0;
//			this.ultraChart1.Location = new System.Drawing.Point(16, 16);
//			this.ultraChart1.Name = "ultraChart1";
//			this.ultraChart1.Size = new System.Drawing.Size(832, 488);
//			this.ultraChart1.TabIndex = 14;
//			// 
//			// Charts
//			// 
//			this.AutoScale = false;
//			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
//			this.ClientSize = new System.Drawing.Size(1006, 699);
//			this.Controls.AddRange(new System.Windows.Forms.Control[] {
//																		  this.ultraChart1,
//																		  this.cbxLegendLoc,
//																		  this.label2,
//																		  this.label1,
//																		  this.cbxChart,
//																		  this.lblChartType,
//																		  this.cbxChartType,
//																		  this.GroupBox1});
//			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
//			this.Name = "Charts";
//			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//			this.Text = "Data Chart";
//			this.Load += new System.EventHandler(this.Charts_Load);
//			this.GroupBox1.ResumeLayout(false);
//			((System.ComponentModel.ISupportInitialize)(this.DataGrid1)).EndInit();
//			this.ResumeLayout(false);
//
//		}
//		#endregion
//
//		/// <summary>
//		/// The main entry point for the application.
//		/// </summary>
//		[STAThread]
//		static void Main() 
//		{
//			Application.Run(new Charts());
//		}
////		public void InitializeForm()
////		{
////			Charts_Load();
////		}
//
//		private void Charts_Load(object sender, System.EventArgs e)
//			//		private void Charts_Load()
//		{
//			ultraChart1.ColorModel.ModelStyle=ColorModels.CustomLinear;
//			Color[] chartColors;
//			chartColors = new Color[] {Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet}; 
//			ultraChart1.ColorModel.CustomPalette = chartColors; 
//
//			DataTable chartTable  = MIDEnvironment.CreateDataTable();
//			DataColumn chartColType = new DataColumn("Chart", Type.GetType("System.String"));
//			chartTable.Columns.Add(chartColType);
//			chartTable.Rows.Add(new object[] { "Chain"});
//			chartTable.Rows.Add(new object[] { "Store"});
//			cbxChart.DataSource = chartTable;
//			cbxChart.DisplayMember = "Chart";
//
//			_typeTable  = MIDEnvironment.CreateDataTable();
//			DataColumn ColType = new DataColumn("Type", Type.GetType("System.String"));
//			_typeTable.Columns.Add(ColType);
//			_typeTable.Rows.Add(new object[] { "Column"});
//			_typeTable.Rows.Add(new object[] { "Bar"});
//			_typeTable.Rows.Add(new object[] { "Line"});
//			_typeTable.Rows.Add(new object[] { "Pie"});
//			_typeTable.Rows.Add(new object[] { "3D Column Chart"});
//			cbxChartType.DataSource = _typeTable;
//			cbxChartType.DisplayMember = "Type";
//
//			DataTable legendLocTable  = MIDEnvironment.CreateDataTable();
//			DataColumn legendColType = new DataColumn("Location", Type.GetType("System.String"));
//			legendLocTable.Columns.Add(legendColType);
//			legendLocTable.Rows.Add(new object[] { "Right"});
//			legendLocTable.Rows.Add(new object[] { "Left"});
//			legendLocTable.Rows.Add(new object[] { "Top"});
//			legendLocTable.Rows.Add(new object[] { "Bottom"});
//			legendLocTable.Rows.Add(new object[] { "None"});
//			cbxLegendLoc.DataSource = legendLocTable;
//			cbxLegendLoc.DisplayMember = "Location";
//					
//			ChainTableLoad();
//			StoreTableLoad();
//			PieTableLoad();
//			LineTableLoad();
//			// Add in the DataGrid
//
//			this.ultraChart1.Axis.Y.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//			this.ultraChart1.Axis.Y.TickmarkInterval=10000;
//			_chartType = ChartTypeEnum.column;
//			_chainData = true;
//			DisplayData();
//
//			ultraChart1.Legend.Visible=true;
//			ultraChart1.Legend.SpanPercentage = 15;
//
//		}
//
//		private void ChainTableLoad()
//		{
//
//			_chainTable  = MIDEnvironment.CreateDataTable();
//			DataColumn Col1 = new DataColumn("Class", Type.GetType("System.String"));
//			DataColumn Col2 = new DataColumn("Sales", Type.GetType("System.Int32"));
//			DataColumn Col3 = new DataColumn("Stock", Type.GetType("System.Int32"));
//
//			_chainTable.Columns.Add(Col1);
//			_chainTable.Columns.Add(Col2);
//			_chainTable.Columns.Add(Col3);
//
//			DataRow aRow = _chainTable.NewRow();
//
//			aRow[0] = "Collection";
//			aRow[1] = 70599;
//			aRow[2] = 95212;
//			_chainTable.Rows.Add(aRow);
//
//			aRow = _chainTable.NewRow();
//			aRow[0] = "Denim";
//			aRow[1] = 60145;
//			aRow[2] = 84139;
//			_chainTable.Rows.Add(aRow);
//
//			aRow = _chainTable.NewRow();
//			aRow[0] = "Bottoms";
//			aRow[1] = 61662;
//			aRow[2] = 88948;
//			_chainTable.Rows.Add(aRow);
//
//			aRow = _chainTable.NewRow();
//			aRow[0] = "Tops";
//			aRow[1] = 49694;
//			aRow[2] = 75258;
//			_chainTable.Rows.Add(aRow);
//
//			aRow = _chainTable.NewRow();
//			aRow[0] = "Sweaters";
//			aRow[1] = 35089;
//			aRow[2] = 58678;
//			_chainTable.Rows.Add(aRow);
//
//		}
//
//		private void StoreTableLoad()
//		{
//
//			_storeTable  = MIDEnvironment.CreateDataTable();
//			DataColumn Col1 = new DataColumn("Group", Type.GetType("System.String"));
//			DataColumn Col2 = new DataColumn("Sales", Type.GetType("System.Int32"));
//			DataColumn Col3 = new DataColumn("Stock", Type.GetType("System.Int32"));
//
//			_storeTable.Columns.Add(Col1);
//			_storeTable.Columns.Add(Col2);
//			_storeTable.Columns.Add(Col3);
//
//			DataRow aRow = _storeTable.NewRow();
//
//			aRow[0] = "NorthEast";
//			aRow[1] = 3249;
//			aRow[2] = 2719;
//			_storeTable.Rows.Add(aRow);
//
//			aRow = _storeTable.NewRow();
//			aRow[0] = "SouthEast";
//			aRow[1] = 4888;
//			aRow[2] = 5277;
//			_storeTable.Rows.Add(aRow);
//
//			aRow = _storeTable.NewRow();
//			aRow[0] = "NorthWest";
//			aRow[1] = 4278;
//			aRow[2] = 3876;
//			_storeTable.Rows.Add(aRow);
//
//			aRow = _storeTable.NewRow();
//			aRow[0] = "SouthWest";
//			aRow[1] = 3408;
//			aRow[2] = 3579;
//			_storeTable.Rows.Add(aRow);
//
//
//		}
//
//		private void LineTableLoad()
//		{
//
//			_lineChainTable  = MIDEnvironment.CreateDataTable();
//			DataColumn Col1 = new DataColumn("Group", Type.GetType("System.String"));
//			DataColumn Col2 = new DataColumn("Wk 29, 2003", Type.GetType("System.Int32"));
//			DataColumn Col3 = new DataColumn("Wk 30, 2003", Type.GetType("System.Int32"));
//			DataColumn Col4 = new DataColumn("Wk 31, 2003", Type.GetType("System.Int32"));
//			DataColumn Col5 = new DataColumn("Wk 32, 2003", Type.GetType("System.Int32"));
//			DataColumn Col6 = new DataColumn("Wk 33, 2003", Type.GetType("System.Int32"));
//
//			_lineChainTable.Columns.Add(Col1);
//			_lineChainTable.Columns.Add(Col2);
//			_lineChainTable.Columns.Add(Col3);
//			_lineChainTable.Columns.Add(Col4);
//			_lineChainTable.Columns.Add(Col5);
//			_lineChainTable.Columns.Add(Col6);
//
//			DataRow aRow = _lineChainTable.NewRow();
//
//			aRow[0] = "Collection";
//			aRow[1] = 36011;
//			aRow[2] = 34587;
//			aRow[3] = 33487;
//			aRow[4] = 35765;
//			aRow[5] = 35432;
//			_lineChainTable.Rows.Add(aRow);
//
//			aRow = _lineChainTable.NewRow();
//			aRow[0] = "Denim";
//			aRow[1] = 29917;
//			aRow[2] = 31128;
//			aRow[3] = 32675;
//			aRow[4] = 31456;
//			aRow[5] = 32456;
//			_lineChainTable.Rows.Add(aRow);
//
//			aRow = _lineChainTable.NewRow();
//			aRow[0] = "Bottoms";
//			aRow[1] = 28887;
//			aRow[2] = 32775;
//			aRow[3] = 33456;
//			aRow[4] = 34567;
//			aRow[5] = 32456;
//			_lineChainTable.Rows.Add(aRow);
//
//			aRow = _lineChainTable.NewRow();
//			aRow[0] = "Tops";
//			aRow[1] = 24664;
//			aRow[2] = 25031;
//			aRow[3] = 26432;
//			aRow[4] = 27543;
//			aRow[5] = 25432;
//			_lineChainTable.Rows.Add(aRow);
//
//			aRow = _lineChainTable.NewRow();
//			aRow[0] = "Sweaters";
//			aRow[1] = 15471;
//			aRow[2] = 19618;
//			aRow[3] = 20111;
//			aRow[4] = 19876;
//			aRow[5] = 19231;
//			_lineChainTable.Rows.Add(aRow);
//
//			_lineChainTable  = MIDEnvironment.CreateDataTable();
//			DataColumn bCol1 = new DataColumn("Group", Type.GetType("System.String"));
//			DataColumn bCol2 = new DataColumn("Wk 28, 2003", Type.GetType("System.Int32"));
//			DataColumn bCol3 = new DataColumn("Wk 29, 2003", Type.GetType("System.Int32"));
//			DataColumn bCol4 = new DataColumn("Wk 30, 2003", Type.GetType("System.Int32"));
//			DataColumn bCol5 = new DataColumn("Wk 31, 2003", Type.GetType("System.Int32"));
//			DataColumn bCol6 = new DataColumn("Wk 32, 2003", Type.GetType("System.Int32"));
//
//			_lineStoreTable.Columns.Add(bCol1);
//			_lineStoreTable.Columns.Add(bCol2);
//			_lineStoreTable.Columns.Add(bCol3);
//			_lineStoreTable.Columns.Add(bCol4);
//			_lineStoreTable.Columns.Add(bCol5);
//			_lineStoreTable.Columns.Add(bCol6);
//
//			aRow = _lineStoreTable.NewRow();
//
//			aRow[0] = "NorthEast";
//			aRow[1] = 725;
//			aRow[2] = 2524;
//			aRow[3] = 2678;
//			aRow[4] = 2442;
//			aRow[5] = 2315;
//			_lineStoreTable.Rows.Add(aRow);
//
//			aRow = _lineStoreTable.NewRow();
//			aRow[0] = "SouthEast";
//			aRow[1] = 1388;
//			aRow[2] = 3588;
//			aRow[3] = 3725;
//			aRow[4] = 3815;
//			aRow[5] = 3627;
//			_lineStoreTable.Rows.Add(aRow);
//
//			aRow = _lineStoreTable.NewRow();
//			aRow[0] = "NorthWest";
//			aRow[1] = 4278;
//			aRow[2] = 4367;
//			aRow[3] = 4439;
//			aRow[4] = 4217;
//			aRow[5] = 4267;
//			_lineStoreTable.Rows.Add(aRow);
//
//			aRow = _lineStoreTable.NewRow();
//			aRow[0] = "SouthWest";
//			aRow[1] = 3408;
//			aRow[2] = 3389;
//			aRow[3] = 3409;
//			aRow[4] = 3590;
//			aRow[5] = 3276;
//			_lineStoreTable.Rows.Add(aRow);
//
//		}
//
//		private void PieTableLoad()
//		{
//
//			_pieChainTable  = MIDEnvironment.CreateDataTable();
//			DataColumn Col1 = new DataColumn("Element", Type.GetType("System.String"));
//			DataColumn Col2 = new DataColumn("Sales", Type.GetType("System.Int32"));
//
//			_pieChainTable.Columns.Add(Col1);
//			_pieChainTable.Columns.Add(Col2);
//
//			DataRow aRow = _pieChainTable.NewRow();
//			aRow[0] = "Collection";
//			aRow[1] = 70599;
//			_pieChainTable.Rows.Add(aRow);
//
//			aRow = _pieChainTable.NewRow();
//			aRow[0] = "Denim";
//			aRow[1] = 61045;
//			_pieChainTable.Rows.Add(aRow);
//
//			aRow = _pieChainTable.NewRow();
//			aRow[0] = "Bottoms";
//			aRow[1] = 61662;
//			_pieChainTable.Rows.Add(aRow);
//
//			aRow = _pieChainTable.NewRow();
//			aRow[0] = "Tops";
//			aRow[1] = 49694;
//			_pieChainTable.Rows.Add(aRow);
//
//			aRow = _pieChainTable.NewRow();
//			aRow[0] = "Sweaters";
//			aRow[1] = 35089;
//			_pieChainTable.Rows.Add(aRow);
//
//			_pieStoreTable  = MIDEnvironment.CreateDataTable();
//			DataColumn bCol1 = new DataColumn("Element", Type.GetType("System.String"));
//			DataColumn bCol2 = new DataColumn("Sales", Type.GetType("System.Int32"));
//
//			_pieStoreTable.Columns.Add(bCol1);
//			_pieStoreTable.Columns.Add(bCol2);
//
//			aRow = _pieStoreTable.NewRow();
//			aRow[0] = "NorthEast";
//			aRow[1] = 3249;
//			_pieStoreTable.Rows.Add(aRow);
//
//			aRow = _pieStoreTable.NewRow();
//			aRow[0] = "SouthEast";
//			aRow[1] = 4888;
//			_pieStoreTable.Rows.Add(aRow);
//
//			aRow = _pieStoreTable.NewRow();
//			aRow[0] = "NorthWest";
//			aRow[1] = 4278;
//			_pieStoreTable.Rows.Add(aRow);
//
//			aRow = _pieStoreTable.NewRow();
//			aRow[0] = "SouthWest";
//			aRow[1] = 3408;
//			_pieStoreTable.Rows.Add(aRow);
//		}
//
//
//		private void cbxChartType_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//			switch (cbxChartType.SelectedIndex)
//			{
//				case 0:
//					_chartType = ChartTypeEnum.column;
//					break;
//				case 1:
//					_chartType = ChartTypeEnum.bar;
//					break;
//				case 2:
//					_chartType = ChartTypeEnum.line;
//					break;
//				case 3:
//					_chartType = ChartTypeEnum.pie;
//					break;
//				case 4:
//					_chartType = ChartTypeEnum.threeDBar;
//					break;
//			}
//			DisplayData();
//		}
//
//		private void cbxChart_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//			switch (cbxChart.SelectedIndex)
//			{
//				case 0:
//					_chainData = true;
//					break;
//				case 1:
//					_chainData = false;
//					break;
//			}
//			DisplayData();
//		}
//
//		private void DisplayData()
//		{
//			switch (_chartType)
//			{
//				case ChartTypeEnum.column:
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart;
//					this.ultraChart1.Axis.Y.Labels.ItemFormatString="<DATA_VALUE:0>";
//					if (_chainData)
//					{
//						this.ultraChart1.Axis.Y.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.Y.TickmarkInterval=10000; 
//						this.ultraChart1.TitleTop.Text = "Sales/Stock by Class Chain Column Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _chainTable;
//						ultraChart1.Data.DataSource = _chainTable;
//					}
//					else
//					{
//						this.ultraChart1.Axis.Y.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.Y.TickmarkInterval=1000; 
//						this.ultraChart1.TitleTop.Text = "Sales/Stock by Store Group Store Column Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _storeTable;
//						ultraChart1.Data.DataSource = _storeTable;
//					}
//					break;
//				case ChartTypeEnum.bar:
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.BarChart;
//					this.ultraChart1.Axis.X.Labels.ItemFormatString="<DATA_VALUE:0>";
//					if (_chainData)
//					{
//						this.ultraChart1.Axis.X.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.X.TickmarkInterval=10000; 
//						this.ultraChart1.TitleTop.Text = "Sales/Stock by Class Chain Bar Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _chainTable;
//						ultraChart1.Data.DataSource = _chainTable;
//					}
//					else
//					{
//						this.ultraChart1.Axis.X.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.X.TickmarkInterval=1000; 
//						this.ultraChart1.TitleTop.Text = "Sales/Stock by Store Group Store Bar Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _storeTable;
//						ultraChart1.Data.DataSource = _storeTable;
//					}
//					break;
//				case ChartTypeEnum.line:
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//					if (_chainData)
//					{
//						this.ultraChart1.Axis.Y.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.Y.TickmarkInterval=10000; 
//						this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//						this.ultraChart1.Axis.Y.Labels.ItemFormatString="<DATA_VALUE:0>";
//						this.ultraChart1.TitleTop.Text = "Chain Sales Line Chart Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _lineChainTable;
//						ultraChart1.Data.DataSource = _lineChainTable;
//					}
//					else
//					{
//						this.ultraChart1.Axis.Y.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//						this.ultraChart1.Axis.Y.TickmarkInterval=1000; 
//						this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
//						this.ultraChart1.Axis.Y.Labels.ItemFormatString="<DATA_VALUE:0>";
//						this.ultraChart1.TitleTop.Text = "Store Sales Line Chart Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _lineStoreTable;
//						ultraChart1.Data.DataSource = _lineStoreTable;
//					}
//					break;
//				case ChartTypeEnum.pie:
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
//					this.ultraChart1.Axis.Y.Labels.ItemFormatString="<DATA_VALUE:0>";
//					if (_chainData)
//					{
//						this.ultraChart1.TitleTop.Text = "Sales by Class Pie Chart Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _pieChainTable;
//						ultraChart1.Data.DataSource = _pieChainTable;
//					}
//					else
//					{
//						this.ultraChart1.TitleTop.Text = "Sales by Store Group Pie Chart Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _pieStoreTable;
//						ultraChart1.Data.DataSource = _pieStoreTable;
//					}
//					break;
//				case ChartTypeEnum.threeDBar:
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart3D;
//					this.ultraChart1.Axis.Z.TickmarkStyle=Infragistics.UltraChart.Shared.Styles.AxisTickStyle.DataInterval; 
//					this.ultraChart1.Axis.Z.Labels.ItemFormatString="<DATA_VALUE:0>";
//					this.ultraChart1.Axis.Y.Labels.ItemFormatString="<ITEM_LABEL>";
//					this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart3D;
//					if (_chainData)
//					{
//						this.ultraChart1.Axis.Z.TickmarkInterval=10000;
//						this.ultraChart1.TitleTop.Text = "3D Chain Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _chainTable;
//						ultraChart1.Data.DataSource = _chainTable;
//					}
//					else
//					{
//						this.ultraChart1.Axis.Z.TickmarkInterval=1000;
//						this.ultraChart1.TitleTop.Text = "3D Store Sample for <TODAY_DATE>";
//						DataGrid1.DataSource = _storeTable;
//						ultraChart1.Data.DataSource = _storeTable;
//					}
//					break;
//			}
//			ultraChart1.Data.DataBind();
//			
//		}
//
//		private void cbxLegendLoc_SelectionChangeCommitted(object sender, System.EventArgs e)
//		{
//			switch (cbxLegendLoc.SelectedIndex)
//			{
//				case 0:
//					ultraChart1.Legend.Location=LegendLocation.Right;
//					ultraChart1.Legend.Visible=true;
//					break;
//				case 1:
//					ultraChart1.Legend.Location=LegendLocation.Left;
//					ultraChart1.Legend.Visible=true;
//					break;
//				case 2:
//					ultraChart1.Legend.Location=LegendLocation.Top;
//					ultraChart1.Legend.Visible=true;
//					break;
//				case 3:
//					ultraChart1.Legend.Location=LegendLocation.Bottom;
//					ultraChart1.Legend.Visible=true;
//					break;
//				case 4:
//					ultraChart1.Legend.Visible=false;
//					break;
//			}
//		
//		}
//
//	}
//}
