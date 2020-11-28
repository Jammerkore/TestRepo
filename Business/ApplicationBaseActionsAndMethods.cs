using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;
using Logility.ROWebSharedTypes;

namespace MIDRetail.Business
{
	[Serializable]
	abstract public class ApplicationBaseAction:Profile
	{
		//=======
		// FIELDS
		//=======
		private eMethodType _action;
        private string _method_Type_Name;  // TT#1966-MD - JSmith- DC Fulfillment

		//=============
		// CONSTRUCTORS
		//=============
		public ApplicationBaseAction(int aKey, eMethodType aMethodType):base(aKey)
		{
			_action = aMethodType;
			VerifyAction(aMethodType);
            _method_Type_Name = MIDText.GetTextFromCode(Convert.ToInt32(aMethodType));  // TT#1966-MD - JSmith- DC Fulfillment
		}

		//===========
		// PROPERTIES
		//===========
		public eMethodType MethodType
		{
			get
			{
				return _action;
			}
		}

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        protected string MethodTypeName
        {
            get
            {
                return _method_Type_Name;
            }
        }
        // End TT#1966-MD - JSmith- DC Fulfillment

		//========
		// METHODS
		//========
		abstract internal void VerifyAction(eMethodType aMethodType);

		abstract public void ProcessMethod(ApplicationSessionTransaction aApplicationTransaction,
			int aStoreFilter, Profile methodProfile);

		abstract public bool WithinTolerance(double aTolerancePercent);

		abstract public void ProcessAction(
			SessionAddressBlock aSAB, 
			ApplicationSessionTransaction aApplicationTransaction, 
			ApplicationWorkFlowStep aWorkFlowStep, 
			Profile aProfile,
			bool WriteToDB,
			int aStoreFilterRID);

		internal void SetMethodType(eMethodType aMethodType)
		{
			_action = aMethodType;
            _method_Type_Name = MIDText.GetTextFromCode(Convert.ToInt32(aMethodType));  // TT#1966-MD - JSmith- DC Fulfillment
		}

	}

	/// <summary>
	/// The base application business method.
	/// </summary>
	/// <remarks>
	/// This method is the base method for all application business method objects.
	/// It contains the following common information:
	/// <list type="bullet">
	/// <item>MethodName: name of the allocation method within method type for a given user (either global or specific).</item>
	/// <item>UserRID: The RID for the user who defined this method (when global, UserRID = 1).</item>
	/// <item>MethodGlobal: True indicates a global method; False incicates a specific user's method.</item>
	/// <item>MethodDescription: A user supplied description of the method.</item>
	/// <item>StoreGroupRID: An optional RID that identifies an associated Store Group (aka Attribute)</item>
	/// </list>
	/// </remarks>
	[Serializable]
	abstract public class ApplicationBaseMethod:ApplicationBaseAction, IComparable , ICloneable
	{
		//=======
		// FIELDS
		//=======
		private SessionAddressBlock _SAB;
//		private MethodBaseData _mb;
		private string _method_Name;
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		private eProfileType _methodProfileType;
		//End TT#523 - JScott - Duplicate folder when new folder added
		private int _user_RID;
		private string _method_Description;
		private int _sg_RID;
		private bool _virtual_IND;
        private bool _filled;
		private eChangeType		_method_Change_Type;
		private eMethodStatus	_methodStatus;
        private int _customOLL_RID;     // MID Track #5530 - add CUSTOM_OLL_RID column 
        private bool? _containsUserData = null;   // TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        private eLockStatus _lockStatus;

		//============
		// Constructor
		//============
		public ApplicationBaseMethod(SessionAddressBlock aSAB,
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			//int aMethodRID, eMethodType aMethodType)
			int aMethodRID, eMethodType aMethodType, eProfileType aProfileType)
			//End TT#523 - JScott - Duplicate folder when new folder added
			: base(aMethodRID, aMethodType)
		{
			_SAB = aSAB;
//			MethodBaseData mb = null;
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			_methodProfileType = aProfileType;
			//End TT#523 - JScott - Duplicate folder when new folder added
			_method_Name = null;
			_user_RID = Include.UndefinedUserRID;
			_method_Description = null;
			if ((Enum.IsDefined(typeof(eAllocationMethodType),Convert.ToInt32(aMethodType, CultureInfo.CurrentUICulture))))
			{
				_sg_RID = aSAB.ApplicationServerSession.GlobalOptions.AllocationStoreGroupRID;
			}
			else
				if ((Enum.IsDefined(typeof(eForecastMethodType),Convert.ToInt32(aMethodType, CultureInfo.CurrentUICulture))))
			{
				_sg_RID = aSAB.ApplicationServerSession.GlobalOptions.OTSPlanStoreGroupRID;
			}
			else
			{
				_sg_RID = -1;	// unknown method type
			}
			_virtual_IND = false;
			_filled = false;
			_method_Change_Type = eChangeType.none;
            _customOLL_RID = -1;    // MID Track #5530 - add CUSTOM_OLL_RID
			if (base.Key > 0)
			{
				Populate(aMethodRID);
			}
            _lockStatus = eLockStatus.Undefined;
		}

		//===========
		// PROPERTIES
		//===========
		/// <summary>
		/// Gets SessionAddressBlock associated with this method.
		/// </summary>
		public SessionAddressBlock SAB
		{
			get
			{
				return _SAB;
			}
		}

		/// <summary>
		/// Gets Filled
		/// </summary>
		/// <remarks>True: Method is populated; False: Method has not been populated.</remarks>
		public bool Filled 
		{
			get { return _filled ; }
		}

		/// <summary>
		/// Gets or sets method name.
		/// </summary>
		public string Name 
		{
			get { return _method_Name ; }
			set { _method_Name = value; }
		}

		//Begin TT#523 - JScott - Duplicate folder when new folder added
		public eProfileType MethodProfileType
		{
			get
			{
				return _methodProfileType;
			}
		}

		//End TT#523 - JScott - Duplicate folder when new folder added
//		/// <summary>
//		/// Gets or sets method type
//		/// </summary>
//		public eMethodType Method_Type_ID 
//		{
//			get { return _method_Type_ID ; }
//			set { _method_Type_ID = value;}
//		}

		/// <summary>
		/// Gets or sets User RID
		/// </summary>
		public int User_RID 
		{
			get { return _user_RID ; }
			set { _user_RID = value; }
		}

		/// <summary>
		/// Gets or sets Global User type
		/// </summary>
		public eGlobalUserType GlobalUserType
		{
			get 
			{
				if (User_RID == Include.GetGlobalUserRID())
					 return eGlobalUserType.Global;
				 else
					 return eGlobalUserType.User; }
		}

		/// <summary>
		/// Gets or sets Method description.
		/// </summary>
		public string Method_Description 
		{
			get { return _method_Description ; }
			set { _method_Description = value; }
		}

		/// <summary>
		/// Gets or sets Store Group RID
		/// </summary>
		public int SG_RID 
		{
			get { return _sg_RID ; }
			set { _sg_RID = value; }
		}

		/// <summary>
		/// Gets or sets Virtual indicator.
		/// </summary>
		/// <remarks>True:  indicates this is an "on-the-fly" method generated by the system; False: indicates a user defined method.</remarks>
		public bool Virtual_IND 
		{
			get { return _virtual_IND ; }
			set { _virtual_IND = value; }
		}

		/// <summary>
		/// Gets or set Method_Change_Type
		/// </summary>
		public eChangeType Method_Change_Type
		{
			get
			{
				return _method_Change_Type;
			}
			set
			{
				_method_Change_Type = value;
			}
		}

		/// <summary>
		/// gets or sets the method status
		/// </summary>
		public eMethodStatus MethodStatus 
		{
			get { return _methodStatus ; }
			set { _methodStatus = value; }
		}
        // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column
        /// <summary>
        /// gets or sets the Custom OverRide Low Level RID
        /// </summary>
        public int CustomOLL_RID
        {
            get { return _customOLL_RID; }
            set { _customOLL_RID = value; }
        }
        // END MID Track #5530  

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
		/// <summary>
        /// Gets a flag identifying if the workflow or method contains user data
        /// </summary>
        public bool? ContainsUserData
        {
            get 
            { 
                if (_containsUserData == null)
                {
                    DetermineIfContainsUserData();
                }
                return _containsUserData; 
            }
            set 
            { 
                _containsUserData = value; 
            }
        }

        /// <summary>
		/// Gets or set Workflow lock status
		/// </summary>
		public eLockStatus LockStatus
        {
            get
            {
                return _lockStatus;
            }
            set
            {
                _lockStatus = value;
            }
        }

        /// <summary>
        /// Determines if filter is a user filter
        /// </summary>
        /// <param name="aFilterRID">Filter Key</param>
        /// <returns></returns>
        public bool IsFilterUser(int aFilterRID)
        {
            if (aFilterRID > Include.UndefinedStoreFilter)
            {
                FilterData fd = new FilterData();
                if (fd.FilterGetOwner(aFilterRID) != Include.GlobalUserRID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if store group/attributes is a user attribute
        /// </summary>
        /// <param name="aStoreGroupRID">Store Group Key</param>
        /// <returns></returns>
        public bool IsStoreGroupUser(int aStoreGroupRID)
        {
            if (aStoreGroupRID > 1)
            {
                if (StoreMgmt.StoreGroup_Get(aStoreGroupRID).OwnerUserRID != Include.GlobalUserRID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a hierarchy is a user hierarchy 
        /// </summary>
        /// <param name="aHierarchyRID">Hierarchy key</param>
        /// <returns></returns>
        public bool IsHierarchyUser(int aHierarchyRID)
        {
            if (aHierarchyRID > 1)
            {
                if (SAB.HierarchyServerSession.GetHierarchyOwner(aHierarchyRID) != Include.GlobalUserRID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a node in a hierarchy is from a user hierarchy
        /// </summary>
        /// <param name="aHierarchyNodeRID">Node key</param>
        /// <returns></returns>
        public bool IsHierarchyNodeUser(int aHierarchyNodeRID)
        {
            if (aHierarchyNodeRID > 1)
            {
                if (SAB.HierarchyServerSession.GetNodeOwner(aHierarchyNodeRID) != Include.GlobalUserRID
                    )
                {
                    return true;
                }
            }

            return false;
        }
		// End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

		//========
		// METHODS
		//========
        /// <summary>
        /// Override of the Hash Code generator.
        /// </summary>
        /// <returns>Hash code for this object</returns>
		public override int GetHashCode()
		{
			return this.Key;
		}

		/// <summary>
		/// overrided Equals
		/// </summary>
		/// <param name="obj">MethodGenAllocProfile</param>
		/// <returns>Bool</returns>
		public override bool Equals(Object obj) 
		{
			// Check for null values and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) 
				return false;

			return (this.Key == ((ApplicationBaseMethod)obj).Key);
		} 

		//public int IComparable.CompareTo(object obj)
		public int CompareTo(object obj)
		{ 
			return Key - ((ApplicationBaseMethod)obj).Key; 
		} 
 
		public static bool operator<(ApplicationBaseMethod lhs, ApplicationBaseMethod rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) < 0;
		}

		public static bool operator<=(ApplicationBaseMethod lhs, ApplicationBaseMethod rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) <= 0;
		}

		public static bool operator>(ApplicationBaseMethod lhs, ApplicationBaseMethod rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) > 0;
		}

		public static bool operator>=(ApplicationBaseMethod lhs, ApplicationBaseMethod rhs)
		{ 
			return ((IComparable)lhs).CompareTo(rhs) >= 0;
		}

		public object Clone()
		{
			return this;
		}

		//Begin TT#523 - JScott - Duplicate folder when new folder added
		private void SetMethodProfileType(eProfileType aProfileType)
		{
			_methodProfileType = aProfileType;
		}

		//End TT#523 - JScott - Duplicate folder when new folder added
//		private void Populate(string aMethodName)
//		{
//			// base.Key = something;
//		}
		private void Populate(int aMethodRID)
		{
			MethodBaseData mb = new MethodBaseData();
			_filled = mb.PopulateMethod(base.Key);
			if (_filled)
			{
				// if unknown method type, set base method type once known
				if (Convert.ToInt32(base.MethodType, CultureInfo.CurrentUICulture) == -1)  
				{
					base.SetMethodType(mb.Method_Type_ID);
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					SetMethodProfileType(mb.Profile_Type_ID);
					//End TT#523 - JScott - Duplicate folder when new folder added
				}
				else
				if (mb.Method_Type_ID != base.MethodType)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)(eMIDTextCode.msg_MethodsOutOfSync),
						MIDText.GetText(eMIDTextCode.msg_MethodsOutOfSync));
				}
				
				_method_Name = mb.Method_Name;
				_user_RID = mb.User_RID;
				_method_Description = mb.Method_Description;
				_sg_RID = mb.SG_RID;
				_virtual_IND = Include.ConvertCharToBool(mb.Virtual_IND);
				_methodStatus = mb.Method_Status;
                _customOLL_RID = mb.Custom_OLL_RID;     // MID Track #5530 - add CUSTOM_OLL_RID column
			}
		}

        // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
        internal void DetermineIfContainsUserData()
        {
            if (GlobalUserType == eGlobalUserType.User)
            {
                _containsUserData = CheckForUserData();
            }
            else
            {
                _containsUserData = false;
            }
        }

        abstract internal bool CheckForUserData();
        // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)

        // Begin TT#1966-MD - JSmith- DC Fulfillment
        protected void WriteBeginProcessingMessage()
        {
            string message = MIDText.GetText(eMIDTextCode.msg_BeginProcessing, MethodTypeName, _method_Name);
            _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name, true);
        }

        protected void WriteEndProcessingMessage(ApplicationSessionTransaction aApplicationTransaction)
        {
            string message;
            if ((aApplicationTransaction.AllocationActionAllHeaderStatus == eAllocationActionStatus.NoActionPerformed
                || aApplicationTransaction.AllocationActionAllHeaderStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                && (aApplicationTransaction.OTSPlanActionStatus == 0
                || aApplicationTransaction.OTSPlanActionStatus == eOTSPlanActionStatus.NoActionPerformed
                || aApplicationTransaction.OTSPlanActionStatus == eOTSPlanActionStatus.ActionCompletedSuccessfully)
                )
            {
                message = MIDText.GetText(eMIDTextCode.msg_EndProcessingSuccessfully, MethodTypeName, _method_Name);
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name, true);
            }
            else
            {
                message = MIDText.GetText(eMIDTextCode.msg_EndProcessingFailed, MethodTypeName, _method_Name);
                _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, message, this.GetType().Name, true);
            }
        }

        // End TT#1966-MD - JSmith- DC Fulfillment

		/// <summary>
		/// Creates or updates the base method.
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		virtual public void Update(TransactionData td)
		{
			MethodBaseData mb = new MethodBaseData(td);
			if (base.Key < 0 || _method_Change_Type == eChangeType.add)
			{
				base.Key = Include.NoRID;
			}
			mb.Method_Type_ID = base.MethodType;
			//Begin TT#523 - JScott - Duplicate folder when new folder added
			mb.Profile_Type_ID = MethodProfileType;
			//End TT#523 - JScott - Duplicate folder when new folder added
			mb.Method_RID = base.Key;
			mb.Method_Name = _method_Name ;
			mb.User_RID = _user_RID;
			mb.Method_Description = _method_Description;
			mb.SG_RID = _sg_RID;
			mb.Virtual_IND = Include.ConvertBoolToChar(_virtual_IND);
			mb.Method_Status = _methodStatus;
            mb.Custom_OLL_RID = _customOLL_RID;     // MID Track #5530 - add CUSTOM_OLL_RID column
			try
			{
				if (base.Key < 0)
				{
                    // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                    //base.Key = mb.CreateMethod();
                    base.Key = mb.CreateMethod(SAB.ClientServerSession.UserRID);
                    // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
				}				
				else if
					(_method_Change_Type == eChangeType.delete)
				{
					mb.DeleteMethod(td);
				}
				else
				{
                    // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                    //mb.UpdateMethod();
                    mb.UpdateMethod(td, SAB.ClientServerSession.UserRID);
                    // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
				}

                // Begin TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
                _containsUserData = null;
                // End TT#2080-MD - JSmith - User Method with User Header Filter may be copied to Global Method (user Header Filter is not valid in a Global Method)
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
		/// Returns a copy of this object.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aCloneDateRanges">
		/// A flag identifying if date ranges are to be cloned or use the original</param>
        /// <param name="aCloneCustomOverrideModels">
        /// A flag identifying if custom override models are to be cloned or use the original
        /// </param>
		/// <returns>
		/// A copy of the object.
		/// </returns>
        // Begin Track #5912 - JSmith - Save As needs to clone custom override models
        //abstract public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges);
        abstract public ApplicationBaseMethod Copy(Session aSession, bool aCloneDateRanges, bool aCloneCustomOverrideModels);
        // End Track #5912

		// Begin MID Track 4858 - JSmith - Security changes
		/// <summary>
		/// Returns a flag identifying if the user can update the data on the method.
		/// </summary>
		/// <param name="aSession">
		/// The current session
		/// </param>
		/// <param name="aUserRID">
		/// The internal key of the user</param>
		/// <returns>
		/// A flag.
		/// </returns>
		abstract public bool AuthorizedToUpdate(Session aSession, int aUserRID);
		// End MID Track 4858

        // Begin TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.
        /// <summary>
        /// Returns a flag identifying if the user can view the data on the method.
        /// </summary>
        /// <param name="aSession">
        /// The current session
        /// </param>
        /// <param name="aUserRID">
        /// The internal key of the user</param>
        /// <returns>
        /// A flag.
        /// </returns>
        virtual public bool AuthorizedToView(Session aSession, int aUserRID)
        {
            return true;
        }
        // End TT#3572 - JSmith - Security- Version set to Deny - Ran a velocity method with this version and receive a Null reference error.

        //RO-642 - Generic classes for handling Allocation Methods
        abstract public FunctionSecurityProfile GetFunctionSecurity();

        abstract public ROMethodProperties MethodGetData(bool processingApply);

        abstract public bool MethodSetData(ROMethodProperties methodProperties, bool processingApply);

        abstract public ROMethodProperties MethodCopyData();

        internal static List<KeyValuePair<int, string>> DataTableToKeyValues(DataTable dataTable, string keyColName, string valueColName, bool includeusername = false)
        {
            List<KeyValuePair<int, string>> keyValueList = new List<KeyValuePair<int, string>>();

            foreach (DataRow aRow in dataTable.Rows)
            {
                int key = Convert.ToInt32(aRow[keyColName]);
                string value = Convert.ToString(aRow[valueColName]);
                if (includeusername)
                {
                    value = Adjust_Name(aRow["METHOD_NAME"].ToString(), Convert.ToInt32(aRow["USER_RID"]));
                }
                else
                {
                    value = Convert.ToString(aRow[valueColName]);
                }
                keyValueList.Add(new KeyValuePair<int, string>(key, value));
            }
            return keyValueList;
        }

        internal static DataTable SortDataTable(DataTable dataTable, string sColName, bool bAscending = true)
        {
            DataView dv = dataTable.DefaultView;

            if (bAscending)
            {
                dv.Sort = sColName + " ASC";
            }
            else
            {
                dv.Sort = sColName + " DESC";
            }

            return dv.ToTable();
        }

        internal static string Adjust_Name(string aMethodName, int aUserRID)
        {
            if (aUserRID != Include.GlobalUserRID)
            {
                aMethodName += " (" + UserNameStorage.GetUserName(aUserRID) + ")";
            }
            return aMethodName;
        }
    }
}
