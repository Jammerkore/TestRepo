using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Collections;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using Infragistics.UltraChart.Shared.Styles; // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options

namespace MIDRetail.Windows
{
  
    public class PlanChainLadderManager
    {
        private PlanOpenParms _openParms;
        public PlanOpenParms OpenParms { get {return _openParms;} private set {} }
        private SessionAddressBlock _sab;
        
       
        public PlanCubeGroup CubeGroup { get { return managerData._planCubeGroup; } private set {} }
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        //private LadderManagerData managerData;
        public LadderManagerData managerData;
        private ChartType _chartType = ChartType.TreeMapChart;
        private int _chartTableIndex = 0;
        private bool _chartShowBasis= true;
        // End TT#1748-MD
        private LadderViewData ladderViewData;

        public PlanChainLadderManager(SessionAddressBlock sab, PlanOpenParms aOpenParms)
        {
            _sab = sab;
            _openParms = aOpenParms;
        }

        public void InitializeData()
        {
            managerData = new LadderManagerData();
            managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(_openParms.GetSummaryDateProfile(_sab.ClientServerSession).ProfileType, _openParms.GetSummaryDateProfile(_sab.ClientServerSession).Key);
            managerData._currentChainPlanProfile = _openParms.ChainHLPlanProfile;
            managerData._commonWaferCoordinateList = new CubeWaferCoordinateList();
            managerData._transaction = _sab.ApplicationServerSession.CreateTransaction(); //Create an App Server Transaction
            managerData._planCubeGroup = managerData._transaction.CreateChainPlanMaintCubeGroup(); //Create a StoreMaintCubeGroup

            managerData._planCubeGroup.OpenCubeGroup(_openParms); //Open the cubegroup

            managerData.weekProfileList = _openParms.GetWeekProfileList(_sab.ClientServerSession);
            managerData._periodProfileList = _sab.ClientServerSession.Calendar.GetPeriodProfileList(_openParms.DateRangeProfile.Key); //Retrieve Period ProfileList from Calendar
            managerData._variableProfileList = managerData._planCubeGroup.GetFilteredProfileList(eProfileType.Variable);
            managerData._quantityVariableProfileList = managerData._planCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

            managerData.BuildBasisItems(_openParms.GetBasisProfileList(managerData._planCubeGroup, _openParms.ChainHLPlanProfile.NodeProfile.Key, _openParms.StoreHLPlanProfile.VersionProfile.Key), _openParms.ChainHLPlanProfile.NodeProfile);
           
            ((ChainPlanMaintCubeGroup)managerData._planCubeGroup).GetReadOnlyFlags(out managerData._chainReadOnly);
            managerData.headerDesc = "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
            if (_openParms.DateRangeProfile.Name == string.Empty)
            {
                managerData.timeTotalName = "Total";
            }
            else
            {
                managerData.timeTotalName = _openParms.DateRangeProfile.Name;
            }
         


        }

        public DataSet GetInitialDataSetForView()
        {
            return GetDataSetForView(_openParms.ViewRID);
        }

        public DataSet GetDataSetForView(int viewRID)
        {
            _openParms.ViewRID = viewRID;
            //_openParms.ViewName = viewName;
            //_openParms.ViewUserID = viewUserRID;
            ladderViewData = new LadderViewData(_openParms.ViewRID, ref managerData);
            return ladderViewData.CreateDataSetFromLadderViewData();
        }
        //public List<BasisToolTip> GetBasisToolTips()
        //{
        //    return this.ladderViewData.basisToolTips;
        //}
        //Begin TT#3712 -jsobek -No Edit-Undo in Chain Ladder
        public void UndoLastRecompute()
        {
            managerData._planCubeGroup.UndoLastRecompute();
        }
        //End TT#3712 -jsobek -No Edit-Undo in Chain Ladder

        public FunctionSecurityProfile GetFunctionSecurityProfile()
        {
            return _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
        }
        public int GetViewRID()
        {
            return _openParms.ViewRID; 
        }
        public DataTable GetViewListDataTable()
        {
            //SecurityAdmin secAdmin = new SecurityAdmin(); //TT#827-MD -jsobek -Allocation Reviews Performance
            FunctionSecurityProfile viewUserSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
            FunctionSecurityProfile viewGlobalSecurity = _sab.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);
            ArrayList _viewUserRIDList = new ArrayList();

            _viewUserRIDList.Add(-1);

            if (viewUserSecurity.AllowView)
            {
                _viewUserRIDList.Add(_sab.ClientServerSession.UserRID);
            }

            if (viewGlobalSecurity.AllowView)
            {
                _viewUserRIDList.Add(Include.GlobalUserRID);
            }
            PlanViewData _viewDL = new PlanViewData();
            DataTable _dtView = _viewDL.PlanView_Read(_viewUserRIDList);

            _dtView.Columns.Add(new DataColumn("DISPLAY_ID", typeof(string)));

            foreach (DataRow row in _dtView.Rows)
            {
                int userRID = Convert.ToInt32(row["USER_RID"], CultureInfo.CurrentUICulture);
                if (userRID != Include.GlobalUserRID)
                {
                    //Begin TT#827-MD -jsobek -Allocation Reviews Performance
                    //row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + secAdmin.GetUserName(userRID) + ")";
                    row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                    //End TT#827-MD -jsobek -Allocation Reviews Performance
                }
                else
                {
                    row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                }
            }
            return _dtView;
        }
        public List<string> GetBasisMenuList()
        {
            return managerData.basisMenuList;
        }
        public string GetTitleText()
        {
            return MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate;
        }
        public string GetExportTitleText()
        {
            return MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + _openParms.ChainHLPlanProfile.NodeProfile.Text + " / " + _openParms.ChainHLPlanProfile.VersionProfile.Description + " / " + _openParms.DateRangeProfile.DisplayDate;
            //return "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
        }
        public string GetHeaderDescription()
        {
            return managerData.headerDesc;
        }
        public bool HasDisplayableVariables()
        {
            if (ladderViewData._sortedVariableHeaders.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ShowHideBasis(string basisKey, int basisSequence, bool doShow)
        {
            foreach (RowColProfileHeader basisHeader in managerData._selectableBasisHeaders)
            {
                if (basisHeader.Name == basisKey && basisHeader.Sequence == basisSequence)
                {
                    basisHeader.IsDisplayed = doShow;
                    break;
                }
            }

            //also update the tooltip list
            //foreach (BasisToolTip tip in this.ladderViewData.basisToolTips)
            //{
            //    if (tip.basisHeaderName == basisKey && tip.basisHeaderSequence == basisSequence)
            //    {
            //        tip.isDisplayed = doShow;
            //        break;
            //    }
            //}
     
          
        }
        public ArrayList GetSelectableQuantityHeaders()
        {
            return ladderViewData._selectableQuantityHeaders;
        }
        public ArrayList GetSelectableVariableHeaders()
        {
            return ladderViewData._selectableVariableHeaders;
        }
        public ArrayList GetSelectablePeriodHeaders()
        {
            return ladderViewData._selectablePeriodHeaders;
        }
       
        public ArrayList GetVariableGroupings()
        {
           return managerData._transaction.PlanComputations.PlanVariables.GetVariableGroupings();
        }

        public string GetTimeTotalName()
        {
            return managerData.timeTotalName;
        }
       
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        public ArrayList GetSelectableVariableHeadersForChart()
        {
            //return ladderViewData._selectableVariableHeadersForChart;
            return ladderViewData.GetSelectedVariableHeadersForChart();
        }

        public ChartType ChartType
        {
            get { return _chartType; }
            set { _chartType = value; }
        }

        public int ChartTableIndex
        {
            get { return _chartTableIndex; }
            set { _chartTableIndex = value; }
        }

        public bool ChartShowBasis
        {
            get { return _chartShowBasis; }
            set { _chartShowBasis = value; }
        }
        // End TT#1748-MD
        public bool IsNewCellValueValid(int rowIndex, int columnIndex, object newValue)
        {
            bool isValid = false;
            double result;
            if (Double.TryParse(newValue.ToString(), out result) == true)
            {
                isValid = true;
            }
            return isValid;
        }
        public bool IsCellValueNegative(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (ladderViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            if (ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] != null && ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].isValueNumeric)
            {
                Double dblValue = ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].Value;
                if (dblValue < 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public bool IsCellIneligible(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (ladderViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            return PlanCellFlagValues.isIneligible(ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].Flags);
        }
        public bool IsCellLocked(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (ladderViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

            return ComputationCellFlagValues.isLocked(ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].Flags);
        }
        public bool IsCellEditable(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (ladderViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            ComputationCellFlags cellFlags = ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].Flags;
            if (PlanCellFlagValues.isClosed(cellFlags) ||
                    ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                    ComputationCellFlagValues.isNull(cellFlags) ||
                    PlanCellFlagValues.isProtected(cellFlags) ||
                    ComputationCellFlagValues.isHidden(cellFlags) ||
                    ComputationCellFlagValues.isReadOnly(cellFlags))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsCellBasis(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (columnIndex < ladderViewData._extraColumns)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData.GetRowCount())
            {
                return false;
            }
          
            return ladderViewData.isRowBasis(columnIndex - ladderViewData._extraColumns);  //swap row and column
        }

        public void CellValueChanged(int rowIndex, int columnIndex, double newValue)
        {

            //CubeWaferCoordinateList colCoordList = new CubeWaferCoordinateList();
            //CubeWaferCoordinateList rowCoordList = new CubeWaferCoordinateList();
            //colCoordList = ladderViewData.GetRowCubeWaferCoordinateList(columnIndex - ladderViewData._extraColumns);
            //rowCoordList = ladderViewData.GetColumnCubeWaferCoordinateList(rowIndex);

            //CubeWafer aCubeWafer = new CubeWafer();
            //PlanWaferCell[,] planWaferCellTable = null;

            ////Fill CommonWaferCoordinateListGroup
            //aCubeWafer.CommonWaferCoordinateList = managerData._commonWaferCoordinateList;

            ////Fill ColWaferCoordinateListGroup
            //aCubeWafer.ColWaferCoordinateListGroup.Clear();
            //aCubeWafer.ColWaferCoordinateListGroup.Add(colCoordList);


            ////Fill RowWaferCoordinateListGroup
            //aCubeWafer.RowWaferCoordinateListGroup.Clear();
            //aCubeWafer.RowWaferCoordinateListGroup.Add(rowCoordList);


            //// Retreive array of values
            //planWaferCellTable = managerData._planCubeGroup.GetPlanWaferCellValues(aCubeWafer, managerData.unitsScalingString, managerData.dollarScalingString);





            //swap the column and row indexes
            ladderViewData.managerData._planCubeGroup.SetCellValue(
                                                    managerData._commonWaferCoordinateList,
                                                    ladderViewData.GetRowCubeWaferCoordinateList(columnIndex - ladderViewData._extraColumns),
                                                    ladderViewData.GetColumnCubeWaferCoordinateList(rowIndex),
                                                    newValue,
                                                    managerData.unitsScalingString,
                                                    managerData.dollarScalingString);
            this.ladderViewData.UpdateGridDataSet();
        }

       

        public void RecomputePlanCubes()
        {
                ladderViewData.managerData._planCubeGroup.RecomputeCubes(true);
                this.ladderViewData.UpdateGridDataSet();
        }
        public void UpdateGridDataset()
        {
            this.ladderViewData.UpdateGridDataSet();
        }
        public DataSet GetGridDataset()
        {
            return this.ladderViewData.gridDataSet;
        }
        public DataSet GetChartDataset()
        {
            //DataSet dsChart = new DataSet();
            //dsChart.Tables.Add("chartData");
            //dsChart.Tables[0].Columns.Add("TIME", typeof(DateTime));
            //dsChart.Tables[0].Columns.Add("ITEM_LABEL", typeof(string));
            //dsChart.Tables[0].Columns.Add("DATA", typeof(float));

        

            return ladderViewData.CreateDatasetForChart();
        }
        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        public DataSet ReconstructChartDataset(ArrayList alVariables)
        {
            return ladderViewData.ReconstructDatasetForChart(alVariables);
        }
        // End TT#1748-MD
        public bool DoesDataSetContainInventoryUnitVariables()
        {
            return ladderViewData.DoesDataSetContainInventoryUnitVariables();
        }
        public string GetSalesUnitsVariableName()
        {
            return ladderViewData.GetSalesUnitsVariableName();
        }
        public string GetInventoryUnitsVariableName()
        {
            return ladderViewData.GetInventoryUnitsVariableName();
        }
        public DataSet ReconstructCubeCoordinatesAndDataset()
        {
           return this.ladderViewData.ReconstructCubeCoordinatesAndDataset();
        }


        public void PeriodChanged(bool selectYear, bool selectSeason, bool selectQuarter, bool selectMonth, bool selectWeek)
        {
            this.ladderViewData.selectYear = selectYear;
            this.ladderViewData.selectSeason = selectSeason;
            this.ladderViewData.selectQuarter = selectQuarter;
            this.ladderViewData.selectMonth = selectMonth;
            this.ladderViewData.selectWeek = selectWeek;
            this.ladderViewData.CreateSelectablePeriodHeaders();


            this.ladderViewData._periodHeaderHash = this.ladderViewData.CreatePeriodHash();
            this.ladderViewData.BuildTimeHeaders();
     
        }
        public bool ShowYears()
        {
            return ladderViewData.selectYear;
        }
        public bool ShowSeasons()
        {
            return ladderViewData.selectSeason;
        }
        public bool ShowQuarters()
        {
            return ladderViewData.selectQuarter;
        }
        public bool ShowMonths()
        {
            return ladderViewData.selectMonth;
        }
        public bool ShowWeeks()
        {
            return ladderViewData.selectWeek;
        }
        public int GetInitialTableIndexForChart()
        {
            if(ShowYears())
            {
                return 0;
            }
            else if (ShowSeasons())
            {
                return 1;
            }
            else if (ShowQuarters())
            {
                return 2;
            }
            else if (ShowMonths())
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        #region "Locking"

        public bool CanLockCell(int rowIndex, int columnIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (ladderViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
            ComputationCellFlags cellFlags = ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex].Flags; //swap the column and row indexes
            if (!PlanCellFlagValues.isClosed(cellFlags) &&
                            !ComputationCellFlagValues.isDisplayOnly(cellFlags) &&
                            !PlanCellFlagValues.isIneligible(cellFlags) &&
                            !ComputationCellFlagValues.isNull(cellFlags) &&
                            !PlanCellFlagValues.isProtected(cellFlags) &&
                            !ComputationCellFlagValues.isHidden(cellFlags) &&
                            !ComputationCellFlagValues.isReadOnly(cellFlags) &&
                            ComputationCellFlagValues.isLocked(cellFlags) != blnLock)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void LockUnlockCell(int rowIndex, int columnIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }
            if (ladderViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return;
            }
            if (rowIndex >= ladderViewData._cubeValues.GetLength(1))
            {
                return;
            }

            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                ladderViewData.managerData._planCubeGroup.SetCellLockStatus(
                            managerData._commonWaferCoordinateList,
                            ladderViewData.GetRowCubeWaferCoordinateList(columnIndex - ladderViewData._extraColumns), //swap the column and row indexes
                            ladderViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                            blnLock);

            }
        }
        public void LockUnlockCellCascade(int rowIndex, int columnIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }
            if (ladderViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < ladderViewData._extraColumns || ladderViewData._cubeValues[columnIndex - ladderViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                ladderViewData.managerData._planCubeGroup.SetCellRecursiveLockStatus(
                    managerData._commonWaferCoordinateList,
                    ladderViewData.GetRowCubeWaferCoordinateList(columnIndex - ladderViewData._extraColumns), //swap the column and row indexes
                    ladderViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                    blnLock);

            }

        }
        public void LockUnlockColumn(int columnIndex, bool blnLock)
        {
            if (ladderViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < ladderViewData._extraColumns)
            {
                return;
            }
            // TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = ladderViewData.GetRowCount() - 1;
            int maxRows = ladderViewData.GetColumnCount() - 1;
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCell(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockColumnCascade(int columnIndex, bool blnLock)
        {
            if (ladderViewData._cubeValues == null)
            {
                return;
            }

            if (columnIndex - ladderViewData._extraColumns >= ladderViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < ladderViewData._extraColumns)
            {
                return;
            }
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = ladderViewData.GetRowCount() - 1;
            int maxRows = ladderViewData.GetColumnCount() - 1;
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCellCascade(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockRow(int rowIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }

            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = ladderViewData.GetColumnCount() - 1;
			//for (int i = 0; i <= maxColumns; i++)
            int maxColumns = ladderViewData.GetRowCount() - 1;
            for (int i = ladderViewData._extraColumns; i <= ladderViewData._extraColumns + maxColumns; i++)
			// End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCell(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockRowCascade(int rowIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }


            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = ladderViewData.GetColumnCount() - 1;
			//for (int i = 0; i <= maxColumns; i++)
            int maxColumns = ladderViewData.GetRowCount() - 1;
            for (int i = ladderViewData._extraColumns; i <= ladderViewData._extraColumns + maxColumns; i++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCellCascade(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockSheet(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = ladderViewData.GetColumnCount() - 1;
            //int maxRows = ladderViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = ladderViewData.GetRowCount() - 1;
            int maxRows = ladderViewData.GetColumnCount() - 1;
            for (int columnIndex = ladderViewData._extraColumns; columnIndex <= ladderViewData._extraColumns + maxColumns; columnIndex++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                for (int rowIndex = 0; rowIndex <= maxRows; rowIndex++)
                {
                    LockUnlockCell(rowIndex, columnIndex, blnLock);
                }
            }
        }
        public void LockUnlockSheetCascade(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = ladderViewData.GetColumnCount() - 1;
            //int maxRows = ladderViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = ladderViewData.GetRowCount() - 1;
            int maxRows = ladderViewData.GetColumnCount() - 1;
            for (int columnIndex = ladderViewData._extraColumns; columnIndex <= ladderViewData._extraColumns + maxColumns; columnIndex++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                for (int rowIndex = 0; rowIndex <= maxRows; rowIndex++)
                {
                    LockUnlockCellCascade(rowIndex, columnIndex, blnLock);
                }
            }
        }

        #endregion

        #region "Scaling"
        public DataTable ScalingDollar_GetDataTable()
        {
            DataTable dtDollarScaling = new DataTable();
            dtDollarScaling.Columns.Add("TEXT_CODE", typeof(int));
            dtDollarScaling.Columns.Add("TEXT_VALUE", typeof(string));

            DataTable dtText = MIDText.GetTextType(eMIDTextType.eDollarScaling, eMIDTextOrderBy.TextValue);

            DataRow dr = dtDollarScaling.NewRow();
            dr["TEXT_CODE"] = (int)eDollarScaling.Ones;
            dr["TEXT_VALUE"] = "1";
            dtDollarScaling.Rows.Add(dr);

            foreach (DataRow row in dtText.Rows)
            {
                if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
                {
                    DataRow dr2 = dtDollarScaling.NewRow();
                    dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
                    dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
                    dtDollarScaling.Rows.Add(dr2);

                }
            }

            return dtDollarScaling;

        }
        public int ScalingDollar_GetDefaultValue()
        {
            return (int)eDollarScaling.Ones;
        }

        public DataTable ScalingUnits_GetDataTable()
        {
            DataTable dtUnitsScaling = new DataTable();
            dtUnitsScaling.Columns.Add("TEXT_CODE", typeof(int));
            dtUnitsScaling.Columns.Add("TEXT_VALUE", typeof(string));

            DataTable dtText = MIDText.GetTextType(eMIDTextType.eUnitScaling, eMIDTextOrderBy.TextValue);

            DataRow dr = dtUnitsScaling.NewRow();
            dr["TEXT_CODE"] = (int)eUnitScaling.Ones;
            dr["TEXT_VALUE"] = "1";
            dtUnitsScaling.Rows.Add(dr);

            foreach (DataRow row in dtText.Rows)
            {

                if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
                {
                    DataRow dr2 = dtUnitsScaling.NewRow();
                    dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
                    dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
                    dtUnitsScaling.Rows.Add(dr2);
                }
            }

            return dtUnitsScaling;
        }

        public int ScalingUnits_GetDefaultValue()
        {
            return (int)eUnitScaling.Ones;
        }
        public void ScalingDollarChanged(string scalingValue)
        {
            ladderViewData.managerData.dollarScalingString = scalingValue;
            ladderViewData.UpdateGridDataSet();
        }
        public void ScalingUnitsChanged(string scalingValue)
        {
            ladderViewData.managerData.unitsScalingString = scalingValue;
            ladderViewData.UpdateGridDataSet();
        }
        #endregion

       
    }
}
