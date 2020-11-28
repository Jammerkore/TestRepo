using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business.Allocation;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {

        //allocation review selection
        protected ApplicationSessionTransaction _applicationSessionTransaction;
        private AllocationHeaderProfileList _allocationHeaderProfileList;
        private HierarchyNodeProfile _hierarchyNodeProfile;
        private int _lastValidBasisRID = Include.NoRID;
        private System.Data.DataTable _basisDataTable;
        private bool _bindingView;

        /// <summary>
        /// GetFilters
        /// </summary>
        /// <returns>ROIntStringPair containing the Header Filters for the user</returns>
        private ROOut GetFilters(RONoParms parms)
        {
            FilterData _filterData;
            _filterData = new FilterData();
            List<KeyValuePair<int, string>> filters = new List<KeyValuePair<int, string>>();

            ArrayList userRIDList = new ArrayList();
            userRIDList.Add(Include.GlobalUserRID);
            userRIDList.Add(SAB.ClientServerSession.UserRID);

            DataTable dtHeaderFilters = _filterData.FilterRead(filterTypes.HeaderFilter, eProfileType.FilterHeader, userRIDList);
            DataView dv = new DataView(dtHeaderFilters);
            dv.Sort = "FILTER_NAME";

            foreach (DataRowView rowView in dv)
            {
                int filterRID = Convert.ToInt32(rowView.Row["FILTER_RID"]);
                string filterName = Convert.ToString(rowView.Row["FILTER_NAME"]);
                filters.Add(new KeyValuePair<int, string>(filterRID, filterName));
            }

            ROIntStringPairListOut ROKeyValuePairOut = new ROIntStringPairListOut(eROReturnCode.Successful, null, parms.ROInstanceID, filters);

            return ROKeyValuePairOut;
        }

        #region AllocationViewSelection 

        internal ROOut GetAllocationViewSeletionDetails(ROKeyParms rOKeyParms)
        {
            try
            {
                ROAllocationReviewSelectionProperties allocationProperties = BuildAllocationPropertiesList(rOKeyParms);
                return new ROAllocationReviewSelectionPropertiesOut(eROReturnCode.Successful, null, ROInstanceID, allocationProperties);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAllocationViewSeletionDetails failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);

                throw;
            }

        }

        private ROAllocationReviewSelectionProperties BuildAllocationPropertiesList(ROKeyParms rOKeyParms)
        {
            try
            {

                _applicationSessionTransaction = GetApplicationSessionTransaction(getNewTransaction: true);

                _applicationSessionTransaction.CreateAllocationViewSelectionCriteria();

                _applicationSessionTransaction.NewCriteriaHeaderList();

                AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(rOKeyParms.Key, true, true, true);
                SAB.ClientServerSession.AddSelectedHeaderList(ahp.Key, ahp.HeaderID, ahp.HeaderType, ahp.AsrtRID, ahp.StyleHnRID);

                var selectedHeaders = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();
                List<ROAllocationHeaderSummary> headerDetails = BuildHeaderDetailsForReviewSelection(selectedHeaders, ahp);

                bool chkIneligibleStore = _applicationSessionTransaction.AllocationIncludeIneligibleStores;

                KeyValuePair<int, string> DateRangeBeginRID = GetName.GetCalendarDateRange(_applicationSessionTransaction.AllocationNeedAnalysisPeriodBeginRID,SAB);
                KeyValuePair<int, string> DateRangeEndRID = GetName.GetCalendarDateRange(_applicationSessionTransaction.AllocationNeedAnalysisPeriodEndRID,SAB);

                string needAnalysisBasis = string.Empty;
                if (_applicationSessionTransaction.AllocationNeedAnalysisHNID != Include.NoRID)
                {
                    _hierarchyNodeProfile = SAB.HierarchyServerSession.GetNodeData(_applicationSessionTransaction.AllocationNeedAnalysisHNID, true, true);
                    needAnalysisBasis = _hierarchyNodeProfile.Text;
                    _lastValidBasisRID = _applicationSessionTransaction.AllocationNeedAnalysisHNID;
                }
                               
                eAllocationSelectionViewType _viewType = _applicationSessionTransaction.AllocationViewType;
                DataTable _basisData = SetUpBasisGrid();
                List<ROAllocationReviewSelectionBasis> allocationPropertiesBasis = DataTableToAllocationBasis(_basisData);
                List<ROAllocationReviewSelectionGridData> allocationPropertiesGridData = DataTableToAllocationReviewSelection(headerDetails);


                KeyValuePair<int, string> storefilterValue = GetName.GetFilterName(_applicationSessionTransaction.AllocationFilterID);
                //stores name
                KeyValuePair<int, string> storeProfileValue = GetName.GetStoreName(Include.NoRID);
                KeyValuePair<int, string> groupbyValue = GetName.GetGroupByName(_applicationSessionTransaction.AllocationGroupBy);

                //store attribute
                KeyValuePair<int, string> storeAttributeValue = GetName.GetAttributeName(_applicationSessionTransaction.AllocationStoreAttributeID);
                KeyValuePair<int, string> views = GetName.GetAllocationViewName(_applicationSessionTransaction.AllocationViewRID);                                
                KeyValuePair<int, string> sizeCurveValue = GetName.GetSizeCurveGroupName(_applicationSessionTransaction.SizeCurveRID);

                _viewType = EnumTools.VerifyEnumValue(_viewType);

                ROAllocationReviewSelectionProperties rOAllocationProperties =
                    new ROAllocationReviewSelectionProperties(_viewType, allocationPropertiesBasis, allocationPropertiesGridData, storefilterValue, storeProfileValue, groupbyValue,
                    storeAttributeValue, views, chkIneligibleStore, DateRangeBeginRID, DateRangeBeginRID, needAnalysisBasis, sizeCurveValue);

                return rOAllocationProperties;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //_trans.Dispose();
            }
        }




        private List<ROAllocationReviewSelectionBasis> DataTableToAllocationBasis(DataTable dataTable)
        {
            List<ROAllocationReviewSelectionBasis> allocationBasis = new List<ROAllocationReviewSelectionBasis>();
            HierarchyProfile _hierarchyProfile = SAB.HierarchyServerSession.GetMainHierarchyData();
          
            string Merchandise = string.Empty;
            if (dataTable.Rows.Count > 0)
            {
                for (int iCounter = 0; iCounter < dataTable.Rows.Count; iCounter++)
                {
                    int basisPHRID = Convert.ToInt32(dataTable.Rows[iCounter]["BasisPHRID"].ToString());
                    int basicSequence = Convert.ToInt32(dataTable.Rows[iCounter]["BasisSequence"].ToString());
                    int HN_RID = Convert.ToInt32(dataTable.Rows[iCounter]["BasisHNRID"].ToString());
                    int FV_RID = Convert.ToInt32(dataTable.Rows[iCounter]["BasisFVRID"].ToString()); ;
                    int CDR_RID = Convert.ToInt32(dataTable.Rows[iCounter]["CdrRID"].ToString());
                    double weight = Convert.ToDouble(dataTable.Rows[iCounter]["Weight"].ToString());
                    int basisPHLSequence = Convert.ToInt32(dataTable.Rows[iCounter]["BasisPHLSequence"].ToString());


                    //Merchandise
                    if (HN_RID > -1)
                    {
                        HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(HN_RID, true, true);
                        Merchandise = hnp.Text;
                    }

                    if (basisPHLSequence == 0)
                    {
                        Merchandise = MIDText.GetTextOnly(eMIDTextCode.lbl_OTSPlanLevel);
                    }

                    if (basisPHLSequence > 0)
                    {
                        HierarchyProfile hp = SAB.HierarchyServerSession.GetMainHierarchyData();
                        HierarchyLevelProfile hlp = (HierarchyLevelProfile)_hierarchyProfile.HierarchyLevels[basisPHLSequence];
                        Merchandise = hlp.Level.ToString();
                    }

                    //Version
                    ForecastVersion fv = new ForecastVersion();
                    string Version = fv.GetVersionText(FV_RID);

                    //Date Range
                    DateRangeProfile dr = SAB.ClientServerSession.Calendar.GetDateRange(CDR_RID);
                    string HorizonDateRange = dr.DisplayDate;

                    allocationBasis.Add(new ROAllocationReviewSelectionBasis(basisPHRID, basicSequence, HN_RID, FV_RID, CDR_RID, weight, Merchandise, Version, HorizonDateRange));
                }
            }
            return allocationBasis;
        }

        private List<ROAllocationHeaderSummary> BuildHeaderDetailsForReviewSelection(SelectedHeaderList selectedHeaders, AllocationHeaderProfile ahp)
        {
            ArrayList headerProfileList = new ArrayList();
            if (selectedHeaders.Count > 0)
            {
                foreach (SelectedHeaderProfile item in selectedHeaders)
                {
                    headerProfileList.Add(SAB.HeaderServerSession.GetHeaderData(item.Key, true, true, true));
                }
            }

            return BuildHeaderSummaryList(headerProfileList: headerProfileList, includeDetails: true);
        }

        private List<ROAllocationReviewSelectionGridData> DataTableToAllocationReviewSelection(List<ROAllocationHeaderSummary> headerDetails)
        {
            List<ROAllocationReviewSelectionGridData> allocationReviewSelections = new List<ROAllocationReviewSelectionGridData>();

            if (headerDetails.Count > 0)
            {
                int count = 0;
                string header;
                int rowPosition;
                string description;
                int? rid;

                foreach (ROAllocationHeaderSummary item in headerDetails)
                {
                    header = item.HeaderID;
                    rowPosition = count;
                    description = item.HeaderDescription;
                    rid = item.Key;

                    allocationReviewSelections.Add(new ROAllocationReviewSelectionGridData(rowPosition, header, description, rid));
                    ++count;
                }
            }
            return allocationReviewSelections;

        }


        private DataTable SetUpBasisGrid()
        {
            _basisDataTable = _applicationSessionTransaction.DTUserAllocBasis;
            if (!_basisDataTable.Columns.Contains("Merchandise"))
            {
                _basisDataTable.Columns.Add("Merchandise");
                _basisDataTable.Columns.Add("DateRange");
                _basisDataTable.Columns.Add("Picture");
                _basisDataTable.Columns["Weight"].DefaultValue = 1;
                _basisDataTable.AcceptChanges();
            }

            return _basisDataTable;
        }

        #endregion for AllocationViewSelection

        internal ROOut DeleteViewDetails(int viewKey)
        {
            string message = null;
            eROReturnCode returnCode = eROReturnCode.Successful;
            bool successful = true;

            if (viewKey == Include.DefaultVelocityMatrixViewRID
                || viewKey == Include.DefaultVelocityDetailViewRID
                || viewKey == Include.DefaultStyleViewRID
                || viewKey == Include.DefaultSizeViewRID)
            {
                message = "Default view cannot be deleted";
                returnCode = eROReturnCode.Failure;
                successful = false;
            }
            else
            {
                GridViewData data = new GridViewData();

                try
                {
                    if (viewKey > 0)
                    {
                        data.OpenUpdateConnection();
                        if (data.GridView_Delete(viewKey) > 0)
                        {
                            data.CommitData();
                        }
                        else
                        {
                            message = "View delete failed";
                            returnCode = eROReturnCode.Failure;
                            successful = false;
                        }
                    }
                    else
                    {
                        message = "View not selected";
                        returnCode = eROReturnCode.Failure;
                        successful = false;
                    }
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    returnCode = eROReturnCode.Failure;
                    successful = false;
                }
                finally
                {
                    data.CloseUpdateConnection();
                    _currentViewRID = Include.NoRID;
                }
            }

            return new ROBoolOut(returnCode, message, ROInstanceID, successful);
        }
    }
}
