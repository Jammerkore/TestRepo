using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;   // TT#1185 - Verify ENQ before Update
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.IO;

//Begin TT#708 - JScott - Services need a Retry availalbe.
using System.Threading;

//End TT#708 - JScott - Services need a Retry availalbe.
using MIDRetail.DataCommon;
using MIDRetail.Common;
using MIDRetail.Business.Allocation;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// ApplicationServerGlobal is a static class that contains fields that are global to all ApplicationServerSession objects.
	/// </summary>
	/// <remarks>
	/// The ApplicationServerGlobal class is used to store information that is global to all ApplicationServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>

	public class ApplicationServerGlobal : Global
	{
		//=======
		// FIELDS
		//=======
		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

		static private ArrayList _loadLock;
		static private bool _loaded;
		static private Audit _audit;
		//Begin TT#1234 - JScott - Add trigger file for release file
		static private string _releaseFileTriggerExtension;
		//End TT#1234 - JScott - Add trigger file for release file

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private string _forecastLogPath;

		static public string ForecastLogPath
		{
			get
			{
				return _forecastLogPath;
			}
			set
			{
				_forecastLogPath = value;
			}
		}


		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ApplicationServerGlobal
		/// </summary>

		static ApplicationServerGlobal()
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				_loadLock = new ArrayList();
				_loaded = false;

				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				if (!EventLog.SourceExists("MIDApplicationService"))
				{
					EventLog.CreateEventSource("MIDApplicationService", null);
				}
//				CreateAudit(eProcesses.applicationService);
				//Begin TT#1234 - JScott - Add trigger file for release file

				_releaseFileTriggerExtension = MIDConfigurationManager.AppSettings["ReleaseFileTriggerExtension"];

				if (_releaseFileTriggerExtension != null)
				{
					_releaseFileTriggerExtension = _releaseFileTriggerExtension.TrimStart('.', ' ');
				}
				//End TT#1234 - JScott - Add trigger file for release file
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDApplicationService", ex.Message, EventLogEntryType.Error);
			}
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private Audit Audit
		{
			get
			{
				return _audit;
			}
		}

		static public bool Loaded
		{
			get
			{
				return _loaded;
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		//Begin TT#1234 - JScott - Add trigger file for release file
		static public string ReleaseFileTriggerExtension
		{
			get
			{
				return _releaseFileTriggerExtension;
			}
		}

		//End TT#1234 - JScott - Add trigger file for release file
		//========
		// METHODS
		//========

		/// <summary>
		/// The Load method is called by the service or client to trigger the instantiation of the static ApplicationServerGlobal
		/// object
		/// </summary>

        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
        //static public void Load()
        static public void Load(bool aLocal)
        // End TT#189
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				//LoadBase(eProcesses.applicationService);
				lock (_loadLock.SyncRoot)
				{
					if (!_loaded)
					{
                        //Begin TT#5320-VStuart-deadlock issues-FinishLine
                        if (!aLocal)
                        {
                            MarkRunningProcesses(eProcesses.applicationService);  // TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
                        }
                        //End TT#5320-VStuart-deadlock issues-FinishLine

                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //_audit = new Audit(eProcesses.applicationService, Include.AdministratorUserRID);
                        if (!aLocal)
                        {
                            _audit = new Audit(eProcesses.applicationService, Include.AdministratorUserRID);
                        }
                        // End TT#189 
                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        int messagingInterval = Include.Undefined;
                        object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
                        if (parm != null)
                        {
                            messagingInterval = Convert.ToInt32(parm);
                        }
                        //LoadBase();
                        LoadBase(eMIDMessageSenderRecepient.applicationService, messagingInterval, aLocal, eProcesses.applicationService);
                        // End TT#2307;

                        // Begin TT#195 MD - JSmith - Add environment authentication
                        if (!aLocal)
                        {
                            RegisterServiceStart();
                        }
                        // End TT#195 MD

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
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                if (isExecutingLocal &&
                    MessageProcessor.isListeningForMessages)
                {
                    StopMessageListener();
                }
                // End TT#2307

				if (Audit != null)
				{
					Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
                    // Begin TT#1243 - JSmith - Audit Performance
                    Audit.CloseUpdateConnection();
                    // End TT#1243
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static public void SetPostingDate(DateTime postingDate)
		{
			try
			{
				Calendar.SetPostingDate(postingDate);				
			}
			catch
			{
				throw;
			}
		}
	}

	/// <summary>
	/// ApplicationServerSession is a class that contains fields, properties, and methods that are available to other sessions
	/// of the system.
	/// </summary>
	/// <remarks>
	/// The ApplicationServerSession class is the interface to the ApplicationServer functionality.  All requests for functionality
	/// or information in the ApplicationServer should be made through methods and properties in this class.
	/// </remarks>

	//Begin TT#708 - JScott - Services need a Retry availalbe.
	//public class ApplicationServerSession : Session
	public class ApplicationServerSessionRemote : SessionRemote
	//End TT#708 - JScott - Services need a Retry availalbe.
	{
		//=======
		// FIELDS
		//=======


//		private HierarchySessionTransaction _hierarchySessionTransaction = null;
		private System.Collections.Hashtable _profileHash;
		private System.Collections.Hashtable _profileXRefHash;
// has been moved to transaction		private Allocation.AllocationProfileList _apl = null;
//		private GlobalOptionsProfile _gop;
		private Assembly _compAssembly;
		private IPlanComputationsCollection _compCollection;
		private GetMethods _getMethods;

        // Begin TT#1684 - JSmith - Showing incorrect on hand data
        private bool _dailyOnhandExists = false;
        private bool _dailyOnhandSet = false;
        // End TT#1684

        // Begin TT#4988 - JSmith - Performance
        //private System.Collections.Hashtable _storeStatusHash;
        private Dictionary<int, Dictionary<int, eStoreStatus>> _storeStatusHash;
        // End TT#4988 - JSmith - Performance
        private System.Collections.Hashtable _openAsrtViewWindows;       // TT#2 - RMatelic - Assortment Planning
		private Dictionary<int, EnqueueDictionary> _enqueueDictionary;  // TT#1185 - JEllis Verify ENQ before Update (part 2)
		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public ApplicationServerSession(bool aLocal)
        private ArrayList _alStoreOnHandDay = null; // TT#1779 - JSmith - Invalid Calendar Date with Cancel Alloc in Style Reivew and Shipping Horizon=0

        ReaderWriterLockSlim rw = Locks.GetLockInstance();  // TT#4481 - Object reference error in calendar

		public ApplicationServerSessionRemote(bool aLocal)
		//End TT#708 - JScott - Services need a Retry availalbe.
			: base(aLocal)
		{
			_profileHash = new System.Collections.Hashtable();
			_profileXRefHash = new System.Collections.Hashtable();
			//???DAT???			_hdr = new MIDRetail.Data.Header();
			//_gop = new GlobalOptionsProfile(-1);
            // Begin TT#4988 - JSmith - Performance
            //_storeStatusHash = new System.Collections.Hashtable();
            _storeStatusHash = new Dictionary<int, Dictionary<int, eStoreStatus>>();
            // End TT#4988 - JSmith - Performance
            _openAsrtViewWindows = new System.Collections.Hashtable();  // TT#2 - RMatelic - Assortment Planning
			_enqueueDictionary = new Dictionary<int, EnqueueDictionary>();  // TT#1185 - JEllist Verify ENQ before Update (part 2)
		}

        //private string _monitorForecastFilePath = null;

		
		//===========
		// PROPERTIES
		//===========

//		public GlobalOptionsProfile GlobalOptions
//		{
//			get 
//			{ 
//				if (_gop == null)
//				{
//					_gop = new GlobalOptionsProfile(-1);
//					_gop.LoadOptions();
//				}
//				return _gop;  
//			}
//		}

//		public string MonitorForecastFilePath
//		{
//			get
//			{
//				if (_monitorForecastFilePath == null)
//					_monitorForecastFilePath = MIDConfigurationManager.AppSettings["MonitorForecastFilePath"];
//
//				return _monitorForecastFilePath;
//			}
//			set
//			{
//				_monitorForecastFilePath = value;
//			}
//		}

        // Begin TT#1684 - JSmith - Showing incorrect on hand data
        public bool DailyOnhandExists
        {
            get
            {
                if (!_dailyOnhandSet)
                {
                    //VariablesData vd = new VariablesData();
                    //_dailyOnhandExists = vd.StoreDailyOnhandExists(Calendar.CurrentDate.Key);
                    _dailyOnhandExists = (Calendar.CurrentDate.Key == Calendar.CurrentDate.Week.Days[0].Key) ? false : true;
                    _dailyOnhandSet = true;
                }
                return _dailyOnhandExists;
            }
        }
        // End TT#1684


		//Begin TT#1234 - JScott - Add trigger file for release file
		public string ReleaseFileTriggerExtension
		{
			get
			{
				return ApplicationServerGlobal.ReleaseFileTriggerExtension;
			}
		}

		//End TT#1234 - JScott - Add trigger file for release file
		/// <summary>
		/// Gets the Computations assembly from the database.
		/// </summary>

		public Assembly ComputationsAssembly
		{
			get
			{
				ComputationsDLLData compDLLDL;
				string configVersion;
				string dbDLLVersion;
				string diskDLLVersion;
				byte[] compDLL = null;
				bool dbDLLExists;
				bool diskDLLExists;
				string computationsDLL;

				try
				{
					if (_compAssembly == null)
					{
						compDLLDL = new ComputationsDLLData();

                        // Begin Track #6357 - KJohnson - Upload 14 sku's All DC's- did not upload
                        try
                        {
                            compDLLDL.OpenUpdateConnection(eLockType.ComputationItem);

                            // Begin TT#1738 - JSmith - Disable getting computation dll from database
                            //configVersion = MIDConfigurationManager.AppSettings["ForecastComputationsVersion"];
                            configVersion = "None";
                            // End TT#1738
                            computationsDLL = System.Reflection.Assembly.GetExecutingAssembly().Location;
                            computationsDLL = computationsDLL.Substring(0, computationsDLL.LastIndexOf(@"\")) + @"\" + Include.ComputationsAssemblyName;

                            if (configVersion != null)
                            {
                                if (configVersion != "None")
                                {
                                    if (compDLLDL.ComputationsDLL_Exists(configVersion))
                                    {
                                        compDLL = compDLLDL.ComputationsDLL_ReadDLL(configVersion);
                                        CreateComputationsDLLFile(computationsDLL, compDLL);
                                    }
                                    else
                                    {
                                        throw new ComputationsNotLoadedException();
                                    }
                                }
                            }
                            else
                            {
                                dbDLLExists = compDLLDL.ComputationsDLL_Exists();
                                diskDLLExists = System.IO.File.Exists(computationsDLL);

                                if (dbDLLExists && diskDLLExists)
                                {
                                    dbDLLVersion = compDLLDL.ComputationsDLL_ReadLatestVersion();
                                    diskDLLVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(computationsDLL).ProductVersion;

                                    if (dbDLLVersion != diskDLLVersion)
                                    {
                                        compDLL = compDLLDL.ComputationsDLL_ReadLatestDLL();
                                        CreateComputationsDLLFile(computationsDLL, compDLL);
                                    }
                                }
                                else if (dbDLLExists)
                                {
                                    compDLL = compDLLDL.ComputationsDLL_ReadLatestDLL();
                                    CreateComputationsDLLFile(computationsDLL, compDLL);
                                }
                                else if (!diskDLLExists)
                                {
                                    throw new ComputationsNotLoadedException();
                                }
                            }

                            _compAssembly = Assembly.LoadFrom(computationsDLL);

                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        finally 
                        {
                            compDLLDL.CloseUpdateConnection();
                        }
                        // End Track #6357
                    }

					return _compAssembly;
				}
				catch (ComputationsNotLoadedException)
				{
					throw new MIDException(eErrorLevel.severe,
						(int)eMIDTextCode.msg_pl_ComputationsNotLoaded,
						MIDText.GetText(eMIDTextCode.msg_pl_ComputationsNotLoaded));
				}
				catch (Exception exc)
				{
					string message = exc.ToString();
					throw;
				}
			}
		}

		/// <summary>
		/// Gets the ComputationsCollection object.
		/// </summary>

		public IPlanComputationsCollection ComputationsCollection
		{
			get
			{
				if (_compCollection == null)
				{
					_compCollection = (IPlanComputationsCollection)Activator.CreateInstance(
						ComputationsAssembly.GetType("MIDRetail.ForecastComputations.PlanComputationsCollection"));
				}

				return _compCollection;
			}
		}

		/// <summary>
		/// Gets the Computations object from the ApplicationServerSession.
		/// </summary>

		public IPlanComputations DefaultPlanComputations
		{
			get
			{
				return ComputationsCollection.GetDefaultComputations();
			}
		}

		/// <summary>
		/// Gets the GetMethods object.
		/// </summary>

		public GetMethods GetMethods
		{
			get
			{
				if (_getMethods == null)
				{
					_getMethods = new GetMethods(this.SessionAddressBlock);
				}

				return _getMethods;
			}
		}

		/// <summary>
		/// Creates the Computations DLL assembly file.
		/// </summary>
		/// <param name="aCompDLL">
		/// An array of bytes that contains the DLL source.
		/// </param>

		public void CreateComputationsDLLFile(string aCompFile, byte[] aCompDLL)
		{
			FileStream file;
			BinaryWriter writer;

			try
			{
                // Begin TT#1089 - JSmith - IdentifyNotMappedException during upgrade
                //AddFileSecurity(aCompFile, Environment.UserDomainName, System.Security.AccessControl.FileSystemRights.Modify, System.Security.AccessControl.AccessControlType.Allow);
                FileInfo fi = new System.IO.FileInfo(aCompFile);
                if (fi.IsReadOnly)
                {
                    AddFileSecurity(aCompFile, @"Users", System.Security.AccessControl.FileSystemRights.Modify, System.Security.AccessControl.AccessControlType.Allow);
                }
                // End TT#1089
				file = File.Open(aCompFile, FileMode.Create);
				writer = new BinaryWriter(file);
				writer.Write(aCompDLL);
				writer.Close();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        public void AddFileSecurity(string fileName, string account,
          System.Security.AccessControl.FileSystemRights rights, System.Security.AccessControl.AccessControlType controlType)
        {


            // Get a FileSecurity object that represents the
            // current security settings.
            System.Security.AccessControl.FileSecurity fSecurity = File.GetAccessControl(fileName);

            // Add the FileSystemAccessRule to the security settings.
            fSecurity.AddAccessRule(new System.Security.AccessControl.FileSystemAccessRule(account,
                rights, controlType));

            // Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);

        }

		public override void Initialize()
		{
			base.Initialize();
			// BEGIN 4320 - stodd 2.15.2007
			Calendar = ApplicationServerGlobal.Calendar;
			// END 4320 - stodd 2.15.2007
            // Begin TT#46 MD - JSmith - User Dashboard
			//CreateAudit();
			CreateAudit(true);
            // End TT#46 MD
			//_gop.LoadOptions();
			DateTime postingDate = SessionAddressBlock.HierarchyServerSession.GetPostingDate();
			Calendar.SetPostingDate(postingDate);
			ApplicationServerGlobal.SetPostingDate(postingDate);
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                ApplicationServerGlobal.VerifyEnvironment(aClientProfile);
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
			ApplicationServerGlobal.CleanUp();
		}

		/// <summary>
		/// Updates the posting date.
		/// </summary>
		/// <param name="postingDate">The posting date</param>
		/// <remarks>Currently updates the posting date for the organizational hierarchy</remarks>
		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				Calendar.SetPostingDate(postingDate);
				ApplicationServerGlobal.SetPostingDate(postingDate);
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}


		/// <summary>
		/// Clears all cached areas in the session.
		/// </summary>
		public void Refresh()
		{
			try
			{
				_profileHash.Clear();
				_profileXRefHash.Clear();
				_storeStatusHash.Clear();
                //Begin TT#1684 - JSmith - Showing incorrect on hand data
                _dailyOnhandSet = false;
                //End TT#1684
                // Begin TT#1779 - JSmith - Invalid Calendar Date with Cancel Alloc in Style Reivew and Shipping Horizon=0
                _alStoreOnHandDay = null;
                // End TT#1779
				RefreshBase();
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
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}

//		// if set to null the next it is accessed it will repull the latest version of Global Options
//		public void RefreshGlobalOptions()
//		{
//			base.RefreshBase();
//		}

        //Begin TT#1517-MD -jsobek -Store Service Optimization


        //// Begin Track #4872 - JSmith - Global/User Attributes
        ///// <summary>
        ///// This method will retrieve the current ProfileList stored in this session.  If the ProfileList has not yet been created, the
        ///// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
        ///// be thrown.
        ///// </summary>
        ///// <param name="aProfileType">
        ///// The eProfileType of the ProfileList to retieve.
        ///// </param>
        ///// <returns>
        ///// The ProfileList object for the given eProfileType.
        ///// </returns>
        //public ProfileList GetProfileList(eProfileType aProfileType)
        //{
        //    return GetProfileList(aProfileType, false);
        //}
        //// End Track #4872

        /// <summary>
        /// This method will retrieve the current ProfileList stored in this session.  If the ProfileList has not yet been created, the
        /// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
        /// be thrown.
        /// </summary>
        /// <param name="aProfileType">
        /// The eProfileType of the ProfileList to retieve.
        /// </param>
        /// <param name="aAddGlobalUserLabel">
        /// This flag indicates that the entry should be identified with global or user name.
        /// </param>
		/// <returns>
		/// The ProfileList object for the given eProfileType.
		/// </returns>

        public ProfileList GetProfileList(eProfileType aProfileType, bool aAddGlobalUserLabel)
		{
			ProfileList profileList;

            // Begin TT#4481 - JSmith - Object reference error in calendar
            using (new WriteLock(rw))
            {
            // End TT#4481 - JSmith - Object reference error in calendar
                profileList = (ProfileList)_profileHash[aProfileType];

                if (profileList == null)
                {
                    switch (aProfileType)
                    {
                        case eProfileType.Store:

                            profileList = StoreMgmt.StoreProfiles_GetActiveStoresList(); //SessionAddressBlock.StoreServerSession.GetActiveStoresList();
                            _profileHash.Add(profileList.ProfileType, profileList);

                            break;

                        case eProfileType.StoreGroup:

                            profileList = StoreMgmt.StoreGroup_GetList(); //SessionAddressBlock.StoreServerSession.GetStoreGroupList();
                            _profileHash.Add(profileList.ProfileType, profileList);

                            break;

                        case eProfileType.StoreGroupListView:

                            // Begin Track #4872 - JSmith - Global/User Attributes
                            //profileList = SessionAddressBlock.StoreServerSession.GetStoreGroupListViewList();
                            profileList = StoreMgmt.StoreGroup_GetListViewList(eStoreGroupSelectType.MyUserAndGlobal, aAddGlobalUserLabel); //SessionAddressBlock.StoreServerSession.GetStoreGroupListViewList(eStoreGroupSelectType.All, aAddGlobalUserLabel);
                            // End Track #4872
                            _profileHash.Add(aProfileType, profileList);

                            break;

                        case eProfileType.Version:

                            profileList = SessionAddressBlock.ClientServerSession.GetUserForecastVersions();
                            _profileHash.Add(profileList.ProfileType, profileList);

                            break;

                        case eProfileType.SelectedHeader:

                            profileList = SessionAddressBlock.ClientServerSession.GetSelectedHeaderList();
                            // do not cache copy here.  Need to make sure session always always has current copy from ClientServerSession

                            break;
                    }
                }

			return profileList;
            }  // TT#4481 - JSmith - Object reference error in calendar
        }

        public ProfileList GetProfileListVersion()
        {
            ProfileList profileList = (ProfileList)_profileHash[eProfileType.Version];

            if (profileList == null)
            {
                profileList = SessionAddressBlock.ClientServerSession.GetUserForecastVersions();
                _profileHash.Add(profileList.ProfileType, profileList);
            }

            return profileList;
        }
        
        // Begin TT#3371 - JSmith - Attribute Set Order is not being honored
        public void RemoveProfileList(eProfileType aProfileType)
        {
            // Begin TT#4481 - JSmith - Object reference error in calendar
            //_profileHash.Remove(aProfileType);
            using (new WriteLock(rw))
            {
                _profileHash.Remove(aProfileType);
            }
            // End TT#4481 - JSmith - Object reference error in calendar
        }
        // End TT#3371 - JSmith - Attribute Set Order is not being honored

//Begin Track #3767 - JScott - Force client to use cached store group lists in application session
		/// <summary>
		/// This method will retrieve the current StoreGroupLevel ProfileList stored in this session.  If the ProfileList has not yet been created, the
		/// values will be retrieved from the StoreSession, if necessary.
		/// </summary>
		/// <param name="aGroupRID">
		/// The GroupRID of the StoreGroup to retrieve the levels for.
		/// </param>
		/// <returns>
		/// The StoreGroupLevel ProfileList object.
		/// </returns>

        //public ProfileList GetStoreGroupLevelProfileList(int aGroupRID)
        //{
        //    //StoreGroupProfile storeGroupProf;
        //    //storeGroupProf = (StoreGroupProfile)(GetProfileList(eProfileType.StoreGroup)).FindKey(aGroupRID);

        //    StoreGroupProfile storeGroupProf = (StoreGroupProfile)(StoreMgmt.GetStoreGroupList()).FindKey(aGroupRID);



        //    if (!storeGroupProf.Filled)
        //    {
        //        storeGroupProf.GroupLevels = StoreMgmt.GetStoreGroupLevelList(aGroupRID); //SessionAddressBlock.StoreServerSession.GetStoreGroupLevelList(aGroupRID);
        //        storeGroupProf.Filled = true;
        //    }

        //    return storeGroupProf.GroupLevels;
        //}

		/// <summary>
		/// This method will retrieve the current StoreGroupLevelListView ProfileList stored in this session.  If the ProfileList has not yet been created, the
		/// values will be retrieved from the StoreSession, if necessary.
		/// </summary>
		/// <param name="aGroupRID">
		/// The GroupRID of the StoreGroupListView to retrieve the levels for.
		/// </param>
		/// <returns>
		/// The StoreGroupLevelListView ProfileList object.
		/// </returns>

        //public ProfileList GetStoreGroupLevelListViewProfileList(int aGroupRID, bool aFillStores)
        //{
        //    StoreGroupListViewProfile storeGroupListViewProf;

        //    storeGroupListViewProf = (StoreGroupListViewProfile)(GetProfileList(eProfileType.StoreGroupListView)).FindKey(aGroupRID);

        //    return ((StoreGroupListViewProfile)(GetProfileList(eProfileType.StoreGroupListView)).FindKey(aGroupRID)).GetGroupLevelList(aFillStores);
        //}

//End Track #3767 - JScott - Force client to use cached store group lists in application session
		/// <summary>
		/// This method will retrieve the current ProfileXRef stored in this session.  If the ProfileXRef has not yet been created, the
		/// values will be retrieved from other Sessions, if necessary.  If the information is not available in other sessions, an error will
		/// be thrown.
		/// </summary>
		/// <returns>
		/// The ProfileXRef object for this total/detail profile combination.
		/// </returns>

		public BaseProfileXRef GetProfileXRef(BaseProfileXRef aProfXRef)
		{
			BaseProfileXRef profileXRef;

			profileXRef = (BaseProfileXRef)_profileXRefHash[aProfXRef];

			if (profileXRef == null && aProfXRef.GetType() == typeof(ProfileXRef))
			{
				switch (((ProfileXRef)aProfXRef).TotalType)
				{
					case eProfileType.Period:

						switch (((ProfileXRef)aProfXRef).DetailType)
						{
							case eProfileType.Week:
								profileXRef = new DateProfileXRef(Calendar);
								break;
						}

						break;
				}

				if (profileXRef != null)
				{
					_profileXRefHash.Add(profileXRef, profileXRef);
				}
			}

			return profileXRef;
		}

		/// <summary>
		/// Creates and returns a new Transaction object.
		/// </summary>
		/// <returns>
		/// The newly created Transaction object that points to this Session.
		/// </returns>
		public ApplicationSessionTransaction CreateTransaction()
		{
			return new ApplicationSessionTransaction(SessionAddressBlock);
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
        /// <summary>
        /// Creates and returns a new Transaction object
        /// </summary>
        /// <param name="aSession">Session to use in the creation of the new transaction</param>
        /// <returns>ApplicationSessionTransaction</returns>
        public ApplicationSessionTransaction CreateTransaction(Session aSession)
        {
            return new ApplicationSessionTransaction(SessionAddressBlock, aSession);
        }
        // end TT#1185 - JEllis - Verify ENQ before UPdate (part 2)

		/// <summary>
		/// Refreshes the Global's calendar.
		/// </summary>
		/// <remarks>
		/// Since the calendar is static, it may have already been refreshed.
		/// That's why we check the refresh date.
		/// </remarks>
		/// <param name="refreshDate"></param>
		public void RefreshCalendar(DateTime refreshDate)
		{
			if (refreshDate != ApplicationServerGlobal.CalendarRefreshDate)
			{
				ApplicationServerGlobal.Calendar.Refresh();
				ApplicationServerGlobal.CalendarRefreshDate = refreshDate;
			}

			// Refresh the Calendar of THIS session
			Calendar.Refresh();
		}

		// begin MID Track 6001 Header Run Times Too Long
		int _lastStoreStatusYearWeek = int.MinValue;
        // Begin TT#4988 - JSmith - Performance
        //Hashtable _lastStoreStatusYearWeekHash;
        //public eStoreStatus GetStoreStatus(int storeRID, int yearWeek)
        //{
        //    try
        //    {
        //        // begin MID Track 6001 Header Run Times Too Long
        //        //Hashtable yearWeekHash;
        //        //yearWeekHash = (Hashtable) _storeStatusHash[yearWeek];
        //        //if (yearWeekHash == null)
        //        //{
        //        //	yearWeekHash = SessionAddressBlock.StoreServerSession.GetStoreSalesStatusHash(yearWeek);
        //        //	_storeStatusHash.Add(yearWeek, yearWeekHash);
        //        //}
        //        //return (eStoreStatus)yearWeekHash[storeRID];
        //        if (_lastStoreStatusYearWeek != yearWeek)
        //        {
        //            _lastStoreStatusYearWeek = yearWeek;
        //            _lastStoreStatusYearWeekHash = (Hashtable)_storeStatusHash[_lastStoreStatusYearWeek];
        //        }
        //        if (_lastStoreStatusYearWeekHash == null)
        //        {
        //            _lastStoreStatusYearWeekHash = SessionAddressBlock.StoreServerSession.GetStoreSalesStatusHash(_lastStoreStatusYearWeek);
        //            _storeStatusHash.Add(_lastStoreStatusYearWeek, _lastStoreStatusYearWeekHash);
        //        }
        //        return (eStoreStatus)_lastStoreStatusYearWeekHash[storeRID];
        //        // end MID Track 6001 Header Run Times Too Long
        //    }
        //    catch (Exception err)
        //    {
        //        string message = err.ToString();
        //        throw;
        //    }
        //}

        Dictionary<int, eStoreStatus> _lastStoreStatusYearWeekHash;
        
		// end MID Track 6001 Header Run Times Too Long
		public eStoreStatus GetStoreStatus(int storeRID, int yearWeek)
		{
			try
			{
				if (_lastStoreStatusYearWeek != yearWeek)
				{
					_lastStoreStatusYearWeek = yearWeek;
                    if (!_storeStatusHash.TryGetValue(_lastStoreStatusYearWeek, out _lastStoreStatusYearWeekHash))
                    {
                        // Begin TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                        //_lastStoreStatusYearWeekHash = StoreMgmt.GetStoreSalesStatusHash(_lastStoreStatusYearWeek,SessionAddressBlock.ApplicationServerSession); //SessionAddressBlock.StoreServerSession.GetStoreSalesStatusHash(_lastStoreStatusYearWeek);
                        _lastStoreStatusYearWeekHash = StoreMgmt.GetStoreSalesStatusHash(_lastStoreStatusYearWeek, SessionAddressBlock.ApplicationServerSession, SessionAddressBlock, GetProfileList(eProfileType.Store, false));
                        // End TT#1872-MD JSmith - Str Load API runninng.  User tries to open the OTS Forecast Review Screen and receives mssg that the Str Service is not available.
                        _storeStatusHash.Add(_lastStoreStatusYearWeek, _lastStoreStatusYearWeekHash);
                    }
				}
				return (eStoreStatus)_lastStoreStatusYearWeekHash[storeRID];
			}
			catch ( Exception err )
			{
				string message = err.ToString();
				throw;
			}
		}
        // End TT#4988 - JSmith - Performance

        // Begin TT#1779 - JSmith - Invalid Calendar Date with Cancel Alloc in Style Reivew and Shipping Horizon=0
        public ArrayList DetermineInStoreOnHandDay(ArrayList beginDates, int[] storeList)
        {
            if (_alStoreOnHandDay == null)
            {
                _alStoreOnHandDay = StoreMgmt.DetermineInStoreOnHandDay(beginDates, storeList, SessionAddressBlock.ApplicationServerSession);  //SessionAddressBlock.StoreServerSession.DetermineInStoreOnHandDay(beginDates, storeList); // SMR Change Calendar
            }
            return _alStoreOnHandDay;
        }
        // End TT#1779

		/// <summary>
		/// Gets the list of Computations.
		/// </summary>
		/// <returns>
		/// A string array of Computations.
		/// </returns>

		public string[] GetComputationModes()
		{
			IPlanComputations[] compArr;
			string[] compNameArr;
			int i;

			try
			{
				compArr = ComputationsCollection.GetComputationList();
				compNameArr = new string[compArr.Length];

				for (i = 0; i < compArr.Length; i++)
				{
					compNameArr[i] = compArr[i].Name;
				}

				return compNameArr;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		/// <summary>
		/// Gets the Default Computations.
		/// </summary>
		/// <returns>
		/// A string for Default Computations.
		/// </returns>

		public string GetDefaultComputations()
		{
			try
			{
				return ComputationsCollection.GetDefaultComputations().Name;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)

        private object enqueueLockObject = new Object();
        public bool EnqueueHeaders(int aThreadID, long aTransactionID, int aUserRID, List<int> aHdrRIDList, out string aHdrConflictMsg)
        {
            if (aHdrRIDList.Count == 0)
            {
                aHdrConflictMsg = "There are no headers to enqueue.";
                return false;
            }
            HeaderEnqueue he = GetHeaderEnqueueObject(aThreadID, aTransactionID, aUserRID);
            if (he.EnqueueHeaders(aHdrRIDList))
            {
                aHdrConflictMsg = "Header Enqueue Successful";
                return true;
            }
            aHdrConflictMsg = he.FormatHeaderConflictMsg();
            return false;
        }
        public void DequeueHeaders(int aThreadID, long aTransactionID, int aUserRID)
        {
            HeaderEnqueue he = GetHeaderEnqueueObject(aThreadID, aTransactionID, aUserRID);
            lock (enqueueLockObject)
            {
                he.DequeueHeaders();
                RemoveEnqueueHeaderObject(aThreadID, aTransactionID);
            }
        }
        private void RemoveEnqueueHeaderObject(int aThreadID, long aTransactionID)
        {
            EnqueueDictionary ed;
            if (_enqueueDictionary.TryGetValue(aThreadID, out ed))
            {
                ed.Remove(aTransactionID);
                if (ed.Count == 0)
                {
                    _enqueueDictionary.Remove(aThreadID);
                }
            }
        }
        int _lastThreadID = -1;
        long _lastTransactionID;
        HeaderEnqueue _lastHeaderEnqueue;
        EnqueueDictionary _lastEnqueueDictionary;
        public HeaderEnqueue GetHeaderEnqueueObject(int aThreadID, long aTransactionID, int aUserRID)
        {
            lock (enqueueLockObject)
            {
                if (_lastThreadID != aThreadID)
                {
                    _lastThreadID = aThreadID;
                    _lastTransactionID = -1;
                    if (!_enqueueDictionary.TryGetValue(_lastThreadID, out _lastEnqueueDictionary))
                    {
                        _lastEnqueueDictionary = new EnqueueDictionary();
                        _enqueueDictionary.Add(_lastThreadID, _lastEnqueueDictionary);
                    }
                }
                if (_lastTransactionID != aTransactionID)
                {
                    _lastTransactionID = aTransactionID;
                    if (!_lastEnqueueDictionary.TryGetValue(_lastTransactionID, out _lastHeaderEnqueue))
                    {
                        _lastHeaderEnqueue = new HeaderEnqueue(_lastThreadID, _lastTransactionID, aUserRID);
                        _lastEnqueueDictionary.Add(aTransactionID, _lastHeaderEnqueue);
                    }
                }
                return _lastHeaderEnqueue;
            }
        }

        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

        // Begin TT#634 - JSmith - Color rename
        public bool RenameColor(ref EditMsgs em, bool aUpdateHeaders,
            HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        {
            bool successful = true;
            ApplicationSessionTransaction trans = null;
            try
            {
                trans = CreateTransaction();
                trans.DataAccess.OpenUpdateConnection();

                if (aUpdateHeaders)
                {
                    if (!UpdateColorOnHeaders(ref em, trans, aHnp, aOldColorCodeProfile, aNewColorCodeProfile))
                    {
                        successful = false;
                    }
                }

                //Begin TT#739-MD -jsobek -Delete Stores
                //if (successful)
                //{
                //    if (!UpdateBinRecords(ref em, trans, aHnp, aOldColorCodeProfile, aNewColorCodeProfile))
                //    {
                //        successful = false;
                //    }
                //}
                //End TT#739-MD -jsobek -Delete Stores

                if (successful)
                {
                    trans.DataAccess.CommitData();
                }
                else
                {
                    trans.DataAccess.Rollback();
                }

                return successful;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
            finally
            {
                if (trans != null &&
                    trans.DataAccess.ConnectionIsOpen)
                {
                    trans.DataAccess.CloseUpdateConnection();
                }
            }
        }

        // begin TT#1185 - Verify ENQ before Update
        private bool UpdateColorOnHeaders(ref EditMsgs em, ApplicationSessionTransaction aTrans,
            HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        {
            Header HeaderData = null;
            ApplicationSessionTransaction trans = null;
            AllocationProfileList multiHeaderChildrenList;
            bool headerUpdateFailed = false;
            try
            {
                string message = null;
                bool headersUpdated = true;
                HeaderData = new Header(aTrans.DataAccess);
                DataTable styleHeaders = HeaderData.GetHeaders(aHnp.HomeHierarchyParentRID, aOldColorCodeProfile.Key);
                List<int> hdrRidList = new List<int>();
                List<int> nonMultiHdrRidList = new List<int>();
                Dictionary<int, int> hdrMultiDict = new Dictionary<int, int>(); 
                Dictionary<int, string> hdrIDDict = new Dictionary<int, string>();
                if (styleHeaders.Rows.Count > 0)
                {
                    trans = CreateTransaction();
                    foreach (DataRow dr in styleHeaders.Rows)
                    {
                        int hdrRid = Convert.ToInt32(dr["HDR_RID"], CultureInfo.CurrentUICulture);
                        hdrRidList.Add(hdrRid);
                        hdrIDDict.Add(hdrRid, Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentUICulture));
                        int hdrGroupRID = Convert.ToInt32(dr["HDR_GROUP_RID"], CultureInfo.CurrentUICulture);
                        if (hdrGroupRID > 1)
                        {
                            if (hdrGroupRID != hdrRid)
                            {
                                hdrRidList.Add(hdrGroupRID);
                                hdrMultiDict.Add(hdrRid, hdrGroupRID);
                            }
                        }
                        else
                        {
                            nonMultiHdrRidList.Add(hdrRid);
                        }
                    }
                    if (trans.EnqueueHeaders(trans.GetHeadersToEnqueue(hdrRidList), out message))
                    {
                        int hdrMultiRID;
                        foreach (int hdrRID in nonMultiHdrRidList)
                        {
                            if (hdrMultiDict.TryGetValue(hdrRID, out hdrMultiRID))
                            {
                                // if header is a member of a multi-header, remove it from the multi
                                // NOTE: Remove of child header is committed whether or not the "color" change occurs
                                AllocationProfile ap = new AllocationProfile(trans, null, hdrRID, trans.SAB.ApplicationServerSession);
                                ap.LoadStores();
                                AllocationProfile multiHeaderProfile = new AllocationProfile(trans, null, hdrMultiDict[hdrRID], trans.SAB.ApplicationServerSession);
                                multiHeaderProfile.LoadStores();
                                multiHeaderProfile.RemoveHeaderFromMulti(multiHeaderProfile, ap);
                                message = MIDText.GetText(eMIDTextCode.msg_al_HeaderRemovedFromMultiHeader);
                                message = message.Replace("{0}", ap.HeaderID);
                                message = message.Replace("{1}", multiHeaderProfile.HeaderID);
                                em.AddMsg(eMIDMessageLevel.Warning, message, GetType().Name);

                                // delete multi if no children left
                                multiHeaderChildrenList = multiHeaderProfile.GetMultiHeaderChildren();
                                if (multiHeaderChildrenList.Count == 0)
                                {
                                    multiHeaderProfile.DeleteHeader();
                                    message = MIDText.GetText(eMIDTextCode.msg_al_MultiHeaderNoChildrenDeleted);
                                    message = message.Replace("{0}", multiHeaderProfile.HeaderID);
                                    em.AddMsg(eMIDMessageLevel.Warning, message, GetType().Name);
                                }
                            }
                            try
                            {
                                HeaderData.UpdateBulkColorOnHeader(hdrRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
                            }
                            catch (MIDException ex)
                            {
                                em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, GetType().Name);
                                headerUpdateFailed = true;
                            }
                            catch (Exception ex)
                            {
                                em.AddMsg(eMIDMessageLevel.Error, ex.Message, GetType().Name);
                                headerUpdateFailed = true;
                            }
                            try
                            {
                                HeaderData.UpdatePackColorOnHeader(hdrRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
                            }
                            catch (MIDException ex)
                            {
                                em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, GetType().Name);
                                headerUpdateFailed = true;
                            }
                            catch (Exception ex)
                            {
                                em.AddMsg(eMIDMessageLevel.Error, ex.Message, GetType().Name);
                                headerUpdateFailed = true;
                            }
                            if (headerUpdateFailed)
                            {
                                headersUpdated = false;
                                message = MIDText.GetText(eMIDTextCode.msg_al_HeaderColorRenameFailed);
                                message = message.Replace("{0}", aOldColorCodeProfile.ColorCodeID);
                                message = message.Replace("{1}", hdrIDDict[hdrRID]);
                                em.AddMsg(eMIDMessageLevel.Error, message, GetType().Name);
                            }
                        }
                    }
                    else
                    {
                        headersUpdated = false;
                        em.AddMsg(eMIDMessageLevel.Information,
                                  MIDText.GetText(eMIDTextCode.msg_al_HeaderEnqFailed),
                                  GetType().Name); 
                        em.AddMsg(eMIDMessageLevel.Severe, message, GetType().Name);
                    }
                }
                return headersUpdated;
            }
            finally
            {
                trans.DequeueHeaders();
            }
        }

        //private bool UpdateColorOnHeaders(ref EditMsgs em, ApplicationSessionTransaction aTrans,
        //    HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        //{
        //    Header HeaderData = null;
        //    HeaderEnqueue headerEnqueue = null;
        //    AllocationHeaderProfile ahp;
        //    AllocationHeaderProfileList headerList;
        //    ApplicationSessionTransaction trans = null;
        //    AllocationProfileList apl;
        //    AllocationProfileList multiHeaderChildrenList;
        //    bool headerContainsColor = false;
        //    bool headerUpdateFailed = false;
        //    bool foundBulkColor = false;
        //    ArrayList packs = null;

        //    try
        //    {
        //        string message = null;
        //        bool headersUpdated = true;
        //        apl = new AllocationProfileList(eProfileType.AllocationHeader);

        //        HeaderData = new Header();

        //        DataTable styleHeaders = HeaderData.GetHeaders(aHnp.HomeHierarchyParentRID);

        //        if (styleHeaders.Rows.Count > 0)
        //        {
        //            // enqueue all headers for style that contain the color to make sure they can be updated
        //            headerList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);
        //            foreach (DataRow dr in styleHeaders.Rows)
        //            {
        //                headerContainsColor = false;
        //                int headerRID = Convert.ToInt32(dr["HDR_RID"]);
        //                string headerID = Convert.ToString(dr["HDR_ID"], CultureInfo.CurrentCulture);
        //                trans = CreateTransaction();
        //                AllocationProfileList apl2 = new AllocationProfileList(eProfileType.Allocation);
        //                //Begin TT#708 - JScott - Services need a Retry availalbe.
        //                //AllocationProfile ap = new AllocationProfile(trans, headerID, headerRID, this);
        //                AllocationProfile ap = new AllocationProfile(trans, headerID, headerRID, trans.SAB.ApplicationServerSession);
        //                //End TT#708 - JScott - Services need a Retry availalbe.
        //                apl2.Add(ap);
        //                trans.SetMasterProfileList(apl2);
        //                ap.LoadStores();

        //                if (ap.MultiHeader)
        //                {
        //                    continue;
        //                }

        //                if (ap.BulkColors != null && ap.BulkColors.Count > 0)
        //                {
        //                    foreach (HdrColorBin aColor in ap.BulkColors.Values)
        //                    {
        //                        if (aColor.ColorCodeRID == aOldColorCodeProfile.Key)
        //                        {
        //                            headerContainsColor = true;
        //                            break;
        //                        }
        //                    }
        //                }

        //                if (!headerContainsColor)
        //                {
        //                    if ((ap.Packs != null && ap.Packs.Count > 0))
        //                    {
        //                        foreach (PackHdr aPack in ap.Packs.Values)
        //                        {
        //                            foreach (PackColorSize pcs in aPack.PackColors.Values)
        //                            {
        //                                if (pcs.ColorCodeRID == aOldColorCodeProfile.Key)
        //                                {
        //                                    headerContainsColor = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //                if (headerContainsColor)
        //                {
        //                    // add header to process list
        //                    apl.Add(ap);
        //                    // add header to enqueue list
        //                    ahp = new AllocationHeaderProfile(headerRID);
        //                    headerList.Add(ahp);
        //                    // if in use by multi, add multi to enqueue list
        //                    if (ap.InUseByMulti)
        //                    {
        //                        if (!headerList.Contains(ap.HeaderGroupRID))
        //                        {
        //                            ahp = new AllocationHeaderProfile(ap.HeaderGroupRID);
        //                            headerList.Add(ahp);
        //                        }
        //                    }
        //                }
        //            }

        //            // no headers contain color, so just return
        //            if (headerList.Count == 0)
        //            {
        //                return true;
        //            }

        //            try
        //            {
        //                trans = CreateTransaction();
        //                headerEnqueue = new HeaderEnqueue(trans, headerList);
        //                headerEnqueue.EnqueueHeaders();
        //            }
        //            catch (HeaderConflictException)
        //            {
        //                DisplayEnqueueConflict(ref em, headerEnqueue, apl);
        //                return false;
        //            }

        //            //HeaderData.OpenUpdateConnection();
        //            // create new object using open database transaction
        //            HeaderData = new Header(aTrans.DataAccess);
        //            // replace colors on header
        //            foreach (AllocationProfile ap in apl)
        //            {
        //                // set the data layer so all use the same database transaction
        //                ap.HeaderDataRecord = HeaderData;

        //                if (ap.InUseByMulti)
        //                {
        //                    //Begin TT#708 - JScott - Services need a Retry availalbe.
        //                    //AllocationProfile multiHeaderProfile = new AllocationProfile(ap.AppSessionTransaction, null, ap.HeaderGroupRID, this);
        //                    AllocationProfile multiHeaderProfile = new AllocationProfile(ap.AppSessionTransaction, null, ap.HeaderGroupRID, trans.SAB.ApplicationServerSession);
        //                    //End TT#708 - JScott - Services need a Retry availalbe.
        //                    multiHeaderProfile.LoadStores();
        //                    multiHeaderProfile.RemoveHeaderFromMulti(multiHeaderProfile, ap);
        //                    message = MIDText.GetText(eMIDTextCode.msg_al_HeaderRemovedFromMultiHeader);
        //                    message = message.Replace("{0}", ap.HeaderID);
        //                    message = message.Replace("{1}", multiHeaderProfile.HeaderID);
        //                    em.AddMsg(eMIDMessageLevel.Warning, message, GetType().Name);

        //                    // delete multi if no children left
        //                    multiHeaderChildrenList = multiHeaderProfile.GetMultiHeaderChildren();
        //                    if (multiHeaderChildrenList.Count == 0)
        //                    {
        //                        multiHeaderProfile.DeleteHeader();
        //                        message = MIDText.GetText(eMIDTextCode.msg_al_MultiHeaderNoChildrenDeleted);
        //                        message = message.Replace("{0}", multiHeaderProfile.HeaderID);
        //                        em.AddMsg(eMIDMessageLevel.Warning, message, GetType().Name);
        //                    }
        //                }

        //                if (ap.BulkColors != null && ap.BulkColors.Count > 0)
        //                {
        //                    foundBulkColor = false;
        //                    foreach (HdrColorBin aColor in ap.BulkColors.Values)
        //                    {
        //                        if (aColor.ColorCodeRID == aOldColorCodeProfile.Key)
        //                        {
        //                            foundBulkColor = true;
        //                        }
        //                        else if (aColor.ColorCodeRID == aNewColorCodeProfile.Key)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed), GetType().Name);
        //                            headerUpdateFailed = true;
        //                        }
        //                    }
        //                    if (!headerUpdateFailed &&
        //                        foundBulkColor)
        //                    {
        //                        try
        //                        {
        //                            HeaderData.UpdateBulkColorOnHeader(ap.Key, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //                            //ap.SetBulkColorCodeRID(aOldColorCodeProfile.Key, aNewColorCodeProfile.Key, true);
        //                        }
        //                        catch (MIDException ex)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, GetType().Name);
        //                            headerUpdateFailed = true;
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            em.AddMsg(eMIDMessageLevel.Error, ex.Message, GetType().Name);
        //                            headerUpdateFailed = true;
        //                        }
        //                    }
        //                }

        //                if ((ap.Packs != null && ap.Packs.Count > 0))
        //                {
        //                    packs = new ArrayList();
        //                    foreach (PackHdr aPack in ap.Packs.Values)
        //                    {
        //                        foreach (PackColorSize pcs in aPack.PackColors.Values)
        //                        {
        //                            if (pcs.ColorCodeRID == aOldColorCodeProfile.Key)
        //                            {
        //                                packs.Add(aPack);
        //                            }
        //                            else if (pcs.ColorCodeRID == aNewColorCodeProfile.Key)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, MIDText.GetText(eMIDTextCode.msg_DuplicateColorNotAllowed), GetType().Name);
        //                                headerUpdateFailed = true;
        //                            }
        //                        }
        //                    }
        //                    if (!headerUpdateFailed)
        //                    {
        //                        foreach (PackHdr aPack in packs)
        //                        {
        //                            try
        //                            {
        //                                HeaderData.UpdatePackColorOnHeader(aPack.PackRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //                                //ap.SetPackColorCodeRID(aPack, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key, true);
        //                            }
        //                            catch (MIDException ex)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, ex.ErrorMessage, GetType().Name);
        //                                headerUpdateFailed = true;
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                em.AddMsg(eMIDMessageLevel.Error, ex.Message, GetType().Name);
        //                                headerUpdateFailed = true;
        //                            }
        //                        }
        //                    }
        //                }

        //                if (headerUpdateFailed)
        //                {
        //                    headersUpdated = false;
        //                    message = MIDText.GetText(eMIDTextCode.msg_al_HeaderColorRenameFailed);
        //                    message = message.Replace("{0}", aOldColorCodeProfile.ColorCodeID);
        //                    message = message.Replace("{1}", ap.HeaderID);
        //                    em.AddMsg(eMIDMessageLevel.Error, message, GetType().Name);
        //                }
        //            }
        //        }

        //        return headersUpdated;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (headerEnqueue != null)
        //        {
        //            DequeueHeaders(headerEnqueue);
        //        }
        //    }
        //}

        //private void DisplayEnqueueConflict(ref EditMsgs em, HeaderEnqueue aHeaderEnqueue, AllocationProfileList apl)
        //{
        //    SecurityAdmin secAdmin = new SecurityAdmin();
        //    string errMsg = MIDText.GetText(eMIDTextCode.msg_al_HeadersInUse) + ":" + System.Environment.NewLine;

        //    foreach (HeaderConflict hdrCon in aHeaderEnqueue.HeaderConflictList)
        //    {
        //        AllocationProfile ap = (AllocationProfile)apl.FindKey(System.Convert.ToInt32(hdrCon.HeaderRID, CultureInfo.CurrentUICulture));
        //        errMsg += System.Environment.NewLine + ap.HeaderID + ", User: " + secAdmin.GetUserName(hdrCon.UserRID);
        //    }
        //    errMsg += System.Environment.NewLine + System.Environment.NewLine;

        //    em.AddMsg(eMIDMessageLevel.Severe, errMsg, GetType().Name);

        //}

        //public void DequeueHeaders(HeaderEnqueue aHeaderEnqueue)
        //{
        //    try
        //    {
        //        if (aHeaderEnqueue != null)
        //        {
        //            aHeaderEnqueue.DequeueHeaders();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        // end TT#1185 - Verify ENQ before Update 

        //Begin TT#739-MD -jsobek -Delete Stores
        //private bool UpdateBinRecords(ref EditMsgs em, ApplicationSessionTransaction aTrans,
        //    HierarchyNodeProfile aHnp, ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
        //{
        //    StoreVariableHistoryBin dlStoreVarHist;
        //    try
        //    {
        //        bool updateSuccessful = true;
        //        dlStoreVarHist = new StoreVariableHistoryBin(true, 0, aTrans.DataAccess);
        //        dlStoreVarHist.UpdateStyleColorOnDayBin(aHnp.HomeHierarchyParentRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);
        //        dlStoreVarHist.UpdateStyleColorOnWeekBin(aHnp.HomeHierarchyParentRID, aOldColorCodeProfile.Key, aNewColorCodeProfile.Key);

        //        return updateSuccessful;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        //End TT#739-MD -jsobek -Delete Stores
        // End TT#634

        // Begin TT#2 - RMatelic - Assortment Planning
        /// <summary>
        /// Used to clear the list of assortment View windows
        /// </summary>
        public void ClearOpenAsrtViewList()
        {
            try
            {
                _openAsrtViewWindows.Clear();
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to add an assortment View windows
        /// </summary>
        public void AddOpenAsrtView(int aAsrtRID, System.Windows.Forms.Form aAssortmentView)
        {
            try
            {
                if (!_openAsrtViewWindows.ContainsKey(aAsrtRID))
                {
                    _openAsrtViewWindows.Add(aAsrtRID, aAssortmentView);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to remove an assortment View windows
        /// </summary>
        public void RemoveOpenAsrtView(int aAsrtRID)
        {
            try
            {
                if (_openAsrtViewWindows.ContainsKey(aAsrtRID))
                {
                    _openAsrtViewWindows.Remove(aAsrtRID);
                }
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }

        /// <summary>
        /// Used to get an assortment View windows
        /// </summary>
        public System.Windows.Forms.Form GetOpenAsrtView(int aAsrtRID)
        {
            try
            {
                if (_openAsrtViewWindows.ContainsKey(aAsrtRID))
                {
                    return (System.Windows.Forms.Form)_openAsrtViewWindows[aAsrtRID];
                }
                return null;
            }
            catch (Exception err)
            {
                string message = err.ToString();
                throw;
            }
        }
        // End TT#2 

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return ApplicationServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD

//		public Allocation.AllocationProfile HeaderGet(int headerRID)
//		{
//			Allocation.AllocationProfile adhp = new Allocation.AllocationProfile("", -1);
//			adhp.ReadHeader(headerRID);
//			return adhp;
//		}
//		public Allocation.AllocationProfile HeaderGet(string headerID)
//		{
//			Allocation.AllocationProfile adhp = new Allocation.AllocationProfile("", -1);
//			adhp.ReadHeader(headerID);
//			return adhp;
//		}
//
//		public bool HeaderExists(string headerID)
//		{
//			return adhp.HeaderExists(headerID);
//		}
//
//		public int GetHeaderRID(string headerID)
//		{
//			return adhp.GetHeaderRID(headerID);
//		}
//
//		public Allocation.AllocationProfile HeaderUpdate(Allocation.AllocationProfile adhp)
//		{
//			try
//			{
//				switch (adhp.HeaderChangeType)
//				{
//					case eChangeType.none: 
//					{
//						break;
//					}
//					case eChangeType.add: 
//					{
//						adhp.WriteHeader();
//						break;
//					}
//					case eChangeType.update: 
//					{
//						adhp.WriteHeader();
//						break;
//					}
//					case eChangeType.delete: 
//					{
//						adhp.DeleteHeader();
//						break;
//					}
//				}
//
//				return adhp;
//			}
//			catch ( Exception err )
//			{
//				throw;
//			}
//		}
	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class ApplicationServerSession : Session
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ApplicationServerSession(ApplicationServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.applicationService, aServiceRetryCount, aServiceRetryInterval)
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
        // Begin TT#1684 - JSmith - Showing incorrect on hand data
        public bool DailyOnhandExists
        {
            get
            {
                try
                {
                    return ApplicationServerSessionRemote.DailyOnhandExists;
                }
                catch
                {
                    throw;
                }
            }
        }
        // End TT#1684

		//Begin TT#1234 - JScott - Add trigger file for release file
		public string ReleaseFileTriggerExtension
		{
			get
			{
				try
				{
					return ApplicationServerSessionRemote.ReleaseFileTriggerExtension;
				}
				catch
				{
					throw;
				}
			}
		}

		//End TT#1234 - JScott - Add trigger file for release file
		public IPlanComputationsCollection ComputationsCollection
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ApplicationServerSessionRemote.ComputationsCollection;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public IPlanComputations DefaultPlanComputations
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ApplicationServerSessionRemote.DefaultPlanComputations;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public GetMethods GetMethods
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ApplicationServerSessionRemote.GetMethods;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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

		public Assembly ComputationsAssembly
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ApplicationServerSessionRemote.ComputationsAssembly;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public void CreateComputationsDLLFile(string aCompFile, byte[] aCompDLL)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.CreateComputationsDLLFile(aCompFile, aCompDLL);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Initialize()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.Initialize();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.CleanUpGlobal();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ApplicationServerSessionRemote.CloseSession();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ApplicationServerSessionRemote.CloseAudit();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		public void PostingDateUpdate(DateTime postingDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.PostingDateUpdate(postingDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}


		public void Refresh()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.Refresh();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.RefreshHierarchy();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        //public ProfileList GetProfileList(eProfileType aProfileType)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return ApplicationServerSessionRemote.GetProfileList(aProfileType);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}
        public ProfileList GetProfileListVersion()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ApplicationServerSessionRemote.GetProfileListVersion();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        public ProfileList GetProfileList(eProfileType aProfileType)
        {
            return GetProfileList(aProfileType, false);
        }
		public ProfileList GetProfileList(eProfileType aProfileType, bool aAddGlobalUserLabel)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.GetProfileList(aProfileType, aAddGlobalUserLabel);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#3371 - JSmith - Attribute Set Order is not being honored
        public void RemoveProfileList(eProfileType aProfileType)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ApplicationServerSessionRemote.RemoveProfileList(aProfileType);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#3371 - JSmith - Attribute Set Order is not being honored

        //public ProfileList GetStoreGroupLevelProfileList(int aGroupRID)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return ApplicationServerSessionRemote.GetStoreGroupLevelProfileList(aGroupRID);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //public ProfileList GetStoreGroupLevelListViewProfileList(int aGroupRID, bool aFillStores)
        //{
        //    try
        //    {
        //        for (int i = 0; i < ServiceRetryCount; i++)
        //        {
        //            try
        //            {
        //                return ApplicationServerSessionRemote.GetStoreGroupLevelListViewProfileList(aGroupRID, aFillStores);
        //            }
        //            catch (Exception exc)
        //            {
        //                if (isServiceRetryException(exc))
        //                {
        //                    Thread.Sleep(ServiceRetryInterval);
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

		public BaseProfileXRef GetProfileXRef(BaseProfileXRef aProfXRef)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.GetProfileXRef(aProfXRef);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public ApplicationSessionTransaction CreateTransaction()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.CreateTransaction();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
        public ApplicationSessionTransaction CreateTransaction(Session aSession)
        {
            for (int i = 0; i < ServiceRetryCount; i++)
            {
                try
                {
                    return ApplicationServerSessionRemote.CreateTransaction(aSession);
                }
                catch (Exception exc)
                {
                    if (isServiceRetryException(exc))
                    {
                        Thread.Sleep(ServiceRetryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        }
        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

		public void RefreshCalendar(DateTime refreshDate)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ApplicationServerSessionRemote.RefreshCalendar(refreshDate);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public eStoreStatus GetStoreStatus(int storeRID, int yearWeek)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.GetStoreStatus(storeRID, yearWeek);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1779 - JSmith - Invalid Calendar Date with Cancel Alloc in Style Reivew and Shipping Horizon=0
        public ArrayList DetermineInStoreOnHandDay(ArrayList beginDates, int[] storeList)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ApplicationServerSessionRemote.DetermineInStoreOnHandDay(beginDates, storeList);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1779

		public string[] GetComputationModes()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.GetComputationModes();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetDefaultComputations()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.GetDefaultComputations();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // begin TT#1185 - JEllis - Verify ENQ before Update (part 2)
        public bool EnqueueHeaders(int aThreadID, long aTransactionID, int aUserRID, List<int> aHdrRIDList, out string aHdrConflictMsg)
        {
            for (int i = 0; i < ServiceRetryCount; i++)
            {
                try
                {
                    return ApplicationServerSessionRemote.EnqueueHeaders(aThreadID, aTransactionID, aUserRID, aHdrRIDList, out aHdrConflictMsg);
                }
                catch (Exception exc)
                {
                    if (isServiceRetryException(exc))
                    {
                        Thread.Sleep(ServiceRetryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        }
        public void DequeueHeaders(int aThreadID, long aTransactionID, int aUserRID)
        {
            for (int i = 0; i < ServiceRetryCount; i++)
            {
                try
                {
                    ApplicationServerSessionRemote.DequeueHeaders(aThreadID, aTransactionID, aUserRID);
                    return;
                }
                catch (Exception exc)
                {
                    if (isServiceRetryException(exc))
                    {
                        Thread.Sleep(ServiceRetryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        }
        public HeaderEnqueue GetHeaderEnqueueObject(int aThreadID, long aTransactionID, int aUserRID)
        {
            for (int i = 0; i < ServiceRetryCount; i++)
            {
                try
                {
                    return ApplicationServerSessionRemote.GetHeaderEnqueueObject(aThreadID, aTransactionID, aUserRID);
                }
                catch (Exception exc)
                {
                    if (isServiceRetryException(exc))
                    {
                        Thread.Sleep(ServiceRetryInterval);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
        }
        // end TT#1185 - JEllis - Verify ENQ before Update (part 2)

		public bool RenameColor(ref EditMsgs em, bool aUpdateHeaders, HierarchyNodeProfile aHnp,
			ColorCodeProfile aOldColorCodeProfile, ColorCodeProfile aNewColorCodeProfile)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ApplicationServerSessionRemote.RenameColor(ref em, aUpdateHeaders, aHnp,
							aOldColorCodeProfile, aNewColorCodeProfile);
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2 - RMatelic - Assortment Planning
        public void ClearOpenAsrtViewList()
        {
            try
            {
                ApplicationServerSessionRemote.ClearOpenAsrtViewList();
            }
            catch 
            {
                throw;
            }
        }

        public void AddOpenAsrtView(int aAsrtRID, System.Windows.Forms.Form aAssortmentView)
        {
            try
            {
                ApplicationServerSessionRemote.AddOpenAsrtView(aAsrtRID, aAssortmentView);
            }
            catch 
            {
                throw;
            }
        }

        public void RemoveOpenAsrtView(int aAsrtRID)
        {
            try
            {
                ApplicationServerSessionRemote.RemoveOpenAsrtView(aAsrtRID);
            }
            catch 
            {
                throw;
            }
        }

        public System.Windows.Forms.Form GetOpenAsrtView(int aAsrtRID)
        {
            try
            {
                 return ApplicationServerSessionRemote.GetOpenAsrtView(aAsrtRID);
            }
            catch 
            {
                throw;
            }
        }
        // End TT#2 

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ApplicationServerSessionRemote.GetServiceProfile();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ApplicationServerSessionRemote.VerifyEnvironment(aClientProfile);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
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
