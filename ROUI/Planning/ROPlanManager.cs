using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebSharedTypes;
using static Logility.ROWebSharedTypes.ROGridCellChange;

namespace Logility.ROUI
{
    abstract public class ROPlanManager : ROManager
    {
        private PlanOpenParms _openParms;

        int _viewKey = -1;
        eGridOrientation _orientation = eGridOrientation.None;
        int _storeAttributeSetKey = -1;
        int _storeAttributeKey = -1;
        int _filterKey = -1;

        public ROPlanManager(SessionAddressBlock SAB, PlanOpenParms aOpenParms)
            : base(SAB)
        {
            _openParms = aOpenParms;
        }

        public PlanOpenParms OpenParms { get { return _openParms; } private set { } }

        public int ViewKey { get { return _viewKey; } set { _viewKey = value; } }
        public eGridOrientation Orientation { get { return _orientation; } set { _orientation = value; } }
        public int StoreAttributeSetKey { get { return _storeAttributeSetKey; } set { _storeAttributeSetKey = value; } }
        public int StoreAttributeKey { get { return _storeAttributeKey; } set { _storeAttributeKey = value; } }
        public int FilterKey { get { return _filterKey; } set { _filterKey = value; } }
        abstract public PlanCubeGroup CubeGroup { get; }

        abstract public int iAddedColumnsCount { get; }

        abstract public void InitializeData();

        abstract public PagingCoordinates SetPageCoordinates(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns);

        abstract public Dictionary<string, int> GetColumnCoordinateMap();

        abstract public int StartingRowIndex { get; }
        abstract public int NumberOfRows { get; }
        abstract public int StartingColIndex { get; }
        abstract public int NumberOfColumns { get; }

        //public DataSet GetInitialDataSetForView(int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns)
        //{
        //    return GetDataSetForView(OpenParms.ViewRID, iStartingRowIndex, iNumberOfRows, iStartingColIndex, iNumberOfColumns);
        //}

        //abstract public DataSet GetDataSetForView(int viewRID);
        //abstract public DataSet GetDataSetForView(int viewRID, int iStartingRowIndex, int iNumberOfRows, int iStartingColIndex, int iNumberOfColumns);
        abstract public void SetViewAndOrientation(int viewRID, eGridOrientation gridOrientation, int StoreAttributeSetKey, int StoreAttributeKey, int FilterKey);

        abstract public void UndoLastRecompute();

        abstract public FunctionSecurityProfile GetFunctionSecurityProfile();

        public DataTable GetViewListDataTable()
        {
            FunctionSecurityProfile viewUserSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsUser);
            FunctionSecurityProfile viewGlobalSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.ForecastViewsGlobal);
            ArrayList _viewUserRIDList = new ArrayList();

            _viewUserRIDList.Add(-1);

            if (viewUserSecurity.AllowView)
            {
                _viewUserRIDList.Add(SAB.ClientServerSession.UserRID);
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
                    row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture) + " (" + UserNameStorage.GetUserName(userRID) + ")";
                }
                else
                {
                    row["DISPLAY_ID"] = Convert.ToString(row["VIEW_ID"], CultureInfo.CurrentUICulture);
                }
            }
            return _dtView;
        }

        override public int GetViewRID()
        {
            return OpenParms.ViewRID;
        }

        abstract public List<string> GetBasisMenuList();

        abstract public string GetTitleText();

        abstract public string GetExportTitleText();

        abstract public string GetHeaderDescription();

        abstract public bool HasDisplayableVariables();

        abstract public void ShowHideBasis(string basisKey, int basisSequence, bool doShow);

        abstract public bool IsNewCellValueValid(int rowIndex, int columnIndex, object newValue);

        abstract public bool IsCellValueNegative(int rowIndex, int columnIndex);

        abstract public bool IsCellIneligible(int rowIndex, int columnIndex);

        abstract public bool IsCellLocked(int rowIndex, int columnIndex);

        abstract public bool IsCellEditable(int rowIndex, int columnIndex);

        abstract public bool IsCellBasis(int rowIndex, int columnIndex);

        abstract public void CellValueChanged(int rowIndex, int columnIndex, double newValue, eROCellAction eROCellActionParam, eDataType eDataType);

        abstract public void RecomputePlanCubes();

        abstract public void RebuildGridData();

        //abstract public void UpdateGridDataset();

        //abstract public DataSet GetGridDataset();

        abstract public ArrayList GetSelectableQuantityHeaders();

        abstract public ArrayList GetSelectableVariableHeaders();

        abstract public ArrayList GetSelectablePeriodHeaders();

        abstract public ArrayList GetVariableGroupings();

        abstract public DataSet ReconstructChartDataset(ArrayList alVariables);

        abstract public bool DoesDataSetContainInventoryUnitVariables();

        abstract public string GetSalesUnitsVariableName();

        abstract public string GetInventoryUnitsVariableName();

        //abstract public DataSet ReconstructCubeCoordinatesAndDataset();

        abstract public ROData ReconstructPage();

        abstract public ROCubeMetadata CunstructMetadata(ROCubeGetMetadataParams metadataParams);

        abstract public void IncrementAddedColumnsCount(uint iColumnsAdded);

        abstract public bool ShowYears();
        abstract public bool ShowSeasons();
        abstract public bool ShowQuarters();
        abstract public bool ShowMonths();
        abstract public bool ShowWeeks();

        #region "Scaling"
        public DataTable ScalingDollar_GetDataTable()
        {
            DataTable dtDollarScaling = new DataTable("Dollar Scaling");
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
            DataTable dtUnitsScaling = new DataTable("Unit Scaling");
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

        abstract public void ScalingDollarChanged(string scalingValue);

        abstract public void ScalingUnitsChanged(string scalingValue);

        #endregion

        

    }


}
