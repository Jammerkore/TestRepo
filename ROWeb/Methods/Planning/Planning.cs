using System;
using System.Collections.Generic;
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

namespace Logility.ROWeb
{
    public partial class ROPlanning : ROWebFunction
    {
        private ROWorkflowMethodManager _ROWorkflowMethodManager = null;
        private GetMethods _getMethods = null;
        private ClientSessionTransaction _clientTransaction = null;

        /// <summary>
        /// Creates an instance of the class 
        /// </summary>
        /// <param name="SAB">The SessionAddressBlock for this user and environment</param>
        /// <param name="ROWebTools">An instance of the ROWebTools</param>
        public ROPlanning(SessionAddressBlock SAB, ROWebTools ROWebTools)
            : base(SAB, ROWebTools)
		{
            
		}

        override public void CleanUp()
        {
            if (_ROWorkflowMethodManager != null)
            {
                _ROWorkflowMethodManager.CleanUp();
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

        public ROWorkflowMethodManager ROWorkflowMethodManager
        {
            get
            {
                if (_ROWorkflowMethodManager == null)
                {
                    _ROWorkflowMethodManager = new ROPlanningWorkflowMethodManager(SAB: SAB, applicationSessionTransaction: null, ROWebTools: ROWebTools, ROInstanceID: ROInstanceID);
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

        override public ROOut ProcessRequest(ROParms Parms)
        {
            switch (Parms.RORequest)
            {
                // Common 
                case eRORequest.GetViews:
                    return GetViews(parms: (RONoParms)Parms);

                // Selection
                case eRORequest.GetOTSPlanSelectionData:
                    return GetOTSPlanSelectionData();

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

                // Workflows
                case eRORequest.OTSForecastWorkflows:
                    return ROWorkflowMethodManager.GetWorkflowsData();
                case eRORequest.OTSForecastWorkFlowDetails:
                    return ROWorkflowMethodManager.GetWorkflowDetails(parms: (ROKeyParms)Parms);
                case eRORequest.SavePlanningWorkflow:
                    return ROWorkflowMethodManager.SaveWorkflow(rOWorkflow: (ROWorkflowPropertiesParms)Parms);
                case eRORequest.DeleteWorkflow:
                    return ROWorkflowMethodManager.DeleteWorkflow(parms: (ROKeyParms)Parms);
                case eRORequest.GetPlanningWorkFlowMethodList:
                    return ROWorkflowMethodManager.GetWorkFlowMethodList((ROKeyParms)Parms);
                case eRORequest.GetPlanningWorkFlowMethodActionsList:
                    return GetPlanningWorkFlowMethodActionsList();
                case eRORequest.GetPlanningWorkFlowVariablesList:
                    return GetPlanningWorkFlowVariablesList((ROMethodParms)Parms);
                case eRORequest.ProcessOTSForecastWorkflow:
                    return ProcessOTSForecastWorkflow(rOIntParms: (ROIntParms)Parms);
                case eRORequest.SaveAs:
                    return ROWorkflowMethodManager.SaveAs((RODataExplorerSaveAsParms)Parms);
            }
            
            return new RONoDataOut(eROReturnCode.Failure, "Invalid Request", ROInstanceID);
        }

        private VariableProfile GetDefaultVariableProfile()
        {
            try
            {
                return new VariableProfile(0, "(No Override)", eVariableCategory.None, eVariableType.None, null, eVariableDatabaseType.None, eVariableDatabaseType.None, eVariableStyle.None, eVariableAccess.None, eVariableScope.None, eVariableSpreadType.None, eVariableWeekType.None, eEligibilityType.None, eSimilarStoreDateType.None, eVariableTimeTotalType.None, eVariableForecastType.None, eValueFormatType.None, 0, false, false, false, eLevelRollType.None, eDayToWeekRollType.None, eStoreToChainRollType.None, 0, 0, 0, 0, 0, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, eVariableDatabaseModelType.None, null);
            }
            catch
            {
                throw;
            }
        }

    }
}
