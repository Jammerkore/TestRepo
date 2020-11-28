using System;
using System.Collections;
using System.Data;
using System.Globalization;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
	/// <summary>
	/// Base, abstract class for an application work flow.
	/// </summary>
	[Serializable] 
	abstract public class ApplicationBaseWorkFlow:Profile
	{
		//=======
		// FIELDS
		//=======
		private string					_workflow_Name;
		private int						_workflow_UserRID;
		private eWorkflowType			_workflow_Type;
		private bool					_isGlobal;
		private string					_workflow_Description;
		private int						_storeFilterRID;
		private bool					_manualOverride;
		private bool					_filled;
		private eChangeType				_workflow_Change_Type;
		private ProfileList				_workflow_Steps;
        private SessionAddressBlock     _SAB;
	

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of an ApplicationBaseWorkFlow
		/// </summary>
		/// <param name="SAB">Session Address Block.</param>
		/// <param name="aWorkflow_Type">The type of workflow being created.</param>
		/// <param name="aWorkflowRID">RID of the workflow (primary key).</param>
		/// <param name="aUserRID">RID for the user who created this workflow (primary key).</param>
		/// <param name="aGlobalFlagValue">IsGlobal flag value (primary key): true indicates this is a global workflow; false indicates it is a user specific workflow.</param>
		internal ApplicationBaseWorkFlow(
			SessionAddressBlock SAB,
			eWorkflowType aWorkflow_Type,
			int aWorkflowRID, 
			int aUserRID, 
			bool aGlobalFlagValue):base(aWorkflowRID)
		{
            _SAB = SAB;
			_workflow_UserRID = aUserRID;
			_isGlobal = aGlobalFlagValue;
			_workflow_Type  = aWorkflow_Type;
			_storeFilterRID = 0;
			_manualOverride = false;
			_filled = false;
			_workflow_Change_Type = eChangeType.none;
			_workflow_Steps = new ProfileList(eProfileType.AllocationWorkFlowStep);
			if (aWorkflowRID > 0)
			{
				Populate(aWorkflowRID);
			}
		}

		//===========
		// PROPERTIES
		//===========
        /// <summary>
        /// Gets the SessionAddressBlock.
        /// </summary>
        public SessionAddressBlock SAB
        {
            get
            {
                return _SAB;
            }
        }

		/// <summary>
		/// Gets WorkFlowName.  This is one of three primary keys for the workflow.
		/// </summary>
		public string WorkFlowName
		{
			get
			{
				return _workflow_Name;
			}
			set
			{
				_workflow_Name = value;
			}
		}

		/// <summary>
		/// Gets UserRID associated with this work flow.  This is one of three primary keys for the workflow. 
		/// </summary>
		public int UserRID
		{
			get
			{
				return _workflow_UserRID;
			}
			set
			{
				_workflow_UserRID = value;
			}
		}

		/// <summary>
		/// Gets IsGlobal flag value associated with this work flow. This is one of three primary keys for the workflow.
		/// </summary>
		public bool IsGlobal
		{
			get
			{
				return _isGlobal;
			}
			set
			{
				_isGlobal = value;
			}
		}

		/// <summary>
		/// Gets or sets Global User type
		/// </summary>
		public eGlobalUserType GlobalUserType
		{
			get 
			{
				if (UserRID == Include.GetGlobalUserRID())
					return eGlobalUserType.Global;
				else
					return eGlobalUserType.User; 
			}
		}

		/// <summary>
		/// Gets or sets the work flow description.
		/// </summary>
		public string WorkFlowDescription
		{
			get
			{
				return _workflow_Description;
			}
			set
			{
				_workflow_Description = value;
			}
		}

		/// <summary>
		/// Gets or sets Store Filter RID
		/// </summary>
		/// <remarks>
		/// Identifies the universe of stores whose allocations may be changed while this workflow is being processed.
		/// </remarks>
		public int StoreFilterRID
		{
			get
			{
				return _storeFilterRID;
			}
			set
			{
				_storeFilterRID = value;
                // Begin TT#2741 - JSmith - Error with [None] filter in allocation workflow
                if (_storeFilterRID < Include.UndefinedStoreFilter)
                {
                    _storeFilterRID = Include.UndefinedStoreFilter;
                }
                // End TT#2741 - JSmith - Error with [None] filter in allocation workflow
			}
		}

		/// <summary>
		/// Gets or sets manual override
		/// </summary>
		/// <remarks>
		/// True indicates the user had the opportunity to issue overrides prior to the execution of the workflow.
		/// False indicates the user did not make any manual adjustments to the override Workflow prior to execution.
		/// </remarks>
		public bool ManualOverride
		{
			get
			{
				return	_manualOverride;
			}
			set
			{
				_manualOverride = value;
			}
		}


		/// <summary>
		/// Abstract property: Gets workflow type.
		/// </summary>
		public eWorkflowType WorkFlowType
		{
			get
			{
				return	_workflow_Type;
			}
			set
			{
				_workflow_Type = value;
			}
		}

		/// <summary>
		/// Gets Filled
		/// </summary>
		/// <remarks>True: Workflow is populated; False: Workflow has not been populated.</remarks>
		public bool Filled 
		{
			get { return _filled ; }
		}

		/// <summary>
		/// Gets or set Workflow_Change_Type
		/// </summary>
		public eChangeType Workflow_Change_Type
		{
			get
			{
				return _workflow_Change_Type;
			}
			set
			{
				_workflow_Change_Type = value;
			}
		}

		/// <summary>
		/// Gets or set Workflow_Steps
		/// </summary>
		public ProfileList Workflow_Steps
		{
			get
			{
				return _workflow_Steps;
			}
			set
			{
				_workflow_Steps = value;
			}
		}

		//========
		// Methods
		//========
		private void Populate(int aWorkflowRID)
		{
			WorkflowBaseData wb = new WorkflowBaseData();
			_filled = wb.PopulateWorkflow(base.Key);
			if (_filled)
			{
				// if unknown Workflow type, set base Workflow type once known
				if (Convert.ToInt32(_workflow_Type, CultureInfo.CurrentUICulture) == -1)  
				{
					wb.Workflow_Type_ID = _workflow_Type;
				}
				else
					if (wb.Workflow_Type_ID != _workflow_Type)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_MethodsOutOfSync),
						MIDText.GetText(eMIDTextCode.msg_MethodsOutOfSync));
				}
				
				_workflow_Name = wb.Workflow_Name;
				_workflow_UserRID = wb.User_RID;
				_workflow_Description = wb.Workflow_Description;
				_storeFilterRID = wb.Store_Filter_RID;
				_manualOverride = wb.ManualOverride;
			}
		}

		abstract public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder);


		abstract public bool Process(
			ApplicationSessionTransaction aApplicationSessionTransaction,
			bool aIgnoreReview,
			bool aWriteToDB,
			int aRestartAtLine,
			eWorkflowProcessOrder aWorkflowProcessOrder,
			ProfileList aProfileList);

		/// <summary>
		/// Creates or updates the base Workflow.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		virtual public void Update(TransactionData td)
		{
			WorkflowBaseData wb = new WorkflowBaseData(td);
			if (base.Key < 0 || _workflow_Change_Type == eChangeType.add)
			{
				base.Key = Include.NoRID;
			}
			wb.Workflow_Type_ID = _workflow_Type;
			wb.Workflow_RID = base.Key;
			wb.Workflow_Name = _workflow_Name ;
			wb.User_RID = _workflow_UserRID;
			wb.Workflow_Description = _workflow_Description;
			wb.Store_Filter_RID = _storeFilterRID;
			wb.ManualOverride = _manualOverride;
			try
			{
				switch (_workflow_Change_Type)
				{
					case eChangeType.add:
                        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                        //base.Key = wb.CreateWorkflow();
                        base.Key = wb.CreateWorkflow(SAB.ClientServerSession.UserRID);
                        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
						break;
					case eChangeType.update:
                        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                        //wb.UpdateWorkflow();
                        wb.UpdateWorkflow(SAB.ClientServerSession.UserRID);
                        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
						break;
					case eChangeType.delete:
						wb.DeleteWorkflow();
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
				//               To Do:  whatever is necessary to complete the update or recover from exception
			}
		}

		/// <summary>
		/// Adds Workflows to the workflow.
		/// </summary>
		/// <param name="aWorkflowStep">ApplicationWorkflowStep describing the Workflow/action and how to process it.</param>
		abstract public void AddWorkFlowStep(ApplicationWorkFlowStep aWorkflowStep);

		/// <summary>
		/// Loads workflow from database.
		/// </summary>
		abstract public void LoadFromDB();
		
		/// <summary>
		/// Updates workflow on database.
		/// </summary>
		abstract public void UpdateDB();

		/// <summary>
		/// Attaches this workflow to the specified header's history.
		/// </summary>
		/// <param name="aHeaderRID">RID of the header.</param>
		abstract public void UpdateHeaderHistory(int aHeaderRID);

		/// <summary>
		/// Makes a copy of the object.
		/// </summary>
		/// <remarks>
		/// Clone can not be overidden in an inheritance chain
		/// </remarks>
		abstract public ApplicationBaseWorkFlow Copy(Session aSession, bool aCloneDateRanges);
	
	}

	/// <summary>
	/// Describes a workflow step instance.
	/// </summary>
	[Serializable]
	abstract public class ApplicationWorkFlowStep:Profile
	{
		//=======
		// FIELDS
		//=======
		internal ApplicationBaseAction _method;
		internal int _storeFilterRID;

		//=============
		// CONSTRUCTORS
		//=============
		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		/// <param name="aMethod">Workflow to attach to the workflow.</param>
		/// <param name="aStoreFilterRID">RID of the store filter</param>
		/// <param name="aWorkflowStepKey">Workflow Step Key</param>
		public ApplicationWorkFlowStep(ApplicationBaseAction aMethod, int aStoreFilterRID, int aWorkflowStepKey)
			:base(aWorkflowStepKey)
		{
			CheckMethod (aMethod);
			_method = aMethod;
			_storeFilterRID = aStoreFilterRID;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets or sets the application base Workflow 
		/// </summary>
		public ApplicationBaseAction Method
		{
			get
			{
				return _method;
			}
			set
			{
				CheckMethod(value);
				_method = value;
			}
		}

		/// <summary>
		/// Gets or sets the store filter RID for the Workflow step
		/// </summary>
		public int StoreFilterRID
		{
			get
			{
				return _storeFilterRID;
			}
			set
			{
				_storeFilterRID = value;
			}
		}

		//========
		// WorkflowS
		//========
		/// <summary>
		/// Verifies that a Method is a valid Workflow for the workflow.
		/// </summary>
		/// <param name="aMethod">ApplicationBaseWorkflow to be checked.</param>
		abstract public void CheckMethod(ApplicationBaseAction aMethod);
	}

}
