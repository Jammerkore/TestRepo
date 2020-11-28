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

    public class ROPlanStoreSingleLevelManager : ROPlanStoreManager
    {
        override public PlanCubeGroup CubeGroup { get { return managerData.PlanCubeGroup; } }
        public ROPlanStoreSingleLevelManagerData managerData;
        private ROStoreSingleLevelViewData storeSingleLevelViewData;
		private LowLevelVersionOverrideProfileList _lowlevelVersionOverrideList; 
        public ROPlanStoreSingleLevelManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB, aOpenParms)
        {

        }

        public ROStoreSingleLevelViewData StoreSingleLevelViewData { get { return storeSingleLevelViewData; } }

        override public void InitializeData()
        {
            managerData = new ROPlanStoreSingleLevelManagerData(SAB, OpenParms);
            //managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).ProfileType, OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).Key);
            DateRangeProfile summaryDateRangeProfile = OpenParms.GetSummaryDateProfile(aSession: SAB.ClientServerSession);
            if (summaryDateRangeProfile != null)
            {
                managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(summaryDateRangeProfile.ProfileType, summaryDateRangeProfile.Key);
            }
            managerData._currentStorePlanProfile = OpenParms.StoreHLPlanProfile;
            managerData._commonWaferCoordinateList = new CubeWaferCoordinateList();
            managerData.Transaction = SAB.ApplicationServerSession.CreateTransaction(); //Create an App Server Transaction
            managerData.PlanCubeGroup = managerData.Transaction.CreateStorePlanMaintCubeGroup();

            managerData.PlanCubeGroup.OpenCubeGroup(OpenParms); //Open the cubegroup

            managerData.weekProfileList = OpenParms.GetWeekProfileList(SAB.ClientServerSession);
            managerData._periodProfileList = SAB.ClientServerSession.Calendar.GetPeriodProfileList(OpenParms.DateRangeProfile.Key); //Retrieve Period ProfileList from Calendar
            managerData._variableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.Variable);
            managerData._quantityVariableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);

            managerData.BuildBasisItems(OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, OpenParms.StoreHLPlanProfile.NodeProfile.Key, OpenParms.StoreHLPlanProfile.VersionProfile.Key), OpenParms.ChainHLPlanProfile.NodeProfile);

            managerData._basisProfileList = OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, OpenParms.StoreHLPlanProfile.NodeProfile.Key, OpenParms.StoreHLPlanProfile.VersionProfile.Key);
            managerData.BuildBasisItems(managerData._basisProfileList, OpenParms.StoreHLPlanProfile.NodeProfile);

            ((StorePlanMaintCubeGroup)managerData.PlanCubeGroup).GetReadOnlyFlags(out managerData._storeReadOnly, out managerData._chainReadOnly);
            managerData.headerDesc = "Store" + ((managerData._storeReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentStorePlanProfile.NodeProfile.Text + " / " + managerData._currentStorePlanProfile.VersionProfile.Description;
            if (OpenParms.DateRangeProfile.Name == string.Empty)
            {
                managerData.timeTotalName = "Total";
            }
            else
            {
                managerData.timeTotalName = OpenParms.DateRangeProfile.Name;
            }

            SetViewAndOrientation(OpenParms.ViewRID, eGridOrientation.TimeOnColumn, StoreAttributeSetKey, OpenParms.StoreGroupRID, FilterKey);

        }
	
        override public int StartingRowIndex { get { return storeSingleLevelViewData._extraColumns; } }
        override public int NumberOfRows { get { return storeSingleLevelViewData._extraColumns; } }
        override public int StartingColIndex { get { return storeSingleLevelViewData._extraColumns; } }
        override public int NumberOfColumns { get { return storeSingleLevelViewData._extraColumns; } }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            return storeSingleLevelViewData.SetPageCoordinates(iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns);
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            return storeSingleLevelViewData.GetColumnCoordinateMap();
        }

        override public void SetViewAndOrientation(int viewRID, eGridOrientation gridOrientation, int storeAttributeSetKey, int storeAttributeKey, int filterKey)
        {
            OpenParms.ViewRID = viewRID;
            managerData._storeAttributeSetKey = storeAttributeSetKey;
            managerData._storeAttributeKey = storeAttributeKey;
            managerData._filterKey = filterKey;
            
            if (gridOrientation == eGridOrientation.TimeOnRow)
            {
                storeSingleLevelViewData = new ROStoreSingleLevelLadderViewData(OpenParms.ViewRID, ref managerData);
            }
            else
            {
                storeSingleLevelViewData = new ROStoreSingleLevelPeriodViewData(OpenParms.ViewRID, ref managerData);
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
            if (storeSingleLevelViewData._sortedVariableHeaders.Count == 0)
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
            return storeSingleLevelViewData._selectableQuantityHeaders;
        }
        override public ArrayList GetSelectableVariableHeaders()
        {
            return storeSingleLevelViewData._selectableVariableHeaders;
        }
        override public ArrayList GetSelectablePeriodHeaders()
        {
            return storeSingleLevelViewData._selectablePeriodHeaders;
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
            return storeSingleLevelViewData.GetSelectedVariableHeadersForChart();
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
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

            
                Double dblValue = Convert.ToDouble(storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].Value);
			
                if (dblValue < 0)
                {
                    return true;
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
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

			if (storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsIneligible)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        override public bool IsCellLocked(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
			if (storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsLocked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        override public bool IsCellEditable(int rowIndex, int columnIndex)
        {
            if (rowIndex == -1)
            {
                return false;
            }
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

			if (storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsClosed ||
                    storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsDisplayOnly ||
                    storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null ||
                    storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsProtected) 
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
            if (columnIndex < storeSingleLevelViewData._extraColumns)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData.GetRowCount())
            {
                return false;
            }

            return storeSingleLevelViewData.isRowBasis(columnIndex - storeSingleLevelViewData._extraColumns);  //swap row and column
        }

        override public void CellValueChanged(int rowIndex, int columnIndex, double newValue, eROCellAction eROCellActionParam, eDataType eDataType)
        {

            if (eROCellActionParam == eROCellAction.CellChanged)
            {
                eDataType _dataType = eDataType;

                //swap the column and row indexes
                storeSingleLevelViewData.managerData.PlanCubeGroup.SetCellValue(
                                                    managerData._commonWaferCoordinateList,
                                                    //storeSingleLevelViewData.GetRowCubeWaferCoordinateList(rowIndex - storeSingleLevelViewData._extraColumns, _dataType),
                                                    storeSingleLevelViewData.GetRowCubeWaferCoordinateList(rowIndex, _dataType),
                                                    storeSingleLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex, _dataType),
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
            storeSingleLevelViewData.managerData.PlanCubeGroup.RecomputeCubes(true);
        }

        override public void RebuildGridData()
        {
            storeSingleLevelViewData.RebuildGridData();
        }
		
        public DataSet GetChartDataset()
        {
            return storeSingleLevelViewData.CreateDatasetForChart();
        }
        override public DataSet ReconstructChartDataset(ArrayList alVariables)
        {
            return storeSingleLevelViewData.ReconstructDatasetForChart(alVariables);
        }
        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            return storeSingleLevelViewData.DoesDataSetContainInventoryUnitVariables();
        }
        override public string GetSalesUnitsVariableName()
        {
            return storeSingleLevelViewData.GetSalesUnitsVariableName();
        }
        override public string GetInventoryUnitsVariableName()
        {
            return storeSingleLevelViewData.GetInventoryUnitsVariableName();
        }

        override public ROData ReconstructPage()
        {
            return this.storeSingleLevelViewData.ReconstructPage();
        }

        public override ROCubeMetadata CunstructMetadata(ROCubeGetMetadataParams metadataParams)
        {
            return this.storeSingleLevelViewData.ConstructMetadata(metadataParams);
        }
        override public int iAddedColumnsCount { get { return storeSingleLevelViewData._extraColumns; } }

        override public void IncrementAddedColumnsCount(uint iColumnsAdded)
        {
            storeSingleLevelViewData._extraColumns += (int)iColumnsAdded;
        }

        public void PeriodChanged(bool selectYear, bool selectSeason, bool selectQuarter, bool selectMonth, bool selectWeek)
        {
            this.storeSingleLevelViewData.selectYear = selectYear;
            this.storeSingleLevelViewData.selectSeason = selectSeason;
            this.storeSingleLevelViewData.selectQuarter = selectQuarter;
            this.storeSingleLevelViewData.selectMonth = selectMonth;
            this.storeSingleLevelViewData.selectWeek = selectWeek;
            this.storeSingleLevelViewData.CreateSelectablePeriodHeaders();


            this.storeSingleLevelViewData._periodHeaderHash = this.storeSingleLevelViewData.CreatePeriodHash();
            this.storeSingleLevelViewData.BuildTimeHeaders();

        }
        override public bool ShowYears()
        {
            return storeSingleLevelViewData.selectYear;
        }
        override public bool ShowSeasons()
        {
            return storeSingleLevelViewData.selectSeason;
        }
        override public bool ShowQuarters()
        {
            return storeSingleLevelViewData.selectQuarter;
        }
        override public bool ShowMonths()
        {
            return storeSingleLevelViewData.selectMonth;
        }
        override public bool ShowWeeks()
        {
            return storeSingleLevelViewData.selectWeek;
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
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

			if (!storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsClosed &&
                    !storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsDisplayOnly &&
                    !storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsIneligible &&
                    storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes != null &&
                    !storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsProtected &&
                    storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsLocked != blnLock 
                    )
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

            eDataType _dataType = eDataType.StoreDetail; //default

            if (rowIndex == -1)
            {
                return;
            }
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }
            if (rowIndex >= storeSingleLevelViewData._cubeValues.GetLength(1))
            {
                return;
            }

            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                storeSingleLevelViewData.managerData.PlanCubeGroup.SetCellLockStatus(
                            managerData._commonWaferCoordinateList,
                            storeSingleLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - storeSingleLevelViewData._extraColumns, _dataType), //swap the column and row indexes
                            storeSingleLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex, _dataType), //swap the column and row indexes
                            blnLock);

            }
        }
        public void LockUnlockCellCascade(int rowIndex, int columnIndex, bool blnLock)
        {
            eDataType _dataType = eDataType.StoreDetail; //default

            if (rowIndex == -1)
            {
                return;
            }
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeSingleLevelViewData._extraColumns || storeSingleLevelViewData._cubeValues[columnIndex - storeSingleLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                storeSingleLevelViewData.managerData.PlanCubeGroup.SetCellRecursiveLockStatus(
                    managerData._commonWaferCoordinateList,
                    storeSingleLevelViewData.GetRowCubeWaferCoordinateList(columnIndex - storeSingleLevelViewData._extraColumns, _dataType), //swap the column and row indexes
                    storeSingleLevelViewData.GetColumnCubeWaferCoordinateList(rowIndex, _dataType), //swap the column and row indexes
                    blnLock);

            }

        }
        public void LockUnlockColumn(int columnIndex, bool blnLock)
        {
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeSingleLevelViewData._extraColumns)
            {
                return;
            }
            // TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = storeSingleLevelViewData.GetRowCount() - 1;
            int maxRows = storeSingleLevelViewData.GetColumnCount() - 1;
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCell(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockColumnCascade(int columnIndex, bool blnLock)
        {
            if (storeSingleLevelViewData._cubeValues == null)
            {
                return;
            }

            if (columnIndex - storeSingleLevelViewData._extraColumns >= storeSingleLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < storeSingleLevelViewData._extraColumns)
            {
                return;
            }
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = storeSingleLevelViewData.GetRowCount() - 1;
            int maxRows = storeSingleLevelViewData.GetColumnCount() - 1;
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
            //int maxColumns = storeSingleLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = storeSingleLevelViewData.GetRowCount() - 1;
            for (int i = storeSingleLevelViewData._extraColumns; i <= storeSingleLevelViewData._extraColumns + maxColumns; i++)
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
            //int maxColumns = storeSingleLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = storeSingleLevelViewData.GetRowCount() - 1;
            for (int i = storeSingleLevelViewData._extraColumns; i <= storeSingleLevelViewData._extraColumns + maxColumns; i++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCellCascade(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockSheet(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = storeSingleLevelViewData.GetColumnCount() - 1;
            //int maxRows = storeSingleLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = storeSingleLevelViewData.GetRowCount() - 1;
            int maxRows = storeSingleLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = storeSingleLevelViewData._extraColumns; columnIndex <= storeSingleLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            //int maxColumns = storeSingleLevelViewData.GetColumnCount() - 1;
            //int maxRows = storeSingleLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = storeSingleLevelViewData.GetRowCount() - 1;
            int maxRows = storeSingleLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = storeSingleLevelViewData._extraColumns; columnIndex <= storeSingleLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            storeSingleLevelViewData.managerData.DollarScalingString = scalingValue;
            //storeSingleLevelViewData.UpdateGridDataSet();
        }
        override public void ScalingUnitsChanged(string scalingValue)
        {
            storeSingleLevelViewData.managerData.UnitsScalingString = scalingValue;
            //storeSingleLevelViewData.UpdateGridDataSet();
        }
        #endregion


    }
}
