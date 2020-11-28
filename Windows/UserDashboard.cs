// Begin Track #5005 - JSmith - Explorer Organization
// Too many changes to mark.  Use difference tool for comparison.
// End Track #5005
using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using System.Timers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Windows.Controls;
using MIDRetail.DataCommon;
using MIDRetail.Data;


namespace MIDRetail.Windows
{
	/// <summary>
	/// This control is used to scroll through Merchandise
	/// available to the user.  The is done by dispaying
	/// a tree view control in which the user can navigate
	/// to find appropriate filters.
	/// </summary>
	public class UserDashboardExplorer : ExplorerBase
	{
        private const int FILL_SPACE = 150;

        private int sortColumn = -1;
        private string sortColumnName = string.Empty;
        private ArrayList _alActivityEntries = new ArrayList();
        eMIDMessageLevel _currentActivitySelectedLevel = eMIDMessageLevel.Information;
        eMIDMessageLevel _currentActivityLevel = eMIDMessageLevel.Information;
        private int _initialWidth = 0;
        private int _expandedWidth = 0;
        private int _initialInterval = 0;

        private ListView listView1;
        private Button button1;
        private ColumnHeader colTime;
        private ColumnHeader colModule;
        private ColumnHeader colMessageLevel;
        private ColumnHeader colMessage;
        private ColumnHeader colImage;
        private ColumnHeader colMessageDetails;
        private Label lblDetailMessageLevel;
        private ComboBox cboDetailMessageLevel;
        private Infragistics.Win.UltraWinChart.UltraChart ultraChart1;
        private CheckBox cbxShowChart;
        // Begin TT#335 - JSmith - Menu should say Remove and not Delete
        //private string sDelete = null;
        //private string sRemove = null;
        // End TT#335
        //private System.Windows.Forms.ImageList ilMerchandiseHierarchy;

        private System.Windows.Forms.ToolStripMenuItem cmiShowChart;
        private System.Windows.Forms.ToolStripMenuItem cmiAutoSizeColumns;
        private System.Windows.Forms.ToolStripMenuItem cmiClearMessages;

		private System.ComponentModel.IContainer components;

        public UserDashboardExplorer(SessionAddressBlock aSAB, ExplorerAddressBlock aEAB, Form aMainMDIForm)
			: base(aSAB, aEAB, aMainMDIForm)
		{
			aEAB.UserDashboard = this;
		}

		protected override void Dispose(bool disposing)
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Infragistics.UltraChart.Resources.Appearance.PaintElement paintElement1 = new Infragistics.UltraChart.Resources.Appearance.PaintElement();
            Infragistics.UltraChart.Resources.Appearance.GradientEffect gradientEffect1 = new Infragistics.UltraChart.Resources.Appearance.GradientEffect();
            Infragistics.UltraChart.Resources.Appearance.PieChartAppearance pieChartAppearance1 = new Infragistics.UltraChart.Resources.Appearance.PieChartAppearance();
            Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance chartTextAppearance1 = new Infragistics.UltraChart.Resources.Appearance.ChartTextAppearance();
            this.listView1 = new System.Windows.Forms.ListView();
            this.colImage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colModule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessageLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessageDetails = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.lblDetailMessageLevel = new System.Windows.Forms.Label();
            this.cboDetailMessageLevel = new System.Windows.Forms.ComboBox();
            this.ultraChart1 = new Infragistics.Win.UltraWinChart.UltraChart();
            this.cbxShowChart = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraChart1)).BeginInit();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colImage,
            this.colTime,
            this.colModule,
            this.colMessageLevel,
            this.colMessage,
            this.colMessageDetails});
            this.listView1.Location = new System.Drawing.Point(15, 20);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(832, 278);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            this.listView1.Resize += new System.EventHandler(this.listView1_Resize);
            // 
            // colImage
            // 
            this.colImage.Text = "";
            this.colImage.Width = 20;
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 125;
            // 
            // colModule
            // 
            this.colModule.Text = "Module";
            this.colModule.Width = 150;
            // 
            // colMessageLevel
            // 
            this.colMessageLevel.Text = "Message Level";
            this.colMessageLevel.Width = 125;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 175;
            // 
            // colMessageDetails
            // 
            this.colMessageDetails.Text = "Message Details";
            this.colMessageDetails.Width = 200;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(1062, 310);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Clear";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDetailMessageLevel
            // 
            this.lblDetailMessageLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDetailMessageLevel.Location = new System.Drawing.Point(12, 313);
            this.lblDetailMessageLevel.Name = "lblDetailMessageLevel";
            this.lblDetailMessageLevel.Size = new System.Drawing.Size(132, 23);
            this.lblDetailMessageLevel.TabIndex = 14;
            this.lblDetailMessageLevel.Text = "Highest Message Level:";
            this.lblDetailMessageLevel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboDetailMessageLevel
            // 
            this.cboDetailMessageLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboDetailMessageLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDetailMessageLevel.Location = new System.Drawing.Point(146, 311);
            this.cboDetailMessageLevel.Name = "cboDetailMessageLevel";
            this.cboDetailMessageLevel.Size = new System.Drawing.Size(128, 21);
            this.cboDetailMessageLevel.TabIndex = 13;
            this.cboDetailMessageLevel.SelectedIndexChanged += new System.EventHandler(this.cboDetailMessageLevel_SelectedIndexChanged);
            // 
//			'UltraChart' properties's serialization: Since 'ChartType' changes the way axes look,
//			'ChartType' must be persisted ahead of any Axes change made in design time.
//		
            this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            // 
            // ultraChart1
            // 
            this.ultraChart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ultraChart1.Axis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            paintElement1.ElementType = Infragistics.UltraChart.Shared.Styles.PaintElementType.None;
            paintElement1.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(220)))));
            this.ultraChart1.Axis.PE = paintElement1;
            this.ultraChart1.Axis.X.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.X.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.X.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.X.Labels.ItemFormatString = "<ITEM_LABEL>";
            this.ultraChart1.Axis.X.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.X.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.FormatString = "";
            this.ultraChart1.Axis.X.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.X.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.X.LineThickness = 1;
            this.ultraChart1.Axis.X.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.X.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.X.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.X.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.X.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.X.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.X.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.X.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.X.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.X.Visible = true;
            this.ultraChart1.Axis.X2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.X2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.X2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.X2.Labels.ItemFormatString = "";
            this.ultraChart1.Axis.X2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.X2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.FormatString = "";
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.X2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.X2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.X2.Labels.Visible = false;
            this.ultraChart1.Axis.X2.LineThickness = 1;
            this.ultraChart1.Axis.X2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.X2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.X2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.X2.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.X2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.X2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.X2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.X2.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.X2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.X2.Visible = false;
            this.ultraChart1.Axis.Y.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Y.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.Y.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:00.##>";
            this.ultraChart1.Axis.Y.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Y.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.FormatString = "";
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Y.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Y.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Y.LineThickness = 1;
            this.ultraChart1.Axis.Y.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Y.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.Y.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Y.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.Y.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Y.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.Y.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Y.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.Y.TickmarkInterval = 10D;
            this.ultraChart1.Axis.Y.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.Y.Visible = true;
            this.ultraChart1.Axis.Y2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Y2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.Y2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Y2.Labels.ItemFormatString = "";
            this.ultraChart1.Axis.Y2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Y2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.FormatString = "";
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Y2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Y2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Y2.Labels.Visible = false;
            this.ultraChart1.Axis.Y2.LineThickness = 1;
            this.ultraChart1.Axis.Y2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Y2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.Y2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Y2.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.Y2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Y2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.Y2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Y2.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.Y2.TickmarkInterval = 10D;
            this.ultraChart1.Axis.Y2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.Y2.Visible = false;
            this.ultraChart1.Axis.Z.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Z.Labels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.Z.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Z.Labels.ItemFormatString = "";
            this.ultraChart1.Axis.Z.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Z.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.FontColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Z.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Z.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Z.Labels.Visible = false;
            this.ultraChart1.Axis.Z.LineThickness = 1;
            this.ultraChart1.Axis.Z.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Z.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.Z.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Z.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.Z.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Z.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.Z.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Z.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.Z.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.Z.Visible = false;
            this.ultraChart1.Axis.Z2.Labels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Z2.Labels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.Z2.Labels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Z2.Labels.ItemFormatString = "";
            this.ultraChart1.Axis.Z2.Labels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Z2.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Font = new System.Drawing.Font("Verdana", 7F);
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.FontColor = System.Drawing.Color.Gray;
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.HorizontalAlign = System.Drawing.StringAlignment.Near;
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Layout.Behavior = Infragistics.UltraChart.Shared.Styles.AxisLabelLayoutBehaviors.Auto;
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Horizontal;
            this.ultraChart1.Axis.Z2.Labels.SeriesLabels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Z2.Labels.VerticalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.Axis.Z2.Labels.Visible = false;
            this.ultraChart1.Axis.Z2.LineThickness = 1;
            this.ultraChart1.Axis.Z2.MajorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Z2.MajorGridLines.Color = System.Drawing.Color.Gainsboro;
            this.ultraChart1.Axis.Z2.MajorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Z2.MajorGridLines.Visible = true;
            this.ultraChart1.Axis.Z2.MinorGridLines.AlphaLevel = ((byte)(255));
            this.ultraChart1.Axis.Z2.MinorGridLines.Color = System.Drawing.Color.LightGray;
            this.ultraChart1.Axis.Z2.MinorGridLines.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
            this.ultraChart1.Axis.Z2.MinorGridLines.Visible = false;
            this.ultraChart1.Axis.Z2.TickmarkStyle = Infragistics.UltraChart.Shared.Styles.AxisTickStyle.Smart;
            this.ultraChart1.Axis.Z2.Visible = false;
            this.ultraChart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ultraChart1.ColorModel.AlphaLevel = ((byte)(150));
            this.ultraChart1.ColorModel.ColorBegin = System.Drawing.Color.Pink;
            this.ultraChart1.ColorModel.ColorEnd = System.Drawing.Color.DarkRed;
            this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.ultraChart1.Effects.Effects.Add(gradientEffect1);
            this.ultraChart1.Location = new System.Drawing.Point(853, 20);
            this.ultraChart1.Name = "ultraChart1";
            chartTextAppearance1.ChartTextFont = new System.Drawing.Font("Arial", 7F);
            chartTextAppearance1.ClipText = false;
            chartTextAppearance1.Column = -2;
            chartTextAppearance1.ItemFormatString = "<DATA_VALUE:00.00>";
            chartTextAppearance1.Row = -2;
            chartTextAppearance1.Visible = true;
            pieChartAppearance1.ChartText.Add(chartTextAppearance1);
            this.ultraChart1.PieChart = pieChartAppearance1;
            this.ultraChart1.Size = new System.Drawing.Size(284, 278);
            this.ultraChart1.TabIndex = 15;
            this.ultraChart1.TitleBottom.HorizontalAlign = System.Drawing.StringAlignment.Center;
            this.ultraChart1.TitleBottom.Text = "Message Breakdown";
            this.ultraChart1.Tooltips.HighlightFillColor = System.Drawing.Color.DimGray;
            this.ultraChart1.Tooltips.HighlightOutlineColor = System.Drawing.Color.DarkGray;
            // 
            // cbxShowChart
            // 
            this.cbxShowChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxShowChart.AutoSize = true;
            this.cbxShowChart.Location = new System.Drawing.Point(966, 313);
            this.cbxShowChart.Name = "cbxShowChart";
            this.cbxShowChart.Size = new System.Drawing.Size(80, 17);
            this.cbxShowChart.TabIndex = 16;
            this.cbxShowChart.Text = "checkBox1";
            this.cbxShowChart.UseVisualStyleBackColor = true;
            this.cbxShowChart.CheckedChanged += new System.EventHandler(this.cbxShowChart_CheckedChanged);
            // 
            // UserDashboardExplorer
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.cbxShowChart);
            this.Controls.Add(this.ultraChart1);
            this.Controls.Add(this.lblDetailMessageLevel);
            this.Controls.Add(this.cboDetailMessageLevel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F);
            this.Name = "UserDashboardExplorer";
            this.Size = new System.Drawing.Size(1155, 344);
            this.Load += new System.EventHandler(this.UserDashboardExplorer_Load);
            this.Enter += new System.EventHandler(this.UserDashboard_Enter);
            this.Leave += new System.EventHandler(this.UserDashboard_Leave);
            this.Resize += new System.EventHandler(this.UserDashboardExplorer_Resize);
            this.Controls.SetChildIndex(this.listView1, 0);
            this.Controls.SetChildIndex(this.button1, 0);
            this.Controls.SetChildIndex(this.cboDetailMessageLevel, 0);
            this.Controls.SetChildIndex(this.lblDetailMessageLevel, 0);
            this.Controls.SetChildIndex(this.ultraChart1, 0);
            this.Controls.SetChildIndex(this.cbxShowChart, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ultraChart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void UserDashboardExplorer_Resize(object sender, EventArgs e)
        {
            // auto hide chart if explorer too narrow
            //Debug.WriteLine("Widths=" + this.Width + "," + ultraChart1.Width);
            if (this.Width - FILL_SPACE < ultraChart1.Width &&
                ultraChart1.Visible)
            {
                cmiShowChart_Click(this, new EventArgs());
                cmiShowChart.Visible = false;
            }
            else if (!cmiShowChart.Visible &&
                _alActivityEntries.Count > 0)
            {
                cmiShowChart.Visible = true;
            }

            _initialWidth = ultraChart1.Left - listView1.Left - _initialInterval;
            _expandedWidth = ultraChart1.Right - listView1.Left;
            //Debug.WriteLine("_initialWidth Coordinates=" + ultraChart1.Left + "," + listView1.Left + "," + _initialInterval);
            //Debug.WriteLine("_expandedWidth Coordinates=" + ultraChart1.Right + "," + listView1.Left);
            if (!ultraChart1.Visible)
            {
                listView1.Width = _expandedWidth;
            }
            else
            {
                listView1.Width = _initialWidth;
            }
        }

		#endregion

        protected override void InitializeExplorer()
        {
            try
            {
                base.InitializeExplorer();

                InitializeComponent();
                _initialInterval = ultraChart1.Left - listView1.Right;
                SetText();
                FormatListView();
                SAB.ClientServerSession.Audit.LogUserDashboardEvent.OnLogUserDashboardHandler += new LogUserDashboardEvent.LogUserDashboardEventHandler(LogUserDashboardEvent_OnLogUserDashboardHandler);
                SAB.ApplicationServerSession.Audit.LogUserDashboardEvent.OnLogUserDashboardHandler += new LogUserDashboardEvent.LogUserDashboardEventHandler(LogUserDashboardEvent_OnLogUserDashboardHandler);
                SetUpPieChart();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void SetUpPieChart()
        {
            this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.CustomLinear;
            this.ultraChart1.ColorModel.CustomPalette = new Color[] { Color.Green, Color.Blue, Color.Yellow, Color.OrangeRed, Color.Red }; 

            DataTable dt = new DataTable();
            dt.Columns.Add("Count", typeof(int));
            dt.Columns.Add("Type", typeof(string));
            dt.Rows.Add(new object[] { 0, "Information" });
            dt.Rows.Add(new object[] { 0, "Edit" });
            dt.Rows.Add(new object[] { 0, "Warning" });
            dt.Rows.Add(new object[] { 0, "Error" });
            dt.Rows.Add(new object[] { 0, "Severe" });
            this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
            this.ultraChart1.Data.RowLabelsColumn = 1;
            this.ultraChart1.Data.UseRowLabelsColumn = true;
            this.ultraChart1.Legend.Visible = true;
            this.ultraChart1.Data.DataSource = dt;
            this.ultraChart1.Data.DataBind();
            this.cbxShowChart.Checked = true;
        }


        void LogUserDashboardEvent_OnLogUserDashboardHandler(object source, LogUserDashboardEventArgs e)
        {
            _alActivityEntries.Add(e);
            AddItem(e);

            EAB.Explorer.SetActivityImage(e.MessageLevel, false);
        }

        private void AddItem (LogUserDashboardEventArgs e)
        {
            int row = 0;
            if (e.MessageLevel > _currentActivityLevel)
            {
                _currentActivityLevel = e.MessageLevel;
            }

            //if (!cbxShowChart.Visible)
            //{
            //    cbxShowChart.Visible = true;
            //}
            if (!cmiShowChart.Visible &&
                this.Width - FILL_SPACE > ultraChart1.Width)
            {
                cmiShowChart.Visible = true;
            }
            if (!cmiClearMessages.Visible)
            {
                cmiClearMessages.Visible = true;
            }
            

            //if (e.MessageLevel < _currentActivitySelectedLevel)
            //{
            //    return;
            //}

            int imageIndex;
            lock (_alActivityEntries.SyncRoot)
            {
                switch (e.MessageLevel)
                {
                    case eMIDMessageLevel.Severe:
                        imageIndex = MIDGraphics.ImageIndex(MIDGraphics.SmileyErrorImage);
                        row = 4;
                        break;
                    case eMIDMessageLevel.Error:
                        imageIndex = MIDGraphics.ImageIndex(MIDGraphics.SmileyErrorImage);
                        row = 3;
                        break;
                    case eMIDMessageLevel.Warning:
                        imageIndex = MIDGraphics.ImageIndex(MIDGraphics.SmileyWarningImage);
                        row = 2;
                        break;
                    case eMIDMessageLevel.Edit:
                        imageIndex = MIDGraphics.ImageIndex(MIDGraphics.SmileyWarningImage);
                        row = 1;
                        break;
                    default:
                        imageIndex = MIDGraphics.ImageIndex(MIDGraphics.SmileyOKImage);
                        row = 0;
                        break;
                }
                if (e.MessageCode != eMIDTextCode.Unassigned &&
                    e.Message == null)
                {
                    e.Message = MIDText.GetText(e.MessageCode);
                }
                if (e.MessageLevel >= _currentActivitySelectedLevel)
                {
                    AddDataItem(e.Time, e.Module, MIDText.GetTextOnly(Convert.ToInt32(e.MessageLevel)), e.Message, e.MessageDetails, imageIndex);
                }

                ((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] = (int)((DataTable)this.ultraChart1.Data.DataSource).Rows[row]["Count"] + 1;

                if (cmiAutoSizeColumns.Checked)
                {
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
        }

        private void FormatListView()
        {
            listView1.View = View.Details;
            listView1.SmallImageList = MIDGraphics.ImageList;
            listView1.ShowItemToolTips = true;

            listView1.AllowColumnReorder = false;
            listView1.Items.Clear();
            listView1.Visible = true;
            listView1.Sort();

            // sort decending by time
            listView1.Sorting = SortOrder.Descending;
            listView1.ListViewItemSorter = new ListViewItemComparer(1, listView1.Sorting);
        }

        private void AddDataItem(string aTime, string aModule, string aMessageLevel, string aMessage, string aMessageDetails, int imageIndex)
        {
            string[] str;
            str = new String[6];
            str[0] = string.Empty;
            str[1] = aTime;
            str[2] = aModule;
            str[3] = aMessageLevel;
            str[4] = aMessage;
            str[5] = aMessageDetails;

            ListViewItem listViewItem = new ListViewItem(str, imageIndex);
            listView1.Items.Add(listViewItem);
        }

        //------------------
        // Virtual overrides
        //------------------

        /// <summary>
        /// Virtual method that is called to initialize the ExplorerBase TreeView
        /// </summary>

        override protected void InitializeTreeView()
        {
            try
            {
                
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }


        /// <summary>
        /// Virtual method that is called to perform Form Load tasks
        /// </summary>

        override protected void ExplorerLoad()
        {
            this.ultraChart1.Visible = false;
            cbxShowChart.Checked = false;
            cbxShowChart.Visible = false;
            BuildContextmenu();
            cmiShowChart.Checked = false;
            cmiShowChart.Visible = false;
            cmiClearMessages.Checked = false;
            cmiClearMessages.Visible = false;
            cmiAutoSizeColumns.Visible = true;
            cmiAutoSizeColumns.Checked = true;
            button1.Visible = false;
        }

        /// <summary>
        /// Virtual method that is called to build the ExplorerBase TreeView
        /// </summary>

        override protected void BuildTreeView()
        {
            //TODO: Implement Base TreeView
        }

		/// <summary>
		/// Virtual method that is called to refresh the ExplorerBase TreeView
		/// </summary>

		override protected void RefreshTreeView()
		{
            //TODO: Implement Base TreeView
		}

        /// <summary>
        /// Virtual method that gets the text for the New Item menu item.
        /// </summary>
        /// <returns>
        /// The text for the New Item menu item.
        /// </returns>

        
        override protected void CustomizeActionMenu(MIDTreeNode aNode)
        {
            
        }




        private void SetText()
        {
            this.lblDetailMessageLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_AF_HighestDetailLevel);
            this.button1.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Button_Clear);
            this.colTime.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Time);
            this.colModule.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Module);
            this.colMessageLevel.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Level);
            this.colMessage.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Message);
            this.colMessageDetails.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Details);
            this.ultraChart1.TitleBottom.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Message_Breaddown);
            this.cbxShowChart.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Show_Chart);
        }


        private void BuildContextmenu()
        {
            cmiShowChart = new System.Windows.Forms.ToolStripMenuItem();
            cmiShowChart.Name = "cmiShowChart";
            cmiShowChart.Size = new System.Drawing.Size(195, 22);
            cmiShowChart.Text = "Show Chart";
            cmiShowChart.Image = MIDGraphics.GetImage(MIDGraphics.ChartImage);
            cmiShowChart.Click += new System.EventHandler(this.cmiShowChart_Click);
            AddContextMenuItem(cmiShowChart, eExplorerActionMenuItem.None, cmiShowChart.Name);

            cmiAutoSizeColumns = new System.Windows.Forms.ToolStripMenuItem();
            cmiAutoSizeColumns.Name = "cmiAutoSizeColumns";
            cmiAutoSizeColumns.Size = new System.Drawing.Size(195, 22);
            cmiAutoSizeColumns.Text = "Auto Size Columns";
            //cmiAutoSizeColumns.Image = MIDGraphics.GetImage(MIDGraphics.FindImage);
            cmiAutoSizeColumns.Click += new System.EventHandler(this.cmiAutoSizeColumns_Click);
            AddContextMenuItem(cmiAutoSizeColumns, eExplorerActionMenuItem.None, cmiAutoSizeColumns.Name);

            cmiClearMessages = new System.Windows.Forms.ToolStripMenuItem();
            cmiClearMessages.Name = "cmiClearMessages";
            cmiClearMessages.Size = new System.Drawing.Size(195, 22);
            cmiClearMessages.Text = "Clear Messages";
            //cmiClearMessages.Text = MIDText.GetTextOnly(eMIDTextCode.lbl_Search) + "...";
            cmiClearMessages.Image = MIDGraphics.GetImage(MIDGraphics.DeleteIcon);
            cmiClearMessages.Click += new EventHandler(cmiClearMessages_Click);
            AddContextMenuItem(cmiClearMessages, eExplorerActionMenuItem.None, cmiClearMessages.Name);
        }

        private void DisplayMessages(EditMsgs em)
        {
            MIDRetail.Windows.DisplayMessages.Show(em, SAB, MIDText.GetTextOnly(eMIDTextCode.menu_Tools_Activity_Monitor));
        }

		#region IFormBase Members

		#endregion

        private void UserDashboard_Enter(object sender, EventArgs e)
        {
            // make sure menu is set
            //HideMenuItem(this, eMIDMenuItem.EditClear);
            HideMenuItem(this, eMIDMenuItem.FileSave);
            HideMenuItem(this, eMIDMenuItem.FileSaveAs);
        }

        private void UserDashboard_Leave(object sender, EventArgs e)
        {
            // undo all menu changes
            ShowMenuItem(this, eMIDMenuItem.EditClear);
            ShowMenuItem(this, eMIDMenuItem.FileSave);
            ShowMenuItem(this, eMIDMenuItem.FileSaveAs);
            EnableMenuItem(this, eMIDMenuItem.EditDelete);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearMessages();
        }

        private void ClearMessages()
        {
            this.listView1.Items.Clear();
            _alActivityEntries.Clear();
            _alActivityEntries = null;
            _alActivityEntries = new ArrayList();
            EAB.Explorer.SetActivityImage(eMIDMessageLevel.Information, true);
            ClearChart();
            cbxShowChart.Checked = false;
            cbxShowChart.Visible = false;
            cmiShowChart.Checked = false;
            cmiShowChart.Visible = false;
            cmiClearMessages.Checked = false;
            cmiClearMessages.Visible = false;
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                // Determine whether the column is the same as the last column clicked.
                if (e.Column != sortColumn)
                {
                    // Set the sort column to the new column.
                    sortColumn = e.Column;
                    foreach (ColumnHeader colHeader in listView1.Columns)
                    {
                        if (colHeader.DisplayIndex == e.Column)
                        {
                            sortColumnName = colHeader.Name;
                        }
                    }
                    // Set the sort order to ascending by default.
                    listView1.Sorting = SortOrder.Ascending;
                }
                else
                {
                    // Determine what the last sort order was and change it.
                    if (listView1.Sorting == SortOrder.Ascending)
                        listView1.Sorting = SortOrder.Descending;
                    else
                        listView1.Sorting = SortOrder.Ascending;
                }

                // Call the sort method to manually sort.
                listView1.Sort();
                // Set the ListViewItemSorter property to a new ListViewItemComparer
                // object.
                listView1.ListViewItemSorter = new ListViewItemComparer(e.Column,
                    listView1.Sorting);
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}

        private void listView1_Resize(object sender, EventArgs e)
        {
            colMessageDetails.Width = listView1.Width - colTime.Width - colModule.Width - colMessageLevel.Width - colMessage.Width - 10;
        }

        private void UserDashboardExplorer_Load(object sender, EventArgs e)
        {
            DataTable dt = MIDText.GetTextType(eMIDTextType.eMIDMessageLevel, eMIDTextOrderBy.TextCode);
            foreach (DataRow dr in dt.Rows)
            {
                switch ((eMIDMessageLevel)(Convert.ToInt32(dr["TEXT_CODE"])))
                {
                    case eMIDMessageLevel.Information:
                    case eMIDMessageLevel.Edit:
                    case eMIDMessageLevel.Warning:
                    case eMIDMessageLevel.Error:
                    case eMIDMessageLevel.Severe:
                        break;
                    default:
                        dr.Delete();
                        break;
                }
            }
            cboDetailMessageLevel.DataSource = dt;
			cboDetailMessageLevel.ValueMember = "TEXT_CODE";
			cboDetailMessageLevel.DisplayMember = "TEXT_VALUE";

            cboDetailMessageLevel.SelectedValue = (int)eMIDMessageLevel.Edit;
        }

        private void cboDetailMessageLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView dvRow;
            if (cboDetailMessageLevel.SelectedItem != null)
            {
                dvRow = (DataRowView)cboDetailMessageLevel.SelectedItem;
                _currentActivitySelectedLevel = (eMIDMessageLevel) Convert.ToInt32(dvRow.Row["TEXT_CODE"]);

                RebuildList();
            }
        }

        private void RebuildList()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ClearChart();
                listView1.Items.Clear();
                foreach (LogUserDashboardEventArgs e in _alActivityEntries)
                {
                    AddItem(e);
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void cmiShowChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    if (!cmiShowChart.Checked)
                    {
                        cmiShowChart.Checked = true;
                        if (!this.ultraChart1.Visible)
                        {
                            this.ultraChart1.Visible = true;
                            listView1.Width = _initialWidth;
                        }
                    }
                    else
                    {
                        cmiShowChart.Checked = false;
                        this.ultraChart1.Visible = false;
                        listView1.Width = _expandedWidth;
                        if (cmiAutoSizeColumns.Checked)
                        {
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cbxShowChart_CheckedChanged(object sender, EventArgs e)
        {
            if (FormLoaded)
            {
                if (cbxShowChart.Checked)
                {
                    if (!this.ultraChart1.Visible)
                    {
                        this.ultraChart1.Visible = true;
                        listView1.Width = _initialWidth;
                    }
                }
                else
                {
                    this.ultraChart1.Visible = false;
                    listView1.Width = _expandedWidth;
                }
            }
        }

        private void cmiAutoSizeColumns_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    if (!cmiAutoSizeColumns.Checked)
                    {
                        cmiAutoSizeColumns.Checked = true;
                        if (_alActivityEntries.Count > 0)
                        {
                            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                        }
                    }
                    else
                    {
                        cmiAutoSizeColumns.Checked = false;
                    }
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void cmiClearMessages_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormLoaded)
                {
                    ClearMessages();
                }
            }
            catch (Exception exc)
            {
                HandleException(exc);
            }
        }

        private void ClearChart()
        {
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[0]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[1]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[2]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[3]["Count"] = 0;
            ((DataTable)this.ultraChart1.Data.DataSource).Rows[4]["Count"] = 0;
        }

        override public void IRefresh()
        {
            RebuildList();
        }
	}
}
