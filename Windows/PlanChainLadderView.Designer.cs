namespace MIDRetail.Windows
{
    partial class PlanChainLadderView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            this.ladderControl.ApplyEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ApplyEventHandler(HandleApply);
            this.ladderControl.ShowChartEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowChartEventHandler(HandleShowChart);
            this.ladderControl.ChooseVariablesEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseVariablesEventHandler(HandleChooseVariables);
            this.ladderControl.ChooseQuantityEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseQuantityEventHandler(HandleChooseQuantity);

            this.ladderControl.ViewChangedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ViewChangedEventHandler(HandleViewChanged);
            this.ladderControl.ShowHideBasisEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowHideBasisEventHandler(HandleShowHideBasis);
            this.ladderControl.PeriodChangedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.PeriodChangedEventHandler(HandlePeriodChanged);
            this.ladderControl.DollarScalingChangedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.DollarScalingChangedEventHandler(HandleDollarScalingChanged);
            this.ladderControl.UnitsScalingChangedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.UnitsScalingChangedEventHandler(HandleUnitsScalingChanged);

            this.ladderControl.ExpandAllPeriodsEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ExpandAllPeriodsEventHandler(HandleExpandAllPeriods);
            this.ladderControl.CollapseAllPeriodsEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.CollapseAllPeriodsEventHandler(HandleCollapseAllPeriods);
            this.ladderControl.FreezeColumnEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.FreezeColumnEventHandler(HandleFreezeColumn);
            this.ladderControl.LockColumnEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnEventHandler(HandleLockColumn);
            this.ladderControl.LockColumnCascadeEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnCascadeEventHandler(HandleLockColumnCascade);
            this.ladderControl.LockRowEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowEventHandler(HandleLockRow);
            this.ladderControl.LockRowCascadeEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowCascadeEventHandler(HandleLockRowCascade);
            this.ladderControl.LockCellEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellEventHandler(HandleLockCell);
            this.ladderControl.LockCellCascadeEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellCascadeEventHandler(HandleLockCellCascade);
            this.ladderControl.LockSheetEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetEventHandler(HandleLockSheet);
            this.ladderControl.LockSheetCascadeEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetCascadeEventHandler(HandleLockSheetCascade);

            this.ladderControl.GridExportAllRowsEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridExportAllRowsEventHandler(HandleGridExportAllRows);
            this.ladderControl.GridEmailAllRowsEvent -= new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridEmailAllRowsEventHandler(HandleGridEmailAllRows);

            this.ladderControl.ladderGrid.CellValueChangedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderGridControl.CellValueChangedEventHandler(HandleCellValueChanged);
            this.ladderControl.ladderGrid.DoubleReturnKeyPressedEvent -= new MIDRetail.Windows.Controls.PlanChainLadderGridControl.DoubleReturnKeyPressedEventHandler(HandleDoubleReturnKeyPressed);
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private MIDRetail.Windows.Controls.PlanChainLadderToolbarControl ladderControl;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ladderControl = new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl();
            this.SuspendLayout();

            // 
            // ladderControl
            // 
            this.ladderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ladderControl.Location = new System.Drawing.Point(0, 0);
            this.ladderControl.Name = "ladderControl";
            this.ladderControl.Size = new System.Drawing.Size(200, 200);
            this.ladderControl.TabIndex = 1; 
            // 
            // PlanChainLadderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 351);
            this.Controls.Add(this.ladderControl);
            this.Name = "PlanChainLadderView";
            this.Text = "OTS Chain Ladder View";
            this.Load += new System.EventHandler(this.PlanChainLadderView_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PlanChainLadderView_Closing);
            this.ResumeLayout(false);

        }

        #endregion
    }
}