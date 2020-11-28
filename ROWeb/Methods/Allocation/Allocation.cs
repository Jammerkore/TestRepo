
using Logility.ROWebCommon;
using Logility.ROWebSharedTypes;
using MIDRetail.Business;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace Logility.ROWeb
{
    public partial class ROAllocation : ROWebFunction
    {
        private string _noSizeDimensionLbl;
        private string _lblQuantity;
        private FunctionSecurityProfile _allocationReviewSecurity = null;
        private FunctionSecurityProfile _allocationReviewStyleSecurity = null;
        private FunctionSecurityProfile _allocationReviewSummarySecurity = null;
        private FunctionSecurityProfile _allocationReviewSizeSecurity = null;
        private bool _headersLocked = false;
        private ROWorkflowMethodManager _ROWorkflowMethodManager = null;
        private bool _removedPctTotalRows = false;
        private bool _fromAssortment = false;
        private bool _allocationListFromAssortmentBuilt = false;
        private ClientSessionTransaction _clientTransaction = null;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROAllocation(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
        {
            _noSizeDimensionLbl = MIDText.GetTextOnly((int)eMIDTextCode.lbl_NoSecondarySize);
            _lblQuantity = MIDText.GetTextOnly(eMIDTextCode.lbl_Quantity);
            
        }

        override public void CleanUp()
        {
            if (_applicationSessionTransaction != null)
            {
                _applicationSessionTransaction.DequeueHeaders();
            }

            if (_ROWorkflowMethodManager != null)
            {
                _ROWorkflowMethodManager.CleanUp();
            }

        }

        private ClientSessionTransaction ClientTransaction
        {
            get
            {
                if (_clientTransaction == null)
                {
                    _clientTransaction = SAB.ClientServerSession.CreateTransaction();
                }
                return _clientTransaction;
            }
        }

        public GetMethods GetMethods
        {
            get
            {
                if (_getMethods == null)
                {
                    _getMethods = new GetMethods(SAB);
                }
                return _getMethods;
            }
        }

        public ROWorkflowMethodManager ROWorkflowMethodManager
        {
            get
            {
                if (_ROWorkflowMethodManager == null)
                {
                    _ROWorkflowMethodManager = new ROAllocationWorkflowMethodManager(SAB: SAB, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
                }
                return _ROWorkflowMethodManager;
            }
        }

        public ApplicationBaseMethod ABM
        {
            get
            {
                return ROWorkflowMethodManager.ABM;
            }
            set
            {
                ROWorkflowMethodManager.ABM = value;
            }
        }

        public ApplicationBaseWorkFlow ABW
        {
            get
            {
                return ROWorkflowMethodManager.ABW;
            }
            set
            {
                ROWorkflowMethodManager.ABW = value;
            }
        }

        public FunctionSecurityProfile AllocationReviewSecurity
        {
            get
            {
                if (_allocationReviewSecurity == null)
                {
                    _allocationReviewSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReview);
                }
                return _allocationReviewSecurity;
            }
        }

        public FunctionSecurityProfile AllocationReviewStyleSecurity
        {
            get
            {
                if (_allocationReviewStyleSecurity == null)
                {
                    _allocationReviewStyleSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewStyle);
                }
                return _allocationReviewStyleSecurity;
            }
        }

        public FunctionSecurityProfile AllocationReviewSizeSecurity
        {
            get
            {
                if (_allocationReviewSizeSecurity == null)
                {
                    _allocationReviewSizeSecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSize);
                }
                return _allocationReviewSizeSecurity;
            }
        }

        public FunctionSecurityProfile AllocationReviewSummarySecurity
        {
            get
            {
                if (_allocationReviewSummarySecurity == null)
                {
                    _allocationReviewSummarySecurity = SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationReviewSummary);
                }
                return _allocationReviewSummarySecurity;
            }
        }

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                // Common
                case eRORequest.SetAllocationSelectedHeaders:
                    return SetAllocationSelectedHeaders((ROListParms)Parms);
                case eRORequest.AllocationActions:
                    return GetAllocationActionsData();
                case eRORequest.AllocationViews:
                    return GetAllocationViewsData();
                case eRORequest.GetFilters:
                    return GetFilters(parms: (RONoParms)Parms);

                // Worklist
                case eRORequest.AllocationWorklistView:
                    return GetAllocationWorklistViewDetails(rOKeyParams: (ROKeyParms)Parms);
                case eRORequest.GetAllocationWorklistColumns:
                    return GetAllocationWorklistColumns();
                case eRORequest.GetAllocationHeaders:
                    return GetAllocationHeaderData();
                case eRORequest.GetAllocationSelectedHeaderDetails:
                    return GetSelectedHeaderDetails();
                case eRORequest.SetAllocationSelectedFilterHeaders:
                    return GetSelectedFilterHeaderList((ROKeyParms)Parms);
                case eRORequest.ProcessAllocationWorklistAction:
                    return ProcessAllocationWorklistAction(actionType: (ROIntParms)Parms);
                case eRORequest.GetUserLastValues:
                    return GetAllocationUserLastValues();
                case eRORequest.SaveUserLastValues:
                    return SaveUserLastValues((ROAllocationWorklistLastDataParms)Parms);

                // Selection
                case eRORequest.GetAllocationViewSelection:
                    return GetAllocationViewSeletionDetails(rOKeyParms: (ROKeyParms)Parms);

                // Style Review
                case eRORequest.AllocationStyleReviewViews:
                    return GetAllocationStyleReviewViewsInfo();
                case eRORequest.GetAllocationStyleReview:
                    return GetAllocationStyleReviewData((ROAllocationReviewOptionsParms)Parms);
                case eRORequest.UpdateStyleReviewChanges:
                    return UpdateStyleReview((ROGridChangesParms)Parms);
                case eRORequest.ProcessAllocationStyleReviewAction:
                    return ProcessAllocationStyleReviewAction((ROAllocationReviewOptionsParms)Parms);
                case eRORequest.SaveAllocationStyleReview:
                    return SaveAllocationStyleReview();

                // Size Review
                case eRORequest.GetAllocationSizeReviewViews:
                    return GetAllocationSizeReviewViews();
                case eRORequest.GetAllocationSizeReview:
                    return GetAllocationSizeReviewData((ROAllocationReviewOptionsParms)Parms);
                case eRORequest.UpdateSizeReviewChanges:
                    return UpdateSizeReview((ROGridChangesParms)Parms);
                case eRORequest.ProcessAllocationSizeReviewAction:
                    return ProcessAllocationSizeReviewAction(reviewOptionsParms: (ROAllocationReviewOptionsParms)Parms);
                case eRORequest.SaveAllocationSizeReview:
                    return SaveAllocationSizeReview();

                // Summary Review
                case eRORequest.GetAllocationSummaryReview:
                    return GetAllocationSummaryReviewData((ROAllocationReviewOptionsParms)Parms);
                case eRORequest.UpdateSummaryReviewChanges:
                    return UpdateSummaryReview((ROGridChangesParms)Parms);
                case eRORequest.ProcessAllocationSummaryReviewAction:
                    return ProcessAllocationSummaryReviewAction((ROAllocationReviewOptionsParms)Parms);
                case eRORequest.SaveAllocationSummaryReview:
                    return SaveAllocationSummaryReview();

                //Methods
                case eRORequest.GetMethod:
                    return ROWorkflowMethodManager.GetMethod((ROMethodParms)Parms);
                case eRORequest.SaveMethod:
                    return ROWorkflowMethodManager.SaveMethod((ROMethodPropertiesParms)Parms);
                case eRORequest.ApplyMethod:
                    return ROWorkflowMethodManager.ApplyMethod((ROMethodPropertiesParms)Parms);
                case eRORequest.ProcessMethod:
                    return ROWorkflowMethodManager.ProcessMethod((ROMethodParms)Parms);
                case eRORequest.CopyMethod:
                    return ROWorkflowMethodManager.CopyMethod((ROMethodParms)Parms);
                case eRORequest.DeleteMethod:
                    return ROWorkflowMethodManager.DeleteMethod((ROMethodParms)Parms);

                // Workflow
                case eRORequest.AllocationWorkflows:
                    return ROWorkflowMethodManager.GetWorkflowsData();
                case eRORequest.AllocationWorkFlowDetails:
                    return ROWorkflowMethodManager.GetWorkflowDetails(parms: (ROKeyParms)Parms);
                case eRORequest.SaveAllocationWorkflow:
                    return ROWorkflowMethodManager.SaveWorkflow(rOWorkflow: (ROWorkflowPropertiesParms)Parms);
                case eRORequest.DeleteWorkflow:
                    return ROWorkflowMethodManager.DeleteWorkflow(parms: (ROKeyParms)Parms);
                case eRORequest.GetAllocationWorkFlowMethodList:
                    return ROWorkflowMethodManager.GetWorkFlowMethodList((ROKeyParms)Parms);
                case eRORequest.GetAllocationWorkFlowMethodActionsList:
                    return GetAllocationWorkFlowMethodActionsList();
                case eRORequest.ProcessAllocationWorkflow:
                    return ProcessAllocationWorkflow(rOIntParms: (ROIntParms)Parms);
                case eRORequest.SaveAs:
                    return ROWorkflowMethodManager.SaveAs((RODataExplorerSaveAsParms)Parms);
            }

            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        

    }
}
