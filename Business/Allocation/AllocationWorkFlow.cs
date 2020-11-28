using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business.Allocation
{
	/// <summary>
	/// Container to hold allocation method descriptions within a workflow.
	/// </summary>
	/// <remarks>
	/// Inherits from the Application Workflow Method Bin.  Additional fields for allocation:
	/// <list type="bullet">
	/// <item>ComponentRID: The RID of the component associated with the method (when applicable).</item>
	/// <item>Review: Bool to indicate that processing should stop at the end of the processing for this method.</item>
	/// <item>TolerancePercent: Tolerance percentage for balancing purposes.</item>
	/// </list>
	/// </remarks>
	[Serializable]
	public class AllocationWorkFlowStep:ApplicationWorkFlowStep
	{
		//=======
		// FIELDS
		//=======
		private	GeneralComponent		_component;
		private	int						_componentCriteriaRID;
		private bool					_review;
		private bool					_usedSystemTolerancePercent;
		private double					_tolerancePercent;
		private int						_methodUserRid;
		
		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aMethod">ApplicationBaseMethod that describes the desired action and action parameters.</param>
		/// <param name="aComponent">Allocation component to which this action applies.</param>
		/// <param name="aReviewFlag">Review Flag value: True indicates to stop processing after processing this action/method; false indicates to continue processing.</param>
		/// <param name="aUsedSystemTolerancePercent">Used System Tolerarnce Flag value: True indicates the value came from systems options; false indicates the value was defined to the step.</param>
		/// <param name="aTolerancePercent">Balance tolerance percent when a balance action is requested.</param>
		/// <param name="aStoreFilter">Store Filter RID associated with the method.</param>
		/// <param name="aWorkFlowStepKey">Workflow step</param>
		public AllocationWorkFlowStep(
			ApplicationBaseAction aMethod,
			GeneralComponent aComponent,
			bool aReviewFlag, 
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent,
			int aStoreFilter, int aWorkFlowStepKey)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_component = aComponent;
			_review = aReviewFlag;
			_componentCriteriaRID = Include.NoRID;
			if (aTolerancePercent < 0)
			{
				throw new MIDException (eErrorLevel.severe,
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
		/// <param name="aComponent">Allocation component to which this action applies.</param>
		/// <param name="aReviewFlag">Review Flag value: True indicates to stop processing after processing this action/method; false indicates to continue processing.</param>
		/// <param name="aUsedSystemTolerancePercent">Used System Tolerarnce Flag value: True indicates the value came from systems options; false indicates the value was defined to the step.</param>
		/// <param name="aTolerancePercent">Balance tolerance percent when a balance action is requested.</param>
		/// <param name="aStoreFilter">Store Filter RID associated with the method.</param>
		/// <param name="aWorkFlowStepKey">Workflow step</param>
		public AllocationWorkFlowStep(
			ApplicationBaseAction aMethod,
			int aComponentCriteriaRID,
			GeneralComponent aComponent,
			bool aReviewFlag, 
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent,
			int aStoreFilter, int aWorkFlowStepKey)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_component = aComponent;
			_review = aReviewFlag;
			_componentCriteriaRID = aComponentCriteriaRID;
			if (aTolerancePercent < 0)
			{
				throw new MIDException (eErrorLevel.severe,
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
		/// <param name="aComponent">Allocation component to which this action applies.</param>
		/// <param name="aReviewFlag">Review Flag value: True indicates to stop processing after processing this action/method; false indicates to continue processing.</param>
		/// <param name="aUsedSystemTolerancePercent">Used System Tolerarnce Flag value: True indicates the value came from systems options; false indicates the value was defined to the step.</param>
		/// <param name="aTolerancePercent">Balance tolerance percent when a balance action is requested.</param>
		/// <param name="aStoreFilter">Store Filter RID associated with the method.</param>
		/// <param name="aWorkFlowStepKey">Workflow step</param>
		public AllocationWorkFlowStep(
			ApplicationBaseAction aMethod,
			int aComponentCriteriaRID,
			GeneralComponent aComponent,
			bool aReviewFlag, 
			bool aUsedSystemTolerancePercent,
			double aTolerancePercent,
			int aStoreFilter, int aWorkFlowStepKey,
			int methodUserRid)
			: base(aMethod, aStoreFilter, aWorkFlowStepKey)
		{
			_component = aComponent;
			_review = aReviewFlag;
			_componentCriteriaRID = aComponentCriteriaRID;
			if (aTolerancePercent < 0)
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
					MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
			}
			_usedSystemTolerancePercent = aUsedSystemTolerancePercent;
			_tolerancePercent = aTolerancePercent;
			_methodUserRid = methodUserRid;
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
				return eProfileType.AllocationWorkFlowStep;
			}
		}
		/// <summary>
		/// Gets or sets the component RID associated with this method or action.
		/// </summary>
		public GeneralComponent Component
		{
			get
			{
				return _component;
			}
			set
			{
				_component = value;
			}
		}

		/// <summary>
		/// Gets or sets record ID of the Allocation component to which this action applies.
		/// </summary>
		public int ComponentCriteriaRID
		{
			get
			{
				return _componentCriteriaRID;
			}
			set
			{
				_componentCriteriaRID = value;
			}
		}

		/// <summary>
		/// Gets or sets the Review Flag value.
		/// </summary>
		/// <remarks>
		/// True indicates to stop processing when processing of this method is complete.
		/// False indicates to continue processing when processing of this method is complete
		/// </remarks>
		public bool Review
		{
			get 
			{
				return _review;
			}
			set
			{
				_review = value;
			}
		}

		/// <summary>
		/// Gets or sets the Used System Tolerance Percent Flag value.
		/// </summary>
		/// <remarks>
		/// True indicates the value came from systems options.
		/// False indicates the value was defined to the step.
		/// </remarks>
		public bool UsedSystemTolerancePercent
		{
			get 
			{
				return _usedSystemTolerancePercent;
			}
			set
			{
				_usedSystemTolerancePercent = value;
			}
		}

		public int MethodUserRid
		{
			get 
			{
				return _methodUserRid;
			}
			set
			{
				_methodUserRid = value;
			}
		}

		/// <summary>
		/// Gets or sets Tolerance Percent for balancing purposes.
		/// </summary>
		public double TolerancePercent
		{
			get
			{
				return _tolerancePercent;
			}
			set
			{
				if (value < 0)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_TolerancePctCannotBeNeg,
						MIDText.GetText(eMIDTextCode.msg_TolerancePctCannotBeNeg));
				}
				_tolerancePercent = value;
			}
		}


		/// <summary>
		/// Verifies that the specified method is a valid method for this workflow.
		/// </summary>
		/// <param name="aMethod">Method to be checked</param>
		override public void CheckMethod(ApplicationBaseAction aMethod)
		{
			if (!Enum.IsDefined(typeof(eAllocationMethodType), (eAllocationMethodType)aMethod.MethodType))
			{
				throw new MIDException (eErrorLevel.severe,
					(int)eMIDTextCode.msg_UnknownMethodInWorkFlow,
					MIDText.GetText(eMIDTextCode.msg_UnknownMethodInWorkFlow));
			}
		}

		public AllocationWorkFlowStep Copy()
		{
			try
			{
				AllocationWorkFlowStep aws = (AllocationWorkFlowStep)this.MemberwiseClone();
				aws.Key = Key;
				aws.Method = Method;
				aws.Component = Component;
				aws.Review = Review;
				aws.UsedSystemTolerancePercent = UsedSystemTolerancePercent;
				aws.TolerancePercent = TolerancePercent;
				aws.StoreFilterRID = StoreFilterRID;
				return aws;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// Processes an allocation work flow.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Inherits from the ApplicationBaseWorkFlow.
	/// </para><para>
	/// The Process method of this class executes the requested actions/methods within the workflow.
	/// </para><para>  
	/// Additional fields for an allocation work flow:
	/// <list type="bullet">
	/// <item>AllocationProfile:  Identifies the header for which an allocation is to be performed.</item>
	/// <item>AllocationCubeGroup: Identifies the cube group where the allocation profiles and their supporting plans reside.</item>
	/// <item>AllocationWorkFlowStep: Defines the requested action.</item>
	/// <item>AllocationBaseMethod: Defines action inputs.</item>
	/// <item>RestartAtLine: Identifies the action/method line within the workflow where processing is to begin.</item>
	/// </list></para>
	/// <para>
	/// The RestartAtLine is automatically set to the first action/method line within the workflow when the workflow is created.
	/// </para>
	/// </remarks>
	public class AllocationWorkFlow : ApplicationBaseWorkFlow
	{
		//=======
		// FIELDS
		//=======
		private AllocationWorkflowData			_workflowData;
		private GetMethods						_getMethods;
        //private SessionAddressBlock				SAB;
		private AllocationProfile				_allocationProfile;
		private ApplicationSessionTransaction	_applicationSessionTransaction;
		private ApplicationBaseAction			_method;
		private int								_restartAtLine;

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
		public AllocationWorkFlow(
			SessionAddressBlock aSAB,
			int aWorkFlowRID,
			int aUserRID,
			bool aGlobalFlagValue)
			: base(aSAB, eWorkflowType.Allocation, aWorkFlowRID, aUserRID, aGlobalFlagValue)
		{
			//			ApplicationSessionTransaction applicationTransaction = new ApplicationSessionTransaction(aSAB);
			_restartAtLine = 0;
			if (base.Filled)
			{
				if (base.WorkFlowType != eWorkflowType.Allocation)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_WorkflowTypeInvalid),
						MIDText.GetText(eMIDTextCode.msg_WorkflowTypeInvalid));
				}
				_workflowData = new AllocationWorkflowData(base.Key, eChangeType.populate);
				_getMethods = new GetMethods(aSAB);
				DataTable ws = _workflowData.GetWorkflowSteps();
                // Begin TT#697 - JSmith - Performance
                DataTable wsc = _workflowData.GetWorkflowStepComponents(base.Key);
                // End TT#697
				foreach (DataRow dr in ws.Rows)
				{
					int stepNumber = Convert.ToInt32(dr["STEP_NUMBER"], CultureInfo.CurrentUICulture);
					eMethodType methodType = (eMethodType)(Convert.ToInt32(dr["ACTION_METHOD_TYPE"], CultureInfo.CurrentUICulture));
					int methodRID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);
					double pctTolerance = Convert.ToDouble(dr["PCT_TOLERANCE"], CultureInfo.CurrentUICulture);
					bool reviewInd = Include.ConvertCharToBool(Convert.ToChar(dr["REVIEW_IND"], CultureInfo.CurrentUICulture));
					int storeFilterRID = Convert.ToInt32(dr["STORE_FILTER_RID"], CultureInfo.CurrentUICulture);

					bool usedSystemTolerancePercent = false;
					if (pctTolerance == Include.UseSystemTolerancePercent)
					{
						usedSystemTolerancePercent = true;
						pctTolerance = aSAB.ApplicationServerSession.GlobalOptions.BalanceTolerancePercent;
					}

					int componentCriteriaRID;
					GeneralComponentWrapper gcw = null;
                    // Begin TT#697 - JSmith - Performance
                    //DataTable wsc = _workflowData.GetWorkflowStepComponents(base.Key, stepNumber);
                    DataRow[] rows = wsc.Select("STEP_NUMBER = " + stepNumber.ToString(CultureInfo.CurrentUICulture));
                    // End TT#697
					// for now, only one component is allowed in a workflow step.
					//					foreach (DataRow cdr in wsc.Rows)
					//					{
                    // Begin TT#697 - JSmith - Performance
                    //if(wsc != null && wsc.Rows.Count == 1)
                    //{
                    //    DataRow cdr = wsc.Rows[0];
                    if (rows != null && rows.Length == 1)
					{
                        DataRow cdr = rows[0];
                    // End TT#697 - JSmith - Performance
						eComponentType componentType = (eComponentType)(Convert.ToInt32(cdr["COMP_TYPE_ID"], CultureInfo.CurrentUICulture));
						componentCriteriaRID = Convert.ToInt32(cdr["COMP_CRIT_RID"], CultureInfo.CurrentUICulture);
						int colorCodeRID = Convert.ToInt32(cdr["COLOR_CODE_RID"], CultureInfo.CurrentUICulture);
						int sizeCodeRID = Convert.ToInt32(cdr["SIZE_CODE_RID"], CultureInfo.CurrentUICulture);
						string packName = Convert.ToString(cdr["PACK_NAME"], CultureInfo.CurrentUICulture);

						gcw = new GeneralComponentWrapper(componentType, colorCodeRID, sizeCodeRID, packName);
					}
					else
					{
						throw new Exception("Should be one workflow step component");
					}

					ApplicationBaseAction action = null;
					ApplicationBaseMethod method = null;
					int methodUserRid = 1; // Underfined
					if (methodRID != Include.NoRID)
					{
						// TODO add getmethod to transaction
						//						method = applicationTransaction.GetUnknownMethod((eMethodType) methodType);
						//						method = applicationTransaction.GetUnknownMethod(methodRID, false);
						action = _getMethods.GetMethod(methodRID, methodType);
						if (methodRID != Include.UndefinedMethodRID)
						{
							method = (ApplicationBaseMethod)action;
							methodUserRid = method.User_RID;
						}
					}
					AllocationWorkFlowStep allocationWorkFlowStep = new AllocationWorkFlowStep(action, 
						componentCriteriaRID, gcw.GeneralComponent, reviewInd,
						usedSystemTolerancePercent, pctTolerance, storeFilterRID, stepNumber, methodUserRid);  // Issue 5253
					AddWorkFlowStep(allocationWorkFlowStep);

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

		/// <summary>
		/// Gets the RestartAtLine
		/// </summary>
		public int RestartAtLine
		{
			get
			{
				return _restartAtLine + 1;
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

            foreach (AllocationWorkFlowStep workFlowStep in Workflow_Steps.ArrayList)
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
			if (_workflowData == null || base.Key < 0 || base.Workflow_Change_Type == eChangeType.add)
			{
				_workflowData = new AllocationWorkflowData(td);
			}
			try
			{
				int stepNumber = 0;
				switch (base.Workflow_Change_Type)
				{
					case eChangeType.add:
						base.Update(td);
						foreach (AllocationWorkFlowStep aws in Workflow_Steps)
						{
							_workflowData.Comp_Crit_RID = Include.NoRID;
							GeneralComponentWrapper gcw = new GeneralComponentWrapper(aws.Component);
							_workflowData.Component_Type = gcw.ComponentType;
							_workflowData.Color_Code_RID = gcw.ColorRID;
							_workflowData.Size_Code_RID = gcw.SizeRID;
							_workflowData.PackName = gcw.PackName;
							if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)gcw.ComponentType))
							{
								_workflowData.InsertComponentCriteria(td);
							}
							_workflowData.Workflow_RID = base.Key;
							_workflowData.Step_Number = stepNumber;
							_workflowData.Method_RID = aws.Method.Key;
							_workflowData.Action_Method_Type = aws.Method.MethodType;
							_workflowData.Step_Store_Filter_RID = aws.StoreFilterRID;
							if (aws.UsedSystemTolerancePercent)
							{
								_workflowData.Pct_Tolerance = Include.UseSystemTolerancePercent;
							}
							else
							{
								_workflowData.Pct_Tolerance = aws.TolerancePercent;
							}
							_workflowData.InsertWorkflowStep(base.Key, td);
							_workflowData.InsertWorkflowStepComponent(base.Key, td);
							++stepNumber;
						}
						break;
					case eChangeType.update:
						base.Update(td);
						_workflowData.DeleteAllWorkflowSteps(base.Key, td);
						foreach (AllocationWorkFlowStep aws in Workflow_Steps)
						{
							_workflowData.Comp_Crit_RID = Include.NoRID;
							GeneralComponentWrapper gcw = new GeneralComponentWrapper(aws.Component);
							_workflowData.Component_Type = gcw.ComponentType;
							_workflowData.Color_Code_RID = gcw.ColorRID;
							_workflowData.Size_Code_RID = gcw.SizeRID;
							_workflowData.PackName = gcw.PackName;
							if (Enum.IsDefined(typeof(eSpecificComponentType),(eSpecificComponentType)gcw.ComponentType))
							{
								_workflowData.InsertComponentCriteria(td);
							}
							
							_workflowData.Workflow_RID = base.Key;
							_workflowData.Step_Number = stepNumber;
							_workflowData.Method_RID = aws.Method.Key;
							_workflowData.Action_Method_Type = aws.Method.MethodType;
							_workflowData.Step_Store_Filter_RID = aws.StoreFilterRID;
							if (aws.UsedSystemTolerancePercent)
							{
								_workflowData.Pct_Tolerance = Include.UseSystemTolerancePercent;
							}
							else
							{
								_workflowData.Pct_Tolerance = aws.TolerancePercent;
							}
							_workflowData.InsertWorkflowStep(base.Key, td);
							_workflowData.InsertWorkflowStepComponent(base.Key, td);
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

		/// <summary>
		/// Processes the methods and actions contained in the workflow.
		/// </summary>
		/// <param name="aApplicationSessionTransaction">Transaction containing the list of AllocationProfiles on which this workflow will act.</param>
		/// <param name="aIgnoreReview">True indicates to ignore all review requests and continue processing; false indicates to stop processing when a review request is encountered.</param>
		/// <param name="aWriteToDB">True indicates to write the allocation to the database on successful completion of each request, false incidates that no writes to the database are to occur.</param>
		/// <param name="aRestartAtLine">Workflow line where processing is to begin. Zero or 1 starts at the beginning.</param>
		/// <param name="aWorkflowProcessOrder">Indicates the orders which the headers and steps are to be executed</param>
		/// <returns>True if the process action is successful; false if the process action is unsuccessful.</returns> 
		override public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder)
		{
			// BEGIN TT#698-MD - Stodd - add ability for workflows to be run against assortments.
			AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
			// Begin TT#1098 - MD - stodd - add Group Allocation Method to workflow screen - 
            //================================================================================================================
            // The master allocation profile list now contains any selected headers from assortment or group allocation.
            // So the following code is now commented out
            //================================================================================================================
            //if (AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID != Include.NoRID)	// use an assortment
            //{
            //    ApplicationBaseAction aba = ((AllocationWorkFlowStep)Workflow_Steps[0]).Method;
            //    eMethodType methodType = aba.MethodType;
            //    SelectedHeaderList assrtHdrList = SAB.AssortmentSelectedHeaderEvent.GetSelectedHeaders(this, AssortmentActiveProcessToolbarHelper.ActiveProcess.screenID, methodType);
            //    SelectedHeaderList selHdrList = new SelectedHeaderList(eProfileType.SelectedHeader);
            //    // Remove assortment header from processing list
            //    foreach (SelectedHeaderProfile ahp in assrtHdrList)
            //    {
            //        if (ahp.HeaderType != eHeaderType.Assortment)
            //        {
            //            selHdrList.Add(ahp);
            //        }
            //        //else
            //        //{
            //        //    selHdrList.Add(ahp);
            //        //}
            //    }
            //    apl.LoadHeaders(aApplicationSessionTransaction, selHdrList, SAB.ApplicationServerSession);
            //}
            //else
            //{
				apl = (AllocationProfileList)aApplicationSessionTransaction.GetMasterProfileList(eProfileType.Allocation);
            //}
			// End TT#1098 - MD - stodd - add Group Allocation Method to workflow screen - 
			// END TT#698-MD - Stodd - add ability for workflows to be run against assortments.

			return Process(
					aApplicationSessionTransaction,
					aIgnoreReview,
					aWriteToDB,
					aRestartAtLine,
					aWorkflowProcessOrder,
					apl);	// TT#698-MD - Stodd - add ability for workflows to be run against assortments.
		}
		override public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder,
			ProfileList aProfileList)
		{
			this._applicationSessionTransaction = aApplicationSessionTransaction;
            //this.SAB = _applicationSessionTransaction.SAB;
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
			if (UserRID == Include.GlobalUserRID)  // issue 3806
			{
				wfn += ":" + this.WorkFlowName;
			}
			else
			{
				wfn += "/" + this.SAB.ClientServerSession.GetUserName(UserRID) + ":" + this.WorkFlowName;  
			}
			SAB.ApplicationServerSession.Audit.Add_Msg(
				eMIDMessageLevel.Information,
				eMIDTextCode.msg_AllocationWorkFlowStart,
				wfn, this.GetType().Name);
			if (aWorkflowProcessOrder == eWorkflowProcessOrder.AllStepsForHeader)
			{
				foreach (AllocationProfile ap in aProfileList)
				{
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    if (
                        (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                        || (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                        )
                    {
                        continue;
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment

// (CSMITH) - BEG MID Track #3156: Headers with status Rec'd OUT of Bal are being processed
					// BEGIN MID Track #4022 - add 'InUseByMultiHeader' to 'if...' statement
					if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance
					 || ap.HeaderAllocationStatus == eHeaderAllocationStatus.InUseByMultiHeader)
					// END MID Track #4022
					{
						string msg = string.Format(
							SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false),
																		MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
						msg = msg.Replace(" action for headers", " workflow for header");
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Warning,
							("Workflow: " + wfn + ", Header: " + ap.HeaderID + " - " + msg),
							this.GetType().Name);
						successFlag = false;
					}
					else
					{
						if (aRestartAtLine < 1)
						{
							_restartAtLine = 0;
						}
						else  
						{
							if (aRestartAtLine > Workflow_Steps.Count)
							{
								SAB.ApplicationServerSession.Audit.Add_Msg(
									eMIDMessageLevel.Error,
									eMIDTextCode.msg_RestartAtLineExceedsCount,
									wfn + " " + (aRestartAtLine.ToString(CultureInfo.CurrentUICulture) + " > " + Workflow_Steps.Count.ToString(CultureInfo.CurrentUICulture)),
									this.GetType().Name);
								successFlag = false;
							}
							else
							{
								_restartAtLine = aRestartAtLine - 1;
							}
						}
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Information,
							eMIDTextCode.msg_RestartWorkFlow,
							wfn + " " + _restartAtLine.ToString(CultureInfo.CurrentUICulture), this.GetType().Name);
						if (!ProcessHeader (wfn, ap, aIgnoreReview, aWriteToDB))
						{
							successFlag = false;
						}
					}
				}
			}
			else
			{
				bool stopForReview = false;
				if (aRestartAtLine < 1)
				{
					_restartAtLine = 0;
				}
				else  
				{
					if (aRestartAtLine > Workflow_Steps.Count)
					{
						SAB.ApplicationServerSession.Audit.Add_Msg(
							eMIDMessageLevel.Error,
							eMIDTextCode.msg_RestartAtLineExceedsCount,
							wfn + " " + (aRestartAtLine.ToString(CultureInfo.CurrentUICulture) + " > " + Workflow_Steps.Count.ToString(CultureInfo.CurrentUICulture)),
							this.GetType().Name);
						successFlag = false;
					}
					else
					{
						_restartAtLine = aRestartAtLine - 1;
					}
				}
				if (successFlag)
				{
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Information,
						eMIDTextCode.msg_RestartWorkFlow,
						wfn + " " + _restartAtLine.ToString(CultureInfo.CurrentUICulture), this.GetType().Name);
					while (stopForReview == false && _restartAtLine <= Workflow_Steps.Count)
					{
						AllocationWorkFlowStep workFlowStep = 
							(AllocationWorkFlowStep) Workflow_Steps[_restartAtLine];
						if (!ProcessStepForAllHeaders(wfn, workFlowStep, aWriteToDB, (AllocationProfileList)aProfileList))
						{
							successFlag = false;
							break;
						}
						else
						{
							if (aIgnoreReview == false & workFlowStep.Review)
							{
								stopForReview = true;
								SAB.ApplicationServerSession.Audit.Add_Msg(
									eMIDMessageLevel.Information,
									wfn + " " + MIDText.GetText(eMIDTextCode.msg_al_WokflowStoppedForReview),
									this.GetType().Name);
							}
							_restartAtLine++;
						}
					}
				}
			}
			SAB.ApplicationServerSession.Audit.Add_Msg(
				eMIDMessageLevel.Information,
				eMIDTextCode.msg_AllocationWorkFlowEnd,
				wfn, this.GetType().Name);
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
			SAB.ApplicationServerSession.Audit.Add_Msg(
				eMIDMessageLevel.Information,
				eMIDTextCode.msg_AllocationHeaderStart,
				aWorkFlowName + " " + aAllocationProfile.HeaderID.ToString(), this.GetType().Name);
			aAllocationProfile.ActiveWorkflowRID = this.Key;
			bool processSuccessFlag = true;
			_allocationProfile = aAllocationProfile;
			bool stopForReview = false;
            // Begin TT#2028-MD - JSmith - When processing a workflow on a header in an asst the PH does not reduce in qty.
            if (aAllocationProfile.AsrtRID != Include.NoRID)
            {
                aAllocationProfile.ActivateAssortment = true;
            }
            // End TT#2028-MD - JSmith - When processing a workflow on a header in an asst the PH does not reduce in qty.
			while (stopForReview == false && _restartAtLine < Workflow_Steps.Count)
			{
				AllocationWorkFlowStep workFlowStep = 
					(AllocationWorkFlowStep) Workflow_Steps[_restartAtLine];
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
					// Begin Track #5727
					this._applicationSessionTransaction.SetAllocationActionStatus(aAllocationProfile.Key, eAllocationActionStatus.ActionFailed);
					// End Track #5727
					break;
				}
			}
			aAllocationProfile.ActiveWorkflowRID = Include.NoRID;
            // Begin TT#2028-MD - JSmith - When processing a workflow on a header in an asst the PH does not reduce in qty.
            aAllocationProfile.ActivateAssortment = false;
            // End TT#2028-MD - JSmith - When processing a workflow on a header in an asst the PH does not reduce in qty.
			SAB.ApplicationServerSession.Audit.Add_Msg(
				eMIDMessageLevel.Information,
				eMIDTextCode.msg_AllocationHeaderEnd,
				aWorkFlowName + " " + aAllocationProfile.HeaderID.ToString(), this.GetType().Name);
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
			foreach (AllocationProfile ap in aAllocationProfileList)
			{
                // Begin TT#1966-MD - JSmith - DC Fulfillment
                if (
                    (ap.IsMasterHeader && ap.DCFulfillmentProcessed)
                    || (ap.IsSubordinateHeader && !ap.DCFulfillmentProcessed)
                    )
                {
                    continue;
                }
                // End TT#1966-MD - JSmith - DC Fulfillment

				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_AllocationHeaderStart,
					aWorkFlowName + " " + ap.HeaderID.ToString(), this.GetType().Name);
				ap.ActiveWorkflowRID = this.Key;


				_allocationProfile = ap;

				if (ap.HeaderAllocationStatus == eHeaderAllocationStatus.ReceivedOutOfBalance)
				{
					string msg = string.Format(
						SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_HeaderStatusDisallowsAction, false),
																	MIDText.GetTextOnly((int)ap.HeaderAllocationStatus));
					msg = msg.Replace(" action for headers", " workflow for header");
					SAB.ApplicationServerSession.Audit.Add_Msg(
						eMIDMessageLevel.Warning,
						("Workflow: " + aWorkFlowName + ", Header: " + ap.HeaderID + " - " + msg),
						this.GetType().Name);
					processSuccessFlag = false;
				}
				else
				{
					if (!this.DoStep(aWorkFlowName, aWorkflowStep, aWriteToDB))
					{
						processSuccessFlag = false;
					}
					ap.ActiveWorkflowRID = Include.NoRID;
				}

				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					eMIDTextCode.msg_AllocationHeaderEnd,
					aWorkFlowName + " " + ap.HeaderID.ToString(), this.GetType().Name);
			}
			if (processSuccessFlag)
			{
				_restartAtLine++;
			}
			return processSuccessFlag;
		}
		private bool DoStep(string aWorkFlowName, AllocationWorkFlowStep aWorkFlowStep, bool aWriteToDB)
		{
			bool successFlag = true;

			_method = aWorkFlowStep._method;
			try
			{
				_method.ProcessAction(
					SAB,
					_applicationSessionTransaction,
					aWorkFlowStep,
					_allocationProfile,
					aWriteToDB,
					_workflowData.Store_Filter_RID);
                //if (_applicationSessionTransaction.GetAllocationActionStatus(_allocationProfile.Key) == eAllocationActionStatus.ActionFailed) // TT#241 - MD JEllis - Header Enqueue Process
                if (_applicationSessionTransaction.GetAllocationActionStatus(_allocationProfile.Key) != eAllocationActionStatus.ActionCompletedSuccessfully
                    && _applicationSessionTransaction.GetAllocationActionStatus(_allocationProfile.Key) != eAllocationActionStatus.NoActionPerformed
                    ) // TT#241 - MD Jellis - Header Enqueue Process
				{
					successFlag = false;
				}
                // Begin TT#378 - JSmith - Workflow Run individual methods and get expected results run, Run the same methods in a Workflow and do not get the same results.
                // Begin TT#668 - JSmith - Workflow does not match individual
                //if (aWorkFlowStep.Method != null &&
                //    (aWorkFlowStep.Method.MethodType == eMethodType.GeneralAllocation ||
                //    aWorkFlowStep.Method.MethodType == eMethodType.AllocationOverride))
                //{
                    //_applicationSessionTransaction.ClearAllocationCubeGroup();
                //}
                _applicationSessionTransaction.ClearAllocationCubeGroup();
                // End TT#668
                // End TT#378
				//Begin TT#668 - JScott - Workflow does not match individual

				_applicationSessionTransaction.ResetFilter(eProfileType.Store);
				//End TT#668 - JScott - Workflow does not match individual
			}
			catch (MIDException err)
			{
				//Begin TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				//eMIDMessageLevel messageLevel = eMIDMessageLevel.Information;
				//switch (err.ErrorLevel)
				//{
				//    case eErrorLevel.fatal:
				//    case eErrorLevel.severe:
				//        messageLevel = eMIDMessageLevel.Severe;
				//        break;
				//    case eErrorLevel.warning:
				//        messageLevel = eMIDMessageLevel.Warning;
				//        break;
				//}
				eMIDMessageLevel messageLevel = Include.TranslateErrorLevel(err.ErrorLevel);
				//End TT#769 - JScott - Incorrect Error Level - "Cannot change Units to Allocate when Released"
				SAB.ApplicationServerSession.Audit.Add_Msg(messageLevel, err.ErrorMessage, this.GetType().Name);  // MID Track 5778 - Schedule 'Run Now' features gets error in audit
				SAB.ApplicationServerSession.Audit.Add_Msg(
					eMIDMessageLevel.Information,
					aWorkFlowName + " " + MIDText.GetText(eMIDTextCode.msg_AllocationWorkFlowError),
					this.GetType().Name);
				if (err.ErrorLevel != eErrorLevel.information
					& err.ErrorLevel != eErrorLevel.warning)
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
				
				successFlag = false;
				// begin TT#3019 - Jellis - AnF - Workflow VSW results different than step by step
            }
            finally
            {
                _applicationSessionTransaction.GetIMOReader().ResetIMO_Reader();
                _applicationSessionTransaction.GetIntransitReader().ResetIntransitReader();
                _allocationProfile.LoadIntransit = true; // TT#3071 - AnF - VsW logic not using Intransit in Workflow
            }
				// end TT#3019 - Jellis - AnF - Workflow VSW results different than step by step
			return successFlag;
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
			AllocationWorkFlow newAllocationWorkFlow = null;

			try
			{
				newAllocationWorkFlow = (AllocationWorkFlow)this.MemberwiseClone();
				newAllocationWorkFlow.Key = Include.NoRID;

				newAllocationWorkFlow.IsGlobal = IsGlobal;
				newAllocationWorkFlow.StoreFilterRID = StoreFilterRID;
				newAllocationWorkFlow.UserRID = UserRID;
				newAllocationWorkFlow.Workflow_Change_Type = eChangeType.none;
				newAllocationWorkFlow.WorkFlowDescription = WorkFlowDescription;
				newAllocationWorkFlow.WorkFlowName = WorkFlowName;
				newAllocationWorkFlow.WorkFlowType = WorkFlowType;

				newAllocationWorkFlow.Workflow_Steps = new ProfileList(eProfileType.AllocationWorkFlowStep);
				foreach (AllocationWorkFlowStep allocationWorkFlowStep in Workflow_Steps)
				{
					newAllocationWorkFlow.Workflow_Steps.Add(allocationWorkFlowStep.Copy());
				}

				return newAllocationWorkFlow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        override public FunctionSecurityProfile GetFunctionSecurity()
        {
            if (this.GlobalUserType == eGlobalUserType.Global)
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsGlobalGeneralAllocation);
            }
            else
            {
                return SAB.ClientServerSession.GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationMethodsUserGeneralAllocation);
            }

        }
    }
}
