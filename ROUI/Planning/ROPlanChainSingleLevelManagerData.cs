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

using Logility.ROWebSharedTypes;

namespace Logility.ROUI
{
    /// <summary>
    /// Data that is defined at the manager level and used in the view
    /// This data is set only once per instance of the ladder screen
    /// </summary>
    public class ROPlanChainSingleLevelManagerData : ROPlanChainManagerData
    {
        //public PlanCubeGroup _planCubeGroup;
        public ProfileList _periodProfileList;
        public ProfileList _variableProfileList;
        public ProfileList _quantityVariableProfileList;
        public ProfileList weekProfileList;

        public ArrayList _selectableBasisHeaders;
        public Hashtable _basisLabelList;
        public string _basisLabel = null;
        public string headerDesc;
        public List<string> basisMenuList = new List<string>();
        public List<string> basisToolTipList = new List<string>();
        

        public PlanViewData _planViewDataLayer = null;
        public PlanProfile _currentChainPlanProfile;
        //public ApplicationSessionTransaction _transaction;
        public bool _chainReadOnly;
        public CubeWaferCoordinateList _commonWaferCoordinateList;
        public CubeWaferCoordinate SummaryDateProfile_WaferCoordinate; //used in period/time wafer coordinate
        //public string dollarScalingString = "1";
        //public string unitsScalingString = "1";
        public string timeTotalName;

        public ROPlanChainSingleLevelManagerData(SessionAddressBlock aSAB, PlanOpenParms aOpenParms)
             : base(aSAB, aOpenParms)
        {

        }

        public void BuildBasisItems(ProfileList basisProfList, HierarchyNodeProfile nodeProfile)
        {
           
            _basisLabel = LoadBasisLabel();
            _basisLabelList = new Hashtable();
            int basisCount = 0;
            foreach (BasisProfile basisProfile in basisProfList)
            {
                string tmpLabel = _basisLabel;
                foreach (BasisDetailProfile basisDetailProfile in basisProfile.BasisDetailProfileList)
                {
                    tmpLabel = tmpLabel.Replace("Merchandise", basisDetailProfile.HierarchyNodeProfile.Text);
                    tmpLabel = tmpLabel.Replace("Version", basisDetailProfile.VersionProfile.Description);
                    tmpLabel = tmpLabel.Replace("Time_Period", basisDetailProfile.DateRangeProfile.DisplayDate);
                    if (tmpLabel == "")
                    {
                        tmpLabel = basisProfile.Name;
                    }
                    else
                    {
                        basisProfile.Name = tmpLabel;
                    }
                    break;
                }
                _basisLabelList[new HashKeyObject(nodeProfile.Key, basisProfile.Key)] = tmpLabel;



                // Create Basis Tooltips
                if (basisProfile.BasisDetailProfileList.Count > 1)
                {
                    string toolTipStr = "";
                    string newLine = "";
                    int i = 0;

                    foreach (BasisDetailProfile basisDetailProfile in basisProfile.BasisDetailProfileList)
                    {
                        i++;
                        toolTipStr += newLine + "Detail " + Convert.ToInt32(i, CultureInfo.CurrentUICulture) + ": " + basisDetailProfile.HierarchyNodeProfile.Text + " / " + basisDetailProfile.VersionProfile.Description + " / " + basisDetailProfile.DateRangeProfile.DisplayDate + " / " + Convert.ToString(basisDetailProfile.Weight, CultureInfo.CurrentUICulture);
                        newLine = System.Environment.NewLine;
                    }
             
                    basisToolTipList.Add(toolTipStr);
                }
                else
                {

                    basisToolTipList.Add(tmpLabel);
                }

                basisCount++;
            }

            BuildBasisHeaders(basisProfList);
        }

        public string GetBasisLabel(int basisHeaderKey, int hierarchyNodeKey)
        {
            HashKeyObject activeKey = new HashKeyObject(hierarchyNodeKey, basisHeaderKey);
            return (string)_basisLabelList[activeKey];
        }
        public void BuildBasisHeaders(ProfileList BasisProfileList)
        {

            _selectableBasisHeaders = new ArrayList();

            if (BasisProfileList.Count > 0)
            {
                for (int i = 0; i < BasisProfileList.Count; i++)
                {

                    BasisProfile bp = (BasisProfile)BasisProfileList[i];
                    BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[0];

                    string lblName = GetBasisLabel(bp.Key, bdp.HierarchyNodeProfile.Key);
                    if (lblName != null)
                    {
                        bp.Name = lblName;
                    }

                    _selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
                    basisMenuList.Add(bp.Name);
                   
                }
            }

        }

       
        private string LoadBasisLabel()
        {
            try
            {
                //---Load Basis Label-----------
                string tmpBasisLabel = "";
                string concat = "";

                MIDRetail.Data.GlobalOptions opts = new MIDRetail.Data.GlobalOptions();
                DataTable dt = opts.GetGlobalOptions();
                DataRow dr = dt.Rows[0];

                int _productDisplayCombination = Convert.ToInt32(dr["PRODUCT_LEVEL_DISPLAY_ID"], CultureInfo.CurrentUICulture);
                int _storeDisplayCombination = Convert.ToInt32(dr["STORE_DISPLAY_OPTION_ID"], CultureInfo.CurrentUICulture);

                BasisLabelTypeProfile viewVarProf;
                ProfileList varProfList = GetBasisLabelProfList(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                dt = opts.GetBasisLabelInfo(Convert.ToInt32(dr["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture));
                foreach (DataRow releaseRow in dt.Rows)
                {
                    int basisLabelType = Convert.ToInt32(releaseRow["LABEL_TYPE"], CultureInfo.CurrentUICulture);
                    BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(basisLabelType);
                    viewVarProf = (BasisLabelTypeProfile)varProfList.FindKey(basisLabelType);
                    bltp.BasisLabelSystemOptionRID = Convert.ToInt32(releaseRow["SYSTEM_OPTION_RID"], CultureInfo.CurrentUICulture);
                    bltp.BasisLabelName = Convert.ToString(viewVarProf.BasisLabelName);
                    bltp.BasisLabelType = basisLabelType;
                    bltp.BasisLabelSequence = Convert.ToInt32(releaseRow["LABEL_SEQ"], CultureInfo.CurrentUICulture);
                    tmpBasisLabel = tmpBasisLabel + concat + bltp.BasisLabelName;
                    concat = " / ";
                }
                return tmpBasisLabel;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private ProfileList GetBasisLabelProfList(int systemOptionRID)
        {
            ProfileList basisLabelList = new ProfileList(eProfileType.BasisLabelType);

            Array values;
            string[] names;

            values = System.Enum.GetValues(typeof(eBasisLabelType));
            names = System.Enum.GetNames(typeof(eBasisLabelType));

            for (int i = 0; i < names.Length; i++)
            {
                BasisLabelTypeProfile bltp = new BasisLabelTypeProfile(i);
                bltp.BasisLabelSystemOptionRID = systemOptionRID;
                bltp.BasisLabelName = names[i];
                bltp.BasisLabelType = i;
                bltp.BasisLabelSequence = -1;

                basisLabelList.Add(bltp);
            }
            return basisLabelList;
        }


    }

    /// <summary>
    /// Data that is defined per view
    /// </summary>
    abstract public class ROChainSingleLevelViewData : ROChainViewData
    {
        public DataSet chartDataSet = null;
        public ROPlanChainSingleLevelManagerData managerData; //this gets passed in and is set once per instance of the screen

        protected IPlanComputationQuantityVariables _quantityVariables;

        protected RowColProfileHeader _adjustmentRowHeader;
        protected RowColProfileHeader _originalRowHeader;
        protected RowColProfileHeader _currentRowHeader;

        public ArrayList _selectableQuantityHeaders;
        public ArrayList _selectableVariableHeaders;
        public ArrayList _selectableVariableHeadersForChart;
        protected ArrayList _selectableTimeHeaders;

        public ArrayList _selectablePeriodHeaders;
        public SortedList _sortedVariableHeaders;
        protected SortedList _sortedTimeHeaders;
        public Hashtable _periodHeaderHash;

        protected List<SimpleRowHeader> rowHeaderList = new List<SimpleRowHeader>();
        protected List<SimpleColumnHeader> columnHeaderList = new List<SimpleColumnHeader>();

        public PlanWaferCell[,] _cubeValues;

        protected int maxTimeTotVars;

        protected ArrayList _variableNameParts;
        protected int _maxBandDepth;

        public bool selectYear;
        public bool selectSeason;
        public bool selectQuarter;
        public bool selectMonth;
        public bool selectWeek;

        // Page coordinates
        protected int _startingRowIndex, _numberOfRows, _startingColIndex, _numberOfColumns;
        protected int _years, _seasons, _quarters, _months, _weeks;
        protected PagingCoordinates _pagingCoordinates;


        public ROChainSingleLevelViewData(int viewRID, ref ROPlanChainSingleLevelManagerData managerData)
            : base (viewRID, managerData)
        {
            this.managerData = managerData;



            int i;
            VariableProfile viewVarProf;
            QuantityVariableProfile viewQVarProf;
            DataRow viewRow;
            Hashtable varKeyHash;
            Hashtable perKeyHash;

            Hashtable qVarKeyHash;
            bool cont;

            try
            {
                //Read PlanViewDetail table
                if (managerData._planViewDataLayer == null)
                {
                    managerData._planViewDataLayer = new PlanViewData();
                }
                // Get view details and load view formatting.
                DataTable _planViewDetail = GetViewDetails(viewKey: viewRID);

                varKeyHash = new Hashtable();
                _selectableVariableHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Variable)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Variable)
                        {
                            viewVarProf = (VariableProfile)managerData._variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewVarProf != null)
                            {
                                varKeyHash.Add(viewVarProf.Key, row);
                            }
                        }
                    }
                }

                foreach (VariableProfile variableProf in managerData._variableProfileList)
                {
                    viewRow = (DataRow)varKeyHash[variableProf.Key];
                    if (viewRow != null)
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), variableProf, variableProf.Groupings));
                    }
                    else
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, false, -1, variableProf, variableProf.Groupings));
                    }
                }

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.QuantityVariable)
                        {
                            viewQVarProf = (QuantityVariableProfile)managerData._quantityVariableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewQVarProf != null)
                            {
                                qVarKeyHash.Add(viewQVarProf.Key, row);
                            }
                        }
                    }
                }

                _quantityVariables = managerData.Transaction.PlanComputations.PlanQuantityVariables;

                _currentRowHeader = new RowColProfileHeader("Current Plan", true, 0, _quantityVariables.ValueQuantity);
                _originalRowHeader = new RowColProfileHeader("Original Plan", false, 1, _quantityVariables.ValueQuantity);

                viewRow = (DataRow)qVarKeyHash[_quantityVariables.ValueQuantity.Key];

                if (viewRow != null)
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", true, 0, _quantityVariables.ValueQuantity);
                }
                else
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", false, 0, _quantityVariables.ValueQuantity);
                }

                _selectableQuantityHeaders.Add(_adjustmentRowHeader);
                i = 2;

                foreach (QuantityVariableProfile qVarProf in managerData._quantityVariableProfileList)
                {
                    cont = false;


                    if (qVarProf.isChainSingleView && qVarProf.isHighLevel &&
                        qVarProf.isChainDetailCube)
                    {
                        cont = true;
                    }


                    if (qVarProf.isSelectable && cont)
                    {
                        viewRow = (DataRow)qVarKeyHash[qVarProf.Key];
                        if (viewRow != null)
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, true, i, qVarProf));
                        }
                        else
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, false, i, qVarProf));
                        }
                        i++;
                    }
                }
     

                // Load Periods

                perKeyHash = new Hashtable();
                _selectablePeriodHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Period)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Period)
                        {
                            perKeyHash.Add(Convert.ToInt32(row["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), row);
                        }
                    }
                }

                selectMonth = (DataRow)perKeyHash[(int)eProfileType.Month] != null;
                selectWeek = (DataRow)perKeyHash[(int)eProfileType.Week] != null;


                selectYear = (DataRow)perKeyHash[(int)eProfileType.Year] != null;
                selectSeason = (DataRow)perKeyHash[(int)eProfileType.Season] != null;
                selectQuarter = (DataRow)perKeyHash[(int)eProfileType.Quarter] != null;

                CreateSelectablePeriodHeaders();


                _periodHeaderHash = CreatePeriodHash();
                BuildTimeHeaders();
                
                CreateRowHeaderList();
                CreateColumnHeaderList();
                BuildVariableNameArrayList();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int StartingRowIndex { get { return _startingRowIndex; } }
        public int NumberOfRows { get { return _numberOfRows; } }
        public int StartingColIndex { get { return _startingColIndex; } }
        public int NumberOfColumns { get { return _numberOfColumns; } }

        abstract public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns);

        abstract public Dictionary<string, int> GetColumnCoordinateMap();

        /// <summary>
        /// Used to retrieve cube values
        /// </summary>
        abstract protected void CreateRowHeaderList();

        /// <summary>
        /// Call after creating the rowHeaderList
        /// Used to retrieve cube values
        /// </summary>
        abstract protected void CreateColumnHeaderList();

        public Hashtable CreatePeriodHash()
        {
            int i;

            try
            {
                Hashtable ht = new Hashtable();

                for (i = 0; i < _selectablePeriodHeaders.Count; i++)
                {
                    if (((RowColProfileHeader)_selectablePeriodHeaders[i]).IsDisplayed)
                    {
                        ht.Add(((RowColProfileHeader)_selectablePeriodHeaders[i]).Sequence, null);
                    }
                }
                return ht;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void BuildTimeHeaders()
        {
            int i;

            try
            {
                i = 0;

                _selectableTimeHeaders = new ArrayList();

                
                BuildPeriodHeaders(managerData._periodProfileList, ref i);

                _sortedTimeHeaders = CreateSortedList(_selectableTimeHeaders);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        private void BuildPeriodHeaders(ProfileList aPeriodList, ref int aSeq)
        {
            try
            {
                if (aPeriodList.ProfileType == eProfileType.Period)
                {
                    foreach (PeriodProfile perProf in aPeriodList)
                    {
                        if (_periodHeaderHash.Contains((int)perProf.PeriodProfileType))
                        {
                            _selectableTimeHeaders.Add(new RowColProfileHeader(perProf.Text(), true, aSeq++, perProf));
                        }

                        if (perProf.ChildPeriodList.Count > 0)
                        {
                            BuildPeriodHeaders(perProf.ChildPeriodList, ref aSeq);
                        }
                        else
                        {
                            BuildPeriodHeaders(perProf.Weeks, ref aSeq);
                        }
                    }
                }
                else
                {
                    if (_periodHeaderHash.Contains((int)aPeriodList.ProfileType))
                    {
                        foreach (WeekProfile weekProf in aPeriodList)
                        {
                            if (managerData.weekProfileList.Contains(weekProf.Key))
                            {
                                _selectableTimeHeaders.Add(new RowColProfileHeader(weekProf.Text(), true, aSeq++, weekProf));
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
        public void CreateSelectablePeriodHeaders()
        {
            _selectablePeriodHeaders.Clear();
            if (selectYear)
            {
                _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", true, (int)eProfileType.Year, null));
            }
            else
            {
                _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Years", false, (int)eProfileType.Year, null));
            }

            if (selectSeason)
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", true, (int)eProfileType.Season, null));
            }
            else
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Seasons", false, (int)eProfileType.Season, null));
            }

            if (selectQuarter)
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", true, (int)eProfileType.Quarter, null));
            }
            else
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Quarters", false, (int)eProfileType.Quarter, null));
            }

            if (!selectYear && !selectSeason && !selectQuarter && !selectMonth && !selectWeek)
            {
                selectMonth = true;
            }


            if (selectMonth)
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", true, (int)eProfileType.Month, null));
            }
            else
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Months", false, (int)eProfileType.Month, null));
            }

            if (selectWeek)
            {
                _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", true, (int)eProfileType.Week, null));
            }
            else
            {
               _selectablePeriodHeaders.Add(new RowColProfileHeader("Show Weeks", false, (int)eProfileType.Week, null));
            }

        }

  
        protected SortedList CreateSortedList(ArrayList aSelectableList)
        {
            SortedList sortList;
            IDictionaryEnumerator enumerator;
            int i, j;
            int newCols;

            try
            {
                sortList = new SortedList();
                newCols = 0;

                for (i = 0; i < aSelectableList.Count; i++)
                {
                    if (((RowColProfileHeader)aSelectableList[i]).IsDisplayed)
                    {
                        if (((RowColProfileHeader)aSelectableList[i]).Sequence == -1)
                        {
                            newCols++;
                            ((RowColProfileHeader)aSelectableList[i]).Sequence = newCols * -1;
                        }
                        sortList.Add(((RowColProfileHeader)aSelectableList[i]).Sequence, i);
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[i]).Sequence = -1;
                    }
                }

                enumerator = sortList.GetEnumerator();
                j = 0;

                while (enumerator.MoveNext())
                {
                    if (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) < 0)
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = sortList.Count - newCols + (Convert.ToInt32(enumerator.Key, CultureInfo.CurrentUICulture) * -1) - 1;
                    }
                    else
                    {
                        ((RowColProfileHeader)aSelectableList[(int)enumerator.Value]).Sequence = j;
                        j++;
                    }
                }

                SortedList aSortedList = new SortedList();

                foreach (RowColProfileHeader rowColHeader in aSelectableList)
                {
                    if (rowColHeader.IsDisplayed)
                    {
                        aSortedList.Add(rowColHeader.Sequence, rowColHeader);
                    }
                }
                return aSortedList;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public CubeWaferCoordinateList GetRowCubeWaferCoordinateList(int rowIndex)
        {
            return rowHeaderList[rowIndex].CubeWaferCoorList;
        }
        public CubeWaferCoordinateList GetColumnCubeWaferCoordinateList(int columnIndex)
        {
            return columnHeaderList[columnIndex].CubeWaferCoorList;
        }
        public VariableProfile GetVariableProfile(int rowIndex)
        {
            return rowHeaderList[rowIndex].varProf;
        }
        public bool isRowBasis(int rowIndex)
        {
            if (rowHeaderList[rowIndex].CubeWaferCoorList.FindCoordinateType(eProfileType.Basis) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int GetRowCount()
        {
            return rowHeaderList.Count;
        }
        public int GetColumnCount()
        {
            return columnHeaderList.Count;
        }
        public int _extraColumns;

        public ROData ReconstructPage()
        {
            _pagingCoordinates = SetPageCoordinates(StartingRowIndex, NumberOfRows, StartingColIndex, NumberOfColumns);
            CreateRowHeaderList();
            CreateColumnHeaderList();
            return CreatePageFromViewData();
        }

        public ROCubeMetadata ConstructMetadata(ROCubeGetMetadataParams metadataParams)
        {
            _pagingCoordinates = SetPageCoordinates(StartingRowIndex, NumberOfRows, StartingColIndex, NumberOfColumns);
            CreateRowHeaderList();
            CreateColumnHeaderList();
            return CreateMetadataFromViewData(metadataParams);
        }

        protected void BuildVariableNameArrayList()
        {
            try
            {
                _variableNameParts = new ArrayList();
                _variableNameParts.Add(" Reg IDX");
                _variableNameParts.Add(" R/P IDX");
                _variableNameParts.Add(" Mkdn IDX");
                _variableNameParts.Add(" M/D IDX");
                _variableNameParts.Add(" Promo IDX");
                _variableNameParts.Add(" Reg");
                _variableNameParts.Add(" R/P");
                _variableNameParts.Add(" Mkdn");
                _variableNameParts.Add(" M/D");
                _variableNameParts.Add(" Promo");
            }
            catch
            {
                throw;
            }
        }

        abstract public ROData CreatePageFromViewData();

        abstract public ROCubeMetadata CreateMetadataFromViewData(ROCubeGetMetadataParams metadataParams);

        /// <summary>
        /// used to get the sales units variable name for the chart
        /// </summary>
        /// <returns></returns>
        public string GetSalesUnitsVariableName()
        {
            string salesUnitsVariableName;

            if (isChainNodePlanLevelEqualTotal() == true)
            {
                salesUnitsVariableName = managerData.Transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.VariableName; //"Sales";
            }
            else
            {
                salesUnitsVariableName = managerData.Transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.VariableName; //"Sales R/P";
            }
            return salesUnitsVariableName;
        }

        /// <summary>
        /// used to get the inventory units variable name for the chart
        /// </summary>
        /// <returns></returns>
        public string GetInventoryUnitsVariableName()
        {
            string inventoryUnitsVariableName;

            if (isChainNodePlanLevelEqualTotal() == true)
            {
                inventoryUnitsVariableName = managerData.Transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.VariableName; //"Stock";
            }
            else
            {
                inventoryUnitsVariableName = managerData.Transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.VariableName; //"Stock Reg";
            }
            return inventoryUnitsVariableName;
        }

        /// <summary>
        /// used to help determine the sales units variable name and the inventory units variable name
        /// </summary>
        /// <returns></returns>
        protected bool isChainNodePlanLevelEqualTotal()
        {
            if (this.managerData._currentChainPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Total)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// checks the grid dataset to ensure the sales and inventory unit variables are contained in this view
        /// </summary>
        /// <returns></returns>
        abstract public bool DoesDataSetContainInventoryUnitVariables();
        
        protected bool hasAdjustmentColumnsDisplayed()
        {
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();
            bool found = false;
            foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            {
                if (rowHdrTag.varProf.VariableName == salesUnitsVariableName || rowHdrTag.varProf.VariableName == inventoryUnitsVariableName)
                {
                    if (rowHdrTag.RowHeading.Trim() == "ADJ")
                    {
                        found = true;
                    }
                }
            }
            return found;
        }

        protected List<chartColumnRef> chartColumnRefList = new List<chartColumnRef>();
        abstract public DataSet CreateDatasetForChart();


        abstract public DataSet ReconstructDatasetForChart(ArrayList alVariables);
        
        protected string ParseTotalVariableName(string aVarName)
        {
            try
            {
                string parsedName = aVarName;
                string part1, part2;
                string searchPart = string.Empty;
                for (int i = 0; i < _variableNameParts.Count; i++)
                {
                    if (aVarName.EndsWith(_variableNameParts[i].ToString()))
                    {
                        searchPart = _variableNameParts[i].ToString();
                        break;
                    }
                }

                if (searchPart != string.Empty)
                {
                    if (aVarName.Contains(searchPart))
                    {
                        part1 = aVarName.Substring(0, aVarName.IndexOf(searchPart));
                        part2 = aVarName.Substring(aVarName.IndexOf(searchPart));
                        parsedName = part1 + Environment.NewLine + part2;
                    }
                }
                return parsedName;
            } 
            catch
            {
                throw;
            }
        }

        protected PlanWaferCell[,] GetCubeValues()
        {
            try
            {
                CubeWafer aCubeWafer = new CubeWafer();
                PlanWaferCell[,] planWaferCellTable = null;

                //Fill CommonWaferCoordinateListGroup
                aCubeWafer.CommonWaferCoordinateList = managerData._commonWaferCoordinateList;

                //Fill ColWaferCoordinateListGroup
                aCubeWafer.ColWaferCoordinateListGroup.Clear();
                foreach (SimpleColumnHeader c in columnHeaderList)
                {
                    aCubeWafer.ColWaferCoordinateListGroup.Add(c.CubeWaferCoorList);
                }

                //Fill RowWaferCoordinateListGroup
                aCubeWafer.RowWaferCoordinateListGroup.Clear();
                foreach (SimpleRowHeader r in rowHeaderList)
                {
                    aCubeWafer.RowWaferCoordinateListGroup.Add(r.CubeWaferCoorList);
                }

                if (aCubeWafer.ColWaferCoordinateListGroup.Count > 0 && aCubeWafer.RowWaferCoordinateListGroup.Count > 0)
                {
                    // Retreive array of values
                    planWaferCellTable = managerData.PlanCubeGroup.GetPlanWaferCellValues(aCubeWafer, managerData.UnitsScalingString, managerData.DollarScalingString);
                }

                return planWaferCellTable;
            }
            catch
            {
                throw;
            }

        }




        abstract protected void SetCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex);

        abstract protected void SetChartCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex);

        abstract protected void SetCellValuesOnRow(ROCells cells, int rowIndex, int rowCellIndex);

        protected void CheckForParentKey(DataRow aDataRow, Profile aProfile, int aIndex)
        {
            try
            {
                string profileTypeStr = null;
                switch (aProfile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)aProfile;
                        profileTypeStr = perProf.PeriodProfileType.ToString();
                        break; 
                         
                    case eProfileType.Week:
                        profileTypeStr = aProfile.ProfileType.ToString();
                        break;
                }
                string parentName = null;

                switch (aProfile.ProfileType)
                {
                    case eProfileType.Year:
                        break;

                    case eProfileType.Season:
                        if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Quarter:
                        if (_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Month:
                        if (_quarters > 0)
                        {
                            parentName = "Quarter";
                        }
                        else if(_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Week:
                        if (_months > 0)
                        {
                            parentName = "Month";
                        }
                        else if(_quarters > 0)
                        {
                            parentName = "Quarter";
                        }
                        else if (_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                }

                if (parentName != null && aIndex > 0)
                {
                    for (int i = aIndex - 1; i >= 0; i--)
                    {
                        string profileTypeStr2 = null;
                        RowColProfileHeader timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);
                        switch (timeHeader.Profile.ProfileType)
                        {
                            case eProfileType.Period:
                                PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                                profileTypeStr2 = perProf.PeriodProfileType.ToString();
                                break;

                            case eProfileType.Week:
                                profileTypeStr2 = timeHeader.Profile.ProfileType.ToString();
                                break;
                        }
                        if (profileTypeStr2 != profileTypeStr &&  profileTypeStr2 == parentName)
                        {
                            aDataRow["ParentRowKey"] = timeHeader.Name;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        protected void CheckForParentKey(ROCells cellValues, Profile aProfile, int aIndex, int aRowIndex)
        {
            try
            {
                string profileTypeStr = null;
                
                switch (aProfile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)aProfile;
                        profileTypeStr = perProf.PeriodProfileType.ToString();
                        break;

                    case eProfileType.Week:
                        profileTypeStr = aProfile.ProfileType.ToString();
                        break;
                }

                string parentName = null;
                switch (aProfile.ProfileType)
                {
                    case eProfileType.Year:
                        break;

                    case eProfileType.Season:
                        if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Quarter:
                        if (_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Month:
                        if (_quarters > 0)
                        {
                            parentName = "Quarter";
                        }
                        else if (_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                    case eProfileType.Week:
                        if (_months > 0)
                        {
                            parentName = "Month";
                        }
                        else if (_quarters > 0)
                        {
                            parentName = "Quarter";
                        }
                        else if (_seasons > 0)
                        {
                            parentName = "Season";
                        }
                        else if (_years > 0)
                        {
                            parentName = "Year";
                        }
                        break;
                }

                if (parentName != null && aIndex > 0)
                {
                    for (int i = aIndex - 1; i >= 0; i--)
                    {
                        string profileTypeStr2 = null;
                        RowColProfileHeader timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);
                        switch (timeHeader.Profile.ProfileType)
                        {
                            case eProfileType.Period:
                                PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                                profileTypeStr2 = perProf.PeriodProfileType.ToString();
                                break;

                            case eProfileType.Week:
                                profileTypeStr2 = timeHeader.Profile.ProfileType.ToString();
                                break;
                        }
                        if (profileTypeStr2 != profileTypeStr && profileTypeStr2 == parentName)
                        {
                            cellValues.ROCell[aRowIndex][cellValues.GetIndexOfColumn("ParentRowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        protected void CheckDataSetMember(DataSet aDataset, eProfileType aProfileType)
        {
            if (aDataset.ExtendedProperties == null || aDataset.ExtendedProperties.Count == 0)
            {
                aDataset.ExtendedProperties.Add(aProfileType.ToString(), _maxBandDepth);
            }
        }

        protected eProfileType GetProfileType(RowColProfileHeader aRowColProfileHeader)
        {
            try
            {
                eProfileType profileType = eProfileType.None;
                switch (aRowColProfileHeader.Profile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)aRowColProfileHeader.Profile;
                        profileType = perProf.PeriodProfileType;
                         
                        break;

                    case eProfileType.Week:
                        profileType =  aRowColProfileHeader.Profile.ProfileType;
                        break;
                }
                return profileType;
            }
            catch
            {
                throw;
            }
        }

        public ArrayList GetSelectedVariableHeadersForChart()
        {
            try 
            {
                _selectableVariableHeadersForChart = new ArrayList();

                foreach (RowColProfileHeader rcph in _selectableVariableHeaders)
                {
                    if (rcph.IsDisplayed)
                    {
                        VariableProfile vp = (VariableProfile)rcph.Profile;
                        if (vp.VariableStyle == eVariableStyle.Units || vp.VariableStyle == eVariableStyle.Dollar)   
                        {
                             _selectableVariableHeadersForChart.Add(new RowColProfileHeader(vp.VariableName, true, rcph.Sequence,  vp, vp.Groupings));
                        }
                    }
                }
                return _selectableVariableHeadersForChart;
            }
            catch
            {
                throw;
            }
        }

    }

    /// <summary>
    /// Data that is defined per view
    /// </summary>
    public class ROChainSingleLevelLadderViewData : ROChainSingleLevelViewData
    {
        public ROChainSingleLevelLadderViewData(int viewRID, ref ROPlanChainSingleLevelManagerData managerData)
            : base(viewRID, ref managerData)
        {
            this.managerData = managerData;



            int i;
            VariableProfile viewVarProf;
            QuantityVariableProfile viewQVarProf;
            DataRow viewRow;
            Hashtable varKeyHash;
            Hashtable perKeyHash;

            Hashtable qVarKeyHash;
            bool cont;

            try
            {
                //Read PlanViewDetail table
                if (managerData._planViewDataLayer == null)
                {
                    managerData._planViewDataLayer = new PlanViewData();
                }
                // Get view details and load view formatting.
                DataTable _planViewDetail = GetViewDetails(viewKey: viewRID);



                varKeyHash = new Hashtable();
                _selectableVariableHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Variable)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Variable)
                        {
                            viewVarProf = (VariableProfile)managerData._variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewVarProf != null)
                            {
                                varKeyHash.Add(viewVarProf.Key, row);
                            }
                        }
                    }
                }

                foreach (VariableProfile variableProf in managerData._variableProfileList)
                {
                    viewRow = (DataRow)varKeyHash[variableProf.Key];
                    if (viewRow != null)
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), variableProf, variableProf.Groupings));
                    }
                    else
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, false, -1, variableProf, variableProf.Groupings));
                    }
                }

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.QuantityVariable)
                        {
                            viewQVarProf = (QuantityVariableProfile)managerData._quantityVariableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewQVarProf != null)
                            {
                                qVarKeyHash.Add(viewQVarProf.Key, row);
                            }
                        }
                    }
                }

                _quantityVariables = managerData.Transaction.PlanComputations.PlanQuantityVariables;

                _currentRowHeader = new RowColProfileHeader("Current Plan", true, 0, _quantityVariables.ValueQuantity);
                _originalRowHeader = new RowColProfileHeader("Original Plan", false, 1, _quantityVariables.ValueQuantity);

                viewRow = (DataRow)qVarKeyHash[_quantityVariables.ValueQuantity.Key];

                if (viewRow != null)
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", true, 0, _quantityVariables.ValueQuantity);
                }
                else
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", false, 0, _quantityVariables.ValueQuantity);
                }

                _selectableQuantityHeaders.Add(_adjustmentRowHeader);
                i = 2;

                foreach (QuantityVariableProfile qVarProf in managerData._quantityVariableProfileList)
                {
                    cont = false;


                    if (qVarProf.isChainSingleView && qVarProf.isHighLevel &&
                        qVarProf.isChainDetailCube)
                    {
                        cont = true;
                    }


                    if (qVarProf.isSelectable && cont)
                    {
                        viewRow = (DataRow)qVarKeyHash[qVarProf.Key];
                        if (viewRow != null)
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, true, i, qVarProf));
                        }
                        else
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, false, i, qVarProf));
                        }
                        i++;
                    }
                }


                // Load Periods

                perKeyHash = new Hashtable();
                _selectablePeriodHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Period)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Period)
                        {
                            perKeyHash.Add(Convert.ToInt32(row["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), row);
                        }
                    }
                }

                selectMonth = (DataRow)perKeyHash[(int)eProfileType.Month] != null;
                selectWeek = (DataRow)perKeyHash[(int)eProfileType.Week] != null;


                selectYear = (DataRow)perKeyHash[(int)eProfileType.Year] != null;
                selectSeason = (DataRow)perKeyHash[(int)eProfileType.Season] != null;
                selectQuarter = (DataRow)perKeyHash[(int)eProfileType.Quarter] != null;

                CreateSelectablePeriodHeaders();


                _periodHeaderHash = CreatePeriodHash();
                BuildTimeHeaders();

                CreateRowHeaderList();
                CreateColumnHeaderList();
                BuildVariableNameArrayList();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            int i, first, last;

            PagingCoordinates pagingCoordinates = new PagingCoordinates();

            _startingRowIndex = iStartingRowIndex;
            _numberOfRows = iNumberOfRows;
            _startingColIndex = iStartingColIndex;
            _numberOfColumns = iNumberOfColumns;

            first = _startingRowIndex;
            last = first + _numberOfRows;

            eProfileType highestPeriodType = eProfileType.None;
            int periodCount = 0;
            int selectedPeriodCount = 0;
            if (selectYear)
            {
                highestPeriodType = eProfileType.Year;
            }
            else if (selectSeason)
            {
                highestPeriodType = eProfileType.Season;
            }
            else if (selectQuarter)
            {
                highestPeriodType = eProfileType.Quarter;
            }
            else if (selectMonth)
            {
                highestPeriodType = eProfileType.Month;
            }
            else if (selectWeek)
            {
                highestPeriodType = eProfileType.Week;
            }

            _years = 0;
            _seasons = 0;
            _quarters = 0;
            _months = 0;
            _weeks = 0;
            bool selectTimePeriod = false;
            for (i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                RowColProfileHeader rowColHeader = (RowColProfileHeader)_sortedTimeHeaders[i];
                selectTimePeriod = false;
                //** RO-1180 Block of Original Code left for Reference
                //switch (highestPeriodType)
                //{
                //    case eProfileType.Year:
                //        if (rowColHeader.Profile is YearProfile)
                //        {
                //            ++periodCount;
                //            if (periodCount > first
                //                && periodCount <= last)
                //            {
                //                ++selectedPeriodCount;
                //                selectTimePeriod = true;
                //            }
                //            else
                //            {
                //                selectTimePeriod = false;
                //            }
                //        }
                //        break;
                //    case eProfileType.Season:
                //        if (rowColHeader.Profile is SeasonProfile)
                //        {
                //            ++periodCount;
                //            if (periodCount > first
                //                && periodCount <= last)
                //            {
                //                ++selectedPeriodCount;
                //                selectTimePeriod = true;
                //            }
                //            else
                //            {
                //                selectTimePeriod = false;
                //            }
                //        }
                //        break;
                //    case eProfileType.Quarter:
                //        if (rowColHeader.Profile is QuarterProfile)
                //        {
                //            ++periodCount;
                //            if (periodCount > first
                //                && periodCount <= last)
                //            {
                //                ++selectedPeriodCount;
                //                selectTimePeriod = true;
                //            }
                //            else
                //            {
                //                selectTimePeriod = false;
                //            }
                //        }
                //        break;
                //    case eProfileType.Month:
                //        if (rowColHeader.Profile is MonthProfile)
                //        {
                //            ++periodCount;
                //            if (periodCount > first
                //                && periodCount <= last)
                //            {
                //                ++selectedPeriodCount;
                //                selectTimePeriod = true;
                //            }
                //            else
                //            {
                //                selectTimePeriod = false;
                //            }
                //        }
                //        break;
                //    case eProfileType.Week:
                //        if (rowColHeader.Profile is WeekProfile)
                //        {
                //            ++periodCount;
                //            if (periodCount > first
                //                && periodCount <= last)
                //            {
                //                ++selectedPeriodCount;
                //                selectTimePeriod = true;
                //            }
                //            else
                //            {
                //                selectTimePeriod = false;
                //            }
                //        }
                //        break;
                //    default:
                //        break;
                //}
                //*** RO-1180 End Block


                if (rowColHeader.Profile is YearProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if  (rowColHeader.Profile is SeasonProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is QuarterProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is MonthProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is WeekProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                        
                

                if (selectTimePeriod)
                {
                    if (rowColHeader.Profile is YearProfile)
                    {
                        ++_years;
                    }
                    else if (rowColHeader.Profile is SeasonProfile)
                    {
                        ++_seasons;
                    }
                    else if (rowColHeader.Profile is QuarterProfile)
                    {
                        ++_quarters;
                    }
                    else if (rowColHeader.Profile is MonthProfile)
                    {
                        ++_months;
                    }
                    else if (rowColHeader.Profile is WeekProfile)
                    {
                        ++_weeks;
                    }
                }
            }

            if (selectedPeriodCount == 0)
            {
                pagingCoordinates.FirstRowItem = selectedPeriodCount;
            }
            else
            {
                pagingCoordinates.FirstRowItem = first + 1;
            }
            if (selectedPeriodCount == 0)
            {
                pagingCoordinates.LastRowItem = selectedPeriodCount;
            }
            else
            {
                pagingCoordinates.LastRowItem = pagingCoordinates.FirstRowItem + (selectedPeriodCount - 1);
            }
            pagingCoordinates.TotalRowItems = periodCount;

            first = _startingColIndex;
            last = first + _numberOfColumns;

            if (first >= rowHeaderList.Count)
            {
                pagingCoordinates.FirstColItem = rowHeaderList.Count;
            }
            else
            {
                pagingCoordinates.FirstColItem = first + 1;
            }
            if (last >= rowHeaderList.Count)
            {
                pagingCoordinates.LastColItem = rowHeaderList.Count;
            }
            else
            {
                pagingCoordinates.LastColItem = last;
            }
            pagingCoordinates.TotalColItems = rowHeaderList.Count;

            //** RO-1180 Additional coordinates added for paging row and columns logic
            if (rowHeaderList.Count > 0 && pagingCoordinates.TotalColItems > 0)
            {
                pagingCoordinates.CountDistinctColumns = rowHeaderList.Count / _sortedVariableHeaders.Count;
            }
            else
            {
                pagingCoordinates.CountDistinctColumns = 1;
            }
            pagingCoordinates.ColumnsSelected = pagingCoordinates.LastColItem - pagingCoordinates.FirstColItem + 1;
            pagingCoordinates.FirstColumnIndex = pagingCoordinates.FirstColItem - 1;
            pagingCoordinates.LastColumnIndex = pagingCoordinates.LastColItem - 1;
            // ** RO-1180 End Block

            return pagingCoordinates;
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            Dictionary<string, int> coordinateMap = new Dictionary<string, int>();

            return coordinateMap;

        }

        /// <summary>
        /// Used to retrieve cube values
        /// </summary>
        override protected void CreateRowHeaderList()
        {
            rowHeaderList.Clear();

            ArrayList compList = new ArrayList();


            foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
            {
                if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                {
                    if (managerData._selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                    {
                        if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                        {
                            compList.Add(detailHeader);
                        }
                    }
                }
            }

            _sortedVariableHeaders = CreateSortedList(_selectableVariableHeaders);

            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
            {

                RowColProfileHeader varHeader = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)varHeader.Profile;

                if (_adjustmentRowHeader.IsDisplayed)
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));

                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, "ADJ", varProf));

                }
                else
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));
                }

                foreach (RowColProfileHeader detailHeader in compList)
                {
                    if (detailHeader.IsDisplayed)
                    {
                        if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                        {
                            CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                            rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, detailHeader.Name, varProf));
                        }
                    }
                }

                foreach (RowColProfileHeader basisHeader in managerData._selectableBasisHeaders)
                {
                    if (basisHeader.IsDisplayed)
                    {
                        CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        
                        rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.NodeProfile.Key), varProf, "Basis" + (basisHeader.Sequence + 1).ToString(), managerData.basisToolTipList[basisHeader.Sequence], basisHeader.Name, basisHeader.Sequence));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                if (basisHeader.Sequence != ((RowColProfileHeader)managerData._selectableBasisHeaders[managerData._selectableBasisHeaders.Count - 1]).Sequence ||
                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    CubeWaferCoordinateList cubeWaferCoordinateList2 = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));

                                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList2, detailHeader.Name, varProf));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Call after creating the rowHeaderList
        /// Used to retrieve cube values
        /// </summary>
        override protected void CreateColumnHeaderList()
        {
            columnHeaderList.Clear();

            //int sortedTimeEntryIndex = -1;


            foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
            {
                RowColProfileHeader timeHeader = (RowColProfileHeader)timeEntry.Value;

                //sortedTimeEntryIndex++;
                CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));

                columnHeaderList.Add(new SimpleColumnHeader(cubeWaferCoordinateList));
            }


            maxTimeTotVars = 0;

            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
            {
                RowColProfileHeader varHeader = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)varHeader.Profile;

                maxTimeTotVars = Math.Max(maxTimeTotVars, varProf.TimeTotalChainVariables.Count);
            }

            for (int t = 0; t < maxTimeTotVars; t++)
            {
                CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                cubeWaferCoordinateList.Add(managerData.SummaryDateProfile_WaferCoordinate);
                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainTimeTotalIndex, t + 1));
                columnHeaderList.Add(new SimpleColumnHeader(cubeWaferCoordinateList));
            }
        }

        override public ROData CreatePageFromViewData()
        {
            ROData ROData = new ROData();
            int baseColumnCount = 5;

            if (_years > 0)
            {
                ROCells years = new ROCells();
                AddColumns(years);
                years.AddCells(_years, years.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Year, years);
            }
            if (_seasons > 0)
            {
                ROCells seasons = new ROCells(_seasons, columnHeaderList.Count + baseColumnCount);
                AddColumns(seasons);
                seasons.AddCells(_seasons, seasons.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Season, seasons);
            }
            if (_quarters > 0)
            {
                ROCells quarters = new ROCells();
                AddColumns(quarters);
                quarters.AddCells(_quarters, quarters.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Quarter, quarters);
            }
            if (_months > 0)
            {
                ROCells months = new ROCells();
                AddColumns(months);
                months.AddCells(_months, months.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Month, months);
            }
            if (_weeks > 0)
            {
                ROCells weeks = new ROCells();
                AddColumns(weeks);
                weeks.AddCells(_weeks, weeks.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Week, weeks);
            }
            ROCells totals = new ROCells();
            AddColumns(totals);
            totals.AddCells(maxTimeTotVars * 2, totals.Columns.Count);
            ROData.AddCells(eDataType.ChainTotals, totals);

            AddValues(ROData);

            return ROData;
        }

        override public ROCubeMetadata CreateMetadataFromViewData(ROCubeGetMetadataParams metadataParams)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = "Chain Ladder Metadata Returned";
            long instanceID = metadataParams.ROInstanceID;
            eGridOrientation gridOrientation = eGridOrientation.TimeOnRow;

            List<ROCubeMetadatAttribute> cubeMetadataAttributes = new List<ROCubeMetadatAttribute>();
            ROCubeMetadatAttribute cubeMetadatAttribute1 = new ROCubeMetadatAttribute(eDataType.ChainDetail, _pagingCoordinates.TotalRowItems, 1, _pagingCoordinates.TotalColItems, _pagingCoordinates.CountDistinctColumns);
            cubeMetadataAttributes.Add(cubeMetadatAttribute1);
            ROCubeMetadatAttribute cubeMetadatAttribute2 = new ROCubeMetadatAttribute(eDataType.ChainTotals, maxTimeTotVars * 2, 1, _pagingCoordinates.TotalColItems, _pagingCoordinates.CountDistinctColumns);
            cubeMetadataAttributes.Add(cubeMetadatAttribute2);

            ROCubeMetadata rOCubeMetadata = new ROCubeMetadata(returnCode, message, instanceID);
            rOCubeMetadata.GridOrientation = gridOrientation;
            rOCubeMetadata.CubeMetadataAttributes = cubeMetadataAttributes;
            return rOCubeMetadata;
        }

        private Dictionary<string, int> AddColumns(ROCells cells)
        {
            Dictionary<string, int> columnMap = new Dictionary<string, int>();
            string columnName;
            ePlanBasisType planBasisType;
            int variableNumber;
            int quantityVariableKey;
            int timePeriodType = Include.Undefined;
            int width;

            AddColumn("ParentRowKey", cells);
            AddColumn("RowIndex", cells);
            AddColumn("RowSortIndex", cells);
            AddColumn("RowKey", cells);
            // RowKeyDisplay is the heading row
            width = GetColumnWidth();
            AddColumn("RowKeyDisplay", cells, width);

            _extraColumns = cells.Columns.Count;

            int cubeColumnIndex = 0;

            foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            {
                int varKey = 0;
                //** RO-1180 Only Add columns that are withing the current column paging requested
                if (cubeColumnIndex <=  (int)_pagingCoordinates.LastColumnIndex && cubeColumnIndex >= (int)_pagingCoordinates.FirstColumnIndex)
                    {
                    CubeWaferCoordinate coord = null;
                    switch (rowHdrTag.RowHeading.Trim())
                    {
                        case "":
                            break;

                        case "ADJ":
                            varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                            GetColumnCoordinates(
                                columnIndex: cubeColumnIndex,
                                planBasisType: out planBasisType,
                                variableNumber: out variableNumber,
                                quantityVariableKey: out quantityVariableKey
                                );
                            width = GetColumnWidth(
                                 planBasisType: (int)planBasisType,
                                 variableNumber: variableNumber,
                                 quantityVariableKey: quantityVariableKey,
                                 timePeriodType: timePeriodType
                                 );
                            AddColumn(varKey.ToString() + "~" + rowHdrTag.RowHeading, cells, width);
                            break;

                        case "% Change":
                        case "% Time Period":
                        case "% Change to Plan":
                            string displayHeading = string.Empty;
                            string[] headingParts = rowHdrTag.RowHeading.Split(new char[] { ' ' });
                            if (rowHdrTag.RowHeading.StartsWith("% Change"))
                            {
                                if (rowHdrTag.RowHeading.Trim() == "% Change")
                                {
                                    displayHeading = headingParts[0] + Environment.NewLine + headingParts[1];
                                }
                                else
                                {
                                    displayHeading = headingParts[0] + Environment.NewLine + headingParts[1] + Environment.NewLine + headingParts[2] + " " + headingParts[3];
                                }
                            }
                            else
                            {
                                displayHeading = headingParts[0] + " " + headingParts[1] + Environment.NewLine + headingParts[2];
                            }
                            varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                            coord = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);
                            GetColumnCoordinates(
                                columnIndex: cubeColumnIndex,
                                planBasisType: out planBasisType,
                                variableNumber: out variableNumber,
                                quantityVariableKey: out quantityVariableKey
                                );
                            width = GetColumnWidth(
                                 planBasisType: (int)planBasisType,
                                 variableNumber: variableNumber,
                                 quantityVariableKey: quantityVariableKey,
                                 timePeriodType: timePeriodType
                                 );
                            if (coord == null)
                            {
                                AddColumn(varKey.ToString() + "~" + displayHeading, cells, width);
                            }
                            else
                            {
                                AddColumn(varKey.ToString() + "~" + coord.Key.ToString() + " ~" + displayHeading, cells, width);
                            }
                            break;

                        default:
                            coord = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);
                            varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                            GetColumnCoordinates(
                                columnIndex: cubeColumnIndex,
                                planBasisType: out planBasisType,
                                variableNumber: out variableNumber,
                                quantityVariableKey: out quantityVariableKey
                                );
                            width = GetColumnWidth(
                                 planBasisType: (int)planBasisType,
                                 variableNumber: variableNumber,
                                 quantityVariableKey: quantityVariableKey,
                                 timePeriodType: timePeriodType
                                 );
                            if (coord == null)
                            {
                                AddColumn(rowHdrTag.RowHeading, cells, width);
                            }
                            else
                            {

                                varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                                columnName = varKey.ToString() + "@@" + rowHdrTag.RowHeading;


                                if (columnMap.ContainsKey(columnName) == true)
                                {
                                    bool alreadyExists = true;
                                    int icount = 2;
                                    while (alreadyExists)
                                    {
                                        string tempColumnName = columnName + icount.ToString();
                                        if (columnMap.ContainsKey(tempColumnName) == false)
                                        {
                                            columnName = tempColumnName;
                                            alreadyExists = false;
                                        }
                                        else
                                        {
                                            icount++;
                                        }
                                    }
                                }
                                GetColumnCoordinates(
                                columnIndex: cubeColumnIndex,
                                planBasisType: out planBasisType,
                                variableNumber: out variableNumber,
                                quantityVariableKey: out quantityVariableKey
                                );
                                width = GetColumnWidth(
                                     planBasisType: (int)planBasisType,
                                     variableNumber: variableNumber,
                                     quantityVariableKey: quantityVariableKey,
                                     timePeriodType: timePeriodType
                                     );
                                AddColumn(columnName, cells, width);
                            }
                            break;
                    }
                }
            cubeColumnIndex++;
            }

            return columnMap;
        }

        private void AddColumn(string columnName, ROCells cells, int width = Include.DefaultColumnWidth)
        {
            cells.Columns.Add(new ROColumnAttributes(columnName, cells.Columns.Count, width));
        }

        private void AddValues(ROData ROData)
        {
            _cubeValues = GetCubeValues();

            int yearRowIndex = -1;
            int seasonRowIndex = -1;
            int quarterRowIndex = -1;
            int monthRowIndex = -1;
            int weekRowIndex = -1;

            int firstRowIndex = _pagingCoordinates.FirstRowItem - 1;
            int lastRowIndex = _pagingCoordinates.LastRowItem - 1;

            RowColProfileHeader timeHeader = null;

            for (int i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);
                //** RO-1180 only create the number of rows for the paging requested
                if (i <= lastRowIndex && i >= firstRowIndex)
                {

                    switch (timeHeader.Profile.ProfileType)
                    {
                        case eProfileType.Period:
                            PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                            switch (perProf.PeriodProfileType)
                            {
                                case eProfileType.Year:
                                    YearProfile yearProf = (YearProfile)timeHeader.Profile;
                                    ++yearRowIndex;
                                    ROCells yearValues = ROData.GetCells(eDataType.ChainDetail_Year);
                                    yearValues.ROCell[yearRowIndex][yearValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    yearValues.ROCell[yearRowIndex][yearValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    yearValues.ROCell[yearRowIndex][yearValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    yearValues.ROCell[yearRowIndex][yearValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    SetCellValuesOnRow(yearValues, i, yearRowIndex);
                                    break;

                                case eProfileType.Season:
                                    SeasonProfile seasonProf = (SeasonProfile)timeHeader.Profile;
                                    ++seasonRowIndex;
                                    ROCells seasonValues = ROData.GetCells(eDataType.ChainDetail_Season);
                                    seasonValues.ROCell[seasonRowIndex][seasonValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    seasonValues.ROCell[seasonRowIndex][seasonValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    seasonValues.ROCell[seasonRowIndex][seasonValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    seasonValues.ROCell[seasonRowIndex][seasonValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    CheckForParentKey(seasonValues, seasonProf, i, seasonRowIndex);
                                    SetCellValuesOnRow(seasonValues, i, seasonRowIndex);
                                    break;

                                case eProfileType.Quarter:
                                    QuarterProfile quarterProf = (QuarterProfile)timeHeader.Profile;
                                    ++quarterRowIndex;
                                    ROCells quarterValues = ROData.GetCells(eDataType.ChainDetail_Quarter);
                                    quarterValues.ROCell[quarterRowIndex][quarterValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    quarterValues.ROCell[quarterRowIndex][quarterValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    quarterValues.ROCell[quarterRowIndex][quarterValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    quarterValues.ROCell[quarterRowIndex][quarterValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    CheckForParentKey(quarterValues, quarterProf, i, quarterRowIndex);
                                    SetCellValuesOnRow(quarterValues, i, quarterRowIndex);
                                    break;

                                case eProfileType.Month:
                                    MonthProfile monthProf = (MonthProfile)timeHeader.Profile;
                                    ++monthRowIndex;
                                    ROCells monthValues = ROData.GetCells(eDataType.ChainDetail_Month);
                                    monthValues.ROCell[monthRowIndex][monthValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    monthValues.ROCell[monthRowIndex][monthValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                                    monthValues.ROCell[monthRowIndex][monthValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    monthValues.ROCell[monthRowIndex][monthValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                                    CheckForParentKey(monthValues, monthProf, i, monthRowIndex);
                                    SetCellValuesOnRow(monthValues, i, monthRowIndex);
                                    break;
                            }

                            break;

                        case eProfileType.Week:
                            WeekProfile weekProf = (WeekProfile)timeHeader.Profile;
                            ++weekRowIndex;
                            ROCells weekValues = ROData.GetCells(eDataType.ChainDetail_Week);
                            weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                            weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                            weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                            weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, timeHeader.Name);
                            CheckForParentKey(weekValues, weekProf, i, weekRowIndex);
                            SetCellValuesOnRow(weekValues, i, weekRowIndex);
                            break;

                    }
                }
            }


            //Load totals

            ROCells totalValues = ROData.GetCells(eDataType.ChainTotals);

            //Make two rows for each of the totals. The first rows holds the name.  The second row holds the total value.
            //The total value comes from the cube.  It has a unique column coordinate list, but uses the same row coordinate list as the variable.

            int totalRowIndex = -1;
            int totalIndex = 1; // 1, 2, 3
            int currentRowIndex = _sortedTimeHeaders.Count;
            int totalRowCount = _sortedTimeHeaders.Count;
            for (int t = 0; t < maxTimeTotVars; t++)
            {
                //** RO-1180 The Total Contains All Rows but add back to change for only create the number of rows in the total array for the paging requested
                //if (t <= lastRowIndex && t >= firstRowIndex)
                //{
                    ++totalRowIndex;
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, -1);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, currentRowIndex);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, managerData.timeTotalName + totalRowCount.ToString());

                    //Set the caption "Total" on the first row only
                    if (totalIndex == 1)
                    {
                        totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, managerData.timeTotalName);
                    }
                    else
                    {
                        totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, string.Empty);
                    }

                    string prevName = string.Empty;
                    int cellColumnIndex = _extraColumns;
                    int rowHeaderIndex = 0;
                    foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
                    {
                        if (rowHeaderIndex <= (int)_pagingCoordinates.LastColumnIndex && rowHeaderIndex >= (int)_pagingCoordinates.FirstColumnIndex)
                        { 
                        VariableProfile varProf = rowHdrTag.varProf;
                            TimeTotalVariableProfile timeTotalProf = ((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(totalIndex));
                            if (timeTotalProf != null)
                            {
                                if (timeTotalProf.VariableName != prevName)
                                {
                                    prevName = timeTotalProf.VariableName;
                                    string displayName = ParseTotalVariableName(timeTotalProf.VariableName);
                                    totalValues.ROCell[totalRowIndex][cellColumnIndex] = new ROCell(eCellDataType.Coordinate, displayName);
                                }
                            }
                            cellColumnIndex++;
                        }
                        rowHeaderIndex++;
                    }

                    currentRowIndex++;

                    ++totalRowIndex;
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, totalRowCount);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, currentRowIndex);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, managerData.timeTotalName + "Value" + totalRowCount.ToString());
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, string.Empty);
                    SetCellValuesOnRow(totalValues, totalRowCount, totalRowIndex);
                    currentRowIndex++;


                    totalIndex++;
                    totalRowCount++;

                //}
            }
        }

        /// <summary>
        /// checks the grid dataset to ensure the sales and inventory unit variables are contained in this view
        /// </summary>
        /// <returns></returns>
        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();
            bool foundSalesUnitsVariable = false;
            bool foundInventoryUnitsVariable = false;

            //    for (int columnIndex = 0; columnIndex < gridDataSet.Tables[0].Columns.Count; columnIndex++)
            //    {
            //        string colName = gridDataSet.Tables[0].Columns[columnIndex].ColumnName;

            //        if (colName == salesUnitsVariableName)
            //        {
            //            foundSalesUnitsVariable = true;
            //        }
            //        if (colName == inventoryUnitsVariableName)
            //        {
            //            foundInventoryUnitsVariable = true;
            //        }

            //    }
            if (foundSalesUnitsVariable == true && foundInventoryUnitsVariable == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public DataSet CreateDatasetForChart()
        {
            this.chartDataSet = null;
            this.chartDataSet = MIDEnvironment.CreateDataSet();
            DataTable dtChartYear = new DataTable("Year");
            DataTable dtChartSeason = new DataTable("Season");
            DataTable dtChartQuarter = new DataTable("Quarter");
            DataTable dtChartMonth = new DataTable("Month");
            DataTable dtChartWeek = new DataTable("Week");

            dtChartYear.Columns.Add("RowKeyDisplay");
            dtChartSeason.Columns.Add("RowKeyDisplay");
            dtChartQuarter.Columns.Add("RowKeyDisplay");
            dtChartMonth.Columns.Add("RowKeyDisplay");
            dtChartWeek.Columns.Add("RowKeyDisplay");

            //Define the columns for chart tables
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();

            int cubeColumnIndex = 0;
            string priorVariableName = string.Empty;
            bool hasAdjustmentColumns = hasAdjustmentColumnsDisplayed();
            chartColumnRefList.Clear();
            foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            {
                bool showVariable = false;
                foreach (RowColProfileHeader rcph in _selectableVariableHeadersForChart)
                {
                    VariableProfile varProf = (VariableProfile)rcph.Profile;
                    if (rowHdrTag.varProf.VariableName == varProf.VariableName)
                    {
                        if (rcph.IsDisplayed)
                        {
                            showVariable = true;
                        }
                        break;
                    }
                }

                if (showVariable)
                {
                    switch (rowHdrTag.RowHeading.Trim())
                    {
                        case "":
                        case "ADJ":
                        case "% Change":
                        case "% Time Period":
                        case "% Change to Plan":
                            break;

                        default:
                            string columnName;

                            CubeWaferCoordinate coord = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);
                            if (coord == null) //Not a basis - just add the variable name
                            {
                                columnName = rowHdrTag.RowHeading;
                                priorVariableName = rowHdrTag.RowHeading;

                                if (hasAdjustmentColumns)
                                {
                                    //add one to get the adjustment column
                                    int adjustmentCubeIndex = cubeColumnIndex + 1;
                                    chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, adjustmentCubeIndex));
                                }
                                else
                                {
                                    chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, -1));
                                }

                            }
                            else //Basis - add the variable name + basis
                            {
                                columnName = priorVariableName + "-" + rowHdrTag.basisHeading;
                                chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, -1));
                            }


                            dtChartYear.Columns.Add(columnName, typeof(double));
                            dtChartSeason.Columns.Add(columnName, typeof(double));
                            dtChartQuarter.Columns.Add(columnName, typeof(double));
                            dtChartMonth.Columns.Add(columnName, typeof(double));
                            dtChartWeek.Columns.Add(columnName, typeof(double));
                            break;
                    }
                }
                cubeColumnIndex++;
            }

            this.chartDataSet.Tables.Add(dtChartYear);
            this.chartDataSet.Tables.Add(dtChartSeason);
            this.chartDataSet.Tables.Add(dtChartQuarter);
            this.chartDataSet.Tables.Add(dtChartMonth);
            this.chartDataSet.Tables.Add(dtChartWeek);

            for (int i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                RowColProfileHeader timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);

                switch (timeHeader.Profile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                        switch (perProf.PeriodProfileType)
                        {
                            case eProfileType.Year:
                                YearProfile yearProf = (YearProfile)timeHeader.Profile;
                                DataRow drYear = dtChartYear.NewRow();
                                drYear.SetField(dtChartYear.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartYear, drYear, i);
                                dtChartYear.Rows.Add(drYear);
                                break;

                            case eProfileType.Season:
                                SeasonProfile seasonProf = (SeasonProfile)timeHeader.Profile;
                                DataRow drSeason = dtChartSeason.NewRow();
                                drSeason.SetField(dtChartSeason.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartSeason, drSeason, i);
                                dtChartSeason.Rows.Add(drSeason);
                                break;

                            case eProfileType.Quarter:
                                QuarterProfile quarterProf = (QuarterProfile)timeHeader.Profile;
                                DataRow drQuarter = dtChartQuarter.NewRow();
                                drQuarter.SetField(dtChartQuarter.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartQuarter, drQuarter, i);
                                dtChartQuarter.Rows.Add(drQuarter);
                                break;

                            case eProfileType.Month:
                                MonthProfile monthProf = (MonthProfile)timeHeader.Profile;
                                DataRow drMonth = dtChartMonth.NewRow();
                                drMonth.SetField(dtChartMonth.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartMonth, drMonth, i);
                                dtChartMonth.Rows.Add(drMonth);
                                break;
                        }

                        break;

                    case eProfileType.Week:
                        WeekProfile weekProf = (WeekProfile)timeHeader.Profile;
                        DataRow drWeek = dtChartWeek.NewRow();
                        drWeek.SetField(dtChartWeek.Columns["RowKeyDisplay"], timeHeader.Name);
                        SetChartCellValuesOnRow(dtChartWeek, drWeek, i);
                        dtChartWeek.Rows.Add(drWeek);

                        break;
                }

            }

            return chartDataSet;

        }

        override public DataSet ReconstructDatasetForChart(ArrayList alVariables)
        {
            SortedList sortedVariablesAdjusted = new SortedList();
            SortedList sortedVariables = CreateSortedList(alVariables);
            int basisCount = managerData._selectableBasisHeaders.Count;
            int sequence = 0;
            if (basisCount > 0)
            {
                basisCount++;
                foreach (DictionaryEntry varEntry in sortedVariables)
                {
                    RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                    sequence = ((int)varEntry.Key * basisCount) + 1;        // the +1 accounts for the "RowKeyDisplay" column
                    rcph.Sequence = sequence;
                    sortedVariablesAdjusted.Add(sequence, varEntry.Value);
                }
            }
            else
            {
                foreach (DictionaryEntry varEntry in sortedVariables)
                {
                    RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                    sequence = (int)varEntry.Key + 1;                       // the +1 accounts for the "RowKeyDisplay" column
                    rcph.Sequence = sequence;
                    sortedVariablesAdjusted.Add(sequence, varEntry.Value);
                }
            }

            foreach (DictionaryEntry varEntry in sortedVariablesAdjusted)
            {
                RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)rcph.Profile;
                string varName = string.Empty;
                int basisSequence = 0;
                for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
                {
                    for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                    {
                        string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                        if (columnName != "RowKeyDisplay")
                        {
                            if (columnName.Trim() == varProf.VariableName.Trim())
                            {
                                this.chartDataSet.Tables[tableIndex].Columns[columnIndex].SetOrdinal(rcph.Sequence);
                                varName = varProf.VariableName;
                                basisSequence = rcph.Sequence;
                            }
                            else if (columnName.Trim().StartsWith(varProf.VariableName.Trim())) // basis
                            {
                                basisSequence++;
                                this.chartDataSet.Tables[tableIndex].Columns[columnIndex].SetOrdinal(basisSequence);
                            }
                        }
                    }
                }
            }
            return this.chartDataSet;
        }

        override protected void SetCellValuesOnRow(ROCells cells, int rowIndex, int rowCellIndex)
        {
            int currColumnIndex = _extraColumns -1;

            //** RO-1180 Only process the columns requested for the column paging
            for (int j = _pagingCoordinates.FirstColumnIndex + _extraColumns ; j <=  _pagingCoordinates.LastColumnIndex + _extraColumns; j++)

            //** RO-1180 Block of Original Code left for Reference
            //for (int j = _extraColumns; j < cells.Columns.Count; j++)
            {
                currColumnIndex++;
                //if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null && _cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
                if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null)
                {
                    eCellDataType cellDataType = eCellDataType.Units;
                    switch (_cubeValues[j - _extraColumns, rowIndex].VariableStyle)
                    {
                        case eVariableStyle.Dollar:
                            cellDataType = eCellDataType.Dollar;
                            break;
                        case eVariableStyle.Percentage:
                            cellDataType = eCellDataType.Percentage;
                            break;
                        case eVariableStyle.Units:
                            cellDataType = eCellDataType.Units;
                            break;
                        default:
                            cellDataType = eCellDataType.None;
                            break;
                    }
                    if (_cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
                    {
                        if (cellDataType == eCellDataType.Dollar
                            || cellDataType == eCellDataType.Percentage
                            || cellDataType == eCellDataType.Units)
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[j - _extraColumns, rowIndex].Value);
                        }
                        else
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[j - _extraColumns, rowIndex].ValueAsString);
                        }
                    }
                    else
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, null);
                    }

                    if (rowIndex != -1
                        && _cubeValues != null
                        //&& _extraColumns < _cubeValues.GetLength(0)  // causes attributes to not get set
                        && _cubeValues[j - _extraColumns, rowIndex] != null)
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.DecimalPositions = _cubeValues[j - _extraColumns, rowIndex].NumberOfDecimals;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNegative = _cubeValues[j - _extraColumns, rowIndex].isValueNegative;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNumeric = _cubeValues[j - _extraColumns, rowIndex].isValueNumeric;

                        ComputationCellFlags cellFlags = _cubeValues[j - _extraColumns, rowIndex].Flags;

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsIneligible = PlanCellFlagValues.isIneligible(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsProtected = PlanCellFlagValues.isProtected(cellFlags);

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsBasis = isRowBasis(j - _extraColumns);

                        if (PlanCellFlagValues.isClosed(cellFlags) ||
                            ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                            ComputationCellFlagValues.isNull(cellFlags) ||
                            PlanCellFlagValues.isProtected(cellFlags) ||
                            ComputationCellFlagValues.isHidden(cellFlags) ||
                            ComputationCellFlagValues.isReadOnly(cellFlags))
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = false;
                        }
                        else
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = true;
                        }
                    }
                    //** RO-1180 Block of Original Code left for Reference
                    //cells.ROCell[rowCellIndex][j] = new ROCell(eCellDataType.Dollar, _cubeValues[j - _extraColumns, rowIndex].ValueAsString);

                    //if (rowIndex != -1
                    //    && _cubeValues != null
                    //    && _extraColumns < _cubeValues.GetLength(0)
                    //    && _cubeValues[j - _extraColumns, rowIndex] != null)
                    //{
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.DecimalPositions = _cubeValues[j - _extraColumns, rowIndex].NumberOfDecimals;
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsNegative = _cubeValues[j - _extraColumns, rowIndex].isValueNegative;
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsNumeric = _cubeValues[j - _extraColumns, rowIndex].isValueNumeric;

                    //    ComputationCellFlags cellFlags = _cubeValues[j - _extraColumns, rowIndex].Flags;

                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsIneligible = PlanCellFlagValues.isIneligible(cellFlags);
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsProtected = PlanCellFlagValues.isProtected(cellFlags);

                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                    //    cells.ROCell[rowCellIndex][j].ROCellAttributes.IsBasis = isRowBasis(j - _extraColumns);

                    //    if (PlanCellFlagValues.isClosed(cellFlags) ||
                    //        ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                    //        ComputationCellFlagValues.isNull(cellFlags) ||
                    //        PlanCellFlagValues.isProtected(cellFlags) ||
                    //        ComputationCellFlagValues.isHidden(cellFlags) ||
                    //        ComputationCellFlagValues.isReadOnly(cellFlags))
                    //    {
                    //        cells.ROCell[rowCellIndex][j].ROCellAttributes.IsEditable = false;
                    //    }
                    //    else
                    //    {
                    //        cells.ROCell[rowCellIndex][j].ROCellAttributes.IsEditable = true;
                    //    }
                    //}
                    // *** RO-1180 End Block
                }
            }
        }


        override protected void SetCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex)
        {
            for (int j = _extraColumns; j < dt.Columns.Count; j++)
            {
                if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null && _cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
                {
                     dr.SetField(dt.Columns[j], _cubeValues[j - _extraColumns, rowIndex].ValueAsString);
                }
            }
        }
        override protected void SetChartCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex)
        {
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                chartColumnRef cref = chartColumnRefList.Find(delegate (chartColumnRef c) { return c.columnName == dt.Columns[j].ColumnName; });
                int cubeColumnIndex = cref.cubeColumnIndex;
                int cubeColumnADJ_Index = cref.cubeColumnADJ_Index;


                //if the adjustment variable ADJ is visisble and has a value - use that value, otherwise use the "normal" value
                Double dblAdjustmentValue = -1;
                bool hasAdjustmentValue = false;
                if (cubeColumnADJ_Index != -1)
                {
                    if (cubeColumnADJ_Index >= 0 && _cubeValues != null && _cubeValues[cubeColumnADJ_Index, rowIndex] != null && _cubeValues[cubeColumnADJ_Index, rowIndex].isValueNumeric)
                    {
                        dblAdjustmentValue = Convert.ToDouble(_cubeValues[cubeColumnADJ_Index, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);
                        hasAdjustmentValue = true;
                    }
                }

                if (hasAdjustmentValue)
                {
                    dr.SetField(dt.Columns[j], dblAdjustmentValue);
                }
                else
                {
                    if (cubeColumnIndex >= 0 && _cubeValues != null && _cubeValues[cubeColumnIndex, rowIndex] != null && _cubeValues[cubeColumnIndex, rowIndex].isValueNumeric)
                    {
                        Double dblValue = Convert.ToDouble(_cubeValues[cubeColumnIndex, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);
                        dr.SetField(dt.Columns[j], dblValue);
                    }
                    else
                    {
                        Double dblValue = 0;
                        dr.SetField(dt.Columns[j], dblValue);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the coordinates of the column 
        /// </summary>
        /// <param name="columnIndex">The position of the column</param>
        /// <param name="planBasisType">Output: Is plan or basis</param>
        /// <param name="variableNumber">Output: Variable Number</param>
        /// <param name="quantityVariableKey">Output: Indicates if value or comparative</param>
        private void GetColumnCoordinates(
            int columnIndex, 
            out ePlanBasisType planBasisType,
            out int variableNumber,
            out int quantityVariableKey
            )
        {
            // data is rotated.  So rows are columns and columns are rows.
            CubeWaferCoordinateList columnCoordinateList;
            CubeWaferCoordinateList rowCoordinateList = GetColumnCubeWaferCoordinateList(columnIndex: 0);

            columnCoordinateList = GetRowCubeWaferCoordinateList(rowIndex: columnIndex);

            // type is plan if no basis coordinate exists
            planBasisType = ePlanBasisType.Plan;
            if (columnCoordinateList.FindCoordinateType(eProfileType.Basis) != null)
            {
                planBasisType = ePlanBasisType.Basis;
            }
            variableNumber = columnCoordinateList.FindCoordinateType(eProfileType.Variable).Key;
            // type is value and not comparative if no QuantityVariable coordinate exists
            quantityVariableKey = 1;
            if (columnCoordinateList.FindCoordinateType(eProfileType.QuantityVariable) != null)
            {
                quantityVariableKey = columnCoordinateList.FindCoordinateType(eProfileType.QuantityVariable).Key;
            }
        }

        /// <summary>
        /// Save column format information to the database
        /// </summary>
        /// <param name="ROViewFormatParms">
        /// An instance of the ROViewFormatParms containing view formatting</param>
        /// <returns></returns>
        override public ROOut SaveViewFormat(ROViewFormatParms ROViewFormatParms)
        {
            // data is rotated.  So rows are columns and columns are rows.
            PlanViewData planViewData;
            
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = null;
            bool success = true;
            ePlanBasisType planBasisType;
            int variableNumber;
            int quantityVariableKey;
            int timePeriodType = Include.Undefined; // This orientation does not have time as a column

            // create data layer to communicate with the database
            planViewData = new PlanViewData();

            try
            {
                planViewData.OpenUpdateConnection();
                
                //add format for heading column
                //heading column has all coordinates undefined
                planViewData.PlanViewFormat_Insert(
                    aViewRID: ViewRID,
                    planBasisType: Include.Undefined,
                    variableNumber: Include.Undefined,
                    quantityVariableKey: Include.Undefined,
                    timePeriodType: Include.Undefined,
                    timePeriodKey: Include.Undefined,
                    variableTotalKey: Include.Undefined,
                    width: ROViewFormatParms.HeadingColumn.Width
                    );

                // add each column formatting
                foreach (ROColumnFormat columnFormat in ROViewFormatParms.ColumnFormats)
                {
                    GetColumnCoordinates(
                        columnIndex: columnFormat.ColumnIndex,
                        planBasisType: out planBasisType,
                        variableNumber: out variableNumber,
                        quantityVariableKey: out quantityVariableKey
                        );

                    // if row exists, width will be updated
					planViewData.PlanViewFormat_Insert(
                        aViewRID: ViewRID,
                        planBasisType: (int)planBasisType,
                        variableNumber: variableNumber,
                        quantityVariableKey: quantityVariableKey,
                        timePeriodType: timePeriodType,
                        width: columnFormat.Width
                        );

                }
                // commit the new values to the database
                planViewData.CommitData();
            }
            catch (Exception exc)
            {
                // something went wrong.  Roll back all values inserted to the database
                planViewData.Rollback();
                message = exc.ToString();
                success = false;
                returnCode = eROReturnCode.Failure;
            }
            finally
            {
                // close the database connection and update the flag so the view data will be rebuilt during the next data access
                planViewData.CloseUpdateConnection();
                ViewUpdated = true;
            } 

            return new ROBoolOut(returnCode, message, ROViewFormatParms.ROInstanceID, success);
        }
    }

    /// <summary>
    /// Data that is defined per view
    /// </summary>
    public class ROChainSingleLevelPeriodViewData : ROChainSingleLevelViewData
    {
        public ROChainSingleLevelPeriodViewData(int viewRID, ref ROPlanChainSingleLevelManagerData managerData)
            : base(viewRID, ref managerData)
        {
            this.managerData = managerData;



            int i;
            VariableProfile viewVarProf;
            QuantityVariableProfile viewQVarProf;
            DataRow viewRow;
            Hashtable varKeyHash;
            Hashtable perKeyHash;

            Hashtable qVarKeyHash;
            bool cont;

            try
            {
                //Read PlanViewDetail table
                if (managerData._planViewDataLayer == null)
                {
                    managerData._planViewDataLayer = new PlanViewData();
                }
                // Get view details and load view formatting.
                DataTable _planViewDetail = GetViewDetails(viewKey: viewRID);



                varKeyHash = new Hashtable();
                _selectableVariableHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Variable)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Variable)
                        {
                            viewVarProf = (VariableProfile)managerData._variableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewVarProf != null)
                            {
                                varKeyHash.Add(viewVarProf.Key, row);
                            }
                        }
                    }
                }

                foreach (VariableProfile variableProf in managerData._variableProfileList)
                {
                    viewRow = (DataRow)varKeyHash[variableProf.Key];
                    if (viewRow != null)
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, true, Convert.ToInt32(viewRow["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), variableProf, variableProf.Groupings));
                    }
                    else
                    {
                        _selectableVariableHeaders.Add(new RowColProfileHeader(variableProf.VariableName, false, -1, variableProf, variableProf.Groupings));
                    }
                }

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.QuantityVariable)
                        {
                            viewQVarProf = (QuantityVariableProfile)managerData._quantityVariableProfileList.FindKey(Convert.ToInt32(row["PROFILE_KEY"], CultureInfo.CurrentUICulture));

                            if (viewQVarProf != null)
                            {
                                qVarKeyHash.Add(viewQVarProf.Key, row);
                            }
                        }
                    }
                }

                _quantityVariables = managerData.Transaction.PlanComputations.PlanQuantityVariables;

                _currentRowHeader = new RowColProfileHeader("Current Plan", true, 0, _quantityVariables.ValueQuantity);
                _originalRowHeader = new RowColProfileHeader("Original Plan", false, 1, _quantityVariables.ValueQuantity);

                viewRow = (DataRow)qVarKeyHash[_quantityVariables.ValueQuantity.Key];

                if (viewRow != null)
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", true, 0, _quantityVariables.ValueQuantity);
                }
                else
                {
                    _adjustmentRowHeader = new RowColProfileHeader("Adjusted Plan", false, 0, _quantityVariables.ValueQuantity);
                }

                _selectableQuantityHeaders.Add(_adjustmentRowHeader);
                i = 2;

                foreach (QuantityVariableProfile qVarProf in managerData._quantityVariableProfileList)
                {
                    cont = false;


                    if (qVarProf.isChainSingleView && qVarProf.isHighLevel &&
                        qVarProf.isChainDetailCube)
                    {
                        cont = true;
                    }


                    if (qVarProf.isSelectable && cont)
                    {
                        viewRow = (DataRow)qVarKeyHash[qVarProf.Key];
                        if (viewRow != null)
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, true, i, qVarProf));
                        }
                        else
                        {
                            _selectableQuantityHeaders.Add(new RowColProfileHeader(qVarProf.VariableName, false, i, qVarProf));
                        }
                        i++;
                    }
                }


                // Load Periods

                perKeyHash = new Hashtable();
                _selectablePeriodHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Period)
                    {
                        if (Convert.ToInt32(row["PROFILE_TYPE"], CultureInfo.CurrentUICulture) == (int)eProfileType.Period)
                        {
                            perKeyHash.Add(Convert.ToInt32(row["AXIS_SEQUENCE"], CultureInfo.CurrentUICulture), row);
                        }
                    }
                }

                selectMonth = (DataRow)perKeyHash[(int)eProfileType.Month] != null;
                selectWeek = (DataRow)perKeyHash[(int)eProfileType.Week] != null;


                selectYear = (DataRow)perKeyHash[(int)eProfileType.Year] != null;
                selectSeason = (DataRow)perKeyHash[(int)eProfileType.Season] != null;
                selectQuarter = (DataRow)perKeyHash[(int)eProfileType.Quarter] != null;

                CreateSelectablePeriodHeaders();


                _periodHeaderHash = CreatePeriodHash();
                BuildTimeHeaders();

                CreateRowHeaderList();
                CreateColumnHeaderList();
                BuildVariableNameArrayList();

            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        override public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        {
            int i, first, last;

            PagingCoordinates pagingCoordinates = new PagingCoordinates();

            _startingRowIndex = iStartingRowIndex;
            _numberOfRows = iNumberOfRows;
            _startingColIndex = iStartingColIndex;
            _numberOfColumns = iNumberOfColumns;

            first = _startingColIndex;
            last = first + _numberOfColumns;

            eProfileType highestPeriodType = eProfileType.None;
            int periodCount = 0;
            int selectedPeriodCount = 0;
            if (selectYear)
            {
                highestPeriodType = eProfileType.Year;
            }
            else if (selectSeason)
            {
                highestPeriodType = eProfileType.Season;
            }
            else if (selectQuarter)
            {
                highestPeriodType = eProfileType.Quarter;
            }
            else if (selectMonth)
            {
                highestPeriodType = eProfileType.Month;
            }
            else if (selectWeek)
            {
                highestPeriodType = eProfileType.Week;
            }

            _years = 0;
            _seasons = 0;
            _quarters = 0;
            _months = 0;
            _weeks = 0;
            bool selectTimePeriod = false;
            for (i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                RowColProfileHeader rowColHeader = (RowColProfileHeader)_sortedTimeHeaders[i];
                selectTimePeriod = false;
                
                if (rowColHeader.Profile is YearProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is SeasonProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is QuarterProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is MonthProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }
                else if (rowColHeader.Profile is WeekProfile)
                {
                    ++periodCount;
                    if (periodCount > first
                        && periodCount <= last)
                    {
                        ++selectedPeriodCount;
                        selectTimePeriod = true;
                    }
                    else
                    {
                        selectTimePeriod = false;
                    }
                }



                if (selectTimePeriod)
                {
                    if (rowColHeader.Profile is YearProfile)
                    {
                        ++_years;
                    }
                    else if (rowColHeader.Profile is SeasonProfile)
                    {
                        ++_seasons;
                    }
                    else if (rowColHeader.Profile is QuarterProfile)
                    {
                        ++_quarters;
                    }
                    else if (rowColHeader.Profile is MonthProfile)
                    {
                        ++_months;
                    }
                    else if (rowColHeader.Profile is WeekProfile)
                    {
                        ++_weeks;
                    }
                }
            }

            if (selectedPeriodCount == 0)
            {
                pagingCoordinates.FirstColItem = selectedPeriodCount;
            }
            else
            {
                pagingCoordinates.FirstColItem = first + 1;
            }
            if (selectedPeriodCount == 0)
            {
                pagingCoordinates.LastColItem = selectedPeriodCount;
            }
            else
            {
                pagingCoordinates.LastColItem = first + selectedPeriodCount;
            }
            pagingCoordinates.TotalColItems = periodCount;
            pagingCoordinates.FirstColumnIndex = pagingCoordinates.FirstColItem - 1;
            pagingCoordinates.LastColumnIndex = pagingCoordinates.LastColItem - 1;
            
            first = _startingRowIndex;
            last = first + _numberOfRows;

            if (first >= rowHeaderList.Count)
            {
                pagingCoordinates.FirstRowItem = rowHeaderList.Count;
            }
            else
            {
                pagingCoordinates.FirstRowItem = first + 1;
            }
            if (last >= rowHeaderList.Count)
            {
                pagingCoordinates.LastRowItem = rowHeaderList.Count;
            }
            else
            {
                pagingCoordinates.LastRowItem = last;
            }
            pagingCoordinates.TotalRowItems = rowHeaderList.Count;

            // For time on column orientation these become row coordinates for the variables
            if (rowHeaderList.Count > 0 && _sortedVariableHeaders.Count > 0)
            {
                pagingCoordinates.CountDistinctColumns = rowHeaderList.Count / _sortedVariableHeaders.Count;
            }
            else
            {
                pagingCoordinates.CountDistinctColumns = 1;
            }
            pagingCoordinates.ColumnsSelected = pagingCoordinates.LastRowItem - pagingCoordinates.FirstRowItem + 1;
            //
                       
            return pagingCoordinates;
        }

        override public Dictionary<string, int> GetColumnCoordinateMap()
        {
            Dictionary<string, int> coordinateMap = new Dictionary<string, int>();

            return coordinateMap;

        }

        /// <summary>
        /// Used to retrieve cube values
        /// </summary>
        override protected void CreateRowHeaderList()
        {
            rowHeaderList.Clear();

            ArrayList compList = new ArrayList();


            foreach (RowColProfileHeader detailHeader in _selectableQuantityHeaders)
            {
                if (detailHeader.Profile.Key != _quantityVariables.ValueQuantity.Key)
                {
                    if (managerData._selectableBasisHeaders.Count > 0 || detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                    {
                        if (((QuantityVariableProfile)detailHeader.Profile).isChainDetailCube && ((QuantityVariableProfile)detailHeader.Profile).isHighLevel)
                        {
                            compList.Add(detailHeader);
                        }
                    }
                }
            }

            _sortedVariableHeaders = CreateSortedList(_selectableVariableHeaders);

            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
            {

                RowColProfileHeader varHeader = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)varHeader.Profile;

                if (_adjustmentRowHeader.IsDisplayed)
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));

                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, "ADJ", varProf));

                }
                else
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));
                }

                foreach (RowColProfileHeader detailHeader in compList)
                {
                    if (detailHeader.IsDisplayed)
                    {
                        if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                        {
                            CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                            rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, detailHeader.Name, varProf));
                        }
                    }
                }

                foreach (RowColProfileHeader basisHeader in managerData._selectableBasisHeaders)
                {
                    if (basisHeader.IsDisplayed)
                    {
                        CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));

                        rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.NodeProfile.Key), varProf, "Basis" + (basisHeader.Sequence + 1).ToString(), managerData.basisToolTipList[basisHeader.Sequence], basisHeader.Name, basisHeader.Sequence));

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                if (basisHeader.Sequence != ((RowColProfileHeader)managerData._selectableBasisHeaders[managerData._selectableBasisHeaders.Count - 1]).Sequence ||
                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    CubeWaferCoordinateList cubeWaferCoordinateList2 = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));

                                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList2, detailHeader.Name, varProf));
                                }
                            }
                        }
                    }
                }
            }
           
        }

        /// <summary>
        /// Call after creating the rowHeaderList
        /// Used to retrieve cube values
        /// </summary>
        override protected void CreateColumnHeaderList()
        {
            columnHeaderList.Clear();

            //int sortedTimeEntryIndex = -1;


            foreach (DictionaryEntry timeEntry in _sortedTimeHeaders)
            {
                RowColProfileHeader timeHeader = (RowColProfileHeader)timeEntry.Value;

                //sortedTimeEntryIndex++;
                CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(timeHeader.Profile.ProfileType, timeHeader.Profile.Key));

                columnHeaderList.Add(new SimpleColumnHeader(cubeWaferCoordinateList));
            }


           maxTimeTotVars = 0;

            foreach (DictionaryEntry varEntry in _sortedVariableHeaders)
            {
                RowColProfileHeader varHeader = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)varHeader.Profile;

                maxTimeTotVars = Math.Max(maxTimeTotVars, varProf.TimeTotalChainVariables.Count);
            }

            for (int t = 0; t < maxTimeTotVars; t++)
            {
                CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                cubeWaferCoordinateList.Add(managerData.SummaryDateProfile_WaferCoordinate);
                cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainTimeTotalIndex, t + 1));
                columnHeaderList.Add(new SimpleColumnHeader(cubeWaferCoordinateList));
            }
        }

        override public ROData CreatePageFromViewData()
        {
            ROData ROData = new ROData();
            int baseColumnCount = 5;

            if (_sortedTimeHeaders.Count > 0)
            {
                //add values for weeks detail grid
                ROCells weeks = new ROCells();
                AddColumns(weeks);
                //weeks.AddCells(rowHeaderList.Count * 2, _sortedTimeHeaders.Count + baseColumnCount);
                weeks.AddCells(_pagingCoordinates.ColumnsSelected, weeks.Columns.Count);
                ROData.AddCells(eDataType.ChainDetail_Week, weeks);
                
                //add values fort he same rows for the totals grid
                ROCells totals = new ROCells();
                AddTotalColumns(totals);
                //rows should match same as above
                totals.AddCells(_pagingCoordinates.ColumnsSelected, totals.Columns.Count);
                ROData.AddCells(eDataType.ChainTotals, totals);

                //Add cube values for both weeks and totals details
                AddValues(ROData);
            }

            return ROData;
        }

        override public ROCubeMetadata CreateMetadataFromViewData(ROCubeGetMetadataParams metadataParams)
        {
            eROReturnCode returnCode = eROReturnCode.Successful;
            string message = "Chain Period Metadata Returned";
            long instanceID = metadataParams.ROInstanceID;
            eGridOrientation gridOrientation = eGridOrientation.TimeOnColumn;

            List<ROCubeMetadatAttribute> cubeMetadataAttributes = new List<ROCubeMetadatAttribute>();
            ROCubeMetadatAttribute cubeMetadatAttribute1 = new ROCubeMetadatAttribute(eDataType.ChainDetail, _pagingCoordinates.TotalRowItems, _pagingCoordinates.CountDistinctColumns, _pagingCoordinates.TotalColItems, 1);
            cubeMetadataAttributes.Add(cubeMetadatAttribute1);
            ROCubeMetadatAttribute cubeMetadatAttribute2 = new ROCubeMetadatAttribute(eDataType.ChainTotals, _pagingCoordinates.TotalRowItems, _pagingCoordinates.CountDistinctColumns,  maxTimeTotVars * 2, 1);
            cubeMetadataAttributes.Add(cubeMetadatAttribute2);

            ROCubeMetadata rOCubeMetadata = new ROCubeMetadata(returnCode, message, instanceID);
            rOCubeMetadata.GridOrientation = gridOrientation;
            rOCubeMetadata.CubeMetadataAttributes = cubeMetadataAttributes;
            return rOCubeMetadata;
        }
        private Dictionary<string, int> AddColumns(ROCells cells)
        {
            Dictionary<string, int> columnMap = new Dictionary<string, int>();
            string columnName;
            ePlanBasisType planBasisType;
            int variableNumber;
            int quantityVariableKey;
            int timePeriodType = Include.Undefined;
            int width;

            AddColumn("ParentRowKey", cells);
            AddColumn("RowIndex", cells);
            AddColumn("RowSortIndex", cells);
            AddColumn("RowKey", cells);
            // RowKeyDisplay is the heading row
            width = GetColumnWidth();
            AddColumn("RowKeyDisplay", cells, width);

            _extraColumns = cells.Columns.Count;

            int cubeColumnIndex = 0;
            int firstColumnIndex = _pagingCoordinates.FirstColItem - 1;
            int lastColumnIndex = _pagingCoordinates.LastColItem - 1;

            RowColProfileHeader timeHeader = null;

            for (int i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);
                //PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                // Add columns that are within the current column paging requested
                if (cubeColumnIndex <= lastColumnIndex && cubeColumnIndex >= firstColumnIndex)
                {
                    columnName = timeHeader.Name;
                    if (columnMap.ContainsKey(columnName) == true)
                    {
                        bool alreadyExists = true;
                        int icount = 2;
                        while (alreadyExists)
                        {
                            string tempColumnName = columnName + icount.ToString();
                            if (columnMap.ContainsKey(tempColumnName) == false)
                            {
                                columnName = tempColumnName;
                                alreadyExists = false;
                            }
                            else
                            {
                                icount++;
                            }
                        }
                    }
                    AddColumn(columnName, cells);
                }
                cubeColumnIndex++;
            }

            //foreach (RowColProfileHeader timeHeader in _sortedTimeHeaders)
            //{
            //    AddColumn(timeHeader.Name, cells);
            //}



            return columnMap;
        }

        private Dictionary<string, int> AddTotalColumns(ROCells cells)
        {
            Dictionary<string, int> columnMap = new Dictionary<string, int>();
            string columnName;
            ePlanBasisType planBasisType;
            int variableNumber;
            int quantityVariableKey;
            int timePeriodType = Include.Undefined;
            int width;

            AddColumn("ParentRowKey", cells);
            AddColumn("RowIndex", cells);
            AddColumn("RowSortIndex", cells);
            AddColumn("RowKey", cells);
            // RowKeyDisplay is the heading row
            width = GetColumnWidth();
            AddColumn("RowKeyDisplay", cells, width);

            _extraColumns = cells.Columns.Count;

            int cubeColumnIndex = 0;

            //for totals grid two columns for each time tot variable (first is description, 2nd is total)
            for (int i = 0; i < maxTimeTotVars * 2; i++)
            {
                AddColumn("Total" + i.ToString(), cells);
            }

            return columnMap;
        }

        private void AddColumn(string columnName, ROCells cells, int width = Include.DefaultColumnWidth)
        {
            cells.Columns.Add(new ROColumnAttributes(columnName, cells.Columns.Count, width));
        }

        private void AddValues(ROData ROData)
        {
            _cubeValues = GetCubeValues();
            int weekRowIndex = -1;

            int firstRowIndex = _pagingCoordinates.FirstRowItem - 1;
            int lastRowIndex = _pagingCoordinates.LastRowItem - 1;
            SimpleRowHeader rowHeader = null;
            RowColProfileHeader variableHeader = null;
            VariableProfile variableProfile = null;

            for (int i = 0; i < rowHeaderList.Count; i++)
            {
                rowHeader = (SimpleRowHeader)rowHeaderList[i];
                if (i <= lastRowIndex && i >= firstRowIndex)
                {
                    ++weekRowIndex;
                    ROCells weekValues = ROData.GetCells(eDataType.ChainDetail_Week);
                    weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, i);
                    weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, i);
                    weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, rowHeader.RowHeading);
                    weekValues.ROCell[weekRowIndex][weekValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, rowHeader.RowHeading);
                    //CheckForParentKey(weekValues, weekProf, i, weekRowIndex);
                    SetCellValuesOnRow(weekValues, i, weekRowIndex);
                }
            }


            //Load totals

            ROCells totalValues = ROData.GetCells(eDataType.ChainTotals);

            //For Chain Ladder view
            //Make two rows for each of the totals. The first rows holds the name.  The second row holds the total value.
            //The total value comes from the cube.  It has a unique column coordinate list, but uses the same row coordinate list as the variable.

            //for Chain Ladder Period View make same number of rows as the detail grid 
            //make two coolumns for each time tot variable total, the first column holds the description and the second column holds the total value

            int totalRowIndex = -1;
            int totalIndex = 1; // 1, 2, 3
            string prevName = string.Empty;

            for (int i = 0; i < rowHeaderList.Count; i++)
            {
                rowHeader = (SimpleRowHeader)rowHeaderList[i];
                // make a row for each row in header list
                if (i <= lastRowIndex && i >= firstRowIndex)
                {
                    ++totalRowIndex;
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowIndex")] = new ROCell(eCellDataType.Coordinate, totalRowIndex);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowSortIndex")] = new ROCell(eCellDataType.Coordinate, totalRowIndex);
                    totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKey")] = new ROCell(eCellDataType.Coordinate, managerData.timeTotalName + totalRowIndex.ToString());

                    //Set the caption "Total" on the first row only
                    if (totalRowIndex == firstRowIndex)
                    {
                        totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, managerData.timeTotalName);
                    }
                    else
                    {
                        totalValues.ROCell[totalRowIndex][totalValues.GetIndexOfColumn("RowKeyDisplay")] = new ROCell(eCellDataType.Coordinate, string.Empty);
                    }

                    // make two columns for each maxtimetotvars
                    prevName = string.Empty;
                    totalIndex = 1;
                    int cellColumnIndex = _extraColumns;
                    for (int t = 0; t < maxTimeTotVars; t++)
                    {
                        VariableProfile varProf = rowHeader.varProf;
                        TimeTotalVariableProfile timeTotalProf = ((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(totalIndex));
                        if (timeTotalProf != null)
                        {
                            if (timeTotalProf.VariableName != prevName)
                            {
                                prevName = timeTotalProf.VariableName;
                                string displayName = ParseTotalVariableName(timeTotalProf.VariableName);
                                totalValues.ROCell[totalRowIndex][cellColumnIndex] = new ROCell(eCellDataType.Coordinate, displayName);
                            }
                        }
                        cellColumnIndex++; // skip the next column used for the value
                        cellColumnIndex++;
                        ++totalIndex;
                    }
                    SetTotalCellValuesOnRow(totalValues, totalRowIndex, totalRowIndex);
                }
            }

        }

        protected void SetTotalCellValuesOnRow(ROCells cells, int rowIndex, int rowCellIndex)
        {
            int currColumnIndex = _extraColumns - 1;
            //J is the index into the cube data
            for (int j = columnHeaderList.Count - maxTimeTotVars ; j < columnHeaderList.Count; j++)
            {
                currColumnIndex++;
                currColumnIndex++;
                if (_cubeValues != null && _cubeValues[rowIndex, j] != null)
                {
                    eCellDataType cellDataType = eCellDataType.Units;
                    switch (_cubeValues[rowIndex, j].VariableStyle)
                    {
                        case eVariableStyle.Dollar:
                            cellDataType = eCellDataType.Dollar;
                            break;
                        case eVariableStyle.Percentage:
                            cellDataType = eCellDataType.Percentage;
                            break;
                        case eVariableStyle.Units:
                            cellDataType = eCellDataType.Units;
                            break;
                        default:
                            cellDataType = eCellDataType.None;
                            break;
                    }
                    if (cellDataType == eCellDataType.Dollar
                        || cellDataType == eCellDataType.Percentage
                        || cellDataType == eCellDataType.Units)
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[rowIndex, j].Value);
                    }
                    else
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[rowIndex, j].ValueAsString);
                    }

                    if (rowIndex != -1
                        && _cubeValues != null
                        //&& _extraColumns < _cubeValues.GetLength(0)
                        && _cubeValues[rowIndex, j] != null)
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.DecimalPositions = _cubeValues[rowIndex, j ].NumberOfDecimals;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNegative = _cubeValues[rowIndex, j ].isValueNegative;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNumeric = _cubeValues[rowIndex, j ].isValueNumeric;

                        ComputationCellFlags cellFlags = _cubeValues[rowIndex, j].Flags;

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsIneligible = PlanCellFlagValues.isIneligible(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsProtected = PlanCellFlagValues.isProtected(cellFlags);

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsBasis = isRowBasis(rowIndex);

                        if (PlanCellFlagValues.isClosed(cellFlags) ||
                            ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                            ComputationCellFlagValues.isNull(cellFlags) ||
                            PlanCellFlagValues.isProtected(cellFlags) ||
                            ComputationCellFlagValues.isHidden(cellFlags) ||
                            ComputationCellFlagValues.isReadOnly(cellFlags))
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = false;
                        }
                        else
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = true;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// checks the grid dataset to ensure the sales and inventory unit variables are contained in this view
        /// </summary>
        /// <returns></returns>
        override public bool DoesDataSetContainInventoryUnitVariables()
        {
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();
            bool foundSalesUnitsVariable = false;
            bool foundInventoryUnitsVariable = false;

            //    for (int columnIndex = 0; columnIndex < gridDataSet.Tables[0].Columns.Count; columnIndex++)
            //    {
            //        string colName = gridDataSet.Tables[0].Columns[columnIndex].ColumnName;

            //        if (colName == salesUnitsVariableName)
            //        {
            //            foundSalesUnitsVariable = true;
            //        }
            //        if (colName == inventoryUnitsVariableName)
            //        {
            //            foundInventoryUnitsVariable = true;
            //        }

            //    }
            if (foundSalesUnitsVariable == true && foundInventoryUnitsVariable == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        override public DataSet CreateDatasetForChart()
        {
            this.chartDataSet = null;
            this.chartDataSet = MIDEnvironment.CreateDataSet();
            DataTable dtChartYear = new DataTable("Year");
            DataTable dtChartSeason = new DataTable("Season");
            DataTable dtChartQuarter = new DataTable("Quarter");
            DataTable dtChartMonth = new DataTable("Month");
            DataTable dtChartWeek = new DataTable("Week");

            dtChartYear.Columns.Add("RowKeyDisplay");
            dtChartSeason.Columns.Add("RowKeyDisplay");
            dtChartQuarter.Columns.Add("RowKeyDisplay");
            dtChartMonth.Columns.Add("RowKeyDisplay");
            dtChartWeek.Columns.Add("RowKeyDisplay");

            //Define the columns for chart tables
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();

            int cubeColumnIndex = 0;
            string priorVariableName = string.Empty;
            bool hasAdjustmentColumns = hasAdjustmentColumnsDisplayed();
            chartColumnRefList.Clear();
            foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            {
                bool showVariable = false;
                foreach (RowColProfileHeader rcph in _selectableVariableHeadersForChart)
                {
                    VariableProfile varProf = (VariableProfile)rcph.Profile;
                    if (rowHdrTag.varProf.VariableName == varProf.VariableName)
                    {
                        if (rcph.IsDisplayed)
                        {
                            showVariable = true;
                        }
                        break;
                    }
                }

                if (showVariable)
                {
                    switch (rowHdrTag.RowHeading.Trim())
                    {
                        case "":
                        case "ADJ":
                        case "% Change":
                        case "% Time Period":
                        case "% Change to Plan":
                            break;

                        default:
                            string columnName;

                            CubeWaferCoordinate coord = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);
                            if (coord == null) //Not a basis - just add the variable name
                            {
                                columnName = rowHdrTag.RowHeading;
                                priorVariableName = rowHdrTag.RowHeading;

                                if (hasAdjustmentColumns)
                                {
                                    //add one to get the adjustment column
                                    int adjustmentCubeIndex = cubeColumnIndex + 1;
                                    chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, adjustmentCubeIndex));
                                }
                                else
                                {
                                    chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, -1));
                                }

                            }
                            else //Basis - add the variable name + basis
                            {
                                columnName = priorVariableName + "-" + rowHdrTag.basisHeading;
                                chartColumnRefList.Add(new chartColumnRef(columnName, cubeColumnIndex, -1));
                            }


                            dtChartYear.Columns.Add(columnName, typeof(double));
                            dtChartSeason.Columns.Add(columnName, typeof(double));
                            dtChartQuarter.Columns.Add(columnName, typeof(double));
                            dtChartMonth.Columns.Add(columnName, typeof(double));
                            dtChartWeek.Columns.Add(columnName, typeof(double));
                            break;
                    }
                }
                cubeColumnIndex++;
            }

            this.chartDataSet.Tables.Add(dtChartYear);
            this.chartDataSet.Tables.Add(dtChartSeason);
            this.chartDataSet.Tables.Add(dtChartQuarter);
            this.chartDataSet.Tables.Add(dtChartMonth);
            this.chartDataSet.Tables.Add(dtChartWeek);

            for (int i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                RowColProfileHeader timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);

                switch (timeHeader.Profile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                        switch (perProf.PeriodProfileType)
                        {
                            case eProfileType.Year:
                                YearProfile yearProf = (YearProfile)timeHeader.Profile;
                                DataRow drYear = dtChartYear.NewRow();
                                drYear.SetField(dtChartYear.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartYear, drYear, i);
                                dtChartYear.Rows.Add(drYear);
                                break;

                            case eProfileType.Season:
                                SeasonProfile seasonProf = (SeasonProfile)timeHeader.Profile;
                                DataRow drSeason = dtChartSeason.NewRow();
                                drSeason.SetField(dtChartSeason.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartSeason, drSeason, i);
                                dtChartSeason.Rows.Add(drSeason);
                                break;

                            case eProfileType.Quarter:
                                QuarterProfile quarterProf = (QuarterProfile)timeHeader.Profile;
                                DataRow drQuarter = dtChartQuarter.NewRow();
                                drQuarter.SetField(dtChartQuarter.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartQuarter, drQuarter, i);
                                dtChartQuarter.Rows.Add(drQuarter);
                                break;

                            case eProfileType.Month:
                                MonthProfile monthProf = (MonthProfile)timeHeader.Profile;
                                DataRow drMonth = dtChartMonth.NewRow();
                                drMonth.SetField(dtChartMonth.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartMonth, drMonth, i);
                                dtChartMonth.Rows.Add(drMonth);
                                break;
                        }

                        break;

                    case eProfileType.Week:
                        WeekProfile weekProf = (WeekProfile)timeHeader.Profile;
                        DataRow drWeek = dtChartWeek.NewRow();
                        drWeek.SetField(dtChartWeek.Columns["RowKeyDisplay"], timeHeader.Name);
                        SetChartCellValuesOnRow(dtChartWeek, drWeek, i);
                        dtChartWeek.Rows.Add(drWeek);

                        break;
                }

            }

            return chartDataSet;

        }

        override public DataSet ReconstructDatasetForChart(ArrayList alVariables)
        {
            SortedList sortedVariablesAdjusted = new SortedList();
            SortedList sortedVariables = CreateSortedList(alVariables);
            int basisCount = managerData._selectableBasisHeaders.Count;
            int sequence = 0;
            if (basisCount > 0)
            {
                basisCount++;
                foreach (DictionaryEntry varEntry in sortedVariables)
                {
                    RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                    sequence = ((int)varEntry.Key * basisCount) + 1;        // the +1 accounts for the "RowKeyDisplay" column
                    rcph.Sequence = sequence;
                    sortedVariablesAdjusted.Add(sequence, varEntry.Value);
                }
            }
            else
            {
                foreach (DictionaryEntry varEntry in sortedVariables)
                {
                    RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                    sequence = (int)varEntry.Key + 1;                       // the +1 accounts for the "RowKeyDisplay" column
                    rcph.Sequence = sequence;
                    sortedVariablesAdjusted.Add(sequence, varEntry.Value);
                }
            }

            foreach (DictionaryEntry varEntry in sortedVariablesAdjusted)
            {
                RowColProfileHeader rcph = (RowColProfileHeader)varEntry.Value;
                VariableProfile varProf = (VariableProfile)rcph.Profile;
                string varName = string.Empty;
                int basisSequence = 0;
                for (int tableIndex = 0; tableIndex < this.chartDataSet.Tables.Count; tableIndex++)
                {
                    for (int columnIndex = 0; columnIndex < this.chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
                    {
                        string columnName = this.chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;
                        if (columnName != "RowKeyDisplay")
                        {
                            if (columnName.Trim() == varProf.VariableName.Trim())
                            {
                                this.chartDataSet.Tables[tableIndex].Columns[columnIndex].SetOrdinal(rcph.Sequence);
                                varName = varProf.VariableName;
                                basisSequence = rcph.Sequence;
                            }
                            else if (columnName.Trim().StartsWith(varProf.VariableName.Trim())) // basis
                            {
                                basisSequence++;
                                this.chartDataSet.Tables[tableIndex].Columns[columnIndex].SetOrdinal(basisSequence);
                            }
                        }
                    }
                }
            }
            return this.chartDataSet;
        }

        override protected void SetCellValuesOnRow(ROCells cells, int rowIndex, int rowCellIndex)
        {
            int currColumnIndex = _extraColumns - 1;
            int firstColumnIndex = _pagingCoordinates.FirstColItem - 1;
            int lastColumnIndex = _pagingCoordinates.LastColItem - 1;

            //for (int j = _pagingCoordinates.FirstColumnIndex + _extraColumns; j < _pagingCoordinates.LastColumnIndex + _extraColumns; j++)
            for (int j = firstColumnIndex + _extraColumns; j <= lastColumnIndex + _extraColumns; j++)
            {
                    currColumnIndex++;
                    //if (_cubeValues != null && _cubeValues[rowIndex, j - _extraColumns] != null && _cubeValues[rowIndex, j - _extraColumns].isValueNumeric)
                    if (_cubeValues != null && _cubeValues[rowIndex, j - _extraColumns] != null)
                    {
                    eCellDataType cellDataType = eCellDataType.Units;
                    switch (_cubeValues[rowIndex, j - _extraColumns].VariableStyle)
                    {
                        case eVariableStyle.Dollar:
                            cellDataType = eCellDataType.Dollar;
                            break;
                        case eVariableStyle.Percentage:
                            cellDataType = eCellDataType.Percentage;
                            break;
                        case eVariableStyle.Units:
                            cellDataType = eCellDataType.Units;
                            break;
                        default:
                            cellDataType = eCellDataType.None;
                            break;
                    }
                    if (cellDataType == eCellDataType.Dollar
                        || cellDataType == eCellDataType.Percentage
                        || cellDataType == eCellDataType.Units)
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[rowIndex, j - _extraColumns].Value);
                    }
                    else
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex] = new ROCell(cellDataType, _cubeValues[rowIndex, j - _extraColumns].ValueAsString);
                    }

                    if (rowIndex != -1
                        && _cubeValues != null
                    //&& _extraColumns < _cubeValues.GetLength(0)
                    //&& _cubeValues[j - _extraColumns, rowIndex] != null)
                        && _cubeValues[rowIndex, j - _extraColumns] != null)
                    {
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.DecimalPositions = _cubeValues[rowIndex, j - _extraColumns].NumberOfDecimals;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNegative = _cubeValues[rowIndex, j - _extraColumns].isValueNegative;
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsNumeric = _cubeValues[rowIndex, j - _extraColumns].isValueNumeric;

                        ComputationCellFlags cellFlags = _cubeValues[rowIndex, j - _extraColumns].Flags;

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsIneligible = PlanCellFlagValues.isIneligible(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsProtected = PlanCellFlagValues.isProtected(cellFlags);

                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsLocked = ComputationCellFlagValues.isLocked(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsDisplayOnly = ComputationCellFlagValues.isDisplayOnly(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsModified = ComputationCellFlagValues.isChanged(cellFlags);
                        cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsBasis = isRowBasis(rowIndex);

                        if (PlanCellFlagValues.isClosed(cellFlags) ||
                            ComputationCellFlagValues.isDisplayOnly(cellFlags) ||
                            ComputationCellFlagValues.isNull(cellFlags) ||
                            PlanCellFlagValues.isProtected(cellFlags) ||
                            ComputationCellFlagValues.isHidden(cellFlags) ||
                            ComputationCellFlagValues.isReadOnly(cellFlags))
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = false;
                        }
                        else
                        {
                            cells.ROCell[rowCellIndex][currColumnIndex].ROCellAttributes.IsEditable = true;
                        }
                    }

                }
            }
        }


        override protected void SetCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex)
        {
            for (int j = _extraColumns; j < dt.Columns.Count; j++)
            {
                if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null && _cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
                {
                    dr.SetField(dt.Columns[j], _cubeValues[j - _extraColumns, rowIndex].ValueAsString);
                }
            }
        }

        override protected void SetChartCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex)
        {
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                chartColumnRef cref = chartColumnRefList.Find(delegate (chartColumnRef c) { return c.columnName == dt.Columns[j].ColumnName; });
                int cubeColumnIndex = cref.cubeColumnIndex;
                int cubeColumnADJ_Index = cref.cubeColumnADJ_Index;


                //if the adjustment variable ADJ is visisble and has a value - use that value, otherwise use the "normal" value
                Double dblAdjustmentValue = -1;
                bool hasAdjustmentValue = false;
                if (cubeColumnADJ_Index != -1)
                {
                    if (cubeColumnADJ_Index >= 0 && _cubeValues != null && _cubeValues[cubeColumnADJ_Index, rowIndex] != null && _cubeValues[cubeColumnADJ_Index, rowIndex].isValueNumeric)
                    {
                        dblAdjustmentValue = Convert.ToDouble(_cubeValues[cubeColumnADJ_Index, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);
                        hasAdjustmentValue = true;
                    }
                }

                if (hasAdjustmentValue)
                {
                    dr.SetField(dt.Columns[j], dblAdjustmentValue);
                }
                else
                {
                    if (cubeColumnIndex >= 0 && _cubeValues != null && _cubeValues[cubeColumnIndex, rowIndex] != null && _cubeValues[cubeColumnIndex, rowIndex].isValueNumeric)
                    {
                        Double dblValue = Convert.ToDouble(_cubeValues[cubeColumnIndex, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);
                        dr.SetField(dt.Columns[j], dblValue);
                    }
                    else
                    {
                        Double dblValue = 0;
                        dr.SetField(dt.Columns[j], dblValue);
                    }
                }
            }
        }

        // Stub method for inheritance to be completed later
		override public ROOut SaveViewFormat(ROViewFormatParms ROViewFormatParms)
        {
            throw new Exception("SaveViewFormat method not implemented");
        }
    }
}


    
