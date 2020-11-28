using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
	/// <summary>
	/// Summary description for UserOptions.
	/// </summary>
	public class frmPlanChainLadderChart : MIDFormBase
    {
        public PlanChainLadderChartControl chart;
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        private ArrayList _variables;                     
        private ArrayList _variableGroupings;
        private PlanChainLadderManager _ladderManager;   
        // End TT#1748-MD  
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
         //public frmPlanChainLadderChart(SessionAddressBlock aSAB)
         //   : base(aSAB)
        public frmPlanChainLadderChart(SessionAddressBlock aSAB, PlanChainLadderManager aLadderManager)
            : base(aSAB)
        // End TT#1748-MD
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

            // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
            _ladderManager = aLadderManager;
            _variables = _ladderManager.GetSelectableVariableHeadersForChart();
            _variableGroupings = _ladderManager.GetVariableGroupings();
            this.chart.ChooseVariablesEvent += new MIDRetail.Windows.Controls.PlanChainLadderChartControl.ChooseVariablesEventHandler(HandleChooseVariables);
            this.chart.SetChartSelectionsEvent += new MIDRetail.Windows.Controls.PlanChainLadderChartControl.SetChartSelectionsEventHandler(HandleSetChartSelections);
            // Edn TT#1748-MD
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
                // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
                this.chart.ChooseVariablesEvent -= new MIDRetail.Windows.Controls.PlanChainLadderChartControl.ChooseVariablesEventHandler(HandleChooseVariables);
                // End TT#1748-MD				
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
            this.chart = new MIDRetail.Windows.Controls.PlanChainLadderChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).BeginInit();
            this.SuspendLayout();
            // 
            // utmMain
            // 
            this.utmMain.MenuSettings.ForceSerialization = true;
            this.utmMain.ToolbarSettings.ForceSerialization = true;
            // 
            // chart
            // 
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(700, 500);
            this.chart.TabIndex = 1;
            // 
            // frmPlanChainLadderChart
            // 
            this.AllowDragDrop = true;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(700, 500);
            this.Controls.Add(this.chart);
            this.Name = "frmPlanChainLadderChart";
            this.Text = "OTS Chain Ladder Chart";
            this.Controls.SetChildIndex(this.chart, 0);
            ((System.ComponentModel.ISupportInitialize)(this.utmMain)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        //public void SetChartValues(DataSet chartDataSet, string salesUnitsVariableName, string inventoryUnitsVariableName, int chartInitialTableIndex)
        //{
        //    this.chart.SetChartBinding(chartDataSet, salesUnitsVariableName, inventoryUnitsVariableName, chartInitialTableIndex);
        //}

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        private void HandleChooseVariables(object sender, MIDRetail.Windows.Controls.PlanChainLadderChartControl.ChooseVariablesEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RowColChooser frm = new RowColChooser(_variables, true, "Variable Chooser", true, _variableGroupings);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    DataSet ds = _ladderManager.ReconstructChartDataset(_variables);
                    this.chart.SetChartBinding(ds, _ladderManager.ChartTableIndex, _ladderManager.GetTitleText(), _ladderManager.ChartType);
                    this.chart.ShowHideVariables(_variables);
                }
                frm.Dispose();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleSetChartSelections(object sender, MIDRetail.Windows.Controls.PlanChainLadderChartControl.SetChartSelectionsEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _ladderManager.ChartType = e.ChartType;
                _ladderManager.ChartTableIndex = e.ChartTableIndex;
                _ladderManager.ChartShowBasis = e.ChartShowBasis;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        // End TT#1748-MD 
	}
}
