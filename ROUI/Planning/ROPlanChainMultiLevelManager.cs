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

    public class ROPlanChainMultiLevelManager : ROPlanChainManager
    {
        override public PlanCubeGroup CubeGroup { get { return managerData.PlanCubeGroup; } }
        public ROPlanChainMultiLevelManagerData managerData;
        private ROChainMultiLevelViewData chainMultiLevelViewData;
        private LowLevelVersionOverrideProfileList _lowlevelVersionOverrideList; // Override low level enhancement
        private ProfileList _basisProfileList; // basis profile list
        
        public ROPlanChainMultiLevelManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB, aOpenParms)
        {

        }

        override public void InitializeData()
        {
            managerData = new ROPlanChainMultiLevelManagerData(SAB, OpenParms);
            //managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).ProfileType, OpenParms.GetSummaryDateProfile(SAB.ClientServerSession).Key);
            DateRangeProfile summaryDateRangeProfile = OpenParms.GetSummaryDateProfile(aSession: SAB.ClientServerSession);
            if (summaryDateRangeProfile != null)
            {
                managerData.SummaryDateProfile_WaferCoordinate = new CubeWaferCoordinate(summaryDateRangeProfile.ProfileType, summaryDateRangeProfile.Key);
            }
            managerData._currentChainPlanProfile = OpenParms.ChainHLPlanProfile;
            managerData._commonWaferCoordinateList = new CubeWaferCoordinateList();
            managerData.Transaction = SAB.ApplicationServerSession.CreateTransaction(); //Create an App Server Transaction
            managerData._currentChainPlanProfile.VersionProfile.ChainSecurity = SAB.ClientServerSession.GetMyVersionSecurityAssignment(OpenParms.ChainHLPlanProfile.VersionProfile.Key, (int)eSecurityTypes.Chain); //this.VersionRID
            BuildLowLevelVersionList();
            if (OpenParms.LowLevelPlanProfileList.Count > 0)
            { 
                managerData._lowLevelProfileList = OpenParms.LowLevelPlanProfileList;
            }
            
            if (OpenParms.GroupBy == eStorePlanSelectedGroupBy.ByVariable)
            {
                managerData.groupedBy = eStorePlanSelectedGroupBy.ByVariable;
            }
            else
            {
                //default from OTSPlanSelection.cs
                managerData.groupedBy = eStorePlanSelectedGroupBy.ByTimePeriod;
            }
            managerData.lowLevelsType = OpenParms.LowLevelsType;
            managerData.lowLevelsOffset = OpenParms.LowLevelsOffset;
            managerData.lowLevelsSequence = OpenParms.LowLevelsSequence;
            managerData.PlanCubeGroup = managerData.Transaction.CreateChainMultiLevelPlanMaintCubeGroup(); //Create a CreateChainMultiLevelPlanMaintCubeGroup

            managerData.PlanCubeGroup.OpenCubeGroup(OpenParms); //Open the cubegroup

            managerData.weekProfileList = OpenParms.GetWeekProfileList(SAB.ClientServerSession);
            managerData._periodProfileList = SAB.ClientServerSession.Calendar.GetPeriodProfileList(OpenParms.DateRangeProfile.Key); //Retrieve Period ProfileList from Calendar
            managerData._variableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.Variable);
            managerData._quantityVariableProfileList = managerData.PlanCubeGroup.GetFilteredProfileList(eProfileType.QuantityVariable);
            managerData._basisProfileList = OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, OpenParms.ChainHLPlanProfile.NodeProfile.Key, OpenParms.ChainHLPlanProfile.VersionProfile.Key);

            managerData.BuildBasisItems(managerData._basisProfileList, OpenParms.ChainHLPlanProfile.NodeProfile);

            if (OpenParms.LowLevelPlanProfileList.Count > 0)
            {
                AddLowLevelBasisProfiles();
            }

            //((ChainMultiLevelPlanMaintCubeGroup)managerData.PlanCubeGroup).GetReadOnlyFlags(out managerData._chainReadOnly);
            managerData.headerDesc = "Chain" + ((managerData._chainReadOnly) ? " (Read Only)" : "") + ": " + managerData._currentChainPlanProfile.NodeProfile.Text + " / " + managerData._currentChainPlanProfile.VersionProfile.Description;
            if (OpenParms.DateRangeProfile.Name == string.Empty)
            {
                managerData.timeTotalName = "Total";
            }
            else
            {
                managerData.timeTotalName = OpenParms.DateRangeProfile.Name;
            }
            if (managerData.lowLevelsType == eLowLevelsType.HierarchyLevel)
            {
                managerData.hierarchyLevelID = ((HierarchyLevelProfile)SAB.HierarchyServerSession.GetMainHierarchyData().HierarchyLevels[managerData.lowLevelsSequence]).LevelID;
            }
            else
            {
                managerData.hierarchyLevelID = "+" + managerData.lowLevelsOffset.ToString();
            }
            managerData.dateRangeProfileDisplayDate = OpenParms.DateRangeProfile.DisplayDate;

            SetViewAndOrientation(OpenParms.ViewRID, eGridOrientation.TimeOnColumn, StoreAttributeSetKey, StoreAttributeKey, FilterKey);
        }

        /// <summary>
        /// RO-1170 Build Low Level Nodes List for Multi Level Chain Plan
        /// </summary>
        private void BuildLowLevelVersionList()
        {
            try
            {
                OpenParms.ClearLowLevelPlanProfileList();

                PopulateVersionOverrideList(ePlanType.Chain);

                foreach (LowLevelVersionOverrideProfile lvop in _lowlevelVersionOverrideList)
                {
                    if (!lvop.Exclude)
                    {
                        PlanProfile planProfile = new PlanProfile(lvop.Key);
                        planProfile.NodeProfile = lvop.NodeProfile;
                        planProfile.NodeProfile.ChainSecurityProfile = this.SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Chain);
                        planProfile.NodeProfile.StoreSecurityProfile = this.SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lvop.Key, (int)eSecurityTypes.Store);
                        if (lvop.VersionIsOverridden)
                        {
                            planProfile.VersionProfile = lvop.VersionProfile;
                        }
                        else
                        {
                            planProfile.VersionProfile = OpenParms.LowLevelVersionDefault;
                        }
                        OpenParms.AddLowLevelPlanProfile(planProfile);
                    }
                }
            }
            catch
            {
            }
        }

        private void PopulateVersionOverrideList(ePlanType aPlanType)
        {
            _lowlevelVersionOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
            try
            {
                int hierNode = Include.NoRID;
                if (aPlanType == ePlanType.Chain)
                    hierNode = OpenParms.ChainHLPlanProfile.NodeProfile.Key;
                else
                    hierNode = OpenParms.StoreHLPlanProfile.NodeProfile.Key;

                HierarchySessionTransaction hTran = new HierarchySessionTransaction(this.SAB);
                if (OpenParms.LowLevelsType == eLowLevelsType.LevelOffset)
                {
                    _lowlevelVersionOverrideList = hTran.GetOverrideList(OpenParms.OverrideLowLevelRid, hierNode, OpenParms.LowLevelVersionDefault.Key,
                                                                               OpenParms.LowLevelsOffset, Include.NoRID, true, false);
                }
                else if (OpenParms.LowLevelsType == eLowLevelsType.HierarchyLevel)
                {
                    HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(hierNode);

                    _lowlevelVersionOverrideList = hTran.GetOverrideList(OpenParms.OverrideLowLevelRid, hierNode, OpenParms.LowLevelVersionDefault.Key,
                                                                             eHierarchyDescendantType.levelType, OpenParms.LowLevelsSequence, Include.NoRID, true, false);
                }
                else
                {
                    _lowlevelVersionOverrideList = new LowLevelVersionOverrideProfileList(eProfileType.LowLevelVersionOverride);
                }

            }
            catch
            {
                throw;
            }
        }

        private void AddLowLevelBasisProfiles()
        {
            try
            {
                foreach (PlanProfile planProf in OpenParms.LowLevelPlanProfileList)
                {

                    managerData._lowLevelBasisProfileList = OpenParms.GetBasisProfileList(managerData.PlanCubeGroup, planProf.NodeProfile.Key, OpenParms.ChainHLPlanProfile.VersionProfile.Key);
                    managerData.AddLowLevelBasisItems(managerData._lowLevelBasisProfileList, planProf.NodeProfile);
                    
                }
            }
            catch
            {
                throw;
            }
        }

        override public int StartingRowIndex { get { return chainMultiLevelViewData._extraColumns; } }
        override public int NumberOfRows { get { return chainMultiLevelViewData._extraColumns; } }
        override public int StartingColIndex { get { return chainMultiLevelViewData._extraColumns; } }
        override public int NumberOfColumns { get { return chainMultiLevelViewData._extraColumns; } }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            return chainMultiLevelViewData.SetPageCoordinates(iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns);
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            return chainMultiLevelViewData.GetColumnCoordinateMap();
        }

        override public void SetViewAndOrientation(int viewRID, eGridOrientation gridOrientation, int StoreAttributeSetKey, int StoreAttributeKey, int FilterKey)
        {
            OpenParms.ViewRID = viewRID;
            if (gridOrientation == eGridOrientation.TimeOnRow)
            {
                chainMultiLevelViewData = new ROChainMultiLevelLadderViewData(OpenParms.ViewRID, ref managerData);
            }
            else
            {
                chainMultiLevelViewData = new ROChainMultiLevelPeriodViewData(OpenParms.ViewRID, ref managerData);
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
            if (chainMultiLevelViewData._sortedVariableHeaders.Count == 0)
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
            return chainMultiLevelViewData._selectableQuantityHeaders;
        }
        override public ArrayList GetSelectableVariableHeaders()
        {
            return chainMultiLevelViewData._selectableVariableHeaders;
        }
        override public ArrayList GetSelectablePeriodHeaders()
        {
            return chainMultiLevelViewData._selectablePeriodHeaders;
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
            return chainMultiLevelViewData.GetSelectedVariableHeadersForChart();
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
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            if (chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] != null && chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsNumeric)
            {
                Double dblValue = Convert.ToDouble(chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex]);
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
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }


            if (chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsIneligible)
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
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }
            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }

            if (chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsLocked)
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
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
            
           // ComputationCellFlags cellFlags = chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsClosed;
            if (chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsClosed ||
                    chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsDisplayOnly ||
                    chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null ||
                    chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsProtected) 
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
            if (columnIndex < chainMultiLevelViewData._extraColumns)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData.GetRowCount())
            {
                return false;
            }

            return chainMultiLevelViewData.isRowBasis(columnIndex - chainMultiLevelViewData._extraColumns);  //swap row and column
        }

        override public void CellValueChanged(int rowIndex, int columnIndex, double newValue, eROCellAction eROCellActionParam, eDataType eDataType)
        {
            if (eROCellActionParam == eROCellAction.CellChanged)
            {
                eDataType _dataType = eDataType;
                chainMultiLevelViewData.managerData.PlanCubeGroup.SetCellValue(
                                                        managerData._commonWaferCoordinateList,
                                                        //chainMultiLevelViewData.GetRowCubeWaferCoordinateList(rowIndex - chainMultiLevelViewData._extraColumns),
                                                        chainMultiLevelViewData.GetRowCubeWaferCoordinateList(rowIndex, _dataType),
                                                        chainMultiLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex, _dataType),
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
            chainMultiLevelViewData.managerData.PlanCubeGroup.RecomputeCubes(true);
        }

        override public void RebuildGridData()
        {
            chainMultiLevelViewData.RebuildGridData();
        }

        public DataSet GetChartDataset()
        {
            return chainMultiLevelViewData.CreateDatasetForChart();
        }
        override public DataSet ReconstructChartDataset(ArrayList alVariables)
        {
            return chainMultiLevelViewData.ReconstructDatasetForChart(alVariables);
        }

        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            return chainMultiLevelViewData.DoesDataSetContainInventoryUnitVariables();
        }
        override public string GetSalesUnitsVariableName()
        {
            return chainMultiLevelViewData.GetSalesUnitsVariableName();
        }
        override public string GetInventoryUnitsVariableName()
        {
            return chainMultiLevelViewData.GetInventoryUnitsVariableName();
        }

        override public ROData ReconstructPage()
        {
            return this.chainMultiLevelViewData.ReconstructPage();
        }

        public override ROCubeMetadata CunstructMetadata(ROCubeGetMetadataParams metadataParams)
        {
            return this.chainMultiLevelViewData.ConstructMetadata(metadataParams);
        }

        override public int iAddedColumnsCount { get { return chainMultiLevelViewData._extraColumns; } }

        override public void IncrementAddedColumnsCount(uint iColumnsAdded)
        {
            chainMultiLevelViewData._extraColumns += (int)iColumnsAdded;
        }

        public void PeriodChanged(bool selectYear, bool selectSeason, bool selectQuarter, bool selectMonth, bool selectWeek)
        {
            this.chainMultiLevelViewData.selectYear = selectYear;
            this.chainMultiLevelViewData.selectSeason = selectSeason;
            this.chainMultiLevelViewData.selectQuarter = selectQuarter;
            this.chainMultiLevelViewData.selectMonth = selectMonth;
            this.chainMultiLevelViewData.selectWeek = selectWeek;
            this.chainMultiLevelViewData.CreateSelectablePeriodHeaders();


            this.chainMultiLevelViewData._periodHeaderHash = this.chainMultiLevelViewData.CreatePeriodHash();
            this.chainMultiLevelViewData.BuildTimeHeaders();

        }
        override public bool ShowYears()
        {
            return chainMultiLevelViewData.selectYear;
        }
        override public bool ShowSeasons()
        {
            return chainMultiLevelViewData.selectSeason;
        }
        override public bool ShowQuarters()
        {
            return chainMultiLevelViewData.selectQuarter;
        }
        override public bool ShowMonths()
        {
            return chainMultiLevelViewData.selectMonth;
        }
        override public bool ShowWeeks()
        {
            return chainMultiLevelViewData.selectWeek;
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
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return false;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return false;
            }


            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return false;
            }
            //ComputationCellFlags cellFlags = chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].Flags; //swap the column and row indexes
            //if (!PlanCellFlagValues.isClosed(cellFlags) &&
            //                !ComputationCellFlagValues.isDisplayOnly(cellFlags) &&
            //                !PlanCellFlagValues.isIneligible(cellFlags) &&
            //                !ComputationCellFlagValues.isNull(cellFlags) &&
            //                !PlanCellFlagValues.isProtected(cellFlags) &&
            //                !ComputationCellFlagValues.isHidden(cellFlags) &&
            //                !ComputationCellFlagValues.isReadOnly(cellFlags) &&
            //                ComputationCellFlagValues.isLocked(cellFlags) != blnLock)
            if (!chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsClosed &&
                    !chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsDisplayOnly &&
                    !chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsIneligible &&
                    chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes != null &&
                    !chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsProtected &&
                    chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex].ROCellAttributes.IsLocked != blnLock 
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
            eDataType _dataType = eDataType.ChainLowLevelDetail; //default
            if (rowIndex == -1)
            {
                return;
            }
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }
            if (rowIndex >= chainMultiLevelViewData._cubeValues.GetLength(1))
            {
                return;
            }

            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                chainMultiLevelViewData.managerData.PlanCubeGroup.SetCellLockStatus(
                            managerData._commonWaferCoordinateList,
                            chainMultiLevelViewData.GetRowCubeWaferCoordinateList(rowIndex, _dataType),
                            chainMultiLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex, _dataType),
                            blnLock);

            }
        }
        public void LockUnlockCellCascade(int rowIndex, int columnIndex, bool blnLock)
        {
            eDataType _dataType = eDataType.ChainLowLevelDetail; //default
            if (rowIndex == -1)
            {
                return;
            }
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainMultiLevelViewData._extraColumns || chainMultiLevelViewData._cubeValues[columnIndex - chainMultiLevelViewData._extraColumns, rowIndex] == null)
            {
                return;
            }
            if (CanLockCell(rowIndex, columnIndex, blnLock))
            {
                chainMultiLevelViewData.managerData.PlanCubeGroup.SetCellRecursiveLockStatus(
                    managerData._commonWaferCoordinateList,
                    chainMultiLevelViewData.GetRowCubeWaferCoordinateList(rowIndex, _dataType),
                    chainMultiLevelViewData.GetColumnCubeWaferCoordinateList(columnIndex, _dataType),
                    blnLock);

            }

        }
        public void LockUnlockColumn(int columnIndex, bool blnLock)
        {
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return;
            }
            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainMultiLevelViewData._extraColumns)
            {
                return;
            }
            // TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = chainMultiLevelViewData.GetRowCount() - 1;
            int maxRows = chainMultiLevelViewData.GetColumnCount() - 1;
            for (int i = 0; i <= maxRows; i++)
            {
                LockUnlockCell(i, columnIndex, blnLock);
            }
        }
        public void LockUnlockColumnCascade(int columnIndex, bool blnLock)
        {
            if (chainMultiLevelViewData._cubeValues == null)
            {
                return;
            }

            if (columnIndex - chainMultiLevelViewData._extraColumns >= chainMultiLevelViewData._cubeValues.GetLength(0))
            {
                return;
            }


            if (columnIndex < chainMultiLevelViewData._extraColumns)
            {
                return;
            }
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxRows = chainMultiLevelViewData.GetRowCount() - 1;
            int maxRows = chainMultiLevelViewData.GetColumnCount() - 1;
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
            //int maxColumns = chainMultiLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = chainMultiLevelViewData.GetRowCount() - 1;
            for (int i = chainMultiLevelViewData._extraColumns; i <= chainMultiLevelViewData._extraColumns + maxColumns; i++)
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
            //int maxColumns = chainMultiLevelViewData.GetColumnCount() - 1;
            //for (int i = 0; i <= maxColumns; i++)
            int maxColumns = chainMultiLevelViewData.GetRowCount() - 1;
            for (int i = chainMultiLevelViewData._extraColumns; i <= chainMultiLevelViewData._extraColumns + maxColumns; i++)
            // End TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            {
                LockUnlockCellCascade(rowIndex, i, blnLock);
            }
        }
        public void LockUnlockSheet(bool blnLock)
        {
            // Begin TT#4003 - JSmith - Chain Ladder locking inconsistent with locking in OTS Forecast Review
            // FYI - Rows are columns and columns are rows
            //int maxColumns = chainMultiLevelViewData.GetColumnCount() - 1;
            //int maxRows = chainMultiLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = chainMultiLevelViewData.GetRowCount() - 1;
            int maxRows = chainMultiLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = chainMultiLevelViewData._extraColumns; columnIndex <= chainMultiLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            //int maxColumns = chainMultiLevelViewData.GetColumnCount() - 1;
            //int maxRows = chainMultiLevelViewData.GetRowCount() - 1;
            //for (int columnIndex = 0; columnIndex <= maxColumns; columnIndex++)
            int maxColumns = chainMultiLevelViewData.GetRowCount() - 1;
            int maxRows = chainMultiLevelViewData.GetColumnCount() - 1;
            for (int columnIndex = chainMultiLevelViewData._extraColumns; columnIndex <= chainMultiLevelViewData._extraColumns + maxColumns; columnIndex++)
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
            chainMultiLevelViewData.managerData.DollarScalingString = scalingValue;
            //chainMultiLevelViewData.UpdateGridDataSet();
        }
        override public void ScalingUnitsChanged(string scalingValue)
        {
            chainMultiLevelViewData.managerData.UnitsScalingString = scalingValue;
            //chainMultiLevelViewData.UpdateGridDataSet();
        }
        #endregion


    }
}
