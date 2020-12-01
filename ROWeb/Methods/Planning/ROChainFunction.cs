using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;

using Logility.ROWebSharedTypes;
using Logility.ROUI;

namespace Logility.ROWeb
{
    abstract public class ROChainFunction : ROWebPlanningFunction
    {

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="sessionType">the type of planning session (store/chain and single/multi level)</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROChainFunction(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {

        }

        //RO-1170 Replace the GetExtraColumnCount with an Abstract Class
        //abstract used in both ROChainMultiLevelFunctio.cs and ROChainSingleLevelFunction.cs
        abstract protected int GetExtraColumnCount();

        /// <summary>
        /// Disposes of any internal resources used to manage cube group data.
        /// </summary>
        public override void CleanUp()
        {
            CloseCubeGroup();
        }

        override protected DataTable GetForecastVersions()
        {
            ForecastVersion forVersion = new ForecastVersion();
            DataTable dtFV = forVersion.GetForecastVersions(false);

            DataTable dt = new DataTable("Versions");
            DataRow dr;

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("NAME", typeof(string));

            FVRowHandler rowHandler = FVRowHandler.Instance;

            foreach (DataRow drIn in dtFV.Rows)
            {
                rowHandler.ParseDBRow(drIn);
                dr = dt.NewRow();
                dr["ID"] = rowHandler.iRID;
                dr["NAME"] = rowHandler.sDescription;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private DataSet BuildBasisDataSet(bool useYearBasisDateRange)
        {
            string basisDateRangeName = useYearBasisDateRange ? "basis date range_year" : "basis date range";
            DateRangeProfile basisDateRangeProfile = getDateRangeProfileByName(basisDateRangeName);
            VersionProfile basisVersion = GetVersionProfile("Actual", ePlanType.Chain, ePlanBasisType.Plan);
            DataSet dsBasis = new DataSet();
            DataTable dtBasis = new DataTable("Basis");

            if (basisDateRangeProfile == null)
            {
                string msg = string.Format("Could not find date range profile '{0}'", basisDateRangeName);

                throw new Exception(msg);
            }
            dtBasis.Columns.Add("BasisID", System.Type.GetType("System.Int32"));
            dtBasis.Columns.Add("BasisName", System.Type.GetType("System.String"));

            DataTable dtBasisDetail = new DataTable("BasisDetails");
            dtBasisDetail.Columns.Add("BasisID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Merchandise", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("MerchandiseID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Version", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("VersionID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("DateRange", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("DateRangeID", System.Type.GetType("System.Int32"));
            dtBasisDetail.Columns.Add("Picture", System.Type.GetType("System.String"));
            dtBasisDetail.Columns.Add("Weight", System.Type.GetType("System.Decimal"));
            dtBasisDetail.Columns.Add("IsIncluded", System.Type.GetType("System.Boolean"));
            dtBasisDetail.Columns.Add("IncludeButton", System.Type.GetType("System.String"));

            DataRow basisRow = dtBasis.NewRow();
            basisRow["BasisID"] = 0;
            basisRow["BasisName"] = "Basis 1";

            DataRow basisDetailRow = dtBasisDetail.NewRow();
            basisDetailRow["BasisID"] = basisRow["BasisID"];
            basisDetailRow["IsIncluded"] = true;
            basisDetailRow["Merchandise"] = OpenParms.ChainHLPlanProfile.NodeProfile.Text;
            basisDetailRow["MerchandiseID"] = OpenParms.ChainHLPlanProfile.NodeProfile.Key;
            basisDetailRow["Version"] = basisVersion.Description;
            basisDetailRow["VersionID"] = basisVersion.Key;
            basisDetailRow["DateRange"] = basisDateRangeProfile.DisplayDate;
            basisDetailRow["DateRangeID"] = basisDateRangeProfile.Key;

            dtBasis.Rows.Add(basisRow);
            dtBasisDetail.Rows.Add(basisDetailRow);

            dsBasis.Tables.Add(dtBasis);
            dsBasis.Tables.Add(dtBasisDetail);
            return dsBasis;
        }

        /// <summary>
        /// Get the specified cube data.
        /// </summary>
        /// <param name="getDataParams">Specifies the data to get from the cube group and the desired orientation</param>
        /// <returns>
        /// A DataSet containing the specified values for the cube group and their parent/child relationships.
        /// </returns>
        override protected ROGridData GetCubeData(ROCubeGetDataParams getDataParams)
        {
            int ignoreVal;
            string ignoreString;

            int storeAttributeSetKey = getDataParams.StoreAttributeSetKey;
            int storeAttributeKey = getDataParams.StoreAttributeKey;
            int filterKey = getDataParams.FilterKey;

            //RO-3083 and RO-3084 check for changed attribute key, key set and view key
            //if the view key = -1 use the view string instead
            //int viewRID = GetViewRID(getDataParams.view, out ignoreVal);
            int viewRID = GetViewRID(getDataParams.ViewKey, out ignoreVal, out ignoreString);
            if (viewRID == -1)
            {
                //try to use the string instead
                viewRID = GetViewRID(getDataParams.view, out ignoreVal);
            }

            ROData ROData = null;

            //RO-3156 and store group set key to get data for store single/multi level
            if (getDataParams.GridOrientation != planManager.Orientation
                || viewRID != planManager.GetViewRID() || filterKey != planManager.FilterKey
                || planManager.GetViewData.ViewUpdated) 
            {
                planManager.SetViewAndOrientation(viewRID, getDataParams.GridOrientation, getDataParams.StoreAttributeSetKey, getDataParams.StoreAttributeKey, getDataParams.FilterKey);
                planManager.GetViewData.ViewUpdated = false;
            }

            if (getDataParams.iStartingRowIndex != planManager.StartingRowIndex
                || getDataParams.iNumberOfRows != planManager.NumberOfRows
                || getDataParams.iStartingColIndex != planManager.StartingColIndex
                || getDataParams.iNumberOfColumns != planManager.NumberOfColumns)
            {
                pagingCoordinates = planManager.SetPageCoordinates(getDataParams.iStartingRowIndex, getDataParams.iNumberOfRows, getDataParams.iStartingColIndex, getDataParams.iNumberOfColumns);
            }

            if (getDataParams.sUnitScaling != sPrevUnitScaling)
            {
                planManager.ScalingUnitsChanged(getDataParams.sUnitScaling);
                sPrevUnitScaling = getDataParams.sUnitScaling;
            }
            if (getDataParams.sDollarScaling != sPrevDollarScaling)
            {
                planManager.ScalingDollarChanged(getDataParams.sDollarScaling);
                sPrevDollarScaling = getDataParams.sDollarScaling;
            }

            ROData = planManager.ReconstructPage();

            int iExtraColumnCount = GetExtraColumnCount();

            return new ROGridData(eROReturnCode.Successful, null, ROInstanceID, ROData,
                pagingCoordinates.FirstRowItem, pagingCoordinates.LastRowItem, pagingCoordinates.TotalRowItems, getDataParams.iNumberOfRows,
                pagingCoordinates.FirstColItem, pagingCoordinates.LastColItem, pagingCoordinates.TotalColItems, getDataParams.iNumberOfColumns);
        }

        /// <summary>
        /// Post a value change to the cube
        /// </summary>
        /// <param name="cellChanges">The list of cell changes</param>ROGridChanges
        override protected ROOut CellChanged(ROGridChangesParms gridChanges)
        {
            int iRowIndex;
            int iColumnIndex;
            foreach (ROGridCellChange cellChange in gridChanges.CellChanges)
            {
                //if (((ROPlanChainSingleLevelManager)planManager).ChainSingleLevelViewData is Logility.ROUI.ROChainSingleLevelLadderViewData)
                if ( planManager.Orientation == eGridOrientation.TimeOnRow)
                {
                    // reverse indexes because the coordinate lists are reversed in chain ladder
                    iRowIndex = cellChange.ColumnIndex - GetExtraColumnCount();
                    iColumnIndex = cellChange.RowIndex;
                }
                else
                {
                    iRowIndex = cellChange.RowIndex;
                    iColumnIndex = cellChange.ColumnIndex - GetExtraColumnCount();
                }

                var currentCellAction = cellChange.CellAction;
                planManager.CellValueChanged(iRowIndex, iColumnIndex, cellChange.dNewValue, currentCellAction, cellChange.DataType);

            }

            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

        /// <summary>
        /// Save the changes to the database
        /// </summary>
        override public ROOut SaveCubeGroup()
        {
            RecomputeCubes();

            PlanSaveParms planSaveParms = new PlanSaveParms();

            planSaveParms.SaveChainHighLevel = true;
            planSaveParms.ChainHighLevelNodeRID = planManager.OpenParms.ChainHLPlanProfile.NodeProfile.Key;
            planSaveParms.ChainHighLevelVersionRID = planManager.OpenParms.ChainHLPlanProfile.VersionProfile.Key;
            planSaveParms.ChainHighLevelDateRangeRID = planManager.OpenParms.DateRangeProfile.Key;
            planSaveParms.SaveHighLevelAllStoreAsChain = false;

            planManager.CubeGroup.SaveCubeGroup(planSaveParms);
            return new RONoDataOut(eROReturnCode.Successful, null, ROInstanceID);
        }

    }
}
