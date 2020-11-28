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

    public class ROPlanChainSingleLevelManager : ROPlanChainManager
    {
        override public PlanCubeGroup CubeGroup { get { return managerData.PlanCubeGroup; } }
        public ROPlanChainSingleLevelManagerData managerData;
        private ROChainSingleLevelViewData chainSingleLevelViewData;

        public ROPlanChainSingleLevelManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB, aOpenParms)
        {

        }        
        public ROChainSingleLevelViewData ChainSingleLevelViewData { get { return chainSingleLevelViewData; } }        

        override public void InitializeData()
        {
            managerData = new ROPlanChainSingleLevelManagerData(SAB, OpenParms);
            //managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).ProfileType, OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).Key);
            DateRangeProfile summaryDateRangeProfile = OpenParms.GetSummaryDateProfile(aSession: SAB.ClientServerSession);
            if (summaryDateRangeProfile != null)
            {
                managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(summaryDateRangeProfile.ProfileType, summaryDateRangeProfile.Key);
            }
            managerData._currentChainPlanProfile = OpenParms.ChainHLPlanProfile;
            managerData._commonWaferCoordinateList = new CubeWaferCoordinateList();
            managerData.Transaction = SAB.ApplicationServerSession.CreateTransaction(); //Create an App Server Transaction
            managerData.PlanCubeGroup = managerData.Transaction.CreateChainPlanMaintCubeGroup(); //Create a StoreMaintCubeGroup

            managerData.PlanCubeGroup.OpenCubeGroup(OpenParms); //Open the cubegroup

            managerData.weekProfileList = OpenParms.GetWeekProfileList(SAB.ClientServerSession);
            managerData._periodProfileList = SAB.ClientServerSession.Calendar.GetPeriodProfileList(OpenParms.DateRangeProfile.Key); //Retrieve Period ProfileList from Calendar
            managerData._variableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.Variable);
            managerData._quantityVariableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

            managerData.BuildBasisItems(OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, OpenParms.ChainHLPlanProfile.NodeProfile.Key, OpenParms.StoreHLPlanProfile.VersionProfile.Key), OpenParms.ChainHLPlanProfile.NodeProfile);

            ((ChainPlanMaintCubeGroup)managerData.PlanCubeGroup).GetReadOnlyFlags(out managerData._chainReadOnly);
            managerData.headerDesc = "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
            if (OpenParms.DateRangeProfile.Name == string.Empty)
            {
                managerData.timeTotalName = "Total";
            }
            else
            {
                managerData.timeTotalName = OpenParms.DateRangeProfile.Name;
            }

            SetViewAndOrientation(OpenParms.ViewRID, eGridOrientation.TimeOnRow, StoreAttributeSetKey, StoreAttributeKey, FilterKey);

        }

        override public int StartingRowIndex { get { return chainSingleLevelViewData.StartingRowIndex; } }
        override public int NumberOfRows { get { return chainSingleLevelViewData.NumberOfRows; } }
        override public int StartingColIndex { get { return chainSingleLevelViewData.StartingColIndex; } }
        override public int NumberOfColumns { get { return chainSingleLevelViewData.NumberOfColumns; } }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            return chainSingleLevelViewData.SetPageCoordinates(iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns);
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            return chainSingleLevelViewData.GetColumnCoordinateMap();
        }

        override public void SetViewAndOrientation(int viewRID, eGridOrientation gridOrientation, int StoreAttributeSetKey, int StoreAttributeKey, int FilterKey)
        {
            OpenParms.ViewRID = viewRID;
            if (gridOrientation == eGridOrientation.TimeOnRow)
            {
                chainSingleLevelViewData = new ROChainSingleLevelLadderViewData(OpenParms.ViewRID, ref managerData);
            }
            else
            {
                chainSingleLevelViewData = new ROChainSingleLevelPeriodViewData(OpenParms.ViewRID, ref managerData);
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
            if (chainSingleLevelViewData._sortedVariableHeaders.Count == 0)
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
            return chainSingleLevelViewData._selectableQuantityHeaders;
        }
        override public ArrayList GetSelectableVariableHeaders()
        {
            return chainSingleLevelViewData._selectableVariableHeaders;
        }
        override public ArrayList GetSelectablePeriodHeaders()
        {
            return chainSingleLevelViewData._selectablePeriodHeaders;
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
            return chainSingleLevelViewData.GetSelectedVariableHeadersForChart();
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
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            if (chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] != null && chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].isValueNumeric)
            {
                Double dblValue = chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].Value;
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
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            return PlanCellFlagValues.isIneligible(chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].Flags);
        }
        override public bool IsCellLocked(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

            return ComputationCellFlagValues.isLocked(chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].Flags);
        }
        override public bool IsCellEditable(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            ComputationCellFlags cellFlags = chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].Flags;
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
            if (columnIndex < chainSingleLevelViewData._extraColumns)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData.GetRowCount())
            {
                return false;
            }

            return chainSingleLevelViewData.isRowBasis(columnIndex - chainSingleLevelViewData._extraColumns);  //swap row and column
        }

        

        override public void CellValueChanged(int rowIndex, int columnIndex, double newValue, eROCellAction eROCellActionParam, eDataType eDataType)
        {
            if (eROCellActionParam == eROCellAction.CellChanged)
            {
                //swap the column and row indexes
                chainSingleLevelViewData.managerData.PlanCubeGroup.SetCellValue(
                                                        managerData._commonWaferCoordinateList,
                                                        chainSingleLevelViewData.GetRowCubeWaferCoordinateList(rowIndex),
                                                        chainSingleLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex),
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
            chainSingleLevelViewData.managerData.PlanCubeGroup.RecomputeCubes(true);
        }

        override public void RebuildGridData()
        {
            // Does not use GridData
        }
		
        public DataSet GetChartDataset()
        {
            return chainSingleLevelViewData.CreateDatasetForChart();
        }
        override public DataSet ReconstructChartDataset(ArrayList alVariables)
        {
            return chainSingleLevelViewData.ReconstructDatasetForChart(alVariables);
        }

        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            return chainSingleLevelViewData.DoesDataSetContainInventoryUnitVariables();
        }
        override public string GetSalesUnitsVariableName()
        {
            return chainSingleLevelViewData.GetSalesUnitsVariableName();
        }
        override public string GetInventoryUnitsVariableName()
        {
            return chainSingleLevelViewData.GetInventoryUnitsVariableName();
        }

        override public ROData ReconstructPage()
        {
            return this.chainSingleLevelViewData.ReconstructPage();
        }

        public override ROCubeMetadata CunstructMetadata(ROCubeGetMetadataParams metadataParams)
        {
            return this.chainSingleLevelViewData.ConstructMetadata(metadataParams);
        }

        override public int iAddedColumnsCount { get { return chainSingleLevelViewData._extraColumns; } }

        override public void IncrementAddedColumnsCount(uint iColumnsAdded)
        {
            chainSingleLevelViewData._extraColumns += (int)iColumnsAdded;
        }

        public void PeriodChanged(bool selectYear, bool selectSeason, bool selectQuarter, bool selectMonth, bool selectWeek)
        {
            this.chainSingleLevelViewData.selectYear = selectYear;
            this.chainSingleLevelViewData.selectSeason = selectSeason;
            this.chainSingleLevelViewData.selectQuarter = selectQuarter;
            this.chainSingleLevelViewData.selectMonth = selectMonth;
            this.chainSingleLevelViewData.selectWeek = selectWeek;
            this.chainSingleLevelViewData.CreateSelectablePeriodHeaders();


            this.chainSingleLevelViewData._periodHeaderHash = this.chainSingleLevelViewData.CreatePeriodHash();
            this.chainSingleLevelViewData.BuildTimeHeaders();

        }
        public bool ShowYears()
        {
            return chainSingleLevelViewData.selectYear;
        }
        public bool ShowSeasons()
        {
            return chainSingleLevelViewData.selectSeason;
        }
        public bool ShowQuarters()
        {
            return chainSingleLevelViewData.selectQuarter;
        }
        public bool ShowMonths()
        {
            return chainSingleLevelViewData.selectMonth;
        }
        public bool ShowWeeks()
        {
            return chainSingleLevelViewData.selectWeek;
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
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
            ComputationCellFlags cellFlags = chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex].Flags; //swap the column and row indexes
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
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }
            if (rowIndex >= chainSingleLevelViewData._cubeValues.GetLength(1))
            {
                return;
            }

            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                chainSingleLevelViewData.managerData.PlanCubeGroup.SetCellLockStatus(
                            managerData._commonWaferCoordinateList,
                            chainSingleLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - chainSingleLevelViewData._extraColumns), //swap the column and row indexes
                            chainSingleLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                            blnLock);

            }
        }
        public void LockUnlockCellCascade(int rowIndex, int columnIndex, bool blnLock)
        {
            if (rowIndex == -1)
            {
                return;
            }
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainSingleLevelViewData._extraColumns || chainSingleLevelViewData._cubeValues[columnIndex - chainSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                chainSingleLevelViewData.managerData.PlanCubeGroup.SetCellRecursiveLockStatus(
                    managerData._commonWaferCoordinateList,
                    chainSingleLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - chainSingleLevelViewData._extraColumns), //swap the column and row indexes
                    chainSingleLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex), //swap the column and row indexes
                    blnLock);

            }

        }
        public void LockUnlockColumn(int columnIndex, bool blnLock)
        {
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainSingleLevelViewData._extraColumns)
            {
                return;
            }
            // TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = chainSingleLevelViewData.GetRowCount() - 1;
            int maxRows = chainSingleLevelViewData.GetColumnCount() - 1;
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCell(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockColumnCascade(int columnIndex, bool blnLock)
        {
            if (chainSingleLevelViewData._cubeValues == null)
            {
                return;
            }

            if (columnIndex - chainSingleLevelViewData._extraColumns >= chainSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainSingleLevelViewData._extraColumns)
            {
                return;
            }
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = chainSingleLevelViewData.GetRowCount() - 1;
            int maxRows = chainSingleLevelViewData.GetColumnCount() - 1;
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
            //int maxColumns = chainSingleLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = chainSingleLevelViewData.GetRowCount() - 1;
            for (int i = chainSingleLevelViewData._extraColumns; i <= chainSingleLevelViewData._extraColumns + maxColumns; i++)
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
            //int maxColumns = chainSingleLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = chainSingleLevelViewData.GetRowCount() - 1;
            for (int i = chainSingleLevelViewData._extraColumns; i <= chainSingleLevelViewData._extraColumns + maxColumns; i++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCellCascade(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockSheet(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = chainSingleLevelViewData.GetColumnCount() - 1;
            //int maxRows = chainSingleLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = chainSingleLevelViewData.GetRowCount() - 1;
            int maxRows = chainSingleLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = chainSingleLevelViewData._extraColumns; columnIndex <= chainSingleLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            //int maxColumns = chainSingleLevelViewData.GetColumnCount() - 1;
            //int maxRows = chainSingleLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = chainSingleLevelViewData.GetRowCount() - 1;
            int maxRows = chainSingleLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = chainSingleLevelViewData._extraColumns; columnIndex <= chainSingleLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            chainSingleLevelViewData.managerData.DollarScalingString = scalingValue;
            //chainSingleLevelViewData.UpdateGridDataSet();
        }
        override public void ScalingUnitsChanged(string scalingValue)
        {
            chainSingleLevelViewData.managerData.UnitsScalingString = scalingValue;
            //chainSingleLevelViewData.UpdateGridDataSet();
        }
        #endregion


    }
}
