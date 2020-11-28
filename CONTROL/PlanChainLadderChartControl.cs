using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;       // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
using MIDRetail.DataCommon;     // End TT#1748-MD

using Infragistics.Win.UltraWinChart;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Shared.Styles;        // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options

namespace MIDRetail.Windows.Controls
{
    public partial class PlanChainLadderChartControl : UserControl
    {
        public PlanChainLadderChartControl()
        {
            InitializeComponent();
        }

        private DataSet chartDataSet;
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        //public void SetChartBinding(DataSet chartDataSet, string salesUnitsVariableName, string inventoryUnitsVariableName, int chartInitialTableIndex, string chartTitle)
        public void SetChartBinding(DataSet chartDataSet, int chartInitialTableIndex, string chartTitle, ChartType aChartType)
        // End TT#1748-MD
        {
            this.chartDataSet = chartDataSet;



            //this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.Office2007Style;

      

         

            //this.ultraChart1.ColorModel.Scaling = Infragistics.UltraChart.Shared.Styles.ColorScaling.Decreasing;
            // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
            if (aChartType == ChartType.TreeMapChart)
            {
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
            }
            else
            {
                this.ultraChart1.ChartType = aChartType;
            }
            // End TT#1748-MD

            //this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnLineChart;

            //this.ultraChart1.Data.RowLabelsColumn = 1;
            //this.ultraChart1.Data.UseRowLabelsColumn = true;
           
            this.ultraChart1.Legend.Visible = true;
            this.ultraChart1.TitleTop.Text = chartTitle;
            this.ultraChart1.TitleTop.HorizontalAlign = StringAlignment.Center;
            this.ultraChart1.TitleBottom.Text = chartTitle;
            this.ultraChart1.TitleBottom.HorizontalAlign = StringAlignment.Center;
            this.ultraChart1.TitleLeft.Text = chartTitle;
            this.ultraChart1.TitleRight.Text = chartTitle;

            this.ultraChart1.TitleTop.Text = chartTitle;
            this.ultraChart1.TitleTop.Visible = true;
            this.ultraChart1.TitleBottom.Visible = false;
            this.ultraChart1.TitleLeft.Visible = false;
            this.ultraChart1.TitleRight.Visible = false;

            this.ultraChart1.Legend.Margins.Left = 5;
            this.ultraChart1.Legend.Margins.Right = 10;
            this.ultraChart1.Legend.Margins.Top = 15;
            this.ultraChart1.Legend.Margins.Bottom = 15;
            this.ultraChart1.Legend.SpanPercentage = 15;

            // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
            //this.ultraChart1.Data.SwapRowsAndColumns = true;
            if (this.ultraChart1.ChartType == Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
            }
            else
            {
                this.ultraChart1.Data.SwapRowsAndColumns = true;
            }
            // End TT#1748-MD
            
            this.ultraChart1.Tooltips.Format = Infragistics.UltraChart.Shared.Styles.TooltipStyle.LabelPlusDataValue;
           

           

           LineAppearance lineApp1 = new LineAppearance();
           //lineApp1.LineStyle.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
           //lineApp1.SplineTension = 0.5f;
           lineApp1.Thickness = 5;
           this.ultraChart1.LineChart.LineAppearances.Add(lineApp1);

           //LineAppearance lineApp2 = new LineAppearance();
           //lineApp2.LineStyle.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dot;
           //lineApp2.Thickness = 3;
           //lineApp2.IconAppearance.Icon = Infragistics.UltraChart.Shared.Styles.SymbolIcon.Circle;
           //lineApp2.IconAppearance.PE.Fill = Color.Green;
           //lineApp2.IconAppearance.PE.Stroke = Color.Green;
           //this.ultraChart1.LineChart.LineAppearances.Add(lineApp2);

           //LineAppearance lineApp3 = new LineAppearance();
           //lineApp3.LineStyle.DrawStyle = Infragistics.UltraChart.Shared.Styles.LineDrawStyle.Dash;
           //lineApp3.LineStyle.EndStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.ArrowAnchor;
           //lineApp3.LineStyle.MidPointAnchors = true;
           //lineApp3.LineStyle.StartStyle = Infragistics.UltraChart.Shared.Styles.LineCapStyle.SquareAnchor;
           //lineApp3.Thickness = 7;
           //this.ultraChart1.LineChart.LineAppearances.Add(lineApp3);
           this.ultraChart1.Data.ZeroAligned = true;


        
           SetChartAxisLabels();
           

            _settingPeriod = true;
            switch (chartInitialTableIndex)
            {
                case 0:
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = true;
                    break;
                case 1:
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = true;
                     break;
                case 2:
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = true;
                     break;
                case 3:
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = true;
                    break;
                case 4:
                    ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = true;
                    break;
            }
            _settingPeriod = false;
            SetChartDataSourceTable(chartInitialTableIndex);
        }
        private void SetChartDataSourceTable(int chartTableIndex)
        {
            this.ultraChart1.DataSource = chartDataSet.Tables[chartTableIndex];
        }

        private bool _settingPeriod = false;
        public void SetPeriods(bool showYears, bool showSeasons, bool showQuarters, bool showMonths, bool showWeeks)
        {
            _settingPeriod = true;
            
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).SharedProps.Visible = showYears;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).SharedProps.Visible = showSeasons;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).SharedProps.Visible = showQuarters;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).SharedProps.Visible = showMonths;
            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).SharedProps.Visible = showWeeks;
            _settingPeriod = false;

        }
        private void ShowHideBasis(bool doShow)
        {
            for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                {
                    string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                    if (columnName.Contains("Basis") == true)
                    {
                        this.ultraChart1.Data.IncludeColumn(columnIndex, doShow);
                    }
                
                 }
            }
        }
        private void ShowHideSales(bool doShow)
        {
            for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                {
                    string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                    if (columnName.Contains("Sales") == true)
                    {
                        this.ultraChart1.Data.IncludeColumn(columnIndex, doShow);
                    }

                }
            }
        }
        private void ShowHideStock(bool doShow)
        {
            for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                {
                    string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                    if (columnName.Contains("Stock") == true)
                    {
                        this.ultraChart1.Data.IncludeColumn(columnIndex, doShow);
                    }

                }
            }
        }

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options>>> replaces ShowHideSales and ShowHideStock
        public void ShowHideVariables(ArrayList alVariables)
        {
            for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                {
                    string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                    if (columnName != "RowKeyDisplay")
                    {
                        foreach (RowColProfileHeader rcph in alVariables)
                        {
                            VariableProfile varProf = (VariableProfile)rcph.Profile;
                            if (columnName.Trim().StartsWith(varProf.VariableName.Trim()))  // basis
                            {
                                this.ultraChart1.Data.IncludeColumn(columnIndex, rcph.IsDisplayed);
                            }
                        }
                    }
                }
            }
        }
        // End TT#1748-MD
        private void utlraToolbarsManager1_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                //case "colorRandom":
                //    if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["colorRandom"]).Checked == true)
                //    {
                //        this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.LinearRange;
                //    }
                //    else
                //    {
                //        this.ultraChart1.ColorModel.ModelStyle = Infragistics.UltraChart.Shared.Styles.ColorModels.Office2007Style;
                //        this.ultraChart1.ColorModel.Scaling = Infragistics.UltraChart.Shared.Styles.ColorScaling.Oscillating;
                //    }
                   
                //    break;
                case "zeroAlignData":
                    this.ultraChart1.Data.ZeroAligned = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["zeroAlignData"]).Checked;
                    break;
                case "basisShow":
                    ShowHideBasis(((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["basisShow"]).Checked);
                    break;
                // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
                //case "salesShow":
                //    ShowHideSales(((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["salesShow"]).Checked);
                //    break;
                //case "stockShow":
                //    ShowHideStock(((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["stockShow"]).Checked);
                //    break;
                case "chooseVariables":
                    RaiseChooseVariablesEvent();
                    break;
                // End TT#1748-MD
                case "periodYears":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked == true)
                        {
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = false;
                        }

                        SetChartPeriod();
                        _settingPeriod = false;
                    }
                    break;
                case "periodSeasons":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked == true)
                        {
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = false;
                        }
                        SetChartPeriod();
                        _settingPeriod = false;
                    }
                    break;
                case "periodQuarters":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked == true)
                        {
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = false;
                        }
                        SetChartPeriod();
                        _settingPeriod = false;
                    }
                    break;
                case "periodMonths":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked == true)
                        {
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked = false;
                        }
                        SetChartPeriod();
                        _settingPeriod = false;
                    }
                    break;
                case "periodWeeks":
                    if (_settingPeriod == false)
                    {
                        _settingPeriod = true;
                        if (((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked == true)
                        {
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked = false;
                            ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked = false;
                        }
                        SetChartPeriod();
                        _settingPeriod = false;
                    }
                    break;

                #region "Chart Tools"
   
                case "chartLegendTop":
                    SetChartLegend();
                    break;

                case "chartLegendLeft":
                    SetChartLegend();
                    break;

                case "chartLegendRight":
                    SetChartLegend();
                    break;

                case "chartLegendBottom":
                    SetChartLegend();
                    break;

                case "chartTypeBar":
                    SetChartType();
                    break;
                case "chartTypeLine":
                    SetChartType();
                    break;
                case "chartTypePie":
                    SetChartType();
                    break;
                case "chartTypePyramid":
                    SetChartType();
                    break;
                case "chartTypeHistogram":
                    SetChartType();
                    break;
                case "chartShowLegend":
                    SetChartLegend();
                    break;
                case "chartTitleShowHide":
                    SetChartTitle();
                    break;
                case "chartTitleLocationTop":
                    SetChartTitle();
                    break;
                case "chartTitleLocationLeft":
                    SetChartTitle();
                    break;
                case "chartTitleLocationRight":
                    SetChartTitle();
                    break;
                case "chartTitleLocationBottom":
                    SetChartTitle();
                    break;
                case "chartExport":
                    ExportChart();
                    break;
                //case "chartShowRowLabels":
                //    this.ultraChart1.Data.UseRowLabelsColumn = !this.ultraChart1.Data.UseRowLabelsColumn;
                //    break;

                #endregion
                

            }
        }

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        public class ChooseVariablesEventArgs
        {
            public ChooseVariablesEventArgs() { }
        }
        public delegate void ChooseVariablesEventHandler(object sender, ChooseVariablesEventArgs e);
        public event ChooseVariablesEventHandler ChooseVariablesEvent;
        protected virtual void RaiseChooseVariablesEvent()
        {
            if (ChooseVariablesEvent != null)
                ChooseVariablesEvent(this, new ChooseVariablesEventArgs());
        }

        public class SetChartSelectionsEventArgs : EventArgs
        {
            public ChartType ChartType;
            public int ChartTableIndex;
            public bool ChartShowBasis;
            public SetChartSelectionsEventArgs(ChartType aChartType, int aChartTableIndex, bool aChartShowBasis) 
            {
                ChartType = aChartType;
                ChartTableIndex = aChartTableIndex;
                ChartShowBasis = aChartShowBasis;
            }
        }
        public delegate void SetChartSelectionsEventHandler(object sender, SetChartSelectionsEventArgs e);
        public event SetChartSelectionsEventHandler SetChartSelectionsEvent;
        protected virtual void RaiseSetChartSelectionsEvent(ChartType aChartType, int aChartIndex, bool aShowBasis)
        {
            if (SetChartSelectionsEvent != null)
                SetChartSelectionsEvent(this, new SetChartSelectionsEventArgs(aChartType, aChartIndex, aShowBasis));
        }
        // End TT#1748-MD

        private void SetChartPeriod()
        {
           // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
           //bool showYears = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked;
           //bool showSeasons = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked;
           //bool showQuarters = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked;
           //bool showMonths = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked;
           //bool showWeeks = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked;


           //int chartTableIndex;
           //if (showYears)
           //{
           //    chartTableIndex = 0;
           //}
           //else if (showSeasons)
           //{
           //    chartTableIndex = 1;
           //}
           //else if (showQuarters)
           //{
           //    chartTableIndex = 2;
           //}
           //else if (showMonths)
           //{
           //    chartTableIndex = 3;
           //}
           //else 
           //{
           //    chartTableIndex = 4;
           //}

            //SetChartDataSourceTable(chartTableIndex);

            SetChartDataSourceTable(GetChartTableIndex());
            //End TT#1748-MD
        }

        private int GetChartTableIndex()
        {
            bool showYears = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodYears"]).Checked;
            bool showSeasons = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodSeasons"]).Checked;
            bool showQuarters = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodQuarters"]).Checked;
            bool showMonths = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodMonths"]).Checked;
            bool showWeeks = ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["periodWeeks"]).Checked;

            int chartTableIndex;
            if (showYears)
            {
                chartTableIndex = 0;
            }
            else if (showSeasons)
            {
                chartTableIndex = 1;
            }
            else if (showQuarters)
            {
                chartTableIndex = 2;
            }
            else if (showMonths)
            {
                chartTableIndex = 3;
            }
            else
            {
                chartTableIndex = 4;
            }
            return chartTableIndex;
        }
        private void SetChartAxisLabels()
        {
            this.ultraChart1.Axis.X.Labels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
            this.ultraChart1.Axis.X.Labels.OrientationAngle = 60;
            this.ultraChart1.Axis.X.Labels.VerticalAlign = StringAlignment.Far;
            this.ultraChart1.Axis.X.Labels.HorizontalAlign = StringAlignment.Near;
            this.ultraChart1.Axis.X.Labels.Layout.Padding = 10;

            this.ultraChart1.Axis.X.Labels.SeriesLabels.Orientation = Infragistics.UltraChart.Shared.Styles.TextOrientation.Custom;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.OrientationAngle = 60;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.VerticalAlign = StringAlignment.Far;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.HorizontalAlign = StringAlignment.Center;
            this.ultraChart1.Axis.X.Labels.SeriesLabels.Layout.Padding = 25;
  

            this.ultraChart1.Axis.Y.Labels.ItemFormatString = "<DATA_VALUE:###,###,###,##0>";
        }

        private void SetChartTitle()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTitleShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleShowHide"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationTop"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationLeft"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationRight"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTitleLocationBottom"];

            this.ultraChart1.TitleTop.Visible = false;
            this.ultraChart1.TitleLeft.Visible = false;
            this.ultraChart1.TitleRight.Visible = false;
            this.ultraChart1.TitleBottom.Visible = false;

            if (sbTitleShow.Checked == true)
            {
                if (sbTop.Checked == true)
                {
                    this.ultraChart1.TitleTop.Visible = true;
                }
                else if (sbLeft.Checked == true)
                {
                    this.ultraChart1.TitleLeft.Visible = true;
                }
                else if (sbRight.Checked == true)
                {
                    this.ultraChart1.TitleRight.Visible = true;
                }
                else if (sbBottom.Checked == true)
                {
                    this.ultraChart1.TitleBottom.Visible = true;
                }
            }
        }
        private void SetChartLegend()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLegendShow = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartShowLegend"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbTop = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendTop"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLeft = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendLeft"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbRight = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendRight"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBottom = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartLegendBottom"];

            this.ultraChart1.Legend.Visible = false;

            if (sbLegendShow.Checked == true)
            {
                this.ultraChart1.Legend.Visible = true;
                if (sbTop.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Top;
                }
                else if (sbLeft.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Left;
                }
                else if (sbRight.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Right;
                }
                else if (sbBottom.Checked == true)
                {
                    this.ultraChart1.Legend.Location = Infragistics.UltraChart.Shared.Styles.LegendLocation.Bottom;
                }
            }
        }
     
      
        private void SetChartType()
        {
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbBar = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeBar"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbLine = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeLine"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPie = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePie"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbPyramid = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypePyramid"];
            Infragistics.Win.UltraWinToolbars.StateButtonTool sbHistogram = (Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["chartTypeHistogram"];


            this.ultraChart1.ResetAxis();
            if (sbBar.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = false;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.ColumnChart;

     
                this.ultraChart1.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.None;
        

            }
            else if (sbLine.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = true;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.LineChart;
                //this.ultraChart1.Axis.X.Labels.ItemFormatString = "";
                //this.ultraChart1.Axis.X2.Visible = false;
                this.ultraChart1.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.ItemLabel;
           
            }
            else if (sbPyramid.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = true;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PyramidChart;
            }
            else if (sbHistogram.Checked == true)
            {
                this.ultraChart1.Data.SwapRowsAndColumns = true;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.AreaChart;
                this.ultraChart1.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.ItemLabel;
           
            }
            else //Use Pie Chart as default
            {
                this.ultraChart1.Data.SwapRowsAndColumns = true;
                this.ultraChart1.ChartType = Infragistics.UltraChart.Shared.Styles.ChartType.PieChart;
                this.ultraChart1.Axis.X.Labels.ItemFormat = Infragistics.UltraChart.Shared.Styles.AxisItemLabelFormat.ItemLabel;
           
            }
            SetChartAxisLabels(); //reset labels after setting chart type

            SetChartSelections(); // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        }

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        private void SetChartSelections()
        {
            RaiseSetChartSelectionsEvent(this.ultraChart1.ChartType, GetChartTableIndex(), ((Infragistics.Win.UltraWinToolbars.StateButtonTool)this.ultraToolbarsManager1.Tools["basisShow"]).Checked); 
        }
        // End TT#1748-MD
         
        private void ExportChart()
        {

            string PDF_File = FindPDFSavePath();
            if (PDF_File != null)
            {
                Infragistics.Documents.Reports.Report.Report r = new Infragistics.Documents.Reports.Report.Report();

                Graphics g = r.AddSection().AddCanvas().CreateGraphics();
                ultraChart1.RenderPdfFriendlyGraphics(g);

                r.Publish(PDF_File, Infragistics.Documents.Reports.Report.FileFormat.PDF);
            }

        }
        private String FindPDFSavePath()
        {
            System.IO.Stream myStream;
            string myFilepath = null;
            try
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "PDF files (*.pdf)|*.pdf";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if ((myStream = saveFileDialog1.OpenFile()) != null)
                    {
                        myFilepath = saveFileDialog1.FileName;
                        myStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myFilepath;
        }
    }
}
