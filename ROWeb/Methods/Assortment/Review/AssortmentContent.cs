using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Windows;

using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using System.Reflection;

namespace Logility.ROWeb
{
    public partial class ROAssortment : ROAllocation
    {
        private string _sROMessage;
        private eROReturnCode _returnCode;
        private bool _enqueueHeaderError = false;
        private bool _cancelAllocationCancelled = false;
        private SelectedHeaderList _selectedHeaderList = new SelectedHeaderList(eProfileType.SelectedHeader);

        private string _lblHierarchyNodeValue = null;
        private string _lblPlaceholderValue;
        private string _lblPhStyleValue;

        private HierarchyLevelProfile _hlpStyleValue = null;
        private HierarchyLevelProfile _hlpProductValue = null;

        private bool _skipRowUpdate = false;

        public string _lblHierarchyNode
        {
            get
            {
                if (_lblHierarchyNodeValue == null)
                {
                    _lblHierarchyNodeValue = MIDText.GetTextOnly(eMIDTextCode.lbl_HierarchyNode);
                }
                return _lblHierarchyNodeValue;
            }
        }

        public string _lblPlaceholder
        {
            get
            {
                if (_lblPlaceholderValue == null)
                {
                    DataTable dataTable = MIDText.GetTextType(eMIDTextType.eHeaderType, eMIDTextOrderBy.TextCode);

                    foreach (DataRow dr in dataTable.Rows)
                    {
                        if (Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture) == (int)eHeaderType.Placeholder)
                        {
                            _lblPlaceholderValue = dr["TEXT_VALUE"].ToString();
                        }
                    }
                }
                return _lblPlaceholderValue;
            }
        }

        public string _lblPhStyle
        {
            get
            {
                if (_lblPhStyleValue == null)
                {
                    _lblPhStyleValue = MIDText.GetTextOnly(eMIDTextCode.lbl_PhStyle);
                }
                return _lblPhStyleValue;
            }
        }

        public HierarchyLevelProfile _hlpStyle
        {
            get
            {
                if (_hlpStyleValue == null)
                {
                    SetLevelIDs();
                }
                return _hlpStyleValue;
            }
        }

        public HierarchyLevelProfile _hlpProduct
        {
            get
            {
                if (_hlpProductValue == null)
                {
                    SetLevelIDs();
                }
                return _hlpProductValue;
            }
        }

        private void SetLevelIDs()
        {
            _hlpStyleValue = null;
            _hlpProductValue = null;
            for (int level = 1; level <= MainHp.HierarchyLevels.Count; level++)
            {
                _hlpProductValue = _hlpStyleValue;
                _hlpStyleValue = (HierarchyLevelProfile)MainHp.HierarchyLevels[level];
                if (_hlpStyleValue.LevelType == eHierarchyLevelType.Style)
                {
                    break;
                }
            }
            if (_hlpStyleValue == null)
            {
                _hlpStyleValue = new HierarchyLevelProfile(0);
                _hlpStyleValue.LevelID = "Style";
            }
            if (_hlpProductValue == null)
            {
                _hlpProductValue = new HierarchyLevelProfile(1);
                _hlpProductValue.LevelID = "Product";
            }
        }

        #region AssortmentContentCharacteristics
        public ROOut GetAssortmentContentCharacteristics()
        {
            try
            {
                List<ROAllocationHeaderSummary> assortmentContentCharacteristics = BuildAssortmentContentCharacteristics();
                return new ROIListOut(eROReturnCode.Successful, null, ROInstanceID, assortmentContentCharacteristics);
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "GetAssortmentContentCharacteristics failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private List<ROAllocationHeaderSummary> BuildAssortmentContentCharacteristics()
        {
            try
            {
                _applicationSessionTransaction = GetApplicationSessionTransaction();

                if (_headerList == null)
                {
                    _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();
                }

                bool excludePlaceholderTypeHeaders = false;
                if (GetAssortmentType() == eAssortmentType.PostReceipt)
                {
                    excludePlaceholderTypeHeaders = true;
                }

                List<ROAllocationHeaderSummary> contentHeaders = BuildHeaderSummaryList(headerProfileList: _headerList.ArrayList, includeDetails: true, includeCharacteristics: true, excludePlaceholderTypeHeaders: excludePlaceholderTypeHeaders);

                return contentHeaders;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "BuildAssortmentContentCharacteristics failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }
        #endregion

        #region Assortment Allocation Action Processing
        private ROOut ProcessAssortmentReviewAllocationAction(ROAssortmentAllocationActionParms assortmentAllocationActionParms)
        {
            try
            {
                _returnCode = eROReturnCode.Successful;
                _sROMessage = null;
                List<ROAllocationHeaderSummary> assortmentContentCharacteristics = ProcessAllocationActionType(assortmentAllocationActionParms);

                if (_returnCode == eROReturnCode.Successful)
                {
                    if (IsAssortment)
                    {
                        UpdateData(reload: true, rebuildComponents: true, reloadBlockedCells: true, reloadHeaders: true);
                    }

                    LoadCurrentPages();
                }

                if (assortmentAllocationActionParms.AssortmentTabType == eAssortmentReviewTabType.AssortmentReviewMatrix)
                {
                    //if (_returnCode == eROReturnCode.Successful)
                    //{
                    //    //return GetAssortmentReviewMatrixDisplay(isAsrtPropertyChanged: false, isAsrtBasisChanged: false);
                    //    if (IsAssortment)
                    //    {
                    //        UpdateData(reload: true, rebuildComponents: true, reloadBlockedCells: false, reloadHeaders: false);
                    //    }

                    //    LoadCurrentPages();
                    //}

                    return new ROGridData(_returnCode, _sROMessage, ROInstanceID, _asrtMatrixROData, 0, 0, 0, 0, 0, 0, 0, 0);
                }
                else
                {
                    return new ROIListOut(_returnCode, _sROMessage, ROInstanceID, assortmentContentCharacteristics);
                }
                
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAssortmentReviewAllocationAction failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
        }

        private List<ROAllocationHeaderSummary> ProcessAllocationActionType(ROAssortmentAllocationActionParms assortmentAllocationActionParms)
        {
            GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total);
            int action = (int)assortmentAllocationActionParms.AllocationActionType;

            try
            {
                if (assortmentAllocationActionParms.Headerkeys.Count == 0
                    && assortmentAllocationActionParms.AssortmentTabType != eAssortmentReviewTabType.AssortmentReviewMatrix)
                {
                    _selectedHeaderList = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();
                }
                else
                {
                    _selectedHeaderList.Clear();
                    foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                    {
                        if (assortmentAllocationActionParms.Headerkeys.Contains(ho.Key)
                            || assortmentAllocationActionParms.AssortmentTabType == eAssortmentReviewTabType.AssortmentReviewMatrix)
                        {
                            //if (ho.HeaderType != eHeaderType.Assortment && ho.HeaderType != eHeaderType.Placeholder)
                            if (ho.HeaderType != eHeaderType.Assortment)
                            {
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                _selectedHeaderList.Add(selectedHeader);
                            }
                        }
                    }
                }


                if (action == Include.NoRID
                    && assortmentAllocationActionParms.WorkflowKey == Include.Undefined
                    && assortmentAllocationActionParms.MethodType == eMethodType.NotSpecified)
                {
                    _sROMessage = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_ActionIsRequired);
                    return new List<ROAllocationHeaderSummary>();
                }

                ApplicationBaseAction aMethod = null;
                if (assortmentAllocationActionParms.MethodType != eMethodType.NotSpecified)
                {
                    aMethod = SAB.ApplicationServerSession.GetMethods.GetMethod(assortmentAllocationActionParms.MethodKey, assortmentAllocationActionParms.MethodType);
                }
                else
                {
                    aMethod = _applicationSessionTransaction.CreateNewMethodAction((eMethodType)action);
                }
                aComponent = new GeneralComponent(eGeneralComponentType.Total);

                bool aReviewFlag = false;
                bool aUseSystemTolerancePercent = true;
                double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                int aStoreFilter = Include.AllStoreFilterRID;
                int aWorkFlowStepKey = -1;
                int selectedHdrCount = 0;
                SelectedHeaderList shl = null;

                if (IsGroupAllocation)
                {
                    _applicationSessionTransaction.ActionOrigin = eActionOrigin.GroupAllocation;
                }
                else
                {
                    _applicationSessionTransaction.ActionOrigin = eActionOrigin.Assortment;
                }


                // The Create Placeholders and Spread Average actions can be cancelled. Wait until precessing those actions to do save.
                // Otherwise, do the save now.
                if (action != (int)eAssortmentActionType.CreatePlaceholders && action != (int)eAssortmentActionType.SpreadAverage)
                {
                    SaveChanges();
                }
                string actionText = string.Empty;

                if (Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)action))
                {
                    ////==============================
                    //// ASSORTMENT ACTION PROCESSING
                    ////==============================

                    //bool noActionPerformed = false;
                    //shl = ProcessAssortmentMethod(ref _applicationSessionTransaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey, ref noActionPerformed);

                    //selectedHdrCount = shl.Count;

                    //if (noActionPerformed)
                    //{
                    //    foreach (SelectedHeaderProfile shp in shl.ArrayList)
                    //    {
                    //        _applicationSessionTransaction.SetAllocationActionStatus(shp.Key, eAllocationActionStatus.NoActionPerformed);
                    //    }
                    //}
                }
                else
                {
                    if (assortmentAllocationActionParms.WorkflowKey != Include.Undefined)
                    {
                        //==============================
                        // ALLOCATION WORKFLOW PROCESSING
                        //==============================
                        shl = ProcessAllocationWorkflow(ref _applicationSessionTransaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent,
                            aTolerancePercent, aStoreFilter, aWorkFlowStepKey, assortmentAllocationActionParms);
                    }
                    else
                    {
                        //==============================
                        // ALLOCATION ACTION and METHOD PROCESSING
                        //==============================
                        shl = ProcessAllocationMethod(ref _applicationSessionTransaction, action, aComponent, aMethod, aReviewFlag, aUseSystemTolerancePercent,
                            aTolerancePercent, aStoreFilter, aWorkFlowStepKey, assortmentAllocationActionParms);
                    }
                    selectedHdrCount = shl.Count;

                    if (_cancelAllocationCancelled)
                    {
                        return new List<ROAllocationHeaderSummary>();
                    }

                    if (assortmentAllocationActionParms.MethodType != eMethodType.NotSpecified)
                    {
                        if (action != (int)eAssortmentAllocationActionType.BackoutAllocation
                            && action != (int)eAssortmentAllocationActionType.BackoutSizeAllocation
                            && action != (int)eAssortmentAllocationActionType.BackoutStyleIntransit
                            && action != (int)eAssortmentAllocationActionType.BackoutSizeIntransit
                            )
                        {
                            OutStoresInBlockedStyles(_selectedHeaderList);
                        }
                    }
                }

                if (selectedHdrCount == 0)
                {
                    eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
                    _sROMessage = MIDText.GetTextOnly((int)actionStatus);

                }
                else if (_applicationSessionTransaction != null)
                {
                    eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;
                    _sROMessage = MIDText.GetTextOnly((int)actionStatus);
                }
            }

            catch (MIDException MIDexc)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAction failed: " + MIDexc.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                _sROMessage = MIDexc.ErrorMessage;
                _returnCode = eROReturnCode.Failure;
            }
            catch (Exception ex)
            {
                ROWebTools.LogMessage(eROMessageLevel.Error, "ProcessAction failed: " + ex.Message, ROWebTools.ROUserID, ROWebTools.ROSessionID);
                throw;
            }
            finally
            {
                eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;

                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    // No reason to do the UpdateData for CreatePlaceholders
                    if ((eAssortmentActionType)action != eAssortmentActionType.CreatePlaceholders && (eAssortmentActionType)action != eAssortmentActionType.BalanceAssortment)
                    {
                        SelectedHeaderList allocHdrList = GetSelectableHeaderList(action, _applicationSessionTransaction, false, assortmentAllocationActionParms);

                        int[] hdrList = new int[allocHdrList.Count];
                        int i = 0;

                        foreach (SelectedHeaderProfile shp in allocHdrList)
                        {
                            hdrList[i] = shp.Key;
                            i++;
                        }

                        CheckHeaderListForUpdate(allocHdrList, true);
                        CheckForAllocationHeaders(hdrList);
                    }

                    _headerList = (AllocationHeaderProfileList)_asrtCubeGroup.GetHeaderList();

                    if (IsGroupAllocation && (eAllocationActionType)action == eAllocationActionType.BackoutAllocation && IsProcessAsGroup)
                    {
                        if (IsProcessAsGroup)
                        {
                            AssortProfile.FillAssortGradesFromGroupAllocation(_applicationSessionTransaction);
                            _storeGradeProfileList = _asrtCubeGroup.GetFilteredProfileList(eProfileType.StoreGrade);
                            BuildGrades();
                        }

                        RebuildAssortmentSummary();
                        _asrtCubeGroup.ClearTotalCubes(true);
                    }
                }
                _cancelAllocationCancelled = false;
            }
            return BuildAssortmentContentCharacteristics();
        }
        public bool IsProcessAsHeaders
        {
            get
            {
                bool answer = false;

                if (IsGroupAllocation)
                {
                    answer = true;
                }
                return answer;
            }
        }

        public bool IsProcessAsGroup
        {
            get
            {
                bool answer = false;

                if (IsGroupAllocation)
                {
                    answer = true;
                }
                return answer;
            }
        }

        public bool CheckForAllocationHeaders(int[] aKeys)
        {
            bool headerFound = false;
            try
            {
                if (IsGroupAllocation)
                {
                    headerFound = true;
                }
                else
                {
                    int key;
                    for (int i = 0; i < aKeys.Length; i++)
                    {
                        key = aKeys[i];
                        if (_headerList.Contains(key))
                        {
                            AllocationHeaderProfile ahp = (AllocationHeaderProfile)_headerList.FindKey(key);
                            if (ahp.HeaderType != eHeaderType.Assortment && ahp.HeaderType != eHeaderType.Placeholder)
                            {
                                headerFound = true;
                                break;
                            }
                        }
                    }
                }
                return headerFound;
            }
            catch
            {
                throw;
            }
        }

        public void CheckHeaderListForUpdate(SelectedHeaderList shpl, bool updateOtherViews)
        {
            try
            {
                int key;
                bool headerFound = false;
                bool allAssortmentTypeHeaders = true;
                for (int i = 0; i < shpl.ArrayList.Count; i++)
                {
                    SelectedHeaderProfile shp = (SelectedHeaderProfile)shpl[i];
                    if (_headerList.Contains(shp.Key))
                    {
                        headerFound = true;
                    }
                    //==========================================================================
                    // We want to know if all the headers in the list are assortment headers
                    //==========================================================================
                    if (shp.HeaderType != eHeaderType.Assortment)
                    {
                        allAssortmentTypeHeaders = false;
                    }
                    //============================================================================================
                    // We know we can stop looping if we found a header and if we know that not all headers are 
                    // assortment headers
                    //============================================================================================
                    if (headerFound && !allAssortmentTypeHeaders)
                    {
                        break;
                    }
                }
                if (headerFound)
                {
                    CloseAndReOpenCubeGroup();

                    UpdateData(true);
                    LoadSurroundingPages();

                    int[] hdrList = null;
                    int i = 0;


                    // Removed: processing by header (when headers have packs and bulk) causes the wrong allocated units to appear in other headers
                    //================================================================================================
                    // If only the assortment header(s) are in the list, we want to refresh all of their members
                    //================================================================================================

                    hdrList = new int[_headerList.Count];
                    foreach (AllocationHeaderProfile ahp in _headerList)
                    {
                        hdrList[i] = ahp.Key;
                        i++;
                    }

                    ReloadUpdatedHeaders(hdrList);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateData(bool reload)
        {
            UpdateData(reload: reload, rebuildComponents: true, reloadBlockedCells: true);
        }

        public void UpdateData(bool reload, bool rebuildComponents)
        {
            UpdateData(reload: reload, rebuildComponents: rebuildComponents, reloadBlockedCells: true);
        }

        public void ReloadUpdatedHeaders(int[] aHdrList)
        {
            int key;
            try
            {
                if (_allocProfileList == null || _allocProfileList.Count == 0)
                {
                    return;
                }
                for (int i = 0; i < aHdrList.Length; i++)
                {
                    key = aHdrList[i];
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private bool IsActionSelectable(int action)
        {
            bool isSelectable = false;
            if (Enum.IsDefined(typeof(eAssortmentSelectableActionType), action))
            {
                isSelectable = true;
            }
            // Is a method
            else if (Enum.IsDefined(typeof(eMethodTypeUI), action))
            {
                isSelectable = true;
            }

            return isSelectable;
        }
        private bool IsActionForHeadersOnly(int action)
        {
            bool isHeaderOnly = false;
            if (Enum.IsDefined(typeof(eAssortmentAllocHeaderOnlyActionType), action))
            {
                isHeaderOnly = true;
            }

            return isHeaderOnly;
        }

        private bool IsAssortmentAction(int action)
        {
            bool isAssortmentAction = false;
            if (Enum.IsDefined(typeof(eAssortmentActionType), action))
            {
                isAssortmentAction = true;
            }
            return isAssortmentAction;
        }
        private bool ActionAllowed(int actionRid, AllocationProfile ap)
        {
            if ((eAllocationActionType)actionRid == eAllocationActionType.StyleNeed
                && ap.HeaderType == eHeaderType.Placeholder)
            {
                SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_as_ActionNotAllowedOnPlaceholder, this.GetType().Name);
                return false;
            }

            return true;
        }
        private SelectedHeaderList GetSelectableHeaderList(int actionRid, ApplicationSessionTransaction aTrans,
           bool createTransProfileList, ROAssortmentAllocationActionParms parms, bool useHeadersAndPlaceholdersOnly = false)
        {
            return GetSelectableHeaderList(actionRid, aTrans, createTransProfileList, parms.AssortmentTabType, useHeadersAndPlaceholdersOnly);
        }

        private SelectedHeaderList GetSelectableHeaderList(int actionRid, ApplicationSessionTransaction aTrans, bool createTransProfileList,
            eAssortmentReviewTabType tabType, bool useHeadersAndPlaceholdersOnly = false)
        {
            bool actionSelectable = IsActionSelectable(actionRid);
            bool actionForHeadersOnly = IsActionForHeadersOnly(actionRid);
            bool AssortmentAction = IsAssortmentAction(actionRid);

            SelectedHeaderList selectedHdrList = new SelectedHeaderList(eProfileType.SelectedHeader);

            if (tabType == eAssortmentReviewTabType.AssortmentReviewMatrix)
            {
                if (IsGroupAllocation)
                {
                    if (IsProcessAsGroup && !actionForHeadersOnly)
                    {
                        SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(_asrtCubeGroup.DefaultAllocationProfile.Key);
                        selectedHeader.HeaderType = eHeaderType.Assortment;
                        selectedHdrList.Add(selectedHeader);
                    }
                    else
                    {
                        foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                        {
                            if (ho.HeaderType != eHeaderType.Assortment && ho.HeaderType != eHeaderType.Placeholder)
                            {
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                selectedHdrList.Add(selectedHeader);
                            }
                        }
                    }
                }
                else
                {
                    foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                    {
                        if (actionForHeadersOnly)
                        {
                            if (ho.HeaderType == eHeaderType.Assortment || ho.HeaderType == eHeaderType.Placeholder)
                            {
                                // Skip these types of headers
                            }
                            else
                            {
                                // Add header
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                selectedHdrList.Add(selectedHeader);
                                if (AssortmentAction)
                                {
                                    if (ho.AsrtRID != Include.NoRID)
                                    {
                                        if (!selectedHdrList.Contains(ho.AsrtRID))
                                        {
                                            SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                                            assortmentHeader.HeaderType = eHeaderType.Assortment;
                                            selectedHdrList.Clear();
                                            selectedHdrList.Add(assortmentHeader);
                                        }
                                    }
                                }
                            }
                        }
                        else if (useHeadersAndPlaceholdersOnly)
                        {
                            if (ho.HeaderType == eHeaderType.Assortment)
                            {
                                continue;
                            }
                            SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                            selectedHeader.HeaderType = ho.HeaderType;
                            selectedHdrList.Add(selectedHeader);
                        }
                        else
                        {
                            if (!AssortmentAction)
                            {
                                if (ho.HeaderType != eHeaderType.Assortment)
                                {
                                    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                    selectedHeader.HeaderType = ho.HeaderType;
                                    selectedHdrList.Add(selectedHeader);
                                }
                            }
                            else
                            {
                                AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(ho.Key);
                                if (ap.Placeholder && ap.NumberOfHeadersOnPlaceholder > 0 && !AssortmentAction)
                                {
                                    //selectedHdrKeyList.Add(ho.Key);
                                }
                                else
                                {
                                    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                    selectedHeader.HeaderType = ho.HeaderType;
                                    selectedHdrList.Add(selectedHeader);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (IsGroupAllocation && IsProcessAsGroup)
                {
                    if (actionForHeadersOnly)
                    {
                        foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                        {
                            if (ho.HeaderType != eHeaderType.Assortment && ho.HeaderType != eHeaderType.Placeholder)
                            {
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                selectedHdrList.Add(selectedHeader);
                            }
                        }
                    }
                    else
                    {
                        SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(_asrtCubeGroup.DefaultAllocationProfile.Key);
                        selectedHeader.HeaderType = eHeaderType.Assortment;
                        selectedHdrList.Add(selectedHeader);
                    }

                }
                else if (actionSelectable)
                {
                    AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                    if (_selectedHeaderList.Count > 0)
                    {
                        foreach (SelectedHeaderProfile shp in _selectedHeaderList)
                        {
                            AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);
                            if (ap != null)
                            {
                                if (actionForHeadersOnly)
                                {
                                    if (ap.HeaderType == eHeaderType.Assortment || ap.HeaderType == eHeaderType.Placeholder)
                                    {
                                        // Skip these types of headers
                                    }
                                    else
                                    {
                                        SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ap.Key);
                                        selectedHeader.HeaderType = ap.HeaderType;
                                        selectedHdrList.Add(selectedHeader);
                                    }
                                }
                                else
                                {
                                    if (!ActionAllowed(actionRid, ap))
                                    {
                                        continue;
                                    }
                                    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ap.Key);
                                    selectedHeader.HeaderType = ap.HeaderType;
                                    selectedHdrList.Add(selectedHeader);
                                }
                                //==============================================================================================================
                                // When processing selected placeholders/headers, the assortment header that includes the selected headers
                                // also needs to be included. This looks up and adds the Assortment header.
                                //==============================================================================================================
                                if (AssortmentAction)
                                {
                                    if (!selectedHdrList.Contains(ap.AsrtRID))
                                    {
                                        SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ap.AsrtRID);
                                        assortmentHeader.HeaderType = eHeaderType.Assortment;
                                        selectedHdrList.Add(assortmentHeader);
                                    }
                                }
                            }
                        }
                    }
                    else if (!IsGroupAllocation)
                    {
                        if (actionRid == (int)eAssortmentSelectableActionType.OpenReview)
                        {
                            foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                            {
                                if (ho.HeaderType != eHeaderType.Assortment)
                                {
                                    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                    selectedHeader.HeaderType = ho.HeaderType;
                                    selectedHdrList.Add(selectedHeader);
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (AllocationHeaderProfile ho in _asrtCubeGroup.GetHeaderList().ArrayList)
                    {
                        if (actionForHeadersOnly)
                        {
                            if (ho.HeaderType == eHeaderType.Assortment || ho.HeaderType == eHeaderType.Placeholder)
                            {
                                // Skip these types of headers
                            }
                            else
                            {
                                // Add header
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                selectedHdrList.Add(selectedHeader);
                                if (AssortmentAction)
                                {
                                    if (ho.AsrtRID != Include.NoRID)
                                    {
                                        if (!selectedHdrList.Contains(ho.AsrtRID))
                                        {
                                            SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                                            assortmentHeader.HeaderType = eHeaderType.Assortment;
                                            selectedHdrList.Add(assortmentHeader);
                                        }
                                    }
                                }
                            }
                        }
                        else if (useHeadersAndPlaceholdersOnly)
                        {
                            if (ho.HeaderType == eHeaderType.Assortment)
                            {
                                continue;
                            }
                            if (_selectedHeaderList.FindKey(ho.Key) != null)
                            {
                                SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                selectedHeader.HeaderType = ho.HeaderType;
                                selectedHdrList.Add(selectedHeader);
                            }
                        }
                        else
                        {
                            foreach (SelectedHeaderProfile shp in _selectedHeaderList)
                            {
                                if (ho.Key == shp.Key)
                                {
                                    SelectedHeaderProfile selectedHeader = new SelectedHeaderProfile(ho.Key);
                                    selectedHeader.HeaderType = ho.HeaderType;
                                    selectedHdrList.Add(selectedHeader);

                                    if (ho.AsrtRID != Include.NoRID)
                                    {
                                        if (!selectedHdrList.Contains(ho.AsrtRID))
                                        {
                                            SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                                            assortmentHeader.HeaderType = eHeaderType.Assortment;
                                            selectedHdrList.Add(assortmentHeader);
                                        }
                                    }
                                }
                            }

                            if (selectedHdrList.Count == 0)
                            {
                                SelectedHeaderProfile assortmentHeader = new SelectedHeaderProfile(ho.AsrtRID);
                                assortmentHeader.HeaderType = eHeaderType.Assortment;
                                selectedHdrList.Add(assortmentHeader);
                            }
                        }
                    }
                }
            }

            if (createTransProfileList)
            {
                TransactionProfileList_Load(selectedHdrList, aTrans, AssortmentAction);
            }

            return selectedHdrList;
        }

        public void TransactionProfileList_Load(SelectedHeaderList aSelectedHeaderList, ApplicationSessionTransaction aTrans, bool isAssortmentAction)
        {
            TransactionProfileList_RemoveAll(aTrans);
            AllocationProfileList apl = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.AssortmentMember);
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                if (ap.HeaderType != eHeaderType.Assortment || (isAssortmentAction && ap.HeaderType == eHeaderType.Assortment)
                    || (IsGroupAllocation && ap.HeaderType != eHeaderType.Placeholder))
                {
                    if (aSelectedHeaderList.Contains(ap.Key))
                    {
                        aTrans.AddAllocationProfile(ap);
                        aTrans.AddAllocationProfileToGrandTotal(ap);
                    }
                }
            }

            AllocationProfileList headerList = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.Allocation);
            AllocationSubtotalProfile grandTotal = aTrans.GetAllocationGrandTotalProfile();

        }

        public void TransactionProfileList_RemoveAll(ApplicationSessionTransaction aTrans)
        {
            AllocationProfileList apl = (AllocationProfileList)aTrans.GetMasterProfileList(eProfileType.Allocation);
            foreach (AllocationProfile ap in apl.ArrayList)
            {
                aTrans.RemoveAllocationProfileFromGrandTotal(ap);
            }
            aTrans.NewAllocationMasterProfileList();
        }

        //TO DO Need to check the code in AllocationReview- can be replaced or not.
        private bool IsEligibleToProcessAllocationActionType(eAllocationActionType aAction, SelectedHeaderList allocHdrList)
        {
            string errorMessage = string.Empty;
            string errorParm = string.Empty;
            bool isEligibleToProcess = true;


            AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

            foreach (SelectedHeaderProfile shp in allocHdrList)
            {
                AllocationProfile ap = (AllocationProfile)alp.FindKey(shp.Key);

                if (!IsGroupAllocation && ap.HeaderType == eHeaderType.Assortment)
                    continue;

                switch (ap.HeaderAllocationStatus)
                {
                    case eHeaderAllocationStatus.ReceivedOutOfBalance:
                        if (aAction != eAllocationActionType.BackoutAllocation)
                        {
                            _returnCode = eROReturnCode.Failure;
                            isEligibleToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.ReleaseApproved:
                        if (aAction != eAllocationActionType.Reset &&
                            aAction != eAllocationActionType.Release)
                        {
                            _returnCode = eROReturnCode.Failure;
                            isEligibleToProcess = false;
                        }
                        break;
                    case eHeaderAllocationStatus.Released:
                        if (aAction != eAllocationActionType.Reset)
                        {
                            _returnCode = eROReturnCode.Failure;
                            isEligibleToProcess = false;
                        }
                        break;
                    default:
                        if (aAction == eAllocationActionType.Reset)
                        {
                            _returnCode = eROReturnCode.Failure;
                            isEligibleToProcess = false;
                        }
                        else if (aAction == eAllocationActionType.ChargeIntransit)
                        {
                            if (ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllInBalance
                                && ap.HeaderAllocationStatus != eHeaderAllocationStatus.AllocatedInBalance)
                            {
                                _returnCode = eROReturnCode.Failure;
                                isEligibleToProcess = false;
                            }
                        }
                        break;
                }
                if (!isEligibleToProcess)
                {
                    errorMessage = string.Format
                        (MIDText.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction),
                        MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
                    _sROMessage = errorMessage;
                    _returnCode = eROReturnCode.Failure;
                    isEligibleToProcess = false;
                    break;
                }
                if (isEligibleToProcess)
                {
                    if (!SAB.ClientServerSession.GlobalOptions.IsReleaseable(ap.HeaderType)
                        && aAction == eAllocationActionType.Release)
                    {
                        if (ap.IsDummy)
                        {
                            errorParm = MIDText.GetTextOnly((int)eHeaderType.Dummy) + " " + MIDText.GetTextOnly(eMIDTextCode.lbl_Header);
                        }
                        else
                        {
                            errorParm = MIDText.GetTextOnly((int)ap.HeaderType);
                        }
                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_ActionNotAllowed), errorParm);
                        _sROMessage = errorMessage;
                        _returnCode = eROReturnCode.Failure;
                        isEligibleToProcess = false;
                        break;
                    }
                }

                if (isEligibleToProcess)
                {
                    if (aAction == eAllocationActionType.BackoutAllocation
                        || aAction == eAllocationActionType.BackoutSizeAllocation
                        || aAction == eAllocationActionType.BackoutSizeIntransit
                        || aAction == eAllocationActionType.BackoutStyleIntransit
                        )
                    {
                        if (IsGroupAllocation)
                        {
                            if (ap.AsrtType == (int)eAssortmentType.GroupAllocation)
                            {
                                foreach (AllocationProfile member in alp)
                                {
                                    if (member.IsMasterHeader && member.DCFulfillmentProcessed)
                                    {
                                        errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                                        _sROMessage = errorMessage;
                                        _returnCode = eROReturnCode.Failure;
                                        isEligibleToProcess = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                                {
                                    errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentProcessedActionNotAllowed), ap.HeaderID);
                                    _sROMessage = errorMessage;
                                    _returnCode = eROReturnCode.Failure;
                                    isEligibleToProcess = false;
                                    break;
                                }
                                else if (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                                {
                                    errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_al_DCFulfillmentNotProcessedActionNotAllowed), ap.HeaderID);
                                    _sROMessage = errorMessage;
                                    _returnCode = eROReturnCode.Failure;
                                    isEligibleToProcess = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            //if (isEligibleToProcess)
            //{
            //    if (aAction == eAllocationActionType.BackoutAllocation
            //        || aAction == eAllocationActionType.BackoutSizeAllocation
            //        || aAction == eAllocationActionType.BackoutSizeIntransit
            //        || aAction == eAllocationActionType.BackoutStyleIntransit
            //        || aAction == eAllocationActionType.Reset)
            //    {

            //        errorMessage = string.Format(MIDText.GetTextOnly(eMIDTextCode.msg_ActionWarning),
            //                MIDText.GetTextOnly((int)aAction));
            //        _sROMessage = errorMessage;
            //        _returnCode = eROReturnCode.Failure;

            //        isEligibleToProcess = false;
            //        foreach (SelectedHeaderProfile shp in allocHdrList)
            //        {
            //            _applicationSessionTransaction.SetAllocationActionStatus(shp.Key, eAllocationActionStatus.NoActionPerformed);
            //        }
            //        _cancelAllocationCancelled = true;
            //    }
            //}

            if (isEligibleToProcess)
            {
                isEligibleToProcess = VerifySecurity2(allocHdrList);
            }

            return isEligibleToProcess;
        }
        private bool VerifySecurity2(SelectedHeaderList selectedHeaderList)
        {
            HierarchyNodeSecurityProfile hierNodeSecProfile;
            try
            {
                bool allowUpdate = true;
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    AllocationProfile ap = _applicationSessionTransaction.GetAssortmentMemberProfile(shp.Key);
                    if (ap != null && ap.StyleHnRID > 0)
                    {
                        hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(ap.StyleHnRID, (int)eSecurityTypes.Allocation);
                        if (!hierNodeSecProfile.AllowUpdate)
                        {
                            HierarchyNodeProfile hnp = SAB.HierarchyServerSession.GetNodeData(shp.StyleHnRID, false, false);
                            allowUpdate = false;
                            string errorMessage = MIDText.GetText(eMIDTextCode.msg_NotAuthorizedForNode);
                            errorMessage = errorMessage + " Node: " + hnp.Text;
                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_NotAuthorizedForNode, errorMessage, "Assortment View");
                            break;
                        }
                    }
                }
                return allowUpdate;
            }
            catch
            {
                throw;
            }
        }
        private void SetActivateAssortmentOnHeaders(ProfileList selectedHdrList, bool isActive)
        {
            ArrayList hdrList = new ArrayList();
            foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
            {
                hdrList.Add(shp.Key);
            }
            SetActivateAssortmentOnHeaders(hdrList, isActive);
        }
        private void SetActivateAssortmentOnHeaders(ArrayList selectedHdrList, bool isActive)
        {
            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
            foreach (int hdrRid in selectedHdrList)
            {
                AllocationProfile ap = (AllocationProfile)apl.FindKey(hdrRid);
                if (ap != null)
                {
                    if (ap.HeaderType != eHeaderType.Placeholder && ap.HeaderType != eHeaderType.Assortment)
                    {
                        ap.ActivateAssortment = isActive;
                    }
                }
            }
        }
        private SelectedHeaderList ProcessAllocationMethod(ref ApplicationSessionTransaction actionTransaction, int action, GeneralComponent aComponent,
            ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent, double aTolerancePercent, int aStoreFilter,
            int aWorkFlowStepKey, ROAssortmentAllocationActionParms assortmentAllocationActionParms)
        {
            SelectedHeaderList selectedHdrList = GetSelectableHeaderList(action, actionTransaction, true, assortmentAllocationActionParms);
            try
            {
                //=============================================================================
                // Get a header key list of just the allocation headers in the Header List
                //=============================================================================
                if (selectedHdrList.Count == 0)
                {
                    string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
                        errorMessage, "Assortment View");
                    return selectedHdrList;
                }
                _assortmentProfile.GroupAllocationProcessAs = eGroupAllocationProcessAs.Group;
                if (IsProcessAsHeaders)
                {
                    _assortmentProfile.GroupAllocationProcessAs = eGroupAllocationProcessAs.Headers;
                }

                _cancelAllocationCancelled = false;
                if (!IsEligibleToProcessAllocationActionType((eAllocationActionType)action, selectedHdrList))
                {
                    return selectedHdrList;
                }
                else
                {
                    actionTransaction.AssortmentSelectedHdrList = selectedHdrList;
                }

                SetActivateAssortmentOnHeaders(selectedHdrList, true);


                if (_applicationSessionTransaction.AllocationFilterID != Include.NoRID)
                { aStoreFilter = _applicationSessionTransaction.AllocationFilterID; }
                else { aStoreFilter = Include.AllStoreFilterRID; }

                AllocationWorkFlowStep aAllocationWorkFlowStep
                    = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                actionTransaction.DoAllocationAction(aAllocationWorkFlowStep);

                if (action != (int)eAssortmentAllocationActionType.BackoutAllocation
                    && action != (int)eAssortmentAllocationActionType.BackoutSizeAllocation
                    && action != (int)eAssortmentAllocationActionType.BackoutStyleIntransit
                    && action != (int)eAssortmentAllocationActionType.BackoutSizeIntransit
                    )
                {
                    OutStoresInBlockedStyles(selectedHdrList);
                }
                SetActivateAssortmentOnHeaders(selectedHdrList, false);
            }
            catch (MIDException MIDexc)
            {
                _sROMessage = MIDexc.ToString();
                throw;
            }
            catch (SpreadFailed err)
            {
                _sROMessage = err.ToString();
            }
            catch (HeaderInUseException err)
            {
                string headerListMsg = string.Empty;
                foreach (string headerId in err.HeaderList)
                {
                    if (headerListMsg.Length > 0)
                        headerListMsg += ", " + headerId;
                    else
                        headerListMsg = " " + headerId;
                }
                _sROMessage = err.Message;
            }
            catch (Exception err)
            {
                _sROMessage = err.Message;
            }
            finally
            {
                _assortmentProfile.GroupAllocationProcessAs = eGroupAllocationProcessAs.Unknown;
            }
            return selectedHdrList;
        }

        private SelectedHeaderList ProcessAllocationWorkflow(ref ApplicationSessionTransaction actionTransaction, int action, GeneralComponent aComponent,
            ApplicationBaseAction aMethod, bool aReviewFlag, bool aUseSystemTolerancePercent, double aTolerancePercent, int aStoreFilter,
            int aWorkFlowStepKey, ROAssortmentAllocationActionParms assortmentAllocationActionParms)
        {
            // set so legacy code thinks it is coming from assortment
            AssortmentActiveProcessToolbarHelper.ActiveProcess = new AssortmentActiveProcessToolbarHelper.AssortmentScreen(screenTitle: "Web", screenID: assortmentAllocationActionParms.AssortmentTabType.GetHashCode(), screenType: "Assortment", form: null);

            SelectedHeaderList selectedHdrList = GetSelectableHeaderList(actionRid: action, aTrans: actionTransaction, createTransProfileList: true, parms: assortmentAllocationActionParms, useHeadersAndPlaceholdersOnly: true);
            try
            {
                //=============================================================================
                // Get a header key list of just the allocation headers in the Header List
                //=============================================================================
                if (selectedHdrList.Count == 0)
                {
                    string errorMessage = MIDText.GetText(eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected);
                    SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, eMIDTextCode.msg_as_NoHeadersPlaceholdersSelected,
                        errorMessage, "Assortment View");
                    return selectedHdrList;
                }

                _applicationSessionTransaction.ProcessAllocationWorkflow(assortmentAllocationActionParms.WorkflowKey, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
                eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;
                _sROMessage = MIDText.GetTextOnly((int)actionStatus);

                if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                    || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                {
                    _returnCode = eROReturnCode.Failure;
                }
            }
            catch (MIDException MIDexc)
            {
                _sROMessage = MIDexc.ToString();
                throw;
            }
            catch (SpreadFailed err)
            {
                _sROMessage = err.ToString();
            }
            catch (HeaderInUseException err)
            {
                string headerListMsg = string.Empty;
                foreach (string headerId in err.HeaderList)
                {
                    if (headerListMsg.Length > 0)
                        headerListMsg += ", " + headerId;
                    else
                        headerListMsg = " " + headerId;
                }
                _sROMessage = err.Message;
            }
            catch (Exception err)
            {
                _sROMessage = err.Message;
            }
            finally
            {
                _assortmentProfile.GroupAllocationProcessAs = eGroupAllocationProcessAs.Unknown;
            }
            return selectedHdrList;
        }

        private void OutStoresInBlockedStyles(SelectedHeaderList selectedHdrList)
        {
            AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

            ArrayList blockedKeyList = new ArrayList();
            IDictionaryEnumerator iEnum = null;
            BlockedListHashKey blockedKey = null;
            bool didUpdateHdr = false;
            foreach (SelectedHeaderProfile shp in selectedHdrList.ArrayList)
            {
                AllocationProfile ap = (AllocationProfile)apl.FindKey(shp.Key);
                if (ap != null)
                {
                    iEnum = _asrtCubeGroup.BlockedList.GetEnumerator();

                    while (iEnum.MoveNext())
                    {
                        blockedKey = (BlockedListHashKey)iEnum.Key;
                        if (blockedKey.PlaceholderRID == ap.Key)
                        {
                            ProfileList gradeStoreList = _asrtCubeGroup.GetStoresInSetGrade(blockedKey.StrGrpLvlRID, blockedKey.GradeRID);
                            ProfileList storeList = new ProfileList(eProfileType.Store);
                            foreach (StoreProfile sp in gradeStoreList)
                            {
                                if (sp.Key != _applicationSessionTransaction.ReserveStore.RID)
                                {
                                    storeList.Add(sp);
                                }
                            }
                            ap.SetAllocatedUnits(storeList, 0);
                            didUpdateHdr = true;
                        }
                    }
                }
                if (didUpdateHdr)
                {
                    if (!ap.WriteHeader())
                    {
                        EnqueueError(ap);
                        return;
                    }
                }
            }
        }

        private void EnqueueError(AllocationProfile ap)
        {
            _enqueueHeaderError = true;
            // Message to audit
            string msgText = string.Format(MIDText.GetText(eMIDTextCode.msg_EnqueueFailedForHeader), ap.HeaderID);
            SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, msgText, this.GetType().Name);
            // Message to user
            msgText = string.Format(MIDText.GetText((int)eAllocationActionStatus.NoHeaderResourceLocks));
            _sROMessage = msgText;
            MIDEnvironment.Message = msgText;
            MIDEnvironment.requestFailed = true;
        }

        protected bool SaveChanges()
        {
            bool headerChanged;
            bool placeholderChanged;

            try
            {
                headerChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasHeaderCubeChanged();
                placeholderChanged = ((AssortmentCubeGroup)_asrtCubeGroup).hasPlaceholderCubeChanged();

                if (headerChanged || placeholderChanged)
                {
                    _asrtCubeGroup.SaveCubeGroup();
                }

                _asrtCubeGroup.SaveBlockedStyles();

            }
            catch (Exception exc)
            {
                throw;
            }
            return true;
        }
        #endregion

        #region Assortment Update Content Characteristics

        private ROOut UpdateAssortmentContentCharacteristics(ROAssortmentUpdateContentCharacteristicsParms rOAssortmentUpdateContentCharacteristicsParms)
        {
            try
            {
                string message;
                if (rOAssortmentUpdateContentCharacteristicsParms.ROAllocationHeaderSummary != null)
                {
                    var result = SetAssortmentProfile(rOAssortmentUpdateContentCharacteristicsParms);
                }

                if (rOAssortmentUpdateContentCharacteristicsParms.ContentUpdateRequests != null
                    && rOAssortmentUpdateContentCharacteristicsParms.ContentUpdateRequests.Count > 0)
                {
                    var result = UpdateAssortment(rOAssortmentUpdateContentCharacteristicsParms.ContentUpdateRequests, out message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (!MIDEnvironment.requestFailed)
                {
                    OnAssortmentSaveHeaderData(null);
                }
            }

            return GetAssortmentContentCharacteristics();
        }

        internal AllocationProfileList SetAssortmentProfile(ROAssortmentUpdateContentCharacteristicsParms rOAssortmentUpdateContentCharacteristicsParms)
        {
            try
            {
                string message = string.Empty;

                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                foreach (ROAllocationHeaderSummary lstItem in rOAssortmentUpdateContentCharacteristicsParms.ROAllocationHeaderSummary)
                {
                    if (lstItem != null)
                    {
                        AllocationProfile ap = GetAllocationProfile(lstItem.Key);
                        ap.HeaderID = lstItem.HeaderID;
                        ap.HeaderDescription = lstItem.HeaderDescription;
                        ap.UnitRetail = lstItem.UnitRetail;
                        ap.UnitCost = lstItem.UnitCost;

                        if (ap.StyleHnRID != lstItem.StyleHnRID)
                        {
                            HierarchyNodeSecurityProfile securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lstItem.StyleHnRID, (int)eSecurityTypes.Allocation);
                            if (!securityNode.AllowUpdate)
                            {
                                MIDEnvironment.Message = (SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_NotAuthorizedForNode));
                                MIDEnvironment.requestFailed = true;
                                break;
                            }

                            HierarchyNodeProfile styleHnp = SAB.HierarchyServerSession.GetNodeData(MainHp.Key, lstItem.StyleHnRID);
                            HierarchyNodeProfile productHnp = SAB.HierarchyServerSession.GetNodeData(styleHnp.HomeHierarchyParentRID);

                            if (styleHnp.LevelType != eHierarchyLevelType.Style)
                            {
                                MIDEnvironment.Message = string.Format(MIDText.GetText(eMIDTextCode.msg_Data1NotValidData2),
                                                   styleHnp.Text, _hlpStyle.LevelID);
                                MIDEnvironment.requestFailed = true;
                                break;
                            }

                            if (ap.Placeholder)
                            {
                                if (!ValidPlaceholderStyle(lstItem.StyleHnRID, ref message))
                                {
                                    MIDEnvironment.Message = message;
                                    MIDEnvironment.requestFailed = true;
                                    break;
                                }

                            }

                            ap.StyleHnRID = styleHnp.Key;
                            ap.HeaderDescription = styleHnp.NodeDescription;

                            if (!styleHnp.IsVirtual)
                            {
                                AddColorAndSizesToStyle(styleHnp.Key, lstItem, ap);
                            }
                        }
                        
                        ap.Vendor = lstItem.Vendor;
                        ap.PurchaseOrder = lstItem.PurchaseOrder;
                        ap.SizeGroupRID = lstItem.SizeGroupRID;
                        ap.API_WorkflowRID = lstItem.API_WorkflowRID;
                        ap.DistributionCenter = lstItem.DistributionCenter;
                        ap.AllocationNotes = lstItem.AllocationNotes;
                        if (_assortmentProfile != null
                            && ap.AsrtRID != _assortmentProfile.Key)
                        {
                            ap.AsrtRID = _assortmentProfile.Key;
                        }
                        ap.UnitsPerCarton = lstItem.UnitsPerCarton;
                        
                        //BULK DETAILS
                        for (int j = 0; j < lstItem.BulkColorDetails.Count; j++)
                        {
                            var bulkInfo = lstItem.BulkColorDetails[j];

                            //if (bulkInfo.ChangeType == eChangeType.add)
                            //{
                            //    ap.AddBulkColor((int)bulkInfo.AsrtBCRID, (int)bulkInfo.ReserveUnits, (int)bulkInfo.Sequence);

                            //    foreach (var sizeInfo in lstItem.BulkColorDetails[j].BulkColorSizeProfile)
                            //    {
                            //        ap.AddBulkSizeToColor((int)bulkInfo.AsrtBCRID, (int)sizeInfo.HDR_BCSZ_Key, (int)sizeInfo.Units, (int)sizeInfo.Sequence);
                            //    }
                            //}

                            if (bulkInfo.ContentUpdateRequests != null
                                && bulkInfo.ContentUpdateRequests.Count > 0)
                            {
                                eAllocationActionStatus result = UpdateBulkColor(ap, bulkInfo.Color.Key, lstItem.SizeGroupRID, bulkInfo.ContentUpdateRequests, out message);
                                if (result != eAllocationActionStatus.ActionCompletedSuccessfully)
                                {
                                    MIDEnvironment.Message = message;
                                    MIDEnvironment.requestFailed = true;
                                    break;
                                }
                            }
                        }

                        if (MIDEnvironment.requestFailed)
                        {
                            break;
                        }

                        //PACK DETAILS
                        for (int i = 0; i < lstItem.PackDetails.Count; i++)
                        {
                            var packInfo = lstItem.PackDetails[i];


                            ap.AddPack(packInfo.Pack.Value, eAllocationType.DetailType, (int)packInfo.Multiple, (int)packInfo.ReservePacks, (int)packInfo.Sequence);

                            foreach (var colorInfo in lstItem.PackDetails[i].ColorsInfo)
                            {
                                if (packInfo.ChangeType == eChangeType.add)
                                {
                                    ap.AddColorToPack(colorInfo.ColorName, (int)colorInfo.ColorCodeRID, (int)colorInfo.Units, (int)colorInfo.Sequence);

                                    for (int j = 0; j < colorInfo.Sizes.Count; j++)
                                    {
                                        var pckColoSizeInfo = colorInfo.Sizes[j];
                                        string pckColorSize = colorInfo.Sizes[j].Size.Value;
                                        ap.AddSizeToPackColor(colorInfo.ColorName, (int)colorInfo.ColorCodeRID, Convert.ToInt32(pckColorSize), (int)pckColoSizeInfo.Units, (int)pckColoSizeInfo.Sequence);
                                    }
                                }
                            }
                        }

                        eHeaderType headerType = lstItem.HeaderType;
                        string headerTypeText = MIDText.GetTextOnly(Convert.ToInt32(lstItem.HeaderType));
                        bool canView = false;
                        bool canUpdate = false;
                        switch (headerType)
                        {
                            case eHeaderType.Assortment:
                            case eHeaderType.Placeholder:
                                canUpdate = _assortmentSecurity.AllowUpdate;
                                canView = _assortmentSecurity.AllowView;
                                break;

                            default:
                                HierarchyNodeSecurityProfile securityNode = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(lstItem.StyleHnRID, (int)eSecurityTypes.Allocation);
                                canUpdate = securityNode.AllowUpdate;
                                canView = securityNode.AllowView;
                                break;
                        }

                        //HEADER CHARACTERISTICS
                        ArrayList charArrayList = new ArrayList();
                        foreach (ROAllocationHeaderCharacteristic item in lstItem.HeaderCharacteristics)
                        {
                            if (item.HeaderCharKey > 0)
                            {
                                charArrayList.Add(item.HeaderCharKey);
                                ap.AddCharacteristic(item.HeaderCharGroupName, item.HeaderCharValue, eStoreCharType.unknown);
                            }
                        }
                        SAB.HeaderServerSession.RefreshHeaderCharacteristics(ap.Key, charArrayList);

                        //PRODUCT CHARACTERISTICS AND DIGITAL ASSET KEYS

                        bool pendingChanges = false;
                        // STYLE
                        HierarchyNodeProfile hnp_style = SAB.HierarchyServerSession.GetNodeData(lstItem.StyleHnRID);
                        NodeCharProfile ncp;
                        NodeCharProfileList nodeCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics(ap.StyleHnRID);
                        foreach (ROCharacteristic item in lstItem.ProductCharacteristics)
                        {
                            ncp = (NodeCharProfile)nodeCharProfileList.FindKey(item.CharacteristicGroup.Key);
                            if (ncp == null)
                            {
                                ncp = new NodeCharProfile(item.CharacteristicGroup.Key)
                                {
                                    ProductCharChangeType = eChangeType.add,
                                    ProductCharID = item.CharacteristicGroup.Value,
                                    ProductCharValueRID = item.CharacteristicValue.Key,
                                    ProductCharValue = item.CharacteristicValue.Value,
                                };
                                nodeCharProfileList.Add(ncp);
                            }
                            else if (item.CharacteristicValue.Key > 0)
                            {
                                if (ncp.ProductCharValueRID != item.CharacteristicValue.Key
                                    || ncp.ProductCharValue != item.CharacteristicValue.Value
                                   )
                                {
                                    ncp.ProductCharValueRID = item.CharacteristicValue.Key;
                                    ncp.ProductCharValue = item.CharacteristicValue.Value;
                                    ncp.ProductCharChangeType = eChangeType.update;
                                    pendingChanges = true;
                                }
                            }
                            else
                            {
                                if (ncp.ProductCharValueRID != 0)
                                {
                                    ncp.ProductCharChangeType = eChangeType.delete;
                                    pendingChanges = true;
                                }
                            }
                        }
                        EditMsgs em = new EditMsgs();
                        SAB.HierarchyServerSession.UpdateProductCharacteristics(ap.StyleHnRID, nodeCharProfileList);
                        if (lstItem.DigitalAssetKey != hnp_style.DigitalAssetKey)
                        {
                            hnp_style.DigitalAssetKey = lstItem.DigitalAssetKey;
                            //hnp_style.NodeChangeType = eChangeType.update;
                            //SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp_style);
                            //_hierMaint.ProcessNodeProfileInfo(ref em, hnp_style);
                            pendingChanges = true;
                        }

                        if (pendingChanges)
                        {
                            hnp_style.NodeChangeType = eChangeType.update;
                            HierMaint.ProcessNodeProfileInfo(ref em, hnp_style);
                        }


                        int colorHnRID = Include.NoRID;
                        foreach (ROHeaderBulkColorDetails color in lstItem.BulkColorDetails)
                        {
                            ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(color.Color.Key);

                            if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                            {
                                pendingChanges = false;
                                nodeCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics(colorHnRID);
                                foreach (ROCharacteristic item in color.ProductCharacteristics)
                                {
                                    ncp = (NodeCharProfile)nodeCharProfileList.FindKey(item.CharacteristicGroup.Key);
                                    if (ncp == null)
                                    {
                                        ncp = new NodeCharProfile(item.CharacteristicGroup.Key)
                                        {
                                            ProductCharChangeType = eChangeType.add,
                                            ProductCharID = item.CharacteristicGroup.Value,
                                            ProductCharValueRID = item.CharacteristicValue.Key,
                                            ProductCharValue = item.CharacteristicValue.Value,
                                        };
                                        nodeCharProfileList.Add(ncp);
                                    }
                                    else if (item.CharacteristicValue.Key > 0)
                                    {
                                        if (ncp.ProductCharValueRID != item.CharacteristicValue.Key
                                            || ncp.ProductCharValue != item.CharacteristicValue.Value
                                           )
                                        {
                                            ncp.ProductCharValueRID = item.CharacteristicValue.Key;
                                            ncp.ProductCharValue = item.CharacteristicValue.Value;
                                            ncp.ProductCharChangeType = eChangeType.update;
                                            pendingChanges = true;
                                        }
                                    }
                                    else
                                    {
                                        if (ncp.ProductCharValueRID != 0)
                                        {
                                            ncp.ProductCharChangeType = eChangeType.delete;
                                            pendingChanges = true;
                                        }
                                    }
                                }
                                SAB.HierarchyServerSession.UpdateProductCharacteristics(colorHnRID, nodeCharProfileList);
                                HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
                                if (color.DigitalAssetKey != hnp_color.DigitalAssetKey)
                                {
                                    hnp_color.DigitalAssetKey = color.DigitalAssetKey == null ? Include.NoRID : (int)color.DigitalAssetKey;
                                    //hnp_color.NodeChangeType = eChangeType.update;
                                    ////SAB.HierarchyServerSession.NodeUpdateProfileInfo(hnp_color);
                                    //em = new EditMsgs();
                                    //_hierMaint.ProcessNodeProfileInfo(ref em, hnp_color);
                                    pendingChanges = true;
                                }

                                if (pendingChanges)
                                {
                                    hnp_color.NodeChangeType = eChangeType.update;
                                    em = new EditMsgs();
                                    HierMaint.ProcessNodeProfileInfo(ref em, hnp_color);
                                }
                            }
                        }

                        foreach (ROHeaderPackProfile pack in lstItem.PackDetails)
                        {
                            foreach (HeaderPackColor color in pack.ColorsInfo)
                            {
                                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(color.Color.Key);

                                if (SAB.HierarchyServerSession.ColorExistsForStyle(hnp_style.HomeHierarchyRID, hnp_style.Key, ccp.ColorCodeID, ref colorHnRID))
                                {
                                    pendingChanges = false;
                                    nodeCharProfileList = SAB.HierarchyServerSession.GetProductCharacteristics(colorHnRID);
                                    foreach (ROCharacteristic item in color.ProductCharacteristics)
                                    {
                                        ncp = (NodeCharProfile)nodeCharProfileList.FindKey(item.CharacteristicGroup.Key);
                                        if (ncp == null)
                                        {
                                            ncp = new NodeCharProfile(item.CharacteristicGroup.Key)
                                            {
                                                ProductCharChangeType = eChangeType.add,
                                                ProductCharID = item.CharacteristicGroup.Value,
                                                ProductCharValueRID = item.CharacteristicValue.Key,
                                                ProductCharValue = item.CharacteristicValue.Value,
                                            };
                                            nodeCharProfileList.Add(ncp);
                                        }
                                        else if (item.CharacteristicValue.Key > 0)
                                        {
                                            if (ncp.ProductCharValueRID != item.CharacteristicValue.Key
                                                || ncp.ProductCharValue != item.CharacteristicValue.Value
                                               )
                                            {
                                                ncp.ProductCharValueRID = item.CharacteristicValue.Key;
                                                ncp.ProductCharValue = item.CharacteristicValue.Value;
                                                ncp.ProductCharChangeType = eChangeType.update;
                                                pendingChanges = true;
                                            }
                                        }
                                        else
                                        {
                                            if (ncp.ProductCharValueRID != 0)
                                            {
                                                ncp.ProductCharChangeType = eChangeType.delete;
                                                pendingChanges = true;
                                            }
                                        }
                                    }
                                    SAB.HierarchyServerSession.UpdateProductCharacteristics(colorHnRID, nodeCharProfileList);
                                    HierarchyNodeProfile hnp_color = SAB.HierarchyServerSession.GetNodeData(colorHnRID);
                                    if (color.DigitalAssetKey != hnp_color.DigitalAssetKey)
                                    {
                                        hnp_color.DigitalAssetKey = color.DigitalAssetKey == null ? Include.NoRID : (int)color.DigitalAssetKey;
                                        pendingChanges = true;
                                    }

                                    if (pendingChanges)
                                    {
                                        hnp_color.NodeChangeType = eChangeType.update;
                                        em = new EditMsgs();
                                        HierMaint.ProcessNodeProfileInfo(ref em, hnp_color);
                                    }
                                }
                            }
                        }

                        if (lstItem.ContentUpdateRequests != null
                            && lstItem.ContentUpdateRequests.Count > 0)
                        {
                            eAllocationActionStatus result = UpdateHeader(ap, lstItem.ContentUpdateRequests, out message);
                            if (result != eAllocationActionStatus.ActionCompletedSuccessfully)
                            {
                                MIDEnvironment.Message = message;
                                MIDEnvironment.requestFailed = true;
                                break;
                            }
                        }

                        _allocProfileList.Add(ap);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _allocProfileList;
        }

        private bool ValidPlaceholderStyle(int aStyleHnRID, ref string errorMessage)
        {
            bool validStyle = true;
            try
            {
                AllocationProfileList alp = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);

                if (alp.Count > 0)
                {
                    foreach (AllocationProfile ap in alp)
                    {
                        if (ap.StyleHnRID == aStyleHnRID)
                        {
                            errorMessage = string.Format(MIDText.GetText(eMIDTextCode.msg_as_DupPhStylesNotAllowed),
                                                      _lblPlaceholder + " " + _hlpStyle.LevelID);
                            validStyle = false;
                            break;
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return validStyle;
        }

        private void AddColorAndSizesToStyle(int styleKey, ROAllocationHeaderSummary lstItem, AllocationProfile ap)
        {
            string errorMessage;
            EditMsgs em = null;

            foreach (ROHeaderBulkColorDetails color in lstItem.BulkColorDetails)
            {
                int colorCodeRID = color.Color.Key;
                ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeRID);
                if (ccp.Purpose == ePurpose.Default)  // Do not add if not real color
                {
                    string bulkColorID = color.Color.Value;
                    string bulkColorDescription = color.Description;
                    em = new EditMsgs();
                    int colorHnRID = HierMaint.QuickAdd(ref em, styleKey, bulkColorID, bulkColorDescription);
                    if (em.ErrorFound)
                    {
                        errorMessage = FormatMessage(em);
                        SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, this.GetType().Name);
                        return;
                    }
                    // Get sizes from header
                    HdrColorBin aPhColor = (HdrColorBin)ap.BulkColors[colorCodeRID];
                    foreach (HdrSizeBin aSize in aPhColor.ColorSizes.Values)
                    {
                        em = new EditMsgs();
                        SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.SizeCodeRID);
                        int sizeHnRID = HierMaint.QuickAdd(ref em, colorHnRID, scp.SizeCodeID);
                        if (em.ErrorFound)
                        {
                            errorMessage = FormatMessage(em);
                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, this.GetType().Name);
                            return;
                        }
                    }
                }
            }

            foreach (ROHeaderPackProfile pack in lstItem.PackDetails)
            {
                foreach (HeaderPackColor color in pack.ColorsInfo)
                {
                    int colorCodeRID = (int)color.ColorCodeRID;
                    ColorCodeProfile ccp = SAB.HierarchyServerSession.GetColorCodeProfile(colorCodeRID);
                    if (ccp.Purpose == ePurpose.Default)  // Do not add if not real color
                    {
                        string packName = pack.Pack.Value;
                        string packColorID = color.Color.Value;
                        string packColorDescription = color.ColorDescription;
                        em = new EditMsgs();
                        int colorHnRID = HierMaint.QuickAdd(ref em, styleKey, packColorID, packColorDescription);
                        if (em.ErrorFound)
                        {
                            errorMessage = FormatMessage(em);
                            SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, this.GetType().Name);
                            return;
                        }
                        // Get sizes from header
                        PackHdr apPackHdr = (PackHdr)ap.Packs[packName];
                        PackColorSize packColor = (PackColorSize)apPackHdr.PackColors[colorCodeRID];
                        foreach (HdrSizeBin aSize in packColor.ColorSizes.Values)
                        {
                            em = new EditMsgs();
                            SizeCodeProfile scp = SAB.HierarchyServerSession.GetSizeCodeProfile(aSize.SizeCodeRID);
                            int sizeHnRID = HierMaint.QuickAdd(ref em, colorHnRID, scp.SizeCodeID);
                            if (em.ErrorFound)
                            {
                                errorMessage = FormatMessage(em);
                                SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, errorMessage, this.GetType().Name);
                                return;
                            }
                        }
                    }
                }
            }
        }

        internal AllocationProfileList UpdateAssortment(List<ROUpdateContent> ContentUpdateRequests, out string message)
        {
            try
            {
                message = null;
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AssortmentMember);
                eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
                List<int> addKeys = new List<int>();
                List<int> deleteKeys = new List<int>();

                if (_applicationSessionTransaction.DataState == eDataState.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                    MIDEnvironment.requestFailed = true;
                    return _allocProfileList;
                }

                foreach (ROUpdateContent contentUpdate in ContentUpdateRequests)
                {
                    switch (contentUpdate.ContentType)
                    {
                        case eContentType.PlaceholderStyle:
                            switch (contentUpdate.ChangeType)
                            {
                                case eChangeType.add:
                                    if (contentUpdate.Count > 0)
                                    {
                                        if (OkToAddPlaceholders(ref message))
                                        {
                                            AddPlaceholders(AssortProfile, contentUpdate.Count, Include.NoRID, false, ref message);
                                            actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                        }
                                        else
                                        {
                                            actionStatus = eAllocationActionStatus.ActionFailed;
                                        }
                                    }
                                    else
                                    {
                                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_PlaceholderCountGreaterThanZero);
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    break;
                            }
                            break;
                        case eContentType.Header:
                            switch (contentUpdate.ChangeType)
                            {
                                case eChangeType.add:
                                    // collect all adds to perform at the same time
                                    addKeys.Add(contentUpdate.Key);
                                    actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    break;
                                case eChangeType.update:
                                    if (ReplaceHeader(AssortProfile, contentUpdate.Key, contentUpdate.NewKey, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }

                                    break;
                                case eChangeType.delete:
                                    deleteKeys.Add(contentUpdate.Key);
                                    actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    
                                    break;
                            }
                            break;
                    }
                    if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                    {
                        break;
                    }
                }

                if (addKeys.Count > 0)
                {
                    if (AddHeader(AssortProfile, addKeys, ref message))
                    {
                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                    }
                    else
                    {
                        actionStatus = eAllocationActionStatus.ActionFailed;
                    }
                }

                if (deleteKeys.Count > 0)
                {
                    if (DeleteHeader(AssortProfile, deleteKeys, ref message))
                    {
                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                    }
                    else
                    {
                        actionStatus = eAllocationActionStatus.ActionFailed;
                    }
                }

                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                {
                    FilterHeaderOptions headerFilterOptions = new FilterHeaderOptions();
                    headerFilterOptions.USE_WORKSPACE_FIELDS = true;
                    headerFilterOptions.filterType = filterTypes.AssortmentFilter;
                    SAB.HeaderServerSession.RebuildHeaderCharacteristicData(SharedRoutines.GetHeaderFilterForAssortmentView(), headerFilterOptions);

                    AllocationProfileList apl = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);

                    AllocationHeaderProfileList newHeaderList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                    if (apl != null)
                    {
                        foreach (object asrtObj in apl)
                        {
                            AllocationProfile ap = (AllocationProfile)asrtObj;
                            AllocationHeaderProfile ahp = SAB.HeaderServerSession.GetHeaderData(ap.Key, true, true, true);
                            if (!newHeaderList.Contains(ap.Key))
                            {
                                newHeaderList.Add(ahp);
                            }
                        }
                    }

                    _applicationSessionTransaction.RemoveMasterProfileList(newHeaderList);
                    _applicationSessionTransaction.SetMasterProfileList(newHeaderList);

                    _headerList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

                    _asrtCubeGroup.SaveCubeGroup();
                    ReloadGridData(reformatRows: true);
                    ComponentsChanged();
                }
                else
                {
                    if (message != null)
                    {
                        MIDEnvironment.Message = message;
                    }
                    MIDEnvironment.requestFailed = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _allocProfileList;
        }

        internal eAllocationActionStatus UpdateHeader(AllocationProfile ap, List<ROUpdateContent> ContentUpdateRequests, out string message)
        {
            eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
            message = null;
            try
            {
                if (_applicationSessionTransaction.DataState == eDataState.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                    return eAllocationActionStatus.ActionFailed;
                }

                foreach (ROUpdateContent contentUpdate in ContentUpdateRequests)
                {
                    actionStatus = eAllocationActionStatus.NoActionPerformed;
                    switch (contentUpdate.ContentType)
                    {
                        case eContentType.PlaceholderColor:
                            switch (contentUpdate.ChangeType)
                            {
                                case eChangeType.add:
                                    if (contentUpdate.Count > 0)
                                    {
                                        if (AddPlaceholderColors(eProfileType.HeaderBulkColor, AssortProfile, contentUpdate.Count, ap, ref message))
                                        {
                                            actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                        }
                                        else
                                        {
                                            actionStatus = eAllocationActionStatus.ActionFailed;
                                        }
                                    }
                                    else
                                    {
                                        message = SAB.ClientServerSession.Audit.GetText(eMIDTextCode.msg_al_PlaceholderColorCountGreaterThanZero);
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    break;
                                case eChangeType.update:
                                    if (ReplaceBulkColor(AssortProfile, ap, contentUpdate.Key, contentUpdate.NewKey, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }

                                    break;
                            }
                            break;
                        case eContentType.BulkColor:
                            switch (contentUpdate.ChangeType)
                            {
                                case eChangeType.add:
                                    if (AddBulkColor(ap, contentUpdate.Key, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    break;
                                case eChangeType.update:
                                    if (ReplaceBulkColor(AssortProfile, ap, contentUpdate.Key, contentUpdate.NewKey, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }

                                    break;
                                case eChangeType.delete:
                                    if ((eAssortmentType)AssortProfile.AsrtType == eAssortmentType.PostReceipt)
                                    {
                                        message = string.Format(MIDText.GetText(eMIDTextCode.msg_as_CannotRemoveColorFromPostReceipt), true);
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    else
                                    {
                                        if (DeleteBulkColor(ap, contentUpdate.Key, ref message))
                                        {
                                            actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                        }
                                        else
                                        {
                                            actionStatus = eAllocationActionStatus.ActionFailed;
                                        }
                                    }
                                    break;
                            }
                            break;
                    }

                    if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                actionStatus = eAllocationActionStatus.ActionFailed;
                throw ex;
            }

            return actionStatus;
        }

        internal eAllocationActionStatus UpdateBulkColor(AllocationProfile ap, int colorKey, int sizeGroupKey, List<ROUpdateContent> ContentUpdateRequests, out string message)
        {
            eAllocationActionStatus actionStatus = eAllocationActionStatus.NoActionPerformed;
            message = null;
            try
            {
                if (_applicationSessionTransaction.DataState == eDataState.ReadOnly)
                {
                    message = MIDText.GetText(eMIDTextCode.msg_DataNotLocked);
                    return eAllocationActionStatus.ActionFailed;
                }

                foreach (ROUpdateContent contentUpdate in ContentUpdateRequests)
                {
                    switch (contentUpdate.ContentType)
                    {
                        case eContentType.BulkColorSize:
                            switch (contentUpdate.ChangeType)
                            {
                                case eChangeType.add:
                                    if (AddSizeGroupToBulkColor(ap, colorKey, sizeGroupKey, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    break;
                                case eChangeType.delete:
                                    if (DeleteSizesFromBulkColor(ap, colorKey, ref message))
                                    {
                                        actionStatus = eAllocationActionStatus.ActionCompletedSuccessfully;
                                    }
                                    else
                                    {
                                        actionStatus = eAllocationActionStatus.ActionFailed;
                                    }
                                    break;
                            }
                            break;
                    }

                    if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                actionStatus = eAllocationActionStatus.ActionFailed;
                throw ex;
            }

            return actionStatus;
        }

        private AllocationProfile GetAllocationProfile(int aHeaderRID)
        {
            try
            {
                AllocationProfile ap = (AllocationProfile)_allocProfileList.FindKey(aHeaderRID);
                return ap;
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
