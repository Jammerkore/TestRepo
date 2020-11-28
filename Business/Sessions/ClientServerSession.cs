using System;
using System.Data;
using System.Diagnostics;
using System.Collections;
using System.Globalization;

using MIDRetail.DataCommon;
using MIDRetail.Data;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;

namespace MIDRetail.Business
{
	/// <summary>
	/// ClientServerGlobal is a static class that contains fields that are global to all ClientServerSession objects.
	/// </summary>
	/// <remarks>
	/// The ClientServerGlobal class is used to store information that is global to all ClientServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>

	public class ClientServerGlobal : Global
	{
		//=======
		// FIELDS
		//=======

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private ArrayList _loadLock;
		static private bool _loaded;

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ClientServerGlobal
		/// </summary>

		static ClientServerGlobal()
		{
			//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			try
			{
				_loadLock = new ArrayList();
				_loaded = false;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
			//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static public bool Loaded
		{
			get
			{
				return _loaded;
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		//========
		// METHODS
		//========

		/// <summary>
		/// The Load method is called by the service or client to trigger the instantiation of the static ClientServerGlobal
		/// object
		/// </summary>

		static public void Load()
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				lock (_loadLock.SyncRoot)
				{
					if (!_loaded)
					{
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                        //Calendar = new MRSCalendar();
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        // Begin TT#195 MD - JSmith - Add environment authentication
                        // use unknown since is being started for all processes
                        //LoadEnvironmentInformation(eProcesses.unknown);
                        //RegisterServiceStart();
                        // End TT#195 MD

                        LoadBase(eMIDMessageSenderRecepient.clientApplication, 30, true, eProcesses.clientApplication);

						_loaded = true;
					}
				}
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			}
			catch
			{
				throw;
			}		
		}
		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

		/// <summary>
		/// Cleans up all resources for the service
		/// </summary>

		static public void CleanUp()
		{
            // Begin TT#2307 - JSmith - Incorrect Stock Values
            if (isExecutingLocal &&
                MessageProcessor.isListeningForMessages)
            {
                StopMessageListener();
            }
            // End TT#2307
		}
		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
	}

	/// <summary>
	/// ClientServerSession is a class that contains fields, properties, and methods that are available to other sessions
	/// of the system.
	/// </summary>
	/// <remarks>
	/// The ClientServerSession class is the interface to the ClientServer functionality.  All requests for functionality
	/// or information in the ClientServer should be made through methods and properties in this class.
	/// </remarks>

	//Begin TT#708 - JScott - Services need a Retry availalbe.
	//public class ClientServerSession : Session
	public class ClientServerSessionRemote : SessionRemote
	//End TT#708 - JScott - Services need a Retry availalbe.
	{
		//=======
		// FIELDS
		//=======

		private int _userRID = -1;
        private string _userID = null;
        private string _userName = string.Empty;
        private string _userFullName = string.Empty;
        private string _userDescription = string.Empty;
        private bool _userIsActive = false;
        private bool _userIsSetToBeDeleted = false;
        private DateTime _userDateTimeWhenDeleted;
        private eProcesses _process = eProcesses.unknown;
//		private string _myHierarchyName = null;
//		private string _myHierarchyColor = null;
//		private string _myWorkflowMethods = null;
//		private int _themeRID = Include.Undefined;
		private Theme _theme;
		private Security _security = null;
		private Hashtable _functionSecurityTable;
		private Hashtable _nodeSecurityTable;
		private ArrayList _nodeSecurityList;
//		private GlobalOptionsProfile _globalOptions = null;
		private SelectedHeaderList _selectedHeader;
		private ArrayList _selectedComponent;
		private int _threadID = -1;
		private System.Collections.Hashtable _profileHash;
//		private VersionProfileList _versionProfileList;
		private UserOptionsProfile _userOptions = null;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private Hashtable _userNameHash;
		//Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        private InfragisticsLayoutData layoutData;
        private bool _layoutDelete = false;
		//End  TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
		//End Track #4815

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ClientSessionGlobal as either local or remote, depending on the value of aLocal
		/// </summary>
		/// <param name="aLocal">
		/// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
		/// </param>

		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public ClientServerSession(bool aLocal, int aThreadID)
		public ClientServerSessionRemote(bool aLocal, int aThreadID)
		//End TT#708 - JScott - Services need a Retry availalbe.
			: base(aLocal)
		{
			try
			{
				_security = new Security();
				_functionSecurityTable = new Hashtable();
				_nodeSecurityTable = new Hashtable();
				_selectedHeader =  new SelectedHeaderList(eProfileType.SelectedHeader);
				_selectedComponent =  new ArrayList();
				_profileHash = new System.Collections.Hashtable();
                _threadID = aThreadID;
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				_userNameHash = new System.Collections.Hashtable();
				//End Track #4815
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the RID of the user
		/// </summary>

		public int UserRID
		{
			get
			{
				if (_userRID == -1)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_UserRIDNotInitialized,
						MIDText.GetText(eMIDTextCode.msg_UserRIDNotInitialized));
				}
				return _userRID;
			}
		}

        /// <summary>
        /// Gets the ID of the user
        /// </summary>

        public string UserID
        {
            get
            { 
                return _userID;
            }
        }

        /// <summary>
        /// Gets the name of the user
        /// </summary>

        public string UserName
        {
            get
            {
                return _userName;
            }
        }

        /// <summary>
        /// Gets the full name of the user
        /// </summary>

        public string UserFullName
        {
            get
            {
                return _userFullName;
            }
        }

        /// <summary>
        /// Gets the description of the user
        /// </summary>

        public string UserDescription
        {
            get
            {
                return _userDescription;
            }
        }

        /// <summary>
        /// Gets the flag identifying if the user is active
        /// </summary>

        public bool UserIsActive
        {
            get
            {
                return _userIsActive;
            }
        }

        /// <summary>
        /// Gets the flag identifying if the user is scheduled to be deleted
        /// </summary>

        public bool UserIsSetToBeDeleted
        {
            get
            {
                return _userIsSetToBeDeleted;
            }
        }

        /// <summary>
        /// Gets the date and time when the user was scheduled to be deleted
        /// </summary>

        public DateTime UserDateTimeWhenDeleted
        {
            get
            {
                return _userDateTimeWhenDeleted;
            }
        }

        /// <summary>
        /// Gets and sets the process associated with the client.
        /// </summary>

        public eProcesses Process
		{
			get
			{
				return _process;
			}
			set
			{
				_process = value;
			}
		}

		/// <summary>
		/// Gets the reader lock timeout setting from global.
		/// </summary>

		public int ReaderLockTimeOut
		{
			get
			{
				return ClientServerGlobal.ReaderLockTimeOut;
			}
		}

		/// <summary>
		/// Gets the writer lock timeout setting from global.
		/// </summary>

		public int WriterLockTimeOut
		{
			get
			{
				return ClientServerGlobal.WriterLockTimeOut;
			}
		}

		/// <summary>
		/// Gets the name associated with the user's My Hierarchies
		/// </summary>

		public string MyHierarchyName
		{
			get
			{
				if (UserOptions.MyHierarchyName == null)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_HierNameNotInitialized,
						MIDText.GetText(eMIDTextCode.msg_HierNameNotInitialized));
				}
				return UserOptions.MyHierarchyName;
			}
		}

		/// <summary>
		/// Gets the color associated with the user's My Hierarchies
		/// </summary>

		public string MyHierarchyColor
		{
			get
			{
				if (UserOptions.MyHierarchyColor == null)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_HierColorNotInitialized,
						MIDText.GetText(eMIDTextCode.msg_HierColorNotInitialized));
				}
				return UserOptions.MyHierarchyColor;
			}
		}

		/// <summary>
		/// Gets the name associated with the user's My Workflow/Mehtods folder text
		/// </summary>

		public string MyWorkflowMethods
		{
			get
			{
				if (UserOptions.MyWorkflowMethods == null)
				{
					throw new MIDException (eErrorLevel.severe,
						(int)eMIDTextCode.msg_WrkflwMthdsNotInitialized,
						MIDText.GetText(eMIDTextCode.msg_WrkflwMthdsNotInitialized));
				}
				return UserOptions.MyWorkflowMethods;
			}
		}

		/// <summary>
		/// Gets or sets the current Theme
		/// </summary>

		public Theme Theme
		{
			get
			{
				DataTable dt;
				ThemeData themeData;
	
				try
				{
					if (_theme == null)
					{
						themeData = new ThemeData();

						if (UserOptions.ThemeRID == Include.Undefined)
						{
							GetDefaultTheme(themeData);
						}
						else
						{
							dt = themeData.Theme_ReadByTheme(UserOptions.ThemeRID);

							switch (dt.Rows.Count)
							{
								case 0:
									GetDefaultTheme(themeData);
									break;
								case 1:
									_theme = new Theme(dt, 0);
									break;
								default:
									throw new Exception("Invalid # of theme records returned");
//									break;
							}
						}
					}

					return _theme;
				}
				catch (Exception err)
				{
					string message = err.ToString();
					throw;
				}
			}
			set
			{
				ThemeData themeData;
				
				try
				{
					themeData = new ThemeData();

					_theme.CopyFrom(value);
					themeData.OpenUpdateConnection();

					try
					{
						themeData.Theme_Update(UserOptions.ThemeRID, _theme);
						themeData.CommitData();
					}
					catch (Exception err)
					{
						themeData.Rollback();
						string message = err.ToString();
						throw;
					}
					finally
					{
						themeData.CloseUpdateConnection();
					}
				}
				catch (Exception err)
				{
					string message = err.ToString();
					throw;
				}
			}
		}

//		/// <summary>
//		/// Gets global options values.
//		/// </summary>
//
//		public GlobalOptionsProfile GlobalOptions
//		{
//			get 
//			{
//				try
//				{
//					if (_globalOptions == null)
//					{
//						_globalOptions = new GlobalOptionsProfile(-1);
//						_globalOptions.LoadOptions();
//					}
//
//					return _globalOptions;
//				}
//				catch (Exception err)
//				{
//					string message = err.ToString();
//					throw;
//				}
//			}
//		}

				/// <summary>
				/// Gets global options values.
				/// </summary>
		
		public UserOptionsProfile UserOptions
		{
			get 
			{
				try
				{
					if (_userOptions == null)
					{
						_userOptions = new UserOptionsProfile(UserRID);
						_userOptions.LoadOptions();
					}
		
					return _userOptions;
				}
				catch (Exception err)
				{
					string message = err.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the thread ID of the client.
		/// </summary>

		public int ThreadID
		{
			get 
			{ 
				return _threadID;  
			}
		}

		//========
		// METHODS
		//========

		/// <summary>
		/// Initializes the session.
		/// </summary>

		public override void Initialize()
		{
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			ScheduleData dlSchedule;
			int processId;

			try
			{
				dlSchedule = new ScheduleData();
				dlSchedule.OpenUpdateConnection();

				try
				{
					processId = dlSchedule.GetNextScheduleProcessId();
					dlSchedule.CommitData();
				}
				catch (Exception err)
				{
					dlSchedule.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					dlSchedule.CloseUpdateConnection();
				}

                // Begin TT#739-MD - JSmith - Delete Stores
                // Uncomment if using SQLBulkCopy
                //MIDConnectionString.ThreadID = SessionAddressBlock.ControlServerSession.ThreadID;
                //Header headerData = new Header();
                //headerData.CreateWorkTables();
                // End TT#739-MD - JSmith - Delete Stores
				Initialize(processId);
                // Begin TT#1808-MD - JSmith - Store Load Error
                ExceptionHandler.Initialize(SessionAddressBlock.ClientServerSession, false);
                // End TT#1808-MD - JSmith - Store Load Error
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public void Initialize(int aProcessId)
		{
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			try
			{
				base.Initialize();
				// BEGIN 4320 - stodd 2.15.2007
				Calendar = ClientServerGlobal.Calendar;
				// END 4320 - stodd 2.15.2007
                // Begin TT#46 MD - JSmith - User Dashboard 
				//CreateAudit(_process);
				//CreateAudit(_process, true);	// Moved to UserLogin() TT#1581-MD - stodd Header Reconcile
                // End TT#46 MD
//				Calendar.SetPostingDate(SessionAddressBlock.HierarchyServerSession.GetPostingDate());
				DateTime postingDate = DateTime.Now;
				MerchandiseHierarchyData mhd = new MerchandiseHierarchyData();
				DataTable dt = mhd.PostingDate_Read();
				if (dt.Rows.Count == 1)
				{
					DataRow dr = dt.Rows[0];
					if (dr["POSTING_DATE"] != DBNull.Value)
					{
						postingDate = Convert.ToDateTime(dr["POSTING_DATE"], CultureInfo.CurrentUICulture);
					}
				}
				Calendar.SetPostingDate(postingDate);

				// should really be process ID
//				_threadID = System.Threading.Thread.CurrentThread.GetHashCode();
//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//				Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
//				Audit.LoggingLevel = UserOptions.AuditLoggingLevel;
//				_threadID = currentProcess.Id;
				Audit.LoggingLevel = UserOptions.AuditLoggingLevel;
				_threadID = aProcessId;
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#739-MD - JSmith - Delete Stores
        /// <summary>
		/// Closes the session.
		/// </summary>

        public override void CloseSession()
        {
            // Uncomment if using SQLBulkCopy
            //Header headerData = new Header();
            //headerData.DropWorkTables();
            base.CloseSession();
        }
        // End TT#739-MD - JSmith - Delete Stores

//Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
//		/// <summary>
//		/// Initializes the session.
//		/// </summary>
//		
//		public void Initialize(int aProcessRID)
//		{
//			try
//			{
//				base.Initialize();
//				CreateAudit(aProcessRID);
//			}
//			catch (Exception err)
//			{
//				string message = err.ToString();
//				throw;
//			}
//		}
//
//End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                ClientServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

		/// <summary>
		/// Clean up the global resources
		/// </summary>
		public void CleanUpGlobal()
		{
			ClientServerGlobal.CleanUp();
		}

		/// <summary>
		/// Clears all cached areas in the session.
		/// </summary>

		public void Refresh()
		{
			try
			{
				if (_functionSecurityTable != null)
				{
					_functionSecurityTable.Clear();
                    // Begin TT#1434 - JSmith - Slow Header Selection + Memory Issue?
                    _functionSecurityTable = null;
                    _functionSecurityTable = new Hashtable();
                    // End TT#1434
				}
				if (_nodeSecurityTable != null)
				{
					_nodeSecurityTable.Clear();
                    // Begin TT#1434 - JSmith - Slow Header Selection + Memory Issue?
                    _nodeSecurityTable = null;
                    _nodeSecurityTable = new Hashtable();
                    // End TT#1434
				}
				if (_nodeSecurityList != null)
				{
					_nodeSecurityList.Clear();
                    // Begin TT#1434 - JSmith - Slow Header Selection + Memory Issue?
                    _nodeSecurityList = null;
                    _nodeSecurityList = new ArrayList();
                    // End TT#1434
				}

				RefreshBase();
				_profileHash.Clear();
                // Begin TT#1434 - JSmith - Slow Header Selection + Memory Issue?
                _profileHash = null;
                _profileHash = new Hashtable();
                // End TT#1434
				// get new security object to reset security cache
				_security = new Security();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears all cached areas in the session related to security.
		/// </summary>

		public void RefreshSecurity()
		{
			try
			{
				if (_functionSecurityTable != null)
				{
					_functionSecurityTable.Clear();
				}
				if (_nodeSecurityTable != null)
				{
					_nodeSecurityTable.Clear();
				}
				if (_nodeSecurityList != null)
				{
					_nodeSecurityList.Clear();
				}

				// get new security object to reset security cache
				_security = new Security();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Clears all cached areas in the session relating to the hierarchy.
		/// </summary>

		public void RefreshHierarchy()
		{
			try
			{
				if (_nodeSecurityTable != null)
				{
					_nodeSecurityTable.Clear();
				}
				if (_nodeSecurityList != null)
				{
					_nodeSecurityList.Clear();
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Creates and returns a new Transaction object.
		/// </summary>
		/// <returns>
		/// The newly created Transaction object that points to this Session.
		/// </returns>

		public ClientSessionTransaction CreateTransaction()
		{
			try
			{
				return new ClientSessionTransaction(SessionAddressBlock);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		// BEGIN Issue 4858 stodd 10.30.2007 forecast Methods Security
		/// <summary>
		/// This method will retrieve the current ProfileList stored in this session.  If the ProfileList has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <param name="aProfileType">
		/// The eProfileType of the ProfileList to retieve.
		/// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>

		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			ProfileList profileList;
			VersionProfile versionProf;
			ForecastVersion versionDL;
			DataTable dtVersions;

			profileList = (ProfileList)_profileHash[aProfileType];

			if (profileList == null)
			{
				switch (aProfileType)
				{
					case eProfileType.Version:

						versionDL = new ForecastVersion();
						dtVersions = versionDL.GetForecastVersions();
						profileList = new ProfileList(eProfileType.Version);
						
						foreach (DataRow row in dtVersions.Rows)
						{
							ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
							versionProf = fvpb.Build(Convert.ToInt32(row["FV_RID"]));
							versionProf.ChainSecurity = _security.GetUserVersionAssignment(UserRID, versionProf.Key, (int)eSecurityTypes.Chain);
							versionProf.StoreSecurity = _security.GetUserVersionAssignment(UserRID, versionProf.Key, (int)eSecurityTypes.Store);
							if (!versionProf.ChainSecurity.AccessDenied ||
								!versionProf.StoreSecurity.AccessDenied)
							{
								profileList.Add(versionProf);
							}
						}

						_profileHash.Add(profileList.ProfileType, profileList);

						break;
				}
			}
			return profileList;
		}
		// END Issue 4858 stodd 10.30.2007 forecast Methods Security

		/// <summary>
		/// Logs in a user based upon aUserName and aPassword
		/// </summary>
		/// <param name="aUserName">
		/// String containing the user's UserName
		/// </param>
		/// <param name="aPassword">
		/// String containing the Password for the user
		/// </param>
		public eSecurityAuthenticate UserLogin(string aUserName, string aPassword, eProcesses aProcess)
		{
			try
			{
				// Begin TT#1581-MD - stodd Header Reconcile
                //=========================
                // Authenticate User
                //=========================
				eSecurityAuthenticate retVal = _security.AuthenticateUser(aUserName, aPassword);

                if (retVal == eSecurityAuthenticate.UserAuthenticated)
                {
                    _userRID = _security.UserRID;
                    _userID = aUserName;
                    _process = aProcess;
                    //GetUserOptions(_userRID);

                    retVal = GetProcessingPermission(aProcess, retVal); // TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created

                    MIDRetail.Data.SecurityAdmin secAdmin = new MIDRetail.Data.SecurityAdmin();
                    System.Data.DataTable dt = secAdmin.GetUser(_userRID);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0].IsNull("USER_NAME") == false)
                        {
                            _userName = (string)dt.Rows[0]["USER_NAME"];
                        }
                        if (dt.Rows[0].IsNull("USER_FULLNAME") == false)
                        {
                            _userFullName = (string)dt.Rows[0]["USER_FULLNAME"];
                        }
                        if (dt.Rows[0].IsNull("USER_DESCRIPTION") == false)
                        {
                            _userDescription = (string)dt.Rows[0]["USER_DESCRIPTION"];
                        }
                        if (dt.Rows[0].IsNull("USER_ACTIVE_IND") == false)
                        {
                            _userIsActive = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["USER_ACTIVE_IND"]));
                        }
                        if (dt.Rows[0].IsNull("USER_DELETE_IND") == false)
                        {
                            _userIsSetToBeDeleted = Include.ConvertCharToBool(Convert.ToChar(dt.Rows[0]["USER_DELETE_IND"]));
                        }
                        if (dt.Rows[0].IsNull("USER_DELETE_DATETIME") == false)
                        {
                            _userDateTimeWhenDeleted = (DateTime)dt.Rows[0]["USER_DELETE_DATETIME"];
                        }

                    }
                }
                // End TT#1581-MD - stodd Header Reconcile

				return retVal;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created
        private eSecurityAuthenticate GetProcessingPermission(eProcesses aProcess, eSecurityAuthenticate retVal)
        {
            CreateAudit(_process, true);

            //================================
            //Process Control
            //================================
            if (aProcess != eProcesses.clientApplication)
            {
                string reasonMsg = string.Empty;
                //SessionAddressBlock.ControlServerSession.CurrentProcess = aProcess;
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                //base.SessionAddressBlock.ControlServerSession.CurrentProcessID = currentProcess.Id;
                //if (!SessionAddressBlock.ControlServerSession.GetProcessingPermission(eProcesses.HeaderReconcile, currentProcess.Id, ref reasonMsg))
               
                //if (!ControlServerGlobal.GetProcessingPermission(aProcess, currentProcess.Id, ref reasonMsg))   // TT#1611-MD - stodd - Error running task List

                // Begin TT#1654-MD - stodd - The Control Service no long seems to recognize Running API processes
                if (SessionAddressBlock == null)
                {
                    if (Audit != null)
                    {
                        Audit.Add_Msg(eMIDMessageLevel.Error, "SessionAddressBlock is NULL in ClientServerSessionRemote:GetProcessingPermission()", "Control Service");
                    }
                }
                // if (!ControlServerGlobal.GetProcessingPermission(aProcess, currentProcess.Id, ref reasonMsg))   // TT#1611-MD - stodd - Error running task List
                if (!SessionAddressBlock.ControlServerSession.GetProcessingPermission(aProcess, currentProcess.Id, ref reasonMsg))  
                {
                // End TT#1654-MD - stodd - The Control Service no long seems to recognize Running API processes
                    if (Audit != null)
                    {
                        Audit.Add_Msg(eMIDMessageLevel.ProcessUnavailable, "Processing terminated due to permission to process was denied.", "Control Service");
                        Audit.Add_Msg(eMIDMessageLevel.ProcessUnavailable, reasonMsg, "Control Service");
                    }
                    retVal = eSecurityAuthenticate.Unavailable;
                }
            }
            return retVal;
        }
        // End TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created

		/// <summary>
		/// Logs in a user based upon aUserName and aPassword
		/// </summary>
		/// <param name="aUserRID">
		/// The record ID of the user
		/// </param>
		/// <param name="aProcess">
		/// The process for which the user is to be authenticated
		/// </param>

		public eSecurityAuthenticate UserLogin(int aUserRID, eProcesses aProcess)
		{
			try
			{
                eSecurityAuthenticate retVal = eSecurityAuthenticate.ActiveUser;    // TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created

				_userRID = aUserRID;
				_process = aProcess;
//				GetUserOptions(_userRID);

                retVal = GetProcessingPermission(aProcess, retVal);     // TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created

                return retVal;      // TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a User's name
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the user to get the name of
		/// </param>
		/// <returns>
		/// A string containing the User's name
		/// </returns>

		public string GetUserName(int userRID)
		{
			try
			{
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//				return _security.GetUserName(userRID);
				string userName;
				if (_userNameHash.ContainsKey(userRID))
				{
					userName = (string)_userNameHash[userRID];
				}
				else
				{
					userName = _security.GetUserName(userRID);
					_userNameHash.Add(userRID, userName);
				}
				return userName;
				//End Track #4815
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}



		/// <summary>
		/// Used to retrieve the selected header list
		/// </summary>
		/// <returns>
		/// A ProfileList of selected headers
		/// </returns>

		public ProfileList GetSelectedHeaderList()
		{
			return _selectedHeader;
		}

		/// <summary>
		/// Used to clear the list of selected headers
		/// </summary>
		
		public void ClearSelectedHeaderList()
		{
			try
			{
				_selectedHeader.Clear();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Used to add a header to the selected header list
		/// </summary>
		/// <param name="aHeaderRID">
		/// The record ID of the header
		/// </param>
		/// <param name="aHeaderID">
		/// The ID of the header
		/// </param>

		public void AddSelectedHeaderList(int aHeaderRID, string aHeaderID, eHeaderType aHeaderType, int aAsrtRID, int aStyleHnRID)
		{
			try
			{
				if (!_selectedHeader.Contains(aHeaderRID))
				{
					SelectedHeaderProfile shp = new SelectedHeaderProfile(aHeaderRID);
					shp.HeaderID = aHeaderID;
					shp.HeaderType = aHeaderType;
                    shp.AsrtRID = aAsrtRID;
                    shp.StyleHnRID = aStyleHnRID;
					_selectedHeader.Add(shp);
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> temporary
        public void AddSelectedHeaderList(int aHeaderRID, string aHeaderID, eHeaderType aHeaderType, int aAsrtRID, int aStyleHnRID, bool bBypassEnqueue)
        {
            try
            {
                if (!_selectedHeader.Contains(aHeaderRID))
                {
                    SelectedHeaderProfile shp = new SelectedHeaderProfile(aHeaderRID);
                    shp.HeaderID = aHeaderID;
                    shp.HeaderType = aHeaderType;
                    shp.AsrtRID = aAsrtRID;
                    shp.StyleHnRID = aStyleHnRID;
                    //shp.BypassEnqueue = bBypassEnqueue;	// TT#1154-MD - stodd - null reference when opening selection - 
                    _selectedHeader.Add(shp);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#1442  

		/// <summary>
		/// Used to retrieve the selected header list
		/// </summary>
		/// <returns>
		/// A ArrayList of selected GeneralComponents
		/// </returns>

		public ArrayList GetSelectedComponentList()
		{
			return _selectedComponent;
		}


		/// <summary>
		/// Used to clear the list of selected components
		/// </summary>
		public void ClearSelectedComponentList()
		{
			try
			{
				_selectedComponent.Clear();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		// Begin issue 4108
		/// <summary>
		/// Used to add a component wrapper to the selected component list
		/// </summary>
		/// <param name="aComponentWrapper"></param>
		public void AddSelectedComponentList(GeneralComponentWrapper aComponentWrapper)
		{
			try
			{
				_selectedComponent.Add(aComponentWrapper);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}
		// end issue 4108

		// BEGIN Issue 4858 stodd 10.30.2007 forecast Methods Security
		//===========================================================================
		// if ePlanBasisType.Basis, return all where access is NOT denied.
		// if ePlanBasisType.Basis, return only those that have at least Maintain.
		//===========================================================================
		/// <summary>
		/// Used to retrieve the list of version profiles for which the user has authority. Defaults to Basis/Store.
		/// </summary>
		/// <returns>
		/// A ProfileList of VersionProfile objects.
		/// </returns>

		public ProfileList GetUserForecastVersions()
		{
			// Begin Issue 5871 stodd 8.29.2008
			// Begin Issue 5807 stodd 8.7.2008
			// This not the final fix for this. It gives a blended list of versions, which is better than 
			// the Basis, store list it was returning previously. 
			ProfileList versionProfList = new ProfileList(eProfileType.Version);
			// Begin Track #5882 stodd
			ProfileList chainVersionProfList = GetUserForecastVersions(eSecuritySelectType.Update | eSecuritySelectType.View, eSecurityTypes.Chain);
			//ProfileList chainBasisVersionProfList = GetUserForecastVersions(ePlanBasisType.Basis, eSecurityTypes.Chain);
			//ProfileList storePlanVersionProfList = GetUserForecastVersions(ePlanBasisType.Plan, eSecurityTypes.Store);
			ProfileList storeVersionProfList = GetUserForecastVersions(eSecuritySelectType.Update | eSecuritySelectType.View, eSecurityTypes.Store);
			// End Track #5882 stodd

			foreach (VersionProfile versionProfile in chainVersionProfList.ArrayList)
			{
				if (!versionProfList.Contains(versionProfile.Key))
					versionProfList.Add(versionProfile);
			}
			foreach (VersionProfile versionProfile in storeVersionProfList.ArrayList)
			{
				if (!versionProfList.Contains(versionProfile.Key))
					versionProfList.Add(versionProfile);
			}
			//foreach (VersionProfile versionProfile in storePlanVersionProfList.ArrayList)
			//{
			//    if (!versionProfList.Contains(versionProfile.Key))
			//        versionProfList.Add(versionProfile);
			//}
			//foreach (VersionProfile versionProfile in chainPlanVersionProfList.ArrayList)
			//{
			//    if (!versionProfList.Contains(versionProfile.Key))
			//        versionProfList.Add(versionProfile);
			//}
			return versionProfList;
			// END Issue 5807
			// END Issue 5871
		}

		//Begin Track #5871 stodd
        //Begin Track #5858 - JSmith - Validating store security only
        //public ProfileList GetUserForecastVersions(ePlanBasisType planBasisType, eSecurityTypes chainOrStore)
        //{
        //    return GetUserForecastVersions(planBasisType, chainOrStore, Include.NoRID);
        //}

		//public ProfileList GetUserForecastVersions(ePlanBasisType planBasisType, eSecurityTypes chainOrStore, bool aIncludeAllowUpdateOnly)
		//{
		//    return GetUserForecastVersions(planBasisType, chainOrStore, Include.NoRID, aIncludeAllowUpdateOnly);
		//}

		public ProfileList GetUserForecastVersions(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore)
        {
			return GetUserForecastVersions(securitySelectType, chainOrStore, Include.NoRID);
        }
        //End Track #5858
		//End Track #5871

		/// <summary>
		/// Used to retrieve the list of version profiles for which the user has authority;
		/// </summary>
		/// <param name="versionRid">
		/// If this version RID is not = -1, it will force it in to the version list.
		/// </param>
		/// <returns>
		/// A ProfileList of VersionProfile objects.
		/// </returns>

		// BEGIN Track #5871
        //Begin Track #5858 - JSmith - Validating store security only
		//public ProfileList GetUserForecastVersions(eSecuritySelectType selectType, eSecurityTypes chainOrStore, int versionRid)
		//{
		//    return GetUserForecastVersions(planBasisType, chainOrStore, versionRid, false);
		//}

        //public ProfileList GetUserForecastVersions(ePlanBasisType planBasisType, eSecurityTypes chainOrStore, int versionRid)
		public ProfileList GetUserForecastVersions(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore, int versionRid)
        //End Track #5858
		{
            //Begin Track #5858 - JSmith - Validating store security only
            bool addVersion;
            //End Track #5858
			ProfileList versionProfList = GetProfileList(eProfileType.Version);
			ProfileList forecastVersionProfList = new ProfileList(eProfileType.Version);
		
			// BEGIN Track #5871 stodd
			foreach (VersionProfile versionProf in versionProfList.ArrayList)
			{
				addVersion = false;
				if (System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Chain))
				{
					if (!versionProf.ChainSecurity.AccessDenied)
					{
						// Begin Track #5882 stodd
						////======================================================================
						//// When planning, don't include it if it's Actual (if this is not the administrator) or
						//// if it's Blended AND the forecast version isn't equal to itself.
						////======================================================================
						//if (versionProf.Key == Include.FV_ActualRID && UserRID != Include.AdministratorUserRID)
						//{
						//    // do not include this version.
						//}
						//else if (versionProf.IsBlendedVersion && versionProf.ForecastVersionRID != versionProf.Key)
						//{
						//    // do not include this version.
						//}
						//else
						//{
						// Begin Track #5882
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.View))
						{
							if (versionProf.ChainSecurity.AllowView)
							{
								addVersion = true;
							}
						}
						// End Track #5882
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.Update))
						{
							if (versionProf.ChainSecurity.AllowUpdate)
							{
								addVersion = true;
							}
						}
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.Delete))
						{
							if (versionProf.ChainSecurity.AllowDelete)
							{
								addVersion = true;
							}
						}
						//}
						// End Track #5882 stodd

					}
				}

				if (System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Store))
				{
					if (!versionProf.StoreSecurity.AccessDenied)
					{
						// Begin Track #5882 stodd
						////======================================================================
						//// When planning, don't include it if it's Actual (if this is not the administrator) or
						//// if it's Blended AND the forecast version isn't equal to itself.
						////======================================================================
						//if (versionProf.Key == Include.FV_ActualRID && UserRID != Include.AdministratorUserRID)
						//{
						//    // do not include this version.
						//}
						//else if (versionProf.IsBlendedVersion && versionProf.ForecastVersionRID != versionProf.Key)
						//{
						//    // do not include this version.
						//}
						//else
						//{
						// Begin Track #5882 stodd
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.View))
						{
							if (versionProf.StoreSecurity.AllowView)
							{
								addVersion = true;
							}
						}
						// End track #5882
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.Update))
						{
							if (versionProf.StoreSecurity.AllowUpdate)
							{
								addVersion = true;
							}
						}
						if (System.Convert.ToBoolean(securitySelectType & eSecuritySelectType.Delete))
						{
							if (versionProf.StoreSecurity.AllowDelete)
							{
								addVersion = true;
							}
						}
						//}
						// End track #5882 stodd
					}
				}
				// END Track #5871 stodd

				//addVersion = false;
				//if (planBasisType == ePlanBasisType.Basis)
				//{
				//    if (System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Chain))
				//    {
				//        if (!versionProf.ChainSecurity.AccessDenied)
				//        {
				//            if (aIncludeAllowUpdateOnly)
				//            {
				//                if (versionProf.ChainSecurity.AllowUpdate)
				//                {
				//                    addVersion = true;
				//                }
				//            }
				//            else
				//            {
				//                addVersion = true;
				//            }
				//        }
				//    }

				//    if (!addVersion && System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Store))
				//    {
				//        if (!versionProf.StoreSecurity.AccessDenied)
				//        {
				//            if (aIncludeAllowUpdateOnly)
				//            {
				//                if (versionProf.StoreSecurity.AllowUpdate)
				//                {
				//                    addVersion = true;
				//                }
				//            }
				//            else
				//            {
				//                addVersion = true;
				//            }
				//        }
				//    }
				//}

				//if (!addVersion && planBasisType == ePlanBasisType.Plan)
				//{
				//    if (System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Chain))
				//    {
				//        if (!versionProf.ChainSecurity.AccessDenied)
				//        {
				//            if (!versionProf.ChainSecurity.IsReadOnly)
				//            {
				//                //======================================================================
				//                // For a plan, don't include it if it's Actual or
				//                // if it's Blended AND the forecast version isn't equal to itself.
				//                //======================================================================
				//                if (versionProf.Key == Include.FV_ActualRID ||
				//                    (versionProf.IsBlendedVersion && versionProf.ForecastVersionRID != versionProf.Key))
				//                {
				//                    // do not include this version.
				//                }
				//                else
				//                {
				//                    addVersion = true;
				//                }
				//            }
				//        }
				//    }

				//    if (!addVersion && planBasisType == ePlanBasisType.Plan && System.Convert.ToBoolean(chainOrStore & eSecurityTypes.Store))
				//    {
				//        if (!versionProf.StoreSecurity.AccessDenied)
				//        {
				//            if (!versionProf.StoreSecurity.IsReadOnly)
				//            {
				//                //======================================================================
				//                // For a plan, don't include it if it's Actual or
				//                // if it's Blended AND the forecast version isn't equal to itself.
				//                //======================================================================
				//                if (versionProf.Key == Include.FV_ActualRID ||
				//                    (versionProf.IsBlendedVersion && versionProf.ForecastVersionRID != versionProf.Key))
				//                {
				//                    // do not include this version.
				//                }
				//                else
				//                {
				//                    addVersion = true;
				//                }
				//            }
				//        }
				//    }
				//}

                if (versionRid != Include.NoRID && versionRid == versionProf.Key)
                {
                    addVersion = true;
                }

                if (addVersion)
                {
                    forecastVersionProfList.Add(versionProf);
                }
			
			}
            //End Track #5858
			//End Track #5871

            // Begin Track #5926 - JSmith - Save As when no security
            // make sure version is in list
            if (versionRid != Include.NoRID &&
                !forecastVersionProfList.Contains(versionRid))
            {
                ForecastVersionProfileBuilder fvpb = new ForecastVersionProfileBuilder();
                VersionProfile versionProf = fvpb.Build(versionRid);
                versionProf.ChainSecurity = _security.GetUserVersionAssignment(UserRID, versionProf.Key, (int)eSecurityTypes.Chain);
                versionProf.StoreSecurity = _security.GetUserVersionAssignment(UserRID, versionProf.Key, (int)eSecurityTypes.Store);
                forecastVersionProfList.Add(versionProf);
            }
            // End Track #5926

			return forecastVersionProfList;
		}
		// BEGIN Issue 4858

		/// <summary>
		/// Sets a User's password
		/// </summary>
		/// <param name="userName">
		/// The User's name to set the password for
		/// </param>
		/// <param name="oldPassword">
		/// The User's old password
		/// </param>
		/// <param name="newPassword">
		/// The User's new password
		/// </param>
		/// <returns>
		/// A eSecurityAuthenticate value indicating the result of the call
		/// </returns>

		public eSecurityAuthenticate SetUserPassword(string userName, string oldPassword, string newPassword )
		{
			try
			{
				return _security.SetUserPassword(userName, oldPassword, newPassword);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a User's node security assignment list
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the User
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A HierarchyNodeSecurityProfileList of node security assignments
		/// </returns>

		public HierarchyNodeSecurityProfileList GetUserNodesSecurityAssignment(int userRID, int aSecurityType)
		{
			try
			{
				return _security.GetUserNodesAssignment(userRID, aSecurityType);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		
		/// <summary>
		/// Returns a User's node security assignment list
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the User
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// An ArrayList of node security assignments
		/// </returns>

		public HierarchyNodeSecurityProfileList GetUserNodesSecurityAssignment(int userRID, eSecurityActions aActionID, int aSecurityType)
		{
			try
			{
				return _security.GetUserNodesAssignment(userRID, aSecurityType);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a FunctionSecurityProfile object containing a User's security access for the actions of a given eSecurityFunctions
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the User
		/// </param>
		/// <param name="funcID">
		/// The eSecurityFunctions value
		/// </param>
		/// <returns>
		/// A FunctionSecurityProfile containing the security level of each funciton for the User
		/// </returns>

		public FunctionSecurityProfile GetUserFunctionSecurityAssignment(int userRID, eSecurityFunctions funcID)
		{
			try
			{
                //Begin TT#827-MD -JSmith -Allocation Reviews Performance
                if (userRID == UserRID)
                {
                    return GetMyUserFunctionSecurityAssignment(funcID);
                }
                //End TT#827-MD -JSmith -Allocation Reviews Performance

				return _security.GetUserFunctionAssignment(userRID, funcID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a HierarchyNodeSecurityProfile object containing a User's security access for the actions of a given node
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the User
		/// </param>
		/// <param name="nodeRID">
		/// The node record ID
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A HierarchyNodeSecurityProfile containing the security level of each action for the User
		/// </returns>

		public HierarchyNodeSecurityProfile GetUserNodeSecurityAssignment(int userRID, int nodeRID, int aSecurityType)
		{
			HierarchyNodeSecurityProfile securityProfile;

			try
			{
				Hashtable nodeSecurityHash;
				// check for security type in node cache.  if not found create 
				if (!_nodeSecurityTable.Contains(aSecurityType))
				{
					nodeSecurityHash = new Hashtable();
					_nodeSecurityTable.Add(aSecurityType, nodeSecurityHash);
				}
				else
				{
					nodeSecurityHash = (Hashtable)_nodeSecurityTable[aSecurityType];
				}

				// check for node in security type
				if (!nodeSecurityHash.Contains(nodeRID))
				{
					// override the security to full access if personal node
					if (userRID == SessionAddressBlock.HierarchyServerSession.GetNodeOwner(nodeRID))
					{
						securityProfile = new HierarchyNodeSecurityProfile(nodeRID);
						securityProfile.SetFullControl();
					}
					else
					{
						securityProfile = _security.GetUserNodeAssignment(SessionAddressBlock, userRID, nodeRID, aSecurityType);
					}

					nodeSecurityHash.Add(nodeRID, securityProfile);
				}

				
				return (HierarchyNodeSecurityProfile)((HierarchyNodeSecurityProfile)nodeSecurityHash[nodeRID]).Clone();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a FunctionSecurityProfile object containing a User's security access to a given node and eSecurityFunctions
		/// </summary>
		/// <param name="userRID">
		/// The user RID of the User
		/// </param>
		/// <param name="nodeID">
		/// The node ID
		/// </param>
		/// <param name="funcID">
		/// The eSecurityFunctions value
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A FunctionSecurityProfile object containing the security level for the User
		/// </returns>

		public FunctionSecurityProfile GetUserNodeFunctionSecurityAssignment(int userRID, int nodeID, eSecurityFunctions funcID, int aSecurityType)
		{
			HierarchyNodeSecurityProfile nodeSecurity;
			FunctionSecurityProfile functionSecurity;

			try
			{
				// override the security to full access if personal node
				if (userRID == SessionAddressBlock.HierarchyServerSession.GetNodeOwner(nodeID))
				{
					nodeSecurity = new HierarchyNodeSecurityProfile(nodeID);
					nodeSecurity.SetFullControl();
					functionSecurity = new FunctionSecurityProfile(Convert.ToInt32(funcID, CultureInfo.CurrentCulture));
					functionSecurity.SetFullControl();
				}
				else
				{
					nodeSecurity = GetUserNodeSecurityAssignment(userRID, nodeID, aSecurityType);
					functionSecurity = GetUserFunctionSecurityAssignment(userRID, funcID);
				}
			
				return DetermineNodeFunctionSecurityAssignment(nodeSecurity, functionSecurity);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		public FunctionSecurityProfile GetMyUserActionSecurityAssignment(eAllocationActionType aAction, bool aInterfacedHeader)
		{
			try
			{
				eSecurityFunctions funcID;
				switch (aAction)
				{
					case eAllocationActionType.StyleNeed:
						funcID = eSecurityFunctions.AllocationActionsNeed;
						break;
					case eAllocationActionType.BreakoutSizesAsReceived:
                        // Begin TT#5294 - Size Proportional Action Displays and Security is Set to Deny
						//funcID = eSecurityFunctions.AllocationActions;
                        funcID = eSecurityFunctions.AllocationActionsBalSizeProportional;
                        // End TT#5294 - Size Proportional Action Displays and Security is Set to Deny
						break;
					case eAllocationActionType.BalanceStyleProportional:
						funcID = eSecurityFunctions.AllocationActionsBalProportional;
						break;
					case eAllocationActionType.BalanceToDC:
						funcID = eSecurityFunctions.AllocationActionsBalProportional;
						break;
					case eAllocationActionType.BalanceSizeWithSubs:
						funcID = eSecurityFunctions.AllocationActionsBalSizeProportional;
						break;
					case eAllocationActionType.BalanceSizeNoSubs:
                        // Begin Development TT#22 - JSmith- Security -> Allocation Actions->Balance Size to Reserve
						//funcID = eSecurityFunctions.AllocationActionsBalSizeProportional;
                        funcID = eSecurityFunctions.AllocationActionsBalSizeNoSubs;
                        // End Development TT#22
						break;
					case eAllocationActionType.ChargeIntransit:
						funcID = eSecurityFunctions.AllocationActionsChargeInTransit;
						break;
					case eAllocationActionType.Release:
						funcID = eSecurityFunctions.AllocationActionsRelease;
						break;
					case eAllocationActionType.Reset:
						funcID = eSecurityFunctions.AllocationActionsReset;
						break;
					case eAllocationActionType.BackoutSizeIntransit:
						funcID = eSecurityFunctions.AllocationActionsCancelSizeInTransit;
						break;
					case eAllocationActionType.BackoutStyleIntransit:
						funcID = eSecurityFunctions.AllocationActionsCancelInTransit;
						break;
					case eAllocationActionType.BackoutSizeAllocation:
						funcID = eSecurityFunctions.AllocationActionsCancelSizeAllocation;
						break;
					case eAllocationActionType.BackoutAllocation:
						funcID = eSecurityFunctions.AllocationActionsCancelAllocation;
						break;
					case eAllocationActionType.DeleteHeader:
						if (aInterfacedHeader)
						{
							FunctionSecurityProfile securityProfile = GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersInterfaced);
							if (securityProfile.AllowUpdate &&
								GlobalOptions.ProtectInterfacedHeadersInd)
							{
								securityProfile.SetSecurity(eSecurityActions.Maintain, eSecurityLevel.Deny);
							}
							return securityProfile;
						}
						else
						{
							return GetMyUserFunctionSecurityAssignment(eSecurityFunctions.AllocationHeadersNonInterfaced);
						}
//						break;
					case eAllocationActionType.ApplyAPI_Workflow:
						funcID = eSecurityFunctions.AllocationActionsApplyAPIWorkflow;
						break;
					case eAllocationActionType.RemoveAPI_Workflow:
						funcID = eSecurityFunctions.AllocationActionsCancelAPIWorkflow;
						break;
                    // Begin TT#785 - Header Load Interfacing a transaction  trying to Modify a WUB header with a PO type
                    case eAllocationActionType.ReapplyTotalAllocation:
                        funcID = eSecurityFunctions.AllocationActionsReapplyTotalAllocation;
                        break;
                    // End TT#785
                        // begin TT#843 - New Size Contraints Balance
                    case eAllocationActionType.BalanceSizeWithConstraints:
                        funcID = eSecurityFunctions.AllocationActionsBalSizeWithConstraints;
                        break;
                        // end TT#843 - New Size Constraints Balance
                    // begin TT#794 - New Size Balance for Wet Seal
                    case eAllocationActionType.BalanceSizeBilaterally:
                        funcID = eSecurityFunctions.AllocationActionBalSizeBilaterally;
                        break;
                        // end TT#794 - New Size Balance for Wet Seal
                    // begin TT#1391 - JEllis - Balance Size With Constraint Other Options
                    case eAllocationActionType.BreakoutSizesAsReceivedWithConstraints:
                        funcID = eSecurityFunctions.BreakoutSizesAsReceivedWithContraints;
                        break;
                        // end TT#1391 - JEllis - Balance Size With Constraint Other Options
                    // begin TT#1334-MD - stodd - Balance to VSW Action
                    case eAllocationActionType.BalanceToVSW:
                        funcID = eSecurityFunctions.AllocationActionsBalToVSW;
                        break;
                    // end TT#1334-MD - stodd - Balance to VSW Action
                    default:
						funcID = eSecurityFunctions.AllocationActions;
						break;
				}

				return GetMyUserFunctionSecurityAssignment(funcID);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#2 - RMatelic - Assortment Planning
        public FunctionSecurityProfile GetMyUserActionSecurityAssignment(eAssortmentActionType aAction)
        {
            try
            {
                eSecurityFunctions funcID;
                switch (aAction)
                {
                    case eAssortmentActionType.Redo:
                        funcID = eSecurityFunctions.AssortmentActionsRedo;
                        break;
                    case eAssortmentActionType.CancelAssortment:
                        funcID = eSecurityFunctions.AssortmentActionsCancelAssortment;
                        break;
                    case eAssortmentActionType.SpreadAverage:
                        funcID = eSecurityFunctions.AssortmentActionsSpreadAverage;
                        break;
                    case eAssortmentActionType.CreatePlaceholders:
                        funcID = eSecurityFunctions.AssortmentActionsCreatePlaceholders;
                        break;
                    case eAssortmentActionType.BalanceAssortment:
                        funcID = eSecurityFunctions.AssortmentActionsBalanceAssortment;
                        break;
                    case eAssortmentActionType.ChargeCommitted:
                        funcID = eSecurityFunctions.AssortmentActionsChargeCommitted;
                        break;
                    case eAssortmentActionType.CancelCommitted:
                        funcID = eSecurityFunctions.AssortmentActionsCancelCommitted;
                        break;
					// Begin TT#1225 - stodd
					//case eAssortmentActionType.ChargeIntransit:
					//    funcID = eSecurityFunctions.AssortmentActionsChargeIntransit;
					//    break;
					//case eAssortmentActionType.CancelIntransit:
					//    funcID = eSecurityFunctions.AssortmentActionsCancelIntransit;
					//    break;
					// End TT#1225 - stodd
                    default:
                        funcID = eSecurityFunctions.AssortmentActions;
                        break;
                }

                return GetMyUserFunctionSecurityAssignment(funcID);
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2

		public FunctionSecurityProfile GetMyUserFunctionSecurityAssignment(eSecurityFunctions funcID)
		{
			try
			{
				if (!_functionSecurityTable.Contains(funcID))
				{
					_functionSecurityTable.Add(funcID, _security.GetUserFunctionAssignment(_userRID, funcID));
				}

				return (FunctionSecurityProfile)((FunctionSecurityProfile)_functionSecurityTable[funcID]).Clone();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines if the user has access to the nodes in the list
		/// </summary>
		/// <param name="aNodes">
		/// An ArrayList containing the record IDs for which you want to determine the security.
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// An ArrayList containing the nodes to which the user has security 
		/// </returns>

		public ArrayList GetMyUserNodes(ArrayList aNodes, int aSecurityType)
		{
			try
			{
				ArrayList nodeSecurity = new ArrayList();
				
				foreach (int nodeRID in aNodes)
				{
					if (GetMyUserNodeSecurityAssignment(nodeRID, aSecurityType).AllowUpdate ||
						GetMyUserNodeSecurityAssignment(nodeRID, aSecurityType).AllowView)
					{
						nodeSecurity.Add(nodeRID);
					}
				}

				return nodeSecurity;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Determines the security for a list of nodes
		/// </summary>
		/// <param name="aNodes">
		/// An ArrayList containing the record IDs for which you want to determine the security.
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// An Hashtable containing the record ID as key with the eSecurityLevel value for the user/node 
		/// </returns>

		public Hashtable GetMyUserNodeSecurityAssignments(ArrayList aNodes, int aSecurityType)
		{
			try
			{
				Hashtable nodeSecurity = new Hashtable();
				foreach (int nodeRID in aNodes)
				{
					nodeSecurity.Add(nodeRID, GetMyUserNodeSecurityAssignment(nodeRID, aSecurityType));
				}

				return nodeSecurity;
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a HierarchyNodeSecurityProfile object containing the current User's security access to a given node
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A HierarchyNodeSecurityProfile object containing the security level for the User
		/// </returns>

		public HierarchyNodeSecurityProfile GetMyUserNodeSecurityAssignment(int nodeRID, int aSecurityType)
		{
			try
			{
				Hashtable nodeSecurityHash;
				// check for security type in node cache.  if not found create 
				if (!_nodeSecurityTable.Contains(aSecurityType))
				{
					nodeSecurityHash = new Hashtable();
					_nodeSecurityTable.Add(aSecurityType, nodeSecurityHash);
				}
				else
				{
					nodeSecurityHash = (Hashtable)_nodeSecurityTable[aSecurityType];
				}

				// check for node in security type
				if (!nodeSecurityHash.Contains(nodeRID))
				{
					HierarchyNodeSecurityProfile nodeSecurity;
					// override the security to full access if personal node
					if (_userRID == SessionAddressBlock.HierarchyServerSession.GetNodeOwner(nodeRID))
					{
						nodeSecurity = new HierarchyNodeSecurityProfile(nodeRID);
						nodeSecurity.SetFullControl();
					}
					else
					{
						nodeSecurity = _security.GetUserNodeAssignment(SessionAddressBlock, _userRID, nodeRID, aSecurityType);
					}
					nodeSecurityHash.Add(nodeRID, nodeSecurity);
				}

				return (HierarchyNodeSecurityProfile)((HierarchyNodeSecurityProfile)nodeSecurityHash[nodeRID]).Clone();
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a FunctionSecurityProfile object value containing the current User's security access to a given node and eSecurityFunctions
		/// </summary>
		/// <param name="nodeRID">
		/// The record ID of the node
		/// </param>
		/// <param name="funcID">
		/// The eSecurityFunctions value
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A FunctionSecurityProfile object containing the security level for the User
		/// </returns>

		public FunctionSecurityProfile GetMyUserNodeFunctionSecurityAssignment(int nodeRID, eSecurityFunctions funcID, int aSecurityType)
		{
			HierarchyNodeSecurityProfile nodeSecurity;
			FunctionSecurityProfile functionSecurity;

			try
			{
//				// override the security to full access if personal node
//				if (_userRID == SessionAddressBlock.HierarchyServerSession.GetNodeOwner(nodeRID))
//				{
//					nodeSecurity = new HierarchyNodeSecurityProfile(nodeRID);
//					nodeSecurity.SetFullControl();
//					functionSecurity = new FunctionSecurityProfile(Convert.ToInt32(funcID, CultureInfo.CurrentCulture));
//					functionSecurity.SetFullControl();
//				}
//				else
//				{
					nodeSecurity = GetMyUserNodeSecurityAssignment(nodeRID, aSecurityType);
					functionSecurity = GetMyUserFunctionSecurityAssignment(funcID);
//				}
			
				return DetermineNodeFunctionSecurityAssignment(nodeSecurity, functionSecurity);
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Returns a VersionSecurityProfile object containing the current User's security access to a given version
		/// </summary>
		/// <param name="aVersionRID">
		/// The record ID of the version
		/// </param>
		/// <param name="aSecurityType">
		/// Integer that contains bit flags indicating which types of security to check.  The eSecurityType enumerator contains
		/// the valid values for this parameter, and may be or'd together to request multiple types of security.
		/// </param>
		/// <returns>
		/// A VersionSecurityProfile object containing the security level for the User
		/// </returns>

		public VersionSecurityProfile GetMyVersionSecurityAssignment(int aVersionRID, int aSecurityType)
		{
			try
			{
				VersionProfile versionProf = (VersionProfile)GetProfileList(eProfileType.Version).FindKey(aVersionRID);

				if (versionProf == null)
				{
					return new VersionSecurityProfile(aVersionRID);
				}
				else if (aSecurityType == (int)eSecurityTypes.Chain)
				{
					return versionProf.ChainSecurity;
				}
				else
				{
					return versionProf.StoreSecurity;
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Saves the MyHierarchy information to the database.
		/// </summary>
		/// <param name="myHierarchyName">
		/// The Hierarchy Name to save
		/// </param>
		/// <param name="myHierarchyColor">
		/// The Hierarchy Color to save
		/// </param>

		public void UpdateMyHierarchy(string myHierarchyName, string myHierarchyColor)
		{
			try
			{
				UserOptions.UpdateMyHierarchy(myHierarchyName, myHierarchyColor);
//				// default parameters if not provided
//				if (myHierarchyName == null)
//				{
//					myHierarchyName = _myHierarchyName;
//				}
//				if (myHierarchyColor == null)
//				{
//					myHierarchyColor = _myHierarchyColor;
//				}
//
//				SecurityAdmin securityAdmin = new SecurityAdmin();
//				securityAdmin.OpenUpdateConnection();
//
//				try
//				{
//					securityAdmin.UpdateMyHierarchy(_userRID, myHierarchyName, myHierarchyColor);
//					securityAdmin.CommitData();
//
//					_myHierarchyName = myHierarchyName;
//					_myHierarchyColor = myHierarchyColor;
//				}
//				catch (Exception err)
//				{
//					securityAdmin.Rollback();
//					string message = err.ToString();
//					throw;
//				}
//				finally
//				{
//					securityAdmin.CloseUpdateConnection();
//				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		public void UpdateUserOptions()
		{
			try
			{
				SecurityAdmin securityAdmin = new SecurityAdmin();
				securityAdmin.OpenUpdateConnection();
				
				try
				{
					securityAdmin.UpdateUserOptions(_userRID, UserOptions.MyHierarchyName, UserOptions.MyHierarchyColor,
						UserOptions.ForecastMonitorIsActive, UserOptions.ForecastMonitorDirectory,
						UserOptions.ModifySalesMonitorIsActive, UserOptions.ModifySalesMonitorDirectory,
						UserOptions.AuditLoggingLevel, UserOptions.ShowLogin, UserOptions.ShowSignOffPrompt,
                        UserOptions.DCFulfillmentMonitorIsActive, UserOptions.DCFulfillmentMonitorDirectory);     // TT#4243- Add UserOptions.ShowSignOffPrompt
					
					securityAdmin.CommitData();
					Audit.LoggingLevel = UserOptions.AuditLoggingLevel;
                    // Begin Track #5755 - JSmith - Windows login changes
                    //MIDUserInfo userInfo = new MIDUserInfo();
                    //userInfo.ShowLogin = UserOptions.ShowLogin;
                    //userInfo.WriteUserInfo();
                    // End Track #5755
				}
				catch (Exception err)
				{
					securityAdmin.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					securityAdmin.CloseUpdateConnection();
				}
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

		/// <summary>
		/// Refreshes the Global's calendar.
		/// </summary>
		/// <remarks>
		/// Since the calendar is static, it may have already been refreshed.
		/// That's why we check the refresh date.
		/// </remarks>
		/// <param name="refreshDate">
		/// The refresh Date to update the calendar to.
		/// </param>
		
		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				if (refreshDate != ClientServerGlobal.CalendarRefreshDate)
				{
					ClientServerGlobal.Calendar.Refresh();
					ClientServerGlobal.CalendarRefreshDate = refreshDate;
				}

				// Refresh the Calendar of THIS session
				Calendar.Refresh();
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


//		/// <summary>
//		/// Sets the global options pointer to null so the next time it is accessed, it will repull the latest version of Global Options
//		/// </summary>
//
//		public void RefreshGlobalOptions()
//		{
//			this._globalOptions = null;
//		}

		/// <summary>
		/// Insert/Update user's MyWorkflowMethods folder text
		/// </summary>
		/// <param name="myWorkflowMethods">
		/// New My Workflow/Methods text
		/// </param>

		public void UpdateMyWorkflowMethodsText(string myWorkflowMethods)
		{
			UserOptions.UpdateMyWorkflowMethodsText(myWorkflowMethods);
//			try
//			{
//				SecurityAdmin securityAdmin = new SecurityAdmin();
//				securityAdmin.OpenUpdateConnection();
//
//				try
//				{
//					securityAdmin.UpdateMyWorkflowMethodsText(_userRID, myWorkflowMethods);
//					_myWorkflowMethods = myWorkflowMethods;
//					securityAdmin.CommitData();
//				}
//				catch ( Exception err )
//				{
//					securityAdmin.Rollback();
//					string message = err.ToString();
//					throw;
//				}
//				finally
//				{
//					securityAdmin.CloseUpdateConnection();
//				}
//
//				_myWorkflowMethods = myWorkflowMethods;
//			}
//			catch ( Exception err )
//			{
//				string message = err.ToString();
//				throw;
//			}
		}

		/// <summary>
		/// Private method that loads the default Theme
		/// </summary>
		/// <param name="aThemeData">
		/// The ThemeData data layer object to use to read the Theme from the DB
		/// </param>

		private void GetDefaultTheme(ThemeData aThemeData)
		{

			DataTable dt;
			SecurityAdmin securityAdmin;

			try
			{
                //BEGIN TT#3914-VStuart-Set default Theme to Modern 1 (for new users)-MID
                dt = aThemeData.Theme_ReadByTheme(aThemeData.GetDefaultThemeRID());
                //dt = aThemeData.Theme_ReadByTheme(Include.DefaultThemeRID);
                //END TT#3914-VStuart-Set default Theme to Modern 1 (for new users)-MID


				if (dt.Rows.Count == 1)
				{
					_theme = new Theme(dt, 0);
				}
				else
				{
					throw new Exception("Invalid # of theme records returned");
				}

				aThemeData.OpenUpdateConnection();

				try
				{
					UserOptions.ThemeRID = aThemeData.Theme_Insert(_userRID, _theme);
					aThemeData.CommitData();
				}
				catch (Exception err)
				{
					aThemeData.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					aThemeData.CloseUpdateConnection();
				}

				securityAdmin = new SecurityAdmin();
				securityAdmin.OpenUpdateConnection();

				try
				{
					securityAdmin.UpdateThemeRID(_userRID, UserOptions.ThemeRID);
					securityAdmin.CommitData();
				}
				catch (Exception err)
				{
					securityAdmin.Rollback();
					string message = err.ToString();
					throw;
				}
				finally
				{
					securityAdmin.CloseUpdateConnection();
				}
			}
			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        /// <summary>
        /// Public method that deletes the scheduled infragistics layouts for a user who is logged on
        /// </summary>
        /// <param name="aUserRID">
        /// The currently Logged on user RID
        /// <param name="aLogOff">
        /// Is the application logging off
        /// </param>


        public int ScheduledLayoutDelete(int aUserRID, bool aLogOff)
        {
            
            int retRows = -1;
            try
            {
                if (aLogOff && _layoutDelete)
                {
                    layoutData = new InfragisticsLayoutData();
                    retRows = layoutData.InfragisticsUserLayout_Delete(aUserRID);
                }
                else if (!aLogOff && !_layoutDelete)
                {
                    _layoutDelete = true;
                }
                return retRows;
            }
            catch
            {
                throw;
            }
            
        }
        //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts

		/// <summary>
		/// Determines a user's security based on the security of the node and the function
		/// </summary>
		/// <param name="aNodeSecurity">
		/// The user's security to the node
		/// </param>
		/// <param name="aFunctionSecurity">
		/// The user's security to the function
		/// </param>
		/// <returns>
		/// A FunctionSecurityProfile object containing the eSecurityLevel of the actions for the Node for this User
		/// </returns>
		
		private FunctionSecurityProfile DetermineNodeFunctionSecurityAssignment(HierarchyNodeSecurityProfile aNodeSecurity, FunctionSecurityProfile aFunctionSecurity)
		{
			
			try
			{
				if (aFunctionSecurity.AllowUpdate)
				{
					if (aNodeSecurity.IsReadOnly)
					{
						aFunctionSecurity.SetReadOnly();
					}
					else if (aNodeSecurity.AccessDenied)
					{
						aFunctionSecurity.SetAccessDenied();
					}
				}
				else if (aFunctionSecurity.AllowView)
				{
					if (aNodeSecurity.AccessDenied)
					{
						aFunctionSecurity.SetAccessDenied();
					}
				}
				
				return aFunctionSecurity;
			}

			catch (Exception err)
			{
				string message = err.ToString();
				throw;
			}
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return ClientServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        ///// <summary>
        ///// Retrieves the user options from the database.
        ///// </summary>
        ///// <param name="userRID">
        ///// The user RID of the User to retrieve options for.
        ///// </param>
		
//		private void GetUserOptions(int userRID)
//		{
//			_userOptions = new UserOptionsProfile(userRID);
//			SecurityAdmin securityAdmin = null;
//			DataTable dt = null;
//			
//			try
//			{
//				securityAdmin = new SecurityAdmin();
//				dt = securityAdmin.GetUserOptions(userRID);
//
//				if (dt.Rows.Count == 0)
//				{
//					_myHierarchyName = "My Hierarchies";
//					_myHierarchyColor = Include.MIDDefault;
//					_myWorkflowMethods = "My Workflow/Methods";
//				}
//				else
//				{
//					DataRow dr = dt.Rows[0];
//
//					if ((string)dr["MY_HIERARCHY"] == " ")
//					{
//						_myHierarchyName = "My Hierarchies";
//					}
//					else
//					{
//						_myHierarchyName = (string)dr["MY_HIERARCHY"];
//					}
//
//					if ((string)dr["MY_HIERARCHY_COLOR"] == " ")
//					{
//						_myHierarchyColor = Include.MIDDefault;
//					}
//					else
//					{
//						_myHierarchyColor = (string)dr["MY_HIERARCHY_COLOR"];
//					}
//
//					if (dr["MY_WORKFLOWMETHODS"] == System.DBNull.Value)
//					{
//						_myWorkflowMethods = "My Workflow/Methods";
//					}
//					else
//					{
//						_myWorkflowMethods = (string)dr["MY_WORKFLOWMETHODS"];
//					}
//
//					if (dr["THEME_RID"] == System.DBNull.Value)
//					{
//						_themeRID = Include.Undefined;
//					}
//					else
//					{
//						_themeRID = System.Convert.ToInt32(dr["THEME_RID"], CultureInfo.CurrentUICulture);
//					}
//				}
//			}
//			catch (Exception err)
//			{
//				string message = err.ToString();
//				throw;
//			}
//		}

//		#region Basis
//		/// <summary>
//		/// Save Basis Plan and Basis Range (delete current DB records & insert new) 
//		/// as two seperate DataTables (as they are in the DB)
//		/// based on METHOD_RID and SGL_RID
//		/// </summary>
//		/// <param name="method_RID">method_RID - PK of Group Level Function</param>
//		/// <param name="sgl_RID">sgl_RID - PK of Group Level Function</param>
//		/// <param name="dtBasisPlan">DataTable containing Basis_Plan data</param>
//		/// <param name="dtBasisRange">DataTable containing Basis_Range data</param>
//		/// <param name="eTyLy">eTyLyType enum</param>
//		/// <returns>boolean; true if successful, false if failed</returns>
//		public bool InsertBasisDetails(int method_RID, int sgl_RID, DataTable dtBasisPlan, DataTable dtBasisRange, eTyLyType eTyLy)
//		{
//			try
//			{
//				BasisData bd = new BasisData();
//				bd.InsertBasis(method_RID, sgl_RID, dtBasisPlan,dtBasisRange,eTyLy);
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}
// 
//		/// <summary>
//		/// Get Basis_Plan DataTable based on METHOD_RID - Should have GLF.SGL_RID??
//		/// </summary>
//		/// <returns>DataTable</returns>
//		public DataTable GetBasisPlan(int method_RID)
//		{
//			BasisPlan bp = new  BasisPlan();
//			return bp.GetBasisPlan(method_RID);
//		}
//
//		/// <summary>
//		/// Get Basis_Plan DataTable based on METHOD_RID and SGL_RID
//		/// </summary>
//		/// <param name="method_RID"></param>
//		/// <param name="sgl_RID"></param>
//		/// <returns>DataTable</returns>
//		public DataTable GetBasisPlan(int method_RID, int sgl_RID)
//		{
//			BasisPlan bp = new  BasisPlan();
//			return bp.GetBasisPlan(method_RID, sgl_RID);
//		}
//
//		/// <summary>
//		/// Get Basis_Range DataTable based on METHOD_RID - Should have GLF.SGL_RID??
//		/// </summary>
//		/// <returns>DataTable</returns>
//		public DataTable GetBasisRange(int method_RID)
//		{
//			BasisRange br = new BasisRange();
//			return br.GetBasisRange(method_RID);
//		}
//
//		/// <summary>
//		/// Get Basis_Range DataTable based on METHOD_RID and SGL_RID
//		/// </summary>
//		/// <param name="method_RID"></param>
//		/// <param name="sgl_RID"></param>
//		/// <returns>DataTable</returns>
//		public DataTable GetBasisRange(int method_RID, int sgl_RID)
//		{
//			BasisRange br = new BasisRange();
//			return br.GetBasisRange(method_RID, sgl_RID);
//		}
//
//		/// <summary>
//		/// Gets Both the Basis_Plan table and the Basis_Range DataTable 
//		/// as one combined table based on METHOD_RID - Should have GLF.SGL_RID??
//		/// </summary>
//		/// <returns></returns>
//		public DataTable GetBasisPlanAndBasisRange(int method_RID)
//		{
//			BasisRange br = new BasisRange();
//			return br.GetBasisPlanAndBasisRange(method_RID);
//		}
//
//		/// <summary>
//		/// Gets Both the Basis_Plan table and the Basis_Range DataTable 
//		/// as one combined table based on METHOD_RID and SGL_RID
//		/// </summary>
//		/// <param name="method_RID"></param>
//		/// <param name="sgl_RI"></param>
//		/// <returns>DataTable</returns>
//		public DataTable GetBasisPlanAndBasisRange(int method_RID, int sgl_RID)
//		{
//			BasisRange br = new BasisRange();
//			return br.GetBasisPlanAndBasisRange(method_RID, sgl_RID);
//		}
//		#endregion



	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class ClientServerSession : Session
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ClientServerSession(ClientServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.clientApplication, aServiceRetryCount, aServiceRetryInterval)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public int UserRID
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.UserRID;
				}
				catch
				{
					throw;
				}
			}
		}

        public string UserID
        {
            get
            {
                try
                {
                    return ClientServerSessionRemote.UserID;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the name of the user
        /// </summary>

        public string UserName
        {
            get
            {
                try
                {
                    return ClientServerSessionRemote.UserName;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the full name of the user
        /// </summary>

        public string UserFullName
        {
            get
            {
                try
                {
                    return ClientServerSessionRemote.UserFullName;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the description of the user
        /// </summary>

        public string UserDescription
        {
            get
            {
                try
                {
                    return ClientServerSessionRemote.UserDescription;
                }
                catch
                {
                    throw;
                }
            }
        }

    /// <summary>
    /// Gets the flag identifying if the user is active
    /// </summary>

    public bool UserIsActive
    {
        get
        {
            try
            {
                return ClientServerSessionRemote.UserIsActive;
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the flag identifying if the user is scheduled to be deleted
    /// </summary>

    public bool UserIsSetToBeDeleted
    {
        get
        {
            try
            {
                return ClientServerSessionRemote.UserIsSetToBeDeleted;
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Gets the date and time when the user was scheduled to be deleted
    /// </summary>

    public DateTime UserDateTimeWhenDeleted
    {
        get
        {
            try
            {
                return ClientServerSessionRemote.UserDateTimeWhenDeleted;
            }
            catch
            {
                throw;
            }
        }
    }

    public eProcesses Process
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.Process;
				}
				catch
				{
					throw;
				}
			}
		}

		public int ReaderLockTimeOut
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.ReaderLockTimeOut;
				}
				catch
				{
					throw;
				}
			}
		}

		public int WriterLockTimeOut
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.WriterLockTimeOut;
				}
				catch
				{
					throw;
				}
			}
		}

		public string MyHierarchyName
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.MyHierarchyName;
				}
				catch
				{
					throw;
				}
			}
		}

		public string MyHierarchyColor
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.MyHierarchyColor;
				}
				catch
				{
					throw;
				}
			}
		}

		public string MyWorkflowMethods
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.MyWorkflowMethods;
				}
				catch
				{
					throw;
				}
			}
		}

		public Theme Theme
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.Theme;
				}
				catch
				{
					throw;
				}
			}
			set
			{
				try
				{
					ClientServerSessionRemote.Theme = value;
				}
				catch
				{
					throw;
				}
			}
		}

		public UserOptionsProfile UserOptions
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.UserOptions;
				}
				catch
				{
					throw;
				}
			}
		}

		public int ThreadID
		{
			get
			{
				try
				{
					return ClientServerSessionRemote.ThreadID;
				}
				catch
				{
					throw;
				}
			}
		}

		//========
		// METHODS
		//========

		public void Initialize()
		{
			try
			{
				ClientServerSessionRemote.Initialize();
			}
			catch
			{
				throw;
			}
		}

		public void Initialize(int aProcessId)
		{
			try
			{
				ClientServerSessionRemote.Initialize(aProcessId);
			}
			catch
			{
				throw;
			}
		}

		public void CleanUpGlobal()
		{
			try
			{
				ClientServerSessionRemote.CleanUpGlobal();
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        public void CloseSession()
        {
            try
            {
                ClientServerSessionRemote.CloseSession();
            }
            catch
            {
                throw;
            }
        }

        public void CloseAudit()
        {
            try
            {
                ClientServerSessionRemote.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		public void Refresh()
		{
			try
			{
				ClientServerSessionRemote.Refresh();
			}
			catch
			{
				throw;
			}
		}

		public void RefreshSecurity()
		{
			try
			{
				ClientServerSessionRemote.RefreshSecurity();
			}
			catch
			{
				throw;
			}
		}

		public void RefreshHierarchy()
		{
			try
			{
				ClientServerSessionRemote.RefreshHierarchy();
			}
			catch
			{
				throw;
			}
		}

		public ClientSessionTransaction CreateTransaction()
		{
			try
			{
				return ClientServerSessionRemote.CreateTransaction();
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetProfileList(eProfileType aProfileType)
		{
			try
			{
				return ClientServerSessionRemote.GetProfileList(aProfileType);
			}
			catch
			{
				throw;
			}
		}

		public eSecurityAuthenticate UserLogin(string aUserName, string aPassword, eProcesses aProcess)
		{
			try
			{
				// Begin TT#1581-MD - stodd Header Reconcile
				//=======================================================================================
				// Tells control server session what process and window's process ID this session is for.
				// This is used later for process control logic
				//=======================================================================================
                SessionAddressBlock.ControlServerSession.SetCurrentProcess(aProcess);
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                SessionAddressBlock.ControlServerSession.SetCurrentProcessID(currentProcess.Id);
				// End TT#1581-MD - stodd Header Reconcile

				return ClientServerSessionRemote.UserLogin(aUserName, aPassword, aProcess);
			}
			catch
			{
				throw;
			}
		}

		public eSecurityAuthenticate UserLogin(int aUserRID, eProcesses aProcess)
		{
			try
			{
                // Begin TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created
                //=======================================================================================
                // Tells control server session what process and window's process ID this session is for.
                // This is used later for process control logic
                //=======================================================================================
                SessionAddressBlock.ControlServerSession.SetCurrentProcess(aProcess);
                Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                SessionAddressBlock.ControlServerSession.SetCurrentProcessID(currentProcess.Id);
                // End TT#1611-MD - stodd - Size Curve Generate abends due to no Audit Created

                return ClientServerSessionRemote.UserLogin(aUserRID, aProcess);

			}
			catch
			{
				throw;
			}
		}

		public string GetUserName(int userRID)
		{
			try
			{
				return ClientServerSessionRemote.GetUserName(userRID);
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetSelectedHeaderList()
		{
			try
			{
				return ClientServerSessionRemote.GetSelectedHeaderList();
			}
			catch
			{
				throw;
			}
		}

		public void ClearSelectedHeaderList()
		{
			try
			{
				ClientServerSessionRemote.ClearSelectedHeaderList();
			}
			catch
			{
				throw;
			}
		}

		public void AddSelectedHeaderList(int aHeaderRID, string aHeaderID, eHeaderType headerType, int aAsrtRID, int aStyleHnRID)
		{
			try
			{
				ClientServerSessionRemote.AddSelectedHeaderList(aHeaderRID, aHeaderID, headerType, aAsrtRID, aStyleHnRID);
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1442 - RMatelic - Error processing General Allocation Method on Assortment>>> temporary
        public void AddSelectedHeaderList(int aHeaderRID, string aHeaderID, eHeaderType headerType, int aAsrtRID, int aStyleHnRID, bool bBypassEnqueue)
        {
            try
            {
                ClientServerSessionRemote.AddSelectedHeaderList(aHeaderRID, aHeaderID, headerType, aAsrtRID, aStyleHnRID, bBypassEnqueue);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2

		public ArrayList GetSelectedComponentList()
		{
			try
			{
				return ClientServerSessionRemote.GetSelectedComponentList();
			}
			catch
			{
				throw;
			}
		}

		public void ClearSelectedComponentList()
		{
			try
			{
				ClientServerSessionRemote.ClearSelectedComponentList();
			}
			catch
			{
				throw;
			}
		}

		public void AddSelectedComponentList(GeneralComponentWrapper aComponentWrapper)
		{
			try
			{
				ClientServerSessionRemote.AddSelectedComponentList(aComponentWrapper);
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetUserForecastVersions()
		{
			try
			{
				return ClientServerSessionRemote.GetUserForecastVersions();
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetUserForecastVersions(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore)
		{
			try
			{
				return ClientServerSessionRemote.GetUserForecastVersions(securitySelectType, chainOrStore);
			}
			catch
			{
				throw;
			}
		}

		public ProfileList GetUserForecastVersions(eSecuritySelectType securitySelectType, eSecurityTypes chainOrStore, int versionRid)
		{
			try
			{
				return ClientServerSessionRemote.GetUserForecastVersions(securitySelectType, chainOrStore, versionRid);
			}
			catch
			{
				throw;
			}
		}

		public eSecurityAuthenticate SetUserPassword(string userName, string oldPassword, string newPassword)
		{
			try
			{
				return ClientServerSessionRemote.SetUserPassword(userName, oldPassword, newPassword);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfileList GetUserNodesSecurityAssignment(int userRID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetUserNodesSecurityAssignment(userRID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfileList GetUserNodesSecurityAssignment(int userRID, eSecurityActions aActionID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetUserNodesSecurityAssignment(userRID, aActionID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetUserFunctionSecurityAssignment(int userRID, eSecurityFunctions funcID)
		{
			try
			{
				return ClientServerSessionRemote.GetUserFunctionSecurityAssignment(userRID, funcID);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetUserNodeSecurityAssignment(int userRID, int nodeRID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetUserNodeSecurityAssignment(userRID, nodeRID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetUserNodeFunctionSecurityAssignment(int userRID, int nodeID, eSecurityFunctions funcID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetUserNodeFunctionSecurityAssignment(userRID, nodeID, funcID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetMyUserActionSecurityAssignment(eAllocationActionType aAction, bool aInterfacedHeader)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserActionSecurityAssignment(aAction, aInterfacedHeader);
			}
			catch
			{
				throw;
			}
		}
        
        // Begin TT#2 - RMatelic - Assortment Planning
        public FunctionSecurityProfile GetMyUserActionSecurityAssignment(eAssortmentActionType aAction)
        {
            try
            {
                return ClientServerSessionRemote.GetMyUserActionSecurityAssignment(aAction);
            }
            catch
            {
                throw;
            }
        }
        // End TT#2

		public FunctionSecurityProfile GetMyUserFunctionSecurityAssignment(eSecurityFunctions funcID)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserFunctionSecurityAssignment(funcID);
			}
			catch
			{
				throw;
			}
		}

		public ArrayList GetMyUserNodes(ArrayList aNodes, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserNodes(aNodes, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public Hashtable GetMyUserNodeSecurityAssignments(ArrayList aNodes, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserNodeSecurityAssignments(aNodes, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public HierarchyNodeSecurityProfile GetMyUserNodeSecurityAssignment(int nodeRID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserNodeSecurityAssignment(nodeRID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public FunctionSecurityProfile GetMyUserNodeFunctionSecurityAssignment(int nodeRID, eSecurityFunctions funcID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetMyUserNodeFunctionSecurityAssignment(nodeRID, funcID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public VersionSecurityProfile GetMyVersionSecurityAssignment(int aVersionRID, int aSecurityType)
		{
			try
			{
				return ClientServerSessionRemote.GetMyVersionSecurityAssignment(aVersionRID, aSecurityType);
			}
			catch
			{
				throw;
			}
		}

		public void UpdateMyHierarchy(string myHierarchyName, string myHierarchyColor)
		{
			try
			{
				ClientServerSessionRemote.UpdateMyHierarchy(myHierarchyName, myHierarchyColor);
			}
			catch
			{
				throw;
			}
		}

		public void UpdateUserOptions()
		{
			try
			{
				ClientServerSessionRemote.UpdateUserOptions();
			}
			catch
			{
				throw;
			}
		}

		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				ClientServerSessionRemote.RefreshCalendar(refreshDate);
			}
			catch
			{
				throw;
			}
		}

		public void UpdateMyWorkflowMethodsText(string myWorkflowMethods)
		{
			try
			{
				ClientServerSessionRemote.UpdateMyWorkflowMethodsText(myWorkflowMethods);
			}
			catch
			{
				throw;
			}
		}

        //Begin TT#1888 - DOConnell - Add feature to clear Infragistics Layouts
        public int ScheduledLayoutDelete(int aUserRID, bool aLogOff)
        {
            try
            {
               return ClientServerSessionRemote.ScheduledLayoutDelete(aUserRID, aLogOff);
            }
            catch
            {
                throw;
            }
        }
        //End TT#1888 - DOConnell - Add feature to clear Infragistics Layouts

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return ClientServerSessionRemote.GetServiceProfile();
            }
            catch
            {
                throw;
            }
        }
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                ClientServerSessionRemote.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

	}
	//End TT#708 - JScott - Services need a Retry availalbe.
}
