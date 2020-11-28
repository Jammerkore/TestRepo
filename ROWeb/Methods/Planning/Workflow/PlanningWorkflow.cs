using Logility.ROWebSharedTypes;
using Logility.ROWebCommon;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Logility.ROWeb
{
    public partial class ROPlanning : ROWebFunction
    {
        //=================
        // PRIVATE METHODS
        //=================
        private string _workflowActionStatus;
        private WorkflowBaseData _workflowData = new WorkflowBaseData();
        private OTSPlanWorkFlow _planningWorkflow;

        #region "Process OTS Forecast Workflow #RO-2925"

        private ROOut ProcessOTSForecastWorkflow(ROIntParms rOIntParms)
        {

            int aWorkflowRID = 0;
            _applicationSessionTransaction = GetApplicationSessionTransaction();
            aWorkflowRID = rOIntParms.ROInt;
            var processedOTSForecastWorkflow = ProcessWorkflow(aWorkflowRID);

            if (processedOTSForecastWorkflow)
            {
                return new ROBoolOut(eROReturnCode.Successful, _workflowActionStatus, ROInstanceID, processedOTSForecastWorkflow);
            }

            return new ROBoolOut(eROReturnCode.Failure, _workflowActionStatus, ROInstanceID, processedOTSForecastWorkflow);
        }

        internal bool ProcessWorkflow(int aWorkflowRID)
        {
            try
            {
                _planningWorkflow = new OTSPlanWorkFlow(SAB, aWorkflowRID, SAB.ClientServerSession.UserRID, false);

                return ProcessWorkflow();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        internal bool ProcessWorkflow()
        {
            bool processedOTSForecastWorkflow = false;

            try
            {
                SetObject();

                _applicationSessionTransaction.ProcessOTSPlanWorkflow(ABW.Key, true, true, 1);
                eOTSPlanActionStatus actionStatus = _applicationSessionTransaction.OTSPlanActionStatus;
                _workflowActionStatus = actionStatus.ToString();
                string message = MIDText.GetTextOnly((int)actionStatus);

                if (actionStatus == eOTSPlanActionStatus.ActionCompletedSuccessfully)
                {
                    processedOTSForecastWorkflow = true;
                }
                else if (actionStatus == eOTSPlanActionStatus.ActionFailed)
                {
                    processedOTSForecastWorkflow = false;
                }
                else
                {
                    MIDEnvironment.Message = MIDText.GetTextFromCode(Convert.ToInt32(eOTSPlanActionStatus.NoActionPerformed));
                }
            }
            catch (Exception ex)
            {
                processedOTSForecastWorkflow = false;
            }
            finally
            {

            }

            return processedOTSForecastWorkflow;

        }

        internal void SetObject()
        {
            try
            {
                ABW = _planningWorkflow;
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Method to get the Planning WorkFlows Methods / Actions List
        private ROOut GetPlanningWorkFlowMethodActionsList()
        {
            try
            {
                List<KeyValuePair<int, string>> results = GetPlanningWorkFlowMethodActions();
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, results);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetPlanningWorkFlowMethodActions()
        {

            List<KeyValuePair<int, string>> planningMethodActionsList = new List<KeyValuePair<int, string>>();
            DataTable _dtMethods = MIDText.GetLabels((int)eMethodType.OTSPlan, (int)eMethodType.PlanningExtract);

            DataView dv = new DataView(_dtMethods);

            dv.Sort = "TEXT_VALUE";

            foreach (DataRowView dr in dv)
            {
                bool addAction = false;
                // exclude non OTS Plan methods
                eForecastMethodType methodType = (eForecastMethodType)(Convert.ToInt32(dr["TEXT_CODE"], CultureInfo.CurrentUICulture));
                if (Enum.IsDefined(typeof(eForecastMethodType), methodType))
                {
                    addAction = true;
                }

                if (methodType == eForecastMethodType.PlanningExtract
                    && !SAB.ROExtractEnabled)
                {
                    addAction = false;
                }

                if (addAction)
                {
                    int actionID = Convert.ToInt32(dr["TEXT_CODE"]);
                    string actionName = Convert.ToString(dr["TEXT_VALUE"]);
                    planningMethodActionsList.Add(new KeyValuePair<int, string>(actionID, actionName));
                }
            }
            return planningMethodActionsList;

        }

        #endregion

        #region Method to get the Planning WorkFlows Variables List
        private ROOut GetPlanningWorkFlowVariablesList(ROMethodParms rOMethodParms)
        {
            try
            {
                List<KeyValuePair<int, string>> results = GetPlanningWorkFlowVariables(rOMethodParms);
                return new ROIntStringPairListOut(eROReturnCode.Successful, null, ROInstanceID, results);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        internal List<KeyValuePair<int, string>> GetPlanningWorkFlowVariables(ROMethodParms rOMethodParms)
        {

            List<KeyValuePair<int, string>> planningVariablesList = new List<KeyValuePair<int, string>>();

            VariableProfile vp = GetDefaultVariableProfile();

            planningVariablesList.Add(new KeyValuePair<int, string>(vp.Key, vp.VariableName));

            if (rOMethodParms.MethodType == eMethodType.ForecastBalance)
            {
                foreach (VariableProfile varProf in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                {
                    if (varProf.AllowForecastBalance)
                    {
                        planningVariablesList.Add(new KeyValuePair<int, string>(varProf.Key, varProf.VariableName));
                    }
                }
            }
            else if (rOMethodParms.MethodType == eMethodType.OTSPlan)
            {
                if (rOMethodParms.Key == Include.Undefined)
                {
                    foreach (VariableProfile varProf in SAB.ApplicationServerSession.DefaultPlanComputations.PlanVariables.VariableProfileList)
                    {
                        if (varProf.AllowOTSForecast)
                        {
                            planningVariablesList.Add(new KeyValuePair<int, string>(varProf.Key, varProf.VariableName));
                        }
                    }
                }
                else
                {
                    OTSPlanMethod forecastMethod = new OTSPlanMethod(SAB, rOMethodParms.Key);

                    if (forecastMethod.ForecastingModel == null)
                    {
                        forecastMethod.SetDefaultSalesStockVariables(GetApplicationSessionTransaction());
                        if (forecastMethod.SalesVariable.Key != Include.NoRID)
                        {
                            planningVariablesList.Add(new KeyValuePair<int, string>(forecastMethod.SalesVariable.VariableProfile.Key, forecastMethod.SalesVariable.VariableProfile.VariableName));
                        }
                        if (forecastMethod.StockVariable.Key != Include.NoRID)
                        {
                            planningVariablesList.Add(new KeyValuePair<int, string>(forecastMethod.StockVariable.VariableProfile.Key, forecastMethod.StockVariable.VariableProfile.VariableName));
                        }
                    }
                    else
                    {
                        foreach (ModelVariableProfile mvp in forecastMethod.ForecastingModel.Variables)
                        {

                            planningVariablesList.Add(new KeyValuePair<int, string>(mvp.VariableProfile.Key, mvp.VariableProfile.VariableName));
                        }
                    }
                }
                
            }

            return planningVariablesList;

        }

        #endregion
    }
}
