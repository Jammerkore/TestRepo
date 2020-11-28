using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {
        //=================
        // PRIVATE METHODS
        //=================
        private AllocationWorkFlow _allocationWorkflow;
        private string _workflowMethodName;
        private string _workflowMethodDescription;
        private string _workflowActionStatus;
        private eWorkflowMethodType _workflowMethodType;
        private bool _saveUserGridView = false;
        private DataTable _dtActions = null;
        private GetMethods _getMethods;


        #region "Process Allocation Workflow #RO-2905"

        private ROOut ProcessAllocationWorkflow(ROIntParms rOIntParms)
        {
            bool isFromExplorer = false;
            int aWorkflowRID = 0;
            bool successful = true;

            if (rOIntParms.ROInt != 0)
            {
                isFromExplorer = true;
                aWorkflowRID = rOIntParms.ROInt;
            }

            var rOAllocationHeaderSummaries = ProcessWorkflow(isFromExplorer, out successful, aWorkflowRID);

            if (rOAllocationHeaderSummaries.Count > 0
                && successful)
            {
                return new ROIListOut(eROReturnCode.Successful, _workflowActionStatus, ROInstanceID, rOAllocationHeaderSummaries);
            }

            return new ROIListOut(eROReturnCode.Failure, _workflowActionStatus, ROInstanceID, rOAllocationHeaderSummaries);
        }

        internal List<ROAllocationHeaderSummary> ProcessWorkflow(bool isFromExplorer, out bool successful, int aWorkflowRID = 0)
        {
            try
            {
                if (isFromExplorer)
                {
                    //Call method which is used to Process the Action from Explorer
                    _allocationWorkflow = new AllocationWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);
                    return ProcessWorkflowMethod(true, out successful);
                }
                else
                {
                    //Call method which is used to Process the Action from Process Button Click
                    return ProcessWorkflowMethod(false, out successful);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        internal List<ROAllocationHeaderSummary> ProcessWorkflowMethod(bool isFromExplorer, out bool successful)
        {
            successful = true;

            List<ROAllocationHeaderSummary> headerDetails = new List<ROAllocationHeaderSummary>();

            bool processedAlocWorkflow = false;

            bool useAssortment = false;

            try
            {
                _allocProfileList = (AllocationProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
                AllocationHeaderProfileList ahpl = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
                ahpl.LoadAll(_allocProfileList);
                _applicationSessionTransaction.SetMasterProfileList(ahpl);
                _allocationHeaderProfileList = (AllocationHeaderProfileList)_applicationSessionTransaction.GetMasterProfileList(eProfileType.AllocationHeader);

                if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)   // Use assortment
                {
                    useAssortment = true;
                }

                if (isFromExplorer)
                {
                    SetObject();
                }
                else
                {
                    SetCommonFields();
                    SetObject();
                }

                if (ABW.WorkFlowType == eWorkflowType.Allocation)
                {

                    if (useAssortment)
                    {
                        ApplicationSessionTransaction assortTrans = SAB.AssortmentTransactionEvent.GetAssortmentTransaction(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID);
                        SelectedHeaderList shl = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, eMethodType.GroupAllocation);
                        assortTrans.ProcessAllocationWorkflow(ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
                        eAllocationActionStatus actionStatus = assortTrans.AllocationActionAllHeaderStatus;
                        _workflowActionStatus = actionStatus.ToString();

                        string message = MIDText.GetTextOnly((int)actionStatus);

                        if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                            || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                        {
                            //CloseForms();
                        }
                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                        {
                            successful = true;
                        }
                        else
                        {
                            successful = false;
                        }
                    }
                    else
                    {
                        if (VerifySecurity())
                        {

                            string enqMessage = string.Empty;

                            if (_applicationSessionTransaction.EnqueueSelectedHeaders(out enqMessage))
                            {
                                _applicationSessionTransaction.ProcessAllocationWorkflow(ABW.Key, true, true, 1, eWorkflowProcessOrder.AllStepsForHeader);
                                eAllocationActionStatus actionStatus = _applicationSessionTransaction.AllocationActionAllHeaderStatus;
                                _workflowActionStatus = actionStatus.ToString();
                                string message = MIDText.GetTextOnly((int)actionStatus);

                                if (actionStatus == eAllocationActionStatus.HeaderEnqueueFailed
                                    || actionStatus == eAllocationActionStatus.NoHeaderResourceLocks)
                                {
                                    //CloseForms();
                                }
                                if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                                {
                                    successful = true;
                                }
                                else
                                {
                                    successful = false;
                                }
                            }
                            else
                            {
                                DoEnqueue();
                            }

                        }

                    }
                }
                else
                {
                    _applicationSessionTransaction.ProcessOTSPlanWorkflow(ABW.Key, true, true, 1);
                    eOTSPlanActionStatus actionStatus = _applicationSessionTransaction.OTSPlanActionStatus;
                    string message = MIDText.GetTextOnly((int)actionStatus);
                    if (actionStatus == eOTSPlanActionStatus.ActionCompletedSuccessfully)
                    {
                        successful = true;
                    }
                    else
                    {
                        successful = false;
                    }
                }

                processedAlocWorkflow = true;
            }
            catch (Exception ex)
            {
                processedAlocWorkflow = false;
            }
            finally
            {

            }

            if (processedAlocWorkflow)
            {
                var selectedHeaders = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();
                headerDetails = BuildHeaderDetails(selectedHeaders);
            }

            return headerDetails;

        }
        #endregion

        

        #region "Common Methods"
        internal bool VerifySecurity()
        {
            HierarchyNodeSecurityProfile hierNodeSecProfile;
            try
            {
                bool allowUpdate = true;
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)_applicationSessionTransaction.GetSelectedHeaders();
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {

                    hierNodeSecProfile = SAB.ClientServerSession.GetMyUserNodeSecurityAssignment(shp.StyleHnRID, (int)eSecurityTypes.Allocation);
                    if (!hierNodeSecProfile.AllowUpdate)
                    {
                        allowUpdate = false;
                        break;
                    }

                }
                return allowUpdate;
            }
            catch
            {
                throw;
            }
        }

        internal void SetObject()
        {
            try
            {
                ABW = _allocationWorkflow;
            }
            catch (Exception ex)
            {

            }
        }

        internal void DoEnqueue()
        {
            bool doEnqueue = true;

            try
            {
                SelectedHeaderList selectedHeaderList = (SelectedHeaderList)SAB.ClientServerSession.GetSelectedHeaderList();
                foreach (SelectedHeaderProfile shp in selectedHeaderList)
                {
                    if (shp.HeaderType != eHeaderType.Assortment)
                    {
                        if (shp.BypassEnqueue)
                        {
                            doEnqueue = false;
                        }
                        else
                        {
                            doEnqueue = true;
                            break;
                        }
                    }
                    else
                    {
                        selectedHeaderList.Remove(shp);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected eWorkflowMethodIND WorkflowMethodInd()
        {
            return eWorkflowMethodIND.Workflows;
        }

        public string WorkflowMethodName
        {
            get { return _workflowMethodName; }
            set { _workflowMethodName = value.Trim(); }
        }

        public string WorkflowMethodDescription
        {
            get { return _workflowMethodDescription; }
            set { _workflowMethodDescription = value.Trim(); }
        }
        private void SetCommonFields()
        {
            try
            {

                int selectedHeaderRID = _applicationSessionTransaction.GetAllocationProfileKeys()[0];
                AllocationProfile selectedAllocationProfile = _applicationSessionTransaction.GetAllocationProfile(selectedHeaderRID);

                _allocationWorkflow = new AllocationWorkFlow(SAB, selectedAllocationProfile.WorkflowRID, SAB.ClientServerSession.UserRID, false);

                ABW.Workflow_Change_Type = eChangeType.update;

            }
            catch (Exception ex)
            {
                //HandleException(ex);
            }
        }
        #endregion


        #region Method to get the Allocation WorkFlows Methods / Actions List
        private ROOut GetAllocationWorkFlowMethodActionsList()
        {
            try
            {
                List<KeyValuePair<int, string>> results = GetAllocationWorkFlowMethodActions();
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, results);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetAllocationWorkFlowMethodActions()
        {

            List<KeyValuePair<int, string>> allocationMethodActionsList = new List<KeyValuePair<int, string>>();
            DataView dv = new DataView(DtActions);

            CheckSecurityForActions(DtActions);
            dv.Sort = "TEXT_ORDER";
            foreach (DataRowView dr in dv)
            {
                bool addAction = true;
                if (!SAB.ClientServerSession.GlobalOptions.AppConfig.SizeInstalled)
                {
                    eSizeMethodType sizeMethodType = (eSizeMethodType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                    if (Enum.IsDefined(typeof(eSizeMethodType), sizeMethodType))
                    {
                        addAction = false;
                    }
                }

                eMethodType methodType = (eMethodType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if (methodType == eMethodType.WarehouseSizeAllocation ||
                    methodType == eMethodType.SizeOverrideAllocation)
                {
                    addAction = false;
                }

                if (addAction)
                {
                    int actionID = Convert.ToInt32(dr.Row["TEXT_CODE"]);
                    string actionName = Convert.ToString(dr.Row["TEXT_VALUE"]);
                    allocationMethodActionsList.Add(new KeyValuePair<int, string>(actionID, actionName));
                }
            }
            return allocationMethodActionsList;

        }

        private DataTable DtActions
        {
            get
            {
                if (_dtActions == null)
                {
                    _dtActions = MIDText.GetLabels((int)eAllocationMethodType.ApplyAPI_Workflow, (int)eAllocationMethodType.ApplyAPI_Workflow);
                    DataTable dt = MIDText.GetLabels((int)eAllocationMethodType.GeneralAllocation, (int)eAllocationMethodType.Release);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.SizeCurve, (int)eAllocationMethodType.BuildPacks);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.ReapplyTotalAllocation, (int)eAllocationMethodType.ReapplyTotalAllocation);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceSizeWithConstraints, (int)eAllocationMethodType.BreakoutSizesAsReceivedWithConstraints);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.GroupAllocation, (int)eAllocationMethodType.GroupAllocation);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.DCCartonRounding, (int)eAllocationMethodType.DCCartonRounding);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.CreateMasterHeaders, (int)eAllocationMethodType.CreateMasterHeaders);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.DCFulfillment, (int)eAllocationMethodType.DCFulfillment);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationMethodType.BalanceToVSW, (int)eAllocationMethodType.BalanceToVSW);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }

                    dt = MIDText.GetLabels((int)eAllocationActionType.BackoutAllocation, (int)eAllocationActionType.BackoutAllocation);
                    foreach (DataRow dr in dt.Rows)
                    {
                        _dtActions.Rows.Add(dr.ItemArray);
                    }
                }
                return _dtActions;
            }
        }
        #endregion

    }


}
