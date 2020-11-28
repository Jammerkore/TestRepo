using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows.Controls;

namespace MIDRetail.Windows
{
    public partial class PlanChainLadderView :  MIDFormBase, IFormBase
    {
        private PlanChainLadderManager ladderManager;
        private bool _formLoading = true;
        private Theme _theme;
        private ThemeProperties _frmThemeProperties;
        private const string _windowName = "OTS Chain Ladder View";

        public PlanChainLadderView(SessionAddressBlock sab, PlanOpenParms aOpenParms)
            : base(sab)
        {
            InitializeComponent();

            this.ladderManager = new PlanChainLadderManager(sab, aOpenParms);
        }

        private void PlanChainLadderView_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor; // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options >>> unrelated
            this.ladderControl.ApplyEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ApplyEventHandler(HandleApply);
            this.ladderControl.ShowChartEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowChartEventHandler(HandleShowChart);
            this.ladderControl.ChooseVariablesEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseVariablesEventHandler(HandleChooseVariables);
            this.ladderControl.ChooseQuantityEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseQuantityEventHandler(HandleChooseQuantity);
           
            this.ladderControl.ViewChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ViewChangedEventHandler(HandleViewChanged);
            this.ladderControl.ShowHideBasisEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowHideBasisEventHandler(HandleShowHideBasis);
            this.ladderControl.PeriodChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.PeriodChangedEventHandler(HandlePeriodChanged);
            this.ladderControl.DollarScalingChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.DollarScalingChangedEventHandler(HandleDollarScalingChanged);
            this.ladderControl.UnitsScalingChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.UnitsScalingChangedEventHandler(HandleUnitsScalingChanged);

            this.ladderControl.ExpandAllPeriodsEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ExpandAllPeriodsEventHandler(HandleExpandAllPeriods);
            this.ladderControl.CollapseAllPeriodsEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.CollapseAllPeriodsEventHandler(HandleCollapseAllPeriods);
            this.ladderControl.FreezeColumnEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.FreezeColumnEventHandler(HandleFreezeColumn);
            this.ladderControl.LockColumnEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnEventHandler(HandleLockColumn);
            this.ladderControl.LockColumnCascadeEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnCascadeEventHandler(HandleLockColumnCascade);
            this.ladderControl.LockRowEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowEventHandler(HandleLockRow);
            this.ladderControl.LockRowCascadeEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowCascadeEventHandler(HandleLockRowCascade);
            this.ladderControl.LockCellEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellEventHandler(HandleLockCell);
            this.ladderControl.LockCellCascadeEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellCascadeEventHandler(HandleLockCellCascade);
            this.ladderControl.LockSheetEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetEventHandler(HandleLockSheet);
            this.ladderControl.LockSheetCascadeEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetCascadeEventHandler(HandleLockSheetCascade);
          
            this.ladderControl.GridExportAllRowsEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridExportAllRowsEventHandler(HandleGridExportAllRows);
            this.ladderControl.GridEmailAllRowsEvent += new MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridEmailAllRowsEventHandler(HandleGridEmailAllRows);



            //this.ladderControl.ladderGrid.SelectedCellChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderGridControl.SelectedCellChangedEventHandler(HandleActiveCellChanged);
            this.ladderControl.ladderGrid.CellValueChangedEvent += new MIDRetail.Windows.Controls.PlanChainLadderGridControl.CellValueChangedEventHandler(HandleCellValueChanged);
            this.ladderControl.ladderGrid.DoubleReturnKeyPressedEvent += new MIDRetail.Windows.Controls.PlanChainLadderGridControl.DoubleReturnKeyPressedEventHandler(HandleDoubleReturnKeyPressed);
           
            // MIDFormBase Load event sets default grid properties after the ladder grid is built; this next method overrides some of those defaults 
            this.ladderControl.ladderGrid.AddGridProperties();
            Cursor.Current = Cursors.Default;   // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options >>> unrelated
        }
         


        public void Initialize()
        {
            _theme = base.SAB.ClientServerSession.Theme;
            BuildMenu();
           
            base.FunctionSecurity = ladderManager.GetFunctionSecurityProfile(); //set security on the MIDFormBase - assuming single chain for this form
            base.BypassBaseClosingLogic = true;
            base.DisposeChildControlsOnClose = false;

            this.ladderControl.LoadViewList(ladderManager.GetViewListDataTable(), ladderManager.GetViewRID()); //load the View combobox 
            this.ladderControl.LoadDollarScalingList(ladderManager.ScalingDollar_GetDataTable(), ladderManager.ScalingDollar_GetDefaultValue());
            this.ladderControl.LoadUnitsScalingList(ladderManager.ScalingUnits_GetDataTable(), ladderManager.ScalingUnits_GetDefaultValue());
  



            ladderManager.InitializeData();
            this.ladderControl.ladderGrid.Initialize(new MIDRetail.Windows.Controls.PlanChainLadderGridControl.IsCellNewValueValidDelegate(ladderManager.IsNewCellValueValid), 
                                                     new IsChainLadderCellValueNegative(ladderManager.IsCellValueNegative),
                                                     new IsChainLadderCellIneligible(ladderManager.IsCellIneligible),
                                                     new IsChainLadderCellLocked(ladderManager.IsCellLocked),
                                                     new IsChainLadderCellEditable(ladderManager.IsCellEditable),
                                                     new IsChainLadderCellBasis(ladderManager.IsCellBasis),
                                                     //new GetCellDisplayFormattedValue(ladderManager.GetCellDisplayFormattedValue),
                                                     _windowName,
                                                     MIDRetail.Data.MIDText.GetText(eMIDTextCode.msg_pl_InvalidInput),
                                                     ladderManager.GetHeaderDescription(), 
                                                     ladderManager.GetTimeTotalName(), 
                                                     _theme);

            this.ladderControl.LoadBasisList(ladderManager.GetBasisMenuList());


            this.Text = ladderManager.GetTitleText(); // Set Title bar
     

            RebindGrid(ladderManager.GetInitialDataSetForView());
            ladderControl.SetPeriods(ladderManager.ShowYears(), ladderManager.ShowSeasons(), ladderManager.ShowQuarters(), ladderManager.ShowMonths(), ladderManager.ShowWeeks());

           

            _formLoading = false;
        }
        private void PlanChainLadderView_Closing(object sender, CancelEventArgs e)
        {

                if (ladderManager.CubeGroup.UserChanged)
                {
                    CheckChainSingleLevel(e);
                }

                if (!e.Cancel)
                {
                    ladderManager.CubeGroup.CloseCubeGroup();
                    ladderManager.CubeGroup.Dispose();
                }

        }

        private void CheckChainSingleLevel(CancelEventArgs e)
        {
            bool chainChanged;
            PlanSaveParms planSaveParms;
            DialogResult result;


                chainChanged = ((ChainPlanMaintCubeGroup)ladderManager.CubeGroup).hasChainCubeChanged();

                if (chainChanged)
                {
                    result = MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_SaveChain), _windowName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    switch (result)
                    {
                        case DialogResult.Yes:

                            try
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                ladderManager.RecomputePlanCubes();

                                planSaveParms = new PlanSaveParms();

                                planSaveParms.SaveChainHighLevel = true;
                                planSaveParms.ChainHighLevelNodeRID = ladderManager.OpenParms.ChainHLPlanProfile.NodeProfile.Key;
                                planSaveParms.ChainHighLevelVersionRID = ladderManager.OpenParms.ChainHLPlanProfile.VersionProfile.Key;
                                planSaveParms.ChainHighLevelDateRangeRID = ladderManager.OpenParms.DateRangeProfile.Key;
                                planSaveParms.SaveHighLevelAllStoreAsChain = false;

                                ladderManager.CubeGroup.SaveCubeGroup(planSaveParms);
                            }
                            catch (PlanInUseException)
                            {
                                e.Cancel = true;
                            }
                            catch (Exception exc)
                            {
                                string sTemp = exc.ToString();
                                e.Cancel = true;
                                throw;
                               
                            }
                            finally
                            {
                                Cursor.Current = Cursors.Default;
                            }

                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;

                        default:
                            break;
                    }
                }

        }

      

        private void CheckForDisplayableVariables()
        {
            if (ladderManager.HasDisplayableVariables() == false)
            {
                MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoDisplayableVariables), "No Displayable Variables", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void RebindGrid(DataSet ds)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderControl.ladderGrid.BindGrid(ds);
                this.ladderControl.ladderGrid.UpdateTotalDataset(ds);
            
                if (!_formLoading)
                {
                    this.ladderControl.ladderGrid.ApplyGridProperties();
                }
                //this.ladderControl.ladderGrid.SetColumnToolTips(this.ladderManager.GetBasisToolTips());
                this.CheckForDisplayableVariables();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleCellValueChanged(object sender, MIDRetail.Windows.Controls.PlanChainLadderGridControl.CellValueChangedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.CellValueChanged(e.rowIndex, e.columnIndex, e.newValue); 
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleShowChart(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowChartEventArgs e)
        {
            string salesUnitsVariableName = ladderManager.GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = ladderManager.GetInventoryUnitsVariableName();
            // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options; remove edits
            //if (this.ladderManager.DoesDataSetContainInventoryUnitVariables() == false)
            //{
            //    string msg = "This view does not contain the necessary sales and inventory unit variables.";
            //    msg += Environment.NewLine + "Sales Units variable: " + salesUnitsVariableName;
            //    msg += Environment.NewLine + "Inventory Units variable: " + inventoryUnitsVariableName;

            //    MessageBox.Show(msg, "No Unit Variables for Chart", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            ArrayList unitVariables = this.ladderManager.GetSelectableVariableHeadersForChart();
            if (unitVariables.Count == 0)
            {
                MessageBox.Show(MIDText.GetText(eMIDTextCode.msg_pl_ChartRequiresUnits), _windowName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            // End TT#1749-MD
            else            
            {
                // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
                //frmPlanChainLadderChart frm = new frmPlanChainLadderChart(base.SAB);
                //frm.chart.SetChartBinding(this.ladderManager.GetChartDataset(), salesUnitsVariableName, inventoryUnitsVariableName, ladderManager.GetInitialTableIndexForChart(), ladderManager.GetTitleText());
                frmPlanChainLadderChart frm = new frmPlanChainLadderChart(base.SAB, this.ladderManager);
                ladderManager.ChartTableIndex = ladderManager.GetInitialTableIndexForChart();
                frm.chart.SetChartBinding(this.ladderManager.GetChartDataset(), ladderManager.ChartTableIndex, ladderManager.GetTitleText(), ladderManager.ChartType);
                // End TT#1748-MD
                frm.chart.SetPeriods(ladderManager.ShowYears(), ladderManager.ShowSeasons(), ladderManager.ShowQuarters(), ladderManager.ShowMonths(), ladderManager.ShowWeeks());

                frm.MdiParent = base.MdiParent;
                frm.Show();
            }
        }

        private void HandleApply(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ApplyEventArgs e)
        {
            DoApply();
        }
        private void HandleDoubleReturnKeyPressed(object sender, MIDRetail.Windows.Controls.PlanChainLadderGridControl.DoubleReturnKeyPressedEventArgs e)
        {
            DoApply();
        }
        private void DoApply()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.RecomputePlanCubes();
                //Begin TT#3722 -jsobek -In chain ladder some variables not re-calculating upon a sales change
                ////RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());
                //this.ladderControl.ladderGrid.UpdateTotalDataset(this.ladderManager.GetGridDataset());
                //ladderControl.ladderGrid.ReformatRows();
                ////this.ladderControl.ladderGrid.AdjustGridColumns(false);
                //this.ladderControl.ladderGrid.ApplyGridProperties();
                RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());

                ladderControl.ladderGrid.ReformatRows();
                //End TT#3722 -jsobek -In chain ladder some variables not re-calculating upon a sales change
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        
        private void HandleChooseVariables(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseVariablesEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RowColChooser frm = new RowColChooser(ladderManager.GetSelectableVariableHeaders(), true, "Variable Chooser", true, ladderManager.GetVariableGroupings());

                if (frm.ShowDialog() == DialogResult.OK)
                {

                    //CreateSortedList(_selectableVariableHeaders, out _sortedVariableHeaders);
                    //ReformatRowsChanged(true);
                    //ladderManager.ReconstructCubeCoordinatesAndDataset();
                    RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());
                  
                    ladderControl.ladderGrid.ReformatRows();
                }

                frm.Dispose();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleChooseQuantity(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ChooseQuantityEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RowColChooser frm = new RowColChooser(ladderManager.GetSelectableQuantityHeaders(), false, "Quantity Chooser", false, null); //_selectableQuantityHeaders
 

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());

                    ladderControl.ladderGrid.ReformatRows();
                }

                frm.Dispose();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleDollarScalingChanged(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.DollarScalingChangedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ladderManager.ScalingDollarChanged(e.scalingValue);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleUnitsScalingChanged(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.UnitsScalingChangedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ladderManager.ScalingUnitsChanged(e.scalingValue);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleFreezeColumn(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.FreezeColumnEventArgs e)
        {
            this.ladderControl.ladderGrid.SetFreezeColumn(e.blnFreeze);
        }
        private void HandleExpandAllPeriods(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ExpandAllPeriodsEventArgs e)
        {
            this.ladderControl.ladderGrid.ExpandAllPeriods();
        }
        private void HandleCollapseAllPeriods(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.CollapseAllPeriodsEventArgs e)
        {
            this.ladderControl.ladderGrid.CollapseAllPeriods();
        }
       
        private void HandleLockColumn(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeColumnIndex != -1)
                {
                    this.ladderManager.LockUnlockColumn(cubeColumnIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleLockColumnCascade(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockColumnCascadeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeColumnIndex != -1)
                {
                    this.ladderManager.LockUnlockColumnCascade(cubeColumnIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleLockRow(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeRowIndex != -1)
                {
                    this.ladderManager.LockUnlockRow(cubeRowIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleLockRowCascade(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockRowCascadeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeRowIndex != -1)
                {
                    this.ladderManager.LockUnlockRowCascade(cubeRowIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleLockCell(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeRowIndex != -1 && cubeColumnIndex != -1)
                {
                    this.ladderManager.LockUnlockCell(cubeRowIndex, cubeColumnIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandleLockCellCascade(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockCellCascadeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                int cubeRowIndex;
                int cubeColumnIndex;
                this.ladderControl.ladderGrid.GetSelectedCellCubeRowAndColumnIndex(out cubeRowIndex, out cubeColumnIndex);
                if (cubeRowIndex != -1 && cubeColumnIndex != -1)
                {
                    this.ladderManager.LockUnlockCellCascade(cubeRowIndex, cubeColumnIndex, e.blnLock);
                    UpdateGrid();
                }
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleLockSheet(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.LockUnlockSheet(e.blnLock);
                UpdateGrid();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleLockSheetCascade(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.LockSheetCascadeEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.LockUnlockSheetCascade(e.blnLock);
                UpdateGrid();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void UpdateGrid()
        {
            this.ladderManager.UpdateGridDataset();
            this.ladderControl.ladderGrid.UpdateTotalDataset(this.ladderManager.GetGridDataset());
            this.ladderControl.ladderGrid.ReformatRows();
            //this.ladderControl.ladderGrid.AdjustGridColumns(false);
            this.ladderControl.ladderGrid.ApplyGridProperties();
        }

        private void HandleViewChanged(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ViewChangedEventArgs e)
        {
            if (!_formLoading)
            {
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    ladderManager.OpenParms.ViewRID = e.viewRID;
                    ladderManager.OpenParms.ViewName = e.viewName;
                    ladderManager.OpenParms.ViewUserID = base.SAB.ClientServerSession.UserRID;
                    RebindGrid(ladderManager.GetDataSetForView(e.viewRID));
                    ladderControl.ladderGrid.ReformatRows();
                    ladderControl.SetPeriods(ladderManager.ShowYears(), ladderManager.ShowSeasons(), ladderManager.ShowQuarters(), ladderManager.ShowMonths(), ladderManager.ShowWeeks());
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

        }
        private void HandleShowHideBasis(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.ShowHideBasisEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.ShowHideBasis(e.basisKey, e.basisSequence, e.doShow);
                RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());
                ladderControl.ladderGrid.ReformatRows();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void HandlePeriodChanged(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.PeriodChangedEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                this.ladderManager.PeriodChanged(e.showYears, e.showSeasons, e.showQuarters, e.showMonths, e.showWeeks);
                RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());
                ladderControl.ladderGrid.ReformatRows();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void HandleGridExportAllRows(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridExportAllRowsEventArgs e)
        {
            SharedRoutines.GridExport.ExportAllRowsToExcel(this.ladderControl.ladderGrid.grid, ladderManager.GetTimeTotalName(), "rows", ladderManager.GetExportTitleText());
        }

        private void HandleGridEmailAllRows(object sender, MIDRetail.Windows.Controls.PlanChainLadderToolbarControl.GridEmailAllRowsEventArgs e)
        {
            string exportFileName = ladderManager.GetTitleText();
            int shortIndex = exportFileName.IndexOf("/");
            if (shortIndex > 0)
                exportFileName = exportFileName.Substring(0, shortIndex - 1);

            exportFileName = exportFileName.Trim() + ".xls";
      
            //string subject = ladderManager.GetTitleText();
            //MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
            //DataTable dt = secAdmin.GetUser(base.SAB.ClientServerSession.UserRID);
            //if (dt.Rows.Count > 0)
            //{
            //    string userName = String.Empty;
            //    string userFullName = String.Empty;
            //    if (dt.Rows[0].IsNull("USER_NAME") == false)
            //    {
            //        userName = (string)dt.Rows[0]["USER_NAME"];
            //    }
            //    if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
            //    {
            //        userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
            //    }
            //    subject += " - " + userName + " (" + userFullName + ")";
            //}
            SharedRoutines.GridExport.EmailAllRows(SharedRoutines.GridExport.BuildEmailSubjectWithUserName(base.SAB, ladderManager.GetTitleText()), exportFileName, this.ladderControl.ladderGrid.grid, ladderManager.GetTimeTotalName(), "rows", ladderManager.GetExportTitleText());
        }

        //private void ShowEmailForm(System.Net.Mail.Attachment a)
        //{
        //    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        //    EmailMessageForm frm = new EmailMessageForm();
        //    frm.AddAttachment(a);
        //    string subject = ladderManager.GetTitleText();
        //    MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
        //    DataTable dt = secAdmin.GetUser(base.SAB.ClientServerSession.UserRID);
        //    if (dt.Rows.Count > 0)
        //    {
        //        string userName = String.Empty;
        //        string userFullName = String.Empty;
        //        if (dt.Rows[0].IsNull("USER_NAME") == false)
        //        {
        //            userName = (string)dt.Rows[0]["USER_NAME"];
        //        }
        //        if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
        //        {
        //            userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
        //        }
        //        subject += " - " + userName + " (" + userFullName + ")";
        //    }

        //    frm.SetDefaults("", "", "", subject, subject + Environment.NewLine + "Please see attached file.");
        //    frm.ShowDialog();

        //}

       
        
        override protected void utmMain_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            switch (e.Tool.Key)
            {
                case "btTheme":
                    Theme();
                    break;
                //Begin TT#3712 -jsobek -No Edit-Undo in Chain Ladder
                case "btUndo":
                    Undo();
                    break;
                //End TT#3712 -jsobek -No Edit-Undo in Chain Ladder
                default:
                    base.utmMain_ToolClick(sender, e);
                    break;
            } 
        }
        //Begin TT#3712 -jsobek -No Edit-Undo in Chain Ladder
        private void Undo()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                this.ladderManager.UndoLastRecompute();

                RebindGrid(ladderManager.ReconstructCubeCoordinatesAndDataset());
                ladderControl.ladderGrid.ReformatRows();
                //UpdateGrid();
                
            
                Cursor.Current = Cursors.Default;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        //End TT#3712 -jsobek -No Edit-Undo in Chain Ladder
        private void BuildMenu()
        {
            try
            {
                Infragistics.Win.UltraWinToolbars.ButtonTool btTheme;

                btTheme = new Infragistics.Win.UltraWinToolbars.ButtonTool("btTheme");
                btTheme.SharedProps.Caption = "T&heme";
                btTheme.SharedProps.Shortcut = Shortcut.CtrlH;
                btTheme.SharedProps.MergeOrder = 0;
                btTheme.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.ThemeImage);
                utmMain.Tools.Add(btTheme);
                ToolsMenuTool.Tools.Add(btTheme);

                //Begin TT#3712 -jsobek -No Edit-Undo in Chain Ladder
                //Add an Undo button to the Edit menu
                Infragistics.Win.UltraWinToolbars.ButtonTool btUndo;
                btUndo = new Infragistics.Win.UltraWinToolbars.ButtonTool("btUndo");
                btUndo.SharedProps.Caption = "&Undo";
                btUndo.SharedProps.Shortcut = Shortcut.CtrlZ;
                btUndo.SharedProps.MergeOrder = 0;
                btUndo.SharedProps.AppearancesSmall.Appearance.Image = MIDGraphics.ImageIndex(MIDGraphics.UndoImage);
                utmMain.Tools.Add(btUndo);
                EditMenuTool.Tools.Add(btUndo);
                EditMenuTool.Tools["btUndo"].InstanceProps.IsFirstInGroup = true;
                //End TT#3712 -jsobek -No Edit-Undo in Chain Ladder

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
     

        private void Theme()
        {
            _frmThemeProperties = new ThemeProperties(_theme);
            _frmThemeProperties.ApplyButtonClicked += new EventHandler(StylePropertiesOnChanged);
            _frmThemeProperties.StartPosition = FormStartPosition.CenterParent;

            if (_frmThemeProperties.ShowDialog() == DialogResult.OK)
            {
                StylePropertiesChanged();
            }
        }

        private void StylePropertiesOnChanged(object sender, System.EventArgs e)
        {
            StylePropertiesChanged();
        }

        private void StylePropertiesChanged()
        {
            _theme = _frmThemeProperties.CurrentTheme;
            base.SAB.ClientServerSession.Theme = _theme;
            this.ladderControl.ladderGrid.ApplyTheme(_theme);
            this.ladderControl.ladderGrid.SetElementsToTheme();
            this.ladderControl.ladderGrid.ReformatRows();
        }
    

        #region Methods to satisfy IFormBase

        override public void ICut()
        {
        }
        override public void ICopy()
        {
        }
        override public void IPaste()
        {
        }
        override public void IDelete()
        {
        }
        override public void IRefresh()
        {
        }

        private PlanViewSave _saveForm;
        private bool _doSaveAs = false;
        private FormWindowState _windowStateBeforeSave;
        override public void ISave()
        {
            _doSaveAs = false;
            doSave();
        }
        override public void ISaveAs()
        {
            _doSaveAs = true;
            doSave();
        }

        private void doSave()
        {
                ladderManager.RecomputePlanCubes();
                this.Enabled = false;
                _saveForm = new PlanViewSave(base.SAB, ladderManager.OpenParms, ladderManager.CubeGroup, ladderManager.GetSelectableVariableHeaders(), ladderManager.GetSelectableQuantityHeaders(), true, ladderManager.GetSelectablePeriodHeaders());
                _saveForm.OnPlanSaveClosingEventHandler += new PlanViewSave.PlanSaveClosingEventHandler(OnPlanSave);
                _windowStateBeforeSave = this.WindowState;
                if (_doSaveAs == true)
                {

                    this.WindowState = FormWindowState.Normal;  //set window state to Normal to allow the Save form to be display as Normal
                    this.Visible = false;
                    _saveForm.MdiParent = this.MdiParent;
                    // _saveForm.StartPosition = FormStartPosition.CenterScreen;
                    _saveForm.Show();  //must support drag and drop to allow selecting a different merchandise node
                }
                else
                {
                    _saveForm.DisableControlsForSave();  //do not allow changing of the date range of selecting a different merchandise node
                    _saveForm.ShowDialog();
                }
        }

        void OnPlanSave(object source, ePlanViewSaveResult closeResult)
        {
            _saveForm.OnPlanSaveClosingEventHandler -= new PlanViewSave.PlanSaveClosingEventHandler(OnPlanSave);

                if (closeResult == ePlanViewSaveResult.Save)
                {
                    if (_doSaveAs)
                    {
                        //Begin TT#763-MD -jsobek -Save View and Plan in Chain Ladder-> conflict when attempt to go back in, not releasing the user after the save view.
                        //Has nothing to do with saving views. Only occurs on SAVE AS, not save. Cube was not dequeueing.
                        ladderManager.CubeGroup.CloseCubeGroup();
                        ladderManager.CubeGroup.Dispose();
                        //End TT#763-MD -jsobek -Save View and Plan in Chain Ladder-> conflict when attempt to go back in, not releasing the user after the save view.
                        this.Close();
                    }
                    else
                    {
                        this.Enabled = true;
                    }
                }
                else
                {
                    if (WindowState != _windowStateBeforeSave)
                    {
                        this.WindowState = _windowStateBeforeSave;
                    }
                    this.Visible = true;
                    this.Enabled = true;
                }

        }

        #endregion


    }
}
