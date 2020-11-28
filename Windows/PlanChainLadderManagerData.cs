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

namespace MIDRetail.Windows
{
    /// <summary>
    /// Data that is defined at the manager level and used in the view
    /// This data is set only once per instance of the ladder screen
    /// </summary>
    public class LadderManagerData
    {
        public PlanCubeGroup _planCubeGroup;
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
        public ApplicationSessionTransaction _transaction;
        public bool _chainReadOnly;
        public CubeWaferCoordinateList _commonWaferCoordinateList;
        public CubeWaferCoordinate SummaryDateProfile_WaferCoordinate; //used in period/time wafer coordinate
        public string dollarScalingString = "1";
        public string unitsScalingString = "1";
        public string timeTotalName;

       
       
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
            //_cmiBasisList = new ArrayList();
      

            if (BasisProfileList.Count > 0)
            {
                //basisCmiSeparator = new ToolStripSeparator();
                //cmsg4g7g10.Items.Add(basisCmiSeparator);
                //_cmiBasisList.Add(basisCmiSeparator);

                for (int i = 0; i < BasisProfileList.Count; i++)
                {

                    BasisProfile bp = (BasisProfile)BasisProfileList[i];
                    BasisDetailProfile bdp = (BasisDetailProfile)bp.BasisDetailProfileList[0];

                    string lblName = GetBasisLabel(bp.Key, bdp.HierarchyNodeProfile.Key);
                    if (lblName != null)
                    {
                        bp.Name = lblName;
                    }


                    //basisCmiItem = new ToolStripMenuItem();
                    //Begin Track #5779 - JScott - Right Click Menu for Labels
                    _selectableBasisHeaders.Add(new RowColProfileHeader(bp.Name, true, i, bp));
                    //End Track #5779 - JScott - Right Click Menu for Labels
                    // basisCmiItem.Text = bp.Name;
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
    public class LadderViewData
    {
        public DataSet gridDataSet = null;
        public DataSet chartDataSet = null;
        public LadderManagerData managerData; //this gets passed in and is set once per instance of the screen

        private IPlanComputationQuantityVariables _quantityVariables;

        private RowColProfileHeader _adjustmentRowHeader;
        private RowColProfileHeader _originalRowHeader;
        private RowColProfileHeader _currentRowHeader;

        public ArrayList _selectableQuantityHeaders;
        public ArrayList _selectableVariableHeaders;
        public ArrayList _selectableVariableHeadersForChart;  // TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        private ArrayList _selectableTimeHeaders;

        public ArrayList _selectablePeriodHeaders;
        public SortedList _sortedVariableHeaders;
        private SortedList _sortedTimeHeaders;
        public Hashtable _periodHeaderHash;

        private List<SimpleRowHeader> rowHeaderList = new List<SimpleRowHeader>();
        private List<SimpleColumnHeader> columnHeaderList = new List<SimpleColumnHeader>();

        public PlanWaferCell[,] _cubeValues;

        private int maxTimeTotVars;

        private ArrayList _variableNameParts;
        private int _maxBandDepth;

        public bool selectYear;
        public bool selectSeason;
        public bool selectQuarter;
        public bool selectMonth;
        public bool selectWeek;

        //public List<BasisToolTip> basisToolTips = new List<BasisToolTip>();

        public LadderViewData(int viewRID, ref LadderManagerData managerData)
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
                DataTable _planViewDetail = managerData._planViewDataLayer.PlanViewDetail_Read(viewRID);

               

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



                //if (_sortedVariableHeaders.Count == 0)
                //{
                //    MessageBox.Show(MIDText.GetTextOnly(eMIDTextCode.msg_pl_NoDisplayableVariables), "No Displayable Variables", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}

          

                qVarKeyHash = new Hashtable();
                _selectableQuantityHeaders = new ArrayList();

                foreach (DataRow row in _planViewDetail.Rows)
                {
                    //Begin Track #5121 - JScott - Add Year/Season/Quarter totals
                    //if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Row)
                    if (Convert.ToInt32(row["AXIS"], CultureInfo.CurrentUICulture) == (int)eViewAxis.Quantity)
                    //End Track #5121 - JScott - Add Year/Season/Quarter totals
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

                _quantityVariables = managerData._transaction.PlanComputations.PlanQuantityVariables;

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

        /// <summary>
        /// Used to retrieve cube values
        /// </summary>
        private void CreateRowHeaderList()
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

                //RowColProfileHeader groupHeader = new RowColProfileHeader(varProf.VariableName, false, 0, varProf);

                //cubeWaferCoordinateList = new CubeWaferCoordinateList();
                //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, new RowColProfileHeader("Original Plan", false, 2, null), rowHeaderList.Count, " ", varProf.VariableName, false));

                if (_adjustmentRowHeader.IsDisplayed)
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    //End Track #5006 - JScott - Display Low-levels one at a time
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.PostInit));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _originalRowHeader, rowHeaderList.Count, varProf.VariableName, varProf.VariableName));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));

                    cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    //End Track #5006 - JScott - Display Low-levels one at a time
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Adjusted));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _adjustmentRowHeader, rowHeaderList.Count, "ADJ", varProf.VariableName + "|ADJ"));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, "ADJ", varProf));
                   
                }
                else
                {
                    CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                    //End Track #5006 - JScott - Display Low-levels one at a time
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                    cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                    //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, rowHeaderList.Count, varProf.VariableName, varProf.VariableName));
                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, varProf.VariableName, varProf));
                }

                foreach (RowColProfileHeader detailHeader in compList)
                {
                    if (detailHeader.IsDisplayed)
                    {
                        //Begin Track #6010 - JScott - Bad % Change on Basis2
                        if (detailHeader.Profile.Key != _quantityVariables.PctChangeToPlanQuantity.Key)
                        {
                            //End Track #6010 - JScott - Bad % Change on Basis2
                            CubeWaferCoordinateList cubeWaferCoordinateList = new CubeWaferCoordinateList();
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                            //Begin Track #5006 - JScott - Display Low-levels one at a time
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                            //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                            //End Track #5006 - JScott - Display Low-levels one at a time
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                            cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                            //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, rowHeaderList.Count, detailHeader.Name, varProf.VariableName + "|" + detailHeader.Name));
                            rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, detailHeader.Name, varProf));
                            //Begin Track #6010 - JScott - Bad % Change on Basis2
                        }
                        //End Track #6010 - JScott - Bad % Change on Basis2
                    }
                }

                foreach (RowColProfileHeader basisHeader in managerData._selectableBasisHeaders)
                {
                    if (basisHeader.IsDisplayed)
                    {
                        //Begin Track #5648 - JScott - Export Option from OTS Forecast Review Scrren
                        //groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, null);
                        //groupHeader = new RowColProfileHeader("Chain " + basisHeader.Name, true, 0, varProf);
                        //End Track #5648 - JScott - Export Option from OTS Forecast Review Scrren

                 

                        CubeWaferCoordinateList  cubeWaferCoordinateList = new CubeWaferCoordinateList();
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                        //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                        //End Track #5006 - JScott - Display Low-levels one at a time
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                        cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, _quantityVariables.ValueQuantity.Key));
                        //Begin Track #5006 - JScott - Display Low-levels one at a time
                        ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                        ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, ((BasisProfile)basisHeader.Profile).Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name));
                        //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, gridRowList.Count, GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.NodeProfile.Key)));
                        ////End Track #5782
                        //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, _currentRowHeader, rowHeaderList.Count, managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.NodeProfile.Key), varProf.VariableName + "|" + managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.NodeProfile.Key)));


                       
                        rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList, managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.NodeProfile.Key), varProf, "Basis" + (basisHeader.Sequence + 1).ToString(), managerData.basisToolTipList[basisHeader.Sequence], basisHeader.Name, basisHeader.Sequence));

                       // basisToolTips.Add(new BasisToolTip(rowHeaderList.Count -1, managerData.basisToolTipList[basisHeader.Sequence]));
                        //End Track #5006 - JScott - Display Low-levels one at a time

                        foreach (RowColProfileHeader detailHeader in compList)
                        {
                            if (detailHeader.IsDisplayed)
                            {
                                if (basisHeader.Sequence != ((RowColProfileHeader)managerData._selectableBasisHeaders[managerData._selectableBasisHeaders.Count - 1]).Sequence ||
                                    detailHeader.Profile.Key != _quantityVariables.PctChangeQuantity.Key)
                                {
                                    CubeWaferCoordinateList cubeWaferCoordinateList2 = new CubeWaferCoordinateList();
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.ChainPlan, 0));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, _openParms.ChainHLPlanProfile.NodeProfile.Key));
                                    //cubeWaferCoordinateList.Add(new CubeWaferCoordinate(eProfileType.Version, _openParms.ChainHLPlanProfile.VersionProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.HierarchyNode, managerData._currentChainPlanProfile.NodeProfile.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Version, managerData._currentChainPlanProfile.VersionProfile.Key));
                                    //End Track #5006 - JScott - Display Low-levels one at a time
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Basis, ((BasisProfile)basisHeader.Profile).Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.PlanValue, (int)ePlanValueType.Current));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.Variable, varProf.Key));
                                    cubeWaferCoordinateList2.Add(new CubeWaferCoordinate(eProfileType.QuantityVariable, detailHeader.Profile.Key));
                                    //Begin Track #5006 - JScott - Display Low-levels one at a time
                                    ////Begin Track #5782 - KJohnson - Basis Label not showing the right stuff
                                    ////gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + ((BasisProfile)basisHeader.Profile).Name + "|" + detailHeader.Name));
                                    //gridRowList.Add(new RowHeaderTag(cubeWaferCoordinateList, groupHeader, detailHeader, gridRowList.Count, detailHeader.Name, varProf.VariableName + "|" + GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, _openParms.ChainHLPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));
                                    ////End Track #5782
                                    //rowHeaderList.Add(new RowHeaderTag(cubeWaferCoordinateList2, groupHeader, detailHeader, rowHeaderList.Count, detailHeader.Name, varProf.VariableName + "|" + managerData.GetBasisLabel(((BasisProfile)basisHeader.Profile).Key, managerData._currentChainPlanProfile.VersionProfile.Key) + "|" + detailHeader.Name));

         

                                    rowHeaderList.Add(new SimpleRowHeader(cubeWaferCoordinateList2, detailHeader.Name, varProf));
                                    //End Track #5006 - JScott - Display Low-levels one at a time
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
        private void CreateColumnHeaderList()
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

  
        private SortedList CreateSortedList(ArrayList aSelectableList)
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

        public DataSet ReconstructCubeCoordinatesAndDataset()
        {
            CreateRowHeaderList();
            CreateColumnHeaderList();
            this.gridDataSet = null;
            return CreateDataSetFromLadderViewData(); 
        }

        public void UpdateGridDataSet()
        {
            try
            {
                //reset the values from the cube onto the existing dataset
                _cubeValues = GetCubeValues();

                for (int tableIndex = 0; tableIndex < this.gridDataSet.Tables.Count; tableIndex++)
                {
                    for (int rowIndex = 0; rowIndex < this.gridDataSet.Tables[tableIndex].Rows.Count; rowIndex++)
                    {
                        int cubeRowIndex = -1;
                        int.TryParse(this.gridDataSet.Tables[tableIndex].Rows[rowIndex]["RowIndex"].ToString(), out cubeRowIndex);
                    
                        if (cubeRowIndex != -1)
                        {
                            SetCellValuesOnRow(this.gridDataSet.Tables[tableIndex], this.gridDataSet.Tables[tableIndex].Rows[rowIndex], cubeRowIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }
        }

        private void BuildVariableNameArrayList()
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

    

        public DataSet CreateDataSetFromLadderViewData()
        {
            this.gridDataSet = MIDEnvironment.CreateDataSet();
            DataTable dtYear = new DataTable("Year");
            DataTable dtSeason = new DataTable();
            DataTable dtQuarter = new DataTable();
            DataTable dtMonth = new DataTable();
            DataTable dtWeek = new DataTable();

            //Add extra columns
            _extraColumns = 5;
            dtYear.Columns.Add("ParentRowKey");
            dtYear.Columns.Add("RowIndex");
            dtYear.Columns.Add("RowSortIndex",System.Type.GetType("System.Int32"));
            dtYear.Columns.Add("RowKey");
            dtYear.Columns.Add("RowKeyDisplay");

           

            int cubeColumnIndex = 0;
        
            foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            {
                int varKey = 0;
                CubeWaferCoordinate coord = null;
                switch (rowHdrTag.RowHeading.Trim())
                {
                    case "":
                        break;

                    case "ADJ":
                        varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                        dtYear.Columns.Add(varKey.ToString() + "~" + rowHdrTag.RowHeading);
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
                        if (coord == null)
                        {
                            //dtYear.Columns.Add(varKey.ToString() + "~" + rowHdrTag.RowHeading);
                            dtYear.Columns.Add(varKey.ToString() + "~" + displayHeading);
                        }
                        else
                        {
                            //dtYear.Columns.Add(varKey.ToString() + "~" + coord.Key.ToString() + " ~" + rowHdrTag.RowHeading);
                            dtYear.Columns.Add(varKey.ToString() + "~" + coord.Key.ToString() + " ~" + displayHeading);
                        }
                        break;

                    default:
                        coord = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Basis);
                        if (coord == null)
                        {
                            dtYear.Columns.Add(rowHdrTag.RowHeading);
                        }
                        else
                        {

                            varKey = rowHdrTag.CubeWaferCoorList.FindCoordinateType(eProfileType.Variable).Key;
                            string columnName = varKey.ToString() + "@@" + rowHdrTag.RowHeading;
                            

                            if (dtYear.Columns.Contains(columnName) == true)
                            {
                                bool alreadyExists = true;
                                int icount = 2;
                                while (alreadyExists)
                                {
                                    string tempColumnName = columnName + icount.ToString();
                                    if (dtYear.Columns.Contains(tempColumnName) == false)
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
                            dtYear.Columns.Add(columnName);

                            dtYear.Columns[dtYear.Columns.Count - 1].Caption = rowHdrTag.basisHeading + "<TOOLTIP>" + rowHdrTag.basisToolTip;

                            //basisToolTips.Add(new BasisToolTip(dtYear.Columns.Count - 1, rowHdrTag.basisToolTip, rowHdrTag.basisHeaderName, rowHdrTag.basisHeaderSequence));
                            
                        }
                        break;
                }
                cubeColumnIndex++;
            }
            dtSeason = dtYear.Clone();
            dtSeason.TableName = "Season";
            dtQuarter = dtYear.Clone();
            dtQuarter.TableName = "Quarter";
            dtMonth = dtYear.Clone();
            dtMonth.TableName = "Month";
            dtWeek = dtYear.Clone();
            dtWeek.TableName = "Week";

            this.gridDataSet.Tables.Add(dtYear);
            this.gridDataSet.Tables.Add(dtSeason);
            this.gridDataSet.Tables.Add(dtQuarter);
            this.gridDataSet.Tables.Add(dtMonth);
            this.gridDataSet.Tables.Add(dtWeek);

            AddDataSetRelations();




            _cubeValues = GetCubeValues();


            RowColProfileHeader timeHeader = null;

            for (int i = 0; i < _sortedTimeHeaders.Count; i++)
            {
                timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);



                switch (timeHeader.Profile.ProfileType)
                {
                    case eProfileType.Period:
                        PeriodProfile perProf = (PeriodProfile)timeHeader.Profile;
                        switch (perProf.PeriodProfileType)
                        {
                            case eProfileType.Year:
                                YearProfile yearProf = (YearProfile)timeHeader.Profile;
                                DataRow drYear = dtYear.NewRow();
                                drYear.SetField(dtYear.Columns["RowIndex"], i);
                                drYear.SetField(dtYear.Columns["RowSortIndex"], i);
                                drYear.SetField(dtYear.Columns["RowKey"], timeHeader.Name);
                                drYear.SetField(dtYear.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetCellValuesOnRow(dtYear, drYear, i);
                                dtYear.Rows.Add(drYear);
                                break;

                            case eProfileType.Season:
                                SeasonProfile seasonProf = (SeasonProfile)timeHeader.Profile;
                                DataRow drSeason = dtSeason.NewRow();
                                drSeason.SetField(dtSeason.Columns["RowIndex"], i);
                                drSeason.SetField(dtSeason.Columns["RowSortIndex"], i);
                                drSeason.SetField(dtSeason.Columns["RowKey"], timeHeader.Name);
                                drSeason.SetField(dtSeason.Columns["RowKeyDisplay"], timeHeader.Name);
                                CheckForParentKey(drSeason, seasonProf, i);
                                SetCellValuesOnRow(dtSeason, drSeason, i);
                                dtSeason.Rows.Add(drSeason);
                                break;

                            case eProfileType.Quarter:
                                QuarterProfile quarterProf = (QuarterProfile)timeHeader.Profile;
                                DataRow drQuarter = dtQuarter.NewRow();
                                drQuarter.SetField(dtQuarter.Columns["RowIndex"], i);
                                drQuarter.SetField(dtQuarter.Columns["RowSortIndex"], i);
                                drQuarter.SetField(dtQuarter.Columns["RowKey"], timeHeader.Name);
                                drQuarter.SetField(dtQuarter.Columns["RowKeyDisplay"], timeHeader.Name);
                                CheckForParentKey(drQuarter, quarterProf, i);
                                SetCellValuesOnRow(dtQuarter, drQuarter, i);
                                dtQuarter.Rows.Add(drQuarter);
                                break;

                            case eProfileType.Month:
                                MonthProfile monthProf = (MonthProfile)timeHeader.Profile;
                                DataRow drMonth = dtMonth.NewRow();
                                drMonth.SetField(dtMonth.Columns["RowIndex"], i);
                                drMonth.SetField(dtMonth.Columns["RowSortIndex"], i);
                                drMonth.SetField(dtMonth.Columns["RowKey"], timeHeader.Name);
                                drMonth.SetField(dtMonth.Columns["RowKeyDisplay"], timeHeader.Name);
                                CheckForParentKey(drMonth, monthProf, i);
                                SetCellValuesOnRow(dtMonth, drMonth, i);
                                dtMonth.Rows.Add(drMonth);
                                break;
                        }
                        CheckDataSetMember(gridDataSet, perProf.PeriodProfileType);
                        break;

                    case eProfileType.Week:
                        WeekProfile weekProf = (WeekProfile)timeHeader.Profile;
                        DataRow drWeek = dtWeek.NewRow();
                        drWeek.SetField(dtWeek.Columns["RowIndex"], i);
                        drWeek.SetField(dtWeek.Columns["RowSortIndex"], i);
                        drWeek.SetField(dtWeek.Columns["RowKey"], timeHeader.Name);
                        drWeek.SetField(dtWeek.Columns["RowKeyDisplay"], timeHeader.Name);
                        CheckForParentKey(drWeek, weekProf, i);
                        SetCellValuesOnRow(dtWeek, drWeek, i);
                        dtWeek.Rows.Add(drWeek);
                        CheckDataSetMember(gridDataSet, weekProf.ProfileType);
                        break;
                }

            }


            //Load totals

            string datamember = "";
            foreach (string member in gridDataSet.ExtendedProperties.Keys)
            {
                datamember = member;
            }
            DataTable dtTotal;  //Set to outer band
            if (datamember == "Year")
            {
                dtTotal = dtYear;
            }
            else if (datamember == "Season")
            {
                dtTotal = dtSeason;
            }
            else if (datamember == "Quarter")
            {
                dtTotal = dtQuarter;
            }
            else if (datamember == "Month")
            {
                dtTotal = dtMonth;
            }
            else //if (datamember == "Week")
            {
                dtTotal = dtWeek;
            }

            //Make two rows for each of the totals. The first rows holds the name.  The second row holds the total value.
            //The total value comes from the cube.  It has a unique column coordinate list, but uses the same row coordinate list as the variable.
            int totalRowIndex = _sortedTimeHeaders.Count;
            int totalIndex = 1; // 1, 2, 3
            int currentRowIndex = totalRowIndex;
            for (int t = totalRowIndex; t < totalRowIndex + maxTimeTotVars; t++)
            {
                DataRow drTotalName = dtTotal.NewRow();
                drTotalName.SetField(dtTotal.Columns["RowIndex"], -1);
                drTotalName.SetField(dtTotal.Columns["RowSortIndex"], currentRowIndex);
                drTotalName.SetField(dtTotal.Columns["RowKey"], managerData.timeTotalName + t.ToString());
                //Set the caption "Total" on the first row only
                if (totalIndex == 1)
                {
                    drTotalName.SetField(dtTotal.Columns["RowKeyDisplay"], managerData.timeTotalName);
                }
                else
                {
                    drTotalName.SetField(dtTotal.Columns["RowKeyDisplay"], string.Empty);
                }

                string prevName = string.Empty;
                int totalColumnIndex = _extraColumns;
                foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
                {
                    VariableProfile varProf = rowHdrTag.varProf;//(VariableProfile)((RowColProfileHeader)(rowHdrTag).GroupRowColHeader).Profile;
                    TimeTotalVariableProfile timeTotalProf = ((TimeTotalVariableProfile)varProf.GetChainTimeTotalVariable(totalIndex));
                    if (timeTotalProf != null)
                    {
                         //drTotalName.SetField(dtTotal.Columns[totalColumnIndex], timeTotalProf.VariableName);
                        if (timeTotalProf.VariableName != prevName)
                        {
                            prevName = timeTotalProf.VariableName;
                            string displayName = ParseTotalVariableName(timeTotalProf.VariableName);
                            drTotalName.SetField(dtTotal.Columns[totalColumnIndex], displayName);
                        }
                    }
                    totalColumnIndex++;
                }
                dtTotal.Rows.Add(drTotalName);
                currentRowIndex++;
                
                DataRow drTotalValue = dtTotal.NewRow();
                drTotalValue.SetField(dtTotal.Columns["RowIndex"], t);
                drTotalValue.SetField(dtTotal.Columns["RowSortIndex"], currentRowIndex);
                drTotalValue.SetField(dtTotal.Columns["RowKey"], managerData.timeTotalName + "Value" + t.ToString());
                drTotalValue.SetField(dtTotal.Columns["RowKeyDisplay"], string.Empty);
                SetCellValuesOnRow(dtTotal, drTotalValue, t);
                dtTotal.Rows.Add(drTotalValue);
                currentRowIndex++;


                totalIndex++;
            }

            return this.gridDataSet;
        }


        

        /// <summary>
        /// used to get the sales units variable name for the chart
        /// </summary>
        /// <returns></returns>
        public string GetSalesUnitsVariableName()
        {
            string salesUnitsVariableName;

            if (isChainNodePlanLevelEqualTotal() == true)
            {
                salesUnitsVariableName = managerData._transaction.PlanComputations.PlanVariables.SalesTotalUnitsVariable.VariableName; //"Sales";
            }
            else
            {
                salesUnitsVariableName = managerData._transaction.PlanComputations.PlanVariables.SalesRegPromoUnitsVariable.VariableName; //"Sales R/P";
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
                inventoryUnitsVariableName = managerData._transaction.PlanComputations.PlanVariables.InventoryTotalUnitsVariable.VariableName; //"Stock";
            }
            else
            {
                inventoryUnitsVariableName = managerData._transaction.PlanComputations.PlanVariables.InventoryRegularUnitsVariable.VariableName; //"Stock Reg";
            }
            return inventoryUnitsVariableName;
        }

        /// <summary>
        /// used to help determine the sales units variable name and the inventory units variable name
        /// </summary>
        /// <returns></returns>
        private bool isChainNodePlanLevelEqualTotal()
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
        public bool DoesDataSetContainInventoryUnitVariables()
        {
            string salesUnitsVariableName = GetSalesUnitsVariableName();
            string inventoryUnitsVariableName = GetInventoryUnitsVariableName();
            bool foundSalesUnitsVariable = false;
            bool foundInventoryUnitsVariable = false;

            for (int columnIndex = 0; columnIndex < gridDataSet.Tables[0].Columns.Count; columnIndex++)
            {
                string colName = gridDataSet.Tables[0].Columns[columnIndex].ColumnName;

                if (colName == salesUnitsVariableName)
                {
                    foundSalesUnitsVariable = true;
                }
                if (colName == inventoryUnitsVariableName)
                {
                    foundInventoryUnitsVariable = true;
                }

            }
            if (foundSalesUnitsVariable == true && foundInventoryUnitsVariable == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private class chartColumnRef
        {
            public string columnName;
            public int cubeColumnIndex;
            public int cubeColumnADJ_Index;

            public chartColumnRef(string columnName, int cubeColumnIndex, int cubeColumnADJ_Index)
            {
                this.columnName = columnName;
                this.cubeColumnIndex = cubeColumnIndex;
                this.cubeColumnADJ_Index = cubeColumnADJ_Index;
            }
        }

        private bool hasAdjustmentColumnsDisplayed()
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

        private List<chartColumnRef> chartColumnRefList = new List<chartColumnRef>();
        public DataSet CreateDatasetForChart()
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
                // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
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
                
                //if (rowHdrTag.varProf.VariableName == salesUnitsVariableName || rowHdrTag.varProf.VariableName == inventoryUnitsVariableName)
                if (showVariable)
                // End TT#1748-MD 
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
                                //drYear.SetField(dtYear.Columns["RowIndex"], i);
                                //drYear.SetField(dtYear.Columns["RowSortIndex"], i);
                                //drYear.SetField(dtYear.Columns["RowKey"], timeHeader.Name);
                                drYear.SetField(dtChartYear.Columns["RowKeyDisplay"], timeHeader.Name);
                                SetChartCellValuesOnRow(dtChartYear, drYear, i);
                                dtChartYear.Rows.Add(drYear);
                                break;

                            case eProfileType.Season:
                                SeasonProfile seasonProf = (SeasonProfile)timeHeader.Profile;
                                DataRow drSeason = dtChartSeason.NewRow();
                                //drSeason.SetField(dtSeason.Columns["RowIndex"], i);
                                //drSeason.SetField(dtSeason.Columns["RowSortIndex"], i);
                                //drSeason.SetField(dtSeason.Columns["RowKey"], timeHeader.Name);
                                drSeason.SetField(dtChartSeason.Columns["RowKeyDisplay"], timeHeader.Name);
                                //CheckForParentKey(drSeason, seasonProf, i);
                                SetChartCellValuesOnRow(dtChartSeason, drSeason, i);
                                dtChartSeason.Rows.Add(drSeason);
                                break;

                            case eProfileType.Quarter:
                                QuarterProfile quarterProf = (QuarterProfile)timeHeader.Profile;
                                DataRow drQuarter = dtChartQuarter.NewRow();
                                //drQuarter.SetField(dtQuarter.Columns["RowIndex"], i);
                                //drQuarter.SetField(dtQuarter.Columns["RowSortIndex"], i);
                                //drQuarter.SetField(dtQuarter.Columns["RowKey"], timeHeader.Name);
                                drQuarter.SetField(dtChartQuarter.Columns["RowKeyDisplay"], timeHeader.Name);
                                //CheckForParentKey(drQuarter, quarterProf, i);
                                SetChartCellValuesOnRow(dtChartQuarter, drQuarter, i);
                                dtChartQuarter.Rows.Add(drQuarter);
                                break;

                            case eProfileType.Month:
                                MonthProfile monthProf = (MonthProfile)timeHeader.Profile;
                                DataRow drMonth = dtChartMonth.NewRow();
                                //drMonth.SetField(dtMonth.Columns["RowIndex"], i);
                                //drMonth.SetField(dtMonth.Columns["RowSortIndex"], i);
                                //drMonth.SetField(dtMonth.Columns["RowKey"], timeHeader.Name);
                                drMonth.SetField(dtChartMonth.Columns["RowKeyDisplay"], timeHeader.Name);
                                //CheckForParentKey(drMonth, monthProf, i);
                                SetChartCellValuesOnRow(dtChartMonth, drMonth, i);
                                dtChartMonth.Rows.Add(drMonth);
                                break;
                        }
                        //CheckDataSetMember(gridDataSet, perProf.PeriodProfileType);
                        break;

                    case eProfileType.Week:
                        WeekProfile weekProf = (WeekProfile)timeHeader.Profile;
                        DataRow drWeek = dtChartWeek.NewRow();
                        //drWeek.SetField(dtWeek.Columns["RowIndex"], i);
                        //drWeek.SetField(dtWeek.Columns["RowSortIndex"], i);
                        //drWeek.SetField(dtWeek.Columns["RowKey"], timeHeader.Name);
                        drWeek.SetField(dtChartWeek.Columns["RowKeyDisplay"], timeHeader.Name);
                        //CheckForParentKey(drWeek, weekProf, i);
                        SetChartCellValuesOnRow(dtChartWeek, drWeek, i);
                        dtChartWeek.Rows.Add(drWeek);
                        //CheckDataSetMember(gridDataSet, weekProf.ProfileType);
                        break;
                }

            }


            
           

            //DataSet chartDataSet = this.gridDataSet.Copy();
            //chartDataSet.Relations.Clear();
            //chartDataSet.EnforceConstraints = false;
            ////foreach (SimpleRowHeader rowHdrTag in rowHeaderList)
            ////{
            ////    if (rowHdrTag.varProf.VariableName == "Sales R/P")

            ////    for (int j = _extraColumns; j < dt.Columns.Count; j++)
            ////    {
            ////        if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null && _cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
            ////        {
            ////            Double dblValue = Convert.ToDouble(_cubeValues[j - _extraColumns, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);
            ////            dr.SetField(dt.Columns[j], dblValue);
            ////        }
            ////    }
            ////}

            //string salesUnitsVariableName;
            //string inventoryUnitsVariableName;
            //bool isChainNodePlanLevelTypeTotal;

            //if (this.managerData._currentChainPlanProfile.NodeProfile.OTSPlanLevelType == eOTSPlanLevelType.Total)
            //{
            //    isChainNodePlanLevelTypeTotal = true;
            //}
            //else
            //{
            //    isChainNodePlanLevelTypeTotal = false;
            //}

            //if (isChainNodePlanLevelTypeTotal == true)
            //{
            //    salesUnitsVariableName = "Sales R/P";
            //    inventoryUnitsVariableName = "Stock Reg";
            //}
            //else
            //{
            //    salesUnitsVariableName = "Sales";
            //    inventoryUnitsVariableName = "Stock";
            //}
            //for (int tableIndex = 0; tableIndex < chartDataSet.Tables.Count; tableIndex++)
            //{
            //    for (int columnIndex = 0; columnIndex < chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
            //    {
            //        string colName = chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;


            //        if (colName == salesUnitsVariableName)
            //        {
            //            chartDataSet.Tables[0].Columns[columnIndex].DataType = typeof(double);
            //        }


            //        if (colName != "RowKeyDisplay" && colName != salesUnitsVariableName && colName != inventoryUnitsVariableName)
            //        {
            //            chartDataSet.Tables[tableIndex].Columns.RemoveAt(columnIndex);
            //        }

            //    }
            //}


            //for (int tableIndex = chartDataSet.Tables.Count -1; tableIndex >= 0; tableIndex--)
            //{
            //    for (int columnIndex = 0; columnIndex < chartDataSet.Tables[tableIndex].Columns.Count; columnIndex++)
            //    {
            //        string colName = chartDataSet.Tables[tableIndex].Columns[columnIndex].ColumnName;


            //        //if (colName != "RowKeyDisplay")
            //        //{
            //        //    chartDataSet.Tables[0].Columns[i].DataType = typeof(DateTime);
            //        //}


            //        if (colName != "RowKeyDisplay" && colName != salesUnitsVariableName && colName != inventoryUnitsVariableName)
            //        {
            //            chartDataSet.Tables[tableIndex].Columns.RemoveAt(columnIndex);
            //        }

            //    }
            //}


            return chartDataSet;

        }

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
        public DataSet ReconstructDatasetForChart(ArrayList alVariables)
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
        // End TT#1748-MD
        private string ParseTotalVariableName(string aVarName)
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

        private PlanWaferCell[,] GetCubeValues()
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
                    planWaferCellTable = managerData._planCubeGroup.GetPlanWaferCellValues(aCubeWafer, managerData.unitsScalingString, managerData.dollarScalingString);
                }

                return planWaferCellTable;
            }
            catch
            {
                throw;
            }

        }


       
     
        private void SetCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex )
        {
            for (int j = _extraColumns; j < dt.Columns.Count; j++)
            {
                if (_cubeValues != null && _cubeValues[j - _extraColumns, rowIndex] != null && _cubeValues[j - _extraColumns, rowIndex].isValueNumeric)
                {
                    //Double dblValue = Convert.ToDouble(_cubeValues[j - _extraColumns, rowIndex].ValueAsString, CultureInfo.CurrentUICulture);




                    dr.SetField(dt.Columns[j], _cubeValues[j - _extraColumns, rowIndex].ValueAsString);
                }
            }
        }
        //private string GetCellDisplayFormattedValue(int rowIndex, double unformattedValue)
        //{
        //    //if (rowIndex == -1)
        //    //{
        //    //    return unformattedValue.ToString();
        //    //}
        //    //if (columnIndex < _extraColumns)
        //    //{
        //    //    return unformattedValue.ToString();
        //    //}
        //    //if (columnIndex - _extraColumns >= GetRowCount())
        //    //{
        //    //    return unformattedValue.ToString();
        //    //}

        //    VariableProfile varProf = GetVariableProfile(rowIndex); 

        //    if (varProf.FormatType == eValueFormatType.GenericNumeric)
        //    {
        //        //eVariableStyle varStyle = varProf.VariableStyle;
        //        //switch (varStyle)
        //        //{
        //        //    //Begin Modification - JScott - Add Scaling Decimals
        //        //    //case eVariableStyle.Dollar:

        //        //    //    cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _dollarScaling, _numDecimals);
        //        //    //    return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);

        //        //    //case eVariableStyle.Units:

        //        //    //    cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _unitScaling, _numDecimals);
        //        //    //    return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);
        //        //    case eVariableStyle.Dollar:
        //        //    case eVariableStyle.Units:

        //        //        //cellValue = (double)(decimal)System.Math.Round(_waferCell.Value / _scaling, _numDecimals);
        //        //        return cellValue.ToString(_formatString, CultureInfo.CurrentUICulture);


        //        //    default:

        //        //        return _waferCell.Value.ToString(_formatString, CultureInfo.CurrentUICulture);
        //        //}
        //        string _formatString = GetVariableFormat(varProf.NumDisplayDecimals);
        //        return unformattedValue.ToString(_formatString, CultureInfo.CurrentUICulture);
        //    }
        //    else
        //    {
        //        return unformattedValue.ToString();
        //    }
        //}
        //private string GetVariableFormat(int _numDecimals)
        //{
        //    bool aUseCommas = true;
        //    string _formatString = string.Empty;
        //    if (_numDecimals < Include.DecimalFormats.Length)
        //    {
        //        if (aUseCommas)
        //        {
        //            _formatString = Include.DecimalFormats[_numDecimals];
        //        }
        //        else
        //        {
        //            _formatString = Include.NoCommaDecimalFormats[_numDecimals];
        //        }
        //    }
        //    else
        //    {
        //        if (aUseCommas)
        //        {
        //            _formatString = "###,###,##0.";
        //        }
        //        else
        //        {
        //            _formatString = "########0.";
        //        }

        //        _formatString = _formatString.PadRight(_formatString.Length + _numDecimals, '0');
        //    }
        //    return _formatString;
        //}
        private void SetChartCellValuesOnRow(DataTable dt, DataRow dr, int rowIndex)
        {
            for (int j = 1; j < dt.Columns.Count; j++)
            {
                chartColumnRef cref = chartColumnRefList.Find(delegate(chartColumnRef c) {return c.columnName == dt.Columns[j].ColumnName;});
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

       
       
        private void CheckForParentKey(DataRow aDataRow, Profile aProfile, int aIndex)
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
                foreach (DataRelation rel in gridDataSet.Relations)
                {
                    if (rel.RelationName.EndsWith(profileTypeStr))
                    {
                        parentName = rel.RelationName.Substring(0, rel.RelationName.Length - profileTypeStr.Length); 
                        break;
                    }
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

        private void CheckDataSetMember(DataSet aDataset, eProfileType aProfileType)
        {
            if (aDataset.ExtendedProperties == null || aDataset.ExtendedProperties.Count == 0)
            {
                aDataset.ExtendedProperties.Add(aProfileType.ToString(), _maxBandDepth);
            }
        }

        private void AddDataSetRelations()
        {
            try
            {
                Hashtable ht = new Hashtable();
                ht.Add("Year", 1);
                ht.Add("Season", 2);
                ht.Add("Quarter", 3);
                ht.Add("Month", 4);
                ht.Add("Week", 5);

                _maxBandDepth = 1;
                for (int i = 0; i < _sortedTimeHeaders.Count; i++)
                {
                    RowColProfileHeader timeHeader = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i);
                    if (i > 0)
                    {
                        eProfileType relProfileType = GetProfileType(timeHeader);
                        if (relProfileType == eProfileType.Year)
                        {
                            if (i == 1) // only Years - there won't be any relations
                            {
                                break;
                            }
                        }
                        if (relProfileType != eProfileType.None)
                        {
                            RowColProfileHeader timeHeaderPrev = (RowColProfileHeader)_sortedTimeHeaders.GetByIndex(i - 1);
                            eProfileType prevRelProfileType = GetProfileType(timeHeaderPrev);
                            if (prevRelProfileType.ToString() != relProfileType.ToString())
                            {
                                int prevRelValue = (int)ht[prevRelProfileType.ToString()];
                                int relValue = (int)ht[relProfileType.ToString()];

                                if (prevRelValue < relValue)
                                {
                                    string relationName = prevRelProfileType.ToString() + relProfileType.ToString();
                                    if (!gridDataSet.Relations.Contains(relationName))
                                    {
                                        gridDataSet.Relations.Add(relationName, gridDataSet.Tables[prevRelProfileType.ToString()].Columns["RowKey"], gridDataSet.Tables[relProfileType.ToString()].Columns["ParentRowKey"]);
                                        _maxBandDepth++;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private eProfileType GetProfileType(RowColProfileHeader aRowColProfileHeader)
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

        // Begin TT#1748-MD - RMatelic - Chain Ladder Chart - Add additional variable selection options
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
        // End TT#1748-MD 

        private class SimpleRowHeader
        {
            public CubeWaferCoordinateList CubeWaferCoorList;
            public string RowHeading;
            public VariableProfile varProf;
            public string basisHeading;
            public string basisToolTip;
            public string basisHeaderName;
            public int basisHeaderSequence;

            public SimpleRowHeader(CubeWaferCoordinateList aCoorList, string aRowHeading, VariableProfile varProf)
		    {
			    CubeWaferCoorList = aCoorList;
			    RowHeading = aRowHeading;
                this.varProf = varProf;
                this.basisHeading = string.Empty;
                this.basisToolTip = string.Empty;
            
		    }
            public SimpleRowHeader(CubeWaferCoordinateList aCoorList, string aRowHeading, VariableProfile varProf, string basisHeading, string basisToolTip, string basisHeaderName, int basisHeaderSequence)
            {
                CubeWaferCoorList = aCoorList;
                RowHeading = aRowHeading;
                this.varProf = varProf;
                this.basisHeading = basisHeading;
                this.basisToolTip = basisToolTip;
                this.basisHeaderName = basisHeaderName;
                this.basisHeaderSequence = basisHeaderSequence;
            }
        }
        private class SimpleColumnHeader
        {
            public CubeWaferCoordinateList CubeWaferCoorList;
        

            public SimpleColumnHeader(CubeWaferCoordinateList aCoorList)
            {
                CubeWaferCoorList = aCoorList;
              
            }
        }



    }
}
