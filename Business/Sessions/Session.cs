using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.Remoting.Lifetime;
//Begin TT#708 - JScott - Services need a Retry availalbe.
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Threading;
//End TT#708 - JScott - Services need a Retry availalbe.

using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.Business
{
	/// <summary>
	/// Session is an MBR base class that allows the inheritor to present functionality to the other remote pieces of the system.
	/// The aLocal switch on the constructor indicates whether this class is being instantiated in the Client or in a remote service.
	/// If this class is being instantiated on the Client, the lease lifetime is set to "permanent".  Otherwise, the lease is initialized
	/// and must be sponsored for it to remain alive.
	/// </summary>

	[Serializable]
	//Begin TT#708 - JScott - Services need a Retry availalbe.
	//abstract public class Session : MarshalByRefObject, ISponsor
	abstract public class SessionRemote : MarshalByRefObject, ISponsor
	//End TT#708 - JScott - Services need a Retry availalbe.
	{
		//=======
		// FIELDS
		//=======

		private bool _local;
		private SessionAddressBlock _SAB;
		private System.Threading.Thread _cleanupThread = null;
		private Audit _audit;
		private MRSCalendar _calendar;
		private GlobalOptionsProfile _gop = null;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates an instance of the SessionRemote class.
		/// </summary>
		/// <param name="aLocal">Identifies if the SessionRemote is local to the process.</param>
		/// <remarks>
		/// Starts a thread to check the current state of the lease.
		/// </remarks>

		//Begin TT#708 - JScott - Services need a Retry availalbe.
		//public Session(bool aLocal)
		public SessionRemote(bool aLocal)
		//End TT#708 - JScott - Services need a Retry availalbe.
		{
			try
			{
				_local = aLocal;

				if (!_local)
				{
					_cleanupThread = new System.Threading.Thread(new System.Threading.ThreadStart(CheckCurrentState));
					_cleanupThread.Start();
				}
			}
			catch
			{
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the SessionAddressBlock.
		/// </summary>

		public SessionAddressBlock SessionAddressBlock
		{
			get
			{
				return _SAB;
			}
		}

        //Begin TT#901-MD -jsobek -Batch Only Mode
        public bool IsLocal
        {
            get
            {
                return _local;
            }
        }
        //End TT#901-MD -jsobek -Batch Only Mode

		/// <summary>
		/// Gets the Audit.
		/// </summary>

		public Audit Audit
		{
			get
			{
				return _audit;
			}
		}
        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        public string GetEnvironmentBusinessInfo()
        {
            return MIDRetail.Business.EnvironmentBusinessInfo.GetAllBusinessEnvironmentInfo(this.SessionAddressBlock, Environment.NewLine);
        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application

		/// <summary>
		/// Gets or sets the Calendar for the SessionRemote.
		/// </summary>
		/// 
		public MRSCalendar Calendar
		{
			get
			{
				return _calendar;
			}
			set
			{
				_calendar = value;
			}
		}

		public GlobalOptionsProfile GlobalOptions
		{
			get 
			{ 
				if (_gop == null)
				{
					_gop = new GlobalOptionsProfile(-1);
					_gop.LoadOptions();
				}
				return _gop;  
			}
		}


		//========
		// METHODS
		//========

		/// <summary>
		/// Passes the Session Address Block to the session.
		/// </summary>
		/// <param name="aSAB">The SessionAddressBlock.</param>

		public void SetSAB(SessionAddressBlock aSAB)
		{
			_SAB = aSAB;
		}

		/// <summary>
		/// Returns the database name where the SessionRemote is connected.
		/// </summary>
		
		public string GetDatabaseName()
		{
			SystemData systemData;
			
			try
			{
				systemData = new SystemData();
				return systemData.GetDatabaseName();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the machine name where the SessionRemote is located.
		/// </summary>
		
		public string GetMachineName()
		{
			try
			{
				// Get machine name
				return System.Net.Dns.GetHostName();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the machine name where the SessionRemote is located.
		/// </summary>
		
		public string GetIPAddress()
		{
			try
			{
				// Get machine name
				string machineName = System.Net.Dns.GetHostName();
				// Then using host name, get the IP address list..
				System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(machineName);
				System.Net.IPAddress [] addr = ipEntry.AddressList;
				return addr[0].ToString ();
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Returns the machine name where the SessionRemote is located.
		/// </summary>
		
		public eMIDMessageLevel GetHighestMessageLevel()
		{
			try
			{
				if (Audit != null)
				{
					return Audit.HighestMessageLevel;
				}
				else
				{
					return eMIDMessageLevel.None;
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#698 - JSmith - Enhance environment information
        /// <summary>
        /// Returns the environment for the service from the configuration file.
        /// </summary>

        public string GetMIDEnvironment()
        {
            string MIDEnvironment = "Unknown";

            try
            {
                string strEnvironment = MIDConfigurationManager.AppSettings["MIDEnvironment"];
                if (strEnvironment != null)
                {
                    MIDEnvironment = strEnvironment;
                }
                return MIDEnvironment;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#698

		/// <summary>
		/// Initializes the SessionRemote.
		/// </summary>

		public virtual void Initialize()
		{
			// Begin 4320 - stodd 2.15.2007
			try
			{
			}
			catch
			{
				throw;
			}
			// Begin 4320 - stodd 2.15.2007
		}

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public virtual void CloseSession()
        {
            try
            {
                if (_audit != null)
                {
                    _audit.CloseUpdateConnection();
                }
                // Begin TT#1440 - JSmith - Memory Issues
                CleanUpSession();
                System.GC.Collect();
                // End TT#1243
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Close audit
        /// </summary>
        public virtual void CloseAudit()
        {
            try
            {
                if (_audit != null)
                {
                    _audit.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

        // Begin TT#1440 - JSmith - Memory Issues
        public virtual void CleanUpSession()
        {
           _calendar = null;
            _gop = null;
        }
        // End TT#1243

		/// <summary>
		/// Refreshes the SessionRemote base.
		/// </summary>

		public virtual void RefreshBase()
		{
			try
			{
				_gop = null;
			}
			catch
			{
				throw;
			}
		}

		public void RefreshGlobalOptions()
		{
			try
			{
				_gop = null;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates the Audit instance for this SessionRemote
		/// </summary>
		/// <remarks>
		/// This is used to create a new audit report.
		/// </remarks>

		protected void CreateAudit(eProcesses aProcess)
		{
			try
			{
                _audit = new Audit(aProcess, SessionAddressBlock.ClientServerSession.UserRID
                    , this.GetEnvironmentBusinessInfo //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    );
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#46 MD - JSmith - User Dashboard 
        /// <summary>
        /// Creates the Audit instance for this SessionRemote
        /// </summary>
        /// <remarks>
        /// This is used to create a new audit report.
        /// </remarks>

        protected void CreateAudit(eProcesses aProcess, bool aEnableActivityLog)
        {
            try
            {
                _audit = new Audit(aProcess, SessionAddressBlock.ClientServerSession.UserRID, aEnableActivityLog
                    , this.GetEnvironmentBusinessInfo //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    );
            }
            catch
            {
                throw;
            }
        }
        
		/// <summary>
		/// Creates the Audit instance for this SessionRemote
		/// </summary>
		/// <remarks>
		/// This is used to continue an existing audit report from within the same execution.
		/// </remarks>

        protected void CreateAudit(bool aEnableActivityLog)
		{
			try
			{
                _audit = new Audit(SessionAddressBlock.ClientServerSession.externGetAuditHeader(), SessionAddressBlock.ClientServerSession.UserRID, aEnableActivityLog
                    , this.GetEnvironmentBusinessInfo //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    );
			}
			catch
			{
				throw;
			}
		}
        // End TT#46 MD

        /// <summary>
        /// Creates the Audit instance for this SessionRemote
        /// </summary>
        /// <remarks>
        /// This is used to continue an existing audit report from within the same execution.
        /// </remarks>

		protected void CreateAudit()
		{
			try
			{
                _audit = new Audit(SessionAddressBlock.ClientServerSession.externGetAuditHeader(), SessionAddressBlock.ClientServerSession.UserRID
                    , this.GetEnvironmentBusinessInfo //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Creates the Audit instance for this SessionRemote
		/// </summary>
		/// <remarks>
		/// This is used to continue an existing audit report from an external source.
		/// </remarks>

		protected void CreateAudit(int aProcessRID)
		{
			try
			{
                _audit = new Audit(aProcessRID, SessionAddressBlock.ClientServerSession.UserRID
                    , this.GetEnvironmentBusinessInfo //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    );
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Gets the ID for the audit.
		/// </summary>
		/// <returns></returns>

		public int externGetAuditHeader()
		{
			try
			{
				return Audit.ProcessRID;
			}
			catch
			{
				throw;
			}
		}
        // Begin TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
        public void SetAuditLoggingLevel(eMIDMessageLevel aLoggingLevel)
        {
            try
            {
                if (_audit != null)
                {
                    _audit.LoggingLevel = aLoggingLevel;
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect

		public string GetProductVersion()
		{
			try
			{
				return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Checks the current state of the lease.  Calls cleanup when lease has expired.
		/// </summary>
		/// <remarks>
		/// This protects against most sudden and unexpected termination of processes using the SessionRemote.
		/// </remarks>

		private void CheckCurrentState()
		{
			ILease lease;

			try
			{
				while (true)
				{
					lease = (ILease)base.GetLifetimeService();

					if (lease != null && lease.CurrentState == LeaseState.Expired)  
					{
						ExpiredCleanup();
						// Begin MID Track #5270 - JSmith - Thread abort error
//						_cleanupThread.Abort();
						// End MID Track #5270
						break;
					}

					System.Threading.Thread.Sleep(30000);
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Virtual method to be overridden by each SessionRemote that requires resources to be cleaned up when the
		/// SessionRemote ends.
		/// </summary>

		protected virtual void ExpiredCleanup()
		{
            // Begin TT#1243 - JSmith - Audit Performance
            try
            {
                if (_audit != null)
                {
                    _audit.CloseUpdateConnection();
                }
            }
            catch
            {
                throw;
            }
            // End TT#1243
		}

		/// <summary>
		/// Requests a sponsoring client to renew the lease for the specified object.
		/// </summary>
		/// <param name="aLease">
		/// The lifetime lease of the object that requires lease renewal. 
		/// </param>
		/// <returns>
		/// The additional lease time for the specified object.
		/// </returns>

		public TimeSpan Renewal(ILease aLease)
		{
			try
			{
				return TimeSpan.FromMinutes(1);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Initializes lifetime services for remoting.
		/// </summary>
		/// <returns></returns>

		override public object InitializeLifetimeService()
		{
			ILease lease;

			try
			{
				if (_local)
				{
					return null;
				}
				else
				{
					lease = (ILease)base.InitializeLifetimeService();

					if (lease.CurrentState == LeaseState.Initial)  
					{
						lease.InitialLeaseTime = TimeSpan.FromMinutes(1);
						lease.SponsorshipTimeout = TimeSpan.FromMinutes(1);
						lease.RenewOnCallTime = TimeSpan.FromSeconds(60);
					}

					return lease;
				}
			}
			catch
			{
				throw;
			}
		}
	}

//	/// <summary>
//	/// MBRSessionObject is a base class that causes the inheritor to be a Session-sponsored MarshalByRefObject.  Classes that will be
//	/// passed between sessions will inherit from this
//	/// </summary>
//
//	abstract public class MBRSessionObject : MarshalByRefObject, IDisposable
//	{
//		//=======
//		// FIELDS
//		//=======
//
//		private ISponsor _sponsor;
//		private int _leaseTime;
//		private bool _leaseInitialized;
//
//		//=============
//		// CONSTRUCTORS
//		//=============
//
//		/// <summary>
//		/// Creates a new instance of MBRSessionObject using the given ISponsor as the sponsoring object.
//		/// </summary>
//		/// <param name="aSponsor">
//		/// The ISponsor object that will become the sponsor for the inheriting object.
//		/// </param>
//
//		public MBRSessionObject(ISponsor aSponsor)
//		{
//			_sponsor = aSponsor;
//			_leaseTime = 60;
//			_leaseInitialized = false;
//		}
//
//		/// <summary>
//		/// Creates a new instance of MBRSessionObject using the given ISponsor as the sponsoring object.
//		/// </summary>
//		/// <param name="aSponsor">
//		/// The ISponsor object that will become the sponsor for the inheriting object.
//		/// </param>
//
//		public MBRSessionObject(ISponsor aSponsor, int aLeaseTimeInSeconds)
//		{
//			_sponsor = aSponsor;
//			_leaseTime = aLeaseTimeInSeconds;
//			_leaseInitialized = false;
//		}
//
//		#region IDisposable Members
//
//		public void Dispose()
//		{
//			Dispose(true);
//			System.GC.SuppressFinalize(this);
//		}
//
//		abstract protected void Dispose(bool disposing);
//
//		~MBRSessionObject()
//		{
//			Dispose(false);
//		}
//
//		#endregion
//
//		//===========
//		// PROPERTIES
//		//===========
//
//		//========
//		// METHODS
//		//========
//
//		/// <summary>
//		/// Initializes lifetime services for remoting.  This method also Registers the home Session as the sponsor for this object.
//		/// </summary>
//		/// <returns>
//		/// The lease for this object.
//		/// </returns>
//
//		override public object InitializeLifetimeService()
//		{
//			ILease lease;
//
//			try
//			{
//				lease = (ILease)base.InitializeLifetimeService();
//
//				if (lease.CurrentState == LeaseState.Initial)  
//				{
//					lease.InitialLeaseTime = TimeSpan.FromSeconds(_leaseTime);
//					lease.SponsorshipTimeout = TimeSpan.FromMinutes(1);
//					lease.RenewOnCallTime = TimeSpan.FromSeconds(_leaseTime);
//					lease.Register(_sponsor);
//				}
//
//				_leaseInitialized = true;
//
//				return lease;
//			}
//			catch
//			{
//				throw;
//			}
//		}
//
//		virtual public void UnregisterLease()
//		{
//			ILease lease;
//
//			try
//			{
//				if (_leaseInitialized)
//				{
//					lease = (ILease)base.InitializeLifetimeService();
//					lease.Unregister(_sponsor);
//				}
//			}
//			catch
//			{
//				throw;
//			}
//		}
//	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	abstract public class Session
	{
		//=======
		// FIELDS
		//=======

		private SessionRemote _SessionRemote;
		private eProcesses _sessionType;
		private int _serviceRetryCount;
		private int _serviceRetryInterval;

		//=============
		// CONSTRUCTORS
		//=============

		public Session(SessionRemote aSessionRemote, eProcesses aSessionType, int aServiceRetryCount, int aServiceRetryInterval)
		{
			try
			{
				_SessionRemote = aSessionRemote;
				_sessionType = aSessionType;
				_serviceRetryCount = aServiceRetryCount;
				_serviceRetryInterval = aServiceRetryInterval;
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

		public SessionRemote SessionRemote
		{
			get
			{
				return _SessionRemote;
			}
		}

		public ControlServerSessionRemote ControlServerSessionRemote
		{
			get
			{
				return (ControlServerSessionRemote)_SessionRemote;
			}
		}

		public ClientServerSessionRemote ClientServerSessionRemote
		{
			get
			{
				return (ClientServerSessionRemote)_SessionRemote;
			}
		}

		public StoreServerSessionRemote StoreServerSessionRemote
		{
			get
			{
				return (StoreServerSessionRemote)_SessionRemote;
			}
		}

		public HierarchyServerSessionRemote HierarchyServerSessionRemote
		{
			get
			{
				return (HierarchyServerSessionRemote)_SessionRemote;
			}
		}

		public ApplicationServerSessionRemote ApplicationServerSessionRemote
		{
			get
			{
				return (ApplicationServerSessionRemote)_SessionRemote;
			}
		}

		public SchedulerServerSessionRemote SchedulerServerSessionRemote
		{
			get
			{
				return (SchedulerServerSessionRemote)_SessionRemote;
			}
		}

		public HeaderServerSessionRemote HeaderServerSessionRemote
		{
			get
			{
				return (HeaderServerSessionRemote)_SessionRemote;
			}
		}

		public eProcesses SessionType
		{
			get
			{
				return _sessionType;
			}
		}

		public int ServiceRetryCount
		{
			get
			{
				return _serviceRetryCount;
			}
		}

		public int ServiceRetryInterval
		{
			get
			{
				return _serviceRetryInterval;
			}
		}

		public SessionAddressBlock SessionAddressBlock
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return _SessionRemote.SessionAddressBlock;
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

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public Audit Audit
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return _SessionRemote.Audit;
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

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
				}
				catch
				{
					throw;
				}
			}
		}
        //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        public string GetEnvironmentBusinessInfo()
        {
            return MIDRetail.Business.EnvironmentBusinessInfo.GetAllBusinessEnvironmentInfo(this.SessionAddressBlock, Environment.NewLine);
        }
        //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
        
		public MRSCalendar Calendar
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return _SessionRemote.Calendar;
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

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
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
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							_SessionRemote.Calendar = value;
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

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public GlobalOptionsProfile GlobalOptions
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return _SessionRemote.GlobalOptions;
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

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public bool isSessionRunningLocal
		{
			get
			{
				try
				{
					return !RemotingServices.IsTransparentProxy(_SessionRemote);
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

		protected bool isServiceRetryException(Exception aExc)
		{
			if (aExc.GetType() == typeof(RemotingException) ||
				aExc.GetType() == typeof(SocketException))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool isSessionRunningRemote()
		{
			try
			{
				return RemotingServices.IsTransparentProxy(SessionRemote);
			}
			catch
			{
				throw;
			}
		}

		public void SetSAB(SessionAddressBlock aSAB)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						_SessionRemote.SetSAB(aSAB);
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetDatabaseName()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetDatabaseName();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetMachineName()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetMachineName();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetIPAddress()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetIPAddress();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public eMIDMessageLevel GetHighestMessageLevel()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetHighestMessageLevel();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetMIDEnvironment()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetMIDEnvironment();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public void RefreshGlobalOptions()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						_SessionRemote.RefreshGlobalOptions();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public int externGetAuditHeader()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.externGetAuditHeader();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
        public void SetAuditLoggingLevel(eMIDMessageLevel aLoggingLevel)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        _SessionRemote.SetAuditLoggingLevel(aLoggingLevel);
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
                throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#605-MD - JSmith - Changing Audit Logging Level requires restarting the client to take effect
		public string GetProductVersion()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.GetProductVersion();
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}

		public TimeSpan Renewal(ILease aLease)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return _SessionRemote.Renewal(aLease);
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

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)_sessionType));
			}
			catch
			{
				throw;
			}
		}
	}
	//End TT#708 - JScott - Services need a Retry availalbe.
}
