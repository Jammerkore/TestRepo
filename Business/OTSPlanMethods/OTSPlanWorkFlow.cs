using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// Summary description for OTSPlanWorkFlow.
	/// </summary>
	public class OTSPlanWorkFlow : ApplicationBaseWorkFlow
	{
		//=======
		// FIELDS
		//=======
		private OTSPlanWorkflowData				_workflowData;
		private GetMethods						_getMethods;
        //private SessionAddressBlock				SAB;
		private ApplicationSessionTransaction	_applicationSessionTransaction;
		private ApplicationBaseAction			_method;
		private int								_restartAtLine;

		//=============
		// CONSTRUCTORS
		//=============
		public OTSPlanWorkFlow(
			SessionAddressBlock aSAB,
			int aWorkFlowRID,
			int aUserRID,
			bool aGlobalFlagValue)
			: base(aSAB, eWorkflowType.Forecast, aWorkFlowRID, aUserRID, aGlobalFlagValue)
		{
			_restartAtLine = 0;
			if (base.Filled)
			{
				if (base.WorkFlowType != eWorkflowType.Forecast)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_WorkflowTypeInvalid),
						MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
				}
				_workflowData = new OTSPlanWorkflowData(base.Key, eChangeType.populate);
				_getMethods = new GetMethods(aSAB);
				DataTable ws = _workflowData.GetWorkflowSteps();
				foreach (DataRow dr in ws.Rows)
				{
					int stepNumber = Convert.ToInt32(dr["STEP_NUMBER"], CultureInfo.CurrentUICulture);
					eMethodType methodType = (eMethodType)(Convert.ToInt32(dr["ACTION_METHOD_TYPE"], CultureInfo.CurrentUICulture));
					int methodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
					double pctTolerance = Convert.ToDouble(dr["PCT_TOLERANCE"], CultureInfo.CurrentUICulture);
					bool reviewInd = Include.ConvertCharToBool(Convert.ToChar(dr["REVIEW_IND"], CultureInfo.CurrentUICulture));
					int storeFilterRID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);
					int variableNumber = -1;
					if (dr["VARIABLE_NUMBER"] != System.DBNull.Value)
					{
						variableNumber = Convert.ToInt32(dr["VARIABLE_NUMBER"], CultureInfo.CurrentUICulture);
					}
					string computationMode = aSAB.ApplicationServerSession.ComputationsCollection.GetDefaultComputations().Name;
					if (dr["CALC_MODE"] != System.DBNull.Value)
					{
						computationMode = Convert.ToString(dr["CALC_MODE"], CultureInfo.CurrentUICulture);
					}
					bool balInd = false;
					if (dr["BAL_IND"] != System.DBNull.Value)
					{
						balInd = Include.ConvertCharToBool(Convert.ToChar(dr["BAL_IND"], CultureInfo.CurrentUICulture));
					}

					bool usedSystemTolerancePercent = false;
					if (pctTolerance == Include.UseSystemTolerancePercent)
					{
						usedSystemTolerancePercent = true;
						pctTolerance = aSAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;
					}

//					int componentCriteriaRID;
//					GeneralComponentWrapper gcw = null;
//					DataTable wsc = _workflowData.GetWorkflowStepComponents(base.Key, stepNumber);
					// for now, only one component is allowed in a workflow step.
					//					foreach (DataRow cdr in wsc.Rows)
					//					{
//					if(wsc != null && wsc.Rows.Count == 1)
//					{
//						DataRow cdr = wsc.Rows[0];
//						eComponentType componentType = (eComponentType)(Convert.ToInt32(cdr["COMP_TYPE_ID"], CultureInfo.CurrentUICulture));
//						componentCriteriaRID = Convert.ToInt32(cdr["COMP_CRIT_RID"], CultureInfo.CurrentUICulture);
//						int colorCodeRID = Convert.ToInt32(cdr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
//						int sizeCodeRID = Convert.ToInt32(cdr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
//						string packName = Convert.ToString(cdr["PACK_NAME"], CultureInfo.CurrentUICulture);
//
//						gcw = new GeneralComponentWrapper(componentType, colorCodeRID, sizeCodeRID, packName);
//					}
//					else
//					{
//						throw new Exception("Should be one workflow step component");
//					}

					ApplicationBaseAction method = null;
					if (methodRID != Include.NoRID)
					{
						// TODO add getmethod to transaction
						//						method = applicationTransaction.GetUnknownMethod((eMethodType) methodType);
						//						method = applicationTransaction.GetUnknownMethod(methodRID, false);
						method = _getMethods.GetMethod(methodRID, methodType);
					}
					OTSPlanWorkFlowStep OTSPlanWorkFlowStep = new OTSPlanWorkFlowStep(method, 
						reviewInd, usedSystemTolerancePercent, pctTolerance, storeFilterRID, stepNumber,
						variableNumber, computationMode, balInd);
					AddWorkFlowStep(OTSPlanWorkFlowStep);

				}
				
			}
			else
			{
				
			}
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Get Profile Type of this profile.
		/// </summary>
		public override eProfileType ProfileType
		{
			get
			{
				return eProfileType.Workflow;
			}
		}
//		/// <summary>
//		/// Gets the workflow type.
//		/// </summary>
//		override public eWorkflowType WorkFlowType
//		{
//			get
//			{
//				return eWorkflowType.Forecast;
//			}
//		}

		//========
		// METHODS
		//========
		/// <summary>
		/// Adds a method and its parameters to the workflow.
		/// </summary>
		/// <param name="aWorkFlowStep">ApplicationWorkFlowStep that describes the method and its input parameters.</param>
		override public void AddWorkFlowStep(ApplicationWorkFlowStep aWorkFlowStep)
		{
			try
			{
				Workflow_Steps.Add(aWorkFlowStep);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Loads workflow from data base.
		/// </summary>
		override public void LoadFromDB ()
		{
			// To do.
			throw new Exception("Not Implemented");
		}

		/// <summary>
		/// Updates workflow on data base.
		/// </summary>
		override public void UpdateDB()
		{
			// To do.
			throw new Exception("Not Implemented");

		}

		/// <summary>
		/// Updates the workflow history of a specified header.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header to update.</param>
		override public void UpdateHeaderHistory (int aHeaderRID)
		{
			// To do.
			throw new Exception("Not Implemented");
		}

		override public void Update(TransactionData td)
		{
			try
			{
				if (_workflowData == null || base.Key < 0 || base.Workflow_Change_Type == eChangeType.add)
				{
					_workflowData = new OTSPlanWorkflowData(td);
				}
			
				int stepNumber = 0;
				switch (base.Workflow_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						foreach (OTSPlanWorkFlowStep aws in Workflow_Steps)
						{
							double tolerance;
							if (aws.UsedSystemTolerancePercent)
							{
								tolerance = Include.UseSystemTolerancePercent;
							}
							else
							{
								tolerance = aws.TolerancePercent;
							}
							_workflowData.InsertWorkflowStep(base.Key, td, stepNumber, aws.Method.MethodType,
								aws.Method.Key, false, tolerance, aws.StoreFilterRID, aws.VariableNumber,
								aws.ComputationMode, aws.Balance);
							++stepNumber;
						}
						break;
					case eChangeType.update:
						base.Update(td);
						_workflowData.DeleteAllWorkflowSteps(base.Key, td);
						foreach (OTSPlanWorkFlowStep aws in Workflow_Steps)
						{
							double tolerance;
							if (aws.UsedSystemTolerancePercent)
							{
								tolerance = Include.UseSystemTolerancePercent;
							}
							else
							{
								tolerance = aws.TolerancePercent;
							}
							_workflowData.InsertWorkflowStep(base.Key, td, stepNumber, aws.Method.MethodType,
								aws.Method.Key, false, tolerance, aws.StoreFilterRID, aws.VariableNumber,
								aws.ComputationMode, aws.Balance);
							++stepNumber;
						}
						break;
					case eChangeType.delete:
						_workflowData.DeleteAllWorkflowSteps(base.Key, td);
						base.Update(td);
						break;
				}
			}
			catch (Exception e)
			{
				string message = e.ToString();
				throw;
			}
			finally
			{
				//TO DO:  whatever has to be done after an update or exception.
			}
		}

		override public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder,
			ProfileList aProfileList)
		{
			return false;
		}

		/// <summary>
		/// Processes the methods and actions contained in the workflow.
		/// </summary>
		/// <param name="aApplicationSessionTransaction">Transaction to which this workflow belongs.</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests and continue processing; false indicates to stop processing when a review request is encountered.</param>
		/// <param name="aWriteToDB">True indicates to write the OTSPlan to the database on successful completion of each request, false incidates that no writes to the database are to occur.</param>
		/// <param name="aRestartAtLine">Workflow line where processing is to begin. Zero or 1 starts at the beginning.</param>
		/// <returns>True if the process action is successful; false if the process action is unsuccessful.</returns> 
		override public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder)
		{
			try
			{
				_applicationSessionTransaction = aApplicationSessionTransaction;
                //SAB = aApplicationSessionTransaction.SAB;
				bool successFlag = true;
				string wfn = MIDText.GetTextOnly((eMIDTextCode) this.WorkFlowType);
				if (this.IsGlobal)
				{
					wfn += "/" + MIDText.GetTextOnly((eMIDTextCode) eGlobalUserType.Global);
				}
				else
				{
					wfn += "/" + MIDText.GetTextOnly((eMIDTextCode) eGlobalUserType.User);
				}
				if (UserRID == Include.GlobalUserRID)  // Issue 3806
				{
					wfn += ":" + this.WorkFlowName;
				}
				else
				{
					wfn += "/" + this.SAB.ClientServerSession.GetUserName(UserRID) + ":" + this.WorkFlowName;  
				}
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_OTSPlanWorkFlowStart,
					wfn, this.GetType().Name);

				//			foreach (OTSPlanProfile planProfile in aProfileList)
				//			{
				if (!ProcessNode (wfn, aIgnoreReview, aWriteToDB))
				{
					successFlag = false;
					_applicationSessionTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
				}
				else
				{
					_applicationSessionTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionCompletedSuccessfully;
				}
				//			}
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_OTSPlanWorkFlowEnd,
					wfn, this.GetType().Name);
				return successFlag;
			}
			catch
			{
				_applicationSessionTransaction.OTSPlanActionStatus = eOTSPlanActionStatus.ActionFailed;
				throw;
			}
		}

		/// <summary>
		/// Excutes the requested action for the Allocation Profile.
		/// </summary>
		/// <param name="aWorkFlowName">The name of the workflow</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests within the workflow; false indicates to stop workflow processing when a review request is found.</param>
		/// <param name="aWriteToDB">True indicates to write the results of each action/method to the database; false incidates to delay writing results to the database until instructed to do so.</param>
		/// <returns>True when processing is successful; false when processing fails</returns>
		private bool ProcessNode(string aWorkFlowName, bool aIgnoreReview,
			bool aWriteToDB)
		{
			bool processSuccessFlag = true;
			try
			{
				bool stopForReview = false;
				while (stopForReview == false && _restartAtLine < Workflow_Steps.Count)
				{
					OTSPlanWorkFlowStep workFlowStep = 
						(OTSPlanWorkFlowStep) Workflow_Steps[_restartAtLine];
					// Begin MID Track #5210 - JSmith - Out of memory
					// End MID Track #5210
					if (DoStep(aWorkFlowName, workFlowStep, aWriteToDB))
					{
						if (aIgnoreReview == false & workFlowStep.Review)
						{
							stopForReview = true;
							SAB.ApplicationServerSession.Audit.Add_Msg(
								eMIDMessageLevel.Information,
								aWorkFlowName + " " + MIDText.GetText(eMIDTextCode.msg_al_WokflowStoppedForReview),
								this.GetType().Name);
						}
						_restartAtLine++;
					}
					else
					{
						processSuccessFlag = false;
						break;
					}
					// Begin MID Track #5210 - JSmith - Out of memory
					System.GC.Collect();
					// End MID Track #5210
				}
			}
			catch (Exception err)
			{
				try
				{
					MIDException Merr = new MIDException(eErrorLevel.information,
						(int)eMIDTextCode.msg_OTSPlanWorkFlowError,
						MIDText.GetText(eMIDTextCode.msg_OTSPlanWorkFlowError),
						err);
					throw Merr;
				}
				catch (MIDException ex)
				{
					string message = ex.ToString();
					SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, MIDText.GetText(eMIDTextCode.msg_AllocationWorkFlowError), this.ToString());

					if (err.GetType() == typeof(MIDException))
					{
						MIDException Mex = (MIDException) err;
						if (Mex.ErrorLevel != eErrorLevel.information
							& Mex.ErrorLevel != eErrorLevel.warning)
						{
							processSuccessFlag = false;
						}
					}
				}
			}
			finally
			{
			}
			return processSuccessFlag;
		}

		private bool DoStep(string aWorkFlowName, OTSPlanWorkFlowStep aWorkFlowStep, bool aWriteToDB)
		{
			bool successFlag = true;

			_method = aWorkFlowStep.Method;
			try
			{
				//================================================================================
				// this adds the workflow step's override values to the forecasting override list.
				// the override list is used during forecasting to override the node and version
				// specified on the method. (and someday, variable, comp, and balance)
				//================================================================================
				// Begin Issue 4010 - stodd
				 if (Enum.IsDefined(typeof(eForecastMethodType),(eForecastMethodType)_method.MethodType))
				{
					BuildOTSPlanForecastOverride(aWorkFlowStep);
				}
				// End Issue 4010 - stodd

				_method.ProcessMethod(
					_applicationSessionTransaction,
					_workflowData.Store_Filter_RID,
					_method);

				if (_applicationSessionTransaction.OTSPlanActionStatus == eOTSPlanActionStatus.ActionFailed)
				{
					successFlag = false;
				}
			}
			catch (Exception err)
			{
				SAB.ApplicationServerSession.Audit.Log_Exception(err,this.GetType().Name,eExceptionLogging.logAllInnerExceptions);
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					aWorkFlowName + " " + MIDText.GetText(eMIDTextCode.msg_AllocationWorkFlowError),
					this.GetType().Name);
				if (err.GetType() == typeof(MIDException))
				{
					MIDException Mex = (MIDException) err;
					if (Mex.ErrorLevel != eErrorLevel.information
						& Mex.ErrorLevel != eErrorLevel.warning)
					{
						successFlag = false;
					}
				}
				else
				{
					successFlag = false;
				}
			}
			return successFlag;
		}

		private void BuildOTSPlanForecastOverride(OTSPlanWorkFlowStep aWorkFlowStep)
		{
			try
			{
				// Begin Issue 4010 - stodd
				bool overrideBalance = false;
				int hierNodeRid = Include.NoRID;
				int versionRid = Include.NoRID;

				//OTSPlanMethod method = (OTSPlanMethod)aWorkFlowStep.Method;
				if (aWorkFlowStep.Balance)
				{
					overrideBalance = true;
				}

				ArrayList forecastingOverrideList = _applicationSessionTransaction.ForecastingOverrideList;
				if (forecastingOverrideList != null)
				{
					if (forecastingOverrideList.Count > 0)
					{
						ForecastingOverride fo = (ForecastingOverride)forecastingOverrideList[0];
						hierNodeRid = fo.HierarchyNodeRid;
						versionRid = fo.ForecastVersionRid;
						_applicationSessionTransaction.ForecastingOverride_ClearAll();
					}
				}

				_applicationSessionTransaction.ForecastingOverride_Add(hierNodeRid, versionRid, aWorkFlowStep.ComputationMode, 
					aWorkFlowStep.VariableNumber, overrideBalance, aWorkFlowStep.Balance);
				// Begin Issue 4010
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
		override public ApplicationBaseWorkFlow Copy(Session aSession, bool aCloneDateRanges)
		{
			OTSPlanWorkFlow newOTSPlanWorkFlow = null;

			try
			{
				newOTSPlanWorkFlow = (OTSPlanWorkFlow)this.MemberwiseClone();
				newOTSPlanWorkFlow.IsGlobal = IsGlobal;
				newOTSPlanWorkFlow.StoreFilterRID = StoreFilterRID;
				newOTSPlanWorkFlow.UserRID = UserRID;
				newOTSPlanWorkFlow.Workflow_Change_Type = eChangeType.none;
				newOTSPlanWorkFlow.WorkFlowDescription = WorkFlowDescription;
				newOTSPlanWorkFlow.WorkFlowName = WorkFlowName;
				newOTSPlanWorkFlow.WorkFlowType = WorkFlowType;

				newOTSPlanWorkFlow.Workflow_Steps = new ProfileList(eProfileType.OTSPlanWorkFlowStep);
				foreach (OTSPlanWorkFlowStep OTSPlanWorkFlowStep in Workflow_Steps)
				{
					newOTSPlanWorkFlow.Workflow_Steps.Add(OTSPlanWorkFlowStep.Copy());
				}

				return newOTSPlanWorkFlow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
