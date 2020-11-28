using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// Container to hold assortment method descriptions within a workflow.
	/// </summary>
	/// <list type="bullet">
	/// <item>ComponentRID: The RID of the component associated with the method (when applicable).</item>
	/// <item>Review: Bool to indicate that processing should stop at the end of the processing for this method.</item>
	/// <item>TolerancePercent: Tolerance percentage for balancing purposes.</item>
	/// </list>
	[Serializable]
	public class AssortmentWorkFlowStep : ApplicationWorkFlowStep
	{
		//=======
		// FIELDS
		//=======
		private GeneralComponent _component;
		private int _componentCriteriaRID;
		private bool _review;
		private bool _usedSystemTolerancePercent;
		private double _tolerancePercent;
		private AssortmentCubeGroup _assrtCubeGroup;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aMethod">ApplicationBaseMethod that describes the desired action and action parameters.</param>
		/// <param name="aComponent">Assortment component to which this action applies.</param>
		/// <param name="aReviewFlag">Review Flag value: True indicates to stop processing after processing this action/method; false indicates to continue processing.</param>
		/// <param name="aUsedSystemTolerancePercent">Used System Tolerarnce Flag value: True indicates the value came from systems options; false indicates the value was defined to the step.</param>
		/// <param name="aTolerancePercent">Balance tolerance percent when a balance action is requested.</param>
		/// <param name="aStoreFilter">Store Filter RID associated with the method.</param>
		/// <param name="aWorkFlowStepKey">Workflow step</param>
		public AssortmentWorkFlowStep(
			ApplicationBaseAction aMethod,
			GeneralComponent aComponent,
			bool aReviewFlag,
			AssortmentCubeGroup aAssortmentCubeGroup,
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent,
			int aStoreFilter, int aWorkFlowStepKey)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_component = aComponent;
			_review = aReviewFlag;
			_componentCriteriaRID = Include.NoRID;
			_assrtCubeGroup = aAssortmentCubeGroup;
			if (aTolerancePercent < 0)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
			}
			_usedSystemTolerancePercent = aUsedSystemTolerancePercent;
			_tolerancePercent = aTolerancePercent;
		}

		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aMethod">ApplicationBaseMethod that describes the desired action and action parameters.</param>
		/// <param name="aComponentCriteriaRID">Record ID of the Allocation component to which this action applies.</param>
		/// <param name="aComponent">Assortment component to which this action applies.</param>
		/// <param name="aReviewFlag">Review Flag value: True indicates to stop processing after processing this action/method; false indicates to continue processing.</param>
		/// <param name="aUsedSystemTolerancePercent">Used System Tolerarnce Flag value: True indicates the value came from systems options; false indicates the value was defined to the step.</param>
		/// <param name="aTolerancePercent">Balance tolerance percent when a balance action is requested.</param>
		/// <param name="aStoreFilter">Store Filter RID associated with the method.</param>
		/// <param name="aWorkFlowStepKey">Workflow step</param>
		public AssortmentWorkFlowStep(
			ApplicationBaseAction aMethod,
			int aComponentCriteriaRID,
			GeneralComponent aComponent,
			bool aReviewFlag,
			AssortmentCubeGroup aAssortmentCubeGroup,
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent,
			int aStoreFilter, int aWorkFlowStepKey)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_component = aComponent;
			_review = aReviewFlag;
			_componentCriteriaRID = aComponentCriteriaRID;
			_assrtCubeGroup = aAssortmentCubeGroup;
			if (aTolerancePercent < 0)
			{
				throw new MIDException(eErrorLevel.severe,
					(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
			}
			_usedSystemTolerancePercent = aUsedSystemTolerancePercent;
			_tolerancePercent = aTolerancePercent;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets the ProfileType of the Profile.
		/// </summary>
		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.AssortmentWorkFlowStep;
			}
		}

		public AssortmentCubeGroup AssortmentCubeGroup
		{
			get
			{
				return _assrtCubeGroup;
			}
		}

		/// <summary>
		/// Verifies that the specified method is a valid method for this workflow.
		/// </summary>
		/// <param name="aMethod">Method to be checked</param>
		override public void CheckMethod(ApplicationBaseAction aMethod)
		{
			if (!Enum.IsDefined(typeof(eAssortmentMethodType), (eAssortmentMethodType)aMethod.MethodType))
			{
				if (!Enum.IsDefined(typeof(eAssortmentActionType), (eAssortmentActionType)aMethod.MethodType))
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
						MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
				}
			}
		}
	}

	/// <summary>
	/// Processes an Assortment work flow.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Inherits from the ApplicationBaseWorkFlow.
	/// </para><para>
	/// The Process method of this class executes the requested actions/methods within the workflow.
	/// </para>
	/// <para>
	/// The RestartAtLine is automatically set to the first action/method line within the workflow when the workflow is created.
	/// </para>
	/// </remarks>
	public class AssortmentWorkFlow : ApplicationBaseWorkFlow
	{
		//=======
		// FIELDS
		//=======
        //private AllocationWorkflowData			_workflowData;
        //private GetMethods						_getMethods;
        //private SessionAddressBlock SAB;
        //private AssortmentProfile				_assortmentProfile;
        //private ApplicationSessionTransaction	_applicationSessionTransaction;
        //private ApplicationBaseAction			_method;
        //private int								_restartAtLine;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aSAB">Session Address Block.</param>
		/// <param name="aWorkFlowRID">Workflow record id (one of three primary keys).</param>
		/// <param name="aUserRID">RID of user who created this workflow (one of three primary keys).</param>
		/// <param name="aGlobalFlagValue">True indicates this is a global workflow; false indicates it is a user specific workflow. (One of three primary keys).</param>
		public AssortmentWorkFlow(
			SessionAddressBlock aSAB,
			int aWorkFlowRID,
			int aUserRID,
			bool aGlobalFlagValue)
			: base(aSAB, eWorkflowType.Assortment, aWorkFlowRID, aUserRID, aGlobalFlagValue)
		{
			//_restartAtLine = 0;
			//if (base.Filled)
			//{
			//    if (base.WorkFlowType != eWorkflowType.Allocation)
			//    {
			//        throw new MIDException(eErrorLevel.severe,
			//            (int)(eMIDTextCode.msg_WorkflowTypeInvalid),
			//            MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
			//    }
			//    _workflowData = new AllocationWorkflowData(base.Key, eChangeType.populate);
			//    _getMethods = new GetMethods(aSAB);
			//    DataTable ws = _workflowData.GetWorkflowSteps();
			//    foreach (DataRow dr in ws.Rows)
			//    {
			//        int stepNumber = Convert.ToInt32(dr["STEP_NUMBER"], CultureInfo.CurrentUICulture);
			//        eMethodType methodType = (eMethodType)(Convert.ToInt32(dr["ACTION_METHOD_TYPE"], CultureInfo.CurrentUICulture));
			//        int methodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
			//        double pctTolerance = Convert.ToDouble(dr["PCT_TOLERANCE"], CultureInfo.CurrentUICulture);
			//        bool reviewInd = Include.ConvertCharToBool(Convert.ToChar(dr["REVIEW_IND"], CultureInfo.CurrentUICulture));
			//        int storeFilterRID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);

			//        bool usedSystemTolerancePercent = false;
			//        if (pctTolerance == Include.UseSystemTolerancePercent)
			//        {
			//            usedSystemTolerancePercent = true;
			//            pctTolerance = aSAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;
			//        }

			//        int componentCriteriaRID;
			//        GeneralComponentWrapper gcw = null;
			//        DataTable wsc = _workflowData.GetWorkflowStepComponents(base.Key, stepNumber);
			//        // for now, only one component is allowed in a workflow step.
			//        //					foreach (DataRow cdr in wsc.Rows)
			//        //					{
			//        if(wsc != null && wsc.Rows.Count == 1)
			//        {
			//            DataRow cdr = wsc.Rows[0];
			//            eComponentType componentType = (eComponentType)(Convert.ToInt32(cdr["COMP_TYPE_ID"], CultureInfo.CurrentUICulture));
			//            componentCriteriaRID = Convert.ToInt32(cdr["COMP_CRIT_RID"], CultureInfo.CurrentUICulture);
			//            int colorCodeRID = Convert.ToInt32(cdr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
			//            int sizeCodeRID = Convert.ToInt32(cdr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
			//            string packName = Convert.ToString(cdr["PACK_NAME"], CultureInfo.CurrentUICulture);

			//            gcw = new GeneralComponentWrapper(componentType, colorCodeRID, sizeCodeRID, packName);
			//        }
			//        else
			//        {
			//            throw new Exception("Should be one workflow step component");
			//        }

			//        ApplicationBaseAction method = null;
			//        if (methodRID != Include.NoRID)
			//        {
			//            // TODO add getmethod to transaction
			//            //						method = applicationTransaction.GetUnknownMethod((eMethodType) methodType);
			//            //						method = applicationTransaction.GetUnknownMethod(methodRID, false);
			//            method = _getMethods.GetMethod(methodRID, methodType);
			//        }
			//        AllocationWorkFlowStep allocationWorkFlowStep = new AllocationWorkFlowStep(method, 
			//            componentCriteriaRID, gcw.GeneralComponent, reviewInd,
			//            usedSystemTolerancePercent, pctTolerance, storeFilterRID, stepNumber);
			//        AddWorkFlowStep(allocationWorkFlowStep);

			//    }
				
			//}
			//else
			//{
				
			//}
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

		//========
		// METHODS
		//========

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        override internal bool CheckForUserData()
        {
            if (IsFilterUser(StoreFilterRID))
            {
                return true;
            }

            foreach (OTSPlanWorkFlowStep workFlowStep in Workflow_Steps.ArrayList)
            {
                if (Enum.IsDefined(typeof(eMethodTypeUI), (eMethodTypeUI)workFlowStep.Method.MethodType))
                {
                    if (((ApplicationBaseMethod)workFlowStep.Method).GlobalUserType == eGlobalUserType.User)
                    {
                        return true;
                    }

                    if (IsFilterUser(workFlowStep.StoreFilterRID))
                    {
                        return true;
                    }

                    if (((ApplicationBaseMethod)workFlowStep.Method).CheckForUserData())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)


		override public ApplicationBaseWorkFlow Copy(Session aSession, bool aCloneDateRanges)
		{
			return new AssortmentWorkFlow(SAB, Key, UserRID, false);
		}

		/// <summary>
		/// Adds a method and its parameters to the workflow.
		/// </summary>
		/// <param name="aWorkFlowStep">ApplicationWorkFlowStep that describes the method and its input parameters.</param>
		override public void AddWorkFlowStep(ApplicationWorkFlowStep aWorkFlowStep)
		{
			Workflow_Steps.Add(aWorkFlowStep);
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
			////			bool create = false;
			//if (_workflowData == null || base.Key < 0 || base.Workflow_Change_Type == eChangeType.add)
			//{
			//    _workflowData = new AllocationWorkflowData(td);
			//    //				create = true;
			//}
			//try
			//{
			//    int stepNumber = 0;
			//    switch (base.Workflow_Change_Type)
			//    {
			//        case eChangeType.add:
			//            base.Update(td);
			//            foreach (AllocationWorkFlowStep aws in Workflow_Steps)
			//            {
			//                _workflowData.Comp_Crit_RID = Include.NoRID;
			//                GeneralComponentWrapper gcw = new GeneralComponentWrapper(aws.Component);
			//                _workflowData.Component_Type = gcw.ComponentType;
			//                _workflowData.Color_Code_RID = gcw.ColorRID;
			//                _workflowData.Size_Code_RID = gcw.SizeRID;
			//                _workflowData.PackName = gcw.PackName;
			//                if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)gcw.ComponentType))
			//                {
			//                    _workflowData.InsertComponentCriteria(td);
			//                }
			//                _workflowData.Workflow_RID = base.Key;
			//                _workflowData.Step_Number = stepNumber;
			//                _workflowData.Method_RID = aws.Method.Key;
			//                _workflowData.Action_Method_Type = aws.Method.MethodType;
			//                _workflowData.Step_Store_Filter_RID = aws.StoreFilterRID;
			//                if (aws.UsedSystemTolerancePercent)
			//                {
			//                    _workflowData.Pct_Tolerance = Include.UseSystemTolerancePercent;
			//                }
			//                else
			//                {
			//                    _workflowData.Pct_Tolerance = aws.TolerancePercent;
			//                }
			//                _workflowData.InsertWorkflowStep(base.Key, td);
			//                _workflowData.InsertWorkflowStepComponent(base.Key, td);
			//                ++stepNumber;
			//            }
			//            break;
			//        case eChangeType.update:
			//            base.Update(td);
			//            _workflowData.DeleteAllWorkflowSteps(base.Key, td);
			//            foreach (AllocationWorkFlowStep aws in Workflow_Steps)
			//            {
			//                _workflowData.Comp_Crit_RID = Include.NoRID;
			//                GeneralComponentWrapper gcw = new GeneralComponentWrapper(aws.Component);
			//                _workflowData.Component_Type = gcw.ComponentType;
			//                _workflowData.Color_Code_RID = gcw.ColorRID;
			//                _workflowData.Size_Code_RID = gcw.SizeRID;
			//                _workflowData.PackName = gcw.PackName;
			//                if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)gcw.ComponentType))
			//                {
			//                    _workflowData.InsertComponentCriteria(td);
			//                }
							
			//                _workflowData.Workflow_RID = base.Key;
			//                _workflowData.Step_Number = stepNumber;
			//                _workflowData.Method_RID = aws.Method.Key;
			//                _workflowData.Action_Method_Type = aws.Method.MethodType;
			//                _workflowData.Step_Store_Filter_RID = aws.StoreFilterRID;
			//                if (aws.UsedSystemTolerancePercent)
			//                {
			//                    _workflowData.Pct_Tolerance = Include.UseSystemTolerancePercent;
			//                }
			//                else
			//                {
			//                    _workflowData.Pct_Tolerance = aws.TolerancePercent;
			//                }
			//                _workflowData.InsertWorkflowStep(base.Key, td);
			//                _workflowData.InsertWorkflowStepComponent(base.Key, td);
			//                ++stepNumber;
			//            }
			//            break;
			//        case eChangeType.delete:
			//            _workflowData.DeleteAllWorkflowSteps(base.Key, td);
			//            base.Update(td);
			//            break;
			//    }
			//}
			//catch (Exception e)
			//{
			//    string message = e.ToString();
			//    throw;
			//}
			//finally
			//{
			//    //TO DO:  whatever has to be done after an update or exception.
			//}
		}

		/// <summary>
		/// Processes the methods and actions contained in the workflow.
		/// </summary>
		/// <param name="aApplicationSessionTransaction">Transaction containing the list of AllocationProfiles on which this workflow will act.</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests and continue processing; false indicates to stop processing when a review request is encountered.</param>
		/// <param name="aWriteToDB">True indicates to write the allocation to the database on successful completion of each request, false incidates that no writes to the database are to occur.</param>
		/// <param name="aRestartAtLine">Workflow line where processing is to begin. Zero or 1 starts at the beginning.</param>
		/// <param name="aWorkflowProcessOrder">Indicates the orders which the headers and steps are to be executed</param>
		/// <returns>True if the process action is successful; false if the process action is unsuccessful.</returns> 
		override public bool Process (
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder)
		{
			return Process(
				aApplicationSessionTransaction,
				aIgnoreReview,
				aWriteToDB,
				aRestartAtLine,
				aWorkflowProcessOrder,
				(AllocationProfileList)aApplicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation));
		}

		override public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder,
			ProfileList aProfileList)
		{
//            this._applicationSessionTransaction = aApplicationSessionTransaction;
//            this.SAB = _applicationSessionTransaction.SAB;
			bool successFlag = true;
//            string wfn = MIDText.GetTextOnly((eMIDTextCode) this.WorkFlowType);
//            if (this.IsGlobal)
//            {
//                wfn += "/" + MIDText.GetTextOnly((eMIDTextCode) eGlobalUserType.Global);
//            }
//            else
//            {
//                wfn += "/" + MIDText.GetTextOnly((eMIDTextCode) eGlobalUserType.User);
//            }
//            if (UserRID == Include.GlobalUserRID)  // issue 3806
//            {
//                wfn += ":" + this.WorkFlowName;
//            }
//            else
//            {
//                wfn += "/" + this.SAB.ClientServerSession.GetUserName(UserRID) + ":" + this.WorkFlowName;  
//            }
//            SAB.ApplicationServerSession.Audit.Add_Msg(
//                eMIDMessageLevel.Information,
//                eMIDTextCode.msg_AllocationWorkFlowStart,
//                wfn, this.GetType().Name);
//            if (aWorkflowProcessOrder == eWorkflowProcessOrder.AllStepsForHeader)
//            {
//                foreach (AllocationProfile ap in aAllocationProfileList)
//                {
//// (CSMITH) - BEG MID Track #3156: Headers with status Rec'd OUT of Bal are being processed
//                    // BEGIN MID Track #4022 - add 'InUseByMultiHeader' to 'if...' statement
//                    if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance
//                     || ap.HeaderAllocationStatus == eHeaderAllocationStatus.InUseByMultiHeader)
//                    // END MID Track #4022
//                    {
//                        string msg = string.Format(
//                            SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false),
//                                                                        MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
//                        msg = msg.Replace(" action for headers", " workflow for header");
//                        SAB.ApplicationServerSession.Audit.Add_Msg(
//                            eMIDMessageLevel.Warning,
//                            ("Workflow: " + wfn + ", Header: " + ap.HeaderID + " - " + msg),
//                            this.GetType().Name);
//                        successFlag = false;
//                    }
//                    else
//                    {
//                        if (aRestartAtLine < 1)
//                        {
//                            _restartAtLine = 0;
//                        }
//                        else  
//                        {
//                            if (aRestartAtLine > Workflow_Steps.Count)
//                            {
//                                SAB.ApplicationServerSession.Audit.Add_Msg(
//                                    eMIDMessageLevel.Error,
//                                    eMIDTextCode.msg_RestartAtLineExceedsCount,
//                                    wfn + " " + (aRestartAtLine.ToString(CultureInfo.CurrentUICulture) + " > " + Workflow_Steps.Count.ToString(CultureInfo.CurrentUICulture)),
//                                    this.GetType().Name);
//                                successFlag = false;
//                            }
//                            else
//                            {
//                                _restartAtLine = aRestartAtLine - 1;
//                            }
//                        }
//                        SAB.ApplicationServerSession.Audit.Add_Msg(
//                            eMIDMessageLevel.Information,
//                            eMIDTextCode.msg_RestartWorkFlow,
//                            wfn + " " + _restartAtLine.ToString(CultureInfo.CurrentUICulture), this.GetType().Name);
//                        if (!ProcessHeader (wfn, ap, aIgnoreReview, aWriteToDB))
//                        {
//                            successFlag = false;
//                        }
//                    }
//// (CSMITH) - END MID Track #3156
//                }
//            }
//            else
//            {
//                bool stopForReview = false;
//                if (aRestartAtLine < 1)
//                {
//                    _restartAtLine = 0;
//                }
//                else  
//                {
//                    if (aRestartAtLine > Workflow_Steps.Count)
//                    {
//                        SAB.ApplicationServerSession.Audit.Add_Msg(
//                            eMIDMessageLevel.Error,
//                            eMIDTextCode.msg_RestartAtLineExceedsCount,
//                            wfn + " " + (aRestartAtLine.ToString(CultureInfo.CurrentUICulture) + " > " + Workflow_Steps.Count.ToString(CultureInfo.CurrentUICulture)),
//                            this.GetType().Name);
//                        successFlag = false;
//                    }
//                    else
//                    {
//                        _restartAtLine = aRestartAtLine - 1;
//                    }
//                }
//                if (successFlag)
//                {
//                    SAB.ApplicationServerSession.Audit.Add_Msg(
//                        eMIDMessageLevel.Information,
//                        eMIDTextCode.msg_RestartWorkFlow,
//                        wfn + " " + _restartAtLine.ToString(CultureInfo.CurrentUICulture), this.GetType().Name);
//                    while (stopForReview == false && _restartAtLine <= Workflow_Steps.Count)
//                    {
//                        AllocationWorkFlowStep workFlowStep = 
//                            (AllocationWorkFlowStep) Workflow_Steps[_restartAtLine];
//                        if (!ProcessStepForAllHeaders (wfn, workFlowStep, aWriteToDB, aAllocationProfileList))
//                        {
//                            successFlag = false;
//                            break;
//                        }
//                        else
//                        {
//                            if (aIgnoreReview == false & workFlowStep.Review)
//                            {
//                                stopForReview = true;
//                                SAB.ApplicationServerSession.Audit.Add_Msg(
//                                    eMIDMessageLevel.Information,
//                                    wfn + " " + MIDText.GetText(eMIDTextCode.msg_al_WokflowStoppedForReview),
//                                    this.GetType().Name);
//                            }
//                            _restartAtLine++;
//                        }
//                    }
//                }
//            }
//            SAB.ApplicationServerSession.Audit.Add_Msg(
//                eMIDMessageLevel.Information,
//                eMIDTextCode.msg_AllocationWorkFlowEnd,
//                wfn, this.GetType().Name);
			return successFlag;
		}

		/// <summary>
		/// Excutes the requested action for the Allocation Profile.
		/// </summary>
		/// <param name="aAllocationProfile">Allocation Profile on which the action is to occur.</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests within the workflow; false indicates to stop workflow processing when a review request is found.</param>
		/// <param name="aWriteToDB">True indicates to write the results of each action/method to the database; false incidates to delay writing results to the database until instructed to do so.</param>
		/// <returns>True when processing is successful; false when processing fails</returns>
		private bool ProcessHeader(
			string aWorkFlowName,
			AllocationProfile aAllocationProfile,
			bool aIgnoreReview,
			bool aWriteToDB)
		{
			//SAB.ApplicationServerSession.Audit.Add_Msg(
			//    eMIDMessageLevel.Information,
			//    eMIDTextCode.msg_AllocationHeaderStart,
			//    aWorkFlowName + " " + aAllocationProfile.HeaderID.ToString(), this.GetType().Name);
			//aAllocationProfile.ActiveWorkflowRID = this.Key;
			bool processSuccessFlag = true;
			//_assortmentProfile = aAllocationProfile;
			//bool stopForReview = false;
			//while (stopForReview == false && _restartAtLine < Workflow_Steps.Count)
			//{
			//    AllocationWorkFlowStep workFlowStep = 
			//        (AllocationWorkFlowStep) Workflow_Steps[_restartAtLine];
			//    if (DoStep(aWorkFlowName, workFlowStep, aWriteToDB))
			//    {
			//        if (aIgnoreReview == false & workFlowStep.Review)
			//        {
			//            stopForReview = true;
			//            SAB.ApplicationServerSession.Audit.Add_Msg(
			//                eMIDMessageLevel.Information,
			//                aWorkFlowName + " " + MIDText.GetText(eMIDTextCode.msg_al_WokflowStoppedForReview),
			//                this.GetType().Name);
			//        }
			//        _restartAtLine++;
			//    }
			//    else
			//    {
			//        processSuccessFlag = false;
			//        break;
			//    }
			//}
			//aAllocationProfile.ActiveWorkflowRID = Include.NoRID;
			//SAB.ApplicationServerSession.Audit.Add_Msg(
			//    eMIDMessageLevel.Information,
			//    eMIDTextCode.msg_AllocationHeaderEnd,
			//    aWorkFlowName + " " + aAllocationProfile.HeaderID.ToString(), this.GetType().Name);
			return processSuccessFlag;
		}

		/// <summary>
		/// Excutes the requested action for the Allocation Profile.
		/// </summary>
		/// <param name="aWorkFlowName">Name of the workflow</param>
		/// <param name="aWorkflowStep">Workflow step on which the action is to occur.</param>
		/// <param name="aWriteToDB">True indicates to write the results of each action/method to the database; false incidates to delay writing results to the database until instructed to do so.</param>
		/// <param name="aAllocationProfileList">List of Allocation Profiles to which the workflow applies</param>
		/// <returns>True when processing is successful; false when processing fails</returns>
		private bool ProcessStepForAllHeaders(
			string aWorkFlowName,
			AllocationWorkFlowStep aWorkflowStep,
			bool aWriteToDB,
			AllocationProfileList aAllocationProfileList)
		{
			bool processSuccessFlag = true;
//            foreach (AllocationProfile ap in aAllocationProfileList)
//            {
//                SAB.ApplicationServerSession.Audit.Add_Msg(
//                    eMIDMessageLevel.Information,
//                    eMIDTextCode.msg_AllocationHeaderStart,
//                    aWorkFlowName + " " + ap.HeaderID.ToString(), this.GetType().Name);
//                ap.ActiveWorkflowRID = this.Key;


//                _assortmentProfile = ap;

//// (CSMITH) - BEG MID Track #3156: Headers with status Rec'd OUT of Bal are being processed
//                if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
//                {
//                    string msg = string.Format(
//                        SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false),
//                                                                    MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
//                    msg = msg.Replace(" action for headers", " workflow for header");
//                    SAB.ApplicationServerSession.Audit.Add_Msg(
//                        eMIDMessageLevel.Warning,
//                        ("Workflow: " + aWorkFlowName + ", Header: " + ap.HeaderID + " - " + msg),
//                        this.GetType().Name);
//                    processSuccessFlag = false;
//                }
//                else
//                {
//                    if (!this.DoStep(aWorkFlowName, aWorkflowStep, aWriteToDB))
//                    {
//                        processSuccessFlag = false;
//                    }
//                    ap.ActiveWorkflowRID = Include.NoRID;
//                }
//// (CSMITH) - END MID Track #3156

//                SAB.ApplicationServerSession.Audit.Add_Msg(
//                    eMIDMessageLevel.Information,
//                    eMIDTextCode.msg_AllocationHeaderEnd,
//                    aWorkFlowName + " " + ap.HeaderID.ToString(), this.GetType().Name);
//            }
//            if (processSuccessFlag)
//            {
//                _restartAtLine++;
//            }
			return processSuccessFlag;
		}
		private bool DoStep(string aWorkFlowName, AllocationWorkFlowStep aWorkFlowStep, bool aWriteToDB)
		{
			bool successFlag = true;

			//_method = aWorkFlowStep._method;
			//try
			//{
			//    _method.ProcessAction(
			//        SAB,
			//        _applicationSessionTransaction,
			//        aWorkFlowStep,
			//        _assortmentProfile,
			//        aWriteToDB,
			//        _workflowData.Store_Filter_RID);
			//    // BEGIN MID Track 3216 Workflow not stopping when Intransit Update fails
			//    if (_applicationSessionTransaction.GetAllocationActionStatus(_assortmentProfile.Key) == eAllocationActionStatus.ActionFailed)
			//    {
			//        successFlag = false;
			//    }
			//    // END MID Track 3216 Workflow not stopping when Intransit Update fails
			//}
			//catch (Exception err)
			//{
			//    SAB.ApplicationServerSession.Audit.Log_Exception(err,this.GetType().Name,eExceptionLogging.logAllInnerExceptions);
			//    SAB.ApplicationServerSession.Audit.Add_Msg(
			//        eMIDMessageLevel.Information,
			//        aWorkFlowName + " " + MIDText.GetText(eMIDTextCode.msg_AllocationWorkFlowError),
			//        this.GetType().Name);
			//    if (err.GetType() == typeof(MIDException))
			//    {
			//        MIDException Mex = (MIDException) err;
			//        if (Mex.ErrorLevel != eErrorLevel.information
			//            & Mex.ErrorLevel != eErrorLevel.warning)
			//        {
			//            successFlag = false;
			//        }
			//    }
			//    else
			//    {
			//        successFlag = false;
			//    }
			//}
			return successFlag;
		}

        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationWorkflowsGlobal);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationWorkflowsUser);
            }

        }
    }
}