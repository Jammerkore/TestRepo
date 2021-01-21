using System;
using System.Data;
using System.Text;
using System.Globalization;

using MIDRetail.DataCommon;
 
namespace MIDRetail.Data
{
	/// <summary>
	/// Base Method class containing all properties for a Method.
	/// </summary>
	public partial class MethodBaseData: DataLayer
	{
        public MethodBaseData() : base()
        {
            _Method_RID = Include.NoRID;
        }


		private int			_Method_RID;
		private string		_Method_Name;
		private eMethodType	_Method_Type_ID;
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		private eProfileType _Profile_Type_ID;
		//End TT#523 - JScott - Duplicate folder when new folder added
		private int _User_RID;
		private string		_Method_Description;
		private int			_SG_RID;
		private char		_Virtual_IND;
        private char        _template_IND;
        private string		_Method_Comment = null;
		eMethodStatus		_methodStatus;
        private int         _customOLL_RID;     // MID Track #5530 - add CUSTOM_OLL_RID column 
		
		public int Method_RID
		{
			get	{return _Method_RID;}
			set	{_Method_RID = value;	}
		}
		public string Method_Name
		{
			get	{return _Method_Name;}
			set	{_Method_Name = value;	}
		}
		public int User_RID
		{
			get	{return _User_RID;}
			set	{_User_RID = value;	}
		}

		public eMethodType Method_Type_ID
		{
			get	{return _Method_Type_ID;}
			set	{_Method_Type_ID = value;	}
		}
		public int int_Method_Type_ID
		{
			get	{return (int)_Method_Type_ID;}
		}
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		public eProfileType Profile_Type_ID
		{
			get { return _Profile_Type_ID; }
			set { _Profile_Type_ID = value; }
		}
		public int int_Profile_Type_ID
		{
			get { return (int)_Profile_Type_ID; }
		}
		//End TT#523 - JScott - Duplicate folder when new folder added
		public string Method_Description
		{
			get	{return _Method_Description;}
			set	{_Method_Description = value;	}
		}
		public int SG_RID
		{
			get	{return _SG_RID;}
			set	{_SG_RID = value;	}
		}
		public char Virtual_IND
		{
			get	{return _Virtual_IND;}
			set	{_Virtual_IND = value;	}
		}

        public char Template_IND
        {
            get { return _template_IND; }
            set { _template_IND = value; }
        }

        public string Method_Comment
		{
			get	{return _Method_Comment;}
			set	{_Method_Comment = value;	}
		}

		public eMethodStatus Method_Status
		{
			get	{return _methodStatus;}
			set	{_methodStatus = value;	}
		}


        // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column
        public int Custom_OLL_RID                       
        {
            get { return _customOLL_RID; }
            set { _customOLL_RID = value; }
        }
        // END MID Track #5530   
                               
		/// <summary>
		/// Creates an instance of the MethodBase class
		/// </summary>
		/// <param name="td">An instance of the TransactionData class which contains the database connection</param>
		public MethodBaseData(TransactionData td): base(td.DBA)
		{
			_Method_RID = Include.NoRID;
		}

		/// <summary>
		/// Populates the MethodBase class based on PK input param.
		/// </summary>
		/// <param name="Method_RID">PK Method_RID</param>
		/// <returns>Returns boolean value: true = populated</returns>
		public bool PopulateMethod(int method_RID)
		{
			try
			{	
				DataTable dtMethod = MIDEnvironment.CreateDataTable();
                dtMethod = StoredProcedures.MID_METHOD_READ.Read(_dba, METHOD_RID: method_RID);

				if(dtMethod != null && dtMethod.Rows.Count != 0)
				{
					DataRow dr = dtMethod.Rows[0];
					_Method_RID = method_RID;
					_Method_Name = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentUICulture);
					_Method_Type_ID = (eMethodType)Convert.ToInt32(dr["METHOD_TYPE_ID"], CultureInfo.CurrentUICulture);
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					_Profile_Type_ID = (eProfileType)Convert.ToInt32(dr["PROFILE_TYPE_ID"], CultureInfo.CurrentUICulture);
					//End TT#523 - JScott - Duplicate folder when new folder added
					_User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
					_Method_Description = Convert.ToString(dr["METHOD_DESCRIPTION"], CultureInfo.CurrentUICulture);
					if (dr["SG_RID"] == DBNull.Value)
						_SG_RID = Include.NoRID;
					else
						_SG_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
					_Virtual_IND = Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture);
                    _template_IND = Convert.ToChar(dr["TEMPLATE_IND"], CultureInfo.CurrentUICulture);
                    _methodStatus = (eMethodStatus)Convert.ToInt32(dr["METHOD_STATUS"], CultureInfo.CurrentUICulture);
                    
                    // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column
                    if (dr["CUSTOM_OLL_RID"] == DBNull.Value)
                    {
                        _customOLL_RID = Include.NoRID;
                    }
                    else
                    {
                        _customOLL_RID = Convert.ToInt32(dr["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
                    }
                    // END MID Track #5530  

					return true;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Populates the MethodBase class based on the name of the method.
		/// </summary>
		/// <param name="aMethodName">Name of the method</param>
		/// <returns>Returns boolean value: true = populated</returns>
		public bool PopulateMethod(string aMethodName)
		{
			try
			{	
				
				DataTable dtMethod = MIDEnvironment.CreateDataTable();
                dtMethod = StoredProcedures.MID_METHOD_READ_FROM_NAME.Read(_dba, METHOD_NAME: aMethodName);

				if(dtMethod != null && dtMethod.Rows.Count != 0)
				{
					DataRow dr = dtMethod.Rows[0];
					_Method_RID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);;
					_Method_Name = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentUICulture);
					_Method_Type_ID = (eMethodType)Convert.ToInt32(dr["METHOD_TYPE_ID"], CultureInfo.CurrentUICulture);
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					_Profile_Type_ID = (eProfileType)Convert.ToInt32(dr["PROFILE_TYPE_ID"], CultureInfo.CurrentUICulture);
					//End TT#523 - JScott - Duplicate folder when new folder added
					_User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
					_Method_Description = Convert.ToString(dr["METHOD_DESCRIPTION"], CultureInfo.CurrentUICulture);
					if (dr["SG_RID"] == DBNull.Value)
						_SG_RID = Include.NoRID;
					else
						_SG_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
					_Virtual_IND = Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture);
                    _template_IND = Convert.ToChar(dr["TEMPLATE_IND"], CultureInfo.CurrentUICulture);
                    _methodStatus = (eMethodStatus)Convert.ToInt32(dr["METHOD_STATUS"], CultureInfo.CurrentUICulture);

                    // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column
                    if (dr["CUSTOM_OLL_RID"] == DBNull.Value)
                    {
                        _customOLL_RID = Include.NoRID;
                    }
                    else
                    {
                        _customOLL_RID = Convert.ToInt32(dr["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
                    }
                    // END MID Track #5530  

					return true;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Populates the MethodBase class based on the name of the method.
		/// </summary>
		/// <param name="aMethodName">Name of the method</param>
		/// <param name="aMethodType">The type of the method</param>
		/// <returns>Returns boolean value: true = populated</returns>
		public bool PopulateMethod(string aMethodName, eMethodType aMethodType)
		{
			try
			{	
				DataTable dtMethod = MIDEnvironment.CreateDataTable();
                dtMethod = StoredProcedures.MID_METHOD_READ_FROM_NAME_AND_TYPE.Read(_dba,
                                                                                METHOD_NAME: aMethodName,
                                                                                METHOD_TYPE_ID: (int)aMethodType
                                                                                );

				if(dtMethod != null && dtMethod.Rows.Count != 0)
				{
					DataRow dr = dtMethod.Rows[0];
					_Method_RID = Convert.ToInt32(dr["METHOD_RID"], CultureInfo.CurrentUICulture);;
					_Method_Name = Convert.ToString(dr["METHOD_NAME"], CultureInfo.CurrentUICulture);
					_Method_Type_ID = (eMethodType)Convert.ToInt32(dr["METHOD_TYPE_ID"], CultureInfo.CurrentUICulture);
					//Begin TT#523 - JScott - Duplicate folder when new folder added
					_Profile_Type_ID = (eProfileType)Convert.ToInt32(dr["PROFILE_TYPE_ID"], CultureInfo.CurrentUICulture);
					//End TT#523 - JScott - Duplicate folder when new folder added
					_User_RID = Convert.ToInt32(dr["USER_RID"], CultureInfo.CurrentUICulture);
					_Method_Description = Convert.ToString(dr["METHOD_DESCRIPTION"], CultureInfo.CurrentUICulture);
					if (dr["SG_RID"] == DBNull.Value)
						_SG_RID = Include.NoRID;
					else
						_SG_RID = Convert.ToInt32(dr["SG_RID"], CultureInfo.CurrentUICulture);
					_Virtual_IND = Convert.ToChar(dr["VIRTUAL_IND"], CultureInfo.CurrentUICulture);
                    _template_IND = Convert.ToChar(dr["TEMPLATE_IND"], CultureInfo.CurrentUICulture);
                    _methodStatus = (eMethodStatus)Convert.ToInt32(dr["METHOD_STATUS"], CultureInfo.CurrentUICulture);

                    // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column
                    if (dr["CUSTOM_OLL_RID"] == DBNull.Value)
                    {
                        _customOLL_RID = Include.NoRID;
                    }
                    else
                    {
                        _customOLL_RID = Convert.ToInt32(dr["CUSTOM_OLL_RID"], CultureInfo.CurrentUICulture);
                    }
                    // END MID Track #5530  

					return true;
				}
				else
					return false;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Methods by Method ID
		/// </summary>
		/// <param name="MethodTypeID">eMethodType enum</param>
		/// <returns>DataTable</returns>
		public DataTable GetMethods(eMethodType MethodTypeID)
		{
			try
			{	
                return StoredProcedures.MID_METHOD_READ_FROM_TYPE.Read(_dba, METHOD_TYPE_ID: (int)MethodTypeID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Methods by Method ID and User ID
		/// </summary>
		/// <param name="MethodTypeID">eMethodType enu</param>
		/// <param name="UserID">USER_RID</param>
		/// <returns>DataTable</returns>
		public DataTable GetMethods(eMethodType MethodTypeID, int UserID, bool isVirtual)
		{
			try
			{	
                return StoredProcedures.MID_METHOD_READ_FROM_TYPE_AND_USER.Read(_dba,
                                                                                METHOD_TYPE_ID: (int)MethodTypeID,
                                                                                USER_RID: UserID,
                                                                                VIRTUAL_IND: Include.ConvertBoolToChar(isVirtual)
                                                                                );
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#63 - JSmith - Shared folder showing when nothing shared
        /// <summary>
        /// Returns DataTable of all Methods by Method ID and User ID
        /// </summary>
        /// <param name="aUserID">USER_RID</param>
        /// <param name="aIsVirtual">Flag identifying if virtual methods should be included</param>
        /// <returns>DataTable</returns>
        public DataTable GetSharedMethods(int aUserID, bool aIsVirtual)
        {
            try
            {
                return StoredProcedures.MID_METHOD_READ_SHARED.Read(_dba,
                                                                    USER_RID: aUserID,
                                                                    VIRTUAL_IND: Include.ConvertBoolToChar(aIsVirtual)
                                                                    );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Returns DataTable of all Methods by Method ID and User ID
        /// </summary>
        /// <param name="aUserID">USER_RID</param>
        /// <param name="aOwnerUserID">Owner USER_RID</param>
        /// <param name="aIsVirtual">Flag identifying if virtual methods should be included</param>
        /// <returns>DataTable</returns>
        public DataTable GetSharedMethods(int aUserID, int aOwnerUserID, bool aIsVirtual)
        {
            try
            {
                return StoredProcedures.MID_METHOD_READ_SHARED_FROM_OWNER.Read(_dba,
                                                                               USER_RID: aUserID,
                                                                               VIRTUAL_IND: Include.ConvertBoolToChar(aIsVirtual),
                                                                               OWNER_USER_RID: aOwnerUserID
                                                                               );
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#63

      

		/// <summary>
		/// Returns DataTable of all Methods per USER_RID or GLOBAL_USER_IND
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetMethods(int UserID)
		{
			try
			{	
                return StoredProcedures.MID_METHOD_READ_FROM_USER_OR_GLOBAL_USER.Read(_dba, USER_RID: UserID);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Methods
		/// </summary>
		/// <returns>DataTable</returns>
		public DataTable GetMethods()
		{
			try
			{	
                return StoredProcedures.MID_METHOD_READ_ALL.Read(_dba);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns DataTable of all Methods by Method ID and User ID
		/// </summary>
		/// <param name="MethodTypeID">eMethodType enu</param>
		/// <param name="UserID">USER_RID</param>
		/// <returns>DataTable</returns>
		public eMethodType GetMethodType(int MethodRid)
		{
			try
			{	
				eMethodType aMethodType = eMethodType.NotSpecified; 

                object obj = StoredProcedures.MID_METHOD_READ_TYPE_ID.ReadValue(_dba, METHOD_RID: MethodRid);
				if (obj != DBNull.Value)
					aMethodType = (eMethodType)Convert.ToInt32(obj, CultureInfo.CurrentUICulture);

				return aMethodType;
				
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

     
		/// <summary>
		/// Insert a new row in the Method table
		/// </summary>
		/// <param name="method_Name"></param>
		/// <param name="method_Type_ID"></param>
		/// <param name="user_RID"></param>
		/// <param name="method_Description"></param>
		/// <param name="sg_RID"></param>
		/// <returns>
		/// return eGenericDBError.DuplicateKey if dupe Alt. Key 
		/// otherwise returns new Method_RID
		/// </returns>
        // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column & METHOD_STATUS - unsure if this is ever invoked
		//Begin TT#523 - JScott - Duplicate folder when new folder added
		//public int InsertMethod(string method_Name, eMethodType method_Type_ID,
		// Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //public int InsertMethod(string method_Name, eMethodType method_Type_ID, eProfileType profile_Type_ID,
        ////End TT#523 - JScott - Duplicate folder when new folder added
        //    int user_RID, string method_Description, int sg_RID, char virtual_IND, int methodStatus, int customOLL_RID,
        //    // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //    DatabaseAccess dba)
        public int InsertMethod(string method_Name, eMethodType method_Type_ID, eProfileType profile_Type_ID,
            int user_RID, string method_Description, int sg_RID, char virtual_IND, int methodStatus, int customOLL_RID,
            DatabaseAccess dba, int aUpdateUserRID, char template_IND)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			int method_RID = -1;

			try
			{
				//if(GenericRowExists("METHOD", BuildMethodAK(method_Name, method_Type_ID,
				//	user_RID, method_RID)))
				//{
				//	return (int)eGenericDBError.DuplicateKey;
				//}

				object oSg_rid = null;
				if (sg_RID != Include.NoRID)
					oSg_rid = sg_RID;
                
                // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column (& METHOD_STATUS update)
                object oLL_RID = null;
                if (Custom_OLL_RID != Include.NoRID)
                {
                    oLL_RID = Custom_OLL_RID;
                }
                // END MID Track #5530
                
                method_RID = StoredProcedures.SP_MID_METHOD_INSERT.InsertAndReturnRID(_dba,
                                                                                      METHOD_NAME: method_Name,
                                                                                      METHOD_TYPE_ID: (int)method_Type_ID,
                                                                                      PROFILE_TYPE_ID: (int)profile_Type_ID,
                                                                                      USER_RID: user_RID,
                                                                                      METHOD_DESCRIPTION: method_Description,
                                                                                      SG_RID: Include.ConvertObjectToNullableInt(oSg_rid),
                                                                                      VIRTUAL_IND: virtual_IND,
                                                                                      TEMPLATE_IND: template_IND,
                                                                                      METHOD_STATUS: (int)Method_Status,
                                                                                      CUSTOM_OLL_RID: Include.ConvertObjectToNullableInt(oLL_RID)
                                                                                      );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //AddChangeHistory(dba, method_RID, "Created");
                AddChangeHistory(dba, method_RID, "Created", aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(user_RID, int_Profile_Type_ID, method_RID, user_RID);
                }

                ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin TT#523 - JScott - Duplicate folder when new folder added
                //    ////Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    ////sa.AddUserItem(user_RID, (int)eSharedDataType.Method, method_RID, user_RID);
                //    //sa.AddUserItem(user_RID, (int)eProfileType.Method, method_RID, user_RID);
                //    ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.AddUserItem(user_RID, int_Profile_Type_ID, method_RID, user_RID);
                //    //End TT#523 - JScott - Duplicate folder when new folder added
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                ////End Track #4815
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				return method_RID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


        public bool GenericRowExists(string method_Name, eMethodType method_Type_ID,
            int user_RID, int method_RID)
		{
            //MID Track # 2354 - removed nolock because it causes concurrency issues
            int recordCount = StoredProcedures.MID_METHOD_READ_COUNT_FOR_WORKFLOW.ReadRecordCount(_dba,
                                                                                                    METHOD_NAME: method_Name, //method_Name.Replace("'", "''"),
                                                                                                    METHOD_TYPE_ID: (int)method_Type_ID,
                                                                                                    USER_RID: user_RID,
                                                                                                    METHOD_RID: method_RID
                                                                                                    );
				
			if (recordCount == 0)
				return false;

			return true;
		}


        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //public int CreateMethod()
        public int CreateMethod(int aUpdateUserRID)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			int method_RID = Include.NoRID;

			try
			{
				//if(GenericRowExists("METHOD", BuildMethodAK(Method_Name, Method_Type_ID,
				//	User_RID,method_RID)))
				//{
				//	return (int)eGenericDBError.DuplicateKey;
				//}

				object oSg_rid = null;
                if (SG_RID != Include.NoRID && SG_RID != 0)     // TT#1689-MD - stodd - DC Carton Rounding Default Attribute should be blank if not selected 
					oSg_rid = SG_RID;
               
                // BEGIN MID Track #5530 - add CUSTOM_OLL_RID column (& METHOD_STATUS update)
                object oLL_RID = null;
                if (Custom_OLL_RID != Include.NoRID)
                {
                    oLL_RID = Custom_OLL_RID;
                }
                // END MID Track #5530

                method_RID = StoredProcedures.SP_MID_METHOD_INSERT.InsertAndReturnRID(_dba,
                                                                                 METHOD_NAME: Method_Name,
                                                                                 METHOD_TYPE_ID: int_Method_Type_ID,
                                                                                 PROFILE_TYPE_ID: int_Profile_Type_ID,
                                                                                 USER_RID: User_RID,
                                                                                 METHOD_DESCRIPTION: Method_Description,
                                                                                 SG_RID: Include.ConvertObjectToNullableInt(oSg_rid),
                                                                                 VIRTUAL_IND: Virtual_IND,
                                                                                 TEMPLATE_IND: Template_IND,
                                                                                 METHOD_STATUS: (int)Method_Status,
                                                                                 CUSTOM_OLL_RID: Include.ConvertObjectToNullableInt(oLL_RID)
                                                                                 );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //AddChangeHistory(_dba, method_RID, "Created");
                AddChangeHistory(_dba, method_RID, "Created", aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
//				_dba.CloseUpdateConnection();

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(User_RID, int_Profile_Type_ID, method_RID, User_RID);
                }

                ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin TT#523 - JScott - Duplicate folder when new folder added
                //    ////Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    ////sa.AddUserItem(User_RID, (int)eSharedDataType.Method, method_RID, User_RID);
                //    //sa.AddUserItem(User_RID, (int)eProfileType.Method, method_RID, User_RID);
                //    ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.AddUserItem(User_RID, int_Profile_Type_ID, method_RID, User_RID);
                //    //End TT#523 - JScott - Duplicate folder when new folder added

                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                ////End Track #4815

                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				return method_RID;
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public bool DeleteMethod(TransactionData td)
		{
			bool DeleteSuccessfull = true;
			try
			{
                StoredProcedures.SP_MID_METHOD_DELETE.Delete(td.DBA, METHOD_RID: _Method_RID);
                // End Track #5908
                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.DeleteUserItemByTypeAndRID(int_Profile_Type_ID, _Method_RID);
                }

                ////Begin Track #4815 - JSmith - #283-User (Security) Maintenance
                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin TT#523 - JScott - Duplicate folder when new folder added
                //    ////Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    ////sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.Method, _Method_RID);
                //    //sa.DeleteUserItemByTypeAndRID((int)eProfileType.Method, _Method_RID);
                //    ////End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.DeleteUserItemByTypeAndRID(int_Profile_Type_ID, _Method_RID);
                //    //End TT#523 - JScott - Duplicate folder when new folder added

                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                ////End Track #4815
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				DeleteSuccessfull = true;
			}
			catch 
			{
				DeleteSuccessfull = false;
				throw;
			}
			finally
			{
			}
			return DeleteSuccessfull;
		}

        // Begin Track #6302 - JSmith - Assign user permanently to another, folders do not come over
        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
            //public bool UpdateMethod()
        public bool UpdateMethod(int aUpdateUserRID)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
		{
			bool UpdateSuccessfull = true;
			try
			{
              
                int? SG_RID_nullable = null;
                if (_SG_RID != Include.NoRID) SG_RID_nullable = _SG_RID;
                int? CUSTOM_OLL_RID_nullable = null;
                if (_customOLL_RID != Include.NoRID) CUSTOM_OLL_RID_nullable = _customOLL_RID;
                StoredProcedures.MID_METHOD_UPDATE.Update(DBA,
                                                          METHOD_RID: _Method_RID,
                                                          METHOD_NAME: _Method_Name,
                                                          METHOD_TYPE_ID: (int)_Method_Type_ID,
                                                          PROFILE_TYPE_ID: (int)_Profile_Type_ID,
                                                          USER_RID: (int)_User_RID,
                                                          METHOD_DESCRIPTION: _Method_Description,
                                                          SG_RID: SG_RID_nullable,
                                                          VIRTUAL_IND: _Virtual_IND,
                                                          TEMPLATE_IND: _template_IND,
                                                          METHOD_STATUS: (int)_methodStatus,
                                                          CUSTOM_OLL_RID: CUSTOM_OLL_RID_nullable
                                                          );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                //AddChangeHistory(DBA, _Method_RID, _Method_Comment);
                AddChangeHistory(DBA, _Method_RID, _Method_Comment, aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine

				UpdateSuccessfull = true;
			}
			catch
			{
				UpdateSuccessfull = false;
				throw;
            }
			return UpdateSuccessfull;
		}
        // End Track #6302

        // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        //public bool UpdateMethod(TransactionData td)
        public bool UpdateMethod(TransactionData td, int aUpdateUserRID)
        // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
        {
            bool UpdateSuccessfull = true;
            try
            {
                int? SG_RID_nullable = null;
                if (_SG_RID != Include.NoRID) SG_RID_nullable = _SG_RID;
                int? CUSTOM_OLL_RID_nullable = null;
                if (_customOLL_RID != Include.NoRID) CUSTOM_OLL_RID_nullable = _customOLL_RID;
                StoredProcedures.MID_METHOD_UPDATE.Update(td.DBA,
                                                          METHOD_RID: _Method_RID,
                                                          METHOD_NAME: _Method_Name,
                                                          METHOD_TYPE_ID: (int)_Method_Type_ID,
                                                          PROFILE_TYPE_ID: (int)_Profile_Type_ID,
                                                          USER_RID: (int)_User_RID,
                                                          METHOD_DESCRIPTION: _Method_Description,
                                                          SG_RID: SG_RID_nullable,
                                                          VIRTUAL_IND: _Virtual_IND,
                                                          TEMPLATE_IND: _template_IND,
                                                          METHOD_STATUS: (int)_methodStatus,
                                                          CUSTOM_OLL_RID: CUSTOM_OLL_RID_nullable
                                                          );
                // Begin TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                AddChangeHistory(td.DBA, _Method_RID, _Method_Comment, aUpdateUserRID, EnvironmentInfo.MIDInfo.userName, EnvironmentInfo.MIDInfo.machineName, EnvironmentInfo.MIDInfo.remoteMachineName);
                // End TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine

                UpdateSuccessfull = true;
            }
            catch
            {
                UpdateSuccessfull = false;
                throw;
            }
            return UpdateSuccessfull;
        }

        public int Method_GetKey(int aUserRID, string aMethodName)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_METHOD_READ_FROM_NAME_AND_USER.Read(_dba,
                                                                                METHOD_NAME: aMethodName,
                                                                                USER_RID: aUserRID
				                                                                );

                if (dt.Rows.Count == 1)
                {
                    return (Convert.ToInt32(dt.Rows[0]["METHOD_RID"], CultureInfo.CurrentUICulture));
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		
		/// <summary>
		/// Updates Custom OLL rid behind the scenes for methods
		/// </summary>
		/// <param name="methodRid"></param>
		/// <param name="customOLLRid"></param>
		public void UpdateMethodCustomOLLRid(TransactionData td, int methodRid, int customOLLRid)
		{
			try
			{
                int? CUSTOM_OLL_RID_nullable = null;
                if (customOLLRid != Include.NoRID) CUSTOM_OLL_RID_nullable = customOLLRid;
                StoredProcedures.MID_METHOD_UPDATE_CUSTOM_OLL.Update(td.DBA,
                                                                     METHOD_RID: methodRid,
                                                                     CUSTOM_OLL_RID: CUSTOM_OLL_RID_nullable
                                                                     );
			}
			catch (Exception ex)
			{
                string s = ex.ToString();
				throw;
			}
		}




        private void AddChangeHistory(DatabaseAccess aDBA, int aMethodRID, string aComment, int aUpdateUserRID, string windowsUser, string windowsMachine, string windowsRemoteMachine) //TT#1511-MD -jsobek  -Data Layer Request - Changes for Method and Workflow change history
		{
			try
			{
                StoredProcedures.MID_METHOD_CHANGE_HISTORY_INSERT.Insert(aDBA,
                                                                         METHOD_RID: aMethodRID,
                                                                         USER_RID: (int)aUpdateUserRID,   // TT#1510-MD - JSmith - Correct Method and Workflow Change History and Add Fields for Windows User and Machine
                                                                         CHANGE_DATE: DateTime.Now,
                                                                         METHOD_COMMENT: aComment,
                                                                         WINDOWS_USER: windowsUser,
                                                                         WINDOWS_MACHINE: windowsMachine,
                                                                         WINDOWS_REMOTE_MACHINE: windowsRemoteMachine
                                                                         );
			}
			catch
			{
				throw;
			}
		}
		//Begin TT#523 - JScott - Duplicate folder when new folder added



	}			
} 
 