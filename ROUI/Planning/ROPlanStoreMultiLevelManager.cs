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
//using Infragistics.UltraChart.Shared.Styles; // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options

using Logility.ROWebSharedTypes;
using static Logility.ROWebSharedTypes.ROGridCellChange;

namespace Logility.ROUI
{

    public class ROPlanStoreMultiLevelManager : ROPlanStoreManager
    {
        override public PlanCubeGroup CubeGroup { get { return managerData.PlanCubeGroup; } }
        public ROPlanStoreMultiLevelManagerData managerData;
        private ROStoreMultiLevelViewData storeMultiLevelViewData;

        public ROPlanStoreMultiLevelManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB, aOpenParms)
        {

        }

        override public ROPlanViewData GetViewData { get { return storeMultiLevelViewData; } }  // Access view data as base for inheritance

        override public void InitializeData()
        {
            managerData = new ROPlanStoreMultiLevelManagerData(SAB, OpenParms);
            //managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).ProfileType, OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).Key);
            DateRangeProfile summaryDateRangeProfile = OpenParms.GetSummaryDateProfile(aSession: SAB.ClientServerSession);
            if (summaryDateRangeProfile != null)
            {
                managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(summaryDateRangeProfile.ProfileType, summaryDateRangeProfile.Key);
            }
            managerData._currentChainPlanProfile = OpenParms.ChainHLPlanProfile;
            managerData._commonWaferCoordinateList = new CubeWaferCoordinateList();
            managerData.Transaction = SAB.ApplicationServerSession.CreateTransaction(); //Create an App Server Transaction
            managerData.PlanCubeGroup = managerData.Transaction.CreateStoreMultiLevelPlanMaintCubeGroup();

            managerData.PlanCubeGroup.OpenCubeGroup(OpenParms); //Open the cubegroup

            managerData.weekProfileList = OpenParms.GetWeekProfileList(SAB.ClientServerSession);
            managerData._periodProfileList = SAB.ClientServerSession.Calendar.GetPeriodProfileList(OpenParms.DateRangeProfile.Key); //Retrieve Period ProfileList from Calendar
            managerData._variableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.Variable);
            managerData._quantityVariableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

            managerData.BuildBasisItems(OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, OpenParms.ChainHLPlanProfile.NodeProfile.Key, OpenParms.StoreHLPlanProfile.VersionProfile.Key), OpenParms.ChainHLPlanProfile.NodeProfile);

            //((StoreMultiLevelPlanMaintCubeGroup)managerData.PlanCubeGroup).GetReadOnlyFlags(out managerData._chainReadOnly);
            managerData.headerDesc = "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
            if (OpenParms.DateRangeProfile.Name == string.Empty)
            {
                managerData.timeTotalName = "Total";
            }
            else
            {
                managerData.timeTotalName = OpenParms.DateRangeProfile.Name;
            }

            SetViewAndOrientation(OpenParms.ViewRID, eGridOrientation.TimeOnColumn, StoreAttributeSetKey, StoreAttributeKey, FilterKey);

        }

        override public int StartingRowIndex { get { return storeMultiLevelViewData._extraColumns; } }
        override public int NumberOfRows { get { return storeMultiLevelViewData._extraColumns; } }
        override public int StartingColIndex { get { return storeMultiLevelViewData._extraColumns; } }
        override public int NumberOfColumns { get { return storeMultiLevelViewData._extraColumns; } }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            return storeMultiLevelViewData.SetPageCoordinates(iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns);
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            return storeMultiLevelViewData.GetColumnCoordinateMap();
        }

        override public void SetViewAndOrientation(int viewRID, eGridOrientation gridOrientation, int StoreAttributeSetKey, int StoreAttributeKey, int FilterKey)
        {
            OpenParms.ViewRID = viewRID;
            if (gridOrientation == eGridOrientation.TimeOnRow)
            {
                storeMultiLevelViewData = new ROStoreMultiLevelLadderViewData(OpenParms.ViewRID, ref managerData);
            }
            else
            {
                storeMultiLevelViewData = new ROStoreMultiLevelPeriodViewData(OpenParms.ViewRID, ref managerData);
            }

            Orientation = gridOrientation;
        }

        override public void UndoLastRecompute()
        {
            managerData.PlanCubeGroup.UndoLastRecompute();
        }

        override public FunctionSecurityProfile GetFunctionSecurityProfile()
        {
            return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastSingleLevelChain);
        }

        override public List<string> GetBasisMenuList()
        {
            return managerData.basisMenuList;
        }

        override public string GetTitleText()
        {
            return MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + OpenParms.ChainHLPlanProfile.NodeProfile.Text + " / " + OpenParms.ChainHLPlanProfile.VersionProfile.Description + " / " + OpenParms.DateRangeProfile.DisplayDate;
        }

        override public string GetExportTitleText()
        {
            return MIDText.GetTextOnly(eMIDTextCode.frm_OTSPlanReview) + " - " + OpenParms.ChainHLPlanProfile.NodeProfile.Text + " / " + OpenParms.ChainHLPlanProfile.VersionProfile.Description + " / " + OpenParms.DateRangeProfile.DisplayDate;
            //return "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
        }

        override public string GetHeaderDescription()
        {
            return managerData.headerDesc;
        }

        override public bool HasDisplayableVariables()
        {
            if (storeMultiLevelViewData._sortedVariableHeaders.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        override public void ShowHideBasis(string basisKey, int basisSequence, bool doShow)
        {
            foreach (RowColProfileHeader basisHeader in managerData._selectableBasisHeaders)
            {
                if (basisHeader.Name == basisKey && basisHeader.Sequence == basisSequence)
                {
                    basisHeader.IsDisplayed = doShow;
                    break;
                }
            }
        }
        override public ArrayList GetSelectableQuantityHeaders()
        {
            return storeMultiLevelViewData._selectableQuantityHeaders;
        }
        override public ArrayList GetSelectableVariableHeaders()
        {
            return storeMultiLevelViewData._selectableVariableHeaders;
        }
        override public ArrayList GetSelectablePeriodHeaders()
        {
            return storeMultiLevelViewData._selectablePeriodHeaders;
        }

        override public ArrayList GetVariableGroupings()
        {
            return managerData.Transaction.PlanComputations.PlanVariables.GetVariableGroupings();
        }

        public string GetTimeTotalName()
        {
            return managerData.timeTotalName;
        }

        public ArrayList GetSelectableVariableHeadersForChart()
        {
            return storeMultiLevelViewData.GetSelectedVariableHeadersForChart();
        }

        override public bool IsNewCellValueValid(int rowIndex, int columnIndex, object newValue)
        {
            bool isValid = false;
            double result;
            if (Double.TryParse(newValue.ToString(), out result) == true)
            {
                isValid = true;
            }
            return isValid;
        }
        override public bool IsCellValueNegative(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            if (storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] != null && storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].isValueNumeric)
            {
                Double dblValue = storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].Value;
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
        override public bool IsCellIneligible(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            return PlanCellFlagValues.isIneligible(storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].Flags);
        }
        override public bool IsCellLocked(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

            return ComputationCellFlagValues.isLocked(storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].Flags);
        }
        override public bool IsCellEditable(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            ComputationCellFlags cellFlags = storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].Flags;
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
        override public bool IsCellBasis(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (columnIndex < storeMultiLevelViewData._extraColumns)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData.GetRowCount())
            {
                return false;
            }

            return storeMultiLevelViewData.isRowBasis(columnIndex - storeMultiLevelViewData._extraColumns);  //swap row and column
        }

        override public void CellValueChanged(int rowIndex, int columnIndex, double newValue, eROCellAction eROCellActionParam, eDataType eDataType)
        {
            if (eROCellActionParam == eROCellAction.CellChanged)
            {
                //swap the column and row indexes
                storeMultiLevelViewData.managerData.PlanCubeGroup.SetCellValue(
                                                        managerData._commonWaferCoordinateList,
                                                        storeMultiLevelViewData.GetRowCubeWaferCoordinateList(rowIndex - storeMultiLevelViewData._extraColumns),
                                                        storeMultiLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex),
                                                        newValue,
                                                        managerData.UnitsScalingString,
                                                        managerData.DollarScalingString);

            }
            else
            {
                bool currentCellLockUnAction = eROCellActionParam == eROCellAction.Lock ? true : false;
                LockUnlockCell(rowIndex, columnIndex, currentCellLockUnAction);
            }
        }

        override public void RecomputePlanCubes()
        {
            storeMultiLevelViewData.managerData.PlanCubeGroup.RecomputeCubes(true);
        }

        override public void RebuildGridData()
        {
            storeMultiLevelViewData.RebuildGridData();
        }

        public DataSet GetChartDataset()
        {
            return storeMultiLevelViewData.CreateDatasetForChart();
        }
        override public DataSet ReconstructChartDataset(ArrayList alVariables)
        {
            return storeMultiLevelViewData.ReconstructDatasetForChart(alVariables);
        }

        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            return storeMultiLevelViewData.DoesDataSetContainInventoryUnitVariables();
        }
        override public string GetSalesUnitsVariableName()
        {
            return storeMultiLevelViewData.GetSalesUnitsVariableName();
        }
        override public string GetInventoryUnitsVariableName()
        {
            return storeMultiLevelViewData.GetInventoryUnitsVariableName();
        }

        override public ROData ReconstructPage()
        {
            return this.storeMultiLevelViewData.ReconstructPage();
        }

        public override ROCubeMetadata CunstructMetadata(ROCubeGetMetadataParams metadataParams)
        {
            return this.storeMultiLevelViewData.ConstructMetadata(metadataParams);
        }

        override public int iAddedColumnsCount { get { return storeMultiLevelViewData._extraColumns; } }

        override public void IncrementAddedColumnsCount(uint iColumnsAdded)
        {
            storeMultiLevelViewData._extraColumns += (int)iColumnsAdded;
        }

        public void PeriodChanged(bool selectYear, bool selectSeason, bool selectQuarter, bool selectMonth, bool selectWeek)
        {
            this.storeMultiLevelViewData.selectYear = selectYear;
            this.storeMultiLevelViewData.selectSeason = selectSeason;
            this.storeMultiLevelViewData.selectQuarter = selectQuarter;
            this.storeMultiLevelViewData.selectMonth = selectMonth;
            this.storeMultiLevelViewData.selectWeek = selectWeek;
            this.storeMultiLevelViewData.CreateSelectablePeriodHeaders();


            this.storeMultiLevelViewData._periodHeaderHash = this.storeMultiLevelViewData.CreatePeriodHash();
            this.storeMultiLevelViewData.BuildTimeHeaders();

        }
        override public bool ShowYears()
        {
            return storeMultiLevelViewData.selectYear;
        }
        override public bool ShowSeasons()
        {
            return storeMultiLevelViewData.selectSeason;
        }
        override public bool ShowQuarters()
        {
            return storeMultiLevelViewData.selectQuarter;
        }
        override public bool ShowMonths()
        {
            return storeMultiLevelViewData.selectMonth;
        }
        override public bool ShowWeeks()
        {
            return storeMultiLevelViewData.selectWeek;
        }
        public int GetInitialTableIndexForChart()
        {
            if (ShowYears())
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
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
            ComputationCellFlags cellFlags = storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex].Flags; //swap the column and row indexes
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
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }
            if (rowIndex >= storeMultiLevelViewData._cubeValues.GetLength(1))
            {
                return;
            }

            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                storeMultiLevelViewData.managerData.PlanCubeGroup.SetCellLockStatus(
                            managerData._commonWaferCoordinateList,
                            storeMultiLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - storeMultiLevelViewData._extraColumns), //swap the column and row indexes
                            storeMultiLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                            blnLock);

            }
        }
        public void LockUnlockCellCascade(int rowIndex, int columnIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeMultiLevelViewData._extraColumns || storeMultiLevelViewData._cubeValues[columnIndex - storeMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                storeMultiLevelViewData.managerData.PlanCubeGroup.SetCellRecursiveLockStatus(
                    managerData._commonWaferCoordinateList,
                    storeMultiLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - storeMultiLevelViewData._extraColumns), //swap the column and row indexes
                    storeMultiLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                    blnLock);

            }

        }
        public void LockUnlockColumn(int columnIndex, bool blnLock)
        {
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeMultiLevelViewData._extraColumns)
            {
                return;
            }
            // TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = storeMultiLevelViewData.GetRowCount() - 1;
            int maxRows = storeMultiLevelViewData.GetColumnCount() - 1;
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCell(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockColumnCascade(int columnIndex, bool blnLock)
        {
            if (storeMultiLevelViewData._cubeValues == null)
            {
                return;
            }

            if (columnIndex - storeMultiLevelViewData._extraColumns >= storeMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeMultiLevelViewData._extraColumns)
            {
                return;
            }
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = storeMultiLevelViewData.GetRowCount() - 1;
            int maxRows = storeMultiLevelViewData.GetColumnCount() - 1;
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
            //int maxColumns = storeMultiLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = storeMultiLevelViewData.GetRowCount() - 1;
            for (int i = storeMultiLevelViewData._extraColumns; i <= storeMultiLevelViewData._extraColumns + maxColumns; i++)
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
            //int maxColumns = storeMultiLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = storeMultiLevelViewData.GetRowCount() - 1;
            for (int i = storeMultiLevelViewData._extraColumns; i <= storeMultiLevelViewData._extraColumns + maxColumns; i++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCellCascade(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockSheet(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = storeMultiLevelViewData.GetColumnCount() - 1;
            //int maxRows = storeMultiLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = storeMultiLevelViewData.GetRowCount() - 1;
            int maxRows = storeMultiLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = storeMultiLevelViewData._extraColumns; columnIndex <= storeMultiLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            //int maxColumns = storeMultiLevelViewData.GetColumnCount() - 1;
            //int maxRows = storeMultiLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = storeMultiLevelViewData.GetRowCount() - 1;
            int maxRows = storeMultiLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = storeMultiLevelViewData._extraColumns; columnIndex <= storeMultiLevelViewData._extraColumns + maxColumns; columnIndex++)
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
        //public DataTable ScalingDollar_GetDataTable()
        //{
        //    DataTable dtDollarScaling = new DataTable("Dollar Scaling");
        //    dtDollarScaling.Columns.Add("TEXT_CODE", typeof(int));
        //    dtDollarScaling.Columns.Add("TEXT_VALUE", typeof(string));

        //    DataTable dtText = MIDText.GetTextType(eMIDTextType.eDollarScaling, eMIDTextOrderBy.TextValue);

        //    DataRow dr = dtDollarScaling.NewRow();
        //    dr["TEXT_CODE"] = (int)eDollarScaling.Ones;
        //    dr["TEXT_VALUE"] = "1";
        //    dtDollarScaling.Rows.Add(dr);

        //    foreach (DataRow row in dtText.Rows)
        //    {
        //        if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
        //        {
        //            DataRow dr2 = dtDollarScaling.NewRow();
        //            dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
        //            dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
        //            dtDollarScaling.Rows.Add(dr2);

        //        }
        //    }

        //    return dtDollarScaling;

        //}
        //public int ScalingDollar_GetDefaultValue()
        //{
        //    return (int)eDollarScaling.Ones;
        //}

        //public DataTable ScalingUnits_GetDataTable()
        //{
        //    DataTable dtUnitsScaling = new DataTable("Unit Scaling");
        //    dtUnitsScaling.Columns.Add("TEXT_CODE", typeof(int));
        //    dtUnitsScaling.Columns.Add("TEXT_VALUE", typeof(string));

        //    DataTable dtText = MIDText.GetTextType(eMIDTextType.eUnitScaling, eMIDTextOrderBy.TextValue);

        //    DataRow dr = dtUnitsScaling.NewRow();
        //    dr["TEXT_CODE"] = (int)eUnitScaling.Ones;
        //    dr["TEXT_VALUE"] = "1";
        //    dtUnitsScaling.Rows.Add(dr);

        //    foreach (DataRow row in dtText.Rows)
        //    {

        //        if (Convert.ToString(row["TEXT_VALUE"]) != string.Empty)
        //        {
        //            DataRow dr2 = dtUnitsScaling.NewRow();
        //            dr2["TEXT_CODE"] = Convert.ToInt32(row["TEXT_CODE"]);
        //            dr2["TEXT_VALUE"] = Convert.ToString(row["TEXT_VALUE"]);
        //            dtUnitsScaling.Rows.Add(dr2);
        //        }
        //    }

        //    return dtUnitsScaling;
        //}

        //public int ScalingUnits_GetDefaultValue()
        //{
        //    return (int)eUnitScaling.Ones;
        //}
        override public void ScalingDollarChanged(string scalingValue)
        {
            storeMultiLevelViewData.managerData.DollarScalingString = scalingValue;
            //storeMultiLevelViewData.UpdateGridDataSet();
        }
        override public void ScalingUnitsChanged(string scalingValue)
        {
            storeMultiLevelViewData.managerData.UnitsScalingString = scalingValue;
            //storeMultiLevelViewData.UpdateGridDataSet();
        }
        #endregion


    }
}
